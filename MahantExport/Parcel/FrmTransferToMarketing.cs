using BusLib.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusLib.Configuration;
using BusLib.TableName;
using System.Data.SqlClient;
using MahantExport;
using System.IO;
using System.Text.RegularExpressions;
using MahantExport.Utility;
using DevExpress.XtraGrid.Columns;
using System.Collections;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Data;
using DevExpress.XtraGrid.Views.Base;

namespace MahantExport.Parcel
{
    public partial class FrmTransferToMarketing  : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_KapanInward ObjKapan = new BOTRN_KapanInward();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        BODevGridSelection ObjGridSelection;

        DataTable DTabDetail = new DataTable();
        DataTable DTabSummury = new DataTable();

        double DouSuratCarat = 0;
        double DouSuratAmount = 0;
        double DouBombayCarat = 0;
        double DouBombayAmount = 0;

        public FrmTransferToMarketing()
        {
            InitializeComponent();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = false;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjKapan);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            DTPToDate.Value = DateTime.Now;

            this.Show();
           
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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTSelected = GetTableOfSelectedRows(GrdDetDetail, true);

                if (DTSelected.Rows.Count == 0 )
                {
                    Global.Message("No Row Selected For Transfer To Marketing");
                    return;
                }

                if (Convert.ToDecimal(DTSelected.Compute("SUM(Carat)", string.Empty)) != Convert.ToDecimal(DTSelected.Compute("SUM(NewCarat)", string.Empty)))
                {
                    Global.Message("Carat Adjustment is not proper. Please check..");
                    return;
                }

