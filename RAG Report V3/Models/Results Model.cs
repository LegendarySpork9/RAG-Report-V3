namespace RAG_Report_V3.Models
{
    internal class Results_Model
    {
        // Stores the result of the checks.
        public bool ContactIssue { get; set; } = false;
        public bool NoNewData { get; set; } = false;
        public bool PropertyIssue { get; set; } = false;
    }
}
