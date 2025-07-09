
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using SchoolsDb_DataAccess;
using Newtonsoft.Json;

namespace SchoolsDb_DataLayer
{
    public class clsسجل_الأخطاءData
    {
        //#nullable enable

        public static bool Getسجل_الأخطاءInfoByID(int? معرّف_الخطأ , ref string رسالة_الخطأ, ref string تفاصيل_التتبع, ref DateTime? الطابع_الزمني, ref string شدة_الخطأ, ref string معلومات_إضافية)
{
    bool isFound = false;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_سجل_الأخطاء_ByID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Ensure correct parameter assignment
                command.Parameters.AddWithValue("@معرّف_الخطأ", معرّف_الخطأ ?? (object)DBNull.Value);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                { 
                    if (reader.Read())
                    {
                        // The record was found
                        isFound = true;

                                رسالة_الخطأ = (string)reader["رسالة_الخطأ"];
                                تفاصيل_التتبع = reader["تفاصيل_التتبع"] != DBNull.Value ? reader["تفاصيل_التتبع"].ToString() : null;
                                الطابع_الزمني = reader["الطابع_الزمني"] != DBNull.Value ? (DateTime?)reader["الطابع_الزمني"] : null;
                                شدة_الخطأ = reader["شدة_الخطأ"] != DBNull.Value ? reader["شدة_الخطأ"].ToString() : null;
                                معلومات_إضافية = reader["معلومات_إضافية"] != DBNull.Value ? reader["معلومات_إضافية"].ToString() : null;

                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way
        ErrorHandler.HandleException(ex, nameof(Getسجل_الأخطاءInfoByID), $"Parameter: معرّف_الخطأ = " + معرّف_الخطأ);
    }

    return isFound;
}

        public static DataTable GetAllسجل_الأخطاء()
{
    DataTable dt = new DataTable();

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_All_سجل_الأخطاء";

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
        ErrorHandler.HandleException(ex, nameof(GetAllسجل_الأخطاء), "No parameters for this method.");
    }

    return dt;
}

        public static int? AddNewسجل_الأخطاء(string رسالة_الخطأ, DateTime? الطابع_الزمني, string تفاصيل_التتبع = null, string شدة_الخطأ = null, string معلومات_إضافية = null)
    {
        int? معرّف_الخطأ = null;

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SP_Add_سجل_الأخطاء";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@رسالة_الخطأ", رسالة_الخطأ);
                    command.Parameters.AddWithValue("@تفاصيل_التتبع", تفاصيل_التتبع ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@الطابع_الزمني", الطابع_الزمني ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@شدة_الخطأ", شدة_الخطأ ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@معلومات_إضافية", معلومات_إضافية ?? (object)DBNull.Value);


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
                        معرّف_الخطأ = (int)outputIdParam.Value;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            // Handle all exceptions in a general way
            ErrorHandler.HandleException(ex, nameof(AddNewسجل_الأخطاء), $"Parameters: string رسالة_الخطأ, DateTime? الطابع_الزمني, string تفاصيل_التتبع = null, string شدة_الخطأ = null, string معلومات_إضافية = null");
        }

        return معرّف_الخطأ;
    }

        public static bool Updateسجل_الأخطاءByID(int? معرّف_الخطأ, string رسالة_الخطأ, DateTime? الطابع_الزمني, string تفاصيل_التتبع = null, string شدة_الخطأ = null, string معلومات_إضافية = null)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Update_سجل_الأخطاء_ByID"; 

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Create the parameters for the stored procedure
                    command.Parameters.AddWithValue("@معرّف_الخطأ", معرّف_الخطأ);
                    command.Parameters.AddWithValue("@رسالة_الخطأ", رسالة_الخطأ);
                    command.Parameters.AddWithValue("@تفاصيل_التتبع", تفاصيل_التتبع ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@الطابع_الزمني", الطابع_الزمني ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@شدة_الخطأ", شدة_الخطأ ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@معلومات_إضافية", معلومات_إضافية ?? (object)DBNull.Value);


                // Open the connection and execute the update
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle exceptions
        ErrorHandler.HandleException(ex, nameof(Updateسجل_الأخطاءByID), $"Parameter: معرّف_الخطأ = " + معرّف_الخطأ);
    }

    return (rowsAffected > 0);
}

        public static bool Deleteسجل_الأخطاء(int معرّف_الخطأ)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Delete_سجل_الأخطاء_ByID";  

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@معرّف_الخطأ", معرّف_الخطأ);

                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way, this includes errors from SP_HandleError if any
        ErrorHandler.HandleException(ex, nameof(Deleteسجل_الأخطاء), $"Parameter: معرّف_الخطأ = " + معرّف_الخطأ);
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
            string query = $@"SP_Search_سجل_الأخطاء_ByColumn";

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
