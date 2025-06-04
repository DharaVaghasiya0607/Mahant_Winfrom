using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using System.Globalization;
using BusLib.TableName;

namespace BusLib.Configuration
{
    public class BOEmailSettings
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        public enum EmailType
        {
            Registration = 1,
            RegistrationApproval = 2,
            ForgotPassword = 3,
            ContactUS = 6,
            DailySalesEmail = 11,
            GeneralEmail = 20,
        }

        public EmailSettingProperty GetDataByPK(int pIntID)
        {
            AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();

            EmailSettingProperty Property = new EmailSettingProperty();
            try
            {
                Ope.ClearParams();
                string StrSql = "SELECT * FROM MST_EmailSetting With(NOLock) Where EmailID = '" + pIntID + "' ";
                DataRow DRow = Ope.GetDataRow(BusLib.Configuration.BOConfiguration.ConnectionString, BusLib.Configuration.BOConfiguration.ProviderName, StrSql, CommandType.Text);
                if (DRow == null)
                {
                    Property.EMAIL_ID = 0;
                    Property.EMAILTYPE = string.Empty;
                    Property.SMTPEMAILUSERNAME = string.Empty;
                    Property.SMTPEMAILPASSWORD = string.Empty;
                    Property.SMTPDISPLAYNAME = string.Empty;
                    Property.SMTPHOST = string.Empty;
                    Property.SMTPPORT  = 0;
                    Property.SMTPENABLESSL  = false;
                    Property.TOEMAIL = string.Empty;
                    Property.CCEMAIL = string.Empty;
                    Property.BCCEMAIL = string.Empty;
                    Property.SUBJECT = string.Empty;
                    Property.HTMLBODY  = string.Empty;
                    Property.ISACTIVE = false;
                }
                else
                {
                    Property.EMAIL_ID = Val.ToInt(DRow["EMAIL_ID"]); ;
                    Property.EMAILTYPE = Val.ToString(DRow["EMAILTYPE"]); ;
                    Property.SMTPEMAILUSERNAME = Val.ToString(DRow["SMTPEMAILUSERNAME"]); ;
                    Property.SMTPEMAILPASSWORD = Val.ToString(DRow["SMTPEMAILPASSWORD"]); ;
                    Property.SMTPDISPLAYNAME = Val.ToString(DRow["SMTPDISPLAYNAME"]); ;
                    Property.SMTPHOST = Val.ToString(DRow["SMTPHOST"]); ;
                    Property.SMTPPORT = Val.ToInt(DRow["SMTPPORT"]); ;
                    Property.SMTPENABLESSL = Val.ToBoolean(DRow["SMTPENABLESSL"]); ;
                    Property.TOEMAIL = Val.ToString(DRow["TOEMAIL"]); ;
                    Property.CCEMAIL = Val.ToString(DRow["CCEMAIL"]); ;
                    Property.BCCEMAIL = Val.ToString(DRow["BCCEMAIL"]); ;
                    Property.SUBJECT = Val.ToString(DRow["SUBJECT"]); ;
                    Property.HTMLBODY = Val.ToString(DRow["HTMLBODY"]); ;
                    Property.ISACTIVE = Val.ToBoolean(DRow["ISACTIVE"]); ;
                
                }

                Ope = null;
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
            }
            return Property;

        }

