namespace MahantExport
{
    partial class FrmPartyMerge
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPartyMerge));
            this.BtnTrnsfer = new System.Windows.Forms.Button();
            this.cLabel11 = new AxonContLib.cLabel(this.components);
            this.txtFromParty = new AxonContLib.cTextBox(this.components);
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            this.txtToParty = new AxonContLib.cTextBox(this.components);
            this.btnClear = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1 = new AxonContLib.cPanel(this.components);
            this.panel2 = new AxonContLib.cPanel(this.components);
            this.MainGrid = new DevExpress.XtraGrid.GridControl();
            this.GrdDet = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn23 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn12 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).BeginInit();
            this.SuspendLayout();
            // 
            // BtnTrnsfer
            // 
            this.BtnTrnsfer.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnTrnsfer.Location = new System.Drawing.Point(21, 81);
            this.BtnTrnsfer.Name = "BtnTrnsfer";
            this.BtnTrnsfer.Size = new System.Drawing.Size(79, 37);
            this.BtnTrnsfer.TabIndex = 12;
            this.BtnTrnsfer.Text = "Trnasfer";
            this.BtnTrnsfer.UseVisualStyleBackColor = true;
            this.BtnTrnsfer.Click += new System.EventHandler(this.BtnTrnsfer_Click);
            // 
            // cLabel11
            // 
            this.cLabel11.AutoSize = true;
            this.cLabel11.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel11.ForeColor = System.Drawing.Color.Black;
            this.cLabel11.Location = new System.Drawing.Point(12, 15);
            this.cLabel11.Name = "cLabel11";
            this.cLabel11.Size = new System.Drawing.Size(85, 17);
            this.cLabel11.TabIndex = 13;
            this.cLabel11.Text = "From Party";
            this.cLabel11.ToolTips = "";
            // 
            // txtFromParty
            // 
            this.txtFromParty.ActivationColor = true;
            this.txtFromParty.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtFromParty.AllowTabKeyOnEnter = false;
            this.txtFromParty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFromParty.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFromParty.Format = "";
            this.txtFromParty.IsComplusory = false;
            this.txtFromParty.Location = new System.Drawing.Point(99, 11);
            this.txtFromParty.MaxLength = 100;
            this.txtFromParty.Name = "txtFromParty";
            this.txtFromParty.SelectAllTextOnFocus = true;
            this.txtFromParty.Size = new System.Drawing.Size(171, 24);
            this.txtFromParty.TabIndex = 14;
            this.txtFromParty.ToolTips = "";
            this.txtFromParty.WaterMarkText = null;
            this.txtFromParty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFromParty_KeyPress);
            // 
            // cLabel1
            // 
            this.cLabel1.AutoSize = true;
            this.cLabel1.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel1.ForeColor = System.Drawing.Color.Black;
            this.cLabel1.Location = new System.Drawing.Point(73, 47);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(24, 17);
            this.cLabel1.TabIndex = 15;
            this.cLabel1.Text = "To";
            this.cLabel1.ToolTips = "";
            // 
            // txtToParty
            // 
            this.txtToParty.ActivationColor = true;
            this.txtToParty.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtToParty.AllowTabKeyOnEnter = false;
            this.txtToParty.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtToParty.Font = new System.Drawing.Font("Verdana", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtToParty.Format = "";
            this.txtToParty.IsComplusory = false;
            this.txtToParty.Location = new System.Drawing.Point(99, 43);
            this.txtToParty.MaxLength = 100;
            this.txtToParty.Name = "txtToParty";
            this.txtToParty.SelectAllTextOnFocus = true;
            this.txtToParty.Size = new System.Drawing.Size(171, 24);
            this.txtToParty.TabIndex = 16;
            this.txtToParty.ToolTips = "";
            this.txtToParty.WaterMarkText = null;
            this.txtToParty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtToParty_KeyPress);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(106, 81);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(79, 37);
            this.btnClear.TabIndex = 17;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnExit
            // 
            this.btnExit.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Location = new System.Drawing.Point(191, 81);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(79, 37);
            this.btnExit.TabIndex = 18;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cLabel11);
            this.panel1.Controls.Add(this.btnExit);
            this.panel1.Controls.Add(this.txtFromParty);
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.txtToParty);
            this.panel1.Controls.Add(this.BtnTrnsfer);
            this.panel1.Controls.Add(this.cLabel1);
            this.panel1.Location = new System.Drawing.Point(0, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(282, 124);
            this.panel1.TabIndex = 19;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.MainGrid);
            this.panel2.Location = new System.Drawing.Point(0, 130);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(282, 220);
            this.panel2.TabIndex = 20;
            // 
            // MainGrid
            // 
            this.MainGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGrid.Location = new System.Drawing.Point(0, 0);
            this.MainGrid.MainView = this.GrdDet;
            this.MainGrid.Name = "MainGrid";
            this.MainGrid.Size = new System.Drawing.Size(282, 220);
            this.MainGrid.TabIndex = 40;
            this.MainGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GrdDet});
            // 
            // GrdDet
            // 
            this.GrdDet.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.GrdDet.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(244)))), ((int)(((byte)(191)))));
            this.GrdDet.Appearance.FocusedCell.Font = new System.Drawing.Font("Verdana", 8F);
            this.GrdDet.Appearance.FocusedCell.Options.UseBackColor = true;
            this.GrdDet.Appearance.FocusedCell.Options.UseFont = true;
            this.GrdDet.Appearance.FocusedRow.Font = new System.Drawing.Font("Verdana", 8F);
            this.GrdDet.Appearance.FocusedRow.Options.UseFont = true;
            this.GrdDet.Appearance.FooterPanel.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.FooterPanel.Options.UseFont = true;
            this.GrdDet.Appearance.GroupFooter.Font = new System.Drawing.Font("Verdana", 7F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.GroupFooter.Options.UseFont = true;
            this.GrdDet.Appearance.GroupRow.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.GroupRow.ForeColor = System.Drawing.Color.Teal;
            this.GrdDet.Appearance.GroupRow.Options.UseFont = true;
            this.GrdDet.Appearance.GroupRow.Options.UseForeColor = true;
            this.GrdDet.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.HeaderPanel.Options.UseFont = true;
            this.GrdDet.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.GrdDet.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDet.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.GrdDet.Appearance.Row.Font = new System.Drawing.Font("Verdana", 8F);
            this.GrdDet.Appearance.Row.Options.UseFont = true;
            this.GrdDet.Appearance.Row.Options.UseTextOptions = true;
            this.GrdDet.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDet.AppearancePrint.HeaderPanel.Font = new System.Drawing.Font("Tahoma", 13F);
            this.GrdDet.AppearancePrint.HeaderPanel.Options.UseFont = true;
            this.GrdDet.ColumnPanelRowHeight = 40;
            this.GrdDet.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn23,
            this.gridColumn12,
            this.gridColumn1});
            this.GrdDet.GridControl = this.MainGrid;
            this.GrdDet.Name = "GrdDet";
            this.GrdDet.OptionsBehavior.Editable = false;
            this.GrdDet.OptionsCustomization.AllowSort = false;
            this.GrdDet.OptionsFilter.AllowFilterEditor = false;
            this.GrdDet.OptionsPrint.ExpandAllGroups = false;
            this.GrdDet.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.GrdDet.OptionsView.ColumnAutoWidth = false;
            this.GrdDet.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.GrdDet.OptionsView.ShowFooter = true;
            this.GrdDet.OptionsView.ShowGroupPanel = false;
            this.GrdDet.RowHeight = 25;
            // 
            // gridColumn23
            // 
            this.gridColumn23.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn23.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn23.AppearanceCell.Options.UseFont = true;
            this.gridColumn23.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn23.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn23.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn23.AppearanceCell.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.gridColumn23.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn23.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn23.AppearanceHeader.Options.UseFont = true;
            this.gridColumn23.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn23.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn23.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn23.Caption = "PROCESS_ID";
            this.gridColumn23.FieldName = "PROCESS_ID";
            this.gridColumn23.Name = "gridColumn23";
            this.gridColumn23.OptionsColumn.AllowEdit = false;
            this.gridColumn23.Width = 206;
            // 
            // gridColumn12
            // 
            this.gridColumn12.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn12.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn12.AppearanceCell.Options.UseFont = true;
            this.gridColumn12.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn12.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn12.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn12.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn12.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn12.AppearanceHeader.Options.UseFont = true;
            this.gridColumn12.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn12.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn12.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn12.Caption = "Process";
            this.gridColumn12.FieldName = "PROCESSNAME";
            this.gridColumn12.Name = "gridColumn12";
            this.gridColumn12.Visible = true;
            this.gridColumn12.VisibleIndex = 0;
            this.gridColumn12.Width = 159;
            // 
            // gridColumn1
            // 
            this.gridColumn1.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn1.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn1.AppearanceCell.Options.UseFont = true;
            this.gridColumn1.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn1.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn1.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn1.AppearanceHeader.Options.UseFont = true;
            this.gridColumn1.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn1.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn1.Caption = "Total";
            this.gridColumn1.FieldName = "TOTALPROCESS";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            this.gridColumn1.Width = 57;
            // 
            // FrmPartyMerge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(285, 353);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("FrmPartyMerge.IconOptions.Icon")));
            this.Name = "FrmPartyMerge";
            this.Tag = "PartyMerge\r\n";
            this.Text = "PARTY MERGE";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnTrnsfer;
        private AxonContLib.cLabel cLabel11;
        private AxonContLib.cTextBox txtFromParty;
        private AxonContLib.cLabel cLabel1;
        private AxonContLib.cTextBox txtToParty;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnExit;
        private AxonContLib.cPanel panel1;
        private AxonContLib.cPanel panel2;
        private DevExpress.XtraGrid.GridControl MainGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView GrdDet;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn23;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn12;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
    }
}

