using QE.Models.DTO;
using System.Windows.Controls;
using System.Windows.Media;

namespace QE.ViewModel
{
    public class HeaderOfficeText : TextBox
    {
        public HeaderOfficeText(ColorDto color, string name)
        {
            FontFamily = new FontFamily("Area");
            FontSize = 30;
            //Background = new SolidColorBrush(Colors.White);
            Foreground = new SolidColorBrush(color.ColorTextHeader);
            BorderBrush = new SolidColorBrush(Colors.Transparent);
            Text = name;
        }
    }
}
