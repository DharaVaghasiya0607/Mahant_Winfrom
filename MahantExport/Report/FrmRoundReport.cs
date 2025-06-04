using BusLib;
using BusLib.Configuration;
using BusLib.Attendance;
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
using BusLib.Master;
using BusLib.ReportGrid;
using DevExpress.XtraGrid.Views.BandedGrid;
using System.Reflection;
using DevExpress.Data;
using DevExpress.XtraPrintingLinks;
using System.Drawing.Printing;
using BusLib.Rapaport;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using BusLib.View;
using OfficeOpenXml;
using MahantExport;

namespace MahantExport.Report
{
    public partial class FrmRoundReport : DevControlLib.cDevXtraForm
    {
        BOFindRap ObjRap = new BOFindRap();
        public delegate void SetControlValueCallback(Control oControl, string propName, object propValue);

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_RunninPossition ObjView = new BOTRN_RunninPossition();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        BOFormPer ObjPer = new BOFormPer();
        string mStrStockNo = "";
        string StrKapan = "";

        #region Property Settings

        public FrmRoundReport()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            if (ObjPer.ISVIEW == false)
            {
                Global.MessageError(BusLib.TPV.BOMessage.ViewDeniedMsg);
                return;
            }

            this.Show();

            DataTable DTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.TRN_KAPAN);

