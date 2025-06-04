using BusLib.Account;
using BusLib.TableName;
using BusLib.Transaction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Config = BusLib.Configuration.BOConfiguration;

namespace MahantExport.Account
{
    public partial class FrmPolishRoughInward : Form
    {
        String PasteData = "";
        IDataObject PasteclipData = Clipboard.GetDataObject();

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        DataTable DTabLookup = new DataTable();
        DataTable DTabMax = new DataTable();
        BOLedgerTransaction objLedgerTrn = new BOLedgerTransaction();
        TRN_LedgerTranJournalProperty mJournalTranProperty = new TRN_LedgerTranJournalProperty();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();

        BOACC_PolishRoughInward ObjMast = new BOACC_PolishRoughInward();

        string StrReport = "";
        string StrProcedure = "";
        string StrRPTName = "";
        bool Autosave = false;
        int pId = 0;
        public FrmPolishRoughInward()
        {
            InitializeComponent();
        }
        #region Validation

        private bool ValSave()
        {
            if (Val.ToString(txtItemName.Text) == "")
            {
                Global.MessageError("Please enter Item Name");
                txtVoucherDate.Focus();
                return false;
            }

            if (Val.ToString(txtAccountName.Text) == "")
            {
                Global.MessageError("Please enter Account Name");
                txtVoucherDate.Focus();
                return false;
            }
            return true;
        }

        //private bool Check_DataExist()
        //{
        //    if (Model.Journal_Transaction_ID != null && lblMode.Text == "Edit Mode")
        //    {

        //        DataTable dt = new DataTable();
        //        GlobalFillControls GlobalFillControls = new GlobalFillControls();
        //        dt = GlobalFillControls.Get_Approval_Transaction(Model.Journal_Transaction_ID, Model.Company_ID, Model.Book_Code, Model.Year_ID, Model.Menu_ID);


        //        if (dt.Rows.Count == 0 && Approval_System_Required == true)
        //        {
        //            XtraMessageBox.Show("Entry is Already Approved so. So You Can Not Update/Delete Record. ");
        //            return false;
        //        }

        //    }
        //    return true;
        //}

        //private bool Receipt_Payment_Details_AdjustCheck()
        //{
        //    var result = "";
        //    if (Model.Journal_Transaction_ID != null && Val.ToString(Model.Journal_Transaction_ID) != "")
        //    {
        //        result = new Receipt_Payment_DetailsBLL().Receipt_Payment_Details_AdjustCheck(Model.Journal_Transaction_ID, Model.Book_Code);
        //        if (result != "")
        //        {
        //            XtraMessageBox.Show(result, "Information", MessageBoxButtons.OK, MessageBoxIcon.Stop);
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        #endregion

        private void txtAccountName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;

