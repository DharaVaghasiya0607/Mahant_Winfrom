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
    public class BOTRN_StockTally
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public TrnStockTallyProperty Save(TrnStockTallyProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();

                Ope.AddParams("STOCKTALLYDATE", pClsProperty.STOCKTALLYDATE, DbType.Date, ParameterDirection.Input);
                //Ope.AddParams("PACKET_ID", pClsProperty.PACKET_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("PARTYSTOCKNO", pClsProperty.PARTYSTOCKNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("STOCK_ID", pClsProperty.STOCK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("GIACONTROLNO", pClsProperty.GIACONTROLNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("RFIDTAGNO", pClsProperty.RFIDTAGNO, DbType.String, ParameterDirection.Input); 
                Ope.AddParams("PROCESS_ID", pClsProperty.PROCESS_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("CARAT", pClsProperty.CARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FOUNDSTATUS", pClsProperty.FOUNDSTATUS, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PROCESSNAME", pClsProperty.STATUS, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SERIALNO", pClsProperty.SERIALNO, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MAINCATAGORY", pClsProperty.MAINCATAGORY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BOXNAME", pClsProperty.BOXNAME, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_StockTallySave", CommandType.StoredProcedure);

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

        public TrnStockTallyProperty Delete(TrnStockTallyProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();

                Ope.AddParams("STOCKTALLYDATE", pClsProperty.STOCKTALLYDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("MAINCATAGORY", pClsProperty.MAINCATAGORY, DbType.Date, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_StockTalllyDelete", CommandType.StoredProcedure);

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


        public DataSet GetData(string pStrDate , string StockType)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("STOCKDATE", pStrDate, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STOCKTYPE", StockType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Trn_StockTallyGetData", CommandType.StoredProcedure);

            return DS;
        }

        public DataTable GetPartyStockNofromGIAControlNo(string pStrGIAControlNo) //#K : 10-11-2020
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("GIACONTROLNO", pStrGIAControlNo, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_StockGetPartyStockNoFromGIAControlNo", CommandType.StoredProcedure);
            return DTab;
        }
        public DataTable GetPartyStockNofromStockNo(string pStrStockNo, string pStrRFIDTagNo) //#K : 10-11-2020
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("STOCKNO", pStrStockNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("RFIDTAGNO", pStrRFIDTagNo, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_StockGetPartyStockNoFromStockNo", CommandType.StoredProcedure);
            return DTab;
        }
        public DataTable GetPartyStockNofromRFIDTagNo(string pStrRFIDTagNo) //#P : 08-02-2022
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("RFIDTAGNO", pStrRFIDTagNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("OPE", "SEARCHRFIDTAGNOWISE_INDIVIDUAL", DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_StockGetPartyStockNoFromRFIDTagNo", CommandType.StoredProcedure);
            return DTab;
        }
        public String UpdateBoxID(string pStreXml,int Box_ID,string BoxName,string StockDate) //#hinal: 16-01-2022
        {
            Ope.ClearParams();
            Ope.AddParams("XMLDETSTR", pStreXml, DbType.Xml, ParameterDirection.Input);
            Ope.AddParams("BOXNAME",BoxName,DbType.String,ParameterDirection.Input);
            Ope.AddParams("BOX_ID",Box_ID,DbType.Int32,ParameterDirection.Input);
            Ope.AddParams("STOCKDATE", StockDate, DbType.String, ParameterDirection.Input);
            Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

            ArrayList AL= Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_StockTallyUpdateBoxID", CommandType.StoredProcedure); 
                
            string str=Val.ToString(AL[0]);

            return str;

            }
       
    }
}
