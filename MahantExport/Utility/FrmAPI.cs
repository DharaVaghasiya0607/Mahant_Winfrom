using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Globalization;
using System.IO;
using WinSCP;
using MahantExport.Class;
using Spire.Xls;
using System.Net;
using BusLib.Configuration;
using BusLib.Utility;


namespace MahantExport
{
    public partial class FrmAPI : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOMST_ApiSetting ObjMast = new BOMST_ApiSetting();
        BOFormPer ObjPer = new BOFormPer();

        public FrmAPI()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            AttachFormDefaultEvent();
            Val.FormGeneralSetting(this);

            //if (ObjPer.ISVIEW == false)
            //{
            //    Global.MessageError(BusLib.TPV.BOMessage.ViewDeniedMsg);
            //    return;
            //}
            //ObjMast.Fill();
            //SetDataBinding();
            this.Show();
        }
        private void AttachFormDefaultEvent()
        {
            Val.FormGeneralSetting(this);
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.FormKeyPress = true;

            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMast);

        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            DataTable DTab = ObjMast.GetAPISetting();
            MainGrid.DataSource = DTab;
            MainGrid.Refresh();
        }

        private void BtnSync_Click(object sender, EventArgs e)
        {
            if (GrdDet.FocusedRowHandle < 0)
            {
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            DataRow DRow = GrdDet.GetFocusedDataRow();

            int IntAPIID = Val.ToInt(DRow["API_ID"]);
            string StrUrl = Val.ToString(DRow["URL"]);
            string StrAPICode = Val.ToString(DRow["APICODE"]);
            string StrHostName = Val.ToString(DRow["HOSTNAME"]);
            string StrUserName = Val.ToString(DRow["USERNAME"]);
            string StrPassword = Val.ToString(DRow["PASSWORD"]);
            int IntPort = Val.ToInt(DRow["PORT"]);
            string StrCertificate = Val.ToString(DRow["CERTICATE"]);

            string StrResult = "Successfully Upload";
            string StrFileName = string.Empty;
            int IntTotalPcs = 0;
            double DouTotalCarat = 0;


            /*
            if (StrAPICode == "JA")
            {
                #region JA Upload

                try
                {
                    StrFileName = Application.StartupPath + "\\APIFile\\JA.csv";
                    DataTable DTab = ObjMast.GetJamelsAllenData("FULLSTOCK", null);

                    IntTotalPcs = DTab.Rows.Count;
                    DouTotalCarat = Val.Val(DTab.Compute("SUM(CARAT)", ""));

                    StrFileName = Global.ExportToCSV(DTab, StrFileName);

                    SessionOptions sessionOptions = new SessionOptions
                    {
                        Protocol = Protocol.Sftp,
                        HostName = StrHostName,
                        UserName = StrUserName,
                        Password = StrPassword,
                        PortNumber = IntPort,
                        SshHostKeyFingerprint = StrCertificate

                    };

                    using (Session session = new Session())
                    {
                        // Connect
                        session.DisableVersionCheck = true;
                        session.Open(sessionOptions);

                        // Upload files
                        TransferOptions transferOptions = new TransferOptions();
                        transferOptions.TransferMode = TransferMode.Binary;

                        TransferOperationResult transferResult;
                        transferResult = session.PutFiles(StrFileName, "/", false, transferOptions);
                        transferResult.Check();
                        session.Close();
                    }

                    // Put API Code Is Here

                    StrResult = "Successfully Upload";

                    Global.Message(StrResult);
                }
                catch (Exception Ex)
                {
                    this.Cursor = Cursors.Default;
                    StrResult = "Error : " + Ex.Message;
                    Global.Message(StrResult);
                }

                #endregion
            }
            */

            if (StrAPICode == "RAPNET")
            {
                #region Rapnet Upload

                try
                {
                    StrFileName = Application.StartupPath + "\\APIFile\\RapNet.csv";
                    DataTable DTab = ObjMast.GetRapnetData("FULLSTOCK", null);

                    IntTotalPcs = DTab.Rows.Count;
                    DouTotalCarat = Val.Val(DTab.Compute("SUM(Weight)", ""));

                    StrFileName = Global.ExportToCSV(DTab, StrFileName);

                    string URLAuth = "https://technet.rapaport.com/HTTP/Authenticate.aspx";

                    ExtendedWebClient webClient = new ExtendedWebClient(new Uri(URLAuth));

                    System.Collections.Specialized.NameValueCollection formData = new System.Collections.Specialized.NameValueCollection();

                    formData["Username"] = StrUserName;
                    formData["Password"] = StrPassword;

                    byte[] responseBytes = webClient.UploadValues(URLAuth, "POST", formData);
                    string ResultAuthTicket = Encoding.UTF8.GetString(responseBytes);

                    webClient.Dispose();
                    string URL = "http://technet.rapaport.com/HTTP/Upload/Upload.aspx?Method=file&ReplaceAll=true&ticket=" + ResultAuthTicket;
                    formData.Clear();

                    webClient = new ExtendedWebClient(new Uri(URL));
                    responseBytes = webClient.UploadFile(URL, "POST", StrFileName);
                    StrResult = Encoding.UTF8.GetString(responseBytes);
                    Global.Message(StrResult);

                }
                catch (Exception Ex)
                {
                    this.Cursor = Cursors.Default;
                    StrResult = "Error : " + Ex.Message;
                    Global.Message(StrResult);
                }

                #endregion
            }


            #region :: Comment ::
            /*
        else if (StrAPICode == "PG")
        {
            #region PolyGon Upload

            try
            {
                StrFileName = Application.StartupPath + "\\APIFile\\Polygon.csv";

                string PGFileName = Application.StartupPath + "\\APIFile\\158518_" + DateTime.Now.ToString("yyyyMMdd") + "_prod_c_DM.xls";
                //StrFileName = Application.StartupPath + "\\APIFile\\158518_" + DateTime.Now.ToString("yyyyMMdd") + "_prod_c_DM.xls";

                DataTable DTab = ObjMast.GetPolyGonData("FULLSTOCK", null);

                IntTotalPcs = DTab.Rows.Count;
                DouTotalCarat = Val.Val(DTab.Compute("SUM([Carat Weight])", ""));

                StrFileName = Global.ExportToCSV(DTab, StrFileName);

                if (File.Exists(PGFileName))
                {
                    File.Delete(PGFileName);
                }
                File.Copy(StrFileName, PGFileName);

                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = StrHostName,
                    UserName = StrUserName,
                    Password = StrPassword,
                    PortNumber = IntPort,
                    //SshHostKeyFingerprint = ""

                };

                //string FileName = "158518_" + DateTime.Now.ToString("yyyyMMdd") + "_prod_c_DM.xls"; ;

                using (Session session = new Session())
                {
                    // Connect
                    session.DisableVersionCheck = true;
                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult;
                    transferResult = session.PutFiles(PGFileName, "/", false, transferOptions);
                    transferResult.Check();
                    session.Close();
                }

                if (File.Exists(PGFileName))
                {
                    File.Delete(PGFileName);
                }

                // Put API Code Is Here

                StrResult = "Successfully Upload";

                Global.Message(StrResult);
            }
            catch (Exception Ex)
            {
                this.Cursor = Cursors.Default;
                StrResult = "Error : " + Ex.Message;
                Global.Message(StrResult);
            }

            #endregion
        }
        else if (StrAPICode == "LD")
        {
            #region Liquid Diamond Upload

            try
            {
                StrFileName = Application.StartupPath + "\\APIFile\\LiquidDiamond.csv";
                DataTable DTab = ObjMast.GetLiquidDiamondData("FULLSTOCK", null);

                IntTotalPcs = DTab.Rows.Count;

                DouTotalCarat = Val.Val(DTab.Compute("SUM(Weight)", ""));

                StrFileName = Global.ExportToCSV(DTab, StrFileName);

                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = StrHostName,
                    UserName = StrUserName,
                    Password = StrPassword,
                    PortNumber = IntPort,
                    //SshHostKeyFingerprint = StrCertificate

                };

                using (Session session = new Session())
                {
                    // Connect
                    session.DisableVersionCheck = true;
                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult;
                    transferResult = session.PutFiles(StrFileName, "/", false, transferOptions);
                    transferResult.Check();
                    session.Close();
                }

                // Put API Code Is Here

                StrResult = "Successfully Upload";

                Global.Message(StrResult);
            }
            catch (Exception Ex)
            {
                this.Cursor = Cursors.Default;
                StrResult = "Error : " + Ex.Message;
                Global.Message(StrResult);
            }

            #endregion
        }
        else if (StrAPICode == "GD")
        {
            #region GetDiamond Upload

            try
            {
                StrFileName = Application.StartupPath + "\\APIFile\\GetDiamond.csv";
                DataTable DTab = ObjMast.GetGETDiamondData("FULLSTOCK", null);

                IntTotalPcs = DTab.Rows.Count;
                DouTotalCarat = Val.Val(DTab.Compute("SUM(Weight)", ""));

                StrFileName = Global.ExportToCSV(DTab, StrFileName);

                //SessionOptions.GiveUpSecurityAndAcceptAnySshHostKey = true;
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = StrHostName,
                    UserName = StrUserName,
                    Password = StrPassword,
                    PortNumber = IntPort,
                    //SshHostKeyFingerprint = certificate
                    GiveUpSecurityAndAcceptAnySshHostKey = true

                };

                using (Session session = new Session())
                {
                    // Connect
                    //session.DisableVersionCheck = true;
                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult;
                    transferResult = session.PutFiles(StrFileName, "/home/SHREERAMDOOTGEMSLLP330/", false, transferOptions);
                    transferResult.Check();
                    session.Close();
                }

                // Put API Code Is Here

                StrResult = "Successfully Upload";

                Global.Message(StrResult);
            }
            catch (Exception Ex)
            {
                this.Cursor = Cursors.Default;
                StrResult = "Error : " + Ex.Message;
                Global.Message(StrResult);
            }

            #endregion
        }
        else if (StrAPICode == "IDEX")
        {
            #region IDex Upload

            try
            {
                DataTable dtDetail = ObjMast.GetIDexData("FULLSTOCK", null);

                IntTotalPcs = dtDetail.Rows.Count;
                DouTotalCarat = Val.Val(dtDetail.Compute("SUM(Carat)", ""));

                StrFileName = Application.StartupPath + "\\APIFile\\IDEX.csv";

                using (Session session = new Session())
                {
                    //Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult;
                    #region :: IDEX ::

                    if (dtDetail.Rows.Count > 0)
                    {

                        string IDexFilename = Application.StartupPath + "\\APIFile\\" + "9091e9e9-643b-46d8-aa19-d5889f766350_" + DateTime.Now.ToString("MMMddyy") + "_1.xls";

                        // DataTable DtIdex = IDEXTable(dtDetail).Copy();
                        //if (DtIdex != null && DtIdex.Rows.Count > 0)
                        {
                            ExportToXLS(dtDetail, IDexFilename, "");
                            UploadIDEX_FtpFile(IDexFilename);
                            //SendExcelSheetMail_Idex(dtDetail, IDEXStock, "UPLOAD", "IDEX");

                            if (File.Exists(StrFileName))
                            {
                                File.Delete(StrFileName);
                            }
                            File.Copy(IDexFilename, StrFileName);

                            if (File.Exists(IDexFilename))
                            {
                                File.Delete(IDexFilename);
                            }

                            if (File.Exists(IDexFilename))
                                System.GC.Collect();
                            System.GC.WaitForPendingFinalizers();
                            File.Delete(IDexFilename);
                        }
                    }
                    #endregion
                }

                StrResult = "Successfully Upload";
                Global.Message(StrResult);

            }
            catch (Exception Ex)
            {
                this.Cursor = Cursors.Default;
                StrResult = "Error : " + Ex.Message;
                Global.Message(StrResult);
            }

            #endregion
        }
        else if (StrAPICode == "NIVODA")
        {
            #region Nivoda Upload

            try
            {
                StrFileName = Application.StartupPath + "\\APIFile\\Nivoda.csv";
                DataTable DTab = ObjMast.GetNivodaData("FULLSTOCK", null);

                IntTotalPcs = DTab.Rows.Count;
                DouTotalCarat = Val.Val(DTab.Compute("SUM(Weight)", ""));

                StrFileName = Global.ExportToCSV(DTab, StrFileName);

                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = StrHostName,
                    UserName = StrUserName,
                    Password = StrPassword,
                    PortNumber = IntPort,
                    //SshHostKeyFingerprint = StrCertificate

                };

                using (Session session = new Session())
                {
                    // Connect
                    session.DisableVersionCheck = true;
                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult;
                    transferResult = session.PutFiles(StrFileName, "/", false, transferOptions);
                    transferResult.Check();
                    session.Close();
                }

                // Put API Code Is Here

                StrResult = "Successfully Upload";

                Global.Message(StrResult);
            }
            catch (Exception Ex)
            {
                this.Cursor = Cursors.Default;
                StrResult = "Error : " + Ex.Message;
                Global.Message(StrResult);
            }

            #endregion
        }
        else if (StrAPICode == "NICANOR")
        {
            #region Nicanor Upload

            try
            {
                StrFileName = Application.StartupPath + "\\APIFile\\Nicanor.csv";
                DataTable DTab = ObjMast.GetNicanorData("FULLSTOCK", null);

                IntTotalPcs = DTab.Rows.Count;
                DouTotalCarat = Val.Val(DTab.Compute("SUM(Weight)", ""));

                StrFileName = Global.ExportToCSV(DTab, StrFileName);

                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = StrHostName,
                    UserName = StrUserName,
                    Password = StrPassword,
                    PortNumber = IntPort,
                    //SshHostKeyFingerprint = StrCertificate

                };

                using (Session session = new Session())
                {
                    // Connect
                    session.DisableVersionCheck = true;
                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult;
                    transferResult = session.PutFiles(StrFileName, "/", false, transferOptions);
                    transferResult.Check();
                    session.Close();
                }

                // Put API Code Is Here

                StrResult = "Successfully Upload";

                Global.Message(StrResult);
            }
            catch (Exception Ex)
            {
                this.Cursor = Cursors.Default;
                StrResult = "Error : " + Ex.Message;
                Global.Message(StrResult);
            }

            #endregion
        }
        else if (StrAPICode == "MD")
        {
            #region Marker DiaMond Upload

            try
            {
                StrFileName = Application.StartupPath + "\\APIFile\\MarketDiamonds.csv";
                DataTable DTab = ObjMast.GetMarketDiamondData("FULLSTOCK", null);

                IntTotalPcs = DTab.Rows.Count;
                DouTotalCarat = Val.Val(DTab.Compute("SUM(Weight)", ""));

                StrFileName = Global.ExportToCSV(DTab, StrFileName);

                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Ftp,
                    HostName = StrHostName,
                    UserName = StrUserName,
                    Password = StrPassword,
                    PortNumber = IntPort,
                    //SshHostKeyFingerprint = StrCertificate

                };

                using (Session session = new Session())
                {
                    // Connect
                    session.DisableVersionCheck = true;
                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    TransferOperationResult transferResult;
                    transferResult = session.PutFiles(StrFileName, "/", false, transferOptions);
                    transferResult.Check();
                    session.Close();
                }

                // Put API Code Is Here

                StrResult = "Successfully Upload";

                Global.Message(StrResult);
            }
            catch (Exception Ex)
            {
                this.Cursor = Cursors.Default;
                StrResult = "Error : " + Ex.Message;
                Global.Message(StrResult);
            }

            #endregion
        }
        else if (StrAPICode == "UNIAXON") //#P : 25-12-2020
        {
            #region UNIAxon Stock Upload

            try
            {
                StrFileName = Application.StartupPath + "\\APIFile\\UNIAxon.csv";
                DataTable DTab = ObjMast.GetUNIAxonDiamonddData("FULLSTOCK", null);

                IntTotalPcs = DTab.Rows.Count;
                DouTotalCarat = Val.Val(DTab.Compute("SUM(Carat)", ""));

                StrFileName = Global.ExportToCSV(DTab, StrFileName);

                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = StrHostName,
                    UserName = StrUserName,
                    Password = StrPassword,
                    PortNumber = IntPort,
                    GiveUpSecurityAndAcceptAnySshHostKey = true,

                };

                using (Session session = new Session())
                {
                    // Connect
                    session.DisableVersionCheck = true;
                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions();
                    transferOptions.TransferMode = TransferMode.Binary;

                    transferOptions.PreserveTimestamp = false;

                    TransferOperationResult transferResult;
                    transferResult = session.PutFiles(StrFileName, "/", false, transferOptions);
                    transferResult.Check();

                    session.Close();
                }

                // Put API Code Is Here

                StrResult = "Successfully Upload";

                Global.Message(StrResult);
            }
            catch (Exception Ex)
            {
                this.Cursor = Cursors.Default;
                StrResult = "Error : " + Ex.Message;
                Global.Message(StrResult);
            }

            #endregion
        }

        */
            #endregion

            FileInfo f = new FileInfo(StrFileName);
            ObjMast.UpdateHistory(IntAPIID, StrResult, f.Name, IntTotalPcs, DouTotalCarat);
            this.Cursor = Cursors.Default;

        }

        public void ExportToXLS(DataTable dt, string strFilePath, string fileName)
        {
            try
            {
                if (dt.Rows.Count > 0)
                {
                    Workbook workbookGrid = new Workbook();
                    workbookGrid.DocumentProperties.Author = "DiaSales";
                    workbookGrid.DocumentProperties.Title = "DiaSales Exported Diamonds";

                    workbookGrid.Worksheets.Add("Stock");

                    Spire.Xls.Worksheet worksheet;
                    worksheet = workbookGrid.Worksheets[0];
                    worksheet.Name = "Singles";
                    worksheet[1, 1, dt.Rows.Count, dt.Columns.Count].Style.Font.Size = 10;
                    worksheet[1, 1, dt.Rows.Count, dt.Columns.Count].Style.Font.FontName = "Arial";

                    worksheet.InsertDataTable(dt, true, 1, 1, -1, -1);
                    worksheet[1, 1, 1, dt.Columns.Count].Style.Font.IsBold = true;

                    worksheet.AllocatedRange.AutoFitColumns();
                    worksheet.AllocatedRange.AutoFitRows();

                recall1:

                    if (workbookGrid.Worksheets.Count > 1)
                    {
                        worksheet = workbookGrid.Worksheets[1];
                        worksheet.Activate();

                        if (worksheet.Name != "Stone ")
                        {
                            worksheet.Remove();
                        }
                        goto recall1;
                    }

                    worksheet = workbookGrid.Worksheets[0];

                    workbookGrid.SaveToFile(strFilePath);
                    workbookGrid = null;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        public void UploadIDEX_FtpFile(string fileNm)
        {


            FtpWebRequest request;
            try
            {
                //eventLog1.WriteEntry("IDEX SERVICE UPLOAD START", EventLogEntryType.Information);
                string absoluteFileName = Path.GetFileName(fileNm);

                request = WebRequest.Create(new Uri(string.Format(@"ftp://{0}/{1}/{2}", "ftp1.idexonline.com", "9091e9e9-643b-46d8-aa19-d5889f766350", absoluteFileName))) as FtpWebRequest;
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.UseBinary = true;
                request.UsePassive = true;
                request.KeepAlive = false;
                request.Credentials = new NetworkCredential("IDX_INV_FTP", "inv20U15");
                request.ConnectionGroupName = "MyGroupName";
                using (FileStream fs = File.OpenRead(fileNm))
                {
                    byte[] buffer = new byte[fs.Length];
                    fs.Read(buffer, 0, buffer.Length);
                    fs.Close();
                    Stream requestStream = request.GetRequestStream();
                    requestStream.Write(buffer, 0, buffer.Length);
                    requestStream.Flush();
                    requestStream.Close();
                }
                //eventLog1.WriteEntry("IDEX SERVICE UPLOAD DONE", EventLogEntryType.Information);
            }
            catch (WebException e)
            {
                // eventLog1.WriteEntry("IDEX SERVICE UPLOAD ERROR :" + e.Message, EventLogEntryType.Error);
                Global.Message("IDEX SERVICE UPLOAD ERROR :" + e.Message);
                String status = ((FtpWebResponse)e.Response).StatusDescription;
            }
        }

        private void BtnRefreshHistory_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            DataTable DTab = ObjMast.GetAPIHistory(Val.SqlDate(DTPDate.Value.ToShortDateString()));
            MainGridHistory.DataSource = DTab;
            MainGridHistory.Refresh();
            this.Cursor = Cursors.Default;
        }

    }
}