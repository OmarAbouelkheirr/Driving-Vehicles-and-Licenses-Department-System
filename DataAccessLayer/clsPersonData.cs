using System;
using System.Data;
using System.Data.SqlClient;


namespace DataAccessLayer
{
    public class clsPersonData
    {
        // Just For Null Values
        private static string GetSafeString(SqlDataReader reader, string columnName)
        {
            return reader[columnName] != DBNull.Value ? (string)reader[columnName] : "";
        }

        public static void AddNullableParameter(SqlCommand command, string parameterName, object value)
        {
            if (value == null || (value is string str && string.IsNullOrEmpty(str)))
            {
                command.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue(parameterName, value);
            }
        }

        public static bool GetPersonByID(int PersonID, ref string NationalNo, ref string fName,
            ref string sName, ref string tName, ref string lName,
            ref DateTime DateOfBirth, ref short Gendor, ref string Address,
            ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "select * from People where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                conn.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    NationalNo = (string)reader["NationalNo"];

                    fName = (string)reader["FirstName"];
                    sName = (string)reader["SecondName"];
                    tName = GetSafeString(reader, "ThirdName");
                    lName = (string)reader["LastName"];

                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Gendor = (Byte)reader["Gendor"];

                    Address = (string)reader["Address"];
                    Phone = (string)reader["Phone"];

                    NationalityCountryID = (int)reader["NationalityCountryID"];

                    Email = GetSafeString(reader, "Email");
                    ImagePath = GetSafeString(reader, "ImagePath");
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
                conn.Close();
            }

            return isFound;
        }


        public static bool GetPersonByNationalNo(string NationalNo, ref int PersonID, ref string fName,
            ref string sName, ref string tName, ref string lName,
            ref DateTime DateOfBirth, ref short Gendor, ref string Address,
            ref string Phone, ref string Email, ref int NationalityCountryID, ref string ImagePath)
        {
            bool isFound = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "select * from People where NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                conn.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    PersonID = (int)reader["PersonID"];

                    fName = (string)reader["FirstName"];
                    sName = (string)reader["SecondName"];
                    tName = GetSafeString(reader, "ThirdName");
                    lName = (string)reader["LastName"];

                    DateOfBirth = (DateTime)reader["DateOfBirth"];
                    Gendor = (Byte)reader["Gendor"];

                    Address = (string)reader["Address"];
                    Phone = (string)reader["Phone"];

                    NationalityCountryID = (int)reader["NationalityCountryID"];

                    Email = GetSafeString(reader, "Email");
                    ImagePath = GetSafeString(reader, "ImagePath");
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
                conn.Close();
            }

            return isFound;
        }


        public static int AddNewPerson(string NationalNo, string fName,
            string sName, string tName, string lName,
            DateTime DateOfBirth, short Gendor, string Address,
            string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            int PersonID = -1;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"
                INSERT INTO People
                (NationalNo, FirstName, SecondName, ThirdName, LastName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath)
                VALUES
                (@NationalNo, @fName, @sName, @tName, @lName, @DateOfBirth, @Gendor, @Address, @Phone, @Email, @NationalityCountryID, @ImagePath);

                SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@fName", fName);
            command.Parameters.AddWithValue("@sName", sName);
            AddNullableParameter(command, "@tName", tName);
            command.Parameters.AddWithValue("@lName", lName);

            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            AddNullableParameter(command, "@Email", Email);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            AddNullableParameter(command, "@ImagePath", ImagePath);
            try
            {
                conn.Open();
                
                object result = command.ExecuteScalar();
                
                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    PersonID = insertedID;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }

            return PersonID;
        }

        public static bool UpdatePerson(int PersonID,string NationalNo, string fName,
          string sName, string tName, string lName,
          DateTime DateOfBirth, short Gendor, string Address,
          string Phone, string Email, int NationalityCountryID, string ImagePath)
        {
            int rowsAffected = 0;
            
            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"
                UPDATE People
                SET [NationalNo] = @NationalNo,
                    [FirstName] = @fName,
                    [SecondName] = @sName,
                    [ThirdName] = @tName,
                    [LastName] = @lName,
                    [DateOfBirth] = @DateOfBirth,
                    [Gendor] = @Gendor,
                    [Address] = @Address,
                    [Phone] = @Phone,
                    [Email] = @Email,
                    [NationalityCountryID] = @NationalityCountryID,
                    [ImagePath] = @ImagePath

                WHERE PersonID = @PersonID; ";

            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);
            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@fName", fName);
            command.Parameters.AddWithValue("@sName", sName);
            AddNullableParameter(command, "@tName", tName);
            command.Parameters.AddWithValue("@lName", lName);

            command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            command.Parameters.AddWithValue("@Gendor", Gendor);
            command.Parameters.AddWithValue("@Address", Address);
            command.Parameters.AddWithValue("@Phone", Phone);
            AddNullableParameter(command, "@Email", Email);
            command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            AddNullableParameter(command, "@ImagePath", ImagePath);


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

        public static DataTable GetAllPeople()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"
SELECT 
    People.PersonID,
    People.NationalNo,
    People.FirstName,
    People.LastName,
    People.ThirdName,
    People.LastName,
    People.DateOfBirth,
    People.Phone,
    People.Email,
    
    Countries.CountryName AS Nationality,

    CASE 
        WHEN People.Gendor = 0 THEN 'Male'
        ELSE 'Female'
    END AS Gendor

FROM People
INNER JOIN Countries
    ON People.NationalityCountryID = Countries.CountryID
                ";

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

        public static bool DeletePerson(int PersonID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Delete People 
                                where PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }


        public static bool IsPersonExistByID(int PersonID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM People WHERE PersonID = @PersonID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static bool IsPersonExistByNationalNo(string NationalNo)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM People WHERE NationalNo = @NationalNo";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
    }
}