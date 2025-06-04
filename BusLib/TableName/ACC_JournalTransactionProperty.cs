using System;

namespace BusLib.TableName
{
    public class ACC_JournalTransactionProperty
    {
        public Guid JournalTransaction_ID { get; set; }
        public int Year_ID { get; set; }
        public int Location_ID { get; set; }
        public Guid Company_ID { get; set; }
        public string BookType { get; set; }
        public int VoucherNo { get; set; }
        public string VoucherDate { get; set; }
        public string DebitLedger_ID { get; set; }
        public string CreditLedger_ID { get; set; }
        public decimal Amount { get; set; }
        public decimal AdjAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public string DebitNarration { get; set; }
        public string CreditNarration { get; set; }
        public Guid TdsReferance_ID { get; set; }
        public string TdsLedger_ID { get; set; }
        public decimal TdsRate { get; set; }
        public decimal TdsAmount { get; set; }
        public decimal TdsAdjAmount { get; set; }
        public string TdsNarration { get; set; }
        public bool IsApprov { get; set; }
        public string ReceiptPayment_ID { get; set; }
        public DateTime BankReconciliationDate { get; set; }
        public int ChequeNo { get; set; }
        public DateTime ChequeDate { get; set; }
        public int DrawnBank_ID { get; set; }
        public decimal PaidAmount { get; set; }
        public bool IsPaid { get; set; }
        public bool TdsApplicableOnJv { get; set; }
        public string TdsOnType { get; set; }
        public string Invoice_Id { get; set; }
        public string Invoice_BookType_ID { get; set; }
        public string CGSTLedger_ID { get; set; }
        public string SGSTLedger_ID { get; set; }
        public string IGSTLedger_ID { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public decimal CGSTAmount { get; set; }
        public decimal SGSTAmount { get; set; }
        public decimal IGSTAmount { get; set; }
        public decimal NetAmount { get; set; }
        public string TaxType { get; set; }
        public int RefVoucherNo { get; set; }
        public string Credit_Ac { get; set; }
        public string Debit_Ac { get; set; }
        public string Book_Name { get; set; }
        public int Pre_Record_ID { get; set; }
        public string Transaction_Remark { get; set; }
        public string BillNo { get; set; }
        public string HsnCode { get; set; }
        public string CGSTRCMRevLedger_ID { get; set; }
        public string SGSTRCMRevLedger_ID { get; set; }
        public string IGSTRCMRevLedger_ID { get; set; }
        public string CGSTRCMPayLedger_ID { get; set; }
        public string SGSTRCMPayLedger_ID { get; set; }
        public string IGSTRCMPayLedger_ID { get; set; }
        public string GstOnType { get; set; }
        public int BrokerageNo { get; set; }
        public bool IsAdjustDetail { get; set; }
        public string InputType { get; set; }
        public decimal RoundOfAmt { get; set; }
        public string RoundOfLedger_ID { get; set; }
        public int GST_ID { get; set; }
        public string DrCrNo { get; set; }
        public string DrCrDate { get; set; }
        public decimal BeforeC_GST_Amount { get; set; }
        public decimal BeforeS_GST_Amount { get; set; }
        public decimal BeforeI_GST_Amount { get; set; }
        public decimal DisPer { get; set; }
        public decimal DisAmount { get; set; }
        public decimal GrossAmount { get; set; }
        public DateTime Before_EditDate { get; set; }
        public int SubCost_ID { get; set; }

        public bool _WantUpdateDate = false;
        public bool AutoExpence = false;
        public string Mst_OnlineSalesOrderId { get; set; }
        public string IncentiveType { get; set; }
        public string CalcType { get; set; }
        public decimal IncentivePer { get; set; }
        public decimal IncentiveLessAmt { get; set; }
        public decimal IncentiveSale { get; set; }
        public decimal IncentiveReturn { get; set; }
        public decimal IncentiveNetAmt { get; set; }
        public decimal OtherAddAmt { get; set; }
        public int SortNo { get; set; }
        public string IRN { get; set; }
        public string AckDt { get; set; }
        public string AckNo { get; set; }
        public decimal TotBrokerageAmt { get; set; }
        public decimal OtherLessAmt { get; set; }
        public string Fromtransid { get; set; }
        public string Fromtransbook { get; set; }     
        public int Account_ID { get; set; }
        public string Bill_Narration { get; set; }
        public decimal SpecDiscRate { get; set; }
        public decimal SpecDiscAmt { get; set; }

        public string XMLDATA { get; set; }
        public bool AllowRepeatParty { get; set; }
        public bool IsDebit { get; set; }

        public string RETURNVALUE { get; set; }
        public string RETURNMESSAGETYPE { get; set; }
        public string RETURNMESSAGEDESC { get; set; }

    }
}
