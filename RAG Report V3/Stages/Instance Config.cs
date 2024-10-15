using log4net;
using System.Data.SqlClient;
using RAG_Report_V3.Models;
using RAG_Report_V3.Converters;

namespace RAG_Report_V3.Stages
{
    internal class Instance_Config
    {
        static readonly ILog _logger = LogManager.GetLogger("Log");
        static readonly ILog _sqlLogger = LogManager.GetLogger("SQLLog");
        readonly Instance_Configuration _instanceConfiguration = new();

        // Gets all integrations from the Integration table.
        public Output_Integrations_Model GetOutputIntegrations()
        {
            Output_Integrations_Model integrations = new();

            _logger.Info("System - Getting Integrations");

            try
            {
                SqlConnection connection;
                SqlCommand command;
                SqlDataReader dataReader;

                string sqlQuery = "";

                connection = new SqlConnection(App_Settings_Model.OutputConnectionString);
                connection.Open();
                command = new SqlCommand(sqlQuery, connection);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    integrations.IntegrationId.Add(dataReader.GetInt32(0));
                    integrations.Name.Add(dataReader.GetString(1));
                    integrations.Type.Add(dataReader.GetString(2));

                    _logger.Debug($"System - Integration ID: {dataReader.GetInt32(0)}");
                    _logger.Debug($"System - Integration Name: {dataReader.GetString(1)}");
                    _logger.Debug($"System - Integration Type: {dataReader.GetString(2)}");

                    if (!dataReader.IsDBNull(3))
                    {
                        integrations.UniqueField.Add(dataReader.GetString(3));

                        _logger.Debug($"System - Integration Unique Fields: {dataReader.GetString(3)}");
                    }

                    else
                    {
                        integrations.UniqueField.Add("");
                        
                        _logger.Debug($"System - Integration Unique Fields: ");
                    }

                    if (!dataReader.IsDBNull(4))
                    {
                        integrations.Source.Add(dataReader.GetString(4));

                        _logger.Debug($"System - Integration Source: {dataReader.GetString(4)}");
                    }

                    else
                    {
                        integrations.Source.Add("");

                        _logger.Debug($"System - Integration Source: ");
                    }
                }

                dataReader.Close();
                connection.Close();

                _logger.Info("System - Integrations Obtained.");
                return integrations;
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running GetOutputIntegrations in the Instance_Config class.";
                _sqlLogger.Error(ex);
                return new Output_Integrations_Model();
            }
        }

