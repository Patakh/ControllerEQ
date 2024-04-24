using QE.Models.DTO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace QE.ViewModel;

public class ButtonAction : Button
{
    public ButtonAction(ColorDto settingsColor, string buttonName)
    {

        //HorizontalAlignment = HorizontalAlignment.Center;
        //VerticalAlignment = VerticalAlignment.Top;
        //Height = 75;
        //Width = 200;
        //DropShadowEffect shadowEffect = new DropShadowEffect();
        //shadowEffect.Color = Color.FromRgb(22, 22, 22);
        //shadowEffect.Direction = 315;
        //shadowEffect.ShadowDepth = 3;
        //Effect = shadowEffect;
        Content = buttonName;
        Margin = new Thickness(0, 18, 32, 0);
        Background = new SolidColorBrush(settingsColor.ColorBtnAction);
        BorderBrush = new SolidColorBrush(settingsColor.ColorBtnAction);
        FontFamily = new FontFamily("Area");
        FontSize = 25;
        Foreground = new SolidColorBrush(settingsColor.ColorBtnTextAction);
        ControlTemplate myControlTemplate = new ControlTemplate(typeof(Button));
        FrameworkElementFactory border = new FrameworkElementFactory(typeof(Border));
        border.Name = "border";
        border.SetValue(Border.BackgroundProperty, new TemplateBindingExtension(Border.BackgroundProperty));
        border.SetValue(Border.BorderBrushProperty, new TemplateBindingExtension(Border.BorderBrushProperty));
        border.SetValue(Border.BorderThicknessProperty, new TemplateBindingExtension(Border.BorderThicknessProperty));
        border.SetValue(Border.CornerRadiusProperty, new CornerRadius(20));
        FrameworkElementFactory contentPresenter = new FrameworkElementFactory(typeof(ContentPresenter));
        contentPresenter.SetValue(HorizontalAlignmentProperty, HorizontalAlignment.Center);
        contentPresenter.SetValue(VerticalAlignmentProperty, VerticalAlignment.Center);
        border.AppendChild(contentPresenter);
        myControlTemplate.VisualTree = border;
        Template = myControlTemplate;
        //MaxHeight = 400;
        //MaxWidth = 600;
    }
}

