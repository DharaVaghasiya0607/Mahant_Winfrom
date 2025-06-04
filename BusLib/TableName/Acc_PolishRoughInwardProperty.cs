using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class Acc_PolishRoughInwardProperty
    {
        public Int64 ID { get; set; }
        public Int32 VoucherNo { get; set; }
        public DateTime VoucherDate { get; set; }
        public Guid PartyId { get; set; }
        public string PartyName { get; set; }
        public string ItemName { get; set; }
        public double RoughCarat { get; set; }
        public double RoughRate { get; set; }
        public double RoughAmtRs { get; set; }
        public double RegCarat { get; set; }
        public double RegRate { get; set; }
        public double RegAmtRs { get; set; }
        public double MfgCarat { get; set; }
        public double MfgAmtRs { get; set; }
        public double PolishPCarat { get; set; }
        public double LabourAmt { get; set; }
        public DateTime EntryDate { get; set; }
        public Guid EntryBy { get; set; }
        public string EntryIP { get; set; }
        public DateTime UpdateDate { get; set; }
        public Guid UpdateBy { get; set; }
        public string UpdateIP { get; set; }
        public Guid Company_ID { get; set; }
        public int FinYear_ID { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }
}
