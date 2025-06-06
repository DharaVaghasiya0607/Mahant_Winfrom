using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using AxonDataLib;
using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using BusLib.TableName;
using System.Collections;

namespace BusLib.Master
{
    public class BOMST_Ledger
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable Fill(string pStrType, string StrActive = "ALL")
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("OPE", StrActive, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LEDGERTYPE", pStrType, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, SProc.MST_LedgerGetData, CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable FillLedgerBank(Guid IntLedger_ID)
        {
            Ope.ClearParams();
            DataTable DS = new DataTable();
            Ope.AddParams("LEDGER_ID", IntLedger_ID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DS, "MST_LedgerBankGetData", CommandType.StoredProcedure);

            return DS;
        }

        public DataTable FillLedgerPartnerDetail(Guid IntLedger_ID)
        {
            Ope.ClearParams();
            DataTable DS = new DataTable();
            Ope.AddParams("LEDGER_ID", IntLedger_ID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DS, "MST_LedgerPartnerDetailGetData", CommandType.StoredProcedure);

            return DS;
        }

        public DataTable FillInterDetail(Guid IntLedger_ID)
        {
            Ope.ClearParams();
            DataTable DS = new DataTable();
            Ope.AddParams("LEDGER_ID", IntLedger_ID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DS, "MST_IntermediaryGetData", CommandType.StoredProcedure);

            return DS;
        }


        public DataRow GetDataByPK(string pStrLedgerID)
        {
            Ope.ClearParams();
            Ope.AddParams("OPE", "ALL", DbType.String, ParameterDirection.Input);
            Ope.AddParams("LEDGER_ID", pStrLedgerID, DbType.String, ParameterDirection.Input);
            return Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, SProc.MST_LedgerGetData, CommandType.StoredProcedure);
        }

