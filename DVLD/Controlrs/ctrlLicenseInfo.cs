using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;

namespace DVLD.Controlrs
{
    public partial class ctrlLicenseInfo : UserControl
    {
        public ctrlLicenseInfo()
        {
            InitializeComponent();
        }

        //string _GetReason(short reasonNum)
        //{
        //    string Reason;

        //    switch (reasonNum)
        //    {
        //        case 1:
        //            Reason = "First Time";
        //            break;
        //        case 2:
        //            Reason = "Renew";
        //            break;
        //        case 3:
        //            Reason = "Replacement for Damaged";
        //            break;
        //        case 4:
        //            Reason = "Replacement for Lost";
        //            break;
        //        default:
        //            Reason = "Unknown";
        //            break;
        //    }

        //    return Reason;
        //}

        // ChatGPT
        string _GetReason(short reasonNum)
        {
            switch (reasonNum)
            {
                case 1: return "First Time";
                case 2: return "Renew";
                case 3: return "Replacement for Damaged";
                case 4: return "Replacement for Lost";
                default: return "Unknown";
            }
        }


        public void LoadLicenseData(int licenseID)
        {
            clsLicense _License = clsLicense.Find(licenseID);

            if (_License != null)
            {
                ctrlPersonInfo1.LoadPersonData(_License.DriverData.PersonID);

                lblLicenseID.Text = _License.LicenseID.ToString();
                lblClass.Text = clsLicenseClasses.FindClassByID(_License.LicenseClassID).ClassName;
                lblDriverID.Text = _License.DriverID.ToString();
                lblExDate.Text = _License.ExpirationDate.ToString("dd/MM/yyyy");
                lblIssueDate.Text = _License.IssueDate.ToString("dd/MM/yyyy");
                lblReason.Text = _GetReason(_License.IssueReason);
                lblNotes.Text = _License.Notes.ToString();
                lblIsActive.Text = _License.IsActive ? "Yes" : "No";
                lblIsDetained.Text = clsDetainedLicense.IsLicenseDetained(licenseID) ? "Yes" : "No";
            }
            else
            {
                MessageBox.Show("Lisense not found!!!!!!");
                return;
            }

        }

        private void ctrlLicenseInfo_Load(object sender, EventArgs e)
        {

        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
