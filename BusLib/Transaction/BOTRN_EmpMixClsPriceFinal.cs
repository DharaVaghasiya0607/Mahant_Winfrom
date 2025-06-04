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
    public class BOTRN_EmpMixClsPriceFinal
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable GetEmpwiseMixClsPricing(string pStrKapan, string pStrShape , string pStrMixCls , string pStrMixSize)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("KAPAN", pStrKapan, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", pStrShape, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MIXCLARITY_ID", pStrMixCls, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MIXSIZE_ID", pStrMixSize, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_ParcelGetEmpwiseMixClsPricing", CommandType.StoredProcedure);

            return DTab;
        }

        public ArrayList ParcelSaveFinalEmpTFlag(string pStrIssueReturn)
        {
            ArrayList AL;
            try
            {
                Ope.ClearParams();
                Ope.AddParams("XMLFINALEMPTFLAG", pStrIssueReturn, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);
                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_ParcelSaveEmployeeFinalTFlag", CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                AL = new ArrayList();
                if (AL.Count == 0)
                {
                    AL.Add("");
                    AL.Add("");
                    AL.Add("FAIL");
                    AL.Add(ex.Message);
                }
            }
            return AL;
        }
    }
}
