using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using BLL = BusLib;
using AxonDataLib;

namespace BusLib
{
    public class BOComboFill
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();


        public enum SELTABLE
        {
            MST_PROCESS = 1,
            MST_DEPARTMENT = 2,
            MST_DESIGNATION = 3,
            MST_EMPLOYEE = 4,
            MST_PRDTYPE = 6,
            MST_DOCUMENTTYPE = 7,
            MST_REPORT = 8,
            TRN_KAPANSINGLE = 9,
            MST_CURRENCY = 10,
            MST_CURRENTEXCRATECURRENCYWISE = 11,
            MST_COUNTRY = 12,
            MST_PARTY = 13, // Only Active Party
            MST_PARTYALL = 14,  //All Party (Active/Deactive/WebPending)
            MST_SALEPARTY = 15,
            MST_EXCELSETTING = 16,
            MST_PURCHASEPARTY = 17,
            MST_SHAPE = 18,
            MST_COLOR = 19,
            MST_CLARITY = 20,
            MST_CUT = 21,
            MST_POL = 22,
            MST_SYM = 23,
            MST_FL = 24,
            MST_TERMS = 25,
            WEBSTATUS = 26,
            MST_BROKERADAT = 27,
            MST_LOCATION = 28,
            MST_SIZE = 29,
            MST_LAB = 30,
            MST_ACCTHEAD = 31,
            MST_ACCTTYPE = 32,
            TRN_STOCK = 33,
            MST_SIZEGROUP = 34,
            MST_COLORSHADE = 35,
            MST_MILKY = 36,
            MST_GREEN = 37,
            MST_TINGE = 38,
            MST_EYECLEAN = 39,
            MST_NATTS = 40,
            MST_LEDGERCASHPAYMENT = 41,
            MST_LEDGERSALE = 42,
            MST_LEDGERBANK = 43,
            MST_LEDGERCASH = 44,
            MST_PARAALL = 45,
            MST_LBLC = 46,
            MST_LUSTER = 47,
            MST_GRAIN = 48,
            MST_TENSION = 49,
            MST_BLACKINC = 50,
            MST_OPENINC = 51,
            MST_WHITEINC = 52,
            MST_HA = 53,
            MST_PAVILLION = 54,
            MST_NATURAL = 55,
            MST_BOX = 56,
            MST_SALEPARTYONLY = 57,
            MST_COURIER = 58,
            MST_AIRFREIGHT = 59,
            MST_LEDGERBANKCASH = 60,
            MST_LEDGER = 61,
            MST_DAYS = 62,
            MST_PAYMENTTYPE = 63,
            MST_LEDGERWITHOUTCASHBANK = 64,
            MST_OFFERPRICEPARTY = 65,
            TRN_KAPAN = 66,
            MST_MIXCLARITY = 67,
            MST_MIXSIZE = 68,
            MST_BRANCHCOMPANY = 69,
            MST_PROCESSALL = 70,
            MST_RECHECKCODE = 71,
            MST_INSCRIPTIONCODE = 72,
            MST_PRICEHEAD = 73,
            PARCEL_KAPANINWARD = 74,
            PARCEL_STOCK = 75,
            PARCEL_MIXCLARITY = 76,
            MST_LABSERVICECODE = 77,
            PARCEL_BOX = 78,
            PARCEL_MIXSIZE = 79,
            ACC_NARRATION = 80,
            MST_STATE = 81,
            PARCEL_KAPAN = 82,
            MST_INVTERMS = 83,
            MST_FANCYCOLOR = 84,
            MST_SMARTSEARCH = 85,
            MST_ACCTRNTYPE = 86,
            MST_BROKER = 87,
            MST_ADAT = 88,
            MST_CABIN = 89,
            MST_PARCELSURATMERGENO = 90,
            PARCEL_DEPTTANSFERNO = 91,
            MST_COMMISSION = 92,
            TRN_SINGLESTOCK = 93,
            MST_EXPLED = 94,
            MST_CITY = 95,
            TRN_STOCKFORMEMO = 96,
            ACC_PURCHASEPARTY = 97,
            ACC_PURCHASEBANK = 98,
            ACC_IMPORTPENDINGBILL = 99,
            ACC_IMPORTGETBILL = 100,
            ACC_IMPORTLOCALPENDINGBILL = 101,
            ACC_IMPORTLOCALGETBILL = 102,
            MST_EXPLCLED = 103,
            MST_LCPARTY = 104,
            MST_ACC_COMPANY = 105,
            MST_TDSACCOUNT = 106,
            MST_TDSON = 107,
            PARCEL_LOT = 108,
            PARCEL_CLARITY = 109,
            PARCEL_COLOR = 110,
            PARCEL_SIZE = 111,
            PARCEL_SHAPE = 112,
            TRN_MIXPACKETCREATE = 113,
            PARCEL_BOXNAME = 114,
            PARCEL_STOCKNO = 115,
            MST_PARACOLORSHADE= 116,
            MST_TABLEBLACKINC =117,
            MST_SIDEBLACKINC = 118,
            MST_TABLE = 119,
            TRN_LOT = 120,
            MST_ADDITIONALPROPERTY = 121,
            MST_MATERIALCOLOR = 122,
            MST_MATERIALSHAPE = 123,
            MST_MATERIALTYPE = 124,
            PRODUCT_CATEGORY = 125,
            PRODUCT_SUBCATEGORY= 126,
            MST_CUSTOMER = 127,
            MST_PRDTYPE_PREDICTION = 128,
            RAPDATE = 129,
            PARCEL_BOXMASTER = 130,
            MST_LEDGERBROKER = 131,
            MST_LAB_PRD = 132,
            MST_CROWN_OPEN = 133,
            MST_PAVILION_OPEN = 134,
            MST_EMPPARTY = 135
        }

