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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.Utils.Drawing;
using DevExpress.Data;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
using DevExpress.XtraGrid.Columns;
using MahantExport.Utility;
using DevExpress.XtraGrid.Views.BandedGrid;
using OfficeOpenXml;
using DevExpress.XtraPrinting;
using DevExpress.Utils;
using DevExpress.XtraPivotGrid;
using System.Collections;
using Microsoft.VisualBasic.CompilerServices;

namespace MahantExport.Parcel
{
    public partial class FrmClarityAssortmentView  : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_KapanInward ObjKapan = new BOTRN_KapanInward();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        DataTable DTabAssort = new DataTable();
        double DouTAmt = 0;
        double DouTCarat = 0;
        double DouAmount = 0;
        double DouCarat= 0;
        bool IsBombayTransfer = false;

        #region Property

        public FrmClarityAssortmentView()
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
           // ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }

        public void ShowForm(string _KapanName = "", bool _IsBombayTransfer = false)
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();

            //Added by Daksha on 25/08/2023
            BtnClear_Click(null, null);            
            txtKapan.Text = _KapanName;
            IsBombayTransfer = _IsBombayTransfer;
            if (_IsBombayTransfer)
            {
                BtnSearch_Click(null, null);
                BtnSave.Enabled = false;
                btnBombayTransfer.Enabled = false;
                BtnSearch.Enabled = false;
            }
            //End as Daksha
        }

        #endregion

        #region Operation
        public void Calculation()
        {
            try
            {
                //txtTotalWeight.Text = Val.ToString(GridDetail.Columns["CARAT"].SummaryItem.SummaryValue);//Comment By Gunjan:19/08/2023
                //txtTotalAverage.Text = Val.ToString(GridDetail.Columns["AVERAGE"].SummaryItem.SummaryValue);//Comment By Gunjan:19/08/2023

                txtCertiCarat.Text = Val.ToString(GridCerti.Columns["CARAT"].SummaryItem.SummaryValue);
                txtNonCertiCarat.Text = Val.ToString(GridNonCerti.Columns["CARAT"].SummaryItem.SummaryValue);

                txtCertiAmount.Text = Val.ToString(GridCerti.Columns["COSTAMOUNT"].SummaryItem.SummaryValue);
                txtNonCertiAmount.Text = Val.ToString(GridNonCerti.Columns["COSTAMOUNT"].SummaryItem.SummaryValue);

                txtTotalCarat.Text = Math.Round(Val.Val(txtCertiCarat.Text) + Val.Val(txtNonCertiCarat.Text), 3).ToString();
                txtTotalAmount.Text = Math.Round(Val.Val(txtCertiAmount.Text) + Val.Val(txtNonCertiAmount.Text), 3).ToString();

                //Added By Gunjan : 19/08/2023
                double StrParcelCts = Val.ToDouble(GridDetail.Columns["CARAT"].SummaryItem.SummaryValue);
                double StrFancyCts = Val.ToDouble(dgvFancySummary.Columns["Carat"].SummaryItem.SummaryValue);
                double StrCertCts = Val.Val(txtTotalCarat.Text);
                double StrTotalCts = Math.Round(StrParcelCts + StrFancyCts + StrCertCts, 3);

                double StrParcelAmt = Val.ToDouble(GridDetail.Columns["AMOUNT"].SummaryItem.SummaryValue);
                double StrFancyAmt = Val.ToDouble(dgvFancySummary.Columns["Amount"].SummaryItem.SummaryValue);
                double StrCertAmt = Val.Val(txtTotalAmount.Text);
                double StrTotalAmt = Math.Round(StrParcelAmt + StrFancyAmt + StrCertAmt, 2);

                txtTotalWeight.Text = Math.Round(StrTotalCts, 3).ToString();
                txtTotalAverage.Text = Math.Round(StrTotalAmt / StrTotalCts, 2).ToString();
                //End As Gunjan

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        #endregion

        #region Control Event
        private void BtnBack_Click(object sender, EventArgs e)
        {
             this.Close();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                
                this.Cursor = Cursors.WaitCursor;

                DataSet DS = ObjKapan.GetClarityAssortmentData(Val.ToString(txtKapan.Text), IsBombayTransfer);

                DTabAssort = DS.Tables[0];
                DataTable DTabStock = DS.Tables[1];
                DataTable DTabNonCerti = DS.Tables[2];
                DataTable DtabEverage = DS.Tables[3];                

                pivotGrid.DataSource = DTabAssort;
                MainGrid.DataSource = DTabStock;
                GrdMain.DataSource = DTabNonCerti;
                MainGridView.DataSource = DtabEverage;

                //Added by Daksha on 01/07/2023
                GCFancyDetail.DataSource = DS.Tables[4];
                GCFancySumary.DataSource = DS.Tables[5];               
                //End as Daksha

                txtLotNo.Text = Val.ToString(txtKapan.Text);

                if (DtabEverage.Rows.Count > 0)
                {
                    txtRate.Text = Val.ToString(DtabEverage.Rows[0]["ExcRate"].ToString());
                    txtAadat_TextChanged(null, null);
                }

                Calculation();
                this.Cursor = Cursors.Default;
                string[] SplitKapan = txtKapan.Text.Split(',');
                if (SplitKapan.Length == 1 && SplitKapan[0] != "")
                {
                    BtnSave.Enabled = true;
                    pivotGrid.Fields["RATE"].Options.AllowEdit = true;
                    // GridDetail.Columns["AVERAGE"].OptionsColumn.AllowEdit = true;//Gunjan:27/04/2023
                    btnBombayTransfer.Enabled = true; //Added by Daksha on 21/08/2023
                }
                else
                {
                    BtnSave.Enabled = false;
                    pivotGrid.Fields["RATE"].Options.AllowEdit = false;
                    //GridDetail.Columns["AVERAGE"].OptionsColumn.AllowEdit = false;//Gunjan:27/04/2023
                    btnBombayTransfer.Enabled = false; //Added by Daksha on 21/08/2023
                }

                GridDetail.Columns["CLAGROUP"].Group(); 
                GridDetail.Columns["CLAGROUP"].Visible = false;
                GridDetail.Columns["SIZEGROUP"].Group();
                GridDetail.Columns["SIZEGROUP"].Visible = false;

                if (GridDetail.GroupSummary.Count == 0)
                {
                    GridDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "CARAT", GridDetail.Columns["CARAT"], "{0:N2}");
                    GridDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, "SIZE", GridDetail.Columns["SIZE"], "{0:N0}");
                    GridDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Custom, "AVERAGE", GridDetail.Columns["AVERAGE"], "{0:N2}");
                    GridDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Custom, "OLDAVERAGE", GridDetail.Columns["OLDAVERAGE"], "{0:N2}");
                    GridDetail.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "AMOUNT", GridDetail.Columns["AMOUNT"], "{0:N2}");
                }
                GridDetail.ExpandAllGroups();

                GridCerti.BestFitColumns();
                GridNonCerti.BestFitColumns();
                GridDetail.BestFitColumns();

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Global.ExcelExport("Kapan Analysis", pivotGrid);

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (DTabAssort.Rows.Count == 0)
                {
                    return;
                }
                if (Global.Confirm("Are You sure,You want to Save Rate?") == DialogResult.No)
                {
                    return;
                }
                ParcelKapanInwardProperty Property = new ParcelKapanInwardProperty();
                DTabAssort.TableName = "Table";
                string StrXMLValuesInsert = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabAssort.WriteXml(sw);
                    StrXMLValuesInsert = sw.ToString();
                }

                Property.StrRateXml = StrXMLValuesInsert;
                Property = ObjKapan.SaveRate(Property);

                this.Cursor = Cursors.Default;

                Global.Message(Property.ReturnMessageDesc);
                if (Property.ReturnMessageType == "SUCCESS")
                {
                    BtnSearch_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        #endregion

        #region GridEvent
        private void txtKapan_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearchPopupBoxMultipleSelect FrmSearch = new FrmSearchPopupBoxMultipleSelect();
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_KAPAN);
                    FrmSearch.mStrColumnsToHide = "";
                    FrmSearch.ValueMemeter = "KAPANNAME";
                    FrmSearch.DisplayMemeter = "KAPANNAME";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.SelectedDisplaymember != "" && FrmSearch.SelectedValuemember != "")
                    {
                        txtKapan.Text = Val.ToString(FrmSearch.SelectedDisplaymember);
                        txtKapan.Tag = Val.ToString(FrmSearch.SelectedValuemember);
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

        private void GrdCerti_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            try
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    DouAmount = 0;
                    DouCarat = 0;
                }
                else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DouCarat = DouCarat + Val.Val(GridCerti.GetRowCellValue(e.RowHandle, "CARAT"));
                    DouAmount = DouAmount + Val.Val(GridCerti.GetRowCellValue(e.RowHandle, "COSTAMOUNT"));
                }
                else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("COSTRATE") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouAmount) / Val.Val(DouCarat), 2);
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

        double TNewAmt = 0;
        private void GridDetail_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        { 
            try
            {
                GridView view = sender as GridView;
                string strFieldName = "";
                if (e.IsTotalSummary)
                {
                    DevExpress.XtraGrid.GridColumnSummaryItem objObject;
                    objObject = (GridColumnSummaryItem)e.Item;
                    strFieldName = objObject.FieldName;
                }
                if (e.IsGroupSummary)
                {
                    DevExpress.XtraGrid.GridGroupSummaryItem objObject;
                    objObject = (GridGroupSummaryItem)e.Item;
                    strFieldName = objObject.FieldName;
                }
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    DouTAmt = 0;
                    DouTCarat = 0;
                    TNewAmt = 0;
                }

                if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    TNewAmt += Val.Val(GridDetail.GetRowCellValue(e.RowHandle, "CARAT")) * Val.Val(GridDetail.GetRowCellValue(e.RowHandle, "AVERAGE"));
                    DouTCarat = DouTCarat + Val.Val(GridDetail.GetRowCellValue(e.RowHandle, "CARAT"));
                    DouTAmt = DouTAmt + Val.Val(GridDetail.GetRowCellValue(e.RowHandle, "AMOUNT"));
                }
                if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    if (strFieldName == "OLDAVERAGE")
                    {
                        if (DouTCarat == 0)
                            e.TotalValue = 0;
                        else
                            e.TotalValue = Math.Round(Val.Val(DouTAmt) / Val.Val(DouTCarat), 2);
                    }

                    if (strFieldName == "AVERAGE")
                    {
                        if (DouTCarat == 0)
                            e.TotalValue = 0;
                        else
                            e.TotalValue = Val.Format(Math.Round((TNewAmt / DouTCarat), 2), "n2");
                        //Math.Round(Val.Val(TNewAmt) / Val.Val(DouTCarat), 2);
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        private void txtAadat_TextChanged(object sender, EventArgs e)
        {
            try
            {
                double Amt = 0, NewAmt = 0;
                for (int i = 0; i < GridDetail.RowCount; i++)
                {
                    Amt = Val.ToDouble(GridDetail.GetRowCellValue(i, "OLDAVERAGE"));
                    NewAmt = Amt * Val.ToDouble(txtAadat.Text) * Val.ToDouble(txtRate.Text);
                    GridDetail.SetRowCellValue(i, "AVERAGE", Val.Format(NewAmt, "n2"));
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
      
        private void pivotGrid_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            try
            {              
                double Carat = 0;
                if (e.DataField == colRate)
                {
                    PivotDrillDownDataSource ds = e.CreateDrillDownDataSource();
                    for (int i = 0; i < ds.RowCount; i++)
                    {
                        ds.SetValue(i, e.DataField, Val.ToDouble(e.Editor.EditValue));
                        Carat = Val.ToDouble(ds.GetValue(i, "CARAT"));
                        ds.SetValue(i, "AMOUNT", Val.Format(Val.ToDouble(e.Editor.EditValue) * Carat, "n2"));
                    }
                }
            }
             
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void pivotGrid_CustomCellValue(object sender, PivotCellValueEventArgs e)
        {
            try
            {
                double TCarat = 0, TAmount = 0;                
                if (object.ReferenceEquals(e.DataField, colRate))
                {
                    TCarat = Val.ToDouble(e.GetCellValue(colCarat));
                    TAmount = Val.ToDouble(e.GetCellValue(colAmount));
                    e.Value = TAmount == 0 ? 0 : Math.Round(TAmount / TCarat, 2);                    
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        #endregion

        private void GridDetail_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                //if (e.RowHandle < 0)
                //{
                //    return;
                //}

                //double TRate = 0;
                //if (Val.ToDouble(txtAadat.Text) > 0 || Val.ToDouble(txtRate.Text) > 0)
                //{
                //    TRate = Math.Round(Val.Val(GridDetail.GetFocusedRowCellValue("AVERAGE")) / Val.ToDouble(txtAadat.Text) / Val.ToDouble(txtRate.Text), 2);
                //    PivotDrillDownDataSource ds = pivotGrid.CreateDrillDownDataSource();
                //   ds.SetValue(0, colRate, TRate);
                //}               
            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
            }
        }

        //Added by Daksha on 01/07/2023
        private void dgvParcelFancy_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            try
            {
                GridView view = sender as GridView;
                string strFieldName = "";
                if (e.IsTotalSummary)
                {
                    DevExpress.XtraGrid.GridColumnSummaryItem objObject;
                    objObject = (GridColumnSummaryItem)e.Item;
                    strFieldName = objObject.FieldName;
                }
                if (e.IsGroupSummary)
                {
                    DevExpress.XtraGrid.GridGroupSummaryItem objObject;
                    objObject = (GridGroupSummaryItem)e.Item;
                    strFieldName = objObject.FieldName;
                }
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    DouTAmt = 0;
                    DouTCarat = 0;
                }

                if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DouTCarat = DouTCarat + Val.ToDouble(view.GetRowCellValue(e.RowHandle, "Carat"));
                    DouTAmt = DouTAmt + Val.ToDouble(view.GetRowCellValue(e.RowHandle, "Amount"));
                }
                if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    if (strFieldName == "Rate")
                    {
                        if (DouTCarat == 0)
                            e.TotalValue = 0;
                        else
                            e.TotalValue = Math.Round(DouTAmt / DouTCarat, 0, MidpointRounding.AwayFromZero);
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        //End as Daksha

        //Added by Daksha on 21/08/2023
        private void btnBombayTransfer_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.Confirm("Are You sure,You want to Transfer this Kapan?") == DialogResult.No)
                {
                    return;
                }
                ParcelKapanInwardProperty Property = new ParcelKapanInwardProperty();
                Property.KAPANNAME = txtKapan.Text;
                Property.CARAT = Val.ToDouble(txtTotalWeight.Text);
                Property.COSTPRICEPERCARAT = Val.ToDouble(txtTotalAverage.Text);
                Property.COSTAMOUNT = Math.Round(Val.ToDouble(txtTotalWeight.Text) * Val.ToDouble(txtTotalAverage.Text), 3);
                Property = ObjKapan.Parcel_BombayTransferNew_Insert(Property);

                this.Cursor = Cursors.Default;

                Global.Message(Property.ReturnMessageDesc);
                if (Property.ReturnMessageType == "SUCCESS")
                {
                    BtnSearch_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtAadat.Text = "0.97";
                txtLotNo.Text = string.Empty;

                DtpFromDate.Checked = false;
                DtpToDate.Checked = false;
                IsBombayTransfer = false;
                txtKapan.Text = string.Empty;

                pivotGrid.DataSource = null;
                MainGrid.DataSource = null;
                GrdMain.DataSource = null;
                MainGridView.DataSource = null;

                GCFancyDetail.DataSource = null;
                GCFancySumary.DataSource = null;
                Calculation();

                BtnSearch.Enabled = true;
                txtKapan.Focus();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
        //End as Daksha
    }
}
