using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsApplicationType
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.Update;

        public int AppTypeID { get; set; }
        public string AppTypeTitle { get; set; }
        public decimal AppTypeFees { get; set; }

        public clsApplicationType(int AppTypeID, string AppTypeTitle, decimal AppTypeFees)
        {
            Mode = enMode.Update;

            this.AppTypeID = AppTypeID;
            this.AppTypeFees = AppTypeFees;
            this.AppTypeTitle = AppTypeTitle;
        }

        public bool UpdateAppType()
        {
            return clsApplicationTypesData.UpdateAppType(AppTypeID, AppTypeTitle, AppTypeFees);
        }

        public static DataTable GetApplicationTypes()
        {
            return clsApplicationTypesData.GetApplicationTypes();
        }

        public static clsApplicationType FindAppTypeByID(int AppTypeID)
        {
            string AppTypeTitle = "";
            decimal AppTypeFees = 0;

            if (clsApplicationTypesData.GetApplicationTypeByID(AppTypeID, ref AppTypeTitle, ref AppTypeFees))
            {
                return new clsApplicationType(AppTypeID,AppTypeTitle, AppTypeFees);
            }
            else
            {
                return null;
            }
        }
    }
}
