using System;
using System.Data;
using System.Windows.Forms;
using BusinessLayer;

namespace DVLD.Local_Driving_License_Application
{
    public partial class frmAddLocalDrivingLicenseApplication : Form
    {
        int _LocalAppID = -1;
        clsLocalDrivingLicenseApplication _Application;

        public frmAddLocalDrivingLicenseApplication()
        {
            InitializeComponent();
        }
        public frmAddLocalDrivingLicenseApplication(int LocalAppID)
        {
            InitializeComponent();

            _LocalAppID = LocalAppID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        void _FillClasses()
        {
            DataTable dtAllClasses = clsLicenseClasses.GetAllClasses();
            foreach (DataRow Row in dtAllClasses.Rows)
            {
                cbClasses.Items.Add(Row["ClassName"]);
            }

            cbClasses.SelectedIndex = 0;
        }

        void _AddNewLocalApp()
        {
            lblAppFees.Text = clsApplicationType.FindAppTypeByID(1).AppTypeFees.ToString();
            lblDate.Text = DateTime.Now.ToString("dd/MM/yyyy"); ;
            lblCreatedBy.Text = clsGlobalSettings.LoggedInUser.PersonData.fName;
        }

        void _UpdateLocalApp()
        {
            lblAppFees.Text = _Application.AppData.PaidFees.ToString();
            lblDate.Text = _Application.AppData.ApplicationDate.ToString(); ;
            lblCreatedBy.Text = _Application.AppData.CreatedByUserID.ToString();
            cbClasses.SelectedIndex = _Application.LicenseClassID;
            lblID.Text = _Application.LocalDrivingLicenseApplicationID.ToString();

            _Application.AppData.Mode = clsApplication.enMode.Update;
            _Application.Mode = clsLocalDrivingLicenseApplication.enMode.Update;
        }

        void _SaveLocalApp()
        {
            _Application.LicenseClassID = cbClasses.SelectedIndex + 1;

            _Application.AppData.ApplicationStatus = 1;
            _Application.AppData.ApplicationTypeID = 1;
            _Application.AppData.ApplicationDate = DateTime.Now;
            _Application.AppData.LastStatusDate = DateTime.Now;
            _Application.AppData.PaidFees = clsApplicationType.FindAppTypeByID(1).AppTypeFees;
            _Application.AppData.CreatedByUserID = clsGlobalSettings.LoggedInUser.UserID;

            _Application.AppData.ApplicantPersonID = ctrlPersonInfoWithFilter1.GetSelectedPersonID();
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            //_FillClasses();
        }

        private void frmAddLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            _FillClasses();

            if (_LocalAppID == -1)
            {
                _AddNewLocalApp();

                _Application = new clsLocalDrivingLicenseApplication();
                _Application.Mode = clsLocalDrivingLicenseApplication.enMode.AddNew;

                guna2TabControl1.SelectedIndex = 0;
                tabPage2.Enabled = false;
            }
            else
            {
                _Application = clsLocalDrivingLicenseApplication.FindLocalAppByID(_LocalAppID);

                ctrlPersonInfoWithFilter1.LoadPersonData(_Application.AppData.ApplicantPersonID);

                _UpdateLocalApp();
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            int personID = ctrlPersonInfoWithFilter1.GetSelectedPersonID();

            if (personID == -1)
            {
                MessageBox.Show("You should select person!");
            }
            else
            {

                tabPage2.Enabled = true;
                guna2TabControl1.SelectedIndex = 1;
            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            int personID = ctrlPersonInfoWithFilter1.GetSelectedPersonID();

            _SaveLocalApp();
            
            if (clsLocalDrivingLicenseApplication.IfPersonHasActiveLocalApplication(personID, cbClasses.SelectedIndex + 1))
            {
                MessageBox.Show("This person has active local application!");
                return;
            }

            if (_Application.Save())
            {
                MessageBox.Show("Added successfully!");
                lblID.Text = _Application.LocalDrivingLicenseApplicationID.ToString();
            }
            else
            {
                MessageBox.Show("Error");
            }
        }
    }
}
