using BusLib;
using BusLib.Account;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using Google.API.Translate;
using MahantExport.Utility;
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
using Config = BusLib.Configuration.BOConfiguration;

namespace MahantExport.Account
{
    public partial class FrmBillWiseEntryNew : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_Ledger ObjMast = new BOMST_Ledger();
        BOFormPer ObjPer = new BOFormPer();
        BOLedgerTransaction objLedgerTrn = new BOLedgerTransaction();
        BOACC_FinanceJournalEntry ObjFinance = new BOACC_FinanceJournalEntry();

        DataTable DtabPaymentSummry = new DataTable();
        DataTable DtabPaymentDetail = new DataTable();
        DataTable DtabPaymentPickupBillDetail = new DataTable();
        public FORMTYPE mFormType = FORMTYPE.CP;
        //Kuldeep 19012021
        int IntAllowToCalculate = 0;

        DataTable DTBal = new DataTable(); //Added by Daksha on 25/04/2023
        DataTable DTCASH = new DataTable();
        DataTable DTBANK = new DataTable();

        public enum FORMTYPE
        {
            CP = 0,
            BP = 1,
            CR = 2,
            BR = 3,
            CO = 4
        }

        int IntSumryCount = 1;

        #region Property Settings

        public FrmBillWiseEntryNew()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            this.Show();

            DTPVoucherDate.Value = DateTime.Now;
            //DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            //DTPToDate.Value = DateTime.Now;

            DataSet DSet = objLedgerTrn.GetBillWisePaymentGetDataNew("DETAIL", "", "", "", Guid.Empty, Guid.Empty);
            DSet.DataSetName = "DocumentElement";
            if (DSet.Tables.Count == 3)
            {
                DtabPaymentPickupBillDetail = DSet.Tables[0];
                DtabPaymentSummry = DSet.Tables[1];
                DtabPaymentDetail = DSet.Tables[2];
            }

