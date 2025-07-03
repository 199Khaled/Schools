
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using SchoolsDb_DataAccess;
using Newtonsoft.Json;

namespace SchoolsDb_DataLayer
{
    public class clsSchedulesData
    {
        //#nullable enable

        public static bool GetSchedulesInfoByID(int? ScheduleID , ref int? TeacherID, ref int? ClassroomID, ref int? SubjectID, ref DateTime Day, ref TimeSpan? StartTime, ref TimeSpan? EndTime)
{
    bool isFound = false;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_Schedules_ByID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Ensure correct parameter assignment
                command.Parameters.AddWithValue("@ScheduleID", ScheduleID ?? (object)DBNull.Value);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                { 
                    if (reader.Read())
                    {
                        // The record was found
                        isFound = true;

                                TeacherID = reader["TeacherID"] != DBNull.Value ? (int?)reader["TeacherID"] : null;
                                ClassroomID = reader["ClassroomID"] != DBNull.Value ? (int?)reader["ClassroomID"] : null;
                                SubjectID = reader["SubjectID"] != DBNull.Value ? (int?)reader["SubjectID"] : null;
                                Day = (DateTime)reader["Day"];
                                StartTime = reader["StartTime"] != DBNull.Value ? (TimeSpan?)reader["StartTime"] : null;
                                EndTime = reader["EndTime"] != DBNull.Value ? (TimeSpan?)reader["EndTime"] : null;

                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way
        ErrorHandler.HandleException(ex, nameof(GetSchedulesInfoByID), $"Parameter: ScheduleID = " + ScheduleID);
    }

    return isFound;
}

        public static DataTable GetAllSchedules()
{
    DataTable dt = new DataTable();

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_All_Schedules";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure; 

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way
        ErrorHandler.HandleException(ex, nameof(GetAllSchedules), "No parameters for this method.");
    }

    return dt;
}

        public static int? AddNewSchedules(int? TeacherID, int? ClassroomID, int? SubjectID, DateTime? Day, TimeSpan? StartTime, TimeSpan? EndTime)
    {
        int? ScheduleID = null;

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SP_Add_Schedules";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@TeacherID", TeacherID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ClassroomID", ClassroomID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@SubjectID", SubjectID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Day", Day);
                    command.Parameters.AddWithValue("@StartTime", StartTime ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EndTime", EndTime ?? (object)DBNull.Value);


                    SqlParameter outputIdParam = new SqlParameter("@NewID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(outputIdParam);

                    connection.Open();
                    command.ExecuteNonQuery();

                    // Bring added value
                    if (outputIdParam.Value != DBNull.Value)
                    {
                        ScheduleID = (int)outputIdParam.Value;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            // Handle all exceptions in a general way
            ErrorHandler.HandleException(ex, nameof(AddNewSchedules), $"Parameters: int? TeacherID, int? ClassroomID, int? SubjectID, TimeSpan? StartTime, TimeSpan? EndTime, string Day = null");
        }

        return ScheduleID;
    }

        public static bool UpdateSchedulesByID(int? ScheduleID, int? TeacherID, int? ClassroomID, int? SubjectID, DateTime? Day,TimeSpan? StartTime, TimeSpan? EndTime)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Update_Schedules_ByID"; 

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Create the parameters for the stored procedure
                    command.Parameters.AddWithValue("@ScheduleID", ScheduleID);
                    command.Parameters.AddWithValue("@TeacherID", TeacherID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ClassroomID", ClassroomID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@SubjectID", SubjectID ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@Day", Day);
                    command.Parameters.AddWithValue("@StartTime", StartTime ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@EndTime", EndTime ?? (object)DBNull.Value);


                // Open the connection and execute the update
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle exceptions
        ErrorHandler.HandleException(ex, nameof(UpdateSchedulesByID), $"Parameter: ScheduleID = " + ScheduleID);
    }

    return (rowsAffected > 0);
}

        public static bool DeleteSchedules(int ScheduleID)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Delete_Schedules_ByID";  

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ScheduleID", ScheduleID);

                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way, this includes errors from SP_HandleError if any
        ErrorHandler.HandleException(ex, nameof(DeleteSchedules), $"Parameter: ScheduleID = " + ScheduleID);
    }

    return (rowsAffected > 0);
}
        
        public static DataTable SearchData(string ColumnName, string SearchValue, string Mode = "Anywhere")
{
    DataTable dt = new DataTable();

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Search_Schedules_ByColumn";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@ColumnName", ColumnName);
                command.Parameters.AddWithValue("@SearchValue", SearchValue);
                command.Parameters.AddWithValue("@Mode", Mode);  // Added Mode parameter

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        dt.Load(reader);
                    }

                    reader.Close();
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way
        ErrorHandler.HandleException(ex, nameof(SearchData), $"ColumnName: {ColumnName}, SearchValue: {SearchValue}, Mode: {Mode}");
    }

    return dt;
}
    }
}
