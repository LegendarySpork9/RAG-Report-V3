using log4net;
using System.Data.SqlClient;
using RAG_Report_V3.Models;

namespace RAG_Report_V3.Stages
{
    internal class Compare_Exclusion
    {
        static readonly ILog _logger = LogManager.GetLogger("Log");
        static readonly ILog _sqlLogger = LogManager.GetLogger("SQLLog");

        // Compares the input exclusion to the output exclusion.
        public string CompareExclusion(Instance_Model instance)
        {
            string[] name = Array.Empty<string>();

            var existingExclusions = GetExistingExclusion(instance);

            if (existingExclusions.Item1.Count != 0)
            {
                _logger.Info($"{instance.SubDomain} - Comparing Existing Exclusions.");

                for (int x = 0; x < instance.Exclusions.Name.Count; x++)
                {
                    for (int y = 0; y < existingExclusions.Item1.Count; y++)
                    {
                        if (instance.Exclusions.Name[x] == existingExclusions.Item1[y])
                        {
                            if (DateOnly.Parse(DateTime.Now.ToString("yyyy-MM-dd")) < existingExclusions.Item2[y] &&
                                instance.Exclusions.ExcludeTillDate[x] == existingExclusions.Item2[y])
                            {
                                _logger.Debug($"{instance.SubDomain} - Exclusion for {instance.Exclusions.Name[x]} is valid, marking to ignore.");

                                name = name.Append(instance.Exclusions.Name[x]).ToArray();
                            }

                            if (instance.Exclusions.ExcludeTillDate[x] > existingExclusions.Item2[y])
                            {
                                _logger.Debug($"{instance.SubDomain} - Exclusion for {instance.Exclusions.Name[x]} is invalid, updating and marking to ignore.");

                                string result = Exclusion(instance.Exclusions.Type[x], instance.Exclusions.Name[x], instance.Exclusions.ExcludeTillDate[x],
                                    instance.Exclusions.ExcludeReason[x], instance.Exclusions.Comments[x], instance.InstanceId);

                                if (result != "Success")
                                {
                                    _logger.Warn($"{instance.SubDomain} - Updating exclusion for {instance.Exclusions.Name[x]} has failed.");
                                    return "Failed";
                                }

                                else
                                {
                                    _logger.Debug($"{instance.SubDomain} - Exclusion for {instance.Exclusions.Name[x]} has been updated.");
                                }

                                name = name.Append(instance.Exclusions.Name[x]).ToArray();
                            }
                        }
                    }
                }
            }

            if (instance.Exclusions != null)
            {
                foreach (string intName in instance.Exclusions.Name)
                {
                    if (!name.Contains(intName))
                    {
                        _logger.Debug($"{instance.SubDomain} - No existing exclusions found for: {intName}");

                        int x = instance.Exclusions.Name.IndexOf(intName);

                        string result = Exclusion(instance.Exclusions.Type[x], intName, instance.Exclusions.ExcludeTillDate[x],
                            instance.Exclusions.ExcludeReason[x], instance.Exclusions.Comments[x], instance.InstanceId);

                        if (result != "Success")
                        {
                            _logger.Warn($"{instance.SubDomain} - Adding exclusion for {intName} has failed.");
                            return "Failed";
                        }

                        else
                        {
                            _logger.Debug($"{instance.SubDomain} - Exclusion added for: {intName}");
                        }

                        name = name.Append(intName).ToArray();
                    }
                }
            }

            _logger.Info($"{instance.SubDomain} - Comparisons of Exclusions is complete.");
            return "Success";
        }

        // Gets the existing eclusions for the given ID.
        private static (List<string>, List<DateOnly>) GetExistingExclusion(Instance_Model instance)
        {
            _logger.Info($"{instance.SubDomain} - Getting Existing Exclusions.");

            try
            {
                SqlConnection connection;
                SqlCommand command;
                SqlDataReader dataReader;

                string sqlQuery = @"with CountCTE as (
	select count(*) as ExclusionCount from Table with (nolock)
	where InstanceID = @InstanceID
)

select cast(ExclusionCount as varchar), null from CountCTE with (nolock)
union all
select Name, ExcludeTillDate from Table with (nolock)
where InstanceID = @InstanceID
and (select ExclusionCount from CountCTE with (nolock)) > 0";
                bool firstRow = true;
                List<string> name = new();
                List<DateOnly> date = new();

                connection = new SqlConnection(App_Settings_Model.OutputConnectionString);
                connection.Open();
                command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add(new SqlParameter("@InstanceID", instance.InstanceId));
                dataReader = command.ExecuteReader();

                while (dataReader.Read())
                {
                    if (firstRow)
                    {
                        if (int.Parse(dataReader.GetString(0)) == 0)
                        {
                            _logger.Info($"{instance.SubDomain} - No Existing Exclusions.");

                            dataReader.Close();
                            connection.Close();

                            return (new List<string>(), new List<DateOnly>());
                        }

                        else
                        {
                            firstRow = false;
                        }
                    }

                    else
                    {
                        name.Add(dataReader.GetString(0));
                        date.Add(DateOnly.Parse(dataReader.GetDateTime(1).ToString("yyyy-MM-dd")));

                        _logger.Debug($"{instance.SubDomain} - Exclusion Name: {dataReader.GetString(0)}");
                        _logger.Debug($"{instance.SubDomain} - Exclusion Date: {dataReader.GetDateTime(1):yyyy-MM-dd}");
                    }
                }

                dataReader.Close();
                connection.Close();

                _logger.Info($"{instance.SubDomain} - Existing Exclusions Obtained.");
                return (name, date);
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running GetExistingExclusion in the Compare_Exclusion class.";
                _sqlLogger.Error($"{instance.InstanceId} - {ex}");
                return (new List<string>(), new List<DateOnly>());
            }
        }

        // Adds/Updates the exclusion information for the given ID.
        private static string Exclusion(string type, string name, DateOnly date, string reason, string comment, int instanceID)
        {
            try
            {
                SqlConnection connection;
                SqlCommand command;

                string sqlQuery = @"if exists (
	select InstanceID from Table with (nolock) where InstanceID = @InstanceID and Name = @Name
)
begin
	update Table set ExcludeTillDate = @Date, ExclusionReason = @Reason, Comment = @Comment
    where InstanceID = @InstanceID
    and Name = @Name
end
else
begin
    insert into Table(InstanceID, Type, Name, ExcludeTillDate, ExclusionReason, Comment)
    values (@InstanceID, @Type, @Name, @Date, @Reason, @Comment)
end";
                int rowsAffected = 0;

                connection = new SqlConnection(App_Settings_Model.OutputConnectionString);
                connection.Open();
                command = new SqlCommand(sqlQuery, connection);
                command.Parameters.Add(new SqlParameter("@InstanceID", instanceID));
                command.Parameters.Add(new SqlParameter("@Type", type));
                command.Parameters.Add(new SqlParameter("@Name", name));
                command.Parameters.Add(new SqlParameter("@Date", date.ToString("yyyy-MM-dd")));
                command.Parameters.Add(new SqlParameter("@Reason", reason));
                command.Parameters.Add(new SqlParameter("@Comment", comment));
                rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected != 1)
                {
                    connection.Close();
                    return "Failed";
                }

                connection.Close();
                return "Success";
            }

            catch (Exception ex)
            {
                ThreadContext.Properties["Summary"] = "An error occured when running Exclusion in the Compare_Exclusion class.";
                _sqlLogger.Error($"{instanceID}, {type}, {name}, {date:yyyy-MM-dd}, {reason}, {comment} - {ex}");
                return "Failed";
            }
        }
    }
}
