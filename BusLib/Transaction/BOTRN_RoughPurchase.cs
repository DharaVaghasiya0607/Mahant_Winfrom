using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AxonDataLib;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using BusLib.TableName;
using System.Collections;

namespace BusLib.Transaction
{
    public class BOTRN_RoughPurchase
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable GetDataForPurchaseLiveStock(bool pBoolDispAllLot, string pType)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("YearID", Config.FINYEAR_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ISDISPLAYALLLOT", pBoolDispAllLot, DbType.Boolean, ParameterDirection.Input);
            Ope.AddParams("TYPE", pType, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_PurchaseLiveStockGetData", CommandType.StoredProcedure);
            return DTab;
        }

public DataSet GetDataForSaleInvoiceList(int pIntProcessID, string pStrStatus, string pStrMemoID, string pStrStockType,string InvType,string pStrFormDate,string pStrTodate)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("PROCESS_ID", pIntProcessID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STATUS", pStrStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMO_ID", pStrMemoID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STOCKTYPE", pStrStockType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("INVTYPE", InvType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFormDate, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrTodate, DbType.String, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Table", "Trn_SaleInvoiceManGetData", CommandType.StoredProcedure);
            return DS;
        }

        public DataRow GetPartyDetails(Guid StrLedger_ID)
        {
            Ope.ClearParams();
            string StrSql = "SELECT EmailID,LedgerCode,AccCode,MobileNo1 FROM dbo.MST_Ledger where Ledger_ID = '" + StrLedger_ID + "' ";
            DataRow DRow = Ope.GetDataRow(BusLib.Configuration.BOConfiguration.ConnectionString, BusLib.Configuration.BOConfiguration.ProviderName, StrSql, CommandType.Text);
            return DRow;
        }
    }
}
