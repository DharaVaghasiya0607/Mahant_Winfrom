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


namespace MahantExport.Stock
{
    public partial class FrmJangedEntry  : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_Janged ObjKapan = new BOTRN_Janged();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();

        DataTable DTabDetail = new DataTable();

        public FrmJangedEntry()
        {
            InitializeComponent();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
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

            DtpFromDate.Value = DateTime.Now; //DateTime.Now.AddMonths(-1);
            DtpToDate.Value = DateTime.Now;

            DTabDetail = ObjKapan.FillDetail(Guid.NewGuid().ToString());
            MainGrid.DataSource = DTabDetail;
            MainGrid.Refresh();

            BtnSearch_Click(null, null);
            this.Show();

        }

        #region Button Events


        private void txtThrough_Validated(object sender, EventArgs e)
        {
            DataRow Dr = DTabDetail.NewRow();
            Dr["SRNO"] = DTabDetail.Rows.Count + 1;
            DTabDetail.Rows.Add(Dr);

            GrdDet.Focus();
            GrdDet.PostEditor();
            GrdDet.FocusedRowHandle = GrdDet.RowCount - 1;
            GrdDet.FocusedColumn = GrdDet.Columns["SRNO"];
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtParty.Text.Trim() == "" && txtBroker.Text.Trim() == "")
                {
                    Global.Message("Party Or Broker One Of Them Is Required..");
                    txtParty.Focus();
                    return;
                }

                if (txtParty.Text.Trim() == "")
                {
                    txtParty.Tag = "";
                }
                if (txtBroker.Text.Trim() == "")
                {
                    txtBroker.Tag = "";
                }

