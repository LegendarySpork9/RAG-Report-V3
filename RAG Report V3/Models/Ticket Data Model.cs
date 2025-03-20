namespace RAG_Report_V3.Models
{
    internal class TicketDataModel
    {
        public string Domain { get; set; }
        public string DefinitionName { get; set; }
        public DateOnly LastFeedDate { get; set; }
        public string RAGStatus { get; set; }
    }
}
