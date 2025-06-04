using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class ShapeMasterProperty
    {
        public Int32 ID { get; set; }
        public string CODE { get; set; }
        public string SHAPE { get; set; }
        public int SEQUENCENO { get; set; }
        public bool ISACTIVE { get; set; }
        public string REMARK { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }

        public string SIZE { get; set; }
        public string CLARITY { get; set; }
        public string LOT { get; set; }
        public string COLOR { get; set; }
        public string BOXNAME { get; set; }

        public Int32 LOT_ID { get; set; }

        public Int32 CLARITY_ID { get; set; }

        public Int32 COLOR_ID { get; set; }

        public Int32 SIZE_ID { get; set; }

        public Int32 CUTNO { get; set; }

        public Int32 SHAPE_ID { get; set; }

        public string XMLDETSTR { get; set; }

        public int BOX_ID { get; set; }
        public Int32 COLORSHADE_ID { get; set; }
    }
}
