using System.Windows.Input;
namespace TVQE.HandleKey;
public class KeysDown
{
    public static void Press(KeyEventArgs keyEvent)
    {
        switch (keyEvent.Key)
        {
            case Key.F1:
                App.ChangeConnectionString();
                break;
            case Key.F2:
                App.Restart();
                break;
        }
    }
}
