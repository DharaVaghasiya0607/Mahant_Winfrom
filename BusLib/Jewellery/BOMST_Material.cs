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
    public class BOMST_Material
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable Fill(Int32 pStrGroup)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MATERIALTYPE_ID", pStrGroup, DbType.Int32, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "JW_MST_MaterialGetData", CommandType.StoredProcedure);
            return DTab;

        }
       
        public MaterialMasterProperty Save(MaterialMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MATERIAL_ID", pClsProperty.MATERIAL_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MATERIALTYPE_ID", pClsProperty.MATERIALTYPE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MATERIALCODE", pClsProperty.MATERIALCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MATERIALNAME", pClsProperty.MATERIALNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PARENTMATERIAL_ID", pClsProperty.PARENTMATERIAL_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("STATUS", pClsProperty.STATUS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);  
                Ope.AddParams("ISACTIVE", pClsProperty.ISACTIVE, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "JW_MST_MaterialInsertUpdate", CommandType.StoredProcedure);

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


        public MaterialMasterProperty Delete(MaterialMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MATERIAL_ID", pClsProperty.MATERIAL_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);


                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "JW_MST_MaterialDelete", CommandType.StoredProcedure);

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
            string StrQuery = "SELECT MATERIAL_ID,MATERIALCODE,MATERIALNAME,REMARK FROM JW_MST_Material WITH(NOLOCK) WHERE 1=1 ";
            //DataRow DrParam =  Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
            //return Val.ToInt32(DrParam[0]);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrQuery, CommandType.Text);
            return DTab;
        }
        public DataTable GetFancyColorData()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            string StrQuery = "SELECT FANCYCOLOR_ID,REMARK FROM MST_FANCYCOLOR  WITH(NOLOCK) WHERE ISACTIVE = 1 ";
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrQuery, CommandType.Text);
            return DTab;
        }

        public DataTable GetParameterDataParcel()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            string StrQuery = "SELECT ISNULL(MATERIAL_ID,0) AS MATERIAL_ID,ISNULL(MATERIALCODE,'') AS MATERIALCODE ,ISNULL(MATERIALNAME,'') AS MATERIALNAME ,ISNULL(LABCODE,'') AS LABCODE,PARATYPE ";
            StrQuery += " FROM MST_PARA WITH(NOLOCK) WHERE PARATYPE IN('SHAPE','COLOR','CLARITY','CUT','POLISH','SYMMETRY','FLUORESCENCE','LOCATION','MILKY','FANCYCOLOR','LAB','MIX_CLARITY' ";
            StrQuery += " ) UNION ALL SELECT SIZE_ID as MATERIAL_ID, SIZENAME as MATERIALCODE, SIZENAME AS MATERIALNAME, remark AS LABCODE, 'MIX_SIZE' as PARATYPE ";
            StrQuery += " FROM MST_MIXSIZE WITH(NOLOCK) WHERE 1=1 ";
            //DataRow DrParam =  Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
            //return Val.ToInt32(DrParam[0]);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrQuery, CommandType.Text);
            return DTab;
        }

        public DataTable GetColorClarityComment()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "ColorClarityComment_GetData", CommandType.StoredProcedure);
            return DTab;
        }

        //Added by Daksha on 4/05/2023
        public DataTable ParcelParameter_GetData()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "ParcelParameter_GetData", CommandType.StoredProcedure);
            return DTab;
        }
        //End as Daksha
    }
}
