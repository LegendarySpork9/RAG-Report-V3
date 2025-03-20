using System.Data.SqlClient;
using log4net;
using RAG_Report_V3.Models;

namespace RAG_Report_V3.Stages
{
    internal class Instance_Delete
    {
        static readonly ILog _logger = LogManager.GetLogger("Log");
        static readonly ILog _sqlLogger = LogManager.GetLogger("SQLLog");

        // Sets the status of the Instances on BYMReporting to 'Deleted'.
        public string DeleteInstances()
        {
            _logger.Info("Setting all Instances to Deleted.");

            try
            {
                SqlConnection connection;
                SqlCommand command;

                string sqlQuery = "update Table set Status = 'Deleted'";
                int rowsAffected;

                connection = new SqlConnection(App_Settings_Model.OutputConnectionString);
                connection.Open();
                command = new SqlCommand(sqlQuery, connection);
                rowsAffected = command.ExecuteNonQuery();

                _logger.Info("All Instance Statuses Updated.");
                return "Success";
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running DeleteInstances in the Instance_Delete class.";
                _sqlLogger.Error(ex);
                return "Failed";
            }
        }
    }
}
