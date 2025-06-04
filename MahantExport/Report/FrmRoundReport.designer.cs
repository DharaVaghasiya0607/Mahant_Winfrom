namespace MahantExport.Report
{
    partial class FrmRoundReport
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
            DevExpress.XtraEditors.Controls.EditorButtonImageOptions editorButtonImageOptions1 = new DevExpress.XtraEditors.Controls.EditorButtonImageOptions();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject1 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject2 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject3 = new DevExpress.Utils.SerializableAppearanceObject();
            DevExpress.Utils.SerializableAppearanceObject serializableAppearanceObject4 = new DevExpress.Utils.SerializableAppearanceObject();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRoundReport));
            this.CmbKapan = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.BtnRejectionTransfer = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
            this.cLabel2 = new AxonContLib.cLabel(this.components);
            this.txtStoneMFGComparision = new AxonContLib.cTextBox(this.components);
            this.cLabel6 = new AxonContLib.cLabel(this.components);
            this.BtnExit = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnRoundReportExcelExport = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnClear = new DevControlLib.cDevSimpleButton(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.CmbKapan.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.BtnRejectionTransfer)).BeginInit();
            this.SuspendLayout();
            // 
            // CmbKapan
            // 
            this.CmbKapan.EditValue = "";
            this.CmbKapan.Location = new System.Drawing.Point(28, 40);
            this.CmbKapan.Name = "CmbKapan";
            this.CmbKapan.Properties.AllowMultiSelect = true;
            this.CmbKapan.Properties.Appearance.Font = new System.Drawing.Font("Verdana", 9F);
            this.CmbKapan.Properties.Appearance.Options.UseFont = true;
            this.CmbKapan.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Verdana", 9F);
            this.CmbKapan.Properties.AppearanceDropDown.Options.UseFont = true;
            this.CmbKapan.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.CmbKapan.Properties.DropDownRows = 20;
            this.CmbKapan.Properties.IncrementalSearch = true;
            this.CmbKapan.Size = new System.Drawing.Size(400, 20);
            this.CmbKapan.TabIndex = 1;
            // 
            // BtnRejectionTransfer
            // 
            this.BtnRejectionTransfer.AutoHeight = false;
            serializableAppearanceObject1.Font = new System.Drawing.Font("Verdana", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            serializableAppearanceObject1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            serializableAppearanceObject1.Options.UseFont = true;
            serializableAppearanceObject1.Options.UseForeColor = true;
            serializableAppearanceObject1.Options.UseTextOptions = true;
            serializableAppearanceObject1.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            serializableAppearanceObject2.Font = new System.Drawing.Font("Verdana", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            serializableAppearanceObject2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            serializableAppearanceObject2.Options.UseFont = true;
            serializableAppearanceObject2.Options.UseForeColor = true;
            serializableAppearanceObject2.Options.UseTextOptions = true;
            serializableAppearanceObject2.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            serializableAppearanceObject3.Font = new System.Drawing.Font("Verdana", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            serializableAppearanceObject3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            serializableAppearanceObject3.Options.UseFont = true;
            serializableAppearanceObject3.Options.UseForeColor = true;
            serializableAppearanceObject3.Options.UseTextOptions = true;
            serializableAppearanceObject3.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            serializableAppearanceObject4.Font = new System.Drawing.Font("Verdana", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            serializableAppearanceObject4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            serializableAppearanceObject4.Options.UseFont = true;
            serializableAppearanceObject4.Options.UseForeColor = true;
            serializableAppearanceObject4.Options.UseTextOptions = true;
            serializableAppearanceObject4.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.BtnRejectionTransfer.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Glyph, "Rej. x\'fer", -1, true, true, false, editorButtonImageOptions1, new DevExpress.Utils.KeyShortcut(System.Windows.Forms.Keys.None), serializableAppearanceObject1, serializableAppearanceObject2, serializableAppearanceObject3, serializableAppearanceObject4, "", null, null, DevExpress.Utils.ToolTipAnchor.Default)});
            this.BtnRejectionTransfer.Name = "BtnRejectionTransfer";
            this.BtnRejectionTransfer.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.HideTextEditor;
            // 
            // cLabel2
            // 
            this.cLabel2.AutoSize = true;
            this.cLabel2.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.cLabel2.ForeColor = System.Drawing.Color.Black;
            this.cLabel2.Location = new System.Drawing.Point(25, 19);
            this.cLabel2.Name = "cLabel2";
            this.cLabel2.Size = new System.Drawing.Size(48, 14);
            this.cLabel2.TabIndex = 0;
            this.cLabel2.Text = "Kapan";
            this.cLabel2.ToolTips = "";
            // 
            // txtStoneMFGComparision
            // 
            this.txtStoneMFGComparision.ActivationColor = true;
            this.txtStoneMFGComparision.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtStoneMFGComparision.AllowTabKeyOnEnter = false;
            this.txtStoneMFGComparision.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStoneMFGComparision.Font = new System.Drawing.Font("Verdana", 8F);
            this.txtStoneMFGComparision.Format = "";
            this.txtStoneMFGComparision.IsComplusory = false;
            this.txtStoneMFGComparision.Location = new System.Drawing.Point(28, 95);
            this.txtStoneMFGComparision.Multiline = true;
            this.txtStoneMFGComparision.Name = "txtStoneMFGComparision";
            this.txtStoneMFGComparision.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStoneMFGComparision.SelectAllTextOnFocus = true;
            this.txtStoneMFGComparision.Size = new System.Drawing.Size(400, 56);
            this.txtStoneMFGComparision.TabIndex = 227;
            this.txtStoneMFGComparision.ToolTips = "";
            this.txtStoneMFGComparision.WaterMarkText = null;
            this.txtStoneMFGComparision.TextChanged += new System.EventHandler(this.txtStoneMFGComparision_TextChanged);
            // 
            // cLabel6
            // 
            this.cLabel6.AutoSize = true;
            this.cLabel6.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel6.ForeColor = System.Drawing.Color.Black;
            this.cLabel6.Location = new System.Drawing.Point(25, 75);
            this.cLabel6.Name = "cLabel6";
            this.cLabel6.Size = new System.Drawing.Size(65, 13);
            this.cLabel6.TabIndex = 226;
            this.cLabel6.Text = "Stone No";
            this.cLabel6.ToolTips = "";
            // 
            // BtnExit
            // 
            this.BtnExit.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnExit.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnExit.Appearance.Options.UseFont = true;
            this.BtnExit.Appearance.Options.UseForeColor = true;
            this.BtnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnExit.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnExit.ImageOptions.SvgImage")));
            this.BtnExit.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnExit.Location = new System.Drawing.Point(578, 95);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(75, 32);
            this.BtnExit.TabIndex = 228;
            this.BtnExit.Text = "Exit";
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // BtnRoundReportExcelExport
            // 
            this.BtnRoundReportExcelExport.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnRoundReportExcelExport.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnRoundReportExcelExport.Appearance.Options.UseFont = true;
            this.BtnRoundReportExcelExport.Appearance.Options.UseForeColor = true;
            this.BtnRoundReportExcelExport.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnRoundReportExcelExport.ImageOptions.SvgImage")));
            this.BtnRoundReportExcelExport.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnRoundReportExcelExport.Location = new System.Drawing.Point(494, 35);
            this.BtnRoundReportExcelExport.Name = "BtnRoundReportExcelExport";
            this.BtnRoundReportExcelExport.Size = new System.Drawing.Size(159, 31);
            this.BtnRoundReportExcelExport.TabIndex = 229;
            this.BtnRoundReportExcelExport.Text = "Round Report Excel";
            this.BtnRoundReportExcelExport.Click += new System.EventHandler(this.BtnRoundReportExcelExport_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnClear.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnClear.Appearance.Options.UseFont = true;
            this.BtnClear.Appearance.Options.UseForeColor = true;
            this.BtnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnClear.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnClear.ImageOptions.SvgImage")));
            this.BtnClear.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnClear.Location = new System.Drawing.Point(494, 95);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(75, 32);
            this.BtnClear.TabIndex = 230;
            this.BtnClear.Text = "Clear";
            this.BtnClear.Click += new System.EventHandler(this.BtnClear_Click);
            // 
            // FrmRoundReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 173);
            this.Controls.Add(this.BtnClear);
            this.Controls.Add(this.BtnRoundReportExcelExport);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.txtStoneMFGComparision);
            this.Controls.Add(this.cLabel6);
            this.Controls.Add(this.cLabel2);
            this.Controls.Add(this.CmbKapan);
            this.Name = "FrmRoundReport";
            this.Tag = "RoundReport";
            this.Text = "ROUND REPORT";
            ((System.ComponentModel.ISupportInitialize)(this.CmbKapan.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.BtnRejectionTransfer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit BtnRejectionTransfer;
        private DevExpress.XtraEditors.CheckedComboBoxEdit CmbKapan;
        private AxonContLib.cLabel cLabel2;
        private AxonContLib.cTextBox txtStoneMFGComparision;
        private AxonContLib.cLabel cLabel6;
        private DevControlLib.cDevSimpleButton BtnExit;
        private DevControlLib.cDevSimpleButton BtnRoundReportExcelExport;
        private DevControlLib.cDevSimpleButton BtnClear;


    }
}