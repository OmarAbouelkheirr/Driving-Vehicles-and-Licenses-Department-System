using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    internal class clsDataAccessSettings
    {
        //public static string ConnectionString = "Server=.;Database=DVLD; Integrated Security=True;";
        public static string ConnectionString
         = ConfigurationManager.ConnectionStrings["DVLD_DB"].ConnectionString;
    }
}
