
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using SchoolsDb_DataAccess;
using Newtonsoft.Json;

namespace SchoolsDb_DataLayer
{
    public class clsمواد_الصفوفData
    {
        //#nullable enable

        public static bool Getمواد_الصفوفInfoByID(int? معرّف_مادة_الصف , ref int? معرّف_الصف, ref int? معرّف_المادة, ref int? معرّف_المعلم)
{
    bool isFound = false;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_مواد_الصفوف_ByID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Ensure correct parameter assignment
                command.Parameters.AddWithValue("@معرّف_مادة_الصف", معرّف_مادة_الصف ?? (object)DBNull.Value);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                { 
                    if (reader.Read())
                    {
                        // The record was found
                        isFound = true;

                                معرّف_الصف = (int)reader["معرّف_الصف"];
                                معرّف_المادة = (int)reader["معرّف_المادة"];
                                معرّف_المعلم = reader["معرّف_المعلم"] != DBNull.Value ? (int?)reader["معرّف_المعلم"] : null;

                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way
        ErrorHandler.HandleException(ex, nameof(Getمواد_الصفوفInfoByID), $"Parameter: معرّف_مادة_الصف = " + معرّف_مادة_الصف);
    }

    return isFound;
}

        public static DataTable GetAllمواد_الصفوف()
{
    DataTable dt = new DataTable();

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_All_مواد_الصفوف";

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
        ErrorHandler.HandleException(ex, nameof(GetAllمواد_الصفوف), "No parameters for this method.");
    }

    return dt;
}

        public static int? AddNewمواد_الصفوف(int? معرّف_الصف, int? معرّف_المادة, int? معرّف_المعلم)
    {
        int? معرّف_مادة_الصف = null;

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SP_Add_مواد_الصفوف";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@معرّف_الصف", معرّف_الصف);
                    command.Parameters.AddWithValue("@معرّف_المادة", معرّف_المادة);
                    command.Parameters.AddWithValue("@معرّف_المعلم", معرّف_المعلم ?? (object)DBNull.Value);


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
                        معرّف_مادة_الصف = (int)outputIdParam.Value;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            // Handle all exceptions in a general way
            ErrorHandler.HandleException(ex, nameof(AddNewمواد_الصفوف), $"Parameters: int? معرّف_الصف, int? معرّف_المادة, int? معرّف_المعلم");
        }

        return معرّف_مادة_الصف;
    }

        public static bool Updateمواد_الصفوفByID(int? معرّف_مادة_الصف, int? معرّف_الصف, int? معرّف_المادة, int? معرّف_المعلم)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Update_مواد_الصفوف_ByID"; 

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Create the parameters for the stored procedure
                    command.Parameters.AddWithValue("@معرّف_مادة_الصف", معرّف_مادة_الصف);
                    command.Parameters.AddWithValue("@معرّف_الصف", معرّف_الصف);
                    command.Parameters.AddWithValue("@معرّف_المادة", معرّف_المادة);
                    command.Parameters.AddWithValue("@معرّف_المعلم", معرّف_المعلم ?? (object)DBNull.Value);


                // Open the connection and execute the update
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle exceptions
        ErrorHandler.HandleException(ex, nameof(Updateمواد_الصفوفByID), $"Parameter: معرّف_مادة_الصف = " + معرّف_مادة_الصف);
    }

    return (rowsAffected > 0);
}

        public static bool Deleteمواد_الصفوف(int معرّف_مادة_الصف)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Delete_مواد_الصفوف_ByID";  

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@معرّف_مادة_الصف", معرّف_مادة_الصف);

                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way, this includes errors from SP_HandleError if any
        ErrorHandler.HandleException(ex, nameof(Deleteمواد_الصفوف), $"Parameter: معرّف_مادة_الصف = " + معرّف_مادة_الصف);
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
            string query = $@"SP_Search_مواد_الصفوف_ByColumn";

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
