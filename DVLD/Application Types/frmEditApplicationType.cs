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

namespace DVLD.Application_Types
{
    public partial class frmEditApplicationType : Form
    {
        int AppTypeID;

        clsApplicationType AppType;
        public frmEditApplicationType(int AppTypeID)
        {
            InitializeComponent();

            this.AppTypeID = AppTypeID;

            AppType = clsApplicationType.FindAppTypeByID(AppTypeID);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (AppType == null)
            {
                MessageBox.Show("Application Type Not Found");
            }
            else
            {
                string appTitle = txtTitle.Text;
                decimal appFees = decimal.Parse(txtFees.Text);

                AppType.AppTypeTitle = appTitle;
                AppType.AppTypeFees = appFees;

                if (AppType.UpdateAppType())
                {
                    MessageBox.Show("Application Type Updates Successfully!");
                }
            }
        }

        private void frmEditApplicationType_Load(object sender, EventArgs e)
        {
            lblID.Text = AppTypeID.ToString();

            txtFees.Text = AppType.AppTypeFees.ToString();
            txtTitle.Text = AppType.AppTypeTitle;
        }
    }
}
