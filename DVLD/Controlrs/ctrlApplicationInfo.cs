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
using DVLD.People;

namespace DVLD.Controlrs
{
    public partial class ctrlApplicationInfo : UserControl
    {
        clsApplication _Application;

        public ctrlApplicationInfo()
        {
            InitializeComponent();
        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {

        }


        public void loadApplicationData(int AppID)
        {
            if (!clsApplication.IsApplicationExistByAppID(AppID)) 
            {
                MessageBox.Show("Application Not Found!");
                return;
            }

            _Application = clsApplication.FindApplicationByAppID(AppID);

            lblAppID.Text = AppID.ToString();
            lblStatus.Text = _Application.ApplicationStatus.ToString();
            lblFees.Text = _Application.PaidFees.ToString();
            lblType.Text = clsApplicationType.FindAppTypeByID(_Application.ApplicationTypeID).AppTypeTitle;
            lblPersonName.Text = _Application.PersonData.FullName();
            lblDate.Text = _Application.ApplicationDate.ToString();
            lblStatusDate.Text = _Application.LastStatusDate.ToString();
            lblCreatedBy.Text = _Application.CreatedByUserID.ToString();
        }

        private void ctrlApplicationInfo_Load(object sender, EventArgs e)
        {
            
        }

        private void btnPersonInfo_Click(object sender, EventArgs e)
        {
            int personID = _Application.ApplicantPersonID;

            if (!clsPerson.isPersonExistByID(personID))
            {
                MessageBox.Show("Sorry Person Not Found!");
                return;
            }

            frmPersonDedails form = new frmPersonDedails(personID);
            form.ShowDialog();
        }
    }
}
