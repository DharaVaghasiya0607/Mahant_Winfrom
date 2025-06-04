using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class MemoEntryProperty
    {

        public string MEMO_ID { get; set; }
        public Int64 MEMONO { get; set; }
        public string JANGEDNOSTR { get; set; }
        public Int32 SRNO { get; set; }
        public Int32 ISCHECKED { get; set; }
        public string MEMOTYPE { get; set; }
        public string MEMODATE { get; set; }
        
        public string THROUGH { get; set; }

        public string BILLINGPARTY_ID { get; set; }
        public string BILLINGPARTNAME { get; set; }
        public string SHIPPINGPARTY_ID { get; set; }
        public string BROKER_ID { get; set; }
        public double BROKERBASEPER { get; set; }
        public double BROKERPROFITPER { get; set; }

        public string ADAT_ID { get; set; }
        public double ADATPER { get; set; }

        public string SOLDPARTY_ID { get; set; }
        public string SELLER_ID { get; set; }

        public Int32 TERMS_ID { get; set; }
        public int TERMSDAYS { get; set; }
        public double TERMSPER { get; set; }
        public string TERMSDATE { get; set; }

        public Int32 CURRENCY_ID { get; set; }
        public double EXCRATE { get; set; }
        public double MEMODISCOUNT { get; set; }

        public string DELIVERYTYPE { get; set; }
        public string PAYMENTMODE { get; set; }
        public string BILLTYPE { get; set; }

        public string BILLINGADDRESS1 { get; set; }
        public string BILLINGADDRESS2 { get; set; }
        public string BILLINGADDRESS3 { get; set; }
        public Int32 BILLINGCOUNTRY_ID { get; set; }
        public string BILLINGSTATE { get; set; }
        public string BILLINGCITY { get; set; }
        public string BILLINGZIPCODE { get; set; }

        public string TRANSPORTNAME { get; set; }
        public string PLACEOFSUPPLY { get; set; }

        public string SHIPPINGADDRESS1 { get; set; }
        public string SHIPPINGADDRESS2 { get; set; }
        public string SHIPPINGADDRESS3 { get; set; }
        public Int32 SHIPPINGCOUNTRY_ID { get; set; }
        public string SHIPPINGSTATE { get; set; }
        public string SHIPPINGCITY { get; set; }
        public string SHIPPINGZIPCODE { get; set; }

        public int TOTALPCS { get; set; }
        public double TOTALCARAT { get; set; }
        public double TOTALAVGDISC { get; set; }
        public double TOTALAVGRATE { get; set; }

        public double GROSSAMOUNT { get; set; }
        public double DISCOUNTPER { get; set; }
        public double DISCOUNTAMOUNT { get; set; }

        public double INSURANCEPER { get; set; }
        public double INSURANCEAMOUNT { get; set; }

        public double SHIPPINGPER { get; set; }
        public double SHIPPINGAMOUNT { get; set; }

        public double GSTPER { get; set; }
        public double GSTAMOUNT { get; set; }

        //#P : 02-08-2020
        public double IGSTPER { get; set; }
        public double IGSTAMOUNT { get; set; }
        public double FIGSTAMOUNT { get; set; }
        public double CGSTPER { get; set; }
        public double CGSTAMOUNT { get; set; }
        public double FCGSTAMOUNT { get; set; }
        public double SGSTPER { get; set; }
        public double SGSTAMOUNT { get; set; }
        public double FSGSTAMOUNT { get; set; }
        
        //End : #P : 02-08-2020

        public string TrnNo_OldDB { get; set; }

        public double NETAMOUNT { get; set; }

        public double FGROSSAMOUNT { get; set; }
        public double FDISCOUNTAMOUNT { get; set; }
        public double FINSURANCEAMOUNT { get; set; }
        public double FSHIPPINGAMOUNT { get; set; }
        public double FGSTAMOUNT { get; set; }
        public double FNETAMOUNT { get; set; }

        public string REMARK { get; set; }
        public string STATUS { get; set; }
        public string SOURCE { get; set; }

        public int PROCESS_ID { get; set; }
        public string PROCESSNAME { get; set; }

        public Guid COMPANYBANK_ID { get; set; }
        public Guid PARTYBANK_ID { get; set; }
        public Guid COURIER_ID { get; set; }
        public Guid AIRFREIGHT_ID { get; set; }
        public string PLACEOFRECEIPTBYPRECARRIER { get; set; }
        public string PORTOFLOADING { get; set; }
        public string PORTOFDISCHARGE { get; set; }
        public string FINALDESTINATION { get; set; }
        public string COUNTRYOFORIGIN { get; set; }
        public string COUNTRYOFFINALDESTINATION { get; set; }

        public string FINYEAR { get; set; }
        public string VOUCHERNOSTR { get; set; }

        public string INSURANCETYPE { get; set; }
        public double GROSSWEIGHT { get; set; }

        public string CONSIGNEE { get; set; }
        public string ADDRESSTYPE { get; set; }
        public int ORDERAPPROVAL { get; set; }

        public double TCSPER { get; set; }
        public double TCSAMOUNT { get; set; }
        public double FTCSAMOUNT { get; set; }

        public double ROUNDOFFPER { get; set; }
        public double ROUNDOFFAMOUNT { get; set; }
        public double FROUNDOFFAMOUNT { get; set; }
        public string ROUNDOFFTPYE { get; set; }

        public string BILLDATE { get; set; }
        public string BILLNO { get; set; }

        // #D: 13-01-2021
        public Int32 LABSERVICECODE_ID { get; set; }
        public string LABSERVICECODE { get; set; }

        public Int32 NARRATION_ID { get; set; }
        // #D: 13-01-2021

        //#p : 21-04-2021
        public double BACKADDLESS { get; set; }
        public double TERMSADDLESSPER { get; set; }
        public double BLINDADDLESSPER { get; set; }
        //End : 21-04-2021

        //#D
        public string FINALBUYER_ID { get; set; }
        public string FINALBUYERADDRESS1 { get; set; }
        public string FINALBUYERADDRESS2 { get; set; }
        public string FINALBUYERADDRESS3 { get; set; }
        public Int32 FINALBUYERCOUNTRY_ID { get; set; }
        public string FINALBUYERSTATE { get; set; }
        public string FINALBUYERCITY { get; set; }
        public string FINALBUYERZIPCODE { get; set; }
        public bool ISCONSINGEE { get; set; }
        public string INVOICENO { get; set; }
        //End : #D

        public string CUSTOMPORT { get; set; }
        public string BILLOFENTRY { get; set; }
        public string ADCODE { get; set; }
        public string HAWBNO { get; set; }
        public string MAWBNO { get; set; }
        public string BILLOFENTRYDATE { get; set; }
        public string KPCERTINO { get; set; }

        public Guid AIRFREIGHT { get; set; }

        public int Consignee_Id { get; set; }

        public string CustomeBroker { get; set; }
        public string CustomeDesignation { get; set; }
        public string CustomeIDCard { get; set; }

        public string INTERMIDATORYBANK { get; set; }
        public Guid OTHERSTONEPARTY_ID { get; set; } //ADD MILAN(04-02-2021)
        public string OTHERSTONEPARTYNAME { get; set; } //ADD MILAN ""

        public string PRECARRIERBY { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnValueJanged { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }

        // Add khushbu 22-05-21
        public string BankRefNo { get; set; }
        public double BrokerAmount { get; set; }
        public double BrokerAmountFE { get; set; }
        public string InvTermsID { get; set; }

        // Add khushbu 27-05-21
        public double LessPer1 { get; set; }
        public double LessPer2 { get; set; }
        public double LessPer3 { get; set; }
        public double AdatAmt { get; set; }
        public double AdatAmtFE { get; set; }
        public double StkAmtFE { get; set; }
        public double ExpInvoiceAmt { get; set; }
        public double ExpInvoiceAmtFE { get; set; }
        public double ORG_EXCRATE { get; set; }
        public double ADDLESS_EXCRATE { get; set; }
        public double ROUND_ADDAMOUNT { get; set; }
        public double ROUND_LESSAMOUNT { get; set; }
        public string PURCHASE_ACCID { get; set; }

        //Add Khushbu 25-06-21
        public string APPROVEMEMO_ID { get; set; }
        public string APPROVEMEMONO { get; set; }
        public string APPROVEMEMO_ID_A { get; set; }
        public string ORDERMEMO_ID { get; set; }
        public string ORDERJANGEDNO { get; set; }
        public string BILLFORMAT { get; set; }

        public string IGST_RefNo { get; set; }
        public string ExcRateType { get; set; }

        public string BRNO { get; set; }
        public string ENTRYTYPE { get; set; }

        //Shiv
        public Int32 IS_APARTRECEIVE { get; set; }
        public Int32 APARTAPPROVAL { get; set; }
        public string ACCTYPE { get; set; }

        //shiv 27-05-2022
        public double BASEBROKERAGEEXCRATE { get; set; }

        public double ADATEXCRATE { get; set; }
        public double GRFREIGHT { get; set; }

        //shiv 03-06-2022
        public double BROTDSPER { get; set; }
        public double BROTDSRS { get; set; }
        public double BROTOTALAMT { get; set; }
        public bool IS_BROTDS { get; set; }

        public string Pickup { get; set; }
        public string BILLINGPARTY_NAMEFORBRANCH { get; set; }
        public bool IS_OUTSIDESTONE { get; set; }

        public double BROCGSTPER { get; set; }
        public double BROCGSTRS { get; set; }
        public double BROSGSTPER { get; set; }
        public double BROSGSTRS { get; set; }
        public double BROIGSTPER { get; set; }
        public double BROIGSTRS { get; set; }
        public bool IS_BROGST { get; set; }
        public string SBNo { get; set; }
        public string ConsignmentRefNo { get; set; }

        public double HKDRATE { get; set; }
        public double HKDAMOUNT { get; set; }
    }

}
