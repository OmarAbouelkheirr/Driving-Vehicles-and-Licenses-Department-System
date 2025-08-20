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

namespace DVLD.Applications.Dedain_And_Release_Licenses
{
    public partial class frmDedainLicense : Form
    {
        int UserID = clsGlobalSettings.LoggedInUser.UserID;
        clsDetainedLicense _Detain;


        public frmDedainLicense()
        {
            InitializeComponent();
        }

        void fillDetainInfo()
        {
            lblDetainDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblCreatedBy.Text = UserID.ToString();
        }

        private void frmDedainLicense_Load(object sender, EventArgs e)
        {
            fillDetainInfo();
        }

        private void txtFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }


        bool _DetainLicense()
        {
            _Detain = new clsDetainedLicense();

            _Detain.LicenseID = ctrlLicenseInfoWithFilter1.GetLicenseID();
            _Detain.DetainDate = DateTime.Now;
            _Detain.FineFees = int.Parse(txtFineFees.Text);
            _Detain.CreatedByUserID = UserID;
            
            return _Detain.Save();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            int licenseID = ctrlLicenseInfoWithFilter1.GetLicenseID();

            if (licenseID == -1)
            {
                MessageBox.Show("You should enter licenseID!");
                return;
            }
            
            if (clsDetainedLicense.IsLicenseDetained(licenseID))
            {
                MessageBox.Show("This license is already detained!");
                return;
            }

            if (txtFineFees.Text == "")
            {
                MessageBox.Show("Enter tha fine fees!!!");
                txtFineFees.Focus();
                return;
            }

            if(_DetainLicense())
            {
                MessageBox.Show($"Done detain and Fine Fees {_Detain.FineFees}");
                lblDetainID.Text = _Detain.DetainID.ToString();
                lbllicenseID.Text = _Detain.LicenseID.ToString();
                btnDetain.Enabled = false;
            }
            else
            {
                MessageBox.Show("Can't detain!");
            }

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
