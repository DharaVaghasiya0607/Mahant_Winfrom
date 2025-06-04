using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class Trn_SinglePrdProperty
    {
        public Int64 MFGGradingNo { get; set; }
        public string XMLDETSTR { get; set; }

        public string MfgGradingStatus { get; set; }
        public string StockNo { get; set; }
        public string STOCK_ID { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }

}
