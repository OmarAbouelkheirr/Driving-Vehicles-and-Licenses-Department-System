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
    public partial class ctrlPersonInfo : UserControl
    {
        public ctrlPersonInfo()
        {
            InitializeComponent();
        }

        clsPerson Person;
        public int _PersonID;

        public const string folderPath = @"C:\DVLD-People-Images\";

        public void LoadPersonData(int PersonID)
        {
            Person = clsPerson.FindPersonByID(PersonID);

            if (Person == null)
            {
                MessageBox.Show("Person " + PersonID + " Not Found");
                return;
            }

            _PersonID = PersonID;

            lblName.Text = Person.FullName();
            lblNational.Text = Person.NationalNo;
            lblCountry.Text = clsCountries.GetCountryNameByID(Person.NationalityCountryID);
            lblPhone.Text = Person.Phone;
            lblEmail.Text = Person.Email;

            lblGendor.Text = (Person.Gendor == 0 ? "Male" : "Female");
            lblPersonID.Text = PersonID.ToString();

            lblAdrress.Text = Person.Address;
            lblDateOfBirth.Text = Person.DateOfBirth.ToString("dd/MM/yyyy");

            pbPersonImage.ImageLocation = folderPath + Person.ImagePath;
        }

        public void LoadPersonData(string NationalNo)
        {
            Person = clsPerson.FindPersonByNationalNo(NationalNo);

            if (Person == null)
            {
                MessageBox.Show("Person " + Person + " Not Found");
                return;
            }

            _PersonID = Person.PersonID;

            lblName.Text = Person.FullName();
            lblNational.Text = Person.NationalNo;
            lblCountry.Text = clsCountries.GetCountryNameByID(Person.NationalityCountryID);
            lblPhone.Text = Person.Phone;
            lblEmail.Text = Person.Email;

            lblGendor.Text = (Person.Gendor == 0 ? "Male" : "Female");
            lblPersonID.Text = Person.PersonID.ToString();

            lblAdrress.Text = Person.Address;
            lblDateOfBirth.Text = Person.DateOfBirth.ToString("dd/MM/yyyy");

            pbPersonImage.ImageLocation = folderPath + Person.ImagePath;
        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {

        }

        private void ctrlPersonInfo_Load(object sender, EventArgs e)
        {

        }

        private void lblEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson form = new frmAddUpdatePerson(_PersonID);
            form.ShowDialog();

            LoadPersonData(_PersonID);
        }
    }
}
