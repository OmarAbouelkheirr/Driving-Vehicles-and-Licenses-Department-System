using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusinessLayer;

namespace DVLD.Controlrs
{
    public partial class ctrlLicenseHistory : UserControl
    {
        public ctrlLicenseHistory()
        {
            InitializeComponent();
        }

        void loadLocalHistory(int driverID)
        {
            dgvLocal.DataSource = clsLicense.GetLicenseHistory(driverID);
            lblCount.Text = dgvLocal.RowCount.ToString();
        }
        void loadInternationalHistory(int driverID)
        {
            dgvInternational.DataSource = clsInternationalLicense.GetLicenseHistory(driverID);
            lblCounnt.Text = dgvInternational.RowCount.ToString();
        }

        public void loadHistoryData(int DriverID)
        {
            loadLocalHistory(DriverID);
            loadInternationalHistory(DriverID);
        }

    }
}
