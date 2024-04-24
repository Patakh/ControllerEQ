using СontrollerEQ.Views.Error;

namespace СontrollerEQ.Model;
class ShowErrorModel
{
    public static bool? ShowError(string errorMessage)
        => new ErrorWindow(errorMessage).ShowDialog();
}

