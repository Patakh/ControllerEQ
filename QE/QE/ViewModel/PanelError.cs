using System.Windows;
using System.Windows.Controls;

namespace QE.ViewModel
{
    public class PanelError : WrapPanel
    {
        public PanelError(string errorMessage)
        {
            Orientation = Orientation.Vertical;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            Children.Add(new TextErorr(errorMessage));
        }
    }
}
