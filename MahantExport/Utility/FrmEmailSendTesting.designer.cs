using HTMLEditorControl;
namespace MahantExport
{
    partial class FrmEmailSendTesting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEmailSendTesting));
            this.txtToEmail = new System.Windows.Forms.TextBox();
            this.panel3 = new AxonContLib.cPanel(this.components);
            this.BtnNotification = new DevControlLib.cDevSimpleButton(this.components);
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
            this.panel2 = new AxonContLib.cPanel(this.components);
            this.ChkEnableSSL = new DevExpress.XtraEditors.CheckEdit();
            this.txtSMTPPort = new System.Windows.Forms.TextBox();
            this.cLabel9 = new AxonContLib.cLabel(this.components);
            this.txtSMTPHost = new System.Windows.Forms.TextBox();
            this.cLabel8 = new AxonContLib.cLabel(this.components);
            this.txtSMTPPassword = new System.Windows.Forms.TextBox();
            this.cLabel7 = new AxonContLib.cLabel(this.components);
            this.cLabel6 = new AxonContLib.cLabel(this.components);
            this.txtSMTPUserName = new System.Windows.Forms.TextBox();
            this.cLabel5 = new AxonContLib.cLabel(this.components);
            this.BtnBrowse = new DevControlLib.cDevSimpleButton(this.components);
            this.cLabel4 = new AxonContLib.cLabel(this.components);
            this.txtAttachment = new System.Windows.Forms.TextBox();
            this.panel4 = new AxonContLib.cPanel(this.components);
            this.panel5 = new AxonContLib.cPanel(this.components);
            this.panel3.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChkEnableSSL.Properties)).BeginInit();
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
            this.panel3.Controls.Add(this.BtnNotification);
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
            // BtnNotification
            // 
            this.BtnNotification.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnNotification.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnNotification.Appearance.Options.UseFont = true;
            this.BtnNotification.Appearance.Options.UseForeColor = true;
            this.BtnNotification.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnNotification.ImageOptions.SvgImage")));
            this.BtnNotification.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnNotification.Location = new System.Drawing.Point(424, 6);
            this.BtnNotification.Name = "BtnNotification";
            this.BtnNotification.Size = new System.Drawing.Size(116, 31);
            this.BtnNotification.TabIndex = 4;
            this.BtnNotification.Text = "Notification";
            this.BtnNotification.Click += new System.EventHandler(this.BtnNotification_Click);
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.BtnSendBulkEmail);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel6.Location = new System.Drawing.Point(892, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(137, 45);
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
            this.BtnSendBulkEmail.Size = new System.Drawing.Size(132, 31);
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
            this.htmlEditor.Location = new System.Drawing.Point(10, 159);
            this.htmlEditor.Name = "htmlEditor";
            this.htmlEditor.Size = new System.Drawing.Size(1009, 318);
            this.htmlEditor.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.ChkEnableSSL);
            this.panel2.Controls.Add(this.txtSMTPPort);
            this.panel2.Controls.Add(this.cLabel9);
            this.panel2.Controls.Add(this.txtSMTPHost);
            this.panel2.Controls.Add(this.cLabel8);
            this.panel2.Controls.Add(this.txtSMTPPassword);
            this.panel2.Controls.Add(this.cLabel7);
            this.panel2.Controls.Add(this.cLabel6);
            this.panel2.Controls.Add(this.txtSMTPUserName);
            this.panel2.Controls.Add(this.cLabel5);
            this.panel2.Controls.Add(this.BtnBrowse);
            this.panel2.Controls.Add(this.lblInvoiceNo);
            this.panel2.Controls.Add(this.txtToEmail);
            this.panel2.Controls.Add(this.txtCCEmail);
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
            this.panel2.Size = new System.Drawing.Size(1029, 159);
            this.panel2.TabIndex = 0;
            // 
            // ChkEnableSSL
            // 
            this.ChkEnableSSL.Location = new System.Drawing.Point(698, 132);
            this.ChkEnableSSL.Name = "ChkEnableSSL";
            this.ChkEnableSSL.Properties.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.ChkEnableSSL.Properties.Appearance.Options.UseFont = true;
            this.ChkEnableSSL.Properties.Caption = "  Enable SSL";
            this.ChkEnableSSL.Properties.CheckStyle = DevExpress.XtraEditors.Controls.CheckStyles.UserDefined;
            this.ChkEnableSSL.Properties.ImageOptions.ImageChecked = global::MahantExport.Properties.Resources.Checked;
            this.ChkEnableSSL.Properties.ImageOptions.ImageUnchecked = global::MahantExport.Properties.Resources.Unchecked;
            this.ChkEnableSSL.Size = new System.Drawing.Size(137, 20);
            this.ChkEnableSSL.TabIndex = 18;
            // 
            // txtSMTPPort
            // 
            this.txtSMTPPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSMTPPort.Font = new System.Drawing.Font("Verdana", 10F);
            this.txtSMTPPort.Location = new System.Drawing.Point(699, 104);
            this.txtSMTPPort.Name = "txtSMTPPort";
            this.txtSMTPPort.Size = new System.Drawing.Size(302, 24);
            this.txtSMTPPort.TabIndex = 17;
            // 
            // cLabel9
            // 
            this.cLabel9.AutoSize = true;
            this.cLabel9.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel9.ForeColor = System.Drawing.Color.Black;
            this.cLabel9.Location = new System.Drawing.Point(624, 108);
            this.cLabel9.Name = "cLabel9";
            this.cLabel9.Size = new System.Drawing.Size(34, 13);
            this.cLabel9.TabIndex = 16;
            this.cLabel9.Text = "Port";
            this.cLabel9.ToolTips = "";
            // 
            // txtSMTPHost
            // 
            this.txtSMTPHost.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSMTPHost.Font = new System.Drawing.Font("Verdana", 10F);
            this.txtSMTPHost.Location = new System.Drawing.Point(699, 75);
            this.txtSMTPHost.Name = "txtSMTPHost";
            this.txtSMTPHost.Size = new System.Drawing.Size(302, 24);
            this.txtSMTPHost.TabIndex = 15;
            // 
            // cLabel8
            // 
            this.cLabel8.AutoSize = true;
            this.cLabel8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel8.ForeColor = System.Drawing.Color.Black;
            this.cLabel8.Location = new System.Drawing.Point(624, 79);
            this.cLabel8.Name = "cLabel8";
            this.cLabel8.Size = new System.Drawing.Size(36, 13);
            this.cLabel8.TabIndex = 14;
            this.cLabel8.Text = "Host";
            this.cLabel8.ToolTips = "";
            // 
            // txtSMTPPassword
            // 
            this.txtSMTPPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSMTPPassword.Font = new System.Drawing.Font("Verdana", 10F);
            this.txtSMTPPassword.Location = new System.Drawing.Point(699, 47);
            this.txtSMTPPassword.Name = "txtSMTPPassword";
            this.txtSMTPPassword.Size = new System.Drawing.Size(302, 24);
            this.txtSMTPPassword.TabIndex = 13;
            // 
            // cLabel7
            // 
            this.cLabel7.AutoSize = true;
            this.cLabel7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel7.ForeColor = System.Drawing.Color.Black;
            this.cLabel7.Location = new System.Drawing.Point(624, 51);
            this.cLabel7.Name = "cLabel7";
            this.cLabel7.Size = new System.Drawing.Size(69, 13);
            this.cLabel7.TabIndex = 12;
            this.cLabel7.Text = "Password";
            this.cLabel7.ToolTips = "";
            // 
            // cLabel6
            // 
            this.cLabel6.AutoSize = true;
            this.cLabel6.Font = new System.Drawing.Font("Verdana", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel6.ForeColor = System.Drawing.Color.Black;
            this.cLabel6.Location = new System.Drawing.Point(742, 2);
            this.cLabel6.Name = "cLabel6";
            this.cLabel6.Size = new System.Drawing.Size(151, 13);
            this.cLabel6.TabIndex = 11;
            this.cLabel6.Text = "--: SMTP Credential :--";
            this.cLabel6.ToolTips = "";
            // 
            // txtSMTPUserName
            // 
            this.txtSMTPUserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSMTPUserName.Font = new System.Drawing.Font("Verdana", 10F);
            this.txtSMTPUserName.Location = new System.Drawing.Point(699, 19);
            this.txtSMTPUserName.Name = "txtSMTPUserName";
            this.txtSMTPUserName.Size = new System.Drawing.Size(302, 24);
            this.txtSMTPUserName.TabIndex = 10;
            // 
            // cLabel5
            // 
            this.cLabel5.AutoSize = true;
            this.cLabel5.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel5.ForeColor = System.Drawing.Color.Black;
            this.cLabel5.Location = new System.Drawing.Point(624, 23);
            this.cLabel5.Name = "cLabel5";
            this.cLabel5.Size = new System.Drawing.Size(74, 13);
            this.cLabel5.TabIndex = 9;
            this.cLabel5.Text = "UserName";
            this.cLabel5.ToolTips = "";
            // 
            // BtnBrowse
            // 
            this.BtnBrowse.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnBrowse.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnBrowse.Appearance.Options.UseFont = true;
            this.BtnBrowse.Appearance.Options.UseForeColor = true;
            this.BtnBrowse.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnBrowse.ImageOptions.SvgImage")));
            this.BtnBrowse.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnBrowse.Location = new System.Drawing.Point(528, 123);
            this.BtnBrowse.Name = "BtnBrowse";
            this.BtnBrowse.Size = new System.Drawing.Size(78, 26);
            this.BtnBrowse.TabIndex = 5;
            this.BtnBrowse.Text = "Browse";
            this.BtnBrowse.Click += new System.EventHandler(this.BtnBrowse_Click);
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
            this.panel4.Location = new System.Drawing.Point(1019, 159);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(10, 318);
            this.panel4.TabIndex = 8;
            // 
            // panel5
            // 
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(0, 159);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(10, 318);
            this.panel5.TabIndex = 9;
            // 
            // FrmEmailSendTesting
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1029, 522);
            this.Controls.Add(this.htmlEditor);
            this.Controls.Add(this.panel5);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("FrmEmailSendTesting.IconOptions.Icon")));
            this.KeyPreview = true;
            this.LookAndFeel.SkinName = "London Liquid Sky";
            this.LookAndFeel.UseDefaultLookAndFeel =true;
            this.Name = "FrmEmailSendTesting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "EmailSendTesting";
            this.Text = "EMAIL SEND UTILITY";
            this.Load += new System.EventHandler(this.FrmEmailSend_Load);
            this.panel3.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ChkEnableSSL.Properties)).EndInit();
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
        private AxonContLib.cPanel panel2;
        private AxonContLib.cPanel panel4;
        private AxonContLib.cPanel panel5;
        private AxonContLib.cLabel cLabel4;
        private System.Windows.Forms.TextBox txtAttachment;
        private DevControlLib.cDevSimpleButton BtnBrowse;
        private DevControlLib.cDevSimpleButton BtnExport;
        private AxonContLib.cPanel panel6;
        private DevControlLib.cDevSimpleButton BtnSendBulkEmail;
        private AxonContLib.cLabel cLabel5;
        private System.Windows.Forms.TextBox txtSMTPUserName;
        private AxonContLib.cLabel cLabel6;
        private System.Windows.Forms.TextBox txtSMTPPassword;
        private AxonContLib.cLabel cLabel7;
        private System.Windows.Forms.TextBox txtSMTPHost;
        private AxonContLib.cLabel cLabel8;
        private System.Windows.Forms.TextBox txtSMTPPort;
        private AxonContLib.cLabel cLabel9;
        private DevExpress.XtraEditors.CheckEdit ChkEnableSSL;
        private DevControlLib.cDevSimpleButton BtnNotification;




    }
}