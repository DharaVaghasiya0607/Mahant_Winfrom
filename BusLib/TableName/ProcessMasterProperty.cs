using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class ProcessMasterProperty
    {
        public Int32 PROCESS_ID { get; set; }

        public string PROCESSNAME { get; set; }
        public string PROCESSCODE { get; set; }
        public string JANGEDPREFIX { get; set; }

        public string PRINTHEADER { get; set; }
        public string BILLDATEPREFIX { get; set; }
        public string BILLNOPREFIX { get; set; }

        public int SEQUENCENO { get; set; }

        public string PREVPROCESS_ID { get; set; }

        
        public bool ISACTIVE { get; set; }
        public bool ISACTIVESTATUS { get; set; }

        public string WEBSTATUS { get; set; }
        public string STOCKSTATUS { get; set; }

        public string REMARK { get; set; }
        
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
