using BusLib;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.Data;
using MahantExport.Stock;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using BusLib.Account;
using BusLib.Configuration;
using System.IO;
using OfficeOpenXml;
using System.Text;
using DevExpress.XtraVerticalGrid.Rows;
using MahantExport.Utility;
using DevExpress.XtraGrid.Columns;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;
using System.Linq;
using System.Text.RegularExpressions;

namespace MahantExport.Masters
{
    public partial class FrmMemoList : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();
        //BusLib.BOFormPermission ObjPermission = new BOFormPermission();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        //DevExpress selection;

        private bool isPasteAction = false;
        private const Keys PasteKeys = Keys.Control | Keys.V;

        DataTable DTabSummary = new DataTable();
        DataTable DTabDetail = new DataTable();
        DataTable DTabDetailParcel = new DataTable();
        DataTable DTabProcess = new DataTable();
        BODevGridSelection ObjGridSelection;

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

        //HINA - START
        string mStrStockType = "ALL";
        //HINA - END

        int FromData = 0, ToData = 0;
        string TrnOldFromData = "", TrnOldToData = "";
        int YearId = 0;

        string MenuTagName = "";
        string mStrStockNo = "";
        string StrKapan = "";
        string StrFilePath = "";
        string pStrBillingPartyID = "";
        string pStrFromDate = "";
        string pStrToDate = "";
        string pStrMemoNo = "";
        string pStrSellerID = "";
        string StrHKStoneType = "";

        Color mSelectedColor = Color.FromArgb(192, 0, 0);
        Color mDeSelectColor = Color.Black;
        Color mSelectedBackColor = Color.FromArgb(255, 224, 192);
        Color mDSelectedBackColor = Color.White;

        DataTable DTabDiamondType = new DataTable();

        #region Property Settings

        public FrmMemoList()
        {
            InitializeComponent();
        }

        //Add : Pinali : 27-08-2019
        public FORMTYPE mFormType = FORMTYPE.SALEINVOICE;
        public enum FORMTYPE
        {
            PURCHASE = 1,
            PURCHASERETURN = 2,
            MEMOISSUE = 3,
            MEMORETURN = 4,
            SALEINVOICE = 5,
            SALESDELIVERYRETURN = 6,
            HOLD = 7,
            RELEASE = 8,
            ORDERCONFIRM = 9,
            ORDERCONFIRMRETURN = 10,
            ONLINE = 11,
            OFFLINE = 12
        }

        //HINA - START
        public STOCKTYPE mStockType = STOCKTYPE.SINGLE;
        public enum STOCKTYPE
        {
            SINGLE = 1,
            PARCEL = 2,
            ALL = 3
        }
        //HINA - END

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
                if (Val.ToString(DRow["PROCESS_ID"]) == "12" || Val.ToString(DRow["PROCESS_ID"]) == "1" || Val.ToString(DRow["PROCESS_ID"]) == "14")  //None,RapnetPrice,TransferToAvailable Remove from Activity
                    continue;

                DevExpress.XtraTab.XtraTabPage Page = new DevExpress.XtraTab.XtraTabPage();
                Page.Text = Val.ToString(DRow["PROCESSNAME"]);
                Page.Tag = Val.ToString(DRow["PROCESS_ID"]);

