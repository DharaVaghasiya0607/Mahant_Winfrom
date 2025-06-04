using MahantExport.Masters;
using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.Data;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using Google.API.Translate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MahantExport.View
{
    public partial class FrmOpeningClosingMarketing : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();
        BOFormPer ObjPer = new BOFormPer();
        DataTable DTabDetail = new DataTable();
        string mStrFromDate = "";
        string mStrToDate = "";
		string StrDetail = "";
		double DouTotalPcs = 0;
		double DouTotalCts = 0;
		double DouTotalAmt = 0;
		double DouCloseingPcs = 0;
		double DouCloseingCts = 0;
		double DouCloseingAmt = 0;
		double DouOpeSaleAmt = 0;
		double DouopeSaleExp = 0;
		double DouCloSaleAmt = 0;
		double DouCloSaleExp = 0;
		string StrShape = "";
		string StrInvoiceNo = "";
		double FromCarat = 0;
		double ToCarat = 0;

        #region Property Settings

        public FrmOpeningClosingMarketing()
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

            ChkExpand.Checked = false;
            this.Show();
			string Str = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdDet.Name);
			if (Str != "")
			{
				byte[] byteArray = Encoding.ASCII.GetBytes(Str);
				MemoryStream stream = new MemoryStream(byteArray);
				GrdDet.RestoreLayoutFromStream(stream);
            }
            DTPFromDate.Value = DateTime.Now.AddDays(-7);
            DTPToDate.Value = DateTime.Now;
            DTPFromDate.Focus();
            //if (BOConfiguration.gEmployeeProperty.DEPARTMENT_ID == 436)
            //{
            //    gbSalesInvoice.Visible = true;
            //    gbInvoiceReturn.Visible = true;
            //}
            //else
            //{
            //    gbSalesInvoice.Visible = false;
            //    gbInvoiceReturn.Visible = false;
            //}            
            ChkPcs_CheckedChanged(null, null);
            ChkCts_CheckedChanged(null, null);
            ChkRate_CheckedChanged(null, null);
            ChkAmt_CheckedChanged(null, null);
            ChkSaleAmt_CheckedChanged(null, null);
            ChkExpAmt_CheckedChanged(null, null);
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMemo);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        #endregion

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string GetSelectCmbValue(CheckedListBoxControl chkBoxcontrol)
        {
            string tempstr = string.Empty;
            if (chkBoxcontrol.DataSource != null)
            {
                foreach (object itemChecked in chkBoxcontrol.CheckedItems)
                {
                    DataRowView castedItem = itemChecked as DataRowView;
                    tempstr = tempstr + Val.ToString(castedItem[chkBoxcontrol.ValueMember]) + ",";
                }
            }
            if (tempstr.Length != 0)
            {
                tempstr = tempstr.Substring(0, tempstr.Length - 1);
            }
            return tempstr;
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            try
            {
				ChkExpand.Checked = false;
                
                StrDetail = "ALL";
                mStrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
                mStrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());
                string StrOpe = string.Empty;
                StrInvoiceNo = Val.ToString(txtInvoiceNo.Text);
                DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
                DTabDetail.Rows.Clear();
                BtnShow.Enabled = false;
                PanelProgress.Visible = true;
                if (!backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ChkPcs_CheckedChanged(object sender, EventArgs e)
        {
            GrdDet.Columns["OPENINGPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["SURATINCOMEPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["KAPANINWARDPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["GIACANCELPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["PLUSPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["MINUSPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["TRANSFERTOANKITPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["TRANSFERTONIKUNJPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["CLOSINGPCS"].Visible = ChkPcs.Checked;
        }

        private void ChkCts_CheckedChanged(object sender, EventArgs e)
        {
            GrdDet.Columns["OPENINGCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["SURATINCOMECARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["KAPANINWARDCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["GIACANCELCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["PLUSCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["MINUSCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["TRANSFERTOANKITCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["TRANSFERTONIKUNJCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["CLOSINGCARAT"].Visible = ChkCts.Checked;
        }

        private void ChkRate_CheckedChanged(object sender, EventArgs e)
        {
            GrdDet.Columns["OPENINGRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["SURATINCOMERATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["KAPANINWARDRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["GIACANCELRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["PLUSRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["MINUSRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["TRANSFERTOANKITRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["TRANSFERTONIKUNJRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["CLOSINGRATE"].Visible = ChkRate.Checked;
        }

        private void ChkAmt_CheckedChanged(object sender, EventArgs e)
        {
            GrdDet.Columns["OPENINGAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["SURATINCOMEAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["KAPANINWARDAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["GIACANCELAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["PLUSAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["MINUSAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["TRANSFERTOANKITAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["TRANSFERTONIKUNJAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["CLOSINGAMOUNT"].Visible = ChkAmt.Checked;
        }

        private void MainGrd_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                GridControl gridC = sender as GridControl;
                GridView gridView = gridC.FocusedView as GridView;
                BandedGridViewInfo info = (BandedGridViewInfo)gridView.GetViewInfo();
                for (int i = 0; i < info.BandsInfo.BandCount; i++)
                {
                    //e.Graphics.DrawLine(new Pen(Brushes.Black, 1), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.BandsInfo[i].Bounds.Top), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.RowsInfo[info.RowsInfo.Count - 1].Bounds.Bottom - 1));
                    //if (i == 1) e.Graphics.DrawLine(new Pen(Brushes.Black), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.BandsInfo[i].Bounds.Top), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.RowsInfo[info.RowsInfo.Count - 1].Bounds.Bottom - 1));
                    //if (i == info.BandsInfo.BandCount - 1) e.Graphics.DrawLine(new Pen(Brushes.Black), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.BandsInfo[i].Bounds.Top), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.RowsInfo[info.RowsInfo.Count - 1].Bounds.Bottom - 1));
                }
            }
            catch (Exception EX)
            {

            }

        }

        private void GrdDet_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000")
            {
                e.DisplayText = String.Empty;
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Today;
            Global.ExcelExport("StockOpeningClosingMarketingReport_" + dateTime.ToString("dd-MM-yyyy") + ".xlsx", GrdDet);
        }

        private void GrdDet_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                if (e.Clicks == 2)
                {
                    string StrDate = Val.SqlDate(Val.ToString(GrdDet.GetRowCellValue(e.RowHandle, "ENTRYDATE")));

                    FrmSearchPopupBox FrmPopupSearchBox = new FrmSearchPopupBox();
                    if (chkShowSummary.Checked == false)
                        FrmPopupSearchBox.mStrSearchField = "TransferNo,StockCarat,ShapeName,Size,Clarity,Department,EntryType";
                    else
                        FrmPopupSearchBox.mStrSearchField = "StockCarat,Size,Department,EntryType";
                    FrmPopupSearchBox.mStrSearchText = "";
                    this.Cursor = Cursors.WaitCursor;
                    string StrOpe = string.Empty;

                    if (e.Column.FieldName.Contains("OPENING"))
                        StrOpe = "Opening";
                    else if (e.Column.FieldName.Contains("CLOSING"))
                        StrOpe = "Closing";
                    else if (e.Column.FieldName.Contains("SURATINCOMECARAT"))
                        StrOpe = "SURAT INCOME";
                    else if (e.Column.FieldName.Contains("KAPANINWARDCARAT"))
                        StrOpe = "KAPAN INWARD";
                    else if (e.Column.FieldName.Contains("GIACANCELCARAT"))
                        StrOpe = "GIA CANCEL";
                    else if (e.Column.FieldName.Contains("PLUSCARAT"))
                        StrOpe = "PLUS";
                    else if (e.Column.FieldName.Contains("MINUSCARAT"))
                        StrOpe = "MINUS";
                    else if (e.Column.FieldName.Contains("TRANSFERTOANKITCARAT"))
                        StrOpe = "TRANSFER TO ANKIT";
                    else if (e.Column.FieldName.Contains("TRANSFERTONIKUNJCARAT"))
                        StrOpe = "TRANSFER TO NIKUNJ";

                    //StrDetail = "ALL";
                    StrInvoiceNo = Val.ToString(txtInvoiceNo.Text);
                    if (chkShowSummary.Checked == false)
                        FrmPopupSearchBox.mDTab = ObjMemo.GetOpeningClosingMarketingReportDetail(StrOpe, StrDate, "");
                    else
                        FrmPopupSearchBox.mDTab = ObjMemo.GetOpeningClosingMarketingReportDetail_SizeWise(StrOpe, StrDate, "");
                    //FrmPopupSearchBox.mStrColumnsToHide = "Stock_ID";
                    FrmPopupSearchBox.Height = 619;
                    FrmPopupSearchBox.Width = 838;
                    
                    this.Cursor = Cursors.Default;
                    FrmPopupSearchBox.ShowDialog();
                    //FrmPopupSearchBox.GrdDet.BestFitColumns();
                    e.Handled = true;
                    if (FrmPopupSearchBox.DRow != null)
                    {  }
                    FrmPopupSearchBox.Hide();
                    FrmPopupSearchBox.Dispose();
                    FrmPopupSearchBox = null;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DTabDetail = ObjMemo.GetOpeningClosingMarketingReport(mStrFromDate, mStrToDate, "");
            }
            catch (Exception ex)
            {
                PanelProgress.Visible = false;
                BtnShow.Enabled = true;
                Global.Message(ex.Message.ToString());
            }

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                PanelProgress.Visible = false;
                BtnShow.Enabled = true;

                if (DTabDetail.Rows.Count <= 0)
                {
                    Global.Message("No Data Found..");
                }

                GrdDet.BeginUpdate();
                MainGrd.DataSource = DTabDetail;

				GrdDet.CollapseAllGroups();
                GrdDet.RefreshData();

                if (GrdDet.GroupSummary.Count == 0)
                {
                   GrdDet.GroupSummary.Add(SummaryItemType.Custom, "OPENINGPCS", GrdDet.Columns["OPENINGPCS"]);
				   GrdDet.GroupSummary.Add(SummaryItemType.Custom, "OPENINGCARAT", GrdDet.Columns["OPENINGCARAT"]);
				   GrdDet.GroupSummary.Add(SummaryItemType.Custom, "OPENINGAMOUNT", GrdDet.Columns["OPENINGAMOUNT"]);
				   GrdDet.GroupSummary.Add(SummaryItemType.Custom, "OPENINGSALEAMOUNT", GrdDet.Columns["OPENINGSALEAMOUNT"]);
				   GrdDet.GroupSummary.Add(SummaryItemType.Custom, "OPENINGEXPAMOUNT", GrdDet.Columns["OPENINGEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "SURATINCOMEPCS", GrdDet.Columns["SURATINCOMEPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "SURATINCOMECARAT", GrdDet.Columns["SURATINCOMECARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "SURATINCOMEAMOUNT", GrdDet.Columns["SURATINCOMEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "SURATINCOMESALEAMOUNT", GrdDet.Columns["SURATINCOMESALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "SURATINCOMEEXPAMOUNT", GrdDet.Columns["SURATINCOMEEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "KAPANINWARDPCS", GrdDet.Columns["KAPANINWARDPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "KAPANINWARDCARAT", GrdDet.Columns["KAPANINWARDCARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "KAPANINWARDAMOUNT", GrdDet.Columns["KAPANINWARDAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "KAPANINWARDSALEAMOUNT", GrdDet.Columns["KAPANINWARDSALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "KAPANINWARDEXPAMOUNT", GrdDet.Columns["KAPANINWARDEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "GIACANCELPCS", GrdDet.Columns["GIACANCELPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "GIACANCELCARAT", GrdDet.Columns["GIACANCELCARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "GIACANCELAMOUNT", GrdDet.Columns["GIACANCELAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "GIACANCELSALEAMOUNT", GrdDet.Columns["GIACANCELSALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "GIACANCELEXPAMOUNT", GrdDet.Columns["GIACANCELEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "PLUSPCS", GrdDet.Columns["PLUSPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "PLUSCARAT", GrdDet.Columns["PLUSCARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "PLUSAMOUNT", GrdDet.Columns["PLUSAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "PLUSSALEAMOUNT", GrdDet.Columns["PLUSSALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "PLUSEXPAMOUNT", GrdDet.Columns["PLUSEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "MINUSPCS", GrdDet.Columns["MINUSPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "MINUSCARAT", GrdDet.Columns["MINUSCARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "MINUSAMOUNT", GrdDet.Columns["MINUSAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "MINUSSALEAMOUNT", GrdDet.Columns["MINUSSALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "MINUSEXPAMOUNT", GrdDet.Columns["MINUSEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "TRANSFERTOANKITPCS", GrdDet.Columns["TRANSFERTOANKITPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "TRANSFERTOANKITCARAT", GrdDet.Columns["TRANSFERTOANKITCARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "TRANSFERTOANKITAMOUNT", GrdDet.Columns["TRANSFERTOANKITAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "TRANSFERTOANKITSALEAMOUNT", GrdDet.Columns["TRANSFERTOANKITSALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "TRANSFERTOANKITEXPAMOUNT", GrdDet.Columns["TRANSFERTOANKITEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "TRANSFERTONIKUNJPCS", GrdDet.Columns["TRANSFERTONIKUNJPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "TRANSFERTONIKUNJCARAT", GrdDet.Columns["TRANSFERTONIKUNJCARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "TRANSFERTONIKUNJAMOUNT", GrdDet.Columns["TRANSFERTONIKUNJAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "TRANSFERTONIKUNJSALEAMOUNT", GrdDet.Columns["TRANSFERTONIKUNJSALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "TRANSFERTONIKUNJEXPAMOUNT", GrdDet.Columns["TRANSFERTONIKUNJEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Custom, "CLOSINGPCS", GrdDet.Columns["CLOSINGPCS"]);
					GrdDet.GroupSummary.Add(SummaryItemType.Custom, "CLOSINGCARAT", GrdDet.Columns["CLOSINGCARAT"]);
					GrdDet.GroupSummary.Add(SummaryItemType.Custom, "CLOSINGAMOUNT", GrdDet.Columns["CLOSINGAMOUNT"]);
					GrdDet.GroupSummary.Add(SummaryItemType.Custom, "CLOSINGSALEAMOUNT", GrdDet.Columns["CLOSINGSALEAMOUNT"]);
					GrdDet.GroupSummary.Add(SummaryItemType.Custom, "CLOSINGEXPAMOUNT", GrdDet.Columns["CLOSINGEXPAMOUNT"]);
                }
                //GrdDet.Columns["ENTRYDATE"].SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                GrdDet.EndUpdate();
            }
            catch (Exception ex)
            {
                PanelProgress.Visible = false;
                BtnShow.Enabled = true;
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDet_CustomColumnGroup(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
        {
            //try
            //{
            //    if (e.Column.FieldName == "YEARMONTH")
            //    {
            //        //double x = Math.Floor(Convert.ToDouble(e.Value1) / 100);
            //        //double y = Math.Floor(Convert.ToDouble(e.Value2) / 100);

            //        int x = Val.ToInt(Val.ToString(e.Value1).Replace("-", ""));
            //        int y = Val.ToInt(Val.ToString(e.Value2).Replace("-", ""));
            //        //int res = Comparer.Default.Compare(x, y);
            //        if (x == y)
            //            e.Result = 0;
            //        else
            //            e.Result = 1;
            //        e.Handled = true;
            //    }

            //}
            //catch (Exception ex)
            //{

            //}

        }

        private void ChkSaleAmt_CheckedChanged(object sender, EventArgs e)
        {
            GrdDet.Columns["OPENINGSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["SURATINCOMESALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["KAPANINWARDSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["GIACANCELSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["PLUSSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["MINUSSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["TRANSFERTOANKITSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["TRANSFERTONIKUNJSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["CLOSINGSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
        }

        private void ChkExpAmt_CheckedChanged(object sender, EventArgs e)
        {
            GrdDet.Columns["OPENINGEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["SURATINCOMEEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["KAPANINWARDEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["GIACANCELEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["PLUSEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["MINUSEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["TRANSFERTOANKITEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["TRANSFERTONIKUNJEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["CLOSINGEXPAMOUNT"].Visible = ChkExpAmt.Checked;
        }

        private void BtnBestFit_Click(object sender, EventArgs e)
        {
            GrdDet.BestFitColumns();
        }

		private void ChkExpand_CheckedChanged(object sender, EventArgs e)
		{
			if (ChkExpand.Checked == true)
			{
				GrdDet.ExpandAllGroups();
			}
			else 
			{
				GrdDet.CollapseAllGroups();
			}
		}

		private void GrdDet_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
		{//CLOSINGPCS
			try
			{
				if (e.SummaryProcess == CustomSummaryProcess.Start)
				{
					DouTotalPcs = 0;
					DouTotalCts = 0;
					DouTotalAmt = 0;
					DouCloseingPcs = 0;
					DouCloseingCts = 0;
					DouCloseingAmt = 0;
					DouOpeSaleAmt = 0;
					DouopeSaleExp = 0;
					DouCloSaleAmt = 0;
					DouCloSaleExp = 0;

				}
				else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
				{
					string P1 = Val.ToString(GrdDet.GetRowCellValue(e.RowHandle, "YEARMONTH"));
					string P2 = Val.ToString(GrdDet.GetRowCellValue(e.RowHandle - 1, "YEARMONTH"));

					//string P3 = Val.ToString(GrdDet.GetRowCellValue(e.RowHandle, "CLOSINGPCS"));
					if (P1 != P2)
					{
						DouTotalPcs = Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "OPENINGPCS"));
						DouTotalCts = Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "OPENINGCARAT"));
						DouTotalAmt = Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "OPENINGAMOUNT"));
						DouOpeSaleAmt = Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "OPENINGSALEAMOUNT"));
						DouopeSaleExp = Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "OPENINGEXPAMOUNT"));
					}
					else
					{
						DouCloseingPcs = Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "CLOSINGPCS"));
						DouCloseingCts = Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "CLOSINGCARAT"));
						DouCloseingAmt = Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "CLOSINGAMOUNT"));
						DouCloSaleAmt = Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "CLOSINGSALEAMOUNT"));
						DouCloSaleExp = Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "CLOSINGEXPAMOUNT"));
					}
				}

				else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
				{
					if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("OPENINGPCS") == 0)
					{
						e.TotalValue = DouTotalPcs;
					}
					if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("CLOSINGPCS") == 0)
					{
						e.TotalValue = DouCloseingPcs;
					}
					if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("OPENINGCARAT") == 0)
					{
						e.TotalValue = DouTotalCts;
					}
					if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("OPENINGAMOUNT") == 0)
					{
						e.TotalValue = DouTotalAmt;
					}
					if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("CLOSINGCARAT") == 0)
					{
						e.TotalValue = DouCloseingCts;
					}
					if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("CLOSINGAMOUNT") == 0)
					{
						e.TotalValue = DouCloseingAmt;
					}

					if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("OPENINGSALEAMOUNT") == 0)
					{
						e.TotalValue = DouOpeSaleAmt;
					}
					if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("OPENINGEXPAMOUNT") == 0)
					{
						e.TotalValue = DouopeSaleExp;
					}
					if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("CLOSINGSALEAMOUNT") == 0)
					{
						e.TotalValue = DouCloSaleAmt;
					}
					if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("CLOSINGEXPAMOUNT") == 0)
					{
						e.TotalValue = DouCloSaleExp;
					}				   
				}
			}
			catch (Exception ex)
			{

			}
		}

		private void ChkDiffPer_CheckedChanged(object sender, EventArgs e)
		{
			GrdDet.Columns["SALEDELIVERYDIFF"].Visible = ChkDiffPer.Checked;
			GrdDet.Columns["CLOSINGDIFF"].Visible = ChkDiffPer.Checked;
			GrdDet.Columns["OPENINGDIFF"].Visible = ChkDiffPer.Checked;
			GrdDet.Columns["UPLOADDIFFPER"].Visible = ChkDiffPer.Checked;
			
		}

		private void ChkCostAmt_CheckedChanged(object sender, EventArgs e)
		{
			GrdDet.Columns["CLOSINGCOSTAMT"].Visible = ChkCostAmt.Checked;
            GrdDet.Columns["SALEDELIVEYCOSTAMT"].Visible = ChkCostAmt.Checked;
			GrdDet.Columns["UPLOADCOSTAMT"].Visible = ChkCostAmt.Checked;
		}

		private void lblSaveLayout_Click(object sender, EventArgs e)
		{
			Stream str = new System.IO.MemoryStream();
			GrdDet.SaveLayoutToStream(str);
			str.Seek(0, System.IO.SeekOrigin.Begin);
			StreamReader reader = new StreamReader(str);
			string text = reader.ReadToEnd();

			int IntRes = new BOTRN_StockUpload().SaveGridLayout(this.Name, GrdDet.Name, text);
			if (IntRes != -1)
			{
				Global.Message("Layout Successfully Saved");
			}
		}

		private void lblDefaultLayout_Click(object sender, EventArgs e)
		{
			int IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdDet.Name);
			if (IntRes != -1)
			{
				Global.Message("Layout Successfully Deleted");
			}
		}

		private void txtInvoiceNo_TextChanged(object sender, EventArgs e)
		{
			String str1 = txtInvoiceNo.Text.Trim().Replace("\r\n", ",");

			txtInvoiceNo.Text = str1;
			txtInvoiceNo.Select(txtInvoiceNo.Text.Length, 0);

		}
    
    }	
}
