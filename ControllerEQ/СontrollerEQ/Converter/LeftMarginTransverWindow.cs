using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace СontrollerEQ.Views;       
[ValueConversion(typeof(double), typeof(double))]
public class LeftMarginTransverWindow : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is double width)
        {
            return SystemParameters.PrimaryScreenWidth - ((double)value + 313);
        }
        return DependencyProperty.UnsetValue;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    { 
        if (serviceProvider.GetService(typeof(double)) is double width)
        {
            return SystemParameters.PrimaryScreenWidth - width;
        }
        return this;
    }
}