                    DataTable DtabParty = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);
                    //FrmSearch.mDTab = DtabParty.Select("PARTYTYPE = 'SALE'").CopyToDataTable();
                    FrmSearch.mDTab = DtabParty;
                    FrmSearch.mStrColumnsToHide = "PARTY_ID,BILLINGCOUNTRY_ID,SHIPPINGCOUNTRY_ID,PARTYTYPE,COMPANYNAME";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtAccountName.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]).ToUpper();
                        txtAccountName.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValSave() == false)
                {
                    return;
                }
                if (Autosave != true)
                {
                    if (Global.Confirm("Are You Sure For Entry") == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                }

                Acc_PolishRoughInwardProperty Property = new Acc_PolishRoughInwardProperty();

                Property.ID = pId.Equals(0) ? 0 : pId;
                Property.VoucherDate = Convert.ToDateTime(txtVoucherDate.Text);
                Property.PartyName = Val.ToString(txtAccountName.Text);
                Property.PartyId = Val.ToString(txtAccountName.Tag).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtAccountName.Tag));
                Property.VoucherNo = Val.ToInt32(txtVoucherNo.Text);

                Property.ItemName = Val.ToString(txtItemName.Text);
                Property.RoughCarat = Val.ToDouble(txtRoughCts.Text);
                Property.RoughRate = Val.ToDouble(txtRoughRate.Text);
                Property.RoughAmtRs = Val.ToDouble(txtRoughAmt.Text);
                Property.RegRate = Val.ToDouble(txtRegRate.Text);

                Property.RegAmtRs = Val.ToDouble(txtRegAmt.Text);
                Property.RegCarat = Val.ToDouble(txtRegCts.Text);
                Property.MfgAmtRs = Val.ToDouble(txtmfgAmt.Text);
                Property.MfgCarat = Val.ToDouble(txtmfgCts.Text);

                Property.PolishPCarat = Val.ToDouble(txtPolishCts.Text);
                Property.LabourAmt = Val.ToDouble(txtLabourAmt.Text);

                Property = ObjMast.Save(Property);

                this.Cursor = Cursors.Default;

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    Global.Message(Property.ReturnMessageDesc);
                    ClearAll();
                }
                else if (Property.ReturnMessageType == "FAIL")
                {
                    Global.MessageError(Property.ReturnMessageDesc);
                    txtVoucherDate.Focus();
                }

                Property = null;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.MessageError(ex.Message);
            }

        }

        private void FrmOutstandingResgisterReport_Load(object sender, EventArgs e)
        {
            this.ActiveControl = txtVoucherDate;
            ClearAll();
        }

        private void btnclear_Click(object sender, EventArgs e)
        {
            try
            {
                ClearAll();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void ClearAll()
        {
            txtVoucherDate.EditValue = DateTime.Now;
            txtAccountName.Text = string.Empty;
            txtAccountName.Tag = "";
            txtAccountName.Text = "";
            txtItemName.Text = "";
            txtItemName.Tag = "";
            txtRoughCts.Text = "0.00";
            txtRoughRate.Text = "0.00";
            txtRoughAmt.Text = "0.00";
            txtRegCts.Text = "0.00";
            txtRegRate.Text = "0.00";
            txtRegAmt.Text = "0.00";
            txtmfgCts.Text = "0.00";
            txtmfgAmt.Text = "0.00";
            txtPolishCts.Text = "0.00";
            txtLabourAmt.Text = "0.00";
            //txtVoucherNo.Text = ObjMast.FindVoucherNo(Config.FINYEARNAME, "").ToString();
            txtVoucherNo.Text = MaxVoucher(Val.ToString(txtVoucherDate.Text)).ToString();
        }

        private int MaxVoucher(string pIntVoucherDate)
        {
            DTabMax = ObjMast.MaxVoucherNoDateWise(pIntVoucherDate);
            int MaxVoucherNo = 0;
            if (DTabMax.Rows.Count > 0)
            {
                MaxVoucherNo = Val.ToInt(DTabMax.Rows[0]["MAXNO"]);
            }
            return MaxVoucherNo;
        }

        private void txtItemName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                try
                {
                    if (Global.OnKeyPressEveToPopup(e))
                    {
                        FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                        FrmSearch.mStrSearchField = "ITEMNAME";
                        FrmSearch.mStrSearchText = e.KeyChar.ToString();
                        this.Cursor = Cursors.WaitCursor;
                        FrmSearch.mDTab = ObjMemo.GetDistinctItemName();
                        FrmSearch.mBoolISPostBack = true;
                        FrmSearch.mStrISPostBackColumn = "ITEMNAME";
                        FrmSearch.mStrColumnsToHide = "";

                        this.Cursor = Cursors.Default;
                        FrmSearch.ShowDialog();
                        e.Handled = true;
                        if (FrmSearch.DRow != null)
                        {
                            //GrdSummuryMNL.SetFocusedRowCellValue("ITEMNAME", Val.ToString(FrmSearch.DRow["ITEMNAME"]));
                            txtItemName.Text = Val.ToString(FrmSearch.DRow["ITEMNAME"]).ToUpper();
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
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtVoucherNo_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)      
        {
            try
            {
                if (e.KeyCode == Keys.PageUp)
                {
                    //int VoucherNo = Val.ToInt(ObjMast.FindVoucherNo(Config.FINYEARNAME, ""));
                    int VoucherNo = MaxVoucher(Val.ToString(txtVoucherDate.Text));
                    if (VoucherNo != Val.ToInt(txtVoucherNo.Text))
                    {
                        txtVoucherNo.Text = Val.ToString(Val.ToInt(txtVoucherNo.Text) + 1);
                    }
                    else
                    {
                        txtVoucherNo.Text = Convert.ToString(VoucherNo);
                    }
                    txtVoucherNo_Validated(null, null);
                }
                else if (e.KeyCode == Keys.PageDown)
                {
                    //int VoucherNo = Val.ToInt(ObjMast.FindVoucherNo(Config.FINYEARNAME, ""));
                    int VoucherNo = MaxVoucher(Val.ToString(txtVoucherDate.Text));
                    if (VoucherNo != Val.ToInt(txtVoucherNo.Text))
                    {
                        if (Val.ToInt(txtVoucherNo.Text) > 1)
                            txtVoucherNo.Text = Val.ToString(Val.ToInt(txtVoucherNo.Text) - 1);
                    }
                    else
                    {
                        if (VoucherNo > 1)
                        {
                            txtVoucherNo.Text = Convert.ToString(VoucherNo - 1);
                        }
                        else
                        {
                            txtVoucherNo.Text = Convert.ToString(VoucherNo);
                        }
                    }
                    txtVoucherNo_Validated(null, null);
                }
                txtVoucherNo.Focus();
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
            }
        }

        private void txtVoucherNo_Validated(object sender, EventArgs e)
        {
            try
            {
                GetVoucherNoDetail();
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
            }
        }
        public void GetVoucherNoDetail()
        {
            try
            {
                DTabLookup = ObjMast.Fill(Val.ToInt(txtVoucherNo.Text), Val.ToString(txtVoucherDate.Text));
                if (DTabLookup.Rows.Count > 0)
                {
                    pId = Val.ToInt(DTabLookup.Rows[0]["ID"]);
                    txtVoucherDate.EditValue = Convert.ToDateTime(DTabLookup.Rows[0]["VOUCHERDATE"]);
                    txtAccountName.Text = Val.ToString(DTabLookup.Rows[0]["PARTYNAME"]);
                    txtAccountName.Tag = Val.ToGuid(DTabLookup.Rows[0]["PARTYID"]);
                    txtVoucherNo.Text = Val.ToString(DTabLookup.Rows[0]["VOUCHERNO"]);

                    txtItemName.Text = Val.ToString(DTabLookup.Rows[0]["ITEMNAME"]);
                    txtRoughCts.Text = Val.ToString(DTabLookup.Rows[0]["ROUGHCARAT"]);
                    txtRoughAmt.Text = Val.ToString(DTabLookup.Rows[0]["ROUGHAMTRS"]);
                    txtRoughRate.Text = Val.ToString(DTabLookup.Rows[0]["ROUGHRATE"]);
                    txtRegCts.Text = Val.ToString(DTabLookup.Rows[0]["REGCARAT"]);
                    txtRegRate.Text = Val.ToString(DTabLookup.Rows[0]["REGRATE"]);
                    txtRegAmt.Text = Val.ToString(DTabLookup.Rows[0]["REGAMTRS"]);
                    txtmfgCts.Text = Val.ToString(DTabLookup.Rows[0]["MFGCARAT"]);
                    txtmfgAmt.Text = Val.ToString(DTabLookup.Rows[0]["MFGAMTRS"]);
                    txtPolishCts.Text = Val.ToString(DTabLookup.Rows[0]["POLISHPCARAT"]);
                    txtLabourAmt.Text = Val.ToString(DTabLookup.Rows[0]["LABOURAMT"]);
                    this.ActiveControl = btnSave;
                }
                else
                {
                    txtVoucherDate.EditValue = DateTime.Now;
                    txtAccountName.Text = string.Empty;
                    txtAccountName.Tag = string.Empty;
                    //txtVoucherNo.Text = string.Empty;
                    txtAccountName.Text = string.Empty;
                    txtItemName.Text = string.Empty;
                    txtItemName.Tag = string.Empty;
                    txtRoughCts.Text = string.Empty;
                    txtRoughRate.Text = string.Empty;
                    txtRoughAmt.Text = string.Empty;
                    txtRegRate.Text = string.Empty;
                    txtRegAmt.Text = string.Empty;
                    txtmfgCts.Text = string.Empty;
                    txtmfgAmt.Text = string.Empty;
                    txtPolishCts.Text = string.Empty;
                    txtLabourAmt.Text = string.Empty;
                    pId = 0;
                }
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (Global.Confirm("Are You Sure You Want To Delete This Entry ?") == System.Windows.Forms.DialogResult.No)
            {
                return;
            }
            this.Cursor = Cursors.WaitCursor;

            Acc_PolishRoughInwardProperty Property = new Acc_PolishRoughInwardProperty();
            Property.ID = Val.ToInt32(DTabLookup.Rows[0]["ID"]);
            Property = ObjMast.RoughDelete(Property);
            this.Cursor = Cursors.Default;
            if (Property.ReturnMessageType == "SUCCESS")
            {
                Global.Message(Property.ReturnMessageDesc);
                ClearAll();
            }
            else if (Property.ReturnMessageType == "FAIL")
            {
                Global.MessageError(Property.ReturnMessageDesc);
            }
           
        }

        private void txtVoucherDate_Validated(object sender, EventArgs e)
        {
            txtVoucherNo.Text = MaxVoucher(Val.ToString(txtVoucherDate.Text)).ToString();
        }
    }
}
