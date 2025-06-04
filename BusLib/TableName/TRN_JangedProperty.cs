using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class TRN_JangedProperty
    {
        public string Janged_ID { get; set; }
        public string JangedDate { get; set; }
        public string FinYear { get; set; }
        public int JangedNo { get; set; }
        public string JangedNoStr { get; set; }
        public string Party_ID { get; set; }
        public string Broker_ID { get; set; }
        public string Through { get; set; }
        public string DetailXML { get; set; }
        public string Currency { get; set; }
     
        public string ReturnJangedNo { get; set; }
        public string ReturnJangedStr { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }

    }
}
