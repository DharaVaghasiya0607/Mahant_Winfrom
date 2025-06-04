using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using DevExpress.XtraPrinting;
using Google.API.Translate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OfficeOpenXml;
using Spire.Xls;
using DevExpress.Data;
using DevExpress.XtraPrintingLinks;
using System.Drawing.Printing;
using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using BusLib.Transaction;
using System.Xml;
using BusLib.Rapaport;
using BusLib.Report;
using MahantExport.Utility;
using DevExpress.XtraGrid.Views.Grid;

namespace MahantExport.MFG
{
    public partial class FrmMFGGradingView : DevControlLib.cDevXtraForm
    {
        String PasteData = "";
        IDataObject PasteclipData = Clipboard.GetDataObject();

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        BOFindRap ObjRap = new BOFindRap();
        DataTable DTabGrdData = new DataTable();
        DataTable DTabPara = new DataTable();

        BODevGridSelection ObjGridSelection;

        #region Property Settings

        public FrmMFGGradingView()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            DTabPara = new BOMST_Parameter().GetParameterData();

            DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            DtpToDate.Value = DateTime.Now;

            this.Show();

            if (MainGrd.RepositoryItems.Count == 3)
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
            GrdDet.Columns["COLSELECTCHECKBOX"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            if (ObjGridSelection != null)
            {
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            }
            FillListControls();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = false;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        #endregion

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void BtnBestFit_Click(object sender, EventArgs e)
        {
            GrdDet.BestFitColumns();
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
               
                string StrStatus = "";
                if (RbtAll.Checked == true)
                {
                    StrStatus = "ALL";
                }
                else if (RbtPending.Checked == true)
                {
                    StrStatus = "PENDING";
                }
                else if (RbtTransfer.Checked == true)
                {
                    StrStatus = "TRANSFER";
                }  
                else if(RBtnPacking.Checked == true) // K : 27/12/2022
                {
                    StrStatus = "PACKING";
                }

                Trn_SinglePrdProperty Property = new Trn_SinglePrdProperty();
                Property.MFGGradingNo = Val.ToInt64(txtGradingNo.Text);
                Property.MfgGradingStatus = StrStatus;

                Property.StockNo = Val.Trim(rTxtStoneCertiMfgMemo.Text); //hinal 01-01-2022

                DTabGrdData = ObjStock.MFGGradingLiveStockGetDetail(Property, Val.SqlDate(DTPFromDate.Value.ToShortDateString()), Val.SqlDate(DtpToDate.Value.ToShortDateString()));

                int IntAll = 0;
                int IntTransfer = 0;
                int IntPending = 0;
                int IntPacking = 0;

                foreach (DataRow DRow in DTabGrdData.Rows)
                {
                    IntAll++;
                    if (Val.ToString(DRow["MFGGRADINGSTATUS"]) == "TRANSFER")
                    {
                        IntTransfer++;
                    }
                    else if (Val.ToString(DRow["MFGGRADINGSTATUS"]) == "PENDING")
                    {
                        IntPending++;
                    }
                    else if (Val.ToString(DRow["MFGGRADINGSTATUS"]) == "PACKING")
                    {
                        IntPacking++;
                    }
                }

                lblAll.Text = "ALL (" + IntAll.ToString() + ")";
                lbPending.Text = "PENDING (" + IntPending.ToString() + ")";
                lblTransfer.Text = "TRANSFER (" + IntTransfer.ToString() + ")";
                lblPacking.Text = "PACKING (" + IntPacking.ToString() + ")";

                MainGrd.DataSource = DTabGrdData;
                GrdDet.RefreshData();
                GrdDet.BestFitMaxRowCount = 500;
                GrdDet.BestFitColumns();

                CalculateSummary();

                if (RbtPending.Checked)
                {
                    BtnTransfer.Enabled = true;
                }
                else if(RBtnPacking.Checked)
                {
                    BtnTransfer.Enabled = true;
                }
                else
                {
                    BtnTransfer.Enabled = false;
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDet_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                CalculateSummary();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void FillListControls()
        {
            DataTable DTab = new DataTable();
            DTab = DTabPara.Select("PARATYPE = 'TABLEINC'").CopyToDataTable();
            repCmbTableInc.DataSource = DTab;
            repCmbTableInc.DisplayMember = "SHORTNAME";
            repCmbTableInc.ValueMember = "PARA_ID";

            DTab = DTabPara.Select("PARATYPE = 'SIDETABLEINC'").CopyToDataTable();
            repCmbSideTable.DataSource = DTab;
            repCmbSideTable.DisplayMember = "SHORTNAME";
            repCmbSideTable.ValueMember = "PARA_ID";
        }

        private void BtnTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                string StrOpe = "";
                DataTable DTab = new DataTable();
                DTab.Rows.Clear();
                DTab = Global.GetSelectedRecordOfGrid(GrdDet, true, ObjGridSelection);
                if (DTab.Rows.Count == 0)
                {
                    Global.Message("Please Select atleast One Record For Transfer");
                    return;
                }
                if(RbtPending.Checked == true)
                {
                    StrOpe = "PACKING";
                }
                else if(RBtnPacking.Checked == true)
                {
                    StrOpe = "TRANSFER";
                }

                DTab.TableName = "DETAIL";

                string ParameterUpdateXml;
                using (StringWriter sw = new StringWriter())
                {
                    DTab.WriteXml(sw);
                    ParameterUpdateXml = sw.ToString();
                }

                Trn_SinglePrdProperty Property = new Trn_SinglePrdProperty();
                Property.MFGGradingNo = Val.ToInt64(txtGradingNo.Text);
                Property.XMLDETSTR = ParameterUpdateXml;

                Property = ObjStock.MFGGradingTransferToStock(Property, StrOpe);
                this.Cursor = Cursors.Default;
                txtGradingNo.Text = Property.ReturnValue;
                Global.Message(Property.ReturnMessageDesc);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    DTabGrdData.AcceptChanges();
                    ObjGridSelection.ClearSelection();
                }
            }
            catch (Exception EX)
            {
                Global.Message(EX.Message.ToString());
            }
        }

