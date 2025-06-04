using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class AccDailyRateMasterProperty
    {
        public Int64 ACCDAILYRATE_ID { get; set; }

        public string FROMDATE { get; set; }
        public string TODATE { get; set; }

        public Int32 CURRENCY_ID { get; set; }
        public double EXCRATE { get; set; }

        public bool ISACTIVE { get; set; }
        
        public string RETURNVALUE       { get; set; }
        public string RETURNMESSAGETYPE { get; set; }
        public string RETURNMESSAGEDESC { get; set; }

    }

}
