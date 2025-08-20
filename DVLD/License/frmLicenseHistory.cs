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

namespace DVLD.License
{
    public partial class frmLicenseHistory : Form
    {
        int DriverID;

        public frmLicenseHistory(int driverID)
        {
            InitializeComponent();

            DriverID = driverID;
        }

        private void frmLicenseHistory_Load(object sender, EventArgs e)
        {
            int personID = clsDriver.Find(DriverID).PersonID;

            ctrlLicenseHistory1.loadHistoryData(DriverID);
            ctrlPersonInfo1.LoadPersonData(personID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
