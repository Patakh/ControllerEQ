using ÑontrollerEQ.ViewModel;
using System.Windows;
namespace ControllerEQTest;
public class MainWindowViewModelTests
{
    [Fact]
    public void BodyVisibility_SetProperty_NotifiesChange()
    {
        // Arrange
        var mainWindowViewModel = new MainWindowViewModel();
        bool propertyChangedRaised = false;
        mainWindowViewModel.PropertyChanged += (sender, args) => 
        {
            if (args.PropertyName == "BodyVisibility")
            {
                propertyChangedRaised = true;
            }
        };

        // Act
        mainWindowViewModel.BodyVisibility = Visibility.Collapsed;

        // Assert
        Assert.True(propertyChangedRaised);
        Assert.Equal(Visibility.Collapsed, mainWindowViewModel.BodyVisibility);
    }

    [Fact]
    public void ShowMainWindowCommand_ExecutesLogic()
    {
        // Arrange
        var mainWindowViewModel = new MainWindowViewModel();
        bool showMainWindowCommandExecuted = false;
        mainWindowViewModel.BodyVisibility = Visibility.Visible;
        mainWindowViewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == "BodyVisibility")
            {
                if (mainWindowViewModel.BodyVisibility == Visibility.Hidden)
                {
                    showMainWindowCommandExecuted = true;
                }
            }
        };

        // Act
        mainWindowViewModel.ShowMainWindowCommand.Execute(null);

        // Assert
        Assert.True(showMainWindowCommandExecuted);
        Assert.Equal(Visibility.Hidden, mainWindowViewModel.BodyVisibility);
    }
     

}
