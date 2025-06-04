using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AxonDataLib;
using Config = BusLib.Configuration.BOConfiguration;
using BusLib.TableName;
using System.Collections;

namespace BusLib.Master
{
    public class BOTRN_LedgerTransaction
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataSet Fill(string pStrDate, String pIntLedgerID, string pFinYear, int pCurrency_ID)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("LEDGER_ID", pIntLedgerID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINYEAR", pFinYear, DbType.String, ParameterDirection.Input);
            Ope.AddParams("VOUCHERDATE", pStrDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("CURRENCY_ID", pCurrency_ID, DbType.Int32, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Table", "ACC_LedgerTransactionIncomeExpenseGetData", CommandType.StoredProcedure);
            return DS;
        }
      
        public DataSet GetMonthWiseSummary(string pStrLedgerID,int pYYYY, int pMM)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("LEDGER_ID", pStrLedgerID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Year", pYYYY, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("Month", pMM, DbType.Int32, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS,"Temp", "RP_MonthWiseIncomeExpense", CommandType.StoredProcedure);
            return DS;
        }
      

    }
}
