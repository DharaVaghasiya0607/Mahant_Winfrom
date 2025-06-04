using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using BusLib.TableName;
using System.Collections;
using System.Data;

namespace BusLib.Master
{
   public class BOMST_Subscribe
    {
       AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
       AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

       public DataTable Fill()
       {
           Ope.ClearParams();
           DataTable DTab = new DataTable();
           Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "JW_MST_SubscribeGetData", CommandType.StoredProcedure);
           return DTab;
       }

       public SubscribeProperty Save(SubscribeProperty pC1sProperty)
       {
           try
           {
                Ope.AddParams("Email_id", pC1sProperty.EMAILID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ISACTIVE", pC1sProperty.ISACTIVE, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL;
                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "JW_MST_Subscribe_insertupdate", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pC1sProperty.ReturnValue = Val.ToString(AL[0]);
                    pC1sProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pC1sProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
           }
           catch (System.Exception ex)
           {
               pC1sProperty.ReturnValue = "";
               pC1sProperty.ReturnMessageType = "FAIL";
               pC1sProperty.ReturnMessageDesc = ex.Message;

           }
           return pC1sProperty;

       }



    }
}
