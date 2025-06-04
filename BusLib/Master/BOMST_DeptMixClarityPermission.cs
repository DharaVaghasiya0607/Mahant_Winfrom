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
    public class BOMST_DeptMixClarityPermission
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();



        public DataTable Fill(int pStrDepartment,Guid pGuidUser_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("DEPARTMENT_ID", pStrDepartment, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("USER_ID", pGuidUser_ID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "MST_MixClarityPermissionGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DeptWiseMixClaPermissionProperty Save(DeptWiseMixClaPermissionProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("PERMISSION_ID", pClsProperty.PERMISSION_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("DEPARTMENT_ID", pClsProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("USER_ID", pClsProperty.USER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("MIXSIZE_ID", pClsProperty.MIXSIZE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MIXCLARITY_ID", pClsProperty.MIXCLARITY_ID, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ISACTIVE", pClsProperty.ISACTIVE, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_MixClarityPermissionSave", CommandType.StoredProcedure);

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


        public DeptWiseMixClaPermissionProperty Delete(DeptWiseMixClaPermissionProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("PERMISSION_ID", pClsProperty.PERMISSION_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_MixClarityPermissionDelete", CommandType.StoredProcedure);

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
