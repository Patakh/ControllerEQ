﻿using ControllerEQ.Views.Error;

namespace ControllerEQ.Model;
class ShowErrorModel
{
    public static bool? ShowError(string errorMessage)
        => new ErrorWindow(errorMessage).ShowDialog();
}

