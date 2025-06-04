namespace MahantExport.UserControl
{
    partial class CustomNotifiation
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomNotifiation));
            this.webService1 = new MahantExport.DocumentDownload.WebService();
            this.lblMessage = new System.Windows.Forms.Label();
            this.panel1 = new AxonContLib.cPanel();
            this.BtnClose = new System.Windows.Forms.Button();
            this.panel2 = new AxonContLib.cPanel();
            this.BtnConfirm = new System.Windows.Forms.Button();
            this.panel3 = new AxonContLib.cPanel();
            this.BtnIcon = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // webService1
            // 
            this.webService1.Credentials = null;
            this.webService1.Url = "http://192.168.0.250/SKESalesService/webservice.asmx";
            this.webService1.UseDefaultCredentials = false;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.lblMessage.ForeColor = System.Drawing.Color.White;
            this.lblMessage.Location = new System.Drawing.Point(94, 14);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(116, 21);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Message Text";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Highlight;
            this.panel1.Controls.Add(this.BtnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(521, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(60, 100);
            this.panel1.TabIndex = 1;
            // 
            // BtnClose
            // 
            this.BtnClose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnClose.BackgroundImage")));
            this.BtnClose.FlatAppearance.BorderSize = 0;
            this.BtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnClose.ForeColor = System.Drawing.Color.White;
            this.BtnClose.Location = new System.Drawing.Point(6, 28);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(47, 46);
            this.BtnClose.TabIndex = 0;
            this.BtnClose.UseVisualStyleBackColor = true;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Highlight;
            this.panel2.Controls.Add(this.BtnConfirm);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(461, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(60, 100);
            this.panel2.TabIndex = 2;
            // 
            // BtnConfirm
            // 
            this.BtnConfirm.BackgroundImage = global::MahantExport.Properties.Resources.Confirm;
            this.BtnConfirm.FlatAppearance.BorderSize = 0;
            this.BtnConfirm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnConfirm.Location = new System.Drawing.Point(6, 26);
            this.BtnConfirm.Name = "BtnConfirm";
            this.BtnConfirm.Size = new System.Drawing.Size(47, 46);
            this.BtnConfirm.TabIndex = 0;
            this.BtnConfirm.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel3.Controls.Add(this.BtnIcon);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(64, 100);
            this.panel3.TabIndex = 3;
            // 
            // BtnIcon
            // 
            this.BtnIcon.BackColor = System.Drawing.SystemColors.Highlight;
            this.BtnIcon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BtnIcon.FlatAppearance.BorderSize = 0;
            this.BtnIcon.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnIcon.Location = new System.Drawing.Point(0, 0);
            this.BtnIcon.Name = "BtnIcon";
            this.BtnIcon.Size = new System.Drawing.Size(64, 100);
            this.BtnIcon.TabIndex = 0;
            this.BtnIcon.UseVisualStyleBackColor = false;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // CustomNotifiation
            // 
            this.Appearance.BackColor = System.Drawing.SystemColors.Highlight;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblMessage);
            this.Name = "CustomNotifiation";
            this.Size = new System.Drawing.Size(581, 100);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DocumentDownload.WebService webService1;
        private System.Windows.Forms.Label lblMessage;
        private AxonContLib.cPanel panel1;
        private System.Windows.Forms.Button BtnClose;
        private AxonContLib.cPanel panel2;
        private System.Windows.Forms.Button BtnConfirm;
        private AxonContLib.cPanel panel3;
        private System.Windows.Forms.Button BtnIcon;
        private System.Windows.Forms.Timer timer1;




    }
}
