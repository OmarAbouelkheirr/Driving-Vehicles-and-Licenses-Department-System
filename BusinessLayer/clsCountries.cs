using System.Data;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsCountries
    {
        public static DataTable GetCountries()
        {
            return clsCountriesData.GetCountries();
        }
        public static string GetCountryNameByID(int CountryID)
        {
            return clsCountriesData.GetCountryNameByID(CountryID);
        }
        public static int GetCountryIDByName(string CountryName)
        {
            return clsCountriesData.GetCountryIDByName(CountryName);
        }
    }
}
