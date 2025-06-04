using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BusLib.TableName
{
    public class LedgerMasterProperty
    {
        //public Int64 LEDGER_ID { get; set; }

        public Guid LEDGER_ID { get; set; }

        public Int32 LEDGERCODE { get; set; }
        public string LEDGERNAME { get; set; }
        public string LEDGERTYPE { get; set; }

        public string CONTACTPER { get; set; }
        public string CONTACTPERMOBILENO { get; set; }

        public Int64 MANAGER_ID { get; set; } // Added By Keyur : 23082023
        public Int64 COMPANY_ID { get; set; } // Added By Keyur : 23082023
        public string COMPANYNAME { get; set; }

        public string EMAILID { get; set; }
        public string QQID { get; set; }
        public string WECHATID { get; set; }

        public string SKYPEID { get; set; }
        public string WEBSITE { get; set; }

        public string GENDER { get; set; }
        public Int32 DESIGNATION_ID { get; set; }
        public Int32 DEPARTMENT_ID { get; set; }

        public string BILLINGADDRESS1 { get; set; }
        public string BILLINGADDRESS2 { get; set; }
        public string BILLINGADDRESS3 { get; set; }
        public Int32 BILLINGCOUNTRY_ID { get; set; }
        public string BILLINGSTATE { get; set; }
        public string BILLINGCITY { get; set; }
        public string BILLINGZIPCODE { get; set; }

        public bool ISDISPLAYCOSTPRICE { get; set; }

        public string SHIPPINGADDRESS1 { get; set; }
        public string SHIPPINGADDRESS2 { get; set; }
        public string SHIPPINGADDRESS3 { get; set; }
        public Int32 SHIPPINGCOUNTRY_ID { get; set; }
        public string SHIPPINGSTATE { get; set; }
        public string SHIPPINGCITY { get; set; }
        public string SHIPPINGZIPCODE { get; set; }

        public string MOBILENO1 { get; set; }
        public string MOBILENO2 { get; set; }
        public string LANDLINENO { get; set; }

        public bool ISALLOWWEBLOGIN { get; set; }

        public string USERNAME { get; set; }
        public string PASSWORD { get; set; }
        public string MPIN { get; set; }

        public double DEFAULTDISCOUNTDIFF { get; set; }
        public double BROKERAGEPER { get; set; }
        public bool ISDEFAULTDISCOUNTDIFF { get; set; }

        public double MEMBERDISCOUNT { get; set; }
        public bool ISMEMBERDISCOUNT { get; set; }

        public double MEMBEPRICEPERCARAT { get; set; }
        public bool ISMEMBERPRICEPERCARAT { get; set; }

        public string STATUS { get; set; }
        public string REMARK { get; set; }
        public string SOURCE { get; set; }

        // Add : #P : 23-11-2019
        public bool ISOTHERSTOCKDISCDIFF { get; set; }
        public double OTHERSTOCKDISCOUNTDIFF { get; set; }
        //End : #P : 23-11-2019

        public byte[] EMPPHOTO { get; set; }
        //ADD BHAGYASHREE 03/08/2019
        public Int32 ACCTTYPE_ID { get; set; }
        public Int32 CURRENCY_ID { get; set; }
        public double EXCRATE { get; set; }
        public double OPENINGCREDITUSD { get; set; }
        public double OPENINGDEBITUSD { get; set; }

        public double OPENINGCREDITFE { get; set; }
        public double OPENINGDEBITFE { get; set; }

        public string PANNO { get; set; }
        public string GSTNO { get; set; }
        public string IECODE { get; set; }

        //

        public bool ISDISPLAYPURCHASEPARTY { get; set; }

        public Guid DEFAULTSELLER_ID { get; set; }
        public Guid BROKER_ID { get; set; }
        public string BROKERNAME { get; set; }
        public Guid FINDBYWHOM_ID { get; set; }
        public string PARTYFINDTYPE { get; set; }
        public string DATEOFJOIN { get; set; }
        public string DATEOFLEAVE { get; set; }
        public string DATEOFBIRTH { get; set; }
        public string DATEOFANNIVERSARY { get; set; }

        public Guid COORDINATOR_ID { get; set; }
        public bool ISNOBROKER { get; set; }

        public string REFFERENCE { get; set; }
        public double SALELIMIT { get; set; }

        public string GRFORMNO { get; set; }
        public string GRFORMDATE { get; set; }
        public string ARNO { get; set; }

        public Guid BANK_ID { get; set; }
        public string BANKNAME { get; set; }
        public string BANKACCOUNTNO { get; set; }
        public string BANKACCOUNTNAME { get; set; }
        public string IFSCCODE { get; set; }
        public string SWIFTCODE { get; set; }
        public string BRANCHNAME { get; set; }
        public string ADDRESS { get; set; }

        //Add shiv
        public string INTERMEDIARYBANK { get; set; }
        public Boolean ISALLOWWEBAPI { get; set; }


        public Guid OPENINGID { get; set; }

        public string ADCODE { get; set; }
        public string INTERMEDIATEBANKDETAIL { get; set; }

        public string SHIPPINGGSTNO { get; set; }
        public Int32 BILLINGDISTRICTCODE { get; set; }
        public Int32 SHIPPINGDISTRICTCODE { get; set; }
        public string BILLINGPLACEOFRECEIPTBYPRECARRIER { get; set; }
        public string SHIPPINGPLACEOFRECEIPTBYPRECARRIER { get; set; }
        // K ; 01-11-2020 For JameAllenPrice
        public bool ISSYNCJAMESALLEN { get; set; }

        public bool ISALLDISPLAYPARTY { get; set; }
        public bool ISDISPLAYALLORDER { get; set; }
        public bool ISDISPLAYALLMFGCOST { get; set; }

        public bool ISCOMPUTERPRICE { get; set; }

        public string PROCESS_ID { get; set; }

        public Int32 LOCATION_ID { get; set; }

        public string EINV_CLIENTID { get; set; }
        public string EINV_CLIENTSECRET { get; set; }
        public string EINV_GSTIN { get; set; }
        public string EINV_USERNAME { get; set; }
        public string EINV_PASSWORD { get; set; }
        public string EINV_URL { get; set; }
        public string EINV_TOKENURL { get; set; }

        public string FAXNO { get; set; }
        public string TYPEOFBUSINESS { get; set; }
        public string PASSPORTNO { get; set; }
        public string SOCIALSECURITYNO { get; set; }
        public string NATUREOFBUSINESS { get; set; }
        public string DATEOFBUSINESSESTABLISMENT { get; set; }
        public string BUSINESSREGISTRATIONNO { get; set; }
        public string ORGANIZATIONNO { get; set; }
        public string QBCNO { get; set; }

        public Guid PARTNER_ID { get; set; }
        public string PARTNERNAME { get; set; }
        public string PARTNERMOBILENO { get; set; }
        public string PARTNEREMAIL_ID { get; set; }
        public string NATIVEPLACE { get; set; }
        public string DISTRICT { get; set; }
        public string SAMAJ { get; set; }

        public string ReturnValue { get; set; }
        public string ReturnMessageType { get; set; }
        public string ReturnMessageDesc { get; set; }
        public string ACCCODE { get; set; } 

        //Add shiv
        public string PRECARRIBY { get; set; }
        public string VESSETFLIGHT { get; set; }
        public string PORTOFLODING { get; set; }
        public string FINALDEST { get; set; }
        public string PLACEOFREC { get; set; }
        public string PORTOFDISCHARGE { get; set; }
        public string PARTYDEC { get; set; }
        public string COMPDEC { get; set; }
        public string LEGALNAME { get; set; }
        public string DDADEC { get; set; }
        public string EXPORTDEC { get; set; }
        public string KYCTYPE { get; set; }
        public string AADHARCARDNO { get; set; }

        //add shiv
        public bool IsAllowHoldAccess { get; set; }
        public bool IsAllowReleaseAccess { get; set; }
        public string EMAILGROUP { get; set; }

        public bool IS_SecAddress { get; set; }
        public string Sec_Address { get; set; }
        public bool IS_TDSLIMIT { get; set; }
        public string Sec_Address1 { get; set; }
        public string Sec_Address2 { get; set; }
        public string SALLEREMAIL_ID { get; set; }
    }
}
