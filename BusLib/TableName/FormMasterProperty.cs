using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class FormMasterProperty
    {
        public Int32 FORM_ID { get; set; }

        public string FORMNAME { get; set; }
        public string FORMDESCNEW { get; set; }
        public string FORMGROUP { get; set; }
        public string FORMDESC { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
