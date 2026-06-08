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

        public MainWindow()
        {
            InitializeComponent();

            DataContextChanged += (_, _) => HookViewModel();
            Loaded += (_, _) =>
            {
                HookViewModel();
                ApplyLayout();
                InputBox.Focus();
            };
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

        /// <summary>Positions and sizes the image panel for the selected layout; the transcript fills the rest.</summary>
        private void ApplyLayout()
        {
            var mode = _viewModel?.SelectedLayout.Mode ?? LayoutMode.ImageLeft;
            switch (mode)
            {
                case LayoutMode.ImageLeft:
                    DockPanel.SetDock(ImagePanel, Dock.Left);
                    ImagePanel.Width = 320;
                    ImagePanel.Height = double.NaN;
                    ImagePanel.Margin = new Thickness(0, 0, 10, 0);
                    break;

                case LayoutMode.ImageRight:
                    DockPanel.SetDock(ImagePanel, Dock.Right);
                    ImagePanel.Width = 320;
                    ImagePanel.Height = double.NaN;
                    ImagePanel.Margin = new Thickness(10, 0, 0, 0);
                    break;

                case LayoutMode.ImageTop:
                    DockPanel.SetDock(ImagePanel, Dock.Top);
                    ImagePanel.Width = double.NaN;
                    ImagePanel.Height = 240;
                    ImagePanel.Margin = new Thickness(0, 0, 0, 10);
                    break;
            }
        }

        private void ScrollTranscriptToEnd() =>
            TranscriptScroller.Offset = TranscriptScroller.Offset.WithY(TranscriptScroller.Extent.Height);
    }
}
