using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QE.ViewModel
{
    public class ButtonPreRecordTime : Button
    {
        public ButtonPreRecordTime(string textContent)
        {
            Content = textContent;
            VerticalAlignment = VerticalAlignment.Center;
            Height = 75;
            Width = 200;
            Margin = new Thickness(32, 18, 0, 0);
            Background = new SolidColorBrush(Color.FromRgb(81, 96, 151));
            BorderBrush = new SolidColorBrush(Color.FromRgb(255, 250, 255));
            FontFamily = new FontFamily("Area");
            FontSize = 20;
            Foreground = new SolidColorBrush(Color.FromRgb(252, 252, 240));
        }
    }
}
