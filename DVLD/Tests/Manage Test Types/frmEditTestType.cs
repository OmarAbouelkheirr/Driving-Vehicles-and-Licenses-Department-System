using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;

namespace DVLD.Manage_Test_Types
{
    public partial class frmEditTestType : Form
    {
        int TestTypeID;
        clsManageTestType TestType;

        public frmEditTestType(int testType)
        {
            InitializeComponent();

            TestTypeID = testType;

            TestType = clsManageTestType.FindTestTypeByID(TestTypeID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmEditTestType_Load(object sender, EventArgs e)
        {
            lblID.Text = TestTypeID.ToString();

            txtFees.Text = TestType.TestTypeFees.ToString();
            txtTitle.Text = TestType.TestTypeTitle;
            txtDesc.Text = TestType.TestTypeDescription;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string testTitle = txtTitle.Text;
            string testDesc = txtDesc.Text;
            decimal testFees = decimal.Parse(txtFees.Text);

            TestType.TestTypeTitle = testTitle;
            TestType.TestTypeDescription = testDesc;
            TestType.TestTypeFees = testFees;
            
            if (TestType.UpdateTestType())
            {
                MessageBox.Show("Test Type Updated Successfully!");
            }
        }
    }
}
