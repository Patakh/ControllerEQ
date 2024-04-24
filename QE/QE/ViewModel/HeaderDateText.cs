using QE.Models.DTO;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QE.ViewModel
{
    public class HeaderDateText : TextBox
    {
        public HeaderDateText(string text)
        {
            FontSize = 30;
            FontFamily = new FontFamily("Area");
            Foreground = new SolidColorBrush(Colors.Black);
            BorderBrush = new SolidColorBrush(Colors.Transparent);
            Text = text;
        }
        public HeaderDateText(ColorDto color, string text)
        {
            FontSize = 30;
            FontFamily = new FontFamily("Area");
            Foreground = new SolidColorBrush(color.ColorTextHeader);
            BorderBrush = new SolidColorBrush(Colors.Transparent);
            Text = text;
        }

        public HeaderDateText(ColorDto color, DateTime date)
        {
            FontSize = 30;
            FontFamily = new FontFamily("Area");
            Foreground = new SolidColorBrush(color.ColorTextHeader);
            BorderBrush = new SolidColorBrush(Colors.Transparent);
            Text = date.ToString("F") + "\n" + date.ToString("dddd");
            TextAlignment = TextAlignment.Right;
        }

        public HeaderDateText(string text, HorizontalAlignment horizontal)
        {
            FontSize = 30;
            FontFamily = new FontFamily("Area");
            Foreground = new SolidColorBrush(Colors.Black);
            HorizontalAlignment = horizontal;
            BorderBrush = new SolidColorBrush(Colors.Transparent);
            Text = text;
        }
        public HeaderDateText(ColorDto color, string text, HorizontalAlignment horizontal)
        {
            FontSize = 30;
            FontFamily = new FontFamily("Area");
            Foreground = new SolidColorBrush(color.ColorTextHeader);
            HorizontalAlignment = horizontal;
            BorderBrush = new SolidColorBrush(Colors.Transparent);
            Text = text;
        }
    }
}
