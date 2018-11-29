using System.Configuration;

namespace Common
{
    public static class ConfigurationKit
    {
        public static string GetConnectingString(string key)
        {
            return ConfigurationManager.ConnectionStrings[key].ConnectionString;
        }

        public static string HostName
        {
            get { return ConfigurationManager.AppSettings["HostName"]; }
        }

        public static string TcpConnectionString
        {
            get { return ConfigurationManager.AppSettings[HostName + "." + "AgentNameAndPwd"]; }
        }

    }
}
