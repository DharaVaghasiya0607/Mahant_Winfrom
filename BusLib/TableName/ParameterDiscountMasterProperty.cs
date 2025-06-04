using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class ParameterDiscountMasterProperty
    {
        public string PARAMETER_ID { get; set; }
	    public string PARAMETERNAME { get; set; }
	    public Int32 PARAMETERRANK { get; set; }
	    public bool MIXRATE { get; set; }
        public string GROUPTYPE { get; set; }	

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
