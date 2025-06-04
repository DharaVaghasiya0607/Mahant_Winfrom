using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
   
    public   class DeptWiseMixClaPermissionProperty
    {
        public Int32 PERMISSION_ID { get; set; }
        public Int32 DEPARTMENT_ID { get; set; }

        public Guid USER_ID { get; set; }
        public Int32 MIXSIZE_ID { get; set; }
        public string MIXCLARITY_ID { get; set; }

        public string REMARK { get; set; }
        public bool ISACTIVE { get; set; }

        public string RETURNVALUE { get; set; }
        public string RETURNMESSAGETYPE { get; set; }
        public string RETURNMESSAGEDESC { get; set; }






    }
}
