using System;
using System.Windows.Forms;
using BusinessLayer;

namespace DVLD.Controlrs
{
    public partial class ctrlLicenseInfoWithFilter : UserControl
    {
        public ctrlLicenseInfoWithFilter()
        {
            InitializeComponent();
        }

        private void ctrlLicenseInfoWithFilter_Load(object sender, EventArgs e)
        {

        }

        bool loaded = false;

        public int GetLicenseID()
        {
            return (loaded ? int.Parse(txtLicenseID.Text) : -1);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtLicenseID.Text == "")
            {
                MessageBox.Show("Enter ID!");
                txtLicenseID.Focus();
                return;
            }
            int licenseID = int.Parse(txtLicenseID.Text);
            clsLicense lis = clsLicense.Find(licenseID);
            
            if (lis != null)
            {
                ctrlLicenseInfo1.LoadLicenseData(licenseID);
                loaded = true;  
            }
            else
            {
                MessageBox.Show("License not found, try again!");
                loaded = false;
                txtLicenseID.Clear();
                txtLicenseID.Focus();
            }
        }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }
    }
}
