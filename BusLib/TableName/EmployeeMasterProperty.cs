using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class EmployeeMasterProperty
    {
        public Int64 EMPLOYEE_ID { get; set; }
        public Int64 DEPARTMENT_ID { get; set; }
        public Int64 DESIGNATION_ID { get; set; }

        public string EMPLOYEENAME { get; set; }

        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }

        public int IS_DISPLAYVALUE { get; set; }
    }
}
