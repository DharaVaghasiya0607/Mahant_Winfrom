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
    public partial class FrmOpeningClosingNew : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();
        BOFormPer ObjPer = new BOFormPer();
        DataTable DTabDetail = new DataTable();
        DataTable DT = new DataTable();

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
        bool IsOrderApproved = false;

        //Add shiv 08-11-22
        double DouPURCHASEPCS = 0;
        double DouPURCHASECTS = 0;
        double DouPURCHASERETURNPCS = 0;
        double DouPURCHASERETURNCTS = 0;
        double DouLABISSUEPCS = 0;
        double DouLABISSUECTS = 0;
        double DouLABRETURNPCS = 0;
        double DouLABRETURNCTS = 0;
        double DouORDERCONFIRMAPPROVEDPCS = 0;
        double DouORDERCONFIRMAPPROVEDCTS = 0;
        double DouORDERNOTCONFIRMAPPROVEDPCS = 0;
        double DouORDERNOTCONFIRMAPPROVEDCTS = 0;
        double DouORDERCONFIRMRETURNPCS = 0;
        double DouORDERCONFIRMRETURNCTS = 0;
        double DouCONSIGNMENTISSUEPCS = 0;
        double DouCONSIGNMENTISSUECTS = 0;
        double DouCONSIGNMENTRETURNPCS = 0;
        double DouCONSIGNMENTRETURNCTS = 0;
        double DouSALEDELIVERYPCS = 0;
        double DouSALEDELIVERYCTS = 0;
        double DouSALEDELIVERYRETURNPCS = 0;
        double DouSALEDELIVERYRETURNCTS = 0;


        #region Property Settings

        public FrmOpeningClosingNew()
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

            CmbShape.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SHAPE);
            CmbShape.Properties.DisplayMember = "SHAPENAME";
            CmbShape.Properties.ValueMember = "SHAPE_ID";


            DTPFromDate.Value = DateTime.Now.AddDays(-7);
            DTPToDate.Value = DateTime.Now;
            DTPFromDate.Focus();
            if (BOConfiguration.gEmployeeProperty.DEPARTMENT_ID == 436)
            {
                gbSalesInvoice.Visible = true;
                gbInvoiceReturn.Visible = true;
            }
            else
            {
                gbSalesInvoice.Visible = false;
                gbInvoiceReturn.Visible = false;
            }            
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
                //Add shiv 07-11-2022
                cLabel1.Visible = true;
                CmbShape.Visible = true;
                CmbShape.Visible = true;
                txtFromCarat.Visible = true;
                cLabel3.Visible = true;
                txtToCarat.Visible = true;
                ChkCostAmt.Visible = true;
                ChkPcs.Visible = true;
                ChkCts.Visible = true;
                ChkRate.Visible = true;
                ChkAmt.Visible = true;
                cLabel4.Visible = true;
                txtInvoiceNo.Visible = true;
                ChkSaleAmt.Visible = true;
                ChkExpAmt.Visible = true;
                ChkDiffPer.Visible = true;
                RdbOrderApproved.Visible = true;
                RdbSales.Visible = true;
                cLabel2.Visible = true;

                PnlSerch.Visible = false;
                PnlMain.Visible = true;
                PnlSerch.AutoSize = true;
                PnlSerch.SendToBack();
                PnlMain.BringToFront();

                ChkExpand.Checked = false;
                //				StrDetail

                //if (RbtnAll.Checked == true)
                //{
                //	StrDetail = "ALL";
                //}
                //else if (RbtnPurchase.Checked == true)
                //{
                //	StrDetail = "PURCHASE";
                //}
                //else
                //{
                //	StrDetail = "MFG";
                //}

                StrDetail = "ALL";
                mStrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
                mStrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());
                string StrOpe = string.Empty;

				StrShape = Val.Trim(CmbShape.Properties.GetCheckedItems()); ;
				 FromCarat = Val.Val(txtFromCarat.Text);
				 ToCarat = Val.Val(txtToCarat.Text);
				 StrInvoiceNo = Val.ToString(txtInvoiceNo.Text);

                DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
                DTabDetail.Rows.Clear();
                BtnShow.Enabled = false;
                PanelProgress.Visible = true;
                if (!backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.RunWorkerAsync();
                }
                //DataTable DTab = ObjMemo.GetOpeningClosingReport("SINGLE", StrFromDate, StrToDate);
                //MainGrd.DataSource = DTab;
                //MainGrd.Refresh();
                //GrdDet.BestFitColumns();
                if (RdbOrderApproved.Checked == true)
                    IsOrderApproved = true;
                else
                    IsOrderApproved = false;
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
            GrdDet.Columns["UPLOADPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["PURCHASEPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["PURCHASERETURNPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["MEMOISSUEPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["MEMORETURNPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["LABISSUEPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["LABRETURNPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["CONSISSUEPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["CONSRETURNPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["ORDERCONFIRMPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["ORDERRETURNPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["SALESDELIVERYPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["SALESRETURNPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["CLOSINGPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["DEPARTMENTTRANSFERPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["SINGLETOPARCELPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["MIXTOSINGLEPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["FACTORYGRADINGPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["ORDERAPPROVEDPCS"].Visible = ChkPcs.Checked;
        }

        private void ChkCts_CheckedChanged(object sender, EventArgs e)
        {
            GrdDet.Columns["OPENINGCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["UPLOADCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["PURCHASECARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["PURCHASERETURNCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["MEMOISSUECARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["MEMORETURNCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["LABISSUECARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["LABRETURNCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["CONSISSUECARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["CONSRETURNCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["ORDERCONFIRMCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["ORDERRETURNCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["SALESDELIVERYCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["SALESRETURNCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["CLOSINGCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["DEPARTMENTTRANSFERCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["SINGLETOPARCELCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["MIXTOSINGLECARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["FACTORYGRADINGCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["ORDERAPPROVEDCARAT"].Visible = ChkCts.Checked;
        }

        private void ChkRate_CheckedChanged(object sender, EventArgs e)
        {
            GrdDet.Columns["OPENINGRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["UPLOADRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["PURCHASERATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["PURCHASERETURNRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["MEMOISSUERATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["MEMORETURNRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["LABISSUERATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["LABRETURNRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["CONSISSUERATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["CONSRETURNRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["ORDERCONFIRMRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["ORDERRETURNRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["SALESDELIVERYRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["SALESRETURNRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["CLOSINGRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["DEPARTMENTTRANSFERRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["MIXTOSINGLERATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["SINGLETOPARCELRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["FACTORYGRADINGRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["ORDERAPPROVEDRATE"].Visible = ChkRate.Checked;
        }

        private void ChkAmt_CheckedChanged(object sender, EventArgs e)
        {
            GrdDet.Columns["OPENINGAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["UPLOADAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["PURCHASEAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["PURCHASERETURNAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["MEMOISSUEAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["MEMORETURNAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["LABISSUEAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["LABRETURNAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["CONSISSUEAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["CONSRETURNAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["ORDERCONFIRMAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["ORDERRETURNAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["SALESDELIVERYAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["SALESRETURNAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["CLOSINGAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["DEPARTMENTTRANSFERAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["MIXTOSINGLEAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["SINGLETOPARCELAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["FACTORYGRADINGAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["ORDERAPPROVEDAMOUNT"].Visible = ChkAmt.Checked;
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
            Global.ExcelExport("StockLedger.xlsx", GrdDet);
        }

        private void GrdDet_RowCellClick(object sender, RowCellClickEventArgs e)
        {

            try
            {
                if (e.Clicks == 2)
                {
                    string StrDate = Val.SqlDate(Val.ToString(GrdDet.GetRowCellValue(e.RowHandle, "ENTRYDATE")));

                    FrmSearchPopupBox FrmPopupSearchBox = new FrmSearchPopupBox();
                    FrmPopupSearchBox.mStrSearchField = "StockNo,Carat,Shape,Color,Clarity,Cut,Pol,Sym,FL,Lab,LabReportNo,SaleAmount,DaysDiff,Status";
                    FrmPopupSearchBox.mStrSearchText = "";
                    this.Cursor = Cursors.WaitCursor;
                    string StrOpe = string.Empty;

                    if (e.Column.FieldName.Contains("OPENING"))
                        StrOpe = "Opening";
                    else if (e.Column.FieldName.Contains("CLOSING"))
                        StrOpe = "Closing";
                    else if (e.Column.FieldName.Contains("UPLOAD"))
                        StrOpe = "Upload";
                    else if (e.Column.FieldName.Contains("PURCHASE"))
                        StrOpe = "Purchase";
                    else if (e.Column.FieldName.Contains("PURCHASERETURN"))
                        StrOpe = "Purchase Return";
                    else if (e.Column.FieldName.Contains("MEMOISSUE"))
                        StrOpe = "Memo Issue";
                    else if (e.Column.FieldName.Contains("MEMORETURN"))
                        StrOpe = "Memo Return";
                    else if (e.Column.FieldName.Contains("LABISSUE"))
                        StrOpe = "Lab Issue";
                    else if (e.Column.FieldName.Contains("LABRETURN"))
                        StrOpe = "Lab Return";
                    else if (e.Column.FieldName.Contains("CONSISSUE"))
                        StrOpe = "Consignment Issue";
                    else if (e.Column.FieldName.Contains("CONSRETURN"))
                        StrOpe = "Consignment Return";
                    else if (e.Column.FieldName.Contains("ORDERCONFIRM"))
                        StrOpe = "Order Confirm";
                    else if (e.Column.FieldName.Contains("ORDERRETURN"))
                        StrOpe = "Order Return";
                    else if (e.Column.FieldName.Contains("SALESDELIVERY"))
                        StrOpe = "Delivery";
                    else if (e.Column.FieldName.Contains("SALESRETURN"))
                        StrOpe = "Delivery Return";
                    else if (e.Column.FieldName.Contains("SINGLETOPARCELPCS"))
                        StrOpe = "Single To Parcel";
                    else if (e.Column.FieldName.Contains("DEPARTMENTTRANSFERPCS"))
                        StrOpe = "Department Transfer";
                    else if (e.Column.FieldName.Contains("MIXTOSINGLEPCS"))
                        StrOpe = "Mix To Single";
                    else if (e.Column.FieldName.Contains("FACTORYGRADINGPCS"))
                        StrOpe = "Factory Grading";
                    else if (e.Column.FieldName.Contains("ORDERAPPROVEDPCS"))
                        StrOpe = "Order Approved";


                    //if (RbtnAll.Checked == true)
                    //{
                    //    StrDetail = "ALL";
                    //}
                    //else if (RbtnPurchase.Checked == true)
                    //{
                    //    StrDetail = "PURCHASE";
                    //}
                    //else
                    //{
                    //    StrDetail = "MFG";
                    //}
                    StrDetail = "ALL";
                    StrShape = Val.Trim(CmbShape.Properties.GetCheckedItems()); ;
                    FromCarat = Val.Val(txtFromCarat.Text);
                    ToCarat = Val.Val(txtToCarat.Text);
                    StrInvoiceNo = Val.ToString(txtInvoiceNo.Text);

                    if (RdbOrderApproved.Checked == true)
                        IsOrderApproved = true;
                    else
                        IsOrderApproved = false;

                    FrmPopupSearchBox.mDTab = ObjMemo.GetOpeningClosingReportDetail(StrOpe, StrDate, StrDetail, StrShape,FromCarat, ToCarat, StrInvoiceNo, IsOrderApproved);
                    FrmPopupSearchBox.mStrColumnsToHide = "Stock_ID";
                    FrmPopupSearchBox.Height = 619;
                    FrmPopupSearchBox.Width = 838;
                    
                    this.Cursor = Cursors.Default;
                    FrmPopupSearchBox.ShowDialog();
                    //FrmPopupSearchBox.GrdDet.BestFitColumns();
                    e.Handled = true;
                    if (FrmPopupSearchBox.DRow != null)
                    {

                    }
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
                DTabDetail = ObjMemo.GetOpeningClosingReport("SINGLE", mStrFromDate, mStrToDate,StrDetail,StrShape,FromCarat,ToCarat,StrInvoiceNo, IsOrderApproved);
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

                //RdbOrderApproved.Checked = true;
                //RdbOrderApproved_CheckedChanged(sender, e);

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

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "UPLOADPCS", GrdDet.Columns["UPLOADPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "UPLOADCARAT", GrdDet.Columns["UPLOADCARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "UPLOADAMOUNT", GrdDet.Columns["UPLOADAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "UPLOADSALEAMOUNT", GrdDet.Columns["UPLOADSALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "UPLOADEXPAMOUNT", GrdDet.Columns["UPLOADEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "PURCHASEPCS", GrdDet.Columns["PURCHASEPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "PURCHASECARAT", GrdDet.Columns["PURCHASECARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "PURCHASEAMOUNT", GrdDet.Columns["PURCHASEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "PURCHASESALEAMOUNT", GrdDet.Columns["PURCHASESALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "PURCHASEEXPAMOUNT", GrdDet.Columns["PURCHASEEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "PURCHASERETURNPCS", GrdDet.Columns["PURCHASERETURNPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "PURCHASERETURNCARAT", GrdDet.Columns["PURCHASERETURNCARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "PURCHASERETURNAMOUNT", GrdDet.Columns["PURCHASERETURNAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "PURCHASERETURNSALEAMOUNT", GrdDet.Columns["PURCHASERETURNSALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "PURCHASERETURNEXPAMOUNT", GrdDet.Columns["PURCHASERETURNEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "MEMOISSUEPCS", GrdDet.Columns["MEMOISSUEPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "MEMOISSUECARAT", GrdDet.Columns["MEMOISSUECARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "MEMOISSUEAMOUNT", GrdDet.Columns["MEMOISSUEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "MEMOISSUESALEAMOUNT", GrdDet.Columns["MEMOISSUESALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "MEMOISSUEEXPAMOUNT", GrdDet.Columns["MEMOISSUEEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "MEMORETURNPCS", GrdDet.Columns["MEMORETURNPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "MEMORETURNCARAT", GrdDet.Columns["MEMORETURNCARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "MEMORETURNAMOUNT", GrdDet.Columns["MEMORETURNAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "MEMORETURNSALEAMOUNT", GrdDet.Columns["MEMORETURNSALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "MEMORETURNEXPAMOUNT", GrdDet.Columns["MEMORETURNEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "LABISSUEPCS", GrdDet.Columns["LABISSUEPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "LABISSUECARAT", GrdDet.Columns["LABISSUECARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "LABISSUEAMOUNT", GrdDet.Columns["LABISSUEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "LABISSUESALEAMOUNT", GrdDet.Columns["LABISSUESALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "LABISSUEEXPAMOUNT", GrdDet.Columns["LABISSUEEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "LABRETURNPCS", GrdDet.Columns["LABRETURNPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "LABRETURNCARAT", GrdDet.Columns["LABRETURNCARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "LABRETURNAMOUNT", GrdDet.Columns["LABRETURNAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "LABRETURNSALEAMOUNT", GrdDet.Columns["LABRETURNSALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "LABRETURNEXPAMOUNT", GrdDet.Columns["LABRETURNEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "CONSISSUEPCS", GrdDet.Columns["CONSISSUEPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "CONSISSUECARAT", GrdDet.Columns["CONSISSUECARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "CONSISSUEAMOUNT", GrdDet.Columns["CONSISSUEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "CONSISSUESALEAMOUNT", GrdDet.Columns["CONSISSUESALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "CONSISSUEEXPAMOUNT", GrdDet.Columns["CONSISSUEEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "CONSRETURNPCS", GrdDet.Columns["CONSRETURNPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "CONSRETURNCARAT", GrdDet.Columns["CONSRETURNCARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "CONSRETURNAMOUNT", GrdDet.Columns["CONSRETURNAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "CONSRETURNSALEAMOUNT", GrdDet.Columns["CONSRETURNSALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "CONSRETURNEXPAMOUNT", GrdDet.Columns["CONSRETURNEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "ORDERCONFIRMPCS", GrdDet.Columns["ORDERCONFIRMPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "ORDERCONFIRMCARAT", GrdDet.Columns["ORDERCONFIRMCARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "ORDERCONFIRMAMOUNT", GrdDet.Columns["ORDERCONFIRMAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "ORDERCONFIRMSALEAMOUNT", GrdDet.Columns["ORDERCONFIRMSALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "ORDERCONFIRMEXPAMOUNT", GrdDet.Columns["ORDERCONFIRMEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "ORDERRETURNPCS", GrdDet.Columns["ORDERRETURNPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "ORDERRETURNCARAT", GrdDet.Columns["ORDERRETURNCARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "ORDERRETURNAMOUNT", GrdDet.Columns["ORDERRETURNAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "ORDERRETURNSALEAMOUNT", GrdDet.Columns["ORDERRETURNSALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "ORDERRETURNEXPAMOUNT", GrdDet.Columns["ORDERRETURNEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "SALESDELIVERYPCS", GrdDet.Columns["SALESDELIVERYPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "SALESDELIVERYCARAT", GrdDet.Columns["SALESDELIVERYCARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "SALESDELIVERYAMOUNT", GrdDet.Columns["SALESDELIVERYAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "SALESDELIVERYSALEAMOUNT", GrdDet.Columns["SALESDELIVERYSALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "SALESDELIVERYEXPAMOUNT", GrdDet.Columns["SALESDELIVERYEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "SALESRETURNPCS", GrdDet.Columns["SALESRETURNPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "SALESRETURNCARAT", GrdDet.Columns["SALESRETURNCARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "SALESRETURNAMOUNT", GrdDet.Columns["SALESRETURNAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "SALESRETURNSALEAMOUNT", GrdDet.Columns["SALESRETURNSALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "SALESRETURNEXPAMOUNT", GrdDet.Columns["SALESRETURNEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "FACTORYGRADINGPCS", GrdDet.Columns["FACTORYGRADINGPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "FACTORYGRADINGCARAT", GrdDet.Columns["FACTORYGRADINGCARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "FACTORYGRADINGAMOUNT", GrdDet.Columns["FACTORYGRADINGAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "FACTORYGRADINGSALEAMOUNT", GrdDet.Columns["FACTORYGRADINGSALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "FACTORYGRADINGEXPAMOUNT", GrdDet.Columns["FACTORYGRADINGEXPAMOUNT"]);

                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "ORDERAPPROVEDPCS", GrdDet.Columns["ORDERAPPROVEDPCS"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "ORDERAPPROVEDCARAT", GrdDet.Columns["ORDERAPPROVEDCARAT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "ORDERAPPROVEDAMOUNT", GrdDet.Columns["ORDERAPPROVEDAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "ORDERAPPROVEDSALEAMOUNT", GrdDet.Columns["ORDERAPPROVEDSALEAMOUNT"]);
                    GrdDet.GroupSummary.Add(SummaryItemType.Sum, "ORDERAPPROVEDEXPAMOUNT", GrdDet.Columns["ORDERAPPROVEDEXPAMOUNT"]);

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
            GrdDet.Columns["UPLOADSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["PURCHASESALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["PURCHASERETURNSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["MEMOISSUESALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["MEMORETURNSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["LABISSUESALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["LABRETURNSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["CONSISSUESALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["CONSRETURNSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["ORDERCONFIRMSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["ORDERRETURNSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["SALESDELIVERYSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["SALESRETURNSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["CLOSINGSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["DEPARTMENTTRANSFERSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["MIXTOSINGLESALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["SINGLETOPARCELSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["FACTORYGRADINGSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
            GrdDet.Columns["ORDERAPPROVEDSALEAMOUNT"].Visible = ChkSaleAmt.Checked;
        }

        private void ChkExpAmt_CheckedChanged(object sender, EventArgs e)
        {
            GrdDet.Columns["OPENINGEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["UPLOADEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["PURCHASEEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["PURCHASERETURNEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["MEMOISSUEEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["MEMORETURNEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["LABISSUEEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["LABRETURNEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["CONSISSUEEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["CONSRETURNEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["ORDERCONFIRMEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["ORDERRETURNEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["SALESDELIVERYEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["SALESRETURNEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["CLOSINGEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["DEPARTMENTTRANSFEREXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["MIXTOSINGLEEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["SINGLETOPARCELEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["FACTORYGRADINGEXPAMOUNT"].Visible = ChkExpAmt.Checked;
            GrdDet.Columns["ORDERAPPROVEDEXPAMOUNT"].Visible = ChkExpAmt.Checked;
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

        private void RdbOrderApproved_CheckedChanged(object sender, EventArgs e)
        {
            //if (RdbOrderApproved.Checked == true)
            //{
            //    for (int i = 0; i < DTabDetail.Rows.Count; i++)
            //    {
            //        double ORDERAPPROVEDPCS = Val.ToDouble(DTabDetail.Rows[i]["ORDERAPPROVEDPCS"].ToString());
            //        double ORDERAPPROVEDCARAT = Val.ToDouble(DTabDetail.Rows[i]["ORDERAPPROVEDCARAT"].ToString());
            //        double ORDERAPPROVEDAMOUNT = Val.ToDouble(DTabDetail.Rows[i]["ORDERAPPROVEDAMOUNT"].ToString());
            //        double ORDERAPPROVEDRATE = Val.ToDouble(DTabDetail.Rows[i]["ORDERAPPROVEDRATE"].ToString());
            //        double ORDERAPPROVEDSALEAMOUNT = Val.ToDouble(DTabDetail.Rows[i]["ORDERAPPROVEDSALEAMOUNT"].ToString());
            //        double ORDERAPPROVEDEXPAMOUNT = Val.ToDouble(DTabDetail.Rows[i]["ORDERAPPROVEDEXPAMOUNT"].ToString());

            //        double CLOSINGPCS = Val.ToDouble(DTabDetail.Rows[i]["CLOSINGPCS"].ToString());
            //        double CLOSINGCARAT = Val.ToDouble(DTabDetail.Rows[i]["CLOSINGCARAT"].ToString());
            //        double CLOSINGAMOUNT = Val.ToDouble(DTabDetail.Rows[i]["CLOSINGAMOUNT"].ToString());
            //        double CLOSINGRATE = Val.ToDouble(DTabDetail.Rows[i]["CLOSINGRATE"].ToString());
            //        double CLOSINGSALEAMOUNT = Val.ToDouble(DTabDetail.Rows[i]["CLOSINGSALEAMOUNT"].ToString());
            //        double CLOSINGEXPAMOUNT = Val.ToDouble(DTabDetail.Rows[i]["CLOSINGEXPAMOUNT"].ToString());

            //        DTabDetail.Rows[i]["CLOSINGPCS"] = CLOSINGPCS + ORDERAPPROVEDPCS;
            //        DTabDetail.Rows[i]["CLOSINGCARAT"] = CLOSINGCARAT + ORDERAPPROVEDCARAT;
            //        DTabDetail.Rows[i]["CLOSINGAMOUNT"] = CLOSINGAMOUNT + ORDERAPPROVEDAMOUNT;
            //        DTabDetail.Rows[i]["CLOSINGRATE"] = CLOSINGRATE + ORDERAPPROVEDRATE;
            //        DTabDetail.Rows[i]["CLOSINGSALEAMOUNT"] = CLOSINGSALEAMOUNT + ORDERAPPROVEDSALEAMOUNT;
            //        DTabDetail.Rows[i]["CLOSINGEXPAMOUNT"] = CLOSINGEXPAMOUNT + ORDERAPPROVEDEXPAMOUNT;

            //        DTabDetail.AcceptChanges();
            //    }
            //}
            //else
            //{
            //    for (int j = 0; j < DTabDetail.Rows.Count; j++)
            //    {
            //        double ORDERAPPROVEDPCS = Val.ToDouble(DTabDetail.Rows[j]["ORDERAPPROVEDPCS"].ToString());
            //        double ORDERAPPROVEDCARAT = Val.ToDouble(DTabDetail.Rows[j]["ORDERAPPROVEDCARAT"].ToString());
            //        double ORDERAPPROVEDAMOUNT = Val.ToDouble(DTabDetail.Rows[j]["ORDERAPPROVEDAMOUNT"].ToString());
            //        double ORDERAPPROVEDRATE = Val.ToDouble(DTabDetail.Rows[j]["ORDERAPPROVEDRATE"].ToString());
            //        double ORDERAPPROVEDSALEAMOUNT = Val.ToDouble(DTabDetail.Rows[j]["ORDERAPPROVEDSALEAMOUNT"].ToString());
            //        double ORDERAPPROVEDEXPAMOUNT = Val.ToDouble(DTabDetail.Rows[j]["ORDERAPPROVEDEXPAMOUNT"].ToString());

            //        double CLOSINGPCS = Val.ToDouble(DTabDetail.Rows[j]["CLOSINGPCS"].ToString());
            //        double CLOSINGCARAT = Val.ToDouble(DTabDetail.Rows[j]["CLOSINGCARAT"].ToString());
            //        double CLOSINGAMOUNT = Val.ToDouble(DTabDetail.Rows[j]["CLOSINGAMOUNT"].ToString());
            //        double CLOSINGRATE = Val.ToDouble(DTabDetail.Rows[j]["CLOSINGRATE"].ToString());
            //        double CLOSINGSALEAMOUNT = Val.ToDouble(DTabDetail.Rows[j]["CLOSINGSALEAMOUNT"].ToString());
            //        double CLOSINGEXPAMOUNT = Val.ToDouble(DTabDetail.Rows[j]["CLOSINGEXPAMOUNT"].ToString());

            //        DTabDetail.Rows[j]["CLOSINGPCS"] = CLOSINGPCS - ORDERAPPROVEDPCS;
            //        DTabDetail.Rows[j]["CLOSINGCARAT"] = CLOSINGCARAT - ORDERAPPROVEDCARAT;
            //        DTabDetail.Rows[j]["CLOSINGAMOUNT"] = CLOSINGAMOUNT - ORDERAPPROVEDAMOUNT;
            //        DTabDetail.Rows[j]["CLOSINGRATE"] = CLOSINGRATE - ORDERAPPROVEDRATE;
            //        DTabDetail.Rows[j]["CLOSINGSALEAMOUNT"] = CLOSINGSALEAMOUNT - ORDERAPPROVEDSALEAMOUNT;
            //        DTabDetail.Rows[j]["CLOSINGEXPAMOUNT"] = CLOSINGEXPAMOUNT - ORDERAPPROVEDEXPAMOUNT;

            //        DTabDetail.AcceptChanges();
            //    }
            //}
        }

        private void btnNewReport_Click(object sender, EventArgs e)
        {
            try
            {
                string StrSummryFormat = "";
                StrSummryFormat = "{0:N0}";

                PnlSerch.Visible = true;
                PnlMain.Visible = false;
                PnlSerch.AutoSize = true;

                cLabel1.Visible = false;
                CmbShape.Visible = false;
                CmbShape.Visible = false;
                txtFromCarat.Visible = false;
                cLabel3.Visible = false;
                txtToCarat.Visible = false;
                ChkCostAmt.Visible = false;
                ChkPcs.Visible = false;
                ChkCts.Visible = false;
                ChkRate.Visible = false;
                ChkAmt.Visible = false;
                cLabel4.Visible = false;
                txtInvoiceNo.Visible = false;
                ChkSaleAmt.Visible = false;
                ChkExpAmt.Visible = false;
                ChkDiffPer.Visible = false;
                RdbOrderApproved.Visible = false;
                RdbSales.Visible = false;
                cLabel2.Visible = false;

                PnlSerch.BringToFront();
                PnlMain.SendToBack();


                mStrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
                mStrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());

                DTabDetail = ObjMemo.GetOpeningClosingReportNew(mStrFromDate, mStrToDate);

                if (DTabDetail.Rows.Count <= 0)
                {
                    Global.Message("No Data Found..");
                }
                
                MainGrdSummary.DataSource = DTabDetail;
                GrdSummary.ColumnPanelRowHeight = 30;
                GrdSummary.RowHeight = 30;
                GrdSummary.CollapseAllGroups();

                GrdSummary.Columns["PURCHASE PCS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["PURCHASE PCS"].DisplayFormat.FormatString = "{0:N0}";
                GrdSummary.Columns["PURCHASE PCS"].Summary.Add(SummaryItemType.Custom, "PURCHASE PCS", "{0:N0}");

                GrdSummary.Columns["PURCHASE CTS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["PURCHASE CTS"].DisplayFormat.FormatString = "{0:N3}";
                GrdSummary.Columns["PURCHASE CTS"].Summary.Add(SummaryItemType.Custom, "PURCHASE CTS", "{0:N3}");

                GrdSummary.Columns["PURCHASERETURN PCS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["PURCHASERETURN PCS"].DisplayFormat.FormatString = "{0:N0}";
                GrdSummary.Columns["PURCHASERETURN PCS"].Summary.Add(SummaryItemType.Custom, "PURCHASERETURN PCS", "{0:N0}");

                GrdSummary.Columns["PURCHASERETURN CTS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["PURCHASERETURN CTS"].DisplayFormat.FormatString = "{0:N3}";
                GrdSummary.Columns["PURCHASERETURN CTS"].Summary.Add(SummaryItemType.Custom, "PURCHASERETURN CTS", "{0:N3}");

                GrdSummary.Columns["LABISSUE PCS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["LABISSUE PCS"].DisplayFormat.FormatString = "{0:N0}";
                GrdSummary.Columns["LABISSUE PCS"].Summary.Add(SummaryItemType.Custom, "LABISSUE PCS", "{0:N0}");

                GrdSummary.Columns["LABISSUE CTS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["LABISSUE CTS"].DisplayFormat.FormatString = "{0:N3}";
                GrdSummary.Columns["LABISSUE CTS"].Summary.Add(SummaryItemType.Custom, "LABISSUE CTS", "{0:N3}");

                GrdSummary.Columns["LABRETURN PCS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["LABRETURN PCS"].DisplayFormat.FormatString = "{0:N0}";
                GrdSummary.Columns["LABRETURN PCS"].Summary.Add(SummaryItemType.Custom, "LABRETURN PCS", "{0:N0}");

                GrdSummary.Columns["LABRETURN CTS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["LABRETURN CTS"].DisplayFormat.FormatString = "{0:N3}";
                GrdSummary.Columns["LABRETURN CTS"].Summary.Add(SummaryItemType.Custom, "LABRETURN CTS", "{0:N3}");

                GrdSummary.Columns["ORDERCONFIRMAPPROVED PCS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["ORDERCONFIRMAPPROVED PCS"].DisplayFormat.FormatString = "{0:N0}";
                GrdSummary.Columns["ORDERCONFIRMAPPROVED PCS"].Summary.Add(SummaryItemType.Custom, "ORDERCONFIRMAPPROVED PCS", "{0:N0}");

                GrdSummary.Columns["ORDERNOTCONFIRMAPPROVED PCS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["ORDERNOTCONFIRMAPPROVED PCS"].DisplayFormat.FormatString = "{0:N0}";
                GrdSummary.Columns["ORDERNOTCONFIRMAPPROVED PCS"].Summary.Add(SummaryItemType.Custom, "ORDERNOTCONFIRMAPPROVED PCS", "{0:N0}");

                GrdSummary.Columns["ORDERNOTCONFIRMAPPROVED CTS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["ORDERNOTCONFIRMAPPROVED CTS"].DisplayFormat.FormatString = "{0:N3}";
                GrdSummary.Columns["ORDERNOTCONFIRMAPPROVED CTS"].Summary.Add(SummaryItemType.Custom, "ORDERNOTCONFIRMAPPROVED CTS", "{0:N3}");

                GrdSummary.Columns["ORDERCONFIRMRETURN PCS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["ORDERCONFIRMRETURN PCS"].DisplayFormat.FormatString = "{0:N0}";
                GrdSummary.Columns["ORDERCONFIRMRETURN PCS"].Summary.Add(SummaryItemType.Custom, "ORDERCONFIRMRETURN PCS", "{0:N0}");

                GrdSummary.Columns["ORDERCONFIRMRETURN CTS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["ORDERCONFIRMRETURN CTS"].DisplayFormat.FormatString = "{0:N3}";
                GrdSummary.Columns["ORDERCONFIRMRETURN CTS"].Summary.Add(SummaryItemType.Custom, "ORDERCONFIRMRETURN CTS", "{0:N3}");

                GrdSummary.Columns["CONSIGNMENTISSUE PCS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["CONSIGNMENTISSUE PCS"].DisplayFormat.FormatString = "{0:N0}";
                GrdSummary.Columns["CONSIGNMENTISSUE PCS"].Summary.Add(SummaryItemType.Custom, "CONSIGNMENTISSUE PCS", "{0:N0}");

                GrdSummary.Columns["CONSIGNMENTISSUE CTS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["CONSIGNMENTISSUE CTS"].DisplayFormat.FormatString = "{0:N3}";
                GrdSummary.Columns["CONSIGNMENTISSUE CTS"].Summary.Add(SummaryItemType.Custom, "CONSIGNMENTISSUE CTS", "{0:N3}");

                GrdSummary.Columns["CONSIGNMENTRETURN PCS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["CONSIGNMENTRETURN PCS"].DisplayFormat.FormatString = "{0:N0}";
                GrdSummary.Columns["CONSIGNMENTRETURN PCS"].Summary.Add(SummaryItemType.Custom, "CONSIGNMENTRETURN PCS", "{0:N0}");

                GrdSummary.Columns["CONSIGNMENTRETURN CTS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["CONSIGNMENTRETURN CTS"].DisplayFormat.FormatString = "{0:N3}";
                GrdSummary.Columns["CONSIGNMENTRETURN CTS"].Summary.Add(SummaryItemType.Custom, "CONSIGNMENTRETURN CTS", "{0:N3}");

                GrdSummary.Columns["SALEDELIVERY PCS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["SALEDELIVERY PCS"].DisplayFormat.FormatString = "{0:N0}";
                GrdSummary.Columns["SALEDELIVERY PCS"].Summary.Add(SummaryItemType.Custom, "SALEDELIVERY PCS", "{0:N0}");

                GrdSummary.Columns["SALEDELIVERY CTS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["SALEDELIVERY CTS"].DisplayFormat.FormatString = "{0:N3}";
                GrdSummary.Columns["SALEDELIVERY CTS"].Summary.Add(SummaryItemType.Custom, "SALEDELIVERY CTS", "{0:N3}");

                GrdSummary.Columns["SALEDELIVERYRETURN PCS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["SALEDELIVERYRETURN PCS"].DisplayFormat.FormatString = "{0:N0}";
                GrdSummary.Columns["SALEDELIVERYRETURN PCS"].Summary.Add(SummaryItemType.Custom, "SALEDELIVERYRETURN PCS", "{0:N0}");

                GrdSummary.Columns["SALEDELIVERYRETURN CTS"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                GrdSummary.Columns["SALEDELIVERYRETURN CTS"].DisplayFormat.FormatString = "{0:N3}";
                GrdSummary.Columns["SALEDELIVERYRETURN CTS"].Summary.Add(SummaryItemType.Custom, "SALEDELIVERYRETURN CTS", "{0:N3}");


                if (GrdSummary.GroupSummary.Count == 0)
                {
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "PURCHASE PCS", GrdSummary.Columns["PURCHASE PCS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "PURCHASE CTS", GrdSummary.Columns["PURCHASE CTS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "PURCHASERETURN PCS", GrdSummary.Columns["PURCHASERETURN PCS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "PURCHASERETURN CTS", GrdSummary.Columns["PURCHASERETURN CTS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "LABISSUE PCS", GrdSummary.Columns["LABISSUE PCS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "LABISSUE CTS", GrdSummary.Columns["LABISSUE CTS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "LABRETURN PCS", GrdSummary.Columns["LABRETURN PCS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "LABRETURN CTS", GrdSummary.Columns["LABRETURN CTS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "ORDERCONFIRMAPPROVED PCS", GrdSummary.Columns["ORDERCONFIRMAPPROVED PCS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "ORDERCONFIRMAPPROVED CTS", GrdSummary.Columns["ORDERCONFIRMAPPROVED CTS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "ORDERNOTCONFIRMAPPROVED PCS", GrdSummary.Columns["ORDERNOTCONFIRMAPPROVED PCS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "ORDERNOTCONFIRMAPPROVED CTS", GrdSummary.Columns["ORDERNOTCONFIRMAPPROVED CTS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "ORDERCONFIRMRETURN PCS", GrdSummary.Columns["ORDERCONFIRMRETURN PCS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "ORDERCONFIRMRETURN CTS", GrdSummary.Columns["ORDERCONFIRMRETURN CTS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "CONSIGNMENTISSUE PCS", GrdSummary.Columns["CONSIGNMENTISSUE PCS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "CONSIGNMENTISSUE CTS", GrdSummary.Columns["CONSIGNMENTISSUE CTS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "CONSIGNMENTRETURN PCS", GrdSummary.Columns["CONSIGNMENTRETURN PCS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "CONSIGNMENTRETURN CTS", GrdSummary.Columns["CONSIGNMENTRETURN CTS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "SALEDELIVERY PCS", GrdSummary.Columns["SALEDELIVERY PCS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "SALEDELIVERY CTS", GrdSummary.Columns["SALEDELIVERY CTS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "SALEDELIVERYRETURN PCS", GrdSummary.Columns["SALEDELIVERYRETURN PCS"]);
                    GrdSummary.GroupSummary.Add(SummaryItemType.Custom, "SALEDELIVERYRETURN CTS", GrdSummary.Columns["SALEDELIVERYRETURN CTS"]);
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdSummary_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                if (e.Clicks == 2)
                {
                    string mStrDesc = Val.ToString(e.Column.FieldName);
                    string mStrDate = Val.SqlDate(Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "TRNDATE")));

                    DT = ObjMemo.GetOpeningClosingReportDetailNew(mStrDate, mStrDesc);

                    FrmSearchPopupBox FrmPopupSearchBox = new FrmSearchPopupBox();
                    FrmPopupSearchBox.mStrSearchField = "StockNo,CTS,PCS";
                    FrmPopupSearchBox.mStrSearchText = "";
                    this.Cursor = Cursors.WaitCursor;
                    string StrOpe = string.Empty;

                    StrDetail = "ALL";
                    StrShape = Val.Trim(CmbShape.Properties.GetCheckedItems()); ;
                    FromCarat = Val.Val(txtFromCarat.Text);
                    ToCarat = Val.Val(txtToCarat.Text);
                    StrInvoiceNo = Val.ToString(txtInvoiceNo.Text);

                    if (RdbOrderApproved.Checked == true)
                        IsOrderApproved = true;
                    else
                        IsOrderApproved = false;

                    FrmPopupSearchBox.mDTab = DT;
                    FrmPopupSearchBox.mStrColumnsToHide = "Addles,DESCRIPTION";
                    FrmPopupSearchBox.Height = 619;
                    FrmPopupSearchBox.Width = 838;

                    this.Cursor = Cursors.Default;
                    FrmPopupSearchBox.ShowDialog();
                    //FrmPopupSearchBox.GrdDet.BestFitColumns();
                    e.Handled = true;
                    if (FrmPopupSearchBox.DRow != null)
                    {

                    }
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

        private void GrdSummary_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            try
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    DouPURCHASEPCS = 0;
                    DouPURCHASECTS = 0;
                    DouPURCHASERETURNPCS = 0;
                    DouPURCHASERETURNCTS = 0;
                    DouLABISSUEPCS = 0;
                    DouLABISSUECTS = 0;
                    DouLABRETURNPCS = 0;
                    DouLABRETURNCTS = 0;
                    DouORDERCONFIRMAPPROVEDPCS = 0;
                    DouORDERCONFIRMAPPROVEDCTS = 0;
                    DouORDERNOTCONFIRMAPPROVEDPCS = 0;
                    DouORDERNOTCONFIRMAPPROVEDCTS = 0;
                    DouORDERCONFIRMRETURNPCS = 0;
                    DouORDERCONFIRMRETURNCTS = 0;
                    DouCONSIGNMENTISSUEPCS = 0;
                    DouCONSIGNMENTISSUECTS = 0;
                    DouCONSIGNMENTRETURNPCS = 0;
                    DouCONSIGNMENTRETURNCTS = 0;
                    DouSALEDELIVERYPCS = 0;
                    DouSALEDELIVERYCTS = 0;
                    DouSALEDELIVERYRETURNPCS = 0;
                    DouSALEDELIVERYRETURNCTS = 0;
                }
                else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DouPURCHASEPCS = DouPURCHASEPCS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "PURCHASE PCS"));
                    DouPURCHASECTS = DouPURCHASECTS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "PURCHASE CTS"));

                    DouPURCHASERETURNPCS = DouPURCHASERETURNPCS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "PURCHASERETURN PCS"));
                    DouPURCHASERETURNCTS = DouPURCHASERETURNCTS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "PURCHASERETURN CTS"));

                    DouLABISSUEPCS = DouLABISSUEPCS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "LABISSUE PCS"));
                    DouLABISSUECTS = DouLABISSUECTS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "LABISSUE CTS"));

                    DouLABRETURNPCS = DouLABRETURNPCS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "LABRETURN PCS"));
                    DouLABRETURNCTS = DouLABRETURNCTS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "LABRETURN CTS"));

                    DouORDERCONFIRMAPPROVEDPCS = DouORDERCONFIRMAPPROVEDPCS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "ORDERCONFIRMAPPROVED PCS"));
                    DouORDERCONFIRMAPPROVEDCTS = DouORDERCONFIRMAPPROVEDCTS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "ORDERCONFIRMAPPROVED CTS"));

                    DouORDERNOTCONFIRMAPPROVEDPCS = DouORDERNOTCONFIRMAPPROVEDPCS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "ORDERNOTCONFIRMAPPROVED PCS"));
                    DouORDERNOTCONFIRMAPPROVEDCTS = DouORDERNOTCONFIRMAPPROVEDCTS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "ORDERNOTCONFIRMAPPROVED CTS"));

                    DouORDERCONFIRMRETURNPCS = DouORDERCONFIRMRETURNPCS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "ORDERCONFIRMRETURN PCS"));
                    DouORDERCONFIRMRETURNCTS = DouORDERCONFIRMRETURNCTS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "ORDERCONFIRMRETURN CTS"));

                    DouCONSIGNMENTISSUEPCS = DouCONSIGNMENTISSUEPCS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "CONSIGNMENTISSUE PCS"));
                    DouCONSIGNMENTISSUECTS = DouCONSIGNMENTISSUECTS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "CONSIGNMENTISSUE CTS"));

                    DouCONSIGNMENTRETURNPCS = DouCONSIGNMENTRETURNPCS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "CONSIGNMENTRETURN PCS"));
                    DouCONSIGNMENTRETURNCTS = DouCONSIGNMENTRETURNCTS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "CONSIGNMENTRETURN CTS"));

                    DouSALEDELIVERYPCS = DouSALEDELIVERYPCS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "SALEDELIVERY PCS"));
                    DouSALEDELIVERYCTS = DouSALEDELIVERYCTS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "SALEDELIVERY CTS"));

                    DouSALEDELIVERYRETURNPCS = DouSALEDELIVERYRETURNPCS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "SALEDELIVERYRETURN PCS"));
                    DouSALEDELIVERYRETURNCTS = DouSALEDELIVERYRETURNCTS + Val.Val(GrdSummary.GetRowCellValue(e.RowHandle, "SALEDELIVERYRETURN CTS"));

                }

                else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("PURCHASE PCS") == 0)
                    {
                        e.TotalValue = DouPURCHASEPCS;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("PURCHASE CTS") == 0)
                    {
                        e.TotalValue = DouPURCHASECTS;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("PURCHASERETURN PCS") == 0)
                    {
                        e.TotalValue = DouPURCHASERETURNPCS;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("PURCHASERETURN CTS") == 0)
                    {
                        e.TotalValue = DouPURCHASERETURNCTS;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("LABISSUE PCS") == 0)
                    {
                        e.TotalValue = DouLABISSUEPCS;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("LABISSUE CTS") == 0)
                    {
                        e.TotalValue = DouLABISSUECTS;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("LABISSUE PCS") == 0)
                    {
                        e.TotalValue = DouLABRETURNPCS;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("LABISSUE CTS") == 0)
                    {
                        e.TotalValue = DouLABRETURNCTS;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("ORDERCONFIRMAPPROVED PCS") == 0)
                    {
                        e.TotalValue = DouORDERCONFIRMAPPROVEDPCS;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("ORDERCONFIRMAPPROVED CTS") == 0)
                    {
                        e.TotalValue = DouORDERCONFIRMAPPROVEDCTS;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("ORDERNOTCONFIRMAPPROVED PCS") == 0)
                    {
                        e.TotalValue = DouORDERNOTCONFIRMAPPROVEDPCS;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("ORDERNOTCONFIRMAPPROVED CTS") == 0)
                    {
                        e.TotalValue = DouORDERNOTCONFIRMAPPROVEDCTS;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("ORDERCONFIRMRETURN PCS") == 0)
                    {
                        e.TotalValue = DouORDERCONFIRMRETURNPCS;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("ORDERCONFIRMRETURN CTS") == 0)
                    {
                        e.TotalValue = DouORDERCONFIRMRETURNCTS;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("CONSIGNMENTISSUE PCS") == 0)
                    {
                        e.TotalValue = DouCONSIGNMENTISSUEPCS;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("CONSIGNMENTISSUE CTS") == 0)
                    {
                        e.TotalValue = DouCONSIGNMENTISSUECTS;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("CONSIGNMENTRETURN PCS") == 0)
                    {
                        e.TotalValue = DouCONSIGNMENTRETURNPCS;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("CONSIGNMENTRETURN CTS") == 0)
                    {
                        e.TotalValue = DouCONSIGNMENTRETURNCTS;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("SALEDELIVERY PCS") == 0)
                    {
                        e.TotalValue = DouSALEDELIVERYPCS;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("SALEDELIVERY CTS") == 0)
                    {
                        e.TotalValue = DouSALEDELIVERYCTS;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("SALEDELIVERYRETURN PCS") == 0)
                    {
                        e.TotalValue = DouSALEDELIVERYRETURNPCS;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("SALEDELIVERYRETURN CTS") == 0)
                    {
                        e.TotalValue = DouSALEDELIVERYRETURNCTS;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
    }	
}
