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

namespace BusLib.Account
{
    public class BOLedgerTransaction
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public double FindLedgerClosing(Guid pIntLedgerID)
        {
            Ope.ClearParams();

            Ope.AddParams("Ledger_ID", pIntLedgerID, DbType.Guid, ParameterDirection.Input);

            DataRow DRow = Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, "Acc_FindLedgerClosing", CommandType.StoredProcedure);

            if (DRow == null)
            {
                return 0;
            }

            return Val.Val(DRow[0]);
        }


        public DataTable GetFromToDateYear()
        {
            Ope.ClearParams();

            DataTable DSet = new DataTable();

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DSet, "SELECT FROMDATE,TODATE from MST_YearMaster where year_id = '" + Config.FINYEAR_ID + "'  ", CommandType.Text);
            return DSet;

        }

        
        public DataTable GetFromToDateYearID(String Finyear = "")
        {
            Ope.ClearParams();

            DataTable DSet = new DataTable();

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DSet, "SELECT Year_ID from MST_YearMaster where YearShortName = '" + Finyear + "'  ", CommandType.Text);
            return DSet;

        }

        public DataSet GetBillWisePaymentGetDataNew(string pStrOpe, string pStrFromDate, string pStrToDate, string pStrBookType, Guid pGuidTrnId, Guid Party_ID,
          string VoucherNoStr = "", string PaymentType = "", int Currency_Id = -1)
        {
            Ope.ClearParams();

            DataSet DSet = new DataSet();
            Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);

            Ope.AddParams("BOOKTYPE", pStrBookType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TRNID", pGuidTrnId, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("PARTYID", Party_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("VOUCHERNOSTR", VoucherNoStr, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PAYMENTTYPE", PaymentType, DbType.String, ParameterDirection.Input); //Added by Daksha on 15/09/2023
            Ope.AddParams("CURRENCY_ID", Currency_Id, DbType.Int32, ParameterDirection.Input); //Added by Daksha on 15/09/2023           
            {
                Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DSet, "TEMP1", "Acc_BillWisePaymentGetDatanew", CommandType.StoredProcedure);
            }
            return DSet;

        }

        public DataTable FindLedgerClosingNew(Guid pIntLedgerID)
        {
            Ope.ClearParams();
            DataTable DTClose = new DataTable();
            Ope.AddParams("Ledger_ID", pIntLedgerID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTClose, "Acc_FindLedgerClosing_New", CommandType.StoredProcedure);
            return DTClose;
        }

        public Int64 FindVoucherNoNew(string pStrFinYearID, string pStrBookType)
        {
            Int64 IntNewID = 0;
            string StrSql = " AND BOOKTYPE = '" + pStrBookType + "' AND finyear_id = '" + Config.FINYEAR_ID + "' ";
            Ope.ClearParams();          
            {
                IntNewID = Ope.FindNewID(Config.ConnectionString, Config.ProviderName, "ACC_LedgerTranJournal", "ISNULL(MAX(VOUCHERNO),0)", StrSql);
            }
            return IntNewID;
        }
        public LedgerTransactionProperty DeleteNew(LedgerTransactionProperty pClsProperty)
        {
            Ope.ClearParams();

            Ope.AddParams("TRN_ID", pClsProperty.Trn_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

            Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

            ArrayList AL = new ArrayList();          
            {
                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "ACC_LedgerTranJournalDelete", CommandType.StoredProcedure);
            }
            if (AL.Count != 0)
            {
                pClsProperty.ReturnValueTrnID = Val.ToString(AL[0]);
                pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);

            }
            return pClsProperty;
        }

        public DataTable GetBillWiseOutstandingNew(Guid pGuidLedgerID, Guid pGuidTrnId, string pStrEntryDate, Int32 CURRENCY_ID, string pStrPaymentType = "", string pStrHKType = "")
        {
            Ope.ClearParams();

            DataTable DTabBillDetail = new DataTable();

            Ope.AddParams("LEDGER_ID", pGuidLedgerID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("TRN_ID", pGuidTrnId, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("PAYMENTTYPE", pStrPaymentType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ENTRYDATE", pStrEntryDate, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CURRENCY_ID", CURRENCY_ID, DbType.Int32, ParameterDirection.Input);           
            {               
                {
                    Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTabBillDetail, "Acc_LedgerPendingBillGetDatanew", CommandType.StoredProcedure);
                }
            }

            return DTabBillDetail;

        }
        public DataTable PaymetReceive_GetVoucherNoStr()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("YearId", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);
            {
                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "PaymentReceive_GetVoucherNoStr", CommandType.StoredProcedure);
            }
            return DTab;
        }

    }
}
