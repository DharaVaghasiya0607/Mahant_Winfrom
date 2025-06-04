using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AxonDataLib;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using BusLib.TableName;
using System.Collections;
using System.Data;

namespace BusLib.Transaction
{
    public class BOTRN_ParcelIssueReturn
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable GetParcelStockIssueReturn(string pStrFrmType)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("EMPLOYEE_ID", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("FRMTYPE", pStrFrmType, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_GetParcelStockIssueReturn", CommandType.StoredProcedure);
            return DTab;
        }

        public ArrayList ParcelIssueReturnSave(string pStrIssueReturn)
        {
             ArrayList AL ;
            try
            {
                Ope.ClearParams();
                Ope.AddParams("XMLFORISSUERETURN", pStrIssueReturn, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);
                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_ParcelIssueReturnSave", CommandType.StoredProcedure);
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

        public ArrayList ParcelSavePricingWithReturn(string pStrIssueReturn)
        {
            ArrayList AL;
            try
            {
                Ope.ClearParams();
                Ope.AddParams("XMLFORPRICINGWITHRETURN", pStrIssueReturn, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);
                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_ParcelSavePricingWithReturn", CommandType.StoredProcedure);
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

        public DataTable GetParcelIssueMemoIdSummery(string pStrFromType)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("EMPLOYEE_ID", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("FORMTYPE", pStrFromType, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_GetParcelIssueMemoIdSummery", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetParcelIssueMemoIdDetail(string pStrIssueNo)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("ISSUEJANGADNO", pStrIssueNo, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_GetParcelIssueMemoIdDetail", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetParcelIssueReturnHistory(string pStrKapan, string pStrShape, string pStrMixCls, string pStrMixSize , string pStrFrmDate , string pStrToDate , string pStrJangedno , string pStrFormType)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("KAPAN", pStrKapan, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", pStrShape, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MIXCLARITY_ID", pStrMixCls, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MIXSIZE_ID", pStrMixSize, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SYNCFROMDATE", pStrFrmDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("SYNCTODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("JANGEDNO", pStrJangedno, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FORMTYPE", pStrFormType, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_GetParcelIssueReturnHistory", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetParcelMixClsAndPrice()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_GetParcelMixClsAndPrice", CommandType.StoredProcedure);
            return DTab;
        }

    }
}
