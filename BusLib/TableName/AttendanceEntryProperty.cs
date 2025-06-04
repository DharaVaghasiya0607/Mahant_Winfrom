using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class AttendanceEntryProperty
    {
        public Guid ATD_ID { get; set; }

        public string ATDDATE { get; set; }
        public Int32 SRNO { get; set; }
        
        public Int64 EMPLOYEE_ID { get; set; }
        public Int32 DEPARTMENT_ID { get; set; }
        public Int32 DESIGNATION_ID { get; set; }
        public string AP { get; set; }
        public double WDAYS{ get; set; }
        public double WHOURS { get; set; }
        public Int32 OTHH { get; set; }
        public Int32 OTMM { get; set; }

        public string REMARK { get; set; }       
        
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
