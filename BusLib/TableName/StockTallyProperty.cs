using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class StockTallyProperty
    {
        public Guid STOCKTALLY_ID { get; set; }
        public Guid STOCK_ID { get; set; }
        public string STOCKNO { get; set; }
        public Int32 SHAPE_ID { get; set; }
        public Int32 SIZE_ID { get; set; }
        public Int32 CLARITY_ID { get; set; }
        public double BALANCECARAT { get; set; }
        public double ACTUALCARAT { get; set; }
        public double LOSSCARAT { get; set; }
        public string STOCKTALLYDATE { get; set; }
        public double PLUSCARAT { get; set; }
        public double SALERATE { get; set; }
        public double OLDSALERATE { get; set; }

        public string REMARK { get; set; }

        public int ISSALERATEUPDATE { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }
}
