using System.Windows;
using TVQE.Model.Data;
using static TVQE.Model.ShowErrorModel;
using System.Net;
using System.Linq;
using TVQE.Properties;
using System;
using TVQE.Model.Data.Context;
using System.IO;
using System.Collections.Generic;

namespace TVQE;
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
        LoadingVoices();
    }

    private void App_Exit(object sender, ExitEventArgs e)
    {

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

    static void СheckingErrors()
    {
        if (!new EqContext().Database.CanConnect())
            if (ShowError("Не удалось установить подключение к базе данных.") == true)
                Environment.Exit(0);

        if (DataWorker.OfficeScoreboards?.Id == null || DataWorker.OfficeScoreboards.Id == 0)
            if (true == ShowError($"Табло ({string.Join(" ", Dns.GetHostAddresses(Dns.GetHostName()).Where(w => w.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).Select(s => s.ToString()).ToList())}) не зарегистрировано "))
                Environment.Exit(0);
    }

    void LoadingVoices()
    {
        List<SVoice> voices = DataWorker.GetVoices();
        if (voices.Any())
        {
            try
            {
                string voiceFilePath = $"{Path.GetFullPath(Environment.CurrentDirectory)}\\Voice";

                Directory.CreateDirectory(voiceFilePath);

                voices.ForEach(voice =>
                {
                    File.WriteAllBytes($"{voiceFilePath}\\{voice.VoiceName}", voice.File);
                });
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }
    }
}