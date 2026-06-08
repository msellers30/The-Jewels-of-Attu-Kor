using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Game.Gui.ViewModels;

namespace Game.Gui.Views
{
    public partial class MainWindow : Window
    {
        private MainViewModel? _viewModel;
        private readonly AppSettings _settings = SettingsStore.Load();

        public MainWindow()
        {
            InitializeComponent();

            RestoreWindowBounds();

            DataContextChanged += (_, _) => HookViewModel();
            Loaded += (_, _) =>
            {
                HookViewModel();
                RestoreLayout();
                ApplyLayout();
                InputBox.Focus();
            };
            Closing += (_, _) => SaveSettings();
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            EnsureWindowOnScreen();
        }

        private void HookViewModel()
        {
            if (_viewModel is not null)
            {
                _viewModel.PropertyChanged -= OnViewModelPropertyChanged;
            }

            _viewModel = DataContext as MainViewModel;

            if (_viewModel is not null)
            {
                _viewModel.PropertyChanged += OnViewModelPropertyChanged;
            }
        }

        private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainViewModel.SelectedLayout))
            {
                ApplyLayout();
            }
            else if (e.PropertyName == nameof(MainViewModel.Transcript))
            {
                // Keep the newest output visible.
                Dispatcher.UIThread.Post(ScrollTranscriptToEnd, DispatcherPriority.Background);
            }
        }

        private void OnSubmit(object? sender, RoutedEventArgs e)
        {
            _viewModel?.Submit();
            InputBox.Focus();
        }

        private async void OnOpenOptions(object? sender, RoutedEventArgs e)
        {
            // Share the same view-model so layout changes in the popup apply to this window live.
            await new OptionsWindow { DataContext = DataContext }.ShowDialog(this);
            InputBox.Focus();
        }

        /// <summary>
        /// Arranges the image panel and the content (transcript) panel into the proportional grid for
        /// the selected layout. Uses star sizing so each area is a fixed PERCENTAGE of the window and
        /// resizes live with it: a 50/50 (1*:1*) split between image and transcript.
        /// </summary>
        private void ApplyLayout()
        {
            var mode = _viewModel?.SelectedLayout.Mode ?? LayoutMode.ImageLeft;

            MainArea.ColumnDefinitions.Clear();
            MainArea.RowDefinitions.Clear();
            Grid.SetRow(ImagePanel, 0);
            Grid.SetColumn(ImagePanel, 0);
            Grid.SetRow(ContentPanel, 0);
            Grid.SetColumn(ContentPanel, 0);

            switch (mode)
            {
                case LayoutMode.ImageLeft:
                    MainArea.ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));
                    MainArea.ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));
                    Grid.SetColumn(ImagePanel, 0);
                    Grid.SetColumn(ContentPanel, 1);
                    ImagePanel.Margin = new Thickness(0, 0, 10, 0);
                    break;

                case LayoutMode.ImageRight:
                    MainArea.ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));
                    MainArea.ColumnDefinitions.Add(new ColumnDefinition(1, GridUnitType.Star));
                    Grid.SetColumn(ContentPanel, 0);
                    Grid.SetColumn(ImagePanel, 1);
                    ImagePanel.Margin = new Thickness(10, 0, 0, 0);
                    break;

                case LayoutMode.ImageTop:
                    MainArea.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));
                    MainArea.RowDefinitions.Add(new RowDefinition(1, GridUnitType.Star));
                    Grid.SetRow(ImagePanel, 0);
                    Grid.SetRow(ContentPanel, 1);
                    ImagePanel.Margin = new Thickness(0, 0, 0, 10);
                    break;
            }
        }

        private void ScrollTranscriptToEnd() =>
            TranscriptScroller.Offset = TranscriptScroller.Offset.WithY(TranscriptScroller.Extent.Height);

        // --- Window persistence: restore saved size/position/layout on open, save them on close. ---

        private void RestoreWindowBounds()
        {
            if (_settings.WindowWidth is { } w && _settings.WindowHeight is { } h)
            {
                Width = w;
                Height = h;
            }

            if (_settings.WindowX is { } x && _settings.WindowY is { } y)
            {
                WindowStartupLocation = WindowStartupLocation.Manual;
                Position = new PixelPoint(x, y);
            }
        }

        private void RestoreLayout()
        {
            if (_viewModel is null || _settings.Layout is not { } saved)
            {
                return;
            }

            var option = _viewModel.Layouts.FirstOrDefault(o => o.Mode == saved);
            if (option is not null)
            {
                _viewModel.SelectedLayout = option;
            }
        }

        private void SaveSettings()
        {
            _settings.WindowX = Position.X;
            _settings.WindowY = Position.Y;
            _settings.WindowWidth = ClientSize.Width;
            _settings.WindowHeight = ClientSize.Height;
            if (_viewModel is not null)
            {
                _settings.Layout = _viewModel.SelectedLayout.Mode;
            }

            SettingsStore.Save(_settings);
        }

        // If the saved position lands off every screen (e.g. a monitor was unplugged), recenter on the
        // primary screen so the window can't get stranded out of view.
        private void EnsureWindowOnScreen()
        {
            try
            {
                var screens = Screens;
                if (screens is null || screens.All.Count == 0)
                {
                    return;
                }

                var topLeft = Position;
                if (screens.All.Any(s => s.Bounds.Contains(topLeft)))
                {
                    return;
                }

                var target = screens.Primary ?? screens.All[0];
                var bounds = target.Bounds;
                Position = new PixelPoint(
                    bounds.X + Math.Max(0, (bounds.Width - (int)ClientSize.Width) / 2),
                    bounds.Y + Math.Max(0, (bounds.Height - (int)ClientSize.Height) / 2));
            }
            catch
            {
                // Screen enumeration is best-effort; never block the window from opening.
            }
        }
    }
}
