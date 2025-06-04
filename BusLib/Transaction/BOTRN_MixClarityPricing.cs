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
    public class BOTRN_MixClarityPricing
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable SavePriceListUsingDataTable(string pStrStockUploadXML, Int32 pIntDepartment_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("XMLSTOCKUPLOAD", pStrStockUploadXML, DbType.Xml, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", pIntDepartment_ID, DbType.Int32, ParameterDirection.Input);

            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_PriceChartBulkUpload", CommandType.StoredProcedure);

            return DTab;
        }


        //public DataTable GetStockUploadData(string pStrRapeDate, string StrOpe = "WITHOUTSTOCK", string pStrShape = "", string pStrMixCls = "" , decimal pDecCarat =0 ) //WITHOUTSTOCK OR WITHSTOCK
        //{
        //    Ope.ClearParams();
        //    DataTable DTab = new DataTable();

        //    Ope.AddParams("OPE", StrOpe, DbType.String, ParameterDirection.Input);
        //    Ope.AddParams("RAPEDATE", pStrRapeDate, DbType.Date, ParameterDirection.Input);
        //    Ope.AddParams("SHAPE", pStrShape, DbType.String, ParameterDirection.Input);
        //    Ope.AddParams("MIXCLS", pStrMixCls, DbType.String, ParameterDirection.Input);
        //    Ope.AddParams("ACTCARAT", pDecCarat, DbType.Decimal, ParameterDirection.Input);

        //    Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_ParcelMixClarityPriceGetData", CommandType.StoredProcedure);

        //    return DTab;
        //}

       


        public DataTable GetParcelPriceData(int pIntPriceID,int pIntShapeID,int pIntDepartmentID) //WITHOUTSTOCK OR WITHSTOCK
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("PRICE_ID", pIntPriceID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", pIntShapeID, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", pIntDepartmentID, DbType.String, ParameterDirection.Input);
           
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_PriceChartGetData", CommandType.StoredProcedure);

            return DTab;
        }


        public DataTable SaveSingleData(ParcelPriceChartProperty pClsProperty) //WITHOUTSTOCK OR WITHSTOCK
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("PRICE_ID", pClsProperty.PRICE_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("PRICEDATE", pClsProperty.PRICEDATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", pClsProperty.SHAPE_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", pClsProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("MIXSIZE_ID", pClsProperty.MIXSIZE_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("MIXCLARITY_ID", pClsProperty.MIXCLARITY_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("RATE", pClsProperty.RATE, DbType.Double, ParameterDirection.Input);

            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_PriceChartSingleSave", CommandType.StoredProcedure);

            return DTab;
        }

    }
}
