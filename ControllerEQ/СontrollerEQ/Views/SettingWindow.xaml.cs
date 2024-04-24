using System.Windows;

namespace СontrollerEQ
{
    /// <summary>
    /// Логика взаимодействия для SettingWindow.xaml
    /// </summary>
    public partial class SettingWindow : Window
    {
        public string Connection { get; private set; }
        public SettingWindow(string connection)
        {
            InitializeComponent();
            ConnectionText.Text = connection;
        }

        void Accept_Click(object sender, RoutedEventArgs e)
        {
            Connection = ConnectionText.Text;
            DialogResult = true;
        }
    }
}
