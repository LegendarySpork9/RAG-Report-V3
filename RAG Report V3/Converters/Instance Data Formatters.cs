using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAG_Report_V3.Converters
{
    internal class Instance_Data_Formatters
    {
        // Formats the required fields for the SQL query.
        public (string?, string?) FormatUniqueFields(string uniqueFields)
        {
            if (!string.IsNullOrWhiteSpace(uniqueFields))
            {
                string? contactField = null;
                string[] relatedObjects = Array.Empty<string>();
                string roClause = string.Empty;

                if (uniqueFields.Contains('^'))
                {
                    contactField = uniqueFields[..uniqueFields.IndexOf("^")];
                    uniqueFields = uniqueFields.Remove(0, uniqueFields.IndexOf("^") + 1);
                }

                relatedObjects = uniqueFields.Split(",");

                foreach (string relatedObject in relatedObjects)
                {
                    roClause += $"'{relatedObject}',\n";
                }

                roClause = roClause.Remove(roClause.LastIndexOf(",\n"));

                return (contactField, roClause);
            }

            return (null, null);
        }
    }
}
