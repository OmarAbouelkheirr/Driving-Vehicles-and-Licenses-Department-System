namespace DVLD.Controlrs
{
    partial class ctrlAppsInfo
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ctrlApplicationInfo1 = new DVLD.Controlrs.ctrlApplicationInfo();
            this.ctrlDrivingLicenseAppInfo1 = new DVLD.Controlrs.ctrlDrivingLicenseAppInfo();
            this.SuspendLayout();
            // 
            // ctrlApplicationInfo1
            // 
            this.ctrlApplicationInfo1.Location = new System.Drawing.Point(3, 193);
            this.ctrlApplicationInfo1.Name = "ctrlApplicationInfo1";
            this.ctrlApplicationInfo1.Size = new System.Drawing.Size(735, 333);
            this.ctrlApplicationInfo1.TabIndex = 0;
            // 
            // ctrlDrivingLicenseAppInfo1
            // 
            this.ctrlDrivingLicenseAppInfo1.Location = new System.Drawing.Point(4, 7);
            this.ctrlDrivingLicenseAppInfo1.Name = "ctrlDrivingLicenseAppInfo1";
            this.ctrlDrivingLicenseAppInfo1.Size = new System.Drawing.Size(735, 185);
            this.ctrlDrivingLicenseAppInfo1.TabIndex = 1;
            // 
            // ctrlAppsInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ctrlDrivingLicenseAppInfo1);
            this.Controls.Add(this.ctrlApplicationInfo1);
            this.Name = "ctrlAppsInfo";
            this.Size = new System.Drawing.Size(740, 528);
            this.ResumeLayout(false);

        }

        #endregion

        private ctrlApplicationInfo ctrlApplicationInfo1;
        private ctrlDrivingLicenseAppInfo ctrlDrivingLicenseAppInfo1;
    }
}
