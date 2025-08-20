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

namespace DVLD.Controlrs
{
    public partial class ctrlUserInfo : UserControl
    {
        public ctrlUserInfo()
        {
            InitializeComponent();
        }

        clsUser User;

        public void LoadUserData(int UserID)
        {

            User = clsUser.FindUserByID(UserID);

            if (User == null)
            {
                MessageBox.Show("User " + UserID + " Not Found");
                return;
            }

            ctrlPersonInfo1.LoadPersonData(User.PersonID);

            lblUserID.Text = UserID.ToString();
            lblUserName.Text = User.UserName;
             cbIsActive.Checked = User.IsActive;

        }
        private void ctrlPersonInfo1_Load(object sender, EventArgs e)
        {
            //ctrlPersonInfo1.LoadPersonData();
        }
    }
}
