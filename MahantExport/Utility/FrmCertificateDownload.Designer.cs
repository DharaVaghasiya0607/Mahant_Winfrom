namespace MahantExport.Utility
{
    partial class FrmCertificateDownload
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCertificateDownload));
            this.txtStoneNo = new System.Windows.Forms.TextBox();
            this.BtnDownload = new DevControlLib.cDevSimpleButton(this.components);
            this.progressPanel1 = new DevExpress.XtraWaitForm.ProgressPanel();
            this.panel1 = new AxonContLib.cPanel(this.components);
            this.groupControl1 = new DevControlLib.cDevGroupControl(this.components);
            this.panel3 = new AxonContLib.cPanel(this.components);
            this.RdoGIA = new AxonContLib.cRadioButton(this.components);
            this.panel2 = new AxonContLib.cPanel(this.components);
            this.MainGrd = new DevExpress.XtraGrid.GridControl();
            this.GrdDet = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).BeginInit();
            this.SuspendLayout();
            // 
            // txtStoneNo
            // 
            this.txtStoneNo.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtStoneNo.Location = new System.Drawing.Point(92, 23);
            this.txtStoneNo.Multiline = true;
            this.txtStoneNo.Name = "txtStoneNo";
            this.txtStoneNo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtStoneNo.Size = new System.Drawing.Size(373, 54);
            this.txtStoneNo.TabIndex = 1;
            this.txtStoneNo.TextChanged += new System.EventHandler(this.txtStoneNo_TextChanged);
            this.txtStoneNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtStoneNo_KeyDown);
            // 
            // BtnDownload
            // 
            this.BtnDownload.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.BtnDownload.Appearance.Options.UseFont = true;
            this.BtnDownload.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnDownload.ImageOptions.SvgImage")));
            this.BtnDownload.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnDownload.Location = new System.Drawing.Point(471, 29);
            this.BtnDownload.Name = "BtnDownload";
            this.BtnDownload.Size = new System.Drawing.Size(118, 39);
            this.BtnDownload.TabIndex = 2;
            this.BtnDownload.Text = "DOWNLOAD";
            this.BtnDownload.Click += new System.EventHandler(this.BtnDownload_Click);
            // 
            // progressPanel1
            // 
            this.progressPanel1.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.progressPanel1.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.progressPanel1.Appearance.Options.UseBackColor = true;
            this.progressPanel1.Appearance.Options.UseFont = true;
            this.progressPanel1.AppearanceCaption.Font = new System.Drawing.Font("Verdana", 12F);
            this.progressPanel1.AppearanceCaption.Options.UseFont = true;
            this.progressPanel1.AppearanceDescription.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.progressPanel1.AppearanceDescription.Options.UseFont = true;
            this.progressPanel1.Location = new System.Drawing.Point(595, 24);
            this.progressPanel1.LookAndFeel.SkinName = "DevExpress Dark Style";
            this.progressPanel1.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.progressPanel1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.progressPanel1.Name = "progressPanel1";
            this.progressPanel1.Size = new System.Drawing.Size(151, 52);
            this.progressPanel1.TabIndex = 4;
            this.progressPanel1.Text = "progressPanel1";
            this.progressPanel1.Visible = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(989, 81);
            this.panel1.TabIndex = 7;
            // 
            // groupControl1
            // 
            this.groupControl1.Appearance.BackColor = System.Drawing.Color.White;
            this.groupControl1.Appearance.Options.UseBackColor = true;
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.txtStoneNo);
            this.groupControl1.Controls.Add(this.progressPanel1);
            this.groupControl1.Controls.Add(this.panel3);
            this.groupControl1.Controls.Add(this.BtnDownload);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(989, 80);
            this.groupControl1.TabIndex = 205;
            this.groupControl1.Text = "Comma Filter";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.RdoGIA);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(2, 23);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(86, 55);
            this.panel3.TabIndex = 196;
            // 
            // RdoGIA
            // 
            this.RdoGIA.AllowTabKeyOnEnter = false;
            this.RdoGIA.AutoSize = true;
            this.RdoGIA.Checked = true;
            this.RdoGIA.Font = new System.Drawing.Font("Verdana", 10F);
            this.RdoGIA.ForeColor = System.Drawing.Color.Black;
            this.RdoGIA.Location = new System.Drawing.Point(8, 12);
            this.RdoGIA.Name = "RdoGIA";
            this.RdoGIA.Size = new System.Drawing.Size(52, 21);
            this.RdoGIA.TabIndex = 7;
            this.RdoGIA.TabStop = true;
            this.RdoGIA.Text = "&GIA";
            this.RdoGIA.ToolTips = "";
            this.RdoGIA.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.MainGrd);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 81);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(989, 532);
            this.panel2.TabIndex = 7;
            // 
            // MainGrd
            // 
            this.MainGrd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGrd.Location = new System.Drawing.Point(0, 0);
            this.MainGrd.MainView = this.GrdDet;
            this.MainGrd.Name = "MainGrd";
            this.MainGrd.Size = new System.Drawing.Size(989, 532);
            this.MainGrd.TabIndex = 4;
            this.MainGrd.TabStop = false;
            this.MainGrd.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GrdDet});
            // 
            // GrdDet
            // 
            this.GrdDet.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.GrdDet.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.GrdDet.Appearance.FocusedCell.Options.UseBackColor = true;
            this.GrdDet.Appearance.FooterPanel.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.FooterPanel.Options.UseFont = true;
            this.GrdDet.Appearance.GroupFooter.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.GroupFooter.Options.UseFont = true;
            this.GrdDet.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.HeaderPanel.Options.UseFont = true;
            this.GrdDet.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.GrdDet.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDet.Appearance.HorzLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDet.Appearance.HorzLine.Options.UseBackColor = true;
            this.GrdDet.Appearance.Row.Font = new System.Drawing.Font("Verdana", 8.5F);
            this.GrdDet.Appearance.Row.Options.UseFont = true;
            this.GrdDet.Appearance.VertLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDet.Appearance.VertLine.Options.UseBackColor = true;
            this.GrdDet.AppearancePrint.EvenRow.BackColor = System.Drawing.Color.Transparent;
            this.GrdDet.AppearancePrint.EvenRow.Options.UseBackColor = true;
            this.GrdDet.AppearancePrint.FooterPanel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.GrdDet.AppearancePrint.FooterPanel.Options.UseFont = true;
            this.GrdDet.AppearancePrint.HeaderPanel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.GrdDet.AppearancePrint.HeaderPanel.Options.UseFont = true;
            this.GrdDet.AppearancePrint.OddRow.BackColor = System.Drawing.Color.Transparent;
            this.GrdDet.AppearancePrint.OddRow.Options.UseBackColor = true;
            this.GrdDet.AppearancePrint.Row.BackColor = System.Drawing.Color.Transparent;
            this.GrdDet.AppearancePrint.Row.Options.UseBackColor = true;
            this.GrdDet.ColumnPanelRowHeight = 35;
            this.GrdDet.GridControl = this.MainGrd;
            this.GrdDet.Name = "GrdDet";
            this.GrdDet.OptionsBehavior.Editable = false;
            this.GrdDet.OptionsClipboard.AllowCopy = DevExpress.Utils.DefaultBoolean.True;
            this.GrdDet.OptionsClipboard.CopyColumnHeaders = DevExpress.Utils.DefaultBoolean.False;
            this.GrdDet.OptionsFilter.AllowFilterEditor = false;
            this.GrdDet.OptionsNavigation.EnterMoveNextColumn = true;
            this.GrdDet.OptionsPrint.EnableAppearanceEvenRow = true;
            this.GrdDet.OptionsPrint.EnableAppearanceOddRow = true;
            this.GrdDet.OptionsPrint.ExpandAllGroups = false;
            this.GrdDet.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.GrdDet.OptionsSelection.MultiSelect = true;
            this.GrdDet.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.GrdDet.OptionsView.ColumnAutoWidth = false;
            this.GrdDet.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.GrdDet.OptionsView.ShowAutoFilterRow = true;
            this.GrdDet.OptionsView.ShowFooter = true;
            this.GrdDet.OptionsView.ShowGroupPanel = false;
            this.GrdDet.RowHeight = 23;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // FrmCertificateDownload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 613);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FrmCertificateDownload";
            this.Tag = "CertificateDownload";
            this.Text = "CERTIFICATE DOWNLOAD";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainGrd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TextBox txtStoneNo;
        private DevControlLib.cDevSimpleButton BtnDownload;
        private DevExpress.XtraWaitForm.ProgressPanel progressPanel1;
        private AxonContLib.cPanel panel1;
        private AxonContLib.cPanel panel2;
        private AxonContLib.cRadioButton RdoGIA;
        private DevControlLib.cDevGroupControl groupControl1;
        private AxonContLib.cPanel panel3;
        private DevExpress.XtraGrid.GridControl MainGrd;
        private DevExpress.XtraGrid.Views.Grid.GridView GrdDet;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}