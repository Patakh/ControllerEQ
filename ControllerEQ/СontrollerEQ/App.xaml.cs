 using System.Data;
using System.Linq;
using System.Windows;
using СontrollerEQ.Model.Data.Context;
using СontrollerEQ.Model.Data;
using static СontrollerEQ.Model.ShowErrorModel;
using System.Net;
using СontrollerEQ.Views; 
using СontrollerEQ.Properties;
using System;

namespace СontrollerEQ
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = ShowError(e.Exception?.InnerException?.Message ?? e.Exception.Message).Value;
        }

        public App()
        {
            СheckingErrors();
        }

        public static void Restart()
        {
            string fileName = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            System.Diagnostics.Process.Start(fileName);
            Environment.Exit(0);
        }

        public static void ChangeConnectionStrring()
        {
            var settingWindow = new SettingWindow(Settings.Default.connection);
            if (settingWindow.ShowDialog() == true)
            {
                Settings.Default.connection = settingWindow.Connection;
                Settings.Default.Save();
            }
        }
         
        static void СheckingErrors()
        {
            if (!new EqContext().Database.CanConnect())
              ShowError("Не удалось установить подключение к базе данных.");

            if (DataWorker.Window?.Id == null || DataWorker.Window.Id == 0)
                ShowError($"Окно ({string.Join(" ", Dns.GetHostAddresses(Dns.GetHostName()).Where(w => w.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(s => s.ToString()).ToList())}) не зарегистрировано ");
        }
    }
}
