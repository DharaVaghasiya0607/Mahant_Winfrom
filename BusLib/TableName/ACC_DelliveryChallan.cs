using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class DeliveryChallanProperty
    {
        public Int32 DELIVERY_ID { get; set; }
        public string  DATE { get; set; }
        public string CHALLANNO { get; set; }
        public string CARAT { get; set; }

        public string ENTRYBY { get; set; }
        public string ENTRYIP { get; set; }




        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }

    }
}
