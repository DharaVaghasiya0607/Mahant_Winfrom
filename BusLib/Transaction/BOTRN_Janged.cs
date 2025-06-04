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
    public class BOTRN_Janged
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public TRN_JangedProperty Save(TRN_JangedProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("Janged_ID", pClsProperty.Janged_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("Company_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("JangedDate", pClsProperty.JangedDate, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("FinYear", pClsProperty.FinYear, DbType.String, ParameterDirection.Input);
                Ope.AddParams("JangedNo", pClsProperty.JangedNo, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("Party_ID", pClsProperty.Party_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("Broker_ID", pClsProperty.Broker_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("Through", pClsProperty.Through, DbType.String, ParameterDirection.Input);
                Ope.AddParams("Currency", pClsProperty.Currency, DbType.String, ParameterDirection.Input);

                Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("DETAILXML", pClsProperty.DetailXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnValueStr", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);


                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_JangedSave", CommandType.StoredProcedure);

                if (AL.Count != 0)
                {
                    pClsProperty.ReturnJangedNo = Val.ToString(AL[0]);
                    pClsProperty.ReturnJangedStr = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnJangedNo = "";
                pClsProperty.ReturnJangedStr = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;
        }

        public DataTable FillSummary(string pStrFromData,string pStrToDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("FROMDATE", pStrFromData, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("Company_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input); 

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_JangedGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable FillDetail(string pStrJangedID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("Janged_ID", pStrJangedID, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_JangedGetDetail", CommandType.StoredProcedure);
            return DTab;
        }

        public TRN_JangedProperty Delete(TRN_JangedProperty pClsProperty)
        {
            try
            {

                Ope.ClearParams();
                Ope.AddParams("Janged_ID", pClsProperty.Janged_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);


                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_JangedGetDetail", CommandType.StoredProcedure);

                if (AL.Count != 0)
                {
                    pClsProperty.ReturnJangedNo = Val.ToString(AL[0]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnJangedNo = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;
        }

        public DataTable Print(string pStrJangedID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("Janged_ID", pStrJangedID, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_JangedGetPrint", CommandType.StoredProcedure);
            return DTab;
        }

    }

}
