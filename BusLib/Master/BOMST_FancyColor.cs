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
    public class BOMST_FancyColor
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable Fill()
        {
            DataTable DTab = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "MST_FancyColorGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public FancyColorMasterProperty Save(FancyColorMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("FANCYCOLOR_ID", pClsProperty.FANCYCOLOR_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("FANCYCODE", pClsProperty.FANCYCODE, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("FANCYCOLORNAME", pClsProperty.FANCYCOLORNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHORTNAME", pClsProperty.SHORTNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FANCYCOLOR", pClsProperty.FANCYCOLOR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FANCYOVERTONE", pClsProperty.FANCYOVERTONE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FANCYINTENSITY", pClsProperty.FANCYINTENSITY, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ISACTIVE", pClsProperty.ISACTIVE, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_FancyColorSave", CommandType.StoredProcedure);

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

      
        public FancyColorMasterProperty Delete(FancyColorMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("FANCYCOLOR_ID", pClsProperty.FANCYCOLOR_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_FancyColorDelete", CommandType.StoredProcedure);

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
