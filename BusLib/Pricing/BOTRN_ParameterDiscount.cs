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
    public class BOTRN_ParameterDiscount
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

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Pri_ParameterDiscountGetData", CommandType.StoredProcedure);
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

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Pri_ParameterDiscountSave", CommandType.StoredProcedure);

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
                Ope.AddParams("PRICINGTYPE", pClsProperty.PRICINGTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("xmlDisc", pClsProperty.XML, DbType.String, ParameterDirection.Input);
                
                Ope.AddParams("ENTRYBY", Configuration.BOConfiguration.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Configuration.BOConfiguration.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Pri_ParameterDiscountSaveXML", CommandType.StoredProcedure);

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
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Pri_OriginalRapRateGetData", CommandType.StoredProcedure);
            return DTab;
        }


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
        public DataTable GetRapnetDataComparision(string pStrOpe,
          string pStrRapDate1,
          string pStrRapDate2,
          string pStrRapDate3,
          string pStrRapDate4,
          string pStrRapDate5,
          string pStrRapDate6,

          string pStrShape, double pDouFromCarat, double pDouToCarat)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("RAPDATE1", Val.SqlDate(pStrRapDate1), DbType.Date, ParameterDirection.Input);
            Ope.AddParams("RAPDATE2", Val.SqlDate(pStrRapDate2), DbType.Date, ParameterDirection.Input);
            Ope.AddParams("RAPDATE3", Val.SqlDate(pStrRapDate3), DbType.Date, ParameterDirection.Input);
            Ope.AddParams("RAPDATE4", Val.SqlDate(pStrRapDate4), DbType.Date, ParameterDirection.Input);
            Ope.AddParams("RAPDATE5", Val.SqlDate(pStrRapDate5), DbType.Date, ParameterDirection.Input);
            Ope.AddParams("RAPDATE6", Val.SqlDate(pStrRapDate6), DbType.Date, ParameterDirection.Input);
            Ope.AddParams("SHAPE", pStrShape, DbType.String, ParameterDirection.Input);
            Ope.AddParams("F_CARAT", pDouFromCarat, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("T_CARAT", pDouToCarat, DbType.Double, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Pri_RapnetDataComparision", CommandType.StoredProcedure);
            return DTab;
        }


        public DataTable GetRapComparisionExportGetData(string StrRapDate1, string StrRapDate2)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("RapDate1", StrRapDate1, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("RapDate2", StrRapDate2, DbType.Date, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Rep_RapComparisionExportGetData", CommandType.StoredProcedure);
            return DTab;
        }

        #region Price Check

        public int PriceCheck_SaveAllCombination(string pStrRapDate)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("RAPDATE", pStrRapDate, DbType.Date, ParameterDirection.Input);
                return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "Rap_PriceCheckGetAllCombination", CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                return -1;
            }

        }

        public DataRow PriceCheck_GetPendingCount(string pStrRapDate)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("RAPDATE", pStrRapDate, DbType.Date, ParameterDirection.Input);
                return Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, "Rap_PriceCheckGetPendingProcess", CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public DataTable PriceCheck_GetProblemData(
            string pStrRapDate,
            string pStrShape,
            string pStrColor,
            string pStrClarity,
            string pStrCut,
            string pStrPol,
            string pStrSym,
            string pStrFL,
            string pStrSize
            )
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();
                Ope.AddParams("RAPDATE", pStrRapDate, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("SHAPE", pStrShape, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COLOR", pStrColor, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CLARITY", pStrClarity, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CUT", pStrCut, DbType.String, ParameterDirection.Input);
                Ope.AddParams("POL", pStrPol, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SYM", pStrSym, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FL", pStrFL, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SIZE", pStrSize, DbType.String, ParameterDirection.Input);

                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Rap_PriceCheckGetProblemData", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public DataSet PriceCheck_GetParameter()
        {
            try
            {
                DataSet DS = new DataSet();
                Ope.ClearParams();
                Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Rap_PriceCheckGetAllParameter", CommandType.StoredProcedure);
                return DS;
            }
            catch (Exception ex)
            {
                return null;
            }

        }



        #endregion
    }
}
