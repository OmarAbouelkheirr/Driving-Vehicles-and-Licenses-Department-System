using System;
using System.Data;
using System.Windows.Forms;
using BusinessLayer;
using DVLD.License;
using DVLD.Local_Driving_License_Application;
using DVLD.Tests;

namespace DVLD.Applications.Local_Driving_License_Application
{
    public partial class frmLocalDrivingLicenseApplications : Form
    {
        public frmLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }

        void _LoadApps()
        {
            dgvApps.DataSource = clsLocalDrivingLicenseApplication.GetAllLocalApps();
        }

        void _CountOfApps()
        {
            lblCountOFApps.Text = dgvApps.RowCount.ToString();
        }

        void _LoadAndRefreshData()
        {
            _LoadApps();
            _CountOfApps();

            cbFilterBy.SelectedIndex = 0;
            txtFilterBy.Visible = false;

        }

        private void frmLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            _LoadAndRefreshData();
        }


        void _PrepareToFiltering()
        {
            txtFilterBy.Visible = true;
            cbStatus.Visible = false;
            txtFilterBy.Clear();
            txtFilterBy.Focus();
        }

        void _PrepareToFilteringByStatus()
        {
            cbStatus.SelectedItem = "All";
            cbStatus.Visible = true;
            txtFilterBy.Visible = false;
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddLocalDrivingLicenseApplication form = new frmAddLocalDrivingLicenseApplication();
            form.ShowDialog();

            _LoadAndRefreshData();
        }

        private void _CheckTests(int LocalAppID)
        {
            //int PassedTests = clsLocalDrivingLicenseApplication.GetPassedTestsCount(LocalAppID);

            //switch (PassedTests)
            //{
            //    case 0:
            //        visionTestToolStripMenuItem.Enabled = true;
            //        writtenTestToolStripMenuItem.Enabled = false;
            //        streetTestToolStripMenuItem.Enabled = false;
            //        break;

            //    case 1:
            //        visionTestToolStripMenuItem.Enabled = false;
            //        writtenTestToolStripMenuItem.Enabled = true;
            //        streetTestToolStripMenuItem.Enabled = false;
            //        break;

            //    case 2:
            //        visionTestToolStripMenuItem.Enabled = false;
            //        writtenTestToolStripMenuItem.Enabled = true;
            //        streetTestToolStripMenuItem.Enabled = false;
            //        break;

            //    case 3:
            //        visionTestToolStripMenuItem.Enabled = false;
            //        writtenTestToolStripMenuItem.Enabled = false;
            //        streetTestToolStripMenuItem.Enabled = false;
            //        break;
            //}

            int PassedTests = clsLocalDrivingLicenseApplication.GetPassedTestsCount(LocalAppID);

            if (PassedTests >= 3)
            {
                sechduleTestsToolStripMenuItem.Enabled = false;
                visionTestToolStripMenuItem.Enabled = false;
                writtenTestToolStripMenuItem.Enabled = false;
                streetTestToolStripMenuItem.Enabled = false;
                return;
            }

            sechduleTestsToolStripMenuItem.Enabled = true;
            visionTestToolStripMenuItem.Enabled = (PassedTests == 0);
            writtenTestToolStripMenuItem.Enabled = (PassedTests == 1);
            streetTestToolStripMenuItem.Enabled = (PassedTests == 2);


        }

        string _CurrentFilterColumn;
        DataTable _AppTable;


        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SelectedItem = cbFilterBy.SelectedItem.ToString();
            _AppTable = clsLocalDrivingLicenseApplication.GetAllLocalApps();

            switch (SelectedItem)
            {
                case "None":
                    txtFilterBy.Visible = false;
                    cbStatus.Visible = false;
                    _LoadAndRefreshData();
                    break;
                case "LDL AppID":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "LocalAppID";
                    break;
                case "National No":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "NationalNo";
                    break;
                case "Full Name":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "FullName";
                    break;
                case "Status":
                    _PrepareToFilteringByStatus();
                    break;
            }
        }

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

            _CountOfApps();
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            _ApplyLiveSearch(txtFilterBy.Text);
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SelectedItem = cbStatus.SelectedItem.ToString();
            _AppTable = clsLocalDrivingLicenseApplication.GetAllLocalApps();

            _CurrentFilterColumn = "Status";

            switch (SelectedItem)
            {
                case "All":
                    _LoadApps();
                    _CountOfApps();
                    break;
                case "New":
                    _ApplyLiveSearch("New");
                    break;
                case "Completed":
                    _ApplyLiveSearch("Completed");
                    break;
                case "Cancelled":
                    _ApplyLiveSearch("Cancelled");
                    break;
            }
        }

        private void showApplicationDedailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int AppID = (int)dgvApps.CurrentRow.Cells[0].Value;

            frmLocalDrivingLicenseApplicationInfo form = new frmLocalDrivingLicenseApplicationInfo(AppID);
            form.ShowDialog();
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int AppID = (int)dgvApps.CurrentRow.Cells[0].Value;

            if (clsLocalDrivingLicenseApplication.DeleteLocalApp(AppID))
            {
                MessageBox.Show("deleted successfully!");
                _LoadAndRefreshData();
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (dgvApps.CurrentRow == null)
            {
                e.Cancel = true;
                return;
            }

            string Status = (string)dgvApps.CurrentRow.Cells[5].Value;

            switch (Status)
            {
                case "New":

                    _CheckTests((int)dgvApps.CurrentRow.Cells[0].Value);

                    showLicenseToolStripMenuItem.Enabled = false;
                    showPersonLicenseHistoryToolStripMenuItem.Enabled = false;

                    editToolStripMenuItem.Enabled = true;
                    cancelApplicationToolStripMenuItem.Enabled = true;
                    //sechduleTestsToolStripMenuItem.Enabled = true;
                    issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = true;
                    showApplicationDedailsToolStripMenuItem.Enabled = true;
                    deleteApplicationToolStripMenuItem.Enabled = true;
                    break;

                case "Cancelled":
                    editToolStripMenuItem.Enabled = false;
                    cancelApplicationToolStripMenuItem.Enabled = false;
                    sechduleTestsToolStripMenuItem.Enabled = false;
                    showLicenseToolStripMenuItem.Enabled = false;
                    showPersonLicenseHistoryToolStripMenuItem.Enabled = false;
                    issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;

                    deleteApplicationToolStripMenuItem.Enabled = true;
                    showApplicationDedailsToolStripMenuItem.Enabled = true;

                    break;

                case "Completed":
                    editToolStripMenuItem.Enabled = false;
                    cancelApplicationToolStripMenuItem.Enabled = false;
                    sechduleTestsToolStripMenuItem.Enabled = false;
                    issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
                    deleteApplicationToolStripMenuItem.Enabled = false;

                    showPersonLicenseHistoryToolStripMenuItem.Enabled = true;
                    showApplicationDedailsToolStripMenuItem.Enabled = true;
                    showLicenseToolStripMenuItem.Enabled = true;
                    break;


                default:
                    e.Cancel = true;
                    break;
            }
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalAppID = (int)dgvApps.CurrentRow.Cells[0].Value;
            
            if(clsLocalDrivingLicenseApplication.Cancel(LocalAppID))
            {
                MessageBox.Show("Cancelled");
                _LoadAndRefreshData();
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        private void visionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalAppID = (int)dgvApps.CurrentRow.Cells[0].Value;
            //int TestTypeID = clsLocalDrivingLicenseApplication.GetPassedTestsCount(LocalAppID) + 1;

            frmManageTestAppointments form = new frmManageTestAppointments(LocalAppID, 1);
            form.ShowDialog();
            _LoadAndRefreshData();
        }

        private void writtenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalAppID = (int)dgvApps.CurrentRow.Cells[0].Value;

            frmManageTestAppointments form = new frmManageTestAppointments(LocalAppID, 2);
            form.ShowDialog();
            _LoadAndRefreshData();
        }

        private void streetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalAppID = (int)dgvApps.CurrentRow.Cells[0].Value;

            frmManageTestAppointments form = new frmManageTestAppointments(LocalAppID, 3);
            form.ShowDialog();
            _LoadAndRefreshData();
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalAppID = (int)dgvApps.CurrentRow.Cells[0].Value;

            frmIssueLicense form = new frmIssueLicense(LocalAppID);
            form.ShowDialog();
            _LoadAndRefreshData();
        }


        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalAppID = (int)dgvApps.CurrentRow.Cells[0].Value;

            frmShowlLicense form = new frmShowlLicense(-1 ,LocalAppID);
            form.ShowDialog();
            _LoadAndRefreshData();
        }
    }
}
