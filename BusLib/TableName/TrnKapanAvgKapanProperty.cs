using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class TrnKapanAvgKapanProperty
    {
        public int DETAIL_ID { get; set; }

        public Guid KAPAN_ID { get; set; }
        
        public string REMARK { get; set; }

        public double EXCRATE { get; set; }
        public double SURATAVARAGE { get; set; }
        
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }
}
