using SharpDX;
using SharpDX.D3DCompiler;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using Buffer = SharpDX.Direct3D11.Buffer;
using Device = SharpDX.Direct3D11.Device;

namespace EyeGuardGDI
{
    public partial class ScreenCapture2 : IDisposable
    {
        private Adapter _adapter;
        private Texture2D _backBuffer;
        private Buffer _blurParamsBuffer;
        private DepthStencilState _depthStencilState;
        private DepthStencilView _depthStencilView;
        private Device _device;
        private Buffer _indexBuffer;
        private InputLayout _inputLayout;
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
        private OutputDuplication _outputDuplication;
        private Texture2D _intermediateTexture;
        private RenderTargetView _intermediateRenderTargetView;
        private ShaderResourceView _intermediateShaderResourceView;
        public ScreenCapture2()
        {
        }

        public void CaptureScreen()
        {
            // 获取屏幕内容
            SharpDX.DXGI.Resource desktopResource = default;
            try
            {
                if (_outputDuplication.TryAcquireNextFrame(1000, out var frameInfo, out desktopResource).Success)
                    using (var texture = desktopResource.QueryInterface<Texture2D>())
                    {
                        // 将桌面资源复制到中间纹理（用于避免循环捕捉）
                        _device.ImmediateContext.CopyResource(texture, _intermediateTexture);
                    }
            }
            finally
            {
                // 必须释放帧，否则会阻塞或泄露
                desktopResource?.Dispose();
                _outputDuplication.ReleaseFrame();
            }

            // 现在将中间纹理的内容复制到屏幕纹理用于渲染
            _device.ImmediateContext.CopyResource(_intermediateTexture, _screenTexture);
        }

        public void Dispose()
        {
            // 释放资源
            _renderTargetView?.Dispose();
            _depthStencilView?.Dispose();
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
            _swapChain?.Dispose();
            _device?.Dispose();
            _outputDuplication?.Dispose();
            _intermediateTexture?.Dispose();
            _intermediateRenderTargetView?.Dispose();
            _intermediateShaderResourceView?.Dispose();
            _blurParamsBuffer?.Dispose();
        }

