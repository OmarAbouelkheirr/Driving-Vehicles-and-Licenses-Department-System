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
    public partial class frmTakeTest : Form
    {
        int AppointentID;
        clsTestAppointment _Appointment;
        clsLocalDrivingLicenseApplication _LocalApp;

        public frmTakeTest(int appointentID)
        {
            InitializeComponent();

            AppointentID = appointentID;
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            _Appointment = clsTestAppointment.FindTestAppointmentByID(AppointentID);
            _LocalApp = clsLocalDrivingLicenseApplication.FindLocalAppByID(_Appointment.LocalDrivingLicenseApplicationID);

            lblTestName.Text = clsTestType.FindTestTypeByID(_Appointment.TestTypeID).TestTypeTitle;
            lblDLAppID.Text = _LocalApp.LocalDrivingLicenseApplicationID.ToString();
            lblClass.Text = clsLicenseClasses.FindClassByID(_LocalApp.LicenseClassID).ClassName;
            lblName.Text = _LocalApp.AppData.PersonData.FullName();
            lblTrial.Text = "___";
            lblDate.Text = _Appointment.AppointmentDate.ToString("dd/MM/yyyy");
            lblFees.Text = _Appointment.PaidFees.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            clsTest _Test = new clsTest();

            _Test.TestAppointmentID = AppointentID;
            _Test.Notes = txtBoxNotes.Text;
            _Test.TestResult = rbPass.Checked;
            _Test.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;

            _Appointment.IsLocked = true;

            if (_Test.Save() && _Appointment.Save())
            {
                MessageBox.Show($"Test Saved Succefully With ID: {_Test.TestID.ToString()}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblTestID.Text = _Test.TestID.ToString();
                this.Close();
            }
            else
            {
                MessageBox.Show("Test Was not Saved Succefully", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
