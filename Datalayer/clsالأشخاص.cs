
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using SchoolsDb_DataAccess;
using Newtonsoft.Json;

namespace SchoolsDb_DataLayer
{
    public class clsالأشخاصData
    {
        //#nullable enable

        public static bool GetالأشخاصInfoByID(int? معرّف_الشخص , ref string الاسم_الأول, ref string اسم_الأب, ref string اسم_الأم, ref string اسم_العائلة, ref DateTime? تاريخ_الميلاد, ref string الجنس, ref string المدينة, ref string الهاتف, ref string البريد_الإلكتروني)
{
    bool isFound = false;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_الأشخاص_ByID";

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

                                الاسم_الأول = (string)reader["الاسم_الأول"];
                                اسم_الأب = reader["اسم_الأب"] != DBNull.Value ? reader["اسم_الأب"].ToString() : null;
                                اسم_الأم = reader["اسم_الأم"] != DBNull.Value ? reader["اسم_الأم"].ToString() : null;
                                اسم_العائلة = (string)reader["اسم_العائلة"];
                                تاريخ_الميلاد = reader["تاريخ_الميلاد"] != DBNull.Value ? (DateTime?)reader["تاريخ_الميلاد"] : null;
                                الجنس = reader["الجنس"] != DBNull.Value ? reader["الجنس"].ToString() : null;
                                المدينة = reader["المدينة"] != DBNull.Value ? reader["المدينة"].ToString() : null;
                                الهاتف = reader["الهاتف"] != DBNull.Value ? reader["الهاتف"].ToString() : null;
                                البريد_الإلكتروني = reader["البريد_الإلكتروني"] != DBNull.Value ? reader["البريد_الإلكتروني"].ToString() : null;

                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way
        ErrorHandler.HandleException(ex, nameof(GetالأشخاصInfoByID), $"Parameter: معرّف_الشخص = " + معرّف_الشخص);
    }

    return isFound;
}

        public static DataTable GetAllالأشخاص()
{
    DataTable dt = new DataTable();

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_All_الأشخاص";

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
        ErrorHandler.HandleException(ex, nameof(GetAllالأشخاص), "No parameters for this method.");
    }

    return dt;
}

        public static int? AddNewالأشخاص(string الاسم_الأول, string اسم_العائلة, DateTime? تاريخ_الميلاد, string اسم_الأب = null, string اسم_الأم = null,
            string الجنس = null, string المدينة = null, string الهاتف = null, string البريد_الإلكتروني = null)
    {
        int? معرّف_الشخص = null;

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SP_Add_الأشخاص";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@الاسم_الأول", الاسم_الأول);
                    command.Parameters.AddWithValue("@اسم_الأب", اسم_الأب ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@اسم_الأم", اسم_الأم ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@اسم_العائلة", اسم_العائلة);
                    command.Parameters.AddWithValue("@تاريخ_الميلاد", تاريخ_الميلاد ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@الجنس", الجنس ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@المدينة", المدينة ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@الهاتف", الهاتف ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@البريد_الإلكتروني", البريد_الإلكتروني ?? (object)DBNull.Value);


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
                        معرّف_الشخص = (int)outputIdParam.Value;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            // Handle all exceptions in a general way
            ErrorHandler.HandleException(ex, nameof(AddNewالأشخاص), $"Parameters: string الاسم_الأول, string اسم_العائلة, DateTime? تاريخ_الميلاد, string اسم_الأب = null, string اسم_الأم = null, string الجنس = null, string المدينة = null, string الهاتف = null, string البريد_الإلكتروني = null");
        }

        return معرّف_الشخص;
    }

        public static bool UpdateالأشخاصByID(int? معرّف_الشخص, string الاسم_الأول, string اسم_العائلة, DateTime? تاريخ_الميلاد,
            string اسم_الأب = null, string اسم_الأم = null,
        string الجنس = null, string المدينة = null, string الهاتف = null, string البريد_الإلكتروني = null)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Update_الأشخاص_ByID"; 

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Create the parameters for the stored procedure
                    command.Parameters.AddWithValue("@معرّف_الشخص", معرّف_الشخص);
                    command.Parameters.AddWithValue("@الاسم_الأول", الاسم_الأول);
                    command.Parameters.AddWithValue("@اسم_الأب", اسم_الأب ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@اسم_الأم", اسم_الأم ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@اسم_العائلة", اسم_العائلة);
                    command.Parameters.AddWithValue("@تاريخ_الميلاد", تاريخ_الميلاد ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@الجنس", الجنس ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@المدينة", المدينة ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@الهاتف", الهاتف ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@البريد_الإلكتروني", البريد_الإلكتروني ?? (object)DBNull.Value);


                // Open the connection and execute the update
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle exceptions
        ErrorHandler.HandleException(ex, nameof(UpdateالأشخاصByID), $"Parameter: معرّف_الشخص = " + معرّف_الشخص);
    }

    return (rowsAffected > 0);
}

        public static bool Deleteالأشخاص(int معرّف_الشخص)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Delete_الأشخاص_ByID";  

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@معرّف_الشخص", معرّف_الشخص);

                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way, this includes errors from SP_HandleError if any
        ErrorHandler.HandleException(ex, nameof(Deleteالأشخاص), $"Parameter: معرّف_الشخص = " + معرّف_الشخص);
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
            string query = $@"SP_Search_الأشخاص_ByColumn";

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
