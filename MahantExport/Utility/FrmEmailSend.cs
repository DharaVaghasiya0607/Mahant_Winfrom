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
using System.Net.Mail;
using BusLib.Configuration;
using BusLib.TableName;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Net.Security;
using MahantExport.Utility;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using MahantExport.Class;


namespace MahantExport
{
    public partial class FrmEmailSend : DevControlLib.cDevXtraForm
    {
        BODevGridSelection ObjGridSelection;

        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        public FrmEmailSend()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            AttachFormDefaultEvent();
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            if (ObjPer.ISVIEW == false)
            {
                Global.MessageError(BusLib.TPV.BOMessage.ViewDeniedMsg);
                return;
            }

            Val.FormGeneralSetting(this);

            this.Show();
        }

        public void ShowForm(string pStrFileName, string pStrToEmail)
        {
            txtAttachment.Text = pStrFileName;
            txtToEmail.Text = pStrToEmail;
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

        private void BtnSend_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string StrFileName = string.Empty;
                CommonEmail SendEmail = new CommonEmail();
                string Str = SendEmail.SendGeneralEmail(txtToEmail.Text, txtCCEmail.Text, txtBCCEmail.Text, txtSubject.Text, htmlEditor.InnerHtml, txtAttachment.Text, StrFileName);
                SendEmail = null;

                this.Cursor = Cursors.Default;
                Global.Message(Str);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.MessageError(ex.ToString());
            }

            //try
            //{
            //    this.Cursor = Cursors.WaitCursor;

            //    EmailSettingProperty EmailProperty = new BOEmailSettings().GetDataByEmailType(BOEmailSettings.EmailType.GeneralEmail);

            //    string stReturnText = string.Empty;


            //    //MailMessage objEmail = new MailMessage(from, to);
            //    MailMessage objEmail = new MailMessage();
            //    MailAddress from = new MailAddress(EmailProperty.SMTPEMAILUSERNAME, EmailProperty.SMTPDISPLAYNAME);
            //    objEmail.From = from;

            //    EmailProperty.TOEMAIL = txtToEmail.Text + "," + EmailProperty.TOEMAIL;

            //    string[] EmailIDs = EmailProperty.TOEMAIL.Split(',');
            //    int i;
            //    for (i = 0; i < EmailIDs.Length; i++)
            //    {
            //        if (EmailIDs[i].Contains("@") == true && EmailIDs[i].Contains(".") == true)
            //        {
            //            objEmail.To.Add(EmailIDs[i]);
            //        }
            //    }

            //    EmailProperty.CCEMAIL = txtCCEmail.Text + "," + EmailProperty.CCEMAIL;

            //    string[] CCEmailIDs = EmailProperty.CCEMAIL.Split(',');

            //    for (i = 0; i < CCEmailIDs.Length; i++)
            //    {
            //        if (CCEmailIDs[i].Contains("@") == true && CCEmailIDs[i].Contains(".") == true)
            //        {
            //            objEmail.CC.Add(CCEmailIDs[i]);
            //        }
            //    }

            //    EmailProperty.BCCEMAIL = txtBCCEmail.Text + "," + EmailProperty.BCCEMAIL;

            //    string[] BCCEmailIDs = EmailProperty.BCCEMAIL.Split(',');
            //    for (i = 0; i < BCCEmailIDs.Length; i++)
            //    {
            //        if (BCCEmailIDs[i].Contains("@") == true && BCCEmailIDs[i].Contains(".") == true)
            //        {
            //            objEmail.Bcc.Add(BCCEmailIDs[i]);
            //        }
            //    }

            //    if (txtAttachment.Text.Trim().Length != 0)
            //    {
            //        objEmail.Attachments.Add(new Attachment(txtAttachment.Text));
            //    }

            //    objEmail.Subject = EmailProperty.SUBJECT;
            //    objEmail.Body = htmlEditor.InnerHtml;
            //    objEmail.IsBodyHtml = true;
            //    objEmail.Priority = MailPriority.High;

