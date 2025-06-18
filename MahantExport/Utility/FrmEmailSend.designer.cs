using HTMLEditorControl;
namespace MahantExport
{
    partial class FrmEmailSend
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEmailSend));
            this.txtToEmail = new System.Windows.Forms.TextBox();
            this.panel3 = new AxonContLib.cPanel(this.components);
            this.panel6 = new AxonContLib.cPanel(this.components);
            this.BtnSendBulkEmail = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnClear = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnSend = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnExport = new DevControlLib.cDevSimpleButton(this.components);
            this.lblInvoiceNo = new AxonContLib.cLabel(this.components);
            this.txtCCEmail = new System.Windows.Forms.TextBox();
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            this.txtBCCEmail = new System.Windows.Forms.TextBox();
            this.cLabel2 = new AxonContLib.cLabel(this.components);
            this.txtSubject = new System.Windows.Forms.TextBox();
            this.cLabel3 = new AxonContLib.cLabel(this.components);
            this.htmlEditor = new HTMLEditorControl.HtmlEditorControl();
            this.panel1 = new AxonContLib.cPanel(this.components);
            this.MainGridMarty = new DevExpress.XtraGrid.GridControl();
            this.GrdDetParty = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.bandedGridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.bandedGridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.bandedGridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.bandedGridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel2 = new AxonContLib.cPanel(this.components);
            this.BtnBrowse = new DevControlLib.cDevSimpleButton(this.components);
            this.lblOpenFile = new AxonContLib.cLabel(this.components);
            this.cLabel4 = new AxonContLib.cLabel(this.components);
            this.txtAttachment = new System.Windows.Forms.TextBox();
            this.panel4 = new AxonContLib.cPanel(this.components);
            this.panel5 = new AxonContLib.cPanel(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGridMarty)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDetParty)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtToEmail
            // 
            this.txtToEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtToEmail.Font = new System.Drawing.Font("Verdana", 10F);
            this.txtToEmail.Location = new System.Drawing.Point(88, 8);
            this.txtToEmail.Name = "txtToEmail";
            this.txtToEmail.Size = new System.Drawing.Size(518, 24);
            this.txtToEmail.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel6);
            this.panel3.Controls.Add(this.BtnClear);
            this.panel3.Controls.Add(this.BtnSend);
            this.panel3.Controls.Add(this.BtnExport);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 477);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1029, 45);
            this.panel3.TabIndex = 2;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.BtnSendBulkEmail);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(871, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(158, 45);
            this.panel6.TabIndex = 3;
            // 
            // BtnSendBulkEmail
            // 
            this.BtnSendBulkEmail.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnSendBulkEmail.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnSendBulkEmail.Appearance.Options.UseFont = true;
            this.BtnSendBulkEmail.Appearance.Options.UseForeColor = true;
            this.BtnSendBulkEmail.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSendBulkEmail.ImageOptions.SvgImage")));
            this.BtnSendBulkEmail.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnSendBulkEmail.Location = new System.Drawing.Point(3, 6);
            this.BtnSendBulkEmail.Name = "BtnSendBulkEmail";
            this.BtnSendBulkEmail.Size = new System.Drawing.Size(143, 31);
            this.BtnSendBulkEmail.TabIndex = 0;
            this.BtnSendBulkEmail.Text = "&Send Bulk Email";
            this.BtnSendBulkEmail.Click += new System.EventHandler(this.BtnSendBulkEmail_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnClear.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnClear.Appearance.Options.UseFont = true;
            this.BtnClear.Appearance.Options.UseForeColor = true;
            this.BtnClear.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnClear.ImageOptions.SvgImage")));
            this.BtnClear.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnClear.Location = new System.Drawing.Point(98, 7);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(85, 31);
            this.BtnClear.TabIndex = 1;
            this.BtnClear.Text = "Clear";
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // BtnSend
            // 
            this.BtnSend.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnSend.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnSend.Appearance.Options.UseFont = true;
            this.BtnSend.Appearance.Options.UseForeColor = true;
            this.BtnSend.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSend.ImageOptions.SvgImage")));
            this.BtnSend.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnSend.Location = new System.Drawing.Point(10, 7);
            this.BtnSend.Name = "BtnSend";
            this.BtnSend.Size = new System.Drawing.Size(82, 31);
            this.BtnSend.TabIndex = 0;
            this.BtnSend.Text = "&Send";
            this.BtnSend.Click += new System.EventHandler(this.BtnSend_Click);
            // 
            // BtnExport
            // 
            this.BtnExport.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnExport.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnExport.Appearance.Options.UseFont = true;
            this.BtnExport.Appearance.Options.UseForeColor = true;
            this.BtnExport.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnExport.ImageOptions.SvgImage")));
            this.BtnExport.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnExport.Location = new System.Drawing.Point(189, 7);
            this.BtnExport.Name = "BtnExport";
            this.BtnExport.Size = new System.Drawing.Size(85, 31);
            this.BtnExport.TabIndex = 2;
            this.BtnExport.Text = "Exit";
            this.BtnExport.Click += new System.EventHandler(this.BtnExport_Click);
            // 
            // lblInvoiceNo
            // 
            this.lblInvoiceNo.AutoSize = true;
            this.lblInvoiceNo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInvoiceNo.ForeColor = System.Drawing.Color.Black;
            this.lblInvoiceNo.Location = new System.Drawing.Point(5, 14);
            this.lblInvoiceNo.Name = "lblInvoiceNo";
            this.lblInvoiceNo.Size = new System.Drawing.Size(63, 13);
            this.lblInvoiceNo.TabIndex = 0;
            this.lblInvoiceNo.Text = "To Email";
            this.lblInvoiceNo.ToolTips = "";
            // 
            // txtCCEmail
            // 
            this.txtCCEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCCEmail.Font = new System.Drawing.Font("Verdana", 10F);
            this.txtCCEmail.Location = new System.Drawing.Point(88, 36);
            this.txtCCEmail.Name = "txtCCEmail";
            this.txtCCEmail.Size = new System.Drawing.Size(518, 24);
            this.txtCCEmail.TabIndex = 2;
            // 
            // cLabel1
            // 
            this.cLabel1.AutoSize = true;
            this.cLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel1.ForeColor = System.Drawing.Color.Black;
            this.cLabel1.Location = new System.Drawing.Point(5, 42);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(62, 13);
            this.cLabel1.TabIndex = 2;
            this.cLabel1.Text = "Cc Email";
            this.cLabel1.ToolTips = "";
            // 
            // txtBCCEmail
            // 
            this.txtBCCEmail.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBCCEmail.Font = new System.Drawing.Font("Verdana", 10F);
            this.txtBCCEmail.Location = new System.Drawing.Point(88, 65);
            this.txtBCCEmail.Name = "txtBCCEmail";
            this.txtBCCEmail.Size = new System.Drawing.Size(518, 24);
            this.txtBCCEmail.TabIndex = 3;
            // 
            // cLabel2
            // 
            this.cLabel2.AutoSize = true;
            this.cLabel2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel2.ForeColor = System.Drawing.Color.Black;
            this.cLabel2.Location = new System.Drawing.Point(5, 71);
            this.cLabel2.Name = "cLabel2";
            this.cLabel2.Size = new System.Drawing.Size(69, 13);
            this.cLabel2.TabIndex = 4;
            this.cLabel2.Text = "Bcc Email";
            this.cLabel2.ToolTips = "";
            // 
            // txtSubject
            // 
            this.txtSubject.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSubject.Font = new System.Drawing.Font("Verdana", 10F);
            this.txtSubject.Location = new System.Drawing.Point(88, 94);
            this.txtSubject.Name = "txtSubject";
            this.txtSubject.Size = new System.Drawing.Size(518, 24);
            this.txtSubject.TabIndex = 4;
            // 
            // cLabel3
            // 
            this.cLabel3.AutoSize = true;
            this.cLabel3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel3.ForeColor = System.Drawing.Color.Black;
            this.cLabel3.Location = new System.Drawing.Point(5, 100);
            this.cLabel3.Name = "cLabel3";
            this.cLabel3.Size = new System.Drawing.Size(56, 13);
            this.cLabel3.TabIndex = 5;
            this.cLabel3.Text = "Subject";
            this.cLabel3.ToolTips = "";
            // 
            // htmlEditor
            // 
            this.htmlEditor.BodyFont = new HTMLEditorControl.HtmlFontProperty("verdana", HTMLEditorControl.HtmlFontSize.Small, false, false, false, false, false, false);
            this.htmlEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.htmlEditor.InnerText = null;
            this.htmlEditor.Location = new System.Drawing.Point(0, 171);
            this.htmlEditor.Name = "htmlEditor";
            this.htmlEditor.Size = new System.Drawing.Size(755, 306);
            this.htmlEditor.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.MainGridMarty);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(250, 477);
            this.panel1.TabIndex = 7;
            // 
            // MainGridMarty
            // 
            this.MainGridMarty.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGridMarty.Location = new System.Drawing.Point(0, 0);
            this.MainGridMarty.MainView = this.GrdDetParty;
            this.MainGridMarty.Name = "MainGridMarty";
            this.MainGridMarty.Size = new System.Drawing.Size(250, 477);
            this.MainGridMarty.TabIndex = 2;
            this.MainGridMarty.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GrdDetParty});
            // 
            // GrdDetParty
            // 
            this.GrdDetParty.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(234)))), ((int)(((byte)(141)))));
            this.GrdDetParty.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(234)))), ((int)(((byte)(141)))));
            this.GrdDetParty.Appearance.FocusedCell.Options.UseBackColor = true;
            this.GrdDetParty.Appearance.FooterPanel.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.GrdDetParty.Appearance.FooterPanel.Options.UseFont = true;
            this.GrdDetParty.Appearance.GroupFooter.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.GrdDetParty.Appearance.GroupFooter.Options.UseFont = true;
            this.GrdDetParty.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.GrdDetParty.Appearance.HeaderPanel.Options.UseFont = true;
            this.GrdDetParty.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.GrdDetParty.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDetParty.Appearance.HorzLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDetParty.Appearance.HorzLine.Options.UseBackColor = true;
            this.GrdDetParty.Appearance.Row.Font = new System.Drawing.Font("Verdana", 9F);
            this.GrdDetParty.Appearance.Row.Options.UseFont = true;
            this.GrdDetParty.Appearance.VertLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDetParty.Appearance.VertLine.Options.UseBackColor = true;
            this.GrdDetParty.AppearancePrint.FooterPanel.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.GrdDetParty.AppearancePrint.FooterPanel.Options.UseFont = true;
            this.GrdDetParty.AppearancePrint.HeaderPanel.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.GrdDetParty.AppearancePrint.HeaderPanel.Options.UseFont = true;
            this.GrdDetParty.ColumnPanelRowHeight = 25;
            this.GrdDetParty.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.bandedGridColumn2,
            this.bandedGridColumn3,
            this.bandedGridColumn6,
            this.bandedGridColumn7,
            this.gridColumn1});
            this.GrdDetParty.GridControl = this.MainGridMarty;
            this.GrdDetParty.Name = "GrdDetParty";
            this.GrdDetParty.OptionsBehavior.Editable = false;
            this.GrdDetParty.OptionsPrint.ExpandAllGroups = false;
            this.GrdDetParty.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.GrdDetParty.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.GrdDetParty.OptionsView.ColumnAutoWidth = false;
            this.GrdDetParty.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.GrdDetParty.OptionsView.ShowAutoFilterRow = true;
            this.GrdDetParty.OptionsView.ShowFooter = true;
            this.GrdDetParty.OptionsView.ShowGroupPanel = false;
            this.GrdDetParty.RowHeight = 22;
            // 
            // bandedGridColumn2
            // 
            this.bandedGridColumn2.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.bandedGridColumn2.AppearanceCell.Options.UseFont = true;
            this.bandedGridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn2.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.bandedGridColumn2.AppearanceHeader.Options.UseFont = true;
            this.bandedGridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.bandedGridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn2.Caption = "Party Name";
            this.bandedGridColumn2.FieldName = "PARTYNAME";
            this.bandedGridColumn2.Name = "bandedGridColumn2";
            this.bandedGridColumn2.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.bandedGridColumn2.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Count)});
            this.bandedGridColumn2.Visible = true;
            this.bandedGridColumn2.VisibleIndex = 1;
            this.bandedGridColumn2.Width = 247;
            // 
            // bandedGridColumn3
            // 
            this.bandedGridColumn3.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.bandedGridColumn3.AppearanceCell.Options.UseFont = true;
            this.bandedGridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn3.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.bandedGridColumn3.AppearanceHeader.Options.UseFont = true;
            this.bandedGridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.bandedGridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn3.Caption = "PARTY_ID";
            this.bandedGridColumn3.FieldName = "PARTY_ID";
            this.bandedGridColumn3.Name = "bandedGridColumn3";
            this.bandedGridColumn3.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            // 
            // bandedGridColumn6
            // 
            this.bandedGridColumn6.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.bandedGridColumn6.AppearanceCell.Options.UseFont = true;
            this.bandedGridColumn6.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn6.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn6.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.bandedGridColumn6.AppearanceHeader.Options.UseFont = true;
            this.bandedGridColumn6.AppearanceHeader.Options.UseTextOptions = true;
            this.bandedGridColumn6.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn6.Caption = "Company";
            this.bandedGridColumn6.FieldName = "COMPANYNAME";
            this.bandedGridColumn6.Name = "bandedGridColumn6";
            this.bandedGridColumn6.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.bandedGridColumn6.Visible = true;
            this.bandedGridColumn6.VisibleIndex = 2;
            this.bandedGridColumn6.Width = 150;
            // 
            // bandedGridColumn7
            // 
            this.bandedGridColumn7.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.bandedGridColumn7.AppearanceCell.Options.UseFont = true;
            this.bandedGridColumn7.AppearanceCell.Options.UseTextOptions = true;
            this.bandedGridColumn7.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn7.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.bandedGridColumn7.AppearanceHeader.Options.UseFont = true;
            this.bandedGridColumn7.AppearanceHeader.Options.UseTextOptions = true;
            this.bandedGridColumn7.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.bandedGridColumn7.Caption = "Email ID";
            this.bandedGridColumn7.FieldName = "EMAILID";
            this.bandedGridColumn7.Name = "bandedGridColumn7";
            this.bandedGridColumn7.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.bandedGridColumn7.Visible = true;
            this.bandedGridColumn7.VisibleIndex = 3;
            this.bandedGridColumn7.Width = 150;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F);
            this.gridColumn1.AppearanceCell.Options.UseFont = true;
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.gridColumn1.AppearanceHeader.Options.UseFont = true;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "Email Group";
            this.gridColumn1.FieldName = "EMAILGROUP";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.OptionsColumn.AllowEdit = false;
            this.gridColumn1.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 0;
            this.gridColumn1.Width = 136;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.BtnBrowse);
            this.panel2.Controls.Add(this.lblInvoiceNo);
            this.panel2.Controls.Add(this.txtToEmail);
            this.panel2.Controls.Add(this.txtCCEmail);
            this.panel2.Controls.Add(this.lblOpenFile);
            this.panel2.Controls.Add(this.cLabel4);
            this.panel2.Controls.Add(this.cLabel3);
            this.panel2.Controls.Add(this.txtBCCEmail);
            this.panel2.Controls.Add(this.cLabel2);
            this.panel2.Controls.Add(this.txtAttachment);
            this.panel2.Controls.Add(this.txtSubject);
            this.panel2.Controls.Add(this.cLabel1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(755, 171);
            this.panel2.TabIndex = 0;
            // 
            // BtnBrowse
            // 
            this.BtnBrowse.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnBrowse.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnBrowse.Appearance.Options.UseFont = true;
            this.BtnBrowse.Appearance.Options.UseForeColor = true;
            this.BtnBrowse.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnBrowse.ImageOptions.SvgImage")));
            this.BtnBrowse.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnBrowse.Location = new System.Drawing.Point(529, 123);
            this.BtnBrowse.Name = "BtnBrowse";
            this.BtnBrowse.Size = new System.Drawing.Size(79, 26);
            this.BtnBrowse.TabIndex = 5;
            this.BtnBrowse.Text = "Browse";
            this.BtnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
            // 
            // lblOpenFile
            // 
            this.lblOpenFile.AutoSize = true;
            this.lblOpenFile.Font = new System.Drawing.Font("Verdana", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOpenFile.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblOpenFile.Location = new System.Drawing.Point(457, 151);
            this.lblOpenFile.Name = "lblOpenFile";
            this.lblOpenFile.Size = new System.Drawing.Size(68, 13);
            this.lblOpenFile.TabIndex = 7;
            this.lblOpenFile.Text = "Open File";
            this.lblOpenFile.ToolTips = "";
            this.lblOpenFile.Click += new System.EventHandler(this.lblOpenFile_Click);
            // 
            // cLabel4
            // 
            this.cLabel4.AutoSize = true;
            this.cLabel4.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel4.ForeColor = System.Drawing.Color.Black;
            this.cLabel4.Location = new System.Drawing.Point(5, 130);
            this.cLabel4.Name = "cLabel4";
            this.cLabel4.Size = new System.Drawing.Size(82, 13);
            this.cLabel4.TabIndex = 7;
            this.cLabel4.Text = "Attachment";
            this.cLabel4.ToolTips = "";
            // 
            // txtAttachment
            // 
            this.txtAttachment.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAttachment.Enabled = false;
            this.txtAttachment.Font = new System.Drawing.Font("Verdana", 10F);
            this.txtAttachment.Location = new System.Drawing.Point(88, 124);
            this.txtAttachment.Name = "txtAttachment";
            this.txtAttachment.ReadOnly = true;
            this.txtAttachment.Size = new System.Drawing.Size(437, 24);
            this.txtAttachment.TabIndex = 8;
            // 
            // panel4
            // 
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(1019, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(10, 477);
            this.panel4.TabIndex = 8;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(10, 477);
            this.panel5.TabIndex = 9;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(10, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.htmlEditor);
            this.splitContainer1.Panel1.Controls.Add(this.panel2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(1009, 477);
            this.splitContainer1.SplitterDistance = 755;
            this.splitContainer1.TabIndex = 10;
            // 
            // FrmEmailSend
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 522);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("FrmEmailSend.IconOptions.Icon")));
            this.KeyPreview = true;
            this.Name = "FrmEmailSend";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "FrmEmailSend";
            this.Text = "EMAIL SEND UTILITY";
            this.Load += new System.EventHandler(this.FrmEmailSend_Load);
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainGridMarty)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDetParty)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtToEmail;
        private AxonContLib.cPanel panel3;
        private DevControlLib.cDevSimpleButton BtnClear;
        private DevControlLib.cDevSimpleButton BtnSend;
        private AxonContLib.cLabel lblInvoiceNo;
        private System.Windows.Forms.TextBox txtCCEmail;
        private AxonContLib.cLabel cLabel1;
        private System.Windows.Forms.TextBox txtBCCEmail;
        private AxonContLib.cLabel cLabel2;
        private System.Windows.Forms.TextBox txtSubject;
        private AxonContLib.cLabel cLabel3;
        private HtmlEditorControl htmlEditor;
        private AxonContLib.cPanel panel1;
        private AxonContLib.cPanel panel2;
        private AxonContLib.cPanel panel4;
        private AxonContLib.cPanel panel5;
        private DevExpress.XtraGrid.GridControl MainGridMarty;
        private AxonContLib.cLabel cLabel4;
        private System.Windows.Forms.TextBox txtAttachment;
        private DevControlLib.cDevSimpleButton BtnBrowse;
        private DevControlLib.cDevSimpleButton BtnExport;
        private AxonContLib.cPanel panel6;
        private DevControlLib.cDevSimpleButton BtnSendBulkEmail;
        private DevExpress.XtraGrid.Views.Grid.GridView GrdDetParty;
        private DevExpress.XtraGrid.Columns.GridColumn bandedGridColumn2;
        private DevExpress.XtraGrid.Columns.GridColumn bandedGridColumn3;
        private DevExpress.XtraGrid.Columns.GridColumn bandedGridColumn6;
        private DevExpress.XtraGrid.Columns.GridColumn bandedGridColumn7;
        private AxonContLib.cLabel lblOpenFile;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
    }
}