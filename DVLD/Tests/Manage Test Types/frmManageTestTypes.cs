using System;
using System.Windows.Forms;
using BusinessLayer;
using DVLD.Application_Types;

namespace DVLD.Manage_Test_Types
{
    public partial class frmManageTestTypes : Form
    {
        public frmManageTestTypes()
        {
            InitializeComponent();
        }

        private void frmManageTestTypes_Load(object sender, EventArgs e)
        {
            _LoadAndRefreshData();
        }


        void _CountOfRecourds()
        {
            lblCount.Text = dgvTestTypes.RowCount.ToString();
        }

        void _LoadAndRefreshData()
        {
            _LoadTestTypes();
            _CountOfRecourds();
        }

        void _LoadTestTypes()
        {
            dgvTestTypes.DataSource = clsManageTestType.GetTestTypes();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestTypeID = (int)dgvTestTypes.CurrentRow.Cells[0].Value;

            frmEditTestType form = new frmEditTestType(TestTypeID);
            form.ShowDialog();

            _LoadAndRefreshData();
        }

        private void cmsManageTestTypes_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //cmsManageTestTypes
        }
    }
}
