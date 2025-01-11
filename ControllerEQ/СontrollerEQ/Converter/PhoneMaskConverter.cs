using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;

namespace ControllerEQ.Views;

public class PhoneMaskConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        string input = value as string;
        if (input != null)
        {
            // Удаление всех нецифровых символов из введенного номера телефона
            string digitsOnly = new string(input.Where(char.IsDigit).ToArray());

            // Применение маски (например, формат "+X (XXX) XXX-XXXX")
            StringBuilder maskedNumber = new StringBuilder();

            int index = 0;
            foreach (char c in "+X (XXX) XXX-XX-XX")
            {
                if (c == 'X') // Заменяем "X" на символ из введенного номера телефона или пустое значение для неполного номера
                {
                    if (index < digitsOnly.Length)
                        maskedNumber.Append(digitsOnly[index]);
                    else
                        break; // Номер телефона имеет меньше цифр, чем маска
                    index++;
                }
                else
                {
                    maskedNumber.Append(c);
                }
            }
            return maskedNumber.ToString();
        }
        return input;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        // Реализация метода ConvertBack не требуется для маски ввода
       return value;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        return this;
    }
}
