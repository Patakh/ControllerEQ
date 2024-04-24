using System;
using System.Windows;
using СontrollerEQ.Model;
using СontrollerEQ.ViewModel;

namespace СontrollerEQ.Views; 
public partial class TransverClientWindow : Window
{ 
    public TransverClientWindow(ClientModel client)
    {
        InitializeComponent();
        DataContext = new TransverClientWindowViewModel(client, this); 
    } 
}