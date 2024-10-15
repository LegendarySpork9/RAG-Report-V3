namespace RAG_Report_V3.Models
{
    internal class Integration_Model
    {
        // Stores the integration information.
        public List<string> Name { get; set; } = new List<string>();
        public List<string> Type { get; set; } = new List<string>();
        public List<string> UniqueFields { get; set; } = new List<string>();
        public List<DateTime> SetUpDate { get; set; } = new List<DateTime>();
        public List<bool> HasRun { get; set; } = new List<bool>();
    }
}
