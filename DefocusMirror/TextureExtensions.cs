using SharpDX;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using MapFlags = SharpDX.Direct3D11.MapFlags;

namespace DefocusMirror
{
    public static class TextureExtensions
    {
        public static byte[] GetBuffer(this Texture2D texture)
        {
            var device = texture.Device;
            var stagingDesc = new Texture2DDescription
            {
                Width = texture.Description.Width,
                Height = texture.Description.Height,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.B8G8R8A8_UNorm,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Staging,
                BindFlags = BindFlags.None,
                CpuAccessFlags = CpuAccessFlags.Read,
                OptionFlags = ResourceOptionFlags.None
            };
            using var stagingTexture = new Texture2D(device, stagingDesc);
            device.ImmediateContext.CopyResource(texture, stagingTexture);

            var mappedSubresource = device.ImmediateContext.MapSubresource(stagingTexture, 0, MapMode.Read, MapFlags.None, out DataStream dataStream);
            var data = new byte[dataStream.Length];
            dataStream.ReadExactly(data);
            return data;
        }


        public static byte[] GetBuffer2(this Texture2D texture)
        {
            var device = texture.Device;
            var stagingDesc = new Texture2DDescription
            {
                Width = texture.Description.Width,
                Height = texture.Description.Height,
                MipLevels = 1,
                ArraySize = 1,
                Format = Format.B8G8R8A8_UNorm,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Staging,
                BindFlags = BindFlags.None,
                CpuAccessFlags = CpuAccessFlags.Read,
                OptionFlags = ResourceOptionFlags.None
            };
            var stagingTexture = new Texture2D(device, stagingDesc);

            device.ImmediateContext.CopyResource(texture, stagingTexture);
            var mappedSubresource = device.ImmediateContext.MapSubresource(stagingTexture, 0, MapMode.Read, MapFlags.None, out DataStream dataStream);
            using var st = new StreamReader(dataStream);
            return [.. st.ReadToEnd().Select(c => (byte)c)];
        }


    }
}