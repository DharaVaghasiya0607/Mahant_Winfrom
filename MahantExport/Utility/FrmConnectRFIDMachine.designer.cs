namespace MahantExport.Utility
{
    partial class FrmConnectRFIDMachine
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
            this.PanelProgress = new DevExpress.XtraWaitForm.ProgressPanel();
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // PanelProgress
            // 
            this.PanelProgress.AnimationToTextDistance = 10;
            this.PanelProgress.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.PanelProgress.Appearance.Options.UseBackColor = true;
            this.PanelProgress.AppearanceCaption.Font = new System.Drawing.Font("Verdana", 12F);
            this.PanelProgress.AppearanceCaption.Options.UseFont = true;
            this.PanelProgress.AppearanceDescription.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.PanelProgress.AppearanceDescription.Options.UseFont = true;
            this.PanelProgress.Location = new System.Drawing.Point(12, 10);
            this.PanelProgress.LookAndFeel.UseDefaultLookAndFeel = false;
            this.PanelProgress.Name = "PanelProgress";
            this.PanelProgress.ShowCaption = false;
            this.PanelProgress.ShowDescription = false;
            this.PanelProgress.Size = new System.Drawing.Size(37, 41);
            this.PanelProgress.TabIndex = 9;
            this.PanelProgress.Text = "progressPanel1";
            // 
            // cLabel1
            // 
            this.cLabel1.AutoSize = true;
            this.cLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel1.ForeColor = System.Drawing.Color.Navy;
            this.cLabel1.Location = new System.Drawing.Point(56, 24);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(207, 13);
            this.cLabel1.TabIndex = 10;
            this.cLabel1.Text = "Connecting To RFID Machine....";
            this.cLabel1.ToolTips = "";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // FrmConnectRFIDMachine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 63);
            this.Controls.Add(this.cLabel1);
            this.Controls.Add(this.PanelProgress);
            this.IconOptions.ShowIcon = false;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(288, 102);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(274, 95);
            this.Name = "FrmConnectRFIDMachine";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Tag = "ConnectRFIDMachine";
            this.Text = "RFID Machine";
            this.Load += new System.EventHandler(this.FrmConnectRFIDMachine_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraWaitForm.ProgressPanel PanelProgress;
        private AxonContLib.cLabel cLabel1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;



    }
}