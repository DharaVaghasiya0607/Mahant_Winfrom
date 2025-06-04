using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class ClvMixRapSizeMappingProperty
    {
        public Int32 ID { get; set; }
        public string S_CODE { get; set; }
        public double RAPFROMCARAT { get; set; }
        public double RAPTOCARAT { get; set; }
        public double MAPFROMCARAT { get; set; }
        public double MAPTOCARAT { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }

    }
}
