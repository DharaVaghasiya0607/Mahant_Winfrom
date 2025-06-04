namespace Licence
{
    partial class Activation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Activation));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBoardID = new System.Windows.Forms.TextBox();
            this.BtnSubmit = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtActivation = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtDays = new System.Windows.Forms.TextBox();
            this.cLabel6 = new AxonContLib.cLabel(this.components);
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            this.cLabel2 = new AxonContLib.cLabel(this.components);
            this.cLabel3 = new AxonContLib.cLabel(this.components);
            this.cLabel4 = new AxonContLib.cLabel(this.components);
            this.cLabel5 = new AxonContLib.cLabel(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.CmbMode = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(78)))), ((int)(((byte)(150)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(640, 36);
            this.label1.TabIndex = 58;
            this.label1.Text = "Software Activation";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label2.Location = new System.Drawing.Point(20, 53);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 23);
            this.label2.TabIndex = 0;
            this.label2.Text = "Board ID";
            // 
            // txtBoardID
            // 
            this.txtBoardID.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBoardID.ForeColor = System.Drawing.Color.Purple;
            this.txtBoardID.Location = new System.Drawing.Point(20, 83);
            this.txtBoardID.Margin = new System.Windows.Forms.Padding(4);
            this.txtBoardID.Name = "txtBoardID";
            this.txtBoardID.ReadOnly = true;
            this.txtBoardID.Size = new System.Drawing.Size(596, 31);
            this.txtBoardID.TabIndex = 1;
            this.txtBoardID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // BtnSubmit
            // 
            this.BtnSubmit.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSubmit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.BtnSubmit.Location = new System.Drawing.Point(20, 320);
            this.BtnSubmit.Margin = new System.Windows.Forms.Padding(4);
            this.BtnSubmit.Name = "BtnSubmit";
            this.BtnSubmit.Size = new System.Drawing.Size(596, 65);
            this.BtnSubmit.TabIndex = 7;
            this.BtnSubmit.Text = "Activate";
            this.BtnSubmit.UseVisualStyleBackColor = true;
            this.BtnSubmit.Click += new System.EventHandler(this.BtnSubmit_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(20, 142);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 23);
            this.label3.TabIndex = 2;
            this.label3.Text = "Activation Key";
            // 
            // txtActivation
            // 
            this.txtActivation.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtActivation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtActivation.Location = new System.Drawing.Point(20, 172);
            this.txtActivation.Margin = new System.Windows.Forms.Padding(4);
            this.txtActivation.Name = "txtActivation";
            this.txtActivation.PasswordChar = '*';
            this.txtActivation.Size = new System.Drawing.Size(473, 31);
            this.txtActivation.TabIndex = 3;
            this.txtActivation.Tag = "AxnInf#623_enc";
            this.txtActivation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label4.Location = new System.Drawing.Point(500, 142);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 23);
            this.label4.TabIndex = 59;
            this.label4.Text = "Days";
            // 
            // txtDays
            // 
            this.txtDays.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDays.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtDays.Location = new System.Drawing.Point(501, 172);
            this.txtDays.Margin = new System.Windows.Forms.Padding(4);
            this.txtDays.Name = "txtDays";
            this.txtDays.Size = new System.Drawing.Size(115, 31);
            this.txtDays.TabIndex = 4;
            this.txtDays.Text = "15";
            this.txtDays.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // cLabel6
            // 
            this.cLabel6.AutoSize = true;
            this.cLabel6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(78)))), ((int)(((byte)(150)))));
            this.cLabel6.Font = new System.Drawing.Font("Verdana", 8F);
            this.cLabel6.ForeColor = System.Drawing.Color.White;
            this.cLabel6.Location = new System.Drawing.Point(10, 149);
            this.cLabel6.Name = "cLabel6";
            this.cLabel6.Size = new System.Drawing.Size(245, 17);
            this.cLabel6.TabIndex = 73;
            this.cLabel6.Text = "Raj Vakadiya (+91-80008 05300)";
            this.cLabel6.ToolTips = "";
            // 
            // cLabel1
            // 
            this.cLabel1.AutoSize = true;
            this.cLabel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(78)))), ((int)(((byte)(150)))));
            this.cLabel1.Font = new System.Drawing.Font("Verdana", 8F);
            this.cLabel1.ForeColor = System.Drawing.Color.White;
            this.cLabel1.Location = new System.Drawing.Point(10, 103);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(256, 17);
            this.cLabel1.TabIndex = 73;
            this.cLabel1.Text = "Vipul Vakadiya (+91-85300 67187)";
            this.cLabel1.ToolTips = "";
            // 
            // cLabel2
            // 
            this.cLabel2.AutoSize = true;
            this.cLabel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(78)))), ((int)(((byte)(150)))));
            this.cLabel2.Font = new System.Drawing.Font("Verdana", 15F, System.Drawing.FontStyle.Bold);
            this.cLabel2.ForeColor = System.Drawing.Color.White;
            this.cLabel2.Location = new System.Drawing.Point(144, 11);
            this.cLabel2.Name = "cLabel2";
            this.cLabel2.Size = new System.Drawing.Size(363, 31);
            this.cLabel2.TabIndex = 73;
            this.cLabel2.Text = "AXONE INFOTECH INDIA";
            this.cLabel2.ToolTips = "";
            // 
            // cLabel3
            // 
            this.cLabel3.AutoSize = true;
            this.cLabel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(78)))), ((int)(((byte)(150)))));
            this.cLabel3.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.cLabel3.ForeColor = System.Drawing.Color.White;
            this.cLabel3.Location = new System.Drawing.Point(170, 63);
            this.cLabel3.Name = "cLabel3";
            this.cLabel3.Size = new System.Drawing.Size(246, 18);
            this.cLabel3.TabIndex = 73;
            this.cLabel3.Text = "COMMITTED TO THE FUTURE";
            this.cLabel3.ToolTips = "";
            // 
            // cLabel4
            // 
            this.cLabel4.AutoSize = true;
            this.cLabel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(78)))), ((int)(((byte)(150)))));
            this.cLabel4.Font = new System.Drawing.Font("Verdana", 8F);
            this.cLabel4.ForeColor = System.Drawing.Color.White;
            this.cLabel4.Location = new System.Drawing.Point(310, 103);
            this.cLabel4.Name = "cLabel4";
            this.cLabel4.Size = new System.Drawing.Size(306, 34);
            this.cLabel4.TabIndex = 73;
            this.cLabel4.Text = "623-624, Laxmi Enclave-2, Gajera School,\r\nKatargam, Surat-395004, India. ";
            this.cLabel4.ToolTips = "";
            // 
            // cLabel5
            // 
            this.cLabel5.AutoSize = true;
            this.cLabel5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(78)))), ((int)(((byte)(150)))));
            this.cLabel5.Font = new System.Drawing.Font("Verdana", 8F);
            this.cLabel5.ForeColor = System.Drawing.Color.White;
            this.cLabel5.Location = new System.Drawing.Point(310, 149);
            this.cLabel5.Name = "cLabel5";
            this.cLabel5.Size = new System.Drawing.Size(222, 17);
            this.cLabel5.TabIndex = 73;
            this.cLabel5.Text = "sales@axoneinfotechindia.com";
            this.cLabel5.ToolTips = "";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(78)))), ((int)(((byte)(150)))));
            this.panel1.Controls.Add(this.cLabel2);
            this.panel1.Controls.Add(this.cLabel5);
            this.panel1.Controls.Add(this.cLabel6);
            this.panel1.Controls.Add(this.cLabel4);
            this.panel1.Controls.Add(this.cLabel3);
            this.panel1.Controls.Add(this.cLabel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 404);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(640, 182);
            this.panel1.TabIndex = 74;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.label5.Location = new System.Drawing.Point(20, 227);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 23);
            this.label5.TabIndex = 2;
            this.label5.Text = "Server Mode";
            // 
            // CmbMode
            // 
            this.CmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbMode.Font = new System.Drawing.Font("Cambria", 12F, System.Drawing.FontStyle.Bold);
            this.CmbMode.FormattingEnabled = true;
            this.CmbMode.Items.AddRange(new object[] {
            "SERVER MODE",
            "SINGLE CLIENT MODE"});
            this.CmbMode.Location = new System.Drawing.Point(20, 257);
            this.CmbMode.Name = "CmbMode";
            this.CmbMode.Size = new System.Drawing.Size(596, 31);
            this.CmbMode.TabIndex = 75;
            // 
            // Activation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(640, 586);
            this.Controls.Add(this.CmbMode);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.BtnSubmit);
            this.Controls.Add(this.txtDays);
            this.Controls.Add(this.txtActivation);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtBoardID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Activation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Activation";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Activation_FormClosing);
            this.Load += new System.EventHandler(this.Activation_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBoardID;
        private System.Windows.Forms.Button BtnSubmit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtActivation;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtDays;
        private AxonContLib.cLabel cLabel6;
        private AxonContLib.cLabel cLabel1;
        private AxonContLib.cLabel cLabel2;
        private AxonContLib.cLabel cLabel3;
        private AxonContLib.cLabel cLabel4;
        private AxonContLib.cLabel cLabel5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox CmbMode;
    }
}