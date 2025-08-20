using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsLocalDrivingLicenseApplication
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
    
        public int LocalDrivingLicenseApplicationID { get; set; }
        public int ApplicationID { get; set; }
        public int LicenseClassID { get; set; }

        public clsApplication AppData { get; set; }


        public clsLocalDrivingLicenseApplication()
        {
            Mode = enMode.AddNew;

            LocalDrivingLicenseApplicationID = -1;
            ApplicationID = -1;
            LicenseClassID = -1;

            AppData = new clsApplication();
        }

        public clsLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int ApplicationID, int LicenseClassID)
        {
            Mode = enMode.Update;

            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.ApplicationID = ApplicationID;
            this.LicenseClassID = LicenseClassID;

            AppData = clsApplication.FindApplicationByAppID(ApplicationID);
        }

        private bool _AddNewLocalApp()
        {
            this.LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseApplicationsData.AddNewLocalApp(ApplicationID, LicenseClassID);
            return LocalDrivingLicenseApplicationID != -1;
        }
        private bool _UpdateLocalApp()
        {
            return clsLocalDrivingLicenseApplicationsData.UpdateLocalApp(LocalDrivingLicenseApplicationID, ApplicationID, LicenseClassID);
        }

        public bool Save()
        {
            if(AppData.Save())
            {
                ApplicationID = AppData.ApplicationID;
            }
            else
            {
                return false;
            }

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLocalApp())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdateLocalApp();
            }
            return false;
        }

        public static bool DeleteLocalApp(int LocalAppID)
        {
            return clsLocalDrivingLicenseApplicationsData.DeleteLocalApp(LocalAppID);
        }
        public static DataTable GetAllLocalApps()
        {
            return clsLocalDrivingLicenseApplicationsData.GetAllLocalApps();
        }
        public static bool IsLocalAppExistByID(int LocalAppID)
        {
            return clsLocalDrivingLicenseApplicationsData.IsLocalAppExistByID(LocalAppID);
        }
        public static bool IfPersonHasActiveLocalApplication(int PersonID, int LicenseClassID)
        {
            return clsLocalDrivingLicenseApplicationsData.IfPersonHasActiveLocalApplication(PersonID, LicenseClassID);
        }

        public static clsLocalDrivingLicenseApplication FindLocalAppByID(int LocalAppID)
        {
            int ApplicationID = -1, LicenseClassID = -1;

            if (clsLocalDrivingLicenseApplicationsData.GetLocalAppByID(LocalAppID, ref ApplicationID, ref LicenseClassID))
            {
                return new clsLocalDrivingLicenseApplication(LocalAppID, ApplicationID, LicenseClassID);
            }
            else
            {
                return null;
            }
        }

        public static int GetPassedTestsCount(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenseApplicationsData.GetPassedTestsCount(LocalDrivingLicenseApplicationID);
        }

        public static bool Cancel(int LocalAppID)
        {
            clsLocalDrivingLicenseApplication _LocalApp = clsLocalDrivingLicenseApplication.FindLocalAppByID(LocalAppID);

            if (_LocalApp == null)
                return false;

            _LocalApp.AppData.ApplicationStatus = 2;
            _LocalApp.AppData.LastStatusDate = DateTime.Now;

            return _LocalApp.Save();
        }

    }

}
