using MahantExport.Masters;
using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
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
    public partial class FrmOpeningClosing : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();

        #region Property Settings

        public FrmOpeningClosing()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();           
            this.Show();

            DTPFromDate.Value = DateTime.Now.AddDays(-7);
            DTPToDate.Value = DateTime.Now;
            DTPFromDate.Focus();
            ChkPcs_CheckedChanged(null, null);
            ChkCts_CheckedChanged(null, null);
            ChkRate_CheckedChanged(null, null);
            ChkAmt_CheckedChanged(null, null);
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
                this.Cursor = Cursors.WaitCursor;
                
                string StrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
                string StrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());
                string StrOpe = string.Empty;
                
                DataTable DTab = ObjMemo.GetOpeningClosingReport("SINGLE", StrFromDate, StrToDate);

                MainGrd.DataSource = DTab;
                MainGrd.Refresh();
                //GrdDet.BestFitColumns();

                this.Cursor = Cursors.Default;
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
            GrdDet.Columns["SALESDELIVERYPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["SALESRETURNPCS"].Visible = ChkPcs.Checked;
            GrdDet.Columns["CLOSINGPCS"].Visible = ChkPcs.Checked;
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
            GrdDet.Columns["SALESDELIVERYCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["SALESRETURNCARAT"].Visible = ChkCts.Checked;
            GrdDet.Columns["CLOSINGCARAT"].Visible = ChkCts.Checked;
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
            GrdDet.Columns["SALESDELIVERYRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["SALESRETURNRATE"].Visible = ChkRate.Checked;
            GrdDet.Columns["CLOSINGRATE"].Visible = ChkRate.Checked;
        
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
            GrdDet.Columns["SALESDELIVERYAMOUNT"].Visible = ChkAmt.Checked;
            GrdDet.Columns["SALESRETURNAMOUNT"].Visible = ChkAmt.Checked;
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
                    e.Graphics.DrawLine(new Pen(Brushes.Black, 1), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.BandsInfo[i].Bounds.Top), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.RowsInfo[info.RowsInfo.Count - 1].Bounds.Bottom - 1));
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

            if (e.Clicks == 2)
            {
                string StrDate = Val.SqlDate(Val.ToString(GrdDet.GetRowCellValue(e.RowHandle, "ENTRYDATE")));

                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "StockNo,Carat,Shape,Color,Clarity,Cut,Pol,Sym,FL,Lab,LabReportNo";
                FrmSearch.mStrSearchText = "";
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
                else if (e.Column.FieldName.Contains("SALESDELIVERY"))
                    StrOpe = "Delivery";
                else if (e.Column.FieldName.Contains("SALESRETURN"))
                    StrOpe = "Delivery Return";
                
                FrmSearch.mDTab = ObjMemo.GetOpeningClosingReportDetail(StrOpe, StrDate);
                FrmSearch.mStrColumnsToHide = "Stock_ID";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.DRow != null)
                {
                   
                }
                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }

        }

    }
}
