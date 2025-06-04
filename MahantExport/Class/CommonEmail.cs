using BusLib.Configuration;
using BusLib.TableName;
using BusLib.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace MahantExport.Class
{
    public class CommonEmail
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        #region Memo RelaredMail

        private string GetStoneBody(DataTable dtDiamonds, string OrderNo, string StrUserName)
        {
            int IntTotalPcs = dtDiamonds.Rows.Count;
            double DouTotalCarat = Val.Val(dtDiamonds.Compute("Sum(Carat)", ""));
            double DouTotalAmount = Val.Val(dtDiamonds.Compute("Sum(MemoAmount)", ""));
            double DouAvgRate = DouTotalCarat != 0 ? Math.Round(DouTotalAmount / DouTotalCarat, 2) : 0;

            StringBuilder sbBody = new StringBuilder();

            sbBody.Append("<div style=\"font-family:Verdana, Arial, Helvetica, sans-serif;font-size: 14px; font-weight: 600; text-align: left;\">Dear " + StrUserName + ",</div>");
            sbBody.Append("<div style=\"font-family:Verdana, Arial, Helvetica, sans-serif;font-size: 12px; font-weight: 400; padding: 12px 0px 0; letter-spacing: 1.2px;\">Thank you for your order !</div>");
            sbBody.Append("<div style=\"font-family:Verdana, Arial, Helvetica, sans-serif;font-size: 12px; font-weight: 400; padding: 12px 0px 0; letter-spacing: 1.2px;\">Please find following stone(s) details<br>thats you are confirmed<br>It is our privilege to work with you and we look forward<br>to continuing this business relationship further.<br><br><h4>Order No : " + OrderNo + " <br>Total Pcs : " + IntTotalPcs + "<br>Total Carat : " + DouTotalCarat + "<br>Avg $/Cts : " + DouAvgRate + "<br>Total Amt $ : " + DouTotalAmount + "</h4></div>");
            sbBody.Append("<div style=\"font-family:Verdana, Arial, Helvetica, sans-serif;font-size: 12px; font-weight: 400; padding: 12px 0px; letter-spacing: 1.2px;\">");
            sbBody.Append("</div>");
            sbBody.Append("</div>");
            sbBody.Append("<div style=\"width: 49%; padding:0 2% 0 0;\"><img style =\"width:75%;\" src=\"http://login.skrishna.co/Content/mail/ConfirmOrder.png\" alt=\"SKrishna World\"/></div> ");
            sbBody.Append("</div>");

            // Detail
            sbBody.Append("<div style = \"padding: 15px 0;\">");
            sbBody.Append("<table width = \"100%\">");
            sbBody.Append("<thead>");
            sbBody.Append("<tr style=\"font-family:Verdana, Arial, Helvetica, sans-serif;font-size: 12px;\">");
            sbBody.Append("<th style=\"background-color: #DFDFFF; padding:5px 8px; font-weight: 400; border:1px solid #a2a2a2;\">No.</th>");
            sbBody.Append("<th style=\"background-color: #DFDFFF; padding:5px 8px; font-weight: 400; border:1px solid #a2a2a2;\">Stone</th>");
            sbBody.Append("<th style=\"background-color: #DFDFFF; padding:5px 8px; font-weight: 400; border:1px solid #a2a2a2;\">Lab</th>");
            sbBody.Append("<th style=\"background-color: #DFDFFF; padding:5px 8px; font-weight: 400; border:1px solid #a2a2a2;\">Certificate</th>");
            sbBody.Append("<th style=\"background-color: #DFDFFF; padding:5px 8px; font-weight: 400; border:1px solid #a2a2a2;\">Shape</th>");
            sbBody.Append("<th style=\"background-color: #DFDFFF; padding:5px 8px; font-weight: 400; border:1px solid #a2a2a2;\">Ct</th>");
            sbBody.Append("<th style=\"background-color: #DFDFFF; padding:5px 8px; font-weight: 400; border:1px solid #a2a2a2;\">Color</th>");
            sbBody.Append("<th style=\"background-color: #DFDFFF; padding:5px 8px; font-weight: 400; border:1px solid #a2a2a2;\">Clarity</th>");
            sbBody.Append("<th style=\"background-color: #DFDFFF; padding:5px 8px; font-weight: 400; border:1px solid #a2a2a2;\">Cut</th>");
            sbBody.Append("<th style=\"background-color: #DFDFFF; padding:5px 8px; font-weight: 400; border:1px solid #a2a2a2;\">Pol</th>");
            sbBody.Append("<th style=\"background-color: #DFDFFF; padding:5px 8px; font-weight: 400; border:1px solid #a2a2a2;\">Sym</th>");
            sbBody.Append("<th style=\"background-color: #DFDFFF; padding:5px 8px; font-weight: 400; border:1px solid #a2a2a2;\">FL</th>");
            sbBody.Append("<th style=\"background-color: #DFDFFF; padding:5px 8px; font-weight: 400; border:1px solid #a2a2a2;\">Measurement</th>");
            sbBody.Append("<th style=\"background-color: #DFDFFF; padding:5px 8px; font-weight: 400; border:1px solid #a2a2a2;\">$/Cts</th>");
            sbBody.Append("<th style=\"background-color: #DFDFFF; padding:5px 8px; font-weight: 400; border:1px solid #a2a2a2;\">Amt $</th>");
            sbBody.Append("</tr>");
            sbBody.Append("</thead>");
            sbBody.Append("<tbody>");

            foreach (DataRow DRow in dtDiamonds.Rows)
            {
                sbBody.Append("<tr style=\"font-family:Verdana, Arial, Helvetica, sans-serif;font-size: 12px; text-align: center; \">");
                sbBody.Append("<td style=\"padding:5px 8px; font-weight:400; border:1px solid #a2a2a2;\">" + Val.ToString(DRow["ENTRYSRNO"]) + "</td>");
                sbBody.Append("<td style=\"padding:5px 0px; font-weight:400; border:1px solid #a2a2a2;\">" + Val.ToString(DRow["PARTYSTOCKNO"]) + "</td>");
                sbBody.Append("<td style=\"padding:5px 8px; font-weight:400; border:1px solid #a2a2a2;\">" + Val.ToString(DRow["LABNAME"]) + "</td>");
                sbBody.Append("<td style=\"padding:5px 8px; font-weight:400; border:1px solid #a2a2a2;\">" + Val.ToString(DRow["LABREPORTNO"]) + "</td>");
                sbBody.Append("<td style=\"padding:5px 8px; font-weight:400; border:1px solid #a2a2a2;\">" + Val.ToString(DRow["SHAPENAME"]) + "</td>");
                sbBody.Append("<td style=\"padding:5px 8px; font-weight:400; border:1px solid #a2a2a2;\">" + Val.ToString(DRow["CARAT"]) + "</td>");
                sbBody.Append("<td style=\"padding:5px 8px; font-weight:400; border:1px solid #a2a2a2;\">" + Val.ToString(DRow["COLORNAME"]) + "</td>");
                sbBody.Append("<td style=\"padding:5px 8px; font-weight:400; border:1px solid #a2a2a2;\">" + Val.ToString(DRow["CLARITYNAME"]) + "</td>");
                sbBody.Append("<td style=\"padding:5px 8px; font-weight:400; border:1px solid #a2a2a2;\">" + Val.ToString(DRow["CUTNAME"]) + "</td>");
                sbBody.Append("<td style=\"padding:5px 8px; font-weight:400; border:1px solid #a2a2a2;\">" + Val.ToString(DRow["POLNAME"]) + "</td>");
                sbBody.Append("<td style=\"padding:5px 8px; font-weight:400; border:1px solid #a2a2a2;\">" + Val.ToString(DRow["SYMNAME"]) + "</td>");
                sbBody.Append("<td style=\"padding:5px 8px; font-weight:400; border:1px solid #a2a2a2;\">" + Val.ToString(DRow["FLNAME"]) + "</td>");
                sbBody.Append("<td style=\"padding:5px 8px; font-weight:400; border:1px solid #a2a2a2;\">" + Val.ToString(DRow["MEASUREMENT"]) + "</td>");
                sbBody.Append("<td style=\"padding:5px 8px; font-weight:400; border:1px solid #a2a2a2;\">" + Val.ToString(DRow["MEMOPRICEPERCARAT"]) + "</td>");
                sbBody.Append("<td style=\"padding:5px 8px; font-weight:400; border:1px solid #a2a2a2;\">" + Val.ToString(DRow["MEMOAMOUNT"]) + "</td>");
                sbBody.Append("</tr>");
            }

            sbBody.Append("<tr>");
            sbBody.Append("<td colspan = \"15\" style = \"background-color: #DFDFFF; color: #16325a; text-align: right; padding:2px 5px; font-family:Verdana, Arial, Helvetica, sans-serif;font-size: 14px;font-weight: 600; border: 1px solid #a2a2a2\"> Total Amt $ : " + DouTotalAmount + " </td>");
            sbBody.Append("</tr>");
            sbBody.Append("</tbody>");
            sbBody.Append("</table>");
            sbBody.Append("</div>");
            return sbBody.ToString();
        }

        public string MemoSendEmail(string pStrMemoID)
        {
            BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();
            AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

            //DataSet DS = ObjMemo.GetMemoListData(0, null, null, "", "", "", 0, "", 0, "", "ALL", pStrMemoID, "SINGLE", false, false);
            DataSet DS = ObjMemo.GetMemoListData(0, null, null, "", "", "", 0, "", 0, "", "ALL", pStrMemoID, "SINGLE", false, -1);
            DataTable DTabMemo = DS.Tables[0];
            DataTable DTabMemoDetail = DS.Tables[1];

            if (DTabMemo.Rows.Count == 0)
            {
                return "";
            }

            DataRow DRowMain = DTabMemo.Rows[0];

            string OrderNo = Val.ToString(DRowMain["JANGEDNOSTR"]);
            string LedgerName = Val.ToString(DRowMain["BILLINGPARTYNAME"]);

            EmailSettingProperty EmailProperty = new BOEmailSettings().GetDataByProcessID(Val.ToInt(DRowMain["PROCESS_ID"]));
            string strheader = SetEmailHeader(EmailProperty.SUBJECT.Replace("Regarding", ""));
            string strStoneDetail = GetStoneBody(DTabMemoDetail, OrderNo, LedgerName);
            string strfooter = SetEmailFooter();

            string strbody = strheader + strStoneDetail + strfooter;

            string stReturnValue = SendMail(OrderNo, Val.ToString(DRowMain["BILLINGEMAIL"]), strbody, EmailProperty, "");
            return stReturnValue;
        }


        public static string SetEmailHeader(string StrTitle)
        {
            StringBuilder sbBody = new StringBuilder();

            sbBody.Append("<html>");
            sbBody.Append("<body>");
            sbBody.Append("<div style=\"width: 900px; background-color:#ffffff; color:#16325a\">");
            sbBody.Append("<div style=\"text-align: center;\"><img src=\"https://MahantExport.com/Content/mail/logo.png\" alt=\"Logo\"/></div>");
            //sbBody.Append("<div style=\"text-align: center;\"><img src=\"https://www.MahantExport.com/images/MahantExport.png\" alt =\"Shivam Gems World\"/></div>");
            sbBody.Append("<div style=\"text-align: center; text-transform: uppercase; padding: 4px 0; font-weight: 600;\"></div>");
            sbBody.Append("<div style=\"background-color: #ffffff; margin: 0 10px 10px; border: 1px solid #cccccc\">");
            sbBody.Append("<div style=\"width: 100%; display: inline-flex;\">");
            sbBody.Append("<div style=\"width: 49%; padding:4% 3%;\">");
            sbBody.Append("<h3 style=\"font-family:Georgia, 'Times New Roman', Times, serif; text-transform: uppercase; font-weight: 600; letter-spacing: 0px; font-size: 23px; line-height: 1.5;\">" + StrTitle + "</h3>");
            sbBody.Append("<div style=\"font-family:Verdana, Arial, Helvetica, sans-serif;font-size: 13px; font-weight: 700; text-align: left; padding-bottom: 25px;\"> This is auto - mail which is not monitored so do not reply to this email.</div>");

            return Convert.ToString(sbBody);
        }

        public static string SetEmailFooter()
        {
            StringBuilder sbBody = new StringBuilder();

            sbBody.Append("<div style=\"padding: 1% 3% 0;\">");
            sbBody.Append("<div style=\"font-family:Verdana, Arial, Helvetica, sans-serif;font-weight: 600; font-size: 12px; padding: 20px 0; line-height:20px\" >Thanks & Regards,<br>"+Global.gStrCompanyName+ "<br><br>Email ID: info@MahantExport.com<br>"); //yahoo
            sbBody.Append("Skype : MahantExport<br>Tel :-+919330303098<br>QBC: 9951<br>GW-5031, BHARAT DIAMOND BOURSE,<br>BANDRA KURLA COMPLEX,<br>BANDRA -E-<br>Mumbai,India-400 051.<br></div>");
            sbBody.Append("</div>");
            sbBody.Append("<div style=\"width: 100%; display: inline-flex; position: relative; z-index: 1111;\">");
            sbBody.Append("<div style=\"width: 49%; text-align: center; position: relative;\"></div>");
            sbBody.Append("<div style=\"width: 50%; text-align: right; position: relative;\">");
            //sbBody.Append("<a style=\"text-decoration: none; text-align: right; padding-left: 5px;\" href=\"https://www.MahantExport.com/\" target=\"_blank\"><img style=\"width: 30px;\" src=\"http://login.skrishna.co/Content/mail/apple.png\" alt=\"Application\"/></a>");
            //sbBody.Append("<a style=\"text-decoration: none; text-align: right; padding-left: 5px;\" href=\"https://www.MahantExport.com/\" target=\"_blank\"><img style=\"width: 30px;\" src=\"http://login.skrishna.co/Content/mail/android.png\" alt=\"Application\"/></a>");
            //sbBody.Append("<a style=\"text-decoration: none; text-align: right; padding-left: 5px;\" href=\"https://www.MahantExport.com/\" target=\"_blank\" ><img style = \"width: 30px;\" src=\"http://login.skrishna.co/Content/mail/fb.png\" alt=\"Facebook\"/></a>");
            //sbBody.Append("<a style=\"text-decoration: none; text-align: right; padding-left: 5px;\" href=\"https://www.MahantExport.com/\" target=\"_blank\"><img style = \"width: 30px;\" src=\"http://login.skrishna.co/Content/mail/insta.png\" alt=\"Instagram\"/></a>");
            //sbBody.Append("<a style=\"text-decoration: none; text-align: right; padding-left: 5px;\" href=\"https://www.MahantExport.com/\" target=\"_blank\"><img style=\"width: 30px;\" src=\"http://login.skrishna.co/Content/mail/in.png\" alt=\"Instagram\"/></a></div>");
            sbBody.Append("<a style=\"text-decoration: none; text-align: right; padding-left: 5px;\" href=\"https://www.MahantExport.com/\" target=\"_blank\"><img style=\"width: 30px;\" src=\"https://www.MahantExport.com/Content/mail/apple.png\" alt=\"Application\"/></a>");
            sbBody.Append("<a style=\"text-decoration: none; text-align: right; padding-left: 5px;\" href=\"https://www.MahantExport.com/\" target=\"_blank\"><img style=\"width: 30px;\" src=\"https://www.MahantExport.com/Content/mail/android.png\" alt=\"Application\"/></a>");
            sbBody.Append("<a style=\"text-decoration: none; text-align: right; padding-left: 5px;\" href=\"https://www.MahantExport.com/\" target=\"_blank\" ><img style = \"width: 30px;\" src=\"https://www.MahantExport.com/Content/mail/fb.png\" alt=\"Facebook\"/></a>");
            sbBody.Append("<a style=\"text-decoration: none; text-align: right; padding-left: 5px;\" href=\"https://www.MahantExport.com/\" target=\"_blank\"><img style = \"width: 30px;\" src=\"https://www.MahantExport.com/Content/mail/insta.png\" alt=\"Instagram\"/></a>");
            sbBody.Append("<a style=\"text-decoration: none; text-align: right; padding-left: 5px;\" href=\"https://www.MahantExport.com/\" target=\"_blank\"><img style=\"width: 30px;\" src=\"https://www.MahantExport.com/Content/mail/in.png\" alt=\"Instagram\"/></a></div>");

            sbBody.Append("</div>");
            sbBody.Append("</div>");
            sbBody.Append("<div style=\"background-image: url(https://www.MahantExport.com/Content/mail/footer-blue.png); background-size: contain; background-position: center; background-repeat: no-repeat; color: #ffffff; position: relative; margin-top: -5%; text-align: center; padding: 8px 22px;\"><a href=\"https://www.MahantExport.com/\" target=\"_blank\" style = \"position: relative; z-index: 1111; text-transform: uppercase; font-weight: 600; font-size: 19px; color: #ffffff; text-decoration: none;\"> www.MahantExport.com </a></div>");
            sbBody.Append("</div>");
            sbBody.Append("</body>");
            sbBody.Append("</html>");

            return Convert.ToString(sbBody);
        }


        private string SendMail(string StrOrderNo, string pStrEmailID, string stHtmlBody, EmailSettingProperty pClsProperty, string pStrAttachment)
        {
            try
            {
                string stReturnText = string.Empty;

                StringBuilder sbHTML = new StringBuilder();
                sbHTML.Append(stHtmlBody);

                //MailMessage objEmail = new MailMessage(from, to);
                MailMessage objEmail = new MailMessage();
                MailAddress from = new MailAddress(pClsProperty.SMTPEMAILUSERNAME, pClsProperty.SMTPDISPLAYNAME);
                objEmail.From = from;

                pClsProperty.TOEMAIL = pStrEmailID + "," + pClsProperty.TOEMAIL;

                string[] EmailIDs = pClsProperty.TOEMAIL.Split(',');
                int i;
                for (i = 0; i < EmailIDs.Length; i++)
                {
                    if (EmailIDs[i].Contains("@") == true && EmailIDs[i].Contains(".") == true)
                    {
                        objEmail.To.Add(EmailIDs[i]);
                    }
                }
                string[] CCEmailIDs = pClsProperty.CCEMAIL.Split(',');

                for (i = 0; i < CCEmailIDs.Length; i++)
                {
                    if (CCEmailIDs[i].Contains("@") == true && CCEmailIDs[i].Contains(".") == true)
                    {
                        objEmail.CC.Add(CCEmailIDs[i]);
                    }
                }
                string[] BCCEmailIDs = pClsProperty.BCCEMAIL.Split(',');
                for (i = 0; i < BCCEmailIDs.Length; i++)
                {
                    if (BCCEmailIDs[i].Contains("@") == true && BCCEmailIDs[i].Contains(".") == true)
                    {
                        objEmail.Bcc.Add(BCCEmailIDs[i]);
                    }
                }

                objEmail.Subject = pClsProperty.SUBJECT + " of No : " + StrOrderNo;
                objEmail.Body = sbHTML.ToString();
                objEmail.IsBodyHtml = true;
                objEmail.Priority = MailPriority.High;

                if (pStrAttachment != "")
                {
                    Attachment oAttachment = new Attachment(pStrAttachment);
                    objEmail.Attachments.Add(oAttachment);
                }


                SmtpClient client = new SmtpClient();
                System.Net.NetworkCredential auth = new System.Net.NetworkCredential(pClsProperty.SMTPEMAILUSERNAME, pClsProperty.SMTPEMAILPASSWORD);
                client.Host = pClsProperty.SMTPHOST;
                client.Port = pClsProperty.SMTPPORT;
                client.UseDefaultCredentials = false;
                client.Credentials = auth;
                client.EnableSsl = pClsProperty.SMTPENABLESSL;


                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };



                client.Send(objEmail);

                return "SUCCESS";

            }
            catch (Exception EX)
            {
                return EX.Message.ToString();
            }
        }

        #endregion

        #region Registration Approval


        public string RegistrationApproval(string pStrLedgerName, string pStrEmailID, string pStrUserName, string pStrStatus, string pStrSallerEmailID)
        {
            StringBuilder sbBody = new StringBuilder();

            sbBody.Append("<div style=\"font-family:Verdana, Arial, Helvetica, sans-serif;font-size: 14px; font-weight: 600; text-align: left;\">Dear " + pStrLedgerName + ",</div>");
            sbBody.Append("<div style=\"font-family:Verdana, Arial, Helvetica, sans-serif;font-size: 12px; font-weight: 400; padding: 12px 0px 0; letter-spacing: 1.2px;\">As per your account verification process , Rijiya Gems has change status of your account</div>");
            sbBody.Append("<div style=\"font-family:Verdana, Arial, Helvetica, sans-serif;font-size: 12px; font-weight: 400; padding: 12px 0px; letter-spacing: 1.2px;\">");
            sbBody.Append("<p>Your account status for MahantExport.com login is: <strong>" + pStrStatus + "</strong></p>");
            sbBody.Append("</div>");
            sbBody.Append("</div>");
            sbBody.Append("<div style=\"width: 49%; padding:0 2% 0 0;\"><img style =\"width:80%;\" src=\"https://MahantExport.com/Content/mail/Activity.png\" alt=\"Rijiya\"/></div> ");
            sbBody.Append("</div>");

            string StrFinalBody = SetEmailHeader("Your Account Is [ " + pStrStatus + " ] ") + sbBody.ToString() + SetEmailFooter();

            EmailSettingProperty EmailProperty = new BOEmailSettings().GetDataByPK("RegistrationApproval");
            string strbody = StrFinalBody.ToString();

            string stReturnValue = SendMail(strbody, pStrEmailID, EmailProperty);//, pStrSallerEmailID);
            return stReturnValue;
        }
        private string SendMail(string stHtmlBody, string pStrToEmail, EmailSettingProperty pClsProperty)
        {
            try
            {
                string stReturnText = string.Empty;

                StringBuilder sbHTML = new StringBuilder();
                sbHTML.Append(stHtmlBody);

                //MailMessage objEmail = new MailMessage(from, to);
                MailMessage objEmail = new MailMessage();
                MailAddress from = new MailAddress(pClsProperty.SMTPEMAILUSERNAME, pClsProperty.SMTPDISPLAYNAME);
                objEmail.From = from;

                pClsProperty.TOEMAIL = pStrToEmail + "," + pClsProperty.TOEMAIL;
               

                string[] EmailIDs = pClsProperty.TOEMAIL.Split(',');
                int i;
                for (i = 0; i < EmailIDs.Length; i++)
                {
                    if (EmailIDs[i].Contains("@") == true && EmailIDs[i].Contains(".") == true)
                    {
                        objEmail.To.Add(EmailIDs[i]);
                    }
                }
                string[] CCEmailIDs = pClsProperty.CCEMAIL.Split(',');

                for (i = 0; i < CCEmailIDs.Length; i++)
                {
                    if (CCEmailIDs[i].Contains("@") == true && CCEmailIDs[i].Contains(".") == true)
                    {
                        objEmail.CC.Add(CCEmailIDs[i]);
                    }
                }
                string[] BCCEmailIDs = pClsProperty.BCCEMAIL.Split(',');
                for (i = 0; i < BCCEmailIDs.Length; i++)
                {
                    if (BCCEmailIDs[i].Contains("@") == true && BCCEmailIDs[i].Contains(".") == true)
                    {
                        objEmail.Bcc.Add(BCCEmailIDs[i]);
                    }
                }

                objEmail.Subject = pClsProperty.SUBJECT;
                objEmail.Body = sbHTML.ToString();
                objEmail.IsBodyHtml = true;
                objEmail.Priority = MailPriority.High;

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                SmtpClient client = new SmtpClient();
                System.Net.NetworkCredential auth = new System.Net.NetworkCredential(pClsProperty.SMTPEMAILUSERNAME, pClsProperty.SMTPEMAILPASSWORD);
                client.Host = pClsProperty.SMTPHOST;
                client.Port = pClsProperty.SMTPPORT;
                client.UseDefaultCredentials = false;
                client.Credentials = auth;
                client.EnableSsl = pClsProperty.SMTPENABLESSL;
                ServicePointManager.ServerCertificateValidationCallback = delegate (object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

                client.Send(objEmail);
                return "SUCCESS";

            }
            catch (Exception EX)
            {
                return EX.Message;
            }
        }

        #endregion

        #region General Emails

        public string SendGeneralEmail(string pStrToEmail, string pStrCCEmail, string pStrBCCEmail, string pStrSubject, string pStrBody, byte[] pByte, string pStrFileName)
        {
            try
            {

                EmailSettingProperty EmailProperty = new BOEmailSettings().GetDataByEmailType(BOEmailSettings.EmailType.GeneralEmail);

                string stReturnText = string.Empty;


                //MailMessage objEmail = new MailMessage(from, to);
                MailMessage objEmail = new MailMessage();
                MailAddress from = new MailAddress(EmailProperty.SMTPEMAILUSERNAME, EmailProperty.SMTPDISPLAYNAME);
                objEmail.From = from;

                EmailProperty.TOEMAIL = pStrToEmail + "," + EmailProperty.TOEMAIL;

                string[] EmailIDs = EmailProperty.TOEMAIL.Split(',');
                int i;
                for (i = 0; i < EmailIDs.Length; i++)
                {
                    if (EmailIDs[i].Contains("@") == true && EmailIDs[i].Contains(".") == true)
                    {
                        objEmail.To.Add(EmailIDs[i]);
                    }
                }

                EmailProperty.CCEMAIL = pStrCCEmail + "," + EmailProperty.CCEMAIL;

                string[] CCEmailIDs = EmailProperty.CCEMAIL.Split(',');

                for (i = 0; i < CCEmailIDs.Length; i++)
                {
                    if (CCEmailIDs[i].Contains("@") == true && CCEmailIDs[i].Contains(".") == true)
                    {
                        objEmail.CC.Add(CCEmailIDs[i]);
                    }
                }

                EmailProperty.BCCEMAIL = pStrBCCEmail + "," + EmailProperty.BCCEMAIL;

                string[] BCCEmailIDs = EmailProperty.BCCEMAIL.Split(',');
                for (i = 0; i < BCCEmailIDs.Length; i++)
                {
                    if (BCCEmailIDs[i].Contains("@") == true && BCCEmailIDs[i].Contains(".") == true)
                    {
                        objEmail.Bcc.Add(BCCEmailIDs[i]);
                    }
                }

                if (pByte != null && pByte.Length != 0)
                {
                    objEmail.Attachments.Add(new Attachment(new MemoryStream(pByte), pStrFileName));
                }

                objEmail.Subject = pStrSubject;
                objEmail.Body = pStrBody;
                objEmail.IsBodyHtml = true;
                objEmail.Priority = MailPriority.High;

                SmtpClient client = new SmtpClient();
                System.Net.NetworkCredential auth = new System.Net.NetworkCredential(EmailProperty.SMTPEMAILUSERNAME, EmailProperty.SMTPEMAILPASSWORD);
                client.Host = EmailProperty.SMTPHOST;
                client.Port = EmailProperty.SMTPPORT;
                client.UseDefaultCredentials = false;
                client.Credentials = auth;
                client.EnableSsl = EmailProperty.SMTPENABLESSL;
                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

                client.Send(objEmail);

                return "Email Sent Successfully";

            }
            catch (Exception EX)
            {
                return EX.Message;
            }
        }

        public string SendGeneralEmailAsync(string pStrToEmail, string pStrCCEmail, string pStrBCCEmail, string pStrSubject, string pStrBody, byte[] pByte, string pStrFileName)
        {
            try
            {
                EmailSettingProperty EmailProperty = new BOEmailSettings().GetDataByEmailType(BOEmailSettings.EmailType.GeneralEmail);

                string stReturnText = string.Empty;

                //MailMessage objEmail = new MailMessage(from, to);
                MailMessage objEmail = new MailMessage();
                MailAddress from = new MailAddress(EmailProperty.SMTPEMAILUSERNAME, EmailProperty.SMTPDISPLAYNAME);
                objEmail.From = from;

                EmailProperty.TOEMAIL = pStrToEmail + "," + EmailProperty.TOEMAIL;

                string[] EmailIDs = EmailProperty.TOEMAIL.Split(',');
                int i;
                for (i = 0; i < EmailIDs.Length; i++)
                {
                    if (EmailIDs[i].Contains("@") == true && EmailIDs[i].Contains(".") == true)
                    {
                        objEmail.To.Add(EmailIDs[i]);
                    }
                }

                EmailProperty.CCEMAIL = pStrCCEmail + "," + EmailProperty.CCEMAIL;

                string[] CCEmailIDs = EmailProperty.CCEMAIL.Split(',');

                for (i = 0; i < CCEmailIDs.Length; i++)
                {
                    if (CCEmailIDs[i].Contains("@") == true && CCEmailIDs[i].Contains(".") == true)
                    {
                        objEmail.CC.Add(CCEmailIDs[i]);
                    }
                }

                EmailProperty.BCCEMAIL = pStrBCCEmail + "," + EmailProperty.BCCEMAIL;

                string[] BCCEmailIDs = EmailProperty.BCCEMAIL.Split(',');
                for (i = 0; i < BCCEmailIDs.Length; i++)
                {
                    if (BCCEmailIDs[i].Contains("@") == true && BCCEmailIDs[i].Contains(".") == true)
                    {
                        objEmail.Bcc.Add(BCCEmailIDs[i]);
                    }
                }

                if (pByte != null && pByte.Length != 0)
                {
                    objEmail.Attachments.Add(new Attachment(new MemoryStream(pByte), pStrFileName));
                }

                objEmail.Subject = pStrSubject;
                objEmail.Body = pStrBody;
                objEmail.IsBodyHtml = true;
                objEmail.Priority = MailPriority.High;

                SmtpClient client = new SmtpClient();
                System.Net.NetworkCredential auth = new System.Net.NetworkCredential(EmailProperty.SMTPEMAILUSERNAME, EmailProperty.SMTPEMAILPASSWORD);
                client.Host = EmailProperty.SMTPHOST;
                client.Port = EmailProperty.SMTPPORT;
                client.UseDefaultCredentials = false;
                client.Credentials = auth;
                client.EnableSsl = EmailProperty.SMTPENABLESSL;
                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

                client.SendAsync(objEmail, null);

                return "Email Sent Successfully";

            }
            catch (Exception EX)
            {
                return EX.Message;
            }
        }

        public string SendGeneralEmail(string pStrToEmail, string pStrCCEmail, string pStrBCCEmail, string pStrSubject, string pStrBody, string pStrAttachment, string pStrFileName)
        {
            try
            {

                EmailSettingProperty EmailProperty = new BOEmailSettings().GetDataByEmailType(BOEmailSettings.EmailType.GeneralEmail);

                string stReturnText = string.Empty;


                //MailMessage objEmail = new MailMessage(from, to);
                MailMessage objEmail = new MailMessage();
                MailAddress from = new MailAddress(EmailProperty.SMTPEMAILUSERNAME, EmailProperty.SMTPDISPLAYNAME);
                objEmail.From = from;

                EmailProperty.TOEMAIL = pStrToEmail + "," + EmailProperty.TOEMAIL;

                string[] EmailIDs = EmailProperty.TOEMAIL.Split(',');
                int i;
                for (i = 0; i < EmailIDs.Length; i++)
                {
                    if (EmailIDs[i].Contains("@") == true && EmailIDs[i].Contains(".") == true)
                    {
                        objEmail.To.Add(EmailIDs[i]);
                    }
                }

                EmailProperty.CCEMAIL = pStrCCEmail + "," + EmailProperty.CCEMAIL;

                string[] CCEmailIDs = EmailProperty.CCEMAIL.Split(',');

                for (i = 0; i < CCEmailIDs.Length; i++)
                {
                    if (CCEmailIDs[i].Contains("@") == true && CCEmailIDs[i].Contains(".") == true)
                    {
                        objEmail.CC.Add(CCEmailIDs[i]);
                    }
                }

                EmailProperty.BCCEMAIL = pStrBCCEmail + "," + EmailProperty.BCCEMAIL;

                string[] BCCEmailIDs = EmailProperty.BCCEMAIL.Split(',');
                for (i = 0; i < BCCEmailIDs.Length; i++)
                {
                    if (BCCEmailIDs[i].Contains("@") == true && BCCEmailIDs[i].Contains(".") == true)
                    {
                        objEmail.Bcc.Add(BCCEmailIDs[i]);
                    }
                }

                if (pStrAttachment != "")
                {
                    objEmail.Attachments.Add(new Attachment(pStrAttachment));
                }

                objEmail.Subject = pStrSubject;
                objEmail.Body = pStrBody;
                objEmail.IsBodyHtml = true;
                objEmail.Priority = MailPriority.High;

                SmtpClient client = new SmtpClient();
                System.Net.NetworkCredential auth = new System.Net.NetworkCredential(EmailProperty.SMTPEMAILUSERNAME, EmailProperty.SMTPEMAILPASSWORD);
                client.Host = EmailProperty.SMTPHOST;
                client.Port = EmailProperty.SMTPPORT;
                client.UseDefaultCredentials = false;
                client.Credentials = auth;
                client.EnableSsl = EmailProperty.SMTPENABLESSL;
                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

                client.Send(objEmail);

                return "Email Sent Successfully";

            }
            catch (Exception EX)
            {
                return EX.Message;
            }
        }

        public string SendGeneralEmailAsync(string pStrToEmail, string pStrCCEmail, string pStrBCCEmail, string pStrSubject, string pStrBody, string pStrAttachment, string pStrFileName)
        {
            try
            {
                EmailSettingProperty EmailProperty = new BOEmailSettings().GetDataByEmailType(BOEmailSettings.EmailType.GeneralEmail);

                string stReturnText = string.Empty;

                //MailMessage objEmail = new MailMessage(from, to);
                MailMessage objEmail = new MailMessage();
                MailAddress from = new MailAddress(EmailProperty.SMTPEMAILUSERNAME, EmailProperty.SMTPDISPLAYNAME);
                objEmail.From = from;

                EmailProperty.TOEMAIL = pStrToEmail + "," + EmailProperty.TOEMAIL;

                string[] EmailIDs = EmailProperty.TOEMAIL.Split(',');
                int i;
                for (i = 0; i < EmailIDs.Length; i++)
                {
                    if (EmailIDs[i].Contains("@") == true && EmailIDs[i].Contains(".") == true)
                    {
                        objEmail.To.Add(EmailIDs[i]);
                    }
                }

                EmailProperty.CCEMAIL = pStrCCEmail + "," + EmailProperty.CCEMAIL;

                string[] CCEmailIDs = EmailProperty.CCEMAIL.Split(',');

                for (i = 0; i < CCEmailIDs.Length; i++)
                {
                    if (CCEmailIDs[i].Contains("@") == true && CCEmailIDs[i].Contains(".") == true)
                    {
                        objEmail.CC.Add(CCEmailIDs[i]);
                    }
                }

                EmailProperty.BCCEMAIL = pStrBCCEmail + "," + EmailProperty.BCCEMAIL;

                string[] BCCEmailIDs = EmailProperty.BCCEMAIL.Split(',');
                for (i = 0; i < BCCEmailIDs.Length; i++)
                {
                    if (BCCEmailIDs[i].Contains("@") == true && BCCEmailIDs[i].Contains(".") == true)
                    {
                        objEmail.Bcc.Add(BCCEmailIDs[i]);
                    }
                }

                if (pStrAttachment != "")
                {
                    objEmail.Attachments.Add(new Attachment(pStrAttachment));
                }

                objEmail.Subject = pStrSubject;
                objEmail.Body = pStrBody;
                objEmail.IsBodyHtml = true;
                objEmail.Priority = MailPriority.High;

                SmtpClient client = new SmtpClient();
                System.Net.NetworkCredential auth = new System.Net.NetworkCredential(EmailProperty.SMTPEMAILUSERNAME, EmailProperty.SMTPEMAILPASSWORD);
                client.Host = EmailProperty.SMTPHOST;
                client.Port = EmailProperty.SMTPPORT;
                client.UseDefaultCredentials = false;
                client.Credentials = auth;
                client.EnableSsl = EmailProperty.SMTPENABLESSL;
                ServicePointManager.ServerCertificateValidationCallback = delegate(object s, System.Security.Cryptography.X509Certificates.X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };

                client.SendAsync(objEmail, null);

                return "Email Sent Successfully";

            }
            catch (Exception EX)
            {
                return EX.Message;
            }
        }
        public string APIApproval(string pStrLedgerName, string pStrTokenID, string pStrEmailID, string pStrUserName, string pStrStatus)
        {
            StringBuilder sbBody = new StringBuilder();

            sbBody.Append("<div style=\"font-family:Verdana, Arial, Helvetica, sans-serif;font-size: 14px; font-weight: 600; text-align: left;\">Dear " + pStrLedgerName + ",</div>");
            sbBody.Append("<div style=\"font-family:Verdana, Arial, Helvetica, sans-serif;font-size: 12px; font-weight: 400; padding: 12px 0px 0; letter-spacing: 1.2px;\">As per your request to acces Shree Krishna Export Stock via API , Shree Krishna Export has allowed you to get the stock</div>");
            sbBody.Append("<div style=\"font-family:Verdana, Arial, Helvetica, sans-serif;font-size: 12px; font-weight: 400; padding: 12px 0px; letter-spacing: 1.2px;\">");
            sbBody.Append("<p>Your Token No : <strong>" + pStrTokenID + "</strong><br></p>");
            //sbBody.Append("<p>Your Token No : <strong>" + pStrTokenID + "</strong><br><br>API Link : <strong>https://skrishna.co/SkeApiWebservice/webservice.asmx</strong><br><br>API Help Doc : <strong>https://skrishna.co/APIHelpDesk.pdf</strong><br><br></p>");
            sbBody.Append("</div>");
            sbBody.Append("</div>");
            sbBody.Append("<div style=\"width: 49%; padding:0 2% 0 0;\"><img style =\"width:80%;\" src=\"http://login.skrishna.co/Content/mail/ConfirmOrder.png\" alt=\"SKrishna\"/></div> ");
            sbBody.Append("</div>");

            string StrFinalBody = SetEmailHeader("STOCK API ACCESS") + sbBody.ToString() + SetEmailFooter();

            EmailSettingProperty EmailProperty = new BOEmailSettings().GetDataByPK("APIMail");
            string strbody = StrFinalBody.ToString();

            string stReturnValue = SendMail(strbody, pStrEmailID, EmailProperty);
            return stReturnValue;
        }

        #endregion


    }
}
