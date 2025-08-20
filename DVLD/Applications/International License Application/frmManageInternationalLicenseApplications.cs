using System;
using System.Data;
using System.Windows.Forms;
using BusinessLayer;
using DVLD.License;
using DVLD.People;

namespace DVLD.Applications.International_License_Application
{
    public partial class frmManageInternationalLicenseApplications : Form
    {
        public frmManageInternationalLicenseApplications()
        {
            InitializeComponent();
        }

        void _loadData()
        {
            dgvApps.DataSource = clsInternationalLicense.GetAllInternationalLicenses();
        }

        void _countOfRows()
        {
            lblCountOFApps.Text = dgvApps.RowCount.ToString();
        }


        void _LoadAndRefreshData()
        {
            _loadData();
            _countOfRows();

            cbFilterBy.SelectedIndex = 0;
            txtFilterBy.Visible = false;
        }

        private void frmManageInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            _LoadAndRefreshData();
        }

        private void btnAddInternational_Click(object sender, EventArgs e)
        {
            frmAddInternationalLicenseApplication form = new frmAddInternationalLicenseApplication();
            form.ShowDialog();

            _LoadAndRefreshData();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvApps.CurrentRow.Cells[2].Value;

            clsDriver driver = clsDriver.Find(DriverID);

            if (driver != null)
            {
                frmPersonDedails form = new frmPersonDedails(driver.PersonID);
                form.ShowDialog();
            }
            else
            {
                MessageBox.Show("Driver not found!");
            }
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void showPersoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dgvApps.CurrentRow.Cells[2].Value;

            frmLicenseHistory form = new frmLicenseHistory(DriverID);
            form.ShowDialog();
        }


        void _PrepareToFiltering()
        {
            txtFilterBy.Visible = true;
            txtFilterBy.Clear();
            txtFilterBy.Focus();
        }

        string _CurrentFilterColumn;
        DataTable _AppTable;
        
        void _ApplyLiveSearch(string searchText)
        {
            if (_AppTable == null) return;

            searchText = searchText.Trim().Replace("'", "''");

            DataView dv = _AppTable.DefaultView;

            if (string.IsNullOrEmpty(_CurrentFilterColumn) || string.IsNullOrEmpty(searchText))
            {
                dv.RowFilter = "";
            }
            else
            {
                dv.RowFilter = $"Convert({_CurrentFilterColumn}, 'System.String') LIKE '%{searchText}%'";
            }

            dgvApps.DataSource = dv;

            _countOfRows();
        }


        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SelectedItem = cbFilterBy.SelectedItem.ToString();
            _AppTable = clsInternationalLicense.GetAllInternationalLicenses();

            switch (SelectedItem)
            {
                case "None":
                    txtFilterBy.Visible = false;
                    _LoadAndRefreshData();
                    break;
                case "IntLicense":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "IntLicenseID";
                    break;
                case "LocLicense":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "LocLicenseID";
                    break;
                case "AppID":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "ApplicationID";
                    break;
            }
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            _ApplyLiveSearch(txtFilterBy.Text);
        }

    }
}
