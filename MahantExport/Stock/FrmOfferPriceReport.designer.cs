namespace MahantExport.Report
{
    partial class FrmOfferPriceReport
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmOfferPriceReport));
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            this.MainGrid = new DevExpress.XtraGrid.GridControl();
            this.GrdDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.OFFERPRICE_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.STOCK_ID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RepBtnDelete = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.PanelTop = new AxonContLib.cPanel(this.components);
            this.btnExport = new DevControlLib.cDevSimpleButton(this.components);
            this.cLabel3 = new AxonContLib.cLabel(this.components);
            this.ChkCmbParty = new DevControlLib.cDevCheckedComboBoxEdit(this.components);
            this.DTPFromDate = new AxonContLib.cDateTimePicker(this.components);
            this.DTPToDate = new AxonContLib.cDateTimePicker(this.components);
            this.cLabel2 = new AxonContLib.cLabel(this.components);
            this.cLabel8 = new AxonContLib.cLabel(this.components);
            this.btnExit = new AxonContLib.cButton(this.components);
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            this.txtStockNo = new AxonContLib.cTextBox(this.components);
            this.btnSearch = new AxonContLib.cButton(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepBtnDelete)).BeginInit();
            this.PanelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChkCmbParty.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // MainGrid
            // 
            this.MainGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGrid.Location = new System.Drawing.Point(0, 67);
            this.MainGrid.MainView = this.GrdDetail;
            this.MainGrid.Name = "MainGrid";
            this.MainGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.RepBtnDelete});
            this.MainGrid.Size = new System.Drawing.Size(966, 397);
            this.MainGrid.TabIndex = 0;
            this.MainGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GrdDetail});
            // 
            // GrdDetail
            // 
            this.GrdDetail.Appearance.FocusedRow.Font = new System.Drawing.Font("Verdana", 9F);
            this.GrdDetail.Appearance.FocusedRow.Options.UseFont = true;
            this.GrdDetail.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.GrdDetail.Appearance.HeaderPanel.Options.UseFont = true;
            this.GrdDetail.Appearance.HorzLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDetail.Appearance.HorzLine.Options.UseBackColor = true;
            this.GrdDetail.Appearance.Row.Font = new System.Drawing.Font("Verdana", 9F);
            this.GrdDetail.Appearance.Row.Options.UseFont = true;
            this.GrdDetail.Appearance.VertLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDetail.Appearance.VertLine.Options.UseBackColor = true;
            this.GrdDetail.ColumnPanelRowHeight = 25;
            this.GrdDetail.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn9,
            this.OFFERPRICE_ID,
            this.STOCK_ID,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn13});
            this.GrdDetail.GridControl = this.MainGrid;
            this.GrdDetail.Name = "GrdDetail";
            this.GrdDetail.OptionsNavigation.EnterMoveNextColumn = true;
            this.GrdDetail.OptionsSelection.MultiSelect = true;
            this.GrdDetail.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.GrdDetail.OptionsView.AllowCellMerge = true;
            this.GrdDetail.OptionsView.ColumnAutoWidth = false;
            this.GrdDetail.OptionsView.ShowFooter = true;
            this.GrdDetail.OptionsView.ShowGroupPanel = false;
            this.GrdDetail.RowHeight = 25;
            this.GrdDetail.CustomSummaryCalculate += new DevExpress.Data.CustomSummaryEventHandler(this.GrdDetail_CustomSummaryCalculate);
            // 
            // gridColumn9
            // 
            this.gridColumn9.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn9.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn9.AppearanceCell.Options.UseFont = true;
            this.gridColumn9.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn9.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn9.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn9.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn9.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn9.AppearanceHeader.Options.UseFont = true;
            this.gridColumn9.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn9.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn9.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn9.Caption = "Cts";
            this.gridColumn9.FieldName = "CARAT";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowEdit = false;
            this.gridColumn9.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn9.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // OFFERPRICE_ID
            // 
            this.OFFERPRICE_ID.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.OFFERPRICE_ID.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.OFFERPRICE_ID.AppearanceCell.Options.UseFont = true;
            this.OFFERPRICE_ID.AppearanceCell.Options.UseForeColor = true;
            this.OFFERPRICE_ID.AppearanceCell.Options.UseTextOptions = true;
            this.OFFERPRICE_ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.OFFERPRICE_ID.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.OFFERPRICE_ID.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.OFFERPRICE_ID.AppearanceHeader.Options.UseFont = true;
            this.OFFERPRICE_ID.AppearanceHeader.Options.UseForeColor = true;
            this.OFFERPRICE_ID.AppearanceHeader.Options.UseTextOptions = true;
            this.OFFERPRICE_ID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.OFFERPRICE_ID.Caption = "OFFERPRICE_ID";
            this.OFFERPRICE_ID.FieldName = "OFFERPRICE_ID";
            this.OFFERPRICE_ID.Name = "OFFERPRICE_ID";
            this.OFFERPRICE_ID.OptionsColumn.AllowEdit = false;
            this.OFFERPRICE_ID.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.OFFERPRICE_ID.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // STOCK_ID
            // 
            this.STOCK_ID.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.STOCK_ID.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.STOCK_ID.AppearanceCell.Options.UseFont = true;
            this.STOCK_ID.AppearanceCell.Options.UseForeColor = true;
            this.STOCK_ID.AppearanceCell.Options.UseTextOptions = true;
            this.STOCK_ID.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.STOCK_ID.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.STOCK_ID.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.STOCK_ID.AppearanceHeader.Options.UseFont = true;
            this.STOCK_ID.AppearanceHeader.Options.UseForeColor = true;
            this.STOCK_ID.AppearanceHeader.Options.UseTextOptions = true;
            this.STOCK_ID.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.STOCK_ID.Caption = "STOCK_ID";
            this.STOCK_ID.FieldName = "STOCK_ID";
            this.STOCK_ID.Name = "STOCK_ID";
            this.STOCK_ID.OptionsColumn.AllowEdit = false;
            this.STOCK_ID.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.STOCK_ID.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn3.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn3.AppearanceCell.Options.UseFont = true;
            this.gridColumn3.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn3.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn3.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn3.AppearanceHeader.Options.UseFont = true;
            this.gridColumn3.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "Stock No";
            this.gridColumn3.FieldName = "STOCKNO";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
            this.gridColumn3.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn3.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "STOCKNO", "{0}")});
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 2;
            this.gridColumn3.Width = 99;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn4.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn4.AppearanceCell.Options.UseFont = true;
            this.gridColumn4.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn4.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn4.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn4.AppearanceHeader.Options.UseFont = true;
            this.gridColumn4.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "Rapaport";
            this.gridColumn4.FieldName = "OFFERRAPAPORT";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn4.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn4.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Custom)});
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 5;
            this.gridColumn4.Width = 109;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn5.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn5.AppearanceCell.Options.UseFont = true;
            this.gridColumn5.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn5.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn5.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn5.AppearanceHeader.Options.UseFont = true;
            this.gridColumn5.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "Dis";
            this.gridColumn5.FieldName = "OFFERDISCOUNT";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn5.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn5.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Custom)});
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 6;
            this.gridColumn5.Width = 107;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn6.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn6.AppearanceCell.Options.UseFont = true;
            this.gridColumn6.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn6.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn6.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn6.AppearanceHeader.Options.UseFont = true;
            this.gridColumn6.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "$/Cts";
            this.gridColumn6.FieldName = "OFFERPRICEPERCARAT";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn6.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn6.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Custom)});
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 7;
            this.gridColumn6.Width = 87;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn1.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn1.AppearanceCell.Options.UseFont = true;
            this.gridColumn1.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn1.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn1.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn1.AppearanceHeader.Options.UseFont = true;
            this.gridColumn1.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "Amount";
            this.gridColumn1.FieldName = "OFFERAMOUNT";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn1.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "OFFERAMOUNT", "SUM={0:0.##}")});
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 8;
            this.gridColumn1.Width = 99;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn2.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn2.AppearanceCell.Options.UseFont = true;
            this.gridColumn2.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
            this.gridColumn2.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn2.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn2.AppearanceHeader.Options.UseFont = true;
            this.gridColumn2.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "Remark";
            this.gridColumn2.FieldName = "OFFERCOMMENT";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn2.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 9;
            this.gridColumn2.Width = 178;
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn7.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn7.AppearanceCell.Options.UseFont = true;
            this.gridColumn7.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn7.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn7.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn7.AppearanceHeader.Options.UseFont = true;
            this.gridColumn7.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn7.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.Caption = "OFFERPARTY_ID";
            this.gridColumn7.FieldName = "OFFERPARTY_ID";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowEdit = false;
            this.gridColumn7.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn7.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // gridColumn8
            // 
            this.gridColumn8.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn8.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn8.AppearanceCell.Options.UseFont = true;
            this.gridColumn8.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn8.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn8.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn8.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn8.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn8.AppearanceHeader.Options.UseFont = true;
            this.gridColumn8.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn8.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn8.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn8.Caption = "Party";
            this.gridColumn8.FieldName = "PARTYNAME";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowEdit = false;
            this.gridColumn8.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn8.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 3;
            this.gridColumn8.Width = 224;
            // 
            // gridColumn10
            // 
            this.gridColumn10.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn10.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn10.AppearanceCell.Options.UseFont = true;
            this.gridColumn10.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn10.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn10.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn10.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn10.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn10.AppearanceHeader.Options.UseFont = true;
            this.gridColumn10.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn10.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn10.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn10.Caption = "Date";
            this.gridColumn10.FieldName = "ENTRYDATE";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsColumn.AllowEdit = false;
            this.gridColumn10.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn10.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 1;
            this.gridColumn10.Width = 104;
            // 
            // gridColumn11
            // 
            this.gridColumn11.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn11.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn11.AppearanceCell.Options.UseFont = true;
            this.gridColumn11.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn11.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn11.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn11.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn11.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn11.AppearanceHeader.Options.UseFont = true;
            this.gridColumn11.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn11.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn11.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn11.Caption = "OFFERBROKER_ID";
            this.gridColumn11.FieldName = "OFFERBROKER_ID";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsColumn.AllowEdit = false;
            this.gridColumn11.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn11.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // gridColumn12
            // 
            this.gridColumn12.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn12.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn12.AppearanceCell.Options.UseFont = true;
            this.gridColumn12.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn12.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn12.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn12.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn12.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn12.AppearanceHeader.Options.UseFont = true;
            this.gridColumn12.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn12.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn12.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn12.Caption = "Broker";
            this.gridColumn12.FieldName = "BROKERNAME";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.OptionsColumn.AllowEdit = false;
            this.gridColumn12.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn12.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 4;
            this.gridColumn12.Width = 224;
            // 
            // gridColumn13
            // 
            this.gridColumn13.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn13.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn13.AppearanceCell.Options.UseFont = true;
            this.gridColumn13.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn13.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn13.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn13.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn13.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn13.AppearanceHeader.Options.UseFont = true;
            this.gridColumn13.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn13.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn13.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn13.Caption = "Delete";
            this.gridColumn13.ColumnEdit = this.RepBtnDelete;
            this.gridColumn13.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
            this.gridColumn13.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 0;
            this.gridColumn13.Width = 40;
            // 
            // RepBtnDelete
            // 
            this.RepBtnDelete.AutoHeight = false;
            editorButtonImageOptions1.Image = ((System.Drawing.Image)(resources.GetObject("editorButtonImageOptions1.Image")));
            this.RepBtnDelete.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.RepBtnDelete.Name = "RepBtnDelete";
            this.RepBtnDelete.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            this.RepBtnDelete.Click += new System.EventHandler(this.RepBtnDelete_Click);
            // 
            // PanelTop
            // 
            this.PanelTop.BackColor = System.Drawing.Color.Gainsboro;
            this.PanelTop.Controls.Add(this.btnExport);
            this.PanelTop.Controls.Add(this.cLabel3);
            this.PanelTop.Controls.Add(this.ChkCmbParty);
            this.PanelTop.Controls.Add(this.DTPFromDate);
            this.PanelTop.Controls.Add(this.DTPToDate);
            this.PanelTop.Controls.Add(this.cLabel2);
            this.PanelTop.Controls.Add(this.cLabel8);
            this.PanelTop.Controls.Add(this.btnExit);
            this.PanelTop.Controls.Add(this.cLabel1);
            this.PanelTop.Controls.Add(this.txtStockNo);
            this.PanelTop.Controls.Add(this.btnSearch);
            this.PanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelTop.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.PanelTop.Location = new System.Drawing.Point(0, 0);
            this.PanelTop.Name = "PanelTop";
            this.PanelTop.Size = new System.Drawing.Size(966, 67);
            this.PanelTop.TabIndex = 0;
            this.PanelTop.TabStop = true;
            // 
            // btnExport
            // 
            this.btnExport.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.btnExport.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExport.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnExport.Appearance.Options.UseBackColor = true;
            this.btnExport.Appearance.Options.UseFont = true;
            this.btnExport.Appearance.Options.UseForeColor = true;
            this.btnExport.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnExport.ImageOptions.SvgImage")));
            this.btnExport.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.btnExport.Location = new System.Drawing.Point(841, 18);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(72, 31);
            this.btnExport.TabIndex = 205;
            this.btnExport.TabStop = false;
            this.btnExport.Text = "Export";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // cLabel3
            // 
            this.cLabel3.AutoSize = true;
            this.cLabel3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel3.ForeColor = System.Drawing.Color.Black;
            this.cLabel3.Location = new System.Drawing.Point(12, 42);
            this.cLabel3.Name = "cLabel3";
            this.cLabel3.Size = new System.Drawing.Size(42, 13);
            this.cLabel3.TabIndex = 43;
            this.cLabel3.Text = "Party";
            this.cLabel3.ToolTips = "";
            // 
            // ChkCmbParty
            // 
            this.ChkCmbParty.Location = new System.Drawing.Point(89, 38);
            this.ChkCmbParty.Name = "ChkCmbParty";
            this.ChkCmbParty.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ChkCmbParty.Size = new System.Drawing.Size(237, 20);
            this.ChkCmbParty.TabIndex = 42;
            // 
            // DTPFromDate
            // 
            this.DTPFromDate.AllowTabKeyOnEnter = true;
            this.DTPFromDate.CustomFormat = "dd/MM/yyyy";
            this.DTPFromDate.Font = new System.Drawing.Font("Verdana", 9F);
            this.DTPFromDate.ForeColor = System.Drawing.Color.Black;
            this.DTPFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTPFromDate.Location = new System.Drawing.Point(89, 9);
            this.DTPFromDate.Name = "DTPFromDate";
            this.DTPFromDate.Size = new System.Drawing.Size(102, 22);
            this.DTPFromDate.TabIndex = 38;
            this.DTPFromDate.ToolTips = "";
            // 
            // DTPToDate
            // 
            this.DTPToDate.AllowTabKeyOnEnter = true;
            this.DTPToDate.CustomFormat = "dd/MM/yyyy";
            this.DTPToDate.Font = new System.Drawing.Font("Verdana", 9F);
            this.DTPToDate.ForeColor = System.Drawing.Color.Black;
            this.DTPToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTPToDate.Location = new System.Drawing.Point(224, 9);
            this.DTPToDate.Name = "DTPToDate";
            this.DTPToDate.Size = new System.Drawing.Size(102, 22);
            this.DTPToDate.TabIndex = 39;
            this.DTPToDate.ToolTips = "";
            // 
            // cLabel2
            // 
            this.cLabel2.AutoSize = true;
            this.cLabel2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel2.ForeColor = System.Drawing.Color.Black;
            this.cLabel2.Location = new System.Drawing.Point(192, 14);
            this.cLabel2.Name = "cLabel2";
            this.cLabel2.Size = new System.Drawing.Size(23, 13);
            this.cLabel2.TabIndex = 41;
            this.cLabel2.Text = "To";
            this.cLabel2.ToolTips = "";
            // 
            // cLabel8
            // 
            this.cLabel8.AutoSize = true;
            this.cLabel8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel8.ForeColor = System.Drawing.Color.Black;
            this.cLabel8.Location = new System.Drawing.Point(12, 14);
            this.cLabel8.Name = "cLabel8";
            this.cLabel8.Size = new System.Drawing.Size(71, 13);
            this.cLabel8.TabIndex = 40;
            this.cLabel8.Text = "FromDate";
            this.cLabel8.ToolTips = "";
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(748, 18);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(90, 31);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.ToolTips = "";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // cLabel1
            // 
            this.cLabel1.AutoSize = true;
            this.cLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel1.ForeColor = System.Drawing.Color.Black;
            this.cLabel1.Location = new System.Drawing.Point(332, 14);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(64, 13);
            this.cLabel1.TabIndex = 0;
            this.cLabel1.Text = "Stock No";
            this.cLabel1.ToolTips = "";
            // 
            // txtStockNo
            // 
            this.txtStockNo.ActivationColor = false;
            this.txtStockNo.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtStockNo.AllowTabKeyOnEnter = false;
            this.txtStockNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStockNo.Format = "";
            this.txtStockNo.IsComplusory = false;
            this.txtStockNo.Location = new System.Drawing.Point(399, 5);
            this.txtStockNo.Multiline = true;
            this.txtStockNo.Name = "txtStockNo";
            this.txtStockNo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStockNo.SelectAllTextOnFocus = true;
            this.txtStockNo.Size = new System.Drawing.Size(241, 53);
            this.txtStockNo.TabIndex = 1;
            this.txtStockNo.ToolTips = "";
            this.txtStockNo.WaterMarkText = null;
            this.txtStockNo.TextChanged += new System.EventHandler(this.txtStockNo_TextChanged);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Location = new System.Drawing.Point(655, 18);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(90, 31);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "Show";
            this.btnSearch.ToolTips = "";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // FrmOfferPriceReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(966, 464);
            this.Controls.Add(this.MainGrid);
            this.Controls.Add(this.PanelTop);
            this.Name = "FrmOfferPriceReport";
            this.Tag = "OfferPriceReport";
            this.Text = "OFFER PRICE REPORT";
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepBtnDelete)).EndInit();
            this.PanelTop.ResumeLayout(false);
            this.PanelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChkCmbParty.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl MainGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView GrdDetail;
        private AxonContLib.cPanel PanelTop;
        private AxonContLib.cLabel cLabel1;
        private AxonContLib.cTextBox txtStockNo;
        private AxonContLib.cButton btnSearch;
        private DevExpress.XtraGrid.Columns.GridColumn OFFERPRICE_ID;
        private DevExpress.XtraGrid.Columns.GridColumn STOCK_ID;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private AxonContLib.cButton btnExit;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private AxonContLib.cDateTimePicker DTPFromDate;
        private AxonContLib.cDateTimePicker DTPToDate;
        private AxonContLib.cLabel cLabel2;
        private AxonContLib.cLabel cLabel8;
        private AxonContLib.cLabel cLabel3;
        private DevControlLib.cDevCheckedComboBoxEdit ChkCmbParty;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit RepBtnDelete;
        private DevControlLib.cDevSimpleButton btnExport;
    }
}