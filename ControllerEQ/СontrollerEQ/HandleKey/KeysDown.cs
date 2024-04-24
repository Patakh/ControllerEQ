using System.Windows.Input; 
namespace СontrollerEQ.HandleKey;
public class KeysDown
{
    public static void Press(KeyEventArgs keyEvent)
    {
        switch (keyEvent.Key)
        {
            case Key.F1:
                App.ChangeConnectionStrring();
                break;
            case Key.F2:
                App.Restart();
                break;
        }
    }
}
