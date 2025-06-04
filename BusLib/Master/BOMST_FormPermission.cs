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
using System.Data.SqlClient;
using BusLib.Configuration;
using System.Security.Cryptography;
using System.IO;

namespace BusLib.Master
{
    public class BOMST_FormPermission
    {
        AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataSet Fill(string pIntEmployeeID)
        {
            Ope.ClearParams();
            DataSet DS = new DataSet();
            Ope.AddParams("EMPLOYEE_ID", pIntEmployeeID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDSet(Config.ConnectionString, Config.ProviderName, DS, "Temp", "MST_FormPermissionGetData", CommandType.StoredProcedure);
           return DS;
        }


        public int Save(string pIntEmployeeID, DataTable DTab, DataTable pDTabDisplay, DataTable pDTabTransfer, Boolean pBoolISDisplayCost, Boolean pBoolIsDisplayPurParty, Boolean pBoolIsDisplayParty, Boolean pBoolIsDisplayOrder, string pStrProcess_ID, Boolean pBoolIsDisplayAllMfgCost, Boolean pBoolIsComputerPrice)
      {
            int IntRes = 0;
               
            try
            {

                Ope.ClearParams();
                string Str = "DELETE FROM MST_FORMPERMISSION With(RowLock) Where Employee_ID = '" + pIntEmployeeID + "' ; ";
                Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);

                Ope.ClearParams();
                Str = "UPDATE MST_LEDGER WITH(ROWLOCK) SET PROCESS_ID = '" + pStrProcess_ID + "' ,ISDisplayCostPrice = " + (pBoolISDisplayCost == true ? 1 : 0) + " ,ISDisplayPurparty = " + (pBoolIsDisplayPurParty == true ? 1 : 0) + ", IsDisplayAllParty = " + (pBoolIsDisplayParty == true ? 1 : 0) + " , IsDisplayAllOrder = " + (pBoolIsDisplayOrder == true ? 1 : 0) + ", IsDisplayAllMfgCost = " + (pBoolIsDisplayAllMfgCost == true ? 1 : 0) + ", IsComputerPrice = " + (pBoolIsComputerPrice == true ? 1 : 0) + " Where Ledger_ID = '" + pIntEmployeeID + "' ; ";
                Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);

                foreach (DataRow DRow in DTab.Rows)
                {
                    Ope.ClearParams();
                    Ope.AddParams("EMPLOYEE_ID", pIntEmployeeID, DbType.Guid, ParameterDirection.Input);
                    Ope.AddParams("FORM_ID", Val.ToInt32(DRow["FORM_ID"]), DbType.Int32, ParameterDirection.Input);
                    Ope.AddParams("ISVIEW", Val.ToBoolean(DRow["ISVIEW"]), DbType.Boolean, ParameterDirection.Input);
                    Ope.AddParams("ISINSERT", Val.ToBoolean(DRow["ISINSERT"]), DbType.Boolean, ParameterDirection.Input);
                    Ope.AddParams("ISUPDATE", Val.ToBoolean(DRow["ISUPDATE"]), DbType.Boolean, ParameterDirection.Input);
                    Ope.AddParams("ISDELETE", Val.ToBoolean(DRow["ISDELETE"]), DbType.Boolean, ParameterDirection.Input);
                    Ope.AddParams("PASSWORD", Val.ToString(DRow["PASSWORD"]), DbType.String, ParameterDirection.Input);
                    IntRes = IntRes + Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "MST_FormPermissionSave", CommandType.StoredProcedure);
                }
            }
            catch (System.Exception ex)
            {
                return -1;
            }

