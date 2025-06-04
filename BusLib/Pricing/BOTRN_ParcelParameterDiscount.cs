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
    public class BOTRN_ParcelParameterDiscount
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable GetParameterDiscountData(string pStrOpe, string pStrParameterID, string pStrRapDate, string pStrShape, double pDouFromCarat, double pDouToCarat,string pStrPricingType)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PARAMETER_ID", pStrParameterID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("RAPDATE", pStrRapDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", pStrShape, DbType.String, ParameterDirection.Input);
            Ope.AddParams("F_CARAT", pDouFromCarat, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("T_CARAT", pDouToCarat, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("PRICINGTYPE", pStrPricingType, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Pri_ParcelParameterDiscountGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public ParameterDiscountProperty SaveParameterDiscount(ParameterDiscountProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("F_CARAT", pClsProperty.F_CARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("T_CARAT", pClsProperty.T_CARAT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("SHAPE_ID", pClsProperty.SHAPE_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COLOR_ID", pClsProperty.COLOR_ID, DbType.String, ParameterDirection.Input);

                Ope.AddParams("Q_CODE", pClsProperty.Q_CODE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RAPDATE", pClsProperty.RAPDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("PARAMETER_ID", pClsProperty.PARAMETER_ID, DbType.String, ParameterDirection.Input);

                Ope.AddParams("PARAMETER_VALUE", pClsProperty.PARAMETER_VALUE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("OLDVALUE", pClsProperty.OLDVALUE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("NEWVALUE", pClsProperty.NEWVALUE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("PRICINGTYPE", pClsProperty.PRICINGTYPE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Configuration.BOConfiguration.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Configuration.BOConfiguration.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Pri_ParcelParameterDiscountSave", CommandType.StoredProcedure);

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


        public ParameterDiscountProperty SaveParameterDiscountUsingXml(ParameterDiscountProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();

                Ope.AddParams("RAPDATE", pClsProperty.RAPDATE, DbType.DateTime, ParameterDirection.Input);
                Ope.AddParams("PARAMETER_ID", pClsProperty.PARAMETER_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("xmlDisc", pClsProperty.XML, DbType.String, ParameterDirection.Input);

                Ope.AddParams("PRICINGTYPE", pClsProperty.PRICINGTYPE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Configuration.BOConfiguration.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Configuration.BOConfiguration.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Pri_ParcelParameterDiscountSaveXML", CommandType.StoredProcedure);

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


        public DataTable GetOriginalRapData(string pStrOpe, string pStrRapDate, string pStrShape, double pDouFromCarat, double pDouToCarat)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PARAMETER_ID", "", DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHAPE", pStrShape, DbType.String, ParameterDirection.Input);
            Ope.AddParams("RAPDATE", Val.SqlDate(pStrRapDate), DbType.Date, ParameterDirection.Input);
            Ope.AddParams("F_CARAT", pDouFromCarat, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("T_CARAT", pDouToCarat, DbType.Double, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Pri_ParcelOriginalRapRateGetData", CommandType.StoredProcedure);
            return DTab;
        }

       
        public string UpdateRapnetWithAllDiscount(string pRoundXml, string pPearXml, string pStrRapDate)
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

            ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Pri_ParcelUpdateRapnetWithAllDiscount", CommandType.StoredProcedure);

            if (AL.Count != 0)
            {
                return Val.ToString(AL[1]);
            }

            return "";
        }

        public ParameterDiscountProperty SaveParcelParameterDiscount(ParameterDiscountProperty pClsProperty) // Dhara : 24-11-2021
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("F_CARAT", pClsProperty.F_CARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("T_CARAT", pClsProperty.T_CARAT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("SHAPE_ID", pClsProperty.SHAPE_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COLOR_ID", pClsProperty.COLOR_ID, DbType.String, ParameterDirection.Input);

                Ope.AddParams("Q_CODE", pClsProperty.Q_CODE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RAPDATE", pClsProperty.RAPDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("PARAMETER_ID", pClsProperty.PARAMETER_ID, DbType.String, ParameterDirection.Input);

                Ope.AddParams("PARAMETER_VALUE", pClsProperty.PARAMETER_VALUE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("OLDVALUE", pClsProperty.OLDVALUE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("NEWVALUE", pClsProperty.NEWVALUE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("PRICINGTYPE", pClsProperty.PRICINGTYPE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Configuration.BOConfiguration.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Configuration.BOConfiguration.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Pri_ParcelParameterDiscountSave", CommandType.StoredProcedure);

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
        public DataTable GetClvMixPriceChartData(string pStrOpe, string pStrParameterID, string pStrRapDate, string pStrShape, double pDouFromCarat, double pDouToCarat, string pStrPricingType) // Dhara:24-11-2021
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("PARAMETER_ID", pStrParameterID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("RAPDATE", pStrRapDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("F_CARAT", pDouFromCarat, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("T_CARAT", pDouToCarat, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("PRICINGTYPE", pStrPricingType, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "PRI_ClvMixPriceChartSummary", CommandType.StoredProcedure);
            return DTab;
        }
        public DataTable GetPricePriceViewGetData(LiveStockProperty pClsProperty)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("SHAPE_ID", pClsProperty.MULTYSHAPE_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLOR_ID", pClsProperty.MULTYCOLOR_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CLARITY_ID", pClsProperty.MULTYCLARITY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUT_ID", pClsProperty.MULTYCUT_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("POL_ID", pClsProperty.MULTYPOL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SYM_ID", pClsProperty.MULTYSYM_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FL_ID", pClsProperty.MULTYFL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LAB_ID", pClsProperty.MULTYLAB_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("WEBSTATUS", pClsProperty.WEBSTATUS, DbType.String, ParameterDirection.Input);

            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LABREPORTNO", pClsProperty.LABREPORTNO, DbType.String, ParameterDirection.Input);

            Ope.AddParams("SALESFROMDATE", pClsProperty.SALESFROMDATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("SALESTODATE", pClsProperty.SALESTODATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("AVAILBLEFROMDATE", pClsProperty.AVAILBLEFROMDATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("AVAILBLETODATE", pClsProperty.AVAILBLETODATE, DbType.Date, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_PriceViewGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetDataForPriceDetail(LiveStockProperty pClsProperty)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("SHAPE_ID", pClsProperty.SHAPE_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("COLOR_ID", pClsProperty.COLOR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("CLARITY_ID", pClsProperty.CLARITY_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("CUT_ID", pClsProperty.CUT_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("POL_ID", pClsProperty.POL_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("SYM_ID", pClsProperty.SYM_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("WEBSTATUS", pClsProperty.WEBSTATUS, DbType.String, ParameterDirection.Input);

            Ope.AddParams("CARAT", pClsProperty.CARAT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("SALEDISCOUNT", pClsProperty.SALEDISCOUNT, DbType.Double, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_PriceViewDetailGetData", CommandType.StoredProcedure);
            return DTab;
        }
    }
}
