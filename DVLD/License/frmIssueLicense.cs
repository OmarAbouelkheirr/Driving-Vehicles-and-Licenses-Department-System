using System;
using System.Windows.Forms;
using BusinessLayer;

namespace DVLD.License
{
    public partial class frmIssueLicense : Form
    {
        int _LocalAppID;
        clsApplication _Application;
        clsLocalDrivingLicenseApplication _LocalApp;
        clsDriver _Driver;
        clsLicense _License = new clsLicense();

        public frmIssueLicense(int localAppID)
        {
            InitializeComponent();
            _LocalAppID = localAppID;
            _LocalApp = clsLocalDrivingLicenseApplication.FindLocalAppByID(_LocalAppID);
            _Application = clsApplication.FindApplicationByAppID(_LocalApp.ApplicationID);
        }

        private void frmIssueLicense_Load(object sender, EventArgs e)
        {
            ctrlAppsInfo1.loadAppsData(_LocalAppID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        bool _NewDriver()
        {
            _Driver = new clsDriver();
            _Driver.PersonID = _Application.PersonData.PersonID;
            _Driver.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;
            _Driver.CreatedDate = DateTime.Now;

            return _Driver.Save();
        }

        bool _IssueLicense()
        {
            _License.ApplicationID = _Application.ApplicationID;
            _License.DriverID = _Driver.DriverID;
            _License.LicenseClassID = _LocalApp.LicenseClassID;
            _License.IssueDate = DateTime.Now;
            _License.ExpirationDate = DateTime.Now.AddYears(clsLicenseClasses.FindClassByID(_License.LicenseClassID).DefaultValidityLength);
            _License.Notes = guna2TextBox1.Text;
            _License.PaidFees = 10;
            _License.IsActive = true;
            _License.IssueReason = 1;
            _License.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;
            _LocalApp.AppData.ApplicationStatus = 3;
            _LocalApp.AppData.Mode = clsApplication.enMode.Update;

            return _License.Save() && _LocalApp.Save() ;
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (clsDriver.IsDriverExistByPersonID(_Application.PersonData.PersonID))
            {
                _Driver = clsDriver.GetDriverByPersonID(_Application.PersonData.PersonID);
                if (_IssueLicense())
                {
                    MessageBox.Show("License Done with id " + _License.LicenseID.ToString());
                }
            }
            else
            {
                if (_NewDriver())
                {
                    if (_IssueLicense())
                        MessageBox.Show("License Done with id " + _License.LicenseID.ToString() + " and driver id " + _Driver.DriverID.ToString());
                }
            }
        }
    }
}
