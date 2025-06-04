using MahantExport.Masters;
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

namespace MahantExport.Stock
{
    public partial class FrmStockStatusSearch : DevControlLib.cDevXtraForm
    {
        IDataObject PasteclipData = Clipboard.GetDataObject();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        String PasteData = "";

        DataTable DTabSelection = new DataTable();
        DataTable DtabLiveStockDetail = new DataTable();
        DataTable DtabLiveStockSummary = new DataTable();
        BODevGridSelection ObjGridSelection;
        DataTable DTabPageSummary = new DataTable();

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

        string mStrStockStatus = "";
        string mStrStockType = "";

        #region Property Settings

        public FrmStockStatusSearch()
        {
            InitializeComponent();
        }

        //HINA - START
        public FORMTYPE mFormType = FORMTYPE.SINGLELIVESTOCK;
        public enum FORMTYPE
        {
            SINGLELIVESTOCK = 1,
            PARCELLIVESTOCK = 2
        }

        public void ShowForm()
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            this.Show();

            //GrdDetail.BeginUpdate();
            //if (MainGrdDetail.RepositoryItems.Count == 6)
            //{
            //    ObjGridSelection = new DevExpressGrid();
            //    ObjGridSelection.View = GrdDetail;
            //    ObjGridSelection.ISBoolApplicableForPageConcept = true;
            //    ObjGridSelection.ClearSelection();
            //    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            //    GrdDetail.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;
            //}
            //else
            //{
            //    ObjGridSelection.ClearSelection();
            //}

            //GrdDetail.EndUpdate();
            //if (ObjGridSelection != null)
            //{
            //    ObjGridSelection.ClearSelection();
            //    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            //}


