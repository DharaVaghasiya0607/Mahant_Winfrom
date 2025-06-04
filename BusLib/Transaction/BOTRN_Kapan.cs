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
    public class BOTRN_Kapan
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable Fill()
        {
            DataTable DTab = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_KapanGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public TrnKapanProperty Save(TrnKapanProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("KAPAN_ID", pClsProperty.KAPAN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("KAPANNAME", pClsProperty.KAPANNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PARCELKAPANNAME", pClsProperty.PARCELKAPANNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("KAPANCODE", pClsProperty.KAPANCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ISACTIVE", pClsProperty.ISACTIVE, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EXCRATE", pClsProperty.EXCRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SURATAVARAGE", pClsProperty.SURATAVARAGE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_KapanSave", CommandType.StoredProcedure);

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


        public TrnKapanProperty Delete(TrnKapanProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("KAPAN_ID", pClsProperty.KAPAN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_KapanDelete", CommandType.StoredProcedure);

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

        public DataTable BombayTransferView_GetData(string strFromDate, string strToDate, string KapanName)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("FromDate", strFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("ToDate", strToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("KapanName", KapanName, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BombayTransferNew_GetData", CommandType.StoredProcedure);
            return DTab;
        }
    }
}
