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
    public class BOMST_Style
    {
        AxonDataLib.BOSQLHelper Ope = new BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable Fill()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "JW_TRN_StyleGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable FillPro()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            string StrQuery = "SELECT Para_ID AS PROPERTY_ID,ParaCode AS PROPERTYCODE,ParaName AS PROPERTYNAME, '' AS PROPERTYVALUE FROM MST_Para WITH(NOLOCK) WHERE 1=1 AND ParaType='STYLE ADDITIONAL PROPERTY'";
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrQuery, CommandType.Text);
            return DTab;

        }

        public DataSet GetStyleDetailDInfoata(Guid pGuidStyle_Id) //06-04-2019
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("STYLE_ID", pGuidStyle_Id, DbType.Guid, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "JW_TRN_StyleDetailInfoGetData", CommandType.StoredProcedure);
            return DS;
        }

        //public DataSet GetStyleDetailDInfoata(Guid pGuidStyle_Id) //06-04-2019
        //{
        //    Ope.ClearParams();
        //    DataSet DS = new DataSet();
        //    Ope.AddParams("STOCK_ID", pGuidStyle_Id, DbType.Guid, ParameterDirection.Input);
        //    Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "JW_TRN_StyleDetailInfoGetData", CommandType.StoredProcedure);
        //    return DS;
        //}

        public StyleMasterProperty Save(StyleMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("STYLE_ID", pClsProperty.STYLE_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("LOCATION_ID", pClsProperty.LOCATION_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("STYLENO", pClsProperty.STYLENO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PRODUCTCATEGORY_ID", pClsProperty.PRODUCTCATEGORY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("PRODUCTSUBCATEGORY_ID", pClsProperty.PRODUCTSUBCATEGORY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("GROSSWT", pClsProperty.GROSSWT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("NETWT", pClsProperty.NETWT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("METALWT", pClsProperty.METALWT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DIAMONDPCS", pClsProperty.DIAMONDPCS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("DIAMONDWT", pClsProperty.DIAMONDWT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("COLORPCS", pClsProperty.COLORPCS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("COLORWT", pClsProperty.COLORWT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("SIMILLARSTYLE", pClsProperty.SIMILLARSTYLE, DbType.String, ParameterDirection.Input);
                //Ope.AddParams("MATERIAL_ID", pClsProperty.MATERIAL_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COLLECTION_ID", pClsProperty.COLLECTION_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SHORTDESCRIPTION", pClsProperty.SHORTDESCRIPTION, DbType.String, ParameterDirection.Input);
                Ope.AddParams("LONGDESCRIPTION", pClsProperty.LONGDESCRIPTION, DbType.String, ParameterDirection.Input);
                //Ope.AddParams("PROPERTY_ID", pClsProperty.PROPERTY_ID, DbType.String, ParameterDirection.Input);
                //Ope.AddParams("PROPERTYVALUE", pClsProperty.PROPERTYVALUE, DbType.String, ParameterDirection.Input);
                //Ope.AddParams("STATUS", pClsProperty.STATUS, DbType.String, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);

                Ope.AddParams("MATERIAL", pClsProperty.MATERIAL, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("COLLECTION", pClsProperty.COLLECTION, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("MEDIA", pClsProperty.MEDIA, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("PROPERTY", pClsProperty.PROPERTY, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("RANKING", pClsProperty.RANKING, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("REVIEW", pClsProperty.REVIEW, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString,Config.ProviderName, "JW_TRN_StyleSave", CommandType.StoredProcedure);

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

        public DataTable GetDataForCartSummary(Guid pCustomer_ID, string pStrStyleNo, string pStrFromDate, string pStrToDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("CUSTOMER_ID", pCustomer_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("STYLENO", pStrStyleNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "JW_TRN_CartGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public int DeleteStyleDetailInfo(string StrOpe, string StrID)
        {
            string StrQuery = "";

            if (StrOpe == "METALDETAIL")
            {
                StrQuery = "DELETE FROM MST_STYLEMETAL WITH(ROWLOCK) WHERE MATERIAL_ID = '" + StrID + "'";
            }
            if (StrOpe == "DIAMOND")
            {
                StrQuery = "DELETE FROM MST_STYLEDIAMOND WITH(ROWLOCK) WHERE MATERIAL_ID = '" + StrID + "'";
            }
            if (StrOpe == "COLLECTION")
            {
                StrQuery = "DELETE FROM MST_STYLECOLLECTION WITH(ROWLOCK) WHERE COLLECTION_ID = '" + StrID + "'";
            }
            if (StrOpe == "ATTACHMENTDETAIL")
            {
                StrQuery = "DELETE FROM MST_STYLEATTACHMENT WITH(ROWLOCK) WHERE ATTACHMENT_ID = '" + StrID + "'";
            }
            return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
        }

        public StyleMasterProperty Delete(StyleMasterProperty pClsProperty)
        { 
            try
            {
                Ope.ClearParams();
                Ope.AddParams("STYLE_ID", pClsProperty.STYLE_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "JW_TRN_StyleDelete", CommandType.StoredProcedure);

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

        public DataTable GetCollectionData()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            string StrQuery = "SELECT COLLECTION_ID,COLLECTIONCODE,COLLECTIONNAME FROM JW_MST_Collection";
           // string StrQuery = "SELECT COLLECTION_ID,COLLECTIONCODE,COLLECTIONNAME FROM JW_MST_Collection WITH(NOLOCK)  WHERE COLLECTIONNAME = '" + pStrCollectionType + "' And COLLECTION_ID = '" + pIntCollectionID.ToString() + "' And ISActive = 1";
            //DataRow DrParam =  Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
            //return Val.ToInt32(DrParam[0]);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrQuery, CommandType.Text);
            return DTab;
        }

        public DataSet GetDataForOrderSummary(Int32 pIntCustomer_ID, string pStrStyleNo, string pStrOrderNo, string pStrCmbStatus, string pStrFromDate, string pStrToDate)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("CUSTOMER_ID", pIntCustomer_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STYLENO", pStrStyleNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ORDERNO", pStrOrderNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STATUS", pStrCmbStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.String, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "TEMP", "TRN_OrderSummaryGetData", CommandType.StoredProcedure);
            return DS;
        }

        public DataTable GetMaterialTypeData(int pIntMaterialTypeID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            string StrQuery = @"SELECT M.MATERIALTYPE_ID,M.MATERIAL_ID,M.MATERIALCODE,M.MATERIALNAME FROM DBO.JW_MST_MATERIAL M WITH(NOLOCK)
                                LEFT JOIN DBO.JW_MST_MATERIALTYPE MT ON MT.MATERIALTYPE_ID = M.MATERIALTYPE_ID 
                                WHERE M.MATERIALTYPE_ID = '" + pIntMaterialTypeID.ToString() + "' AND M.ISACTIVE = 1 ORDER BY M.MATERIALTYPE_ID";
            //DataRow DrParam =  Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
            //return Val.ToInt32(DrParam[0]);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrQuery, CommandType.Text);
            return DTab;
        }

        //public DataTable GetMaterialTypeData(string pStrParaType, int pIntParentID)
        //{
        //    Ope.ClearParams();
        //    DataTable DTab = new DataTable();
        //    string StrQuery = "SELECT PARA_ID,PARACODE,PARANAME,SHORTNAME FROM MST_PARA WITH(NOLOCK) WHERE PARATYPE = '" + pStrParaType + "' And ParentPara_ID = '" + pIntParentID.ToString() + "' And ISActive = 1 Order By SequenceNo ";
        //    //DataRow DrParam =  Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
        //    //return Val.ToInt32(DrParam[0]);
        //    Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrQuery, CommandType.Text);
        //    return DTab;
        //}



    }
}
