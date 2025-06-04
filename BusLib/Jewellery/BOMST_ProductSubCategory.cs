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
    public class BOMST_ProductSubCategory
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable Fill()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "JW_MST_ProductSubCategoryGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public ProductSubCategoryProperty Save(ProductSubCategoryProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("PRODUCTSUBCATEGORY_ID", pClsProperty.PRODUCTSUBCATEGORY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("PRODUCTSUBCATEGORYCODE", pClsProperty.PRODUCTSUBCATEGORYCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PRODUCTSUBCATEGORYNAME", pClsProperty.PRODUCTSUBCATEGORYNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PRODUCTCATEGORY_ID", pClsProperty.PRODUCTCATEGORY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("STATUS", pClsProperty.STATUS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ISACTIVE", pClsProperty.ISACTIVE, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
               
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL;
                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "JW_MST_ProductSubCategoryInsertUpdate", CommandType.StoredProcedure);
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

        public ProductSubCategoryProperty Delete(ProductSubCategoryProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("PRODUCTSUBCATEGORY_ID", pClsProperty.PRODUCTSUBCATEGORY_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL;
                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "JW_MST_ProductSubCategoryDelete", CommandType.StoredProcedure);
                
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

    }
}
