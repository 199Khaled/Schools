
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using SchoolsDb_DataAccess;
using Newtonsoft.Json;

namespace SchoolsDb_DataLayer
{
    public class clsالموظفونData
    {
        //#nullable enable

        public static bool GetالموظفونInfoByID(int? معرّف_الموظف , ref int? معرّف_الشخص, ref string النوع, ref DateTime? تاريخ_التوظيف, ref string تاريخ_الإنهاء, ref bool? نشط)
{
    bool isFound = false;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_الموظفون_ByID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Ensure correct parameter assignment
                command.Parameters.AddWithValue("@معرّف_الموظف", معرّف_الموظف ?? (object)DBNull.Value);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                { 
                    if (reader.Read())
                    {
                        // The record was found
                        isFound = true;

                                معرّف_الشخص = (int)reader["معرّف_الشخص"];
                                النوع = (string)reader["النوع"];
                                تاريخ_التوظيف = (DateTime)reader["تاريخ_التوظيف"];
                                تاريخ_الإنهاء = reader["تاريخ_الإنهاء"] != DBNull.Value ? reader["تاريخ_الإنهاء"].ToString() : null;
                                نشط = reader["نشط"] != DBNull.Value ? (bool?)reader["نشط"] : null;

                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way
        ErrorHandler.HandleException(ex, nameof(GetالموظفونInfoByID), $"Parameter: معرّف_الموظف = " + معرّف_الموظف);
    }

    return isFound;
}

        public static bool GetالموظفونInfoByمعرّف_الشخص(ref int? معرّف_الموظف,  int? معرّف_الشخص, ref string النوع, ref DateTime? تاريخ_التوظيف, ref string تاريخ_الإنهاء, ref bool? نشط)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SELECT * FROM الموظفون WHERE معرّف_الشخص = @معرّف_الشخص";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Ensure correct parameter assignment
                        command.Parameters.AddWithValue("@معرّف_الشخص", معرّف_الشخص ?? (object)DBNull.Value);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                معرّف_الموظف = (int)reader["معرّف_الموظف"];
                                النوع = (string)reader["النوع"];
                                تاريخ_التوظيف = (DateTime)reader["تاريخ_التوظيف"];
                                تاريخ_الإنهاء = reader["تاريخ_الإنهاء"] != DBNull.Value ? reader["تاريخ_الإنهاء"].ToString() : null;
                                نشط = reader["نشط"] != DBNull.Value ? (bool?)reader["نشط"] : null;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle all exceptions in a general way
                ErrorHandler.HandleException(ex, nameof(GetالموظفونInfoByID), $"Parameter: معرّف_الموظف = " + معرّف_الموظف);
            }

            return isFound;
        }

        public static bool GetالموظفونInfoBy(int? معرّف_الموظف, ref int? معرّف_الشخص, ref string النوع, ref DateTime? تاريخ_التوظيف, ref string تاريخ_الإنهاء, ref bool? نشط)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = "SP_Get_الموظفون_ByID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Ensure correct parameter assignment
                        command.Parameters.AddWithValue("@معرّف_الموظف", معرّف_الموظف ?? (object)DBNull.Value);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                معرّف_الشخص = (int)reader["معرّف_الشخص"];
                                النوع = (string)reader["النوع"];
                                تاريخ_التوظيف = (DateTime)reader["تاريخ_التوظيف"];
                                تاريخ_الإنهاء = reader["تاريخ_الإنهاء"] != DBNull.Value ? reader["تاريخ_الإنهاء"].ToString() : null;
                                نشط = reader["نشط"] != DBNull.Value ? (bool?)reader["نشط"] : null;

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle all exceptions in a general way
                ErrorHandler.HandleException(ex, nameof(GetالموظفونInfoByID), $"Parameter: معرّف_الموظف = " + معرّف_الموظف);
            }

            return isFound;
        }
        public static DataTable GetAllالموظفون()
{
    DataTable dt = new DataTable();

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
                    // الأشخاص.[معرّف_الشخص]           AS [معرّف الشخص],
                    string query = @"

             SELECT 
    الموظفون.[معرّف_الموظف]          AS [معرّف_الموظف],
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
    الأشخاص.[البريد_الإلكتروني]     AS [البريد الإلكتروني],

    الموظفون.[النوع]                 AS [نوع الوظيفة],
    الموظفون.[تاريخ_التوظيف]         AS [تاريخ التوظيف],
    الموظفون.[تاريخ_الإنهاء]         AS [تاريخ الإنهاء],
    الموظفون.[نشط]                  AS [نشط]

FROM 
    الموظفون
INNER JOIN 
    الأشخاص 
ON 
    الموظفون.[معرّف_الشخص] = الأشخاص.[معرّف_الشخص]
";
                    using (SqlCommand command = new SqlCommand(query, connection))
            {
              //  command.CommandType = CommandType.StoredProcedure; 

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
        ErrorHandler.HandleException(ex, nameof(GetAllالموظفون), "No parameters for this method.");
    }

    return dt;
}

        public static int? AddNewالموظفون(int? معرّف_الشخص, string النوع, DateTime? تاريخ_التوظيف, bool? نشط, string تاريخ_الإنهاء = null)
    {
        int? معرّف_الموظف = null;

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SP_Add_الموظفون";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@معرّف_الشخص", معرّف_الشخص);
                    command.Parameters.AddWithValue("@النوع", النوع);
                    command.Parameters.AddWithValue("@تاريخ_التوظيف", تاريخ_التوظيف);
                    command.Parameters.AddWithValue("@تاريخ_الإنهاء", تاريخ_الإنهاء ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@نشط", نشط ?? (object)DBNull.Value);


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
                        معرّف_الموظف = (int)outputIdParam.Value;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            // Handle all exceptions in a general way
            ErrorHandler.HandleException(ex, nameof(AddNewالموظفون), $"Parameters: int? معرّف_الشخص, string النوع, DateTime? تاريخ_التوظيف, bool? نشط, string تاريخ_الإنهاء = null");
        }

        return معرّف_الموظف;
    }

        public static bool UpdateالموظفونByID(int? معرّف_الموظف, int? معرّف_الشخص, string النوع, DateTime? تاريخ_التوظيف, bool? نشط, string تاريخ_الإنهاء = null)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Update_الموظفون_ByID"; 

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Create the parameters for the stored procedure
                    command.Parameters.AddWithValue("@معرّف_الموظف", معرّف_الموظف);
                    command.Parameters.AddWithValue("@معرّف_الشخص", معرّف_الشخص);
                    command.Parameters.AddWithValue("@النوع", النوع);
                    command.Parameters.AddWithValue("@تاريخ_التوظيف", تاريخ_التوظيف);
                    command.Parameters.AddWithValue("@تاريخ_الإنهاء", تاريخ_الإنهاء ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@نشط", نشط ?? (object)DBNull.Value);


                // Open the connection and execute the update
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle exceptions
        ErrorHandler.HandleException(ex, nameof(UpdateالموظفونByID), $"Parameter: معرّف_الموظف = " + معرّف_الموظف);
    }

    return (rowsAffected > 0);
}

        public static bool Deleteالموظفون(int معرّف_الموظف)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Delete_الموظفون_ByID";  

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@معرّف_الموظف", معرّف_الموظف);

                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way, this includes errors from SP_HandleError if any
        ErrorHandler.HandleException(ex, nameof(Deleteالموظفون), $"Parameter: معرّف_الموظف = " + معرّف_الموظف);
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
            string query = $@"SP_Search_الموظفون_ByColumn";

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
