using System;
using System.Data;
using AxonDataLib;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using BusLib.TableName;
using System.Collections;


namespace BusLib.Utility
{
    public class BOMST_ApiSetting
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        

        #region Other Function

        public DataTable GetAPISetting()
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();

                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "MST_APISettingGetData", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {
               
                return null;
            }
        }

        public int UpdateHistory(int pIntAPID, string pStrResult, string pStrFileName,int TotalPcs, double DouCarat)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("API_ID", pIntAPID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("RESULT", pStrResult, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FILENAME", pStrFileName, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TOTALPCS", TotalPcs, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TOTALCARAT", DouCarat, DbType.String, ParameterDirection.Input);

                int IntRes = Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "HST_APICallSave", CommandType.StoredProcedure);

                return IntRes;
            }
            catch (Exception Ex)
            {
                return -1;
            }
        }

        public DataTable GetJamelsAllenData(string pStrOpe, string pStrStoneDetail)
        {
            try
            {
                DataTable DTab = new DataTable();
                 
                Ope.ClearParams();
                Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", pStrStoneDetail, DbType.Xml, ParameterDirection.Input);

                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "API_GetDataForJamesAllen", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }

        public DataTable GetRapnetData(string pStrOpe, string pStrStoneDetail)
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();
                Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", pStrStoneDetail, DbType.Xml, ParameterDirection.Input);

                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "API_GetDataForRapnet", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }
        public DataTable GetPolyGonData(string pStrOpe, string pStrStoneDetail)
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();
                Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", pStrStoneDetail, DbType.Xml, ParameterDirection.Input);

                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "API_GetDataForPolygon", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }
        public DataTable GetLiquidDiamondData(string pStrOpe, string pStrStoneDetail)
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();
                Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", pStrStoneDetail, DbType.Xml, ParameterDirection.Input);

                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "API_GetDataForLiquidDiamond", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }
        public DataTable GetGETDiamondData(string pStrOpe, string pStrStoneDetail)
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();
                Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", pStrStoneDetail, DbType.Xml, ParameterDirection.Input);

                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "API_GetDataForGetDiamond", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }
        public DataTable GetIDexData(string pStrOpe, string pStrStoneDetail)
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();
                Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", pStrStoneDetail, DbType.Xml, ParameterDirection.Input);

                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "API_GetDataForIdex", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }
        public DataTable GetNivodaData(string pStrOpe, string pStrStoneDetail)
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();
                Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", pStrStoneDetail, DbType.Xml, ParameterDirection.Input);

                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "API_GetDataForNivoda", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }
        public DataTable GetNicanorData(string pStrOpe, string pStrStoneDetail)
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();
                Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", pStrStoneDetail, DbType.Xml, ParameterDirection.Input);

                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "API_GetDataForNicanor", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }
        public DataTable GetMarketDiamondData(string pStrOpe, string pStrStoneDetail)
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();
                Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", pStrStoneDetail, DbType.Xml, ParameterDirection.Input);

                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "API_GetDataForMarketDiamond", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }
        public DataTable GetAPIHistory(string pStrEntryDate)
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();
                Ope.AddParams("ENTRYDATE", pStrEntryDate, DbType.Date, ParameterDirection.Input);
                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "HST_APICallGetData", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }

        public DataTable GetUNIAxonDiamonddData(string pStrOpe, string pStrStoneDetail)
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();
                Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", pStrStoneDetail, DbType.Xml, ParameterDirection.Input);

                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "API_GetDataForUNIAxon", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }


        public MSTAPIProperty GetAPISetting(string pStrAPICode)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("APICODE", pStrAPICode, DbType.String, ParameterDirection.Input);

                DataRow DRow = Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, "MST_APISettingGetData", CommandType.StoredProcedure);

                MSTAPIProperty Property = new TableName.MSTAPIProperty();
                if (DRow == null)
                {
                    Property.API_ID = 0;
                    Property.APICODE = "";
                    Property.APINAME = "";
                    Property.ACTIVE = false;
                    Property.URL = "";
                    Property.HOSTNAME = "";
                    Property.USERNAME = "";
                    Property.PASSWORD = "";
                    Property.PORT = 0;
                    Property.CERTICATE = "";

                    return Property;
                }
                else
                {
                    Property.API_ID = Val.ToInt(DRow["API_ID"]);
                    Property.APICODE = Val.ToString(DRow["APICODE"]);
                    Property.APINAME = Val.ToString(DRow["APINAME"]);
                    Property.ACTIVE = Val.ToBoolean(DRow["ACTIVE"]);
                    Property.URL = Val.ToString(DRow["URL"]);
                    Property.HOSTNAME = Val.ToString(DRow["HOSTNAME"]);
                    Property.USERNAME = Val.ToString(DRow["USERNAME"]);
                    Property.PASSWORD = Val.ToString(DRow["PASSWORD"]);
                    Property.PORT = Val.ToInt(DRow["PORT"]);
                    Property.CERTICATE = Val.ToString(DRow["CERTICATE"]);

                    return Property;

                }
            }
            catch (Exception Ex)
            {

                return null;
            }
        }

        //End As
        #endregion
    }
}

