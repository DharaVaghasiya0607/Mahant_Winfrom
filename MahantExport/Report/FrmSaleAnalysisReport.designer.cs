namespace MahantExport.Report
{
    partial class FrmSaleAnalysisReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSaleAnalysisReport));
            DevExpress.XtraPivotGrid.DataSourceColumnBinding dataSourceColumnBinding1 = new DevExpress.XtraPivotGrid.DataSourceColumnBinding();
            DevExpress.XtraPivotGrid.DataSourceColumnBinding dataSourceColumnBinding2 = new DevExpress.XtraPivotGrid.DataSourceColumnBinding();
            DevExpress.XtraPivotGrid.DataSourceColumnBinding dataSourceColumnBinding3 = new DevExpress.XtraPivotGrid.DataSourceColumnBinding();
            DevExpress.XtraPivotGrid.DataSourceColumnBinding dataSourceColumnBinding4 = new DevExpress.XtraPivotGrid.DataSourceColumnBinding();
            DevExpress.XtraPivotGrid.DataSourceColumnBinding dataSourceColumnBinding5 = new DevExpress.XtraPivotGrid.DataSourceColumnBinding();
            DevExpress.XtraPivotGrid.DataSourceColumnBinding dataSourceColumnBinding6 = new DevExpress.XtraPivotGrid.DataSourceColumnBinding();
            DevExpress.XtraPivotGrid.DataSourceColumnBinding dataSourceColumnBinding7 = new DevExpress.XtraPivotGrid.DataSourceColumnBinding();
            DevExpress.XtraPivotGrid.DataSourceColumnBinding dataSourceColumnBinding8 = new DevExpress.XtraPivotGrid.DataSourceColumnBinding();
            DevExpress.XtraPivotGrid.DataSourceColumnBinding dataSourceColumnBinding9 = new DevExpress.XtraPivotGrid.DataSourceColumnBinding();
            this.BtnExport = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnBack = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnSearch = new DevControlLib.cDevSimpleButton(this.components);
            this.panel2 = new AxonContLib.cPanel(this.components);
            this.BtnClear = new DevControlLib.cDevSimpleButton(this.components);
            this.DtpToDate = new AxonContLib.cDateTimePicker(this.components);
            this.DtpFromDate = new AxonContLib.cDateTimePicker(this.components);
            this.label3 = new AxonContLib.cLabel(this.components);
            this.label5 = new AxonContLib.cLabel(this.components);
            this.pivotGrid = new DevExpress.XtraPivotGrid.PivotGridControl();
            this.pivotGridField2 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField3 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField4 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField5 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField6 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField8 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.pivotGridField7 = new DevExpress.XtraPivotGrid.PivotGridField();
            this.Carat = new DevExpress.XtraPivotGrid.PivotGridField();
            this.sale = new DevExpress.XtraPivotGrid.PivotGridField();
            this.repTxtRate = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pivotGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtRate)).BeginInit();
            this.SuspendLayout();
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
            this.BtnExport.Location = new System.Drawing.Point(102, 33);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(108, 35);
            this.BtnExport.TabIndex = 33;
            this.BtnExport.TabStop = false;
            this.BtnExport.Text = "Export";
            this.BtnExport.Click += new System.EventHandler(this.BtnExport_Click);
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
            this.BtnBack.Location = new System.Drawing.Point(216, 33);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.Size = new System.Drawing.Size(90, 35);
            this.BtnBack.TabIndex = 32;
            this.BtnBack.TabStop = false;
            this.BtnBack.Text = "E&xit";
            this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // BtnSearch
            // 
            this.BtnSearch.AllowFocus = false;
            this.BtnSearch.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnSearch.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnSearch.Appearance.Options.UseFont = true;
            this.BtnSearch.Appearance.Options.UseForeColor = true;
            this.BtnSearch.ImageOptions.Image = global::MahantExport.Properties.Resources.btnsearch;
            this.BtnSearch.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSearch.ImageOptions.SvgImage")));
            this.BtnSearch.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnSearch.Location = new System.Drawing.Point(6, 33);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(90, 35);
            this.BtnSearch.TabIndex = 1;
            this.BtnSearch.Text = "Search";
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.BtnClear);
            this.panel2.Controls.Add(this.DtpToDate);
            this.panel2.Controls.Add(this.DtpFromDate);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.BtnExport);
            this.panel2.Controls.Add(this.BtnBack);
            this.panel2.Controls.Add(this.BtnSearch);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(795, 80);
            this.panel2.TabIndex = 40;
            // 
            // BtnClear
            // 
            this.BtnClear.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnClear.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnClear.Appearance.Options.UseFont = true;
            this.BtnClear.Appearance.Options.UseForeColor = true;
            this.BtnClear.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnClear.ImageOptions.Image")));
            this.BtnClear.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnClear.ImageOptions.SvgImage")));
            this.BtnClear.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnClear.Location = new System.Drawing.Point(312, 33);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(75, 35);
            this.BtnClear.TabIndex = 201;
            this.BtnClear.Text = "&Clear";
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // DtpToDate
            // 
            this.DtpToDate.AllowTabKeyOnEnter = false;
            this.DtpToDate.Checked = false;
            this.DtpToDate.CustomFormat = "dd/MM/yyyy";
            this.DtpToDate.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpToDate.ForeColor = System.Drawing.Color.Black;
            this.DtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpToDate.Location = new System.Drawing.Point(255, 5);
            this.DtpToDate.Name = "DtpToDate";
            this.DtpToDate.ShowCheckBox = true;
            this.DtpToDate.Size = new System.Drawing.Size(144, 22);
            this.DtpToDate.TabIndex = 176;
            this.DtpToDate.ToolTips = "";
            // 
            // DtpFromDate
            // 
            this.DtpFromDate.AllowTabKeyOnEnter = false;
            this.DtpFromDate.Checked = false;
            this.DtpFromDate.CustomFormat = "dd/MM/yyyy";
            this.DtpFromDate.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DtpFromDate.ForeColor = System.Drawing.Color.Black;
            this.DtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpFromDate.Location = new System.Drawing.Point(81, 5);
            this.DtpFromDate.Name = "DtpFromDate";
            this.DtpFromDate.ShowCheckBox = true;
            this.DtpFromDate.Size = new System.Drawing.Size(139, 22);
            this.DtpFromDate.TabIndex = 175;
            this.DtpFromDate.ToolTips = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label3.Location = new System.Drawing.Point(4, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 14);
            this.label3.TabIndex = 170;
            this.label3.Text = "From Date";
            this.label3.ToolTips = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.label5.Location = new System.Drawing.Point(226, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 14);
            this.label5.TabIndex = 171;
            this.label5.Text = "To";
            this.label5.ToolTips = "";
            // 
            // pivotGrid
            // 
            this.pivotGrid.Appearance.ColumnHeaderArea.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.pivotGrid.Appearance.ColumnHeaderArea.Options.UseFont = true;
            this.pivotGrid.Appearance.DataHeaderArea.Font = new System.Drawing.Font("Verdana", 9F);
            this.pivotGrid.Appearance.DataHeaderArea.Options.UseFont = true;
            this.pivotGrid.Appearance.FieldHeader.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.pivotGrid.Appearance.FieldHeader.Options.UseFont = true;
            this.pivotGrid.Appearance.FieldValue.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.pivotGrid.Appearance.FieldValue.Options.UseFont = true;
            this.pivotGrid.Appearance.FieldValueGrandTotal.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGrid.Appearance.FieldValueGrandTotal.Options.UseFont = true;
            this.pivotGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pivotGrid.Fields.AddRange(new DevExpress.XtraPivotGrid.PivotGridField[] {
            this.pivotGridField2,
            this.pivotGridField3,
            this.pivotGridField4,
            this.pivotGridField5,
            this.pivotGridField6,
            this.pivotGridField8,
            this.pivotGridField7,
            this.Carat,
            this.sale});
            this.pivotGrid.Location = new System.Drawing.Point(0, 80);
            this.pivotGrid.Name = "pivotGrid";
            this.pivotGrid.OptionsBehavior.BestFitMode = DevExpress.XtraPivotGrid.PivotGridBestFitMode.FieldHeader;
            this.pivotGrid.OptionsData.DataProcessingEngine = DevExpress.XtraPivotGrid.PivotDataProcessingEngine.Optimized;
            this.pivotGrid.OptionsView.ShowColumnTotals = false;
            this.pivotGrid.OptionsView.ShowRowTotals = false;
            this.pivotGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repTxtRate});
            this.pivotGrid.Size = new System.Drawing.Size(795, 400);
            this.pivotGrid.TabIndex = 0;
            this.pivotGrid.CustomCellValue += new System.EventHandler<DevExpress.XtraPivotGrid.PivotCellValueEventArgs>(this.pivotGrid_CustomCellValue);
            this.pivotGrid.EditValueChanged += new DevExpress.XtraPivotGrid.EditValueChangedEventHandler(this.pivotGrid_EditValueChanged);
            this.pivotGrid.Click += new System.EventHandler(this.pivotGrid_Click);
            // 
            // pivotGridField2
            // 
            this.pivotGridField2.Appearance.Cell.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.pivotGridField2.Appearance.Cell.Options.UseFont = true;
            this.pivotGridField2.Appearance.Cell.Options.UseTextOptions = true;
            this.pivotGridField2.Appearance.Cell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField2.Appearance.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField2.Appearance.Header.Options.UseFont = true;
            this.pivotGridField2.Appearance.Header.Options.UseTextOptions = true;
            this.pivotGridField2.Appearance.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField2.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.pivotGridField2.AreaIndex = 1;
            this.pivotGridField2.Caption = "SHAPE_ID";
            dataSourceColumnBinding1.ColumnName = "SHAPE_ID";
            this.pivotGridField2.DataBinding = dataSourceColumnBinding1;
            this.pivotGridField2.Name = "pivotGridField2";
            this.pivotGridField2.Visible = false;
            // 
            // pivotGridField3
            // 
            this.pivotGridField3.Appearance.Cell.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.pivotGridField3.Appearance.Cell.Options.UseFont = true;
            this.pivotGridField3.Appearance.Cell.Options.UseTextOptions = true;
            this.pivotGridField3.Appearance.Cell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField3.Appearance.CellGrandTotal.Font = new System.Drawing.Font("Verdana", 9F);
            this.pivotGridField3.Appearance.CellGrandTotal.Options.UseFont = true;
            this.pivotGridField3.Appearance.CellTotal.Font = new System.Drawing.Font("Verdana", 9F);
            this.pivotGridField3.Appearance.CellTotal.Options.UseFont = true;
            this.pivotGridField3.Appearance.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField3.Appearance.Header.Options.UseFont = true;
            this.pivotGridField3.Appearance.Header.Options.UseTextOptions = true;
            this.pivotGridField3.Appearance.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField3.Appearance.Value.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pivotGridField3.Appearance.Value.Options.UseFont = true;
            this.pivotGridField3.Appearance.ValueGrandTotal.Font = new System.Drawing.Font("Verdana", 9F);
            this.pivotGridField3.Appearance.ValueGrandTotal.Options.UseFont = true;
            this.pivotGridField3.Appearance.ValueTotal.Font = new System.Drawing.Font("Verdana", 9F);
            this.pivotGridField3.Appearance.ValueTotal.Options.UseFont = true;
            this.pivotGridField3.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.pivotGridField3.AreaIndex = 0;
            this.pivotGridField3.Caption = "Shape";
            dataSourceColumnBinding2.ColumnName = "SHAPENAME";
            this.pivotGridField3.DataBinding = dataSourceColumnBinding2;
            this.pivotGridField3.Name = "pivotGridField3";
            // 
            // pivotGridField4
            // 
            this.pivotGridField4.Appearance.Cell.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.pivotGridField4.Appearance.Cell.Options.UseFont = true;
            this.pivotGridField4.Appearance.Cell.Options.UseTextOptions = true;
            this.pivotGridField4.Appearance.Cell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField4.Appearance.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField4.Appearance.Header.Options.UseFont = true;
            this.pivotGridField4.Appearance.Header.Options.UseTextOptions = true;
            this.pivotGridField4.Appearance.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField4.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.pivotGridField4.AreaIndex = 1;
            this.pivotGridField4.Caption = "COLOR_ID";
            dataSourceColumnBinding3.ColumnName = "MIXCLARITY_ID";
            this.pivotGridField4.DataBinding = dataSourceColumnBinding3;
            this.pivotGridField4.Name = "pivotGridField4";
            this.pivotGridField4.Visible = false;
            // 
            // pivotGridField5
            // 
            this.pivotGridField5.Appearance.Cell.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.pivotGridField5.Appearance.Cell.Options.UseFont = true;
            this.pivotGridField5.Appearance.Cell.Options.UseTextOptions = true;
            this.pivotGridField5.Appearance.Cell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField5.Appearance.CellGrandTotal.Font = new System.Drawing.Font("Verdana", 9F);
            this.pivotGridField5.Appearance.CellGrandTotal.Options.UseFont = true;
            this.pivotGridField5.Appearance.CellTotal.Font = new System.Drawing.Font("Verdana", 9F);
            this.pivotGridField5.Appearance.CellTotal.Options.UseFont = true;
            this.pivotGridField5.Appearance.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField5.Appearance.Header.Options.UseFont = true;
            this.pivotGridField5.Appearance.Header.Options.UseTextOptions = true;
            this.pivotGridField5.Appearance.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField5.Appearance.Value.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pivotGridField5.Appearance.Value.Options.UseFont = true;
            this.pivotGridField5.Appearance.Value.Options.UseTextOptions = true;
            this.pivotGridField5.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField5.Appearance.ValueGrandTotal.Font = new System.Drawing.Font("Verdana", 9F);
            this.pivotGridField5.Appearance.ValueGrandTotal.Options.UseFont = true;
            this.pivotGridField5.Appearance.ValueTotal.Font = new System.Drawing.Font("Verdana", 9F);
            this.pivotGridField5.Appearance.ValueTotal.Options.UseFont = true;
            this.pivotGridField5.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.pivotGridField5.AreaIndex = 1;
            this.pivotGridField5.Caption = "Color";
            dataSourceColumnBinding4.ColumnName = "COLORNAME";
            this.pivotGridField5.DataBinding = dataSourceColumnBinding4;
            this.pivotGridField5.Name = "pivotGridField5";
            this.pivotGridField5.Width = 80;
            // 
            // pivotGridField6
            // 
            this.pivotGridField6.Appearance.Cell.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.pivotGridField6.Appearance.Cell.Options.UseFont = true;
            this.pivotGridField6.Appearance.Cell.Options.UseTextOptions = true;
            this.pivotGridField6.Appearance.Cell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField6.Appearance.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField6.Appearance.Header.Options.UseFont = true;
            this.pivotGridField6.Appearance.Header.Options.UseTextOptions = true;
            this.pivotGridField6.Appearance.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField6.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.pivotGridField6.AreaIndex = 1;
            this.pivotGridField6.Caption = "CLARITY_ID";
            dataSourceColumnBinding5.ColumnName = "MIXSIZE_ID";
            this.pivotGridField6.DataBinding = dataSourceColumnBinding5;
            this.pivotGridField6.Name = "pivotGridField6";
            this.pivotGridField6.Visible = false;
            // 
            // pivotGridField8
            // 
            this.pivotGridField8.Appearance.Cell.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.pivotGridField8.Appearance.Cell.Options.UseFont = true;
            this.pivotGridField8.Appearance.Cell.Options.UseTextOptions = true;
            this.pivotGridField8.Appearance.Cell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField8.Appearance.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField8.Appearance.Header.Options.UseFont = true;
            this.pivotGridField8.Appearance.Header.Options.UseTextOptions = true;
            this.pivotGridField8.Appearance.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField8.Appearance.Value.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pivotGridField8.Appearance.Value.Options.UseFont = true;
            this.pivotGridField8.Appearance.Value.Options.UseTextOptions = true;
            this.pivotGridField8.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField8.Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            this.pivotGridField8.AreaIndex = 1;
            this.pivotGridField8.Caption = "Clarity";
            dataSourceColumnBinding6.ColumnName = "CLARITYNAME";
            this.pivotGridField8.DataBinding = dataSourceColumnBinding6;
            this.pivotGridField8.Name = "pivotGridField8";
            // 
            // pivotGridField7
            // 
            this.pivotGridField7.Appearance.Cell.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.pivotGridField7.Appearance.Cell.Options.UseFont = true;
            this.pivotGridField7.Appearance.Cell.Options.UseTextOptions = true;
            this.pivotGridField7.Appearance.Cell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField7.Appearance.CellGrandTotal.Font = new System.Drawing.Font("Verdana", 9F);
            this.pivotGridField7.Appearance.CellGrandTotal.Options.UseFont = true;
            this.pivotGridField7.Appearance.CellTotal.Font = new System.Drawing.Font("Verdana", 9F);
            this.pivotGridField7.Appearance.CellTotal.Options.UseFont = true;
            this.pivotGridField7.Appearance.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.pivotGridField7.Appearance.Header.Options.UseFont = true;
            this.pivotGridField7.Appearance.Header.Options.UseTextOptions = true;
            this.pivotGridField7.Appearance.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField7.Appearance.Value.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pivotGridField7.Appearance.Value.Options.UseFont = true;
            this.pivotGridField7.Appearance.Value.Options.UseTextOptions = true;
            this.pivotGridField7.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.pivotGridField7.Appearance.ValueGrandTotal.Font = new System.Drawing.Font("Verdana", 9F);
            this.pivotGridField7.Appearance.ValueGrandTotal.Options.UseFont = true;
            this.pivotGridField7.Appearance.ValueTotal.Font = new System.Drawing.Font("Verdana", 9F);
            this.pivotGridField7.Appearance.ValueTotal.Options.UseFont = true;
            this.pivotGridField7.Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            this.pivotGridField7.AreaIndex = 0;
            this.pivotGridField7.Caption = "Size";
            dataSourceColumnBinding7.ColumnName = "SIZENAME";
            this.pivotGridField7.DataBinding = dataSourceColumnBinding7;
            this.pivotGridField7.Name = "pivotGridField7";
            // 
            // Carat
            // 
            this.Carat.Appearance.Cell.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Carat.Appearance.Cell.Options.UseFont = true;
            this.Carat.Appearance.Cell.Options.UseTextOptions = true;
            this.Carat.Appearance.Cell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Carat.Appearance.CellGrandTotal.Font = new System.Drawing.Font("Verdana", 9F);
            this.Carat.Appearance.CellGrandTotal.Options.UseFont = true;
            this.Carat.Appearance.CellTotal.Font = new System.Drawing.Font("Verdana", 9F);
            this.Carat.Appearance.CellTotal.Options.UseFont = true;
            this.Carat.Appearance.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.Carat.Appearance.Header.Options.UseFont = true;
            this.Carat.Appearance.Header.Options.UseTextOptions = true;
            this.Carat.Appearance.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Carat.Appearance.Value.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Carat.Appearance.Value.Options.UseFont = true;
            this.Carat.Appearance.Value.Options.UseTextOptions = true;
            this.Carat.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.Carat.Appearance.ValueGrandTotal.Font = new System.Drawing.Font("Verdana", 9F);
            this.Carat.Appearance.ValueGrandTotal.Options.UseFont = true;
            this.Carat.Appearance.ValueTotal.Font = new System.Drawing.Font("Verdana", 9F);
            this.Carat.Appearance.ValueTotal.Options.UseFont = true;
            this.Carat.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.Carat.AreaIndex = 0;
            this.Carat.Caption = "Stock";
            this.Carat.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dataSourceColumnBinding8.ColumnName = "STOCKCARAT";
            this.Carat.DataBinding = dataSourceColumnBinding8;
            this.Carat.Name = "Carat";
            this.Carat.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.Carat.Width = 44;
            // 
            // sale
            // 
            this.sale.Appearance.Cell.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.sale.Appearance.Cell.Options.UseFont = true;
            this.sale.Appearance.Cell.Options.UseTextOptions = true;
            this.sale.Appearance.Cell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.sale.Appearance.Header.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold);
            this.sale.Appearance.Header.Options.UseFont = true;
            this.sale.Appearance.Header.Options.UseTextOptions = true;
            this.sale.Appearance.Header.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.sale.Appearance.Value.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sale.Appearance.Value.Options.UseFont = true;
            this.sale.Appearance.Value.Options.UseTextOptions = true;
            this.sale.Appearance.Value.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.sale.Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            this.sale.AreaIndex = 1;
            this.sale.Caption = "Sale";
            this.sale.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            dataSourceColumnBinding9.ColumnName = "SALECARAT";
            this.sale.DataBinding = dataSourceColumnBinding9;
            this.sale.Name = "sale";
            this.sale.TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.None;
            this.sale.Width = 51;
            // 
            // repTxtRate
            // 
            this.repTxtRate.AutoHeight = false;
            this.repTxtRate.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.repTxtRate.MaskSettings.Set("MaskManagerType", typeof(DevExpress.Data.Mask.NumericMaskManager));
            this.repTxtRate.MaskSettings.Set("MaskManagerSignature", "allowNull=False");
            this.repTxtRate.MaskSettings.Set("mask", "n2");
            this.repTxtRate.Name = "repTxtRate";
            this.repTxtRate.UseMaskAsDisplayFormat = true;
            // 
            // FrmSaleAnalysisReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(795, 480);
            this.Controls.Add(this.pivotGrid);
            this.Controls.Add(this.panel2);
            this.LookAndFeel.SkinName = "London Liquid Sky";
            this.LookAndFeel.UseDefaultLookAndFeel =true;
            this.Name = "FrmSaleAnalysisReport";
            this.Text = "CLARITY ASSORTMENT VIEW";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pivotGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtRate)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevControlLib.cDevSimpleButton BtnExport;
        private DevControlLib.cDevSimpleButton BtnBack;
        private DevControlLib.cDevSimpleButton BtnSearch;
        private AxonContLib.cPanel panel2;
        private AxonContLib.cLabel label3;
        private AxonContLib.cLabel label5;
        private DevExpress.XtraPivotGrid.PivotGridControl pivotGrid;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField2;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField3;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField4;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField5;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField6;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField7;
        private DevExpress.XtraPivotGrid.PivotGridField Carat;
        private AxonContLib.cDateTimePicker DtpToDate;
        private AxonContLib.cDateTimePicker DtpFromDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repTxtRate;
        private DevControlLib.cDevSimpleButton BtnClear;
        private DevExpress.XtraPivotGrid.PivotGridField sale;
        private DevExpress.XtraPivotGrid.PivotGridField pivotGridField8;
    }
}