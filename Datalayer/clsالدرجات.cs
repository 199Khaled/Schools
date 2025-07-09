
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using SchoolsDb_DataAccess;
using Newtonsoft.Json;

namespace SchoolsDb_DataLayer
{
    public class clsالدرجاتData
    {
        //#nullable enable

        public static bool GetالدرجاتInfoByID(int? معرّف_الدرجة , ref int? معرّف_الطالب, ref int? معرّف_مادة_الصف, ref decimal? الدرجة, ref DateTime? تاريخ_الدرجة)
{
    bool isFound = false;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_الدرجات_ByID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Ensure correct parameter assignment
                command.Parameters.AddWithValue("@معرّف_الدرجة", معرّف_الدرجة ?? (object)DBNull.Value);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                { 
                    if (reader.Read())
                    {
                        // The record was found
                        isFound = true;

                                معرّف_الطالب = (int)reader["معرّف_الطالب"];
                                معرّف_مادة_الصف = (int)reader["معرّف_مادة_الصف"];
                                الدرجة = (decimal)reader["الدرجة"];
                                تاريخ_الدرجة = (DateTime)reader["تاريخ_الدرجة"];

                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way
        ErrorHandler.HandleException(ex, nameof(GetالدرجاتInfoByID), $"Parameter: معرّف_الدرجة = " + معرّف_الدرجة);
    }

    return isFound;
}

        public static DataTable GetAllالدرجات()
{
    DataTable dt = new DataTable();

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_All_الدرجات";

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
        ErrorHandler.HandleException(ex, nameof(GetAllالدرجات), "No parameters for this method.");
    }

    return dt;
}

        public static int? AddNewالدرجات(int? معرّف_الطالب, int? معرّف_مادة_الصف, decimal? الدرجة, DateTime? تاريخ_الدرجة)
    {
        int? معرّف_الدرجة = null;

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SP_Add_الدرجات";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@معرّف_الطالب", معرّف_الطالب);
                    command.Parameters.AddWithValue("@معرّف_مادة_الصف", معرّف_مادة_الصف);
                    command.Parameters.AddWithValue("@الدرجة", الدرجة);
                    command.Parameters.AddWithValue("@تاريخ_الدرجة", تاريخ_الدرجة);


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
                        معرّف_الدرجة = (int)outputIdParam.Value;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            // Handle all exceptions in a general way
            ErrorHandler.HandleException(ex, nameof(AddNewالدرجات), $"Parameters: int? معرّف_الطالب, int? معرّف_مادة_الصف, decimal? الدرجة, DateTime? تاريخ_الدرجة");
        }

        return معرّف_الدرجة;
    }

        public static bool UpdateالدرجاتByID(int? معرّف_الدرجة, int? معرّف_الطالب, int? معرّف_مادة_الصف, decimal? الدرجة, DateTime? تاريخ_الدرجة)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Update_الدرجات_ByID"; 

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Create the parameters for the stored procedure
                    command.Parameters.AddWithValue("@معرّف_الدرجة", معرّف_الدرجة);
                    command.Parameters.AddWithValue("@معرّف_الطالب", معرّف_الطالب);
                    command.Parameters.AddWithValue("@معرّف_مادة_الصف", معرّف_مادة_الصف);
                    command.Parameters.AddWithValue("@الدرجة", الدرجة);
                    command.Parameters.AddWithValue("@تاريخ_الدرجة", تاريخ_الدرجة);


                // Open the connection and execute the update
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle exceptions
        ErrorHandler.HandleException(ex, nameof(UpdateالدرجاتByID), $"Parameter: معرّف_الدرجة = " + معرّف_الدرجة);
    }

    return (rowsAffected > 0);
}

        public static bool Deleteالدرجات(int معرّف_الدرجة)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Delete_الدرجات_ByID";  

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@معرّف_الدرجة", معرّف_الدرجة);

                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way, this includes errors from SP_HandleError if any
        ErrorHandler.HandleException(ex, nameof(Deleteالدرجات), $"Parameter: معرّف_الدرجة = " + معرّف_الدرجة);
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
            string query = $@"SP_Search_الدرجات_ByColumn";

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
