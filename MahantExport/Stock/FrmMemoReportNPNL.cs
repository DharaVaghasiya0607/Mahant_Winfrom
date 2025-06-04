using MahantExport.Parcel;
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

namespace MahantExport.Masters
{
    public partial class FrmMemoReportNPNL : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();
        BOFormPer ObjPer = new BOFormPer();
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


        #region Property Settings

        public FrmMemoReportNPNL()
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

            DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            DTPToDate.Value = DateTime.Now;

            DTPFromDate.Focus();
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
            Global.ExcelExport("Memo Detail", GrdDet);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string StrFromDate = "";
                string StrToDate = "";
              
                if (DTPFromDate.Checked)
                {
                    StrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
                }
                if(DTPToDate.Checked)
                {
                    StrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());
                }
                DataTable DTab = ObjMemo.GetMemoReportNPNL(StrFromDate,StrToDate);
                    
                GrdDet.BeginUpdate();
                MainGrid.DataSource = DTab;
                GrdDet.BestFitColumns();
                GrdDet.EndUpdate();

                this.Cursor = Cursors.Default;


            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDetail_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            /*
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
                    DouWebSiteRapAmount = DouWebSiteRapAmount + (Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "WEBSITERAPAPORT"))*Cts);
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
            catch(Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
            */
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

        private void GrdSummary_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                return;
            }
            if (e.Clicks == 2 && e.Column.FieldName == "JANGEDNOSTR")
            {
                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                FrmMemoEntry.ShowForm(Val.ToString(GrdDet.GetRowCellValue(e.RowHandle, "MEMO_ID")));
            }
        }
        private void FrmMemoEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            BtnSearch.PerformClick();
        }

        private void GrdDet_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "COSTDIFF" ||
                e.Column.FieldName == "FCOSTDIFF" ||
                e.Column.FieldName == "SALEDIFF" ||
                e.Column.FieldName == "FSALEDIFF" ||
                e.Column.FieldName == "EXPDIFF" ||
                e.Column.FieldName == "FEXPEDIFF" ||
                e.Column.FieldName == "COSTPER" ||
                e.Column.FieldName == "SALEPER" ||
                e.Column.FieldName == "EXPPER"
                )
            {
                if (Val.Val(e.CellValue) < 0)
                {
                    e.Appearance.ForeColor = Color.FromArgb(192, 0, 0);
                }
                else
                {
                    e.Appearance.ForeColor = Color.DarkGreen;
                }
            }
        }

        private void GrdDet_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Clicks == 2 && e.Column.FieldName == "JANGEDNOSTR")
            {
                this.Cursor = Cursors.WaitCursor;
                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                FrmMemoEntry.ShowForm(Val.ToString(GrdDet.GetRowCellValue(e.RowHandle, "MEMO_ID")));
                this.Cursor = Cursors.Default;
            }
            if (e.Clicks == 2 && e.Column.FieldName == "STOCKNO")
            {
                this.Cursor = Cursors.WaitCursor;
                FrmStoneHistory FrmStoneHistory = new FrmStoneHistory();
                FrmStoneHistory.MdiParent = Global.gMainRef;
                FrmStoneHistory.ShowForm(Val.ToString(GrdDet.GetRowCellValue(e.RowHandle, "STOCK_ID")), Val.ToString(GrdDet.GetRowCellValue(e.RowHandle, "STOCKNO")), Stock.FrmStoneHistory.FORMTYPE.DISPLAY);
                this.Cursor = Cursors.Default;
            }
        }
    }
}
