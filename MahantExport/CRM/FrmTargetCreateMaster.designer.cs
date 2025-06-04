namespace MahantExport.CRM
{
	partial class FrmTargetCreateMaster
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmTargetCreateMaster));
            this.panel2 = new AxonContLib.cPanel(this.components);
            this.CmbMonth = new AxonContLib.cComboBox(this.components);
            this.BtnShow = new DevControlLib.cDevSimpleButton(this.components);
            this.txtYear = new AxonContLib.cTextBox(this.components);
            this.cLabel2 = new AxonContLib.cLabel(this.components);
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            this.BtnExport = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnSave = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnAdd = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnBack = new DevControlLib.cDevSimpleButton(this.components);
            this.MainGrid = new DevExpress.XtraGrid.GridControl();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteSelectedAmountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GrdDet = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repTxtRemark = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.CmbDollarType = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.repTxtShapeName = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repTxtCutName = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repTxtPolName = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repTxtSymName = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.panel1 = new AxonContLib.cPanel(this.components);
            this.BtnLeft = new DevControlLib.cDevSimpleButton(this.components);
            this.PnlCopyPaste = new AxonContLib.cPanel(this.components);
            this.CmbCopyToMonth = new AxonContLib.cComboBox(this.components);
            this.cLabel5 = new AxonContLib.cLabel(this.components);
            this.cLabel6 = new AxonContLib.cLabel(this.components);
            this.txtCopyToYear = new AxonContLib.cTextBox(this.components);
            this.BtnCopy = new DevControlLib.cDevSimpleButton(this.components);
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtRemark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbDollarType)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtShapeName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtCutName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtPolName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtSymName)).BeginInit();
            this.panel1.SuspendLayout();
            this.PnlCopyPaste.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.CmbMonth);
            this.panel2.Controls.Add(this.BtnShow);
            this.panel2.Controls.Add(this.txtYear);
            this.panel2.Controls.Add(this.cLabel2);
            this.panel2.Controls.Add(this.cLabel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1137, 44);
            this.panel2.TabIndex = 14;
            // 
            // CmbMonth
            // 
            this.CmbMonth.AllowTabKeyOnEnter = false;
            this.CmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbMonth.Font = new System.Drawing.Font("Verdana", 11.25F);
            this.CmbMonth.ForeColor = System.Drawing.Color.Black;
            this.CmbMonth.FormattingEnabled = true;
            this.CmbMonth.Items.AddRange(new object[] {
            "Jan",
            "Feb",
            "Mar",
            "Apr",
            "May",
            "Jun",
            "Jul",
            "Aug",
            "Sep",
            "Oct",
            "Nov",
            "Dec"});
            this.CmbMonth.Location = new System.Drawing.Point(180, 9);
            this.CmbMonth.MaxDropDownItems = 12;
            this.CmbMonth.Name = "CmbMonth";
            this.CmbMonth.Size = new System.Drawing.Size(126, 26);
            this.CmbMonth.TabIndex = 10;
            this.CmbMonth.ToolTips = "";
            // 
            // BtnShow
            // 
            this.BtnShow.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnShow.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnShow.Appearance.Options.UseFont = true;
            this.BtnShow.Appearance.Options.UseForeColor = true;
            this.BtnShow.ImageOptions.Image = global::MahantExport.Properties.Resources.btnsearch;
            this.BtnShow.Location = new System.Drawing.Point(311, 6);
            this.BtnShow.Name = "BtnShow";
            this.BtnShow.Size = new System.Drawing.Size(103, 31);
            this.BtnShow.TabIndex = 15;
            this.BtnShow.Text = "Show";
            this.BtnShow.Click += new System.EventHandler(this.BtnShow_Click);
            // 
            // txtYear
            // 
            this.txtYear.ActivationColor = true;
            this.txtYear.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtYear.AllowTabKeyOnEnter = false;
            this.txtYear.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtYear.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.txtYear.Format = "######";
            this.txtYear.IsComplusory = false;
            this.txtYear.Location = new System.Drawing.Point(49, 11);
            this.txtYear.MaxLength = 4;
            this.txtYear.Name = "txtYear";
            this.txtYear.SelectAllTextOnFocus = true;
            this.txtYear.Size = new System.Drawing.Size(78, 23);
            this.txtYear.TabIndex = 1;
            this.txtYear.Text = "0";
            this.txtYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtYear.ToolTips = "";
            this.txtYear.WaterMarkText = null;
            // 
            // cLabel2
            // 
            this.cLabel2.AutoSize = true;
            this.cLabel2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel2.ForeColor = System.Drawing.Color.Black;
            this.cLabel2.Location = new System.Drawing.Point(9, 15);
            this.cLabel2.Name = "cLabel2";
            this.cLabel2.Size = new System.Drawing.Size(34, 14);
            this.cLabel2.TabIndex = 0;
            this.cLabel2.Text = "Year";
            this.cLabel2.ToolTips = "";
            // 
            // cLabel1
            // 
            this.cLabel1.AutoSize = true;
            this.cLabel1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel1.ForeColor = System.Drawing.Color.Black;
            this.cLabel1.Location = new System.Drawing.Point(131, 15);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(46, 14);
            this.cLabel1.TabIndex = 2;
            this.cLabel1.Text = "Month";
            this.cLabel1.ToolTips = "";
            // 
            // BtnExport
            // 
            this.BtnExport.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnExport.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnExport.Appearance.Options.UseFont = true;
            this.BtnExport.Appearance.Options.UseForeColor = true;
            this.BtnExport.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnExport.ImageOptions.SvgImage")));
            this.BtnExport.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnExport.Location = new System.Drawing.Point(225, 8);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(103, 35);
            this.BtnExport.TabIndex = 33;
            this.BtnExport.TabStop = false;
            this.BtnExport.Text = "Export";
            this.BtnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnSave.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnSave.Appearance.Options.UseFont = true;
            this.BtnSave.Appearance.Options.UseForeColor = true;
            this.BtnSave.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSave.ImageOptions.SvgImage")));
            this.BtnSave.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnSave.Location = new System.Drawing.Point(9, 8);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(103, 35);
            this.BtnSave.TabIndex = 0;
            this.BtnSave.Text = "&Save";
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnAdd
            // 
            this.BtnAdd.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnAdd.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnAdd.Appearance.Options.UseFont = true;
            this.BtnAdd.Appearance.Options.UseForeColor = true;
            this.BtnAdd.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnAdd.ImageOptions.SvgImage")));
            this.BtnAdd.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnAdd.Location = new System.Drawing.Point(117, 8);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(103, 35);
            this.BtnAdd.TabIndex = 31;
            this.BtnAdd.TabStop = false;
            this.BtnAdd.Text = "&Clear";
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // BtnBack
            // 
            this.BtnBack.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnBack.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnBack.Appearance.Options.UseFont = true;
            this.BtnBack.Appearance.Options.UseForeColor = true;
            this.BtnBack.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnBack.ImageOptions.SvgImage")));
            this.BtnBack.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnBack.Location = new System.Drawing.Point(333, 8);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.Size = new System.Drawing.Size(103, 35);
            this.BtnBack.TabIndex = 32;
            this.BtnBack.TabStop = false;
            this.BtnBack.Text = "E&xit";
            this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // MainGrid
            // 
            this.MainGrid.ContextMenuStrip = this.contextMenuStrip1;
            this.MainGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGrid.Location = new System.Drawing.Point(0, 44);
            this.MainGrid.MainView = this.GrdDet;
            this.MainGrid.Name = "MainGrid";
            this.MainGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repTxtRemark,
            this.CmbDollarType,
            this.repTxtShapeName,
            this.repTxtCutName,
            this.repTxtPolName,
            this.repTxtSymName});
            this.MainGrid.Size = new System.Drawing.Size(1137, 398);
            this.MainGrid.TabIndex = 12;
            this.MainGrid.Tag = "TargetCreateMaster";
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
            this.GrdDet.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(234)))), ((int)(((byte)(141)))));
            this.GrdDet.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(234)))), ((int)(((byte)(141)))));
            this.GrdDet.Appearance.FocusedCell.Options.UseBackColor = true;
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
            this.GrdDet.ColumnPanelRowHeight = 25;
            this.GrdDet.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3,
            this.gridColumn2,
            this.gridColumn1,
            this.gridColumn5,
            this.gridColumn7,
            this.gridColumn6,
            this.gridColumn4});
            this.GrdDet.GridControl = this.MainGrid;
            this.GrdDet.Name = "GrdDet";
            this.GrdDet.OptionsFilter.AllowFilterEditor = false;
            this.GrdDet.OptionsNavigation.EnterMoveNextColumn = true;
            this.GrdDet.OptionsPrint.ExpandAllGroups = false;
            this.GrdDet.OptionsView.ColumnAutoWidth = false;
            this.GrdDet.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.GrdDet.OptionsView.ShowAutoFilterRow = true;
            this.GrdDet.OptionsView.ShowFooter = true;
            this.GrdDet.OptionsView.ShowGroupPanel = false;
            this.GrdDet.RowHeight = 25;
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Ledger_Id";
            this.gridColumn3.FieldName = "Ledger_Id";
            this.gridColumn3.Name = "gridColumn3";
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "ID";
            this.gridColumn2.FieldName = "Employee_Id";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn2.Width = 86;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "TARGET_ID";
            this.gridColumn1.FieldName = "Target_Id";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn1.Width = 276;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "Employee";
            this.gridColumn5.FieldName = "LedgerName";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn5.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 0;
            this.gridColumn5.Width = 287;
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.Caption = "Sale Target Doller";
            this.gridColumn7.FieldName = "SaleTargetDollar";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn7.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 1;
            this.gridColumn7.Width = 167;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "No Of Customers";
            this.gridColumn6.FieldName = "NoOfCustomer";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn6.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 2;
            this.gridColumn6.Width = 170;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "No Of New Customers";
            this.gridColumn4.FieldName = "NoOfNewCustomer";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn4.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 172;
            // 
            // repTxtRemark
            // 
            this.repTxtRemark.AutoHeight = false;
            this.repTxtRemark.Name = "repTxtRemark";
            // 
            // CmbDollarType
            // 
            this.CmbDollarType.AutoHeight = false;
            this.CmbDollarType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbDollarType.Items.AddRange(new object[] {
            "Plus Dollar",
            "Minus Dollar",
            "Final DF +",
            "Final DF -",
            "Checker DF +",
            "Checker DF -",
            "Plan Variation",
            "Breaking"});
            this.CmbDollarType.Name = "CmbDollarType";
            this.CmbDollarType.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // repTxtShapeName
            // 
            this.repTxtShapeName.AutoHeight = false;
            this.repTxtShapeName.Name = "repTxtShapeName";
            // 
            // repTxtCutName
            // 
            this.repTxtCutName.AutoHeight = false;
            this.repTxtCutName.Name = "repTxtCutName";
            // 
            // repTxtPolName
            // 
            this.repTxtPolName.AutoHeight = false;
            this.repTxtPolName.Name = "repTxtPolName";
            // 
            // repTxtSymName
            // 
            this.repTxtSymName.AutoHeight = false;
            this.repTxtSymName.Name = "repTxtSymName";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.BtnLeft);
            this.panel1.Controls.Add(this.PnlCopyPaste);
            this.panel1.Controls.Add(this.BtnExport);
            this.panel1.Controls.Add(this.BtnSave);
            this.panel1.Controls.Add(this.BtnAdd);
            this.panel1.Controls.Add(this.BtnBack);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 442);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1137, 50);
            this.panel1.TabIndex = 13;
            // 
            // BtnLeft
            // 
            this.BtnLeft.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnLeft.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnLeft.Appearance.Options.UseFont = true;
            this.BtnLeft.Appearance.Options.UseForeColor = true;
            this.BtnLeft.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.BtnLeft.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnLeft.ImageOptions.Image = global::MahantExport.Properties.Resources.A2;
            this.BtnLeft.Location = new System.Drawing.Point(575, 0);
            this.BtnLeft.Name = "BtnLeft";
            this.BtnLeft.Size = new System.Drawing.Size(39, 50);
            this.BtnLeft.TabIndex = 37;
            this.BtnLeft.TabStop = false;
            this.BtnLeft.Click += new System.EventHandler(this.BtnLeft_Click);
            // 
            // PnlCopyPaste
            // 
            this.PnlCopyPaste.BackColor = System.Drawing.Color.WhiteSmoke;
            this.PnlCopyPaste.Controls.Add(this.CmbCopyToMonth);
            this.PnlCopyPaste.Controls.Add(this.cLabel5);
            this.PnlCopyPaste.Controls.Add(this.cLabel6);
            this.PnlCopyPaste.Controls.Add(this.txtCopyToYear);
            this.PnlCopyPaste.Controls.Add(this.BtnCopy);
            this.PnlCopyPaste.Dock = System.Windows.Forms.DockStyle.Right;
            this.PnlCopyPaste.Location = new System.Drawing.Point(614, 0);
            this.PnlCopyPaste.Name = "PnlCopyPaste";
            this.PnlCopyPaste.Size = new System.Drawing.Size(523, 50);
            this.PnlCopyPaste.TabIndex = 36;
            // 
            // CmbCopyToMonth
            // 
            this.CmbCopyToMonth.AllowTabKeyOnEnter = false;
            this.CmbCopyToMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbCopyToMonth.Font = new System.Drawing.Font("Verdana", 11.25F);
            this.CmbCopyToMonth.ForeColor = System.Drawing.Color.Black;
            this.CmbCopyToMonth.FormattingEnabled = true;
            this.CmbCopyToMonth.Items.AddRange(new object[] {
            "Jan",
            "Feb",
            "Mar",
            "Apr",
            "May",
            "Jun",
            "Jul",
            "Aug",
            "Sep",
            "Oct",
            "Nov",
            "Dec"});
            this.CmbCopyToMonth.Location = new System.Drawing.Point(310, 12);
            this.CmbCopyToMonth.MaxDropDownItems = 12;
            this.CmbCopyToMonth.Name = "CmbCopyToMonth";
            this.CmbCopyToMonth.Size = new System.Drawing.Size(70, 26);
            this.CmbCopyToMonth.TabIndex = 2;
            this.CmbCopyToMonth.ToolTips = "";
            // 
            // cLabel5
            // 
            this.cLabel5.AutoSize = true;
            this.cLabel5.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel5.ForeColor = System.Drawing.Color.Black;
            this.cLabel5.Location = new System.Drawing.Point(205, 18);
            this.cLabel5.Name = "cLabel5";
            this.cLabel5.Size = new System.Drawing.Size(104, 14);
            this.cLabel5.TabIndex = 9;
            this.cLabel5.Text = "Copy To Month";
            this.cLabel5.ToolTips = "";
            // 
            // cLabel6
            // 
            this.cLabel6.AutoSize = true;
            this.cLabel6.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel6.ForeColor = System.Drawing.Color.Black;
            this.cLabel6.Location = new System.Drawing.Point(6, 18);
            this.cLabel6.Name = "cLabel6";
            this.cLabel6.Size = new System.Drawing.Size(96, 14);
            this.cLabel6.TabIndex = 7;
            this.cLabel6.Text = "Copy To Year";
            this.cLabel6.ToolTips = "";
            // 
            // txtCopyToYear
            // 
            this.txtCopyToYear.ActivationColor = true;
            this.txtCopyToYear.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtCopyToYear.AllowTabKeyOnEnter = false;
            this.txtCopyToYear.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCopyToYear.Font = new System.Drawing.Font("Verdana", 9.75F);
            this.txtCopyToYear.Format = "######";
            this.txtCopyToYear.IsComplusory = false;
            this.txtCopyToYear.Location = new System.Drawing.Point(104, 14);
            this.txtCopyToYear.MaxLength = 4;
            this.txtCopyToYear.Name = "txtCopyToYear";
            this.txtCopyToYear.SelectAllTextOnFocus = true;
            this.txtCopyToYear.Size = new System.Drawing.Size(90, 23);
            this.txtCopyToYear.TabIndex = 1;
            this.txtCopyToYear.Text = "0";
            this.txtCopyToYear.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtCopyToYear.ToolTips = "";
            this.txtCopyToYear.WaterMarkText = null;
            // 
            // BtnCopy
            // 
            this.BtnCopy.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnCopy.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnCopy.Appearance.Options.UseFont = true;
            this.BtnCopy.Appearance.Options.UseForeColor = true;
            this.BtnCopy.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnCopy.ImageOptions.SvgImage")));
            this.BtnCopy.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnCopy.Location = new System.Drawing.Point(391, 8);
            this.BtnCopy.Name = "BtnCopy";
            this.BtnCopy.Size = new System.Drawing.Size(103, 32);
            this.BtnCopy.TabIndex = 3;
            this.BtnCopy.Text = "Copy To";
            this.BtnCopy.Click += new System.EventHandler(this.BtnCopy_Click);
            // 
            // FrmTargetCreateMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1137, 492);
            this.Controls.Add(this.MainGrid);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "FrmTargetCreateMaster";
            this.Text = "TARGET CREATE MASTER";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtRemark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CmbDollarType)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtShapeName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtCutName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtPolName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtSymName)).EndInit();
            this.panel1.ResumeLayout(false);
            this.PnlCopyPaste.ResumeLayout(false);
            this.PnlCopyPaste.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private AxonContLib.cPanel panel2;
		private DevControlLib.cDevSimpleButton BtnShow;
		private AxonContLib.cTextBox txtYear;
		private AxonContLib.cLabel cLabel2;
		private AxonContLib.cLabel cLabel1;
		private DevControlLib.cDevSimpleButton BtnExport;
		private DevControlLib.cDevSimpleButton BtnSave;
		private DevControlLib.cDevSimpleButton BtnAdd;
		private DevControlLib.cDevSimpleButton BtnBack;
		private DevExpress.XtraGrid.GridControl MainGrid;
		private DevExpress.XtraGrid.Views.Grid.GridView GrdDet;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
		private DevExpress.XtraEditors.Repository.RepositoryItemComboBox CmbDollarType;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
		private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repTxtRemark;
		private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repTxtShapeName;
		private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repTxtCutName;
		private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repTxtPolName;
		private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repTxtSymName;
		private AxonContLib.cPanel panel1;
		private AxonContLib.cComboBox CmbMonth;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
		private DevControlLib.cDevSimpleButton BtnLeft;
		private AxonContLib.cPanel PnlCopyPaste;
		private AxonContLib.cComboBox CmbCopyToMonth;
		private AxonContLib.cLabel cLabel5;
		private AxonContLib.cLabel cLabel6;
		private AxonContLib.cTextBox txtCopyToYear;
		private DevControlLib.cDevSimpleButton BtnCopy;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectedAmountToolStripMenuItem;
	}
}