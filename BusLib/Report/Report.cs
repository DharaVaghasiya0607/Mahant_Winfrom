using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BusLib.TableName;
using AxonDataLib;
using Config = BusLib.Configuration.BOConfiguration;
namespace BusLib.Report
{
    public class Report
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        //public string TableName
        //{
        //    get { return BusLib.TPV.Table.Temp; }
        //}

        private DataSet _DS = new DataSet();

        public DataSet DS
        {
            get { return _DS; }
            set { _DS = value; }
        }

        private DataTable _DTab = new DataTable();

        public DataTable DTab
        {
            get { return _DTab; }
            set { _DTab = value; }
        }


        #region Other Function

        public DataTable GetPacketLivStockData(TrnReportProperty pClsProperty, string pStrSPName)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            
            Ope.AddParams("KAPANNAME", pClsProperty.KAPANNAME, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMDEPARTMENT_ID", pClsProperty.FROM_DEPARTMENT_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TODEPARTMENT_ID", pClsProperty.TO_DEPARTMENT_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMPROCESS_ID", pClsProperty.FROM_PROCESS_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TOPROCESS_ID", pClsProperty.TO_PROCESS_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMEMPLOYEE_ID", pClsProperty.FROM_EMPLOYEE_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TOEMPLOYEE_ID", pClsProperty.TO_EMPLOYEE_ID, DbType.String, ParameterDirection.Input);

            Ope.AddParams("MAINMARKER_ID", pClsProperty.MAINMARKER_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PACKETNO", pClsProperty.PACKETNO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FROMPACKETNO", pClsProperty.FROM_PACKET_NO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("TOPACKETNO", pClsProperty.TO_PACKET_NO, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PACKETTAG", pClsProperty.PACKETTAG, DbType.String, ParameterDirection.Input);

            Ope.AddParams("CONFFROMDATE", pClsProperty.CONFFROM_DATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("CONFTODATE", pClsProperty.CONFTO_DATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TRANSFROMDATE", pClsProperty.STOCKFROM_DATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("TRANSTODATE", pClsProperty.STOCKTO_DATE, DbType.Date, ParameterDirection.Input);
            Ope.AddParams("GROUPBY", pClsProperty.GROUP_BY, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, pStrSPName, CommandType.StoredProcedure);
            return DTab;
        }
       


        #endregion
    }
}
