
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace MahantExport.Report
{
    partial class FrmRepeatCustomerReport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRepeatCustomerReport));
            DevExpress.XtraGrid.GridFormatRule gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
            DevExpress.XtraEditors.FormatConditionRuleValue formatConditionRuleValue1 = new DevExpress.XtraEditors.FormatConditionRuleValue();
            this.panel2 = new AxonContLib.cPanel(this.components);
            this.PanelProgress = new DevExpress.XtraWaitForm.ProgressPanel();
            this.DTPFromDate = new AxonContLib.cDateTimePicker(this.components);
            this.DTPToDate = new AxonContLib.cDateTimePicker(this.components);
            this.cLabel6 = new AxonContLib.cLabel(this.components);
            this.cLabel5 = new AxonContLib.cLabel(this.components);
            this.BtnExit = new DevExpress.XtraEditors.SimpleButton();
            this.BtnExport = new DevExpress.XtraEditors.SimpleButton();
            this.BtnSearch = new DevExpress.XtraEditors.SimpleButton();
            this.MainGrid = new DevExpress.XtraGrid.GridControl();
            this.GrdDet = new DevExpress.XtraGrid.Views.BandedGrid.BandedGridView();
            this.BandGeneral = new DevExpress.XtraGrid.Views.BandedGrid.GridBand();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.PanelProgress);
            this.panel2.Controls.Add(this.DTPFromDate);
            this.panel2.Controls.Add(this.DTPToDate);
            this.panel2.Controls.Add(this.cLabel6);
            this.panel2.Controls.Add(this.cLabel5);
            this.panel2.Controls.Add(this.BtnExit);
            this.panel2.Controls.Add(this.BtnExport);
            this.panel2.Controls.Add(this.BtnSearch);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1061, 46);
            this.panel2.TabIndex = 5;
            this.panel2.Text = "Panel2";
            // 
            // PanelProgress
            // 
            this.PanelProgress.AnimationToTextDistance = 10;
            this.PanelProgress.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.PanelProgress.Appearance.Options.UseBackColor = true;
            this.PanelProgress.AppearanceCaption.Font = new System.Drawing.Font("Verdana", 12F);
            this.PanelProgress.AppearanceCaption.Options.UseFont = true;
            this.PanelProgress.AppearanceDescription.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.PanelProgress.AppearanceDescription.Options.UseFont = true;
            this.PanelProgress.Location = new System.Drawing.Point(670, 6);
            this.PanelProgress.LookAndFeel.UseDefaultLookAndFeel = false;
            this.PanelProgress.Name = "PanelProgress";
            this.PanelProgress.ShowCaption = false;
            this.PanelProgress.ShowDescription = false;
            this.PanelProgress.Size = new System.Drawing.Size(37, 34);
            this.PanelProgress.TabIndex = 211;
            this.PanelProgress.Text = "progressPanel1";
            this.PanelProgress.Visible = false;
            // 
            // DTPFromDate
            // 
            this.DTPFromDate.AllowTabKeyOnEnter = false;
            this.DTPFromDate.Checked = false;
            this.DTPFromDate.CustomFormat = "dd/MM/yyyy";
            this.DTPFromDate.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DTPFromDate.ForeColor = System.Drawing.Color.Black;
            this.DTPFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTPFromDate.Location = new System.Drawing.Point(91, 11);
            this.DTPFromDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DTPFromDate.Name = "DTPFromDate";
            this.DTPFromDate.ShowCheckBox = true;
            this.DTPFromDate.Size = new System.Drawing.Size(131, 22);
            this.DTPFromDate.TabIndex = 209;
            this.DTPFromDate.ToolTips = "";
            // 
            // DTPToDate
            // 
            this.DTPToDate.AllowTabKeyOnEnter = false;
            this.DTPToDate.Checked = false;
            this.DTPToDate.CustomFormat = "dd/MM/yyyy";
            this.DTPToDate.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DTPToDate.ForeColor = System.Drawing.Color.Black;
            this.DTPToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTPToDate.Location = new System.Drawing.Point(256, 11);
            this.DTPToDate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.DTPToDate.Name = "DTPToDate";
            this.DTPToDate.ShowCheckBox = true;
            this.DTPToDate.Size = new System.Drawing.Size(131, 22);
            this.DTPToDate.TabIndex = 210;
            this.DTPToDate.ToolTips = "";
            // 
            // cLabel6
            // 
            this.cLabel6.AutoSize = true;
            this.cLabel6.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold);
            this.cLabel6.ForeColor = System.Drawing.Color.Black;
            this.cLabel6.Location = new System.Drawing.Point(229, 15);
            this.cLabel6.Name = "cLabel6";
            this.cLabel6.Size = new System.Drawing.Size(25, 16);
            this.cLabel6.TabIndex = 207;
            this.cLabel6.Text = "To";
            this.cLabel6.ToolTips = "";
            // 
            // cLabel5
            // 
            this.cLabel5.AutoSize = true;
            this.cLabel5.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold);
            this.cLabel5.ForeColor = System.Drawing.Color.Black;
            this.cLabel5.Location = new System.Drawing.Point(6, 15);
            this.cLabel5.Name = "cLabel5";
            this.cLabel5.Size = new System.Drawing.Size(84, 16);
            this.cLabel5.TabIndex = 208;
            this.cLabel5.Text = "From Date";
            this.cLabel5.ToolTips = "";
            // 
            // BtnExit
            // 
            this.BtnExit.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnExit.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnExit.Appearance.Options.UseFont = true;
            this.BtnExit.Appearance.Options.UseForeColor = true;
            this.BtnExit.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnExit.ImageOptions.SvgImage")));
            this.BtnExit.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnExit.Location = new System.Drawing.Point(573, 6);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(82, 31);
            this.BtnExit.TabIndex = 8;
            this.BtnExit.TabStop = false;
            this.BtnExit.Text = "E&xit";
            this.BtnExit.Click += new System.EventHandler(this.BtnRapaportExit_Click);
            // 
            // BtnExport
            // 
            this.BtnExport.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnExport.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnExport.Appearance.Options.UseFont = true;
            this.BtnExport.Appearance.Options.UseForeColor = true;
            this.BtnExport.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnExport.ImageOptions.SvgImage")));
            this.BtnExport.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnExport.Location = new System.Drawing.Point(481, 6);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(87, 31);
            this.BtnExport.TabIndex = 7;
            this.BtnExport.TabStop = false;
            this.BtnExport.Text = "Export";
            this.BtnExport.Click += new System.EventHandler(this.BtnExportRapaport_Click);
            // 
            // BtnSearch
            // 
            this.BtnSearch.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnSearch.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnSearch.Appearance.Options.UseFont = true;
            this.BtnSearch.Appearance.Options.UseForeColor = true;
            this.BtnSearch.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSearch.ImageOptions.SvgImage")));
            this.BtnSearch.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnSearch.Location = new System.Drawing.Point(402, 6);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(74, 31);
            this.BtnSearch.TabIndex = 14;
            this.BtnSearch.Text = "Show";
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // MainGrid
            // 
            this.MainGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGrid.Location = new System.Drawing.Point(0, 46);
            this.MainGrid.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MainGrid.MainView = this.GrdDet;
            this.MainGrid.Name = "MainGrid";
            this.MainGrid.Size = new System.Drawing.Size(1061, 512);
            this.MainGrid.TabIndex = 169;
            this.MainGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GrdDet});
            // 
            // GrdDet
            // 
            this.GrdDet.Appearance.BandPanel.Font = new System.Drawing.Font("Verdana", 7.5F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.BandPanel.Options.UseFont = true;
            this.GrdDet.Appearance.BandPanel.Options.UseTextOptions = true;
            this.GrdDet.Appearance.BandPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDet.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(234)))), ((int)(((byte)(141)))));
            this.GrdDet.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(234)))), ((int)(((byte)(141)))));
            this.GrdDet.Appearance.FocusedCell.Options.UseBackColor = true;
            this.GrdDet.Appearance.FocusedRow.Font = new System.Drawing.Font("Verdana", 8F);
            this.GrdDet.Appearance.FocusedRow.Options.UseFont = true;
            this.GrdDet.Appearance.FooterPanel.Font = new System.Drawing.Font("Verdana", 6F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.FooterPanel.Options.UseFont = true;
            this.GrdDet.Appearance.GroupFooter.Font = new System.Drawing.Font("Verdana", 6F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.GroupFooter.Options.UseFont = true;
            this.GrdDet.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 7.5F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.HeaderPanel.Options.UseFont = true;
            this.GrdDet.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.GrdDet.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDet.Appearance.HorzLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDet.Appearance.HorzLine.Options.UseBackColor = true;
            this.GrdDet.Appearance.Row.Font = new System.Drawing.Font("Verdana", 8F);
            this.GrdDet.Appearance.Row.Options.UseFont = true;
            this.GrdDet.Appearance.VertLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDet.Appearance.VertLine.Options.UseBackColor = true;
            this.GrdDet.AppearancePrint.BandPanel.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.GrdDet.AppearancePrint.BandPanel.Options.UseFont = true;
            this.GrdDet.AppearancePrint.EvenRow.BackColor = System.Drawing.Color.Transparent;
            this.GrdDet.AppearancePrint.EvenRow.Font = new System.Drawing.Font("Verdana", 7F);
            this.GrdDet.AppearancePrint.EvenRow.Options.UseBackColor = true;
            this.GrdDet.AppearancePrint.EvenRow.Options.UseFont = true;
            this.GrdDet.AppearancePrint.FooterPanel.Font = new System.Drawing.Font("Verdana", 6F, System.Drawing.FontStyle.Bold);
            this.GrdDet.AppearancePrint.FooterPanel.Options.UseFont = true;
            this.GrdDet.AppearancePrint.GroupFooter.Font = new System.Drawing.Font("Verdana", 6F, System.Drawing.FontStyle.Bold);
            this.GrdDet.AppearancePrint.GroupFooter.Options.UseFont = true;
            this.GrdDet.AppearancePrint.HeaderPanel.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.GrdDet.AppearancePrint.HeaderPanel.Options.UseFont = true;
            this.GrdDet.AppearancePrint.Lines.BackColor = System.Drawing.Color.Gray;
            this.GrdDet.AppearancePrint.Lines.Options.UseBackColor = true;
            this.GrdDet.AppearancePrint.OddRow.BackColor = System.Drawing.Color.Transparent;
            this.GrdDet.AppearancePrint.OddRow.Font = new System.Drawing.Font("Verdana", 7F);
            this.GrdDet.AppearancePrint.OddRow.Options.UseBackColor = true;
            this.GrdDet.AppearancePrint.OddRow.Options.UseFont = true;
            this.GrdDet.AppearancePrint.Row.Font = new System.Drawing.Font("Verdana", 7F);
            this.GrdDet.AppearancePrint.Row.Options.UseFont = true;
            this.GrdDet.BandPanelRowHeight = 25;
            this.GrdDet.Bands.AddRange(new DevExpress.XtraGrid.Views.BandedGrid.GridBand[] {
            this.BandGeneral});
            this.GrdDet.ColumnPanelRowHeight = 25;
            gridFormatRule1.ApplyToRow = true;
            gridFormatRule1.Name = "Format0";
            formatConditionRuleValue1.Condition = DevExpress.XtraEditors.FormatCondition.LessOrEqual;
            gridFormatRule1.Rule = formatConditionRuleValue1;
            this.GrdDet.FormatRules.Add(gridFormatRule1);
            this.GrdDet.GridControl = this.MainGrid;
            this.GrdDet.Name = "GrdDet";
            this.GrdDet.OptionsBehavior.Editable = false;
            this.GrdDet.OptionsFilter.AllowFilterEditor = false;
            this.GrdDet.OptionsPrint.EnableAppearanceEvenRow = true;
            this.GrdDet.OptionsPrint.EnableAppearanceOddRow = true;
            this.GrdDet.OptionsPrint.ExpandAllGroups = false;
            this.GrdDet.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.GrdDet.OptionsSelection.MultiSelect = true;
            this.GrdDet.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            this.GrdDet.OptionsView.ColumnAutoWidth = false;
            this.GrdDet.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.GrdDet.OptionsView.ShowAutoFilterRow = true;
            this.GrdDet.OptionsView.ShowFooter = true;
            this.GrdDet.OptionsView.ShowGroupPanel = false;
            this.GrdDet.RowHeight = 23;
            this.GrdDet.CustomDrawBandHeader += new DevExpress.XtraGrid.Views.BandedGrid.BandHeaderCustomDrawEventHandler(this.GrdDet_CustomDrawBandHeader);
            this.GrdDet.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.GrdDet_RowCellClick);
            this.GrdDet.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.GrdDet_CustomDrawCell);
            this.GrdDet.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.GrdDet_RowStyle);
            this.GrdDet.ShowingEditor += new System.ComponentModel.CancelEventHandler(this.GrdDet_ShowingEditor);
            this.GrdDet.CustomColumnSort += new DevExpress.XtraGrid.Views.Base.CustomColumnSortEventHandler(this.GrdDet_CustomColumnSort);
            this.GrdDet.CustomColumnDisplayText += new DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventHandler(this.GrdDet_CustomColumnDisplayText);
            // 
            // BandGeneral
            // 
            this.BandGeneral.Caption = "General";
            this.BandGeneral.Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            this.BandGeneral.Name = "BandGeneral";
            this.BandGeneral.VisibleIndex = 0;
            this.BandGeneral.Width = 71;
            // 
            // FrmRepeatCustomerReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1061, 558);
            this.Controls.Add(this.MainGrid);
            this.Controls.Add(this.panel2);
            this.Name = "FrmRepeatCustomerReport";
            this.Text = "REPEAT CUSTOMER REPORT";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private AxonContLib.cPanel panel2;
        private AxonContLib.cDateTimePicker DTPFromDate;
        private AxonContLib.cDateTimePicker DTPToDate;
        private AxonContLib.cLabel cLabel6;
        private AxonContLib.cLabel cLabel5;
        private DevExpress.XtraEditors.SimpleButton BtnExit;
        private DevExpress.XtraEditors.SimpleButton BtnExport;
        private DevExpress.XtraEditors.SimpleButton BtnSearch;
        private DevExpress.XtraWaitForm.ProgressPanel PanelProgress;
        private GridControl MainGrid;
        private DevExpress.XtraGrid.Views.BandedGrid.BandedGridView GrdDet;
        private DevExpress.XtraGrid.Views.BandedGrid.GridBand BandGeneral;
    }
}