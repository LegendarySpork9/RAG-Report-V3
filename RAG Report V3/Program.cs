using log4net;
using RAG_Report_V3.Models;
using RAG_Report_V3.Stages;
using RAG_Report_V3.Converters;

namespace RAG_Report_V3
{
    internal class Program
    {
        static readonly ILog _logger = LogManager.GetLogger("Log");

        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
            _logger.Info("System - Logging Started");

            _logger.Info($"Input Source: {App_Settings_Model.InputConnectionString}");
            _logger.Info($"Output Source: {App_Settings_Model.OutputConnectionString}");
            _logger.Info($"SQL Queries: {App_Settings_Model.SQLScripts}");
            _logger.Info($"Exclude Instances: {App_Settings_Model.ExcludeInstances}");
            _logger.Info($"Exclude Integrations: {App_Settings_Model.ExcludeIntegrations}");
            _logger.Info($"Raise Tickets: {TicketSettingsModel.RaiseTickets}");
            _logger.Info($"Max Tickets: {TicketSettingsModel.MaxTickets}");
            _logger.Info($"Include Status Filters: {TicketSettingsModel.StatusFilter.Length}");
            _logger.Info($"Exclude Date Filter: {TicketSettingsModel.DaysFilter.Length}");
            _logger.Info($"Base URL: {TicketSettingsModel.BaseURL}");
            _logger.Info($"Auth: {TicketSettingsModel.Auth}");
            _logger.Info($"Endpoints: {TicketSettingsModel.Endpoints.Length}");

            Instance_Delete _instanceDelete = new();
            Instance_Config _instanceConfig = new();
            Compare_Exclusion _compareExclusion = new();
            Instance_Data _instanceData = new();
            Decide_Result _decideResult = new();
            RAG_Status _ragStatus = new();
            Output_Data _outputData = new();
            UpdateGeckoboard _updateGeckoboard = new();
            TicketHandler _ticketHandler = new();

            List<Instance_Model> instances = new();
            List<TicketDataModel> ticketData = new();

            Output_Integrations_Model outputIntegrations = _instanceConfig.GetOutputIntegrations();
            Output_Property_Feed_Model outputPropertyfeeds = _instanceConfig.GetOutputPropertyFeeds();
            RAGNumbers ragNumbers = new();

            bool raiseTickets = false;

            Console.WriteLine("\nPlease choose which action to perform. -Full will refresh the data for every instance. -Refresh {ID} will refresh the data for the given instance. -RaiseTickets will raise the RAG tickets.");
            Console.Write("Action: ");

            if (args.Length != 0 && args[0] == "-Full")
            {
                _logger.Info("System - Running a full refresh of all tables.");

                if (_instanceDelete.DeleteInstances() != "Success")
                {
                    Console.Write("An error occured while updating the status of the instances.");
                    _logger.Error("System - An error occured while updating the status of the instances.");
                    _logger.Info("System - Logging Stopped");
                    Environment.Exit(0);
                }

                instances = _instanceConfig.GetInstanceDetails(null);
                raiseTickets = true;
            }

            else
            {
                string? action = Console.ReadLine();

                while (string.IsNullOrWhiteSpace(action) || !action.Contains("-Refresh ") && action != "-Full" && action != "-RaiseTickets")
                {
                    Console.Write("That command is not recognised please enter one of the valid input commands. Action: ");
                    action = Console.ReadLine();
                }

                if (action == "-Full")
                {
                    _logger.Info("System - Running a full refresh of all tables.");

                    if (_instanceDelete.DeleteInstances() != "Success")
                    {
                        Console.Write("An error occured while updating the status of the instances.");
                        _logger.Error("System - An error occured while updating the status of the instances.");
                        _logger.Info("System - Logging Stopped");
                        Environment.Exit(0);
                    }

                    instances = _instanceConfig.GetInstanceDetails(null);
                    raiseTickets = true;
                }

                else if (action == "-RaiseTickets")
                {
                    ticketData = _instanceConfig.GetTicketData("Contact");
                    raiseTickets = true;
                }

                else
                {
                    _logger.Info($"System - Running a refresh of all tables for {action.Replace("-Refresh ", "").Trim()}.");

                    instances = _instanceConfig.GetInstanceDetails(Convert.ToInt32(action.Replace("-Refresh ", "").Trim()));
                    ragNumbers = _instanceConfig.GetCurrentNumbers(Convert.ToInt32(action.Replace("-Refresh ", "").Trim()));
                }
            }

