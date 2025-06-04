using BusLib.Configuration;
using BusLib.Master;
using BusLib.Rapaport;
using BusLib.TableName;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;

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

namespace MahantExport.Report
{
    public partial class FrmRapComparision : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_ParameterDiscount ObjTrn = new BOTRN_ParameterDiscount();
        BOFormPer ObjPer = new BOFormPer();

        DataTable DTabDiscountData = new DataTable();
        DataTable DTabRapaportData = new DataTable();
        DataTable DTabRangeData = new DataTable();
        DataTable DTabRapaportCriteria = new DataTable();
        
        #region Property Settings

        public FrmRapComparision()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();
            
         }

        public void AttachFormDefaultEvent()
        {
           ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = false;
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

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        #endregion

        #region Rapaport

        private void BtnRapaportGetData_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            double DouFromSize = 0;
            double DouToSize = 0;

            if (Val.ToString(txtSize.Text) != "")
            {
                DouFromSize = Val.Val(Val.ToString(txtSize.Text).Split('-')[0]);
                DouToSize = Val.Val(Val.ToString(txtSize.Text).Split('-')[1]);
            }

            DTabRapaportData = ObjTrn.GetRapnetDataComparision("RAPVALUE", 
                txtRapDate1.Text,
                txtRapDate2.Text,
                txtRapDate3.Text,
                txtRapDate4.Text,
                txtRapDate5.Text,
                txtRapDate6.Text, 

                txtShape.Text, DouFromSize, DouToSize);

            MainGridRapaport.DataSource = DTabRapaportData;
            MainGridRapaport.Refresh();
            GrdDet.BestFitColumns();

            GrdDet.Bands["BAND1"].Caption = txtRapDate1.Text;
            GrdDet.Bands["BAND2"].Caption = txtRapDate2.Text;
            GrdDet.Bands["BAND3"].Caption = txtRapDate3.Text;
            GrdDet.Bands["BAND4"].Caption = txtRapDate4.Text;
            GrdDet.Bands["BAND5"].Caption = txtRapDate5.Text;
            GrdDet.Bands["BAND6"].Caption = txtRapDate6.Text;

            this.Cursor = Cursors.Default;
        }

        #endregion


        private void txtRapDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {

                AxonContLib.cTextBox txt = (AxonContLib.cTextBox)sender;

                if (Global.OnKeyPressEveToPopup(e))
                {
                    this.Cursor = Cursors.WaitCursor;
                    DataTable DTabParameter = ObjTrn.GetOriginalRapData("RAPDATE", "", "", 0, 0);
                    DTabParameter.DefaultView.Sort = "RAPDATE DESC";
                    DTabParameter = DTabParameter.DefaultView.ToTable();

                    
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "RAPDATE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = DTabParameter;

                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txt.Text = DateTime.Parse(Val.ToString(FrmSearch.DRow["RAPDATE"])).ToString("dd/MM/yyyy");
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

        private void txtShape_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    this.Cursor = Cursors.WaitCursor;
                    DataTable DTabParameter = ObjTrn.GetOriginalRapData("SHAPE", txtRapDate1.Text, "", 0, 0);
                    
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "SHAPE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = DTabParameter;

                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtShape.Text = Val.ToString(FrmSearch.DRow["SHAPE"]);
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

        private void txtSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    this.Cursor = Cursors.WaitCursor;
                    DataTable DTabParameter = ObjTrn.GetOriginalRapData("SIZE", txtRapDate1.Text, txtShape.Text, 0, 0);

                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "SIZE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = DTabParameter;

                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtSize.Text = Val.ToString(FrmSearch.DRow["SIZE"]);
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

        private void GrdDet_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000" || e.DisplayText == "0.0000")
            {
                e.DisplayText = String.Empty;
            }


        }

        private void BtnExportRapaport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTabExport = ObjTrn.GetRapComparisionExportGetData(Val.SqlDate(txtRapDate1.Text), Val.SqlDate(txtRapDate2.Text));

                if (DTabExport == null || DTabExport.Rows.Count == 0)
                {
                    Global.MessageError("No Data Found For Export Please Check Once !!");
                    return;
                }

                string StrFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + "RapComparision_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.LightGray;
                Color FontColor = Color.Black;
                string FontName = "Calibri";
                float FontSize = 10;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int StartHeaderColumn = 1;

                this.Cursor = Cursors.WaitCursor;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Sheet1");

                    StartRow = 1;
                    EndRow = StartRow;
                    StartColumn = 1;

                    DataTable DTabFancy = DTabExport.Select("SHAPETYPE = 'Fancy'").CopyToDataTable();
                    DataTable DTabDiscountDistinct = DTabExport.DefaultView.ToTable(true, "SIZE", "SHAPETYPE");
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

                        if (StartColumn == 1 && StartRow == 1 || StartColumn == 15 && StartRow == 1)
                        {
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Value = " " + StrShapeType + "";
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

                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Fill.PatternColor.SetColor(Color.FromArgb(255, 192, 128));
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 128));
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Font.Color.SetColor(FontColor);

                            StartRow = StartRow + 1;
                        }

                        
                        StartColumn = StartColumn + 1;

                        DataRow[] UDRow = DTabExport.Select("SIZE='" + StrSize + "' AND SHAPETYPE = '" + StrShapeType + "'");
                        if (UDRow.Length == 0)
                        {
                            continue;
                        }
                        worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn - 1].Value = StrSize;
                        worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn - 1].Style.Font.Bold = true;
                        worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn - 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn - 1].Style.Fill.PatternColor.SetColor(Color.LightSteelBlue);
                        worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn - 1].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);


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
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 11].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 11].Style.Fill.PatternColor.SetColor(Color.LightSteelBlue);
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 11].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);

                        StartRow = StartRow + 1;

                        foreach (DataRow DRowDetail in UDRow)
                        {
                            worksheet.Cells[StartRow, StartHeaderColumn, StartRow, StartHeaderColumn].Value = Val.ToString(DRowDetail["C_NAME"]);
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Value = Val.ToString(DRowDetail["Q1"]);
                            worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = Val.ToString(DRowDetail["Q2"]);
                            worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = Val.ToString(DRowDetail["Q3"]);
                            worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = Val.ToString(DRowDetail["Q4"]);
                            worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = Val.ToString(DRowDetail["Q5"]);
                            worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = Val.ToString(DRowDetail["Q6"]);
                            worksheet.Cells[StartRow, StartColumn + 6, StartRow, StartColumn + 6].Value = Val.ToString(DRowDetail["Q7"]);
                            worksheet.Cells[StartRow, StartColumn + 7, StartRow, StartColumn + 7].Value = Val.ToString(DRowDetail["Q8"]);
                            worksheet.Cells[StartRow, StartColumn + 8, StartRow, StartColumn + 8].Value = Val.ToString(DRowDetail["Q9"]);
                            worksheet.Cells[StartRow, StartColumn + 9, StartRow, StartColumn + 9].Value = Val.ToString(DRowDetail["Q10"]);
                            worksheet.Cells[StartRow, StartColumn + 10, StartRow, StartColumn + 10].Value = Val.ToString(DRowDetail["Q11"]);
                            worksheet.Cells[StartRow, StartColumn + 11, StartRow, StartColumn + 11].Value = Val.ToString(DRowDetail["Q12"]);

                            worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.Font.Name = FontName;
                            worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.Font.Size = FontSize;
                            //worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            //worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.Fill.PatternColor.SetColor(Color.FromArgb(225, 150, 150));
                            //worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(225, 150, 150));
                            worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 11].Style.Font.Color.SetColor(FontColor);
                            worksheet.Cells[StartRow, StartHeaderColumn, StartRow, StartHeaderColumn].Style.Font.Bold = true;

                            StartRow = StartRow + 1;

                        }
                        //StartRow = StartRow + 1;
                    }

                    AddFancyShapeDetail(worksheet, DTabFancy);

                    worksheet.Cells[1, 1, 1000, 1000].AutoFitColumns();

                    xlPackage.Save();

                    if (Global.Confirm("Do You Want To Open File ?") == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(StrFilePath, "CMD");
                    }
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }
        public void AddFancyShapeDetail(ExcelWorksheet worksheet, DataTable pDtab)
        {
            Color BackColor = Color.LightGray;
            Color FontColor = Color.Black;
            string FontName = "Calibri";
            float FontSize = 10;

            int StartRow = 1;
            int StartColumn = 29;
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
                    StartColumn = 29;
                    StartHeaderColumn = 29;
                    EndRow = StartRow;
                }
                else
                {
                    intCnt = 0;
                    StartColumn = 43;
                    StartHeaderColumn = 43;
                    StartRow = EndRow;
                }

                string StrSize = Val.ToString(DRowDisc["SIZE"]);
                string StrShapeType = Val.ToString(DRowDisc["SHAPETYPE"]);
                if (StartColumn == 29 && StartRow == 1 || StartColumn == 43 && StartRow == 1)
                {
                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Value = "" + StrShapeType + " ";
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
                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Fill.PatternColor.SetColor(Color.FromArgb(255, 192, 128));
                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(255, 192, 128));
                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 12].Style.Font.Color.SetColor(FontColor);

                    StartRow = StartRow + 1;
                }
                StartColumn = StartColumn + 1;

                DataRow[] UDRow = pDtab.Select("SIZE='" + StrSize + "' AND SHAPETYPE = '" + StrShapeType + "'");
                if (UDRow.Length == 0)
                {
                    continue;
                }

                worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn - 1].Value = StrSize;
                worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn - 1].Style.Font.Bold = true;
                worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn - 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn - 1].Style.Fill.PatternColor.SetColor(Color.LightSteelBlue);
                worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn - 1].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);

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
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 11].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 11].Style.Fill.PatternColor.SetColor(Color.LightSteelBlue);
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 11].Style.Fill.BackgroundColor.SetColor(Color.LightSteelBlue);


                StartRow = StartRow + 1;

                foreach (DataRow DRowDetail in UDRow)
                {
                    worksheet.Cells[StartRow, StartHeaderColumn, StartRow, StartHeaderColumn].Value = Val.ToString(DRowDetail["C_NAME"]);
                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Value = Val.ToString(DRowDetail["Q1"]);
                    worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = Val.ToString(DRowDetail["Q2"]);
                    worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = Val.ToString(DRowDetail["Q3"]);
                    worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = Val.ToString(DRowDetail["Q4"]);
                    worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = Val.ToString(DRowDetail["Q5"]);
                    worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = Val.ToString(DRowDetail["Q6"]);
                    worksheet.Cells[StartRow, StartColumn + 6, StartRow, StartColumn + 6].Value = Val.ToString(DRowDetail["Q7"]);
                    worksheet.Cells[StartRow, StartColumn + 7, StartRow, StartColumn + 7].Value = Val.ToString(DRowDetail["Q8"]);
                    worksheet.Cells[StartRow, StartColumn + 8, StartRow, StartColumn + 8].Value = Val.ToString(DRowDetail["Q9"]);
                    worksheet.Cells[StartRow, StartColumn + 9, StartRow, StartColumn + 9].Value = Val.ToString(DRowDetail["Q10"]);
                    worksheet.Cells[StartRow, StartColumn + 10, StartRow, StartColumn + 10].Value = Val.ToString(DRowDetail["Q11"]);
                    worksheet.Cells[StartRow, StartColumn + 11, StartRow, StartColumn + 11].Value = Val.ToString(DRowDetail["Q12"]);

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
                //StartRow = StartRow + 1;
            }

        }

        private void FrmRapComparision_Load(object sender, EventArgs e)
        {
            DataTable DTabParameter = ObjTrn.GetOriginalRapData("RAPDATE", "", "", 0, 0);
            DTabParameter.DefaultView.Sort = "RAPDATE DESC";
            DTabParameter = DTabParameter.DefaultView.ToTable();
            if (DTabParameter.Rows.Count >= 6 )
            {
                txtRapDate1.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[0]["RAPDATE"])).ToString("dd/MM/yyyy");
                txtRapDate2.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[1]["RAPDATE"])).ToString("dd/MM/yyyy");
                txtRapDate3.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[2]["RAPDATE"])).ToString("dd/MM/yyyy");
                txtRapDate4.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[3]["RAPDATE"])).ToString("dd/MM/yyyy");
                txtRapDate5.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[4]["RAPDATE"])).ToString("dd/MM/yyyy");
                txtRapDate6.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[5]["RAPDATE"])).ToString("dd/MM/yyyy");
            }
            else if (DTabParameter.Rows.Count >= 5)
            {
                txtRapDate1.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[0]["RAPDATE"])).ToString("dd/MM/yyyy");
                txtRapDate2.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[1]["RAPDATE"])).ToString("dd/MM/yyyy");
                txtRapDate3.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[2]["RAPDATE"])).ToString("dd/MM/yyyy");
                txtRapDate4.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[3]["RAPDATE"])).ToString("dd/MM/yyyy");
                txtRapDate5.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[4]["RAPDATE"])).ToString("dd/MM/yyyy");
                txtRapDate6.Text = string.Empty;
            }
            else if (DTabParameter.Rows.Count >= 4)
            {
                txtRapDate1.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[0]["RAPDATE"])).ToString("dd/MM/yyyy");
                txtRapDate2.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[1]["RAPDATE"])).ToString("dd/MM/yyyy");
                txtRapDate3.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[2]["RAPDATE"])).ToString("dd/MM/yyyy");
                txtRapDate4.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[3]["RAPDATE"])).ToString("dd/MM/yyyy");
                txtRapDate5.Text = string.Empty;
                txtRapDate6.Text = string.Empty;
            }
            else if (DTabParameter.Rows.Count >= 3)
            {
                txtRapDate1.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[0]["RAPDATE"])).ToString("dd/MM/yyyy");
                txtRapDate2.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[1]["RAPDATE"])).ToString("dd/MM/yyyy");
                txtRapDate3.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[2]["RAPDATE"])).ToString("dd/MM/yyyy");
                txtRapDate4.Text = string.Empty;
                txtRapDate5.Text = string.Empty;
                txtRapDate6.Text = string.Empty;
            }
            else if (DTabParameter.Rows.Count >= 2)
            {
                txtRapDate1.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[0]["RAPDATE"])).ToString("dd/MM/yyyy");
                txtRapDate2.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[1]["RAPDATE"])).ToString("dd/MM/yyyy");
                txtRapDate3.Text = string.Empty;
                txtRapDate4.Text = string.Empty;
                txtRapDate5.Text = string.Empty;
                txtRapDate6.Text = string.Empty;
            }
            else if (DTabParameter.Rows.Count >= 1)
            {
                txtRapDate1.Text = DateTime.Parse(Val.ToString(DTabParameter.Rows[0]["RAPDATE"])).ToString("dd/MM/yyyy");
                txtRapDate2.Text = string.Empty;
                txtRapDate3.Text = string.Empty;
                txtRapDate4.Text = string.Empty;
                txtRapDate5.Text = string.Empty;
                txtRapDate6.Text = string.Empty;
            }
        }
    }
}
