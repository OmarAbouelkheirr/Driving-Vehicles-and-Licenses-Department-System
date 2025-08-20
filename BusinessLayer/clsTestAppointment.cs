using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsTestAppointment
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int TestAppointmentID { get; set; }
        public int TestTypeID { get; set; }
        public int LocalDrivingLicenseApplicationID { get; set; }
        public DateTime AppointmentDate { get; set; }
        public decimal PaidFees { get; set; }
        public int CreatedByUserID { get; set; }
        public bool IsLocked { get; set; }
        public int RetakeTestApplicationID { get; set; }


        //public clsTestType TestType { get; set; }
        //public clsLocalDrivingLicenseApplication LocalApp { get; set; }
        //public clsUser UserData { get; set; }


        public clsTestAppointment()
        {
            Mode = enMode.AddNew;

            this.TestAppointmentID = -1;
            this.TestTypeID = -1;
            this.LocalDrivingLicenseApplicationID = -1;
            this.CreatedByUserID = -1;
            this.AppointmentDate = DateTime.Now;
            this.IsLocked = false;
            this.PaidFees = 0;
            this.RetakeTestApplicationID = -1;
        }

        public clsTestAppointment(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate,
           decimal PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            Mode = enMode.Update;

            this.TestAppointmentID = TestAppointmentID;
            this.TestTypeID = TestTypeID;
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.AppointmentDate = AppointmentDate;
            this.PaidFees = PaidFees;
            this.CreatedByUserID = CreatedByUserID;
            this.IsLocked = IsLocked;
            this.RetakeTestApplicationID = RetakeTestApplicationID;
        }

        private bool _AddNewTestAppointment()
        {
            TestAppointmentID = clsTestAppointmentsData.AddNewTestAppointment(TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);
            return TestAppointmentID != -1;
        }
        private bool _UpdateTestAppointment()
        {
            return clsTestAppointmentsData.UpdateTestAppointment(TestAppointmentID, TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestAppointment())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateTestAppointment();
            }
            return false;
        }


        public static bool Delete(int TestAppointmentID)
        {
            return clsTestAppointmentsData.DeleteTestAppointment(TestAppointmentID);
        }
        public static DataTable GetAllTestAppointments()
        {
            return clsTestAppointmentsData.GetAllTestAppointments();
        }
        public static DataTable GetTestAppointmentsByLocalAppID(int LocalAppID, int TestTypeID)
        {
            return clsTestAppointmentsData.GetTestAppointmentsByLocalAppID(LocalAppID, TestTypeID);
        }

        public static clsTestAppointment FindTestAppointmentByID(int TestAppointmentID)
        {
            DateTime AppointmentDate = DateTime.Now;
            int TestTypeID = -1, LocalDrivingLicenseApplicationID = -1, CreatedByUserID = -1, RetakeTestApplicationID = -1;
            bool IsLocked = false;
            decimal PaidFees = 0;

            if (clsTestAppointmentsData.GetTestAppointmentByID(TestAppointmentID, ref TestTypeID, ref LocalDrivingLicenseApplicationID, ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))
            {
                return new clsTestAppointment(TestAppointmentID, TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);
            }
            else
            {
                return null;
            }
        }

        public static bool IsTestAppointmentLocked(int TestAppointmentID)
        {
            return clsTestAppointmentsData.IsTestAppointmentLocked(TestAppointmentID);
        }


        public static bool IfAppointmentIsActive(int LocalAppID, int TestTypeID)
        {
            return clsTestAppointmentsData.IfAppointmentIsActive(LocalAppID, TestTypeID);
        }

        public static bool? IsLastTestFailed(int localDrivingLicenseApplicationID, int testTypeID)
        {
            return clsTestAppointmentsData.IsLastTestFailed(localDrivingLicenseApplicationID, testTypeID);
        }


    }
}
