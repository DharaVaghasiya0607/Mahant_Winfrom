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
    public class BOTRN_KapanCreate
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable GetKapanData(TrnKapanCreateProperty pClsProperty)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("LOT_ID", pClsProperty.LOT_ID, DbType.Guid, ParameterDirection.Input);
            //Ope.AddParams("TODATE", pClsProperty.TODATE, DbType.Date, ParameterDirection.Input);


            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_KapanCreateGetData", CommandType.StoredProcedure);
            return DTab;
        }       

        public DataTable GetRejectionData(string pStrRejectionFrom, TrnKapanCreateProperty pClsProperty)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("REJECTIONFROM", pStrRejectionFrom, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LOT_ID", pClsProperty.LOT_ID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_RejectionCreateGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public TrnKapanCreateProperty Save(TrnKapanCreateProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("KAPAN_ID", pClsProperty.KAPAN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("KAPANNAME", pClsProperty.KAPANNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("KAPANCATEGORY", pClsProperty.KAPANCATEGORY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COMPARMEMO", pClsProperty.COMPARMEMO, DbType.String, ParameterDirection.Input);

                Ope.AddParams("SUBLOT", pClsProperty.SUBLOT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SUBLOT1", pClsProperty.SUBLOT1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("KAPANDATE", pClsProperty.KAPANDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("LOT_ID", pClsProperty.LOT_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("KAPANPCS", pClsProperty.KAPANPCS, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("KAPANCARAT", pClsProperty.KAPANCARAT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("KAPANRATE", pClsProperty.KAPANRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("KAPANAMOUNT", pClsProperty.KAPANAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("EXPMAKPER", pClsProperty.EXPMAKPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EXPMAKCARAT", pClsProperty.EXPMAKCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EXPPOLPER", pClsProperty.EXPPOLPER, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EXPPOLCARAT", pClsProperty.EXPPOLCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("EXPDOLLAR", pClsProperty.EXPDOLLAR, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("ISNOTAPPLYANYLOCK", pClsProperty.ISNOTAPPLYANYLOCK, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);
                Ope.AddParams("KAPANTYPE", pClsProperty.KAPANTYPE, DbType.String, ParameterDirection.Input);

              //  Ope.AddParams("COMPANY_ID", Config.gEmployeeProperty.COMPANY_ID, DbType.Int64, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_KapanCreateSave", CommandType.StoredProcedure);

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

      

        public TrnKapanCreateProperty CheckValSaveKapanWithSublot(TrnKapanCreateProperty pClsProperty, string pStrOpe)  //Add : Pinali : 18-07-2019 :For Check Kapan Created With Proper Sublot Sequence
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("KAPANNAME", pClsProperty.KAPANNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SUBLOT", pClsProperty.SUBLOT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SUBLOT1", pClsProperty.SUBLOT1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("KAPAN_ID", pClsProperty.KAPAN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "CheckValSaveForKapanSublot", CommandType.StoredProcedure);
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

        public TrnKapanCreateProperty CheckValSaveKapanForRateAndAmount(TrnKapanCreateProperty pClsProperty, string KapanXmlDetail, string pStrEntryMode)  //Add : Pinali : 09-09-2020 : KapanAmount Not Greater than InvoiceAmount
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("XMLKAPANDETAIL", KapanXmlDetail, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("LOT_ID", pClsProperty.LOT_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYMODE", pStrEntryMode, DbType.Guid, ParameterDirection.Input);
                
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "CheckValSaveKapanWithRateAndAmount", CommandType.StoredProcedure);
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


        public DataTable GetDataForPurchaseLiveStock(bool pBoolDispAllLot)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

          //  Ope.AddParams("COMPANY_ID", Config.gEmployeeProperty.COMPANY_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("ISDISPLAYALLLOT", pBoolDispAllLot, DbType.Boolean, ParameterDirection.Input);


            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_PurchaseLiveStockGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataRow GetOrgAndBalCarat(Guid pIntLotID)
        {
            string StrQuery = "";

            Ope.ClearParams();

            StrQuery = "SELECT CARAT,BALANCECARAT FROM TRN_IMPORT WITH(NOLOCK) WHERE LOT_ID = '" + pIntLotID + "'";

            DataRow Dr = Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);

            return Dr;
        }

        public DataTable GetDataForKapanLiveStock(string pStrReport, string pStrKapanStatus)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("REPORT", pStrReport, DbType.String, ParameterDirection.Input);
            Ope.AddParams("KAPANSTATUS", pStrKapanStatus, DbType.String, ParameterDirection.Input);
            //Ope.AddParams("COMPANY_ID", Config.gEmployeeProperty.COMPANY_ID, DbType.Int64, ParameterDirection.Input);
            //Ope.AddParams("TODATE", pClsProperty.TODATE, DbType.Date, ParameterDirection.Input);


            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_PacketLiveStockGetData", CommandType.StoredProcedure);
            return DTab;
        }



        public DataTable GetDataForSingleKapanLiveStock(string pStrKapanStatus,Guid pGuidLot_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("KAPANSTATUS", pStrKapanStatus, DbType.String, ParameterDirection.Input);
           // Ope.AddParams("COMPANY_ID", Config.gEmployeeProperty.COMPANY_ID, DbType.Int64, ParameterDirection.Input);

            Ope.AddParams("LOT_ID", pGuidLot_ID, DbType.Guid, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_SingleKapanLiveStockGetData", CommandType.StoredProcedure);
            return DTab;
        }

        //public DataTable GetDataForSinglePacketLiveStock(string StrOpe, string pStrDisplay, string pStrKapanName, int pIntPacketNo, string pStrTag, bool pIsWithExtraStock)
        //{
        //    Ope.ClearParams();
        //    DataTable DTab = new DataTable();

        //    Ope.AddParams("OPE", StrOpe, DbType.String, ParameterDirection.Input);
        //  //  Ope.AddParams("COMPANY_ID", Config.gEmployeeProperty.COMPANY_ID, DbType.Int64, ParameterDirection.Input);
        //    Ope.AddParams("EMPLOYEE_ID", Config.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
        //    Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int64, ParameterDirection.Input);
        //    Ope.AddParams("DISPLAYTYPE", pStrDisplay, DbType.String, ParameterDirection.Input);

        //    Ope.AddParams("WITHEXTRASTOCK", pIsWithExtraStock, DbType.Boolean, ParameterDirection.Input);

        //    Ope.AddParams("KAPANNAME", pStrKapanName, DbType.String, ParameterDirection.Input);
        //    Ope.AddParams("PACKETNO", pIntPacketNo, DbType.Int32, ParameterDirection.Input);
        //    Ope.AddParams("TAG", pStrTag, DbType.String, ParameterDirection.Input);

        //    Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_SinglePacketLiveStockGetData", CommandType.StoredProcedure);
        //    return DTab;
        //}
        public DataTable GetDataForSinglePacketLiveStock(string StrOpe, string pStrDisplay, string pStrKapanName, 
            string pStrBarcode,
            string pStrRFID, int pStrTopktNo)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("OPE", StrOpe, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COMPANY_ID", Config.gEmployeeProperty.COMPANY_ID, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("EMPLOYEE_ID", Config.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("DISPLAYTYPE", pStrDisplay, DbType.String, ParameterDirection.Input);


            Ope.AddParams("KAPANNAME", pStrKapanName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BARCODE", pStrBarcode, DbType.String, ParameterDirection.Input);
            Ope.AddParams("RFIDTAG", pStrRFID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MANAGER_ID", Config.gEmployeeProperty.MANAGER_ID, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("TOPACKETNO", pStrTopktNo, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_SinglePacketLiveStockGetData", CommandType.StoredProcedure);
            return DTab;
        }


        public DataRow GetDataForSinglePacketLiveStockPacketInfo(string StrOpe, string pStrDisplay, string pStrKapanName, int pIntPacketNo, string pStrTag)
        {
            Ope.ClearParams();

            Ope.AddParams("OPE", StrOpe, DbType.String, ParameterDirection.Input);
          //  Ope.AddParams("COMPANY_ID", Config.gEmployeeProperty.COMPANY_ID, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("EMPLOYEE_ID", Config.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("DISPLAYTYPE", pStrDisplay, DbType.String, ParameterDirection.Input);

            Ope.AddParams("KAPANNAME", pStrKapanName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PACKETNO", pIntPacketNo, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TAG", pStrTag, DbType.String, ParameterDirection.Input);

            return Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, "Trn_SinglePacketLiveStockGetData", CommandType.StoredProcedure);
        }

        public DataRow GetDataForSinglePacketLiveStockCurrentOutstanding(string pStrKapanName, int pIntPacketNo, string pStrTag)
        {
            Ope.ClearParams();

            Ope.AddParams("KAPANNAME", pStrKapanName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PACKETNO", pIntPacketNo, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TAG", pStrTag, DbType.String, ParameterDirection.Input);

            return Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, "Trn_SinglePacketGetCurrentOutStanding", CommandType.StoredProcedure);
        }

        public DataTable GetPacketDataForBarcodePrint(string KapanName, string SubLot, string SubLot1)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("KAPAN_ID", null, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("KAPANNAME", KapanName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SUBLOT", SubLot, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SUBLOT1", SubLot1, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PACKETNO", 0, DbType.Int64, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_SinglePacketGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetPacketDataForGradingBarcodePrint(Int64 pIntJangedNo) //Add : Pinali : 16-08-2019 Used In Grading Barcode Print
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("JANGEDNO", pIntJangedNo, DbType.Int64, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_SinglePacketGradingBarcodePrintData", CommandType.StoredProcedure);
            return DTab;
        }
        public DataRow GetPacketDataForGradingBarcodePrint(string pStrKapan, Int32 pIntPacketNo, string pStrTag, Int64 pIntJangedNo = 0) //Add : Pinali : 16-08-2019 Used In Grading Barcode Print
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("JANGEDNO", pIntJangedNo, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PACKETNO", pIntPacketNo, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TAG", pStrTag, DbType.String, ParameterDirection.Input);
            return Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, "Trn_SinglePacketGradingBarcodePrintData", CommandType.StoredProcedure);
        }

        public DataRow GetBarcodePrintForLabIssueReturn(string KapanName, int pIntPacketNo, string pStrTag) //Add : Pinali : 12-08-2019
        {
            Ope.ClearParams();

            Ope.AddParams("KAPAN_ID", null, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("KAPANNAME", KapanName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PACKETNO", pIntPacketNo, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TAG", pStrTag, DbType.String, ParameterDirection.Input);

            return Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, "Trn_SinglePacketGetData", CommandType.StoredProcedure);
        }

        public DataTable GetPacketLiveStock(string pStrReport, string pStrPacketStatus)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("REPORT", pStrReport, DbType.String, ParameterDirection.Input);
         //   Ope.AddParams("COMPANY_ID", Config.gEmployeeProperty.COMPANY_ID, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("PACKETSTATUS", pStrPacketStatus, DbType.String, ParameterDirection.Input);

            //Ope.AddParams("TODATE", pClsProperty.TODATE, DbType.Date, ParameterDirection.Input);


            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_PacketLiveStockGetData", CommandType.StoredProcedure);
            return DTab;
        }


        public TrnKapanCreateProperty EditKapan(TrnKapanCreateProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("KAPAN_ID", pClsProperty.KAPAN_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("STATUS", pClsProperty.STATUS, DbType.String, ParameterDirection.Input);
                Ope.AddParams("KAPANNAME", pClsProperty.KAPANNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SUBLOT", pClsProperty.SUBLOT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SUBLOT1", pClsProperty.SUBLOT1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("KAPANCARAT", pClsProperty.KAPANCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("ISHIDE", pClsProperty.ISHIDE, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("COMPLETEDATE", pClsProperty.COMPLETEDATE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ISNOTAPPLYANYLOCK", pClsProperty.ISNOTAPPLYANYLOCK, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("LABOURAMOUNT", pClsProperty.LABOURAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("KAPANRATE", pClsProperty.KAPANRATE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("KAPANAMOUNT", pClsProperty.KAPANAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("OPE", pClsProperty.Ope, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_KapanCreateUpdateDelete", CommandType.StoredProcedure);

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

        public TrnPacketCreationProperty DeleteIssRetEntry(TrnPacketCreationProperty pSProperty)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("OPE", pSProperty.Ope, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PACKET_ID", pSProperty.PACKET_ID, DbType.Guid, ParameterDirection.Input);

            Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

            //Ope.AddParams("TODATE", pClsProperty.TODATE, DbType.Date, ParameterDirection.Input);


            ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_DelIssRetEntry", CommandType.StoredProcedure);

            if (AL.Count != 0)
            {
                pSProperty.ReturnValue = Val.ToString(AL[0]);
                pSProperty.ReturnMessageType = Val.ToString(AL[1]);
                pSProperty.ReturnMessageDesc = Val.ToString(AL[2]);
            }
            return pSProperty;
        }

        public TrnPacketCreationProperty PacketUpdateFromLiveStock(TrnPacketCreationProperty pSProperty)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("PACKET_ID", pSProperty.PACKET_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", pSProperty.SHAPE_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("PURITY_ID", pSProperty.PURITY_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("CHARNI_ID", pSProperty.CHARNI_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("ISSUEPCS", pSProperty.ISSUEPCS, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("ISSUECARAT", pSProperty.ISSUECARAT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("RETURNPCS", pSProperty.RETURNPCS, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("RETURNCARAT", pSProperty.RETURNCARAT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("LOSSCARAT", pSProperty.LOSSCARAT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("KACHAPCS", pSProperty.KACHAPCS, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("KACHACARAT", pSProperty.KACHACARAT, DbType.Double, ParameterDirection.Input);
            Ope.AddParams("CANCELPCS", pSProperty.CANCELPCS, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("CANCELCARAT", pSProperty.CANCELCARAT, DbType.Double, ParameterDirection.Input);

            Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

            ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_PacketUpdateFromLiveStock", CommandType.StoredProcedure);

            if (AL.Count != 0)
            {
                pSProperty.ReturnValue = Val.ToString(AL[0]);
                pSProperty.ReturnMessageType = Val.ToString(AL[1]);
                pSProperty.ReturnMessageDesc = Val.ToString(AL[2]);
            }
            return pSProperty;
        }

        public DataTable GetDataForSinglePacketFinalJamaInfo(string pStrKapanName, int pIntPacketNo, string pStrTag) //#P : 27-06-2020 : Used In FinalJama
        {
            DataTable DTab = new DataTable();
            Ope.ClearParams();

            Ope.AddParams("KAPANNAME", pStrKapanName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PACKETNO", pIntPacketNo, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TAG", pStrTag, DbType.String, ParameterDirection.Input);
            Ope.AddParams("EMPLOYEE_ID", Config.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int64, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_SinglePacketFinalJamaGetData", CommandType.StoredProcedure);
            //return Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, "Trn_SinglePacketFinalJamaGetData", CommandType.StoredProcedure);
            return DTab;
        }
        
        #region Grid Layout

        public int SaveGridLayout(string pStrFormName, string pStrGridName, string pStrGridLayout)
        {
            string StrQuery = "";

            Ope.ClearParams();

            StrQuery = "Delete From MST_GridLayout With(RowLock) Where Employee_ID = " + Config.gEmployeeProperty.LEDGER_ID + " And FormName='" + pStrFormName + "' And GridName='" + pStrGridName + "' ";
            StrQuery += " Insert Into MST_GridLayout With(RowLock) (Employee_ID,FormName,GridName,GridLayout) Values (" + Config.gEmployeeProperty.LEDGER_ID + ",'" + pStrFormName + "','" + pStrGridName + "','" + pStrGridLayout + "') ";

            return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
        }

        public int DeleteGridLayout(string pStrFormName, string pStrGridName)
        {
            string StrQuery = "";

            Ope.ClearParams();

            StrQuery = "Delete From MST_GridLayout With(RowLock) Where Employee_ID = " + Config.gEmployeeProperty.LEDGER_ID + " And FormName='" + pStrFormName + "' And GridName='" + pStrGridName + "' ";

            return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
        }


        public string GetGridLayout(string pStrFormName, string pStrGridName)
        {
            Ope.ClearParams();

            string StrQuery = " And Employee_ID = '" + Config.gEmployeeProperty.LEDGER_ID + "' AND FormName = '" + pStrFormName + "' And GridName = '" + pStrGridName + "'";

            return Ope.FindText(Config.ConnectionString, Config.ProviderName, "MST_GridLayout", "GridLayout", StrQuery);

        }
        public DataTable GetDataForSinglePacketAutoIssue(string pStrKapanName, int pIntPacketNo, string pStrTag, string pFromDate, string pToDate) //#D : 22-06-2020
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.AddParams("EMPLOYEE_ID", Config.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int64, ParameterDirection.Input);

            Ope.AddParams("KAPANNAME", pStrKapanName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PACKETNO", pIntPacketNo, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("TAG", pStrTag, DbType.String, ParameterDirection.Input);

            Ope.AddParams("FROMDATE", pFromDate, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TODATE", pToDate, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_SinglePacketAutoIssueGetData", CommandType.StoredProcedure);
            return DTab;
        }

        #endregion

        public DataTable GetDataForLiveStock()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            string strQuery = "select * from TRN_MumbaiTransferNew where isnull(ISMumbaiReceive , 0) = 0";
            Ope.FillDTab(Config.Surat_ConnectionString, Config.Surat_ProviderName, DTab, strQuery, CommandType.Text);
            return DTab;
        }
        public LiveStockProperty SaveMumbaiReceiveData(LiveStockProperty pClsProperty, string mumbaiReceiveXml)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MUMBAIRECEIVEXML", mumbaiReceiveXml, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("COMPANY_ID", BusLib.Configuration.BOConfiguration.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Trn_MumbaiReceiveInsert", CommandType.StoredProcedure);

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
        public int UpdateMumbaiReceiveFlag(string strHelium_Id)
        {
            try
            {
                Ope.ClearParams();
                string Str = "update Trn_MumbaiTransferNew set MumbaiRecDate = GetDate(),ISMumbaiReceive = '" + 1 + "' where Helium_ID in ('" + strHelium_Id + "') ";
                return Ope.ExeNonQuery(Config.Surat_ConnectionString, Config.Surat_ProviderName, Str, CommandType.Text);
            }

            catch (System.Exception ex)
            {
                return -1;
            }
        }
    }
}
