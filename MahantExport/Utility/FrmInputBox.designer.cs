namespace MahantExport
{
    partial class FrmInputBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmInputBox));
            this.groupControl1 = new DevControlLib.cDevGroupControl(this.components);
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            this.cLabel6 = new AxonContLib.cLabel(this.components);
            this.txtMessageChinese = new AxonContLib.cTextBox(this.components);
            this.BtnClose = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnSubmit = new DevControlLib.cDevSimpleButton(this.components);
            this.txtMessageEnglish = new AxonContLib.cTextBox(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Appearance.BackColor = System.Drawing.Color.White;
            this.groupControl1.Appearance.Options.UseBackColor = true;
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Cambria", 12F);
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.cLabel1);
            this.groupControl1.Controls.Add(this.cLabel6);
            this.groupControl1.Controls.Add(this.txtMessageChinese);
            this.groupControl1.Controls.Add(this.BtnClose);
            this.groupControl1.Controls.Add(this.BtnSubmit);
            this.groupControl1.Controls.Add(this.txtMessageEnglish);
            this.groupControl1.Location = new System.Drawing.Point(16, 20);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(583, 308);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "Enter Your Message";
            // 
            // cLabel1
            // 
            this.cLabel1.AutoSize = true;
            this.cLabel1.Font = new System.Drawing.Font("Verdana", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel1.ForeColor = System.Drawing.Color.Black;
            this.cLabel1.Location = new System.Drawing.Point(15, 152);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(61, 14);
            this.cLabel1.TabIndex = 12;
            this.cLabel1.Text = "Chinese";
            this.cLabel1.ToolTips = "";
            // 
            // cLabel6
            // 
            this.cLabel6.AutoSize = true;
            this.cLabel6.Font = new System.Drawing.Font("Verdana", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel6.ForeColor = System.Drawing.Color.Black;
            this.cLabel6.Location = new System.Drawing.Point(15, 36);
            this.cLabel6.Name = "cLabel6";
            this.cLabel6.Size = new System.Drawing.Size(56, 14);
            this.cLabel6.TabIndex = 11;
            this.cLabel6.Text = "English";
            this.cLabel6.ToolTips = "";
            // 
            // txtMessageChinese
            // 
            this.txtMessageChinese.ActivationColor = true;
            this.txtMessageChinese.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtMessageChinese.AllowTabKeyOnEnter = false;
            this.txtMessageChinese.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMessageChinese.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessageChinese.Format = "";
            this.txtMessageChinese.IsComplusory = false;
            this.txtMessageChinese.Location = new System.Drawing.Point(15, 169);
            this.txtMessageChinese.Multiline = true;
            this.txtMessageChinese.Name = "txtMessageChinese";
            this.txtMessageChinese.SelectAllTextOnFocus = true;
            this.txtMessageChinese.Size = new System.Drawing.Size(559, 92);
            this.txtMessageChinese.TabIndex = 3;
            this.txtMessageChinese.ToolTips = "";
            this.txtMessageChinese.WaterMarkText = null;
            // 
            // BtnClose
            // 
            this.BtnClose.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnClose.Appearance.Options.UseFont = true;
            this.BtnClose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnClose.ImageOptions.SvgImage")));
            this.BtnClose.Location = new System.Drawing.Point(494, 267);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(80, 36);
            this.BtnClose.TabIndex = 2;
            this.BtnClose.Text = "Exit";
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // BtnSubmit
            // 
            this.BtnSubmit.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.BtnSubmit.Appearance.Options.UseFont = true;
            this.BtnSubmit.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSubmit.ImageOptions.SvgImage")));
            this.BtnSubmit.Location = new System.Drawing.Point(383, 267);
            this.BtnSubmit.Name = "BtnSubmit";
            this.BtnSubmit.Size = new System.Drawing.Size(105, 36);
            this.BtnSubmit.TabIndex = 1;
            this.BtnSubmit.Text = "&Submit";
            this.BtnSubmit.Click += new System.EventHandler(this.BtnSubmit_Click);
            // 
            // txtMessageEnglish
            // 
            this.txtMessageEnglish.ActivationColor = true;
            this.txtMessageEnglish.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtMessageEnglish.AllowTabKeyOnEnter = false;
            this.txtMessageEnglish.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtMessageEnglish.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessageEnglish.Format = "";
            this.txtMessageEnglish.IsComplusory = false;
            this.txtMessageEnglish.Location = new System.Drawing.Point(15, 53);
            this.txtMessageEnglish.Multiline = true;
            this.txtMessageEnglish.Name = "txtMessageEnglish";
            this.txtMessageEnglish.SelectAllTextOnFocus = true;
            this.txtMessageEnglish.Size = new System.Drawing.Size(559, 92);
            this.txtMessageEnglish.TabIndex = 0;
            this.txtMessageEnglish.ToolTips = "";
            this.txtMessageEnglish.WaterMarkText = null;
            this.txtMessageEnglish.TextChanged += new System.EventHandler(this.txtSeach_TextChanged);
            // 
            // FrmInputBox
            // 
            this.Appearance.BackColor = System.Drawing.Color.LightGray;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 346);
            this.Controls.Add(this.groupControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("FrmInputBox.IconOptions.Icon")));
            this.KeyPreview = true;
            this.LookAndFeel.SkinName = "London Liquid Sky";
            this.LookAndFeel.UseDefaultLookAndFeel =true;
            this.Name = "FrmInputBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "WebsiteMessage\r\n";
            this.Text = "WEBSITE MESSAGE";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FrmInputBox_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSearch_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevControlLib.cDevGroupControl groupControl1;
        private DevControlLib.cDevSimpleButton BtnSubmit;
        private AxonContLib.cTextBox txtMessageEnglish;
        private DevControlLib.cDevSimpleButton BtnClose;
        private AxonContLib.cTextBox txtMessageChinese;
        private AxonContLib.cLabel cLabel6;
        private AxonContLib.cLabel cLabel1;




    }
}