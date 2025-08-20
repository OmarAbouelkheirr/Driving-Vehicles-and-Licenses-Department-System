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

namespace DVLD.Application_Types
{
    public partial class frmManageApplicationTypes : Form
    {
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }

        void _CountOfRecourds()
        {
            lblCount.Text = dgvApplicationTypes.RowCount.ToString();
        }

        void _LoadAndRefreshData()
        {
            _LoadAppsTypes();
            _CountOfRecourds();
        }

        void _LoadAppsTypes()
        {
            dgvApplicationTypes.DataSource = clsApplicationType.GetApplicationTypes();
        }

        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            _LoadAndRefreshData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int AppTypeID = (int)dgvApplicationTypes.CurrentRow.Cells[0].Value;

            frmEditApplicationType form = new frmEditApplicationType(AppTypeID);
            form.ShowDialog();

            _LoadAndRefreshData();
        }
    }
}
