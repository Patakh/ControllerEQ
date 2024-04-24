using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QE.ViewModel
{
    public class TextErorr : TextBlock
    {
        public TextErorr(string text)
        {
            FontSize = 72;
            Foreground = new SolidColorBrush(Colors.Black);
            Text = text;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
        }
    }
}
