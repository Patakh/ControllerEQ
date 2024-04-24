using System.Windows;
using СontrollerEQ.Model;
using СontrollerEQ.ViewModel;
using static СontrollerEQ.ViewModel.ClientListViewModel;

namespace СontrollerEQ.Views;
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
