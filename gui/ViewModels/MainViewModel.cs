using System.ComponentModel;
using System.Runtime.CompilerServices;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Game.Gui.Rooms;
using GameEngine = global::Game.Game;
using TurnResult = global::Game.TurnResult;

namespace Game.Gui.ViewModels
{
    /// <summary>Which side the room image sits on. Chosen by the user at runtime.</summary>
    public enum LayoutMode { ImageLeft, ImageRight, ImageTop }

    /// <summary>A selectable layout choice with a friendly label for the picker.</summary>
    public sealed class LayoutOption
    {
        public required LayoutMode Mode { get; init; }
        public required string Label { get; init; }
        public override string ToString() => Label;
    }

    /// <summary>
    /// Drives the GUI front-end over the decoupled engine. It owns a <see cref="GameEngine"/> and
    /// renders each <see cref="TurnResult"/> into the transcript + room image. The PUT/bottle two-step
    /// needs no special handling here: the engine suspends/resumes internally, so we just keep feeding
    /// it input lines.
    /// </summary>
    public sealed class MainViewModel : INotifyPropertyChanged
    {
        private readonly GameEngine _engine = new();
        private readonly RoomImageProvider _images = new();
        private bool _isGameOver;

        public MainViewModel()
        {
            Layouts = new List<LayoutOption>
            {
                new() { Mode = LayoutMode.ImageLeft,  Label = "Image left"  },
                new() { Mode = LayoutMode.ImageRight, Label = "Image right" },
                new() { Mode = LayoutMode.ImageTop,   Label = "Image top"   },
            };
            _selectedLayout = Layouts.First(option => option.Mode == LayoutMode.ImageLeft);

            Render(_engine.Start());
        }

        public IReadOnlyList<LayoutOption> Layouts { get; }

        private LayoutOption _selectedLayout;
        public LayoutOption SelectedLayout
        {
            get => _selectedLayout;
            set => Set(ref _selectedLayout, value);
        }

        private string _transcript = string.Empty;
        public string Transcript
        {
            get => _transcript;
            private set => Set(ref _transcript, value);
        }

        private string _input = string.Empty;
        public string Input
        {
            get => _input;
            set => Set(ref _input, value);
        }

        private Bitmap? _roomImage;
        public Bitmap? RoomImage
        {
            get => _roomImage;
            private set
            {
                if (Set(ref _roomImage, value))
                {
                    OnPropertyChanged(nameof(HasImage));
                }
            }
        }

        public bool HasImage => _roomImage is not null;

        private string _roomPlaceholder = string.Empty;
        public string RoomPlaceholder
        {
            get => _roomPlaceholder;
            private set => Set(ref _roomPlaceholder, value);
        }

        public bool IsInputEnabled => !_isGameOver;

        /// <summary>Submit the current input line to the engine (invoked by the view on Enter/Send).</summary>
        public void Submit()
        {
            if (_isGameOver)
            {
                return;
            }

            var line = Input ?? string.Empty;
            Append((Transcript.Length == 0 ? string.Empty : "\n") + "> " + line + "\n");
            Input = string.Empty;

            Render(_engine.ProcessCommand(line));
        }

        private void Render(TurnResult result)
        {
            Append(result.Text);
            UpdateImage(result.Room);

            if (result.GameOver)
            {
                _isGameOver = true;
                OnPropertyChanged(nameof(IsInputEnabled));
                Append("\n[ The End — close the window to exit. ]\n");
            }
        }

        private void UpdateImage(int room)
        {
            var images = _images.GetImages(room);
            if (images.Count > 0)
            {
                using var stream = AssetLoader.Open(images[0]);
                RoomImage = new Bitmap(stream);
                RoomPlaceholder = string.Empty;
            }
            else
            {
                RoomImage = null;
                RoomPlaceholder = _images.PlaceholderText(room);
            }
        }

        private void Append(string text) => Transcript += text;

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string? name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        private bool Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
            {
                return false;
            }

            field = value;
            OnPropertyChanged(name);
            return true;
        }
    }
}
