using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class BoxMasterProperty
    {
        public Int32 BOX_ID { get; set; }
        public string BOXCODE { get; set; }
        public string NAME { get; set; }

        public bool ISACTIVE { get; set; }
        public string REMARK { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
