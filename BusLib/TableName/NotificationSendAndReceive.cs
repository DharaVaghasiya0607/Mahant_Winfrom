using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class NotificationSendAndReceive
    {
        public string STONENO { get; set; }
        public string ORDERNO { get; set; }
        public string SALENO { get; set; }

        public Guid Notification_ID { get; set; }
        public Guid MEMO_ID { get; set; }


        public string DISCRIPTION { get; set; }
        public bool ISRECEIVE { get; set; }
        public bool ISCLEAR { get; set; }
        public bool ISSEND { get; set; }


        public int LOACTION_ID { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }
}
