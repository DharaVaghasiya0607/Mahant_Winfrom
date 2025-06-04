using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class ParcelKapanInwardProperty
    {
        public Guid INWARD_ID { get; set; }
        public Guid SIZEASSORT_ID { get; set; }

        public Int32 DEPARTMENT_ID { get; set; }
        public string INWARDDATE { get; set; }
        public Int64 INWARDNO { get; set; }
        public Int64 TRANSFERNO { get; set; }
        public string KAPANNAME { get; set; }
        public Int32 PACKETNO { get; set; }
        public string TAG { get; set; }
        public string STOCKNO { get; set; }
        public Int32 SHAPE_ID { get; set; }

        public double CARAT { get; set; }
        public double FINALCARAT { get; set; }
        public double COSTPRICEPERCARAT { get; set; }
        public double COSTAMOUNT { get; set; }
        public string STATUS { get; set; }
        public double PENDINGCARAT { get; set; }

        public double EXCRATE { get; set; }

        public string REMARK { get; set; }

        public Int64 BYSIZEASSORT_ID { get; set; } //Used In ByAssortment (Size/Clarity)

        public string STRDEPARTMENTXML { get; set; }

        public Int64 BYDEPTTRANSFERNO { get; set; } //#p : 10-03-2022
        public Int32 MIXSIZE_ID { get; set; } //#p : 10-03-2022

        public string StrInwardXml { get; set; }
        public string StrXmlValuesForInwardXml { get; set; }
        public string TransferSummaryXml { get; set; }
        public string TransferDetailXml { get; set; }
        public string MODE { get; set; }

        public string SuratMergeTransferNo { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
        public string StrRateXml { get; set; }
    }

}
