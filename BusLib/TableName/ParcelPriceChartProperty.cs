using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class ParcelPriceChartProperty
    {
        public Guid ID { get; set; }
        public Int32 SHAPE_ID { get; set; }
        public Int32 MIXCLARITY_ID { get; set; }
        public Int32 MIXSIZE_ID { get; set; }
        public string MIXSIZENAME { get; set; }

        public Int32 DEPARTMENT_ID { get; set; }
        public Int32 PRICE_ID { get; set; }
        public string PRICEDATE { get; set; }
        public double RATE { get; set; }
        
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }
}
