namespace MahantExport.Parcel
{
    partial class FrmBombayTransfer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBombayTransfer));
            this.BtnDelete = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnClear = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnSave = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnBack = new DevControlLib.cDevSimpleButton(this.components);
            this.txtKapan = new AxonContLib.cTextBox(this.components);
            this.BtnSearch = new DevControlLib.cDevSimpleButton(this.components);
            this.label2 = new AxonContLib.cLabel(this.components);
            this.panel2 = new AxonContLib.cPanel(this.components);
            this.txtAmount = new AxonContLib.cTextBox(this.components);
            this.TxtPricePerCarat = new AxonContLib.cTextBox(this.components);
            this.label5 = new AxonContLib.cLabel(this.components);
            this.txtBalanceCarat = new AxonContLib.cTextBox(this.components);
            this.label1 = new AxonContLib.cLabel(this.components);
            this.txtPcs = new AxonContLib.cTextBox(this.components);
            this.label7 = new AxonContLib.cLabel(this.components);
            this.label10 = new AxonContLib.cLabel(this.components);
            this.label4 = new AxonContLib.cLabel(this.components);
            this.txtBombayTransferNo = new AxonContLib.cTextBox(this.components);
            this.MainGridDetail = new DevExpress.XtraGrid.GridControl();
            this.GrdDetDetail = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn16 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn17 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn18 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn19 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn20 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn21 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn23 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn24 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn25 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn29 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn30 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn31 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn32 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn33 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel4 = new AxonContLib.cPanel(this.components);
            this.panel5 = new AxonContLib.cPanel(this.components);
            this.label6 = new AxonContLib.cLabel(this.components);
            this.DTPToDate = new System.Windows.Forms.DateTimePicker();
            this.DTPFromDate = new System.Windows.Forms.DateTimePicker();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGridDetail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDetDetail)).BeginInit();
            this.panel4.SuspendLayout();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnDelete
            // 
            this.BtnDelete.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnDelete.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnDelete.Appearance.Options.UseFont = true;
            this.BtnDelete.Appearance.Options.UseForeColor = true;
            this.BtnDelete.ImageOptions.Image = global::MahantExport.Properties.Resources.btndelete;
            this.BtnDelete.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnDelete.ImageOptions.SvgImage")));
            this.BtnDelete.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnDelete.Location = new System.Drawing.Point(104, 7);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(90, 35);
            this.BtnDelete.TabIndex = 34;
            this.BtnDelete.TabStop = false;
            this.BtnDelete.Text = "&Delete";
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnClear.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnClear.Appearance.Options.UseFont = true;
            this.BtnClear.Appearance.Options.UseForeColor = true;
            this.BtnClear.ImageOptions.Image = global::MahantExport.Properties.Resources.btnclear;
            this.BtnClear.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnClear.ImageOptions.SvgImage")));
            this.BtnClear.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnClear.Location = new System.Drawing.Point(200, 7);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(90, 35);
            this.BtnClear.TabIndex = 33;
            this.BtnClear.TabStop = false;
            this.BtnClear.Text = "Clear";
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
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
            this.BtnSave.Location = new System.Drawing.Point(8, 7);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(90, 35);
            this.BtnSave.TabIndex = 30;
            this.BtnSave.Text = "&Transfer";
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
            this.BtnBack.Location = new System.Drawing.Point(296, 7);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.Size = new System.Drawing.Size(90, 35);
            this.BtnBack.TabIndex = 32;
            this.BtnBack.TabStop = false;
            this.BtnBack.Text = "E&xit";
            this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // txtKapan
            // 
            this.txtKapan.ActivationColor = false;
            this.txtKapan.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtKapan.AllowTabKeyOnEnter = false;
            this.txtKapan.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtKapan.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtKapan.Format = "";
            this.txtKapan.IsComplusory = false;
            this.txtKapan.Location = new System.Drawing.Point(65, 13);
            this.txtKapan.Name = "txtKapan";
            this.txtKapan.SelectAllTextOnFocus = true;
            this.txtKapan.Size = new System.Drawing.Size(90, 21);
            this.txtKapan.TabIndex = 35;
            this.txtKapan.ToolTips = "";
            this.txtKapan.WaterMarkText = null;
            // 
            // BtnSearch
            // 
            this.BtnSearch.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnSearch.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnSearch.Appearance.Options.UseFont = true;
            this.BtnSearch.Appearance.Options.UseForeColor = true;
            this.BtnSearch.ImageOptions.Image = global::MahantExport.Properties.Resources.btnsearch;
            this.BtnSearch.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSearch.ImageOptions.SvgImage")));
            this.BtnSearch.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnSearch.Location = new System.Drawing.Point(161, 4);
            this.BtnSearch.Name = "BtnSearch";
            this.BtnSearch.Size = new System.Drawing.Size(168, 37);
            this.BtnSearch.TabIndex = 34;
            this.BtnSearch.Text = "Load Pending Entries";
            this.BtnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(15, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Kapan";
            this.label2.ToolTips = "";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.txtAmount);
            this.panel2.Controls.Add(this.TxtPricePerCarat);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.txtBalanceCarat);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.txtPcs);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.BtnSearch);
            this.panel2.Controls.Add(this.txtKapan);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1176, 48);
            this.panel2.TabIndex = 40;
            // 
            // txtAmount
            // 
            this.txtAmount.ActivationColor = false;
            this.txtAmount.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtAmount.AllowTabKeyOnEnter = false;
            this.txtAmount.BackColor = System.Drawing.Color.White;
            this.txtAmount.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAmount.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAmount.ForeColor = System.Drawing.Color.Green;
            this.txtAmount.Format = "";
            this.txtAmount.IsComplusory = false;
            this.txtAmount.Location = new System.Drawing.Point(1061, 11);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.ReadOnly = true;
            this.txtAmount.SelectAllTextOnFocus = true;
            this.txtAmount.Size = new System.Drawing.Size(110, 24);
            this.txtAmount.TabIndex = 39;
            this.txtAmount.TabStop = false;
            this.txtAmount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAmount.ToolTips = "";
            this.txtAmount.WaterMarkText = null;
            // 
            // TxtPricePerCarat
            // 
            this.TxtPricePerCarat.ActivationColor = false;
            this.TxtPricePerCarat.ActivationColorCode = System.Drawing.Color.Empty;
            this.TxtPricePerCarat.AllowTabKeyOnEnter = false;
            this.TxtPricePerCarat.BackColor = System.Drawing.Color.White;
            this.TxtPricePerCarat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TxtPricePerCarat.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TxtPricePerCarat.ForeColor = System.Drawing.Color.Green;
            this.TxtPricePerCarat.Format = "";
            this.TxtPricePerCarat.IsComplusory = false;
            this.TxtPricePerCarat.Location = new System.Drawing.Point(916, 11);
            this.TxtPricePerCarat.Name = "TxtPricePerCarat";
            this.TxtPricePerCarat.ReadOnly = true;
            this.TxtPricePerCarat.SelectAllTextOnFocus = true;
            this.TxtPricePerCarat.Size = new System.Drawing.Size(92, 24);
            this.TxtPricePerCarat.TabIndex = 39;
            this.TxtPricePerCarat.TabStop = false;
            this.TxtPricePerCarat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TxtPricePerCarat.ToolTips = "";
            this.TxtPricePerCarat.WaterMarkText = null;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(1013, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(45, 13);
            this.label5.TabIndex = 37;
            this.label5.Text = "Amt $";
            this.label5.ToolTips = "";
            // 
            // txtBalanceCarat
            // 
            this.txtBalanceCarat.ActivationColor = false;
            this.txtBalanceCarat.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtBalanceCarat.AllowTabKeyOnEnter = false;
            this.txtBalanceCarat.BackColor = System.Drawing.Color.White;
            this.txtBalanceCarat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBalanceCarat.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBalanceCarat.ForeColor = System.Drawing.Color.Green;
            this.txtBalanceCarat.Format = "";
            this.txtBalanceCarat.IsComplusory = false;
            this.txtBalanceCarat.Location = new System.Drawing.Point(771, 11);
            this.txtBalanceCarat.Name = "txtBalanceCarat";
            this.txtBalanceCarat.ReadOnly = true;
            this.txtBalanceCarat.SelectAllTextOnFocus = true;
            this.txtBalanceCarat.Size = new System.Drawing.Size(92, 24);
            this.txtBalanceCarat.TabIndex = 39;
            this.txtBalanceCarat.TabStop = false;
            this.txtBalanceCarat.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtBalanceCarat.ToolTips = "";
            this.txtBalanceCarat.WaterMarkText = null;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(868, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(43, 13);
            this.label1.TabIndex = 37;
            this.label1.Text = "$/Cts";
            this.label1.ToolTips = "";
            // 
            // txtPcs
            // 
            this.txtPcs.ActivationColor = false;
            this.txtPcs.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtPcs.AllowTabKeyOnEnter = false;
            this.txtPcs.BackColor = System.Drawing.Color.White;
            this.txtPcs.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPcs.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPcs.Format = "";
            this.txtPcs.IsComplusory = false;
            this.txtPcs.Location = new System.Drawing.Point(653, 11);
            this.txtPcs.Name = "txtPcs";
            this.txtPcs.ReadOnly = true;
            this.txtPcs.SelectAllTextOnFocus = true;
            this.txtPcs.Size = new System.Drawing.Size(66, 24);
            this.txtPcs.TabIndex = 40;
            this.txtPcs.TabStop = false;
            this.txtPcs.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtPcs.ToolTips = "";
            this.txtPcs.WaterMarkText = null;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(723, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 37;
            this.label7.Text = "Carat";
            this.label7.ToolTips = "";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.label10.Location = new System.Drawing.Point(580, 17);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(67, 13);
            this.label10.TabIndex = 38;
            this.label10.Text = "Selected ";
            this.label10.ToolTips = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(307, 17);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Transfer No";
            this.label4.ToolTips = "";
            // 
            // txtBombayTransferNo
            // 
            this.txtBombayTransferNo.ActivationColor = false;
            this.txtBombayTransferNo.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtBombayTransferNo.AllowTabKeyOnEnter = false;
            this.txtBombayTransferNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBombayTransferNo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBombayTransferNo.Format = "";
            this.txtBombayTransferNo.IsComplusory = false;
            this.txtBombayTransferNo.Location = new System.Drawing.Point(395, 13);
            this.txtBombayTransferNo.Name = "txtBombayTransferNo";
            this.txtBombayTransferNo.SelectAllTextOnFocus = true;
            this.txtBombayTransferNo.Size = new System.Drawing.Size(150, 21);
            this.txtBombayTransferNo.TabIndex = 35;
            this.txtBombayTransferNo.ToolTips = "";
            this.txtBombayTransferNo.WaterMarkText = null;
            this.txtBombayTransferNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBombayTransferNo_KeyPress);
            // 
            // MainGridDetail
            // 
            this.MainGridDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGridDetail.Location = new System.Drawing.Point(0, 48);
            this.MainGridDetail.MainView = this.GrdDetDetail;
            this.MainGridDetail.Name = "MainGridDetail";
            this.MainGridDetail.Size = new System.Drawing.Size(1176, 405);
            this.MainGridDetail.TabIndex = 44;
            this.MainGridDetail.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GrdDetDetail});
            // 
            // GrdDetDetail
            // 
            this.GrdDetDetail.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.GrdDetDetail.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.GrdDetDetail.Appearance.FocusedCell.Font = new System.Drawing.Font("Verdana", 8F);
            this.GrdDetDetail.Appearance.FocusedCell.Options.UseBackColor = true;
            this.GrdDetDetail.Appearance.FocusedCell.Options.UseFont = true;
            this.GrdDetDetail.Appearance.FocusedRow.Font = new System.Drawing.Font("Verdana", 8F);
            this.GrdDetDetail.Appearance.FocusedRow.Options.UseFont = true;
            this.GrdDetDetail.Appearance.FooterPanel.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.GrdDetDetail.Appearance.FooterPanel.Options.UseFont = true;
            this.GrdDetDetail.Appearance.GroupFooter.BackColor = System.Drawing.Color.Gray;
            this.GrdDetDetail.Appearance.GroupFooter.BackColor2 = System.Drawing.Color.Gray;
            this.GrdDetDetail.Appearance.GroupFooter.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.GrdDetDetail.Appearance.GroupFooter.Options.UseBackColor = true;
            this.GrdDetDetail.Appearance.GroupFooter.Options.UseFont = true;
            this.GrdDetDetail.Appearance.GroupRow.BackColor = System.Drawing.Color.WhiteSmoke;
            this.GrdDetDetail.Appearance.GroupRow.BackColor2 = System.Drawing.Color.WhiteSmoke;
            this.GrdDetDetail.Appearance.GroupRow.ForeColor = System.Drawing.Color.MediumBlue;
            this.GrdDetDetail.Appearance.GroupRow.Options.UseBackColor = true;
            this.GrdDetDetail.Appearance.GroupRow.Options.UseForeColor = true;
            this.GrdDetDetail.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.GrdDetDetail.Appearance.HeaderPanel.Options.UseFont = true;
            this.GrdDetDetail.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.GrdDetDetail.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDetDetail.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.GrdDetDetail.Appearance.HorzLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDetDetail.Appearance.HorzLine.Options.UseBackColor = true;
            this.GrdDetDetail.Appearance.Row.Font = new System.Drawing.Font("Verdana", 8F);
            this.GrdDetDetail.Appearance.Row.Options.UseFont = true;
            this.GrdDetDetail.Appearance.Row.Options.UseTextOptions = true;
            this.GrdDetDetail.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDetDetail.Appearance.VertLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDetDetail.Appearance.VertLine.Options.UseBackColor = true;
            this.GrdDetDetail.AppearancePrint.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 13F);
            this.GrdDetDetail.AppearancePrint.HeaderPanel.Options.UseFont = true;
            this.GrdDetDetail.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn3,
            this.gridColumn4,
            this.gridColumn6,
            this.gridColumn9,
            this.gridColumn11,
            this.gridColumn16,
            this.gridColumn17,
            this.gridColumn18,
            this.gridColumn19,
            this.gridColumn20,
            this.gridColumn21,
            this.gridColumn23,
            this.gridColumn24,
            this.gridColumn25,
            this.gridColumn29,
            this.gridColumn30,
            this.gridColumn31,
            this.gridColumn32,
            this.gridColumn33,
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn5});
            this.GrdDetDetail.GridControl = this.MainGridDetail;
            this.GrdDetDetail.GroupFormat = "{1} {2}";
            this.GrdDetDetail.Name = "GrdDetDetail";
            this.GrdDetDetail.OptionsBehavior.Editable = false;
            this.GrdDetDetail.OptionsCustomization.AllowSort = false;
            this.GrdDetDetail.OptionsFilter.AllowFilterEditor = false;
            this.GrdDetDetail.OptionsPrint.ExpandAllGroups = false;
            this.GrdDetDetail.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.GrdDetDetail.OptionsView.ColumnAutoWidth = false;
            this.GrdDetDetail.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.GrdDetDetail.OptionsView.ShowAutoFilterRow = true;
            this.GrdDetDetail.OptionsView.ShowFooter = true;
            this.GrdDetDetail.CustomSummaryCalculate += new DevExpress.Data.CustomSummaryEventHandler(this.GrdDetDetail_CustomSummaryCalculate);
            this.GrdDetDetail.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GrdDetDetail_MouseUp);
            // 
            // gridColumn3
            // 
            this.gridColumn3.Caption = "CLARITYASSORT_ID";
            this.gridColumn3.FieldName = "CLARITYASSORT_ID";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // gridColumn4
            // 
            this.gridColumn4.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn4.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn4.Caption = "SIZEASSORT_ID";
            this.gridColumn4.FieldName = "SIZEASSORT_ID";
            this.gridColumn4.Name = "gridColumn4";
            this.gridColumn4.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // gridColumn6
            // 
            this.gridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn6.Caption = "INWARD_ID";
            this.gridColumn6.FieldName = "INWARD_ID";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn6.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn6.Width = 72;
            // 
            // gridColumn9
            // 
            this.gridColumn9.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn9.AppearanceCell.Options.UseFont = true;
            this.gridColumn9.Caption = "Kapan";
            this.gridColumn9.FieldName = "KAPANNAME";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn9.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.gridColumn9.Visible = true;
            this.gridColumn9.VisibleIndex = 0;
            this.gridColumn9.Width = 94;
            // 
            // gridColumn11
            // 
            this.gridColumn11.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn11.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn11.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn11.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn11.Caption = "SHAPE_ID";
            this.gridColumn11.FieldName = "SHAPE_ID";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // gridColumn16
            // 
            this.gridColumn16.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn16.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn16.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn16.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn16.Caption = "Shape";
            this.gridColumn16.FieldName = "SHAPENAME";
            this.gridColumn16.Name = "gridColumn16";
            this.gridColumn16.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn16.Visible = true;
            this.gridColumn16.VisibleIndex = 1;
            // 
            // gridColumn17
            // 
            this.gridColumn17.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn17.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn17.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn17.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn17.Caption = "MIXSIZE_ID";
            this.gridColumn17.FieldName = "MIXSIZE_ID";
            this.gridColumn17.Name = "gridColumn17";
            this.gridColumn17.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn17.Width = 93;
            // 
            // gridColumn18
            // 
            this.gridColumn18.Caption = "Size";
            this.gridColumn18.FieldName = "MIXSIZENAME";
            this.gridColumn18.Name = "gridColumn18";
            this.gridColumn18.OptionsColumn.AllowEdit = false;
            this.gridColumn18.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn18.Visible = true;
            this.gridColumn18.VisibleIndex = 2;
            this.gridColumn18.Width = 100;
            // 
            // gridColumn19
            // 
            this.gridColumn19.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn19.AppearanceCell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.gridColumn19.AppearanceCell.Options.UseFont = true;
            this.gridColumn19.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn19.Caption = "Carat";
            this.gridColumn19.DisplayFormat.FormatString = "{0:N3}";
            this.gridColumn19.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn19.FieldName = "CARAT";
            this.gridColumn19.Name = "gridColumn19";
            this.gridColumn19.OptionsColumn.AllowEdit = false;
            this.gridColumn19.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn19.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum)});
            this.gridColumn19.Visible = true;
            this.gridColumn19.VisibleIndex = 5;
            this.gridColumn19.Width = 76;
            // 
            // gridColumn20
            // 
            this.gridColumn20.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn20.AppearanceCell.ForeColor = System.Drawing.Color.Green;
            this.gridColumn20.AppearanceCell.Options.UseFont = true;
            this.gridColumn20.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn20.Caption = "Amt";
            this.gridColumn20.DisplayFormat.FormatString = "{0:N2}";
            this.gridColumn20.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn20.FieldName = "AMOUNT";
            this.gridColumn20.Name = "gridColumn20";
            this.gridColumn20.OptionsColumn.AllowEdit = false;
            this.gridColumn20.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn20.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "AMOUNT", "{0:N2}")});
            this.gridColumn20.Visible = true;
            this.gridColumn20.VisibleIndex = 7;
            this.gridColumn20.Width = 78;
            // 
            // gridColumn21
            // 
            this.gridColumn21.Caption = "$/Cts";
            this.gridColumn21.DisplayFormat.FormatString = "{0:N2}";
            this.gridColumn21.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.gridColumn21.FieldName = "PRICEPERCARAT";
            this.gridColumn21.Name = "gridColumn21";
            this.gridColumn21.OptionsColumn.AllowEdit = false;
            this.gridColumn21.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn21.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Custom, "PRICEPERCARAT", "{0:N2}")});
            this.gridColumn21.Visible = true;
            this.gridColumn21.VisibleIndex = 6;
            // 
            // gridColumn23
            // 
            this.gridColumn23.Caption = "TRANSFER_ID";
            this.gridColumn23.FieldName = "TRANSFER_ID";
            this.gridColumn23.Name = "gridColumn23";
            this.gridColumn23.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn23.Width = 100;
            // 
            // gridColumn24
            // 
            this.gridColumn24.Caption = "MIXCLARITY_ID";
            this.gridColumn24.FieldName = "MIXCLARITY_ID";
            this.gridColumn24.Name = "gridColumn24";
            this.gridColumn24.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // gridColumn25
            // 
            this.gridColumn25.Caption = "Clarity";
            this.gridColumn25.FieldName = "MIXCLARITYNAME";
            this.gridColumn25.Name = "gridColumn25";
            this.gridColumn25.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn25.Visible = true;
            this.gridColumn25.VisibleIndex = 3;
            this.gridColumn25.Width = 100;
            // 
            // gridColumn29
            // 
            this.gridColumn29.Caption = "Assortment Date";
            this.gridColumn29.DisplayFormat.FormatString = "dd/MM/yyyy hh:mm:ss tt";
            this.gridColumn29.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
            this.gridColumn29.FieldName = "ENTRYDATE";
            this.gridColumn29.Name = "gridColumn29";
            this.gridColumn29.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn29.Visible = true;
            this.gridColumn29.VisibleIndex = 8;
            this.gridColumn29.Width = 177;
            // 
            // gridColumn30
            // 
            this.gridColumn30.Caption = "Bombay Transfer Date";
            this.gridColumn30.FieldName = "TRANSFERDATE";
            this.gridColumn30.Name = "gridColumn30";
            this.gridColumn30.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn30.Visible = true;
            this.gridColumn30.VisibleIndex = 9;
            this.gridColumn30.Width = 166;
            // 
            // gridColumn31
            // 
            this.gridColumn31.Caption = "TRANSFER_ID";
            this.gridColumn31.FieldName = "TRANSFER_ID";
            this.gridColumn31.Name = "gridColumn31";
            this.gridColumn31.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // gridColumn32
            // 
            this.gridColumn32.Caption = "Group ID";
            this.gridColumn32.FieldName = "GROUP_ID";
            this.gridColumn32.Name = "gridColumn32";
            this.gridColumn32.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // gridColumn33
            // 
            this.gridColumn33.Caption = "Sr.";
            this.gridColumn33.FieldName = "RNO";
            this.gridColumn33.Name = "gridColumn33";
            this.gridColumn33.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn33.Width = 51;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "Group Name";
            this.gridColumn1.FieldName = "GROUPNAME";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 10;
            this.gridColumn1.Width = 123;
            // 
            // gridColumn2
            // 
            this.gridColumn2.Caption = "Department ID";
            this.gridColumn2.FieldName = "DEPARTMENT_ID";
            this.gridColumn2.Name = "gridColumn2";
            // 
            // gridColumn5
            // 
            this.gridColumn5.Caption = "Department";
            this.gridColumn5.FieldName = "DEPARTMENTNAME";
            this.gridColumn5.Name = "gridColumn5";
            this.gridColumn5.Visible = true;
            this.gridColumn5.VisibleIndex = 4;
            this.gridColumn5.Width = 134;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel5);
            this.panel4.Controls.Add(this.BtnDelete);
            this.panel4.Controls.Add(this.BtnSave);
            this.panel4.Controls.Add(this.BtnClear);
            this.panel4.Controls.Add(this.BtnBack);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(0, 453);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1176, 47);
            this.panel4.TabIndex = 45;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.DTPToDate);
            this.panel5.Controls.Add(this.txtBombayTransferNo);
            this.panel5.Controls.Add(this.DTPFromDate);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel5.Location = new System.Drawing.Point(614, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(562, 47);
            this.panel5.TabIndex = 40;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(6, 17);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 13);
            this.label6.TabIndex = 36;
            this.label6.Text = "From Date";
            this.label6.ToolTips = "";
            // 
            // DTPToDate
            // 
            this.DTPToDate.Font = new System.Drawing.Font("Verdana", 9F);
            this.DTPToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTPToDate.Location = new System.Drawing.Point(199, 12);
            this.DTPToDate.Name = "DTPToDate";
            this.DTPToDate.Size = new System.Drawing.Size(105, 22);
            this.DTPToDate.TabIndex = 39;
            // 
            // DTPFromDate
            // 
            this.DTPFromDate.Font = new System.Drawing.Font("Verdana", 9F);
            this.DTPFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTPFromDate.Location = new System.Drawing.Point(88, 12);
            this.DTPFromDate.Name = "DTPFromDate";
            this.DTPFromDate.Size = new System.Drawing.Size(105, 22);
            this.DTPFromDate.TabIndex = 37;
            // 
            // FrmBombayTransfer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 500);
            this.Controls.Add(this.MainGridDetail);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.LookAndFeel.SkinName = "London Liquid Sky";
            this.LookAndFeel.UseDefaultLookAndFeel =true;
            this.Name = "FrmBombayTransfer";
            this.Text = "BOMBAY TRANSFER";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGridDetail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDetDetail)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private DevControlLib.cDevSimpleButton BtnClear;
        private DevControlLib.cDevSimpleButton BtnSave;
        private DevControlLib.cDevSimpleButton BtnBack;
        private DevControlLib.cDevSimpleButton BtnDelete;
        private AxonContLib.cTextBox txtKapan;
        private DevControlLib.cDevSimpleButton BtnSearch;
        private AxonContLib.cLabel label2;
        private AxonContLib.cPanel panel2;
        private DevExpress.XtraGrid.GridControl MainGridDetail;
        private DevExpress.XtraGrid.Views.Grid.GridView GrdDetDetail;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn16;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn17;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn18;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn19;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn20;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn21;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn23;
        private AxonContLib.cLabel label4;
        private AxonContLib.cTextBox txtBombayTransferNo;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn24;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn25;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn29;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn30;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn31;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn32;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn33;
        private AxonContLib.cPanel panel4;
        private System.Windows.Forms.DateTimePicker DTPToDate;
        private System.Windows.Forms.DateTimePicker DTPFromDate;
        private AxonContLib.cLabel label6;
        private AxonContLib.cPanel panel5;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private AxonContLib.cTextBox txtBalanceCarat;
        private AxonContLib.cTextBox txtPcs;
        private AxonContLib.cLabel label7;
        private AxonContLib.cLabel label10;
        private AxonContLib.cTextBox txtAmount;
        private AxonContLib.cTextBox TxtPricePerCarat;
        private AxonContLib.cLabel label5;
        private AxonContLib.cLabel label1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
	}
}