            if (BusLib.Configuration.BOConfiguration.gEmployeeProperty.ISDISPLAYCOSTPRICE == false)
            {
                GrdDetail.Columns["COSTRAPAPORT"].Visible = false;
                GrdDetail.Columns["COSTDISCOUNT"].Visible = false;
                GrdDetail.Columns["COSTPRICEPERCARAT"].Visible = false;
                GrdDetail.Columns["COSTAMOUNT"].Visible = false;

                GrdDetail.Columns["FCOSTPRICEPERCARAT"].Visible = false;
                GrdDetail.Columns["FCOSTAMOUNT"].Visible = false;
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

        //public void FillListControls()
        //{
        //    ListShape.DTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.TABLE.MST_SHAPE);
        //    ListColor.DTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.TABLE.MST_COLOR);
        //    ListClarity.DTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.TABLE.MST_CLARITY);
        //    ListCut.DTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.TABLE.MST_CUT);
        //    ListPol.DTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.TABLE.MST_POL);
        //    ListSym.DTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.TABLE.MST_SYM);
        //    ListFL.DTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.TABLE.MST_FL);

        //    CmbStatus.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.TABLE.WEBSTATUS);
        //    CmbStatus.Properties.DisplayMember = "WEBSTATUS";
        //    CmbStatus.Properties.ValueMember = "WEBSTATUS";
        //    //CmbStatus.SetEditValue("AVAILABLE,HOLD,MEMO,NONE,OFFLINE,SOLD");
        //    CmbStatus.SetEditValue("AVAILABLE,HOLD,MEMO,NONE,DELIVERY,SOLD,PURCHASE-RETURN");
        //}


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

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                if (backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.CancelAsync();
                }
                progressPanel1.Visible = true;
                backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnBestFit_Click(object sender, EventArgs e)
        {
            GrdDetail.BestFitColumns();
        }

        public void CalculateTotalSummary()
        {
            try
            {
                if (DTabPageSummary.Rows.Count > 0)
                {
                    txtTotalPcs.Text = string.Format("{0:0}", Val.ToString(DTabPageSummary.Rows[0]["PCS"]));
                    txtTotalCarat.Text = string.Format("{0:0.000}", Val.ToString(DTabPageSummary.Rows[0]["CARAT"]));
                    txtTotalAmount.Text = string.Format("{0:0.00}", Val.ToString(DTabPageSummary.Rows[0]["AMOUNT"]));
                    txtTotalPricePerCarat.Text = string.Format("{0:0.00}", Val.ToString(DTabPageSummary.Rows[0]["PRICEPERCARAT"]));
                    txtTotalDisc.Text = string.Format("{0:0.00}", Val.ToString(DTabPageSummary.Rows[0]["AVGDISC"]));
                }
                else
                {

                    txtTotalPcs.Text = string.Empty;
                    txtTotalCarat.Text = string.Empty;
                    txtTotalDisc.Text = string.Empty;
                    txtTotalAmount.Text = string.Empty;
                    txtTotalPricePerCarat.Text = string.Empty;
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private DataTable GetTableOfSelectedRowsPage(GridView view, Boolean IsSelect)
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
                aryLst = ObjGridSelection.GetSelectedArrayListPage();
                resultTable = sourceTable.Clone();
                for (int i = 0; i < aryLst.Count; i++)
                {
                    DataRow oDataRowView = aryLst[i] as DataRow;
                    resultTable.ImportRow(oDataRowView);
                }
            }

            return resultTable;
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

        private void txtStoneNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtStoneCertiMFGMemo.Text.Length > 0 && Convert.ToString(PasteData) != "")
                {
                    txtStoneCertiMFGMemo.SelectAll();
                    String str1 = PasteData.Replace("\r\n", ",");                   //data.Replace(\n, ",");
                    str1 = str1.Trim();
                    str1 = str1.TrimEnd();
                    str1 = str1.TrimStart();
                    str1 = str1.TrimEnd(',');
                    str1 = str1.TrimStart(',');
                    txtStoneCertiMFGMemo.Text = str1;
                    PasteData = "";
                }

                lblTotalCount.Text = "(" + txtStoneCertiMFGMemo.Text.Split(',').Length + ")";
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


        private void GrdDetail_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
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
                        //DouCostDisc = Math.Round(((DouCostPricePerCarat - DouCostRapaport) / DouCostRapaport * 100), 2);
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
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDetail_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                if (GrdDetail.FocusedRowHandle < 0)
                    return;

                if (Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("CHECKMARKSELECTION"))
                {
                    Int32[] selectedRowHandles = GrdDetail.GetSelectedRows();
                    if (selectedRowHandles.Length > 1)
                    {
                        for (int i = 0; i < selectedRowHandles.Length; i++)
                        {
                            GrdDetail.SetRowCellValue(selectedRowHandles[i], e.Column, e.CellValue);
                            e.Handled = true;
                        }
                    }
                }
                if (e.Clicks == 2 && Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("PARTYSTOCKNO")) //Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("STOCKNO")
                {
                    FrmStoneHistory FrmStoneHistory = new FrmStoneHistory();
                    FrmStoneHistory.MdiParent = Global.gMainRef;
                    FrmStoneHistory.ShowForm(Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "STOCK_ID")), Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "PARTYSTOCKNO")), Stock.FrmStoneHistory.FORMTYPE.DISPLAY); //Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "STOCKNO"))
                }

                //if (e.Clicks == 2 && Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("ISCERTI") && Val.ToBoolean(GrdDetail.GetFocusedRowCellValue("ISCERTI")) == true)
                if (e.Clicks == 2 && Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("ISCERTI") && Val.ToInt32(GrdDetail.GetFocusedRowCellValue("ISCERTI")) == 1)
                {
                    string CertificateUrl = Val.ToString(GrdDetail.GetFocusedRowCellValue("CERTIFICATEURL"));
                    if (CertificateUrl != "")
                        System.Diagnostics.Process.Start(CertificateUrl, "cmd");
                    //else
                    //    System.Diagnostics.Process.Start("www.mdfhk.com/certificate/" + Val.ToString(GrdDetail.GetFocusedRowCellValue("LABREPORTNO")) + ".pdf", "cmd");
                }

                if (e.Clicks == 2 && Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("ISVIDEO") && Val.ToInt32(GrdDetail.GetFocusedRowCellValue("ISVIDEO")) == 1)
                {
                    string VideoUrl = Val.ToString(GrdDetail.GetFocusedRowCellValue("VIDEOURL"));
                    System.Diagnostics.Process.Start(VideoUrl, "cmd");
                    //System.Diagnostics.Process.Start("www.mdfhk.com/stonevideo/" + Val.ToString(GrdDetail.GetFocusedRowCellValue("PARTYSTOCKNO")) + ".mp4", "cmd");
                }

                if (e.Clicks == 2 && Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("ISIMAGE") && Val.ToInt32(GrdDetail.GetFocusedRowCellValue("ISIMAGE")) == 1)
                {
                    //System.Diagnostics.Process.Start("www.mdfhk.com/stoneimage/" + Val.ToString(GrdDetail.GetFocusedRowCellValue("PARTYSTOCKNO")) + "/" + Val.ToString(GrdDetail.GetFocusedRowCellValue("PARTYSTOCKNO")) + "-1.jpg", "cmd");
                    //System.Diagnostics.Process.Start("www.mdfhk.com/stoneimage/" + Val.ToString(GrdDetail.GetFocusedRowCellValue("PARTYSTOCKNO")) + "/gray01.jpg", "cmd");
                    string ImageUrl = Val.ToString(GrdDetail.GetFocusedRowCellValue("IMAGEURL"));
                    System.Diagnostics.Process.Start(ImageUrl, "cmd");
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }

        }

        private void GrdSum_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }

                string StrCol = Val.ToString(GrdSum.GetRowCellValue(e.RowHandle, "STATUS"));
                if (StrCol.ToUpper() == "AVAILABLE")
                {
                    e.Appearance.BackColor = lblAvailable.BackColor;
                    e.Appearance.BackColor2 = lblAvailable.BackColor;
                }
                else if (StrCol.ToUpper() == "NONE")
                {
                    e.Appearance.BackColor = lblNone.BackColor;
                    e.Appearance.BackColor2 = lblNone.BackColor;
                }
                else if (StrCol.ToUpper() == "MEMO" || Val.ToString(GrdSum.GetRowCellValue(e.RowHandle, "STOCKTYPE")).Contains("MEMO"))
                {
                    e.Appearance.BackColor = lblMemo.BackColor;
                    e.Appearance.BackColor2 = lblMemo.BackColor;
                }
                else if (StrCol.ToUpper() == "HOLD")
                {
                    e.Appearance.BackColor = lblHold.BackColor;
                    e.Appearance.BackColor2 = lblHold.BackColor;
                }
                else if (StrCol.ToUpper() == "OFFLINE")
                {
                    e.Appearance.BackColor = lblOffline.BackColor;
                    e.Appearance.BackColor2 = lblOffline.BackColor;
                }
                else if (StrCol.ToUpper() == "SOLD")
                {
                    e.Appearance.BackColor = lblSold.BackColor;
                    e.Appearance.BackColor2 = lblSold.BackColor;
                }
                else if (StrCol.ToUpper() == "DELIVERY")
                {
                    e.Appearance.BackColor = lblInvoice.BackColor;
                    e.Appearance.BackColor2 = lblInvoice.BackColor;
                }
                else if (StrCol.ToUpper() == "PURCHASE-RETURN")
                {
                    e.Appearance.BackColor = lblPurchaseReturn.BackColor;
                    e.Appearance.BackColor2 = lblPurchaseReturn.BackColor;
                }
                else if (StrCol.ToUpper() == "FACTGRD")
                {
                    e.Appearance.BackColor = lblFactGrd.BackColor;
                    e.Appearance.BackColor2 = lblFactGrd.BackColor;
                }
                else if (StrCol.ToUpper() == "ASSRET")
                {
                    e.Appearance.BackColor = lblAssReturn.BackColor;
                    e.Appearance.BackColor2 = lblAssReturn.BackColor;
                }
                else if (StrCol.ToUpper() == "LABRET")
                {
                    e.Appearance.BackColor = lblLabReturn.BackColor;
                    e.Appearance.BackColor2 = lblLabReturn.BackColor;
                }
                else if (StrCol.ToUpper() == "NOTFOUND")
                {
                    e.Appearance.BackColor = lblNotFound.BackColor;
                    e.Appearance.BackColor2 = lblNotFound.BackColor;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }


        private void FrmMemoEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            BtnSearch.PerformClick();
        }
        private void FrmLiveStock_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void BtnMemoIssue_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetTableOfSelectedRowsPage(GrdDetail, true);

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (!(Val.ToString(DRow["STATUS"]) == "AVAILABLE" || Val.ToString(DRow["STATUS"]) == "OFFLINE") && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Available Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.MEMOISSUE, DtInvDetail, mStrStockType);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnMemoReturn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetTableOfSelectedRowsPage(GrdDetail, true);

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToString(DRow["STATUS"]) != "MEMO" && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Memo Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                if (DtInvDetail.DefaultView.ToTable(true, "BILLPARTY_ID").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                    return;
                }

                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.MEMORETURN, DtInvDetail, mStrStockType);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnSalesOrder_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetTableOfSelectedRowsPage(GrdDetail, true);

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }


                if (DtInvDetail.DefaultView.ToTable(true, "BILLPARTY_ID").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                    return;
                }

                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (!(
                        Val.ToString(DRow["STATUS"]) == "AVAILABLE" ||
                        Val.ToString(DRow["STATUS"]) == "MEMO" ||
                        Val.ToString(DRow["STATUS"]) == "OFFLINE" ||
                        Val.ToString(DRow["STATUS"]) == "HOLD"
                        ) && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select (Available / Memo / Offline / Hold) Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.ORDERCONFIRM, DtInvDetail, mStrStockType);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;

                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnSaleInvoice_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetTableOfSelectedRowsPage(GrdDetail, true);

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                //if (DtInvDetail.DefaultView.ToTable(true, "BILLPARTY_ID").Rows.Count > 1)
                //{
                //    this.Cursor = Cursors.Default;

                //    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                //    return;
                //}

                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (!(
                        Val.ToString(DRow["STATUS"]) == "AVAILABLE" ||
                        Val.ToString(DRow["STATUS"]) == "MEMO" ||
                        Val.ToString(DRow["STATUS"]) == "OFFLINE" ||
                        Val.ToString(DRow["STATUS"]) == "SOLD"
                        ) && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select (SOLD) Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
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
        }

        private void BtnSalesReturn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetTableOfSelectedRowsPage(GrdDetail, true);

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                if (DtInvDetail.DefaultView.ToTable(true, "BILLPARTY_ID").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                    return;
                }

                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToString(DRow["STATUS"]) != "DELIVERY" && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select (DELIVERY) Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.SALESDELIVERYRETURN, DtInvDetail, mStrStockType);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());

            }
        }

        private void BtnOnline_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetTableOfSelectedRowsPage(GrdDetail, true);

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                if (DtInvDetail.DefaultView.ToTable(true, "STATUS").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Status Stone. Please Select Only Single Status");
                    return;
                }

                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToString(DRow["STATUS"]) != "OFFLINE" && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Offline Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.ONLINE, DtInvDetail, mStrStockType);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnOffline_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetTableOfSelectedRowsPage(GrdDetail, true);

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                if (DtInvDetail.DefaultView.ToTable(true, "STATUS").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Status Stone. Please Select Only Single Status");
                    return;
                }

                if (DtInvDetail.DefaultView.ToTable(true, "BILLPARTY_ID").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                    return;
                }

                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToString(DRow["STATUS"]) != "AVAILABLE" && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }

                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Available Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.OFFLINE, DtInvDetail, mStrStockType);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());

            }
        }

        private void BtnHold_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetTableOfSelectedRowsPage(GrdDetail, true);

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                if (DtInvDetail.DefaultView.ToTable(true, "STATUS").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Status Stone. Please Select Only Single Status");
                    return;
                }


                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (!(Val.ToString(DRow["STATUS"]) == "AVAILABLE" || Val.ToString(DRow["STATUS"]) == "OFFLINE") && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }

                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Available Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.HOLD, DtInvDetail, mStrStockType);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnRelease_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetTableOfSelectedRowsPage(GrdDetail, true);

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                if (DtInvDetail.DefaultView.ToTable(true, "STATUS").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Status Stone. Please Select Only Single Status");
                    return;
                }


                if (DtInvDetail.DefaultView.ToTable(true, "BILLPARTY_ID").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                    return;
                }

                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToString(DRow["STATUS"]) != "HOLD" && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }

                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Available Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.RELEASE, DtInvDetail, mStrStockType);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }


        private void BtnPurchaseReturn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetTableOfSelectedRowsPage(GrdDetail, true);

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                if (DtInvDetail.DefaultView.ToTable(true, "STATUS").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Status Stone. Please Select Only Single Status");
                    return;
                }


                if (DtInvDetail.DefaultView.ToTable(true, "BILLPARTY_ID").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                    return;
                }

                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToString(DRow["STATUS"]) != "NONE" && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }

                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Available Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.PURCHASERETURN, DtInvDetail, mStrStockType);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }
        private void BtnPricing_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtStoneDetail = GetTableOfSelectedRowsPage(GrdDetail, true);

                if (DtStoneDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                string StrStoneNo = string.Empty;
                string StrStoneList = string.Empty;

                foreach (DataRow DRow in DtStoneDetail.Rows)
                {
                    if (Val.ToString(DRow["STATUS"]) != "OFFLINE" && Val.ToString(DRow["STATUS"]) != "AVAILABLE" && Val.ToString(DRow["STATUS"]) != "NONE"
                        //&& Val.ToString(DRow["STATUS"]) != "FACTGRD" && Val.ToString(DRow["STATUS"]) != "ASSISS" && Val.ToString(DRow["STATUS"]) != "ASSRET"  && Val.ToString(DRow["STATUS"]) != "LABISS" && Val.ToString(DRow["STATUS"]) != "LABRES" 
                        && Val.ToString(DRow["STATUS"]) != "LAB-RETURN")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                    else
                    {
                        StrStoneList = StrStoneList + Val.ToString(DRow["STOCKNO"]) + ",";
                    }
                }

                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select 'Available/None/Offline/LabReturn' Status Stones\n\nThis Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                if (StrStoneList.Length != 0 )
                {
                    StrStoneList = StrStoneList.Substring(0, StrStoneList.Length - 1);
                }

                FrmParameterUpdate FrmParameterUpdate = new FrmParameterUpdate();
                FrmParameterUpdate.MdiParent = Global.gMainRef;
                FrmParameterUpdate.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                FrmParameterUpdate.ShowForm(StrStoneList);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdSum_RowClick(object sender, RowClickEventArgs e)
        {

        }

        private void GrdSum_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Clicks == 2)
            {
                if (e.Column.FieldName == "DISPLAYSTATUS")
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (Val.ToString(GrdSum.GetFocusedRowCellValue("STOCKTYPE")).Contains("SINGLE"))
                    {
                        GrdDetail.Columns["STATUS"].ClearFilter();
                        GrdDetail.Columns["STATUS"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo("STATUS='" + Val.ToString(GrdSum.GetRowCellValue(e.RowHandle, "STATUS")) + "' And STOCKTYPE = 'SINGLE'");
                    }
                    else
                    {
                        GrdDetail.Columns["STATUS"].ClearFilter();
                    }
                    this.Cursor = Cursors.Default;
                }

            }
        }

        private void BtnClearFilter_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            GrdDetail.Columns["STATUS"].ClearFilter();
            this.Cursor = Cursors.Default;
        }




        private void GrdDetail_RowStyle(object sender, RowStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }

                string StrCol = Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "STATUS"));
                string StrStockType = Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "STOCKTYPE"));
                if (StrStockType == "SINGLE")
                {
                    if (StrCol.ToUpper() == "AVAILABLE")
                    {
                        e.Appearance.BackColor = lblAvailable.BackColor;
                        e.Appearance.BackColor2 = lblAvailable.BackColor;
                    }
                    else if (StrCol.ToUpper() == "NONE")
                    {
                        e.Appearance.BackColor = lblNone.BackColor;
                        e.Appearance.BackColor2 = lblNone.BackColor;
                    }
                    else if (StrCol.ToUpper() == "MEMO")
                    {
                        e.Appearance.BackColor = lblMemo.BackColor;
                        e.Appearance.BackColor2 = lblMemo.BackColor;
                    }
                    else if (StrCol.ToUpper() == "HOLD")
                    {
                        e.Appearance.BackColor = lblHold.BackColor;
                        e.Appearance.BackColor2 = lblHold.BackColor;
                    }
                    else if (StrCol.ToUpper() == "OFFLINE")
                    {
                        e.Appearance.BackColor = lblOffline.BackColor;
                        e.Appearance.BackColor2 = lblOffline.BackColor;
                    }
                    else if (StrCol.ToUpper() == "SOLD")
                    {
                        e.Appearance.BackColor = lblSold.BackColor;
                        e.Appearance.BackColor2 = lblSold.BackColor;
                    }
                    else if (StrCol.ToUpper() == "DELIVERY")
                    {
                        e.Appearance.BackColor = lblInvoice.BackColor;
                        e.Appearance.BackColor2 = lblInvoice.BackColor;
                    }
                    else if (StrCol.ToUpper() == "PURCHASE-RETURN")
                    {
                        e.Appearance.BackColor = lblPurchaseReturn.BackColor;
                        e.Appearance.BackColor2 = lblPurchaseReturn.BackColor;
                    }
                    else if (StrCol.ToUpper() == "FACTGRD")
                    {
                        e.Appearance.BackColor = lblFactGrd.BackColor;
                        e.Appearance.BackColor2 = lblFactGrd.BackColor;
                    }
                    else if (StrCol.ToUpper() == "ASSRET")
                    {
                        e.Appearance.BackColor = lblAssReturn.BackColor;
                        e.Appearance.BackColor2 = lblAssReturn.BackColor;
                    }
                    else if (StrCol.ToUpper() == "LABRET")
                    {
                        e.Appearance.BackColor = lblLabReturn.BackColor;
                        e.Appearance.BackColor2 = lblLabReturn.BackColor;
                    }
                    else if (StrCol.ToUpper() == "NOTFOUND")
                    {
                        e.Appearance.BackColor = lblNotFound.BackColor;
                        e.Appearance.BackColor2 = lblNotFound.BackColor;
                    }
                }
                if (StrStockType == "PARCEL")
                {
                    double DouMemoPending = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "MEMOPENDINGCARAT"));

                    if (DouMemoPending != 0)
                    {
                        e.Appearance.BackColor = lblMemo.BackColor;
                        e.Appearance.BackColor2 = lblMemo.BackColor;
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

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

            LiveStockProperty mLStockProperty = new LiveStockProperty();
            if (RbtStoneNo.Checked == true)
            {
                mLStockProperty.STOCKNO = txtStoneCertiMFGMemo.Text;
                mLStockProperty.LABREPORTNO = string.Empty;
                mLStockProperty.SERIALNO = string.Empty;
                mLStockProperty.MEMONO = string.Empty;
            }
            else if (RbtCertiNo.Checked == true)
            {
                mLStockProperty.STOCKNO = string.Empty;
                mLStockProperty.LABREPORTNO = txtStoneCertiMFGMemo.Text;
                mLStockProperty.SERIALNO = string.Empty;
                mLStockProperty.MEMONO = string.Empty;
            }
            else if (RbtMFGNo.Checked == true)
            {
                mLStockProperty.STOCKNO = string.Empty;
                mLStockProperty.LABREPORTNO = string.Empty;
                mLStockProperty.SERIALNO = txtStoneCertiMFGMemo.Text;
                mLStockProperty.MEMONO = string.Empty;
            }
            else if (RbtMemoNo.Checked == true)
            {
                mLStockProperty.STOCKNO = string.Empty;
                mLStockProperty.LABREPORTNO = string.Empty;
                mLStockProperty.SERIALNO = string.Empty;
                mLStockProperty.MEMONO = txtStoneCertiMFGMemo.Text;
            }

            DataSet DsLiveStock = ObjStock.GetStockStatusSearchData(mLStockProperty);
            DtabLiveStockDetail = DsLiveStock.Tables[0];
            DTabPageSummary = DsLiveStock.Tables[1];
            DtabLiveStockSummary = DsLiveStock.Tables[2];
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                GrdDetail.BeginUpdate();
                GrdSum.BeginUpdate();

                MainGrdDetail.DataSource = DtabLiveStockDetail;
                MainGrdSum.DataSource = DtabLiveStockSummary;

                GrdSum.ExpandAllGroups();
                GrdSum.BestFitColumns();


                MainGrdDetail.Refresh();
                MainGrdSum.Refresh();

                //if (MainGrdDetail.RepositoryItems.Count == 6)
                //{
                //    ObjGridSelection = new DevExpressGrid();
                //    ObjGridSelection.View = GrdDetail;
                //    ObjGridSelection.ISBoolApplicableForPageConcept = true;
                //    ObjGridSelection.ClearSelection();
                //    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                //    GrdDetail.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;
                //}
                //else
                //{
                //    ObjGridSelection.ClearSelection();
                //}
                //if (ObjGridSelection != null)
                //{
                //    ObjGridSelection.ClearSelection();
                //    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                //}

                CalculateTotalSummary();

                progressPanel1.Visible = false;

                GrdDetail.EndUpdate();
                GrdSum.EndUpdate();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtStoneCertiMFGMemo_TextChanged(object sender, EventArgs e)
        {
            if (txtStoneCertiMFGMemo.Text.Length > 0 && Convert.ToString(PasteData) != "")
            {
                txtStoneCertiMFGMemo.SelectAll();
                String str1 = PasteData.Replace("\r\n", ",");                   //data.Replace(\n, ",");
                str1 = str1.Trim();
                str1 = str1.TrimEnd();
                str1 = str1.TrimStart();
                str1 = str1.TrimEnd(',');
                str1 = str1.TrimStart(',');
                txtStoneCertiMFGMemo.Text = str1;
                PasteData = "";
            }

            lblTotalCount.Text = "(" + txtStoneCertiMFGMemo.Text.Split(',').Length + ")";

        }

        private void BtnClear_Click_1(object sender, EventArgs e)
        {
            txtStoneCertiMFGMemo.Text = string.Empty;
            lblTotalCount.Text = "(0)";
            GrdDetail.Columns["STATUS"].ClearFilter();
            RbtStoneNo.Checked = true;
            CalculateTotalSummary();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Global.ExcelExport("StockStatusSearch", GrdDetail);
            }
            catch(Exception ex)
            {
                Global.Message(ex.Message.ToString());
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
    }
}
