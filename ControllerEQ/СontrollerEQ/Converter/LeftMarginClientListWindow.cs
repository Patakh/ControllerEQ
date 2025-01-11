using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ControllerEQ.Views;       
[ValueConversion(typeof(double), typeof(double))]
public class LeftMarginClientListWindow : MarkupExtension, IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return Application.Current.MainWindow.Left - ((double)value);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    { 
        if (serviceProvider.GetService(typeof(double)) is double width)
        {
            return Application.Current.MainWindow.Left - width;
        }
        return this;
    }
}