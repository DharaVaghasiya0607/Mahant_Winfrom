using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class MST_ReportProperty
    {

        public Int32 REPORT_ID { get; set; }
        public Int32 SRNO { get; set; }
        public string REPORTGROUP { get; set; }
        public string REPORTGROUPNEW { get; set; }

        public string REPORTNAME { get; set; }
        public string FORMNAME { get; set; }
        public Int32 SEQUENCENO { get; set; }
        public string SPNAME { get; set; }
        public string REPORTVIEW { get; set; }
        public string DISPLAYFONTNAME { get; set; }
        public double DISPLAYFONTSIZE { get; set; }
        public string PRINTFONTNAME { get; set; }
        public double PRINTFONTSIZE { get; set; }
        public string PRINTORIENTATION { get; set; }

        public bool ISACTIVE { get; set; }

        public bool ISPRINTFIRMNAME { get; set; }
        public bool ISPRINTFIRMADDRESS { get; set; }
        public bool ISPRINTFILTERCRITERIA { get; set; }
        public bool ISPRINTHEADINGONEACHPAGE { get; set; }
        public bool ISPRINTDATETIME { get; set; }

        public string REMARK { get; set; }

        public string XMLDATA { get; set; }
        public string XMLDATAGROUP { get; set; }

        public string FROMCOLLECTIONDATE { get; set; }
        public string TOCOLLECTIONDATE { get; set; }
        public string FROMCHEQUEDATE { get; set; }
        public string TOCHEQUEDATE { get; set; }

        public string REPORTTYPE { get; set; }
        public string COMPANY_ID  { get; set; }
        public string LOCATION_ID { get; set; }

        public string STOCKTYPE { get; set; }
        public string STOCKNO { get; set; }
        
        public string FROMPROCESS_ID  { get; set; }
        public string STOCKPROCESS_ID { get; set; }

        public string SHAPE_ID { get; set; }
        public string COLOR_ID { get; set; }
        public string CLARITY_ID { get; set; }
        public string CUT_ID { get; set; }
        public string POL_ID { get; set; }
        public string SYM_ID { get; set; }
        public string FL_ID { get; set; }

        public Double FROMCARAT { get; set; }
        public Double TOCARAT { get; set; }

        public string FROMDATE { get; set; }
        public string TODATE { get; set; }

        public string FROMSALEDATE { get; set; }
        public string TOSALEDATE { get; set; }

        public string GROUPBY { get; set; }

        public string RETURNVALUE { get; set; }
        public string RETURNMESSAGETYPE { get; set; }
        public string RETURNMESSAGEDESC { get; set; }


    }

}
