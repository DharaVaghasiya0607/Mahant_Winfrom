using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using AxonDataLib;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using BusLib.TableName;

namespace BusLib.FunctionClasses.Master
{
    public class BOMST_ReportMaster
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        #region Property Settings

        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();

        private DataSet _DS = new DataSet();

        public DataSet DS
        {
            get { return _DS; }
            set { _DS = value; }
        }

        public string TableName
        {
            get { return BusLib.TPV.BOTableName.MST_Report; }
        }

        public string TableNameDetail
        {
            get { return BusLib.TPV.BOTableName.MST_ReportOutput; }
        }

        //public string TableNameSettings
        //{
        //    get { return BLL.TPV.Table.Report_Settings; }
        //}

        #endregion

        #region Other Function



        public MSTReportMasterProperty Save(MSTReportMasterProperty pClsProperty)
        {

            Ope.ClearParams();

            Ope.AddParams("REPORT_ID", pClsProperty.Report_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("REPORTNAME", pClsProperty.ReportName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("REPORTGROUPNAME", pClsProperty.ReportGroupName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FORMNAME", pClsProperty.FormName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SEQUENCENO", pClsProperty.SequenceNo, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("ISACTIVE", pClsProperty.Active, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("REMARK", pClsProperty.Remark, DbType.String, ParameterDirection.Input);

            Ope.AddParams("REPORTTYPE", pClsProperty.ReportType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PROCEDURENAME", pClsProperty.ProcedureName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("REPORTHEADERNAME", pClsProperty.ReportHeaderName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("RPTNAME", pClsProperty.RptName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("DEFAULTORDERBY", pClsProperty.DefaultOrderBy, DbType.String, ParameterDirection.Input);
            Ope.AddParams("DEFAULTGROUPBY", pClsProperty.DefaultGroupBy, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ISPIVOT", pClsProperty.IsPivot, DbType.Int32, ParameterDirection.Input);

            Ope.AddParams("FONTSIZE", pClsProperty.FontSize, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("FONTNAME", pClsProperty.FontName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PRINTFONTNAME", pClsProperty.PrintFont_Name, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PRINTFONTSIZE", pClsProperty.PrintFont_Size, DbType.Decimal, ParameterDirection.Input);
            Ope.AddParams("PRINTPAGEORIENTATION", pClsProperty.PrintPage_Orientation, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PRINTPAGESIZE", pClsProperty.PrintPage_Size, DbType.String, ParameterDirection.Input);

            //Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
            //Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

            Ope.AddParams("ENTRYBY", 0, DbType.Int64, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", "" , DbType.String, ParameterDirection.Input);

            Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

            ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_ReportMasterSave", CommandType.StoredProcedure);

            if (AL.Count != 0)
            {
                pClsProperty.ReturnValue = Val.ToString(AL[0]);
                pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
            }
            return pClsProperty;
        }

        public MSTReportOutputMasterProperty SaveReportSettings(MSTReportOutputMasterProperty pClsProperty)
        {
            Ope.ClearParams();
            Ope.AddParams("REPORT_ID", pClsProperty.Report_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("OUTPUT_ID", pClsProperty.OutPut_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("REPORTTYPE", pClsProperty.ReportType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FIELDNO", pClsProperty.FieldNo, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("FIELDNAME", pClsProperty.FieldName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLUMNNAME", pClsProperty.ColumnName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLSEQUENCENO", pClsProperty.ColSequenceNo, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("SUMMARYTYPE", pClsProperty.SummaryType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("VISIBLE", pClsProperty.Visible, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("ISMERGE", pClsProperty.IsMerge, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("MERGEON", pClsProperty.MergeOn, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ISGROUP", pClsProperty.IsGroup, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("ISROWAREA", pClsProperty.IsRowArea, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("ISCOLUMNAREA", pClsProperty.IsColumnArea, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("ISDATAAREA", pClsProperty.IsDataArea, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("ORDERBY", pClsProperty.OrderBy, DbType.String, ParameterDirection.Input);
            Ope.AddParams("REMARK", pClsProperty.Remark, DbType.String, ParameterDirection.Input);
            Ope.AddParams("DATATYPE", pClsProperty.DataType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ISUNBOUND", pClsProperty.IsUnbound, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("EXPRESSION", pClsProperty.Expression, DbType.String, ParameterDirection.Input);
            Ope.AddParams("BANDS", pClsProperty.Bands, DbType.String, ParameterDirection.Input);
            Ope.AddParams("SUMMARYFORMAT", pClsProperty.Format, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ALIGNMENT", pClsProperty.Alignment, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COLUMNWIDTH", pClsProperty.ColumnWidth, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("ISBOLD", pClsProperty.IsBold, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("ISITALIC", pClsProperty.IsItalic, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("ISUNDERLINE", pClsProperty.IsUnderline, DbType.Int32, ParameterDirection.Input);

            Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

            //return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, SProc.MST_ReportMasterOutputSave, CommandType.StoredProcedure);

            ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_ReportMasterOutputSave", CommandType.StoredProcedure);
            if (AL.Count != 0)
            {
                pClsProperty.ReturnValue = Val.ToString(AL[0]);
                pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
            }
            return pClsProperty;
        }



        public int DeleteSettings(MSTReportOutputMasterProperty pClsProperty)
        {
            Ope.ClearParams();

            Ope.AddParams("REPORT_ID", pClsProperty.Report_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("REPORTTYPE", pClsProperty.ReportType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("FIELDNO", pClsProperty.FieldNo, DbType.String, ParameterDirection.Input);

            return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "MST_ReportOutputDelete", CommandType.StoredProcedure);
        }
        public int Delete(int pIntReportCode)
        {
            Ope.ClearParams();

            Ope.AddParams("REPORT_ID", pIntReportCode, DbType.Int32, ParameterDirection.Input);
            return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "MST_ReportMasterDelete", CommandType.StoredProcedure);
        }


        public MSTReportMasterProperty GetReportMasterProperty(int pIntReportCode, string pStrReportGroup,string pStrReportType = "")
        {

            Ope.ClearParams();
            Ope.AddParams("REPORT_ID", pIntReportCode, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("REPORTTYPE", pStrReportType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("REPORTGROUP", pStrReportGroup, DbType.String, ParameterDirection.Input);

            //Request.CommandText = BLL.TPV.SProc.New_Report_Master_GetData;
            //Request.CommandType = CommandType.StoredProcedure;
            DataRow DRow = Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, "MST_ReportGetData", CommandType.StoredProcedure);

            MSTReportMasterProperty Report_MasterProperty = new MSTReportMasterProperty();
            Report_MasterProperty.Report_ID = Val.ToInt(DRow["REPORT_ID"]);
            Report_MasterProperty.ReportName = Val.ToString(DRow["REPORTNAME"]);
            Report_MasterProperty.ReportGroupName = Val.ToString(DRow["REPORTGROUPNAME"]);
            Report_MasterProperty.FormName = Val.ToString(DRow["FORMNAME"]);
            Report_MasterProperty.SequenceNo = Val.ToInt(DRow["SEQUENCENO"]);
            Report_MasterProperty.Active = Val.ToInt(DRow["ISACTIVE"]);
            Report_MasterProperty.IsPivot= Val.ToInt(DRow["ISPIVOT"]);
            Report_MasterProperty.Remark = Val.ToString(DRow["REMARK"]);
            Report_MasterProperty.ReportType = Val.ToString(DRow["REPORTTYPE"]);
            Report_MasterProperty.ProcedureName = Val.ToString(DRow["PROCEDURENAME"]);
            Report_MasterProperty.ReportHeaderName = Val.ToString(DRow["REPORTHEADERNAME"]);
            Report_MasterProperty.FontName = Val.ToString(DRow["FONTNAME"]);
            Report_MasterProperty.FontSize = Val.Val(DRow["FONTSIZE"]);
            Report_MasterProperty.PrintFont_Name = Val.ToString(DRow["PRINTFONTNAME"]);
            Report_MasterProperty.PrintFont_Size = Val.Val(DRow["PRINTFONTSIZE"]);
            DRow = null;
            return Report_MasterProperty;

        }

        public int DeleteDetail(int pIntReport_ID)
        {

            Ope.ClearParams();
            string StrQuey = "DELETE FROM MST_REPORTOUTPUT WITH(ROWLOCK) WHERE REPORT_ID = " + pIntReport_ID;
            return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, StrQuey , CommandType.Text);
        }


        public DataTable GetDataForSearchSettings(int pIntReportCode = 0, string pStrReportType = null)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("REPORT_ID", pIntReportCode, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("REPORTTYPE", pStrReportType, DbType.String, ParameterDirection.Input);
            //Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "MST_ReportOutputGetData", CommandType.StoredProcedure);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, TableNameDetail, "MST_ReportOutputGetData", CommandType.StoredProcedure);
            return DS.Tables[TableNameDetail];
        }

        public DataTable GetData(int pIntReportID, string pStrReportType, string pStrReportGroup)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("REPORT_ID", pIntReportID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("REPORTTYPE", pStrReportType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("REPORTGROUP", pStrReportGroup, DbType.String, ParameterDirection.Input);
            //Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "MST_ReportOutputGetData", CommandType.StoredProcedure);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab,"MST_ReportGetData", CommandType.StoredProcedure);
            return DTab;
        }


        #endregion
    }
}
