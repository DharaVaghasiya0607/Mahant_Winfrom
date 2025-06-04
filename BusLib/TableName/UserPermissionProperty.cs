using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class UserPermissionProperty
    {
        public Int64 USER_ID { get; set; }
        public Int32 FORM_ID { get; set; }
        public Guid PERMISSION_ID { get; set; }

        public bool ISVIEW { get; set; }
        public bool ISINSERT { get; set; }
        public bool ISUPDATE { get; set; }
        public bool ISDELETE { get; set; }
        public string PASSWORD { get; set; }

        public string FORMGROUP { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