        // Gets all integrations from the Integration table.
        public Output_Property_Feed_Model GetOutputPropertyFeeds()
        {
            Output_Property_Feed_Model propertyFeeds = new();

            _logger.Info("System - Getting Property Feeds.");

            try
            {
                SqlConnection connection;
                SqlCommand command;
                SqlDataReader dataReader;

                string sqlQuery = "";

                connection = new SqlConnection(App_Settings_Model.OutputConnectionString);
                connection.Open();
                command = new SqlCommand(sqlQuery, connection);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    propertyFeeds.PropertyId.Add(dataReader.GetInt32(0));
                    propertyFeeds.Name.Add(dataReader.GetString(1));
                    propertyFeeds.Type.Add(dataReader.GetString(2));

                    _logger.Debug($"System - Property Feed ID: {dataReader.GetInt32(0)}");
                    _logger.Debug($"System - Property Feed Name: {dataReader.GetString(1)}");
                    _logger.Debug($"System - Property Feed Type: {dataReader.GetString(2)}");
                }

                dataReader.Close();
                connection.Close();

                _logger.Info("System - Property Feeds Obtained.");
                return propertyFeeds;
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running GetOutputPropertyFeeds in the Instance_Config class.";
                _sqlLogger.Error(ex);
                return new Output_Property_Feed_Model();
            }
        }

        // Gets a list of all instance details.
        public List<Instance_Model> GetInstanceDetails(int? id)
        {
            _logger.Info("System - Getting Instance Details.");

            List<Instance_Model> instances = new();

            try
            {
                SqlConnection connection;
                SqlCommand command;
                SqlDataReader dataReader;

                string sqlQuery = @"";

                if (id.HasValue)
                {
                    sqlQuery += "";
                }

                sqlQuery += "";

                connection = new SqlConnection(App_Settings_Model.InputConnectionString);
                connection.Open();
                command = new SqlCommand(sqlQuery, connection);

                if (id.HasValue)
                {
                    command.Parameters.Add(new SqlParameter("@InstanceID", id.Value));
                }

                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    Instance_Model instance = new();

                    instance.InstanceId = dataReader.GetInt32(0);
                    instance.Status = _instanceConfiguration.GetInstanceStatus(dataReader.GetInt32(1));
                    instance.URL = dataReader.GetString(2).Replace("http://", "").Replace("https://", "").Replace("/", "");
                    instance.SubDomain = instance.URL.Replace(".briefyourmarket.com", "");
                    instance.Server = dataReader.GetString(3);
                    instance.Database = dataReader.GetString(4);

                    _logger.Debug($"System - Instance ID: {instance.InstanceId}");
                    _logger.Debug($"System - Instance URL: {instance.URL}");
                    _logger.Debug($"System - Instance Status: {instance.Status}");
                    _logger.Debug($"System - Instance Server: {instance.Server}");
                    _logger.Debug($"System - Instance Database: {instance.Database}");

                    instances.Add(instance);
                }

                dataReader.Close();
                connection.Close();

                _logger.Info("System - Instance Details Obtained.");
                return instances;
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running GetInstanceDetails in the Instance_Config class.";
                _sqlLogger.Error(ex);
                return new List<Instance_Model>();
            }
        }

        // Gets the information about the instances.
        public string GetInstanceConfig(List<Instance_Model> instances, Output_Integrations_Model integrations)
        {
            Parallel.ForEach(instances, new ParallelOptions { MaxDegreeOfParallelism = App_Settings_Model.MaxThreads }, (instance, ParallelLoopState) =>
            {
                 _logger.Info($"{instance.SubDomain} - Getting all instance information for: {instance.InstanceId}");

                 if (GetActiveTriggers(instance) != "Success")
                 {
                     Console.WriteLine($"{instance.SubDomain} - Obtaining instance information for {instance.InstanceId} has failed.");
                     _logger.Error($"{instance.SubDomain} - Obtaining instance information for {instance.InstanceId} has failed.");
                     ParallelLoopState.Break();
                 }

                 if (GetIntegrations(instance, integrations) != "Success")
                 {
                     Console.WriteLine($"{instance.SubDomain} - Obtaining instance information for {instance.InstanceId} has failed.");
                     _logger.Error($"{instance.SubDomain} - Obtaining instance information for {instance.InstanceId} has failed.");
                     ParallelLoopState.Break();
                 }

                 if (GetPropertyFeed(instance) != "Success")
                 {
                     Console.WriteLine($"{instance.SubDomain} - Obtaining instance information for {instance.InstanceId} has failed.");
                     _logger.Error($"{instance.SubDomain} - Obtaining instance information for {instance.InstanceId} has failed.");
                     ParallelLoopState.Break();
                 }

                 if (GetExclusion(instance) != "Success")
                 {
                     Console.WriteLine($"{instance.SubDomain} - Obtaining instance information for {instance.InstanceId} has failed.");
                     _logger.Error($"{instance.SubDomain} - Obtaining instance information for {instance.InstanceId} has failed.");
                     ParallelLoopState.Break();
                 }

                 _logger.Info($"{instance.SubDomain} - Obtained all instance information for: {instance.InstanceId}");
            });

            return "Success";
        }

        // Gets the number of active triggers for the given instance.
        private static string GetActiveTriggers(Instance_Model instance)
        {
            try
            {
                SqlConnection connection;
                SqlCommand command;
                SqlDataReader dataReader;

                string sqlQuery = @"";

                connection = new SqlConnection($"Data Source={instance.Server};Initial Catalog={instance.Database};User Id=;Password=");
                connection.Open();
                command = new SqlCommand(sqlQuery, connection);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    instance.ActiveTriggers = dataReader.GetInt32(0);

                    _logger.Debug($"{instance.SubDomain} - Active Triggers: {instance.ActiveTriggers}");
                }

                dataReader.Close();
                connection.Close();

                return "Success";
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running GetActiveTriggers in the Instance_Config class.";
                _sqlLogger.Error($"{instance.InstanceId} - {ex}");
                return "Failed";
            }
        }

        // Gets the integration information for the given instance.
        private string GetIntegrations(Instance_Model instance, Output_Integrations_Model integrations)
        {
            try
            {
                SqlConnection connection;
                SqlCommand command;
                SqlDataReader dataReader;

                instance.Integrations = new Integration_Model();
                string excludedIntegrations = File.ReadAllText(App_Settings_Model.ExcludeIntegrations);
                string sqlQuery = @$"";
                bool firstRow = true;

                connection = new SqlConnection(App_Settings_Model.InputConnectionString);
                connection.Open();
                command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add(new SqlParameter("@InstanceID", instance.InstanceId));
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    if (firstRow)
                    {
                        if (int.Parse(dataReader.GetString(0)) > 0)
                        {
                            firstRow = false;
                        }

                        if (int.Parse(dataReader.GetString(0)) == 0)
                        {
                            _logger.Debug($"{instance.SubDomain} - No Integrations Found");
                            instance.Integrations = null;

                            dataReader.Close();
                            connection.Close();

                            return "Success";
                        }
                    }

                    else
                    {
                        if (dataReader.GetString(0) == "Property Schema")
                        {
                            GetPropertySchemaDetails(_instanceConfiguration.GetIntegrationType(dataReader.GetString(0), integrations), dataReader.GetDateTime(1), instance);
                        }

                        else
                        {
                            instance.Integrations.Name.Add(dataReader.GetString(0));
                            instance.Integrations.Type.Add(_instanceConfiguration.GetIntegrationType(dataReader.GetString(0), integrations));
                            instance.Integrations.SetUpDate.Add(dataReader.GetDateTime(1));

                            _logger.Debug($"{instance.SubDomain} - Integration Name: {dataReader.GetString(0)}");
                            _logger.Debug($"{instance.SubDomain} - Integration Type: {_instanceConfiguration.GetIntegrationType(dataReader.GetString(0), integrations)}");
                            _logger.Debug($"{instance.SubDomain} - Integration Set Up Date: {dataReader.GetDateTime(1)}");
                        }
                    }
                }

                dataReader.Close();
                connection.Close();

                bool psProcessed = false;

                foreach (string name in instance.Integrations.Name)
                {
                    int index = 0;

                    if (name.Contains("Property Schema - ") && name != "Property Schema - Gazeal")
                    {
                        index = integrations.Name.IndexOf(name.Remove(name.IndexOf("-") - 1));
                    }

                    else
                    {
                        index = integrations.Name.IndexOf(name);
                    }

                    if (!string.IsNullOrEmpty(integrations.UniqueField[index]))
                    {
                        if (name.Contains("Property Schema - ") && name != "Property Schema - Gazeal")
                        {
                            if (!psProcessed)
                            {
                                instance.Integrations.UniqueFields.Add(integrations.UniqueField[index]);

                                _logger.Debug($"{instance.SubDomain} - {name.Remove(name.IndexOf("-") - 1)} Unique Fields: {integrations.UniqueField[index]}");

                                psProcessed = true;
                            }

                            else
                            {
                                instance.Integrations.UniqueFields.Add(integrations.UniqueField[index]);
                            }
                        }

                        else
                        {
                            instance.Integrations.UniqueFields.Add(integrations.UniqueField[index]);

                            _logger.Debug($"{instance.SubDomain} - {name} Unique Fields: {integrations.UniqueField[index]}");
                        }
                    }

                    else
                    {
                        instance.Integrations.UniqueFields.Add("");

                        _logger.Debug($"{instance.SubDomain} - {name} Unique Fields: ");
                    }
                }

                return "Success";
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running GetIntegrations in the Instance_Config class.";
                _sqlLogger.Error($"{instance.InstanceId} - {ex}");
                return "Failed";
            }
        }

        // Gets the integration type from the given instance.
        private string GetPropertySchemaDetails(string Type, DateTime SetUpDate, Instance_Model instance)
        {
            try
            {
                SqlConnection connection;
                SqlCommand command;
                SqlDataReader dataReader;

                string sqlQuery = @"";

                connection = new SqlConnection($"Data Source={instance.Server};Initial Catalog={instance.Database};User Id=;Password=");
                connection.Open();
                command = new SqlCommand(sqlQuery, connection);
                dataReader = command.ExecuteReader();
                int source = 0;

                while (dataReader.Read())
                {
                    instance.Integrations.Name.Add("Property Schema - " + _instanceConfiguration.GetInstanceSource(dataReader.GetString(0)));
                    instance.Integrations.Type.Add(Type);
                    instance.Integrations.SetUpDate.Add(SetUpDate);

                    _logger.Debug($"{instance.SubDomain} - Integration Name: Property Schema - {_instanceConfiguration.GetInstanceSource(dataReader.GetString(0))}");
                    _logger.Debug($"{instance.SubDomain} - Integration Type: {Type}");
                    _logger.Debug($"{instance.SubDomain} - Integration Set Up Date: {SetUpDate}");

                    source++;
                }

                if (source == 0)
                {
                    instance.Integrations.Name.Add("Property Schema");
                    instance.Integrations.Type.Add(Type);
                    instance.Integrations.SetUpDate.Add(SetUpDate);

                    _logger.Debug($"{instance.SubDomain} - Integration Name: Property Schema");
                    _logger.Debug($"{instance.SubDomain} - Integration Type: {Type}");
                    _logger.Debug($"{instance.SubDomain} - Integration Set Up Date: {SetUpDate}");

                    source++;
                }

                if (source > 1)
                {
                    instance.MultiplePSIntegrations = true;
                }

                _logger.Debug($"{instance.SubDomain} - Multiple Property Schema Integrations: {instance.MultiplePSIntegrations}");

                dataReader.Close();
                connection.Close();

                return "Success";
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running GetPropertySchemaDetails in the Instance_Config class.";
                _sqlLogger.Error($"{instance.InstanceId} - {ex}");
                return "Failed";
            }
        }

        // Gets the property feed information for the given instance.
        private static string GetPropertyFeed(Instance_Model instance)
        {
            try
            {
                SqlConnection connection;
                SqlCommand command;
                SqlDataReader dataReader;

                instance.PropertyFeed = new Property_Feed_Model();
                string sqlQuery = @"";
                bool firstRow = true;

                connection = new SqlConnection(App_Settings_Model.InputConnectionString);
                connection.Open();
                command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add(new SqlParameter("@InstanceID", instance.InstanceId));
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    if (firstRow)
                    {
                        if (int.Parse(dataReader.GetString(0)) > 0)
                        {
                            firstRow = false;
                        }

                        if (int.Parse(dataReader.GetString(0)) == 0)
                        {
                            _logger.Debug($"{instance.SubDomain} - No Property Feed Found");
                            instance.PropertyFeed = null;

                            dataReader.Close();
                            connection.Close();

                            return "Success";
                        }
                    }

                    else
                    {
                        instance.PropertyFeed.Name = dataReader.GetString(0);
                        instance.PropertyFeed.Type = dataReader.GetString(1);
                        instance.PropertyFeed.SetUpDate = dataReader.GetDateTime(2);
                    }
                }

                _logger.Debug($"{instance.SubDomain} - Property Feed Name: {instance.PropertyFeed.Name}");
                _logger.Debug($"{instance.SubDomain} - Property Feed Type: {instance.PropertyFeed.Type}");
                _logger.Debug($"{instance.SubDomain} - Property Feed Set Up Date: {instance.PropertyFeed.SetUpDate}");

                dataReader.Close();
                connection.Close();

                return "Success";
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running GetPropertyFeed in the Instance_Config class.";
                _sqlLogger.Error($"{instance.InstanceId} - {ex}");
                return "Failed";
            }
        }

        // Gets the exclusion information for the given instance.
        private string GetExclusion(Instance_Model instance)
        {
            try
            {
                SqlConnection connection;
                SqlCommand command;
                SqlDataReader dataReader;

                string sqlQuery = @"";
                int exclusions = 0;
                int comments = 0;
                bool firstRow = true;

                connection = new SqlConnection(App_Settings_Model.InputConnectionString);
                connection.Open();
                command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add(new SqlParameter("@InstanceID", instance.InstanceId));
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    if (firstRow)
                    {
                        exclusions = dataReader.GetInt32(0);
                        firstRow = false;
                    }

                    else
                    {
                        comments = dataReader.GetInt32(0);
                    }
                }

                dataReader.Close();
                connection.Close();

                if (exclusions > 0)
                {
                    instance.Exclusions = new Exclusion_Model();

                    if (instance.Integrations != null)
                    {
                        foreach (string name in instance.Integrations.Name)
                        {
                            sqlQuery = @"";

                            string integration = name;

                            if (integration.Contains("Property Schema - ") && integration != "Property Schema - Gazeal")
                            {
                                integration = integration.Remove(integration.IndexOf("-") - 1);

                                if (comments > 0)
                                {
                                    sqlQuery += "";
                                }
                            }

                            sqlQuery += "";

                            connection = new SqlConnection(App_Settings_Model.InputConnectionString);
                            connection.Open();
                            command = new SqlCommand(sqlQuery, connection);
                            command.Parameters.Add(new SqlParameter("@InstanceID", instance.InstanceId));
                            command.Parameters.Add(new SqlParameter("@Name", integration));

                            if (sqlQuery.Contains(""))
                            {
                                command.Parameters.Add(new SqlParameter("@Source", $"%*Partner: {_instanceConfiguration.GetPSSource(name.Remove(0, name.IndexOf("-") + 2))}*%"));
                            }

                            dataReader = command.ExecuteReader();

                            while (dataReader.Read())
                            {
                                string intName = dataReader.GetString(1);
                                string comment = dataReader.GetString(4);

                                if ((intName == "Property Schema" && comment.Contains("*Partner: ")) || intName == "Property Schema")
                                {
                                    instance.Exclusions.Type.Add(dataReader.GetString(0));
                                    instance.Exclusions.Name.Add(name);
                                    instance.Exclusions.ExcludeTillDate.Add(DateOnly.Parse(dataReader.GetDateTime(2).ToString("yyyy-MM-dd")));
                                    instance.Exclusions.ExcludeReason.Add(dataReader.GetString(3));
                                    instance.Exclusions.Comments.Add(dataReader.GetString(4));

                                    _logger.Debug($"{instance.SubDomain} - Exclusion Type: {dataReader.GetString(0)}");
                                    _logger.Debug($"{instance.SubDomain} - Exclusion Name: {name}");
                                    _logger.Debug($"{instance.SubDomain} - Exclusion Date: {dataReader.GetDateTime(2):yyyy-MM-dd}");
                                    _logger.Debug($"{instance.SubDomain} - Exclusion Reason: {dataReader.GetString(3)}");
                                    _logger.Debug($"{instance.SubDomain} - Exclusion Comments: {dataReader.GetString(4)}");
                                }

                                else
                                {
                                    instance.Exclusions.Type.Add(dataReader.GetString(0));
                                    instance.Exclusions.Name.Add(intName);
                                    instance.Exclusions.ExcludeTillDate.Add(DateOnly.Parse(dataReader.GetDateTime(2).ToString("yyyy-MM-dd")));
                                    instance.Exclusions.ExcludeReason.Add(dataReader.GetString(3));
                                    instance.Exclusions.Comments.Add(comment);

                                    _logger.Debug($"{instance.SubDomain} - Exclusion Type: {dataReader.GetString(0)}");
                                    _logger.Debug($"{instance.SubDomain} - Exclusion Name: {intName}");
                                    _logger.Debug($"{instance.SubDomain} - Exclusion Date: {dataReader.GetDateTime(2):yyyy-MM-dd}");
                                    _logger.Debug($"{instance.SubDomain} - Exclusion Reason: {dataReader.GetString(3)}");
                                    _logger.Debug($"{instance.SubDomain} - Exclusion Comments: {comment}");
                                }
                            }

                            dataReader.Close();
                            connection.Close();
                        }
                    }

                    if (instance.PropertyFeed != null)
                    {
                        sqlQuery = @"";

                        connection = new SqlConnection(App_Settings_Model.InputConnectionString);
                        connection.Open();
                        command = new SqlCommand(sqlQuery, connection);
                        command.Parameters.Add(new SqlParameter("@InstanceID", instance.InstanceId));
                        command.Parameters.Add(new SqlParameter("@Name", instance.PropertyFeed.Name));
                        dataReader = command.ExecuteReader();

                        while (dataReader.Read())
                        {
                            instance.Exclusions.Type.Add(dataReader.GetString(0));
                            instance.Exclusions.Name.Add(dataReader.GetString(1));
                            instance.Exclusions.ExcludeTillDate.Add(DateOnly.Parse(dataReader.GetDateTime(2).ToString("yyyy-MM-dd")));
                            instance.Exclusions.ExcludeReason.Add(dataReader.GetString(3));
                            instance.Exclusions.Comments.Add(dataReader.GetString(4));

                            _logger.Debug($"{instance.SubDomain} - Exclusion Type: {dataReader.GetString(0)}");
                            _logger.Debug($"{instance.SubDomain} - Exclusion Name: {dataReader.GetString(1)}");
                            _logger.Debug($"{instance.SubDomain} - Exclusion Date: {dataReader.GetDateTime(2).ToString("yyyy-MM-dd")}");
                            _logger.Debug($"{instance.SubDomain} - Exclusion Reason: {dataReader.GetString(3)}");
                            _logger.Debug($"{instance.SubDomain} - Exclusion Comments: {dataReader.GetString(4)}");
                        }

                        dataReader.Close();
                        connection.Close();
                    }

                    return "Success";
                }

                _logger.Debug($"{instance.SubDomain} - No Exclusions found");
                return "Success";
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running GetExclusion in the Instance_Config class.";
                _sqlLogger.Error($"{instance.InstanceId} - {ex}");
                return "Failed";
            }
        }
    }
}
