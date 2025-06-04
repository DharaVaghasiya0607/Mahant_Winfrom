
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace MahantExport
{
    partial class FrmstonePriceHistoryComparision
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmstonePriceHistoryComparision));
            this.panel2 = new AxonContLib.cPanel(this.components);
            this.txtStoneNo = new AxonContLib.cTextBox(this.components);
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            this.GrpViewType = new AxonContLib.cGroupBox(this.components);
            this.RbtWeekly = new AxonContLib.cRadioButton(this.components);
            this.RbtDaily = new AxonContLib.cRadioButton(this.components);
            this.RbtMonthly = new AxonContLib.cRadioButton(this.components);
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
            this.GrpViewType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.txtStoneNo);
            this.panel2.Controls.Add(this.cLabel1);
            this.panel2.Controls.Add(this.GrpViewType);
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
            this.panel2.Size = new System.Drawing.Size(1053, 94);
            this.panel2.TabIndex = 5;
            this.panel2.Text = "Panel2";
            // 
            // txtStoneNo
            // 
            this.txtStoneNo.ActivationColor = true;
            this.txtStoneNo.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtStoneNo.AllowTabKeyOnEnter = false;
            this.txtStoneNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStoneNo.Font = new System.Drawing.Font("Verdana", 8F);
            this.txtStoneNo.Format = "";
            this.txtStoneNo.IsComplusory = false;
            this.txtStoneNo.Location = new System.Drawing.Point(84, 8);
            this.txtStoneNo.Multiline = true;
            this.txtStoneNo.Name = "txtStoneNo";
            this.txtStoneNo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStoneNo.SelectAllTextOnFocus = true;
            this.txtStoneNo.Size = new System.Drawing.Size(324, 33);
            this.txtStoneNo.TabIndex = 214;
            this.txtStoneNo.ToolTips = "";
            this.txtStoneNo.WaterMarkText = null;
            this.txtStoneNo.TextChanged += new System.EventHandler(this.txtStoneNo_TextChanged);
            this.txtStoneNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtStoneNo_KeyDown);
            // 
            // cLabel1
            // 
            this.cLabel1.AutoSize = true;
            this.cLabel1.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold);
            this.cLabel1.ForeColor = System.Drawing.Color.Black;
            this.cLabel1.Location = new System.Drawing.Point(12, 13);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(73, 16);
            this.cLabel1.TabIndex = 213;
            this.cLabel1.Text = "Stone No";
            this.cLabel1.ToolTips = "";
            // 
            // GrpViewType
            // 
            this.GrpViewType.Controls.Add(this.RbtWeekly);
            this.GrpViewType.Controls.Add(this.RbtDaily);
            this.GrpViewType.Controls.Add(this.RbtMonthly);
            this.GrpViewType.Location = new System.Drawing.Point(9, 40);
            this.GrpViewType.Name = "GrpViewType";
            this.GrpViewType.Size = new System.Drawing.Size(223, 47);
            this.GrpViewType.TabIndex = 212;
            this.GrpViewType.TabStop = false;
            this.GrpViewType.Text = "View Type";
            // 
            // RbtWeekly
            // 
            this.RbtWeekly.AllowTabKeyOnEnter = false;
            this.RbtWeekly.AutoSize = true;
            this.RbtWeekly.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbtWeekly.ForeColor = System.Drawing.Color.Black;
            this.RbtWeekly.Location = new System.Drawing.Point(66, 20);
            this.RbtWeekly.Name = "RbtWeekly";
            this.RbtWeekly.Size = new System.Drawing.Size(75, 18);
            this.RbtWeekly.TabIndex = 0;
            this.RbtWeekly.Tag = "Weekly";
            this.RbtWeekly.Text = "Weekly";
            this.RbtWeekly.ToolTips = "";
            this.RbtWeekly.UseVisualStyleBackColor = true;
            // 
            // RbtDaily
            // 
            this.RbtDaily.AllowTabKeyOnEnter = false;
            this.RbtDaily.AutoSize = true;
            this.RbtDaily.Checked = true;
            this.RbtDaily.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbtDaily.ForeColor = System.Drawing.Color.Black;
            this.RbtDaily.Location = new System.Drawing.Point(6, 20);
            this.RbtDaily.Name = "RbtDaily";
            this.RbtDaily.Size = new System.Drawing.Size(59, 18);
            this.RbtDaily.TabIndex = 0;
            this.RbtDaily.TabStop = true;
            this.RbtDaily.Tag = "Daily";
            this.RbtDaily.Text = "Daily";
            this.RbtDaily.ToolTips = "";
            this.RbtDaily.UseVisualStyleBackColor = true;
            // 
            // RbtMonthly
            // 
            this.RbtMonthly.AllowTabKeyOnEnter = false;
            this.RbtMonthly.AutoSize = true;
            this.RbtMonthly.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RbtMonthly.ForeColor = System.Drawing.Color.Black;
            this.RbtMonthly.Location = new System.Drawing.Point(142, 20);
            this.RbtMonthly.Name = "RbtMonthly";
            this.RbtMonthly.Size = new System.Drawing.Size(77, 18);
            this.RbtMonthly.TabIndex = 0;
            this.RbtMonthly.Tag = "Monthly";
            this.RbtMonthly.Text = "Monthly";
            this.RbtMonthly.ToolTips = "";
            this.RbtMonthly.UseVisualStyleBackColor = true;
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
            this.PanelProgress.Location = new System.Drawing.Point(497, 49);
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
            this.DTPFromDate.Location = new System.Drawing.Point(497, 13);
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
            this.DTPToDate.Location = new System.Drawing.Point(662, 13);
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
            this.cLabel6.Location = new System.Drawing.Point(635, 17);
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
            this.cLabel5.Location = new System.Drawing.Point(412, 17);
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
            this.BtnExit.Location = new System.Drawing.Point(409, 52);
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
            this.BtnExport.Location = new System.Drawing.Point(317, 52);
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
            this.BtnSearch.Location = new System.Drawing.Point(238, 52);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(74, 31);
            this.BtnSearch.TabIndex = 14;
            this.BtnSearch.Text = "Show";
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // MainGrid
            // 
            this.MainGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGrid.Location = new System.Drawing.Point(0, 94);
            this.MainGrid.LookAndFeel.UseDefaultLookAndFeel = false;
            this.MainGrid.MainView = this.GrdDet;
            this.MainGrid.Name = "MainGrid";
            this.MainGrid.Size = new System.Drawing.Size(1053, 465);
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
            // FrmstonePriceHistoryComparision
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1053, 559);
            this.Controls.Add(this.MainGrid);
            this.Controls.Add(this.panel2);
            this.LookAndFeel.SkinName = "London Liquid Sky";
            this.LookAndFeel.UseDefaultLookAndFeel =true;
            this.Name = "FrmstonePriceHistoryComparision";
            this.Text = "PRICE COMPARISION HISTORY";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.GrpViewType.ResumeLayout(false);
            this.GrpViewType.PerformLayout();
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
        private AxonContLib.cGroupBox GrpViewType;
        private AxonContLib.cRadioButton RbtWeekly;
        private AxonContLib.cRadioButton RbtDaily;
        private AxonContLib.cRadioButton RbtMonthly;
        private AxonContLib.cLabel cLabel1;
        private AxonContLib.cTextBox txtStoneNo;
    }
}