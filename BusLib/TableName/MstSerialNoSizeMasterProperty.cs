using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class MstSerialNoSizeMasterProperty
    {
        public Int32 ID { get; set; }
        public string SIZENAME { get; set; }
        public double FROMSIZE { get; set; }
        public double TOSIZE { get; set; }
        public bool ISACTIVE { get; set; }
        public string REMARK { get; set; }
        public Int32 SEQUENCENO { get; set; }
        public Int32 DEPARTMENT_ID { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }
}
