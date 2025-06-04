using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class PriceHeadMasterProperty
    {
        public Int64 HEAD_ID { get; set; }

        public string HEADCODE { get; set; }
        public string HEADNAME { get; set; }
        public string DETAILCOLUMNS { get; set; }

        public Int32 SEQUENCENO{ get; set; }
        
        public bool ISACTIVE { get; set; }
        
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
