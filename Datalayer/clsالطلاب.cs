
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using SchoolsDb_DataAccess;
using Newtonsoft.Json;

namespace SchoolsDb_DataLayer
{
    public class clsالطلابData
    {
        //#nullable enable

        public static bool GetالطلابInfoByID(int? معرّف_الطالب , ref int? معرّف_الشخص)
{
    bool isFound = false;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_الطلاب_ByID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Ensure correct parameter assignment
                command.Parameters.AddWithValue("@معرّف_الطالب", معرّف_الطالب ?? (object)DBNull.Value);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                { 
                    if (reader.Read())
                    {
                        // The record was found
                        isFound = true;

                                معرّف_الشخص = (int)reader["معرّف_الشخص"];
                              
                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way
        ErrorHandler.HandleException(ex, nameof(GetالطلابInfoByID), $"Parameter: معرّف_الطالب = " + معرّف_الطالب);
    }

    return isFound;
}


        public static bool GetالطلابInfoByPersonID(ref int? معرّف_الطالب, int? معرّف_الشخص)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"Select * from الطلاب where معرّف_الشخص = @معرّف_الشخص ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Ensure correct parameter assignment
                        command.Parameters.AddWithValue("@معرّف_الشخص", معرّف_الشخص ?? (object)DBNull.Value);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                معرّف_الطالب = (int)reader["معرّف_الطالب"];
                             

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle all exceptions in a general way
                ErrorHandler.HandleException(ex, nameof(GetالطلابInfoByID), $"Parameter: معرّف_الشخص = " + معرّف_الطالب);
            }

            return isFound;
        }

     
      public static DataTable GetAllالطلاب()
            {
                DataTable dt = new DataTable();

                try
                {
                    using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                    {
                        string query = @"
SELECT 
    الطلاب.[معرّف_الطالب]             AS [معرّف_الطالب],
    
    CASE 
        WHEN الأشخاص.[اسم_الأب] IS NULL OR الأشخاص.[اسم_الأب] = ''
            THEN الأشخاص.[الاسم_الأول] + ' ' + الأشخاص.[اسم_العائلة]
        ELSE 
            الأشخاص.[الاسم_الأول] + ' ' + الأشخاص.[اسم_الأب] + ' ' + الأشخاص.[اسم_العائلة]
    END AS [الاسم_الكامل],

    الأشخاص.[تاريخ_الميلاد]          AS [تاريخ الميلاد],
    الأشخاص.[الجنس]                 AS [الجنس],
    الأشخاص.[المدينة]               AS [المدينة],
    الأشخاص.[الهاتف]               AS [الهاتف],
    الأشخاص.[البريد_الإلكتروني]     AS [البريد الإلكتروني]


FROM 
    الطلاب
INNER JOIN 
    الأشخاص 
    ON الطلاب.[معرّف_الشخص] = الأشخاص.[معرّف_الشخص]
";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
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
                    ErrorHandler.HandleException(ex, nameof(GetAllالطلاب), "No parameters for this method.");
                }

                return dt;
        }
       

        public static int? AddNewالطلاب(int? معرّف_الشخص)
    {
        int? معرّف_الطالب = null;

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SP_Add_الطلاب";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@معرّف_الشخص", معرّف_الشخص);
     
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
                        معرّف_الطالب = (int)outputIdParam.Value;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            // Handle all exceptions in a general way
            ErrorHandler.HandleException(ex, nameof(AddNewالطلاب), $"Parameters: int? معرّف_الشخص, DateTime? تاريخ_الالتحاق");
        }

        return معرّف_الطالب;
    }

        public static bool UpdateالطلابByID(int? معرّف_الطالب, int? معرّف_الشخص )
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Update_الطلاب_ByID"; 

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Create the parameters for the stored procedure
                    command.Parameters.AddWithValue("@معرّف_الطالب", معرّف_الطالب);
                    command.Parameters.AddWithValue("@معرّف_الشخص", معرّف_الشخص);


                // Open the connection and execute the update
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle exceptions
        ErrorHandler.HandleException(ex, nameof(UpdateالطلابByID), $"Parameter: معرّف_الطالب = " + معرّف_الطالب);
    }

    return (rowsAffected > 0);
}

        public static bool Deleteالطلاب(int? معرّف_الطالب)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Delete_الطلاب_ByID";  

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@معرّف_الطالب", معرّف_الطالب);

                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way, this includes errors from SP_HandleError if any
        ErrorHandler.HandleException(ex, nameof(Deleteالطلاب), $"Parameter: معرّف_الطالب = " + معرّف_الطالب);
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
            string query = $@"SP_Search_الطلاب_ByColumn";

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
