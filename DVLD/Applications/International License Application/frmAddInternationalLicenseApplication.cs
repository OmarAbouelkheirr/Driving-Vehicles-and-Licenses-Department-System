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
using DVLD.License;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Applications.International_License_Application
{
    public partial class frmAddInternationalLicenseApplication : Form
    {
        clsApplication _Application;
        clsLicense _License;
        clsInternationalLicense _InternationalLicense;
        decimal ApplicationFees = clsApplicationType.FindAppTypeByID(6).AppTypeFees;
        int UserID = clsGlobalSettings.LoggedInUser.UserID;
        public frmAddInternationalLicenseApplication()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void fillApplication()
        {
            lblAppDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblIssueDate.Text = DateTime.Now.ToString("dd/MM/yyyy");

            lblFees.Text = ApplicationFees.ToString();
            lblExipirationDate.Text = DateTime.Now.AddYears(1).ToString("dd/MM/yyyy");
            lblCreatedBy.Text = UserID.ToString();
        }

        private void frmAddInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            fillApplication();
        }


        bool _CreateAndSaveApplication()
        {
            _Application = new clsApplication();
            _Application.ApplicantPersonID = _License.DriverData.PersonID;
            _Application.ApplicationDate = DateTime.Now;
            _Application.ApplicationTypeID = 6;
            _Application.ApplicationStatus = 1;
            _Application.LastStatusDate = DateTime.Now;
            _Application.PaidFees = ApplicationFees;
            _Application.CreatedByUserID = UserID;

            return _Application.Save();
        }

        bool _CreateAndSaveInternationalLicense()
        {
            _InternationalLicense = new clsInternationalLicense();

            _InternationalLicense.ApplicationID = _Application.ApplicationID;
            _InternationalLicense.DriverID = _License.DriverID;
            _InternationalLicense.IssuedUsingLocalLicenseID = _License.LicenseID;
            _InternationalLicense.IssueDate = DateTime.Now;
            _InternationalLicense.ExpirationDate = DateTime.Now.AddYears(1);
            _InternationalLicense.IsActive = true;
            _InternationalLicense.CreatedByUserID = UserID;

            return _InternationalLicense.Save();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            int licenseID = ctrlLicenseInfoWithFilter1.GetLicenseID();

            if (licenseID != -1)
            {
                _License = clsLicense.Find(licenseID);

                if (_License.IsActive == true && _License.LicenseClassID == 3 && _License.ExpirationDate >= DateTime.Now)
                {
                    if (clsInternationalLicense.IsInternationalLicenseExist(_License.DriverID))
                    {
                        MessageBox.Show("Driver Has International License!!!!!!");
                        return;
                    }

                    _CreateAndSaveApplication();
                    _CreateAndSaveInternationalLicense();

                    MessageBox.Show("Created international license successfully!, With id " + _InternationalLicense.InternationalLicenseID.ToString());

                    lblInternationalAppID.Text = _Application.ApplicationID.ToString();
                    lblInternationallicenseID.Text = _InternationalLicense.InternationalLicenseID.ToString();
                    lblLocalLicenseID.Text = _License.LicenseID.ToString();
                    ctrlLicenseInfoWithFilter1.Enabled = false;
                }
                else
                {
                    MessageBox.Show("License not active or LicenseClass not Ordinary or License is Expired!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("You should enter licenseID!");
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            int licenseID = ctrlLicenseInfoWithFilter1.GetLicenseID();
            _License = clsLicense.Find(licenseID);

            frmLicenseHistory form = new frmLicenseHistory(_License.DriverID);
            form.ShowDialog();
        }
    }
}
