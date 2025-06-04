using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class AccountTypeMasterProperty
    {
        public Int32 ACCTTYPE_ID { get; set; }
        public string ACCTHEAD { get; set; }
        public string ACCTTYPENAME { get; set; }
        public Int32 ACCTTYPEUPPER_ID { get; set; }
        public bool ISACTIVE { get; set; }
        public Int32 SEQUENCENO { get; set; }
        public string REMARK { get; set; }
        public string GroupHerarchi { get; set; }
        public int Root_id { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }

    }
}
