namespace MahantExport
{
    partial class FrmImageVideoCertiUrlUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmImageVideoCertiUrlUpdate));
            this.TimerCertificate = new System.Windows.Forms.Timer(this.components);
            this.Btn_CertDownload = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.progressPanel1 = new DevExpress.XtraWaitForm.ProgressPanel();
            this.panel2 = new AxonContLib.cPanel(this.components);
            this.lblMessage = new AxonContLib.cLabel(this.components);
            this.label1 = new AxonContLib.cLabel(this.components);
            this.ChkForceFullyUpdate = new AxonContLib.cCheckBox(this.components);
            this.panel1 = new AxonContLib.cPanel(this.components);
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // TimerCertificate
            // 
            this.TimerCertificate.Enabled = true;
            this.TimerCertificate.Interval = 30000;
            this.TimerCertificate.Tag = "0.3 Minute";
            this.TimerCertificate.Tick += new System.EventHandler(this.TimerCertificate_Tick);
            // 
            // Btn_CertDownload
            // 
            this.Btn_CertDownload.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.Btn_CertDownload.Location = new System.Drawing.Point(3, 46);
            this.Btn_CertDownload.Name = "Btn_CertDownload";
            this.Btn_CertDownload.Size = new System.Drawing.Size(235, 61);
            this.Btn_CertDownload.TabIndex = 12;
            this.Btn_CertDownload.Text = "MANUAL CALL";
            this.Btn_CertDownload.UseVisualStyleBackColor = true;
            this.Btn_CertDownload.Click += new System.EventHandler(this.Btn_CertDownload_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // progressPanel1
            // 
            this.progressPanel1.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.progressPanel1.Appearance.Options.UseBackColor = true;
            this.progressPanel1.AppearanceCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.progressPanel1.AppearanceCaption.Options.UseFont = true;
            this.progressPanel1.AppearanceDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.progressPanel1.AppearanceDescription.Options.UseFont = true;
            this.progressPanel1.Location = new System.Drawing.Point(246, 46);
            this.progressPanel1.LookAndFeel.SkinName = "Office 2013";
            this.progressPanel1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.progressPanel1.Name = "progressPanel1";
            this.progressPanel1.Size = new System.Drawing.Size(174, 61);
            this.progressPanel1.TabIndex = 15;
            this.progressPanel1.Text = "progressPanel1";
            this.progressPanel1.Visible = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Gray;
            this.panel2.Controls.Add(this.lblMessage);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.ForeColor = System.Drawing.Color.White;
            this.panel2.Location = new System.Drawing.Point(0, 116);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(424, 37);
            this.panel2.TabIndex = 16;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Verdana", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.Navy;
            this.lblMessage.Location = new System.Drawing.Point(6, 11);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(64, 14);
            this.lblMessage.TabIndex = 13;
            this.lblMessage.Text = "Message";
            this.lblMessage.ToolTips = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Cambria", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Teal;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(235, 23);
            this.label1.TabIndex = 17;
            this.label1.Text = "Image | Video | Certificate ";
            this.label1.ToolTips = "";
            // 
            // ChkForceFullyUpdate
            // 
            this.ChkForceFullyUpdate.AllowTabKeyOnEnter = false;
            this.ChkForceFullyUpdate.AutoSize = true;
            this.ChkForceFullyUpdate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkForceFullyUpdate.Location = new System.Drawing.Point(278, 7);
            this.ChkForceFullyUpdate.Name = "ChkForceFullyUpdate";
            this.ChkForceFullyUpdate.Size = new System.Drawing.Size(131, 17);
            this.ChkForceFullyUpdate.TabIndex = 18;
            this.ChkForceFullyUpdate.Text = "Force Fully Update";
            this.ChkForceFullyUpdate.ToolTips = "";
            this.ChkForceFullyUpdate.UseVisualStyleBackColor = true;
            this.ChkForceFullyUpdate.CheckedChanged += new System.EventHandler(this.ChkForceFullyUpdate_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Btn_CertDownload);
            this.panel1.Controls.Add(this.ChkForceFullyUpdate);
            this.panel1.Controls.Add(this.progressPanel1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(424, 116);
            this.panel1.TabIndex = 19;
            // 
            // FrmImageVideoCertiUrlUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 269);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("FrmImageVideoCertiUrlUpdate.IconOptions.Icon")));
            this.Name = "FrmImageVideoCertiUrlUpdate";
            this.Tag = "ImageCertiFlagUpdate";
            this.Text = "IMAGE VIDEO CERTI FLAG UPDATE";
            this.Load += new System.EventHandler(this.FrmImageVideoCertiUrlUpdate_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer TimerCertificate;
        private System.Windows.Forms.Button Btn_CertDownload;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private DevExpress.XtraWaitForm.ProgressPanel progressPanel1;
        private AxonContLib.cPanel panel2;
        private AxonContLib.cLabel lblMessage;
        private AxonContLib.cLabel label1;
        private AxonContLib.cCheckBox ChkForceFullyUpdate;
        private AxonContLib.cPanel panel1;
    }
}

