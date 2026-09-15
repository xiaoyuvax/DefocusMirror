using System.Diagnostics;
using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;

namespace DefocusMirror;

public class ScreenCapture : IDisposable
{
    private const int DefaultFrameRate = 30;
    private const int DefaultViewportWidth = 1920;
    private const int DefaultViewportHeight = 1080;
    private const int AcquireFrameTimeoutMs = 1000;
    private const int MaxRecoveryAttempts = 3;

    private Adapter _adapter;
    private Texture2D _backBuffer;
    private Buffer _blurParamsBuffer;
    private BlurParams _blurParams;
    private Texture2D _depthStencilTexture;
    private DepthStencilState _depthStencilState;
    private DepthStencilView _depthStencilView;
    private Device _device;
    private bool _disposed;
    private Buffer _indexBuffer;
    private InputLayout _inputLayout;
    private bool _initialized;
    private OutputDuplication _outputDuplication;
    private PixelShader _pixelShader;
    private RenderTargetView _renderTargetView;
    private SamplerState _samplerState;
    private int _screenHeight;
    private ShaderResourceView _screenShaderResourceView;
    private Texture2D _screenTexture;
    private int _screenWidth;
    private SwapChain _swapChain;
    private Buffer _vertexBuffer;
    private VertexShader _vertexShader;

    private readonly int _sourceAdapterIndex;
    private readonly int _sourceOutputIndex;
    private readonly int _frameRate;
    private readonly int _targetViewportWidth;
    private readonly int _targetViewportHeight;

    public ScreenCapture(
        float blurScaleR = 0, float blurScaleG = 0, float blurScaleB = 0,
        float blurSigmaR = 1, float blurSigmaG = 1, float blurSigmaB = 1,
        nint? hwnd = null,
        int? srcAdapterIndex = null, int? sourceOutputIndex = null,
        int? frameRate = null,
        int? targetViewportWidth = null, int? targetViewportHeight = null)
    {
        _blurParams = new BlurParams(
            DefaultViewportWidth, DefaultViewportHeight,
            blurScaleR, blurScaleG, blurScaleB,
            blurSigmaR, blurSigmaG, blurSigmaB);
        _sourceAdapterIndex = srcAdapterIndex ?? 0;
        _sourceOutputIndex = sourceOutputIndex ?? 0;
        _frameRate = frameRate ?? DefaultFrameRate;
        _targetViewportWidth = targetViewportWidth ?? DefaultViewportWidth;
        _targetViewportHeight = targetViewportHeight ?? DefaultViewportHeight;

        if (hwnd != null)
            Initialize(hwnd.Value);
    }

    ~ScreenCapture()
    {
        Dispose(false);
    }

    public void Initialize(nint hwnd)
    {
        DisposeNativeResources();

        using var dxgiFactory = new Factory1();

        ValidateAdapterAndOutput(dxgiFactory);

        _adapter = dxgiFactory.Adapters[_sourceAdapterIndex];
        _device = new Device(_adapter, DeviceCreationFlags.None, FeatureLevel.Level_11_0)
            .QueryInterface<SharpDX.Direct3D11.Device1>();

        CreateOutputDuplication();
        CreateSwapChain(hwnd);
        CreateDepthStencil();
        CreateShaders();
        CreateScreenTexture();
        CreateSamplerState();
        CreateConstantBuffer();
        CreateVertexBuffers();

        _initialized = true;
    }

    public void CaptureScreen()
    {
        if (!_initialized || _disposed) return;

        SharpDX.DXGI.Resource desktopResource = null;
        try
        {
            var result = _outputDuplication.TryAcquireNextFrame(
                AcquireFrameTimeoutMs, out var frameInfo, out desktopResource);

            if (result.Success)
            {
                using var texture = desktopResource.QueryInterface<Texture2D>();
                _device.ImmediateContext.CopyResource(texture, _screenTexture);
                _outputDuplication.ReleaseFrame();
            }
        }
        catch (SharpDXException ex)
        {
            Debug.WriteLine($"CaptureScreen failed: {ex.Message}");
            TryRecover();
        }
        finally
        {
            desktopResource?.Dispose();
        }
    }

