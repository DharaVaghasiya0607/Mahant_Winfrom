using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class MixClarityMasterProperty
    {
        public Int32 MIXCLARITYMAPP_ID { get; set; }
        public Int32 MIXCLARITY_ID { get; set; }
        public string MIXCLARITY_NAME { get; set; }
        public double FROMCARAT { get; set; }
        public double TOCARAT { get; set; }
        public Int32 COLOR_ID { get; set; }
        public Int32 CLARITY_ID { get; set; }

        public bool ISACTIVE { get; set; }
        public string REMARK { get; set; }
        public Int32 SEQUENCENO { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }
}
