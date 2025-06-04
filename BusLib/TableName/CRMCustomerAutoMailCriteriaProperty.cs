using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class CRMCustomerAutoMailCriteriaProperty
    {
        public Guid AUTOEMAIL_ID { get; set; }

        public Guid CUSTOMER_ID { get; set; }

        public string EMAILTYPE { get; set; }
        public string VALIDDATE { get; set; }
        public string LABFROMDATE { get; set; }
        public string LABTODATE { get; set; }
        public string AVAILBLEFROMDATE { get; set; }
        public string AVAILBLETODATE { get; set; }
        public string UPLOADFROMDATE { get; set; }
        public string UPLOADTODATE { get; set; }
        public string FROMLABRETURNDATE { get; set; }
        public string TOLABRETURNDATE { get; set; }
        public string FROMLABRESULTDATE { get; set; }
        public string LABRESULTTODATE { get; set; }
        public string SALESFROMDATE { get; set; }
        public string SALESTODATE { get; set; }
        public string PRICEREVICESFROMDATE { get; set; }
        public string PRICEREVICESTODATE { get; set; }
        public string DELIVERYFROMDATE { get; set; }
        public string DELIVERYTODATE { get; set; }


        public string SERCHPATTERN { get; set; }

        public bool ISACTIVE { get; set; }

        public string PARTYSTOCKNO { get; set; }
        public string STOCKNO { get; set; }


        public string STOCKTYPE { get; set; }

        //public int SHAPE_ID { get; set; }
        //public int COLOR_ID { get; set; }
        //public int CLARITY_ID { get; set; }
        //public int CUT_ID { get; set; }
        //public int POL_ID { get; set; }
        //public int SYM_ID { get; set; }
        //public int FL { get; set; }

        public string MULTYSHAPE_ID { get; set; }
        public string MULTISHAPENAME { get; set; }
        public string MULTYKAPAN_ID { get; set; }
        public string MULTIKAPANNAME{ get; set; }
        public string MULTYCOLOR_ID { get; set; }
        public string MULTYCOLORNAME { get; set; }
        public string MULTYCLARITY_ID { get; set; }
        public string MULTYCLARITYNAME { get; set; }
        public string MULTYCUT_ID { get; set; }
        public string MULTYCUTNAME { get; set; }
        public string MULTYPOL_ID { get; set; }
        public string MULTYPOLNAME { get; set; }
        public string MULTYSYM_ID { get; set; }
        public string MULTYSYMNAME { get; set; }
        public string MULTYFL_ID { get; set; }
        public string MULTYFLNAME { get; set; }
        public string MULTYFANCYCOLOR_ID { get; set; }
        public string MULTYFANCYCOLORNAME { get; set; }
        public string MULTYLOCATION_ID { get; set; }
        public string MULTYLOCATIONNAME { get; set; }
        public string MULTYMILKY_ID { get; set; }
        public string MULTYMILKYNAME { get; set; }
        public string MULTYCOLORSHADE_ID { get; set; }
        public string MULTYCOLORSHADENAME { get; set; }
        public string MULTYTABLEBLACKINC_ID { get; set; }
        public string MULTYTABLEBLACKINCNAME { get; set; }
        public string MULTYSIDEBLACKINC_ID { get; set; }
        public string MULTYSIDEBLACKINCNAME { get; set; }
        public string MULTYLAB_ID { get; set; }
        public string MULTYLABNAME { get; set; }
        public string MULTYBOX_ID { get; set; }
        public string MULTYBOXNAME { get; set; }
        public string MULTYDAYS_ID { get; set; }

        public int LOCATION_ID { get; set; }
        public int LOCATIONNAME { get; set; }

        public int PCS { get; set; }
        public double CARAT { get; set; }

        public int BALANCEPCS { get; set; }
        public double BALANCECARAT { get; set; }

        public int SIZE_ID { get; set; }
        public int LAB_ID { get; set; }
        public int LABNAME { get; set; }

        public string LABREPORTNO { get; set; }
        public string SERIALNO { get; set; }
        public string MEMONO { get; set; }
        public string WEBSTATUS_ID { get; set; }
        public string WEBSTATUSNAME { get; set; }
        public string BROWN { get; set; }

        public string BOX_ID { get; set; }
        public string BOXNAME { get; set; }

        public double LENGTH { get; set; }
        public double WIDTH { get; set; }
        public double HEIGHT { get; set; }
        public double DEPTHPER { get; set; }

        public int PAGENO { get; set; }
        public int PAGESIZE { get; set; }

        public string MEASUREMENT { get; set; }

        public double CRANGLE { get; set; }
        public double CRHEIGHT { get; set; }
        public double PAVANGLE { get; set; }
        public double PAVHEIGHT { get; set; }

        public double GIRDLEPER { get; set; }
        public string GIRDLEDESC { get; set; }

        public string KEYTOSYMBOL { get; set; }

        public double COSTRAPAPORT { get; set; }
        public double COSTDISCOUNT { get; set; }
        public double COSTPRICEPERCARAT { get; set; }
        public double COSTAMOUNT { get; set; }

        public double SALERAPAPORT { get; set; }
        public double SALEDISCOUNT { get; set; }
        public double SALEPRICEPERCARAT { get; set; }
        public double SALEAMOUNT { get; set; }

        public int PARTY_ID { get; set; }

        public int PROCESS_ID { get; set; }
        public int PREVPROCESS_ID { get; set; }

        public double FROMCARAT { get; set; }
        public double TOCARAT { get; set; }

        public double FROMCARAT1 { get; set; }
        public double TOCARAT1 { get; set; }

        public double FROMCARAT2 { get; set; }
        public double TOCARAT2 { get; set; }

        public double FROMCARAT3 { get; set; }
        public double TOCARAT3 { get; set; }

        public double FROMCARAT4 { get; set; }
        public double TOCARAT4 { get; set; }

        public double FROMCARAT5 { get; set; }
        public double TOCARAT5 { get; set; }

        public double FROMLENGTH { get; set; }
        public double TOLENGTH { get; set; }

        public double FROMWIDTH { get; set; }
        public double TOWIDTH { get; set; }

        public double FROMHEIGHT { get; set; }
        public double TOHEIGHT { get; set; }

        public double FROMTABLEPER { get; set; }
        public double TOTABLEPER { get; set; }

        public double FROMDEPTHPER { get; set; }
        public double TODEPTHPER { get; set; }

        public string DAYS { get; set; }
        public bool ISACTIVEDAYS { get; set; }
        public TimeSpan TIME { get; set; }

        public string ENTRYDATE { get; set; }
        public string EMAIL_ID { get; set; }

        public int CARATSRNO { get; set; }

        public string DAYXML { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }

        public string MFG_ID { get; set; } //Added by Daksha on 27/02/2023

    }
}
