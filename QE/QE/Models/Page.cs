using QE.ViewModel;
using System.Windows.Controls;

namespace QE.Models
{
    public class Page
    {
        public static void Start(Grid panel, string message)
        {
            panel.Children.Clear();
            panel.Children.Add(new PanelStart(message));
        }
        public static void Error(Grid panel, string message)
        {
            panel.Children.Clear();
            panel.Children.Add(new PanelError(message));
        }
    }
}
