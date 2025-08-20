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

namespace DVLD.People
{
    public partial class frmPersonDedails : Form
    {
        int _PersonID;
        
        public frmPersonDedails(int PersonID)
        {
            InitializeComponent();

            _PersonID = PersonID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPersonDedails_Load(object sender, EventArgs e)
        {
            if (clsPerson.FindPersonByID(_PersonID) != null)
            {
                ctrlPersonInfo1.LoadPersonData(_PersonID);
            }
            else
            {
                MessageBox.Show("Person " + _PersonID + " is not found!");
                return;
            }
        }

        private void ctrlPersonInfo1_Load(object sender, EventArgs e)
        {

        }
    }
}
