namespace MahantExport.Parcel
{
    partial class FrmParcelBoxMaster
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmParcelBoxMaster));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteSelectedItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtnAdd = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnExport = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnSave = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnBack = new DevControlLib.cDevSimpleButton(this.components);
            this.RepTxtExpPricePerCarat = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.GrdBox = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RepTxtShape = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RepTxtMixSize = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TxtRepMixCla = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn15 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn17 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.RepTxtDepartment = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn18 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repChkISOneTImeUpdate = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn19 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn20 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn21 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn22 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.MainGrdBox = new DevExpress.XtraGrid.GridControl();
            this.behaviorManager1 = new DevExpress.Utils.Behaviors.BehaviorManager(this.components);
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RepTxtExpPricePerCarat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepTxtShape)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepTxtMixSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtRepMixCla)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepTxtDepartment)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repChkISOneTImeUpdate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrdBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteSelectedItemsToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(182, 26);
            // 
            // deleteSelectedItemsToolStripMenuItem
            // 
            this.deleteSelectedItemsToolStripMenuItem.Name = "deleteSelectedItemsToolStripMenuItem";
            this.deleteSelectedItemsToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.deleteSelectedItemsToolStripMenuItem.Text = "Delete Selected Item";
            this.deleteSelectedItemsToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectedItemsToolStripMenuItem_Click);
            // 
            // panelControl1
            // 
            this.panelControl1.Controls.Add(this.BtnAdd);
            this.panelControl1.Controls.Add(this.BtnExport);
            this.panelControl1.Controls.Add(this.BtnSave);
            this.panelControl1.Controls.Add(this.BtnBack);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControl1.Location = new System.Drawing.Point(0, 392);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(1388, 52);
            this.panelControl1.TabIndex = 1;
            this.panelControl1.TabStop = true;
            // 
            // BtnAdd
            // 
            this.BtnAdd.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnAdd.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnAdd.Appearance.Options.UseFont = true;
            this.BtnAdd.Appearance.Options.UseForeColor = true;
            this.BtnAdd.ImageOptions.Image = global::MahantExport.Properties.Resources.btnclear;
            this.BtnAdd.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnAdd.ImageOptions.SvgImage")));
            this.BtnAdd.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnAdd.Location = new System.Drawing.Point(116, 9);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(103, 35);
            this.BtnAdd.TabIndex = 1;
            this.BtnAdd.TabStop = false;
            this.BtnAdd.Text = "&Clear";
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
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
            this.BtnExport.Location = new System.Drawing.Point(223, 9);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(103, 35);
            this.BtnExport.TabIndex = 2;
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
            this.BtnSave.Location = new System.Drawing.Point(8, 9);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(103, 35);
            this.BtnSave.TabIndex = 0;
            this.BtnSave.Text = "&Save";
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
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
            this.BtnBack.Location = new System.Drawing.Point(332, 9);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.Size = new System.Drawing.Size(103, 35);
            this.BtnBack.TabIndex = 3;
            this.BtnBack.TabStop = false;
            this.BtnBack.Text = "E&xit";
            this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // RepTxtExpPricePerCarat
            // 
            this.RepTxtExpPricePerCarat.AutoHeight = false;
            this.RepTxtExpPricePerCarat.Name = "RepTxtExpPricePerCarat";
            this.RepTxtExpPricePerCarat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.RepTxtExpPricePerCarat_KeyDown);
            // 
            // GrdBox
            // 
            this.GrdBox.Appearance.FocusedCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.GrdBox.Appearance.FocusedCell.Options.UseFont = true;
            this.GrdBox.Appearance.FocusedRow.Font = new System.Drawing.Font("Verdana", 8F);
            this.GrdBox.Appearance.FocusedRow.Options.UseFont = true;
            this.GrdBox.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.GrdBox.Appearance.HeaderPanel.Options.UseFont = true;
            this.GrdBox.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.GrdBox.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdBox.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.GrdBox.Appearance.HorzLine.BackColor = System.Drawing.Color.Gray;
            this.GrdBox.Appearance.HorzLine.Options.UseBackColor = true;
            this.GrdBox.Appearance.Row.Font = new System.Drawing.Font("Verdana", 8F);
            this.GrdBox.Appearance.Row.Options.UseFont = true;
            this.GrdBox.Appearance.VertLine.BackColor = System.Drawing.Color.Gray;
            this.GrdBox.Appearance.VertLine.Options.UseBackColor = true;
            this.GrdBox.ColumnPanelRowHeight = 40;
            this.GrdBox.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn13,
            this.gridColumn14,
            this.gridColumn15,
            this.gridColumn16,
            this.gridColumn17,
            this.gridColumn18,
            this.gridColumn19,
            this.gridColumn20,
            this.gridColumn21,
            this.gridColumn22});
            this.GrdBox.GridControl = this.MainGrdBox;
            this.GrdBox.Name = "GrdBox";
            this.GrdBox.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.GrdBox.OptionsView.ColumnAutoWidth = false;
            this.GrdBox.OptionsView.ShowFooter = true;
            this.GrdBox.OptionsView.ShowGroupPanel = false;
            this.GrdBox.RowHeight = 25;
            this.GrdBox.CustomSummaryCalculate += new DevExpress.Data.CustomSummaryEventHandler(this.GrdBox_CustomSummaryCalculate);
            this.GrdBox.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.GrdBox_FocusedRowChanged);
            this.GrdBox.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.GrdBox_CellValueChanged);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn1.AppearanceCell.Options.UseFont = true;
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "STOCK_ID";
            this.gridColumn1.FieldName = "STOCK_ID";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn2.AppearanceCell.Options.UseFont = true;
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "DEPARTMENT_ID";
            this.gridColumn2.FieldName = "DEPARTMENT_ID";
            this.gridColumn2.Name = "gridColumn2";
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn3.AppearanceCell.Options.UseFont = true;
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "SHAPE_ID";
            this.gridColumn3.FieldName = "SHAPE_ID";
            this.gridColumn3.Name = "gridColumn3";
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn4.AppearanceCell.Options.UseFont = true;
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "Shape ";
            this.gridColumn4.ColumnEdit = this.RepTxtShape;
            this.gridColumn4.FieldName = "SHAPENAME";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 1;
            this.gridColumn4.Width = 86;
            // 
            // RepTxtShape
            // 
            this.RepTxtShape.AutoHeight = false;
            this.RepTxtShape.Name = "RepTxtShape";
            this.RepTxtShape.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.RepTxtShape_KeyPress);
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn5.AppearanceCell.Options.UseFont = true;
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "MIXSIZE_ID";
            this.gridColumn5.FieldName = "MIXSIZE_ID";
            this.gridColumn5.Name = "gridColumn5";
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn6.AppearanceCell.Options.UseFont = true;
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "Mix Size";
            this.gridColumn6.ColumnEdit = this.RepTxtMixSize;
            this.gridColumn6.FieldName = "MIXSIZENAME";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 3;
            // 
            // RepTxtMixSize
            // 
            this.RepTxtMixSize.AutoHeight = false;
            this.RepTxtMixSize.Name = "RepTxtMixSize";
            this.RepTxtMixSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.RepTxtMixSize_KeyPress);
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn7.AppearanceCell.Options.UseFont = true;
            this.gridColumn7.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.Caption = "MIXCLARITY_ID";
            this.gridColumn7.FieldName = "MIXCLARITY_ID";
            this.gridColumn7.Name = "gridColumn7";
            // 
            // gridColumn8
            // 
            this.gridColumn8.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn8.AppearanceCell.Options.UseFont = true;
            this.gridColumn8.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn8.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn8.Caption = "Mix Cla";
            this.gridColumn8.ColumnEdit = this.TxtRepMixCla;
            this.gridColumn8.FieldName = "MIXCLARITYNAME";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 2;
            this.gridColumn8.Width = 92;
            // 
            // TxtRepMixCla
            // 
            this.TxtRepMixCla.AutoHeight = false;
            this.TxtRepMixCla.Name = "TxtRepMixCla";
            this.TxtRepMixCla.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TxtRepMixCla_KeyPress);
            // 
            // gridColumn9
            // 
            this.gridColumn9.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn9.AppearanceCell.Options.UseFont = true;
            this.gridColumn9.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn9.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn9.Caption = "Opening Cts";
            this.gridColumn9.FieldName = "OPENINGCARAT";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 7;
            this.gridColumn9.Width = 100;
            // 
            // gridColumn10
            // 
            this.gridColumn10.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn10.AppearanceCell.Options.UseFont = true;
            this.gridColumn10.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn10.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn10.Caption = "Opening $/Cts";
            this.gridColumn10.FieldName = "OPENINGPRICEPERCARAT";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Custom)});
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 8;
            this.gridColumn10.Width = 100;
            // 
            // gridColumn11
            // 
            this.gridColumn11.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn11.AppearanceCell.Options.UseFont = true;
            this.gridColumn11.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn11.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn11.Caption = "Opening Amt";
            this.gridColumn11.FieldName = "OPENINGAMOUNT";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsColumn.AllowEdit = false;
            this.gridColumn11.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 9;
            this.gridColumn11.Width = 100;
            // 
            // gridColumn12
            // 
            this.gridColumn12.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn12.AppearanceCell.Options.UseFont = true;
            this.gridColumn12.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn12.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn12.Caption = "MFG $/Cts";
            this.gridColumn12.FieldName = "MFGPRICEPERCARAT";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 10;
            this.gridColumn12.Width = 100;
            // 
            // gridColumn13
            // 
            this.gridColumn13.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn13.AppearanceCell.Options.UseFont = true;
            this.gridColumn13.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn13.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn13.Caption = "Cost $/Cts";
            this.gridColumn13.FieldName = "COSTPRICEPERCARAT";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 11;
            this.gridColumn13.Width = 100;
            // 
            // gridColumn14
            // 
            this.gridColumn14.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn14.AppearanceCell.Options.UseFont = true;
            this.gridColumn14.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn14.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn14.Caption = "Sale $/Cts";
            this.gridColumn14.FieldName = "SALEPRICEPERCARAT";
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 12;
            this.gridColumn14.Width = 100;
            // 
            // gridColumn15
            // 
            this.gridColumn15.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn15.AppearanceCell.Options.UseFont = true;
            this.gridColumn15.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn15.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn15.Caption = "Exp $/Cts";
            this.gridColumn15.ColumnEdit = this.RepTxtExpPricePerCarat;
            this.gridColumn15.FieldName = "EXPPRICEPERCARAT";
            this.gridColumn15.Name = "gridColumn15";
            this.gridColumn15.Visible = true;
            this.gridColumn15.VisibleIndex = 13;
            this.gridColumn15.Width = 100;
            // 
            // gridColumn16
            // 
            this.gridColumn16.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn16.AppearanceCell.Options.UseFont = true;
            this.gridColumn16.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn16.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn16.Caption = "Stock No";
            this.gridColumn16.FieldName = "STOCKNO";
            this.gridColumn16.Name = "gridColumn16";
            this.gridColumn16.OptionsColumn.AllowEdit = false;
            this.gridColumn16.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn16.Visible = true;
            this.gridColumn16.VisibleIndex = 4;
            this.gridColumn16.Width = 153;
            // 
            // gridColumn17
            // 
            this.gridColumn17.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn17.AppearanceCell.Options.UseFont = true;
            this.gridColumn17.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn17.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn17.Caption = "Department";
            this.gridColumn17.ColumnEdit = this.RepTxtDepartment;
            this.gridColumn17.FieldName = "DEPARTMENTNAME";
            this.gridColumn17.Name = "gridColumn17";
            this.gridColumn17.Visible = true;
            this.gridColumn17.VisibleIndex = 0;
            this.gridColumn17.Width = 94;
            // 
            // RepTxtDepartment
            // 
            this.RepTxtDepartment.AccessibleName = "";
            this.RepTxtDepartment.AutoHeight = false;
            this.RepTxtDepartment.Name = "RepTxtDepartment";
            this.RepTxtDepartment.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.RepTxtDepartment_KeyPress);
            // 
            // gridColumn18
            // 
            this.gridColumn18.Caption = "ISOneTimeUpdate";
            this.gridColumn18.ColumnEdit = this.repChkISOneTImeUpdate;
            this.gridColumn18.FieldName = "ISONETIMEUPDATE";
            this.gridColumn18.Name = "gridColumn18";
            // 
            // repChkISOneTImeUpdate
            // 
            this.repChkISOneTImeUpdate.AutoHeight = false;
            this.repChkISOneTImeUpdate.Name = "repChkISOneTImeUpdate";
            this.repChkISOneTImeUpdate.NullStyle = DevExpress.XtraEditors.Controls.StyleIndeterminate.Unchecked;
            // 
            // gridColumn19
            // 
            this.gridColumn19.Caption = "IsSalePricePerCarat";
            this.gridColumn19.FieldName = "ISSALEPRICEPERCARAT";
            this.gridColumn19.Name = "gridColumn19";
            // 
            // gridColumn20
            // 
            this.gridColumn20.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn20.AppearanceCell.Options.UseFont = true;
            this.gridColumn20.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn20.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn20.Caption = "Balance Carat";
            this.gridColumn20.FieldName = "BALANCECARAT";
            this.gridColumn20.Name = "gridColumn20";
            this.gridColumn20.Width = 123;
            // 
            // gridColumn21
            // 
            this.gridColumn21.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn21.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn21.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn21.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn21.Caption = "ParcelGroupNo";
            this.gridColumn21.FieldName = "PARCELGROUPNO";
            this.gridColumn21.Name = "gridColumn21";
            this.gridColumn21.Visible = true;
            this.gridColumn21.VisibleIndex = 5;
            this.gridColumn21.Width = 112;
            // 
            // gridColumn22
            // 
            this.gridColumn22.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn22.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn22.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn22.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn22.Caption = "ParcelSeqNo";
            this.gridColumn22.FieldName = "PARCELSEQNO";
            this.gridColumn22.Name = "gridColumn22";
            this.gridColumn22.Visible = true;
            this.gridColumn22.VisibleIndex = 6;
            this.gridColumn22.Width = 111;
            // 
            // MainGrdBox
            // 
            this.MainGrdBox.ContextMenuStrip = this.contextMenuStrip1;
            this.MainGrdBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGrdBox.EmbeddedNavigator.ContextMenuStrip = this.contextMenuStrip1;
            this.MainGrdBox.Location = new System.Drawing.Point(0, 0);
            this.MainGrdBox.MainView = this.GrdBox;
            this.MainGrdBox.Name = "MainGrdBox";
            this.MainGrdBox.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.RepTxtExpPricePerCarat,
            this.RepTxtDepartment,
            this.RepTxtMixSize,
            this.RepTxtShape,
            this.TxtRepMixCla,
            this.repChkISOneTImeUpdate});
            this.MainGrdBox.Size = new System.Drawing.Size(1388, 392);
            this.MainGrdBox.TabIndex = 0;
            this.MainGrdBox.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GrdBox});
            // 
            // FrmParcelBoxMaster
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1388, 444);
            this.Controls.Add(this.MainGrdBox);
            this.Controls.Add(this.panelControl1);
            this.LookAndFeel.SkinName = "London Liquid Sky";
            this.LookAndFeel.UseDefaultLookAndFeel =true;
            this.Name = "FrmParcelBoxMaster";
            this.Tag = "";
            this.Text = "PARCEL BOX MASTER";
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.RepTxtExpPricePerCarat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepTxtShape)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepTxtMixSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtRepMixCla)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepTxtDepartment)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repChkISOneTImeUpdate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrdBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.behaviorManager1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevControlLib.cDevSimpleButton BtnExport;
        private DevControlLib.cDevSimpleButton BtnSave;
        private DevControlLib.cDevSimpleButton BtnAdd;
        private DevControlLib.cDevSimpleButton BtnBack;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectedItemsToolStripMenuItem;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit RepTxtExpPricePerCarat;
        private DevExpress.XtraGrid.Views.Grid.GridView GrdBox;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn15;
        private DevExpress.XtraGrid.GridControl MainGrdBox;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit RepTxtDepartment;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn16;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn17;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit RepTxtMixSize;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit RepTxtShape;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit TxtRepMixCla;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn18;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repChkISOneTImeUpdate;
        private DevExpress.Utils.Behaviors.BehaviorManager behaviorManager1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn19;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn20;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn21;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn22;
    }
}