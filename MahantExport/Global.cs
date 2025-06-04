using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

using SProc = BusLib.TPV.BOSProc;
using TabName = BusLib.TPV.BOTableName;
using Config = BusLib.Configuration.BOConfiguration;
using MahantExport.MDI;
using System.Net;
using System.Net.Sockets;
using MahantExport.Utility;
using DevExpress.LookAndFeel;
using System.Data;
using BusLib;
using BusLib.Configuration;
using System.IO;
using Microsoft.VisualBasic;
using System.Text;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Data.OleDb;
using System.Xml;
using System.Xml.Serialization;
using MahantExport.GIADownload;
using BusLib.Master;
using System.Data.SqlClient;
using DevExpress.XtraGrid.Columns;
using System.Drawing;
using DevExpress.Utils.Drawing;
using DevExpress.Data;
using OfficeOpenXml;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid;
using System.Net.NetworkInformation;
using System.Management;

namespace MahantExport
{
    static class Global
    {
        public enum LANGUAGE
        {
            GUJARATI = 0,
            ENGLISH = 1
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>

        static readonly string PasswordHash = "";
        static readonly string SaltKey = "AxoneInfotech";
        static readonly string VIKey = "AxoneRajVakadiya";

        public static string gStrMessage = string.Empty;
        public static Form gMainRef = null;
        public static Form gMainLiveStock = null;

        public static List<InputLanguage> AL = new List<InputLanguage>();
        public static string gStrRegisterKey = string.Empty;
        public static string gStrExePath = string.Empty;
        public static string gStrExeVersion = string.Empty;
        public static string gStrCompanyName = string.Empty;
        public static string gStrSuvichar = string.Empty;
        public static string gstrEventUrl = string.Empty;
        public static string ComputerMACID = string.Empty;

        public static string FILTERSHAPE_ID = string.Empty;
        public static string FILTERSIZE_ID = string.Empty;
        public static string FILTERCOLOR_ID = string.Empty;
        public static string FILTERCLARITY_ID = string.Empty;
        public static string FILTERCUT_ID = string.Empty;
        public static string FILTERPOL_ID = string.Empty;
        public static string FILTERSYM_ID = string.Empty;
        public static string FILTERFL_ID = string.Empty;
        public static string FILTERCOLORSHADE_ID = string.Empty;
        public static string FILTERMILKY_ID = string.Empty;
        public static string FILTERLAB_ID = string.Empty;
        public static string FILTERWEBSTATUS = string.Empty;
        public static string FILTERTABLEBLACKINC_ID = string.Empty;
        public static string FILTERSIDEBLACKINC_ID = string.Empty;

        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-GB");

                Config.ConnectionFileName = "conn1";  //192.168.0.243
                Config.ConnectionString = System.IO.File.ReadAllText(Application.StartupPath + "\\conn1.txt");
                Config.ConnectionString = TextDecrypt(Config.ConnectionString);
                Config.ProviderName = "System.Data.SqlClient";
                //Config.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                //Config.ProviderName = System.Configuration.ConfigurationManager.ConnectionStrings["ConnectionString"].ProviderName;

                Config.Surat_ConnectionString = System.IO.File.ReadAllText(Application.StartupPath + "\\Surat_conn1.txt");//Gunjan:29/08/2023
                Config.Surat_ConnectionString = TextDecrypt(Config.Surat_ConnectionString);
                Config.Surat_ProviderName = "System.Data.SqlClient";//Ens as Gunjan

                gStrCompanyName = System.Configuration.ConfigurationManager.AppSettings["CompanyName"].ToString();

                gStrRegisterKey = System.Configuration.ConfigurationManager.AppSettings["SecurityKey"].ToString();

                gStrExePath = System.Configuration.ConfigurationManager.AppSettings["ExeUpdatePath"].ToString();

                Config.ComputerMACID = GetProcessorID();
                if (System.Configuration.ConfigurationManager.AppSettings["EmailAddress"].ToString() != "AxoneInfotech")
                {
                    Licence.Activation Activation = new Licence.Activation();
                    Activation.BoardID = Config.ComputerMACID;
                    Activation.ShowDialog();
                    Application.Exit();
                }

                else if (System.Configuration.ConfigurationManager.AppSettings["EmailAddress"].ToString() == "AxoneInfotech")
                {
                    Licence.Activation Activation = new Licence.Activation();
                    Activation.BoardID = Config.ComputerMACID;
                    if (Activation.CheckActivation() == false)
                    {
                        Activation.ShowDialog();
                        Activation.Dispose();
                        Activation = null;
                        Application.Exit();
                        return;
                    }
                    Activation.Dispose();
                    Activation = null;
                }
                else
                {
                    Message("NO Any Activation Key Found , Please Contact To Administration");
                    return;
                }

                string strHostName = "";
                strHostName = System.Net.Dns.GetHostName();

                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                foreach (IPAddress a in localIPs)
                {
                    if (a.AddressFamily == AddressFamily.InterNetwork)
                    {
                        strHostName = a.ToString();
                    }
                }
                if (localIPs.Length == 0)
                {
                    Config.ComputerIP = System.Environment.MachineName.ToString();
                }
                else
                {
                    Config.ComputerIP = strHostName;
                }

                DevExpress.Skins.SkinManager.EnableFormSkins();
                DevExpress.UserSkins.BonusSkins.Register();
               
                UserLookAndFeel.Default.SetSkinStyle(SkinStyle.Office2019Colorful, "Default");
                //UserLookAndFeel.Default.SetSkinStyle("Blue");

                foreach (InputLanguage ilItem in InputLanguage.InstalledInputLanguages)
                {
                    //Add all installed input languages on system to List<>
                    AL.Add(ilItem);
                }

                FrmLogin FrmLogin = new Utility.FrmLogin();
                FrmLogin.ShowForm();

