using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG_Report_V3.Models
{
    internal class Output_Integrations_Model
    {
        // Stores important information from the Integration table.
        public List<int> IntegrationId { get; set; } = new List<int>();
        public List<string> Name { get; set; } = new List<string>();
        public List<string> Type { get; set; } = new List<string>();
        public List<string> UniqueField { get; set; } = new List<string>();
        public List<string> Source { get; set; } = new List<string>();
    }
}
