using System;
using System.Windows.Forms;
using BusinessLayer;
using DVLD.License;

namespace DVLD.Applications
{
    public partial class frmReplacementLicense : Form
    {
        clsApplication _ReplacementApplication;
        clsLicense _NewLicense;
        clsLicense _OldLicense;
        decimal ApplicationDamageFees = clsApplicationType.FindAppTypeByID(4).AppTypeFees;
        decimal ApplicationLostFees = clsApplicationType.FindAppTypeByID(3).AppTypeFees;
        int UserID = clsGlobalSettings.LoggedInUser.UserID;


        // 4 for Damage , 3 for Lost
        int ApplicationTypeID = 3;

        public frmReplacementLicense()
        {
            InitializeComponent();
        }

        void fillApplication()
        {
            lblAppDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblApplicationFees.Text = rbForDamage.Checked ? ApplicationDamageFees.ToString() : ApplicationLostFees.ToString();
            lblCreatedBy.Text = UserID.ToString();
        }

        private void frmReplaceOrLostLicense_Load(object sender, EventArgs e)
        {
            rbForLost.Checked = true;
            
            fillApplication();
        }

        private void rbForDamage_CheckedChanged(object sender, EventArgs e)
        {
            lblForDamage.Visible = true;
            lblReplaceForLost.Visible = false;
            ApplicationTypeID = 4;
            lblApplicationFees.Text = ApplicationDamageFees.ToString();
        }

        private void rbForLost_CheckedChanged(object sender, EventArgs e)
        {
            lblForDamage.Visible = false;
            lblReplaceForLost.Visible = true;
            ApplicationTypeID = 3;
            lblApplicationFees.Text = ApplicationLostFees.ToString();
        }

        bool _CreateAndSaveReplacementApplication()
        {
            // renew application!
            _ReplacementApplication = new clsApplication();
            _ReplacementApplication.ApplicantPersonID = _OldLicense.DriverData.PersonID;
            _ReplacementApplication.ApplicationDate = DateTime.Now;
            _ReplacementApplication.ApplicationTypeID = ApplicationTypeID;
            _ReplacementApplication.ApplicationStatus = 1;
            _ReplacementApplication.LastStatusDate = DateTime.Now;
            _ReplacementApplication.PaidFees = clsApplicationType.FindAppTypeByID(ApplicationTypeID).AppTypeFees;
            _ReplacementApplication.CreatedByUserID = UserID;

            return _ReplacementApplication.Save();
        }


        bool _IssueReplacementLicense()
        {
            _NewLicense = new clsLicense();

            _NewLicense.ApplicationID = _ReplacementApplication.ApplicationID;
            _NewLicense.DriverID = _OldLicense.DriverID;
            _NewLicense.LicenseClassID = _OldLicense.LicenseClassID;
            _NewLicense.IssueDate = DateTime.Now;
            _NewLicense.ExpirationDate = DateTime.Now.AddYears(clsLicenseClasses.FindClassByID(_OldLicense.LicenseClassID).DefaultValidityLength);
            _NewLicense.PaidFees = clsLicenseClasses.FindClassByID(_OldLicense.LicenseClassID).ClassFees;
            _NewLicense.IsActive = true;
            _NewLicense.CreatedByUserID = UserID;

            if (ApplicationTypeID == 3) // Lost
                _NewLicense.IssueReason = 4; // Replacement for Lost
            else if (ApplicationTypeID == 4) // Damage
                _NewLicense.IssueReason = 3; // Replacement for Damaged

            _ReplacementApplication.ApplicationStatus = 3;
            _ReplacementApplication.Mode = clsApplication.enMode.Update;

            return _NewLicense.Save() && _ReplacementApplication.Save();
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            int licenseID = ctrlLicenseInfoWithFilter1.GetLicenseID();

            if (licenseID == -1)
            {
                MessageBox.Show("You should enter LicenseID!");
                return;
            }
            _OldLicense = clsLicense.Find(licenseID);

            if (_OldLicense.IsActive && !(_OldLicense.ExpirationDate < DateTime.Now))
            {
                if (_CreateAndSaveReplacementApplication())
                {
                    _OldLicense.IsActive = false;
                    _OldLicense.Mode = clsLicense.enMode.Update;
                    _OldLicense.Save();

                    if (_IssueReplacementLicense())
                    {
                        lblReplacementAppID.Text = _ReplacementApplication.ApplicationID.ToString();
                        lblOldLicenseID.Text = _OldLicense.LicenseID.ToString();
                        lblReplacementlicenseID.Text = _NewLicense.LicenseID.ToString();
                        MessageBox.Show("Done Replacement license " + _OldLicense.LicenseID + " and new LicenseID is " + _NewLicense.LicenseID);
                    }
                    else
                    {
                        MessageBox.Show("Error!!!!!!!!!!!!");
                    }
                }
                else
                {
                    MessageBox.Show("Error!!!!!!!!!!!!");
                }
            }
            else
            {
                MessageBox.Show("This license is not Active or Expired!!!!!");
            }

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            int licenseID = ctrlLicenseInfoWithFilter1.GetLicenseID();

            if (licenseID != -1)
            {
                frmLicenseHistory form = new frmLicenseHistory(clsLicense.Find(licenseID).DriverID);
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("You should enter LicenseID!");
            }
        }
    }
}
