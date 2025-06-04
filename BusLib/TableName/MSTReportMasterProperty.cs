using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class MSTReportMasterProperty
    {
        public int Report_ID{get; set;}
        public string ReportName{get; set;}
        public string ReportGroupName {get; set;}
        public string FormName{get; set;}
        public int SequenceNo{get; set;}
        public int Active {get; set;}
        public string Remark{get; set;}
        public string ReportType { get; set; }

        public string ReportHeaderName { get; set; }
        public string ProcedureName{get; set;}
        public string RptName{get; set;}
        public string DefaultOrderBy{get; set;}
        public string DefaultGroupBy{get; set;}
        public int IsPivot{get; set;}
        public string FontName{get; set;}
        public double FontSize{get; set;}
        public string PrintFont_Name{get; set;}
        public double PrintFont_Size{get; set;}
        public string PrintPage_Orientation{get; set;}
        public string PrintPage_Size{get; set;}

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }

    }

    public class MSTReportOutputMasterProperty
    {
        public int Report_ID { get; set; }
        public Guid OutPut_ID { get; set; }
        public string ReportType{get; set;}
        public int FieldNo{get; set;}
        public string FieldName{get; set;}
        public string ColumnName{get; set;}
        public string SummaryType{get; set;}
        public string Alignment{get; set;}
        public string Format{get; set;}
        public int ColumnWidth{get; set;}
        public int Visible{get; set;}
        public int IsMerge{get; set;}
        public int IsBold{get; set;}
        public int IsItalic{get; set;}
        public int IsUnderline{get; set;}
        public string MergeOn{get; set;}
        public string DataType{get; set;}
        public int ColSequenceNo{get; set;}
        public int Active{get; set;}
        public string Remark{get; set;}
        public int IsGroup{get; set;}
        public int IsRowArea{get; set;}
        public int IsColumnArea{get; set;}
        public int IsDataArea{get; set;}
        public string OrderBy{get; set;}
        public int IsUnbound{get; set;}
        public string Expression{get; set;}
        public string Bands{get; set;}

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }

        //public string Themes_Name{get; set;}
        //public int Employee_Code{get; set;}
        //public string BackColor{get; set;}
        //public string ForeColor{get; set;}
        //public string GridColor{get; set;}
        //public int Active_Theme{get; set;}
        //public string Filter_Expression{get; set;}
        //public string Grid_Theme{get; set;}

    }

}
