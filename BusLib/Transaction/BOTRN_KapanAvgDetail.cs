using AxonDataLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using System.Collections;
using BusLib.TableName;

namespace BusLib.Transaction
{
    public class BOTRN_KapanAvgDetail
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable Fill()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int16, ParameterDirection.Input);
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_KapanAvgDetailGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public TrnKapanAvgKapanProperty Save(TrnKapanAvgKapanProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("DETAIL_ID", pClsProperty.DETAIL_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("KAPAN_ID", pClsProperty.KAPAN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EXCRATE", pClsProperty.EXCRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SURATAVARAGE", pClsProperty.SURATAVARAGE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int16, ParameterDirection.Input);
                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_KapanAvgDetailSave", CommandType.StoredProcedure);

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


        public TrnKapanAvgKapanProperty Delete(TrnKapanAvgKapanProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("DETAIL_ID", pClsProperty.DETAIL_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_kapanavgdetailDelete", CommandType.StoredProcedure);

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
