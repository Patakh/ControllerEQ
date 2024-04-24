using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QE.ViewModel
{
    public class TextEmpty : TextBlock
    {
        public TextEmpty()
        {
            FontSize = 40;
            Foreground = new SolidColorBrush(Colors.LightGray);
            Text = "Нет данных";
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
        }
    }
}
