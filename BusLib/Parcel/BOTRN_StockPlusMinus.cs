using BusLib.TableName;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Config = BusLib.Configuration.BOConfiguration;

namespace BusLib.Parcel
{
    public class BOTRN_StockPlusMinus
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public StockPlusMinusProperty Save(StockPlusMinusProperty pClsproperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("STOCK_ID", pClsproperty.STOCK_ID, DbType.Guid,ParameterDirection.Input);
                Ope.AddParams("OLDBALCARAT", pClsproperty.OLDBALCARAT, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("PLUSMINUSCARAT", pClsproperty.PLUSMINUSCARAT, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsproperty.REMARK, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

               

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_StockPlusMinusSave", CommandType.StoredProcedure);

                if(AL.Count != 0)
                {
                    pClsproperty.ReturnValue = Val.ToString(AL[0]);
                    pClsproperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsproperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch(System.Exception ex)
            {
                pClsproperty.ReturnValue = "";
                pClsproperty.ReturnMessageType = "SUCCESS";
                pClsproperty.ReturnMessageDesc = ex.Message;
            }
            return pClsproperty;
        }

        public DataTable GetStockPlusMinusData(string pStrDate)
        {
            Ope.ClearParams();
            DataTable Dtab = new DataTable();
            Ope.AddParams("STRDATE", pStrDate, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, Dtab, "TRN_StockPlusMinusGetData", CommandType.StoredProcedure);
            return Dtab;
        }
    }
}
