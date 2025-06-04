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
    public class BOTRN_StockUpload
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();


        public DataTable SaveStockUploadUsingDataTable(string pStrStockUploadXML, string pStockStatus, string pStockUploadType, Guid gParty_ID, Int32 pPrdType_ID, String pStrBillFormat, string pStrBillType)

        
        //public string SaveStockUploadUsingDataTable(DataTable DtabStock, string pStockStatus, string pStockUploadType, Guid gParty_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("XMLSTOCKUPLOAD", pStrStockUploadXML, DbType.Xml, ParameterDirection.Input);
            Ope.AddParams("STOCKSTATUS", pStockStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("UPLOADTYPE", pStockUploadType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PARTY_ID", gParty_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("PRDTYPE_ID", pPrdType_ID, DbType.Int32, ParameterDirection.Input);

            Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

            Ope.AddParams("BILLFORMAT", pStrBillFormat, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLTYPE", pStrBillType, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_StockUploadSaveNew", CommandType.StoredProcedure);

            return DTab;
        }
        public DataRow GetGIAPartyDetail(string strBillingParty)
        {
            Ope.ClearParams();
            string Str = "Select * From MST_Ledger With(NOLOCK) Where Ledger_ID  = '" + Val.ToGuid(strBillingParty) + "'";
            return Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);

        }

        public DataTable UpdateLink(string pStrUpdateLinkXML)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("UPDATELINKXML", pStrUpdateLinkXML, DbType.Xml, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_UpdateLink", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable GetDataForPacketPrint(string strStock_ID)
        {
            DataTable Dset = new DataTable();
            Ope.AddParams("STOCK_ID", strStock_ID, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, Dset, "RPT_LiveStockPacketPrint", CommandType.StoredProcedure);
            return Dset;
        }

        public DataTable GetStoneAlreadyAvaliableInSystem(string pStrSTOCKNO)
        //public string SaveStockUploadUsingDataTable(DataTable DtabStock, string pStockStatus, string pStockUploadType, Guid gParty_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("STOCKNO", pStrSTOCKNO, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_StockGetStone", CommandType.StoredProcedure);
            return DTab;

        }

        public DataTable MFGStockSyncSave(string pStrXmlForVertifedDetail) //#P : 25-02-2020
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("XMLFORVERIDIEDDETAIL", pStrXmlForVertifedDetail, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_StockSyncFromMFGSave", CommandType.StoredProcedure);
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

        public SingleStockUpdateProperty UpdateSingleStockStatus(SingleStockUpdateProperty pClStonePricingProperty, string StrStock_ID)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("STOCK_ID", StrStock_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_UpdateSingleStockStatus", CommandType.StoredProcedure);
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

        public SingleStockUpdateProperty UpdateSingleStockStatusOffline(SingleStockUpdateProperty pClStonePricingProperty, string StrStock_ID)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("STOCK_ID", StrStock_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_UpdateSingleStockStatusToOffline", CommandType.StoredProcedure);
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

            Ope.AddParams("SHAPE_ID", pClsProperty.MULTYSHAPE_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLOR_ID", pClsProperty.MULTYCOLOR_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CLARITY_ID", pClsProperty.MULTYCLARITY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUT_ID", pClsProperty.MULTYCUT_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("POL_ID", pClsProperty.MULTYPOL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SYM_ID", pClsProperty.MULTYSYM_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FL_ID", pClsProperty.MULTYFL_ID, DbType.String, ParameterDirection.Input);

            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BOX_ID", pClsProperty.BOX_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LABREPORTNO", pClsProperty.LABREPORTNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SERIALNO", pClsProperty.SERIALNO, DbType.String, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT", pClsProperty.FROMCARAT, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT", pClsProperty.TOCARAT, DbType.Decimal, ParameterDirection.Input);

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

            Ope.AddParams("STOCKTYPE", pClsProperty.STOCKTYPE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("WEBSTATUS", pClsProperty.WEBSTATUS, DbType.String, ParameterDirection.Input);

            Ope.AddParams("PARTY_ID", pClsProperty.MULTYPARTY_ID, DbType.String, ParameterDirection.Input);

            Ope.AddParams("SALEPARTY_ID", pClsProperty.SALESPARTY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SALEJANGADNO", pClsProperty.SALESJANGADNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PURCHASEJANGADNO", pClsProperty.PURCHASEJANGADNO, DbType.String, ParameterDirection.Input);

            Ope.AddParams("PAGENO", pClsProperty.PAGENO, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("PAGESIZE", pClsProperty.PAGESIZE, DbType.Int32, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Table", "Trn_LiveStockGetDataNew", CommandType.StoredProcedure);

            return DS;
        }

       

        public DataSet GetStoneDetailForMemoForm(LiveStockProperty pClsProperty)  // Used In Live Stock : 25-06-2019
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();

             Ope.AddParams("SHAPE_ID", pClsProperty.MULTYSHAPE_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLOR_ID", pClsProperty.MULTYCOLOR_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CLARITY_ID", pClsProperty.MULTYCLARITY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUT_ID", pClsProperty.MULTYCUT_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("POL_ID", pClsProperty.MULTYPOL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SYM_ID", pClsProperty.MULTYSYM_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FL_ID", pClsProperty.MULTYFL_ID, DbType.String, ParameterDirection.Input);

            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LABREPORTNO", pClsProperty.LABREPORTNO, DbType.String, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT1", pClsProperty.FROMCARAT, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT1", pClsProperty.TOCARAT, DbType.Decimal, ParameterDirection.Input);

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

            Ope.AddParams("STOCKTYPE", pClsProperty.STOCKTYPE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("WEBSTATUS", pClsProperty.WEBSTATUS, DbType.String, ParameterDirection.Input);

            Ope.AddParams("PARTY_ID", pClsProperty.MULTYPARTY_ID, DbType.String, ParameterDirection.Input);

            Ope.AddParams("DIAMONDTYPE", pClsProperty.DIAMONDTYPE, DbType.String, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Table", "Trn_LiveStockGetData_SaleDelivery", CommandType.StoredProcedure);

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

        public PurchaseProperty OpeninfStockNotExistsRecord_Save(string BoxSaveXml, PurchaseProperty pClsProperty)//Gunjan:07/07/2023
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("BOXSAVEXML", BoxSaveXml, DbType.String, ParameterDirection.Input);               
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Polish_OpeningStock_BoxInsert", CommandType.StoredProcedure);

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
        }//End as Gunjan:07/07/2023

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

            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("STOCK_ID", StrXml, DbType.Xml, ParameterDirection.Input);
            Ope.AddParams("WEBSTATUS", WebStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STOCKTYPE", StockType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FORMAT", FormatName, DbType.String, ParameterDirection.Input);

            Ope.AddParams("SHAPE_ID", pClsProperty.MULTYSHAPE_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLOR_ID", pClsProperty.MULTYCOLOR_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CLARITY_ID", pClsProperty.MULTYCLARITY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUT_ID", pClsProperty.MULTYCUT_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("POL_ID", pClsProperty.MULTYPOL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FL_ID", pClsProperty.MULTYFL_ID, DbType.String, ParameterDirection.Input);
          
            Ope.AddParams("BOX_ID", pClsProperty.BOX_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LABREPORTNO", pClsProperty.LABREPORTNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SERIALNO", pClsProperty.SERIALNO, DbType.String, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT", pClsProperty.FROMCARAT, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT", pClsProperty.TOCARAT, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("PARTY_ID", pClsProperty.MULTYPARTY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LOCATION_ID", pClsProperty.LOCATION_ID , DbType.String, ParameterDirection.Input);
            Ope.AddParams("DISCOUNTGP", pClsProperty.DISCOUNTGP, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("PERCARATGP", pClsProperty.PERCARATGP, DbType.Decimal, ParameterDirection.Input);

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
        public DataTable GetPriceParameterGetData(LiveStockProperty pClsProperty, bool pISCalcAvgPrice,string StrType,Boolean IsAvailable) //#P : 16-03-2020
        {
            Ope.ClearParams();
            DataTable DT = new DataTable();
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", pClsProperty.MULTYSHAPE_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLOR_ID", pClsProperty.MULTYCOLOR_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CLARITY_ID", pClsProperty.MULTYCLARITY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUT_ID", pClsProperty.MULTYCUT_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("POL_ID", pClsProperty.MULTYPOL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SYM_ID", pClsProperty.MULTYSYM_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FL_ID", pClsProperty.MULTYFL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ISCALCAVGPRICE", pISCalcAvgPrice, DbType.Boolean, ParameterDirection.Input);
            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LABJANGEDNO", pClsProperty.LABJANGEDNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("DIAMONDTYPE", StrType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ISAVAILABLE", IsAvailable, DbType.Boolean, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT", pClsProperty.FROMCARAT, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT", pClsProperty.TOCARAT, DbType.Decimal, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "Trn_PriceParameterGetData", CommandType.StoredProcedure);
            return DT;
        }
        public MemoEntryProperty SaveParameterOrPriceUpdateDetail(string StrPriceParameteDetXML, bool ISUPDATEWITHAVAILABLE) //Add : Pinali : 28-07-2019
        {
            MemoEntryProperty pClsProperty = new MemoEntryProperty();
            try
            {
                Ope.ClearParams();
                DataTable DTab = new DataTable();

                Ope.AddParams("XMLDETSTR", StrPriceParameteDetXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ISUPDATEWITHAVAILABLE", ISUPDATEWITHAVAILABLE, DbType.Boolean, ParameterDirection.Input);

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
            try
            {
                Ope.ClearParams();
                Ope.AddParams("Employee_ID", Config.gEmployeeProperty.LEDGER_ID, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("FormName", pStrFormName, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("GridName", pStrGridName, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("GridLayout", pStrGridLayout, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("EntryIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("AccType", "", DbType.String, ParameterDirection.Input);

                return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "MST_GridLayoutSave", CommandType.StoredProcedure);

            }
            catch (System.Exception ex)
            {
                return -1;
            }

        }

        public int SaveGridACCLayout(string pStrFormName, string pStrGridName, string pStrGridLayout, string AccType)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("Employee_ID", Config.gEmployeeProperty.LEDGER_ID, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("FormName", pStrFormName, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("GridName", pStrGridName, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("GridLayout", pStrGridLayout, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("EntryIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("AccType", AccType, DbType.String, ParameterDirection.Input);

                return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "MST_GridLayoutSave", CommandType.StoredProcedure);

            }
            catch (System.Exception ex)
            {
                return -1;
            }

        }


        //Added by Daksha on 5/05/2023
        public DataTable MFG_CheckDuplicateRecord(string MFGFileUploadXml, string pStrMFGDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MFGFileUploadXml", MFGFileUploadXml, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MFGDATE", Val.SqlDate(pStrMFGDate), DbType.Date, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_ReceiveFromMFG_CheckDuplicateRecord", CommandType.StoredProcedure);
            return DTab;
        }
        //End as Daksha

        public DataTable Save(string MFGFileUploadXml, string pStrMFGDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MFGFileUploadXml", MFGFileUploadXml, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MFGDATE", Val.SqlDate(pStrMFGDate), DbType.Date, ParameterDirection.Input);
            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_ReceiveFromMFG_FileUpload", CommandType.StoredProcedure);
            return DTab;
        }

        public int DeleteGridLayout(string pStrFormName, string pStrGridName)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("Employee_ID", Config.gEmployeeProperty.LEDGER_ID, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("FormName", pStrFormName, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("GridName", pStrGridName, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("AccType", "", DbType.String, ParameterDirection.Input);

                return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "MST_GridLayoutDelete", CommandType.StoredProcedure);

            }
            catch (System.Exception ex)
            {
                return -1;
            }

        }

        public int DeleteAccGridLayout(string pStrFormName, string pStrGridName, string pstrAccType)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("Employee_ID", Config.gEmployeeProperty.LEDGER_ID, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("FormName", pStrFormName, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("GridName", pStrGridName, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("AccType", pstrAccType, DbType.String, ParameterDirection.Input);

                return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "MST_GridLayoutDelete", CommandType.StoredProcedure);

            }
            catch (System.Exception ex)
            {
                return -1;
            }

        }

        public string GetGridLayout(string pStrFormName, string pStrGridName)
        {
            Ope.ClearParams();

            string StrQuery = " And Employee_ID = '" + Config.gEmployeeProperty.LEDGER_ID + "' AND FormName = '" + pStrFormName + "' And GridName = '" + pStrGridName + "'";

            return Ope.FindText(Config.ConnectionString, Config.ProviderName, "MST_GridLayout", "GridLayout", StrQuery);

        }

        public string GetGridAccLayout(string pStrFormName, string pStrGridName, string pstrAccType)
        {
            Ope.ClearParams();

            string StrQuery = " And Employee_ID = '" + Config.gEmployeeProperty.LEDGER_ID + "' AND FormName = '" + pStrFormName + "' And GridName = '" + pStrGridName + "' And ISNULL(AccType,'') = '" + pstrAccType + "'";

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

        public DataSet GetLiveStockDataNew(LiveStockProperty pClsProperty, string pStrStockType)  // Used In Live Stock : 25-06-2019
        {
            DataSet DS = new DataSet();

            Ope.ClearParams();

            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
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
            Ope.AddParams("SIZE_ID", pClsProperty.MULTYSIZE_ID, DbType.String, ParameterDirection.Input);

            Ope.AddParams("TABLEBLACK_ID", pClsProperty.MULTYTABLEBLACK_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SIDEBLACK_ID", pClsProperty.MULTYSIDEBLACK_ID, DbType.String, ParameterDirection.Input);

            Ope.AddParams("WEBSTATUS", pClsProperty.WEBSTATUS, DbType.String, ParameterDirection.Input);

            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LABREPORTNO", pClsProperty.LABREPORTNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SERIALNO", pClsProperty.SERIALNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMONO", pClsProperty.MEMONO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MFG_ID", pClsProperty.MFG_ID, DbType.String, ParameterDirection.Input); //Added by Daksha on 27/02/2023

            Ope.AddParams("FROMCARAT", pClsProperty.FROMCARAT, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT", pClsProperty.TOCARAT, DbType.Decimal, ParameterDirection.Input);

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
            Ope.AddParams("STOCKTYPE", pStrStockType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("KAPAN_ID", pClsProperty.MULTYKAPAN, DbType.String, ParameterDirection.Input);

            Ope.AddParams("LABISSUEFROMDATE", pClsProperty.LABISSUEFROMDATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("LABISSUETODATE", pClsProperty.LABISSUETODATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("LABRESULTFROMDATE", pClsProperty.LABRESULTFROMDATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("LABRESULTTODATE", pClsProperty.LABRESULTTODATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("LABRETURNFROMDATE", pClsProperty.LABRETURNFROMDATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("LABRETURNTODATE", pClsProperty.LABRETURNTODATE, DbType.Date, ParameterDirection.Input);

            Ope.AddParams("SALESFROMDATE", pClsProperty.SALESFROMDATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("SALESTODATE", pClsProperty.SALESTODATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("AVAILBLEFROMDATE", pClsProperty.AVAILBLEFROMDATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("AVAILBLETODATE", pClsProperty.AVAILBLETODATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("DELIVERYFROMDATE", pClsProperty.DELIVERYFROMDATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("DELIVERYTODATE", pClsProperty.DELIVERYTODATE, DbType.Date, ParameterDirection.Input);

            Ope.AddParams("UPLOADFROMDATE", pClsProperty.UPLOADFROMDATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("UPLOADTODATE", pClsProperty.UPLOADTODATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("COLORSHADE_ID", pClsProperty.MULTYCOLORSHADE_ID, DbType.String, ParameterDirection.Input);

            Ope.AddParams("PRICEREVICEDFROMDATE", pClsProperty.PRICEREVICEDFROMDATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("PRICEREVICEDTODATE", pClsProperty.PRICEREVICEDTODATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("ISSALEDELIVERY", pClsProperty.ISSALEDELIVERY, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("ISHKSTONEDATA", pClsProperty.ISHKSTONEDATA, DbType.Int32, ParameterDirection.Input);

            Ope.AddParams("DIAMONDTYPE", pClsProperty.DIAMONDTYPE, DbType.String, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Trn_LiveStockGetData", CommandType.StoredProcedure);
            //Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Trn_LiveStockGetData_Test", CommandType.StoredProcedure);

            return DS;
        }

      

        public DataSet GetMumbaiLivestockData(LiveStockProperty pClsProperty, string pStrStockType)  // Used In Live Stock : 25-06-2019
        {
            DataSet DS = new DataSet();

            Ope.ClearParams();
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);           
            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);            
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Trn_MumbaiLiveStockGetData", CommandType.StoredProcedure);

            return DS;
        }

        public DataSet GetNewLiveStockDataNew(LiveStockProperty pClsProperty, string pStrStockType , string StockType, bool StrLabStock)  // Used In Live Stock : 25-06-2019
        {
            DataSet DS = new DataSet();

            Ope.ClearParams();

            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", pClsProperty.MULTYSHAPE_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLOR_ID", pClsProperty.MULTYCOLOR_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CLARITY_ID", pClsProperty.MULTYCLARITY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUT_ID", pClsProperty.MULTYCUT_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("POL_ID", pClsProperty.MULTYPOL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SYM_ID", pClsProperty.MULTYSYM_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FL_ID", pClsProperty.MULTYFL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LAB_ID", pClsProperty.MULTYLAB_ID, DbType.String, ParameterDirection.Input);

            Ope.AddParams("WEBSTATUS", pClsProperty.WEBSTATUS, DbType.String, ParameterDirection.Input);

            Ope.AddParams("BARCODE", pClsProperty.BARCODE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("DIAMONDTYPE", pClsProperty.DIAMONDTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ISUNGRADEDTOMIX", pClsProperty.ISUNGRADEDTOMIX, DbType.Boolean, ParameterDirection.Input);

            Ope.AddParams("STOCKTYPENEW", StockType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LABSTOCK", StrLabStock, DbType.Boolean, ParameterDirection.Input);
            Ope.AddParams("EMPLOYEE_ID", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);


            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Trn_LiveStockGetDataNew", CommandType.StoredProcedure);

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
      
        public DataTable BulkPropertUpdate(string pStrStoneType, string pStrParaType, string pStrStoneNoXML, string pStrValueXML, Guid pStrParty_ID, Guid pStrBroker_ID, string pStrComment) //Chng : Dhara
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("STONETYPE", pStrStoneType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PARATYPE", pStrParaType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STONENOXML", pStrStoneNoXML, DbType.Xml, ParameterDirection.Input);
            Ope.AddParams("VALUEXML", pStrValueXML, DbType.Xml, ParameterDirection.Input);

            Ope.AddParams("PARTY_ID", pStrParty_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("BROKER_ID", pStrBroker_ID, DbType.Guid, ParameterDirection.Input);
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
        public DataTable GetDataForStockBarcodePrintWithRFID(string pStoneNo, string pStoneRFIDNo) //Used In Barcode Print : Change: #Nikita : 03-02-2022
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            //Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
            //Ope.AddParams("LABREPORTNO", pClsProperty.LABREPORTNO, DbType.String, ParameterDirection.Input);
            //Ope.AddParams("SERIALNO", pClsProperty.SERIALNO, DbType.String, ParameterDirection.Input);
            //Ope.AddParams("MEMONO", pClsProperty.MEMONO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STOCKTYPE", "SINGLE", DbType.String, ParameterDirection.Input);
            Ope.AddParams("STONENO", pStoneNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STONERFIDNO", pStoneRFIDNo, DbType.String, ParameterDirection.Input);

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

        //public DataTable GetPartyStockNofromGIAControlNo(string pStrGIAControlNo) //#K : 10-11-2020
        //{
        //    Ope.ClearParams();
        //    DataTable DTab = new DataTable();
        //    Ope.AddParams("GIACONTROLNO", pStrGIAControlNo, DbType.String, ParameterDirection.Input);
        //    Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_StockGetPartyStockNoFromGIAControlNo", CommandType.StoredProcedure);
        //    return DTab;
        //}

        public DataSet GetPartyStockNofromRFIDTagNo(string pStrXmlRFIDTagNo, string pStrOpe) //#P : 11-01-2022
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("XMLRFIDTAGNO", pStrXmlRFIDTagNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Table", "TRN_StockGetPartyStockNoFromRFIDTagNo", CommandType.StoredProcedure);
            return DS;
        }

        public LiveStockProperty UpdateRapnetUpload(LiveStockProperty pClsProperty, Int32 pStrChkState) //WITHOUTSTOCK OR WITHSTOCK
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("STOCK_ID", pClsProperty.STOCK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("RAPNETUPLOAD", pStrChkState, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_LiveStockUpdateRAPNETUPLOAD", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "FAIL";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = "FAIL";

            }
            return pClsProperty;
        }

        public SingleStockUpdateProperty UpdateSingleStock(SingleStockUpdateProperty pClsroperty) // D: 08-01-2021
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("STOCK_ID", pClsroperty.STOCK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SHAPE_ID", pClsroperty.SHAPE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("COLOR_ID", pClsroperty.COLOR_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("CARAT", pClsroperty.CARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("CLARITY_ID", pClsroperty.CLARITY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("CUT_ID", pClsroperty.CUT_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("POL_ID", pClsroperty.POL_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SYM_ID", pClsroperty.SYM_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("FL_ID", pClsroperty.FL_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("FLUORESCENCECOLOR", pClsroperty.FLCOLOR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COLORSHADE_ID", pClsroperty.COLORSHADE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("LOCATION_ID", pClsroperty.LOCATION_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SIZE_ID", pClsroperty.SIZE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("LAB_ID", pClsroperty.LAB_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("BOX_ID", pClsroperty.BOX_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("LABREPORTNO", pClsroperty.LABREPORTNO, DbType.String, ParameterDirection.Input);

                Ope.AddParams("MFGCOLOR_ID", pClsroperty.MFGCOLOR_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MFGCLARITY_ID", pClsroperty.MFGCLARITY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MFGCUT_ID", pClsroperty.MFGCUT_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MFGPOL_ID", pClsroperty.MFGPOL_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MFGSYM_ID", pClsroperty.MFGSYM_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MFGFL_ID", pClsroperty.MFGFL_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("DIAMIN", pClsroperty.DIAMIN, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DIAMAX", pClsroperty.DIAMAX, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("HEIGHT", pClsroperty.HEIGHT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("LENGTH", pClsroperty.LENGTH, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("WIDTH", pClsroperty.WIDTH, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("RATIO", pClsroperty.RATIO, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DIAMETER", pClsroperty.DIAMETER, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("CRANGLE", pClsroperty.CRANGLE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("CRHEIGHT", pClsroperty.CRHEIGHT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("PAVANGLE", pClsroperty.PAVANGLE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("PAVHEIGHT", pClsroperty.PAVHEIGHT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TABLEPER", pClsroperty.TABLEPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DEPTHPER", pClsroperty.DEPTHPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GIRDLEPER", pClsroperty.GIRDLEPER, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("TABLEINC_ID", pClsroperty.TABLEINC_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TABLEOPENINC_ID", pClsroperty.TABLEOPENINC_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SIDETABLEINC_ID", pClsroperty.SIDETABLEINC_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SIDEOPENINC_ID", pClsroperty.SIDEOPENINC_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TABLEBLACKINC_ID", pClsroperty.TABLEBLACKINC_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SIDEBLACKINC_ID", pClsroperty.SIDEBLACKINC_ID, DbType.Int32, ParameterDirection.Input);
                // Forgot By Dhara
                Ope.AddParams("REDSPORTINC_ID", pClsroperty.REDSPORTINC_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("MILKY_ID", pClsroperty.MILKY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("GIRDLE_ID", pClsroperty.GIRDLE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("FROMGIRDLE_ID", pClsroperty.FROMGIRDLE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TOGIRDLE_ID", pClsroperty.TOGIRDLE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("CULET_ID", pClsroperty.CULET_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("LUSTER_ID", pClsroperty.LUSTER_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EYECLEAN_ID", pClsroperty.EYECLEAN_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("HA_ID", pClsroperty.HA_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("STARLENGTH", pClsroperty.STARLENGTH, DbType.String, ParameterDirection.Input);
                Ope.AddParams("LOWERHALF", pClsroperty.LOWERHALF, DbType.String, ParameterDirection.Input);

                Ope.AddParams("UPLOADDATE", pClsroperty.UPLOADDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("LABISSUEDATE", pClsroperty.LABISSUEDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("LABRESULTDATE", pClsroperty.LABRESULTDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("LABRETURNDATE", pClsroperty.LABRETURNDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("PRICEREVISEDATE", pClsroperty.PRICEREVISEDDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("AVAILABLEDATE", pClsroperty.AVAILABLEDATE, DbType.Date, ParameterDirection.Input);

                Ope.AddParams("ISNOBLACK", pClsroperty.ISBLACK, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("ISNOBGM", pClsroperty.ISNOBGM, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("ISEXCLUSIVE", pClsroperty.ISEXCLUSIVE, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("COLORDESC", pClsroperty.COLORDESC, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FANCYCOLOR", pClsroperty.FANCYCOLOR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FANCYCOLORINTENSITY", pClsroperty.FANCYCOLORINTENSITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FANCYCOLOROVERTONE", pClsroperty.FANCYCOLOROVERTONE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("KEYTOSYMBOL", pClsroperty.KEYTOSYMBOL, DbType.String, ParameterDirection.Input);
                Ope.AddParams("REPORTCOMMENT", pClsroperty.REPORTCOMMENT, DbType.String, ParameterDirection.Input);

                Ope.AddParams("GIRDLECONDITION", pClsroperty.GIRDLECONDITION, DbType.String, ParameterDirection.Input);
                Ope.AddParams("GIRDLEDESC", pClsroperty.GIRDLEDESC, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PAINTING", pClsroperty.PAINTING, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PROPORTIONS", pClsroperty.PROPORTIONS, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PAINTCOMM", pClsroperty.PAINTCOMM, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SYNTHETICINDICATOR", pClsroperty.SYNTHETICINDICATOR, DbType.String, ParameterDirection.Input);

                Ope.AddParams("REMARK", pClsroperty.REMARK, DbType.String, ParameterDirection.Input);
                Ope.AddParams("INSCRIPTION", pClsroperty.INSCRIPTION, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CLIENTCOMMENT", pClsroperty.CLIENTCOMMENT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("POLISHFEATURES", pClsroperty.POLISHFEATURES, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SYMMETRYFEATURES", pClsroperty.SYMMETRYFEATURES, DbType.String, ParameterDirection.Input);

                Ope.AddParams("MFGRAPAPORT", pClsroperty.MFGRAPAPORT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("MFGDISCOUNT", pClsroperty.MFGDISCOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("MFGPRICEPERCARAT", pClsroperty.MFGPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("MFGAMOUNT", pClsroperty.MFGAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("COSTRAPAPORT", pClsroperty.COSTRAPAPORT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("COSTDISCOUNT", pClsroperty.COSTDISCOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("COSTPRICEPERCARAT", pClsroperty.COSTPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("COSTAMOUNT", pClsroperty.COSTAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("SALERAPAPORT", pClsroperty.SALERAPAPORT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SALEDISCOUNT", pClsroperty.SALEDISCOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SALEPRICEPERCARAT", pClsroperty.SALEPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SALEAMOUNT", pClsroperty.SALEAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("EXPRAPAPORT", pClsroperty.EXPRAPAPORT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EXPDISCOUNT", pClsroperty.EXPDISCOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EXPPRICEPERCARAT", pClsroperty.EXPPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EXPAMOUNT", pClsroperty.EXPAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("RAPNETRAPAPORT", pClsroperty.RAPNETRAPAPORT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("RAPNETDISCOUNT", pClsroperty.RAPNETDISCOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("RAPNETPRICEPERCARAT", pClsroperty.RAPNETPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("RAPNETAMOUNT", pClsroperty.RAPNETAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("JAMESALLENRAPAPORT", pClsroperty.JAMESALLENRAPAPORT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("JAMESALLENDISCOUNT", pClsroperty.JAMESALLENDISCOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("JAMESALLENPRICEPERCARAT", pClsroperty.JAMESALLENPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("JAMESALLENAMOUNT", pClsroperty.JAMESALLENAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("COMPRAPAPORT", pClsroperty.COMPRAPAPORT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("COMPDISCOUNT", pClsroperty.COMPDISCOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("COMPPRICEPERCARAT", pClsroperty.COMPPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("COMPAMOUNT", pClsroperty.COMPAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("PROCESS_ID", pClsroperty.PROCESS_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("PROCESSNAME", pClsroperty.PROCESSNAME, DbType.String, ParameterDirection.Input);

                Ope.AddParams("MEASUREMENT", pClsroperty.MEASUREMENT, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_SingleStockUpdate", CommandType.StoredProcedure);
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


        public SingleStockUpdateProperty MixToSingleSave(SingleStockUpdateProperty pClsroperty) // D: 27-01-2021
        {
            try
            {
                Ope.ClearParams();

                Ope.AddParams("STOCK_ID", pClsroperty.STOCK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PARCELSTOCK_ID", pClsroperty.PARCELSTOCK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("STOCKNO", pClsroperty.STOCKNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PARCELSTOCKNO", pClsroperty.PARCELSTOCKNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("KAPANNAME", pClsroperty.KAPANNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PACKETNO", pClsroperty.PACKETNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TAG", pClsroperty.TAG, DbType.String, ParameterDirection.Input);

                Ope.AddParams("SHAPE_ID", pClsroperty.SHAPE_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("COLOR_ID", pClsroperty.COLOR_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("CARAT", pClsroperty.CARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("CLARITY_ID", pClsroperty.CLARITY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("CUT_ID", pClsroperty.CUT_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("POL_ID", pClsroperty.POL_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SYM_ID", pClsroperty.SYM_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("FL_ID", pClsroperty.FL_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("FLUORESCENCECOLOR", pClsroperty.FLCOLOR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COLORSHADE_ID", pClsroperty.COLORSHADE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("LOCATION_ID", pClsroperty.LOCATION_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SIZE_ID", pClsroperty.SIZE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("LAB_ID", pClsroperty.LAB_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("BOX_ID", pClsroperty.BOX_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("LABREPORTNO", pClsroperty.LABREPORTNO, DbType.String, ParameterDirection.Input);

                Ope.AddParams("MFGCOLOR_ID", pClsroperty.MFGCOLOR_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MFGCLARITY_ID", pClsroperty.MFGCLARITY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MFGCUT_ID", pClsroperty.MFGCUT_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MFGPOL_ID", pClsroperty.MFGPOL_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MFGSYM_ID", pClsroperty.MFGSYM_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MFGFL_ID", pClsroperty.MFGFL_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("DIAMIN", pClsroperty.DIAMIN, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DIAMAX", pClsroperty.DIAMAX, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("HEIGHT", pClsroperty.HEIGHT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("LENGTH", pClsroperty.LENGTH, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("WIDTH", pClsroperty.WIDTH, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("RATIO", pClsroperty.RATIO, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DIAMETER", pClsroperty.DIAMETER, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("CRANGLE", pClsroperty.CRANGLE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("CRHEIGHT", pClsroperty.CRHEIGHT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("PAVANGLE", pClsroperty.PAVANGLE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("PAVHEIGHT", pClsroperty.PAVHEIGHT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TABLEPER", pClsroperty.TABLEPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DEPTHPER", pClsroperty.DEPTHPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GIRDLEPER", pClsroperty.GIRDLEPER, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("TABLEINC_ID", pClsroperty.TABLEINC_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TABLEOPENINC_ID", pClsroperty.TABLEOPENINC_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SIDETABLEINC_ID", pClsroperty.SIDETABLEINC_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SIDEOPENINC_ID", pClsroperty.SIDEOPENINC_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TABLEBLACKINC_ID", pClsroperty.TABLEBLACKINC_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SIDEBLACKINC_ID", pClsroperty.SIDEBLACKINC_ID, DbType.Int32, ParameterDirection.Input);
                // Forgot By Dhara
                Ope.AddParams("REDSPORTINC_ID", pClsroperty.REDSPORTINC_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("MILKY_ID", pClsroperty.MILKY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("GIRDLE_ID", pClsroperty.GIRDLE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("FROMGIRDLE_ID", pClsroperty.FROMGIRDLE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TOGIRDLE_ID", pClsroperty.TOGIRDLE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("CULET_ID", pClsroperty.CULET_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("LUSTER_ID", pClsroperty.LUSTER_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EYECLEAN_ID", pClsroperty.EYECLEAN_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("HA_ID", pClsroperty.HA_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("STARLENGTH", pClsroperty.STARLENGTH, DbType.String, ParameterDirection.Input);
                Ope.AddParams("LOWERHALF", pClsroperty.LOWERHALF, DbType.String, ParameterDirection.Input);

                Ope.AddParams("UPLOADDATE", pClsroperty.UPLOADDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("LABISSUEDATE", pClsroperty.LABISSUEDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("LABRESULTDATE", pClsroperty.LABRESULTDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("LABRETURNDATE", pClsroperty.LABRETURNDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("PRICEREVISEDATE", pClsroperty.PRICEREVISEDDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("AVAILABLEDATE", pClsroperty.AVAILABLEDATE, DbType.Date, ParameterDirection.Input);

                Ope.AddParams("ISNOBLACK", pClsroperty.ISBLACK, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("ISNOBGM", pClsroperty.ISNOBGM, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("ISEXCLUSIVE", pClsroperty.ISEXCLUSIVE, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("COLORDESC", pClsroperty.COLORDESC, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FANCYCOLOR", pClsroperty.FANCYCOLOR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FANCYCOLORINTENSITY", pClsroperty.FANCYCOLORINTENSITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FANCYCOLOROVERTONE", pClsroperty.FANCYCOLOROVERTONE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("KEYTOSYMBOL", pClsroperty.KEYTOSYMBOL, DbType.String, ParameterDirection.Input);
                Ope.AddParams("REPORTCOMMENT", pClsroperty.REPORTCOMMENT, DbType.String, ParameterDirection.Input);

                Ope.AddParams("GIRDLECONDITION", pClsroperty.GIRDLECONDITION, DbType.String, ParameterDirection.Input);
                Ope.AddParams("GIRDLEDESC", pClsroperty.GIRDLEDESC, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PAINTING", pClsroperty.PAINTING, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PROPORTIONS", pClsroperty.PROPORTIONS, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PAINTCOMM", pClsroperty.PAINTCOMM, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SYNTHETICINDICATOR", pClsroperty.SYNTHETICINDICATOR, DbType.String, ParameterDirection.Input);

                Ope.AddParams("REMARK", pClsroperty.REMARK, DbType.String, ParameterDirection.Input);
                Ope.AddParams("INSCRIPTION", pClsroperty.INSCRIPTION, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CLIENTCOMMENT", pClsroperty.CLIENTCOMMENT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("POLISHFEATURES", pClsroperty.POLISHFEATURES, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SYMMETRYFEATURES", pClsroperty.SYMMETRYFEATURES, DbType.String, ParameterDirection.Input);

                Ope.AddParams("MFGRAPAPORT", pClsroperty.MFGRAPAPORT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("MFGDISCOUNT", pClsroperty.MFGDISCOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("MFGPRICEPERCARAT", pClsroperty.MFGPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("MFGAMOUNT", pClsroperty.MFGAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("COSTRAPAPORT", pClsroperty.COSTRAPAPORT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("COSTDISCOUNT", pClsroperty.COSTDISCOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("COSTPRICEPERCARAT", pClsroperty.COSTPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("COSTAMOUNT", pClsroperty.COSTAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("SALERAPAPORT", pClsroperty.SALERAPAPORT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SALEDISCOUNT", pClsroperty.SALEDISCOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SALEPRICEPERCARAT", pClsroperty.SALEPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SALEAMOUNT", pClsroperty.SALEAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("EXPRAPAPORT", pClsroperty.EXPRAPAPORT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EXPDISCOUNT", pClsroperty.EXPDISCOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EXPPRICEPERCARAT", pClsroperty.EXPPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EXPAMOUNT", pClsroperty.EXPAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("RAPNETRAPAPORT", pClsroperty.RAPNETRAPAPORT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("RAPNETDISCOUNT", pClsroperty.RAPNETDISCOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("RAPNETPRICEPERCARAT", pClsroperty.RAPNETPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("RAPNETAMOUNT", pClsroperty.RAPNETAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("JAMESALLENRAPAPORT", pClsroperty.JAMESALLENRAPAPORT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("JAMESALLENDISCOUNT", pClsroperty.JAMESALLENDISCOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("JAMESALLENPRICEPERCARAT", pClsroperty.JAMESALLENPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("JAMESALLENAMOUNT", pClsroperty.JAMESALLENAMOUNT, DbType.Double, ParameterDirection.Input);


                Ope.AddParams("PARCELBALANCECARAT", pClsroperty.PARCELCARAT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("PROCESS_ID", pClsroperty.PROCESS_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("PROCESSNAME", pClsroperty.PROCESSNAME, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYTYPE", pClsroperty.EntryType, DbType.String, ParameterDirection.Input);

                Ope.AddParams("LOT_ID", pClsroperty.Lot_ID, DbType.Guid, ParameterDirection.Input); // Add Khushbu 29-06-21
                Ope.AddParams("LOTNAME", pClsroperty.LotName, DbType.String, ParameterDirection.Input); // Add Khushbu 29-06-21
                Ope.AddParams("MFG_ID", pClsroperty.MFGID, DbType.String, ParameterDirection.Input);// Add Khushbu 29-06-21

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_MixToSingleTransferInsertUpdate", CommandType.StoredProcedure);
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

        public DataSet GetDataForWebActivity(string pStrFromdate, string pStrToDate, string pStrParty_ID)
        {

            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("FROMDATE", pStrFromdate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("PARTY_ID", pStrParty_ID, DbType.String, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "TEMP", "CRM_WebActivityGetData", CommandType.StoredProcedure);

            return DS;

        }

        public DataSet GetDataForWebActivitySummury(string pStrFromdate, string pStrToDate, string pStrParty_ID)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("FROMDATE", pStrFromdate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("PARTY_ID", pStrParty_ID, DbType.String, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "TEMP", "CRM_WebActivitySummuryGetData", CommandType.StoredProcedure);

            return DS;
        }

        public DataSet GetLatestPurchaseData(string pStrProcessType, string pStrStockType) // D: For Get Dashboard Data
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("PROCESSTYPE", pStrProcessType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STOCKTYPE", pStrStockType, DbType.String, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp1", "CRM_LatestPurchaseGetData", CommandType.StoredProcedure);
            return DS;
        }

        public DataSet GetDataForDemandView(string pStrParty_ID)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("PARTY_ID", pStrParty_ID, DbType.String, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "TEMP", "Crm_CustomerDemandGetData", CommandType.StoredProcedure);

            return DS;
        }
      
        public DataTable GetDataFroAutoMailDateTime(Guid pGuidParty_ID)
        {
            Ope.ClearParams();
            DataTable DS = new DataTable();

            Ope.AddParams("PARTY_ID", pGuidParty_ID, DbType.Guid, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DS, "CRM_AutoMailDayandTimeGetData", CommandType.StoredProcedure);

            return DS;
        }

        public DataSet GetDataForExcelExportEmailSettingNew(CRMCustomerAutoMailCriteriaProperty pClsProperty) //WITHOUTSTOCK OR WITHSTOCK
        {
            Ope.ClearParams();
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
            Ope.AddParams("WEBSTATUSNAME", pClsProperty.WEBSTATUSNAME, DbType.String, ParameterDirection.Input);

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

            Ope.AddParams("PARTY_ID", pClsProperty.CUSTOMER_ID, DbType.String, ParameterDirection.Input);


            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Trn_EmailSettingExportExcelModified", CommandType.StoredProcedure);

            return DS;
        }

        public DataTable GetStonePriceExcelWiseGetData(string pStrStonePriceXml, string pStrSearchType) //#P : 26-04-2021 : StonePriceUpload
        {
            Ope.ClearParams();
            DataTable DT = new DataTable();
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("XMLFORSTONEPRICE", pStrStonePriceXml, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SEARCHTYPE", pStrSearchType, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "Trn_PriceParameterFileUpload", CommandType.StoredProcedure);
            return DT;
        }

        public Trn_SinglePrdProperty MFGGradingSave(Trn_SinglePrdProperty pClsProperty) //Add : Pinali : 28-07-2019
        {
            try
            {
                Ope.ClearParams();
                DataTable DTab = new DataTable();

                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("MFGGRADING_NO", pClsProperty.MFGGradingNo, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", pClsProperty.XMLDETSTR, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_MFGGradingSave", CommandType.StoredProcedure);
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
        public DataTable MFGGradingSaveForStock(Trn_SinglePrdProperty pClsProperty) //K : 
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("MFGGRADING_NO", pClsProperty.MFGGradingNo, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("XMLDETSTR", pClsProperty.XMLDETSTR, DbType.Xml, ParameterDirection.Input);

            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
            Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_MFGGradingSave", CommandType.StoredProcedure);

            return DTab;

        }


        public DataTable MFGGradingGetDetail(Trn_SinglePrdProperty pClsProperty) //#P : 16-03-2020
        {
            Ope.ClearParams();
            DataTable DT = new DataTable();
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("MFGGRADINGNO", pClsProperty.MFGGradingNo, DbType.Int64, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "Trn_MFGGradingGetDetail", CommandType.StoredProcedure);
            return DT;
        }

        public DataTable MFGGradingLiveStockGetDetail(Trn_SinglePrdProperty pClsProperty, string pStrFromDate, string pStrToDate) //#P : 16-03-2020
        {
            Ope.ClearParams();
            DataTable DT = new DataTable();
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("MFGGRADINGNO", pClsProperty.MFGGradingNo, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("STATUS", pClsProperty.MfgGradingStatus, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("STOCKNO", pClsProperty.StockNo, DbType.String, ParameterDirection.Input);  //HINAL 02-01-2022
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "Trn_MFGGradingLiveStockGetDetail", CommandType.StoredProcedure);
            return DT;
        }

        public DataTable MFGGradingGetSummary(string pStrFromDate, string pStrToDate, string pStrStoneNo) //#P : 16-03-2020
        {
            Ope.ClearParams();
            DataTable DT = new DataTable();
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("STONENO", pStrStoneNo, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "Trn_MFGGradingGetSummary", CommandType.StoredProcedure);
            return DT;
        }

        public Trn_SinglePrdProperty MFGGradingTransferToStock(Trn_SinglePrdProperty pClsProperty , string StrOpe) //Add : Pinali : 28-07-2019
        {
            try
            {
                Ope.ClearParams();
                DataTable DTab = new DataTable();

                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", pClsProperty.XMLDETSTR, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("OPE", StrOpe, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_MFGGradingLiveStockTransfer", CommandType.StoredProcedure);
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

        public DataTable GetMixToSingleStockData() // 23-06-2021 Khushbu
        {
            Ope.ClearParams();
            DataTable DT = new DataTable();
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "Trn_MixToSingleGetData", CommandType.StoredProcedure);
            return DT;
        }
        public DataSet MFGGradingGetDetail_Export(Trn_SinglePrdProperty pClsProperty) //#P : 16-03-2020
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("MFGGRADINGNO", pClsProperty.MFGGradingNo, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("STOCKNO", pClsProperty.StockNo, DbType.String, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Trn_MFGGradingGetDetailExport", CommandType.StoredProcedure);

            return DS;
        }
        public DataSet MISHeatMapGetData(
           string StrReportType,
           string StrOpe,
           string StrShape,
           string StrSize,
           string StrCut,
           string StrPol,
           string StrSym,
           string StrFL,
           string StrFromDate,
           string StrToDate

           )
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("ReportType", StrReportType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Ope", StrOpe, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Shape", StrShape, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Size", StrSize, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Cut", StrCut, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Pol", StrPol, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Sym", StrSym, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FL", StrFL, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FromDate", StrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("ToDate", StrToDate, DbType.Date, ParameterDirection.Input);


            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "MIS_StockHeatMap", CommandType.StoredProcedure);
            return DS;
        }
        public DataTable CheckStockNo(string StockNo) //hinal : 13-01-2022
        {
            DataTable Dtab = new DataTable();

            Ope.ClearParams();
            string qry = "SELECT PARTYSTOCKNO FROM TRN_SinglePrd WITH(NOLOCK) WHERE PartyStockNo='" + StockNo + "'";
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, Dtab, qry, CommandType.Text);
            return Dtab;
        }

        public DataSet GetIssueReturnStockData(LiveStockProperty pClsProperty)  // Khushbu 08-02-22
        {
            DataSet DS = new DataSet();

            Ope.ClearParams();

            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ISISSUESTOCK", pClsProperty.ISISSUESTOCK, DbType.Int32, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Trn_StockIssueReturnGetData", CommandType.StoredProcedure);

            return DS;
        }
        public LiveStockProperty StockIssueReturn(LiveStockProperty pClsProperty, string XMLDETSTR, string pStrEntryType) //Add : khushbu : 18-02-22
        {
            try
            {
                Ope.ClearParams();
                DataTable DTab = new DataTable();

                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EMPLOYEE_ID", pClsProperty.EMPLOYEE_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("CABIN_ID", pClsProperty.CABIN_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("DEPARTMENT_ID", pClsProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("PROCESS_ID", pClsProperty.PROCESS_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYTYPE", pStrEntryType, DbType.String, ParameterDirection.Input);
                Ope.AddParams("DETAILXML", XMLDETSTR, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_StockIssueReturnSave", CommandType.StoredProcedure);
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
        public Trn_SinglePrdProperty Delete(Trn_SinglePrdProperty pClsProperty) // Add Darshan : 26-03-2020
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("STOCK_ID", pClsProperty.STOCK_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_MFGGradingDelete", CommandType.StoredProcedure);
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

        public LiveStockProperty Delete(LiveStockProperty pClsProperty) 
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_SingleStoneDelete", CommandType.StoredProcedure);
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

        public DataSet GetDataForRoundReport(string pStrStockNo, string pStrKapanName) //ADD DARSHAN : 26-03-2020
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("STOCKNO", pStrStockNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("KAPANNAME", pStrKapanName, DbType.String, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "RP_LiveStockRoundReportExcel", CommandType.StoredProcedure);
            return DS;
        }
        public DataSet GetDataForSaleReport(string pStrStockNo, string pStrKapanName, string pStrBillingPartyID, string pStrFromDate, string pStrToDate, int pStrFromData, int pStrToData, string pStrTrnOldFromData, string pStrTrnOldToData, string pStrMemoNo, string pStrSellerID,string StrbrokerId="", string  StrHKStoneType="",string StrStaus="",int IntOrderStatus=-1) //ADD Dhara : 06-05-2022
        {

            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("STOCKNO", pStrStockNo, DbType.String, ParameterDirection.Input);
            //Ope.AddParams("KAPANNAME", pStrKapanName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGPARTY_ID", pStrBillingPartyID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMINVDATA", pStrFromData, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TOINVDATA", pStrToData, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TRNOLDFROMDATA", pStrTrnOldFromData, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TRNOLDTODATA", pStrTrnOldToData, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMONO", pStrMemoNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SELLER_ID", pStrSellerID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BROKER_ID", StrbrokerId, DbType.String, ParameterDirection.Input);
            Ope.AddParams("HKSTONETYPE", StrHKStoneType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STATUS", StrStaus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SALESORDERAPPROVAL", IntOrderStatus, DbType.Int32, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "TEMP", "RP_LiveStockSaleReportExcel", CommandType.StoredProcedure);
            return DS;
        }
        public LiveStockProperty UpdateNewArrival(string strNewArrivalXML) // Dhara : 18-06-2022 Update Available Date
        {
            LiveStockProperty pClsroperty = new LiveStockProperty();
            try
            {
                Ope.ClearParams();
                Ope.AddParams("XMLMASTER", strNewArrivalXML, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_LiveStockAddNewArrivalStoneUpdate", CommandType.StoredProcedure);
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
        public DataTable CheckKapan(string kapanName, string packetNo, string tag)
        {
            DataTable Dtab = new DataTable();

            Ope.ClearParams();
            //string qry = "SELECT KAPANNAME,PACKETNO,TAG FROM TRN_SinglePrd WITH(NOLOCK) WHERE KAPANNAME ='" + kapanName + "' AND PACKETNO='" + packetNo + "'AND TAG='" + tag + "'";
            string qry = "SELECT KAPANNAME,PACKETNO,TAG FROM TRN_SinglePrd WITH(NOLOCK) WHERE KAPANNAME ='" + kapanName + "' AND PACKETNO='" + packetNo + "'";
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, Dtab, qry, CommandType.Text);
            return Dtab;
        }

        //Added by Daksha on 23/01/2023
        public PriceHeadDetailProperty OfferPrice_Delete(PriceHeadDetailProperty pClsProperty)
        {
            try

            {
                Ope.ClearParams();
                Ope.AddParams("PRICE_ID", pClsProperty.PRICE_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_OfferPrice_Delete", CommandType.StoredProcedure);

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
        //End as Daksha

        public DataTable SaveVlaue(string strXMLValuesInsert, string pStrMFGDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MFGFileUploadXml", strXMLValuesInsert, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MFGDATE", Val.SqlDate(pStrMFGDate), DbType.Date, ParameterDirection.Input);
            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_ReceiveFromMFG_FileUpload", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetData(string pStrFromDate, string pStrToDate)
        {
            Ope.ClearParams();
            DataTable DT = new DataTable();
            Ope.AddParams("OPE", "SUMMARY", DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "Parcel_ReceiveFromMFG_GetData", CommandType.StoredProcedure);
            return DT;
        }

        public DataTable FillDetail(string pStrMfgDate)
        {
            Ope.ClearParams();
            DataTable DT = new DataTable();
            Ope.AddParams("OPE", "DETAIL", DbType.String, ParameterDirection.Input);
            Ope.AddParams("MFGDATE", Val.SqlDate(pStrMfgDate), DbType.Date, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "Parcel_ReceiveFromMFG_GetData", CommandType.StoredProcedure);
            return DT;
        }

        public DataTable FillDetail(string pStrFromDate, string pStrToDate)
        {
            Ope.ClearParams();
            DataTable DT = new DataTable();
            Ope.AddParams("OPE", "DETAIL", DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "Parcel_ReceiveFromMFG_GetData", CommandType.StoredProcedure);
            return DT;
        }

        public DataTable CheckExistsOrNotExists(string OpeningFileUploadXml)//Add By Gunjan:14/06/2023
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("OpeningFileUploadXml", OpeningFileUploadXml, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BoxMaster_CheckExistsRecord", CommandType.StoredProcedure);
            return DTab;
        }

        public PurchaseProperty OpeningStockSave(string openingStockSaveXml, string pStrUploadDate, PurchaseProperty pClsProperty)//Add By Gunjan:14/06/2023
        {
            try
            { 
                Ope.ClearParams();
                Ope.AddParams("StockSaveXml", openingStockSaveXml, DbType.String, ParameterDirection.Input);
                Ope.AddParams("UPLOADDATE", Val.SqlDate(pStrUploadDate), DbType.Date, ParameterDirection.Input);
                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Polish_OpeningStock_Insert", CommandType.StoredProcedure);

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

        public DataTable GetAssortmentData(string pStrOpe, string pStrFromDate, string pStrToDate,Guid UploadId)
        {
            Ope.ClearParams();
            DataTable DT = new DataTable();
            Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", Val.SqlDate(pStrFromDate), DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", Val.SqlDate(pStrToDate), DbType.Date, ParameterDirection.Input);
            Ope.AddParams("UPLOAD_ID", UploadId, DbType.Guid, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "Parcel_Assortment_GetData", CommandType.StoredProcedure);
            return DT;
        }
        public PurchaseProperty AssortmentUploadSave( Guid NewTrn_Id, string openingStockSaveXml, PurchaseProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("UPLOADFILENAME", pClsProperty.FILENAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PACKETNO", pClsProperty.PacketNo, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("StockSaveXml", openingStockSaveXml, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MIXPACKET_ID", pClsProperty.Packet_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("UPLOAD_ID", NewTrn_Id, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_Assortment_InsertUpdate", CommandType.StoredProcedure);

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

        public PurchaseProperty Assortment_Delete(PurchaseProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();               
                Ope.AddParams("UPLOAD_ID", pClsProperty.UPLOAD_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_Assortment_Delete", CommandType.StoredProcedure);

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
        public PurchaseProperty AssortmentApproved(String strOpe,PurchaseProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("OPE", strOpe, DbType.String, ParameterDirection.Input);
                Ope.AddParams("StockSaveXml", pClsProperty.XMLDETSTR, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_AssortmentApproveAndReject", CommandType.StoredProcedure);

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

        public DataTable GetAssortmentApproveData(string pStrOpe, Guid UploadId,string strStatus, string pStrFromDate, string pStrToDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
            Ope.AddParams("UPLOAD_ID", UploadId, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("STATUS", strStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_AssortmentApprove_GetData", CommandType.StoredProcedure);
            return DTab;
        }


        public DataTable GetStockDetail(Int32 pIntPrdType_ID, String pStrStockNo, Guid pGuidEmployee_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("PRDTYPE_ID", pIntPrdType_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("STOCKNO", pStrStockNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("EMPLOYEE_ID", pGuidEmployee_ID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_SingleStoneCreateAndUpdateGetData", CommandType.StoredProcedure);
            return DTab;
        }
        public DataTable GetBulkPredictionStockDetail(Int32 pIntPrdType_ID, String pStrStockNo, string pGuidEmployee_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("PRDTYPE_ID", pIntPrdType_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("STOCKNO", pStrStockNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("EMPLOYEE_ID", pGuidEmployee_ID, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_SingleStoneBulkPredictionGetData", CommandType.StoredProcedure);
            return DTab;
        }
        public DataTable GetManuallStockDetail( String pStrStockNo, string StrOPE)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("STOCKNO", pStrStockNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("OPE", StrOPE, DbType.Guid, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_SingleStoneManualPredictionUpdateGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public LiveStockProperty LiveStockUpdateLocation(LiveStockProperty pClsProperty, string strStock_ID, long intLocation_ID)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("STOCK_ID", strStock_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("LOCATION_ID", intLocation_ID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_LiveStockLocationUpdate", CommandType.StoredProcedure);

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

        public LiveStockProperty SingleStoneManualPredictionCreateAndUpdate(LiveStockProperty pClsProperty,string StrOpr)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("STOCK_ID", pClsProperty.STOCK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("OPE", StrOpr, DbType.String, ParameterDirection.Input);

                Ope.AddParams("LAB", pClsProperty.LAB, DbType.String, ParameterDirection.Input);

                Ope.AddParams("CARAT", pClsProperty.CARAT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("SHAPE_ID", pClsProperty.SHAPE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("COLOR_ID", pClsProperty.COLOR_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("CLARITY_ID", pClsProperty.CLARITY_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("EXCRATE", pClsProperty.EXCRATE, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("COSTPRICEPERCARAT", pClsProperty.COSTPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("COSTRAPAPORT", pClsProperty.COSTRAPAPORT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("COSTAMOUNT", pClsProperty.COSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("COSTDISCOUNT", pClsProperty.COSTDISCOUNT, DbType.Double, ParameterDirection.Input);


                Ope.AddParams("BLACKINC_ID", pClsProperty.BLACKINC_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TABLEINC_ID", pClsProperty.TABLEINC_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MILKY_ID", pClsProperty.MILKY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("LUSTER_ID", pClsProperty.LUSTER_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TABLEOPEN_ID", pClsProperty.TABLEOPEN_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("COLORSHADE_ID", pClsProperty.COLORSHADE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("CROWNOPEN_ID", pClsProperty.CROWNOPEN_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("PAVOPEN_ID", pClsProperty.PAVOPEN_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("COMMENT", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);


                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_SingleStoneManualPredictionCreateAndUpdate", CommandType.StoredProcedure);

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

        public LiveStockProperty SingleStoneCreateAndUpdate(LiveStockProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("STOCK_ID", pClsProperty.STOCK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CARAT", pClsProperty.CARAT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("PAVANGLE", pClsProperty.PAVANGLE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("MEASUREMENT", pClsProperty.MEASUREMENT, DbType.String, ParameterDirection.Input);

                Ope.AddParams("SHAPE_ID", pClsProperty.SHAPE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("COLOR_ID", pClsProperty.COLOR_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("CLARITY_ID", pClsProperty.CLARITY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("CUT_ID", pClsProperty.CUT_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("POL_ID", pClsProperty.POL_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SYM_ID", pClsProperty.SYM_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("FL", pClsProperty.FL, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("LAB_ID", pClsProperty.LAB_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("BLACKINC_ID", pClsProperty.BLACKINC_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TABLEINC_ID", pClsProperty.TABLEINC_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TABLEOPEN_ID", pClsProperty.TABLEOPEN_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("LUSTER_ID", pClsProperty.LUSTER_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MILKY_ID", pClsProperty.MILKY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("COLORSHADE_ID", pClsProperty.COLORSHADE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("CROWNOPEN_ID", pClsProperty.CROWNOPEN_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("PAVOPEN_ID", pClsProperty.PAVOPEN_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("COSTRAPAPORT", pClsProperty.COSTRAPAPORT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("COSTDISCOUNT", pClsProperty.COSTDISCOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("COSTPRICEPERCARAT", pClsProperty.COSTPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("COSTAMOUNT", pClsProperty.COSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EMPLOYEE_ID", pClsProperty.EMPLOYEE_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("DIAMONDTYPE", pClsProperty.DIAMONDTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PRDTYPE_ID", pClsProperty.PRDTYPE_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMAXSRNO", "", DbType.Int32, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_SingleStoneCreateAndUpdate", CommandType.StoredProcedure);

                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                    pClsProperty.MAXSRNO = Val.ToInt32(AL[3]);
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

        public LiveStockProperty SingleBulkPredictionInsert(LiveStockProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("STOCK_ID", pClsProperty.STOCK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CARAT", pClsProperty.CARAT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("SHAPE_ID", pClsProperty.SHAPE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("COLOR_ID", pClsProperty.COLOR_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("CLARITY_ID", pClsProperty.CLARITY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("CUT_ID", pClsProperty.CUT_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("POL_ID", pClsProperty.POL_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SYM_ID", pClsProperty.SYM_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("FL", pClsProperty.FL, DbType.Int32, ParameterDirection.Input);               
                Ope.AddParams("MILKY_ID", pClsProperty.MILKY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("COLORSHADE_ID", pClsProperty.COLORSHADE_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("COSTRAPAPORT", pClsProperty.COSTRAPAPORT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("COSTDISCOUNT", pClsProperty.COSTDISCOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("COSTPRICEPERCARAT", pClsProperty.COSTPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("COSTAMOUNT", pClsProperty.COSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EMPLOYEE_ID", pClsProperty.EMPLOYEE_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PRDTYPE_ID", pClsProperty.PRDTYPE_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMAXSRNO", "", DbType.Int32, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_SingleBulkPredictionInsert", CommandType.StoredProcedure);

                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                    pClsProperty.MAXSRNO = Val.ToInt32(AL[3]);
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

        public DataTable GetSingleStoneManualPredictionData(string strStoneNo)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("STOCKNO", strStoneNo, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_SingleStoneManualPredictionGetData", CommandType.StoredProcedure);
            return DTab;
        }
        public DataTable GetSingleStoneManualPredictionDetailData(string strStoneNo)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("STOCKNO", strStoneNo, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_SingleStoneManualPredictionDetailsGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetTranferToMarketingData(string strStatus, string strStoneNo)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("STOCKNO", strStoneNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STATUS", strStatus, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_TransferToMarketingGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetDataForSaleDelivery(string strStoneNo, string pStrFromDate, string pStrToDate , string DIAMONDTYPE)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("STOCKNO", strStoneNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("DIAMONDTYPE", DIAMONDTYPE, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_SaleDeliveryLiveStockGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetDataForGIAControlNoMap(string stoneNo, string StrOpe)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("STONENO", stoneNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("OPE", StrOpe, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_GIACoontrolNoMapping", CommandType.StoredProcedure);
            return DTab;
        }

        public DataSet MISHeatMapGetData(
           string StrReportType,
           string StrOpe,
           string StrShape,
           string StrSize,
           string StrCut,
           string StrPol,
           string StrSym,
           string StrFL,
           string StrFromDate,
           string StrToDate,
           string StrColor,
           string StrClarity,
           string StrMilky,
           string StrLab,
           string StrStatus,
           string StrColorShade,
           string StrTableBlackInc,
           string StrSideBlackInc

           )
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("ReportType", StrReportType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Ope", StrOpe, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FromDate", StrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("ToDate", StrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("Shape", StrShape, DbType.String, ParameterDirection.Input);//Add By Gunjan:16/08/2023
            Ope.AddParams("Size", StrSize, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Cut", StrCut, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Pol", StrPol, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Sym", StrSym, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FL", StrFL, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Color", StrColor, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Clarity", StrClarity, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Milky", StrMilky, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Lab", StrLab, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Status", StrStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ColorShade", StrColorShade, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TableBlackInc", StrTableBlackInc, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SideBlackInc", StrSideBlackInc, DbType.String, ParameterDirection.Input);//End As Gunjan

            //Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "MIS_StockHeatMap", CommandType.StoredProcedure);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "MIS_StockHeatMap", CommandType.StoredProcedure);
            return DS;
        }

        public DataSet GetStonePriceHistoryReportGetData(string strFromDate, string strToDate,string StrReporytType,string StrStoneNo)
        {
            Ope.ClearParams();
            DataSet DSet = new DataSet();
            Ope.AddParams("FROMDATE", strFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", strToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("VIEWTYPE", StrReporytType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STONENO", StrStoneNo, DbType.String, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DSet, "Temp", "Rep_SingleStonePriceHistoryReport", CommandType.StoredProcedure);
            return DSet;
        }
        public DataTable GetDataForPickUpPending(string strStoneNo, string pStrFromDate, string pStrToDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("STOCKNO", strStoneNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_SaleDeliveryPickupPendingGetData", CommandType.StoredProcedure);
            return DTab;
        }
        public DataTable GetDataForDeliveryPrint(string StrMemoFetail_IDs)
        {
            DataTable DT = new DataTable();
            Ope.ClearParams();
            Ope.AddParams("MemoDetailIds", StrMemoFetail_IDs, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "RPT_SaleDeliveryPrint", CommandType.StoredProcedure);
            return DT;
        }
        public DataSet GetSaleReportComparisionData(string strReporytType, string strFromDate, string strToDate)
        {
            Ope.ClearParams();
            DataSet DSet = new DataSet();
            Ope.AddParams("VIEWTYPE", strReporytType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", strFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", strToDate, DbType.Date, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DSet, "Temp", "Rep_SingleSaleReportComparisionGetData", CommandType.StoredProcedure);
            return DSet;
        }
        public DataSet GetRepeatCustomerReportGetData(string strFromDate, string strToDate)
        {
            Ope.ClearParams();
            DataSet DSet = new DataSet();
            Ope.AddParams("FROMDATE", strFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", strToDate, DbType.Date, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DSet, "Temp", "Rep_SingleSaleRepeatCustomerReport", CommandType.StoredProcedure);
            return DSet;
        }
    }
}
