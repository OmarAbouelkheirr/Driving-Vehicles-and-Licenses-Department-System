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
using Guna.UI2.WinForms;

namespace DVLD.Tests
{
    public partial class frmScheduleTest : Form
    {

        int _LocalAppID;
        clsLocalDrivingLicenseApplication _LocalApp;
        int _TestTypeID;
        clsTestType _TestType;
        int AppointentID;

        clsTestAppointment _Appointment;


        bool retake;


        public frmScheduleTest(int LocalAppID, int TestTypeID, int AppointentID, bool _retake = false)
        {
            InitializeComponent();

            if (clsLocalDrivingLicenseApplication.IsLocalAppExistByID(LocalAppID))
            {
                _LocalAppID = LocalAppID;
                _TestTypeID = TestTypeID;
                _LocalApp = clsLocalDrivingLicenseApplication.FindLocalAppByID(LocalAppID);
                _TestType = clsTestType.FindTestTypeByID(_TestTypeID);
                this.AppointentID = AppointentID;

                retake = _retake;
            }
            else
            {
                return;
            }
        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            guna2GroupBox1.Text = _TestType.TestTypeTitle;

            lblClass.Text = clsLicenseClasses.FindClassByID(_LocalApp.LicenseClassID).ClassName;
            lblLocalAppID.Text = _LocalAppID.ToString();
            lblName.Text = _LocalApp.AppData.PersonData.FullName();
            lblTrail.Text = "____";
            lblFees.Text = _TestType.TestTypeFees.ToString();

            if (retake)
            {
                RetakeGroupBox.Enabled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        void _addNew()
        {
            _Appointment = new clsTestAppointment();

            _Appointment.TestTypeID = _TestTypeID;
            _Appointment.LocalDrivingLicenseApplicationID = _LocalAppID;
            _Appointment.PaidFees = _TestType.TestTypeFees;
            _Appointment.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;
            _Appointment.AppointmentDate = dtpDate.Value;

            if (_Appointment.Save())
            {
                MessageBox.Show("saved by id " + _Appointment.TestAppointmentID);
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        void _Update()
        {
            _Appointment = clsTestAppointment.FindTestAppointmentByID(AppointentID);

            _Appointment.AppointmentDate = dtpDate.Value;

            if (_Appointment.Save())
            {
                MessageBox.Show("Updated AppointentID " + _Appointment.TestAppointmentID);
            }
            else
            {
                MessageBox.Show("Error");
            }
        }

        //void _Retake()
        //{
        //    clsApplication RetakeApplication = new clsApplication();

        //    RetakeApplication = _LocalApp.AppData;
        //    RetakeApplication.ApplicationStatus = 1;
        //    RetakeApplication.ApplicationDate = DateTime.Now;
        //    RetakeApplication.ApplicationTypeID = 7;

        //    _Appointment = new clsTestAppointment();

        //    _Appointment.TestTypeID = _TestTypeID;
        //    _Appointment.LocalDrivingLicenseApplicationID = _LocalAppID;
        //    _Appointment.PaidFees = _TestType.TestTypeFees;
        //    _Appointment.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;
        //    _Appointment.AppointmentDate = dtpDate.Value;

        //    if (RetakeApplication.Save())
        //    {
        //        MessageBox.Show("Retake app id " + RetakeApplication.ApplicationID.ToString());

        //        lblRetakeFees.Text = RetakeApplication.ApplicationID.ToString();
        //        lblRetakeTestID.Text = _Appointment.TestAppointmentID.ToString();

        //        _Appointment.RetakeTestApplicationID = RetakeApplication.ApplicationID;

        //        if (_Appointment.Save())
        //        {
        //            MessageBox.Show("Retake test done and new appointment is " + _Appointment.TestAppointmentID.ToString());
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("Error");
        //    }
        //}

        void _Retake()
        {
            clsApplication RetakeApplication = new clsApplication();
            RetakeApplication.Mode = clsApplication.enMode.AddNew;

            RetakeApplication.ApplicantPersonID = _LocalApp.AppData.ApplicantPersonID;
            RetakeApplication.ApplicationStatus = 1;
            RetakeApplication.ApplicationDate = DateTime.Now;
            RetakeApplication.ApplicationTypeID = 7;
            RetakeApplication.PaidFees = _TestType.TestTypeFees;
            RetakeApplication.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;

            if (RetakeApplication.Save())
            {
                MessageBox.Show("Retake app id " + RetakeApplication.ApplicationID.ToString());

                _Appointment = new clsTestAppointment();
                _Appointment.Mode = clsTestAppointment.enMode.AddNew;

                _Appointment.TestTypeID = _TestTypeID;
                _Appointment.LocalDrivingLicenseApplicationID = _LocalAppID;
                _Appointment.PaidFees = _TestType.TestTypeFees;
                _Appointment.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;
                _Appointment.AppointmentDate = dtpDate.Value;
                _Appointment.RetakeTestApplicationID = RetakeApplication.ApplicationID;

                if (_Appointment.Save())
                {
                    MessageBox.Show("Retake test done and new appointment is " + _Appointment.TestAppointmentID.ToString());
                    //lblRetakeFees.Text = RetakeApplication.ApplicationID.ToString();
                    lblRetakeFees.Text = "5";
                    decimal total = 5 + _TestType.TestTypeFees;
                    lblTotalFees.Text = total.ToString();
                    lblRetakeTestID.Text = _Appointment.TestAppointmentID.ToString();
                }
            }
            else
            {
                MessageBox.Show("Error");
            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {   
            if (retake)
            {
                _Retake();
                return;
            }

            if(AppointentID == -1) 
            {
                _addNew();
            }
            else
            {
                _Update();
            }
        }
    }
}
