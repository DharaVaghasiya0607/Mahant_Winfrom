using BusLib.TableName;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Config = BusLib.Configuration.BOConfiguration;

namespace BusLib.Master
{
    public class BOMST_ApiSettingMaster
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable Fill()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "MST_APISettingGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public ApiSettingProperty Save(ApiSettingProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("API_ID", pClsProperty.API_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("APICODE", pClsProperty.APICODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("APINAME", pClsProperty.APINAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("URL", pClsProperty.URL, DbType.String, ParameterDirection.Input);
                Ope.AddParams("HOSTNAME", pClsProperty.HOSTNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("APITYPE", pClsProperty.APITYPE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("EXCEL", pClsProperty.EXCEL, DbType.String, ParameterDirection.Input);
                Ope.AddParams("DIAMONDTYPE", pClsProperty.DIAMONDTYPE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("PORT", pClsProperty.PORT, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ACTIVE", pClsProperty.ACTIVE, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("USERNAME", pClsProperty.USERNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PASSWORD", pClsProperty.PASSWORD, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_ApiSettingSave", CommandType.StoredProcedure);

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

        public ApiSettingProperty Delete(ApiSettingProperty pClsProperty)
        {                      
                try
                {
                    Ope.ClearParams();
                    Ope.AddParams("API_ID", pClsProperty.API_ID, DbType.Int32, ParameterDirection.Input);

                    Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                    Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                    Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                    ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_ApiSettingDelete", CommandType.StoredProcedure);

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
