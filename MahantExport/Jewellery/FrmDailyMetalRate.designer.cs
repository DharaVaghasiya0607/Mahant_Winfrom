namespace MahantExport.Masters
{
    partial class FrmDailyMetalRate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDailyMetalRate));
            this.BtnExport = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnSave = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnAdd = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnExit = new DevControlLib.cDevSimpleButton(this.components);
            this.PanelTop = new AxonContLib.cPanel(this.components);
            this.BtnDelete = new DevControlLib.cDevSimpleButton(this.components);
            this.lblLatestRate = new AxonContLib.cLabel(this.components);
            this.txtRemark = new AxonContLib.cTextBox(this.components);
            this.txtPerGramRate = new AxonContLib.cTextBox(this.components);
            this.cLabel3 = new AxonContLib.cLabel(this.components);
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            this.txtMetal = new AxonContLib.cTextBox(this.components);
            this.cLabel6 = new AxonContLib.cLabel(this.components);
            this.lblAccID = new AxonContLib.cLabel(this.components);
            this.DTPRateDate = new AxonContLib.cDateTimePicker(this.components);
            this.cLabel2 = new AxonContLib.cLabel(this.components);
            this.panel2 = new AxonContLib.cPanel(this.components);
            this.progressPanel1 = new DevExpress.XtraWaitForm.ProgressPanel();
            this.MainGrid = new DevExpress.XtraGrid.GridControl();
            this.GrdDet = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repTxtRemark = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repChkActive = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.repTxtCategoryCode = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repTxtCategoryName = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.RepTxtClientCode = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.PanelTop.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtRemark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repChkActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtCategoryCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtCategoryName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepTxtClientCode)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnExport
            // 
            this.BtnExport.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.BtnExport.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnExport.Appearance.Options.UseFont = true;
            this.BtnExport.Appearance.Options.UseForeColor = true;
            this.BtnExport.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnExport.ImageOptions.SvgImage")));
            this.BtnExport.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnExport.Location = new System.Drawing.Point(933, 12);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(91, 36);
            this.BtnExport.TabIndex = 6;
            this.BtnExport.TabStop = false;
            this.BtnExport.Text = "Export";
            this.BtnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.BtnSave.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnSave.Appearance.Options.UseFont = true;
            this.BtnSave.Appearance.Options.UseForeColor = true;
            this.BtnSave.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSave.ImageOptions.SvgImage")));
            this.BtnSave.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnSave.Location = new System.Drawing.Point(648, 12);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(91, 36);
            this.BtnSave.TabIndex = 3;
            this.BtnSave.Text = "&Save";
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnAdd
            // 
            this.BtnAdd.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.BtnAdd.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnAdd.Appearance.Options.UseFont = true;
            this.BtnAdd.Appearance.Options.UseForeColor = true;
            this.BtnAdd.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnAdd.ImageOptions.SvgImage")));
            this.BtnAdd.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnAdd.Location = new System.Drawing.Point(838, 12);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(91, 36);
            this.BtnAdd.TabIndex = 4;
            this.BtnAdd.TabStop = false;
            this.BtnAdd.Text = "&Clear";
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.BtnExit.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnExit.Appearance.Options.UseFont = true;
            this.BtnExit.Appearance.Options.UseForeColor = true;
            this.BtnExit.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnExit.ImageOptions.SvgImage")));
            this.BtnExit.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnExit.Location = new System.Drawing.Point(1028, 12);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(91, 36);
            this.BtnExit.TabIndex = 7;
            this.BtnExit.TabStop = false;
            this.BtnExit.Text = "E&xit";
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // PanelTop
            // 
            this.PanelTop.BackColor = System.Drawing.Color.Gainsboro;
            this.PanelTop.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PanelTop.Controls.Add(this.BtnDelete);
            this.PanelTop.Controls.Add(this.lblLatestRate);
            this.PanelTop.Controls.Add(this.BtnExit);
            this.PanelTop.Controls.Add(this.BtnSave);
            this.PanelTop.Controls.Add(this.BtnExport);
            this.PanelTop.Controls.Add(this.txtRemark);
            this.PanelTop.Controls.Add(this.txtPerGramRate);
            this.PanelTop.Controls.Add(this.BtnAdd);
            this.PanelTop.Controls.Add(this.cLabel3);
            this.PanelTop.Controls.Add(this.cLabel1);
            this.PanelTop.Controls.Add(this.txtMetal);
            this.PanelTop.Controls.Add(this.cLabel6);
            this.PanelTop.Controls.Add(this.lblAccID);
            this.PanelTop.Controls.Add(this.DTPRateDate);
            this.PanelTop.Controls.Add(this.cLabel2);
            this.PanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelTop.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.PanelTop.Location = new System.Drawing.Point(0, 0);
            this.PanelTop.Name = "PanelTop";
            this.PanelTop.Size = new System.Drawing.Size(1127, 64);
            this.PanelTop.TabIndex = 189;
            this.PanelTop.TabStop = true;
            // 
            // BtnDelete
            // 
            this.BtnDelete.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.BtnDelete.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnDelete.Appearance.Options.UseFont = true;
            this.BtnDelete.Appearance.Options.UseForeColor = true;
            this.BtnDelete.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnDelete.ImageOptions.SvgImage")));
            this.BtnDelete.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnDelete.Location = new System.Drawing.Point(743, 12);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(91, 36);
            this.BtnDelete.TabIndex = 8;
            this.BtnDelete.Text = "&Delete";
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // lblLatestRate
            // 
            this.lblLatestRate.AutoSize = true;
            this.lblLatestRate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblLatestRate.Font = new System.Drawing.Font("Verdana", 8F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.lblLatestRate.ForeColor = System.Drawing.Color.Blue;
            this.lblLatestRate.Location = new System.Drawing.Point(565, 12);
            this.lblLatestRate.Name = "lblLatestRate";
            this.lblLatestRate.Size = new System.Drawing.Size(73, 13);
            this.lblLatestRate.TabIndex = 192;
            this.lblLatestRate.Text = "Get Latest";
            this.lblLatestRate.ToolTips = "";
            this.lblLatestRate.Click += new System.EventHandler(this.lblLatestRate_Click);
            // 
            // txtRemark
            // 
            this.txtRemark.ActivationColor = true;
            this.txtRemark.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtRemark.AllowTabKeyOnEnter = false;
            this.txtRemark.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRemark.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemark.Format = "############0.00";
            this.txtRemark.IsComplusory = false;
            this.txtRemark.Location = new System.Drawing.Point(408, 28);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.SelectAllTextOnFocus = true;
            this.txtRemark.Size = new System.Drawing.Size(104, 22);
            this.txtRemark.TabIndex = 13;
            this.txtRemark.Text = "0";
            this.txtRemark.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtRemark.ToolTips = "";
            this.txtRemark.WaterMarkText = null;
            // 
            // txtPerGramRate
            // 
            this.txtPerGramRate.ActivationColor = true;
            this.txtPerGramRate.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtPerGramRate.AllowTabKeyOnEnter = false;
            this.txtPerGramRate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPerGramRate.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPerGramRate.Format = "############0.00";
            this.txtPerGramRate.IsComplusory = false;
            this.txtPerGramRate.Location = new System.Drawing.Point(298, 28);
            this.txtPerGramRate.Name = "txtPerGramRate";
            this.txtPerGramRate.SelectAllTextOnFocus = true;
            this.txtPerGramRate.Size = new System.Drawing.Size(104, 22);
            this.txtPerGramRate.TabIndex = 13;
            this.txtPerGramRate.Text = "0";
            this.txtPerGramRate.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtPerGramRate.ToolTips = "";
            this.txtPerGramRate.WaterMarkText = null;
            // 
            // cLabel3
            // 
            this.cLabel3.AutoSize = true;
            this.cLabel3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel3.ForeColor = System.Drawing.Color.Black;
            this.cLabel3.Location = new System.Drawing.Point(408, 11);
            this.cLabel3.Name = "cLabel3";
            this.cLabel3.Size = new System.Drawing.Size(58, 14);
            this.cLabel3.TabIndex = 12;
            this.cLabel3.Text = "Remark";
            this.cLabel3.ToolTips = "";
            // 
            // cLabel1
            // 
            this.cLabel1.AutoSize = true;
            this.cLabel1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel1.ForeColor = System.Drawing.Color.Black;
            this.cLabel1.Location = new System.Drawing.Point(298, 11);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(104, 14);
            this.cLabel1.TabIndex = 12;
            this.cLabel1.Text = "Per Gram Rate";
            this.cLabel1.ToolTips = "";
            // 
            // txtMetal
            // 
            this.txtMetal.ActivationColor = true;
            this.txtMetal.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtMetal.AllowTabKeyOnEnter = false;
            this.txtMetal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMetal.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMetal.Format = "";
            this.txtMetal.IsComplusory = false;
            this.txtMetal.Location = new System.Drawing.Point(125, 28);
            this.txtMetal.Name = "txtMetal";
            this.txtMetal.SelectAllTextOnFocus = true;
            this.txtMetal.Size = new System.Drawing.Size(167, 22);
            this.txtMetal.TabIndex = 11;
            this.txtMetal.ToolTips = "";
            this.txtMetal.WaterMarkText = null;
            this.txtMetal.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCurrency_KeyPress);
            // 
            // cLabel6
            // 
            this.cLabel6.AutoSize = true;
            this.cLabel6.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel6.ForeColor = System.Drawing.Color.Black;
            this.cLabel6.Location = new System.Drawing.Point(125, 11);
            this.cLabel6.Name = "cLabel6";
            this.cLabel6.Size = new System.Drawing.Size(43, 14);
            this.cLabel6.TabIndex = 10;
            this.cLabel6.Text = "Metal";
            this.cLabel6.ToolTips = "";
            // 
            // lblAccID
            // 
            this.lblAccID.AutoSize = true;
            this.lblAccID.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccID.ForeColor = System.Drawing.Color.Black;
            this.lblAccID.Location = new System.Drawing.Point(6, 11);
            this.lblAccID.Name = "lblAccID";
            this.lblAccID.Size = new System.Drawing.Size(38, 14);
            this.lblAccID.TabIndex = 2;
            this.lblAccID.Text = "Date";
            this.lblAccID.ToolTips = "";
            // 
            // DTPRateDate
            // 
            this.DTPRateDate.AllowTabKeyOnEnter = false;
            this.DTPRateDate.CustomFormat = "dd/MM/yyyy";
            this.DTPRateDate.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DTPRateDate.ForeColor = System.Drawing.Color.Black;
            this.DTPRateDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTPRateDate.Location = new System.Drawing.Point(6, 28);
            this.DTPRateDate.Name = "DTPRateDate";
            this.DTPRateDate.Size = new System.Drawing.Size(113, 22);
            this.DTPRateDate.TabIndex = 1;
            this.DTPRateDate.ToolTips = "";
            // 
            // cLabel2
            // 
            this.cLabel2.AutoSize = true;
            this.cLabel2.Font = new System.Drawing.Font("Verdana", 7.5F, System.Drawing.FontStyle.Bold);
            this.cLabel2.ForeColor = System.Drawing.Color.Maroon;
            this.cLabel2.Location = new System.Drawing.Point(566, 36);
            this.cLabel2.Name = "cLabel2";
            this.cLabel2.Size = new System.Drawing.Size(76, 12);
            this.cLabel2.TabIndex = 191;
            this.cLabel2.Text = "(Against 1$)";
            this.cLabel2.ToolTips = "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.progressPanel1);
            this.panel2.Controls.Add(this.MainGrid);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel2.Location = new System.Drawing.Point(0, 64);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1127, 409);
            this.panel2.TabIndex = 190;
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
            this.progressPanel1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Simple;
            this.progressPanel1.Location = new System.Drawing.Point(439, 95);
            this.progressPanel1.LookAndFeel.SkinName = "Caramel";
            this.progressPanel1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.progressPanel1.Name = "progressPanel1";
            this.progressPanel1.Size = new System.Drawing.Size(204, 65);
            this.progressPanel1.TabIndex = 183;
            this.progressPanel1.Text = "progressPanel1";
            this.progressPanel1.Visible = false;
            // 
            // MainGrid
            // 
            this.MainGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGrid.Location = new System.Drawing.Point(0, 0);
            this.MainGrid.MainView = this.GrdDet;
            this.MainGrid.Name = "MainGrid";
            this.MainGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repChkActive,
            this.repTxtCategoryCode,
            this.repTxtCategoryName,
            this.repTxtRemark,
            this.RepTxtClientCode});
            this.MainGrid.Size = new System.Drawing.Size(1127, 409);
            this.MainGrid.TabIndex = 182;
            this.MainGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GrdDet});
            // 
            // GrdDet
            // 
            this.GrdDet.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.GrdDet.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.GrdDet.Appearance.FocusedCell.Font = new System.Drawing.Font("Verdana", 9F);
            this.GrdDet.Appearance.FocusedCell.Options.UseBackColor = true;
            this.GrdDet.Appearance.FocusedCell.Options.UseFont = true;
            this.GrdDet.Appearance.FocusedRow.Font = new System.Drawing.Font("Verdana", 9F);
            this.GrdDet.Appearance.FocusedRow.Options.UseFont = true;
            this.GrdDet.Appearance.FooterPanel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.FooterPanel.Options.UseFont = true;
            this.GrdDet.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
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
            this.GrdDet.ColumnPanelRowHeight = 25;
            this.GrdDet.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn4,
            this.gridColumn1,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn2,
            this.gridColumn5,
            this.gridColumn3});
            this.GrdDet.GridControl = this.MainGrid;
            this.GrdDet.Name = "GrdDet";
            this.GrdDet.OptionsBehavior.Editable = false;
            this.GrdDet.OptionsCustomization.AllowFilter = false;
            this.GrdDet.OptionsCustomization.AllowSort = false;
            this.GrdDet.OptionsFilter.AllowFilterEditor = false;
            this.GrdDet.OptionsNavigation.EnterMoveNextColumn = true;
            this.GrdDet.OptionsPrint.ExpandAllGroups = false;
            this.GrdDet.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.GrdDet.OptionsView.ColumnAutoWidth = false;
            this.GrdDet.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.GrdDet.OptionsView.ShowFooter = true;
            this.GrdDet.OptionsView.ShowGroupPanel = false;
            this.GrdDet.RowHeight = 25;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn4.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn4.AppearanceCell.Options.UseFont = true;
            this.gridColumn4.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn4.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn4.AppearanceHeader.Options.UseFont = true;
            this.gridColumn4.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "METAL_ID";
            this.gridColumn4.FieldName = "METAL_ID";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn1.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn1.AppearanceCell.Options.UseFont = true;
            this.gridColumn1.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn1.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn1.AppearanceHeader.Options.UseFont = true;
            this.gridColumn1.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "Date";
            this.gridColumn1.FieldName = "RATEDATE";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 134;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn6.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn6.AppearanceCell.Options.UseFont = true;
            this.gridColumn6.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn6.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn6.AppearanceHeader.Options.UseFont = true;
            this.gridColumn6.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "METAL_ID";
            this.gridColumn6.FieldName = "METAL_ID";
            this.gridColumn6.Name = "gridColumn6";
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.gridColumn7.Caption = "Metal";
            this.gridColumn7.FieldName = "METAL";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 1;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn2.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn2.AppearanceCell.Options.UseFont = true;
            this.gridColumn2.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn2.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn2.AppearanceHeader.Options.UseFont = true;
            this.gridColumn2.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "Per Gram Rate";
            this.gridColumn2.FieldName = "PERGRAMRATE";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 300;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn5.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn5.AppearanceCell.Options.UseFont = true;
            this.gridColumn5.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn5.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn5.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn5.AppearanceHeader.Options.UseFont = true;
            this.gridColumn5.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn5.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn5.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn5.Caption = "Remark";
            this.gridColumn5.ColumnEdit = this.repTxtRemark;
            this.gridColumn5.FieldName = "REMARK";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 3;
            this.gridColumn5.Width = 400;
            // 
            // repTxtRemark
            // 
            this.repTxtRemark.AutoHeight = false;
            this.repTxtRemark.Name = "repTxtRemark";
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn3.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn3.AppearanceCell.Options.UseFont = true;
            this.gridColumn3.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn3.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn3.AppearanceHeader.Options.UseFont = true;
            this.gridColumn3.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "Active";
            this.gridColumn3.ColumnEdit = this.repChkActive;
            this.gridColumn3.FieldName = "ISACTIVE";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 4;
            this.gridColumn3.Width = 66;
            // 
            // repChkActive
            // 
            this.repChkActive.AutoHeight = false;
            this.repChkActive.Caption = "Check";
            this.repChkActive.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
            this.repChkActive.ImageOptions.ImageChecked = ((System.Drawing.Image)(resources.GetObject("repChkActive.ImageOptions.ImageChecked")));
            this.repChkActive.ImageOptions.ImageUnchecked = ((System.Drawing.Image)(resources.GetObject("repChkActive.ImageOptions.ImageUnchecked")));
            this.repChkActive.Name = "repChkActive";
            // 
            // repTxtCategoryCode
            // 
            this.repTxtCategoryCode.AutoHeight = false;
            this.repTxtCategoryCode.Name = "repTxtCategoryCode";
            // 
            // repTxtCategoryName
            // 
            this.repTxtCategoryName.AutoHeight = false;
            this.repTxtCategoryName.Name = "repTxtCategoryName";
            // 
            // RepTxtClientCode
            // 
            this.RepTxtClientCode.AutoHeight = false;
            this.RepTxtClientCode.Name = "RepTxtClientCode";
            // 
            // FrmDailyMetalRate
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1127, 473);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.PanelTop);
            this.Name = "FrmDailyMetalRate";
            this.Tag = "DailyMetalRateMaster\r\n";
            this.Text = "DAILY METAL RATE MASTER";
            this.PanelTop.ResumeLayout(false);
            this.PanelTop.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtRemark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repChkActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtCategoryCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtCategoryName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepTxtClientCode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevControlLib.cDevSimpleButton BtnExport;
        private DevControlLib.cDevSimpleButton BtnSave;
        private DevControlLib.cDevSimpleButton BtnAdd;
        private DevControlLib.cDevSimpleButton BtnExit;
        private AxonContLib.cPanel PanelTop;
        private AxonContLib.cPanel panel2;
        private AxonContLib.cDateTimePicker DTPRateDate;
        private AxonContLib.cLabel lblAccID;
        private AxonContLib.cTextBox txtMetal;
        private AxonContLib.cLabel cLabel6;
        private AxonContLib.cTextBox txtPerGramRate;
        private AxonContLib.cLabel cLabel1;
        private DevControlLib.cDevSimpleButton BtnDelete;
        private AxonContLib.cLabel cLabel2;
        private AxonContLib.cLabel lblLatestRate;
        private AxonContLib.cTextBox txtRemark;
        private AxonContLib.cLabel cLabel3;
        private DevExpress.XtraWaitForm.ProgressPanel progressPanel1;
        private DevExpress.XtraGrid.GridControl MainGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView GrdDet;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repTxtCategoryCode;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repTxtCategoryName;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repChkActive;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repTxtRemark;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit RepTxtClientCode;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
    }
}