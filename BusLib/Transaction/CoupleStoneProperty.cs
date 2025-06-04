using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.Transaction
{
    public class CoupleStoneProperty
    {
        public Int32 BOX_ID { get; set; }
        public string MFG_ID { get; set; }


        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }
}
