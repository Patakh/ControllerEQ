using System.Windows;
using System.Windows.Controls;

namespace QE.ViewModel
{
    public class StackPanelKeyboardNumbers : StackPanel
    {
        public StackPanelKeyboardNumbers()
        {
            Orientation = Orientation.Vertical;
            HorizontalAlignment = HorizontalAlignment.Center;

            WrapPanel wrapPanelLine1 = new WrapPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
            wrapPanelLine1.Children.Add(new ButtonKeyboardNumber("1"));
            wrapPanelLine1.Children.Add(new ButtonKeyboardNumber("2"));
            wrapPanelLine1.Children.Add(new ButtonKeyboardNumber("3"));
            Children.Add(wrapPanelLine1);

            WrapPanel wrapPanelLine2 = new WrapPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
            wrapPanelLine2.Children.Add(new ButtonKeyboardNumber("4"));
            wrapPanelLine2.Children.Add(new ButtonKeyboardNumber("5"));
            wrapPanelLine2.Children.Add(new ButtonKeyboardNumber("6"));
            Children.Add(wrapPanelLine2);

            WrapPanel wrapPanelLine3 = new WrapPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
            wrapPanelLine3.Children.Add(new ButtonKeyboardNumber("7"));
            wrapPanelLine3.Children.Add(new ButtonKeyboardNumber("8"));
            wrapPanelLine3.Children.Add(new ButtonKeyboardNumber("9"));
            Children.Add(wrapPanelLine3);

            WrapPanel wrapPanelLine4 = new WrapPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
            wrapPanelLine4.Children.Add(new ButtonKeyboardNumber("0"));
            wrapPanelLine4.Children.Add(new ButtonKeyboardNumber("Удалить", 128));
            Children.Add(wrapPanelLine4);
        }
    }
}
