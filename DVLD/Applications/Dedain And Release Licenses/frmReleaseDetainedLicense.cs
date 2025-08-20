using System;
using System.Windows.Forms;
using BusinessLayer;

namespace DVLD.Applications.Dedain_And_Release_Licenses
{
    public partial class frmReleaseDetainedLicense : Form
    {
        decimal ApplicationFees = clsApplicationType.FindAppTypeByID(5).AppTypeFees;
        int UserID = clsGlobalSettings.LoggedInUser.UserID;

        clsDetainedLicense _DetainedLicense;
        clsApplication _ReleaseApp;
        clsLicense _License;

        public frmReleaseDetainedLicense()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void fillReleaseInfo()
        {
            lblAppFees.Text = ApplicationFees.ToString();
            lblCreatedBy.Text = UserID.ToString();
        }

        private void frmReleaseDetainedLicense_Load(object sender, EventArgs e)
        {
            fillReleaseInfo();
        }

        bool _CreateReleaseApplication()
        {
            _ReleaseApp = new clsApplication();

            _ReleaseApp.ApplicantPersonID = _License.DriverData.PersonID;
            _ReleaseApp.ApplicationTypeID = 5;
            _ReleaseApp.ApplicationStatus = 1;
            _ReleaseApp.PaidFees = ApplicationFees;
            _ReleaseApp.CreatedByUserID = UserID;

            return _ReleaseApp.Save();
        }

        bool _UpdateLicenseAndConnectItWithRelApp()
        {
            _DetainedLicense.ReleaseDate = DateTime.Now;
            _DetainedLicense.ReleasedByUserID = UserID;
            _DetainedLicense.ReleaseApplicationID = _ReleaseApp.ApplicationID;
            _DetainedLicense.IsReleased = true;
            _DetainedLicense.Mode = clsDetainedLicense.enMode.Update;
            
            _ReleaseApp.ApplicationStatus = 3;
            _ReleaseApp.LastStatusDate = DateTime.Now;
            _ReleaseApp.Mode = clsApplication.enMode.Update;

            return _DetainedLicense.Save() && _ReleaseApp.Save();
        }


        void _UpdateInfo()
        {
            lblDetainID.Text = _DetainedLicense.DetainID.ToString();
            lbllicenseID.Text = _License.LicenseID.ToString();
            lblDetainDate.Text = _DetainedLicense.DetainDate.ToString("dd/MM/yyyy");
            lblFineFees.Text = _DetainedLicense.FineFees.ToString();

            lblTotalFees.Text = (ApplicationFees + _DetainedLicense.FineFees).ToString();
            lblAppID.Text = _ReleaseApp.ApplicationID.ToString();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            int licenseID = ctrlLicenseInfoWithFilter1.GetLicenseID();

            if (licenseID == -1)
            {
                MessageBox.Show("You should enter licenseID!");
                return;
            }

            if (!clsDetainedLicense.IsLicenseDetained(licenseID))
            {
                MessageBox.Show("This license is not detained!");
                return;
            }

            _License = clsLicense.Find(licenseID);
            _DetainedLicense = clsDetainedLicense.Find(licenseID);

            if (!_CreateReleaseApplication())
            {
                MessageBox.Show("Error (Create Release App!)");
                return;
            }

            if(_UpdateLicenseAndConnectItWithRelApp())
            {
                MessageBox.Show("License Released successfully!");
                _UpdateInfo();
                btnRelease.Enabled = false;
            }
            else
            {
                MessageBox.Show("Error (Update License And Connect It With Rel App!)");
            }

        }
    }
}