            //    SmtpClient client = new SmtpClient();
            //    System.Net.NetworkCredential auth = new System.Net.NetworkCredential(EmailProperty.SMTPEMAILUSERNAME, EmailProperty.SMTPEMAILPASSWORD);
            //    client.Host = EmailProperty.SMTPHOST;
            //    client.Port = EmailProperty.SMTPPORT;
            //    client.UseDefaultCredentials = false;
            //    client.Credentials = auth;
            //    client.EnableSsl = EmailProperty.SMTPENABLESSL;
            //    ServicePointManager.ServerCertificateValidationCallback = delegate(object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            //    { return true; };

            //    client.Send(objEmail);

            //    this.Cursor = Cursors.Default;
            //    Global.Message("Email Sent Successfully");

            //}
            //catch (Exception EX)
            //{
            //    this.Cursor = Cursors.Default;
            //    Global.Message(EX.Message);                
            //}
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtToEmail.Text = string.Empty;
            txtCCEmail.Text = string.Empty;
            txtBCCEmail.Text = string.Empty;
            txtSubject.Text = "(no subject)";
            htmlEditor.InnerText = string.Empty;
            htmlEditor.InnerHtml = string.Empty;
            txtAttachment.Text = string.Empty;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog Open = new OpenFileDialog();
            Open.Title = "Browse Text Files";
            if (Open.ShowDialog() == DialogResult.OK)
            {
                txtAttachment.Text = Open.FileName;
            }
            Open.Dispose();
            Open = null;

        }

        private void FrmEmailSend_Load(object sender, EventArgs e)
        {
            MainGridMarty.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);
            if (MainGridMarty.RepositoryItems.Count == 0)
            {
                ObjGridSelection = new BODevGridSelection();
                ObjGridSelection.View = GrdDetParty;
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                GrdDetParty.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;
            }
            else
            {
                ObjGridSelection.ClearSelection();
            }

            GrdDetParty.BestFitColumns();

        }

        private DataTable GetTableOfSelectedRows(GridView view, Boolean IsSelect)
        {
            if (view.RowCount <= 0)
            {
                return null;
            }
            ArrayList aryLst = new ArrayList();


            DataTable resultTable = new DataTable();
            DataTable sourceTable = null;
            sourceTable = ((DataView)view.DataSource).Table;

            if (IsSelect)
            {
                aryLst = ObjGridSelection.GetSelectedArrayList();
                resultTable = sourceTable.Clone();
                for (int i = 0; i < aryLst.Count; i++)
                {
                    DataRowView oDataRowView = aryLst[i] as DataRowView;
                    resultTable.Rows.Add(oDataRowView.Row.ItemArray);
                }
            }

            return resultTable;
        }

        private void BtnSendBulkEmail_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                byte[] FileByte = null;
                string StrFileName = string.Empty;

                if (txtAttachment.Text.Trim().Length != 0)
                {
                    FileByte = System.IO.File.ReadAllBytes(txtAttachment.Text);
                    System.IO.FileInfo f = new System.IO.FileInfo(txtAttachment.Text);
                    StrFileName = f.Name;
                }

                DataTable DtInvDetail = GetTableOfSelectedRows(GrdDetParty, true);
                if (DtInvDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.MessageError("To Email ID Is Requird");
                    txtToEmail.Focus();
                    return;
                }

                string StrEmail = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.Trim(DRow["EMAILID"]) != "")
                    {
                        StrEmail = StrEmail + Val.Trim(DRow["EMAILID"]) + ",";
                    }
                }

                //string Str = new CommonEmail().SendGeneralEmailAsync(StrEmail, txtCCEmail.Text, txtBCCEmail.Text, txtSubject.Text, htmlEditor.InnerHtml, FileByte, StrFileName);
                string Str = new CommonEmail().SendGeneralEmailAsync(txtToEmail.Text, txtCCEmail.Text, StrEmail, txtSubject.Text, htmlEditor.InnerHtml, FileByte, StrFileName);
                this.Cursor = Cursors.Default;
                Global.Message(Str);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.MessageError(ex.ToString());
            }
        }

        private void lblOpenFile_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(txtAttachment.Text, "cmd");
        }
    }
}