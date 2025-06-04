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
using System.Text.RegularExpressions;
using OfficeOpenXml;
using System.Diagnostics;
using DevExpress.Data;
using System.Net;

namespace MahantExport.Stock
{
    public partial class FrmSaleDeliveryLiveStock : DevControlLib.cDevXtraForm
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
        BODevGridSelection ObjGridSelection;
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();

        private bool isPasteAction = false;
        private const Keys PasteKeys = Keys.Control | Keys.V;

        Color mSelectedColor = Color.FromArgb(192, 0, 0);
        Color mDeSelectColor = Color.Black;
        Color mSelectedBackColor = Color.FromArgb(255, 224, 192);
        Color mDSelectedBackColor = Color.White;

        DataTable DtabPacket = new DataTable();
        DataTable DtabStockDetail = new DataTable();
        BOTRN_StockUpload ObjStone = new BOTRN_StockUpload();
        String PasteData = "";
        IDataObject PasteclipData = Clipboard.GetDataObject();
        int IntIsChkChecked = 0;

        DataTable DTabDiamondType = new DataTable();

        double DouCarat = 0;
        double DouCostRapaport = 0;
        double DouCostRapaportAmt = 0;
        double DouCostDisc = 0;
        double DouCostPricePerCarat = 0;
        double DouCostAmount = 0;

        double DouSaleRapaport = 0;
        double DouSaleRapaportAmt = 0;
        double DouSaleDisc = 0;
        double DouSalePricePerCarat = 0;
        double DouSaleAmount = 0;


        double DouMemoRapaport = 0;
        double DouMemoRapaportAmt = 0;
        double DouMemoDisc = 0;
        double DouMemoPricePerCarat = 0;
        double DouMemoAmount = 0;


        #region Property Settings

