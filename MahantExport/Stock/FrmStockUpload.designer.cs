namespace MahantExport.Stock
{
    partial class FrmStockUpload
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
            DevExpress.XtraGrid.GridLevelNode gridLevelNode1 = new DevExpress.XtraGrid.GridLevelNode();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmStockUpload));
            DevExpress.XtraGrid.GridLevelNode gridLevelNode2 = new DevExpress.XtraGrid.GridLevelNode();
            this.MainGrdStock = new DevExpress.XtraGrid.GridControl();
            this.GrdDetStock = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.PnlHeader = new AxonContLib.cPanel(this.components);
            this.CmbSheetName = new AxonContLib.cComboBox(this.components);
            this.cmbBillType = new AxonContLib.cComboBox(this.components);
            this.lblBillType = new AxonContLib.cLabel(this.components);
            this.lblTime = new AxonContLib.cLabel(this.components);
            this.cLabel41 = new AxonContLib.cLabel(this.components);
            this.cmbPrdType = new AxonContLib.cComboBox(this.components);
            this.BtnLeft = new DevControlLib.cDevSimpleButton(this.components);
            this.GrpRFIDBox = new DevControlLib.cDevGroupControl(this.components);
            this.BtnFindAndConnect = new DevControlLib.cDevSimpleButton(this.components);
            this.lblDeviceName = new AxonContLib.cLabel(this.components);
            this.cLabel39 = new AxonContLib.cLabel(this.components);
            this.BtnStop = new DevControlLib.cDevSimpleButton(this.components);
            this.lblTotal = new AxonContLib.cLabel(this.components);
            this.lblUnMatched = new AxonContLib.cLabel(this.components);
            this.lblMatched = new AxonContLib.cLabel(this.components);
            this.cLabel37 = new AxonContLib.cLabel(this.components);
            this.cLabel36 = new AxonContLib.cLabel(this.components);
            this.cLabel33 = new AxonContLib.cLabel(this.components);
            this.cLabel31 = new AxonContLib.cLabel(this.components);
            this.BtnLEDAllAtOnce = new DevControlLib.cDevSimpleButton(this.components);
            this.cLabel30 = new AxonContLib.cLabel(this.components);
            this.BtnScan = new DevControlLib.cDevSimpleButton(this.components);
            this.GrpStockSync = new DevControlLib.cDevGroupControl(this.components);
            this.RbtGIAControlNo = new AxonContLib.cRadioButton(this.components);
            this.RbtJangedNo = new AxonContLib.cRadioButton(this.components);
            this.RbtStoneNo = new AxonContLib.cRadioButton(this.components);
            this.DTPSyncToDate = new AxonContLib.cDateTimePicker(this.components);
            this.DTPSyncFromDate = new AxonContLib.cDateTimePicker(this.components);
            this.txtJangedNo = new AxonContLib.cTextBox(this.components);
            this.BtnVerified = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnStockSync = new DevControlLib.cDevSimpleButton(this.components);
            this.cLabel10 = new AxonContLib.cLabel(this.components);
            this.cLabel9 = new AxonContLib.cLabel(this.components);
            this.cLabel8 = new AxonContLib.cLabel(this.components);
            this.GrpExcRateDetail = new DevControlLib.cDevGroupControl(this.components);
            this.cLabel7 = new AxonContLib.cLabel(this.components);
            this.txtExcRate = new AxonContLib.cTextBox(this.components);
            this.cLabel34 = new AxonContLib.cLabel(this.components);
            this.cLabel35 = new AxonContLib.cLabel(this.components);
            this.CmbCurrency = new AxonContLib.cComboBox(this.components);
            this.cLabel6 = new AxonContLib.cLabel(this.components);
            this.CmbStockStatus = new System.Windows.Forms.ComboBox();
            this.RbtAppendStock = new AxonContLib.cRadioButton(this.components);
            this.RbtReplaceAllStock = new AxonContLib.cRadioButton(this.components);
            this.cLabel4 = new AxonContLib.cLabel(this.components);
            this.lblSampleExcelFile = new AxonContLib.cLabel(this.components);
            this.PnlFooter = new AxonContLib.cPanel(this.components);
            this.panel1 = new AxonContLib.cPanel(this.components);
            this.progressPanel1 = new DevExpress.XtraWaitForm.ProgressPanel();
            this.BtnCalculate = new DevControlLib.cDevSimpleButton(this.components);
            this.lblMessage = new AxonContLib.cLabel(this.components);
            this.BtnBestFit = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnBack = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnAdd = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnDelStonePricing = new DevControlLib.cDevSimpleButton(this.components);
            this.cLabel3 = new AxonContLib.cLabel(this.components);
            this.cLabel2 = new AxonContLib.cLabel(this.components);
            this.BtnBrowse = new DevControlLib.cDevSimpleButton(this.components);
            this.txtFileName = new AxonContLib.cTextBox(this.components);
            this.txtPartyName = new AxonContLib.cTextBox(this.components);
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            this.cLabel5 = new AxonContLib.cLabel(this.components);
            this.CmbStockType = new System.Windows.Forms.ComboBox();
            this.lblDefaultLayout = new AxonContLib.cLabel(this.components);
            this.lblSaveLayout = new AxonContLib.cLabel(this.components);
            this.cLabel28 = new AxonContLib.cLabel(this.components);
            this.BtnClearFilter = new DevControlLib.cDevSimpleButton(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel2 = new AxonContLib.cPanel(this.components);
            this.cLabel29 = new AxonContLib.cLabel(this.components);
            this.lblPurchaseReturn = new AxonContLib.cLabel(this.components);
            this.cLabel27 = new AxonContLib.cLabel(this.components);
            this.cLabel40 = new AxonContLib.cLabel(this.components);
            this.cLabel38 = new AxonContLib.cLabel(this.components);
            this.cLabel11 = new AxonContLib.cLabel(this.components);
            this.cLabel32 = new AxonContLib.cLabel(this.components);
            this.lblNone = new AxonContLib.cLabel(this.components);
            this.lblInvoice = new AxonContLib.cLabel(this.components);
            this.lblSold = new AxonContLib.cLabel(this.components);
            this.lblMemo = new AxonContLib.cLabel(this.components);
            this.lblAvailable = new AxonContLib.cLabel(this.components);
            this.txtSelectedAvgRap = new AxonContLib.cTextBox(this.components);
            this.cLabel19 = new AxonContLib.cLabel(this.components);
            this.cLabel13 = new AxonContLib.cLabel(this.components);
            this.cLabel26 = new AxonContLib.cLabel(this.components);
            this.cLabel24 = new AxonContLib.cLabel(this.components);
            this.txtSelectedCarat = new AxonContLib.cTextBox(this.components);
            this.cLabel14 = new AxonContLib.cLabel(this.components);
            this.txtSelectedDisc = new AxonContLib.cTextBox(this.components);
            this.cLabel20 = new AxonContLib.cLabel(this.components);
            this.txtSelectedPricePerCarat = new AxonContLib.cTextBox(this.components);
            this.txtSelectedAmount = new AxonContLib.cTextBox(this.components);
            this.txtTotalAvgRap = new AxonContLib.cTextBox(this.components);
            this.cLabel22 = new AxonContLib.cLabel(this.components);
            this.txtTotalCarat = new AxonContLib.cTextBox(this.components);
            this.cLabel25 = new AxonContLib.cLabel(this.components);
            this.txtTotalDisc = new AxonContLib.cTextBox(this.components);
            this.cLabel23 = new AxonContLib.cLabel(this.components);
            this.txtTotalPricePerCarat = new AxonContLib.cTextBox(this.components);
            this.cLabel21 = new AxonContLib.cLabel(this.components);
            this.txtTotalAmount = new AxonContLib.cTextBox(this.components);
            this.cLabel16 = new AxonContLib.cLabel(this.components);
            this.cLabel15 = new AxonContLib.cLabel(this.components);
            this.cLabel12 = new AxonContLib.cLabel(this.components);
            this.MainGrdDateWiseSum = new DevExpress.XtraGrid.GridControl();
            this.GrdDateWiseSum = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrdStock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDetStock)).BeginInit();
            this.PnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrpRFIDBox)).BeginInit();
            this.GrpRFIDBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrpStockSync)).BeginInit();
            this.GrpStockSync.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrpExcRateDetail)).BeginInit();
            this.GrpExcRateDetail.SuspendLayout();
            this.PnlFooter.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrdDateWiseSum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDateWiseSum)).BeginInit();
            this.SuspendLayout();
            // 
            // MainGrdStock
            // 
            this.MainGrdStock.Dock = System.Windows.Forms.DockStyle.Fill;
            gridLevelNode1.RelationName = "Level1";
            this.MainGrdStock.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode1});
            this.MainGrdStock.Location = new System.Drawing.Point(0, 17);
            this.MainGrdStock.MainView = this.GrdDetStock;
            this.MainGrdStock.Name = "MainGrdStock";
            this.MainGrdStock.Size = new System.Drawing.Size(1364, 465);
            this.MainGrdStock.TabIndex = 0;
            this.MainGrdStock.TabStop = false;
            this.MainGrdStock.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GrdDetStock});
            // 
            // GrdDetStock
            // 
            this.GrdDetStock.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.GrdDetStock.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.GrdDetStock.Appearance.FocusedCell.Options.UseBackColor = true;
            this.GrdDetStock.Appearance.FooterPanel.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.GrdDetStock.Appearance.FooterPanel.Options.UseFont = true;
            this.GrdDetStock.Appearance.GroupFooter.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.GrdDetStock.Appearance.GroupFooter.Options.UseFont = true;
            this.GrdDetStock.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.GrdDetStock.Appearance.HeaderPanel.Options.UseFont = true;
            this.GrdDetStock.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.GrdDetStock.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDetStock.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.GrdDetStock.Appearance.HorzLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDetStock.Appearance.HorzLine.Options.UseBackColor = true;
            this.GrdDetStock.Appearance.Row.Font = new System.Drawing.Font("Verdana", 8.5F);
            this.GrdDetStock.Appearance.Row.Options.UseFont = true;
            this.GrdDetStock.Appearance.Row.Options.UseTextOptions = true;
            this.GrdDetStock.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDetStock.Appearance.VertLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDetStock.Appearance.VertLine.Options.UseBackColor = true;
            this.GrdDetStock.AppearancePrint.EvenRow.BackColor = System.Drawing.Color.Transparent;
            this.GrdDetStock.AppearancePrint.EvenRow.Options.UseBackColor = true;
            this.GrdDetStock.AppearancePrint.FooterPanel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.GrdDetStock.AppearancePrint.FooterPanel.Options.UseFont = true;
            this.GrdDetStock.AppearancePrint.HeaderPanel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.GrdDetStock.AppearancePrint.HeaderPanel.Options.UseFont = true;
            this.GrdDetStock.AppearancePrint.OddRow.BackColor = System.Drawing.Color.Transparent;
            this.GrdDetStock.AppearancePrint.OddRow.Options.UseBackColor = true;
            this.GrdDetStock.AppearancePrint.Row.BackColor = System.Drawing.Color.Transparent;
            this.GrdDetStock.AppearancePrint.Row.Options.UseBackColor = true;
            this.GrdDetStock.ColumnPanelRowHeight = 40;
            this.GrdDetStock.GridControl = this.MainGrdStock;
            this.GrdDetStock.Name = "GrdDetStock";
            this.GrdDetStock.OptionsBehavior.Editable = false;
            this.GrdDetStock.OptionsFilter.AllowFilterEditor = false;
            this.GrdDetStock.OptionsNavigation.EnterMoveNextColumn = true;
            this.GrdDetStock.OptionsPrint.EnableAppearanceEvenRow = true;
            this.GrdDetStock.OptionsPrint.EnableAppearanceOddRow = true;
            this.GrdDetStock.OptionsPrint.ExpandAllGroups = false;
            this.GrdDetStock.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.GrdDetStock.OptionsView.ColumnAutoWidth = false;
            this.GrdDetStock.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.GrdDetStock.OptionsView.ShowAutoFilterRow = true;
            this.GrdDetStock.OptionsView.ShowFooter = true;
            this.GrdDetStock.OptionsView.ShowGroupPanel = false;
            // 
            // PnlHeader
            // 
            this.PnlHeader.Controls.Add(this.CmbSheetName);
            this.PnlHeader.Controls.Add(this.cmbBillType);
            this.PnlHeader.Controls.Add(this.lblBillType);
            this.PnlHeader.Controls.Add(this.lblTime);
            this.PnlHeader.Controls.Add(this.cLabel41);
            this.PnlHeader.Controls.Add(this.cmbPrdType);
            this.PnlHeader.Controls.Add(this.BtnLeft);
            this.PnlHeader.Controls.Add(this.GrpRFIDBox);
            this.PnlHeader.Controls.Add(this.GrpStockSync);
            this.PnlHeader.Controls.Add(this.GrpExcRateDetail);
            this.PnlHeader.Controls.Add(this.cLabel6);
            this.PnlHeader.Controls.Add(this.CmbStockStatus);
            this.PnlHeader.Controls.Add(this.RbtAppendStock);
            this.PnlHeader.Controls.Add(this.RbtReplaceAllStock);
            this.PnlHeader.Controls.Add(this.cLabel4);
            this.PnlHeader.Controls.Add(this.lblSampleExcelFile);
            this.PnlHeader.Controls.Add(this.PnlFooter);
            this.PnlHeader.Controls.Add(this.cLabel3);
            this.PnlHeader.Controls.Add(this.cLabel2);
            this.PnlHeader.Controls.Add(this.BtnBrowse);
            this.PnlHeader.Controls.Add(this.txtFileName);
            this.PnlHeader.Controls.Add(this.txtPartyName);
            this.PnlHeader.Controls.Add(this.cLabel1);
            this.PnlHeader.Controls.Add(this.cLabel5);
            this.PnlHeader.Controls.Add(this.CmbStockType);
            this.PnlHeader.Controls.Add(this.lblDefaultLayout);
            this.PnlHeader.Controls.Add(this.lblSaveLayout);
            this.PnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.PnlHeader.Location = new System.Drawing.Point(0, 0);
            this.PnlHeader.Name = "PnlHeader";
            this.PnlHeader.Size = new System.Drawing.Size(1366, 129);
            this.PnlHeader.TabIndex = 0;
            // 
            // CmbSheetName
            // 
            this.CmbSheetName.AllowTabKeyOnEnter = false;
            this.CmbSheetName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbSheetName.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.CmbSheetName.ForeColor = System.Drawing.Color.Black;
            this.CmbSheetName.FormattingEnabled = true;
            this.CmbSheetName.Items.AddRange(new object[] {
            "PARCEL"});
            this.CmbSheetName.Location = new System.Drawing.Point(341, 42);
            this.CmbSheetName.Name = "CmbSheetName";
            this.CmbSheetName.Size = new System.Drawing.Size(188, 22);
            this.CmbSheetName.TabIndex = 195;
            this.CmbSheetName.ToolTips = "";
            // 
            // cmbBillType
            // 
            this.cmbBillType.AllowTabKeyOnEnter = false;
            this.cmbBillType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBillType.Font = new System.Drawing.Font("Verdana", 8F);
            this.cmbBillType.ForeColor = System.Drawing.Color.Black;
            this.cmbBillType.FormattingEnabled = true;
            this.cmbBillType.Items.AddRange(new object[] {
            "None",
            "RupeesBill",
            "DollarBill",
            "Cash",
            "CashPF",
            "Export",
            "Consignment"});
            this.cmbBillType.Location = new System.Drawing.Point(641, 41);
            this.cmbBillType.Name = "cmbBillType";
            this.cmbBillType.Size = new System.Drawing.Size(150, 21);
            this.cmbBillType.TabIndex = 193;
            this.cmbBillType.ToolTips = "";
            this.cmbBillType.SelectedIndexChanged += new System.EventHandler(this.cmbBillType_SelectedIndexChanged);
            // 
            // lblBillType
            // 
            this.lblBillType.AutoSize = true;
            this.lblBillType.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBillType.ForeColor = System.Drawing.Color.Black;
            this.lblBillType.Location = new System.Drawing.Point(573, 45);
            this.lblBillType.Name = "lblBillType";
            this.lblBillType.Size = new System.Drawing.Size(63, 13);
            this.lblBillType.TabIndex = 194;
            this.lblBillType.Text = "Bill Type";
            this.lblBillType.ToolTips = "";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.lblTime.ForeColor = System.Drawing.Color.Purple;
            this.lblTime.Location = new System.Drawing.Point(801, 44);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(71, 14);
            this.lblTime.TabIndex = 192;
            this.lblTime.Text = "00:00:00";
            this.lblTime.ToolTips = "";
            // 
            // cLabel41
            // 
            this.cLabel41.AutoSize = true;
            this.cLabel41.Font = new System.Drawing.Font("Verdana", 9F);
            this.cLabel41.ForeColor = System.Drawing.Color.Black;
            this.cLabel41.Location = new System.Drawing.Point(546, 19);
            this.cLabel41.Name = "cLabel41";
            this.cLabel41.Size = new System.Drawing.Size(89, 14);
            this.cLabel41.TabIndex = 191;
            this.cLabel41.Text = "Grading Type";
            this.cLabel41.ToolTips = "";
            // 
            // cmbPrdType
            // 
            this.cmbPrdType.AllowTabKeyOnEnter = false;
            this.cmbPrdType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrdType.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbPrdType.ForeColor = System.Drawing.Color.Black;
            this.cmbPrdType.FormattingEnabled = true;
            this.cmbPrdType.Items.AddRange(new object[] {
            "None"});
            this.cmbPrdType.Location = new System.Drawing.Point(641, 15);
            this.cmbPrdType.Name = "cmbPrdType";
            this.cmbPrdType.Size = new System.Drawing.Size(188, 21);
            this.cmbPrdType.TabIndex = 190;
            this.cmbPrdType.ToolTips = "";
            // 
            // BtnLeft
            // 
            this.BtnLeft.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnLeft.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnLeft.Appearance.Options.UseFont = true;
            this.BtnLeft.Appearance.Options.UseForeColor = true;
            this.BtnLeft.ButtonStyle = DevExpress.XtraEditors.Controls.BorderStyles.Office2003;
            this.BtnLeft.ImageOptions.Image = global::MahantExport.Properties.Resources.A1;
            this.BtnLeft.Location = new System.Drawing.Point(1021, 0);
            this.BtnLeft.Name = "BtnLeft";
            this.BtnLeft.Size = new System.Drawing.Size(33, 91);
            this.BtnLeft.TabIndex = 189;
            this.BtnLeft.TabStop = false;
            this.BtnLeft.Visible = false;
            this.BtnLeft.Click += new System.EventHandler(this.BtnLeft_Click);
            // 
            // GrpRFIDBox
            // 
            this.GrpRFIDBox.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(234)))), ((int)(((byte)(221)))));
            this.GrpRFIDBox.Appearance.Options.UseBackColor = true;
            this.GrpRFIDBox.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.GrpRFIDBox.Controls.Add(this.BtnFindAndConnect);
            this.GrpRFIDBox.Controls.Add(this.lblDeviceName);
            this.GrpRFIDBox.Controls.Add(this.cLabel39);
            this.GrpRFIDBox.Controls.Add(this.BtnStop);
            this.GrpRFIDBox.Controls.Add(this.lblTotal);
            this.GrpRFIDBox.Controls.Add(this.lblUnMatched);
            this.GrpRFIDBox.Controls.Add(this.lblMatched);
            this.GrpRFIDBox.Controls.Add(this.cLabel37);
            this.GrpRFIDBox.Controls.Add(this.cLabel36);
            this.GrpRFIDBox.Controls.Add(this.cLabel33);
            this.GrpRFIDBox.Controls.Add(this.cLabel31);
            this.GrpRFIDBox.Controls.Add(this.BtnLEDAllAtOnce);
            this.GrpRFIDBox.Controls.Add(this.cLabel30);
            this.GrpRFIDBox.Controls.Add(this.BtnScan);
            this.GrpRFIDBox.Location = new System.Drawing.Point(1054, 1);
            this.GrpRFIDBox.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.GrpRFIDBox.LookAndFeel.UseDefaultLookAndFeel = false;
            this.GrpRFIDBox.Name = "GrpRFIDBox";
            this.GrpRFIDBox.ShowCaption = false;
            this.GrpRFIDBox.Size = new System.Drawing.Size(232, 91);
            this.GrpRFIDBox.TabIndex = 188;
            this.GrpRFIDBox.Visible = false;
            // 
            // BtnFindAndConnect
            // 
            this.BtnFindAndConnect.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnFindAndConnect.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnFindAndConnect.Appearance.Options.UseFont = true;
            this.BtnFindAndConnect.Appearance.Options.UseForeColor = true;
            this.BtnFindAndConnect.Location = new System.Drawing.Point(6, 15);
            this.BtnFindAndConnect.Name = "BtnFindAndConnect";
            this.BtnFindAndConnect.Size = new System.Drawing.Size(10, 10);
            this.BtnFindAndConnect.TabIndex = 228;
            this.BtnFindAndConnect.TabStop = false;
            this.BtnFindAndConnect.Text = "Find && Connect";
            this.BtnFindAndConnect.Visible = false;
            this.BtnFindAndConnect.Click += new System.EventHandler(this.BtnFindAndConnect_Click);
            // 
            // lblDeviceName
            // 
            this.lblDeviceName.AutoSize = true;
            this.lblDeviceName.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDeviceName.ForeColor = System.Drawing.Color.Black;
            this.lblDeviceName.Location = new System.Drawing.Point(83, 18);
            this.lblDeviceName.Name = "lblDeviceName";
            this.lblDeviceName.Size = new System.Drawing.Size(0, 13);
            this.lblDeviceName.TabIndex = 227;
            this.lblDeviceName.ToolTips = "";
            // 
            // cLabel39
            // 
            this.cLabel39.AutoSize = true;
            this.cLabel39.Font = new System.Drawing.Font("Verdana", 8F);
            this.cLabel39.ForeColor = System.Drawing.Color.Black;
            this.cLabel39.Location = new System.Drawing.Point(1, 1);
            this.cLabel39.Name = "cLabel39";
            this.cLabel39.Size = new System.Drawing.Size(225, 13);
            this.cLabel39.TabIndex = 225;
            this.cLabel39.Text = "-------------: RFID Detail :--------------";
            this.cLabel39.ToolTips = "";
            // 
            // BtnStop
            // 
            this.BtnStop.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnStop.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnStop.Appearance.Options.UseFont = true;
            this.BtnStop.Appearance.Options.UseForeColor = true;
            this.BtnStop.Enabled = false;
            this.BtnStop.Location = new System.Drawing.Point(7, 42);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(72, 20);
            this.BtnStop.TabIndex = 224;
            this.BtnStop.TabStop = false;
            this.BtnStop.Text = "Stop";
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // lblTotal
            // 
            this.lblTotal.AutoSize = true;
            this.lblTotal.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotal.ForeColor = System.Drawing.Color.Black;
            this.lblTotal.Location = new System.Drawing.Point(175, 73);
            this.lblTotal.Name = "lblTotal";
            this.lblTotal.Size = new System.Drawing.Size(15, 13);
            this.lblTotal.TabIndex = 222;
            this.lblTotal.Text = "0";
            this.lblTotal.ToolTips = "";
            // 
            // lblUnMatched
            // 
            this.lblUnMatched.AutoSize = true;
            this.lblUnMatched.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUnMatched.ForeColor = System.Drawing.Color.Black;
            this.lblUnMatched.Location = new System.Drawing.Point(175, 51);
            this.lblUnMatched.Name = "lblUnMatched";
            this.lblUnMatched.Size = new System.Drawing.Size(14, 13);
            this.lblUnMatched.TabIndex = 221;
            this.lblUnMatched.Text = "0";
            this.lblUnMatched.ToolTips = "";
            // 
            // lblMatched
            // 
            this.lblMatched.AutoSize = true;
            this.lblMatched.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMatched.ForeColor = System.Drawing.Color.Black;
            this.lblMatched.Location = new System.Drawing.Point(175, 36);
            this.lblMatched.Name = "lblMatched";
            this.lblMatched.Size = new System.Drawing.Size(14, 13);
            this.lblMatched.TabIndex = 220;
            this.lblMatched.Text = "0";
            this.lblMatched.ToolTips = "";
            // 
            // cLabel37
            // 
            this.cLabel37.AutoSize = true;
            this.cLabel37.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel37.ForeColor = System.Drawing.Color.Black;
            this.cLabel37.Location = new System.Drawing.Point(83, 73);
            this.cLabel37.Name = "cLabel37";
            this.cLabel37.Size = new System.Drawing.Size(40, 13);
            this.cLabel37.TabIndex = 219;
            this.cLabel37.Text = "Total";
            this.cLabel37.ToolTips = "";
            // 
            // cLabel36
            // 
            this.cLabel36.AutoSize = true;
            this.cLabel36.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel36.ForeColor = System.Drawing.Color.Black;
            this.cLabel36.Location = new System.Drawing.Point(83, 62);
            this.cLabel36.Name = "cLabel36";
            this.cLabel36.Size = new System.Drawing.Size(145, 13);
            this.cLabel36.TabIndex = 218;
            this.cLabel36.Text = "-----------------------";
            this.cLabel36.ToolTips = "";
            // 
            // cLabel33
            // 
            this.cLabel33.AutoSize = true;
            this.cLabel33.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel33.ForeColor = System.Drawing.Color.Black;
            this.cLabel33.Location = new System.Drawing.Point(83, 51);
            this.cLabel33.Name = "cLabel33";
            this.cLabel33.Size = new System.Drawing.Size(71, 13);
            this.cLabel33.TabIndex = 217;
            this.cLabel33.Text = "Unmatched";
            this.cLabel33.ToolTips = "";
            // 
            // cLabel31
            // 
            this.cLabel31.AutoSize = true;
            this.cLabel31.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel31.ForeColor = System.Drawing.Color.Black;
            this.cLabel31.Location = new System.Drawing.Point(83, 36);
            this.cLabel31.Name = "cLabel31";
            this.cLabel31.Size = new System.Drawing.Size(54, 13);
            this.cLabel31.TabIndex = 215;
            this.cLabel31.Text = "Matched";
            this.cLabel31.ToolTips = "";
            // 
            // BtnLEDAllAtOnce
            // 
            this.BtnLEDAllAtOnce.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnLEDAllAtOnce.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnLEDAllAtOnce.Appearance.Options.UseFont = true;
            this.BtnLEDAllAtOnce.Appearance.Options.UseForeColor = true;
            this.BtnLEDAllAtOnce.Enabled = false;
            this.BtnLEDAllAtOnce.Location = new System.Drawing.Point(8, 65);
            this.BtnLEDAllAtOnce.Name = "BtnLEDAllAtOnce";
            this.BtnLEDAllAtOnce.Size = new System.Drawing.Size(72, 20);
            this.BtnLEDAllAtOnce.TabIndex = 216;
            this.BtnLEDAllAtOnce.TabStop = false;
            this.BtnLEDAllAtOnce.Text = "Start Led";
            this.BtnLEDAllAtOnce.Click += new System.EventHandler(this.BtnLEDAllAtOnce_Click);
            // 
            // cLabel30
            // 
            this.cLabel30.AutoSize = true;
            this.cLabel30.Font = new System.Drawing.Font("Verdana", 8F);
            this.cLabel30.ForeColor = System.Drawing.Color.Black;
            this.cLabel30.Location = new System.Drawing.Point(44, -15);
            this.cLabel30.Name = "cLabel30";
            this.cLabel30.Size = new System.Drawing.Size(113, 13);
            this.cLabel30.TabIndex = 206;
            this.cLabel30.Text = "------: RFID :------";
            this.cLabel30.ToolTips = "";
            // 
            // BtnScan
            // 
            this.BtnScan.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnScan.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnScan.Appearance.Options.UseFont = true;
            this.BtnScan.Appearance.Options.UseForeColor = true;
            this.BtnScan.Enabled = false;
            this.BtnScan.Location = new System.Drawing.Point(7, 18);
            this.BtnScan.Name = "BtnScan";
            this.BtnScan.Size = new System.Drawing.Size(72, 20);
            this.BtnScan.TabIndex = 215;
            this.BtnScan.TabStop = false;
            this.BtnScan.Text = "Scan";
            this.BtnScan.Click += new System.EventHandler(this.BtnScan_Click);
            // 
            // GrpStockSync
            // 
            this.GrpStockSync.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(234)))), ((int)(((byte)(221)))));
            this.GrpStockSync.Appearance.Options.UseBackColor = true;
            this.GrpStockSync.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.GrpStockSync.Controls.Add(this.RbtGIAControlNo);
            this.GrpStockSync.Controls.Add(this.RbtJangedNo);
            this.GrpStockSync.Controls.Add(this.RbtStoneNo);
            this.GrpStockSync.Controls.Add(this.DTPSyncToDate);
            this.GrpStockSync.Controls.Add(this.DTPSyncFromDate);
            this.GrpStockSync.Controls.Add(this.txtJangedNo);
            this.GrpStockSync.Controls.Add(this.BtnVerified);
            this.GrpStockSync.Controls.Add(this.BtnStockSync);
            this.GrpStockSync.Controls.Add(this.cLabel10);
            this.GrpStockSync.Controls.Add(this.cLabel9);
            this.GrpStockSync.Controls.Add(this.cLabel8);
            this.GrpStockSync.Location = new System.Drawing.Point(1282, 0);
            this.GrpStockSync.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.GrpStockSync.LookAndFeel.UseDefaultLookAndFeel = false;
            this.GrpStockSync.Name = "GrpStockSync";
            this.GrpStockSync.ShowCaption = false;
            this.GrpStockSync.Size = new System.Drawing.Size(10, 91);
            this.GrpStockSync.TabIndex = 187;
            this.GrpStockSync.Visible = false;
            // 
            // RbtGIAControlNo
            // 
            this.RbtGIAControlNo.AllowTabKeyOnEnter = false;
            this.RbtGIAControlNo.AutoSize = true;
            this.RbtGIAControlNo.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbtGIAControlNo.ForeColor = System.Drawing.Color.Black;
            this.RbtGIAControlNo.Location = new System.Drawing.Point(361, 3);
            this.RbtGIAControlNo.Name = "RbtGIAControlNo";
            this.RbtGIAControlNo.Size = new System.Drawing.Size(121, 17);
            this.RbtGIAControlNo.TabIndex = 215;
            this.RbtGIAControlNo.Text = "GIA Control No";
            this.RbtGIAControlNo.ToolTips = "";
            this.RbtGIAControlNo.UseVisualStyleBackColor = true;
            // 
            // RbtJangedNo
            // 
            this.RbtJangedNo.AllowTabKeyOnEnter = false;
            this.RbtJangedNo.AutoSize = true;
            this.RbtJangedNo.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbtJangedNo.ForeColor = System.Drawing.Color.Black;
            this.RbtJangedNo.Location = new System.Drawing.Point(270, 3);
            this.RbtJangedNo.Name = "RbtJangedNo";
            this.RbtJangedNo.Size = new System.Drawing.Size(92, 17);
            this.RbtJangedNo.TabIndex = 214;
            this.RbtJangedNo.Text = "Janged No";
            this.RbtJangedNo.ToolTips = "";
            this.RbtJangedNo.UseVisualStyleBackColor = true;
            // 
            // RbtStoneNo
            // 
            this.RbtStoneNo.AllowTabKeyOnEnter = false;
            this.RbtStoneNo.AutoSize = true;
            this.RbtStoneNo.Checked = true;
            this.RbtStoneNo.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbtStoneNo.ForeColor = System.Drawing.Color.Black;
            this.RbtStoneNo.Location = new System.Drawing.Point(186, 3);
            this.RbtStoneNo.Name = "RbtStoneNo";
            this.RbtStoneNo.Size = new System.Drawing.Size(83, 17);
            this.RbtStoneNo.TabIndex = 213;
            this.RbtStoneNo.TabStop = true;
            this.RbtStoneNo.Text = "Stone No";
            this.RbtStoneNo.ToolTips = "";
            this.RbtStoneNo.UseVisualStyleBackColor = true;
            // 
            // DTPSyncToDate
            // 
            this.DTPSyncToDate.AllowTabKeyOnEnter = false;
            this.DTPSyncToDate.Checked = false;
            this.DTPSyncToDate.CustomFormat = "dd/MM/yyyy";
            this.DTPSyncToDate.Font = new System.Drawing.Font("Verdana", 8F);
            this.DTPSyncToDate.ForeColor = System.Drawing.Color.Black;
            this.DTPSyncToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTPSyncToDate.Location = new System.Drawing.Point(45, 44);
            this.DTPSyncToDate.Name = "DTPSyncToDate";
            this.DTPSyncToDate.ShowCheckBox = true;
            this.DTPSyncToDate.Size = new System.Drawing.Size(138, 20);
            this.DTPSyncToDate.TabIndex = 15;
            this.DTPSyncToDate.ToolTips = "";
            // 
            // DTPSyncFromDate
            // 
            this.DTPSyncFromDate.AllowTabKeyOnEnter = false;
            this.DTPSyncFromDate.Checked = false;
            this.DTPSyncFromDate.CustomFormat = "dd/MM/yyyy";
            this.DTPSyncFromDate.Font = new System.Drawing.Font("Verdana", 8F);
            this.DTPSyncFromDate.ForeColor = System.Drawing.Color.Black;
            this.DTPSyncFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTPSyncFromDate.Location = new System.Drawing.Point(45, 21);
            this.DTPSyncFromDate.Name = "DTPSyncFromDate";
            this.DTPSyncFromDate.ShowCheckBox = true;
            this.DTPSyncFromDate.Size = new System.Drawing.Size(138, 20);
            this.DTPSyncFromDate.TabIndex = 14;
            this.DTPSyncFromDate.ToolTips = "";
            // 
            // txtJangedNo
            // 
            this.txtJangedNo.ActivationColor = true;
            this.txtJangedNo.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtJangedNo.AllowTabKeyOnEnter = false;
            this.txtJangedNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJangedNo.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtJangedNo.Format = "";
            this.txtJangedNo.IsComplusory = false;
            this.txtJangedNo.Location = new System.Drawing.Point(189, 21);
            this.txtJangedNo.Multiline = true;
            this.txtJangedNo.Name = "txtJangedNo";
            this.txtJangedNo.SelectAllTextOnFocus = true;
            this.txtJangedNo.Size = new System.Drawing.Size(290, 65);
            this.txtJangedNo.TabIndex = 211;
            this.txtJangedNo.ToolTips = "";
            this.txtJangedNo.WaterMarkText = null;
            this.txtJangedNo.TextChanged += new System.EventHandler(this.txtJangedNo_TextChanged);
            // 
            // BtnVerified
            // 
            this.BtnVerified.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnVerified.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnVerified.Appearance.Options.UseFont = true;
            this.BtnVerified.Appearance.Options.UseForeColor = true;
            this.BtnVerified.Location = new System.Drawing.Point(96, 66);
            this.BtnVerified.Name = "BtnVerified";
            this.BtnVerified.Size = new System.Drawing.Size(88, 20);
            this.BtnVerified.TabIndex = 210;
            this.BtnVerified.TabStop = false;
            this.BtnVerified.Text = "Verified";
            this.BtnVerified.Click += new System.EventHandler(this.BtnVerified_Click);
            // 
            // BtnStockSync
            // 
            this.BtnStockSync.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnStockSync.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnStockSync.Appearance.Options.UseFont = true;
            this.BtnStockSync.Appearance.Options.UseForeColor = true;
            this.BtnStockSync.Location = new System.Drawing.Point(4, 66);
            this.BtnStockSync.Name = "BtnStockSync";
            this.BtnStockSync.Size = new System.Drawing.Size(89, 20);
            this.BtnStockSync.TabIndex = 185;
            this.BtnStockSync.TabStop = false;
            this.BtnStockSync.Text = "Stock Sync";
            this.BtnStockSync.Click += new System.EventHandler(this.BtnStockSync_Click);
            // 
            // cLabel10
            // 
            this.cLabel10.AutoSize = true;
            this.cLabel10.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.cLabel10.ForeColor = System.Drawing.Color.Black;
            this.cLabel10.Location = new System.Drawing.Point(4, 46);
            this.cLabel10.Name = "cLabel10";
            this.cLabel10.Size = new System.Drawing.Size(23, 13);
            this.cLabel10.TabIndex = 209;
            this.cLabel10.Text = "To";
            this.cLabel10.ToolTips = "";
            // 
            // cLabel9
            // 
            this.cLabel9.AutoSize = true;
            this.cLabel9.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.cLabel9.ForeColor = System.Drawing.Color.Black;
            this.cLabel9.Location = new System.Drawing.Point(4, 25);
            this.cLabel9.Name = "cLabel9";
            this.cLabel9.Size = new System.Drawing.Size(41, 13);
            this.cLabel9.TabIndex = 207;
            this.cLabel9.Text = "From";
            this.cLabel9.ToolTips = "";
            // 
            // cLabel8
            // 
            this.cLabel8.AutoSize = true;
            this.cLabel8.Font = new System.Drawing.Font("Verdana", 8F, ((System.Drawing.FontStyle)(((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic) 
                | System.Drawing.FontStyle.Underline))));
            this.cLabel8.ForeColor = System.Drawing.Color.Black;
            this.cLabel8.Location = new System.Drawing.Point(2, 2);
            this.cLabel8.Name = "cLabel8";
            this.cLabel8.Size = new System.Drawing.Size(182, 13);
            this.cLabel8.TabIndex = 206;
            this.cLabel8.Text = "--: Stock Sync From MFG :--";
            this.cLabel8.ToolTips = "";
            // 
            // GrpExcRateDetail
            // 
            this.GrpExcRateDetail.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(234)))), ((int)(((byte)(221)))));
            this.GrpExcRateDetail.Appearance.Options.UseBackColor = true;
            this.GrpExcRateDetail.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.GrpExcRateDetail.Controls.Add(this.cLabel7);
            this.GrpExcRateDetail.Controls.Add(this.txtExcRate);
            this.GrpExcRateDetail.Controls.Add(this.cLabel34);
            this.GrpExcRateDetail.Controls.Add(this.cLabel35);
            this.GrpExcRateDetail.Controls.Add(this.CmbCurrency);
            this.GrpExcRateDetail.Dock = System.Windows.Forms.DockStyle.Right;
            this.GrpExcRateDetail.Location = new System.Drawing.Point(1163, 0);
            this.GrpExcRateDetail.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            this.GrpExcRateDetail.LookAndFeel.UseDefaultLookAndFeel = false;
            this.GrpExcRateDetail.Name = "GrpExcRateDetail";
            this.GrpExcRateDetail.ShowCaption = false;
            this.GrpExcRateDetail.Size = new System.Drawing.Size(203, 91);
            this.GrpExcRateDetail.TabIndex = 41;
            // 
            // cLabel7
            // 
            this.cLabel7.AutoSize = true;
            this.cLabel7.Font = new System.Drawing.Font("Verdana", 8F);
            this.cLabel7.ForeColor = System.Drawing.Color.Black;
            this.cLabel7.Location = new System.Drawing.Point(15, 1);
            this.cLabel7.Name = "cLabel7";
            this.cLabel7.Size = new System.Drawing.Size(172, 13);
            this.cLabel7.TabIndex = 206;
            this.cLabel7.Text = "------: Exc Rate Detail :------";
            this.cLabel7.ToolTips = "";
            // 
            // txtExcRate
            // 
            this.txtExcRate.ActivationColor = true;
            this.txtExcRate.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtExcRate.AllowTabKeyOnEnter = false;
            this.txtExcRate.BackColor = System.Drawing.Color.AliceBlue;
            this.txtExcRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtExcRate.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtExcRate.Format = "############0.00";
            this.txtExcRate.IsComplusory = false;
            this.txtExcRate.Location = new System.Drawing.Point(70, 40);
            this.txtExcRate.MaxLength = 100;
            this.txtExcRate.Name = "txtExcRate";
            this.txtExcRate.SelectAllTextOnFocus = true;
            this.txtExcRate.Size = new System.Drawing.Size(80, 22);
            this.txtExcRate.TabIndex = 202;
            this.txtExcRate.Text = "7.00";
            this.txtExcRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtExcRate.ToolTips = "";
            this.txtExcRate.WaterMarkText = null;
            // 
            // cLabel34
            // 
            this.cLabel34.AutoSize = true;
            this.cLabel34.Font = new System.Drawing.Font("Verdana", 9F);
            this.cLabel34.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cLabel34.Location = new System.Drawing.Point(39, 44);
            this.cLabel34.Name = "cLabel34";
            this.cLabel34.Size = new System.Drawing.Size(28, 14);
            this.cLabel34.TabIndex = 204;
            this.cLabel34.Text = "Exc";
            this.cLabel34.ToolTips = "";
            // 
            // cLabel35
            // 
            this.cLabel35.AutoSize = true;
            this.cLabel35.Font = new System.Drawing.Font("Verdana", 9F);
            this.cLabel35.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cLabel35.Location = new System.Drawing.Point(6, 19);
            this.cLabel35.Name = "cLabel35";
            this.cLabel35.Size = new System.Drawing.Size(63, 14);
            this.cLabel35.TabIndex = 203;
            this.cLabel35.Text = "Currency";
            this.cLabel35.ToolTips = "";
            // 
            // CmbCurrency
            // 
            this.CmbCurrency.AllowTabKeyOnEnter = false;
            this.CmbCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbCurrency.Font = new System.Drawing.Font("Verdana", 9F);
            this.CmbCurrency.ForeColor = System.Drawing.Color.Black;
            this.CmbCurrency.FormattingEnabled = true;
            this.CmbCurrency.Location = new System.Drawing.Point(70, 15);
            this.CmbCurrency.Name = "CmbCurrency";
            this.CmbCurrency.Size = new System.Drawing.Size(128, 22);
            this.CmbCurrency.TabIndex = 6;
            this.CmbCurrency.ToolTips = "";
            this.CmbCurrency.SelectedIndexChanged += new System.EventHandler(this.CmbCurrency_SelectedIndexChanged);
            // 
            // cLabel6
            // 
            this.cLabel6.AutoSize = true;
            this.cLabel6.Font = new System.Drawing.Font("Verdana", 8F);
            this.cLabel6.ForeColor = System.Drawing.Color.Black;
            this.cLabel6.Location = new System.Drawing.Point(346, 2);
            this.cLabel6.Name = "cLabel6";
            this.cLabel6.Size = new System.Drawing.Size(91, 13);
            this.cLabel6.TabIndex = 184;
            this.cLabel6.Text = "---: Status :---";
            this.cLabel6.ToolTips = "";
            // 
            // CmbStockStatus
            // 
            this.CmbStockStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbStockStatus.Font = new System.Drawing.Font("Verdana", 9F);
            this.CmbStockStatus.FormattingEnabled = true;
            this.CmbStockStatus.Items.AddRange(new object[] {
            "NONE",
            "TRANSFER TO AVAILABLE",
            "PURCHASE"});
            this.CmbStockStatus.Location = new System.Drawing.Point(341, 15);
            this.CmbStockStatus.Name = "CmbStockStatus";
            this.CmbStockStatus.Size = new System.Drawing.Size(188, 22);
            this.CmbStockStatus.TabIndex = 6;
            this.CmbStockStatus.SelectedIndexChanged += new System.EventHandler(this.CmbStockStatus_SelectedIndexChanged);
            // 
            // RbtAppendStock
            // 
            this.RbtAppendStock.AllowTabKeyOnEnter = false;
            this.RbtAppendStock.AutoSize = true;
            this.RbtAppendStock.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbtAppendStock.ForeColor = System.Drawing.Color.Black;
            this.RbtAppendStock.Location = new System.Drawing.Point(684, 66);
            this.RbtAppendStock.Name = "RbtAppendStock";
            this.RbtAppendStock.Size = new System.Drawing.Size(146, 18);
            this.RbtAppendStock.TabIndex = 10;
            this.RbtAppendStock.Text = "Update / Edit Stock";
            this.RbtAppendStock.ToolTips = "";
            this.RbtAppendStock.UseVisualStyleBackColor = true;
            // 
            // RbtReplaceAllStock
            // 
            this.RbtReplaceAllStock.AllowTabKeyOnEnter = false;
            this.RbtReplaceAllStock.AutoSize = true;
            this.RbtReplaceAllStock.Checked = true;
            this.RbtReplaceAllStock.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbtReplaceAllStock.ForeColor = System.Drawing.Color.Black;
            this.RbtReplaceAllStock.Location = new System.Drawing.Point(549, 66);
            this.RbtReplaceAllStock.Name = "RbtReplaceAllStock";
            this.RbtReplaceAllStock.Size = new System.Drawing.Size(130, 18);
            this.RbtReplaceAllStock.TabIndex = 9;
            this.RbtReplaceAllStock.TabStop = true;
            this.RbtReplaceAllStock.Text = "Replace All Stock";
            this.RbtReplaceAllStock.ToolTips = "";
            this.RbtReplaceAllStock.UseVisualStyleBackColor = true;
            // 
            // cLabel4
            // 
            this.cLabel4.AutoSize = true;
            this.cLabel4.Font = new System.Drawing.Font("Verdana", 9F);
            this.cLabel4.ForeColor = System.Drawing.Color.Black;
            this.cLabel4.Location = new System.Drawing.Point(268, 19);
            this.cLabel4.Name = "cLabel4";
            this.cLabel4.Size = new System.Drawing.Size(41, 14);
            this.cLabel4.TabIndex = 181;
            this.cLabel4.Text = "Stock";
            this.cLabel4.ToolTips = "";
            // 
            // lblSampleExcelFile
            // 
            this.lblSampleExcelFile.AutoSize = true;
            this.lblSampleExcelFile.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblSampleExcelFile.Font = new System.Drawing.Font("Verdana", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.lblSampleExcelFile.ForeColor = System.Drawing.Color.Green;
            this.lblSampleExcelFile.Location = new System.Drawing.Point(7, 70);
            this.lblSampleExcelFile.Name = "lblSampleExcelFile";
            this.lblSampleExcelFile.Size = new System.Drawing.Size(239, 13);
            this.lblSampleExcelFile.TabIndex = 177;
            this.lblSampleExcelFile.Text = "Click Here To Download File Format";
            this.lblSampleExcelFile.ToolTips = "";
            this.lblSampleExcelFile.Click += new System.EventHandler(this.lblSampleExcelFile_Click);
            // 
            // PnlFooter
            // 
            this.PnlFooter.BackColor = System.Drawing.Color.Silver;
            this.PnlFooter.Controls.Add(this.panel1);
            this.PnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.PnlFooter.Location = new System.Drawing.Point(0, 91);
            this.PnlFooter.Name = "PnlFooter";
            this.PnlFooter.Size = new System.Drawing.Size(1366, 38);
            this.PnlFooter.TabIndex = 176;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.progressPanel1);
            this.panel1.Controls.Add(this.BtnCalculate);
            this.panel1.Controls.Add(this.lblMessage);
            this.panel1.Controls.Add(this.BtnBestFit);
            this.panel1.Controls.Add(this.BtnBack);
            this.panel1.Controls.Add(this.BtnAdd);
            this.panel1.Controls.Add(this.BtnDelStonePricing);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1218, 38);
            this.panel1.TabIndex = 177;
            // 
            // progressPanel1
            // 
            this.progressPanel1.AnimationToTextDistance = 10;
            this.progressPanel1.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.progressPanel1.Appearance.Options.UseBackColor = true;
            this.progressPanel1.AppearanceCaption.Font = new System.Drawing.Font("Verdana", 12F);
            this.progressPanel1.AppearanceCaption.Options.UseFont = true;
            this.progressPanel1.AppearanceDescription.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.progressPanel1.AppearanceDescription.Options.UseFont = true;
            this.progressPanel1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.progressPanel1.Location = new System.Drawing.Point(552, 5);
            this.progressPanel1.LookAndFeel.SkinName = "Caramel";
            this.progressPanel1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.progressPanel1.Name = "progressPanel1";
            this.progressPanel1.ShowDescription = false;
            this.progressPanel1.Size = new System.Drawing.Size(170, 28);
            this.progressPanel1.TabIndex = 193;
            this.progressPanel1.Text = "progressPanel1";
            this.progressPanel1.Visible = false;
            // 
            // BtnCalculate
            // 
            this.BtnCalculate.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCalculate.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnCalculate.Appearance.Options.UseFont = true;
            this.BtnCalculate.Appearance.Options.UseForeColor = true;
            this.BtnCalculate.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnCalculate.ImageOptions.SvgImage")));
            this.BtnCalculate.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnCalculate.Location = new System.Drawing.Point(5, 5);
            this.BtnCalculate.Name = "BtnCalculate";
            this.BtnCalculate.Size = new System.Drawing.Size(90, 28);
            this.BtnCalculate.TabIndex = 1;
            this.BtnCalculate.Text = "Upload";
            this.BtnCalculate.Click += new System.EventHandler(this.BtnCalculate_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.lblMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblMessage.Location = new System.Drawing.Point(741, 14);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(63, 13);
            this.lblMessage.TabIndex = 171;
            this.lblMessage.Text = "Message";
            this.lblMessage.ToolTips = "";
            // 
            // BtnBestFit
            // 
            this.BtnBestFit.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnBestFit.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnBestFit.Appearance.Options.UseFont = true;
            this.BtnBestFit.Appearance.Options.UseForeColor = true;
            this.BtnBestFit.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnBestFit.ImageOptions.SvgImage")));
            this.BtnBestFit.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnBestFit.Location = new System.Drawing.Point(97, 5);
            this.BtnBestFit.Name = "BtnBestFit";
            this.BtnBestFit.Size = new System.Drawing.Size(90, 28);
            this.BtnBestFit.TabIndex = 14;
            this.BtnBestFit.Text = "Best Fit";
            this.BtnBestFit.Click += new System.EventHandler(this.BtnBestFit_Click);
            // 
            // BtnBack
            // 
            this.BtnBack.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnBack.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnBack.Appearance.Options.UseFont = true;
            this.BtnBack.Appearance.Options.UseForeColor = true;
            this.BtnBack.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnBack.ImageOptions.SvgImage")));
            this.BtnBack.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnBack.Location = new System.Drawing.Point(470, 5);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.Size = new System.Drawing.Size(76, 28);
            this.BtnBack.TabIndex = 32;
            this.BtnBack.TabStop = false;
            this.BtnBack.Text = "E&xit";
            this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // BtnAdd
            // 
            this.BtnAdd.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAdd.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnAdd.Appearance.Options.UseFont = true;
            this.BtnAdd.Appearance.Options.UseForeColor = true;
            this.BtnAdd.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnAdd.ImageOptions.SvgImage")));
            this.BtnAdd.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnAdd.Location = new System.Drawing.Point(189, 5);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(90, 28);
            this.BtnAdd.TabIndex = 13;
            this.BtnAdd.Text = "&Clear";
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // BtnDelStonePricing
            // 
            this.BtnDelStonePricing.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDelStonePricing.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnDelStonePricing.Appearance.Options.UseFont = true;
            this.BtnDelStonePricing.Appearance.Options.UseForeColor = true;
            this.BtnDelStonePricing.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnDelStonePricing.ImageOptions.SvgImage")));
            this.BtnDelStonePricing.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnDelStonePricing.Location = new System.Drawing.Point(281, 5);
            this.BtnDelStonePricing.Name = "BtnDelStonePricing";
            this.BtnDelStonePricing.Size = new System.Drawing.Size(187, 28);
            this.BtnDelStonePricing.TabIndex = 173;
            this.BtnDelStonePricing.TabStop = false;
            this.BtnDelStonePricing.Text = "Delete This File Records";
            this.BtnDelStonePricing.Click += new System.EventHandler(this.BtnDelStonePricing_Click);
            // 
            // cLabel3
            // 
            this.cLabel3.AutoSize = true;
            this.cLabel3.Font = new System.Drawing.Font("Verdana", 9F);
            this.cLabel3.ForeColor = System.Drawing.Color.Black;
            this.cLabel3.Location = new System.Drawing.Point(268, 46);
            this.cLabel3.Name = "cLabel3";
            this.cLabel3.Size = new System.Drawing.Size(67, 14);
            this.cLabel3.TabIndex = 170;
            this.cLabel3.Text = "Sel Sheet";
            this.cLabel3.ToolTips = "";
            // 
            // cLabel2
            // 
            this.cLabel2.AutoSize = true;
            this.cLabel2.Font = new System.Drawing.Font("Verdana", 9F);
            this.cLabel2.ForeColor = System.Drawing.Color.Black;
            this.cLabel2.Location = new System.Drawing.Point(5, 46);
            this.cLabel2.Name = "cLabel2";
            this.cLabel2.Size = new System.Drawing.Size(28, 14);
            this.cLabel2.TabIndex = 168;
            this.cLabel2.Text = "File";
            this.cLabel2.ToolTips = "";
            // 
            // BtnBrowse
            // 
            this.BtnBrowse.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnBrowse.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnBrowse.Appearance.Options.UseFont = true;
            this.BtnBrowse.Appearance.Options.UseForeColor = true;
            this.BtnBrowse.Location = new System.Drawing.Point(229, 41);
            this.BtnBrowse.Name = "BtnBrowse";
            this.BtnBrowse.Size = new System.Drawing.Size(25, 24);
            this.BtnBrowse.TabIndex = 3;
            this.BtnBrowse.Text = "..";
            this.BtnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.ActivationColor = true;
            this.txtFileName.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtFileName.AllowTabKeyOnEnter = false;
            this.txtFileName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFileName.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtFileName.Format = "";
            this.txtFileName.IsComplusory = false;
            this.txtFileName.Location = new System.Drawing.Point(48, 42);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.SelectAllTextOnFocus = true;
            this.txtFileName.Size = new System.Drawing.Size(181, 22);
            this.txtFileName.TabIndex = 1;
            this.txtFileName.ToolTips = "";
            this.txtFileName.WaterMarkText = null;
            // 
            // txtPartyName
            // 
            this.txtPartyName.ActivationColor = true;
            this.txtPartyName.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtPartyName.AllowTabKeyOnEnter = false;
            this.txtPartyName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPartyName.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtPartyName.Format = "";
            this.txtPartyName.IsComplusory = false;
            this.txtPartyName.Location = new System.Drawing.Point(48, 15);
            this.txtPartyName.Name = "txtPartyName";
            this.txtPartyName.SelectAllTextOnFocus = true;
            this.txtPartyName.Size = new System.Drawing.Size(206, 22);
            this.txtPartyName.TabIndex = 0;
            this.txtPartyName.ToolTips = "";
            this.txtPartyName.WaterMarkText = null;
            this.txtPartyName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtParty_KeyPress);
            // 
            // cLabel1
            // 
            this.cLabel1.AutoSize = true;
            this.cLabel1.Font = new System.Drawing.Font("Verdana", 9F);
            this.cLabel1.ForeColor = System.Drawing.Color.Black;
            this.cLabel1.Location = new System.Drawing.Point(5, 19);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(40, 14);
            this.cLabel1.TabIndex = 30;
            this.cLabel1.Text = "Party";
            this.cLabel1.ToolTips = "";
            // 
            // cLabel5
            // 
            this.cLabel5.AutoSize = true;
            this.cLabel5.Font = new System.Drawing.Font("Verdana", 8F);
            this.cLabel5.ForeColor = System.Drawing.Color.Black;
            this.cLabel5.Location = new System.Drawing.Point(22, 92);
            this.cLabel5.Name = "cLabel5";
            this.cLabel5.Size = new System.Drawing.Size(82, 13);
            this.cLabel5.TabIndex = 183;
            this.cLabel5.Text = "---: Type :---";
            this.cLabel5.ToolTips = "";
            // 
            // CmbStockType
            // 
            this.CmbStockType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbStockType.Font = new System.Drawing.Font("Verdana", 9F);
            this.CmbStockType.FormattingEnabled = true;
            this.CmbStockType.Items.AddRange(new object[] {
            "SINGLE",
            "PARCEL"});
            this.CmbStockType.Location = new System.Drawing.Point(15, 105);
            this.CmbStockType.Name = "CmbStockType";
            this.CmbStockType.Size = new System.Drawing.Size(115, 22);
            this.CmbStockType.TabIndex = 5;
            this.CmbStockType.SelectedIndexChanged += new System.EventHandler(this.CmbStockType_SelectedIndexChanged);
            // 
            // lblDefaultLayout
            // 
            this.lblDefaultLayout.AutoSize = true;
            this.lblDefaultLayout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblDefaultLayout.Font = new System.Drawing.Font("Verdana", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.lblDefaultLayout.ForeColor = System.Drawing.Color.Navy;
            this.lblDefaultLayout.Location = new System.Drawing.Point(966, 69);
            this.lblDefaultLayout.Name = "lblDefaultLayout";
            this.lblDefaultLayout.Size = new System.Drawing.Size(99, 14);
            this.lblDefaultLayout.TabIndex = 247;
            this.lblDefaultLayout.Text = "Delete Layout";
            this.lblDefaultLayout.ToolTips = "";
            this.lblDefaultLayout.Click += new System.EventHandler(this.lblDefaultLayout_Click);
            // 
            // lblSaveLayout
            // 
            this.lblSaveLayout.AutoSize = true;
            this.lblSaveLayout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblSaveLayout.Font = new System.Drawing.Font("Verdana", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.lblSaveLayout.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblSaveLayout.Location = new System.Drawing.Point(871, 69);
            this.lblSaveLayout.Name = "lblSaveLayout";
            this.lblSaveLayout.Size = new System.Drawing.Size(89, 14);
            this.lblSaveLayout.TabIndex = 246;
            this.lblSaveLayout.Text = "Save Layout";
            this.lblSaveLayout.ToolTips = "";
            this.lblSaveLayout.Click += new System.EventHandler(this.lblSaveLayout_Click);
            // 
            // cLabel28
            // 
            this.cLabel28.Dock = System.Windows.Forms.DockStyle.Top;
            this.cLabel28.Font = new System.Drawing.Font("Verdana", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.cLabel28.ForeColor = System.Drawing.Color.Black;
            this.cLabel28.Location = new System.Drawing.Point(0, 0);
            this.cLabel28.Name = "cLabel28";
            this.cLabel28.Size = new System.Drawing.Size(94, 17);
            this.cLabel28.TabIndex = 180;
            this.cLabel28.Text = "Date Wise Summary";
            this.cLabel28.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cLabel28.ToolTips = "";
            // 
            // BtnClearFilter
            // 
            this.BtnClearFilter.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnClearFilter.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnClearFilter.Appearance.Options.UseFont = true;
            this.BtnClearFilter.Appearance.Options.UseForeColor = true;
            this.BtnClearFilter.Location = new System.Drawing.Point(2, 0);
            this.BtnClearFilter.Name = "BtnClearFilter";
            this.BtnClearFilter.Size = new System.Drawing.Size(86, 17);
            this.BtnClearFilter.TabIndex = 1;
            this.BtnClearFilter.Text = "Clear Filter";
            this.BtnClearFilter.Click += new System.EventHandler(this.BtnClearFilter_Click);
            // 
            // gridView1
            // 
            this.gridView1.Name = "gridView1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 129);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.MainGrdStock);
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            this.splitContainer1.Panel1.Controls.Add(this.cLabel12);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.MainGrdDateWiseSum);
            this.splitContainer1.Panel2.Controls.Add(this.BtnClearFilter);
            this.splitContainer1.Panel2.Controls.Add(this.cLabel28);
            this.splitContainer1.Panel2Collapsed = true;
            this.splitContainer1.Size = new System.Drawing.Size(1366, 536);
            this.splitContainer1.SplitterDistance = 1084;
            this.splitContainer1.TabIndex = 194;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.cLabel29);
            this.panel2.Controls.Add(this.lblPurchaseReturn);
            this.panel2.Controls.Add(this.cLabel27);
            this.panel2.Controls.Add(this.cLabel40);
            this.panel2.Controls.Add(this.cLabel38);
            this.panel2.Controls.Add(this.cLabel11);
            this.panel2.Controls.Add(this.cLabel32);
            this.panel2.Controls.Add(this.lblNone);
            this.panel2.Controls.Add(this.lblInvoice);
            this.panel2.Controls.Add(this.lblSold);
            this.panel2.Controls.Add(this.lblMemo);
            this.panel2.Controls.Add(this.lblAvailable);
            this.panel2.Controls.Add(this.txtSelectedAvgRap);
            this.panel2.Controls.Add(this.cLabel19);
            this.panel2.Controls.Add(this.cLabel13);
            this.panel2.Controls.Add(this.cLabel26);
            this.panel2.Controls.Add(this.cLabel24);
            this.panel2.Controls.Add(this.txtSelectedCarat);
            this.panel2.Controls.Add(this.cLabel14);
            this.panel2.Controls.Add(this.txtSelectedDisc);
            this.panel2.Controls.Add(this.cLabel20);
            this.panel2.Controls.Add(this.txtSelectedPricePerCarat);
            this.panel2.Controls.Add(this.txtSelectedAmount);
            this.panel2.Controls.Add(this.txtTotalAvgRap);
            this.panel2.Controls.Add(this.cLabel22);
            this.panel2.Controls.Add(this.txtTotalCarat);
            this.panel2.Controls.Add(this.cLabel25);
            this.panel2.Controls.Add(this.txtTotalDisc);
            this.panel2.Controls.Add(this.cLabel23);
            this.panel2.Controls.Add(this.txtTotalPricePerCarat);
            this.panel2.Controls.Add(this.cLabel21);
            this.panel2.Controls.Add(this.txtTotalAmount);
            this.panel2.Controls.Add(this.cLabel16);
            this.panel2.Controls.Add(this.cLabel15);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 482);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1364, 52);
            this.panel2.TabIndex = 182;
            // 
            // cLabel29
            // 
            this.cLabel29.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cLabel29.AutoSize = true;
            this.cLabel29.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.cLabel29.ForeColor = System.Drawing.Color.Black;
            this.cLabel29.Location = new System.Drawing.Point(1117, 26);
            this.cLabel29.Name = "cLabel29";
            this.cLabel29.Size = new System.Drawing.Size(76, 13);
            this.cLabel29.TabIndex = 242;
            this.cLabel29.Text = "Pur Return";
            this.cLabel29.ToolTips = "";
            // 
            // lblPurchaseReturn
            // 
            this.lblPurchaseReturn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPurchaseReturn.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(211)))), ((int)(((byte)(178)))));
            this.lblPurchaseReturn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblPurchaseReturn.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblPurchaseReturn.ForeColor = System.Drawing.Color.Black;
            this.lblPurchaseReturn.Location = new System.Drawing.Point(1089, 24);
            this.lblPurchaseReturn.Name = "lblPurchaseReturn";
            this.lblPurchaseReturn.Size = new System.Drawing.Size(26, 18);
            this.lblPurchaseReturn.TabIndex = 195;
            this.lblPurchaseReturn.ToolTips = "";
            // 
            // cLabel27
            // 
            this.cLabel27.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cLabel27.AutoSize = true;
            this.cLabel27.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.cLabel27.ForeColor = System.Drawing.Color.Black;
            this.cLabel27.Location = new System.Drawing.Point(1327, 26);
            this.cLabel27.Name = "cLabel27";
            this.cLabel27.Size = new System.Drawing.Size(34, 13);
            this.cLabel27.TabIndex = 241;
            this.cLabel27.Text = "New";
            this.cLabel27.ToolTips = "";
            // 
            // cLabel40
            // 
            this.cLabel40.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cLabel40.AutoSize = true;
            this.cLabel40.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.cLabel40.ForeColor = System.Drawing.Color.Black;
            this.cLabel40.Location = new System.Drawing.Point(1227, 26);
            this.cLabel40.Name = "cLabel40";
            this.cLabel40.Size = new System.Drawing.Size(62, 13);
            this.cLabel40.TabIndex = 232;
            this.cLabel40.Text = "Delivery";
            this.cLabel40.ToolTips = "";
            // 
            // cLabel38
            // 
            this.cLabel38.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cLabel38.AutoSize = true;
            this.cLabel38.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.cLabel38.ForeColor = System.Drawing.Color.Black;
            this.cLabel38.Location = new System.Drawing.Point(1323, 5);
            this.cLabel38.Name = "cLabel38";
            this.cLabel38.Size = new System.Drawing.Size(35, 13);
            this.cLabel38.TabIndex = 233;
            this.cLabel38.Text = "Sold";
            this.cLabel38.ToolTips = "";
            // 
            // cLabel11
            // 
            this.cLabel11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cLabel11.AutoSize = true;
            this.cLabel11.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.cLabel11.ForeColor = System.Drawing.Color.Black;
            this.cLabel11.Location = new System.Drawing.Point(1227, 5);
            this.cLabel11.Name = "cLabel11";
            this.cLabel11.Size = new System.Drawing.Size(45, 13);
            this.cLabel11.TabIndex = 234;
            this.cLabel11.Text = "Memo";
            this.cLabel11.ToolTips = "";
            // 
            // cLabel32
            // 
            this.cLabel32.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cLabel32.AutoSize = true;
            this.cLabel32.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.cLabel32.ForeColor = System.Drawing.Color.Black;
            this.cLabel32.Location = new System.Drawing.Point(1117, 5);
            this.cLabel32.Name = "cLabel32";
            this.cLabel32.Size = new System.Drawing.Size(68, 13);
            this.cLabel32.TabIndex = 235;
            this.cLabel32.Text = "Available";
            this.cLabel32.ToolTips = "";
            // 
            // lblNone
            // 
            this.lblNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblNone.BackColor = System.Drawing.Color.LightCyan;
            this.lblNone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNone.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblNone.ForeColor = System.Drawing.Color.Black;
            this.lblNone.Location = new System.Drawing.Point(1295, 25);
            this.lblNone.Name = "lblNone";
            this.lblNone.Size = new System.Drawing.Size(26, 17);
            this.lblNone.TabIndex = 236;
            this.lblNone.ToolTips = "";
            // 
            // lblInvoice
            // 
            this.lblInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInvoice.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.lblInvoice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblInvoice.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblInvoice.ForeColor = System.Drawing.Color.Black;
            this.lblInvoice.Location = new System.Drawing.Point(1199, 25);
            this.lblInvoice.Name = "lblInvoice";
            this.lblInvoice.Size = new System.Drawing.Size(26, 17);
            this.lblInvoice.TabIndex = 237;
            this.lblInvoice.ToolTips = "";
            // 
            // lblSold
            // 
            this.lblSold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSold.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblSold.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSold.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblSold.ForeColor = System.Drawing.Color.Black;
            this.lblSold.Location = new System.Drawing.Point(1295, 3);
            this.lblSold.Name = "lblSold";
            this.lblSold.Size = new System.Drawing.Size(26, 17);
            this.lblSold.TabIndex = 238;
            this.lblSold.ToolTips = "";
            // 
            // lblMemo
            // 
            this.lblMemo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMemo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.lblMemo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblMemo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblMemo.ForeColor = System.Drawing.Color.Black;
            this.lblMemo.Location = new System.Drawing.Point(1199, 3);
            this.lblMemo.Name = "lblMemo";
            this.lblMemo.Size = new System.Drawing.Size(26, 17);
            this.lblMemo.TabIndex = 239;
            this.lblMemo.ToolTips = "";
            // 
            // lblAvailable
            // 
            this.lblAvailable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAvailable.BackColor = System.Drawing.Color.White;
            this.lblAvailable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblAvailable.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblAvailable.ForeColor = System.Drawing.Color.Black;
            this.lblAvailable.Location = new System.Drawing.Point(1089, 3);
            this.lblAvailable.Name = "lblAvailable";
            this.lblAvailable.Size = new System.Drawing.Size(26, 17);
            this.lblAvailable.TabIndex = 240;
            this.lblAvailable.ToolTips = "";
            // 
            // txtSelectedAvgRap
            // 
            this.txtSelectedAvgRap.ActivationColor = true;
            this.txtSelectedAvgRap.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtSelectedAvgRap.AllowTabKeyOnEnter = false;
            this.txtSelectedAvgRap.BackColor = System.Drawing.Color.AliceBlue;
            this.txtSelectedAvgRap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSelectedAvgRap.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.txtSelectedAvgRap.ForeColor = System.Drawing.Color.Black;
            this.txtSelectedAvgRap.Format = "";
            this.txtSelectedAvgRap.IsComplusory = false;
            this.txtSelectedAvgRap.Location = new System.Drawing.Point(277, 27);
            this.txtSelectedAvgRap.MaxLength = 100;
            this.txtSelectedAvgRap.Name = "txtSelectedAvgRap";
            this.txtSelectedAvgRap.ReadOnly = true;
            this.txtSelectedAvgRap.SelectAllTextOnFocus = true;
            this.txtSelectedAvgRap.Size = new System.Drawing.Size(90, 20);
            this.txtSelectedAvgRap.TabIndex = 230;
            this.txtSelectedAvgRap.TabStop = false;
            this.txtSelectedAvgRap.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSelectedAvgRap.ToolTips = "";
            this.txtSelectedAvgRap.WaterMarkText = null;
            // 
            // cLabel19
            // 
            this.cLabel19.AutoSize = true;
            this.cLabel19.Font = new System.Drawing.Font("Verdana", 7F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.cLabel19.ForeColor = System.Drawing.Color.Black;
            this.cLabel19.Location = new System.Drawing.Point(370, 31);
            this.cLabel19.Name = "cLabel19";
            this.cLabel19.Size = new System.Drawing.Size(27, 12);
            this.cLabel19.TabIndex = 231;
            this.cLabel19.Text = "Rap";
            this.cLabel19.ToolTips = "";
            // 
            // cLabel13
            // 
            this.cLabel13.AutoSize = true;
            this.cLabel13.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.cLabel13.ForeColor = System.Drawing.Color.Black;
            this.cLabel13.Location = new System.Drawing.Point(5, 30);
            this.cLabel13.Name = "cLabel13";
            this.cLabel13.Size = new System.Drawing.Size(46, 13);
            this.cLabel13.TabIndex = 222;
            this.cLabel13.Text = "Selec.";
            this.cLabel13.ToolTips = "";
            // 
            // cLabel26
            // 
            this.cLabel26.AutoSize = true;
            this.cLabel26.Font = new System.Drawing.Font("Verdana", 7F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.cLabel26.ForeColor = System.Drawing.Color.Black;
            this.cLabel26.Location = new System.Drawing.Point(636, 31);
            this.cLabel26.Name = "cLabel26";
            this.cLabel26.Size = new System.Drawing.Size(12, 12);
            this.cLabel26.TabIndex = 223;
            this.cLabel26.Text = "$";
            this.cLabel26.ToolTips = "";
            // 
            // cLabel24
            // 
            this.cLabel24.AutoSize = true;
            this.cLabel24.Font = new System.Drawing.Font("Verdana", 7F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.cLabel24.ForeColor = System.Drawing.Color.Black;
            this.cLabel24.Location = new System.Drawing.Point(492, 31);
            this.cLabel24.Name = "cLabel24";
            this.cLabel24.Size = new System.Drawing.Size(37, 12);
            this.cLabel24.TabIndex = 224;
            this.cLabel24.Text = "$/Cts";
            this.cLabel24.ToolTips = "";
            // 
            // txtSelectedCarat
            // 
            this.txtSelectedCarat.ActivationColor = true;
            this.txtSelectedCarat.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtSelectedCarat.AllowTabKeyOnEnter = false;
            this.txtSelectedCarat.BackColor = System.Drawing.Color.AliceBlue;
            this.txtSelectedCarat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSelectedCarat.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.txtSelectedCarat.ForeColor = System.Drawing.Color.Black;
            this.txtSelectedCarat.Format = "";
            this.txtSelectedCarat.IsComplusory = false;
            this.txtSelectedCarat.Location = new System.Drawing.Point(58, 27);
            this.txtSelectedCarat.MaxLength = 100;
            this.txtSelectedCarat.Name = "txtSelectedCarat";
            this.txtSelectedCarat.ReadOnly = true;
            this.txtSelectedCarat.SelectAllTextOnFocus = true;
            this.txtSelectedCarat.Size = new System.Drawing.Size(90, 20);
            this.txtSelectedCarat.TabIndex = 218;
            this.txtSelectedCarat.TabStop = false;
            this.txtSelectedCarat.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSelectedCarat.ToolTips = "";
            this.txtSelectedCarat.WaterMarkText = null;
            // 
            // cLabel14
            // 
            this.cLabel14.AutoSize = true;
            this.cLabel14.Font = new System.Drawing.Font("Verdana", 7F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.cLabel14.ForeColor = System.Drawing.Color.Black;
            this.cLabel14.Location = new System.Drawing.Point(254, 31);
            this.cLabel14.Name = "cLabel14";
            this.cLabel14.Size = new System.Drawing.Size(18, 12);
            this.cLabel14.TabIndex = 225;
            this.cLabel14.Text = "%";
            this.cLabel14.ToolTips = "";
            // 
            // txtSelectedDisc
            // 
            this.txtSelectedDisc.ActivationColor = true;
            this.txtSelectedDisc.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtSelectedDisc.AllowTabKeyOnEnter = false;
            this.txtSelectedDisc.BackColor = System.Drawing.Color.AliceBlue;
            this.txtSelectedDisc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSelectedDisc.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.txtSelectedDisc.ForeColor = System.Drawing.Color.Black;
            this.txtSelectedDisc.Format = "";
            this.txtSelectedDisc.IsComplusory = false;
            this.txtSelectedDisc.Location = new System.Drawing.Point(181, 27);
            this.txtSelectedDisc.MaxLength = 100;
            this.txtSelectedDisc.Name = "txtSelectedDisc";
            this.txtSelectedDisc.ReadOnly = true;
            this.txtSelectedDisc.SelectAllTextOnFocus = true;
            this.txtSelectedDisc.Size = new System.Drawing.Size(71, 20);
            this.txtSelectedDisc.TabIndex = 219;
            this.txtSelectedDisc.TabStop = false;
            this.txtSelectedDisc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSelectedDisc.ToolTips = "";
            this.txtSelectedDisc.WaterMarkText = null;
            // 
            // cLabel20
            // 
            this.cLabel20.AutoSize = true;
            this.cLabel20.Font = new System.Drawing.Font("Verdana", 7F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.cLabel20.ForeColor = System.Drawing.Color.Black;
            this.cLabel20.Location = new System.Drawing.Point(151, 31);
            this.cLabel20.Name = "cLabel20";
            this.cLabel20.Size = new System.Drawing.Size(23, 12);
            this.cLabel20.TabIndex = 226;
            this.cLabel20.Text = "Cts";
            this.cLabel20.ToolTips = "";
            // 
            // txtSelectedPricePerCarat
            // 
            this.txtSelectedPricePerCarat.ActivationColor = true;
            this.txtSelectedPricePerCarat.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtSelectedPricePerCarat.AllowTabKeyOnEnter = false;
            this.txtSelectedPricePerCarat.BackColor = System.Drawing.Color.AliceBlue;
            this.txtSelectedPricePerCarat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSelectedPricePerCarat.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.txtSelectedPricePerCarat.ForeColor = System.Drawing.Color.Black;
            this.txtSelectedPricePerCarat.Format = "";
            this.txtSelectedPricePerCarat.IsComplusory = false;
            this.txtSelectedPricePerCarat.Location = new System.Drawing.Point(399, 27);
            this.txtSelectedPricePerCarat.MaxLength = 100;
            this.txtSelectedPricePerCarat.Name = "txtSelectedPricePerCarat";
            this.txtSelectedPricePerCarat.ReadOnly = true;
            this.txtSelectedPricePerCarat.SelectAllTextOnFocus = true;
            this.txtSelectedPricePerCarat.Size = new System.Drawing.Size(91, 20);
            this.txtSelectedPricePerCarat.TabIndex = 220;
            this.txtSelectedPricePerCarat.TabStop = false;
            this.txtSelectedPricePerCarat.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSelectedPricePerCarat.ToolTips = "";
            this.txtSelectedPricePerCarat.WaterMarkText = null;
            // 
            // txtSelectedAmount
            // 
            this.txtSelectedAmount.ActivationColor = true;
            this.txtSelectedAmount.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtSelectedAmount.AllowTabKeyOnEnter = false;
            this.txtSelectedAmount.BackColor = System.Drawing.Color.AliceBlue;
            this.txtSelectedAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSelectedAmount.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.txtSelectedAmount.ForeColor = System.Drawing.Color.Black;
            this.txtSelectedAmount.Format = "";
            this.txtSelectedAmount.IsComplusory = false;
            this.txtSelectedAmount.Location = new System.Drawing.Point(535, 27);
            this.txtSelectedAmount.MaxLength = 100;
            this.txtSelectedAmount.Name = "txtSelectedAmount";
            this.txtSelectedAmount.ReadOnly = true;
            this.txtSelectedAmount.SelectAllTextOnFocus = true;
            this.txtSelectedAmount.Size = new System.Drawing.Size(98, 20);
            this.txtSelectedAmount.TabIndex = 221;
            this.txtSelectedAmount.TabStop = false;
            this.txtSelectedAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSelectedAmount.ToolTips = "";
            this.txtSelectedAmount.WaterMarkText = null;
            // 
            // txtTotalAvgRap
            // 
            this.txtTotalAvgRap.ActivationColor = true;
            this.txtTotalAvgRap.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtTotalAvgRap.AllowTabKeyOnEnter = false;
            this.txtTotalAvgRap.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtTotalAvgRap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalAvgRap.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.txtTotalAvgRap.ForeColor = System.Drawing.Color.Black;
            this.txtTotalAvgRap.Format = "";
            this.txtTotalAvgRap.IsComplusory = false;
            this.txtTotalAvgRap.Location = new System.Drawing.Point(277, 3);
            this.txtTotalAvgRap.MaxLength = 100;
            this.txtTotalAvgRap.Name = "txtTotalAvgRap";
            this.txtTotalAvgRap.ReadOnly = true;
            this.txtTotalAvgRap.SelectAllTextOnFocus = true;
            this.txtTotalAvgRap.Size = new System.Drawing.Size(90, 20);
            this.txtTotalAvgRap.TabIndex = 214;
            this.txtTotalAvgRap.TabStop = false;
            this.txtTotalAvgRap.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotalAvgRap.ToolTips = "";
            this.txtTotalAvgRap.WaterMarkText = null;
            // 
            // cLabel22
            // 
            this.cLabel22.AutoSize = true;
            this.cLabel22.Font = new System.Drawing.Font("Verdana", 7F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.cLabel22.ForeColor = System.Drawing.Color.Black;
            this.cLabel22.Location = new System.Drawing.Point(370, 7);
            this.cLabel22.Name = "cLabel22";
            this.cLabel22.Size = new System.Drawing.Size(27, 12);
            this.cLabel22.TabIndex = 216;
            this.cLabel22.Text = "Rap";
            this.cLabel22.ToolTips = "";
            // 
            // txtTotalCarat
            // 
            this.txtTotalCarat.ActivationColor = true;
            this.txtTotalCarat.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtTotalCarat.AllowTabKeyOnEnter = false;
            this.txtTotalCarat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtTotalCarat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalCarat.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.txtTotalCarat.ForeColor = System.Drawing.Color.Black;
            this.txtTotalCarat.Format = "";
            this.txtTotalCarat.IsComplusory = false;
            this.txtTotalCarat.Location = new System.Drawing.Point(58, 3);
            this.txtTotalCarat.MaxLength = 100;
            this.txtTotalCarat.Name = "txtTotalCarat";
            this.txtTotalCarat.ReadOnly = true;
            this.txtTotalCarat.SelectAllTextOnFocus = true;
            this.txtTotalCarat.Size = new System.Drawing.Size(90, 20);
            this.txtTotalCarat.TabIndex = 2;
            this.txtTotalCarat.TabStop = false;
            this.txtTotalCarat.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotalCarat.ToolTips = "";
            this.txtTotalCarat.WaterMarkText = null;
            // 
            // cLabel25
            // 
            this.cLabel25.AutoSize = true;
            this.cLabel25.Font = new System.Drawing.Font("Verdana", 7F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.cLabel25.ForeColor = System.Drawing.Color.Black;
            this.cLabel25.Location = new System.Drawing.Point(636, 7);
            this.cLabel25.Name = "cLabel25";
            this.cLabel25.Size = new System.Drawing.Size(12, 12);
            this.cLabel25.TabIndex = 179;
            this.cLabel25.Text = "$";
            this.cLabel25.ToolTips = "";
            // 
            // txtTotalDisc
            // 
            this.txtTotalDisc.ActivationColor = true;
            this.txtTotalDisc.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtTotalDisc.AllowTabKeyOnEnter = false;
            this.txtTotalDisc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtTotalDisc.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalDisc.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.txtTotalDisc.ForeColor = System.Drawing.Color.Black;
            this.txtTotalDisc.Format = "";
            this.txtTotalDisc.IsComplusory = false;
            this.txtTotalDisc.Location = new System.Drawing.Point(181, 3);
            this.txtTotalDisc.MaxLength = 100;
            this.txtTotalDisc.Name = "txtTotalDisc";
            this.txtTotalDisc.ReadOnly = true;
            this.txtTotalDisc.SelectAllTextOnFocus = true;
            this.txtTotalDisc.Size = new System.Drawing.Size(71, 20);
            this.txtTotalDisc.TabIndex = 2;
            this.txtTotalDisc.TabStop = false;
            this.txtTotalDisc.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotalDisc.ToolTips = "";
            this.txtTotalDisc.WaterMarkText = null;
            // 
            // cLabel23
            // 
            this.cLabel23.AutoSize = true;
            this.cLabel23.Font = new System.Drawing.Font("Verdana", 7F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.cLabel23.ForeColor = System.Drawing.Color.Black;
            this.cLabel23.Location = new System.Drawing.Point(492, 7);
            this.cLabel23.Name = "cLabel23";
            this.cLabel23.Size = new System.Drawing.Size(37, 12);
            this.cLabel23.TabIndex = 179;
            this.cLabel23.Text = "$/Cts";
            this.cLabel23.ToolTips = "";
            // 
            // txtTotalPricePerCarat
            // 
            this.txtTotalPricePerCarat.ActivationColor = true;
            this.txtTotalPricePerCarat.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtTotalPricePerCarat.AllowTabKeyOnEnter = false;
            this.txtTotalPricePerCarat.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtTotalPricePerCarat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalPricePerCarat.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.txtTotalPricePerCarat.ForeColor = System.Drawing.Color.Black;
            this.txtTotalPricePerCarat.Format = "";
            this.txtTotalPricePerCarat.IsComplusory = false;
            this.txtTotalPricePerCarat.Location = new System.Drawing.Point(399, 3);
            this.txtTotalPricePerCarat.MaxLength = 100;
            this.txtTotalPricePerCarat.Name = "txtTotalPricePerCarat";
            this.txtTotalPricePerCarat.ReadOnly = true;
            this.txtTotalPricePerCarat.SelectAllTextOnFocus = true;
            this.txtTotalPricePerCarat.Size = new System.Drawing.Size(91, 20);
            this.txtTotalPricePerCarat.TabIndex = 2;
            this.txtTotalPricePerCarat.TabStop = false;
            this.txtTotalPricePerCarat.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotalPricePerCarat.ToolTips = "";
            this.txtTotalPricePerCarat.WaterMarkText = null;
            // 
            // cLabel21
            // 
            this.cLabel21.AutoSize = true;
            this.cLabel21.Font = new System.Drawing.Font("Verdana", 7F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.cLabel21.ForeColor = System.Drawing.Color.Black;
            this.cLabel21.Location = new System.Drawing.Point(254, 7);
            this.cLabel21.Name = "cLabel21";
            this.cLabel21.Size = new System.Drawing.Size(18, 12);
            this.cLabel21.TabIndex = 179;
            this.cLabel21.Text = "%";
            this.cLabel21.ToolTips = "";
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.ActivationColor = true;
            this.txtTotalAmount.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtTotalAmount.AllowTabKeyOnEnter = false;
            this.txtTotalAmount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtTotalAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalAmount.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.txtTotalAmount.ForeColor = System.Drawing.Color.Black;
            this.txtTotalAmount.Format = "";
            this.txtTotalAmount.IsComplusory = false;
            this.txtTotalAmount.Location = new System.Drawing.Point(535, 3);
            this.txtTotalAmount.MaxLength = 100;
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.ReadOnly = true;
            this.txtTotalAmount.SelectAllTextOnFocus = true;
            this.txtTotalAmount.Size = new System.Drawing.Size(98, 20);
            this.txtTotalAmount.TabIndex = 2;
            this.txtTotalAmount.TabStop = false;
            this.txtTotalAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtTotalAmount.ToolTips = "";
            this.txtTotalAmount.WaterMarkText = null;
            // 
            // cLabel16
            // 
            this.cLabel16.AutoSize = true;
            this.cLabel16.Font = new System.Drawing.Font("Verdana", 7F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.cLabel16.ForeColor = System.Drawing.Color.Black;
            this.cLabel16.Location = new System.Drawing.Point(151, 7);
            this.cLabel16.Name = "cLabel16";
            this.cLabel16.Size = new System.Drawing.Size(23, 12);
            this.cLabel16.TabIndex = 179;
            this.cLabel16.Text = "Cts";
            this.cLabel16.ToolTips = "";
            // 
            // cLabel15
            // 
            this.cLabel15.AutoSize = true;
            this.cLabel15.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.cLabel15.ForeColor = System.Drawing.Color.Black;
            this.cLabel15.Location = new System.Drawing.Point(5, 7);
            this.cLabel15.Name = "cLabel15";
            this.cLabel15.Size = new System.Drawing.Size(40, 13);
            this.cLabel15.TabIndex = 179;
            this.cLabel15.Text = "Total";
            this.cLabel15.ToolTips = "";
            // 
            // cLabel12
            // 
            this.cLabel12.Dock = System.Windows.Forms.DockStyle.Top;
            this.cLabel12.Font = new System.Drawing.Font("Verdana", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.cLabel12.ForeColor = System.Drawing.Color.Black;
            this.cLabel12.Location = new System.Drawing.Point(0, 0);
            this.cLabel12.Name = "cLabel12";
            this.cLabel12.Size = new System.Drawing.Size(1364, 17);
            this.cLabel12.TabIndex = 181;
            this.cLabel12.Text = "Packet Detail";
            this.cLabel12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cLabel12.ToolTips = "";
            // 
            // MainGrdDateWiseSum
            // 
            this.MainGrdDateWiseSum.Dock = System.Windows.Forms.DockStyle.Fill;
            gridLevelNode2.RelationName = "Level1";
            this.MainGrdDateWiseSum.LevelTree.Nodes.AddRange(new DevExpress.XtraGrid.GridLevelNode[] {
            gridLevelNode2});
            this.MainGrdDateWiseSum.Location = new System.Drawing.Point(0, 17);
            this.MainGrdDateWiseSum.MainView = this.GrdDateWiseSum;
            this.MainGrdDateWiseSum.Name = "MainGrdDateWiseSum";
            this.MainGrdDateWiseSum.Size = new System.Drawing.Size(94, 81);
            this.MainGrdDateWiseSum.TabIndex = 181;
            this.MainGrdDateWiseSum.TabStop = false;
            this.MainGrdDateWiseSum.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GrdDateWiseSum});
            // 
            // GrdDateWiseSum
            // 
            this.GrdDateWiseSum.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(234)))), ((int)(((byte)(141)))));
            this.GrdDateWiseSum.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(234)))), ((int)(((byte)(141)))));
            this.GrdDateWiseSum.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.GrdDateWiseSum.Appearance.FocusedCell.Options.UseBackColor = true;
            this.GrdDateWiseSum.Appearance.FocusedCell.Options.UseForeColor = true;
            this.GrdDateWiseSum.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.GrdDateWiseSum.Appearance.FocusedRow.Options.UseForeColor = true;
            this.GrdDateWiseSum.Appearance.FooterPanel.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.GrdDateWiseSum.Appearance.FooterPanel.Options.UseFont = true;
            this.GrdDateWiseSum.Appearance.GroupFooter.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.GrdDateWiseSum.Appearance.GroupFooter.Options.UseFont = true;
            this.GrdDateWiseSum.Appearance.GroupRow.BackColor = System.Drawing.Color.WhiteSmoke;
            this.GrdDateWiseSum.Appearance.GroupRow.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.GrdDateWiseSum.Appearance.GroupRow.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.GrdDateWiseSum.Appearance.GroupRow.Options.UseBackColor = true;
            this.GrdDateWiseSum.Appearance.GroupRow.Options.UseFont = true;
            this.GrdDateWiseSum.Appearance.GroupRow.Options.UseForeColor = true;
            this.GrdDateWiseSum.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.GrdDateWiseSum.Appearance.HeaderPanel.Options.UseFont = true;
            this.GrdDateWiseSum.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.GrdDateWiseSum.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDateWiseSum.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.Black;
            this.GrdDateWiseSum.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.GrdDateWiseSum.Appearance.HorzLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDateWiseSum.Appearance.HorzLine.Options.UseBackColor = true;
            this.GrdDateWiseSum.Appearance.Row.Font = new System.Drawing.Font("Verdana", 8.5F);
            this.GrdDateWiseSum.Appearance.Row.Options.UseFont = true;
            this.GrdDateWiseSum.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Black;
            this.GrdDateWiseSum.Appearance.SelectedRow.Options.UseForeColor = true;
            this.GrdDateWiseSum.Appearance.VertLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDateWiseSum.Appearance.VertLine.Options.UseBackColor = true;
            this.GrdDateWiseSum.AppearancePrint.FooterPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.GrdDateWiseSum.AppearancePrint.FooterPanel.Options.UseFont = true;
            this.GrdDateWiseSum.AppearancePrint.HeaderPanel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.GrdDateWiseSum.AppearancePrint.HeaderPanel.Options.UseFont = true;
            this.GrdDateWiseSum.ColumnPanelRowHeight = 25;
            this.GrdDateWiseSum.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3,
            this.gridColumn4});
            this.GrdDateWiseSum.GridControl = this.MainGrdDateWiseSum;
            this.GrdDateWiseSum.GroupFormat = "{1} {2}";
            this.GrdDateWiseSum.Name = "GrdDateWiseSum";
            this.GrdDateWiseSum.OptionsBehavior.Editable = false;
            this.GrdDateWiseSum.OptionsCustomization.AllowSort = false;
            this.GrdDateWiseSum.OptionsFilter.AllowFilterEditor = false;
            this.GrdDateWiseSum.OptionsNavigation.EnterMoveNextColumn = true;
            this.GrdDateWiseSum.OptionsPrint.ExpandAllGroups = false;
            this.GrdDateWiseSum.OptionsSelection.MultiSelect = true;
            this.GrdDateWiseSum.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.GrdDateWiseSum.OptionsView.ColumnAutoWidth = false;
            this.GrdDateWiseSum.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.GrdDateWiseSum.OptionsView.ShowFooter = true;
            this.GrdDateWiseSum.OptionsView.ShowGroupPanel = false;
            this.GrdDateWiseSum.RowHeight = 25;
            this.GrdDateWiseSum.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.GrdDateWiseSum_RowCellClick);
            this.GrdDateWiseSum.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.GrdDateWiseSum_FocusedRowChanged);
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "Trans Date";
            this.gridColumn3.FieldName = "TRANSDATE";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn3.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            this.gridColumn3.Width = 125;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Font = new System.Drawing.Font("Tahoma", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.gridColumn4.AppearanceCell.Options.UseFont = true;
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "Pcs";
            this.gridColumn4.FieldName = "PCS";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn4.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 1;
            this.gridColumn4.Width = 80;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // FrmStockUpload
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1366, 665);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.PnlHeader);
            this.Name = "FrmStockUpload";
            this.Tag = "StockUpload\r\n";
            this.Text = "STOCK UPLOAD";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmStockUpload_FormClosing);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmPricing_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.FrmStockUpload_KeyUp);
            ((System.ComponentModel.ISupportInitialize)(this.MainGrdStock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDetStock)).EndInit();
            this.PnlHeader.ResumeLayout(false);
            this.PnlHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrpRFIDBox)).EndInit();
            this.GrpRFIDBox.ResumeLayout(false);
            this.GrpRFIDBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrpStockSync)).EndInit();
            this.GrpStockSync.ResumeLayout(false);
            this.GrpStockSync.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GrpExcRateDetail)).EndInit();
            this.GrpExcRateDetail.ResumeLayout(false);
            this.GrpExcRateDetail.PerformLayout();
            this.PnlFooter.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrdDateWiseSum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDateWiseSum)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl MainGrdStock;
        private AxonContLib.cPanel PnlHeader;
        private DevControlLib.cDevSimpleButton BtnAdd;
        private DevControlLib.cDevSimpleButton BtnBack;
        private AxonContLib.cLabel cLabel1;
        private AxonContLib.cTextBox txtPartyName;
        private AxonContLib.cTextBox txtFileName;
        private DevControlLib.cDevSimpleButton BtnBrowse;
        private AxonContLib.cLabel cLabel2;
        private AxonContLib.cLabel cLabel3;
        private DevControlLib.cDevSimpleButton BtnCalculate;
        private DevControlLib.cDevSimpleButton BtnDelStonePricing;
        private AxonContLib.cLabel lblMessage;
        private AxonContLib.cPanel PnlFooter;
        private DevControlLib.cDevSimpleButton BtnBestFit;
        private AxonContLib.cLabel lblSampleExcelFile;
        private AxonContLib.cRadioButton RbtReplaceAllStock;
        private AxonContLib.cRadioButton RbtAppendStock;
        private AxonContLib.cPanel panel1;
        private System.Windows.Forms.ComboBox CmbStockType;
        private AxonContLib.cLabel cLabel4;
        private System.Windows.Forms.ComboBox CmbStockStatus;
        private AxonContLib.cLabel cLabel5;
        private AxonContLib.cLabel cLabel6;
        private AxonContLib.cComboBox CmbCurrency;
        private AxonContLib.cTextBox txtExcRate;
        private AxonContLib.cLabel cLabel35;
        private AxonContLib.cLabel cLabel34;
        private DevControlLib.cDevGroupControl GrpExcRateDetail;
        private AxonContLib.cLabel cLabel7;
        private DevControlLib.cDevSimpleButton BtnStockSync;
        private DevControlLib.cDevGroupControl GrpStockSync;
        private AxonContLib.cLabel cLabel8;
        private AxonContLib.cLabel cLabel9;
        private AxonContLib.cLabel cLabel10;
        private DevControlLib.cDevSimpleButton BtnVerified;
        private AxonContLib.cTextBox txtJangedNo;
        private AxonContLib.cDateTimePicker DTPSyncFromDate;
        private AxonContLib.cDateTimePicker DTPSyncToDate;
        private AxonContLib.cLabel cLabel28;
        private DevControlLib.cDevSimpleButton BtnClearFilter;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private AxonContLib.cLabel cLabel12;
        private DevExpress.XtraGrid.GridControl MainGrdDateWiseSum;
        private DevExpress.XtraGrid.Views.Grid.GridView GrdDateWiseSum;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private AxonContLib.cPanel panel2;
        private AxonContLib.cTextBox txtTotalCarat;
        private AxonContLib.cLabel cLabel25;
        private AxonContLib.cTextBox txtTotalDisc;
        private AxonContLib.cLabel cLabel23;
        private AxonContLib.cTextBox txtTotalPricePerCarat;
        private AxonContLib.cLabel cLabel21;
        private AxonContLib.cTextBox txtTotalAmount;
        private AxonContLib.cLabel cLabel16;
        private AxonContLib.cLabel cLabel15;
        private AxonContLib.cTextBox txtTotalAvgRap;
        private AxonContLib.cLabel cLabel22;
        private AxonContLib.cLabel cLabel13;
        private AxonContLib.cLabel cLabel26;
        private AxonContLib.cLabel cLabel24;
        private AxonContLib.cTextBox txtSelectedCarat;
        private AxonContLib.cLabel cLabel14;
        private AxonContLib.cTextBox txtSelectedDisc;
        private AxonContLib.cLabel cLabel20;
        private AxonContLib.cTextBox txtSelectedPricePerCarat;
        private AxonContLib.cTextBox txtSelectedAmount;
        private AxonContLib.cTextBox txtSelectedAvgRap;
        private AxonContLib.cLabel cLabel19;
        private AxonContLib.cRadioButton RbtStoneNo;
        private AxonContLib.cRadioButton RbtJangedNo;
        private AxonContLib.cLabel cLabel27;
        private AxonContLib.cLabel cLabel40;
        private AxonContLib.cLabel cLabel38;
        private AxonContLib.cLabel cLabel11;
        private AxonContLib.cLabel cLabel32;
        private AxonContLib.cLabel lblNone;
        private AxonContLib.cLabel lblInvoice;
        private AxonContLib.cLabel lblSold;
        private AxonContLib.cLabel lblMemo;
        private AxonContLib.cLabel lblAvailable;
        private AxonContLib.cLabel cLabel29;
        private AxonContLib.cLabel lblPurchaseReturn;
        private DevControlLib.cDevGroupControl GrpRFIDBox;
        private AxonContLib.cLabel cLabel30;
        private DevControlLib.cDevSimpleButton BtnLEDAllAtOnce;
        private DevControlLib.cDevSimpleButton BtnScan;
        private AxonContLib.cLabel cLabel37;
        private AxonContLib.cLabel cLabel36;
        private AxonContLib.cLabel cLabel33;
        private AxonContLib.cLabel cLabel31;
        private AxonContLib.cLabel lblTotal;
        private AxonContLib.cLabel lblUnMatched;
        private AxonContLib.cLabel lblMatched;
        private DevControlLib.cDevSimpleButton BtnStop;
        private AxonContLib.cLabel cLabel39;
        private AxonContLib.cLabel lblDeviceName;
        private DevControlLib.cDevSimpleButton BtnFindAndConnect;
        private AxonContLib.cRadioButton RbtGIAControlNo;
        private DevControlLib.cDevSimpleButton BtnLeft;
        private AxonContLib.cComboBox cmbPrdType;
        private AxonContLib.cLabel cLabel41;
        private AxonContLib.cLabel lblTime;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private DevExpress.XtraWaitForm.ProgressPanel progressPanel1;
        private DevExpress.XtraGrid.Views.Grid.GridView GrdDetStock;
        private AxonContLib.cComboBox cmbBillType;
        private AxonContLib.cLabel lblBillType;
        private AxonContLib.cComboBox CmbSheetName;
        private AxonContLib.cLabel lblDefaultLayout;
        private AxonContLib.cLabel lblSaveLayout;
    }
}