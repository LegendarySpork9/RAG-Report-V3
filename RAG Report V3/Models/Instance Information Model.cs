namespace RAG_Report_V3.Models
{
    internal class Instance_Information_Model
    {
        // Stores specific details about the instance.
        public DateOnly LastIntegrationDate { get; set; } = DateOnly.Parse("1900-01-01");
        public DateOnly LastFeedReceived { get; set; } = DateOnly.Parse("1900-01-01");
        public int RAGStatusID { get; set; }
    }
}