        public DataSet GetDataForLedgerListForPendingKYC(string pStrStatus)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("OPE", pStrStatus, DbType.String, ParameterDirection.Input);
            // Ope.AddParams("LEDGERTYPE", pStrLedgerType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "MST_LedgerListGetDataForPendingKYC", CommandType.StoredProcedure);
            return DS;
        }

        public Int32 FindMaxLedgerCode()
        {
            int pIntLedgerCode = 0; string StrQuery = "";
            int pIntLedgerCode_B = 0;

            Ope.ClearParams();
            DataTable DTab = new DataTable();
            //StrQuery = "SELECT MAX(LEDGERCODE) AS LedgerCode FROM MST_LEDGER WITH(NOLOCK) WHERE 1=1";
            StrQuery = "SELECT maxid AS LedgerCode FROM MST_Maximum WITH(NOLOCK) WHERE ID_TYPE = 'LEDGERCODE' ";
            pIntLedgerCode = Val.ToInt32(Ope.ExeScal(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text));

            return pIntLedgerCode + 1;

        }


        public void UpdateLedgerCode(int pLedgerCode)  //Add Khushbu
        {
            string StrQuery = "";

            StrQuery = "UPDATE MST_MAXIMUM WITH(ROWLOCK) SET MAXID =" + pLedgerCode + ", MAXIDSTR =" + pLedgerCode + " WHERE ID_TYPE = 'LEDGERCODE'";
            Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
        }

        public DataTable GetQMaster(int fINYEAR_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            string Str = "Select * From MST_QMaster With(NOLOCK) WHERE Q6 = '" + fINYEAR_ID + "'";
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, Str, CommandType.Text);
            return DTab;
        }

        public LedgerMasterProperty Save(LedgerMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("LEDGER_ID", pClsProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("LEDGERCODE", pClsProperty.LEDGERCODE, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("LEDGERNAME", pClsProperty.LEDGERNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("LEDGERTYPE", pClsProperty.LEDGERTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CONTACTPER", pClsProperty.CONTACTPER, DbType.String, ParameterDirection.Input);
                Ope.AddParams("CONTACTPERMOBILENO", pClsProperty.CONTACTPERMOBILENO, DbType.String, ParameterDirection.Input);

                Ope.AddParams("COMPANYNAME", pClsProperty.COMPANYNAME, DbType.String, ParameterDirection.Input);

                Ope.AddParams("EMAILID", pClsProperty.EMAILID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SALLEREMAIL_ID", pClsProperty.SALLEREMAIL_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("QQID", pClsProperty.QQID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("WECHATID", pClsProperty.WECHATID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SKYPEID", pClsProperty.SKYPEID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("WEBSITE", pClsProperty.WEBSITE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("GENDER", pClsProperty.GENDER, DbType.String, ParameterDirection.Input);

                Ope.AddParams("DEPARTMENT_ID", pClsProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("DESIGNATION_ID", pClsProperty.DESIGNATION_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("BILLINGADDRESS1", pClsProperty.BILLINGADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGADDRESS2", pClsProperty.BILLINGADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGADDRESS3", pClsProperty.BILLINGADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGSTATE", pClsProperty.BILLINGSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGCOUNTRY_ID", pClsProperty.BILLINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("BILLINGCITY", pClsProperty.BILLINGCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGZIPCODE", pClsProperty.BILLINGZIPCODE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("SHIPPINGADDRESS1", pClsProperty.SHIPPINGADDRESS1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS2", pClsProperty.SHIPPINGADDRESS2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGADDRESS3", pClsProperty.SHIPPINGADDRESS3, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGSTATE", pClsProperty.SHIPPINGSTATE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGCOUNTRY_ID", pClsProperty.SHIPPINGCOUNTRY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGCITY", pClsProperty.SHIPPINGCITY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGZIPCODE", pClsProperty.SHIPPINGZIPCODE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("MOBILENO1", pClsProperty.MOBILENO1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MOBILENO2", pClsProperty.MOBILENO2, DbType.String, ParameterDirection.Input);
                Ope.AddParams("LANDLINENO", pClsProperty.LANDLINENO, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ISALLOWWEBLOGIN", pClsProperty.ISALLOWWEBLOGIN, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("ISOTHERSTOCKDISCOUNTDIFF", pClsProperty.ISOTHERSTOCKDISCDIFF, DbType.Boolean, ParameterDirection.Input); //#P : 23-11-2019
                Ope.AddParams("OTHERSTOCKDISCOUNTDIFF", pClsProperty.OTHERSTOCKDISCOUNTDIFF, DbType.Double, ParameterDirection.Input); //#P : 23-11-2019

                Ope.AddParams("USERNAME", pClsProperty.USERNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PASSWORD", pClsProperty.PASSWORD, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MPIN", pClsProperty.MPIN, DbType.String, ParameterDirection.Input);

                Ope.AddParams("DEFAULTDISCOUNTDIFF", pClsProperty.DEFAULTDISCOUNTDIFF, DbType.Decimal, ParameterDirection.Input);
                //#K : 04-11-2020
                Ope.AddParams("BROKERAGEPER", pClsProperty.BROKERAGEPER, DbType.Decimal, ParameterDirection.Input);

                Ope.AddParams("ISDEFAULTDISCOUNTDIFF", pClsProperty.ISDEFAULTDISCOUNTDIFF, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("MEMBERDISCOUNT", pClsProperty.MEMBERDISCOUNT, DbType.Decimal, ParameterDirection.Input);
                Ope.AddParams("ISMEMBERDISCOUNT", pClsProperty.ISMEMBERDISCOUNT, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("ISMEMBERPRICEPERCARAT", pClsProperty.ISMEMBERPRICEPERCARAT, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("MEMBERPRICEPERCARAT", pClsProperty.MEMBEPRICEPERCARAT, DbType.Decimal, ParameterDirection.Input);

                Ope.AddParams("STATUS", pClsProperty.STATUS, DbType.String, ParameterDirection.Input);
                Ope.AddParams("REMARK", pClsProperty.REMARK, DbType.String, ParameterDirection.Input);

                Ope.AddParams("SOURCE", pClsProperty.SOURCE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("DEFAULTSELLER_ID", pClsProperty.DEFAULTSELLER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("BROKER_ID", pClsProperty.BROKER_ID, DbType.Guid, ParameterDirection.Input);

                //ADD BHAGYASHREE 03/08/2019
                Ope.AddParams("ACCTTYPE_ID", pClsProperty.ACCTTYPE_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("CURRENCY_ID", pClsProperty.CURRENCY_ID, DbType.Int32, ParameterDirection.Input);
                Ope.AddParams("EXCRATE", pClsProperty.EXCRATE, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("OPENINGCREDITUSD", pClsProperty.OPENINGCREDITUSD, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("OPENINGDEBITUSD", pClsProperty.OPENINGDEBITUSD, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("OPENINGCREDITFE", pClsProperty.OPENINGCREDITFE, DbType.Double, ParameterDirection.Input);
                Ope.AddParams("OPENINGDEBITFE", pClsProperty.OPENINGDEBITFE, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("GSTNO", pClsProperty.GSTNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PANNO", pClsProperty.PANNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("IECODE", pClsProperty.IECODE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("FINDBYWHOM_ID", pClsProperty.FINDBYWHOM_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PARTYFINDTYPE", pClsProperty.PARTYFINDTYPE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("DATEOFJOIN", pClsProperty.DATEOFJOIN, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("DATEOFLEAVE", pClsProperty.DATEOFLEAVE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("DATEOFBIRTH", pClsProperty.DATEOFBIRTH, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("DATEOFANNIVERSARY", pClsProperty.DATEOFANNIVERSARY, DbType.Date, ParameterDirection.Input);

                Ope.AddParams("COORDINATOR_ID", pClsProperty.COORDINATOR_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ISNOBROKER", pClsProperty.ISNOBROKER, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("REFFERENCE", pClsProperty.REFFERENCE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SALELIMIT", pClsProperty.SALELIMIT, DbType.Double, ParameterDirection.Input);

                Ope.AddParams("GRFORMNO", pClsProperty.GRFORMNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("GRFORMDATE", pClsProperty.GRFORMDATE, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("ARNNO", pClsProperty.ARNO, DbType.String, ParameterDirection.Input);

                Ope.AddParams("BANKACCOUNTNO", pClsProperty.BANKACCOUNTNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BANKACCOUNTNAME", pClsProperty.BANKNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BANKBRANCHNAME", pClsProperty.BRANCHNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BANKIFSCCODE", pClsProperty.IFSCCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BANKSWIFTCODE", pClsProperty.SWIFTCODE, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ISALLOWWEBAPI", pClsProperty.ISALLOWWEBAPI, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINYEAR_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("ADCODE", pClsProperty.ADCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("INTERMEDIATEBANKDETAIL", pClsProperty.INTERMEDIATEBANKDETAIL, DbType.String, ParameterDirection.Input);
                // #D: 19-08-2020

                Ope.AddParams("SHIPPINGGSTNO", pClsProperty.SHIPPINGGSTNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGPLACEOFRECEIPTBYPRECARRIER", pClsProperty.BILLINGPLACEOFRECEIPTBYPRECARRIER, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BILLINGDISTRICTCODE", pClsProperty.BILLINGDISTRICTCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGDISTRICTCODE", pClsProperty.SHIPPINGDISTRICTCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SHIPPINGPLACEOFRECEIPTBYPRECARRIER", pClsProperty.SHIPPINGPLACEOFRECEIPTBYPRECARRIER, DbType.String, ParameterDirection.Input);

                // #D; 19-08-2020

                //#K : 01-11-2020
                Ope.AddParams("ISSYNCJAMESALLEN", pClsProperty.ISSYNCJAMESALLEN, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("LOCATION_ID", pClsProperty.LOCATION_ID, DbType.Int32, ParameterDirection.Input);

                Ope.AddParams("EINV_CLIENTID", pClsProperty.EINV_CLIENTID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EINV_CLIENTSECRET", pClsProperty.EINV_CLIENTSECRET, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EINV_GSTIN", pClsProperty.EINV_GSTIN, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EINV_USERNAME", pClsProperty.EINV_USERNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EINV_PASSWORD", pClsProperty.EINV_PASSWORD, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EINV_URL", pClsProperty.EINV_URL, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EINV_TOKENURL", pClsProperty.EINV_TOKENURL, DbType.String, ParameterDirection.Input);

                Ope.AddParams("FAXNO", pClsProperty.FAXNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("TYPEOFBUSINESS", pClsProperty.TYPEOFBUSINESS, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PASSPORTNO", pClsProperty.PASSPORTNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SOCIALSECURITYNO", pClsProperty.SOCIALSECURITYNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("NATUREOFBUSINESS", pClsProperty.NATUREOFBUSINESS, DbType.String, ParameterDirection.Input);
                Ope.AddParams("DATEOFBUSINESSESTABLISMENT", pClsProperty.DATEOFBUSINESSESTABLISMENT, DbType.Date, ParameterDirection.Input);
                Ope.AddParams("BUSINESSREGISTRATIONNO", pClsProperty.BUSINESSREGISTRATIONNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ORGANIZATIONNO", pClsProperty.ORGANIZATIONNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("QBCNO", pClsProperty.QBCNO, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ACCCODE", pClsProperty.ACCCODE, DbType.String, ParameterDirection.Input); //Add Khushbu 13-05-21
                Ope.AddParams("Company_ID", BusLib.Configuration.BOConfiguration.COMPANY_ID, DbType.Guid, ParameterDirection.Input);

                //Add shiv 17-05-22
                Ope.AddParams("PRECARRIBY", pClsProperty.PRECARRIBY, DbType.String, ParameterDirection.Input);
                Ope.AddParams("VESSETFLIGHT", pClsProperty.VESSETFLIGHT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PORTOFLODING", pClsProperty.PORTOFLODING, DbType.String, ParameterDirection.Input);
                Ope.AddParams("FINALDEST", pClsProperty.FINALDEST, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PLACEOFREC", pClsProperty.PLACEOFREC, DbType.String, ParameterDirection.Input);
                Ope.AddParams("PORTOFDISCHARGE", pClsProperty.PORTOFDISCHARGE, DbType.String, ParameterDirection.Input);
                //


                //Add shiv 21-05-22
                Ope.AddParams("PARTYDEC", pClsProperty.PARTYDEC, DbType.String, ParameterDirection.Input);
                Ope.AddParams("COMPDEC", pClsProperty.COMPDEC, DbType.String, ParameterDirection.Input);
                Ope.AddParams("LEGALNAME", pClsProperty.LEGALNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("DDADEC", pClsProperty.DDADEC, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EXPORTDEC", pClsProperty.EXPORTDEC, DbType.String, ParameterDirection.Input);
                Ope.AddParams("KYCTYPE", pClsProperty.KYCTYPE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("AADHARCARDNO", pClsProperty.AADHARCARDNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SEC_ADDRESS", pClsProperty.Sec_Address, DbType.String, ParameterDirection.Input);
                Ope.AddParams("IS_SECADDRESS", pClsProperty.IS_SecAddress, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("IS_TDSLIMIT", pClsProperty.IS_TDSLIMIT, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("Sec_Address1", pClsProperty.Sec_Address1, DbType.String, ParameterDirection.Input);
                Ope.AddParams("Sec_Address2", pClsProperty.Sec_Address2, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ISALLOWHOLDACCESS", pClsProperty.IsAllowHoldAccess, DbType.Boolean, ParameterDirection.Input);
                Ope.AddParams("ISALLOWRELEASEACCESS", pClsProperty.IsAllowReleaseAccess, DbType.Boolean, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL;
                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, SProc.MST_LedgerSave, CommandType.StoredProcedure);
                
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;

        }
        public LedgerMasterProperty SaveLedgerDetailInfo(LedgerMasterProperty pClsProperty, DataTable DtExperience, DataTable DtFamily, DataTable DtReference, DataTable DtAttachment)
        {
            try
            {
                Ope.ClearParams();

                Ope.AddParams("TBL_MST_LEDGERATTACHMENTDETAILS", DtAttachment, DbType.Object, ParameterDirection.Input);

                Ope.AddParams("LEDGER_ID", pClsProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_LedgerDetailInfoSave", CommandType.StoredProcedure);

                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;

        }

        public LedgerMasterProperty SaveBank(LedgerMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("BANK_ID", pClsProperty.BANK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("LEDGER_ID", pClsProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("BANKNAME", pClsProperty.BANKNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BANKACCOUNTNO", pClsProperty.BANKACCOUNTNO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("IFSCCODE", pClsProperty.IFSCCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SWIFTCODE", pClsProperty.SWIFTCODE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("BRANCHNAME", pClsProperty.BRANCHNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("ADDRESS", pClsProperty.ADDRESS, DbType.String, ParameterDirection.Input);
                Ope.AddParams("INTERMEDIARYBANK", pClsProperty.INTERMEDIARYBANK, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL;
                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_LedgerBankSave", CommandType.StoredProcedure);
               
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;
        }

        public LedgerMasterProperty SavePartnerDetail(LedgerMasterProperty pClsProperty)
        {
            try
            {

                Ope.AddParams("PARTNER_ID", pClsProperty.PARTNER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("LEDGER_ID", pClsProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("PARTNERNAME", pClsProperty.PARTNERNAME, DbType.String, ParameterDirection.Input);
                Ope.AddParams("MOBILENO", pClsProperty.PARTNERMOBILENO, DbType.String, ParameterDirection.Input);
                Ope.AddParams("EMAIL_ID", pClsProperty.PARTNEREMAIL_ID, DbType.String, ParameterDirection.Input);
                Ope.AddParams("NATIVEPLACE", pClsProperty.NATIVEPLACE, DbType.String, ParameterDirection.Input);
                Ope.AddParams("DISTRICT", pClsProperty.DISTRICT, DbType.String, ParameterDirection.Input);
                Ope.AddParams("SAMAJ", pClsProperty.SAMAJ, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL;
                AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_LedgerPartnerDetailSave", CommandType.StoredProcedure);
               
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;
        }

        public int DeleteLedgerDetailInfo(string StrOpe, string StrID)
        {
            string StrQuery = "";

            if (StrOpe == "ATTACHMENTDETAIL")
            {
                StrQuery = "DELETE FROM MST_LEDGERATTACHMENT WITH(ROWLOCK) WHERE ATTACHMENT_ID = '" + StrID + "'";
            }

            return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
        }

        public int DeleteLedgerBank(string StrOpe, string StrBank_ID, Boolean pIsActive)
        {
            string StrQuery = "";

            if (StrOpe == "BANKDETAIL")
            {
                StrQuery = "UPDATE MST_LEDGERBANK WITH(ROWLOCK) SET ISACTIVE = '" + pIsActive + "' WHERE BANK_ID = '" + StrBank_ID + "'";
            }

            return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, StrQuery, CommandType.Text);
        }


        public DataRow CheckLogin(string pStrUserName, string pStrPassword)
        {
            Ope.ClearParams();
            Ope.AddParams("USERNAME", pStrUserName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PASSWORD", pStrPassword, DbType.String, ParameterDirection.Input);
            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

            DataRow DR = Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, "CheckLogin", CommandType.StoredProcedure);

            return DR;

        }


        public string CheckLoginValidation(string pStrUserName, string pStrPassword, string pStrExeVersion)
        {
            Ope.ClearParams();
            Ope.AddParams("USERNAME", pStrUserName, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PASSWORD", pStrPassword, DbType.String, ParameterDirection.Input);

            Ope.AddParams("EXEVERSION", pStrExeVersion, DbType.String, ParameterDirection.Input); //#P : 07-02-2020

            Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

            Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
            Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

            ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "CheckLoginValidation", CommandType.StoredProcedure);

            if (AL.Count != 0)
            {
                return Val.ToString(AL[2]);
            }
            return "";
        }

        public LedgerMasterProperty Delete(LedgerMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("LEDGER_ID", pClsProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, SProc.MST_LedgerDelete, CommandType.StoredProcedure);

                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }

            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;

        }

        public LedgerMasterProperty LedgerBanKDelete(LedgerMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();
                Ope.AddParams("BANK_ID", pClsProperty.BANK_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_LedgerBankDelete", CommandType.StoredProcedure);

                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }

            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;

        }

        



        public Int64 GetCompnayID(string pStrName)
        {
            string Str = Ope.FindText(Config.ConnectionString, Config.ProviderName, "MST_Ledger", "Ledger_ID", " And LedgerName = '" + pStrName + "' And LedgerGroup='Company'");
            return Val.ToInt64(Str);
        }


        public DataRow GetLedgerInfoByCode(string pStrGroup, string pStrCode, Guid StrLedger_ID, string StrOpe = "ALL")
        {
            Ope.ClearParams();
            Ope.AddParams("OPE", StrOpe, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LEDGERTYPE", pStrGroup, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LEDGER_ID", StrLedger_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("LEDGERCODE", pStrCode, DbType.String, ParameterDirection.Input);
            return Ope.GetDataRow(BusLib.Configuration.BOConfiguration.ConnectionString, BusLib.Configuration.BOConfiguration.ProviderName, "MST_LedgerGetData", CommandType.StoredProcedure);          
        }

        public DataSet GetledgerDetailDInfoata(Guid IntLedger_ID) //06-04-2019
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("LEDGER_ID", IntLedger_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "MST_LedgerDetailInfoGetData", CommandType.StoredProcedure);

            return DS;
        }
        public DataTable GetDataForLedgerPrint(string pIntLedger_ID, string pStrPartyGroup)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("LEDGER_ID", pIntLedger_ID, DbType.String, ParameterDirection.Input);
            Ope.AddParams("PARTYGROUP", pStrPartyGroup, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "RP_LedgerKYCPrint", CommandType.StoredProcedure);
            return DTab;
        }
        public DataSet GetDataForLedgerList(string pStrStatus, string pStrLedgerType)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("OPE", pStrStatus, DbType.String, ParameterDirection.Input);
            Ope.AddParams("LEDGERTYPE", pStrLedgerType, DbType.String, ParameterDirection.Input);
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "MST_LedgerListGetData", CommandType.StoredProcedure);
          return DS;
        }


        public DataTable FillFinYear(string pdept)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("DEPT", pdept, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "FillFinYear", CommandType.StoredProcedure);
            return DTab;
        }

        public string MergeParty(Guid pGuidFromParty_ID, Guid pGuidToParty_ID) // D: 13-02-2021
        {
            string ReturnValue = "";
            string ReturnMessageType = "";
            string ReturnMessageDesc = "";

            try
            {
                Ope.ClearParams();
                Ope.AddParams("FROMPARTY_ID", pGuidFromParty_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("TOPARTY_ID", pGuidToParty_ID, DbType.Guid, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_PartyMergeSave", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    ReturnValue = Val.ToString(AL[0]);
                    ReturnMessageType = Val.ToString(AL[1]);
                    ReturnMessageDesc = Val.ToString(AL[2]);
                }

            }
            catch (Exception Ex)
            {
                ReturnValue = "";
                ReturnMessageType = "FAIL";
                ReturnMessageDesc = Ex.Message;
            }
            return ReturnMessageType;

        }

        public DataTable GetProcessDataForParty(Guid pGuidFromParty_ID)
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("FROMPARTY_ID", pGuidFromParty_ID, DbType.Guid, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "MST_PartyMergeProcessGetData", CommandType.StoredProcedure);
            return DTab;
        }
        
        //public DataTable GetQMaster(string part)
        //{
        //    Ope.ClearParams();
        //    DataTable DTab = new DataTable();
        //    string Str = "Select * From MST_QMaster With(NOLOCK) WHERE Q6 = '" + part +"'";
        //    Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, Str, CommandType.Text);
        //    return DTab;
        //}

        public DataTable GetLedgerDataForTCSAmount(string pIntLedger_ID) // Add Khushbu 20-05-21
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("LEDGER_ID", pIntLedger_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("FINYEAR", Config.FINYEARNAME, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_LedgerTCSAmountGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetLedgerDataForTDSCredit(string pIntLedger_ID, string pMemoDate, Guid pStrMemoID, double pDouGrossAmount) // Add shiv 01-07-22
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("LEDGER_ID", pIntLedger_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("FinYear_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("MEMODATE", pMemoDate, DbType.String, ParameterDirection.Input);
            Ope.AddParams("Memo_ID", pStrMemoID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("GrossAmount", pDouGrossAmount, DbType.Int32, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_LedgerTDSAmountGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetLedgerDataForTDSCreditLimit(string pIntLedger_ID) // Add shiv 01-07-22
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("LEDGER_ID", pIntLedger_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("FinYear_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);            

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "GetPartyMaxLimit", CommandType.StoredProcedure);
            return DTab;
        }

        public DataSet GetLedgerDataForSyncronised() // Add Khushbu 23-06-21
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "MST_LedgerSyncronised_Getdata", CommandType.StoredProcedure);

            return DS;
        }

        public DataTable CheckEvent(string strMacAddress)
        {
            DataTable Dtab = new DataTable();
            Ope.ClearParams();
            Ope.AddParams("MACADDRESS", strMacAddress, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, Dtab, "MST_AxoneEvents", CommandType.StoredProcedure);
            return Dtab;

        }
        public LedgerMasterProperty UpdateLedgerEmailGroup(LedgerMasterProperty pClsProperty)
        {
            try
            {
                Ope.ClearParams();

                Ope.AddParams("LEDGER_ID", pClsProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("EMAILGROUP", pClsProperty.EMAILGROUP, DbType.String, ParameterDirection.Input);

                Ope.AddParams("ReturnValue", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageType", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("ReturnMessageDesc", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "MST_LedgerListEmailGroupUpdate", CommandType.StoredProcedure);

                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }

            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;
        }

        //ADD SHIV 28-11-22
        public DataTable GetLedgerDataForPurTDSCredit(string pIntLedger_ID, string pMemoDate) // Add shiv 28-11-22
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("LEDGER_ID", pIntLedger_ID, DbType.Guid, ParameterDirection.Input);
            Ope.AddParams("FinYear_ID", Config.FINYEAR_ID, DbType.Int32, ParameterDirection.Input);
            Ope.AddParams("MEMODATE", pMemoDate, DbType.String, ParameterDirection.Input);

            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_LedgerPurchaseTDSAmountGetData", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetRemotVersion()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            string Str = "SELECT settingvalue FROM mst_setting WHERE settingkey = 'ExeVersion'";
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, Str, CommandType.Text);
            return DTab;
        }


    }
}
