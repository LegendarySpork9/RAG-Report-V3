using System.ComponentModel;
using System.Configuration;

namespace RAG_Report_V3.Models
{
    internal static class TicketSettingsModel
    {
        // Holds the values for ticket creation.
        public static bool RaiseTickets { get; set; } = bool.Parse(ConfigurationManager.AppSettings["RaiseTickets"].ToString());
        public static int MaxTickets { get; set; } = int.Parse(ConfigurationManager.AppSettings["MaxTickets"].ToString());
        public static string[] StatusFilter { get; set; } = ConfigurationManager.AppSettings["StatusFilter"].ToString().Split(',');
        public static string[] DaysFilter { get; set; } = ConfigurationManager.AppSettings["ExclDaysFilter"].ToString().Split(',');
        public static string BaseURL { get; set; } = ConfigurationManager.AppSettings["BaseURL"].ToString();
        public static string Auth { get; set; } = ConfigurationManager.AppSettings["Auth"].ToString();
        public static string[] Endpoints { get; set; } = ConfigurationManager.AppSettings["Endpoints"].ToString().Split(',');
    }
}
