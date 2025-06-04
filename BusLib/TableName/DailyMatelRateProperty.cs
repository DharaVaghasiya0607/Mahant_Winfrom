using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class DailyMatelRateProperty
    {
        public Guid ID { get; set; }

        public Int32 METAL_ID { get; set; }
        public string RATEDATE { get; set; }
        public double PERGRAMRATE { get; set; }
        public string REMARK { get; set; }
        public bool ISACTIVE { get; set; }


        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
