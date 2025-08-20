using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsLocalDrivingLicenseApplicationsData
    {
        public static bool GetLocalAppByID(int LocalAppID, ref int ApplicationID, ref int LicenseClassID)
        {
            bool isFound = false;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"select * from LocalDrivingLicenseApplications 
                            where LocalDrivingLicenseApplicationID = @LocalAppID";

            SqlCommand command = new SqlCommand(query, conn);
            command.Parameters.AddWithValue("@LocalAppID", LocalAppID);

            try
            {
                conn.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    ApplicationID = (int)reader["ApplicationID"];
                    LicenseClassID = (int)reader["LicenseClassID"];
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
        public static int AddNewLocalApp(int ApplicationID, int LicenseClassID)
        {
            int LocalAppID = -1;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"
                INSERT INTO LocalDrivingLicenseApplications
                (ApplicationID, LicenseClassID)
                VALUES
                (@ApplicationID, @LicenseClassID)

                SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                conn.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    LocalAppID = insertedID;
            }
            catch (Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }

            return LocalAppID;
        }
        public static bool UpdateLocalApp(int LocalAppID, int ApplicationID, int LicenseClassID)
        {
            int rowsAffected = 0;

            SqlConnection conn = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"
                UPDATE [dbo].[LocalDrivingLicenseApplications]
                SET ApplicationID = @ApplicationID,
                    LicenseClassID = @LicenseClassID
                WHERE LocalDrivingLicenseApplicationID=@LocalAppID";

            SqlCommand command = new SqlCommand(query, conn);

            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            command.Parameters.AddWithValue("@LocalAppID", LocalAppID);

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
        public static DataTable GetAllLocalApps()
        {
            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"select LocalApps.LocalDrivingLicenseApplicationID as LocalAppID, LicenseClasses.ClassName as DrivingClass, People.NationalNo, 
(People.FirstName + ' ' + People.SecondName + ' ' + ISNULL(People.ThirdName, '') + ' ' + People.LastName) as FullName,
Applications.ApplicationDate,
case 
	when Applications.ApplicationStatus = 1 then 'New'
	when Applications.ApplicationStatus = 2 then 'Cancelled'
	when Applications.ApplicationStatus = 3 then 'Completed'
end as Status 
from LocalDrivingLicenseApplications as LocalApps
join Applications on LocalApps.ApplicationID = Applications.ApplicationID
join People on People.PersonID = Applications.ApplicantPersonID
join LicenseClasses on LicenseClasses.LicenseClassID = LocalApps.LicenseClassID";

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

        public static bool DeleteLocalApp(int LocalAppID)
        {
            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Delete LocalDrivingLicenseApplications
                                where LocalDrivingLicenseApplicationID = @LocalAppID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalAppID", LocalAppID);

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


        public static bool IsLocalAppExistByID(int LocalAppID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID = @LocalAppID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@LocalAppID", LocalAppID);

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

        public static bool IfPersonHasActiveLocalApplication(int PersonID, int LicenseClassID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"select found=1 from LocalDrivingLicenseApplications 
            join Applications on LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID and Applications.ApplicationStatus = 1
            where Applications.ApplicantPersonID = @PersonID and LocalDrivingLicenseApplications.LicenseClassID = @LicenseClassID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@PersonID", PersonID);
            command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

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

        public static int GetPassedTestsCount(int LocalDrivingLicenseApplicationID)
        {
            int PassedTestsCount;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"select count(TestID) as Result from Tests 
                join TestAppointments on Tests.TestAppointmentID = TestAppointments.TestAppointmentID and Tests.TestResult = 1 
                where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand command = new SqlCommand(query, Connection);
            command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                Connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    PassedTestsCount = (int)reader["Result"];
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

            return PassedTestsCount;
        }

    }
}
