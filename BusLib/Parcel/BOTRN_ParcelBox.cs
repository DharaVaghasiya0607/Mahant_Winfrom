using AxonDataLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using BusLib.TableName;
using System.Collections;

namespace BusLib.Parcel
{
    public class BOTRN_Box
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable Fill()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID , DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BoxGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public ParcelBoxMasterProperty Save(ParcelBoxMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();

                Ope.AddParams("STOCK_ID", pClsProperty.STOCK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("DEPARTMENT_ID", pClsProperty.DEPARTMENT_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHAPE_ID", pClsProperty.SHAPE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MIXSIZE_ID", pClsProperty.MIXSIZE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MIXSIZENAME", pClsProperty.MIXSIZENAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MIXCLARITY_ID", pClsProperty.MIXCLARITY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MIXCLARITYNAME", pClsProperty.MIXCLARITYNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("OPENINGCARAT", pClsProperty.OPENINGCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("OPENINGPRICEPERCARAT", pClsProperty.OPENINGPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("OPENINGAMOUNT", pClsProperty.OPENINGAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("MFGPRICEPERCARAT", pClsProperty.MFGPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("COSTPRICEPERCARAT", pClsProperty.COSTPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SALEPRICEPERCARAT", pClsProperty.SALEPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EXPPRICEPERCARAT", pClsProperty.EXPPRICEPERCARAT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("PARCELGROUPNO", pClsProperty.PARCELGROUPNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PARCELSEQNO", pClsProperty.PARCELSEQNO, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ISSALEPRICEPERCARAT", pClsProperty.ISSALEPRICEPERCARAT, DbType.Int32, ParameterDirection.Input);

                
                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_BoxInsertUpdate", CommandType.StoredProcedure);

                if (AL.Count != 0)
                {
                    pClsProperty.RETURNVALUE = Val.ToString(AL[0]);
                    pClsProperty.RETURNMESSAGETYPE = Val.ToString(AL[1]);
                    pClsProperty.RETURNMESSAGEDESC = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.RETURNVALUE = "";
                pClsProperty.RETURNMESSAGETYPE = "FAIL";
                pClsProperty.RETURNMESSAGEDESC = ex.Message;

            }
            return pClsProperty;

        }

        public ParcelBoxMasterProperty Delete(ParcelBoxMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("STOCK_ID", pClsProperty.STOCK_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_BoxDelete", CommandType.StoredProcedure);

                if (AL.Count != 0)
                {
                    pClsProperty.RETURNVALUE = Val.ToString(AL[0]);
                    pClsProperty.RETURNMESSAGETYPE = Val.ToString(AL[1]);
                    pClsProperty.RETURNMESSAGEDESC = Val.ToString(AL[2]);
                }

            }
            catch (System.Exception ex)
            {
                pClsProperty.RETURNVALUE = "";
                pClsProperty.RETURNMESSAGETYPE = "FAIL";
                pClsProperty.RETURNMESSAGEDESC = ex.Message;

            }
            return pClsProperty;

        }

        public string DirectParcelSave(Guid pStock_ID, double pTotalCarat, double pTotalRate)
        {

            string RETURNMESSAGETYPE = "";
            try
            {

                Ope.ClearParams();
                Ope.AddParams("STOCK_ID", pStock_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("TOTALCARAT", pTotalCarat, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TOTALRATE", pTotalRate, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_DirectParcelInsertUpdate", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    RETURNMESSAGETYPE = Val.ToString(AL[0]);
                }

            }
            catch (Exception ex)
            {
                RETURNMESSAGETYPE = "FAIL";
            }
            return RETURNMESSAGETYPE;

        }

        public DataTable BoxMasterGetData(string pStrFromDate, string pStrToDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("FROMDATE", Val.SqlDate(pStrFromDate), DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", Val.SqlDate(pStrToDate), DbType.Date, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BoxMaster_GetData", CommandType.StoredProcedure);
            return DTab;
        }

        public ParcelBoxMasterProperty BoxMasterSave(ParcelBoxMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("BOX_ID", pClsProperty.BOX_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("BOXNAME", pClsProperty.BOXNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MIXCLARITY_ID", pClsProperty.MIXCLARITY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MIXSIZE_ID", pClsProperty.MIXSIZE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SHAPE_ID", pClsProperty.SHAPE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ISACTIVE", pClsProperty.ISACTIVE, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("RATE", pClsProperty.RATE, DbType.Double, ParameterDirection.Input);


                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_BoxMaster_InsertUpdate", CommandType.StoredProcedure);

                if (AL.Count != 0)
                {
                    pClsProperty.RETURNVALUE = Val.ToString(AL[0]);
                    pClsProperty.RETURNMESSAGETYPE = Val.ToString(AL[1]);
                    pClsProperty.RETURNMESSAGEDESC = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.RETURNVALUE = "";
                pClsProperty.RETURNMESSAGETYPE = "FAIL";
                pClsProperty.RETURNMESSAGEDESC = ex.Message;

            }
            return pClsProperty;
        }

    }
}
