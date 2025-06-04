using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class KapanWiseMumbaiRateProperty
    {
        public Guid KAPANRATE_ID { get; set; }

        public string KAPANNAME { get; set; }

        public string KAPANDATE { get; set; }

        public double GIAKAPANCARAT { get; set; }
        public double GIAKAPANRATE { get; set; }
        public double GIAKAPANAMOUNT { get; set; }

        public double MIXKAPANCARAT { get; set; }
        public double MIXKAPANRATE { get; set; }
        public double MIXKAPANAMOUNT { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