        public void CalculateSummary()
        {
            double DblSelectedRapAmount = 0.00;
            double DblSelectedRapaport = 0.00;
            double DblSelectedDiscount = 0.00;

            txtTotalPcs.Text = string.Empty;
            txtTotalCarat.Text = string.Empty;
            txtTotalDisc.Text = string.Empty;
            txtTotalAmount.Text = string.Empty;
            txtTotalPricePerCarat.Text = string.Empty;

            txtSelectedPcs.Text = string.Empty;
            txtSelectedCarat.Text = string.Empty;
            txtSelectedDisc.Text = string.Empty;
            txtSelectedAmount.Text = string.Empty;
            txtSelectedPricePerCarat.Text = string.Empty;

            DataTable DTab = Global.GetSelectedRecordOfGrid(GrdDet, true, ObjGridSelection);

            if (DTab != null && DTab.Rows.Count > 0)
            {
                txtSelectedPcs.Text = DTab.Rows.Count.ToString();
                txtSelectedCarat.Text = Val.ToString(DTab.Compute("SUM(CARAT)", string.Empty));
                txtSelectedAmount.Text = Val.ToString(DTab.Compute("SUM(MFGAMOUNT)", string.Empty));
                txtSelectedPricePerCarat.Text = string.Format("{0:0.00}", Val.Val(txtSelectedAmount.Text) / Val.Val(txtSelectedCarat.Text));

                foreach (DataRow DRow in DTab.Rows)
                {
                    DblSelectedRapAmount += Val.Val(DRow["MFGRAPAPORT"]) * Val.Val(DRow["CARAT"]);
                }

                DblSelectedRapaport = DblSelectedRapAmount / Val.Val(txtSelectedCarat.Text);
                DblSelectedDiscount = (Val.Val(txtSelectedPricePerCarat.Text) - Math.Round(DblSelectedRapaport, 2)) / Math.Round(DblSelectedRapaport, 2) * 100;
                txtSelectedDisc.Text = string.Format("{0:0.00}", DblSelectedDiscount, string.Empty);

            }

            if (DTabGrdData != null && DTabGrdData.Rows.Count > 0)
            {
                txtTotalPcs.Text = DTabGrdData.Rows.Count.ToString();
                txtTotalCarat.Text = Val.ToString(DTabGrdData.Compute("SUM(CARAT)", string.Empty));
                txtTotalAmount.Text = Val.ToString(DTabGrdData.Compute("SUM(MFGAMOUNT)", string.Empty));
                txtTotalPricePerCarat.Text = string.Format("{0:0.00}", Val.Val(txtTotalAmount.Text) / Val.Val(txtTotalCarat.Text));

                foreach (DataRow DRow in DTab.Rows)
                {
                    DblSelectedRapAmount += Val.Val(DRow["MFGRAPAPORT"]) * Val.Val(DRow["CARAT"]);
                }

                DblSelectedRapaport = DblSelectedRapAmount / Val.Val(txtTotalCarat.Text);
                DblSelectedDiscount = (Val.Val(txtTotalPricePerCarat.Text) - Math.Round(DblSelectedRapaport, 2)) / Math.Round(DblSelectedRapaport, 2) * 100;
                txtTotalDisc.Text = string.Format("{0:0.00}", DblSelectedDiscount, string.Empty);
            }
          
        }

