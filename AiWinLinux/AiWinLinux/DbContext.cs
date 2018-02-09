using System;
using System.Data;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using MySql.Data;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace AiWinLinux
{
    public class DbContext
    {


        public static string DefaultConnectionString => ConnectionString().AppSettings.DefaultConnection;
        public static string SubCode => ConnectionString().AppSettings.SubCode;

        public static AppSettingsModel ConnectionString()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile(path);
            var item = builder.Build().Get<AppSettingsModel>();
            return item;

        }

        public static MySqlConnection Open()
        {
            var cs = DefaultConnectionString;
           
            var connection = new MySqlConnection(cs);
           if(connection.State==ConnectionState.Closed)
            {
                try
                {
                    connection.Open();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    connection.Open();
                }
                
            }
         
            return connection;
        }
   
    }
}
