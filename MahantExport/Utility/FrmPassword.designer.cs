namespace MahantExport.Utility
{
    partial class FrmPassword
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPassword));
            this.BtnOk = new DevControlLib.cDevSimpleButton();
            this.groupControl1 = new DevControlLib.cDevGroupControl();
            this.BtnClose = new DevControlLib.cDevSimpleButton();
            this.txtPassword = new AxonContLib.cTextBox(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnOk
            // 
            this.BtnOk.Appearance.Font = new System.Drawing.Font("Cambria", 13F, System.Drawing.FontStyle.Bold);
            this.BtnOk.Appearance.Options.UseFont = true;
            this.BtnOk.Location = new System.Drawing.Point(12, 79);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(88, 32);
            this.BtnOk.TabIndex = 3;
            this.BtnOk.Text = "Ok";
            this.BtnOk.Click += new System.EventHandler(this.BtnLogin_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Appearance.BackColor = System.Drawing.Color.White;
            this.groupControl1.Appearance.Options.UseBackColor = true;
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Cambria", 12F);
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.BtnClose);
            this.groupControl1.Controls.Add(this.BtnOk);
            this.groupControl1.Controls.Add(this.txtPassword);
            this.groupControl1.Location = new System.Drawing.Point(12, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(212, 120);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Enter Password";
            // 
            // BtnClose
            // 
            this.BtnClose.Appearance.Font = new System.Drawing.Font("Cambria", 13F, System.Drawing.FontStyle.Bold);
            this.BtnClose.Appearance.Options.UseFont = true;
            this.BtnClose.Location = new System.Drawing.Point(111, 79);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(88, 32);
            this.BtnClose.TabIndex = 4;
            this.BtnClose.Text = "Close";
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.ActivationColor = true;
            this.txtPassword.AllowTabKeyOnEnter = false;
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtPassword.Font = new System.Drawing.Font("Cambria", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Format = "";
            this.txtPassword.IsComplusory = false;
            this.txtPassword.Location = new System.Drawing.Point(11, 36);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.SelectAllTextOnFocus = true;
            this.txtPassword.Size = new System.Drawing.Size(188, 31);
            this.txtPassword.TabIndex = 0;
            this.txtPassword.Tag = "JV123";
            this.txtPassword.ToolTips = "";
            this.txtPassword.WaterMarkText = null;
            // 
            // FrmPassword
            // 
            this.Appearance.BackColor = System.Drawing.Color.LightGray;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 140);
            this.Controls.Add(this.groupControl1);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.LookAndFeel.SkinName = "Caramel";
            this.LookAndFeel.UseDefaultLookAndFeel =true;
            this.Name = "FrmPassword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PASSWORD";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmPassword_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AxonContLib.cTextBox txtPassword;
        private DevControlLib.cDevSimpleButton BtnOk;
        private DevControlLib.cDevGroupControl groupControl1;
        private DevControlLib.cDevSimpleButton BtnClose;

    }
}