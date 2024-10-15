using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG_Report_V3.Models
{
    internal class Output_Property_Feed_Model
    {
        // Stores important information from the PropertyFeed table.
        public List<int> PropertyId { get; set; } = new List<int>();
        public List<string> Name { get; set; } = new List<string>();
        public List<string> Type { get; set; } = new List<string>();
    }
}
