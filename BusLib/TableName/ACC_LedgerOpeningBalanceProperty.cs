using System;

namespace BusLib.TableName
{
    public class ACC_LedgerOpeningBalanceProperty
    {
        public Int32 COMPANY_ID { get; set; }
        public Int32 LOCATION_ID { get; set; }
        public Int32 BOOKTYPE_ID { get; set; }
        public Int32 LEDGER_ID { get; set; }
        public Int32 ACCOUNTTYPE_ID { get; set; }
        
        public decimal CREDITAMOUNT { get; set; }
        public decimal DEBITAMOUNT { get; set; }

        public string NARRATION { get; set; }

        public string RETURNVALUE { get; set; }
        public string RETURNMESSAGETYPE { get; set; }
        public string RETURNMESSAGEDESC { get; set; }

    }

}
