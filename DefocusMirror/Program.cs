using System.Diagnostics;

namespace DefocusMirror;

internal static partial class Program
{
    [STAThread]
    static void Main()
    {
        using var mutex = new Mutex(true, "DefocusMirror_SingleInstance", out bool createdNew);
        if (!createdNew)
        {
            Debug.WriteLine("Another instance is already running.");
            return;
        }

        ApplicationConfiguration.Initialize();
        var culture = L.LoadSavedCulture();
        L.ApplyCulture(culture);
        Application.Run(new ControlBox());
    }
}
