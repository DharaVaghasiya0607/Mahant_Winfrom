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
using BusLib.Account;

namespace MahantExport.Stock
{
    public partial class FrmInvoiceEntryExport : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_Invoice ObjInvoice = new BOTRN_Invoice();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();

        DataTable DTabDetail = new DataTable();
        DataTable DtabProcess = new DataTable();
        DataTable DtExportAccountingEffect = new DataTable(); //
        DataTable DtAccountingEffect = new DataTable();
        BOACC_FinanceJournalEntry ObjFinance = new BOACC_FinanceJournalEntry();
        DataTable DtLedger;
        int IntAccCnt = 0;
        public FrmInvoiceEntryExport()
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
            ObjFormEvent.ObjToDisposeList.Add(ObjInvoice);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }

        public FORMTYPE mFormType = FORMTYPE.SALEINVOICE;
        public enum FORMTYPE
        {
            SALEINVOICE = 1,
            MEMOINVOICE = 5
        }

        public void ShowForm(FORMTYPE pFormType)
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            DtpFromDate.Value = DateTime.Now.AddMonths(-1);
            DtpToDate.Value = DateTime.Now;

            DTabDetail = ObjInvoice.FillDetail(Guid.NewGuid().ToString());
            MainGrid.DataSource = DTabDetail;
            MainGrid.Refresh();
            xtraTabControl1.SelectedTabPageIndex = 0;

            mFormType = pFormType;
            FillControlName();

            BtnSearch_Click(null, null);
            this.Show();
            lblMode.Text = "Add Mode";
            DTPInvoiceDate.Focus();

        }

        public void ShowForm(FORMTYPE pFormType, DataTable pDtInvoice, string pStockType = "ALL")  //Call When BranchReceive Pickup Bank Option
        {
            try
            {
                Val.FormGeneralSetting(this);
                AttachFormDefaultEvent();

                DtpFromDate.Value = DateTime.Now.AddMonths(-1);
                DtpToDate.Value = DateTime.Now;

                DTabDetail.Columns.Add("SRNO", typeof(int));
                DTabDetail.Columns.Add("COMPANY_ID", typeof(Guid));
                DTabDetail.Columns.Add("SHAPE_ID", typeof(int));
                DTabDetail.Columns.Add("SHAPENAME", typeof(string));
                DTabDetail.Columns.Add("DESCRIPTION", typeof(string));
                DTabDetail.Columns.Add("CERTINO", typeof(string));
                DTabDetail.Columns.Add("CARAT", typeof(decimal));
                DTabDetail.Columns.Add("RATEHKD", typeof(decimal));
                DTabDetail.Columns.Add("RATEUSD", typeof(decimal));
                DTabDetail.Columns.Add("AMOUNTHKD", typeof(decimal));
                DTabDetail.Columns.Add("AMOUNTUSD", typeof(decimal));
                DTabDetail.Columns.Add("REMARK", typeof(string));
                DTabDetail.Columns.Add("MEMODETAIL_ID", typeof(Guid));
                DTabDetail.Columns.Add("SOMEMODETAIL_ID", typeof(Guid));

                //DTabDetail = pDtInvoice;

                foreach (DataRow DRow in pDtInvoice.Rows)
                {
                    if (DRow != null)
                    {
                        DataRow DRNew = DTabDetail.NewRow();

                        DRNew["SRNO"] = DRow["ENTRYSRNO"];
                        DRNew["COMPANY_ID"] = BOConfiguration.COMPANY_ID;
                        DRNew["SHAPE_ID"] = DRow["SHAPE_ID"];
                        DRNew["SHAPENAME"] = DRow["SHAPEFULLNAME"];
                        //DRNew["DESCRIPTION"] = DRow["PARTYSTOCKNO"];
                        DRNew["DESCRIPTION"] = DRow["COLORNAME"].ToString() + " - " + DRow["CLARITYNAME"].ToString();
                        DRNew["CERTINO"] = DRow["LABREPORTNO"];
                        DRNew["CARAT"] = DRow["CARAT"];
                        DRNew["RATEHKD"] = DRow["FMEMOPRICEPERCARAT"];
                        DRNew["RATEUSD"] = DRow["MEMOPRICEPERCARAT"];
                        DRNew["AMOUNTHKD"] = DRow["FMEMOAMOUNT"];
                        DRNew["AMOUNTUSD"] = DRow["MEMOAMOUNT"];
                        DRNew["REMARK"] = DRow["REMARK"];
                        DRNew["MEMODETAIL_ID"] = DRow["MEMODETAIL_ID"];
                        DRNew["SOMEMODETAIL_ID"] = DRow["PREVMEMODETAIL_ID"];

                        DTabDetail.Rows.Add(DRNew);
                        DTabDetail.AcceptChanges();
                    }
                }

                MainGrid.DataSource = DTabDetail;
                GrdDet.RefreshData();

                xtraTabControl1.SelectedTabPageIndex = 0;
                mFormType = pFormType;
                FillControlName();
                this.Show();
                lblMode.Text = "Add Mode";
                DTPInvoiceDate.Focus();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        public void FillControlName()
        {
            DtabProcess = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PROCESS);

            if (mFormType == FORMTYPE.SALEINVOICE)
            {
                GetSelectedProcessIssue("SALES DELIVERY");
                pnlBank.Visible = true;
            }
            else if (mFormType == FORMTYPE.MEMOINVOICE)
            {
                GetSelectedProcessIssue("MEMO ISSUE");
                pnlBank.Visible = false;
            }

            txtSalePerson.Tag = BOConfiguration.gEmployeeProperty.LEDGER_ID;
            txtSalePerson.Text = BOConfiguration.gEmployeeProperty.LEDGERNAME;

            RbUSD_CheckedChanged(null, null);
        }

        public void GetSelectedProcessIssue(string StrProcessName)
        {
            DataRow[] UDRow = DtabProcess.Select("PROCESSNAME = '" + StrProcessName + "'");

            if (UDRow.Length != 0)
            {
                this.Text = Val.ToString(UDRow[0]["PROCESSNAME"]);
                txtProcess.Text = Val.ToString(UDRow[0]["PROCESSNAME"]);
                txtProcess.Tag = Val.ToInt(UDRow[0]["PROCESS_ID"]);
                BtnSave.Enabled = true;
            }
            else
            {
                BtnSave.Enabled = false;
                Global.Message("NOT VALID ISSUE PROCESS FOUND");
                this.Text = string.Empty;
                txtProcess.Text = string.Empty;
                txtProcess.Tag = 0;
            }
        }

        #region Button Events

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtParty.Text.Trim() == "")
                {
                    Global.Message("Party Is Requried");
                    txtParty.Focus();
                    return;
                }
                if (DTabDetail.Rows.Count == 0)
                {
                    Global.Message("Please Enter Proper Detail.");
                    return;
                }
                // Comments by darshan bcz client not alert msg show
                //if (Global.Confirm("Are You Sure To Save This Entry ?") == System.Windows.Forms.DialogResult.No)
                //{
                //    return;
                //}

                this.Cursor = Cursors.WaitCursor;

                int IntRow = 0;
                foreach (DataRow DRow in DTabDetail.Rows)
                {
                    IntRow++;
                    DRow["SRNO"] = IntRow;
                }
                DTabDetail.AcceptChanges();

                TRN_InvoiceProperty Property = new TRN_InvoiceProperty();
                if (Val.ToString(txtInvoiceNo.Tag) == "")
                {
                    txtInvoiceNo.Tag = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                }

                Property.Invoice_ID = Val.ToString(txtInvoiceNo.Tag);
                Property.InvoiceDate = Val.SqlDate(DTPInvoiceDate.Value.ToShortDateString());
                Property.FinYear = Global.GetFinancialYear(DTPInvoiceDate.Value.ToShortDateString());
                Property.InvoiceNo = Val.ToInt(txtInvoiceNo.Text);
                if (Val.ToString(txtParty.Tag) == "")
                {
                    txtParty.Tag = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                }

                Property.Party_ID = Val.ToString(txtParty.Tag);
                Property.PartyName = Val.ToString(txtParty.Text);
                Property.BrNo = txtBRNo.Text;

                if (txtSalePerson.Text.Trim().Length == 0)
                {
                    Property.Seller_ID = null;
                }
                else
                {
                    Property.Seller_ID = Val.ToString(txtSalePerson.Tag);
                }

                Property.AuthorisePersonName = Val.ToString(txtAuthorisePerson.Text);

                if (txtTerms.Text.Trim().Length == 0)
                {
                    Property.Terms_ID = null;
                }
                else
                {
                    Property.Terms_ID = Val.ToString(txtTerms.Tag);
                }

                Property.PhNo = Val.ToString(txtPhNo.Text);
                Property.Process_ID = Val.ToInt(txtProcess.Tag);
                Property.Remark = txtRemark.Text;
                Property.BankType = (rbDBS.Checked) ? rbDBS.Tag.ToString() : rbHSBC.Tag.ToString();
                Property.CurrencyType = (RbUSD.Checked) ? RbUSD.Tag.ToString() : RbHKD.Tag.ToString();
                Property.MemoType = Val.ToString(mFormType);

                DTabDetail.TableName = "Table";
                string StrXMLValuesInsert = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabDetail.WriteXml(sw);
                    StrXMLValuesInsert = sw.ToString();
                }

                Property.DetailXML = StrXMLValuesInsert;
                Property.Pickup = (ChkOrderConfirmPickup.Checked) ? "YES" : "NO";
                //Property.PaymentDone = (chkPaymentDone.Checked) ? true : false;
                Property.INVOICETYPE = "EXPORT";

                Property = ObjInvoice.Save(Property);
                txtInvoiceNo.Text = Property.ReturnInvoiceNo;
                txtInvoiceNoStr.Text = Property.ReturnInvoiceStr;

                //if (lblMode.Text == "Add Mode")
                //{
                //    BtnPrint_Click(null, null);
                //}

                this.Cursor = Cursors.Default;

                Global.Message(Property.ReturnMessageDesc);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    //Add shiv 14-11-22
                    SqlConnection cn_T;
                    cn_T = new SqlConnection(BOConfiguration.ConnectionString);
                    if (cn_T.State == ConnectionState.Open) { cn_T.Close(); }
                    cn_T.Open();
                    SaveAccountDetail(cn_T, false);
                    cn_T.Close();
                    BtnPrint_Click(null, null);
                    BtnClear_Click(null, null);
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
            txtInvoiceNo.Text = string.Empty;
            txtInvoiceNoStr.Text = string.Empty;
            txtParty.Text = string.Empty;
            txtParty.Tag = string.Empty;
            //txtSalePerson.Text = string.Empty;
            // txtSalePerson.Tag = string.Empty;
            txtAuthorisePerson.Text = string.Empty;
            txtAuthorisePerson.Tag = string.Empty;
            txtTerms.Text = string.Empty;
            txtTerms.Tag = string.Empty;
            txtProcess.Text = string.Empty;
            RbUSD.Checked = true;
            rbDBS.Checked = true;
            txtPhNo.Text = string.Empty;
            DTabDetail.Rows.Clear();
            txtInvoiceNo.Tag = string.Empty;
            txtRemark.Text = string.Empty;
            txtBRNo.Text = string.Empty;
            txtTermsDays.Text = string.Empty;
            BtnSearch_Click(null, null);
            DTPInvoiceDate.Focus();
            txtAuthorisePerson.Enabled = true;
            txtBRNo.Enabled = true;
            txtPhNo.Enabled = true;
            lblMode.Text = "Add Mode";
            ChkOrderConfirmPickup.Checked = false;
            DTPInvoiceDate.Value = DateTime.Now;
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("InvoiceExport" + txtInvoiceNo.Text, GrdDet);
        }

        private void BtnExportSummary_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("InvoiceSummary", GrdSummury);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            TRN_InvoiceProperty Property = new TRN_InvoiceProperty();
            try
            {

                if (Global.Confirm("Are Your Sure To Delete The Record ?") == System.Windows.Forms.DialogResult.No)
                    return;

                FrmPassword FrmPassword = new FrmPassword();


                //if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;
                    Property.Invoice_ID = Val.ToString(txtInvoiceNo.Tag);
                    Property = ObjInvoice.Delete(Property);
                    this.Cursor = Cursors.Default;
                    Global.Message(Property.ReturnMessageDesc);

                    if (Property.ReturnMessageType == "SUCCESS")
                    {
                        BtnClear_Click(null, null);
                    }
                    else
                    {
                        DTPInvoiceDate.Focus();
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

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DTabSummury = ObjInvoice.FillSummaryExport(
                                Val.SqlDate(DtpFromDate.Value.ToShortDateString()),
                                Val.SqlDate(DtpToDate.Value.ToShortDateString()),
                                Val.ToInt(txtProcess.Tag.ToString()));
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
                if (e.Column.FieldName == "RATEHKD" || e.Column.FieldName == "CARAT")
                {
                    DataRow DRow = GrdDet.GetDataRow(e.RowHandle);
                    DTabDetail.Rows[e.RowHandle]["AMOUNTHKD"] = Math.Round(Val.Val(DRow["CARAT"]) * Val.Val(DRow["RATEHKD"]), 2);
                    DTabDetail.AcceptChanges();
                }
                if (e.Column.FieldName == "AMOUNTHKD" || e.Column.FieldName == "CARAT")
                {
                    DataRow DRow = GrdDet.GetDataRow(e.RowHandle);
                    DTabDetail.Rows[e.RowHandle]["RATEHKD"] = Math.Round(Val.Val(DRow["AMOUNTHKD"]) / Val.Val(DRow["CARAT"]), 2);
                    DTabDetail.AcceptChanges();
                }

                if (e.Column.FieldName == "RATEUSD" || e.Column.FieldName == "CARAT")
                {
                    DataRow DRow = GrdDet.GetDataRow(e.RowHandle);
                    DTabDetail.Rows[e.RowHandle]["AMOUNTUSD"] = Math.Round(Val.Val(DRow["CARAT"]) * Val.Val(DRow["RATEUSD"]), 2);
                    DTabDetail.AcceptChanges();
                }
                if (e.Column.FieldName == "AMOUNTUSD" || e.Column.FieldName == "CARAT")
                {
                    DataRow DRow = GrdDet.GetDataRow(e.RowHandle);
                    DTabDetail.Rows[e.RowHandle]["RATEUSD"] = Math.Round(Val.Val(DRow["AMOUNTUSD"]) / Val.Val(DRow["CARAT"]), 2);
                    DTabDetail.AcceptChanges();
                }

                CalculationNewInvoiceEntry(); //Add shiv 14-11-22

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

                string StrInvoiceID = Val.ToString(GrdSummury.GetRowCellValue(e.RowHandle, "INVOICE_ID"));

                txtInvoiceNo.Text = Val.ToString(DRow["INVOICENO"]);
                txtInvoiceNoStr.Text = Val.ToString(DRow["INVOICENOSTR"]);
                txtInvoiceNo.Tag = Val.ToString(DRow["INVOICE_ID"]);
                DTPInvoiceDate.Text = Val.ToString(DRow["INVOICEDATE"]);

                txtParty.Tag = Val.ToString(DRow["PARTY_ID"]);
                txtParty.Text = Val.ToString(DRow["PARTYNAME"]);

                txtSalePerson.Tag = Val.ToString(DRow["SELLER_ID"]);
                txtSalePerson.Text = Val.ToString(DRow["SELLERNAME"]);

                txtBRNo.Text = Val.ToString(DRow["BRNO"]);  //hinal 03-02-2022

                txtAuthorisePerson.Tag = Val.ToString(DRow["AUTHORISEPERSON_ID"]);
                txtAuthorisePerson.Text = Val.ToString(DRow["AUTHORISEPERSONNAME"]);

                txtTerms.Tag = Val.ToString(DRow["TERMS_ID"]);
                txtTerms.Text = Val.ToString(DRow["TERMSNAME"]);
                txtPhNo.Text = Val.ToString(DRow["PHNO"]);
                txtRemark.Text = Val.ToString(DRow["REMARK"]);
                if (Val.ToString(DRow["ISPickup"]) == "YES")
                    ChkOrderConfirmPickup.Checked = true;
                else
                    ChkOrderConfirmPickup.Checked = false;

                if (Val.ToString(DRow["CURRENCYTYPE"]) == Val.ToString(RbUSD.Tag.ToString()))
                {
                    RbUSD.Checked = true;
                }
                else
                {
                    RbHKD.Checked = true;
                }

                if (Val.ToString(DRow["BANKTYPE"]) == Val.ToString(rbDBS.Tag.ToString()))
                {
                    rbDBS.Checked = true;
                }
                else
                {
                    rbHSBC.Checked = true;
                }

                DTabDetail = ObjInvoice.FillDetail(Val.ToString(txtInvoiceNo.Tag));
                MainGrid.DataSource = DTabDetail;
                MainGrid.Refresh();
                xtraTabControl1.SelectedTabPageIndex = 0;
                txtSalePerson_Validated(null, null);

                txtAuthorisePerson.Enabled = false;
                txtBRNo.Enabled = false;
                txtPhNo.Enabled = false;
                lblMode.Text = "Edit Mode";

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
                        BtnSave_Click(null, null);
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
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;

                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);
                    FrmSearch.mBoolISPostBack = true;
                    FrmSearch.mStrISPostBackColumn = "PARTYNAME";
                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtParty.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtParty.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
                        txtAuthorisePerson.Text = Val.ToString(FrmSearch.DRow["CONTACTPERSON"]);
                        txtPhNo.Text = Val.ToString(FrmSearch.DRow["LANDLINENO"]);
                        txtBRNo.Text = Val.ToString(FrmSearch.DRow["BUSINESSREGISTRATIONNO"]);

                        if (Val.ToString(txtParty.Tag) == "")
                        {
                            txtAuthorisePerson.Enabled = true;
                            txtBRNo.Enabled = true;
                            txtPhNo.Enabled = true;
                        }
                        else
                        {
                            txtAuthorisePerson.Enabled = false;
                            txtBRNo.Enabled = false;
                            txtPhNo.Enabled = false;

                            if (DTabDetail.Rows.Count <= 0)
                                txtBRNo_Validated(null, null);

                        }

                        if (txtParty.Text != string.Empty)
                        {
                            if (GrdDet.RowCount <= 0)
                            {
                                txtRemark_Validated(null, null);
                            }
                        }
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
            txtDescription.Text = string.Empty;//Gunjan:19/08/2023
            txtHSNCode.Text = string.Empty;//Gunjan:19/08/2023
            GrpExport.Visible = true;//Gunjan:10/08/2023
            //string StrReportHeader = Val.ToString(DTabPrint.Rows[0]["ReportHeader"]);
            //foreach (DataRow DRow in DTabPrint.Rows)
            //{
            //    DRow["ReportHeader"] = StrReportHeader + " [Original]";
            //}

            //FrmReportViewer.MdiParent = Global.gMainRef;

            //if (mFormType == FORMTYPE.MEMOINVOICE)
            //{
            //    FrmReportViewer.ShowMemoInvoiceHKPrint("MemoInvoiceHK", DTabPrint, txtParty.Text, txtInvoiceNo.Text);

            //    foreach (DataRow DRow in DTabPrint.Rows)
            //    {
            //        DRow["ReportHeader"] = StrReportHeader + " [Duplicate]";
            //    }
            //    FrmReportViewer = new Report.FrmReportViewer();
            //    FrmReportViewer.MdiParent = Global.gMainRef;
            //    FrmReportViewer.ShowMemoInvoiceHKPrint("MemoInvoiceHK", DTabPrint, txtParty.Text, txtInvoiceNo.Text);
            //}
            //else if (mFormType == FORMTYPE.SALEINVOICE)
            //{
            //    FrmReportViewer.ShowMemoInvoiceHKPrint("SaleInvoiceHK", DTabPrint, txtParty.Text, txtInvoiceNo.Text);

            //    foreach (DataRow DRow in DTabPrint.Rows)
            //    {
            //        DRow["ReportHeader"] = StrReportHeader + " [Audit Copy]";
            //    }
            //FrmReportViewer = new Report.FrmReportViewer();
            //FrmReportViewer.MdiParent = Global.gMainRef;
            //FrmReportViewer.ShowMemoInvoiceHKPrint("AccSalesExportHKSummaryPrint", DTabPrint, txtParty.Text, txtInvoiceNo.Text);
            //}

            this.Cursor = Cursors.Default;
        }

        private void txtTerms_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "TERMSNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_TERMS);
                    FrmSearch.mStrColumnsToHide = "TERMS_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtTerms.Text = Val.ToString(FrmSearch.DRow["TERMSNAME"]);
                        txtTerms.Tag = Val.ToString(FrmSearch.DRow["TERMS_ID"]);
                        txtTermsDays.Text = Val.ToString(FrmSearch.DRow["TERMSDAYS"]);
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

        private void txtSalePerson_Validated(object sender, EventArgs e)
        {
            DataRow Dr = DTabDetail.NewRow();
            Dr["SRNO"] = DTabDetail.Rows.Count + 1;
            DTabDetail.Rows.Add(Dr);

            GrdDet.Focus();
            GrdDet.PostEditor();
            GrdDet.FocusedRowHandle = GrdDet.RowCount - 1;
            GrdDet.FocusedColumn = GrdDet.Columns["SRNO"];
        }

        private void txtSalePerson_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);
                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtSalePerson.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtSalePerson.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
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

        private void RbUSD_CheckedChanged(object sender, EventArgs e)
        {
            GrdDet.Columns["RATEHKD"].Visible = false;
            GrdDet.Columns["AMOUNTHKD"].Visible = false;
            GrdDet.Columns["RATEUSD"].Visible = true;
            GrdDet.Columns["AMOUNTUSD"].Visible = true;
            GrdDet.Columns["RATEUSD"].VisibleIndex = 5;
            GrdDet.Columns["AMOUNTUSD"].VisibleIndex = 6;
        }

        private void RbHKD_CheckedChanged(object sender, EventArgs e)
        {
            GrdDet.Columns["RATEHKD"].Visible = true;
            GrdDet.Columns["AMOUNTHKD"].Visible = true;
            GrdDet.Columns["RATEUSD"].Visible = false;
            GrdDet.Columns["AMOUNTUSD"].Visible = false;
            GrdDet.Columns["RATEHKD"].VisibleIndex = 5;
            GrdDet.Columns["AMOUNTHKD"].VisibleIndex = 6;
        }

        private void PanelTop_Paint(object sender, PaintEventArgs e)
        {

        }
        private void txtBRNo_Validated(object sender, EventArgs e)
        {
            //DataRow Dr = DTabDetail.NewRow();
            //Dr["SRNO"] = DTabDetail.Rows.Count + 1;
            //DTabDetail.Rows.Add(Dr);

            //GrdDet.Focus();
            //GrdDet.PostEditor();
            //GrdDet.FocusedRowHandle = GrdDet.RowCount - 1;
            //GrdDet.FocusedColumn = GrdDet.Columns["SRNO"];
        }

        private void txtRemark_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GrdDet.Focus();
                GrdDet.PostEditor();
                GrdDet.FocusedRowHandle = GrdDet.RowCount - 1;
                GrdDet.FocusedColumn = GrdDet.Columns["SRNO"];
            }
        }

        private void ReptxtShapeName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataRow Dro = GrdDet.GetFocusedDataRow();
                if (Val.ToString(Dro["SHAPENAME"]) == "" && GrdDet.IsLastRow)
                {
                    e.Handled = true;
                    BtnSave.Focus();
                    BtnSave.PerformClick();
                }
            }
        }

        private void txtPhNo_Validated(object sender, EventArgs e)
        {
            //DataRow Dr = DTabDetail.NewRow();
            //Dr["SRNO"] = DTabDetail.Rows.Count + 1;
            //DTabDetail.Rows.Add(Dr);

            //GrdDet.Focus();
            //GrdDet.PostEditor();
            //GrdDet.FocusedRowHandle = GrdDet.RowCount - 1;
            //GrdDet.FocusedColumn = GrdDet.Columns["SRNO"];
        }

        private void txtRemark_Validated(object sender, EventArgs e)
        {
            DataRow Dr = DTabDetail.NewRow();
            Dr["SRNO"] = DTabDetail.Rows.Count + 1;
            DTabDetail.Rows.Add(Dr);

            GrdDet.Focus();
            GrdDet.PostEditor();
            GrdDet.FocusedRowHandle = GrdDet.RowCount - 1;
            GrdDet.FocusedColumn = GrdDet.Columns["SRNO"];
        }

        private void InvoicePaymentDoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdSummury.FocusedRowHandle >= 0)
                {
                    if (Global.Confirm("ARE YOU SURE INVOICE PAYMENT DONE") == System.Windows.Forms.DialogResult.Yes)
                    {
                        FrmPassword FrmPassword = new FrmPassword();
                        ObjPer.PASSWORD = "123";
                        if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                        {
                            DataRow Drow = GrdSummury.GetDataRow(GrdSummury.FocusedRowHandle);
                            TRN_InvoiceProperty Property = new TRN_InvoiceProperty();
                            Property.Invoice_ID = Val.ToString(GrdSummury.GetRowCellValue(GrdSummury.FocusedRowHandle, "INVOICE_ID"));
                            Property = ObjInvoice.UpdatePaymentDone(Property);

                            if (Property.ReturnMessageType == "SUCCESS")
                            {
                                Global.Message("ENTRY UPDATED SUCCESSFULLY");
                                BtnSearch_Click(sender, e);
                            }
                            else
                            {
                                Global.Message("ERROR IN UPDATE ENTRY");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        //Add shiv 14-11-22
        public void SaveAccountDetail(SqlConnection pConConnection, bool pExpValueAdd)
        {
            TRN_LedgerTranJournalProperty AccoutProperty = new TRN_LedgerTranJournalProperty();
            BOLedgerTransaction objLedgerTrn = new BOLedgerTransaction();

            string Year = Global.GetFinancialYear(DTPInvoiceDate.Value.ToShortDateString());
            int FinyearId = 0;
            DataTable DTYearId = objLedgerTrn.GetFromToDateYearID(Year);
            if (DTYearId.Rows.Count > 0)
            {
                FinyearId = Val.ToInt32(DTYearId.Rows[0]["Year_ID"]);
            }

            AccoutProperty.TRN_ID = Guid.NewGuid();
            AccoutProperty.ACCLEDGTRNTRN_ID = Val.ToString(txtInvoiceNo.Tag);
            AccoutProperty.ACCLEDGTRNSRNO = 1;

            AccoutProperty.VOUCHERDATE = Val.SqlDate(DTPInvoiceDate.Text);
            AccoutProperty.FINYEAR = Global.GetFinancialYear(DTPInvoiceDate.Value.ToShortDateString());
            AccoutProperty.FINYEAR_ID = FinyearId;
            AccoutProperty.VOUCHERNO = Val.ToInt32(txtInvoiceNo.Text);
            AccoutProperty.VOUCHERSTR = txtInvoiceNoStr.Text;
            AccoutProperty.CURRENCY_ID = Val.ToInt32((RbUSD.Checked) ? 1 : 0);
            //AccoutProperty.EXCRATE = Val.Val(txtExcRate.Text);
            AccoutProperty.NOTE = txtRemark.Text;
            AccoutProperty.REFBOOKTYPEFULL = "";

            AccoutProperty.BILL_NO = txtInvoiceNoStr.Text;
            AccoutProperty.BILL_DT = Val.SqlDate(DTPInvoiceDate.Text);
            //End Kuldeep 24122020
            AccoutProperty.EXCRATEDIFF = 0;
            //AccoutProperty.TERMSDATE = Val.SqlDate(DTPTermsDate.Text);
            AccoutProperty.CHQ_NO = "";
            //AccoutProperty.CHQISSUEDT = "01/01/1900";
            //AccoutProperty.CHQCLEARDT = "01/01/1900";
            AccoutProperty.DATAFREEZ = 0;
            AccoutProperty.PAYTYPE = "";
            AccoutProperty.REFTYPE = "";
            AccoutProperty.PAYTERMS = Val.ToInt32(txtTermsDays.Text);
            //AccoutProperty.ACCTYPE = Val.ToString(cmbAccType.Text);

            string AccountSaveForXML = string.Empty;
            // DtAccountingEffect.TableName = "ACCOUNT";
            if (pExpValueAdd)
            {
                using (StringWriter sw = new StringWriter())
                {
                    DtExportAccountingEffect.TableName = "ACCOUNT";
                    DtExportAccountingEffect.WriteXml(sw);
                    AccountSaveForXML = sw.ToString();
                }
            }
            else
            {
                using (StringWriter sw = new StringWriter())
                {
                    // DtAccountingEffect.Merge(DtExportAccountingEffect);
                    DtAccountingEffect.TableName = "ACCOUNT";
                    DtAccountingEffect.WriteXml(sw);
                    AccountSaveForXML = sw.ToString();
                }
            }

            if (mFormType == FORMTYPE.SALEINVOICE)
            {
                AccoutProperty.ENTRYTYPE = "INVOICE";
                AccoutProperty.BOOKTYPE = "INV";
                AccoutProperty.BOOKTYPEFULL = "SALE DELIVERY";
                AccoutProperty.TRNTYPE = "SALE DELIVERY";
            }

            AccoutProperty.BIll_TYPE = (RbUSD.Checked) ? RbUSD.Tag.ToString() : RbHKD.Tag.ToString();
            ObjFinance.SaveAccountingEffectHK(pConConnection, AccoutProperty, AccountSaveForXML);
            string ReturnMessageDesc = "";
            string ReturnMessageType = "";
        }

        public void AddColumnsAccountDt()
        {
            DtAccountingEffect.Columns.Add("VOUCHERNO");
            DtAccountingEffect.Columns.Add("VOUCHERNOSTR");
            DtAccountingEffect.Columns.Add("LEDGER_ID");
            DtAccountingEffect.Columns.Add("LEDGERNAME");
            DtAccountingEffect.Columns.Add("REFLEDGER_ID");
            DtAccountingEffect.Columns.Add("DEBAMT");
            DtAccountingEffect.Columns.Add("CRDAMT");
            DtAccountingEffect.Columns.Add("DEBAMTFE");
            DtAccountingEffect.Columns.Add("CRDAMTFE");
            DtAccountingEffect.Columns.Add("XCONTRA");
            DtAccountingEffect.Columns.Add("SRNO");
            DtAccountingEffect.Columns.Add("ENTRYTRANTYPE");
            DtAccountingEffect.Columns.Add("ISAUTOACCENTRY");
        }

        public void AddAccountingEffect()
        {
            // int IntCnt = 1;            
            if (DtAccountingEffect.Columns.Count == 0)
                AddColumnsAccountDt();

            DtAccountingEffect.Rows.Clear();
            if (mFormType == FORMTYPE.SALEINVOICE)
            {

                if (Val.Val(txtGrossAmt.Text) > 0 || Val.Val(txtGrossAmt.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "BASIC");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(txtParty.Tag);
                        Dr["LEDGERNAME"] = Val.ToString(txtParty.Text);
                        Dr["REFLEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["DEBAMT"] = txtGrossAmt.Text;
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMTFE"] = txtGrossAmt.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["XCONTRA"] = "Y";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        Dr["ENTRYTRANTYPE"] = "";
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales Basic Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }

                if (Val.Val(txtGrossAmt.Text) > 0 || Val.Val(txtGrossAmt.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "BASIC");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtParty.Tag);
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMT"] = txtGrossAmt.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["CRDAMTFE"] = txtGrossAmt.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        Dr["ENTRYTRANTYPE"] = "";
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales Basic Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }

            }
            gridControl1.DataSource = DtAccountingEffect;
            gridControl1.RefreshDataSource();
        }

        public void CalculationNewInvoiceEntry()
        {
            double DouAmtUSD = 0.00;
            double DounAmtHKD = 0.00;

            if (RbUSD.Checked)
            {
                for (int IntI = 0; IntI < GrdDet.RowCount; IntI++)
                {
                    DataRow DRow = GrdDet.GetDataRow(IntI);
                    DouAmtUSD = DouAmtUSD + Val.Val(DRow["AMOUNTUSD"]);
                }
                txtGrossAmt.Text = Val.ToString(DouAmtUSD);
            }
            else
            {
                for (int IntI = 0; IntI < GrdDet.RowCount; IntI++)
                {
                    DataRow DRow = GrdDet.GetDataRow(IntI);
                    DounAmtHKD = DounAmtHKD + Val.Val(DRow["AMOUNTHKD"]);
                }
                txtGrossAmt.Text = Val.ToString(DounAmtHKD);
            }
            if (txtGrossAmt.Text != "")
            {
                AddAccountingEffect();
            }
        }

        private void BtnUpdateClose_Click(object sender, EventArgs e)
        {
            GrpExport.Visible = false;
        }

        private void btnSaleInvoicePrint_Click(object sender, EventArgs e)
        {
            if (txtInvoiceNo.Text.Length == 0)
            {
                Global.Message("Invoice No Is Required");
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            //DataTable DTabPrint = ObjInvoice.PrintExport(Val.ToString(txtInvoiceNo.Tag));//Comment By Gunjan:09/08/2023

            DataTable DTabPrint = ObjInvoice.SaleDeliveryExportPrint(Val.ToString(txtInvoiceNo.Tag), Val.ToString(txtDescription.Text), Val.ToString(txtHSNCode.Text));
            if (DTabPrint.Rows.Count == 0)
            {
                this.Cursor = Cursors.Default;
                Global.Message("No Data Found");
                return;
            }
            Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
            FrmReportViewer = new Report.FrmReportViewer();
            FrmReportViewer.MdiParent = Global.gMainRef;
            FrmReportViewer.ShowMemoInvoiceHKPrint("RPT_SalesDeliveryExportHK", DTabPrint, txtParty.Text, txtInvoiceNo.Text);

            GrpExport.Visible = false;//Gunjan:19/08/2023
        }
    }

}
