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
    public class BOTRN_ParcelMerge
    {

        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();


        public DataTable GetMFGParcelStockUsingDataTable()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_GetMFGParcelUnVerifiedStock", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable ParcelGetStockAvailForMarketing()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_ParcelGetStockAvailForMarketing", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable GetParcelVerifiedStone()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_GetParcelVerifiedStock", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable GetParcelSecLevelMergeStoneDetail()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_GetParcelSecLevelMergeStoneDetail", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable GetParcelSecLevelMergeStoneSumry()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_GetParcelSecLevelMergeStoneSumry", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable GetParcelMergeMemoStockUsingDataTable()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_GetMergeMemowiseParcelStock", CommandType.StoredProcedure);

            return DTab;
        }

        public DataTable ParcelVerifiedStoneSave(string pStrXmlForVertifedDetail) //#P : 25-02-2020
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("XMLFORVERIDIEDDETAIL", pStrXmlForVertifedDetail, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_ParcelStockVerifiedSave", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable ParcelStockAvilMarketingSave(string pStrXmlForVertifedDetail) //#P : 25-02-2020
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("XMLFORVERIDIEDDETAIL", pStrXmlForVertifedDetail, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_ParcelStockAvilMarketingSave", CommandType.StoredProcedure);
            return DTab;
        }
        public DataTable ParcelStockAvilMarketingDirctFstLvlSave(string pStrXmlForVertifedDetail) //#P : 25-02-2020
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("XMLFORVERIDIEDDETAIL", pStrXmlForVertifedDetail, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_ParcelStockAvilMarketingDirctFstLvlSave", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable ParcelMemoWiseStoneMergeSave(string pStrXmlMergeStone, string pStrXmlDetailStonewise)
        {
            Ope.ClearParams();
            DataTable Dtab = new DataTable();
            Ope.AddParams("XMLFORVERIDIEDDETAIL", pStrXmlMergeStone, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("XMLFORDETAILMEMOWISE", pStrXmlDetailStonewise, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, Dtab, "TRN_ParcelMemoWiseStoneMergeSave", CommandType.StoredProcedure);
            return Dtab;

        }
        public DataTable ParcelSecondMergeSave(string pStrXmlMergeStone, string pStrXmlDetailStonewise)
        {
            Ope.ClearParams();
            DataTable Dtab = new DataTable();
            Ope.AddParams("XMLFORVERIDIEDDETAIL", pStrXmlMergeStone, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("XMLFORDETAILMEMOWISE", pStrXmlDetailStonewise, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, Dtab, "TRN_ParcelSecondMergeSave", CommandType.StoredProcedure);
            return Dtab;

        }
        public ParcelMixToMixConvertProperty ParcelMixToMixSave(ParcelMixToMixConvertProperty pMTMProperty)
        {
            try
            {
                Ope.ClearParams();
                DataTable Dtab = new DataTable();
                Ope.AddParams("STOCK_ID", pMTMProperty.STOCK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("STOCK_NO", pMTMProperty.STOCKNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHAPE_ID", pMTMProperty.SHAPE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MIXCLARITY_ID", pMTMProperty.MIXCLARITY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MIXSIZE_ID", pMTMProperty.MIXSIZE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("CARAT", pMTMProperty.CARAT, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("ACTCARAT", pMTMProperty.ACTCARAT, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("PRICE", pMTMProperty.PRICE, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("AMOUNT", pMTMProperty.AMOUNT, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("PARTY_ID", pMTMProperty.PARTY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_ParcelMixToMixSave", CommandType.StoredProcedure);

                if (AL.Count != 0)
                {
                    pMTMProperty.ReturnValue = Val.ToString(AL[0]);
                    pMTMProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pMTMProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pMTMProperty.ReturnValue = "";
                pMTMProperty.ReturnMessageType = "FAIL";
                pMTMProperty.ReturnMessageDesc = ex.Message;
            }

            return pMTMProperty;

        }
        public DataTable GetParcelLiveStock()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_GetParcelLiveStock", CommandType.StoredProcedure);

            return DTab;
        }
        public DataSet GetParcelConvertMixToMixHistory()
        {
            DataSet DS = new DataSet();
            Ope.ClearParams();
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "TRN_GetParcelConvertMixToMixHistory", CommandType.StoredProcedure);
            return DS;
        }
    }
}
