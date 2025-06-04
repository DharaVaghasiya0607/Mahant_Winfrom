using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class SingleStockUpdateProperty
    {
        public Guid STOCK_ID { get; set; }
        public Guid PARCELSTOCK_ID { get; set; }

        public string STOCKNO { get; set; }
        public string PARCELSTOCKNO { get; set; }
        public string KAPANNAME { get; set; }
        public int PACKETNO { get; set; }
        public string TAG { get; set; }

        public Int32 SHAPE_ID { get; set; }
        public Int32 COLOR_ID { get; set; }

        public double PARCELCARAT { get; set; }
        public double CARAT { get; set; }
        public Int32 CLARITY_ID { get; set; }
        public Int32 CUT_ID { get; set; }
        public Int32 POL_ID { get; set; }
        public Int32 SYM_ID { get; set; }
        public Int32 FL_ID { get; set; }
        public string FLCOLOR { get; set; }
        public Int32 COLORSHADE_ID { get; set; }
        public Int32 LOCATION_ID { get; set; }
        public Int32 SIZE_ID { get; set; }
        public Int32 LAB_ID { get; set; }
        public string LABREPORTNO { get; set; }
        public Int32 BOX_ID { get; set; }

        public Int32 MFGCOLOR_ID { get; set; }
        public Int32 MFGCLARITY_ID { get; set; }
        public Int32 MFGCUT_ID { get; set; }
        public Int32 MFGPOL_ID { get; set; }
        public Int32 MFGSYM_ID { get; set; }
        public Int32 MFGFL_ID { get; set; }

        public double DIAMIN { get; set; }
        public double DIAMAX { get; set; }
        public double LENGTH { get; set; }
        public double WIDTH { get; set; }
        public double HEIGHT { get; set; }
        public double RATIO { get; set; }
        public double DIAMETER { get; set; }

        public double CRANGLE { get; set; }
        public double CRHEIGHT { get; set; }

        public double PAVANGLE { get; set; }
        public double PAVHEIGHT { get; set; }

        public Int32 PCS { get; set; }
        
        public Int32 BALANCEPCS { get; set; }
        public double BALANCECARAT { get; set; }
        
        public double TABLEPER { get; set; }
        public double DEPTHPER { get; set; }
        public double GIRDLEPER { get; set; }

        //public Int32 TABLEINC_ID { get; set; } //#Chng : 07-03-2022
        public string TABLEINC_ID { get; set; }
        public Int32 TABLEOPENINC_ID { get; set; }
        //public Int32 SIDETABLEINC_ID { get; set; } //#Chng : 07-03-2022
        public string SIDETABLEINC_ID { get; set; } 
        public Int32 SIDEOPENINC_ID { get; set; }
        public Int32 TABLEBLACKINC_ID { get; set; }
        public Int32 SIDEBLACKINC_ID { get; set; }
        public Int32 REDSPORTINC_ID { get; set; }

        public Int32 GIRDLE_ID { get; set; }
        public Int32 FROMGIRDLE_ID { get; set; }
        public Int32 TOGIRDLE_ID { get; set; }
        public Int32 CULET_ID { get; set; }
        public Int32 LUSTER_ID { get; set; }
        public Int32 MILKY_ID { get; set; }

        public Int32 HA_ID { get; set; }
        public Int32 EYECLEAN_ID { get; set; }
        public string STARLENGTH { get; set; }
        public string  LOWERHALF { get; set; }

        public string UPLOADDATE { get; set; }
        public string LABISSUEDATE { get; set; }
        public string LABRESULTDATE { get; set; }
        public string LABRETURNDATE { get; set; }
        public string AVAILABLEDATE { get; set; }
        public string PRICEREVISEDDATE { get; set; }

        public bool ISNOBGM { get; set; }
        public bool ISBLACK { get; set; }
        public bool ISEXCLUSIVE { get; set; }

        public string COLORDESC { get; set; }
        public string FANCYCOLOR { get; set; }
        public string FANCYCOLORINTENSITY { get; set; }
        public string FANCYCOLOROVERTONE { get; set; }
        public string KEYTOSYMBOL { get; set; }
        public string REPORTCOMMENT { get; set; }

        public string GIRDLEDESC { get; set; }
        public string GIRDLECONDITION { get; set; }
        public string PAINTING { get; set; }
        public string PROPORTIONS { get; set; }
        public string PAINTCOMM { get; set; }
        public string SYNTHETICINDICATOR { get; set; }

        public string REMARK { get; set; }
        public string INSCRIPTION { get; set; }
        public string POLISHFEATURES { get; set; }
        public string SYMMETRYFEATURES { get; set; }
        public string CLIENTCOMMENT { get; set; }

        public double MFGRAPAPORT { get; set; }
        public double MFGDISCOUNT { get; set; }
        public double MFGPRICEPERCARAT { get; set; }
        public double MFGAMOUNT { get; set; }

        public double COSTRAPAPORT { get; set; }
        public double COSTDISCOUNT { get; set; }
        public double COSTPRICEPERCARAT { get; set; }
        public double COSTAMOUNT { get; set; }

        public double SALERAPAPORT { get; set; }
        public double SALEDISCOUNT { get; set; }
        public double SALEPRICEPERCARAT { get; set; }
        public double SALEAMOUNT { get; set; }

        public double EXPRAPAPORT { get; set; }
        public double EXPDISCOUNT { get; set; }
        public double EXPPRICEPERCARAT { get; set; }
        public double EXPAMOUNT { get; set; }

        public double RAPNETRAPAPORT { get; set; }
        public double RAPNETDISCOUNT { get; set; }
        public double RAPNETPRICEPERCARAT { get; set; }
        public double RAPNETAMOUNT { get; set; }

        public double JAMESALLENRAPAPORT { get; set; }
        public double JAMESALLENDISCOUNT { get; set; }
        public double JAMESALLENAMOUNTDISCOUNT { get; set; }
        public double JAMESALLENPRICEPERCARAT { get; set; }
        public double JAMESALLENAMOUNT { get; set; }

        public double COMPRAPAPORT { get; set; }
        public double COMPDISCOUNT { get; set; }
        public double COMPPRICEPERCARAT { get; set; }
        public double COMPAMOUNT { get; set; }

        public Int32 PROCESS_ID { get; set; }
        public string  PROCESSNAME { get; set; }
        
        public string MEASUREMENT { get; set; }
            
        public Guid PARTY_ID { get; set; }

        public string PARTYSTONENO { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }

        public string EntryType { get; set; }

        public Guid Lot_ID { get; set; }
        public string LotName { get; set; }
        public string MFGID { get; set; }

    }

}
