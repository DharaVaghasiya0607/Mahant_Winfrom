namespace MahantExport
{
    partial class FrmSearchPopupBoxMultipleSelect
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
            this.GrdDet = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.MainGrid = new DevExpress.XtraGrid.GridControl();
            this.cPanel1 = new AxonContLib.cPanel(this.components);
            this.BtnExit = new DevExpress.XtraEditors.SimpleButton();
            this.BtnSelect = new DevExpress.XtraEditors.SimpleButton();
            this.txtSelectedMeter = new AxonContLib.cTextBox(this.components);
            this.txtTotalMeter = new AxonContLib.cTextBox(this.components);
            this.txtSelectedTaka = new AxonContLib.cTextBox(this.components);
            this.txtTotalTaka = new AxonContLib.cTextBox(this.components);
            this.lblSelection = new AxonContLib.cLabel(this.components);
            this.lblTaka = new AxonContLib.cLabel(this.components);
            this.txtSeach = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).BeginInit();
            this.cPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GrdDet
            // 
            this.GrdDet.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.GrdDet.Appearance.FocusedCell.Font = new System.Drawing.Font("Verdana", 9F);
            this.GrdDet.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.GrdDet.Appearance.FocusedCell.Options.UseBackColor = true;
            this.GrdDet.Appearance.FocusedCell.Options.UseFont = true;
            this.GrdDet.Appearance.FocusedCell.Options.UseForeColor = true;
            this.GrdDet.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.GrdDet.Appearance.FocusedRow.Font = new System.Drawing.Font("Verdana", 9F);
            this.GrdDet.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.GrdDet.Appearance.FocusedRow.Options.UseBackColor = true;
            this.GrdDet.Appearance.FocusedRow.Options.UseFont = true;
            this.GrdDet.Appearance.FocusedRow.Options.UseForeColor = true;
            this.GrdDet.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.HeaderPanel.Options.UseFont = true;
            this.GrdDet.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.GrdDet.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDet.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.GrdDet.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.GrdDet.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.Black;
            this.GrdDet.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.GrdDet.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.GrdDet.Appearance.HorzLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDet.Appearance.HorzLine.Options.UseBackColor = true;
            this.GrdDet.Appearance.Row.Font = new System.Drawing.Font("Verdana", 9F);
            this.GrdDet.Appearance.Row.Options.UseFont = true;
            this.GrdDet.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.GrdDet.Appearance.SelectedRow.Font = new System.Drawing.Font("Verdana", 8F);
            this.GrdDet.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Black;
            this.GrdDet.Appearance.SelectedRow.Options.UseBackColor = true;
            this.GrdDet.Appearance.SelectedRow.Options.UseFont = true;
            this.GrdDet.Appearance.SelectedRow.Options.UseForeColor = true;
            this.GrdDet.Appearance.VertLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDet.Appearance.VertLine.Options.UseBackColor = true;
            this.GrdDet.ColumnPanelRowHeight = 30;
            this.GrdDet.DetailHeight = 431;
            this.GrdDet.GridControl = this.MainGrid;
            this.GrdDet.Name = "GrdDet";
            this.GrdDet.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.GrdDet.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.GrdDet.OptionsBehavior.Editable = false;
            this.GrdDet.OptionsBehavior.FocusLeaveOnTab = true;
            this.GrdDet.OptionsFilter.ShowAllTableValuesInFilterPopup = true;
            this.GrdDet.OptionsFilter.UseNewCustomFilterDialog = true;
            this.GrdDet.OptionsFind.FindDelay = 100;
            this.GrdDet.OptionsFind.FindFilterColumns = "";
            this.GrdDet.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.GrdDet.OptionsFind.SearchInPreview = true;
            this.GrdDet.OptionsNavigation.AutoFocusNewRow = true;
            this.GrdDet.OptionsNavigation.EnterMoveNextColumn = true;
            this.GrdDet.OptionsView.ColumnAutoWidth = false;
            this.GrdDet.OptionsView.ShowAutoFilterRow = true;
            this.GrdDet.OptionsView.ShowFooter = true;
            this.GrdDet.OptionsView.ShowGroupPanel = false;
            this.GrdDet.OptionsView.ShowPreviewRowLines = DevExpress.Utils.DefaultBoolean.True;
            this.GrdDet.RowHeight = 30;
            this.GrdDet.KeyUp += new System.Windows.Forms.KeyEventHandler(this.GrdDet_KeyUp);
            this.GrdDet.MouseUp += new System.Windows.Forms.MouseEventHandler(this.GrdDet_MouseUp);
            // 
            // MainGrid
            // 
            this.MainGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGrid.Location = new System.Drawing.Point(0, 28);
            this.MainGrid.MainView = this.GrdDet;
            this.MainGrid.Name = "MainGrid";
            this.MainGrid.Size = new System.Drawing.Size(965, 470);
            this.MainGrid.TabIndex = 3;
            this.MainGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GrdDet});
            this.MainGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainGrid_KeyDown);
            // 
            // cPanel1
            // 
            this.cPanel1.Controls.Add(this.BtnExit);
            this.cPanel1.Controls.Add(this.BtnSelect);
            this.cPanel1.Controls.Add(this.txtSelectedMeter);
            this.cPanel1.Controls.Add(this.txtTotalMeter);
            this.cPanel1.Controls.Add(this.txtSelectedTaka);
            this.cPanel1.Controls.Add(this.txtTotalTaka);
            this.cPanel1.Controls.Add(this.lblSelection);
            this.cPanel1.Controls.Add(this.lblTaka);
            this.cPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.cPanel1.Location = new System.Drawing.Point(0, 498);
            this.cPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cPanel1.Name = "cPanel1";
            this.cPanel1.Size = new System.Drawing.Size(965, 41);
            this.cPanel1.TabIndex = 5;
            // 
            // BtnExit
            // 
            this.BtnExit.Appearance.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.BtnExit.Appearance.Options.UseFont = true;
            this.BtnExit.Appearance.Options.UseTextOptions = true;
            this.BtnExit.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.BtnExit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnExit.Location = new System.Drawing.Point(127, 6);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(97, 29);
            this.BtnExit.TabIndex = 132;
            this.BtnExit.Text = "Exit";
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // BtnSelect
            // 
            this.BtnSelect.Appearance.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.BtnSelect.Appearance.Options.UseFont = true;
            this.BtnSelect.Appearance.Options.UseTextOptions = true;
            this.BtnSelect.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            this.BtnSelect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.BtnSelect.Location = new System.Drawing.Point(27, 6);
            this.BtnSelect.Name = "BtnSelect";
            this.BtnSelect.Size = new System.Drawing.Size(97, 29);
            this.BtnSelect.TabIndex = 11;
            this.BtnSelect.Text = "Select";
            this.BtnSelect.Click += new System.EventHandler(this.BtnSelect_Click);
            // 
            // txtSelectedMeter
            // 
            this.txtSelectedMeter.ActivationColor = true;
            this.txtSelectedMeter.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtSelectedMeter.AllowTabKeyOnEnter = true;
            this.txtSelectedMeter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.txtSelectedMeter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSelectedMeter.Font = new System.Drawing.Font("Calibri Light", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSelectedMeter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSelectedMeter.Format = "";
            this.txtSelectedMeter.IsComplusory = false;
            this.txtSelectedMeter.Location = new System.Drawing.Point(726, 8);
            this.txtSelectedMeter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSelectedMeter.Name = "txtSelectedMeter";
            this.txtSelectedMeter.ReadOnly = true;
            this.txtSelectedMeter.SelectAllTextOnFocus = true;
            this.txtSelectedMeter.Size = new System.Drawing.Size(126, 25);
            this.txtSelectedMeter.TabIndex = 128;
            this.txtSelectedMeter.TabStop = false;
            this.txtSelectedMeter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSelectedMeter.ToolTips = "";
            this.txtSelectedMeter.WaterMarkText = null;
            // 
            // txtTotalMeter
            // 
            this.txtTotalMeter.ActivationColor = true;
            this.txtTotalMeter.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtTotalMeter.AllowTabKeyOnEnter = true;
            this.txtTotalMeter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.txtTotalMeter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalMeter.Font = new System.Drawing.Font("Calibri Light", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalMeter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTotalMeter.Format = "";
            this.txtTotalMeter.IsComplusory = false;
            this.txtTotalMeter.Location = new System.Drawing.Point(371, 8);
            this.txtTotalMeter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTotalMeter.Name = "txtTotalMeter";
            this.txtTotalMeter.ReadOnly = true;
            this.txtTotalMeter.SelectAllTextOnFocus = true;
            this.txtTotalMeter.Size = new System.Drawing.Size(91, 25);
            this.txtTotalMeter.TabIndex = 129;
            this.txtTotalMeter.TabStop = false;
            this.txtTotalMeter.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTotalMeter.ToolTips = "";
            this.txtTotalMeter.WaterMarkText = null;
            // 
            // txtSelectedTaka
            // 
            this.txtSelectedTaka.ActivationColor = true;
            this.txtSelectedTaka.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtSelectedTaka.AllowTabKeyOnEnter = true;
            this.txtSelectedTaka.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.txtSelectedTaka.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSelectedTaka.Font = new System.Drawing.Font("Calibri Light", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSelectedTaka.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtSelectedTaka.Format = "";
            this.txtSelectedTaka.IsComplusory = false;
            this.txtSelectedTaka.Location = new System.Drawing.Point(635, 8);
            this.txtSelectedTaka.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSelectedTaka.Name = "txtSelectedTaka";
            this.txtSelectedTaka.ReadOnly = true;
            this.txtSelectedTaka.SelectAllTextOnFocus = true;
            this.txtSelectedTaka.Size = new System.Drawing.Size(91, 25);
            this.txtSelectedTaka.TabIndex = 130;
            this.txtSelectedTaka.TabStop = false;
            this.txtSelectedTaka.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSelectedTaka.ToolTips = "";
            this.txtSelectedTaka.WaterMarkText = null;
            // 
            // txtTotalTaka
            // 
            this.txtTotalTaka.ActivationColor = true;
            this.txtTotalTaka.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtTotalTaka.AllowTabKeyOnEnter = true;
            this.txtTotalTaka.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.txtTotalTaka.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTotalTaka.Font = new System.Drawing.Font("Calibri Light", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotalTaka.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.txtTotalTaka.Format = "";
            this.txtTotalTaka.IsComplusory = false;
            this.txtTotalTaka.Location = new System.Drawing.Point(281, 8);
            this.txtTotalTaka.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTotalTaka.Name = "txtTotalTaka";
            this.txtTotalTaka.ReadOnly = true;
            this.txtTotalTaka.SelectAllTextOnFocus = true;
            this.txtTotalTaka.Size = new System.Drawing.Size(91, 25);
            this.txtTotalTaka.TabIndex = 131;
            this.txtTotalTaka.TabStop = false;
            this.txtTotalTaka.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtTotalTaka.ToolTips = "";
            this.txtTotalTaka.WaterMarkText = null;
            // 
            // lblSelection
            // 
            this.lblSelection.AutoSize = true;
            this.lblSelection.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelection.ForeColor = System.Drawing.Color.Black;
            this.lblSelection.Location = new System.Drawing.Point(568, 13);
            this.lblSelection.Name = "lblSelection";
            this.lblSelection.Size = new System.Drawing.Size(63, 13);
            this.lblSelection.TabIndex = 126;
            this.lblSelection.Text = "Selected";
            this.lblSelection.ToolTips = "";
            // 
            // lblTaka
            // 
            this.lblTaka.AutoSize = true;
            this.lblTaka.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTaka.ForeColor = System.Drawing.Color.Black;
            this.lblTaka.Location = new System.Drawing.Point(239, 13);
            this.lblTaka.Name = "lblTaka";
            this.lblTaka.Size = new System.Drawing.Size(39, 13);
            this.lblTaka.TabIndex = 127;
            this.lblTaka.Text = "Taka";
            this.lblTaka.ToolTips = "";
            // 
            // txtSeach
            // 
            this.txtSeach.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSeach.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSeach.Font = new System.Drawing.Font("Cambria", 13F);
            this.txtSeach.Location = new System.Drawing.Point(0, 0);
            this.txtSeach.Name = "txtSeach";
            this.txtSeach.Size = new System.Drawing.Size(965, 28);
            this.txtSeach.TabIndex = 1;
            this.txtSeach.Visible = false;
            this.txtSeach.TextChanged += new System.EventHandler(this.txtSeach_TextChanged);
            // 
            // FrmSearchPopupBoxMultipleSelect
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(965, 539);
            this.Controls.Add(this.MainGrid);
            this.Controls.Add(this.cPanel1);
            this.Controls.Add(this.txtSeach);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.LookAndFeel.SkinName = "London Liquid Sky";
            this.LookAndFeel.UseDefaultLookAndFeel =true;
            this.Name = "FrmSearchPopupBoxMultipleSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PLEASE SELECT MULTIPLE RECORDS";
            this.Load += new System.EventHandler(this.FrmSearch_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSearch_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).EndInit();
            this.cPanel1.ResumeLayout(false);
            this.cPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public DevExpress.XtraGrid.Views.Grid.GridView GrdDet;
        public DevExpress.XtraGrid.GridControl MainGrid;
        private AxonContLib.cPanel cPanel1;
        private AxonContLib.cTextBox txtSelectedMeter;
        private AxonContLib.cTextBox txtTotalMeter;
        private AxonContLib.cTextBox txtSelectedTaka;
        private AxonContLib.cTextBox txtTotalTaka;
        private AxonContLib.cLabel lblSelection;
        private AxonContLib.cLabel lblTaka;
        private System.Windows.Forms.TextBox txtSeach;
        private DevExpress.XtraEditors.SimpleButton BtnExit;
        private DevExpress.XtraEditors.SimpleButton BtnSelect;



    }
}