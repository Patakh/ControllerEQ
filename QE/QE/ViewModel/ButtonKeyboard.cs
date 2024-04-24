using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QE.ViewModel
{
    public class ButtonKeyboard : Button
    {
        public ButtonKeyboard(string content)
        {
            Width = 48;
            Height = 48;
            Content = content;
            Background = new SolidColorBrush(Colors.LightGray);
            Margin = new Thickness(4);
            Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
        public ButtonKeyboard(string content, int width)
        {
            Width = width;
            Height = 48;
            Content = content;
            Background = new SolidColorBrush(Colors.LightGray);
            Margin = new Thickness(4);
            Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
    }
}
