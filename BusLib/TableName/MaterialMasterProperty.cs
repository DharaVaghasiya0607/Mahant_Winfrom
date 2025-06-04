using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class MaterialMasterProperty
    {
        public Int32 MATERIAL_ID { get; set; }
        public Int32 MATERIALTYPE_ID { get; set; }
        public string MATERIALCODE { get; set; }
        public string MATERIALNAME { get; set; }        
        public Int32 PARENTMATERIAL_ID { get; set; }
        public Int32 STATUS { get; set; }
        public string REMARK { get; set; } 
        public bool ISACTIVE { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }

    }

}
