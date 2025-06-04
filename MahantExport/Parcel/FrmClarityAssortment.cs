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
using DevExpress.Data;


namespace MahantExport.Parcel
{
    public partial class FrmClarityAssortment  : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_KapanInward ObjKapan = new BOTRN_KapanInward();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();

        DataTable DTabDetail = new DataTable();

        double DouCarat = 0;
        double DouAmount = 0;

        #region Property

        public FrmClarityAssortment()
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

            DtpFromDate.Value = DateTime.Now.AddMonths(-1);
            DtpToDate.Value = DateTime.Now;
            BtnSearch_Click(null, null);

            LookUpShape.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SHAPE);
            LookUpShape.Properties.ValueMember = "SHAPE_ID";
            LookUpShape.Properties.DisplayMember = "SHAPENAME";

            LookUpSize.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_MIXSIZE);
            LookUpSize.Properties.ValueMember = "MIXSIZE_ID";
            LookUpSize.Properties.DisplayMember = "MIXSIZENAME";


            this.Show();

        }

        #endregion

        #region Validation


        private bool ValSave()
        {

            if (Val.Val(txtDiff.Text) < 0)
            {
                Global.Message("Error In Balance Mismatch [" + txtDiff.Text + "]..");
                return false;
            }
            if (txtKapan.Text.Length == 0)
            {
                Global.Message("Kapan Name Is Required..");
                return false;
            }
            if (LookUpShape.Text.Length == 0)
            {
                Global.Message("Shape Name Is Required..");
                return false;
            }
            if (txtDepartment.Text.Length == 0)
            {
                Global.Message("Department Name Is Required..");
                txtDepartment.Focus();
                return false;
            }
            if (Val.Val(txtInwardCarat.Text) == 0)
            {
                Global.Message("Inward Carat Is Required..");
                return false;
            }


            return true;
        }


        #endregion

        #region Control Event

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValSave() == false)
                {
                    return;
                }

                //if (Global.Confirm("Are You Sure To Save This Entry ?") == System.Windows.Forms.DialogResult.No)
                //{
                //    return;
                //}

                this.Cursor = Cursors.WaitCursor;

                ParcelKapanInwardProperty Property = new ParcelKapanInwardProperty();
                DTabDetail.TableName = "Table";
                string StrXMLValues = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabDetail.WriteXml(sw);
                    StrXMLValues = sw.ToString();
                }
                Property.KAPANNAME = Val.ToString(txtKapan.Text);//add by gunjan:09/03/2023
                Property.SIZEASSORT_ID = Val.ToGuid(Val.ToString(txtKapan.Tag));
                Property.SHAPE_ID = Val.ToInt(LookUpShape.EditValue);//add by gunjan:09/03/2023
                Property.MIXSIZE_ID = Val.ToInt32(LookUpSize.EditValue);//add by gunjan:09/03/2023
                Property.DEPARTMENT_ID = Val.ToInt(txtDepartment.Tag);
                Property.StrInwardXml = StrXMLValues;

                Property = ObjKapan.ClarityAssortmentSave(Property);

                this.Cursor = Cursors.Default;

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    DTabDetail.Rows.Clear();
                    txtKapan.Text = string.Empty;
                    txtKapan.Tag = string.Empty;
                    LookUpShape.Text = string.Empty;
                    LookUpShape.Tag = string.Empty;
                    txtBalanceCarat.Text = string.Empty;
                    txtInwardCarat.Text = string.Empty;
                    BtnSearch_Click(null, null);
                    txtDepartment.Focus();
                }
                else
                {
                    Global.Message(Property.ReturnMessageDesc);
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

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("ClarityAssortment", GrdDetSize);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            ParcelKapanInwardProperty Property = new ParcelKapanInwardProperty();
            try
            {

                if (Global.Confirm("Are Your Sure To Delete The Record ?") == System.Windows.Forms.DialogResult.No)
                    return;

                Property.KAPANNAME = Val.ToString(txtKapan.Text);//add by gunjan:16/03/2023
                Property.SHAPE_ID = Val.ToInt(LookUpShape.EditValue);//add by gunjan:16/03/2023
                Property.MIXSIZE_ID = Val.ToInt32(LookUpSize.EditValue);//add by gunjan:16/03/2023
                Property.DEPARTMENT_ID = Val.ToInt(txtDepartment.Tag);

                Property = ObjKapan.ClarityAssortmentDelete(Property);
                Global.Message(Property.ReturnMessageDesc);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    DTabDetail.Rows.Clear();

                    txtKapan.Text = string.Empty;
                    txtKapan.Tag = string.Empty;

                    LookUpShape.Text = string.Empty;
                    LookUpShape.Tag = string.Empty;

                    txtBalanceCarat.Text = string.Empty;
                    txtInwardCarat.Text = string.Empty;

                    BtnSearch_Click(null, null);

                }

            }
            catch (System.Exception ex)
            {
                Global.MessageToster(ex.Message);
            }
            Property = null;
        }


        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string StrStatus = ",";
                if (ChkPending.Checked == true)
                {
                    StrStatus = StrStatus + "PENDING,";
                }
                if (ChkComplete.Checked == true)
                {
                    StrStatus = StrStatus + "COMPLETE,";
                }
                if (ChkPartial.Checked == true)
                {
                    StrStatus = StrStatus + "PARTIAL,";
                }
                StrStatus = StrStatus + ",";

                Guid gUser_ID = Val.ToString(txtUser_ID.Text).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtUser_ID.Tag));

                DataTable DTabSummury = ObjKapan.ClarityAssortmentGetKapanData(//txtSearchInwardNo.Text,
                                txtSearchKapanName.Text,
                                Val.SqlDate(DtpFromDate.Value.ToShortDateString()),
                                Val.SqlDate(DtpToDate.Value.ToShortDateString()),
                                StrStatus,
                                "",
                                gUser_ID
                                );
                MainGrdSummry.DataSource = DTabSummury;
                MainGrdSummry.Refresh();

                GrdDetSummry.Columns["KAPANNAME"].Group();
                GrdDetSummry.Columns["KAPANNAME"].Visible = false;

                if (GrdDetSummry.GroupSummary.Count == 0)
                {
                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, "KAPANNAME", GrdDetSummry.Columns["KAPANNAME"], "{0:N0}");
                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "InwardCarat", GrdDetSummry.Columns["InwardCarat"], "{0:N3}");
                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "PendingCarat", GrdDetSummry.Columns["PendingCarat"], "{0:N3}");
                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "AssortCarat", GrdDetSummry.Columns["AssortCarat"], "{0:N3}");
                }

                //Added by Daksha on 24/08/2023
                string[] SplitKapan = txtSearchKapanName.Text.Split(',');
                if (SplitKapan.Length == 1 && SplitKapan[0] != "")
                {
                    GrdDetSummry.ExpandAllGroups();
                }                
                //End as Daksha

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }

        }

        #endregion

        #region Grid Detail

        private void GrdDetSummry_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }

                string StrStatus = Val.ToString(GrdDetSummry.GetRowCellValue(e.RowHandle, "STATUS"));
                if (StrStatus == "PENDING")
                {
                    e.Appearance.BackColor = lblPending.BackColor;
                    e.Appearance.BackColor2 = lblPending.BackColor;
                }
                else if (StrStatus == "PARTIAL")
                {
                    e.Appearance.BackColor = lblPartial.BackColor;
                    e.Appearance.BackColor2 = lblPartial.BackColor;
                }
                else if (StrStatus == "COMPLETE")
                {
                    e.Appearance.BackColor = Color.Transparent;
                    e.Appearance.BackColor2 = Color.Transparent;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        public void Calculate()
        {
            try
            {
                double DouCarat = 0;
                double DouLessPlus = 0;

                if (DTabDetail != null && DTabDetail.Rows.Count != 0)
                {

                    double DouTotalCarat = Val.Val(DTabDetail.Compute("SUM(CARAT)", ""));

                    double DouPer = 0;

                    for (int IntI = 0; IntI < GrdDetSize.RowCount; IntI++)
                    {
                        DataRow DRow = GrdDetSize.GetDataRow(IntI);
                        DouCarat = DouCarat + Val.Val(DRow["CARAT"]);
                        DouLessPlus = DouLessPlus + Val.Val(DRow["LESSPLUSCARAT"]);


                        if (DouTotalCarat != 0)
                        {
                            DouPer = Math.Round((Val.Val(DRow["CARAT"]) / DouTotalCarat) * 100, 2);
                        }
                        //DRow["AMOUNT"] = Math.Round(Val.Val(DRow["PRICEPERCARAT"]) * Val.Val(DRow["CARAT"]), 2);
                        DRow["PER"] = DouPer;
                    }

                    //DTabDetail.AcceptChanges();

                    //double DouMainCarat = Val.Val(GrdDetSummry.GetFocusedRowCellValue("CARAT"));
                    //double DouAssCarat = Val.Val(GrdDetSummry.GetFocusedRowCellValue("ASSORTEDCARAT"));

                    //double DouOtherAssortment = 0;
                    //if (DTabDetail.Rows.Count != 0)
                    //{
                    //    DouOtherAssortment = Val.Val(DTabDetail.Rows[0]["OTHERASSORTMENT"]);
                    //}

                    //txtDiff.Text = Val.ToString(Math.Round(DouMainCarat - DouOtherAssortment - (DouCarat + DouLessPlus), 3));

                    txtDiff.Text = Val.ToString(Math.Round(Val.Val(txtInwardCarat.Text) - (DouCarat + DouLessPlus), 3));
                }

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void GrdDetSummry_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            FetchRecord(e.RowHandle);
        }

        private void GrdDetSize_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000")
            {
                e.DisplayText = String.Empty;
            }
        }

        private void GrdDetSize_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                Calculate();
                if (e.Column.FieldName == "CARAT")
                {
                    if (Val.Val(txtDiff.Text) < 0)
                    {
                        Global.Message("Not Enough Balance For Assortment, Carat Difference Is [" + txtDiff.Text + "]");
                        DTabDetail.Rows[e.RowHandle]["CARAT"] = 0;
                        DTabDetail.AcceptChanges();

                        //                    GrdDetSize.SetRowCellValue(e.RowHandle, "CARAT", 0); ;
                    }
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        public void FetchRecord(int IntRowhandle)
        {
            try
            {
                if (IntRowhandle < 0)
                {
                    return;
                }
                this.Cursor = Cursors.WaitCursor;

                string SizeAssortID = Val.ToString(GrdDetSummry.GetRowCellValue(IntRowhandle, "SIZEASSORT_ID"));
                txtKapan.Text = Val.ToString(GrdDetSummry.GetRowCellValue(IntRowhandle, "KAPANNAME"));
                txtKapan.Tag = Val.ToString(GrdDetSummry.GetRowCellValue(IntRowhandle, "SIZEASSORT_ID"));

                LookUpShape.EditValue = Val.ToString(GrdDetSummry.GetRowCellValue(IntRowhandle, "SHAPE_ID"));
                LookUpSize.EditValue = Val.ToString(GrdDetSummry.GetRowCellValue(IntRowhandle, "MIXSIZE_ID"));

                txtBalanceCarat.Text = Val.ToString(GrdDetSummry.GetRowCellValue(IntRowhandle, "PendingCarat"));
                txtInwardCarat.Text = Val.ToString(GrdDetSummry.GetRowCellValue(IntRowhandle, "InwardCarat"));

                lblTitle.Text = "Clarity Wise Assortment Of [ " + Val.ToString(GrdDetSummry.GetRowCellValue(IntRowhandle, "KAPANNAME")) + " / " + Val.ToString(GrdDetSummry.GetRowCellValue(IntRowhandle, "MIXSIZENAME")) + " ]";

                txtDepartment.Focus();

                DTabDetail.Rows.Clear();
               // txtDepartment.Clear();
               
                LoadData();
                txtDepartment.Text = "ADMIN";
                txtDepartment.Tag = 436;
                LoadDataForSize();

                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void GrdDetSummry_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            FetchRecord(e.FocusedRowHandle);
        }

        private void GrdDetSize_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (GrdDetSize.FocusedRowHandle < 0)
                {
                    return;
                }

                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    GrdDetSize.FocusedRowHandle = GrdDetSize.FocusedRowHandle + 1;
                    GrdDetSize.FocusedColumn = GrdDetSize.Columns["CARAT"];
                    GrdDetSize.Focus();
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void GrdDetSummry_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000")
            {
                e.DisplayText = String.Empty;
            }
        }

        private void GrdDetSize_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            try
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    DouCarat = 0;
                    DouAmount = 0;

                }
                else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DouCarat = DouCarat + Val.Val(GrdDetSize.GetRowCellValue(e.RowHandle, "CARAT"));
                    DouAmount = DouAmount + (Val.Val(GrdDetSize.GetRowCellValue(e.RowHandle, "CARAT")) * Val.Val(GrdDetSize.GetRowCellValue(e.RowHandle, "PRICEPERCARAT")));
                }
                else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("PRICEPERCARAT") == 0)
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

        public void LoadData()
        {
            Guid SizeAssortID = Val.ToGuid(GrdDetSummry.GetFocusedRowCellValue("SIZEASSORT_ID"));

            if (Val.Trim(txtDepartment.Text).Length == 0)
            {
                DTabDetail.Rows.Clear();
            }

            else
            {
                DTabDetail = ObjKapan.ClarityAssortmentGetSizeData("", SizeAssortID, Val.ToInt(txtDepartment.Tag), "", 0);
            }

            MainGridSize.DataSource = DTabDetail;
            MainGridSize.Refresh();
            Calculate();
        }

        public void LoadDataForSize()
        {
            Guid SizeAssortID = Val.ToGuid(GrdDetSummry.GetFocusedRowCellValue("SIZEASSORT_ID"));
            string KapanName = Val.ToString(GrdDetSummry.GetFocusedRowCellValue("KAPANNAME"));
            if (Val.Trim(txtDepartment.Text).Length == 0)
            {
                DTabDetail.Rows.Clear();
            }

            else
            {
                DTabDetail = ObjKapan.ClarityAssortmentGetSizeData(KapanName, SizeAssortID, Val.ToInt(txtDepartment.Tag), Val.ToString(LookUpSize.EditValue), Val.ToInt32(LookUpShape.EditValue));

            }
            MainGridSize.DataSource = DTabDetail;
            MainGridSize.Refresh();
            if (DTabDetail.Rows.Count > 0)
            {
                lblTitle.Text = "Clarity Wise Assortment Of [ " + Val.ToString(DTabDetail.Rows[0]["SIZEASSORTNO"].ToString()) + " / " + Val.ToString(LookUpSize.Text) + " ]";
                //txtInwardCarat.Text = DTabDetail.Rows[0]["SIZEASSORTMENTCARAT"].ToString();
                //txtBalanceCarat.Text = DTabDetail.Rows[0]["PENDINGCARAT"].ToString();
                //txtInwardNo.Text = DTabDetail.Rows[0]["INWARDNO"].ToString();
            }
            else
            {
                lblTitle.Text = "Clarity Wise Assortment";
                txtBalanceCarat.Text = string.Empty;
                txtInwardCarat.Text = string.Empty;
                txtInwardNo.Text = string.Empty;
            }
            Calculate();
        }

        private void GrdDetSize_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                if (DTabDetail.Rows.Count > 0)
                {
                    Int32 pIntMixClarity_ID = Val.ToInt32(GrdDetSize.GetFocusedRowCellValue("MIXCLARITY_ID"));
                    Int32 pIntMixSizeSeqNo = Val.ToInt32(GrdDetSize.GetFocusedRowCellValue("MIXSIZESEQNO"));

                    Int32 pIntMixSize90Up = Val.ToInt32(GrdDetSize.GetFocusedRowCellValue("MIXSIZE90UP_SEQNO"));

                    if (pIntMixSizeSeqNo >= pIntMixSize90Up || pIntMixClarity_ID == 1727 || pIntMixClarity_ID == 1728) //1728=GIA 1727=Mix Clarity) 
                    {
                        GrdDetSize.Columns["PRICEPERCARAT"].OptionsColumn.AllowEdit = true;
                    }
                    else
                    {
                        GrdDetSize.Columns["PRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                    }
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void GrdDetSize_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            //try
            //{
            //    if (e.FocusedColumn.FieldName == "PRICEPERCARAT")
            //    {
            //        Int32 pIntMixClarity_ID = Val.ToInt32(GrdDetSize.GetFocusedRowCellValue("MIXCLARITY_ID"));
            //        Int32 pIntMixSizeSeqNo = Val.ToInt32(GrdDetSize.GetFocusedRowCellValue("MIXSIZESEQNO"));

            //        Int32 pIntMixSize90Up = Val.ToInt32(GrdDetSize.GetFocusedRowCellValue("MIXSIZE90UP_SEQNO"));

            //        if (pIntMixSizeSeqNo > pIntMixSize90Up && pIntMixClarity_ID == 1727)
            //        {
            //            GrdDetSize.Columns["PRICEPERCARAT"].OptionsColumn.AllowEdit = true;
            //        }
            //        else
            //        {
            //            GrdDetSize.Columns["PRICEPERCARAT"].OptionsColumn.AllowEdit = false;
            //        }
            //    }
            //}
            //catch (Exception Ex)
            //{
            //    Global.Message(Ex.Message.ToString());
            //}
        }
        #endregion

        #region KeyPress

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
                        txtSearchInwardNo.Text = Val.ToString(FrmSearch.DRow["INWARDNO"]);
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


      
        private void txtDepartment_KeyPress(object sender, KeyPressEventArgs e)
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
                        txtDepartment.Text = Val.ToString(FrmSearch.DRow["DEPARTMENTNAME"]);
                        txtDepartment.Tag = Val.ToString(FrmSearch.DRow["DEPARTMENT_ID"]);
                        //LoadData();
                        LoadDataForSize();
                        
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

     
        private void txtKapan_Validated(object sender, EventArgs e)
        {
            LoadDataForSize();
        }

        private void txtKapan_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "KAPANNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_KAPAN);
                    //FrmSearch.ColumnsToHide = "DEPARTMENT_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtKapan.Text = Val.ToString(FrmSearch.DRow["KAPANNAME"]);
                        LoadDataForSize();
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

        private void txtShape_KeyPress(object sender, KeyPressEventArgs e)
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
                        txtShape.Text = Val.ToString(FrmSearch.DRow["SHAPENAME"]);
                        txtShape.Tag = Val.ToString(FrmSearch.DRow["SHAPE_ID"]);
                        LoadDataForSize();
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

        private void txtSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "MIXSIZENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_MIXSIZE);
                    FrmSearch.mStrColumnsToHide = "MIXSIZE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtSize.Text = Val.ToString(FrmSearch.DRow["MIXSIZENAME"]);
                        txtSize.Tag = Val.ToString(FrmSearch.DRow["MIXSIZE_ID"]);
                        LoadDataForSize();
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

        private void LookUpSize_EditValueChanged(object sender, EventArgs e)
        {
            LoadDataForSize();
        }

        private void LookUpShape_EditValueChanged(object sender, EventArgs e)
        {
            LoadDataForSize();
        }

        private void LookUpShape_Validating(object sender, CancelEventArgs e)
        {
            LoadDataForSize();
        }

        private void txtUser_ID_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "EMPLOYEECODE,EMPLOYEENAME";
                    FrmSearch.mStrColumnsToHide = "EMPLOYEE_ID,,DEPARTMENT_ID,DEPARTMENTNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_EMPLOYEE);
                    // FrmSearch.ColumnsToHide = "EMPLOYEE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtUser_ID.Text = Val.ToString(FrmSearch.DRow["EMPLOYEENAME"]);
                        txtUser_ID.Tag = Val.ToString(FrmSearch.DRow["EMPLOYEE_ID"]);
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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtInwardNo_KeyPress(object sender, KeyPressEventArgs e)
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
                        txtInwardNo.Text = Val.ToString(FrmSearch.DRow["INWARDNO"]);
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
        #endregion
    }
}
