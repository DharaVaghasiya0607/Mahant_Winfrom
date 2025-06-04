using AxonDataLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using BusLib.TableName;
using System.Collections;
using System.Collections.Generic;

namespace BusLib.EInvoice
{

    public class TRN_EInvoiceProperty
    {
        public Guid COMPANY_ID { get; set; }
        public string CLIENTID { get; set; }
        public string CLIENTSECRET { get; set; }
        public string GSTIN { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string URL { get; set; }
        public string TOKENURL { get; set; }

    }

    
    public partial class Acct_EinvoiceApiRequest
    {
        private string _Data;

        public string Data
        {
            get
            {
                return _Data;
            }

            set
            {
                _Data = value;
            }
        }

        private string _rek;

        public string rek
        {
            get
            {
                return _rek;
            }

            set
            {
                _rek = value;
            }
        }

        private System.Collections.Generic.List<Acct_EinvoiceApiRequest> _Result;

        public System.Collections.Generic.List<Acct_EinvoiceApiRequest> Result
        {
            get
            {
                return _Result;
            }

            set
            {
                _Result = value;
            }
        }
    }

    public class BOTRN_EInvoice
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataSet GetEInvoiceInvoiceInfo(string pStrInvoiceID)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("Invoice_ID", pStrInvoiceID, DbType.Guid, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName,DS,"Temp", "MST_EInvoiceGetInvoiceInfo", CommandType.StoredProcedure);

            return DS;
        }
        
        public DataSet GetEInvoiceCancelInvoiceInfo(string pStrInvoiceID)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("Invoice_ID", pStrInvoiceID, DbType.Guid, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "MST_EInvoiceGetCancelInvoiceInfo", CommandType.StoredProcedure);

            return DS;
        }

        public TRN_EInvoiceProperty GetEInvoiceCredential(Guid CompanyID)
        {
            TRN_EInvoiceProperty Property = new TRN_EInvoiceProperty();
            Property.COMPANY_ID = Config.COMPANY_ID;
            Property.CLIENTID = string.Empty;
            Property.CLIENTSECRET = string.Empty;
            Property.GSTIN = string.Empty;
            Property.USERNAME = string.Empty;
            Property.PASSWORD = string.Empty;
            Property.URL = string.Empty;
            Property.TOKENURL = string.Empty;

            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("COMPANY_ID", CompanyID, DbType.Guid, ParameterDirection.Input);

            DataRow DRow =  Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, "MST_EInvoiceGetData", CommandType.StoredProcedure);
            if (DRow != null)
            {
                Property.COMPANY_ID = Guid.Parse(Val.ToString(DRow["COMPANY_ID"]));
                Property.CLIENTID = Val.ToString(DRow["CLIENTID"]);
                Property.CLIENTSECRET = Val.ToString(DRow["CLIENTSECRET"]);
                Property.GSTIN = Val.ToString(DRow["GSTIN"]);
                Property.USERNAME = Val.ToString(DRow["USERNAME"]);
                Property.PASSWORD = Val.ToString(DRow["PASSWORD"]);
                Property.URL = Val.ToString(DRow["URL"]);
                Property.TOKENURL = Val.ToString(DRow["TOKENURL"]);
            }
            return Property;
        }

        public int InsertEInvoiceDetail(string Memo_ID,string AckNo,string IrnNo,string IrnDate,string SignedInvoice,string SignedQRCode , string CancelDate,TRN_EInvoiceProperty pClsProperty)
        {
            Ope.ClearParams();
            Ope.AddParams("MEMO_ID", Memo_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ACKNO", AckNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("IRNNO", IrnNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ACKDATE", IrnDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("SIGNEDINVOICE", SignedInvoice, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SIGNEDQRCODE", SignedQRCode, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CLIENTID", pClsProperty.CLIENTID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CLIENTSECRET", pClsProperty.CLIENTSECRET, DbType.String, ParameterDirection.Input);
            Ope.AddParams("GSTIN", pClsProperty.GSTIN, DbType.String, ParameterDirection.Input);
            Ope.AddParams("USERNAME", pClsProperty.USERNAME, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PASSWORD", pClsProperty.PASSWORD, DbType.String, ParameterDirection.Input);
            Ope.AddParams("URL", pClsProperty.URL, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TOKENURL", pClsProperty.TOKENURL, DbType.String, ParameterDirection.Input);
		    Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CANCELDATE", CancelDate, DbType.Date, ParameterDirection.Input);

            return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "MST_EInvoiceSaveInvoiceInfo", CommandType.StoredProcedure);
            
        }

        public void InsertEInvoiceReustIdMessage(string StrReqId, string Msg, string Memo_id)
        {
            Ope.ClearParams();
            string Str = "Insert into Trn_EInvoiceRequestIdMessage(RequestNo , Message , Memo_id) values ('" + StrReqId + "' , '" + Msg + "'  ,'" + Memo_id + "' ) ";
            Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);

        }

        public void InsertEInvoicePrintOutPutXML(string Memo_id , string OutXml)
        {
            Ope.ClearParams();
            Ope.AddParams("MEMO_ID", Memo_id, DbType.String, ParameterDirection.Input);
            Ope.AddParams("OUTPUTXML", OutXml, DbType.String, ParameterDirection.Input);
            Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "Trn_EInvoicePrintSave" , CommandType.StoredProcedure);

        }

        public void UpdateEInvoiceUploadDate(string StrUploadDate,string Memo_id)
        {
            Ope.ClearParams();
            string Str = "update trn_memo set EInvoiceUploadDate = '" + StrUploadDate + "',IsEInvoiceDone = '" + 1 + "' where memo_id = '" + Memo_id + "'  ";
            Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);

        }
        public void UpdateEInvoiceCancelDate(string StrUploadDate, string Memo_id)
        {
            Ope.ClearParams();
            string Str = "update trn_memo set EInvoiceCancelDate = '" + StrUploadDate + "',IsEInvoiceDone = '" + 0 + "' where memo_id = '" + Memo_id + "'  ";
            Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);

        }
        public DataRow GetEInvoiceExists(string Memo_ID)
        {
            Ope.ClearParams();
            Ope.AddParams("MEMO_ID", Memo_ID, DbType.Guid, ParameterDirection.Input);
            return Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, "MST_EInvoiceCheckExists", CommandType.StoredProcedure);
        }

        public DataSet GetEInvoiceData(string Memo_ID)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("MEMO_ID", Memo_ID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "MST_EInvoiceCheckExists", CommandType.StoredProcedure);
            return DS;
        }

        public string GetMaxRequstId(string ReqName)
        {
            Ope.ClearParams();
            Ope.AddParams("ID_TYPE", ReqName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
            ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_Maximum_ID_GenerateDirectMax", CommandType.StoredProcedure);
            if (AL.Count != 0)
               return  Val.ToString(AL[0]);
            return "0";
        }

        public DataSet GetSaleBillBulkEInvoiceUpload(string pStrFromDate, string pStrToDate, string pStrBillingPartyID, string pStrStatus)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("BILLINGPARTY_ID", pStrBillingPartyID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STATUS", pStrStatus, DbType.String, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Trn_GetSaleBillBulkEInvoiceUpload", CommandType.StoredProcedure);

            return DS;
        }
        
    }
    public class Output
    {
        public string name { get; set; }
        public string[] results { get; set; }
    }
}
