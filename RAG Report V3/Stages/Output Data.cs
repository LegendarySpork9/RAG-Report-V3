using log4net;
using System.Data.SqlClient;
using RAG_Report_V3.Models;
using RAG_Report_V3.Converters;

namespace RAG_Report_V3.Stages
{
    internal class Output_Data
    {
        static readonly ILog _logger = LogManager.GetLogger("Log");
        static readonly ILog _sqlLogger = LogManager.GetLogger("SQLLog");

        // Controls the output method.
        public string OutputResult(Instance_Model instance, Output_Integrations_Model integrations, Output_Property_Feed_Model propertyFeeds)
        {
            Output _output = new();
            Instance_Data _instanceData = new();
            RAG_Status _ragStatus = new();

            _logger.Info($"{instance.SubDomain} - Outputting Data.");

            try
            {
                _logger.Debug($"{instance.SubDomain} - Outputting Instance Model.");

                SqlConnection connection;
                SqlCommand command;

                string sqlQuery = @"";

                connection = new SqlConnection(App_Settings_Model.OutputConnectionString);
                connection.Open();
                command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add(new SqlParameter("@URL", instance.URL));
                command.Parameters.Add(new SqlParameter("@Status", instance.Status));
                command.Parameters.Add(new SqlParameter("@AT", instance.ActiveTriggers));
                command.Parameters.Add(new SqlParameter("@InstanceID", instance.InstanceId));
                command.CommandTimeout = 0;
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected != 1)
                {
                    _logger.Warn($"{instance.SubDomain} - Instance table has not been updated correctly or there are no details to update.");
                }

                connection.Close();

                DateTime setUpdate = DateTime.Now;
                string date = DateTime.Now.ToString("yyyy-MM-dd");
                int instanceInfoid = 0;

                if (instance.Integrations != null)
                {
                    for (int x = 0; x < instance.Integrations.Name.Count; x++)
                    {
                        Instance_Information_Model instanceInformation = instance.InstanceInformation[x];

                        if (instanceInformation.RAGStatusID != 5)
                        {
                            Instance_Data_Model instanceData = instance.InstanceData[x];
                            Results_Model result = instance.Results[x];

                            if (instance.InstanceInformation[x].RAGStatusID == 0)
                            {
                                _logger.Warn($"{instance.SubDomain} - Invalid RAG Status Detected.");

                                _ragStatus.GetRAGStatus(false, x, instance, null);

                                _logger.Debug($"{instance.SubDomain} - Invalid RAG Status Fixed.");
                            }

                            _logger.Debug($"{instance.SubDomain} - Outputting Instance Information Model for: {instance.Integrations.Name[x]}");

                            sqlQuery = @"";

                            int integrationID = _output.GetIntID(instance.Integrations.Name[x], integrations);
                            setUpdate = _output.GetIntSetUpDate(integrationID, instance.Integrations.Name.IndexOf(instance.Integrations.Name[x]), instance);

                            command = new SqlCommand(sqlQuery, connection);
                            connection.Open();
                            command.Parameters.Add(new SqlParameter("@Date", date));
                            command.Parameters.Add(new SqlParameter("@InstanceID", instance.InstanceId));
                            command.Parameters.Add(new SqlParameter("@IntegrationID", integrationID));
                            command.Parameters.Add(new SqlParameter("@IntDate", instanceInformation.LastIntegrationDate.ToString("yyyy-MM-dd")));
                            command.Parameters.Add(new SqlParameter("@StatusID", instanceInformation.RAGStatusID));
                            command.Parameters.Add(new SqlParameter("@SetUpDate", setUpdate.ToString("yyyy-MM-dd")));
                            command.CommandTimeout = 0;
                            var infoId = command.ExecuteScalar();

                            if (infoId == null)
                            {
                                _logger.Warn($"{instance.SubDomain} - InstanceInformation table has not been updated correctly or there are no details to update.");
                            }

                            connection.Close();

                            _logger.Debug($"{instance.SubDomain} - Outputting Instance Data Model for: {instance.Integrations.Name[x]}");

                            sqlQuery = @"";

                            if (infoId != null)
                            {
                                instanceInfoid = (int)infoId;
                            }

                            for (int y = 0; y < instanceData.DataEndpoint.Count; y++)
                            {
                                command = new SqlCommand(sqlQuery, connection);
                                connection.Open();
                                command.Parameters.Add(new SqlParameter("@Created", instanceData.Created[y]));
                                command.Parameters.Add(new SqlParameter("@Updated", instanceData.Modified[y]));
                                command.Parameters.Add(new SqlParameter("@InstanceInfoID", instanceInfoid));
                                command.Parameters.Add(new SqlParameter("@Date", date));
                                command.Parameters.Add(new SqlParameter("@Endpoint", instanceData.DataEndpoint[y]));
                                command.CommandTimeout = 0;
                                rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected != 1)
                                {
                                    _logger.Warn($"{instance.SubDomain} - InstanceData table has not been updated correctly or there are no details to update.");
                                }

                                connection.Close();
                            }

                            if (result.ContactIssue)
                            {
                                _logger.Debug($"{instance.SubDomain} - Outputting Results Model for: {instance.Integrations.Name[x]}");

                                sqlQuery = @"";

                                command = new SqlCommand(sqlQuery, connection);
                                connection.Open();
                                command.Parameters.Add(new SqlParameter("@Date", date));
                                command.Parameters.Add(new SqlParameter("@InstanceID", instance.InstanceId));
                                command.Parameters.Add(new SqlParameter("@IntegrationID", integrationID));
                                command.Parameters.Add(new SqlParameter("@InstanceInfoID", instanceInfoid));
                                command.CommandTimeout = 0;
                                rowsAffected = command.ExecuteNonQuery();

                                if (rowsAffected != 1)
                                {
                                    _logger.Warn($"{instance.SubDomain} - IntegrationIssue table has not been updated correctly or there are no details to update.");
                                }

                                connection.Close();
                            }
                        }

                        else
                        {
                            _logger.Debug($"{instance.SubDomain} - Outputting Instance Information Model for: {instance.Integrations.Name[x]}");

                            sqlQuery = @"";

                            int integrationID = _output.GetIntID(instance.Integrations.Name[x], integrations);
                            DateOnly integrationDate = _instanceData.LastIntegrationDate(instance, instance.Integrations.Name[x], instance.Integrations.UniqueFields[x]);
                            setUpdate = _output.GetIntSetUpDate(integrationID, instance.Integrations.Name.IndexOf(instance.Integrations.Name[x]), instance);

                            command = new SqlCommand(sqlQuery, connection);
                            connection.Open();
                            command.Parameters.Add(new SqlParameter("@Date", date));
                            command.Parameters.Add(new SqlParameter("@InstanceID", instance.InstanceId));
                            command.Parameters.Add(new SqlParameter("@IntegrationID", integrationID));
                            command.Parameters.Add(new SqlParameter("@IntDate", integrationDate.ToString("yyyy-MM-dd")));
                            command.Parameters.Add(new SqlParameter("@StatusID", instanceInformation.RAGStatusID));
                            command.Parameters.Add(new SqlParameter("@SetUpDate", setUpdate.ToString("yyyy-MM-dd")));
                            command.CommandTimeout = 0;
                            rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected != 1)
                            {
                                _logger.Warn($"{instance.SubDomain} - InstanceInformation table has not been updated correctly or there are no details to update.");
                            }

                            connection.Close();
                        }
                    }
                }

