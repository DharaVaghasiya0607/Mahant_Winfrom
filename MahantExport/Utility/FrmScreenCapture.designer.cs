namespace MahantExport
{
    partial class FrmScreenCapture
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmScreenCapture));
            this.groupControl1 = new DevControlLib.cDevGroupControl(this.components);
            this.BtnRecord = new DevControlLib.cDevSimpleButton(this.components);
            this.pictureBox1 = new AxonContLib.cPictureBox(this.components);
            this.BtnClose = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnTransfer = new DevControlLib.cDevSimpleButton(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupControl1
            // 
            this.groupControl1.Appearance.BackColor = System.Drawing.Color.White;
            this.groupControl1.Appearance.Options.UseBackColor = true;
            this.groupControl1.AppearanceCaption.Font = new System.Drawing.Font("Cambria", 12F);
            this.groupControl1.AppearanceCaption.Options.UseFont = true;
            this.groupControl1.Controls.Add(this.BtnRecord);
            this.groupControl1.Controls.Add(this.pictureBox1);
            this.groupControl1.Controls.Add(this.BtnClose);
            this.groupControl1.Controls.Add(this.BtnTransfer);
            this.groupControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupControl1.Location = new System.Drawing.Point(0, 0);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(1133, 606);
            this.groupControl1.TabIndex = 0;
            this.groupControl1.Text = "File Transfer Testing";
            // 
            // BtnRecord
            // 
            this.BtnRecord.Appearance.Font = new System.Drawing.Font("Cambria", 13F, System.Drawing.FontStyle.Bold);
            this.BtnRecord.Appearance.Options.UseFont = true;
            this.BtnRecord.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnRecord.ImageOptions.SvgImage")));
            this.BtnRecord.Location = new System.Drawing.Point(958, 93);
            this.BtnRecord.Name = "BtnRecord";
            this.BtnRecord.Size = new System.Drawing.Size(178, 52);
            this.BtnRecord.TabIndex = 4;
            this.BtnRecord.Text = "Record";
            this.BtnRecord.Click += new System.EventHandler(this.BtnRecord_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(5, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(947, 568);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // BtnClose
            // 
            this.BtnClose.Appearance.Font = new System.Drawing.Font("Cambria", 13F, System.Drawing.FontStyle.Bold);
            this.BtnClose.Appearance.Options.UseFont = true;
            this.BtnClose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnClose.ImageOptions.SvgImage")));
            this.BtnClose.Location = new System.Drawing.Point(958, 151);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(178, 52);
            this.BtnClose.TabIndex = 2;
            this.BtnClose.Text = "Exit";
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // BtnTransfer
            // 
            this.BtnTransfer.Appearance.Font = new System.Drawing.Font("Cambria", 13F, System.Drawing.FontStyle.Bold);
            this.BtnTransfer.Appearance.Options.UseFont = true;
            this.BtnTransfer.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnTransfer.ImageOptions.SvgImage")));
            this.BtnTransfer.Location = new System.Drawing.Point(958, 35);
            this.BtnTransfer.Name = "BtnTransfer";
            this.BtnTransfer.Size = new System.Drawing.Size(178, 52);
            this.BtnTransfer.TabIndex = 1;
            this.BtnTransfer.Text = "Screen Capture";
            this.BtnTransfer.Click += new System.EventHandler(this.BtnSubmit_Click);
            // 
            // FrmScreenCapture
            // 
            this.Appearance.BackColor = System.Drawing.Color.LightGray;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1133, 606);
            this.Controls.Add(this.groupControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("FrmScreenCapture.IconOptions.Icon")));
            this.KeyPreview = true;
            this.LookAndFeel.SkinName = "London Liquid Sky";
            this.LookAndFeel.UseDefaultLookAndFeel =true;
            this.Name = "FrmScreenCapture";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "ScreenCapture";
            this.Text = "Transfer File";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSearch_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevControlLib.cDevGroupControl groupControl1;
        private DevControlLib.cDevSimpleButton BtnTransfer;
        private DevControlLib.cDevSimpleButton BtnClose;
        private AxonContLib.cPictureBox pictureBox1;
        private DevControlLib.cDevSimpleButton BtnRecord;




    }
}