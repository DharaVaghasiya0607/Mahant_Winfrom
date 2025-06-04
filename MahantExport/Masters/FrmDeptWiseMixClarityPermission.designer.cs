
namespace MahantExport.Masters
{
    partial class FrmDeptWiseMixClarityPermission
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDeptWiseMixClarityPermission));
            this.panel1 = new AxonContLib.cPanel(this.components);
            this.BtnExport = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnSave = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnAdd = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnBack = new DevControlLib.cDevSimpleButton(this.components);
            this.MainGrid = new DevExpress.XtraGrid.GridControl();
            this.GrdDet = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repChkIsActive = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repTxrRemark = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtSize = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repClarity = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repTxtYearName = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.RepChkIsLock = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.RepDtpToDate = new DevExpress.XtraEditors.Repository.RepositoryItemDateEdit();
            this.RepFromDate = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.cmbClarity = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.cPanel1 = new AxonContLib.cPanel(this.components);
            this.BtnShow = new DevControlLib.cDevSimpleButton(this.components);
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            this.txtUser_ID = new AxonContLib.cTextBox(this.components);
            this.CmbDepartment = new System.Windows.Forms.ComboBox();
            this.cLabel8 = new AxonContLib.cLabel(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteSelectedItemsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repChkIsActive)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxrRemark)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repClarity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtYearName)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepChkIsLock)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepDtpToDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepDtpToDate.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepFromDate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClarity)).BeginInit();
            this.cPanel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.BtnExport);
            this.panel1.Controls.Add(this.BtnSave);
            this.panel1.Controls.Add(this.BtnAdd);
            this.panel1.Controls.Add(this.BtnBack);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 400);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(855, 50);
            this.panel1.TabIndex = 32;
            // 
            // BtnExport
            // 
            this.BtnExport.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnExport.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnExport.Appearance.Options.UseFont = true;
            this.BtnExport.Appearance.Options.UseForeColor = true;
            this.BtnExport.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnExport.ImageOptions.SvgImage")));
            this.BtnExport.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnExport.Location = new System.Drawing.Point(225, 8);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(103, 35);
            this.BtnExport.TabIndex = 33;
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
            this.BtnSave.Location = new System.Drawing.Point(12, 8);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(103, 35);
            this.BtnSave.TabIndex = 30;
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
            this.BtnAdd.Location = new System.Drawing.Point(117, 8);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(103, 35);
            this.BtnAdd.TabIndex = 31;
            this.BtnAdd.TabStop = false;
            this.BtnAdd.Text = "&Clear";
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // BtnBack
            // 
            this.BtnBack.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnBack.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnBack.Appearance.Options.UseFont = true;
            this.BtnBack.Appearance.Options.UseForeColor = true;
            this.BtnBack.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnBack.ImageOptions.SvgImage")));
            this.BtnBack.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnBack.Location = new System.Drawing.Point(333, 8);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.Size = new System.Drawing.Size(103, 35);
            this.BtnBack.TabIndex = 32;
            this.BtnBack.TabStop = false;
            this.BtnBack.Text = "E&xit";
            this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // MainGrid
            // 
            this.MainGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGrid.Location = new System.Drawing.Point(0, 50);
            this.MainGrid.MainView = this.GrdDet;
            this.MainGrid.Name = "MainGrid";
            this.MainGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repChkIsActive,
            this.repTxrRemark,
            this.repTxtYearName,
            this.RepChkIsLock,
            this.RepDtpToDate,
            this.RepFromDate,
            this.txtSize,
            this.cmbClarity,
            this.repClarity});
            this.MainGrid.Size = new System.Drawing.Size(855, 350);
            this.MainGrid.TabIndex = 33;
            this.MainGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GrdDet});
            // 
            // GrdDet
            // 
            this.GrdDet.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.GrdDet.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.GrdDet.Appearance.FocusedCell.Options.UseBackColor = true;
            this.GrdDet.Appearance.FooterPanel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
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
            this.GrdDet.AppearancePrint.Row.Font = new System.Drawing.Font("Verdana", 9F);
            this.GrdDet.AppearancePrint.Row.Options.UseFont = true;
            this.GrdDet.ColumnPanelRowHeight = 25;
            this.GrdDet.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn6,
            this.gridColumn8,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn7,
            this.gridColumn5});
            this.GrdDet.GridControl = this.MainGrid;
            this.GrdDet.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.GrdDet.Name = "GrdDet";
            this.GrdDet.OptionsFilter.AllowFilterEditor = false;
            this.GrdDet.OptionsNavigation.EnterMoveNextColumn = true;
            this.GrdDet.OptionsPrint.ExpandAllGroups = false;
            this.GrdDet.OptionsView.ColumnAutoWidth = false;
            this.GrdDet.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.GrdDet.OptionsView.ShowAutoFilterRow = true;
            this.GrdDet.OptionsView.ShowFooter = true;
            this.GrdDet.OptionsView.ShowGroupPanel = false;
            this.GrdDet.RowHeight = 25;
            this.GrdDet.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.GrdDet_RowCellClick);
            this.GrdDet.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GrdDet_KeyDown);
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Font = new System.Drawing.Font("Verdana", 9F);
            this.gridColumn6.AppearanceCell.Options.UseFont = true;
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "Active";
            this.gridColumn6.ColumnEdit = this.repChkIsActive;
            this.gridColumn6.FieldName = "ISACTIVE";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 2;
            this.gridColumn6.Width = 60;
            // 
            // repChkIsActive
            // 
            this.repChkIsActive.AutoHeight = false;
            this.repChkIsActive.Caption = "Check";
            this.repChkIsActive.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
            this.repChkIsActive.ImageOptions.ImageChecked = global::MahantExport.Properties.Resources.Checked;
            this.repChkIsActive.ImageOptions.ImageUnchecked = global::MahantExport.Properties.Resources.Unchecked;
            this.repChkIsActive.Name = "repChkIsActive";
            // 
            // gridColumn8
            // 
            this.gridColumn8.AppearanceCell.Font = new System.Drawing.Font("Verdana", 9F);
            this.gridColumn8.AppearanceCell.Options.UseFont = true;
            this.gridColumn8.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn8.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn8.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn8.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn8.Caption = "Remark";
            this.gridColumn8.ColumnEdit = this.repTxrRemark;
            this.gridColumn8.FieldName = "REMARK";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 3;
            this.gridColumn8.Width = 335;
            // 
            // repTxrRemark
            // 
            this.repTxrRemark.AutoHeight = false;
            this.repTxrRemark.Name = "repTxrRemark";
            this.repTxrRemark.KeyDown += new System.Windows.Forms.KeyEventHandler(this.repTxrRemark_KeyDown);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Font = new System.Drawing.Font("Verdana", 9F);
            this.gridColumn1.AppearanceCell.Options.UseFont = true;
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "MixSize";
            this.gridColumn1.ColumnEdit = this.txtSize;
            this.gridColumn1.FieldName = "MIXSIZENAME";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 113;
            // 
            // txtSize
            // 
            this.txtSize.AutoHeight = false;
            this.txtSize.Name = "txtSize";
            this.txtSize.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSize_KeyPress);
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Font = new System.Drawing.Font("Verdana", 9F);
            this.gridColumn2.AppearanceCell.Options.UseFont = true;
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "MixClarity";
            this.gridColumn2.ColumnEdit = this.repClarity;
            this.gridColumn2.FieldName = "MIXCLARITY_ID";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count, "MIXCLARITY_ID", "{0}")});
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 1;
            this.gridColumn2.Width = 250;
            // 
            // repClarity
            // 
            this.repClarity.AutoHeight = false;
            this.repClarity.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repClarity.Name = "repClarity";
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "Permission_Id";
            this.gridColumn3.FieldName = "PERMISSION_ID";
            this.gridColumn3.Name = "gridColumn3";
            // 
            // gridColumn4
            // 
            this.gridColumn4.Caption = "MixSize_ID";
            this.gridColumn4.FieldName = "MIXSIZE_ID";
            this.gridColumn4.Name = "gridColumn4";
            // 
            // gridColumn7
            // 
            this.gridColumn7.Caption = "Department_ID";
            this.gridColumn7.FieldName = "DEPARTMENT_ID";
            this.gridColumn7.Name = "gridColumn7";
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "User_ID";
            this.gridColumn5.FieldName = "USER_ID";
            this.gridColumn5.Name = "gridColumn5";
            // 
            // repTxtYearName
            // 
            this.repTxtYearName.AutoHeight = false;
            this.repTxtYearName.Name = "repTxtYearName";
            // 
            // RepChkIsLock
            // 
            this.RepChkIsLock.AutoHeight = false;
            this.RepChkIsLock.Caption = "Check";
            this.RepChkIsLock.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
            this.RepChkIsLock.ImageOptions.ImageChecked = global::MahantExport.Properties.Resources.Checked;
            this.RepChkIsLock.ImageOptions.ImageUnchecked = global::MahantExport.Properties.Resources.Unchecked;
            this.RepChkIsLock.Name = "RepChkIsLock";
            // 
            // RepDtpToDate
            // 
            this.RepDtpToDate.AutoHeight = false;
            this.RepDtpToDate.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.RepDtpToDate.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.RepDtpToDate.Name = "RepDtpToDate";
            // 
            // RepFromDate
            // 
            this.RepFromDate.AutoHeight = false;
            this.RepFromDate.Name = "RepFromDate";
            // 
            // cmbClarity
            // 
            this.cmbClarity.AutoHeight = false;
            this.cmbClarity.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbClarity.Name = "cmbClarity";
            // 
            // cPanel1
            // 
            this.cPanel1.BackColor = System.Drawing.Color.Silver;
            this.cPanel1.Controls.Add(this.BtnShow);
            this.cPanel1.Controls.Add(this.cLabel1);
            this.cPanel1.Controls.Add(this.txtUser_ID);
            this.cPanel1.Controls.Add(this.CmbDepartment);
            this.cPanel1.Controls.Add(this.cLabel8);
            this.cPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.cPanel1.Location = new System.Drawing.Point(0, 0);
            this.cPanel1.Name = "cPanel1";
            this.cPanel1.Size = new System.Drawing.Size(855, 50);
            this.cPanel1.TabIndex = 34;
            // 
            // BtnShow
            // 
            this.BtnShow.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnShow.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnShow.Appearance.Options.UseFont = true;
            this.BtnShow.Appearance.Options.UseForeColor = true;
            this.BtnShow.ImageOptions.Image = global::MahantExport.Properties.Resources.btnsearch;
            this.BtnShow.Location = new System.Drawing.Point(497, 7);
            this.BtnShow.Name = "BtnShow";
            this.BtnShow.Size = new System.Drawing.Size(103, 35);
            this.BtnShow.TabIndex = 40;
            this.BtnShow.Text = "Show";
            this.BtnShow.Click += new System.EventHandler(this.BtnShow_Click);
            // 
            // cLabel1
            // 
            this.cLabel1.AutoSize = true;
            this.cLabel1.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel1.ForeColor = System.Drawing.Color.Black;
            this.cLabel1.Location = new System.Drawing.Point(296, 15);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(42, 18);
            this.cLabel1.TabIndex = 40;
            this.cLabel1.Text = "User";
            this.cLabel1.ToolTips = "";
            // 
            // txtUser_ID
            // 
            this.txtUser_ID.ActivationColor = false;
            this.txtUser_ID.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtUser_ID.AllowTabKeyOnEnter = true;
            this.txtUser_ID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUser_ID.Font = new System.Drawing.Font("Verdana", 11.25F);
            this.txtUser_ID.Format = "";
            this.txtUser_ID.IsComplusory = false;
            this.txtUser_ID.Location = new System.Drawing.Point(344, 11);
            this.txtUser_ID.Name = "txtUser_ID";
            this.txtUser_ID.SelectAllTextOnFocus = true;
            this.txtUser_ID.Size = new System.Drawing.Size(137, 26);
            this.txtUser_ID.TabIndex = 39;
            this.txtUser_ID.ToolTips = "";
            this.txtUser_ID.WaterMarkText = null;
            this.txtUser_ID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtUser_ID_KeyPress);
            // 
            // CmbDepartment
            // 
            this.CmbDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbDepartment.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbDepartment.FormattingEnabled = true;
            this.CmbDepartment.Location = new System.Drawing.Point(112, 11);
            this.CmbDepartment.Name = "CmbDepartment";
            this.CmbDepartment.Size = new System.Drawing.Size(169, 26);
            this.CmbDepartment.TabIndex = 30;
            this.CmbDepartment.SelectedIndexChanged += new System.EventHandler(this.CmbDepartment_SelectedIndexChanged);
            // 
            // cLabel8
            // 
            this.cLabel8.AutoSize = true;
            this.cLabel8.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel8.ForeColor = System.Drawing.Color.Black;
            this.cLabel8.Location = new System.Drawing.Point(7, 15);
            this.cLabel8.Name = "cLabel8";
            this.cLabel8.Size = new System.Drawing.Size(97, 18);
            this.cLabel8.TabIndex = 29;
            this.cLabel8.Text = "Department";
            this.cLabel8.ToolTips = "";
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
            // FrmDeptWiseMixClarityPermission
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 450);
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Controls.Add(this.MainGrid);
            this.Controls.Add(this.cPanel1);
            this.Controls.Add(this.panel1);
            this.Name = "FrmDeptWiseMixClarityPermission";
            this.Tag = "DeptWiseMixClaPermission\r\n";
            this.Text = "DEPTWISE MIXCLA PERMISSION";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repChkIsActive)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxrRemark)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtSize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repClarity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repTxtYearName)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepChkIsLock)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepDtpToDate.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepDtpToDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RepFromDate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbClarity)).EndInit();
            this.cPanel1.ResumeLayout(false);
            this.cPanel1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AxonContLib.cPanel panel1;
        private DevControlLib.cDevSimpleButton BtnExport;
        private DevControlLib.cDevSimpleButton BtnSave;
        private DevControlLib.cDevSimpleButton BtnAdd;
        private DevControlLib.cDevSimpleButton BtnBack;
        private DevExpress.XtraGrid.GridControl MainGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView GrdDet;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repChkIsActive;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repTxrRemark;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repTxtYearName;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit RepChkIsLock;
        private DevExpress.XtraEditors.Repository.RepositoryItemDateEdit RepDtpToDate;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit RepFromDate;
        private AxonContLib.cPanel cPanel1;
        private AxonContLib.cLabel cLabel8;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txtSize;
        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox cmbClarity;
        private System.Windows.Forms.ComboBox CmbDepartment;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit repClarity;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectedItemsToolStripMenuItem;
        private AxonContLib.cLabel cLabel1;
        private AxonContLib.cTextBox txtUser_ID;
        private DevControlLib.cDevSimpleButton BtnShow;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
    }
}