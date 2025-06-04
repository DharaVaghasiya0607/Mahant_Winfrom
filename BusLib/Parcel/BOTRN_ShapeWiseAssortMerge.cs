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

namespace BusLib.Master
{
    public class BOTRN_ShapeWiseAssortMerge
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable BombayShapeWiseAssortmentMergeGetData(string pStrTrasferNo, string pStrStatus, Int64 pIntMergeSummary_ID, Guid pGuidUser_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", "MERGESUMMARY", DbType.String, ParameterDirection.Input);
            Ope.AddParams("TRANSFERNO", pStrTrasferNo, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("STATUS", pStrStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MERGESUMMARY_ID", pIntMergeSummary_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("USER_ID", pGuidUser_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BombayShapeWiseAssortmentGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable BombayAssortmentGetData(string pStrOpe, Int64 pIntMergeSummaryID, Int32 pIntShape_ID, Int32 pIntDepartment_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MERGESUMMARY_ID", pIntMergeSummaryID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", pIntShape_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", pIntDepartment_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BombayShapeWiseAssortmentGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public BombayShapeWiseAssortMergeProperty Save(BombayShapeWiseAssortMergeProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MERGESUMMARY_ID", pClsProperty.MERGESUMMARY_ID, DbType.UInt32, ParameterDirection.Input);
                Ope.AddParams("FROMSHAPE_ID", pClsProperty.FROMSHAPE_ID, DbType.UInt32, ParameterDirection.Input);
                Ope.AddParams("TOSHAPE_ID", pClsProperty.TOSHAPE_ID, DbType.UInt32, ParameterDirection.Input);
                Ope.AddParams("FROMDEPARTMENT_ID", pClsProperty.FROMDEPARTMENT_ID, DbType.UInt32, ParameterDirection.Input);
                Ope.AddParams("TODEPARTMENT_ID", pClsProperty.TODEPARTMENT_ID, DbType.UInt32, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_BombayShapeWiseAssortmentSave", CommandType.StoredProcedure);

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
