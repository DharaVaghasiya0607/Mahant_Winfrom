using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class ParcelBoxMasterProperty
    {
        public Guid STOCK_ID { get; set; }
        public string STOCKNO { get; set; }

        public Int32 DEPARTMENT_ID { get; set; }
        public Int32 SHAPE_ID { get; set; }
        public Int32 MIXSIZE_ID { get; set; }
        public string MIXSIZENAME { get; set; }
        public Int32 MIXCLARITY_ID { get; set; }
        public string MIXCLARITYNAME { get; set; }

        public double OPENINGCARAT { get; set; }
        public double OPENINGPRICEPERCARAT { get; set; }
        public double OPENINGAMOUNT { get; set; }
        public double MFGPRICEPERCARAT { get; set; }
        public double COSTPRICEPERCARAT { get; set; }
        public double SALEPRICEPERCARAT { get; set; }
        public double EXPPRICEPERCARAT { get; set; }
        public string PARCELGROUPNO { get; set; }
        public int PARCELSEQNO { get; set; }

        public Int32 ISSALEPRICEPERCARAT { get; set; }

        public string RETURNVALUE { get; set; }
        public string RETURNMESSAGETYPE { get; set; }
        public string RETURNMESSAGEDESC { get; set; }
        public string BOXNAME { get; set; }
        public int BOX_ID { get; set; }
        public string REMARK { get; set; }

        public bool ISACTIVE { get; set; }
        public Guid PARTY_ID { get; set; }
        public Guid BROKER_ID { get; set; }
        public double BROKPER { get; set; }
        public double DISCOUNT { get; set; }


        public double CARAT { get; set; }
        public double RATE { get; set; }
        public double AMOUNT { get; set; }
        public double BRKAMOUNT { get; set; }

        public string ENTRYTYPE { get; set; }
    }

}
