
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using SchoolsDb_DataAccess;
using Newtonsoft.Json;

namespace SchoolsDb_DataLayer
{
    public class clsنتائج_الامتحانات_للطلابData
    {
        //#nullable enable

        public static bool Getنتائج_الامتحانات_للطلابInfoByID(int? معرّف_النتيجة , ref int? معرّف_الطالب, ref int? معرّف_الاختبار, ref decimal? الدرجة)
{
    bool isFound = false;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_نتائج_الامتحانات_للطلاب_ByID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Ensure correct parameter assignment
                command.Parameters.AddWithValue("@معرّف_النتيجة", معرّف_النتيجة ?? (object)DBNull.Value);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                { 
                    if (reader.Read())
                    {
                        // The record was found
                        isFound = true;

                                معرّف_الطالب = (int)reader["معرّف_الطالب"];
                                معرّف_الاختبار = (int)reader["معرّف_الاختبار"];
                                الدرجة = reader["الدرجة"] != DBNull.Value ? (decimal?)reader["الدرجة"] : null;

                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way
        ErrorHandler.HandleException(ex, nameof(Getنتائج_الامتحانات_للطلابInfoByID), $"Parameter: معرّف_النتيجة = " + معرّف_النتيجة);
    }

    return isFound;
}

        public static DataTable GetAllنتائج_الامتحانات_للطلاب()
{
    DataTable dt = new DataTable();

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_All_نتائج_الامتحانات_للطلاب";

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
        ErrorHandler.HandleException(ex, nameof(GetAllنتائج_الامتحانات_للطلاب), "No parameters for this method.");
    }

    return dt;
}

        public static int? AddNewنتائج_الامتحانات_للطلاب(int? معرّف_الطالب, int? معرّف_الاختبار, decimal? الدرجة)
    {
        int? معرّف_النتيجة = null;

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SP_Add_نتائج_الامتحانات_للطلاب";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@معرّف_الطالب", معرّف_الطالب);
                    command.Parameters.AddWithValue("@معرّف_الاختبار", معرّف_الاختبار);
                    command.Parameters.AddWithValue("@الدرجة", الدرجة ?? (object)DBNull.Value);


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
                        معرّف_النتيجة = (int)outputIdParam.Value;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            // Handle all exceptions in a general way
            ErrorHandler.HandleException(ex, nameof(AddNewنتائج_الامتحانات_للطلاب), $"Parameters: int? معرّف_الطالب, int? معرّف_الاختبار, decimal? الدرجة");
        }

        return معرّف_النتيجة;
    }

        public static bool Updateنتائج_الامتحانات_للطلابByID(int? معرّف_النتيجة, int? معرّف_الطالب, int? معرّف_الاختبار, decimal? الدرجة)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Update_نتائج_الامتحانات_للطلاب_ByID"; 

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Create the parameters for the stored procedure
                    command.Parameters.AddWithValue("@معرّف_النتيجة", معرّف_النتيجة);
                    command.Parameters.AddWithValue("@معرّف_الطالب", معرّف_الطالب);
                    command.Parameters.AddWithValue("@معرّف_الاختبار", معرّف_الاختبار);
                    command.Parameters.AddWithValue("@الدرجة", الدرجة ?? (object)DBNull.Value);


                // Open the connection and execute the update
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle exceptions
        ErrorHandler.HandleException(ex, nameof(Updateنتائج_الامتحانات_للطلابByID), $"Parameter: معرّف_النتيجة = " + معرّف_النتيجة);
    }

    return (rowsAffected > 0);
}

        public static bool Deleteنتائج_الامتحانات_للطلاب(int معرّف_النتيجة)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Delete_نتائج_الامتحانات_للطلاب_ByID";  

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@معرّف_النتيجة", معرّف_النتيجة);

                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way, this includes errors from SP_HandleError if any
        ErrorHandler.HandleException(ex, nameof(Deleteنتائج_الامتحانات_للطلاب), $"Parameter: معرّف_النتيجة = " + معرّف_النتيجة);
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
            string query = $@"SP_Search_نتائج_الامتحانات_للطلاب_ByColumn";

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
