
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using SchoolsDb_DataAccess;
using Newtonsoft.Json;

namespace SchoolsDb_DataLayer
{
    public class clsالحضورData
    {
        //#nullable enable

        public static bool GetالحضورInfoByID(int? معرّف_الحضور , ref int? معرّف_الطالب, ref int? معرّف_الفصل, ref DateTime? تاريخ_الحضور, ref string الحالة)
{
    bool isFound = false;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_الحضور_ByID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Ensure correct parameter assignment
                command.Parameters.AddWithValue("@معرّف_الحضور", معرّف_الحضور ?? (object)DBNull.Value);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                { 
                    if (reader.Read())
                    {
                        // The record was found
                        isFound = true;

                                معرّف_الطالب = (int)reader["معرّف_الطالب"];
                                معرّف_الفصل = (int)reader["معرّف_الفصل"];
                                تاريخ_الحضور = reader["تاريخ_الحضور"] != DBNull.Value ? (DateTime?)reader["تاريخ_الحضور"] : null;
                                الحالة = reader["الحالة"] != DBNull.Value ? reader["الحالة"].ToString() : null;

                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way
        ErrorHandler.HandleException(ex, nameof(GetالحضورInfoByID), $"Parameter: معرّف_الحضور = " + معرّف_الحضور);
    }

    return isFound;
}

        public static DataTable GetAllالحضور()
{
    DataTable dt = new DataTable();

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_All_الحضور";

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
        ErrorHandler.HandleException(ex, nameof(GetAllالحضور), "No parameters for this method.");
    }

    return dt;
}

        public static int? AddNewالحضور(int? معرّف_الطالب, int? معرّف_الفصل, DateTime? تاريخ_الحضور, string الحالة = null)
    {
        int? معرّف_الحضور = null;

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SP_Add_الحضور";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@معرّف_الطالب", معرّف_الطالب);
                    command.Parameters.AddWithValue("@معرّف_الفصل", معرّف_الفصل);
                    command.Parameters.AddWithValue("@تاريخ_الحضور", تاريخ_الحضور ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@الحالة", الحالة ?? (object)DBNull.Value);


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
                        معرّف_الحضور = (int)outputIdParam.Value;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            // Handle all exceptions in a general way
            ErrorHandler.HandleException(ex, nameof(AddNewالحضور), $"Parameters: int? معرّف_الطالب, int? معرّف_الفصل, DateTime? تاريخ_الحضور, string الحالة = null");
        }

        return معرّف_الحضور;
    }

        public static bool UpdateالحضورByID(int? معرّف_الحضور, int? معرّف_الطالب, int? معرّف_الفصل, DateTime? تاريخ_الحضور, string الحالة = null)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Update_الحضور_ByID"; 

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Create the parameters for the stored procedure
                    command.Parameters.AddWithValue("@معرّف_الحضور", معرّف_الحضور);
                    command.Parameters.AddWithValue("@معرّف_الطالب", معرّف_الطالب);
                    command.Parameters.AddWithValue("@معرّف_الفصل", معرّف_الفصل);
                    command.Parameters.AddWithValue("@تاريخ_الحضور", تاريخ_الحضور ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@الحالة", الحالة ?? (object)DBNull.Value);


                // Open the connection and execute the update
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle exceptions
        ErrorHandler.HandleException(ex, nameof(UpdateالحضورByID), $"Parameter: معرّف_الحضور = " + معرّف_الحضور);
    }

    return (rowsAffected > 0);
}

        public static bool Deleteالحضور(int معرّف_الحضور)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Delete_الحضور_ByID";  

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@معرّف_الحضور", معرّف_الحضور);

                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way, this includes errors from SP_HandleError if any
        ErrorHandler.HandleException(ex, nameof(Deleteالحضور), $"Parameter: معرّف_الحضور = " + معرّف_الحضور);
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
            string query = $@"SP_Search_الحضور_ByColumn";

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
