using System.Windows;
using СontrollerEQ.HandleKey; 
using СontrollerEQ.ViewModel;

namespace СontrollerEQ.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        PreviewKeyDown += (s, e) => KeysDown.Press(e); 
        DataContext =  new MainWindowViewModel();
    } 
}