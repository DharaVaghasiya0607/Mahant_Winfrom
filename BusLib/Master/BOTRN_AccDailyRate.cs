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

namespace BusLib.Master
{
    public class BOTRN_AccDailyRate
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable Fill()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "MST_AccDailyRateGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public AccDailyRateMasterProperty Save(AccDailyRateMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("ACCDAILYRATE_ID", pClsProperty.ACCDAILYRATE_ID, DbType.Int64, ParameterDirection.Input);

                Ope.AddParams("FROMDATE", pClsProperty.FROMDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("TODATE", pClsProperty.TODATE, DbType.Date, ParameterDirection.Input);

                Ope.AddParams("CURRENCY_ID", pClsProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EXCRATE", pClsProperty.EXCRATE, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("ISACTIVE", pClsProperty.ISACTIVE, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL;
                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_AccDailyRateSave", CommandType.StoredProcedure);
               
                if (AL.Count != 0)
                {
                    pClsProperty.RETURNVALUE = Val.ToString(AL[0]);
                    pClsProperty.RETURNMESSAGETYPE = Val.ToString(AL[1]);
                    pClsProperty.RETURNMESSAGEDESC = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.RETURNVALUE = "";
                pClsProperty.RETURNMESSAGETYPE = "FAIL";
                pClsProperty.RETURNMESSAGEDESC = ex.Message;

            }
            return pClsProperty;

        }


        public AccDailyRateMasterProperty Delete(AccDailyRateMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("ACCDAILYRATE_ID", pClsProperty.ACCDAILYRATE_ID, DbType.Int64, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL;
                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_AccDailyRateDelete", CommandType.StoredProcedure);
               
                if (AL.Count != 0)
                {
                    pClsProperty.RETURNVALUE = Val.ToString(AL[0]);
                    pClsProperty.RETURNMESSAGETYPE = Val.ToString(AL[1]);
                    pClsProperty.RETURNMESSAGEDESC = Val.ToString(AL[2]);
                }

            }
            catch (System.Exception ex)
            {
                pClsProperty.RETURNVALUE = "";
                pClsProperty.RETURNMESSAGETYPE = "FAIL";
                pClsProperty.RETURNMESSAGEDESC = ex.Message;

            }
            return pClsProperty;

        }

    }
}
