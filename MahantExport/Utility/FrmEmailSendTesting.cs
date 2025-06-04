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
using System.IO;
using MahantExport.Properties;


namespace MahantExport
{
    public partial class FrmEmailSendTesting : DevControlLib.cDevXtraForm
    {
        BODevGridSelection ObjGridSelection;
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();

        public FrmEmailSendTesting()
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

        public void ShowForm(string pStrFileName)
        {
            txtAttachment.Text = pStrFileName;
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
                byte[] FileByte = null;
                string StrFileName = string.Empty;

                if (txtAttachment.Text.Trim().Length != 0)
                {
                    FileByte = System.IO.File.ReadAllBytes(txtAttachment.Text);
                    System.IO.FileInfo f = new System.IO.FileInfo(txtAttachment.Text);
                    StrFileName = f.Name;
                }


                //Add : Pinali : 14-10-2019
                EmailSettingProperty EmailProperty = new EmailSettingProperty();
                EmailProperty.SMTPEMAILUSERNAME = txtSMTPUserName.Text;
                EmailProperty.SMTPEMAILPASSWORD = txtSMTPPassword.Text;
                EmailProperty.SMTPHOST = txtSMTPHost.Text;
                EmailProperty.SMTPPORT = Val.ToInt(txtSMTPPort.Text);
                EmailProperty.SMTPENABLESSL = ChkEnableSSL.Checked;

                EmailProperty.SMTPDISPLAYNAME = "SKE Diamonds";
                string pStrBody = htmlEditor.InnerHtml;
                // Instantiate a send mail class.
                MailMessage mailMessage = new MailMessage();
                //Sender's email address, method overload is different, you can choose according to your needs.
                mailMessage.From = new MailAddress(EmailProperty.SMTPEMAILUSERNAME, EmailProperty.SMTPDISPLAYNAME);
                //Recipient's email address.
                mailMessage.To.Add(new MailAddress(txtToEmail.Text));

                //mail title.
                mailMessage.Subject = txtSubject.Text;
                mailMessage.SubjectEncoding = Encoding.UTF8;
                //content of email.
                mailMessage.Body = pStrBody;
                mailMessage.BodyEncoding = Encoding.UTF8;
                // Is it html format
                mailMessage.IsBodyHtml = true;

                // Instantiate a SmtpClient class.
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                using (SmtpClient client1 = new SmtpClient())
                {
                    //I am using qq mailbox here, so it is smtp.qq.com. If you are using 126 mailbox, then it is smtp.126.com.
                    client1.Host = txtSMTPHost.Text;
                    //Use a secure encrypted connection.
                    client1.EnableSsl = true;
                    client1.Port = Val.ToInt(txtSMTPPort.Text);//456 port has been deprecated, see: https://stackoverflow.com/questions/20228644/smtpexception-unable-to-read-data-from-the-transport-connection-net- Io-connect
                    //Do not send with the request.
                    //client1.UseDefaultCredentials = false;
                    // Verify the sender's identity (the sender's mailbox, the generated authorization code in the mailbox);
                    client1.Credentials = new NetworkCredential(txtSMTPUserName.Text, txtSMTPPassword.Text);
                    //send
                    client1.Send(mailMessage);
                    Global.Message("Email Sent Successfully");

                    this.Cursor = Cursors.Default;
                }

                //End : Pinali : 14-10-2019

                    //                //string Str = new EmailService.Service1().SendGeneralEmail(txtToEmail.Text, txtCCEmail.Text, txtBCCEmail.Text, txtSubject.Text, htmlEditor.InnerHtml, FileByte, StrFileName);
                    ////////Start
                    //                //MailMessage objEmail = new MailMessage(from, to);

                    //                byte[] pByte = FileByte;
                    //                string pStrFileName = StrFileName;
                    //                string pStrBody = htmlEditor.InnerHtml;

                    //                EmailSettingProperty EmailProperty = new EmailSettingProperty();
                    //                EmailProperty.SMTPEMAILUSERNAME = txtSMTPUserName.Text;
                    //                EmailProperty.SMTPEMAILPASSWORD= txtSMTPPassword.Text;
                    //                EmailProperty.SMTPHOST = txtSMTPHost.Text;
                    //                EmailProperty.SMTPPORT = Val.ToInt(txtSMTPPort.Text);
                    //                EmailProperty.SMTPENABLESSL = ChkEnableSSL.Checked;
                    //                EmailProperty.SMTPDISPLAYNAME = "QQ id";

                    //                //System.Web.Mail.SmtpMail.SmtpServer = "mail.example.com";
                    //                //System.Web.Mail.SmtpMail.Send("530020268@qq.com", txtToEmail.Text, txtSubject.Text, pStrBody);

                    //                MailMessage objEmail = new MailMessage();
                    //                MailAddress from = new MailAddress(EmailProperty.SMTPEMAILUSERNAME, EmailProperty.SMTPDISPLAYNAME);
                    //                objEmail.From = from;

                    //                EmailProperty.TOEMAIL = txtToEmail.Text + "," + EmailProperty.TOEMAIL;

                    //                string[] EmailIDs = EmailProperty.TOEMAIL.Split(',');
                    //                int i;
                    //                for (i = 0; i < EmailIDs.Length; i++)
                    //                {
                    //                    if (EmailIDs[i].Contains("@") == true && EmailIDs[i].Contains(".") == true)
                    //                    {
                    //                        objEmail.To.Add(EmailIDs[i]);
                    //                    }
                    //                }

                    //                EmailProperty.CCEMAIL = txtCCEmail.Text + "," + EmailProperty.CCEMAIL;

                    //                string[] CCEmailIDs = EmailProperty.CCEMAIL.Split(',');

                    //                for (i = 0; i < CCEmailIDs.Length; i++)
                    //                {
                    //                    if (CCEmailIDs[i].Contains("@") == true && CCEmailIDs[i].Contains(".") == true)
                    //                    {
                    //                        objEmail.CC.Add(CCEmailIDs[i]);
                    //                    }
                    //                }

                    //                EmailProperty.BCCEMAIL = txtBCCEmail.Text + "," + EmailProperty.BCCEMAIL;

                    //                string[] BCCEmailIDs = EmailProperty.BCCEMAIL.Split(',');
                    //                for (i = 0; i < BCCEmailIDs.Length; i++)
                    //                {
                    //                    if (BCCEmailIDs[i].Contains("@") == true && BCCEmailIDs[i].Contains(".") == true)
                    //                    {
                    //                        objEmail.Bcc.Add(BCCEmailIDs[i]);
                    //                    }
                    //                }

                    //                if (pByte != null && pByte.Length != 0)
                    //                {
                    //                    objEmail.Attachments.Add(new Attachment(new MemoryStream(pByte), pStrFileName));
                    //                }

                    //                objEmail.Subject = txtSubject.Text;
                    //                objEmail.Body = pStrBody;
                    //                objEmail.IsBodyHtml = true;
                    //                objEmail.Priority = MailPriority.High;

                    //                SmtpClient client = new SmtpClient();
                    //                System.Net.NetworkCredential auth = new System.Net.NetworkCredential(EmailProperty.SMTPEMAILUSERNAME, EmailProperty.SMTPEMAILPASSWORD);
                    //                client.Host = EmailProperty.SMTPHOST;
                    //                client.Port = EmailProperty.SMTPPORT;
                    //                client.UseDefaultCredentials = false;
                    //                client.Credentials = auth;
                    //                client.EnableSsl = EmailProperty.SMTPENABLESSL;
                    //                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                    //                { return true; };

                    //                client.Send(objEmail);

                    //                MessageBox.Show("mail Send");
                    ///////////End


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
           
        }

        private void BtnNotification_Click(object sender, EventArgs e)
        {
            FrmNotification FrmNotification = new FrmNotification();
            FrmNotification.ShowAlert("This Is NotiFication", Resources.OrderPlace);
        }
    }
}