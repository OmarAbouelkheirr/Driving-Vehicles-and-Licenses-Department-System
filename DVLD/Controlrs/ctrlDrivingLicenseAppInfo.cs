using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;

namespace DVLD.Controlrs
{
    public partial class ctrlDrivingLicenseAppInfo : UserControl
    {
        public ctrlDrivingLicenseAppInfo()
        {
            InitializeComponent();
        }

        public void loadLocalAppInfo(int localAppID)
        {
            if (!clsLocalDrivingLicenseApplication.IsLocalAppExistByID(localAppID))
            {
                MessageBox.Show("Local App Not Found!");
                return;
            }

            clsLocalDrivingLicenseApplication _LocalApp = clsLocalDrivingLicenseApplication.FindLocalAppByID(localAppID);

            lblAppID.Text = _LocalApp.LocalDrivingLicenseApplicationID.ToString();
            lblClass.Text = clsLicenseClasses.FindClassByID(_LocalApp.LicenseClassID).ClassName;
            lblTests.Text = clsLocalDrivingLicenseApplication.GetPassedTestsCount(localAppID) + "/3";
        }
    }
}
