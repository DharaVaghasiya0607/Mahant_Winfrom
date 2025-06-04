using BusLib;
using BusLib.Configuration;
using BusLib.Transaction;
using DevExpress.XtraPrinting;
using Google.API.Translate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusLib.TableName;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.Utils;
using MahantExport.Utility;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using System.Runtime.InteropServices;
using System.Drawing.Printing;
using BarcodeLib.Barcode;

using System.Net;
using System.Text.RegularExpressions;

namespace MahantExport.Stock
{
    public partial class FrmStockBarcodePrint : DevControlLib.cDevXtraForm
    {
        [DllImport("Winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetDefaultPrinter(string printerName);
        private string GetDefaultPrinter()
        {
            PrinterSettings settings = new PrinterSettings();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                settings.PrinterName = printer;
                if (settings.IsDefaultPrinter)
                    return printer;
            }
            return string.Empty;
        }

        private bool isPasteAction = false;
        private const Keys PasteKeys = Keys.Control | Keys.V;


        BODevGridSelection ObjGridSelection;
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        DataTable DtabPacket = new DataTable();
        DataTable DtabStockDetail = new DataTable();
        BOTRN_StockUpload ObjStone = new BOTRN_StockUpload();
        String PasteData = "";
        IDataObject PasteclipData = Clipboard.GetDataObject();
        int IntIsChkChecked = 0;

        #region Property Settings

        public FrmStockBarcodePrint()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            this.Show();
            PanelStoneRFID.Visible = false;

            DtabStockDetail = ObjStone.GetDataForStockBarcodePrintWithRFID("None", "None");
            MainGrid.DataSource = DtabStockDetail;
            MainGrid.Refresh();

        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        #endregion


        private void BtnSearch_Click(object sender, EventArgs e)

        {
            try
            {

                if (IntIsChkChecked == 1)
                {

                    string StoneNo = txtStoneCertiNo.Text;
                    string StoneRFIDNO = txtStoneRFIDTagNo.Text;

                    DataTable DtabData = new BOTRN_StockUpload().GetDataForStockBarcodePrintWithRFID(StoneNo, StoneRFIDNO);

                    if (DtabData.Rows.Count <= 0)
                    {
                        Global.Message("No Data Found");
                        return;
                    }

                    DataRow DRow = DtabData.Rows[0];


                    //Check That Packet Already Exists In Grid then Skip - 07-06-2019
                    IEnumerable<DataRow> rowsNew = DtabStockDetail.Rows.Cast<DataRow>();
                    if (rowsNew.Where(s => Val.ToString(s["STOCK_ID"]) == Val.ToString(DRow["STOCK_ID"])).Count() > 0)
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("This Stone Is Already Exist.");
                        txtStoneCertiNo.Text = string.Empty;
                        txtStoneRFIDTagNo.Text = string.Empty;
                        txtStoneCertiNo.Focus();
                        return;
                    }

                    DataRow DRNew = DtabStockDetail.NewRow();

                    foreach (DataColumn DCol in DtabStockDetail.Columns)
                    {
                        DRNew[DCol.ColumnName] = DRow[DCol.ColumnName];
                    }

                    DtabStockDetail.Rows.Add(DRNew);
                    MainGrid.DataSource = DtabStockDetail;
                    MainGrid.Refresh();
                    GrdDet.BestFitColumns();

                    txtStoneCertiNo.Text = string.Empty;
                    txtStoneRFIDTagNo.Text = string.Empty;
                    txtStoneCertiNo.Focus();


                    if (MainGrid.RepositoryItems.Count == 0)
                    {
                        ObjGridSelection = new BODevGridSelection();
                        ObjGridSelection.View = GrdDet;
                        ObjGridSelection.ClearSelection();
                        ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                    }
                    else
                    {
                        ObjGridSelection.ClearSelection();
                    }
                    GrdDet.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;
                    if (ObjGridSelection != null)
                    {
                        ObjGridSelection.ClearSelection();
                        ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                    }

                    this.Cursor = Cursors.Default;
                    return;
                }


                if (txtStoneCertiMFGMemo.Text.Trim().Length == 0)
                {
                    Global.Message("StoneNo / CertiNo / SerialNo / MemoNo Is Required");
                    txtStoneCertiMFGMemo.Focus();
                    return;
                }
                this.Cursor = Cursors.WaitCursor;

                //LiveStockProperty mLStockProperty = new LiveStockProperty();
                //if (RbtStoneNo.Checked == true)
                //{
                //    mLStockProperty.STOCKNO = txtStoneCertiMFGMemo.Text;
                //    mLStockProperty.LABREPORTNO = string.Empty;
                //    mLStockProperty.SERIALNO = string.Empty;
                //    mLStockProperty.MEMONO = string.Empty;
                //}
                //else if (RbtCertiNo.Checked == true)
                //{
                //    mLStockProperty.STOCKNO = string.Empty;
                //    mLStockProperty.LABREPORTNO = txtStoneCertiMFGMemo.Text;
                //    mLStockProperty.SERIALNO = string.Empty;
                //    mLStockProperty.MEMONO = string.Empty;
                //}
                //else if (RbtMFGNo.Checked == true)
                //{
                //    mLStockProperty.STOCKNO = string.Empty;
                //    mLStockProperty.LABREPORTNO = string.Empty;
                //    mLStockProperty.SERIALNO = txtStoneCertiMFGMemo.Text;
                //    mLStockProperty.MEMONO = string.Empty;
                //}
                //else if (RbtMemoNo.Checked == true)
                //{
                //    mLStockProperty.STOCKNO = string.Empty;
                //    mLStockProperty.LABREPORTNO = string.Empty;
                //    mLStockProperty.SERIALNO = string.Empty;
                //    mLStockProperty.MEMONO = txtStoneCertiMFGMemo.Text;
                //}

                if (txtStoneCertiMFGMemo.Text.EndsWith(","))
                {
                    txtStoneCertiMFGMemo.Text = txtStoneCertiMFGMemo.Text.Substring(0, txtStoneCertiMFGMemo.Text.Length - 1);
                }

                string[] SplitStoneNo = txtStoneCertiMFGMemo.Text.Split(',');
                DataTable DTabStoneNo = new DataTable("Table");
                DTabStoneNo.Columns.Add(new DataColumn("SRNO", typeof(int)));
                DTabStoneNo.Columns.Add(new DataColumn("STONENO", typeof(string)));

                for (int i = 0; i < SplitStoneNo.Length; i++)
                {
                    DataRow DR = DTabStoneNo.NewRow();
                    DR["SRNO"] = i + 1;
                    DR["STONENO"] = SplitStoneNo[i].ToString().Replace("\r\n", "");
                    DTabStoneNo.Rows.Add(DR);
                }

                string StrXMLStoneNo = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabStoneNo.WriteXml(sw);
                    StrXMLStoneNo = sw.ToString();
                }


                DtabStockDetail = new BOTRN_StockUpload().GetDataForStockBarcodePrint(StrXMLStoneNo);
                MainGrid.DataSource = DtabStockDetail;
                MainGrid.Refresh();
                GrdDet.BestFitColumns();

                if (MainGrid.RepositoryItems.Count == 0)
                {
                    ObjGridSelection = new BODevGridSelection();
                    ObjGridSelection.View = GrdDet;
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                }
                else
                {
                    ObjGridSelection.ClearSelection();
                }
                GrdDet.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;
                if (ObjGridSelection != null)
                {
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private DataTable GetTableOfSelectedRows(GridView view, Boolean IsSelect)
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

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBestFit_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            GrdDet.BestFitColumns();
            this.Cursor = Cursors.Default;
        }


        private void BtnBarcodePrintCurrEmp_Click(object sender, EventArgs e)
        {
            try
            {
                
                DataTable DTab = GetTableOfSelectedRows(GrdDet, true);

                if (DTab.Rows.Count == 0)
                {
                    Global.Message("Please Select at lease One Row For Barcode Print");
                    return;
                }

                if (Global.Confirm("Are you Sure You Want For Print Barcode of All Selected Packets?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                string StrBatchFileName = ""; string DefaultPrinter = "";
                StrBatchFileName = Application.StartupPath + "\\TSC_MakableBarcodeNew.txt ";

                string[] lines = File.ReadAllLines(StrBatchFileName);
                DefaultPrinter = GetDefaultPrinter();


                this.Cursor = Cursors.WaitCursor;
                List<StiReport> rps = new List<StiReport>();

                int IntCount = 0;
                foreach (DataRow DRow in DTab.Rows)
                {
                    string StrStoneNo = Val.ToString(DRow["PARTYSTOCKNO"]);
                    string StrLab = Val.ToString(DRow["LABNAME"]) + " " + Val.ToString(DRow["LABREPORTNO"]);
                    string StrShape = Val.ToString(DRow["SHAPENAME"]);
                    string StrColCla = Val.ToString(DRow["COLORNAME"]) + "-" + Val.ToString(DRow["CLARITYNAME"]);
                    string StrWeight = Val.ToString(DRow["CARAT"]);
                    string StrTablePer = Val.ToString(DRow["TABLEPER"]);
                    string StrTableDepth = Val.ToString(DRow["DEPTHPER"]);
                    string StrMeasurment = Val.ToString(DRow["MEASUREMENT"]);
                    //string StrCut = Val.ToString(DRow["CUTNAME"]);
                    //string StrPol = Val.ToString(DRow["POLNAME"]);
                    string StrSymm = Val.ToString(DRow["CUTNAME"]) +"-"+ Val.ToString(DRow["POLNAME"])+"-" + Val.ToString(DRow["SYMNAME"]);
                    string StrCRHeight = Val.ToString(DRow["CRHEIGHT"]);
                    string StrPavHeight = Val.ToString(DRow["PAVHEIGHT"]);
                    string StrRatio = Val.ToString(DRow["RATIO"]);

                    IntCount++;

                    StiReport report = new StiReport();
                    string BarcodeName = "PrintStockBarcode";
                    report.Load(Application.StartupPath + "\\Barcode\\" + BarcodeName + ".mrt");
                    report.Compile();
                    report.RequestParameters = false;

                    foreach (Stimulsoft.Report.Dictionary.StiSqlDatabase item in report.CompiledReport.Dictionary.Databases)
                    {
                        item.ConnectionString = BusLib.Configuration.BOConfiguration.ConnectionString;
                    }

                    report["StoneID"] = "'" + StrStoneNo + "'";
                    report["Lab"] = "'" + StrLab + "'";
                    report["Shape"] = "'" + StrShape + "'";
                    report["ColCla"] = "'" + StrColCla + "'";
                    report["Weight"] = StrWeight;
                    report["TABLEPER"] = StrTablePer;
                    report["TblDepth"] = StrTableDepth;
                    report["Measurment"] = "'" + StrMeasurment + "'";
                    //report["Cut"] = "'" + StrCut + "'";
                    //report["Pol"] = "'" + StrPol + "'";
                    report["Symm"] = "'" + StrSymm + "'";
                    report["CRHeight"] = StrCRHeight;
                    report["PavHeight"] = StrPavHeight;
                    report["Ratio"] = StrRatio;

                    StiSqlDatabase sql = new StiSqlDatabase("Connection", BusLib.Configuration.BOConfiguration.ConnectionString);
                    sql.Alias = "Connection";
                    report.CompiledReport.Dictionary.Databases.Clear();
                    report.CompiledReport.Dictionary.Databases.Add(sql);

                    report.PreviewMode = StiPreviewMode.StandardAndDotMatrix;
                    report.Render(false);
                    report.Print(false);
                    //rps.Add(report);
                }

                //StiReport singleFile = new StiReport();
                //singleFile.NeedsCompiling = false;
                //singleFile.IsRendered = true;

                //Stimulsoft.Report.Units.StiUnit newUnit = Stimulsoft.Report.Units.StiUnit.GetUnitFromReportUnit(singleFile.ReportUnit);
                //singleFile.RenderedPages.Clear();
                //foreach (StiReport rpt in rps)
                //{
                //    foreach (Stimulsoft.Report.Components.StiPage page in rpt.CompiledReport.RenderedPages)
                //    {
                //        page.Report = singleFile;
                //        page.NewGuid();
                //        Stimulsoft.Report.Units.StiUnit oldUnit = Stimulsoft.Report.Units.StiUnit.GetUnitFromReportUnit(rpt.ReportUnit);
                //        if (singleFile.ReportUnit != rpt.ReportUnit)
                //        {
                //            page.Convert(oldUnit, newUnit);
                //            page.Watermark.Image = null;
                //        }
                //        singleFile.RenderedPages.Add(page);
                //    }
                //}

                ////SetDefaultPrinter(lines[0]);
                //singleFile.Print(false);
                ////SetDefaultPrinter(DefaultPrinter);
                //rps.Clear();
                //rps = null;
                //Comment By Gunjan:17/01/2024
                //foreach (DataRow DRow in DTab.Rows)
                //{
                //    Global.BombayPrintBarcodePrint(DRow);
                //}
                //End As Gunjan
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.ToString());
            }
        }




        private void txtStoneCertiMFGMemo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            {
                IDataObject clipData = Clipboard.GetDataObject();
                String Data = Convert.ToString(clipData.GetData(System.Windows.Forms.DataFormats.Text));
                String str1 = Data.Replace("\r\n", ",");                   //data.Replace(\n, ",");
                str1 = str1.Trim();
                str1 = str1.TrimEnd();
                str1 = str1.TrimStart();
                str1 = str1.TrimEnd(',');
                str1 = str1.TrimStart(',');
                txtStoneCertiMFGMemo.Text = str1;
            }
            lblTotalCount.Text = "(" + txtStoneCertiMFGMemo.Text.Split(',').Length + ")";
        }

        private void txtStoneCertiMFGMemo_MouseDown(object sender, MouseEventArgs e)
        {
            if (txtStoneCertiMFGMemo.Focus())
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    PasteData = Convert.ToString(PasteclipData.GetData(System.Windows.Forms.DataFormats.Text));
                }
            }
            lblTotalCount.Text = "(" + txtStoneCertiMFGMemo.Text.Split(',').Length + ")";
        }

