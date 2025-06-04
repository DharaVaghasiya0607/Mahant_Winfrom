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
    public partial class FrmSaleReport : DevControlLib.cDevXtraForm
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
        string StrFilePath = "";

        #region Property Settings

        public FrmSaleReport()
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
                StrFilePath = SaleReportExportExcel(StrFilePath);
                string StrFileName = "Sale_Report";
                
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

        public void AddShortStockDetail(ExcelWorksheet worksheet, DataTable pDtab)
        {
            Color BackColor = Color.FromArgb(2, 68, 143);
            Color FontColor = Color.White;
            string FontName = "Calibri";
            float FontSize = 9;


            worksheet.Cells[2, 3, 4, 13].Value = "Shree Krishna Export Inclusion Grading";
            worksheet.Cells[2, 3, 4, 13].Style.Font.Name = FontName;
            worksheet.Cells[2, 3, 4, 13].Style.Font.Size = 20;
            worksheet.Cells[2, 3, 4, 13].Style.Font.Bold = true;

            worksheet.Cells[2, 3, 4, 13].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            worksheet.Cells[2, 3, 4, 13].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            worksheet.Cells[2, 3, 4, 13].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            worksheet.Cells[2, 3, 4, 13].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            worksheet.Cells[2, 3, 4, 13].Merge = true;
            worksheet.Cells[2, 3, 4, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 3, 4, 13].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;


            worksheet.Cells[2, 3, 4, 13].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[2, 3, 4, 13].Style.Fill.PatternColor.SetColor(BackColor);
            worksheet.Cells[2, 3, 4, 13].Style.Fill.BackgroundColor.SetColor(BackColor);
            worksheet.Cells[2, 3, 4, 13].Style.Font.Color.SetColor(FontColor);

            DataTable DTabDistinct = pDtab.DefaultView.ToTable(true, "PARATYPE_ID", "PARATYPECODE", "PARATYPENAME");
            DTabDistinct.DefaultView.Sort = "PARATYPE_ID";
            DTabDistinct = DTabDistinct.DefaultView.ToTable();

            int StartRow = 0;
            int StartColumn = 3;
            int IntRow = 0;

            int[] array = new int[4];

            for (int i = 0; i < DTabDistinct.Rows.Count; i++)
            {
                string Str = Val.ToString(DTabDistinct.Rows[i]["PARATYPECODE"]);
                string StrName = Val.ToString(DTabDistinct.Rows[i]["PARATYPENAME"]);

                DataTable DTab = pDtab.Select("PARATYPECODE='" + Str + "'").CopyToDataTable();

                if (i % 4 == 0)
                {
                    StartColumn = 3;
                    StartRow = IntRow + (i % 4) + (array.Max() == 0 ? 6 : array.Max()) + 2;
                    IntRow = StartRow;
                    array = new int[4];
                }
                else
                {
                    StartRow = IntRow;
                }
                array[i % 4] = DTab.Rows.Count;

                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Value = StrName;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Merge = true;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Fill.PatternColor.SetColor(BackColor);
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Fill.BackgroundColor.SetColor(BackColor);
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Color.SetColor(FontColor);
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Name = FontName;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Size = 11;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Bold = true;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                StartRow = StartRow + 1;
                for (int J = 0; J < DTab.Rows.Count; J++)
                {
                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Value = Val.ToString(DTab.Rows[J]["CODE"]);
                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Style.Font.Bold = true;
                    worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = Val.ToString(DTab.Rows[J]["NAME"]);
                    StartRow = StartRow + 1;

                }
                worksheet.Column(StartColumn + 1).Width = 20;

                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Name = FontName;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Size = 11;

                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Name = FontName;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Size = FontSize;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                StartColumn = StartColumn + 3;

            }
            worksheet.Cells[1, 1, 50, 50].AutoFitColumns();
        }

        public string SaleReportExportExcel(string StrFilePath)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataSet Ds = ObjStock.GetDataForSaleReport(mStrStockNo, StrKapan, "", "", "", 00, 00, "", "", "", "");
                DataTable DTabDetail = Ds.Tables[0];
                DataTable DTabShortStock = Ds.Tables[1];
                DataTable DTabBhveshStock = Ds.Tables[2];

                this.Cursor = Cursors.Default;

                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }


                this.Cursor = Cursors.WaitCursor;
                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = ".xlsx";
                svDialog.Title = "Export to Excel";
                svDialog.FileName = BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";
                svDialog.Filter = "Excel File (*.xlsx)|*.xlsx ";

                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    StrFilePath = svDialog.FileName;
                }

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                //Color BackColor = Color.Yellow;
                Color FontColor = Color.Red;
                string FontName = "Calibri";
                float FontSize = 9;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Full Stock List");
                    ExcelWorksheet worksheetShortStock = xlPackage.Workbook.Worksheets.Add("For Short Sale List");
                    ExcelWorksheet worksheetBhaveshBhai = xlPackage.Workbook.Worksheets.Add("For Bhaveshbhai");

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
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(FontColor);

                    worksheet.Cells[2, 1, 2, EndColumn].Style.Font.Color.SetColor(Color.Blue);

                    //int CtsWColumn = DTabDetail.Columns["Carat"].Ordinal + 1;
                    //int MFGRapColumn = DTabDetail.Columns["Mfg Rap"].Ordinal + 1;
                    //int PricePerCaratColumn = DTabDetail.Columns["Net Rate"].Ordinal + 1;
                    //int DiscountColumn = DTabDetail.Columns["Disc %"].Ordinal + 1;
                    //int CaratColumn = DTabDetail.Columns["Carats"].Ordinal + 1;
                    //int AmountColumn = DTabDetail.Columns["Net Value"].Ordinal + 1;
                    //int DepthPerColumn = DTabDetail.Columns["Depth%"].Ordinal + 1;
                    //int TablePerColumn = DTabDetail.Columns["Table%"].Ordinal + 1;
                    //int ShapeColumn = DTabDetail.Columns["Shape"].Ordinal + 1;

                    //for (int IntI = 2; IntI <= EndRow; IntI++)
                    //{
                    //    string RapColumns = Global.ColumnIndexToColumnLetter(RapaportColumn) + IntI.ToString();
                    //    string Discount = Global.ColumnIndexToColumnLetter(DiscountColumn) + IntI.ToString();
                    //    string Carat = Global.ColumnIndexToColumnLetter(CaratColumn) + IntI.ToString();
                    //    string PricePerCarat = Global.ColumnIndexToColumnLetter(PricePerCaratColumn) + IntI.ToString();

                    //    worksheet.Cells[IntI, RxWColumn].Formula = "=ROUND(" + RapColumns + " * " + Carat + ",2)";
                    //    //worksheet.Cells[IntI, PricePerCaratColumn].Formula = "=ROUND( (100 +" + Discount + ") * " + RapColumns + "/100,2)";
                    //    worksheet.Cells[IntI, PricePerCaratColumn].Formula = "=ROUND(" + RapColumns + " + (" + " ( " + RapColumns + " * " + Discount + " )/100),2)"; 
                    //    worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                    //}

                    EndRow = EndRow + 2;
                    worksheet.Cells[EndRow, 1, EndRow, 1].Value = "Summary";

                    //string RxW = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["RXW"].Ordinal + 1);
                    //string CaratCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Carats"].Ordinal + 1);
                    //string Discount1 = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Disc %"].Ordinal + 1);
                    //string NetRate = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Net Rate"].Ordinal + 1);
                    //string NetValue = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Net Value"].Ordinal + 1);

                    int IntTotRow = DTabDetail.Rows.Count + 1;

                    StartRow = StartRow + 1;

                    //worksheet.Cells[EndRow, ShapeColumn, EndRow, ShapeColumn].Formula = "SUBTOTAL(2," + CaratCol + StartRow + ":" + CaratCol + IntTotRow + ")";
                    //worksheet.Cells[EndRow, CaratColumn, EndRow, CaratColumn].Formula = "ROUND(SUBTOTAL(9," + CaratCol + StartRow + ":" + CaratCol + IntTotRow + "),2)";
                    //worksheet.Cells[EndRow, RxWColumn, EndRow, RxWColumn].Formula = "SUBTOTAL(9," + RxW + StartRow + ":" + RxW + IntTotRow + ")";
                    //worksheet.Cells[EndRow, AmountColumn, EndRow, AmountColumn].Formula = "ROUND(SUBTOTAL(9," + NetValue + StartRow + ":" + NetValue + IntTotRow + "),2)";
                    //worksheet.Cells[EndRow, PricePerCaratColumn, EndRow, PricePerCaratColumn].Formula = "ROUND(" + NetValue + EndRow + "/" + CaratCol + EndRow + ",0)";
                    //worksheet.Cells[EndRow, DiscountColumn, EndRow, DiscountColumn].Formula = "ROUND((" + NetValue + EndRow + "/" + RxW + EndRow + "-1 ) * 100,2)";

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Font.Bold = true;

                    //worksheet.Cells[StartRow, CaratColumn, EndRow, CaratColumn].Style.Numberformat.Format = "0.00";
                    //worksheet.Cells[StartRow, DiscountColumn, EndRow, DiscountColumn].Style.Numberformat.Format = "0.00";
                    //worksheet.Cells[StartRow, AmountColumn, EndRow, AmountColumn].Style.Numberformat.Format = "0.00";
                    //worksheet.Cells[StartRow, DepthPerColumn, EndRow, DepthPerColumn].Style.Numberformat.Format = "0.00";
                    //worksheet.Cells[StartRow, TablePerColumn, EndRow, TablePerColumn].Style.Numberformat.Format = "0.00";

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    //worksheet.Column(17).Hidden = true; //Cmnt : Coz due to AutoFitcolumns column can not hide
                    worksheet.Cells[1, 1, 100, 100].AutoFitColumns();
                    #endregion

                    #region Short Stock Detail
                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabShortStock.Columns.Count;
                    EndRow = StartRow + DTabShortStock.Rows.Count;

                    worksheetShortStock.Cells[StartRow, StartColumn, EndRow, EndColumn].LoadFromDataTable(DTabShortStock, true);
                    worksheetShortStock.Cells[StartRow, 1, StartRow, EndColumn].Style.WrapText = true;
                    worksheetShortStock.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheetShortStock.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheetShortStock.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheetShortStock.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheetShortStock.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheetShortStock.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheetShortStock.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Name = FontName;
                    worksheetShortStock.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Size = 10;

                    worksheetShortStock.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Font.Bold = true;
                    worksheetShortStock.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetShortStock.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetShortStock.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetShortStock.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    
                    worksheetShortStock.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetShortStock.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheetShortStock.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    worksheetShortStock.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(Color.Black);

                    worksheetShortStock.Cells[1,12,1,15].Style.Font.Bold = true;
                    worksheetShortStock.Cells[1, 12, 1, 15].Style.Font.Color.SetColor(Color.Blue);

                    worksheetShortStock.Cells[1, 16, 1, 18].Style.Font.Color.SetColor(Color.Blue);

                    worksheetShortStock.Cells[1, 19, 1, 21].Style.Font.Bold = true;
                    worksheetShortStock.Cells[1, 19, 1, 21].Style.Font.Color.SetColor(Color.Red);

                    int ShortCaratColumn = DTabShortStock.Columns["Carat"].Ordinal + 1;
                    int ShortMemoRapColumn = DTabShortStock.Columns["Memo Rap($)"].Ordinal + 1;
                    int ShortMemoDiscColumn = DTabShortStock.Columns["Memo Disc(%)($)"].Ordinal + 1;
                    int ShortMemoPricePerCaratColumn = DTabShortStock.Columns["Memo $/Cts($)"].Ordinal + 1;
                    int ShortMemoAmtColumn = DTabShortStock.Columns["Memo Amt($)"].Ordinal + 1;
                    int ShortDiscountColumn = DTabShortStock.Columns["Dis(%)"].Ordinal + 1;
                    int ShortTermsColumn = DTabShortStock.Columns["Term(%)"].Ordinal + 1;
                    int ShortSaleRapColumn = DTabShortStock.Columns["Sale Rap($)"].Ordinal + 1;
                    int ShortSaleDiscountColumn = DTabShortStock.Columns["Sale Disc(%)($)"].Ordinal + 1;
                    int ShortSalePricePerCaratColumn = DTabShortStock.Columns["Sale $/Cts($)"].Ordinal + 1;
                    int ShortSaleAmtColumn = DTabShortStock.Columns["Sale Amt($)"].Ordinal + 1;

                    for (int IntI = 2; IntI <= EndRow; IntI++)
                    {
                        string MemoDiscColumns = Global.ColumnIndexToColumnLetter(ShortMemoDiscColumn) + IntI.ToString();
                        string SaleDiscount = Global.ColumnIndexToColumnLetter(ShortSaleDiscountColumn) + IntI.ToString();

                        worksheetShortStock.Cells[IntI, ShortDiscountColumn].Formula = "=ROUND(" + MemoDiscColumns + " - " + SaleDiscount + ",2)";

                        worksheetShortStock.Cells[IntI, 12, IntI, 15].Style.Font.Bold = true;
                        worksheetShortStock.Cells[IntI, 12, IntI, 15].Style.Font.Color.SetColor(Color.Blue);

                        worksheetShortStock.Cells[IntI, 16, IntI, 18].Style.Font.Color.SetColor(Color.Blue);

                        worksheetShortStock.Cells[IntI, 19, IntI, 21].Style.Font.Bold = true;
                        worksheetShortStock.Cells[IntI, 19, IntI, 21].Style.Font.Color.SetColor(Color.Red);
                    }
                    EndRow = EndRow + 2;
                    worksheetShortStock.Cells[EndRow, 1, EndRow, 1].Value = "Summary";

                    string ShortCaratCol = Global.ColumnIndexToColumnLetter(DTabShortStock.Columns["Carat"].Ordinal + 1);
                    string ShortMemoRap = Global.ColumnIndexToColumnLetter(DTabShortStock.Columns["Memo Rap($)"].Ordinal + 1);
                    string ShortMemoDiscount = Global.ColumnIndexToColumnLetter(DTabShortStock.Columns["Memo Disc(%)($)"].Ordinal + 1);
                    string ShortMemoPricePerCarat = Global.ColumnIndexToColumnLetter(DTabShortStock.Columns["Memo $/Cts($)"].Ordinal + 1);
                    string ShortMemoAmt = Global.ColumnIndexToColumnLetter(DTabShortStock.Columns["Memo Amt($)"].Ordinal + 1);
                    string ShortDisc = Global.ColumnIndexToColumnLetter(DTabShortStock.Columns["Dis(%)"].Ordinal + 1);
                    string ShortTerms = Global.ColumnIndexToColumnLetter(DTabShortStock.Columns["Term(%)"].Ordinal + 1);
                    string ShortSaleRap = Global.ColumnIndexToColumnLetter(DTabShortStock.Columns["Sale Rap($)"].Ordinal + 1);
                    string ShortSaleDisc = Global.ColumnIndexToColumnLetter(DTabShortStock.Columns["Sale Disc(%)($)"].Ordinal + 1);
                    string ShortSalePricePerCarat = Global.ColumnIndexToColumnLetter(DTabShortStock.Columns["Sale $/Cts($)"].Ordinal + 1);
                    string ShortSaleAmt = Global.ColumnIndexToColumnLetter(DTabShortStock.Columns["Sale Amt($)"].Ordinal + 1);

                    int IntShortTotRow = DTabShortStock.Rows.Count + 1;

                    StartRow = StartRow + 1;
                    worksheetShortStock.Cells[EndRow, ShortCaratColumn, EndRow, ShortCaratColumn].Formula = "ROUND(SUBTOTAL(9," + ShortCaratCol + StartRow + ":" + ShortCaratCol + IntShortTotRow + "),2)";
                    worksheetShortStock.Cells[EndRow, ShortMemoRapColumn, EndRow, ShortMemoRapColumn].Formula = "ROUND(SUBTOTAL(9," + ShortMemoRap + StartRow + ":" + ShortMemoRap + IntShortTotRow + "),2)";
                    worksheetShortStock.Cells[EndRow, ShortMemoDiscColumn, EndRow, ShortMemoDiscColumn].Formula = "SUBTOTAL(9," + ShortMemoDiscount + StartRow + ":" + ShortMemoDiscount + IntShortTotRow + ")";
                    worksheetShortStock.Cells[EndRow, ShortMemoPricePerCaratColumn, EndRow, ShortMemoPricePerCaratColumn].Formula = "ROUND(" + ShortMemoAmt + EndRow + "/" + ShortCaratCol + IntShortTotRow + ",0)";
                    worksheetShortStock.Cells[EndRow, ShortMemoAmtColumn, EndRow, ShortMemoAmtColumn].Formula = "ROUND(SUBTOTAL(9," + ShortMemoAmt + StartRow + ":" + ShortMemoAmt + IntShortTotRow + "),2)";
                    worksheetShortStock.Cells[EndRow, ShortDiscountColumn, EndRow, ShortDiscountColumn].Formula = "ROUND((" + ShortMemoDiscount + EndRow + "-" + ShortSaleDisc + EndRow + " ),2)";
                    worksheetShortStock.Cells[EndRow, ShortTermsColumn, EndRow, ShortTermsColumn].Formula = "SUBTOTAL(9," + ShortTerms + StartRow + ":" + ShortTerms + IntShortTotRow + ")";
                    worksheetShortStock.Cells[EndRow, ShortSaleRapColumn, EndRow, ShortSaleRapColumn].Formula = "ROUND(SUBTOTAL(9," + ShortSaleRap + StartRow + ":" + ShortSaleRap + IntShortTotRow + "),2)";
                    worksheetShortStock.Cells[EndRow, ShortSalePricePerCaratColumn, EndRow, ShortSalePricePerCaratColumn].Formula = "ROUND(" + ShortSaleAmt + EndRow + "/" + ShortCaratCol + IntShortTotRow + ",0)";
                    worksheetShortStock.Cells[EndRow, ShortSaleAmtColumn, EndRow, ShortSaleAmtColumn].Formula = "ROUND(SUBTOTAL(9," + ShortSaleAmt + StartRow + ":" + ShortSaleAmt + IntShortTotRow + "),2)";
                    worksheetShortStock.Cells[EndRow, ShortSaleDiscountColumn, EndRow, ShortSaleDiscountColumn].Formula = "SUBTOTAL(9," + ShortSaleDisc + StartRow + ":" + ShortSaleDisc + IntShortTotRow + ")";

                    worksheetShortStock.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Font.Bold = true;

                    worksheetShortStock.Cells[StartRow, ShortCaratColumn, EndRow, ShortCaratColumn].Style.Numberformat.Format = "0.00";
                    worksheetShortStock.Cells[StartRow, ShortMemoRapColumn, EndRow, ShortMemoRapColumn].Style.Numberformat.Format = "0.00";
                    worksheetShortStock.Cells[StartRow, ShortMemoDiscColumn, EndRow, ShortMemoDiscColumn].Style.Numberformat.Format = "0.00";
                    worksheetShortStock.Cells[StartRow, ShortMemoPricePerCaratColumn, EndRow, ShortMemoPricePerCaratColumn].Style.Numberformat.Format = "0.00";
                    worksheetShortStock.Cells[StartRow, ShortMemoAmtColumn, EndRow, ShortMemoAmtColumn].Style.Numberformat.Format = "0.00";
                    worksheetShortStock.Cells[StartRow, ShortDiscountColumn, EndRow, ShortDiscountColumn].Style.Numberformat.Format = "0.00";
                    worksheetShortStock.Cells[StartRow, ShortTermsColumn, EndRow, ShortTermsColumn].Style.Numberformat.Format = "0.00";
                    worksheetShortStock.Cells[StartRow, ShortSaleRapColumn, EndRow, ShortSaleRapColumn].Style.Numberformat.Format = "0.00";
                    worksheetShortStock.Cells[StartRow, ShortSalePricePerCaratColumn, EndRow, ShortSalePricePerCaratColumn].Style.Numberformat.Format = "0.00";
                    worksheetShortStock.Cells[StartRow, ShortSaleAmtColumn, EndRow, ShortSaleAmtColumn].Style.Numberformat.Format = "0.00";
                    worksheetShortStock.Cells[StartRow, ShortSaleDiscountColumn, EndRow, ShortSaleDiscountColumn].Style.Numberformat.Format = "0.00";

                    worksheetShortStock.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheetShortStock.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheetShortStock.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheetShortStock.Cells[1, 1, 100, 100].AutoFitColumns();
                    #endregion

                    #region Bhaveshbhai Stock
                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabBhveshStock.Columns.Count;
                    EndRow = StartRow + DTabBhveshStock.Rows.Count;

                    worksheetBhaveshBhai.Cells[StartRow, StartColumn, EndRow, EndColumn].LoadFromDataTable(DTabBhveshStock, true);
                    worksheetBhaveshBhai.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheetBhaveshBhai.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheetBhaveshBhai.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheetBhaveshBhai.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheetBhaveshBhai.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheetBhaveshBhai.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheetBhaveshBhai.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Name = FontName;
                    worksheetBhaveshBhai.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Size = 10;

                    worksheetBhaveshBhai.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Font.Bold = true;
                    worksheetBhaveshBhai.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetBhaveshBhai.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetBhaveshBhai.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetBhaveshBhai.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheetBhaveshBhai.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetBhaveshBhai.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheetBhaveshBhai.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    worksheetBhaveshBhai.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(Color.Black);

                    worksheetBhaveshBhai.Cells[1, 18, 1, 20].Style.Font.Bold = true;
                    worksheetBhaveshBhai.Cells[1, 18, 1, 20].Style.Font.Color.SetColor(Color.Red);

                    int BhavRxWColumn = DTabBhveshStock.Columns["RXW"].Ordinal + 1;
                    int BhavRapaportColumn = DTabBhveshStock.Columns["Rap.Price"].Ordinal + 1;
                    int BhavPricePerCaratColumn = DTabBhveshStock.Columns["Net Rate"].Ordinal + 1;
                    int BhavDiscountColumn = DTabBhveshStock.Columns["Disc %"].Ordinal + 1;
                    int BhavCaratColumn = DTabBhveshStock.Columns["Carats"].Ordinal + 1;
                    int BhavAmountColumn = DTabBhveshStock.Columns["Net Value"].Ordinal + 1;
                    int BhavDepthPerColumn = DTabBhveshStock.Columns["Depth%"].Ordinal + 1;
                    int BhavTablePerColumn = DTabBhveshStock.Columns["Table%"].Ordinal + 1;
                    int BhavShapeColumn = DTabBhveshStock.Columns["Shape"].Ordinal + 1;

                    for (int IntI = 2; IntI <= EndRow; IntI++)
                    {
                        string BhavRapColumns = Global.ColumnIndexToColumnLetter(BhavRapaportColumn) + IntI.ToString();
                        string BhavDiscount = Global.ColumnIndexToColumnLetter(BhavDiscountColumn) + IntI.ToString();
                        string BhavCarat = Global.ColumnIndexToColumnLetter(BhavCaratColumn) + IntI.ToString();
                        string BhavPricePerCarat = Global.ColumnIndexToColumnLetter(BhavPricePerCaratColumn) + IntI.ToString();

                        worksheetBhaveshBhai.Cells[IntI, BhavRxWColumn].Formula = "=ROUND(" + BhavRapColumns + " * " + BhavCarat + ",2)";
                        worksheetBhaveshBhai.Cells[IntI, BhavPricePerCaratColumn].Formula = "=ROUND(" + BhavRapColumns + " + (" + " ( " + BhavRapColumns + " * " + BhavDiscount + " )/100),2)";
                        worksheetBhaveshBhai.Cells[IntI, BhavAmountColumn].Formula = "=ROUND(" + BhavPricePerCarat + " * " + BhavCarat + ",2)";

                        worksheetBhaveshBhai.Cells[IntI, 18, IntI, 20].Style.Font.Bold = true;
                        worksheetBhaveshBhai.Cells[IntI, 18, IntI, 20].Style.Font.Color.SetColor(Color.Red);
                    }

                    EndRow = EndRow + 2;
                    worksheetBhaveshBhai.Cells[EndRow, 1, EndRow, 1].Value = "Summary";

                    string BhavRxW = Global.ColumnIndexToColumnLetter(DTabBhveshStock.Columns["RXW"].Ordinal + 1);
                    string BhavCaratCol = Global.ColumnIndexToColumnLetter(DTabBhveshStock.Columns["Carats"].Ordinal + 1);
                    string BhavDiscount1 = Global.ColumnIndexToColumnLetter(DTabBhveshStock.Columns["Disc %"].Ordinal + 1);
                    string BhavNetRate = Global.ColumnIndexToColumnLetter(DTabBhveshStock.Columns["Net Rate"].Ordinal + 1);
                    string BhavNetValue = Global.ColumnIndexToColumnLetter(DTabBhveshStock.Columns["Net Value"].Ordinal + 1);

                    int IntBhavTotRow = DTabBhveshStock.Rows.Count + 1;

                    StartRow = StartRow + 1;

                    worksheetBhaveshBhai.Cells[EndRow, BhavShapeColumn, EndRow, BhavShapeColumn].Formula = "SUBTOTAL(2," + BhavCaratCol + StartRow + ":" + BhavCaratCol + IntBhavTotRow + ")";
                    worksheetBhaveshBhai.Cells[EndRow, BhavCaratColumn, EndRow, BhavCaratColumn].Formula = "ROUND(SUBTOTAL(9," + BhavCaratCol + StartRow + ":" + BhavCaratCol + IntBhavTotRow + "),2)";
                    worksheetBhaveshBhai.Cells[EndRow, BhavRxWColumn, EndRow, BhavRxWColumn].Formula = "SUBTOTAL(9," + BhavRxW + StartRow + ":" + BhavRxW + IntBhavTotRow + ")";
                    worksheetBhaveshBhai.Cells[EndRow, BhavAmountColumn, EndRow, BhavAmountColumn].Formula = "ROUND(SUBTOTAL(9," + BhavNetValue + StartRow + ":" + BhavNetValue + IntBhavTotRow + "),2)";
                    worksheetBhaveshBhai.Cells[EndRow, BhavPricePerCaratColumn, EndRow, BhavPricePerCaratColumn].Formula = "ROUND(" + BhavNetValue + EndRow + "/" + BhavCaratCol + EndRow + ",0)";
                    worksheetBhaveshBhai.Cells[EndRow, BhavDiscountColumn, EndRow, BhavDiscountColumn].Formula = "ROUND((" + BhavNetValue + EndRow + "/" + BhavRxW + EndRow + "-1 ) * 100,2)";

                    worksheetBhaveshBhai.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Font.Bold = true;

                    worksheetBhaveshBhai.Cells[StartRow, BhavCaratColumn, EndRow, BhavCaratColumn].Style.Numberformat.Format = "0.00";
                    worksheetBhaveshBhai.Cells[StartRow, BhavDiscountColumn, EndRow, BhavDiscountColumn].Style.Numberformat.Format = "0.00";
                    worksheetBhaveshBhai.Cells[StartRow, BhavAmountColumn, EndRow, BhavAmountColumn].Style.Numberformat.Format = "0.00";
                    worksheetBhaveshBhai.Cells[StartRow, BhavDepthPerColumn, EndRow, BhavDepthPerColumn].Style.Numberformat.Format = "0.00";
                    worksheetBhaveshBhai.Cells[StartRow, BhavTablePerColumn, EndRow, BhavTablePerColumn].Style.Numberformat.Format = "0.00";

                    worksheetBhaveshBhai.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheetBhaveshBhai.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheetBhaveshBhai.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    //worksheet.Column(17).Hidden = true; //Cmnt : Coz due to AutoFitcolumns column can not hide
                    worksheetBhaveshBhai.Cells[1, 1, 100, 100].AutoFitColumns();
                    #endregion



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
