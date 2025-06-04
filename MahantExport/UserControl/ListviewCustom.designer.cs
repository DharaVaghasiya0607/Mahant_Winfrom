namespace MahantExport.UserControl
{
    partial class ListviewCustom
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.GrpList = new DevControlLib.cDevGroupControl(this.components);
            this.CheckedList = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.ChkDelSelAll = new DevExpress.XtraEditors.CheckEdit();
            ((System.ComponentModel.ISupportInitialize)(this.GrpList)).BeginInit();
            this.GrpList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CheckedList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkDelSelAll.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // GrpList
            // 
            this.GrpList.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.GrpList.Appearance.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.GrpList.Appearance.Options.UseBackColor = true;
            this.GrpList.Appearance.Options.UseFont = true;
            this.GrpList.AppearanceCaption.Font = new System.Drawing.Font("Verdana", 8F, System.Drawing.FontStyle.Bold);
            this.GrpList.AppearanceCaption.Options.UseFont = true;
            this.GrpList.AppearanceCaption.Options.UseTextOptions = true;
            this.GrpList.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.GrpList.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.Style3D;
            this.GrpList.Controls.Add(this.CheckedList);
            this.GrpList.Controls.Add(this.ChkDelSelAll);
            this.GrpList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GrpList.Location = new System.Drawing.Point(0, 0);
            this.GrpList.Name = "GrpList";
            this.GrpList.Size = new System.Drawing.Size(180, 193);
            this.GrpList.TabIndex = 182;
            // 
            // CheckedList
            // 
            this.CheckedList.Appearance.Font = new System.Drawing.Font("Verdana", 8F);
            this.CheckedList.Appearance.Options.UseFont = true;
            this.CheckedList.CheckOnClick = true;
            this.CheckedList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CheckedList.IncrementalSearch = true;
            this.CheckedList.Location = new System.Drawing.Point(2, 23);
            this.CheckedList.Name = "CheckedList";
            this.CheckedList.SelectionMode = System.Windows.Forms.SelectionMode.None;
            this.CheckedList.Size = new System.Drawing.Size(176, 168);
            this.CheckedList.TabIndex = 3;
            this.CheckedList.SelectedIndexChanged += new System.EventHandler(this.CheckedList_SelectedIndexChanged);
            this.CheckedList.Enter += new System.EventHandler(this.CheckedList_Enter);
            // 
            // ChkDelSelAll
            // 
            this.ChkDelSelAll.Location = new System.Drawing.Point(2, 1);
            this.ChkDelSelAll.Name = "ChkDelSelAll";
            this.ChkDelSelAll.Properties.Caption = "";
            this.ChkDelSelAll.Size = new System.Drawing.Size(21, 19);
            this.ChkDelSelAll.TabIndex = 1;
            this.ChkDelSelAll.CheckStateChanged += new System.EventHandler(this.ChkDelSelAll_CheckStateChanged);
            // 
            // ListviewCustom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.GrpList);
            this.LookAndFeel.SkinName = "London Liquid Sky";
            this.LookAndFeel.UseDefaultLookAndFeel =true;
            this.Name = "ListviewCustom";
            this.Size = new System.Drawing.Size(180, 193);
            ((System.ComponentModel.ISupportInitialize)(this.GrpList)).EndInit();
            this.GrpList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CheckedList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ChkDelSelAll.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevControlLib.cDevGroupControl GrpList;
        private DevExpress.XtraEditors.CheckedListBoxControl CheckedList;
        private DevExpress.XtraEditors.CheckEdit ChkDelSelAll;
        
        

    }
}