    public void Render()
    {
        if (!_initialized || _disposed) return;

        var context = _device.ImmediateContext;
        context.OutputMerger.SetTargets(_depthStencilView, _renderTargetView);
        context.Rasterizer.SetViewports([new Viewport(0, 0, _targetViewportWidth, _targetViewportHeight, 0.0f, 1.0f)]);

        context.ClearDepthStencilView(_depthStencilView, DepthStencilClearFlags.Depth, 0.5f, 0);
        context.ClearRenderTargetView(_renderTargetView, new Color4(0, 0, 0, 1));

        context.OutputMerger.SetDepthStencilState(_depthStencilState, 0);
        context.VertexShader.Set(_vertexShader);
        context.PixelShader.Set(_pixelShader);
        context.InputAssembler.InputLayout = _inputLayout;
        context.InputAssembler.SetVertexBuffers(0,
            new VertexBufferBinding(_vertexBuffer, Utilities.SizeOf<Vertex>(), 0));
        context.InputAssembler.SetIndexBuffer(_indexBuffer, Format.R32_UInt, 0);
        context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
        context.PixelShader.SetShaderResource(0, _screenShaderResourceView);
        context.PixelShader.SetSampler(0, _samplerState);

        context.DrawIndexed(6, 0, 0);
        _swapChain.Present(0, PresentFlags.None);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;

        if (disposing)
        {
            _renderTargetView?.Dispose();
            _depthStencilView?.Dispose();
            _depthStencilTexture?.Dispose();
            _depthStencilState?.Dispose();
            _vertexBuffer?.Dispose();
            _indexBuffer?.Dispose();
            _vertexShader?.Dispose();
            _pixelShader?.Dispose();
            _inputLayout?.Dispose();
            _screenShaderResourceView?.Dispose();
            _samplerState?.Dispose();
            _screenTexture?.Dispose();
            _backBuffer?.Dispose();
            _blurParamsBuffer?.Dispose();
            _swapChain?.Dispose();
            _device?.Dispose();
            _outputDuplication?.Dispose();
        }

        _disposed = true;
    }

    private void ValidateAdapterAndOutput(Factory1 dxgiFactory)
    {
        if (_sourceAdapterIndex >= dxgiFactory.Adapters.Length)
            throw new ArgumentOutOfRangeException(nameof(_sourceAdapterIndex),
                $"Adapter index {_sourceAdapterIndex} is out of range. Available: {dxgiFactory.Adapters.Length}");

        using var adapter = dxgiFactory.Adapters[_sourceAdapterIndex];
        if (_sourceOutputIndex >= adapter.Outputs.Length)
            throw new ArgumentOutOfRangeException(nameof(_sourceOutputIndex),
                $"Output index {_sourceOutputIndex} is out of range. Available: {adapter.Outputs.Length}");
    }

    private void CreateOutputDuplication()
    {
        using var output = _adapter.Outputs[_sourceOutputIndex];
        using var output1 = output.QueryInterface<Output1>();
        var bounds = output1.Description.DesktopBounds;
        _screenWidth = bounds.Right - bounds.Left;
        _screenHeight = bounds.Bottom - bounds.Top;
        _outputDuplication = output1.DuplicateOutput(_device);
    }

    private void CreateSwapChain(nint hwnd)
    {
        using var dxgiFactory = new Factory1();
        _swapChain = new SwapChain(dxgiFactory, _device, new SwapChainDescription
        {
            ModeDescription = new ModeDescription(
                _targetViewportWidth, _targetViewportHeight,
                new Rational(_frameRate, 1),
                Format.B8G8R8A8_UNorm),
            BufferCount = 1,
            IsWindowed = true,
            OutputHandle = hwnd,
            SampleDescription = new SampleDescription(1, 0),
            SwapEffect = SwapEffect.Discard,
            Usage = Usage.RenderTargetOutput
        });

        _backBuffer = _swapChain.GetBackBuffer<Texture2D>(0);
        _renderTargetView = new RenderTargetView(_device, _backBuffer);
    }

