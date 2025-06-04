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


namespace MahantExport.Parcel
{
    public partial class FrmBombayTransfer  : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_KapanInward ObjKapan = new BOTRN_KapanInward();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        BODevGridSelection ObjGridSelection;

        DataTable DTabDetail = new DataTable();
        
        double DouCarat = 0;
        double DouAmount = 0;

        double DouDetailCarat = 0;
        double DouDetailAmount = 0;

        public FrmBombayTransfer()
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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTSelected = GetTableOfSelectedRows(GrdDetDetail, true);

                if (DTSelected.Rows.Count == 0)
                {
                    Global.Message("No Row Selected For Transfer To Marketing");
                    return;
                }

                if (Global.Confirm("Are You Sure To Save This Entry ?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                DTSelected.AcceptChanges();
                DTSelected.TableName = "Table";
                string StrDetail = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTSelected.WriteXml(sw);
                    StrDetail = sw.ToString();
                }


                ParcelKapanInwardProperty Property = new ParcelKapanInwardProperty();
                Property.TRANSFERNO = Val.ToInt64(txtBombayTransferNo.Text);
                Property.TransferSummaryXml = string.Empty;
                Property.TransferDetailXml = StrDetail;

                Property = ObjKapan.BombayTransferSave(Property);

                this.Cursor = Cursors.Default;
                txtBombayTransferNo.Text = Property.ReturnValue;
                Global.Message(Property.ReturnMessageDesc);
                if (Property.ReturnMessageType == "SUCCESS")
                {
                    DTabDetail.Rows.Clear();
                    ObjGridSelection.ClearSelection();

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
                DataTable DTSelected = GetTableOfSelectedRows(GrdDetDetail, true);

                if (DTSelected.Rows.Count == 0)
                {
                    Global.Message("No Row Selected For Transfer To Marketing");
                    return;
                }

                DTSelected.AcceptChanges();
                DTSelected.TableName = "Table";
                string StrDetail = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTSelected.WriteXml(sw);
                    StrDetail = sw.ToString();
                }

                if (Global.Confirm("Are Your Sure To Delete The Recor?") == System.Windows.Forms.DialogResult.No)
                    return;

                this.Cursor = Cursors.WaitCursor;

                ParcelKapanInwardProperty Property = new ParcelKapanInwardProperty();
                Property.TRANSFERNO = Val.ToInt64(txtBombayTransferNo.Text);
                Property = ObjKapan.BombayTransferDelete(Property);
                this.Cursor = Cursors.Default;

                Global.Message(Property.ReturnMessageDesc);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    DTabDetail.Rows.Clear();
                    
                    txtKapan.Text = string.Empty;

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

                GrdDetDetail.BeginUpdate();

                DataSet DS = ObjKapan.BombayTransferGetData(txtKapan.Text, 
                                "", 
                                "",
                                Val.ToInt64(txtBombayTransferNo.Text),
                                Guid.Empty
                                );

                DTabDetail = DS.Tables[1];

                MainGridDetail.DataSource = DTabDetail;
                MainGridDetail.Refresh();

                if (MainGridDetail.RepositoryItems.Count == 0)
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

               

                GrdDetDetail.Columns["GROUPNAME"].Group();
                GrdDetDetail.ExpandAllGroups();
                GrdDetDetail.Columns["GROUPNAME"].Visible = false;

                GrdDetDetail.GroupSummary.Clear();
                if (GrdDetDetail.GroupSummary.Count == 0)
                {
                    GrdDetDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, "KAPANNAME", GrdDetDetail.Columns["KAPANNAME"], "{0:N3}");
                    
                    GrdDetDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "CARAT", GrdDetDetail.Columns["CARAT"], "{0:N3}");
                    GrdDetDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Custom, "PRICEPERCARAT", GrdDetDetail.Columns["PRICEPERCARAT"], "{0:N2}");
                    GrdDetDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "AMOUNT", GrdDetDetail.Columns["AMOUNT"], "{0:N2}");

                }

                GrdDetDetail.EndUpdate();

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
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

        private void txtSearchInwardNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "INWARDNO";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_KAPANINWARD);
                   // FrmSearch.ColumnsToHide = "EMPLOYEE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBombayTransferNo.Text = Val.ToString(FrmSearch.DRow["INWARDNO"]);
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

        private void BtnClearAllFilter_Click(object sender, EventArgs e)
        {
            GrdDetDetail.Columns["GROUP_ID"].ClearFilter();
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
                    FrmSearch.mDTab = ObjKapan.BombayTransferGetPopupData(Val.SqlDate(DTPFromDate.Value.ToShortDateString()), Val.SqlDate(DTPToDate.Value.ToShortDateString()));
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
            txtKapan.Text = string.Empty;
            DTabDetail.Rows.Clear();
           
            BtnSave.Enabled = true;
        }

        private void GrdDetDetail_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            try
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    DouDetailCarat = 0;
                    DouDetailAmount = 0;

                }
                else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DouDetailCarat = DouDetailCarat + Val.Val(GrdDetDetail.GetRowCellValue(e.RowHandle, "CARAT"));
                    DouDetailAmount = DouDetailAmount + (Val.Val(GrdDetDetail.GetRowCellValue(e.RowHandle, "CARAT")) * Val.Val(GrdDetDetail.GetRowCellValue(e.RowHandle, "PRICEPERCARAT")));
                }
                else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("PRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouDetailCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouDetailAmount) / Val.Val(DouDetailCarat), 2);
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

        public void CalculateSelectedSummary()
        {
            double DblSelectedRapAmount = 0.00;
            double DblSelectedRapaport = 0.00;
            double DblSelectedDiscount = 0.00;

            DataTable DTab = GetTableOfSelectedRows(GrdDetDetail, true);

            if (DTab != null && DTab.Rows.Count > 0)
            {
                txtBalanceCarat.Text = Val.ToString(DTab.Compute("SUM(CARAT)", string.Empty));
                txtPcs.Text = Val.ToString(DTab.Compute("COUNT(KAPANNAME)", string.Empty));
                txtAmount.Text = Val.ToString(DTab.Compute("SUM(AMOUNT)", string.Empty));
                TxtPricePerCarat.Text = string.Format("{0:0.00}", Val.Val(txtAmount.Text) / Val.Val(txtBalanceCarat.Text));
            }
            else
            {
                txtBalanceCarat.Text = string.Empty;
                txtPcs.Text = string.Empty;
                txtAmount.Text = string.Empty;
                TxtPricePerCarat.Text = string.Empty;
            }

        }

        private void GrdDetDetail_MouseUp(object sender, MouseEventArgs e)
        {
            CalculateSelectedSummary();
        }


    }
}
