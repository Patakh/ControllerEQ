namespace TVQE.Model;
static public class Settings
{
    private static string _connectionString;
    public static string ConnectionString
    {
        get
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                _connectionString = TVQE.Properties.Settings.Default.connection;
            }
            return _connectionString;
        }
        set
        {
            _connectionString = value;
        }
    }
}
