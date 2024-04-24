using System.Windows;
using TVQE.HandleKey;
using TVQE.ViewModel;

namespace TVQE;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        PreviewKeyDown += (s, e) =>  KeysDown.Press(e);
        InitializeComponent();
        DataContext = new MainWindowViewModel(MediaElementBanner, Convas, RunningText, RotationAxisClientList, RotationAxisClient);
    }
}