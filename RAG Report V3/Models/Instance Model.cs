namespace RAG_Report_V3.Models
{
    internal class Instance_Model
    {
        // Stores the instance information.
        public int InstanceId { get; set; }
        public string? URL { get; set; }
        public string? SubDomain { get; set; }
        public string? Status { get; set; }
        public int ActiveContactTriggers { get; set; }
        public int ActivePropertyTriggers { get; set; }
        public string? Server { get; set; }
        public string? Database { get; set; }
        public bool MultiplePSIntegrations { get; set; } = false;
        public Integration_Model? Integrations { get; set; }
        public Property_Feed_Model? PropertyFeed { get; set; }
        public Exclusion_Model? Exclusions { get; set; }
        public List<Instance_Information_Model> InstanceInformation { get; set; } = new List<Instance_Information_Model>();
        public List<Instance_Data_Model> InstanceData { get; set; } = new List<Instance_Data_Model>();
        public List<Results_Model> Results { get; set; } = new List<Results_Model>();
    }
}
