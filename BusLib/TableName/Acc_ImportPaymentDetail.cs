using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class Acc_ImportPaymentDetail
    {
        public Int64 Id { get; set; }
        public string BeneficiaryId { get; set; }
        public string BeneficiaryAdd { get; set; }
        public string BeneficiaryBank { get; set; }
        public string BeneficiaryAcNo { get; set; }
        public string ForeignBankCharges { get; set; }
        public string BeneficiarySwiftcode { get; set; }
        public string CuurentAcNo { get; set; }
        public string USDAcNo { get; set; }
        public string DDAAcNo { get; set; }
        public string IPCDate { get; set; }
        public string InvoiceNo { get; set; }
        public double AmountDollar { get; set; }
        public string BillDate { get; set; }
        public string ThirdPartyCountry { get; set; }
        public string USDdetail { get; set; }
        public string Remark { get; set; }
        public string ReturnValue {get;set;}
        public string ReturnValueJanged  {get;set;}
        public string ReturnMessageType  {get;set;}
        public string ReturnMessageDesc { get; set; }
    }
}
