using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class TrnKapanProperty
    {
        public Guid KAPAN_ID { get; set; }
        public string KAPANNAME { get; set; }
        public string KAPANCODE { get; set; }
        public bool ISACTIVE { get; set; }
        public string REMARK { get; set; }

        public string PARCELKAPANNAME { get; set; }

        public double EXCRATE { get; set; }
        public double SURATAVARAGE { get; set; }
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }
}
