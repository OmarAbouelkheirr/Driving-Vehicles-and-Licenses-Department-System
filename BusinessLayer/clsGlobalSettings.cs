using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class clsGlobalSettings
    {
        //public static clsUser LoggedInUser = new clsUser();
        public static clsUser LoggedInUser = clsUser.FindUserByUserName("Omar22113");
    }
}
