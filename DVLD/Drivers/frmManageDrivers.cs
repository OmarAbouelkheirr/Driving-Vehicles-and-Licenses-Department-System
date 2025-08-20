using System;
using System.Data;
using System.Windows.Forms;
using BusinessLayer;

namespace DVLD.Drivers
{
    public partial class frmManageDrivers : Form
    {
        public frmManageDrivers()
        {
            InitializeComponent();
        }

        void _LoadAndRefreshData()
        {
            _LoadPeople();
            _CountOfRecourds();

            cbFilterBy.SelectedIndex = 0;
            txtFilterBy.Visible = false;
        }

        void _CountOfRecourds()
        {
            lblCountOFUsers.Text = dgvDrivers.RowCount.ToString();
        }

        void _LoadPeople()
        {
            dgvDrivers.DataSource = clsDriver.GetAllDrivers();
        }

        private void frmManageDrivers_Load(object sender, EventArgs e)
        {
            _LoadAndRefreshData();
        }

        void _PrepareToFiltering()
        {
            txtFilterBy.Visible = true;
            txtFilterBy.Clear();
            txtFilterBy.Focus();
        }

        DataTable _licenseTable;
        string _CurrentFilterColumn;

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SelectedItem = cbFilterBy.SelectedItem.ToString();
            _licenseTable = clsDriver.GetAllDrivers();

            switch (SelectedItem)
            {
                case "None":
                    txtFilterBy.Visible = false;
                    break;
                case "National No.":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "NationalNo";
                    break;
                case "Driver ID":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "DriverID";
                    break;
                case "Person ID":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "PersonID";
                    break;
                case "Full Name":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "FullName";
                    break;
            }
        }

        void _ApplyLiveSearch(string searchText)
        {
            if (_licenseTable == null) return;

            searchText = searchText.Trim().Replace("'", "''");

            DataView dv = _licenseTable.DefaultView;

            if (string.IsNullOrEmpty(_CurrentFilterColumn) || string.IsNullOrEmpty(searchText))
            {
                dv.RowFilter = "";
            }
            else
            {
                dv.RowFilter = $"Convert({_CurrentFilterColumn}, 'System.String') LIKE '%{searchText}%'";
            }

            dgvDrivers.DataSource = dv;
            _CountOfRecourds();
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            _ApplyLiveSearch(txtFilterBy.Text);
        }
    }
}