            BtnClear_Click(null, null);
        }
        
        public void ShowForm(string pStrBookTypeFull, string pStrTran_id)
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            this.Show();

            DTPVoucherDate.Value = DateTime.Now;
            //DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            //DTPToDate.Value = DateTime.Now;

            BtnClear_Click(null, null);


            DataSet DSet = new DataSet();
            DSet = objLedgerTrn.GetBillWisePaymentGetDataNew("DETAIL", "", "", pStrBookTypeFull, Guid.Parse(pStrTran_id), Guid.Empty);
            if (DSet.Tables.Count <= 0)
            {
                this.Cursor = Cursors.Default;
                Global.Message("Some Thing Went Wrong");
                MainGrid.DataSource = null;
                return;
            }
            DSet.DataSetName = "DocumentElement";
            if (DSet.Tables.Count == 3)
            {
                DtabPaymentPickupBillDetail = DSet.Tables[0];
                DtabPaymentSummry = DSet.Tables[1];
                DtabPaymentDetail = DSet.Tables[2];
                LblMode.Text = "Edit Mode";

                if (!DtabPaymentSummry.Columns.Contains("CLOSINGBALANCE"))
                {
                    DtabPaymentSummry.Columns.Add("CLOSINGBALANCE");
                }


                for (int i = 0; i < DtabPaymentSummry.Rows.Count; i++)
                {
                    DtabPaymentSummry.Rows[i]["CLOSINGBALANCE"] = Global.FindLedgerClosingStr(Val.ToString(DtabPaymentSummry.Rows[i]["LEDGER_ID"]).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(DtabPaymentSummry.Rows[i]["LEDGER_ID"])));
                    DtabPaymentSummry.Rows[i]["STATUS"] = "1"; //Added by Daksha on 26/04/2023
                }

                //Added by Daksha on 26/04/2023
                for (int i = 0; i < DtabPaymentPickupBillDetail.Rows.Count; i++)
                {
                    DtabPaymentPickupBillDetail.Rows[i]["STATUS"] = "1";
                }
                //lblBalance.Text = Global.FindLedgerClosingStr(Val.ToString(txtCashBankAC.Tag).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtCashBankAC.Tag)));
                //End as Daksha



                if (Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "CASH PAYMENT")
                    CmbPaymentType.SelectedIndex = 0;
                else if (Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "BANK PAYMENT")
                    CmbPaymentType.SelectedIndex = 1;
                else if (Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "CASH RECEIPT")
                    CmbPaymentType.SelectedIndex = 2;
                else if (Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "BANK RECEIPT")
                    CmbPaymentType.SelectedIndex = 3;

                txtTrnID.Text = Val.ToString(DSet.Tables[0].Rows[0]["TRN_ID"]);
                DTPVoucherDate.Text = Val.ToString(DSet.Tables[0].Rows[0]["VOUCHERDATE"]);
                txtVoucherNo.Text = Val.ToString(DSet.Tables[0].Rows[0]["VOUCHERNO"]);
                txtVoucherStr.Text = Val.ToString(DSet.Tables[0].Rows[0]["VOUCHERNOSTR"]);
                txtCurrency.Tag = Val.ToString(DSet.Tables[0].Rows[0]["CURRENCY_ID"]);
                txtCurrency.Text = Val.ToString(DSet.Tables[0].Rows[0]["CURRENCY"]);
                txtExcRate.Text = Val.ToString(DSet.Tables[0].Rows[0]["EXCRATE"]);
                txtRemark.Text = Val.ToString(DSet.Tables[0].Rows[0]["NOTE"]);
                txtChqNo.Text = Val.ToString(DSet.Tables[0].Rows[0]["CHQ_NO"]);
                DtpChqIssue.Text = Val.ToString(DSet.Tables[0].Rows[0]["CHQISSUEDT"]);
                cmbPaymentMode.SelectedItem = Val.ToString(DSet.Tables[0].Rows[0]["PAYTYPE"]);
                txtCashBankAC.Tag = Val.ToString(DSet.Tables[0].Rows[0]["LEDGER_ID"]);
                txtCashBankAC.Text = Val.ToString(DSet.Tables[0].Rows[0]["LEDGERNAME"]);

                if (Val.ToInt32(DSet.Tables[0].Rows[0]["CONVERTTOINR"]) == 1)
                    ChkBxConvertToInr.Checked = true;
                else
                    ChkBxConvertToInr.Checked = false;

                if (Val.ToInt32(DSet.Tables[0].Rows[0]["CURRENCY_ID"]) == 1)
                {
                    if (Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "CASH PAYMENT" || Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "BANK PAYMENT")
                        txtAmount.Text = Val.ToString(DSet.Tables[0].Rows[0]["CREDIT"]);
                    else
                        txtAmount.Text = Val.ToString(DSet.Tables[0].Rows[0]["DEBIT"]);
                }
                else
                {
                    if (Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "CASH PAYMENT" || Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "BANK PAYMENT")
                        txtAmount.Text = Val.ToString(DSet.Tables[0].Rows[0]["CREDITFE"]);
                    else
                        txtAmount.Text = Val.ToString(DSet.Tables[0].Rows[0]["DEBITFE"]);
                }

                if (Val.ToString(RdoCurrency.SelectedIndex.ToString()) == "1")//if (Val.ToString(txtCurrency.Text) == "USD")
                { ChkBxConvertToInr.Visible = true; lblAmount.Text = "Amount ($)"; }
                else
                { ChkBxConvertToInr.Visible = false; lblAmount.Text = "Amount (₹)"; }

                MainGrdPayment.DataSource = DtabPaymentSummry;
                MainGrdPayment.Refresh();

                MainGridDetail.DataSource = DtabPaymentPickupBillDetail;
                MainGridDetail.Refresh();
                xtraTabControl1.SelectedTabPageIndex = 0;

                DataTable DtCheckAllocated = ObjFinance.CheckBillAllocatedOrNot(txtTrnID.Text, "");
                if (DtCheckAllocated.Rows.Count > 0)
                {
                    LblBillPickupMessage.Text = "This Entry Allocated In Accounts ( " + Val.ToString(DtCheckAllocated.Rows[0][0]) + " ) ,Please Delete For Edit.";
                    BtnSave.Enabled = false;
                    BtnDelete.Enabled = false;
                    LblMode.Text = "View Mode";
                }
                else
                {
                    LblBillPickupMessage.Text = "";
                    BtnSave.Enabled = true;
                    BtnDelete.Enabled = true;
                    LblMode.Text = "Edit Mode";
                }
                RdoCurrency.SelectedIndex = 0;
            }

            //Added by Daksha on 26/04/2023
            for (int i = 0; i < DtabPaymentSummry.Rows.Count; i++)
                DTBal = new BusLib.Account.BOLedgerTransaction().FindLedgerClosingNew(Val.ToString(DtabPaymentSummry.Rows[i]["LEDGER_ID"]).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(DtabPaymentSummry.Rows[i]["LEDGER_ID"])));

            if (DTBal.Rows.Count > 0)
            {
                if (RdoCurrency.SelectedIndex.ToString() == "1") //if (Val.ToString(txtCurrency.Text) == "USD")
                {
                    if (Val.ToDouble(DTBal.Rows[0]["CloseBalanceDollar"]) < 0)
                    {
                        lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Dr";
                    }
                    else
                    {
                        lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Cr";
                    }                   
                }
                else
                {
                    if (Val.ToDouble(DTBal.Rows[0]["CloseBalanceRs"]) < 0)
                    {
                        lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Dr";
                    }
                    else
                    {
                        lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Cr";
                    }                    
                }
            }
            //End as Daksha
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

        #region Validation

        private bool ValSave()
        {

            if (Val.ISDate(DTPVoucherDate.Text) == false)
            {
                Global.Message("Entry Date Is Required");
                DTPVoucherDate.Focus();
                return false;
            }
            if (Val.ToString(txtVoucherStr.Text.Trim()) == "")
            {
                Global.Message("Voucher No Is Required");
                txtVoucherStr.Focus();
                return false;
            }
            if (Val.ToString(txtCashBankAC.Text.Trim()) == "")
            {
                Global.Message("Source Account Is Required");
                txtCashBankAC.Focus();
                return false;
            }

            if (Val.ToString(txtAmount.Text.Trim()) == "")
            {
                Global.Message("Amount Is Required");
                txtAmount.Focus();
                return false;
            }
            int intValid = 0;
            double DouDebAmt = 0, DouCrdAmt = 0, DouDebAmtFe = 0, DouCrdAmtFe = 0;
            double NewDouDebAmt = 0, NewDouCrdAmt = 0, NewDouDebAmtFe = 0, NewDouCrdAmtFe = 0;
            for (int IntI = 0; IntI < GrdDetPayment.RowCount; IntI++)
            {
                DataRow DR = GrdDetPayment.GetDataRow(IntI);
                if ((Val.Val(DR["DEBITAMOUNT"]) == 0 && Val.Val(DR["CREDITAMOUNT"]) == 0 && Val.Val(DR["DEBITAMOUNTFE"]) == 0 && Val.Val(DR["CREDITAMOUNTFE"]) == 0) && Val.ToString(DR["LEDGERNAME"]) != "")
                {
                    Global.MessageError("Amount Has Not Been Entered For SrNo. : " + Val.ToString((IntI + 1)));
                    intValid = 1;
                    break;
                }
                else
                {
                    DouDebAmt += Val.Val(DR["DEBITAMOUNT"]);
                    DouCrdAmt += Val.Val(DR["CREDITAMOUNT"]);
                    DouDebAmtFe += Val.Val(DR["DEBITAMOUNTFE"]);
                    DouCrdAmtFe += Val.Val(DR["CREDITAMOUNTFE"]);

                    NewDouDebAmt = Math.Round(DouDebAmt, 3);
                    NewDouCrdAmt = Math.Round(DouCrdAmt, 3);
                    NewDouDebAmtFe = Math.Round(DouDebAmtFe, 3);
                    NewDouCrdAmtFe = Math.Round(DouCrdAmtFe, 3);
                }
            }
            if (intValid == 1)
                return false;

            if (mFormType == FORMTYPE.CP || mFormType == FORMTYPE.BP)
            {
                if (DouDebAmt == 0 || DouDebAmt < DouCrdAmt)
                {
                    Global.MessageError("Please Pass Proper Entry");
                    return false;
                }
            }
            else
            {
                if (DouCrdAmt == 0 || DouCrdAmt < DouDebAmt)
                {
                    Global.MessageError("Please Pass Proper Entry");
                    return false;
                }
            }

            if (Val.ToInt32(Val.ToString(RdoCurrency.SelectedIndex.ToString())) == 1) //if (Val.ToInt32(Val.ToString(txtCurrency.Tag)) == 1)
            {
                if (mFormType == FORMTYPE.CP || mFormType == FORMTYPE.BP)
                {
                    DouCrdAmt += Val.Val(txtAmount.Text);
                    if (BOConfiguration.gStrLoginSection == "B")
                    {
                        //DouCrdAmtFe += Math.Round((Val.Val(txtAmount.Text) * Val.Val(txtExcRate.Text)) / 1000, 2); //Comment by Daksha on 25/03/2023
                        DouCrdAmtFe += Math.Round(Val.ToDouble(txtAmount.Text) * Val.ToDouble(txtExcRate.Text), 3);
                    }
                    else
                    {
                        DouCrdAmtFe += Math.Round(Val.Val(txtAmount.Text) * Val.Val(txtExcRate.Text), 2);
                    }

                    if ((DouCrdAmtFe != DouDebAmtFe) || (DouCrdAmt != DouDebAmt))
                    {
                        Global.MessageError("Please Debit And Credit Side Not Matching");
                        return false;
                    }
                }
                else
                {
                    //NewDouDebAmt += Val.Val(txtAmount.Text);
                    NewDouDebAmt += Math.Round(Val.Val(txtAmount.Text),3);

                    if (BOConfiguration.gStrLoginSection == "B")
                    {
                        //DouDebAmtFe += Math.Round((Val.Val(txtAmount.Text) * Val.Val(txtExcRate.Text)) / 1000, 2);
                        NewDouDebAmtFe += Math.Round((Val.Val(txtAmount.Text) * Val.Val(txtExcRate.Text)), 3);
                    }
                    else
                    {
                        NewDouDebAmtFe += Math.Round(Val.Val(txtAmount.Text) * Val.Val(txtExcRate.Text), 2);
                    }

                    //if ((DouCrdAmtFe != DouDebAmtFe) || (DouCrdAmt != DouDebAmt))
                    if ((Val.ToDouble(NewDouCrdAmtFe) != Val.ToDouble(NewDouDebAmtFe)) || (Val.ToDouble(NewDouCrdAmt) != Val.ToDouble(NewDouDebAmt)))
                    {
                        Global.MessageError("Please Debit And Credit Side Not Matching");
                        return false;
                    }
                }
            }
            else
            {
                if (mFormType == FORMTYPE.CP || mFormType == FORMTYPE.BP)
                {
                    if (BOConfiguration.gStrLoginSection == "B")
                    {
                        //DouCrdAmt += Math.Round((Val.Val(txtAmount.Text) * 1000) / Val.Val(txtExcRate.Text), 2); //Comment by Daksha on 25/04/2023
                        DouCrdAmt += Math.Round(Val.ToDouble(txtAmount.Text) / Val.ToDouble(txtExcRate.Text), 3);
                    }
                    else
                    {
                        DouCrdAmt += Math.Round(Val.Val(txtAmount.Text) / Val.Val(txtExcRate.Text), 2);
                    }
                    DouCrdAmtFe += Val.Val(txtAmount.Text);

                    if ((DouCrdAmtFe != DouDebAmtFe) || (DouCrdAmt != DouDebAmt))
                    {
                        Global.MessageError("Please Debit And Credit Side Not Matching");
                        return false;
                    }
                }
                else
                {
                    if (BOConfiguration.gStrLoginSection == "B")
                    {
                        //DouDebAmt += Math.Round((Val.Val(txtAmount.Text) * 1000) / Val.Val(txtExcRate.Text), 2); //Comment by Daksha on 25/04/2023
                        DouDebAmt += Math.Round(Val.ToDouble(txtAmount.Text) / Val.ToDouble(txtExcRate.Text), 3);
                    }
                    else
                    {
                        DouDebAmt += Math.Round(Val.Val(txtAmount.Text) / Val.Val(txtExcRate.Text), 2);
                    }
                    DouDebAmtFe += Val.Val(txtAmount.Text);

                    if ((DouCrdAmtFe != DouDebAmtFe) || (DouCrdAmt != DouDebAmt))
                    {
                        Global.MessageError("Please Debit And Credit Side Not Matching");
                        return false;
                    }
                }
            }
            if (RdoCurrency.SelectedIndex == 1) //if (Val.ToString(txtCurrency.Text) == "USD") //Added Condition by Daksha on 25/04/2023
            {
                if (Val.Val(txtExcRate.Text) < 50)
                {
                    Global.Message("Please Enter Proper Exchange Rate");
                    txtCurrency.Focus();
                    return false;
                }
            }

            return true;
        }


        #endregion

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCashAC_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "LEDGERCODE,LEDGERNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;

                    string Str = Val.Left(Val.ToString(CmbPaymentType.SelectedItem), 2);
                    if (Str == "CP" || Str == "CR")
                    {
                        FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LEDGERCASH);
                    }
                    else if (Str == "BP" || Str == "BR")
                    {
                        FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LEDGERBANK);
                    }

                    FrmSearch.mStrColumnsToHide = "LEDGER_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtCashBankAC.Text = Val.ToString(FrmSearch.DRow["LEDGERNAME"]);
                        txtCashBankAC.Tag = Val.ToString(FrmSearch.DRow["LEDGER_ID"]);
                        BtnAddRow_Click(null, null); //Added by Daksha on 26/04/2023
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;

                    //Added by Daksha on 26/04/2023                    
                    DTBal = new BusLib.Account.BOLedgerTransaction().FindLedgerClosingNew(Val.ToString(txtCashBankAC.Tag).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtCashBankAC.Tag)));
                    if (DTBal.Rows.Count > 0)
                    {
                        if (RdoCurrency.SelectedIndex.ToString() == "1") //if (Val.ToString(txtCurrency.Text) == "USD")
                        {
                            if (Val.ToDouble(DTBal.Rows[0]["CloseBalanceDollar"]) < 0)
                            {
                                lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Dr";
                            }
                            else
                            {
                                lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Cr";
                            }
                        }
                        else
                        {
                            if (Val.ToDouble(DTBal.Rows[0]["CloseBalanceRs"]) < 0)
                            {
                                lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Dr";
                            }
                            else
                            {
                                lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Cr";
                            }
                        }
                    }
                    //Old Code
                    //lblBalance.Text = Global.FindLedgerClosingStr(Val.ToString(txtCashBankAC.Tag).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtCashBankAC.Tag)));
                    //End as Daksha

                }

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
                if (ValSave() == false)
                {
                    return;
                }

                if (Global.Confirm("Are You Sure For Payment Entry") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                //for (int IntI = 0; IntI < GrdDetPayment.RowCount; IntI++)
                //{
                //    DataRow DR = GrdDetPayment.GetDataRow(IntI);

                //    if (Val.Val(DR["PAYMENTAMOUNT"]) == 0)
                //    {
                //        continue;
                //    }

                //    if (Val.ToString(txtCurrency.Text) == "USD" && Val.Val(DR["PAYMENTAMOUNT"]) > Val.Val(DR["PENDINGAMOUNT"]))
                //    {
                //        Global.Message("PAYMENT AMOUNT IS GREATER THEN BILL PENDING AMOUNT, PLEASE CHECK BILLNO : '" + Val.ToString(DR["JANGEDNOSTR"]) + "'");
                //        return;
                //    }

                //    if (Val.ToString(txtCurrency.Text) != "USD" && Val.Val(DR["FPAYMENTAMOUNT"]) > Val.Val(DR["PENDINGAMOUNTFE"]))
                //    {
                //        Global.Message("PAYMENT AMOUNT IS GREATER THEN BILL PENDING AMOUNT, PLEASE CHECK BILLNO : '" + Val.ToString(DR["JANGEDNOSTR"]) + "'");
                //        return;
                //    }
                //}

                TRN_LedgerTranJournalProperty AccoutProperty = new TRN_LedgerTranJournalProperty();

                //Added by Daksha on 25/04/2023
                var DRows = DtabPaymentSummry.Rows.Cast<DataRow>().Where(row => Val.ToString(row["SRNO"]) == "").ToArray();
                foreach (DataRow dr in DRows)
                    DtabPaymentSummry.Rows.Remove(dr);

                //End as Daksha
                for (int i = 0; i < DtabPaymentSummry.Rows.Count; i++) //Added by Daksha on 25/04/2023
                {

                    //if (DtabPaymentSummry.Rows[i]["STATUS"].ToString() == "")
                    //{
                    AccoutProperty.TRN_ID = Val.ToString(txtTrnID.Text).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtTrnID.Text));
                    //Added by Daksha on 26/04/2023
                    DtabPaymentSummry.Rows[i]["TRN_ID"] = AccoutProperty.TRN_ID;
                    int SRNOFORDTL = Val.ToInt32(DtabPaymentSummry.Rows[i]["SRNO"]);
                    //End as Daksha
                    AccoutProperty.ACCLEDGTRNTRN_ID = Val.ToString("00000000-0000-0000-0000-000000000000");
                    AccoutProperty.ACCLEDGTRNSRNO = 1;

                    AccoutProperty.VOUCHERDATE = Val.SqlDate(DTPVoucherDate.Text);
                    AccoutProperty.FINYEAR = Config.FINYEARNAME;//"2020-2021";
                    AccoutProperty.VOUCHERNO = Val.ToInt32(txtVoucherNo.Text);
                    AccoutProperty.VOUCHERSTR = txtVoucherStr.Text;
                    AccoutProperty.CURRENCY_ID = Val.ToInt32(RdoCurrency.SelectedIndex.ToString()); //Val.ToInt32(txtCurrency.Tag);
                    AccoutProperty.EXCRATE = Val.Val(txtExcRate.Text);
                    AccoutProperty.MEMO_ID = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    AccoutProperty.REFTRN_ID = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    AccoutProperty.REFSRNO = 0;
                    AccoutProperty.BILL_NO = "";
                    AccoutProperty.NOTE = txtRemark.Text;
                    AccoutProperty.BILL_DT = "01/01/1900";
                    AccoutProperty.EXCRATEDIFF = 0;
                    AccoutProperty.TERMSDATE = Val.SqlDate(DTPVoucherDate.Text);
                    AccoutProperty.CHQ_NO = txtChqNo.Text;
                    AccoutProperty.CHQISSUEDT = Val.SqlDate(DtpChqIssue.Text);
                    //AccoutProperty.CHQCLEARDT = "01/01/1900";
                    AccoutProperty.DATAFREEZ = 0;
                    AccoutProperty.PAYTYPE = Val.ToString(cmbPaymentMode.SelectedItem);
                    AccoutProperty.REFTYPE = "";
                    AccoutProperty.PAYTERMS = 0;
                    AccoutProperty.REFBOOKTYPEFULL = "";
                    AccoutProperty.FROMLEDGER_ID = Guid.Parse(Val.ToString(txtCashBankAC.Tag));
                    AccoutProperty.REFLEDGER_ID = Guid.Parse(Val.ToString(DtabPaymentSummry.Rows[i]["LEDGER_ID"]));//Guid.Parse(Val.ToString(DtabPaymentSummry.Rows[0]["LEDGER_ID"]));
                    AccoutProperty.CONVERTTOINR = Val.ToBooleanToInt(ChkBxConvertToInr.Checked);
                    string AccountSaveForXML = string.Empty;


                    for (int j = 0; j < DtabPaymentPickupBillDetail.Rows.Count; j++)
                    {
                        DtabPaymentPickupBillDetail.Rows[j]["SRNO"] = Val.ToString(j + 1);
                    }

                    var rows = DtabPaymentSummry.Rows.Cast<DataRow>().Where(row => Val.ToString(row["RefType"]) == "New Ref").ToArray();
                    int IntCnt = DtabPaymentPickupBillDetail.Rows.Count + 1;
                    foreach (DataRow Dr in rows)
                    {

                        DataRow NewDr = DtabPaymentPickupBillDetail.NewRow();

                        NewDr["TRN_ID"] = "00000000-0000-0000-0000-000000000000";
                        NewDr["SRNO"] = IntCnt;
                        NewDr["ACCLEDGTRNTRN_ID"] = "00000000-0000-0000-0000-000000000000";
                        NewDr["ACCLEDGTRNSRNO"] = IntCnt;
                        NewDr["ENTRYTYPE"] = "";
                        NewDr["BOOKTYPEFULL"] = "";

                        NewDr["VOUCHERNOSTR"] = "";
                        NewDr["VOUCHERDATE"] = "01/01/1900";
                        NewDr["FINYEAR"] = "";
                        NewDr["LEDGER_ID"] = Dr["LEDGER_ID"];
                        NewDr["LEDGERNAME"] = Dr["LEDGERNAME"];

                        NewDr["CURRENCY_ID"] = "0";
                        NewDr["CURRENCY"] = "0";
                        NewDr["PENDDEBITAMOUNT"] = "0";
                        NewDr["PENDCREDITAMOUNT"] = "0";
                        NewDr["EXCRATE"] = "0";
                        NewDr["PENDDEBITAMOUNTFE"] = "0";
                        NewDr["PENDCREDITAMOUNTFE"] = "0";
                        NewDr["BILL_NO"] = txtVoucherStr.Text;

                        NewDr["BILL_DT"] = Val.ToString(DTPVoucherDate.Value);// DateTime.ParseExact(Val.ToString(DTPEntryDate.Value), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd hh:mm:ss");// Val.SqlDate(DTPEntryDate.Text);

                        NewDr["DEBIT"] = Dr["DEBITAMOUNT"];
                        NewDr["CREDIT"] = Dr["CREDITAMOUNT"];
                        NewDr["DEBITFE"] = Dr["DEBITAMOUNTFE"];
                        NewDr["CREDITFE"] = Dr["CREDITAMOUNTFE"];

                        NewDr["REFTRN_ID"] = "00000000-0000-0000-0000-000000000000";
                        NewDr["REFSRNO"] = "0";
                        NewDr["REFACCLEDGTRNTRN_ID"] = "00000000-0000-0000-0000-000000000000";
                        NewDr["REFACCLEDGTRNSRNO"] = "0";

                        NewDr["REFTYPE"] = "New Ref";
                        NewDr["REFLEDGER_ID"] = Dr["REFLEDGER_ID"];
                        NewDr["NOTE"] = Dr["NOTE"]; //"";
                        NewDr["MainGrdRow_id"] = IntCnt;
                        DtabPaymentPickupBillDetail.Rows.Add(NewDr);
                        IntCnt++;

                    }
                    AccoutProperty.REFTYPE = "New Ref";
                    AccoutProperty.SRNO = DtabPaymentPickupBillDetail.Rows.Count + 1;
                    AccoutProperty.Mode = LblMode.Text;

                    //Added by Daksha on 26/04/2023
                    DataTable dt1 = new DataTable();
                    var DRow1 = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToInt32(row["MainGrdRow_id"]) == SRNOFORDTL).ToArray();
                    foreach (DataRow dr in DRow1)
                        dt1 = DtabPaymentPickupBillDetail.AsEnumerable().Where(rows1 => Val.ToInt32(rows1["MainGrdRow_id"]) == SRNOFORDTL && Val.ToString(rows1["STATUS"]) == "").CopyToDataTable();

                    DataRow[] foundRows = DtabPaymentPickupBillDetail.Select("MainGrdRow_id=" + "'" + SRNOFORDTL + "'");
                    if (foundRows.Length > 0)
                    {
                        foreach (DataRow drAdd in foundRows)
                        {
                            drAdd["TRN_ID"] = AccoutProperty.TRN_ID;
                            drAdd["STATUS"] = 1;
                        }
                    }
                    dt1.TableName = "ACCOUNT";
                    using (StringWriter sw = new StringWriter())
                    {
                        dt1.WriteXml(sw);
                        AccountSaveForXML = sw.ToString();
                    }
                    //Old Code
                    //DtabPaymentPickupBillDetail.TableName = "ACCOUNT";
                    //using (StringWriter sw = new StringWriter())
                    //{
                    //    DtabPaymentPickupBillDetail.WriteXml(sw);
                    //    AccountSaveForXML = sw.ToString();
                    //}
                    //End as Daksha

                    if (mFormType == FORMTYPE.BP)
                    {
                        AccoutProperty.ENTRYTYPE = "PAYMENT";
                        AccoutProperty.BOOKTYPE = "BP";
                        AccoutProperty.BOOKTYPEFULL = "BANK PAYMENT";
                        AccoutProperty.TRNTYPE = "BANK PAYMENT";
                        if (RdoCurrency.SelectedIndex.ToString() == "1") //if (txtCurrency.Text == "USD")
                        {
                            AccoutProperty.DEBAMOUNTFE = 0;
                            AccoutProperty.DEBAMOUNT = 0;
                            AccoutProperty.CRDAMOUNT = Val.Val(txtAmount.Text);
                            AccoutProperty.CRDAMOUNTFE = Val.Val(Val.ToDecimal(txtAmount.Text) * Val.ToDecimal(txtExcRate.Text));
                        }
                        else
                        {
                            AccoutProperty.DEBAMOUNTFE = 0;
                            AccoutProperty.DEBAMOUNT = 0;
                            AccoutProperty.CRDAMOUNTFE = Val.Val(txtAmount.Text);
                            if (Val.ToDecimal(txtExcRate.Text) != 0)
                                AccoutProperty.CRDAMOUNT = Val.Val(Val.ToDecimal(txtAmount.Text) / Val.ToDecimal(txtExcRate.Text));
                            else
                                AccoutProperty.CRDAMOUNT = Val.Val(txtAmount.Text);
                        }

                    }
                    else if (mFormType == FORMTYPE.CP)
                    {
                        AccoutProperty.ENTRYTYPE = "PAYMENT";
                        AccoutProperty.BOOKTYPE = "CP";
                        AccoutProperty.BOOKTYPEFULL = "CASH PAYMENT";
                        AccoutProperty.TRNTYPE = "CASH PAYMENT";
                        if (RdoCurrency.SelectedIndex.ToString() == "1") //if (txtCurrency.Text == "USD")
                        {
                            AccoutProperty.DEBAMOUNTFE = 0;
                            AccoutProperty.DEBAMOUNT = 0;
                            AccoutProperty.CRDAMOUNT = Val.Val(txtAmount.Text);
                            AccoutProperty.CRDAMOUNTFE = Val.Val(Val.ToDecimal(txtAmount.Text) * Val.ToDecimal(txtExcRate.Text));
                        }
                        else
                        {
                            AccoutProperty.DEBAMOUNTFE = 0;
                            AccoutProperty.DEBAMOUNT = 0;
                            AccoutProperty.CRDAMOUNTFE = Val.Val(txtAmount.Text);
                            if (Val.ToDecimal(txtExcRate.Text) != 0)
                                AccoutProperty.CRDAMOUNT = Val.Val(Val.ToDecimal(txtAmount.Text) / Val.ToDecimal(txtExcRate.Text));
                            else
                                AccoutProperty.CRDAMOUNT = Val.Val(txtAmount.Text);
                        }
                    }
                    else if (mFormType == FORMTYPE.BR)
                    {
                        AccoutProperty.ENTRYTYPE = "RECEIPT";
                        AccoutProperty.BOOKTYPE = "BR";
                        AccoutProperty.BOOKTYPEFULL = "BANK RECEIPT";
                        AccoutProperty.TRNTYPE = "BANK RECEIPT";
                        if (RdoCurrency.SelectedIndex.ToString() == "1") //if (txtCurrency.Text == "USD")
                        {
                            AccoutProperty.CRDAMOUNTFE = 0;
                            AccoutProperty.CRDAMOUNT = 0;
                            AccoutProperty.DEBAMOUNT = Val.Val(txtAmount.Text);
                            AccoutProperty.DEBAMOUNTFE = Val.Val(Val.ToDecimal(txtAmount.Text) * Val.ToDecimal(txtExcRate.Text));
                        }
                        else
                        {
                            AccoutProperty.CRDAMOUNTFE = 0;
                            AccoutProperty.CRDAMOUNT = 0;
                            AccoutProperty.DEBAMOUNTFE = Val.Val(txtAmount.Text);
                            if (Val.ToDecimal(txtExcRate.Text) != 0)
                                AccoutProperty.DEBAMOUNT = Val.Val(Val.ToDecimal(txtAmount.Text) / Val.ToDecimal(txtExcRate.Text));
                            else
                                AccoutProperty.DEBAMOUNT = Val.Val(txtAmount.Text);
                        }
                    }
                    else if (mFormType == FORMTYPE.CR)
                    {
                        AccoutProperty.ENTRYTYPE = "RECEIPT";
                        AccoutProperty.BOOKTYPE = "CR";
                        AccoutProperty.BOOKTYPEFULL = "CASH RECEIPT";
                        AccoutProperty.TRNTYPE = "CASH RECEIPT";
                        if (RdoCurrency.SelectedIndex.ToString() == "1") //if (txtCurrency.Text == "USD")
                        {
                            AccoutProperty.CRDAMOUNTFE = 0;
                            AccoutProperty.CRDAMOUNT = 0;
                            AccoutProperty.DEBAMOUNT = Val.Val(txtAmount.Text);
                            AccoutProperty.DEBAMOUNTFE = Val.Val(Val.ToDecimal(txtAmount.Text) * Val.ToDecimal(txtExcRate.Text));
                        }
                        else
                        {
                            AccoutProperty.CRDAMOUNTFE = 0;
                            AccoutProperty.CRDAMOUNT = 0;
                            AccoutProperty.DEBAMOUNTFE = Val.Val(txtAmount.Text);
                            if (Val.ToDecimal(txtExcRate.Text) != 0)
                                AccoutProperty.DEBAMOUNT = Val.Val(Val.ToDecimal(txtAmount.Text) / Val.ToDecimal(txtExcRate.Text));
                            else
                                AccoutProperty.DEBAMOUNT = Val.Val(txtAmount.Text);
                        }
                    }

                    //Added by Daksha on 26/04/2023
                    DataTable dt = new DataTable();
                    var DRow = DtabPaymentSummry.Rows.Cast<DataRow>().Where(row => Val.ToString(row["STATUS"]) == "" && Val.ToInt32(row["SRNO"]) == SRNOFORDTL).ToArray();
                    foreach (DataRow dr in DRow)
                        dt = DtabPaymentSummry.AsEnumerable().Where(rows1 => Val.ToString(rows1["STATUS"]) == "" && Val.ToInt32(rows1["SRNO"]) == SRNOFORDTL).CopyToDataTable();

                    dt.TableName = "ACCOUNTSUM";
                    string AccountSummrySaveForXML;
                    using (StringWriter sw = new StringWriter())
                    {
                        dt.WriteXml(sw);
                        AccountSummrySaveForXML = sw.ToString();
                    }

                    dt.TableName = "ACCOUNTMED";
                    string AccountMeditorSaveForXML;
                    using (StringWriter sw = new StringWriter())
                    {
                        dt.WriteXml(sw);
                        AccountMeditorSaveForXML = sw.ToString();
                    }

                    //Old Code
                    //DtabPaymentSummry.TableName = "ACCOUNTSUM";
                    //string AccountSummrySaveForXML;
                    //using (StringWriter sw = new StringWriter())
                    //{
                    //    DtabPaymentSummry.WriteXml(sw);
                    //    AccountSummrySaveForXML = sw.ToString();
                    //}

                    //DtabPaymentDetail.TableName = "ACCOUNTMED";
                    //string AccountMeditorSaveForXML;
                    //using (StringWriter sw = new StringWriter())
                    //{
                    //    DtabPaymentDetail.WriteXml(sw);
                    //    AccountMeditorSaveForXML = sw.ToString();
                    //}
                    //End as Daksha

                    AccountSaveForXML = Regex.Replace(AccountSaveForXML,
                            @"<BILL_DT>(?<year>\d{4})-(?<month>\d{2})-(?<date>\d{2}).*?</BILL_DT>",
                            @"<BILL_DT>${month}/${date}/${year}</BILL_DT>",
                            RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                    AccountSaveForXML = Regex.Replace(AccountSaveForXML,
                           @"<REFVOUCHERDATE>(?<year>\d{4})-(?<month>\d{2})-(?<date>\d{2}).*?</REFVOUCHERDATE>",
                           @"<REFVOUCHERDATE>${month}/${date}/${year}</REFVOUCHERDATE>",
                           RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
                    AccountMeditorSaveForXML = Regex.Replace(AccountMeditorSaveForXML,
                         @"<BILL_DT>(?<year>\d{4})-(?<month>\d{2})-(?<date>\d{2}).*?</BILL_DT>",
                         @"<BILL_DT>${month}/${date}/${year}</BILL_DT>",
                         RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);


                    AccoutProperty = ObjFinance.SaveAccountingEffectFinance(AccoutProperty, AccountSaveForXML, AccountSummrySaveForXML, AccountMeditorSaveForXML);

                    this.Cursor = Cursors.Default;
                    //Added by Daksha on 26/04/2023
                    if (AccoutProperty.ReturnMessageType == "SUCCESS")
                    {
                        DtabPaymentSummry.Rows[i]["STATUS"] = 1;
                        IntSumryCount = 1;
                        IntAllowToCalculate = 0;
                    }
                    MainGrdPayment.DataSource = DtabPaymentSummry;
                    MainGrdPayment.RefreshDataSource();
                    //End as Daksha
                    //}
                }

                if (AccoutProperty.ReturnMessageType == "SUCCESS")
                {
                    Global.Message(AccoutProperty.ReturnMessageDesc);
                    BtnClear.PerformClick();
                }
            }
            catch (System.Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtTrnID.Text = "";
            txtTrnID.Tag = "";
            txtCashBankAC.Text = "";
            txtCashBankAC.Tag = "";
            lblBalance.Text = "( Balance )";

            txtVoucherStr.Text = "";
            txtVoucherNo.Text = "";

            LblMode.Text = "Add Mode";

            txtCurrency.Text = "";
            txtCurrency.Tag = "";

            CmbPaymentType.SelectedIndex = 0;
            RdoCurrency.SelectedIndex = 0;
            txtAmount.Text = "0";
            txtRefDocNo.Text = "";
            txtRemark.Text = "";
            txtExcRate.Text = "";

            DTPVoucherDate.Text = DateTime.Now.ToString();

            MainGrdPayment.DataSource = null;
            MainGrdPayment.Refresh();

            txtFinYear.Text = Config.FINYEARNAME;

            string Str = Val.Left(CmbPaymentType.SelectedItem.ToString(), 2);

            txtVoucherNo.Text = objLedgerTrn.FindVoucherNoNew(txtFinYear.Text, Str).ToString();
            txtVoucherStr.Text = Config.FINYEARSHORTNAME + "/" + Str + "/" + txtVoucherNo.Text;

            DtabPaymentPickupBillDetail.Rows.Clear();
            DtabPaymentSummry.Rows.Clear();
            DtabPaymentDetail.Rows.Clear();
            IntSumryCount = 1;
            //Kuldeep 19012021
            IntAllowToCalculate = 0;
            Global.SelectLanguage(Global.LANGUAGE.ENGLISH);
            RdoCurrency_SelectedIndexChanged(null, null);
            CmbPaymentType_SelectedIndexChanged(null, null);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {

                if (Val.ToString(txtTrnID.Text).Trim().Equals(string.Empty))
                    return;

                if (Global.Confirm("Are You Sure To Delete This Payment Record ?") == System.Windows.Forms.DialogResult.No)
                    return;
                 FrmPassword FrmPassword = new FrmPassword();              
                 if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                 {
                     this.Cursor = Cursors.WaitCursor;

                     LedgerTransactionProperty Property = new LedgerTransactionProperty();
                     Property.Trn_ID = Val.ToString(txtTrnID.Text).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtTrnID.Text));

                     Property = objLedgerTrn.DeleteNew(Property);

                     Global.Message(Property.ReturnMessageDesc);
                     if (Property.ReturnMessageType == "SUCCESS")
                     {
                         BtnClear_Click(null, null);
                     }
                 }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void CmbPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCashBankAC.Text = "";
            txtCashBankAC.Tag = "";

            string Str = Val.Left(CmbPaymentType.SelectedItem.ToString(), 2);

            //Added by Daksha on 26/04/2023
            if (Str == "CP" || Str == "CR")
            {
                DTCASH = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LEDGERCASH);
            }
            else if (Str == "BP" || Str == "BR")
            {
                DTBANK = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LEDGERBANK);
            }
            //End as Daksha

            switch (Str)
            {
                case "CP":
                    mFormType = FORMTYPE.CP;
                    lblAccount.Text = "Cash A/C";
                    cmbPaymentMode.SelectedIndex = 0;
                    cmbPaymentMode.Enabled = false;
                    txtChqNo.Enabled = false;
                    DtpChqIssue.Enabled = false;
                    //GrdDetail.Columns["PENDCREDITAMOUNTFE"].Visible = false;
                    //GrdDetail.Columns["PENDCREDITAMOUNT"].Visible = false;
                    //GrdDetail.Columns["PENDDEBITAMOUNTFE"].Visible = true;
                    //GrdDetail.Columns["PENDDEBITAMOUNT"].Visible = true;
                    if (DTCASH.Rows.Count > 0)
                    {
                        txtCashBankAC.Text = Val.ToString(DTCASH.Rows[0]["LEDGERNAME"]);
                        txtCashBankAC.Tag = Val.ToString(DTCASH.Rows[0]["LEDGER_ID"]);
                        txtCashBankAC.Select();
                    }
                    break;
                case "CR":
                    mFormType = FORMTYPE.CR;
                    lblAccount.Text = "Cash A/C";
                    cmbPaymentMode.SelectedIndex = 0;
                    cmbPaymentMode.Enabled = false;
                    txtChqNo.Enabled = false;
                    DtpChqIssue.Enabled = false;
                    //GrdDetail.Columns["PENDCREDITAMOUNTFE"].Visible = true;
                    //GrdDetail.Columns["PENDCREDITAMOUNT"].Visible = true;
                    //GrdDetail.Columns["PENDDEBITAMOUNTFE"].Visible = false;
                    //GrdDetail.Columns["PENDDEBITAMOUNT"].Visible = false;
                    if (DTCASH.Rows.Count > 0)
                    {
                        txtCashBankAC.Text = Val.ToString(DTCASH.Rows[0]["LEDGERNAME"]);
                        txtCashBankAC.Tag = Val.ToString(DTCASH.Rows[0]["LEDGER_ID"]);
                        txtCashBankAC.Select();
                    }
                    break;
                case "BP":
                    mFormType = FORMTYPE.BP;
                    lblAccount.Text = "Bank A/C";
                    cmbPaymentMode.SelectedIndex = 1;
                    cmbPaymentMode.Enabled = true;
                    txtChqNo.Enabled = true;
                    DtpChqIssue.Enabled = true;
                    //GrdDetail.Columns["PENDCREDITAMOUNTFE"].Visible = false;
                    //GrdDetail.Columns["PENDCREDITAMOUNT"].Visible = false;
                    //GrdDetail.Columns["PENDDEBITAMOUNTFE"].Visible = true;
                    //GrdDetail.Columns["PENDDEBITAMOUNT"].Visible = true;
                    if (DTBANK.Rows.Count > 0)
                    {
                        txtCashBankAC.Text = Val.ToString(DTBANK.Rows[0]["LEDGERNAME"]);
                        txtCashBankAC.Tag = Val.ToString(DTBANK.Rows[0]["LEDGER_ID"]);
                        txtCashBankAC.Select();
                    }
                    break;
                case "BR":
                    mFormType = FORMTYPE.BR;
                    lblAccount.Text = "Bank A/C";
                    cmbPaymentMode.SelectedIndex = 1;
                    cmbPaymentMode.Enabled = true;
                    txtChqNo.Enabled = true;
                    DtpChqIssue.Enabled = true;
                    //GrdDetail.Columns["PENDCREDITAMOUNTFE"].Visible = true;
                    //GrdDetail.Columns["PENDCREDITAMOUNT"].Visible = true;
                    //GrdDetail.Columns["PENDDEBITAMOUNTFE"].Visible = false;
                    //GrdDetail.Columns["PENDDEBITAMOUNT"].Visible = false;
                    if (DTBANK.Rows.Count > 0)
                    {
                        txtCashBankAC.Text = Val.ToString(DTBANK.Rows[0]["LEDGERNAME"]);
                        txtCashBankAC.Tag = Val.ToString(DTBANK.Rows[0]["LEDGER_ID"]);
                        txtCashBankAC.Select();
                    }
                    break;
                case "CO":
                    mFormType = FORMTYPE.CO;
                    lblAccount.Text = "Account";
                    break;
                default:
                    break;
            }

            txtVoucherNo.Text = objLedgerTrn.FindVoucherNoNew(txtFinYear.Text, Str).ToString();
            txtVoucherStr.Text = Config.FINYEARSHORTNAME + "/" + Str + "/" + txtVoucherNo.Text;

        }

        private void txtCurrency_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "CURRENCYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CURRENCY);

                    FrmSearch.mStrColumnsToHide = "CURRENCY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtCurrency.Text = Val.ToString(FrmSearch.DRow["CURRENCYNAME"]);
                        txtCurrency.Tag = Val.ToString(FrmSearch.DRow["CURRENCY_ID"]);

                        if (Val.ToString(txtCurrency.Text) == "USD")
                        { ChkBxConvertToInr.Visible = true; lblAmount.Text = "Amount ($)"; }
                        else
                        { ChkBxConvertToInr.Visible = false; lblAmount.Text = "Amount (₹)"; }

                        //txtExcRate.Text = new BOTRN_MemoEntry().GetExchangeRate(Val.ToInt(txtCurrency.Tag), Val.SqlDate(DTPEntryDate.Value.ToShortDateString()), mFormType.ToString()).ToString();
                        txtExcRate.Text = new BOTRN_MemoEntry().GetExchangeRate(Val.ToInt(RdoCurrency.SelectedIndex.ToString()), Val.SqlDate(DTPVoucherDate.Value.ToShortDateString()), mFormType.ToString(),"").ToString();
                        txtExcRate_Validated(null, null);

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

        private void txtExcRate_Validated(object sender, EventArgs e)
        {
            try
            {
                decimal decExcRateDiffDebitOld = 0, decExcRateDiffCreditOld = 0, decExcRateDiffOld = 0, decExcRateDiffNewDeb = 0, decExcRateDiffNewCrd = 0;
                DataTable DtExchRateDiff = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "EXCRATEDIFF");

                for (int IntI = 0; IntI < DtabPaymentPickupBillDetail.Rows.Count; IntI++)
                {
                    if (RdoCurrency.SelectedIndex.ToString() == "1") //if (txtCurrency.Text == "USD")
                    {
                        if (Val.Val(DtabPaymentPickupBillDetail.Rows[IntI]["DEBIT"]) > 0 || Val.Val(DtabPaymentPickupBillDetail.Rows[IntI]["CREDIT"]) > 0)
                        {
                            DtabPaymentPickupBillDetail.Rows[IntI]["DEBITFE"] = Math.Round(Val.Val(DtabPaymentPickupBillDetail.Rows[IntI]["DEBIT"]) * Val.Val(txtExcRate.Text),2);
                            DtabPaymentPickupBillDetail.Rows[IntI]["CREDITFE"] = Math.Round(Val.Val(DtabPaymentPickupBillDetail.Rows[IntI]["CREDIT"]) * Val.Val(txtExcRate.Text),2);
                        }
                    }
                    else
                    {
                        if (Val.Val(DtabPaymentPickupBillDetail.Rows[IntI]["DEBIT"]) > 0 || Val.Val(DtabPaymentPickupBillDetail.Rows[IntI]["CREDIT"]) > 0)
                        {
                            DtabPaymentPickupBillDetail.Rows[IntI]["DEBIT"] = Math.Round(Val.Val(DtabPaymentPickupBillDetail.Rows[IntI]["DEBITFE"]) / Val.Val(txtExcRate.Text),2);
                            DtabPaymentPickupBillDetail.Rows[IntI]["CREDIT"] = Math.Round(Val.Val(DtabPaymentPickupBillDetail.Rows[IntI]["CREDITFE"]) / Val.Val(txtExcRate.Text),2);
                        }
                    }
                }

                for (int IntI = 0; IntI < DtabPaymentDetail.Rows.Count; IntI++)
                {
                    if (RdoCurrency.SelectedIndex.ToString() == "1") //if (txtCurrency.Text == "USD")
                    {
                        DtabPaymentDetail.Rows[IntI]["FPAYMENTAMOUNT"] = Math.Round(Val.Val(DtabPaymentDetail.Rows[IntI]["PAYMENTAMOUNT"]) * Val.Val(txtExcRate.Text),2);
                        DtabPaymentDetail.Rows[IntI]["FEBANKCHARGES"] = Math.Round(Val.Val(DtabPaymentDetail.Rows[IntI]["BANKCHARGES"]) * Val.Val(txtExcRate.Text),2);
                    }
                    else
                    {
                        DtabPaymentDetail.Rows[IntI]["PAYMENTAMOUNT"] = Math.Round(Val.Val(DtabPaymentDetail.Rows[IntI]["FPAYMENTAMOUNT"]) / Val.Val(txtExcRate.Text),2);
                        DtabPaymentDetail.Rows[IntI]["BANKCHARGES"] = Math.Round(Val.Val(DtabPaymentDetail.Rows[IntI]["FEBANKCHARGES"]) / Val.Val(txtExcRate.Text),2);
                    }


                    if (Val.ToString(DtabPaymentDetail.Rows[IntI]["PAYMENTMADE"]) == "Full")
                    {
                        int IntCurrId = Val.ToInt32(DtabPaymentDetail.Rows[IntI]["CURRENCY_ID"]);
                        if (IntCurrId == 1 && Val.ToInt32(RdoCurrency.SelectedIndex.ToString()) == 1) //if (IntCurrId == 1 && Val.ToInt32(txtCurrency.Tag) == 1)
                        {
                            double DouPendAmt, DouBankChrge, DouPaymntAmt;
                            double DouExcRate = Val.Val(txtExcRate.Text);
                            DouPaymntAmt = Val.Val(DtabPaymentDetail.Rows[IntI]["FPAYMENTAMOUNT"]);
                            if (mFormType == FORMTYPE.CP || mFormType == FORMTYPE.BP)
                            {
                                if (Val.Val(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]) > 0)
                                {
                                    decExcRateDiffDebitOld = Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]);
                                    decExcRateDiffCreditOld = 0;
                                    decExcRateDiffOld += Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]);
                                }
                                else
                                {
                                    decExcRateDiffDebitOld = 0;
                                    decExcRateDiffCreditOld = Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]) * -1;
                                    decExcRateDiffOld -= Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]) * -1;
                                }
                            }
                            else
                            {
                                if (Val.Val(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]) < 0)
                                {
                                    decExcRateDiffDebitOld = Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]) * -1;
                                    decExcRateDiffCreditOld = 0;
                                    decExcRateDiffOld -= Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]) * -1;
                                }
                                else
                                {
                                    decExcRateDiffDebitOld = 0;
                                    decExcRateDiffCreditOld = Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]);
                                    decExcRateDiffOld += Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]);
                                }
                            }
                            if (DouPaymntAmt > 0)
                            {
                                if (mFormType == FORMTYPE.CP || mFormType == FORMTYPE.BP)
                                    DouPendAmt = Val.Val(DtabPaymentDetail.Rows[IntI]["PENDDEBITAMOUNTFE"]);
                                else
                                    DouPendAmt = Val.Val(DtabPaymentDetail.Rows[IntI]["PENDCREDITAMOUNTFE"]);
                                DouBankChrge = Val.Val(DtabPaymentDetail.Rows[IntI]["FEBANKCHARGES"]);

                                DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"] = Math.Round(DouPendAmt - (DouPaymntAmt + DouBankChrge), 2);
                            }
                        }
                        else
                            DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"] = "0";
                    }
                    else
                        DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"] = "0";

                    if (Val.Val(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]) != 0)
                    {
                        if (mFormType == FORMTYPE.CP || mFormType == FORMTYPE.BP)
                        {
                            if (decExcRateDiffDebitOld != 0)
                            {
                                var rows = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToDecimal(row["DEBITFE"]) == decExcRateDiffDebitOld).ToArray();
                                foreach (DataRow NewDr in rows)
                                {

                                    if (Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]) > 0)
                                    {
                                        NewDr["DEBITFE"] = DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"];
                                        decExcRateDiffNewCrd += Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]);
                                        NewDr["CREDITFE"] = "0";
                                    }
                                    else
                                    {
                                        decimal DecExcRate = Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]) * -1;
                                        NewDr["DEBITFE"] = "0";
                                        NewDr["CREDITFE"] = DecExcRate;
                                        decExcRateDiffNewDeb += DecExcRate;
                                    }

                                    NewDr["DEBIT"] = "0";
                                    NewDr["CREDIT"] = "0";
                                }
                            }
                            else if (decExcRateDiffCreditOld != 0)
                            {
                                var rows = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToDecimal(row["CREDITFE"]) == decExcRateDiffCreditOld).ToArray();
                                foreach (DataRow NewDr in rows)
                                {

                                    if (Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]) > 0)
                                    {
                                        NewDr["DEBITFE"] = DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"];
                                        decExcRateDiffNewCrd += Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]);
                                        NewDr["CREDITFE"] = "0";
                                    }
                                    else
                                    {
                                        decimal DecExcRate = Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]) * -1;
                                        NewDr["DEBITFE"] = "0";
                                        NewDr["CREDITFE"] = DecExcRate;
                                        decExcRateDiffNewDeb += DecExcRate;
                                    }
                                    NewDr["DEBIT"] = "0";
                                    NewDr["CREDIT"] = "0";
                                }
                            }
                            else
                            {
                                DataRow NewDr = DtabPaymentPickupBillDetail.NewRow();

                                NewDr["TRN_ID"] = GrdDetPayment.GetFocusedRowCellValue("TRN_ID");
                                NewDr["SRNO"] = GrdDetPayment.GetFocusedRowCellValue("SRNO");
                                NewDr["ACCLEDGTRNTRN_ID"] = GrdDetPayment.GetFocusedRowCellValue("ACCLEDGTRNTRN_ID");
                                NewDr["ACCLEDGTRNSRNO"] = GrdDetPayment.GetFocusedRowCellValue("ACCLEDGTRNSRNO");
                                NewDr["ENTRYTYPE"] = DtabPaymentDetail.Rows[IntI]["ENTRYTYPE"];
                                NewDr["BOOKTYPEFULL"] = DtabPaymentDetail.Rows[IntI]["BOOKTYPEFULL"];

                                NewDr["VOUCHERNOSTR"] = DtabPaymentDetail.Rows[IntI]["VOUCHERNOSTR"];
                                NewDr["VOUCHERDATE"] = DtabPaymentDetail.Rows[IntI]["VOUCHERDATE"];
                                NewDr["FINYEAR"] = DtabPaymentDetail.Rows[IntI]["FINYEAR"];
                                NewDr["LEDGER_ID"] = DtabPaymentDetail.Rows[IntI]["LEDGER_ID"];
                                NewDr["LEDGERNAME"] = DtabPaymentDetail.Rows[IntI]["LEDGERNAME"];

                                NewDr["CURRENCY_ID"] = DtabPaymentDetail.Rows[IntI]["CURRENCY_ID"];
                                NewDr["PENDDEBITAMOUNT"] = DtabPaymentDetail.Rows[IntI]["PENDDEBITAMOUNT"];
                                NewDr["PENDCREDITAMOUNT"] = DtabPaymentDetail.Rows[IntI]["PENDCREDITAMOUNT"];
                                NewDr["EXCRATE"] = DtabPaymentDetail.Rows[IntI]["EXCRATE"];
                                NewDr["PENDDEBITAMOUNTFE"] = DtabPaymentDetail.Rows[IntI]["PENDDEBITAMOUNTFE"];
                                NewDr["PENDCREDITAMOUNTFE"] = DtabPaymentDetail.Rows[IntI]["PENDCREDITAMOUNTFE"];
                                NewDr["BILL_NO"] = DtabPaymentDetail.Rows[IntI]["BILL_NO"];
                                NewDr["BILL_DT"] = DateTime.ParseExact(Val.ToString(DtabPaymentDetail.Rows[IntI]["BILL_DT"]), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd hh:mm:ss");//Val.SqlDate(Val.ToString( Dr["BILL_DT"]));

                                if (Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]) > 0)
                                {
                                    NewDr["DEBITFE"] = DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"];
                                    decExcRateDiffNewDeb += Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]);
                                    NewDr["CREDITFE"] = "0";
                                }
                                else
                                {
                                    decimal DecExcRate = Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]) * -1;
                                    NewDr["DEBITFE"] = "0";
                                    NewDr["CREDITFE"] = DecExcRate;
                                    decExcRateDiffNewCrd += DecExcRate;
                                }
                                NewDr["DEBIT"] = "0";
                                NewDr["CREDIT"] = "0";


                                NewDr["REFTRN_ID"] = DtabPaymentDetail.Rows[IntI]["TRN_ID"];
                                NewDr["REFSRNO"] = DtabPaymentDetail.Rows[IntI]["SRNO"];
                                NewDr["REFACCLEDGTRNTRN_ID"] = DtabPaymentDetail.Rows[IntI]["ACCLEDGTRNTRN_ID"];
                                NewDr["REFACCLEDGTRNSRNO"] = DtabPaymentDetail.Rows[IntI]["ACCLEDGTRNSRNO"];
                                NewDr["REFTYPE"] = "Agnst Ref";
                                NewDr["MainGrdRow_id"] = DtabPaymentDetail.Rows[IntI]["MainGrdRow_id"];
                                NewDr["REFLEDGER_ID"] = Val.ToString(DtExchRateDiff.Rows[0]["LEDGER_ID"]);

                                DtabPaymentPickupBillDetail.Rows.Add(NewDr);
                            }
                        }
                        else
                        {
                            if (decExcRateDiffDebitOld != 0)
                            {
                                var rows = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToDecimal(row["DEBITFE"]) == decExcRateDiffDebitOld).ToArray();
                                foreach (DataRow NewDr in rows)
                                {

                                    if (Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]) > 0)
                                    {
                                        NewDr["CREDITFE"] = DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"];
                                        decExcRateDiffNewDeb += Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]);
                                        NewDr["DEBITFE"] = "0";
                                    }
                                    else
                                    {
                                        decimal DecExcRate = Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]) * -1;
                                        NewDr["CREDITFE"] = "0";
                                        NewDr["DEBITFE"] = DecExcRate;
                                        decExcRateDiffNewCrd += DecExcRate;
                                    }

                                    NewDr["DEBIT"] = "0";
                                    NewDr["CREDIT"] = "0";
                                }
                            }
                            else if (decExcRateDiffCreditOld != 0)
                            {
                                var rows = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToDecimal(row["CREDITFE"]) == decExcRateDiffCreditOld).ToArray();
                                foreach (DataRow NewDr in rows)
                                {

                                    if (Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]) > 0)
                                    {
                                        NewDr["CREDITFE"] = DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"];
                                        decExcRateDiffNewDeb += Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]);
                                        NewDr["DEBITFE"] = "0";
                                    }
                                    else
                                    {
                                        decimal DecExcRate = Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]) * -1;
                                        NewDr["CREDITFE"] = "0";
                                        NewDr["DEBITFE"] = DecExcRate;
                                        decExcRateDiffNewCrd += DecExcRate;
                                    }
                                    NewDr["DEBIT"] = "0";
                                    NewDr["CREDIT"] = "0";
                                }
                            }
                            else
                            {
                                DataRow NewDr = DtabPaymentPickupBillDetail.NewRow();

                                NewDr["TRN_ID"] = GrdDetPayment.GetFocusedRowCellValue("TRN_ID");
                                NewDr["SRNO"] = GrdDetPayment.GetFocusedRowCellValue("SRNO");
                                NewDr["ACCLEDGTRNTRN_ID"] = GrdDetPayment.GetFocusedRowCellValue("ACCLEDGTRNTRN_ID");
                                NewDr["ACCLEDGTRNSRNO"] = GrdDetPayment.GetFocusedRowCellValue("ACCLEDGTRNSRNO");
                                NewDr["ENTRYTYPE"] = DtabPaymentDetail.Rows[IntI]["ENTRYTYPE"];
                                NewDr["BOOKTYPEFULL"] = DtabPaymentDetail.Rows[IntI]["BOOKTYPEFULL"];

                                NewDr["VOUCHERNOSTR"] = DtabPaymentDetail.Rows[IntI]["VOUCHERNOSTR"];
                                NewDr["VOUCHERDATE"] = DtabPaymentDetail.Rows[IntI]["VOUCHERDATE"];
                                NewDr["FINYEAR"] = DtabPaymentDetail.Rows[IntI]["FINYEAR"];
                                NewDr["LEDGER_ID"] = DtabPaymentDetail.Rows[IntI]["LEDGER_ID"];

                                NewDr["LEDGERNAME"] = DtabPaymentDetail.Rows[IntI]["LEDGERNAME"];

                                NewDr["CURRENCY_ID"] = DtabPaymentDetail.Rows[IntI]["CURRENCY_ID"];
                                NewDr["PENDDEBITAMOUNT"] = DtabPaymentDetail.Rows[IntI]["PENDDEBITAMOUNT"];
                                NewDr["PENDCREDITAMOUNT"] = DtabPaymentDetail.Rows[IntI]["PENDCREDITAMOUNT"];
                                NewDr["EXCRATE"] = DtabPaymentDetail.Rows[IntI]["EXCRATE"];
                                NewDr["PENDDEBITAMOUNTFE"] = DtabPaymentDetail.Rows[IntI]["PENDDEBITAMOUNTFE"];
                                NewDr["PENDCREDITAMOUNTFE"] = DtabPaymentDetail.Rows[IntI]["PENDCREDITAMOUNTFE"];
                                NewDr["BILL_NO"] = DtabPaymentDetail.Rows[IntI]["BILL_NO"];
                                NewDr["BILL_DT"] = DateTime.ParseExact(Val.ToString(DtabPaymentDetail.Rows[IntI]["BILL_DT"]), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd hh:mm:ss");//Val.SqlDate(Val.ToString( Dr["BILL_DT"]));

                                if (Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]) > 0)
                                {
                                    NewDr["CREDITFE"] = DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"];
                                    decExcRateDiffNewCrd += Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]);
                                    NewDr["DEBITFE"] = "0";
                                }
                                else
                                {
                                    decimal DecExcRate = Val.ToDecimal(DtabPaymentDetail.Rows[IntI]["EXCHRATEDIFF"]) * -1;
                                    NewDr["CREDITFE"] = "0";
                                    NewDr["DEBITFE"] = DecExcRate;
                                    decExcRateDiffNewDeb += DecExcRate;
                                }
                                NewDr["DEBIT"] = "0";
                                NewDr["CREDIT"] = "0";


                                NewDr["REFTRN_ID"] = DtabPaymentDetail.Rows[IntI]["TRN_ID"];
                                NewDr["REFSRNO"] = DtabPaymentDetail.Rows[IntI]["SRNO"];
                                NewDr["REFACCLEDGTRNTRN_ID"] = DtabPaymentDetail.Rows[IntI]["ACCLEDGTRNTRN_ID"];
                                NewDr["REFACCLEDGTRNSRNO"] = DtabPaymentDetail.Rows[IntI]["ACCLEDGTRNSRNO"];
                                NewDr["REFTYPE"] = "Agnst Ref";
                                NewDr["MainGrdRow_id"] = DtabPaymentDetail.Rows[IntI]["MainGrdRow_id"];
                                NewDr["REFLEDGER_ID"] = Val.ToString(DtExchRateDiff.Rows[0]["LEDGER_ID"]);

                                DtabPaymentPickupBillDetail.Rows.Add(NewDr);
                            }
                        }
                    }
                    else
                    {
                        if (decExcRateDiffDebitOld > 0)
                        {
                            var rows = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToDecimal(row["DEBITFE"]) == decExcRateDiffDebitOld).ToArray();
                            foreach (DataRow NewDr in rows)
                                DtabPaymentPickupBillDetail.Rows.Remove(NewDr);
                        }
                        else if (decExcRateDiffCreditOld > 0)
                        {
                            var rows = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToDecimal(row["CREDITFE"]) == decExcRateDiffCreditOld).ToArray();
                            foreach (DataRow NewDr in rows)
                                DtabPaymentPickupBillDetail.Rows.Remove(NewDr);
                        }
                    }
                }
                for (int IntI = 0; IntI < DtabPaymentSummry.Rows.Count; IntI++)
                {
                    if (RdoCurrency.SelectedIndex.ToString() == "1") //if (txtCurrency.Text == "USD")
                    {
                        if (Val.Val(DtabPaymentSummry.Rows[IntI]["DEBITAMOUNT"]) > 0 || Val.Val(DtabPaymentSummry.Rows[IntI]["CREDITAMOUNT"]) > 0)
                        {
                            DtabPaymentSummry.Rows[IntI]["DEBITAMOUNTFE"] = Math.Round(Val.Val(DtabPaymentSummry.Rows[IntI]["DEBITAMOUNT"]) * Val.Val(txtExcRate.Text),2);
                            DtabPaymentSummry.Rows[IntI]["CREDITAMOUNTFE"] = Math.Round(Val.Val(DtabPaymentSummry.Rows[IntI]["CREDITAMOUNT"]) * Val.Val(txtExcRate.Text),2);
                        }
                        else if ((Val.Val(DtabPaymentSummry.Rows[IntI]["DEBITAMOUNT"]) == 0 || Val.Val(DtabPaymentSummry.Rows[IntI]["CREDITAMOUNT"]) == 0) && (decExcRateDiffNewCrd > 0 || decExcRateDiffNewDeb > 0))
                        {
                            DtabPaymentSummry.Rows[IntI]["DEBITAMOUNTFE"] = decExcRateDiffNewDeb;
                            DtabPaymentSummry.Rows[IntI]["CREDITAMOUNTFE"] = decExcRateDiffNewCrd;
                            decExcRateDiffNewCrd = 0;
                            decExcRateDiffNewDeb = 0;
                        }
                        else if ((Val.Val(DtabPaymentSummry.Rows[IntI]["DEBITAMOUNT"]) == 0 || Val.Val(DtabPaymentSummry.Rows[IntI]["CREDITAMOUNT"]) == 0) && (decExcRateDiffNewCrd == 0 || decExcRateDiffNewDeb == 0))
                        {
                            DtabPaymentSummry.Rows.RemoveAt(IntI);
                            IntI--;
                        }
                        if (DtabPaymentSummry.Rows.Count - 1 == IntI && (decExcRateDiffNewCrd > 0 || decExcRateDiffNewDeb > 0))
                        {
                            DataRow Drow = DtabPaymentSummry.NewRow();
                            Drow["SRNO"] = DtabPaymentSummry.Rows.Count + 1;
                            Drow["DEBITAMOUNT"] = 0;
                            Drow["CREDITAMOUNT"] = 0;
                            Drow["DEBITAMOUNTFE"] = decExcRateDiffNewCrd;
                            Drow["CREDITAMOUNTFE"] = decExcRateDiffNewDeb;
                            Drow["LEDGERNAME"] = Val.ToString(DtExchRateDiff.Rows[0]["LEDGERNAME"]);
                            Drow["LEDGER_ID"] = Val.ToString(DtExchRateDiff.Rows[0]["LEDGER_ID"]);
                            Drow["REFTYPE"] = "New Ref";
                            Drow["REFLEDGER_ID"] = Val.ToString(GrdDetPayment.GetRowCellValue(0, "LEDGER_ID"));
                            Drow["EDTBL"] = "No";
                            DtabPaymentSummry.Rows.Add(Drow);
                        }

                    }

                    else
                    {
                        if (Val.Val(DtabPaymentSummry.Rows[IntI]["DEBITAMOUNT"]) > 0 || Val.Val(DtabPaymentSummry.Rows[IntI]["CREDITAMOUNT"]) > 0)
                        {
                            DtabPaymentSummry.Rows[IntI]["DEBITAMOUNT"] = Math.Round(Val.Val(DtabPaymentSummry.Rows[IntI]["DEBITAMOUNTFE"]) / Val.Val(txtExcRate.Text),2);
                            DtabPaymentSummry.Rows[IntI]["CREDITAMOUNT"] = Math.Round(Val.Val(DtabPaymentSummry.Rows[IntI]["CREDITAMOUNTFE"]) / Val.Val(txtExcRate.Text),2);
                        }
                    }
                }

                GrdDetail.RefreshData();
                GrdDetPayment.RefreshData();

            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message.ToString());
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                DataSet DSetSummary = objLedgerTrn.GetBillWisePaymentGetDataNew("SUMMARY", Val.SqlDate(DTPFromDate.Text), Val.SqlDate(DTPToDate.Text), "", Guid.Empty, Guid.Empty);

                if (DSetSummary.Tables.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("No Data Found.");
                    MainGrid.DataSource = null;
                    return;
                }

                if (DSetSummary.Tables[0].Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("No Data Found.");
                    MainGrid.DataSource = null;
                    return;
                }
                MainGrid.DataSource = DSetSummary.Tables[0];
                GrdDet.RefreshData();

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDet_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                    return;

                if (e.Clicks == 2)
                {
                    this.Cursor = Cursors.WaitCursor;

                    DataRow Drow = GrdDet.GetFocusedDataRow();
                    DataSet DSet = new DataSet();
                    
                    DSet = objLedgerTrn.GetBillWisePaymentGetDataNew("DETAIL", "", "", Val.ToString(Drow["BOOKTYPEFULL"]), Val.ToGuid(Drow["TRN_ID"]), Guid.Empty);
                    if (DSet.Tables.Count <= 0)
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("Some Thing Went Wrong");
                        MainGrid.DataSource = null;
                        return;
                    }
                    DSet.DataSetName = "DocumentElement";
                    if (DSet.Tables.Count == 3)
                    {
                        DtabPaymentPickupBillDetail = DSet.Tables[0];
                        DtabPaymentSummry = DSet.Tables[1];
                        DtabPaymentDetail = DSet.Tables[2];
                        LblMode.Text = "Edit Mode";

                        if (!DtabPaymentSummry.Columns.Contains("CLOSINGBALANCE"))
                        {
                            DtabPaymentSummry.Columns.Add("CLOSINGBALANCE");
                        }

                        for (int i = 0; i < DtabPaymentSummry.Rows.Count; i++)
                            DtabPaymentSummry.Rows[i]["CLOSINGBALANCE"] = Global.FindLedgerClosingStr(Val.ToString(DtabPaymentSummry.Rows[i]["LEDGER_ID"]).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(DtabPaymentSummry.Rows[i]["LEDGER_ID"])));

                        //Added by Daksha on 25/04/2023
                        if (LblMode.Text == "Edit Mode")
                        {
                            for (int j = 0; j < DtabPaymentSummry.Rows.Count; j++)
                            {
                                DtabPaymentSummry.Rows[j]["STATUS"] = "";
                            }
                        }
                        //End as Daksha


                        //lblBalance.Text = Global.FindLedgerClosingStr(Val.ToString(txtCashBankAC.Tag).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtCashBankAC.Tag))); //Comment by Daksha on 25/04/2023

                        if (Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH PAYMENT")
                            CmbPaymentType.SelectedIndex = 0;
                        else if (Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK PAYMENT")
                            CmbPaymentType.SelectedIndex = 1;
                        else if (Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH RECEIPT")
                            CmbPaymentType.SelectedIndex = 2;
                        else if (Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK RECEIPT")
                            CmbPaymentType.SelectedIndex = 3;

                        if (Val.ToInt32(Drow["CURRENCY_ID"]) == 1)
                        {
                            RdoCurrency.SelectedIndex = 1;
                        }
                        else
                        {
                            RdoCurrency.SelectedIndex = 0;
                        }

                        txtTrnID.Text = Val.ToString(Drow["TRN_ID"]);
                        DTPVoucherDate.Text = Val.ToString(Drow["VOUCHERDATE"]); //Uncomment by Daksha on 20/06/2023
                        //DTPVoucherDate.Text = Val.ToString(Drow["ENTRYDATE"]); //Comment by Daksha on 20/06/2023
                        txtVoucherNo.Text = Val.ToString(Drow["VOUCHERNO"]);
                        txtVoucherStr.Text = Val.ToString(Drow["VOUCHERNOSTR"]);
                        txtCurrency.Tag = Val.ToString(Drow["CURRENCY_ID"]);
                        txtCurrency.Text = Val.ToString(Drow["CURRENCYNAME"]);
                        txtExcRate.Text = Val.ToString(Drow["EXCRATE"]);
                        txtRemark.Text = Val.ToString(Drow["NOTE"]);
                        txtChqNo.Text = Val.ToString(Drow["CHQ_NO"]);
                        DtpChqIssue.Text = Val.ToString(Drow["CHQISSUEDT"]);
                        cmbPaymentMode.SelectedItem = Val.ToString(Drow["PAYTYPE"]);

                        //Added & Comment by Daksha on 25/04/2023
                        txtCashBankAC.Tag = Val.ToString(Drow["REFLEDGER_ID"]);
                        txtCashBankAC.Text = Val.ToString(Drow["REFLEDGERNAME"]);
                        //Old Code
                        //txtCashBankAC.Tag = Val.ToString(Drow["LEDGER_ID"]);
                        //txtCashBankAC.Text = Val.ToString(Drow["LEDGERNAME"]);
                        //End as Daksha


                        if (Val.ToInt32(Drow["CONVERTTOINR"]) == 1)
                            ChkBxConvertToInr.Checked = true;
                        else
                            ChkBxConvertToInr.Checked = false;

                        //Added & Comment by Daksha on 25/04/2023
                        if (Val.ToInt32(Drow["CURRENCY_ID"]) == 1)
                        {
                            if (Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH PAYMENT" || Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK PAYMENT")
                                txtAmount.Text = Val.ToString(Drow["DEBAMT"]);
                            else
                                txtAmount.Text = Val.ToString(Drow["CRDAMT"]);
                        }
                        else
                        {
                            if (Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH PAYMENT" || Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK PAYMENT")
                                txtAmount.Text = Val.ToString(Drow["DEBAMTFE"]);
                            else
                                txtAmount.Text = Val.ToString(Drow["CRDAMTFE"]);
                        }
                        //Old Code
                        //if (Val.ToInt32(Drow["CURRENCY_ID"]) == 1)
                        //{
                        //    if (Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH PAYMENT" || Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK PAYMENT")
                        //        txtAmount.Text = Val.ToString(Drow["CRDAMT"]);
                        //    else
                        //        txtAmount.Text = Val.ToString(Drow["DEBAMT"]);
                        //}
                        //else
                        //{
                        //    if (Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH PAYMENT" || Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK PAYMENT")
                        //        txtAmount.Text = Val.ToString(Drow["CRDAMTFE"]);
                        //    else
                        //        txtAmount.Text = Val.ToString(Drow["DEBAMTFE"]);
                        //}
                        //End as Daksha

                        if (Val.ToString(RdoCurrency.SelectedIndex.ToString()) == "1") //if (Val.ToString(txtCurrency.Text) == "USD")
                        { ChkBxConvertToInr.Visible = true; lblAmount.Text = "Amount ($)"; }
                        else
                        { ChkBxConvertToInr.Visible = false; lblAmount.Text = "Amount (₹)"; }

                        MainGrdPayment.DataSource = DtabPaymentSummry;
                        MainGrdPayment.Refresh();

                        MainGridDetail.DataSource = DtabPaymentPickupBillDetail;
                        MainGridDetail.Refresh();
                        xtraTabControl1.SelectedTabPageIndex = 0;

                        DataTable DtCheckAllocated = ObjFinance.CheckBillAllocatedOrNot(txtTrnID.Text, "");
                        if (DtCheckAllocated.Rows.Count > 0)
                        {
                            LblBillPickupMessage.Text = "This Entry Allocated In Accounts ( " + Val.ToString(DtCheckAllocated.Rows[0][0]) + " ) ,Please Delete For Edit.";
                            BtnSave.Enabled = false;
                            BtnDelete.Enabled = false;
                            LblMode.Text = "View Mode";
                        }
                        else
                        {
                            LblBillPickupMessage.Text = "";
                            BtnSave.Enabled = true;
                            BtnDelete.Enabled = true;
                            LblMode.Text = "Edit Mode";
                        }

                        //Added by Daksha on 25/04/2023
                        DTBal = new BusLib.Account.BOLedgerTransaction().FindLedgerClosingNew(Val.ToString(txtCashBankAC.Tag).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtCashBankAC.Tag)));
                        if (DTBal.Rows.Count > 0)
                        {
                            if (RdoCurrency.SelectedIndex.ToString() == "1") //if (Val.ToString(txtCurrency.Text) == "USD")
                            {
                                if (Val.ToDouble(DTBal.Rows[0]["CloseBalanceDollar"]) < 0)
                                {
                                    lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Dr";
                                }
                                else
                                {
                                    lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Cr";
                                }
                            }
                            else
                            {
                                if (Val.ToDouble(DTBal.Rows[0]["CloseBalanceRs"]) < 0)
                                {
                                    lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Dr";
                                }
                                else
                                {
                                    lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Cr";
                                }
                            }
                        }
                        //End as Daksha

                    }
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void respTxtLedgerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "LEDGERCODE,LEDGERNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;

                    string Str = Val.Left(Val.ToString(CmbPaymentType.SelectedItem), 2);
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LEDGER);
                    //for (int i = 0; i < DtabPaymentSummry.Rows.Count; i++)
                    //{
                    //    var VarPartyNameRows = FrmSearch.DTab.Rows.Cast<DataRow>().Where(row => Val.ToString(row["LEDGERNAME"]) == Val.ToString(DtabPaymentSummry.Rows[i]["LEDGERNAME"])).ToArray();
                    //    foreach (DataRow dr in VarPartyNameRows)
                    //        FrmSearch.DTab.Rows.Remove(dr);
                    //}
                    FrmSearch.mStrColumnsToHide = "LEDGER_ID,COMPANYNAME";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    GrdDetPayment.SetFocusedRowCellValue("LEDGERNAME", "");
                    GrdDetPayment.SetFocusedRowCellValue("LEDGER_ID", "00000000-0000-0000-0000-000000000000");
                    if (FrmSearch.DRow != null)
                    {
                        GrdDetPayment.SetFocusedRowCellValue("SRNO", GrdDetPayment.FocusedRowHandle + 1);
                        GrdDetPayment.SetFocusedRowCellValue("LEDGERNAME", Val.ToString(FrmSearch.DRow["LEDGERNAME"]));
                        GrdDetPayment.SetFocusedRowCellValue("LEDGER_ID", Val.ToString(FrmSearch.DRow["LEDGER_ID"]));
                        if (Val.Val(GrdDetPayment.GetFocusedRowCellValue("DEBITAMOUNT")) == 0 && Val.Val(GrdDetPayment.GetFocusedRowCellValue("CREDITAMOUNT")) == 0 &&
                            Val.Val(GrdDetPayment.GetFocusedRowCellValue("DEBITAMOUNTFE")) == 0 && Val.Val(GrdDetPayment.GetFocusedRowCellValue("CREDITAMOUNTFE")) == 0)
                        {
                            GrdDetPayment.SetFocusedRowCellValue("DEBITAMOUNT", 0.00);
                            GrdDetPayment.SetFocusedRowCellValue("CREDITAMOUNT", 0.00);
                            GrdDetPayment.SetFocusedRowCellValue("DEBITAMOUNTFE", 0.00);
                            GrdDetPayment.SetFocusedRowCellValue("CREDITAMOUNTFE", 0.00);
                            GrdDetPayment.SetFocusedRowCellValue("REFTYPE", "New Ref");
                            GrdDetPayment.SetFocusedRowCellValue("REFLEDGER_ID", Val.ToString(txtCashBankAC.Tag));
                            GrdDetPayment.SetFocusedRowCellValue("EDTBL", "Yes");
                        }
                        IntSumryCount++;

                        GrdDetPayment.SetFocusedRowCellValue("CLOSINGBALANCE", Global.FindLedgerClosingStr(Val.ToString(FrmSearch.DRow["LEDGER_ID"]).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(FrmSearch.DRow["LEDGER_ID"]))));

                        int intGrdIndex = GrdDetPayment.GetFocusedDataSourceRowIndex() + 1;
                        //Comment by Daksha on 26/04/2023
                        //var rows = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToInt32(row["MainGrdRow_id"]) == intGrdIndex).ToArray();
                        //foreach (DataRow dr in rows)
                        //    DtabPaymentPickupBillDetail.Rows.Remove(dr);
                        //End as Daksha
                        calculate();
                        MainGridDetail.RefreshDataSource();
                        IntAllowToCalculate = 0;
                    }
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnAddRow_Click(object sender, EventArgs e)
        {
            if (txtCashBankAC.Text != string.Empty)
            {
                DataRow drow = DtabPaymentSummry.NewRow();
                //if (DtabPaymentSummry.Columns.Count == 0)
                //    AddDtTblColumns();
                DtabPaymentSummry.Rows.Add(drow);
                MainGrdPayment.DataSource = DtabPaymentSummry;
                GrdDetPayment.FocusedRowHandle = drow.Table.Rows.IndexOf(drow);
                GrdDetPayment.FocusedColumn = GrdDetPayment.VisibleColumns[0];
                GrdDetPayment.Focus();
                GrdDetPayment.ShowEditor();
                calculate();
            }
            else
            {
                Global.MessageError("Cash/Bank Ledger Required");
                txtCashBankAC.Focus();
            }
        }

        //private void AddDtTblColumns()
        //{
        //    DtabPaymentSummry.Columns.Add("SRNO");
        //    DtabPaymentSummry.Columns.Add("MEMODATE");
        //    DtabPaymentSummry.Columns.Add("MEMO_ID");
        //    DtabPaymentSummry.Columns.Add("JANGEDNOSTR");
        //    DtabPaymentSummry.Columns.Add("INVCURRENCYNAME");
        //    DtabPaymentSummry.Columns.Add("NETAMOUNT");
        //    DtabPaymentSummry.Columns.Add("FNETAMOUNT");
        //    DtabPaymentSummry.Columns.Add("CURRENCY");
        //    DtabPaymentSummry.Columns.Add("CURRENCY_ID");
        //    DtabPaymentSummry.Columns.Add("PAYMENTEXCRATE");
        //    DtabPaymentSummry.Columns.Add("DEBITAMOUNT", typeof(decimal));
        //    DtabPaymentSummry.Columns.Add("CREDITAMOUNT", typeof(decimal));
        //    DtabPaymentSummry.Columns.Add("DEBITAMOUNTFE", typeof(decimal));
        //    DtabPaymentSummry.Columns.Add("CREDITAMOUNTFE", typeof(decimal));
        //    DtabPaymentSummry.Columns.Add("VOUCHERDATE");
        //    DtabPaymentSummry.Columns.Add("VOUCHERNO");
        //    DtabPaymentSummry.Columns.Add("VOUCHERNOSTR");
        //    DtabPaymentSummry.Columns.Add("BOOKTYPE");
        //    DtabPaymentSummry.Columns.Add("BOOKTYPEFULL");
        //    DtabPaymentSummry.Columns.Add("LEDGER_ID");
        //    DtabPaymentSummry.Columns.Add("REFLEDGER_ID");
        //    DtabPaymentSummry.Columns.Add("LEDGERNAME");
        //    DtabPaymentSummry.Columns.Add("BILLTYPE");
        //    DtabPaymentSummry.Columns.Add("REFTYPE");
        //    DtabPaymentSummry.Columns.Add("BILLALLOCATION");
        //    DtabPaymentSummry.Columns.Add("CLOSINGBALANCE");
        //    //DtabPaymentSummry.Columns.Add("CLOSINGBALANCE");
        //    DtabPaymentSummry.Columns.Add("EDTBL");
        //}

        //private void AddDtTblColumnsDetailPickupBill()
        //{
        //    DtabPaymentPickupBillDetail.Columns.Add("TRN_ID");
        //    DtabPaymentPickupBillDetail.Columns.Add("SRNO");
        //    DtabPaymentPickupBillDetail.Columns.Add("ACCLEDGTRNTRN_ID");
        //    DtabPaymentPickupBillDetail.Columns.Add("ACCLEDGTRNSRNO");
        //    DtabPaymentPickupBillDetail.Columns.Add("ENTRYTYPE");
        //    DtabPaymentPickupBillDetail.Columns.Add("BOOKTYPEFULL");

        //    DtabPaymentPickupBillDetail.Columns.Add("VOUCHERNOSTR");
        //    DtabPaymentPickupBillDetail.Columns.Add("VOUCHERDATE");
        //    DtabPaymentPickupBillDetail.Columns.Add("FINYEAR");
        //    DtabPaymentPickupBillDetail.Columns.Add("LEDGER_ID");
        //    DtabPaymentPickupBillDetail.Columns.Add("LEDGERNAME");

        //    DtabPaymentPickupBillDetail.Columns.Add("CURRENCY_ID");
        //    DtabPaymentPickupBillDetail.Columns.Add("CURRENCY");
        //    DtabPaymentPickupBillDetail.Columns.Add("PENDDEBITAMOUNT");
        //    DtabPaymentPickupBillDetail.Columns.Add("PENDCREDITAMOUNT");
        //    DtabPaymentPickupBillDetail.Columns.Add("EXCRATE");
        //    DtabPaymentPickupBillDetail.Columns.Add("PENDDEBITAMOUNTFE");
        //    DtabPaymentPickupBillDetail.Columns.Add("PENDCREDITAMOUNTFE");
        //    DtabPaymentPickupBillDetail.Columns.Add("BILL_NO");
        //    DtabPaymentPickupBillDetail.Columns.Add("BILL_DT");
        //    DtabPaymentPickupBillDetail.Columns.Add("DEBIT");
        //    DtabPaymentPickupBillDetail.Columns.Add("CREDIT");
        //    DtabPaymentPickupBillDetail.Columns.Add("DEBITFE");
        //    DtabPaymentPickupBillDetail.Columns.Add("CREDITFE");

        //    DtabPaymentPickupBillDetail.Columns.Add("REFTRN_ID");
        //    DtabPaymentPickupBillDetail.Columns.Add("REFSRNO");
        //    DtabPaymentPickupBillDetail.Columns.Add("REFACCLEDGTRNTRN_ID");
        //    DtabPaymentPickupBillDetail.Columns.Add("REFACCLEDGTRNSRNO");
        //    DtabPaymentPickupBillDetail.Columns.Add("REFTYPE");
        //    DtabPaymentPickupBillDetail.Columns.Add("REFLEDGER_ID");
        //    DtabPaymentPickupBillDetail.Columns.Add("MainGrdRow_id");
        //    DtabPaymentPickupBillDetail.Columns.Add("REFVOUCHERNOSTR");
        //    DtabPaymentPickupBillDetail.Columns.Add("REFBOOKTYPEFULL");
        //    DtabPaymentPickupBillDetail.Columns.Add("REFVOUCHERDATE");
        //}

        //private void AddDtTblColumnsDetail()
        //{
        //    DtabPaymentDetail.Columns.Add("TRN_ID");
        //    DtabPaymentDetail.Columns.Add("SRNO");
        //    DtabPaymentDetail.Columns.Add("ACCLEDGTRNTRN_ID");
        //    DtabPaymentDetail.Columns.Add("ACCLEDGTRNSRNO");
        //    DtabPaymentDetail.Columns.Add("ENTRYTYPE");
        //    DtabPaymentDetail.Columns.Add("BOOKTYPEFULL");

        //    DtabPaymentDetail.Columns.Add("VOUCHERNOSTR");
        //    DtabPaymentDetail.Columns.Add("VOUCHERDATE");
        //    DtabPaymentDetail.Columns.Add("FINYEAR");
        //    DtabPaymentDetail.Columns.Add("LEDGER_ID");
        //    DtabPaymentDetail.Columns.Add("LEDGERNAME");

        //    DtabPaymentDetail.Columns.Add("CURRENCY_ID");
        //    DtabPaymentDetail.Columns.Add("CURRENCY");
        //    DtabPaymentDetail.Columns.Add("PENDDEBITAMOUNT");
        //    DtabPaymentDetail.Columns.Add("PENDCREDITAMOUNT");
        //    DtabPaymentDetail.Columns.Add("EXCRATE");
        //    DtabPaymentDetail.Columns.Add("PENDDEBITAMOUNTFE");
        //    DtabPaymentDetail.Columns.Add("PENDCREDITAMOUNTFE");
        //    DtabPaymentDetail.Columns.Add("BILL_NO");
        //    DtabPaymentDetail.Columns.Add("BILL_DT");
        //    DtabPaymentDetail.Columns.Add("PAYMENTAMOUNT");
        //    DtabPaymentDetail.Columns.Add("FPAYMENTAMOUNT");

        //    DtabPaymentDetail.Columns.Add("REFTRN_ID");
        //    DtabPaymentDetail.Columns.Add("REFSRNO");
        //    DtabPaymentDetail.Columns.Add("REFACCLEDGTRNTRN_ID");
        //    DtabPaymentDetail.Columns.Add("REFACCLEDGTRNSRNO");
        //    DtabPaymentDetail.Columns.Add("REFTYPE");
        //    DtabPaymentDetail.Columns.Add("MainGrdRow_id");

        //    DtabPaymentDetail.Columns.Add("BANKCHARGES");
        //    DtabPaymentDetail.Columns.Add("FEBANKCHARGES");
        //    DtabPaymentDetail.Columns.Add("EXCHRATEDIFF");
        //    DtabPaymentDetail.Columns.Add("PAYMENTMADE");
        //}



        private void rspBtnBillAllocation_Click(object sender, EventArgs e)
        {
            try
            {
                if (RdoCurrency.SelectedIndex.ToString() != string.Empty) //if (txtCurrency.Text != string.Empty)
                {
                    if (Val.ToString(GrdDetPayment.GetFocusedRowCellValue("LEDGER_ID")) != "")
                    {
                        string StrEdit = Val.ToString(GrdDetPayment.GetFocusedRowCellValue("EDTBL"));
                        if (StrEdit != "No")
                        {
                            int intGrdIndex = GrdDetPayment.GetFocusedDataSourceRowIndex() + 1;
                            decimal decDebit = 0, decCredit = 0, decDebitFe = 0, decCreditFe = 0;
                            decimal decBankDebit = 0, decBankCredit = 0, decBankDebitFe = 0, decBankCreditFe = 0;
                            //Kuldeep 19012021
                            decimal decDiscountDebit = 0, decDiscountCredit = 0, decDiscountDebitFe = 0, decDiscountCreditFe = 0;
                            decimal decExcRateDiffDebit = 0, decExcRateDiffCredit = 0, decExcRateDiffDebitFe = 0, decExcRateDiffCreditFe = 0;
                            string Str = Val.Left(Val.ToString(CmbPaymentType.SelectedItem), 2);
                            FrmPickupBillAllocation frm = new FrmPickupBillAllocation();
                            frm.Ledger_Id = Guid.Parse(Val.ToString(GrdDetPayment.GetFocusedRowCellValue("LEDGER_ID")));
                            frm.TRN_ID = Val.ToString(txtTrnID.Text).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtTrnID.Text));
                            frm.FormType = Str;
                            frm.ExcRAte = Val.ToDecimal(txtExcRate.Text);
                            //kuldeep 05012021
                            frm.IntCurrency_ID = Val.ToInt32(RdoCurrency.SelectedIndex.ToString()); //Val.ToInt32(txtCurrency.Tag);
                            frm.DtAllocatedBill = DtabPaymentDetail;
                            frm.IntRowIndex = intGrdIndex;
                            frm.StrEntryDate = Val.SqlDate(DTPVoucherDate.Text);
                            frm.ShowDialog();
                            if (frm.FrmSubited == "Y")
                            {
                                //if (DtabPaymentDetail.Columns.Count == 0)
                                //    AddDtTblColumnsDetail();
                                //if (DtabPaymentPickupBillDetail.Columns.Count == 0)
                                //    AddDtTblColumnsDetailPickupBill();

                                var rows = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToInt32(row["MainGrdRow_id"]) == intGrdIndex).ToArray();
                                foreach (DataRow dr in rows)
                                    DtabPaymentPickupBillDetail.Rows.Remove(dr);

                                rows = DtabPaymentDetail.Rows.Cast<DataRow>().Where(row => Val.ToInt32(row["MainGrdRow_id"]) == intGrdIndex).ToArray();
                                foreach (DataRow dr in rows)
                                    DtabPaymentDetail.Rows.Remove(dr);

                                DataTable DtBankCharges = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "BANKCHARGES");
                                DataTable DtExchRateDiff = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "EXCRATEDIFF");
                                DataTable DtDiscount = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "DISCOUNT");
                                //Kuldeep 19012021
                                IntAllowToCalculate = 1;
                                foreach (DataRow Dr in frm.Allocation.Rows)
                                {
                                    DataRow NewDr = DtabPaymentDetail.NewRow();
                                    NewDr["TRN_ID"] = GrdDetPayment.GetFocusedRowCellValue("TRN_ID");
                                    NewDr["SRNO"] = GrdDetPayment.GetFocusedRowCellValue("SRNO");
                                    NewDr["ACCLEDGTRNTRN_ID"] = GrdDetPayment.GetFocusedRowCellValue("ACCLEDGTRNTRN_ID");
                                    NewDr["ACCLEDGTRNSRNO"] = GrdDetPayment.GetFocusedRowCellValue("ACCLEDGTRNSRNO");
                                    NewDr["ENTRYTYPE"] = Dr["ENTRYTYPE"];
                                    NewDr["BOOKTYPEFULL"] = Dr["BOOKTYPEFULL"];

                                    NewDr["VOUCHERNOSTR"] = Dr["VOUCHERNOSTR"];
                                    NewDr["VOUCHERDATE"] = Dr["VOUCHERDATE"];
                                    NewDr["FINYEAR"] = Dr["FINYEAR"];
                                    NewDr["LEDGER_ID"] = Dr["LEDGER_ID"];
                                    NewDr["CURRENCY_ID"] = Dr["CURRENCY_ID"];
                                    NewDr["PENDDEBITAMOUNT"] = Dr["PENDDEBITAMOUNT"];
                                    NewDr["PENDCREDITAMOUNT"] = Dr["PENDCREDITAMOUNT"];
                                    NewDr["EXCRATE"] = Dr["EXCRATE"];
                                    NewDr["PENDDEBITAMOUNTFE"] = Dr["PENDDEBITAMOUNTFE"];
                                    NewDr["PENDCREDITAMOUNTFE"] = Dr["PENDCREDITAMOUNTFE"];
                                    NewDr["BILL_NO"] = Dr["BILL_NO"];
                                    //NewDr["BILL_DT"] = Val.SqlDate(Val.ToString(Dr["BILL_DT"]));
                                    NewDr["BILL_DT"] = Dr["BILL_DT"];
                                    NewDr["PAYMENTAMOUNT"] = Dr["PAYMENTAMOUNT"];
                                    NewDr["FPAYMENTAMOUNT"] = Dr["FPAYMENTAMOUNT"];
                                    NewDr["REFTRN_ID"] = Dr["TRN_ID"];
                                    NewDr["REFSRNO"] = Dr["SRNO"];
                                    NewDr["REFACCLEDGTRNTRN_ID"] = Dr["ACCLEDGTRNTRN_ID"];
                                    NewDr["REFACCLEDGTRNSRNO"] = Dr["ACCLEDGTRNSRNO"];
                                    NewDr["REFTYPE"] = "Agnst Ref";
                                    NewDr["BANKCHARGES"] = Dr["BANKCHARGES"];
                                    NewDr["FEBANKCHARGES"] = Dr["FEBANKCHARGES"];
                                    NewDr["MainGrdRow_id"] = intGrdIndex;

                                    NewDr["EXCHRATEDIFF"] = Dr["EXCHRATEDIFF"];
                                    NewDr["PAYMENTMADE"] = Dr["PAYMENTMADE"];

                                    //Kuldeep 19012021
                                    NewDr["EXCHRATEDIFFUSD"] = Dr["EXCHRATEDIFFUSD"];
                                    NewDr["DISCOUNTAMOUNT"] = Dr["DISCOUNTAMOUNT"];
                                    NewDr["FDISCOUNTAMOUNT"] = Dr["FDISCOUNTAMOUNT"];

                                    DtabPaymentDetail.Rows.Add(NewDr);
                                }


                                foreach (DataRow Dr in frm.Allocation.Rows)
                                {
                                    DataRow NewDr = DtabPaymentPickupBillDetail.NewRow();

                                    NewDr["TRN_ID"] = GrdDetPayment.GetFocusedRowCellValue("TRN_ID");
                                    NewDr["SRNO"] = GrdDetPayment.GetFocusedRowCellValue("SRNO");
                                    NewDr["ACCLEDGTRNTRN_ID"] = GrdDetPayment.GetFocusedRowCellValue("ACCLEDGTRNTRN_ID");
                                    NewDr["ACCLEDGTRNSRNO"] = GrdDetPayment.GetFocusedRowCellValue("ACCLEDGTRNSRNO");
                                    NewDr["ENTRYTYPE"] = Dr["ENTRYTYPE"];
                                    NewDr["BOOKTYPEFULL"] = Dr["BOOKTYPEFULL"];

                                    NewDr["VOUCHERNOSTR"] = Dr["VOUCHERNOSTR"];
                                    NewDr["VOUCHERDATE"] = Dr["VOUCHERDATE"];
                                    NewDr["FINYEAR"] = Dr["FINYEAR"];
                                    NewDr["LEDGER_ID"] = Dr["LEDGER_ID"];
                                    NewDr["LEDGERNAME"] = Dr["LEDGERNAME"];

                                    NewDr["CURRENCY_ID"] = Dr["CURRENCY_ID"];
                                    NewDr["CURRENCY"] = Dr["CURRENCY"];
                                    NewDr["PENDDEBITAMOUNT"] = Dr["PENDDEBITAMOUNT"];
                                    NewDr["PENDCREDITAMOUNT"] = Dr["PENDCREDITAMOUNT"];
                                    NewDr["EXCRATE"] = Dr["EXCRATE"];
                                    NewDr["PENDDEBITAMOUNTFE"] = Dr["PENDDEBITAMOUNTFE"];
                                    NewDr["PENDCREDITAMOUNTFE"] = Dr["PENDCREDITAMOUNTFE"];
                                    NewDr["BILL_NO"] = Dr["BILL_NO"];
                                    //NewDr["BILL_DT"] = Val.SqlDate(Val.ToString(Dr["BILL_DT"]));
                                    NewDr["BILL_DT"] = Dr["BILL_DT"];

                                    if (Str == "CP" || Str == "BP")
                                    {
                                        NewDr["DEBIT"] = Dr["PAYMENTAMOUNT"];
                                        decDebit +=  Val.ToDecimal(Dr["PAYMENTAMOUNT"]);
                                        NewDr["CREDIT"] = "0";
                                        NewDr["DEBITFE"] =Math.Round( Val.ToDecimal(Dr["FPAYMENTAMOUNT"]),2);
                                        decDebitFe += Math.Round( Val.ToDecimal(Dr["FPAYMENTAMOUNT"]),2);
                                        NewDr["CREDITFE"] = "0";
                                    }
                                    else
                                    {
                                        NewDr["DEBIT"] = "0";
                                        NewDr["CREDIT"] = Dr["PAYMENTAMOUNT"];
                                        decCredit += Val.ToDecimal(Dr["PAYMENTAMOUNT"]);
                                        NewDr["DEBITFE"] = "0";
                                        NewDr["CREDITFE"] = Math.Round(Val.ToDecimal(Dr["FPAYMENTAMOUNT"]), 2);
                                        decCreditFe += Math.Round( Val.ToDecimal(Dr["FPAYMENTAMOUNT"]),2);
                                    }
                                    NewDr["REFTRN_ID"] = Dr["TRN_ID"];
                                    NewDr["REFSRNO"] = Dr["SRNO"];
                                    NewDr["REFACCLEDGTRNTRN_ID"] = Dr["ACCLEDGTRNTRN_ID"];
                                    NewDr["REFACCLEDGTRNSRNO"] = Dr["ACCLEDGTRNSRNO"];
                                    NewDr["REFTYPE"] = "Agnst Ref";
                                    NewDr["MainGrdRow_id"] = intGrdIndex;
                                    NewDr["REFLEDGER_ID"] = Val.ToString(txtCashBankAC.Tag);
                                    NewDr["REFBOOKTYPEFULL"] = Dr["BOOKTYPEFULL"];
                                    NewDr["REFVOUCHERNOSTR"] = Dr["VOUCHERNOSTR"];
                                    NewDr["REFVOUCHERDATE"] = Dr["VOUCHERDATE"];


                                    DtabPaymentPickupBillDetail.Rows.Add(NewDr);
                                    if (Val.Val(Dr["BANKCHARGES"]) > 0)
                                    {
                                        NewDr = DtabPaymentPickupBillDetail.NewRow();

                                        NewDr["TRN_ID"] = GrdDetPayment.GetFocusedRowCellValue("TRN_ID");
                                        NewDr["SRNO"] = GrdDetPayment.GetFocusedRowCellValue("SRNO");
                                        NewDr["ACCLEDGTRNTRN_ID"] = GrdDetPayment.GetFocusedRowCellValue("ACCLEDGTRNTRN_ID");
                                        NewDr["ACCLEDGTRNSRNO"] = GrdDetPayment.GetFocusedRowCellValue("ACCLEDGTRNSRNO");
                                        NewDr["ENTRYTYPE"] = Dr["ENTRYTYPE"];
                                        NewDr["BOOKTYPEFULL"] = Dr["BOOKTYPEFULL"];

                                        NewDr["VOUCHERNOSTR"] = Dr["VOUCHERNOSTR"];
                                        NewDr["VOUCHERDATE"] = Dr["VOUCHERDATE"];
                                        NewDr["FINYEAR"] = Dr["FINYEAR"];
                                        NewDr["LEDGER_ID"] = Dr["LEDGER_ID"];
                                        NewDr["LEDGERNAME"] = Dr["LEDGERNAME"];

                                        NewDr["CURRENCY_ID"] = Dr["CURRENCY_ID"];
                                        NewDr["CURRENCY"] = Dr["CURRENCY"];
                                        NewDr["PENDDEBITAMOUNT"] = Dr["PENDDEBITAMOUNT"];
                                        NewDr["PENDCREDITAMOUNT"] = Dr["PENDCREDITAMOUNT"];
                                        NewDr["EXCRATE"] = Dr["EXCRATE"];
                                        NewDr["PENDDEBITAMOUNTFE"] = Dr["PENDDEBITAMOUNTFE"];
                                        NewDr["PENDCREDITAMOUNTFE"] = Dr["PENDCREDITAMOUNTFE"];
                                        NewDr["BILL_NO"] = Dr["BILL_NO"];
                                        //NewDr["BILL_DT"] = Val.SqlDate(Val.ToString(Dr["BILL_DT"]));
                                        NewDr["BILL_DT"] = Dr["BILL_DT"];
                                        if (Str == "CP" || Str == "BP")
                                        {
                                            NewDr["DEBIT"] = Dr["BANKCHARGES"];
                                            decDebit += Val.ToDecimal(Dr["BANKCHARGES"]);
                                            decBankCredit += Val.ToDecimal(Dr["BANKCHARGES"]);
                                            NewDr["CREDIT"] = "0";
                                            NewDr["DEBITFE"] = Math.Round(  Val.ToDecimal(Dr["FEBANKCHARGES"]),2);
                                            decDebitFe +=Math.Round(  Val.ToDecimal(Dr["FEBANKCHARGES"]),2);
                                            decBankCreditFe +=Math.Round(  Val.ToDecimal(Dr["FEBANKCHARGES"]),2);
                                            NewDr["CREDITFE"] = "0";
                                        }
                                        else
                                        {
                                            NewDr["DEBIT"] = "0";
                                            NewDr["CREDIT"] = Dr["BANKCHARGES"];
                                            decCredit += Val.ToDecimal(Dr["BANKCHARGES"]);
                                            decBankDebit += Val.ToDecimal(Dr["BANKCHARGES"]);
                                            NewDr["DEBITFE"] = "0";
                                            NewDr["CREDITFE"] =Math.Round(   Val.ToDecimal(Dr["FEBANKCHARGES"]),2);
                                            decBankDebitFe +=Math.Round(  Val.ToDecimal(Dr["FEBANKCHARGES"]),2);
                                            decCreditFe +=Math.Round(  Val.ToDecimal(Dr["FEBANKCHARGES"]),2);
                                        }
                                        NewDr["REFTRN_ID"] = Dr["TRN_ID"];
                                        NewDr["REFSRNO"] = Dr["SRNO"];
                                        NewDr["REFACCLEDGTRNTRN_ID"] = Dr["ACCLEDGTRNTRN_ID"];
                                        NewDr["REFACCLEDGTRNSRNO"] = Dr["ACCLEDGTRNSRNO"];
                                        NewDr["REFTYPE"] = "Agnst Ref";
                                        NewDr["MainGrdRow_id"] = intGrdIndex;
                                        NewDr["REFLEDGER_ID"] = Val.ToString(DtBankCharges.Rows[0]["LEDGER_ID"]);
                                        //kuldeep 05012021
                                        NewDr["REFBOOKTYPEFULL"] = Dr["BOOKTYPEFULL"];
                                        NewDr["REFVOUCHERNOSTR"] = Dr["VOUCHERNOSTR"];
                                        NewDr["REFVOUCHERDATE"] = Dr["VOUCHERDATE"];
                                        DtabPaymentPickupBillDetail.Rows.Add(NewDr);
                                    }
                                    //Kuldeep 19012021
                                    if (Val.Val(Dr["DISCOUNTAMOUNT"]) > 0)
                                    {
                                        NewDr = DtabPaymentPickupBillDetail.NewRow();

                                        NewDr["TRN_ID"] = GrdDetPayment.GetFocusedRowCellValue("TRN_ID");
                                        NewDr["SRNO"] = GrdDetPayment.GetFocusedRowCellValue("SRNO");
                                        NewDr["ACCLEDGTRNTRN_ID"] = GrdDetPayment.GetFocusedRowCellValue("ACCLEDGTRNTRN_ID");
                                        NewDr["ACCLEDGTRNSRNO"] = GrdDetPayment.GetFocusedRowCellValue("ACCLEDGTRNSRNO");
                                        NewDr["ENTRYTYPE"] = Dr["ENTRYTYPE"];
                                        NewDr["BOOKTYPEFULL"] = Dr["BOOKTYPEFULL"];

                                        NewDr["VOUCHERNOSTR"] = Dr["VOUCHERNOSTR"];
                                        NewDr["VOUCHERDATE"] = Dr["VOUCHERDATE"];
                                        NewDr["FINYEAR"] = Dr["FINYEAR"];
                                        NewDr["LEDGER_ID"] = Dr["LEDGER_ID"];
                                        NewDr["LEDGERNAME"] = Dr["LEDGERNAME"];

                                        NewDr["CURRENCY_ID"] = Dr["CURRENCY_ID"];
                                        NewDr["CURRENCY"] = Dr["CURRENCY"];
                                        NewDr["PENDDEBITAMOUNT"] = Dr["PENDDEBITAMOUNT"];
                                        NewDr["PENDCREDITAMOUNT"] = Dr["PENDCREDITAMOUNT"];
                                        NewDr["EXCRATE"] = Dr["EXCRATE"];
                                        NewDr["PENDDEBITAMOUNTFE"] = Dr["PENDDEBITAMOUNTFE"];
                                        NewDr["PENDCREDITAMOUNTFE"] = Dr["PENDCREDITAMOUNTFE"];
                                        NewDr["BILL_NO"] = Dr["BILL_NO"];
                                        NewDr["BILL_DT"] = Dr["BILL_DT"];
                                        if (Str == "CP" || Str == "BP")
                                        {
                                            NewDr["DEBIT"] = Dr["DISCOUNTAMOUNT"];
                                            decDebit += Val.ToDecimal(Dr["DISCOUNTAMOUNT"]);
                                            decDiscountCredit += Val.ToDecimal(Dr["DISCOUNTAMOUNT"]);
                                            NewDr["CREDIT"] = "0";
                                            NewDr["DEBITFE"] = Math.Round(  Val.ToDecimal(Dr["FDISCOUNTAMOUNT"]),2);
                                            decDebitFe +=Math.Round(  Val.ToDecimal(Dr["FDISCOUNTAMOUNT"]),2);
                                            decDiscountCreditFe += Math.Round( Val.ToDecimal(Dr["FDISCOUNTAMOUNT"]),2);
                                            NewDr["CREDITFE"] = "0";
                                        }
                                        else
                                        {
                                            NewDr["DEBIT"] = "0";
                                            NewDr["CREDIT"] = Dr["DISCOUNTAMOUNT"];
                                            decCredit += Val.ToDecimal(Dr["DISCOUNTAMOUNT"]);
                                            decDiscountDebit += Val.ToDecimal(Dr["DISCOUNTAMOUNT"]);
                                            NewDr["DEBITFE"] = "0";
                                            NewDr["CREDITFE"] =Math.Round(  Val.ToDecimal(Dr["FDISCOUNTAMOUNT"]),2);
                                            decDiscountDebitFe +=Math.Round(  Val.ToDecimal(Dr["FDISCOUNTAMOUNT"]),2);
                                            decCreditFe += Math.Round( Val.ToDecimal(Dr["FDISCOUNTAMOUNT"]),2);
                                        }
                                        NewDr["REFTRN_ID"] = Dr["TRN_ID"];
                                        NewDr["REFSRNO"] = Dr["SRNO"];
                                        NewDr["REFACCLEDGTRNTRN_ID"] = Dr["ACCLEDGTRNTRN_ID"];
                                        NewDr["REFACCLEDGTRNSRNO"] = Dr["ACCLEDGTRNSRNO"];
                                        NewDr["REFTYPE"] = "Agnst Ref";
                                        NewDr["MainGrdRow_id"] = intGrdIndex;
                                        NewDr["REFLEDGER_ID"] = Val.ToString(DtBankCharges.Rows[0]["LEDGER_ID"]);
                                        NewDr["REFBOOKTYPEFULL"] = Dr["BOOKTYPEFULL"];
                                        NewDr["REFVOUCHERNOSTR"] = Dr["VOUCHERNOSTR"];
                                        NewDr["REFVOUCHERDATE"] = Dr["VOUCHERDATE"];
                                        DtabPaymentPickupBillDetail.Rows.Add(NewDr);
                                    }


                                    if (Val.Val(Dr["EXCHRATEDIFF"]) != 0)
                                    {
                                        NewDr = DtabPaymentPickupBillDetail.NewRow();

                                        NewDr["TRN_ID"] = GrdDetPayment.GetFocusedRowCellValue("TRN_ID");
                                        NewDr["SRNO"] = GrdDetPayment.GetFocusedRowCellValue("SRNO");
                                        NewDr["ACCLEDGTRNTRN_ID"] = GrdDetPayment.GetFocusedRowCellValue("ACCLEDGTRNTRN_ID");
                                        NewDr["ACCLEDGTRNSRNO"] = GrdDetPayment.GetFocusedRowCellValue("ACCLEDGTRNSRNO");
                                        NewDr["ENTRYTYPE"] = Dr["ENTRYTYPE"];
                                        NewDr["BOOKTYPEFULL"] = Dr["BOOKTYPEFULL"];

                                        NewDr["VOUCHERNOSTR"] = Dr["VOUCHERNOSTR"];
                                        NewDr["VOUCHERDATE"] = Dr["VOUCHERDATE"];
                                        NewDr["FINYEAR"] = Dr["FINYEAR"];
                                        NewDr["LEDGER_ID"] = Dr["LEDGER_ID"];
                                        NewDr["LEDGERNAME"] = Dr["LEDGERNAME"];

                                        NewDr["CURRENCY_ID"] = Dr["CURRENCY_ID"];
                                        NewDr["CURRENCY"] = Dr["CURRENCY"];
                                        NewDr["PENDDEBITAMOUNT"] = Dr["PENDDEBITAMOUNT"];
                                        NewDr["PENDCREDITAMOUNT"] = Dr["PENDCREDITAMOUNT"];
                                        NewDr["EXCRATE"] = Dr["EXCRATE"];
                                        NewDr["PENDDEBITAMOUNTFE"] = Dr["PENDDEBITAMOUNTFE"];
                                        NewDr["PENDCREDITAMOUNTFE"] = Dr["PENDCREDITAMOUNTFE"];
                                        NewDr["BILL_NO"] = Dr["BILL_NO"];
                                        //NewDr["BILL_DT"] = Val.SqlDate(Val.ToString(Dr["BILL_DT"]));
                                        NewDr["BILL_DT"] = Dr["BILL_DT"];
                                        if (Str == "CP" || Str == "BP")
                                        {
                                            if (Val.ToDecimal(Dr["EXCHRATEDIFF"]) > 0)
                                            {
                                                NewDr["DEBITFE"] =Math.Round(  Val.ToDecimal(Dr["EXCHRATEDIFF"]),2);
                                                decDebitFe +=Math.Round(  Val.ToDecimal(Dr["EXCHRATEDIFF"]),2);
                                                decExcRateDiffCreditFe +=Math.Round(  Val.ToDecimal(Dr["EXCHRATEDIFF"]),2);
                                                NewDr["CREDITFE"] = "0";
                                            }
                                            else
                                            {
                                                decimal DecExcRate =Math.Round(  Val.ToDecimal(Dr["EXCHRATEDIFF"]) * -1,2);
                                                NewDr["DEBITFE"] = "0";
                                                NewDr["CREDITFE"] = DecExcRate;
                                                decExcRateDiffDebitFe += DecExcRate;
                                                decCreditFe += DecExcRate;
                                            }
                                            NewDr["DEBIT"] = "0";
                                            NewDr["CREDIT"] = "0";
                                        }
                                        else
                                        {
                                            if (Val.ToDecimal(Dr["EXCHRATEDIFF"]) < 0)
                                            {
                                                decimal DecExcRate = Math.Round( Val.ToDecimal(Dr["EXCHRATEDIFF"]) * -1,2);
                                                NewDr["DEBITFE"] = DecExcRate;
                                                decDebitFe += DecExcRate;
                                                decExcRateDiffCreditFe += DecExcRate;
                                                NewDr["CREDITFE"] = "0";
                                            }
                                            else
                                            {
                                                NewDr["DEBITFE"] = "0";
                                                NewDr["CREDITFE"] =Math.Round(   Val.ToDecimal(Dr["EXCHRATEDIFF"]),2);
                                                decExcRateDiffDebitFe +=Math.Round(  Val.ToDecimal(Dr["EXCHRATEDIFF"]),2);
                                                decCreditFe +=Math.Round(  Val.ToDecimal(Dr["EXCHRATEDIFF"]),2);
                                            }
                                            NewDr["DEBIT"] = "0";
                                            NewDr["CREDIT"] = "0";
                                        }
                                        NewDr["REFTRN_ID"] = Dr["TRN_ID"];
                                        NewDr["REFSRNO"] = Dr["SRNO"];
                                        NewDr["REFACCLEDGTRNTRN_ID"] = Dr["ACCLEDGTRNTRN_ID"];
                                        NewDr["REFACCLEDGTRNSRNO"] = Dr["ACCLEDGTRNSRNO"];
                                        NewDr["REFTYPE"] = "Agnst Ref";
                                        NewDr["MainGrdRow_id"] = intGrdIndex;
                                        NewDr["REFLEDGER_ID"] = Val.ToString(DtExchRateDiff.Rows[0]["LEDGER_ID"]);
                                        //kuldeep 05012021
                                        NewDr["REFBOOKTYPEFULL"] = Dr["BOOKTYPEFULL"];
                                        NewDr["REFVOUCHERNOSTR"] = Dr["VOUCHERNOSTR"];
                                        NewDr["REFVOUCHERDATE"] = Dr["VOUCHERDATE"];
                                        DtabPaymentPickupBillDetail.Rows.Add(NewDr);
                                    }
                                    //kuldeep 19012021
                                    if (Val.Val(Dr["EXCHRATEDIFFUSD"]) != 0)
                                    {
                                        NewDr = DtabPaymentPickupBillDetail.NewRow();

                                        NewDr["TRN_ID"] = GrdDetPayment.GetFocusedRowCellValue("TRN_ID");
                                        NewDr["SRNO"] = GrdDetPayment.GetFocusedRowCellValue("SRNO");
                                        NewDr["ACCLEDGTRNTRN_ID"] = GrdDetPayment.GetFocusedRowCellValue("ACCLEDGTRNTRN_ID");
                                        NewDr["ACCLEDGTRNSRNO"] = GrdDetPayment.GetFocusedRowCellValue("ACCLEDGTRNSRNO");
                                        NewDr["ENTRYTYPE"] = Dr["ENTRYTYPE"];
                                        NewDr["BOOKTYPEFULL"] = Dr["BOOKTYPEFULL"];

                                        NewDr["VOUCHERNOSTR"] = Dr["VOUCHERNOSTR"];
                                        NewDr["VOUCHERDATE"] = Dr["VOUCHERDATE"];
                                        NewDr["FINYEAR"] = Dr["FINYEAR"];
                                        NewDr["LEDGER_ID"] = Dr["LEDGER_ID"];
                                        NewDr["LEDGERNAME"] = Dr["LEDGERNAME"];

                                        NewDr["CURRENCY_ID"] = Dr["CURRENCY_ID"];
                                        NewDr["CURRENCY"] = Dr["CURRENCY"];
                                        NewDr["PENDDEBITAMOUNT"] = Dr["PENDDEBITAMOUNT"];
                                        NewDr["PENDCREDITAMOUNT"] = Dr["PENDCREDITAMOUNT"];
                                        NewDr["EXCRATE"] = Dr["EXCRATE"];
                                        NewDr["PENDDEBITAMOUNTFE"] = Dr["PENDDEBITAMOUNTFE"];
                                        NewDr["PENDCREDITAMOUNTFE"] = Dr["PENDCREDITAMOUNTFE"];
                                        NewDr["BILL_NO"] = Dr["BILL_NO"];
                                        //NewDr["BILL_DT"] = Val.SqlDate(Val.ToString(Dr["BILL_DT"]));
                                        NewDr["BILL_DT"] = Dr["BILL_DT"];
                                        if (Str == "CP" || Str == "BP")
                                        {
                                            if (Val.ToDecimal(Dr["EXCHRATEDIFFUSD"]) > 0)
                                            {
                                                NewDr["DEBIT"] = Dr["EXCHRATEDIFFUSD"];
                                                decDebit += Val.ToDecimal(Dr["EXCHRATEDIFFUSD"]);
                                                decExcRateDiffCredit += Val.ToDecimal(Dr["EXCHRATEDIFFUSD"]);
                                                NewDr["CREDIT"] = "0";
                                            }
                                            else
                                            {
                                                decimal DecExcRate = Val.ToDecimal(Dr["EXCHRATEDIFFUSD"]) * -1;
                                                NewDr["DEBIT"] = "0";
                                                NewDr["CREDIT"] = DecExcRate;
                                                decExcRateDiffDebit += DecExcRate;
                                                decCredit += DecExcRate;
                                            }
                                            NewDr["DEBITFE"] = "0";
                                            NewDr["CREDITFE"] = "0";
                                        }
                                        else
                                        {
                                            if (Val.ToDecimal(Dr["EXCHRATEDIFFUSD"]) < 0)
                                            {
                                                decimal DecExcRate = Val.ToDecimal(Dr["EXCHRATEDIFFUSD"]) * -1;
                                                NewDr["DEBIT"] = DecExcRate;
                                                decDebit += DecExcRate;
                                                decExcRateDiffCredit += DecExcRate;
                                                NewDr["CREDIT"] = "0";
                                            }
                                            else
                                            {
                                                NewDr["DEBIT"] = "0";
                                                NewDr["CREDIT"] = Dr["EXCHRATEDIFFUSD"];
                                                decExcRateDiffDebit += Val.ToDecimal(Dr["EXCHRATEDIFFUSD"]);
                                                decCredit += Val.ToDecimal(Dr["EXCHRATEDIFFUSD"]);
                                            }
                                            NewDr["DEBITFE"] = "0";
                                            NewDr["CREDITFE"] = "0";
                                        }
                                        NewDr["REFTRN_ID"] = Dr["TRN_ID"];
                                        NewDr["REFSRNO"] = Dr["SRNO"];
                                        NewDr["REFACCLEDGTRNTRN_ID"] = Dr["ACCLEDGTRNTRN_ID"];
                                        NewDr["REFACCLEDGTRNSRNO"] = Dr["ACCLEDGTRNSRNO"];
                                        NewDr["REFTYPE"] = "Agnst Ref";
                                        NewDr["MainGrdRow_id"] = intGrdIndex;
                                        NewDr["REFLEDGER_ID"] = Val.ToString(DtExchRateDiff.Rows[0]["LEDGER_ID"]);
                                        //kuldeep 05012021
                                        NewDr["REFBOOKTYPEFULL"] = Dr["BOOKTYPEFULL"];
                                        NewDr["REFVOUCHERNOSTR"] = Dr["VOUCHERNOSTR"];
                                        NewDr["REFVOUCHERDATE"] = Dr["VOUCHERDATE"];
                                        DtabPaymentPickupBillDetail.Rows.Add(NewDr);
                                    }
                                }
                                //kuldeep 19012021
                                if (decDebit > decCredit)
                                {
                                    GrdDetPayment.SetFocusedRowCellValue("DEBITAMOUNT", decDebit - decCredit);
                                    GrdDetPayment.SetFocusedRowCellValue("CREDITAMOUNT", 0);
                                    GrdDetPayment.SetFocusedRowCellValue("DEBITAMOUNTFE", decDebitFe - decCreditFe);
                                    GrdDetPayment.SetFocusedRowCellValue("CREDITAMOUNTFE", 0);
                                    GrdDetPayment.SetFocusedRowCellValue("EDTBL", "Yes");
                                }
                                else
                                {
                                    GrdDetPayment.SetFocusedRowCellValue("DEBITAMOUNT", 0);
                                    GrdDetPayment.SetFocusedRowCellValue("CREDITAMOUNT", decCredit - decDebit);
                                    GrdDetPayment.SetFocusedRowCellValue("DEBITAMOUNTFE", 0);
                                    GrdDetPayment.SetFocusedRowCellValue("CREDITAMOUNTFE", decCreditFe - decDebitFe);
                                    GrdDetPayment.SetFocusedRowCellValue("EDTBL", "Yes");
                                }
                                if (decDebit != 0 || decDebitFe != 0 || decCredit != 0 || decCreditFe != 0)
                                    GrdDetPayment.SetFocusedRowCellValue("REFTYPE", "Agnst Ref");
                                else
                                    GrdDetPayment.SetFocusedRowCellValue("REFTYPE", "New Ref");

                                rows = DtabPaymentSummry.Rows.Cast<DataRow>().Where(row => Val.ToString(row["REFLEDGER_ID"]) == Val.ToString(GrdDetPayment.GetFocusedRowCellValue("LEDGER_ID"))).ToArray();
                                foreach (DataRow dr in rows)
                                    DtabPaymentSummry.Rows.Remove(dr);

                                if (decBankDebit > 0 || decBankCredit > 0)
                                {
                                    DataRow Drow = DtabPaymentSummry.NewRow();
                                    Drow["SRNO"] = DtabPaymentSummry.Rows.Count + 1;
                                    Drow["DEBITAMOUNT"] = decBankDebit;
                                    Drow["CREDITAMOUNT"] = decBankCredit;
                                    Drow["DEBITAMOUNTFE"] = decBankDebitFe;
                                    Drow["CREDITAMOUNTFE"] = decBankCreditFe;
                                    Drow["LEDGERNAME"] = Val.ToString(DtBankCharges.Rows[0]["LEDGERNAME"]);
                                    Drow["LEDGER_ID"] = Val.ToString(DtBankCharges.Rows[0]["LEDGER_ID"]);
                                    Drow["REFTYPE"] = "New Ref";
                                    Drow["REFLEDGER_ID"] = Val.ToString(GrdDetPayment.GetFocusedRowCellValue("LEDGER_ID"));
                                    Drow["EDTBL"] = "No";
                                    DtabPaymentSummry.Rows.Add(Drow);
                                }
                                //Kuldeep 19012021
                                if (decDiscountDebit > 0 || decDiscountCredit > 0)
                                {
                                    DataRow Drow = DtabPaymentSummry.NewRow();
                                    Drow["SRNO"] = DtabPaymentSummry.Rows.Count + 1;
                                    Drow["DEBITAMOUNT"] = decDiscountDebit;
                                    Drow["CREDITAMOUNT"] = decDiscountCredit;
                                    Drow["DEBITAMOUNTFE"] = decDiscountDebitFe;
                                    Drow["CREDITAMOUNTFE"] = decDiscountCreditFe;
                                    Drow["LEDGERNAME"] = Val.ToString(DtDiscount.Rows[0]["LEDGERNAME"]);
                                    Drow["LEDGER_ID"] = Val.ToString(DtDiscount.Rows[0]["LEDGER_ID"]);
                                    Drow["REFTYPE"] = "New Ref";
                                    Drow["REFLEDGER_ID"] = Val.ToString(GrdDetPayment.GetFocusedRowCellValue("LEDGER_ID"));
                                    Drow["EDTBL"] = "No";
                                    DtabPaymentSummry.Rows.Add(Drow);
                                }
                                if (decExcRateDiffDebitFe > 0 || decExcRateDiffCreditFe > 0)
                                {
                                    DataRow Drow = DtabPaymentSummry.NewRow();
                                    Drow["SRNO"] = DtabPaymentSummry.Rows.Count + 1;
                                    Drow["DEBITAMOUNT"] = 0;
                                    Drow["CREDITAMOUNT"] = 0;
                                    Drow["DEBITAMOUNTFE"] = decExcRateDiffDebitFe;
                                    Drow["CREDITAMOUNTFE"] = decExcRateDiffCreditFe;
                                    Drow["LEDGERNAME"] = Val.ToString(DtExchRateDiff.Rows[0]["LEDGERNAME"]);
                                    Drow["LEDGER_ID"] = Val.ToString(DtExchRateDiff.Rows[0]["LEDGER_ID"]);
                                    Drow["REFTYPE"] = "New Ref";
                                    Drow["REFLEDGER_ID"] = Val.ToString(GrdDetPayment.GetFocusedRowCellValue("LEDGER_ID"));
                                    Drow["EDTBL"] = "No";
                                    DtabPaymentSummry.Rows.Add(Drow);
                                }
                                //Kuldeep 19012021
                                if (decExcRateDiffDebit > 0 || decExcRateDiffCredit > 0)
                                {
                                    DataRow Drow = DtabPaymentSummry.NewRow();
                                    Drow["SRNO"] = DtabPaymentSummry.Rows.Count + 1;
                                    Drow["DEBITAMOUNT"] = decExcRateDiffDebit;
                                    Drow["CREDITAMOUNT"] = decExcRateDiffCredit;
                                    Drow["DEBITAMOUNTFE"] = 0;
                                    Drow["CREDITAMOUNTFE"] = 0;
                                    Drow["LEDGERNAME"] = Val.ToString(DtExchRateDiff.Rows[0]["LEDGERNAME"]);
                                    Drow["LEDGER_ID"] = Val.ToString(DtExchRateDiff.Rows[0]["LEDGER_ID"]);
                                    Drow["REFTYPE"] = "New Ref";
                                    Drow["REFLEDGER_ID"] = Val.ToString(GrdDetPayment.GetFocusedRowCellValue("LEDGER_ID"));
                                    Drow["EDTBL"] = "No";
                                    DtabPaymentSummry.Rows.Add(Drow);
                                }
                                MainGridDetail.DataSource = DtabPaymentPickupBillDetail;
                                MainGridDetail.RefreshDataSource();
                                calculate();

                                MainGrid.RefreshDataSource();
                            }
                        }
                        else
                        {
                            Global.MessageError("Auto Generated Row Cant Be Allocated");
                            GrdDet.FocusedRowHandle = GrdDet.GetFocusedDataSourceRowIndex();
                            GrdDet.FocusedColumn = GrdDet.VisibleColumns[GrdDet.Columns["LEDGERNAME"].VisibleIndex];
                            GrdDet.ShowEditor();
                        }
                    }
                    else
                    {
                        Global.MessageError("Party Is Required");
                        GrdDet.FocusedRowHandle = GrdDet.GetFocusedDataSourceRowIndex();
                        GrdDet.FocusedColumn = GrdDet.VisibleColumns[GrdDet.Columns["LEDGERNAME"].VisibleIndex];
                        GrdDet.ShowEditor();
                    }
                }
                else
                {
                    Global.MessageError("Currency Is Required");
                    txtCurrency.Focus();
                }
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message.ToString());
            }
        }

        private void GrdDetPayment_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView view = (GridView)sender;
            string strRefType = Val.ToString(view.GetFocusedRowCellValue("REFTYPE"));
            string strColName = Val.ToString(view.FocusedColumn.FieldName);
            string StrEdit = Val.ToString(view.GetFocusedRowCellValue("EDTBL"));
            if ((strColName == "DEBITAMOUNT" || strColName == "CREDITAMOUNT" || strColName == "DEBITAMOUNTFE" || strColName == "CREDITAMOUNTFE") && (strRefType == "Agnst Ref" || StrEdit == "No"))
            {
                e.Cancel = true;
            }
            else if (Val.ToInt32(RdoCurrency.SelectedIndex.ToString()) == 1 && (strColName == "DEBITAMOUNTFE" || strColName == "CREDITAMOUNTFE") && strRefType == "New Ref")
                e.Cancel = true;
            else if (Val.ToInt32(RdoCurrency.SelectedIndex.ToString()) == 0 && (strColName == "DEBITAMOUNT" || strColName == "CREDITAMOUNT") && strRefType == "New Ref")
                e.Cancel = true;
            //else if (Val.ToInt32(txtCurrency.Tag) == 1 && (strColName == "DEBITAMOUNTFE" || strColName == "CREDITAMOUNTFE") && strRefType == "New Ref")
            //    e.Cancel = true;
            //else if (Val.ToInt32(txtCurrency.Tag) != 1 && (strColName == "DEBITAMOUNT" || strColName == "CREDITAMOUNT") && strRefType == "New Ref")
            //    e.Cancel = true;
        }

        private void cmbPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPaymentMode.Text == "Cash")
            {
                lblChqIssue.Text = "Cash Date";
                lblChqNo.Text = "Cash No.";
            }
            else if (cmbPaymentMode.Text == "Cheque")
            {
                lblChqIssue.Text = "Chq Issue";
                lblChqNo.Text = "Chq No.";
            }
            else if (cmbPaymentMode.Text == "IMPS")
            {
                lblChqIssue.Text = "IMPS Date";
                lblChqNo.Text = "IMPS No.";
            }
            else if (cmbPaymentMode.Text == "RTGS")
            {
                lblChqIssue.Text = "RTGS Date";
                lblChqNo.Text = "RTGS No.";
            }
            else if (cmbPaymentMode.Text == "NEFT")
            {
                lblChqIssue.Text = "NETF Date";
                lblChqNo.Text = "NETF No.";
            }
            else if (cmbPaymentMode.Text == "Others")
            {
                lblChqIssue.Text = "Others Date";
                lblChqNo.Text = "Others No.";
            }
        }

        private void GrdDetPayment_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int IntBillCurId;
                decimal DecAmt, DecExcRate;
                if (e.RowHandle < 0)
                    return;
                //Kuldeep 19012021
                if (IntAllowToCalculate != 0)
                    return;
                if (txtExcRate.Text == string.Empty || txtExcRate.Text == "")
                    txtExcRate.Text = "1";

                string Str = Val.Left(Val.ToString(CmbPaymentType.SelectedItem), 2);
                switch (Val.ToString(e.Column.FieldName))
                {
                    case "DEBITAMOUNT":
                        if (RdoCurrency.SelectedIndex.ToString() == "1") //if (txtCurrency.Text == "USD")
                        {
                            GrdDetPayment.PostEditor();
                            IntBillCurId = Val.ToInt32(GrdDetPayment.GetFocusedRowCellValue("CURRENCY_ID"));
                            DecExcRate = Val.ToDecimal(txtExcRate.Text);
                            DataRow Dr = GrdDetPayment.GetDataRow(e.RowHandle);

                            DecAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("DEBITAMOUNT"));

                            GrdDetPayment.SetFocusedRowCellValue("DEBITAMOUNTFE", Math.Round(DecExcRate * DecAmt,2));
                        }
                        break;

                    case "CREDITAMOUNT":
                        if (RdoCurrency.SelectedIndex.ToString() == "1") //if (txtCurrency.Text == "USD")
                        {
                            GrdDetPayment.PostEditor();
                            DecExcRate = Val.ToDecimal(txtExcRate.Text);

                            DataRow Dr = GrdDetPayment.GetDataRow(e.RowHandle);
                            DecAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("CREDITAMOUNT"));
                            GrdDetPayment.SetFocusedRowCellValue("CREDITAMOUNTFE", Math.Round(DecExcRate * DecAmt,2));
                        }
                        break;
                    case "DEBITAMOUNTFE":
                        if (RdoCurrency.SelectedIndex.ToString() != "1") //if (txtCurrency.Text != "USD")
                        {
                            GrdDetPayment.PostEditor();
                            DecExcRate = Val.ToDecimal(txtExcRate.Text);

                            DataRow Dr = GrdDetPayment.GetDataRow(e.RowHandle);
                            DecAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("DEBITAMOUNTFE"));

                            GrdDetPayment.SetFocusedRowCellValue("DEBITAMOUNT", Math.Round(DecAmt / DecExcRate, 2));
                        }
                        break;
                    case "CREDITAMOUNTFE":
                        if (RdoCurrency.SelectedIndex.ToString() != "1") //if (txtCurrency.Text != "USD")
                        {
                            GrdDetPayment.PostEditor();
                            DecExcRate = Val.ToDecimal(txtExcRate.Text);

                            DataRow Dr = GrdDetPayment.GetDataRow(e.RowHandle);
                            DecAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("CREDITAMOUNTFE"));

                            GrdDetPayment.SetFocusedRowCellValue("CREDITAMOUNT", Math.Round(DecAmt / DecExcRate, 2));
                        }
                        break;
                }
                calculate();
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message.ToString());
            }
        }

        void calculate()
        {
            decimal decTotalDeb, decTotalCrd;

            if (RdoCurrency.SelectedIndex.ToString() == "1") //if (txtCurrency.Text == "USD")
            {
                decTotalDeb = Val.ToDecimal(DtabPaymentSummry.Compute("SUM(DEBITAMOUNT)", string.Empty));
                decTotalCrd = Val.ToDecimal(DtabPaymentSummry.Compute("SUM(CREDITAMOUNT)", string.Empty));
                string Str = Val.Left(Val.ToString(CmbPaymentType.SelectedItem), 2);
                if (Str == "CP" || Str == "BP")
                {
                    txtAmount.Text = Val.ToString(decTotalDeb - decTotalCrd);
                }
                else
                {
                    txtAmount.Text = Val.ToString(decTotalCrd - decTotalDeb);
                }
            }
            else
            {
                decTotalDeb = Val.ToDecimal(DtabPaymentSummry.Compute("SUM(DEBITAMOUNTFE)", string.Empty));
                decTotalCrd = Val.ToDecimal(DtabPaymentSummry.Compute("SUM(CREDITAMOUNTFE)", string.Empty));
                string Str = Val.Left(Val.ToString(CmbPaymentType.SelectedItem), 2);
                if (Str == "CP" || Str == "BP")
                {
                    txtAmount.Text = Val.ToString(decTotalDeb - decTotalCrd);
                }
                else
                {
                    txtAmount.Text = Val.ToString(decTotalCrd - decTotalDeb);
                }
            }

        }
        //Kuldeep 19012021
        private void respTxtLedgerName_Validating(object sender, CancelEventArgs e)
        {
            IntAllowToCalculate = 0;
        }

        private void DTPEntryDate_Validated(object sender, EventArgs e)
        {
            //Added by Daksha on 20/06/2023
            for (int i = DtabPaymentPickupBillDetail.Rows.Count - 1; i >= 0; i--)
            {
                DataRow dr = DtabPaymentPickupBillDetail.Rows[i];
                DtabPaymentPickupBillDetail.Rows.Remove(dr);
            }

            //Old Code //Throws Exception
            //foreach (DataRow dr in DtabPaymentPickupBillDetail.Rows)
            //    DtabPaymentPickupBillDetail.Rows.Remove(dr);
            //End as Daksha
            
            calculate();
            MainGridDetail.RefreshDataSource();
           // CheckEntryDateCurrentDate(); 
        }

        void CheckEntryDateCurrentDate()
        {
            DataTable DtabFrmToDate = objLedgerTrn.GetFromToDateYear();
            if (DtabFrmToDate != null)
            {
                if (DtabFrmToDate.Rows.Count > 0)
                {
                    DateTime DatFromDate = DateTime.Parse(Val.ToString(DtabFrmToDate.Rows[0]["FROMDATE"]));
                    DateTime DatToDate = DateTime.Parse(Val.ToString(DtabFrmToDate.Rows[0]["TODATE"]));
                    if (DTPVoucherDate.Value < DatFromDate || DTPVoucherDate.Value > DatToDate)
                    {
                        if (DateTime.Now.Date > DatToDate)
                            DTPVoucherDate.Value = DatToDate;
                        else
                            DTPVoucherDate.Value = DateTime.Now.Date;
                    }
                }
            }
        }

        private void DTPEntryDate_CloseUp(object sender, EventArgs e)
        {
            DtabPaymentPickupBillDetail.Rows.Clear();
            MainGridDetail.RefreshDataSource();
            for (int i = 0; i < DtabPaymentSummry.Rows.Count; i++)
            {
                DtabPaymentSummry.Rows[i]["REFTYPE"] = "New Ref";
            }
            DtabPaymentDetail.Rows.Clear();
            CheckEntryDateCurrentDate();
        }

        private void RdoCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Val.ToString(RdoCurrency.SelectedIndex.ToString()) == "1")
                {
                    ChkBxConvertToInr.Visible = true; 
                    lblAmount.Text = "Amount ($)";
                    GrdDetPayment.Columns["DEBITAMOUNT"].Visible = true;
                    GrdDetPayment.Columns["CREDITAMOUNT"].Visible = true;
                    GrdDetPayment.Columns["DEBITAMOUNT"].VisibleIndex = 3;
                    GrdDetPayment.Columns["CREDITAMOUNT"].VisibleIndex = 4;
                    GrdDetPayment.Columns["DEBITAMOUNTFE"].Visible = false;
                    GrdDetPayment.Columns["CREDITAMOUNTFE"].Visible = false;
                    GrdDetail.Columns["CREDIT"].Visible = true;
                    GrdDetail.Columns["DEBIT"].Visible = true;
                    GrdDetail.Columns["DEBITFE"].Visible = false;
                    GrdDetail.Columns["CREDITFE"].Visible = false;
                }
                else
                {
                    ChkBxConvertToInr.Visible = false; 
                    lblAmount.Text = "Amount (₹)";
                    GrdDetPayment.Columns["DEBITAMOUNTFE"].Visible = true;
                    GrdDetPayment.Columns["CREDITAMOUNTFE"].Visible = true;
                    GrdDetPayment.Columns["DEBITAMOUNTFE"].VisibleIndex = 3;
                    GrdDetPayment.Columns["CREDITAMOUNTFE"].VisibleIndex = 4;
                    GrdDetPayment.Columns["DEBITAMOUNT"].Visible = false;
                    GrdDetPayment.Columns["CREDITAMOUNT"].Visible = false;
                    GrdDetail.Columns["CREDIT"].Visible = false;
                    GrdDetail.Columns["DEBIT"].Visible = false;
                    GrdDetail.Columns["DEBITFE"].Visible = true;
                    GrdDetail.Columns["CREDITFE"].Visible = true;
                }

                txtExcRate.Text = new BOTRN_MemoEntry().GetExchangeRate(Val.ToInt(RdoCurrency.SelectedIndex.ToString()), Val.SqlDate(DTPVoucherDate.Value.ToShortDateString()), mFormType.ToString(),"").ToString();               
                txtExcRate_Validated(null, null);
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        //Added by Daksha on 20/06/2023
        private void FrmBillWiseEntryNew_Load(object sender, EventArgs e)
        {
            try
            {
            DataTable dt = objLedgerTrn.GetFromToDateYear();
            if (dt.Rows.Count > 0)
            {
                DTPFromDate.Text = Val.ToString(dt.Rows[0][0]);
                DTPToDate.Text = Val.ToString(dt.Rows[0][1]);
            }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
        //End as Daksha
    }
}
