using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class BrokeragePaymentProperty
    {
        public Guid BROKERAGE_ID { get; set; }

        public string PAYMENTDATE { get; set; }

        public string TRNTYPE { get; set; }
        public string ENTRYTYPE { get; set; }
        public string PAYMENTTYPE { get; set; }

        public Guid LEDGER_ID { get; set; }
        public string LEDGERNAME { get; set; }

        public Guid REFLEDGER_ID { get; set; }
        public string REFLEDGERNAME { get; set; }

        public int CURRENCY_ID { get; set; }
        public string CURRENCYNAME { get; set; }

        public double EXCRATE { get; set; }
        public string FINYEAR { get; set; }

        public int VOUCHERNO { get; set; }
        public string VOUCHERSTR { get; set; }

        public string SUBTYPE { get; set; }
        public string TRANSREFNO { get; set; }

        public double AMOUNTUSD { get; set; }
        public double AMOUNTFE { get; set; }

        public Guid FIANANCEACCOUNT_ID { get; set; }
        public double FIANANCEAMTUSD { get; set; }
        public double FIANANCEAMTFE { get; set; }

        public string REMARK { get; set; }

        public double GROSSAMOUNTUSD { get; set; }
        public double GROSSAMOUNTFE { get; set; }

        public double TDSPer { get; set; }
        public double TDSAMOUNTUSD { get; set; }
        public double TDSAMOUNTFE { get; set; }

        public double CGSTPER { get; set; }
        public double CGSTAMOUNTUSD { get; set; }
        public double CGSTAMOUNTFE { get; set; }
        public double SGSTPER { get; set; }
        public double SGSTAMOUNTUSD { get; set; }
        public double SGSTAMOUNTFE { get; set; }
        public double IGSTPER { get; set; }
        public double IGSTAMOUNTUSD { get; set; }
        public double IGSTAMOUNTFE { get; set; }
        public double NETAMOUNTUSD { get; set; }
        public double NETAMOUNTFE { get; set; }

        public string BROKERAGEDETAIL { get; set; }

        public Guid BROKERAGEDETAIL_ID { get; set; }

        public Guid MEMO_ID { get; set; }

        public int SRNO { get; set; }

        public string BOOKTYPE { get; set; }
        public string BOOKTYPEFULL { get; set; }
        
        public double BROKERAGEAMOUNTUSD { get; set; }
        public double BROKERAGEAMOUNTFE { get; set; }
        public double PAIDAMOUNTUSD { get; set; }
        public double PAIDAMOUNTFE { get; set; }
        public double INVCURRENCY_ID { get; set; }
        public double INVEXCRATE { get; set; }
        public double EXCRATEDIFF { get; set; }

        public string ENTRYDATE { get; set; }
        
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }


}
