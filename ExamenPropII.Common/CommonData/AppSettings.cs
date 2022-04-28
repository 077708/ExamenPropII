using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenPropII.Common.CommonData
{
    public class AppSettings
    {
        public static string ApiUrlCity { get => ConfigurationManager.AppSettings.Get("ApiUrlCity"); }
        public static string ApiUrlHistory { get => ConfigurationManager.AppSettings.Get("ApiUrlHistory"); }
        public static string Token { get => ConfigurationManager.AppSettings.Get("ApiKey"); }
    }
}
