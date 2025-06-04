using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spire.Xls;
using System.Diagnostics;
using OfficeOpenXml;
using System.Net;
using System.Collections.Specialized;
using System.Threading;

using System.Configuration;
using System.Runtime.InteropServices;
using System.Reflection;
using GhostscriptSharp;
using WinAxoneActivity;
using BusLib.Utility;



namespace MahantExport
{
    public partial class FrmImageVideoCertiUrlUpdate  : DevControlLib.cDevXtraForm
    {
        [DllImportAttribute("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int SetProcessWorkingSetSize(IntPtr process, int minimumWorkingSetSize, int maximumWorkingSetSize);

        public delegate void SetControlValueCallback(Control oControl, string propName, object propValue);

        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        string mStrOpe = "";

        string IpAdderess = "121.0.0.0";

        public FrmImageVideoCertiUrlUpdate()
        {
            InitializeComponent();
        }

        #region Timer

        private void TimerCertificate_Tick(object sender, EventArgs e)
        {
            try
            {
                if (DateTime.Now.ToString("HH:mm") == System.Configuration.ConfigurationManager.AppSettings["CertificateDownloadTime"].ToString())
                {
                    if (backgroundWorker1.IsBusy)
                    {
                        return;
                    }
                    TimerCertificate.Enabled = false;
                    Btn_CertDownload.Enabled = false;
                    backgroundWorker1.RunWorkerAsync();
                }
            }
            catch (Exception EX)
            {
                Global.Message(EX.Message.ToString());
                Btn_CertDownload.Enabled = true;
            }

        }


        #endregion

        #region image/certi

        private void FreezMemory()
        {
            try
            {
                //  iDispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();

                if (
                    Environment.OSVersion.Platform == PlatformID.Win32NT ||
                    Environment.OSVersion.Platform == PlatformID.Win32S ||
                    Environment.OSVersion.Platform == PlatformID.Win32Windows ||
                    Environment.OSVersion.Platform == PlatformID.WinCE
                    )
                {
                    SetProcessWorkingSetSize(System.Diagnostics.Process.GetCurrentProcess().Handle, -1, -1);
                }
            }
            catch
            {

            }
        }

        public int URLExists(string url)
        {
            int exist = 0;
            try
            {
                HttpWebRequest request = (HttpWebRequest)System.Net.WebRequest.Create(url);
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        exist = 1;
                    }

                }
                request = null;
            }
            catch
            {
                exist = 0;
            }
            return exist;
        }

