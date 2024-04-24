using QE.Models;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Window = System.Windows.Window;

namespace QE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += async (s, e) =>
            {
                await Window_Loaded(s, e);
            };
            PreviewKeyDown += HandleKeyPress;
        }

        private async Task Window_Loaded(object sender, RoutedEventArgs e)
        {
            Main main = new Main(this);
            await main.InitTerminal();
        }

        #region закритие приложения
        private void HandleKeyPress(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control && e.Key == Key.Escape)
            {
                // Закрываем приложение
                Application.Current.Shutdown();
            }
            if (e.Key == Key.F1)
            {
                var settingWindow = new SettingWindow(Properties.Settings.Default.connection);
                if (settingWindow.ShowDialog() == true)
                {
                    Properties.Settings.Default.connection = settingWindow.Connection;
                    Properties.Settings.Default.Save();
                }
            }
            if (e.Key == Key.F2)
            {
                MainWindow newWindow = new MainWindow();
                Application.Current.MainWindow = newWindow;
                newWindow.Show();
                this.Close();
            }
        }
        #endregion



    }
}
