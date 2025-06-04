using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class ExcelSettingMasterProperty
    {
        public Int64 EXCELSETTING_ID { get; set; }

        public Guid LEDGER_ID { get; set; }

        public string EXCELSETTINGNAME { get; set; }
        public string EXCELSETTINGREFNAME{ get; set; }

        public Int32 SEQUENCENO{ get; set; }
        public string REMARK { get; set; }
        
        public bool ISACTIVE { get; set; }
        public bool ISCOMPULSORYINSINGLE { get; set; }
        public bool ISCOMPULSORYINPARCEL { get; set; }
        
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