        private void txtStoneCertiMFGMemo_TextChanged(object sender, EventArgs e)
        {
            //if (txtStoneCertiMFGMemo.Text.Length > 0 && Convert.ToString(PasteData) != "")
            //{
            //    txtStoneCertiMFGMemo.SelectAll();
            //    String str1 = PasteData.Replace("\r\n", ",");                   //data.Replace(\n, ",");
            //    str1 = str1.Trim();
            //    str1 = str1.TrimEnd();
            //    str1 = str1.TrimStart();
            //    str1 = str1.TrimEnd(',');
            //    str1 = str1.TrimStart(',');
            //    txtStoneCertiMFGMemo.Text = str1;
            //    PasteData = "";
            //}
            try
            {
                
                    String str1 = Val.ToString(txtStoneCertiMFGMemo.Text);
                    string result = Regex.Replace(str1, @"\r\n?|\n", ",");
                    if (result.EndsWith(",,"))
                    {
                        result = result.Remove(result.Length - 1);
                    }
                    txtStoneCertiMFGMemo.Text = result;
                if (isPasteAction)
                {
                    isPasteAction = false;
                    txtStoneCertiNo.Select(txtStoneCertiNo.Text.Length, 0);
                }
                lblTotalCount.Text = "(" + txtStoneCertiMFGMemo.Text.Split(',').Length + ")";
            }
            catch (Exception EX)
            {
                Global.MessageError(EX.Message);
                return;
            }
            

        }

