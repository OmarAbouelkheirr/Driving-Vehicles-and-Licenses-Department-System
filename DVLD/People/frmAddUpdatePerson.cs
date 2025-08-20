using System;
using System.Data;
using System.IO;
using System.Windows.Forms;
using BusinessLayer;
using DVLD.Properties;

namespace DVLD.People
{
    public partial class frmAddUpdatePerson : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;

        public int _PersonID;
        clsPerson _Person;

        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;

            if (_PersonID == -1)
            {
                _Mode = enMode.AddNew;
            }
            else
            {
                _Mode = enMode.Update;
            }
        }

        public const string folderPath = @"C:\DVLD-People-Images\";

        void _CreateFolderIsNotExists(string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
        }

        void _LoadCountiers()
        {
            DataTable dtCountries = clsCountries.GetCountries();

            foreach (DataRow dr in dtCountries.Rows)
            {
                cbCountries.Items.Add(dr["CountryName"]);
            }

            cbCountries.SelectedItem = "Egypt";
        }

        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            rbMale.Checked = true;
            dtpDateOfBirth.MaxDate = DateTime.Today.AddYears(-18);
            _LoadCountiers();
            _savePersonData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            pbPersonImage.Image = Resources.man;
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            pbPersonImage.Image = Resources.girl;
        }

        private void lblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            opfImage.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";

            if (opfImage.ShowDialog() == DialogResult.OK)
            {
                pbPersonImage.ImageLocation = opfImage.FileName;
                lblRemoveImg.Visible = true;
            }
        }

        string _copyAndRenameImage(string oldPath)
        {
            _CreateFolderIsNotExists(folderPath);

            string newName = Guid.NewGuid().ToString() + Path.GetExtension(oldPath);
            string destFile = Path.Combine(folderPath, newName);
            File.Copy(oldPath, destFile, overwrite: true);

            return newName;
        }

        void _savePersonData()
        {

            if (_Mode == enMode.AddNew)
            {
                lblMode.Text = "Add New Person";
                _Person = new clsPerson();
                return;
            }

            _Person = clsPerson.FindPersonByID(_PersonID);

            if (_Person == null) 
            {
                MessageBox.Show("Not found person " + _PersonID);
                this.Close();

                return;
            }

            lblMode.Text = "Update Person: " + _Person.PersonID.ToString();

            lblPersonID.Text = _PersonID.ToString();
            txtFName.Text = _Person.fName;
            txtSName.Text = _Person.sName;
            txtTName.Text = _Person.tName;
            txtLName.Text = _Person.lName;

            txtNationalNo.Text = _Person.NationalNo;
            txtAddress.Text = _Person.Address;
            txtEmail.Text = _Person.Email;
            txtPhone.Text = _Person.Phone;

            dtpDateOfBirth.Value = _Person.DateOfBirth;
            cbCountries.SelectedItem = clsCountries.GetCountryNameByID(_Person.NationalityCountryID);


            if (_Person.ImagePath != null)
            {
                pbPersonImage.ImageLocation = folderPath + _Person.ImagePath;
            }
            else if (_Person.ImagePath == "")
            {
                if (rbMale.Checked)
                {
                    pbPersonImage.Image = Resources.man;
                }
                else
                {
                    pbPersonImage.Image = Resources.girl;
                }
            }


            if (_Person.Gendor == 0)
            {
                rbMale.Checked = true;
            }
            else
            {
                rbFemale.Checked = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _Person.fName = txtFName.Text;
            _Person.sName = txtSName.Text;
            _Person.tName = txtTName.Text;
            _Person.lName = txtLName.Text;

            _Person.Address = txtAddress.Text;
            _Person.Email = txtEmail.Text;
            _Person.NationalNo = txtNationalNo.Text;
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            _Person.Phone = txtPhone.Text;
            _Person.NationalityCountryID = clsCountries.GetCountryIDByName(cbCountries.SelectedItem.ToString());

            if (pbPersonImage.ImageLocation != null)
                _Person.ImagePath = _copyAndRenameImage(pbPersonImage.ImageLocation);
            else
                _Person.ImagePath = "";

            if (rbMale.Checked)
                _Person.Gendor = 0;
            else
                _Person.Gendor = 1;

            if (_Person.Save())
            {
                MessageBox.Show("Person with ID " + _Person.PersonID + " Saved Successfully.");
                _Mode = enMode.Update;

                _PersonID = _Person.PersonID;
                lblMode.Text = "Update Person";
                lblPersonID.Text = _Person.PersonID.ToString();
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.");
        }

        private void lblRemoveImg_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (rbFemale.Checked)
                pbPersonImage.Image = Resources.girl;
            else
                pbPersonImage.Image = Resources.man;

            lblRemoveImg.Visible = false;
        }
    }
}