        public DataTable FillCmb(SELTABLE tenum)
        {
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", tenum.ToString());
            Ope.AddParams("EMPLOYEE_ID", BusLib.Configuration.BOConfiguration.gEmployeeProperty.LEDGER_ID);
            Ope.AddParams("Company_ID", BusLib.Configuration.BOConfiguration.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("YEAR_ID", BusLib.Configuration.BOConfiguration.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.FillDTab(BLL.Configuration.BOConfiguration.ConnectionString, BLL.Configuration.BOConfiguration.ProviderName, DTab, TPV.BOSProc.Fill_Combo, CommandType.StoredProcedure, "");
            return DTab;
        }

        public DataTable GetState(int pIntCountry_ID)
        {
            DataTable DTab = new DataTable();

            string Str = "Select STATE_ID,STATECODE,STATENAME,COUNTRY_ID,GSTCODE From MST_State With(NOLOCK) Where Country_ID = '" + pIntCountry_ID + "' And ISActive = 1 Order by StateName";
            Ope.FillDTab(BLL.Configuration.BOConfiguration.ConnectionString, BLL.Configuration.BOConfiguration.ProviderName, DTab, Str, CommandType.Text, "");
            return DTab;
        }

        public DataTable FillCmb(SELTABLE tenum, string pStrDataTableName)
        {
            DataTable DTab = new DataTable(pStrDataTableName);
            Ope.AddParams("OPE", tenum.ToString());
            Ope.FillDTab(BLL.Configuration.BOConfiguration.ConnectionString, BLL.Configuration.BOConfiguration.ProviderName, DTab, TPV.BOSProc.Fill_Combo, CommandType.StoredProcedure, "");
            return DTab;
        }

        public DataTable FillCmbLedgerWithGroup(SELTABLE tenum, Int64 pIntLedgerGroup)
        {
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", tenum.ToString());
            Ope.AddParams("LEDGERGROUP_ID", pIntLedgerGroup);

            Ope.FillDTab(BLL.Configuration.BOConfiguration.ConnectionString, BLL.Configuration.BOConfiguration.ProviderName, DTab, TPV.BOSProc.Fill_Combo, CommandType.StoredProcedure, "");
            return DTab;
        }

        public DataTable FillCmb(string tenum, Int64 pIntParentID)
        {
            DataTable DTab = new DataTable();

            Ope.AddParams("PARAMTYPE", tenum.ToString());
            Ope.AddParams("PARENTPARAM_ID", pIntParentID);

            Ope.FillDTab(BLL.Configuration.BOConfiguration.ConnectionString, BLL.Configuration.BOConfiguration.ProviderName, DTab, TPV.BOSProc.Fill_ComboParam, CommandType.StoredProcedure, "");
            return DTab;
        }
        public DataTable GetBuyerPartyDetail(Guid pBuyer_ID) //#Darshan : 24-05-2022
        {
            DataTable DTab = new DataTable();
            string Str = @"Select 
	                            L.LEDGER_ID AS PARTY_ID, L.LEDGERCODE AS PARTYCODE,	L.LEDGERNAME AS PARTYNAME,	L.COMPANYNAME,	L.LANDLINENO,	L.EMAILID,	L.LEDGERTYPE AS PARTYTYPE,	L.BILLINGADDRESS1 AS BILLINGADDRESS1 ,
	                            L.BILLINGADDRESS2 AS BILLINGADDRESS2,	L.BILLINGADDRESS3 AS BILLINGADDRESS3,	L.BILLINGCITY AS BILLINGCITY ,	L.BILLINGCOUNTRY_ID AS BILLINGCOUNTRY_ID,	BC.COUNTRYNAME AS BILLINGCOUNTRYNAME,
	                            L.BILLINGSTATE AS BILLINGSTATE,	L.BILLINGZIPCODE AS BILLINGZIPCODE,	L.Coordinator_ID AS COORDINATOR_ID,	Brk.LedgerName AS COORDINATORNAME
                            From MST_Ledger L With(NOLOCK) 
                            LEFT JOIN MST_Country BC WITH(NOLOCK) ON BC.Country_ID = L.BillingCountry_ID 
                            LEFT JOIN MST_Ledger Brk With(Nolock) On Brk.Ledger_ID = L.Coordinator_ID
                            Where L.LEDGER_ID = '" + pBuyer_ID + "'";
            Ope.FillDTab(BLL.Configuration.BOConfiguration.ConnectionString, BLL.Configuration.BOConfiguration.ProviderName, DTab, Str, CommandType.Text, "");
            return DTab;
        }
    }
}
