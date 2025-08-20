using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using BusinessLayer;
using DVLD.People;

namespace DVLD.Controlrs
{
    public partial class ctrlPersonInfoWithFilter : UserControl
    {
        public ctrlPersonInfoWithFilter()
        {
            InitializeComponent();

            cbFilterBy.SelectedIndex = 0;
        }

        public int addedPersonID;

        public int GetSelectedPersonID()
        {
            int personID = ctrlPersonInfo1._PersonID;

            if (clsPerson.FindPersonByID(personID) == null)
            {
                return -1;
            }
            else
            {
                return personID;
            }
        }

        public void LoadPersonData(int PersonID)
        {
            cbFilterBy.Enabled = false;
            btnAddUser.Enabled = false;
            btnSearch.Enabled = false;

            ctrlPersonInfo1.LoadPersonData(PersonID);
        }
        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson form = new frmAddUpdatePerson(-1);
            form.ShowDialog();
            
            addedPersonID = form._PersonID;

            if (addedPersonID != -1)
            {
                ctrlPersonInfo1.LoadPersonData(addedPersonID);
                ClearFilter();
            }
        }

        private void guna2GroupBox1_Click(object sender, EventArgs e)
        {

        }

        string selectedItem;

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {

            selectedItem = cbFilterBy.SelectedItem.ToString();

            switch (selectedItem)
            {
                case "None":
                    txtFilter.Clear();
                    txtFilter.Enabled = btnSearch.Enabled = false;
                     break;
                case "Person ID":
                case "National No.":
                    txtFilter.Enabled = btnSearch.Enabled = true;
                    txtFilter.Clear();
                    txtFilter.Focus(); break;
                default:
                    break;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtFilter.Text))
            {
                txtFilter.Focus();
                return;
            }

            switch (selectedItem)
            {
                case "National No.":
                    ctrlPersonInfo1.LoadPersonData(txtFilter.Text.ToString());
                    break;
                case "Person ID":
                    ctrlPersonInfo1.LoadPersonData(int.Parse(txtFilter.Text));
                    break;
                default:
                    break;
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {

        }

        public void ClearFilter()
        {
            txtFilter.Enabled = btnSearch.Enabled = true;
            txtFilter.Clear();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (selectedItem == "Person ID")
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }
    }
}
