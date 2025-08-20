using System;
using System.Data;
using System.Windows.Forms;
using BusinessLayer;
using DVLD.People;

namespace DVLD
{
    public partial class frmPeople : Form
    {
        public frmPeople()
        {
            InitializeComponent();
        }

        void _LoadAndRefreshData()
        {
            _LoadPeople();
            _CountOfRecourds();

            cbFilter.SelectedIndex = 0;

        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            Form addUpdatePerson = new frmAddUpdatePerson(-1);
            addUpdatePerson.ShowDialog();
            _LoadAndRefreshData();
        }

        void _LoadPeople()
        {
            dgvPeopleList.DataSource = clsPerson.GetAllPeople();
        }

        void _CountOfRecourds()
        {
            lblCountOfRecords.Text = dgvPeopleList.RowCount.ToString();
        }

        private void frmPeople_Load(object sender, EventArgs e)
        {
            _LoadAndRefreshData();
        }

        private void EditPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson form = new frmAddUpdatePerson((int)dgvPeopleList.CurrentRow.Cells[0].Value);
            form.ShowDialog();

            _LoadAndRefreshData();
        }

        private void AddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson form = new frmAddUpdatePerson(-1);
            form.ShowDialog();

            _LoadAndRefreshData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DeletePerson_Click(object sender, EventArgs e)
        {
            int PersonIDFromRow = (int)dgvPeopleList.CurrentRow.Cells[0].Value;

            if (MessageBox.Show($"Are you sure you want to delete this person {PersonIDFromRow} ?", "Delete Person", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                if (clsPerson.DeletePerson(PersonIDFromRow))
                {
                    MessageBox.Show($"Person with ID {PersonIDFromRow} was deleted successfully!", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _LoadAndRefreshData();
                }
                else
                {
                    MessageBox.Show("An error occurred while deleting the person.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ShowPersonDetails_Click(object sender, EventArgs e)
        {
            frmPersonDedails frm = new frmPersonDedails((int)dgvPeopleList.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            _LoadAndRefreshData();
        }

        void _PrepareToFiltering()
        {
            txtFilter.Visible = true;
            txtFilter.Clear();
            txtFilter.Focus();
        }
        
        
        DataTable _peopleTable;
        string _CurrentFilterColumn;


        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SelectedItem = cbFilter.SelectedItem.ToString();

            _peopleTable = clsPerson.GetAllPeople();

            switch (SelectedItem)
            {
                case "None":
                    txtFilter.Visible = false;
                    break;
                case "Person ID":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "PersonID";
                    break;
                case "Naional No.":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "NationalNo";
                    break;
                case "First Name":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "FirstName";
                    break;
                case "Second Name":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "SecondName";
                    break;
                case "Third Name":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "ThirdName";
                    break;
                case "Last Name":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "LastName";
                    break;
                case "Nationality":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "NationalityCountryID";
                    break;
                case "Gendor":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "Gendor";
                    break; 
                case "Phone":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "Phone";
                    break;
                case "Email":
                    _PrepareToFiltering();
                    _CurrentFilterColumn = "Email";
                    break;
            }
        }

        // ChatGPT Codeeeeeeeee
        void _ApplyLiveSearch(string searchText)
        {
            if (_peopleTable == null) return;

            searchText = searchText.Trim().Replace("'", "''");

            DataView dv = _peopleTable.DefaultView;

            if (string.IsNullOrEmpty(_CurrentFilterColumn) || string.IsNullOrEmpty(searchText))
            {
                dv.RowFilter = "";
            }
            else
            {
                dv.RowFilter = $"Convert({_CurrentFilterColumn}, 'System.String') LIKE '%{searchText}%'";
            }

            dgvPeopleList.DataSource = dv;
        }


        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            _ApplyLiveSearch(txtFilter.Text);
        }
    }
}
