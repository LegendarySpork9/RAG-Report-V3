using System.Configuration;

namespace RAG_Report_V3.Models
{
    internal static class App_Settings_Model
    {
        // Holds the values from the appSettings tag.
        public static string InputConnectionString = ConfigurationManager.AppSettings["InputConnectionString"];
        public static string OutputConnectionString = ConfigurationManager.AppSettings["OutputConnectionString"];
        public static string SQLScripts = ConfigurationManager.AppSettings["SQLScriptsLocation"];
        public static string ExcludeInstances = ConfigurationManager.AppSettings["ExcludeInstances"].ToString();
        public static string ExcludeIntegrations = ConfigurationManager.AppSettings["ExcludeIntsLocation"];
        public static int MaxThreads = int.Parse(ConfigurationManager.AppSettings["MaxThreads"].ToString());
    }
}
