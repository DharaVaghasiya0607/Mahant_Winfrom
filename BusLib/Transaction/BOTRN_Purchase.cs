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

namespace BusLib.Transaction
{
    public class BOTRN_Purchase
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable PurchaseGetData(Int64 pStrInwardNo, string pStrFromDate, string pStrToDate)
        {
            Ope.ClearParams();

            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", "SUMMARY", DbType.String, ParameterDirection.Input);
            Ope.AddParams("INWARDNO", pStrInwardNo, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_Purchase_GetData", CommandType.StoredProcedure);
            return DTab;
        }

        public PurchaseProperty PurchaseSave(Guid NewTrn_Id,PurchaseProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("PURCHASE_ID", NewTrn_Id, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("INWARDNO",pClsProperty.INWARDNO, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("INWARDDATE", pClsProperty.INWARDDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("DUEDATE", pClsProperty.DUEDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("PARTY_ID", pClsProperty.PARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("CONVERSIONRATE", pClsProperty.CONVERSIONRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("TERMS_ID", pClsProperty.TERMS_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TERMS", pClsProperty.TERMS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("DISCOUNTPER", pClsProperty.DISCOUNTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FDISCOUNTPER", pClsProperty.FDISCOUNTPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("DISCOUNTAMT", pClsProperty.DISCOUNTAMT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FDISCOUNTAMT", pClsProperty.FDISCOUNTAMT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKERAGEPER", pClsProperty.BROKERAGEPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FBROKRAGEPER", pClsProperty.FBROKRAGEPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKERAGEAMT", pClsProperty.BROKERAGEAMT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("FBROKRAGEAMT", pClsProperty.FBROKRAGEAMT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("BROKER_ID", pClsProperty.BROKER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("DESCRIPTION", pClsProperty.DESCRIPTION, DbType.String, ParameterDirection.Input);

                Ope.AddParams("GROSSAMOUNT", pClsProperty.GROSSAMOUNT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("NETAMOUNT", pClsProperty.NETAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("CURRENCY_ID", pClsProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("XMLDETSTR", pClsProperty.XMLDETSTR, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_Purchase_InsertUpdate", CommandType.StoredProcedure);

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

        
        public DataTable Fill(Guid pStrPurchase_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", "DETAIL", DbType.String, ParameterDirection.Input);
            Ope.AddParams("PURCHASE_ID", pStrPurchase_ID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_Purchase_GetData", CommandType.StoredProcedure);
            
            return DTab;
        }

        public PurchaseProperty PurchaseDelete(PurchaseProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("PURCHASE_ID", pClsProperty.PURCHASE_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_Purchase_Delete", CommandType.StoredProcedure);

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


        public DataTable ReadyForAssortmentGetData(string pStrType,string StrStatus)//Gunjan:24/05/2023
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", pStrType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Status", StrStatus, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_ReadyForAssortment_GetData", CommandType.StoredProcedure);

            return DTab;
        }

        public PurchaseProperty SingleTransfer(PurchaseProperty pClsProperty, string strMFG_Id)//Gunjan:25/05/2023
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MFG_Id", strMFG_Id, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);


                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_SingleTransfer_Insert", CommandType.StoredProcedure);

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

        public PurchaseProperty CreateMixPacket(PurchaseProperty pClsProperty,string pStrType)//Gunjan:26/05/2023
        {
            try
            {
                Ope.ClearParams();

                Ope.AddParams("PACKET_ID", pClsProperty.Packet_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PACKETNO", pClsProperty.PacketNo, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("PACKETDATE", pClsProperty.PacketDate, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("TRANSFERTYPE", pStrType, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR_ID ",Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("XMLDETSTR", pClsProperty.XMLDETSTR, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_MixPacketCreate_Insert", CommandType.StoredProcedure);

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

        public DataTable ReadyForAssortmentSearchGetData(string pStrType, string pStrFromDate, string pStrToDate, string pStrStatus)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", pStrType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("STATUS", pStrStatus, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_ReadyForAssortment_SearchGetData", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable GetData_TransferID(string pStr_ID, string strOpe)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", strOpe, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TRANSFER_ID", pStr_ID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_ReadyForAssortment_GetDataByTrnID", CommandType.StoredProcedure);

            return DTab;
        }
        public long FindPacketNoMax()
        {
            string StrSql = "";
            Ope.ClearParams();
            Int64 IntNewID = Ope.FindNewID(Config.ConnectionString, Config.ProviderName, "TRN_MixPacketCreate", "MAX(PACKETNO)", StrSql);

            return IntNewID;
        }

        public PurchaseProperty ReadyForAssortment_Delete(PurchaseProperty pClsProperty,string strType)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("TRN_ID", pClsProperty.TRN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("TRANSFERTYPE", pClsProperty.TRANSFERTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TYPE ", strType, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_ReadyForAssortment_Delete", CommandType.StoredProcedure);

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
