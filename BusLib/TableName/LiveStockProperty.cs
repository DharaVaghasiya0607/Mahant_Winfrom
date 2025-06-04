using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class LiveStockProperty
    {
        public Guid STOCK_ID { get; set; }

        public string STOCKNO { get; set; }
        public string LABJANGEDNO { get; set; }

        public string PARTYSTOCKNO { get; set; }

        public string STOCKTYPE { get; set; }

        public int SHAPE_ID { get; set; }
        public int COLOR_ID { get; set; }
        public int CLARITY_ID { get; set; }
        public int CUT_ID { get; set; }
        public int POL_ID { get; set; }
        public int SYM_ID { get; set; }
        public int FL { get; set; }


        public int BLACKINC_ID { get; set; }

        public string EMPLOYEE_ID { get; set; }
        public string EMPLOYEECODE { get; set; }

        public int CABIN_ID { get; set; }
        public string REMARK { get; set; }
        public int ISISSUESTOCK { get; set; }

        public string MULTYSHAPE_ID { get; set; }
        public string MULTYCOLOR_ID { get; set; }
        public string MULTYCLARITY_ID { get; set; }
        public string MULTYCUT_ID { get; set; }
        public string MULTYPOL_ID { get; set; }
        public string MULTYSYM_ID { get; set; }
        public string MULTYFL_ID { get; set; }
        public string MULTYSIZE_ID { get; set; }

        public string MULTYFANCYCOLOR_ID { get; set; }
        public string MULTYLOCATION_ID { get; set; }
        public string MULTYMILKY_ID { get; set; }

        public string MULTYTABLEBLACK_ID { get; set; }
        public string MULTYSIDEBLACK_ID { get; set; }
        public string MULTYLAB_ID { get; set; }
        public string MULTYBOX_ID { get; set; }
        public string MULTYKAPAN { get; set; }
        public string MULTYCOLORSHADE_ID { get; set; }
         public double DISCOUNTGP { get; set; }
        public double PERCARATGP { get; set; }

        public string MULTYPARTY_ID { get; set; }

        public int TABLEINC_ID { get; set; }
        public int TABLEOPEN_ID { get; set; }
        public int LUSTER_ID { get; set; }
        public int MILKY_ID { get; set; }

        public int LOCATION_ID { get; set; }

        public int PCS { get; set; }
        public double CARAT { get; set; }

        public int BALANCEPCS { get; set; }
        public double BALANCECARAT { get; set; }

        public string DEPARTMENT_ID { get; set; }

        public string SIZE_ID { get; set; }

        public int LAB_ID { get; set; }
        public string LAB { get; set; }




        public string LABREPORTNO { get; set; }
        public string SERIALNO { get; set; }
        public string MEMONO { get; set; }
        //public string MFGNO { get; set; }
        public string WEBSTATUS { get; set; }
        public string BROWN { get; set; }

        public string BOX_ID { get; set; }

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
        public double EXCRATE { get; set; }

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


        public bool ISUNGRADEDTOMIX { get; set; }


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

        public string SALESPARTY_ID { get; set; }
        public string SALESJANGADNO { get; set; }
        public string PURCHASEJANGADNO { get; set; }

        public Guid MEMO_ID { get; set; }

        public string REFMEMO_ID { get; set; }

        public int ISSALEDELIVERY { get; set; }

        public string LABISSUEFROMDATE { get; set; }
        public string LABISSUETODATE { get; set; }

        public string LABRESULTFROMDATE { get; set; }
        public string LABRESULTTODATE { get; set; }

        public string LABRETURNFROMDATE { get; set; }
        public string LABRETURNTODATE { get; set; }

        public string AVAILBLEFROMDATE { get; set; }
        public string AVAILBLETODATE { get; set; }

        public string SALESFROMDATE { get; set; }
        public string SALESTODATE { get; set; }

        public string DELIVERYFROMDATE { get; set; }
        public string DELIVERYTODATE { get; set; }

        public string UPLOADFROMDATE { get; set; }
        public string UPLOADTODATE { get; set; }

        public string PRICEREVICEDFROMDATE { get; set; }
        public string PRICEREVICEDTODATE { get; set; }

        public int ISHKSTONEDATA { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }

        public string MFG_ID { get; set; } //Added by Daksha on 27/02/2023
        public string BARCODE { get; set; }
        public string KAPANNAME { get; set; }
        public string JANGEDNO { get; set; }
        public int PACKETNO { get; set; }
        public string DIAMONDTYPE { get; set; }

        public Int32 COLORSHADE_ID { get; set; }
        public Int32 CROWNOPEN_ID { get; set; }
        public Int32 PAVOPEN_ID { get; set; }

        public Int32 PRDTYPE_ID { get; set; }

        public Int32 MAXSRNO { get; set; }
    }

}
