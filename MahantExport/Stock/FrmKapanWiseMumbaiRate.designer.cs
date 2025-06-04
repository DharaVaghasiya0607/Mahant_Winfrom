namespace MahantExport.Stock
{
    partial class FrmKapanWiseMumbaiRate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmKapanWiseMumbaiRate));
            this.BtnExport = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnSave = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnAdd = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnExit = new DevControlLib.cDevSimpleButton(this.components);
            this.MainGrid = new DevExpress.XtraGrid.GridControl();
            this.GrdDet = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridColumn3 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.gridColumn2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand3 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.bandedGridColumn1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn3 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.PanelTop = new AxonContLib.cPanel(this.components);
            this.cLabel2 = new AxonContLib.cLabel(this.components);
            this.DtpKapanDate = new AxonContLib.cDateTimePicker(this.components);
            this.BtnDelete = new DevControlLib.cDevSimpleButton(this.components);
            this.cLabel7 = new AxonContLib.cLabel(this.components);
            this.txtKapanRate_ID = new AxonContLib.cTextBox(this.components);
            this.txtMixKapanAmount = new AxonContLib.cTextBox(this.components);
            this.txtMixKapanRate = new AxonContLib.cTextBox(this.components);
            this.txtMixKapanCarat = new AxonContLib.cTextBox(this.components);
            this.cLabel5 = new AxonContLib.cLabel(this.components);
            this.txtGIAKapanAmount = new AxonContLib.cTextBox(this.components);
            this.cLabel4 = new AxonContLib.cLabel(this.components);
            this.txtGIAKapanRate = new AxonContLib.cTextBox(this.components);
            this.cLabel3 = new AxonContLib.cLabel(this.components);
            this.txtGIAKapanCarat = new AxonContLib.cTextBox(this.components);
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            this.txtKapanName = new AxonContLib.cTextBox(this.components);
            this.cLabel6 = new AxonContLib.cLabel(this.components);
            this.panel2 = new AxonContLib.cPanel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).BeginInit();
            this.PanelTop.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnExport
            // 
            this.BtnExport.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnExport.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnExport.Appearance.Options.UseFont = true;
            this.BtnExport.Appearance.Options.UseForeColor = true;
            this.BtnExport.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnExport.ImageOptions.SvgImage")));
            this.BtnExport.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnExport.Location = new System.Drawing.Point(715, 10);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(91, 40);
            this.BtnExport.TabIndex = 15;
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
            this.BtnSave.Location = new System.Drawing.Point(436, 10);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(91, 40);
            this.BtnSave.TabIndex = 12;
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
            this.BtnAdd.Location = new System.Drawing.Point(622, 10);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(91, 40);
            this.BtnAdd.TabIndex = 14;
            this.BtnAdd.TabStop = false;
            this.BtnAdd.Text = "&Clear";
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnExit.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnExit.Appearance.Options.UseFont = true;
            this.BtnExit.Appearance.Options.UseForeColor = true;
            this.BtnExit.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnExit.ImageOptions.SvgImage")));
            this.BtnExit.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnExit.Location = new System.Drawing.Point(808, 10);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(91, 40);
            this.BtnExit.TabIndex = 16;
            this.BtnExit.TabStop = false;
            this.BtnExit.Text = "E&xit";
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // MainGrid
            // 
            this.MainGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGrid.Location = new System.Drawing.Point(0, 0);
            this.MainGrid.MainView = this.GrdDet;
            this.MainGrid.Name = "MainGrid";
            this.MainGrid.Size = new System.Drawing.Size(1077, 372);
            this.MainGrid.TabIndex = 0;
            this.MainGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GrdDet});
            // 
            // GrdDet
            // 
            this.GrdDet.Appearance.BandPanel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.BandPanel.Options.UseFont = true;
            this.GrdDet.Appearance.BandPanel.Options.UseTextOptions = true;
            this.GrdDet.Appearance.BandPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
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
            this.GrdDet.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1,
            this.gridBand2,
            this.gridBand3});
            this.GrdDet.ColumnPanelRowHeight = 25;
            this.GrdDet.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.gridColumn3,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn4,
            this.gridColumn5,
            this.bandedGridColumn1,
            this.bandedGridColumn2,
            this.bandedGridColumn3});
            this.GrdDet.GridControl = this.MainGrid;
            this.GrdDet.Name = "GrdDet";
            this.GrdDet.OptionsBehavior.Editable = false;
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
            // gridBand1
            // 
            this.gridBand1.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gridBand1.AppearanceHeader.Options.UseFont = true;
            this.gridBand1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand1.Caption = "..";
            this.gridBand1.Columns.Add(this.gridColumn3);
            this.gridBand1.Columns.Add(this.gridColumn1);
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.VisibleIndex = 0;
            this.gridBand1.Width = 264;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "Date";
            this.gridColumn3.FieldName = "KAPANDATE";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn3.Visible = true;
            this.gridColumn3.Width = 128;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "Kapan";
            this.gridColumn1.FieldName = "KAPANNAME";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn1.Visible = true;
            this.gridColumn1.Width = 136;
            // 
            // gridBand2
            // 
            this.gridBand2.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gridBand2.AppearanceHeader.Options.UseFont = true;
            this.gridBand2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand2.Caption = "GIA";
            this.gridBand2.Columns.Add(this.gridColumn2);
            this.gridBand2.Columns.Add(this.gridColumn4);
            this.gridBand2.Columns.Add(this.gridColumn5);
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.VisibleIndex = 1;
            this.gridBand2.Width = 320;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "Carat";
            this.gridColumn2.FieldName = "GIAKAPANCARAT";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn2.Visible = true;
            this.gridColumn2.Width = 100;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "Rate";
            this.gridColumn4.FieldName = "GIAKAPANRATE";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn4.Visible = true;
            this.gridColumn4.Width = 100;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "Amt";
            this.gridColumn5.FieldName = "GIAKAPANAMOUNT";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn5.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn5.Visible = true;
            this.gridColumn5.Width = 120;
            // 
            // gridBand3
            // 
            this.gridBand3.AppearanceHeader.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold);
            this.gridBand3.AppearanceHeader.Options.UseFont = true;
            this.gridBand3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridBand3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridBand3.Caption = "Mix";
            this.gridBand3.Columns.Add(this.bandedGridColumn1);
            this.gridBand3.Columns.Add(this.bandedGridColumn2);
            this.gridBand3.Columns.Add(this.bandedGridColumn3);
            this.gridBand3.Name = "gridBand3";
            this.gridBand3.VisibleIndex = 2;
            this.gridBand3.Width = 320;
            // 
            // bandedGridColumn1
            // 
            this.bandedGridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn1.Caption = "Carat";
            this.bandedGridColumn1.FieldName = "MIXKAPANCARAT";
            this.bandedGridColumn1.Name = "bandedGridColumn1";
            this.bandedGridColumn1.OptionsColumn.AllowEdit = false;
            this.bandedGridColumn1.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.bandedGridColumn1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.bandedGridColumn1.Visible = true;
            this.bandedGridColumn1.Width = 100;
            // 
            // bandedGridColumn2
            // 
            this.bandedGridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn2.Caption = "Rate";
            this.bandedGridColumn2.FieldName = "MIXKAPANRATE";
            this.bandedGridColumn2.Name = "bandedGridColumn2";
            this.bandedGridColumn2.OptionsColumn.AllowEdit = false;
            this.bandedGridColumn2.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.bandedGridColumn2.Visible = true;
            this.bandedGridColumn2.Width = 100;
            // 
            // bandedGridColumn3
            // 
            this.bandedGridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn3.Caption = "Amt";
            this.bandedGridColumn3.FieldName = "MIXKAPANAMOUNT";
            this.bandedGridColumn3.Name = "bandedGridColumn3";
            this.bandedGridColumn3.OptionsColumn.AllowEdit = false;
            this.bandedGridColumn3.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.bandedGridColumn3.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.bandedGridColumn3.Visible = true;
            this.bandedGridColumn3.Width = 120;
            // 
            // PanelTop
            // 
            this.PanelTop.BackColor = System.Drawing.Color.Gainsboro;
            this.PanelTop.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PanelTop.Controls.Add(this.cLabel2);
            this.PanelTop.Controls.Add(this.DtpKapanDate);
            this.PanelTop.Controls.Add(this.BtnDelete);
            this.PanelTop.Controls.Add(this.cLabel7);
            this.PanelTop.Controls.Add(this.BtnExit);
            this.PanelTop.Controls.Add(this.txtKapanRate_ID);
            this.PanelTop.Controls.Add(this.BtnSave);
            this.PanelTop.Controls.Add(this.BtnAdd);
            this.PanelTop.Controls.Add(this.txtMixKapanAmount);
            this.PanelTop.Controls.Add(this.BtnExport);
            this.PanelTop.Controls.Add(this.txtMixKapanRate);
            this.PanelTop.Controls.Add(this.txtMixKapanCarat);
            this.PanelTop.Controls.Add(this.cLabel5);
            this.PanelTop.Controls.Add(this.txtGIAKapanAmount);
            this.PanelTop.Controls.Add(this.cLabel4);
            this.PanelTop.Controls.Add(this.txtGIAKapanRate);
            this.PanelTop.Controls.Add(this.cLabel3);
            this.PanelTop.Controls.Add(this.txtGIAKapanCarat);
            this.PanelTop.Controls.Add(this.cLabel1);
            this.PanelTop.Controls.Add(this.txtKapanName);
            this.PanelTop.Controls.Add(this.cLabel6);
            this.PanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelTop.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.PanelTop.Location = new System.Drawing.Point(0, 0);
            this.PanelTop.Name = "PanelTop";
            this.PanelTop.Size = new System.Drawing.Size(1077, 102);
            this.PanelTop.TabIndex = 189;
            this.PanelTop.TabStop = true;
            // 
            // cLabel2
            // 
            this.cLabel2.AutoSize = true;
            this.cLabel2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel2.ForeColor = System.Drawing.Color.Black;
            this.cLabel2.Location = new System.Drawing.Point(30, 10);
            this.cLabel2.Name = "cLabel2";
            this.cLabel2.Size = new System.Drawing.Size(47, 14);
            this.cLabel2.TabIndex = 224;
            this.cLabel2.Text = "Date :";
            this.cLabel2.ToolTips = "";
            // 
            // DtpKapanDate
            // 
            this.DtpKapanDate.AllowTabKeyOnEnter = false;
            this.DtpKapanDate.Font = new System.Drawing.Font("Verdana", 9F);
            this.DtpKapanDate.ForeColor = System.Drawing.Color.Black;
            this.DtpKapanDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.DtpKapanDate.Location = new System.Drawing.Point(81, 6);
            this.DtpKapanDate.Name = "DtpKapanDate";
            this.DtpKapanDate.Size = new System.Drawing.Size(125, 22);
            this.DtpKapanDate.TabIndex = 1;
            this.DtpKapanDate.ToolTips = "";
            // 
            // BtnDelete
            // 
            this.BtnDelete.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnDelete.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnDelete.Appearance.Options.UseFont = true;
            this.BtnDelete.Appearance.Options.UseForeColor = true;
            this.BtnDelete.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnDelete.ImageOptions.SvgImage")));
            this.BtnDelete.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnDelete.Location = new System.Drawing.Point(529, 10);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(91, 40);
            this.BtnDelete.TabIndex = 13;
            this.BtnDelete.Text = "&Delete";
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // cLabel7
            // 
            this.cLabel7.AutoSize = true;
            this.cLabel7.Font = new System.Drawing.Font("Verdana", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.cLabel7.ForeColor = System.Drawing.Color.Black;
            this.cLabel7.Location = new System.Drawing.Point(10, 74);
            this.cLabel7.Name = "cLabel7";
            this.cLabel7.Size = new System.Drawing.Size(71, 13);
            this.cLabel7.TabIndex = 203;
            this.cLabel7.Text = "--: MIX :--";
            this.cLabel7.ToolTips = "";
            // 
            // txtKapanRate_ID
            // 
            this.txtKapanRate_ID.ActivationColor = true;
            this.txtKapanRate_ID.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtKapanRate_ID.AllowTabKeyOnEnter = false;
            this.txtKapanRate_ID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKapanRate_ID.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKapanRate_ID.Format = "";
            this.txtKapanRate_ID.IsComplusory = false;
            this.txtKapanRate_ID.Location = new System.Drawing.Point(436, 52);
            this.txtKapanRate_ID.Name = "txtKapanRate_ID";
            this.txtKapanRate_ID.ReadOnly = true;
            this.txtKapanRate_ID.SelectAllTextOnFocus = true;
            this.txtKapanRate_ID.Size = new System.Drawing.Size(55, 22);
            this.txtKapanRate_ID.TabIndex = 198;
            this.txtKapanRate_ID.TabStop = false;
            this.txtKapanRate_ID.ToolTips = "";
            this.txtKapanRate_ID.Visible = false;
            this.txtKapanRate_ID.WaterMarkText = null;
            // 
            // txtMixKapanAmount
            // 
            this.txtMixKapanAmount.ActivationColor = true;
            this.txtMixKapanAmount.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtMixKapanAmount.AllowTabKeyOnEnter = false;
            this.txtMixKapanAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMixKapanAmount.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMixKapanAmount.Format = "";
            this.txtMixKapanAmount.IsComplusory = false;
            this.txtMixKapanAmount.Location = new System.Drawing.Point(289, 69);
            this.txtMixKapanAmount.Name = "txtMixKapanAmount";
            this.txtMixKapanAmount.ReadOnly = true;
            this.txtMixKapanAmount.SelectAllTextOnFocus = true;
            this.txtMixKapanAmount.Size = new System.Drawing.Size(125, 22);
            this.txtMixKapanAmount.TabIndex = 10;
            this.txtMixKapanAmount.TabStop = false;
            this.txtMixKapanAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMixKapanAmount.ToolTips = "";
            this.txtMixKapanAmount.WaterMarkText = null;
            // 
            // txtMixKapanRate
            // 
            this.txtMixKapanRate.ActivationColor = true;
            this.txtMixKapanRate.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtMixKapanRate.AllowTabKeyOnEnter = false;
            this.txtMixKapanRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMixKapanRate.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMixKapanRate.Format = "";
            this.txtMixKapanRate.IsComplusory = false;
            this.txtMixKapanRate.Location = new System.Drawing.Point(161, 69);
            this.txtMixKapanRate.Name = "txtMixKapanRate";
            this.txtMixKapanRate.SelectAllTextOnFocus = true;
            this.txtMixKapanRate.Size = new System.Drawing.Size(125, 22);
            this.txtMixKapanRate.TabIndex = 9;
            this.txtMixKapanRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMixKapanRate.ToolTips = "";
            this.txtMixKapanRate.WaterMarkText = null;
            this.txtMixKapanRate.Validated += new System.EventHandler(this.txtMixKapanCarat_Validated);
            // 
            // txtMixKapanCarat
            // 
            this.txtMixKapanCarat.ActivationColor = true;
            this.txtMixKapanCarat.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtMixKapanCarat.AllowTabKeyOnEnter = false;
            this.txtMixKapanCarat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMixKapanCarat.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMixKapanCarat.Format = "";
            this.txtMixKapanCarat.IsComplusory = false;
            this.txtMixKapanCarat.Location = new System.Drawing.Point(81, 69);
            this.txtMixKapanCarat.Name = "txtMixKapanCarat";
            this.txtMixKapanCarat.SelectAllTextOnFocus = true;
            this.txtMixKapanCarat.Size = new System.Drawing.Size(77, 22);
            this.txtMixKapanCarat.TabIndex = 8;
            this.txtMixKapanCarat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMixKapanCarat.ToolTips = "";
            this.txtMixKapanCarat.WaterMarkText = null;
            this.txtMixKapanCarat.Validated += new System.EventHandler(this.txtMixKapanCarat_Validated);
            // 
            // cLabel5
            // 
            this.cLabel5.AutoSize = true;
            this.cLabel5.Font = new System.Drawing.Font("Verdana", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.cLabel5.ForeColor = System.Drawing.Color.Black;
            this.cLabel5.Location = new System.Drawing.Point(10, 49);
            this.cLabel5.Name = "cLabel5";
            this.cLabel5.Size = new System.Drawing.Size(70, 13);
            this.cLabel5.TabIndex = 199;
            this.cLabel5.Text = "--: GIA :--";
            this.cLabel5.ToolTips = "";
            // 
            // txtGIAKapanAmount
            // 
            this.txtGIAKapanAmount.ActivationColor = true;
            this.txtGIAKapanAmount.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtGIAKapanAmount.AllowTabKeyOnEnter = false;
            this.txtGIAKapanAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGIAKapanAmount.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGIAKapanAmount.Format = "";
            this.txtGIAKapanAmount.IsComplusory = false;
            this.txtGIAKapanAmount.Location = new System.Drawing.Point(289, 44);
            this.txtGIAKapanAmount.Name = "txtGIAKapanAmount";
            this.txtGIAKapanAmount.ReadOnly = true;
            this.txtGIAKapanAmount.SelectAllTextOnFocus = true;
            this.txtGIAKapanAmount.Size = new System.Drawing.Size(125, 22);
            this.txtGIAKapanAmount.TabIndex = 7;
            this.txtGIAKapanAmount.TabStop = false;
            this.txtGIAKapanAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtGIAKapanAmount.ToolTips = "";
            this.txtGIAKapanAmount.WaterMarkText = null;
            // 
            // cLabel4
            // 
            this.cLabel4.AutoSize = true;
            this.cLabel4.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel4.ForeColor = System.Drawing.Color.Black;
            this.cLabel4.Location = new System.Drawing.Point(327, 30);
            this.cLabel4.Name = "cLabel4";
            this.cLabel4.Size = new System.Drawing.Size(33, 14);
            this.cLabel4.TabIndex = 196;
            this.cLabel4.Text = "Amt";
            this.cLabel4.ToolTips = "";
            // 
            // txtGIAKapanRate
            // 
            this.txtGIAKapanRate.ActivationColor = true;
            this.txtGIAKapanRate.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtGIAKapanRate.AllowTabKeyOnEnter = false;
            this.txtGIAKapanRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGIAKapanRate.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGIAKapanRate.Format = "";
            this.txtGIAKapanRate.IsComplusory = false;
            this.txtGIAKapanRate.Location = new System.Drawing.Point(161, 44);
            this.txtGIAKapanRate.Name = "txtGIAKapanRate";
            this.txtGIAKapanRate.SelectAllTextOnFocus = true;
            this.txtGIAKapanRate.Size = new System.Drawing.Size(125, 22);
            this.txtGIAKapanRate.TabIndex = 6;
            this.txtGIAKapanRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtGIAKapanRate.ToolTips = "";
            this.txtGIAKapanRate.WaterMarkText = null;
            this.txtGIAKapanRate.Validated += new System.EventHandler(this.txtGIAKapanCarat_Validated);
            // 
            // cLabel3
            // 
            this.cLabel3.AutoSize = true;
            this.cLabel3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel3.ForeColor = System.Drawing.Color.Black;
            this.cLabel3.Location = new System.Drawing.Point(200, 30);
            this.cLabel3.Name = "cLabel3";
            this.cLabel3.Size = new System.Drawing.Size(37, 14);
            this.cLabel3.TabIndex = 194;
            this.cLabel3.Text = "Rate";
            this.cLabel3.ToolTips = "";
            // 
            // txtGIAKapanCarat
            // 
            this.txtGIAKapanCarat.ActivationColor = true;
            this.txtGIAKapanCarat.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtGIAKapanCarat.AllowTabKeyOnEnter = false;
            this.txtGIAKapanCarat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGIAKapanCarat.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGIAKapanCarat.Format = "";
            this.txtGIAKapanCarat.IsComplusory = false;
            this.txtGIAKapanCarat.Location = new System.Drawing.Point(81, 44);
            this.txtGIAKapanCarat.Name = "txtGIAKapanCarat";
            this.txtGIAKapanCarat.SelectAllTextOnFocus = true;
            this.txtGIAKapanCarat.Size = new System.Drawing.Size(77, 22);
            this.txtGIAKapanCarat.TabIndex = 5;
            this.txtGIAKapanCarat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtGIAKapanCarat.ToolTips = "";
            this.txtGIAKapanCarat.WaterMarkText = null;
            this.txtGIAKapanCarat.Validated += new System.EventHandler(this.txtGIAKapanCarat_Validated);
            // 
            // cLabel1
            // 
            this.cLabel1.AutoSize = true;
            this.cLabel1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel1.ForeColor = System.Drawing.Color.Black;
            this.cLabel1.Location = new System.Drawing.Point(98, 30);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(43, 14);
            this.cLabel1.TabIndex = 192;
            this.cLabel1.Text = "Carat";
            this.cLabel1.ToolTips = "";
            // 
            // txtKapanName
            // 
            this.txtKapanName.ActivationColor = true;
            this.txtKapanName.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtKapanName.AllowTabKeyOnEnter = false;
            this.txtKapanName.BackColor = System.Drawing.Color.Linen;
            this.txtKapanName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKapanName.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKapanName.Format = "";
            this.txtKapanName.IsComplusory = false;
            this.txtKapanName.Location = new System.Drawing.Point(286, 6);
            this.txtKapanName.Name = "txtKapanName";
            this.txtKapanName.SelectAllTextOnFocus = true;
            this.txtKapanName.Size = new System.Drawing.Size(128, 22);
            this.txtKapanName.TabIndex = 2;
            this.txtKapanName.ToolTips = "";
            this.txtKapanName.WaterMarkText = null;
            this.txtKapanName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtKapanName_KeyPress);
            // 
            // cLabel6
            // 
            this.cLabel6.AutoSize = true;
            this.cLabel6.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel6.ForeColor = System.Drawing.Color.Black;
            this.cLabel6.Location = new System.Drawing.Point(224, 10);
            this.cLabel6.Name = "cLabel6";
            this.cLabel6.Size = new System.Drawing.Size(57, 14);
            this.cLabel6.TabIndex = 10;
            this.cLabel6.Text = "Kapan :";
            this.cLabel6.ToolTips = "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.MainGrid);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel2.Location = new System.Drawing.Point(0, 102);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1077, 372);
            this.panel2.TabIndex = 190;
            // 
            // FrmKapanWiseMumbaiRate
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1077, 474);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.PanelTop);
            this.LookAndFeel.SkinName = "London Liquid Sky";
            this.LookAndFeel.UseDefaultLookAndFeel =true;
            this.Name = "FrmKapanWiseMumbaiRate";
            this.Tag = "KapanWiseMumbaiRate\r\n";
            this.Text = "KAPAN WISE MUMBAI RATE";
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).EndInit();
            this.PanelTop.ResumeLayout(false);
            this.PanelTop.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevControlLib.cDevSimpleButton BtnExport;
        private DevControlLib.cDevSimpleButton BtnSave;
        private DevControlLib.cDevSimpleButton BtnAdd;
        private DevControlLib.cDevSimpleButton BtnExit;
        private DevExpress.XtraGrid.GridControl MainGrid;
        private AxonContLib.cPanel PanelTop;
        private AxonContLib.cPanel panel2;
        private AxonContLib.cTextBox txtKapanName;
        private AxonContLib.cLabel cLabel6;
        private DevControlLib.cDevSimpleButton BtnDelete;
        private AxonContLib.cTextBox txtGIAKapanCarat;
        private AxonContLib.cLabel cLabel1;
        private AxonContLib.cTextBox txtGIAKapanRate;
        private AxonContLib.cLabel cLabel3;
        private AxonContLib.cTextBox txtGIAKapanAmount;
        private AxonContLib.cLabel cLabel4;
        private AxonContLib.cTextBox txtKapanRate_ID;
        private AxonContLib.cLabel cLabel5;
        private AxonContLib.cTextBox txtMixKapanAmount;
        private AxonContLib.cTextBox txtMixKapanRate;
        private AxonContLib.cTextBox txtMixKapanCarat;
        private AxonContLib.cLabel cLabel7;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView GrdDet;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn3;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn2;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn4;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn gridColumn5;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand3;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn2;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn3;
        private AxonContLib.cDateTimePicker DtpKapanDate;
        private AxonContLib.cLabel cLabel2;


    }
}