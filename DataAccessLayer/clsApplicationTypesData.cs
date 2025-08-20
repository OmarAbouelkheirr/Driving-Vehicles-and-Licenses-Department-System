using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Messaging;

namespace DataAccessLayer
{
    public class clsApplicationTypesData
    {
        public static DataTable GetApplicationTypes()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM ApplicationTypes";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)
                    dt.Load(reader);

                reader.Close();
            }

            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return dt;
        }

        public static bool GetApplicationTypeByID(int AppTypeID, ref string applicationTypeTitle, ref decimal applicationFees)
        {
            bool isFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"select * from ApplicationTypes where ApplicationTypeID=@AppTypeID";
            SqlCommand command = new SqlCommand(query, Connection);

            command.Parameters.AddWithValue("@AppTypeID", AppTypeID);

            try
            {
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    isFound = true;

                    applicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                    applicationFees =  (decimal)reader["ApplicationFees"];
                }
                else
                {
                    isFound = false;
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                Connection.Close();
            }

            return isFound;
        }
        public static bool UpdateAppType(int AppTypeID, string AppTypeTitle, decimal AppTypeFees)
        {
            int rowsAffected = 0;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"
                UPDATE ApplicationTypes
                   SET ApplicationTypeTitle = @AppTypeTitle, 
                      ApplicationFees = @AppTypeFees
                 WHERE ApplicationTypeID = @AppTypeID";

            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("@AppTypeTitle", AppTypeTitle);
            command.Parameters.AddWithValue("@AppTypeID", AppTypeID);
            command.Parameters.AddWithValue("@AppTypeFees", AppTypeFees);

            try
            {
                conn.Open();
                rowsAffected = command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                conn.Close();
            }

            return (rowsAffected > 0);
        }



    }
}
