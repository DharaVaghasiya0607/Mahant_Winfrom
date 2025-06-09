namespace MahantExport.Utility
{
    partial class FrmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            this.groupControl1 = new DevControlLib.cDevGroupControl(this.components);
            this.cLabel3 = new AxonContLib.cLabel(this.components);
            this.cLabel2 = new AxonContLib.cLabel(this.components);
            this.lblVersion = new AxonContLib.cLabel(this.components);
            this.BtnClose = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnLogin = new DevControlLib.cDevSimpleButton(this.components);
            this.txtUserName = new AxonContLib.cTextBox(this.components);
            this.txtPassWord = new AxonContLib.cTextBox(this.components);
            this.pictureBox1 = new AxonContLib.cPictureBox(this.components);
            this.panel1 = new AxonContLib.cPanel(this.components);
            this.txtConnectionString = new AxonContLib.cTextBox(this.components);
            this.BtnUpdate = new DevControlLib.cDevSimpleButton(this.components);
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Appearance.BackColor = System.Drawing.Color.White;
            this.groupControl1.Appearance.Options.UseBackColor = true;
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Cambria", 12F);
            this.groupControl1.AppearanceCaption.ForeColor = System.Drawing.Color.Black;
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Options.UseForeColor = true;
            this.groupControl1.Controls.Add(this.cLabel3);
            this.groupControl1.Controls.Add(this.cLabel2);
            this.groupControl1.Controls.Add(this.lblVersion);
            this.groupControl1.Controls.Add(this.BtnClose);
            this.groupControl1.Controls.Add(this.BtnLogin);
            this.groupControl1.Controls.Add(this.txtUserName);
            this.groupControl1.Controls.Add(this.txtPassWord);
            this.groupControl1.Location = new System.Drawing.Point(22, 187);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(394, 228);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Enter Your Username && Password";
            // 
            // cLabel3
            // 
            this.cLabel3.AutoSize = true;
            this.cLabel3.BackColor = System.Drawing.Color.Transparent;
            this.cLabel3.Font = new System.Drawing.Font("Cambria", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.cLabel3.ForeColor = System.Drawing.Color.Black;
            this.cLabel3.Location = new System.Drawing.Point(12, 103);
            this.cLabel3.Name = "cLabel3";
            this.cLabel3.Size = new System.Drawing.Size(69, 16);
            this.cLabel3.TabIndex = 2;
            this.cLabel3.Text = "Password";
            this.cLabel3.ToolTips = "";
            // 
            // cLabel2
            // 
            this.cLabel2.AutoSize = true;
            this.cLabel2.BackColor = System.Drawing.Color.Transparent;
            this.cLabel2.Font = new System.Drawing.Font("Cambria", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.cLabel2.ForeColor = System.Drawing.Color.Black;
            this.cLabel2.Location = new System.Drawing.Point(12, 57);
            this.cLabel2.Name = "cLabel2";
            this.cLabel2.Size = new System.Drawing.Size(71, 16);
            this.cLabel2.TabIndex = 0;
            this.cLabel2.Text = "Username";
            this.cLabel2.ToolTips = "";
            // 
            // lblVersion
            // 
            this.lblVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblVersion.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblVersion.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblVersion.Location = new System.Drawing.Point(2, 209);
            this.lblVersion.Name = "lblVersion";
            this.lblVersion.Size = new System.Drawing.Size(390, 17);
            this.lblVersion.TabIndex = 25;
            this.lblVersion.Text = "1.1.1.8";
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblVersion.ToolTips = "";
            // 
            // BtnClose
            // 
            this.BtnClose.Appearance.Font = new System.Drawing.Font("Cambria", 13F, System.Drawing.FontStyle.Bold);
            this.BtnClose.Appearance.Options.UseFont = true;
            this.BtnClose.ImageOptions.Image = global::MahantExport.Properties.Resources.btnexit;
            this.BtnClose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnClose.ImageOptions.SvgImage")));
            this.BtnClose.Location = new System.Drawing.Point(207, 148);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(169, 43);
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
            this.BtnLogin.Location = new System.Drawing.Point(15, 148);
            this.BtnLogin.Name = "BtnLogin";
            this.BtnLogin.Size = new System.Drawing.Size(171, 43);
            this.BtnLogin.TabIndex = 6;
            this.BtnLogin.Text = "&Login";
            this.BtnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // txtUserName
            // 
            this.txtUserName.ActivationColor = true;
            this.txtUserName.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtUserName.AllowTabKeyOnEnter = false;
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserName.Font = new System.Drawing.Font("Cambria", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUserName.Format = "";
            this.txtUserName.IsComplusory = false;
            this.txtUserName.Location = new System.Drawing.Point(111, 51);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.SelectAllTextOnFocus = true;
            this.txtUserName.Size = new System.Drawing.Size(265, 28);
            this.txtUserName.TabIndex = 1;
            this.txtUserName.ToolTips = "";
            this.txtUserName.WaterMarkText = null;
            // 
            // txtPassWord
            // 
            this.txtPassWord.ActivationColor = true;
            this.txtPassWord.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtPassWord.AllowTabKeyOnEnter = false;
            this.txtPassWord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassWord.Font = new System.Drawing.Font("Cambria", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassWord.Format = "";
            this.txtPassWord.IsComplusory = false;
            this.txtPassWord.Location = new System.Drawing.Point(111, 97);
            this.txtPassWord.Name = "txtPassWord";
            this.txtPassWord.PasswordChar = '*';
            this.txtPassWord.SelectAllTextOnFocus = true;
            this.txtPassWord.Size = new System.Drawing.Size(265, 28);
            this.txtPassWord.TabIndex = 3;
            this.txtPassWord.ToolTips = "";
            this.txtPassWord.WaterMarkText = null;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::MahantExport.Properties.Resources.Logo;
            this.pictureBox1.Location = new System.Drawing.Point(133, 45);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(202, 136);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 28;
            this.pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(127)))), ((int)(((byte)(176)))));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtConnectionString);
            this.panel1.Controls.Add(this.BtnUpdate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 427);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(429, 67);
            this.panel1.TabIndex = 26;
            // 
            // txtConnectionString
            // 
            this.txtConnectionString.ActivationColor = true;
            this.txtConnectionString.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtConnectionString.AllowTabKeyOnEnter = false;
            this.txtConnectionString.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConnectionString.Dock = System.Windows.Forms.DockStyle.Left;
            this.txtConnectionString.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtConnectionString.Format = "";
            this.txtConnectionString.IsComplusory = false;
            this.txtConnectionString.Location = new System.Drawing.Point(0, 0);
            this.txtConnectionString.Multiline = true;
            this.txtConnectionString.Name = "txtConnectionString";
            this.txtConnectionString.SelectAllTextOnFocus = true;
            this.txtConnectionString.Size = new System.Drawing.Size(362, 65);
            this.txtConnectionString.TabIndex = 0;
            this.txtConnectionString.ToolTips = "";
            this.txtConnectionString.Visible = false;
            this.txtConnectionString.WaterMarkText = null;
            // 
            // BtnUpdate
            // 
            this.BtnUpdate.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnUpdate.Appearance.Options.UseFont = true;
            this.BtnUpdate.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnUpdate.Location = new System.Drawing.Point(357, 0);
            this.BtnUpdate.Name = "BtnUpdate";
            this.BtnUpdate.Size = new System.Drawing.Size(70, 65);
            this.BtnUpdate.TabIndex = 3;
            this.BtnUpdate.Text = "Update";
            this.BtnUpdate.Visible = false;
            this.BtnUpdate.Click += new System.EventHandler(this.BtnUpdate_Click);
            // 
            // cLabel1
            // 
            this.cLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(111)))), ((int)(((byte)(127)))), ((int)(((byte)(176)))));
            this.cLabel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.cLabel1.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel1.ForeColor = System.Drawing.Color.White;
            this.cLabel1.Location = new System.Drawing.Point(0, 0);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(429, 42);
            this.cLabel1.TabIndex = 24;
            this.cLabel1.Text = "Welcome To Diamond Sales Program";
            this.cLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cLabel1.ToolTips = "";
            // 
            // FrmLogin
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(429, 494);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cLabel1);
            this.Controls.Add(this.groupControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("FrmLogin.IconOptions.Icon")));
            this.Name = "FrmLogin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.FrmLogin_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmLogin_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AxonContLib.cTextBox txtUserName;
        private AxonContLib.cTextBox txtPassWord;
        private AxonContLib.cLabel cLabel1;
        private DevControlLib.cDevSimpleButton BtnClose;
        private DevControlLib.cDevSimpleButton BtnLogin;
        private AxonContLib.cLabel lblVersion;
        private DevControlLib.cDevGroupControl groupControl1;
        private AxonContLib.cPanel panel1;
        private AxonContLib.cTextBox txtConnectionString;
        private DevControlLib.cDevSimpleButton BtnUpdate;
        private AxonContLib.cLabel cLabel2;
        private AxonContLib.cLabel cLabel3;
        private AxonContLib.cPictureBox pictureBox1;
    }
}