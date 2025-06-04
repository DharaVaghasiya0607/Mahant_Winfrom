using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class StockUploadProperty
    {
        public Guid STOCK_ID { get; set; }

        public Int64 STOCKNO { get; set; }
        public string STOCKTYPE { get; set; }

        public Int32 SHAPE_ID { get; set; }
        public Int32 COLOR_ID { get; set; }
        public Int32 CLARITY_ID { get; set; }
        public Int32 CUT_ID { get; set; }
        public Int32 POL_ID { get; set; }
        public Int32 SYM_ID { get; set; }
        public Int32 FL_ID { get; set; }
        public Int32 LOCATION_ID { get; set; }

        public Int32 PCS { get; set; }
        public double CARAT { get; set; }
        public Int32 BALANCEPCS { get; set; }
        public double BALANCECARAT { get; set; }

        public Int32 SIZE_ID { get; set; }
        public Int32 LAB_ID { get; set; }
        
        public string LABREPORTNO{ get; set; }

        public double LENGTH { get; set; }
        public double WIDTH { get; set; }
        public double HEIGHT { get; set; }
        public double TABLEPER { get; set; }
        public double DEPTHPER { get; set; }

        public string MEASUREMENT { get; set; }

        public double CRANGLE { get; set; }
        public double CRHEIGHT { get; set; }

        public double PAVANGLE { get; set; }
        public double PAVHEIGHT { get; set; }
        public double GIRDLEPER { get; set; }
        public string GIRDLEDESC { get; set; }

        public double COSTRAPAPORT { get; set; }
        public double COSTDISCOUNT { get; set; }
        public double COSTPRICEPERCARAT { get; set; }
        public double COSTAMOUNT { get; set; }

        public double SALERAPAPORT { get; set; }
        public double SALEDISCOUNT { get; set; }
        public double SALEPRICEPERCARAT { get; set; }
        public double SALEAMOUNT { get; set; }

        public Guid PARTY_ID { get; set; }

        public string REMARK { get; set; }

        public Int32 PROCESS_ID { get; set; }
        public Int32 PREVPROCESS_ID { get; set; }

        public Guid MEMO_ID { get; set; }
        public Guid MEMODETAIL_ID { get; set; }

        public string PARTYSTONENO { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }


    }

}
