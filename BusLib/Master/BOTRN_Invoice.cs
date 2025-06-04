using AxonDataLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using System.Collections;
using BusLib.TableName;

namespace BusLib.Master
{
    public class BOTRN_Invoice
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public TRN_InvoiceProperty Save(TRN_InvoiceProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("Invoice_ID", pClsProperty.Invoice_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("Company_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("InvoiceDate", pClsProperty.InvoiceDate, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("FinYear", pClsProperty.FinYear, DbType.String, ParameterDirection.Input);
                Ope.AddParams("InvoiceNo", pClsProperty.InvoiceNo, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("Party_ID", pClsProperty.Party_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PartyName", pClsProperty.PartyName, DbType.String, ParameterDirection.Input);
                Ope.AddParams("Seller_ID", pClsProperty.Seller_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("AuthorisePerson_ID", pClsProperty.AuthorisePerson_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("AuthorisePersonName", pClsProperty.AuthorisePersonName, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MemoType", pClsProperty.MemoType, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CurrencyType", pClsProperty.CurrencyType, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BankType", pClsProperty.BankType, DbType.String, ParameterDirection.Input);
                Ope.AddParams("Process_ID", pClsProperty.Process_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("Terms_ID", pClsProperty.Terms_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("PhNo", pClsProperty.PhNo, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ExcRate", pClsProperty.ExcRate, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("Remark", pClsProperty.Remark, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BrNo", pClsProperty.BrNo, DbType.String, ParameterDirection.Input);
                Ope.AddParams("Pickup", pClsProperty.Pickup, DbType.String, ParameterDirection.Input);
                Ope.AddParams("InvoiceType", pClsProperty.INVOICETYPE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("IS_PAYMENT", pClsProperty.IS_Payment, DbType.Boolean, ParameterDirection.Input);//Gunjan:23/05/2023

                Ope.AddParams("DETAILXML", pClsProperty.DetailXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnValueStr", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);


                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_InvoiceHKSave", CommandType.StoredProcedure);

                if (AL.Count != 0)
                {
                    pClsProperty.ReturnInvoiceNo = Val.ToString(AL[0]);
                    pClsProperty.ReturnInvoiceStr = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnInvoiceNo = "";
                pClsProperty.ReturnInvoiceStr = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;
        }


        public DataTable FillSummary(string pStrFromData, string pStrToDate, int pStrProcess_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("FROMDATE", pStrFromData, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("Company_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("Process_ID", pStrProcess_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_InvoiceHKGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable FillDetail(string pStrInvoiceID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("Invoice_ID", pStrInvoiceID, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_InvoiceHKGetDetail", CommandType.StoredProcedure);
            return DTab;
        }

        public TRN_InvoiceProperty Delete(TRN_InvoiceProperty pClsProperty)
        {
            try
            {

                Ope.ClearParams();
                Ope.AddParams("Invoice_ID", pClsProperty.Invoice_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("Company_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_InvoiceHKDelete", CommandType.StoredProcedure);

                if (AL.Count != 0)
                {
                    pClsProperty.ReturnInvoiceNo = Val.ToString(AL[0]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnInvoiceNo = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;
        }


        public DataTable Print(string pStrInvoiceID, string pStrOpe)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("Invoice_ID", pStrInvoiceID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Company_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_InvoiceHKGetPrint", CommandType.StoredProcedure);
            return DTab;
        }

        public TRN_InvoiceProperty UpdatePaymentDone(TRN_InvoiceProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("Invoice_ID", pClsProperty.Invoice_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_InvoiceHKPaymentDone", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnInvoiceNo = Val.ToString(AL[0]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnInvoiceNo = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = "FAIL";

            }
            return pClsProperty;
        }

        public DataTable FillSummaryExport(string pStrFromData, string pStrToDate, int pStrProcess_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("FROMDATE", pStrFromData, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("Company_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("Process_ID", pStrProcess_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_InvoiceHKGetDataExport", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable PrintExport(string pStrInvoiceID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("Invoice_ID", pStrInvoiceID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Company_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_InvoiceHKGetPrintExport", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable FillSummaryBYID(string pStrInvoiceID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("Invoice_ID", pStrInvoiceID, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_InvoiceHKGetSummaryData", CommandType.StoredProcedure);
            return DTab;
        }

        public TRN_InvoiceProperty UpdateIS_Payment(TRN_InvoiceProperty pClsProperty, string strInvoiceId)//Gunjan:23/05/2023
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("strInvoiceId", strInvoiceId, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_InvoiceHK_IsPaymentUpdateFlag", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "FAIL";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = "FAIL";

            }
            return pClsProperty;
        }

        public DataTable SaleDeliveryExportPrint(string pStrInvoiceID, string pStrDescription, string pStrHSNCode)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("Invoice_ID", pStrInvoiceID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Company_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("Descroption", pStrDescription, DbType.String, ParameterDirection.Input);
            Ope.AddParams("HSNCode", pStrHSNCode, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "RPT_SalesDeliveryExportHKPrint", CommandType.StoredProcedure);
            return DTab;

        }
    }

}
