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
    public partial class ctrlAppsInfo : UserControl
    {
        public ctrlAppsInfo()
        {
            InitializeComponent();
        }

        public void loadAppsData(int localAppID)
        {
            if (!clsLocalDrivingLicenseApplication.IsLocalAppExistByID(localAppID))
            {
                MessageBox.Show("Local App Not Found");
                return;
            }

            clsLocalDrivingLicenseApplication _localApp = clsLocalDrivingLicenseApplication.FindLocalAppByID(localAppID);

            ctrlApplicationInfo1.loadApplicationData(_localApp.ApplicationID);
            ctrlDrivingLicenseAppInfo1.loadLocalAppInfo(localAppID);
        }

    }
}
