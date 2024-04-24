using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QE.ViewModel
{
    public class ButtonKeyboardRegistrationPreRecord : Button
    {
        public ButtonKeyboardRegistrationPreRecord(string content)
        {
            Width = 100;
            Height = 75;
            Content = content;
            Background = new SolidColorBrush(Colors.LightGray);
            Margin = new Thickness(4);
            FontSize = 18;
            Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
        public ButtonKeyboardRegistrationPreRecord(string content, int width)
        {
            Width = width;
            Height = 75;
            Content = content;
            Background = new SolidColorBrush(Colors.LightGray);
            Margin = new Thickness(4);
            FontSize = 18;
            Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
    }
}
