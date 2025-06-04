using MahantExport.Masters;
using MahantExport.Parcel;
using MahantExport.Stock;
using MahantExport.Utility;
using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using Google.API.Translate;
using OfficeOpenXml;
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
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MahantExport.Parcel
{
    public partial class FrmParcelLiveStock : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUploadParcel ObjStock = new BOTRN_StockUploadParcel();
        DataTable DtabLiveStockDetail = new DataTable();
        DataTable DtabLiveStockSummary = new DataTable();
        DataTable DTabSummaryClob = new DataTable();
        DataTable DTabPageSummary = new DataTable();
        Hashtable hash = new Hashtable();

        BOFormPer ObjPer = new BOFormPer();
        DataTable DTabSummary = new DataTable();

        string StrEmail = "";

        DataTable DTabSelection = new DataTable();

        BODevGridSelection ObjGridSelection;

        int mIntSelectedPage = 0;
        int mIntTotalPage = 0;
        int mIntPageSize = 0;

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

        double DouFMEMOAMOUNT = 0;
        double DouMEMOAMOUNT = 0;
        double DouSaleCarat = 0;


        string mStrStockStatus = "";

        string mStrStockType = "";

        #region Property Settings

        public FrmParcelLiveStock()
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

            mStrStockType = "PARCEL";

            GrdDetail.BeginUpdate();
            if (MainGrdDetail.RepositoryItems.Count == 0)
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
                ObjGridSelection.ClearSelection();
            }

            GrdDetail.EndUpdate();
            if (ObjGridSelection != null)
            {
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            }

            string Str = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdDetail.Name);

            if (Str != "")
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                MemoryStream stream = new MemoryStream(byteArray);
                GrdDetail.RestoreLayoutFromStream(stream);

            }
        }

        public void ShowForm(string StrFromSize, string StrToSize, string StrColor_ID, string StrClarity_ID, string StrShapName)  //Call When Double Click On Current Stock Color Clarity Wise Report Data
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            GrdDetail.BeginUpdate();
            if (MainGrdDetail.RepositoryItems.Count == 0)
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
                ObjGridSelection.ClearSelection();
            }

            GrdDetail.EndUpdate();
            if (ObjGridSelection != null)
            {
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            }

            this.Show();
            BtnSearch_Click(null, null);

            string Str = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdDetail.Name);

            if (Str != "")
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                MemoryStream stream = new MemoryStream(byteArray);
                GrdDetail.RestoreLayoutFromStream(stream);

            }
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            //ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjStock);
        }

        #endregion       
        
        #region Enter Event

        private void ControlEnterForGujarati_Enter(object sender, EventArgs e)
        {
            Global.SelectLanguage(Global.LANGUAGE.GUJARATI);
        }
        private void ControlEnterForEnglish_Enter(object sender, EventArgs e)
        {
            Global.SelectLanguage(Global.LANGUAGE.ENGLISH);
        }


        #endregion

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmLedger_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Escape)
            //{
            //    if (Global.Confirm("Do You Want To Close The Form?") == System.Windows.Forms.DialogResult.Yes)
            //        BtnBack_Click(null, null);
            //}
        }


        private void BtnExport_Click(object sender, EventArgs e)
        {
           
            Global.ExcelExport("ParcelStock.xlsx", GrdDetail);

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                bool StrisZeroCarat = false;
                if(ChkZeroCarat.Checked == true)
                {
                    StrisZeroCarat = true;
                }                    
                Int32 StrLot_Id = Val.ToInt32(txtLotName.Tag);
                Int32 StrBox_Id = Val.ToInt32(txtBoxName.Tag);
                Int32 StrSize_Id = Val.ToInt32(txtSizeName.Tag);
                Int32 StrColor_Id = Val.ToInt32(txtColorName.Tag);
                Int32 StrPurity_Id = Val.ToInt32(txtPurityName.Tag);
                Int32 StrShape_Id = Val.ToInt32(txtShapeName.Tag);

                string StrCutNo = Val.ToString(txtCutNo.Text);

                DTabSummary = ObjStock.GetParcelLiveStockData(StrLot_Id, StrBox_Id, StrSize_Id, StrColor_Id, StrPurity_Id, StrShape_Id, StrCutNo, StrisZeroCarat);
                MainGrdDetail.DataSource = DTabSummary;
                GrdDetail.BestFitColumns();

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            DTabSummary.Rows.Clear();
            txtLotName.Text = string.Empty;
            txtLotName.Tag = string.Empty;
            txtBoxName.Text = string.Empty;
            txtBoxName.Tag = string.Empty;
            txtSizeName.Text = string.Empty;
            txtSizeName.Tag = string.Empty;
            txtColorName.Text = string.Empty;
            txtColorName.Tag = string.Empty;
            txtPurityName.Text = string.Empty;
            txtPurityName.Tag = string.Empty;
            txtShapeName.Text = string.Empty;
            txtShapeName.Tag = string.Empty;
            txtCutNo.Text = string.Empty;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblSaveLayout_Click(object sender, EventArgs e)
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

        private void lblDefaultLayout_Click(object sender, EventArgs e)
        {
            int IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdDetail.Name);
            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Deleted");
            }
        }

        private void FrmMixToSingleTransfer_FormClosing(object sender, FormClosingEventArgs e)
        {
            BtnSearch.PerformClick();
        }

        private void GrdDetail_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                if (GrdDetail.FocusedRowHandle < 0)
                    return;

                if (e.Clicks == 2 && Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("STOCKNO")) 
                {
                    FrmCoupleStoneUpload FrmCoupleStoneUpload = new FrmCoupleStoneUpload();
                    FrmCoupleStoneUpload.MdiParent = Global.gMainRef;
                    FrmCoupleStoneUpload.ShowForm(Val.ToInt32(GrdDetail.GetRowCellValue(e.RowHandle, "BOX_ID")), Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "BOXNAME")));                  
                }

                if (e.Clicks == 2 && Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("LOTNO"))
                {
                    FrmBoxMaster FrmBoxMaster = new FrmBoxMaster();
                    FrmBoxMaster.MdiParent = Global.gMainRef;
                    FrmBoxMaster.ShowForm(Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "BOXNAME")));
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }


        #region KeyPress
       
        private void txtLot_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "LOT_ID,LOTNAME,LOTCODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mStrColumnsToHide = "LOT_ID";
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_LOT);
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtLotName.Tag = Val.ToString(FrmSearch.DRow["LOT_ID"]);
                        txtLotName.Text = Val.ToString(FrmSearch.DRow["LOTNAME"]);
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

        private void txtClarity_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "CLARITY_ID,CLARITYNAME,CLARITYCODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mStrColumnsToHide = "CLARITY_ID";
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_CLARITY);
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtPurityName.Tag = Val.ToString(FrmSearch.DRow["CLARITY_ID"]);
                        txtPurityName.Text = Val.ToString(FrmSearch.DRow["CLARITYNAME"]);
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

        private void txtColor_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "COLOR_ID,COLORNAME,COLORCODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mStrColumnsToHide = "COLOR_ID";
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_COLOR);
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtColorName.Tag = Val.ToString(FrmSearch.DRow["COLOR_ID"]);
                        txtColorName.Text = Val.ToString(FrmSearch.DRow["COLORNAME"]);
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
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "SIZE_ID,SIZENAME,SIZECODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mStrColumnsToHide = "SIZE_ID";
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_SIZE);
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtSizeName.Tag = Val.ToString(FrmSearch.DRow["SIZE_ID"]);
                        txtSizeName.Text = Val.ToString(FrmSearch.DRow["SIZENAME"]);
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
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "SHAPE_ID,SHAPENAME,SHAPECODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mStrColumnsToHide = "SHAPE_ID";
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_SHAPE);
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtShapeName.Tag = Val.ToString(FrmSearch.DRow["SHAPE_ID"]);
                        txtShapeName.Text = Val.ToString(FrmSearch.DRow["SHAPENAME"]);
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
        #endregion

        private void txtBoxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "BOX_ID,BOXNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mStrColumnsToHide = "CUTNO,BOX_ID";
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_BOXNAME);
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBoxName.Tag = Val.ToString(FrmSearch.DRow["BOX_ID"]);
                        txtBoxName.Text = Val.ToString(FrmSearch.DRow["BOXNAME"]);
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

        private void txtCutNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "CUTNO";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mStrColumnsToHide = "BOXNAME,BOX_ID";
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_BOXNAME);
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtCutNo.Text = Val.ToString(FrmSearch.DRow["CUTNO"]);
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

        private void BtnExport_Click_1(object sender, EventArgs e)
        {
            DataTable DTabDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);

            if (DTabDetail.Rows.Count <= 0)
            {
                this.Cursor = Cursors.Default;
                Global.Message("Please Select AtLeast One Record From The List.");
                return;
            }

            string StrStockNo = "";
            foreach (DataRow DR in DTabDetail.Rows)
            {
                StrStockNo = StrStockNo + DR["STOCKNO"] + ",";
            }

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
            if (ObjGridSelection != null)
            {
                ObjGridSelection.ClearSelection();
            }
        }

        public string ExportExcelNew(string StockNo)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                
                DTabSummary = ObjStock.ParcelLiveStock_Export(StockNo);

                this.Cursor = Cursors.Default;


                if (DTabSummary.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }


                this.Cursor = Cursors.WaitCursor;

                string StrFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + ".xlsx";

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



                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Sheet1");

                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabSummary.Columns.Count;


                    #region Stock Detail

                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].LoadFromDataTable(DTabSummary, true);
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

                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].AutoFitColumns();
                   
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

      
    }
}
