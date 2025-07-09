
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using SchoolsDb_DataAccess;
using Newtonsoft.Json;

namespace SchoolsDb_DataLayer
{
    public class clsالجداول_الزمنيةData
    {
        //#nullable enable

        public static bool Getالجداول_الزمنيةInfoByID(int? معرّف_الجدول , ref int? معرّف_المعلم, ref int? معرّف_الفصل, ref int? معرّف_المادة, ref DateTime? اليوم, ref TimeSpan? وقت_البداية, ref TimeSpan? وقت_النهاية)
{
    bool isFound = false;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_الجداول_الزمنية_ByID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Ensure correct parameter assignment
                command.Parameters.AddWithValue("@معرّف_الجدول", معرّف_الجدول ?? (object)DBNull.Value);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                { 
                    if (reader.Read())
                    {
                        // The record was found
                        isFound = true;

                                معرّف_المعلم = (int)reader["معرّف_المعلم"];
                                معرّف_الفصل = (int)reader["معرّف_الفصل"];
                                معرّف_المادة = (int)reader["معرّف_المادة"];
                                اليوم = (DateTime)reader["اليوم"];
                                وقت_البداية = (TimeSpan)reader["وقت_البداية"];
                                وقت_النهاية = (TimeSpan)reader["وقت_النهاية"];

                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way
        ErrorHandler.HandleException(ex, nameof(Getالجداول_الزمنيةInfoByID), $"Parameter: معرّف_الجدول = " + معرّف_الجدول);
    }

    return isFound;
}

        public static DataTable GetAllالجداول_الزمنية()
{
    DataTable dt = new DataTable();

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_All_الجداول_الزمنية";

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
        ErrorHandler.HandleException(ex, nameof(GetAllالجداول_الزمنية), "No parameters for this method.");
    }

    return dt;
}

        public static int? AddNewالجداول_الزمنية(int? معرّف_المعلم, int? معرّف_الفصل, int? معرّف_المادة, DateTime? اليوم, TimeSpan? وقت_البداية, TimeSpan? وقت_النهاية)
    {
        int? معرّف_الجدول = null;

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SP_Add_الجداول_الزمنية";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@معرّف_المعلم", معرّف_المعلم);
                    command.Parameters.AddWithValue("@معرّف_الفصل", معرّف_الفصل);
                    command.Parameters.AddWithValue("@معرّف_المادة", معرّف_المادة);
                    command.Parameters.AddWithValue("@اليوم", اليوم);
                    command.Parameters.AddWithValue("@وقت_البداية", وقت_البداية);
                    command.Parameters.AddWithValue("@وقت_النهاية", وقت_النهاية);


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
                        معرّف_الجدول = (int)outputIdParam.Value;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            // Handle all exceptions in a general way
            ErrorHandler.HandleException(ex, nameof(AddNewالجداول_الزمنية), $"Parameters: int? معرّف_المعلم, int? معرّف_الفصل, int? معرّف_المادة, DateTime? اليوم, TimeSpan? وقت_البداية, TimeSpan? وقت_النهاية");
        }

        return معرّف_الجدول;
    }

        public static bool Updateالجداول_الزمنيةByID(int? معرّف_الجدول, int? معرّف_المعلم, int? معرّف_الفصل, int? معرّف_المادة, DateTime? اليوم, TimeSpan? وقت_البداية, TimeSpan? وقت_النهاية)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Update_الجداول_الزمنية_ByID"; 

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Create the parameters for the stored procedure
                    command.Parameters.AddWithValue("@معرّف_الجدول", معرّف_الجدول);
                    command.Parameters.AddWithValue("@معرّف_المعلم", معرّف_المعلم);
                    command.Parameters.AddWithValue("@معرّف_الفصل", معرّف_الفصل);
                    command.Parameters.AddWithValue("@معرّف_المادة", معرّف_المادة);
                    command.Parameters.AddWithValue("@اليوم", اليوم);
                    command.Parameters.AddWithValue("@وقت_البداية", وقت_البداية);
                    command.Parameters.AddWithValue("@وقت_النهاية", وقت_النهاية);


                // Open the connection and execute the update
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle exceptions
        ErrorHandler.HandleException(ex, nameof(Updateالجداول_الزمنيةByID), $"Parameter: معرّف_الجدول = " + معرّف_الجدول);
    }

    return (rowsAffected > 0);
}

        public static bool Deleteالجداول_الزمنية(int معرّف_الجدول)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Delete_الجداول_الزمنية_ByID";  

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@معرّف_الجدول", معرّف_الجدول);

                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way, this includes errors from SP_HandleError if any
        ErrorHandler.HandleException(ex, nameof(Deleteالجداول_الزمنية), $"Parameter: معرّف_الجدول = " + معرّف_الجدول);
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
            string query = $@"SP_Search_الجداول_الزمنية_ByColumn";

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
