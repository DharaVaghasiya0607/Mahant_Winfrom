namespace MahantExport.UserActivities
{
    partial class FrmSearchHistory
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSearchHistory));
            this.panel1 = new AxonContLib.cPanel(this.components);
            this.BtnExport = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnExit = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnShow = new DevControlLib.cDevSimpleButton(this.components);
            this.DTPToDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new AxonContLib.cLabel(this.components);
            this.DTPFromDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new AxonContLib.cLabel(this.components);
            this.cLabel28 = new AxonContLib.cLabel(this.components);
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.MainGrdDetail = new DevExpress.XtraGrid.GridControl();
            this.GrdDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.bandedGridColumn60 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.bandedGridColumn61 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.bandedGridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView3 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrdDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BtnExport);
            this.panel1.Controls.Add(this.BtnExit);
            this.panel1.Controls.Add(this.BtnShow);
            this.panel1.Controls.Add(this.DTPToDate);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.DTPFromDate);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(814, 42);
            this.panel1.TabIndex = 0;
            // 
            // BtnExport
            // 
            this.BtnExport.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnExport.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnExport.Appearance.Options.UseFont = true;
            this.BtnExport.Appearance.Options.UseForeColor = true;
            this.BtnExport.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnExport.ImageOptions.SvgImage")));
            this.BtnExport.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnExport.Location = new System.Drawing.Point(511, 6);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(85, 31);
            this.BtnExport.TabIndex = 6;
            this.BtnExport.Text = "Export";
            this.BtnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnExit.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnExit.Appearance.Options.UseFont = true;
            this.BtnExit.Appearance.Options.UseForeColor = true;
            this.BtnExit.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnExit.ImageOptions.SvgImage")));
            this.BtnExit.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnExit.Location = new System.Drawing.Point(602, 6);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(82, 31);
            this.BtnExit.TabIndex = 5;
            this.BtnExit.Text = "Exit";
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // BtnShow
            // 
            this.BtnShow.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnShow.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnShow.Appearance.Options.UseFont = true;
            this.BtnShow.Appearance.Options.UseForeColor = true;
            this.BtnShow.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnShow.ImageOptions.SvgImage")));
            this.BtnShow.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnShow.Location = new System.Drawing.Point(423, 6);
            this.BtnShow.Name = "BtnShow";
            this.BtnShow.Size = new System.Drawing.Size(82, 31);
            this.BtnShow.TabIndex = 4;
            this.BtnShow.Text = "&Show";
            this.BtnShow.Click += new System.EventHandler(this.BtnShow_Click);
            // 
            // DTPToDate
            // 
            this.DTPToDate.Font = new System.Drawing.Font("Verdana", 9F);
            this.DTPToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTPToDate.Location = new System.Drawing.Point(300, 9);
            this.DTPToDate.Name = "DTPToDate";
            this.DTPToDate.Size = new System.Drawing.Size(105, 22);
            this.DTPToDate.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(233, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "To Date";
            this.label2.ToolTips = "";
            // 
            // DTPFromDate
            // 
            this.DTPFromDate.Font = new System.Drawing.Font("Verdana", 9F);
            this.DTPFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTPFromDate.Location = new System.Drawing.Point(110, 9);
            this.DTPFromDate.Name = "DTPFromDate";
            this.DTPFromDate.Size = new System.Drawing.Size(105, 22);
            this.DTPFromDate.TabIndex = 1;
            this.DTPFromDate.ValueChanged += new System.EventHandler(this.DTPFromDate_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(28, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "From Date";
            this.label1.ToolTips = "";
            // 
            // cLabel28
            // 
            this.cLabel28.BackColor = System.Drawing.Color.DarkSlateGray;
            this.cLabel28.Dock = System.Windows.Forms.DockStyle.Top;
            this.cLabel28.Font = new System.Drawing.Font("Verdana", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.cLabel28.ForeColor = System.Drawing.Color.White;
            this.cLabel28.Location = new System.Drawing.Point(0, 42);
            this.cLabel28.Name = "cLabel28";
            this.cLabel28.Size = new System.Drawing.Size(814, 22);
            this.cLabel28.TabIndex = 180;
            this.cLabel28.Text = "Search History";
            this.cLabel28.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cLabel28.ToolTips = "";
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.MainGrdDetail;
            this.gridView1.Name = "gridView1";
            // 
            // MainGrdDetail
            // 
            this.MainGrdDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGrdDetail.Location = new System.Drawing.Point(0, 64);
            this.MainGrdDetail.MainView = this.GrdDetail;
            this.MainGrdDetail.Name = "MainGrdDetail";
            this.MainGrdDetail.Size = new System.Drawing.Size(814, 289);
            this.MainGrdDetail.TabIndex = 181;
            this.MainGrdDetail.TabStop = false;
            this.MainGrdDetail.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GrdDetail,
            this.gridView3,
            this.gridView1});
            // 
            // GrdDetail
            // 
            this.GrdDetail.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(234)))), ((int)(((byte)(141)))));
            this.GrdDetail.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(234)))), ((int)(((byte)(141)))));
            this.GrdDetail.Appearance.FocusedCell.Options.UseBackColor = true;
            this.GrdDetail.Appearance.FooterPanel.Font = new System.Drawing.Font("Verdana", 7.5F, System.Drawing.FontStyle.Bold);
            this.GrdDetail.Appearance.FooterPanel.Options.UseFont = true;
            this.GrdDetail.Appearance.GroupFooter.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.GrdDetail.Appearance.GroupFooter.Options.UseFont = true;
            this.GrdDetail.Appearance.GroupRow.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.GrdDetail.Appearance.GroupRow.ForeColor = System.Drawing.Color.Black;
            this.GrdDetail.Appearance.GroupRow.Options.UseFont = true;
            this.GrdDetail.Appearance.GroupRow.Options.UseForeColor = true;
            this.GrdDetail.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.GrdDetail.Appearance.HeaderPanel.Options.UseFont = true;
            this.GrdDetail.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.GrdDetail.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDetail.Appearance.HorzLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDetail.Appearance.HorzLine.Options.UseBackColor = true;
            this.GrdDetail.Appearance.Row.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.GrdDetail.Appearance.Row.Options.UseFont = true;
            this.GrdDetail.Appearance.VertLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDetail.Appearance.VertLine.Options.UseBackColor = true;
            this.GrdDetail.AppearancePrint.FooterPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.GrdDetail.AppearancePrint.FooterPanel.Options.UseFont = true;
            this.GrdDetail.AppearancePrint.HeaderPanel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.GrdDetail.AppearancePrint.HeaderPanel.Options.UseFont = true;
            this.GrdDetail.ColumnPanelRowHeight = 25;
            this.GrdDetail.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.bandedGridColumn60,
            this.gridColumn5,
            this.gridColumn10,
            this.bandedGridColumn61,
            this.bandedGridColumn7,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn11,
            this.gridColumn4,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn12,
            this.gridColumn8,
            this.gridColumn9});
            this.GrdDetail.FooterPanelHeight = 20;
            this.GrdDetail.GridControl = this.MainGrdDetail;
            this.GrdDetail.Name = "GrdDetail";
            this.GrdDetail.OptionsBehavior.Editable = false;
            this.GrdDetail.OptionsFilter.AllowFilterEditor = false;
            this.GrdDetail.OptionsNavigation.EnterMoveNextColumn = true;
            this.GrdDetail.OptionsPrint.ExpandAllGroups = false;
            this.GrdDetail.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.GrdDetail.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.GrdDetail.OptionsSelection.MultiSelect = true;
            this.GrdDetail.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.GrdDetail.OptionsView.ColumnAutoWidth = false;
            this.GrdDetail.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.GrdDetail.OptionsView.ShowAutoFilterRow = true;
            this.GrdDetail.OptionsView.ShowFooter = true;
            this.GrdDetail.OptionsView.ShowGroupPanel = false;
            this.GrdDetail.RowHeight = 23;
            // 
            // bandedGridColumn60
            // 
            this.bandedGridColumn60.AppearanceCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.bandedGridColumn60.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.bandedGridColumn60.AppearanceCell.Options.UseBackColor = true;
            this.bandedGridColumn60.AppearanceCell.Options.UseFont = true;
            this.bandedGridColumn60.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn60.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn60.Caption = "CRITERIA_ID";
            this.bandedGridColumn60.FieldName = "CRITERIA_ID";
            this.bandedGridColumn60.Name = "bandedGridColumn60";
            this.bandedGridColumn60.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "LEDGER_ID";
            this.gridColumn5.FieldName = "LEDGER_ID";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Width = 144;
            // 
            // gridColumn10
            // 
            this.gridColumn10.Caption = "Ledger Name";
            this.gridColumn10.FieldName = "LEDGERNAME";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 0;
            this.gridColumn10.Width = 144;
            // 
            // bandedGridColumn61
            // 
            this.bandedGridColumn61.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.bandedGridColumn61.AppearanceCell.Options.UseFont = true;
            this.bandedGridColumn61.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn61.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn61.Caption = "Criteria Type";
            this.bandedGridColumn61.FieldName = "CRITERIATYPE";
            this.bandedGridColumn61.Name = "bandedGridColumn61";
            this.bandedGridColumn61.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.bandedGridColumn61.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.bandedGridColumn61.Visible = true;
            this.bandedGridColumn61.VisibleIndex = 1;
            this.bandedGridColumn61.Width = 136;
            // 
            // bandedGridColumn7
            // 
            this.bandedGridColumn7.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.bandedGridColumn7.AppearanceCell.Options.UseFont = true;
            this.bandedGridColumn7.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn7.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn7.Caption = "Display Text";
            this.bandedGridColumn7.FieldName = "DISPLAYTEXT";
            this.bandedGridColumn7.Name = "bandedGridColumn7";
            this.bandedGridColumn7.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.bandedGridColumn7.Visible = true;
            this.bandedGridColumn7.VisibleIndex = 3;
            this.bandedGridColumn7.Width = 153;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Criteria";
            this.gridColumn1.FieldName = "CRITERIA";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 2;
            this.gridColumn1.Width = 147;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "Source";
            this.gridColumn2.FieldName = "SOURCE";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 4;
            this.gridColumn2.Width = 116;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "Entry Date";
            this.gridColumn3.DisplayFormat.FormatString = "M/d/yyyy HH:mm:ss";
            this.gridColumn3.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn3.FieldName = "ENTRYDATE";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 5;
            this.gridColumn3.Width = 126;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "Entry By Id";
            this.gridColumn11.FieldName = "ENTRYBYID";
            this.gridColumn11.Name = "gridColumn11";
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "Entry By";
            this.gridColumn4.FieldName = "ENTRYBY";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 6;
            this.gridColumn4.Width = 95;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "Entry Ip";
            this.gridColumn6.FieldName = "ENTRYIP";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 7;
            this.gridColumn6.Width = 127;
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn7.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn7.Caption = "Update Date";
            this.gridColumn7.DisplayFormat.FormatString = "M/d/yyyy HH:mm:ss";
            this.gridColumn7.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn7.FieldName = "UPDATEDATE";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 8;
            this.gridColumn7.Width = 114;
            // 
            // gridColumn12
            // 
            this.gridColumn12.Caption = "Update By Id";
            this.gridColumn12.FieldName = "UPDATEBYID";
            this.gridColumn12.Name = "gridColumn12";
            // 
            // gridColumn8
            // 
            this.gridColumn8.Caption = "Update By";
            this.gridColumn8.FieldName = "UPDATEBY";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 9;
            this.gridColumn8.Width = 141;
            // 
            // gridColumn9
            // 
            this.gridColumn9.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn9.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn9.Caption = "Update Ip";
            this.gridColumn9.FieldName = "UPDATEIP";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 10;
            this.gridColumn9.Width = 119;
            // 
            // gridView3
            // 
            this.gridView3.GridControl = this.MainGrdDetail;
            this.gridView3.Name = "gridView3";
            // 
            // gridView2
            // 
            this.gridView2.Name = "gridView2";
            // 
            // FrmSearchHistory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(814, 353);
            this.Controls.Add(this.MainGrdDetail);
            this.Controls.Add(this.cLabel28);
            this.Controls.Add(this.panel1);
            this.LookAndFeel.SkinName = "London Liquid Sky";
            this.LookAndFeel.UseDefaultLookAndFeel =true;
            this.Name = "FrmSearchHistory";
            this.Text = "SEARCH HISTORY";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrdDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxonContLib.cPanel panel1;
        private System.Windows.Forms.DateTimePicker DTPFromDate;
        private AxonContLib.cLabel label1;
        private System.Windows.Forms.DateTimePicker DTPToDate;
        private AxonContLib.cLabel label2;
        private DevControlLib.cDevSimpleButton BtnExit;
        private DevControlLib.cDevSimpleButton BtnShow;
        private DevControlLib.cDevSimpleButton BtnExport;
        private AxonContLib.cLabel cLabel28;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.GridControl MainGrdDetail;
        private DevExpress.XtraGrid.Views.Grid.GridView GrdDetail;
        private DevExpress.XtraGrid.Columns.GridColumn bandedGridColumn60;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn bandedGridColumn61;
        private DevExpress.XtraGrid.Columns.GridColumn bandedGridColumn7;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
    }
}