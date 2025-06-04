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
    public class BOTRN_SingleStonePriceUpdate
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();    

        public DataTable GetSingleStonePriceData(LiveStockProperty pClsProperty)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
          
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Pri_SingleStonePriceGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetStonePriceExcelWiseGetData(string pStrStonePriceXml, string pStrSearchType,string pStrPriceType) 
        {
            Ope.ClearParams();
            DataTable DT = new DataTable();
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("XMLFORSTONEPRICE", pStrStonePriceXml, DbType.Xml, ParameterDirection.Input);
            Ope.AddParams("SEARCHTYPE", pStrSearchType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PRICETYPE", pStrPriceType, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "Trn_SingleStonePriceParameterFileUpload", CommandType.StoredProcedure);
            return DT;
        }

        public MemoEntryProperty SaveSingleStonePriceUpdateDetail(string StrPriceParameteDetXML,bool pIsFromMFGSide) 
        {
            MemoEntryProperty pClsProperty = new MemoEntryProperty();
            try
            {
                Ope.ClearParams();
                DataTable DTab = new DataTable();

                Ope.AddParams("XMLDETSTR", StrPriceParameteDetXML, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("ISFROMMFGSIDE", pIsFromMFGSide, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_SingleStonePriceSave", CommandType.StoredProcedure);
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
