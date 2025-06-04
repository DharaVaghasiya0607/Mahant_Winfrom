namespace MahantExport.Report
{
    partial class FrmRegisterPrint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRegisterPrint));
            this.groupControl1 = new DevControlLib.cDevGroupControl(this.components);
            this.CmbReportType = new System.Windows.Forms.ComboBox();
            this.cLabel3 = new AxonContLib.cLabel(this.components);
            this.groupBox1 = new AxonContLib.cGroupBox(this.components);
            this.CmbAccountType = new System.Windows.Forms.ComboBox();
            this.CmbChkLedger = new DevControlLib.cDevCheckedComboBoxEdit(this.components);
            this.CmbStoneType = new System.Windows.Forms.ComboBox();
            this.cLabel8 = new AxonContLib.cLabel(this.components);
            this.cLabel7 = new AxonContLib.cLabel(this.components);
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            this.BtnYear = new DevControlLib.cDevSimpleButton(this.components);
            this.btnMonth = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnToday = new DevControlLib.cDevSimpleButton(this.components);
            this.LblToDate = new AxonContLib.cLabel(this.components);
            this.DTPToDate = new AxonContLib.cDateTimePicker(this.components);
            this.LblFromDate = new AxonContLib.cLabel(this.components);
            this.DTPFromDate = new AxonContLib.cDateTimePicker(this.components);
            this.panel3 = new AxonContLib.cPanel(this.components);
            this.BtnReset = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnPrint = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnPreview = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnClose = new DevControlLib.cDevSimpleButton(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbChkLedger.Properties)).BeginInit();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.groupControl1.Controls.Add(this.CmbReportType);
            this.groupControl1.Controls.Add(this.cLabel3);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(630, 50);
            this.groupControl1.TabIndex = 1;
            this.groupControl1.Text = "Report";
            // 
            // CmbReportType
            // 
            this.CmbReportType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbReportType.FormattingEnabled = true;
            this.CmbReportType.Items.AddRange(new object[] {
            "ExportRegister",
            "ImportRegister",
            "LocalRegister "});
            this.CmbReportType.Location = new System.Drawing.Point(339, 24);
            this.CmbReportType.Name = "CmbReportType";
            this.CmbReportType.Size = new System.Drawing.Size(295, 21);
            this.CmbReportType.TabIndex = 196;
            // 
            // cLabel3
            // 
            this.cLabel3.AutoSize = true;
            this.cLabel3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel3.ForeColor = System.Drawing.Color.Black;
            this.cLabel3.Location = new System.Drawing.Point(11, 28);
            this.cLabel3.Name = "cLabel3";
            this.cLabel3.Size = new System.Drawing.Size(86, 13);
            this.cLabel3.TabIndex = 195;
            this.cLabel3.Text = "Report Type";
            this.cLabel3.ToolTips = "";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CmbAccountType);
            this.groupBox1.Controls.Add(this.CmbChkLedger);
            this.groupBox1.Controls.Add(this.CmbStoneType);
            this.groupBox1.Controls.Add(this.cLabel8);
            this.groupBox1.Controls.Add(this.cLabel7);
            this.groupBox1.Controls.Add(this.cLabel1);
            this.groupBox1.Controls.Add(this.BtnYear);
            this.groupBox1.Controls.Add(this.btnMonth);
            this.groupBox1.Controls.Add(this.BtnToday);
            this.groupBox1.Controls.Add(this.LblToDate);
            this.groupBox1.Controls.Add(this.DTPToDate);
            this.groupBox1.Controls.Add(this.LblFromDate);
            this.groupBox1.Controls.Add(this.DTPFromDate);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(0, 50);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(630, 151);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select Criteria";
            // 
            // CmbAccountType
            // 
            this.CmbAccountType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbAccountType.FormattingEnabled = true;
            this.CmbAccountType.Location = new System.Drawing.Point(88, 72);
            this.CmbAccountType.Name = "CmbAccountType";
            this.CmbAccountType.Size = new System.Drawing.Size(302, 22);
            this.CmbAccountType.TabIndex = 197;
            // 
            // CmbChkLedger
            // 
            this.CmbChkLedger.Location = new System.Drawing.Point(88, 47);
            this.CmbChkLedger.Name = "CmbChkLedger";
            this.CmbChkLedger.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbChkLedger.Size = new System.Drawing.Size(302, 20);
            this.CmbChkLedger.TabIndex = 211;
            // 
            // CmbStoneType
            // 
            this.CmbStoneType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbStoneType.FormattingEnabled = true;
            this.CmbStoneType.Items.AddRange(new object[] {
            "Single ",
            "Parcel"});
            this.CmbStoneType.Location = new System.Drawing.Point(481, 46);
            this.CmbStoneType.Name = "CmbStoneType";
            this.CmbStoneType.Size = new System.Drawing.Size(153, 22);
            this.CmbStoneType.TabIndex = 197;
            // 
            // cLabel8
            // 
            this.cLabel8.AutoSize = true;
            this.cLabel8.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel8.ForeColor = System.Drawing.Color.Black;
            this.cLabel8.Location = new System.Drawing.Point(415, 50);
            this.cLabel8.Name = "cLabel8";
            this.cLabel8.Size = new System.Drawing.Size(39, 13);
            this.cLabel8.TabIndex = 210;
            this.cLabel8.Text = "Type";
            this.cLabel8.ToolTips = "";
            // 
            // cLabel7
            // 
            this.cLabel7.AutoSize = true;
            this.cLabel7.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel7.ForeColor = System.Drawing.Color.Black;
            this.cLabel7.Location = new System.Drawing.Point(6, 77);
            this.cLabel7.Name = "cLabel7";
            this.cLabel7.Size = new System.Drawing.Size(59, 13);
            this.cLabel7.TabIndex = 205;
            this.cLabel7.Text = "Account";
            this.cLabel7.ToolTips = "";
            // 
            // cLabel1
            // 
            this.cLabel1.AutoSize = true;
            this.cLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel1.ForeColor = System.Drawing.Color.Black;
            this.cLabel1.Location = new System.Drawing.Point(6, 51);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(52, 13);
            this.cLabel1.TabIndex = 200;
            this.cLabel1.Text = "Ledger";
            this.cLabel1.ToolTips = "";
            // 
            // BtnYear
            // 
            this.BtnYear.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnYear.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnYear.Appearance.Options.UseFont = true;
            this.BtnYear.Appearance.Options.UseForeColor = true;
            this.BtnYear.Location = new System.Drawing.Point(341, 20);
            this.BtnYear.Name = "BtnYear";
            this.BtnYear.Size = new System.Drawing.Size(49, 23);
            this.BtnYear.TabIndex = 199;
            this.BtnYear.Text = "Year";
            // 
            // btnMonth
            // 
            this.btnMonth.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMonth.Appearance.ForeColor = System.Drawing.Color.Black;
            this.btnMonth.Appearance.Options.UseFont = true;
            this.btnMonth.Appearance.Options.UseForeColor = true;
            this.btnMonth.Location = new System.Drawing.Point(287, 20);
            this.btnMonth.Name = "btnMonth";
            this.btnMonth.Size = new System.Drawing.Size(49, 23);
            this.btnMonth.TabIndex = 198;
            this.btnMonth.Text = "Month";
            // 
            // BtnToday
            // 
            this.BtnToday.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnToday.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnToday.Appearance.Options.UseFont = true;
            this.BtnToday.Appearance.Options.UseForeColor = true;
            this.BtnToday.Location = new System.Drawing.Point(233, 20);
            this.BtnToday.Name = "BtnToday";
            this.BtnToday.Size = new System.Drawing.Size(49, 23);
            this.BtnToday.TabIndex = 197;
            this.BtnToday.Text = "Today";
            // 
            // LblToDate
            // 
            this.LblToDate.AutoSize = true;
            this.LblToDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblToDate.ForeColor = System.Drawing.Color.Black;
            this.LblToDate.Location = new System.Drawing.Point(415, 24);
            this.LblToDate.Name = "LblToDate";
            this.LblToDate.Size = new System.Drawing.Size(57, 13);
            this.LblToDate.TabIndex = 196;
            this.LblToDate.Text = "To Date";
            this.LblToDate.ToolTips = "";
            // 
            // DTPToDate
            // 
            this.DTPToDate.AllowTabKeyOnEnter = false;
            this.DTPToDate.CustomFormat = "dd/MM/yyyy";
            this.DTPToDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DTPToDate.ForeColor = System.Drawing.Color.Black;
            this.DTPToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTPToDate.Location = new System.Drawing.Point(481, 20);
            this.DTPToDate.Name = "DTPToDate";
            this.DTPToDate.Size = new System.Drawing.Size(153, 21);
            this.DTPToDate.TabIndex = 195;
            this.DTPToDate.ToolTips = "";
            // 
            // LblFromDate
            // 
            this.LblFromDate.AutoSize = true;
            this.LblFromDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LblFromDate.ForeColor = System.Drawing.Color.Black;
            this.LblFromDate.Location = new System.Drawing.Point(6, 24);
            this.LblFromDate.Name = "LblFromDate";
            this.LblFromDate.Size = new System.Drawing.Size(75, 13);
            this.LblFromDate.TabIndex = 194;
            this.LblFromDate.Text = "From Date";
            this.LblFromDate.ToolTips = "";
            // 
            // DTPFromDate
            // 
            this.DTPFromDate.AllowTabKeyOnEnter = false;
            this.DTPFromDate.CustomFormat = "dd/MM/yyyy";
            this.DTPFromDate.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DTPFromDate.ForeColor = System.Drawing.Color.Black;
            this.DTPFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTPFromDate.Location = new System.Drawing.Point(88, 20);
            this.DTPFromDate.Name = "DTPFromDate";
            this.DTPFromDate.Size = new System.Drawing.Size(141, 21);
            this.DTPFromDate.TabIndex = 193;
            this.DTPFromDate.ToolTips = "";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Silver;
            this.panel3.Controls.Add(this.BtnReset);
            this.panel3.Controls.Add(this.BtnPrint);
            this.panel3.Controls.Add(this.BtnPreview);
            this.panel3.Controls.Add(this.BtnClose);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 151);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(630, 50);
            this.panel3.TabIndex = 33;
            // 
            // BtnReset
            // 
            this.BtnReset.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnReset.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnReset.Appearance.Options.UseFont = true;
            this.BtnReset.Appearance.Options.UseForeColor = true;
            this.BtnReset.ImageOptions.Image = global::MahantExport.Properties.Resources.btnclear;
            this.BtnReset.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnReset.ImageOptions.SvgImage")));
            this.BtnReset.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnReset.Location = new System.Drawing.Point(225, 8);
            this.BtnReset.Name = "BtnReset";
            this.BtnReset.Size = new System.Drawing.Size(103, 35);
            this.BtnReset.TabIndex = 33;
            this.BtnReset.TabStop = false;
            this.BtnReset.Text = "Reset";
            // 
            // BtnPrint
            // 
            this.BtnPrint.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnPrint.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnPrint.Appearance.Options.UseFont = true;
            this.BtnPrint.Appearance.Options.UseForeColor = true;
            this.BtnPrint.ImageOptions.Image = global::MahantExport.Properties.Resources.btnprint;
            this.BtnPrint.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnPrint.ImageOptions.SvgImage")));
            this.BtnPrint.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnPrint.Location = new System.Drawing.Point(9, 8);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(103, 35);
            this.BtnPrint.TabIndex = 30;
            this.BtnPrint.Text = "Print";
            this.BtnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // BtnPreview
            // 
            this.BtnPreview.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnPreview.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnPreview.Appearance.Options.UseFont = true;
            this.BtnPreview.Appearance.Options.UseForeColor = true;
            this.BtnPreview.ImageOptions.Image = global::MahantExport.Properties.Resources.btnprint;
            this.BtnPreview.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnPreview.ImageOptions.SvgImage")));
            this.BtnPreview.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnPreview.Location = new System.Drawing.Point(117, 8);
            this.BtnPreview.Name = "BtnPreview";
            this.BtnPreview.Size = new System.Drawing.Size(103, 35);
            this.BtnPreview.TabIndex = 31;
            this.BtnPreview.TabStop = false;
            this.BtnPreview.Text = "Preview";
            // 
            // BtnClose
            // 
            this.BtnClose.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnClose.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnClose.Appearance.Options.UseFont = true;
            this.BtnClose.Appearance.Options.UseForeColor = true;
            this.BtnClose.ImageOptions.Image = global::MahantExport.Properties.Resources.btnexit;
            this.BtnClose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnClose.ImageOptions.SvgImage")));
            this.BtnClose.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnClose.Location = new System.Drawing.Point(333, 8);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(103, 35);
            this.BtnClose.TabIndex = 32;
            this.BtnClose.TabStop = false;
            this.BtnClose.Text = "&Exit";
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // FrmRegisterPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 201);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupControl1);
            this.LookAndFeel.SkinName = "London Liquid Sky";
            this.LookAndFeel.UseDefaultLookAndFeel =true;
            this.Name = "FrmRegisterPrint";
            this.Text = "REGISTER PRINT";
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CmbChkLedger.Properties)).EndInit();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevControlLib.cDevGroupControl groupControl1;
        private System.Windows.Forms.ComboBox CmbReportType;
        private AxonContLib.cLabel cLabel3;
        private AxonContLib.cGroupBox groupBox1;
        private AxonContLib.cLabel LblToDate;
        private AxonContLib.cDateTimePicker DTPToDate;
        private AxonContLib.cLabel LblFromDate;
        private AxonContLib.cDateTimePicker DTPFromDate;
        private DevControlLib.cDevSimpleButton BtnYear;
        private DevControlLib.cDevSimpleButton btnMonth;
        private DevControlLib.cDevSimpleButton BtnToday;
        private AxonContLib.cLabel cLabel1;
        private AxonContLib.cPanel panel3;
        private DevControlLib.cDevSimpleButton BtnReset;
        private DevControlLib.cDevSimpleButton BtnPrint;
        private DevControlLib.cDevSimpleButton BtnPreview;
        private DevControlLib.cDevSimpleButton BtnClose;
        private AxonContLib.cLabel cLabel7;
        private AxonContLib.cLabel cLabel8;
        private System.Windows.Forms.ComboBox CmbStoneType;
        private DevControlLib.cDevCheckedComboBoxEdit CmbChkLedger;
        private System.Windows.Forms.ComboBox CmbAccountType;


    }
}