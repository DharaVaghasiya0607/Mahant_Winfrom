using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class CollectionProperty
    {

        public Int32 COLLECTION_ID { get; set; }
        public string COLLECTIONCODE { get; set; }
        public string COLLECTIONNAME { get; set; }
        public string REMARK               { get; set; }
        public Int32 STATUS { get; set; }
        public Boolean ISACTIVE { get; set; }
        
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
