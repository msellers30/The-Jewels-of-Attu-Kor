using System.Text.Json;
using System.Text.Json.Serialization;
using Game.Gui.ViewModels;

namespace Game.Gui
{
    /// <summary>Persisted UI preferences: window placement/size and the chosen layout arrangement.</summary>
    public sealed class AppSettings
    {
        public int? WindowX { get; set; }
        public int? WindowY { get; set; }
        public double? WindowWidth { get; set; }
        public double? WindowHeight { get; set; }
        public LayoutMode? Layout { get; set; }
    }

    /// <summary>
    /// Loads/saves <see cref="AppSettings"/> as JSON under the user's application-data folder. All
    /// operations are best-effort: a missing/corrupt file yields defaults, and save failures are
    /// swallowed so they never crash the app or block closing.
    /// </summary>
    public static class SettingsStore
    {
        private static readonly string Dir =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "JewelsOfAttuKor");

        private static readonly string FilePath = Path.Combine(Dir, "settings.json");

        private static readonly JsonSerializerOptions Options = new()
        {
            WriteIndented = true,
            Converters = { new JsonStringEnumConverter() },
        };

        public static AppSettings Load()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    return JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(FilePath), Options) ?? new AppSettings();
                }
            }
            catch
            {
                // Ignore unreadable/corrupt settings and fall back to defaults.
            }

            return new AppSettings();
        }

        public static void Save(AppSettings settings)
        {
            try
            {
                Directory.CreateDirectory(Dir);
                File.WriteAllText(FilePath, JsonSerializer.Serialize(settings, Options));
            }
            catch
            {
                // Best-effort persistence; never let a save failure surface to the user.
            }
        }
    }
}