        private void BtnCertificateBPrint_Click(object sender, EventArgs e)
        {
            DataTable DTab = GetTableOfSelectedRows(GrdDet, true);

            if (DTab.Rows.Count == 0)
            {
                Global.Message("Please Select at lease One Row For Barcode Print");
                return;
            }

            if (Global.Confirm("Are you Sure You Want For Print Barcode of All Selected Packets?") == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            foreach (DataRow DRow in DTab.Rows)
            {
                Global.CertificateBarcodePrint(DRow);
            }
            Global.Message("Print Successfully");
        }

        private void toolTipController1_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            if (e.SelectedControl != MainGrid) return;
            ToolTipControlInfo info = null;
            try
            {
                GridView view = MainGrid.GetViewAt(e.ControlMousePosition) as GridView;
                if (view == null) return;
                GridHitInfo hi = view.CalcHitInfo(e.ControlMousePosition);
                if (hi.HitTest == GridHitTest.RowCell && hi.Column.FieldName == "BILLPARTYCODE")
                {
                    info = new ToolTipControlInfo(hi.RowHandle.ToString() + hi.Column.FieldName, Val.ToString(view.GetRowCellValue(hi.RowHandle, "BILLPARTYNAME")));
                    return;
                }
            }
            finally
            {
                e.Info = info;
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtStoneCertiNo.Text = string.Empty;
            txtStoneRFIDTagNo.Text = string.Empty;
            txtStoneCertiMFGMemo.Text = string.Empty;
            lblTotalCount.Text = "(0)";
            RbtStoneNo.Checked = true;
        }

        private void ChkScanBarcode_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkScanBarcode.Checked == true)
            {
                PanelStoneRFID.Visible = true;
                PanelStone.Visible = false;
                IntIsChkChecked = 1;
                DtabStockDetail.Clear();
                txtStoneCertiMFGMemo.Clear();
                MainGrid.DataSource = DtabStockDetail;
                MainGrid.Refresh();
                txtStoneCertiNo.Focus();
            }
            else
            {
                PanelStoneRFID.Visible = false;
                PanelStone.Visible = true;
                IntIsChkChecked = 0;
                DtabStockDetail.Clear();
                txtStoneCertiNo.Clear();
                txtStoneRFIDTagNo.Clear();
                MainGrid.DataSource = DtabStockDetail;
                MainGrid.Refresh();
            }
        }

        private void txtStoneRFIDTagNo_Validated(object sender, EventArgs e)
        {
            try
            {
                if (txtStoneCertiNo.Text.Trim().Length == 0)
                {
                    return;
                }
                if (txtStoneRFIDTagNo.Text.Trim().Length == 0)
                {
                    txtStoneCertiNo.Focus();
                   
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                //BtnSearch.PerformClick();
                BtnSearch_Click(null, null);
                
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                txtStoneCertiNo.Focus();
            }
        }

        private void BtnPacketPrint_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTab = GetTableOfSelectedRows(GrdDet, true);


                if (DTab.Rows.Count == 0)
                {
                    Global.Message("Please Select at lease One Row For Barcode Print");
                    return;
                }

                if (Global.Confirm("Are you Sure You Want For Print Barcode of All Selected Packets?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                string StrStock_ID = "";
                if (DTab.Rows.Count > 0)
                {
                    var list = DTab.AsEnumerable().Select(r => r["STOCK_ID"].ToString());
                    StrStock_ID = string.Join(",", list);
                }
                else
                {
                    StrStock_ID = Val.ToString(GrdDet.GetFocusedRowCellValue("STOCK_ID"));
                }
                DataTable DtabNew = new BOTRN_StockUpload().GetDataForPacketPrint(StrStock_ID);

                for (int i = 0; i < DtabNew.Rows.Count; i++)
                {
                    string pIntStockdNo = "";
                    var barcode = new Linear();
                    barcode.Type = BarcodeType.CODE128;
                    barcode.ShowText = false;
                    pIntStockdNo = Val.ToString(DtabNew.Rows[i]["StockNo"]);
                    barcode.Data = pIntStockdNo + '/';

                    DtabNew.Rows[i]["Stock"] = barcode.drawBarcodeAsBytes();

                    string imageUrl = Val.ToString(DtabNew.Rows[i]["ShapeImagePath"]); // Assume you have a column with the image URL
                    using (WebClient webClient = new WebClient())
                    {
                        byte[] imageBytes = DownloadRemoteImageFile(imageUrl);

                        DtabNew.Rows[i]["Image"] = imageBytes;
                    }
                }

                Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                FrmReportViewer.MdiParent = Global.gMainRef;
                FrmReportViewer.ShowForm("RPT_PacketPrint", DtabNew);
            }
            catch (Exception ex)
            {
                Global.Message(ex.ToString());
                this.Cursor = Cursors.Default;
                txtStoneCertiNo.Focus();
            }
        }
        private static byte[] DownloadRemoteImageFile(string uri)
        {
            byte[] content;
            var request = (HttpWebRequest)WebRequest.Create(uri);

            using (var response = request.GetResponse())
            using (var reader = new BinaryReader(response.GetResponseStream()))
            {
                content = reader.ReadBytes(100000);
            }

            return content;
        }

        private void txtStoneCertiMFGMemo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                isPasteAction = true;
            }
        }
    }
}
