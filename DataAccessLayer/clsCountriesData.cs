using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsCountriesData
    {
        public static DataTable GetCountries()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Countries";

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

        public static string GetCountryNameByID(int ID)
        {
            string Name = "";

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            
            string query = @"select * from Countries where CountryID=@ID";
            SqlCommand command = new SqlCommand(query, Connection);
            
            command.Parameters.AddWithValue("@ID", ID);

            try
            {
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Name = (string)reader["CountryName"];

                }
                else
                {
                    return null;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                Connection.Close();
            }
            return Name;
        }
        public static int GetCountryIDByName(string CountryName)
        {
            int CountryID;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"select CountryID from Countries where CountryName=@CountryName";
            SqlCommand command = new SqlCommand(query, Connection);

            command.Parameters.AddWithValue("@CountryName", CountryName);
            try
            {
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    CountryID = (int)reader["CountryID"];

                }
                else
                {
                    return -1;
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                Connection.Close();
            }
            return CountryID;
        }
    }

}
