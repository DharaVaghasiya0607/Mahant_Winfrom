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
    public class BOMST_Process
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable Fill()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "MST_ProcessGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public ProcessMasterProperty Save(ProcessMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("PROCESS_ID", pClsProperty.PROCESS_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("PROCESSCODE", pClsProperty.PROCESSCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PROCESSNAME", pClsProperty.PROCESSNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("JANGEDPREFIX", pClsProperty.JANGEDPREFIX, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PRINTHEADER", pClsProperty.PRINTHEADER, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLDATEPREFIX", pClsProperty.BILLDATEPREFIX, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLNOPREFIX", pClsProperty.BILLNOPREFIX, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SEQUENCENO", pClsProperty.SEQUENCENO, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("PREVPROCESS_ID", pClsProperty.PREVPROCESS_ID, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ISACTIVE", pClsProperty.ISACTIVE, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("ISACTIVESTATUS", pClsProperty.ISACTIVESTATUS, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("STOCKSTATUS", pClsProperty.STOCKSTATUS, DbType.String, ParameterDirection.Input);
                Ope.AddParams("WEBSTATUS", pClsProperty.WEBSTATUS, DbType.String, ParameterDirection.Input);

                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL;
                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_ProcessSave", CommandType.StoredProcedure);
                
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

        public ProcessMasterProperty Delete(ProcessMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("PROCESS_ID", pClsProperty.PROCESS_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL;
                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_ProcessDelete", CommandType.StoredProcedure);
                
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
