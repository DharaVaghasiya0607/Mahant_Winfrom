namespace MahantExport.Masters
{
    partial class FrmBanner
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBanner));
            this.MainGrid = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteSelectedAmountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GrdDet = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repTxtFilePath = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repChkIsActive = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repTxtRemark = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1 = new AxonContLib.cPanel(this.components);
            this.chkISActive = new DevExpress.XtraEditors.CheckEdit();
            this.BtnSave = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnDelete = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnExit = new DevControlLib.cDevSimpleButton(this.components);
            this.txtRemark = new AxonContLib.cTextBox(this.components);
            this.BtnAdd = new DevControlLib.cDevSimpleButton(this.components);
            this.Label1 = new AxonContLib.cLabel(this.components);
            this.BtnExport = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnBrowse = new DevControlLib.cDevSimpleButton(this.components);
            this.txtFileName = new AxonContLib.cTextBox(this.components);
            this.Lable1 = new AxonContLib.cLabel(this.components);
            this.panel2 = new AxonContLib.cPanel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtFilePath)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repChkIsActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtRemark)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkISActive.Properties)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainGrid
            // 
            this.MainGrid.ContextMenuStrip = this.contextMenuStrip1;
            this.MainGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGrid.Location = new System.Drawing.Point(0, 0);
            this.MainGrid.MainView = this.GrdDet;
            this.MainGrid.Name = "MainGrid";
            this.MainGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repChkIsActive,
            this.repTxtRemark,
            this.repTxtFilePath});
            this.MainGrid.Size = new System.Drawing.Size(1106, 551);
            this.MainGrid.TabIndex = 0;
            this.MainGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GrdDet});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteSelectedAmountToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(214, 26);
            // 
            // deleteSelectedAmountToolStripMenuItem
            // 
            this.deleteSelectedAmountToolStripMenuItem.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.deleteSelectedAmountToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.deleteSelectedAmountToolStripMenuItem.Name = "deleteSelectedAmountToolStripMenuItem";
            this.deleteSelectedAmountToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.deleteSelectedAmountToolStripMenuItem.Text = "Delete Selected Item";
            this.deleteSelectedAmountToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectedAmountToolStripMenuItem_Click);
            // 
            // GrdDet
            // 
            this.GrdDet.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.GrdDet.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.GrdDet.Appearance.FocusedCell.Font = new System.Drawing.Font("Verdana", 9F);
            this.GrdDet.Appearance.FocusedCell.Options.UseBackColor = true;
            this.GrdDet.Appearance.FocusedCell.Options.UseFont = true;
            this.GrdDet.Appearance.FocusedRow.Font = new System.Drawing.Font("Verdana", 9F);
            this.GrdDet.Appearance.FocusedRow.Options.UseFont = true;
            this.GrdDet.Appearance.FooterPanel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.FooterPanel.Options.UseFont = true;
            this.GrdDet.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.HeaderPanel.Options.UseFont = true;
            this.GrdDet.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.GrdDet.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDet.Appearance.HorzLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDet.Appearance.HorzLine.Options.UseBackColor = true;
            this.GrdDet.Appearance.Row.Font = new System.Drawing.Font("Verdana", 9F);
            this.GrdDet.Appearance.Row.Options.UseFont = true;
            this.GrdDet.Appearance.VertLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDet.Appearance.VertLine.Options.UseBackColor = true;
            this.GrdDet.AppearancePrint.FooterPanel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.GrdDet.AppearancePrint.FooterPanel.Options.UseFont = true;
            this.GrdDet.AppearancePrint.HeaderPanel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.GrdDet.AppearancePrint.HeaderPanel.Options.UseFont = true;
            this.GrdDet.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
            this.GrdDet.AppearancePrint.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDet.AppearancePrint.Row.Font = new System.Drawing.Font("Verdana", 9F);
            this.GrdDet.AppearancePrint.Row.Options.UseFont = true;
            this.GrdDet.AppearancePrint.Row.Options.UseTextOptions = true;
            this.GrdDet.AppearancePrint.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDet.ColumnPanelRowHeight = 25;
            this.GrdDet.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn6,
            this.gridColumn4,
            this.gridColumn3,
            this.gridColumn7});
            this.GrdDet.GridControl = this.MainGrid;
            this.GrdDet.Name = "GrdDet";
            this.GrdDet.OptionsBehavior.Editable = false;
            this.GrdDet.OptionsCustomization.AllowSort = false;
            this.GrdDet.OptionsFilter.AllowFilterEditor = false;
            this.GrdDet.OptionsNavigation.EnterMoveNextColumn = true;
            this.GrdDet.OptionsPrint.ExpandAllGroups = false;
            this.GrdDet.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.GrdDet.OptionsView.ColumnAutoWidth = false;
            this.GrdDet.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.GrdDet.OptionsView.ShowAutoFilterRow = true;
            this.GrdDet.OptionsView.ShowFooter = true;
            this.GrdDet.OptionsView.ShowGroupPanel = false;
            this.GrdDet.RowHeight = 25;
            this.GrdDet.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.GrdDet_RowCellClick);
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Font = new System.Drawing.Font("Verdana", 9F);
            this.gridColumn6.AppearanceCell.Options.UseFont = true;
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.gridColumn6.AppearanceHeader.Options.UseFont = true;
            this.gridColumn6.Caption = "File Path";
            this.gridColumn6.ColumnEdit = this.repTxtFilePath;
            this.gridColumn6.FieldName = "FILEPATH";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn6.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 0;
            this.gridColumn6.Width = 500;
            // 
            // repTxtFilePath
            // 
            this.repTxtFilePath.AutoHeight = false;
            this.repTxtFilePath.MaxLength = 100;
            this.repTxtFilePath.Name = "repTxtFilePath";
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Font = new System.Drawing.Font("Verdana", 9F);
            this.gridColumn4.AppearanceCell.Options.UseFont = true;
            this.gridColumn4.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.gridColumn4.AppearanceHeader.Options.UseFont = true;
            this.gridColumn4.Caption = "Active";
            this.gridColumn4.ColumnEdit = this.repChkIsActive;
            this.gridColumn4.FieldName = "ISACTIVE";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 1;
            this.gridColumn4.Width = 60;
            // 
            // repChkIsActive
            // 
            this.repChkIsActive.AutoHeight = false;
            this.repChkIsActive.Caption = "Check";
            this.repChkIsActive.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
            this.repChkIsActive.ImageOptions.ImageChecked = global::MahantExport.Properties.Resources.Checked;
            this.repChkIsActive.ImageOptions.ImageUnchecked = global::MahantExport.Properties.Resources.Unchecked;
            this.repChkIsActive.Name = "repChkIsActive";
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Font = new System.Drawing.Font("Verdana", 9F);
            this.gridColumn3.AppearanceCell.Options.UseFont = true;
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.gridColumn3.AppearanceHeader.Options.UseFont = true;
            this.gridColumn3.Caption = "Remark";
            this.gridColumn3.ColumnEdit = this.repTxtRemark;
            this.gridColumn3.FieldName = "REMARK";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 500;
            // 
            // repTxtRemark
            // 
            this.repTxtRemark.AutoHeight = false;
            this.repTxtRemark.Mask.EditMask = "[0-9]{0,6}\\.[0-9]{0,2}";
            this.repTxtRemark.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.RegEx;
            this.repTxtRemark.Name = "repTxtRemark";
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceCell.Font = new System.Drawing.Font("Verdana", 9F);
            this.gridColumn7.AppearanceCell.Options.UseFont = true;
            this.gridColumn7.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.gridColumn7.AppearanceHeader.Options.UseFont = true;
            this.gridColumn7.Caption = "Banner ID";
            this.gridColumn7.FieldName = "BANNER_ID";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.Controls.Add(this.chkISActive);
            this.panel1.Controls.Add(this.BtnSave);
            this.panel1.Controls.Add(this.BtnDelete);
            this.panel1.Controls.Add(this.BtnExit);
            this.panel1.Controls.Add(this.txtRemark);
            this.panel1.Controls.Add(this.BtnAdd);
            this.panel1.Controls.Add(this.Label1);
            this.panel1.Controls.Add(this.BtnExport);
            this.panel1.Controls.Add(this.BtnBrowse);
            this.panel1.Controls.Add(this.txtFileName);
            this.panel1.Controls.Add(this.Lable1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1106, 74);
            this.panel1.TabIndex = 31;
            // 
            // chkISActive
            // 
            this.chkISActive.Location = new System.Drawing.Point(451, 8);
            this.chkISActive.Name = "chkISActive";
            this.chkISActive.Properties.Appearance.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.chkISActive.Properties.Appearance.Options.UseFont = true;
            this.chkISActive.Properties.Caption = " Active";
            this.chkISActive.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
            this.chkISActive.Properties.ImageOptions.ImageChecked = global::MahantExport.Properties.Resources.Checked;
            this.chkISActive.Properties.ImageOptions.ImageUnchecked = global::MahantExport.Properties.Resources.Unchecked;
            this.chkISActive.Size = new System.Drawing.Size(84, 20);
            this.chkISActive.TabIndex = 195;
            // 
            // BtnSave
            // 
            this.BtnSave.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnSave.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnSave.Appearance.Options.UseFont = true;
            this.BtnSave.Appearance.Options.UseForeColor = true;
            this.BtnSave.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSave.ImageOptions.SvgImage")));
            this.BtnSave.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnSave.Location = new System.Drawing.Point(538, 31);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(91, 35);
            this.BtnSave.TabIndex = 190;
            this.BtnSave.Text = "&Save";
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnDelete.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnDelete.Appearance.Options.UseFont = true;
            this.BtnDelete.Appearance.Options.UseForeColor = true;
            this.BtnDelete.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnDelete.ImageOptions.SvgImage")));
            this.BtnDelete.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnDelete.Location = new System.Drawing.Point(631, 31);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(91, 35);
            this.BtnDelete.TabIndex = 194;
            this.BtnDelete.Text = "&Delete";
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnExit.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnExit.Appearance.Options.UseFont = true;
            this.BtnExit.Appearance.Options.UseForeColor = true;
            this.BtnExit.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnExit.ImageOptions.SvgImage")));
            this.BtnExit.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnExit.Location = new System.Drawing.Point(910, 31);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(91, 35);
            this.BtnExit.TabIndex = 193;
            this.BtnExit.TabStop = false;
            this.BtnExit.Text = "E&xit";
            this.BtnExit.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // txtRemark
            // 
            this.txtRemark.ActivationColor = true;
            this.txtRemark.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtRemark.AllowTabKeyOnEnter = false;
            this.txtRemark.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemark.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemark.Format = "";
            this.txtRemark.IsComplusory = false;
            this.txtRemark.Location = new System.Drawing.Point(78, 33);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.SelectAllTextOnFocus = true;
            this.txtRemark.Size = new System.Drawing.Size(337, 35);
            this.txtRemark.TabIndex = 39;
            this.txtRemark.ToolTips = "";
            this.txtRemark.WaterMarkText = null;
            // 
            // BtnAdd
            // 
            this.BtnAdd.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnAdd.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnAdd.Appearance.Options.UseFont = true;
            this.BtnAdd.Appearance.Options.UseForeColor = true;
            this.BtnAdd.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnAdd.ImageOptions.SvgImage")));
            this.BtnAdd.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnAdd.Location = new System.Drawing.Point(724, 31);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(91, 35);
            this.BtnAdd.TabIndex = 191;
            this.BtnAdd.TabStop = false;
            this.BtnAdd.Text = "&Clear";
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.ForeColor = System.Drawing.Color.Black;
            this.Label1.Location = new System.Drawing.Point(11, 43);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(58, 14);
            this.Label1.TabIndex = 38;
            this.Label1.Text = "Remark";
            this.Label1.ToolTips = "";
            // 
            // BtnExport
            // 
            this.BtnExport.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnExport.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnExport.Appearance.Options.UseFont = true;
            this.BtnExport.Appearance.Options.UseForeColor = true;
            this.BtnExport.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnExport.ImageOptions.SvgImage")));
            this.BtnExport.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnExport.Location = new System.Drawing.Point(817, 31);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(91, 35);
            this.BtnExport.TabIndex = 192;
            this.BtnExport.TabStop = false;
            this.BtnExport.Text = "Export";
            this.BtnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // BtnBrowse
            // 
            this.BtnBrowse.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnBrowse.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnBrowse.Appearance.Options.UseFont = true;
            this.BtnBrowse.Appearance.Options.UseForeColor = true;
            this.BtnBrowse.Location = new System.Drawing.Point(416, 6);
            this.BtnBrowse.Name = "BtnBrowse";
            this.BtnBrowse.Size = new System.Drawing.Size(26, 24);
            this.BtnBrowse.TabIndex = 36;
            this.BtnBrowse.Text = "...";
            this.BtnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.ActivationColor = true;
            this.txtFileName.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtFileName.AllowTabKeyOnEnter = false;
            this.txtFileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFileName.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFileName.Format = "";
            this.txtFileName.IsComplusory = false;
            this.txtFileName.Location = new System.Drawing.Point(78, 7);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.SelectAllTextOnFocus = true;
            this.txtFileName.Size = new System.Drawing.Size(337, 22);
            this.txtFileName.TabIndex = 35;
            this.txtFileName.ToolTips = "";
            this.txtFileName.WaterMarkText = null;
            // 
            // Lable1
            // 
            this.Lable1.AutoSize = true;
            this.Lable1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Lable1.ForeColor = System.Drawing.Color.Black;
            this.Lable1.Location = new System.Drawing.Point(12, 11);
            this.Lable1.Name = "Lable1";
            this.Lable1.Size = new System.Drawing.Size(69, 14);
            this.Lable1.TabIndex = 34;
            this.Lable1.Text = "File Path ";
            this.Lable1.ToolTips = "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.MainGrid);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 74);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1106, 551);
            this.panel2.TabIndex = 189;
            // 
            // FrmBanner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1106, 625);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.LookAndFeel.SkinName = "London Liquid Sky";
            this.LookAndFeel.UseDefaultLookAndFeel =true;
            this.Name = "FrmBanner";
            this.Tag = "BannerMaster\r\n";
            this.Text = "BANNER MASTER";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmLedger_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtFilePath)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repChkIsActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtRemark)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chkISActive.Properties)).EndInit();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl MainGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView GrdDet;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repChkIsActive;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repTxtRemark;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repTxtFilePath;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectedAmountToolStripMenuItem;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private AxonContLib.cPanel panel1;
        private AxonContLib.cPanel panel2;
        private AxonContLib.cLabel Lable1;
        private AxonContLib.cTextBox txtFileName;
        private DevControlLib.cDevSimpleButton BtnBrowse;
        private AxonContLib.cTextBox txtRemark;
        private AxonContLib.cLabel Label1;
        private DevControlLib.cDevSimpleButton BtnDelete;
        private DevControlLib.cDevSimpleButton BtnExit;
        private DevControlLib.cDevSimpleButton BtnSave;
        private DevControlLib.cDevSimpleButton BtnExport;
        private DevControlLib.cDevSimpleButton BtnAdd;
        private DevExpress.XtraEditors.CheckEdit chkISActive;


    }
}