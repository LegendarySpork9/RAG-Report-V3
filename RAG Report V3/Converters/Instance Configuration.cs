using log4net;
using RAG_Report_V3.Models;
using System.Data.SqlClient;

namespace RAG_Report_V3.Converters
{
    internal class Instance_Configuration
    {
        // Converts the instance status number to the text equivalent.
        public string GetInstanceStatus(int status)
        {
            return status switch
            {
                0 => "New",
                1 => "QA",
                2 => "Live",
                3 => "Suspended",
                4 => "Demo",
                5 => "Pending",
                6 => "Closed",
                7 => "SuspendedRestrictAccess",
                8 => "Trial",
                _ => $"Unknown - {status}"
            };
        }

        // Converts the integration name to the integration type.
        public string GetIntegrationType(string name, Output_Integrations_Model integrations)
        {
            int index = integrations.Name.IndexOf(name);
            return integrations.Type[index];
        }

        // Converts the database source to the formatted equivalent.
        public string GetInstanceSource(string type)
        {
            return type switch
            {
                "reapitfoundations" => "Reapit Foundations",
                "smeprofessional" => "Sme Professional",
                "jupix" => "Jupix",
                "vtukopenview" => "Vtuk Openview",
                "dezrezrezi" => "DezRez Rezi",
                "propertyfiledrop" => "Property File Drop",
                "mrisoftware" => "Mri Software",
                "apex27" => "Apex27",
                "propertybase" => "PropertyBase",
                "mappedfiledrop" => "Mapped File Drop",
                "rexsoftware" => "Rex Software",
                "street" => "Street",
                "custom" => "Custom",
                _ => "Custom"
            };
        }

        // Converts the database source to the formatted equivalent.
        public string GetPSSource(string type)
        {
            return type switch
            {
                "Reapit Foundations" => "reapitfoundations",
                "Sme Professional" => "smeprofessional",
                "Jupix" => "jupix",
                "Vtuk Openview" => "vtukopenview",
                "DezRez Rezi" => "dezrezrezi",
                "Property File Drop" => "propertyfiledrop",
                "Mri Software" => "mrisoftware",
                "Apex27" => "apex27",
                "PropertyBase" => "propertybase",
                "Mapped File Drop" => "mappedfiledrop",
                "Rex Software" => "rexsoftware",
                "Street" => "street",
                "Custom" => "custom",
                _ => "custom"
            };
        }
    }
}
