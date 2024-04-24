using System;
using System.Collections.Generic;

namespace TVQE.Model;
public class CallTickets
{
    public List<string> AudioFiles { get; set; }
    public string Prefix { get; set; }
    public string Number { get; set; }
    public string WindowName { get; set; }
    public CallTickets(string prefix, string number, string windowName)
    {
        Prefix = prefix;
        Number = number;
        WindowName = windowName;
        AudioFiles = new List<string>(new List<string>
                {
                    $"ВНИМАНИЕ",
                    $"КЛИЕНТА",
                    $"НОМЕР" ,
                    $"{prefix}"
                });

        foreach (var audioFile in BuildNumberVoice(Number))
            AudioFiles.Add(audioFile);

        AudioFiles.Add($"ПРИГЛАШАЮТПРОЙТИ");
        AudioFiles.Add($"КОКНУ");
        AudioFiles.Add($"НОМЕР");

        foreach (var audioFile in BuildNumberVoice(WindowName.Split("№")[1]))
            AudioFiles.Add(audioFile);
    }

    string[] BuildNumberVoice(string number)
    {
        return Convert.ToInt32(number) switch
        {
            int n when n <= 20 || n % 10 == 0 && n < 100 || n % 100 == 0 =>
            new string[] { number },
            int n when n < 100 || n % 100 <= 20 || n % 10 == 0 =>
            new string[]{
                    ( (n < 100 ? $"{number[0]}0" : $"{number[0]}00")),
                    ( (n < 100 ? $"{number[1]}" : n % 100 <= 20 ? n % 100 < 10 ? $"{number[2]}" : $"{number[1]}{number[2]}" : $"{number[1]}0"))
            },
            _ => new string[]{
                    $"{number[0]}00",
                    $"{number[1]}0",
                    $"{number[2]}"
                }
        };
    }
}
