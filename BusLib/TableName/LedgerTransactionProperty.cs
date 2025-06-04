using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class LedgerTransactionProperty
    {
        public Guid Trn_ID { get; set; }

        public int SrNo { get; set; }
        public int GSrNo { get; set; }

        public string TrnType { get; set; }
        public string EntryType { get; set; }
        public string BookType { get; set; }
        public string BookTypeFull { get; set; }
        public string FinYear { get; set; }
        public int VoucherNo { get; set; }
        public string VoucherDate { get; set; }

        public string VoucherStr { get; set; }

        public Guid Ledger_ID { get; set; }
        public string LedgerName { get; set; }

        public Guid RefLedger_ID { get; set; }
        public string RefLedgerName { get; set; }

        public int Currency_ID { get; set; }
        public double Amount { get; set; }
        public double ExcRate { get; set; }
        public double FAmount { get; set; }

        //#P : 31-07-2020
        public double BankCharges { get; set; }
        public double BankChargesFE { get; set; }

        public double TDSAmount { get; set; }
        public double TDSAmountFE { get; set; }
        public double TDSPer { get; set; }
        //End : #P : 31-07-2020


        public double PendingAmount { get; set; }
        public double PendingAmountFE { get; set; }

        public string SubType { get; set; }
        public string RefDocNo { get; set; }
        public string ChequeNo { get; set; }
        public string Note { get; set; }

        public Int32 InvCurrency_ID { get; set; }
        public double InvExcRate { get; set; }
        public double ExcRateDiff { get; set; }

        public Guid MEMO_ID { get; set; }
        public string ENTRYDATE { get; set; }

        public string PAYMENTTYPE { get; set; }

        //#P : 10-08-2020
        public int  TERMSMONTH { get; set; }
        public string TERMSDATE { get; set; }
        public double INTERESTPER { get; set; }
        public Guid REFWITHOUTBILLTRN_ID { get; set; }
        //End : #P : 10-08-2020

        public Guid REFTRN_ID { get; set; }

        public Int32 CONVERTTOINR { get; set; }
        
        public string ReturnValueTrnID { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }


}