    private void CreateDepthStencil()
    {
        _depthStencilTexture = new Texture2D(_device, new Texture2DDescription
        {
            Width = _targetViewportWidth,
            Height = _targetViewportHeight,
            MipLevels = 1,
            ArraySize = 1,
            Format = Format.D24_UNorm_S8_UInt,
            SampleDescription = new SampleDescription(1, 0),
            Usage = ResourceUsage.Default,
            BindFlags = BindFlags.DepthStencil,
            CpuAccessFlags = CpuAccessFlags.None,
            OptionFlags = ResourceOptionFlags.None
        });

        _depthStencilView = new DepthStencilView(_device, _depthStencilTexture);
        _depthStencilState = new DepthStencilState(_device, new DepthStencilStateDescription
        {
            IsDepthEnabled = false,
            DepthWriteMask = DepthWriteMask.Zero,
            DepthComparison = Comparison.Always
        });
    }

    private void CreateShaders()
    {
        var vertexShaderByteCode = ShaderBytecode.Compile(VertexShaderSource, "VS", "vs_5_0");
        _vertexShader = new VertexShader(_device, vertexShaderByteCode);
        _inputLayout = new InputLayout(_device, vertexShaderByteCode,
        [
            new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
            new InputElement("TEXCOORD", 0, Format.R32G32_Float, 16, 0)
        ]);

        var hlsl = ResourceHelper.GetBinRes("DefocusMirror.PixelShader2.hlsl");
        var pixelShaderByteCode = ShaderBytecode.Compile(hlsl, "main", "ps_5_0");
        _pixelShader = new PixelShader(_device, pixelShaderByteCode);
    }

    private void CreateScreenTexture()
    {
        _screenTexture = new Texture2D(_device, new Texture2DDescription
        {
            Width = _screenWidth,
            Height = _screenHeight,
            MipLevels = 1,
            ArraySize = 1,
            Format = Format.B8G8R8A8_UNorm,
            SampleDescription = new SampleDescription(1, 0),
            Usage = ResourceUsage.Default,
            BindFlags = BindFlags.ShaderResource,
            CpuAccessFlags = CpuAccessFlags.None,
            OptionFlags = ResourceOptionFlags.None
        });
        _screenShaderResourceView = new ShaderResourceView(_device, _screenTexture);
    }

    private void CreateSamplerState()
    {
        _samplerState = new SamplerState(_device, new SamplerStateDescription
        {
            Filter = Filter.MinMagMipLinear,
            AddressU = TextureAddressMode.Clamp,
            AddressV = TextureAddressMode.Clamp,
            AddressW = TextureAddressMode.Clamp,
            ComparisonFunction = Comparison.Never,
            MaximumAnisotropy = 16,
            MipLodBias = 0.0f,
            MinimumLod = 0.0f,
            MaximumLod = float.MaxValue
        });
    }

    private void CreateConstantBuffer()
    {
        _blurParamsBuffer = new Buffer(_device, new BufferDescription
        {
            SizeInBytes = Utilities.SizeOf<BlurParams>(),
            Usage = ResourceUsage.Default,
            BindFlags = BindFlags.ConstantBuffer,
            CpuAccessFlags = CpuAccessFlags.None,
            OptionFlags = ResourceOptionFlags.None,
            StructureByteStride = 0
        });

        _blurParams = new BlurParams(_screenWidth, _screenHeight,
            _blurParams.BlurScaleR, _blurParams.BlurScaleG, _blurParams.BlurScaleB,
            _blurParams.BlurSigmaR, _blurParams.BlurSigmaG, _blurParams.BlurSigmaB);
        _device.ImmediateContext.UpdateSubresource(ref _blurParams, _blurParamsBuffer);
        _device.ImmediateContext.PixelShader.SetConstantBuffer(0, _blurParamsBuffer);
    }