        public void DownloadCertificateNewMethod()
        {
            //Certificate servies
            try
            {
                //SqlConnection con = new SqlConnection(SqlHelper.GetConnection());
                //SqlCommand cmd = new SqlCommand();
                //cmd.Connection = con;
                //if (cmd.Connection.State == ConnectionState.Closed)
                //{
                //    cmd.Connection.Open();
                //}
                //cmd.Parameters.Clear();
                //cmd.Parameters.Add(new SqlParameter("OPE", mStrOpe));
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.CommandText = "Win_ImageVideoCertDownload";
                //cmd.CommandTimeout = 9999999;

                //SqlDataAdapter MyAdapter1 = new SqlDataAdapter(cmd);
                //DataTable DTab = new DataTable();
                //MyAdapter1.Fill(DTab);

                DataTable DTab = new DataTable();
                DTab = new BOMST_ImageCertiFlagUpdate().GetCertImageVideoFlagUpdateStoneDetail(mStrOpe);

                int IntTotalStone = 0;

                int IntCount1 = 1;
                foreach (DataRow DR in DTab.Rows)
                {
                    string StrPacketID = Val.ToString(DR["Stock_ID"]);
                    string StrPartyStockNo = Val.ToString(DR["PartyStockNo"]);

                    string StrImageUrl = Val.ToString(DR["IMAGEURL"]);
                    string StrJSONUrl = Val.ToString(DR["JSONURL"]);
                    string StrHTMLUrl = Val.ToString(DR["HTMLURL"]);
                    string StrMP4Url = Val.ToString(DR["MP4URL"]);
                    string StrMOVUrl = Val.ToString(DR["MOVURL"]);
                    string StrCertUrl = Val.ToString(DR["CERTURL"]);
                    string StrMediaURL = Val.ToString(DR["MediaURL"]);

                    string StrUpdateImage = string.Empty;
                    string StrUpdateVideo = string.Empty;
                    string StrUpdateVideoDownload = string.Empty;
                    string StrUpdateCertificate = string.Empty;
                    Boolean BlnExists = false;

                    string StrMessage = "StockNo : " + StrPartyStockNo + "  (" + IntCount1 + "/" + DTab.Rows.Count.ToString() + ") Stones Image/Certi/Video Flags Updates Start";
                    SetControlPropertyValue(lblMessage, "Text", StrMessage);



                    int ISImageExists = URLExists(StrImageUrl);
                    if (ISImageExists == 1)
                    {
                        BlnExists = true;
                        StrUpdateImage = StrImageUrl.Replace(StrMediaURL, "");
                    }
                    else
                    {
                        StrUpdateImage = "/Vision360/NoMedia/noimage.jpg";
                    }

                    int ISvideoExists = URLExists(StrJSONUrl);
                    if (ISvideoExists == 1)
                    {
                        BlnExists = true;
                        StrUpdateVideo = "../vision360/vision360.html?d=" + StrPartyStockNo + "&s=0";
                        StrUpdateVideoDownload = "/vision360/imaged/" + StrPartyStockNo + "/video.mp4";
                    }
                    else
                    {
                        ISvideoExists = URLExists(StrHTMLUrl);
                        if (ISvideoExists == 1)
                        {
                            BlnExists = true;
                            StrUpdateVideo = StrHTMLUrl.Replace(StrMediaURL, "");
                            StrUpdateVideoDownload = "/vision360/imaged/" + StrPartyStockNo + "/video.mp4";
                        }
                        else
                        {
                            ISvideoExists = URLExists(StrMP4Url);
                            if (ISvideoExists == 1)
                            {
                                BlnExists = true;
                                StrUpdateVideo = StrMP4Url.Replace(StrMediaURL, "");
                                StrUpdateVideoDownload = "/vision360/imaged/" + StrPartyStockNo + "/video.mp4";

                            }
                            else
                            {
                                ISvideoExists = URLExists(StrMOVUrl);
                                if (ISvideoExists == 1)
                                {
                                    BlnExists = true;
                                    StrUpdateVideo = StrMOVUrl.Replace(StrMediaURL, "");
                                    StrUpdateVideoDownload = "/vision360/imaged/" + StrPartyStockNo + "/video.mp4";
                                }
                                else
                                {
                                    StrUpdateVideo = "../vision360/NoMedia/novideo.pdf";
                                    StrUpdateVideoDownload = "/vision360/NoMedia/novideo.pdf";
                                }
                            }
                        }
                    }


                    int ISCertExists = URLExists(StrCertUrl);
                    if (ISCertExists == 1)
                    {
                        BlnExists = true;
                        StrUpdateCertificate = StrCertUrl.Replace(StrMediaURL, "");
                    }
                    else
                    {
                        StrUpdateCertificate = "/Vision360/NoMedia/nocertificate.pdf";
                    }


                    if (BlnExists == true)
                    {
                        IntTotalStone = IntTotalStone + 1;
                        string StrSql = "Alter Table Trn_Stock Disable Trigger Trg_Trn_Stock;";
                        StrSql = StrSql + " Update TRN_Stock With(Rowlock) Set";
                        StrSql = StrSql + " ISImage=" + ISImageExists + ",";
                        StrSql = StrSql + " ISCerti=" + ISCertExists + ",";
                        StrSql = StrSql + " ISVideo=" + ISvideoExists + ",";
                        StrSql = StrSql + " CertificateUrl='" + StrUpdateCertificate + "',";
                        StrSql = StrSql + " ImageUrl='" + StrUpdateImage + "',";
                        StrSql = StrSql + " VideoUrl='" + StrUpdateVideo + "',";
                        StrSql = StrSql + " DownloadVideoUrl='" + StrUpdateVideoDownload + "'";
                        StrSql = StrSql + " Where Stock_ID = '" + StrPacketID + "';";
                        StrSql = StrSql + " Alter Table Trn_Stock Enable Trigger Trg_Trn_Stock;";
                        int IntRes = new BOMST_ImageCertiFlagUpdate().UpdateImageCertiVideoFlag(StrSql);
                    }
                    IntCount1++;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
            finally
            {

            }
        }

        #endregion

        private void Btn_CertDownload_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
            TimerCertificate.Enabled = false;
            Btn_CertDownload.Enabled = false;
            progressPanel1.Visible = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[] 
                        {
                            oControl,
                            propName,
                            propValue
                        });
            }
            else
            {
                Type t = oControl.GetType();
                PropertyInfo[] props = t.GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if ((p.Name.ToUpper() == propName.ToUpper()))
                    {
                        p.SetValue(oControl, propValue, null);
                    }
                }
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //DownloadCertificate();
            DownloadCertificateNewMethod();
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            TimerCertificate.Enabled = true;
            Btn_CertDownload.Enabled = true;
            progressPanel1.Visible = false;
            SetControlPropertyValue(lblMessage, "Text", "Last Called : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"));

        }

        public string GetValue(string jsonString, string Key)
        {
            string StrReturnValue = "";
            string[] Split = jsonString.Replace(":{", ",").Replace("{", "").Replace("\"", "").Split(',');

            for (int IntI = 0; IntI < Split.Length; IntI++)
            {
                if (Split[IntI].ToLower().Contains(Key.ToLower()))
                {
                    StrReturnValue = Split[IntI].Replace(Key + ":", "");
                    break;
                }

            }
            return StrReturnValue;
        }

        private void ChkForceFullyUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkForceFullyUpdate.Checked == true)
            {
                mStrOpe = "ALL";
            }
            else
            {
                mStrOpe = "ONLYPENDING";
            }

        }

        private void FrmImageVideoCertiUrlUpdate_Load(object sender, EventArgs e)
        {
            ChkForceFullyUpdate_CheckedChanged(null, null);
        }

    }


}