            if (instances == new List<Instance_Model>() && ticketData == new List<TicketDataModel>())
            {
                Console.Write("An error occured while getting a list of all instance IDs.");
                _logger.Error("System - An error occured while getting a list of all instance IDs.");
                _logger.Info("System - Logging Stopped");
                Environment.Exit(0);
            }

            if (_instanceConfig.GetInstanceConfig(instances, outputIntegrations) == "Failed")
            {
                Console.Write("An error occured while obtaining information for an instance.");
                _logger.Error("System - An error occured while obtaining information for an instance.");
                _logger.Info("System - Logging Stopped");
                Environment.Exit(0);
            }

            foreach (Instance_Model instance in instances)
            {
                if (_compareExclusion.CompareExclusion(instance) != "Success")
                {
                    Console.Write($"An error occured while comparing the exclusions for {instance.InstanceId}.");
                    _logger.Error($"System - An error occured while comparing the exclusions for {instance.InstanceId}.");
                }

                if (instance.Integrations != null)
                {
                    foreach (string integration in instance.Integrations.Name)
                    {
                        if (instance.Exclusions == null || !instance.Exclusions.Name.Contains(integration))
                        {
                            if (_instanceData.GatherContactData(instance.Integrations.Name.IndexOf(integration), instance) != "Success")
                            {
                                Console.Write("An error occured while gathering the data for an instance.");
                                _logger.Error("System - An error occured while gathering the data for an instance.");
                            }

                            _decideResult.GetResultIntegration(integration, instance.Integrations.Name.IndexOf(integration), instance);
                            _ragStatus.GetRAGStatus(false, instance.Integrations.Name.IndexOf(integration), instance, null, ragNumbers);
                        }

                        else
                        {
                            _ragStatus.GetRAGStatus(true, null, instance, integration, ragNumbers);
                        }
                    }
                }

                if (instance.PropertyFeed != null)
                {
                    if (instance.Exclusions == null || !instance.Exclusions.Name.Contains(instance.PropertyFeed.Name))
                    {
                        if (_instanceData.GatherPropertyData(instance) != "Success")
                        {
                            Console.Write("An error occured while gathering the data for an instance.");
                            _logger.Error("System - An error occured while gathering the data for an instance.");
                        }

                        _decideResult.GetResultProperty(instance);
                        _ragStatus.GetRAGStatus(false, instance.InstanceInformation.Count - 1, instance, null);
                    }

                    else
                    {
                        _ragStatus.GetRAGStatus(true, null, instance, instance.PropertyFeed.Name);
                    }
                }
            }

            foreach (Instance_Model instance in instances)
            {
                _outputData.OutputResult(instance, outputIntegrations, outputPropertyfeeds, ragNumbers);
            }

            if (instances.Count != 0)
            {
                _updateGeckoboard.UpdateRAGNumbers(ragNumbers);
            }

            if (raiseTickets && TicketSettingsModel.RaiseTickets)
            {
                string day = DateTime.UtcNow.DayOfWeek.ToString();
                bool raise = true;

                foreach (string exclDay in TicketSettingsModel.DaysFilter)
                {
                    if (exclDay == day)
                    {
                        raise = false;
                    }
                }

                if (raise)
                {
                    if (ticketData.Count == 0)
                    {
                        ticketData = _instanceConfig.GetTicketData("Contact");
                    }

                    _ticketHandler.Tickets(ticketData);
                }
            }

            _logger.Info("System - All steps complete, report can be refreshed.");
            _logger.Info("System - Logging Stopped");
            Environment.Exit(0);
        }
    }
}