using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class StyleMasterProperty
    {
        //public Int64 LEDGER_ID { get; set; }

        public Guid STYLE_ID { get; set; }
        public string STYLENO { get; set; }
        public Int32 LOCATION_ID { get; set; }
        public Int32 PRODUCTCATEGORY_ID { get; set; }
        public Int32 PRODUCTSUBCATEGORY_ID { get; set; }
        public double GROSSWT { get; set; }
        //public double NETWT { get; set; }

        public double NETWT { get; set; }
        public double METALWT { get; set; }
        public Int32 DIAMONDPCS { get; set; }
        public double DIAMONDWT { get; set; }
        public Int32 COLORPCS { get; set; }
        public double COLORWT { get; set; }
        public Int32 STYLENONUMERIC { get; set; }
        public string SIMILLARSTYLE { get; set; }
        public string SHORTDESCRIPTION { get; set; }
        public string LONGDESCRIPTION { get; set; }
        public string STATUS { get; set; }
        public string REMARK { get; set; }

        public Int32 MATERIAL_ID { get; set; }
        public Int32 COLLECTION_ID { get; set; }
        public Int32 PROPERTY_ID { get; set; }
        public string PROPERTYVALUE { get; set; }
        //public string MEDIA { get; set; }

        public string MATERIAL { get; set; }
        public string COLLECTION { get; set; }
        public string MEDIA { get; set; }
        public string PROPERTY { get; set; }
        public string RANKING { get; set; }
        public string REVIEW { get; set; }





        //public Guid STOCK_ID { get; set; }

        //public string STOCKNO { get; set; }
        ////public int PRODUCTCATEGORY_ID { get; set; }
        ////public int PRODUCTSUBCATEGORY_ID { get; set; }

        //public string VENDERSTYLENO { get; set; }
        //public Guid VENDER_ID { get; set; }
        ////public int LOCATION_ID { get; set; }

        //public int CURRENCY_ID { get; set; }
        //public int QTY { get; set; }

        //public double COSTPRICE { get; set; }
        //public double SALEPRICE { get; set; }
        //public int MINIMUMORDERQTY { get; set; }
        //public string WEBSTATUS { get; set; }
        //public int JEWELLERYSTATE_ID { get; set; }
        //public int JEWELLERYSTYLE_ID { get; set; }

        //public int JEWELLERYBRAND_ID { get; set; }
        //public int SETTINGTYPE_ID { get; set; }

        //public int LOCKTYPE_ID { get; set; }
        //public string JEWELLERYCOLLECTION_ID { get; set; }
        //public string JEWELLERYCOLLECTION { get; set; }
        //public string SIZE { get; set; }
        //public string MFGDATE { get; set; }
        //public string AVAILABLEDATE { get; set; }

        //public string JEWELLERYTITLE { get; set; }
        //public string JEWELLERYDESCRIPTION { get; set; }
        ////public string REMARK { get; set; }
        //public bool ISACTIVE { get; set; }

        //public string METAL { get; set; }
        //public string DIAMOND { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
