using MahantExport.Masters;
using MahantExport.Parcel;
using MahantExport.Stock;
using MahantExport.Utility;
using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.Rapaport;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
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
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MahantExport.CRM
{
    public partial class FrmWebActivityList : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFindRap ObjRap = new BOFindRap();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        BOFormPer ObjPer = new BOFormPer();

        DataTable DTabLogin = new DataTable();
        DataTable DTabcart = new DataTable();
        DataTable DTabwishlist = new DataTable();
        DataTable DTabMostVisited = new DataTable();
        DataTable DTabOrder = new DataTable();
        DataTable DTabSales = new DataTable();
        DataTable DTabRecent = new DataTable();
        DataTable DTabSaved = new DataTable();

        string StrFromDate = "";
        string StrToDate = "";
        string StrParty_ID = "";

        #region Property Settings

        public FrmWebActivityList()
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



            txtLedgerName.Focus();

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
            ObjFormEvent.ObjToDisposeList.Add(ObjStock);
        }

        #endregion

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {

            if (txtLedgerName.Text.Length == 0)
            {
                txtLedgerName.Tag = string.Empty;
            }

            StrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
            StrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());
            StrParty_ID = Val.ToString(txtLedgerName.Tag);

            this.Cursor = Cursors.WaitCursor;

            DataSet DS = ObjStock.GetDataForWebActivity(StrFromDate, StrToDate, StrParty_ID);


            GrdLogIn.BeginUpdate();
            GrdCart.BeginUpdate();
            GrdWishlist.BeginUpdate();
            GrdMostVisitedSummury.BeginUpdate();
            GrdSales.BeginUpdate();
            GrdOrder.BeginUpdate();
            GrdRecent.BeginUpdate();
            GrdSaved.BeginUpdate();


            DTabLogin = DS.Tables[0];
            DTabcart = DS.Tables[1];
            DTabwishlist = DS.Tables[2];
            DTabMostVisited = DS.Tables[3];
            DTabOrder = DS.Tables[4];
            DTabSales = DS.Tables[5];
            DTabRecent = DS.Tables[6];
            DTabSaved = DS.Tables[7];

         
            MainGrdLogin.DataSource = DTabLogin;
            GrdLogIn.RefreshData();

            MainGrdCart.DataSource = DTabcart;
            GrdCart.RefreshData();

            MainGrdWishlist.DataSource = DTabwishlist;
            GrdWishlist.RefreshData();

            MainGrdMostVisited.DataSource = DTabMostVisited;
            GrdVisited.RefreshData();

            MainGridOder.DataSource = DTabOrder;
            GrdOrder.RefreshData();

            MainGrdSales.DataSource = DTabSales;
            GrdSales.RefreshData();

            MainGrdRecent.DataSource = DTabRecent;
            GrdRecent.RefreshData();

            MainGrdSaved.DataSource = DTabSaved;
            GrdSaved.RefreshData();

            GrdLogIn.EndUpdate();
            GrdCart.EndUpdate();
            GrdWishlist.EndUpdate();
            GrdMostVisitedSummury.EndUpdate();
            GrdSales.EndUpdate();
            GrdOrder.EndUpdate();
            GrdRecent.EndUpdate();
            GrdSaved.EndUpdate();

            DataSet DsSummury = ObjStock.GetDataForWebActivitySummury(StrFromDate, StrToDate, StrParty_ID );

            GrdWishlishSummury.BeginUpdate();
            GrdMostVisitedSummury.BeginUpdate();
            GrdCartSummury.BeginUpdate();
            GrdOrderSummury.BeginUpdate();
            GrdSaleSummary.BeginUpdate();

            DataTable DtabWishlishSummury = DsSummury.Tables[0];
            DataTable DtabMostVisitedSummury = DsSummury.Tables[1];
            DataTable DtabCartSummury = DsSummury.Tables[2];
            DataTable DtabOrderSummary = DsSummury.Tables[3];
            DataTable DtabSaleSummary = DsSummury.Tables[4];

            MainGrdWishListSummury.DataSource = DtabWishlishSummury;
            GrdWishlishSummury.RefreshData();

            MainGrdMostVisitedSummury.DataSource = DtabMostVisitedSummury;
            GrdMostVisitedSummury.RefreshData();

            MainGrdCartSummury.DataSource = DtabCartSummury;
            GrdCartSummury.RefreshData();

            MainGrdOrderSummury.DataSource = DtabOrderSummary;
            GrdOrderSummury.RefreshData();

            MainGrdSaleSummury.DataSource = DtabSaleSummary;
            GrdSaleSummary.RefreshData();

            GrdWishlishSummury.EndUpdate();
            GrdMostVisitedSummury.EndUpdate();
            GrdCartSummury.EndUpdate();
            GrdOrderSummury.EndUpdate();
            GrdSaleSummary.EndUpdate();

            this.Cursor = Cursors.Default;

        }

        private void txtLedgerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "LEDGERCODE,LEDGERNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LEDGER);

                    FrmSearch.mStrColumnsToHide = "LEDGER_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtLedgerName.Text = Val.ToString(FrmSearch.DRow["LEDGERNAME"]);
                        txtLedgerName.Tag = Val.ToString(FrmSearch.DRow["LEDGER_ID"]);
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

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtLedgerName.Tag = string.Empty;
            txtLedgerName.Text = string.Empty;
            DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            DTPToDate.Value = DateTime.Now;
            BtnSearch_Click(null, null);
        }

        private void GrdCart_RowCellClick(object sender, RowCellClickEventArgs e)
        {
           
        }

        private void FrmMemoEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            BtnSearch.PerformClick();
        }
       
        private void GrdRecent_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                if (e.Clicks == 2 && e.Column.FieldName == "JANGEDNOSTR")
                {
                    this.Cursor = Cursors.WaitCursor;
                    FrmStoneHistory FrmStoneHistory = new FrmStoneHistory();
                    FrmStoneHistory.MdiParent = Global.gMainRef;
                    FrmStoneHistory.ShowForm(Val.ToString(GrdRecent.GetRowCellValue(e.RowHandle, "STOCK_ID")), Val.ToString(GrdRecent.GetRowCellValue(e.RowHandle, "STOCKNO")), Stock.FrmStoneHistory.FORMTYPE.DISPLAY);
                    this.Cursor = Cursors.Default;
                }

                
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void GrdSaved_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                if (e.Clicks == 2 && e.Column.FieldName == "JANGEDNOSTR")
                {
                    this.Cursor = Cursors.WaitCursor;
                    FrmStoneHistory FrmStoneHistory = new FrmStoneHistory();
                    FrmStoneHistory.MdiParent = Global.gMainRef;
                    FrmStoneHistory.ShowForm(Val.ToString(GrdSaved.GetRowCellValue(e.RowHandle, "STOCK_ID")), Val.ToString(GrdSaved.GetRowCellValue(e.RowHandle, "STOCKNO")), Stock.FrmStoneHistory.FORMTYPE.DISPLAY);
                    this.Cursor = Cursors.Default;
                }

                
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        
        private void GrdVisited_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                if (e.Clicks == 2 && e.Column.FieldName == "JANGEDNOSTR")
                {
                    this.Cursor = Cursors.WaitCursor;
                    FrmStoneHistory FrmStoneHistory = new FrmStoneHistory();
                    FrmStoneHistory.MdiParent = Global.gMainRef;
                    FrmStoneHistory.ShowForm(Val.ToString(GrdVisited.GetRowCellValue(e.RowHandle, "STOCK_ID")), Val.ToString(GrdVisited.GetRowCellValue(e.RowHandle, "STOCKNO")), Stock.FrmStoneHistory.FORMTYPE.DISPLAY);
                    this.Cursor = Cursors.Default;
                }

                
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void GrdCart_RowCellClick_1(object sender, RowCellClickEventArgs e)
        {
            try
            {
                if (e.Clicks == 2 && e.Column.FieldName == "JANGEDNOSTR")
                {
                    this.Cursor = Cursors.WaitCursor;
                    FrmStoneHistory FrmStoneHistory = new FrmStoneHistory();
                    FrmStoneHistory.MdiParent = Global.gMainRef;
                    FrmStoneHistory.ShowForm(Val.ToString(GrdCart.GetRowCellValue(e.RowHandle, "STOCK_ID")), Val.ToString(GrdCart.GetRowCellValue(e.RowHandle, "STOCKNO")), Stock.FrmStoneHistory.FORMTYPE.DISPLAY);
                    this.Cursor = Cursors.Default;
                }

               
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void GrdCartSummury_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Clicks == 2 && e.Column.FieldName == "RANGWISHLISTCOUNT")
            {
                this.Cursor = Cursors.WaitCursor;
                StrParty_ID = "";
                StrParty_ID = Val.ToString(GrdCartSummury.GetRowCellValue(e.RowHandle, "LEDGER_ID"));
                DataSet DS = ObjStock.GetDataForWebActivity(StrFromDate, StrToDate, StrParty_ID);
                xtraTabControl3.SelectedTabPageIndex = 1;
                MainGrdCart.DataSource = DS.Tables[1];
                GrdCart.RefreshData();
                this.Cursor = Cursors.Default;
            }
        }

        private void GrdMostVisitedSummury_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Clicks == 2 && e.Column.FieldName == "RANGWISHLISTCOUNT")
            {
                this.Cursor = Cursors.WaitCursor;
                StrParty_ID = "";
                StrParty_ID = Val.ToString(GrdMostVisitedSummury.GetRowCellValue(e.RowHandle, "LEDGER_ID"));
                DataSet DS = ObjStock.GetDataForWebActivity(StrFromDate, StrToDate, StrParty_ID);
                xtraTabControl2.SelectedTabPageIndex = 1;
                MainGrdMostVisited.DataSource = DS.Tables[3];
                GrdVisited.RefreshData();
                this.Cursor = Cursors.Default;
            }
        }

        private void GrdWishlishSummury_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Clicks == 2 && e.Column.FieldName == "RANGWISHLISTCOUNT")
            {
                this.Cursor = Cursors.WaitCursor;
                StrParty_ID = "";
                StrParty_ID = Val.ToString(GrdWishlishSummury.GetRowCellValue(e.RowHandle, "LEDGER_ID"));
                DataSet DS = ObjStock.GetDataForWebActivity(StrFromDate, StrToDate, StrParty_ID);
                xtraTabControl1.SelectedTabPageIndex = 1;
                MainGrdWishlist.DataSource = DS.Tables[2];
                GrdWishlist.RefreshData();
                this.Cursor = Cursors.Default;
            }
        }

        private void GrdOrder_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                if (e.Clicks == 2 && e.Column.FieldName == "STOCKNO")
                {
                    this.Cursor = Cursors.WaitCursor;
                    FrmStoneHistory FrmStoneHistory = new FrmStoneHistory();
                    FrmStoneHistory.MdiParent = Global.gMainRef;
                    FrmStoneHistory.ShowForm(Val.ToString(GrdOrder.GetRowCellValue(e.RowHandle, "STOCK_ID")), Val.ToString(GrdOrder.GetRowCellValue(e.RowHandle, "STOCKNO")), Stock.FrmStoneHistory.FORMTYPE.DISPLAY);
                    this.Cursor = Cursors.Default;
                }

                if (e.Clicks == 2 && e.Column.FieldName == "JANGEDNOSTR")
                {
                    this.Cursor = Cursors.WaitCursor;
                    FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                    FrmMemoEntry.MdiParent = Global.gMainRef;
                    FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                    FrmMemoEntry.ShowForm(Val.ToString(GrdOrder.GetRowCellValue(e.RowHandle, "MEMO_ID")));
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void GrdOrderSummury_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Clicks == 2 && e.Column.FieldName == "RANGWISHLISTCOUNT")
            {
                this.Cursor = Cursors.WaitCursor;
                StrParty_ID = "";
                StrParty_ID = Val.ToString(GrdOrderSummury.GetRowCellValue(e.RowHandle, "LEDGER_ID"));
                DataSet DS = ObjStock.GetDataForWebActivity(StrFromDate, StrToDate, StrParty_ID);
                xtraTabControl4.SelectedTabPageIndex = 1;
                MainGridOder.DataSource = DS.Tables[4];
                GrdOrderSummury.RefreshData();
                this.Cursor = Cursors.Default;
            }
        }

        private void GrdSales_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                if (e.Clicks == 2 && e.Column.FieldName == "STOCKNO")
                {
                    this.Cursor = Cursors.WaitCursor;
                    FrmStoneHistory FrmStoneHistory = new FrmStoneHistory();
                    FrmStoneHistory.MdiParent = Global.gMainRef;
                    FrmStoneHistory.ShowForm(Val.ToString(GrdSales.GetRowCellValue(e.RowHandle, "STOCK_ID")), Val.ToString(GrdSales.GetRowCellValue(e.RowHandle, "STOCKNO")), Stock.FrmStoneHistory.FORMTYPE.DISPLAY);
                    this.Cursor = Cursors.Default;
                }


                if (e.Clicks == 2 && e.Column.FieldName == "JANGEDNOSTR" && Val.ToString(GrdSales.GetRowCellValue(e.RowHandle, "STOCKTYPE")) == "SINGLE")
                {
                    this.Cursor = Cursors.WaitCursor;
                    FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                    FrmMemoEntry.MdiParent = Global.gMainRef;
                    FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                    FrmMemoEntry.ShowForm(Val.ToString(GrdSales.GetRowCellValue(e.RowHandle, "MEMO_ID")));
                    this.Cursor = Cursors.Default;
                }

            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void GrdSaleSummary_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Clicks == 2 && e.Column.FieldName == "RANGWISHLISTCOUNT")
            {
                this.Cursor = Cursors.WaitCursor;
                StrParty_ID = "";
                StrParty_ID = Val.ToString(GrdSaleSummary.GetRowCellValue(e.RowHandle, "LEDGER_ID"));
                DataSet DS = ObjStock.GetDataForWebActivity(StrFromDate, StrToDate, StrParty_ID);
                xtraTabControl5.SelectedTabPageIndex = 1;
                MainGrdSales.DataSource = DS.Tables[5];
                GrdSales.RefreshData();
                this.Cursor = Cursors.Default;
            }
        }

        
    }
}
