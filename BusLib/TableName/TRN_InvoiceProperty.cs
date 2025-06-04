using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class TRN_InvoiceProperty
    {
        public string Invoice_ID { get; set; }
        public string InvoiceDate { get; set; }
        public string FinYear { get; set; }
        public int InvoiceNo { get; set; }
        public string InvoiceNoStr { get; set; }
        public string Party_ID { get; set; }
        public string Seller_ID { get; set; }
        public string AuthorisePerson_ID { get; set; }

        public string AuthorisePersonName { get; set; }
        public string PartyName { get; set; }

        public string MemoType { get; set; }
        public int Process_ID { get; set; }
        public string Terms_ID { get; set; }
        public string PhNo { get; set; }
        public string CurrencyType { get; set; }
        public string BankType { get; set; }
        public double ExcRate { get; set; }
        public double TotalCarat { get; set; }
        public double TotalAmountHKD { get; set; }
        public double TotalAmountUSD { get; set; }
        public string Remark { get; set; }
        public string BrNo { get; set; }

        public string DetailXML { get; set; }
        public string Pickup { get; set; }
        public bool PaymentDone { get; set; }
        public string INVOICETYPE { get; set; }

        public bool IS_Payment { get; set; }

        public string ReturnInvoiceNo { get; set; }
        public string ReturnInvoiceStr { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
        public string ReturnValue { get; set; }

    }
}
