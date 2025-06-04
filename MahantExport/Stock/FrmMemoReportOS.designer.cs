namespace MahantExport.Masters
{
    partial class FrmMemoReportOS
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
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMemoReportOS));
            this.MainGrid = new DevExpress.XtraGrid.GridControl();
            this.GrdDet = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.gridBand1 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.bandedGridColumn1 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn2 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand2 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.bandedGridColumn3 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn4 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn12 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand3 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.bandedGridColumn5 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn6 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn13 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.gridBand4 = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.bandedGridColumn7 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn8 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.bandedGridColumn9 = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn();
            this.BtnPrint = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.BtnExport = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnSearch = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnBack = new DevControlLib.cDevSimpleButton(this.components);
            this.panel4 = new AxonContLib.cPanel(this.components);
            this.CmbReportType = new System.Windows.Forms.ComboBox();
            this.cLabel4 = new AxonContLib.cLabel(this.components);
            this.DTPASOnDate = new AxonContLib.cDateTimePicker(this.components);
            this.cLabel8 = new AxonContLib.cLabel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BtnPrint)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainGrid
            // 
            this.MainGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGrid.Location = new System.Drawing.Point(0, 44);
            this.MainGrid.MainView = this.GrdDet;
            this.MainGrid.Name = "MainGrid";
            this.MainGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.BtnPrint});
            this.MainGrid.Size = new System.Drawing.Size(1196, 367);
            this.MainGrid.TabIndex = 0;
            this.MainGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GrdDet});
            this.MainGrid.Paint += new System.Windows.Forms.PaintEventHandler(this.MainGrid_Paint);
            // 
            // GrdDet
            // 
            this.GrdDet.Appearance.BandPanel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.BandPanel.Options.UseFont = true;
            this.GrdDet.Appearance.BandPanel.Options.UseTextOptions = true;
            this.GrdDet.Appearance.BandPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDet.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.GrdDet.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.GrdDet.Appearance.FocusedCell.Options.UseBackColor = true;
            this.GrdDet.Appearance.FooterPanel.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.FooterPanel.Options.UseFont = true;
            this.GrdDet.Appearance.GroupFooter.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.GroupFooter.Options.UseFont = true;
            this.GrdDet.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.HeaderPanel.Options.UseFont = true;
            this.GrdDet.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.GrdDet.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDet.Appearance.HorzLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDet.Appearance.HorzLine.Options.UseBackColor = true;
            this.GrdDet.Appearance.Row.Font = new System.Drawing.Font("Verdana", 8F);
            this.GrdDet.Appearance.Row.Options.UseFont = true;
            this.GrdDet.Appearance.VertLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDet.Appearance.VertLine.Options.UseBackColor = true;
            this.GrdDet.AppearancePrint.FooterPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.GrdDet.AppearancePrint.FooterPanel.Options.UseFont = true;
            this.GrdDet.AppearancePrint.HeaderPanel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.GrdDet.AppearancePrint.HeaderPanel.Options.UseFont = true;
            this.GrdDet.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.gridBand1,
            this.gridBand2,
            this.gridBand3,
            this.gridBand4});
            this.GrdDet.Columns.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn[] {
            this.bandedGridColumn1,
            this.bandedGridColumn2,
            this.bandedGridColumn3,
            this.bandedGridColumn4,
            this.bandedGridColumn12,
            this.bandedGridColumn5,
            this.bandedGridColumn6,
            this.bandedGridColumn13,
            this.bandedGridColumn7,
            this.bandedGridColumn8,
            this.bandedGridColumn9});
            this.GrdDet.GridControl = this.MainGrid;
            this.GrdDet.Name = "GrdDet";
            this.GrdDet.OptionsBehavior.Editable = false;
            this.GrdDet.OptionsFilter.AllowFilterEditor = false;
            this.GrdDet.OptionsNavigation.EnterMoveNextColumn = true;
            this.GrdDet.OptionsPrint.ExpandAllGroups = false;
            this.GrdDet.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.GrdDet.OptionsView.ColumnAutoWidth = false;
            this.GrdDet.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.GrdDet.OptionsView.ShowAutoFilterRow = true;
            this.GrdDet.OptionsView.ShowFooter = true;
            this.GrdDet.OptionsView.ShowGroupPanel = false;
            // 
            // gridBand1
            // 
            this.gridBand1.Caption = "General";
            this.gridBand1.Columns.Add(this.bandedGridColumn1);
            this.gridBand1.Columns.Add(this.bandedGridColumn2);
            this.gridBand1.Name = "gridBand1";
            this.gridBand1.VisibleIndex = 0;
            this.gridBand1.Width = 310;
            // 
            // bandedGridColumn1
            // 
            this.bandedGridColumn1.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn1.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn1.AppearanceCell.Options.UseFont = true;
            this.bandedGridColumn1.AppearanceCell.Options.UseForeColor = true;
            this.bandedGridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn1.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn1.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn1.AppearanceHeader.Options.UseFont = true;
            this.bandedGridColumn1.AppearanceHeader.Options.UseForeColor = true;
            this.bandedGridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.bandedGridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn1.Caption = "Party Name";
            this.bandedGridColumn1.FieldName = "BILLINGPARTYNAME";
            this.bandedGridColumn1.Name = "bandedGridColumn1";
            this.bandedGridColumn1.Visible = true;
            this.bandedGridColumn1.Width = 222;
            // 
            // bandedGridColumn2
            // 
            this.bandedGridColumn2.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn2.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn2.AppearanceCell.Options.UseFont = true;
            this.bandedGridColumn2.AppearanceCell.Options.UseForeColor = true;
            this.bandedGridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn2.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn2.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn2.AppearanceHeader.Options.UseFont = true;
            this.bandedGridColumn2.AppearanceHeader.Options.UseForeColor = true;
            this.bandedGridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.bandedGridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn2.Caption = "Stock No";
            this.bandedGridColumn2.FieldName = "STOCKNO";
            this.bandedGridColumn2.Name = "bandedGridColumn2";
            this.bandedGridColumn2.Visible = true;
            this.bandedGridColumn2.Width = 88;
            // 
            // gridBand2
            // 
            this.gridBand2.Caption = "Single";
            this.gridBand2.Columns.Add(this.bandedGridColumn3);
            this.gridBand2.Columns.Add(this.bandedGridColumn4);
            this.gridBand2.Columns.Add(this.bandedGridColumn12);
            this.gridBand2.Name = "gridBand2";
            this.gridBand2.VisibleIndex = 1;
            this.gridBand2.Width = 250;
            // 
            // bandedGridColumn3
            // 
            this.bandedGridColumn3.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn3.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn3.AppearanceCell.Options.UseFont = true;
            this.bandedGridColumn3.AppearanceCell.Options.UseForeColor = true;
            this.bandedGridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn3.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn3.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn3.AppearanceHeader.Options.UseFont = true;
            this.bandedGridColumn3.AppearanceHeader.Options.UseForeColor = true;
            this.bandedGridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.bandedGridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn3.Caption = "Pcs";
            this.bandedGridColumn3.FieldName = "SINGLEPCS";
            this.bandedGridColumn3.Name = "bandedGridColumn3";
            this.bandedGridColumn3.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.bandedGridColumn3.Visible = true;
            this.bandedGridColumn3.Width = 70;
            // 
            // bandedGridColumn4
            // 
            this.bandedGridColumn4.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn4.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn4.AppearanceCell.Options.UseFont = true;
            this.bandedGridColumn4.AppearanceCell.Options.UseForeColor = true;
            this.bandedGridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn4.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn4.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn4.AppearanceHeader.Options.UseFont = true;
            this.bandedGridColumn4.AppearanceHeader.Options.UseForeColor = true;
            this.bandedGridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.bandedGridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn4.Caption = "Carat";
            this.bandedGridColumn4.FieldName = "SINGLECARAT";
            this.bandedGridColumn4.Name = "bandedGridColumn4";
            this.bandedGridColumn4.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.bandedGridColumn4.Visible = true;
            this.bandedGridColumn4.Width = 80;
            // 
            // bandedGridColumn12
            // 
            this.bandedGridColumn12.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn12.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn12.AppearanceCell.Options.UseFont = true;
            this.bandedGridColumn12.AppearanceCell.Options.UseForeColor = true;
            this.bandedGridColumn12.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn12.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn12.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn12.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn12.AppearanceHeader.Options.UseFont = true;
            this.bandedGridColumn12.AppearanceHeader.Options.UseForeColor = true;
            this.bandedGridColumn12.AppearanceHeader.Options.UseTextOptions = true;
            this.bandedGridColumn12.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn12.Caption = "Amt $";
            this.bandedGridColumn12.FieldName = "SINGLEAMOUNT";
            this.bandedGridColumn12.Name = "bandedGridColumn12";
            this.bandedGridColumn12.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.bandedGridColumn12.Visible = true;
            this.bandedGridColumn12.Width = 100;
            // 
            // gridBand3
            // 
            this.gridBand3.Caption = "Parcel";
            this.gridBand3.Columns.Add(this.bandedGridColumn5);
            this.gridBand3.Columns.Add(this.bandedGridColumn6);
            this.gridBand3.Columns.Add(this.bandedGridColumn13);
            this.gridBand3.Name = "gridBand3";
            this.gridBand3.VisibleIndex = 2;
            this.gridBand3.Width = 250;
            // 
            // bandedGridColumn5
            // 
            this.bandedGridColumn5.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn5.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn5.AppearanceCell.Options.UseFont = true;
            this.bandedGridColumn5.AppearanceCell.Options.UseForeColor = true;
            this.bandedGridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn5.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn5.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn5.AppearanceHeader.Options.UseFont = true;
            this.bandedGridColumn5.AppearanceHeader.Options.UseForeColor = true;
            this.bandedGridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.bandedGridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn5.Caption = "Pcs";
            this.bandedGridColumn5.FieldName = "PARCELPCS";
            this.bandedGridColumn5.Name = "bandedGridColumn5";
            this.bandedGridColumn5.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.bandedGridColumn5.Visible = true;
            this.bandedGridColumn5.Width = 70;
            // 
            // bandedGridColumn6
            // 
            this.bandedGridColumn6.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn6.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn6.AppearanceCell.Options.UseFont = true;
            this.bandedGridColumn6.AppearanceCell.Options.UseForeColor = true;
            this.bandedGridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn6.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn6.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn6.AppearanceHeader.Options.UseFont = true;
            this.bandedGridColumn6.AppearanceHeader.Options.UseForeColor = true;
            this.bandedGridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.bandedGridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn6.Caption = "Carat";
            this.bandedGridColumn6.FieldName = "PARCELCARAT";
            this.bandedGridColumn6.Name = "bandedGridColumn6";
            this.bandedGridColumn6.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.bandedGridColumn6.Visible = true;
            this.bandedGridColumn6.Width = 80;
            // 
            // bandedGridColumn13
            // 
            this.bandedGridColumn13.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn13.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn13.AppearanceCell.Options.UseFont = true;
            this.bandedGridColumn13.AppearanceCell.Options.UseForeColor = true;
            this.bandedGridColumn13.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn13.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn13.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn13.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn13.AppearanceHeader.Options.UseFont = true;
            this.bandedGridColumn13.AppearanceHeader.Options.UseForeColor = true;
            this.bandedGridColumn13.AppearanceHeader.Options.UseTextOptions = true;
            this.bandedGridColumn13.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn13.Caption = "Amt $";
            this.bandedGridColumn13.FieldName = "PARCELAMOUNT";
            this.bandedGridColumn13.Name = "bandedGridColumn13";
            this.bandedGridColumn13.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.bandedGridColumn13.Visible = true;
            this.bandedGridColumn13.Width = 100;
            // 
            // gridBand4
            // 
            this.gridBand4.Caption = "Total";
            this.gridBand4.Columns.Add(this.bandedGridColumn7);
            this.gridBand4.Columns.Add(this.bandedGridColumn8);
            this.gridBand4.Columns.Add(this.bandedGridColumn9);
            this.gridBand4.Name = "gridBand4";
            this.gridBand4.VisibleIndex = 3;
            this.gridBand4.Width = 250;
            // 
            // bandedGridColumn7
            // 
            this.bandedGridColumn7.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn7.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn7.AppearanceCell.Options.UseFont = true;
            this.bandedGridColumn7.AppearanceCell.Options.UseForeColor = true;
            this.bandedGridColumn7.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn7.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn7.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn7.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn7.AppearanceHeader.Options.UseFont = true;
            this.bandedGridColumn7.AppearanceHeader.Options.UseForeColor = true;
            this.bandedGridColumn7.AppearanceHeader.Options.UseTextOptions = true;
            this.bandedGridColumn7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn7.Caption = "Pcs";
            this.bandedGridColumn7.FieldName = "TOTALPCS";
            this.bandedGridColumn7.Name = "bandedGridColumn7";
            this.bandedGridColumn7.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.bandedGridColumn7.Visible = true;
            this.bandedGridColumn7.Width = 70;
            // 
            // bandedGridColumn8
            // 
            this.bandedGridColumn8.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn8.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn8.AppearanceCell.Options.UseFont = true;
            this.bandedGridColumn8.AppearanceCell.Options.UseForeColor = true;
            this.bandedGridColumn8.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn8.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn8.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn8.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn8.AppearanceHeader.Options.UseFont = true;
            this.bandedGridColumn8.AppearanceHeader.Options.UseForeColor = true;
            this.bandedGridColumn8.AppearanceHeader.Options.UseTextOptions = true;
            this.bandedGridColumn8.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn8.Caption = "Carat";
            this.bandedGridColumn8.FieldName = "TOTALCARAT";
            this.bandedGridColumn8.Name = "bandedGridColumn8";
            this.bandedGridColumn8.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.bandedGridColumn8.Visible = true;
            this.bandedGridColumn8.Width = 80;
            // 
            // bandedGridColumn9
            // 
            this.bandedGridColumn9.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn9.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn9.AppearanceCell.Options.UseFont = true;
            this.bandedGridColumn9.AppearanceCell.Options.UseForeColor = true;
            this.bandedGridColumn9.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn9.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn9.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bandedGridColumn9.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.bandedGridColumn9.AppearanceHeader.Options.UseFont = true;
            this.bandedGridColumn9.AppearanceHeader.Options.UseForeColor = true;
            this.bandedGridColumn9.AppearanceHeader.Options.UseTextOptions = true;
            this.bandedGridColumn9.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn9.Caption = "Amt $";
            this.bandedGridColumn9.FieldName = "TOTALAMOUNT";
            this.bandedGridColumn9.Name = "bandedGridColumn9";
            this.bandedGridColumn9.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.bandedGridColumn9.Visible = true;
            this.bandedGridColumn9.Width = 100;
            // 
            // BtnPrint
            // 
            this.BtnPrint.AutoHeight = false;
            editorButtonImageOptions1.Image = global::MahantExport.Properties.Resources.Print;
            this.BtnPrint.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // BtnExport
            // 
            this.BtnExport.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnExport.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnExport.Appearance.Options.UseFont = true;
            this.BtnExport.Appearance.Options.UseForeColor = true;
            this.BtnExport.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnExport.ImageOptions.SvgImage")));
            this.BtnExport.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnExport.Location = new System.Drawing.Point(639, 5);
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
            this.BtnSearch.Location = new System.Drawing.Point(560, 5);
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
            this.BtnBack.Location = new System.Drawing.Point(715, 5);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.Size = new System.Drawing.Size(73, 33);
            this.BtnBack.TabIndex = 32;
            this.BtnBack.TabStop = false;
            this.BtnBack.Text = "E&xit";
            this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.CmbReportType);
            this.panel4.Controls.Add(this.cLabel4);
            this.panel4.Controls.Add(this.DTPASOnDate);
            this.panel4.Controls.Add(this.cLabel8);
            this.panel4.Controls.Add(this.BtnExport);
            this.panel4.Controls.Add(this.BtnBack);
            this.panel4.Controls.Add(this.BtnSearch);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1196, 44);
            this.panel4.TabIndex = 0;
            // 
            // CmbReportType
            // 
            this.CmbReportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbReportType.Font = new System.Drawing.Font("Verdana", 9F);
            this.CmbReportType.FormattingEnabled = true;
            this.CmbReportType.Items.AddRange(new object[] {
            "PARTY WISE",
            "STONENO WISE",
            "PARTY + STONENO WISE"});
            this.CmbReportType.Location = new System.Drawing.Point(246, 12);
            this.CmbReportType.Name = "CmbReportType";
            this.CmbReportType.Size = new System.Drawing.Size(260, 22);
            this.CmbReportType.TabIndex = 182;
            // 
            // cLabel4
            // 
            this.cLabel4.AutoSize = true;
            this.cLabel4.Font = new System.Drawing.Font("Verdana", 9F);
            this.cLabel4.ForeColor = System.Drawing.Color.Black;
            this.cLabel4.Location = new System.Drawing.Point(199, 15);
            this.cLabel4.Name = "cLabel4";
            this.cLabel4.Size = new System.Drawing.Size(41, 14);
            this.cLabel4.TabIndex = 183;
            this.cLabel4.Text = "Stock";
            this.cLabel4.ToolTips = "";
            // 
            // DTPASOnDate
            // 
            this.DTPASOnDate.AllowTabKeyOnEnter = false;
            this.DTPASOnDate.CustomFormat = "dd/MM/yyyy";
            this.DTPASOnDate.Font = new System.Drawing.Font("Verdana", 9F);
            this.DTPASOnDate.ForeColor = System.Drawing.Color.Black;
            this.DTPASOnDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTPASOnDate.Location = new System.Drawing.Point(91, 12);
            this.DTPASOnDate.Name = "DTPASOnDate";
            this.DTPASOnDate.Size = new System.Drawing.Size(102, 22);
            this.DTPASOnDate.TabIndex = 0;
            this.DTPASOnDate.ToolTips = "";
            // 
            // cLabel8
            // 
            this.cLabel8.AutoSize = true;
            this.cLabel8.Font = new System.Drawing.Font("Verdana", 9F);
            this.cLabel8.ForeColor = System.Drawing.Color.Black;
            this.cLabel8.Location = new System.Drawing.Point(10, 15);
            this.cLabel8.Name = "cLabel8";
            this.cLabel8.Size = new System.Drawing.Size(79, 14);
            this.cLabel8.TabIndex = 35;
            this.cLabel8.Text = "AS On Date";
            this.cLabel8.ToolTips = "";
            // 
            // FrmMemoReportOS
            // 
            this.Appearance.BackColor = System.Drawing.Color.White;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1196, 411);
            this.Controls.Add(this.MainGrid);
            this.Controls.Add(this.panel4);
            this.Name = "FrmMemoReportOS";
            this.Tag = "FrmMemoOSReport";
            this.Text = "OUTSTANDING MEMO";
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BtnPrint)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl MainGrid;
        private DevControlLib.cDevSimpleButton BtnExport;
        private DevControlLib.cDevSimpleButton BtnSearch;
        private DevControlLib.cDevSimpleButton BtnBack;
        private AxonContLib.cPanel panel4;
        private AxonContLib.cLabel cLabel8;
        private AxonContLib.cDateTimePicker DTPASOnDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit BtnPrint;
        private System.Windows.Forms.ComboBox CmbReportType;
        private AxonContLib.cLabel cLabel4;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView GrdDet;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn1;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn2;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn3;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn4;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn12;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn5;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn6;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn13;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn7;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn8;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn bandedGridColumn9;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand1;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand2;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand3;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand gridBand4;


    }
}