        public FrmSaleDeliveryLiveStock()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            string Str = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdDetail.Name);

            if (Str != "")
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                MemoryStream stream = new MemoryStream(byteArray);
                GrdDetail.RestoreLayoutFromStream(stream);
            }

            DTabDiamondType = new DataTable();
            DTabDiamondType.Columns.Add(new DataColumn("TYPE", typeof(string)));
            DTabDiamondType.Columns.Add(new DataColumn("VALUE", typeof(string)));

            DataRow row = DTabDiamondType.NewRow();
            row["TYPE"] = "ALL";
            row["VALUE"] = "ALL";
            DTabDiamondType.Rows.Add(row);

            DataRow row1 = DTabDiamondType.NewRow();
            row1["TYPE"] = "NAT";
            row1["VALUE"] = "NATURAL";
            DTabDiamondType.Rows.Add(row1);

            DataRow row2 = DTabDiamondType.NewRow();
            row2["TYPE"] = "CVD";
            row2["VALUE"] = "CVD";
            DTabDiamondType.Rows.Add(row2);

            DataRow row3 = DTabDiamondType.NewRow();
            row3["TYPE"] = "HPHT";
            row3["VALUE"] = "HPHT";
            DTabDiamondType.Rows.Add(row3);


            DesignSystemDiamondTypeButtion(DTabDiamondType, PanelDiamondType, "TYPE", "TYPE", 26, 45);

            if (MainGrdDetail.RepositoryItems.Count == 9)
            {
                ObjGridSelection = new BODevGridSelection();
                ObjGridSelection.View = GrdDetail;
                ObjGridSelection.ISBoolApplicableForPageConcept = true;
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                GrdDetail.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;
            }
            else
            {
                if (ObjGridSelection != null)
                    ObjGridSelection.ClearSelection();
            }
            if (ObjGridSelection != null)
            {
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            }
            this.Show();
        }

        public void DesignSystemDiamondTypeButtion(DataTable DTab, Panel PNL, string pStrDisplayText, string toolTips, int pIntHeight, int pIntWidth)
        {

            if (DTab.Rows.Count == 0)
            {
                return;
            }

            PNL.Controls.Clear();

            int IntI = 0;
            foreach (DataRow DRow in DTab.Rows)
            {
                AxonContLib.cButton ValueList = new AxonContLib.cButton();
                ValueList.Text = DRow[pStrDisplayText].ToString();
                ValueList.FlatStyle = FlatStyle.Flat;
                ValueList.Width = pIntWidth;
                ValueList.Height = pIntHeight;
                ValueList.Tag = DRow["VALUE"].ToString();
                ValueList.AccessibleDescription = Val.ToString(DRow["TYPE"]);
                ValueList.ToolTips = toolTips;
                ValueList.AutoSize = true;
                ValueList.Click += new EventHandler(cButton_Click);
                ValueList.Cursor = Cursors.Hand;
                ValueList.Font = new Font("Verdana", 9, FontStyle.Regular);
                ValueList.ForeColor = mDeSelectColor;
                ValueList.BackColor = mDSelectedBackColor;

                PNL.Controls.Add(ValueList);

                IntI++;
            }
        }

        private void cButton_Click(object sender, EventArgs e)
        {
            try
            {
                AxonContLib.cButton btn = (AxonContLib.cButton)sender;
                if (btn.ForeColor == mSelectedColor)
                {
                    btn.ForeColor = mDeSelectColor;
                    btn.BackColor = mDSelectedBackColor;
                    btn.AccessibleName = "true";
                }
                else
                {
                    btn.ForeColor = mSelectedColor;
                    btn.BackColor = mSelectedBackColor;
                    btn.AccessibleName = "true";
                }
                if (MainGrdDetail.Enabled == false)
                {
                    Global.MessageError("Grid Is Unable To Update");
                    return;
                }

            }
            catch (Exception EX)
            {
                this.Cursor = Cursors.WaitCursor;
                Global.MessageError(EX.Message);
                return;
            }

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
        public string GetSelectedBtnID(Panel StrPanel)
        {
            string StrSelectedID = "";
            for (int i = 0; i < StrPanel.Controls.Count; i++)
            {
                if (StrPanel.Controls[i].BackColor == mSelectedBackColor)
                {
                    StrSelectedID += StrPanel.Controls[i].Tag + ",";
                }
            }
            return StrSelectedID;
        }
        public void RemoveSelectedBtn(Panel StrPanel)
        {
            string StrSelectedID = "";
            for (int i = 0; i < StrPanel.Controls.Count; i++)
            {
                if (StrPanel.Controls[i].BackColor == mSelectedBackColor)
                {
                    StrPanel.Controls[i].ForeColor = mDeSelectColor;
                    StrPanel.Controls[i].BackColor = mDSelectedBackColor;
                    StrPanel.Controls[i].AccessibleName = "true";
                }
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)

        {
            try
            {
                string pStrFromDate = "";
                string pStrToDate = "";
                string DIAMONDTYPE = "";

                if (DTPFromDate.Checked == true)
                {
                    pStrFromDate = Val.SqlDate(DTPFromDate.Value.ToString());
                }
                else
                {
                    pStrFromDate = "";
                }

                if (DTPToDate.Checked == true)
                {
                    pStrToDate = Val.SqlDate(DTPToDate.Value.ToString());
                }
                else
                {
                    pStrToDate = "";
                }
                DIAMONDTYPE = GetSelectedBtnID(PanelDiamondType);
                if (DIAMONDTYPE.EndsWith(","))
                {
                    DIAMONDTYPE = DIAMONDTYPE.Substring(0, DIAMONDTYPE.Length - 1);
                }
                DataTable DTab = ObjStone.GetDataForSaleDelivery("", pStrFromDate, pStrToDate, DIAMONDTYPE);
                MainGrdDetail.DataSource = DTab;
                GrdDetail.RefreshData();

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
            this.Cursor = Cursors.Default;
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
                txtStoneNo.Text = str1;
            }
            lblTotalCount.Text = "(" + txtStoneNo.Text.Split(',').Length + ")";
        }

        private void txtStoneCertiMFGMemo_MouseDown(object sender, MouseEventArgs e)
        {
            if (txtStoneNo.Focus())
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    PasteData = Convert.ToString(PasteclipData.GetData(System.Windows.Forms.DataFormats.Text));
                }
            }
            lblTotalCount.Text = "(" + txtStoneNo.Text.Split(',').Length + ")";
        }

        private void txtStoneCertiMFGMemo_TextChanged(object sender, EventArgs e)
        {
            //if (txtStoneNo.Text.Length > 0 && Convert.ToString(PasteData) != "")
            //{
            //    txtStoneNo.SelectAll();
            //    String str1 = PasteData.Replace("\r\n", ",");                   //data.Replace(\n, ",");
            //    str1 = str1.Trim();
            //    str1 = str1.TrimEnd();
            //    str1 = str1.TrimStart();
            //    str1 = str1.TrimEnd(',');
            //    str1 = str1.TrimStart(',');
            //    txtStoneNo.Text = str1;
            //    PasteData = "";
            //}

            //Comment and Added By Gunjan:16/08/2024
            //String str1 = "";
            //if (txtStoneNo.Text.Trim().Contains("\t\n"))
            //{
            //    str1 = txtStoneNo.Text.Trim().Replace("\t\n", ",");
            //}
            //else
            //{
            //    str1 = txtStoneNo.Text.Trim().Replace("\n", ",");
            //    str1 = str1.Replace("\r", "");
            //}

            //txtStoneNo.Text = str1;
            //txtStoneNo.Select(txtStoneNo.Text.Length, 0);
           
                String str1 = Val.ToString(txtStoneNo.Text);
                string result = Regex.Replace(str1, @"\r\n?|\n", ",");
                if (result.EndsWith(",,"))
                {
                    result = result.Remove(result.Length - 1);
                }
                txtStoneNo.Text = result;
            if (isPasteAction)
            {
                isPasteAction = false;
                txtStoneNo.Select(txtStoneNo.Text.Length, 0);
            }
            //End as Gunjan
            lblTotalCount.Text = "(" + txtStoneNo.Text.Split(',').Length + ")";

        }


        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtStoneNo.Text = string.Empty;
            lblTotalCount.Text = "(0)";
            RbtStoneNo.Checked = true;
            RemoveSelectedBtn(PanelDiamondType);
        }

        private void txtStoneNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    //BtnSearch_Click(null, null);
                    string pStrFromDate = "";
                    string pStrToDate = "";
                    string DIAMONDTYPE = "";

                    DataTable DTab = ObjStone.GetDataForSaleDelivery(Val.ToString(txtStoneNo.Text), pStrFromDate, pStrToDate, DIAMONDTYPE);
                    MainGrdDetail.DataSource = DTab;
                    GrdDetail.RefreshData();
                    txtStoneNo.Select(txtStoneNo.Text.Length, 0);
                    isPasteAction = true;
                }
               
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void lblSaveLayout_Click(object sender, EventArgs e)
        {
            try
            {
                Stream str = new System.IO.MemoryStream();
                GrdDetail.SaveLayoutToStream(str);
                str.Seek(0, System.IO.SeekOrigin.Begin);
                StreamReader reader = new StreamReader(str);
                string text = reader.ReadToEnd();

                int IntRes = new BOTRN_StockUpload().SaveGridLayout(this.Name, GrdDetail.Name, text);
                if (IntRes != -1)
                {
                    Global.Message("Layout Successfully Saved");
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void lblDefaultLayout_Click(object sender, EventArgs e)
        {
            try
            {
                int IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdDetail.Name);
                if (IntRes != -1)
                {
                    Global.Message("Layout Successfully Deleted");
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void GrdDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down && e.Control && e.Shift)
                {
                    GridView view = sender as GridView;

                    if (view != null)
                    {
                        int startRowHandle = view.FocusedRowHandle;
                        GridColumn focusedColumn = view.FocusedColumn;

                        // Start selecting from the current focused cell downwards
                        for (int i = startRowHandle; i < view.RowCount; i++)
                        {
                            // Select only the cell in the focused column for each row
                            view.SelectCell(i, focusedColumn);
                        }
                    }
                }
                if (e.KeyCode == Keys.Up && e.Control && e.Shift)
                {
                    GridView view = sender as GridView;

                    if (view != null)
                    {
                        int startRowHandle = view.FocusedRowHandle;
                        GridColumn focusedColumn = view.FocusedColumn;

                        // Start selecting from the current focused cell upwards
                        for (int i = startRowHandle; i >= 0; i--)
                        {
                            // Select only the cell in the focused column for each row
                            view.SelectCell(i, focusedColumn);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void BtnPickupPending_Click(object sender, EventArgs e)
        {
            try
            {
                string pStrFromDate = "";
                string pStrToDate = "";
                if (DTPFromDate.Checked == true)
                {
                    pStrFromDate = Val.SqlDate(DTPFromDate.Value.ToString());
                }
                else
                {
                    pStrFromDate = "";
                }

                if (DTPToDate.Checked == true)
                {
                    pStrToDate = Val.SqlDate(DTPToDate.Value.ToString());
                }
                else
                {
                    pStrToDate = "";
                }
                DataTable DTab = ObjStone.GetDataForPickUpPending(Val.ToString(txtStoneNo.Text), pStrFromDate, pStrToDate);
                MainGrdDetail.DataSource = DTab;
                //GrdDetail.RefreshData();

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }


        private void BtnExcelExport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);

                if (DtInvDetail.Rows.Count <= 0)
                {
                    Global.Message("Please Select Records to Export");
                    return;
                }
                var list = DtInvDetail.AsEnumerable().Select(r => r["MEMODETAIL_ID"].ToString());
                string MemoDetailIds = string.Join(",", list);
                string StrFilePath = ReportListExportExcel(MemoDetailIds);

                if (StrFilePath != "")
                {
                    if (Global.Confirm("Do You Want To Open File ? ") == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(StrFilePath, "CMD");
                    }
                }
                if (ObjGridSelection != null)
                {
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        public string ReportListExportExcel(string StrMemoFetail_IDs, string StrFilePath = "")
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                //DataTable DTabDetail = ObjMemo.GetDataForSaleReport(strMemoDetailIds);
                DataTable DTabDetail = ObjMemo.GetDataForSaleReportNew("", StrMemoFetail_IDs);


                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }

                this.Cursor = Cursors.WaitCursor;
                if (StrFilePath == "")
                {
                    SaveFileDialog svDialog = new SaveFileDialog();
                    svDialog.DefaultExt = ".xlsx";
                    svDialog.Title = "Export to Excel";
                    svDialog.FileName = "SaleDelivery_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";
                    svDialog.Filter = "Excel File (*.xlsx)|*.xlsx ";

                    if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                    {
                        StrFilePath = svDialog.FileName;
                    }
                }

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color FontColor = Color.Red;
                string FontName = "Calibri";
                float FontSize = 11;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;
                this.Cursor = Cursors.WaitCursor;
                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Sale Delivery");

                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    #region Stock Detail

                    EndRow = StartRow + DTabDetail.Rows.Count;

                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].LoadFromDataTable(DTabDetail, true);
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Name = FontName;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Size = FontSize;

                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Font.Bold = true;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(Color.Transparent);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(Color.Black);

                    int CaratColumn = DTabDetail.Columns["CTS"].Ordinal + 1;
                    int ShapeColumn = DTabDetail.Columns["ID"].Ordinal + 1;

                    int MemoAmtColumn = DTabDetail.Columns["TOT"].Ordinal + 1;
                    int LiveAmtColumn = DTabDetail.Columns["FTOT"].Ordinal + 1;

                    int MemoPerCtsColumn = DTabDetail.Columns["DPC"].Ordinal + 1;
                    int LivePerCtsColumn = DTabDetail.Columns["FDPC"].Ordinal + 1;

                    int MemoDiscColumn = DTabDetail.Columns["BACK"].Ordinal + 1;
                    int LiveDiscColumn = DTabDetail.Columns["FBACK"].Ordinal + 1;

                    int RAPColumn = DTabDetail.Columns["RAP"].Ordinal + 1;
                    int DateColumn = DTabDetail.Columns["REP_DATE"].Ordinal + 1;

                    EndRow = EndRow + 2;
                    worksheet.Cells[EndRow, 1, EndRow, 1].Value = "TOTAL";



                    string CaratCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["CTS"].Ordinal + 1);
                    string MemoAmtCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["TOT"].Ordinal + 1);
                    string LiveAmtCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["FTOT"].Ordinal + 1);
                    string RAPCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["RAP"].Ordinal + 1);

                    int IntTotRow = DTabDetail.Rows.Count + 1;

                    StartRow = StartRow + 1;

                    worksheet.Cells[EndRow, ShapeColumn, EndRow, ShapeColumn].Formula = "SUBTOTAL(2," + CaratCol + StartRow + ":" + CaratCol + IntTotRow + ")";
                    worksheet.Cells[EndRow, CaratColumn, EndRow, CaratColumn].Formula = "SUBTOTAL(9," + CaratCol + StartRow + ":" + CaratCol + IntTotRow + ")";
                    worksheet.Cells[EndRow, MemoAmtColumn, EndRow, MemoAmtColumn].Formula = "SUBTOTAL(9," + MemoAmtCol + StartRow + ":" + MemoAmtCol + IntTotRow + ")";
                    worksheet.Cells[EndRow, LiveAmtColumn, EndRow, LiveAmtColumn].Formula = "SUBTOTAL(9," + LiveAmtCol + StartRow + ":" + LiveAmtCol + IntTotRow + ")";
                    worksheet.Cells[EndRow, MemoPerCtsColumn, EndRow, MemoPerCtsColumn].Formula = "ROUND(" + MemoAmtCol + EndRow + "/" + CaratCol + EndRow + ",0)";
                    worksheet.Cells[EndRow, LivePerCtsColumn, EndRow, LivePerCtsColumn].Formula = "ROUND(" + LiveAmtCol + EndRow + "/" + CaratCol + EndRow + ",0)";

                    worksheet.Cells[EndRow, MemoDiscColumn, EndRow, MemoDiscColumn].Formula =
                    "=100 - (" + MemoAmtCol + EndRow + "/SUMPRODUCT(" + RAPCol + StartRow + ":" + RAPCol + IntTotRow + ", " + CaratCol + StartRow + ":" + CaratCol + IntTotRow + ")) * 100";

                    worksheet.Cells[EndRow, LiveDiscColumn, EndRow, LiveDiscColumn].Formula =
                   "=100 - (" + LiveAmtCol + EndRow + "/SUMPRODUCT(" + RAPCol + StartRow + ":" + RAPCol + IntTotRow + ", " + CaratCol + StartRow + ":" + CaratCol + IntTotRow + ")) * 100";

                    //worksheet.Cells[EndRow, MemoDiscColumn, EndRow, MemoDiscColumn].Formula ="100 - (" + MemoAmtCol + EndRow + "/ (SUBTOTAL(9, " + RAPColumn + StartRow + ":" + CaratCol + IntTotRow + ")) * 100)";
                    // worksheet.Cells[EndRow, LiveDiscColumn, EndRow, LiveDiscColumn].Formula = "100 - (" + LiveAmtCol + EndRow + "/ (SUBTOTAL(9, " + RAPColumn + StartRow + ":" + CaratCol + IntTotRow + ")) * 100)";
                    worksheet.Cells[EndRow + 1, 12, EndRow + 1, 12].Value = "LESS";
                    worksheet.Cells[EndRow + 1, 13, EndRow + 1, 13].Value = Val.ToString(GrdDetail.GetFocusedRowCellValue("LESS"));
                    worksheet.Cells[EndRow + 1, 14, EndRow + 1, 14].Formula = "ROUND(" + LiveAmtCol + EndRow + "-" + LiveAmtCol + EndRow + "*" + GrdDetail.GetFocusedRowCellValue("LESS") + "/100 ,2)";

                    int EndRow1 = EndRow + 1;
                    worksheet.Cells[EndRow + 2, 12, EndRow + 2, 12].Value = "RATE";
                    worksheet.Cells[EndRow + 2, 13, EndRow + 2, 13].Value = Val.ToString(GrdDetail.GetFocusedRowCellValue("INVEXCRATE"));
                    worksheet.Cells[EndRow + 2, 14, EndRow + 2, 14].Formula = "ROUND(" + LiveAmtCol + EndRow1 + "*" + Val.ToDouble(GrdDetail.GetFocusedRowCellValue("INVEXCRATE")) + ",2)";

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Font.Bold = true;

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;


                    #endregion

                    worksheet.Column(17).Hidden = true; //Cmnt : Coz due to AutoFitcolumns column can not hide
                    worksheet.Column(24).Hidden = true;

                    xlPackage.Save();
                }
                this.Cursor = Cursors.Default;
                return StrFilePath;
            }

            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
            return "";
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            //try
            //{
            //    if (GrdDetail.FocusedRowHandle >= 0)
            //    {
            //        DataRow Drow = GrdDetail.GetDataRow(GrdDetail.FocusedRowHandle);
            //        Int64 pIntLabReoortNo = Val.ToInt64(Drow["LABREPORTNO"]);
            //        Clipboard.SetText(pIntLabReoortNo.ToString());
            //        Process.Start("notepad.exe");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Global.Message(ex.Message);
            //}

          
        }

        private void deleteSelectedAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DTabMessage = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);

                if (DTabMessage.Rows.Count <= 0)
                {
                    Global.Message("Please Select Records to Create Message");
                    return;
                }
                string StrFilePath = Application.StartupPath + "\\ClinetMessage.txt";
                if (System.IO.File.Exists(StrFilePath) == true)
                {
                    System.IO.File.Delete(StrFilePath);
                }

                System.IO.File.Create(StrFilePath).Dispose();
                using (StreamWriter sw = File.CreateText(StrFilePath))
                {
                    sw.WriteLine("REPORT NUMBER");
                   
                    for (int i = 0; i < DTabMessage.Rows.Count; i++)
                    {
                        sw.WriteLine(DTabMessage.Rows[i]["LABREPORTNO"]);
                    }
                    sw.WriteLine("Total $ : " + Val.Val(DTabMessage.Compute("Sum(MEMOAMOUNT)", "")) + "");
                    sw.WriteLine(" ");
                    sw.WriteLine("PLEASE COLLECT YOUR STONE FROM THIS ADDRESS,");
                    sw.WriteLine(" ");
                    sw.WriteLine(DTabMessage.Rows[0]["SHIPPINGADDRESS"]);
                }
                System.Diagnostics.Process.Start(StrFilePath, "CMD");
                if (ObjGridSelection != null)
                {
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
            //try
            //{
            //    if (GrdDetail.FocusedRowHandle >= 0)
            //    {


            //        string fileLoc = Application.StartupPath + "\\ClinetMessage.txt";
            //        if (System.IO.File.Exists(fileLoc) == true)
            //        {
            //            System.IO.File.Delete(fileLoc);
            //        }

            //        System.IO.File.Create(fileLoc).Dispose();

            //        DataRow Drow = GrdDetail.GetDataRow(GrdDetail.FocusedRowHandle);
            //        Int64 pIntLabReoortNo = Val.ToInt64(Drow["LABREPORTNO"]);
            //        using (StreamWriter writer = new StreamWriter(filePath, true)) // true for appending
            //        {
            //            foreach (dtab)
            //            {
            //                writer.WriteLine(line);
            //            }
            //        }
            //        writer.Dispose();
            //        writer = null;
            //        Process.Start(fileLoc);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Global.Message(ex.Message);
            //}
        }

        private void GrdDetail_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            try
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    DouCarat = 0;
                    DouCostRapaport = 0;
                    DouCostRapaportAmt = 0;
                    DouCostDisc = 0;
                    DouCostPricePerCarat = 0;
                    DouCostAmount = 0;

                    DouSaleRapaport = 0;
                    DouSaleRapaportAmt = 0;
                    DouSaleDisc = 0;
                    DouSalePricePerCarat = 0;
                    DouSaleAmount = 0;

                    DouMemoRapaport = 0;
                    DouMemoRapaportAmt = 0;
                    DouMemoDisc = 0;
                    DouMemoPricePerCarat = 0;
                    DouMemoAmount = 0;


                }
                else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DouCarat = DouCarat + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT"));
                    DouCostAmount = DouCostAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "COSTAMOUNT"));
                    DouCostRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "COSTRAPAPORT"));
                    DouCostPricePerCarat = DouCostAmount / DouCarat;
                    DouCostRapaportAmt = DouCostRapaportAmt + (DouCostRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

                    DouSaleAmount = DouSaleAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "SALEAMOUNT"));
                    DouSaleRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "SALERAPAPORT"));
                    DouSalePricePerCarat = DouSaleAmount / DouCarat;
                    DouSaleRapaportAmt = DouSaleRapaportAmt + (DouSaleRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));


                    DouMemoAmount = DouMemoAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "MEMOAMOUNT"));
                    DouMemoRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "MEMORAPAPORT"));
                    DouMemoPricePerCarat = DouMemoAmount / DouCarat;
                    DouMemoRapaportAmt = DouMemoRapaportAmt + (DouMemoRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

                }
                else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("COSTPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouCostAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("COSTRAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouCostRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("COSTDISCOUNT") == 0)
                    {
                        DouCostRapaport = Math.Round((DouCostRapaportAmt / DouCarat), 2);
                        DouCostDisc = Math.Round(((DouCostRapaport - DouCostPricePerCarat) / DouCostRapaport * 100), 2);
                        e.TotalValue = DouCostDisc;
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
                        //DouSaleDisc = Math.Round(((DouSalePricePerCarat - DouSaleRapaport) / DouSaleRapaport * 100), 2);
                        DouSaleDisc = Math.Round(((DouSaleRapaport - DouSalePricePerCarat) / DouSaleRapaport * 100), 2);
                        e.TotalValue = DouSaleDisc;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MEMOPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouMemoAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MEMORAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouMemoRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MEMODISCOUNT") == 0)
                    {
                        DouMemoRapaport = Math.Round(DouMemoRapaportAmt / DouCarat);
                        //DouSaleDisc = Math.Round(((DouSalePricePerCarat - DouSaleRapaport) / DouSaleRapaport * 100), 2);
                        DouMemoDisc = Math.Round(((DouMemoRapaport - DouMemoPricePerCarat) / DouMemoRapaport * 100), 2);
                        e.TotalValue = DouMemoDisc;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnPrintPDF_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);

                if (DtInvDetail.Rows.Count <= 0)
                {
                    Global.Message("Please Select Records to Export");
                    return;
                }
                var list = DtInvDetail.AsEnumerable().Select(r => r["MEMODETAIL_ID"].ToString());
                string MemoDetailIds = string.Join(",", list);
                
                DataTable DtabNew = new BOTRN_StockUpload().GetDataForDeliveryPrint(MemoDetailIds);

                if (DtabNew.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("NO DATA FOUND FOR PDF EXPORT");
                }

                string StrFilePath = "";
                if (StrFilePath == "")
                {
                    SaveFileDialog svDialog = new SaveFileDialog();
                    svDialog.DefaultExt = ".pdf";
                    svDialog.Title = "Export to pdf";
                    svDialog.FileName = "Estimate-Axn.pdf";
                    svDialog.Filter = "PDF Files (*.pdf)|*.pdf";


                    if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                    {
                        StrFilePath = svDialog.FileName;
                    }
                }

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                string StrImagePath = Application.StartupPath + "\\StoneImagePath.txt ";
                string[] lines = File.ReadAllLines(StrImagePath);

                for (int i = 0; i < DtabNew.Rows.Count; i++)
                {
                    DtabNew.Rows[i]["ImnageURL"] = lines[0] + DtabNew.Rows[i]["PARTYSTOCKNO"] + "\\still.jpg";                   
                }
                for (int j = 0; j < DtabNew.Rows.Count; j++)
                {
                    string imageUrl = Val.ToString(DtabNew.Rows[j]["ImnageURL"]);
                    if (imageUrl != "")
                    {
                        using (WebClient webClient = new WebClient())
                        {
                            byte[] imageBytes = DownloadRemoteImageFile(imageUrl);

                            DtabNew.Rows[j]["Image"] = imageBytes;
                        }
                    }
                }

                Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                FrmReportViewer.MdiParent = Global.gMainRef;
                FrmReportViewer.ExportPDF("RPT_DeliverystonePDFPrint", DtabNew, StrFilePath);
                if (StrFilePath != "")
                {
                    if (Global.Confirm("Do You Want To Open File ? ") == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(StrFilePath, "CMD");
                    }
                }
                if (ObjGridSelection != null)
                {
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.ToString());
                this.Cursor = Cursors.Default;
            }
        }
        private static byte[] DownloadRemoteImageFile(string uri)
        {
            byte[] content;

            // Parse the URI to check the scheme
            var uriObj = new Uri(uri);

            if (uriObj.Scheme == Uri.UriSchemeHttp || uriObj.Scheme == Uri.UriSchemeHttps)
            {
                // Handle HTTP/HTTPS URIs
                var request = (HttpWebRequest)WebRequest.Create(uri);

                using (var response = request.GetResponse())
                using (var reader = new BinaryReader(response.GetResponseStream()))
                {
                    content = reader.ReadBytes(100000); // Adjust buffer size if needed
                }
            }
            else if (uriObj.Scheme == Uri.UriSchemeFile)
            {
                // Handle file:// URIs
                if (File.Exists(uriObj.LocalPath))
                {
                    content = File.ReadAllBytes(uriObj.LocalPath);
                }
                else
                {
                    content = null;
                }
            }
            else
            {
                throw new NotSupportedException($"URI scheme '{uriObj.Scheme}' is not supported.");
            }

            return content;
        }

    }
}