        public EmailSettingProperty GetDataByPK(string pStrName)
        {
            AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();

            EmailSettingProperty Property = new EmailSettingProperty();
            try
            {
                Ope.ClearParams();
                string StrSql = "SELECT * FROM MST_EmailSetting With(NOLock) Where EmailType = '" + pStrName + "' ";
                DataRow DRow = Ope.GetDataRow(BusLib.Configuration.BOConfiguration.ConnectionString, BusLib.Configuration.BOConfiguration.ProviderName, StrSql, CommandType.Text);
                if (DRow == null)
                {
                    Property.EMAIL_ID = 0;
                    Property.EMAILTYPE = string.Empty;
                    Property.SMTPEMAILUSERNAME = string.Empty;
                    Property.SMTPEMAILPASSWORD = string.Empty;
                    Property.SMTPDISPLAYNAME = string.Empty;
                    Property.SMTPHOST = string.Empty;
                    Property.SMTPPORT = 0;
                    Property.SMTPENABLESSL = false;
                    Property.TOEMAIL = string.Empty;
                    Property.CCEMAIL = string.Empty;
                    Property.BCCEMAIL = string.Empty;
                    Property.SUBJECT = string.Empty;
                    Property.HTMLBODY = string.Empty;
                    Property.ISACTIVE = false;
                }
                else
                {
                    Property.EMAIL_ID = Val.ToInt(DRow["EMAIL_ID"]); ;
                    Property.EMAILTYPE = Val.ToString(DRow["EMAILTYPE"]); ;
                    Property.SMTPEMAILUSERNAME = Val.ToString(DRow["SMTPEMAILUSERNAME"]); ;
                    Property.SMTPEMAILPASSWORD = Val.ToString(DRow["SMTPEMAILPASSWORD"]); ;
                    Property.SMTPDISPLAYNAME = Val.ToString(DRow["SMTPDISPLAYNAME"]); ;
                    Property.SMTPHOST = Val.ToString(DRow["SMTPHOST"]); ;
                    Property.SMTPPORT = Val.ToInt(DRow["SMTPPORT"]); ;
                    Property.SMTPENABLESSL = Val.ToBoolean(DRow["SMTPENABLESSL"]); ;
                    Property.TOEMAIL = Val.ToString(DRow["TOEMAIL"]); ;
                    Property.CCEMAIL = Val.ToString(DRow["CCEMAIL"]); ;
                    Property.BCCEMAIL = Val.ToString(DRow["BCCEMAIL"]); ;
                    Property.SUBJECT = Val.ToString(DRow["SUBJECT"]); ;
                    Property.HTMLBODY = Val.ToString(DRow["HTMLBODY"]); ;
                    Property.ISACTIVE = Val.ToBoolean(DRow["ISACTIVE"]); ;

                }

                Ope = null;
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
            }
            return Property;

        }


        public EmailSettingProperty GetDataByEmailType(EmailType pEmailType)
        {
            AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();

            EmailSettingProperty Property = new EmailSettingProperty();
            try
            {
                Ope.ClearParams();
                string StrSql = "SELECT * FROM MST_EmailSetting With(NOLock) Where EmailType = '" + pEmailType.ToString() + "' ";
                DataRow DRow = Ope.GetDataRow(BusLib.Configuration.BOConfiguration.ConnectionString, BusLib.Configuration.BOConfiguration.ProviderName, StrSql, CommandType.Text);
                if (DRow == null)
                {
                    Property.EMAIL_ID = 0;
                    Property.EMAILTYPE = string.Empty;
                    Property.SMTPEMAILUSERNAME = string.Empty;
                    Property.SMTPEMAILPASSWORD = string.Empty;
                    Property.SMTPDISPLAYNAME = string.Empty;
                    Property.SMTPHOST = string.Empty;
                    Property.SMTPPORT = 0;
                    Property.SMTPENABLESSL = false;
                    Property.TOEMAIL = string.Empty;
                    Property.CCEMAIL = string.Empty;
                    Property.BCCEMAIL = string.Empty;
                    Property.SUBJECT = string.Empty;
                    Property.HTMLBODY = string.Empty;
                    Property.ISACTIVE = false;
                }
                else
                {
                    Property.EMAIL_ID = Val.ToInt(DRow["EMAIL_ID"]); ;
                    Property.EMAILTYPE = Val.ToString(DRow["EMAILTYPE"]); ;
                    Property.SMTPEMAILUSERNAME = Val.ToString(DRow["SMTPEMAILUSERNAME"]); ;
                    Property.SMTPEMAILPASSWORD = Val.ToString(DRow["SMTPEMAILPASSWORD"]); ;
                    Property.SMTPDISPLAYNAME = Val.ToString(DRow["SMTPDISPLAYNAME"]); ;
                    Property.SMTPHOST = Val.ToString(DRow["SMTPHOST"]); ;
                    Property.SMTPPORT = Val.ToInt(DRow["SMTPPORT"]); ;
                    Property.SMTPENABLESSL = Val.ToBoolean(DRow["SMTPENABLESSL"]); ;
                    Property.TOEMAIL = Val.ToString(DRow["TOEMAIL"]); ;
                    Property.CCEMAIL = Val.ToString(DRow["CCEMAIL"]); ;
                    Property.BCCEMAIL = Val.ToString(DRow["BCCEMAIL"]); ;
                    Property.SUBJECT = Val.ToString(DRow["SUBJECT"]); ;
                    Property.HTMLBODY = Val.ToString(DRow["HTMLBODY"]); ;
                    Property.ISACTIVE = Val.ToBoolean(DRow["ISACTIVE"]); ;

                }

                Ope = null;
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
            }
            return Property;

        }

