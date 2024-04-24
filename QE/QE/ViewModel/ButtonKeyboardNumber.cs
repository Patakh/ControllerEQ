using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QE.ViewModel
{
    public class ButtonKeyboardNumber : Button
    {
        public ButtonKeyboardNumber(string content)
        {
            Width = 60;
            Height = 60;
            Content = content;
            Background = new SolidColorBrush(Colors.LightGray);
            Margin = new Thickness(4);
            FontSize = 18;
            Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
        public ButtonKeyboardNumber(string content, int width)
        {
            Width = width;
            Height = 60;
            Content = content;
            Background = new SolidColorBrush(Colors.LightGray);
            Margin = new Thickness(4);
            FontSize = 18;
            Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
    }
}
