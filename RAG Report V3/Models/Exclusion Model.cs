namespace RAG_Report_V3.Models
{
    internal class Exclusion_Model
    {
        // Stores the exclusion information.
        public List<string> Type { get; set; } = new List<string>();
        public List<string> Name { get; set; } = new List<string>();
        public List<DateOnly> ExcludeTillDate { get; set; } = new List<DateOnly>();
        public List<string> ExcludeReason { get; set; } = new List<string>();
        public List<string> Comments { get; set; } = new List<string>();
    }
}
