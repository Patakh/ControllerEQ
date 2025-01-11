using System.Windows;
using ControllerEQ.HandleKey; 
using ControllerEQ.ViewModel;

namespace ControllerEQ.Views;

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