    private void CreateVertexBuffers()
    {
        var vertices = new[]
        {
            new Vertex(new Vector4(-1.0f, 1.0f, 0.0f, 1.0f), new Vector2(0.0f, 0.0f)),
            new Vertex(new Vector4(1.0f, 1.0f, 0.0f, 1.0f), new Vector2(1.0f, 0.0f)),
            new Vertex(new Vector4(1.0f, -1.0f, 0.0f, 1.0f), new Vector2(1.0f, 1.0f)),
            new Vertex(new Vector4(-1.0f, -1.0f, 0.0f, 1.0f), new Vector2(0.0f, 1.0f))
        };

        _vertexBuffer = Buffer.Create(_device, vertices, new BufferDescription
        {
            Usage = ResourceUsage.Default,
            BindFlags = BindFlags.VertexBuffer,
            SizeInBytes = Utilities.SizeOf<Vertex>() * vertices.Length,
            CpuAccessFlags = CpuAccessFlags.None
        });

        _indexBuffer = Buffer.Create(_device, [0, 1, 2, 0, 2, 3], new BufferDescription
        {
            Usage = ResourceUsage.Default,
            BindFlags = BindFlags.IndexBuffer,
            SizeInBytes = Utilities.SizeOf<int>() * 6,
            CpuAccessFlags = CpuAccessFlags.None
        });
    }

    private void TryRecover()
    {
        for (int attempt = 0; attempt < MaxRecoveryAttempts; attempt++)
        {
            try
            {
                Debug.WriteLine($"Attempting recovery, try {attempt + 1}/{MaxRecoveryAttempts}");

                _outputDuplication?.Dispose();
                _screenShaderResourceView?.Dispose();
                _screenTexture?.Dispose();

                using var output = _adapter.Outputs[_sourceOutputIndex];
                using var output1 = output.QueryInterface<Output1>();
                _outputDuplication = output1.DuplicateOutput(_device);

                _screenTexture = new Texture2D(_device, new Texture2DDescription
                {
                    Width = _screenWidth,
                    Height = _screenHeight,
                    MipLevels = 1,
                    ArraySize = 1,
                    Format = Format.B8G8R8A8_UNorm,
                    SampleDescription = new SampleDescription(1, 0),
                    Usage = ResourceUsage.Default,
                    BindFlags = BindFlags.ShaderResource,
                    CpuAccessFlags = CpuAccessFlags.None,
                    OptionFlags = ResourceOptionFlags.None
                });
                _screenShaderResourceView = new ShaderResourceView(_device, _screenTexture);

                Debug.WriteLine("Recovery succeeded");
                return;
            }
            catch (SharpDXException ex)
            {
                Debug.WriteLine($"Recovery attempt {attempt + 1} failed: {ex.Message}");
            }
        }

        Debug.WriteLine("All recovery attempts failed");
    }

    private void DisposeNativeResources()
    {
        if (!_initialized) return;

        _outputDuplication?.Dispose();
        _screenShaderResourceView?.Dispose();
        _screenTexture?.Dispose();
        _renderTargetView?.Dispose();
        _depthStencilView?.Dispose();
        _depthStencilTexture?.Dispose();
        _depthStencilState?.Dispose();
        _backBuffer?.Dispose();
        _swapChain?.Dispose();
        _device?.Dispose();
        _adapter?.Dispose();
    }

    private const string VertexShaderSource = @"
        struct VertexInput
        {
            float4 Position : POSITION;
            float2 TexCoord : TEXCOORD0;
        };

        struct VertexOutput
        {
            float4 Position : SV_POSITION;
            float2 TexCoord : TEXCOORD0;
        };

        VertexOutput VS(float4 Position : POSITION, float2 TexCoord : TEXCOORD0)
        {
            VertexOutput output;
            output.Position = Position;
            output.TexCoord = TexCoord;
            return output;
        }";
}
