using System.Windows;
using System.Windows.Controls;

namespace QE.ViewModel
{
    public class StackPanelKeyboard : StackPanel
    {
        public StackPanelKeyboard()
        {
            Orientation = Orientation.Vertical;
            HorizontalAlignment = HorizontalAlignment.Center;

            WrapPanel wrapPanelLine1 = new WrapPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
            wrapPanelLine1.Children.Add(new ButtonKeyboard("Й"));
            wrapPanelLine1.Children.Add(new ButtonKeyboard("Ц"));
            wrapPanelLine1.Children.Add(new ButtonKeyboard("У"));
            wrapPanelLine1.Children.Add(new ButtonKeyboard("К"));
            wrapPanelLine1.Children.Add(new ButtonKeyboard("Е"));
            wrapPanelLine1.Children.Add(new ButtonKeyboard("Н"));
            wrapPanelLine1.Children.Add(new ButtonKeyboard("Ш"));
            wrapPanelLine1.Children.Add(new ButtonKeyboard("Щ"));
            wrapPanelLine1.Children.Add(new ButtonKeyboard("З"));
            wrapPanelLine1.Children.Add(new ButtonKeyboard("Х"));
            wrapPanelLine1.Children.Add(new ButtonKeyboard("Ъ"));
            wrapPanelLine1.Children.Add(new ButtonKeyboard("Удалить", 75));
            Children.Add(wrapPanelLine1);

            WrapPanel wrapPanelLine2 = new WrapPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
            wrapPanelLine2.Children.Add(new ButtonKeyboard("Ф"));
            wrapPanelLine2.Children.Add(new ButtonKeyboard("Ы"));
            wrapPanelLine2.Children.Add(new ButtonKeyboard("В"));
            wrapPanelLine2.Children.Add(new ButtonKeyboard("А"));
            wrapPanelLine2.Children.Add(new ButtonKeyboard("П"));
            wrapPanelLine2.Children.Add(new ButtonKeyboard("Р"));
            wrapPanelLine2.Children.Add(new ButtonKeyboard("О"));
            wrapPanelLine2.Children.Add(new ButtonKeyboard("Л"));
            wrapPanelLine2.Children.Add(new ButtonKeyboard("Д"));
            wrapPanelLine2.Children.Add(new ButtonKeyboard("Ж"));
            wrapPanelLine2.Children.Add(new ButtonKeyboard("Э"));
            Children.Add(wrapPanelLine2);

            WrapPanel wrapPanelLine3 = new WrapPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
            wrapPanelLine3.Children.Add(new ButtonKeyboard("Я"));
            wrapPanelLine3.Children.Add(new ButtonKeyboard("Ч"));
            wrapPanelLine3.Children.Add(new ButtonKeyboard("С"));
            wrapPanelLine3.Children.Add(new ButtonKeyboard("М"));
            wrapPanelLine3.Children.Add(new ButtonKeyboard("И"));
            wrapPanelLine3.Children.Add(new ButtonKeyboard("Т"));
            wrapPanelLine3.Children.Add(new ButtonKeyboard("Ь"));
            wrapPanelLine3.Children.Add(new ButtonKeyboard("Б"));
            wrapPanelLine3.Children.Add(new ButtonKeyboard("Ю"));
            Children.Add(wrapPanelLine3);

            WrapPanel wrapPanelLine4 = new WrapPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };
            wrapPanelLine4.Children.Add(new ButtonKeyboard("Регистр", 75));
            wrapPanelLine4.Children.Add(new ButtonKeyboard("Пробел", 159));
            wrapPanelLine4.Children.Add(new ButtonKeyboard("Очистить", 80));
            wrapPanelLine4.Children.Add(new ButtonKeyboard("Далее", 150));
            Children.Add(wrapPanelLine4);

        }
    }
}
