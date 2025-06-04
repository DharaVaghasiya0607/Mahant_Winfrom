namespace MahantExport.Masters
{
    partial class FrmMemoReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMemoReport));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteSelectedAmountToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BtnExport = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnSearch = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnBack = new DevControlLib.cDevSimpleButton(this.components);
            this.panel4 = new AxonContLib.cPanel(this.components);
            this.RbtPending = new AxonContLib.cRadioButton(this.components);
            this.RbtAll = new AxonContLib.cRadioButton(this.components);
            this.RbtCompleted = new AxonContLib.cRadioButton(this.components);
            this.RbtPartial = new AxonContLib.cRadioButton(this.components);
            this.DTPFromDate = new AxonContLib.cDateTimePicker(this.components);
            this.DTPToDate = new AxonContLib.cDateTimePicker(this.components);
            this.cLabel4 = new AxonContLib.cLabel(this.components);
            this.cLabel3 = new AxonContLib.cLabel(this.components);
            this.txtMemoNo = new AxonContLib.cTextBox(this.components);
            this.cLabel5 = new AxonContLib.cLabel(this.components);
            this.cLabel6 = new AxonContLib.cLabel(this.components);
            this.cLabel2 = new AxonContLib.cLabel(this.components);
            this.txtStoneNo = new AxonContLib.cTextBox(this.components);
            this.txtShipTo = new AxonContLib.cTextBox(this.components);
            this.txtShipToCountry = new AxonContLib.cTextBox(this.components);
            this.txtBillToCountry = new AxonContLib.cTextBox(this.components);
            this.txtSeller = new AxonContLib.cTextBox(this.components);
            this.txtBillTo = new AxonContLib.cTextBox(this.components);
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            this.cLabel8 = new AxonContLib.cLabel(this.components);
            this.PivotGridDet = new DevExpress.XtraPivotGrid.PivotGridControl();
            this.pivotGridField1 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField2 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField3 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField4 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField6 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField7 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField5 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField8 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField9 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField10 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField11 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.contextMenuStrip1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PivotGridDet)).BeginInit();
            this.SuspendLayout();
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
            // 
            // BtnExport
            // 
            this.BtnExport.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnExport.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnExport.Appearance.Options.UseFont = true;
            this.BtnExport.Appearance.Options.UseForeColor = true;
            this.BtnExport.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnExport.ImageOptions.SvgImage")));
            this.BtnExport.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnExport.Location = new System.Drawing.Point(809, 55);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(73, 33);
            this.BtnExport.TabIndex = 33;
            this.BtnExport.TabStop = false;
            this.BtnExport.Text = "Export";
            this.BtnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // BtnSearch
            // 
            this.BtnSearch.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSearch.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnSearch.Appearance.Options.UseFont = true;
            this.BtnSearch.Appearance.Options.UseForeColor = true;
            this.BtnSearch.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSearch.ImageOptions.SvgImage")));
            this.BtnSearch.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnSearch.Location = new System.Drawing.Point(730, 55);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(73, 33);
            this.BtnSearch.TabIndex = 4;
            this.BtnSearch.Text = "&Search";
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // BtnBack
            // 
            this.BtnBack.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnBack.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnBack.Appearance.Options.UseFont = true;
            this.BtnBack.Appearance.Options.UseForeColor = true;
            this.BtnBack.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnBack.ImageOptions.SvgImage")));
            this.BtnBack.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnBack.Location = new System.Drawing.Point(885, 55);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.Size = new System.Drawing.Size(73, 33);
            this.BtnBack.TabIndex = 32;
            this.BtnBack.TabStop = false;
            this.BtnBack.Text = "E&xit";
            this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.RbtPending);
            this.panel4.Controls.Add(this.RbtAll);
            this.panel4.Controls.Add(this.RbtCompleted);
            this.panel4.Controls.Add(this.RbtPartial);
            this.panel4.Controls.Add(this.DTPFromDate);
            this.panel4.Controls.Add(this.DTPToDate);
            this.panel4.Controls.Add(this.cLabel4);
            this.panel4.Controls.Add(this.cLabel3);
            this.panel4.Controls.Add(this.txtMemoNo);
            this.panel4.Controls.Add(this.cLabel5);
            this.panel4.Controls.Add(this.cLabel6);
            this.panel4.Controls.Add(this.cLabel2);
            this.panel4.Controls.Add(this.txtStoneNo);
            this.panel4.Controls.Add(this.txtShipTo);
            this.panel4.Controls.Add(this.txtShipToCountry);
            this.panel4.Controls.Add(this.txtBillToCountry);
            this.panel4.Controls.Add(this.txtSeller);
            this.panel4.Controls.Add(this.txtBillTo);
            this.panel4.Controls.Add(this.cLabel1);
            this.panel4.Controls.Add(this.cLabel8);
            this.panel4.Controls.Add(this.BtnExport);
            this.panel4.Controls.Add(this.BtnBack);
            this.panel4.Controls.Add(this.BtnSearch);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1121, 98);
            this.panel4.TabIndex = 0;
            // 
            // RbtPending
            // 
            this.RbtPending.AllowTabKeyOnEnter = false;
            this.RbtPending.AutoSize = true;
            this.RbtPending.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbtPending.ForeColor = System.Drawing.Color.Black;
            this.RbtPending.Location = new System.Drawing.Point(981, 7);
            this.RbtPending.Name = "RbtPending";
            this.RbtPending.Size = new System.Drawing.Size(78, 18);
            this.RbtPending.TabIndex = 156;
            this.RbtPending.Text = "Pending";
            this.RbtPending.ToolTips = "";
            this.RbtPending.UseVisualStyleBackColor = true;
            this.RbtPending.Visible = false;
            // 
            // RbtAll
            // 
            this.RbtAll.AllowTabKeyOnEnter = false;
            this.RbtAll.AutoSize = true;
            this.RbtAll.Checked = true;
            this.RbtAll.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbtAll.ForeColor = System.Drawing.Color.Black;
            this.RbtAll.Location = new System.Drawing.Point(910, 7);
            this.RbtAll.Name = "RbtAll";
            this.RbtAll.Size = new System.Drawing.Size(42, 18);
            this.RbtAll.TabIndex = 156;
            this.RbtAll.TabStop = true;
            this.RbtAll.Text = "All";
            this.RbtAll.ToolTips = "";
            this.RbtAll.UseVisualStyleBackColor = true;
            this.RbtAll.Visible = false;
            // 
            // RbtCompleted
            // 
            this.RbtCompleted.AllowTabKeyOnEnter = false;
            this.RbtCompleted.AutoSize = true;
            this.RbtCompleted.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbtCompleted.ForeColor = System.Drawing.Color.Black;
            this.RbtCompleted.Location = new System.Drawing.Point(981, 31);
            this.RbtCompleted.Name = "RbtCompleted";
            this.RbtCompleted.Size = new System.Drawing.Size(87, 18);
            this.RbtCompleted.TabIndex = 156;
            this.RbtCompleted.Text = "Complete";
            this.RbtCompleted.ToolTips = "";
            this.RbtCompleted.UseVisualStyleBackColor = true;
            this.RbtCompleted.Visible = false;
            // 
            // RbtPartial
            // 
            this.RbtPartial.AllowTabKeyOnEnter = false;
            this.RbtPartial.AutoSize = true;
            this.RbtPartial.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbtPartial.ForeColor = System.Drawing.Color.Black;
            this.RbtPartial.Location = new System.Drawing.Point(910, 31);
            this.RbtPartial.Name = "RbtPartial";
            this.RbtPartial.Size = new System.Drawing.Size(69, 18);
            this.RbtPartial.TabIndex = 156;
            this.RbtPartial.Text = "Partial";
            this.RbtPartial.ToolTips = "";
            this.RbtPartial.UseVisualStyleBackColor = true;
            this.RbtPartial.Visible = false;
            // 
            // DTPFromDate
            // 
            this.DTPFromDate.AllowTabKeyOnEnter = false;
            this.DTPFromDate.CustomFormat = "dd/MM/yyyy";
            this.DTPFromDate.Font = new System.Drawing.Font("Verdana", 9F);
            this.DTPFromDate.ForeColor = System.Drawing.Color.Black;
            this.DTPFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTPFromDate.Location = new System.Drawing.Point(88, 9);
            this.DTPFromDate.Name = "DTPFromDate";
            this.DTPFromDate.Size = new System.Drawing.Size(102, 22);
            this.DTPFromDate.TabIndex = 0;
            this.DTPFromDate.ToolTips = "";
            // 
            // DTPToDate
            // 
            this.DTPToDate.AllowTabKeyOnEnter = false;
            this.DTPToDate.CustomFormat = "dd/MM/yyyy";
            this.DTPToDate.Font = new System.Drawing.Font("Verdana", 9F);
            this.DTPToDate.ForeColor = System.Drawing.Color.Black;
            this.DTPToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTPToDate.Location = new System.Drawing.Point(222, 9);
            this.DTPToDate.Name = "DTPToDate";
            this.DTPToDate.Size = new System.Drawing.Size(102, 22);
            this.DTPToDate.TabIndex = 1;
            this.DTPToDate.ToolTips = "";
            // 
            // cLabel4
            // 
            this.cLabel4.AutoSize = true;
            this.cLabel4.Font = new System.Drawing.Font("Verdana", 9F);
            this.cLabel4.ForeColor = System.Drawing.Color.Black;
            this.cLabel4.Location = new System.Drawing.Point(330, 13);
            this.cLabel4.Name = "cLabel4";
            this.cLabel4.Size = new System.Drawing.Size(73, 14);
            this.cLabel4.TabIndex = 39;
            this.cLabel4.Text = "Invoice No";
            this.cLabel4.ToolTips = "";
            // 
            // cLabel3
            // 
            this.cLabel3.AutoSize = true;
            this.cLabel3.Font = new System.Drawing.Font("Verdana", 9F);
            this.cLabel3.ForeColor = System.Drawing.Color.Black;
            this.cLabel3.Location = new System.Drawing.Point(429, 41);
            this.cLabel3.Name = "cLabel3";
            this.cLabel3.Size = new System.Drawing.Size(61, 14);
            this.cLabel3.TabIndex = 39;
            this.cLabel3.Text = "StoneNo";
            this.cLabel3.ToolTips = "";
            // 
            // txtMemoNo
            // 
            this.txtMemoNo.ActivationColor = true;
            this.txtMemoNo.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtMemoNo.AllowTabKeyOnEnter = false;
            this.txtMemoNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMemoNo.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtMemoNo.Format = "";
            this.txtMemoNo.IsComplusory = false;
            this.txtMemoNo.Location = new System.Drawing.Point(405, 9);
            this.txtMemoNo.Name = "txtMemoNo";
            this.txtMemoNo.SelectAllTextOnFocus = true;
            this.txtMemoNo.Size = new System.Drawing.Size(211, 22);
            this.txtMemoNo.TabIndex = 3;
            this.txtMemoNo.ToolTips = "";
            this.txtMemoNo.WaterMarkText = null;
            // 
            // cLabel5
            // 
            this.cLabel5.AutoSize = true;
            this.cLabel5.Font = new System.Drawing.Font("Verdana", 9F);
            this.cLabel5.ForeColor = System.Drawing.Color.Black;
            this.cLabel5.Location = new System.Drawing.Point(15, 76);
            this.cLabel5.Name = "cLabel5";
            this.cLabel5.Size = new System.Drawing.Size(52, 14);
            this.cLabel5.TabIndex = 39;
            this.cLabel5.Text = "Ship To";
            this.cLabel5.ToolTips = "";
            this.cLabel5.Visible = false;
            // 
            // cLabel6
            // 
            this.cLabel6.AutoSize = true;
            this.cLabel6.Font = new System.Drawing.Font("Verdana", 9F);
            this.cLabel6.ForeColor = System.Drawing.Color.Black;
            this.cLabel6.Location = new System.Drawing.Point(620, 13);
            this.cLabel6.Name = "cLabel6";
            this.cLabel6.Size = new System.Drawing.Size(42, 14);
            this.cLabel6.TabIndex = 39;
            this.cLabel6.Text = "Seller";
            this.cLabel6.ToolTips = "";
            // 
            // cLabel2
            // 
            this.cLabel2.AutoSize = true;
            this.cLabel2.Font = new System.Drawing.Font("Verdana", 9F);
            this.cLabel2.ForeColor = System.Drawing.Color.Black;
            this.cLabel2.Location = new System.Drawing.Point(15, 46);
            this.cLabel2.Name = "cLabel2";
            this.cLabel2.Size = new System.Drawing.Size(40, 14);
            this.cLabel2.TabIndex = 39;
            this.cLabel2.Text = "Party";
            this.cLabel2.ToolTips = "";
            // 
            // txtStoneNo
            // 
            this.txtStoneNo.ActivationColor = true;
            this.txtStoneNo.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtStoneNo.AllowTabKeyOnEnter = false;
            this.txtStoneNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStoneNo.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtStoneNo.Format = "";
            this.txtStoneNo.IsComplusory = false;
            this.txtStoneNo.Location = new System.Drawing.Point(490, 39);
            this.txtStoneNo.Multiline = true;
            this.txtStoneNo.Name = "txtStoneNo";
            this.txtStoneNo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStoneNo.SelectAllTextOnFocus = true;
            this.txtStoneNo.Size = new System.Drawing.Size(225, 49);
            this.txtStoneNo.TabIndex = 3;
            this.txtStoneNo.ToolTips = "";
            this.txtStoneNo.WaterMarkText = null;
            // 
            // txtShipTo
            // 
            this.txtShipTo.ActivationColor = true;
            this.txtShipTo.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtShipTo.AllowTabKeyOnEnter = false;
            this.txtShipTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShipTo.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtShipTo.Format = "";
            this.txtShipTo.IsComplusory = false;
            this.txtShipTo.Location = new System.Drawing.Point(72, 72);
            this.txtShipTo.Name = "txtShipTo";
            this.txtShipTo.SelectAllTextOnFocus = true;
            this.txtShipTo.Size = new System.Drawing.Size(217, 22);
            this.txtShipTo.TabIndex = 3;
            this.txtShipTo.ToolTips = "";
            this.txtShipTo.Visible = false;
            this.txtShipTo.WaterMarkText = null;
            this.txtShipTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtShipTo_KeyPress);
            // 
            // txtShipToCountry
            // 
            this.txtShipToCountry.ActivationColor = true;
            this.txtShipToCountry.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtShipToCountry.AllowTabKeyOnEnter = false;
            this.txtShipToCountry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtShipToCountry.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtShipToCountry.Format = "";
            this.txtShipToCountry.IsComplusory = false;
            this.txtShipToCountry.Location = new System.Drawing.Point(295, 72);
            this.txtShipToCountry.Name = "txtShipToCountry";
            this.txtShipToCountry.SelectAllTextOnFocus = true;
            this.txtShipToCountry.Size = new System.Drawing.Size(128, 22);
            this.txtShipToCountry.TabIndex = 3;
            this.txtShipToCountry.ToolTips = "";
            this.txtShipToCountry.Visible = false;
            this.txtShipToCountry.WaterMarkText = null;
            this.txtShipToCountry.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtShipToCountry_KeyPress);
            // 
            // txtBillToCountry
            // 
            this.txtBillToCountry.ActivationColor = true;
            this.txtBillToCountry.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtBillToCountry.AllowTabKeyOnEnter = false;
            this.txtBillToCountry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBillToCountry.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtBillToCountry.Format = "";
            this.txtBillToCountry.IsComplusory = false;
            this.txtBillToCountry.Location = new System.Drawing.Point(295, 42);
            this.txtBillToCountry.Name = "txtBillToCountry";
            this.txtBillToCountry.SelectAllTextOnFocus = true;
            this.txtBillToCountry.Size = new System.Drawing.Size(128, 22);
            this.txtBillToCountry.TabIndex = 3;
            this.txtBillToCountry.ToolTips = "";
            this.txtBillToCountry.WaterMarkText = null;
            this.txtBillToCountry.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBillToCountry_KeyPress);
            // 
            // txtSeller
            // 
            this.txtSeller.ActivationColor = true;
            this.txtSeller.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtSeller.AllowTabKeyOnEnter = false;
            this.txtSeller.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSeller.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtSeller.Format = "";
            this.txtSeller.IsComplusory = false;
            this.txtSeller.Location = new System.Drawing.Point(664, 9);
            this.txtSeller.Name = "txtSeller";
            this.txtSeller.SelectAllTextOnFocus = true;
            this.txtSeller.Size = new System.Drawing.Size(229, 22);
            this.txtSeller.TabIndex = 3;
            this.txtSeller.ToolTips = "";
            this.txtSeller.WaterMarkText = null;
            this.txtSeller.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSeller_KeyPress);
            // 
            // txtBillTo
            // 
            this.txtBillTo.ActivationColor = true;
            this.txtBillTo.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtBillTo.AllowTabKeyOnEnter = false;
            this.txtBillTo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBillTo.Font = new System.Drawing.Font("Verdana", 9F);
            this.txtBillTo.Format = "";
            this.txtBillTo.IsComplusory = false;
            this.txtBillTo.Location = new System.Drawing.Point(72, 42);
            this.txtBillTo.Name = "txtBillTo";
            this.txtBillTo.SelectAllTextOnFocus = true;
            this.txtBillTo.Size = new System.Drawing.Size(217, 22);
            this.txtBillTo.TabIndex = 3;
            this.txtBillTo.ToolTips = "";
            this.txtBillTo.WaterMarkText = null;
            this.txtBillTo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBillTo_KeyPress);
            // 
            // cLabel1
            // 
            this.cLabel1.AutoSize = true;
            this.cLabel1.Font = new System.Drawing.Font("Verdana", 9F);
            this.cLabel1.ForeColor = System.Drawing.Color.Black;
            this.cLabel1.Location = new System.Drawing.Point(195, 13);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(21, 14);
            this.cLabel1.TabIndex = 37;
            this.cLabel1.Text = "To";
            this.cLabel1.ToolTips = "";
            // 
            // cLabel8
            // 
            this.cLabel8.AutoSize = true;
            this.cLabel8.Font = new System.Drawing.Font("Verdana", 9F);
            this.cLabel8.ForeColor = System.Drawing.Color.Black;
            this.cLabel8.Location = new System.Drawing.Point(15, 13);
            this.cLabel8.Name = "cLabel8";
            this.cLabel8.Size = new System.Drawing.Size(68, 14);
            this.cLabel8.TabIndex = 35;
            this.cLabel8.Text = "FromDate";
            this.cLabel8.ToolTips = "";
            // 
            // PivotGridDet
            // 
            this.PivotGridDet.Appearance.Lines.BackColor = System.Drawing.Color.Gray;
            this.PivotGridDet.Appearance.Lines.BackColor2 = System.Drawing.Color.Gray;
            this.PivotGridDet.Appearance.Lines.Options.UseBackColor = true;
            this.PivotGridDet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PivotGridDet.Fields.AddRange(new DevExpress.XtraPivotGrid.PivotGridField[] {
            this.pivotGridField1,
            this.pivotGridField2,
            this.pivotGridField3,
            this.pivotGridField4,
            this.pivotGridField6,
            this.pivotGridField7,
            this.pivotGridField5,
            this.pivotGridField8,
            this.pivotGridField9,
            this.pivotGridField10,
            this.pivotGridField11});
            this.PivotGridDet.Location = new System.Drawing.Point(0, 98);
            this.PivotGridDet.Name = "PivotGridDet";
            this.PivotGridDet.Size = new System.Drawing.Size(1121, 313);
            this.PivotGridDet.TabIndex = 1;
            // 
            // pivotGridField1
            // 
            this.pivotGridField1.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.pivotGridField1.AreaIndex = 0;
            this.pivotGridField1.Caption = "BILLINGPARTY_ID";
            this.pivotGridField1.FieldName = "BILLINGPARTY_ID";
            this.pivotGridField1.Name = "pivotGridField1";
            this.pivotGridField1.Visible = false;
            // 
            // pivotGridField2
            // 
            this.pivotGridField2.Appearance.Cell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField2.Appearance.Cell.Options.UseFont = true;
            this.pivotGridField2.Appearance.CellGrandTotal.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField2.Appearance.CellGrandTotal.Options.UseFont = true;
            this.pivotGridField2.Appearance.CellTotal.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField2.Appearance.CellTotal.Options.UseFont = true;
            this.pivotGridField2.Appearance.Header.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField2.Appearance.Header.Options.UseFont = true;
            this.pivotGridField2.Appearance.Value.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField2.Appearance.Value.Options.UseFont = true;
            this.pivotGridField2.Appearance.ValueGrandTotal.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField2.Appearance.ValueGrandTotal.Options.UseFont = true;
            this.pivotGridField2.Appearance.ValueTotal.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField2.Appearance.ValueTotal.Options.UseFont = true;
            this.pivotGridField2.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.pivotGridField2.AreaIndex = 1;
            this.pivotGridField2.Caption = "Invoice Party";
            this.pivotGridField2.FieldName = "BILLINGPARTYNAME";
            this.pivotGridField2.Name = "pivotGridField2";
            this.pivotGridField2.Width = 150;
            // 
            // pivotGridField3
            // 
            this.pivotGridField3.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.pivotGridField3.AreaIndex = 1;
            this.pivotGridField3.Caption = "SHIPPINGPARTY_ID";
            this.pivotGridField3.FieldName = "SHIPPINGPARTY_ID";
            this.pivotGridField3.Name = "pivotGridField3";
            this.pivotGridField3.Visible = false;
            // 
            // pivotGridField4
            // 
            this.pivotGridField4.Appearance.Cell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField4.Appearance.Cell.Options.UseFont = true;
            this.pivotGridField4.Appearance.CellGrandTotal.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField4.Appearance.CellGrandTotal.Options.UseFont = true;
            this.pivotGridField4.Appearance.CellTotal.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField4.Appearance.CellTotal.Options.UseFont = true;
            this.pivotGridField4.Appearance.Header.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField4.Appearance.Header.Options.UseFont = true;
            this.pivotGridField4.Appearance.Value.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField4.Appearance.Value.Options.UseFont = true;
            this.pivotGridField4.Appearance.ValueGrandTotal.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField4.Appearance.ValueGrandTotal.Options.UseFont = true;
            this.pivotGridField4.Appearance.ValueTotal.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField4.Appearance.ValueTotal.Options.UseFont = true;
            this.pivotGridField4.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.pivotGridField4.AreaIndex = 2;
            this.pivotGridField4.Caption = "Ship Party";
            this.pivotGridField4.FieldName = "SHIPPINGPARTYNAME";
            this.pivotGridField4.Name = "pivotGridField4";
            this.pivotGridField4.Visible = false;
            this.pivotGridField4.Width = 150;
            // 
            // pivotGridField6
            // 
            this.pivotGridField6.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.pivotGridField6.AreaIndex = 3;
            this.pivotGridField6.Caption = "SELLER_ID";
            this.pivotGridField6.FieldName = "SELLER_ID";
            this.pivotGridField6.Name = "pivotGridField6";
            this.pivotGridField6.Visible = false;
            // 
            // pivotGridField7
            // 
            this.pivotGridField7.Appearance.Cell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField7.Appearance.Cell.Options.UseFont = true;
            this.pivotGridField7.Appearance.CellGrandTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField7.Appearance.CellGrandTotal.Options.UseFont = true;
            this.pivotGridField7.Appearance.CellTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField7.Appearance.CellTotal.Options.UseFont = true;
            this.pivotGridField7.Appearance.Header.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField7.Appearance.Header.Options.UseFont = true;
            this.pivotGridField7.Appearance.Value.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField7.Appearance.Value.Options.UseFont = true;
            this.pivotGridField7.Appearance.ValueGrandTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField7.Appearance.ValueGrandTotal.Options.UseFont = true;
            this.pivotGridField7.Appearance.ValueTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField7.Appearance.ValueTotal.Options.UseFont = true;
            this.pivotGridField7.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.pivotGridField7.AreaIndex = 0;
            this.pivotGridField7.Caption = "Seller";
            this.pivotGridField7.FieldName = "SELLERNAME";
            this.pivotGridField7.Name = "pivotGridField7";
            this.pivotGridField7.Width = 150;
            // 
            // pivotGridField5
            // 
            this.pivotGridField5.Appearance.Cell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField5.Appearance.Cell.Options.UseFont = true;
            this.pivotGridField5.Appearance.CellGrandTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField5.Appearance.CellGrandTotal.Options.UseFont = true;
            this.pivotGridField5.Appearance.CellTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField5.Appearance.CellTotal.Options.UseFont = true;
            this.pivotGridField5.Appearance.Header.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField5.Appearance.Header.Options.UseFont = true;
            this.pivotGridField5.Appearance.Value.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField5.Appearance.Value.Options.UseFont = true;
            this.pivotGridField5.Appearance.ValueGrandTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField5.Appearance.ValueGrandTotal.Options.UseFont = true;
            this.pivotGridField5.Appearance.ValueTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField5.Appearance.ValueTotal.Options.UseFont = true;
            this.pivotGridField5.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.pivotGridField5.AreaIndex = 0;
            this.pivotGridField5.Caption = "Process";
            this.pivotGridField5.FieldName = "PROCESSNAME";
            this.pivotGridField5.Name = "pivotGridField5";
            // 
            // pivotGridField8
            // 
            this.pivotGridField8.Appearance.Cell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.pivotGridField8.Appearance.Cell.Options.UseFont = true;
            this.pivotGridField8.Appearance.CellGrandTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField8.Appearance.CellGrandTotal.Options.UseFont = true;
            this.pivotGridField8.Appearance.CellTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField8.Appearance.CellTotal.Options.UseFont = true;
            this.pivotGridField8.Appearance.Header.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField8.Appearance.Header.Options.UseFont = true;
            this.pivotGridField8.Appearance.Value.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField8.Appearance.Value.Options.UseFont = true;
            this.pivotGridField8.Appearance.Value.Options.UseTextOptions = true;
            this.pivotGridField8.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField8.Appearance.ValueGrandTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField8.Appearance.ValueGrandTotal.Options.UseFont = true;
            this.pivotGridField8.Appearance.ValueTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField8.Appearance.ValueTotal.Options.UseFont = true;
            this.pivotGridField8.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.pivotGridField8.AreaIndex = 0;
            this.pivotGridField8.Caption = "Cnt";
            this.pivotGridField8.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.pivotGridField8.FieldName = "CNT";
            this.pivotGridField8.Name = "pivotGridField8";
            this.pivotGridField8.Width = 34;
            // 
            // pivotGridField9
            // 
            this.pivotGridField9.Appearance.Cell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.pivotGridField9.Appearance.Cell.Options.UseFont = true;
            this.pivotGridField9.Appearance.CellGrandTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField9.Appearance.CellGrandTotal.Options.UseFont = true;
            this.pivotGridField9.Appearance.CellTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField9.Appearance.CellTotal.Options.UseFont = true;
            this.pivotGridField9.Appearance.Header.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField9.Appearance.Header.Options.UseFont = true;
            this.pivotGridField9.Appearance.Value.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField9.Appearance.Value.Options.UseFont = true;
            this.pivotGridField9.Appearance.Value.Options.UseTextOptions = true;
            this.pivotGridField9.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField9.Appearance.ValueGrandTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField9.Appearance.ValueGrandTotal.Options.UseFont = true;
            this.pivotGridField9.Appearance.ValueTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField9.Appearance.ValueTotal.Options.UseFont = true;
            this.pivotGridField9.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.pivotGridField9.AreaIndex = 1;
            this.pivotGridField9.Caption = "Pcs";
            this.pivotGridField9.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.pivotGridField9.FieldName = "TOTALPCS";
            this.pivotGridField9.Name = "pivotGridField9";
            this.pivotGridField9.Width = 47;
            // 
            // pivotGridField10
            // 
            this.pivotGridField10.Appearance.Cell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.pivotGridField10.Appearance.Cell.Options.UseFont = true;
            this.pivotGridField10.Appearance.CellGrandTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField10.Appearance.CellGrandTotal.Options.UseFont = true;
            this.pivotGridField10.Appearance.CellTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField10.Appearance.CellTotal.Options.UseFont = true;
            this.pivotGridField10.Appearance.Header.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField10.Appearance.Header.Options.UseFont = true;
            this.pivotGridField10.Appearance.Value.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField10.Appearance.Value.Options.UseFont = true;
            this.pivotGridField10.Appearance.Value.Options.UseTextOptions = true;
            this.pivotGridField10.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField10.Appearance.ValueGrandTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField10.Appearance.ValueGrandTotal.Options.UseFont = true;
            this.pivotGridField10.Appearance.ValueTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField10.Appearance.ValueTotal.Options.UseFont = true;
            this.pivotGridField10.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.pivotGridField10.AreaIndex = 2;
            this.pivotGridField10.Caption = "Cts";
            this.pivotGridField10.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.pivotGridField10.FieldName = "TOTALCARAT";
            this.pivotGridField10.Name = "pivotGridField10";
            this.pivotGridField10.Width = 68;
            // 
            // pivotGridField11
            // 
            this.pivotGridField11.Appearance.Cell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.pivotGridField11.Appearance.Cell.Options.UseFont = true;
            this.pivotGridField11.Appearance.CellGrandTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField11.Appearance.CellGrandTotal.Options.UseFont = true;
            this.pivotGridField11.Appearance.CellTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField11.Appearance.CellTotal.Options.UseFont = true;
            this.pivotGridField11.Appearance.Header.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField11.Appearance.Header.Options.UseFont = true;
            this.pivotGridField11.Appearance.Value.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField11.Appearance.Value.Options.UseFont = true;
            this.pivotGridField11.Appearance.Value.Options.UseTextOptions = true;
            this.pivotGridField11.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField11.Appearance.ValueGrandTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField11.Appearance.ValueGrandTotal.Options.UseFont = true;
            this.pivotGridField11.Appearance.ValueTotal.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.pivotGridField11.Appearance.ValueTotal.Options.UseFont = true;
            this.pivotGridField11.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.pivotGridField11.AreaIndex = 3;
            this.pivotGridField11.Caption = "Amt";
            this.pivotGridField11.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.pivotGridField11.FieldName = "NETAMOUNT";
            this.pivotGridField11.Name = "pivotGridField11";
            this.pivotGridField11.Width = 101;
            // 
            // FrmMemoReport
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1121, 411);
            this.Controls.Add(this.PivotGridDet);
            this.Controls.Add(this.panel4);
            this.Name = "FrmMemoReport";
            this.Tag = "FrmMemoReport";
            this.Text = "DETAIL REPORT";
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PivotGridDet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectedAmountToolStripMenuItem;
        private DevControlLib.cDevSimpleButton BtnExport;
        private DevControlLib.cDevSimpleButton BtnSearch;
        private DevControlLib.cDevSimpleButton BtnBack;
        private AxonContLib.cPanel panel4;
        private AxonContLib.cLabel cLabel8;
        private AxonContLib.cLabel cLabel1;
        private AxonContLib.cTextBox txtBillTo;
        private AxonContLib.cLabel cLabel2;
        private AxonContLib.cDateTimePicker DTPToDate;
        private AxonContLib.cDateTimePicker DTPFromDate;
        private AxonContLib.cLabel cLabel3;
        private AxonContLib.cTextBox txtStoneNo;
        private AxonContLib.cLabel cLabel4;
        private AxonContLib.cTextBox txtMemoNo;
        private AxonContLib.cLabel cLabel5;
        private AxonContLib.cTextBox txtShipTo;
        private AxonContLib.cTextBox txtShipToCountry;
        private AxonContLib.cTextBox txtBillToCountry;
        private AxonContLib.cLabel cLabel6;
        private AxonContLib.cTextBox txtSeller;
        private AxonContLib.cRadioButton RbtPending;
        private AxonContLib.cRadioButton RbtAll;
        private AxonContLib.cRadioButton RbtPartial;
        private AxonContLib.cRadioButton RbtCompleted;
        private DevExpress.XtraPivotGrid.PivotGridControl PivotGridDet;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField1;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField2;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField3;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField4;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField6;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField7;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField5;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField8;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField9;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField10;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField11;


    }
}