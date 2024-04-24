using System.Windows;
using System.Windows.Controls;

namespace QE.ViewModel
{
    public class PanelStart : WrapPanel
    {
        public PanelStart(string startMessage)
        {
            Orientation = Orientation.Vertical;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            Children.Add(new TextStart(startMessage));
        }
    }
}
