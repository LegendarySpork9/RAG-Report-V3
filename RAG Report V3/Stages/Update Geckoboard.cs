using log4net;
using Newtonsoft.Json.Linq;
using RAG_Report_V3.Models;
using RestSharp;

namespace RAG_Report_V3.Stages
{
    internal class UpdateGeckoboard
    {
        public void UpdateRAGNumbers(RAGNumbers rag)
        {
            ILog _logger = LogManager.GetLogger("Log");
            ILog _sqlLogger = LogManager.GetLogger("SQLLog");

            _logger.Info("System - Updating Geckoboard Numbers");

            try
            {
                RestClient rest = new(@"https://push.geckoboard.com/v1/send/bd7b54b8-a1e4-478a-9b82-4a019033b85d");
                RestRequest request = new()
                {
                    Method = Method.Post
                };

                JObject json = JObject.Parse(File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"bin\RAG.json"));
                JArray series = (JArray)json["data"]["series"];

                if (series.Count > 0)
                {
                    JObject seriesObject = (JObject)series[0];
                    seriesObject["data"] = new JArray(rag.DarkRed, rag.Red, rag.Yellow, rag.Green, rag.DarkViolet);
                }

                _logger.Debug($"System - Post JSON: {json}");

                request.AddParameter("application/json", json.ToString(), ParameterType.RequestBody);
                RestResponse response = rest.Execute(request);
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running UpdateRAGNumbers in the UpdateGeckoboard class.";
                _sqlLogger.Error(ex);
            }

            _logger.Info("System - Updated Geckoboard Numbers");
        }
    }
}
