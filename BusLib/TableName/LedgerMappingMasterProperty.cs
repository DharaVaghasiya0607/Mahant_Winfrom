using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class LedgerMappingMasterProperty
    {
        public Int32 LedgerMapping_id { get; set; }

        public string LedgerMappingType { get; set; }
        public Guid PLedger_id { get; set; }
        public Guid SLedger_id { get; set; }
      
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }

        //Add shiv 13-07-2022
        public double GSTPER { get; set; }

    }

}
