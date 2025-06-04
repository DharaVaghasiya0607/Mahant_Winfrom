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
    public class BOTRN_KapanWiseMumbaiRate
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable Fill(string pStrFromDate,string pStrToDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_KapanWiseMumbaiRateGetData", CommandType.StoredProcedure);
            return DTab;
        }
        
        public KapanWiseMumbaiRateProperty Save(KapanWiseMumbaiRateProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("KAPANRATE_ID", pClsProperty.KAPANRATE_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("KAPANDATE", pClsProperty.KAPANDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("KAPANNAME", pClsProperty.KAPANNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("GIAKAPANCARAT", pClsProperty.GIAKAPANCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GIAKAPANRATE", pClsProperty.GIAKAPANRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GIAKAPANAMOUNT", pClsProperty.GIAKAPANAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("MIXKAPANCARAT", pClsProperty.MIXKAPANCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("MIXKAPANRATE", pClsProperty.MIXKAPANRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("MIXKAPANAMOUNT", pClsProperty.MIXKAPANAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
               
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_KapanWiseMumbaiRateSave", CommandType.StoredProcedure);

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

        public KapanWiseMumbaiRateProperty Delete(KapanWiseMumbaiRateProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("KAPANRATE_ID", pClsProperty.KAPANRATE_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_KapanWiseMumbaiRateDelete", CommandType.StoredProcedure);

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
