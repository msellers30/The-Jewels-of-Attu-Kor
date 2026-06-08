using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Game.Gui.Views
{
    /// <summary>
    /// Options popup. Shares the <see cref="ViewModels.MainViewModel"/> as its DataContext, so changing
    /// the layout here updates the main window live (the main window reacts to SelectedLayout changes).
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public OptionsWindow()
        {
            InitializeComponent();
        }

        private void OnClose(object? sender, RoutedEventArgs e) => Close();
    }
}