        public EmailSettingProperty GetDataByProcessID(int pIntProcessID)
        {
            AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();

            EmailSettingProperty Property = new EmailSettingProperty();
            try
            {
                Ope.ClearParams();
                string StrSql = "SELECT * FROM MST_EmailSetting With(NOLock) Where Process_ID = '" + pIntProcessID + "' ";
                DataRow DRow = Ope.GetDataRow(BusLib.Configuration.BOConfiguration.ConnectionString, BusLib.Configuration.BOConfiguration.ProviderName, StrSql, CommandType.Text);
                if (DRow == null)
                {
                    Property.EMAIL_ID = 0;
                    Property.EMAILTYPE = string.Empty;
                    Property.SMTPEMAILUSERNAME = string.Empty;
                    Property.SMTPEMAILPASSWORD = string.Empty;
                    Property.SMTPDISPLAYNAME = string.Empty;
                    Property.SMTPHOST = string.Empty;
                    Property.SMTPPORT = 0;
                    Property.SMTPENABLESSL = false;
                    Property.TOEMAIL = string.Empty;
                    Property.CCEMAIL = string.Empty;
                    Property.BCCEMAIL = string.Empty;
                    Property.SUBJECT = string.Empty;
                    Property.HTMLBODY = string.Empty;
                    Property.ISACTIVE = false;
                }
                else
                {
                    Property.EMAIL_ID = Val.ToInt(DRow["EMAIL_ID"]); ;
                    Property.EMAILTYPE = Val.ToString(DRow["EMAILTYPE"]); ;
                    Property.SMTPEMAILUSERNAME = Val.ToString(DRow["SMTPEMAILUSERNAME"]); ;
                    Property.SMTPEMAILPASSWORD = Val.ToString(DRow["SMTPEMAILPASSWORD"]); ;
                    Property.SMTPDISPLAYNAME = Val.ToString(DRow["SMTPDISPLAYNAME"]); ;
                    Property.SMTPHOST = Val.ToString(DRow["SMTPHOST"]); ;
                    Property.SMTPPORT = Val.ToInt(DRow["SMTPPORT"]); ;
                    Property.SMTPENABLESSL = Val.ToBoolean(DRow["SMTPENABLESSL"]); ;
                    Property.TOEMAIL = Val.ToString(DRow["TOEMAIL"]); ;
                    Property.CCEMAIL = Val.ToString(DRow["CCEMAIL"]); ;
                    Property.BCCEMAIL = Val.ToString(DRow["BCCEMAIL"]); ;
                    Property.SUBJECT = Val.ToString(DRow["SUBJECT"]); ;
                    Property.HTMLBODY = Val.ToString(DRow["HTMLBODY"]); ;
                    Property.ISACTIVE = Val.ToBoolean(DRow["ISACTIVE"]); ;

                }

                Ope = null;
            }
            catch (Exception EX)
            {
                MessageBox.Show(EX.Message);
            }
            return Property;

        }


    }
}
