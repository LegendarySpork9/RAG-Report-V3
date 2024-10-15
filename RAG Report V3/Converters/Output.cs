using log4net;
using RAG_Report_V3.Models;
using System.Data.SqlClient;

namespace RAG_Report_V3.Converters
{
    internal class Output
    {
        static readonly ILog _sqlLogger = LogManager.GetLogger("SQLLog");

        // Converts the Integration Name to the ID.
        public int GetIntID(string name, Output_Integrations_Model integrations)
        {
            int index;

            if (name == "Property Schema")
            {
                index = integrations.Name.LastIndexOf(name);
                return integrations.IntegrationId[index];
            }

            if (name.Contains("Property Schema - ") && name != "Property Schema - Gazeal")
            {
                index = integrations.Source.IndexOf(name.Remove(0, name.IndexOf("-") + 2));
                return integrations.IntegrationId[index];
            }

            index = integrations.Name.IndexOf(name);
            return integrations.IntegrationId[index];
        }

        // Converts the Property Feed Name to the ID.
        public int GetPropID(string name, string type, Output_Property_Feed_Model propertyFeeds)
        {
            int propID = 0;

            for (int index = 0; index < propertyFeeds.Name.Count; index++)
            {
                if (propertyFeeds.Name[index] == name)
                {
                    if (propertyFeeds.Type[index] == type)
                    {
                        propID = propertyFeeds.PropertyId[index];
                    }
                }
            }

            return propID;
        }

        // Converts the Instance ID to the set up date.
        public DateTime GetIntSetUpDate(int intID, int index, Instance_Model instance)
        {
            try
            {
                DateTime setUpdate = DateTime.Now;

                SqlConnection connection;
                SqlCommand command;
                SqlDataReader dataReader;

                string sqlQuery = @"";

                connection = new SqlConnection(App_Settings_Model.OutputConnectionString);
                connection.Open();
                command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add(new SqlParameter("@ID", instance.InstanceId));
                command.Parameters.Add(new SqlParameter("@IntID", intID));
                command.CommandTimeout = 0;
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    setUpdate = dataReader.GetDateTime(0);
                }

                dataReader.Close();
                connection.Close();

                if (setUpdate > instance.Integrations.SetUpDate[index])
                {
                    return instance.Integrations.SetUpDate[index];
                }

                return setUpdate;
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running GetIntSetUpDate in the Output class.";
                _sqlLogger.Error($"{instance.InstanceId}, {intID} - {ex}");
                return DateTime.Now;
            }
        }

        // Converts the Instance ID to the set up date.
        public DateTime GetPropSetUpDate(int propID, Instance_Model instance)
        {
            try
            {
                DateTime setUpdate = DateTime.Now;

                SqlConnection connection;
                SqlCommand command;
                SqlDataReader dataReader;

                string sqlQuery = @"";

                connection = new SqlConnection(App_Settings_Model.OutputConnectionString);
                connection.Open();
                command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add(new SqlParameter("@ID", instance.InstanceId));
                command.Parameters.Add(new SqlParameter("@PropID", propID));
                command.CommandTimeout = 0;
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    setUpdate = dataReader.GetDateTime(0);
                }

                dataReader.Close();
                connection.Close();

                if (setUpdate > instance.PropertyFeed.SetUpDate)
                {
                    return instance.PropertyFeed.SetUpDate;
                }

                return setUpdate;
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running GetPropSetUpDate in the Output class.";
                _sqlLogger.Error($"{instance.InstanceId}, {propID} - {ex}");
                return DateTime.Now;
            }
        }
    }
}