        public void Initialize(nint hwnd)
        {
            // 初始化Direct3D 11
            var dxgiFactory = new Factory1();
            _adapter = dxgiFactory.Adapters[0];
            _device = new Device(_adapter, DeviceCreationFlags.Debug, FeatureLevel.Level_11_0).QueryInterface<SharpDX.Direct3D11.Device1>();

            var output = _adapter.Outputs[0];
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
                _screenWidth,          // 后台缓冲区宽度
                _screenHeight,          // 后台缓冲区高度
                new Rational(30, 1), // 刷新率 (分子, 分母)，例如 60Hz
                Format.B8G8R8A8_UNorm     // 像素格式，最常用的格式之一
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

            // 创建中间纹理用于存储原始屏幕内容（避免捕捉到自己的窗口）
            _intermediateTexture = new Texture2D(_device, new Texture2DDescription()
            {
                Width = _screenWidth,
                Height = _screenHeight,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.B8G8R8A8_UNorm,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                BindFlags = BindFlags.ShaderResource | BindFlags.RenderTarget,
                CpuAccessFlags = CpuAccessFlags.None,
                OptionFlags = ResourceOptionFlags.None
            });

            _intermediateRenderTargetView = new RenderTargetView(_device, _intermediateTexture);
            _intermediateShaderResourceView = new ShaderResourceView(_device, _intermediateTexture);

            // 创建深度缓冲区
            var depthStencilTexture = new Texture2D(_device, new Texture2DDescription()
            {
                Width = _screenWidth,
                Height = _screenHeight,
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
            var blurParams = new BlurParams(_screenWidth, _screenHeight, 5f, 5f, 5f, 0.1f, 1f, 2f);
            _device.ImmediateContext.UpdateSubresource(ref blurParams, _blurParamsBuffer);
            _device.ImmediateContext.VertexShader.SetConstantBuffer(0, _blurParamsBuffer);
            _device.ImmediateContext.PixelShader.SetConstantBuffer(0, _blurParamsBuffer);


            // 创建顶点和索引缓冲区
            var vertices = new[]
            {
                new Vertex(new Vector3(-1.0f, 1.0f, 0.0f), new Vector2(0.0f, 0.0f)),
                new Vertex(new Vector3(1.0f, 1.0f, 0.0f), new Vector2(1.0f, 0.0f)),
                new Vertex(new Vector3(1.0f, -1.0f, 0.0f), new Vector2(1.0f, 1.0f)),
                new Vertex(new Vector3(-1.0f, -1.0f, 0.0f), new Vector2(0.0f, 1.0f))
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
            var viewport = new Viewport(0, 0, _screenWidth, _screenHeight, 0.0f, 1.0f);
            _device.ImmediateContext.Rasterizer.SetViewports([viewport]);

            // 清除渲染目标
            _device.ImmediateContext.ClearRenderTargetView(_renderTargetView, new Color4(0, 0, 0, 1));

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
            _swapChain.Present(0, PresentFlags.None);
        }

        private void CreateInputLayout()
        {
            // 编译顶点着色器代码
            string vsCode = @"
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

cbuffer BlurParams : register(b0)
{
    float Width;
    float Height;
    float BlurScaleR;
    float BlurSigmaR;
    float BlurScaleG;
    float BlurSigmaG;
    float BlurScaleB;
    float BlurSigmaB;
}

VertexOutput VS(VertexInput input)
{
    VertexOutput output;
    output.Position = input.Position;
    output.TexCoord = input.TexCoord;
    return output;
}";

            var vertexShaderByteCode = ShaderBytecode.Compile(vsCode, "VS", "vs_5_0");
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
            // 内联像素着色器代码，实现RGB通道分别模糊
            string psCode = @"
struct VertexInput
{
    float4 Position : SV_POSITION;
    float2 TexCoord : TEXCOORD0;
};

Texture2D screenTexture : register(t0);
SamplerState samplerState : register(s0);

cbuffer BlurParams : register(b0)
{
    float Width;
    float Height;
    float BlurScaleR;
    float BlurSigmaR;
    float BlurSigmaG;
    float BlurScaleG;
    float BlurScaleB;
    float BlurSigmaB;
}

float gaussian(float x, float sigma)
{
    return exp(-(x * x) / (2.0 * sigma * sigma));
}

float4 PS(VertexInput input) : SV_Target
{
    float2 texelSize = float2(1.0 / Width, 1.0 / Height);
    float4 color = float4(0, 0, 0, 0);
    float totalWeight = 0.0;
    
    // R通道模糊
    float4 rColor = float4(0, 0, 0, 0);
    float rWeightTotal = 0.0;
    [unroll]
    for (int i = -3; i <= 3; ++i)
    {
        float weight = gaussian(i, BlurSigmaR);
        float2 offset = float2(i * texelSize.x * BlurScaleR, 0);
        float4 sampleColor = screenTexture.Sample(samplerState, input.TexCoord + offset);
        rColor += sampleColor * weight;
        rWeightTotal += weight;
    }
    rColor /= rWeightTotal;
    
    // G通道模糊
    float4 gColor = float4(0, 0, 0, 0);
    float gWeightTotal = 0.0;
    [unroll]
    for (int i = -3; i <= 3; ++i)
    {
        float weight = gaussian(i, BlurSigmaG);
        float2 offset = float2(0, i * texelSize.y * BlurScaleG);
        float4 sampleColor = screenTexture.Sample(samplerState, input.TexCoord + offset);
        gColor += sampleColor * weight;
        gWeightTotal += weight;
    }
    gColor /= gWeightTotal;
    
    // B通道模糊
    float4 bColor = float4(0, 0, 0, 0);
    float bWeightTotal = 0.0;
    [unroll]
    for (int i = -3; i <= 3; ++i)
    {
        float weight = gaussian(i, BlurSigmaB);
        float2 offset = float2(i * texelSize.x * BlurScaleB, 0);
        float4 sampleColor = screenTexture.Sample(samplerState, input.TexCoord + offset);
        bColor += sampleColor * weight;
        bWeightTotal += weight;
    }
    bColor /= bWeightTotal;
    
    // 合成最终颜色
    color.r = rColor.r;
    color.g = gColor.g;
    color.b = bColor.b;
    color.a = 1.0;
    
    return color;
}";

            var pixelShaderByteCode = ShaderBytecode.Compile(psCode, "PS", "ps_5_0");
            _pixelShader = new PixelShader(_device, pixelShaderByteCode);
        }

        private void CreateVertexShader()
        {
            // 已在CreateInputLayout中创建
        }

        private struct Vertex
        {
            public Vector3 Position;
            public Vector2 TexCoord;

            public Vertex(Vector3 position, Vector2 texCoord)
            {
                Position = position;
                TexCoord = texCoord;
            }
        }
    }
}
