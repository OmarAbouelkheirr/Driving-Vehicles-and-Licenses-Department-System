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
using Guna.UI2.WinForms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Applications
{
    public partial class frmRenewLicense : Form
    {
        clsApplication _RenewApplication;
        clsLicense _NewLicense;
        clsLicense _OldLicense;
        decimal ApplicationFees = clsApplicationType.FindAppTypeByID(2).AppTypeFees;
        int UserID = clsGlobalSettings.LoggedInUser.UserID;

        public frmRenewLicense()
        {
            InitializeComponent();
        }

        void fillApplication()
        {
            lblAppDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblIssueDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblApplicationFees.Text = ApplicationFees.ToString();
            lblCreatedBy.Text = UserID.ToString();
        }

        private void frmRenewLicense_Load(object sender, EventArgs e)
        {
            fillApplication();
        }

        bool _IssueRenewedLicense()
        {
            _NewLicense = new clsLicense();

            _NewLicense.ApplicationID = _RenewApplication.ApplicationID;
            _NewLicense.DriverID = _OldLicense.DriverID;
            _NewLicense.LicenseClassID = _OldLicense.LicenseClassID;
            _NewLicense.IssueDate = DateTime.Now;
            _NewLicense.ExpirationDate = DateTime.Now.AddYears(clsLicenseClasses.FindClassByID(_OldLicense.LicenseClassID).DefaultValidityLength);
            _NewLicense.Notes = txtNotes.Text;
            _NewLicense.PaidFees = clsLicenseClasses.FindClassByID(_OldLicense.LicenseClassID).ClassFees;
            _NewLicense.IsActive = true;
            _NewLicense.IssueReason = 2;
            _NewLicense.CreatedByUserID = UserID;
            _RenewApplication.ApplicationStatus = 3;

            _RenewApplication.Mode = clsApplication.enMode.Update;

            return _NewLicense.Save() && _RenewApplication.Save();
        }

        bool _CreateAndSaveRenewApplication()
        {
            // renew application!
            _RenewApplication = new clsApplication();
            _RenewApplication.ApplicantPersonID = _OldLicense.DriverData.PersonID;
            _RenewApplication.ApplicationDate = DateTime.Now;
            _RenewApplication.ApplicationTypeID = 2;
            _RenewApplication.ApplicationStatus = 1;
            _RenewApplication.LastStatusDate = DateTime.Now;
            _RenewApplication.PaidFees = ApplicationFees;
            _RenewApplication.CreatedByUserID = UserID;

            return _RenewApplication.Save();
        }
        bool _DeactivateOldLicense(int licenseID)
        {
            if(ifLicenseIsExpired(licenseID))
            {
                _OldLicense.IsActive = false;
                _OldLicense.Mode = clsLicense.enMode.Update;

                return _OldLicense.Save();
            }
            else
            {
                return false;
            }
        }

        bool ifLicenseIsExpired(int licenseID)
        {
            _OldLicense = clsLicense.Find(licenseID);

            return _OldLicense.ExpirationDate < DateTime.Now;
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            int licenseID = ctrlLicenseInfoWithFilter1.GetLicenseID();

            if (licenseID != -1)
            {
                _OldLicense = clsLicense.Find(licenseID);

                if (_OldLicense.IsActive)
                {
                    if (_DeactivateOldLicense(licenseID))
                    {
                        if (_CreateAndSaveRenewApplication())
                        {
                            if (_IssueRenewedLicense())
                            {
                                MessageBox.Show("Renewed LicenseID " + _OldLicense.LicenseID + " And New LicenseID Is " + _NewLicense.LicenseID);
                                lblExipirationDate.Text = _NewLicense.ExpirationDate.ToString("dd/MM/yyyy");
                                lblLicenseFees.Text = _NewLicense.PaidFees.ToString();
                                lblTotalFees.Text = (_NewLicense.PaidFees + _RenewApplication.PaidFees).ToString();
                                lblOldLicenseID.Text = _OldLicense.LicenseID.ToString();
                                lblRenewAppID.Text = _RenewApplication.ApplicationID.ToString();
                                lblRenewedlicenseID.Text = _NewLicense.LicenseID.ToString();
                                btnRenew.Enabled = false;
                            }
                            else
                            {
                                MessageBox.Show("Errorrrrrrrrrrrrrrrrrrrr");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("This license is not expired!");
                    }
                }
                else
                {
                    MessageBox.Show("License Not Active!");
                }
            }
            else
            {
                MessageBox.Show("You should enter LicenseID!");
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