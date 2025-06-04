namespace MahantExport.Utility
{
    partial class frmTxtView
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
            this.btnPrint = new AxonContLib.cButton(this.components);
            this.btnClose = new AxonContLib.cButton(this.components);
            this.Rtb = new System.Windows.Forms.RichTextBox();
            this.grpDosWin = new AxonContLib.cGroupBox(this.components);
            this.radWin = new AxonContLib.cRadioButton(this.components);
            this.radDos = new AxonContLib.cRadioButton(this.components);
            this.label1 = new AxonContLib.cLabel(this.components);
            this.grpDosWin.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.LavenderBlush;
            this.btnPrint.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrint.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnPrint.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPrint.ForeColor = System.Drawing.Color.Navy;
            this.btnPrint.Location = new System.Drawing.Point(854, 36);
            this.btnPrint.Margin = new System.Windows.Forms.Padding(0);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(53, 26);
            this.btnPrint.TabIndex = 2;
            this.btnPrint.TabStop = false;
            this.btnPrint.Text = "&Print";
            this.btnPrint.ToolTips = "";
            this.btnPrint.UseVisualStyleBackColor = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.LavenderBlush;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.Navy;
            this.btnClose.Location = new System.Drawing.Point(909, 36);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(56, 26);
            this.btnClose.TabIndex = 3;
            this.btnClose.TabStop = false;
            this.btnClose.Text = "&Close";
            this.btnClose.ToolTips = "";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Rtb
            // 
            this.Rtb.BackColor = System.Drawing.Color.PeachPuff;
            this.Rtb.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Rtb.Location = new System.Drawing.Point(5, 75);
            this.Rtb.Name = "Rtb";
            this.Rtb.ReadOnly = true;
            this.Rtb.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.Rtb.Size = new System.Drawing.Size(971, 482);
            this.Rtb.TabIndex = 4;
            this.Rtb.Text = "";
            this.Rtb.WordWrap = false;
            // 
            // grpDosWin
            // 
            this.grpDosWin.Controls.Add(this.radWin);
            this.grpDosWin.Controls.Add(this.radDos);
            this.grpDosWin.Location = new System.Drawing.Point(597, 31);
            this.grpDosWin.Margin = new System.Windows.Forms.Padding(0);
            this.grpDosWin.Name = "grpDosWin";
            this.grpDosWin.Padding = new System.Windows.Forms.Padding(0);
            this.grpDosWin.Size = new System.Drawing.Size(202, 31);
            this.grpDosWin.TabIndex = 24;
            this.grpDosWin.TabStop = false;
            // 
            // radWin
            // 
            this.radWin.AllowTabKeyOnEnter = false;
            this.radWin.AutoSize = true;
            this.radWin.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radWin.ForeColor = System.Drawing.Color.DarkBlue;
            this.radWin.Location = new System.Drawing.Point(112, 9);
            this.radWin.Margin = new System.Windows.Forms.Padding(0);
            this.radWin.Name = "radWin";
            this.radWin.Size = new System.Drawing.Size(76, 17);
            this.radWin.TabIndex = 1;
            this.radWin.TabStop = true;
            this.radWin.Tag = "W";
            this.radWin.Text = "&DeskJet";
            this.radWin.ToolTips = "";
            this.radWin.UseVisualStyleBackColor = true;
            this.radWin.CheckedChanged += new System.EventHandler(this.radDosWin_CheckedChanged);
            // 
            // radDos
            // 
            this.radDos.AllowTabKeyOnEnter = false;
            this.radDos.AutoSize = true;
            this.radDos.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radDos.ForeColor = System.Drawing.Color.DarkBlue;
            this.radDos.Location = new System.Drawing.Point(3, 9);
            this.radDos.Margin = new System.Windows.Forms.Padding(0);
            this.radDos.Name = "radDos";
            this.radDos.Size = new System.Drawing.Size(102, 17);
            this.radDos.TabIndex = 0;
            this.radDos.TabStop = true;
            this.radDos.Tag = "D";
            this.radDos.Text = "Dot - &Matrix";
            this.radDos.ToolTips = "";
            this.radDos.UseVisualStyleBackColor = true;
            this.radDos.CheckedChanged += new System.EventHandler(this.radDosWin_CheckedChanged);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Teal;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(976, 25);
            this.label1.TabIndex = 25;
            this.label1.Text = "Printing....";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.ToolTips = "";
            // 
            // frmTxtView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 563);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnPrint);
            this.Controls.Add(this.grpDosWin);
            this.Controls.Add(this.Rtb);
            this.KeyPreview = true;
            this.LookAndFeel.SkinName = "London Liquid Sky";
            this.LookAndFeel.UseDefaultLookAndFeel =true;
            this.Name = "frmTxtView";
            this.Text = "Printing ...";
            this.grpDosWin.ResumeLayout(false);
            this.grpDosWin.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        internal AxonContLib.cButton btnPrint;
        internal AxonContLib.cButton btnClose;
        internal System.Windows.Forms.RichTextBox Rtb;
        private AxonContLib.cGroupBox grpDosWin;
        private AxonContLib.cRadioButton radWin;
        private AxonContLib.cRadioButton radDos;
        internal AxonContLib.cLabel label1;


    }
}