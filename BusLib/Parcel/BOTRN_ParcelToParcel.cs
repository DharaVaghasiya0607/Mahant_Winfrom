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
    public class BOTRN_ParcelToParcel
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable GetStoneDetailForParcelToParcel(Guid pGuidStock_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("STOCK_ID", pGuidStock_ID, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_ParcelToParcelGetData", CommandType.StoredProcedure);
            return DTab;
        }


        public ParcelToParcelProperty Save(ParcelToParcelProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();

                Ope.AddParams("TYPE", pClsProperty.TYPE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("FROMSTOCK_ID", pClsProperty.FROMSTOCK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("TOSTOCK_ID", pClsProperty.TOSTOCK_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("FROMSTOCKNO", pClsProperty.FROMSTOCKNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TOSTOCKNO", pClsProperty.TOSTOCKNO, DbType.String, ParameterDirection.Input);

                Ope.AddParams("TOSHAPE_ID", pClsProperty.TOSHAPE_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("TOMIXSIZE_ID", pClsProperty.TOSIZE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TOMIXSIZENAME", pClsProperty.TOSIZENAME, DbType.String, ParameterDirection.Input);

                Ope.AddParams("TOMIXCLARITY_ID", pClsProperty.TOCLARITY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TOMIXCLARITYNAME", pClsProperty.TOCLARITYNAME, DbType.String, ParameterDirection.Input);

                Ope.AddParams("TODEPARTMENT_ID", pClsProperty.TODEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("FROMCARATBEFORE", pClsProperty.FROMCARATBEFORE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FROMCARATAFTER", pClsProperty.FROMCARATAFTER, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("FROMPRICEPERCARATBEFORE", pClsProperty.FROMPRICEPERCARATBEFORE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FROMPRICEPERCARATAFTER", pClsProperty.FROMPRICEPERCARATAFTER, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("FROMAMOUNTBEFORE", pClsProperty.FROMAMOUNTBEFORE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FROMAMOUNTAFTER", pClsProperty.FROMAMOUNTAFTER, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("TOCARATBEFORE", pClsProperty.TOCARATBEFORE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TOCARATAFTER", pClsProperty.TOCARATAFTER, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("TOPRICEPERCARATBEFORE", pClsProperty.TOPRICEPERCARATBEFORE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TOPRICEPERCARATAFTER", pClsProperty.TOPRICEPERCARATAFTER, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("TOAMOUNTBEFORE", pClsProperty.TOAMOUNTBEFORE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TOAMOUNTAFTER", pClsProperty.TOAMOUNTAFTER, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("TRANSFERCARAT", pClsProperty.TRANSFERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TRANSFERPRICEPERCARAT", pClsProperty.TRANSFERPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TRAMSFERAMOUNT", pClsProperty.TRAMSFERAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_ParcelToParcelTransferInsertUpdate", CommandType.StoredProcedure);

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
