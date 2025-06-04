using MahantExport.Stock;
using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.Data;
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

namespace MahantExport.CRM
{
    public partial class FrmMatchingStock : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();

        DataTable DTabSummary = new DataTable();
        DataTable DTabDetail = new DataTable();
        DataTable DTabProcess = new DataTable();

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

        string mStrStockType = "ALL";
        

        #region Property Settings

        public FrmMatchingStock()
        {
            InitializeComponent();
        }

        public FORMTYPE mFormType = FORMTYPE.MEMOISSUE;
        public enum FORMTYPE
        {
            MEMOISSUE = 1,
            SALEINVOICE = 2,
            ORDERCONFIRM = 3,
            SEARCHCRITERIA = 4,
            DEMAND = 5,
            SERCHHISTORY = 6
        }

        public STOCKTYPE mStockType = STOCKTYPE.SINGLE;
        public enum STOCKTYPE
        {
            SINGLE = 1,
            PARCEL = 2,
            ALL = 3
        }
     
        public void ShowForm()
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();

            DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            DTPToDate.Value = DateTime.Now;

            DTPFromDate.Focus();
            DTabProcess = new BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_PROCESS);
            
            xtraTabActivity.TabPages.Clear();
            
            foreach (DataRow DRow in DTabProcess.Rows)
            {
                if (Val.ToString(DRow["PROCESS_ID"]) == "5" || Val.ToString(DRow["PROCESS_ID"]) == "6" || Val.ToString(DRow["PROCESS_ID"]) == "7" || Val.ToString(DRow["PROCESS_ID"]) == "8" || Val.ToString(DRow["PROCESS_ID"]) == "10" ||
                Val.ToString(DRow["PROCESS_ID"]) == "12" || Val.ToString(DRow["PROCESS_ID"]) == "13" || Val.ToString(DRow["PROCESS_ID"]) == "14" || Val.ToString(DRow["PROCESS_ID"]) == "15" || Val.ToString(DRow["PROCESS_ID"]) == "16" ||
                Val.ToString(DRow["PROCESS_ID"]) == "17" || Val.ToString(DRow["PROCESS_ID"]) == "18" || Val.ToString(DRow["PROCESS_ID"]) == "19" || Val.ToString(DRow["PROCESS_ID"]) == "20" || Val.ToString(DRow["PROCESS_ID"]) == "21" ||
                Val.ToString(DRow["PROCESS_ID"]) == "1" || Val.ToString(DRow["PROCESS_ID"]) == "2" || Val.ToString(DRow["PROCESS_ID"]) == "3")  //Remove Tav From Activity Tab
                    continue;

                DevExpress.XtraTab.XtraTabPage Page = new DevExpress.XtraTab.XtraTabPage();
                Page.Text = Val.ToString(DRow["PROCESSNAME"]);
                Page.Tag = Val.ToString(DRow["PROCESS_ID"]);

                DevExpress.XtraTab.XtraTabPage Page1 = new DevExpress.XtraTab.XtraTabPage();
                Page1.Text = "SEARCHCRITERIA";
                Page1.Tag = 22;
                
                xtraTabActivity.TabPages.Add(Page);
            }
            xtraTabActivity.SelectedTabPageIndex = 0;
        }

        public void ShowForm(FORMTYPE pFormType, bool IsActivityEditable, string pStockType)
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();

            mFormType = pFormType;
            mStrStockType = pStockType;
            
            DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            DTPToDate.Value = DateTime.Now;

            DTPFromDate.Focus();
            DTabProcess = new BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_PROCESS);

            int IntIndex = 0;
            int IntSelectedTagPage = 0;
            xtraTabActivity.TabPages.Clear();

            foreach (DataRow DRow in DTabProcess.Rows)
            {
                if (Val.ToString(DRow["PROCESS_ID"]) == "5" || Val.ToString(DRow["PROCESS_ID"]) == "6" || Val.ToString(DRow["PROCESS_ID"]) == "7" || Val.ToString(DRow["PROCESS_ID"]) == "8" || Val.ToString(DRow["PROCESS_ID"]) == "10" ||
                Val.ToString(DRow["PROCESS_ID"]) == "12" || Val.ToString(DRow["PROCESS_ID"]) == "13" || Val.ToString(DRow["PROCESS_ID"]) == "14" || Val.ToString(DRow["PROCESS_ID"]) == "15" || Val.ToString(DRow["PROCESS_ID"]) == "16" ||
                Val.ToString(DRow["PROCESS_ID"]) == "17" || Val.ToString(DRow["PROCESS_ID"]) == "18" || Val.ToString(DRow["PROCESS_ID"]) == "19" || Val.ToString(DRow["PROCESS_ID"]) == "20" || Val.ToString(DRow["PROCESS_ID"]) == "21" ||
                Val.ToString(DRow["PROCESS_ID"]) == "1" || Val.ToString(DRow["PROCESS_ID"]) == "2" || Val.ToString(DRow["PROCESS_ID"]) == "3")  //Remove Tav From Activity Tab
                    continue;

                DevExpress.XtraTab.XtraTabPage Page = new DevExpress.XtraTab.XtraTabPage();
                Page.Text = Val.ToString(DRow["PROCESSNAME"]);
                Page.Tag = Val.ToString(DRow["PROCESS_ID"]);

                xtraTabActivity.TabPages.Add(Page);

                if (mFormType == FORMTYPE.ORDERCONFIRM && Val.ToString(DRow["PROCESSNAME"]) == "ORDER CONFIRM")
                {
                    IntSelectedTagPage = IntIndex;
                }
                else if (mFormType == FORMTYPE.SALEINVOICE && Val.ToString(DRow["PROCESSNAME"]) == "SALES DELIVERY")
                {
                    IntSelectedTagPage = IntIndex;
                }
                if (mFormType == FORMTYPE.MEMOISSUE && Val.ToString(DRow["PROCESSNAME"]) == "MEMO ISSUE")
                {
                    IntSelectedTagPage = IntIndex;
                }
                else if (mFormType == FORMTYPE.SEARCHCRITERIA && Val.ToString(DRow["PROCESSNAME"]) == "SEARCHCRITERIA")
                {
                    IntSelectedTagPage = IntIndex;
                }
                IntIndex++;
            }

            xtraTabActivity.SelectedTabPageIndex = IntSelectedTagPage;

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

        }


        private void CmbActivity_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void GrdSummary_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                return;
            }
            //if (e.Clicks == 2 && e.Column.FieldName == "JANGEDNOSTR" && mFormType == FORMTYPE.PURCHASE)
            //{
            //    FrmPurchaseAPI FrmPurchaseAPI = new FrmPurchaseAPI();
            //    FrmPurchaseAPI.MdiParent = Global.gMainRef;
            //    FrmPurchaseAPI.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
            //    //HINA - START
            //    //FrmMemoEntry.ShowForm(Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "MEMO_ID")));
            //    FrmPurchaseAPI.ShowForm(Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "MEMO_ID")), mStrStockType);
            //    //HINA - END
            //}
            if (e.Clicks == 2 && e.Column.FieldName == "JANGEDNOSTR")
            {
                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                //HINA - START
                //FrmMemoEntry.ShowForm(Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "MEMO_ID")));
                FrmMemoEntry.ShowForm(Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "MEMO_ID")), mStrStockType);
                //HINA - END
            }


        }
        private void FrmMemoEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            BtnSearch.PerformClick();
        }

        private void BtnAddInvoice_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
            //    FrmMemoEntry.MdiParent = Global.gMainRef;
            //    FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
            //    if (mFormType == FORMTYPE.PURCHASE)
            //        //HINA - START
            //        //FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.PURCHASEISSUE, null);
            //        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.PURCHASEISSUE, null, mStrStockType);
            //    //HINA - END
            //    else if (mFormType == FORMTYPE.PURCHASERETURN)
            //        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.PURCHASERETURN, null);
            //    else if (mFormType == FORMTYPE.ORDERCONFIRM)
            //        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.ORDERCONFIRM, null);
            //    else if (mFormType == FORMTYPE.ORDERCONFIRMRETURN)
            //        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.ORDERCONFIRMRETURN, null);
            //    else if (mFormType == FORMTYPE.SALEINVOICE)
            //        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.SALEINVOICE, null);
            //    else if (mFormType == FORMTYPE.SALESDELIVERYRETURN)
            //        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.SALESDELIVERYRETURN, null);
            //}
            //catch (Exception ex)
            //{
            //    Global.Message(ex.Message.ToString());
            //}

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

        private void xtraTabActivity_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (xtraTabActivity.SelectedTabPage == null)
                {
                    return;
                }

                this.Text = xtraTabActivity.SelectedTabPage.Text;

                DTabSummary.Rows.Clear();
                DTabDetail.Rows.Clear();
                BtnSearch.PerformClick();
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.ToString());
            }
        }

        private void xtraTabActivity_SelectedPageChanged_1(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            try
            {
                if (xtraTabActivity.SelectedTabPage == null)
                {
                    return;
                }

                if (Val.ToString(xtraTabActivity.SelectedTabPage.Tag) == "4")
                {

                }

                else if (Val.ToString(xtraTabActivity.SelectedTabPage.Tag) == "11")
                {

                }
                else if (Val.ToString(xtraTabActivity.SelectedTabPage.Tag) == "9")
                {

                }
                this.Text = xtraTabActivity.SelectedTabPage.Text;

                DTabSummary.Rows.Clear();
                DTabDetail.Rows.Clear();
                BtnSearch.PerformClick();
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.ToString());
            }

        }

        private void ReptxtCustomerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle >= 0)
                    return;

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);
                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDet.SetFocusedRowCellValue("PARTYNAME", Val.ProperText(Val.ToString(FrmSearch.DRow["PARTYNAME"])));
                        GrdDet.SetFocusedRowCellValue("PARTY_ID", Val.ToString(FrmSearch.DRow["PARTY_ID"]));
                        GrdDet.SetFocusedRowCellValue("PARTYCODE", Val.ProperText(Val.ToString(FrmSearch.DRow["PARTYCODE"])));
                        GrdDet.SetFocusedRowCellValue("EMAILID", Val.ProperText(Val.ToString(FrmSearch.DRow["EMAILID"])));
                        lblCompany.Text = (Val.ToString(FrmSearch.DRow["COMPANYNAME"]));
                        lblMobileNo.Text = (Val.ToString(FrmSearch.DRow["MOBILENO"]));
                        lblCountry.Text = (Val.ToString(FrmSearch.DRow["SHIPPINGCOUNTRYNAME"]));

                        GrdDet.RefreshData();
                        DataRow Drow = GrdDet.GetFocusedDataRow();
                                              
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string StrFromDate = "";
                string StrToDate = "";

                MainGrdSummary.DataSource = null;
                MainGridDetail.DataSource = null;

                if (DTPFromDate.Checked)
                {
                    StrFromDate = Val.SqlDate(DTPFromDate.Text);
                }
                if (DTPToDate.Checked)
                {
                    StrToDate = Val.SqlDate(DTPToDate.Text);
                }


                DataSet DS = ObjMemo.GetMemoMatchingListData(
                  Val.ToInt(xtraTabActivity.SelectedTabPage.Tag),
                  StrFromDate,
                  StrToDate,
                  Val.ToString(mStrStockType));
               
                GrdSummary.BeginUpdate();
                GrdDetail.BeginUpdate();


                DTabSummary = DS.Tables[0];
                DTabDetail = DS.Tables[1];

                MainGrdSummary.DataSource = DTabSummary;

                MainGridDetail.DataSource = DTabDetail;
                MainGridDetail.Refresh();


                if (GrdDetail.GroupSummary.Count == 0)
                {
                    GrdDetail.GroupSummary.Add(SummaryItemType.Sum, "PCS", GrdDetail.Columns["PCS"]);
                    GrdDetail.GroupSummary.Add(SummaryItemType.Sum, "CARAT", GrdDetail.Columns["CARAT"]);
                    GrdDetail.GroupSummary.Add(SummaryItemType.Custom, "WEBSITERAPAPORT", GrdDetail.Columns["WEBSITERAPAPORT"]);
                    GrdDetail.GroupSummary.Add(SummaryItemType.Custom, "WEBSITEPRICEPERCARAT", GrdDetail.Columns["WEBSITEPRICEPERCARAT"]);
                    GrdDetail.GroupSummary.Add(SummaryItemType.Custom, "WEBSITEDISCOUNT", GrdDetail.Columns["WEBSITEDISCOUNT"]);
                    GrdDetail.GroupSummary.Add(SummaryItemType.Sum, "WEBSITEAMOUNT", GrdDetail.Columns["WEBSITEAMOUNT"]);

                    GrdDetail.GroupSummary.Add(SummaryItemType.Custom, "MEMORAPAPORT", GrdDetail.Columns["MEMORAPAPORT"]);
                    GrdDetail.GroupSummary.Add(SummaryItemType.Custom, "MEMOPRICEPERCARAT", GrdDetail.Columns["MEMOPRICEPERCARAT"]);
                    GrdDetail.GroupSummary.Add(SummaryItemType.Custom, "MEMODISCOUNT", GrdDetail.Columns["MEMODISCOUNT"]);
                    GrdDetail.GroupSummary.Add(SummaryItemType.Sum, "MEMOAMOUNT", GrdDetail.Columns["MEMOAMOUNT"]);

                    GrdDetail.GroupSummary.Add(SummaryItemType.Custom, "FMEMOPRICEPERCARAT", GrdDetail.Columns["FMEMOPRICEPERCARAT"]);
                    GrdDetail.GroupSummary.Add(SummaryItemType.Sum, "FMEMOAMOUNT", GrdDetail.Columns["FMEMOAMOUNT"]);

                    GrdDetail.GroupSummary.Add(SummaryItemType.Count, "STOCKNO", GrdDetail.Columns["STOCKNO"]);
                }

                if (DTabDetail.Rows.Count > 0)
                    GrdDetail.Columns["JANGEDNOSTR"].Group();

                GrdDetail.ExpandAllGroups();

                MainGridDetail.Refresh();
                MainGrdSummary.Refresh();

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
    }


}


