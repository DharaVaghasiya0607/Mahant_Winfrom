namespace MahantExport.Utility
{
    partial class FrmImageCertiFlagUpdate
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmImageCertiFlagUpdate));
            this.BtnUpdate = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnupdateWithUrl = new DevControlLib.cDevSimpleButton(this.components);
            this.SuspendLayout();
            // 
            // BtnUpdate
            // 
            this.BtnUpdate.Appearance.Font = new System.Drawing.Font("Cambria", 13F, System.Drawing.FontStyle.Bold);
            this.BtnUpdate.Appearance.Options.UseFont = true;
            this.BtnUpdate.ImageOptions.Image = global::MahantExport.Properties.Resources.btnsave;
            this.BtnUpdate.Location = new System.Drawing.Point(26, 36);
            this.BtnUpdate.Name = "BtnUpdate";
            this.BtnUpdate.Size = new System.Drawing.Size(168, 44);
            this.BtnUpdate.TabIndex = 3;
            this.BtnUpdate.Text = "Update";
            this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // BtnupdateWithUrl
            // 
            this.BtnupdateWithUrl.Appearance.Font = new System.Drawing.Font("Cambria", 13F, System.Drawing.FontStyle.Bold);
            this.BtnupdateWithUrl.Appearance.Options.UseFont = true;
            this.BtnupdateWithUrl.ImageOptions.Image = global::MahantExport.Properties.Resources.btnsave;
            this.BtnupdateWithUrl.Location = new System.Drawing.Point(219, 36);
            this.BtnupdateWithUrl.Name = "BtnupdateWithUrl";
            this.BtnupdateWithUrl.Size = new System.Drawing.Size(168, 44);
            this.BtnupdateWithUrl.TabIndex = 4;
            this.BtnupdateWithUrl.Text = "Update With Url";
            this.BtnupdateWithUrl.Click += new System.EventHandler(this.BtnupdateWithUrl_Click);
            // 
            // FrmImageCertiFlagUpdate
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 245);
            this.Controls.Add(this.BtnupdateWithUrl);
            this.Controls.Add(this.BtnUpdate);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("FrmImageCertiFlagUpdate.IconOptions.Icon")));
            this.LookAndFeel.SkinName = "London Liquid Sky";
            this.LookAndFeel.UseDefaultLookAndFeel =true;
            this.Name = "FrmImageCertiFlagUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IMAGE CERTI UPDATE";
            this.ResumeLayout(false);

        }

        #endregion
        private DevControlLib.cDevSimpleButton BtnUpdate;
        private DevControlLib.cDevSimpleButton BtnupdateWithUrl;
    }
}