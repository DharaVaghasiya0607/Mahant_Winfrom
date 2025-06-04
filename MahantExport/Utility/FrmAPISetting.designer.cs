
namespace MahantExport.Utility
{
    partial class FrmAPISetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAPISetting));
            this.PanelTop = new System.Windows.Forms.Panel();
            this.CmbDiamondType = new System.Windows.Forms.ComboBox();
            this.cLabel9 = new AxonContLib.cLabel(this.components);
            this.txtExcel = new AxonContLib.cTextBox(this.components);
            this.cLabel8 = new AxonContLib.cLabel(this.components);
            this.ChkActive = new System.Windows.Forms.CheckBox();
            this.txtApi_ID = new AxonContLib.cTextBox(this.components);
            this.CmbApiType = new System.Windows.Forms.ComboBox();
            this.cLabel2 = new AxonContLib.cLabel(this.components);
            this.GrpLoginInfo = new System.Windows.Forms.GroupBox();
            this.txtUserName = new AxonContLib.cTextBox(this.components);
            this.cLabel7 = new AxonContLib.cLabel(this.components);
            this.txtPassword = new AxonContLib.cTextBox(this.components);
            this.cLabel12 = new AxonContLib.cLabel(this.components);
            this.txtPort = new AxonContLib.cTextBox(this.components);
            this.cLabel5 = new AxonContLib.cLabel(this.components);
            this.txtHost = new AxonContLib.cTextBox(this.components);
            this.cLabel4 = new AxonContLib.cLabel(this.components);
            this.txtUrl = new AxonContLib.cTextBox(this.components);
            this.cLabel3 = new AxonContLib.cLabel(this.components);
            this.txtName = new AxonContLib.cTextBox(this.components);
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            this.txtCode = new AxonContLib.cTextBox(this.components);
            this.cLabel6 = new AxonContLib.cLabel(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnDelete = new DevExpress.XtraEditors.SimpleButton();
            this.btnExit = new DevExpress.XtraEditors.SimpleButton();
            this.btnSave = new DevExpress.XtraEditors.SimpleButton();
            this.btnClear = new DevExpress.XtraEditors.SimpleButton();
            this.MainGrid = new DevExpress.XtraGrid.GridControl();
            this.GrdDet = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repChkActive = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn13 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn14 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.PanelTop.SuspendLayout();
            this.GrpLoginInfo.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repChkActive)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelTop
            // 
            this.PanelTop.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PanelTop.Controls.Add(this.CmbDiamondType);
            this.PanelTop.Controls.Add(this.cLabel9);
            this.PanelTop.Controls.Add(this.txtExcel);
            this.PanelTop.Controls.Add(this.cLabel8);
            this.PanelTop.Controls.Add(this.ChkActive);
            this.PanelTop.Controls.Add(this.txtApi_ID);
            this.PanelTop.Controls.Add(this.CmbApiType);
            this.PanelTop.Controls.Add(this.cLabel2);
            this.PanelTop.Controls.Add(this.GrpLoginInfo);
            this.PanelTop.Controls.Add(this.txtPort);
            this.PanelTop.Controls.Add(this.cLabel5);
            this.PanelTop.Controls.Add(this.txtHost);
            this.PanelTop.Controls.Add(this.cLabel4);
            this.PanelTop.Controls.Add(this.txtUrl);
            this.PanelTop.Controls.Add(this.cLabel3);
            this.PanelTop.Controls.Add(this.txtName);
            this.PanelTop.Controls.Add(this.cLabel1);
            this.PanelTop.Controls.Add(this.txtCode);
            this.PanelTop.Controls.Add(this.cLabel6);
            this.PanelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.PanelTop.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.PanelTop.Location = new System.Drawing.Point(0, 0);
            this.PanelTop.Name = "PanelTop";
            this.PanelTop.Size = new System.Drawing.Size(948, 124);
            this.PanelTop.TabIndex = 190;
            this.PanelTop.TabStop = true;
            // 
            // CmbDiamondType
            // 
            this.CmbDiamondType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbDiamondType.FormattingEnabled = true;
            this.CmbDiamondType.Items.AddRange(new object[] {
            "NATURAL",
            "HPHT",
            "CVD"});
            this.CmbDiamondType.Location = new System.Drawing.Point(789, 87);
            this.CmbDiamondType.Name = "CmbDiamondType";
            this.CmbDiamondType.Size = new System.Drawing.Size(158, 21);
            this.CmbDiamondType.TabIndex = 208;
            // 
            // cLabel9
            // 
            this.cLabel9.AutoSize = true;
            this.cLabel9.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel9.ForeColor = System.Drawing.Color.Black;
            this.cLabel9.Location = new System.Drawing.Point(682, 90);
            this.cLabel9.Name = "cLabel9";
            this.cLabel9.Size = new System.Drawing.Size(101, 14);
            this.cLabel9.TabIndex = 207;
            this.cLabel9.Text = "Diamond Type";
            this.cLabel9.ToolTips = "";
            // 
            // txtExcel
            // 
            this.txtExcel.ActivationColor = true;
            this.txtExcel.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtExcel.AllowTabKeyOnEnter = false;
            this.txtExcel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtExcel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExcel.Format = "";
            this.txtExcel.IsComplusory = false;
            this.txtExcel.Location = new System.Drawing.Point(789, 63);
            this.txtExcel.Name = "txtExcel";
            this.txtExcel.SelectAllTextOnFocus = true;
            this.txtExcel.Size = new System.Drawing.Size(158, 22);
            this.txtExcel.TabIndex = 206;
            this.txtExcel.ToolTips = "";
            this.txtExcel.WaterMarkText = null;
            // 
            // cLabel8
            // 
            this.cLabel8.AutoSize = true;
            this.cLabel8.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel8.ForeColor = System.Drawing.Color.Black;
            this.cLabel8.Location = new System.Drawing.Point(741, 67);
            this.cLabel8.Name = "cLabel8";
            this.cLabel8.Size = new System.Drawing.Size(42, 14);
            this.cLabel8.TabIndex = 205;
            this.cLabel8.Text = "Excel";
            this.cLabel8.ToolTips = "";
            // 
            // ChkActive
            // 
            this.ChkActive.AutoSize = true;
            this.ChkActive.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChkActive.Location = new System.Drawing.Point(524, 15);
            this.ChkActive.Name = "ChkActive";
            this.ChkActive.Size = new System.Drawing.Size(73, 20);
            this.ChkActive.TabIndex = 204;
            this.ChkActive.Text = "Active";
            this.ChkActive.UseVisualStyleBackColor = true;
            // 
            // txtApi_ID
            // 
            this.txtApi_ID.ActivationColor = true;
            this.txtApi_ID.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtApi_ID.AllowTabKeyOnEnter = false;
            this.txtApi_ID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtApi_ID.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtApi_ID.Format = "";
            this.txtApi_ID.IsComplusory = false;
            this.txtApi_ID.Location = new System.Drawing.Point(603, 13);
            this.txtApi_ID.Name = "txtApi_ID";
            this.txtApi_ID.SelectAllTextOnFocus = true;
            this.txtApi_ID.Size = new System.Drawing.Size(41, 22);
            this.txtApi_ID.TabIndex = 203;
            this.txtApi_ID.ToolTips = "";
            this.txtApi_ID.Visible = false;
            this.txtApi_ID.WaterMarkText = null;
            // 
            // CmbApiType
            // 
            this.CmbApiType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbApiType.FormattingEnabled = true;
            this.CmbApiType.Items.AddRange(new object[] {
            "UPLOAD",
            "DOWNLOAD"});
            this.CmbApiType.Location = new System.Drawing.Point(206, 15);
            this.CmbApiType.Name = "CmbApiType";
            this.CmbApiType.Size = new System.Drawing.Size(170, 21);
            this.CmbApiType.TabIndex = 202;
            // 
            // cLabel2
            // 
            this.cLabel2.AutoSize = true;
            this.cLabel2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel2.ForeColor = System.Drawing.Color.Black;
            this.cLabel2.Location = new System.Drawing.Point(166, 15);
            this.cLabel2.Name = "cLabel2";
            this.cLabel2.Size = new System.Drawing.Size(39, 14);
            this.cLabel2.TabIndex = 201;
            this.cLabel2.Text = "Type";
            this.cLabel2.ToolTips = "";
            // 
            // GrpLoginInfo
            // 
            this.GrpLoginInfo.Controls.Add(this.txtUserName);
            this.GrpLoginInfo.Controls.Add(this.cLabel7);
            this.GrpLoginInfo.Controls.Add(this.txtPassword);
            this.GrpLoginInfo.Controls.Add(this.cLabel12);
            this.GrpLoginInfo.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GrpLoginInfo.Location = new System.Drawing.Point(388, 43);
            this.GrpLoginInfo.Name = "GrpLoginInfo";
            this.GrpLoginInfo.Size = new System.Drawing.Size(284, 71);
            this.GrpLoginInfo.TabIndex = 200;
            this.GrpLoginInfo.TabStop = false;
            this.GrpLoginInfo.Text = "Login Info";
            // 
            // txtUserName
            // 
            this.txtUserName.ActivationColor = true;
            this.txtUserName.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtUserName.AllowTabKeyOnEnter = false;
            this.txtUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUserName.Font = new System.Drawing.Font("Verdana", 8F);
            this.txtUserName.Format = "";
            this.txtUserName.IsComplusory = false;
            this.txtUserName.Location = new System.Drawing.Point(81, 18);
            this.txtUserName.MaxLength = 100;
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.SelectAllTextOnFocus = true;
            this.txtUserName.Size = new System.Drawing.Size(196, 20);
            this.txtUserName.TabIndex = 2;
            this.txtUserName.ToolTips = "";
            this.txtUserName.WaterMarkText = null;
            // 
            // cLabel7
            // 
            this.cLabel7.AutoSize = true;
            this.cLabel7.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.cLabel7.ForeColor = System.Drawing.Color.Black;
            this.cLabel7.Location = new System.Drawing.Point(4, 22);
            this.cLabel7.Name = "cLabel7";
            this.cLabel7.Size = new System.Drawing.Size(74, 14);
            this.cLabel7.TabIndex = 1;
            this.cLabel7.Text = "Username";
            this.cLabel7.ToolTips = "";
            // 
            // txtPassword
            // 
            this.txtPassword.ActivationColor = true;
            this.txtPassword.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtPassword.AllowTabKeyOnEnter = false;
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.Font = new System.Drawing.Font("Verdana", 8F);
            this.txtPassword.Format = "";
            this.txtPassword.IsComplusory = false;
            this.txtPassword.Location = new System.Drawing.Point(81, 44);
            this.txtPassword.MaxLength = 30;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.SelectAllTextOnFocus = true;
            this.txtPassword.Size = new System.Drawing.Size(196, 20);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.ToolTips = "";
            this.txtPassword.WaterMarkText = null;
            // 
            // cLabel12
            // 
            this.cLabel12.AutoSize = true;
            this.cLabel12.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.cLabel12.ForeColor = System.Drawing.Color.Black;
            this.cLabel12.Location = new System.Drawing.Point(4, 48);
            this.cLabel12.Name = "cLabel12";
            this.cLabel12.Size = new System.Drawing.Size(72, 14);
            this.cLabel12.TabIndex = 3;
            this.cLabel12.Text = "Password";
            this.cLabel12.ToolTips = "";
            // 
            // txtPort
            // 
            this.txtPort.ActivationColor = true;
            this.txtPort.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtPort.AllowTabKeyOnEnter = false;
            this.txtPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPort.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPort.Format = "";
            this.txtPort.IsComplusory = false;
            this.txtPort.Location = new System.Drawing.Point(425, 13);
            this.txtPort.Name = "txtPort";
            this.txtPort.SelectAllTextOnFocus = true;
            this.txtPort.Size = new System.Drawing.Size(83, 22);
            this.txtPort.TabIndex = 199;
            this.txtPort.ToolTips = "";
            this.txtPort.WaterMarkText = null;
            // 
            // cLabel5
            // 
            this.cLabel5.AutoSize = true;
            this.cLabel5.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel5.ForeColor = System.Drawing.Color.Black;
            this.cLabel5.Location = new System.Drawing.Point(385, 15);
            this.cLabel5.Name = "cLabel5";
            this.cLabel5.Size = new System.Drawing.Size(35, 14);
            this.cLabel5.TabIndex = 198;
            this.cLabel5.Text = "Port";
            this.cLabel5.ToolTips = "";
            // 
            // txtHost
            // 
            this.txtHost.ActivationColor = true;
            this.txtHost.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtHost.AllowTabKeyOnEnter = false;
            this.txtHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHost.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHost.Format = "";
            this.txtHost.IsComplusory = false;
            this.txtHost.Location = new System.Drawing.Point(55, 96);
            this.txtHost.Name = "txtHost";
            this.txtHost.SelectAllTextOnFocus = true;
            this.txtHost.Size = new System.Drawing.Size(323, 22);
            this.txtHost.TabIndex = 197;
            this.txtHost.ToolTips = "";
            this.txtHost.WaterMarkText = null;
            // 
            // cLabel4
            // 
            this.cLabel4.AutoSize = true;
            this.cLabel4.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel4.ForeColor = System.Drawing.Color.Black;
            this.cLabel4.Location = new System.Drawing.Point(10, 96);
            this.cLabel4.Name = "cLabel4";
            this.cLabel4.Size = new System.Drawing.Size(37, 14);
            this.cLabel4.TabIndex = 196;
            this.cLabel4.Text = "Host";
            this.cLabel4.ToolTips = "";
            // 
            // txtUrl
            // 
            this.txtUrl.ActivationColor = true;
            this.txtUrl.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtUrl.AllowTabKeyOnEnter = false;
            this.txtUrl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUrl.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUrl.Format = "";
            this.txtUrl.IsComplusory = false;
            this.txtUrl.Location = new System.Drawing.Point(55, 69);
            this.txtUrl.Name = "txtUrl";
            this.txtUrl.SelectAllTextOnFocus = true;
            this.txtUrl.Size = new System.Drawing.Size(323, 22);
            this.txtUrl.TabIndex = 195;
            this.txtUrl.ToolTips = "";
            this.txtUrl.WaterMarkText = null;
            // 
            // cLabel3
            // 
            this.cLabel3.AutoSize = true;
            this.cLabel3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel3.ForeColor = System.Drawing.Color.Black;
            this.cLabel3.Location = new System.Drawing.Point(10, 69);
            this.cLabel3.Name = "cLabel3";
            this.cLabel3.Size = new System.Drawing.Size(34, 14);
            this.cLabel3.TabIndex = 194;
            this.cLabel3.Text = "URL";
            this.cLabel3.ToolTips = "";
            // 
            // txtName
            // 
            this.txtName.ActivationColor = true;
            this.txtName.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtName.AllowTabKeyOnEnter = false;
            this.txtName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtName.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Format = "";
            this.txtName.IsComplusory = false;
            this.txtName.Location = new System.Drawing.Point(55, 42);
            this.txtName.Name = "txtName";
            this.txtName.SelectAllTextOnFocus = true;
            this.txtName.Size = new System.Drawing.Size(323, 22);
            this.txtName.TabIndex = 193;
            this.txtName.ToolTips = "";
            this.txtName.WaterMarkText = null;
            // 
            // cLabel1
            // 
            this.cLabel1.AutoSize = true;
            this.cLabel1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel1.ForeColor = System.Drawing.Color.Black;
            this.cLabel1.Location = new System.Drawing.Point(10, 42);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(45, 14);
            this.cLabel1.TabIndex = 192;
            this.cLabel1.Text = "Name";
            this.cLabel1.ToolTips = "";
            // 
            // txtCode
            // 
            this.txtCode.ActivationColor = true;
            this.txtCode.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtCode.AllowTabKeyOnEnter = false;
            this.txtCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCode.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCode.Format = "";
            this.txtCode.IsComplusory = false;
            this.txtCode.Location = new System.Drawing.Point(55, 15);
            this.txtCode.Name = "txtCode";
            this.txtCode.SelectAllTextOnFocus = true;
            this.txtCode.Size = new System.Drawing.Size(108, 22);
            this.txtCode.TabIndex = 11;
            this.txtCode.ToolTips = "";
            this.txtCode.WaterMarkText = null;
            // 
            // cLabel6
            // 
            this.cLabel6.AutoSize = true;
            this.cLabel6.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel6.ForeColor = System.Drawing.Color.Black;
            this.cLabel6.Location = new System.Drawing.Point(10, 15);
            this.cLabel6.Name = "cLabel6";
            this.cLabel6.Size = new System.Drawing.Size(40, 14);
            this.cLabel6.TabIndex = 10;
            this.cLabel6.Text = "Code";
            this.cLabel6.ToolTips = "";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnDelete);
            this.panel3.Controls.Add(this.btnExit);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.btnClear);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel3.Location = new System.Drawing.Point(0, 124);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(948, 45);
            this.panel3.TabIndex = 192;
            this.panel3.TabStop = true;
            // 
            // btnDelete
            // 
            this.btnDelete.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.btnDelete.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Appearance.Options.UseFont = true;
            this.btnDelete.Appearance.Options.UseForeColor = true;
            this.btnDelete.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnDelete.ImageOptions.SvgImage")));
            this.btnDelete.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.btnDelete.Location = new System.Drawing.Point(102, 8);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(91, 29);
            this.btnDelete.TabIndex = 12;
            this.btnDelete.Text = "&Delete";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnExit
            // 
            this.btnExit.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.btnExit.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Appearance.Options.UseFont = true;
            this.btnExit.Appearance.Options.UseForeColor = true;
            this.btnExit.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnExit.ImageOptions.SvgImage")));
            this.btnExit.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.btnExit.Location = new System.Drawing.Point(290, 8);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(91, 29);
            this.btnExit.TabIndex = 11;
            this.btnExit.TabStop = false;
            this.btnExit.Text = "E&xit";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnSave
            // 
            this.btnSave.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.btnSave.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Appearance.Options.UseFont = true;
            this.btnSave.Appearance.Options.UseForeColor = true;
            this.btnSave.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.ImageOptions.Image")));
            this.btnSave.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.btnSave.Location = new System.Drawing.Point(9, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(91, 29);
            this.btnSave.TabIndex = 9;
            this.btnSave.Text = "&Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClear
            // 
            this.btnClear.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.btnClear.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnClear.Appearance.Options.UseFont = true;
            this.btnClear.Appearance.Options.UseForeColor = true;
            this.btnClear.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("btnClear.ImageOptions.SvgImage")));
            this.btnClear.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.btnClear.Location = new System.Drawing.Point(195, 8);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(91, 29);
            this.btnClear.TabIndex = 10;
            this.btnClear.TabStop = false;
            this.btnClear.Text = "&Clear";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // MainGrid
            // 
            this.MainGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGrid.Location = new System.Drawing.Point(0, 169);
            this.MainGrid.MainView = this.GrdDet;
            this.MainGrid.Name = "MainGrid";
            this.MainGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repChkActive});
            this.MainGrid.Size = new System.Drawing.Size(948, 377);
            this.MainGrid.TabIndex = 193;
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
            this.GrdDet.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.HeaderPanel.Options.UseFont = true;
            this.GrdDet.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.GrdDet.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDet.Appearance.HorzLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDet.Appearance.HorzLine.Options.UseBackColor = true;
            this.GrdDet.Appearance.Row.Font = new System.Drawing.Font("Verdana", 9F);
            this.GrdDet.Appearance.Row.Options.UseFont = true;
            this.GrdDet.Appearance.Row.Options.UseTextOptions = true;
            this.GrdDet.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
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
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn9,
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn5,
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn12,
            this.gridColumn13,
            this.gridColumn14});
            this.GrdDet.GridControl = this.MainGrid;
            this.GrdDet.Name = "GrdDet";
            this.GrdDet.OptionsBehavior.Editable = false;
            this.GrdDet.OptionsFilter.AllowFilterEditor = false;
            this.GrdDet.OptionsNavigation.EnterMoveNextColumn = true;
            this.GrdDet.OptionsPrint.ExpandAllGroups = false;
            this.GrdDet.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.GrdDet.OptionsView.ColumnAutoWidth = false;
            this.GrdDet.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.GrdDet.OptionsView.ShowFooter = true;
            this.GrdDet.OptionsView.ShowGroupPanel = false;
            this.GrdDet.RowHeight = 25;
            this.GrdDet.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.GrdDet_RowCellClick);
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
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
            this.gridColumn1.Caption = "API_ID";
            this.gridColumn1.FieldName = "API_ID";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn1.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn1.Width = 57;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
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
            this.gridColumn2.Caption = "Code";
            this.gridColumn2.FieldName = "APICODE";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.OptionsColumn.AllowEdit = false;
            this.gridColumn2.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 0;
            this.gridColumn2.Width = 88;
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
            this.gridColumn9.Caption = "Name";
            this.gridColumn9.FieldName = "APINAME";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowEdit = false;
            this.gridColumn9.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn9.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 1;
            this.gridColumn9.Width = 180;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
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
            this.gridColumn3.Caption = "URL";
            this.gridColumn3.FieldName = "URL";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsColumn.AllowEdit = false;
            this.gridColumn3.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
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
            this.gridColumn4.Caption = "Host Name";
            this.gridColumn4.FieldName = "HOSTNAME";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsColumn.AllowEdit = false;
            this.gridColumn4.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn4.Visible = true;
            this.gridColumn4.VisibleIndex = 3;
            this.gridColumn4.Width = 226;
            // 
            // gridColumn5
            // 
            this.gridColumn5.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
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
            this.gridColumn5.Caption = "Port";
            this.gridColumn5.FieldName = "PORT";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.OptionsColumn.AllowEdit = false;
            this.gridColumn5.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
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
            this.gridColumn6.Caption = "User Name";
            this.gridColumn6.FieldName = "USERNAME";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsColumn.AllowEdit = false;
            this.gridColumn6.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 6;
            this.gridColumn6.Width = 143;
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
            this.gridColumn7.Caption = "PassWord";
            this.gridColumn7.FieldName = "PASSWORD";
            this.gridColumn7.Name = "gridColumn7";
            this.gridColumn7.OptionsColumn.AllowEdit = false;
            this.gridColumn7.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn7.Visible = true;
            this.gridColumn7.VisibleIndex = 7;
            this.gridColumn7.Width = 136;
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
            this.gridColumn8.Caption = "Certi";
            this.gridColumn8.FieldName = "CERTICATE";
            this.gridColumn8.Name = "gridColumn8";
            this.gridColumn8.OptionsColumn.AllowEdit = false;
            this.gridColumn8.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn8.Visible = true;
            this.gridColumn8.VisibleIndex = 4;
            this.gridColumn8.Width = 145;
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
            this.gridColumn10.Caption = "Type";
            this.gridColumn10.FieldName = "APITYPE";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.OptionsColumn.AllowEdit = false;
            this.gridColumn10.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 2;
            this.gridColumn10.Width = 126;
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
            this.gridColumn11.Caption = "Active";
            this.gridColumn11.ColumnEdit = this.repChkActive;
            this.gridColumn11.FieldName = "ACTIVE";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsColumn.AllowEdit = false;
            this.gridColumn11.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 8;
            // 
            // repChkActive
            // 
            this.repChkActive.AutoHeight = false;
            this.repChkActive.Caption = "Check";
            this.repChkActive.Name = "repChkActive";
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
            this.gridColumn12.Caption = "Port";
            this.gridColumn12.FieldName = "PORT";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.OptionsColumn.AllowEdit = false;
            this.gridColumn12.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 5;
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
            this.gridColumn13.Caption = "Excel";
            this.gridColumn13.FieldName = "EXCELNAME";
            this.gridColumn13.Name = "gridColumn13";
            this.gridColumn13.Visible = true;
            this.gridColumn13.VisibleIndex = 9;
            // 
            // gridColumn14
            // 
            this.gridColumn14.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn14.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn14.AppearanceCell.Options.UseFont = true;
            this.gridColumn14.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn14.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn14.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn14.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn14.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn14.AppearanceHeader.Options.UseFont = true;
            this.gridColumn14.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn14.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn14.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn14.Caption = "Diamond";
            this.gridColumn14.FieldName = "DIAMONDTYPE";
            this.gridColumn14.Name = "gridColumn14";
            this.gridColumn14.Visible = true;
            this.gridColumn14.VisibleIndex = 10;
            // 
            // FrmAPISetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 546);
            this.Controls.Add(this.MainGrid);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.PanelTop);
            this.Name = "FrmAPISetting";
            this.Text = "API SETTING";
            this.PanelTop.ResumeLayout(false);
            this.PanelTop.PerformLayout();
            this.GrpLoginInfo.ResumeLayout(false);
            this.GrpLoginInfo.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repChkActive)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel PanelTop;
        private AxonContLib.cTextBox txtApi_ID;
        private System.Windows.Forms.ComboBox CmbApiType;
        private AxonContLib.cLabel cLabel2;
        private System.Windows.Forms.GroupBox GrpLoginInfo;
        private AxonContLib.cTextBox txtUserName;
        private AxonContLib.cLabel cLabel7;
        private AxonContLib.cTextBox txtPassword;
        private AxonContLib.cLabel cLabel12;
        private AxonContLib.cTextBox txtPort;
        private AxonContLib.cLabel cLabel5;
        private AxonContLib.cTextBox txtHost;
        private AxonContLib.cLabel cLabel4;
        private AxonContLib.cTextBox txtUrl;
        private AxonContLib.cLabel cLabel3;
        private AxonContLib.cTextBox txtName;
        private AxonContLib.cLabel cLabel1;
        private AxonContLib.cTextBox txtCode;
        private AxonContLib.cLabel cLabel6;
        private System.Windows.Forms.Panel panel3;
        private DevExpress.XtraEditors.SimpleButton btnDelete;
        private DevExpress.XtraEditors.SimpleButton btnExit;
        private DevExpress.XtraEditors.SimpleButton btnSave;
        private DevExpress.XtraEditors.SimpleButton btnClear;
        private DevExpress.XtraGrid.GridControl MainGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView GrdDet;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repChkActive;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private System.Windows.Forms.CheckBox ChkActive;
        private System.Windows.Forms.ComboBox CmbDiamondType;
        private AxonContLib.cLabel cLabel9;
        private AxonContLib.cTextBox txtExcel;
        private AxonContLib.cLabel cLabel8;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn13;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn14;
    }
}