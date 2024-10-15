namespace RAG_Report_V3.Models
{
    internal class Instance_Data_Model
    {
        // Stores the data from the instance.
        public List<string> DataEndpoint { get; set; } = new List<string>();
        public List<int> Created { get; set; } = new List<int>();
        public List<int> Modified { get; set; } = new List<int>();
    }
}
