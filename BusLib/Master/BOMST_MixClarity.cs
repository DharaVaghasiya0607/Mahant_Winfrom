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

namespace BusLib.Master
{
    public class BOMST_MixClarity
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable FillMix()
        {
            DataTable DTab = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "MST_MixClarityGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public MixClarityMasterProperty SaveMix(MixClarityMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MIXCLARITYMAPP_ID", pClsProperty.MIXCLARITYMAPP_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MIXCLARITY_ID", pClsProperty.MIXCLARITY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MIXCLARITYNAME", pClsProperty.MIXCLARITY_NAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FROMCARAT", pClsProperty.FROMCARAT, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("TOCARAT", pClsProperty.TOCARAT, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("ISACTIVE", pClsProperty.ISACTIVE, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("SEQUENCENO", pClsProperty.SEQUENCENO, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("COLOR_ID", pClsProperty.COLOR_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("CLARITY_ID", pClsProperty.CLARITY_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_MixClaritySave", CommandType.StoredProcedure);

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

        public MixClarityMasterProperty DeleteMix(MixClarityMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MIXCLARITYMAPP_ID", pClsProperty.MIXCLARITYMAPP_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_MixSizeDelete", CommandType.StoredProcedure);

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