                xtraTabActivity.TabPages.Add(Page);
            }
            xtraTabActivity.SelectedTabPageIndex = 0;
            BtnSearch_Click(null, null);
            BtnAddInvoice.Visible = false;

        }
        //HINA - START
        //public void ShowForm(FORMTYPE pFormType,bool IsActivityEditable) //Add: Pinali : 30-08-2018
        public void ShowForm(FORMTYPE pFormType, bool IsActivityEditable, string pStockType)
        //HINA - END
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();

            mFormType = pFormType;
            //HINA - START
            mStrStockType = pStockType;
            //mStrStockType = (mFormType == FORMTYPE.PURCHASE & mStockType == STOCKTYPE.SINGLE) ? "SINGLE" : (mFormType == FORMTYPE.PURCHASE & mStockType == STOCKTYPE.PARCEL ? "PARCEL" : "ALL");
            //HINA - END
            DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            DTPToDate.Value = DateTime.Now;
            DTPFromDate.Focus();
            DTabProcess = new BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_PROCESS);

            int IntIndex = 0;
            int IntSelectedTagPage = 0;
            xtraTabActivity.TabPages.Clear();
            foreach (DataRow DRow in DTabProcess.Rows)
            {
                if (Val.ToString(DRow["PROCESS_ID"]) == "12" || Val.ToString(DRow["PROCESS_ID"]) == "1" || Val.ToString(DRow["PROCESS_ID"]) == "14")  //None,RapnetPrice,TransferToAvailable Remove from Activity
                    continue;

                DevExpress.XtraTab.XtraTabPage Page = new DevExpress.XtraTab.XtraTabPage();
                Page.Text = Val.ToString(DRow["PROCESSNAME"]);
                Page.Tag = Val.ToString(DRow["PROCESS_ID"]);

                xtraTabActivity.TabPages.Add(Page);

                if (mFormType == FORMTYPE.PURCHASE && Val.ToString(DRow["PROCESSNAME"]) == "PURCHASE")
                {
                    IntSelectedTagPage = IntIndex;
                }
                else if (mFormType == FORMTYPE.PURCHASERETURN && Val.ToString(DRow["PROCESSNAME"]) == "PURCHASE RETURN")
                {
                    IntSelectedTagPage = IntIndex;
                }
                else if (mFormType == FORMTYPE.ORDERCONFIRM && Val.ToString(DRow["PROCESSNAME"]) == "ORDER CONFIRM")
                {
                    IntSelectedTagPage = IntIndex;
                }
                else if (mFormType == FORMTYPE.ORDERCONFIRMRETURN && Val.ToString(DRow["PROCESSNAME"]) == "ORDER CONFIRM RETURN")
                {
                    IntSelectedTagPage = IntIndex;
                }
                else if (mFormType == FORMTYPE.SALEINVOICE && Val.ToString(DRow["PROCESSNAME"]) == "SALES DELIVERY")
                {
                    IntSelectedTagPage = IntIndex;
                }
                else if (mFormType == FORMTYPE.SALESDELIVERYRETURN && Val.ToString(DRow["PROCESSNAME"]) == "SALES DELIVERY RETURN")
                {
                    IntSelectedTagPage = IntIndex;
                }
                else if (mFormType == FORMTYPE.MEMOISSUE && Val.ToString(DRow["PROCESSNAME"]) == "MEMO ISSUE")
                {
                    IntSelectedTagPage = IntIndex;
                }
                else if (mFormType == FORMTYPE.MEMORETURN && Val.ToString(DRow["PROCESSNAME"]) == "MEMO RETURN")
                {
                    IntSelectedTagPage = IntIndex;
                }
                IntIndex++;
            }
            xtraTabActivity.SelectedTabPageIndex = IntSelectedTagPage;
            BtnAddInvoice.Text = "Make " + Val.ProperText(xtraTabActivity.SelectedTabPage.Text);
            BtnAddInvoice.Visible = true;
        }

        public void ShowForm(FORMTYPE pFormType, bool IsActivityEditable, string pStockType, string pStrMenuTag)
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();

            mFormType = pFormType;
            //HINA - START
            mStrStockType = pStockType;
            MenuTagName = pStrMenuTag;
            //mStrStockType = (mFormType == FORMTYPE.PURCHASE & mStockType == STOCKTYPE.SINGLE) ? "SINGLE" : (mFormType == FORMTYPE.PURCHASE & mStockType == STOCKTYPE.PARCEL ? "PARCEL" : "ALL");
            //HINA - END
            DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            DTPToDate.Value = DateTime.Now;

            this.Text = "Cash Invoice List";

            DTPFromDate.Focus();
            DTabProcess = new BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_PROCESS);

            int IntIndex = 0;
            int IntSelectedTagPage = 0;
            xtraTabActivity.TabPages.Clear();
            foreach (DataRow DRow in DTabProcess.Rows)
            {
                if (Val.ToString(DRow["PROCESS_ID"]) == "12" || Val.ToString(DRow["PROCESS_ID"]) == "1" || Val.ToString(DRow["PROCESS_ID"]) == "14")  //None,RapnetPrice,TransferToAvailable Remove from Activity
                    continue;

                DevExpress.XtraTab.XtraTabPage Page = new DevExpress.XtraTab.XtraTabPage();
                Page.Text = Val.ToString(DRow["PROCESSNAME"]);
                Page.Tag = Val.ToString(DRow["PROCESS_ID"]);

                xtraTabActivity.TabPages.Add(Page);

                if (mFormType == FORMTYPE.PURCHASE && Val.ToString(DRow["PROCESSNAME"]) == "PURCHASE")
                {
                    IntSelectedTagPage = IntIndex;
                }
                else if (mFormType == FORMTYPE.PURCHASERETURN && Val.ToString(DRow["PROCESSNAME"]) == "PURCHASE RETURN")
                {
                    IntSelectedTagPage = IntIndex;
                }
                else if (mFormType == FORMTYPE.ORDERCONFIRM && Val.ToString(DRow["PROCESSNAME"]) == "ORDER CONFIRM")
                {
                    IntSelectedTagPage = IntIndex;
                }
                else if (mFormType == FORMTYPE.ORDERCONFIRMRETURN && Val.ToString(DRow["PROCESSNAME"]) == "ORDER CONFIRM RETURN")
                {
                    IntSelectedTagPage = IntIndex;
                }
                else if (mFormType == FORMTYPE.SALEINVOICE && Val.ToString(DRow["PROCESSNAME"]) == "SALES DELIVERY")
                {
                    IntSelectedTagPage = IntIndex;
                }
                else if (mFormType == FORMTYPE.SALESDELIVERYRETURN && Val.ToString(DRow["PROCESSNAME"]) == "SALES DELIVERY RETURN")
                {
                    IntSelectedTagPage = IntIndex;
                }
                else if (mFormType == FORMTYPE.MEMOISSUE && Val.ToString(DRow["PROCESSNAME"]) == "MEMO ISSUE")
                {
                    IntSelectedTagPage = IntIndex;
                }
                else if (mFormType == FORMTYPE.MEMORETURN && Val.ToString(DRow["PROCESSNAME"]) == "MEMO RETURN")
                {
                    IntSelectedTagPage = IntIndex;
                }
                IntIndex++;
            }

            xtraTabActivity.SelectedTabPageIndex = IntSelectedTagPage;

            BtnAddInvoice.Text = "Make " + Val.ProperText(xtraTabActivity.SelectedTabPage.Text);
            BtnAddInvoice.Visible = true;

            if (MenuTagName == "CASHINVOICES")
            {
                GrdSummary.Columns["FGROSSAMOUNT"].Visible = false;
                GrdSummary.Columns["FDISCOUNTAMOUNT"].Visible = false;
                GrdSummary.Columns["FINSURANCEAMOUNT"].Visible = false;
                GrdSummary.Columns["FSHIPPINGAMOUNT"].Visible = false;
                GrdSummary.Columns["FGSTAMOUNT"].Visible = false;
                GrdSummary.Columns["FNETAMOUNT"].Visible = false;
                GrdSummary.Columns["INR"].Visible = false;
                GrdSummary.Columns["CASHCHARGE"].Visible = true;
                this.tabPage3.Hide();
                this.TabSummaryDetail.TabPages.Remove(this.tabPage3);
            }
            else
            {
                GrdSummary.Columns["FGROSSAMOUNT"].Visible = true;
                GrdSummary.Columns["FDISCOUNTAMOUNT"].Visible = true;
                GrdSummary.Columns["FINSURANCEAMOUNT"].Visible = true;
                GrdSummary.Columns["FSHIPPINGAMOUNT"].Visible = true;
                GrdSummary.Columns["FGSTAMOUNT"].Visible = true;
                GrdSummary.Columns["FNETAMOUNT"].Visible = true;
                GrdSummary.Columns["INR"].Visible = true;
                GrdSummary.Columns["CASHCHARGE"].Visible = false;
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
            ObjFormEvent.ObjToDisposeList.Add(ObjMemo);
        }
        #endregion

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (TabSummaryDetail.SelectedIndex == 0)
                Global.ExcelExport("Memo Summary", GrdSummary);
            else
                Global.ExcelExport("Memo Detail", GrdDetail);
        }

        //private void ChangeFixedColumnStyle(object sender, EventArgs e)
        //{
        //    if (GrdDetail.Columns["COLSELECTCHECKBOX"].Fixed == DevExpress.XtraGrid.Columns.FixedStyle.Left)
        //        ;
        //    else
        //        GrdDetail.Columns[""].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
        //} 
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
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string StrFromDate = null;
                string StrToDate = null;
                string DIAMONDTYPE = "";

                MainGrdSummary.DataSource = null;
                MainGridDetail.DataSource = null;
                MainGridDetailParcel.DataSource = null;

                if (DTPFromDate.Checked)
                {
                    StrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
                }
                if (DTPToDate.Checked)
                {
                    StrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());
                }

                if (MainGridDetail.RepositoryItems.Count == 3)
                {
                    ObjGridSelection = new BODevGridSelection();
                    ObjGridSelection.View = GrdDetail;
                    ObjGridSelection.ISBoolApplicableForPageConcept = false;
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                    GrdDetail.Columns["COLSELECTCHECKBOX"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                }
                else
                {
                    ObjGridSelection.ClearSelection();
                }
                if (ObjGridSelection != null)
                {
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                }

                if (txtBillTo.Text.Length == 0) txtBillTo.Tag = null;
                if (txtShipTo.Text.Length == 0) txtShipTo.Tag = null;
                if (txtBillToCountry.Text.Length == 0) txtBillToCountry.Tag = string.Empty;
                if (txtShipToCountry.Text.Length == 0) txtShipToCountry.Tag = string.Empty;
                if (txtSeller.Text.Length == 0) txtSeller.Tag = null;
                if (txtBroker.Text.Length == 0) txtBroker.Tag = null;

                string StrStaus = "ALL";
                
                int IntOrderStatus = 0;
                if (RbOrderAll.Checked)
                {
                    IntOrderStatus = Val.ToInt(RbOrderAll.Tag);
                }
                else if (RbApproveOrder.Checked)
                {
                    IntOrderStatus = Val.ToInt(RbApproveOrder.Tag);
                }
                else if (RbNonApproveOrder.Checked)
                {
                    IntOrderStatus = Val.ToInt(RbNonApproveOrder.Tag);
                }

                FromData = 0; ToData = 0; TrnOldFromData = ""; TrnOldToData = "";              

                DIAMONDTYPE = GetSelectedBtnID(PanelDiamondType);
                if (DIAMONDTYPE.EndsWith(","))
                {
                    DIAMONDTYPE = DIAMONDTYPE.Substring(0, DIAMONDTYPE.Length - 1);
                }

                //GrdDetail.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;
                //  StrHKStoneType = Val.Trim(CmbHkStoneSearch1.Properties.GetCheckedItems());

                if (BOConfiguration.DEPTNAME == "ACCOUNT")
                {
                    YearId = BOConfiguration.FINYEAR_ID;
                }
                DataSet DS = ObjMemo.GetMemoListData(

                  Val.ToInt(xtraTabActivity.SelectedTabPage.Tag),
                  StrFromDate,
                  StrToDate,
                  txtMemoNo.Text,
                  txtStoneNo.Text,
                  Val.ToString(txtBillTo.Tag),
                  Val.ToInt(txtBillToCountry.Tag),
                  Val.ToString(txtShipTo.Tag),
                  Val.ToInt(txtShipToCountry.Tag),
                  Val.ToString(txtSeller.Tag), StrStaus, "", Val.ToString(mStrStockType),
                  false, IntOrderStatus, Val.ToString(txtBroker.Tag), 
                  FromData, ToData, TrnOldFromData, TrnOldToData, YearId, StrHKStoneType,"", DIAMONDTYPE, Val.ToBoolean(ChkShipmentPending.Checked), Val.ToString(txtShipPartyFilter.Tag));
                //HINA - END

                GrdSummary.BeginUpdate();
                GrdDetail.BeginUpdate();
                GrdDetailParcel.BeginUpdate();

                DTabSummary = DS.Tables[0];
                DTabDetail = DS.Tables[1];
                DTabDetailParcel = DS.Tables[2];

                //if (DTabDetail.Rows.Count <= 0)
                //{
                //    this.Cursor = Cursors.Default;
                //    return;
                //}

                MainGrdSummary.DataSource = DTabSummary;
                MainGridDetail.DataSource = DTabDetail;
                MainGridDetail.Refresh();
                MainGridDetailParcel.DataSource = DTabDetailParcel;
                MainGridDetailParcel.Refresh();

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

                if (GrdDetailParcel.GroupSummary.Count == 0)
                {
                    GrdDetailParcel.GroupSummary.Add(SummaryItemType.Count, "PCS", GrdDetailParcel.Columns["PCS"]);
                    GrdDetailParcel.GroupSummary.Add(SummaryItemType.Sum, "CARAT", GrdDetailParcel.Columns["CARAT"]);
                    GrdDetailParcel.GroupSummary.Add(SummaryItemType.Sum, "WEBSITERAPAPORT", GrdDetailParcel.Columns["WEBSITERAPAPORT"]);
                    GrdDetailParcel.GroupSummary.Add(SummaryItemType.Sum, "WEBSITEPRICEPERCARAT", GrdDetailParcel.Columns["WEBSITEPRICEPERCARAT"]);
                    GrdDetailParcel.GroupSummary.Add(SummaryItemType.Sum, "WEBSITEDISCOUNT", GrdDetailParcel.Columns["WEBSITEDISCOUNT"]);
                    GrdDetailParcel.GroupSummary.Add(SummaryItemType.Sum, "WEBSITEAMOUNT", GrdDetailParcel.Columns["WEBSITEAMOUNT"]);
                    GrdDetailParcel.GroupSummary.Add(SummaryItemType.Sum, "MEMORAPAPORT", GrdDetailParcel.Columns["MEMORAPAPORT"]);
                    GrdDetailParcel.GroupSummary.Add(SummaryItemType.Sum, "MEMOPRICEPERCARAT", GrdDetailParcel.Columns["MEMOPRICEPERCARAT"]);
                    GrdDetailParcel.GroupSummary.Add(SummaryItemType.Sum, "MEMODISCOUNT", GrdDetailParcel.Columns["MEMODISCOUNT"]);
                    GrdDetailParcel.GroupSummary.Add(SummaryItemType.Sum, "MEMOAMOUNT", GrdDetailParcel.Columns["MEMOAMOUNT"]);
                    GrdDetailParcel.GroupSummary.Add(SummaryItemType.Sum, "FMEMOPRICEPERCARAT", GrdDetailParcel.Columns["FMEMOPRICEPERCARAT"]);
                    GrdDetailParcel.GroupSummary.Add(SummaryItemType.Sum, "FMEMOAMOUNT", GrdDetailParcel.Columns["FMEMOAMOUNT"]);
                    GrdDetailParcel.GroupSummary.Add(SummaryItemType.Count, "STOCKNO", GrdDetailParcel.Columns["STOCKNO"]);
                }
               
                if (DTabDetail.Rows.Count > 0)
                    GrdDetail.Columns["JANGEDNOSTR"].Group();

                if (DTabDetailParcel.Rows.Count > 0)
                    GrdDetailParcel.Columns["JANGEDNOSTR"].Group();

                GrdDetail.ExpandAllGroups();
                GrdDetailParcel.ExpandAllGroups();

                MainGridDetail.Refresh();
                MainGrdSummary.Refresh();
                MainGridDetailParcel.Refresh();

                GrdSummary.BestFitColumns();
                GrdDetail.BestFitColumns();
                GrdDetailParcel.BestFitColumns();

                GrdSummary.EndUpdate();
                GrdDetail.EndUpdate();
                GrdDetailParcel.EndUpdate();

          //ChangeFixedColumnStyle(null,null);

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
                    if (mFormType == FORMTYPE.PURCHASE || mFormType == FORMTYPE.PURCHASERETURN)
                        FrmSearch.mDTab = DtabParty.Select("PARTYTYPE = 'PURCHASE'").CopyToDataTable();
                    else if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.SALESDELIVERYRETURN || mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.ORDERCONFIRMRETURN)
                        FrmSearch.mDTab = DtabParty.Select("PARTYTYPE = 'SALE'").CopyToDataTable();
                    else
                        FrmSearch.mDTab = DtabParty;

                    FrmSearch.mStrColumnsToHide = "PARTY_ID,BILLINGCOUNTRY_ID,SHIPPINGCOUNTRY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBillTo.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtBillTo.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
                        txtBillToCountry.Tag = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRY_ID"]);
                        txtBillToCountry.Text = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRYNAME"]);
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

        private void txtBillToCountry_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "COUNTRYCODE,COUNTRYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COUNTRY);

                    FrmSearch.mStrColumnsToHide = "COUNTRY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBillToCountry.Text = Val.ToString(FrmSearch.DRow["COUNTRYNAME"]);
                        txtBillToCountry.Tag = Val.ToString(FrmSearch.DRow["COUNTRY_ID"]);
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
        private void txtShipTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);

                    FrmSearch.mStrColumnsToHide = "PARTY_ID,BILLINGCOUNTRY_ID,SHIPPINGCOUNTRY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtShipTo.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtShipTo.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
                        txtShipToCountry.Tag = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRY_ID"]);
                        txtShipToCountry.Text = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRYNAME"]);
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
        private void txtShipToCountry_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "COUNTRYCODE,COUNTRYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COUNTRY);

                    FrmSearch.mStrColumnsToHide = "COUNTRY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtShipToCountry.Text = Val.ToString(FrmSearch.DRow["COUNTRYNAME"]);
                        txtShipToCountry.Tag = Val.ToString(FrmSearch.DRow["COUNTRY_ID"]);
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
                //if (Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "BRANCHRECEIVEDATE")) != "" && BOConfiguration.gStrLoginSection != "B")
                //{
                //    FrmMemoEntryBranch FrmMemoEntry = new FrmMemoEntryBranch();
                //    FrmMemoEntry.MdiParent = Global.gMainRef;
                //    FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                //    FrmMemoEntry.ShowForm(Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "MEMO_ID")), mStrStockType);
                //}
                //else
                {
                    if (MenuTagName == "CASHINVOICES" && Val.ToString(xtraTabActivity.SelectedTabPage.Tag) == "9" && BOConfiguration.gStrLoginSection != "B" && BOConfiguration.COMPANY_ID == Val.ToGuid("FE4C657D-5452-44D3-84F7-C8C71E20446E"))
                    {
                        FrmMemoEntryBranch FrmMemoEntryBranch = new FrmMemoEntryBranch();
                        FrmMemoEntryBranch.MdiParent = Global.gMainRef;
                        FrmMemoEntryBranch.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                        FrmMemoEntryBranch.ShowForm(Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "MEMO_ID")), mStrStockType);
                    }
                    else if(Val.ToString(xtraTabActivity.SelectedTabPage.Tag) == "9" && BOConfiguration.gStrLoginSection != "B")
                    {
                        string mStrStockType = "ALL";
                        FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                        FrmMemoEntry.MdiParent = Global.gMainRef;
                        FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                        FrmMemoEntry.ShowForm(Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "MEMO_ID")), mStrStockType);
                    }
                    else if (Val.ToString(xtraTabActivity.SelectedTabPage.Tag) == "2" && BOConfiguration.gStrLoginSection != "B" && BOConfiguration.COMPANY_ID == Val.ToGuid("FE4C657D-5452-44D3-84F7-C8C71E20446E"))
                    {
                        string mStrStockType = "ALL";
                        FrmMemoEntryBranch FrmMemoEntryBranch = new FrmMemoEntryBranch();
                        FrmMemoEntryBranch.MdiParent = Global.gMainRef;
                        FrmMemoEntryBranch.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                        FrmMemoEntryBranch.ShowForm(Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "MEMO_ID")), mStrStockType, "PurchaseHK");
                    }
                    else
                    {
                        FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                        FrmMemoEntry.MdiParent = Global.gMainRef;
                        FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                        FrmMemoEntry.ShowForm(Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "MEMO_ID")), mStrStockType);
                    }
                }

            }

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

        private void BtnAddInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                if (mFormType == FORMTYPE.PURCHASE)
                    //HINA - START
                    //FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.PURCHASEISSUE, null);
                    FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.PURCHASEISSUE, null, mStrStockType);
                //HINA - END
                else if (mFormType == FORMTYPE.PURCHASERETURN)
                    FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.PURCHASERETURN, null);
                else if (mFormType == FORMTYPE.ORDERCONFIRM)
                    FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.ORDERCONFIRM, null);
                else if (mFormType == FORMTYPE.ORDERCONFIRMRETURN)
                    FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.ORDERCONFIRMRETURN, null);
                else if (mFormType == FORMTYPE.SALEINVOICE)
                    FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.SALEINVOICE, null);
                else if (mFormType == FORMTYPE.SALESDELIVERYRETURN)
                    FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.SALESDELIVERYRETURN, null);
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }

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
                if (Val.ToString(xtraTabActivity.SelectedTabPage.Tag) == "2")
                {
                    DTPFromDate.Value = DateTime.Now.AddMonths(-1);
                    DTPToDate.Value = DateTime.Now;
                    lblSeller.Visible = false;
                    txtSeller.Visible = false;
                    pnlOrder.Visible = false;
                    BtnReportListExcelExpot.Visible = false;
                    GrdSummary.Columns["APPROVAL"].Visible = false;
                    GrdSummary.Columns["INVRECEV"].Visible = false;

                    GrdSummary.Columns["TRNNO_OLDDB"].Visible = false;
                    GrdDetail.Columns["TRNNO_OLDDB"].Visible = false;

                    GrdSummary.Columns["HKStone"].Visible = false;
                    GrdDetail.Columns["HKStone"].Visible = false;

                    PanelDiamondTypeFilter.Visible = true;//Gunjan:10/09/2024
                    BtnUpdateShippingParty.Visible = false;
                    BtnHKExport.Visible = false;

                    ChkShipmentPending.Visible = false;

                    this.tabPage3.Hide();
                    this.TabSummaryDetail.TabPages.Remove(this.tabPage3);
                }
                else if (Val.ToString(xtraTabActivity.SelectedTabPage.Tag) == "3")
                {
                    DTPFromDate.Value = DateTime.Now.AddMonths(-1);
                    DTPToDate.Value = DateTime.Now;
                    lblSeller.Visible = false;
                    txtSeller.Visible = false;
                    pnlOrder.Visible = false;
                    BtnReportListExcelExpot.Visible = false;
                    GrdSummary.Columns["APPROVAL"].Visible = false;
                    GrdSummary.Columns["INVRECEV"].Visible = false;
                    GrdSummary.Columns["TRNNO_OLDDB"].Visible = false;
                    GrdDetail.Columns["TRNNO_OLDDB"].Visible = false;


                    GrdSummary.Columns["HKStone"].Visible = false;
                    GrdDetail.Columns["HKStone"].Visible = false;

                    PanelDiamondTypeFilter.Visible = true;//Gunjan:10/09/2024
                    BtnUpdateShippingParty.Visible = false;
                    ChkShipmentPending.Visible = false;
                    BtnHKExport.Visible = false;

                    this.tabPage3.Hide();
                    this.TabSummaryDetail.TabPages.Remove(this.tabPage3);
                }
                else if (Val.ToString(xtraTabActivity.SelectedTabPage.Tag) == "4")
                {
                    DTPFromDate.Value = DateTime.Now.AddMonths(-1);
                    DTPToDate.Value = DateTime.Now;
                    lblSeller.Visible = true;
                    txtSeller.Visible = true;
                    pnlOrder.Visible = false;
                    BtnReportListExcelExpot.Visible = false;
                    GrdSummary.Columns["APPROVAL"].Visible = false;
                    GrdSummary.Columns["INVRECEV"].Visible = false;
                    GrdSummary.Columns["TRNNO_OLDDB"].Visible = false;
                    GrdDetail.Columns["TRNNO_OLDDB"].Visible = false;

                    ChkShipmentPending.Visible = false;
                    GrdSummary.Columns["HKStone"].Visible = false;
                    GrdDetail.Columns["HKStone"].Visible = false;

                    PanelDiamondTypeFilter.Visible = true;//Gunjan:10/09/2024
                    BtnUpdateShippingParty.Visible = false;
                    BtnHKExport.Visible = false;

                    this.tabPage3.Hide();
                    this.TabSummaryDetail.TabPages.Remove(this.tabPage3);
                }
                else if (Val.ToString(xtraTabActivity.SelectedTabPage.Tag) == "5")
                {
                    DTPFromDate.Value = DateTime.Now.AddMonths(-1);
                    DTPToDate.Value = DateTime.Now;
                    lblSeller.Visible = true;
                    txtSeller.Visible = true;
                    pnlOrder.Visible = false;
                    BtnReportListExcelExpot.Visible = false;
                    GrdSummary.Columns["APPROVAL"].Visible = false;
                    GrdSummary.Columns["INVRECEV"].Visible = false;
                    GrdSummary.Columns["TRNNO_OLDDB"].Visible = false;
                    GrdDetail.Columns["TRNNO_OLDDB"].Visible = false;

                    ChkShipmentPending.Visible = false;
                    GrdSummary.Columns["HKStone"].Visible = false;
                    GrdDetail.Columns["HKStone"].Visible = false;

                    PanelDiamondTypeFilter.Visible = true;//Gunjan:10/09/2024
                    BtnUpdateShippingParty.Visible = false;
                    BtnHKExport.Visible = false;

                    this.tabPage3.Hide();
                    this.TabSummaryDetail.TabPages.Remove(this.tabPage3);
                }
                else if (Val.ToString(xtraTabActivity.SelectedTabPage.Tag) == "6")
                {
                    DTPFromDate.Value = DateTime.Now.AddMonths(-1);
                    DTPToDate.Value = DateTime.Now;
                    lblSeller.Visible = true;
                    txtSeller.Visible = true;
                    pnlOrder.Visible = false;
                    BtnReportListExcelExpot.Visible = false;
                    GrdSummary.Columns["APPROVAL"].Visible = false;
                    GrdSummary.Columns["INVRECEV"].Visible = false;
                    GrdSummary.Columns["TRNNO_OLDDB"].Visible = false;
                    GrdDetail.Columns["TRNNO_OLDDB"].Visible = false;
                    ChkShipmentPending.Visible = false;
                    GrdSummary.Columns["HKStone"].Visible = false;
                    GrdDetail.Columns["HKStone"].Visible = false;
                    PanelDiamondTypeFilter.Visible = true;//Gunjan:10/09/2024
                    BtnUpdateShippingParty.Visible = false;
                    BtnHKExport.Visible = false;
                    this.tabPage3.Hide();
                    this.TabSummaryDetail.TabPages.Remove(this.tabPage3);
                }
                else if (Val.ToString(xtraTabActivity.SelectedTabPage.Tag) == "7")
                {
                    DTPFromDate.Value = DateTime.Now.AddMonths(-1);
                    DTPToDate.Value = DateTime.Now;
                    lblSeller.Visible = true;
                    txtSeller.Visible = true;
                    pnlOrder.Visible = false;
                    BtnReportListExcelExpot.Visible = false;
                    GrdSummary.Columns["APPROVAL"].Visible = false;
                    GrdSummary.Columns["INVRECEV"].Visible = false;
                    GrdSummary.Columns["TRNNO_OLDDB"].Visible = false;
                    GrdDetail.Columns["TRNNO_OLDDB"].Visible = false;
                    ChkShipmentPending.Visible = false;
                    GrdSummary.Columns["HKStone"].Visible = false;
                    GrdDetail.Columns["HKStone"].Visible = false;
                    PanelDiamondTypeFilter.Visible = true;//Gunjan:10/09/2024
                    BtnUpdateShippingParty.Visible = false;
                    BtnHKExport.Visible = false;
                    this.tabPage3.Hide();
                    this.TabSummaryDetail.TabPages.Remove(this.tabPage3);
                }
                else if (Val.ToString(xtraTabActivity.SelectedTabPage.Tag) == "9")
                {
                    DTPFromDate.Value = DateTime.Now;
                    DTPToDate.Value = DateTime.Now;
                    lblSeller.Visible = true;
                    txtSeller.Visible = true;
                    pnlOrder.Visible = false;
                    BtnReportListExcelExpot.Visible = false;
                    GrdSummary.Columns["APPROVAL"].Visible = false;
                    GrdSummary.Columns["INVRECEV"].Visible = true;
                    GrdSummary.Columns["INVRECEV"].VisibleIndex = 4;
                    GrdSummary.Columns["TRNNO_OLDDB"].Visible = true;
                    GrdDetail.Columns["TRNNO_OLDDB"].Visible = true;
                    ChkShipmentPending.Visible = true;
                    GrdSummary.Columns["HKStone"].Visible = false;
                    GrdDetail.Columns["HKStone"].Visible = false;
                    PanelDiamondTypeFilter.Visible = true;//Gunjan:10/09/2024
                    BtnUpdateShippingParty.Visible = true;
                    BtnHKExport.Visible = true;
                    this.tabPage3.Hide();
                    this.TabSummaryDetail.TabPages.Remove(this.tabPage3);
                }
                else if (Val.ToString(xtraTabActivity.SelectedTabPage.Tag) == "10")
                {
                    DTPFromDate.Value = DateTime.Now.AddMonths(-1);
                    DTPToDate.Value = DateTime.Now;
                    lblSeller.Visible = true;
                    txtSeller.Visible = true;
                    pnlOrder.Visible = false;
                    BtnReportListExcelExpot.Visible = false;
                    GrdSummary.Columns["APPROVAL"].Visible = false;
                    GrdSummary.Columns["INVRECEV"].Visible = false;
                    GrdSummary.Columns["TRNNO_OLDDB"].Visible = false;
                    GrdDetail.Columns["TRNNO_OLDDB"].Visible = false;
                    ChkShipmentPending.Visible = false;
                    GrdSummary.Columns["HKStone"].Visible = false;
                    GrdDetail.Columns["HKStone"].Visible = false;
                    PanelDiamondTypeFilter.Visible = true;//Gunjan:10/09/2024
                    BtnUpdateShippingParty.Visible = false;
                    BtnHKExport.Visible = false;
                    this.tabPage3.Hide();
                    this.TabSummaryDetail.TabPages.Remove(this.tabPage3);
                }
                else if (Val.ToString(xtraTabActivity.SelectedTabPage.Tag) == "11") //OrderConf
                {
                    DTPFromDate.Value = DateTime.Now.AddMonths(-1);
                    DTPToDate.Value = DateTime.Now;
                    lblSeller.Visible = true;
                    txtSeller.Visible = true;
                    pnlOrder.Visible = true;
                    BtnReportListExcelExpot.Visible = true;
                    GrdSummary.Columns["INVRECEV"].Visible = false;
                    GrdSummary.Columns["TRNNO_OLDDB"].Visible = true;
                    GrdDetail.Columns["TRNNO_OLDDB"].Visible = true;
                    BtnUpdateShippingParty.Visible = false;
                    BtnHKExport.Visible = false;
                    ChkShipmentPending.Visible = false;
                    GrdSummary.Columns["HKStone"].Visible = true;
                    GrdDetail.Columns["HKStone"].Visible = true;
                    PanelDiamondTypeFilter.Visible = true;//Gunjan:10/09/2024
                    this.tabPage3.Hide();
                    this.TabSummaryDetail.TabPages.Remove(this.tabPage3);
                }
                else if (Val.ToString(xtraTabActivity.SelectedTabPage.Tag) == "15") //OrderConfReturn
                {
                    DTPFromDate.Value = DateTime.Now.AddMonths(-1);
                    DTPToDate.Value = DateTime.Now;
                    lblSeller.Visible = true;
                    txtSeller.Visible = true;
                    pnlOrder.Visible = false;
                    BtnReportListExcelExpot.Visible = false;
                    GrdSummary.Columns["APPROVAL"].Visible = false;
                    GrdSummary.Columns["INVRECEV"].Visible = false;
                    GrdSummary.Columns["TRNNO_OLDDB"].Visible = false;
                    GrdDetail.Columns["TRNNO_OLDDB"].Visible = false;
                    BtnUpdateShippingParty.Visible = false;
                    BtnHKExport.Visible = false;
                    ChkShipmentPending.Visible = false;
                    GrdSummary.Columns["HKStone"].Visible = false;
                    GrdDetail.Columns["HKStone"].Visible = false;
                    PanelDiamondTypeFilter.Visible = true;//Gunjan:10/09/2024
                    this.tabPage3.Hide();
                    this.TabSummaryDetail.TabPages.Remove(this.tabPage3);
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


        private void RspChkBxApproval_CheckedChanged(object sender, EventArgs e)
        {
          
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
                    e.Appearance.BackColor = Color.FromArgb(255, 192, 192);
                    e.Appearance.BackColor2 = Color.FromArgb(255, 192, 192);
                }
                Int32 IntInvRecevFlag = Val.ToBooleanToInt(GrdSummary.GetRowCellValue(e.RowHandle, "INVRECEV"));
                if (IntInvRecevFlag == 1)
                {
                    e.Appearance.BackColor = Color.FromArgb(192, 255, 192);
                    e.Appearance.BackColor2 = Color.FromArgb(192, 255, 192);
                }
                string StrBranchReceive = Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "BRANCHRECEIVEDATE"));
                if (StrBranchReceive != "")
                {
                    e.Appearance.BackColor = Color.Thistle;
                    e.Appearance.BackColor2 = Color.Thistle;
                }
                string StrPickupDate = Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "PICKUPDATE"));
                if (StrPickupDate != "")
                {
                    e.Appearance.BackColor = Color.FromArgb(192, 192, 255);
                    e.Appearance.BackColor2 = Color.FromArgb(192, 192, 255);
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

        private void FrmMemoList_Load(object sender, EventArgs e)
        {
            try
            {               
                string Str = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdSummary.Name);
                if (Str != "")
                {
                    byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                    MemoryStream stream = new MemoryStream(byteArray);
                    GrdSummary.RestoreLayoutFromStream(stream);
                }

                Str = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdDetail.Name);
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


            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
            }
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
                ValueList.Font = new Font("Tahoma", 9, FontStyle.Regular);
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
            }
            catch (Exception EX)
            {
                this.Cursor = Cursors.WaitCursor;
                Global.MessageError(EX.Message);
                return;
            }

        }
        private void txtBroker_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME,MobileNo";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_BROKER);

                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBroker.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtBroker.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
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

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void RbtBarcode_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void BtnReportListExcelExpot_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                mStrStockNo = Val.Trim(txtStoneNo.Text);
                // StrKapan = Val.Trim(CmbKapan.Properties.GetCheckedItems());
                pStrBillingPartyID = Val.Trim(txtBillTo.Tag);
                pStrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
                pStrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());
                pStrMemoNo = Val.Trim(txtMemoNo.Text);
                pStrSellerID = Val.Trim(txtSeller.Tag);
                StrFilePath = ReportListExportExcel(StrFilePath);

                if (StrFilePath != "")
                {
                    string StrFileName = "Report_List";

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
        public string ReportListExportExcel(string StrFilePath)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string StrFromDate = null;
                string StrToDate = null;


                if (DTPFromDate.Checked)
                {
                    StrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
                }
                if (DTPToDate.Checked)
                {
                    StrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());
                }
                FromData = 0; ToData = 0; TrnOldFromData = ""; TrnOldToData = "";
              
                if (txtBroker.Text.Length == 0) txtBroker.Tag = null;
                string StrStaus = "ALL";
               
                int IntOrderStatus = 0;
                if (RbOrderAll.Checked)
                {
                    IntOrderStatus = Val.ToInt(RbOrderAll.Tag);
                }
                else if (RbApproveOrder.Checked)
                {
                    IntOrderStatus = Val.ToInt(RbApproveOrder.Tag);
                }
                else if (RbNonApproveOrder.Checked)
                {
                    IntOrderStatus = Val.ToInt(RbNonApproveOrder.Tag);
                }
                //End as Daksha

                DataSet Ds = ObjStock.GetDataForSaleReport(mStrStockNo, StrKapan, pStrBillingPartyID, StrFromDate, StrToDate, FromData, ToData, TrnOldFromData, TrnOldToData, pStrMemoNo, pStrSellerID, Val.ToString(txtBroker.Tag),StrHKStoneType,StrStaus,IntOrderStatus);
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

                    worksheetShortStock.Cells[1, 12, 1, 15].Style.Font.Bold = true;
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

       
        private void lblSaveLayout_Click(object sender, EventArgs e)
        {
            try
            {
                Stream str = new System.IO.MemoryStream();
                if (TabSummaryDetail.SelectedIndex == 0)
                {
                    GrdSummary.SaveLayoutToStream(str);
                }
                else
                {
                    GrdDetail.SaveLayoutToStream(str);
                }
                str.Seek(0, System.IO.SeekOrigin.Begin);
                StreamReader reader = new StreamReader(str);
                string text = reader.ReadToEnd();
                if (TabSummaryDetail.SelectedIndex == 0)
                {
                    int IntRes = new BOTRN_StockUpload().SaveGridLayout(this.Name, GrdSummary.Name, text);
                    if (IntRes != -1)
                    {
                        Global.Message("GrdSummary Layout Successfully Saved");
                    }
                }
                else
                {
                    int IntRes = new BOTRN_StockUpload().SaveGridLayout(this.Name, GrdDetail.Name, text);
                    if (IntRes != -1)
                    {
                        Global.Message(" GrdDetail Layout Successfully Saved");
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void lblDefaultLayout_Click(object sender, EventArgs e)
        {
            try
            {
                if (TabSummaryDetail.SelectedIndex == 0)
                {
                    int IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdSummary.Name);
                    if (IntRes != -1)
                    {
                        Global.Message("Layout Successfully Deleted");
                    }
                }
                else
                {
                    int IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdDetail.Name);
                    if (IntRes != -1)
                    {
                        Global.Message("Layout Successfully Deleted");
                    }
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }

        }

        private void MainGrdSummary_Click(object sender, EventArgs e)
        {

        }

        private void txtInvNoTo_Leave(object sender, EventArgs e)
        {
            BtnSearch.Focus();
        }

        private void txtOldTrnNoTo_Leave(object sender, EventArgs e)
        {
            BtnSearch.Focus();
        }

        private void GrdDetail_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
          
        }

        public DevExpress.XtraGrid.Columns.FixedStyle Life { get; set; }

        private void BtnUpdateShippingParty_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTab = GetTableOfSelectedRows(GrdDetail, true);
                if (DTab.Rows.Count <= 0)
                {
                    Global.Message("Please Select Records to Update Party....");
                    return;
                }
                GrpUpdate.Visible = true;
                ClearUpdatePanel();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        public void ClearUpdatePanel()
        {
            txtShippingParty.Text = string.Empty;
            txtShippingParty.Tag = string.Empty;
            txtAddress.Text = string.Empty;            
            ChkISHKApprove.Checked = false;
            txtShippingParty.Focus();
        }

        private void BtnUpdateClose_Click(object sender, EventArgs e)
        {
            try
            {
                GrpUpdate.Visible = false;
                ClearUpdatePanel();
                BtnSearch_Click(null, null);
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
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
                        txtAddress.Text = Val.ToString(FrmSearch.DRow["BILLINGADDRESS1"]);                     
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

        private void btnPartyUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                string StrApprove = "";

                DataTable DTab = GetTableOfSelectedRows(GrdDetail, true);
                MemoEntryProperty Property = new MemoEntryProperty();

                if (DTab.Rows.Count <= 0)
                {
                    Global.Message("Please Select Records That You Want To Update..");
                    return;
                }

                if (Val.ToString(txtShippingParty.Tag) == string.Empty)
                {
                    Global.Message("Please Select Shipping Party...");
                    txtShippingParty.Focus();
                    return;
                }
                var list = DTab.AsEnumerable().Select(r => r["MEMO_ID"].ToString());
                string strMemo_Id = string.Join(",", list);


                if (ChkISHKApprove.Checked == true)
                {
                    StrApprove = "1";
                }
                else
                {
                    StrApprove = "0";
                }

                this.Cursor = Cursors.WaitCursor;
                Property.SHIPPINGPARTY_ID = Val.ToString(txtShippingParty.Tag);
                Property.SHIPPINGADDRESS1 = Val.ToString(txtAddress.Text);
                Property = ObjMemo.UpdateMemoData(strMemo_Id, Property, StrApprove);
                string StrReturnDesc = Property.ReturnMessageDesc;
                this.Cursor = Cursors.Default;
                Global.Message(StrReturnDesc);
                if (Property.ReturnMessageType == "SUCCESS")
                {
                    ClearUpdatePanel();
                }
            }
            catch (Exception EX)
            {
                this.Cursor = Cursors.Default;
                Global.MessageError(EX.Message);

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

        private void BtnHKExport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable dtData = GetTableOfSelectedRows(GrdDetail, true);
                if (dtData.Rows.Count <= 0)
                {
                    Global.Message("Please Select Records to Export");
                    return;
                }

                var list = dtData.AsEnumerable().Select(r => r["MEMODETAIL_ID"].ToString());
                string StrFilePath = HKExportExcel(string.Join(",", list));

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
        }

        private void txtStoneNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
               
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
            }
            catch (Exception EX)
            {
                Global.MessageError(EX.Message);
                return;
            }
        }

        private void cLabel7_Click(object sender, EventArgs e)
        {

        }

        private void txtShipPartyFilter_KeyPress(object sender, KeyPressEventArgs e)
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
                        txtShipPartyFilter.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtShipPartyFilter.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
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

        private void txtStoneNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                isPasteAction = true;
            }
        }

        public string HKExportExcel(string strMemoDetailIds, string StrFilePath = "")
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

        private void BtnSalereturn_Click(object sender, EventArgs e)
        {

            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                if (DtInvDetail.DefaultView.ToTable(true, "MEMODETAIL_ID").Rows.Count < 1)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                    return;
                }

                string StrStoneNo = string.Empty;
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select (DELIVERY) Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }
                
                int partyCount = DtInvDetail.AsEnumerable().Select(row => row.Field<Guid>("BILLPARTY_ID")).Distinct().Count();
                if (partyCount > 1)
                {
                    Global.Message("Please select same party.");
                    return;
                }

                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                 FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.SALEINVOICE, DtInvDetail, mStrStockType);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());

            }

            //try
            //{

            //    DataTable DTab = GetTableOfSelectedRows(GrdDetail, true);

            //    if (DTab == null || DTab.Rows.Count == 0)
            //    {
            //        Global.Message("Please Select Atleast One Stone For Transfer");
            //        return;
            //    }


            //    DataTable DTabDistinct = DTab.DefaultView.ToTable(true, "MEMODETAIL_ID");
            //    //if (DTabDistinct.Rows.Count > 1)
            //    //{
            //    //    Global.Message("You Are Selecting Multiple Party Stones ? Please Select Only One Party Stone");
            //    //    return;
            //    //}
            //    DTabDistinct.Dispose();
            //  //  DTabDistinct = null;


            //    FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
            //    FrmMemoEntry.MdiParent = Global.gMainRef;
            // //   FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
            //    //if (mFormType == FORMTYPE.SALESDELIVERYRETURN)
            //        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.SALESDELIVERYRETURN, null);
            //}
            //catch (Exception EX)
            //{
            //    Global.MessageError(EX.Message);
            //}

            //FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
            //FrmMemoEntry.MdiParent = Global.gMainRef;
            //FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
            //if (mFormType == FORMTYPE.SALESDELIVERYRETURN)
            //    FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.SALESDELIVERYRETURN, null);
        }
    }



}


