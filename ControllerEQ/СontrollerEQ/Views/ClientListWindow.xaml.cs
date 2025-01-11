using System.Windows;
using ControllerEQ.Model;
using ControllerEQ.ViewModel;
using static ControllerEQ.ViewModel.ClientListViewModel;

namespace ControllerEQ.Views;
/// <summary>
/// Логика взаимодействия для ClientList.xaml
/// </summary>
public partial class ClientListWindow : Window
{
    public ClientListWindow(Status Status)
    {
        InitializeComponent();
        DataContext = new ClientListViewModel(this, Status);
    }

    private void CommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {

    }
}
