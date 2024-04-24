using System.Windows;
using System.Windows.Controls;

namespace QE.ViewModel
{
    public class StackPanelKeyboardRegistrationPreRecord : StackPanel
    {
        public StackPanelKeyboardRegistrationPreRecord()
        {
            Orientation = Orientation.Vertical;
            HorizontalAlignment = HorizontalAlignment.Center;

            WrapPanel wrapPanelLine1 = new WrapPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
            wrapPanelLine1.Children.Add(new ButtonKeyboardRegistrationPreRecord("1"));
            wrapPanelLine1.Children.Add(new ButtonKeyboardRegistrationPreRecord("2"));
            wrapPanelLine1.Children.Add(new ButtonKeyboardRegistrationPreRecord("3"));
            Children.Add(wrapPanelLine1);

            WrapPanel wrapPanelLine2 = new WrapPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
            wrapPanelLine2.Children.Add(new ButtonKeyboardRegistrationPreRecord("4"));
            wrapPanelLine2.Children.Add(new ButtonKeyboardRegistrationPreRecord("5"));
            wrapPanelLine2.Children.Add(new ButtonKeyboardRegistrationPreRecord("6"));
            Children.Add(wrapPanelLine2);

            WrapPanel wrapPanelLine3 = new WrapPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
            wrapPanelLine3.Children.Add(new ButtonKeyboardRegistrationPreRecord("7"));
            wrapPanelLine3.Children.Add(new ButtonKeyboardRegistrationPreRecord("8"));
            wrapPanelLine3.Children.Add(new ButtonKeyboardRegistrationPreRecord("9"));
            Children.Add(wrapPanelLine3);

            WrapPanel wrapPanelLine4 = new WrapPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
            wrapPanelLine4.Children.Add(new ButtonKeyboardRegistrationPreRecord("Удалить"));
            wrapPanelLine4.Children.Add(new ButtonKeyboardRegistrationPreRecord("0"));
            wrapPanelLine4.Children.Add(new ButtonKeyboardRegistrationPreRecord("Ввод"));

            Children.Add(wrapPanelLine4);
        }
    }
}
