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
    public partial class frmManageDetainedLicenses : Form
    {
        public frmManageDetainedLicenses()
        {
            InitializeComponent();
        }


        void _loadData()
        {
            dgvLicenses.DataSource = clsDetainedLicense.GetAllDetainedLicenses();
        }
        void _countOfRows()
        {
            lblCountOFLicenses.Text = dgvLicenses.RowCount.ToString();
        }
        void _LoadAndRefreshData()
        {
            _loadData();
            _countOfRows();

            cbFilterBy.SelectedIndex = 0;
            txtFilterBy.Visible = false;
        }

        private void frmManageDetainAndReleaseLicenses_Load(object sender, EventArgs e)
        {
            _LoadAndRefreshData();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense form = new frmReleaseDetainedLicense();
            form.ShowDialog();
         
            _LoadAndRefreshData();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            frmDedainLicense form = new frmDedainLicense();
            form.ShowDialog();

            _LoadAndRefreshData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
