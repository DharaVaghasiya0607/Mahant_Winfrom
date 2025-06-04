namespace MahantExport
{
    partial class FrmSearchPopupBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSearchPopupBox));
            this.txtSeach = new System.Windows.Forms.TextBox();
            this.GrdDet = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.MainGrid = new DevExpress.XtraGrid.GridControl();
            this.panel1 = new AxonContLib.cPanel(this.components);
            this.lblExport = new AxonContLib.cLabel(this.components);
            this.cLabel1 = new AxonContLib.cLabel(this.components);
            this.panel3 = new AxonContLib.cPanel(this.components);
            this.panel4 = new AxonContLib.cPanel(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSeach
            // 
            this.txtSeach.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtSeach.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSeach.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSeach.Font = new System.Drawing.Font("Cambria", 12F);
            this.txtSeach.ForeColor = System.Drawing.Color.White;
            this.txtSeach.Location = new System.Drawing.Point(10, 22);
            this.txtSeach.Name = "txtSeach";
            this.txtSeach.Size = new System.Drawing.Size(483, 26);
            this.txtSeach.TabIndex = 1;
            this.txtSeach.TextChanged += new System.EventHandler(this.txtSeach_TextChanged);
            // 
            // GrdDet
            // 
            this.GrdDet.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.GrdDet.Appearance.FocusedCell.Font = new System.Drawing.Font("Verdana", 8F);
            this.GrdDet.Appearance.FocusedCell.ForeColor = System.Drawing.Color.Black;
            this.GrdDet.Appearance.FocusedCell.Options.UseBackColor = true;
            this.GrdDet.Appearance.FocusedCell.Options.UseFont = true;
            this.GrdDet.Appearance.FocusedCell.Options.UseForeColor = true;
            this.GrdDet.Appearance.FocusedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.GrdDet.Appearance.FocusedRow.Font = new System.Drawing.Font("Verdana", 8F);
            this.GrdDet.Appearance.FocusedRow.ForeColor = System.Drawing.Color.Black;
            this.GrdDet.Appearance.FocusedRow.Options.UseBackColor = true;
            this.GrdDet.Appearance.FocusedRow.Options.UseFont = true;
            this.GrdDet.Appearance.FocusedRow.Options.UseForeColor = true;
            this.GrdDet.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.GrdDet.Appearance.HeaderPanel.Options.UseFont = true;
            this.GrdDet.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.GrdDet.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GrdDet.Appearance.HideSelectionRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.GrdDet.Appearance.HideSelectionRow.ForeColor = System.Drawing.Color.Black;
            this.GrdDet.Appearance.HideSelectionRow.Options.UseBackColor = true;
            this.GrdDet.Appearance.HideSelectionRow.Options.UseForeColor = true;
            this.GrdDet.Appearance.HorzLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDet.Appearance.HorzLine.Options.UseBackColor = true;
            this.GrdDet.Appearance.Row.Font = new System.Drawing.Font("Verdana", 8F);
            this.GrdDet.Appearance.Row.Options.UseFont = true;
            this.GrdDet.Appearance.SelectedRow.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.GrdDet.Appearance.SelectedRow.Font = new System.Drawing.Font("Verdana", 8F);
            this.GrdDet.Appearance.SelectedRow.ForeColor = System.Drawing.Color.Black;
            this.GrdDet.Appearance.SelectedRow.Options.UseBackColor = true;
            this.GrdDet.Appearance.SelectedRow.Options.UseFont = true;
            this.GrdDet.Appearance.SelectedRow.Options.UseForeColor = true;
            this.GrdDet.Appearance.VertLine.BackColor = System.Drawing.Color.Gray;
            this.GrdDet.Appearance.VertLine.Options.UseBackColor = true;
            this.GrdDet.GridControl = this.MainGrid;
            this.GrdDet.Name = "GrdDet";
            this.GrdDet.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.True;
            this.GrdDet.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.True;
            this.GrdDet.OptionsBehavior.Editable = false;
            this.GrdDet.OptionsBehavior.FocusLeaveOnTab = true;
            this.GrdDet.OptionsFind.FindDelay = 100;
            this.GrdDet.OptionsFind.FindFilterColumns = "";
            this.GrdDet.OptionsFind.FindMode = DevExpress.XtraEditors.FindMode.Always;
            this.GrdDet.OptionsFind.HighlightFindResults = false;
            this.GrdDet.OptionsFind.SearchInPreview = true;
            this.GrdDet.OptionsFind.ShowCloseButton = false;
            this.GrdDet.OptionsNavigation.AutoFocusNewRow = true;
            this.GrdDet.OptionsNavigation.EnterMoveNextColumn = true;
            this.GrdDet.OptionsView.ColumnAutoWidth = false;
            this.GrdDet.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.ShowAlways;
            this.GrdDet.OptionsView.ShowFooter = true;
            this.GrdDet.OptionsView.ShowGroupPanel = false;
            this.GrdDet.OptionsView.ShowIndicator = false;
            this.GrdDet.OptionsView.ShowPreviewRowLines = DevExpress.Utils.DefaultBoolean.False;
            this.GrdDet.RowHeight = 22;
            this.GrdDet.RowSeparatorHeight = 2;
            this.GrdDet.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.GrdDet_RowClick);
            this.GrdDet.CellValueChanging += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.GrdDet_CellValueChanging);
            this.GrdDet.KeyDown += new System.Windows.Forms.KeyEventHandler(this.GrdDet_KeyDown);
            // 
            // MainGrid
            // 
            this.MainGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGrid.Location = new System.Drawing.Point(10, 48);
            this.MainGrid.MainView = this.GrdDet;
            this.MainGrid.Name = "MainGrid";
            this.MainGrid.Size = new System.Drawing.Size(483, 423);
            this.MainGrid.TabIndex = 3;
            this.MainGrid.UseDisabledStatePainter = false;
            this.MainGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GrdDet});
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.lblExport);
            this.panel1.Controls.Add(this.cLabel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(503, 22);
            this.panel1.TabIndex = 0;
            // 
            // lblExport
            // 
            this.lblExport.AutoSize = true;
            this.lblExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblExport.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblExport.ForeColor = System.Drawing.Color.White;
            this.lblExport.Location = new System.Drawing.Point(271, 5);
            this.lblExport.Name = "lblExport";
            this.lblExport.Size = new System.Drawing.Size(78, 13);
            this.lblExport.TabIndex = 0;
            this.lblExport.Text = "Export Excel";
            this.lblExport.ToolTips = "";
            this.lblExport.Click += new System.EventHandler(this.lblExport_Click);
            // 
            // cLabel1
            // 
            this.cLabel1.AutoSize = true;
            this.cLabel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cLabel1.ForeColor = System.Drawing.Color.White;
            this.cLabel1.Location = new System.Drawing.Point(12, 4);
            this.cLabel1.Name = "cLabel1";
            this.cLabel1.Size = new System.Drawing.Size(130, 13);
            this.cLabel1.TabIndex = 0;
            this.cLabel1.Text = "Type to search / filter";
            this.cLabel1.ToolTips = "";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 22);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(10, 449);
            this.panel3.TabIndex = 4;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(493, 22);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(10, 449);
            this.panel4.TabIndex = 5;
            // 
            // FrmSearchPopupBox
            // 
            this.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.Appearance.Options.UseBackColor = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 471);
            this.Controls.Add(this.MainGrid);
            this.Controls.Add(this.txtSeach);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("FrmSearchPopupBox.IconOptions.Icon")));
            this.KeyPreview = true;
            this.Name = "FrmSearchPopupBox";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PLEASE SELECT ANY OF ONE RECORD";
            this.Load += new System.EventHandler(this.FrmSearch_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmSearch_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.GrdDet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSeach;
        public DevExpress.XtraGrid.Views.Grid.GridView GrdDet;
        public DevExpress.XtraGrid.GridControl MainGrid;
        private AxonContLib.cPanel panel1;
        private AxonContLib.cPanel panel3;
        private AxonContLib.cPanel panel4;
        private AxonContLib.cLabel cLabel1;
        private AxonContLib.cLabel lblExport;

      

    }
}