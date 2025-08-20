using System;
using System.Data;
using System.Security.Policy;
using System.Windows.Forms;
using System.Xml.Linq;
using BusinessLayer;

namespace DVLD.Users
{
    public partial class frmUsers : Form
    {
        public frmUsers()
        {
            InitializeComponent();
        }

        void _LoadAndRefreshData()
        {
            _LoadPeople();
            _CountOfRecourds();

            cbFilterBy.SelectedIndex = 0;
            //txtFilterBy.Enabled = false;
            txtFilterBy.Visible = false;

        }

        void _CountOfRecourds()
        {
            lblCountOFUsers.Text = dgvUsers.RowCount.ToString();
        }

        void _LoadPeople()
        {
            dgvUsers.DataSource = clsUser.GetAllUsers();
        }

        private void frmUsers_Load(object sender, EventArgs e)
        {
            _LoadAndRefreshData();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddUser frmAdd = new frmAddUser();
            frmAdd.ShowDialog();

            _LoadAndRefreshData();
        }

        void _PrepareToFiltering()
        {
            txtFilterBy.Visible = true;
            cbActivation.Visible = false;
            txtFilterBy.Clear();
            txtFilterBy.Focus();
        }

        void _PrepareToFilteringByIsActive()
        {
            cbActivation.SelectedItem = "All";
            cbActivation.Visible = true;
            txtFilterBy.Visible = false;
        }

        DataTable _userTable;
        string _CurrentFilterColumn;

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SelectedItem = cbFilterBy.SelectedItem.ToString();
            _userTable = clsUser.GetAllUsers();

            switch (SelectedItem)
            {
                case "None":
                    txtFilterBy.Visible = false;
                    cbActivation.Visible = false;
                    break;
                case "User ID":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "UserID";
                    break;
                case "User Name":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "UserName";
                    break;
                case "Person ID":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "PersonID";
                    break;
                case "Is Active":
                    _PrepareToFilteringByIsActive();
                    break;
            }
        }

        void _ApplyLiveSearch(string searchText)
        {
            if (_userTable == null) return;

            searchText = searchText.Trim().Replace("'", "''");

            DataView dv = _userTable.DefaultView;

            if (string.IsNullOrEmpty(_CurrentFilterColumn) || string.IsNullOrEmpty(searchText))
            {
                dv.RowFilter = "";
            }
            else
            {
                dv.RowFilter = $"Convert({_CurrentFilterColumn}, 'System.String') LIKE '%{searchText}%'";
            }

            dgvUsers.DataSource = dv;
        }

        private void txtFilterBy_TextChanged(object sender, EventArgs e)
        {
            _ApplyLiveSearch(txtFilterBy.Text);
        }

        private void cbActivation_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SelectedItem = cbActivation.SelectedItem.ToString();
            _userTable = clsUser.GetAllUsers();

            _CurrentFilterColumn = "IsActive";

            switch (SelectedItem)
            {
                case "All":
                    _LoadPeople();
                    _CountOfRecourds();
                    break;
                case "Yes":
                    _ApplyLiveSearch("true");
                    _CountOfRecourds();
                    break;
                case "No":
                    _ApplyLiveSearch("false");
                    _CountOfRecourds();
                    break;
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserIDFromRow = (int)dgvUsers.CurrentRow.Cells[0].Value;

            if (MessageBox.Show($"Are you sure you want to delete this user {UserIDFromRow} ?", "Delete User", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (clsUser.Delete(UserIDFromRow))
                {
                    MessageBox.Show($"User with ID {UserIDFromRow} was deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _LoadAndRefreshData();
                }
                else
                {
                    MessageBox.Show("An error occurred while deleting the user.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserIDFromRow = (int)dgvUsers.CurrentRow.Cells[0].Value;

            frmUserInfo form = new frmUserInfo(UserIDFromRow);
            form.ShowDialog();
        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUser frmAddUser = new frmAddUser();
            frmAddUser.ShowDialog();

            _LoadAndRefreshData();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserIDFromRow = (int)dgvUsers.CurrentRow.Cells[0].Value;

            frmChangePassword form = new frmChangePassword(UserIDFromRow);
            form.ShowDialog();
        }

        private void dgvUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}