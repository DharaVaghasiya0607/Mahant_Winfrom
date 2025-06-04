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
    public partial class FrmInputBoxOther : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable DTab;

        public string ColumnsToHide = "";

        public bool AllowFirstColumnHide = false;

        public string StrInoutText = "";

        string mStrSettingKey = "";

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();

        BOFormPer ObjPer = new BOFormPer();

        public DataRow DRow { get; set; }

        public FrmInputBoxOther()
        {
            InitializeComponent();
        }

        public FORMTYPE mFormType = FORMTYPE.ABOUTUS;
        public enum FORMTYPE
        {
            ABOUTUS = 1,
            OURTERMS = 2,
            CONTACTUS = 3,
            PRIVACYPOLICY = 4
        }

        public void ShowForm()
        {
            //txtMessageEnglish.Text = new BOMST_FormPermission().GetMessage();

            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            BtnSubmit.Enabled = ObjPer.ISINSERT;

            DataTable DtabMessage = new BOMST_FormPermission().GetMessage();

            if (DtabMessage.Rows.Count > 0)
            {
                txtHtmlEditorEnglish.InnerHtml = Val.ToString(DtabMessage.Rows[0]["SETTINGVALUE"]);
                txtHtmlEditorChinese.InnerHtml = Val.ToString(DtabMessage.Rows[0]["SETTINGVALUECHINESE"]);
            }
            else
            {
                txtHtmlEditorEnglish.InnerHtml = string.Empty;
                txtHtmlEditorChinese.InnerHtml = string.Empty;
            }

            AttachFormDefaultEvent();
            Val.FormGeneralSetting(this);
            this.Show();
        }
        public void ShowForm(FORMTYPE pFormType)
        {
            //txtMessageEnglish.Text = new BOMST_FormPermission().GetMessage();

            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            BtnSubmit.Enabled = ObjPer.ISINSERT;

            mFormType = pFormType;
            DataTable DtabMessage = new DataTable();
            if (mFormType == FORMTYPE.ABOUTUS)
            {
                this.Text = "ABOUT US DESCRIPTION";
                mStrSettingKey = "ABOUTUS";
                GrpMsg.Text = "Enter Your Message For :- (About US)";
            }
            else if (mFormType == FORMTYPE.OURTERMS)
            {
                this.Text = "OUR TERMS DESCRIPTION";
                mStrSettingKey = "OURTERMS";
                GrpMsg.Text = "Enter Your Message For :- (Our Terms)";
            }
            else if (mFormType == FORMTYPE.CONTACTUS)
            {
                this.Text = "CONTACT US DESCRIPTION";
                mStrSettingKey = "CONTACTUS";
                GrpMsg.Text = "Enter Your Message For :- (Contact US)";
            }
            else if (mFormType == FORMTYPE.PRIVACYPOLICY)
            {
                this.Text = "PRIVACY POLICY DESCRIPTION";
                mStrSettingKey = "PRIVACYPOLICY";
                GrpMsg.Text = "Enter Your Message For :- (Privacy Policy)";
            }

            DtabMessage = new BOMST_FormPermission().GetMessageFromSettingTable(mStrSettingKey);

            if (DtabMessage.Rows.Count > 0)
            {
                txtHtmlEditorEnglish.InnerHtml = Val.ToString(DtabMessage.Rows[0]["SETTINGVALUE"]);
                txtHtmlEditorChinese.InnerHtml = Val.ToString(DtabMessage.Rows[0]["SETTINGVALUECHINESE"]);
            }
            else
            {
                txtHtmlEditorEnglish.InnerHtml = string.Empty;
                txtHtmlEditorChinese.InnerHtml = string.Empty;
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

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Close();
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int IntRes = 0;

                string StrMessages = Val.ToString(txtHtmlEditorEnglish.InnerHtml).Replace("'", "''");
                string StrMessagesChinese = Val.ToString(txtHtmlEditorChinese.InnerHtml).Replace("'", "''");

                //if (mFormType == FORMTYPE.ABOUTUS)
                //{
                //    IntRes = new BOMST_FormPermission().SaveMessageForAboutUS(StrMessages, StrMessagesChinese);
                //}
                //else if (mFormType == FORMTYPE.OURTERMS)
                //{
                //    IntRes = new BOMST_FormPermission().SaveMessageForOurTerms(StrMessages, StrMessagesChinese);
                //}
                //else if (mFormType == FORMTYPE.CONTACTUS)
                //{

                IntRes = new BOMST_FormPermission().SaveMessageInSettingTable(StrMessages, StrMessagesChinese, mStrSettingKey);

                //}

                if (IntRes != -1)
                {
                    Global.Message("MESSAGE SAVED");
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }



    }
}