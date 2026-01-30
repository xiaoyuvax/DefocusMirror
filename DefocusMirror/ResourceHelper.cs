using System.Reflection;
using System.Text;

namespace DefocusMirror;

public static class ResourceHelper
{
    /// <summary>
    /// 例：ResourceMan.GetEmbeddedRes("VisualDataHub.L5L.Lib.js");
    /// </summary>
    /// <param name="resName"></param>
    /// <returns></returns>
    public static string GetBinRes(string resName) => Encoding.UTF8.GetString(Assembly.GetCallingAssembly().GetBinRes(resName));

    /// <summary>
    /// 获取内嵌资源
    /// </summary>
    /// <param name="resourceName"></param>
    /// <returns></returns>
    public static byte[] GetBinRes(this Assembly asm, string resourceName)
    {
        using (Stream st = asm.GetManifestResourceStream(resourceName))
            if (st != null) using (BinaryReader sr = new(st)) return sr.ReadBytes((int)st.Length);
        return [];
    }
}
