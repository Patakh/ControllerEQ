using ControllerEQ.Model.Data;
using ControllerEQ.Model.Data.Context;
using ControllerEQ.Properties;
using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Windows;
using static ControllerEQ.Model.ShowErrorModel;

namespace ControllerEQ;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
    {
        e.Handled = ShowError(e.Exception?.InnerException?.Message ?? e.Exception.Message)!.Value;
    }

    public App()
    {
        CheckingErrors();
    }

    public static void Restart()
    {
        string fileName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
        System.Diagnostics.Process.Start(fileName);
        Environment.Exit(0);
    }

    public static void ChangeConnectionString()
    {
        var settingWindow = new SettingWindow(Settings.Default.connection);
        if (settingWindow.ShowDialog() == true)
        {
            Settings.Default.connection = settingWindow.Connection;
            Settings.Default.Save();
        }
    }

    static void CheckingErrors()
    {
        if (!new EqContext().Database.CanConnect())
            ShowError("Не удалось установить подключение к базе данных.");

        if (DataWorker.Window?.Id == null || DataWorker.Window.Id == 0)
            ShowError($"Окно ({string.Join(" ", Dns.GetHostAddresses(Dns.GetHostName()).Where(w => w.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(s => s.ToString()).ToList())}) не зарегистрировано ");
    }
}
