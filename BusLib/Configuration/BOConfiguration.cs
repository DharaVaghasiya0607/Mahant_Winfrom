using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AxonDataLib;
using BusLib.TableName;
using System.IO;
using System.Data;
using SDK_SC_RFID_Devices;
using System.Runtime.InteropServices;
namespace BusLib.Configuration
{
    public class BOConfiguration
    {
        [DllImport("rpcrt4.dll", SetLastError = true)]
        static extern int UuidCreateSequential(out Guid guid);

        static AxonDataLib.BOSQLHelper Ope = new AxonDataLib.BOSQLHelper();
        static AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public static string ConnectionString = string.Empty;
        public static string Surat_ConnectionString = string.Empty;//Gunjan:29/08/2023
        public static string Surat_ProviderName = string.Empty;
        public static string ComputerMACID = string.Empty;
        public static string gStrLoginSection = string.Empty;
        public static string ProviderName = string.Empty;

        public static string ConnectionFileName = string.Empty;

        public static string ComputerIP = string.Empty;

        public static string FINYEARSHORTNAME { get; set; }
        public static string FINYEARNAME { get; set; }
        public static int FINYEAR_ID { get; set; }


        public static string COMPANYCODE { get; set; }
        public static string COMPANYNAME { get; set; }
        public static Guid COMPANY_ID { get; set; }

        public static RFID_Device RFIDCurrDevice;

        public static LedgerMasterProperty gEmployeeProperty = new LedgerMasterProperty();
        public static string DEPTNAME { get; set; }

        public static string ENCODE_DECODE(string pStr, string pStrToEncodeOrDecode)
        {
            try
            {
                int IntPos;
                string StrPass;
                string StrECode;
                string StrDCode;
                char ChrSingle;

                StrECode = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
                StrDCode = ")(*&^%$#@!";

                for (int IntLen = 1; IntLen <= 60; IntLen++)
                {
                    StrDCode = StrDCode + (Char)(IntLen + 160);
                }
                try
                {

                    StrPass = "";
                    for (int IntCnt = 0; IntCnt <= pStr.Trim().Length - 1; IntCnt++)
                    {
                        ChrSingle = char.Parse(pStr.Substring(IntCnt, 1));
                        if (pStrToEncodeOrDecode == "E")
                        {
                            IntPos = StrECode.IndexOf(ChrSingle, 0);
                        }
                        else
                        {
                            IntPos = StrDCode.IndexOf(ChrSingle, 0);
                        }
                        if (pStrToEncodeOrDecode == "E")
                        {
                            StrPass = StrPass + StrDCode.Substring(IntPos, 1);
                        }
                        else
                        {
                            StrPass = StrPass + StrECode.Substring(IntPos, 1);
                        }
                    }
                }
                catch (Exception EX)
                {
                    throw;
                }

                return StrPass;
            }
            catch (Exception EX)
            {
                return "";
            }
            
        }


        public static void BackUp()
        {
            try
            {
                string StrDirectory = System.Configuration.ConfigurationManager.AppSettings["BackupPath"].ToString();

                if (System.IO.Directory.Exists(StrDirectory) == false)
                {
                    System.IO.Directory.CreateDirectory(StrDirectory);
                }

                (from f in new DirectoryInfo(StrDirectory).GetFiles()
                 where f.CreationTime < DateTime.Now.Subtract(TimeSpan.FromDays(30))
                 select f
            ).ToList()
               .ForEach(f => f.Delete());

                string DatabaseName = System.Configuration.ConfigurationManager.AppSettings["DBName"].ToString();
                string BackUpName = DatabaseName + DateTime.Now.ToString("yyyyMMddhhmmss") + ".bak";

                string StrSql = "BACKUP DATABASE [" + DatabaseName + "] TO  DISK = N'";
                StrSql = StrSql + StrDirectory + BackUpName + "'";
                StrSql = StrSql + " WITH NOFORMAT, NOINIT,  NAME = N'" + DatabaseName + "-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";

                Ope.ClearParams();
                Ope.ExeNonQuery(ConnectionString, ProviderName, StrSql, CommandType.Text);
            }
            catch (Exception ex)
            {

            }
        }

        public static Guid FindNewSequentialID()
        {
            Guid guid;
            UuidCreateSequential(out guid);
            var s = guid.ToByteArray();
            var t = new byte[16];

            t[3] = s[0];
            t[2] = s[1];
            t[1] = s[2];
            t[0] = s[3];

            t[5] = s[4];
            t[4] = s[5];
            t[7] = s[6];
            t[6] = s[7];
            t[8] = s[8];
            t[9] = s[9];
            t[10] = s[10];
            t[11] = s[11];
            t[12] = s[12];
            t[13] = s[13];
            t[14] = s[14];
            t[15] = s[15];

            return new Guid(t);
        }

    }
}
