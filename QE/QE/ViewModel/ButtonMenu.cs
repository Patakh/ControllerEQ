using QE.Models.DTO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace QE.ViewModel;

/// <summary>
/// Красная кнопка меню
/// </summary>
public class ButtonMenu : Button
{
    public ButtonMenu(ButtonDto button, ColorDto settingsColor)
    {
        //DropShadowEffect shadowEffect = new DropShadowEffect();
        //shadowEffect.Color = Colors.White;
        //shadowEffect.ShadowDepth = 3;
        //Effect = shadowEffect;
        //HorizontalAlignment = HorizontalAlignment.Center;
        //VerticalAlignment = VerticalAlignment.Top;
        //Height = 75;
        //Width = 200;
        //TabIndex = 999;
        Content = button.Name;
        Margin = new Thickness(0, 18, 32, 0);
        Background = new SolidColorBrush(settingsColor.ColorBtnMenu);
        BorderBrush = new SolidColorBrush(settingsColor.ColorBtnMenu);
        FontFamily = new FontFamily("Area");
        FontSize = 25;
        Foreground = new SolidColorBrush(settingsColor.ColorBtnTextMenu);
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
        //MaxWidth = 600;
        //MaxHeight = 400;
    }
}
