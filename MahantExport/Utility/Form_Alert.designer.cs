namespace MahantExport.View
{
    partial class Form_Alert
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
            this.lblMsg = new DevExpress.XtraEditors.LabelControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.BtnDone = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnClose = new DevControlLib.cDevSimpleButton(this.components);
            this.SuspendLayout();
            // 
            // lblMsg
            // 
            this.lblMsg.Appearance.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.Appearance.ForeColor = System.Drawing.Color.White;
            this.lblMsg.Appearance.Options.UseFont = true;
            this.lblMsg.Appearance.Options.UseForeColor = true;
            this.lblMsg.Location = new System.Drawing.Point(32, 8);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(89, 14);
            this.lblMsg.TabIndex = 1;
            this.lblMsg.Text = "labelControl1";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // BtnDone
            // 
            this.BtnDone.ImageOptions.Image = global::MahantExport.Properties.Resources.Confirm;
            this.BtnDone.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.BtnDone.Location = new System.Drawing.Point(301, 49);
            this.BtnDone.Name = "BtnDone";
            this.BtnDone.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.BtnDone.Size = new System.Drawing.Size(41, 39);
            this.BtnDone.TabIndex = 2;
            this.BtnDone.TabStop = false;
            this.BtnDone.Text = "simpleButton1";
            this.BtnDone.Visible = false;
            this.BtnDone.Click += new System.EventHandler(this.BtnDone_Click);
            // 
            // BtnClose
            // 
            this.BtnClose.Appearance.Font = new System.Drawing.Font("Verdana", 18F, System.Drawing.FontStyle.Bold);
            this.BtnClose.Appearance.Options.UseFont = true;
            this.BtnClose.ImageOptions.Location = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.BtnClose.Location = new System.Drawing.Point(311, 5);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.PaintStyle = DevExpress.XtraEditors.Controls.PaintStyles.Light;
            this.BtnClose.Size = new System.Drawing.Size(31, 26);
            this.BtnClose.TabIndex = 3;
            this.BtnClose.TabStop = false;
            this.BtnClose.Text = "X";
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // Form_Alert
            // 
            this.Appearance.BackColor = System.Drawing.Color.SteelBlue;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(348, 121);
            this.Controls.Add(this.BtnClose);
            this.Controls.Add(this.BtnDone);
            this.Controls.Add(this.lblMsg);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.LookAndFeel.SkinName = "London Liquid Sky";
            this.LookAndFeel.UseDefaultLookAndFeel =true;
            this.Name = "Form_Alert";
            this.Text = "Form_Alert";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Alert_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblMsg;
        private System.Windows.Forms.Timer timer1;
        private DevControlLib.cDevSimpleButton BtnDone;
        private DevControlLib.cDevSimpleButton BtnClose;
    }
}