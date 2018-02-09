using System.Configuration;

namespace AiWin
{
    public class Config
    {
        public static string DefaultConnectionString => GetConnectionString("ConnectionString").ConnectionString;

        public static string GetAppSetting(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }

        public static ConnectionStringSettings GetConnectionString(string key)
        {
            return ConfigurationManager.ConnectionStrings[key];
        }

    }
}
