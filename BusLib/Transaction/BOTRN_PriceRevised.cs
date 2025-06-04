using AxonDataLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using BusLib.TableName;

namespace BusLib.Transaction
{
    public class BOTRN_PriceRevised
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public string UpdateRapnetWithAllDiscount(string pRoundXml, string pPearXml,string pStrRapDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("tbl_Round", pRoundXml, DbType.Xml, ParameterDirection.Input);
            Ope.AddParams("tbl_Pear", pPearXml, DbType.Xml, ParameterDirection.Input);
            Ope.AddParams("RAPDATE", pStrRapDate, DbType.Date, ParameterDirection.Input);

            Ope.AddParams("ENTRYBY", Configuration.BOConfiguration.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Configuration.BOConfiguration.ComputerIP, DbType.String, ParameterDirection.Input);

            Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

            ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Pri_UpdateRapnetWithAllDiscount", CommandType.StoredProcedure);

            if (AL.Count != 0)
            {
                return Val.ToString(AL[1]);
            }

            return "";
        }


        public string GetRapnetUserName()
        {
            Ope.ClearParams();
            string Str = "Select SETTINGVALUE FROM MST_Setting With(NoLock) Where SETTINGKEY = 'RapnetUserName'";
            return Ope.ExeScal(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);

        }

     
        public string GetRapnetPassword()
        {
            Ope.ClearParams();
            string Str = "Select SETTINGVALUE FROM MST_Setting With(NoLock) Where SETTINGKEY = 'RapnetPassword'";
            return Ope.ExeScal(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);

        }


        public DataTable GetOriginalRapData(string pStrOpe, string pStrRapDate, string pStrShape, double pDouFromCarat, double pDouToCarat)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHAPE", pStrShape, DbType.String, ParameterDirection.Input);
            Ope.AddParams("RAPDATE", Val.SqlDate(pStrRapDate), DbType.Date, ParameterDirection.Input);
            Ope.AddParams("F_CARAT", pDouFromCarat, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("T_CARAT", pDouToCarat, DbType.Double, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Pri_OriginalRapRateGetData", CommandType.StoredProcedure);
            return DTab;
        }
        public DataTable GetStoneRevisionData(LiveStockProperty pClsProperty, string pStrRapDate, bool blnDisChange)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", pClsProperty.MULTYSHAPE_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLOR_ID", pClsProperty.MULTYCOLOR_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CLARITY_ID", pClsProperty.MULTYCLARITY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUT_ID", pClsProperty.MULTYCUT_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("POL_ID", pClsProperty.MULTYPOL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SYM_ID", pClsProperty.MULTYSYM_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FL_ID", pClsProperty.MULTYFL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("WEBSTATUS", pClsProperty.WEBSTATUS, DbType.String, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT", pClsProperty.FROMCARAT, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT", pClsProperty.TOCARAT, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);

            Ope.AddParams("RAPDATE", Val.SqlDate(pStrRapDate), DbType.Date, ParameterDirection.Input);
            Ope.AddParams("DI" +
                "SCHANGE", blnDisChange, DbType.Boolean, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Pri_StoneRevisionGetData", CommandType.StoredProcedure);
            return DTab;
        }
        public MemoEntryProperty SavePricingMemoEntry(string StrMemoEntryXML, string StrMemoEntryDetXML,bool pISUpdateExpPrice,string pStrRapDate)
        {
            MemoEntryProperty pClsProperty = new MemoEntryProperty();
            try
            {

                Ope.ClearParams();
                DataTable DTab = new DataTable();

                Ope.AddParams("XMLMSTSTR", StrMemoEntryXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", StrMemoEntryDetXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RAPDATE", Val.SqlDate(pStrRapDate), DbType.Date, ParameterDirection.Input);

                Ope.AddParams("ISUPDATEEXPPRICE", pISUpdateExpPrice, DbType.Boolean, ParameterDirection.Input);
                //Ope.AddParams("MODE", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_StoneReviseMemoEntrySave", CommandType.StoredProcedure);
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
