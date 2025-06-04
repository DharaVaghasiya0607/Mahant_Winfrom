using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class ACC_ConversionEntryProperty
    {
        public Guid TRN_ID { get; set; }
        public Int32 VoucherNo { get; set; }
        public string VoucherNoStr { get; set; }
        public string VoucherDate { get; set; }
        public string BillNO { get; set; }
        public string BillDate { get; set; }
        public Guid PartyId { get; set; }
        public string PartyName { get; set; }
        public double SaleAmount { get; set; }
        public double ExcRate { get; set; }
        public double AmtRs { get; set; }
        public double NetAmt { get; set; }
        public double RoundAmt { get; set; }
        public DateTime EntryDate { get; set; }
        public Guid EntryBy { get; set; }
        public string EntryIP { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid UpdateBy { get; set; }
        public string UpdateIP { get; set; }
        public Guid Company_ID { get; set; }
        public int FinYear_ID { get; set; }
        public string Remarks { get; set; }
        public string TYPE { get; set; }
        public Int32 PAYTERMS { get; set; }
        public string DEFAULTACCLEDGER { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }
}
