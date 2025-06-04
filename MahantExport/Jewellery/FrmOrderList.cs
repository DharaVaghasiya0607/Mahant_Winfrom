using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using DevExpress.XtraPrinting;
using Google.API.Translate;
using MahantExport.Utility;
using DevExpress.XtraGrid.Columns;
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
    public partial class FrmOrderList : DevControlLib.cDevXtraForm
    {
        string MergeOn = string.Empty;
        string MergeOnStr = string.Empty;

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_Style ObjMast = new BOMST_Style();

        DataTable DTabSummary = new DataTable();
        DataTable DTabDetail = new DataTable();
        DataTable DtabMaterial = new DataTable();

        DataSet DSet = new DataSet();

        string StrMesg = "";

        #region Property Settings

        public FrmOrderList()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();
            dtpFromDate.Value = DateTime.Today.AddMonths(-1);
            DtpToDate.Value = DateTime.Now;
            GrdMaterial.Columns["ORDERNO"].Fixed = FixedStyle.Left;
            GrdDetail.Columns["ORDERNO"].Fixed = FixedStyle.Left;
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMast);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);

        }

        #endregion

        private void BtnFind_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                Int32 pIntCustomer_ID = Val.ToInt32(txtCustomer.Tag);
                string pStrStyleNo = Val.ToString(txtStyleNo.Text);
                string pStrStatus = "";

                string pStrFromDate = "";
                string pStrTODate = "";

                if (dtpFromDate.Checked == true)
                {
                    pStrFromDate = Val.SqlDate(dtpFromDate.Value.ToShortDateString());
                }
                else
                {
                    pStrFromDate = null;
                }

                if (DtpToDate.Checked == true)
                {
                    pStrTODate = Val.SqlDate(DtpToDate.Value.ToShortDateString());
                }
                else
                {
                    pStrTODate = null;
                }

                if (RbtCompleted.Checked == true)
                {
                    pStrStatus = "COMPLETE";
                }
                else if (RbtPending.Checked == true)
                {
                    pStrStatus = "PENDING";
                }
                else
                {
                    pStrStatus = "";
                }

                DSet = ObjMast.GetDataForOrderSummary(pIntCustomer_ID, pStrStyleNo, Val.ToString(txtOrderno.Text), pStrStatus, pStrFromDate, pStrTODate);


                //MainGrdSummary.RefreshDataSource();
                //DTabSummary = DSet.Tables[0];
                //GrdSummary.BestFitColumns();

                MainGrdSummary.DataSource = DSet.Tables[0];
                MainGrdSummary.RefreshDataSource();
                GrdSummary.BestFitColumns();

                MainGrdDetail.DataSource = DSet.Tables[1];
                MainGrdDetail.RefreshDataSource();
                GrdDetail.BestFitColumns();

                MainGrdMaterial.DataSource = DSet.Tables[2];
                MainGrdMaterial.RefreshDataSource();
                GrdMaterial.BestFitColumns();

                GrdDetail.Columns["ORDERNO"].Group();
                GrdDetail.ExpandAllGroups();
                //GrdDetail.Columns["ORDERNO"].Visible = false;
                if (GrdDetail.GroupSummary.Count == 0)
                {
                    GrdDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, "STYLENO", GrdDetail.Columns["STYLENO"], "{0:N0}");
                    GrdDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "QUANTITY", GrdDetail.Columns["QUANTITY"], "{0:N0}");
                    GrdDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "AMOUNT", GrdDetail.Columns["AMOUNT"], "{0:N3}");
                }


                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtOrderno.Text = string.Empty;
                txtOrderno.Tag = string.Empty;
                txtCustomer.Text = string.Empty;
                txtCustomer.Tag = string.Empty;
                txtStyleNo.Text = string.Empty;
                txtStyleNo.Tag = string.Empty;
                dtpFromDate.Value = DateTime.Today.AddMonths(-1);
                DtpToDate.Value = DateTime.Now;
                RbtAll.Checked = true;

                DSet.Clear();
                //MainGrdSummary.Refresh();

                DTabSummary.Rows.Clear();
                DTabDetail.Rows.Clear();
                DtabMaterial.Rows.Clear();
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "CUSTOMERNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CUSTOMER);
                    FrmSearch.mStrColumnsToHide = "CUSTOMER_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtCustomer.Text = Val.ToString(FrmSearch.DRow["CUSTOMERNAME"]);
                        txtCustomer.Tag = Val.ToString(FrmSearch.DRow["CUSTOMER_ID"]);
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

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                Global.ExcelExport("Order Summary List", GrdSummary);
            }
            else if (xtraTabControl1.SelectedTabPageIndex == 1)
            {
                Global.ExcelExport("Order Detail List", GrdDetail);
            }
            if (xtraTabControl1.SelectedTabPageIndex == 2)
            {
                Global.ExcelExport("Style Material List", GrdMaterial);
            }
        }

        private void GrdSummary_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }
                if (e.Clicks == 2 && e.Column.FieldName == "ORDERNO")
                {
                    string StrOrderNo = Val.ToString(GrdSummary.GetFocusedRowCellValue("ORDERNO"));

                    DataSet DSet = ObjMast.GetDataForOrderSummary(0, "", StrOrderNo, "", null, null);

                    MainGrdDetail.DataSource = DSet.Tables[1];
                    MainGrdDetail.RefreshDataSource();
                    GrdSummary.BestFitColumns();
                    xtraTabControl1.SelectedTabPage = DetailTab;
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void txtStyleNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void GrdMaterial_CellMerge(object sender, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs e)
        {

        }
    }
}
