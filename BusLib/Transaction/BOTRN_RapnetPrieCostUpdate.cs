using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using AxonDataLib;
using System.Data;
using BusLib.TableName;
using System.Collections;

namespace BusLib.Transaction
{
   public class BOTRN_RapnetPrieCostUpdate
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

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

            Ope.AddParams("FROMCARAT", pClsProperty.FROMCARAT, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT", pClsProperty.TOCARAT, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);

            Ope.AddParams("RAPDATE", Val.SqlDate(pStrRapDate), DbType.Date, ParameterDirection.Input);
            Ope.AddParams("DISCHANGE", blnDisChange, DbType.Boolean, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Pri_StoneCostUpdateGetData", CommandType.StoredProcedure);
            return DTab;
        }

		public MemoEntryProperty SaveEntry(string StrMemoEntryXML, string StrMemoEntryDetXML, bool  pISAvgPrice,bool pISUpdateExpPrice, string pStrRapDate)
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

				Ope.AddParams("ISUPDATEAVGPRICE", pISAvgPrice, DbType.Boolean, ParameterDirection.Input);
				Ope.AddParams("ISUPDATEEXPPRICE", pISUpdateExpPrice, DbType.Boolean, ParameterDirection.Input);
				//Ope.AddParams("MODE", Config.ComputerIP, DbType.String, ParameterDirection.Input);

				Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
				Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
				Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

				ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_StoneCostUpdateEntrySave", CommandType.StoredProcedure);
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
