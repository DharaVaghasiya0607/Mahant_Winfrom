using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class MST_CategoryProperty
    {
        public Int32 CATEGORY_ID { get; set; }
        public string CATEGORYNAME { get; set; }
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }

    public class MST_CompanyProperty
    {
        public Int32 COMPANY_ID { get; set; }
        public string COMPANYNAME { get; set; }
        public bool ISACTIVE { get; set; }
        
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }

    public class MST_LedgerProperty
    {
        public Int32 LEDGER_ID { get; set; }
        public string LEDGERNAME { get; set; }
        public string LEDGERTYPE { get; set; }
        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        
        public int COMPANY_ID { get; set; }
        public string COMPANYNAME { get; set; }

        public int YEAR_ID { get; set; }
        public string YEARNAME { get; set; }

        public double OPENINGCREDIT { get; set; }
        public double OPENINGDEBIT { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }

    public class MST_YearProperty
    {
        public Int32 YEAR_ID { get; set; }
        public string YEARNAME { get; set; }
        public string FROMDATE { get; set; }
        public string TODATE { get; set; }
        public Int32 PREVIOUSYEAR_ID { get; set; }
        public bool ISACTIVE { get; set; }
       
        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }

    public class TRN_LedgerTransactionProperty
    {
        public Guid TRN_ID  { get; set; }
        public Int32 SRNO { get; set; }
        public Guid VOUCHER_ID  { get; set; }
        public int COMPANY_ID  { get; set; }
        public  string FINYEAR { get; set; }
        public string VOUCHERDATE  { get; set; }
        public string TRNTYPE  { get; set; }	
        public int VOUCHERNO  { get; set; }
        public Guid FROMLEDGER_ID  { get; set; }	
        public string FROMLEDGERNAME  { get; set; }	
        public Guid TOLEDGER_ID  { get; set; }
        public string TOLEDGERNAME  { get; set; }
        public double AMOUNT  { get; set; }
        public double FAMOUNT { get; set; }
        public string NOTE { get; set; }
        public int Currency_ID { get; set; }
        public double ExcRate { get; set; }
        public double FAmount { get; set; }
        public string ENTRYTYPE { get; set; }

        public string VOUCHERSTR { get; set; }
        public string BOOKTYPE { get; set; }
        public string BOOKTYPEFULL { get; set; }

        public string ENTRYDATE { get; set; } 

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
    }

}
