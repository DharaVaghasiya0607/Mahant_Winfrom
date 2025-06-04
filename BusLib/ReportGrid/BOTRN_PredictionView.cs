using System;
using System.Data;
using AxonDataLib;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using BusLib.TableName;
using System.Collections;


namespace BusLib.ReportGrid
{
    public class BOTRN_PredictionView
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        

        #region Other Function

        public DataSet PredictionViewGetData(string pStrKapan, int pIntFromPacketNo, int pIntToPacketNo, string pStrTag, string pStrParentTag, string pStrMainTag, string pStrPrdType)
        {
            try
            {
                DataSet DS = new DataSet();

                Ope.ClearParams();
                Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FROMPACKETNO", pIntFromPacketNo, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TOPACKETNO", pIntToPacketNo, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TAG", pStrTag, DbType.String, ParameterDirection.Input);
                //Ope.AddParams("MAINTAG", pStrMainTag, DbType.String, ParameterDirection.Input);
                //Ope.AddParams("PARENTTAG", pStrParentTag, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PRDTYPE_ID", pStrPrdType, DbType.String, ParameterDirection.Input);

                Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "RP_SinglePredictionViewNew", CommandType.StoredProcedure);
                return DS;
            }
            catch (Exception Ex)
            {
               
                return null;
            }
        }


        public DataSet PredictionViewGetDataNew(string pStrKapan, int pIntFromPacketNo, int pIntToPacketNo, string pStrTag, string pStrParentTag, string pStrMainTag, string pStrPrdType, string pStrFromDate, string pStrToDate, string pStrOpe, Int64 pIntEmployeeID)
        {
            try
            {
                DataSet DS = new DataSet();

                Ope.ClearParams();
                Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FROMPACKETNO", pIntFromPacketNo, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TOPACKETNO", pIntToPacketNo, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TAG", pStrTag, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FROMDATE", pStrFromDate, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TODATE", pStrToDate, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PRDTYPE_ID", pStrPrdType, DbType.String, ParameterDirection.Input);
                Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EMPLOYEE_ID", pIntEmployeeID, DbType.Int64, ParameterDirection.Input);
                
                Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "RP_SinglePrdViewComparision", CommandType.StoredProcedure);
                return DS;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }



        public DataTable DTabPredictionDataForManagement(string pStrKapan, int pIntPacketNo, string pStrTag)
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();
                Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PACKETNO", pIntPacketNo, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TAG", pStrTag, DbType.String, ParameterDirection.Input);

                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "RP_SinglePrdViewForManagement", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }


        public DataSet DTabPredictionDataForMarker(string pStrKapan, int pIntFromPacketNo, int pIntToPacketNo, string pStrTag, Int64 pIntEmployeeID,string pStrPrdType,string pStrFromDate,string pStrToDate)
        {
            try
            {
                DataSet DS = new DataSet();

                Ope.ClearParams();
                Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FROMPACKETNO", pIntFromPacketNo, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TOPACKETNO", pIntToPacketNo, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TAG", pStrTag, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EMPLOYEE_ID", pIntEmployeeID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("PRDTYPE_ID", pStrPrdType, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FROMDATE", pStrFromDate, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TODATE", pStrToDate, DbType.String, ParameterDirection.Input);

                Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "temp", "RP_SinglePrdViewForMarker", CommandType.StoredProcedure);
                return DS;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }



        public DataTable PriceRevised(string pStrOpe, string pStrKapan, string pStrRapDate)
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();
                Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
                Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
                Ope.AddParams("RAPDATE", pStrRapDate, DbType.String, ParameterDirection.Input);

                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Job_SinglePriceRevised", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }

        public DataSet PredictionViewGetDataForAdmin(string pStrClientRefNo, string pStrPrdType, string pStrFromDate, string pStrToDate, string pIntEmployee_ID, string pStrPrdTypeOther, string pStrKapan)
        {
            try
            {
                DataSet DS = new DataSet();

                Ope.ClearParams();
                Ope.AddParams("CLIENTREFNO", pStrClientRefNo, DbType.String, ParameterDirection.Input);
                Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PRDTYPE_ID", pStrPrdType, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FROMDATE", pStrFromDate, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TODATE", pStrToDate, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EMPLOYEE_ID", pIntEmployee_ID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("PrdTypeOther_ID", pStrPrdTypeOther, DbType.String, ParameterDirection.Input);

                Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS,"TEMP", "RP_SinglePrdViewComparisionAdmin", CommandType.StoredProcedure);
                return DS;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }


        //End As

        #endregion
        public DataTable MFGComparisionData(string pStrFromDate, string pStrToDate, string pStrStockNo) //ADD DARSHAN : 25-03-2020
        {
            Ope.ClearParams();
            DataTable DT = new DataTable();
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input); 
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("STOCKNO", pStrStockNo, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "RP_MFGComparisionReport", CommandType.StoredProcedure);
            return DT;
        }

        public DataSet MFGGradingReport_Export(string pStrFromDate, string pStrToDate)
        {
            DataSet DS = new DataSet();
            Ope.ClearParams();
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.String, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "RP_MFGComparisionExcelExport", CommandType.StoredProcedure);
            return DS;
        }

        public DataTable GetCostRapaport(string pStrStockNo, Double pDouDiscount, Double pDouPricePerCarat)
        {
            DataTable DT = new DataTable();
            Ope.ClearParams();
            Ope.AddParams("STOCKNO", pStrStockNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COSTDISCOUNT", pDouDiscount, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("COSTPRICEPERCARAT", pDouPricePerCarat, DbType.Double, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "TRN_StockCostRapaportGetData", CommandType.StoredProcedure);
            return DT;

        }

        public DataTable GetStonePriceExcelWiseGetData(string pStrStonePriceXml) //#P : 26-04-2021 : StonePriceUpload
        {
            Ope.ClearParams();
            DataTable DT = new DataTable();
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("XMLFORSTONEPRICE", pStrStonePriceXml, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "TRN_StockCostRapaport_FileUpload", CommandType.StoredProcedure);
            return DT;
        }
    }
}