            return IntRes;
        }
        public int SaveLayoutRights(string pIntEmployeeID, string pIntCopyEmployeeID)
        {
            int IntRes = 0;

            try
            {

                    Ope.ClearParams();
                    Ope.AddParams("EMPLOYEE_ID", pIntEmployeeID, DbType.Guid, ParameterDirection.Input);
                    Ope.AddParams("COPYEMPLOYEE_ID", pIntCopyEmployeeID, DbType.Guid, ParameterDirection.Input);              
                    IntRes =  Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, "MST_EpmloyeeLayoutSave", CommandType.StoredProcedure);
                
            }
            catch (System.Exception ex)
            {
                return -1;
            }

            return IntRes;
        }

        public EmployeeActionRightsProperty EmployeeActionRightsGetDataByPK(Guid pIntEmployeeID)
        {
            Ope.ClearParams();
            Ope.AddParams("EMPLOYEE_ID", pIntEmployeeID, DbType.Guid, ParameterDirection.Input);
            DataRow DRow = Ope.GetDataRow(Config.ConnectionString, Config.ProviderName, "MST_EmployeeActionRightsGetdata", CommandType.StoredProcedure);

            EmployeeActionRightsProperty Property = new EmployeeActionRightsProperty();
            Property.EMPLOYEE_ID = pIntEmployeeID;
            Property.PRDTYPE_ID = "";
            Property.ISFULLSTOCK = false;
            Property.ISDEPTSTOCK = false;
            Property.ISMYSTOCK = false;
            Property.ISOTHERSTOCK = false;
            Property.DEPTTRANSFER = false;
            Property.EMPISSUE = false;
            Property.EMPRETURN = false;
            Property.RETURNWITHSPLIT = false;
            Property.IPADDRESS = "";
            Property.ALLOWALLIP = false;
            Property.REJECTIONTRANSFER = false;

            Property.RAPPASSFORDISPDISC = "";
            Property.RAPCHANGEEMPLOYEE = false;
            Property.RAPCHANGEPACKETS = false;
            Property.RAPUPDATEPREDICTION = false;
            Property.MAXPACKETSTOCK = 0;
            
            if (DRow != null)
            {
                Property.PRDTYPE_ID = Val.ToString(DRow["PRDTYPE_ID"]);
                Property.ISFULLSTOCK = Val.ToBoolean(DRow["ISFULLSTOCK"]);
                Property.ISDEPTSTOCK = Val.ToBoolean(DRow["ISDEPTSTOCK"]);
                Property.ISMYSTOCK = Val.ToBoolean(DRow["ISMYSTOCK"]);
                Property.ISOTHERSTOCK = Val.ToBoolean(DRow["ISOTHERSTOCK"]);
                Property.DEPTTRANSFER = Val.ToBoolean(DRow["DEPTTRANSFER"]);
                Property.EMPISSUE = Val.ToBoolean(DRow["EMPISSUE"]);
                Property.EMPRETURN = Val.ToBoolean(DRow["EMPRETURN"]);
                Property.REJECTIONTRANSFER = Val.ToBoolean(DRow["REJECTIONTRANSFER"]);
                Property.RETURNWITHSPLIT = Val.ToBoolean(DRow["RETURNWITHSPLIT"]);
                Property.IPADDRESS = Val.ToString(DRow["IPADDRESS"]);
                Property.ALLOWALLIP = Val.ToBoolean(DRow["ALLOWALLIP"]);

                Property.RAPPASSFORDISPDISC = Val.ToString(DRow["RAPPASSFORDISPDISC"]);
                Property.RAPCHANGEEMPLOYEE = Val.ToBoolean(DRow["RAPCHANGEEMPLOYEE"]);
                Property.RAPCHANGEPACKETS = Val.ToBoolean(DRow["RAPCHANGEPACKETS"]);
                Property.RAPUPDATEPREDICTION = Val.ToBoolean(DRow["RAPUPDATEPREDICTION"]);
                Property.MAXPACKETSTOCK = Val.ToInt(DRow["MAXPACKETSTOCK"]);
            }

            return Property;
        }

        public DataTable GetSettingDataForExePathAndConnection()
        {
            string StrRes = "";
            DataTable DTab = new DataTable();
            try
            {
                Ope.ClearParams();
                string Str = "Select (Isnull(SETTINGKEY,'')) AS SETTINGKEY, (Isnull(SETTINGVALUE,'')) AS SETTINGVALUE From MST_Setting With(Nolock) Where UPPER(SETTINGKEY) LIKE '%EXEUPDATEPATH%'";
                Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, Str, CommandType.Text);
            }
            catch (Exception ex)
            {
                StrRes = string.Empty;
            }
            return DTab;
        }

        public DataTable GetUserAuthenticationGetData(Guid pIntEmployeeID)
        {

            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("EMPLOYEE_ID", pIntEmployeeID, DbType.Guid, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "MST_EmployeePermissionGetDataAuthentication", CommandType.StoredProcedure);
            return DTab;

        }

        //public string GetMessage()
        //{
        //    Ope.ClearParams();
        //    string Str = "Select SETTINGVALUE FROM MST_Setting With(NoLock) Where UPPER(SETTINGKEY) = 'MARQUEETEXT'";
        //    return Ope.ExeScal(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);

        //}

        public DataTable GetMessage() //Add : Pinali : 31-08-2019
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            string Str = "Select SETTINGVALUE,SETTINGVALUECHINESE FROM MST_Setting With(Nolock) Where (UPPER(SETTINGKEY) = 'MARQUEETEXT')";
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName,DTab, Str, CommandType.Text);
            return DTab;
        }

        public int SaveMessage(string MessageEnglish, string MessageChinese)
        {
            Ope.ClearParams();
            string Str = "Update MST_Setting With(RowLock) Set SETTINGVALUE= N'" + MessageEnglish + "' , SETTINGVALUECHINESE = N'" + MessageChinese + "' Where SETTINGKEY = 'MARQUEETEXT'";
            return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);

        }
        public DataTable GetMessageForAboutUS() //Add : Pinali : 11-09-2019
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            string Str = "Select SETTINGVALUE,SETTINGVALUECHINESE FROM MST_Setting With(Nolock) Where (UPPER(SETTINGKEY) = 'ABOUTUS')";
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, Str, CommandType.Text);
            return DTab;
        }

        public int SaveMessageForAboutUS(string MessageEnglish, string MessageChinese) //Add : Pinali : 11-09-2019
        {
            Ope.ClearParams();
            string Str = "Update MST_Setting With(RowLock) Set SETTINGVALUE= N'" + MessageEnglish + "' , SETTINGVALUECHINESE = N'" + MessageChinese + "' Where SETTINGKEY = 'ABOUTUS'";
            return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);

        }
        public DataTable GetMessageForOurTerms() //Add : Pinali : 11-09-2019
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            string Str = "Select SETTINGVALUE,SETTINGVALUECHINESE FROM MST_Setting With(Nolock) Where (UPPER(SETTINGKEY) = 'OURTERMS')";
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, Str, CommandType.Text);
            return DTab;
        }

        public int SaveMessageForOurTerms(string MessageEnglish, string MessageChinese) //Add : Pinali : 11-09-2019
        {
            Ope.ClearParams();
            string Str = "Update MST_Setting With(RowLock) Set SETTINGVALUE= N'" + MessageEnglish + "' , SETTINGVALUECHINESE = N'" + MessageChinese + "' Where SETTINGKEY = 'OURTERMS'";
            return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);

        }
        public DataTable GetMessageFromSettingTable(string pStrSettingKey) //Add : Pinali : 21-09-2019
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            string Str = "Select SETTINGVALUE,SETTINGVALUECHINESE FROM MST_Setting With(Nolock) Where (UPPER(SETTINGKEY) = '" + pStrSettingKey + "')";
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, Str, CommandType.Text);
            return DTab;
        }
        public int SaveMessageInSettingTable(string MessageEnglish, string MessageChinese,string pStrSettingKey) //Add : Pinali : 21-09-2019
        {
            Ope.ClearParams();
            string Str = "Update MST_Setting With(RowLock) Set SETTINGVALUE= N'" + MessageEnglish + "' , SETTINGVALUECHINESE = N'" + MessageChinese + "' Where SETTINGKEY = '" + pStrSettingKey + "'";
            return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);

        }
        public string GetSuratConnetion() //#Dhara
        {
            Ope.ClearParams();
            string Str = "SELECT SETTINGVALUE FROM MST_SETTING WITH(NOLOCK) Where SETTINGKEY = 'SURATCONNECTION'";
            return Ope.ExeScal(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);
        }
        public string GetTCSAmount() //#Khushbu 20-05-21
        {
            Ope.ClearParams();
            string Str = "SELECT SETTINGVALUE FROM MST_SETTING WITH(NOLOCK) Where SETTINGKEY = 'TCSAMOUNT'";
            return Ope.ExeScal(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);
        }

        public string GetTDSAmount() //#shiv 01-07-2022
        {
            Ope.ClearParams();
            string Str = "SELECT SETTINGVALUE FROM MST_SETTING WITH(NOLOCK) Where SETTINGKEY = 'TDSONSALE'";
            return Ope.ExeScal(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);
        }
        public DataTable GetOrderConfNotificationData()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            //Ope.AddParams("DEPARTMENT_ID", Config.gEmployeeProperty.DEPARTMENT_ID, DbType.Int32, ParameterDirection.Input);
            //Ope.AddParams("EMPLOYEE_ID", Config.gEmployeeProperty.LEDGER_ID, DbType.Int64, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_SingleNotificationOrderConfirm", CommandType.StoredProcedure);
            return DTab;
        }

        public int UpdateNotificationOrderConfirm(string pStrMemoIDs)
        {
            string Str = @"UPDATE TRN_Memo With(ROWLOCK) SET IsNotificationView = 1, UpdateDate = GETDATE(), 
            UpdateBy = '" + Config.gEmployeeProperty.LEDGER_ID + "', UpdateIP = '" + Config.ComputerIP + "' WHERE Memo_ID IN ('" + pStrMemoIDs + "')";
            return Ope.ExeNonQuery(Config.ConnectionString, Config.ProviderName, Str, CommandType.Text);
        }


        static readonly string PasswordHash = "";
        static readonly string SaltKey = "AxoneInfotech";
        static readonly string VIKey = "AxoneRajVakadiya";

        public static string TextDecrypt(string encryptedText)
        {
            try
            {
                byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
                byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
                var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

                var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
                var memoryStream = new MemoryStream(cipherTextBytes);
                var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                memoryStream.Close();
                cryptoStream.Close();
                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public DataTable GetDataSingleNotificationOrderSend()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_SingleNotificationOrderReceive", CommandType.StoredProcedure);
            return DTab;
        }

        public DataTable GetDataHKNotificationTermsWise()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.AddParams("FINYEARNAME", Config.FINYEARSHORTNAME, DbType.String, ParameterDirection.Input);
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_NotificationHKSaleEntryTerms", CommandType.StoredProcedure);
            return DTab;
        }

        //Added by Daksha on 16/01/2023
        public DataTable GetOrderConfNotificationData_ForStockiest()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();           
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_GetDataSingleNotificationOrder", CommandType.StoredProcedure);
            return DTab;
        }
        //End as Daksha

        //Added by Daksha on 17/01/2023
        public int Update_IsStockiest_View_Notification(string pStrMemoIDs)
        {            
            try
            {
                Ope.AddParams("Memo_ID", pStrMemoIDs, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("RetVal", 0, DbType.Int32, ParameterDirection.Output);
                ArrayList AL= Ope.ExeNonQueryWithOutParameter(Config.ConnectionString, Config.ProviderName, "TRN_Memo_IsStockiest_View_UpdateFlag", CommandType.StoredProcedure);
                return Val.ToInt32(AL[0]);
            }
            catch (Exception)
            {
                throw;
            }            
        }
        //End as Daksha

        public DataTable GetDataSingleNotificationOrder()
        {
            Ope.ClearParams();
            DataTable DTab = new DataTable();
            Ope.FillDTab(Config.ConnectionString, Config.ProviderName, DTab, "Trn_SingleNotificationOrderReceive", CommandType.StoredProcedure);
            return DTab;
        }
    }
}
