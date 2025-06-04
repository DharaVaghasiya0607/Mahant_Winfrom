
namespace MahantExport.Account
{
    partial class FrmDeliveryChallan
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDeliveryChallan));
            this.panel1 = new AxonContLib.cPanel(this.components);
            this.txtCarat = new AxonContLib.cTextBox(this.components);
            this.txtChallanNo = new AxonContLib.cTextBox(this.components);
            this.BtnSave = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnClose = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnPrint = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnDelete = new DevControlLib.cDevSimpleButton(this.components);
            this.BtnClear = new DevControlLib.cDevSimpleButton(this.components);
            this.DTPDate = new AxonContLib.cDateTimePicker(this.components);
            this.CHALLANNO = new System.Windows.Forms.Label();
            this.CARAT = new System.Windows.Forms.Label();
            this.DATE = new System.Windows.Forms.Label();
            this.MainGrid = new GridViewFooter.GridControlFooterOnTop();
            this.GridData = new GridViewFooter.GridViewFooterOnTop();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.TxtCut = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.TxtPol = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.TxtSym = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.TxtBonusPer = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtCut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtPol)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtSym)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtBonusPer)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtCarat);
            this.panel1.Controls.Add(this.txtChallanNo);
            this.panel1.Controls.Add(this.BtnSave);
            this.panel1.Controls.Add(this.BtnClose);
            this.panel1.Controls.Add(this.BtnPrint);
            this.panel1.Controls.Add(this.BtnDelete);
            this.panel1.Controls.Add(this.BtnClear);
            this.panel1.Controls.Add(this.DTPDate);
            this.panel1.Controls.Add(this.CHALLANNO);
            this.panel1.Controls.Add(this.CARAT);
            this.panel1.Controls.Add(this.DATE);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(609, 125);
            this.panel1.TabIndex = 0;
            // 
            // txtCarat
            // 
            this.txtCarat.ActivationColor = true;
            this.txtCarat.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtCarat.AllowTabKeyOnEnter = false;
            this.txtCarat.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCarat.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCarat.Format = "";
            this.txtCarat.IsComplusory = false;
            this.txtCarat.Location = new System.Drawing.Point(241, 14);
            this.txtCarat.MaxLength = 100;
            this.txtCarat.Name = "txtCarat";
            this.txtCarat.SelectAllTextOnFocus = true;
            this.txtCarat.Size = new System.Drawing.Size(132, 22);
            this.txtCarat.TabIndex = 2;
            this.txtCarat.ToolTips = "";
            this.txtCarat.WaterMarkText = null;
            this.txtCarat.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtCarat_KeyDown);
            // 
            // txtChallanNo
            // 
            this.txtChallanNo.ActivationColor = true;
            this.txtChallanNo.ActivationColorCode = System.Drawing.Color.Empty;
            this.txtChallanNo.AllowTabKeyOnEnter = false;
            this.txtChallanNo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtChallanNo.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChallanNo.Format = "";
            this.txtChallanNo.IsComplusory = false;
            this.txtChallanNo.Location = new System.Drawing.Point(97, 46);
            this.txtChallanNo.MaxLength = 100;
            this.txtChallanNo.Name = "txtChallanNo";
            this.txtChallanNo.ReadOnly = true;
            this.txtChallanNo.SelectAllTextOnFocus = true;
            this.txtChallanNo.Size = new System.Drawing.Size(175, 22);
            this.txtChallanNo.TabIndex = 3;
            this.txtChallanNo.TabStop = false;
            this.txtChallanNo.ToolTips = "";
            this.txtChallanNo.WaterMarkText = null;
            this.txtChallanNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtChallanNo_KeyDown);
            // 
            // BtnSave
            // 
            this.BtnSave.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BtnSave.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSave.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnSave.Appearance.Options.UseBackColor = true;
            this.BtnSave.Appearance.Options.UseFont = true;
            this.BtnSave.Appearance.Options.UseForeColor = true;
            this.BtnSave.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnSave.ImageOptions.SvgImage")));
            this.BtnSave.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnSave.Location = new System.Drawing.Point(19, 87);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(84, 27);
            this.BtnSave.TabIndex = 4;
            this.BtnSave.TabStop = false;
            this.BtnSave.Text = "Save";
            this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // BtnClose
            // 
            this.BtnClose.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BtnClose.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnClose.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnClose.Appearance.Options.UseBackColor = true;
            this.BtnClose.Appearance.Options.UseFont = true;
            this.BtnClose.Appearance.Options.UseForeColor = true;
            this.BtnClose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnClose.ImageOptions.SvgImage")));
            this.BtnClose.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnClose.Location = new System.Drawing.Point(205, 87);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(84, 27);
            this.BtnClose.TabIndex = 6;
            this.BtnClose.TabStop = false;
            this.BtnClose.Text = "Close";
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // BtnPrint
            // 
            this.BtnPrint.Appearance.BackColor = System.Drawing.Color.Gainsboro;
            this.BtnPrint.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPrint.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnPrint.Appearance.Options.UseBackColor = true;
            this.BtnPrint.Appearance.Options.UseFont = true;
            this.BtnPrint.Appearance.Options.UseForeColor = true;
            this.BtnPrint.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnPrint.ImageOptions.SvgImage")));
            this.BtnPrint.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnPrint.Location = new System.Drawing.Point(298, 87);
            this.BtnPrint.Name = "BtnPrint";
            this.BtnPrint.Size = new System.Drawing.Size(84, 27);
            this.BtnPrint.TabIndex = 7;
            this.BtnPrint.TabStop = false;
            this.BtnPrint.Text = "Print";
            this.BtnPrint.Click += new System.EventHandler(this.BtnPrint_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BtnDelete.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDelete.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnDelete.Appearance.Options.UseBackColor = true;
            this.BtnDelete.Appearance.Options.UseFont = true;
            this.BtnDelete.Appearance.Options.UseForeColor = true;
            this.BtnDelete.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnDelete.ImageOptions.SvgImage")));
            this.BtnDelete.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnDelete.Location = new System.Drawing.Point(393, 87);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(84, 27);
            this.BtnDelete.TabIndex = 9;
            this.BtnDelete.TabStop = false;
            this.BtnDelete.Text = "Delete";
            this.BtnDelete.Visible = false;
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // BtnClear
            // 
            this.BtnClear.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BtnClear.Appearance.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnClear.Appearance.ForeColor = System.Drawing.Color.Black;
            this.BtnClear.Appearance.Options.UseBackColor = true;
            this.BtnClear.Appearance.Options.UseFont = true;
            this.BtnClear.Appearance.Options.UseForeColor = true;
            this.BtnClear.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("BtnClear.ImageOptions.SvgImage")));
            this.BtnClear.ImageOptions.SvgImageSize = new System.Drawing.Size(20, 20);
            this.BtnClear.Location = new System.Drawing.Point(110, 87);
            this.BtnClear.Name = "BtnClear";
            this.BtnClear.Size = new System.Drawing.Size(84, 27);
            this.BtnClear.TabIndex = 5;
            this.BtnClear.TabStop = false;
            this.BtnClear.Text = "Clear";
            this.BtnClear.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // DTPDate
            // 
            this.DTPDate.AllowTabKeyOnEnter = false;
            this.DTPDate.CustomFormat = "dd/MM/yyyy";
            this.DTPDate.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DTPDate.ForeColor = System.Drawing.Color.Black;
            this.DTPDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.DTPDate.Location = new System.Drawing.Point(56, 14);
            this.DTPDate.Name = "DTPDate";
            this.DTPDate.Size = new System.Drawing.Size(118, 22);
            this.DTPDate.TabIndex = 1;
            this.DTPDate.ToolTips = "";
            // 
            // CHALLANNO
            // 
            this.CHALLANNO.AutoSize = true;
            this.CHALLANNO.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CHALLANNO.Location = new System.Drawing.Point(16, 50);
            this.CHALLANNO.Name = "CHALLANNO";
            this.CHALLANNO.Size = new System.Drawing.Size(78, 14);
            this.CHALLANNO.TabIndex = 2;
            this.CHALLANNO.Text = "Challan No";
            // 
            // CARAT
            // 
            this.CARAT.AutoSize = true;
            this.CARAT.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CARAT.Location = new System.Drawing.Point(194, 17);
            this.CARAT.Name = "CARAT";
            this.CARAT.Size = new System.Drawing.Size(43, 14);
            this.CARAT.TabIndex = 1;
            this.CARAT.Text = "Carat";
            // 
            // DATE
            // 
            this.DATE.AutoSize = true;
            this.DATE.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DATE.Location = new System.Drawing.Point(16, 17);
            this.DATE.Name = "DATE";
            this.DATE.Size = new System.Drawing.Size(38, 14);
            this.DATE.TabIndex = 0;
            this.DATE.Text = "Date";
            // 
            // MainGrid
            // 
            this.MainGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainGrid.Location = new System.Drawing.Point(0, 125);
            this.MainGrid.MainView = this.GridData;
            this.MainGrid.Name = "MainGrid";
            this.MainGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.TxtCut,
            this.TxtPol,
            this.TxtSym,
            this.TxtBonusPer});
            this.MainGrid.Size = new System.Drawing.Size(609, 340);
            this.MainGrid.TabIndex = 4;
            this.MainGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.GridData});
            // 
            // GridData
            // 
            this.GridData.Appearance.FilterPanel.Options.UseTextOptions = true;
            this.GridData.Appearance.FilterPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GridData.Appearance.FocusedCell.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.GridData.Appearance.FocusedCell.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.GridData.Appearance.FocusedCell.Options.UseBackColor = true;
            this.GridData.Appearance.FooterPanel.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold);
            this.GridData.Appearance.FooterPanel.Options.UseFont = true;
            this.GridData.Appearance.HeaderPanel.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Bold);
            this.GridData.Appearance.HeaderPanel.Options.UseFont = true;
            this.GridData.Appearance.HeaderPanel.Options.UseTextOptions = true;
            this.GridData.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.GridData.Appearance.HorzLine.BackColor = System.Drawing.Color.Gray;
            this.GridData.Appearance.HorzLine.Options.UseBackColor = true;
            this.GridData.Appearance.Row.Font = new System.Drawing.Font("Verdana", 9F);
            this.GridData.Appearance.Row.Options.UseFont = true;
            this.GridData.Appearance.VertLine.BackColor = System.Drawing.Color.Gray;
            this.GridData.Appearance.VertLine.Options.UseBackColor = true;
            this.GridData.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.gridColumn2,
            this.gridColumn3});
            this.GridData.FooterPanelHeight = 25;
            this.GridData.GridControl = this.MainGrid;
            this.GridData.Name = "GridData";
            this.GridData.OptionsBehavior.Editable = false;
            this.GridData.OptionsFilter.AllowFilterEditor = false;
            this.GridData.OptionsNavigation.EnterMoveNextColumn = true;
            this.GridData.OptionsPrint.ExpandAllGroups = false;
            this.GridData.OptionsView.ColumnAutoWidth = false;
            this.GridData.OptionsView.FooterLocation = GridViewFooter.FooterPosition.Bottom;
            this.GridData.OptionsView.GroupFooterShowMode = DevExpress.XtraGrid.Views.Grid.GroupFooterShowMode.VisibleAlways;
            this.GridData.OptionsView.ShowAutoFilterRow = true;
            this.GridData.OptionsView.ShowFooter = true;
            this.GridData.OptionsView.ShowGroupPanel = false;
            this.GridData.RowHeight = 25;
            this.GridData.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.GridData_RowCellClick);
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
            this.gridColumn1.Caption = "Carat";
            this.gridColumn1.FieldName = "CARAT";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 1;
            this.gridColumn1.Width = 101;
            // 
            // gridColumn2
            // 
            this.gridColumn2.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn2.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn2.AppearanceCell.Options.UseFont = true;
            this.gridColumn2.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn2.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn2.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn2.AppearanceHeader.Options.UseFont = true;
            this.gridColumn2.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn2.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn2.Caption = "ChallanNo";
            this.gridColumn2.FieldName = "CHALLANNO";
            this.gridColumn2.Name = "gridColumn2";
            this.gridColumn2.Visible = true;
            this.gridColumn2.VisibleIndex = 2;
            this.gridColumn2.Width = 106;
            // 
            // gridColumn3
            // 
            this.gridColumn3.AppearanceCell.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn3.AppearanceCell.ForeColor = System.Drawing.Color.Black;
            this.gridColumn3.AppearanceCell.Options.UseFont = true;
            this.gridColumn3.AppearanceCell.Options.UseForeColor = true;
            this.gridColumn3.AppearanceCell.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.AppearanceHeader.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gridColumn3.AppearanceHeader.ForeColor = System.Drawing.Color.Black;
            this.gridColumn3.AppearanceHeader.Options.UseFont = true;
            this.gridColumn3.AppearanceHeader.Options.UseForeColor = true;
            this.gridColumn3.AppearanceHeader.Options.UseTextOptions = true;
            this.gridColumn3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridColumn3.Caption = "Date";
            this.gridColumn3.FieldName = "DATE";
            this.gridColumn3.Name = "gridColumn3";
            this.gridColumn3.Visible = true;
            this.gridColumn3.VisibleIndex = 0;
            // 
            // TxtCut
            // 
            this.TxtCut.AutoHeight = false;
            this.TxtCut.Name = "TxtCut";
            // 
            // TxtPol
            // 
            this.TxtPol.AutoHeight = false;
            this.TxtPol.Name = "TxtPol";
            // 
            // TxtSym
            // 
            this.TxtSym.AutoHeight = false;
            this.TxtSym.Name = "TxtSym";
            // 
            // TxtBonusPer
            // 
            this.TxtBonusPer.AutoHeight = false;
            this.TxtBonusPer.Name = "TxtBonusPer";
            // 
            // FrmDeliveryChallan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 465);
            this.Controls.Add(this.MainGrid);
            this.Controls.Add(this.panel1);
            this.Name = "FrmDeliveryChallan";
            this.Tag = "DeliveryChallan\r\n";
            this.Text = "DELIVERY CHALLAN";
            this.Load += new System.EventHandler(this.FrmDeliveryChallan_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtCut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtPol)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtSym)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TxtBonusPer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxonContLib.cPanel panel1;
        private System.Windows.Forms.Label CHALLANNO;
        private System.Windows.Forms.Label CARAT;
        private System.Windows.Forms.Label DATE;
        private AxonContLib.cDateTimePicker DTPDate;
        private DevControlLib.cDevSimpleButton BtnSave;
        private DevControlLib.cDevSimpleButton BtnClose;
        private DevControlLib.cDevSimpleButton BtnPrint;
        private DevControlLib.cDevSimpleButton BtnDelete;
        private DevControlLib.cDevSimpleButton BtnClear;
        private AxonContLib.cTextBox txtChallanNo;
        private AxonContLib.cTextBox txtCarat;
        private GridViewFooter.GridControlFooterOnTop MainGrid;
        private GridViewFooter.GridViewFooterOnTop GridData;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit TxtCut;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit TxtPol;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit TxtSym;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit TxtBonusPer;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
    }
}