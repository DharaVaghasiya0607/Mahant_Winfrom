using AxonDataLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using System.Collections;
using BusLib.TableName;

namespace BusLib.Parcel
{
    public class BOTRN_ParcelStockTally
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable Fill(string pStrStockTallyDate,int pIntDepartment_ID)
        {
            DataTable DTab = new DataTable();
            Ope.AddParams("STOCKTALLYDATE", pStrStockTallyDate, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", pIntDepartment_ID, DbType.Int32, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_StockTallyGetData", CommandType.StoredProcedure);
            return DTab;
        }


        public StockTallyProperty Save(StockTallyProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("STOCKTALLY_ID", pClsProperty.STOCKTALLY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("STOCK_ID", pClsProperty.STOCK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHAPE_ID", pClsProperty.SHAPE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SIZE_ID", pClsProperty.SIZE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("CLARITY_ID", pClsProperty.CLARITY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("BALANCECARAT", pClsProperty.BALANCECARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ACTUALCARAT", pClsProperty.ACTUALCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("PLUSCARAT", pClsProperty.PLUSCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("LOSSCARAT", pClsProperty.LOSSCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SALEPRICEPERCARAT", pClsProperty.SALERATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("OLDSALEPRICEPERCARAT", pClsProperty.OLDSALERATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("STOCKTALLYDATE", pClsProperty.STOCKTALLYDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("ISSALERATEUPDATE", pClsProperty.ISSALERATEUPDATE, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);

                Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_StockTallySave", CommandType.StoredProcedure);

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

        public StockTallyProperty Delete(StockTallyProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("STOCKTALLY_ID", pClsProperty.STOCKTALLY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_StockTallyDelete", CommandType.StoredProcedure);

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
