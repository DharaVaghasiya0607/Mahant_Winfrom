using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class TRN_LedgerTranJournalProperty
    {
        public Guid TRN_ID { get; set; }
        public Int32 SRNO { get; set; }
        public string ACCLEDGTRNTRN_ID { get; set; }
        public Int32 ACCLEDGTRNSRNO { get; set; }
        public string ENTRYTYPE { get; set; }
        public string BOOKTYPE { get; set; }
        public string BOOKTYPEFULL { get; set; }
        public string VOUCHERDATE { get; set; }
        public string FINYEAR { get; set; }
        public int FINYEAR_ID { get; set; }
        public int VOUCHERNO { get; set; }
        public string VOUCHERSTR { get; set; }
        public Guid FROMLEDGER_ID { get; set; }
        public Guid REFLEDGER_ID { get; set; }
        public int CURRENCY_ID { get; set; }
        public double DEBAMOUNT { get; set; }
        public double CRDAMOUNT { get; set; }
        public double EXCRATE { get; set; }
        public double DEBAMOUNTFE { get; set; }
        public double CRDAMOUNTFE { get; set; }
        public string NOTE { get; set; }
        public string TRNTYPE { get; set; }
        public Guid MEMO_ID { get; set; }
        public Guid REFTRN_ID { get; set; }
        public Int32 REFSRNO { get; set; }
        public string BILL_NO { get; set; }
        public string BILL_DT { get; set; }
        public double EXCRATEDIFF { get; set; }
        public string TERMSDATE { get; set; }
        public string CHQ_NO { get; set; }
        public string CHQISSUEDT { get; set; }
        public string CHQCLEARDT { get; set; }
        public string XCONTRA { get; set; }
        public Int32 DATAFREEZ { get; set; }
        public string PAYTYPE { get; set; }
        public string REFTYPE { get; set; }
        public Int32 PAYTERMS { get; set; }

        public string REFBOOKTYPEFULL { get; set; }

        public int CONVERTTOINR { get; set; }


        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
        public string ReturnValueJanged { get; set; }

        public string FROMDATE { get; set; }
        public string TODATE { get; set; }
        public string LEDGER_ID { get; set; }
        public string GROUP_ID { get; set; }
        public string BROKER_ID { get; set; }
        public string CONSIGNEE_ID { get; set; }
        public string SALEPERSON_ID { get; set; }
        public string SHAPE_ID { get; set; }
        public string COLOR_ID { get; set; }
        public string CLARITY_ID { get; set; }
        public string SIZE_ID { get; set; }
        public string KAPAN_ID { get; set; }
        public string STONENO { get; set; }
        public string MEMONO { get; set; }
        public string ALL_OVERDUE { get; set; }
        public string ALL_OPENING { get; set; }
        public string ALL_PENDING { get; set; }
        public string ALL_POLISH { get; set; }
        public string ALL_RS { get; set; }
        public string REPORTTYPE { get; set; }
        public string BIll_TYPE { get; set; }
        public string FROMTRNNO { get; set; }
        public string TOTRNNO { get; set;}

        public string ACCTYPE { get; set; }
        public string Mode { get; set; }

        public string PARTYID { get; set; }

        public double BROKERAMT { get; set; }
    }
}
