using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;


namespace BusLib.Report
{
    public class BOTRN_StockTallyReport
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataSet StockTallyReportGetData(string strFromDate, string strToDate,string StrOpe,string StrMemo_ID,string StrEntryDate, int StrLab,string StrType)
        {
            Ope.ClearParams();
            DataSet DSet = new DataSet();
            Ope.AddParams("FromDate",Val.SqlDate(strFromDate), DbType.Date, ParameterDirection.Input);
            Ope.AddParams("ToDate", Val.SqlDate(strToDate), DbType.Date, ParameterDirection.Input);
            Ope.AddParams("EntryDate", StrEntryDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("Ope", StrOpe, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Memo_ID ", StrMemo_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LAB_ID ", StrLab, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("DiamondType", StrType, DbType.String, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DSet,"Temp", "Grep_StockTallyReport", CommandType.StoredProcedure);
            return DSet;
        }

        public DataSet GetDataForFinalStockReport(string StrOpe)
        {
            Ope.ClearParams();
            DataSet DSet = new DataSet();
            Ope.AddParams("Ope", StrOpe, DbType.String, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DSet, "Temp", "Grep_FinalStockReport", CommandType.StoredProcedure);
            return DSet;
        }
    }
}
