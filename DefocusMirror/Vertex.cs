using SharpDX;
using System.Runtime.InteropServices;

namespace DefocusMirror;

[StructLayout(LayoutKind.Sequential)]
public struct Vertex(Vector4 position, Vector2 texCoord)
{
    public Vector4 Position = position;
    public Vector2 TexCoord = texCoord;
}
