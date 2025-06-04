namespace MahantExport.Masters
{
    partial class FrmEmployeeRights
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEmployeeRights));
            this.MainGridForm = new DevExpress.XtraGrid.GridControl();
            this.GrdDetForm = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemCheckEdit4 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.txtPass = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel2 = new AxonContLib.cPanel(this.components);
            this.ChkComputerPrice = new AxonContLib.cCheckBox(this.components);
            this.ChkIsAllMfgCost = new AxonContLib.cCheckBox(this.components);
            this.cLabel2 = new AxonContLib.cLabel(this.components);
            this.CmbProcess = new DevControlLib.cDevCheckedComboBoxEdit(this.components);
            this.ChkShowAllOrder = new AxonContLib.cCheckBox(this.components);
            this.chkShowAllParty = new AxonContLib.cCheckBox(this.components);
            this.ChkPurParty = new AxonContLib.cCheckBox(this.components);
            this.ChkCostPrice = new AxonContLib.cCheckBox(this.components);
            this.cLabel6 = new AxonContLib.cLabel(this.components);
            this.txtEmployee = new AxonContLib.cTextBox(this.components);
            this.BtnShow = new DevControlLib.cDevSimpleButton(this.components);
            this.chkAllView = new AxonContLib.cCheckBox(this.components);
            this.txtCopyFrom = new AxonContLib.cTextBox(this.components);
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            this.groupControl1 = new DevControlLib.cDevGroupControl(this.components);
            this.panel3 = new AxonContLib.cPanel(this.components);
            this.ChkAllInsert = new AxonContLib.cCheckBox(this.components);
            this.ChkAllUpdate = new AxonContLib.cCheckBox(this.components);
            this.ChkDelete = new AxonContLib.cCheckBox(this.components);
            this.panel1 = new AxonContLib.cPanel(this.components);
            this.BtnSaveLayoutRights = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnSave = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnAdd = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnBack = new DevControlLib.cDevSimpleButton(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            ((System.ComponentModel.ISupportInitialize)(this.MainGridForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDetForm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPass)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbProcess.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainGridForm
            // 
            this.MainGridForm.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGridForm.Location = new System.Drawing.Point(2, 54);
            this.MainGridForm.MainView = this.GrdDetForm;
            this.MainGridForm.Name = "MainGridForm";
            this.MainGridForm.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemCheckEdit1,
            this.repositoryItemCheckEdit2,
            this.repositoryItemCheckEdit3,
            this.repositoryItemCheckEdit4,
            this.txtPass});
            this.MainGridForm.Size = new System.Drawing.Size(1433, 275);
            this.MainGridForm.TabIndex = 0;
            this.MainGridForm.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GrdDetForm});
            // 
            // GrdDetForm
            // 
            this.GrdDetForm.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.GrdDetForm.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.GrdDetForm.Appearance.FocusedCell.Options.UseBackColor = true;
            this.GrdDetForm.Appearance.FooterPanel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.GrdDetForm.Appearance.FooterPanel.Options.UseFont = true;
            this.GrdDetForm.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.GrdDetForm.Appearance.HeaderPanel.Options.UseFont = true;
            this.GrdDetForm.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.GrdDetForm.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDetForm.Appearance.HorzLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDetForm.Appearance.HorzLine.Options.UseBackColor = true;
            this.GrdDetForm.Appearance.Row.Font = new System.Drawing.Font("Verdana", 9F);
            this.GrdDetForm.Appearance.Row.Options.UseFont = true;
            this.GrdDetForm.Appearance.VertLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDetForm.Appearance.VertLine.Options.UseBackColor = true;
            this.GrdDetForm.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
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
            this.gridColumn11});
            this.GrdDetForm.GridControl = this.MainGridForm;
            this.GrdDetForm.Name = "GrdDetForm";
            this.GrdDetForm.OptionsCustomization.AllowFilter = false;
            this.GrdDetForm.OptionsCustomization.AllowSort = false;
            this.GrdDetForm.OptionsFilter.AllowFilterEditor = false;
            this.GrdDetForm.OptionsNavigation.EnterMoveNextColumn = true;
            this.GrdDetForm.OptionsPrint.ExpandAllGroups = false;
            this.GrdDetForm.OptionsView.ColumnAutoWidth = false;
            this.GrdDetForm.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.GrdDetForm.OptionsView.ShowAutoFilterRow = true;
            this.GrdDetForm.OptionsView.ShowGroupPanel = false;
            this.GrdDetForm.RowHeight = 23;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Font = new System.Drawing.Font("Verdana", 9F);
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
            this.gridColumn1.Caption = "Group";
            this.gridColumn1.FieldName = "FORMGROUP";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 121;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Font = new System.Drawing.Font("Verdana", 9F);
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
            this.gridColumn2.Caption = "FORMNAME";
            this.gridColumn2.FieldName = "FORMNAME";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Font = new System.Drawing.Font("Verdana", 9F);
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
            this.gridColumn3.Caption = "Form Name";
            this.gridColumn3.FieldName = "FORMDESC";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn3.Width = 241;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Font = new System.Drawing.Font("Verdana", 9F);
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
            this.gridColumn4.Caption = "FORM_ID";
            this.gridColumn4.FieldName = "FORM_ID";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Font = new System.Drawing.Font("Verdana", 9F);
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
            this.gridColumn5.Caption = "View";
            this.gridColumn5.ColumnEdit = this.repositoryItemCheckEdit1;
            this.gridColumn5.FieldName = "ISVIEW";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 2;
            this.gridColumn5.Width = 55;
            // 
            // repositoryItemCheckEdit1
            // 
            this.repositoryItemCheckEdit1.AutoHeight = false;
            this.repositoryItemCheckEdit1.Caption = "Check";
            this.repositoryItemCheckEdit1.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
            this.repositoryItemCheckEdit1.ImageOptions.ImageChecked = global::MahantExport.Properties.Resources.Checked;
            this.repositoryItemCheckEdit1.ImageOptions.ImageUnchecked = global::MahantExport.Properties.Resources.Unchecked;
            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Font = new System.Drawing.Font("Verdana", 9F);
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
            this.gridColumn6.Caption = "Insert";
            this.gridColumn6.ColumnEdit = this.repositoryItemCheckEdit2;
            this.gridColumn6.FieldName = "ISINSERT";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 3;
            this.gridColumn6.Width = 60;
            // 
            // repositoryItemCheckEdit2
            // 
            this.repositoryItemCheckEdit2.AutoHeight = false;
            this.repositoryItemCheckEdit2.Caption = "Check";
            this.repositoryItemCheckEdit2.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
            this.repositoryItemCheckEdit2.ImageOptions.ImageChecked = global::MahantExport.Properties.Resources.Checked;
            this.repositoryItemCheckEdit2.ImageOptions.ImageUnchecked = global::MahantExport.Properties.Resources.Unchecked;
            this.repositoryItemCheckEdit2.Name = "repositoryItemCheckEdit2";
            // 
            // gridColumn7
            // 
            this.gridColumn7.AppearanceCell.Font = new System.Drawing.Font("Verdana", 9F);
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
            this.gridColumn7.Caption = "Update";
            this.gridColumn7.ColumnEdit = this.repositoryItemCheckEdit3;
            this.gridColumn7.FieldName = "ISUPDATE";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 4;
            this.gridColumn7.Width = 66;
            // 
            // repositoryItemCheckEdit3
            // 
            this.repositoryItemCheckEdit3.AutoHeight = false;
            this.repositoryItemCheckEdit3.Caption = "Check";
            this.repositoryItemCheckEdit3.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
            this.repositoryItemCheckEdit3.ImageOptions.ImageChecked = global::MahantExport.Properties.Resources.Checked;
            this.repositoryItemCheckEdit3.ImageOptions.ImageUnchecked = global::MahantExport.Properties.Resources.Unchecked;
            this.repositoryItemCheckEdit3.Name = "repositoryItemCheckEdit3";
            // 
            // gridColumn8
            // 
            this.gridColumn8.AppearanceCell.Font = new System.Drawing.Font("Verdana", 9F);
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
            this.gridColumn8.Caption = "Delete";
            this.gridColumn8.ColumnEdit = this.repositoryItemCheckEdit4;
            this.gridColumn8.FieldName = "ISDELETE";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 5;
            this.gridColumn8.Width = 65;
            // 
            // repositoryItemCheckEdit4
            // 
            this.repositoryItemCheckEdit4.AutoHeight = false;
            this.repositoryItemCheckEdit4.Caption = "Check";
            this.repositoryItemCheckEdit4.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
            this.repositoryItemCheckEdit4.ImageOptions.ImageChecked = global::MahantExport.Properties.Resources.Checked;
            this.repositoryItemCheckEdit4.ImageOptions.ImageUnchecked = global::MahantExport.Properties.Resources.Unchecked;
            this.repositoryItemCheckEdit4.Name = "repositoryItemCheckEdit4";
            // 
            // gridColumn9
            // 
            this.gridColumn9.AppearanceCell.Font = new System.Drawing.Font("Verdana", 9F);
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
            this.gridColumn9.Caption = "Password";
            this.gridColumn9.ColumnEdit = this.txtPass;
            this.gridColumn9.FieldName = "PASSWORD";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 6;
            this.gridColumn9.Width = 127;
            // 
            // txtPass
            // 
            this.txtPass.AutoHeight = false;
            this.txtPass.Name = "txtPass";
            this.txtPass.PasswordChar = '*';
            // 
            // gridColumn10
            // 
            this.gridColumn10.AppearanceCell.Font = new System.Drawing.Font("Verdana", 9F);
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
            this.gridColumn10.Caption = "Link Name (English)";
            this.gridColumn10.FieldName = "FORMDESC";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsColumn.AllowEdit = false;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 1;
            this.gridColumn10.Width = 281;
            // 
            // gridColumn11
            // 
            this.gridColumn11.AppearanceCell.Font = new System.Drawing.Font("Verdana", 9F);
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
            this.gridColumn11.Caption = "Link Name (Chinese)";
            this.gridColumn11.FieldName = "FORMDESCNEW";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsColumn.AllowEdit = false;
            this.gridColumn11.Width = 242;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.ChkComputerPrice);
            this.panel2.Controls.Add(this.ChkIsAllMfgCost);
            this.panel2.Controls.Add(this.cLabel2);
            this.panel2.Controls.Add(this.CmbProcess);
            this.panel2.Controls.Add(this.ChkShowAllOrder);
            this.panel2.Controls.Add(this.chkShowAllParty);
            this.panel2.Controls.Add(this.ChkPurParty);
            this.panel2.Controls.Add(this.ChkCostPrice);
            this.panel2.Controls.Add(this.cLabel6);
            this.panel2.Controls.Add(this.txtEmployee);
            this.panel2.Controls.Add(this.BtnShow);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1451, 69);
            this.panel2.TabIndex = 0;
            // 
            // ChkComputerPrice
            // 
            this.ChkComputerPrice.AllowTabKeyOnEnter = false;
            this.ChkComputerPrice.AutoSize = true;
            this.ChkComputerPrice.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkComputerPrice.Location = new System.Drawing.Point(1043, 43);
            this.ChkComputerPrice.Name = "ChkComputerPrice";
            this.ChkComputerPrice.Size = new System.Drawing.Size(178, 17);
            this.ChkComputerPrice.TabIndex = 184;
            this.ChkComputerPrice.Text = "Display Computer Price";
            this.ChkComputerPrice.ToolTips = "";
            this.ChkComputerPrice.UseVisualStyleBackColor = true;
            // 
            // ChkIsAllMfgCost
            // 
            this.ChkIsAllMfgCost.AllowTabKeyOnEnter = false;
            this.ChkIsAllMfgCost.AutoSize = true;
            this.ChkIsAllMfgCost.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkIsAllMfgCost.Location = new System.Drawing.Point(900, 42);
            this.ChkIsAllMfgCost.Name = "ChkIsAllMfgCost";
            this.ChkIsAllMfgCost.Size = new System.Drawing.Size(142, 17);
            this.ChkIsAllMfgCost.TabIndex = 183;
            this.ChkIsAllMfgCost.Text = "Display MFG Price";
            this.ChkIsAllMfgCost.ToolTips = "";
            this.ChkIsAllMfgCost.UseVisualStyleBackColor = true;
            // 
            // cLabel2
            // 
            this.cLabel2.AutoSize = true;
            this.cLabel2.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel2.ForeColor = System.Drawing.Color.Black;
            this.cLabel2.Location = new System.Drawing.Point(8, 41);
            this.cLabel2.Name = "cLabel2";
            this.cLabel2.Size = new System.Drawing.Size(62, 17);
            this.cLabel2.TabIndex = 182;
            this.cLabel2.Text = "Process";
            this.cLabel2.ToolTips = "";
            // 
            // CmbProcess
            // 
            this.CmbProcess.Location = new System.Drawing.Point(91, 38);
            this.CmbProcess.Name = "CmbProcess";
            this.CmbProcess.Properties.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.CmbProcess.Properties.Appearance.Options.UseFont = true;
            this.CmbProcess.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Verdana", 9F);
            this.CmbProcess.Properties.AppearanceDropDown.Options.UseFont = true;
            this.CmbProcess.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbProcess.Properties.DropDownRows = 20;
            this.CmbProcess.Size = new System.Drawing.Size(213, 20);
            this.CmbProcess.TabIndex = 181;
            // 
            // ChkShowAllOrder
            // 
            this.ChkShowAllOrder.AllowTabKeyOnEnter = false;
            this.ChkShowAllOrder.AutoSize = true;
            this.ChkShowAllOrder.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkShowAllOrder.Location = new System.Drawing.Point(764, 43);
            this.ChkShowAllOrder.Name = "ChkShowAllOrder";
            this.ChkShowAllOrder.Size = new System.Drawing.Size(136, 17);
            this.ChkShowAllOrder.TabIndex = 6;
            this.ChkShowAllOrder.Text = "Display All Order";
            this.ChkShowAllOrder.ToolTips = "";
            this.ChkShowAllOrder.UseVisualStyleBackColor = true;
            // 
            // chkShowAllParty
            // 
            this.chkShowAllParty.AllowTabKeyOnEnter = false;
            this.chkShowAllParty.AutoSize = true;
            this.chkShowAllParty.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowAllParty.Location = new System.Drawing.Point(630, 43);
            this.chkShowAllParty.Name = "chkShowAllParty";
            this.chkShowAllParty.Size = new System.Drawing.Size(134, 17);
            this.chkShowAllParty.TabIndex = 5;
            this.chkShowAllParty.Text = "Display All Party";
            this.chkShowAllParty.ToolTips = "";
            this.chkShowAllParty.UseVisualStyleBackColor = true;
            // 
            // ChkPurParty
            // 
            this.ChkPurParty.AllowTabKeyOnEnter = false;
            this.ChkPurParty.AutoSize = true;
            this.ChkPurParty.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkPurParty.Location = new System.Drawing.Point(453, 43);
            this.ChkPurParty.Name = "ChkPurParty";
            this.ChkPurParty.Size = new System.Drawing.Size(177, 17);
            this.ChkPurParty.TabIndex = 4;
            this.ChkPurParty.Text = "Display Purchase Party";
            this.ChkPurParty.ToolTips = "";
            this.ChkPurParty.UseVisualStyleBackColor = true;
            // 
            // ChkCostPrice
            // 
            this.ChkCostPrice.AllowTabKeyOnEnter = false;
            this.ChkCostPrice.AutoSize = true;
            this.ChkCostPrice.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkCostPrice.Location = new System.Drawing.Point(310, 43);
            this.ChkCostPrice.Name = "ChkCostPrice";
            this.ChkCostPrice.Size = new System.Drawing.Size(143, 17);
            this.ChkCostPrice.TabIndex = 3;
            this.ChkCostPrice.Text = "Display Cost Price";
            this.ChkCostPrice.ToolTips = "";
            this.ChkCostPrice.UseVisualStyleBackColor = true;
            // 
            // cLabel6
            // 
            this.cLabel6.AutoSize = true;
            this.cLabel6.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel6.ForeColor = System.Drawing.Color.Black;
            this.cLabel6.Location = new System.Drawing.Point(8, 12);
            this.cLabel6.Name = "cLabel6";
            this.cLabel6.Size = new System.Drawing.Size(75, 17);
            this.cLabel6.TabIndex = 0;
            this.cLabel6.Text = "Employee";
            this.cLabel6.ToolTips = "";
            // 
            // txtEmployee
            // 
            this.txtEmployee.ActivationColor = true;
            this.txtEmployee.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtEmployee.AllowTabKeyOnEnter = false;
            this.txtEmployee.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtEmployee.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEmployee.Format = "";
            this.txtEmployee.IsComplusory = false;
            this.txtEmployee.Location = new System.Drawing.Point(91, 8);
            this.txtEmployee.Name = "txtEmployee";
            this.txtEmployee.SelectAllTextOnFocus = true;
            this.txtEmployee.Size = new System.Drawing.Size(338, 24);
            this.txtEmployee.TabIndex = 1;
            this.txtEmployee.ToolTips = "";
            this.txtEmployee.WaterMarkText = null;
            this.txtEmployee.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtEmployee_KeyPress);
            // 
            // BtnShow
            // 
            this.BtnShow.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnShow.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnShow.Appearance.Options.UseFont = true;
            this.BtnShow.Appearance.Options.UseForeColor = true;
            this.BtnShow.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnShow.ImageOptions.SvgImage")));
            this.BtnShow.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnShow.Location = new System.Drawing.Point(435, 3);
            this.BtnShow.Name = "BtnShow";
            this.BtnShow.Size = new System.Drawing.Size(100, 35);
            this.BtnShow.TabIndex = 2;
            this.BtnShow.Text = "Show";
            this.BtnShow.Click += new System.EventHandler(this.BtnShow_Click);
            // 
            // chkAllView
            // 
            this.chkAllView.AllowTabKeyOnEnter = false;
            this.chkAllView.AutoSize = true;
            this.chkAllView.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAllView.Location = new System.Drawing.Point(367, 7);
            this.chkAllView.Name = "chkAllView";
            this.chkAllView.Size = new System.Drawing.Size(77, 17);
            this.chkAllView.TabIndex = 184;
            this.chkAllView.Text = "All View";
            this.chkAllView.ToolTips = "";
            this.chkAllView.UseVisualStyleBackColor = true;
            this.chkAllView.CheckedChanged += new System.EventHandler(this.chkAllView_CheckedChanged);
            // 
            // txtCopyFrom
            // 
            this.txtCopyFrom.ActivationColor = true;
            this.txtCopyFrom.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtCopyFrom.AllowTabKeyOnEnter = false;
            this.txtCopyFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCopyFrom.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCopyFrom.Format = "";
            this.txtCopyFrom.IsComplusory = false;
            this.txtCopyFrom.Location = new System.Drawing.Point(711, 7);
            this.txtCopyFrom.Name = "txtCopyFrom";
            this.txtCopyFrom.SelectAllTextOnFocus = true;
            this.txtCopyFrom.Size = new System.Drawing.Size(240, 24);
            this.txtCopyFrom.TabIndex = 1;
            this.txtCopyFrom.ToolTips = "";
            this.txtCopyFrom.WaterMarkText = null;
            this.txtCopyFrom.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCopyFrom_KeyPress);
            // 
            // cLabel1
            // 
            this.cLabel1.AutoSize = true;
            this.cLabel1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel1.ForeColor = System.Drawing.Color.Black;
            this.cLabel1.Location = new System.Drawing.Point(494, 11);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(213, 17);
            this.cLabel1.TabIndex = 0;
            this.cLabel1.Text = "Form Rights Copy From User";
            this.cLabel1.ToolTips = "";
            // 
            // groupControl1
            // 
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.AppearanceCaption.Options.UseTextOptions = true;
            this.groupControl1.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.groupControl1.Controls.Add(this.MainGridForm);
            this.groupControl1.Controls.Add(this.panel3);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(3, 2);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1437, 331);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Form Permission";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.chkAllView);
            this.panel3.Controls.Add(this.ChkAllInsert);
            this.panel3.Controls.Add(this.ChkAllUpdate);
            this.panel3.Controls.Add(this.ChkDelete);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(2, 23);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1433, 31);
            this.panel3.TabIndex = 185;
            // 
            // ChkAllInsert
            // 
            this.ChkAllInsert.AllowTabKeyOnEnter = false;
            this.ChkAllInsert.AutoSize = true;
            this.ChkAllInsert.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkAllInsert.Location = new System.Drawing.Point(446, 7);
            this.ChkAllInsert.Name = "ChkAllInsert";
            this.ChkAllInsert.Size = new System.Drawing.Size(87, 17);
            this.ChkAllInsert.TabIndex = 187;
            this.ChkAllInsert.Text = "All Insert";
            this.ChkAllInsert.ToolTips = "";
            this.ChkAllInsert.UseVisualStyleBackColor = true;
            this.ChkAllInsert.CheckedChanged += new System.EventHandler(this.ChkAllInsert_CheckedChanged);
            // 
            // ChkAllUpdate
            // 
            this.ChkAllUpdate.AllowTabKeyOnEnter = false;
            this.ChkAllUpdate.AutoSize = true;
            this.ChkAllUpdate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkAllUpdate.Location = new System.Drawing.Point(535, 7);
            this.ChkAllUpdate.Name = "ChkAllUpdate";
            this.ChkAllUpdate.Size = new System.Drawing.Size(93, 17);
            this.ChkAllUpdate.TabIndex = 186;
            this.ChkAllUpdate.Text = "All Update";
            this.ChkAllUpdate.ToolTips = "";
            this.ChkAllUpdate.UseVisualStyleBackColor = true;
            this.ChkAllUpdate.CheckedChanged += new System.EventHandler(this.ChkAllUpdate_CheckedChanged);
            // 
            // ChkDelete
            // 
            this.ChkDelete.AllowTabKeyOnEnter = false;
            this.ChkDelete.AutoSize = true;
            this.ChkDelete.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkDelete.Location = new System.Drawing.Point(630, 7);
            this.ChkDelete.Name = "ChkDelete";
            this.ChkDelete.Size = new System.Drawing.Size(89, 17);
            this.ChkDelete.TabIndex = 185;
            this.ChkDelete.Text = "All Delete";
            this.ChkDelete.ToolTips = "";
            this.ChkDelete.UseVisualStyleBackColor = true;
            this.ChkDelete.CheckedChanged += new System.EventHandler(this.ChkDelete_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.Controls.Add(this.BtnSaveLayoutRights);
            this.panel1.Controls.Add(this.BtnSave);
            this.panel1.Controls.Add(this.BtnAdd);
            this.panel1.Controls.Add(this.BtnBack);
            this.panel1.Controls.Add(this.txtCopyFrom);
            this.panel1.Controls.Add(this.cLabel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 431);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1451, 43);
            this.panel1.TabIndex = 0;
            // 
            // BtnSaveLayoutRights
            // 
            this.BtnSaveLayoutRights.Appearance.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSaveLayoutRights.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnSaveLayoutRights.Appearance.Options.UseFont = true;
            this.BtnSaveLayoutRights.Appearance.Options.UseForeColor = true;
            this.BtnSaveLayoutRights.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSaveLayoutRights.ImageOptions.SvgImage")));
            this.BtnSaveLayoutRights.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnSaveLayoutRights.Location = new System.Drawing.Point(325, 4);
            this.BtnSaveLayoutRights.Name = "BtnSaveLayoutRights";
            this.BtnSaveLayoutRights.Size = new System.Drawing.Size(163, 35);
            this.BtnSaveLayoutRights.TabIndex = 33;
            this.BtnSaveLayoutRights.Text = "&Save Layout Rights";
            this.BtnSaveLayoutRights.Click += new System.EventHandler(this.BtnSaveLayoutRights_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.Appearance.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSave.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnSave.Appearance.Options.UseFont = true;
            this.BtnSave.Appearance.Options.UseForeColor = true;
            this.BtnSave.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSave.ImageOptions.SvgImage")));
            this.BtnSave.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnSave.Location = new System.Drawing.Point(5, 4);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(103, 35);
            this.BtnSave.TabIndex = 0;
            this.BtnSave.Text = "&Save";
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnAdd
            // 
            this.BtnAdd.Appearance.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAdd.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnAdd.Appearance.Options.UseFont = true;
            this.BtnAdd.Appearance.Options.UseForeColor = true;
            this.BtnAdd.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnAdd.ImageOptions.SvgImage")));
            this.BtnAdd.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnAdd.Location = new System.Drawing.Point(111, 4);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(103, 35);
            this.BtnAdd.TabIndex = 31;
            this.BtnAdd.TabStop = false;
            this.BtnAdd.Text = "&Clear";
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // BtnBack
            // 
            this.BtnBack.Appearance.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnBack.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnBack.Appearance.Options.UseFont = true;
            this.BtnBack.Appearance.Options.UseForeColor = true;
            this.BtnBack.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnBack.ImageOptions.SvgImage")));
            this.BtnBack.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnBack.Location = new System.Drawing.Point(217, 4);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.Size = new System.Drawing.Size(103, 35);
            this.BtnBack.TabIndex = 32;
            this.BtnBack.TabStop = false;
            this.BtnBack.Text = "E&xit";
            this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 69);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1451, 362);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupControl1);
            this.tabPage1.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabPage1.Location = new System.Drawing.Point(4, 23);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabPage1.Size = new System.Drawing.Size(1443, 335);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "   FORM PERMISSIONS   ";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // FrmEmployeeRights
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1451, 474);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "FrmEmployeeRights";
            this.Tag = "UserPermission";
            this.Text = "EMPLOYEE RIGHTS";
            ((System.ComponentModel.ISupportInitialize)(this.MainGridForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDetForm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtPass)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbProcess.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraGrid.GridControl MainGridForm;
        private DevExpress.XtraGrid.Views.Grid.GridView GrdDetForm;
        private AxonContLib.cTextBox txtEmployee;
        private AxonContLib.cLabel cLabel6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevControlLib.cDevGroupControl groupControl1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit2;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit3;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit4;
        private DevControlLib.cDevSimpleButton BtnShow;
        private AxonContLib.cTextBox txtCopyFrom;
        private AxonContLib.cLabel cLabel1;
        private AxonContLib.cPanel panel1;
        private DevControlLib.cDevSimpleButton BtnSave;
        private DevControlLib.cDevSimpleButton BtnAdd;
        private DevControlLib.cDevSimpleButton BtnBack;
        private AxonContLib.cPanel panel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private AxonContLib.cCheckBox ChkCostPrice;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private AxonContLib.cCheckBox ChkPurParty;
        private AxonContLib.cCheckBox ChkShowAllOrder;
        private AxonContLib.cCheckBox chkShowAllParty;
        private AxonContLib.cLabel cLabel2;
        private DevControlLib.cDevCheckedComboBoxEdit CmbProcess;
        private AxonContLib.cCheckBox ChkIsAllMfgCost;
        private AxonContLib.cCheckBox chkAllView;
        private AxonContLib.cPanel panel3;
        private AxonContLib.cCheckBox ChkAllInsert;
        private AxonContLib.cCheckBox ChkAllUpdate;
        private AxonContLib.cCheckBox ChkDelete;
        private AxonContLib.cCheckBox ChkComputerPrice;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit txtPass;
        private DevControlLib.cDevSimpleButton BtnSaveLayoutRights;
    }
}