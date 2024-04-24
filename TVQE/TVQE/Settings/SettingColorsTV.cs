using System.Windows.Media;
using TVQE.Extensions;
using TVQE.Model;
using TVQE.Model.Data;
namespace TVQE.SettingsColor;
public static class SettingColorsTV
{
    static SettingColorsTV()
    {
        TitleForeground = new SolidColorBrush(Colors.Brown);
        RunningTextBackground  = new SolidColorBrush(Colors.Brown);
        RunningTextForeground = new SolidColorBrush(Colors.White);
        TicketsForeground = new SolidColorBrush(Colors.Black);
         
        ColorsTV colors = DataWorker.GetColorsTV();
        
        TitleForeground = TitleForeground.ColorConvert(colors.TitleForeground);
        RunningTextBackground = RunningTextBackground.ColorConvert(colors.RunningTextBackground);
        RunningTextForeground = RunningTextForeground.ColorConvert(colors.RunningTextForeground);
        TicketsForeground = TicketsForeground.ColorConvert(colors.TicketsForeground);
    }

    public static SolidColorBrush TitleForeground;
    public static SolidColorBrush RunningTextBackground;
    public static SolidColorBrush RunningTextForeground;
    public static SolidColorBrush TicketsForeground;
}