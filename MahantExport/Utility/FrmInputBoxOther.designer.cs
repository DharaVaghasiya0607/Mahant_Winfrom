using HTMLEditorControl;
namespace MahantExport
{
    partial class FrmInputBoxOther
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmInputBoxOther));
            this.GrpMsg = new DevControlLib.cDevGroupControl(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtHtmlEditorEnglish = new HTMLEditorControl.HtmlEditorControl();
            this.panel7 = new AxonContLib.cPanel(this.components);
            this.cLabel2 = new AxonContLib.cLabel(this.components);
            this.txtHtmlEditorChinese = new HTMLEditorControl.HtmlEditorControl();
            this.panel1 = new AxonContLib.cPanel(this.components);
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            this.panel3 = new AxonContLib.cPanel(this.components);
            this.BtnClose = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnSubmit = new DevControlLib.cDevSimpleButton(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.GrpMsg)).BeginInit();
            this.GrpMsg.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // GrpMsg
            // 
            this.GrpMsg.AppearanceCaption.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.GrpMsg.AppearanceCaption.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.GrpMsg.AppearanceCaption.Font = new System.Drawing.Font("Cambria", 14F);
            this.GrpMsg.AppearanceCaption.ForeColor = System.Drawing.Color.White;
            this.GrpMsg.AppearanceCaption.Options.UseBackColor = true;
            this.GrpMsg.AppearanceCaption.Options.UseFont = true;
            this.GrpMsg.AppearanceCaption.Options.UseForeColor = true;
            this.GrpMsg.Controls.Add(this.splitContainer1);
            this.GrpMsg.Controls.Add(this.panel3);
            this.GrpMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GrpMsg.Location = new System.Drawing.Point(0, 0);
            this.GrpMsg.LookAndFeel.SkinName = "Office 2010 Black";
            this.GrpMsg.LookAndFeel.UseDefaultLookAndFeel = false;
            this.GrpMsg.Name = "GrpMsg";
            this.GrpMsg.Size = new System.Drawing.Size(630, 534);
            this.GrpMsg.TabIndex = 0;
            this.GrpMsg.Text = "Enter Your Message";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(2, 27);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtHtmlEditorEnglish);
            this.splitContainer1.Panel1.Controls.Add(this.panel7);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.txtHtmlEditorChinese);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new System.Drawing.Size(626, 464);
            this.splitContainer1.SplitterDistance = 316;
            this.splitContainer1.TabIndex = 14;
            // 
            // txtHtmlEditorEnglish
            // 
            this.txtHtmlEditorEnglish.BodyFont = new HTMLEditorControl.HtmlFontProperty("verdana", HTMLEditorControl.HtmlFontSize.Small, false, false, false, false, false, false);
            this.txtHtmlEditorEnglish.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHtmlEditorEnglish.InnerText = null;
            this.txtHtmlEditorEnglish.Location = new System.Drawing.Point(0, 24);
            this.txtHtmlEditorEnglish.Name = "txtHtmlEditorEnglish";
            this.txtHtmlEditorEnglish.Size = new System.Drawing.Size(316, 440);
            this.txtHtmlEditorEnglish.TabIndex = 16;
            // 
            // panel7
            // 
            this.panel7.BackColor = System.Drawing.Color.Silver;
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.cLabel2);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(0, 0);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(316, 24);
            this.panel7.TabIndex = 15;
            // 
            // cLabel2
            // 
            this.cLabel2.AutoSize = true;
            this.cLabel2.Font = new System.Drawing.Font("Verdana", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.cLabel2.ForeColor = System.Drawing.Color.Black;
            this.cLabel2.Location = new System.Drawing.Point(9, 3);
            this.cLabel2.Name = "cLabel2";
            this.cLabel2.Size = new System.Drawing.Size(118, 17);
            this.cLabel2.TabIndex = 12;
            this.cLabel2.Text = "--: English :--";
            this.cLabel2.ToolTips = "";
            // 
            // txtHtmlEditorChinese
            // 
            this.txtHtmlEditorChinese.BodyFont = new HTMLEditorControl.HtmlFontProperty("verdana", HTMLEditorControl.HtmlFontSize.Small, false, false, false, false, false, false);
            this.txtHtmlEditorChinese.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHtmlEditorChinese.InnerText = null;
            this.txtHtmlEditorChinese.Location = new System.Drawing.Point(0, 24);
            this.txtHtmlEditorChinese.Name = "txtHtmlEditorChinese";
            this.txtHtmlEditorChinese.Size = new System.Drawing.Size(306, 440);
            this.txtHtmlEditorChinese.TabIndex = 17;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Silver;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cLabel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(306, 24);
            this.panel1.TabIndex = 16;
            // 
            // cLabel1
            // 
            this.cLabel1.AutoSize = true;
            this.cLabel1.Font = new System.Drawing.Font("Verdana", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))));
            this.cLabel1.ForeColor = System.Drawing.Color.Black;
            this.cLabel1.Location = new System.Drawing.Point(1, 3);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(127, 17);
            this.cLabel1.TabIndex = 12;
            this.cLabel1.Text = " --: Chinese :--";
            this.cLabel1.ToolTips = "";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.Silver;
            this.panel3.Controls.Add(this.BtnClose);
            this.panel3.Controls.Add(this.BtnSubmit);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(2, 491);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(626, 41);
            this.panel3.TabIndex = 13;
            // 
            // BtnClose
            // 
            this.BtnClose.Appearance.Font = new System.Drawing.Font("Cambria", 13F, System.Drawing.FontStyle.Bold);
            this.BtnClose.Appearance.Options.UseFont = true;
            this.BtnClose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnClose.ImageOptions.SvgImage")));
            this.BtnClose.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnClose.Location = new System.Drawing.Point(326, 4);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(178, 31);
            this.BtnClose.TabIndex = 2;
            this.BtnClose.Text = "E&xit";
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // BtnSubmit
            // 
            this.BtnSubmit.Appearance.Font = new System.Drawing.Font("Cambria", 13F, System.Drawing.FontStyle.Bold);
            this.BtnSubmit.Appearance.Options.UseFont = true;
            this.BtnSubmit.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSubmit.ImageOptions.SvgImage")));
            this.BtnSubmit.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnSubmit.Location = new System.Drawing.Point(117, 4);
            this.BtnSubmit.Name = "BtnSubmit";
            this.BtnSubmit.Size = new System.Drawing.Size(203, 31);
            this.BtnSubmit.TabIndex = 1;
            this.BtnSubmit.Text = "&Save";
            this.BtnSubmit.Click += new System.EventHandler(this.BtnSubmit_Click);
            // 
            // FrmInputBoxOther
            // 
            this.Appearance.BackColor = System.Drawing.Color.LightGray;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 534);
            this.Controls.Add(this.GrpMsg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("FrmInputBoxOther.IconOptions.Icon")));
            this.KeyPreview = true;
            this.LookAndFeel.SkinName = "London Liquid Sky";
            this.LookAndFeel.UseDefaultLookAndFeel =true;
            this.Name = "FrmInputBoxOther";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ABOUT US DESCRIPTION";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSearch_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.GrpMsg)).EndInit();
            this.GrpMsg.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevControlLib.cDevGroupControl GrpMsg;
        private DevControlLib.cDevSimpleButton BtnSubmit;
        private DevControlLib.cDevSimpleButton BtnClose;
        private AxonContLib.cPanel panel3;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private AxonContLib.cPanel panel7;
        private AxonContLib.cLabel cLabel2;
        private AxonContLib.cPanel panel1;
        private AxonContLib.cLabel cLabel1;
        private HtmlEditorControl txtHtmlEditorEnglish;
        private HtmlEditorControl txtHtmlEditorChinese;




    }
}