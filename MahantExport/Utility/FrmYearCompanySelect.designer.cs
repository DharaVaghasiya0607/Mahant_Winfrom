namespace MahantExport.Utility
{
    partial class FrmYearCompanySelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmYearCompanySelect));
            this.BtnClose = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnLogin = new DevControlLib.cDevSimpleButton(this.components);
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            this.groupControl1 = new DevControlLib.cDevGroupControl(this.components);
            this.cLabel2 = new AxonContLib.cLabel(this.components);
            this.lblFormType = new AxonContLib.cLabel(this.components);
            this.cmbCompany = new AxonContLib.cComboBox(this.components);
            this.cmbFinincialYear = new AxonContLib.cComboBox(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnClose
            // 
            this.BtnClose.Appearance.Font = new System.Drawing.Font("Cambria", 13F, System.Drawing.FontStyle.Bold);
            this.BtnClose.Appearance.Options.UseFont = true;
            this.BtnClose.ImageOptions.Image = global::MahantExport.Properties.Resources.btnexit;
            this.BtnClose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnClose.ImageOptions.SvgImage")));
            this.BtnClose.Location = new System.Drawing.Point(236, 231);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(126, 43);
            this.BtnClose.TabIndex = 7;
            this.BtnClose.Text = "&Close Me";
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // BtnLogin
            // 
            this.BtnLogin.Appearance.Font = new System.Drawing.Font("Cambria", 13F, System.Drawing.FontStyle.Bold);
            this.BtnLogin.Appearance.Options.UseFont = true;
            this.BtnLogin.ImageOptions.Image = global::MahantExport.Properties.Resources.btnsave;
            this.BtnLogin.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnLogin.ImageOptions.SvgImage")));
            this.BtnLogin.Location = new System.Drawing.Point(102, 231);
            this.BtnLogin.Name = "BtnLogin";
            this.BtnLogin.Size = new System.Drawing.Size(128, 43);
            this.BtnLogin.TabIndex = 4;
            this.BtnLogin.Text = "&Enter";
            this.BtnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // cLabel1
            // 
            this.cLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(127)))), ((int)(((byte)(176)))));
            this.cLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.cLabel1.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel1.ForeColor = System.Drawing.Color.White;
            this.cLabel1.Location = new System.Drawing.Point(0, 0);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(534, 42);
            this.cLabel1.TabIndex = 24;
            this.cLabel1.Text = "Select Company And Financial Year";
            this.cLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cLabel1.ToolTips = "";
            // 
            // groupControl1
            // 
            this.groupControl1.Appearance.BackColor = System.Drawing.Color.White;
            this.groupControl1.Appearance.Options.UseBackColor = true;
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Cambria", 12F);
            this.groupControl1.AppearanceCaption.ForeColor = System.Drawing.Color.Black;
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Options.UseForeColor = true;
            this.groupControl1.Controls.Add(this.cLabel2);
            this.groupControl1.Controls.Add(this.lblFormType);
            this.groupControl1.Controls.Add(this.cmbCompany);
            this.groupControl1.Controls.Add(this.cmbFinincialYear);
            this.groupControl1.Controls.Add(this.BtnClose);
            this.groupControl1.Controls.Add(this.BtnLogin);
            this.groupControl1.Location = new System.Drawing.Point(42, 81);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(456, 287);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Select Company And Year";
            // 
            // cLabel2
            // 
            this.cLabel2.AutoSize = true;
            this.cLabel2.BackColor = System.Drawing.Color.Transparent;
            this.cLabel2.Font = new System.Drawing.Font("Cambria", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.cLabel2.ForeColor = System.Drawing.Color.Black;
            this.cLabel2.Location = new System.Drawing.Point(12, 53);
            this.cLabel2.Name = "cLabel2";
            this.cLabel2.Size = new System.Drawing.Size(67, 16);
            this.cLabel2.TabIndex = 0;
            this.cLabel2.Text = "Company";
            this.cLabel2.ToolTips = "";
            // 
            // lblFormType
            // 
            this.lblFormType.AutoSize = true;
            this.lblFormType.BackColor = System.Drawing.Color.Transparent;
            this.lblFormType.Font = new System.Drawing.Font("Cambria", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.lblFormType.ForeColor = System.Drawing.Color.Black;
            this.lblFormType.Location = new System.Drawing.Point(12, 149);
            this.lblFormType.Name = "lblFormType";
            this.lblFormType.Size = new System.Drawing.Size(97, 16);
            this.lblFormType.TabIndex = 2;
            this.lblFormType.Text = "Financial Year";
            this.lblFormType.ToolTips = "";
            // 
            // cmbCompany
            // 
            this.cmbCompany.AllowTabKeyOnEnter = false;
            this.cmbCompany.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCompany.Font = new System.Drawing.Font("Cambria", 13F);
            this.cmbCompany.ForeColor = System.Drawing.Color.Black;
            this.cmbCompany.FormattingEnabled = true;
            this.cmbCompany.Location = new System.Drawing.Point(15, 83);
            this.cmbCompany.Name = "cmbCompany";
            this.cmbCompany.Size = new System.Drawing.Size(415, 28);
            this.cmbCompany.TabIndex = 1;
            this.cmbCompany.ToolTips = "";
            // 
            // cmbFinincialYear
            // 
            this.cmbFinincialYear.AllowTabKeyOnEnter = false;
            this.cmbFinincialYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFinincialYear.Font = new System.Drawing.Font("Cambria", 13F);
            this.cmbFinincialYear.ForeColor = System.Drawing.Color.Black;
            this.cmbFinincialYear.FormattingEnabled = true;
            this.cmbFinincialYear.Location = new System.Drawing.Point(15, 173);
            this.cmbFinincialYear.Name = "cmbFinincialYear";
            this.cmbFinincialYear.Size = new System.Drawing.Size(415, 28);
            this.cmbFinincialYear.TabIndex = 3;
            this.cmbFinincialYear.ToolTips = "";
            // 
            // FrmYearCompanySelect
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 411);
            this.Controls.Add(this.cLabel1);
            this.Controls.Add(this.groupControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("FrmYearCompanySelect.IconOptions.Icon")));
            this.Name = "FrmYearCompanySelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AxonContLib.cLabel cLabel1;
        private DevControlLib.cDevSimpleButton BtnClose;
        private DevControlLib.cDevSimpleButton BtnLogin;
        private DevControlLib.cDevGroupControl groupControl1;
        private AxonContLib.cComboBox cmbFinincialYear;
        private AxonContLib.cLabel lblFormType;
        private AxonContLib.cLabel cLabel2;
        private AxonContLib.cComboBox cmbCompany;
    }
}