using MahantExport.Stock;
using MahantExport.Utility;
using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
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
using System.Windows.Forms;

namespace MahantExport.Account
{
    public partial class FrmBranchReceive : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();

        DataTable DTabSummary = new DataTable();
        DataTable DTabDetail = new DataTable();
        DataTable DTabParcelSummary = new DataTable();
        DataTable DTabParcelDetail = new DataTable();
        DataTable DtInvDetail = new DataTable();
        DataTable DTabProcess = new DataTable();

        BODevGridSelection Selection;
        BODevGridSelection selectionDetail;

        double DouCarat = 0;
        double DouWebSiteRap = 0;
        double DouWebSiteRapAmount = 0;
        double DouWebSitePricePerCarat = 0;
        double DouWebSiteAmount = 0;

        double DouMemoRap = 0;
        double DouMemoRapAmount = 0;
        double DouMemoPricePerCarat = 0;
        double DouMemoAmount = 0;
        double DouMemoAmountFE = 0;

        bool IsSearchClick = false;

        #region Property Settings

        public FrmBranchReceive()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();

            DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            DTPToDate.Value = DateTime.Now;


            DTPFromDate.Focus();

            ChkConsignment_CheckedChanged(null,null);

            CmbPaymentMode.SelectedIndex = 1;
            DataTable DtCompany = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_BRANCHCOMPANY);
            cmbCompany.DataSource = DtCompany;
            cmbCompany.ValueMember = "PARTY_ID";
            cmbCompany.DisplayMember = "PARTYNAME";

            if (DtCompany.Rows.Count > 0)
                cmbCompany.SelectedItem = DtCompany.Rows[0]["PARTYNAME"].ToString();
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
            ObjFormEvent.ObjToDisposeList.Add(ObjMemo);
        }

        #endregion


        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            //if (TabSummaryDetail.SelectedIndex == 0)
            //    Global.ExcelExport("Memo Summary", GrdSummary);
            //else
            //    Global.ExcelExport("Memo Detail", GrdDetail);
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable dtData = GetTableOfSelectedRows(GrdDetail, true, selectionDetail);
                if (dtData.Rows.Count <= 0)
                {
                    Global.Message("Please Select Records to Export");
                    return;
                }

                var list = dtData.AsEnumerable().Select(r => r["MEMODETAIL_ID"].ToString());
                string StrFilePath = ReportListExportExcel(string.Join(",", list));

                if (StrFilePath != "")
                {
                    if (Global.Confirm("Do You Want To Open File ? ") == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(StrFilePath, "CMD");
                    }
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
        public string ReportListExportExcel(string strMemoDetailIds, string StrFilePath = "")
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                //DataTable DTabDetail = ObjMemo.GetDataForSaleReport(strMemoDetailIds);
                DataTable DTabDetail = ObjMemo.GetDataForExport(strMemoDetailIds);


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
                    svDialog.FileName = "Ecport_" + DateTime.Now.ToString("dd-MM-yyyy") + ".xlsx";
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
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Sheet1");

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

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

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
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string StrFromDate = "";
                string StrToDate = "";
                string StrReceiveStatus = "";

                MainGrdSummary.DataSource = null;
                MainGridDetail.DataSource = null;

                if (DTPFromDate.Checked)
                {
                    StrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
                }
                if (DTPToDate.Checked)
                {
                    StrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());
                }

                if (RtbAll.Checked == true)
                {
                    StrReceiveStatus = "All";
                }
                else if (RtbPending.Checked == true)
                {
                    StrReceiveStatus = "Pending";
                }
                else if (RtbReceive.Checked == true)
                {
                    StrReceiveStatus = "Receive";
                }
                else if (RtbPickupPending.Checked == true)
                {
                    StrReceiveStatus = "PickupPending";
                }
                else if (RtbPickupDone.Checked == true)
                {
                    StrReceiveStatus = "PickupDone";
                }
                else if (RbtPickupReturn.Checked == true)//Gunjan : 11/03/2023
                {
                    StrReceiveStatus = "PickupReturn";
                }


                if (txtBillTo.Text.Length == 0) txtBillTo.Tag = null;
                if (txtSeller.Text.Length == 0) txtSeller.Tag = null;
                if (txtBuyer.Text.Length == 0) txtBuyer.Tag = null;

                int P_ID = 9;
                if (ChkConsignment.Checked == true)
                    P_ID = 26;
                else
                    P_ID = 9;

                Guid GstCompany_ID =  Guid.Parse(Val.ToString(cmbCompany.SelectedValue));
                DataSet DS = ObjMemo.BranchReceiveGetData(
                                                                P_ID,
                                                                StrFromDate,
                                                                StrToDate,
                                                                txtMemoNo.Text,
                                                                txtStoneNo.Text,
                                                                Val.ToString(txtBillTo.Tag),
                                                                Val.ToString(txtSeller.Tag),
                                                                "ALL",
                                                                "",
                                                                StrReceiveStatus,
                                                                txtReportNo.Text,
                                                                txtHKStoneNo.Text,
                                                                Val.ToString(txtBuyer.Tag),
                                                                GstCompany_ID
                                                              );

                GrdSummary.BeginUpdate();
                GrdDetail.BeginUpdate();

                DTabSummary = DS.Tables[0];
                DTabDetail = DS.Tables[1];

                MainGrdSummary.DataSource = DTabSummary;
                MainGrdSummary.Refresh();

                MainGridDetail.DataSource = DTabDetail;
                MainGridDetail.Refresh();

                //// FOR SINGLE SUMMURY SELECTION
                //if (MainGrdSummary.RepositoryItems.Count == 0)
                //{
                //    Selection = new BODevGridSelection();
                //    Selection.View = GrdSummary;
                //    Selection.ClearSelection();
                //    Selection.CheckMarkColumn.VisibleIndex = 1;
                //}
                //else
                //{
                //    Selection.ClearSelection();
                //}
                //GrdSummary.Columns["COLSELECTCHECKBOX"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                //if (Selection != null)
                //{
                //    Selection.ClearSelection();
                //    Selection.CheckMarkColumn.VisibleIndex = 1;
                //}
                //// END

                // FOR SINGLE DETAIL SELECTION
                if (MainGridDetail.RepositoryItems.Count == 1)
                {
                    selectionDetail = new BODevGridSelection();
                    selectionDetail.View = GrdDetail;
                    selectionDetail.ClearSelection();
                    selectionDetail.CheckMarkColumn.VisibleIndex = 1;
                }
                else
                {
                    selectionDetail.ClearSelection();
                }
                GrdDetail.Columns["COLSELECTCHECKBOX"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                if (selectionDetail != null)
                {
                    selectionDetail.ClearSelection();
                    selectionDetail.CheckMarkColumn.VisibleIndex = 1;
                }

                // FOR EXPAND SINGLE DETAIL GROUP IN GRID
                if (GrdDetail.GroupSummary.Count == 0)
                {
                    GrdDetail.GroupSummary.Add(SummaryItemType.Sum, "PCS", GrdDetail.Columns["PCS"]);
                    GrdDetail.GroupSummary.Add(SummaryItemType.Sum, "CARAT", GrdDetail.Columns["CARAT"]);
                    GrdDetail.GroupSummary.Add(SummaryItemType.Sum, "WEBSITERAPAPORT", GrdDetail.Columns["WEBSITERAPAPORT"]);
                    GrdDetail.GroupSummary.Add(SummaryItemType.Sum, "WEBSITEPRICEPERCARAT", GrdDetail.Columns["WEBSITEPRICEPERCARAT"]);
                    GrdDetail.GroupSummary.Add(SummaryItemType.Sum, "WEBSITEDISCOUNT", GrdDetail.Columns["WEBSITEDISCOUNT"]);
                    GrdDetail.GroupSummary.Add(SummaryItemType.Sum, "WEBSITEAMOUNT", GrdDetail.Columns["WEBSITEAMOUNT"]);

                    GrdDetail.GroupSummary.Add(SummaryItemType.Sum, "MEMORAPAPORT", GrdDetail.Columns["MEMORAPAPORT"]);
                    GrdDetail.GroupSummary.Add(SummaryItemType.Sum, "MEMOPRICEPERCARAT", GrdDetail.Columns["MEMOPRICEPERCARAT"]);
                    GrdDetail.GroupSummary.Add(SummaryItemType.Sum, "MEMODISCOUNT", GrdDetail.Columns["MEMODISCOUNT"]);
                    GrdDetail.GroupSummary.Add(SummaryItemType.Sum, "MEMOAMOUNT", GrdDetail.Columns["MEMOAMOUNT"]);

                    GrdDetail.GroupSummary.Add(SummaryItemType.Sum, "FMEMOPRICEPERCARAT", GrdDetail.Columns["FMEMOPRICEPERCARAT"]);
                    GrdDetail.GroupSummary.Add(SummaryItemType.Sum, "FMEMOAMOUNT", GrdDetail.Columns["FMEMOAMOUNT"]);

                    GrdDetail.GroupSummary.Add(SummaryItemType.Count, "PARTYSTOCKNO", GrdDetail.Columns["PARTYSTOCKNO"]);
                }

                //if (DTabDetail.Rows.Count > 0)
                //GrdDetail.Columns["JANGEDNOSTR"].Group();

                GrdDetail.ExpandAllGroups();
                // END

                GrdSummary.BestFitColumns();
                GrdDetail.BestFitColumns();

                GrdSummary.EndUpdate();
                GrdDetail.EndUpdate();

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnBestFit_Click(object sender, EventArgs e)
        {
            GrdSummary.BestFitColumns();
            GrdDetail.BestFitColumns();
        }

        private void GrdDetail_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {

            try
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    DouCarat = 0;
                    DouWebSiteRap = 0;
                    DouWebSiteRapAmount = 0;
                    DouWebSitePricePerCarat = 0;
                    DouWebSiteAmount = 0;

                    DouMemoRap = 0;
                    DouMemoRapAmount = 0;
                    DouMemoPricePerCarat = 0;
                    DouMemoAmount = 0;
                    DouMemoAmountFE = 0;


                }
                else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    double Cts = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT"));
                    DouCarat = DouCarat + Cts;
                    DouWebSiteRapAmount = DouWebSiteRapAmount + (Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "WEBSITERAPAPORT")) * Cts);
                    DouWebSiteAmount = DouWebSiteAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "WEBSITEAMOUNT"));

                    DouMemoRapAmount = DouMemoRapAmount + (Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "MEMORAPAPORT")) * Cts);
                    DouMemoAmount = DouMemoAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "MEMOAMOUNT"));

                }
                else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("WEBSITERAPAPORT") == 0)
                    {
                        e.TotalValue = Math.Round(DouWebSiteRapAmount / DouCarat, 2);
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("WEBSITEPRICEPERCARAT") == 0)
                    {
                        e.TotalValue = Math.Round(DouWebSiteAmount / DouCarat, 2);
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("WEBSITEDISCOUNT") == 0)
                    {
                        DouWebSitePricePerCarat = Math.Round(DouWebSiteAmount / DouCarat, 2);
                        DouWebSiteRap = Math.Round(DouWebSiteRapAmount / DouCarat, 2);
                        //e.TotalValue = Val.Format(Math.Round((100) - Val.Val(DouFilePricePerCarat) / Val.Val(DouFileRap) * (100), 2), "######0.000");
                        e.TotalValue = Math.Round(((DouWebSitePricePerCarat) - Val.Val(DouWebSiteRap)) / Val.Val(DouWebSiteRap) * (100), 2);
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MEMORAPAPORT") == 0)
                    {
                        e.TotalValue = Math.Round(DouMemoRapAmount / DouCarat, 2);
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MEMOPRICEPERCARAT") == 0)
                    {
                        e.TotalValue = Math.Round(DouMemoAmount / DouCarat, 2);
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MEMOPRICEPERCARATFE") == 0)
                    {
                        e.TotalValue = Math.Round(DouMemoAmountFE / DouCarat, 2);
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MEMODISCOUNT") == 0)
                    {
                        DouMemoPricePerCarat = Math.Round(DouMemoAmount / DouCarat, 2);
                        DouMemoRap = Math.Round(DouMemoRapAmount / DouCarat, 2);
                        //e.TotalValue = Val.Format(Math.Round((100) - Val.Val(DouFilePricePerCarat) / Val.Val(DouFileRap) * (100), 2), "######0.000");
                        e.TotalValue = Math.Round(((DouMemoPricePerCarat) - Val.Val(DouMemoRap)) / Val.Val(DouMemoRap) * (100), 2);
                    }

                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }

        }

        private void GrdDetail_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            //try
            //{
            //    if (e.RowHandle < 0)
            //    {
            //        return;
            //    }
            //    string StrStatus = Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "STATUS")).ToUpper();

            //    if (StrStatus == "CONFIRM")
            //    {
            //        e.Appearance.BackColor = lblUp.BackColor;
            //    }
            //    else if (StrStatus == "NOT CONFIRM")
            //    {
            //        e.Appearance.BackColor = lblDown.BackColor;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Global.Message(ex.Message.ToString());
            //}
            try
            {
                string StrDetailStatus = Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "STATUS"));
                string StrHk_PickupDate = Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "HK_PICKUPDATE"));
                if (StrHk_PickupDate != "")
                {
                    e.Appearance.BackColor = lblPickup.BackColor;
                    e.Appearance.BackColor2 = lblPickup.BackColor;
                }
                bool StrISREADYFORSOLDConsign = Val.ToBoolean(GrdDetail.GetRowCellValue(e.RowHandle, "ISREADYFORSOLD"));
                if (StrISREADYFORSOLDConsign == true)
                {
                    e.Appearance.BackColor = lblReadyForSoldColor.BackColor;
                    e.Appearance.BackColor2 = lblReadyForSoldColor.BackColor;
                }
                string StrBranchReceiveStatus = Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "BRANCHDELIVERYSTATUS"));
                if (StrBranchReceiveStatus == "PENDING")
                {
                    e.Appearance.BackColor = lblPending.BackColor;
                    e.Appearance.BackColor2 = lblPending.BackColor;
                }

                string StrPickup_ReturnDate = Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "Pickup_ReturnDate"));

                if (StrPickup_ReturnDate != "")
                {
                    e.Appearance.BackColor = lblPickupReturn.BackColor;
                    e.Appearance.BackColor2 = lblPickupReturn.BackColor;
                }

                //string StrBranchStatus = Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "STATUS"));
                //string StrBranchPICKUPDATE = Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "PICKUPDATE"));
                //if (StrBranchReceiveStatus == "RECEIVED" && StrBranchPICKUPDATE != "")
                ////if (StrBranchReceiveStatus == "RECEIVED" && StrBranchStatus == "ORDER CONFIRM" && StrBranchPICKUPDATE != "")
                //{
                //    e.Appearance.BackColor = lblPickup.BackColor;
                //    e.Appearance.BackColor2 = lblPickup.BackColor;
                //}
                //if (StrBranchReceiveStatus == "RECEIVED" && StrBranchPICKUPDATE == "")
                ////if (StrBranchReceiveStatus == "RECEIVED" && StrBranchStatus == "ORDER CONFIRM" && StrBranchPICKUPDATE == "")
                //{
                //    e.Appearance.BackColor = Color.White;
                //    e.Appearance.BackColor2 = Color.White;
                //}
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtSeller_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "EMPLOYEECODE,EMPLOYEENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_EMPLOYEE);

                    FrmSearch.mStrColumnsToHide = "EMPLOYEE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtSeller.Text = Val.ToString(FrmSearch.DRow["EMPLOYEENAME"]);
                        txtSeller.Tag = Val.ToString(FrmSearch.DRow["EMPLOYEE_ID"]);
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                    IsSearchClick = true;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtBillTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;

                    //FrmSearch.DTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.TABLE.MST_SALEPARTY);

                    DataTable DtabParty = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);
                    FrmSearch.mDTab = DtabParty;
                    FrmSearch.mStrColumnsToHide = "PARTY_ID,BILLINGCOUNTRY_ID,SHIPPINGCOUNTRY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBillTo.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtBillTo.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                    IsSearchClick = true;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void CmbActivity_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void FrmMemoEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            BtnSearch.PerformClick();
        }
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (GrdSummary.FocusedRowHandle < 0)
            {
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            DataTable DTab = ObjMemo.Print(Val.ToString(GrdSummary.GetFocusedRowCellValue("MEMO_ID")), "USD");
            if (DTab.Rows.Count == 0)
            {
                this.Cursor = Cursors.Default;
                Global.Message("There Is No Data Found For Print");
                return;
            }
            Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
            FrmReportViewer.MdiParent = Global.gMainRef;
            FrmReportViewer.ShowForm("InvoicePrintWithBack", DTab);
            this.Cursor = Cursors.Default;
        }

        private void RepUsdPrint_Click(object sender, EventArgs e)
        {
            if (GrdSummary.FocusedRowHandle < 0)
            {
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            DataTable DTab = ObjMemo.Print(Val.ToString(GrdSummary.GetFocusedRowCellValue("MEMO_ID")), "USD");
            if (DTab.Rows.Count == 0)
            {
                this.Cursor = Cursors.Default;
                Global.Message("There Is No Data Found For Print");
                return;
            }

            DataSet DS = new DataSet();
            DTab.TableName = "Table";
            DS.Tables.Add(DTab);
            DataTable DTabDuplicate = DTab.Copy();
            DTabDuplicate.TableName = "Table1";
            foreach (DataRow DRow in DTabDuplicate.Rows)
            {
                DRow["PRINTTYPE"] = "DUBLICATE";
            }
            DTabDuplicate.AcceptChanges();
            DS.Tables.Add(DTabDuplicate);

            Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
            FrmReportViewer.MdiParent = Global.gMainRef;
            FrmReportViewer.ShowFormMemoPrint("MemoPrint", DS);
            this.Cursor = Cursors.Default;
        }

        private void RepInrPrint_Click(object sender, EventArgs e)
        {
            if (GrdSummary.FocusedRowHandle < 0)
            {
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            DataTable DTab = ObjMemo.Print(Val.ToString(GrdSummary.GetFocusedRowCellValue("MEMO_ID")), "INR");
            if (DTab.Rows.Count == 0)
            {
                this.Cursor = Cursors.Default;
                Global.Message("There Is No Data Found For Print");
                return;
            }

            DataSet DS = new DataSet();
            DTab.TableName = "Table";
            DS.Tables.Add(DTab);
            DataTable DTabDuplicate = DTab.Copy();
            DTabDuplicate.TableName = "Table1";
            foreach (DataRow DRow in DTabDuplicate.Rows)
            {
                DRow["PRINTTYPE"] = "DUBLICATE";
            }
            DTabDuplicate.AcceptChanges();
            DS.Tables.Add(DTabDuplicate);

            Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
            FrmReportViewer.MdiParent = Global.gMainRef;
            FrmReportViewer.ShowFormMemoPrint("MemoPrint", DS);
            this.Cursor = Cursors.Default;
        }

        private void RspChkBxApproval_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (GrdSummary.FocusedRowHandle < 0)
                {
                    return;
                }

                GrdSummary.PostEditor();

                DataRow DRow = GrdSummary.GetFocusedDataRow();
                Int32 IntTFlag = Val.ToBooleanToInt(DRow["APPROVAL"]);
                MemoEntryProperty Property = new MemoEntryProperty();
                Property.MEMO_ID = Val.ToString(GrdSummary.GetRowCellValue(GrdSummary.FocusedRowHandle, "MEMO_ID"));
                Property = ObjMemo.UpdateSoApprovalFrmMemoList(Property, IntTFlag);
                GrdSummary.Focus();
            }
            catch (Exception ex)
            {
            }
        }

        private void GrdSummary_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }
                Int32 IntTFlag = Val.ToBooleanToInt(GrdSummary.GetRowCellValue(e.RowHandle, "APPROVAL"));
                if (IntTFlag == 1)
                {
                    e.Appearance.BackColor = Color.FromArgb(255, 192, 255);//192 Daksha
                    e.Appearance.BackColor2 = Color.FromArgb(255, 192, 255);
                }
                Int32 IntInvRecevFlag = Val.ToBooleanToInt(GrdSummary.GetRowCellValue(e.RowHandle, "INVRECEV"));
                if (IntInvRecevFlag == 1)
                {
                    e.Appearance.BackColor = Color.FromArgb(192, 255, 192);
                    e.Appearance.BackColor2 = Color.FromArgb(192, 255, 192);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void RepsChckInvRec_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (GrdSummary.FocusedRowHandle < 0)
                {
                    return;
                }

                GrdSummary.PostEditor();

                DataRow DRow = GrdSummary.GetFocusedDataRow();
                Int32 IntTFlag = Val.ToBooleanToInt(DRow["INVRECEV"]);
                MemoEntryProperty Property = new MemoEntryProperty();
                Property.MEMO_ID = Val.ToString(GrdSummary.GetRowCellValue(GrdSummary.FocusedRowHandle, "MEMO_ID"));
                Property = ObjMemo.UpdateInvRecevFrmMemoList(Property, IntTFlag);
                GrdSummary.Focus();
            }
            catch (Exception ex)
            {
            }
        }

        private void FrmMemoEntryParcel_FormClosing(object sender, FormClosingEventArgs e)
        {
            BtnSearch.PerformClick();
        }

        private void GrdParcelSummury_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                return;
            }


        }

        private void BtnAddDelivery_Click(object sender, EventArgs e)
        {
            try
            {
                DtInvDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, selectionDetail);
                if (DtInvDetail.Rows.Count <= 0)
                {
                    GrpBranchReceive.Visible = false;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                else
                {
                    if (DtInvDetail.Rows.Count > 0)
                    {
                        foreach (DataRow Dr in DtInvDetail.Rows)
                        {
                            string StrBranchReceiveStatus = Val.ToString(Dr["BRANCHDELIVERYSTATUS"]);
                            if (StrBranchReceiveStatus == "RECEIVED")
                            {
                                Global.Message("This Stone is Already Received From Branch, You can not Update Detail");
                                return;
                            }
                        }
                    }
                    GrpBranchReceive.Visible = true;
                    GrpBranchReceive.Enabled = true;
                    panel4.Enabled = false;
                    panel2.Enabled = false;
                    MainGrdSummary.Enabled = false;
                    MainGridDetail.Enabled = false;
                    txtComment.Focus();
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;

                Global.Message(ex.Message.ToString());
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string IntRes = "";

                if (DtInvDetail.Rows.Count > 0)
                {
                    if (Global.Confirm("Are You Sure For Goods Entry") == System.Windows.Forms.DialogResult.No)
                    {
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    foreach (DataRow Dr in DtInvDetail.Rows)
                    {
                        //Guid pGuidMemo_ID = Guid.Parse(Val.ToString(DtInvDetail.Rows[0]["MEMO_ID"]));
                        //Guid pGuidMemo_ID = Guid.Parse(Val.ToString(Dr["MEMO_ID"]));
                        Guid pGuidMemo_ID = Guid.Parse(Val.ToString(Dr["MEMODETAIL_ID"]));

                        Guid GstCompany_ID = Guid.Parse(Val.ToString(cmbCompany.SelectedValue));
                        IntRes = ObjMemo.BranchReceiveUpdate(pGuidMemo_ID, Val.ToString(txtComment.Text), Guid.Parse(Val.ToString(txtShippingParty.Tag)), GstCompany_ID);

                        if (ChkConsignment.Checked == false)
                        {
                            MemoEntryProperty Property = new MemoEntryProperty();
                            Property.FINYEAR = Global.GetFinancialYear(DateTime.Today.ToString());
                            Property.MEMO_ID = Val.ToString(pGuidMemo_ID);

                            Property = ObjMemo.BranchPurchaseSave(Property, "Add Mode", GstCompany_ID);
                        }

                    }

                    if (IntRes == "SUCCESS")
                    {

                        Global.Message("SUCCESSFULLY SAVED RIGHTS");
                        txtComment.Text = string.Empty;
                        btngrpClose_Click(null, null);
                        BtnSearch_Click(null, null);
                    }
                    else
                    {
                        Global.Message("OOPS SOMETHING GOES WRONG");
                        txtComment.Text = string.Empty;
                        btngrpClose_Click(null, null);
                        txtComment.Focus();
                        this.Cursor = Cursors.Default;
                        return;
                    }
                }
                this.Cursor = Cursors.Default;

            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
                return;
            }
        }

        private void GrdSummary_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                string StrBranchReceiveStatus = Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "BRANCHDELIVERYSTATUS"));
                if (StrBranchReceiveStatus == "PENDING")
                {
                    e.Appearance.BackColor = lblPending.BackColor;
                    e.Appearance.BackColor2 = lblPending.BackColor;
                }
                //string StrBranchStatus = Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "STATUS"));
                //string StrBranchPICKUPDATE = Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "PICKUPDATE"));
                string StrPickupStatus = Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "PickupStatus"));
                if (StrBranchReceiveStatus == "RECEIVED" && StrPickupStatus == "COMPLETE")//StrBranchPICKUPDATE != "")
                //if (StrBranchReceiveStatus == "RECEIVED" && StrBranchStatus == "ORDER CONFIRM" && StrBranchPICKUPDATE != "")
                {
                    e.Appearance.BackColor = lblPickup.BackColor;
                    e.Appearance.BackColor2 = lblPickup.BackColor;
                }
                if (StrBranchReceiveStatus == "RECEIVED" && (StrPickupStatus == "PARTIAL" || StrPickupStatus == "PENDING"))//StrBranchPICKUPDATE == "")
                //if (StrBranchReceiveStatus == "RECEIVED" && StrBranchStatus == "ORDER CONFIRM" && StrBranchPICKUPDATE == "")
                {
                    e.Appearance.BackColor = Color.White;
                    e.Appearance.BackColor2 = Color.White;
                }
                string StrPickup_ReturnStatus = Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "Pickup_ReturnStatus"));

                if (StrPickup_ReturnStatus == "RETURN")
                {
                    e.Appearance.BackColor = lblPickupReturn.BackColor;
                    e.Appearance.BackColor2 = lblPickupReturn.BackColor;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void btngrpClose_Click(object sender, EventArgs e)
        {
            GrpBranchReceive.Visible = false;
            panel4.Enabled = true;
            panel2.Enabled = true;
            MainGrdSummary.Enabled = true;
            MainGridDetail.Enabled = true;
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

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            GrpPickUpPaymentSelection.Visible = true;
            GrpPickUpPaymentSelection.Enabled = true;
            panel4.Enabled = false;
            panel2.Enabled = false;
            MainGrdSummary.Enabled = false;
            MainGridDetail.Enabled = false;
            CmbPickup.Focus();
        }

        private void GrdSummary_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataRow Drow = GrdSummary.GetFocusedDataRow();
                if (Drow != null)
                {
                    String StrSummuryInward = Val.ToString(Drow["JANGEDNOSTR"]);
                    bool ISSelcected = Val.ToBoolean(GrdSummary.GetFocusedRowCellValue("COLSELECTCHECKBOX"));

                    for (int i = 0; i < GrdDetail.RowCount; i++)
                    {
                        DataRow DrDetail = GrdDetail.GetDataRow(i);
                        if (DrDetail != null)
                        {
                            string StrDetailInward = DrDetail["JANGEDNOSTR"].ToString();

                            if (StrDetailInward == StrSummuryInward)
                            {
                                GrdDetail.SetRowCellValue(i, "COLSELECTCHECKBOX", ISSelcected);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrpBoxCloseButton_Click(object sender, EventArgs e)
        {
            GrpPickUpPaymentSelection.Visible = false;
            GrpPickUpPaymentSelection.Enabled = false;
            panel4.Enabled = true;
            panel2.Enabled = true;
            MainGrdSummary.Enabled = true;
            MainGridDetail.Enabled = true;
            CmbPaymentMode.SelectedIndex = 0;
            CmbPickup.SelectedIndex = -1;

            DTPFromDate.Focus();
        }

        private void FrmInvoiceEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            BtnSearch.PerformClick();
        }

        private void BtnPaymentSelectOk_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable DtInvDetail = GetTableOfSelectedRows(GrdDetail, true, selectionDetail);
                string StrStockType1 = "";

                if (DtInvDetail == null)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                if (DtInvDetail.Rows.Count >= 1)
                {
                    int IntIsSameParty = 0; string strHk_InvoiceNo = "", strStoneNo = "";
                    for (int i = 0; i < DtInvDetail.Rows.Count; i++)
                    {
                        string strBranchRecDate = DtInvDetail.Rows[i]["BRANCHAVAILABLEDATE"].ToString();
                        strHk_InvoiceNo = DtInvDetail.Rows[i]["HK_INVOICENOSTR"].ToString();
                        strStoneNo = DtInvDetail.Rows[i]["PARTYSTOCKNO"].ToString();

                        string StrBuyer1 = DtInvDetail.Rows[0]["FINALBUYER_ID"].ToString(), StrBuyer2 = DtInvDetail.Rows[i]["FINALBUYER_ID"].ToString();//Add By Gunjan :11/03/2023

                        if (strBranchRecDate == "")
                        {
                            Global.MessageError("This Entry Is Not In Branch Receive. First Receive It Then PickUp ....!");
                            this.Cursor = Cursors.Default;
                            return;
                        }

                        if (StrBuyer1 != StrBuyer2)// Add By Gunjan :11/03/2023
                        {
                            IntIsSameParty = 1;
                            break;
                        }

                        if (strHk_InvoiceNo != "")
                        {
                            Global.MessageError("This Stone No '" + strStoneNo + "' already Pickup....!");
                            this.Cursor = Cursors.Default;
                            return;
                        }
                    }
                    if (IntIsSameParty == 1)
                    {
                        Global.MessageError("Buyer Are Different, You Can't Confirm Pending Delivery...!");
                        this.Cursor = Cursors.Default;
                        return;
                    }
                }

                string IntRes = "";

                if (DtInvDetail.Rows.Count > 0)
                {
                    if (Global.Confirm("Are You Sure For Goods Entry") == System.Windows.Forms.DialogResult.No)
                    {
                        this.Cursor = Cursors.Default;
                        return;
                    }
                    foreach (DataRow Dr in DtInvDetail.Rows)
                    {
                        Guid pGuidStock_ID = Guid.Parse(Val.ToString(Dr["STOCK_ID"]));
                        Guid pGuidMemoDetail_ID = Guid.Parse(Val.ToString(Dr["MEMODETAIL_ID"]));
                        Guid GstCompany_ID = Guid.Parse(Val.ToString(cmbCompany.SelectedValue));
                        IntRes = ObjMemo.PickUpUpdate(pGuidStock_ID, pGuidMemoDetail_ID ,Val.ToString(txtComment.Text), GstCompany_ID);
                    }

                    if (IntRes == "SUCCESS")
                    {

                        Global.Message("SUCCESSFULLY SAVED RIGHTS");
                        txtComment.Text = string.Empty;
                        btngrpClose_Click(null, null);
                        BtnSearch_Click(null, null);
                    }
                    else
                    {
                        Global.Message("OOPS SOMETHING GOES WRONG");
                        txtComment.Text = string.Empty;
                        btngrpClose_Click(null, null);
                        txtComment.Focus();
                        this.Cursor = Cursors.Default;
                        return;
                    }
                }
                this.Cursor = Cursors.Default;

                GrpBoxCloseButton_Click(null, null);
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtStoneNo_TextChanged(object sender, EventArgs e)
        {
            String str1 = "";
            //Comment & Added by Daksha on 3/03/2023 B'coz some contain \t and some contains \n
            //Old
            //if (txtStoneNo.Text.Trim().Contains("\t\n"))
            //{
            //    str1 = txtStoneNo.Text.Trim().Replace("\t\n", ",");
            //}
            //else if (txtStoneNo.Text.Trim().Contains("\r"))
            //{
            //    str1 = txtStoneNo.Text.Trim().Replace("\r", "");
            //}
            //else
            //{
            //    str1 = txtStoneNo.Text.Trim().Replace("\n", ",");
            //}            
            str1 = txtStoneNo.Text.Trim().Replace("\t", ",").Replace("\n", ",").Replace(" ", "").Replace(",,", ",");
            //End as Daksha

            txtStoneNo.Text = str1;
            txtStoneNo.Select(txtStoneNo.Text.Length, 0);
            lblTotalCountStoneNo.Text = "(" + txtStoneNo.Text.Split(',').Length + ")";
        }

        private void txtMemoNo_TextChanged(object sender, EventArgs e)
        {
            String str1 = "";
            //Comment & Added by Daksha on 3/03/2023 B'coz some contain \t and some contains \n
            //Old   
            //if (txtMemoNo.Text.Trim().Contains("\t\n"))
            //{
            //    str1 = txtMemoNo.Text.Trim().Replace("\t\n", ",");
            //}
            //else if (txtMemoNo.Text.Trim().Contains("\r"))
            //{
            //    str1 = txtMemoNo.Text.Trim().Replace("\r", "");
            //}
            //else
            //{
            //    str1 = txtMemoNo.Text.Trim().Replace("\n", ",");
            //}                    
            str1 = txtMemoNo.Text.Trim().Replace("\t", ",").Replace("\n", ",").Replace(" ", "").Replace(",,", ",");
            //End as Daksha

            txtMemoNo.Text = str1;
            txtMemoNo.Select(txtMemoNo.Text.Length, 0);
            lblTotalCountInvoiceNo.Text = "(" + txtMemoNo.Text.Split(',').Length + ")";
        }

        private void txtReportNo_TextChanged(object sender, EventArgs e)
        {
            String str1 = "";
            //Comment & Added by Daksha on 3/03/2023 B'coz some contain \t and some contains \n
            //Old   
            //if (txtReportNo.Text.Trim().Contains("\t\n"))
            //{
            //    str1 = txtReportNo.Text.Trim().Replace("\t\n", ",");
            //}
            //else if (txtReportNo.Text.Trim().Contains("\r"))
            //{
            //    str1 = txtReportNo.Text.Trim().Replace("\r", "");
            //}
            //else
            //{
            //    str1 = txtReportNo.Text.Trim().Replace("\n", ",");
            //}                           
            str1 = txtReportNo.Text.Trim().Replace("\t", ",").Replace("\n", ",").Replace(" ", "").Replace(",,", ",");
            //End as Daksha

            txtReportNo.Text = str1;
            txtReportNo.Select(txtReportNo.Text.Length, 0);
            lblTotalCountReportNo.Text = "(" + txtReportNo.Text.Split(',').Length + ")";
        }

        private void GrdDetail_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            //try
            //{
            //    string StrBranchReceiveStatus = Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "BRANCHDELIVERYSTATUS"));
            //    if (StrBranchReceiveStatus == "PENDING")
            //    {
            //        e.Appearance.BackColor = lblPending.BackColor;
            //        e.Appearance.BackColor2 = lblPending.BackColor;
            //    }
            //    string StrBranchStatus = Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "STATUS"));
            //    string StrBranchPICKUPDATE = Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "PICKUPDATE"));
            //    if (StrBranchReceiveStatus == "RECEIVED" && StrBranchPICKUPDATE != "")
            //    //if (StrBranchReceiveStatus == "RECEIVED" && StrBranchStatus == "ORDER CONFIRM" && StrBranchPICKUPDATE != "")
            //    {
            //        e.Appearance.BackColor = lblPickup.BackColor;
            //        e.Appearance.BackColor2 = lblPickup.BackColor;
            //    }
            //    if (StrBranchReceiveStatus == "RECEIVED" && StrBranchPICKUPDATE == "")
            //    //if (StrBranchReceiveStatus == "RECEIVED" && StrBranchStatus == "ORDER CONFIRM" && StrBranchPICKUPDATE == "")
            //    {
            //        e.Appearance.BackColor = Color.White;
            //        e.Appearance.BackColor2 = Color.White;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Global.Message(ex.Message.ToString());
            //}        
        }

        private void GrdDetail_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                DataRow DrDetail = GrdDetail.GetFocusedDataRow();
                if (DrDetail != null)
                {
                    String StrDetail = Val.ToString(DrDetail["JANGEDNOSTR"]);
                    bool ISSelcected = Val.ToBoolean(GrdDetail.GetFocusedRowCellValue("COLSELECTCHECKBOX"));

                    for (int i = 0; i < GrdSummary.RowCount; i++)
                    {
                        DataRow Drow = GrdSummary.GetDataRow(i);
                        if (Drow != null)
                        {
                            string StrSummury = Drow["JANGEDNOSTR"].ToString();

                            if (StrSummury == StrDetail)
                            {
                                GrdSummary.SetRowCellValue(i, "COLSELECTCHECKBOX", ISSelcected);
                            }
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void lblSaveLayout_Click(object sender, EventArgs e)
        {
            try
            {
                int IntRes = 0;
                if (TabSummaryDetail.SelectedIndex == 0) //Summary
                {
                    Stream str = new System.IO.MemoryStream();
                    GrdSummary.SaveLayoutToStream(str);
                    str.Seek(0, System.IO.SeekOrigin.Begin);
                    StreamReader reader = new StreamReader(str);
                    string text = reader.ReadToEnd();

                    IntRes = new BOTRN_StockUpload().SaveGridLayout(this.Name, GrdSummary.Name, text);
                }
                else //Detail
                {
                    Stream str = new System.IO.MemoryStream();
                    GrdDetail.SaveLayoutToStream(str);
                    str.Seek(0, System.IO.SeekOrigin.Begin);
                    StreamReader reader = new StreamReader(str);
                    string text = reader.ReadToEnd();

                    IntRes = new BOTRN_StockUpload().SaveGridLayout(this.Name, GrdDetail.Name, text);
                }


                if (IntRes != -1)
                {
                    Global.Message("Layout Successfully Saved");
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void lblDefaultLayout_Click(object sender, EventArgs e)
        {
            try
            {
                int IntRes = 0;
                if (TabSummaryDetail.SelectedIndex == 0) //Summary
                {
                    IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdSummary.Name);
                }
                else
                {
                    IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdDetail.Name);
                }

                if (IntRes != -1)
                {
                    Global.Message("Layout Successfully Deleted");
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void FrmBranchReceive_Load(object sender, EventArgs e)
        {
            try
            {
                //Summary
                string StrSummary = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdSummary.Name);
                if (StrSummary != "")
                {
                    byte[] byteArray = Encoding.ASCII.GetBytes(StrSummary);
                    MemoryStream stream = new MemoryStream(byteArray);
                    GrdSummary.RestoreLayoutFromStream(stream);
                }

                //Detail 
                string StrDetail = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdDetail.Name);
                if (StrDetail != "")
                {
                    byte[] byteArray = Encoding.ASCII.GetBytes(StrDetail);
                    MemoryStream stream = new MemoryStream(byteArray);
                    GrdDetail.RestoreLayoutFromStream(stream);
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtHKStoneNo_TextChanged(object sender, EventArgs e)
        {
            String str1 = "";
            //Comment & Added by Daksha on 3/03/2023 B'coz some contain \t and some contains \n
            //Old                         
            //if (txtHKStoneNo.Text.Trim().Contains("\t\n"))
            //{
            //    str1 = txtHKStoneNo.Text.Trim().Replace("\t\n", ",");
            //}
            //else if (txtHKStoneNo.Text.Trim().Contains("\r"))
            //{
            //    str1 = txtHKStoneNo.Text.Trim().Replace("\r", "");
            //}
            //else
            //{
            //    str1 = txtHKStoneNo.Text.Trim().Replace("\n", ",");
            //}            
            str1 = txtHKStoneNo.Text.Trim().Replace("\t", ",").Replace("\n", ",").Replace(" ", "").Replace(",,", ",");
            //End as Daksha

            txtHKStoneNo.Text = str1;
            txtHKStoneNo.Select(txtHKStoneNo.Text.Length, 0);
            lblTotalCountHKStoneNo.Text = "(" + txtHKStoneNo.Text.Split(',').Length + ")";
        }

        private void GrdSummary_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                return;
            }

            if (e.Clicks == 2)
            {
                //Comment by Daksha on 18/03/2023 Bcoz table sorted record so fetch invalid Id
                //DataRow DR = DTabSummary.Rows[e.RowHandle];
                //string MEMO_ID = Val.ToString(DR["MEMO_ID"]).Trim();
                //End as Daksha
                string MEMO_ID = Val.ToString(GrdSummary.GetFocusedRowCellValue("MEMO_ID")); //Added by Daksha on 18/03/2023

                DataTable DTableNew = DTabDetail.Clone();
                DataRow[] rowsToCopy;
                rowsToCopy = DTabDetail.Select("MEMO_ID='" + MEMO_ID + "'");
                foreach (DataRow Row in rowsToCopy)
                {
                    DTableNew.ImportRow(Row);
                }

                MainGridDetail.DataSource = DTableNew;
                MainGridDetail.RefreshDataSource();

                TabSummaryDetail.SelectedIndex = 1;
            }
        }

        private void txtBuyer_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;

                    DataTable DtabParty = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PARTYALL);
                    FrmSearch.mDTab = DtabParty;
                    //FrmSearch.mStrColumnsToHide = "PARTY_ID,BILLINGCOUNTRY_ID,SHIPPINGCOUNTRY_ID";
                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBuyer.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtBuyer.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                    IsSearchClick = true;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtBillTo_Validated(object sender, EventArgs e)
        {
            try
            {
                if (IsSearchClick == true)
                {
                    BtnSearch_Click(sender, e);
                    IsSearchClick = false;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtStoneNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    IsSearchClick = true;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtMemoNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    IsSearchClick = true;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtHKStoneNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    IsSearchClick = true;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtReportNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    IsSearchClick = true;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            DTPToDate.Value = DateTime.Now;
            txtBillTo.Text = string.Empty;
            txtSeller.Text = string.Empty;
            txtBuyer.Text = string.Empty;
            txtStoneNo.Text = string.Empty;
            txtMemoNo.Text = string.Empty;
            txtHKStoneNo.Text = string.Empty;
            txtReportNo.Text = string.Empty;
            RtbAll.Checked = true;
            BtnSearch_Click(sender, e);
        }

        private void GrdDetail_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                if (e.Clicks == 2 && Val.ToString(GrdDetail.GetFocusedRowCellValue("HK_INVOICENOSTR")) != "" && Val.ToString(GrdDetail.GetFocusedRowCellValue("HKINVTYPE")) == "BANK")
                {
                    FrmInvoiceEntry FrmMemoEntry = new FrmInvoiceEntry();
                    FrmMemoEntry.MdiParent = Global.gMainRef;
                    FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                    FrmMemoEntry.ShowForm(Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "HKINVOICE_ID")));
                    //FrmMemoEntry.ShowForm(Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "MEMO_ID")), "ALL");
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void ChkConsignment_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkConsignment.Checked == true)
            {
                btnPickUp.Enabled = false;
                btnConsignmentConfirm.Enabled = true;
            }
            else
            {
                btnPickUp.Enabled = true;
                btnConsignmentConfirm.Enabled = false;
            }
        }

        private void btnConsignmentConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetTableOfSelectedRows(GrdDetail, true, selectionDetail);
                string IntRes = "";
                if (DtInvDetail == null)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                if (DtInvDetail.Rows.Count >= 1)
                {
                    int IntIsSameParty = 0; bool IsReadyForSold = false;
                    foreach (DataRow Dr in DtInvDetail.Rows)
                    {
                        IsReadyForSold = Val.ToBoolean(Val.ToString(Dr["ISREADYFORSOLD"]));
                        if (IsReadyForSold == true)
                        {
                            Global.Message("Already Stone Ready For Sold In Select List.");
                            this.Cursor = Cursors.Default;
                            return;
                        }
                    }
                    foreach (DataRow Dr in DtInvDetail.Rows)
                    {
                        Guid pGuidMemo_ID = Guid.Parse(Val.ToString(Dr["MEMO_ID"]));
                        Guid pGuidMemoDetail_ID = Guid.Parse(Val.ToString(Dr["MEMODETAIL_ID"]));
                        Guid pGuidStock_ID = Guid.Parse(Val.ToString(Dr["STOCK_ID"]));

                        IntRes = ObjMemo.BranchReceiveUpdateConsignConfirm(pGuidMemo_ID, pGuidMemoDetail_ID, pGuidStock_ID);
                    }
                    if (IntRes == "SUCCESS")
                    {

                        Global.Message("SUCCESSFULLY SAVED RECORDS");
                        BtnSearch_Click(null, null);
                    }
                    else
                    {
                        Global.Message("OOPS SOMETHING GOES WRONG");
                        DTPFromDate.Focus();
                        this.Cursor = Cursors.Default;
                        return;
                    }
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
                return;
            }
        }

        private void rTxtStoneCertiMfgMemo_TextChanged(object sender, EventArgs e)
        {
            String str1 = "";
            //Comment & Added by Daksha on 3/03/2023 B'coz some contain \t and some contains \n
            //Old  
            //if (txtReportNoFil.Text.Trim().Contains("\t\n"))
            //{
            //    str1 = txtReportNoFil.Text.Trim().Replace("\t\n", ",");
            //}
            //else if (txtReportNoFil.Text.Trim().Contains("\r"))
            //{
            //    str1 = txtReportNoFil.Text.Trim().Replace("\r", "");
            //}
            //else
            //{
            //    str1 = txtReportNoFil.Text.Trim().Replace("\n", ",");
            //}                    
            str1 = txtReportNoFil.Text.Trim().Replace("\t", ",").Replace("\n", ",").Replace(" ", "").Replace(",,", ",");
            //End as Daksha

            txtReportNoFil.Text = str1;
            txtReportNoFil.Select(txtReportNoFil.Text.Length, 0);
            lblStoneNoCount.Text = "(" + txtReportNoFil.Text.Split(',').Length + ")";
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string[] Al = txtReportNoFil.Text.Split(',');
                if (DTabDetail.Rows.Count == 0)
                {
                    Global.Message("Please Search Data");
                    return;
                }
                this.Cursor = Cursors.WaitCursor;

                for (int i = 0; i < Al.Length; i++)
                {
                    for (int IntI = 0; IntI < GrdDetail.RowCount; IntI++)
                    {
                        if (RbtStoneNo.Checked == true)
                        {
                            DataRow DRow = GrdDetail.GetDataRow(IntI);
                            if (Al[i] == Val.ToString(DRow["PARTYSTOCKNO"]).Trim()
                                )
                            {
                                GrdDetail.SetRowCellValue(IntI, "COLSELECTCHECKBOX", true);
                                //txtReportNoFil.Text = string.Empty;
                                //lblStoneNoCount.Text = "(0)";
                                //txtReportNoFil.Focus();                             
                                break;
                            }
                        }
                        if (rbtReportno.Checked == true)
                        {
                            DataRow DRow = GrdDetail.GetDataRow(IntI);
                            if (Al[i] == Val.ToString(DRow["LABREPORTNO"]).Trim()
                                )
                            {
                                GrdDetail.SetRowCellValue(IntI, "COLSELECTCHECKBOX", true);
                                //txtReportNoFil.Text = string.Empty;
                                //lblStoneNoCount.Text = "(0)";
                                //txtReportNoFil.Focus();                          
                                break;
                            }
                        }
                    }
                }

                GrdDetail.Columns["COLSELECTCHECKBOX"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
                GrdDetail.Columns["COLSELECTCHECKBOX"].SortMode = DevExpress.XtraGrid.ColumnSortMode.Value;
                GrdDetail.RefreshData();

                txtReportNoFil.Text = string.Empty;
                lblStoneNoCount.Text = "(0)";
                txtReportNoFil.Focus();

                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void RtbAll_CheckedChanged(object sender, EventArgs e)
        {
            BtnSearch_Click(null, null);
        }

        private void RtbPending_CheckedChanged(object sender, EventArgs e)
        {
            BtnSearch_Click(null, null);
        }

        private void RtbReceive_CheckedChanged(object sender, EventArgs e)
        {
            BtnSearch_Click(null, null);
        }

        private void RtbPickupPending_CheckedChanged(object sender, EventArgs e)
        {
            BtnSearch_Click(null, null);
        }

        private void RtbPickupDone_CheckedChanged(object sender, EventArgs e)
        {
            BtnSearch_Click(null, null);
        }

        private void BtnPickupReturn_Click(object sender, EventArgs e)//Add By Gunjan:11/03/2023
        {
            try
            {
                DataTable DtInvDetail = GetTableOfSelectedRows(GrdDetail, true, selectionDetail);
                if (DtInvDetail == null)
                {
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                if (DtInvDetail.Rows.Count <= 0)
                {
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                else
                {
                    if (DtInvDetail.Rows.Count > 0)
                    {
                        foreach (DataRow Dr in DtInvDetail.Rows)
                        {
                            string StrPickup_ReturnDate = Val.ToString(Dr["Pickup_ReturnDate"]);

                            if (StrPickup_ReturnDate != "")
                            {
                                Global.Message("This Stone is Already in Pickup Return");
                                return;
                            }
                        }
                    }
                }
                GrpPickupReturn.Visible = true;
                GrpPickupReturn.Enabled = true;
                panel4.Enabled = false;
                panel2.Enabled = false;
                MainGrdSummary.Enabled = false;
                MainGridDetail.Enabled = false;
                CmbPickup.Focus();

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void BtnPickupSave_Click(object sender, EventArgs e)//Add By Gunjan:11/03/2023
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string IntReturn = "";
                DataTable DtInvDetail = GetTableOfSelectedRows(GrdDetail, true, selectionDetail);

                if (DtInvDetail.Rows.Count > 0)
                {
                    if (Global.Confirm("Are You Sure For Goods Entry") == System.Windows.Forms.DialogResult.No)
                    {
                        this.Cursor = Cursors.Default;
                        return;
                    }


                    foreach (DataRow Dr in DtInvDetail.Rows)
                    {
                        Guid pGuidMemoDetail_ID = Guid.Parse(Val.ToString(Dr["MEMODETAIL_ID"]));

                        IntReturn = ObjMemo.PickupReturnUpdate(pGuidMemoDetail_ID, Val.ToString(txtPickupRemark.Text));
                    }

                    if (IntReturn == "SUCCESS")
                    {

                        Global.Message("SUCCESSFULLY SAVED RIGHTS");
                        txtPickupRemark.Text = string.Empty;
                        btnPickupClose_Click(null, null);
                        BtnSearch_Click(null, null);
                    }
                    else
                    {
                        Global.Message("OOPS SOMETHING GOES WRONG");
                        txtComment.Text = string.Empty;
                        btnPickupClose_Click(null, null);
                        txtPickupRemark.Focus();
                        this.Cursor = Cursors.Default;
                        return;
                    }
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void btnPickupClose_Click(object sender, EventArgs e)//Add By Gunjan:11/03/2023
        {
            GrpPickupReturn.Visible = false;
            panel4.Enabled = true;
            panel2.Enabled = true;
            MainGrdSummary.Enabled = true;
            MainGridDetail.Enabled = true;
        }

        private void RbtPickupReturn_CheckedChanged(object sender, EventArgs e)
        {
            BtnSearch_Click(null, null);
        }

        private void CmbPickup_SelectedIndexChanged(object sender, EventArgs e)
        {
            BtnPaymentSelectOk_Click(null, null);
        }

        private void txtShippingParty_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_BRANCHCOMPANY);

                    FrmSearch.mStrColumnsToHide = "PARTY_ID,BILLINGCOUNTRY_ID,SHIPPINGCOUNTRY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtShippingParty.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtShippingParty.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
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

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}