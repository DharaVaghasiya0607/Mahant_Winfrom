using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class PriceDateMasterProperty
    {
        public Int32 PRICE_ID { get; set; }

        public string PRICETYPE { get; set; }
        public string PRICEDATE { get; set; }

        public string REMARK { get; set; }
        public bool ISACTIVE { get; set; }
        
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
