using System.Windows.Media;
namespace TVQE.Extensions;
public static class SolidColorBrushExtensions
{
    public static SolidColorBrush ColorConvert(this Brush solid, string colorString)
    {
        BrushConverter converter = new();
        bool canConvert = converter.CanConvertFrom(null, typeof(string)) && converter.IsValid(colorString);
        return canConvert ? (SolidColorBrush)converter.ConvertFromString(colorString ?? solid.ToString()) : (SolidColorBrush)solid;
    }
}
