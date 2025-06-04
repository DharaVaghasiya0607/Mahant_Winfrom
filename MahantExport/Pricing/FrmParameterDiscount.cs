
using MahantExport.Utility;
using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using Google.API.Translate;
using OfficeOpenXml;
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

namespace MahantExport.Pricing
{
    public partial class FrmParameterDiscount : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_ParameterDiscount ObjTrn = new BOTRN_ParameterDiscount();
        BOFormPer ObjPer = new BOFormPer();
        BODevGridSelection ObjGridSelection;

        DataTable DTabRound = new DataTable();
        DataTable DTabPear = new DataTable();


        DataTable DTabDiscountData = new DataTable();
        DataTable DTabRapaportData = new DataTable();
        DataTable DTabRangeData = new DataTable();

        #region Property Settings

        public FrmParameterDiscount()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));

            BtnAddNewRow.Enabled = ObjPer.ISINSERT;
            GrdDet.OptionsBehavior.Editable = ObjPer.ISUPDATE;

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            if (ObjPer.ISVIEW == false)
            {
                Global.MessageError(BusLib.TPV.BOMessage.ViewDeniedMsg);
                return;
            }

            this.Show();

            CmbPricingType.SelectedIndex = 0;

            txtUsername.Text = new BOTRN_PriceRevised().GetRapnetUserName();
            txtPassword.Text = new BOTRN_PriceRevised().GetRapnetPassword();
            txtParameter.Focus();
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

        #region Parameter Discount Method


        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = ".xlsx";
                svDialog.Title = "Export to Excel";
                svDialog.FileName = lblTitle.Text + "_" + Val.ToString(txtRapDate.Text);
                svDialog.Filter = "Excel files 97-2003 (*.xls)|*.xls|Excel files 2007(*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    PrintableComponentLinkBase link = new PrintableComponentLinkBase()
                    {
                        PrintingSystemBase = new PrintingSystemBase(),
                        Component = MainGrid,
                        Landscape = true,
                        PaperKind = PaperKind.A4,
                        Margins = new System.Drawing.Printing.Margins(20, 20, 20, 20)
                    };

                    link.CreateReportHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);

                    link.ExportToXls(svDialog.FileName);

                    if (Global.Confirm("Do You Want To Open [" + svDialog.FileName + ".xlsx] ?") == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(svDialog.FileName, "CMD");
                    }
                }
                svDialog.Dispose();
                svDialog = null;

            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
            }
        }

        public void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            // ' For Report Title

            TextBrick BrickTitle = e.Graph.DrawString(BusLib.Configuration.BOConfiguration.gEmployeeProperty.COMPANYNAME, System.Drawing.Color.Navy, new RectangleF(0, 0, e.Graph.ClientPageSize.Width, 35), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitle.Font = new Font("verdana", 12, FontStyle.Bold);
            BrickTitle.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            BrickTitle.VertAlignment = DevExpress.Utils.VertAlignment.Center;

            // ' For Group 
            TextBrick BrickTitleseller = e.Graph.DrawString("PARAMETER DISCOUNT DATA", System.Drawing.Color.Navy, new RectangleF(0, 35, e.Graph.ClientPageSize.Width, 35), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitleseller.Font = new Font("verdana", 10, FontStyle.Bold);
            BrickTitleseller.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            BrickTitleseller.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            BrickTitleseller.ForeColor = Color.Black;

            // ' For Filter 
            TextBrick BrickTitlesParam = e.Graph.DrawString(lblTitle.Text + " And " + Val.ToString(txtRapDate.Text), System.Drawing.Color.Navy, new RectangleF(0, 70, e.Graph.ClientPageSize.Width, 30), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitlesParam.Font = new Font("verdana", 8, FontStyle.Bold);
            BrickTitlesParam.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
            BrickTitlesParam.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            BrickTitlesParam.ForeColor = Color.Black;


            int IntX = Convert.ToInt32(Math.Round(e.Graph.ClientPageSize.Width - 400, 0));
            TextBrick BrickTitledate = e.Graph.DrawString("Print Date :- " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), System.Drawing.Color.Navy, new RectangleF(IntX, 70, 400, 30), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitledate.Font = new Font("verdana", 8, FontStyle.Bold);
            BrickTitledate.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            BrickTitledate.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            BrickTitledate.ForeColor = Color.Black;

        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            if (Val.ToString(txtParameter.Text) == "")
            {
                Global.Message("Please Select Parameter ID First");
                txtParameter.Focus();
                return;
            }
            if (Val.ToString(txtRapDate.Text) == "")
            {
                Global.Message("Please Select Rap Date");
                txtRapDate.Focus();
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            double DouFromSize = 0;
            double DouToSize = 0;
            string StrPricingType;

            if (Val.ToString(txtSize.Text) != "")
            {
                DouFromSize = Val.Val(Val.ToString(txtSize.Text).Split('-')[0]);
                DouToSize = Val.Val(Val.ToString(txtSize.Text).Split('-')[1]);
            }

            DTabDiscountData = ObjTrn.GetParameterDiscountData("DISCOUNT", Val.ToString(txtParameter.Tag), Val.SqlDate(txtRapDate.Text), Val.ToString(txtShape.Text), DouFromSize, DouToSize, Val.ToString(CmbPricingType.Text));

            MainGrid.DataSource = DTabDiscountData;
            MainGrid.Refresh();

            // GrdDet.BestFitColumns();

            this.Cursor = Cursors.Default;
        }

        private void GrdDet_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000")
            {
                e.DisplayText = String.Empty;
            }
        }

        private void GrdDet_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GrdDet.PostEditor();
            if (e.RowHandle < 0)
            {
                return;
            }

            if (e.Column.FieldName.Contains("Q")
                )
            {

                DataRow DRow = GrdDet.GetDataRow(e.RowHandle);

                if (Val.ToString(DRow["S_CODE"]).Trim().Equals(string.Empty) || Val.ToString(DRow["C_NAME"]).Trim().Equals(string.Empty) || Val.Val(DRow["F_CARAT"]) <= 0 || Val.Val(DRow["T_CARAT"]) <= 0)
                    return;

                ParameterDiscountProperty Property = new ParameterDiscountProperty();

                Property.F_CARAT = Val.Val(DRow["F_CARAT"]);
                Property.T_CARAT = Val.Val(DRow["T_CARAT"]);

                Property.SHAPE_ID = Val.ToInt(DRow["SHAPE_ID"]);
                Property.COLOR_ID = Val.ToInt(DRow["COLOR_ID"]);

                Property.Q_CODE = e.Column.FieldName;
                Property.Q_NAME = e.Column.Caption;
                Property.RAPDATE = Val.SqlDate(Val.ToString(DRow["RAPDATE"]));
                Property.PARAMETER_ID = Val.ToString(DRow["PARAMETER_ID"]);
                Property.PARAMETER_VALUE = Val.ToString(DRow["PARAMETER_VALUE"]);
                Property.PRICINGTYPE = Val.ToString(DRow["PRICINGTYPE"]);

                if (Property.F_CARAT == 0 || Property.T_CARAT == 0 || Property.SHAPE_ID == 0 || Property.COLOR_ID == 0 || Property.Q_CODE == "" || Property.PARAMETER_ID == "")
                {
                    Global.Message("Some Data Has Been Missing");
                    return;
                }

                double OldValue = Val.Val(GrdDet.ActiveEditor.OldEditValue);

                //Property.OLDVALUE = Val.Val(DRow[e.Column.FieldName, DataRowVersion.Original]);

                Property.OLDVALUE = Val.Val(GrdDet.ActiveEditor.OldEditValue);
                Property.NEWVALUE = Val.Val(DRow[e.Column.FieldName, DataRowVersion.Default]);
                Property = ObjTrn.SaveParameterDiscount(Property);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    lblMessage.Text = e.Column.Caption + " Value Updateed From [ " + Property.OLDVALUE.ToString() + " ]  To [ " + Property.NEWVALUE.ToString() + " ] ";
                }
                else
                {
                    lblMessage.Text = "Error......";
                }

                GrdDet.RefreshData();
                DTabDiscountData.AcceptChanges();

                Property = null;
            }


        }


        #endregion


        #region Rapaport

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
                DTabRound = DownRap("Round");
                DTabPear = DownRap("Pear");

                Global.Message("Both Files Are Download Successfully ");
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

        private void txtParameterDiscountShape_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "SHAPECODE,SHAPENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SHAPE);
                    FrmSearch.mStrColumnsToHide = "SHAPE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDet.SetFocusedRowCellValue("S_CODE", Val.ToString(FrmSearch.DRow["SHAPECODE"]));
                        GrdDet.SetFocusedRowCellValue("SHAPE_ID", Val.ToString(FrmSearch.DRow["SHAPE_ID"]));
                    }

                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtParameterDiscountColor_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "COLORCODE,COLORNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COLOR);
                    FrmSearch.mStrColumnsToHide = "COLOR_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDet.SetFocusedRowCellValue("COLOR_ID", Val.ToString(FrmSearch.DRow["COLOR_ID"]));
                        GrdDet.SetFocusedRowCellValue("C_NAME", Val.ToString(FrmSearch.DRow["COLORNAME"]));
                    }

                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void BtnAddNewRow_Click(object sender, EventArgs e)
        {
            try
            {
                if (Val.ToString(txtParameter.Text) == "")
                {
                    Global.Message("Please Select Parameter ID First");
                    txtParameter.Focus();
                    return;
                }
                if (Val.ToString(txtRapDate.Text) == "")
                {
                    Global.Message("Please Select Rap Date");
                    txtRapDate.Focus();
                    return;
                }

                if (DTabDiscountData.Columns.Count > 0)
                {

                    string StrSize = txtSize.Text;
                    string StrFromSize = "";
                    string StrToSize = "";
                    if (!StrSize.Trim().Equals(string.Empty))
                    {
                        StrFromSize = StrSize.ToString().Substring(0, Val.ToString(StrSize).LastIndexOf("-"));
                        StrToSize = StrSize.ToString().Substring(Val.ToString(StrSize).LastIndexOf("-") + 1);
                    }

                    DataRow Dr = DTabDiscountData.NewRow();
                    Dr["RAPDATE"] = txtRapDate.Text;
                    Dr["PARAMETER_ID"] = Val.ToString(txtParameter.Tag);
                    Dr["F_CARAT"] = Val.Val(StrFromSize);
                    Dr["T_CARAT"] = Val.Val(StrToSize);
                    Dr["S_CODE"] = txtShape.Text;
                    Dr["SHAPE_ID"] = Val.ToInt(txtShape.Tag);
                    Dr["PRICINGTYPE"] = Val.ToString(CmbPricingType.Text);

                    DTabDiscountData.Rows.Add(Dr);

                    GrdDet.FocusedRowHandle = Dr.Table.Rows.IndexOf(Dr);
                    GrdDet.FocusedColumn = !Val.ToString(txtSize.Text).Equals(string.Empty) && !Val.ToString(txtSize.Text).Equals(string.Empty) ? GrdDet.VisibleColumns[3] : GrdDet.VisibleColumns[0];
                    GrdDet.Focus();
                    GrdDet.ShowEditor();
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }


        private void BtnUpdateDiscFileWise_Click(object sender, EventArgs e)
        {
            try
            {
                if (Val.ToString(txtParameter.Text) == "")
                {
                    Global.Message("Please Select Parameter ID First");
                    txtParameter.Focus();
                    return;
                }
                if (Val.ToString(txtRapDate.Text) == "")
                {
                    Global.Message("Please Select Rap Date");
                    txtRapDate.Focus();
                    return;
                }

                if (txtFileName.Text.Trim().Length == 0)
                {
                    Global.Message("Please Select File First");
                    txtFileName.Focus();
                    return;
                }

                if (Global.Confirm("Are Your Sure You Want To Save Excel File Records..?") == System.Windows.Forms.DialogResult.No)
                    return;

                this.Cursor = Cursors.WaitCursor;

                DataTable DTabFile = Global.SprireGetDataTableFromExcel(txtFileName.Text, Val.ToString(CmbParaDiscountSheetName.SelectedItem), false);

                // DataTable DTab = Global.ImportExcelXLSWithSheetName(txtFileName.Text, false, Val.ToString(CmbParaDiscountSheetName.SelectedItem));


                if (DTabFile == null || DTabFile.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.MessageError("Data Not Found");
                    return;
                }


                DataTable DTab = new DataTable("TABLE");
                DTab.Columns.Add(new DataColumn("SHAPE", typeof(string)));
                DTab.Columns.Add(new DataColumn("COLOR", typeof(string)));
                DTab.Columns.Add(new DataColumn("FCARAT", typeof(double)));
                DTab.Columns.Add(new DataColumn("TCARAT", typeof(double)));
                DTab.Columns.Add(new DataColumn("PARAMETERVALUE", typeof(string)));
                DTab.Columns.Add(new DataColumn("FL", typeof(double)));
                DTab.Columns.Add(new DataColumn("IF", typeof(double)));
                DTab.Columns.Add(new DataColumn("VVS1", typeof(double)));
                DTab.Columns.Add(new DataColumn("VVS2", typeof(double)));
                DTab.Columns.Add(new DataColumn("VS1", typeof(double)));
                DTab.Columns.Add(new DataColumn("VS2", typeof(double)));
                DTab.Columns.Add(new DataColumn("SI1", typeof(double)));
                DTab.Columns.Add(new DataColumn("SI2", typeof(double)));
                DTab.Columns.Add(new DataColumn("SI3", typeof(double)));
                DTab.Columns.Add(new DataColumn("I1", typeof(double)));
                DTab.Columns.Add(new DataColumn("I2", typeof(double)));
                DTab.Columns.Add(new DataColumn("I3", typeof(double)));

                DataRow DRNew = null;

                string StrParameterValue = string.Empty;
                string StrShape = string.Empty;
                string StrSize = string.Empty;
                string StrFromCarat = string.Empty;
                string StrToCarat = string.Empty;

                int IntShapeRowIndex = 0;
                int IntShapeColIndex = 0;

                int IntColorColIndex = 0;

                int IntSizeRowIndex = 0;
                int IntSizeColIndex = 0;
                int IntParaMeterRowIndex = 0;
                int IntParaMeterColIndex = 0;
                int IntClarityRowIndex = 0;

                for (int IntRow = 0; IntRow < DTabFile.Rows.Count; IntRow++)
                {

                    if (IntRow % 13 == 0)
                    {
                        IntShapeRowIndex = IntRow;
                        IntParaMeterRowIndex = IntRow;

                        continue;
                    }

                    if ((IntRow % 13) == 1)
                    {
                        IntSizeRowIndex = IntRow;
                        IntClarityRowIndex = IntRow;
                        continue;
                    }

                    for (int IntCol = 0; IntCol < DTabFile.Columns.Count; )
                    {
                        if (IntCol % 13 == 0)
                        {
                            IntParaMeterColIndex = IntCol;
                            IntShapeColIndex = IntCol + 1;
                            IntColorColIndex = IntCol;
                            IntSizeColIndex = IntCol;
                            IntCol++;
                            continue;
                        }
                        DRNew = DTab.NewRow();

                        DRNew["SHAPE"] = Val.ToString(DTabFile.Rows[IntShapeRowIndex][IntShapeColIndex]);
                        DRNew["COLOR"] = Val.ToString(DTabFile.Rows[IntRow][IntColorColIndex]);
                        DRNew["FCARAT"] = Val.ToString(DTabFile.Rows[IntSizeRowIndex][IntSizeColIndex]).Split('-')[0];
                        DRNew["TCARAT"] = Val.ToString(DTabFile.Rows[IntSizeRowIndex][IntSizeColIndex]).Split('-')[1];
                        DRNew["PARAMETERVALUE"] = Val.ToString(DTabFile.Rows[IntParaMeterRowIndex][IntParaMeterColIndex]);

                        DRNew["FL"] = Val.Val(DTabFile.Rows[IntRow][IntCol + 0]);
                        DRNew["IF"] = Val.Val(DTabFile.Rows[IntRow][IntCol + 1]);
                        DRNew["VVS1"] = Val.Val(DTabFile.Rows[IntRow][IntCol + 2]);
                        DRNew["VVS2"] = Val.Val(DTabFile.Rows[IntRow][IntCol + 3]);
                        DRNew["VS1"] = Val.Val(DTabFile.Rows[IntRow][IntCol + 4]);
                        DRNew["VS2"] = Val.Val(DTabFile.Rows[IntRow][IntCol + 5]);
                        DRNew["SI1"] = Val.Val(DTabFile.Rows[IntRow][IntCol + 6]);
                        DRNew["SI2"] = Val.Val(DTabFile.Rows[IntRow][IntCol + 7]);
                        DRNew["SI3"] = Val.Val(DTabFile.Rows[IntRow][IntCol + 8]);
                        DRNew["I1"] = Val.Val(DTabFile.Rows[IntRow][IntCol + 9]);
                        DRNew["I2"] = Val.Val(DTabFile.Rows[IntRow][IntCol + 10]);
                        DRNew["I3"] = Val.Val(DTabFile.Rows[IntRow][IntCol + 11]);

                        DTab.Rows.Add(DRNew);
                        IntCol = IntCol + 12;

                    }

                }
                DTab.AcceptChanges();

                DTab.DefaultView.Sort = "SHAPE,FCARAT,TCARAT,COLOR";
                DTab = DTab.DefaultView.ToTable();

                string StrXml = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTab.WriteXml(sw);
                    StrXml = sw.ToString();
                }

                ParameterDiscountProperty Property = new ParameterDiscountProperty();

                Property.RAPDATE = Val.SqlDate(txtRapDate.Text);
                Property.PARAMETER_ID = Val.ToString(txtParameter.Tag);
                Property.PARAMETER_VALUE = "";
                Property.PRICINGTYPE = Val.ToString(CmbPricingType.Text);
                Property.XML = StrXml;
                Property = ObjTrn.SaveParameterDiscountUsingXml(Property);

                this.Cursor = Cursors.Default;
                Global.Message(Property.ReturnMessageDesc);

                Property = null;
                DTab.Dispose();
                DTab = null;

                return;

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private DataTable GetTableOfSelectedRows(GridView view, Boolean IsSelect) //Add : Pinali : 02-08-2019
        {
            if (view.RowCount <= 0)
            {
                return null;
            }
            ArrayList aryLst = new ArrayList();


            DataTable resultTable = new DataTable();
            DataTable sourceTable = null;
            sourceTable = ((DataView)view.DataSource).Table;

            if (IsSelect)
            {
                aryLst = ObjGridSelection.GetSelectedArrayList();
                resultTable = sourceTable.Clone();
                for (int i = 0; i < aryLst.Count; i++)
                {
                    DataRowView oDataRowView = aryLst[i] as DataRowView;
                    resultTable.Rows.Add(oDataRowView.Row.ItemArray);
                }
            }

            return resultTable;
        }


        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            CmbParaDiscountSheetName.Items.Clear();
            OpenFileDialog Open = new OpenFileDialog();
            Open.Filter = "Excel Files|*.xls;*.xlsx";
            if (Open.ShowDialog() == DialogResult.OK)
            {
                txtFileName.Text = Open.FileName;
                Global.SprirelGetSheetNameFromExcel(CmbParaDiscountSheetName, txtFileName.Text);
            }
        }

        private void lblSampleFileDownload_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start(Application.StartupPath+"\\Format\\ParaDiscountUploadFormat.xlsx","CMD");
            System.Diagnostics.Process.Start(Application.StartupPath + "\\Format\\DiscountFormat.xlsx", "CMD");
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                //DataTable DTabRound = DownRap("Round");
                //DataTable DTabPear = DownRap("Pear");

                string StrFilePath = System.Windows.Forms.Application.StartupPath + "\\Round.csv";
                DataTable DTabRound = Global.GetDataTableFromCsv(StrFilePath, false);

                StrFilePath = System.Windows.Forms.Application.StartupPath + "\\Pear.csv";
                DataTable DTabPear = Global.GetDataTableFromCsv(StrFilePath, false);


                if (DTabRound == null || DTabRound.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }
                if (DTabPear == null || DTabPear.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
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

                //string StrRapDate = Val.SqlDate(DTabRound.Rows[0]["RAPDATE"].ToString());
                //string StrRapDate = Val.SqlDate(Val.ToString(DateTime.Now));   ///Changed : 07-08-2020 : coz CurrentDate Store karva mate SqlDate conversion kadhi nakhyu 6e..
                string StrRapDate = Val.ToString(DateTime.Now);
                string StrRapDateFinal = "";

                //Commented : 07-08-2020
                //if (!Val.SqlDate(StrRapDate).Equals(string.Empty))
                //    StrRapDateFinal = Val.SqlDate(StrRapDate);
                //else
                StrRapDateFinal = StrRapDate;

                string Str = ObjTrn.UpdateRapnetWithAllDiscount(RoundXml, PearXml, Val.SqlDate(StrRapDateFinal));
                if (Str == "SUCCESS")
                {
                    string StrDate = "";
                    if (!Val.SqlDate(StrRapDate).Equals(string.Empty))
                    {
                        StrDate = StrRapDate;
                    }
                    else
                    {
                        //StrDate = DateTime.Parse(Val.ToString(DTabRound.Rows[0]["RapDate"])).ToString("dd/MM/yyyy");
                        StrDate = DateTime.Parse(Val.ToString(DateTime.Now)).ToString("dd/MM/yyyy");
                    }

                    Global.Message("SUCCESSFULLY UPLOAD & REVISED DISCOUNT DATA WITH\n\nRAPAPORT : " + StrDate + "\n\nPARAMETER DISCOUNT : " + StrDate + "\n\nSO, KINDLY CHECK IN RAPCALC");

                    DTabDiscountData.Rows.Clear();
                    DTabRapaportData.Rows.Clear();
                    DTabRangeData.Rows.Clear();

                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }


        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressPanel1.Visible = false;

        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            progressPanel1.Visible = true;
            if (!backgroundWorker1.IsBusy)
                backgroundWorker1.RunWorkerAsync();
        }



        private void txtParameter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Global.OnKeyPressEveToPopup(e))
            {
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "PARAMETER_ID,PARAMETER_NAME";
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                this.Cursor = Cursors.WaitCursor;

                FrmSearch.mDTab = ObjTrn.GetParameterDiscountData("PARAMETER", "", "", "", 0, 0, Val.ToString(CmbPricingType.Text));

                FrmSearch.mStrColumnsToHide = "";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.DRow != null)
                {
                    txtParameter.Text = Val.ToString(FrmSearch.DRow["PARAMETER_ID"]);
                    txtParameter.Tag = Val.ToString(FrmSearch.DRow["PARAMETER_ID"]);

                    txtRapDate.Tag = string.Empty;
                    txtRapDate.Text = string.Empty;
                    txtShape.Tag = string.Empty;
                    txtShape.Text = string.Empty;
                    txtSize.Tag = string.Empty;
                    txtSize.Text = string.Empty;
                }

                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
        }

        private void txtRapDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Global.OnKeyPressEveToPopup(e))
            {
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "RAPDATE";
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                this.Cursor = Cursors.WaitCursor;

                FrmSearch.mDTab = ObjTrn.GetParameterDiscountData("RAPDATE", Val.ToString(txtParameter.Tag), "", "", 0, 0, Val.ToString(CmbPricingType.Text));

                FrmSearch.mStrColumnsToHide = "";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.DRow != null)
                {
                    txtRapDate.Text = Val.ToString(FrmSearch.DRow["RAPDATE"]);
                    txtRapDate.Tag = Val.ToString(FrmSearch.DRow["RAPDATE"]);

                    txtShape.Tag = string.Empty;
                    txtShape.Text = string.Empty;
                    txtSize.Tag = string.Empty;
                    txtSize.Text = string.Empty;
                }

                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
        }

        private void txtShape_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Global.OnKeyPressEveToPopup(e))
            {
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "SHAPE";
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                this.Cursor = Cursors.WaitCursor;

                FrmSearch.mDTab = ObjTrn.GetParameterDiscountData("SHAPE", Val.ToString(txtParameter.Tag), Val.SqlDate(txtRapDate.Text), "", 0, 0, Val.ToString(CmbPricingType.Text));

                FrmSearch.mStrColumnsToHide = "";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.DRow != null)
                {
                    txtShape.Text = Val.ToString(FrmSearch.DRow["SHAPE"]);
                    txtShape.Tag = Val.ToString(FrmSearch.DRow["SHAPE_ID"]);

                    txtSize.Tag = string.Empty;
                    txtSize.Text = string.Empty;
                }

                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
        }

        private void txtSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Global.OnKeyPressEveToPopup(e))
            {
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "SIZE";
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                this.Cursor = Cursors.WaitCursor;

                FrmSearch.mDTab = ObjTrn.GetParameterDiscountData("SIZE", Val.ToString(txtParameter.Tag), Val.SqlDate(txtRapDate.Text), txtShape.Text, 0, 0, Val.ToString(CmbPricingType.Text));

                FrmSearch.mStrColumnsToHide = "";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.DRow != null)
                {
                    txtSize.Text = Val.ToString(FrmSearch.DRow["SIZE"]);
                    txtSize.Tag = Val.ToString(FrmSearch.DRow["SIZE"]);
                }

                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
        }

        private void txtParameter_Validated(object sender, EventArgs e)
        {
            if (Val.ToString(txtParameter.Tag) == "ORIGINAL_RAP")
            {
                PanelRaprate.Visible = true;
                PnlHeadDetail1.Visible = false;
                BtnAddNewRow.Visible = false;
                GrdDet.OptionsBehavior.Editable = false;
                pnlBrowse.Visible = true; //Added by Daksha on 16/01/2023
            }
            else
            {
                PanelRaprate.Visible = false;
                PnlHeadDetail1.Visible = true;
                BtnAddNewRow.Visible = true;
                GrdDet.OptionsBehavior.Editable = true;
                pnlBrowse.Visible = false; //Added by Daksha on 16/01/2023
            }
        }

        private void CmbPricingType_Validated(object sender, EventArgs e)
        {
            try
            {
                txtRapDate.Tag = string.Empty;
                txtRapDate.Text = string.Empty;
                txtShape.Tag = string.Empty;
                txtShape.Text = string.Empty;
                txtSize.Tag = string.Empty;
                txtSize.Text = string.Empty;       
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }
        public void AddFancyShapeDetail(ExcelWorksheet worksheet, DataTable pDtab)
        {
            Color BackColor = Color.LightGray;
            Color FontColor = Color.Black;
            string FontName = "verdana";
            float FontSize = 8;

            int StartRow = 1;
            int StartColumn = 1;
            int EndRow = 0;
            int StartHeaderColumn = 1;

            EndRow = StartRow;
            int intCnt = 0;
            DataTable DTabDiscountDistinct = pDtab.DefaultView.ToTable(true, "SIZE", "SHAPETYPE");

            foreach (DataRow DRowDisc in DTabDiscountDistinct.Rows)
            {
                if (intCnt == 0)
                {
                    intCnt = 1;
                    StartColumn = 1;
                    StartHeaderColumn = 1;
                    EndRow = StartRow;
                }
                else
                {
                    intCnt = 0;
                    StartColumn = 15;
                    StartHeaderColumn = 15;
                    StartRow = EndRow;
                }

                string StrSize = Val.ToString(DRowDisc["SIZE"]);
                string StrShapeType = Val.ToString(DRowDisc["SHAPETYPE"]);

                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Value = "RAPAPORT : ( " + StrSize + " ) : " + txtRapDate.Text + " [ " + StrShapeType + " ]";
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Merge = true;

                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Font.Name = FontName;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Font.Size = FontSize;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Font.Bold = true;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Font.Color.SetColor(FontColor);

                StartRow = StartRow + 1;
                StartColumn = StartColumn + 1;

                DataRow[] UDRow = DTabDiscountData.Select("SIZE='" + StrSize + "' AND SHAPETYPE = '" + StrShapeType + "'");
                if (UDRow.Length == 0)
                {
                    continue;
                }

                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Value = "FL";
                worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = "IF";
                worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = "VVS1";
                worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = "VVS2";
                worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = "VS1";
                worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = "VS2";
                worksheet.Cells[StartRow, StartColumn + 6, StartRow, StartColumn + 6].Value = "SI1";
                worksheet.Cells[StartRow, StartColumn + 7, StartRow, StartColumn + 7].Value = "SI2";
                worksheet.Cells[StartRow, StartColumn + 8, StartRow, StartColumn + 8].Value = "SI3";
                worksheet.Cells[StartRow, StartColumn + 9, StartRow, StartColumn + 9].Value = "I1";
                worksheet.Cells[StartRow, StartColumn + 10, StartRow, StartColumn + 10].Value = "I2";
                worksheet.Cells[StartRow, StartColumn + 11, StartRow, StartColumn + 11].Value = "I3";
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 11].Style.Font.Bold = true;

                StartRow = StartRow + 1;

                foreach (DataRow DRowDetail in UDRow)
                {
                    worksheet.Cells[StartRow, StartHeaderColumn, StartRow, StartHeaderColumn].Value = Val.ToString(DRowDetail["C_NAME"]);
                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Value = Val.Val(DRowDetail["Q1"]);
                    worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = Val.Val(DRowDetail["Q2"]);
                    worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = Val.Val(DRowDetail["Q3"]);
                    worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = Val.Val(DRowDetail["Q4"]);
                    worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = Val.Val(DRowDetail["Q5"]);
                    worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = Val.Val(DRowDetail["Q6"]);
                    worksheet.Cells[StartRow, StartColumn + 6, StartRow, StartColumn + 6].Value = Val.Val(DRowDetail["Q7"]);
                    worksheet.Cells[StartRow, StartColumn + 7, StartRow, StartColumn + 7].Value = Val.Val(DRowDetail["Q8"]);
                    worksheet.Cells[StartRow, StartColumn + 8, StartRow, StartColumn + 8].Value = Val.Val(DRowDetail["Q9"]);
                    worksheet.Cells[StartRow, StartColumn + 9, StartRow, StartColumn + 9].Value = Val.Val(DRowDetail["Q10"]);
                    worksheet.Cells[StartRow, StartColumn + 10, StartRow, StartColumn + 10].Value = Val.Val(DRowDetail["Q11"]);
                    worksheet.Cells[StartRow, StartColumn + 11, StartRow, StartColumn + 11].Value = Val.Val(DRowDetail["Q12"]);

                    worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.Font.Name = FontName;
                    worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.Font.Size = FontSize;
                    worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.Font.Color.SetColor(FontColor);
                    worksheet.Cells[StartRow, StartHeaderColumn, StartRow, StartHeaderColumn].Style.Font.Bold = true;

                    StartRow = StartRow + 1;

                }
                StartRow = StartRow + 1;
            }

        }

        private void btnExportWithFormat_Click(object sender, EventArgs e)
        {
            try
            {
                if (DTabDiscountData.Rows.Count == 0)
                {  
                    Global.Message("No Detail Found For Export");
                    return;
                }

                string StrFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.LightGray;
                Color FontColor = Color.Black;
                string FontName = "verdana";
                float FontSize = 8;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int StartHeaderColumn = 1;

                this.Cursor = Cursors.WaitCursor;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Round");
                    ExcelWorksheet worksheetFancy = xlPackage.Workbook.Worksheets.Add("Fancy");

                    StartRow = 1;
                    EndRow = StartRow;
                    StartColumn = 1;

                    DataTable DTabFancy = DTabDiscountData.Select("SHAPETYPE = 'Fancy'").CopyToDataTable();
                    DataTable DTabDiscountDistinct = DTabDiscountData.DefaultView.ToTable(true, "SIZE","SHAPETYPE");  
                    DTabDiscountDistinct = DTabDiscountDistinct.Select("SHAPETYPE = 'Round'").CopyToDataTable();
                   // DTabDiscountDistinct.DefaultView.Sort = "SHAPETYPE DESC"; DTabDiscountDistinct = DTabDiscountDistinct.DefaultView.ToTable();

                    int intCnt = 0;
                    foreach (DataRow DRowDisc in DTabDiscountDistinct.Rows)
                    {
                        if (intCnt == 0)
                        {
                            intCnt = 1;
                            StartColumn = 1;
                            StartHeaderColumn = 1;
                            EndRow = StartRow;
                        }
                        else
                        {
                            intCnt = 0;
                            StartColumn = 15;
                            StartHeaderColumn = 15;
                            StartRow = EndRow;
                        }
                        
                        string StrSize = Val.ToString(DRowDisc["SIZE"]);
                        string StrShapeType = Val.ToString(DRowDisc["SHAPETYPE"]);

                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Value = "RAPAPORT : ( " + StrSize + " ) : " + txtRapDate.Text + " [ " + StrShapeType+ " ]"; 
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Merge = true;

                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Font.Name = FontName;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Font.Size = FontSize;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Font.Bold = true;

                      //  worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        //worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Fill.PatternColor.SetColor(Color.FromArgb(225, 150, 150));
                       // worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(225, 150, 150));
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Font.Color.SetColor(FontColor);

                        StartRow = StartRow + 1;
                        StartColumn = StartColumn + 1;
                       
                        DataRow[] UDRow = DTabDiscountData.Select("SIZE='" + StrSize + "' AND SHAPETYPE = '" + StrShapeType + "'");
                        if (UDRow.Length == 0)
                        {
                            continue;
                        }

                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Value = "FL";
                        worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = "IF";
                        worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = "VVS1";
                        worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = "VVS2";
                        worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = "VS1";
                        worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = "VS2";
                        worksheet.Cells[StartRow, StartColumn + 6, StartRow, StartColumn + 6].Value = "SI1";
                        worksheet.Cells[StartRow, StartColumn + 7, StartRow, StartColumn + 7].Value = "SI2";
                        worksheet.Cells[StartRow, StartColumn + 8, StartRow, StartColumn + 8].Value = "SI3";
                        worksheet.Cells[StartRow, StartColumn + 9, StartRow, StartColumn + 9].Value = "I1";
                        worksheet.Cells[StartRow, StartColumn + 10, StartRow, StartColumn + 10].Value = "I2";
                        worksheet.Cells[StartRow, StartColumn + 11, StartRow, StartColumn + 11].Value = "I3";
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 11].Style.Font.Bold = true;

                        StartRow = StartRow + 1;

                        foreach (DataRow DRowDetail in UDRow)
                        {
                            worksheet.Cells[StartRow,  StartHeaderColumn, StartRow, StartHeaderColumn].Value = Val.ToString(DRowDetail["C_NAME"]);
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Value = Val.Val(DRowDetail["Q1"]);
                            worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = Val.Val(DRowDetail["Q2"]);
                            worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = Val.Val(DRowDetail["Q3"]);
                            worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = Val.Val(DRowDetail["Q4"]);
                            worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = Val.Val(DRowDetail["Q5"]);
                            worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = Val.Val(DRowDetail["Q6"]);
                            worksheet.Cells[StartRow, StartColumn + 6, StartRow, StartColumn + 6].Value = Val.Val(DRowDetail["Q7"]);
                            worksheet.Cells[StartRow, StartColumn + 7, StartRow, StartColumn + 7].Value = Val.Val(DRowDetail["Q8"]);
                            worksheet.Cells[StartRow, StartColumn + 8, StartRow, StartColumn + 8].Value = Val.Val(DRowDetail["Q9"]);
                            worksheet.Cells[StartRow, StartColumn + 9, StartRow, StartColumn + 9].Value = Val.Val(DRowDetail["Q10"]);
                            worksheet.Cells[StartRow, StartColumn + 10, StartRow, StartColumn + 10].Value = Val.Val(DRowDetail["Q11"]);
                            worksheet.Cells[StartRow, StartColumn + 11, StartRow, StartColumn + 11].Value = Val.Val(DRowDetail["Q12"]);

                            worksheet.Cells[StartRow -1, StartColumn-1, StartRow, StartColumn + 11].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow -1, StartColumn-1, StartRow, StartColumn + 11].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow -1, StartColumn-1, StartRow, StartColumn + 11].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow -1, StartColumn-1, StartRow, StartColumn + 11].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow -1, StartColumn-1, StartRow, StartColumn + 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells[StartRow -1, StartColumn-1, StartRow, StartColumn + 11].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            worksheet.Cells[StartRow -1, StartColumn-1, StartRow, StartColumn + 11].Style.Font.Name = FontName;
                            worksheet.Cells[StartRow -1, StartColumn-1, StartRow, StartColumn + 11].Style.Font.Size = FontSize;
                          //  worksheet.Cells[StartRow -1, StartColumn-1, StartRow, StartColumn + 11].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                           // worksheet.Cells[StartRow -1, StartColumn-1, StartRow, StartColumn + 11].Style.Fill.PatternColor.SetColor(Color.FromArgb(225, 150, 150));
                           // worksheet.Cells[StartRow -1, StartColumn-1, StartRow, StartColumn + 11].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(225, 150, 150));
                            worksheet.Cells[StartRow -1, StartColumn-1, StartRow, StartColumn + 11].Style.Font.Color.SetColor(FontColor);
                            worksheet.Cells[StartRow, StartHeaderColumn, StartRow, StartHeaderColumn].Style.Font.Bold = true;

                            StartRow = StartRow + 1;

                        }
                        StartRow = StartRow + 1;                      
                    }

                    AddFancyShapeDetail(worksheetFancy, DTabFancy);
                    xlPackage.Save();

                    System.Diagnostics.Process.Start(StrFilePath, "CMD");
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        //Added by Daksha on 16/01/2023
        private void btnBrowseRound_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog Open = new OpenFileDialog();
                Open.Filter = "CSV Files|*.csv;";
                if (Open.ShowDialog() == DialogResult.OK)
                {
                    string strFile = Open.FileName;
                    string strNewFile = System.Windows.Forms.Application.StartupPath + "\\Round.csv";
                    if (File.Exists(strNewFile))
                    {
                        File.Delete(strNewFile);
                    }
                    File.Copy(strFile, strNewFile);
                    txtRoundFile.Text = strNewFile;
                    //DTabRound = Global.GetDataTableFromCsv(txtRoundFile.Text, false);
                }
            }
            catch (Exception ex)
            {                
                Global.Message(ex.Message);
            }            
        }

        private void btnBrowsePear_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog Open = new OpenFileDialog();
                Open.Filter = "CSV Files|*.csv;";
                if (Open.ShowDialog() == DialogResult.OK)
                {
                    string strFile = Open.FileName;
                    string strNewFile = System.Windows.Forms.Application.StartupPath + "\\Pear.csv";
                    if (File.Exists(strNewFile))
                    {
                        File.Delete(strNewFile);
                    }
                    File.Copy(strFile, strNewFile);
                    txtPearFile.Text = strNewFile;
                    //DTabPear = Global.GetDataTableFromCsv(txtPearFile.Text, false);
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }           
        }
        //End as Daksha
    }
}
