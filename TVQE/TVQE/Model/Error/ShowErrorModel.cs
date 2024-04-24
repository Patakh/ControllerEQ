 using TVQE.Views.Error;

namespace TVQE.Model;
class ShowErrorModel
{
    public static bool? ShowError(string errorMessage)
        => new ErrorWindow(errorMessage).ShowDialog();
}