            CmbKapan.Properties.DataSource = DTab;
            CmbKapan.Properties.DisplayMember = "KAPANNAME";
            CmbKapan.Properties.ValueMember = "KAPANNAME";
            CmbKapan.Focus();
        }

        public void AttachFormDefaultEvent()
        {
           ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjView);
            ObjFormEvent.ObjToDisposeList.Add(Val);

        }

        #endregion               

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnRoundReportExcelExport_Click(object sender, EventArgs e)
        {            
            try
            {
                mStrStockNo = Val.Trim(txtStoneMFGComparision.Text);
                StrKapan = Val.Trim(CmbKapan.Properties.GetCheckedItems());

                string StrFilePath = "";

                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = ".xlsx";
                svDialog.Title = "Export to Excel";
                svDialog.FileName = BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";
                svDialog.Filter = "Excel File (*.xlsx)|*.xlsx ";
                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    StrFilePath = svDialog.FileName;
                }
                StrFilePath = RoundReportExportExcel(StrFilePath);
                string StrFileName = "MFG_Report";
                if (Global.Confirm("Do You Want To Open File ? ") == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(StrFilePath, "CMD");
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        public string RoundReportExportExcel(string StrFilePath)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataSet DS = ObjStock.GetDataForRoundReport(mStrStockNo , StrKapan);
                this.Cursor = Cursors.Default;

                DataTable DTabMain = DS.Tables[0];
                DataTable DTabStock = DS.Tables[1];
                DataTable DTabBook = DS.Tables[2];
                if (DTabMain.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }
                DTabMain.DefaultView.Sort = "Type";
                DTabMain = DTabMain.DefaultView.ToTable();

                DTabStock.DefaultView.Sort = "SrNo";
                DTabStock = DTabStock.DefaultView.ToTable();

                DTabBook.DefaultView.Sort = "SrNo";
                DTabBook = DTabBook.DefaultView.ToTable();

                this.Cursor = Cursors.WaitCursor;

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.White;
                Color FontColor = Color.Black;
                string FontName = "Calibri";
                float FontSize = 11;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                int StartRowStock = 0;
                int StartColumnStock = 0;
                int EndRowStock = 0;
                int EndColumnStock = 0;

                int StartRowBook = 0;
                int StartColumnBook = 0;
                int EndRowBook = 0;
                int EndColumnBook = 0;

                int ROWSTART = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("MFGReport");

                    #region Main

                    StartRow = 1;
                    StartColumn = 2;
                    EndRow = StartRow;
                    EndColumn = DTabMain.Columns.Count + 1;

                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].LoadFromDataTable(DTabMain, true);
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Name = FontName;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Size = FontSize;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Font.Bold = true;
                    worksheet.Cells[StartRow, 2, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, 2, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 2, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 2, StartRow, EndColumn].Style.Font.Color.SetColor(FontColor);

                    for (int i = 2; i <= DTabMain.Columns.Count; i++)
                    {
                        string StrHeader = ExportExcelHeaderMain(Val.ToString(worksheet.Cells[StartRow, i].Value), worksheet, i);
                        worksheet.Cells[StartRow, i].Value = StrHeader;
                        worksheet.Column(i).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }
                    for (int i = 1; i <= DTabMain.Rows.Count; i++)
                    {
                        StartRow += 1;
                        worksheet.Cells[StartRow, 2, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, 2, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, 2, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, 2, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    }


                    ROWSTART += DTabMain.Rows.Count;

                    #endregion

                    #region Book Detail
                    ROWSTART += 4;
                    worksheet.InsertRow(ROWSTART, 1);
                    ROWSTART += 1;
                    worksheet.Cells[ROWSTART, 2].Value = "BOOK";
                    worksheet.Cells[ROWSTART, 2].Style.Font.Size = 16;
                    worksheet.Cells[ROWSTART, 2].Style.Font.Bold = true;

                    ROWSTART += 1;
                    StartRowBook = ROWSTART;
                    StartColumnBook = 1;
                    EndRowBook = StartRowBook + DTabBook.Rows.Count;
                    EndColumnBook = DTabBook.Columns.Count;

                    worksheet.Cells[StartRowBook, StartColumnBook, EndRowBook, EndColumnBook].LoadFromDataTable(DTabBook, true);
                    worksheet.Cells[StartRowBook, StartColumnBook, EndRowBook, EndColumnBook].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[StartRowBook, StartColumnBook, EndRowBook, EndColumnBook].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[StartRowBook, StartColumnBook, EndRowBook, EndColumnBook].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRowBook, StartColumnBook, EndRowBook, EndColumnBook].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRowBook, StartColumnBook, EndRowBook, EndColumnBook].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRowBook, StartColumnBook, EndRowBook, EndColumnBook].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRowBook, StartColumnBook, EndRowBook, EndColumnBook].Style.Font.Name = FontName;
                    worksheet.Cells[StartRowBook, StartColumnBook, EndRowBook, EndColumnBook].Style.Font.Size = FontSize;
                    worksheet.Cells[StartRowBook, StartColumnBook, StartRowBook, EndColumnBook].Style.Font.Bold = true;
                    worksheet.Cells[StartRowBook, 1, StartRowBook, EndColumnBook].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRowBook, 1, StartRowBook, EndColumnBook].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRowBook, 1, StartRowBook, EndColumnBook].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRowBook, 1, StartRowBook, EndColumnBook].Style.Font.Color.SetColor(FontColor);
                    worksheet.Cells[ROWSTART, StartColumnBook, StartRowBook, EndColumnBook].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                    for (int i = 1; i <= DTabBook.Columns.Count; i++)
                    {
                        string StrHeader = ExportExcelHeaderBook(Val.ToString(worksheet.Cells[StartRowBook, i].Value), worksheet, i);
                        worksheet.Cells[StartRowBook, i].Value = StrHeader;
                        worksheet.Column(i).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    }

                    int CaratColumnBook = DTabBook.Columns["Carat"].Ordinal + 1;
                    int RxWColumnBook = DTabBook.Columns["RXW"].Ordinal + 1;
                    int RapaportColumnBook = DTabBook.Columns["Iive Rap. Price"].Ordinal + 1;
                    int DiscountColumnBook = DTabBook.Columns["Iive Disc %"].Ordinal + 1;
                    int PricePerCaratColumnBook = DTabBook.Columns["Iive Net Rate"].Ordinal + 1;
                    int AmountColumnBook = DTabBook.Columns["Iive Net Value"].Ordinal + 1;
                    int StoneIDBook = DTabBook.Columns["Stone id"].Ordinal + 1;

                    StartRowBook += 1;
                    for (int i = StartRowBook; i <= EndRowBook; i++)
                    {
                        string RapColumnsBook = Global.ColumnIndexToColumnLetter(RapaportColumnBook) + i.ToString();
                        string DiscountBook = Global.ColumnIndexToColumnLetter(DiscountColumnBook) + i.ToString();
                        string CaratClmBook = Global.ColumnIndexToColumnLetter(CaratColumnBook) + i.ToString();
                        string PricePerCaratBook = Global.ColumnIndexToColumnLetter(PricePerCaratColumnBook) + i.ToString();

                        worksheet.Cells[i, RxWColumnBook].Formula = "=ROUND(" + RapColumnsBook + " * " + CaratClmBook + ",2)";
                        if (Val.ToString(DTabBook.Rows[i - StartRowBook]["FancyColor"]) == "FANCY")
                        {
                            worksheet.Cells[i, PricePerCaratColumnBook].Value = Val.ToDouble(DTabBook.Rows[i - StartRowBook]["Iive Net Rate"]);
                            worksheet.Cells[i, AmountColumnBook].Value = Math.Round(Val.ToDouble(DTabBook.Rows[i - StartRowBook]["Iive Net Value"]), 2);
                        }
                        else
                        {
                            worksheet.Cells[i, PricePerCaratColumnBook].Formula = "=(" + RapColumnsBook + " + ((" + RapColumnsBook + " * " + DiscountBook + ") / 100))";
                            worksheet.Cells[i, AmountColumnBook].Formula = "=ROUND(" + PricePerCaratBook + " * " + CaratClmBook + ",2)";
                        }

                        if (Val.ToString(DTabBook.Rows[i - StartRowBook]["Iive Rap. Price"]) == "")
                        {
                            worksheet.Cells[i, RapaportColumnBook].Value = "0";
                        }
                    }
                    EndRowBook = EndRowBook + 2;
                    worksheet.Cells[EndRowBook, 1, EndRowBook, 1].Value = "TOTAL";

                    string StoneIDColBook = Global.ColumnIndexToColumnLetter(DTabBook.Columns["Stone id"].Ordinal + 1);
                    string CaratColBook = Global.ColumnIndexToColumnLetter(DTabBook.Columns["Carat"].Ordinal + 1);
                    string RxWBook = Global.ColumnIndexToColumnLetter(DTabBook.Columns["RXW"].Ordinal + 1);
                    string Discount1Book = Global.ColumnIndexToColumnLetter(DTabBook.Columns["Iive Disc %"].Ordinal + 1);
                    string NetRateBook = Global.ColumnIndexToColumnLetter(DTabBook.Columns["Iive Net Rate"].Ordinal + 1);
                    string NetValueBook = Global.ColumnIndexToColumnLetter(DTabBook.Columns["Iive Net Value"].Ordinal + 1);

                    int IntTotRowBook = DTabBook.Rows.Count + StartRowBook - 1;

                    worksheet.Cells[EndRowBook, StoneIDBook, EndRowBook, StoneIDBook].Formula = "ROWS(" + StoneIDColBook + StartRowBook + ":" + StoneIDColBook + IntTotRowBook + ")";
                    worksheet.Cells[EndRowBook, CaratColumnBook, EndRowBook, CaratColumnBook].Formula = "ROUND(SUBTOTAL(9," + CaratColBook + StartRowBook + ":" + CaratColBook + IntTotRowBook + "),2)";
                    worksheet.Cells[EndRowBook, RxWColumnBook, EndRowBook, RxWColumnBook].Formula = "SUBTOTAL(9," + RxWBook + StartRowBook + ":" + RxWBook + IntTotRowBook + ")";
                    worksheet.Cells[EndRowBook, AmountColumnBook, EndRowBook, AmountColumnBook].Formula = "ROUND(SUBTOTAL(9," + NetValueBook + StartRowBook + ":" + NetValueBook + IntTotRowBook + "),2)";
                    worksheet.Cells[EndRowBook, PricePerCaratColumnBook, EndRowBook, PricePerCaratColumnBook].Formula = "ROUND(" + NetValueBook + EndRowBook + "/" + CaratColBook + EndRowBook + ",0)";
                    worksheet.Cells[EndRowBook, DiscountColumnBook, EndRowBook, DiscountColumnBook].Formula = "ROUND((" + NetValueBook + EndRowBook + "/" + RxWBook + EndRowBook + "-1 ) * 100,2)";

                    worksheet.Cells[EndRowBook, StartColumnBook, EndRowBook, EndColumnBook].Style.Font.Bold = true;

                    worksheet.Cells[StartRowBook, CaratColumnBook, EndRowBook, CaratColumnBook].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRowBook, DiscountColumnBook, EndRowBook, DiscountColumnBook].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRowBook, AmountColumnBook, EndRowBook, AmountColumnBook].Style.Numberformat.Format = "0.00";

                    worksheet.Cells[EndRowBook, StartColumnBook, EndRowBook, EndColumnBook].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[EndRowBook, StartColumnBook, EndRowBook, EndColumnBook].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[EndRowBook, StartColumnBook, EndRowBook, EndColumnBook].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    ROWSTART += DTabBook.Rows.Count;

                    #endregion

                    #region Stock Detail
                    ROWSTART += 3;
                    worksheet.InsertRow(ROWSTART, 1);
                    ROWSTART += 1;
                    worksheet.Cells[ROWSTART, 2].Value = "STOCK";
                    worksheet.Cells[ROWSTART, 2].Style.Font.Size = 16;
                    worksheet.Cells[ROWSTART, 2].Style.Font.Bold = true;

                    ROWSTART += 1;
                    StartRowStock = ROWSTART;
                    StartColumnStock = 1;
                    EndRowStock = StartRowStock + DTabStock.Rows.Count;
                    EndColumnStock = DTabStock.Columns.Count;

                    // STOCK
                    worksheet.Cells[StartRowStock, StartColumnStock, EndRowStock, EndColumnStock].LoadFromDataTable(DTabStock, true);
                    worksheet.Cells[StartRowStock, StartColumnStock, EndRowStock, EndColumnStock].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[StartRowStock, StartColumnStock, EndRowStock, EndColumnStock].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[StartRowStock, StartColumnStock, EndRowStock, EndColumnStock].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRowStock, StartColumnStock, EndRowStock, EndColumnStock].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRowStock, StartColumnStock, EndRowStock, EndColumnStock].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRowStock, StartColumnStock, EndRowStock, EndColumnStock].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRowStock, StartColumnStock, EndRowStock, EndColumnStock].Style.Font.Name = FontName;
                    worksheet.Cells[StartRowStock, StartColumnStock, EndRowStock, EndColumnStock].Style.Font.Size = FontSize;
                    worksheet.Cells[StartRowStock, StartColumnStock, StartRowStock, EndColumnStock].Style.Font.Bold = true;
                    worksheet.Cells[StartRowStock, 1, StartRowStock, EndColumnStock].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRowStock, 1, StartRowStock, EndColumnStock].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRowStock, 1, StartRowStock, EndColumnStock].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRowStock, 1, StartRowStock, EndColumnStock].Style.Font.Color.SetColor(FontColor);
                    worksheet.Cells[ROWSTART, StartColumnStock, StartRowStock, EndColumnStock].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                    for (int i = 1; i <= DTabStock.Columns.Count; i++)
                    {
                        string StrHeader = ExportExcelHeaderStock(Val.ToString(worksheet.Cells[StartRowStock, i].Value), worksheet, i);
                        worksheet.Cells[StartRowStock, i].Value = StrHeader;
                        worksheet.Column(i).Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                    }

                    int CaratColumn = DTabStock.Columns["Carat"].Ordinal + 1;
                    int RxWColumn = DTabStock.Columns["RXW"].Ordinal + 1;
                    int RapaportColumn = DTabStock.Columns["Iive Rap. Price"].Ordinal + 1;
                    int DiscountColumn = DTabStock.Columns["Iive Disc %"].Ordinal + 1;
                    int PricePerCaratColumn = DTabStock.Columns["Iive Net Rate"].Ordinal + 1;
                    int AmountColumn = DTabStock.Columns["Iive Net Value"].Ordinal + 1;
                    int StoneID = DTabStock.Columns["Stone id"].Ordinal + 1;

                    StartRowStock += 1;
                    for (int i = StartRowStock; i <= EndRowStock; i++)
                    {
                        string RapColumns = Global.ColumnIndexToColumnLetter(RapaportColumn) + i.ToString();
                        string Discount = Global.ColumnIndexToColumnLetter(DiscountColumn) + i.ToString();
                        string CaratClm = Global.ColumnIndexToColumnLetter(CaratColumn) + i.ToString();
                        string PricePerCarat = Global.ColumnIndexToColumnLetter(PricePerCaratColumn) + i.ToString();

                        worksheet.Cells[i, RxWColumn].Formula = "=ROUND(" + RapColumns + " * " + CaratClm + ",2)";
                        if (Val.ToString(DTabStock.Rows[i - StartRowStock]["FancyColor"]) == "FANCY")
                        {
                            worksheet.Cells[i, PricePerCaratColumn].Value = Val.ToDouble(DTabStock.Rows[i - StartRowStock]["Iive Net Rate"]);
                            worksheet.Cells[i, AmountColumn].Value = Math.Round(Val.ToDouble(DTabStock.Rows[i - StartRowStock]["Iive Net Value"]), 2);
                        }
                        else
                        {
                            worksheet.Cells[i, PricePerCaratColumn].Formula = "=(" + RapColumns + " + ((" + RapColumns + " * " + Discount + ") / 100))";
                            worksheet.Cells[i, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + CaratClm + ",2)";
                        }

                        if (Val.ToString(DTabStock.Rows[i - StartRowStock]["Iive Rap. Price"]) == "")
                        {
                            worksheet.Cells[i, RapaportColumn].Value = "0";
                        }
                    }
                    EndRowStock = EndRowStock + 2;
                    worksheet.Cells[EndRowStock, 1, EndRowStock, 1].Value = "TOTAL";

                    string StoneIDCol = Global.ColumnIndexToColumnLetter(DTabStock.Columns["Stone id"].Ordinal + 1);
                    string CaratCol = Global.ColumnIndexToColumnLetter(DTabStock.Columns["Carat"].Ordinal + 1);
                    string RxW = Global.ColumnIndexToColumnLetter(DTabStock.Columns["RXW"].Ordinal + 1);
                    string Discount1 = Global.ColumnIndexToColumnLetter(DTabStock.Columns["Iive Disc %"].Ordinal + 1);
                    string NetRate = Global.ColumnIndexToColumnLetter(DTabStock.Columns["Iive Net Rate"].Ordinal + 1);
                    string NetValue = Global.ColumnIndexToColumnLetter(DTabStock.Columns["Iive Net Value"].Ordinal + 1);

                    int IntTotRow = DTabStock.Rows.Count + StartRowStock - 1;

                    worksheet.Cells[EndRowStock, StoneID, EndRowStock, StoneID].Formula = "ROWS(" + StoneIDCol + StartRowStock + ":" + StoneIDCol + IntTotRow + ")";
                    worksheet.Cells[EndRowStock, CaratColumn, EndRowStock, CaratColumn].Formula = "ROUND(SUBTOTAL(9," + CaratCol + StartRowStock + ":" + CaratCol + IntTotRow + "),2)";
                    worksheet.Cells[EndRowStock, RxWColumn, EndRowStock, RxWColumn].Formula = "SUBTOTAL(9," + RxW + StartRowStock + ":" + RxW + IntTotRow + ")";
                    worksheet.Cells[EndRowStock, AmountColumn, EndRowStock, AmountColumn].Formula = "ROUND(SUBTOTAL(9," + NetValue + StartRowStock + ":" + NetValue + IntTotRow + "),2)";
                    worksheet.Cells[EndRowStock, PricePerCaratColumn, EndRowStock, PricePerCaratColumn].Formula = "ROUND(" + NetValue + EndRowStock + "/" + CaratCol + EndRowStock + ",0)";
                    worksheet.Cells[EndRowStock, DiscountColumn, EndRowStock, DiscountColumn].Formula = "ROUND((" + NetValue + EndRowStock + "/" + RxW + EndRowStock + "-1 ) * 100,2)";

                    worksheet.Cells[EndRowStock, StartColumnStock, EndRowStock, EndColumnStock].Style.Font.Bold = true;

                    worksheet.Cells[StartRowStock, CaratColumn, EndRowStock, CaratColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRowStock, DiscountColumn, EndRowStock, DiscountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRowStock, AmountColumn, EndRowStock, AmountColumn].Style.Numberformat.Format = "0.00";

                    worksheet.Cells[EndRowStock, StartColumnStock, EndRowStock, EndColumnStock].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[EndRowStock, StartColumnStock, EndRowStock, EndColumnStock].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[EndRowStock, StartColumnStock, EndRowStock, EndColumnStock].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    ROWSTART += DTabStock.Rows.Count;
                    #endregion

                    //MAIN CALCULATION
                    worksheet.Cells[2, 3].Formula = StoneIDColBook + EndRowBook;
                    worksheet.Cells[3, 3].Formula = StoneIDCol + EndRowStock;
                    worksheet.Cells[4, 3].Formula = "ROUND(SUBTOTAL(9,C2:C3),0)";
                    worksheet.Cells[2, 4].Formula = CaratColBook + EndRowBook;
                    worksheet.Cells[3, 4].Formula = CaratCol + EndRowStock;
                    worksheet.Cells[4, 4].Formula = "ROUND(SUBTOTAL(9,D2:D3),2)";

                    worksheet.Cells[2, 5].Formula = NetValueBook + EndRowBook;
                    worksheet.Cells[3, 5].Formula = NetValue + EndRowStock;
                    worksheet.Cells[4, 5].Formula = "ROUND(SUBTOTAL(9,E2:E3),2)";
                    worksheet.Cells[2, 6].Formula = NetRateBook + EndRowBook;
                    worksheet.Cells[3, 6].Formula = NetRate + EndRowStock;
                    worksheet.Cells[4, 6].Formula = "ROUND(E4/D4,0)";

                    worksheet.Cells.AutoFitColumns();

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

        public static string ExportExcelHeaderMain(String pstrHeader, ExcelWorksheet worksheet, int pCol)
        {
            if (pstrHeader.ToLower() == "type")
            {
                worksheet.Column(pCol).Width = 15;
                return "Type";
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
            else if (pstrHeader.ToLower() == "amt")
            {
                worksheet.Column(pCol).Width = 10;
                return "AMT";
            }
            else if (pstrHeader.ToLower() == "avg")
            {
                worksheet.Column(pCol).Width = 10;
                return "AVG";
            }
            return "";
        }

        public static string ExportExcelHeaderStock(String pstrHeader, ExcelWorksheet worksheet, int pCol)
        {
            if (pstrHeader.ToLower() == "tempstock")
            {
                worksheet.Column(pCol).Width = 15;
                return "TempStock";
            }
            else if (pstrHeader.ToLower() == "srno")
            {
                worksheet.Column(pCol).Width = 15;
                return "SrNo";
            }
            else if (pstrHeader.ToLower() == "stone id")
            {
                worksheet.Column(pCol).Width = 10;
                return "Stone id";
            }
            else if (pstrHeader.ToLower() == "kapan")
            {
                worksheet.Column(pCol).Width = 10;
                return "Kapan";
            }
            else if (pstrHeader.ToLower() == "status")
            {
                worksheet.Column(pCol).Width = 10;
                return "Status";
            }
            else if (pstrHeader.ToLower() == "lab")
            {
                worksheet.Column(pCol).Width = 10;
                return "Lab";
            }
            else if (pstrHeader.ToLower() == "report no")
            {
                worksheet.Column(pCol).Width = 10;
                return "Report No";
            }
            else if (pstrHeader.ToLower() == "shape")
            {
                worksheet.Column(pCol).Width = 10;
                return "Shape";
            }
            else if (pstrHeader.ToLower() == "carat")
            {
                worksheet.Column(pCol).Width = 10;
                return "Carat";
            }
            else if (pstrHeader.ToLower() == "color")
            {
                worksheet.Column(pCol).Width = 10;
                return "Color";
            }
            else if (pstrHeader.ToLower() == "clarity")
            {
                worksheet.Column(pCol).Width = 10;
                return "Clarity";
            }
            else if (pstrHeader.ToLower() == "cut")
            {
                worksheet.Column(pCol).Width = 10;
                return "Cut";
            }
            else if (pstrHeader.ToLower() == "polish")
            {
                worksheet.Column(pCol).Width = 10;
                return "Polish";
            }
            else if (pstrHeader.ToLower() == "symm")
            {
                worksheet.Column(pCol).Width = 10;
                return "Symm";
            }
            else if (pstrHeader.ToLower() == "flour")
            {
                worksheet.Column(pCol).Width = 10;
                return "Flour";
            }
            else if (pstrHeader.ToLower() == "measurement")
            {
                worksheet.Column(pCol).Width = 10;
                return "Measurement";
            }
            else if (pstrHeader.ToLower() == "depth%")
            {
                worksheet.Column(pCol).Width = 10;
                return "Depth%";
            }
            else if (pstrHeader.ToLower() == "table%")
            {
                worksheet.Column(pCol).Width = 10;
                return "Table%";
            }
            else if (pstrHeader.ToLower() == "iive rap. price")
            {
                worksheet.Column(pCol).Width = 10;
                return "Live Rap. Price";
            }
            else if (pstrHeader.ToLower() == "rxw")
            {
                worksheet.Column(pCol).Width = 10;
                return "RXW";
            }
            else if (pstrHeader.ToLower() == "iive disc %")
            {
                worksheet.Column(pCol).Width = 10;
                return "Live Disc %";
            }
            else if (pstrHeader.ToLower() == "iive net rate")
            {
                worksheet.Column(pCol).Width = 10;
                return "Live Net Rate";
            }
            else if (pstrHeader.ToLower() == "iive net value")
            {
                worksheet.Column(pCol).Width = 10;
                return "Live Net Value";
            }
            else if (pstrHeader.ToLower() == "comment")
            {
                worksheet.Column(pCol).Width = 10;
                return "Comment";
            }
            else if (pstrHeader.ToLower() == "milky")
            {
                worksheet.Column(pCol).Width = 10;
                return "Milky";
            }
            else if (pstrHeader.ToLower() == "shade")
            {
                worksheet.Column(pCol).Width = 10;
                return "Shade";
            }
            else if (pstrHeader.ToLower() == "fancycolor")
            {
                worksheet.Column(pCol).Width = 10;
                return "Fancy Color";
            }
            return "";
        }

        public static string ExportExcelHeaderBook(String pstrHeader, ExcelWorksheet worksheet, int pCol)
        {
            if (pstrHeader.ToLower() == "#tempbook")
            {
                worksheet.Column(pCol).Width = 15;
                return "TempBook";
            }
            else if (pstrHeader.ToLower() == "srno")
            {
                worksheet.Column(pCol).Width = 15;
                return "SrNo";
            }
            else if (pstrHeader.ToLower() == "stone id")
            {
                worksheet.Column(pCol).Width = 10;
                return "Stone id";
            }
            else if (pstrHeader.ToLower() == "kapan")
            {
                worksheet.Column(pCol).Width = 10;
                return "Kapan";
            }
            else if (pstrHeader.ToLower() == "status")
            {
                worksheet.Column(pCol).Width = 10;
                return "Status";
            }
            else if (pstrHeader.ToLower() == "lab")
            {
                worksheet.Column(pCol).Width = 10;
                return "Lab";
            }
            else if (pstrHeader.ToLower() == "report no")
            {
                worksheet.Column(pCol).Width = 10;
                return "Report No";
            }
            else if (pstrHeader.ToLower() == "shape")
            {
                worksheet.Column(pCol).Width = 10;
                return "Shape";
            }
            else if (pstrHeader.ToLower() == "carat")
            {
                worksheet.Column(pCol).Width = 10;
                return "Carat";
            }
            else if (pstrHeader.ToLower() == "color")
            {
                worksheet.Column(pCol).Width = 10;
                return "Color";
            }
            else if (pstrHeader.ToLower() == "clarity")
            {
                worksheet.Column(pCol).Width = 10;
                return "Clarity";
            }
            else if (pstrHeader.ToLower() == "cut")
            {
                worksheet.Column(pCol).Width = 10;
                return "Cut";
            }
            else if (pstrHeader.ToLower() == "polish")
            {
                worksheet.Column(pCol).Width = 10;
                return "Polish";
            }
            else if (pstrHeader.ToLower() == "symm")
            {
                worksheet.Column(pCol).Width = 10;
                return "Symm";
            }
            else if (pstrHeader.ToLower() == "flour")
            {
                worksheet.Column(pCol).Width = 10;
                return "Flour";
            }
            else if (pstrHeader.ToLower() == "measurement")
            {
                worksheet.Column(pCol).Width = 10;
                return "Measurement";
            }
            else if (pstrHeader.ToLower() == "depth%")
            {
                worksheet.Column(pCol).Width = 10;
                return "Depth%";
            }
            else if (pstrHeader.ToLower() == "table%")
            {
                worksheet.Column(pCol).Width = 10;
                return "Table%";
            }
            else if (pstrHeader.ToLower() == "iive rap. price")
            {
                worksheet.Column(pCol).Width = 10;
                return "Live Rap. Price";
            }
            else if (pstrHeader.ToLower() == "rxw")
            {
                worksheet.Column(pCol).Width = 10;
                return "RXW";
            }
            else if (pstrHeader.ToLower() == "iive disc %")
            {
                worksheet.Column(pCol).Width = 10;
                return "Live Disc %";
            }
            else if (pstrHeader.ToLower() == "iive net rate")
            {
                worksheet.Column(pCol).Width = 10;
                return "Live Net Rate";
            }
            else if (pstrHeader.ToLower() == "iive net value")
            {
                worksheet.Column(pCol).Width = 10;
                return "Live Net Value";
            }
            else if (pstrHeader.ToLower() == "comment")
            {
                worksheet.Column(pCol).Width = 10;
                return "Comment";
            }
            else if (pstrHeader.ToLower() == "milky")
            {
                worksheet.Column(pCol).Width = 10;
                return "Milky";
            }
            else if (pstrHeader.ToLower() == "shade")
            {
                worksheet.Column(pCol).Width = 10;
                return "Shade";
            }
            else if (pstrHeader.ToLower() == "fancycolor")
            {
                worksheet.Column(pCol).Width = 10;
                return "Fancy Color";
            }
            return "";
        }

        private void txtStoneMFGComparision_TextChanged(object sender, EventArgs e)
        {
            String str1 = "";
            if (txtStoneMFGComparision.Text.Trim().Contains("\t\n"))
            {
                str1 = txtStoneMFGComparision.Text.Trim().Replace("\t\n", ",");
            }
            else if (txtStoneMFGComparision.Text.Trim().Contains("\r"))
            {
                str1 = txtStoneMFGComparision.Text.Trim().Replace("\r", "");
            }
            else
            {
                str1 = txtStoneMFGComparision.Text.Trim().Replace("\n", ",");
            }

            txtStoneMFGComparision.Text = str1;
            txtStoneMFGComparision.Select(txtStoneMFGComparision.Text.Length, 0);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtStoneMFGComparision.Text = string.Empty;
            CmbKapan.SetEditValue(-1);
        }

    }
}
