using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.Account
{
    public class ACC_BankConversionRateProperty
    {
        public Guid Trn_Id { get; set; }
        public Int32 VoucherNo { get; set; }
        public string VoucherNoStr { get; set; }
        public string ConversionDate { get; set; }
        public string InvoiceBillType { get; set; }
        public Guid InvoiceParty_Id { get; set; }
        public string InvoiceNoStr { get; set; }
        public string InvoiceDate { get; set; }
        public Int32 InvoiceCurrency_Id { get; set; }


        public double Invoice_ExcRate { get; set; }
        public double Invoice_GrossAmount { get; set; }
        public double Invoice_FGrossAmount { get; set; }
        public double Invoice_CGSTPer { get; set; }
        public double Invoice_CGSTAmount { get; set; }
        public double Invoice_FCGSTAmount { get; set; }
        public double Invoice_SGSTPer { get; set; }
        public Guid INVOICEMEMO_ID { get; set; }
        public double Invoice_SGSTAmount { get; set; }
        public double Invoice_FSGSTAmount { get; set; }
        public double Invoice_IGSTPer { get; set; }
        public double Invoice_IGSTAmount { get; set; }
        public double Invoice_FIGSTAmount { get; set; }
        public double Invoice_NetAmount { get; set; }
        public double Invoice_FNetAmount { get; set; }

        public Guid Bank_Id { get; set; }

        public double ConvRate { get; set; }
        public double GrossAmount { get; set; }
        public double FGrossAmount { get; set; }
        public double TCSPer { get; set; }
        public double TCSAmount { get; set; }
        public double FTCSAmount { get; set; }
        public double ForeChargePer { get; set; }
        public double ForeChargeAmount { get; set; }
        public double FForeChargeAmount { get; set; }
        public double NetAmount { get; set; }
        public double FNetAmount { get; set; }
        public double ExcDiffAmount { get; set; }
        public double FExcDiffAmount { get; set; }

        public string Remark { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }

        public string FINYEAR_ID { get; set; }
        public int INVOICENO { get; set; }
        public string AccType { get; set; }//Gunjan:29/05/2023
    }
}
