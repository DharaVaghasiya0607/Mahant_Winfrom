using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
   public class ParcelMixToMixConvertProperty
    {
        public Guid STOCK_ID { get; set; }
        public String STOCKNO { get; set; }
        public Int32 SHAPE_ID { get; set; }
        public Int32 MIXCLARITY_ID { get; set; }
        public Int32 MIXSIZE_ID { get; set; }
        public decimal CARAT { get; set; }
        public decimal ACTCARAT { get; set; }
        public decimal PRICE { get; set; }
        public decimal AMOUNT { get; set; }
        public Guid PARTY_ID { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }

    }
}
