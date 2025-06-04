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
    public class BOMST_ClvMixRapSizeMapping
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable Fill()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "PRI_ClvMixRapSizeMappingGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public ClvMixRapSizeMappingProperty Save(ClvMixRapSizeMappingProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
               // Ope.AddParams("ID", pClsProperty.ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("S_CODE", pClsProperty.S_CODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("RAPFROMCARAT", pClsProperty.RAPFROMCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("RAPTOCARAT", pClsProperty.RAPTOCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("MAPFROMCARAT", pClsProperty.MAPFROMCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("MAPTOCARAT", pClsProperty.MAPTOCARAT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                
                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "PRI_ClvMixRapSizeMappingSave", CommandType.StoredProcedure);

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

        public ClvMixRapSizeMappingProperty Delete(ClvMixRapSizeMappingProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("ID", pClsProperty.ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "PRI_ClvMixRapSizeMappingDelete", CommandType.StoredProcedure);

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
