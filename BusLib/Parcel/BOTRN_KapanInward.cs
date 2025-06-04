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
    public class BOTRN_KapanInward
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public ParcelKapanInwardProperty Save(ParcelKapanInwardProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("INWARDNO", pClsProperty.INWARDNO, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("INWARDDATE", pClsProperty.INWARDDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("KAPANINWARDXML", pClsProperty.StrInwardXml, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("INWARDXML", pClsProperty.StrXmlValuesForInwardXml, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);//23-12-2021

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_KapanInwardSave", CommandType.StoredProcedure);

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

        public DataTable FillSummary(string pStrInwardNo, string pStrKapan, string pStrFromData, string pStrToDate, string pStrStatus)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", "SUMMARY", DbType.String, ParameterDirection.Input);
            Ope.AddParams("INWARDNO", pStrInwardNo, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromData, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("INWARDDATE", null, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("STATUS", pStrStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_KapanInwardGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable FillDetail(string pStrInwardNo, string pStrInwardDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", "DETAIL", DbType.String, ParameterDirection.Input);
            Ope.AddParams("INWARDNO", pStrInwardNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("INWARDDATE", pStrInwardDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_KapanInwardGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public ParcelKapanInwardProperty Delete(ParcelKapanInwardProperty pClsProperty)
        {
            try
            {

                Ope.ClearParams();
                Ope.AddParams("INWARDNO", pClsProperty.INWARDNO, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("INWARD_ID", pClsProperty.INWARD_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);


                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_KapanInwardDelete", CommandType.StoredProcedure);

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

        #region Assortment View

        public DataSet AssortmentViewGetData(string pStrInwardNo, string pStrKapan, string pStrFromData, string pStrToDate, string pStrStatus)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("INWARDNO", pStrInwardNo, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromData, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("STATUS", pStrStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Parcel_AssortmentViewGetData", CommandType.StoredProcedure);
            return DS;
        }

        #endregion


        #region Assortment View

        public DataSet AssortmentViewKapanSummaryExportExcel(string pStrKapan, string pStrFormateName)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
            Ope.AddParams("OPE", pStrFormateName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Parcel_AssortmentViewKapanSummaryExport", CommandType.StoredProcedure);
            return DS;
        }


        public DataSet AssortmentViewKapanSummaryGetData(string pStrKapan, string pStrMergeKapan)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MERGEKAPANNAME", pStrMergeKapan, DbType.String, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Parcel_AssortmentViewKapanSummaryGetData", CommandType.StoredProcedure);
            return DS;
        }

        public DataSet AssortmentViewKapanDeptSummaryGetData(string pStrKapan) // ADD: Dhara : 10-05-2021
        {
            Ope.ClearParams();
            DataSet Ds = new DataSet();
            Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, Ds, "TEMP", "Parcel_AssortmentViewKapanDeptSummaryGetData", CommandType.StoredProcedure);
            //Ope.FillDSet(Config.ConnectionString, Config.ProviderName, Ds, "TEMP", "Parcel_AssortmentViewKapanDeptSummaryGetData_PINALI", CommandType.StoredProcedure);
            return Ds;
        }


        #endregion

        #region Size Assortment

        public DataTable SizeAssortmentGetKapanData(string pStrInwardNo, string pStrKapan, string pStrFromData, string pStrToDate, string pStrStatus)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", "KAPAN", DbType.String, ParameterDirection.Input);
            Ope.AddParams("INWARDNO", pStrInwardNo, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromData, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("STATUS", pStrStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_SizeAssortmentGetData", CommandType.StoredProcedure);
            return DTab;
        }


        public DataTable SizeAssortmentGetSizeData(Guid pStrInwardID, int pIntShapeID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", "DETAIL", DbType.String, ParameterDirection.Input);
            Ope.AddParams("INWARD_ID", pStrInwardID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", pIntShapeID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_SizeAssortmentGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public ParcelKapanInwardProperty BombaySizeAssortmentSave(ParcelKapanInwardProperty pClsProperty, string Type)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("SIZEASSORTXML", pClsProperty.StrInwardXml, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MERGESUMMARY_ID", pClsProperty.TRANSFERNO, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("BYSIZEASSORT_ID", pClsProperty.BYSIZEASSORT_ID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("SURATMERGETRANSFERNO", pClsProperty.SuratMergeTransferNo, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TYPE", Type, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_BombaySizeAssortmentSave", CommandType.StoredProcedure);

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

        public ParcelKapanInwardProperty SizeAssortmentDelete(ParcelKapanInwardProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("INWARD_ID", pClsProperty.INWARD_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);


                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_SizeAssortmentDelete", CommandType.StoredProcedure);

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

        #endregion



        #region Clarity Assortment

        public DataTable ClarityAssortmentGetKapanData(string pStrKapan, string pStrFromData, string pStrToDate, string pStrStatus, string pStrMixSizeID, Guid pGuidUser_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", "SIZE", DbType.String, ParameterDirection.Input);
            //Ope.AddParams("INWARDNO", pStrInwardNo, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromData, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("STATUS", pStrStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MIXSIZE_ID", pStrMixSizeID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("USER_ID", pGuidUser_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_ClarityAssortmentGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable ClarityAssortmentGetSizeData(string pKapanName, Guid pStrSizeAssortID, int pIntDepartmentID, string pStrMixSize_ID, Int32 pStrShape_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", "DETAIL", DbType.String, ParameterDirection.Input);
            Ope.AddParams("KAPANNAME", pKapanName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SIZEASSORT_ID", pStrSizeAssortID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", pIntDepartmentID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("MIXSIZE_ID", pStrMixSize_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", pStrShape_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_ClarityAssortmentGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public ParcelKapanInwardProperty ClarityAssortmentSave(ParcelKapanInwardProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("KAPANNAME", pClsProperty.KAPANNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MIXSIZE_ID", pClsProperty.MIXSIZE_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHAPE_ID", pClsProperty.SHAPE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SIZEASSORT_ID", pClsProperty.SIZEASSORT_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("SIZEASSORTXML", pClsProperty.StrInwardXml, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("DEPARTMENT_ID", pClsProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);


                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);


                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_ClarityAssortmentSave", CommandType.StoredProcedure);

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

        public ParcelKapanInwardProperty ClarityAssortmentDelete(ParcelKapanInwardProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("SIZEASSORT_ID", pClsProperty.SIZEASSORT_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("DEPARTMENT_ID", pClsProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);


                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_ClarityAssortmentDelete", CommandType.StoredProcedure);

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

        #endregion


        #region Bombay Transfer

        public DataSet BombayTransferGetData(string pStrKapanname, string pStrSize, string pStrClarity, Int64 pIntTransferNo, Guid pGuidUser_ID)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("KapanName", pStrKapanname, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MixSize_ID", pStrSize, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MixClarity_ID", pStrClarity, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TRANSFERNO", pIntTransferNo, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("USER_ID", pGuidUser_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Parcel_BombayTransferGetData", CommandType.StoredProcedure);
            return DS;
        }

        public DataTable BombayTransferGetPopupData(string pStrFromDate, string pStrToDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BombayTransferGetPopup", CommandType.StoredProcedure);
            return DTab;
        }


        public ParcelKapanInwardProperty BombayTransferSave(ParcelKapanInwardProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("TRANSFERNO", pClsProperty.TRANSFERNO, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("XMLDETAIL", pClsProperty.TransferDetailXml, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("XMLSUMMARY", pClsProperty.TransferSummaryXml, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_BombayTransferSaveNew", CommandType.StoredProcedure);

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



        public ParcelKapanInwardProperty BombayTransferDelete(ParcelKapanInwardProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("TRANSFERNO", pClsProperty.TRANSFERNO, DbType.Int64, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);


                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_BombayTransferDelete", CommandType.StoredProcedure);

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

        #endregion

        #region Bombay Receive

        public DataTable BombayReceiveGetData(string pStrFromDate, string pStrToDate, string pStrStatus, Int64 pIntTransferNo)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("TRANSFERNO", pIntTransferNo, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("STATUS", pStrStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);//23-12-2021

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BombayReceiveGetData", CommandType.StoredProcedure);
            return DTab;
        }
        public DataTable BombayReceiveGetDetail(string pStrTransferNo)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("TRANSFERNO", pStrTransferNo, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);//23-12-2021

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BombayReceiveGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable BombayReceiveGetPopupData(string pStrFromDate, string pStrToDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);//23-12-2021
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BombayReceiveGetPopup", CommandType.StoredProcedure);
            return DTab;
        }

        public ParcelKapanInwardProperty BombayReceiveSave(ParcelKapanInwardProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("XMLDETAIL", pClsProperty.TransferDetailXml, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);//23-12-2021

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_BombayReceiveSave", CommandType.StoredProcedure);

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

        public ParcelKapanInwardProperty BombayReceiveDelete(ParcelKapanInwardProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("XMLDETAIL", pClsProperty.TransferDetailXml, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_BombayReceiveDelete", CommandType.StoredProcedure);

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

        public DataTable GetDataForKapanCreate(string pStrXmlValues)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("KAPANINWARDXML", pStrXmlValues, DbType.Xml, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_KapanInwardSingleToMixGetData", CommandType.StoredProcedure);
            return DTab;
        }

        #endregion


        #region Bombay Transfer To Marketing

        public DataTable BombayTransferToMarketingGetData(string pStrFromDate, string pStrToDate, string pStrStatus, Int64 pIntTransferNo, string pStrMode)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("TRANSFERNO", pIntTransferNo, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("STATUS", pStrStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MODE", pStrMode, DbType.String, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BombayTransferToMarketingGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable BombayTransferToMarketingGetDetail(string pStrTransferNo)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("TRANSFERNO", pStrTransferNo, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BombayTransferToMarketingGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable BombayTransferToMarketingGetPopupData(string pStrFromDate, string pStrToDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BombayTransferToMarketingGetPopup", CommandType.StoredProcedure);
            return DTab;
        }

        public ParcelKapanInwardProperty BombayTransferToMarketingSave(ParcelKapanInwardProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("XMLDETAIL", pClsProperty.TransferDetailXml, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("MODE", pClsProperty.MODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_BombayTransferToMarketingSave", CommandType.StoredProcedure);

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


        public ParcelKapanInwardProperty BombayTransferToMarketingDelete(ParcelKapanInwardProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("XMLDETAIL", pClsProperty.TransferDetailXml, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_BombayTransferToMarketingDelete", CommandType.StoredProcedure);

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

        public string UpdateMappingClarity(string StrID, int IntClaID, int IntDeptId) //#Milan : 25-05-2021
        {
            Ope.ClearParams();
            Ope.AddParams("CLARITYASSORT_ID", StrID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("MERGEMIXCLARITY_ID", IntClaID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("MERGEDEPARTMENT_ID", IntDeptId, DbType.Int32, ParameterDirection.Input); // Add Khushbu 11-06-21

            int IntRes = Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "Parcel_UpDateMappingClarity", CommandType.StoredProcedure);
            if (IntRes > 0)
            {
                return "Success";
            }
            else
            {
                return "Fail";
            }
        }
        #endregion

        #region Bombay Transfer To Marketing One To Many

        public DataSet BombayTransferToMarketingOTOMGetData(string pStrFromDate, string pStrToDate, string pStrStatus, Int64 pIntTransferNo, string pStrMode)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("TRANSFERNO", pIntTransferNo, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("STATUS", pStrStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MODE", pStrMode, DbType.String, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS,"Table", "Parcel_BombayTransferToMarketingOTOMGetData", CommandType.StoredProcedure);
            return DS;
        }
        public ParcelKapanInwardProperty BombayTransferToMarketingOTOMSave(ParcelKapanInwardProperty pClsProperty) //#P : 14-10-2022
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("XMLDETAIL", pClsProperty.TransferDetailXml, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("XMLSUMMARY", pClsProperty.TransferSummaryXml, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("MODE", pClsProperty.MODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_BombayTransferToMarketingOTOMSave", CommandType.StoredProcedure);

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


        #endregion


        #region Department Transfer
        public ParcelKapanInwardProperty StockDepartmentTransfer(ParcelKapanInwardProperty pClsProperty) // D: 15-06-2021 :Dhara
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("DEPARTMENTXML", pClsProperty.STRDEPARTMENTXML, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);


                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_StockDeptTransferSave", CommandType.StoredProcedure);

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
        public DataTable DepartmentTransferToParcel(string pStrFromDate, string pStrToDate,int pIntDepartment_ID) // D: 17/06/2021
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", pIntDepartment_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "TRN_DepartmentTransferToPracelGetData", CommandType.StoredProcedure);
            return DTab;
        }
        public ParcelKapanInwardProperty DepartmentTransferSave(ParcelKapanInwardProperty pClsProperty) // D: 17/06/2021
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("XMLDETAIL", pClsProperty.TransferDetailXml, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_DepartmentTransferSave", CommandType.StoredProcedure);

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

        public DataTable BombayTransferClarityAssortmentGetSizeData(string pStrSizeAssortID, int pIntDepartmentID, string pStrMixClarityID, string pStrMixSize_ID, Int32 pStrShape_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("MIXSIZE_ID", pStrMixSize_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SIZEASSORT_ID", pStrSizeAssortID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", pIntDepartmentID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", pStrShape_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("MIXCLARITY_ID", pStrMixClarityID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);


            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BombayTransferClarityAssortmentGetData", CommandType.StoredProcedure);
            return DTab;
        }
        #endregion

        #region Surat Merge
        public DataSet SuratMergeGetData(string pStrKapanname, string pStrSize, string pStrClarity, Int64 pIntTransferNo, Guid pGuidUser_ID)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("KapanName", pStrKapanname, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MixSize_ID", pStrSize, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MixClarity_ID", pStrClarity, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TRANSFERNO", pIntTransferNo, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("USER_ID", pGuidUser_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "Parcel_SuratMergeGetData", CommandType.StoredProcedure);
            return DS;
        }

        public ParcelKapanInwardProperty SuratMergeSave(ParcelKapanInwardProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("TRANSFERNO", pClsProperty.TRANSFERNO, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("XMLDETAIL", pClsProperty.TransferDetailXml, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("XMLSUMMARY", pClsProperty.TransferSummaryXml, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_SuratMergeSaveNew", CommandType.StoredProcedure);

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
        #endregion

        #region Bombay Assortment //#P : 10-02-2022
        public DataTable BombayAssortmentGetKapanData(string pStrTrasferNo, string pStrFromData, string pStrToDate, string pStrStatus, Int64 pIntMergeSummary_ID, Guid pGuidUser_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", "MERGESUMMARY", DbType.String, ParameterDirection.Input);
            Ope.AddParams("TRANSFERNO", pStrTrasferNo, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromData, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("STATUS", pStrStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MERGESUMMARY_ID", pIntMergeSummary_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("USER_ID", pGuidUser_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BombayAssortmentGetData", CommandType.StoredProcedure);
            return DTab;
        }
        public DataTable BombayAssortmentGetSizeData(string pStrOpe, Int64 pIntMergeSummaryID, Int32 pIntShape_ID, Int32 pIntDepartment_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MERGESUMMARY_ID", pIntMergeSummaryID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", pIntShape_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", pIntDepartment_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BombayAssortmentGetData", CommandType.StoredProcedure);
            return DTab;
        }
        public DataTable BombayAssortmentGetClarityData(string pStrOpe, Int64 pIntMergeSummaryID, Int32 pIntShape_ID, Int32 pIntSize_ID, Int32 pIntDepartment_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MERGESUMMARY_ID", pIntMergeSummaryID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", pIntShape_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("MIXSIZE_ID", pIntSize_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", pIntDepartment_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BombayAssortmentGetData", CommandType.StoredProcedure);
            return DTab;
        }

        
        public ParcelKapanInwardProperty BombaySizeAssortmentDelete(ParcelKapanInwardProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("MERGESUMMARY_ID", pClsProperty.TRANSFERNO, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("SHAPE_ID", pClsProperty.SHAPE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("DEPARTMENT_ID", pClsProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);


                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_BombaySizeAssortmentDelete", CommandType.StoredProcedure);

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
        public ParcelKapanInwardProperty BombayClarityAssortmentDelete(ParcelKapanInwardProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                //Ope.AddParams("MergeSummary_ID", pClsProperty.TRANSFERNO, DbType.Int32, ParameterDirection.Input);
                //Ope.AddParams("ShapeId", pClsProperty.SHAPE_ID, DbType.Int32, ParameterDirection.Input);
                //Ope.AddParams("DeptId", pClsProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("BYSIZEASSORT_ID", pClsProperty.BYSIZEASSORT_ID, DbType.Int64, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);


                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_BombayClarityAssortmentDelete", CommandType.StoredProcedure);

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

        #endregion


        #region Bombay Dept Transfer //#P : 10-02-2022
        public ParcelKapanInwardProperty BombayAssortmentClarityDeptTransSave(ParcelKapanInwardProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("XMLDETAIL", pClsProperty.TransferDetailXml, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("XMLSUMMARY", pClsProperty.TransferSummaryXml, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_BombayDeptTransferSave", CommandType.StoredProcedure);

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
        public DataTable BombayDeptTransferGetPopupData(string pStrFromDate, string pStrToDate)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BombayDeptTransferGetPopup", CommandType.StoredProcedure);
            return DTab;
        }
        public DataTable BombayAssortmentDeptGetData(string TransferNo, Int32 pIntSize_ID, Int32 pIntDepartment_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("TransferNo", TransferNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", pIntDepartment_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BombayDeptTransferGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable BombayAssortmentDeptGetClarityData(string TransferNo, Int32 pIntShape_ID, Int32 pIntSize_ID, Int32 pIntDepartment_ID, Int32 pIntClarityID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("SuratMergeTransferNo", TransferNo, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", pIntShape_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("MIXSIZE_ID", pIntSize_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", pIntDepartment_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("MIXCLARITY_ID", pIntClarityID, DbType.Int32, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BombayAssortmentDeptGetData", CommandType.StoredProcedure);
            return DTab;
        }
        public ParcelKapanInwardProperty BombayDeptTransferDelete(ParcelKapanInwardProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("TRANSFERNO", pClsProperty.TRANSFERNO, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_BombayDeptTransferDelete", CommandType.StoredProcedure);
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
        #endregion
        #region Bombay Dept Receive //#P : 10-03-2022
        public DataTable ParcelDeptReceiveGetDataSizeWise(string pStrInwardNo, string pStrFromData, string pStrToDate, string pStrStatus, string pStrMixSizeID, Guid pGuidUser_ID) //Change : #P : 10-03-2022
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", "SIZEWISE", DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMDATE", pStrFromData, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("STATUS", pStrStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("MIXSIZE_ID", pStrMixSizeID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BombayDeptReceiveGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable ParcelDeptReceiveGetDataClarityWise(Int64 pIntByDeptTransNo, int pIntDepartmentID, Int32 pStrMixSize_ID, Int32 pStrShape_ID)  //Change : #P : 10-03-2022
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", "CLARITYWISE", DbType.String, ParameterDirection.Input);
            Ope.AddParams("BYDEPTTRANSFERNO", pIntByDeptTransNo, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("DEPARTMENT_ID", pIntDepartmentID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("MIXSIZE_ID", pStrMixSize_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SHAPE_ID", pStrShape_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Parcel_BombayDeptReceiveGetData", CommandType.StoredProcedure);
            return DTab;
        }
        public ParcelKapanInwardProperty ParcelDeptReceiveClarityWiseSave(ParcelKapanInwardProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("BYDEPTTRANSFERNO", pClsProperty.BYDEPTTRANSFERNO, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("CLARITYASSORTXML", pClsProperty.StrInwardXml, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("DEPARTMENT_ID", pClsProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SHAPE_ID", pClsProperty.SHAPE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("MIXSIZE_ID", pClsProperty.MIXSIZE_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);


                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_BombayDeptReceiveSave", CommandType.StoredProcedure);

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
        #endregion

        public DataSet GetClarityAssortmentData(string pStrKapan, bool IsBombayTransfer)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
            Ope.AddParams("IsBombayTransfer", IsBombayTransfer, DbType.Boolean, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "View_KapanAnalysisGetData", CommandType.StoredProcedure);
            return DS;
        }


        public DataTable GetDataForDSaleAnalysis(string FromDate, string ToDate) // Get Data For Stock Sales Analysis
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("FROMDATE", FromDate, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TODATE", ToDate, DbType.Date, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "RP_SaleAnalysisReport", CommandType.StoredProcedure);
            return DTab;
        }

        public ParcelKapanInwardProperty SizeAssortmentSave(ParcelKapanInwardProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("INWARD_ID", pClsProperty.INWARD_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("KAPANNAME", pClsProperty.KAPANNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SIZEASSORTXML", pClsProperty.StrInwardXml, DbType.Xml, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);


                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_SizeAssortmentSave", CommandType.StoredProcedure);

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

        public ParcelKapanInwardProperty SaveRate(ParcelKapanInwardProperty pClsProperty)
        {
            try
            {

                Ope.AddParams("RATEXML", pClsProperty.StrRateXml, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_ClarityAssortmentRateSave", CommandType.StoredProcedure);

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

        public ParcelKapanInwardProperty Parcel_BombayTransferNew_Insert(ParcelKapanInwardProperty pClsProperty)
        {
            try
            {

                Ope.AddParams("KAPANNAME", pClsProperty.KAPANNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("Carat", pClsProperty.CARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("PricePerCarat", pClsProperty.COSTPRICEPERCARAT, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("Amount", pClsProperty.COSTAMOUNT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("EntryBy", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("EntryIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "Parcel_BombayTransferNew_Insert", CommandType.StoredProcedure);

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
