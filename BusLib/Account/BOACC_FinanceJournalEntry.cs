using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Diagnostics;
using AxonDataLib;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using BusLib.TableName;
using System.Data.SqlClient;

namespace BusLib.Account
{
    public class BOACC_FinanceJournalEntry
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable FindLEdgerIdNameFrmType(string pStrColumnType, string pStrAccountType)
        {
            Ope.ClearParams();

            DataTable DTabBillDetail = new DataTable();

            Ope.AddParams("ACCOUNTTYPE", pStrAccountType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLUMNSIDE", pStrColumnType, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTabBillDetail, "Acc_FindLEdgerIdNameFrmType", CommandType.StoredProcedure);

            return DTabBillDetail;

        }



        public TRN_LedgerTranJournalProperty SaveAccountingEffect(SqlConnection pConConnection, TRN_LedgerTranJournalProperty pProperty, string pStrXML)
        {
            //try
            //{
            Ope.ClearParams();

            DataTable DTabBillDetail = new DataTable();

            Ope.AddParams("TRN_ID", pProperty.TRN_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ACCLEDGTRNTRN_ID", pProperty.ACCLEDGTRNTRN_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ACCLEDGTRNSRNO", pProperty.ACCLEDGTRNSRNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ENTRYTYPE", pProperty.ENTRYTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BOOKTYPE", pProperty.BOOKTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BOOKTYPEFULL", pProperty.BOOKTYPEFULL, DbType.String, ParameterDirection.Input);
            Ope.AddParams("VOUCHERDATE", pProperty.VOUCHERDATE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);
            Ope.AddParams("VOUCHERNO", pProperty.VOUCHERNO, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("VOUCHERSTR", pProperty.VOUCHERSTR, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CURRENCY_ID", pProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("EXCRATE", pProperty.EXCRATE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("NOTE", pProperty.NOTE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TRNTYPE", pProperty.TRNTYPE, DbType.String, ParameterDirection.Input);
            //kuldeep 24122020
            Ope.AddParams("REFACCLEDGTRNTRN_ID", pProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("REFTRN_ID", pProperty.REFTRN_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("REFSRNO", pProperty.REFSRNO, DbType.Int32, ParameterDirection.Input);
            //Kuldeep 24122020
            Ope.AddParams("REFBOOKTYPEFULL", pProperty.REFBOOKTYPEFULL, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILL_NO", pProperty.BILL_NO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILL_DT", pProperty.BILL_DT, DbType.String, ParameterDirection.Input);
            Ope.AddParams("EXCRATEDIFF", pProperty.EXCRATEDIFF, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TERMSDATE", pProperty.TERMSDATE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CHQ_NO", pProperty.CHQ_NO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CHQISSUEDT", pProperty.CHQISSUEDT, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CHQCLEARDT", pProperty.CHQCLEARDT, DbType.String, ParameterDirection.Input);
            Ope.AddParams("DATAFREEZ", pProperty.DATAFREEZ, DbType.Boolean, ParameterDirection.Input);
            Ope.AddParams("PAYTYPE", pProperty.PAYTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("REFTYPE", pProperty.REFTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PAYTERMS", pProperty.PAYTERMS, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("BILLTYPE", pProperty.BIll_TYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ACCTYPE", pProperty.ACCTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("XMLDETSTR", pStrXML, DbType.Xml, ParameterDirection.Input);
            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);

            Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

            ArrayList AL = Ope.ExeNonQueryWithOutParameter(pConConnection, Config.ProviderName, "Trn_AccountJournalEntrySave", CommandType.StoredProcedure);
            if (AL.Count != 0)
            {
                pProperty.ReturnValue = Val.ToString(AL[0]);
                pProperty.ReturnValueJanged = Val.ToString(AL[1]);
                pProperty.ReturnMessageType = Val.ToString(AL[2]);
                pProperty.ReturnMessageDesc = Val.ToString(AL[3]);
            }
            else
            {
                pProperty.ReturnValue = "";
                pProperty.ReturnValueJanged = "";
                pProperty.ReturnMessageType = "FAIL";
                pProperty.ReturnMessageDesc = "";

            }
            //}
            //catch (System.Exception ex)
            //{
            //    pProperty.ReturnValue = "";
            //    pProperty.ReturnValueJanged = "";
            //    pProperty.ReturnMessageType = "FAIL";
            //    pProperty.ReturnMessageDesc = ex.Message;

            //}

            return pProperty;

        }

        public TRN_LedgerTranJournalProperty SaveAccountingEffectHK(SqlConnection pConConnection, TRN_LedgerTranJournalProperty pProperty, string pStrXML)
        {
            //try
            //{
            Ope.ClearParams();

            DataTable DTabBillDetail = new DataTable();

            Ope.AddParams("TRN_ID", pProperty.TRN_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ACCLEDGTRNTRN_ID", pProperty.ACCLEDGTRNTRN_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ACCLEDGTRNSRNO", pProperty.ACCLEDGTRNSRNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ENTRYTYPE", pProperty.ENTRYTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BOOKTYPE", pProperty.BOOKTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BOOKTYPEFULL", pProperty.BOOKTYPEFULL, DbType.String, ParameterDirection.Input);
            Ope.AddParams("VOUCHERDATE", pProperty.VOUCHERDATE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINYEAR_ID", pProperty.FINYEAR_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINYEAR", pProperty.FINYEAR, DbType.String, ParameterDirection.Input);
            Ope.AddParams("VOUCHERNO", pProperty.VOUCHERNO, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("VOUCHERSTR", pProperty.VOUCHERSTR, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CURRENCY_ID", pProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("EXCRATE", pProperty.EXCRATE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("NOTE", pProperty.NOTE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TRNTYPE", pProperty.TRNTYPE, DbType.String, ParameterDirection.Input);
            //kuldeep 24122020
            Ope.AddParams("REFACCLEDGTRNTRN_ID", pProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("REFTRN_ID", pProperty.REFTRN_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("REFSRNO", pProperty.REFSRNO, DbType.Int32, ParameterDirection.Input);
            //Kuldeep 24122020
            Ope.AddParams("REFBOOKTYPEFULL", pProperty.REFBOOKTYPEFULL, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILL_NO", pProperty.BILL_NO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILL_DT", pProperty.BILL_DT, DbType.String, ParameterDirection.Input);
            Ope.AddParams("EXCRATEDIFF", pProperty.EXCRATEDIFF, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TERMSDATE", pProperty.TERMSDATE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CHQ_NO", pProperty.CHQ_NO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CHQISSUEDT", pProperty.CHQISSUEDT, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CHQCLEARDT", pProperty.CHQCLEARDT, DbType.String, ParameterDirection.Input);
            Ope.AddParams("DATAFREEZ", pProperty.DATAFREEZ, DbType.Boolean, ParameterDirection.Input);
            Ope.AddParams("PAYTYPE", pProperty.PAYTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("REFTYPE", pProperty.REFTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PAYTERMS", pProperty.PAYTERMS, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("BILLTYPE", pProperty.BIll_TYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ACCTYPE", pProperty.ACCTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("XMLDETSTR", pStrXML, DbType.Xml, ParameterDirection.Input);
            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);

            Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

            ArrayList AL = Ope.ExeNonQueryWithOutParameter(pConConnection, Config.ProviderName, "Trn_AccountJournalEntrySave", CommandType.StoredProcedure);
            if (AL.Count != 0)
            {
                pProperty.ReturnValue = Val.ToString(AL[0]);
                pProperty.ReturnValueJanged = Val.ToString(AL[1]);
                pProperty.ReturnMessageType = Val.ToString(AL[2]);
                pProperty.ReturnMessageDesc = Val.ToString(AL[3]);
            }
            else
            {
                pProperty.ReturnValue = "";
                pProperty.ReturnValueJanged = "";
                pProperty.ReturnMessageType = "FAIL";
                pProperty.ReturnMessageDesc = "";

            }
            //}
            //catch (System.Exception ex)
            //{
            //    pProperty.ReturnValue = "";
            //    pProperty.ReturnValueJanged = "";
            //    pProperty.ReturnMessageType = "FAIL";
            //    pProperty.ReturnMessageDesc = ex.Message;

            //}

            return pProperty;

        }

        public TRN_LedgerTranJournalProperty SaveAccountingEffectFinance(TRN_LedgerTranJournalProperty pProperty, string pStrXML, string pStrXMLSumry, string pStrXMLMeditor)
        {
            try
            {
                Ope.ClearParams();

                DataTable DTabBillDetail = new DataTable();

                Ope.AddParams("TRN_ID", pProperty.TRN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ACCLEDGTRNTRN_ID", pProperty.ACCLEDGTRNTRN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ACCLEDGTRNSRNO", pProperty.ACCLEDGTRNSRNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.String, ParameterDirection.Input); //Added by Daksha on 19/01/2023
                Ope.AddParams("ENTRYTYPE", pProperty.ENTRYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BOOKTYPE", pProperty.BOOKTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BOOKTYPEFULL", pProperty.BOOKTYPEFULL, DbType.String, ParameterDirection.Input);
                Ope.AddParams("VOUCHERDATE", pProperty.VOUCHERDATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("VOUCHERNO", pProperty.VOUCHERNO, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("VOUCHERSTR", pProperty.VOUCHERSTR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CURRENCY_ID", pProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EXCRATE", pProperty.EXCRATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TRNTYPE", pProperty.TRNTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("NOTE", pProperty.NOTE, DbType.String, ParameterDirection.Input);
                //kuldeep 24122020
                Ope.AddParams("REFACCLEDGTRNTRN_ID", pProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("REFTRN_ID", pProperty.REFTRN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("REFSRNO", pProperty.REFSRNO, DbType.Int32, ParameterDirection.Input);
                //Kuldeep 24122020
                Ope.AddParams("REFBOOKTYPEFULL", pProperty.REFBOOKTYPEFULL, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILL_NO", pProperty.BILL_NO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILL_DT", pProperty.BILL_DT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EXCRATEDIFF", pProperty.EXCRATEDIFF, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TERMSDATE", pProperty.TERMSDATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CHQ_NO", pProperty.CHQ_NO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CHQISSUEDT", pProperty.CHQISSUEDT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CHQCLEARDT", pProperty.CHQCLEARDT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("DATAFREEZ", pProperty.DATAFREEZ, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("PAYTYPE", pProperty.PAYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("REFTYPE", pProperty.REFTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PAYTERMS", pProperty.PAYTERMS, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("CRDAMOUNTFE", pProperty.CRDAMOUNTFE, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("CRDAMOUNT", pProperty.CRDAMOUNT, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("DEBAMOUNT", pProperty.DEBAMOUNT, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("DEBAMOUNTFE", pProperty.DEBAMOUNTFE, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("FROMLEDGER_ID", pProperty.FROMLEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("REFLEDGER_ID", pProperty.REFLEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SRNO", pProperty.SRNO, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", pStrXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTRSUMRY", pStrXMLSumry, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("XMLMEDITOR", pStrXMLMeditor, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CONVERTTOINR", pProperty.CONVERTTOINR, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("Mode", pProperty.Mode, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = new ArrayList();

                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_AccountCashBankEntryFinanceSave", CommandType.StoredProcedure);


                if (AL.Count != 0)
                {
                    pProperty.ReturnValue = Val.ToString(AL[0]);
                    pProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (System.Exception ex)
            {
                pProperty.ReturnValue = "";
                pProperty.ReturnValueJanged = "";
                pProperty.ReturnMessageType = "FAIL";
                pProperty.ReturnMessageDesc = ex.Message;

            }

            return pProperty;

        }

        public TRN_LedgerTranJournalProperty SaveAccounting(TRN_LedgerTranJournalProperty pProperty, string pStrXML, string pStrXMLSumry, string pStrXMLMeditor)
        {
            try
            {
                Ope.ClearParams();

                DataTable DTabBillDetail = new DataTable();

                Ope.AddParams("TRN_ID", pProperty.TRN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ACCLEDGTRNTRN_ID", pProperty.ACCLEDGTRNTRN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ACCLEDGTRNSRNO", pProperty.ACCLEDGTRNSRNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.String, ParameterDirection.Input); //Added by Daksha on 19/01/2023
                Ope.AddParams("ENTRYTYPE", pProperty.ENTRYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BOOKTYPE", pProperty.BOOKTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BOOKTYPEFULL", pProperty.BOOKTYPEFULL, DbType.String, ParameterDirection.Input);
                Ope.AddParams("VOUCHERDATE", pProperty.VOUCHERDATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("VOUCHERNO", pProperty.VOUCHERNO, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("VOUCHERSTR", pProperty.VOUCHERSTR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CURRENCY_ID", pProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EXCRATE", pProperty.EXCRATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TRNTYPE", pProperty.TRNTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("NOTE", pProperty.NOTE, DbType.String, ParameterDirection.Input);
                //kuldeep 24122020
                Ope.AddParams("REFACCLEDGTRNTRN_ID", pProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("REFTRN_ID", pProperty.REFTRN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("REFSRNO", pProperty.REFSRNO, DbType.Int32, ParameterDirection.Input);
                //Kuldeep 24122020
                Ope.AddParams("REFBOOKTYPEFULL", pProperty.REFBOOKTYPEFULL, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILL_NO", pProperty.BILL_NO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILL_DT", pProperty.BILL_DT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EXCRATEDIFF", pProperty.EXCRATEDIFF, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TERMSDATE", pProperty.TERMSDATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CHQ_NO", pProperty.CHQ_NO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CHQISSUEDT", pProperty.CHQISSUEDT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CHQCLEARDT", pProperty.CHQCLEARDT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("DATAFREEZ", pProperty.DATAFREEZ, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("PAYTYPE", pProperty.PAYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("REFTYPE", pProperty.REFTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PAYTERMS", pProperty.PAYTERMS, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("CRDAMOUNTFE", pProperty.CRDAMOUNTFE, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("CRDAMOUNT", pProperty.CRDAMOUNT, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("DEBAMOUNT", pProperty.DEBAMOUNT, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("DEBAMOUNTFE", pProperty.DEBAMOUNTFE, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("FROMLEDGER_ID", pProperty.FROMLEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("REFLEDGER_ID", pProperty.REFLEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SRNO", pProperty.SRNO, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", pStrXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTRSUMRY", pStrXMLSumry, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("XMLMEDITOR", pStrXMLMeditor, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CONVERTTOINR", pProperty.CONVERTTOINR, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("Mode", pProperty.Mode, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BROKER_ID", pProperty.BROKER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("BROKERAMT", pProperty.BROKERAMT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ACCTYPE", pProperty.ACCTYPE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = new ArrayList();

                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_AccountCashBankEntryFinanceExportSave", CommandType.StoredProcedure);

                if (AL.Count != 0)
                {
                    pProperty.ReturnValue = Val.ToString(AL[0]);
                    pProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (System.Exception ex)
            {
                pProperty.ReturnValue = "";
                pProperty.ReturnValueJanged = "";
                pProperty.ReturnMessageType = "FAIL";
                pProperty.ReturnMessageDesc = ex.Message;

            }

            return pProperty;

        }

        public TRN_LedgerTranJournalProperty SaveAccountingEffectExpence(TRN_LedgerTranJournalProperty pProperty, string pStrXML)
        {
            try
            {
                Ope.ClearParams();

                DataTable DTabBillDetail = new DataTable();

                Ope.AddParams("TRN_ID", pProperty.TRN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ACCLEDGTRNTRN_ID", pProperty.ACCLEDGTRNTRN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ACCLEDGTRNSRNO", pProperty.ACCLEDGTRNSRNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYTYPE", pProperty.ENTRYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BOOKTYPE", pProperty.BOOKTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BOOKTYPEFULL", pProperty.BOOKTYPEFULL, DbType.String, ParameterDirection.Input);
                Ope.AddParams("VOUCHERDATE", pProperty.VOUCHERDATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("VOUCHERNO", pProperty.VOUCHERNO, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("VOUCHERSTR", pProperty.VOUCHERSTR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CURRENCY_ID", pProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EXCRATE", pProperty.EXCRATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("NOTE", pProperty.NOTE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TRNTYPE", pProperty.TRNTYPE, DbType.String, ParameterDirection.Input);
                //kuldeep 24122020
                Ope.AddParams("REFACCLEDGTRNTRN_ID", pProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("REFTRN_ID", pProperty.REFTRN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("REFSRNO", pProperty.REFSRNO, DbType.Int32, ParameterDirection.Input);
                //Kuldeep 24122020
                Ope.AddParams("REFBOOKTYPEFULL", pProperty.REFBOOKTYPEFULL, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILL_NO", pProperty.BILL_NO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILL_DT", pProperty.BILL_DT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EXCRATEDIFF", pProperty.EXCRATEDIFF, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TERMSDATE", pProperty.TERMSDATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CHQ_NO", pProperty.CHQ_NO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CHQISSUEDT", pProperty.CHQISSUEDT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CHQCLEARDT", pProperty.CHQCLEARDT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("DATAFREEZ", pProperty.DATAFREEZ, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("PAYTYPE", pProperty.PAYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("REFTYPE", pProperty.REFTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PAYTERMS", pProperty.PAYTERMS, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("CRDAMOUNTFE", pProperty.CRDAMOUNTFE, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("CRDAMOUNT", pProperty.CRDAMOUNT, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("DEBAMOUNT", pProperty.DEBAMOUNT, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("DEBAMOUNTFE", pProperty.DEBAMOUNTFE, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("FROMLEDGER_ID", pProperty.FROMLEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("REFLEDGER_ID", pProperty.REFLEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SRNO", pProperty.SRNO, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", pStrXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_AccountJournalEntryExpenceSave", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pProperty.ReturnValue = Val.ToString(AL[0]);
                    pProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (System.Exception ex)
            {
                pProperty.ReturnValue = "";
                pProperty.ReturnValueJanged = "";
                pProperty.ReturnMessageType = "FAIL";
                pProperty.ReturnMessageDesc = ex.Message;

            }

            return pProperty;

        }

        public TRN_LedgerTranJournalProperty SaveAccountingEffectContra(TRN_LedgerTranJournalProperty pProperty, string pStrXML)
        {
            try
            {
                Ope.ClearParams();

                DataTable DTabBillDetail = new DataTable();

                Ope.AddParams("TRN_ID", pProperty.TRN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ACCLEDGTRNTRN_ID", pProperty.ACCLEDGTRNTRN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ACCLEDGTRNSRNO", pProperty.ACCLEDGTRNSRNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYTYPE", pProperty.ENTRYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BOOKTYPE", pProperty.BOOKTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BOOKTYPEFULL", pProperty.BOOKTYPEFULL, DbType.String, ParameterDirection.Input);
                Ope.AddParams("VOUCHERDATE", pProperty.VOUCHERDATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("VOUCHERNO", pProperty.VOUCHERNO, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("VOUCHERSTR", pProperty.VOUCHERSTR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CURRENCY_ID", pProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EXCRATE", pProperty.EXCRATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("NOTE", pProperty.NOTE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TRNTYPE", pProperty.TRNTYPE, DbType.String, ParameterDirection.Input);
                //kuldeep 24122020
                Ope.AddParams("REFACCLEDGTRNTRN_ID", pProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("REFTRN_ID", pProperty.REFTRN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("REFSRNO", pProperty.REFSRNO, DbType.Int32, ParameterDirection.Input);
                //Kuldeep 24122020
                Ope.AddParams("REFBOOKTYPEFULL", pProperty.REFBOOKTYPEFULL, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILL_NO", pProperty.BILL_NO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILL_DT", pProperty.BILL_DT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EXCRATEDIFF", pProperty.EXCRATEDIFF, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TERMSDATE", pProperty.TERMSDATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CHQ_NO", pProperty.CHQ_NO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CHQISSUEDT", pProperty.CHQISSUEDT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CHQCLEARDT", pProperty.CHQCLEARDT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("DATAFREEZ", pProperty.DATAFREEZ, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("PAYTYPE", pProperty.PAYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("REFTYPE", pProperty.REFTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PAYTERMS", pProperty.PAYTERMS, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("CRDAMOUNTFE", pProperty.CRDAMOUNTFE, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("CRDAMOUNT", pProperty.CRDAMOUNT, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("DEBAMOUNT", pProperty.DEBAMOUNT, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("DEBAMOUNTFE", pProperty.DEBAMOUNTFE, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("FROMLEDGER_ID", pProperty.FROMLEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("REFLEDGER_ID", pProperty.REFLEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SRNO", pProperty.SRNO, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", pStrXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_AccountJournalEntryContraSave", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pProperty.ReturnValue = Val.ToString(AL[0]);
                    pProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (System.Exception ex)
            {
                pProperty.ReturnValue = "";
                pProperty.ReturnValueJanged = "";
                pProperty.ReturnMessageType = "FAIL";
                pProperty.ReturnMessageDesc = ex.Message;

            }

            return pProperty;

        }

        public TRN_LedgerTranJournalProperty SaveAccountingJournalEffectFinance(TRN_LedgerTranJournalProperty pProperty, string pStrXML, string pStrXMLSumry, string pStrXMLMeditor)
        {
            try
            {
                Ope.ClearParams();

                DataTable DTabBillDetail = new DataTable();

                Ope.AddParams("TRN_ID", pProperty.TRN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ACCLEDGTRNTRN_ID", pProperty.ACCLEDGTRNTRN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ACCLEDGTRNSRNO", pProperty.ACCLEDGTRNSRNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYTYPE", pProperty.ENTRYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BOOKTYPE", pProperty.BOOKTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BOOKTYPEFULL", pProperty.BOOKTYPEFULL, DbType.String, ParameterDirection.Input);
                Ope.AddParams("VOUCHERDATE", pProperty.VOUCHERDATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("VOUCHERNO", pProperty.VOUCHERNO, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("VOUCHERSTR", pProperty.VOUCHERSTR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CURRENCY_ID", pProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EXCRATE", pProperty.EXCRATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("NOTE", pProperty.NOTE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TRNTYPE", pProperty.TRNTYPE, DbType.String, ParameterDirection.Input);
                //kuldeep 24122020
                Ope.AddParams("REFACCLEDGTRNTRN_ID", pProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("REFTRN_ID", pProperty.REFTRN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("REFSRNO", pProperty.REFSRNO, DbType.Int32, ParameterDirection.Input);
                //Kuldeep 24122020
                Ope.AddParams("REFBOOKTYPEFULL", pProperty.REFBOOKTYPEFULL, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILL_NO", pProperty.BILL_NO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILL_DT", pProperty.BILL_DT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EXCRATEDIFF", pProperty.EXCRATEDIFF, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TERMSDATE", pProperty.TERMSDATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CHQ_NO", pProperty.CHQ_NO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CHQISSUEDT", pProperty.CHQISSUEDT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CHQCLEARDT", pProperty.CHQCLEARDT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("DATAFREEZ", pProperty.DATAFREEZ, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("PAYTYPE", pProperty.PAYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("REFTYPE", pProperty.REFTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PAYTERMS", pProperty.PAYTERMS, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("CRDAMOUNTFE", pProperty.CRDAMOUNTFE, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("CRDAMOUNT", pProperty.CRDAMOUNT, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("DEBAMOUNT", pProperty.DEBAMOUNT, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("DEBAMOUNTFE", pProperty.DEBAMOUNTFE, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("FROMLEDGER_ID", pProperty.FROMLEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("REFLEDGER_ID", pProperty.REFLEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SRNO", pProperty.SRNO, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", pStrXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTRSUMRY", pStrXMLSumry, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("XMLMEDITOR", pStrXMLMeditor, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COMAPNY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_AccountJournalEntryFinanceSave", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pProperty.ReturnValue = Val.ToString(AL[0]);
                    pProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (System.Exception ex)
            {
                pProperty.ReturnValue = "";
                pProperty.ReturnValueJanged = "";
                pProperty.ReturnMessageType = "FAIL";
                pProperty.ReturnMessageDesc = ex.Message;

            }

            return pProperty;

        }

        public TRN_LedgerTranJournalProperty SaveAccountingEffectOpening(TRN_LedgerTranJournalProperty pProperty, string pStrXML)
        {
            try
            {
                Ope.ClearParams();

                DataTable DTabBillDetail = new DataTable();

                Ope.AddParams("TRN_ID", pProperty.TRN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ACCLEDGTRNTRN_ID", pProperty.ACCLEDGTRNTRN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ACCLEDGTRNSRNO", pProperty.ACCLEDGTRNSRNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYTYPE", pProperty.ENTRYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BOOKTYPE", pProperty.BOOKTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BOOKTYPEFULL", pProperty.BOOKTYPEFULL, DbType.String, ParameterDirection.Input);
                Ope.AddParams("VOUCHERDATE", pProperty.VOUCHERDATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("VOUCHERNO", pProperty.VOUCHERNO, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("VOUCHERSTR", pProperty.VOUCHERSTR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CURRENCY_ID", pProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EXCRATE", pProperty.EXCRATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("NOTE", pProperty.NOTE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TRNTYPE", pProperty.TRNTYPE, DbType.String, ParameterDirection.Input);
                //kuldeep 24122020
                Ope.AddParams("TERMSDATE", pProperty.TERMSDATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("DATAFREEZ", pProperty.DATAFREEZ, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("PAYTYPE", pProperty.PAYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("REFTYPE", pProperty.REFTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PAYTERMS", pProperty.PAYTERMS, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("XMLDETSTR", pStrXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_AccountJournalEntryOpeningSave", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pProperty.ReturnValue = Val.ToString(AL[0]);
                    pProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (System.Exception ex)
            {
                pProperty.ReturnValue = "";
                pProperty.ReturnValueJanged = "";
                pProperty.ReturnMessageType = "FAIL";
                pProperty.ReturnMessageDesc = ex.Message;

            }

            return pProperty;

        }

        public TRN_LedgerTranJournalProperty UpdateChqCLearDate(TRN_LedgerTranJournalProperty pProperty, string pStrXML)
        {
            try
            {
                Ope.ClearParams();

                DataTable DTabBillDetail = new DataTable();

                Ope.AddParams("XMLDETSTR", pStrXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "ACC_UpdateChqCLearDate", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pProperty.ReturnValue = Val.ToString(AL[0]);
                    pProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (System.Exception ex)
            {
                pProperty.ReturnValue = "";
                pProperty.ReturnValueJanged = "";
                pProperty.ReturnMessageType = "FAIL";
                pProperty.ReturnMessageDesc = ex.Message;

            }

            return pProperty;

        }

        public DataTable CheckBillAllocatedOrNot(string pStrTrn_id, string pStrAccLedgTrnTRN_ID)
        {
            Ope.ClearParams();

            DataTable DTabBillDetail = new DataTable();

            Ope.AddParams("TRN_ID", pStrTrn_id, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ACCLEDGTRNTRN_ID", pStrAccLedgTrnTRN_ID, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTabBillDetail, "ACC_CheckBillAllocatedOrNot", CommandType.StoredProcedure);
            return DTabBillDetail;

        }


        public DataTable GetMemoIdFromFinanceID(string pStrTRN_ID)
        {
            Ope.ClearParams();

            DataTable DSet = new DataTable();

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DSet, "SELECT ACCLEDGTRNTRN_ID from ACC_LedgerTranJournal where TRN_ID = '" + pStrTRN_ID + "'  ", CommandType.Text);
            return DSet;

        }
        public DataTable GetLedger_IdFromCompanyName(string pStrLedgerName)
        {
            Ope.ClearParams();

            DataTable DSet = new DataTable();

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DSet, "SELECT LEDGER_ID from MST_LEDGER where (COMPANYNAME = '" + pStrLedgerName + "' or ledgername = '" + pStrLedgerName + "')  ", CommandType.Text);
            return DSet;

        }

        //public DataTable FindLCEntryBillNoWise(string BILLNO)
        //{
        //    Ope.ClearParams();
        //    DataTable DTabBillDetail = new DataTable();
        //    Ope.AddParams("BILLNO", BILLNO, DbType.String, ParameterDirection.Input);

        //    if (Config.gStrLoginSection == "B")
        //    {
        //        Ope.FillDTab(Config.ConnectionStringB, Config.ProviderName, DTabBillDetail, "Acc_FindLCEntryBillNoWise", CommandType.StoredProcedure);
        //    }
        //    else
        //    {
        //        Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTabBillDetail, "Acc_FindLCEntryBillNoWise", CommandType.StoredProcedure);
        //    }
        //    return DTabBillDetail;
        //}


        public DataTable LcEntry(string pStrFromDate, string pStrToDate, string pStrParty, string VoucherNo, string EntryType)
        {
            Ope.ClearParams();

            DataTable Dtab = new DataTable();
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("PARTY_ID", pStrParty, DbType.String, ParameterDirection.Input);
            Ope.AddParams("VOUCHERNOSTR", VoucherNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ENTRYTYPE", EntryType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, Dtab, "Acc_LCEntryList" + "", CommandType.StoredProcedure);
            return Dtab;
        }


        public DataSet GetAccLedgerPartyWiseData(Guid pStrLedgerID, string pStrType)
        {
            Ope.ClearParams();
            DataSet DSet = new DataSet();
            Ope.AddParams("LEDGER_ID", pStrLedgerID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("TYPE", pStrType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.Int64, ParameterDirection.Input);


            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DSet, "TEMP1", "ACC_LedgerPartyWiseData", CommandType.StoredProcedure);
            return DSet;
        }

        public DataSet GetAccLedgerPartyInvoiceData(Int32 pStrVoucherNo, string pStrType, Guid pStrLedgerID)
        {
            Ope.ClearParams();
            DataSet DSet = new DataSet();
            Ope.AddParams("VoucherNo", pStrVoucherNo, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("LEDGER_ID", pStrLedgerID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("TYPE", pStrType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.Int64, ParameterDirection.Input);


            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DSet, "TEMP1", "ACC_LedgerPartyInvoiceWiseData", CommandType.StoredProcedure);
            return DSet;
        }

        public ACC_ConversionEntryProperty SaveConversionEntry(Guid NewTrn_Id, ACC_ConversionEntryProperty pProperty)
        {
            try
            {
                Ope.ClearParams();
                DataTable DTabBillDetail = new DataTable();
                Ope.AddParams("NEWTRN_ID", NewTrn_Id, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("TRN_ID", pProperty.TRN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("VOUCHERNO", pProperty.VoucherNo, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("VoucherDate", pProperty.VoucherDate, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BillDate", pProperty.BillDate, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILL_NO", pProperty.BillNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PartyId", pProperty.PartyId, DbType.Guid, ParameterDirection.Input);
                //Ope.AddParams("PartyName", pProperty.PartyName, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SaleAmount", pProperty.SaleAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ExcRate", pProperty.ExcRate, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("AmtRs", pProperty.AmtRs, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("Company_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("FinYear_ID", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("Remarks", pProperty.Remarks, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TYPE", pProperty.TYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PAYTERMS", pProperty.PAYTERMS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("DEFAULTACC", pProperty.DEFAULTACCLEDGER, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_AccountConversionEntrySave", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pProperty.ReturnValue = Val.ToString(AL[0]);
                    pProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pProperty.ReturnValue = "";
                pProperty.ReturnMessageType = "FAIL";
                pProperty.ReturnMessageDesc = ex.Message;
            }
            return pProperty;
        }

        public DataTable ConversionEntrySKE_GetData(string pStrFromDate, string pStrToDate, Guid pStrParty, string VoucherNo)
        {
            Ope.ClearParams();

            DataTable Dtab = new DataTable();
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("PARTY_ID", pStrParty, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("VOUCHERNOSTR", VoucherNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, Dtab, "Trn_BankConversionRate_GetData", CommandType.StoredProcedure);
            return Dtab;
        }

        public DataTable ConversionEntrySKE_GetData_ByTrnId(Guid pTrn_Id)
        {
            Ope.ClearParams();

            DataTable Dtab = new DataTable();
            Ope.AddParams("TRN_ID", pTrn_Id, DbType.Guid, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, Dtab, "ConversionEntrySKE_GetData_ByTrnId", CommandType.StoredProcedure);
            return Dtab;
        }

        public DataTable ConversionEntrySKE_GetLedgerPartyWiseData(int Process_Id, string InvType, Guid pStrLedgerID, string AccType)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("PROCESS_ID", Process_Id, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("INVTYPE", InvType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LEDGER_ID", pStrLedgerID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ACCTYPE", AccType, DbType.String, ParameterDirection.Input);//Gunjan:29/05/2023
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "ConversionEntrySKE_GetLedgerPartyWiseData", CommandType.StoredProcedure);
            return DTab;
        }



        public DataTable ImportExportReport_GetData(string pStrFromDate, string pStrToDate, string pStrAccType)
        {
            Ope.ClearParams();

            DataTable Dtab = new DataTable();
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("ACCTYPE", pStrAccType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, Dtab, "Trn_BankConversionRate_GetData", CommandType.StoredProcedure);
            return Dtab;
        }

        public Int64 ConversionEntrySKE_FindVoucherNoNew(string pStrBookType)
        {
            Int64 IntNewID = 0;
            string StrSql = " AND InvoiceBillType = '" + pStrBookType + "' AND FinYear_Id = '" + Config.FINYEAR_ID + "' ";
            Ope.ClearParams();

            IntNewID = Ope.FindNewID(Config.ConnectionString, Config.ProviderName, "TRN_BankConversionRate", "ISNULL(MAX(VOUCHERNO),0)", StrSql);

            return IntNewID;
        }

        public ACC_BankConversionRateProperty ConversionEntrySKE_InsertUpdate(ACC_BankConversionRateProperty pProperty)
        {
            try
            {
                Ope.ClearParams();
                DataTable DTabBillDetail = new DataTable();
                Ope.AddParams("TRN_ID", pProperty.Trn_Id, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("INVOICEMEMO_ID", pProperty.INVOICEMEMO_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("VOUCHERNO", pProperty.VoucherNo, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("VOUCHERNOSTR", pProperty.VoucherNoStr, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CONVERSIONDATE", pProperty.ConversionDate, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("INVOICEBILLTYPE", pProperty.InvoiceBillType, DbType.String, ParameterDirection.Input);
                Ope.AddParams("INVOICEPARTY_ID", pProperty.InvoiceParty_Id, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("INVOICENOSTR", pProperty.InvoiceNoStr, DbType.String, ParameterDirection.Input);
                Ope.AddParams("INVOICENO", pProperty.INVOICENO, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("INVOICEDATE", pProperty.InvoiceDate, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("INVOICECURRENCY_ID", pProperty.InvoiceCurrency_Id, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);

                Ope.AddParams("INVOICE_EXCRATE", pProperty.Invoice_ExcRate, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INVOICE_GROSSAMOUNT", pProperty.Invoice_GrossAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INVOICE_FGROSSAMOUNT", pProperty.Invoice_FGrossAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INVOICE_CGSTPER", pProperty.Invoice_CGSTPer, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INVOICE_CGSTAMOUNT", pProperty.Invoice_CGSTAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INVOICE_FCGSTAMOUNT", pProperty.Invoice_FCGSTAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INVOICE_SGSTPER", pProperty.Invoice_SGSTPer, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INVOICE_SGSTAMOUNT", pProperty.Invoice_SGSTAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INVOICE_FSGSTAMOUNT", pProperty.Invoice_FSGSTAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INVOICE_IGSTPER", pProperty.Invoice_IGSTPer, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INVOICE_IGSTAMOUNT", pProperty.Invoice_IGSTAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INVOICE_FIGSTAMOUNT", pProperty.Invoice_FIGSTAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INVOICE_NETAMOUNT", pProperty.Invoice_NetAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INVOICE_FNETAMOUNT", pProperty.Invoice_FNetAmount, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("BANK_ID", pProperty.Bank_Id, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("CONVRATE", pProperty.ConvRate, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GROSSAMOUNT", pProperty.GrossAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FGROSSAMOUNT", pProperty.FGrossAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TCSPER", pProperty.TCSPer, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TCSAMOUNT", pProperty.TCSAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FTCSAMOUNT", pProperty.FTCSAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FORECHARGEPER", pProperty.ForeChargePer, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FORECHARGEAMOUNT", pProperty.ForeChargeAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FFORECHARGEAMOUNT", pProperty.FForeChargeAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("NETAMOUNT", pProperty.NetAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FNETAMOUNT", pProperty.FNetAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EXCDIFFAMOUNT", pProperty.ExcDiffAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FEXCDIFFAMOUNT", pProperty.FExcDiffAmount, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("REMARK", pProperty.Remark, DbType.String, ParameterDirection.Input);

                Ope.AddParams("INVOICEACCTYPE", pProperty.AccType, DbType.String, ParameterDirection.Input);//GUNJAN:29/05/2023

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("EntryIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_BankConversionRate_InsertUpdate", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pProperty.ReturnValue = Val.ToString(AL[0]);
                    pProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pProperty.ReturnValue = "";
                pProperty.ReturnMessageType = "FAIL";
                pProperty.ReturnMessageDesc = ex.Message;
            }
            return pProperty;
        }

        public ACC_BankConversionRateProperty ConversionEntrySKE_Delete(ACC_BankConversionRateProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();

                Ope.AddParams("TRN_ID", pClsProperty.Trn_Id, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);


                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_BankConversionRate_Delete", CommandType.StoredProcedure);

                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);

                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }

            return pClsProperty;
        }




    }
}
