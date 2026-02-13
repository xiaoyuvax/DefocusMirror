using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;

namespace DefocusMirror;

public partial class ScreenCapture : IDisposable
{
    private Adapter _adapter;
    private Texture2D _backBuffer;
    private Buffer _blurParamsBuffer;
    private DepthStencilState _depthStencilState;
    private DepthStencilView _depthStencilView;
    private Device _device;
    private Buffer _indexBuffer;
    private InputLayout _inputLayout;
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
    private int? _sourceAdapterIndex;
    private int? _sourceOutputIndex;
    private int _frameRate;
    private int _targetViewportWidth;
    private int _targetViewportHeight;

    public ScreenCapture(float blurScaleR = 0, float blurScaleG = 0, float blurScaleB = 0, float blurSigmaR = 1, float blurSigmaG = 1, float blurSigmaB = 1, nint? hwnd = null, int? srcAdapterIndex = null, int? sourceOutputIndex = null, int? frameRate = 30, int? targetViewportWidth = 1920, int? targetViewportHeight = 1080)
    {
        blurParams = new BlurParams(1920, 1080, blurScaleR, blurScaleG, blurScaleB, blurSigmaR, blurSigmaG, blurSigmaB);
        _sourceAdapterIndex = srcAdapterIndex;
        _sourceOutputIndex = sourceOutputIndex;
        _frameRate = frameRate ?? 30;
        _targetViewportWidth = targetViewportWidth ?? 1920;
        _targetViewportHeight = targetViewportHeight ?? 1080;
        if (hwnd != null) Initialize(hwnd.Value);

    }


    private BlurParams blurParams;


    public void CaptureScreen()
    {
        // 获取屏幕内容

        SharpDX.DXGI.Resource desktopResource = default;
        try
        {
            if (_outputDuplication.TryAcquireNextFrame(1000, out var frameInfo, out desktopResource).Success)
            {
                using (var texture = desktopResource.QueryInterface<Texture2D>())
                {
                    //var debug = texture.GetBuffer();
                    // 将桌面资源复制到屏幕纹理
                    _device.ImmediateContext.CopyResource(texture, _screenTexture);
                    //var debug2 = _screenTexture.GetBuffer();
                }                
                _outputDuplication.ReleaseFrame();
            }
        }
        finally
        {
            // 必须释放帧，否则会阻塞或泄露
            desktopResource?.Dispose();
        }
    }

    public void Dispose()
    {
        // 释放资源
        _renderTargetView.Dispose();
        _depthStencilView.Dispose();
        _depthStencilState.Dispose();
        _vertexBuffer.Dispose();
        _indexBuffer.Dispose();
        _vertexShader.Dispose();
        _pixelShader.Dispose();
        _inputLayout.Dispose();
        _screenShaderResourceView.Dispose();
        _samplerState.Dispose();
        _screenTexture.Dispose();
        _backBuffer.Dispose();
        _swapChain.Dispose();
        _device.Dispose();
        _outputDuplication.Dispose();
    }

