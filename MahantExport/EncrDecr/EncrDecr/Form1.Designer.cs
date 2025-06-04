namespace EncrDecr
{
    partial class Form1
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
            this.txtPlainText = new System.Windows.Forms.TextBox();
            this.txtConverted = new System.Windows.Forms.TextBox();
            this.BtnEnrypt = new System.Windows.Forms.Button();
            this.BtnDecrypt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtPlainText
            // 
            this.txtPlainText.Location = new System.Drawing.Point(33, 22);
            this.txtPlainText.Multiline = true;
            this.txtPlainText.Name = "txtPlainText";
            this.txtPlainText.Size = new System.Drawing.Size(507, 90);
            this.txtPlainText.TabIndex = 0;
            // 
            // txtConverted
            // 
            this.txtConverted.Location = new System.Drawing.Point(33, 186);
            this.txtConverted.Multiline = true;
            this.txtConverted.Name = "txtConverted";
            this.txtConverted.Size = new System.Drawing.Size(507, 90);
            this.txtConverted.TabIndex = 0;
            // 
            // BtnEnrypt
            // 
            this.BtnEnrypt.Location = new System.Drawing.Point(33, 137);
            this.BtnEnrypt.Name = "BtnEnrypt";
            this.BtnEnrypt.Size = new System.Drawing.Size(75, 23);
            this.BtnEnrypt.TabIndex = 1;
            this.BtnEnrypt.Text = "Encrypt";
            this.BtnEnrypt.UseVisualStyleBackColor = true;
            this.BtnEnrypt.Click += new System.EventHandler(this.BtnEnrypt_Click);
            // 
            // BtnDecrypt
            // 
            this.BtnDecrypt.Location = new System.Drawing.Point(154, 137);
            this.BtnDecrypt.Name = "BtnDecrypt";
            this.BtnDecrypt.Size = new System.Drawing.Size(75, 23);
            this.BtnDecrypt.TabIndex = 1;
            this.BtnDecrypt.Text = "Decrypt";
            this.BtnDecrypt.UseVisualStyleBackColor = true;
            this.BtnDecrypt.Click += new System.EventHandler(this.BtnDecrypt_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 334);
            this.Controls.Add(this.BtnDecrypt);
            this.Controls.Add(this.BtnEnrypt);
            this.Controls.Add(this.txtConverted);
            this.Controls.Add(this.txtPlainText);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPlainText;
        private System.Windows.Forms.TextBox txtConverted;
        private System.Windows.Forms.Button BtnEnrypt;
        private System.Windows.Forms.Button BtnDecrypt;
    }
}

