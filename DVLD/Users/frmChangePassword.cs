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
    public partial class frmChangePassword : Form
    {
        int userID;
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;

        clsUser user;

        public frmChangePassword(int userID)
        {
            InitializeComponent();

            _Mode = enMode.Update;

            this.userID = userID;
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            ctrlUserInfo1.LoadUserData(userID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string currentPassword = txtCurrentPassword.Text;
            string newPassword = txtNewPassword.Text;

            user = clsUser.FindUserByID(userID);

            if (user != null)
            {
                if (currentPassword == user.Password)
                {
                    user.Password = newPassword;

                    if (user.Save())
                    {
                        MessageBox.Show("Updated Password!");
                        return;
                    }
                    else
                    {
                        MessageBox.Show("Error");
                        return;
                    }
                }
            }
            else
            {
                MessageBox.Show("User Not Found!");
            }


        }
    }
}
