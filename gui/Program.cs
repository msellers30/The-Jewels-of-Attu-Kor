using Avalonia;

namespace Game.Gui
{
    internal static class Program
    {
        // Avalonia configuration; also used by the visual designer. Don't move/rename without care.
        public static AppBuilder BuildAvaloniaApp() =>
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .WithInterFont()
                .LogToTrace();

        [STAThread]
        public static void Main(string[] args) =>
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }
}
