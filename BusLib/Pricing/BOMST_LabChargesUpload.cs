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

namespace BusLib.Pricing
{
    public class BOMST_LabChargesUpload
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable GetTableHeader()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            string StrOpe = "Select top(0) LABCHARGEUPLOAD_ID,FROMCARAT,TOCARAT,FROMCOLOR,FROMCOLOR_ID,TOCOLOR,TOCOLOR_ID,SERVICETYPE,SERVICETYPE_ID,AMOUNT From Mst_LabChargesUpload With(Nolock)";

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrOpe, CommandType.Text);
            return DTab;
        }

        public DataTable FindAddLessAmt(string StrDate, string StrToDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            string StrOpe = "select ADDLESS1,ADDLESS2 from Mst_LabChargesUpload With(nolock) where ApplicableDate ='" + StrDate + "' and ApplicableToDate = '" + StrToDate + "'";

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrOpe, CommandType.Text);
            return DTab;
        }

        public DataTable GetShowData(string StrDate, string StrToAppDate, string pStrDiamondType, string pStrLabType)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            string StrOpe = "select LABCHARGEUPLOAD_ID,FROMCARAT,TOCARAT,FROMCOLOR,FROMCOLOR_ID,TOCOLOR,TOCOLOR_ID,APPLICABLEDATE,APPLICABLETODATE,SERVICETYPE,SERVICETYPE_ID,AMOUNT,ADDLESS1,ADDLESS2,CALCTYPE,DIAMONDTYPE,LAB from Mst_LabChargesUpload With(nolock) where  DiamondType = '" + pStrDiamondType + "' AND Lab = '" + pStrLabType + "' Order By FromCarat";

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrOpe, CommandType.Text);
            return DTab;
        }
        public DataTable GetShowDatafETCH(string StrDate, string StrToAppDate, string pStrDiamondType, string pStrLabType)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            string StrOpe = "select LABCHARGEUPLOAD_ID,FROMCARAT,TOCARAT,FROMCOLOR,FROMCOLOR_ID,TOCOLOR,TOCOLOR_ID,APPLICABLEDATE,APPLICABLETODATE,SERVICETYPE,SERVICETYPE_ID,AMOUNT,ADDLESS1,ADDLESS2,CALCTYPE,DIAMONDTYPE,LAB from Mst_LabChargesUpload With(nolock) where  DiamondType = '" + pStrDiamondType + "' AND Lab = '" + pStrLabType + "' AND  CONVERT(DATE,APPLICABLETODATE) = '" + StrDate + "' Order By FromCarat";

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrOpe, CommandType.Text);
            return DTab;
        }

        public DataTable GetSelectedData(string StrDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            string StrOpe = "select LABCHARGEUPLOAD_ID,FROMCARAT,TOCARAT,FROMCOLOR,FROMCOLOR_ID,TOCOLOR,TOCOLOR_ID,APPLICABLEDATE,APPLICABLETODATE,SERVICETYPE,SERVICETYPE_ID,AMOUNT,ADDLESS1,ADDLESS2 from Mst_LabChargesUpload With(nolock) where ApplicableDate ='" + StrDate + "' Order By FromCarat";

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrOpe, CommandType.Text);
            return DTab;
        }


        public string UpdateAddlessValue(string StrDate, double StrAddless1, double StrAddless2)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("APPLICABLEDATE", StrDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("ADDLESS1", StrAddless1, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("ADDLESS2", StrAddless2, DbType.Double, ParameterDirection.Input);

            Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "MST_LabchargeUploadUpdateAdLess", CommandType.StoredProcedure);

            return "";
        }

        public DataTable GetParameterData()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            string StrQuery = "SELECT ISNULL(PARA_ID,0) AS PARA_ID,ISNULL(PARACODE,'') AS PARACODE ,ISNULL(PARANAME,'') AS PARANAME , ISNULL(REMARK,'') AS REMARK,ISNULL(LABCODE,'') AS LABCODE, PARATYPE FROM MST_PARA WITH(NOLOCK) WHERE 1=1 ";
            //DataRow DrParam =  Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
            //return Val.ToInt32(DrParam[0]);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrQuery, CommandType.Text);
            return DTab;
        }

        public string Save(LabChargesUploadProperty pClsProperty)
        {
            Ope.ClearParams();
            Ope.AddParams("LABCHARGEUPLOAD_ID", pClsProperty.LABCHARGEUPLOAD_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("FROMCARAT", pClsProperty.FROMCARAT, DbType.Double, ParameterDirection.Input);

            Ope.AddParams("TOCARAT", pClsProperty.TOCARAT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FROMCOLOR", pClsProperty.FROMCOLOR, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMCOLOR_ID", pClsProperty.FROMCOLOR_ID, DbType.Int32, ParameterDirection.Input);

            Ope.AddParams("TOCOLOR", pClsProperty.TOCOLOR, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TOCOLOR_ID", pClsProperty.TOCOLOR_ID, DbType.Int32, ParameterDirection.Input);

            Ope.AddParams("SERVICETYPE", pClsProperty.SERVICETYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SERVICETYPE_ID", pClsProperty.SERVICETYPE_ID, DbType.Int32, ParameterDirection.Input);

            Ope.AddParams("CALCTYPE", pClsProperty.CALCTYPE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("LAB", pClsProperty.LAB, DbType.String, ParameterDirection.Input);
            Ope.AddParams("DIAMONDTYPE", pClsProperty.DIAMONDTYPE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("AMOUNT", pClsProperty.AMOUNT, DbType.Double, ParameterDirection.Input);

            Ope.AddParams("FROMAPPLICABLEDATE", pClsProperty.APPLICABLEDATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TOAPPLICABLEDATE", pClsProperty.TOAPPLICABLEDATE, DbType.Date, ParameterDirection.Input);

            Ope.AddParams("ADDLESS1", pClsProperty.ADDLESS1, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("ADDLESS2", pClsProperty.ADDLESS2, DbType.Double, ParameterDirection.Input);

            Ope.AddParams("ENTRYBY", Configuration.BOConfiguration.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Configuration.BOConfiguration.ComputerIP, DbType.String, ParameterDirection.Input);

            int IntRes = Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "MST_LabChargesUploadSave", CommandType.StoredProcedure);

            if (IntRes > 0)
            {
                return "SUCCESS";
            }
            else
            {
                return "FAIL";
            }
        }

        public DataTable GetDataForValidation(string StrFromAppDate, string StrToAppDate)
        {
            DataTable DTab = new DataTable();
            Ope.ClearParams();

            Ope.AddParams("FROMAPPLICABLEDATE", StrFromAppDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TOAPPLICABLEDATE", StrToAppDate, DbType.Date, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Mst_LabChargeUploadGetDataForValidation", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetData(string StrFromDate, string StrToDate, string pStrDiamondType, string pStrLab)
        {
            
            DataTable DTab = new DataTable();
            Ope.ClearParams();

            Ope.AddParams("FROMDATE", StrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", StrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("DIAMONDTYPE", pStrDiamondType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LAB", pStrLab, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "MST_LabChargesUploadGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public string Delete(Guid Str_ID)
        {
            DataTable DTab = new DataTable();
            Ope.ClearParams();

            Ope.AddParams("LABCHARGEUPLOAD_ID", Str_ID, DbType.Guid, ParameterDirection.Input);


            int intres = Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "MST_LabChargeUploadDelete", CommandType.StoredProcedure);

            if (intres > 0)
            {
                return "SUCCESS";
            }
            else
            {
                return "FAIL";
            }
        }

    }
}
