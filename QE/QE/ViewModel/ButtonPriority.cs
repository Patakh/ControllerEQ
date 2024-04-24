using QE.Models.DTO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace QE.ViewModel
{
    public class ButtonPriority : Button
    {
        public ButtonPriority(ColorDto settingsColor, string buttonName)
        {
            Content = buttonName;
            Margin = new Thickness(0, 18, 32, 0);
            Background = new SolidColorBrush(Colors.LightYellow);
            BorderBrush = new SolidColorBrush(Colors.LightYellow);
            FontFamily = new FontFamily("Area");
            FontSize = 25;
            Foreground = new SolidColorBrush(Colors.Black);
            ControlTemplate myControlTemplate = new ControlTemplate(typeof(Button));
            FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
            border.Name = "border";
            border.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
            border.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
            border.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
            border.SetValue(Border.CornerRadiusProperty, new CornerRadius(20));
            FrameworkElementFactory contentPresenterMenu = new FrameworkElementFactory(typeof(ContentPresenter));
            contentPresenterMenu.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
            contentPresenterMenu.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
            border.AppendChild(contentPresenterMenu);
            myControlTemplate.VisualTree = border;
            Template = myControlTemplate;
        }
    }
}
