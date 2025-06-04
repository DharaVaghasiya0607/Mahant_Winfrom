using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class ApiSettingProperty
    {
        public Int32 API_ID { get; set; }
        public string APICODE { get; set; }
        public string APINAME { get; set; }

        public string URL { get; set; }
        public string HOSTNAME { get; set; }
        public Int32 PORT { get; set; }

        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }

        public bool ACTIVE { get; set; }

        public string APITYPE { get; set; }

        public string EXCEL { get; set; }
        public string DIAMONDTYPE { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }
}
