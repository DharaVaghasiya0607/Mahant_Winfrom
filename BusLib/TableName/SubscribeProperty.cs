using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
   public class SubscribeProperty
    {
       public Int32 EMAILID { get; set; }
       public Boolean ISACTIVE { get; set; }
      
       public string ReturnValue { get; set; }
       public string ReturnMessageType { get; set; }
       public string ReturnMessageDesc { get; set; }

    }
}
