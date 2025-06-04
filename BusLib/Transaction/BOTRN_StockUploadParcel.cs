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
    public class BOTRN_StockUploadParcel
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();


        public DataTable SaveStockUploadUsingDataTable(string pStrStockUploadXML, string pStockStatus, string pStockUploadType, Guid gParty_ID)
        //public string SaveStockUploadUsingDataTable(DataTable DtabStock, string pStockStatus, string pStockUploadType, Guid gParty_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("XMLSTOCKUPLOAD", pStrStockUploadXML, DbType.Xml, ParameterDirection.Input);
            Ope.AddParams("STOCKSTATUS", pStockStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("UPLOADTYPE", pStockUploadType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PARTY_ID", gParty_ID, DbType.Guid, ParameterDirection.Input);

            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_StockUploadSaveParcel", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable MFGStockSyncSave(string pStrXmlForVertifedDetail) //#P : 25-02-2020
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("XMLFORVERIDIEDDETAIL", pStrXmlForVertifedDetail, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_ParcelStockSyncFromMFGSave", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable MumbaiTrsnStockSyncSave(string pStrXmlForVertifedDetail) //#P : 25-02-2020
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("XMLFORVERIDIEDDETAIL", pStrXmlForVertifedDetail, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_ParcelMumbaiTrnSave", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable MFGStockVerifiedBeforeSave(string pStrXmlForVertifedDetail) //#P : 25-02-2020
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("XMLFORVERIDIEDDETAIL", pStrXmlForVertifedDetail, DbType.Date, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_ParcelStockVerifiBeforeSave", CommandType.StoredProcedure);
            return DTab;
        }

        public StockUploadProperty DeleteStonePricing(StockUploadProperty pClStonePricingProperty)
        {
            try
            {
                Ope.ClearParams();
                //Ope.AddParams("TRN_ID", pClStonePricingProperty.TRN_ID, DbType.Guid, ParameterDirection.Input);
                //Ope.AddParams("PARTY_ID", pClStonePricingProperty.SELLER_ID, DbType.Int64, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_StoneDelete", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClStonePricingProperty.ReturnValue = Val.ToString(AL[0]);
                    pClStonePricingProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClStonePricingProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClStonePricingProperty.ReturnValue = "";
                pClStonePricingProperty.ReturnMessageType = "FAIL";
                pClStonePricingProperty.ReturnMessageDesc = ex.Message;

            }
            return pClStonePricingProperty;

        }

        //public DataTable GetDataForParcelStockView(Guid StrParty_ID)
        //{
        //    Ope.ClearParams();
        //    DataTable DTab = new DataTable();
        //    Ope.AddParams("PARTY_ID", StrParty_ID, DbType.Guid, ParameterDirection.Input);
        //    Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_StockViewGetData", CommandType.StoredProcedure);
        //    return DTab;
        //}
        public DataTable GetDataForParcelStockView(Guid StrParty_ID, string pStrDisplay)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("PARTY_ID", StrParty_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("DISPLAYTYPE", pStrDisplay, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_StockViewGetData", CommandType.StoredProcedure);
            return DTab;
        }


        public DataTable GetStockUploadData(Guid gParty_ID, string StrOpe = "WITHOUTSTOCK") //WITHOUTSTOCK OR WITHSTOCK
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("OPE", StrOpe, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PARTY_ID", gParty_ID, DbType.Guid, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_StockUploadGetData", CommandType.StoredProcedure);

            return DTab;
        }



        public DataSet GetLiveStockData(LiveStockProperty pClsProperty)  // Used In Live Stock : 25-06-2019
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", pClsProperty.MULTYSHAPE_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MIXCLARITY_ID", pClsProperty.MULTYCLARITY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MIXSIZE_ID", pClsProperty.SIZE_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", BusLib.Configuration.BOConfiguration.gEmployeeProperty.DEPARTMENT_ID, DbType.String, ParameterDirection.Input);

            Ope.AddParams("STOCKTYPE", pClsProperty.STOCKTYPE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

            Ope.AddParams("PAGENO", pClsProperty.PAGENO, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("PAGESIZE", pClsProperty.PAGESIZE, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input); // Add Khushbu 01-06-21 

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Table", "Parcel_LiveStockParcelGetData", CommandType.StoredProcedure);

            return DS;
        }


        public DataSet GetStoneDetailForMemoForm(LiveStockProperty pClsProperty)  // Used In Live Stock : 25-06-2019
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", pClsProperty.MULTYSHAPE_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MIXCLARITY_ID", pClsProperty.MULTYCLARITY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MIXSIZE_ID", pClsProperty.SIZE_ID, DbType.String, ParameterDirection.Input);

            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STOCKTYPE", pClsProperty.STOCKTYPE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

            // Ope.AddParams("PAGENO", pClsProperty.PAGENO, DbType.Int32, ParameterDirection.Input);
            // Ope.AddParams("PAGESIZE", pClsProperty.PAGESIZE, DbType.Int32, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Table", "Parcel_LiveStockParcelGetData", CommandType.StoredProcedure);

            return DS;
        }
        public DataRow GetStockDataStoneNoWise(LiveStockProperty pClsProperty)
        {
            Ope.ClearParams();

            DataSet DS = new DataSet();

            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STOCKTYPE", "ALL", DbType.String, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Table", "Trn_LiveStockGetData", CommandType.StoredProcedure);
            if (DS.Tables.Count > 0)
                if (DS.Tables[0].Rows.Count > 0)
                    return DS.Tables[0].Rows[0];
                else
                    return null;
            else
                return null;
        }


        public DataTable GetDataForExcelExport(string StrXml, string WebStatus, string StockType, string FormatName) //WITHOUTSTOCK OR WITHSTOCK
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("STOCK_ID", StrXml, DbType.Xml, ParameterDirection.Input);
            Ope.AddParams("WEBSTATUS", WebStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STOCKTYPE", StockType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FORMAT", FormatName, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_LiveStockExportExcel", CommandType.StoredProcedure);

            return DTab;
        }


        public DataSet GetDataForExcelExportNew(string StrXml, string WebStatus, string StockType, string FormatName, LiveStockProperty pClsProperty) //WITHOUTSTOCK OR WITHSTOCK
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("STOCK_ID", StrXml, DbType.Xml, ParameterDirection.Input);
            Ope.AddParams("WEBSTATUS", WebStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STOCKTYPE", StockType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FORMAT", FormatName, DbType.String, ParameterDirection.Input);

            Ope.AddParams("SHAPE_ID", pClsProperty.MULTYSHAPE_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLOR_ID", pClsProperty.MULTYCOLOR_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CLARITY_ID", pClsProperty.MULTYCLARITY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUT_ID", pClsProperty.MULTYCUT_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("POL_ID", pClsProperty.MULTYPOL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SYM_ID", pClsProperty.MULTYSYM_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FL_ID", pClsProperty.MULTYFL_ID, DbType.String, ParameterDirection.Input);
            //Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);

            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BOX_ID", pClsProperty.BOX_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LABREPORTNO", pClsProperty.LABREPORTNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SERIALNO", pClsProperty.SERIALNO, DbType.String, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT", pClsProperty.FROMCARAT, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT", pClsProperty.TOCARAT, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("PARTY_ID", pClsProperty.MULTYPARTY_ID, DbType.String, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Trn_LiveStockExportExcelModified", CommandType.StoredProcedure);

            return DS;
        }

        public DataTable GetLoginHistoryData(string pStrFromDate, string pStrToDate)
        {
            Ope.ClearParams();
            DataTable DT = new DataTable();
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "TRN_LoginHistoryGetData", CommandType.StoredProcedure);
            return DT;
        }

        public DataTable GetSearchCriteriaHistoryData(string pStrFromDate, string pStrToDate)
        {
            Ope.ClearParams();
            DataTable DT = new DataTable();
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "TRN_SearchHistoryGetData", CommandType.StoredProcedure);
            return DT;
        }


        public DataTable GetPricePacketDetailExcelWise(string pStrXmlForPricePacketDetail) //#P : 16-03-2020
        {
            Ope.ClearParams();
            DataTable DT = new DataTable();
            Ope.AddParams("XMLPRICEPACKETDETAIL", pStrXmlForPricePacketDetail, DbType.Date, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "Trn_PriceParameterExcelGetData", CommandType.StoredProcedure);
            return DT;
        }
        public DataTable GetPriceParameterGetData(LiveStockProperty pClsProperty) //#P : 16-03-2020
        {
            Ope.ClearParams();
            DataTable DT = new DataTable();
            Ope.AddParams("SHAPE_ID", pClsProperty.MULTYSHAPE_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLOR_ID", pClsProperty.MULTYCOLOR_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CLARITY_ID", pClsProperty.MULTYCLARITY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUT_ID", pClsProperty.MULTYCUT_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("POL_ID", pClsProperty.MULTYPOL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SYM_ID", pClsProperty.MULTYSYM_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FL_ID", pClsProperty.MULTYFL_ID, DbType.String, ParameterDirection.Input);

            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT", pClsProperty.FROMCARAT, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT", pClsProperty.TOCARAT, DbType.Decimal, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "Trn_PriceParameterGetData", CommandType.StoredProcedure);
            return DT;
        }
        public MemoEntryProperty SaveParameterOrPriceUpdateDetail(string StrPriceParameteMstXML, string StrPriceParameteDetXML) //Add : Pinali : 28-07-2019
        {
            MemoEntryProperty pClsProperty = new MemoEntryProperty();
            try
            {

                Ope.ClearParams();
                DataTable DTab = new DataTable();

                Ope.AddParams("XMLMSTSTR", StrPriceParameteMstXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", StrPriceParameteDetXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_PriceParameterSave", CommandType.StoredProcedure);
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
        public DataSet GetStoneHistoryData(Guid StockId) //Add : Bhagyashree : 27/07/2019
        {
            Ope.ClearParams();
            DataSet Ds = new DataSet();
            Ope.AddParams("STOCK_ID", StockId, DbType.Guid, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, Ds, "Table", "TRN_GetStoneHistoryData", CommandType.StoredProcedure);
            return Ds;
        }
        public DataSet QuickSearchColorClarityWiseData(string REPORTTYPE, string StrShape, string StrSize, string StrPriceType, string StrCurrencyType) // Add : Bhagyshree
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("REPORTTYPE", REPORTTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHAPE", StrShape, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SIZE", StrSize, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PRICETYPE", StrPriceType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CURRENCYTYPE", StrCurrencyType, DbType.String, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "RPT_QuickSearchColorClarityWiseData", CommandType.StoredProcedure);
            return DS;
        }
        public LiveStockProperty SaveAssignBox(string strBoxMstXML, Int32 Box_Id)
        {
            LiveStockProperty pClsroperty = new LiveStockProperty();
            try
            {
                Ope.ClearParams();
                Ope.AddParams("XMLMASTER", strBoxMstXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("BOX_ID", Box_Id, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_PacketAssignBoxSave", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsroperty.ReturnValue = Val.ToString(AL[0]);
                    pClsroperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsroperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClsroperty.ReturnValue = "";
                pClsroperty.ReturnMessageType = "FAIL";
                pClsroperty.ReturnMessageDesc = ex.Message;
            }
            return pClsroperty;
        }

        public int SaveGridLayout(string pStrFormName, string pStrGridName, string pStrGridLayout)
        {
            string StrQuery = "";

            Ope.ClearParams();

            StrQuery = "Delete From MST_GridLayout With(RowLock) Where Employee_ID = '" + Config.gEmployeeProperty.LEDGER_ID + "' And FormName='" + pStrFormName + "' And GridName='" + pStrGridName + "' ";
            StrQuery += " Insert Into MST_GridLayout With(RowLock) (Employee_ID,FormName,GridName,GridLayout) Values ('" + Config.gEmployeeProperty.LEDGER_ID + "','" + pStrFormName + "','" + pStrGridName + "','" + pStrGridLayout + "') ";

            return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
        }

        public int DeleteGridLayout(string pStrFormName, string pStrGridName)
        {
            string StrQuery = "";

            Ope.ClearParams();

            StrQuery = "Delete From MST_GridLayout With(RowLock) Where Employee_ID = '" + Config.gEmployeeProperty.LEDGER_ID + "' And FormName='" + pStrFormName + "' And GridName='" + pStrGridName + "' ";

            return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
        }

        public string GetGridLayout(string pStrFormName, string pStrGridName)
        {
            Ope.ClearParams();

            string StrQuery = " And Employee_ID = '" + Config.gEmployeeProperty.LEDGER_ID + "' AND FormName = '" + pStrFormName + "' And GridName = '" + pStrGridName + "'";

            return Ope.FindText(Config.ConnectionString, Config.ProviderName, "MST_GridLayout", "GridLayout", StrQuery);

        }

        public DataTable GetCount(LiveStockProperty pClsProperty)  // Used In Live Stock : 25-06-2019
        {
            DataTable DTab = new DataTable();

            Ope.ClearParams();

            Ope.AddParams("SHAPE_ID", pClsProperty.MULTYSHAPE_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLOR_ID", pClsProperty.MULTYCOLOR_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CLARITY_ID", pClsProperty.MULTYCLARITY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUT_ID", pClsProperty.MULTYCUT_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("POL_ID", pClsProperty.MULTYPOL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SYM_ID", pClsProperty.MULTYSYM_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FL_ID", pClsProperty.MULTYFL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FANCYCOLOR_ID", pClsProperty.MULTYFANCYCOLOR_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LOCATION_ID", pClsProperty.MULTYLOCATION_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MILKY_ID", pClsProperty.MULTYMILKY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LAB_ID", pClsProperty.MULTYLAB_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BOX_ID", pClsProperty.MULTYBOX_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("WEBSTATUS", pClsProperty.WEBSTATUS, DbType.String, ParameterDirection.Input);

            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LABREPORTNO", pClsProperty.LABREPORTNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SERIALNO", pClsProperty.SERIALNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMONO", pClsProperty.MEMONO, DbType.String, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT1", pClsProperty.FROMCARAT1, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT1", pClsProperty.TOCARAT1, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT2", pClsProperty.FROMCARAT2, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT2", pClsProperty.TOCARAT2, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT3", pClsProperty.FROMCARAT3, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT3", pClsProperty.TOCARAT3, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT4", pClsProperty.FROMCARAT4, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT4", pClsProperty.TOCARAT4, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT5", pClsProperty.FROMCARAT5, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT5", pClsProperty.TOCARAT5, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMLENGTH", pClsProperty.FROMLENGTH, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOLENGTH", pClsProperty.TOLENGTH, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMWIDTH", pClsProperty.FROMWIDTH, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOWIDTH", pClsProperty.TOWIDTH, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMHEIGHT", pClsProperty.FROMHEIGHT, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOHEIGHT", pClsProperty.TOHEIGHT, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMTABLEPER", pClsProperty.FROMTABLEPER, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOTABLEPER", pClsProperty.TOTABLEPER, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMDEPTHPER", pClsProperty.FROMDEPTHPER, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TODEPTHPER", pClsProperty.TODEPTHPER, DbType.Decimal, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_LiveStockGetCount", CommandType.StoredProcedure);

            return DTab;
        }

        public DataSet GetLiveStockDataNew(LiveStockProperty pClsProperty)  // Used In Live Stock : 25-06-2019
        {
            DataSet DS = new DataSet();

            Ope.ClearParams();

            Ope.AddParams("SHAPE_ID", pClsProperty.MULTYSHAPE_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLOR_ID", pClsProperty.MULTYCOLOR_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CLARITY_ID", pClsProperty.MULTYCLARITY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUT_ID", pClsProperty.MULTYCUT_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("POL_ID", pClsProperty.MULTYPOL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SYM_ID", pClsProperty.MULTYSYM_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FL_ID", pClsProperty.MULTYFL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FANCYCOLOR_ID", pClsProperty.MULTYFANCYCOLOR_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LOCATION_ID", pClsProperty.MULTYLOCATION_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MILKY_ID", pClsProperty.MULTYMILKY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LAB_ID", pClsProperty.MULTYLAB_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BOX_ID", pClsProperty.MULTYBOX_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("WEBSTATUS", pClsProperty.WEBSTATUS, DbType.String, ParameterDirection.Input);

            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LABREPORTNO", pClsProperty.LABREPORTNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SERIALNO", pClsProperty.SERIALNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMONO", pClsProperty.MEMONO, DbType.String, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT1", pClsProperty.FROMCARAT1, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT1", pClsProperty.TOCARAT1, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT2", pClsProperty.FROMCARAT2, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT2", pClsProperty.TOCARAT2, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT3", pClsProperty.FROMCARAT3, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT3", pClsProperty.TOCARAT3, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT4", pClsProperty.FROMCARAT4, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT4", pClsProperty.TOCARAT4, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT5", pClsProperty.FROMCARAT5, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT5", pClsProperty.TOCARAT5, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMLENGTH", pClsProperty.FROMLENGTH, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOLENGTH", pClsProperty.TOLENGTH, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMWIDTH", pClsProperty.FROMWIDTH, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOWIDTH", pClsProperty.TOWIDTH, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMHEIGHT", pClsProperty.FROMHEIGHT, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOHEIGHT", pClsProperty.TOHEIGHT, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMTABLEPER", pClsProperty.FROMTABLEPER, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOTABLEPER", pClsProperty.TOTABLEPER, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMDEPTHPER", pClsProperty.FROMDEPTHPER, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TODEPTHPER", pClsProperty.TODEPTHPER, DbType.Decimal, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Trn_LiveStockGetData", CommandType.StoredProcedure);

            return DS;
        }
        public DataSet GetStockStatusSearchData(LiveStockProperty pClsProperty)  // Used In Stock Staus Search : #P : 05-04-2020
        {
            DataSet DS = new DataSet();

            Ope.ClearParams();

            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LABREPORTNO", pClsProperty.LABREPORTNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SERIALNO", pClsProperty.SERIALNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMONO", pClsProperty.MEMONO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STOCKTYPE", "SINGLE", DbType.String, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Trn_StockStatusGetData", CommandType.StoredProcedure);

            return DS;
        }


        //public string BulkPropertUpdate(string pStrStoneType, string pStrParaType, string pStrStoneNoXML, string pStrValueXML,Guid pStrParty_ID,string pStrComment) //Add : Pinali : 28-07-2019
        //{
        //    try
        //    {

        //        Ope.ClearParams();
        //        DataTable DTab = new DataTable();

        //        Ope.AddParams("STONETYPE", pStrStoneType, DbType.String, ParameterDirection.Input);
        //        Ope.AddParams("PARATYPE", pStrParaType, DbType.String, ParameterDirection.Input);
        //        Ope.AddParams("STONENOXML", pStrStoneNoXML, DbType.Xml, ParameterDirection.Input);
        //        Ope.AddParams("VALUEXML", pStrValueXML, DbType.Xml, ParameterDirection.Input);

        //        Ope.AddParams("PARTY_ID", pStrParty_ID, DbType.Guid, ParameterDirection.Input);
        //        Ope.AddParams("COMMENT", pStrComment, DbType.String, ParameterDirection.Input);

        //        Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
        //        Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
        //        Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

        //        ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_BulkPropertyUpdate", CommandType.StoredProcedure);
        //        if (AL.Count != 0)
        //        {
        //            string ReturnValue = Val.ToString(AL[0]);
        //            string ReturnMessageType = Val.ToString(AL[1]);
        //            string ReturnMessageDesc = Val.ToString(AL[2]);

        //            return Val.ToString(AL[2]);
        //        }
        //        return "";
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return ex.Message;

        //    }
        //}
        public DataTable BulkPropertUpdate(string pStrStoneType, string pStrParaType, string pStrStoneNoXML, string pStrValueXML, Guid pStrParty_ID, string pStrComment) //Chng : Dhara
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("STONETYPE", pStrStoneType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PARATYPE", pStrParaType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STONENOXML", pStrStoneNoXML, DbType.Xml, ParameterDirection.Input);
            Ope.AddParams("VALUEXML", pStrValueXML, DbType.Xml, ParameterDirection.Input);

            Ope.AddParams("PARTY_ID", pStrParty_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("COMMENT", pStrComment, DbType.String, ParameterDirection.Input);

            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

            //Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
            //Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
            //Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);


            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_BulkPropertyUpdate", CommandType.StoredProcedure);
            return DTab;



        }
        public DataTable GetDataForStockBarcodePrint(string pStrStoneXml) //Used In Barcode Print
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            //Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
            //Ope.AddParams("LABREPORTNO", pClsProperty.LABREPORTNO, DbType.String, ParameterDirection.Input);
            //Ope.AddParams("SERIALNO", pClsProperty.SERIALNO, DbType.String, ParameterDirection.Input);
            //Ope.AddParams("MEMONO", pClsProperty.MEMONO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STOCKTYPE", "SINGLE", DbType.String, ParameterDirection.Input);
            Ope.AddParams("STONENOXML", pStrStoneXml, DbType.Xml, ParameterDirection.Input);


            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_StockBarcodePrint", CommandType.StoredProcedure);

            return DTab;
        }

        public DataSet GetLiveStockDataExclusive(LiveStockProperty pClsProperty)  // Used In Live Stock : 25-06-2019
        {
            DataSet DS = new DataSet();

            Ope.ClearParams();

            Ope.AddParams("SHAPE_ID", pClsProperty.MULTYSHAPE_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLOR_ID", pClsProperty.MULTYCOLOR_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CLARITY_ID", pClsProperty.MULTYCLARITY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUT_ID", pClsProperty.MULTYCUT_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("POL_ID", pClsProperty.MULTYPOL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SYM_ID", pClsProperty.MULTYSYM_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FL_ID", pClsProperty.MULTYFL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FANCYCOLOR_ID", pClsProperty.MULTYFANCYCOLOR_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LOCATION_ID", pClsProperty.MULTYLOCATION_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MILKY_ID", pClsProperty.MULTYMILKY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LAB_ID", pClsProperty.MULTYLAB_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BOX_ID", pClsProperty.MULTYBOX_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("WEBSTATUS", pClsProperty.WEBSTATUS, DbType.String, ParameterDirection.Input);

            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LABREPORTNO", pClsProperty.LABREPORTNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SERIALNO", pClsProperty.SERIALNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMONO", pClsProperty.MEMONO, DbType.String, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT1", pClsProperty.FROMCARAT1, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT1", pClsProperty.TOCARAT1, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT2", pClsProperty.FROMCARAT2, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT2", pClsProperty.TOCARAT2, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT3", pClsProperty.FROMCARAT3, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT3", pClsProperty.TOCARAT3, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT4", pClsProperty.FROMCARAT4, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT4", pClsProperty.TOCARAT4, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT5", pClsProperty.FROMCARAT5, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT5", pClsProperty.TOCARAT5, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMLENGTH", pClsProperty.FROMLENGTH, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOLENGTH", pClsProperty.TOLENGTH, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMWIDTH", pClsProperty.FROMWIDTH, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOWIDTH", pClsProperty.TOWIDTH, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMHEIGHT", pClsProperty.FROMHEIGHT, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOHEIGHT", pClsProperty.TOHEIGHT, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMTABLEPER", pClsProperty.FROMTABLEPER, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOTABLEPER", pClsProperty.TOTABLEPER, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMDEPTHPER", pClsProperty.FROMDEPTHPER, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TODEPTHPER", pClsProperty.TODEPTHPER, DbType.Decimal, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Trn_LiveStockGetDataForExclusive", CommandType.StoredProcedure);

            return DS;
        }

        public DataSet GetLiveStockDataNewArrivals(LiveStockProperty pClsProperty)  // Used In Live Stock : 25-06-2019
        {
            DataSet DS = new DataSet();

            Ope.ClearParams();

            Ope.AddParams("SHAPE_ID", pClsProperty.MULTYSHAPE_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLOR_ID", pClsProperty.MULTYCOLOR_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CLARITY_ID", pClsProperty.MULTYCLARITY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUT_ID", pClsProperty.MULTYCUT_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("POL_ID", pClsProperty.MULTYPOL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SYM_ID", pClsProperty.MULTYSYM_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FL_ID", pClsProperty.MULTYFL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FANCYCOLOR_ID", pClsProperty.MULTYFANCYCOLOR_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LOCATION_ID", pClsProperty.MULTYLOCATION_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MILKY_ID", pClsProperty.MULTYMILKY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LAB_ID", pClsProperty.MULTYLAB_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BOX_ID", pClsProperty.MULTYBOX_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("WEBSTATUS", pClsProperty.WEBSTATUS, DbType.String, ParameterDirection.Input);

            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LABREPORTNO", pClsProperty.LABREPORTNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SERIALNO", pClsProperty.SERIALNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMONO", pClsProperty.MEMONO, DbType.String, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT1", pClsProperty.FROMCARAT1, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT1", pClsProperty.TOCARAT1, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT2", pClsProperty.FROMCARAT2, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT2", pClsProperty.TOCARAT2, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT3", pClsProperty.FROMCARAT3, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT3", pClsProperty.TOCARAT3, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT4", pClsProperty.FROMCARAT4, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT4", pClsProperty.TOCARAT4, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT5", pClsProperty.FROMCARAT5, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT5", pClsProperty.TOCARAT5, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMLENGTH", pClsProperty.FROMLENGTH, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOLENGTH", pClsProperty.TOLENGTH, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMWIDTH", pClsProperty.FROMWIDTH, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOWIDTH", pClsProperty.TOWIDTH, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMHEIGHT", pClsProperty.FROMHEIGHT, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOHEIGHT", pClsProperty.TOHEIGHT, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMTABLEPER", pClsProperty.FROMTABLEPER, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOTABLEPER", pClsProperty.TOTABLEPER, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMDEPTHPER", pClsProperty.FROMDEPTHPER, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TODEPTHPER", pClsProperty.TODEPTHPER, DbType.Decimal, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Trn_LiveStockGetDataForNewArrivals", CommandType.StoredProcedure);

            return DS;
        }
        public LiveStockProperty UpdateExclusive(string strExclusiveXML, bool pIsExclusive)
        {
            LiveStockProperty pClsroperty = new LiveStockProperty();
            try
            {
                Ope.ClearParams();
                Ope.AddParams("XMLMASTER", strExclusiveXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ISEXCLUSIVE", pIsExclusive, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_LiveStockAddExclusiveStone", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsroperty.ReturnValue = Val.ToString(AL[0]);
                    pClsroperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsroperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClsroperty.ReturnValue = "";
                pClsroperty.ReturnMessageType = "FAIL";
                pClsroperty.ReturnMessageDesc = ex.Message;
            }
            return pClsroperty;
        }
        public DataTable GetDataForOfferPrice(string pStockNo, string pStrFromdate, string pStrToDate, string pStrParty_ID) //Used In Barcode Print
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("STOCKNO", pStockNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromdate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("PARTY_ID", pStrParty_ID, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_OfferPriceGetData", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable PartyAPICallGetData(string pStrFromdate, string pStrToDate, string pStrParty_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("FROMDATE", pStrFromdate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("PARTY_ID", pStrParty_ID, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "HST_PartyAPICallGetData", CommandType.StoredProcedure);

            return DTab;
        }


        public DataTable GetDataForStockStatment(string StrStockID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("STOCK_ID", StrStockID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_StockStatementGetDataNew", CommandType.StoredProcedure);
            return DTab;
        }
        public MemoEntryProperty Parcel_StockConsiderAsLoss(string StrXML, string StrFlag) //Add : Khushbu : 29-09-21
        {
            MemoEntryProperty pClsProperty = new MemoEntryProperty();
            try
            {
                Ope.ClearParams();
                DataTable DTab = new DataTable();

                Ope.AddParams("XMLDETSTR", StrXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("FLAG", StrFlag, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_StockLossSave", CommandType.StoredProcedure);
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
        public DataTable GetParcelToparcel(string pStrFromDate, string pStrToDate, string StrOPE)//urvisha
        {
            Ope.ClearParams();
            DataTable DT = new DataTable();
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("OPE", StrOPE, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "RP_ParcelToParcelTransferReport", CommandType.StoredProcedure);
            return DT;
        }

        public DataTable GetParcelLiveStockData(Int32 StrLot_Id, Int32 StrBox_Id, Int32 StrSize_Id, Int32 StrColor_Id, Int32 StrPurity_Id, Int32 StrShape_Id, string StrCutNo, bool StrisZeroCarat)
        {
            Ope.ClearParams();
            DataTable DT = new DataTable();
            Ope.AddParams("LOT_ID", StrLot_Id, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("BOX_ID", StrBox_Id, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("SIZE_ID", StrSize_Id, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("COLOR_ID", StrColor_Id, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("PURITY_ID", StrPurity_Id, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", StrShape_Id, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("CUTNO", StrCutNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("DISPLAYZEROCARAT", StrisZeroCarat, DbType.Boolean, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "Parcel_LiveStockGetDataNew", CommandType.StoredProcedure);
            return DT;
        }

        public DataTable CoupleStoneGetData(Int32 StrBox_Id)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("BOX_ID", StrBox_Id, DbType.Int32, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_CoupleStoneGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public CoupleStoneProperty CoupleStone_Delete(CoupleStoneProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("BOX_ID", pClsProperty.BOX_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MFG_ID", pClsProperty.MFG_ID, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_CoupleStone_Delete", CommandType.StoredProcedure);

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

        public PurchaseProperty CoupleStoneSave(string coupleStoneUploadXml, PurchaseProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("coupleStoneUploadXml", coupleStoneUploadXml, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_CoupleStoneUpload_FileUpload", CommandType.StoredProcedure);

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

        public DataTable ParcelLiveStock_Export(string pStrStockNo)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("STOCKNO", pStrStockNo, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_ParcelLiveStockExportExcelModified", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable GetParcelLiveStockDataNew(string pStrFromDate, string pStrToDate, Int32 StrBoxId, string pStrShape, string pStrSize, string pStrClarity)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("FROMDATE", Val.SqlDate(pStrFromDate), DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", Val.SqlDate(pStrToDate), DbType.Date, ParameterDirection.Input);
            Ope.AddParams("BOX_ID", StrBoxId, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHAPE", pStrShape, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MIXSIZE", pStrSize, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MIXCLARITY", pStrClarity, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_LiveStockParcelGetDataNew", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetDataForStockHistory(int StrBoxId)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("BOX_ID", StrBoxId, DbType.Int32, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BoxTransactionGetData", CommandType.StoredProcedure);
            return DTab;
        }



        public ParcelBoxMasterProperty AddLessStockSave(ParcelBoxMasterProperty pClsroperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("BOX_ID", pClsroperty.BOX_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("PARTY_ID", pClsroperty.PARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("CARAT", pClsroperty.CARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("RATE", pClsroperty.RATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("AMOUNT", pClsroperty.AMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ENTRYTYPE", pClsroperty.ENTRYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsroperty.REMARK, DbType.String, ParameterDirection.Input);

                Ope.AddParams("BROKER_ID", pClsroperty.BROKER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("BROKPER", pClsroperty.BROKPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BRKAMOUNT", pClsroperty.BRKAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DISCOUNT", pClsroperty.DISCOUNT, DbType.Double, ParameterDirection.Input);



                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_BoxTransaction_Insert", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsroperty.RETURNVALUE = Val.ToString(AL[0]);
                    pClsroperty.RETURNMESSAGETYPE = Val.ToString(AL[1]);
                    pClsroperty.RETURNMESSAGEDESC = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClsroperty.RETURNVALUE = "";
                pClsroperty.RETURNMESSAGETYPE = "FAIL";
                pClsroperty.RETURNMESSAGEDESC = ex.Message;
            }
            return pClsroperty;
        }
    }

    

}
