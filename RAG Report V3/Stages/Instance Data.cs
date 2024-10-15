using log4net;
using System.Data.SqlClient;
using RAG_Report_V3.Models;
using RAG_Report_V3.Converters;

namespace RAG_Report_V3.Stages
{
    internal class Instance_Data
    {
        static readonly ILog _logger = LogManager.GetLogger("Log");
        static readonly ILog _sqlLogger = LogManager.GetLogger("SQLLog");

        // Gathers the data for an integration.
        public string GatherContactData(int index, Instance_Model instance)
        {
            string name = instance.Integrations.Name[index];

            _logger.Info($"{instance.SubDomain} - Getting all counts data for: {name}");

            while (instance.InstanceData.Count < index)
            {
                instance.InstanceData.Add(new Instance_Data_Model
                { 
                    DataEndpoint = { "CONTACT" }, 
                    Created = { 0 }, 
                    Modified = { 0 } 
                });
                instance.Integrations.HasRun.Add(false);
                instance.Results.Add(new Results_Model());
            }

            try
            {
                SqlConnection connection;
                SqlCommand command;
                SqlDataReader dataReader;

                Instance_Data_Formatters _instanceDataformatters = new();

                Instance_Data_Model instanceData = new();
                Instance_Information_Model instanceInformation = new();

                string sqlQuery = File.ReadAllText($"{App_Settings_Model.SQLScripts}Instance Data Report.sql");
                var queryAddon = _instanceDataformatters.FormatUniqueFields(instance.Integrations.UniqueFields[index]);
                bool firstRow = true;

                if (instance.MultiplePSIntegrations && instance.Integrations.Name[index].Contains("Property Schema - ") &&
                    instance.Integrations.Name[index] != "Property Schema - Gazeal")
                {
                    sqlQuery = sqlQuery.Replace("^", $"and {queryAddon.Item1} is not null and field = ''{instance.Integrations.Name[index].Remove(0, instance.Integrations.Name[index].IndexOf("-") + 2).Replace(" ", "")}''");
                }

                if (!string.IsNullOrWhiteSpace(queryAddon.Item1))
                {
                    sqlQuery = sqlQuery.Replace("^", $"and {queryAddon.Item1} is not null");
                }

                else
                {
                    sqlQuery = sqlQuery.Replace(" ^", "");
                }

                if (!string.IsNullOrEmpty(queryAddon.Item2))
                {
                    sqlQuery = sqlQuery.Replace("|", queryAddon.Item2);
                }

                else
                {
                    sqlQuery = sqlQuery.Replace(",\r\n|", "");
                }

                connection = new SqlConnection(App_Settings_Model.InputConnectionString);
                connection.Open();
                command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add(new SqlParameter("@dbs", instance.Server));
                command.Parameters.Add(new SqlParameter("@db", instance.Database));
                command.CommandTimeout = 0;
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    if (firstRow)
                    {
                        if (dataReader.IsDBNull(0))
                        {
                            instance.Integrations.HasRun.Add(false);
                            _logger.Debug($"{instance.SubDomain} - Integration has run: {false}");

                            break;
                        }

                        else
                        {
                            instance.Integrations.HasRun.Add(true);
                            _logger.Debug($"{instance.SubDomain} - Integration has run: {true}");

                            instanceInformation.LastIntegrationDate = DateOnly.Parse(dataReader.GetString(0));
                            _logger.Debug($"{instance.SubDomain} - Last Integration Date: {dataReader.GetString(0)}");

                            firstRow = false;
                        }
                    }

                    else
                    {
                        instanceData.DataEndpoint.Add(dataReader.GetString(0));
                        instanceData.Created.Add(dataReader.GetInt32(1));
                        instanceData.Modified.Add(dataReader.GetInt32(2));

                        _logger.Debug($"{instance.SubDomain} - Endpoint: {dataReader.GetString(0)}");
                        _logger.Debug($"{instance.SubDomain} - Records Created: {dataReader.GetInt32(1)}");
                        _logger.Debug($"{instance.SubDomain} - Records Modified: {dataReader.GetInt32(2)}");
                    }
                }

                dataReader.Close();
                connection.Close();

                if (instanceData.DataEndpoint.Count == 0)
                {
                    instanceData.DataEndpoint.Add("CONTACT");
                    instanceData.Created.Add(0);
                    instanceData.Modified.Add(0);
                }

                instance.InstanceInformation.Add(instanceInformation);
                instance.InstanceData.Add(instanceData);

                _logger.Info($"{instance.SubDomain} - Counts data obtained for: {name}");
                return "Success";
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running GatherContactData in the Instance_Data class.";
                _sqlLogger.Error($"{instance.Server}, {instance.Database} - {ex}");

                instance.InstanceInformation.Add(new Instance_Information_Model());
                instance.InstanceData.Add(new Instance_Data_Model
                {
                    DataEndpoint = { "CONTACT" },
                    Created = { 0 },
                    Modified = { 0 }
                });
                instance.Integrations.HasRun.Add(false);

                return "Failed";
            }
        }

