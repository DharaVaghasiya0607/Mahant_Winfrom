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
    public class BOMST_DelliveryChallan
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();


        public DataTable Fill()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "ACC_DeliveryChallanGatData", CommandType.StoredProcedure);
            return DTab;
        }
        public DataTable FindVoucherNoNew()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            if (Config.gStrLoginSection == "B")
            {
                Ope.AddParams("FinYear_Id", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);
                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "GetMaxChallanNo", CommandType.StoredProcedure);
            }
            else
            {
                Ope.AddParams("FinYear_Id", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);
                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "GetMaxChallanNo", CommandType.StoredProcedure);
            }
            return DTab;
        }
        public DeliveryChallanProperty Save(DeliveryChallanProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();

                Ope.AddParams("DATE", pClsProperty.DATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("DELIVERY_ID", pClsProperty.DELIVERY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("CARAT", pClsProperty.CARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("CHALLANNO", pClsProperty.CHALLANNO, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "ACC_DeliveryChallanSave", CommandType.StoredProcedure);


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
        public DataTable Print(string pStrChallano)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("CHALLANNO", pStrChallano, DbType.String, ParameterDirection.Input);
            //Ope.AddParams("DELIVERY_ID", pStrDelivery_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Acc_DeliveryChallanPrint", CommandType.StoredProcedure);
            return DTab;
        }
        public DeliveryChallanProperty Delete(DeliveryChallanProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("DELIVERY_ID", pClsProperty.DELIVERY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "ACC_DeliveryChallanDelete", CommandType.StoredProcedure);

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