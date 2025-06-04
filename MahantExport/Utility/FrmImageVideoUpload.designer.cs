namespace MahantExport
{
    partial class FrmImageVideoUpload
    {
        /*
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl3 = new DevExpress.XtraEditors.PanelControl();
            this.panelControl2 = new DevExpress.XtraEditors.PanelControl();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.BTN_AWS_UPLOAD = new DevExpress.XtraEditors.SimpleButton();
            this.BTN_AUTO_UPLOAD = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl4 = new DevExpress.XtraEditors.GroupControl();
            this.BTN_GET = new DevExpress.XtraEditors.SimpleButton();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 82;
            this.dataGridView1.Size = new System.Drawing.Size(2620, 711);
            this.dataGridView1.TabIndex = 0;
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.panelControl3);
            this.panelControl1.Controls.Add(this.panelControl2);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(6);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(2632, 848);
            this.panelControl1.TabIndex = 1;
            // 
            // panelControl3
            // 
            this.panelControl3.Controls.Add(this.dataGridView1);
            this.panelControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl3.Location = new System.Drawing.Point(3, 128);
            this.panelControl3.Margin = new System.Windows.Forms.Padding(6);
            this.panelControl3.Name = "panelControl3";
            this.panelControl3.Size = new System.Drawing.Size(2626, 717);
            this.panelControl3.TabIndex = 2;
            // 
            // panelControl2
            // 
            this.panelControl2.Controls.Add(this.groupControl1);
            this.panelControl2.Controls.Add(this.groupControl4);
            this.panelControl2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControl2.Location = new System.Drawing.Point(3, 3);
            this.panelControl2.Margin = new System.Windows.Forms.Padding(6);
            this.panelControl2.Name = "panelControl2";
            this.panelControl2.Size = new System.Drawing.Size(2626, 125);
            this.panelControl2.TabIndex = 1;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.BTN_AWS_UPLOAD);
            this.groupControl1.Controls.Add(this.BTN_AUTO_UPLOAD);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(227, 3);
            this.groupControl1.Margin = new System.Windows.Forms.Padding(6);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(2396, 119);
            this.groupControl1.TabIndex = 8;
            // 
            // BTN_AWS_UPLOAD
            // 
            this.BTN_AWS_UPLOAD.Appearance.Font = new System.Drawing.Font("Tahoma", 10.25F, System.Drawing.FontStyle.Bold);
            this.BTN_AWS_UPLOAD.Appearance.Options.UseFont = true;
            this.BTN_AWS_UPLOAD.Location = new System.Drawing.Point(350, 42);
            this.BTN_AWS_UPLOAD.Margin = new System.Windows.Forms.Padding(6);
            this.BTN_AWS_UPLOAD.Name = "BTN_AWS_UPLOAD";
            this.BTN_AWS_UPLOAD.Size = new System.Drawing.Size(326, 65);
            this.BTN_AWS_UPLOAD.TabIndex = 6;
            this.BTN_AWS_UPLOAD.Text = "GET HPHT";
            this.BTN_AWS_UPLOAD.Click += new System.EventHandler(this.BTN_AWS_UPLOAD_Click);
            // 
            // BTN_AUTO_UPLOAD
            // 
            this.BTN_AUTO_UPLOAD.Appearance.Font = new System.Drawing.Font("Tahoma", 10.25F, System.Drawing.FontStyle.Bold);
            this.BTN_AUTO_UPLOAD.Appearance.Options.UseFont = true;
            this.BTN_AUTO_UPLOAD.Location = new System.Drawing.Point(12, 44);
            this.BTN_AUTO_UPLOAD.Margin = new System.Windows.Forms.Padding(6);
            this.BTN_AUTO_UPLOAD.Name = "BTN_AUTO_UPLOAD";
            this.BTN_AUTO_UPLOAD.Size = new System.Drawing.Size(326, 65);
            this.BTN_AUTO_UPLOAD.TabIndex = 5;
            this.BTN_AUTO_UPLOAD.Text = "AUTO UPLOAD";
            this.BTN_AUTO_UPLOAD.Click += new System.EventHandler(this.BTN_AUTO_UPLOAD_Click);
            // 
            // groupControl4
            // 
            this.groupControl4.Controls.Add(this.BTN_GET);
            this.groupControl4.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupControl4.Location = new System.Drawing.Point(3, 3);
            this.groupControl4.Margin = new System.Windows.Forms.Padding(6);
            this.groupControl4.Name = "groupControl4";
            this.groupControl4.Size = new System.Drawing.Size(224, 119);
            this.groupControl4.TabIndex = 7;
            this.groupControl4.Text = "IMPORT - UPLOAD";
            // 
            // BTN_GET
            // 
            this.BTN_GET.Appearance.Font = new System.Drawing.Font("Tahoma", 10.25F, System.Drawing.FontStyle.Bold);
            this.BTN_GET.Appearance.Options.UseFont = true;
            this.BTN_GET.Location = new System.Drawing.Point(10, 44);
            this.BTN_GET.Margin = new System.Windows.Forms.Padding(6);
            this.BTN_GET.Name = "BTN_GET";
            this.BTN_GET.Size = new System.Drawing.Size(202, 65);
            this.BTN_GET.TabIndex = 4;
            this.BTN_GET.Text = "GET";
            this.BTN_GET.Click += new System.EventHandler(this.BTN_GET_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            // 
            // FRM_UPLOAD_VIC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2632, 848);
            this.Controls.Add(this.panelControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "FRM_UPLOAD_VIC";
            this.Text = "FRM_UPLOAD_VIDEO";
            this.Load += new System.EventHandler(this.FRM_UPLOAD_VIC_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView1;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.PanelControl panelControl2;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.GroupControl groupControl4;
        private DevExpress.XtraEditors.SimpleButton BTN_GET;
        private DevExpress.XtraEditors.PanelControl panelControl3;
        private DevExpress.XtraEditors.SimpleButton BTN_AUTO_UPLOAD;
        private DevExpress.XtraEditors.SimpleButton BTN_AWS_UPLOAD;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
         */
    }

}