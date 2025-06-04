using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class BombayShapeWiseAssortMergeProperty
    {
        public Int32 MERGESUMMARY_ID { get; set; }

        public Int32 FROMSHAPE_ID { get; set; }
        public Int32 TOSHAPE_ID { get; set; }

        public Int32 FROMDEPARTMENT_ID { get; set; }
        public Int32 TODEPARTMENT_ID { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }
}
