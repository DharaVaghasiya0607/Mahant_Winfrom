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
using System.Data.SqlClient;
using BusLib.Master;
using BusLib.Configuration;
using System.Security.Cryptography;
using System.IO;

namespace BusLib.Transaction
{
    public class BOTRN_MemoEntry
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public string GetMaxMemoNo(string pStrType)
        {
            string StrType = "";
            try
            {

                Ope.ClearParams();
                Ope.AddParams("ID_TYPE", pStrType, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Input);
                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_Maximum_ID_GenerateDirectMax", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    StrType = Val.ToString(AL[0]);
                }
            }
            catch (System.Exception ex)
            {
                StrType = string.Empty;
            }
            return StrType;
        }

        public void UpdateMemoNo(int PMemoNo)
        {
            string StrQuery = "";

            StrQuery = "UPDATE MST_MAXIMUM WITH(ROWLOCK) SET MAXID =" + PMemoNo + "WHERE ID_TYPE = 'MEMONO' and YY = " + DateTime.Today.Year + " and MM = " + DateTime.Today.Month;
            Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);          
        }

        public MemoEntryProperty UpdateAPartEntry(SqlConnection pConConnection, MemoEntryProperty pClsProperty, string StrMemoEntry_OldDB)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("APPROVEMEMO_ID", pClsProperty.APPROVEMEMO_ID_A, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("APARTAPPROVAL", pClsProperty.APARTAPPROVAL, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("PROCESS_ID", pClsProperty.PROCESS_ID, DbType.Int32, ParameterDirection.Input);

                //Ope.AddParams("TRNNO_OLDDB", pClsProperty.TrnNo_OldDB, DbType.String, ParameterDirection.Input);
                Ope.AddParams("XMLSTR_OLDDB", StrMemoEntry_OldDB, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(pConConnection, Config.ProviderName, "Trn_MemoEntryUpdate", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnValueJanged = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;
            }
            return pClsProperty;
        }

        public MemoEntryProperty UpdateAPartEntryStatus(SqlConnection pConConnection, MemoEntryProperty pClsProperty, string StrMemoEntry_OldDB) //cHNG : #P : 21-04-2022
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("APPROVEMEMO_ID", pClsProperty.APPROVEMEMO_ID_A, DbType.String, ParameterDirection.Input);
                Ope.AddParams("APARTAPPROVAL", pClsProperty.APARTAPPROVAL, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("PROCESS_ID", pClsProperty.PROCESS_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("XMLSTR_OLDDB", StrMemoEntry_OldDB, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ORDERMEMO_ID", pClsProperty.ORDERMEMO_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(pConConnection, Config.ProviderName, "Trn_MemoEntryUpdateStatus_A", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnValueJanged = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;
            }
            return pClsProperty;
        }

       



        //ADD SHIV 07-09-2022
        public MemoEntryProperty UpdateBroker(SqlConnection pConConnection, MemoEntryProperty pClsProperty) //S 08-09-2022
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("BROKER_ID", pClsProperty.BROKER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("BROKERBASEPER", pClsProperty.BROKERBASEPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKERPROFITPER", pClsProperty.BROKERPROFITPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKERAMOUNTFE", pClsProperty.BrokerAmountFE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BrokerAmount", pClsProperty.BrokerAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADAT_ID", pClsProperty.ADAT_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ADATPER", pClsProperty.ADATPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADATAMTFE", pClsProperty.AdatAmtFE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BASEBROKERAGEEXCRATE", pClsProperty.BASEBROKERAGEEXCRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADATEXCRATE", pClsProperty.ADATEXCRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GRFREIGHT", pClsProperty.GRFREIGHT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROTDSPER", pClsProperty.BROTDSPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROTDSRS", pClsProperty.BROTDSRS, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROTOTALAMT", pClsProperty.BROTOTALAMT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("IS_BROTDS", pClsProperty.IS_BROTDS, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("BROCGSTPER", pClsProperty.BROCGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROSGSTRS", pClsProperty.BROSGSTRS, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROIGSTPER", pClsProperty.BROIGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROCGSTRS", pClsProperty.BROCGSTRS, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("TCSPER", pClsProperty.TCSPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TCSAMOUNT", pClsProperty.TCSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FTCSAMOUNT", pClsProperty.FTCSAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("TERMS_ID", pClsProperty.TERMS_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TERMSDAYS", pClsProperty.TERMSDAYS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TERMSPER", pClsProperty.TERMSPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TERMSDATE", pClsProperty.TERMSDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("FNETAMOUNT", pClsProperty.FNETAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(pConConnection, Config.ProviderName, "TRN_MEMOBROKERUPDATE", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnValueJanged = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;
            }
            return pClsProperty;
        }

        public MemoEntryProperty UpdateBPartEntry(SqlConnection pConConnection, MemoEntryProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("APPROVEMEMO_ID", pClsProperty.APPROVEMEMO_ID_A, DbType.String, ParameterDirection.Input);
                Ope.AddParams("IS_APARTRECEIVE", pClsProperty.IS_APARTRECEIVE, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("APPROVEMEMONO", pClsProperty.APPROVEMEMONO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PROCESS_ID", pClsProperty.PROCESS_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ORDERMEMOID", pClsProperty.ORDERMEMO_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);


                
                ArrayList AL = Ope.ExeNonQueryWithOutParameter(pConConnection, Config.ProviderName, "Trn_MemoEntryUpdateStatus", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnValueJanged = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;
            }
            return pClsProperty;
        }

        public DataRow GetCurrency()
        {
            string StrQuery = "";
            Ope.ClearParams();
            StrQuery = "select Currency_ID,CurrencyName from MST_Currency where CurrencyCode = 'USD'";
            DataRow Dr = Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
            return Dr;
        }

        public DataRow GetExcRate()
        {
            string StrQuery = "";
            Ope.ClearParams();
            StrQuery = "select Top 1 ExcRate from TRN_DailyRate With(NoLock) Where Currency_ID = '1' order by EntryDate Desc";
            DataRow Der = Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
            return Der;
        }


        public MemoEntryProperty UpdateBPartParcelEntry(SqlConnection pConConnection, MemoEntryProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("IS_APARTRECEIVE", pClsProperty.IS_APARTRECEIVE, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);


                ArrayList AL = Ope.ExeNonQueryWithOutParameter(pConConnection, Config.ProviderName, "Trn_MemoEntryParcelDeleteAfterUpdateStatus", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnValueJanged = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;
            }
            return pClsProperty;
        }

        public MemoEntryProperty SaveMemoEntry(SqlConnection pConConnection, MemoEntryProperty pClsProperty, string StrMemoEntryDetXML, string pStrMode, string StrMemoEntry_OldDB, string StrMemoEntryDetXMLParcel)
        {
            try
            {

                Ope.ClearParams();
                DataTable DTab = new DataTable();

                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("JANGEDNOSTR", pClsProperty.JANGEDNOSTR, DbType.String, ParameterDirection.Input);

                Ope.AddParams("MEMONO", pClsProperty.MEMONO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMOTYPE", pClsProperty.MEMOTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMODATE", pClsProperty.MEMODATE, DbType.Date, ParameterDirection.Input);

                Ope.AddParams("BILLINGPARTY_ID", pClsProperty.BILLINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("BILLINGPARTYNAME", pClsProperty.BILLINGPARTNAME, DbType.String, ParameterDirection.Input);

                Ope.AddParams("SHIPPINGPARTY_ID", pClsProperty.SHIPPINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("BROKER_ID", pClsProperty.BROKER_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("THROUGH", pClsProperty.THROUGH, DbType.String, ParameterDirection.Input);

                Ope.AddParams("BROKRAGEBASEPER", pClsProperty.BROKERBASEPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKRAGEPROFITPER", pClsProperty.BROKERPROFITPER, DbType.Double, ParameterDirection.Input);


                Ope.AddParams("ADAT_ID", pClsProperty.ADAT_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ADATPER", pClsProperty.ADATPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SOLDPARTY_ID", pClsProperty.SOLDPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SELLER_ID", pClsProperty.SELLER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("TERMS_ID", pClsProperty.TERMS_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TERMSDAYS", pClsProperty.TERMSDAYS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TERMSPER", pClsProperty.TERMSPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TERMSDATE", pClsProperty.TERMSDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("CURRENCY_ID", pClsProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EXCRATE", pClsProperty.EXCRATE, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("DELIVERYTYPE", pClsProperty.DELIVERYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PAYMENTMODE", pClsProperty.PAYMENTMODE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("BILLINGADDRESS1", pClsProperty.BILLINGADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGADDRESS2", pClsProperty.BILLINGADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGADDRESS3", pClsProperty.BILLINGADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGCOUNTRY_ID", pClsProperty.BILLINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("BILLINGSTATE", pClsProperty.BILLINGSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGCITY", pClsProperty.BILLINGCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGZIPCODE", pClsProperty.BILLINGZIPCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS1", pClsProperty.SHIPPINGADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS2", pClsProperty.SHIPPINGADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS3", pClsProperty.SHIPPINGADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGCOUNTRY_ID", pClsProperty.SHIPPINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGSTATE", pClsProperty.SHIPPINGSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGCITY", pClsProperty.SHIPPINGCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGZIPCODE", pClsProperty.SHIPPINGZIPCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TOTALPCS", pClsProperty.TOTALPCS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TOTALCARAT", pClsProperty.TOTALCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TOTALAVGDISC", pClsProperty.TOTALAVGDISC, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TOTALAVGRATE", pClsProperty.TOTALAVGRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GROSSAMOUNT", pClsProperty.GROSSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DISCOUNTPER", pClsProperty.DISCOUNTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DISCOUNTAMOUNT", pClsProperty.DISCOUNTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INSURANCEPER", pClsProperty.INSURANCEPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INSURANCEAMOUNT", pClsProperty.INSURANCEAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGPER", pClsProperty.SHIPPINGPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGAMOUNT", pClsProperty.SHIPPINGAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GSTPER", pClsProperty.GSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GSTAMOUNT", pClsProperty.GSTAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("IGSTPER", pClsProperty.IGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("IGSTAMOUNT", pClsProperty.IGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FIGSTAMOUNT", pClsProperty.FIGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("CGSTPER", pClsProperty.CGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("CGSTAMOUNT", pClsProperty.CGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FCGSTAMOUNT", pClsProperty.FCGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SGSTPER", pClsProperty.SGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SGSTAMOUNT", pClsProperty.SGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FSGSTAMOUNT", pClsProperty.FSGSTAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("NETAMOUNT", pClsProperty.NETAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FGROSSAMOUNT", pClsProperty.FGROSSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FDISCOUNTAMOUNT", pClsProperty.FDISCOUNTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FINSURANCEAMOUNT", pClsProperty.FINSURANCEAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FSHIPPINGAMOUNT", pClsProperty.FSHIPPINGAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FGSTAMOUNT", pClsProperty.FGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FNETAMOUNT", pClsProperty.FNETAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);
                Ope.AddParams("STATUS", pClsProperty.STATUS, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SOURCE", pClsProperty.SOURCE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PROCESS_ID", pClsProperty.PROCESS_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("TRANSPORTNAME", pClsProperty.TRANSPORTNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PLACEOFSUPPLY", pClsProperty.PLACEOFSUPPLY, DbType.String, ParameterDirection.Input);

                Ope.AddParams("COMPANYBANK_ID", pClsProperty.COMPANYBANK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PARTYBANK_ID", pClsProperty.PARTYBANK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("COURIER_ID", pClsProperty.COURIER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("AIRFREIGHT", pClsProperty.AIRFREIGHT_ID, DbType.Guid, ParameterDirection.Input);
                //Ope.AddParams("AIRFREIGHT", pClsProperty.AIRFREIGHT, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("PLACEOFRECEIPTBYPRECARRIER", pClsProperty.PLACEOFRECEIPTBYPRECARRIER, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PORTOFLOADING", pClsProperty.PORTOFLOADING, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PORTOFDISCHARGE", pClsProperty.PORTOFDISCHARGE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALDESTINATION", pClsProperty.FINALDESTINATION, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COUNTRYOFORIGIN", pClsProperty.COUNTRYOFORIGIN, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COUNTRYOFFINALDESTINATION", pClsProperty.COUNTRYOFFINALDESTINATION, DbType.String, ParameterDirection.Input);
                Ope.AddParams("INTERMIDATORYBANK", pClsProperty.INTERMIDATORYBANK, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PRECARRIERBY", pClsProperty.PRECARRIERBY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CONSIGNEE_ID", pClsProperty.Consignee_Id, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);
                //Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR", pClsProperty.FINYEAR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("VOUCHERNOSTR", pClsProperty.VOUCHERNOSTR, DbType.String, ParameterDirection.Input);

                Ope.AddParams("XMLDETSTR", StrMemoEntryDetXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MODE", pStrMode, DbType.String, ParameterDirection.Input);

                Ope.AddParams("BILLTYPE", pClsProperty.BILLTYPE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("INSURANCETYPE", pClsProperty.INSURANCETYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("GROSSWEIGHT", pClsProperty.GROSSWEIGHT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("OTHERSTONEPARTY_ID", pClsProperty.OTHERSTONEPARTY_ID, DbType.Guid, ParameterDirection.Input); //ADD MILAN(04-02-20021)
                Ope.AddParams("OTHERSTONEPARTYNAME", pClsProperty.OTHERSTONEPARTYNAME, DbType.String, ParameterDirection.Input);//ADD MILAN ""

                // #D: 19-08-2020
                Ope.AddParams("CONSIGNEE", pClsProperty.CONSIGNEE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ADDRESSTYPE", pClsProperty.ADDRESSTYPE, DbType.String, ParameterDirection.Input);
                // #D: 19-08-2020

                //#K : 05122020
                Ope.AddParams("SOAPPROVAL", pClsProperty.ORDERAPPROVAL, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TCSPER", pClsProperty.TCSPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TCSAMOUNT", pClsProperty.TCSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FTCSAMOUNT", pClsProperty.FTCSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FROUNDOFFAMOUNT", pClsProperty.FROUNDOFFAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ROUNDOFFTPYE", pClsProperty.ROUNDOFFTPYE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("CUSTOMEBROKER", pClsProperty.CustomeBroker, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CUSTOMEDESIGNATION", pClsProperty.CustomeDesignation, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CUSTOMEIDCARD", pClsProperty.CustomeIDCard, DbType.String, ParameterDirection.Input);

                Ope.AddParams("BACKADDLESS", pClsProperty.BACKADDLESS, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TERMSADDLESSPER", pClsProperty.TERMSADDLESSPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BLINDADDLESSPER", pClsProperty.BLINDADDLESSPER, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("FINALBUYER_ID", pClsProperty.FINALBUYER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("FINALADDRESS1", pClsProperty.FINALBUYERADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALADDRESS2", pClsProperty.FINALBUYERADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALADDRESS3", pClsProperty.FINALBUYERADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALCOUNTRY_ID", pClsProperty.FINALBUYERCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("FINALSTATE", pClsProperty.FINALBUYERSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALCITY", pClsProperty.FINALBUYERCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALZIPCODE", pClsProperty.FINALBUYERZIPCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ISCONSINGEE", pClsProperty.ISCONSINGEE, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("INVOICENO", pClsProperty.INVOICENO, DbType.String, ParameterDirection.Input);

                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("EXPINVOICEAMT", pClsProperty.ExpInvoiceAmt, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EXPINVOICEAMTFE", pClsProperty.ExpInvoiceAmtFE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("APPROVEMEMO_ID", pClsProperty.APPROVEMEMO_ID_A, DbType.String, ParameterDirection.Input);

                // Add paramter by khushbu 19-08-21
                Ope.AddParams("ORG_EXCRATE", pClsProperty.ORG_EXCRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADDLESS_EXCRATE", pClsProperty.ADDLESS_EXCRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADATAMT", pClsProperty.AdatAmt, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADATAMTFE", pClsProperty.AdatAmtFE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKERAMT", pClsProperty.BrokerAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKERAMTFE", pClsProperty.BrokerAmountFE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("STKAMTFE", pClsProperty.StkAmtFE, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("ORDERMEMO_ID", pClsProperty.ORDERMEMO_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ORDERJANGEDNO", pClsProperty.ORDERJANGEDNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLFORMAT", pClsProperty.BILLFORMAT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BRNO", pClsProperty.BRNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYTYPE", pClsProperty.ENTRYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("XMLSTR_OLDDB", StrMemoEntry_OldDB, DbType.Xml, ParameterDirection.Input);

                //Add shiv 23-02-22
                Ope.AddParams("IS_APARTRECEIVE", pClsProperty.IS_APARTRECEIVE, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("APARTAPPROVAL", pClsProperty.APARTAPPROVAL, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ACCTYPE", pClsProperty.ACCTYPE, DbType.String, ParameterDirection.Input);

                //Add shiv 27-05-2022
                Ope.AddParams("BROKERAGEEXCRATE", pClsProperty.BASEBROKERAGEEXCRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADATEXCRATE", pClsProperty.ADATEXCRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GRFREIGHT", pClsProperty.GRFREIGHT, DbType.Double, ParameterDirection.Input);

                //Add shiv 03-06-2022
                Ope.AddParams("BROTDSPER", pClsProperty.BROTDSPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROTDSRS", pClsProperty.BROTDSRS, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROTOTALAMT", pClsProperty.BROTOTALAMT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("IS_BROTDS", pClsProperty.IS_BROTDS, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("IS_OUTSIDESTONE", pClsProperty.IS_OUTSIDESTONE, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("BROCGSTPER", pClsProperty.BROCGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROCGSTRS", pClsProperty.BROCGSTRS, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROSGSTPER", pClsProperty.BROSGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROSGSTRS", pClsProperty.BROSGSTRS, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROIGSTPER", pClsProperty.BROIGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROIGSTRS", pClsProperty.BROIGSTRS, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("IS_BROGST", pClsProperty.IS_BROGST, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTRPARCEL", StrMemoEntryDetXMLParcel, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("HAWBNO", pClsProperty.HAWBNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MAWBNO", pClsProperty.MAWBNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SBNo", pClsProperty.SBNo, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ConsignmentRefNo", pClsProperty.ConsignmentRefNo, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                //ArrayList AL = Ope.ExeNonQueryWithOutParameter(pConConnection, Config.ProviderName, "Trn_MemoEntrySaveNew_SaleDelivery", CommandType.StoredProcedure);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(pConConnection, Config.ProviderName, "Trn_MemoEntrySaveNew", CommandType.StoredProcedure);
                //ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_MemoEntrySaveNew", CommandType.StoredProcedure);

                //ArrayList AL = Ope.ExeNonQueryWithOutParameter(pConConnection, Config.ProviderName, "Trn_MemoEntrySaveNew_020722", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnValueJanged = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;
        }


        public MemoEntryProperty SaveMemoEntry_B(SqlConnection pConConnection, MemoEntryProperty pClsProperty, string StrMemoEntryDetXML, string pStrMode, string StrMemoEntry_OldDB)
        {
            try
            {
                Ope.ClearParams();
                DataTable DTab = new DataTable();

                Ope.AddParams("APPROVEMEMO_ID", pClsProperty.APPROVEMEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("APPROVEMEMONO", pClsProperty.APPROVEMEMONO, DbType.String, ParameterDirection.Input);

                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("JANGEDNOSTR", pClsProperty.JANGEDNOSTR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMONO", pClsProperty.MEMONO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMOTYPE", pClsProperty.MEMOTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMODATE", pClsProperty.MEMODATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("BILLINGPARTY_ID", pClsProperty.BILLINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGPARTY_ID", pClsProperty.SHIPPINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("BROKER_ID", pClsProperty.BROKER_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("BROKRAGEBASEPER", pClsProperty.BROKERBASEPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKRAGEPROFITPER", pClsProperty.BROKERPROFITPER, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("ADAT_ID", pClsProperty.ADAT_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ADATPER", pClsProperty.ADATPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SOLDPARTY_ID", pClsProperty.SOLDPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SELLER_ID", pClsProperty.SELLER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("TERMS_ID", pClsProperty.TERMS_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TERMSDAYS", pClsProperty.TERMSDAYS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TERMSPER", pClsProperty.TERMSPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TERMSDATE", pClsProperty.TERMSDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("CURRENCY_ID", pClsProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EXCRATE", pClsProperty.EXCRATE, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("DELIVERYTYPE", pClsProperty.DELIVERYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PAYMENTMODE", pClsProperty.PAYMENTMODE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("BILLINGADDRESS1", pClsProperty.BILLINGADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGADDRESS2", pClsProperty.BILLINGADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGADDRESS3", pClsProperty.BILLINGADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGCOUNTRY_ID", pClsProperty.BILLINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("BILLINGSTATE", pClsProperty.BILLINGSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGCITY", pClsProperty.BILLINGCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGZIPCODE", pClsProperty.BILLINGZIPCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS1", pClsProperty.SHIPPINGADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS2", pClsProperty.SHIPPINGADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS3", pClsProperty.SHIPPINGADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGCOUNTRY_ID", pClsProperty.SHIPPINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGSTATE", pClsProperty.SHIPPINGSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGCITY", pClsProperty.SHIPPINGCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGZIPCODE", pClsProperty.SHIPPINGZIPCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TOTALPCS", pClsProperty.TOTALPCS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TOTALCARAT", pClsProperty.TOTALCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TOTALAVGDISC", pClsProperty.TOTALAVGDISC, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TOTALAVGRATE", pClsProperty.TOTALAVGRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GROSSAMOUNT", pClsProperty.GROSSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DISCOUNTPER", pClsProperty.DISCOUNTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DISCOUNTAMOUNT", pClsProperty.DISCOUNTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INSURANCEPER", pClsProperty.INSURANCEPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INSURANCEAMOUNT", pClsProperty.INSURANCEAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGPER", pClsProperty.SHIPPINGPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGAMOUNT", pClsProperty.SHIPPINGAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GSTPER", pClsProperty.GSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GSTAMOUNT", pClsProperty.GSTAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("IGSTPER", pClsProperty.IGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("IGSTAMOUNT", pClsProperty.IGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FIGSTAMOUNT", pClsProperty.FIGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("CGSTPER", pClsProperty.CGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("CGSTAMOUNT", pClsProperty.CGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FCGSTAMOUNT", pClsProperty.FCGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SGSTPER", pClsProperty.SGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SGSTAMOUNT", pClsProperty.SGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FSGSTAMOUNT", pClsProperty.FSGSTAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("NETAMOUNT", pClsProperty.NETAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FGROSSAMOUNT", pClsProperty.FGROSSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FDISCOUNTAMOUNT", pClsProperty.FDISCOUNTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FINSURANCEAMOUNT", pClsProperty.FINSURANCEAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FSHIPPINGAMOUNT", pClsProperty.FSHIPPINGAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FGSTAMOUNT", pClsProperty.FGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FNETAMOUNT", pClsProperty.FNETAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);
                Ope.AddParams("STATUS", pClsProperty.STATUS, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SOURCE", pClsProperty.SOURCE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PROCESS_ID", pClsProperty.PROCESS_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("TRANSPORTNAME", pClsProperty.TRANSPORTNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PLACEOFSUPPLY", pClsProperty.PLACEOFSUPPLY, DbType.String, ParameterDirection.Input);

                Ope.AddParams("COMPANYBANK_ID", pClsProperty.COMPANYBANK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PARTYBANK_ID", pClsProperty.PARTYBANK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("COURIER_ID", pClsProperty.COURIER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("AIRFREIGHT", pClsProperty.AIRFREIGHT, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PLACEOFRECEIPTBYPRECARRIER", pClsProperty.PLACEOFRECEIPTBYPRECARRIER, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PORTOFLOADING", pClsProperty.PORTOFLOADING, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PORTOFDISCHARGE", pClsProperty.PORTOFDISCHARGE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALDESTINATION", pClsProperty.FINALDESTINATION, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COUNTRYOFORIGIN", pClsProperty.COUNTRYOFORIGIN, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COUNTRYOFFINALDESTINATION", pClsProperty.COUNTRYOFFINALDESTINATION, DbType.String, ParameterDirection.Input);
                Ope.AddParams("INTERMIDATORYBANK", pClsProperty.INTERMIDATORYBANK, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PRECARRIERBY", pClsProperty.PRECARRIERBY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CONSIGNEE_ID", pClsProperty.Consignee_Id, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);
                //Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR", pClsProperty.FINYEAR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("VOUCHERNOSTR", pClsProperty.VOUCHERNOSTR, DbType.String, ParameterDirection.Input);

                Ope.AddParams("XMLDETSTR", StrMemoEntryDetXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MODE", pStrMode, DbType.String, ParameterDirection.Input);

                Ope.AddParams("BILLTYPE", pClsProperty.BILLTYPE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("INSURANCETYPE", pClsProperty.INSURANCETYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("GROSSWEIGHT", pClsProperty.GROSSWEIGHT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("OTHERSTONEPARTY_ID", pClsProperty.OTHERSTONEPARTY_ID, DbType.Guid, ParameterDirection.Input); //ADD MILAN(04-02-20021)
                Ope.AddParams("OTHERSTONEPARTYNAME", pClsProperty.OTHERSTONEPARTYNAME, DbType.String, ParameterDirection.Input);//ADD MILAN ""

                // #D: 19-08-2020
                Ope.AddParams("CONSIGNEE", pClsProperty.CONSIGNEE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ADDRESSTYPE", pClsProperty.ADDRESSTYPE, DbType.String, ParameterDirection.Input);
                // #D: 19-08-2020

                //#K : 05122020
                Ope.AddParams("SOAPPROVAL", pClsProperty.ORDERAPPROVAL, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TCSPER", pClsProperty.TCSPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TCSAMOUNT", pClsProperty.TCSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FTCSAMOUNT", pClsProperty.FTCSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FROUNDOFFAMOUNT", pClsProperty.FROUNDOFFAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ROUNDOFFTPYE", pClsProperty.ROUNDOFFTPYE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("CUSTOMEBROKER", pClsProperty.CustomeBroker, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CUSTOMEDESIGNATION", pClsProperty.CustomeDesignation, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CUSTOMEIDCARD", pClsProperty.CustomeIDCard, DbType.String, ParameterDirection.Input);

                Ope.AddParams("BACKADDLESS", pClsProperty.BACKADDLESS, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TERMSADDLESSPER", pClsProperty.TERMSADDLESSPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BLINDADDLESSPER", pClsProperty.BLINDADDLESSPER, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("FINALBUYER_ID", pClsProperty.FINALBUYER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("FINALADDRESS1", pClsProperty.FINALBUYERADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALADDRESS2", pClsProperty.FINALBUYERADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALADDRESS3", pClsProperty.FINALBUYERADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALCOUNTRY_ID", pClsProperty.FINALBUYERCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("FINALSTATE", pClsProperty.FINALBUYERSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALCITY", pClsProperty.FINALBUYERCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALZIPCODE", pClsProperty.FINALBUYERZIPCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ISCONSINGEE", pClsProperty.ISCONSINGEE, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("INVOICENO", pClsProperty.INVOICENO, DbType.String, ParameterDirection.Input);

                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("EXPINVOICEAMT", pClsProperty.ExpInvoiceAmt, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EXPINVOICEAMTFE", pClsProperty.ExpInvoiceAmtFE, DbType.Double, ParameterDirection.Input);

                // Add paramter by khushbu 19-08-21
                Ope.AddParams("ORG_EXCRATE", pClsProperty.ORG_EXCRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADDLESS_EXCRATE", pClsProperty.ADDLESS_EXCRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADATAMT", pClsProperty.AdatAmt, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADATAMTFE", pClsProperty.AdatAmtFE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKERAMT", pClsProperty.BrokerAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKERAMTFE", pClsProperty.BrokerAmountFE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("STKAMTFE", pClsProperty.StkAmtFE, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("ORDERMEMO_ID", pClsProperty.ORDERMEMO_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ORDERJANGEDNO", pClsProperty.ORDERJANGEDNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLFORMAT", pClsProperty.BILLFORMAT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("APARTAPPROVAL", pClsProperty.APARTAPPROVAL, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("ACCTYPE", pClsProperty.ACCTYPE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("XMLSTR_OLDDB", StrMemoEntry_OldDB, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(pConConnection, Config.ProviderName, "Trn_MemoEntrySaveNew", CommandType.StoredProcedure);

                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnValueJanged = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;
        }

        public double GetExchangeRate(int pIntCurrencyID, string pStrMemoDate, string pStrFormType, string StrStockType = "") //WITHOUTSTOCK OR WITHSTOCK
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("CURRENCT_ID", pIntCurrencyID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("MEMODATE", pStrMemoDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("LOGINTYPE", Config.gStrLoginSection, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FORMTYPE", pStrFormType, DbType.String, ParameterDirection.Input);
            //Ope.AddParams("STOCKTYPE", StrStockType, DbType.String, ParameterDirection.Input);

            DataRow DRow = null;           
            {
                DRow = Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, "Trn_MemoEntryGetExchangeRate", CommandType.StoredProcedure);
            }
            return Val.Val(DRow["ExcRate"]);
        }
        public MemoEntryProperty SaveRoughEntry(MemoEntryProperty pClsProperty, string StrMemoEntryDetXML, string pStrMode)
        {
            try
            {

                Ope.ClearParams();
                DataTable DTab = new DataTable();

                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("JANGEDNOSTR", pClsProperty.JANGEDNOSTR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMONO", pClsProperty.MEMONO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMOTYPE", pClsProperty.MEMOTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMODATE", pClsProperty.MEMODATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("BILLINGPARTY_ID", pClsProperty.BILLINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGPARTY_ID", pClsProperty.SHIPPINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("BROKER_ID", pClsProperty.BROKER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("BROKRAGEBASEPER", pClsProperty.BROKERBASEPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKRAGEPROFITPER", pClsProperty.BROKERPROFITPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADAT_ID", pClsProperty.ADAT_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ADATPER", pClsProperty.ADATPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SOLDPARTY_ID", pClsProperty.SOLDPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SELLER_ID", pClsProperty.SELLER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("TERMS_ID", pClsProperty.TERMS_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TERMSDAYS", pClsProperty.TERMSDAYS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TERMSPER", pClsProperty.TERMSPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TERMSDATE", pClsProperty.TERMSDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("CURRENCY_ID", pClsProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EXCRATE", pClsProperty.EXCRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DELIVERYTYPE", pClsProperty.DELIVERYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PAYMENTMODE", pClsProperty.PAYMENTMODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGADDRESS1", pClsProperty.BILLINGADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGADDRESS2", pClsProperty.BILLINGADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGADDRESS3", pClsProperty.BILLINGADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGCOUNTRY_ID", pClsProperty.BILLINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("BILLINGSTATE", pClsProperty.BILLINGSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGCITY", pClsProperty.BILLINGCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGZIPCODE", pClsProperty.BILLINGZIPCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS1", pClsProperty.SHIPPINGADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS2", pClsProperty.SHIPPINGADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS3", pClsProperty.SHIPPINGADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGCOUNTRY_ID", pClsProperty.SHIPPINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGSTATE", pClsProperty.SHIPPINGSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGCITY", pClsProperty.SHIPPINGCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGZIPCODE", pClsProperty.SHIPPINGZIPCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TOTALPCS", pClsProperty.TOTALPCS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TOTALCARAT", pClsProperty.TOTALCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TOTALAVGDISC", pClsProperty.TOTALAVGDISC, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TOTALAVGRATE", pClsProperty.TOTALAVGRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GROSSAMOUNT", pClsProperty.GROSSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DISCOUNTPER", pClsProperty.DISCOUNTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DISCOUNTAMOUNT", pClsProperty.DISCOUNTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INSURANCEPER", pClsProperty.INSURANCEPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INSURANCEAMOUNT", pClsProperty.INSURANCEAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGPER", pClsProperty.SHIPPINGPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGAMOUNT", pClsProperty.SHIPPINGAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GSTPER", pClsProperty.GSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GSTAMOUNT", pClsProperty.GSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("IGSTPER", pClsProperty.IGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("IGSTAMOUNT", pClsProperty.IGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FIGSTAMOUNT", pClsProperty.FIGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("CGSTPER", pClsProperty.CGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("CGSTAMOUNT", pClsProperty.CGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FCGSTAMOUNT", pClsProperty.FCGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SGSTPER", pClsProperty.SGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SGSTAMOUNT", pClsProperty.SGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FSGSTAMOUNT", pClsProperty.FSGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("NETAMOUNT", pClsProperty.NETAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FGROSSAMOUNT", pClsProperty.FGROSSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FDISCOUNTAMOUNT", pClsProperty.FDISCOUNTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FINSURANCEAMOUNT", pClsProperty.FINSURANCEAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FSHIPPINGAMOUNT", pClsProperty.FSHIPPINGAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FGSTAMOUNT", pClsProperty.FGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FNETAMOUNT", pClsProperty.FNETAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);
                Ope.AddParams("STATUS", pClsProperty.STATUS, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SOURCE", pClsProperty.SOURCE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PROCESS_ID", pClsProperty.PROCESS_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("TRANSPORTNAME", pClsProperty.TRANSPORTNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PLACEOFSUPPLY", pClsProperty.PLACEOFSUPPLY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COMPANYBANK_ID", pClsProperty.COMPANYBANK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PARTYBANK_ID", pClsProperty.PARTYBANK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("COURIER_ID", pClsProperty.COURIER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("AIRFREIGHT", pClsProperty.AIRFREIGHT, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PLACEOFRECEIPTBYPRECARRIER", pClsProperty.PLACEOFRECEIPTBYPRECARRIER, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PORTOFLOADING", pClsProperty.PORTOFLOADING, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PORTOFDISCHARGE", pClsProperty.PORTOFDISCHARGE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALDESTINATION", pClsProperty.FINALDESTINATION, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COUNTRYOFORIGIN", pClsProperty.COUNTRYOFORIGIN, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COUNTRYOFFINALDESTINATION", pClsProperty.COUNTRYOFFINALDESTINATION, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR", pClsProperty.FINYEAR, DbType.String, ParameterDirection.Input);
                //Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("VOUCHERNOSTR", pClsProperty.VOUCHERNOSTR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", StrMemoEntryDetXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MODE", pStrMode, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLTYPE", pClsProperty.BILLTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("INSURANCETYPE", pClsProperty.INSURANCETYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("GROSSWEIGHT", pClsProperty.GROSSWEIGHT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("CONSIGNEE", pClsProperty.CONSIGNEE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ADDRESSTYPE", pClsProperty.ADDRESSTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SOAPPROVAL", pClsProperty.ORDERAPPROVAL, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("BILLNO", pClsProperty.BILLNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLDATE", pClsProperty.BILLDATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TCSPER", pClsProperty.TCSPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TCSAMOUNT", pClsProperty.TCSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FTCSAMOUNT", pClsProperty.FTCSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FROUNDOFFAMOUNT", pClsProperty.FROUNDOFFAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ROUNDOFFTPYE", pClsProperty.ROUNDOFFTPYE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_RoughEntrySaveNew", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnValueJanged = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;
        }


        public MemoEntryProperty SaveRoughEntry(SqlConnection pConConnection, MemoEntryProperty pClsProperty, string StrMemoEntryDetXML, string pStrMode)
        {
            //try
            //{

            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("SRNO", pClsProperty.SRNO, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("ISCHECKED", pClsProperty.ISCHECKED, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("JANGEDNOSTR", pClsProperty.JANGEDNOSTR, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMONO", pClsProperty.MEMONO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMOTYPE", pClsProperty.MEMOTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMODATE", pClsProperty.MEMODATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("BILLINGPARTY_ID", pClsProperty.BILLINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGPARTY_ID", pClsProperty.SHIPPINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("BROKER_ID", pClsProperty.BROKER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("BROKRAGEBASEPER", pClsProperty.BROKERBASEPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROKRAGEPROFITPER", pClsProperty.BROKERPROFITPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("ADAT_ID", pClsProperty.ADAT_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ADATPER", pClsProperty.ADATPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("SOLDPARTY_ID", pClsProperty.SOLDPARTY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("SELLER_ID", pClsProperty.SELLER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("TERMS_ID", pClsProperty.TERMS_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TERMSDAYS", pClsProperty.TERMSDAYS, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TERMSPER", pClsProperty.TERMSPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("TERMSDATE", pClsProperty.TERMSDATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("CURRENCY_ID", pClsProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("EXCRATE", pClsProperty.EXCRATE, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("DELIVERYTYPE", pClsProperty.DELIVERYTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PAYMENTMODE", pClsProperty.PAYMENTMODE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGADDRESS1", pClsProperty.BILLINGADDRESS1, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGADDRESS2", pClsProperty.BILLINGADDRESS2, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGADDRESS3", pClsProperty.BILLINGADDRESS3, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGCOUNTRY_ID", pClsProperty.BILLINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("BILLINGSTATE", pClsProperty.BILLINGSTATE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGCITY", pClsProperty.BILLINGCITY, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGZIPCODE", pClsProperty.BILLINGZIPCODE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGADDRESS1", pClsProperty.SHIPPINGADDRESS1, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGADDRESS2", pClsProperty.SHIPPINGADDRESS2, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGADDRESS3", pClsProperty.SHIPPINGADDRESS3, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGCOUNTRY_ID", pClsProperty.SHIPPINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGSTATE", pClsProperty.SHIPPINGSTATE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGCITY", pClsProperty.SHIPPINGCITY, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGZIPCODE", pClsProperty.SHIPPINGZIPCODE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TOTALPCS", pClsProperty.TOTALPCS, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TOTALCARAT", pClsProperty.TOTALCARAT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("TOTALAVGDISC", pClsProperty.TOTALAVGDISC, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("TOTALAVGRATE", pClsProperty.TOTALAVGRATE, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("GROSSAMOUNT", pClsProperty.GROSSAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("DISCOUNTPER", pClsProperty.DISCOUNTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("DISCOUNTAMOUNT", pClsProperty.DISCOUNTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("INSURANCEPER", pClsProperty.INSURANCEPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("INSURANCEAMOUNT", pClsProperty.INSURANCEAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGPER", pClsProperty.SHIPPINGPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGAMOUNT", pClsProperty.SHIPPINGAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("GSTPER", pClsProperty.GSTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("GSTAMOUNT", pClsProperty.GSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("IGSTPER", pClsProperty.IGSTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("IGSTAMOUNT", pClsProperty.IGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FIGSTAMOUNT", pClsProperty.FIGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("CGSTPER", pClsProperty.CGSTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("CGSTAMOUNT", pClsProperty.CGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FCGSTAMOUNT", pClsProperty.FCGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("SGSTPER", pClsProperty.SGSTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("SGSTAMOUNT", pClsProperty.SGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FSGSTAMOUNT", pClsProperty.FSGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("NETAMOUNT", pClsProperty.NETAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FGROSSAMOUNT", pClsProperty.FGROSSAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FDISCOUNTAMOUNT", pClsProperty.FDISCOUNTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FINSURANCEAMOUNT", pClsProperty.FINSURANCEAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FSHIPPINGAMOUNT", pClsProperty.FSHIPPINGAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FGSTAMOUNT", pClsProperty.FGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FNETAMOUNT", pClsProperty.FNETAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STATUS", pClsProperty.STATUS, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SOURCE", pClsProperty.SOURCE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PROCESS_ID", pClsProperty.PROCESS_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("TRANSPORTNAME", pClsProperty.TRANSPORTNAME, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PLACEOFSUPPLY", pClsProperty.PLACEOFSUPPLY, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COMPANYBANK_ID", pClsProperty.COMPANYBANK_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("PARTYBANK_ID", pClsProperty.PARTYBANK_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("COURIER_ID", pClsProperty.COURIER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("AIRFREIGHT", pClsProperty.AIRFREIGHT, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PLACEOFRECEIPTBYPRECARRIER", pClsProperty.PLACEOFRECEIPTBYPRECARRIER, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PORTOFLOADING", pClsProperty.PORTOFLOADING, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PORTOFDISCHARGE", pClsProperty.PORTOFDISCHARGE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINALDESTINATION", pClsProperty.FINALDESTINATION, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COUNTRYOFORIGIN", pClsProperty.COUNTRYOFORIGIN, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COUNTRYOFFINALDESTINATION", pClsProperty.COUNTRYOFFINALDESTINATION, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);
            //Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINYEAR", pClsProperty.FINYEAR, DbType.String, ParameterDirection.Input);
            Ope.AddParams("VOUCHERNOSTR", pClsProperty.VOUCHERNOSTR, DbType.String, ParameterDirection.Input);
            Ope.AddParams("XMLDETSTR", StrMemoEntryDetXML, DbType.Xml, ParameterDirection.Input);
            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MODE", pStrMode, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLTYPE", pClsProperty.BILLTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("INSURANCETYPE", pClsProperty.INSURANCETYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("GROSSWEIGHT", pClsProperty.GROSSWEIGHT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("CONSIGNEE", pClsProperty.CONSIGNEE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ADDRESSTYPE", pClsProperty.ADDRESSTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SOAPPROVAL", pClsProperty.ORDERAPPROVAL, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("BILLNO", pClsProperty.BILLNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLDATE", pClsProperty.BILLDATE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TCSPER", pClsProperty.TCSPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("TCSAMOUNT", pClsProperty.TCSAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FTCSAMOUNT", pClsProperty.FTCSAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FROUNDOFFAMOUNT", pClsProperty.FROUNDOFFAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("ROUNDOFFTPYE", pClsProperty.ROUNDOFFTPYE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("CUSTOMPORT", pClsProperty.CUSTOMPORT, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLOFENTRY", pClsProperty.BILLOFENTRY, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ADCODE", pClsProperty.ADCODE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("HAWBNO", pClsProperty.HAWBNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MAWBNO", pClsProperty.MAWBNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLOFENTRYDATE", pClsProperty.BILLOFENTRYDATE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("KPCERTINO", pClsProperty.KPCERTINO, DbType.String, ParameterDirection.Input);

            // Add khushbu 22-05-21
            Ope.AddParams("BANKREFNO", pClsProperty.BankRefNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BROKERAGEAMOUNT", pClsProperty.BrokerAmount, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROKERAGEAMOUNTFE", pClsProperty.BrokerAmountFE, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("INVTERMS_ID", pClsProperty.InvTermsID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PURCHASE_ACCID", pClsProperty.PURCHASE_ACCID, DbType.Guid, ParameterDirection.Input);

            Ope.AddParams("IGSTREFNO", pClsProperty.IGST_RefNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("EXCRATETYPE", pClsProperty.ExcRateType, DbType.String, ParameterDirection.Input);
            //---

            Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

            ArrayList AL = Ope.ExeNonQueryWithOutParameter(pConConnection, Config.ProviderName, "Trn_RoughEntrySaveNew", CommandType.StoredProcedure);
            if (AL.Count != 0)
            {
                pClsProperty.ReturnValue = Val.ToString(AL[0]);
                pClsProperty.ReturnValueJanged = Val.ToString(AL[1]);
                pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
            }
            else
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnValueJanged = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = "";

            }
            //}
            //catch (System.Exception ex)
            //{
            //    pClsProperty.ReturnValue = "";
            //    pClsProperty.ReturnValueJanged = "";
            //    pClsProperty.ReturnMessageType = "FAIL";
            //    pClsProperty.ReturnMessageDesc = ex.Message;

            //}
            return pClsProperty;
        }

        public MemoEntryProperty SaveMemoEntryParcel(SqlConnection pConConnection, MemoEntryProperty pClsProperty, string StrMemoEntryDetXML, string pStrMode)
        {
            try
            {

                Ope.ClearParams();
                DataTable DTab = new DataTable();

                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                
                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("JANGEDNOSTR", pClsProperty.JANGEDNOSTR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMONO", pClsProperty.MEMONO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMOTYPE", pClsProperty.MEMOTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMODATE", pClsProperty.MEMODATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("BILLINGPARTY_ID", pClsProperty.BILLINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGPARTY_ID", pClsProperty.SHIPPINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("BROKER_ID", pClsProperty.BROKER_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("BROKRAGEBASEPER", pClsProperty.BROKERBASEPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKRAGEPROFITPER", pClsProperty.BROKERPROFITPER, DbType.Double, ParameterDirection.Input);


                Ope.AddParams("ADAT_ID", pClsProperty.ADAT_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ADATPER", pClsProperty.ADATPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SOLDPARTY_ID", pClsProperty.SOLDPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SELLER_ID", pClsProperty.SELLER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("TERMS_ID", pClsProperty.TERMS_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TERMSDAYS", pClsProperty.TERMSDAYS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TERMSPER", pClsProperty.TERMSPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TERMSDATE", pClsProperty.TERMSDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("CURRENCY_ID", pClsProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EXCRATE", pClsProperty.EXCRATE, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("DELIVERYTYPE", pClsProperty.DELIVERYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PAYMENTMODE", pClsProperty.PAYMENTMODE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("BILLINGADDRESS1", pClsProperty.BILLINGADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGADDRESS2", pClsProperty.BILLINGADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGADDRESS3", pClsProperty.BILLINGADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGCOUNTRY_ID", pClsProperty.BILLINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("BILLINGSTATE", pClsProperty.BILLINGSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGCITY", pClsProperty.BILLINGCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGZIPCODE", pClsProperty.BILLINGZIPCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS1", pClsProperty.SHIPPINGADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS2", pClsProperty.SHIPPINGADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS3", pClsProperty.SHIPPINGADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGCOUNTRY_ID", pClsProperty.SHIPPINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGSTATE", pClsProperty.SHIPPINGSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGCITY", pClsProperty.SHIPPINGCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGZIPCODE", pClsProperty.SHIPPINGZIPCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TOTALPCS", pClsProperty.TOTALPCS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TOTALCARAT", pClsProperty.TOTALCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TOTALAVGDISC", pClsProperty.TOTALAVGDISC, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TOTALAVGRATE", pClsProperty.TOTALAVGRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GROSSAMOUNT", pClsProperty.GROSSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DISCOUNTPER", pClsProperty.DISCOUNTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DISCOUNTAMOUNT", pClsProperty.DISCOUNTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INSURANCEPER", pClsProperty.INSURANCEPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INSURANCEAMOUNT", pClsProperty.INSURANCEAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGPER", pClsProperty.SHIPPINGPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGAMOUNT", pClsProperty.SHIPPINGAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GSTPER", pClsProperty.GSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GSTAMOUNT", pClsProperty.GSTAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("IGSTPER", pClsProperty.IGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("IGSTAMOUNT", pClsProperty.IGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FIGSTAMOUNT", pClsProperty.FIGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("CGSTPER", pClsProperty.CGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("CGSTAMOUNT", pClsProperty.CGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FCGSTAMOUNT", pClsProperty.FCGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SGSTPER", pClsProperty.SGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SGSTAMOUNT", pClsProperty.SGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FSGSTAMOUNT", pClsProperty.FSGSTAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("NETAMOUNT", pClsProperty.NETAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FGROSSAMOUNT", pClsProperty.FGROSSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FDISCOUNTAMOUNT", pClsProperty.FDISCOUNTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FINSURANCEAMOUNT", pClsProperty.FINSURANCEAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FSHIPPINGAMOUNT", pClsProperty.FSHIPPINGAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FGSTAMOUNT", pClsProperty.FGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FNETAMOUNT", pClsProperty.FNETAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);
                Ope.AddParams("STATUS", pClsProperty.STATUS, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SOURCE", pClsProperty.SOURCE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("PROCESS_ID", pClsProperty.PROCESS_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("TRANSPORTNAME", pClsProperty.TRANSPORTNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PLACEOFSUPPLY", pClsProperty.PLACEOFSUPPLY, DbType.String, ParameterDirection.Input);

                Ope.AddParams("COMPANYBANK_ID", pClsProperty.COMPANYBANK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PARTYBANK_ID", pClsProperty.PARTYBANK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("COURIER_ID", pClsProperty.COURIER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("AIRFREIGHT_ID", pClsProperty.AIRFREIGHT_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PLACEOFRECEIPTBYPRECARRIER", pClsProperty.PLACEOFRECEIPTBYPRECARRIER, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PORTOFLOADING", pClsProperty.PORTOFLOADING, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PORTOFDISCHARGE", pClsProperty.PORTOFDISCHARGE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALDESTINATION", pClsProperty.FINALDESTINATION, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COUNTRYOFORIGIN", pClsProperty.COUNTRYOFORIGIN, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COUNTRYOFFINALDESTINATION", pClsProperty.COUNTRYOFFINALDESTINATION, DbType.String, ParameterDirection.Input);


                Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);

                Ope.AddParams("VOUCHERNOSTR", pClsProperty.VOUCHERNOSTR, DbType.String, ParameterDirection.Input);

                Ope.AddParams("XMLDETSTR", StrMemoEntryDetXML, DbType.Xml, ParameterDirection.Input);
              
                Ope.AddParams("MODE", pStrMode, DbType.String, ParameterDirection.Input);

                Ope.AddParams("BILLTYPE", pClsProperty.BILLTYPE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("INSURANCETYPE", pClsProperty.INSURANCETYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("GROSSWEIGHT", pClsProperty.GROSSWEIGHT, DbType.Double, ParameterDirection.Input);

                // #D: 19-08-2020
                Ope.AddParams("CONSIGNEE", pClsProperty.CONSIGNEE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ADDRESSTYPE", pClsProperty.ADDRESSTYPE, DbType.String, ParameterDirection.Input);
                // #D: 19-08-2020

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("STOCKTYPE", "Parcel", DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(pConConnection, Config.ProviderName, "Trn_MemoEntryParcelSaveNew", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnValueJanged = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;
        }


        public StockUploadProperty DeleteStonePricing(StockUploadProperty pClStonePricingProperty)
        {
            try
            {
                Ope.ClearParams();
                //Ope.AddParams("TRN_ID", pClStonePricingProperty.TRN_ID, DbType.Guid, ParameterDirection.Input);
                //Ope.AddParams("PARTY_ID", pClStonePricingProperty.SELLER_ID, DbType.Int64, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_StoneDelete", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClStonePricingProperty.ReturnValue = Val.ToString(AL[0]);
                    pClStonePricingProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClStonePricingProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClStonePricingProperty.ReturnValue = "";
                pClStonePricingProperty.ReturnMessageType = "FAIL";
                pClStonePricingProperty.ReturnMessageDesc = ex.Message;

            }
            return pClStonePricingProperty;

        }

        public DataTable GetStockUploadData(Guid gParty_ID, string StrOpe = "WITHOUTSTOCK") //WITHOUTSTOCK OR WITHSTOCK
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("OPE", StrOpe, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PARTY_ID", gParty_ID, DbType.Guid, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_StockUploadGetData", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable Print(string pStrMemoID, string pPrintType) //WITHOUTSTOCK OR WITHSTOCK
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("MEMO_ID", pStrMemoID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("PRINTTYPE", pPrintType, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_MemoEntryPrint", CommandType.StoredProcedure);
          return DTab;
        }

        public DataTable PrintRs(string pStrMemoID, string pPrintType) //Account
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MEMOID", pStrMemoID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("Type", pPrintType, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Rpt_MemoEInvoiceDetail", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable PrintRsDDA(string pStrMemoID, string pPrintType) //Account DDA
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MEMOID", pStrMemoID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("Type", pPrintType, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Rpt_MemoEntryDDAPrint", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable PrintRsExport(string pStrMemoID, string pPrintType) //Account Export
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MEMOID", pStrMemoID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("Type", pPrintType, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Rpt_MemoEntryExportPrint", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable PrintPacktingListRs(string pStrMemoID, string pPrintType) //Account
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MEMOID", pStrMemoID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("Type", pPrintType, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_PackingListSaleLocalPrint", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable PrintBroDebitNote(string pStrMemoID, string pPrintType) //Account
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MEMOID", pStrMemoID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("Type", pPrintType, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Rpt_AccBrokerGSTDebitNotePrint", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable PrintBroDebitNoteMulti(string pStrMemoID, string pPrintType) //Account
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MEMOID", pStrMemoID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("Type", pPrintType, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Rpt_AccBrokerGSTDebitNoteMultiPrint", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable PrintBroGSTDebitNote(string pStrMemoID, string pPrintType) //Account
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MEMOID", pStrMemoID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("Type", pPrintType, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Rpt_AccBrokerGSTDebitNotePrint", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable PrintExpWraper(string pStrMemoID, string pPrintType) //Account
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MEMOID", pStrMemoID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("Type", pPrintType, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Rpt_AccExportWraperPrint", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable ValDelete(MemoEntryProperty pClsProperty) //WITHOUTSTOCK OR WITHSTOCK
        {
            DataTable DTab = new DataTable();
            try
            {

                Ope.ClearParams();
                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_MemoEntryValDelete", CommandType.StoredProcedure);
            }
            catch (System.Exception ex)
            {

            }
            return DTab;
        }

        public MemoEntryProperty Delete_StatusUpdateFromB(MemoEntryProperty pClsProperty) //WITHOUTSTOCK OR WITHSTOCK
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL;
                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_MemoEntryDelete_StatusUpdateFromB", CommandType.StoredProcedure);

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

        public MemoEntryProperty Delete(MemoEntryProperty pClsProperty) //WITHOUTSTOCK OR WITHSTOCK
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PRO_ID", pClsProperty.PROCESS_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PURCHASE_ORDERMEMO_ID", pClsProperty.ORDERMEMO_ID, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL;
                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_MemoEntryDelete", CommandType.StoredProcedure);

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

        public MemoEntryProperty DeleteParcel(MemoEntryProperty pClsProperty) //WITHOUTSTOCK OR WITHSTOCK
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL;
                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_MemoEntryDeleteParcel", CommandType.StoredProcedure);

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

        public string GetAPIKeyForExchangeRate()
        {
            Ope.ClearParams();
            string Str = "Select SETTINGVALUE FROM MST_Setting With(NoLock) Where SETTINGKEY = 'ExcRateAPIKey'";
            return Ope.ExeScal(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);
        }

        public double GetExchangeRate(int pIntCurrencyID, string pStrMemoDate, string pStrFormType) //WITHOUTSTOCK OR WITHSTOCK
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("CURRENCT_ID", pIntCurrencyID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("MEMODATE", pStrMemoDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("LOGINTYPE", Config.gStrLoginSection, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FORMTYPE", pStrFormType, DbType.String, ParameterDirection.Input);

            DataRow DRow = null;
            DRow = Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, "Trn_MemoEntryGetExchangeRate", CommandType.StoredProcedure);
            return Val.Val(DRow["ExcRate"]);

        }


        public double SaveExchangeRate(int pIntCurrencyID, double DouExcRate, string pStrMemoDate) //WITHOUTSTOCK OR WITHSTOCK
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("CURRENCT_ID", pIntCurrencyID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("MEMODATE", pStrMemoDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("EXCRATE", DouExcRate, DbType.Double, ParameterDirection.Input);

            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
           
            DataRow DRow = Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, "Trn_MemoEntryExchangeRateSave", CommandType.StoredProcedure);

            return Val.Val(DRow["ExcRate"]);

        }

        public DataSet GetLiveStockData(LiveStockProperty pClsProperty)  // Used In Live Stock : 25-06-2019
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("SHAPE_ID", pClsProperty.MULTYSHAPE_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLOR_ID", pClsProperty.MULTYCOLOR_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CLARITY_ID", pClsProperty.MULTYCLARITY_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUT_ID", pClsProperty.MULTYCUT_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("POL_ID", pClsProperty.MULTYPOL_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SYM_ID", pClsProperty.MULTYSYM_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FL_ID", pClsProperty.MULTYFL_ID, DbType.String, ParameterDirection.Input);

            Ope.AddParams("STOCKNO", pClsProperty.STOCKNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LABREPORTNO", pClsProperty.LABREPORTNO, DbType.String, ParameterDirection.Input);

            Ope.AddParams("FROMCARAT", pClsProperty.FROMCARAT, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOCARAT", pClsProperty.TOCARAT, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMLENGTH", pClsProperty.FROMLENGTH, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOLENGTH", pClsProperty.TOLENGTH, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMWIDTH", pClsProperty.FROMWIDTH, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOWIDTH", pClsProperty.TOWIDTH, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMHEIGHT", pClsProperty.FROMHEIGHT, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOHEIGHT", pClsProperty.TOHEIGHT, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMTABLEPER", pClsProperty.FROMTABLEPER, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TOTABLEPER", pClsProperty.TOTABLEPER, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("FROMDEPTHPER", pClsProperty.FROMDEPTHPER, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("TODEPTHPER", pClsProperty.TODEPTHPER, DbType.Decimal, ParameterDirection.Input);

            Ope.AddParams("STOCKTYPE", pClsProperty.STOCKTYPE, DbType.String, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Table", "Trn_LiveStockGetData", CommandType.StoredProcedure);

            return DS;
        }


        public DataSet GetMemoListData(
               int pIntProcessID,
               string pStrFromDate,
               string pStrToDate,
               string pStrMemoNo,
               string pStrStoneNo,
               string pStrBillingPartyID,
               int pIntBillingCountryID,
               string pStrShippingPartyID,
               int pIntShippingCountryID,
               string pStrSellerID,
               string pStrStatus,
               string pStrMemoID            
               , string pStrStockType
               , bool pIsWithSalesDelivery, int pOnlyApprovedSalesOrder,
               string pStrCommID = "",
              int pStrFromData = 0,
              int pStrToData = 0,
              string pStrTrnOldFromData = "",
              string pStrTrnOldToData = "",
              int pIntYearId = 0,
              string pStrHKStoneType = ""            
            ,string pStrCertificateNo="" , string DIAMONDTYPE = "", bool pIsShipmentPending = false, string pStrShiParty = ""
               )
        {

            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("MEMONO", pStrMemoNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PROCESS_ID", pIntProcessID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STONENO", pStrStoneNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGPARTY_ID", pStrBillingPartyID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGCOUNTRY_ID", pIntBillingCountryID, DbType.Int32, ParameterDirection.Input);

            Ope.AddParams("SHIPPINGPARTY_ID", pStrShiParty, DbType.String, ParameterDirection.Input);
            //Ope.AddParams("SHIPPINGPARTY_ID", pStrShippingPartyID, DbType.String, ParameterDirection.Input);

            Ope.AddParams("SHIPPINGCOUNTRY_ID", pIntShippingCountryID, DbType.Int32, ParameterDirection.Input);

            Ope.AddParams("SELLER_ID", pStrSellerID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STATUS", pStrStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMO_ID", pStrMemoID, DbType.String, ParameterDirection.Input);
            
            Ope.AddParams("STOCKTYPE", pStrStockType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("WITHSALESDELIVERY", pIsWithSalesDelivery, DbType.Boolean, ParameterDirection.Input);
            Ope.AddParams("SALESORDERAPPROVAL", pOnlyApprovedSalesOrder, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("BROKER_ID", pStrCommID, DbType.String, ParameterDirection.Input);
            
            Ope.AddParams("FROMINVDATA", pStrFromData, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TOINVDATA", pStrToData, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TRNOLDFROMDATA", pStrTrnOldFromData, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TRNOLDTODATA", pStrTrnOldToData, DbType.String, ParameterDirection.Input);

            Ope.AddParams("DIAMONDTYPE", DIAMONDTYPE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("ISSHIPMENTPENDING", pIsShipmentPending, DbType.Boolean, ParameterDirection.Input);

            Ope.AddParams("YEARID", pIntYearId, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("HKSTONETYPE", pStrHKStoneType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CERTIFICATENO", pStrCertificateNo, DbType.String, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Table", "Trn_MemoEntryGetData", CommandType.StoredProcedure);
            return DS;
        }


        static readonly string PasswordHash = "";
        static readonly string SaltKey = "AxoneInfotech";
        static readonly string VIKey = "AxoneRajVakadiya";

        public static string TextDecrypt(string encryptedText)
        {
            try
            {
                byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
                byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
                var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

                var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
                var memoryStream = new MemoryStream(cipherTextBytes);
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
            }
            catch (Exception ex)
            {
                return "";
            }

        }

     
        public DataSet BranchReceiveGetData(
           int pIntProcessID,
           string pStrFromDate,
           string pStrToDate,
           string pStrMemoNo,
           string pStrStoneNo,
           string pStrBillingPartyID,
           string pStrShippingPartyID,
           string pStrStatus,
           string pStrMemoID,
            string pStrReceiveStatus,
             string pStrLabReportNo,
              string pStrHKStone,
              string pStrBuyerPartyID,
              Guid GstCompany_ID
           ) //D: 18022021 FOR GET BRANCH RECEIVE DATA
        {

            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("COMPANY_ID", GstCompany_ID, DbType.Guid, ParameterDirection.Input);
            //Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);

            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("MEMONO", pStrMemoNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PROCESS_ID", pIntProcessID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STONENO", pStrStoneNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGPARTY_ID", pStrBillingPartyID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGPARTY_ID", pStrShippingPartyID, DbType.String, ParameterDirection.Input);

            Ope.AddParams("STATUS", pStrStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMO_ID", pStrMemoID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LABREPORTNO", pStrLabReportNo, DbType.String, ParameterDirection.Input);

            Ope.AddParams("RECEIVESTATUS", pStrReceiveStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("HKSTOCKNO", pStrHKStone, DbType.String, ParameterDirection.Input); //Add by hinal 30-06-2022
            Ope.AddParams("BUYERPARTY_ID", pStrBuyerPartyID, DbType.String, ParameterDirection.Input);

            //Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Table", "ACC_BranchReceiveGetData", CommandType.StoredProcedure);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Table", "ACC_BranchReceiveGetDataNew", CommandType.StoredProcedure);

            return DS;
        }

        public string BranchReceiveUpdate(Guid pGuidMemo_ID, string pStrComment, Guid pGuidShippingParty,Guid GstCompany_ID) // D: 18-02-2021
        {
            string ReturnValue = "";
            string ReturnMessageType = "";
            string ReturnMessageDesc = "";

            try
            {
                Ope.ClearParams();
                Ope.AddParams("MEMO_ID", pGuidMemo_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGPARTY_ID", pGuidShippingParty, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("COMMENT", pStrComment, DbType.String, ParameterDirection.Input);

                Ope.AddParams("COMPANY_ID", GstCompany_ID, DbType.String, ParameterDirection.Input);
                //Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.String, ParameterDirection.Input);


                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "ACC_BranchReceiveUpdate", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    ReturnValue = Val.ToString(AL[0]);
                    ReturnMessageType = Val.ToString(AL[1]);
                    ReturnMessageDesc = Val.ToString(AL[2]);
                }

            }
            catch (Exception Ex)
            {
                ReturnValue = "";
                ReturnMessageType = "FAIL";
                ReturnMessageDesc = Ex.Message;
            }
            return ReturnMessageType;

        }

        public string PickUpUpdate(Guid pGuidStock_ID,Guid pGuidMemoDetail_ID, string pStrPaymentMode,Guid GstCompany_ID) // D: 18-02-2021
        {
            string ReturnValue = "";
            string ReturnMessageType = "";
            string ReturnMessageDesc = "";

            try
            {
                Ope.ClearParams();
                Ope.AddParams("STOCK_ID", pGuidStock_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("MEMODETAIL_ID", pGuidMemoDetail_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PAYMENTMODE", pStrPaymentMode, DbType.String, ParameterDirection.Input);

                Ope.AddParams("COMPANY_ID", GstCompany_ID, DbType.String, ParameterDirection.Input);
                //Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.String, ParameterDirection.Input);


                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "ACC_PickUpUpdate", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    ReturnValue = Val.ToString(AL[0]);
                    ReturnMessageType = Val.ToString(AL[1]);
                    ReturnMessageDesc = Val.ToString(AL[2]);
                }

            }
            catch (Exception Ex)
            {
                ReturnValue = "";
                ReturnMessageType = "FAIL";
                ReturnMessageDesc = Ex.Message;
            }
            return ReturnMessageType;

        }



        public DataSet GetRoughListData(int pIntProcessID, string pStrFromDate, string pStrToDate, string pStrMemoNo, string pStrStoneNo, string pStrBillingPartyID, int pIntBillingCountryID, string pStrShippingPartyID, int pIntShippingCountryID,
            string pStrSellerID,string pStrStatus,string pStrMemoID, string pStrStockType, bool pIsWithSalesDelivery, bool pOnlyApprovedSalesOrder)
        {

            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("MEMONO", pStrMemoNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PROCESS_ID", pIntProcessID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STONENO", pStrStoneNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGPARTY_ID", pStrBillingPartyID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGCOUNTRY_ID", pIntBillingCountryID, DbType.Int32, ParameterDirection.Input);

            Ope.AddParams("SHIPPINGPARTY_ID", pStrShippingPartyID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGCOUNTRY_ID", pIntShippingCountryID, DbType.Int32, ParameterDirection.Input);

            Ope.AddParams("SELLER_ID", pStrSellerID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STATUS", pStrStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMO_ID", pStrMemoID, DbType.String, ParameterDirection.Input);
            
            Ope.AddParams("STOCKTYPE", pStrStockType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("WITHSALESDELIVERY", pIsWithSalesDelivery, DbType.Boolean, ParameterDirection.Input);
            Ope.AddParams("SALESORDERAPPROVAL", pOnlyApprovedSalesOrder, DbType.Boolean, ParameterDirection.Input);
           
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Table", "Trn_ROUGHEntryGetData", CommandType.StoredProcedure);
            return DS;
        }

        public DataSet GetParcelMemoListData(
          int pIntProcessID,
          string pStrFromDate,
          string pStrToDate,
          string pStrMemoNo,
          string pStrStoneNo,
          string pStrBillingPartyID,
          int pIntBillingCountryID,
          string pStrShippingPartyID,
          int pIntShippingCountryID,
          string pStrSellerID,
          string pStrStatus,
          string pStrMemoID
          , string pStrStockType
          , bool pIsWithSalesDelivery
          , int pStrOCApprovedOrder
          )
        {

            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("MEMONO", pStrMemoNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PROCESS_ID", pIntProcessID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STONENO", pStrStoneNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGPARTY_ID", pStrBillingPartyID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGCOUNTRY_ID", pIntBillingCountryID, DbType.Int32, ParameterDirection.Input);

            Ope.AddParams("SHIPPINGPARTY_ID", pStrShippingPartyID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGCOUNTRY_ID", pIntShippingCountryID, DbType.Int32, ParameterDirection.Input);

            Ope.AddParams("SELLER_ID", pStrSellerID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STATUS", pStrStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMO_ID", pStrMemoID, DbType.String, ParameterDirection.Input);
            
            Ope.AddParams("STOCKTYPE", pStrStockType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("WITHSALESDELIVERY", pIsWithSalesDelivery, DbType.Boolean, ParameterDirection.Input);
            
            Ope.AddParams("ORDERCONFIRMAPPROVAL", pStrOCApprovedOrder, DbType.Int32, ParameterDirection.Input);
Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Table", "Trn_MemoEntryParcelGetData", CommandType.StoredProcedure);

            return DS;
        }


        public DataTable GetMemoReport(
           string pStrFromDate,
           string pStrToDate,
           string pStrMemoNo,
           string pStrStoneNo, 
           string pStrBillingPartyID,
           int pIntBillingCountryID,
           string pStrShippingPartyID,
           int pIntShippingCountryID,
           string pStrSellerID,
           string pStrStatus,
           string pStrMemoID
           )
        {

            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("MEMONO", pStrMemoNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STONENO", pStrStoneNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGPARTY_ID", pStrBillingPartyID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGCOUNTRY_ID", pIntBillingCountryID, DbType.Int32, ParameterDirection.Input);

            Ope.AddParams("SHIPPINGPARTY_ID", pStrShippingPartyID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGCOUNTRY_ID", pIntShippingCountryID, DbType.Int32, ParameterDirection.Input);

            Ope.AddParams("SELLER_ID", pStrSellerID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STATUS", pStrStatus, DbType.String, ParameterDirection.Input);
           
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_MemoEntryReport", CommandType.StoredProcedure);

            return DTab;
        }


        public DataTable GetMemoReportNPNL(string pStrFromDate,string pStrToDate)
        {

            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_MemoEntryReportNPNL", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable GetMemoReportOutStanding(string pStrASOnDate, string pStrReportType)
        {

            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("ASONDATE", pStrASOnDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("REPORTTYPE", pStrReportType, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_MemoEntryReportOutStanding", CommandType.StoredProcedure);

            return DTab;
        }

        public DataSet GetSalesAnalysisReport(string pStrOpe, string pStrProcess, string pStrFromDate, string pStrToDate,

                string pStrShape,
                string StrColor,
                string StrClarity,
                string StrCut,
                string StrPol,
                string StrSym,
                string StrFL,
                string StrLocation,
                string StrSeller,
                string StrParty
            )
        {

            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PROCESS", pStrProcess, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);

            Ope.AddParams("SHAPE_ID", pStrShape, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLOR_ID", StrColor, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CLARITY_ID", StrClarity, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUT_ID", StrCut, DbType.String, ParameterDirection.Input);
            Ope.AddParams("POL_ID", StrPol, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SYM_ID", StrSym, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FL_ID", StrFL, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LOCATION_ID", StrLocation, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SELLER_ID", StrSeller, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PARTY_ID", StrParty, DbType.String, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Trn_MemoEntrySalesAnalysis", CommandType.StoredProcedure);

            return DS;
        }

        public DataSet GetStockAnalysisReport(string pStrShape,
                string StrColor,
                string StrClarity,
                string StrCut,
                string StrPol,
                string StrSym,
                string StrFL,
                string StrLocation
            )
        {

            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("SHAPE_ID", pStrShape, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLOR_ID", StrColor, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CLARITY_ID", StrClarity, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUT_ID", StrCut, DbType.String, ParameterDirection.Input);
            Ope.AddParams("POL_ID", StrPol, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SYM_ID", StrSym, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FL_ID", StrFL, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LOCATION_ID", StrLocation, DbType.String, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Trn_StockAnalysisReportGetData", CommandType.StoredProcedure);

            return DS;
        }



        public DataSet GetStockAnalysisReport(string pStrOpe, string pStrFromDate, string pStrToDate,

                string pStrShape,
                string StrColor,
                string StrClarity,
                string StrCut,
                string StrPol,
                string StrSym,
                string StrFL,
                string StrLocation,
                string StrSeller,
                string StrParty
            )
        {

            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);

            Ope.AddParams("SHAPE_ID", pStrShape, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLOR_ID", StrColor, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CLARITY_ID", StrClarity, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUT_ID", StrCut, DbType.String, ParameterDirection.Input);
            Ope.AddParams("POL_ID", StrPol, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SYM_ID", StrSym, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FL_ID", StrFL, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LOCATION_ID", StrLocation, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SELLER_ID", StrSeller, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PARTY_ID", StrParty, DbType.String, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Trn_StockAnalysisReportGetData", CommandType.StoredProcedure);

            return DS;
        }

        public DataTable GetOpeningClosingReport(string pStrStockType, string pStrFromDate, string pStrToDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("STOCKTYPE", pStrStockType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_MemoEntryOpeningClosingReport", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable GetOpeningClosingReportDetail(string pStrStockType, string pStrCurrentDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("OPE", pStrStockType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CURRENTDATE", pStrCurrentDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_MemoEntryOpeningClosingReportDetail", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable GetOpeningClosingReportDetail(string pStrStockType, string pStrCurrentDate, string StrStoneDetails, string StrShape, double FromCarat, double Tocarat, string StrInvoice, bool IS_ORDER) // Add Khushbu 09-08-21 
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("OPE", pStrStockType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CURRENTDATE", pStrCurrentDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("COMPANY_ID", Guid.Empty, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ISSTONEDETAILS", StrStoneDetails, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", StrShape, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMCARAT", FromCarat, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TOCARAT", Tocarat, DbType.String, ParameterDirection.Input);
            Ope.AddParams("INVOICENO", StrInvoice, DbType.String, ParameterDirection.Input);
            Ope.AddParams("IS_ORDER", IS_ORDER, DbType.Boolean, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_MemoEntryOpeningClosingReportDetail_Pinali_27052022", CommandType.StoredProcedure);

            return DTab;
        }


        public DataTable GetOpeningClosingReportDetail(string pStrStockType, string pStrCurrentDate, string StrStoneDetails) // Add Khushbu 09-08-21 
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("OPE", pStrStockType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CURRENTDATE", pStrCurrentDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("COMPANY_ID", Guid.Empty, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ISSTONEDETAILS", StrStoneDetails, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_MemoEntryOpeningClosingReportDetail_Pinali", CommandType.StoredProcedure);

            return DTab;
        }
        public DataTable GetOpeningClosingReport(string pStrStockType, string pStrFromDate, string pStrToDate, string StrDetail, string StrShape, double FromCarat, double Tocarat, string StrInvoice, bool IS_ORDER) // Add Khushbu 09-08-21
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("STOCKTYPE", pStrStockType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("ISSTONEDETAILS", StrDetail, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", StrShape, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMCARAT", FromCarat, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TOCARAT", Tocarat, DbType.String, ParameterDirection.Input);
            Ope.AddParams("INVOICENO", StrInvoice, DbType.String, ParameterDirection.Input);
            Ope.AddParams("IS_ORDER", IS_ORDER, DbType.Boolean, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_MemoEntryOpeningClosingReport_27052022", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable GetOpeningClosingReportNew(string pStrFromDate, string pStrToDate) // Add Shiv 07-11-22
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("FROM_DATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TO_DATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_LIVESTOCK_NEW", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable GetOpeningClosingReportDetailNew(string pStrFromDate, string pStrType) // Add Shiv 08-11-22
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("FROM_DATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("Type", pStrType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CompId", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_StockGetDetail", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable GetGIALinkForLabResultPDF(string StrLabReportNo, string StrCarat) //WITHOUTSTOCK OR WITHSTOCK
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("LABREPORTNO", StrLabReportNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("WEIGHTCARAT", StrCarat, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "GET_GIALINKFOR_LABRESULTPDF", CommandType.StoredProcedure);

            return DTab;
        }
        public DataTable MemoInvoicePrint(string pStrMemoID, string pPrintType, string pOPE) //#P: 05-05-2020
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("MEMO_ID", pStrMemoID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("PRINTTYPE", pPrintType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("OPE", pOPE, DbType.String, ParameterDirection.Input);
Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_MemoEntryInvoicePrint", CommandType.StoredProcedure);
           return DTab;
        }

        public DataTable MemoInvoicePrint(string pStrMemoID, string pPrintType, string pOPE, int pIntBroker) //#P: 05-05-2020
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("MEMO_ID", pStrMemoID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("PRINTTYPE", pPrintType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("OPE", pOPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BROKREQ", pIntBroker, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_MemoEntryInvoicePrint", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable ExportPackingList(string pStrMemoID) //#P: 05-05-2020
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("INVOICE_ID", pStrMemoID, DbType.Guid, ParameterDirection.Input);
Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_PackingListPrint", CommandType.StoredProcedure);
           return DTab;
        }


        public DataTable GetPartyBankDetail(string pStrParty_ID) //WITHOUTSTOCK OR WITHSTOCK
        {
            DataTable DTab = new DataTable();
            try
            {

                Ope.ClearParams();
                string StrQuery = "Select BANK_ID,BANKNAME,BRANCHNAME,BANKACCOUNTNO AS ACCOUNTNO,IFSCCODE,SWIFTCODE,ADDRESS FROM MST_LEDGERBANK With(NoLock) Where LEDGER_ID = '" + Val.ToString(pStrParty_ID) + "'";
                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrQuery, CommandType.Text);               
            }
            catch (System.Exception ex)
            {

            }
            return DTab;
        }

        public DataSet GetMemoMatchingListData(
           int pIntProcessID,
           string pStrFromDate,
           string pStrToDate,
           string pStrStockType
           )
        {

            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("PROCESS_ID", pIntProcessID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STOCKTYPE", pStrStockType, DbType.String, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Table", "Trn_MemoEntryGetData", CommandType.StoredProcedure);

            return DS;
        }

        public MemoEntryProperty UpdateSoApprovalFrmMemoList(MemoEntryProperty pClsProperty,Int32 pStrChkState) //WITHOUTSTOCK OR WITHSTOCK
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("CHKSTATUS", pStrChkState, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_MEMOLISTAPPROVESALESORDER", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "FAIL";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = "FAIL";

            }
            return pClsProperty;
        }
        public MemoEntryProperty UpdateInvRecevFrmMemoList(MemoEntryProperty pClsProperty, Int32 pStrChkState) //WITHOUTSTOCK OR WITHSTOCK
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("CHKSTATUS", pStrChkState, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_MEMOLISTUPDATEINVRECEV", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "FAIL";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = "FAIL";

            }
            return pClsProperty;
        }

        public DataTable GetDistinctItemName() //WITHOUTSTOCK OR WITHSTOCK
        {
            DataTable DTab = new DataTable();
            try
            {

                Ope.ClearParams();
                //string StrQuery = "Select DISTINCT ITEMNAME FROM TRN_MEMODETAIL With(NoLock) "; shiv 28-04-2022
                string StrQuery = "Select PARANAME AS ITEMNAME FROM MST_ParaItemDec With(NoLock) WHERE PARATYPE = 'ITEMDESCRIPTION'";
                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrQuery, CommandType.Text);
            }
            catch (System.Exception ex)
            {

            }
            return DTab;
        }
        public DataTable GetDistinctHSNCODE() //WITHOUTSTOCK OR WITHSTOCK
        {
            DataTable DTab = new DataTable();
            try
            {

                Ope.ClearParams();
                //string StrQuery = "Select DISTINCT HSNCODE FROM TRN_MEMODETAIL With(NoLock) "; shiv 28-04-2022
                string StrQuery = "Select ACC_CODE AS HSNCODE FROM MST_ParaItemDec With(NoLock) WHERE PARATYPE = 'ITEMDESCRIPTION'";
                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrQuery, CommandType.Text);
            }
            catch (System.Exception ex)
            {

            }
            return DTab;
        }

        public DataTable RegisterPrint(string pStrFromDate, string pStrToDate, string pStrLedger_ID, string pStrAcctType, string pStrReportType)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("FROMDATE", pStrFromDate, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LEDER_ID", pStrLedger_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ACCTYPE", pStrAcctType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("REPORTTYPE", pStrReportType, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "RP_SalesDeliveryReport", CommandType.StoredProcedure);

            return DTab;
        }


        public MemoEntryProperty SaveMemoEntryParcel_SaleEntry(SqlConnection pConConnection, MemoEntryProperty pClsProperty, string StrMemoEntryDetXML, string pStrMode, string StrMemoEntry_OldDB) //Add Khushbu 27-05-21
        {
            try
            {

                Ope.ClearParams();
                DataTable DTab = new DataTable();

                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("JANGEDNOSTR", pClsProperty.JANGEDNOSTR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMONO", pClsProperty.MEMONO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMOTYPE", pClsProperty.MEMOTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMODATE", pClsProperty.MEMODATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("BILLINGPARTY_ID", pClsProperty.BILLINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGPARTY_ID", pClsProperty.SHIPPINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("BROKER_ID", pClsProperty.BROKER_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("BROKRAGEBASEPER", pClsProperty.BROKERBASEPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKRAGEPROFITPER", pClsProperty.BROKERPROFITPER, DbType.Double, ParameterDirection.Input);


                Ope.AddParams("ADAT_ID", pClsProperty.ADAT_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ADATPER", pClsProperty.ADATPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SOLDPARTY_ID", pClsProperty.SOLDPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SELLER_ID", pClsProperty.SELLER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("TERMS_ID", pClsProperty.TERMS_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TERMSDAYS", pClsProperty.TERMSDAYS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TERMSPER", pClsProperty.TERMSPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TERMSDATE", pClsProperty.TERMSDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("CURRENCY_ID", pClsProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EXCRATE", pClsProperty.EXCRATE, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("DELIVERYTYPE", pClsProperty.DELIVERYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PAYMENTMODE", pClsProperty.PAYMENTMODE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("BILLINGADDRESS1", pClsProperty.BILLINGADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGADDRESS2", pClsProperty.BILLINGADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGADDRESS3", pClsProperty.BILLINGADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGCOUNTRY_ID", pClsProperty.BILLINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("BILLINGSTATE", pClsProperty.BILLINGSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGCITY", pClsProperty.BILLINGCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGZIPCODE", pClsProperty.BILLINGZIPCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS1", pClsProperty.SHIPPINGADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS2", pClsProperty.SHIPPINGADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS3", pClsProperty.SHIPPINGADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGCOUNTRY_ID", pClsProperty.SHIPPINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGSTATE", pClsProperty.SHIPPINGSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGCITY", pClsProperty.SHIPPINGCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGZIPCODE", pClsProperty.SHIPPINGZIPCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TOTALPCS", pClsProperty.TOTALPCS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TOTALCARAT", pClsProperty.TOTALCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TOTALAVGDISC", pClsProperty.TOTALAVGDISC, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TOTALAVGRATE", pClsProperty.TOTALAVGRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GROSSAMOUNT", pClsProperty.GROSSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DISCOUNTPER", pClsProperty.DISCOUNTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DISCOUNTAMOUNT", pClsProperty.DISCOUNTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INSURANCEPER", pClsProperty.INSURANCEPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INSURANCEAMOUNT", pClsProperty.INSURANCEAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGPER", pClsProperty.SHIPPINGPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGAMOUNT", pClsProperty.SHIPPINGAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GSTPER", pClsProperty.GSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GSTAMOUNT", pClsProperty.GSTAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("IGSTPER", pClsProperty.IGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("IGSTAMOUNT", pClsProperty.IGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FIGSTAMOUNT", pClsProperty.FIGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("CGSTPER", pClsProperty.CGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("CGSTAMOUNT", pClsProperty.CGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FCGSTAMOUNT", pClsProperty.FCGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SGSTPER", pClsProperty.SGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SGSTAMOUNT", pClsProperty.SGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FSGSTAMOUNT", pClsProperty.FSGSTAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("NETAMOUNT", pClsProperty.NETAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FGROSSAMOUNT", pClsProperty.FGROSSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FDISCOUNTAMOUNT", pClsProperty.FDISCOUNTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FINSURANCEAMOUNT", pClsProperty.FINSURANCEAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FSHIPPINGAMOUNT", pClsProperty.FSHIPPINGAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FGSTAMOUNT", pClsProperty.FGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FNETAMOUNT", pClsProperty.FNETAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);
                Ope.AddParams("STATUS", pClsProperty.STATUS, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SOURCE", pClsProperty.SOURCE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("PROCESS_ID", pClsProperty.PROCESS_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("TRANSPORTNAME", pClsProperty.TRANSPORTNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PLACEOFSUPPLY", pClsProperty.PLACEOFSUPPLY, DbType.String, ParameterDirection.Input);

                Ope.AddParams("COMPANYBANK_ID", pClsProperty.COMPANYBANK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PARTYBANK_ID", pClsProperty.PARTYBANK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("COURIER_ID", pClsProperty.COURIER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("AIRFREIGHT_ID", pClsProperty.AIRFREIGHT_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PLACEOFRECEIPTBYPRECARRIER", pClsProperty.PLACEOFRECEIPTBYPRECARRIER, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PORTOFLOADING", pClsProperty.PORTOFLOADING, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PORTOFDISCHARGE", pClsProperty.PORTOFDISCHARGE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALDESTINATION", pClsProperty.FINALDESTINATION, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COUNTRYOFORIGIN", pClsProperty.COUNTRYOFORIGIN, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COUNTRYOFFINALDESTINATION", pClsProperty.COUNTRYOFFINALDESTINATION, DbType.String, ParameterDirection.Input);


                Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);

                Ope.AddParams("VOUCHERNOSTR", pClsProperty.VOUCHERNOSTR, DbType.String, ParameterDirection.Input);

                Ope.AddParams("XMLDETSTR", StrMemoEntryDetXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("XMLSTR_OLDDB", StrMemoEntry_OldDB, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("MODE", pStrMode, DbType.String, ParameterDirection.Input);

                Ope.AddParams("BILLTYPE", pClsProperty.BILLTYPE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("INSURANCETYPE", pClsProperty.INSURANCETYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("GROSSWEIGHT", pClsProperty.GROSSWEIGHT, DbType.Double, ParameterDirection.Input);

                // #D: 19-08-2020
                Ope.AddParams("CONSIGNEE", pClsProperty.CONSIGNEE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ADDRESSTYPE", pClsProperty.ADDRESSTYPE, DbType.String, ParameterDirection.Input);
                // #D: 19-08-2020

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("STOCKTYPE", "Parcel", DbType.String, ParameterDirection.Input);

                Ope.AddParams("LESSPER1", pClsProperty.LessPer1, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("LESSPER2", pClsProperty.LessPer2, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("LESSPER3", pClsProperty.LessPer3, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADATAMT", pClsProperty.AdatAmt, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADATAMTFE", pClsProperty.AdatAmtFE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKERAMT", pClsProperty.BrokerAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKERAMTFE", pClsProperty.BrokerAmountFE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("STKAMTFE", pClsProperty.StkAmtFE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EXPINVOICEAMT", pClsProperty.ExpInvoiceAmt, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EXPINVOICEAMTFE", pClsProperty.ExpInvoiceAmtFE, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("ORG_EXCRATE", pClsProperty.ORG_EXCRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADDLESS_EXCRATE", pClsProperty.ADDLESS_EXCRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FROUNDOFFAMOUNT", pClsProperty.FROUNDOFFAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ROUND_ADDAMOUNT", pClsProperty.ROUND_ADDAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ROUND_LESSAMOUNT", pClsProperty.ROUND_LESSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SOAPPROVAL", pClsProperty.ORDERAPPROVAL, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("ORDERMEMO_ID", pClsProperty.ORDERMEMO_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ORDERJANGEDNO", pClsProperty.ORDERJANGEDNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLFORMAT", pClsProperty.BILLFORMAT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("APPROVEMEMO_ID", pClsProperty.APPROVEMEMO_ID_A, DbType.String, ParameterDirection.Input);
                Ope.AddParams("Is_APartReceive", pClsProperty.IS_APARTRECEIVE, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("APartApproval", pClsProperty.APARTAPPROVAL, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ACCTYPE", pClsProperty.ACCTYPE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("FINALBUYER_ID", pClsProperty.FINALBUYER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("FINALADDRESS1", pClsProperty.FINALBUYERADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALADDRESS2", pClsProperty.FINALBUYERADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALADDRESS3", pClsProperty.FINALBUYERADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALCOUNTRY_ID", pClsProperty.FINALBUYERCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("FINALSTATE", pClsProperty.FINALBUYERSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALCITY", pClsProperty.FINALBUYERCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALZIPCODE", pClsProperty.FINALBUYERZIPCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ISCONSINGEE", pClsProperty.ISCONSINGEE, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(pConConnection, Config.ProviderName, "Trn_MemoEntryParcelSaveNew_SaleEntry_WithWhile", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnValueJanged = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;
        }

        public MemoEntryProperty SaveMemoEntryParcel_SaleEntry_B(SqlConnection pConConnection, MemoEntryProperty pClsProperty, string StrMemoEntryDetXML, string pStrMode, string StrMemoEntry_OldDB) //Add Khushbu 27-05-21
        {
            try
            {

                Ope.ClearParams();
                DataTable DTab = new DataTable();

                Ope.AddParams("APPROVEMEMO_ID", pClsProperty.APPROVEMEMO_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("APPROVEMEMONO", pClsProperty.APPROVEMEMONO, DbType.String, ParameterDirection.Input);

                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("JANGEDNOSTR", pClsProperty.JANGEDNOSTR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMONO", pClsProperty.MEMONO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMOTYPE", pClsProperty.MEMOTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMODATE", pClsProperty.MEMODATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("BILLINGPARTY_ID", pClsProperty.BILLINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGPARTY_ID", pClsProperty.SHIPPINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("BROKER_ID", pClsProperty.BROKER_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("BROKRAGEBASEPER", pClsProperty.BROKERBASEPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKRAGEPROFITPER", pClsProperty.BROKERPROFITPER, DbType.Double, ParameterDirection.Input);


                Ope.AddParams("ADAT_ID", pClsProperty.ADAT_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ADATPER", pClsProperty.ADATPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SOLDPARTY_ID", pClsProperty.SOLDPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SELLER_ID", pClsProperty.SELLER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("TERMS_ID", pClsProperty.TERMS_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TERMSDAYS", pClsProperty.TERMSDAYS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TERMSPER", pClsProperty.TERMSPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TERMSDATE", pClsProperty.TERMSDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("CURRENCY_ID", pClsProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EXCRATE", pClsProperty.EXCRATE, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("DELIVERYTYPE", pClsProperty.DELIVERYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PAYMENTMODE", pClsProperty.PAYMENTMODE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("BILLINGADDRESS1", pClsProperty.BILLINGADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGADDRESS2", pClsProperty.BILLINGADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGADDRESS3", pClsProperty.BILLINGADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGCOUNTRY_ID", pClsProperty.BILLINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("BILLINGSTATE", pClsProperty.BILLINGSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGCITY", pClsProperty.BILLINGCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGZIPCODE", pClsProperty.BILLINGZIPCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS1", pClsProperty.SHIPPINGADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS2", pClsProperty.SHIPPINGADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS3", pClsProperty.SHIPPINGADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGCOUNTRY_ID", pClsProperty.SHIPPINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGSTATE", pClsProperty.SHIPPINGSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGCITY", pClsProperty.SHIPPINGCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGZIPCODE", pClsProperty.SHIPPINGZIPCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TOTALPCS", pClsProperty.TOTALPCS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TOTALCARAT", pClsProperty.TOTALCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TOTALAVGDISC", pClsProperty.TOTALAVGDISC, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TOTALAVGRATE", pClsProperty.TOTALAVGRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GROSSAMOUNT", pClsProperty.GROSSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DISCOUNTPER", pClsProperty.DISCOUNTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DISCOUNTAMOUNT", pClsProperty.DISCOUNTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INSURANCEPER", pClsProperty.INSURANCEPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INSURANCEAMOUNT", pClsProperty.INSURANCEAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGPER", pClsProperty.SHIPPINGPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGAMOUNT", pClsProperty.SHIPPINGAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GSTPER", pClsProperty.GSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GSTAMOUNT", pClsProperty.GSTAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("IGSTPER", pClsProperty.IGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("IGSTAMOUNT", pClsProperty.IGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FIGSTAMOUNT", pClsProperty.FIGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("CGSTPER", pClsProperty.CGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("CGSTAMOUNT", pClsProperty.CGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FCGSTAMOUNT", pClsProperty.FCGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SGSTPER", pClsProperty.SGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SGSTAMOUNT", pClsProperty.SGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FSGSTAMOUNT", pClsProperty.FSGSTAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("NETAMOUNT", pClsProperty.NETAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FGROSSAMOUNT", pClsProperty.FGROSSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FDISCOUNTAMOUNT", pClsProperty.FDISCOUNTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FINSURANCEAMOUNT", pClsProperty.FINSURANCEAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FSHIPPINGAMOUNT", pClsProperty.FSHIPPINGAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FGSTAMOUNT", pClsProperty.FGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FNETAMOUNT", pClsProperty.FNETAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);
                Ope.AddParams("STATUS", pClsProperty.STATUS, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SOURCE", pClsProperty.SOURCE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("PROCESS_ID", pClsProperty.PROCESS_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("TRANSPORTNAME", pClsProperty.TRANSPORTNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PLACEOFSUPPLY", pClsProperty.PLACEOFSUPPLY, DbType.String, ParameterDirection.Input);

                Ope.AddParams("COMPANYBANK_ID", pClsProperty.COMPANYBANK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PARTYBANK_ID", pClsProperty.PARTYBANK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("COURIER_ID", pClsProperty.COURIER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("AIRFREIGHT_ID", pClsProperty.AIRFREIGHT_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PLACEOFRECEIPTBYPRECARRIER", pClsProperty.PLACEOFRECEIPTBYPRECARRIER, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PORTOFLOADING", pClsProperty.PORTOFLOADING, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PORTOFDISCHARGE", pClsProperty.PORTOFDISCHARGE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALDESTINATION", pClsProperty.FINALDESTINATION, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COUNTRYOFORIGIN", pClsProperty.COUNTRYOFORIGIN, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COUNTRYOFFINALDESTINATION", pClsProperty.COUNTRYOFFINALDESTINATION, DbType.String, ParameterDirection.Input);


                Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);

                Ope.AddParams("VOUCHERNOSTR", pClsProperty.VOUCHERNOSTR, DbType.String, ParameterDirection.Input);

                Ope.AddParams("XMLDETSTR", StrMemoEntryDetXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("XMLSTR_OLDDB", StrMemoEntry_OldDB, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("MODE", pStrMode, DbType.String, ParameterDirection.Input);

                Ope.AddParams("BILLTYPE", pClsProperty.BILLTYPE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("INSURANCETYPE", pClsProperty.INSURANCETYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("GROSSWEIGHT", pClsProperty.GROSSWEIGHT, DbType.Double, ParameterDirection.Input);

                // #D: 19-08-2020
                Ope.AddParams("CONSIGNEE", pClsProperty.CONSIGNEE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ADDRESSTYPE", pClsProperty.ADDRESSTYPE, DbType.String, ParameterDirection.Input);
                // #D: 19-08-2020

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("STOCKTYPE", "Parcel", DbType.String, ParameterDirection.Input);

                Ope.AddParams("LESSPER1", pClsProperty.LessPer1, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("LESSPER2", pClsProperty.LessPer2, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("LESSPER3", pClsProperty.LessPer3, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADATAMT", pClsProperty.AdatAmt, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADATAMTFE", pClsProperty.AdatAmtFE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKERAMT", pClsProperty.BrokerAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKERAMTFE", pClsProperty.BrokerAmountFE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("STKAMTFE", pClsProperty.StkAmtFE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EXPINVOICEAMT", pClsProperty.ExpInvoiceAmt, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EXPINVOICEAMTFE", pClsProperty.ExpInvoiceAmtFE, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("ORG_EXCRATE", pClsProperty.ORG_EXCRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADDLESS_EXCRATE", pClsProperty.ADDLESS_EXCRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FROUNDOFFAMOUNT", pClsProperty.FROUNDOFFAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ROUND_ADDAMOUNT", pClsProperty.ROUND_ADDAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ROUND_LESSAMOUNT", pClsProperty.ROUND_LESSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SOAPPROVAL", pClsProperty.ORDERAPPROVAL, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("ORDERMEMO_ID", pClsProperty.ORDERMEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ORDERJANGEDNO", pClsProperty.ORDERJANGEDNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLFORMAT", pClsProperty.BILLFORMAT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("APARTAPPROVAL", pClsProperty.APARTAPPROVAL, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("ACCTYPE", pClsProperty.ACCTYPE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("FINALBUYER_ID", pClsProperty.FINALBUYER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("FINALADDRESS1", pClsProperty.FINALBUYERADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALADDRESS2", pClsProperty.FINALBUYERADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALADDRESS3", pClsProperty.FINALBUYERADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALCOUNTRY_ID", pClsProperty.FINALBUYERCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("FINALSTATE", pClsProperty.FINALBUYERSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALCITY", pClsProperty.FINALBUYERCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALZIPCODE", pClsProperty.FINALBUYERZIPCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ISCONSINGEE", pClsProperty.ISCONSINGEE, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(pConConnection, Config.ProviderName, "Trn_MemoEntryParcelSaveNew_SaleEntry", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnValueJanged = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;
        }


        public DataTable MissingMediaGetData()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Win_CheckImageExistsGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public Int32 MissingMediaSave(string pStrXML)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("XMLSTOCKUPLOAD", pStrXML, DbType.Xml, ParameterDirection.Input);
            return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "Win_CheckImageExistsSave", CommandType.StoredProcedure);
        }

        public DataTable StoneMediaGetData(string pStrStoneNo) //WITHOUTSTOCK OR WITHSTOCK
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("STOCKNO", pStrStoneNo, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_StockMediaFlagGetData", CommandType.StoredProcedure);

            return DTab;
        }

        public int StoneMediaSave(string pStrStockID,

            bool ISIMAGE,
            string IMAGEURL,
            bool ISCERTI,
            string CERTURL,
            bool ISVIDEO,
            string VIDEOURL
            ) //WITHOUTSTOCK OR WITHSTOCK
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("STOCK_ID", pStrStockID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ISIMAGE", ISIMAGE, DbType.Boolean, ParameterDirection.Input);
            Ope.AddParams("IMAGEURL", IMAGEURL, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ISCERTI", ISCERTI, DbType.Boolean, ParameterDirection.Input);
            Ope.AddParams("CERTURL", CERTURL, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ISVIDEO", ISVIDEO, DbType.Boolean, ParameterDirection.Input);
            Ope.AddParams("VIDEOURL", VIDEOURL, DbType.String, ParameterDirection.Input);

            return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "Trn_StockMediaFlagSave", CommandType.StoredProcedure);

        }

      
        public DataTable GetWebAppOrderPlacedNotificationData() //#P : 16-11-2021 : For Website/Application Order Placed Notification
        {
            DataTable DTab = new DataTable();
            try
            {
               
                Ope.ClearParams();
                Ope.AddParams("LEDGER_ID", Config.gEmployeeProperty.LEDGER_ID, DbType.String, ParameterDirection.Input);
                Ope.FillDTab(Config.ConnectionString, Config.ProviderName,DTab, "TRN_NotificationWebAppOrderPlaced", CommandType.StoredProcedure);
            }
            catch (System.Exception ex)
            {
                DTab = null;
            }
            return DTab;
        }
        public DataTable PrintJanged(string pStrMemo_ID, string pStrOPE) // Dhara : 22-11-2021
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MEMO_ID", pStrMemo_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("OPE", pStrOPE, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_JangedGetPrintSingle", CommandType.StoredProcedure);
            return DTab;
        }
      
        
        public DataSet BranchReceiveGetData(
         int pIntProcessID,
         string pStrFromDate,
         string pStrToDate,
         string pStrMemoNo,
         string pStrStoneNo,
         string pStrBillingPartyID,
         string pStrShippingPartyID,
         string pStrStatus,
         string pStrMemoID,
          string pStrReceiveStatus,
          string pStrLabReportNo,
          string pStrBuyerPartyID
         ) //D: 18022021 FOR GET BRANCH RECEIVE DATA
        {

            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("MEMONO", pStrMemoNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PROCESS_ID", pIntProcessID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STONENO", pStrStoneNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGPARTY_ID", pStrBillingPartyID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGPARTY_ID", pStrShippingPartyID, DbType.String, ParameterDirection.Input);

            Ope.AddParams("STATUS", pStrStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMO_ID", pStrMemoID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LABREPORTNO", pStrLabReportNo, DbType.String, ParameterDirection.Input);

            Ope.AddParams("RECEIVESTATUS", pStrReceiveStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BUYERPARTY_ID", pStrBuyerPartyID, DbType.String, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Table", "ACC_BranchReceiveGetData", CommandType.StoredProcedure);

            return DS;
        }

        public MemoEntryProperty SaveSalesRetEntry(SqlConnection pConConnection, MemoEntryProperty pClsProperty, string StrMemoEntryDetXML, string pStrMode, string StrMemoEntry_OldDB, string StrMemoEntryDetXMLParcel, bool IS_ConsiRet)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("SRNO", pClsProperty.SRNO, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("ISCHECKED", pClsProperty.ISCHECKED, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("JANGEDNOSTR", pClsProperty.JANGEDNOSTR, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMONO", pClsProperty.MEMONO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMOTYPE", pClsProperty.MEMOTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMODATE", pClsProperty.MEMODATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("BILLINGPARTY_ID", pClsProperty.BILLINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGPARTY_ID", pClsProperty.SHIPPINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("BROKER_ID", pClsProperty.BROKER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("BROKRAGEBASEPER", pClsProperty.BROKERBASEPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROKRAGEPROFITPER", pClsProperty.BROKERPROFITPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("ADAT_ID", pClsProperty.ADAT_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ADATPER", pClsProperty.ADATPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("SOLDPARTY_ID", pClsProperty.SOLDPARTY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("SELLER_ID", pClsProperty.SELLER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("TERMS_ID", pClsProperty.TERMS_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TERMSDAYS", pClsProperty.TERMSDAYS, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TERMSPER", pClsProperty.TERMSPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("TERMSDATE", pClsProperty.TERMSDATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("CURRENCY_ID", pClsProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("EXCRATE", pClsProperty.EXCRATE, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("DELIVERYTYPE", pClsProperty.DELIVERYTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PAYMENTMODE", pClsProperty.PAYMENTMODE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGADDRESS1", pClsProperty.BILLINGADDRESS1, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGADDRESS2", pClsProperty.BILLINGADDRESS2, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGADDRESS3", pClsProperty.BILLINGADDRESS3, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGCOUNTRY_ID", pClsProperty.BILLINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("BILLINGSTATE", pClsProperty.BILLINGSTATE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGCITY", pClsProperty.BILLINGCITY, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGZIPCODE", pClsProperty.BILLINGZIPCODE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGADDRESS1", pClsProperty.SHIPPINGADDRESS1, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGADDRESS2", pClsProperty.SHIPPINGADDRESS2, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGADDRESS3", pClsProperty.SHIPPINGADDRESS3, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGCOUNTRY_ID", pClsProperty.SHIPPINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGSTATE", pClsProperty.SHIPPINGSTATE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGCITY", pClsProperty.SHIPPINGCITY, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGZIPCODE", pClsProperty.SHIPPINGZIPCODE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TOTALPCS", pClsProperty.TOTALPCS, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TOTALCARAT", pClsProperty.TOTALCARAT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("TOTALAVGDISC", pClsProperty.TOTALAVGDISC, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("TOTALAVGRATE", pClsProperty.TOTALAVGRATE, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("GROSSAMOUNT", pClsProperty.GROSSAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("DISCOUNTPER", pClsProperty.DISCOUNTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("DISCOUNTAMOUNT", pClsProperty.DISCOUNTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("INSURANCEPER", pClsProperty.INSURANCEPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("INSURANCEAMOUNT", pClsProperty.INSURANCEAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGPER", pClsProperty.SHIPPINGPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGAMOUNT", pClsProperty.SHIPPINGAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("GSTPER", pClsProperty.GSTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("GSTAMOUNT", pClsProperty.GSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("IGSTPER", pClsProperty.IGSTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("IGSTAMOUNT", pClsProperty.IGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FIGSTAMOUNT", pClsProperty.FIGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("CGSTPER", pClsProperty.CGSTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("CGSTAMOUNT", pClsProperty.CGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FCGSTAMOUNT", pClsProperty.FCGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("SGSTPER", pClsProperty.SGSTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("SGSTAMOUNT", pClsProperty.SGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FSGSTAMOUNT", pClsProperty.FSGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("NETAMOUNT", pClsProperty.NETAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FGROSSAMOUNT", pClsProperty.FGROSSAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FDISCOUNTAMOUNT", pClsProperty.FDISCOUNTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FINSURANCEAMOUNT", pClsProperty.FINSURANCEAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FSHIPPINGAMOUNT", pClsProperty.FSHIPPINGAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FGSTAMOUNT", pClsProperty.FGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FNETAMOUNT", pClsProperty.FNETAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STATUS", pClsProperty.STATUS, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SOURCE", pClsProperty.SOURCE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PROCESS_ID", pClsProperty.PROCESS_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("TRANSPORTNAME", pClsProperty.TRANSPORTNAME, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PLACEOFSUPPLY", pClsProperty.PLACEOFSUPPLY, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COMPANYBANK_ID", pClsProperty.COMPANYBANK_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("PARTYBANK_ID", pClsProperty.PARTYBANK_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("COURIER_ID", pClsProperty.COURIER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("AIRFREIGHT", pClsProperty.AIRFREIGHT, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PLACEOFRECEIPTBYPRECARRIER", pClsProperty.PLACEOFRECEIPTBYPRECARRIER, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PORTOFLOADING", pClsProperty.PORTOFLOADING, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PORTOFDISCHARGE", pClsProperty.PORTOFDISCHARGE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINALDESTINATION", pClsProperty.FINALDESTINATION, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COUNTRYOFORIGIN", pClsProperty.COUNTRYOFORIGIN, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COUNTRYOFFINALDESTINATION", pClsProperty.COUNTRYOFFINALDESTINATION, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);
            //Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINYEAR", pClsProperty.FINYEAR, DbType.String, ParameterDirection.Input);
            Ope.AddParams("VOUCHERNOSTR", pClsProperty.VOUCHERNOSTR, DbType.String, ParameterDirection.Input);
            Ope.AddParams("XMLDETSTR", StrMemoEntryDetXML, DbType.Xml, ParameterDirection.Input);
            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MODE", pStrMode, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLTYPE", pClsProperty.BILLTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLFORMAT", pClsProperty.BILLFORMAT, DbType.String, ParameterDirection.Input);
            Ope.AddParams("INSURANCETYPE", pClsProperty.INSURANCETYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("GROSSWEIGHT", pClsProperty.GROSSWEIGHT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("CONSIGNEE", pClsProperty.CONSIGNEE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ADDRESSTYPE", pClsProperty.ADDRESSTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SOAPPROVAL", pClsProperty.ORDERAPPROVAL, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("BILLNO", pClsProperty.BILLNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLDATE", pClsProperty.BILLDATE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TCSPER", pClsProperty.TCSPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("TCSAMOUNT", pClsProperty.TCSAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FTCSAMOUNT", pClsProperty.FTCSAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FROUNDOFFAMOUNT", pClsProperty.FROUNDOFFAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("ROUNDOFFTPYE", pClsProperty.ROUNDOFFTPYE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("CUSTOMPORT", pClsProperty.CUSTOMPORT, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLOFENTRY", pClsProperty.BILLOFENTRY, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ADCODE", pClsProperty.ADCODE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("HAWBNO", pClsProperty.HAWBNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MAWBNO", pClsProperty.MAWBNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLOFENTRYDATE", pClsProperty.BILLOFENTRYDATE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("KPCERTINO", pClsProperty.KPCERTINO, DbType.String, ParameterDirection.Input);

            // Add khushbu 22-05-21
            Ope.AddParams("BANKREFNO", pClsProperty.BankRefNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BROKERAGEAMOUNT", pClsProperty.BrokerAmount, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROKERAGEAMOUNTFE", pClsProperty.BrokerAmountFE, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("INVTERMS_ID", pClsProperty.InvTermsID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PURCHASE_ACCID", pClsProperty.PURCHASE_ACCID, DbType.Guid, ParameterDirection.Input);

            Ope.AddParams("IGSTREFNO", pClsProperty.IGST_RefNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("EXCRATETYPE", pClsProperty.ExcRateType, DbType.String, ParameterDirection.Input);
            //---
            Ope.AddParams("ACCTYPE", pClsProperty.ACCTYPE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("INVOICENO", pClsProperty.INVOICENO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BROKERAGEEXCRATE", pClsProperty.BASEBROKERAGEEXCRATE, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("ADATEXCRATE", pClsProperty.ADATEXCRATE, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("GRFREIGHT", pClsProperty.GRFREIGHT, DbType.Double, ParameterDirection.Input);

            //add shiv 03-06-22
            Ope.AddParams("BROTDSPER", pClsProperty.BROTDSPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROTDSRS", pClsProperty.BROTDSRS, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROTOTALAMT", pClsProperty.BROTOTALAMT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("IS_BROTDS", pClsProperty.IS_BROTDS, DbType.Boolean, ParameterDirection.Input);
            //Add shiv 23-02-22
            Ope.AddParams("IS_APARTRECEIVE", pClsProperty.IS_APARTRECEIVE, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("APARTAPPROVAL", pClsProperty.APARTAPPROVAL, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("CONSIGNEE_ID", pClsProperty.Consignee_Id, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("INTERMIDATORYBANK", pClsProperty.INTERMIDATORYBANK, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PRECARRIERBY", pClsProperty.PRECARRIERBY, DbType.String, ParameterDirection.Input);
            Ope.AddParams("OTHERSTONEPARTY_ID", pClsProperty.OTHERSTONEPARTY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("OTHERSTONEPARTYNAME", pClsProperty.OTHERSTONEPARTYNAME, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUSTOMEBROKER", pClsProperty.CustomeBroker, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUSTOMEDESIGNATION", pClsProperty.CustomeDesignation, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUSTOMEIDCARD", pClsProperty.CustomeIDCard, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BRNO", pClsProperty.BRNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("XMLSTR_OLDDB", StrMemoEntry_OldDB, DbType.Xml, ParameterDirection.Input);

            Ope.AddParams("BACKADDLESS", pClsProperty.BACKADDLESS, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("TERMSADDLESSPER", pClsProperty.TERMSADDLESSPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BLINDADDLESSPER", pClsProperty.BLINDADDLESSPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("ADATAMT", pClsProperty.AdatAmt, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("ADATAMTFE", pClsProperty.AdatAmtFE, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROKERAMT", pClsProperty.BrokerAmount, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROKERAMTFE", pClsProperty.BrokerAmountFE, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("STKAMTFE", pClsProperty.StkAmtFE, DbType.Double, ParameterDirection.Input);

            Ope.AddParams("FINALBUYER_ID", pClsProperty.FINALBUYER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("FINALADDRESS1", pClsProperty.FINALBUYERADDRESS1, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINALADDRESS2", pClsProperty.FINALBUYERADDRESS2, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINALADDRESS3", pClsProperty.FINALBUYERADDRESS3, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINALCOUNTRY_ID", pClsProperty.FINALBUYERCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("FINALSTATE", pClsProperty.FINALBUYERSTATE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINALCITY", pClsProperty.FINALBUYERCITY, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINALZIPCODE", pClsProperty.FINALBUYERZIPCODE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ISCONSINGEE", pClsProperty.ISCONSINGEE, DbType.Boolean, ParameterDirection.Input);
            Ope.AddParams("ORDERMEMO_ID", pClsProperty.ORDERMEMO_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ORDERJANGEDNO", pClsProperty.ORDERJANGEDNO, DbType.String, ParameterDirection.Input);

            Ope.AddParams("IS_OUTSIDESTONE", pClsProperty.IS_OUTSIDESTONE, DbType.Boolean, ParameterDirection.Input);

            Ope.AddParams("BROCGSTPER", pClsProperty.BROCGSTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROCGSTRS", pClsProperty.BROCGSTRS, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROSGSTPER", pClsProperty.BROSGSTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROSGSTRS", pClsProperty.BROSGSTRS, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROIGSTPER", pClsProperty.BROIGSTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROIGSTRS", pClsProperty.BROIGSTRS, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("IS_BROGST", pClsProperty.IS_BROGST, DbType.Boolean, ParameterDirection.Input);
            Ope.AddParams("XMLDETSTRPARCEL", StrMemoEntryDetXMLParcel, DbType.Xml, ParameterDirection.Input);

            //Add shiv 12-10-2022
            Ope.AddParams("IS_CONSIRET", IS_ConsiRet, DbType.Boolean, ParameterDirection.Input);

            Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

            ArrayList AL = Ope.ExeNonQueryWithOutParameter(pConConnection, Config.ProviderName, "Trn_SaleRetEntrySaveNew", CommandType.StoredProcedure);
            if (AL.Count != 0)
            {
                pClsProperty.ReturnValue = Val.ToString(AL[0]);
                pClsProperty.ReturnValueJanged = Val.ToString(AL[1]);
                pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
            }
            else
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnValueJanged = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = "";

            }
            return pClsProperty;
        }

        public MemoEntryProperty SaveSalesEntry(SqlConnection pConConnection, MemoEntryProperty pClsProperty, string StrMemoEntryDetXML, string pStrMode, string StrMemoEntry_OldDB, string StrMemoEntryDetXMLParcel)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("SRNO", pClsProperty.SRNO, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("ISCHECKED", pClsProperty.ISCHECKED, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("JANGEDNOSTR", pClsProperty.JANGEDNOSTR, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMONO", pClsProperty.MEMONO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMOTYPE", pClsProperty.MEMOTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMODATE", pClsProperty.MEMODATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("BILLINGPARTY_ID", pClsProperty.BILLINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGPARTY_ID", pClsProperty.SHIPPINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("BROKER_ID", pClsProperty.BROKER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("BROKRAGEBASEPER", pClsProperty.BROKERBASEPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROKRAGEPROFITPER", pClsProperty.BROKERPROFITPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("ADAT_ID", pClsProperty.ADAT_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ADATPER", pClsProperty.ADATPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("SOLDPARTY_ID", pClsProperty.SOLDPARTY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("SELLER_ID", pClsProperty.SELLER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("TERMS_ID", pClsProperty.TERMS_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TERMSDAYS", pClsProperty.TERMSDAYS, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TERMSPER", pClsProperty.TERMSPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("TERMSDATE", pClsProperty.TERMSDATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("CURRENCY_ID", pClsProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("EXCRATE", pClsProperty.EXCRATE, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("DELIVERYTYPE", pClsProperty.DELIVERYTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PAYMENTMODE", pClsProperty.PAYMENTMODE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGADDRESS1", pClsProperty.BILLINGADDRESS1, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGADDRESS2", pClsProperty.BILLINGADDRESS2, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGADDRESS3", pClsProperty.BILLINGADDRESS3, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGCOUNTRY_ID", pClsProperty.BILLINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("BILLINGSTATE", pClsProperty.BILLINGSTATE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGCITY", pClsProperty.BILLINGCITY, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGZIPCODE", pClsProperty.BILLINGZIPCODE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGADDRESS1", pClsProperty.SHIPPINGADDRESS1, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGADDRESS2", pClsProperty.SHIPPINGADDRESS2, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGADDRESS3", pClsProperty.SHIPPINGADDRESS3, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGCOUNTRY_ID", pClsProperty.SHIPPINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGSTATE", pClsProperty.SHIPPINGSTATE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGCITY", pClsProperty.SHIPPINGCITY, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGZIPCODE", pClsProperty.SHIPPINGZIPCODE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TOTALPCS", pClsProperty.TOTALPCS, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TOTALCARAT", pClsProperty.TOTALCARAT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("TOTALAVGDISC", pClsProperty.TOTALAVGDISC, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("TOTALAVGRATE", pClsProperty.TOTALAVGRATE, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("GROSSAMOUNT", pClsProperty.GROSSAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("DISCOUNTPER", pClsProperty.DISCOUNTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("DISCOUNTAMOUNT", pClsProperty.DISCOUNTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("INSURANCEPER", pClsProperty.INSURANCEPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("INSURANCEAMOUNT", pClsProperty.INSURANCEAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGPER", pClsProperty.SHIPPINGPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("SHIPPINGAMOUNT", pClsProperty.SHIPPINGAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("GSTPER", pClsProperty.GSTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("GSTAMOUNT", pClsProperty.GSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("IGSTPER", pClsProperty.IGSTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("IGSTAMOUNT", pClsProperty.IGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FIGSTAMOUNT", pClsProperty.FIGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("CGSTPER", pClsProperty.CGSTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("CGSTAMOUNT", pClsProperty.CGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FCGSTAMOUNT", pClsProperty.FCGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("SGSTPER", pClsProperty.SGSTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("SGSTAMOUNT", pClsProperty.SGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FSGSTAMOUNT", pClsProperty.FSGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("NETAMOUNT", pClsProperty.NETAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FGROSSAMOUNT", pClsProperty.FGROSSAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FDISCOUNTAMOUNT", pClsProperty.FDISCOUNTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FINSURANCEAMOUNT", pClsProperty.FINSURANCEAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FSHIPPINGAMOUNT", pClsProperty.FSHIPPINGAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FGSTAMOUNT", pClsProperty.FGSTAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FNETAMOUNT", pClsProperty.FNETAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STATUS", pClsProperty.STATUS, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SOURCE", pClsProperty.SOURCE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PROCESS_ID", pClsProperty.PROCESS_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("TRANSPORTNAME", pClsProperty.TRANSPORTNAME, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PLACEOFSUPPLY", pClsProperty.PLACEOFSUPPLY, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COMPANYBANK_ID", pClsProperty.COMPANYBANK_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("PARTYBANK_ID", pClsProperty.PARTYBANK_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("COURIER_ID", pClsProperty.COURIER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("AIRFREIGHT", pClsProperty.AIRFREIGHT, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PLACEOFRECEIPTBYPRECARRIER", pClsProperty.PLACEOFRECEIPTBYPRECARRIER, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PORTOFLOADING", pClsProperty.PORTOFLOADING, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PORTOFDISCHARGE", pClsProperty.PORTOFDISCHARGE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINALDESTINATION", pClsProperty.FINALDESTINATION, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COUNTRYOFORIGIN", pClsProperty.COUNTRYOFORIGIN, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COUNTRYOFFINALDESTINATION", pClsProperty.COUNTRYOFFINALDESTINATION, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);
            //Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINYEAR", pClsProperty.FINYEAR, DbType.String, ParameterDirection.Input);
            Ope.AddParams("VOUCHERNOSTR", pClsProperty.VOUCHERNOSTR, DbType.String, ParameterDirection.Input);
            Ope.AddParams("XMLDETSTR", StrMemoEntryDetXML, DbType.Xml, ParameterDirection.Input);
            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MODE", pStrMode, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLTYPE", pClsProperty.BILLTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLFORMAT", pClsProperty.BILLFORMAT, DbType.String, ParameterDirection.Input);
            Ope.AddParams("INSURANCETYPE", pClsProperty.INSURANCETYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("GROSSWEIGHT", pClsProperty.GROSSWEIGHT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("CONSIGNEE", pClsProperty.CONSIGNEE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ADDRESSTYPE", pClsProperty.ADDRESSTYPE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SOAPPROVAL", pClsProperty.ORDERAPPROVAL, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("BILLNO", pClsProperty.BILLNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLDATE", pClsProperty.BILLDATE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TCSPER", pClsProperty.TCSPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("TCSAMOUNT", pClsProperty.TCSAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FTCSAMOUNT", pClsProperty.FTCSAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("FROUNDOFFAMOUNT", pClsProperty.FROUNDOFFAMOUNT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("ROUNDOFFTPYE", pClsProperty.ROUNDOFFTPYE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("CUSTOMPORT", pClsProperty.CUSTOMPORT, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLOFENTRY", pClsProperty.BILLOFENTRY, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ADCODE", pClsProperty.ADCODE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("HAWBNO", pClsProperty.HAWBNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MAWBNO", pClsProperty.MAWBNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLOFENTRYDATE", pClsProperty.BILLOFENTRYDATE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("KPCERTINO", pClsProperty.KPCERTINO, DbType.String, ParameterDirection.Input);

            // Add khushbu 22-05-21
            Ope.AddParams("BANKREFNO", pClsProperty.BankRefNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BROKERAGEAMOUNT", pClsProperty.BrokerAmount, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROKERAGEAMOUNTFE", pClsProperty.BrokerAmountFE, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("INVTERMS_ID", pClsProperty.InvTermsID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PURCHASE_ACCID", pClsProperty.PURCHASE_ACCID, DbType.Guid, ParameterDirection.Input);

            Ope.AddParams("IGSTREFNO", pClsProperty.IGST_RefNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("EXCRATETYPE", pClsProperty.ExcRateType, DbType.String, ParameterDirection.Input);
            //---
            Ope.AddParams("ACCTYPE", pClsProperty.ACCTYPE, DbType.String, ParameterDirection.Input);

            Ope.AddParams("INVOICENO", pClsProperty.INVOICENO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BROKERAGEEXCRATE", pClsProperty.BASEBROKERAGEEXCRATE, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("ADATEXCRATE", pClsProperty.ADATEXCRATE, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("GRFREIGHT", pClsProperty.GRFREIGHT, DbType.Double, ParameterDirection.Input);    

            //add shiv 03-06-22
            Ope.AddParams("BROTDSPER", pClsProperty.BROTDSPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROTDSRS", pClsProperty.BROTDSRS, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROTOTALAMT", pClsProperty.BROTOTALAMT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("IS_BROTDS", pClsProperty.IS_BROTDS, DbType.Boolean, ParameterDirection.Input);
            //Add shiv 23-02-22
            Ope.AddParams("IS_APARTRECEIVE", pClsProperty.IS_APARTRECEIVE, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("APARTAPPROVAL", pClsProperty.APARTAPPROVAL, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("CONSIGNEE_ID", pClsProperty.Consignee_Id, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("INTERMIDATORYBANK", pClsProperty.INTERMIDATORYBANK, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PRECARRIERBY", pClsProperty.PRECARRIERBY, DbType.String, ParameterDirection.Input);
            Ope.AddParams("OTHERSTONEPARTY_ID", pClsProperty.OTHERSTONEPARTY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("OTHERSTONEPARTYNAME", pClsProperty.OTHERSTONEPARTYNAME, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUSTOMEBROKER", pClsProperty.CustomeBroker, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUSTOMEDESIGNATION", pClsProperty.CustomeDesignation, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CUSTOMEIDCARD", pClsProperty.CustomeIDCard, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BRNO", pClsProperty.BRNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("XMLSTR_OLDDB", StrMemoEntry_OldDB, DbType.Xml, ParameterDirection.Input);

            Ope.AddParams("BACKADDLESS", pClsProperty.BACKADDLESS, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("TERMSADDLESSPER", pClsProperty.TERMSADDLESSPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BLINDADDLESSPER", pClsProperty.BLINDADDLESSPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("ADATAMT", pClsProperty.AdatAmt, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("ADATAMTFE", pClsProperty.AdatAmtFE, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROKERAMT", pClsProperty.BrokerAmount, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROKERAMTFE", pClsProperty.BrokerAmountFE, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("STKAMTFE", pClsProperty.StkAmtFE, DbType.Double, ParameterDirection.Input);

            Ope.AddParams("FINALBUYER_ID", pClsProperty.FINALBUYER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("FINALADDRESS1", pClsProperty.FINALBUYERADDRESS1, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINALADDRESS2", pClsProperty.FINALBUYERADDRESS2, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINALADDRESS3", pClsProperty.FINALBUYERADDRESS3, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINALCOUNTRY_ID", pClsProperty.FINALBUYERCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("FINALSTATE", pClsProperty.FINALBUYERSTATE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINALCITY", pClsProperty.FINALBUYERCITY, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FINALZIPCODE", pClsProperty.FINALBUYERZIPCODE, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ISCONSINGEE", pClsProperty.ISCONSINGEE, DbType.Boolean, ParameterDirection.Input);
            Ope.AddParams("ORDERMEMO_ID", pClsProperty.ORDERMEMO_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ORDERJANGEDNO", pClsProperty.ORDERJANGEDNO, DbType.String, ParameterDirection.Input);

            Ope.AddParams("IS_OUTSIDESTONE", pClsProperty.IS_OUTSIDESTONE, DbType.Boolean, ParameterDirection.Input);

            Ope.AddParams("BROCGSTPER", pClsProperty.BROCGSTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROCGSTRS", pClsProperty.BROCGSTRS, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROSGSTPER", pClsProperty.BROSGSTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROSGSTRS", pClsProperty.BROSGSTRS, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROIGSTPER", pClsProperty.BROIGSTPER, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("BROIGSTRS", pClsProperty.BROIGSTRS, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("IS_BROGST", pClsProperty.IS_BROGST, DbType.Boolean, ParameterDirection.Input);
            Ope.AddParams("XMLDETSTRPARCEL", StrMemoEntryDetXMLParcel, DbType.Xml, ParameterDirection.Input);
            Ope.AddParams("SBNo", pClsProperty.SBNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ConsignmentRefNo", pClsProperty.ConsignmentRefNo, DbType.String, ParameterDirection.Input);

            Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

            ArrayList AL = Ope.ExeNonQueryWithOutParameter(pConConnection, Config.ProviderName, "Trn_SaleEntrySaveNew", CommandType.StoredProcedure);
            if (AL.Count != 0)
            {
                pClsProperty.ReturnValue = Val.ToString(AL[0]);
                pClsProperty.ReturnValueJanged = Val.ToString(AL[1]);
                pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
            }
            else
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnValueJanged = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = "";

            }
            return pClsProperty;
        }
        public DataTable PrintPacktingListRsSaleEntry(string pStrMemoID, string pPrintType) //Account
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MEMOID", pStrMemoID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("Type", pPrintType, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_PackingListSaleLocalPrint_SaleEntry", CommandType.StoredProcedure);
            return DTab;
        }
        public DataTable PrintParcel(string pStrMemoID, string pPrintType) //#P : 28-05-2022
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("MEMO_ID", pStrMemoID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("PRINTTYPE", pPrintType, DbType.String, ParameterDirection.Input);
Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_MemoEntryPrintParcel", CommandType.StoredProcedure);
           return DTab;
        }

        public DataTable GetOpeningClosingParcelReport(string pStrFromDate, string pStrToDate, string StrDepartment)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            //Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", StrDepartment, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FYID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            //Ope.AddParams("INVOICENO", StrInvoice, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_OpeningClosingParcelReport", CommandType.StoredProcedure);
            //Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_OpeningClosingParcelReport_17112022", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetOpeningClosingParcelReportDetail(string pStrStockType, string pStrCurrentDate, string StrDepartment)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("OPE", pStrStockType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CURRENTDATE", pStrCurrentDate, DbType.Date, ParameterDirection.Input);
            //Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", StrDepartment, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FYID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_OpeningClosingParcelReportDetail", CommandType.StoredProcedure);
            //Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_OpeningClosingParcelReportDetail_17112022", CommandType.StoredProcedure);

            return DTab;
        }

        public MemoEntryProperty SaveMemoEntryBranch(SqlConnection pConConnection, MemoEntryProperty pClsProperty, string StrMemoEntryDetXML, string pStrMode, string StrMemoEntry_OldDB)
        {
            try
            {

                Ope.ClearParams();
                DataTable DTab = new DataTable();

                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("JANGEDNOSTR", pClsProperty.JANGEDNOSTR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMONO", pClsProperty.MEMONO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMOTYPE", pClsProperty.MEMOTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MEMODATE", pClsProperty.MEMODATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("BILLINGPARTY_ID", pClsProperty.BILLINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGPARTY_ID", pClsProperty.SHIPPINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("BROKER_ID", pClsProperty.BROKER_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("THROUGH", pClsProperty.THROUGH, DbType.String, ParameterDirection.Input);

                Ope.AddParams("BROKRAGEBASEPER", pClsProperty.BROKERBASEPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKRAGEPROFITPER", pClsProperty.BROKERPROFITPER, DbType.Double, ParameterDirection.Input);


                Ope.AddParams("ADAT_ID", pClsProperty.ADAT_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ADATPER", pClsProperty.ADATPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SOLDPARTY_ID", pClsProperty.SOLDPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SELLER_ID", pClsProperty.SELLER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("TERMS_ID", pClsProperty.TERMS_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TERMSDAYS", pClsProperty.TERMSDAYS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TERMSPER", pClsProperty.TERMSPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TERMSDATE", pClsProperty.TERMSDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("CURRENCY_ID", pClsProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EXCRATE", pClsProperty.EXCRATE, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("DELIVERYTYPE", pClsProperty.DELIVERYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PAYMENTMODE", pClsProperty.PAYMENTMODE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("BILLINGADDRESS1", pClsProperty.BILLINGADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGADDRESS2", pClsProperty.BILLINGADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGADDRESS3", pClsProperty.BILLINGADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGCOUNTRY_ID", pClsProperty.BILLINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("BILLINGSTATE", pClsProperty.BILLINGSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGCITY", pClsProperty.BILLINGCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGZIPCODE", pClsProperty.BILLINGZIPCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS1", pClsProperty.SHIPPINGADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS2", pClsProperty.SHIPPINGADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS3", pClsProperty.SHIPPINGADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGCOUNTRY_ID", pClsProperty.SHIPPINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGSTATE", pClsProperty.SHIPPINGSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGCITY", pClsProperty.SHIPPINGCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGZIPCODE", pClsProperty.SHIPPINGZIPCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TOTALPCS", pClsProperty.TOTALPCS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TOTALCARAT", pClsProperty.TOTALCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TOTALAVGDISC", pClsProperty.TOTALAVGDISC, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TOTALAVGRATE", pClsProperty.TOTALAVGRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GROSSAMOUNT", pClsProperty.GROSSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DISCOUNTPER", pClsProperty.DISCOUNTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DISCOUNTAMOUNT", pClsProperty.DISCOUNTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INSURANCEPER", pClsProperty.INSURANCEPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("INSURANCEAMOUNT", pClsProperty.INSURANCEAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGPER", pClsProperty.SHIPPINGPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGAMOUNT", pClsProperty.SHIPPINGAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GSTPER", pClsProperty.GSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GSTAMOUNT", pClsProperty.GSTAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("IGSTPER", pClsProperty.IGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("IGSTAMOUNT", pClsProperty.IGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FIGSTAMOUNT", pClsProperty.FIGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("CGSTPER", pClsProperty.CGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("CGSTAMOUNT", pClsProperty.CGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FCGSTAMOUNT", pClsProperty.FCGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SGSTPER", pClsProperty.SGSTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("SGSTAMOUNT", pClsProperty.SGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FSGSTAMOUNT", pClsProperty.FSGSTAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("NETAMOUNT", pClsProperty.NETAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FGROSSAMOUNT", pClsProperty.FGROSSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FDISCOUNTAMOUNT", pClsProperty.FDISCOUNTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FINSURANCEAMOUNT", pClsProperty.FINSURANCEAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FSHIPPINGAMOUNT", pClsProperty.FSHIPPINGAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FGSTAMOUNT", pClsProperty.FGSTAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FNETAMOUNT", pClsProperty.FNETAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);
                Ope.AddParams("STATUS", pClsProperty.STATUS, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SOURCE", pClsProperty.SOURCE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PROCESS_ID", pClsProperty.PROCESS_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("TRANSPORTNAME", pClsProperty.TRANSPORTNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PLACEOFSUPPLY", pClsProperty.PLACEOFSUPPLY, DbType.String, ParameterDirection.Input);

                Ope.AddParams("COMPANYBANK_ID", pClsProperty.COMPANYBANK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PARTYBANK_ID", pClsProperty.PARTYBANK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("COURIER_ID", pClsProperty.COURIER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("AIRFREIGHT", pClsProperty.AIRFREIGHT, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PLACEOFRECEIPTBYPRECARRIER", pClsProperty.PLACEOFRECEIPTBYPRECARRIER, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PORTOFLOADING", pClsProperty.PORTOFLOADING, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PORTOFDISCHARGE", pClsProperty.PORTOFDISCHARGE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALDESTINATION", pClsProperty.FINALDESTINATION, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COUNTRYOFORIGIN", pClsProperty.COUNTRYOFORIGIN, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COUNTRYOFFINALDESTINATION", pClsProperty.COUNTRYOFFINALDESTINATION, DbType.String, ParameterDirection.Input);
                Ope.AddParams("INTERMIDATORYBANK", pClsProperty.INTERMIDATORYBANK, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PRECARRIERBY", pClsProperty.PRECARRIERBY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CONSIGNEE_ID", pClsProperty.Consignee_Id, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);
                //Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR", pClsProperty.FINYEAR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("VOUCHERNOSTR", pClsProperty.VOUCHERNOSTR, DbType.String, ParameterDirection.Input);

                Ope.AddParams("XMLDETSTR", StrMemoEntryDetXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MODE", pStrMode, DbType.String, ParameterDirection.Input);

                Ope.AddParams("BILLTYPE", pClsProperty.BILLTYPE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("INSURANCETYPE", pClsProperty.INSURANCETYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("GROSSWEIGHT", pClsProperty.GROSSWEIGHT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("OTHERSTONEPARTY_ID", pClsProperty.OTHERSTONEPARTY_ID, DbType.Guid, ParameterDirection.Input); //ADD MILAN(04-02-20021)
                Ope.AddParams("OTHERSTONEPARTYNAME", pClsProperty.OTHERSTONEPARTYNAME, DbType.String, ParameterDirection.Input);//ADD MILAN ""

                // #D: 19-08-2020
                Ope.AddParams("CONSIGNEE", pClsProperty.CONSIGNEE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ADDRESSTYPE", pClsProperty.ADDRESSTYPE, DbType.String, ParameterDirection.Input);
                // #D: 19-08-2020

                //#K : 05122020
                Ope.AddParams("SOAPPROVAL", pClsProperty.ORDERAPPROVAL, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TCSPER", pClsProperty.TCSPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TCSAMOUNT", pClsProperty.TCSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FTCSAMOUNT", pClsProperty.FTCSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FROUNDOFFAMOUNT", pClsProperty.FROUNDOFFAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ROUNDOFFTPYE", pClsProperty.ROUNDOFFTPYE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("CUSTOMEBROKER", pClsProperty.CustomeBroker, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CUSTOMEDESIGNATION", pClsProperty.CustomeDesignation, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CUSTOMEIDCARD", pClsProperty.CustomeIDCard, DbType.String, ParameterDirection.Input);

                Ope.AddParams("BACKADDLESS", pClsProperty.BACKADDLESS, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TERMSADDLESSPER", pClsProperty.TERMSADDLESSPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BLINDADDLESSPER", pClsProperty.BLINDADDLESSPER, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("FINALBUYER_ID", pClsProperty.FINALBUYER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("FINALADDRESS1", pClsProperty.FINALBUYERADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALADDRESS2", pClsProperty.FINALBUYERADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALADDRESS3", pClsProperty.FINALBUYERADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALCOUNTRY_ID", pClsProperty.FINALBUYERCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("FINALSTATE", pClsProperty.FINALBUYERSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALCITY", pClsProperty.FINALBUYERCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALZIPCODE", pClsProperty.FINALBUYERZIPCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ISCONSINGEE", pClsProperty.ISCONSINGEE, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("INVOICENO", pClsProperty.INVOICENO, DbType.String, ParameterDirection.Input);

                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("EXPINVOICEAMT", pClsProperty.ExpInvoiceAmt, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EXPINVOICEAMTFE", pClsProperty.ExpInvoiceAmtFE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("APPROVEMEMO_ID", pClsProperty.APPROVEMEMO_ID_A, DbType.String, ParameterDirection.Input);

                // Add paramter by khushbu 19-08-21
                Ope.AddParams("ORG_EXCRATE", pClsProperty.ORG_EXCRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADDLESS_EXCRATE", pClsProperty.ADDLESS_EXCRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADATAMT", pClsProperty.AdatAmt, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADATAMTFE", pClsProperty.AdatAmtFE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKERAMT", pClsProperty.BrokerAmount, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKERAMTFE", pClsProperty.BrokerAmountFE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("STKAMTFE", pClsProperty.StkAmtFE, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("ORDERMEMO_ID", pClsProperty.ORDERMEMO_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ORDERJANGEDNO", pClsProperty.ORDERJANGEDNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLFORMAT", pClsProperty.BILLFORMAT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BRNO", pClsProperty.BRNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYTYPE", pClsProperty.ENTRYTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("XMLSTR_OLDDB", StrMemoEntry_OldDB, DbType.Xml, ParameterDirection.Input);

                //Add shiv 23-02-22
                Ope.AddParams("IS_APARTRECEIVE", pClsProperty.IS_APARTRECEIVE, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("APARTAPPROVAL", pClsProperty.APARTAPPROVAL, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ACCTYPE", pClsProperty.ACCTYPE, DbType.String, ParameterDirection.Input);

                //Add shiv 27-05-2022
                Ope.AddParams("BROKERAGEEXCRATE", pClsProperty.BASEBROKERAGEEXCRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ADATEXCRATE", pClsProperty.ADATEXCRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("GRFREIGHT", pClsProperty.GRFREIGHT, DbType.Double, ParameterDirection.Input);

                //Add shiv 03-06-2022
                Ope.AddParams("BROTDSPER", pClsProperty.BROTDSPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROTDSRS", pClsProperty.BROTDSRS, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROTOTALAMT", pClsProperty.BROTOTALAMT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("IS_BROTDS", pClsProperty.IS_BROTDS, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("PICKUP", pClsProperty.Pickup, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGPARTYNAME_FORBRANCH", pClsProperty.BILLINGPARTY_NAMEFORBRANCH, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(pConConnection, Config.ProviderName, "Trn_MemoEntryBranchSave", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnValueJanged = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;
        }

        public MemoEntryProperty DeleteBranchRcv(MemoEntryProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PURCHASE_ORDERMEMO_ID", pClsProperty.ORDERMEMO_ID, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL;
                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_MemoEntryBranchDelete", CommandType.StoredProcedure);

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

        public MemoEntryProperty UpdateISPrintFlag(MemoEntryProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_MemoEntryUpdatePrintFlag", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnValueJanged = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;
            }
            return pClsProperty;
        }

        public MemoEntryProperty UpdateAPartEntry(SqlConnection pConConnection, MemoEntryProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MEMO_ID", pClsProperty.ORDERMEMO_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(pConConnection, Config.ProviderName, "Trn_MemoEntryDeleteBPartAfterUpdateAPart", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnValueJanged = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;
            }
            return pClsProperty;
        }

        public DataTable GetStockDataForMemoDetail(Guid pGuidStock_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("STOCK_ID", pGuidStock_ID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_StockGetDataForMemoDetail", CommandType.StoredProcedure);
            return DTab;
        }
        public DataTable StockGetDataForMemo(string StrJangedNo)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("JANGEDNO", StrJangedNo, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_StockGetDataForMemo", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetOpeningClosingMarketingReport(string pStrFromDate, string pStrToDate, string StrDepartment)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            //Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", StrDepartment, DbType.String, ParameterDirection.Input);
            //Ope.AddParams("INVOICENO", StrInvoice, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_OpeningClosingMarketingReport", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetOpeningClosingMarketingReportDetail(string pStrStockType, string pStrCurrentDate, string StrDepartment)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("OPE", pStrStockType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CURRENTDATE", pStrCurrentDate, DbType.Date, ParameterDirection.Input);
            //Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", StrDepartment, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_OpeningClosingMarketingReportDetail", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable PrintPacktingListRsSaleEntryFileUpload(string pStrMemoID, string pPrintType) //Account
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MEMOID", pStrMemoID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("Type", pPrintType, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_PackingListSaleLocalPrint_SaleEntry_WithFileUpload", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable PrintRsExportFileUpload(string pStrMemoID, string pPrintType) //Account Export
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MEMOID", pStrMemoID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("Type", pPrintType, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Rpt_MemoEntryExportPrint_WithFileUpload", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetOpeningClosingMarketingReportDetail_SizeWise(string pStrStockType, string pStrCurrentDate, string StrDepartment)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("OPE", pStrStockType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("CURRENTDATE", pStrCurrentDate, DbType.Date, ParameterDirection.Input);
            //Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", StrDepartment, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_OpeningClosingMarketingReportDetail_SizeWise", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable PrintPacktingListRsSaleEntryFileUploadOP(string pStrMemoID, string pPrintType, string pOption)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MEMOID", pStrMemoID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("Type", pPrintType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("OP", pOption, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_PackingListSaleLocalPrint_SaleEntry_FileUpload_OP", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable PrintRsExportFileUploadOP(string pStrMemoID, string pPrintType, string pOption)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MEMOID", pStrMemoID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("Type", pPrintType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("OP", pOption, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Rpt_MemoEntryExportPrint_FileUpload_OP", CommandType.StoredProcedure);
            return DTab;
        }

       
        public MemoEntryProperty UpdateDate(MemoEntryProperty pClsProperty,string pStrMemoId) //cHNG : #P : 21-04-2022
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MEMO_ID", pStrMemoId, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_MemoUpdateDebitDate", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnValueJanged = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;
            }
            return pClsProperty;
        }

        public string BranchReceiveUpdateConsignConfirm(Guid pGuidMemo_ID, Guid pGuidMemoDetail_ID, Guid pGuidStock_ID)
        {
            string ReturnValue = "";
            string ReturnMessageType = "";
            string ReturnMessageDesc = "";
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MEMO_ID", pGuidMemo_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("MEMODETAIL_ID", pGuidMemoDetail_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("STOCK_ID", pGuidStock_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "ACC_BranchReceiveUpdate_ConsignConfirm", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    ReturnValue = Val.ToString(AL[0]);
                    ReturnMessageType = Val.ToString(AL[1]);
                    ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (Exception Ex)
            {
                ReturnValue = "";
                ReturnMessageType = "FAIL";
                ReturnMessageDesc = Ex.Message;
            }
            return ReturnMessageType;
        }

        public DataSet BranchReceiveGetDataConsignmentHK(
                string pStrFromDate,
                string pStrToDate,
                string pStrMemoNo,
                string pStrStoneNo,
                string pStrBillingPartyID,
                string pStrStatus
           )
        {

            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("MEMONO", pStrMemoNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STONENO", pStrStoneNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BILLINGPARTY_ID", pStrBillingPartyID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("STATUS", pStrStatus, DbType.String, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Table", "TRN_BranchReceiveGetData_Consignment", CommandType.StoredProcedure);
            return DS;
        }

        //Added By Gunjan:28/12/2023
        public DataTable GetOpeningClosingReportGetDataNew(string mStrFromDate, string mStrToDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("FromDate", mStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("ToDate", mStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_MemoOpeningClosingReport_New", CommandType.StoredProcedure);
            return DTab;
        }
        //End As Gunjan

        // Add by Urvisha :26/02/2024
        public MemoEntryProperty BranchPurchaseSave(MemoEntryProperty pClsProperty, string pStrMode,Guid GstCompany_ID)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR", pClsProperty.FINYEAR, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MODE", pStrMode, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COMPANY_ID", GstCompany_ID, DbType.Guid, ParameterDirection.Input);

                //Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNVALUEJANGED", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "ACC_BranchPurchaseSave", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnValueJanged = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[2]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[3]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnValueJanged = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;
        }


        public string PickupReturnUpdate(Guid pGuidMemoDetail_ID, string pStrRemark)//Add By Gunjan:11/03/2023
        {
            string ReturnValue = "";
            string ReturnMessageType = "";
            string ReturnMessageDesc = "";

            try
            {
                Ope.ClearParams();
                Ope.AddParams("MEMODETAIL_ID", pGuidMemoDetail_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PICKUP_RETURNREMARK", pStrRemark, DbType.String, ParameterDirection.Input);


                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "ACC_PickupReturnUpdate", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    ReturnValue = Val.ToString(AL[0]);
                    ReturnMessageType = Val.ToString(AL[1]);
                    ReturnMessageDesc = Val.ToString(AL[2]);
                }

            }
            catch (Exception Ex)
            {
                ReturnValue = "";
                ReturnMessageType = "FAIL";
                ReturnMessageDesc = Ex.Message;
            }
            return ReturnMessageType;
        }
        // End By Urvisha 

        public DataTable GetHKCashChargePer()
        {
            DataTable DTab = new DataTable();
            Ope.ClearParams();
            string StrQuery = "Select SETTINGVALUE from MST_Setting where SETTINGKEY = 'HKCashChargePer'";
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, StrQuery, CommandType.Text);
            return DTab;
        }

        public DataTable GetJangedPrintData(Int64 strJangedNo)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MEMONO", strJangedNo, DbType.Int64, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_MemoJangedPrintGetData", CommandType.StoredProcedure);
            return DTab;
        }
        public DataTable GetJangedPrintDataNew(Int64 strJangedNo)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MEMONO", strJangedNo, DbType.Int64, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_MemoJangedPrintGetDataNew", CommandType.StoredProcedure);
            return DTab;
        }
        public DataTable GetJangedPrintDataRJ(Int64 strJangedNo)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MEMONO", strJangedNo, DbType.Int64, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_JangedPrintGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetDataForSaleReportNew(string StrJangedNo,string StrMemoFetail_IDs = "")
        {
            DataTable DT = new DataTable();
            Ope.ClearParams();
            Ope.AddParams("JANGEDNO", StrJangedNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MemoDetailIds", StrMemoFetail_IDs, DbType.String, ParameterDirection.Input);
           
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "TRN_SaleDeliveryExportGetData", CommandType.StoredProcedure);
            return DT;
        }

        public DataTable GetDataForExport(string strMemoDetailIds)
        {
            Ope.ClearParams();
            Ope.AddParams("MemoDetailIds", strMemoDetailIds, DbType.String, ParameterDirection.Input);
            DataTable DT = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DT, "TRN_HKExportGetData", CommandType.StoredProcedure);
            return DT;
        }
        public MemoEntryProperty UpdateMemoData(string strMemo_Id, MemoEntryProperty pClsProperty, string strApprove)
        {
            try
            {

                Ope.ClearParams();

                Ope.AddParams("MEMO_IDs", strMemo_Id, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGPARTY_ID", pClsProperty.SHIPPINGPARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS", pClsProperty.SHIPPINGADDRESS1, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("HKAPPROVAL", strApprove, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("UPDATEBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("UPDATEIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_MemoListUpdateData", CommandType.StoredProcedure);


                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }

            catch (Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;
            }
            return pClsProperty;
        }

        public DataSet GetPendingPaymentData(string pStrBillingPartyID,string pStrMemoID,string StrType)
        {

            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("BILLINGPARTY_ID", pStrBillingPartyID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMO_ID", pStrMemoID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("DIAMONDTYPE", StrType, DbType.String, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Table", "Trn_PendingPaymentGetData", CommandType.StoredProcedure);
            return DS;
        }
        public NotificationSendAndReceive UpdatePaymentOrderNo(NotificationSendAndReceive pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("ORDERNO", pClsProperty.ORDERNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SALENO", pClsProperty.SALENO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_PaymentSaleNoUpdate", CommandType.StoredProcedure);

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

        public MemoEntryProperty UpdateSaleExcRate(MemoEntryProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EXCRATE", pClsProperty.EXCRATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_UpdateSaleExcRate", CommandType.StoredProcedure);

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
        public DataSet GetPaymentReceiveData(string pStrBillingPartyID, string pStrMemoID, string StrType ,Boolean IsAll,bool ISClear)
        {

            Ope.ClearParams();
            DataSet DS = new DataSet();

            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("BILLINGPARTY_ID", pStrBillingPartyID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MEMO_ID", pStrMemoID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("DIAMONDTYPE", StrType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SELLER_ID", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ISALL", IsAll, DbType.Boolean, ParameterDirection.Input);
            Ope.AddParams("ISCLEAR", ISClear, DbType.Boolean, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Table", "Trn_PaymentReceiveGetData", CommandType.StoredProcedure);
            return DS;
        }
        public NotificationSendAndReceive PaymentReceiveAndSale(NotificationSendAndReceive pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MEMO_ID", pClsProperty.MEMO_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("ISPAYMENTRECEIVE", pClsProperty.ISRECEIVE, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("ISPAYMENTCLEAR", pClsProperty.ISCLEAR, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_PaymentReceiveUpdate", CommandType.StoredProcedure);

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
        public DataTable GetAssortJangedPrintData(Int64 strJangedNo)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("JANGEDNO", strJangedNo, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "RPT_AssortIssueJangedPrintGetData", CommandType.StoredProcedure);
            return DTab;
        }
    }
}