        // Gathers the property data.
        public string GatherPropertyData(Instance_Model instance)
        {
            string name = instance.PropertyFeed.Name;

            _logger.Info($"{instance.SubDomain} - Getting all counts data for: {name}");

            if (instance.Integrations != null)
            {
                while (instance.InstanceData.Count < instance.Integrations.Name.Count)
                {
                    instance.InstanceData.Add(new Instance_Data_Model
                    { 
                        DataEndpoint = { "PRODUCT" }, 
                        Created = { 0 }, 
                        Modified = { 0 } 
                    });
                    instance.Integrations.HasRun.Add(false);
                    instance.Results.Add(new Results_Model());
                }
            }

            try
            {
                SqlConnection connection;
                SqlCommand command;
                SqlDataReader dataReader;

                Instance_Data_Model instanceData = new();
                Instance_Information_Model instanceInformation = new();
                
                string sqlQuery = File.ReadAllText($"{App_Settings_Model.SQLScripts}Property Data Report.sql");
                bool firstRow = true;

                connection = new SqlConnection(App_Settings_Model.InputConnectionString);
                connection.Open();
                command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add(new SqlParameter("@dbs", instance.Server));
                command.Parameters.Add(new SqlParameter("@db", instance.Database));
                command.CommandTimeout = 0;
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    if (firstRow)
                    {
                        if (dataReader.IsDBNull(0))
                        {
                            instance.PropertyFeed.HasRun = false;
                            _logger.Debug($"{instance.SubDomain} - Property feed has run: {false}");

                            break;
                        }

                        else
                        {
                            instance.PropertyFeed.HasRun = true;
                            _logger.Debug($"{instance.SubDomain} - Property feed has run: {true}");

                            instanceInformation.LastFeedReceived = DateOnly.Parse(dataReader.GetString(0));
                            _logger.Debug($"{instance.SubDomain} - Last Property Feed Received: {dataReader.GetString(0)}");

                            firstRow = false;
                        }
                    }

                    else
                    {
                        instanceData.DataEndpoint.Add(dataReader.GetString(0));
                        instanceData.Created.Add(dataReader.GetInt32(1));
                        instanceData.Modified.Add(dataReader.GetInt32(2));

                        _logger.Debug($"{instance.SubDomain} - Endpoint: {dataReader.GetString(0)}");
                        _logger.Debug($"{instance.SubDomain} - Records Created: {dataReader.GetInt32(1)}");
                        _logger.Debug($"{instance.SubDomain} - Records Modified: {dataReader.GetInt32(2)}");
                    }
                }

                dataReader.Close();
                connection.Close();

                if (instanceData.DataEndpoint.Count == 0)
                {
                    instanceData.DataEndpoint.Add("PRODUCT");
                    instanceData.Created.Add(0);
                    instanceData.Modified.Add(0);
                }

                instance.InstanceInformation.Add(instanceInformation);
                instance.InstanceData.Add(instanceData);

                _logger.Info($"{instance.SubDomain} - Counts data obtained for: {name}");
                return "Success";
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running GatherPropertyData in the Instance_Data class.";
                _sqlLogger.Error($"{instance.Server}, {instance.Database} - {ex}");

                instance.InstanceInformation.Add(new Instance_Information_Model());
                instance.InstanceData.Add(new Instance_Data_Model
                {
                    DataEndpoint = { "PRODUCT" },
                    Created = { 0 },
                    Modified = { 0 }
                });
                instance.PropertyFeed.HasRun = false;

                return "Failed";
            }
        }

        // Gets the last integration date.
        public DateOnly LastIntegrationDate(Instance_Model instance, string name, string uniqueFields)
        {
            Instance_Configuration _instanceConfiguration = new();

            DateOnly integrationDate = DateOnly.Parse("1900-01-01");

            try
            {
                SqlConnection connection;
                SqlCommand command;
                SqlDataReader dataReader;

                Instance_Data_Formatters _instanceDataformatters = new();

                string sqlQuery = "";
                var queryAddon = _instanceDataformatters.FormatUniqueFields(uniqueFields);

                if (instance.MultiplePSIntegrations && name.Contains("Property Schema - ") &&
                    name != "Property Schema - Gazeal")
                {
                    sqlQuery = sqlQuery.Replace("^", $"and {queryAddon.Item1} is not null and field = '{_instanceConfiguration.GetPSSource(name.Remove(0, name.IndexOf("-") + 2))}'");
                }

                if (!string.IsNullOrWhiteSpace(queryAddon.Item1))
                {
                    sqlQuery = sqlQuery.Replace("^", $"and {queryAddon.Item1} is not null");
                }

                else
                {
                    sqlQuery = sqlQuery.Replace(" ^", "");
                }

                connection = new SqlConnection($"Data Source={instance.Server};Initial Catalog={instance.Database};User Id=;Password=");
                connection.Open();
                command = new SqlCommand(sqlQuery, connection);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    if (!dataReader.IsDBNull(0))
                    {
                        integrationDate = DateOnly.Parse(dataReader.GetDateTime(0).ToString("yyyy-MM-dd"));
                    }
                }

                dataReader.Close();
                connection.Close();
                return integrationDate;
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running LastIntegrationDate in the Instance_Data class.";
                _sqlLogger.Error($"{instance.InstanceId} - {ex}");
                return DateOnly.Parse("1900-01-01");
            }
        }

        // Gets the last feed recevied date.
        public DateOnly LastFeedDate(Instance_Model instance)
        {
            DateOnly feedDate = DateOnly.Parse("1900-01-01");

            try
            {
                SqlConnection connection;
                SqlCommand command;
                SqlDataReader dataReader;

                string sqlQuery = "";

                connection = new SqlConnection($"Data Source={instance.Server};Initial Catalog={instance.Database};User Id=;Password=");
                connection.Open();
                command = new SqlCommand(sqlQuery, connection);
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    feedDate = DateOnly.Parse(dataReader.GetDateTime(0).ToString("yyyy-MM-dd"));
                }

                dataReader.Close();
                connection.Close();
                return feedDate;
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running LastFeedDate in the Instance_Data class.";
                _sqlLogger.Error($"{instance.InstanceId} - {ex}");
                return DateOnly.Parse("1900-01-01");
            }
        }
    }
}
