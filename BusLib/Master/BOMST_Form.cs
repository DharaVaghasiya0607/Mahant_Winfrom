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
    public class BOMST_Form
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable Fill()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "MST_FormMasterGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public FormMasterProperty Save(FormMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("FORM_ID", pClsProperty.FORM_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("FORMGROUP", pClsProperty.FORMGROUP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FORMNAME", pClsProperty.FORMNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FORMDESCNEW", pClsProperty.FORMDESCNEW, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FORMDESC", pClsProperty.FORMDESC, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL;
                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_FormMasterSave", CommandType.StoredProcedure);

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
