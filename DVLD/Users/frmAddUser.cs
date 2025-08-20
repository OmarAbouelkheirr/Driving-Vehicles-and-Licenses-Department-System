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

namespace DVLD.Users
{
    public partial class frmAddUser : Form
    {

        public frmAddUser()
        {
            InitializeComponent();
        }


        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            guna2TabControl1.SelectedIndex = 0;
            tabPage2.Enabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {

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

                if (clsUser.IsUserExistsByUserID(personID))
                {
                    MessageBox.Show("Selected person already in users!");
                    return;
                }

                tabPage2.Enabled = true;
                guna2TabControl1.SelectedIndex = 1;
                //MessageBox.Show( ctrlPersonInfoWithFilter1.GetSelectedPersonID().ToString());
            }
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void guna2TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


        private void guna2TabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (guna2TabControl1.SelectedIndex == 0)
                return;
            
            int personID = ctrlPersonInfoWithFilter1.GetSelectedPersonID();

            if (personID == -1 || personID == 0)
            {
                MessageBox.Show("You should select person!");

                e.Cancel = true;
                return;
            }

            if (clsUser.IsUserExistsByPersonID(personID))
            {
                MessageBox.Show("Selected person already in users!");

                e.Cancel = true;
                return;
            }

            tabPage2.Enabled = true;
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {

            int personID = ctrlPersonInfoWithFilter1.GetSelectedPersonID();

            if (clsUser.IsUserExistsByPersonID(personID))
            {
                MessageBox.Show("Selected person already in users!");

                return;
            }
            
            clsUser User = new clsUser();

            string username = txtUsername.Text;
            string password = txtPassword.Text;
            string conPassword = txtConfirmPassword.Text;

            if (String.IsNullOrWhiteSpace(conPassword) || String.IsNullOrWhiteSpace(password) || String.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Please fill all text boxs!");
                return;
            }

            if (clsUser.IsUserExistByUserName(username))
            {
                MessageBox.Show("this username is exist");
                return;
            }

            User.UserName = username;
            //User.Password =  password;
            User.Password = clsHelpers.ComputeHash(password);

            User.PersonID = personID;

            User.IsActive = cbIsUserActive.Checked;

            if (User.Save())
            {
                MessageBox.Show($"User Added Successfully with id {User.UserID}!");
            }
            
            lblUserID.Text = User.UserID.ToString();
        }
    }
}
