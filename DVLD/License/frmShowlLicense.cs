using System;
using System.ComponentModel;
using System.Windows.Forms;
using BusinessLayer;

namespace DVLD.License
{
    public partial class frmShowlLicense : Form
    {
        int _LocalAppID;
        int _LicenseID;

        public frmShowlLicense(int LicenseID, int LocalAppID)
        {
            InitializeComponent();

            _LocalAppID = LocalAppID;
            _LicenseID = LicenseID;
        }

        private void ShowlLicense_Load(object sender, EventArgs e)
        {
            if (_LicenseID == -1)
            {
                int LicenseID = clsLicense.GetLicenseIDByApplicationID(clsLocalDrivingLicenseApplication.FindLocalAppByID(_LocalAppID).ApplicationID);

                if (LicenseID != -1)
                {
                    ctrlLicenseInfo1.LoadLicenseData(LicenseID);
                }
                else
                {
                    MessageBox.Show("License not found!");
                    return;
                }
            }
            else if (_LocalAppID == -1)
            {
                ctrlLicenseInfo1.LoadLicenseData(_LicenseID);
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
