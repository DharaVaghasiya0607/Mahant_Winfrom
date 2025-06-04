using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class BannerMasterProperty
    {
        public Int32 BANNER_ID { get; set; }

        public string FILEPATH { get; set; }

        public bool ISACTIVE { get; set; }

        public string REMARK { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }
}
