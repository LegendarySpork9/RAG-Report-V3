using log4net;
using RAG_Report_V3.Models;

namespace RAG_Report_V3.Stages
{
    internal class Decide_Result
    {
        static readonly ILog _logger = LogManager.GetLogger("Log");

        // Uses the data to decide the result for the Integration.
        public string GetResultIntegration(string integration, int index, Instance_Model instance)
        {
            _logger.Info($"{instance.SubDomain} - Deciding result for: {integration}");

            Instance_Data_Model instanceData = instance.InstanceData[index];
            Results_Model result = new();

            int noData = 0;
            int noNewdata = 0;

            for (int x = 0; x < instanceData.DataEndpoint.Count; x++)
            {
                if (instanceData.Created[x] == 0 && instanceData.Modified[x] == 0)
                {
                    noData++;
                }

                if (instanceData.Created[x] == 0 && instanceData.Modified[x] > 0)
                {
                    noNewdata++;
                }
            }

            if (noNewdata == instanceData.DataEndpoint.Count && noData != instanceData.DataEndpoint.Count)
            {
                _logger.Debug($"{instance.SubDomain} - No new data found");

                result.NoNewData = true;
            }

            if (noData == instanceData.DataEndpoint.Count)
            {
                _logger.Debug($"{instance.SubDomain} - No data found");

                _logger.Debug($"{instance.SubDomain} - Integration has run: {instance.Integrations.HasRun[index]}");

                if (instance.Integrations.HasRun[index])
                {
                    result.ContactIssue = true;

                    _logger.Warn($"{instance.SubDomain} - There is an issue with the integration.");
                }

                else
                {
                    _logger.Debug($"{instance.SubDomain} - There is no issue with the integration.");
                }

                instance.Results.Add(result);

                _logger.Info($"{instance.SubDomain} - Result decided for: {integration}");

                return "Success";
            }

            instance.Results.Add(result);

            _logger.Debug($"{instance.SubDomain} - There is no issue with the integration.");

            _logger.Info($"{instance.SubDomain} - Result decided for: {integration}");

            return "Success";
        }

        // Uses the data to decide the result for the Property Feed.
        public string GetResultProperty(Instance_Model instance)
        {
            string name = instance.PropertyFeed.Name;

            _logger.Info($"{instance.SubDomain} - Deciding result for: {name}");

            Instance_Data_Model instanceData = instance.InstanceData.Last();
            Results_Model result = new();

            int noData = 0;
            int noNewdata = 0;

            for (int x = 0; x < instanceData.DataEndpoint.Count; x++)
            {
                if (instanceData.Created[x] == 0 && instanceData.Modified[x] == 0)
                {
                    noData++;
                }

                if (instanceData.Created[x] == 0 && instanceData.Modified[x] > 0)
                {
                    noNewdata++;
                }
            }

            if (noNewdata == instanceData.DataEndpoint.Count && noData != instanceData.DataEndpoint.Count)
            {
                _logger.Debug($"{instance.SubDomain} - No new data found");

                result.NoNewData = true;
            }

            if (noData == instanceData.DataEndpoint.Count)
            {
                _logger.Debug($"{instance.SubDomain} - No data found");

                _logger.Debug($"{instance.SubDomain} - Integration has run: {instance.PropertyFeed.HasRun}");

                if (instance.PropertyFeed.HasRun)
                {
                    result.PropertyIssue = true;

                    _logger.Warn($"{instance.SubDomain} - There is an issue with the property feed.");
                }

                else
                {
                    _logger.Debug($"{instance.SubDomain} - There is no issue with the property feed.");
                }

                instance.Results.Add(result);

                _logger.Info($"{instance.SubDomain} - Result decided for: {name}");

                return "Success";
            }

            instance.Results.Add(result);

            _logger.Debug($"{instance.SubDomain} - There is no issue with the property feed.");

            _logger.Info($"{instance.SubDomain} - Result decided for: {name}");

            return "Success";
        }
    }
}
