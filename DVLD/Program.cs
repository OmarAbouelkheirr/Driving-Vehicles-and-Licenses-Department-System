using System;
using System.Windows.Forms;
using DVLD.Login;

namespace DVLD
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Main());
            //Application.Run(new Testing());
            Application.Run(new frmLogin());
            //Application.Run(new frmRenewLicense());
            //Application.Run(new frmReplacementLicense());
            //Application.Run(new frmManageDetainedLicenses());
            //Application.Run(new frmReleaseDetainedLicense());
        }
    }
}