using log4net;
using Newtonsoft.Json.Linq;
using RAG_Report_V3.Converters;
using RAG_Report_V3.Models;
using RestSharp;

namespace RAG_Report_V3.Stages
{
    internal class TicketHandler
    {
        public void Tickets(List<TicketDataModel> ticketData)
        {
            (List<string> subjects, int tickets) = GetTickets();

            if (tickets < TicketSettingsModel.MaxTickets)
            {
               RaiseTickets(ticketData, subjects);
            }
        }

        private (List<string>, int) GetTickets()
        {
            ILog _logger = LogManager.GetLogger("Log");
            ILog _sqlLogger = LogManager.GetLogger("SQLLog");

            _logger.Info("System - Obtaining Existing RAG Tickets");

            List<string> subjects = new();
            int ticketCount = 0;

            try
            {
                RestClient rest = new(TicketSettingsModel.BaseURL + TicketSettingsModel.Endpoints[0].ToString());
                RestRequest request = new()
                {
                    Method = Method.Get
                };
                rest.AddDefaultHeader("Authorization", TicketSettingsModel.Auth);
                RestResponse response = rest.Execute(request);

                JObject json = JObject.Parse(response.Content);
                JArray tickets = (JArray)json["results"];

                if (tickets.Count > 0)
                {
                    for (int i = 0; i < tickets.Count; i++)
                    {
                        JObject ticket = (JObject)tickets[i];

                        subjects.Add(ticket["subject"].ToString());
                    }
                }

                ticketCount = int.Parse(json["count"].ToString());

                _logger.Debug($"System - Existing Tickets: {ticketCount}");
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running GetTickets in the TicketHandler class.";
                _sqlLogger.Error(ex);
            }

            _logger.Info("System - Obtained Existing RAG Tickets");

            return (subjects, ticketCount);
        }

        private void RaiseTickets(List<TicketDataModel> ticketData, List<string> subjects)
        {
            TicketConverter _ticketConverter = new();

            ILog _logger = LogManager.GetLogger("Log");
            ILog _sqlLogger = LogManager.GetLogger("SQLLog");

            _logger.Info("System - Raising RAG Tickets");

            try
            {
                RestClient rest = new(TicketSettingsModel.BaseURL + TicketSettingsModel.Endpoints[1].ToString());
                RestRequest request = new()
                {
                    Method = Method.Post
                };
                rest.AddDefaultHeader("Authorization", TicketSettingsModel.Auth);

                string ticketPayloadTemplate = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"bin\Ticket Payload.json");
                int createdTickets = 0;
                int ticketLimit = TicketSettingsModel.MaxTickets - subjects.Count;

                foreach (TicketDataModel data in ticketData)
                {
                    if (createdTickets < ticketLimit)
                    {
                        string subDomain = data.Domain.Replace(".briefyourmarket.com", "");

                        _logger.Info($"{subDomain} - Sorting Tickets");

                        bool raise = true;

                        foreach (string subject in subjects)
                        {
                            if (subject.Contains(data.Domain))
                            {
                                raise = false;
                            }
                        }

                        _logger.Debug($"{subDomain} - Raise Ticket: {raise}");

                        if (raise)
                        {
                            string ticketPayload = ticketPayloadTemplate;

                            ticketPayload = ticketPayload.Replace("{0}", data.DefinitionName);
                            ticketPayload = ticketPayload.Replace("{1}", data.LastFeedDate.ToString());
                            ticketPayload = ticketPayload.Replace("{2}", data.RAGStatus);
                            ticketPayload = ticketPayload.Replace("{3}", data.Domain);
                            ticketPayload = ticketPayload.Replace("{4}", _ticketConverter.GetIntegration(data.DefinitionName));

                            request.AddParameter("application/json", ticketPayload, ParameterType.RequestBody);
                            RestResponse response = rest.Execute(request);
                            request.RemoveParameter(request.Parameters.ToList()[0]);

                            _logger.Debug($"{subDomain} - Ticket Raised: {response.StatusCode}");

                            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                            {
                                createdTickets++;
                            }
                        }

                        _logger.Info($"{subDomain} - Ticket Sorted");
                    }
                }
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running RaiseTickets in the TicketHandler class.";
                _sqlLogger.Error(ex);
            }

            _logger.Info("System - Raised RAG Tickets");
        }

        public void RaiseNewFeedTicket(string name)
        {
            ILog _logger = LogManager.GetLogger("Log");
            ILog _sqlLogger = LogManager.GetLogger("SQLLog");

            _logger.Info("System - Raising New Feed Ticket");

            try
            {
                RestClient rest = new(TicketSettingsModel.BaseURL + TicketSettingsModel.Endpoints[1].ToString());
                RestRequest request = new()
                {
                    Method = Method.Post
                };
                rest.AddDefaultHeader("Authorization", TicketSettingsModel.Auth);

                string ticketPayload = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"bin\New Feed Ticket Payload.json");

                ticketPayload = ticketPayload.Replace("{0}", name);

                request.AddParameter("application/json", ticketPayload, ParameterType.RequestBody);
                rest.Execute(request);
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running RaiseNewFeedTicket in the TicketHandler class.";
                _sqlLogger.Error(ex);
            }

            _logger.Info("System - Raised New Feed Ticket");
        }
    }
}
