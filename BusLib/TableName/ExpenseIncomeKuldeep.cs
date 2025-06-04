using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class trn_ExpenseIncomeKuldeep
    {
        public Guid TRN_ID { get; set; }
        public Int32 SRNO { get; set; }
        public string ENTRYTYPE { get; set; }
        public string VOUCHERDATE { get; set; }
        public string FINYEAR { get; set; }
        public int VOUCHERNO { get; set; }
        public Guid FROMLEDGER_ID { get; set; }
        public string FROMLEDGERNAME { get; set; }
        public Guid TOLEDGER_ID { get; set; }
        public string TOLEDGERNAME { get; set; }
        public int Currency_ID { get; set; }
        public string Currency { get; set; }
        public double AMOUNT { get; set; }
        public double ExcRate { get; set; }
        public double FAMOUNT { get; set; }
        public string ENTRYDATE { get; set; }
        public string TrnType { get; set; }
        public string BookType { get; set; }
        public string BookTypeFull { get; set; }
        public string ReturnValueTrnID { get; set; }
        public string voucherStr { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }
}
