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

namespace BusLib.Transaction
{
    public class BOTRN_Basket
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataSet GetCartDetailData(string FrmDate,string ToDate)
        {
            Ope.ClearParams();
            DataSet Ds = new DataSet();
 
            Ope.AddParams("FROMDATE", FrmDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("ToDATE", ToDate, DbType.Date, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, Ds, "Table", "TRN_CartDetailGetData", CommandType.StoredProcedure);
            return Ds;
        }
    }
}
