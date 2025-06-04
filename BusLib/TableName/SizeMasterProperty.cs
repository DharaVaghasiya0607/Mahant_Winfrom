using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class SizeMasterProperty
    {
        public Int32 SIZE_ID { get; set; }
        public string SIZENAME { get; set; }
        public double FROMCARAT { get; set; }
        public double TOCARAT { get; set; }
        public bool ISACTIVE { get; set; }
        public string REMARK { get; set; }
        public Int32 SEQUENCENO { get; set; }
        public Int32 DEPARTMENT_ID { get; set; }

        public string SIZEWISEDEPARTMENT_ID { get; set; } //HINAL 12/02/2022

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }
}
