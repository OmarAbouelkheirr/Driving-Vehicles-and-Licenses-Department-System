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

namespace DVLD.Tests
{
    public partial class frmManageTestAppointments : Form
    {

        int _LocalAppID;
        clsLocalDrivingLicenseApplication _LocalApp;
        int _TestTypeID;
        clsTestType _TestType;

        public frmManageTestAppointments(int LocalAppID, int TestTypeID)
        {
            InitializeComponent();

            if (clsLocalDrivingLicenseApplication.IsLocalAppExistByID(LocalAppID))
            {
                _LocalAppID = LocalAppID;
                _TestTypeID = TestTypeID;
                _LocalApp = clsLocalDrivingLicenseApplication.FindLocalAppByID(LocalAppID);
                _TestType = clsTestType.FindTestTypeByID(_TestTypeID);
            }
            else
            {
                return;
            }
        }

        void _LoadAndRefreshData()
        {
            _LoadAppointments();
            _CountOfRecourds();
        }

        void _CountOfRecourds()
        {
            lblCount.Text = dgvAppointments.RowCount.ToString();
        }

        void _LoadAppointments()
        {
            dgvAppointments.DataSource = clsTestAppointment.GetTestAppointmentsByLocalAppID(_LocalAppID, _TestTypeID);
        }

        void _Header()
        {
            lblTestName.Text = _TestType.TestTypeTitle;
            ctrlAppsInfo1.loadAppsData(_LocalAppID);
            _LoadAndRefreshData();
        }

        private void frmManageTestAppointments_Load(object sender, EventArgs e)
        {
            _Header();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddAppointment_Click(object sender, EventArgs e)
        {
            //if (clsTestAppointment.IsLastTestFailed(_LocalAppID, _TestTypeID))
            //{
            //    frmScheduleTest form = new frmScheduleTest(_LocalAppID, _TestTypeID, -1, true);

            //    form.ShowDialog();
            //    _LoadAndRefreshData();

            //    return;
            //}

            //if (!clsTestAppointment.IfAppointmentIsActive(_LocalAppID, _TestTypeID))
            //{
            //    frmScheduleTest form = new frmScheduleTest(_LocalAppID, _TestTypeID, -1);

            //    form.ShowDialog();
            //    _LoadAndRefreshData();
            //}
            //else
            //{
            //    MessageBox.Show("Person has active appointment! ");
            //}


            bool hasActiveTest = clsTestAppointment.IfAppointmentIsActive(_LocalAppID, _TestTypeID);
            bool? lastTestFailed = clsTestAppointment.IsLastTestFailed(_LocalAppID, _TestTypeID);

            if (hasActiveTest)
            {
                MessageBox.Show("Person has active appointment!");
                return;
            }

            if (lastTestFailed == true)
            {
                frmScheduleTest form = new frmScheduleTest(_LocalAppID, _TestTypeID, -1, true);
                form.ShowDialog();
                _LoadAndRefreshData();
            }
            else if (lastTestFailed == false)
            {
                MessageBox.Show("Person has already passed the test. No need to schedule another.");
            }
            else
            {
                frmScheduleTest form = new frmScheduleTest(_LocalAppID, _TestTypeID, -1);
                form.ShowDialog();
                _LoadAndRefreshData();
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmScheduleTest form = new frmScheduleTest(_LocalAppID, _TestTypeID, (int)dgvAppointments.CurrentRow.Cells[0].Value);
            form.ShowDialog();
 
            _LoadAndRefreshData();
        }


        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int AppointmentID = (int)dgvAppointments.CurrentRow.Cells[0].Value;

            frmTakeTest form = new frmTakeTest(AppointmentID);
            form.ShowDialog();

            _LoadAndRefreshData();
        }


        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if ((bool)dgvAppointments.CurrentRow.Cells[6].Value)
            {
                editToolStripMenuItem.Enabled = false;
                takeTestToolStripMenuItem.Enabled = false;
            }
            else
            {
                editToolStripMenuItem.Enabled = true;
                takeTestToolStripMenuItem.Enabled = true;
            }
        }


    }
}
