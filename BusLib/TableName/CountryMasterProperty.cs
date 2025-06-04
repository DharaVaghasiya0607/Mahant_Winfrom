using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class CountryMasterProperty
    {
        public Int32 COUNTRY_ID { get; set; }

        public string COUNTRYCODE { get; set; }
        public string COUNTRYNAME { get; set; }
        
        public bool ISACTIVE { get; set; }
        public string REMARK { get; set; }
        
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
