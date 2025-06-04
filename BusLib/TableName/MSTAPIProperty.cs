using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class MSTAPIProperty
    {
        public Int32 API_ID { get; set; }
        public string APICODE { get; set; }
        public string APINAME { get; set; }
        public bool ACTIVE { get; set; }
        public string URL { get; set; }
        public string HOSTNAME { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public int PORT { get; set; }
        public string CERTICATE { get; set; }

    }

}
