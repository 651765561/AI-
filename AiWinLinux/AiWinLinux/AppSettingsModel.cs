using System;
using System.Collections.Generic;
using System.Text;

namespace AiWinLinux
{
    public class AppSettingsModel
    {
        public AppSettings AppSettings { get; set; }
    }

    public class AppSettings
    {
        public string DefaultConnection { get; set; }
        public string SubCode { get; set; }
    }

}
