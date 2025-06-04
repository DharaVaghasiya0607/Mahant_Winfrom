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


namespace MahantExport.Parcel
{
    public partial class FrmSizeAssortment  : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_KapanInward ObjKapan = new BOTRN_KapanInward();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();

        DataTable DTabDetail = new DataTable();

        #region Property

        public FrmSizeAssortment()
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
            //if (txtShape.Text.Length == 0)
            //{
            //    Global.Message("Shape Name Is Required..");
            //    return false;
            //}
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
                foreach(DataRow DR in DTabDetail.Rows)
                {
                    DR["SHAPE_ID"] = Val.ToInt32(txtShape.Tag);
                }
                DTabDetail.AcceptChanges();
                DTabDetail.TableName = "Table";
                string StrXMLValues = string.Empty;
                Property.SHAPE_ID = Val.ToInt32(txtShape.Tag);

                using (StringWriter sw = new StringWriter())
                {
                    DTabDetail.WriteXml(sw);
                    StrXMLValues = sw.ToString();
                }

                Property.INWARD_ID = Guid.Parse(Val.ToString(txtKapan.Tag));
                Property.KAPANNAME = Val.ToString(txtKapan.Text);

                Property.StrInwardXml = StrXMLValues;

                Property = ObjKapan.SizeAssortmentSave(Property);

                this.Cursor = Cursors.Default;

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    DTabDetail.Rows.Clear();

                    txtKapan.Text = string.Empty;
                    txtKapan.Tag = string.Empty;

                    //txtShape.Text = string.Empty;
                    //txtShape.Tag = string.Empty;

                    txtBalanceCarat.Text = string.Empty;
                    txtInwardCarat.Text = string.Empty;

                    BtnSearch_Click(null, null);
                    txtShape.Focus();
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
            Global.ExcelExport("SizeAssortment", GrdDetSize);
        }
   
        private void BtnDelete_Click(object sender, EventArgs e)
        {
            ParcelKapanInwardProperty Property = new ParcelKapanInwardProperty();
            try
            {

                if (Global.Confirm("Are Your Sure To Delete The Record ?") == System.Windows.Forms.DialogResult.No)
                    return;

                FrmPassword FrmPassword = new FrmPassword();
                FrmPassword.ShowForm(ObjPer.PASSWORD);

                Property.INWARD_ID = Guid.Parse(Val.ToString(txtKapan.Tag));
                Property.SHAPE_ID = Val.ToInt32(txtShape.Tag);

                Property = ObjKapan.SizeAssortmentDelete(Property);
                Global.Message(Property.ReturnMessageDesc);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    DTabDetail.Rows.Clear();

                    txtKapan.Text = string.Empty;
                    txtKapan.Tag = string.Empty;

                    txtShape.Text = string.Empty;
                    txtShape.Tag = string.Empty;

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

                DataTable DTabSummury = ObjKapan.SizeAssortmentGetKapanData(txtSearchInwardNo.Text, 
                                txtSearchKapanName.Text, 
                                Val.SqlDate(DtpFromDate.Value.ToShortDateString()), 
                                Val.SqlDate(DtpToDate.Value.ToShortDateString()), 
                                StrStatus);
                MainGrdSummry.DataSource = DTabSummury;
                MainGrdSummry.Refresh();

                GrdDetSummry.Columns["KAPANNAME"].Group();
                GrdDetSummry.Columns["KAPANNAME"].Visible = false;

                if (GrdDetSummry.GroupSummary.Count == 0)
                {
                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, "KAPANNAME", GrdDetSummry.Columns["KAPANNAME"], "{0:N0}");
                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "CARAT", GrdDetSummry.Columns["CARAT"], "{0:N3}");
                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "PENDINGCARAT", GrdDetSummry.Columns["PENDINGCARAT"], "{0:N3}");
                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "ASSORTEDCARAT", GrdDetSummry.Columns["ASSORTEDCARAT"], "{0:N3}");
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


                        DRow["PER"] = DouPer;
                    }

                    DTabDetail.AcceptChanges();

                    //txtDiff.Text = Val.ToString(Math.Round(Val.Val(txtInwardCarat.Text) - (DouCarat + DouLessPlus), 3));
                    txtDiff.Text = Val.ToString(Math.Round(Val.Val(txtBalanceCarat.Text) - (DouCarat + DouLessPlus), 3));
                }
            }
            catch
            {
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
            
            Calculate();
            if (e.Column.FieldName == "CARAT")
            {
                if (Val.Val(txtDiff.Text) < 0)
                {
                    Global.Message("Not Enough Balance For Assortment, Carat Difference Is [" + txtDiff.Text + "]");
                    GrdDetSize.SetRowCellValue(e.RowHandle, "CARAT", 0); ;
                }
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

               
                txtKapan.Text = Val.ToString(GrdDetSummry.GetRowCellValue(IntRowhandle, "KAPANNAME"));
                txtKapan.Tag = Val.ToString(GrdDetSummry.GetRowCellValue(IntRowhandle, "INWARD_ID"));

                txtShape.Text = Val.ToString(GrdDetSummry.GetRowCellValue(IntRowhandle, "SHAPENAME"));
                txtShape.Tag = Val.ToString(GrdDetSummry.GetRowCellValue(IntRowhandle, "SHAPE_ID"));

                //txtBalanceCarat.Text = Val.ToString(GrdDetSummry.GetRowCellValue(IntRowhandle, "PENDINGCARAT"));
                txtPendingCts.Text = Val.ToString(GrdDetSummry.GetRowCellValue(IntRowhandle, "PENDINGCARAT"));
                txtAssortedCts.Text = Val.ToString(GrdDetSummry.GetRowCellValue(IntRowhandle, "ASSORTEDCARAT"));
                txtInwardCarat.Text = Val.ToString(GrdDetSummry.GetRowCellValue(IntRowhandle, "CARAT"));

                lblTitle.Text = "Size Wise Assortment [ " + Val.ToString(GrdDetSummry.GetRowCellValue(IntRowhandle, "INWARDNO")) + " ]";
                txtShape.Text = "ROUND";
                txtShape.Tag = 1;
                LoadData();
              
                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }


        public void LoadData()
        {
            Guid InwardID = Val.ToGuid(GrdDetSummry.GetFocusedRowCellValue("INWARD_ID"));

            if (Val.Trim(txtShape.Text).Length == 0)
            {
                DTabDetail.Rows.Clear();
            }
           
            else
            {
                DTabDetail = ObjKapan.SizeAssortmentGetSizeData(InwardID, Val.ToInt(txtShape.Tag));
            }   

            MainGridSize.DataSource = DTabDetail;
            MainGridSize.Refresh();
            txtBalanceCarat.Text = Val.ToString(Val.ToDouble(DTabDetail.Compute("SUM(CARAT)", "")) + Val.ToDouble(DTabDetail.Compute("SUM(LESSPLUSCARAT)", "")) + Val.ToDouble(txtPendingCts.Text));//Gunjan:22/04/2023
            Calculate();
        }


        private void GrdDetSummry_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            FetchRecord(e.FocusedRowHandle);
        }

        private void GrdDetSize_KeyDown(object sender, KeyEventArgs e)
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

        #endregion

        #region Keypress

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

                        LoadData();
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
