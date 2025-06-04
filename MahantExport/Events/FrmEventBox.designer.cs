namespace MahantExport.Events
{
    partial class FrmEventBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmEventBox));
            this.pImage = new System.Windows.Forms.PictureBox();
            this.PBox = new AxonContLib.cPanel(this.components);
            this.BtnClose = new DevControlLib.cDevSimpleButton(this.components);
            this.cPanel2 = new AxonContLib.cPanel(this.components);
            this.cLabel2 = new AxonContLib.cLabel(this.components);
            this.timer30Second = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pImage)).BeginInit();
            this.PBox.SuspendLayout();
            this.cPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pImage
            // 
            this.pImage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pImage.Location = new System.Drawing.Point(0, 32);
            this.pImage.Name = "pImage";
            this.pImage.Size = new System.Drawing.Size(676, 607);
            this.pImage.TabIndex = 0;
            this.pImage.TabStop = false;
            // 
            // PBox
            // 
            this.PBox.BackColor = System.Drawing.Color.White;
            this.PBox.Controls.Add(this.BtnClose);
            this.PBox.Controls.Add(this.cPanel2);
            this.PBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.PBox.Location = new System.Drawing.Point(0, 0);
            this.PBox.Name = "PBox";
            this.PBox.Size = new System.Drawing.Size(676, 32);
            this.PBox.TabIndex = 5;
            // 
            // BtnClose
            // 
            this.BtnClose.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.BtnClose.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnClose.Appearance.Options.UseFont = true;
            this.BtnClose.Appearance.Options.UseForeColor = true;
            this.BtnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.BtnClose.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.BtnClose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnClose.ImageOptions.SvgImage")));
            this.BtnClose.Location = new System.Drawing.Point(641, 0);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(35, 32);
            this.BtnClose.TabIndex = 9;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click_1);
            // 
            // cPanel2
            // 
            this.cPanel2.BackColor = System.Drawing.Color.White;
            this.cPanel2.Controls.Add(this.cLabel2);
            this.cPanel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.cPanel2.Location = new System.Drawing.Point(0, 0);
            this.cPanel2.Name = "cPanel2";
            this.cPanel2.Size = new System.Drawing.Size(642, 32);
            this.cPanel2.TabIndex = 7;
            // 
            // cLabel2
            // 
            this.cLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cLabel2.AutoSize = true;
            this.cLabel2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.cLabel2.ForeColor = System.Drawing.Color.Navy;
            this.cLabel2.Location = new System.Drawing.Point(204, 9);
            this.cLabel2.Name = "cLabel2";
            this.cLabel2.Size = new System.Drawing.Size(232, 14);
            this.cLabel2.TabIndex = 38;
            this.cLabel2.Text = "!!  Shivam GEMS  !!";
            this.cLabel2.ToolTips = "";
            this.cLabel2.Visible = false;
            // 
            // timer30Second
            // 
            this.timer30Second.Enabled = true;
            this.timer30Second.Interval = 5000;
            this.timer30Second.Tick += new System.EventHandler(this.timer30Second_Tick);
            // 
            // FrmEventBox
            // 
            this.Appearance.BackColor = System.Drawing.Color.LightGray;
            this.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.Appearance.Options.UseBackColor = true;
            this.Appearance.Options.UseForeColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 639);
            this.Controls.Add(this.pImage);
            this.Controls.Add(this.PBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("FrmEventBox.IconOptions.Icon")));
            this.KeyPreview = true;
            this.Name = "FrmEventBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "!! AXONE INFOTECH !!";
            this.Load += new System.EventHandler(this.FrmEventBox_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pImage)).EndInit();
            this.PBox.ResumeLayout(false);
            this.cPanel2.ResumeLayout(false);
            this.cPanel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pImage;
        private AxonContLib.cPanel PBox;
        private AxonContLib.cLabel cLabel2;
        private AxonContLib.cPanel cPanel2;
        private DevControlLib.cDevSimpleButton BtnClose;
        private System.Windows.Forms.Timer timer30Second;





    }
}