
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using SchoolsDb_DataAccess;
using Newtonsoft.Json;

namespace SchoolsDb_DataLayer
{
    public class clsالصفوفData
    {
        //#nullable enable

        public static bool GetالصفوفInfoByClassRoomName(ref int? معرّف_الصف, string اسم_الصف)
        { 
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"SELECT * FROM الصفوف WHERE اسم_الصف = @اسم_الصف";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                       
                        // Ensure correct parameter assignment
                        command.Parameters.AddWithValue("@اسم_الصف", اسم_الصف ?? (object)DBNull.Value);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                معرّف_الصف = (int)reader["معرّف_الصف"];
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle all exceptions in a general way
                ErrorHandler.HandleException(ex, nameof(GetالصفوفInfoByClassRoomName), $"Parameter: اسم_الصف = " + معرّف_الصف);
            }

            return isFound;
        }

        public static bool GetالصفوفInfoByID(int? معرّف_الصف , ref string اسم_الصف )
{
    bool isFound = false;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_الصفوف_ByID";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Ensure correct parameter assignment
                command.Parameters.AddWithValue("@معرّف_الصف", معرّف_الصف ?? (object)DBNull.Value);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                { 
                    if (reader.Read())
                    {
                        // The record was found
                        isFound = true;

                                اسم_الصف = (string)reader["اسم_الصف"];
                          
                    }
                }
            }
        }
    }


    catch (Exception ex)
    {
        // Handle all exceptions in a general way
        ErrorHandler.HandleException(ex, nameof(GetالصفوفInfoByID), $"Parameter: معرّف_الصف = " + معرّف_الصف);
    }

    return isFound;
}

        public static DataTable GetAllالصفوف()
{
    DataTable dt = new DataTable();

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = "SP_Get_All_الصفوف";

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
        ErrorHandler.HandleException(ex, nameof(GetAllالصفوف), "No parameters for this method.");
    }

    return dt;
}

        public static int? AddNewالصفوف(string اسم_الصف)
    {
        int? معرّف_الصف = null;

        try
        {
            using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string query = @"SP_Add_الصفوف";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@اسم_الصف", اسم_الصف);
          
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
                        معرّف_الصف = (int)outputIdParam.Value;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            // Handle all exceptions in a general way
            ErrorHandler.HandleException(ex, nameof(AddNewالصفوف), $"Parameters: string اسم_الصف, string المستوى_الصفّي");
        }

        return معرّف_الصف;
    }

        public static bool UpdateالصفوفByID(int? معرّف_الصف, string اسم_الصف )
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Update_الصفوف_ByID"; 

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                // Create the parameters for the stored procedure
                    command.Parameters.AddWithValue("@معرّف_الصف", معرّف_الصف);
                    command.Parameters.AddWithValue("@اسم_الصف", اسم_الصف);


                // Open the connection and execute the update
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle exceptions
        ErrorHandler.HandleException(ex, nameof(UpdateالصفوفByID), $"Parameter: معرّف_الصف = " + معرّف_الصف);
    }

    return (rowsAffected > 0);
}

        public static bool Deleteالصفوف(int معرّف_الصف)
{
    int rowsAffected = 0;

    try
    {
        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        {
            string query = $@"SP_Delete_الصفوف_ByID";  

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@معرّف_الصف", معرّف_الصف);

                connection.Open();

                rowsAffected = command.ExecuteNonQuery();
            }
        }
    }
    catch (Exception ex)
    {
        // Handle all exceptions in a general way, this includes errors from SP_HandleError if any
        ErrorHandler.HandleException(ex, nameof(Deleteالصفوف), $"Parameter: معرّف_الصف = " + معرّف_الصف);
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
            string query = $@"SP_Search_الصفوف_ByColumn";

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
