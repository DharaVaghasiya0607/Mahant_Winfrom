using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class StockPlusMinusProperty
    {       
        public Guid STOCK_ID { get; set; }
        public decimal OLDBALCARAT { get; set; }
        public decimal PLUSMINUSCARAT { get; set; }
        public string REMARK { get; set; }
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }
}