    public void Initialize(nint hwnd)
    {
        // 初始化Direct3D 11
        var dxgiFactory = new Factory1();
        _adapter = dxgiFactory.Adapters[_sourceAdapterIndex ?? 0];
        _device = new Device(_adapter, DeviceCreationFlags.None, FeatureLevel.Level_11_0).QueryInterface<SharpDX.Direct3D11.Device1>();

        using var output = _adapter.Outputs[_sourceOutputIndex ?? 0];
        // 使用 Output1（DXGI 1.2）以支持桌面复制
        using (var output1 = output.QueryInterface<Output1>())
        {
            // 从 DesktopBounds 计算屏幕分辨率
            var bounds = output1.Description.DesktopBounds;
            _screenWidth = bounds.Right - bounds.Left;
            _screenHeight = bounds.Bottom - bounds.Top;
            // 创建输出复制
            _outputDuplication = output1.DuplicateOutput(_device);
        }

        // 创建交换链
        var swapChainDescription = new SwapChainDescription()
        {
            ModeDescription = new ModeDescription(
            _targetViewportWidth,          // 后台缓冲区宽度
            _targetViewportHeight,          // 后台缓冲区高度
            new Rational(_frameRate, 1), // 刷新率 (分子, 分母)，例如 60Hz
            Format.B8G8R8A8_UNorm // 像素格式，最常用的格式之一
                                                 ),
            BufferCount = 1,
            IsWindowed = true,
            OutputHandle = hwnd,
            SampleDescription = new SampleDescription(1, 0),
            SwapEffect = SwapEffect.Discard,
            Usage = Usage.RenderTargetOutput
        };

        _swapChain = new SwapChain(dxgiFactory, _device, swapChainDescription);

        // 获取后缓冲区
        _backBuffer = _swapChain.GetBackBuffer<Texture2D>(0);
        _renderTargetView = new RenderTargetView(_device, _backBuffer);

        // 创建深度缓冲区
        var depthStencilTexture = new Texture2D(_device, new Texture2DDescription()
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

        _depthStencilView = new DepthStencilView(_device, depthStencilTexture);
        _depthStencilState = new DepthStencilState(_device, new DepthStencilStateDescription()
        {
            //IsDepthEnabled = true,
            //DepthWriteMask = DepthWriteMask.All,
            //DepthComparison = Comparison.Less
            IsDepthEnabled = false,
            DepthWriteMask = DepthWriteMask.Zero,
            DepthComparison = Comparison.Always
        });

        // 创建输入布局和着色器
        CreateInputLayout();
        CreateVertexShader();
        CreatePixelShader();

        // 创建屏幕纹理
        _screenTexture = new Texture2D(_device, new Texture2DDescription()
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

        // 创建采样器
        _samplerState = new SamplerState(_device, new SamplerStateDescription()
        {
            //Filter = Filter.Anisotropic,
            //AddressU = TextureAddressMode.Wrap,
            //AddressV = TextureAddressMode.Wrap,
            //AddressW = TextureAddressMode.Wrap,
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

        // 创建屏幕尺寸常量缓冲区
        _blurParamsBuffer = new Buffer(_device, new BufferDescription()
        {
            SizeInBytes = Utilities.SizeOf<BlurParams>(),
            Usage = ResourceUsage.Default,
            BindFlags = BindFlags.ConstantBuffer,
            CpuAccessFlags = CpuAccessFlags.None,
            OptionFlags = ResourceOptionFlags.None,
            StructureByteStride = 0
        });

        // 更新屏幕尺寸常量缓冲区        
        blurParams = new BlurParams(_screenWidth, _screenHeight, blurParams.BlurScaleR, blurParams.BlurScaleG, blurParams.BlurScaleB, blurParams.BlurSigmaR, blurParams.BlurSigmaG, blurParams.BlurSigmaB);
        _device.ImmediateContext.UpdateSubresource(ref blurParams, _blurParamsBuffer);
        _device.ImmediateContext.PixelShader.SetConstantBuffer(0, _blurParamsBuffer);

        // 创建顶点和索引缓冲区
        var vertices = new[]
        {
            // 顶点0: 左上角 (屏幕左上, 纹理左上)
            new Vertex(new Vector4(-1.0f,  1.0f, 0.0f, 1.0f), new Vector2(0.0f, 0.0f)),

            // 顶点1: 右上角 (屏幕右上, 纹理右上)
            new Vertex(new Vector4( 1.0f,  1.0f, 0.0f, 1.0f), new Vector2(1.0f, 0.0f)),

            // 顶点2: 右下角 (屏幕右下, 纹理右下)
            new Vertex(new Vector4( 1.0f, -1.0f, 0.0f, 1.0f), new Vector2(1.0f, 1.0f)),

            // 顶点3: 左下角 (屏幕左下, 纹理左下)
            new Vertex(new Vector4(-1.0f, -1.0f, 0.0f, 1.0f), new Vector2(0.0f, 1.0f))
        };

        var indices = new[] { 0, 1, 2, 0, 2, 3 };

        _vertexBuffer = Buffer.Create(_device, vertices, new BufferDescription()
        {
            Usage = ResourceUsage.Default,
            BindFlags = BindFlags.VertexBuffer,
            SizeInBytes = Utilities.SizeOf<Vertex>() * vertices.Length,
            CpuAccessFlags = CpuAccessFlags.None
        });

        _indexBuffer = Buffer.Create(_device, indices, new BufferDescription()
        {
            Usage = ResourceUsage.Default,
            BindFlags = BindFlags.IndexBuffer,
            SizeInBytes = Utilities.SizeOf<int>() * indices.Length,
            CpuAccessFlags = CpuAccessFlags.None
        });
    }

    public void Render()
    {
        // 设置渲染目标
        _device.ImmediateContext.OutputMerger.SetTargets(_depthStencilView, _renderTargetView);

        // 设置视口（使用实际屏幕分辨率）
        var viewport = new Viewport(0, 0, _targetViewportWidth, _targetViewportHeight, 0.0f, 1.0f);
        _device.ImmediateContext.Rasterizer.SetViewports([viewport]);

        // 清除渲染目标
        _device.ImmediateContext.ClearDepthStencilView(_depthStencilView, DepthStencilClearFlags.Depth, 0.5f, 0);
        _device.ImmediateContext.ClearRenderTargetView(_renderTargetView, new Color4(0, 0, 0, 1));

        // 设置深度/模板状态（完全禁用深度测试）
        _device.ImmediateContext.OutputMerger.SetDepthStencilState(_depthStencilState, 0);

        // 设置着色器
        _device.ImmediateContext.VertexShader.Set(_vertexShader);
        _device.ImmediateContext.PixelShader.Set(_pixelShader);

        // 设置输入布局
        _device.ImmediateContext.InputAssembler.InputLayout = _inputLayout;

        // 设置顶点和索引缓冲区
        _device.ImmediateContext.InputAssembler.SetVertexBuffers(0,
            new VertexBufferBinding(_vertexBuffer, Utilities.SizeOf<Vertex>(), 0));
        _device.ImmediateContext.InputAssembler.SetIndexBuffer(_indexBuffer, Format.R32_UInt, 0);
        _device.ImmediateContext.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;

        // 设置资源
        _device.ImmediateContext.PixelShader.SetShaderResource(0, _screenShaderResourceView);
        _device.ImmediateContext.PixelShader.SetSampler(0, _samplerState);

        // 绘制
        _device.ImmediateContext.DrawIndexed(6, 0, 0);

        // 交换链
        var r = _swapChain.Present(0, PresentFlags.None);
    }

    private void CreateInputLayout()
    {
        // 创建输入布局
        var vertexShaderByteCode = ShaderBytecode.Compile(@"struct VertexInput
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
        }    ", "VS", "vs_5_0");

        _vertexShader = new VertexShader(_device, vertexShaderByteCode);

        var inputElements = new[]
        {
            new InputElement("POSITION", 0, Format.R32G32B32A32_Float, 0, 0),
            new InputElement("TEXCOORD", 0, Format.R32G32_Float, 16, 0)
        };

        _inputLayout = new InputLayout(_device, vertexShaderByteCode, inputElements);
    }

    private void CreatePixelShader()
    {
        // 创建像素着色器
        var hlsl = ResourceHelper.GetBinRes("DefocusMirror.PixelShader.hlsl");
        var pixelShaderByteCode = ShaderBytecode.Compile(hlsl, "main", "ps_5_0");

        _pixelShader = new PixelShader(_device, pixelShaderByteCode);
    }

    private void CreateVertexShader()
    {
        // 已在CreateInputLayout中创建
    }
}