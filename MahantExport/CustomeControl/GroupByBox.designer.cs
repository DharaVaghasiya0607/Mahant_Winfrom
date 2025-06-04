namespace MahantExport.CustomeControl
{
    partial class GroupByBox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GroupByBox));
            this.grpBack = new DevExpress.XtraEditors.GroupControl();
            this.pnlMiddle = new DevExpress.XtraEditors.PanelControl();
            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
            this.BtnDown = new DevExpress.XtraEditors.SimpleButton();
            this.BtnUp = new DevExpress.XtraEditors.SimpleButton();
            this.BtnBackWardAll = new DevExpress.XtraEditors.SimpleButton();
            this.BtnForwardAll = new DevExpress.XtraEditors.SimpleButton();
            this.BtnForward = new DevExpress.XtraEditors.SimpleButton();
            this.BtnBackward = new DevExpress.XtraEditors.SimpleButton();
            this.pnlFrom = new DevExpress.XtraEditors.PanelControl();
            this.ListFrom = new AxonContLib.cListView(this.components);
            this.panelTO = new DevExpress.XtraEditors.PanelControl();
            this.ListTo = new AxonContLib.cListView(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.grpBack)).BeginInit();
            this.grpBack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlMiddle)).BeginInit();
            this.pnlMiddle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
            this.panelControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pnlFrom)).BeginInit();
            this.pnlFrom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.panelTO)).BeginInit();
            this.panelTO.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpBack
            // 
            this.grpBack.AppearanceCaption.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
            this.grpBack.AppearanceCaption.Options.UseFont = true;
            this.grpBack.Controls.Add(this.pnlMiddle);
            this.grpBack.Controls.Add(this.pnlFrom);
            this.grpBack.Controls.Add(this.panelTO);
            this.grpBack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBack.Location = new System.Drawing.Point(0, 0);
            this.grpBack.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.grpBack.Name = "grpBack";
            this.grpBack.Size = new System.Drawing.Size(434, 325);
            this.grpBack.TabIndex = 0;
            // 
            // pnlMiddle
            // 
            this.pnlMiddle.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlMiddle.Controls.Add(this.panelControl1);
            this.pnlMiddle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMiddle.Location = new System.Drawing.Point(190, 25);
            this.pnlMiddle.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlMiddle.Name = "pnlMiddle";
            this.pnlMiddle.Size = new System.Drawing.Size(64, 298);
            this.pnlMiddle.TabIndex = 1;
            // 
            // panelControl1
            // 
            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelControl1.Controls.Add(this.BtnDown);
            this.panelControl1.Controls.Add(this.BtnUp);
            this.panelControl1.Controls.Add(this.BtnBackWardAll);
            this.panelControl1.Controls.Add(this.BtnForwardAll);
            this.panelControl1.Controls.Add(this.BtnForward);
            this.panelControl1.Controls.Add(this.BtnBackward);
            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelControl1.Location = new System.Drawing.Point(0, 0);
            this.panelControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelControl1.Name = "panelControl1";
            this.panelControl1.Size = new System.Drawing.Size(64, 298);
            this.panelControl1.TabIndex = 0;
            // 
            // BtnDown
            // 
            this.BtnDown.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BtnDown.Appearance.Options.UseFont = true;
            this.BtnDown.Image = ((System.Drawing.Image)(resources.GetObject("BtnDown.Image")));
            this.BtnDown.Location = new System.Drawing.Point(7, 136);
            this.BtnDown.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnDown.Name = "BtnDown";
            this.BtnDown.Size = new System.Drawing.Size(48, 39);
            this.BtnDown.TabIndex = 3;
            this.BtnDown.ToolTip = "Move Down";
            this.BtnDown.Click += new System.EventHandler(this.BtnDown_Click);
            // 
            // BtnUp
            // 
            this.BtnUp.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BtnUp.Appearance.Options.UseFont = true;
            this.BtnUp.Image = ((System.Drawing.Image)(resources.GetObject("BtnUp.Image")));
            this.BtnUp.Location = new System.Drawing.Point(7, 93);
            this.BtnUp.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnUp.Name = "BtnUp";
            this.BtnUp.Size = new System.Drawing.Size(48, 39);
            this.BtnUp.TabIndex = 2;
            this.BtnUp.ToolTip = "Move Up";
            this.BtnUp.Click += new System.EventHandler(this.BtnUp_Click);
            // 
            // BtnBackWardAll
            // 
            this.BtnBackWardAll.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BtnBackWardAll.Appearance.Options.UseFont = true;
            this.BtnBackWardAll.Image = ((System.Drawing.Image)(resources.GetObject("BtnBackWardAll.Image")));
            this.BtnBackWardAll.Location = new System.Drawing.Point(7, 222);
            this.BtnBackWardAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnBackWardAll.Name = "BtnBackWardAll";
            this.BtnBackWardAll.Size = new System.Drawing.Size(48, 39);
            this.BtnBackWardAll.TabIndex = 0;
            this.BtnBackWardAll.ToolTip = "Move All Right To Left";
            this.BtnBackWardAll.Click += new System.EventHandler(this.BtnBackWardAll_Click);
            // 
            // BtnForwardAll
            // 
            this.BtnForwardAll.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BtnForwardAll.Appearance.Options.UseFont = true;
            this.BtnForwardAll.Image = ((System.Drawing.Image)(resources.GetObject("BtnForwardAll.Image")));
            this.BtnForwardAll.Location = new System.Drawing.Point(7, 179);
            this.BtnForwardAll.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnForwardAll.Name = "BtnForwardAll";
            this.BtnForwardAll.Size = new System.Drawing.Size(48, 39);
            this.BtnForwardAll.TabIndex = 0;
            this.BtnForwardAll.ToolTip = "Move All Left To Right";
            this.BtnForwardAll.Click += new System.EventHandler(this.BtnForwardAll_Click);
            // 
            // BtnForward
            // 
            this.BtnForward.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BtnForward.Appearance.Options.UseFont = true;
            this.BtnForward.Image = ((System.Drawing.Image)(resources.GetObject("BtnForward.Image")));
            this.BtnForward.Location = new System.Drawing.Point(7, 7);
            this.BtnForward.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnForward.Name = "BtnForward";
            this.BtnForward.Size = new System.Drawing.Size(48, 39);
            this.BtnForward.TabIndex = 0;
            this.BtnForward.ToolTip = "Move Left To Right";
            this.BtnForward.Click += new System.EventHandler(this.BtnForward_Click);
            // 
            // BtnBackward
            // 
            this.BtnBackward.Appearance.Font = new System.Drawing.Font("Tahoma", 9F);
            this.BtnBackward.Appearance.Options.UseFont = true;
            this.BtnBackward.Image = ((System.Drawing.Image)(resources.GetObject("BtnBackward.Image")));
            this.BtnBackward.Location = new System.Drawing.Point(7, 50);
            this.BtnBackward.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.BtnBackward.Name = "BtnBackward";
            this.BtnBackward.Size = new System.Drawing.Size(48, 39);
            this.BtnBackward.TabIndex = 1;
            this.BtnBackward.ToolTip = "Move Right To Left";
            this.BtnBackward.Click += new System.EventHandler(this.BtnBackward_Click);
            // 
            // pnlFrom
            // 
            this.pnlFrom.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.pnlFrom.Controls.Add(this.ListFrom);
            this.pnlFrom.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlFrom.Location = new System.Drawing.Point(2, 25);
            this.pnlFrom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnlFrom.Name = "pnlFrom";
            this.pnlFrom.Size = new System.Drawing.Size(188, 298);
            this.pnlFrom.TabIndex = 4;
            // 
            // ListFrom
            // 
            this.ListFrom.AllowTabKeyOnEnter = false;
            this.ListFrom.BackColor = System.Drawing.Color.White;
            this.ListFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListFrom.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ListFrom.ForeColor = System.Drawing.Color.Black;
            this.ListFrom.GridLines = true;
            this.ListFrom.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ListFrom.Location = new System.Drawing.Point(0, 0);
            this.ListFrom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ListFrom.Name = "ListFrom";
            this.ListFrom.Size = new System.Drawing.Size(188, 298);
            this.ListFrom.TabIndex = 1;
            this.ListFrom.ToolTips = "";
            this.ListFrom.UseCompatibleStateImageBehavior = false;
            this.ListFrom.View = System.Windows.Forms.View.Details;
            this.ListFrom.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListFrom_MouseDoubleClick);
            // 
            // panelTO
            // 
            this.panelTO.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
            this.panelTO.Controls.Add(this.ListTo);
            this.panelTO.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelTO.Location = new System.Drawing.Point(254, 25);
            this.panelTO.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.panelTO.Name = "panelTO";
            this.panelTO.Size = new System.Drawing.Size(178, 298);
            this.panelTO.TabIndex = 3;
            // 
            // ListTo
            // 
            this.ListTo.AllowTabKeyOnEnter = false;
            this.ListTo.BackColor = System.Drawing.Color.White;
            this.ListTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListTo.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ListTo.ForeColor = System.Drawing.Color.Black;
            this.ListTo.GridLines = true;
            this.ListTo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.ListTo.Location = new System.Drawing.Point(0, 0);
            this.ListTo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ListTo.Name = "ListTo";
            this.ListTo.Size = new System.Drawing.Size(178, 298);
            this.ListTo.TabIndex = 0;
            this.ListTo.ToolTips = "";
            this.ListTo.UseCompatibleStateImageBehavior = false;
            this.ListTo.View = System.Windows.Forms.View.Details;
            this.ListTo.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.ListTo_MouseDoubleClick);
            // 
            // GroupByBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grpBack);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "GroupByBox";
            this.Size = new System.Drawing.Size(434, 325);
            ((System.ComponentModel.ISupportInitialize)(this.grpBack)).EndInit();
            this.grpBack.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlMiddle)).EndInit();
            this.pnlMiddle.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
            this.panelControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pnlFrom)).EndInit();
            this.pnlFrom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.panelTO)).EndInit();
            this.panelTO.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl grpBack;
        private DevExpress.XtraEditors.PanelControl panelControl1;
        private DevExpress.XtraEditors.SimpleButton BtnDown;
        private DevExpress.XtraEditors.SimpleButton BtnUp;
        private DevExpress.XtraEditors.SimpleButton BtnForward;
        private DevExpress.XtraEditors.SimpleButton BtnBackward;
        private DevExpress.XtraEditors.PanelControl pnlMiddle;
        private DevExpress.XtraEditors.PanelControl pnlFrom;
        private DevExpress.XtraEditors.PanelControl panelTO;
        private AxonContLib.cListView ListFrom;
        private AxonContLib.cListView ListTo;
        private DevExpress.XtraEditors.SimpleButton BtnBackWardAll;
        private DevExpress.XtraEditors.SimpleButton BtnForwardAll;
    }
}
