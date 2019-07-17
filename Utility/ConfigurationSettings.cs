using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SecretBook.Helper
{
    public static class ConfigurationSettings
    {
        public static bool IsProductionEnv => Convert.ToBoolean(ConfigurationManager.AppSettings.Get("ProdEnv"));
        public static string GetEndPointUrl => ConfigurationManager.AppSettings.Get("Url");
        public static List<string> GetAllowedDomains => ConfigurationManager.AppSettings.Get("AllowDomains").Split(',').Select(x => x.Trim()).ToList();
    }
}