                //Application.Run(new FrmLogin());
            }
            catch (Exception ex)
            {
                Message(ex.Message);
            }


        }
        public static string GetProcessorID()
        {
            StringBuilder computerID = new StringBuilder();

            // Mac ID
            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    computerID.Append(nic.GetPhysicalAddress().ToString());
                    break;
                }
            }

            ManagementObjectSearcher searcher;

            searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Processor");
            foreach (ManagementObject queryObj in searcher.Get())
            {
                computerID.Append("-");
                computerID.Append(queryObj["ProcessorId"]);
            }


            return computerID.ToString();
        }
        private static bool IsServerConnected(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }

        public static void SprirelGetSheetNameFromExcel(AxonContLib.cComboBox pCombo, string path)
        {
            try
            {
                using (var pck = new OfficeOpenXml.ExcelPackage())
                {
                    using (var stream = File.OpenRead(path))
                    {
                        pck.Load(stream);
                    }
                    var ws = pck.Workbook.Worksheets;
                    pCombo.Items.Clear();
                    for (int rowNum = 1; rowNum <= ws.Count; rowNum++)
                    {
                        pCombo.Items.Add(ws[rowNum].Name);
                    }
                }
            }
            catch (Exception EX)
            {
                MessageError(EX.Message);
            }

        }

        public static DataTable GetSelectedRecordOfGrid(GridView view, Boolean IsSelect, BODevGridSelection pSelection)
        {
            if (view.RowCount <= 0)
            {
                return null;
            }
            ArrayList aryLst = new ArrayList();
            DataTable resultTable = new DataTable();
            DataTable sourceTable = ((DataView)view.DataSource).Table;

            if (IsSelect)
            {
                aryLst = pSelection.GetSelectedArrayList();
                resultTable = sourceTable.Clone();
                for (int i = 0; i < aryLst.Count; i++)
                {
                    DataRowView oDataRowView = aryLst[i] as DataRowView;
                    resultTable.Rows.Add(oDataRowView.Row.ItemArray);
                }
            }
            return resultTable;
        }

        public static DataTable SprireGetDataTableFromExcel(string path, string StrSheetName, bool hasHeader = true)
        {
            try
            {
                using (var pck = new OfficeOpenXml.ExcelPackage())
                {
                    using (var stream = File.OpenRead(path))
                    {
                        pck.Load(stream);
                    }
                    var ws = pck.Workbook.Worksheets[StrSheetName];
                    DataTable tbl = new DataTable();
                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                    {
                        //tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                        tbl.Columns.Add(firstRowCell.Text);
                    }
                    var startRow = hasHeader ? 2 : 1;
                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                    {
                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                        DataRow row = tbl.Rows.Add();
                        foreach (var cell in wsRow)
                        {
                            if ((cell.Start.Column - 1) < tbl.Columns.Count)
                            {
                                row[cell.Start.Column - 1] = cell.Text;
                            }
                        }
                    }
                    return tbl;
                }
            }
            catch (Exception EX)
            {
                MessageError(EX.Message);
                return null;
            }
        }

        public static double GetLiveExchangeRate(string CurrencyCode)
        {
            AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

            double DouExchangeRate = 0;
            try
            {

                string StrAPIKey = new BusLib.Transaction.BOTRN_MemoEntry().GetAPIKeyForExchangeRate();

                string apiURL = "https://free.currconv.com/api/v7/convert?q=USD_" + CurrencyCode + "&compact=ultra&apiKey=" + StrAPIKey + "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiURL);
                string html = "";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    html = reader.ReadToEnd();
                }
                string[] Split = html.Replace("{", "").Replace("}", "").Split(':');
                DouExchangeRate = Val.Val(Split[1]);
            }
            catch (Exception EX)
            {
                Global.Message(EX.ToString());
            }
            return DouExchangeRate;
        }

        /*
        public static DataTable GetCertData(string LabColumn, string WeightColumn, string LabReportno, DataTable dt)
        {
            try
            {
                DataTable dtColumns = new GeneralCls().GetDataCustom("SELECT ColumnName,a.items,ISNULL(FieldName,'') FieldName FROM MST_LabStone_ColumnMapping WITH(NOLOCK) CROSS APPLY dbo.Split(MappedColumn,',') AS a");
                DataTable table = new DataTable();
                foreach (DataRow drow in dtColumns.Rows)
                {
                    if (!table.Columns.Contains(drow["ColumnName"].ToString()))
                    {
                        table.Columns.Add(drow["ColumnName"].ToString());
                    }
                }
                if (LabColumn.ToUpper() == "GIA")
                {
                    ReportCheckWSClient RF1 = new ReportCheckWSClient();
                    DataTable dt1 = new TRN_LabReult_Det().GetGiaReportNoForPDF(LabReportno, WeightColumn);

                    
                    TableCert = new DataTable();
                    GenerateDataTableForResult();
                    if (dt1.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            string Url = @"<?xml version=1.0 encoding=UTF8?><REPORT_CHECK_REQUEST><HEADER><IP_ADDRESS>103.54.98.210</IP_ADDRESS></HEADER><BODY><REPORT_DTLS><REPORT_DTL><REPORT_NO>6222397432</REPORT_NO><REPORT_WEIGHT>1.00</REPORT_WEIGHT></REPORT_DTL></REPORT_DTLS></BODY></REPORT_CHECK_REQUEST>";
                            string str = RF1.processRequest(Url);

                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://igi.org/searchreport_postreq.php?r=" + LabReportno.ToString() + "&_=1");
                            string html = "";
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                html = reader.ReadToEnd();
                            }

                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://my.hrdantwerp.com/?id=34&record_number=" + LabReportno.ToString());
                            string html = "";
                            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                            {
                                html = reader.ReadToEnd();
                            }


                            if (str.Trim().Length > 0)
                            {
                                if (str.Contains("SUCCESS"))
                                {
                                    XmlDocument xmldoc = new XmlDocument();
                                    xmldoc.LoadXml(str);

                                    DataRow Drow = TableCert.NewRow();

                                    Drow["Job No"] = "";
                                    Drow["Control No"] = "";
                                    Drow["Diamond Dossier"] = "";
                                    Drow["LabReport"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/REPORT_NO").InnerText;
                                    Drow["ReportDate"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/REPORT_DT").InnerText;
                                    Drow["Client Ref"] = "";
                                    Drow["Memo No"] = "";
                                    Drow["shape"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/SHAPE").InnerText;
                                    Drow["DiameterMin"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/LENGTH").InnerText;
                                    Drow["DiameterMax"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/WIDTH").InnerText;
                                    Drow["totaldepth"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/DEPTH").InnerText.ToString().Replace("%", "");
                                    Drow["totaldepthper"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/DEPTH_PCT").InnerText.Replace("%", "") == "" ? "0" : xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/DEPTH_PCT").InnerText.Replace("%", "");
                                    Drow["weightincarats"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/WEIGHT").InnerText;
                                    Drow["Color"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/COLOR").InnerText.Replace("*", "");
                                    Drow["ColorDescription"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/COLOR_DESCRIPTIONS").InnerText;
                                    Drow["clarity"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/CLARITY").InnerText;
                                    Drow["Clarity Status"] = "";
                                    Drow["cut"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/FINAL_CUT").InnerText;
                                    Drow["polish"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/POLISH").InnerText;
                                    Drow["symm"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/SYMMETRY").InnerText;
                                    Drow["flr"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/FLUORESCENCE_INTENSITY").InnerText;
                                    Drow["FLRColor"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/FLUORESCENCE_COLOR").InnerText;
                                    Drow["GirdleName"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/GIRDLE").InnerText;
                                    Drow["GirdleCondition"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/GIRDLE_CONDITION").InnerText;
                                    Drow["culetcondition"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/CULET_SIZE").InnerText.ToString().Replace("%", "");
                                    Drow["TableDiameterper"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/TABLE_PCT").InnerText.ToString().Replace("%", "");
                                    Drow["CrownAngle"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/CRN_AG").InnerText.ToString().Replace("°", "");
                                    Drow["CrownHeight"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/CRN_HT").InnerText.ToString().Replace("%", "");
                                    Drow["PavillionAngle"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/PAV_AG").InnerText.ToString().Replace("°", "");
                                    Drow["PavillionHeight"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/PAV_DP").InnerText.ToString().Replace("%", "");
                                    Drow["Star"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/STR_LN").InnerText.ToString().Replace("°", "").Replace("%", "");
                                    Drow["LH"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/LR_HALF").InnerText.ToString().Replace("°", "");
                                    Drow["Painting"] = "";
                                    Drow["Proportion"] = "";
                                    Drow["Paint Comm"] = "";
                                    Drow["keytosymbols"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/KEY_TO_SYMBOLS").InnerText;
                                    Drow["reportcomment"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/REPORT_COMMENTS").InnerText;
                                    Drow["LaserInscription"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/INSCRIPTION").InnerText;
                                    Drow["Synthetic Indicator"] = "";
                                    Drow["GirdlePer"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/GIRDLE_PCT").InnerText.ToString().Replace("%", "");
                                    Drow["Polish Features"] = "";
                                    Drow["Symmetry Features"] = "";
                                    Drow["ShapeDescription"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/FULL_SHAPE_DESCRIPTION").InnerText;
                                    Drow["ReportType"] = xmldoc.SelectSingleNode("REPORT_CHECK_RESPONSE/REPORT_DTLS/REPORT_DTL/REPORT_TYPE").InnerText;
                                    Drow["Sorting"] = "";
                                    Drow["Basket Status"] = "";
                                    Drow["Department"] = "";


                                    TableCert.Rows.Add(Drow);
                                    return TableCert;
                                }

                            }
                        }
                    }
                }
                else if (LabColumn.ToUpper() == "HRD")
                {
                    string col = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://my.hrdantwerp.com/?id=34&record_number=" + LabReportno.ToString());
                        string html = "";
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            html = reader.ReadToEnd();
                        }

                        DataTable dtResult = ConvertHTMLTablesToDataTable(html);
                        DataRow trow = table.NewRow();


                        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                        doc.LoadHtml(html);
                        var headers = doc.DocumentNode.SelectNodes("//strong");

                        foreach (HtmlAgilityPack.HtmlNode header in headers)
                        {
                            if (header.InnerHtml.Contains("/"))
                            {
                                dr["ReportDate"] = header.InnerHtml.ToString();
                            }
                        }
                        if (dtResult.Rows.Count > 0)
                        {
                            dtResult.Columns[0].ColumnName = "Result";
                            foreach (DataRow drResult in dtResult.Rows)
                            {
                                if (drResult.ItemArray[0].ToString().ToUpper() == "SUM &ALPHA; &AMP; &BETA;:")
                                {
                                    continue;
                                }
                                DataRow[] drSelect = dtColumns.Select("items='" + drResult[0].ToString().Replace(":", "").Replace("(β)", "").Replace("(α)", "").Trim() + "'");
                                DataRow[] drValue = dtResult.Select("Result='" + drResult[0].ToString() + "'");
                                if (drSelect.Length > 0)
                                {
                                    col = drSelect[0].ItemArray[2].ToString();

                                    if (col.ToUpper() == "SHAPE")
                                    {
                                        dr["Shape"] = ReplaceString(drValue[0].ItemArray[1].ToString().Replace("ct", "").Replace("deg)", "").Trim());
                                    }
                                    else if (col.ToUpper() == "WEIGHTINCARATS")
                                    {
                                        dr["weightincarats"] = ReplaceString(drValue[0].ItemArray[1].ToString().Replace("ct", "").Replace("deg)", "").Trim());
                                    }
                                    else if (col.ToUpper() == "COLOR")
                                    {
                                        dr["Color"] = ReplaceString(drValue[0].ItemArray[1].ToString().Replace("ct", "").Replace("deg)", "").Trim());
                                    }
                                    else if (col.ToUpper() == "CLARITY")
                                    {
                                        dr["Clarity"] = ReplaceString(drValue[0].ItemArray[1].ToString().Replace("ct", "").Replace("deg)", "").Trim());
                                    }
                                    else if (col.ToUpper() == "CUT")
                                    {
                                        dr["Cut"] = ReplaceString(drValue[0].ItemArray[1].ToString().Replace("ct", "").Replace("deg)", "").Trim());
                                    }
                                    else if (col.ToUpper() == "POLISH")
                                    {
                                        dr["Polish"] = ReplaceString(drValue[0].ItemArray[1].ToString().Replace("ct", "").Replace("deg)", "").Trim());
                                    }
                                    else if (col.ToUpper() == "SYMM")
                                    {
                                        dr["symm"] = ReplaceString(drValue[0].ItemArray[1].ToString().Replace("ct", "").Replace("deg)", "").Trim());
                                    }
                                    else if (col.ToUpper() == "FLR")
                                    {
                                        dr["flr"] = ReplaceString(drValue[0].ItemArray[1].ToString().Replace("ct", "").Replace("deg)", "").Trim());
                                    }
                                    else if (drResult[0].ToString().Replace(":", "").Replace("(β)", "").Replace("(α)", "").Trim().ToUpper() == "MEASUREMENTS")
                                    {
                                        dr["DiameterMin"] = drValue[0].ItemArray[1].ToString().Replace("mm", "").Replace("x", "-").Split('-')[0].Trim();
                                        dr["DiameterMax"] = drValue[0].ItemArray[1].ToString().Replace("mm", "").Replace("x", "-").Split('-')[1].Trim();
                                        dr["TotalDepth"] = drValue[0].ItemArray[1].ToString().Replace("mm", "").Replace("x", "-").Split('-')[2].Trim();
                                    }
                                    else if (drResult[0].ToString().Replace(":", "").Replace("(β)", "").Replace("(α)", "").Trim().ToUpper() == "GIRDLE")
                                    {
                                        string[] str = drValue[0].ItemArray[1].ToString().Replace("mm", "").Replace("x", "-").Split(' ');
                                        dr["GirdleName"] = str[0].Trim();
                                        dr["GirdleCondition"] = str[str.Length - 1].Trim();
                                        dr["Girdle"] = str[1].Trim().Replace("%", "");
                                    }
                                    else if (col.ToUpper() == "CULETSIZE")
                                    {
                                        string[] str = drValue[0].ItemArray[1].ToString().Replace("%", "").Split(' ');
                                        dr["culetsize"] = str[str.Length - 1].Trim();
                                    }
                                    else if (col.ToUpper() == "TOTALDEPTHPER")
                                    {
                                        dr["totaldepth"] = ReplaceString(drValue[0].ItemArray[1].ToString().Replace("ct", "").Replace("deg)", "").Replace("%", "").Trim());
                                    }
                                    else if (col.ToUpper() == "TABLEDIAMETERPER")
                                    {
                                        dr["tableDiameterper"] = ReplaceString(drValue[0].ItemArray[1].ToString().Replace("ct", "").Replace("deg)", "").Replace("%", "").Trim());
                                    }
                                    else if (drResult[0].ToString().Replace(":", "").Replace("(β)", "").Replace("(α)", "").Trim().ToUpper() == "CROWN HEIGHT")
                                    {
                                        string[] str = drValue[0].ItemArray[1].ToString().Replace("(", "-").Replace("deg)", "").Replace("%", "").Split('-');
                                        dr["CrownHeight"] = str[0].Trim();
                                        if (str.Length > 1)
                                        {
                                            dr["CrownAngle"] = str[1].Trim();
                                        }
                                    }
                                    else if (drResult[0].ToString().Replace(":", "").Replace("(β)", "").Replace("(α)", "").Trim().ToUpper() == "PAVILION DEPTH")
                                    {
                                        string[] str = drValue[0].ItemArray[1].ToString().Replace("(", "-").Replace("deg)", "").Replace("%", "").Split('-');
                                        dr["PavillionHeight"] = str[0].Trim();
                                        if (str.Length > 1)
                                        {
                                            dr["PavillionAngle"] = str[1].Trim();
                                        }
                                    }
                                    else if (col.ToUpper() == "LH")
                                    {
                                        dr["LH"] = ReplaceString(drValue[0].ItemArray[1].ToString().Replace("ct", "").Replace("deg)", "").Replace("%", "").Trim());
                                    }
                                }
                            }
                        }
                    }
                }
                else if (LabColumn.ToUpper() == "IGI")
                {
                    string col = "";
                    foreach (DataRow dr in dt.Rows)
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://igi.org/searchreport_postreq.php?r=" + LabReportno.ToString() + "&_=1");
                        string html = "";
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                        {
                            html = reader.ReadToEnd();
                        }
                        DataTable dtResult = ConvertHTMLTablesToDataTable(html);
                        if (dtResult.Rows.Count > 0)
                        {
                            dtResult.Columns[0].ColumnName = "Result";
                            foreach (DataRow drResult in dtResult.Rows)
                            {
                                DataRow[] drSelect = dtColumns.Select("items='" + drResult[0].ToString().Replace(":", "").Replace("(β)", "").Replace("(α)", "").Trim() + "'");
                                DataRow[] drValue = dtResult.Select("Result='" + drResult[0].ToString() + "'");
                                if (drSelect.Length > 0)
                                {
                                    col = drSelect[0].ItemArray[2].ToString();
                                    string col1 = drSelect[0].ItemArray[1].ToString();
                                    string value = ReplaceString(drValue[0].ItemArray[1].ToString().Replace("ct", "").Replace("deg)", "").Replace("Carat", "").Replace("s", "").Trim());
                                    string value1 = ReplaceString(drValue[0].ItemArray[1].ToString().Replace("deg)", "").Replace("Carat", "").Replace("s", "").Trim());
                                    if (col.ToUpper() == "LABREPORT")
                                    {
                                        dr["LabReport"] = value;
                                    }
                                    if (col.ToUpper() == "REPORTDATE")
                                    {
                                        string[] str = value1.Replace(",", " ").Split(' ');
                                        string reportDt = "";
                                        for (int i = 1; i < str.Length; i++)
                                        {
                                            if (reportDt == "")
                                                reportDt = str[i];
                                            else
                                            {
                                                reportDt = reportDt + "/" + str[i];
                                                reportDt = reportDt.TrimEnd('/');
                                            }
                                        }
                                        reportDt = Convert.ToDateTime(reportDt).ToShortDateString();
                                        dr["ReportDate"] = reportDt;
                                    }
                                    if (col.ToUpper() == "SHAPE")
                                    {
                                        dr["Shape"] = value;
                                    }
                                    else if (col.ToUpper() == "WEIGHTINCARATS")
                                    {
                                        dr["weightincarats"] = value;
                                    }
                                    else if (col.ToUpper() == "COLOR")
                                    {
                                        dr["Color"] = value;
                                    }
                                    else if (col.ToUpper() == "CLARITY")
                                    {
                                        dr["Clarity"] = value;
                                    }
                                    else if (col.ToUpper() == "CUT")
                                    {
                                        dr["Cut"] = value;
                                    }
                                    else if (col.ToUpper() == "POLISH")
                                    {
                                        dr["Polish"] = value;
                                        if (col1.ToUpper() == "POLISH - SYMMETRY" || col1.ToUpper() == "POLISH AND SYMMETRY")
                                        {
                                            dr["symm"] = value;
                                        }
                                    }
                                    else if (col.ToUpper() == "SYMM")
                                    {
                                        dr["symm"] = value;
                                    }
                                    else if (col.ToUpper() == "FLR")
                                    {
                                        dr["flr"] = value;
                                    }
                                    else if (drResult[0].ToString().Replace(":", "").Replace("(β)", "").Replace("(α)", "").Trim().ToUpper() == "MEASUREMENTS")
                                    {
                                        string[] str = drValue[0].ItemArray[1].ToString().Replace("mm", "").Replace("x", "-").Split('-');

                                        dr["DiameterMin"] = str[0].Trim();
                                        dr["DiameterMax"] = str[1].Trim();
                                        dr["TotalDepth"] = str[2].Trim();
                                    }
                                    else if (col.ToUpper() == "TABLEDIAMETERPER")
                                    {
                                        dr["tableDiameterper"] = value;
                                    }
                                    else if (col.ToUpper() == "CROWNHEIGHT")
                                    {
                                        string[] str = value.Replace("°", "").Split('-');

                                        dr["CrownHeight"] = str[0].Trim();
                                        if (str.Length > 1)
                                        {
                                            dr["CrownAngle"] = str[1].Trim();
                                        }
                                    }
                                    else if (col.ToUpper() == "PAVILLIONANGLE" || col.ToUpper() == "PAVILLIONHEIGHT")
                                    {
                                        string[] str = value.Replace("°", "").Split('-');
                                        dr["PavillionHeight"] = str[0].Trim();
                                        if (str.Length > 1)
                                        {
                                            dr["PavillionAngle"] = str[1].Trim();
                                        }
                                    }
                                    else if (col.ToUpper() == "GIRDLENAME")
                                    {
                                        string[] str = drValue[0].ItemArray[1].ToString().Replace("mm", "").Replace("x", "-").Split('(');
                                        dr["GirdleName"] = str[0].Trim();
                                        if (str.Length > 1)
                                        {
                                            dr["GirdleCondition"] = str[1].Replace(")", "").Trim();
                                        }
                                    }
                                    else if (col.ToUpper() == "CULETSIZE")
                                    {
                                        dr["culetsize"] = value;
                                    }
                                    else if (col.ToUpper() == "TOTALDEPTHPER")
                                    {
                                        dr["totaldepth"] = value;
                                    }
                                    else if (col.ToUpper() == "LASERINSCRIPTION")
                                    {
                                        dr["LaserInscription"] = value;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string url = URLCheck;
                        url = url.Replace("<lab>", LabColumn.ToString()).Replace("<wt>", WeightColumn.ToString()).Replace("<reportno>", LabReportno.ToString());

                        string htmlCode = "";
                        using (WebClient client = new WebClient())
                        {
                            client.Headers.Add(HttpRequestHeader.UserAgent, "AvoidError");
                            htmlCode = client.DownloadString(url);
                        }

                        HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                        doc.LoadHtml(htmlCode);
                        var headers = doc.DocumentNode.SelectNodes("//td");

                        string col = "";
                        int row = 0, headCount = headers.Count - 1, index = 0;
                        DataRow trow = table.NewRow();
                        trow["Lab"] = LabColumn.ToString();
                        trow["ReportNo"] = LabReportno.ToString();
                        trow["Weight"] = WeightColumn.ToString();
                        string excelColumn = "";
                        foreach (HtmlAgilityPack.HtmlNode header in headers)
                        {
                            if (header.InnerHtml.Contains("FieldName"))
                            {
                                DataRow[] drSelect = dtColumns.Select("items='" + header.InnerText.ToString().Replace(":", "").Trim() + "'");
                                col = drSelect[0].ItemArray[0].ToString();
                                //DataRow[] drExcel = dtColumns.Select("FieldName='" + drSelect[0].ItemArray[2].ToString() + "'");
                                if (drSelect.Length > 0)
                                {
                                    excelColumn = drSelect[0].ItemArray[2].ToString();
                                }
                                else
                                {
                                    excelColumn = "";
                                }

                            }
                            if (header.InnerHtml.Contains("FieldValue"))
                            {
                                if (col == "CrHeight" && LabColumn.ToString() != "GIA")
                                {
                                    trow[col] = ReplaceString(header.InnerText.ToString().Replace("(", "-").Replace("deg)", "").Split('-')[0]);
                                    if (excelColumn != "")
                                    {
                                        dr[excelColumn] = trow[col].ToString();
                                    }

                                    if (header.InnerText.ToString().Replace("(", "-").Replace("deg)", "").Split('-').Length > 1)
                                    {
                                        trow["CrAngle"] = ReplaceString(header.InnerText.ToString().Replace("(", "-").Replace("deg)", "").Split('-')[1]);
                                        string crAg = "";

                                        crAg = dtColumns.Select("FieldName='" + dtColumns.Select("ColumnName='CrAngle'")[0].ItemArray[2].ToString() + "'")[0].ItemArray[3].ToString();
                                        if (crAg != "")
                                        {
                                            dr[crAg] = trow["CrAngle"].ToString();
                                        }
                                    }
                                }
                                else if (col == "PvHeight" && LabColumn.ToString() != "GIA")
                                {
                                    trow[col] = ReplaceString(header.InnerText.ToString().Replace("(", "-").Replace("deg)", "").Split('-')[0]);
                                    if (excelColumn != "")
                                    {
                                        dr[excelColumn] = trow[col].ToString();
                                    }

                                    if (header.InnerText.ToString().Replace("(", "-").Replace("deg)", "").Split('-').Length > 1)
                                    {
                                        trow["PvAngle"] = ReplaceString(header.InnerText.ToString().Replace("(", "-").Replace("deg)", "").Split('-')[1]);

                                        string pvAg = "";

                                        pvAg = dtColumns.Select("FieldName='" + dtColumns.Select("ColumnName='PvAngle'")[0].ItemArray[2].ToString() + "'")[0].ItemArray[3].ToString();
                                        if (pvAg != "")
                                        {
                                            dr[pvAg] = trow["PvAngle"].ToString();
                                        }
                                    }
                                }
                                else if (col == "Length" && LabColumn.ToString() != "GIA")
                                {
                                    trow[col] = header.InnerText.ToString().Replace("mm", "").Replace("x", "-").Split('-')[0].Trim();
                                    trow["Width"] = header.InnerText.ToString().Replace("mm", "").Replace("x", "-").Split('-')[1].Trim();
                                    trow["Depth"] = header.InnerText.ToString().Replace("mm", "").Replace("x", "-").Split('-')[2].Trim();

                                    string lengh = "", width = "", height = "";

                                    lengh = dtColumns.Select("FieldName='" + dtColumns.Select("ColumnName='Length'")[0].ItemArray[2].ToString() + "'")[0].ItemArray[3].ToString();
                                    width = dtColumns.Select("FieldName='" + dtColumns.Select("ColumnName='Width'")[0].ItemArray[2].ToString() + "'")[0].ItemArray[3].ToString();
                                    height = dtColumns.Select("FieldName='" + dtColumns.Select("ColumnName='Depth'")[0].ItemArray[2].ToString() + "'")[0].ItemArray[3].ToString();

                                    if (lengh != "")
                                    {
                                        dr[lengh] = trow[col].ToString();
                                    }
                                    if (width != "")
                                    {
                                        dr[width] = trow["Width"].ToString();
                                    }
                                    if (height != "")
                                    {
                                        dr[height] = trow["Depth"].ToString();
                                    }
                                }
                                else
                                {
                                    trow[col] = ReplaceString(header.InnerText.ToString());
                                    if (excelColumn != "")
                                    {
                                        dr[excelColumn] = ReplaceString(header.InnerText.ToString());
                                    }
                                }
                            }
                            if (col == LabColumn)
                            {
                                trow[col] = dr[LabColumn].ToString();
                            }
                            if (headCount == index)
                            {
                                excelColumn = "";
                                table.Rows.Add(trow);
                                trow = table.NewRow();
                            }
                            index++;
                        }
                    }
                    foreach (DataRow dr in dt.Rows)
                    {
                        if (Convert.ToString(dr["Color"]) == "*")
                        {
                            dr["Color"] = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message.ToString(), "GetCertData", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return dt;
        }
         */

        public static void CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column == null) return;

            DevExpress.Utils.Drawing.ObjectInfoArgs filterInfo = null;
            Rectangle filterBounds, sortBounds;
            filterBounds = sortBounds = Rectangle.Empty;
            try
            {
                UpdateInnerElements(e, false, ref sortBounds, ref filterBounds, ref filterInfo);
                e.Painter.DrawObject(e.Info);
            }
            finally
            {
                UpdateInnerElements(e, true, ref sortBounds, ref filterBounds, ref filterInfo);
            }

            if (!sortBounds.IsEmpty)
                DrawCustomSortedShape(e.Graphics, sortBounds, e.Column.SortOrder);
            if (!filterBounds.IsEmpty && filterInfo != null)
                DrawCustomFilterButton(e.Graphics, e.Column, filterInfo);

            e.Handled = true;
        }


        private static void DrawCustomSortedShape(Graphics g, Rectangle r, ColumnSortOrder so)
        {
            if (so == ColumnSortOrder.None) return;
            if (so == ColumnSortOrder.Descending)
            {
                g.DrawImageUnscaled(
               MahantExport.Properties.Resources.Down,
               r.X + (r.Width - MahantExport.Properties.Resources.Down.Size.Width) / 2 - 10,
               r.Y + (r.Height - MahantExport.Properties.Resources.Down.Size.Height) / 2);

            }
            else if (so == ColumnSortOrder.Ascending)
            {
                g.DrawImageUnscaled(
               MahantExport.Properties.Resources.Up,
               r.X + (r.Width - MahantExport.Properties.Resources.Down.Size.Width) / 2 - 10,
               r.Y + (r.Height - MahantExport.Properties.Resources.Down.Size.Height) / 2);
            }

        }

        private static void DrawCustomFilterButton(Graphics g, GridColumn column, DevExpress.Utils.Drawing.ObjectInfoArgs filterInfo)
        {

            int i = 0;
            if ((filterInfo.State & DevExpress.Utils.Drawing.ObjectState.Hot) != 0) i = 1;
            if ((filterInfo.State & DevExpress.Utils.Drawing.ObjectState.Pressed) != 0) i = 2;
            if (column.FilterInfo.Type != ColumnFilterType.None) i += 3;

            int x = filterInfo.Bounds.X + (filterInfo.Bounds.Width - MahantExport.Properties.Resources.Down.Size.Width) / 2 - 5;
            int y = filterInfo.Bounds.Y + (filterInfo.Bounds.Height - MahantExport.Properties.Resources.Down.Size.Height) / 2 + 2;

            if ((filterInfo.State & DevExpress.Utils.Drawing.ObjectState.Hot) != 0 || (filterInfo.State & DevExpress.Utils.Drawing.ObjectState.Normal) != 0)
            {
                g.DrawImageUnscaled(
               MahantExport.Properties.Resources.Filter,
               x,
               y);
            }
            if ((filterInfo.State & DevExpress.Utils.Drawing.ObjectState.Pressed) != 0)
            {
                g.DrawImageUnscaled(
               MahantExport.Properties.Resources.FilterSelection,
               x,
               y);
            }
        }

        private static void UpdateInnerElements(DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e, bool restore, ref Rectangle sortBounds, ref Rectangle filterBounds, ref ObjectInfoArgs filterInfo)
        {
            foreach (DevExpress.Utils.Drawing.DrawElementInfo item in e.Info.InnerElements)
            {
                if (item.ElementPainter is DevExpress.Utils.Drawing.SortedShapeObjectPainter)
                {
                    if (restore)
                    {
                        item.ElementInfo.Bounds = sortBounds;
                    }
                    else
                    {
                        sortBounds = item.ElementInfo.Bounds;
                        item.ElementInfo.Bounds = Rectangle.Empty;
                    }
                }
                if (item.ElementInfo is DevExpress.XtraEditors.Drawing.GridFilterButtonInfoArgs)
                {
                    if (restore)
                    {
                        item.ElementInfo.Bounds = filterBounds;
                    }
                    else
                    {
                        filterInfo = item.ElementInfo;
                        filterBounds = item.ElementInfo.Bounds;
                        item.ElementInfo.Bounds = Rectangle.Empty;
                    }
                }
            }
        }

        public static string ColumnIndexToColumnLetter(int colIndex)
        {
            int div = colIndex;
            string colLetter = String.Empty;
            int mod = 0;

            while (div > 0)
            {
                mod = (div - 1) % 26;
                colLetter = (char)(65 + mod) + colLetter;
                div = (int)((div - mod) / 26);
            }
            return colLetter;
        }

        public static string ExportExcelHeader(string pStrHeader, ExcelWorksheet worksheet, int pCol)
        {
            if (pStrHeader.ToLower() == "sr")
            {
                worksheet.Column(pCol).Width = 5;
                return "Sr";
            }
            else if (pStrHeader.ToLower() == "skeno")
            {
                worksheet.Column(pCol).Width = 12;
                return "SKE No";
            }
            else if (pStrHeader.ToLower() == "stockno")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "#Stock";
            }
            else if (pStrHeader.ToLower() == "status")
            {
                worksheet.Column(pCol).Width = 10;
                return "Status";
            }
            else if (pStrHeader.ToLower() == "shape")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Shape";
            }
            else if (pStrHeader.ToLower() == "carat")
            {
                worksheet.Column(pCol).Width = 7;
                return "Cts";
            }
            else if (pStrHeader.ToLower() == "clarity")
            {
                worksheet.Column(pCol).Width = 5;
                return "Cla";
            }
            else if (pStrHeader.ToLower() == "color")
            {
                worksheet.Column(pCol).Width = 6;
                return "Col";
            }
            else if (pStrHeader.ToLower() == "colorshade")
            {
                worksheet.Column(pCol).Width = 5;
                return "CS";
            }
            else if (pStrHeader.ToLower() == "raprate")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Rap Rate";
            }
            else if (pStrHeader.ToLower() == "rapvalue")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Rap Value";
            }
            else if (pStrHeader.ToLower() == "rapper")
            {
                worksheet.Column(pCol).Width = 6.5;
                return "Rap %";
            }
            else if (pStrHeader.ToLower() == "pricepercarat")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Pr/Ct";
            }
            else if (pStrHeader.ToLower() == "amount")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Amount";
            }
            else if (pStrHeader.ToLower() == "costraprate")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Cost Rap";
            }
            else if (pStrHeader.ToLower() == "costrapvalue")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Cost RapValue";
            }
            else if (pStrHeader.ToLower() == "costrapper")
            {
                worksheet.Column(pCol).Width = 6.5;
                return "Cost Rap%";
            }
            else if (pStrHeader.ToLower() == "costpricepercarat")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Cost Pr/Ct";
            }
            else if (pStrHeader.ToLower() == "costamount")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Cost Amt";
            }
            //if (pStrHeader.ToLower() == "expraprate")
            //{
            //    worksheet.Column(pCol).Width = 8.5;
            //    return "Exp Rap Rate";
            //}
            else if (pStrHeader.ToLower() == "exprapvalue")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Exp Rap Value";
            }
            else if (pStrHeader.ToLower() == "exprapper")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Exp Rap %";
            }
            else if (pStrHeader.ToLower() == "exppricepercarat")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Exp Pr/Ct";
            }
            else if (pStrHeader.ToLower() == "expamount")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Exp Amt";
            }
            //if (pStrHeader.ToLower() == "rapraprate")
            //{
            //    worksheet.Column(pCol).Width = 8.5;
            //    return "Rap Rate";
            //}
            else if (pStrHeader.ToLower() == "rapnetrapvalue")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Rap Value";
            }
            else if (pStrHeader.ToLower() == "rapnetrapper")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Rap %";
            }
            else if (pStrHeader.ToLower() == "rapnetpricepercarat")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Rap Pr/Ct";
            }
            else if (pStrHeader.ToLower() == "rapnetamount")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Rap Amt";
            }


            else if (pStrHeader.ToLower() == "invoicerapvalue")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Sale RapValue";
            }
            else if (pStrHeader.ToLower() == "invoicerapper")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Sale Rap%";
            }
            else if (pStrHeader.ToLower() == "invoicepricepercarat")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Sale Pr/Ct";
            }
            else if (pStrHeader.ToLower() == "invoiceamount")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Sale Amt";
            }


            else if (pStrHeader.ToLower() == "size")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Size";
            }
            else if (pStrHeader.ToLower() == "cut")
            {
                worksheet.Column(pCol).Width = 4.5;
                return "Cut";
            }
            else if (pStrHeader.ToLower() == "pol")
            {
                worksheet.Column(pCol).Width = 4.5;
                return "Pol";
            }
            else if (pStrHeader.ToLower() == "sym")
            {
                worksheet.Column(pCol).Width = 4.5;
                return "Sym";
            }
            else if (pStrHeader.ToLower() == "fl")
            {
                worksheet.Column(pCol).Width = 4.5;
                return "FL";
            }
            else if (pStrHeader.ToLower() == "flshade")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "FlShd";
            }
            else if (pStrHeader.ToLower() == "depthper")
            {
                worksheet.Column(pCol).Width = 5.5;
                return "TD%";
            }
            else if (pStrHeader.ToLower() == "tableper")
            {
                worksheet.Column(pCol).Width = 5.5;
                return "Tab%";
            }
            else if (pStrHeader.ToLower() == "length")
            {
                worksheet.Column(pCol).Width = 5;
                return "L";
            }
            else if (pStrHeader.ToLower() == "width")
            {
                worksheet.Column(pCol).Width = 5;
                return "W";
            }
            else if (pStrHeader.ToLower() == "height")
            {
                worksheet.Column(pCol).Width = 5;
                return "H";
            }
            else if (pStrHeader.ToLower() == "measurement")
            {
                worksheet.Column(pCol).Width = 10;
                return "Measurement";
            }
            else if (pStrHeader.ToLower() == "lab")
            {
                worksheet.Column(pCol).Width = 5;
                return "Lab";
            }
            else if (pStrHeader.ToLower() == "location")
            {
                worksheet.Column(pCol).Width = 10;
                return "Loc";
            }
            else if (pStrHeader.ToLower() == "certno")
            {
                worksheet.Column(pCol).Width = 12;
                return "Cert No";
            }
            else if (pStrHeader.ToLower() == "videourl")
            {
                worksheet.Column(pCol).Width = 12;
                return "Video URL";
            }
            else if (pStrHeader.ToLower() == "stonedetailurl")
            {
                worksheet.Column(pCol).Width = 20;
                return "Media URL";
            }

            else if (pStrHeader.ToLower() == "milky")
            {
                worksheet.Column(pCol).Width = 5;
                return "Milky";
            }
            else if (pStrHeader.ToLower() == "luster")
            {
                worksheet.Column(pCol).Width = 5;
                return "Luster";
            }
            else if (pStrHeader.ToLower() == "eyec")
            {
                worksheet.Column(pCol).Width = 5;
                return "EC";
            }
            else if (pStrHeader.ToLower() == "ha")
            {
                worksheet.Column(pCol).Width = 5;
                return "HA";
            }

            else if (pStrHeader.ToLower() == "girdle")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Girdle";
            }
            else if (pStrHeader.ToLower() == "tableinc")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "TableInc";
            }
            else if (pStrHeader.ToLower() == "tableopen")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "TableOpen";
            }
            else if (pStrHeader.ToLower() == "sidetable")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "SideTable";
            }
            else if (pStrHeader.ToLower() == "sideopen")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "SideOpen";
            }
            else if (pStrHeader.ToLower() == "tableblack")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "TableBlack";
            }
            else if (pStrHeader.ToLower() == "sideblack")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "SideBlack";
            }
            else if (pStrHeader.ToLower() == "redsport")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "RedSport";
            }
            else if (pStrHeader.ToLower() == "ratio")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Ratio";
            }
            else if (pStrHeader.ToLower() == "girdper")
            {
                worksheet.Column(pCol).Width = 6;
                return "Girdle%";
            }
            else if (pStrHeader.ToLower() == "pavang")
            {
                worksheet.Column(pCol).Width = 6;
                return "PVAng";
            }
            else if (pStrHeader.ToLower() == "pavheight")
            {
                worksheet.Column(pCol).Width = 6;
                return "PVHgt";
            }
            else if (pStrHeader.ToLower() == "crang")
            {
                worksheet.Column(pCol).Width = 6;
                return "CRAng";
            }
            else if (pStrHeader.ToLower() == "crheight")
            {
                worksheet.Column(pCol).Width = 6;
                return "CRHgt";
            }
            else if (pStrHeader.ToLower() == "girddesc")
            {
                worksheet.Column(pCol).Width = 15;
                return "Girdle Desc";
            }
            else if (pStrHeader.ToLower() == "girdcond")
            {
                worksheet.Column(pCol).Width = 10;
                return "Girdle";
            }
            else if (pStrHeader.ToLower() == "keytosymbol")
            {
                worksheet.Column(pCol).Width = 40;
                return "Key To Sym";
            }

            else if (pStrHeader.ToLower() == "mfgid")
            {
                worksheet.Column(pCol).Width = 10;
                return "MFGID";
            }
            else if (pStrHeader.ToLower() == "lotname")
            {
                worksheet.Column(pCol).Width = 10;
                return "LotName";
            }
            else if (pStrHeader.ToLower() == "culet")
            {
                worksheet.Column(pCol).Width = 10;
                return "Culet";
            }
            else if (pStrHeader.ToLower() == "diamin")
            {
                worksheet.Column(pCol).Width = 10;
                return "DiaMin";
            }
            else if (pStrHeader.ToLower() == "diamax")
            {
                worksheet.Column(pCol).Width = 10;
                return "DiaMax";
            }
            else if (pStrHeader.ToLower() == "isnobgm")
            {
                worksheet.Column(pCol).Width = 10;
                return "IsNoBGM";
            }
            else if (pStrHeader.ToLower() == "flcolor")
            {
                worksheet.Column(pCol).Width = 10;
                return "FLColor";
            }
            else if (pStrHeader.ToLower() == "shapedesc")
            {
                worksheet.Column(pCol).Width = 10;
                return "ShapeDesc";
            }
            else if (pStrHeader.ToLower() == "starlength")
            {
                worksheet.Column(pCol).Width = 10;
                return "StarLength";
            }
            else if (pStrHeader.ToLower() == "lowerhalf")
            {
                worksheet.Column(pCol).Width = 10;
                return "LowerHalf";
            }
            else if (pStrHeader.ToLower() == "painting")
            {
                worksheet.Column(pCol).Width = 10;
                return "Painting";
            }
            else if (pStrHeader.ToLower() == "proportion")
            {
                worksheet.Column(pCol).Width = 10;
                return "Proportion";
            }
            else if (pStrHeader.ToLower() == "paintcomm")
            {
                worksheet.Column(pCol).Width = 10;
                return "PaintComm";
            }
            else if (pStrHeader.ToLower() == "inscription")
            {
                worksheet.Column(pCol).Width = 10;
                return "Inscription";
            }
            else if (pStrHeader.ToLower() == "syntheticindicator")
            {
                worksheet.Column(pCol).Width = 10;
                return "SyntheticIndicator";
            }
            else if (pStrHeader.ToLower() == "polishfeatures")
            {
                worksheet.Column(pCol).Width = 10;
                return "PolishFeatures";
            }
            else if (pStrHeader.ToLower() == "symmetryfeatures")
            {
                worksheet.Column(pCol).Width = 10;
                return "SymmetryFeatures";
            }
            else if (pStrHeader.ToLower() == "jobno")
            {
                worksheet.Column(pCol).Width = 10;
                return "JobNo";
            }
            else if (pStrHeader.ToLower() == "giacontrolno")
            {
                worksheet.Column(pCol).Width = 10;
                return "GIAControlNo";
            }
            else if (pStrHeader.ToLower() == "fancycolor")
            {
                worksheet.Column(pCol).Width = 10;
                return "FancyColor";
            }
            else if (pStrHeader.ToLower() == "fancycolorintensity")
            {
                worksheet.Column(pCol).Width = 10;
                return "FancyColorIntensity";
            }
            else if (pStrHeader.ToLower() == "fancycolorovertone")
            {
                worksheet.Column(pCol).Width = 10;
                return "FancyColorOvertone";
            }
            else if (pStrHeader.ToLower() == "diamonddossier")
            {
                worksheet.Column(pCol).Width = 10;
                return "DiamondDossier";
            }
            else if (pStrHeader.ToLower() == "comment")
            {
                worksheet.Column(pCol).Width = 40;
                return "Comment";
            }
            else if (pStrHeader.ToLower() == "fancycolordescription")
            {
                worksheet.Column(pCol).Width = 50;
                return "Fancy Color Description";
            }
            else if (pStrHeader.ToLower() == "imageurl")
            {
                worksheet.Column(pCol).Width = 50;
                worksheet.Column(pCol).Hidden = true;
                return "Image URL";
            }
            else if (pStrHeader.ToLower() == "certurl")
            {
                worksheet.Column(pCol).Width = 50;
                worksheet.Column(pCol).Hidden = true;
                return "Cert URL";
            }
            else if (pStrHeader.ToLower() == "videourl")
            {
                worksheet.Column(pCol).Width = 50;
                worksheet.Column(pCol).Hidden = true;
                return "Video URL";
            }
            else if (pStrHeader.ToLower() == "dnapageurl")
            {
                worksheet.Column(pCol).Width = 50;
                worksheet.Column(pCol).Hidden = true;
                return "DNA Page URL";
            }

            else if (pStrHeader.ToLower() == "colgroup")
            {
                worksheet.Column(pCol).Width = 5;
                worksheet.Column(pCol).Hidden = true;
                return "ColGroup";
            }
            else if (pStrHeader.ToLower() == "clagroup")
            {
                worksheet.Column(pCol).Width = 5;
                worksheet.Column(pCol).Hidden = true;
                return "ClaGroup";
            }
            else if (pStrHeader.ToLower() == "cutgroup")
            {
                worksheet.Column(pCol).Width = 5;
                worksheet.Column(pCol).Hidden = true;
                return "CutGroup";
            }
            else if (pStrHeader.ToLower() == "polgroup")
            {
                worksheet.Column(pCol).Width = 5;
                worksheet.Column(pCol).Hidden = true;
                return "PolGroup";
            }
            else if (pStrHeader.ToLower() == "symgroup")
            {
                worksheet.Column(pCol).Width = 5;
                worksheet.Column(pCol).Hidden = true;
                return "SymGroup";
            }
            else if (pStrHeader.ToLower() == "flgroup")
            {
                worksheet.Column(pCol).Width = 5;
                worksheet.Column(pCol).Hidden = true;
                return "FLGroup";
            }
            return "";
        }

        public static string ExportExcelHeaderMemo(String pstrHeader, ExcelWorksheet worksheet, int pCol)
        {
            if (pstrHeader.ToLower() == "kapanname")
            {
                worksheet.Column(pCol).Width = 15;
                return "KapanName";
            }
            else if (pstrHeader.ToLower() == "pcs")
            {
                worksheet.Column(pCol).Width = 10;
                return "Pcs";
            }
            else if (pstrHeader.ToLower() == "carat")
            {
                worksheet.Column(pCol).Width = 10;
                return "Carat";
            }
            return "";
        }

        public static string ExportExcelHeaderMfg(string pStrHeader, ExcelWorksheet worksheet, int pCol)
        {
            if (pStrHeader.ToLower() == "kapan")
            {
                worksheet.Column(pCol).Width = 10;
                return "Kapan";
            }
            else if (pStrHeader.ToLower() == "stockno")
            {
                worksheet.Column(pCol).Width = 10;
                return "StockNo";
            }
            else if (pStrHeader.ToLower() == "packetno") // K : 22/12/202
            {
                worksheet.Column(pCol).Width = 10;
                return "PacketNo";
            }
            else if (pStrHeader.ToLower() == "tag")// K : 22/12/202
            {
                worksheet.Column(pCol).Width = 10;
                return "Tag";
            }
            else if (pStrHeader.ToLower() == "maxstockno")// K : 22/12/202
            {
                worksheet.Column(pCol).Width = 15;
                return "MaxStockNo";
            }
            else if (pStrHeader.ToLower() == "colorshade")// K : 22/12/202
            {
                worksheet.Column(pCol).Width = 10;
                return "Color Shade";
            }
            else if (pStrHeader.ToLower() == "milky")// K : 22/12/202
            {
                worksheet.Column(pCol).Width = 10;
                return "Milky";
            }
            else if (pStrHeader.ToLower() == "culet")// K : 22/12/202
            {
                worksheet.Column(pCol).Width = 10;
                return "Culet";
            }
            else if (pStrHeader.ToLower() == "girdleinc")// K : 22/12/202
            {
                worksheet.Column(pCol).Width = 10;
                return "Girdel Inc";
            }
            else if (pStrHeader.ToLower() == "width")// K : 22/12/202
            {
                worksheet.Column(pCol).Width = 10;
                return "Width";
            }
            else if (pStrHeader.ToLower() == "length")// K : 22/12/202
            {
                worksheet.Column(pCol).Width = 10;
                return "Length";
            }
            else if (pStrHeader.ToLower() == "ratio")// K : 22/12/202
            {
                worksheet.Column(pCol).Width = 10;
                return "Ratio";
            }
            else if (pStrHeader.ToLower() == "mfgrap")// K : 22/12/202
            {
                worksheet.Column(pCol).Width = 10;
                return "MFG Rap";
            }
            else if (pStrHeader.ToLower() == "mfgpricepercarat")// K : 22/12/202
            {
                worksheet.Column(pCol).Width = 10;
                return "MFG $/Cts";
            }
            else if (pStrHeader.ToLower() == "mfgdiscount")// K : 22/12/202
            {
                worksheet.Column(pCol).Width = 15;
                return "MFG %";
            }
            else if (pStrHeader.ToLower() == "mfgamount")// K : 22/12/202
            {
                worksheet.Column(pCol).Width = 10;
                return "MFGAmount";
            }
            else if (pStrHeader.ToLower() == "shape")
            {
                worksheet.Column(pCol).Width = 10;
                return "Shp";
            }
            else if (pStrHeader.ToLower() == "carat")
            {
                worksheet.Column(pCol).Width = 10;
                return "Carat";
            }
            else if (pStrHeader.ToLower() == "clarity")
            {
                worksheet.Column(pCol).Width = 12;
                return "Cla";
            }
            else if (pStrHeader.ToLower() == "color")
            {
                worksheet.Column(pCol).Width = 15;
                return "Col";
            }
            else if (pStrHeader.ToLower() == "mfgcolor")
            {
                worksheet.Column(pCol).Width = 15;
                return "MfgCol";
            }
            else if (pStrHeader.ToLower() == "mfgclarity")
            {
                worksheet.Column(pCol).Width = 15;
                return "MfgCla";
            }
            else if (pStrHeader.ToLower() == "cut")
            {
                worksheet.Column(pCol).Width = 15;
                return "Cut";
            }
            else if (pStrHeader.ToLower() == "pol")
            {
                worksheet.Column(pCol).Width = 15;
                return "Pol";
            }
            else if (pStrHeader.ToLower() == "sym")
            {
                worksheet.Column(pCol).Width = 15;
                return "Sym";
            }
            else if (pStrHeader.ToLower() == "fl")
            {
                worksheet.Column(pCol).Width = 15;
                return "FL";
            }
            //else if (pStrHeader.ToLower() == "costpercts")
            //{
            //    worksheet.Column(pCol).Width = 10;
            //    return "Crate";
            //}
            else if (pStrHeader.ToLower() == "discount")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Disc%";
            }
            else if (pStrHeader.ToLower() == "diamin")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "MinDia";
            }
            else if (pStrHeader.ToLower() == "depthper")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Depth%";
            }
            else if (pStrHeader.ToLower() == "remark")
            {
                worksheet.Column(pCol).Width = 17;
                return "Remark";
            }
            //else if (pStrHeader.ToLower() == "brown")
            //{
            //    worksheet.Column(pCol).Width = 13;
            //    return "Brn_SortName";
            //}
            else if (pStrHeader.ToLower() == "tableinc")
            {
                worksheet.Column(pCol).Width = 13;
                return "Table Inc";
            }
            else if (pStrHeader.ToLower() == "sidetable")
            {
                worksheet.Column(pCol).Width = 13;
                return "Side Table";
            }
            else if (pStrHeader.ToLower() == "tableblack")
            {
                worksheet.Column(pCol).Width = 13;
                return "Table Balck";
            }
            else if (pStrHeader.ToLower() == "sideblack")
            {
                worksheet.Column(pCol).Width = 13;
                return "Size Black";
            }
            else if (pStrHeader.ToLower() == "tableopen")
            {
                worksheet.Column(pCol).Width = 13;
                return "Table Open";
            }
            else if (pStrHeader.ToLower() == "sideopen")
            {
                worksheet.Column(pCol).Width = 13;
                return "Side Open";
            }
            else if (pStrHeader.ToLower() == "redsport")
            {
                worksheet.Column(pCol).Width = 13;
                return "Red Sport";
            }
            else if (pStrHeader.ToLower() == "luster")
            {
                worksheet.Column(pCol).Width = 13;
                return "Luster";
            }
            else if (pStrHeader.ToLower() == "eyeclean")
            {
                worksheet.Column(pCol).Width = 15;
                return "EC";
            }
            else if (pStrHeader.ToLower() == "lab")
            {
                worksheet.Column(pCol).Width = 10;
                return "Lab";
            }
            else if (pStrHeader.ToLower() == "ha")
            {
                worksheet.Column(pCol).Width = 10;
                return "HA";
            }
            else if (pStrHeader.ToLower() == "diamax")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "MaxDia";
            }
            //else if (pStrHeader.ToLower() == "height")
            //{
            //    worksheet.Column(pCol).Width = 6.5;
            //    return "Hgt ";
            //}
            else if (pStrHeader.ToLower() == "tableper")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Table%";
            }
            else if (pStrHeader.ToLower() == "mfg_id")
            {
                worksheet.Column(pCol).Width = 10;
                return "Mfg Id";
            }
            else if (pStrHeader.ToLower() == "mfggradingentrysrno")  //hinal 03-01-2022
            {
                worksheet.Column(pCol).Width = 15;
                return "MfgGrdSrNo";
            }
            return "";
        }

        public static string GetMonthName(int pIntMonthNumber)
        {
            if (pIntMonthNumber == 1) return "Jan";
            else if (pIntMonthNumber == 2) return "Feb";
            else if (pIntMonthNumber == 3) return "Mar";
            else if (pIntMonthNumber == 4) return "Apr";
            else if (pIntMonthNumber == 5) return "May";
            else if (pIntMonthNumber == 6) return "Jun";
            else if (pIntMonthNumber == 7) return "Jul";
            else if (pIntMonthNumber == 8) return "Aug";
            else if (pIntMonthNumber == 9) return "Sep";
            else if (pIntMonthNumber == 10) return "Oct";
            else if (pIntMonthNumber == 11) return "Nov";
            else if (pIntMonthNumber == 12) return "Dec";
            else
                return "";
        }
        public static string GetMonthCode(string pStrMonthNumber)
        {
            if (pStrMonthNumber.ToUpper() == "JAN") return "01";
            else if (pStrMonthNumber.ToUpper() == "FEB") return "02";
            else if (pStrMonthNumber.ToUpper() == "MAR") return "03";
            else if (pStrMonthNumber.ToUpper() == "APR") return "04";
            else if (pStrMonthNumber.ToUpper() == "MAY") return "05";
            else if (pStrMonthNumber.ToUpper() == "JUN") return "06";
            else if (pStrMonthNumber.ToUpper() == "JUL") return "07";
            else if (pStrMonthNumber.ToUpper() == "AUG") return "08";
            else if (pStrMonthNumber.ToUpper() == "SEP") return "09";
            else if (pStrMonthNumber.ToUpper() == "OCT") return "10";
            else if (pStrMonthNumber.ToUpper() == "NOV") return "11";
            else if (pStrMonthNumber.ToUpper() == "DEC") return "12";
            else
                return "";
        }


        /// <summary>
        /// Method For Check Exe Modification And Also Update
        /// </summary>
        /// <returns></returns>        
        private static Boolean ChkUpdate()
        {
            try
            {
                if (Global.gStrExePath != "" && File.Exists(Global.gStrExePath + "\\" + System.Windows.Forms.Application.ProductName + ".EXE"))
                {
                    if (File.GetLastWriteTime(Application.StartupPath + "\\" + Application.ProductName + ".EXE") < File.GetLastWriteTime(Global.gStrExePath + "\\" + Application.ProductName + ".EXE"))
                    {
                        if (Global.Confirm("Would You Like Update Latest EXE") == DialogResult.Yes)
                        {
                            if (File.Exists(Application.StartupPath + "\\Update.Bat"))
                            {
                                File.Delete(Application.StartupPath + "\\Update.Bat");
                            }
                            FileStream fs = new FileStream(Application.StartupPath + "\\Update.Bat", FileMode.Create, FileAccess.ReadWrite);
                            StreamWriter sw = new StreamWriter(fs, Encoding.Default);

                            sw.Write("taskkill /F /IM " + Application.ProductName + ".EXE");
                            sw.WriteLine("");
                            //sw.WriteLine("copy " + Global.gStrExePath + "\\" + Application.ProductName + ".EXE  " + Application.StartupPath + "\\" + Application.ProductName + ".EXE /Y ");
                            // sw.WriteLine("copy " + Global.gStrExePath + "\\*.*  " + Application.StartupPath + "\\*.* /Y ");
                            //sw.WriteLine(""); 
                            //sw.WriteLine("copy " + Global.gStrExePath + "\\RPT\\*.* " + Application.StartupPath + "\\RPT\\*.* /Y ");

                            sw.WriteLine("xcopy " + Global.gStrExePath + " " + Application.StartupPath + " /E /H /C /I /Y");

                            sw.Flush();
                            sw.Close();
                            fs.Close();
                            System.Diagnostics.Process.Start(Application.StartupPath + "\\Update.Bat").WaitForExit();

                            return true;
                        }
                        else
                            return true;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return false;
        }

        public static long GetMillis()
        {
            DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            return (long)(DateTime.UtcNow - Jan1st1970).TotalMilliseconds;
        }

        public static void SelectLanguage(LANGUAGE pLanguage)
        {
            if (pLanguage == LANGUAGE.GUJARATI)
            {
                foreach (InputLanguage lan in AL)
                {
                    if (lan.Culture.ToString().ToUpper() == "GU-IN")
                    {
                        InputLanguage.CurrentInputLanguage = lan;
                    }
                }
            }
            else if (pLanguage == LANGUAGE.ENGLISH)
            {
                foreach (InputLanguage lan in AL)
                {
                    if (lan.Culture.ToString().ToUpper() == "EN-US")
                    {
                        InputLanguage.CurrentInputLanguage = lan;
                    }
                }
            }
        }

        public static DataTable GetDataTableFromExcel(string path, bool hasHeader = true) //Get Excel Data Without Connection : 26-06-2019
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    if (firstRowCell.Text == string.Empty)
                        tbl.Columns.Add(hasHeader ? "A1" : string.Format("Column {0}", firstRowCell.Start.Column));
                    else
                        tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    //var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    var wsRow = ws.Cells[rowNum, 1, rowNum, tbl.Columns.Count];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                return tbl;
            }
        }
        public static DataTable ImportExcelXLSWithSheetName(string FileName, bool hasHeaders, string SheetName, int IntIMEX = 0)
        {
            string HDR = hasHeaders ? "Yes" : "No";
            string strConn;
            if (FileName.Substring(FileName.LastIndexOf('.')).ToLower() == ".xlsx")
                strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties=\"Excel 12.0;HDR=" + HDR + ";IMEX=" + IntIMEX + "; TypeGuessRows=0;ImportMixedTypes=Text\"";
            //strConn = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + FileName + ";Extended Properties=\"Excel 12.0 Xml;HDR=YES;\"";

            else
                //strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=" + IntIMEX + "; TypeGuessRows=0;ImportMixedTypes=Text\"";
                strConn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName + ";Extended Properties=\"Excel 8.0;HDR=" + HDR + ";IMEX=" + IntIMEX + "; TypeGuessRows=0;ImportMixedTypes=Text\"";


            using (OleDbConnection conn = new OleDbConnection(strConn))
            {
                conn.Open();

                DataTable schemaTable = new DataTable("Temp");
                try
                {
                    OleDbCommand cmd = new OleDbCommand("SELECT * FROM [" + SheetName + "]", conn);
                    cmd.CommandType = CommandType.Text;

                    new OleDbDataAdapter(cmd).Fill(schemaTable);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message + string.Format("Sheet:{0}.File:F{1}", SheetName, FileName), ex);
                }
                return schemaTable;
            }
        }
        public static DataTable GetDataTableFromCsv(string path, bool isFirstRowHeader)
        {
            string header = isFirstRowHeader ? "Yes" : "No";

            string pathOnly = Path.GetDirectoryName(path);
            string fileName = Path.GetFileName(path);

            string sql = @"SELECT * FROM [" + fileName + "]";

            using (OleDbConnection connection = new OleDbConnection(
                      @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathOnly +
                      ";Extended Properties=\"Text;HDR=" + header + "\""))
            using (OleDbCommand command = new OleDbCommand(sql, connection))
            using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
            {
                DataTable dataTable = new DataTable();
                dataTable.Locale = CultureInfo.CurrentCulture;
                adapter.Fill(dataTable);
                return dataTable;
            }
        }


        public static DataTable GetExportFileTemplate()
        {
            DataTable DTab = new DataTable();
            DTab.Columns.Add(new DataColumn("FORMAT"));
            DTab.Rows.Add("Stock List");
            DTab.Rows.Add("General");
            DTab.Rows.Add("Short Detail");
            DTab.Rows.Add("With Exp");
            DTab.Rows.Add("With Sale");
            DTab.Rows.Add("With Rapnet");
            DTab.Rows.Add("With All Details");
            DTab.Rows.Add("Revise List");
            DTab.Rows.Add("Offer Price Report");
            DTab.Rows.Add("Smart-I");
            DTab.Rows.Add("ST & Cost System Price");
            DTab.Rows.Add("MFG Comparision Report"); 
            DTab.Rows.Add("Proportion File ");
            DTab.Rows.Add("IGI");//ADD BY RAJVI : 21/05/2025
            DTab.Rows.Add("VDB");//ADD BY RAJVI : 21/05/2025
            DTab.Rows.Add("Master");//ADD BY RAJVI : 21/05/2025
            DTab.Rows.Add("NP");//ADD BY RAJVI : 21/05/2025

            return DTab;
        }

        public static DataTable GetKapanAssortmentExportFileTemplate()
        {
            DataTable DTab = new DataTable();
            DTab.Columns.Add(new DataColumn("FORMAT"));
            DTab.Rows.Add("CLARITY ASSORTMENT");
            DTab.Rows.Add("BOMBAY TRANSFER");
            DTab.Rows.Add("BOMBAY RECEIVE");
            return DTab;
        }

        public static void BarcodePrint(string StrKapanName, string PacketNo, string Tag, string Date, string Carat, string MarkerCode)
        {
            try
            {
                string fileLoc = Application.StartupPath + "\\PrintBarcodeData.txt";
                if (System.IO.File.Exists(fileLoc) == true)
                {
                    System.IO.File.Delete(fileLoc);
                }

                System.IO.File.Create(fileLoc).Dispose();

                string StrBarcode = StrKapanName + Environment.NewLine + PacketNo + Environment.NewLine + Tag;
                string StrPrint = StrKapanName + "-" + PacketNo + "-" + Tag;
                string SRD = "SKE";
                StreamWriter sw = new StreamWriter(fileLoc);
                using (sw)
                {
                    //sw.WriteLine("I8,A");
                    //sw.WriteLine("ZN");
                    //sw.WriteLine("q400");
                    //sw.WriteLine("O");
                    //sw.WriteLine("JF");
                    //sw.WriteLine("KIZZQ0");
                    //sw.WriteLine("KI9+0.0");
                    //sw.WriteLine("ZT");
                    //sw.WriteLine("Q120,25");
                    //sw.WriteLine("Arglabel 150 31");
                    //sw.WriteLine("exit");
                    //sw.WriteLine("KI80");
                    //sw.WriteLine("N");
                    //sw.WriteLine("B295,87,2,1,2,4,56,N,\"" + StrBarcode + "\"");
                    //sw.WriteLine("A295,104,2,1,1,1,N,\"" + StrPrint + "\"");
                    //sw.WriteLine("A375,91,2,1,2,2,N,\""+SRD+"\"");
                    //sw.WriteLine("A135,109,2,1,1,1,N,\"" + Date + "\"");
                    //sw.WriteLine("A359,53,2,1,1,1,N,\"" + MarkerCode + "\"");
                    //sw.WriteLine("A183,21,2,1,1,1,N,\"" + Carat + "\"");
                    //sw.WriteLine("P1");

                    sw.WriteLine("I8,A");
                    sw.WriteLine("ZN");
                    sw.WriteLine("q400");
                    sw.WriteLine("O");
                    sw.WriteLine("JF");
                    sw.WriteLine("KIZZQ0");
                    sw.WriteLine("KI9+0.0");
                    sw.WriteLine("ZT");
                    sw.WriteLine("Q120,25");
                    sw.WriteLine("Arglabel 150 31");
                    sw.WriteLine("exit");
                    sw.WriteLine("KI80");
                    sw.WriteLine("N");
                    sw.WriteLine("B351,87,2,1,2,4,56,N,\"" + StrBarcode + "\"");
                    sw.WriteLine("A351,111,2,3,1,1,N,\"" + StrPrint + "\"");
                    sw.WriteLine("A111,109,2,1,1,1,N,\"" + Date + "\"");
                    sw.WriteLine("A343,28,2,3,1,1,N,\"" + MarkerCode + "\"");
                    sw.WriteLine("A111,28,2,3,1,1,N,\"" + Carat + "\"");
                    sw.WriteLine("P1");
                }
                sw.Dispose();
                sw = null;
                if (File.Exists(Application.StartupPath + "\\PRINTBARCODE.BAT") && File.Exists(fileLoc))
                {
                    //Global.Message("Ready For Print");
                    //ProcessStartInfo proc = new ProcessStartInfo();
                    //proc.CreateNoWindow = false;
                    //proc.UseShellExecute = true;
                    //proc.WorkingDirectory = Application.StartupPath + "\\";
                    //proc.FileName = "PRINTBARCODE.BAT";
                    //proc.Arguments = fileLoc;
                    //proc.Verb = "runas";
                    //try
                    //{
                    //    System.Diagnostics.Process P = new System.Diagnostics.Process();
                    //    P.StartInfo = proc;
                    //    P.Start();
                    //    Global.Message("Ready For Print dONE");
                    //}
                    //catch (Exception EX)
                    //{
                    //    Global.Message(EX.Message);
                    //}

                    Microsoft.VisualBasic.Interaction.Shell(Application.StartupPath + "\\PRINTBARCODE.BAT " + fileLoc, AppWinStyle.Hide, true, -1);
                }

                Thread.Sleep(800);

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        //public static void BombayPrintBarcodePrint(DataRow DRow)
        //{
        //    BusLib.Configuration.BOValidation Val = new BOValidation();

        //    try
        //    {
        //        // First page

        //        string fileLoc = Application.StartupPath + "\\PrintBarcodeDataBombay.txt";
        //        if (System.IO.File.Exists(fileLoc) == true)
        //        {
        //            System.IO.File.Delete(fileLoc);
        //        }

        //        System.IO.File.Create(fileLoc).Dispose();

        //        StreamWriter sw = new StreamWriter(fileLoc);
        //        using (sw)
        //        {
        //            sw.WriteLine("I8,A");
        //            sw.WriteLine("ZN");
        //            sw.WriteLine("q548");
        //            sw.WriteLine("O");
        //            sw.WriteLine("JF");
        //            sw.WriteLine("KIZZQ0");
        //            sw.WriteLine("KI9+0.0");
        //            sw.WriteLine("ZT");
        //            sw.WriteLine("Q288,B25");
        //            sw.WriteLine("Arglabel 360 31");
        //            sw.WriteLine("exit");
        //            sw.WriteLine("N");

        //            sw.WriteLine("B524,269,2,1,2,4,51,N,\"" + Val.ToString(DRow["PARTYSTOCKNO"]) + "\"");
        //            sw.WriteLine("A230,271,2,3,1,1,N,\"SR\"");
        //            sw.WriteLine("A200,272,2,3,1,1,N,\"" + ":" + Val.ToString(DRow["SERIALNO"]) + "\"");
        //            sw.WriteLine("A230,239,2,3,1,1,N,\"ID\"");
        //            sw.WriteLine("A200,239,2,3,1,1,N,\"" + ":" + Val.ToString(DRow["PARTYSTOCKNO"]) + "\"");
        //            sw.WriteLine("A526,78,2,3,1,1,N,\"" + Val.ToString(DRow["LABNAME"]) + "\"");
        //            sw.WriteLine("A479,78,2,3,1,1,N,\"" + ":" + Val.ToString(DRow["LABREPORTNO"]) + "\"");
        //            sw.WriteLine("A525,111,2,3,1,1,N,\"COL/CLA\"");
        //            sw.WriteLine("A420,111,2,3,1,1,N,\"" + ":" + Val.ToString(DRow["COLORNAME"]) + "/" + "" + Val.ToString(DRow["CLARITYNAME"]) + "\"");
        //            sw.WriteLine("A193,111,2,3,1,1,N,\"FL\"");
        //            sw.WriteLine("A144,111,2,3,1,1,N,\"" + ":" + Val.ToString(DRow["FLNAME"]) + "\"");
        //            sw.WriteLine("A523,46,2,3,1,1,N,\"MEAS\"");
        //            sw.WriteLine("A463,46,2,3,1,1,N,\"" + ":" + Val.ToString(DRow["MEASUREMENT"]) + "\"");
        //            sw.WriteLine("A344,168,2,3,1,1,N,\"" + ":" + Val.ToString(DRow["CARAT"]) + "\"");
        //            sw.WriteLine("A385,168,2,3,1,1,N,\"CTS\"");
        //            sw.WriteLine("A193,198,2,3,1,1,N,\"CUT\"");
        //            sw.WriteLine("A145,198,2,3,1,1,N,\"" + ":" + Val.ToString(DRow["CUTNAME"]) + "\"");
        //            sw.WriteLine("A145,168,2,3,1,1,N,\"" + ":" + Val.ToString(DRow["POLNAME"]) + "\"");
        //            sw.WriteLine("A193,168,2,3,1,1,N,\"POL\"");
        //            sw.WriteLine("A145,140,2,3,1,1,N,\"" + ":" + Val.ToString(DRow["SYMNAME"]) + "\"");
        //            sw.WriteLine("A193,141,2,3,1,1,N,\"SYM\"");
        //            sw.WriteLine("A193,78,2,3,1,1,N,\"TD%\"");
        //            sw.WriteLine("A143,78,2,3,1,1,N,\"" + ":" + Val.ToString(DRow["DEPTHPER"]) + "\"");
        //            sw.WriteLine("A141,46,2,3,1,1,N,\"" + ":" + Val.ToString(DRow["TABLEPER"]) + "\"");
        //            sw.WriteLine("A193,46,2,3,1,1,N,\"TB%\"");
        //            sw.WriteLine("LO25,207,501,1");
        //            sw.WriteLine("A385,198,2,3,1,1,N,\"SHP\"");
        //            sw.WriteLine("A343,198,2,3,1,1,N,\"" + ":" + Val.ToString(DRow["SHAPENAME"]) + "\"");
        //            sw.WriteLine("A520,196,2,2,2,2,N,\"SRD\"");
        //            sw.WriteLine("P1");
        //        }
        //        sw.Dispose();
        //        sw = null;
        //        if (File.Exists(Application.StartupPath + "\\PRINTBARCODE.BAT") && File.Exists(fileLoc))
        //        {
        //            Microsoft.VisualBasic.Interaction.Shell(Application.StartupPath + "\\PRINTBARCODE.BAT " + fileLoc, AppWinStyle.Hide, true, -1);
        //        }

        //        Thread.Sleep(800);


        //        fileLoc = Application.StartupPath + "\\PrintBarcodeDataBombay1.txt";
        //        if (System.IO.File.Exists(fileLoc) == true)
        //        {
        //            System.IO.File.Delete(fileLoc);
        //        }

        //        System.IO.File.Create(fileLoc).Dispose();

        //        sw = new StreamWriter(fileLoc);
        //        using (sw)
        //        {
        //            sw.WriteLine("I8,A");
        //            sw.WriteLine("ZN");
        //            sw.WriteLine("q548");
        //            sw.WriteLine("O");
        //            sw.WriteLine("JF");
        //            sw.WriteLine("KIZZQ0");
        //            sw.WriteLine("KI9+0.0");
        //            sw.WriteLine("ZT");
        //            sw.WriteLine("Q288,B25");
        //            sw.WriteLine("Arglabel 360 31");
        //            sw.WriteLine("exit");
        //            sw.WriteLine("N");
        //            sw.WriteLine("LO276,154,1,123");
        //            sw.WriteLine("A525,272,2,3,1,1,N,\"BLACK TABLE\"");
        //            sw.WriteLine("A358,272,2,3,1,1,N,\":" + Val.ToString(DRow["BLACKTABLE"]) + "\"");
        //            sw.WriteLine("A525,245,2,3,1,1,N,\"BLACK CROWN\"");
        //            sw.WriteLine("A358,244,2,3,1,1,N,\":" + Val.ToString(DRow["BLACKCROWN"]) + "\"");
        //            sw.WriteLine("A525,215,2,3,1,1,N,\"WHITE TABLE\"");
        //            sw.WriteLine("A358,215,2,3,1,1,N,\":" + Val.ToString(DRow["WHITETABLE"]) + "\"");
        //            sw.WriteLine("A358,185,2,3,1,1,N,\":" + Val.ToString(DRow["WHITECROWN"]) + "\"");
        //            sw.WriteLine("A249,272,2,3,1,1,N,\"TABLE OPEN\"");
        //            sw.WriteLine("A99,272,2,3,1,1,N,\":" + Val.ToString(DRow["TABLEOPEN"]) + "\"");
        //            sw.WriteLine("A249,244,2,3,1,1,N,\"CROWN OPEN\"");
        //            sw.WriteLine("A99,244,2,3,1,1,N,\":" + Val.ToString(DRow["CROWNOPEN"]) + "\"");
        //            sw.WriteLine("A249,215,2,3,1,1,N,\"PAV OPEN\"");
        //            sw.WriteLine("A99,215,2,3,1,1,N,\":" + Val.ToString(DRow["PAVILLIONOPEN"]) + "\"");
        //            sw.WriteLine("A249,185,2,3,1,1,N,\"GIRDLE\"");
        //            sw.WriteLine("A517,146,2,3,1,1,N,\"RATIO\"");
        //            sw.WriteLine("A442,146,2,3,1,1,N,\":" + Val.ToString(DRow["RATIO"]) + "\"");
        //            sw.WriteLine("A305,146,2,3,1,1,N,\"CA\"");
        //            sw.WriteLine("A276,146,2,3,1,1,N,\":" + Val.ToString(DRow["CRANGLE"]) + "\"");
        //            sw.WriteLine("A154,146,2,3,1,1,N,\"PA\"");
        //            sw.WriteLine("A275,118,2,3,1,1,N,\":" + Val.ToString(DRow["STARLENGTH"]) + "\"");
        //            sw.WriteLine("A305,118,2,3,1,1,N,\"ST\"");
        //            sw.WriteLine("A479,118,2,3,1,1,N,\"LH\"");
        //            sw.WriteLine("A442,118,2,3,1,1,N,\":" + Val.ToString(DRow["LOWERHALF"]) + "\"");
        //            sw.WriteLine("A516,85,2,3,1,1,N,\"BGM\"");
        //            sw.WriteLine("A468,85,2,3,1,1,N,\":" + Val.ToString(DRow["BGM"]) + "\"");
        //            sw.WriteLine("A123,85,2,3,1,1,N,\"CS\"");
        //            sw.WriteLine("A314,85,2,3,1,1,N,\"LUS\"");
        //            sw.WriteLine("A268,85,2,3,1,1,N,\":" + Val.ToString(DRow["LUSTERNAME"]) + "\"");
        //            sw.WriteLine("A517,59,2,3,1,1,N,\"MI\"");
        //            sw.WriteLine("A468,59,2,3,1,1,N,\":" + Val.ToString(DRow["MILKYNAME"]) + "\"");
        //            sw.WriteLine("A268,59,2,3,1,1,N,\":" + Val.ToString(DRow["EYECLEANNAME"]) + "\"");
        //            sw.WriteLine("A314,59,2,3,1,1,N,\"EC\"");
        //            sw.WriteLine("A123,59,2,3,1,1,N,\"HA\"");
        //            sw.WriteLine("A91,59,2,3,1,1,N,\":" + Val.ToString(DRow["HANAME"]) + "\"");
        //            sw.WriteLine("A326,29,2,2,1,1,N,\"SRD.WORLD\"");
        //            sw.WriteLine("A525,185,2,3,1,1,N,\"WHITE CROWN\"");
        //            sw.WriteLine("LO21,155,508,1");
        //            sw.WriteLine("A155,185,2,3,1,1,N,\":" + Val.ToString(DRow["GIRDLEDESC"]) + "\"");
        //            sw.WriteLine("A124,146,2,3,1,1,N,\":" + Val.ToString(DRow["PAVANGLE"]) + "\"");
        //            sw.WriteLine("A91,85,2,3,1,1,N,\":" + Val.ToString(DRow["COLORSHADENAME"]) + "\"");
        //            sw.WriteLine("LO21,92,508,1");
        //            sw.WriteLine("LO23,35,508,1");
        //            sw.WriteLine("P1");
        //        }
        //        sw.Dispose();
        //        sw = null;
        //        if (File.Exists(Application.StartupPath + "\\PRINTBARCODE.BAT") && File.Exists(fileLoc))
        //        {
        //            Microsoft.VisualBasic.Interaction.Shell(Application.StartupPath + "\\PRINTBARCODE.BAT " + fileLoc, AppWinStyle.Hide, true, -1);
        //        }

        //        Thread.Sleep(800);


        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Message(ex.Message);
        //    }
        //}

        public static void BombayPrintBarcodePrint(DataRow DRow)
        {
            AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

            try
            {
                // First page

                string fileLoc = Application.StartupPath + "\\PrintBarcodeDataBombay.txt";
                if (System.IO.File.Exists(fileLoc) == true)
                {
                    System.IO.File.Delete(fileLoc);
                }

                System.IO.File.Create(fileLoc).Dispose();

                StreamWriter sw = new StreamWriter(fileLoc);
                using (sw)
                {
                    //sw.WriteLine("I8,A");
                    //sw.WriteLine("ZN");
                    //sw.WriteLine("q548");
                    //sw.WriteLine("O");
                    //sw.WriteLine("JF");
                    //sw.WriteLine("KIZZQ0");
                    //sw.WriteLine("KI9+0.0");
                    //sw.WriteLine("ZT");
                    //sw.WriteLine("Q288,B25");
                    //sw.WriteLine("Arglabel 360 31");
                    //sw.WriteLine("exit");
                    //sw.WriteLine("N");

                    //sw.WriteLine("B524,269,2,1,2,4,51,N,\"" + Val.ToString(DRow["SERIALNO"]) + "\"");
                    //sw.WriteLine("A218,272,2,3,1,1,N,\"SKE\"");
                    //sw.WriteLine("A173,273,2,3,1,1,N,\":" + Val.ToString(DRow["SERIALNO"]) + "\"");
                    //sw.WriteLine("A218,241,2,3,1,1,N,\"ID\"");
                    //sw.WriteLine("A173,240,2,3,1,1,N,\":" + Val.ToString(DRow["PARTYSTOCKNO"]) + "\"");
                    //sw.WriteLine("A524,78,2,3,1,1,N,\"" + Val.ToString(DRow["LABNAME"]) + "\"");
                    //sw.WriteLine("A463,78,2,3,1,1,N,\":" + Val.ToString(DRow["LABREPORTNO"]) + "\"");
                    //sw.WriteLine("A170,111,2,3,1,1,N,\"FL\"");
                    //sw.WriteLine("A120,111,2,3,1,1,N,\":" + Val.ToString(DRow["FLNAME"]) + "\"");
                    //sw.WriteLine("A524,46,2,3,1,1,N,\"MEAS\"");
                    //sw.WriteLine("A463,46,2,3,1,1,N,\":" + Val.ToString(DRow["MEASUREMENT"]) + "\"");
                    //sw.WriteLine("A463,168,2,3,1,1,N,\":" + Val.ToString(DRow["CARAT"]) + "\"");
                    //sw.WriteLine("A524,168,2,3,1,1,N,\"CTS\"");
                    //sw.WriteLine("A170,198,2,3,1,1,N,\"CUT\"");
                    //sw.WriteLine("A122,198,2,3,1,1,N,\":" + Val.ToString(DRow["CUTNAME"]) + "\"");
                    //sw.WriteLine("A122,168,2,3,1,1,N,\":" + Val.ToString(DRow["POLNAME"]) + "\"");
                    //sw.WriteLine("A170,168,2,3,1,1,N,\"POL\"");
                    //sw.WriteLine("A122,139,2,3,1,1,N,\":" + Val.ToString(DRow["SYMNAME"]) + "\"");
                    //sw.WriteLine("A170,139,2,3,1,1,N,\"SYM\"");
                    //sw.WriteLine("A170,78,2,3,1,1,N,\"TD%\"");
                    //sw.WriteLine("A120,78,2,3,1,1,N,\":" + Val.ToString(DRow["DEPTHPER"]) + "\"");
                    //sw.WriteLine("A118,46,2,3,1,1,N,\":" + Val.ToString(DRow["TABLEPER"]) + "\"");
                    //sw.WriteLine("A170,46,2,3,1,1,N,\"TB%\"");
                    //sw.WriteLine("LO2,207,543,2");
                    //sw.WriteLine("A524,198,2,3,1,1,N,\"SHP\"");
                    //sw.WriteLine("A463,198,2,3,1,1,N,\":" + Val.ToString(DRow["SHAPENAME"]) + "\"");
                    //sw.WriteLine("A524,139,2,3,1,1,N,\"COL\"");
                    //sw.WriteLine("A463,139,2,3,1,1,N,\":" + Val.ToString(DRow["COLORNAME"]) + "\"");
                    //sw.WriteLine("A524,111,2,3,1,1,N,\"CLA\"");
                    //sw.WriteLine("A463,111,2,3,1,1,N,\":" + Val.ToString(DRow["CLARITYNAME"]) + "\"");
                    //sw.WriteLine("P1");

                    sw.WriteLine("SIZE 67.4 mm, 30 mm");
                    sw.WriteLine("GAP 3 mm, 0 mm");
                    sw.WriteLine("DIRECTION 0,0");
                    sw.WriteLine("REFERENCE 0,0");
                    sw.WriteLine("OFFSET 0 mm");
                    sw.WriteLine("SET PEEL OFF");
                    sw.WriteLine("SET CUTTER OFF");
                    sw.WriteLine("SET PARTIAL_CUTTER OFF");
                    sw.WriteLine("SET TEAR ON");
                    sw.WriteLine("CLS");
                    sw.WriteLine("BAR 538,167, 1, 3");
                    sw.WriteLine("CODEPAGE 1252");

                    sw.WriteLine("TEXT 528,45,\"2\",180,1,1,\"DIA\"");
                    sw.WriteLine("TEXT 428,45,\"2\",180,1,1,\"" + Val.ToString(DRow["MEASUREMENT"]) + "\"");
                    sw.WriteLine("TEXT 529,102,\"2\",180,1,1,\"DEPTH\"");
                    sw.WriteLine("TEXT 428,102,\"2\",180,1,1,\"" + Val.ToString(DRow["DEPTHPER"]) + "\"");
                    sw.WriteLine("TEXT 528,160,\"2\",180,1,1,\"COL\"");
                    sw.WriteLine("TEXT 428,160,\"2\",180,1,1,\"" + Val.ToString(DRow["COLORNAME"]) + "\"");
                    sw.WriteLine("TEXT 528,131,\"2\",180,1,1,\"CLA\"");
                    sw.WriteLine("TEXT 428,133,\"2\",180,1,1,\"" + Val.ToString(DRow["CLARITYNAME"]) + "\"");
                    //sw.WriteLine("TEXT 184,17,\"2\",180,1,1,\"DAT\"");
                    //sw.WriteLine("TEXT 136,17,\"2\",180,1,1,\":\"");
                    //sw.WriteLine("TEXT 118,17,\"2\",180,1,1,\"" + Val.ToString(DRow["REPORTDATE"]) + "\"");
                    sw.WriteLine("TEXT 184,17,\"2\",180,1,1,\"" + Val.ToString(DRow["REPORTDATE"]) + "\"");
                    sw.WriteLine("TEXT 528,185,\"2\",180,1,1,\"Weight\"");
                    sw.WriteLine("TEXT 428,185,\"2\",180,1,1,\"" + Val.ToString(DRow["CARAT"]) + "\"");

                    if (Val.ToInt(DRow["ISCUTNAMEDISPLAY"]) == 1) //When Stone Have Fancy Shape or Fancy Color then Display Ratio Otherwise Display Cut Detail
                    {
                        sw.WriteLine("TEXT 184,131,\"2\",180,1,1,\"CUT\"");
                    }
                    else
                    {
                        sw.WriteLine("TEXT 184,131,\"2\",180,1,1,\"RTO\"");
                    }

                    sw.WriteLine("TEXT 184,102,\"2\",180,1,1,\"POL\"");
                    sw.WriteLine("TEXT 184,75,\"2\",180,1,1,\"SYM\"");
                    sw.WriteLine("TEXT 184,45,\"2\",180,1,1,\"FLO\"");
                    //sw.WriteLine("TEXT 528,17,\"2\",180,1,1,\"GIA\"");
                    sw.WriteLine("TEXT 528,17,\"2\",180,1,1,\"" + Val.ToString(DRow["LABNAME"]) + "\"");
                    sw.WriteLine("TEXT 529,72,\"2\",180,1,1,\"TABLE\"");
                    sw.WriteLine("TEXT 136,44,\"2\",180,1,1,\":\"");
                    sw.WriteLine("TEXT 136,75,\"2\",180,1,1,\":\"");
                    sw.WriteLine("TEXT 136,131,\"2\",180,1,1,\":\"");
                    sw.WriteLine("TEXT 136,102,\"2\",180,1,1,\":\"");

                    if (Val.ToInt(DRow["ISCUTNAMEDISPLAY"]) == 1) //When Stone Have Fancy Shape or Fancy Color then Display Ratio Otherwise Display Cut Detail
                    {
                        sw.WriteLine("TEXT 118,131,\"2\",180,1,1,\"" + Val.ToString(DRow["CUTNAME"]) + "\"");
                    }
                    else
                    {
                        sw.WriteLine("TEXT 118,131,\"2\",180,1,1,\"" + Val.ToString(DRow["RATIO"]) + "\"");
                    }

                    sw.WriteLine("TEXT 118,102,\"2\",180,1,1,\"" + Val.ToString(DRow["POLNAME"]) + "\"");
                    sw.WriteLine("TEXT 118,75,\"2\",180,1,1,\"" + Val.ToString(DRow["SYMNAME"]) + "\"");
                    sw.WriteLine("TEXT 117,45,\"2\",180,1,1,\"" + Val.ToString(DRow["FLNAME"]) + "\"");
                    sw.WriteLine("TEXT 428,17,\"2\",180,1,1,\"" + Val.ToString(DRow["LABREPORTNO"]) + "\"");
                    sw.WriteLine("TEXT 428,74,\"2\",180,1,1,\"" + Val.ToString(DRow["TABLEPER"]) + "\"");
                    sw.WriteLine("BARCODE 233,181,\"128M\",36,0,180,1,2,\"" + Val.ToString(DRow["PARTYSTOCKNO"]) + "\"");

                    sw.WriteLine("TEXT 162,207,\"2\",180,1,1,\"" + Val.ToString(DRow["SHAPENAME"]) + "\"");
                    //sw.WriteLine("TEXT 162,217,\"3\",180,1,1,\"" + Val.ToString(DRow["SHAPENAME"]) + " (" + Val.ToString(DRow["CARAT"]) + ") \"");
                    sw.WriteLine("TEXT 532,217,\"3\",180,1,1,\"" + Val.ToString(DRow["PARTYSTOCKNO"]) + "\"");
                    sw.WriteLine("TEXT 442,185,\"2\",180,1,1,\":\"");
                    sw.WriteLine("TEXT 442,160,\"2\",180,1,1,\":\"");
                    sw.WriteLine("TEXT 442,133,\"2\",180,1,1,\":\"");
                    sw.WriteLine("TEXT 442,45,\"2\",180,1,1,\":\"");
                    sw.WriteLine("TEXT 442,102,\"2\",180,1,1,\":\"");
                    sw.WriteLine("TEXT 442,74,\"2\",180,1,1,\":\"");
                    sw.WriteLine("TEXT 442,17,\"2\",180,1,1,\":\"");
                    sw.WriteLine("PRINT 1,1");
                }
                sw.Dispose();
                sw = null;
                if (File.Exists(Application.StartupPath + "\\PRINTBARCODE_STOCK.BAT") && File.Exists(fileLoc))
                {
                    Microsoft.VisualBasic.Interaction.Shell(Application.StartupPath + "\\PRINTBARCODE_STOCK.BAT " + fileLoc, AppWinStyle.Hide, true, -1);
                }
                Thread.Sleep(800);
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
        public static void CertificateBarcodePrint(DataRow Dr)
        {
            try
            {
                AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
                string fileLoc = Application.StartupPath + "\\PrintCertificateBarcodeData.txt";
                if (System.IO.File.Exists(fileLoc) == true)
                {
                    System.IO.File.Delete(fileLoc);
                }

                System.IO.File.Create(fileLoc).Dispose();

                StreamWriter sw = new StreamWriter(fileLoc);
                using (sw)
                {
                    //sw.WriteLine("I8,A");
                    //sw.WriteLine("ZN");
                    //sw.WriteLine("q400");
                    //sw.WriteLine("O");
                    //sw.WriteLine("JF");
                    //sw.WriteLine("KIZZQ0");
                    //sw.WriteLine("KI9+0.0");
                    //sw.WriteLine("ZT");
                    //sw.WriteLine("Q120,25");
                    //sw.WriteLine("Arglabel 150 31");
                    //sw.WriteLine("exit");
                    //sw.WriteLine("KI80");
                    //sw.WriteLine("N");
                    //sw.WriteLine("B351,87,2,1,2,4,51,N,\"" + Val.ToString(Dr["SERIALNO"]) + "\"");
                    //sw.WriteLine("A351,111,2,3,1,1,N,\"" + Val.ToString(Dr["PARTYSTOCKNO"]) + "\"");
                    //sw.WriteLine("A145,109,2,3,1,1,N,\"" + "SKE:" + Val.ToString(Dr["SERIALNO"]) + "\"");
                    //sw.WriteLine("A343,28,2,3,1,1,N,\"" + Val.ToString(Dr["LABREPORTNO"]) + "\"");
                    //sw.WriteLine("A145,28,2,3,1,1,N,\"" + Val.ToString(Dr["EXPORTORDERNO"]) + "\"");
                    //sw.WriteLine("P1");

                    sw.WriteLine("I8,A,001");
                    sw.WriteLine("Q113,024");
                    sw.WriteLine("q831");
                    sw.WriteLine("rN");
                    sw.WriteLine("S5");
                    sw.WriteLine("D10");
                    sw.WriteLine("ZT");
                    sw.WriteLine("JF");
                    sw.WriteLine("O");
                    sw.WriteLine("R273,0");
                    sw.WriteLine("f100");
                    sw.WriteLine("N");
                    sw.WriteLine("A245,104,2,4,1,1,N,\"" + Val.ToString(Dr["PARTYSTOCKNO"]) + "\"");
                    //sw.WriteLine("B278,76,2,1,2,6,37,N,\"" + Val.ToString(Dr["PARTYSTOCKNO"]) + "\"");
                    sw.WriteLine("B248,76,2,1,1,3,37,N,\"" + Val.ToString(Dr["PARTYSTOCKNO"]) + "\"");
                    sw.WriteLine("A277,28,2,3,1,1,N,\"" + Val.ToString(Dr["LABREPORTNO"]) + "\"");
                    sw.WriteLine("A91,28,2,3,1,1,N,\"" + Val.ToString(Dr["CARAT"]) + "\"");
                    sw.WriteLine("P1");

                }
                sw.Dispose();
                sw = null;
                if (File.Exists(Application.StartupPath + "\\PRINTBARCODE_CERTI.BAT") && File.Exists(fileLoc))
                {
                    Microsoft.VisualBasic.Interaction.Shell(Application.StartupPath + "\\PRINTBARCODE_CERTI.BAT " + fileLoc, AppWinStyle.Hide, true, -1);
                }

                Thread.Sleep(800);

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
        public static void MFGGradingBarcodePrint(DataRow Dr) //TSC Printer : Width Long Barcodse
        {
            try
            {
                AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
                string fileLoc = Application.StartupPath + "\\PrintMFGGradingBarcodeData.txt";
                if (System.IO.File.Exists(fileLoc) == true)
                {
                    System.IO.File.Delete(fileLoc);
                }

                System.IO.File.Create(fileLoc).Dispose();

                string StrColorName = Dr["COLORNAME"].ToString().Trim().Equals(string.Empty) ? "-" : Dr["COLORNAME"].ToString();
                string StrClarityName = Dr["CLARITYNAME"].ToString().Trim().Equals(string.Empty) ? "-" : Dr["CLARITYNAME"].ToString();
                string StrCutName = Dr["CUTNAME"].ToString().Trim().Equals(string.Empty) ? "-" : Dr["CUTNAME"].ToString();
                string StrPolName = Dr["POLNAME"].ToString().Trim().Equals(string.Empty) ? "-" : Dr["POLNAME"].ToString();
                string StrSymName = Dr["SYMNAME"].ToString().Trim().Equals(string.Empty) ? "-" : Dr["SYMNAME"].ToString();
                string StrFLName = Dr["FLNAME"].ToString().Trim().Equals(string.Empty) ? "-" : Dr["FLNAME"].ToString();

                string StrTDPer = Dr["DEPTHPER"].ToString().Trim().Equals(string.Empty) ? "" : Dr["DEPTHPER"].ToString();
                string StrTBPer = Dr["TABLEPER"].ToString().Trim().Equals(string.Empty) ? "" : Dr["TABLEPER"].ToString();

                string StrPartyStockNo = Dr["PARTYSTOCKNO"].ToString();
                string StrRatio = Dr["RATIO"].ToString().Trim().Equals(string.Empty) || Val.Val(Dr["RATIO"]) == 0 ? "" : "(" + Dr["RATIO"].ToString() + ")";

                string StrParamDetail = "(" + StrColorName + "-" + StrClarityName + ") (" + StrCutName + "-" + StrPolName + "-" + StrSymName + "-" + StrFLName + ")";
                StrParamDetail = StrParamDetail + " (TD = " + StrTDPer + "%) (TB = " + StrTBPer + "%)";

                StreamWriter sw = new StreamWriter(fileLoc);
                using (sw)
                {
                    sw.WriteLine("<xpml><page quantity='0' pitch='15.5 mm'></xpml>SIZE 57.5 mm, 15.5 mm");
                    sw.WriteLine("GAP 2.5 mm, 0 mm");
                    sw.WriteLine("DIRECTION 0,0");
                    sw.WriteLine("REFERENCE 0,0");
                    sw.WriteLine("OFFSET 0 mm");
                    sw.WriteLine("SET PEEL OFF");
                    sw.WriteLine("SET CUTTER OFF");
                    sw.WriteLine("SET PARTIAL_CUTTER OFF");
                    sw.WriteLine("<xpml></page></xpml><xpml><page quantity='1' pitch='15.5 mm'></xpml>SET TEAR ON");
                    sw.WriteLine("CLS");
                    sw.WriteLine("CODEPAGE 1252");
                    sw.WriteLine("TEXT 455,103,\"0\",180,8,9,\"" + StrParamDetail + "\"");
                    sw.WriteLine("BARCODE 368,76,\"128M\",49,0,180,2,4,\"" + StrPartyStockNo + "\"");
                    sw.WriteLine("TEXT 292,26,\"0\",180,11,9,\"" + StrPartyStockNo + "\"");
                    sw.WriteLine("TEXT 78,25,\"0\",180,11,9,\"" + Val.ToString(Dr["CARAT"]) + "\"");
                    sw.WriteLine("TEXT 455,26,\"0\",180,11,9,\"" + Val.ToString(Dr["SHAPENAME"]) + "\"");
                    sw.WriteLine("TEXT 455,63,\"0\",180,8,7,\"" + StrRatio + "\"");
                    sw.WriteLine("TEXT 140,63,\"0\",180,11,9,\"" + Val.ToString(Dr["PACKETNO"]) + "\"");
                    sw.WriteLine("PRINT 1,1");
                    sw.WriteLine("<xpml></page></xpml><xpml><end/></xpml>");
                }
                sw.Dispose();
                sw = null;
                if (File.Exists(Application.StartupPath + "\\PRINTBARCODE.BAT") && File.Exists(fileLoc))
                {
                    Microsoft.VisualBasic.Interaction.Shell(Application.StartupPath + "\\PRINTBARCODE.BAT " + fileLoc, AppWinStyle.Hide, true, -1);
                }

                Thread.Sleep(800);

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        public static string NumbersToWords(int inputNumber)
        {
            int inputNo = inputNumber;

            if (inputNo == 0)
                return "Zero";

            int[] numbers = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (inputNo < 0)
            {
                sb.Append("Minus ");
                inputNo = -inputNo;
            }

            string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ",
        "Five " ,"Six ", "Seven ", "Eight ", "Nine "};
            string[] words1 = {"Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ",
        "Fifteen ","Sixteen ","Seventeen ","Eighteen ", "Nineteen "};
            string[] words2 = {"Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ",
        "Seventy ","Eighty ", "Ninety "};
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };

            numbers[0] = inputNo % 1000; // units
            numbers[1] = inputNo / 1000;
            numbers[2] = inputNo / 100000;
            numbers[1] = numbers[1] - 100 * numbers[2]; // thousands
            numbers[3] = inputNo / 10000000; // crores
            numbers[2] = numbers[2] - 100 * numbers[3]; // lakhs

            for (int i = 3; i > 0; i--)
            {
                if (numbers[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (numbers[i] == 0) continue;
                u = numbers[i] % 10; // ones
                t = numbers[i] / 10;
                h = numbers[i] / 100; // hundreds
                t = t - 10 * h; // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i == 0) sb.Append("and ");
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd();

        }

        public static string GetFinancialYear(string pStrDate)
        {
            AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
            string StrReturn = "";
            DateTime DT = DateTime.Parse(pStrDate);
            if (DT.Month >= 4 && DT.Month <= 12)
            {
                StrReturn = Val.Right(DT.Year.ToString(), 2) + "" + Val.Right((DT.Year + 1).ToString(), 2);
            }
            else
            {
                StrReturn = Val.Right((DT.Year - 1).ToString(), 2) + "" + Val.Right((DT.Year).ToString(), 2);
            }

            Val = null;
            return StrReturn;
        }
        public static void OpenPopupParam(string StrTableName, KeyPressEventArgs e, AxonContLib.cTextBox txt, Int64 pIntParentID = 0)
        {
            AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
            if (OnKeyPressEveToPopup(e))
            {
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "SHORTNAME,PARAMVALUE";
                FrmSearch.mDTab = new BOComboFill().FillCmb(StrTableName, pIntParentID);
                FrmSearch.mBoolAllowFirstColumnHide = true;
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                FrmSearch.mStrColumnsToHide = "PARAM_ID";
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                e.Handled = true;
                FrmSearch.ShowDialog();

                if (FrmSearch.DRow != null)
                {
                    txt.Tag = Val.ToString(FrmSearch.DRow["PARAM_ID"]);
                    txt.Text = Val.ToString(FrmSearch.DRow["PARAMVALUE"]);
                }
                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
                Val = null;
            }
        }
        public static XmlDocument ConvertToXml(Object list)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(list.GetType());
            using (MemoryStream xmlStream = new MemoryStream())
            {
                xmlSerializer.Serialize(xmlStream, list);
                xmlStream.Position = 0;
                xmlDoc.Load(xmlStream);
                return xmlDoc;
            }
        }

        public static string ExportToCSV(DataTable dt, string strFilePath)
        {
            var sw = new StreamWriter(strFilePath, false);

            // Write the headers.
            int iColCount = dt.Columns.Count;
            for (int i = 0; i < iColCount; i++)
            {

                sw.Write(dt.Columns[i].Caption);
                if (i < iColCount - 1) sw.Write(",");
            }

            sw.Write(sw.NewLine);

            // Write rows.
            foreach (DataRow dr in dt.Rows)
            {
                for (int i = 0; i < iColCount; i++)
                {
                    if (!Convert.IsDBNull(dr[i]))
                    {
                        //if (dr[i].ToString().StartsWith("0"))
                        //{
                        //    sw.Write(@"=""" + dr[i] + @"""");
                        //}
                        //else
                        //{
                        //    sw.Write(dr[i].ToString());
                        //}

                        // to remove junk characters

                        string sData = dr[i].ToString();
                        sData = sData.Replace("<br>", " ");
                        sData = sData.Replace("<br/>", " ");
                        sData = sData.Replace("\n", " ");
                        sData = sData.Replace("\t", " ");
                        sData = sData.Replace("\r", " ");
                        sData = sData.Replace("\n", " ");

                        if (sData.Contains("\""))
                        {
                            sData = sData.Replace("\"", "\"\"");
                        }

                        if (sData.Contains(","))
                        {
                            sData = String.Format("\"{0}\"", sData);
                        }

                        if (sData.Contains(System.Environment.NewLine))
                        {
                            sData = String.Format("\"{0}\"", sData);
                        }
                        //sData = sData.Replace(",", ":");

                        dr[i] = sData;
                        // dr[i]=dr[i].ToString().Replace(",", ":");

                        sw.Write(dr[i]); //.ToString()
                    }

                    if (i < iColCount - 1) sw.Write(",");
                }
                sw.Write(sw.NewLine);
            }

            sw.Close();

            return strFilePath;
        }

        public static void ExcelExport(string pStrFileName, DevExpress.XtraGrid.Views.Grid.GridView pGrid)
        {
            try
            {
                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = "xlsx";
                svDialog.Title = "Export to Excel";
                svDialog.FileName = pStrFileName;
                svDialog.Filter = "Excel files 2007(*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    string Filepath = svDialog.FileName;
                    pGrid.ExportToXlsx(Filepath);

                    if (Confirm("Do You Want To Open The File ? ") == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(Filepath, "CMD");
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        public static void ExcelExport(string pStrFileName, DevExpress.XtraPivotGrid.PivotGridControl pGrid)
        {
            try
            {
                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = "xlsx";
                svDialog.Title = "Export to Excel";
                svDialog.FileName = pStrFileName;
                svDialog.Filter = "Excel files 2007(*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    string Filepath = svDialog.FileName;
                    pGrid.ExportToXlsx(Filepath);
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }



        public static bool OnKeyPressEveToPopup(KeyPressEventArgs e)
        {
            if (e.KeyChar != (Char)Keys.Enter && e.KeyChar != (Char)Keys.Delete && e.KeyChar != (Char)Keys.Back
                && e.KeyChar != (Char)Keys.Left && e.KeyChar != (Char)Keys.Escape && e.KeyChar != (Char)Keys.Right
                && e.KeyChar != (Char)Keys.Tab && e.KeyChar != (Char)Keys.Up
                )
            {
                return true;
            }
            return false;
        }

        public static bool OnKeyPressEveToPopup(KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete && e.KeyCode != Keys.Enter
                && e.KeyCode != Keys.Back && e.KeyCode != Keys.Right
                && e.KeyCode != Keys.Left && e.KeyCode != Keys.Escape && e.KeyCode != Keys.Up
               )
            {
                return true;
            }
            return false;
        }

        private static readonly string[] unitsMap = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        private static readonly string[] tensMap = { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        private static string ConvertIntegerToWords(long number)
        {
            if (number == 0)
                return "Zero";

            if (number < 0)
                return "Minus " + ConvertIntegerToWords(Math.Abs(number));

            string words = "";

            if ((number / 10000000) > 0)
            {
                words += ConvertIntegerToWords(number / 10000000) + " Crore ";
                number %= 10000000;
            }

            if ((number / 100000) > 0)
            {
                words += ConvertIntegerToWords(number / 100000) + " Lakh ";
                number %= 100000;
            }

            if ((number / 1000) > 0)
            {
                words += ConvertIntegerToWords(number / 1000) + " Thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += ConvertIntegerToWords(number / 100) + " Hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words.Trim();
        }

        public static string ConvertAmountToWords(double amount)
        {
            if (amount == 0)
                return "Zero Rupees";

            // Split integer and decimal parts
            long integerPart = (long)Math.Floor(amount);
            int decimalPart = (int)((amount - integerPart) * 100); // for paise

            string words = ConvertIntegerToWords(integerPart) + " Rupees";

            if (decimalPart > 0)
            {
                words += " and " + ConvertIntegerToWords(decimalPart) + " Paise";
            }

            return words;
        }
        public static string TextEncrypt(string plainText)
        {
            try
            {
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

                byte[] keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
                var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
                var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

                byte[] cipherTextBytes;

                using (var memoryStream = new MemoryStream())
                {
                    using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                        cryptoStream.FlushFinalBlock();
                        cipherTextBytes = memoryStream.ToArray();
                        cryptoStream.Close();
                    }
                    memoryStream.Close();
                }
                return Convert.ToBase64String(cipherTextBytes);
            }
            catch (Exception ex)
            {
                return "";
            }

        }


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

        public static void MessageToster(string pStrMessage)
        {
            gStrMessage = pStrMessage;
        }

        public static void Message(string pStrMessage)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show(pStrMessage.ToUpper(), gStrCompanyName, MessageBoxButtons.OK, MessageBoxIcon.Information);
            //   MessageBox.Show(pStrMessage.ToUpper(), "Account", MessageBoxButtons.OK);
        }

        public static void MessageError(string pStrMessage)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show(pStrMessage.ToUpper(), gStrCompanyName, MessageBoxButtons.OK, MessageBoxIcon.Error);
            //   MessageBox.Show(pStrMessage.ToUpper(), "Account", MessageBoxButtons.OK);
        }

        public static DialogResult Confirm(string pStrMessage)
        {
            return DevExpress.XtraEditors.XtraMessageBox.Show(pStrMessage.ToUpper(), gStrCompanyName, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
        public static string FindLedgerClosingStr(Guid pIntLedgerID) //Add : HinaB
        {
            double DouAmt = new BusLib.Account.BOLedgerTransaction().FindLedgerClosing(pIntLedgerID);
            if (DouAmt < 0)
            {
                return DouAmt.ToString() + " Dr";
            }
            else
            {
                return DouAmt.ToString() + " Cr";
            }
        }
    }
}