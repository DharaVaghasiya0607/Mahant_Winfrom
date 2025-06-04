using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class MemoDetailEntryProperty
    {

        public Guid MEMODETAIL_ID { get; set; }
        public Guid MEMO_ID { get; set; }

        public Guid STOCK_ID { get; set; }

        public double CARAT { get; set; }

        public double WEBSITERAPAPORT { get; set; }
        public double WEBSITEDISCOUNT { get; set; }
        public double WEBSITEPRICEPERCARAT { get; set; }
        public double WEBSITEAMOUNT { get; set; }
        public double MEMORAPAPORT { get; set; }
        public double MEMODISCOUNT { get; set; }
        public double MEMOPRICEPERCARAT { get; set; }
        public double MEMOAMOUNT { get; set; }
        public double EXCRATE { get; set; }
        public double FMEMOPRICEPERCARAT { get; set; }
        public double FMEMOAMOUNT { get; set; }
        public string REMARK { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