        private void GrdDet_KeyUp(object sender, KeyEventArgs e)
        {
            if (GrdDet.FocusedColumn.FieldName == "COLSELECTCHECKBOX" && e.KeyCode == Keys.Space)
            {
                try
                {
                    CalculateSummary();
                }
                catch (Exception ex)
                {
                    Global.Message(ex.Message);
                }
            }
        }

        private void GrdDet_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                return;
            }

            string StrStatus = Val.ToString(GrdDet.GetRowCellValue(e.RowHandle, "MFGGRADINGSTATUS"));
            if (StrStatus == "PENDING")
            {
                e.Appearance.BackColor = lblPending.BackColor;
                e.Appearance.BackColor2 = lblPending.BackColor;
            }

            else if (StrStatus == "TRANSFER")
            {
                e.Appearance.BackColor = Color.Transparent;
                e.Appearance.BackColor2 = Color.Transparent;
            }
        }

        private DataTable GetTableOfSelectedRows(GridView view, Boolean IsSelect, BODevGridSelection pSelectGrid)
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
                if (pSelectGrid != null)
                {
                    aryLst = pSelectGrid.GetSelectedArrayList();
                    resultTable = sourceTable.Clone();
                    for (int i = 0; i < aryLst.Count; i++)
                    {
                        DataRowView oDataRowView = aryLst[i] as DataRowView;
                        resultTable.Rows.Add(oDataRowView.Row.ItemArray);
                    }
                }
            }

            return resultTable;
        }

        private void btnExcelWithFormat_Click(object sender, EventArgs e)
        {
       
            DataTable DTabDetail = GetTableOfSelectedRows(GrdDet, true, ObjGridSelection);

            if (DTabDetail.Rows.Count <= 0)
            {
                this.Cursor = Cursors.Default;
                Global.Message("Please Select AtLeast One Record From The List.");
                return;
            }

            string StrStockNo = "";
            foreach (DataRow DR in DTabDetail.Rows)
            {
                StrStockNo = StrStockNo + DR["PARTYSTOCKNO"] + ",";
            }

            StrStockNo = StrStockNo.Remove(StrStockNo.Length - 1, 1);

            string StrFileName = ExportExcelNew(StrStockNo);
            if (StrFileName == "")
            {
                Global.Message("Please Select Atleast One Packet");
                return;
            }
            if (Global.Confirm("Do You Want To Open File ? ") == System.Windows.Forms.DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(StrFileName, "CMD");
            }
        }

        public string ExportExcelNew(string PartyStockNo)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                Trn_SinglePrdProperty Property = new Trn_SinglePrdProperty();
                Property.MFGGradingNo = 0;
                Property.StockNo = PartyStockNo;
                DataSet DS = ObjStock.MFGGradingGetDetail_Export(Property);

                this.Cursor = Cursors.Default;


                DataTable DTabDetail = DS.Tables[0];
                DataTable DtabMemo = DS.Tables[1];   //hinal 29-12-2021

                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }

                DTabDetail.DefaultView.Sort = "LOTNAME";
                DTabDetail = DTabDetail.DefaultView.ToTable();

                DtabMemo.DefaultView.Sort = "KAPANNAME";
                DtabMemo = DtabMemo.DefaultView.ToTable();

                this.Cursor = Cursors.WaitCursor;

                string StrFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";

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

                //hinal 29-12-2021
                int StartRowMemo = 0;
                int StartColumnMemo = 0;
                int EndRowMemo = 0; 
                int EndColumnMemo = 0;


                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Sheet1");
                    ExcelWorksheet worksheetMemo = xlPackage.Workbook.Worksheets.Add("MEMO");

                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    // hinal 29-12-2021
                    StartRowMemo = 1;
                    StartColumnMemo = 1;
                    EndRowMemo = StartRowMemo;
                    EndColumnMemo = DtabMemo.Columns.Count; 

                    #region Stock Detail

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

                    worksheet.Cells[1, 1, 1, 45].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    //worksheet.Cells[1, 14, 1, 16].Style.Font.Color.SetColor(Color.Red);
                    //worksheet.Cells[1, 24, 1, 24].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    //worksheet.Cells[1, 25, 1, 25].Style.Fill.BackgroundColor.SetColor(Color.Red);
                    //worksheet.Cells[1, 28, 1, 28].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    //worksheet.Cells[1, 29, 1, 32].Style.Font.Color.SetColor(Color.Red);
                    //worksheet.Cells[1, 33, 1, 33].Style.Fill.BackgroundColor.SetColor(Color.Red);

                    // Header Set

                    //hinal 29-12-2021
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, EndRowMemo, EndColumnMemo].LoadFromDataTable(DtabMemo, true);
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, EndRowMemo, EndColumnMemo].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, EndRowMemo, EndColumnMemo].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, EndRowMemo, EndColumnMemo].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, EndRowMemo, EndColumnMemo].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, EndRowMemo, EndColumnMemo].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, EndRowMemo, EndColumnMemo].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, EndRowMemo, EndColumnMemo].Style.Font.Name = FontName;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, EndRowMemo, EndColumnMemo].Style.Font.Size = FontSize;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, StartRowMemo, EndColumnMemo].Style.Font.Bold = true;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, StartRowMemo, EndColumnMemo].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, StartRowMemo, EndColumnMemo].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, StartRowMemo, EndColumnMemo].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, StartRowMemo, EndColumnMemo].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheetMemo.Cells[StartRowMemo, 1, StartRowMemo, EndColumnMemo].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetMemo.Cells[StartRowMemo, 1, StartRowMemo, EndColumnMemo].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheetMemo.Cells[StartRowMemo, 1, StartRowMemo, EndColumnMemo].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheetMemo.Cells[StartRowMemo, 1, StartRowMemo, EndColumnMemo].Style.Font.Color.SetColor(FontColor);

                    worksheetMemo.Cells[1, 1, 1, 3].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    //hinal 29-12-2021

                
                    for (int i = 1; i <= DTabDetail.Columns.Count; i++)
                    {
                        string StrHeader = Global.ExportExcelHeaderMfg(Val.ToString(worksheet.Cells[StartRow, i].Value), worksheet, i);
                        worksheet.Cells[StartRow, i].Value = StrHeader;
                    }
                   
                    for (int i = 1; i <= DtabMemo.Columns.Count; i++)
                    {

                        string StrHeader = Global.ExportExcelHeaderMemo(Val.ToString(worksheetMemo.Cells[StartRow, i].Value), worksheetMemo, i);
                        worksheetMemo.Cells[StartRowMemo, i].Value = StrHeader;
                        //worksheetMemo.Cells[StartRowMemo, StartColumnMemo + 1].Formula = "SUM(" + StrHeader + ")";
                        //count++;
                    }
                    
                    //worksheet.Cells[StartRowMemo,3].Formula = "SUM("+count+")";
                    //int rowCount = sheet.Rows.Count();
                    //worksheetMemo.Cells[StartRow].Value = rowCount;
                    //int number1 = worksheetMemo.Rows.Count - 1;
     
                    

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

        private void rTxtStoneCertiMfgMemo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String str1 = "";
                if (string.IsNullOrEmpty(rTxtStoneCertiMfgMemo.Text))
                {
                    lblTotalCount.Text = "(0)";
                }
                else
                {
                    if (rTxtStoneCertiMfgMemo.Text.Trim().Contains("\t\n"))
                    {
                        str1 = rTxtStoneCertiMfgMemo.Text.Trim().Replace("\t\n", ",");
                    }
                    else
                    {
                        str1 = rTxtStoneCertiMfgMemo.Text.Trim().Replace("\n", ",");

                    }
                    rTxtStoneCertiMfgMemo.Text = str1;
                    rTxtStoneCertiMfgMemo.Select(rTxtStoneCertiMfgMemo.Text.Length, 0);
                    lblTotalCount.Text = "(" + rTxtStoneCertiMfgMemo.Text.Split(',').Length + ")";
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnBarcodePrint_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTab = GetTableOfSelectedRows(GrdDet, true);

                if (DTab.Rows.Count == 0)
                {
                    Global.Message("Please Select at lease One Row For Barcode Print");
                    return;
                }

                if (Global.Confirm("Are you Sure You Want For Print Barcode of All Selected Stones?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                foreach (DataRow DRow in DTab.Rows)
                {
                    Global.MFGGradingBarcodePrint(DRow);
                }
                Global.Message("Print Successfully");
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
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
    }
}
