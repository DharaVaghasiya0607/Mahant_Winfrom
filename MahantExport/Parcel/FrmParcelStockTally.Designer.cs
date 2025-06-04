namespace MahantExport.Parcel
{
    partial class FrmParcelStockTally
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmParcelStockTally));
            this.panel1 = new AxonContLib.cPanel(this.components);
            this.MainGrid = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.DeleteSelectedSizeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GrdDet = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ReptxtShapeName = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn18 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn28 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.ReptxtRemark = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.panel3 = new AxonContLib.cPanel(this.components);
            this.txtPassword = new AxonContLib.cTextBox(this.components);
            this.DTPStockTallyDate = new AxonContLib.cDateTimePicker(this.components);
            this.label4 = new AxonContLib.cLabel(this.components);
            this.BtnShow = new DevControlLib.cDevSimpleButton(this.components);
            this.panel2 = new AxonContLib.cPanel(this.components);
            this.BtnExport = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnSave = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnClear = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnBack = new DevControlLib.cDevSimpleButton(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReptxtShapeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReptxtRemark)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.MainGrid);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1069, 500);
            this.panel1.TabIndex = 0;
            // 
            // MainGrid
            // 
            this.MainGrid.ContextMenuStrip = this.contextMenuStrip1;
            this.MainGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGrid.Location = new System.Drawing.Point(0, 47);
            this.MainGrid.MainView = this.GrdDet;
            this.MainGrid.Name = "MainGrid";
            this.MainGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.ReptxtShapeName,
            this.ReptxtRemark});
            this.MainGrid.Size = new System.Drawing.Size(1069, 403);
            this.MainGrid.TabIndex = 37;
            this.MainGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GrdDet});
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeleteSelectedSizeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(211, 26);
            // 
            // DeleteSelectedSizeToolStripMenuItem
            // 
            this.DeleteSelectedSizeToolStripMenuItem.BackColor = System.Drawing.SystemColors.Control;
            this.DeleteSelectedSizeToolStripMenuItem.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.DeleteSelectedSizeToolStripMenuItem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.DeleteSelectedSizeToolStripMenuItem.Name = "DeleteSelectedSizeToolStripMenuItem";
            this.DeleteSelectedSizeToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.DeleteSelectedSizeToolStripMenuItem.Text = "Delete Selected Size";
            this.DeleteSelectedSizeToolStripMenuItem.Click += new System.EventHandler(this.DeleteSelectedSizeToolStripMenuItem_Click);
            // 
            // GrdDet
            // 
            this.GrdDet.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.GrdDet.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.GrdDet.Appearance.FocusedCell.Font = new System.Drawing.Font("Verdana", 8F);
            this.GrdDet.Appearance.FocusedCell.Options.UseBackColor = true;
            this.GrdDet.Appearance.FocusedCell.Options.UseFont = true;
            this.GrdDet.Appearance.FocusedRow.Font = new System.Drawing.Font("Verdana", 8F);
            this.GrdDet.Appearance.FocusedRow.Options.UseFont = true;
            this.GrdDet.Appearance.FooterPanel.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.FooterPanel.Options.UseFont = true;
            this.GrdDet.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.HeaderPanel.Options.UseFont = true;
            this.GrdDet.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.GrdDet.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDet.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.GrdDet.Appearance.HorzLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDet.Appearance.HorzLine.Options.UseBackColor = true;
            this.GrdDet.Appearance.Row.Font = new System.Drawing.Font("Verdana", 8F);
            this.GrdDet.Appearance.Row.Options.UseFont = true;
            this.GrdDet.Appearance.Row.Options.UseTextOptions = true;
            this.GrdDet.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDet.Appearance.VertLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDet.Appearance.VertLine.Options.UseBackColor = true;
            this.GrdDet.AppearancePrint.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 13F);
            this.GrdDet.AppearancePrint.HeaderPanel.Options.UseFont = true;
            this.GrdDet.ColumnPanelRowHeight = 40;
            this.GrdDet.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn5,
            this.gridColumn7,
            this.gridColumn9,
            this.gridColumn12,
            this.gridColumn13,
            this.gridColumn8,
            this.gridColumn16,
            this.gridColumn18,
            this.gridColumn15,
            this.gridColumn28,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn6,
            this.gridColumn10,
            this.gridColumn14,
            this.gridColumn11});
            this.GrdDet.GridControl = this.MainGrid;
            this.GrdDet.Name = "GrdDet";
            this.GrdDet.OptionsCustomization.AllowSort = false;
            this.GrdDet.OptionsFilter.AllowFilterEditor = false;
            this.GrdDet.OptionsPrint.ExpandAllGroups = false;
            this.GrdDet.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.GrdDet.OptionsView.ColumnAutoWidth = false;
            this.GrdDet.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.GrdDet.OptionsView.ShowAutoFilterRow = true;
            this.GrdDet.OptionsView.ShowFooter = true;
            this.GrdDet.OptionsView.ShowGroupPanel = false;
            this.GrdDet.RowHeight = 25;
            this.GrdDet.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.GrdDet_CellValueChanged);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "STOCK_ID";
            this.gridColumn1.FieldName = "STOCK_ID";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "Stock No";
            this.gridColumn5.FieldName = "STOCKNO";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn5.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 0;
            this.gridColumn5.Width = 114;
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn7.AppearanceCell.Options.UseFont = true;
            this.gridColumn7.Caption = "Balance Cts";
            this.gridColumn7.DisplayFormat.FormatString = "{0:N3}";
            this.gridColumn7.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn7.FieldName = "BALANCECARAT";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowEdit = false;
            this.gridColumn7.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn7.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 4;
            this.gridColumn7.Width = 116;
            // 
            // gridColumn9
            // 
            this.gridColumn9.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn9.AppearanceCell.ForeColor = System.Drawing.Color.Green;
            this.gridColumn9.AppearanceCell.Options.UseFont = true;
            this.gridColumn9.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn9.Caption = "Actual Cts";
            this.gridColumn9.DisplayFormat.FormatString = "{0:N2}";
            this.gridColumn9.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn9.FieldName = "ACTUALCARAT";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn9.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 5;
            this.gridColumn9.Width = 88;
            // 
            // gridColumn12
            // 
            this.gridColumn12.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn12.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn12.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn12.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn12.Caption = "SHAPE_ID";
            this.gridColumn12.FieldName = "SHAPE_ID";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.OptionsColumn.AllowEdit = false;
            this.gridColumn12.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // gridColumn13
            // 
            this.gridColumn13.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn13.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn13.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn13.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn13.Caption = "Shape";
            this.gridColumn13.ColumnEdit = this.ReptxtShapeName;
            this.gridColumn13.FieldName = "SHAPENAME";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.OptionsColumn.AllowEdit = false;
            this.gridColumn13.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 1;
            this.gridColumn13.Width = 67;
            // 
            // ReptxtShapeName
            // 
            this.ReptxtShapeName.AutoHeight = false;
            this.ReptxtShapeName.Name = "ReptxtShapeName";
            // 
            // gridColumn8
            // 
            this.gridColumn8.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn8.AppearanceCell.ForeColor = System.Drawing.Color.Navy;
            this.gridColumn8.AppearanceCell.Options.UseFont = true;
            this.gridColumn8.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn8.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn8.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn8.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn8.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn8.Caption = "Plus Cts";
            this.gridColumn8.DisplayFormat.FormatString = "{0:N2}";
            this.gridColumn8.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn8.FieldName = "PLUSCARAT";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowEdit = false;
            this.gridColumn8.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn8.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 7;
            // 
            // gridColumn16
            // 
            this.gridColumn16.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn16.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.gridColumn16.AppearanceCell.Options.UseFont = true;
            this.gridColumn16.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn16.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn16.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn16.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn16.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn16.Caption = "Loss Cts";
            this.gridColumn16.DisplayFormat.FormatString = "{0:N2}";
            this.gridColumn16.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn16.FieldName = "LOSSCARAT";
            this.gridColumn16.Name = "gridColumn16";
            this.gridColumn16.OptionsColumn.AllowEdit = false;
            this.gridColumn16.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn16.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn16.Visible = true;
            this.gridColumn16.VisibleIndex = 8;
            this.gridColumn16.Width = 77;
            // 
            // gridColumn18
            // 
            this.gridColumn18.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn18.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn18.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn18.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn18.Caption = "MIXSIZE_ID";
            this.gridColumn18.FieldName = "SIZE_ID";
            this.gridColumn18.Name = "gridColumn18";
            this.gridColumn18.OptionsColumn.AllowEdit = false;
            this.gridColumn18.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn18.Width = 96;
            // 
            // gridColumn15
            // 
            this.gridColumn15.Caption = "Sr.";
            this.gridColumn15.FieldName = "ENTRYSRNO";
            this.gridColumn15.Name = "gridColumn15";
            this.gridColumn15.OptionsColumn.AllowEdit = false;
            this.gridColumn15.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn15.Width = 46;
            // 
            // gridColumn28
            // 
            this.gridColumn28.Caption = "MIXCLARITY_ID";
            this.gridColumn28.FieldName = "CLARITY_ID";
            this.gridColumn28.Name = "gridColumn28";
            this.gridColumn28.OptionsColumn.AllowEdit = false;
            this.gridColumn28.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn28.Width = 52;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Size";
            this.gridColumn2.FieldName = "SIZENAME";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Cla";
            this.gridColumn3.FieldName = "CLARITYNAME";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 3;
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "STOCKTALLY_ID";
            this.gridColumn4.FieldName = "STOCKTALLY_ID";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // gridColumn6
            // 
            this.gridColumn6.Caption = "Dept";
            this.gridColumn6.FieldName = "DEPARTMENTNAME";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 9;
            this.gridColumn6.Width = 110;
            // 
            // gridColumn10
            // 
            this.gridColumn10.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn10.AppearanceCell.Options.UseFont = true;
            this.gridColumn10.Caption = "Sale Rate";
            this.gridColumn10.FieldName = "SALEPRICEPERCARAT";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsColumn.AllowEdit = false;
            this.gridColumn10.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 6;
            // 
            // gridColumn14
            // 
            this.gridColumn14.Caption = "OldSaleRate";
            this.gridColumn14.FieldName = "OLDSALEPRICEPERCARAT";
            this.gridColumn14.Name = "gridColumn14";
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "Remark";
            this.gridColumn11.FieldName = "REMARK";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 10;
            this.gridColumn11.Width = 298;
            // 
            // ReptxtRemark
            // 
            this.ReptxtRemark.AutoHeight = false;
            this.ReptxtRemark.Name = "ReptxtRemark";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.txtPassword);
            this.panel3.Controls.Add(this.DTPStockTallyDate);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.BtnShow);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1069, 47);
            this.panel3.TabIndex = 39;
            // 
            // txtPassword
            // 
            this.txtPassword.ActivationColor = false;
            this.txtPassword.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtPassword.AllowTabKeyOnEnter = false;
            this.txtPassword.BackColor = System.Drawing.SystemColors.Control;
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.Format = "";
            this.txtPassword.IsComplusory = false;
            this.txtPassword.Location = new System.Drawing.Point(348, 25);
            this.txtPassword.MaxLength = 100;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.SelectAllTextOnFocus = true;
            this.txtPassword.Size = new System.Drawing.Size(111, 15);
            this.txtPassword.TabIndex = 162;
            this.txtPassword.ToolTips = "";
            this.txtPassword.WaterMarkText = null;
            this.txtPassword.TextChanged += new System.EventHandler(this.txtPassword_TextChanged);
            // 
            // DTPStockTallyDate
            // 
            this.DTPStockTallyDate.AllowTabKeyOnEnter = false;
            this.DTPStockTallyDate.Checked = false;
            this.DTPStockTallyDate.CustomFormat = "dd/MM/yyyy";
            this.DTPStockTallyDate.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DTPStockTallyDate.ForeColor = System.Drawing.Color.Black;
            this.DTPStockTallyDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DTPStockTallyDate.Location = new System.Drawing.Point(122, 13);
            this.DTPStockTallyDate.Name = "DTPStockTallyDate";
            this.DTPStockTallyDate.Size = new System.Drawing.Size(126, 22);
            this.DTPStockTallyDate.TabIndex = 161;
            this.DTPStockTallyDate.ToolTips = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.label4.ForeColor = System.Drawing.Color.Navy;
            this.label4.Location = new System.Drawing.Point(6, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Stock Tally Date";
            this.label4.ToolTips = "";
            // 
            // BtnShow
            // 
            this.BtnShow.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnShow.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnShow.Appearance.Options.UseFont = true;
            this.BtnShow.Appearance.Options.UseForeColor = true;
            this.BtnShow.ImageOptions.Image = global::MahantExport.Properties.Resources.btnsearch;
            this.BtnShow.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnShow.ImageOptions.SvgImage")));
            this.BtnShow.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnShow.Location = new System.Drawing.Point(254, 7);
            this.BtnShow.Name = "BtnShow";
            this.BtnShow.Size = new System.Drawing.Size(90, 35);
            this.BtnShow.TabIndex = 34;
            this.BtnShow.Text = "Show";
            this.BtnShow.Click += new System.EventHandler(this.BtnShow_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Silver;
            this.panel2.Controls.Add(this.BtnExport);
            this.panel2.Controls.Add(this.BtnSave);
            this.panel2.Controls.Add(this.BtnClear);
            this.panel2.Controls.Add(this.BtnBack);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 450);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1069, 50);
            this.panel2.TabIndex = 38;
            // 
            // BtnExport
            // 
            this.BtnExport.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnExport.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnExport.Appearance.Options.UseFont = true;
            this.BtnExport.Appearance.Options.UseForeColor = true;
            this.BtnExport.ImageOptions.Image = global::MahantExport.Properties.Resources.btnexcelexport;
            this.BtnExport.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnExport.ImageOptions.SvgImage")));
            this.BtnExport.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnExport.Location = new System.Drawing.Point(201, 8);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(90, 35);
            this.BtnExport.TabIndex = 33;
            this.BtnExport.TabStop = false;
            this.BtnExport.Text = "Export";
            this.BtnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnSave.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnSave.Appearance.Options.UseFont = true;
            this.BtnSave.Appearance.Options.UseForeColor = true;
            this.BtnSave.ImageOptions.Image = global::MahantExport.Properties.Resources.btnsave;
            this.BtnSave.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSave.ImageOptions.SvgImage")));
            this.BtnSave.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnSave.Location = new System.Drawing.Point(9, 8);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(90, 35);
            this.BtnSave.TabIndex = 30;
            this.BtnSave.Text = "&Save";
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnClear.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnClear.Appearance.Options.UseFont = true;
            this.BtnClear.Appearance.Options.UseForeColor = true;
            this.BtnClear.ImageOptions.Image = global::MahantExport.Properties.Resources.btnclear;
            this.BtnClear.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnClear.ImageOptions.SvgImage")));
            this.BtnClear.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnClear.Location = new System.Drawing.Point(105, 8);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(90, 35);
            this.BtnClear.TabIndex = 31;
            this.BtnClear.TabStop = false;
            this.BtnClear.Text = "&Clear";
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // BtnBack
            // 
            this.BtnBack.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnBack.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnBack.Appearance.Options.UseFont = true;
            this.BtnBack.Appearance.Options.UseForeColor = true;
            this.BtnBack.ImageOptions.Image = global::MahantExport.Properties.Resources.btnexit;
            this.BtnBack.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnBack.ImageOptions.SvgImage")));
            this.BtnBack.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnBack.Location = new System.Drawing.Point(297, 8);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.Size = new System.Drawing.Size(90, 35);
            this.BtnBack.TabIndex = 32;
            this.BtnBack.TabStop = false;
            this.BtnBack.Text = "E&xit";
            // 
            // FrmParcelStockTally
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1069, 500);
            this.Controls.Add(this.panel1);
            this.LookAndFeel.SkinName = "London Liquid Sky";
            this.LookAndFeel.UseDefaultLookAndFeel =true;
            this.Name = "FrmParcelStockTally";
            this.Text = "PARCEL STOCK TALLY";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReptxtShapeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReptxtRemark)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

        private AxonContLib.cPanel panel1;
        private DevExpress.XtraGrid.GridControl MainGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView GrdDet;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit ReptxtShapeName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn16;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn18;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit ReptxtRemark;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn15;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn28;
        private AxonContLib.cPanel panel3;
        private AxonContLib.cDateTimePicker DTPStockTallyDate;
        private AxonContLib.cLabel label4;
        private DevControlLib.cDevSimpleButton BtnShow;
        private AxonContLib.cPanel panel2;
        private DevControlLib.cDevSimpleButton BtnExport;
        private DevControlLib.cDevSimpleButton BtnSave;
        private DevControlLib.cDevSimpleButton BtnClear;
        private DevControlLib.cDevSimpleButton BtnBack;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem DeleteSelectedSizeToolStripMenuItem;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private AxonContLib.cTextBox txtPassword;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;

    }
}