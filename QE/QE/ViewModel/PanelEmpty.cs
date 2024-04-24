using System.Windows;
using System.Windows.Controls;

namespace QE.ViewModel
{
    public class PanelEmpty : WrapPanel
    {
        public PanelEmpty()
        {
            Orientation = Orientation.Vertical;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            Children.Add(new TextEmpty());
        }
    }
}
