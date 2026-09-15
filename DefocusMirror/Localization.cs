using System.Globalization;
using System.Resources;
using DefocusMirror.Properties;

namespace DefocusMirror;

internal static class L
{
    private static readonly ResourceManager Rm = Strings.ResourceManager;

    public static string CurrentCulture { get; private set; } = "zh-CN";

    public static string S(string key) => Rm.GetString(key, CultureInfo.CurrentUICulture) ?? key;

    public static void ApplyCulture(string culture)
    {
        CurrentCulture = culture;
        var ci = new CultureInfo(culture);
        Thread.CurrentThread.CurrentCulture = ci;
        Thread.CurrentThread.CurrentUICulture = ci;
        Strings.Culture = ci;
    }

    public static void SaveCulture()
    {
        try { File.WriteAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lang.txt"), CurrentCulture); } catch { }
    }

    public static string LoadSavedCulture()
    {
        try
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lang.txt");
            if (File.Exists(path))
            {
                var c = File.ReadAllText(path).Trim();
                if (c is "zh-CN" or "en") return c;
            }
        }
        catch { }
        return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName == "zh" ? "zh-CN" : "en";
    }
}
