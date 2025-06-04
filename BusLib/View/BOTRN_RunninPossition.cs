using System;
using System.Data;
using AxonDataLib;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using BusLib.TableName;
using System.Collections;


namespace BusLib.View
{
    public class BOTRN_RunninPossition
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        

        #region Other Function

        public DataSet RunningPosstionData(string pStrFormType,string pStrStockCategory, string pStrStockType, string mStrKapan, Int32 mIntPacketNo, string pStrTag)
        {
            try
            {
                DataSet DS = new DataSet();

                Ope.ClearParams();
                Ope.AddParams("FORMTYPE", pStrFormType, DbType.String, ParameterDirection.Input);
                Ope.AddParams("STOCKCATEGORY", pStrStockCategory, DbType.String, ParameterDirection.Input);
                Ope.AddParams("STOCKTYPE", pStrStockType, DbType.String, ParameterDirection.Input);
                Ope.AddParams("KAPANNAME", mStrKapan, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PACKETNO", mIntPacketNo, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TAG", pStrTag, DbType.String, ParameterDirection.Input);
                //Ope.AddParams("COMPANY_ID", Config.gEmployeeProperty.COMPANY_ID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("EMPLOYEE_ID", Config.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int64, ParameterDirection.Input);

                Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "RP_SingleRunningPossition", CommandType.StoredProcedure);
                return DS;
            }
            catch (Exception Ex)
            {
               
                return null;
            }
        }


        public DataTable RunningPosstionDataWIPPivotReport(string pStrStockCategory, string pStrStockType, string mStrKapan, Int32 mIntPacketNo, string pStrTag)
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();
                Ope.AddParams("STOCKCATEGORY", pStrStockCategory, DbType.String, ParameterDirection.Input);
                Ope.AddParams("STOCKTYPE", pStrStockType, DbType.String, ParameterDirection.Input);
                Ope.AddParams("KAPANNAME", mStrKapan, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PACKETNO", mIntPacketNo, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TAG", pStrTag, DbType.String, ParameterDirection.Input);
                //Ope.AddParams("COMPANY_ID", Config.gEmployeeProperty.COMPANY_ID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("EMPLOYEE_ID", Config.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int64, ParameterDirection.Input);

                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "RP_SingleRunningPossitionProcessWisePivot", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }



        public DataSet FullKapanAnalysis(string pStrKapan, string pStrOpe, string pStrFromDate, string pStrToDate)
        {
            try
            {
                DataSet DS = new DataSet();

                Ope.ClearParams();
                Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
                Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FROMDATE", pStrFromDate, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TODATE", pStrToDate, DbType.String, ParameterDirection.Input);

                Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "RP_SingleKapanAnalysis", CommandType.StoredProcedure);
                return DS;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }


        public DataTable GetFactoryProductionReport(string pStrOpe, string pStrKapan, int pIntProcessID,Int64 pIntEmployeeID, string pStrFromDate, string pStrToDate)
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();
                Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
                Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PROCESS_ID", pIntProcessID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EMPLOYEE_ID", pIntEmployeeID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);

                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "RP_SingleFactoryProductionReport", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }

        public DataSet GetOtherProcessProductionReport(string pStrOpe, string pStrKapan, Int64 pIntEmployeeID, string pStrFromDate, string pStrToDate)
        {
            try
            {
                DataSet DS = new DataSet();

                Ope.ClearParams();
                Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);                
                Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);                
                Ope.AddParams("EMPLOYEE_ID", pIntEmployeeID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);

                Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "RP_SingleOtherProcessProductionReport", CommandType.StoredProcedure);
                return DS;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }

        public DataSet GetFactoryProductionLabour(string pStrOpe, string pStrKapan, int pIntProcessID, Int64 pIntEmployeeID, string pStrFromDate, string pStrToDate)
        {
            try
            {
                DataSet DS = new DataSet();

                Ope.ClearParams();
                Ope.AddParams("OPE", pStrOpe, DbType.String, ParameterDirection.Input);
                Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PROCESS_ID", pIntProcessID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EMPLOYEE_ID", pIntEmployeeID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "RP_SingleFactoryProductionLabourReportNew", CommandType.StoredProcedure);
                return DS;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }

        public DataTable GetFactoryProductionLabourDetail(string pStrKapan, int pIntPacketNo, string pStrTag, string pStrPacketID, Int64 pIntEmployeeID)
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();
                Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PACKETNO", pIntPacketNo, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("TAG", pStrTag, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PACKET_ID", pStrPacketID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("EMPLOYEE_ID", pIntEmployeeID, DbType.Int64, ParameterDirection.Input);

                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "RP_SingleFactoryProductionLabourDetail", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }

        public int UpdateLiveGradingColorStatus(string pStrKapan, int pIntProcessID, Int64 pIntEmployeeID, string pStrFromDate, string pStrToDate)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("WAGESBASE", "GRADING", DbType.String, ParameterDirection.Input);
                Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PROCESS_ID", pIntProcessID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EMPLOYEE_ID", pIntEmployeeID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                int IntRes =  Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "Job_SingleFactoryProductionLabourToUpdateGradingColor", CommandType.StoredProcedure);

                Ope.ClearParams();
                Ope.AddParams("WAGESBASE", "BOMBAY", DbType.String, ParameterDirection.Input);
                Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PROCESS_ID", pIntProcessID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EMPLOYEE_ID", pIntEmployeeID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                IntRes += Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "Job_SingleFactoryProductionLabourToUpdateGradingColor", CommandType.StoredProcedure);
                
                Ope.ClearParams();
                Ope.AddParams("WAGESBASE", "LAB", DbType.String, ParameterDirection.Input);
                Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PROCESS_ID", pIntProcessID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EMPLOYEE_ID", pIntEmployeeID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("FROMDATE", pStrFromDate, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("TODATE", pStrToDate, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                IntRes += Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "Job_SingleFactoryProductionLabourToUpdateGradingColor", CommandType.StoredProcedure);

                return IntRes;
            }
            catch (Exception Ex)
            {
                return -1;
            }
        }



        public int UpdateLiveGradingColorFlag(string pStrTrnID, string pStrWagesBase)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("TRN_ID", pStrTrnID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("WAGESBASE", pStrWagesBase, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                int IntRes = Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "Job_SingleFactoryProductionLabourToUpdateFlag", CommandType.StoredProcedure);

                return IntRes;
            }
            catch (Exception Ex)
            {
                return -1;
            }
        }

        public DataSet GerGradingComparisionWithLatestGrdByLab(string pStrKapan, string pStrOpe, string pStrFromDate, string pStrToDate, string pIntEmployeeID) //Add : Dhara : 20-01-2021
        {
            try
            {
                DataSet DS = new DataSet();

                Ope.ClearParams();
                Ope.AddParams("FROMDATE", pStrFromDate, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TODATE", pStrToDate, DbType.String, ParameterDirection.Input);
                Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EMPLOYEE_ID", pIntEmployeeID, DbType.String, ParameterDirection.Input);

                Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "RP_GradingComparisonWithLatestGrdByLab", CommandType.StoredProcedure);
                return DS;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }

        public DataTable GerGradingComparisionDetailWithLatestGrdByLab(string pStrKapan, string pStrFromDate, string pStrToDate, Int64 pIntEmployeeID,
            string pStrClickType,
            string pStrRowValue,
            string pStrColValue

            )
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();
                Ope.AddParams("FROMDATE", pStrFromDate, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TODATE", pStrToDate, DbType.String, ParameterDirection.Input);
                Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EMPLOYEE_ID", pIntEmployeeID, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("CLICKTYPE", pStrClickType, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ROWVALUE", pStrRowValue, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COLVALUE", pStrColValue, DbType.String, ParameterDirection.Input);

                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_LiveStockGetData", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {
                return null;
            }
        }


        //End As
        public DataSet GetDataForClarityAssortmentShapeSize(Guid pGuidEmp_ID, string pStrKapan,string pStrFromDate,string pStrToDate)
        {
            try
            {
                DataSet DS = new DataSet();

                Ope.ClearParams();
                Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EMP_ID", pGuidEmp_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("FROMDATE", pStrFromDate, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TODATE", pStrToDate, DbType.String, ParameterDirection.Input);
                Ope.AddParams("YEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

                Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS,"Temp", "RP_ClarityAssortmentShapeSizeWise", CommandType.StoredProcedure);
                return DS;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }

        public DataTable GetDataForClarityAssortmentShapeSizeSummary(Guid pGuidEmp_ID, string pStrKapan, string pStrFromDate, string pStrToDate) // Add Khushbu 02-07-21
        {
            try
            {
                DataTable DTab = new DataTable();

                Ope.ClearParams();
                Ope.AddParams("KAPANNAME", pStrKapan, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EMP_ID", pGuidEmp_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("FROMDATE", pStrFromDate, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TODATE", pStrToDate, DbType.String, ParameterDirection.Input);

                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "RP_ClarityAssortmentShapeSizeWiseSummary", CommandType.StoredProcedure);
                return DTab;
            }
            catch (Exception Ex)
            {

                return null;
            }
        }
              #endregion
    }
}

