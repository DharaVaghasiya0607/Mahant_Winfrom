using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class FancyColorMasterProperty
    {
        public Int32 FANCYCOLOR_ID { get; set; }
        public Int32 FANCYCODE { get; set; }
        public string FANCYCOLORNAME { get; set; }
        public string SHORTNAME { get; set; }
        public string FANCYCOLOR { get; set; }
        public string FANCYOVERTONE { get; set; }
        public string FANCYINTENSITY { get; set; }

        public bool ISACTIVE { get; set; }
        public string REMARK { get; set; }
        
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }
}