                foreach (DataRow DROw in DTSelected.Rows)
                {
                    DROw["STOCK_ID"] = BusLib.Configuration.BOConfiguration.FindNewSequentialID();

                    if (Val.Val(DROw["MERGEMIXCLARITY_ID"]) == 0)
                    {
                        Global.Message("Merge Clarity ID Is Missing, It Is Compulsary ");
                        return;
                    }
                    if (Val.ToString(DROw["MERGEMIXCLARITYNAME"]).Length == 0)
                    {
                        Global.Message("Merge Clarity Name Is Missing It Is Compulsary");
                        return;
                    }

                    if (Val.Val(DROw["MERGEMIXSHAPE_ID"]) == 0)
                    {
                        Global.Message("Merge Shape ID Is Missing, It Is Compulsary ");
                        return;
                    }
                    if (Val.ToString(DROw["MERGEMIXSHAPENAME"]).Length == 0)
                    {
                        Global.Message("Merge Shape Name Is Missing It Is Compulsary");
                        return;
                    }

                    if (Val.Val(DROw["MERGEMIXSIZE_ID"]) == 0)
                    {
                        Global.Message("Merge Size ID Is Missing, It Is Compulsary ");
                        return;
                    }
                    if (Val.ToString(DROw["MERGEMIXSIZENAME"]).Length == 0)
                    {
                        Global.Message("Merge Size Name Is Missing It Is Compulsary");
                        return;
                    }
                }
                DTSelected.AcceptChanges();
                DTSelected.TableName = "Table";
                string StrDetail = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTSelected.WriteXml(sw);
                    StrDetail = sw.ToString();
                }

                if (Global.Confirm("Are You Sure To Save This Entry ?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                ParcelKapanInwardProperty Property = new ParcelKapanInwardProperty();
                Property.TransferDetailXml = StrDetail;
                Property.MODE = "TRANSFERTOMARKETING";
                Property = ObjKapan.BombayTransferToMarketingSave(Property);

                this.Cursor = Cursors.Default;
                txtBombayTransferNo.Text = Property.ReturnValue;
                Global.Message(Property.ReturnMessageDesc);
                if (Property.ReturnMessageType == "SUCCESS")
                {
                    DTabDetail.Rows.Clear();
                    DTabSummury.Rows.Clear();
                    
                    txtBombayTransferNo.Text = string.Empty;

                    BtnSearch_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;

                Global.Message(ex.Message);
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

   
        private void BtnDelete_Click(object sender, EventArgs e)
        {
           
            try
            {
                if (DTabDetail.Rows.Count == 0)
                {
                    Global.Message("Data Not Found For Delete");
                    return;
                }

                if (Global.Confirm("Are Your Sure To Delete The Record ?") == System.Windows.Forms.DialogResult.No)
                    return;

                this.Cursor = Cursors.WaitCursor;

                DTabDetail.TableName = "Table";
                string StrDetail = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabDetail.WriteXml(sw);
                    StrDetail = sw.ToString();
                }

                ParcelKapanInwardProperty Property = new ParcelKapanInwardProperty();
                Property.TransferDetailXml = StrDetail;
                Property = ObjKapan.BombayTransferToMarketingDelete(Property);

                this.Cursor = Cursors.Default;

                Global.Message(Property.ReturnMessageDesc);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    DTabDetail.Rows.Clear();
                    DTabSummury.Rows.Clear();
                    BtnClear_Click(null, null);
                    BtnSearch_Click(null, null);

                }

                Property = null;
            }
            catch (System.Exception ex)
            {
                this.Cursor = Cursors.Default;

                Global.MessageToster(ex.Message);
            }
        }


        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                string StrStatus = "";
                if (RbtAll.Checked == true)
                {
                    StrStatus = "ALL";
                }
                else if (RbtPending.Checked == true)
                {
                    StrStatus = "PENDING";
                }
                else if (RbtMerged.Checked == true)
                {
                    StrStatus = "MERGED";
                }
                
                DTabSummury= ObjKapan.BombayTransferToMarketingGetData(Val.SqlDate(DTPTransferFromDate.Value.ToShortDateString()),
                               Val.SqlDate(DTPTransferToDate.Value.ToShortDateString()),
                               StrStatus,
                               Val.ToInt64(txtBombayTransferNo.Text), "TRANSFERTOMARKETING" +
                               ""
                                );

                MainGridDetail.DataSource = DTabSummury;
                MainGridDetail.Refresh();

                GrdDetDetail.Columns["TRANSFERNO"].Group();

                GrdDetDetail.GroupSummary.Clear();
                if (GrdDetDetail.GroupSummary.Count == 0)
                {
                    GrdDetDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, "KAPANNAME", GrdDetDetail.Columns["KAPANNAME"], "{0:N0}");
                    GrdDetDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "NEWCARAT", GrdDetDetail.Columns["NEWCARAT"], "{0:N3}");
                    GrdDetDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "NEWAMOUNT", GrdDetDetail.Columns["NEWAMOUNT"], "{0:N3}");
                    GrdDetDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Custom, "NEWPRICEPERCARAT", GrdDetDetail.Columns["NEWPRICEPERCARAT"], "{0:N3}");

                    GrdDetDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "CARAT", GrdDetDetail.Columns["CARAT"], "{0:N3}");
                    GrdDetDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "AMOUNT", GrdDetDetail.Columns["AMOUNT"], "{0:N3}");
                    GrdDetDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Custom, "PRICEPERCARAT", GrdDetDetail.Columns["PRICEPERCARAT"], "{0:N3}");

                    GrdDetDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "LESSPLUSAMOUNT", GrdDetDetail.Columns["LESSPLUSAMOUNT"], "{0:N3}");
                    GrdDetDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "LESSPLUSCARAT", GrdDetDetail.Columns["LESSPLUSCARAT"], "{0:N3}");
                }

                GrdDetDetail.ExpandAllGroups();

                if (MainGridDetail.RepositoryItems.Count == 4)
                {
                    ObjGridSelection = new BODevGridSelection();
                    ObjGridSelection.View = GrdDetDetail;
                    ObjGridSelection.ISBoolApplicableForPageConcept = false;
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                    GrdDetDetail.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;
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

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
           
        }

        private void txtBombayTransferNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "TRANSFERNO,TRANSFERDATE,TOTALCARAT,TOTALAMOUNT";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = ObjKapan.BombayTransferToMarketingGetPopupData(Val.SqlDate(DTPFromDate.Value.ToShortDateString()), Val.SqlDate(DTPToDate.Value.ToShortDateString()));
                    FrmSearch.mStrColumnsToHide = "";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBombayTransferNo.Text = Val.ToString(FrmSearch.DRow["TRANSFERNO"]);
                        BtnSearch_Click(null, null);
                        BtnSave.Enabled = false;
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
            txtBombayTransferNo.Text = string.Empty;
            DTabDetail.Rows.Clear();
            DTabSummury.Rows.Clear();
            BtnSave.Enabled = true;
            txtMixShape.Text = string.Empty;
            txtMixCla.Text = string.Empty;
            txtMixSize.Text = string.Empty;
            txtMixDept.Text = string.Empty;
        }

        private void GrdDetSummry_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            
        }

        private void GrdDetDetail_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                return;
            }

            string StrStatus = Val.ToString(GrdDetDetail.GetRowCellValue(e.RowHandle, "STATUS"));
            if (StrStatus == "PENDING")
            {
                e.Appearance.BackColor = lblInTransit.BackColor;
                e.Appearance.BackColor2 = lblInTransit.BackColor;
            }

            else if (StrStatus == "MERGED")
            {
                e.Appearance.BackColor = Color.Transparent;
                e.Appearance.BackColor2 = Color.Transparent;
            }
        }

        private void RbtMerged_CheckedChanged(object sender, EventArgs e)
        {
            BtnSave.Enabled = false;
            BtnDelete.Enabled = true;
        }

        private void RbtAll_CheckedChanged(object sender, EventArgs e)
        {
            BtnDelete.Enabled = false;
        }

        private void RbtPending_CheckedChanged(object sender, EventArgs e)
        {
            BtnSave.Enabled = true;
            BtnDelete.Enabled = false;
        }

        private void GrdDetDetail_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            try
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    DouSuratCarat = 0;
                    DouSuratAmount = 0;
                    DouBombayCarat = 0;
                    DouBombayAmount = 0;
                }
                else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DouSuratCarat = DouSuratCarat + Val.Val(GrdDetDetail.GetRowCellValue(e.RowHandle, "CARAT"));
                    DouSuratAmount = DouSuratAmount + (Val.Val(GrdDetDetail.GetRowCellValue(e.RowHandle, "CARAT")) * Val.Val(GrdDetDetail.GetRowCellValue(e.RowHandle, "PRICEPERCARAT")));

                    DouBombayCarat = DouBombayCarat + Val.Val(GrdDetDetail.GetRowCellValue(e.RowHandle, "NEWCARAT"));
                    DouBombayAmount = DouBombayAmount + (Val.Val(GrdDetDetail.GetRowCellValue(e.RowHandle, "NEWCARAT")) * Val.Val(GrdDetDetail.GetRowCellValue(e.RowHandle, "NEWPRICEPERCARAT")));


                }
                else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("PRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouSuratCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouSuratAmount) / Val.Val(DouSuratCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("NEWPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouBombayCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouBombayAmount) / Val.Val(DouBombayCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("SHAPENAME") == 0)
                    {
                        if (Val.Val(DouBombayCarat) > 0)
                            e.TotalValue = Math.Round(DouSuratCarat - DouBombayCarat, 3);
                        else
                            e.TotalValue = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtMergeClarity_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "MIXCLARITYCODE,MIXCLARITYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_MIXCLARITY);
                    FrmSearch.mStrColumnsToHide = "MIXCLARITY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDetDetail.SetFocusedRowCellValue("MERGEMIXCLARITY_ID",Val.ToString(FrmSearch.DRow["MIXCLARITY_ID"]));
                        GrdDetDetail.SetFocusedRowCellValue("MERGEMIXCLARITYNAME",Val.ToString(FrmSearch.DRow["MIXCLARITYNAME"]));
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

        private void txtMixShape_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "SHAPECODE,SHAPENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SHAPE);
                    FrmSearch.mStrColumnsToHide = "SHAPE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtMixShape.Text = Val.ToString(FrmSearch.DRow["SHAPENAME"]);
                        txtMixShape.Tag = Val.ToString(FrmSearch.DRow["SHAPE_ID"]);

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

        private void txtMixCla_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "MIXCLARITYCODE,MIXCLARITYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_MIXCLARITY);
                    FrmSearch.mStrColumnsToHide = "MIXCLARITY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtMixCla.Tag = Val.ToString(FrmSearch.DRow["MIXCLARITY_ID"]);
                        txtMixCla.Text = Val.ToString(FrmSearch.DRow["MIXCLARITYNAME"]);
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

        private void txtMixSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "MIXSIZE_ID,MIXSIZENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_MIXSIZE);
                    FrmSearch.mStrColumnsToHide = "MIXSIZE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtMixSize.Text = Val.ToString(FrmSearch.DRow["MIXSIZENAME"]);
                        txtMixSize.Tag = Val.ToString(FrmSearch.DRow["MIXSIZE_ID"]);
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

        private void txtMergeSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "MIXSIZE_ID,MIXSIZENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_MIXSIZE);
                    FrmSearch.mStrColumnsToHide = "MIXSIZE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDetDetail.SetFocusedRowCellValue("MERGEMIXSIZE_ID", Val.ToString(FrmSearch.DRow["MIXSIZE_ID"]));
                        GrdDetDetail.SetFocusedRowCellValue("MERGEMIXSIZENAME", Val.ToString(FrmSearch.DRow["MIXSIZENAME"]));
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

        private void txtMergeShape_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "SHAPECODE,SHAPENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SHAPE);
                    FrmSearch.mStrColumnsToHide = "SHAPE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDetDetail.SetFocusedRowCellValue("MERGEMIXSHAPE_ID", Val.ToString(FrmSearch.DRow["SHAPE_ID"]));
                        GrdDetDetail.SetFocusedRowCellValue("MERGEMIXSHAPENAME", Val.ToString(FrmSearch.DRow["SHAPENAME"]));
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMixShape.Text == "" )
                {
                    Global.Message("Please Enter Mix Shape !!!");
                    txtMixShape.Focus();
                    return;
                }

                if (txtMixCla.Text == "")
                {
                    Global.Message("Please Enter Mix Clarity !!!");
                    txtMixCla.Focus();
                    return;
                }

                if (txtMixSize.Text == "")
                {
                    Global.Message("Please Enter Mix Size !!!");
                    txtMixSize.Focus();
                    return;
                }

                if (txtMixDept.Text == "")
                {
                    Global.Message("Please Enter Mix Department !!!");
                    txtMixSize.Focus();
                    return;
                }

                DataTable DtInvDetail = GetTableOfSelectedRows(GrdDetDetail, true);
                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                string StrStoneNo = string.Empty;


                for (int IntI = 0; IntI < DtInvDetail.Rows.Count; IntI++)
                {              
                    for (int i = 0; i < GrdDetDetail.DataRowCount; i++)
                    {

                        DataRow DR = GrdDetDetail.GetDataRow(i);
                        if (DR["STOCK_ID"] == DtInvDetail.Rows[IntI]["STOCK_ID"])
                        {
                            if (Val.ToString(txtMixShape.Text).Length != 0)
                            {
                                GrdDetDetail.SetRowCellValue(i, "MERGEMIXSHAPE_ID", txtMixShape.Tag);
                                GrdDetDetail.SetRowCellValue(i, "MERGEMIXSHAPENAME", txtMixShape.Text);
                            }
                            if (Val.ToString(txtMixCla.Text).Length != 0)
                            {
                                GrdDetDetail.SetRowCellValue(i, "MERGEMIXCLARITY_ID", txtMixCla.Tag);
                                GrdDetDetail.SetRowCellValue(i, "MERGEMIXCLARITYNAME", txtMixCla.Text);
                            }
                            if (Val.ToString(txtMixSize.Text).Length != 0)
                            {
                                GrdDetDetail.SetRowCellValue(i, "MERGEMIXSIZE_ID", txtMixSize.Tag);
                                GrdDetDetail.SetRowCellValue(i, "MERGEMIXSIZENAME", txtMixSize.Text);
                            }
                            if (Val.ToString(txtMixDept.Text).Length != 0)
                            {
                                GrdDetDetail.SetRowCellValue(i, "MERGEDEPARTMENT_ID", txtMixDept.Tag);
                                GrdDetDetail.SetRowCellValue(i, "MERGEDEPARTMENTNAME", txtMixDept.Text);
                            }
                        }
                       
                    }
                }
               
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void repTxtMergeDept_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "DEPARTMENTCODE,DEPARTMENTNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_DEPARTMENT);
                    FrmSearch.mStrColumnsToHide = "DEPARTMENT_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDetDetail.SetFocusedRowCellValue("MERGEDEPARTMENT_ID", Val.ToString(FrmSearch.DRow["DEPARTMENT_ID"]));
                        GrdDetDetail.SetFocusedRowCellValue("MERGEDEPARTMENTNAME", Val.ToString(FrmSearch.DRow["DEPARTMENTNAME"]));
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

        private void txtMixDept_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "DEPARTMENTCODE,DEPARTMENTNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_DEPARTMENT);
                    FrmSearch.mStrColumnsToHide = "DEPARTMENT_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtMixDept.Tag = Val.ToString(FrmSearch.DRow["DEPARTMENT_ID"]);
                        txtMixDept.Text = Val.ToString(FrmSearch.DRow["DEPARTMENTNAME"]);
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

        private void GrdDetDetail_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "NEWCARAT")
            {
                DataRow DRow = GrdDetDetail.GetDataRow(e.RowHandle);

                double DouCarat = Val.Val(DRow["CARAT"]);
                double DouRate = Val.Val(DRow["PRICEPERCARAT"]);
                double DouAmount = Val.Val(DRow["AMOUNT"]);

                double DouNewCarat = Val.Val(DRow["NEWCARAT"]);
                double DouNewRate = Val.Val(DRow["NEWPRICEPERCARAT"]);
                double DouNewAmount = Math.Round(DouNewCarat * DouNewRate, 3);

                double LessPlusCarat = Math.Round(DouNewCarat - DouCarat, 3);
                double LessPlusAmount = Math.Round(DouNewAmount - DouAmount, 3);


                double LessPlusCaratPer = DouNewCarat == 0 ? 0 : Math.Round(((DouNewCarat - DouCarat) / DouNewCarat) * 100, 5);
                double LessPlusAmountPer = DouNewAmount == 0 ? 0 : Math.Round(((DouNewAmount - DouAmount) / DouNewAmount) * 100, 5);

                GrdDetDetail.SetRowCellValue(e.RowHandle, "NEWPRICEPERCARAT", DouNewRate);
                GrdDetDetail.SetRowCellValue(e.RowHandle, "NEWAMOUNT", DouNewAmount);
                GrdDetDetail.SetRowCellValue(e.RowHandle, "LESSPLUSCARAT", LessPlusCarat);
                GrdDetDetail.SetRowCellValue(e.RowHandle, "LESSPLUSCARATPER", LessPlusCaratPer);
                GrdDetDetail.SetRowCellValue(e.RowHandle, "LESSPLUSAMOUNT", LessPlusAmount);
                GrdDetDetail.SetRowCellValue(e.RowHandle, "LESSPLUSAMOUNTPER", LessPlusAmountPer);

                DTabDetail.AcceptChanges();

            }
        }
    }
}
