using Avalonia.Platform;

namespace Game.Gui.Rooms
{
    /// <summary>
    /// Maps a room number to the image(s) to display for it. Returns a LIST per room so the planned
    /// "multiple images per room" (e.g. state variants like a broken window) is a non-change later;
    /// today each room resolves to zero or one embedded asset. Art is optional: drop PNGs under
    /// Assets/rooms (room{n}.png, plus room{n}_2.png, _3.png ... for extra variants) and they appear.
    /// When a room has no art yet, callers fall back to <see cref="PlaceholderText"/>.
    /// </summary>
    public sealed class RoomImageProvider
    {
        private const string AssetBase = "avares://Game.Gui/Assets/rooms/";

        // "" is the primary image; the rest are optional future variants for the same room.
        private static readonly string[] Suffixes = { "", "_2", "_3", "_4", "_5" };

        /// <summary>The embedded images that exist for this room, primary first.</summary>
        public IReadOnlyList<Uri> GetImages(int room)
        {
            var found = new List<Uri>();
            foreach (var suffix in Suffixes)
            {
                var uri = new Uri($"{AssetBase}room{room}{suffix}.png");
                if (AssetLoader.Exists(uri))
                {
                    found.Add(uri);
                }
            }

            return found;
        }

        /// <summary>A friendly room name to show when no art exists for the room yet.</summary>
        public string PlaceholderText(int room) =>
            RoomNames.TryGetValue(room, out var name) ? name : $"Room {room}";

        private static readonly IReadOnlyDictionary<int, string> RoomNames = new Dictionary<int, string>
        {
            [0] = "Hotel Lobby",
            [1] = "Your Room",
            [2] = "Party Room",
            [3] = "Tropical Lounge",
            [4] = "Wine Cellar",
            [5] = "Ancient Lab",
            [6] = "Downtown Street",
            [7] = "Arcade",
            [8] = "Uptown Street",
            [9] = "Jim Dandy Market",
            [10] = "Motown Street",
            [11] = "Police Station",
            [12] = "Dark Alley",
            [13] = "Jail Cell",
            [14] = "Evidence Room",
            [15] = "Temple of Attu-Kor",
        };
    }
}
