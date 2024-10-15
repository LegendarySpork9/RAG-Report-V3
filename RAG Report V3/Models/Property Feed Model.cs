namespace RAG_Report_V3.Models
{
    internal class Property_Feed_Model
    {
        // Stores the property feed information.
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public DateTime SetUpDate { get; set; }
        public bool HasRun { get; set; }
    }
}
