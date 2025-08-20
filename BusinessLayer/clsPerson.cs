using System;
using System.Data;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsPerson
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int PersonID { get; set; }
        public string NationalNo { get; set; }
        public string fName { get; set; }
        public string sName { get; set; }
        public string tName { get; set; }
        public string lName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public short Gendor { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int NationalityCountryID { get; set; }
        public string ImagePath { get; set; }

        public string FullName()
        {
            return fName + " " + sName + " " + tName + " " + lName;
        }

        public clsPerson()
        {
            Mode = enMode.AddNew;
            PersonID = -1;
            fName = "";
            sName = "";
            tName = "";
            lName = "";
            DateOfBirth = DateTime.Now;
            Gendor = -1;
            Address = "";
            Phone = "";
            Email = "";
            NationalityCountryID = -1;
            ImagePath = "";

            Mode = enMode.AddNew;
        }


        public clsPerson(int personID, string nationalNo, string fName, string sName, string tName, string lName, DateTime dateOfBirth, short gendor, string address, string phone, string email, int nationalityCountryID, string imagePath)
        {
            PersonID = personID;
            NationalNo = nationalNo;
            this.fName = fName;
            this.sName = sName;
            this.tName = tName;
            this.lName = lName;
            DateOfBirth = dateOfBirth;
            Gendor = gendor;
            Address = address;
            Phone = phone;
            Email = email;
            NationalityCountryID = nationalityCountryID;
            ImagePath = imagePath;
            
            Mode = enMode.Update;
        }

        private bool _AddNewPerson()
        {
            this.PersonID = clsPersonData.AddNewPerson(NationalNo, fName, sName, tName, lName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            return (this.PersonID != -1);
        }

        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(PersonID, NationalNo, fName, sName, tName, lName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
        }

        public static clsPerson FindPersonByID(int PersonID)
        {
            string NationalNo = "", fName = "", sName = "" ,tName = "", lName = "", Address = "", Phone = "", Email = "", ImagePath = "";
            int NationalityCountryID = -1;
            DateTime DateOfBirth = DateTime.Now;
            short Gendor = -1;

            if (clsPersonData.GetPersonByID(PersonID, ref NationalNo, ref fName, ref sName, ref tName, ref lName, ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath))
            {
                return new clsPerson(PersonID, NationalNo, fName, sName, tName, lName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            }
            else
            {
                return null;
            }
        }

        public static clsPerson FindPersonByNationalNo(string NationalNo)
        {
            string fName = "", sName = "", tName = "", lName = "", Address = "", Phone = "", Email = "", ImagePath = "";
            int NationalityCountryID = -1, PersonID = -1;
            DateTime DateOfBirth = DateTime.Now;
            short Gendor = -1;

            if (clsPersonData.GetPersonByNationalNo(NationalNo, ref PersonID, ref fName, ref sName, ref tName, ref lName, ref DateOfBirth, ref Gendor, ref Address, ref Phone, ref Email, ref NationalityCountryID, ref ImagePath))
            {
                return new clsPerson(PersonID, NationalNo, fName, sName, tName, lName, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath);
            }
            else
            {
                return null;
            }
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:
                    return _UpdatePerson();
            }

            return false;
        }

        public static DataTable GetAllPeople()
        {
            return clsPersonData.GetAllPeople();
        }

        public static bool DeletePerson(int PersonID)
        {
            return clsPersonData.DeletePerson(PersonID);
        }

        public static bool isPersonExistByID(int PersonID)
        {
            return clsPersonData.IsPersonExistByID(PersonID);
        }
        public static bool isPersonExistByNationalNo(string NationalNo)
        {
            return clsPersonData.IsPersonExistByNationalNo(NationalNo);
        }

    }
}
