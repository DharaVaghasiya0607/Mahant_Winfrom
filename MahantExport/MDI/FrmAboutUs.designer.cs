namespace MahantExport.Utility
{
    partial class FrmAboutUs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAboutUs));
            this.cLabel2 = new AxonContLib.cLabel(this.components);
            this.cLabel4 = new AxonContLib.cLabel(this.components);
            this.cLabel10 = new AxonContLib.cLabel(this.components);
            this.panel1 = new AxonContLib.cPanel(this.components);
            this.cLabel5 = new AxonContLib.cLabel(this.components);
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cLabel2
            // 
            this.cLabel2.AutoSize = true;
            this.cLabel2.Font = new System.Drawing.Font("Cambria", 13F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel2.ForeColor = System.Drawing.Color.Navy;
            this.cLabel2.Location = new System.Drawing.Point(12, 143);
            this.cLabel2.Name = "cLabel2";
            this.cLabel2.Size = new System.Drawing.Size(117, 21);
            this.cLabel2.TabIndex = 25;
            this.cLabel2.Text = "Our Contacts";
            this.cLabel2.ToolTips = "";
            // 
            // cLabel4
            // 
            this.cLabel4.AutoSize = true;
            this.cLabel4.BackColor = System.Drawing.Color.Transparent;
            this.cLabel4.Font = new System.Drawing.Font("Cambria", 13F);
            this.cLabel4.ForeColor = System.Drawing.Color.White;
            this.cLabel4.Location = new System.Drawing.Point(592, 26);
            this.cLabel4.Name = "cLabel4";
            this.cLabel4.Size = new System.Drawing.Size(137, 21);
            this.cLabel4.TabIndex = 25;
            this.cLabel4.Text = "Mr. Raj Vakadiya";
            this.cLabel4.ToolTips = "";
            // 
            // cLabel10
            // 
            this.cLabel10.AutoSize = true;
            this.cLabel10.BackColor = System.Drawing.Color.Transparent;
            this.cLabel10.Font = new System.Drawing.Font("Cambria", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel10.ForeColor = System.Drawing.Color.White;
            this.cLabel10.Location = new System.Drawing.Point(592, 57);
            this.cLabel10.Name = "cLabel10";
            this.cLabel10.Size = new System.Drawing.Size(146, 21);
            this.cLabel10.TabIndex = 25;
            this.cLabel10.Text = "+91-8000805300";
            this.cLabel10.ToolTips = "";
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::MahantExport.Properties.Resources.AXONE_Infotech_Final_01;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.cLabel4);
            this.panel1.Controls.Add(this.cLabel10);
            this.panel1.Controls.Add(this.cLabel5);
            this.panel1.Controls.Add(this.cLabel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(750, 353);
            this.panel1.TabIndex = 26;
            // 
            // cLabel5
            // 
            this.cLabel5.AutoSize = true;
            this.cLabel5.BackColor = System.Drawing.Color.Transparent;
            this.cLabel5.Font = new System.Drawing.Font("Cambria", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel5.ForeColor = System.Drawing.Color.White;
            this.cLabel5.Location = new System.Drawing.Point(194, 273);
            this.cLabel5.Name = "cLabel5";
            this.cLabel5.Size = new System.Drawing.Size(390, 21);
            this.cLabel5.TabIndex = 25;
            this.cLabel5.Text = "A210, RJD Business Hub, Nagina Wadi , Katargam";
            this.cLabel5.ToolTips = "";
            // 
            // cLabel1
            // 
            this.cLabel1.AutoSize = true;
            this.cLabel1.BackColor = System.Drawing.Color.Transparent;
            this.cLabel1.Font = new System.Drawing.Font("Cambria", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel1.ForeColor = System.Drawing.Color.White;
            this.cLabel1.Location = new System.Drawing.Point(286, 307);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(216, 21);
            this.cLabel1.TabIndex = 25;
            this.cLabel1.Text = "axoneinfotech@gmail.com";
            this.cLabel1.ToolTips = "";
            // 
            // FrmAboutUs
            // 
            this.Appearance.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.ClientSize = new System.Drawing.Size(750, 353);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.cLabel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("FrmAboutUs.IconOptions.Icon")));
            this.IconOptions.ShowIcon = false;
            this.Name = "FrmAboutUs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Tag = "AboutUs";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmLogin_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private AxonContLib.cLabel cLabel2;
        private AxonContLib.cLabel cLabel4;
        private AxonContLib.cLabel cLabel10;
        private AxonContLib.cPanel panel1;
        private AxonContLib.cLabel cLabel1;
        private AxonContLib.cLabel cLabel5;

    }
}