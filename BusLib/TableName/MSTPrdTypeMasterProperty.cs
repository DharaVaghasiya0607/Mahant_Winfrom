﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class MSTPrdTypeMasterProperty
    {
        public Int32 PRDTYPE_ID { get; set; }
        public string PRDTYPENAME  { get; set; }
        public string PRDTYPECODE { get; set; }
       
        public int SEQUENCENO { get; set; }
        public bool ISACTIVE	 { get; set; }
        public string REMARK { get; set; }
        public string REQPRDTYPE_ID { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
