using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class PriceHeadDetailProperty
    {
        public Guid GROUP_ID { get; set; }
        public Guid PRICE_ID { get; set; }

        public Int32 HEAD_ID { get; set; }
        public string SHAPENAME { get; set; }
        public string FIELDNAME1 { get; set; }
        public string FIELDNAME2 { get; set; }
        public string FIELDNAME3 { get; set; }
        public string FIELDNAME4 { get; set; }

        public string FIELDTYPE1 { get; set; }
        public string FIELDTYPE2 { get; set; }
        public string FIELDTYPE3 { get; set; }
        public string FIELDTYPE4 { get; set; }

        public decimal FROMCARAT { get; set; }
        public decimal TOCARAT { get; set; }

        public Int32 SHAPE_ID{ get; set; }
        public Int32 FIELD_ID1 { get; set; }
        public Int32 FIELD_ID2 { get; set; }
        public Int32 FIELD_ID3 { get; set; }
        public Int32 FIELD_ID4 { get; set; }

        public decimal CHANGEVALUE { get; set; }

        public string COLORNAME { get; set; }
        public Int32 COLOR_ID { get; set; }

        public string CLARITYNAME { get; set; }
        public Int32 CLARITY_ID { get; set; }
        public string RAPDATE { get; set; }

        public Int32 ISNOTAPPLICABLE { get; set; }
        public Int32 ISNOTCONSIDERINEXPORT { get; set; }
        
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
