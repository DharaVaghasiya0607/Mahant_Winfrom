using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class YearMasterProperty
    {
        public Int32 YEAR_ID { get; set; }

        public string YEARNAME { get; set; }
        public string YEARSHORTNAME { get; set; }
        public string FROMDATE { get; set; }

        public string TODATE { get; set; }
        public bool ISACTIVE { get; set; }
        public bool ISLOCK { get; set; }

        public string REMARK { get; set; }
        
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