                if (instance.PropertyFeed != null)
                {
                    Instance_Information_Model instanceInformation = instance.InstanceInformation.Last();

                    if (instanceInformation.RAGStatusID != 5)
                    {
                        Instance_Data_Model instanceData = instance.InstanceData.Last();
                        Results_Model result = instance.Results.Last();

                        _logger.Debug($"{instance.SubDomain} - Outputting Instance Information Model for: {instance.PropertyFeed.Name}");

                        sqlQuery = @"";

                        int propertyID = _output.GetPropID(instance.PropertyFeed.Name, instance.PropertyFeed.Type, propertyFeeds);
                        setUpdate = _output.GetPropSetUpDate(propertyID, instance);

                        command = new SqlCommand(sqlQuery, connection);
                        connection.Open();
                        command.Parameters.Add(new SqlParameter("@Date", date));
                        command.Parameters.Add(new SqlParameter("@InstanceID", instance.InstanceId));
                        command.Parameters.Add(new SqlParameter("@PropertyID", propertyID));
                        command.Parameters.Add(new SqlParameter("@FeedDate", instanceInformation.LastFeedReceived.ToString("yyyy-MM-dd")));
                        command.Parameters.Add(new SqlParameter("@StatusID", instanceInformation.RAGStatusID));
                        command.Parameters.Add(new SqlParameter("@SetUpDate", setUpdate.ToString("yyyy-MM-dd")));
                        command.CommandTimeout = 0;
                        var infoId = command.ExecuteScalar();

                        if (infoId == null)
                        {
                            _logger.Warn($"{instance.SubDomain} - InstanceInformation table has not been updated correctly or there are no details to update.");
                        }

                        connection.Close();

                        _logger.Debug($"{instance.SubDomain} - Outputting Instance Data Model for: {instance.PropertyFeed.Name}");

                        sqlQuery = @"";

                        if (infoId != null)
                        {
                            instanceInfoid = (int)infoId;
                        }

                        command = new SqlCommand(sqlQuery, connection);
                        connection.Open();
                        command.Parameters.Add(new SqlParameter("@Created", instanceData.Created[0]));
                        command.Parameters.Add(new SqlParameter("@Updated", instanceData.Modified[0]));
                        command.Parameters.Add(new SqlParameter("@InstanceInfoID", instanceInfoid));
                        command.Parameters.Add(new SqlParameter("@Date", date));
                        command.Parameters.Add(new SqlParameter("@Endpoint", instanceData.DataEndpoint[0]));
                        command.CommandTimeout = 0;
                        rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected != 1)
                        {
                            _logger.Warn($"{instance.SubDomain} - InstanceData table has not been updated correctly or there are no details to update.");
                        }

                        connection.Close();

                        if (result.PropertyIssue)
                        {
                            _logger.Debug($"{instance.SubDomain} - Outputting Results Model for: {instance.PropertyFeed.Name}");

                            sqlQuery = @"";

                            command = new SqlCommand(sqlQuery, connection);
                            connection.Open();
                            command.Parameters.Add(new SqlParameter("@Date", date));
                            command.Parameters.Add(new SqlParameter("@InstanceID", instance.InstanceId));
                            command.Parameters.Add(new SqlParameter("@PropertyID", propertyID));
                            command.Parameters.Add(new SqlParameter("@InstanceInfoID", instanceInfoid));
                            command.CommandTimeout = 0;
                            rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected != 1)
                            {
                                _logger.Warn($"{instance.SubDomain} - PropertyIssue table has not been updated correctly or there are no details to update.");
                            }

                            connection.Close();
                        }
                    }

                    else
                    {
                        _logger.Debug($"{instance.SubDomain} - Outputting Instance Information Model.");

                        sqlQuery = @"";

                        int propertyID = _output.GetPropID(instance.PropertyFeed.Name, instance.PropertyFeed.Type, propertyFeeds);
                        DateOnly feedDate = _instanceData.LastFeedDate(instance);
                        setUpdate = _output.GetPropSetUpDate(propertyID, instance);

                        command = new SqlCommand(sqlQuery, connection);
                        connection.Open();
                        command.Parameters.Add(new SqlParameter("@Date", date));
                        command.Parameters.Add(new SqlParameter("@InstanceID", instance.InstanceId));
                        command.Parameters.Add(new SqlParameter("@PropertyID", propertyID));
                        command.Parameters.Add(new SqlParameter("@FeedDate", feedDate.ToString("yyyy-MM-dd")));
                        command.Parameters.Add(new SqlParameter("@StatusID", instanceInformation.RAGStatusID));
                        command.Parameters.Add(new SqlParameter("@SetUpDate", setUpdate.ToString("yyyy-MM-dd")));
                        command.CommandTimeout = 0;
                        rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected != 1)
                        {
                            _logger.Warn($"{instance.SubDomain} - InstanceInformation table has not been updated correctly or there are no details to update.");
                        }

                        connection.Close();
                    }
                }
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running OutputResult in the Output_Data class.";
                _sqlLogger.Error($"{instance.InstanceId} - {ex}");
                return "Failed";
            }

            _logger.Info($"{instance.SubDomain} - Data Outputted");

            return "Success";
        }
    }
}
