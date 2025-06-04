using MahantExport;
using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.Data;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using Google.API.Translate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace MahantExport.Stock
{
    public partial class FrmPriceRevisedOld : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        BOTRN_PriceRevised ObjTrn = new BOTRN_PriceRevised();
        BOFormPer ObjPer = new BOFormPer();

        DataTable DTabRapaportData = new DataTable();

        DataTable DTabStoneReviseData = new DataTable();

        double DouCarat = 0;
        double DouNewRapaport = 0;
        double DouNewRapaportAmt = 0;
        double DouNewDisc = 0;
        double DouNewPricePerCarat = 0;
        double DouNewAmount = 0;

        double DouSaleRapaport = 0;
        double DouSaleRapaportAmt = 0;
        double DouSaleDisc = 0;
        double DouSalePricePerCarat = 0;
        double DouSaleAmount = 0;

        double DouDiffRapaport = 0;
        double DouDiffRapaportAmt = 0;
        double DouDiffDisc = 0;
        double DouDiffPricePerCarat = 0;
        double DouDiffAmount = 0;

        #region Property Settings

        public FrmPriceRevisedOld()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));

            BtnDownloadRap.Enabled = ObjPer.ISINSERT;
            BtnRapaportGetData.Enabled = ObjPer.ISVIEW;
            //GrdDet.OptionsBehavior.Editable = ObjPer.ISUPDATE;
            
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();
           
            FillRapaportDate();
            FillCombo();

            txtUsername.Text = new BOTRN_PriceRevised().GetRapnetUserName();
            txtPassword.Text = new BOTRN_PriceRevised().GetRapnetPassword();

            //DTabData.Rows[0]["", DataRowVersion.Original] 
            //DTabData.Rows[0]["", DataRowVersion.Current] 
        }
      
        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjTrn);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
            
        }

        #endregion

        #region Rapaport

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void FillRapaportDate()
        {
            this.Cursor = Cursors.WaitCursor;
            DataTable DTabRapDate = ObjTrn.GetOriginalRapData("RAPDATE", "", "", 0, 0);
            DTabRapDate.DefaultView.Sort = "RAPDATE DESC";
            DTabRapDate = DTabRapDate.DefaultView.ToTable();

            CmbRapaportRapDate.Items.Clear();
            CmbRapaportShape.Items.Clear();
            CmbRapaportSize.Items.Clear();
            cmbRapDate.Items.Clear();
            foreach (DataRow DRow in DTabRapDate.Rows)
            {
                CmbRapaportRapDate.Items.Add(DateTime.Parse(Val.ToString(DRow["RAPDATE"])).ToString("dd/MM/yyyy"));
                cmbRapDate.Items.Add(DateTime.Parse(Val.ToString(DRow["RAPDATE"])).ToString("dd/MM/yyyy"));
            }
            DTabRapDate.Dispose();
            DTabRapDate = null;
            this.Cursor = Cursors.Default;
        }

        private void CmbRapaportRapDate_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (Val.ToString(CmbRapaportRapDate.SelectedItem) == "")
            {
                Global.Message("Please Select Rap Date");
                CmbRapaportRapDate.Focus();
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            DataTable DTabShape = ObjTrn.GetOriginalRapData("SHAPE", Val.ToString(CmbRapaportRapDate.SelectedItem), "", 0, 0);
            CmbRapaportShape.Items.Clear();
            CmbRapaportShape.Items.Add("");
            foreach (DataRow DRow in DTabShape.Rows)
            {
                CmbRapaportShape.Items.Add(Val.ToString(DRow["SHAPE"]));
            }
            DTabShape.Dispose();
            DTabShape = null;

            DataTable DTabSize = ObjTrn.GetOriginalRapData("SIZE", Val.ToString(CmbRapaportRapDate.SelectedItem), Val.ToString(CmbRapaportShape.SelectedItem), 0, 0);
            CmbRapaportSize.Items.Clear();
            CmbRapaportSize.Items.Add("");
            foreach (DataRow DRow in DTabSize.Rows)
            {
                CmbRapaportSize.Items.Add(Val.ToString(DRow["SIZE"]));
            }
            DTabSize.Dispose();
            DTabSize = null;

            this.Cursor = Cursors.Default;
        }

        private void CmbRapaportShape_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (Val.ToString(CmbRapaportRapDate.SelectedItem) == "")
            {
                Global.Message("Please Select Rap Date");
                CmbRapaportRapDate.Focus();
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            DataTable DTabSize = ObjTrn.GetOriginalRapData("SIZE", Val.ToString(CmbRapaportRapDate.SelectedItem), Val.ToString(CmbRapaportShape.SelectedItem), 0, 0);
            CmbRapaportSize.Items.Clear();
            CmbRapaportSize.Items.Add("");
            foreach (DataRow DRow in DTabSize.Rows)
            {
                CmbRapaportSize.Items.Add(Val.ToString(DRow["SIZE"]));
            }
            DTabSize.Dispose();
            DTabSize = null;
            this.Cursor = Cursors.Default;
        }

        private void BtnRapaportGetData_Click(object sender, EventArgs e)
        {
            try
            {
                    if (Val.ToString(CmbRapaportRapDate.SelectedItem) == "")
                    {
                        Global.Message("Please Select Rap Date");
                        CmbRapaportRapDate.Focus();
                        return;
                    }

                    this.Cursor = Cursors.WaitCursor;

                    double DouFromSize = 0;
                    double DouToSize = 0;

                    if (Val.ToString(CmbRapaportSize.SelectedItem) != "")
                    {
                        DouFromSize = Val.Val(Val.ToString(CmbRapaportSize.SelectedItem).Split('-')[0]);
                        DouToSize = Val.Val(Val.ToString(CmbRapaportSize.SelectedItem).Split('-')[1]);
                    }

                    DTabRapaportData = ObjTrn.GetOriginalRapData("RAPVALUE", Val.ToString(CmbRapaportRapDate.SelectedItem), Val.ToString(CmbRapaportShape.SelectedItem), DouFromSize, DouToSize);

                    MainGridRapaport.DataSource = DTabRapaportData;
                    MainGridRapaport.Refresh();

                    // GrdDet.BestFitColumns();
                    GrdDetRapaport.Columns["SHAPE"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;

                    GrdDetRapaport.Columns["SHAPE"].Caption = "Shp";
                    GrdDetRapaport.Columns["FROMCARAT"].Caption = "F Cts";
                    GrdDetRapaport.Columns["TOCARAT"].Caption = "T Cts";
                    GrdDetRapaport.Columns["COLOR"].Caption = "Col";
                    
                    
                
                    this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }

        }

        

        private void BtnDownloadRap_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUsername.Text.Trim().Length == 0)
                {
                    Global.Message("Rapnet Username And Password Is Required");
                    txtUsername.Focus();
                    return;
                }
                if (txtPassword.Text.Trim().Length == 0)
                {
                    Global.Message("Rapnet Username And Password Is Required");
                    txtPassword.Focus();
                    return;
                }
                if (Global.Confirm("Are You Sure To Revised Your Pricing With Latest Rapaport Date ?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                DataTable DTabRound = DownRap("Round");
                DataTable DTabPear = DownRap("Pear");

                if (DTabRound == null || DTabRound.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Round Shape Data Not Found From Rapnet Server\n\nOr May Be Invalid Username and Password");
                    return;
                }
                if (DTabPear == null || DTabPear.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("Pear Shape Data Not Found From RapNet Server\n\nOr May Be Invalid Username and Password");
                    return;
                }

                DTabRound.Columns["F1"].ColumnName = "S_CODE";
                DTabRound.Columns["F2"].ColumnName = "Q_CODE";
                DTabRound.Columns["F3"].ColumnName = "C_CODE";
                DTabRound.Columns["F4"].ColumnName = "F_CARAT";
                DTabRound.Columns["F5"].ColumnName = "T_CARAT";
                DTabRound.Columns["F6"].ColumnName = "RAPVALUE";
                DTabRound.Columns["F7"].ColumnName = "RAPDATE";
                DTabRound.TableName = "TABLE";

                DTabPear.Columns["F1"].ColumnName = "S_CODE";
                DTabPear.Columns["F2"].ColumnName = "Q_CODE";
                DTabPear.Columns["F3"].ColumnName = "C_CODE";
                DTabPear.Columns["F4"].ColumnName = "F_CARAT";
                DTabPear.Columns["F5"].ColumnName = "T_CARAT";
                DTabPear.Columns["F6"].ColumnName = "RAPVALUE";
                DTabPear.Columns["F7"].ColumnName = "RAPDATE";
                DTabPear.TableName = "TABLE";


                string RoundXml = string.Empty;

                string StrRapDate = Val.SqlDate(DTabRound.Rows[0]["RAPDATE"].ToString());

                string StrRapDateFinal = "";

                //Comment : #P : 02-06-2020  : Coz 05/06/20(Foramt:mm/dd/yyyy) vali date ne 06/05/20(Format:dd/mm/yyyy) tarike Store nathi kartu.... Coz sql consider format is 'mm/dd/yyy'
                if (!Val.SqlDate(StrRapDate).Equals(string.Empty))
                    StrRapDateFinal = Val.SqlDate(StrRapDate);
                else
                    StrRapDateFinal = StrRapDate;

                using (StringWriter sw = new StringWriter())
                {
                    DTabRound.WriteXml(sw);
                    RoundXml = sw.ToString();
                }
                string PearXml = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabPear.WriteXml(sw);
                    PearXml = sw.ToString();
                }

                string Str = ObjTrn.UpdateRapnetWithAllDiscount(RoundXml, PearXml, StrRapDateFinal);
                if (Str == "SUCCESS")
                {
                    //string StrDate = DateTime.Parse(Val.ToString(DTabRound.Rows[0]["RapDate"])).ToString("dd/MM/yyyy");
                    Global.Message("SUCCESSFULLY UPLOAD & REVISED DISCOUNT DATA WITH\n\nRAPAPORT : " + StrRapDate + "\n");  //\nSO, KINDLY CHECK IN RAPCALC");
                    DTabRapaportData.Rows.Clear();
                    
                    //FillRapaportDate();
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
            
        }


        private DataTable DownRap(string pStrFileName)
        {
            try
            {
                string URL;
                string URLAuth = "https://technet.rapaport.com/HTTP/Authenticate.aspx";
                WebClient webClient = new WebClient();

                NameValueCollection formData = new NameValueCollection();

                formData["Username"] = txtUsername.Text;
                formData["Password"] = txtPassword.Text;
                byte[] responseBytes = webClient.UploadValues(URLAuth, "POST", formData);
                string ResultAuth = Encoding.UTF8.GetString(responseBytes);

                if (ResultAuth == "")
                {
                    return null;
                }

                // ROUND

                URL = "https://technet.rapaport.com/HTTP/Prices/CSV2_" + pStrFileName + ".aspx";

                WebRequest webRequest = WebRequest.Create(URL);

                webRequest.Method = "POST";
                webRequest.ContentType = "Application/X-Www-Form-Urlencoded";
                webRequest.Headers.Add(System.Net.HttpRequestHeader.AcceptEncoding, "Gzip");

                Stream reqStream = webRequest.GetRequestStream();
                string postData = "ticket=" + ResultAuth;
                byte[] postArray = Encoding.ASCII.GetBytes(postData);
                reqStream.Write(postArray, 0, postArray.Length);
                reqStream.Close();

                WebResponse webResponse = webRequest.GetResponse();

                StreamReader str;

                if (webResponse.Headers.Get("Content-Encoding") != null && webResponse.Headers.Get("Content-Encoding").ToLower() == "gzip")
                {
                    str = new StreamReader(new GZipStream(webResponse.GetResponseStream(), CompressionMode.Decompress));
                }
                else
                {
                    str = new StreamReader(webResponse.GetResponseStream());
                }

                string Result = str.ReadToEnd();

                if (File.Exists(System.Windows.Forms.Application.StartupPath + "\\" + pStrFileName + ".csv"))
                {
                    File.Delete(System.Windows.Forms.Application.StartupPath + "\\" + pStrFileName + ".csv");
                }

                using (TextWriter tw = new StreamWriter(System.Windows.Forms.Application.StartupPath + "\\" + pStrFileName + ".csv", false))
                {
                    tw.Write(Result);
                }

                string StrFilePath = System.Windows.Forms.Application.StartupPath + "\\" + pStrFileName + ".csv";
                DataTable DTabRap = Global.GetDataTableFromCsv(StrFilePath, false);
                return DTabRap;
            }
            catch (Exception ex)
            {
                Global.Message(ex.ToString());
                return null;
            }
           
        }

        #endregion

     

        private void lblRoundDownload_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = "csv";
                svDialog.Title = "csv file";
                svDialog.FileName = "round.csv";
                svDialog.Filter = "csv files(*.csv)|*.csv|All files (*.*)|*.*";
                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    this.Cursor = Cursors.WaitCursor;

                    string Filepath = svDialog.FileName;

                    string URL;
                    string URLAuth = "https://technet.rapaport.com/HTTP/Authenticate.aspx";
                    WebClient webClient = new WebClient();

                    NameValueCollection formData = new NameValueCollection();

                    formData["Username"] = txtUsername.Text;
                    formData["Password"] = txtPassword.Text;
                    byte[] responseBytes = webClient.UploadValues(URLAuth, "POST", formData);
                    string ResultAuth = Encoding.UTF8.GetString(responseBytes);

                    if (ResultAuth == "")
                    {
                        this.Cursor = Cursors.Default;
                        return;
                    }

                    

                    // ROUND

                    URL = "https://technet.rapaport.com/HTTP/Prices/CSV2_round.aspx";

                    WebRequest webRequest = WebRequest.Create(URL);

                    webRequest.Method = "POST";
                    webRequest.ContentType = "Application/X-Www-Form-Urlencoded";
                    webRequest.Headers.Add(System.Net.HttpRequestHeader.AcceptEncoding, "Gzip");

                    Stream reqStream = webRequest.GetRequestStream();
                    string postData = "ticket=" + ResultAuth;
                    byte[] postArray = Encoding.ASCII.GetBytes(postData);
                    reqStream.Write(postArray, 0, postArray.Length);
                    reqStream.Close();

                    WebResponse webResponse = webRequest.GetResponse();

                    StreamReader str;

                    if (webResponse.Headers.Get("Content-Encoding") != null && webResponse.Headers.Get("Content-Encoding").ToLower() == "gzip")
                    {
                        str = new StreamReader(new GZipStream(webResponse.GetResponseStream(), CompressionMode.Decompress));
                    }
                    else
                    {
                        str = new StreamReader(webResponse.GetResponseStream());
                    }

                    string Result = str.ReadToEnd();

                    using (TextWriter tw = new StreamWriter(Filepath, false))
                    {
                        tw.Write(Result);
                    }
                    this.Cursor = Cursors.Default;
                    System.Diagnostics.Process.Start(Filepath, "CMD");                
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.ToString());
                return;
            }
           
        }

        private void lblPearDownload_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = "csv";
                svDialog.Title = "csv file";
                svDialog.FileName = "pear.csv";
                svDialog.Filter = "csv files(*.csv)|*.csv|All files (*.*)|*.*";
                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    this.Cursor = Cursors.WaitCursor;
                    string Filepath = svDialog.FileName;

                    string URL;
                    string URLAuth = "https://technet.rapaport.com/HTTP/Authenticate.aspx";
                    WebClient webClient = new WebClient();

                    NameValueCollection formData = new NameValueCollection();

                    formData["Username"] = txtUsername.Text;
                    formData["Password"] = txtPassword.Text;
                    byte[] responseBytes = webClient.UploadValues(URLAuth, "POST", formData);
                    string ResultAuth = Encoding.UTF8.GetString(responseBytes);

                    if (ResultAuth == "")
                    {
                        this.Cursor = Cursors.Default;
                        return;
                    }

                    // ROUND

                    URL = "https://technet.rapaport.com/HTTP/Prices/CSV2_pear.aspx";

                    WebRequest webRequest = WebRequest.Create(URL);

                    webRequest.Method = "POST";
                    webRequest.ContentType = "Application/X-Www-Form-Urlencoded";
                    webRequest.Headers.Add(System.Net.HttpRequestHeader.AcceptEncoding, "Gzip");

                    Stream reqStream = webRequest.GetRequestStream();
                    string postData = "ticket=" + ResultAuth;
                    byte[] postArray = Encoding.ASCII.GetBytes(postData);
                    reqStream.Write(postArray, 0, postArray.Length);
                    reqStream.Close();

                    WebResponse webResponse = webRequest.GetResponse();

                    StreamReader str;

                    if (webResponse.Headers.Get("Content-Encoding") != null && webResponse.Headers.Get("Content-Encoding").ToLower() == "gzip")
                    {
                        str = new StreamReader(new GZipStream(webResponse.GetResponseStream(), CompressionMode.Decompress));
                    }
                    else
                    {
                        str = new StreamReader(webResponse.GetResponseStream());
                    }

                    string Result = str.ReadToEnd();

                    using (TextWriter tw = new StreamWriter(Filepath, false))
                    {
                        tw.Write(Result);
                    }
                    this.Cursor = Cursors.Default;
                    System.Diagnostics.Process.Start(Filepath, "CMD");
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.ToString());
                return;
            }
           
        }

        private void BtnExportRapaport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("Rapaport", GrdDetRapaport);
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void checkedComboBoxEdit6_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void FillCombo()
        {
            this.Cursor = Cursors.WaitCursor;
            cmbShape.Properties.ValueMember = "SHAPE_ID";
            cmbShape.Properties.DisplayMember = "SHAPENAME";
            cmbShape.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SHAPE);
            cmbColor.Properties.ValueMember = "COLOR_ID";
            cmbColor.Properties.DisplayMember = "COLORNAME";
            cmbColor.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COLOR);
            cmbClarity.Properties.ValueMember = "CLARITY_ID";
            cmbClarity.Properties.DisplayMember = "CLARITYCODE";
            cmbClarity.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CLARITY);
            cmbCut.Properties.ValueMember = "CUT_ID";
            cmbCut.Properties.DisplayMember = "CUTNAME";
            cmbCut.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CUT);
            cmbPol.Properties.ValueMember = "POL_ID";
            cmbPol.Properties.DisplayMember = "POLNAME";
            cmbPol.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_POL);
            cmbSym.Properties.ValueMember = "SYM_ID";
            cmbSym.Properties.DisplayMember = "SYMNAME";
            cmbSym.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SYM);
            cmbFL.Properties.ValueMember = "FL_ID";
            cmbFL.Properties.DisplayMember = "FLNAME";
            cmbFL.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_FL);
            this.Cursor = Cursors.Default;
        }

        private void FillStoneReviseGrid()
        {
            if (DTabStoneReviseData.Rows.Count ==0)
            {
                MainGrdDetail.DataSource = null;
                return;
            }
           
            DataView dv = new DataView(DTabStoneReviseData);
            if (rbtnIdle.Checked)
            {
                dv.RowFilter = "FILTERSTATUS = 0";    
            }
            else if (rbtnUp.Checked)
            {
                dv.RowFilter = "FILTERSTATUS = 1";
            }
            else if (rbtnDown.Checked)
            {
                dv.RowFilter = "FILTERSTATUS = 2";
            }


            MainGrdDetail.DataSource = dv.ToTable();
            MainGrdDetail.Refresh();
            //GrdDetail.BestFitColumns();
            CalculateTotalSummary();
         
        }

        private void CalculateTotalSummary()
        {
            try
            {
                txtTotalPackets.Text = GrdDetail.Columns["PCS"].SummaryItem.SummaryValue.ToString();
                txtSelectedPackets.Text = txtTotalPackets.Text;
                txtDiffPackets.Text = "0";
                txtTotalPcs.Text = GrdDetail.Columns["PCS"].SummaryItem.SummaryValue.ToString();
                txtSelectedPcs.Text = txtTotalPcs.Text;
                txtDiffPcs.Text = "0";
                txtTotalCarat.Text = GrdDetail.Columns["CARAT"].SummaryItem.SummaryValue.ToString();
                txtSelectedCarat.Text = txtTotalCarat.Text;
                txtDiffCarat.Text = "0";

                txtTotalDisc.Text = GrdDetail.Columns["SALEDISCOUNT"].SummaryItem.SummaryValue.ToString();
                txtSelectedDisc.Text = GrdDetail.Columns["NEWDISCOUNT"].SummaryItem.SummaryValue.ToString();
                txtDiffDisc.Text = Val.Format( Val.ToDouble(txtSelectedDisc.Text) - Val.ToDouble(txtTotalDisc.Text),"####0.00");
                txtTotalPricePerCarat.Text = GrdDetail.Columns["SALEPRICEPERCARAT"].SummaryItem.SummaryValue.ToString();
                txtSelectedPricePerCarat.Text = GrdDetail.Columns["NEWPRICEPERCARAT"].SummaryItem.SummaryValue.ToString();
                txtDiffPricePerCarat.Text = Val.Format(Val.ToDouble(txtSelectedPricePerCarat.Text) - Val.ToDouble(txtTotalPricePerCarat.Text), "####0.00");
                txtTotalAmount.Text = GrdDetail.Columns["SALEAMOUNT"].SummaryItem.SummaryValue.ToString();
                txtSelectedAmount.Text = GrdDetail.Columns["NEWAMOUNT"].SummaryItem.SummaryValue.ToString();
                txtDiffAmount.Text = Val.Format(Val.ToDouble(txtSelectedAmount.Text) - Val.ToDouble(txtTotalAmount.Text), "####0.00");
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void Clear()
        {
            cmbShape.SetEditValue("");
            cmbColor.SetEditValue("");
            cmbClarity.SetEditValue("");
            cmbPol.SetEditValue("");
            cmbSym.SetEditValue("");
            cmbCut.SetEditValue("");
            cmbFL.SetEditValue("");
            txtFromCarat.Text = "";
            txtToCarat.Text = "";
            cmbRapDate.Text = "";
            txtTotalPackets.Text = "";
            txtSelectedPackets.Text = "";
            txtDiffPackets.Text = "";
            txtTotalPcs.Text = "";
            txtSelectedPcs.Text = "";
            txtDiffPcs.Text = "";
            txtTotalCarat.Text = "";
            txtSelectedCarat.Text = "";
            txtDiffCarat.Text = "";

            txtTotalDisc.Text = "";
            txtSelectedDisc.Text = "";
            txtDiffDisc.Text = "";
            txtTotalPricePerCarat.Text = "";
            txtSelectedPricePerCarat.Text = "";
            txtDiffPricePerCarat.Text = "";
            txtTotalAmount.Text = "";
            txtSelectedAmount.Text = "";
            txtDiffAmount.Text = "";
            MainGrdDetail.DataSource = null;
        }

        private void FrmPriceRevised_Load(object sender, EventArgs e)
        {
        }

        private void btnStoneRevShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbRapDate.Text.Trim() =="")
                {
                     Global.Message("Please Select Rap Date..");
                     return;
                }

                this.Cursor = Cursors.WaitCursor;
               

                LiveStockProperty LStockProperty = new LiveStockProperty();

                LStockProperty.MULTYSHAPE_ID = cmbShape.EditValue.ToString().Replace(" ","");
                LStockProperty.MULTYCOLOR_ID = cmbColor.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYCLARITY_ID = cmbClarity.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYCUT_ID = cmbCut.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYPOL_ID = cmbPol.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYSYM_ID = cmbSym.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYFL_ID = cmbFL.EditValue.ToString().Replace(" ", "");

                LStockProperty.FROMCARAT = Val.Val(txtFromCarat.Text);
                LStockProperty.TOCARAT = Val.Val(txtToCarat.Text);
                LStockProperty.STOCKNO = Val.ToString(txtStoneNo.Text);

                DTabStoneReviseData.TableName = "Detail";
                DTabStoneReviseData = ObjTrn.GetStoneRevisionData(LStockProperty, Val.ToString(cmbRapDate.SelectedItem), rbtnDis.Checked);



                int IntAll = 0;
                int IntIdle = 0;
                int IntUP = 0;
                int IntDown = 0;

                foreach (DataRow DRow in DTabStoneReviseData.Rows)
                {
                    IntAll++;
                    if (Val.Val(DRow["SALERAPAPORT"]) == Val.Val(DRow["NEWRAPAPORT"]))
                    {
                        IntIdle++;
                    }
                    else if (Val.Val(DRow["NEWRAPAPORT"]) > Val.Val(DRow["SALERAPAPORT"]))
                    {
                        IntUP++;
                    }
                    else if (Val.Val(DRow["NEWRAPAPORT"]) < Val.Val(DRow["SALERAPAPORT"]))
                    {
                        IntDown++;
                    }
                }

                rbtnAll.Text = "ALL(" + IntAll.ToString() + ")";
                rbtnIdle.Text = "IDLE(" + IntIdle.ToString() + ")";
                rbtnUp.Text = "UP(" + IntUP.ToString() + ")";
                rbtnDown.Text = "DOWN(" + IntDown.ToString() + ")";
                
                FillStoneReviseGrid();

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void rbtnAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                FillStoneReviseGrid();
            }
            catch (Exception ex)
            {
               Global.Message(ex.Message.ToString());
            }
        }

        private void rbtnPPC_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (rbtnPPC.Checked || rbtnDis.Checked)
                {
                    btnStoneRevShow.PerformClick();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDetail_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            try
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    DouCarat = 0;
                    DouNewRapaport = 0;
                    DouNewRapaportAmt = 0;
                    DouNewDisc = 0;
                    DouNewPricePerCarat = 0;
                    DouNewAmount = 0;

                    DouSaleRapaport = 0;
                    DouSaleRapaportAmt = 0;
                    DouSaleDisc = 0;
                    DouSalePricePerCarat = 0;
                    DouSaleAmount = 0;
                }
                else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DouCarat = DouCarat + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT"));
                    DouNewAmount = DouNewAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "NEWAMOUNT"));
                    DouNewRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "NEWRAPAPORT"));
                    DouNewPricePerCarat = DouNewAmount / DouCarat;
                    DouNewRapaportAmt = DouNewRapaportAmt + (DouNewRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));


                    DouSaleAmount = DouSaleAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "SALEAMOUNT"));
                    DouSaleRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "SALERAPAPORT"));
                    DouSalePricePerCarat = DouSaleAmount / DouCarat;
                    DouSaleRapaportAmt = DouSaleRapaportAmt + (DouSaleRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

                    DouDiffAmount = DouDiffAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "DIFFAMOUNT"));
                    DouDiffRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "DIFFRAPAPORT"));
                    DouDiffPricePerCarat = DouDiffAmount / DouCarat;
                    DouDiffRapaportAmt = DouDiffRapaportAmt + (DouDiffRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));
                }
                else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("NEWPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouNewAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("NEWRAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouNewRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("NEWDISCOUNT") == 0)
                    {
                        DouNewRapaport = Math.Round((DouNewRapaportAmt / DouCarat), 2);
                        //DouNewDisc = Math.Round(((DouNewPricePerCarat - DouNewRapaport) / DouNewRapaport * 100), 2);
                        DouNewDisc = Math.Round(((DouNewRapaport - DouNewPricePerCarat) / DouNewRapaport * 100), 2);
                        e.TotalValue = DouNewDisc;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("SALEPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouSaleAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("SALERAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouSaleRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("SALEDISCOUNT") == 0)
                    {
                        DouSaleRapaport = Math.Round(DouSaleRapaportAmt / DouCarat);
                        DouSaleDisc = Math.Round(((DouSalePricePerCarat - DouSaleRapaport) / DouSaleRapaport * 100), 2);
                       // DouSaleDisc = Math.Round(((DouSaleRapaport - DouSalePricePerCarat) / DouSaleRapaport * 100), 2);
                        e.TotalValue = DouSaleDisc;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("DIFFPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouDiffAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("DIFFRAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouDiffRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("DIFFDISCOUNT") == 0)
                    {
                        DouDiffRapaport = Math.Round(DouDiffRapaportAmt / DouCarat);
                        DouDiffDisc = Math.Round(((DouDiffPricePerCarat - DouDiffRapaport) / DouDiffRapaport * 100), 2);
                        //DouDiffDisc = Math.Round(((DouDiffRapaport - DouDiffPricePerCarat) / DouDiffRapaport * 100), 2);
                        e.TotalValue = DouDiffDisc;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void btnStoneRevUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                BOTRN_PriceRevised ObjMemo = new BOTRN_PriceRevised();
                MemoEntryProperty ObjMstProper = new MemoEntryProperty();
                ObjMstProper.MEMO_ID = null;
                ObjMstProper.MEMONO = 0;
                ObjMstProper.MEMOTYPE = "PRICING";
                ObjMstProper.MEMODATE = Val.SqlDate(System.DateTime.Now.ToShortDateString());

                ObjMstProper.BILLINGPARTY_ID = null;
                ObjMstProper.SHIPPINGPARTY_ID = null;

                ObjMstProper.BROKER_ID = null;
                ObjMstProper.BROKERBASEPER = 0;
                ObjMstProper.BROKERPROFITPER = 0;
                ObjMstProper.ADAT_ID = null;
                ObjMstProper.ADATPER = 0;
                ObjMstProper.SOLDPARTY_ID = null;

                ObjMstProper.SELLER_ID = null;
                ObjMstProper.TERMS_ID =0;
                ObjMstProper.TERMSDAYS = 0;
                ObjMstProper.TERMSPER =0;
                ObjMstProper.TERMSDATE = null;

                ObjMstProper.CURRENCY_ID = 0;
                ObjMstProper.EXCRATE = 0;

                ObjMstProper.MEMODISCOUNT = 0;

                ObjMstProper.BILLINGADDRESS1 = "";
                ObjMstProper.BILLINGADDRESS2 = "";
                ObjMstProper.BILLINGADDRESS3 = "";
                ObjMstProper.BILLINGCOUNTRY_ID = 0;
                ObjMstProper.BILLINGSTATE ="";
                ObjMstProper.BILLINGCITY = "";
                ObjMstProper.BILLINGZIPCODE ="";

                ObjMstProper.SHIPPINGADDRESS1 = "";
                ObjMstProper.SHIPPINGADDRESS2 = "";
                ObjMstProper.SHIPPINGADDRESS3 = "";
                ObjMstProper.SHIPPINGCOUNTRY_ID =0;
                ObjMstProper.SHIPPINGSTATE ="";
                ObjMstProper.SHIPPINGCITY ="";
                ObjMstProper.SHIPPINGZIPCODE = "";

                ObjMstProper.TOTALPCS = 0;
                ObjMstProper.TOTALCARAT = 0;

                ObjMstProper.GROSSAMOUNT = Val.Val(txtTotalAmount.Text);
                ObjMstProper.DISCOUNTPER = 0;
                ObjMstProper.DISCOUNTAMOUNT = 0;
                ObjMstProper.INSURANCEPER = 0;
                ObjMstProper.INSURANCEAMOUNT =0;
                ObjMstProper.SHIPPINGPER =0;
                ObjMstProper.SHIPPINGAMOUNT = 0;
                ObjMstProper.GSTPER = 0;
                ObjMstProper.GSTAMOUNT = 0;
                ObjMstProper.NETAMOUNT = Val.Val(txtTotalAmount.Text);

                ObjMstProper.REMARK = "STONE REVISION PRICING";
                ObjMstProper.SOURCE = "SOFTWARE";
                ObjMstProper.PROCESS_ID = 13;
                ObjMstProper.PROCESSNAME = "PRICING";

                
                XmlDocument xmlSearchStone = Global.ConvertToXml(ObjMstProper);
                string MemoEntryMasterRecordsForXML = "<DocumentElement><ParamList>" + xmlSearchStone.DocumentElement.InnerXml + "</ParamList></DocumentElement>";
                string MemoEntryDetailForXML;
                using (StringWriter sw = new StringWriter())
                {
                    DTabStoneReviseData.TableName ="Detail";
                    DTabStoneReviseData.WriteXml(sw);
                    MemoEntryDetailForXML = sw.ToString();
                }

                string ReturnMessageDesc = "";
                string ReturnMessageType = "";
                ObjMstProper = ObjMemo.SavePricingMemoEntry(MemoEntryMasterRecordsForXML, MemoEntryDetailForXML,ChkUpdateExpPrice.Checked,Val.ToString(cmbRapDate.SelectedItem));

                ReturnMessageDesc = ObjMstProper.ReturnMessageDesc;
                ReturnMessageType = ObjMstProper.ReturnMessageType;

                ObjMstProper = null;
                Global.Message(ReturnMessageDesc);
                if (ReturnMessageType == "SUCCESS")
                {
                    this.Clear();
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void btnStoneRevExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("Stone Revise List", GrdDetail);
        }

        private void btnStoneRevExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtStoneNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String str1 = txtStoneNo.Text.Trim().Replace("\r\n", ",");
                txtStoneNo.Text = str1;
                txtStoneNo.Select(txtStoneNo.Text.Length, 0);

                string[] Str = str1.Split(',');
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

    }
}
