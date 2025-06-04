using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Globalization;
using System.Collections;
using BusLib.Master;
using BusLib.Configuration;


namespace MahantExport
{
    public partial class FrmInputBox : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable DTab;

        public string ColumnsToHide = "";

        public bool AllowFirstColumnHide = false;

        public string StrInoutText = "";

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();

        BOFormPer ObjPer = new BOFormPer();

        public DataRow DRow { get; set; }

        public FrmInputBox()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            //txtMessageEnglish.Text = new BOMST_FormPermission().GetMessage();

            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            BtnSubmit.Enabled = ObjPer.ISINSERT;
            
            DataTable DtabMessage = new BOMST_FormPermission().GetMessage();

            if (DtabMessage.Rows.Count > 0)
            {
                txtMessageEnglish.Text = Val.ToString(DtabMessage.Rows[0]["SETTINGVALUE"]);
                txtMessageChinese.Text = Val.ToString(DtabMessage.Rows[0]["SETTINGVALUECHINESE"]);
            }
            else
            {
                txtMessageEnglish.Text = string.Empty;
                txtMessageChinese.Text = string.Empty;
            }

            AttachFormDefaultEvent();

            Val.FormGeneralSetting(this);         
            this.Show();             
        }
        private void AttachFormDefaultEvent()
        {
            Val.FormGeneralSetting(this);
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.FormKeyPress = false;

            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);

        }
      
        private void FrmSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.No;
                this.Close();
            }            
        }

        private void txtSeach_TextChanged(object sender, EventArgs e)
        {
            StrInoutText = txtMessageEnglish.Text;
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Close();
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            int IntRes = new BOMST_FormPermission().SaveMessage(Val.ToString(txtMessageEnglish.Text),Val.ToString(txtMessageChinese.Text));
            if (IntRes != -1)
            {
                Global.Message("MESSAGE SAVED");
                //Global.gStrSuvichar = txtMessageEnglish.Text;
                //if (Global.gStrSuvichar.Trim() == "")
                //{
                //    Global.gStrSuvichar = "!! WELCOME " + Global.gStrCompanyName + " !!";
                //}
            }
        }

        private void FrmInputBox_FormClosed(object sender, FormClosedEventArgs e)
        {
            StrInoutText = txtMessageEnglish.Text;
        }


    }
}