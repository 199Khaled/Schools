
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using SchoolsDb_DataAccess;
using Newtonsoft.Json;

namespace SchoolsDb_DataLayer
{
    public class clsالمدفوعاتData
    {
        //#nullable enable

        public static bool GetالمدفوعاتInfoByID(int? معرّف_المدفوعات , ref int? معرّف_الطالب, ref decimal? المبلغ, ref DateTime? تاريخ_الدفع, ref string الحالة)
{
    bool isFound = false;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_المدفوعات_ByID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Ensure correct parameter assignment
                command.Parameters.AddWithValue("@معرّف_المدفوعات", معرّف_المدفوعات ?? (object)DBNull.Value);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                { 
                    if (reader.Read())
                    {
                        // The record was found
                        isFound = true;

                                معرّف_الطالب = (int)reader["معرّف_الطالب"];
                                المبلغ = (decimal)reader["المبلغ"];
                                تاريخ_الدفع = reader["تاريخ_الدفع"] != DBNull.Value ? (DateTime?)reader["تاريخ_الدفع"] : null;
                                الحالة = reader["الحالة"] != DBNull.Value ? reader["الحالة"].ToString() : null;

                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way
        ErrorHandler.HandleException(ex, nameof(GetالمدفوعاتInfoByID), $"Parameter: معرّف_المدفوعات = " + معرّف_المدفوعات);
    }

    return isFound;
}

        public static DataTable GetAllالمدفوعات()
{
    DataTable dt = new DataTable();

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_All_المدفوعات";

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
        ErrorHandler.HandleException(ex, nameof(GetAllالمدفوعات), "No parameters for this method.");
    }

    return dt;
}

        public static int? AddNewالمدفوعات(int? معرّف_الطالب, decimal? المبلغ, DateTime? تاريخ_الدفع, string الحالة = null)
    {
        int? معرّف_المدفوعات = null;

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SP_Add_المدفوعات";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@معرّف_الطالب", معرّف_الطالب);
                    command.Parameters.AddWithValue("@المبلغ", المبلغ);
                    command.Parameters.AddWithValue("@تاريخ_الدفع", تاريخ_الدفع ?? (object)DBNull.Value);
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
                        معرّف_المدفوعات = (int)outputIdParam.Value;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            // Handle all exceptions in a general way
            ErrorHandler.HandleException(ex, nameof(AddNewالمدفوعات), $"Parameters: int? معرّف_الطالب, decimal? المبلغ, DateTime? تاريخ_الدفع, string الحالة = null");
        }

        return معرّف_المدفوعات;
    }

        public static bool UpdateالمدفوعاتByID(int? معرّف_المدفوعات, int? معرّف_الطالب, decimal? المبلغ, DateTime? تاريخ_الدفع, string الحالة = null)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Update_المدفوعات_ByID"; 

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Create the parameters for the stored procedure
                    command.Parameters.AddWithValue("@معرّف_المدفوعات", معرّف_المدفوعات);
                    command.Parameters.AddWithValue("@معرّف_الطالب", معرّف_الطالب);
                    command.Parameters.AddWithValue("@المبلغ", المبلغ);
                    command.Parameters.AddWithValue("@تاريخ_الدفع", تاريخ_الدفع ?? (object)DBNull.Value);
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
        ErrorHandler.HandleException(ex, nameof(UpdateالمدفوعاتByID), $"Parameter: معرّف_المدفوعات = " + معرّف_المدفوعات);
    }

    return (rowsAffected > 0);
}

        public static bool Deleteالمدفوعات(int معرّف_المدفوعات)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Delete_المدفوعات_ByID";  

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@معرّف_المدفوعات", معرّف_المدفوعات);

                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way, this includes errors from SP_HandleError if any
        ErrorHandler.HandleException(ex, nameof(Deleteالمدفوعات), $"Parameter: معرّف_المدفوعات = " + معرّف_المدفوعات);
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
            string query = $@"SP_Search_المدفوعات_ByColumn";

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
