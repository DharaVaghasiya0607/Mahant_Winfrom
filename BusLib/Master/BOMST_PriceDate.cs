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
    public class BOMST_PriceDate
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable Fill(string pStrGroup)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("PRICETYPE", pStrGroup, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "MST_PriceDateGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public PriceDateMasterProperty Save(PriceDateMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("PRICE_ID", pClsProperty.PRICE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("PRICETYPE", pClsProperty.PRICETYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PRICEDATE", pClsProperty.PRICEDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("ISACTIVE", pClsProperty.ISACTIVE, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_PriceDateSave", CommandType.StoredProcedure);

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

        public PriceDateMasterProperty Delete(PriceDateMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("PRICE_ID", pClsProperty.PRICE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_PriceDateDelete", CommandType.StoredProcedure);

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
        public DataTable GetParameterData()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            string StrQuery = "SELECT PARA_ID,PARACODE,PARANAME,SHORTNAME,REMARK,LABCODE,PARATYPE,PARAGROUP,RAPAVALUE FROM MST_PARA WITH(NOLOCK) WHERE 1=1 ";
            //DataRow DrParam =  Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
            //return Val.ToInt32(DrParam[0]);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrQuery, CommandType.Text);
            return DTab;
        }
        public DataTable GetParameterDataParcel()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            string StrQuery = "SELECT ISNULL(PARA_ID,0) AS PARA_ID,ISNULL(PARACODE,'') AS PARACODE ,ISNULL(PARANAME,'') AS PARANAME ,ISNULL(LABCODE,'') AS LABCODE,PARATYPE ";
            StrQuery += " FROM MST_PARA WITH(NOLOCK) WHERE PARATYPE IN('SHAPE','COLOR','CLARITY','CUT','POLISH','SYMMETRY','FLUORESCENCE','LOCATION','MILKY','FANCYCOLOR','LAB','MIX_CLARITY' ";
            StrQuery += " ) UNION ALL SELECT SIZE_ID as PARA_ID, SIZENAME as PARACODE, SIZENAME AS PARANAME, remark AS LABCODE, 'MIX_SIZE' as PARATYPE ";
            StrQuery += " FROM MST_MIXSIZE WITH(NOLOCK) WHERE 1=1 ";
            //DataRow DrParam =  Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
            //return Val.ToInt32(DrParam[0]);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrQuery, CommandType.Text);
            return DTab;
        }
    }
}
