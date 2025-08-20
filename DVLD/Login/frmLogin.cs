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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Configuration;

namespace DVLD.Login
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        //string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD\CurrentUser";
        string keyPath = ConfigurationManager.AppSettings["RegistryPath"];
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Clear();
                txtUsername.Clear();
                return;
            }

            clsUser user = clsUser.FindUserByUserName(userName);

            if (user == null)
            {
                MessageBox.Show("Invalid username.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                txtUsername.Clear();
                return;
            }

            string currentUserPasswordHash = user.Password;
            if (currentUserPasswordHash != clsHelpers.ComputeHash(password))
            {
                MessageBox.Show("Invalid password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPassword.Clear();
                return;
            }


            if (chRememberMe.Checked)
            {
                if (!clsHelpers.StorePasswordRegistry(keyPath, currentUserPasswordHash) ||
                    !clsHelpers.StoreUsernameRegistry(keyPath, userName))
                {
                    MessageBox.Show("Failed to store login information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            clsGlobalSettings.LoggedInUser = user;

            this.Hide();
            Main formMain = new Main();
            formMain.ShowDialog();
            this.Close();
        }


        private void frmLogin_Load(object sender, EventArgs e)
        {
            string storedUserName = clsHelpers.GetUsernameFromRegistry(keyPath);
            string storedEncryptedPassword = clsHelpers.GetPasswordFromRegistry(keyPath);

            if (string.IsNullOrEmpty(storedUserName) || string.IsNullOrEmpty(storedEncryptedPassword))
                return;


            clsUser user = clsUser.FindUserByUserName(storedUserName);

            if (user != null && user.Password == storedEncryptedPassword)
            {
                clsGlobalSettings.LoggedInUser = user;

                this.Hide();
                Main formMain = new Main();
                formMain.ShowDialog();
                this.Close();
            }
        }
    }
}
