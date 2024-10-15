﻿using log4net;
using RAG_Report_V3.Models;

namespace RAG_Report_V3.Converters
{
    internal class RAG_Status
    {
        static readonly ILog _logger = LogManager.GetLogger("Log");

        // Returns the status number of the RAG Status.
        public void GetRAGStatus(bool isExcluded, int? index, Instance_Model instance, string? integration)
        {
            Instance_Information_Model instanceInformation = new();
            Results_Model result = new();

            if (index.HasValue)
            {
                instanceInformation = instance.InstanceInformation[index.Value];
                result = instance.Results[index.Value];
            }

            if (isExcluded)
            {
                instanceInformation.RAGStatusID = 5;
                instance.InstanceInformation.Add(instanceInformation);
                _logger.Debug($"{instance.SubDomain} - {integration} Rag Status: Dark Violet");
                return;
            }

            if (result.NoNewData)
            {
                instanceInformation.RAGStatusID = 3;
                _logger.Debug($"{instance.SubDomain} - Rag Status: Yellow");
                return;
            }

            if ((result.ContactIssue && instance.ActiveTriggers > 0) || (result.PropertyIssue && instance.ActiveTriggers > 0))
            {
                instanceInformation.RAGStatusID = 1;
                _logger.Debug($"{instance.SubDomain} - Rag Status: Dark Red");
                return;
            }

            if (result.ContactIssue || result.PropertyIssue)
            {
                instanceInformation.RAGStatusID = 2;
                _logger.Debug($"{instance.SubDomain} - Rag Status: Red");
                return;
            }

            instanceInformation.RAGStatusID = 4;
            _logger.Debug($"{instance.SubDomain} - Rag Status: Green");
            return;
        }
    }
}
