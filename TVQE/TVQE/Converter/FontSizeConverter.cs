

using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace TVQE;
public class FontSizeConverter : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    { 
        if ((double)value != 0 && double.TryParse(value.ToString(), out double screenFactor) && double.TryParse(parameter.ToString(), out double fontSize))
        {
            // Умножаем размер шрифта на коэффициент, зависящий от размера экрана
            return (screenFactor + SystemParameters.PrimaryScreenWidth) / 3000 * fontSize;
        }
        return parameter;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
    public override object ProvideValue(IServiceProvider serviceProvider)
    { 
        return this;
    }
}