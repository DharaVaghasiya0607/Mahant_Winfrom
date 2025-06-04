using BusLib.TableName;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Config = BusLib.Configuration.BOConfiguration;
using System.Collections;


namespace BusLib.Account
{
    public class BOACC_PolishRoughInward
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        

        public DataTable Fill(int pIntVoucherNo, string pIntVoucherDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("VOUCHERNO", pIntVoucherNo, DbType.Int32);
            Ope.AddParams("VOUCHERDATE", pIntVoucherDate, DbType.String);
            Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.Int32);
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Acc_PolishRoughInwardEntryGetData", CommandType.StoredProcedure);
            return DTab;
        }
        public Int64 FindVoucherNo(string pStrFinYearID, string pStrBookType)
        {
            string StrSql = "";
            Ope.ClearParams();
            Int64 IntNewID = Ope.FindNewID(Config.ConnectionString, Config.ProviderName, "Acc_PolishRoughInward", "MAX(VOUCHERNO)", StrSql);

            return IntNewID;
        }


        public Acc_PolishRoughInwardProperty Save(Acc_PolishRoughInwardProperty pClsProperty)
        {
            try
            {
                Ope.AddParams("ID", pClsProperty.ID, DbType.Int32);

                Ope.AddParams("VOUCHERNO", pClsProperty.VoucherNo, DbType.Int32);
                Ope.AddParams("VOUCHERDATE", pClsProperty.VoucherDate, DbType.Date);
                Ope.AddParams("PARTYID", pClsProperty.PartyId, DbType.Guid);
                Ope.AddParams("ITEMNAME", pClsProperty.ItemName, DbType.String);
                Ope.AddParams("ROUGHCARAT", pClsProperty.RoughCarat, DbType.Double);
                Ope.AddParams("ROUGHRATE", pClsProperty.RoughRate, DbType.Double); 
                Ope.AddParams("ROUGHAMTRS", pClsProperty.RoughAmtRs, DbType.Double);
                Ope.AddParams("REGAMTRS", pClsProperty.RegAmtRs, DbType.Double);
                Ope.AddParams("REGRATE", pClsProperty.RegRate, DbType.Double);
                Ope.AddParams("REGCARAT", pClsProperty.RegCarat, DbType.Double); 
                Ope.AddParams("MFGAMTRS", pClsProperty.MfgAmtRs, DbType.Double);
                Ope.AddParams("MFGCARAT", pClsProperty.MfgCarat, DbType.Double);
                Ope.AddParams("POLISHPCARAT", pClsProperty.PolishPCarat, DbType.Double);
                Ope.AddParams("LABOURAMT", pClsProperty.LabourAmt, DbType.Double);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String);
                Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.Int32);
                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);


                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Acc_PolishRoughInwardEntrySave", CommandType.StoredProcedure);

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
                //BusLib.BOException.Save(this.GetType().Name, new System.Diagnostics.StackFrame(1, true).GetMethod().ToString(), new System.Diagnostics.StackFrame(0, true).GetFileLineNumber(), ex.Message, ex.ToString());
            }
            return pClsProperty;
        }
        public Acc_PolishRoughInwardProperty RoughDelete(Acc_PolishRoughInwardProperty pClsProperty)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("ID", pClsProperty.ID, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);

            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

            Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

            ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Acc_PolishRoughInwardEntryDelete", CommandType.StoredProcedure);
            if (AL.Count != 0)
            {
                pClsProperty.ReturnValue = Val.ToString(AL[0]);
                pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
            }
            return pClsProperty;
        }

        public DataTable MaxVoucherNoDateWise(string pIntVoucherDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.Int32);
            Ope.AddParams("VOUCHERDATE", pIntVoucherDate, DbType.String);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Acc_PolishRoughInwardGetVno_Max", CommandType.StoredProcedure);
            return DTab;
        }
    }
}
