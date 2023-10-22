using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace СontrollerEQ.Modal
{

    /// <summary>
    /// модальное окно
    /// </summary> 
    public class ConfirmationDialog : Window
    {
        public ConfirmationDialog(string message)
        {
            Title = "Подтверждение";
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            Width = 255;
            Height = 130;
            AllowDrop = false;
            AllowsTransparency = false;
            Background = new SolidColorBrush(Colors.Black);
            WindowStyle = WindowStyle.None;
            WrapPanel mainPanel = new WrapPanel();

            // Создание текстового блока с сообщением
            TextBlock textBlock = new TextBlock();
            textBlock.Text = message;
            textBlock.FontSize = 16;
            textBlock.Foreground = new SolidColorBrush(Colors.White);
            textBlock.TextAlignment = TextAlignment.Center;
            textBlock.TextWrapping = TextWrapping.Wrap;
            textBlock.Margin = new Thickness(10);
            mainPanel.Children.Add(textBlock);

            // Создание панели с кнопками для подтверждения или отмены действия
            WrapPanel buttonPanel = new WrapPanel();
            buttonPanel.Orientation = Orientation.Horizontal;

            Button confirmButton = new Button();
            confirmButton.HorizontalAlignment = HorizontalAlignment.Center;
            confirmButton.VerticalAlignment = VerticalAlignment.Center;
            confirmButton.Height = 30;
            confirmButton.Width = 100;
            confirmButton.Margin = new Thickness(32, 18, 0, 0);
            confirmButton.Background = new SolidColorBrush(Colors.Green);
            confirmButton.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 250, 255));
            confirmButton.FontFamily = new FontFamily("Area");
            confirmButton.FontSize = 15;
            confirmButton.Foreground = new SolidColorBrush(Color.FromRgb(252, 252, 240));
            confirmButton.Content = "Подтвердить";
            confirmButton.Margin = new Thickness(10);
            confirmButton.Click += (sender, e) => { DialogResult = true; };

            Button cancelButton = new Button();
            cancelButton.HorizontalAlignment = HorizontalAlignment.Center;
            cancelButton.VerticalAlignment = VerticalAlignment.Center;
            cancelButton.Height = 30;
            cancelButton.Width = 100;
            cancelButton.Margin = new Thickness(32, 18, 0, 0);
            cancelButton.Background = new SolidColorBrush(Colors.Red);
            cancelButton.BorderBrush = new SolidColorBrush(Color.FromRgb(255, 250, 255));
            cancelButton.FontFamily = new FontFamily("Area");
            cancelButton.FontSize = 15;
            cancelButton.Foreground = new SolidColorBrush(Color.FromRgb(252, 252, 240));
            cancelButton.Margin = new Thickness(10);
            cancelButton.Content = "Отмена";
            cancelButton.Click += (sender, e) => { DialogResult = false; };

            buttonPanel.Children.Add(confirmButton);
            buttonPanel.Children.Add(cancelButton);

            mainPanel.Children.Add(buttonPanel);

            Content = mainPanel;
        }
    }
}
