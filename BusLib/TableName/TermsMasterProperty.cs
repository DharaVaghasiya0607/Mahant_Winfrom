using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class TermsMasterProperty
    {
        public Int32 TERMS_ID { get; set; }

        public string TERMSNAME { get; set; }

        public Int32 TERMSDAYS{ get; set; }
        public double TERMSPER { get; set; }

        public bool ISACTIVE { get; set; }
        
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