                if (lblAddMode.Text == "Add Mode")
                {
                    this.Cursor = Cursors.WaitCursor;

                    int IntRow = 0;
                    foreach (DataRow DRow in DTabDetail.Rows)
                    {
                        IntRow++;
                        DRow["SRNO"] = IntRow;
                    }
                    DTabDetail.AcceptChanges();

                    TRN_JangedProperty Property = new TRN_JangedProperty();
                    if (Val.ToString(txtJangedNo.Tag) == "")
                    {
                        txtJangedNo.Tag = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                    }

                    Property.Janged_ID = Val.ToString(txtJangedNo.Tag);
                    Property.JangedDate = Val.SqlDate(DTPJangedDate.Value.ToShortDateString());
                    Property.FinYear = Global.GetFinancialYear(DTPJangedDate.Value.ToShortDateString());
                    Property.JangedNo = Val.ToInt(txtJangedNo.Text);

                    Property.Currency = (RbDollar.Checked) ? RbDollar.Text.ToString() : RbRupee.Text.ToString();

                    //Property.Party_ID = Val.ToString(txtParty.Tag).Trim().Equals(string.Empty) ? Guid.Empty.ToString() : Val.ToString(txtParty.Tag);
                    if (txtParty.Text.Trim().Length == 0)
                    {
                        Property.Party_ID = null;
                    }
                    else
                    {
                        Property.Party_ID = Val.ToString(txtParty.Tag);
                    }
                    if (txtBroker.Text.Trim().Length == 0)
                    {
                        Property.Broker_ID = null;
                    }
                    else
                    {
                        Property.Broker_ID = Val.ToString(txtBroker.Tag);
                    }
                    Property.Through = txtThrough.Text;

                    DTabDetail.TableName = "Table";
                    string StrXMLValuesInsert = string.Empty;
                    using (StringWriter sw = new StringWriter())
                    {
                        DTabDetail.WriteXml(sw);
                        StrXMLValuesInsert = sw.ToString();
                    }

                    Property.DetailXML = StrXMLValuesInsert;

                    Property = ObjKapan.Save(Property);
                    txtJangedNo.Text = Property.ReturnJangedNo;
                    txtJangedNoStr.Text = Property.ReturnJangedStr;

                    this.Cursor = Cursors.Default;

                   // Global.Message(Property.ReturnMessageDesc);

                    if (Property.ReturnMessageType == "SUCCESS")
                    {
                        // BtnClear_Click(null, null);
                        BtnPrint_Click(null, null);
                        BtnSearch_Click(null, null);
                    }
                }
                else
                {
                    BtnPrint_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;

                Global.Message(ex.Message);
            }
        }


        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtJangedNo.Text = string.Empty;
            txtJangedNoStr.Text = string.Empty;
            txtParty.Text = string.Empty;
            txtParty.Tag = string.Empty;
            txtBroker.Text = string.Empty;
            txtBroker.Tag = string.Empty;
            txtThrough.Text = string.Empty;
            DTabDetail.Rows.Clear();
            txtJangedNo.Tag = string.Empty;
            RbDollar.Checked = true;
            lblAddMode.Text = "Add Mode";
            DTPJangedDate.Text = DateTime.Now.ToShortDateString();
            DTPJangedDate.Focus();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("JangedExport" + txtJangedNo.Text, GrdDet);
        }

        private void BtnExportSummary_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("JangedSummary", GrdSummury);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            TRN_JangedProperty Property = new TRN_JangedProperty();
            try
            {

                if (Global.Confirm("Are Your Sure To Delete The Record ?") == System.Windows.Forms.DialogResult.No)
                    return;

                FrmPassword FrmPassword = new FrmPassword();
                if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;
                    Property.Janged_ID = Val.ToString(txtJangedNo.Tag);
                    Property = ObjKapan.Delete(Property);
                    this.Cursor = Cursors.Default;
                    Global.Message(Property.ReturnMessageDesc);

                    if (Property.ReturnMessageType == "SUCCESS")
                    {
                        BtnClear_Click(null, null);
                    }
                    else
                    {
                        DTPJangedDate.Focus();
                    }
                }

            }
            catch (System.Exception ex)
            {
                Global.MessageToster(ex.Message);
            }
            Property = null;
        }

        private void ReptxtRemark_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataRow Dro = GrdDet.GetFocusedDataRow();

                if (Val.Val(Dro["CARAT"]) != 0 && GrdDet.IsLastRow)
                {
                    DataRow DRNew = DTabDetail.NewRow();
                    DRNew["SRNO"] = DTabDetail.Rows.Count + 1;
                    DTabDetail.Rows.Add(DRNew);

                    GrdDet.FocusedRowHandle = GrdDet.RowCount - 1;
                    GrdDet.FocusedColumn = GrdDet.Columns["SRNO"];
                }
                else if (GrdDet.IsLastRow)
                {
                    BtnSave.Focus();
                    e.Handled = true;
                }
            }
        }

        private void ReptxtShapeName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle < 0)
                    return;

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
                        GrdDet.PostEditor();
                        DataRow Dr = GrdDet.GetFocusedDataRow();
                        GrdDet.SetFocusedRowCellValue("SHAPENAME", Val.ToString(FrmSearch.DRow["SHAPENAME"]));
                        GrdDet.SetFocusedRowCellValue("SHAPE_ID", Val.ToString(FrmSearch.DRow["SHAPE_ID"]));
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

        private void ReptxtClarityName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle < 0)
                    return;

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "MIXCLARITYCODE,MIXCLARITYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mBoolISPostBack = true;
                    FrmSearch.mStrISPostBackColumn = "MIXCLARITYNAME";
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_MIXCLARITY);
                    FrmSearch.mStrColumnsToHide = "MIXCLARITY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDet.PostEditor();
                        DataRow Dr = GrdDet.GetFocusedDataRow();
                        GrdDet.SetFocusedRowCellValue("CLARITYNAME", Val.ToString(FrmSearch.DRow["MIXCLARITYNAME"]));
                        GrdDet.SetFocusedRowCellValue("CLARITY_ID", Val.ToString(FrmSearch.DRow["MIXCLARITY_ID"]));
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

        private void ReptxtSizeName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle < 0)
                    return;

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
                        GrdDet.PostEditor();
                        DataRow Dr = GrdDet.GetFocusedDataRow();
                        GrdDet.SetFocusedRowCellValue("SIZENAME", Val.ToString(FrmSearch.DRow["MIXSIZENAME"]));
                        GrdDet.SetFocusedRowCellValue("SIZE_ID", Val.ToString(FrmSearch.DRow["MIXSIZE_ID"]));
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


        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DTabSummury = ObjKapan.FillSummary(
                                Val.SqlDate(DtpFromDate.Value.ToShortDateString()),
                                Val.SqlDate(DtpToDate.Value.ToShortDateString()));
                MainGrdSummury.DataSource = DTabSummury;
                MainGrdSummury.Refresh();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }

        }

        private void GrdDet_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }
                if (e.Column.FieldName == "RATE" || e.Column.FieldName == "CARAT")
                {
                    DataRow DRow = GrdDet.GetDataRow(e.RowHandle);
                    DTabDetail.Rows[e.RowHandle]["AMOUNT"] = Math.Round(Val.Val(DRow["CARAT"]) * Val.Val(DRow["RATE"]), 2);
                    DTabDetail.AcceptChanges();
                }
                if (e.Column.FieldName == "SHAPENAME" ||
                    e.Column.FieldName == "CLARITYNAME" ||
                    e.Column.FieldName == "SIZENAME"
                    )
                {
                    DataRow DRow = GrdDet.GetDataRow(e.RowHandle);
                    DTabDetail.Rows[e.RowHandle]["DESCRIPTION"] = Val.ToString(DRow["SHAPENAME"]) + "   " + Val.ToString(DRow["CLARITYNAME"]) + "   " + Val.ToString(DRow["SIZENAME"]);
                    DTabDetail.AcceptChanges();
                }
            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
            }
        }

        private void GrdSummury_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                return;
            }
            if (e.Clicks == 2)
            {
                this.Cursor = Cursors.WaitCursor;

                DTabDetail.Rows.Clear();
                DataRow DRow = GrdSummury.GetDataRow(e.RowHandle);

                lblAddMode.Text = "Edit Mode";

                string StrJangedID = Val.ToString(GrdSummury.GetRowCellValue(e.RowHandle, "JANGED_ID"));

                txtJangedNo.Text = Val.ToString(DRow["JANGEDNO"]);
                txtJangedNoStr.Text = Val.ToString(DRow["JANGEDNOSTR"]);
                txtJangedNo.Tag = Val.ToString(DRow["JANGED_ID"]);

                txtParty.Tag = Val.ToString(DRow["PARTY_ID"]);
                txtParty.Text = Val.ToString(DRow["PARTYNAME"]);

                txtBroker.Tag = Val.ToString(DRow["BROKER_ID"]);
                txtBroker.Text = Val.ToString(DRow["BROKERNAME"]);

                DTPJangedDate.Text = Val.ToString(DRow["JANGEDDATE"]);

                txtThrough.Text = Val.ToString(DRow["THROUGH"]);
                if (Val.ToString(DRow["CURRENCY"]) == "$")
                {
                    RbDollar.Checked = true;
                }
                else
                {
                    RbRupee.Checked = true;
                }

                DTabDetail = ObjKapan.FillDetail(Val.ToString(txtJangedNo.Tag));
                MainGrid.DataSource = DTabDetail;
                MainGrid.Refresh();
                txtThrough_Validated(null, null);
                this.Cursor = Cursors.Default;

            }
        }

        private void deleteSelectedAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle >= 0)
                {
                    if (Global.Confirm("ARE YOU SURE YOU WANT TO DELETE ENTRY") == System.Windows.Forms.DialogResult.Yes)
                    {
                        DTabDetail.Rows.RemoveAt(GrdDet.FocusedRowHandle);
                        DTabDetail.AcceptChanges();
                        //BtnSave_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }


        #endregion

        private void txtParty_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    //FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME,COMPANYNAME";
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;

                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);
                    FrmSearch.mStrColumnsToHide = "PARTY_ID,BILLINGCOUNTRY_ID,SHIPPINGCOUNTRY_ID,PARTYTYPE";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtParty.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtParty.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
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

        private void txtBroker_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_BROKERADAT);
                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBroker.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtBroker.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
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

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (Val.ToInt(txtJangedNo.Text) == 0)
            {
                Global.Message("Janged No Is Required");
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            DataTable DTabPrint = ObjKapan.Print(Val.ToString(txtJangedNo.Tag));
            if (DTabPrint.Rows.Count == 0)
            {
                this.Cursor = Cursors.Default;
                Global.Message("No Data Found");
                return;
            }

            int IntI = 13;
            if (DTabPrint.Rows.Count > 0)
            {
                IntI = IntI - Val.ToInt(DTabPrint.Rows.Count);
                for (int J = 0; J < IntI; J++)
                {
                    DTabPrint.Rows.Add(DTabPrint.NewRow());
                }
            }


            DTabPrint.TableName = "Original";
            DataSet DS = new DataSet();
            DS.Tables.Add(DTabPrint);

            DataTable DTabDuplicate = DTabPrint.Copy();
            foreach (DataRow DRow in DTabDuplicate.Rows)
            {
                DRow["PrintType"] = "Duplicate";
            }
            DTabDuplicate.TableName = "Duplicate";

            DTabDuplicate.AcceptChanges();
            DS.Tables.Add(DTabDuplicate);

            Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
            FrmReportViewer.MdiParent = Global.gMainRef;
            FrmReportViewer.ShowJangedReport("JangedPrintSubReport", DS);

            BtnClear_Click(null,null);
            this.Cursor = Cursors.Default;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblAddMode.Text == "Edit Mode")
                {
                    if (txtParty.Text.Trim() == "" && txtBroker.Text.Trim() == "")
                    {
                        Global.Message("Party Or Broker One Of Them Is Required..");
                        txtParty.Focus();
                        return;
                    }

                    if (txtParty.Text.Trim() == "")
                    {
                        txtParty.Tag = "";
                    }
                    if (txtBroker.Text.Trim() == "")
                    {
                        txtBroker.Tag = "";
                    }
                    
                    this.Cursor = Cursors.WaitCursor;

                    int IntRow = 0;
                    foreach (DataRow DRow in DTabDetail.Rows)
                    {
                        IntRow++;
                        DRow["SRNO"] = IntRow;
                    }
                    DTabDetail.AcceptChanges();

                    TRN_JangedProperty Property = new TRN_JangedProperty();
                    if (Val.ToString(txtJangedNo.Tag) == "")
                    {
                        txtJangedNo.Tag = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                    }

                    Property.Janged_ID = Val.ToString(txtJangedNo.Tag);
                    Property.JangedDate = Val.SqlDate(DTPJangedDate.Value.ToShortDateString());
                    Property.FinYear = Global.GetFinancialYear(DTPJangedDate.Value.ToShortDateString());
                    Property.JangedNo = Val.ToInt(txtJangedNo.Text);

                    Property.Currency = (RbDollar.Checked) ? RbDollar.Text.ToString() : RbRupee.Text.ToString();

                    //Property.Party_ID = Val.ToString(txtParty.Tag).Trim().Equals(string.Empty) ? Guid.Empty.ToString() : Val.ToString(txtParty.Tag);
                    if (txtParty.Text.Trim().Length == 0)
                    {
                        Property.Party_ID = null;
                    }
                    else
                    {
                        Property.Party_ID = Val.ToString(txtParty.Tag);
                    }
                    if (txtBroker.Text.Trim().Length == 0)
                    {
                        Property.Broker_ID = null;
                    }
                    else
                    {
                        Property.Broker_ID = Val.ToString(txtBroker.Tag);
                    }
                    Property.Through = txtThrough.Text;

                    DTabDetail.TableName = "Table";
                    string StrXMLValuesInsert = string.Empty;
                    using (StringWriter sw = new StringWriter())
                    {
                        DTabDetail.WriteXml(sw);
                        StrXMLValuesInsert = sw.ToString();
                    }

                    Property.DetailXML = StrXMLValuesInsert;

                    Property = ObjKapan.Save(Property);
                    txtJangedNo.Text = Property.ReturnJangedNo;
                    txtJangedNoStr.Text = Property.ReturnJangedStr;

                    this.Cursor = Cursors.Default;

                    Global.Message(Property.ReturnMessageDesc);

                    if (Property.ReturnMessageType == "SUCCESS")
                    {
                        BtnSearch_Click(null, null);
                    }
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }

        }

    }
}
