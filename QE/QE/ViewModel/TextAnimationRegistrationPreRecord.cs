using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QE.ViewModel
{
    public class TextAnimationRegistrationPreRecord : TextBlock
    {
        public TextAnimationRegistrationPreRecord()
        {
            FontFamily = new FontFamily("Area");
            FontSize = 60;
            TextWrapping = TextWrapping.Wrap;
            HorizontalAlignment = HorizontalAlignment.Center;
            Foreground = new SolidColorBrush(Color.FromRgb(25, 51, 10));
            Text = "Теперь выберите услугу";
        }
    }
}
