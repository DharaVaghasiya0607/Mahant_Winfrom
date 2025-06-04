using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class DailyRateMasterProperty
    {
        public Guid DAILYRATE_ID { get; set; }

        public Int32 CURRENCY_ID { get; set; }
        public double EXCRATE { get; set; }

        public string APPLICATIONFROM { get; set; }

        //public Int64 ID { get; set; }
        //public string RATEDATE { get; set; }
        //public Int32 CURRENCY_ID { get; set; }
        //public double EXCRATE { get; set; }
        //public Int32 YYYY { get; set; }
        //public Int32 MM { get; set; }
        //public Int32 DD { get; set; }
        //public string REMARK { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
