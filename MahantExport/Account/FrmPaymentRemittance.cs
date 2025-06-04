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
    public partial class FrmPaymentRemittance : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_Ledger ObjMast = new BOMST_Ledger();
        BOFormPer ObjPer = new BOFormPer();
        BOLedgerTransaction objLedgerTrn = new BOLedgerTransaction();
        BOACC_FinanceJournalEntry ObjFinance = new BOACC_FinanceJournalEntry();
        FrmPickupBillAllocation frm = new FrmPickupBillAllocation();

        DataTable DtabPaymentSummry = new DataTable();
        DataTable DtabPaymentDetail = new DataTable();
        DataTable DtabPaymentPickupBillDetail = new DataTable();
        DataTable DTBal = new DataTable();
        public FORMTYPE mFormType = FORMTYPE.CP;
        DataTable DTCASH = new DataTable();
        DataTable DTBANK = new DataTable();
        Boolean AUTOSAVE = false;
        
        //Kuldeep 19012021
        int IntAllowToCalculate = 0;
        Guid NewId;
        DataTable TempDataTable = new DataTable();
        //bool EditModeDateChange = false;

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

        public FrmPaymentRemittance()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            this.Show();

            DTPEntryDate.Value = DateTime.Now;
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
            DtabPaymentPickupBillDetail.Columns.Add("ENTRYDATE", typeof(string));
            BtnClear_Click(null, null);
            GrdDetPayment.BestFitColumns();
            CmbPaymentType_SelectedIndexChanged(null, null);
        }
        
        public void ShowForm(string pStrBookTypeFull, string pStrTran_id)
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            
            this.Show();

            DTPEntryDate.Value = DateTime.Now;
            //DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            //DTPToDate.Value = DateTime.Now;

            BtnClear_Click(null, null);


            DataSet DSet = new DataSet();
            DSet = objLedgerTrn.GetBillWisePaymentGetDataNew("DETAIL", "", "", pStrBookTypeFull, Guid.Parse(pStrTran_id), Guid.Empty);
            if (DSet.Tables.Count <= 0)
            {
                this.Cursor = Cursors.Default;
                Global.Message("Some Thing Went Wrong");
                //MainGrid.DataSource = null;
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
                    DtabPaymentSummry.Rows[i]["STATUS"] = "1";
                }

                for (int i = 0; i < DtabPaymentPickupBillDetail.Rows.Count; i++)
                {
                    DtabPaymentPickupBillDetail.Rows[i]["STATUS"] = "1";
                }

                //lblBalance.Text = Global.FindLedgerClosingStr(Val.ToString(txtCashBankAC.Tag).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtCashBankAC.Tag)));

                //Added & Comment by Daksha on 21/09/2023
                //Old Code
                //if ((Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "CASH PAYMENT") || (Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "BANK PAYMENT"))
                //    CmbPaymentType.SelectedIndex = 0;
                //    RdoPayType.SelectedIndex = 0;

                //if ((Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "CASH RECEIPT") || (Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "BANK RECEIPT"))
                //    CmbPaymentType.SelectedIndex = 1;
                //    RdoPayType.SelectedIndex = 1;

                if (BOConfiguration.gStrLoginSection != "B")
                {                    
                    RdoPayType.SelectedIndex = Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "BANK PAYMENT" ? 0 : 1; //0-Payment 1-Receive
                }
                else
                {
                    if ((Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "CASH PAYMENT") || (Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "BANK PAYMENT"))
                    {
                        CmbPaymentType.SelectedIndex = 0;
                        RdoPayType.SelectedIndex = 0;
                    }
                    else if ((Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "CASH RECEIPT") || (Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "BANK RECEIPT"))
                    {
                        CmbPaymentType.SelectedIndex = 1;
                        RdoPayType.SelectedIndex = 1;
                    }
                }

                if (Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "BANK RECEIPT")
                {
                    CmbPaymentType.Text = "BR - BANK RECEIVE";
                }
                else if (Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "CASH RECEIPT")
                {
                    CmbPaymentType.Text = "CR - CASH RECEIVE";
                }
                else if (Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "BANK PAYMENT")
                {
                    CmbPaymentType.Text = "BP - BANK PAYMENT";
                }
                else if (Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "CASH PAYMENT")
                {
                    CmbPaymentType.Text = "CP - CASH PAYMENT";
                }

                if (Val.ToInt32(DSet.Tables[0].Rows[0]["CURRENCY_ID"]) == 1)
                {
                    RdoCurrency.SelectedIndex = 1;
                }
                else
                {
                    RdoCurrency.SelectedIndex = 0;
                }
                //End as Daksha

                txtTrnID.Text = Val.ToString(DSet.Tables[0].Rows[0]["TRN_ID"]);
                DTPEntryDate.Text = Val.ToString(DSet.Tables[0].Rows[0]["VOUCHERDATE"]);
                txtVoucherNo.Text = Val.ToString(DSet.Tables[0].Rows[0]["VOUCHERNO"]);
                txtVoucherStr.Text = Val.ToString(DSet.Tables[0].Rows[0]["VOUCHERNOSTR"]);
                txtCurrency.Tag = Val.ToString(DSet.Tables[0].Rows[0]["CURRENCY_ID"]);
                txtCurrency.Text = Val.ToString(DSet.Tables[0].Rows[0]["CURRENCY"]);
                txtExcRate.Text = Val.ToString(DSet.Tables[0].Rows[0]["EXCRATE"]);
                txtRemark.Text = Val.ToString(DSet.Tables[0].Rows[0]["NOTE"]);
                txtChqNo.Text = Val.ToString(DSet.Tables[0].Rows[0]["CHQ_NO"]);
                DtpChqIssue.Text = Val.ToString(DSet.Tables[0].Rows[0]["CHQISSUEDT"]);
                cmbPaymentMode.SelectedItem = Val.ToString(DSet.Tables[0].Rows[0]["PAYTYPE"]);
                
                //Added & Comment by Daksha on 21/09/2023
                txtCashBankAC.Tag = Val.ToString(DSet.Tables[0].Rows[0]["REFLEDGER_ID"]);
                txtCashBankAC.Text = Val.ToString(DSet.Tables[0].Rows[0]["REFLEDGERNAME"]);
                //Old Code
                //txtCashBankAC.Tag = Val.ToString(DSet.Tables[0].Rows[0]["LEDGER_ID"]);
                //txtCashBankAC.Text = Val.ToString(DSet.Tables[0].Rows[0]["LEDGERNAME"]);
                //End as Daksha

                if (Val.ToInt32(DSet.Tables[0].Rows[0]["CONVERTTOINR"]) == 1)
                    ChkBxConvertToInr.Checked = true;
                else
                    ChkBxConvertToInr.Checked = false;

                //Added by Daksha on 13/09/2023
                if (Val.ToInt32(DSet.Tables[0].Rows[0]["CURRENCY_ID"]) == 1)
                {
                    if (Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "CASH PAYMENT" || Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "BANK PAYMENT")
                        txtAmount.Text = Val.ToString(DtabPaymentSummry.Compute("SUM(DEBITAMOUNT)", ""));
                    else
                        txtAmount.Text = Val.ToString(DtabPaymentSummry.Compute("SUM(CREDITAMOUNT)", ""));
                }
                else
                {
                    if (Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "CASH PAYMENT" || Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "BANK PAYMENT")
                        txtAmount.Text = Val.ToString(DtabPaymentSummry.Compute("SUM(DEBITAMOUNTFE)", ""));
                    else
                        txtAmount.Text = Val.ToString(DtabPaymentSummry.Compute("SUM(CREDITAMOUNTFE)", ""));
                }
                //End as Daksha
                //Old Code
                //if (Val.ToInt32(DSet.Tables[0].Rows[0]["CURRENCY_ID"]) == 1)
                //{
                //    if (Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "CASH PAYMENT" || Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "BANK PAYMENT")
                //        txtAmount.Text = Val.ToString(DSet.Tables[0].Rows[0]["CREDIT"]);
                //    else
                //        txtAmount.Text = Val.ToString(DSet.Tables[0].Rows[0]["DEBIT"]);
                //}
                //else
                //{
                //    if (Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "CASH PAYMENT" || Val.ToString(DSet.Tables[0].Rows[0]["BOOKTYPEFULL"]) == "BANK PAYMENT")
                //        txtAmount.Text = Val.ToString(DSet.Tables[0].Rows[0]["CREDITFE"]);
                //    else
                //        txtAmount.Text = Val.ToString(DSet.Tables[0].Rows[0]["DEBITFE"]);
                //}

                if (Val.ToString(RdoCurrency.SelectedIndex.ToString()) == "1")
                { ChkBxConvertToInr.Visible = true; lblAmount.Text = "Amount ($)"; }
                else
                { ChkBxConvertToInr.Visible = false; lblAmount.Text = "Amount (₹)"; }

                MainGrdPayment.DataSource = DtabPaymentSummry;
                MainGrdPayment.Refresh();

                MainGridDetail.DataSource = DtabPaymentPickupBillDetail;
                MainGridDetail.Refresh();
                //xtraTabControl1.SelectedTabPageIndex = 0;

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
                DtabPaymentPickupBillDetail.Columns.Add("ENTRYDATE", typeof(string));
                RdoPayType.SelectedIndex = 0;
                RdoCurrency.SelectedIndex = 0;
            }
            //for (int i = 0; i < DtabPaymentSummry.Rows.Count; i++)
            //    DTBal = new BusLib.Account.BOLedgerTransaction().FindLedgerClosingNew(Val.ToString(DtabPaymentSummry.Rows[i]["LEDGER_ID"]).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(DtabPaymentSummry.Rows[i]["LEDGER_ID"])));

            DTBal = new BusLib.Account.BOLedgerTransaction().FindLedgerClosingNew(Val.ToString(txtCashBankAC.Tag).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtCashBankAC.Tag)));
            if (DTBal.Rows.Count > 0)
            {
                if (RdoCurrency.SelectedIndex.ToString() == "1")
                {
                    if (Val.ToDouble(DTBal.Rows[0]["CloseBalanceDollar"]) < 0)
                    {
                        lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Dr";
                    }
                    else
                    {
                        lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Cr";
                    }
                    //lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]);
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
                    //lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceRs"]); }
                }
            }
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

            //if (Val.ISDate(DTPEntryDate.Text) == false)
            //{
            //    Global.Message("Entry Date Is Required");
            //    DTPEntryDate.Focus();
            //    return false;
            //}
            if (Val.ISDate(GrdDetPayment.GetFocusedRowCellValue("VOUCHERDATE")) == false)
            {
                Global.Message("Entry Date Is Required");
                //DTPEntryDate.Focus();
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
            //DataRow DR = GrdDetPayment.GetFocusedDataRow();
            //if ((Val.ToDouble(DR["DEBITAMOUNT"]) == 0 && Val.ToDouble(DR["CREDITAMOUNT"]) == 0 && Val.ToDouble(DR["DEBITAMOUNTFE"]) == 0 && Val.ToDouble(DR["CREDITAMOUNTFE"]) == 0) && Val.ToString(DR["LEDGERNAME"]) != "")
            //{
            //    Global.MessageError("Amount Has Not Been Entered");
            //    intValid = 1;
            //}
            //else
            //{
            //    DouDebAmt = Val.ToDouble(DR["DEBITAMOUNT"]);
            //    DouCrdAmt = Math.Round(Val.ToDouble(DR["CREDITAMOUNT"]), 3);
            //    DouDebAmtFe = Val.ToDouble(DR["DEBITAMOUNTFE"]);
            //    DouCrdAmtFe = Val.ToDouble(DR["CREDITAMOUNTFE"]);
            //}
            for (int IntI = 0; IntI < GrdDetPayment.RowCount; IntI++)
            {
                DataRow DR = GrdDetPayment.GetDataRow(IntI);
                if ((Val.ToDouble(DR["DEBITAMOUNT"]) == 0 && Val.ToDouble(DR["CREDITAMOUNT"]) == 0 && Val.ToDouble(DR["DEBITAMOUNTFE"]) == 0 && Val.ToDouble(DR["CREDITAMOUNTFE"]) == 0) && Val.ToString(DR["LEDGERNAME"]) != "")
                {
                    Global.MessageError("Amount Has Not Been Entered For SrNo. : " + Val.ToString((IntI + 1)));
                    intValid = 1;
                    break;
                }
                else
                {

                    DouDebAmt += Val.ToDouble(DR["DEBITAMOUNT"]);
                    DouCrdAmt += Math.Round(Val.ToDouble(DR["CREDITAMOUNT"]), 3, MidpointRounding.AwayFromZero);
                    DouDebAmtFe += Val.ToDouble(DR["DEBITAMOUNTFE"]);
                    DouCrdAmtFe += Val.ToDouble(DR["CREDITAMOUNTFE"]);

                    NewDouDebAmt = Math.Round(DouDebAmt, 3);
                    NewDouCrdAmt = Math.Round(DouCrdAmt,3);
                    NewDouDebAmtFe = Math.Round(DouDebAmtFe,3);
                    NewDouCrdAmtFe = Math.Round(DouCrdAmtFe,3);
                    
                    //DouDebAmt += Math.Round(Val.ToDouble(DR["DEBITAMOUNT"]), 3, MidpointRounding.AwayFromZero);
                    //DouCrdAmt += Math.Round(Val.ToDouble(DR["CREDITAMOUNT"]), 3, MidpointRounding.AwayFromZero);
                    //DouDebAmtFe += Math.Round(Val.ToDouble(DR["DEBITAMOUNTFE"]), 3, MidpointRounding.AwayFromZero);
                    //DouCrdAmtFe += Math.Round(Val.ToDouble(DR["CREDITAMOUNTFE"]), 3, MidpointRounding.AwayFromZero);
                }
            }

            if (intValid == 1)
                return false;

            if (mFormType == FORMTYPE.CP || mFormType == FORMTYPE.BP)
            {
                if (NewDouDebAmt == 0 || NewDouDebAmt < DouCrdAmt)
                {
                    Global.MessageError("Please Pass Proper Entry");
                    return false;
                }
            }
            else
            {
                if (NewDouCrdAmt == 0 || NewDouCrdAmt < DouDebAmt)
                {
                    Global.MessageError("Please Pass Proper Entry");
                    return false;
                }
            }


            if (Val.ToInt32(Val.ToString(RdoCurrency.SelectedIndex.ToString())) == 1)
            {
                if (mFormType == FORMTYPE.CP || mFormType == FORMTYPE.BP)
                {
                    NewDouCrdAmt += Val.Val(txtAmount.Text);
                    //DouCrdAmt = Val.Val(txtAmount.Text);
                    if (BOConfiguration.gStrLoginSection == "B")
                    {
                        //DouCrdAmtFe += Math.Round((Val.ToDouble(txtAmount.Text) * Val.ToDouble(txtExcRate.Text)) / 1000, 2);
                        NewDouCrdAmtFe += Math.Round(Val.ToDouble(txtAmount.Text) * Val.ToDouble(txtExcRate.Text), 3);
                        //DouCrdAmtFe = Math.Round(Val.ToDouble(txtAmount.Text) * Val.ToDouble(txtExcRate.Text), 3);
                    }
                    else
                    {
                        NewDouCrdAmtFe += Math.Round(Val.ToDouble(txtAmount.Text) * Val.ToDouble(txtExcRate.Text), 3);
                        //DouCrdAmtFe = Math.Round(Val.ToDouble(txtAmount.Text) * Val.ToDouble(txtExcRate.Text), 2);
                    }

                    if ((Val.ToDouble(NewDouCrdAmtFe) != Val.ToDouble(NewDouDebAmtFe)) || (Val.ToDouble(NewDouCrdAmt) != Val.ToDouble(NewDouDebAmt)))
                    {
                        Global.MessageError("Please Debit And Credit Side Not Matching");
                        return false;
                    }
                }
                else
                {
                    NewDouDebAmt += Val.Val(txtAmount.Text);
                    //DouDebAmt = Val.Val(txtAmount.Text);
                    if (BOConfiguration.gStrLoginSection == "B")
                    {
                        //DouDebAmtFe += Math.Round((Val.ToDouble(txtAmount.Text) * Val.ToDouble(txtExcRate.Text)) / 1000, 2);
                        NewDouDebAmtFe += Math.Round(Val.ToDouble(txtAmount.Text) * Val.ToDouble(txtExcRate.Text), 3);
                        //DouDebAmtFe = Math.Round(Val.ToDouble(txtAmount.Text) * Val.ToDouble(txtExcRate.Text), 3);
                    }
                    else
                    {
                        NewDouDebAmtFe += Math.Round(Val.ToDouble(txtAmount.Text) * Val.ToDouble(txtExcRate.Text), 3);
                        //DouDebAmtFe = Math.Round(Val.ToDouble(txtAmount.Text) * Val.ToDouble(txtExcRate.Text), 2);
                    }

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
                        //DouCrdAmt += Math.Round((Val.ToDouble(txtAmount.Text) * 1000) / Val.ToDouble(txtExcRate.Text), 2);
                        NewDouCrdAmt += Math.Round(Val.ToDouble(txtAmount.Text) / Val.ToDouble(txtExcRate.Text), 3);
                        //DouCrdAmt = Math.Round(Val.ToDouble(txtAmount.Text) / Val.ToDouble(txtExcRate.Text), 3);
                    }
                    else
                    {
                        NewDouCrdAmt += Math.Round(Val.ToDouble(txtAmount.Text) / Val.ToDouble(txtExcRate.Text), 3);
                        //DouCrdAmt = Math.Round(Val.ToDouble(txtAmount.Text) / Val.ToDouble(txtExcRate.Text), 2);
                    }
                    NewDouCrdAmtFe += Val.Val(txtAmount.Text);
                    //DouCrdAmtFe = Val.Val(txtAmount.Text);

                    if ((Val.ToDouble(NewDouCrdAmtFe) != Val.ToDouble(NewDouDebAmtFe)) || (Val.ToDouble(NewDouCrdAmt) != Val.ToDouble(NewDouDebAmt)))
                    {
                        Global.MessageError("Please Debit And Credit Side Not Matching");
                        return false;
                    }
                }
                else
                {
                    if (BOConfiguration.gStrLoginSection == "B")
                    {
                        //DouDebAmt += Math.Round((Val.ToDouble(txtAmount.Text) * 1000) / Val.ToDouble(txtExcRate.Text), 2);
                        NewDouDebAmt += Math.Round(Val.ToDouble(txtAmount.Text) / Val.ToDouble(txtExcRate.Text), 3);
                        //DouDebAmt = Math.Round(Val.ToDouble(txtAmount.Text) / Val.ToDouble(txtExcRate.Text), 3);
                    }
                    else
                    {
                        NewDouDebAmt += Math.Round(Val.ToDouble(txtAmount.Text) / Val.ToDouble(txtExcRate.Text), 3);
                        //DouDebAmt = Math.Round(Val.ToDouble(txtAmount.Text) / Val.ToDouble(txtExcRate.Text), 2);
                    }
                    NewDouDebAmtFe += Val.Val(txtAmount.Text);
                    //DouDebAmtFe = Val.Val(txtAmount.Text);

                    if ((NewDouCrdAmtFe != NewDouDebAmtFe) || (NewDouCrdAmt != NewDouDebAmt))
                    {
                        Global.MessageError("Please Debit And Credit Side Not Matching");
                        return false;
                    }
                }
            }
            if (RdoCurrency.SelectedIndex == 1)
            {
                if (BOConfiguration.gStrLoginSection != "B")
                {
                    if (Val.Val(txtExcRate.Text) < 50)
                    {
                        Global.Message("Please Enter Proper Exchange Rate");
                        txtCurrency.Focus();
                        return false;
                    }
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
                    FrmSearch.Width = 700;
                    FrmSearch.Height = 500;
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
                        //Add shiv 21-11-22
                        BtnAddRow_Click(null, null);
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;

                    //lblBalance.Text = Global.FindLedgerClosingStr(Val.ToString(txtCashBankAC.Tag).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtCashBankAC.Tag)));
                    DTBal = new BusLib.Account.BOLedgerTransaction().FindLedgerClosingNew(Val.ToString(txtCashBankAC.Tag).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtCashBankAC.Tag)));
                    if (DTBal.Rows.Count > 0)
                    {
                        if (RdoCurrency.SelectedIndex.ToString() == "1")
                        {
                            if (Val.ToDouble(DTBal.Rows[0]["CloseBalanceDollar"]) < 0)
                            {
                                lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Dr";
                            }
                            else
                            {
                                lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Cr";
                            }
                            //lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]);
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
                            //lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceRs"]); }
                        }
                    }
                    
                    //BtnAddRow_Click(null, null);
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
                if (AUTOSAVE == true)
                {
                    return;
                }
                if (ValSave() == false)
                {
                    return;
                }
                if (BOConfiguration.gStrLoginSection != "B")
                {
                    if (Global.Confirm("Are You Sure For Payment Entry") == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                }

                TRN_LedgerTranJournalProperty AccoutProperty = new TRN_LedgerTranJournalProperty();


                //DataRow drow = GrdDetPayment.GetFocusedDataRow();
                //if (Val.ToString(drow["TRN_ID"]) == String.Empty)
                //{
                if (BOConfiguration.gStrLoginSection != "B")
                {
                    var DRows = DtabPaymentSummry.Rows.Cast<DataRow>().Where(row => Val.ToString(row["SRNO"]) == "").ToArray();
                    foreach (DataRow dr in DRows)
                        DtabPaymentSummry.Rows.Remove(dr);
                }

                for (int i = 0; i < DtabPaymentSummry.Rows.Count; i++)
                {
                    if (DtabPaymentSummry.Rows[i]["STATUS"].ToString() == "")
                    {
                        //NewId = Guid.NewGuid();
                        //GrdDetPayment.SetFocusedRowCellValue("TRN_ID", NewId);

                        if (Val.ToGuid(DtabPaymentSummry.Rows[i]["ACCLEDGTRNTRN_ID"])   == Guid.Empty)
                        {
                            NewId = Guid.NewGuid();
                        }
                        else
                        {
                            NewId = Val.ToGuid(DtabPaymentSummry.Rows[i]["ACCLEDGTRNTRN_ID"]);
                        }
                        
                        //AccoutProperty.TRN_ID = Val.ToString(txtTrnID.Text).Trim().Equals(string.Empty) ? NewId : Guid.Parse(Val.ToString(txtTrnID.Text));
                        AccoutProperty.TRN_ID = NewId;
                        DtabPaymentSummry.Rows[i]["TRN_ID"] = AccoutProperty.TRN_ID;
                        int SRNOFORDTL = Val.ToInt32(DtabPaymentSummry.Rows[i]["SRNO"]);
                        AccoutProperty.ACCLEDGTRNTRN_ID = Val.ToString("00000000-0000-0000-0000-000000000000");
                        AccoutProperty.ACCLEDGTRNSRNO = 1;

                        //AccoutProperty.VOUCHERDATE = Val.SqlDate(DTPEntryDate.Text);
                        //AccoutProperty.VOUCHERDATE = Val.SqlDate(Val.ToString(GrdDetPayment.GetFocusedRowCellValue("VOUCHERDATE")));
                        AccoutProperty.VOUCHERDATE = Val.SqlDate(Val.ToString(DtabPaymentSummry.Rows[i]["VOUCHERDATE"]));
                        AccoutProperty.FINYEAR = Config.FINYEARNAME;
                        AccoutProperty.VOUCHERNO = Val.ToInt32(txtVoucherNo.Text);
                        AccoutProperty.VOUCHERSTR = txtVoucherStr.Text;
                        AccoutProperty.CURRENCY_ID = Val.ToInt32(RdoCurrency.SelectedIndex.ToString());
                        AccoutProperty.EXCRATE = Val.Val(txtExcRate.Text);
                        AccoutProperty.MEMO_ID = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        AccoutProperty.REFTRN_ID = Guid.Parse("00000000-0000-0000-0000-000000000000");
                        AccoutProperty.REFSRNO = 0;
                        AccoutProperty.BILL_NO = "";
                        AccoutProperty.NOTE = txtRemark.Text;
                        AccoutProperty.BILL_DT = "01/01/1900";
                        AccoutProperty.EXCRATEDIFF = 0;
                        AccoutProperty.TERMSDATE = Val.SqlDate(Val.ToString(DtabPaymentSummry.Rows[i]["VOUCHERDATE"]));
                        //AccoutProperty.CHQ_NO = txtChqNo.Text;
                        AccoutProperty.CHQ_NO = Val.ToString(GrdDetPayment.GetFocusedRowCellValue("Chq_no"));
                        //AccoutProperty.CHQISSUEDT = Val.SqlDate(DtpChqIssue.Text);
                        AccoutProperty.CHQISSUEDT = Val.SqlDate(Val.ToString(DtabPaymentSummry.Rows[i]["CHQISSUEDT"]));
                        //AccoutProperty.CHQCLEARDT = "01/01/1900";
                        AccoutProperty.DATAFREEZ = 0;
                        AccoutProperty.PAYTYPE = Val.ToString(cmbPaymentMode.SelectedItem);
                        AccoutProperty.REFTYPE = "";
                        AccoutProperty.PAYTERMS = 0;
                        AccoutProperty.REFBOOKTYPEFULL = "";
                        AccoutProperty.FROMLEDGER_ID = Guid.Parse(Val.ToString(txtCashBankAC.Tag));
                        AccoutProperty.REFLEDGER_ID = Guid.Parse(Val.ToString(DtabPaymentSummry.Rows[i]["LEDGER_ID"]));
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

                            //NewDr["BILL_DT"] = Val.ToString(DTPEntryDate.Value);// DateTime.ParseExact(Val.ToString(DTPEntryDate.Value), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd hh:mm:ss");// Val.SqlDate(DTPEntryDate.Text);
                            NewDr["BILL_DT"] = Dr["ENTRYDATE"];// DateTime.ParseExact(Val.ToString(DTPEntryDate.Value), "dd/MM/yyyy hh:mm:ss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd hh:mm:ss");// Val.SqlDate(DTPEntryDate.Text);

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
                            NewDr["NOTE"] = Dr["NOTE"];
                            NewDr["MainGrdRow_id"] = IntCnt;
                            DtabPaymentPickupBillDetail.Rows.Add(NewDr);
                            IntCnt++;

                        }
                        AccoutProperty.REFTYPE = "New Ref";
                        AccoutProperty.SRNO = DtabPaymentPickupBillDetail.Rows.Count + 1;
                        AccoutProperty.Mode = LblMode.Text;

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

                        if (mFormType == FORMTYPE.BP)
                        {
                            AccoutProperty.ENTRYTYPE = "PAYMENT";
                            AccoutProperty.BOOKTYPE = "BP";
                            AccoutProperty.BOOKTYPEFULL = "BANK PAYMENT";
                            AccoutProperty.TRNTYPE = "BANK PAYMENT";
                            //AccoutProperty.DEBAMOUNT = 0;
                            //AccoutProperty.DEBAMOUNTFE = 0;
                            //AccoutProperty.CRDAMOUNT = Val.Val(DtabPaymentSummry.Rows[i]["DEBITAMOUNT"]);
                            //AccoutProperty.CRDAMOUNTFE = Val.Val(DtabPaymentSummry.Rows[i]["DEBITAMOUNTFE"]);
                            //if (RdoCurrency.SelectedIndex.ToString() == "1")
                            //{
                            //    AccoutProperty.DEBAMOUNTFE = 0;
                            //    AccoutProperty.DEBAMOUNT = 0;
                            //    //AccoutProperty.CRDAMOUNT = Val.Val(txtAmount.Text);
                            //    //AccoutProperty.CRDAMOUNTFE = Val.Val(Val.ToDecimal(txtAmount.Text) * Val.ToDecimal(txtExcRate.Text));
                            //    AccoutProperty.CRDAMOUNT = Val.Val(DtabPaymentSummry.Rows[i]["DEBITAMOUNT"]);
                            //    AccoutProperty.CRDAMOUNTFE = Val.Val(Val.ToDecimal(DtabPaymentSummry.Rows[i]["DEBITAMOUNT"]) * Val.ToDecimal(txtExcRate.Text));
                            //}
                            //else
                            //{
                            //    AccoutProperty.DEBAMOUNTFE = 0;
                            //    AccoutProperty.DEBAMOUNT = 0;
                            //    AccoutProperty.CRDAMOUNTFE = Val.Val(txtAmount.Text);
                            //    if (Val.ToDecimal(txtExcRate.Text) != 0)
                            //        AccoutProperty.CRDAMOUNT = Val.Val(Val.ToDecimal(txtAmount.Text) / Val.ToDecimal(txtExcRate.Text));
                            //    else
                            //        AccoutProperty.CRDAMOUNT = Val.Val(txtAmount.Text);
                            //}

                        }
                        else if (mFormType == FORMTYPE.CP)
                        {
                            AccoutProperty.ENTRYTYPE = "PAYMENT";
                            AccoutProperty.BOOKTYPE = "CP";
                            AccoutProperty.BOOKTYPEFULL = "CASH PAYMENT";
                            AccoutProperty.TRNTYPE = "CASH PAYMENT";
                            //AccoutProperty.DEBAMOUNT = 0;
                            //AccoutProperty.DEBAMOUNTFE = 0;
                            //AccoutProperty.CRDAMOUNT = Val.Val(DtabPaymentSummry.Rows[i]["DEBITAMOUNT"]);
                            //AccoutProperty.CRDAMOUNTFE = Val.Val(DtabPaymentSummry.Rows[i]["DEBITAMOUNTFE"]);
                            //if (RdoCurrency.SelectedIndex.ToString() == "1")
                            //{
                            //    AccoutProperty.DEBAMOUNTFE = 0;
                            //    AccoutProperty.DEBAMOUNT = 0;
                            //    AccoutProperty.CRDAMOUNT = Val.Val(txtAmount.Text);
                            //    AccoutProperty.CRDAMOUNTFE = Val.Val(Val.ToDecimal(txtAmount.Text) * Val.ToDecimal(txtExcRate.Text));
                            //}
                            //else
                            //{
                            //    AccoutProperty.DEBAMOUNTFE = 0;
                            //    AccoutProperty.DEBAMOUNT = 0;
                            //    AccoutProperty.CRDAMOUNTFE = Val.Val(txtAmount.Text);
                            //    if (Val.ToDecimal(txtExcRate.Text) != 0)
                            //        AccoutProperty.CRDAMOUNT = Val.Val(Val.ToDecimal(txtAmount.Text) / Val.ToDecimal(txtExcRate.Text));
                            //    else
                            //        AccoutProperty.CRDAMOUNT = Val.Val(txtAmount.Text);
                            //}
                        }
                        else if (mFormType == FORMTYPE.BR)
                        {
                            AccoutProperty.ENTRYTYPE = "RECEIPT";
                            AccoutProperty.BOOKTYPE = "BR";
                            AccoutProperty.BOOKTYPEFULL = "BANK RECEIPT";
                            AccoutProperty.TRNTYPE = "BANK RECEIPT";
                            
                            //if (RdoCurrency.SelectedIndex.ToString() == "1")
                            //{
                            //    AccoutProperty.CRDAMOUNTFE = 0;
                            //    AccoutProperty.CRDAMOUNT = 0;
                            //    AccoutProperty.DEBAMOUNT = Val.Val(txtAmount.Text);
                            //    AccoutProperty.DEBAMOUNTFE = Val.Val(Val.ToDecimal(txtAmount.Text) * Val.ToDecimal(txtExcRate.Text));
                            //}
                            //else
                            //{
                            //    AccoutProperty.CRDAMOUNTFE = 0;
                            //    AccoutProperty.CRDAMOUNT = 0;
                            //    AccoutProperty.DEBAMOUNTFE = Val.Val(txtAmount.Text);
                            //    if (Val.ToDecimal(txtExcRate.Text) != 0)
                            //        AccoutProperty.DEBAMOUNT = Val.Val(Val.ToDecimal(txtAmount.Text) / Val.ToDecimal(txtExcRate.Text));
                            //    else
                            //        AccoutProperty.DEBAMOUNT = Val.Val(txtAmount.Text);
                            //}
                        }
                        else if (mFormType == FORMTYPE.CR)
                        {
                            AccoutProperty.ENTRYTYPE = "RECEIPT";
                            AccoutProperty.BOOKTYPE = "CR";
                            AccoutProperty.BOOKTYPEFULL = "CASH RECEIPT";
                            AccoutProperty.TRNTYPE = "CASH RECEIPT";
                            //if (RdoCurrency.SelectedIndex.ToString() == "1")
                            //{
                            //    AccoutProperty.CRDAMOUNTFE = 0;
                            //    AccoutProperty.CRDAMOUNT = 0;
                            //    AccoutProperty.DEBAMOUNT = Val.Val(txtAmount.Text);
                            //    AccoutProperty.DEBAMOUNTFE = Val.Val(Val.ToDecimal(txtAmount.Text) * Val.ToDecimal(txtExcRate.Text));
                            //}
                            //else
                            //{
                            //    AccoutProperty.CRDAMOUNTFE = 0;
                            //    AccoutProperty.CRDAMOUNT = 0;
                            //    AccoutProperty.DEBAMOUNTFE = Val.Val(txtAmount.Text);
                            //    if (Val.ToDecimal(txtExcRate.Text) != 0)
                            //        AccoutProperty.DEBAMOUNT = Val.Val(Val.ToDecimal(txtAmount.Text) / Val.ToDecimal(txtExcRate.Text));
                            //    else
                            //        AccoutProperty.DEBAMOUNT = Val.Val(txtAmount.Text);
                            //}
                        }

                        AccoutProperty.DEBAMOUNT = Val.Val(DtabPaymentSummry.Rows[i]["CREDITAMOUNT"]);
                        AccoutProperty.DEBAMOUNTFE = Val.Val(DtabPaymentSummry.Rows[i]["CREDITAMOUNTFE"]);
                        AccoutProperty.CRDAMOUNT = Val.Val(DtabPaymentSummry.Rows[i]["DEBITAMOUNT"]);
                        AccoutProperty.CRDAMOUNTFE = Val.Val(DtabPaymentSummry.Rows[i]["DEBITAMOUNTFE"]);

                        DataTable dt = new DataTable();

                        //var DRows = DtabPaymentSummry.Rows.Cast<DataRow>().Where(row => Val.ToString(row["SRNO"]) == "").ToArray();
                        //foreach(DataRow dr in DRows)
                        //    DtabPaymentSummry.Rows.Remove(dr);

                        var DRow = DtabPaymentSummry.Rows.Cast<DataRow>().Where(row => Val.ToString(row["STATUS"]) == "" && Val.ToInt32(row["SRNO"]) == SRNOFORDTL).ToArray();
                        foreach (DataRow dr in DRow)
                            dt = DtabPaymentSummry.AsEnumerable().Where(rows1 => Val.ToString(rows1["STATUS"]) == "" && Val.ToInt32(rows1["SRNO"]) == SRNOFORDTL).CopyToDataTable();

                        //var DRows = dt.Rows.Cast<DataRow>().Where(row => Val.ToString(row["SRNO"]) == "").ToArray();
                        //foreach (DataRow dr in DRows)
                        //    dt.Rows.Remove(dr);

                        dt.TableName = "ACCOUNTSUM";
                        string AccountSummrySaveForXML;
                        using (StringWriter sw = new StringWriter())
                        {
                            dt.WriteXml(sw);
                            AccountSummrySaveForXML = sw.ToString();
                        }

                        //DtabPaymentDetail.TableName = "ACCOUNTMED";
                        dt.TableName = "ACCOUNTMED";
                        string AccountMeditorSaveForXML;
                        using (StringWriter sw = new StringWriter())
                        {
                            dt.WriteXml(sw);
                            AccountMeditorSaveForXML = sw.ToString();
                        }

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
                        if (AccoutProperty.ReturnMessageType == "SUCCESS")
                        {
                            if (BOConfiguration.gStrLoginSection != "B")
                            {
                                //Global.Message(AccoutProperty.ReturnMessageDesc);
                                DtabPaymentSummry.Rows[i]["STATUS"] = 1;
                                //DtabPaymentPickupBillDetail.Rows.Clear();
                                //DtabPaymentDetail.Rows.Clear();
                                IntSumryCount = 1;
                                IntAllowToCalculate = 0;

                            }
                            else
                            {
                                //AUTOSAVE = true;
                                //GrdDetPayment.SetFocusedRowCellValue("STATUS", 1);
                                DtabPaymentSummry.Rows[i]["STATUS"] = 1;
                                //DtabPaymentPickupBillDetail.Rows.Clear();
                                //DtabPaymentDetail.Rows.Clear();
                                IntSumryCount = 1;
                                IntAllowToCalculate = 0;
                                if(LblMode.Text == "Edit Mode")
                                {
                                    //BtnClear.PerformClick();                                    
                                }
                            }
                        }
                        MainGrdPayment.DataSource = DtabPaymentSummry;
                        MainGrdPayment.RefreshDataSource();
                    }
                }
                if (AccoutProperty.ReturnMessageType == "SUCCESS")
                {
                    if (BOConfiguration.gStrLoginSection != "B")
                    {
                        Global.Message(AccoutProperty.ReturnMessageDesc);
                    }
                }
                if (BOConfiguration.gStrLoginSection != "B")
                {
                    BtnClear.PerformClick();                    
                }
                else
                {
                    if (LblMode.Text == "Edit Mode")
                    {
                        BtnClear.PerformClick();                                    
                    }
                }
                //}
                //else
                //{
                //    NewId = Guid.Parse(Val.ToString(drow["TRN_ID"]));
                //    GrdDetPayment.SetFocusedRowCellValue("TRN_ID", NewId);
                //}                
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

            //CmbPaymentType.SelectedIndex = 0;
            txtAmount.Text = "0";
            txtRefDocNo.Text = "";
            txtRemark.Text = "";
            txtExcRate.Text = "";

            DTPEntryDate.Text = DateTime.Now.ToString();
            //GrdDetPayment.SetFocusedRowCellValue("ENTRYDATE", DateTime.Now.ToShortDateString());

            MainGrdPayment.DataSource = null;
            MainGrdPayment.Refresh();

            txtFinYear.Text = Config.FINYEARNAME;

            RdoPayType.SelectedIndex = 0;
            RdoCurrency.SelectedIndex = 0;
            CmbPaymentType.SelectedIndex = 0;
            string Str = Val.Left(CmbPaymentType.SelectedItem.ToString(), 2);

            txtVoucherNo.Text = objLedgerTrn.FindVoucherNoNew(txtFinYear.Text, Str).ToString();
            txtVoucherStr.Text = Config.FINYEARSHORTNAME + "/" + Str + "/" + txtVoucherNo.Text;

            DtabPaymentPickupBillDetail.Rows.Clear();
            DataColumnCollection col = DtabPaymentPickupBillDetail.Columns;
            if (!col.Contains("ENTRYDATE"))
            {
                DtabPaymentPickupBillDetail.Columns.Add("ENTRYDATE", typeof(string));
            }
            DtabPaymentSummry.Rows.Clear();
            DtabPaymentDetail.Rows.Clear();
            IntSumryCount = 1;
            IntAllowToCalculate = 0;
            txtChqNo.Text = "";
            Global.SelectLanguage(Global.LANGUAGE.ENGLISH);

            lblParty.Text = "( Balance )";
            GrdDetPayment.Columns["CHQISSUEDT"].Visible = false;
            GrdDetPayment.Columns["Chq_no"].Visible = false;
            GrdDetail.Columns["INVAMOUNT"].Visible = false;
            AUTOSAVE = false;
            if (BOConfiguration.gStrLoginSection == "B")
            {
                BtnSave.Enabled = false;
            }
            else { BtnSave.Enabled = true; }
            RdoCurrency_SelectedIndexChanged(null, null);
            TempDataTable.Rows.Clear();
            CmbPaymentType_SelectedIndexChanged(null, null);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (DtabPaymentSummry.Rows.Count == 0)
                {
                    return;
                }
                //if (Val.ToString(txtTrnID.Text).Trim().Equals(string.Empty))
                //    return;

                if (Global.Confirm("Are You Sure To Delete This Payment Record ?") == System.Windows.Forms.DialogResult.No)
                    return;
                FrmPassword FrmPassword = new FrmPassword();
                ObjPer.PASSWORD = "123"; //Added by Daksha on 30/05/2023
                if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;

                    LedgerTransactionProperty Property = new LedgerTransactionProperty();
                    //Property.Trn_ID = Val.ToString(GrdDetPayment.GetFocusedRowCellValue("TRN_ID")).Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(GrdDetPayment.GetFocusedRowCellValue("TRN_ID"))); //Comment by Daksha on 30/05/2023
                    for (int i = 0; i < DtabPaymentSummry.Rows.Count; i++)
                    {
                        Property.Trn_ID = LblMode.Text == "Add Mode" ? Val.ToGuid(DtabPaymentSummry.Rows[i]["TRN_ID"]) : Val.ToGuid(DtabPaymentSummry.Rows[i]["ACCLEDGTRNTRN_ID"]); //Added by Daksha on 13/09/2023
                        if (Property.Trn_ID == Guid.Empty)
                        {
                            continue;
                        }
                        Property = objLedgerTrn.DeleteNew(Property);
                        if (Property.ReturnMessageType != "SUCCESS")
                        {
                            break;
                        }
                    }
                    //Property.Trn_ID =  Guid.Parse(txtTrnID.Text); //Added by Daksha on 30/05/2023
                    //Property = objLedgerTrn.DeleteNew(Property);

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

            if (Str == "CP" || Str == "CR")
            {
               DTCASH = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LEDGERCASH);
            }
            else if (Str == "BP" || Str == "BR")
            {
                DTBANK = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LEDGERBANK);
            }

            switch (Str)
            {
                case "CP":
                    mFormType = FORMTYPE.CP;
                    lblAccount.Text = "Cash A/C";
                    cmbPaymentMode.SelectedIndex = 0;
                    cmbPaymentMode.Enabled = false;
                    txtChqNo.Enabled = false;
                    DtpChqIssue.Enabled = false;
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
                    cmbPaymentMode.SelectedIndex = 3;
                    cmbPaymentMode.Enabled = true;
                    txtChqNo.Enabled = true;
                    DtpChqIssue.Enabled = true;
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
                    cmbPaymentMode.SelectedIndex = 3;
                    cmbPaymentMode.Enabled = true;
                    txtChqNo.Enabled = true;
                    DtpChqIssue.Enabled = true;
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
                    FrmSearch.Width = 700;
                    FrmSearch.Height = 500;
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

                        txtExcRate.Text = new BOTRN_MemoEntry().GetExchangeRate(Val.ToInt(RdoCurrency.SelectedIndex.ToString()), Val.SqlDate(DTPEntryDate.Value.ToShortDateString()), mFormType.ToString()).ToString();
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
                    if (RdoCurrency.SelectedIndex.ToString() == "1")
                    {
                        if (Val.Val(DtabPaymentPickupBillDetail.Rows[IntI]["DEBIT"]) > 0 || Val.Val(DtabPaymentPickupBillDetail.Rows[IntI]["CREDIT"]) > 0)
                        {
                            DtabPaymentPickupBillDetail.Rows[IntI]["DEBITFE"] = Val.Val(DtabPaymentPickupBillDetail.Rows[IntI]["DEBIT"]) * Val.Val(txtExcRate.Text);
                            DtabPaymentPickupBillDetail.Rows[IntI]["CREDITFE"] = Val.Val(DtabPaymentPickupBillDetail.Rows[IntI]["CREDIT"]) * Val.Val(txtExcRate.Text);
                        }
                    }
                    else
                    {
                        if (Val.Val(DtabPaymentPickupBillDetail.Rows[IntI]["DEBIT"]) > 0 || Val.Val(DtabPaymentPickupBillDetail.Rows[IntI]["CREDIT"]) > 0)
                        {
                            DtabPaymentPickupBillDetail.Rows[IntI]["DEBIT"] = Val.Val(DtabPaymentPickupBillDetail.Rows[IntI]["DEBITFE"]) / Val.Val(txtExcRate.Text);
                            DtabPaymentPickupBillDetail.Rows[IntI]["CREDIT"] = Val.Val(DtabPaymentPickupBillDetail.Rows[IntI]["CREDITFE"]) / Val.Val(txtExcRate.Text);
                        }
                    }
                }

                for (int IntI = 0; IntI < DtabPaymentDetail.Rows.Count; IntI++)
                {
                    if (RdoCurrency.SelectedIndex.ToString() == "1")
                    {
                        DtabPaymentDetail.Rows[IntI]["FPAYMENTAMOUNT"] = Val.Val(DtabPaymentDetail.Rows[IntI]["PAYMENTAMOUNT"]) * Val.Val(txtExcRate.Text);
                        DtabPaymentDetail.Rows[IntI]["FEBANKCHARGES"] = Val.Val(DtabPaymentDetail.Rows[IntI]["BANKCHARGES"]) * Val.Val(txtExcRate.Text);
                    }
                    else
                    {
                        DtabPaymentDetail.Rows[IntI]["PAYMENTAMOUNT"] = Val.Val(DtabPaymentDetail.Rows[IntI]["FPAYMENTAMOUNT"]) / Val.Val(txtExcRate.Text);
                        DtabPaymentDetail.Rows[IntI]["BANKCHARGES"] = Val.Val(DtabPaymentDetail.Rows[IntI]["FEBANKCHARGES"]) / Val.Val(txtExcRate.Text);
                    }


                    if (Val.ToString(DtabPaymentDetail.Rows[IntI]["PAYMENTMADE"]) == "Full")
                    {
                        int IntCurrId = Val.ToInt32(DtabPaymentDetail.Rows[IntI]["CURRENCY_ID"]);
                        if (IntCurrId == 1 && Val.ToInt32(RdoCurrency.SelectedIndex.ToString()) == 1)
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
                    if (RdoCurrency.SelectedIndex.ToString() == "1")
                    {
                        if (Val.Val(DtabPaymentSummry.Rows[IntI]["DEBITAMOUNT"]) > 0 || Val.Val(DtabPaymentSummry.Rows[IntI]["CREDITAMOUNT"]) > 0)
                        {
                            DtabPaymentSummry.Rows[IntI]["DEBITAMOUNTFE"] = Val.Val(DtabPaymentSummry.Rows[IntI]["DEBITAMOUNT"]) * Val.Val(txtExcRate.Text);
                            DtabPaymentSummry.Rows[IntI]["CREDITAMOUNTFE"] = Val.Val(DtabPaymentSummry.Rows[IntI]["CREDITAMOUNT"]) * Val.Val(txtExcRate.Text);
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
                            DtabPaymentSummry.Rows[IntI]["DEBITAMOUNT"] = Val.Val(DtabPaymentSummry.Rows[IntI]["DEBITAMOUNTFE"]) / Val.Val(txtExcRate.Text);
                            DtabPaymentSummry.Rows[IntI]["CREDITAMOUNT"] = Val.Val(DtabPaymentSummry.Rows[IntI]["CREDITAMOUNTFE"]) / Val.Val(txtExcRate.Text);
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
            //try
            //{
            //    this.Cursor = Cursors.WaitCursor;

            //    DataSet DSetSummary = objLedgerTrn.GetBillWisePaymentGetDataNew("SUMMARY", Val.SqlDate(DTPFromDate.Text), Val.SqlDate(DTPToDate.Text), "", Guid.Parse("00000000-0000-0000-0000-000000000000"));

            //    if (DSetSummary.Tables.Count <= 0)
            //    {
            //        this.Cursor = Cursors.Default;
            //        Global.Message("No Data Found.");
            //        //MainGrid.DataSource = null;
            //        return;
            //    }

            //    if (DSetSummary.Tables[0].Rows.Count <= 0)
            //    {
            //        this.Cursor = Cursors.Default;
            //        Global.Message("No Data Found.");
            //        //MainGrid.DataSource = null;
            //        return;
            //    }
            //   // MainGrid.DataSource = DSetSummary.Tables[0];
            //   // GrdDet.RefreshData();

            //    this.Cursor = Cursors.Default;
            //}
            //catch (Exception ex)
            //{
            //    this.Cursor = Cursors.Default;
            //    Global.Message(ex.Message.ToString());
            //}
        }

        //private void GrdDet_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        //{
        //    try
        //    {
        //        if (e.RowHandle < 0)
        //            return;

        //        if (e.Clicks == 2)
        //        {
        //            this.Cursor = Cursors.WaitCursor;

        //            DataRow Drow = GrdDet.GetFocusedDataRow();
        //            DataSet DSet = new DataSet();

        //            DSet = objLedgerTrn.GetBillWisePaymentGetDataNew("DETAIL", "", "", Val.ToString(Drow["BOOKTYPEFULL"]), Guid.Parse(Val.ToString(Drow["TRN_ID"])));
        //            if (DSet.Tables.Count <= 0)
        //            {
        //                this.Cursor = Cursors.Default;
        //                Global.Message("Some Thing Went Wrong");
        //                MainGrid.DataSource = null;
        //                return;
        //            }
        //            DSet.DataSetName = "DocumentElement";
        //            if (DSet.Tables.Count == 3)
        //            {
        //                DtabPaymentPickupBillDetail = DSet.Tables[0];
        //                DtabPaymentSummry = DSet.Tables[1];
        //                DtabPaymentDetail = DSet.Tables[2];
        //                LblMode.Text = "Edit Mode";

        //                if (!DtabPaymentSummry.Columns.Contains("CLOSINGBALANCE"))
        //                {
        //                    DtabPaymentSummry.Columns.Add("CLOSINGBALANCE");
        //                }

        //                for (int i = 0; i < DtabPaymentSummry.Rows.Count; i++)
        //                    DtabPaymentSummry.Rows[i]["CLOSINGBALANCE"] = Global.FindLedgerClosingStr(Val.ToString(DtabPaymentSummry.Rows[i]["LEDGER_ID"]).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(DtabPaymentSummry.Rows[i]["LEDGER_ID"])));


        //                lblBalance.Text = Global.FindLedgerClosingStr(Val.ToString(txtCashBankAC.Tag).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtCashBankAC.Tag)));

        //                if (Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH PAYMENT")
        //                    CmbPaymentType.SelectedIndex = 0;
        //                else if (Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK PAYMENT")
        //                    CmbPaymentType.SelectedIndex = 1;
        //                else if (Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH RECEIPT")
        //                    CmbPaymentType.SelectedIndex = 2;
        //                else if (Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK RECEIPT")
        //                    CmbPaymentType.SelectedIndex = 3;

        //                txtTrnID.Text = Val.ToString(Drow["TRN_ID"]);
        //                DTPEntryDate.Text = Val.ToString(Drow["VOUCHERDATE"]);
        //                txtVoucherNo.Text = Val.ToString(Drow["VOUCHERNO"]);
        //                txtVoucherStr.Text = Val.ToString(Drow["VOUCHERNOSTR"]);
        //                txtCurrency.Tag = Val.ToString(Drow["CURRENCY_ID"]);
        //                txtCurrency.Text = Val.ToString(Drow["CURRENCYNAME"]);
        //                txtExcRate.Text = Val.ToString(Drow["EXCRATE"]);
        //                txtRemark.Text = Val.ToString(Drow["NOTE"]);
        //                txtChqNo.Text = Val.ToString(Drow["CHQ_NO"]);
        //                DtpChqIssue.Text = Val.ToString(Drow["CHQISSUEDT"]);
        //                cmbPaymentMode.SelectedItem = Val.ToString(Drow["PAYTYPE"]);
        //                txtCashBankAC.Tag = Val.ToString(Drow["LEDGER_ID"]);
        //                txtCashBankAC.Text = Val.ToString(Drow["LEDGERNAME"]);

        //                if (Val.ToInt32(Drow["CONVERTTOINR"]) == 1)
        //                    ChkBxConvertToInr.Checked = true;
        //                else
        //                    ChkBxConvertToInr.Checked = false;

        //                if (Val.ToInt32(Drow["CURRENCY_ID"]) == 1)
        //                {
        //                    if (Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH PAYMENT" || Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK PAYMENT")
        //                        txtAmount.Text = Val.ToString(Drow["CRDAMT"]);
        //                    else
        //                        txtAmount.Text = Val.ToString(Drow["DEBAMT"]);
        //                }
        //                else
        //                {
        //                    if (Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH PAYMENT" || Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK PAYMENT")
        //                        txtAmount.Text = Val.ToString(Drow["CRDAMTFE"]);
        //                    else
        //                        txtAmount.Text = Val.ToString(Drow["DEBAMTFE"]);
        //                }

        //                if (Val.ToString(txtCurrency.Text) == "USD")
        //                { ChkBxConvertToInr.Visible = true; lblAmount.Text = "Amount ($)"; }
        //                else
        //                { ChkBxConvertToInr.Visible = false; lblAmount.Text = "Amount (₹)"; }

        //                MainGrdPayment.DataSource = DtabPaymentSummry;
        //                MainGrdPayment.Refresh();

        //                MainGridDetail.DataSource = DtabPaymentPickupBillDetail;
        //                MainGridDetail.Refresh();
        //                //xtraTabControl1.SelectedTabPageIndex = 0;

        //                DataTable DtCheckAllocated = ObjFinance.CheckBillAllocatedOrNot(txtTrnID.Text, "");
        //                if (DtCheckAllocated.Rows.Count > 0)
        //                {
        //                    LblBillPickupMessage.Text = "This Entry Allocated In Accounts ( " + Val.ToString(DtCheckAllocated.Rows[0][0]) + " ) ,Please Delete For Edit.";
        //                    BtnSave.Enabled = false;
        //                    BtnDelete.Enabled = false;
        //                    LblMode.Text = "View Mode";
        //                }
        //                else
        //                {
        //                    LblBillPickupMessage.Text = "";
        //                    BtnSave.Enabled = true;
        //                    BtnDelete.Enabled = true;
        //                    LblMode.Text = "Edit Mode";
        //                }

        //            }
        //            this.Cursor = Cursors.Default;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.Cursor = Cursors.Default;
        //        Global.Message(ex.Message.ToString());
        //    }
        //}

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
                    FrmSearch.Width = 700;
                    FrmSearch.Height = 500;
                    string Str = Val.Left(Val.ToString(CmbPaymentType.SelectedItem), 2);
                    DataTable DTFINAL = new DataTable();
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
                            GrdDetPayment.SetFocusedRowCellValue("DEBITAMOUNT", 0.000);
                            GrdDetPayment.SetFocusedRowCellValue("CREDITAMOUNT", 0.000);
                            GrdDetPayment.SetFocusedRowCellValue("DEBITAMOUNTFE","0.000");
                            GrdDetPayment.SetFocusedRowCellValue("CREDITAMOUNTFE", "0.000");
                            GrdDetPayment.SetFocusedRowCellValue("REFTYPE", "New Ref");
                            GrdDetPayment.SetFocusedRowCellValue("REFLEDGER_ID", Val.ToString(txtCashBankAC.Tag));
                            GrdDetPayment.SetFocusedRowCellValue("EDTBL", "Yes");
                        }
                        IntSumryCount++;

                        GrdDetPayment.SetFocusedRowCellValue("CLOSINGBALANCE", Global.FindLedgerClosingStr(Val.ToString(FrmSearch.DRow["LEDGER_ID"]).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(FrmSearch.DRow["LEDGER_ID"]))));

                        DTBal = new BusLib.Account.BOLedgerTransaction().FindLedgerClosingNew(Val.ToString(FrmSearch.DRow["LEDGER_ID"]).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(FrmSearch.DRow["LEDGER_ID"])));
                        if (DTBal.Rows.Count > 0)
                        {
                            if (RdoCurrency.SelectedIndex.ToString() == "1")
                            {
                                if (Val.ToDouble(DTBal.Rows[0]["CloseBalanceDollar"]) < 0)
                                {
                                    lblParty.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Dr";
                                }
                                else
                                {
                                    lblParty.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Cr";
                                }
                            }
                            else
                            {
                                if (Val.ToDouble(DTBal.Rows[0]["CloseBalanceRs"]) < 0)
                                {
                                    lblParty.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Dr";
                                }
                                else
                                {
                                    lblParty.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Cr";
                                }
                            }
                        }

                        int intGrdIndex = GrdDetPayment.GetFocusedDataSourceRowIndex() + 1;
                        //var rows = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToInt32(row["MainGrdRow_id"]) == intGrdIndex).ToArray();
                        //foreach (DataRow dr in rows)
                        //    DtabPaymentPickupBillDetail.Rows.Remove(dr);
                        calculate();
                        MainGridDetail.RefreshDataSource();
                        IntAllowToCalculate = 0;
                        //rspBtnBillAllocation_Click(null, null);
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
                GrdDetPayment.BestFitColumns();
                GrdDetPayment.SetFocusedRowCellValue("VOUCHERDATE", DateTime.Now.ToShortDateString());
                calculate();
                AUTOSAVE = false;
            }
            else
            {
                Global.MessageError("Cash/Bank Ledger Required");
                txtCashBankAC.Focus();
            }
        }

        private void rspBtnBillAllocation_Click(object sender, EventArgs e)
        {
            try
            {
                GrdDetPayment.RefreshData();
                if (RdoCurrency.SelectedIndex.ToString() != string.Empty)
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
                           
                            frm.Ledger_Id = Guid.Parse(Val.ToString(GrdDetPayment.GetFocusedRowCellValue("LEDGER_ID")));
                            frm.TRN_ID = Val.ToString(txtTrnID.Text).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtTrnID.Text));
                            //frm.TRN_ID = Guid.Parse(Val.ToString(NewId)).Equals(String.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(GrdDetPayment.GetFocusedRowCellValue("TRN_ID")));
                            frm.FormType = Str;
                            frm.ExcRAte = Val.ToDecimal(txtExcRate.Text);
                            //kuldeep 05012021
                            frm.IntCurrency_ID = Val.ToInt32(RdoCurrency.SelectedIndex.ToString());
                            frm.DtAllocatedBill = DtabPaymentDetail;
                            frm.IntRowIndex = intGrdIndex;
                            frm.StrEntryDate = Val.SqlDate(DTPEntryDate.Text);
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
                                    NewDr["ENTRYDATE"] = Dr["ENTRYDATE"];

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
                                        decDebit += Val.ToDecimal(Dr["PAYMENTAMOUNT"]);
                                        NewDr["CREDIT"] = "0";
                                        NewDr["DEBITFE"] = Math.Round(Val.ToDecimal(Dr["FPAYMENTAMOUNT"]), 3);
                                        decDebitFe += Math.Round(Val.ToDecimal(Dr["FPAYMENTAMOUNT"]), 3);
                                        NewDr["CREDITFE"] = "0";

                                    }
                                    else
                                    {
                                        NewDr["DEBIT"] = "0";
                                        NewDr["CREDIT"] = Dr["PAYMENTAMOUNT"];
                                        decCredit += Val.ToDecimal(Dr["PAYMENTAMOUNT"]);
                                        NewDr["DEBITFE"] = "0";
                                        NewDr["CREDITFE"] = Math.Round(Val.ToDecimal(Dr["FPAYMENTAMOUNT"]), 3);
                                        decCreditFe += Math.Round(Val.ToDecimal(Dr["FPAYMENTAMOUNT"]), 3);
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
                                    NewDr["ENTRYDATE"] = Dr["ENTRYDATE"];


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
                                            NewDr["DEBITFE"] = Math.Round(Val.ToDecimal(Dr["FEBANKCHARGES"]), 3);
                                            decDebitFe += Math.Round(Val.ToDecimal(Dr["FEBANKCHARGES"]), 3);
                                            decBankCreditFe += Math.Round(Val.ToDecimal(Dr["FEBANKCHARGES"]), 3);
                                            NewDr["CREDITFE"] = "0";
                                        }
                                        else
                                        {
                                            NewDr["DEBIT"] = "0";
                                            NewDr["CREDIT"] = Dr["BANKCHARGES"];
                                            decCredit += Val.ToDecimal(Dr["BANKCHARGES"]);
                                            decBankDebit += Val.ToDecimal(Dr["BANKCHARGES"]);
                                            NewDr["DEBITFE"] = "0";
                                            NewDr["CREDITFE"] = Math.Round(Val.ToDecimal(Dr["FEBANKCHARGES"]), 3);
                                            decBankDebitFe += Math.Round(Val.ToDecimal(Dr["FEBANKCHARGES"]), 3);
                                            decCreditFe += Math.Round(Val.ToDecimal(Dr["FEBANKCHARGES"]), 3);
                                        }
                                        NewDr["REFTRN_ID"] = Dr["TRN_ID"];
                                        NewDr["REFSRNO"] = Dr["SRNO"];
                                        NewDr["REFACCLEDGTRNTRN_ID"] = Dr["ACCLEDGTRNTRN_ID"];
                                        NewDr["REFACCLEDGTRNSRNO"] = Dr["ACCLEDGTRNSRNO"];
                                        NewDr["REFTYPE"] = "Agnst Ref";
                                        NewDr["MainGrdRow_id"] = intGrdIndex;
                                        NewDr["REFLEDGER_ID"] = DtBankCharges.Rows.Count > 0 ? Val.ToString(DtBankCharges.Rows[0]["LEDGER_ID"]) : Val.ToString(Guid.Empty);
                                        //kuldeep 05012021
                                        NewDr["REFBOOKTYPEFULL"] = Dr["BOOKTYPEFULL"];
                                        NewDr["REFVOUCHERNOSTR"] = Dr["VOUCHERNOSTR"];
                                        NewDr["REFVOUCHERDATE"] = Dr["VOUCHERDATE"];
                                        NewDr["ENTRYDATE"] = Dr["ENTRYDATE"];

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
                                            NewDr["DEBITFE"] = Math.Round(Val.ToDecimal(Dr["FDISCOUNTAMOUNT"]), 3);
                                            decDebitFe += Math.Round(Val.ToDecimal(Dr["FDISCOUNTAMOUNT"]), 3);
                                            decDiscountCreditFe += Math.Round(Val.ToDecimal(Dr["FDISCOUNTAMOUNT"]), 3);
                                            NewDr["CREDITFE"] = "0";
                                        }
                                        else
                                        {
                                            NewDr["DEBIT"] = "0";
                                            NewDr["CREDIT"] = Dr["DISCOUNTAMOUNT"];
                                            decCredit += Val.ToDecimal(Dr["DISCOUNTAMOUNT"]);
                                            decDiscountDebit += Val.ToDecimal(Dr["DISCOUNTAMOUNT"]);
                                            NewDr["DEBITFE"] = "0";
                                            NewDr["CREDITFE"] = Math.Round(Val.ToDecimal(Dr["FDISCOUNTAMOUNT"]), 3);
                                            decDiscountDebitFe += Math.Round(Val.ToDecimal(Dr["FDISCOUNTAMOUNT"]), 3);
                                            decCreditFe += Math.Round(Val.ToDecimal(Dr["FDISCOUNTAMOUNT"]), 3);
                                        }
                                        NewDr["REFTRN_ID"] = Dr["TRN_ID"];
                                        NewDr["REFSRNO"] = Dr["SRNO"];
                                        NewDr["REFACCLEDGTRNTRN_ID"] = Dr["ACCLEDGTRNTRN_ID"];
                                        NewDr["REFACCLEDGTRNSRNO"] = Dr["ACCLEDGTRNSRNO"];
                                        NewDr["REFTYPE"] = "Agnst Ref";
                                        NewDr["MainGrdRow_id"] = intGrdIndex;
                                        NewDr["REFLEDGER_ID"] = DtBankCharges.Rows.Count > 0 ? Val.ToString(DtBankCharges.Rows[0]["LEDGER_ID"]) : Val.ToString(Guid.Empty);
                                        NewDr["REFBOOKTYPEFULL"] = Dr["BOOKTYPEFULL"];
                                        NewDr["REFVOUCHERNOSTR"] = Dr["VOUCHERNOSTR"];
                                        NewDr["REFVOUCHERDATE"] = Dr["VOUCHERDATE"];
                                        NewDr["ENTRYDATE"] = Dr["ENTRYDATE"];

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
                                                NewDr["DEBITFE"] = Math.Round(Val.ToDecimal(Dr["EXCHRATEDIFF"]), 3);
                                                decDebitFe += Math.Round(Val.ToDecimal(Dr["EXCHRATEDIFF"]), 3);
                                                decExcRateDiffCreditFe += Math.Round(Val.ToDecimal(Dr["EXCHRATEDIFF"]), 3);
                                                NewDr["CREDITFE"] = "0";

                                                //NewDr["DEBITFE"] = Math.Round(Val.ToDecimal(Dr["EXCHRATEDIFF"]), 2, MidpointRounding.AwayFromZero);
                                                //decDebitFe += Math.Round(Val.ToDecimal(Dr["EXCHRATEDIFF"]), 2, MidpointRounding.AwayFromZero);
                                                //decExcRateDiffCreditFe += Math.Round(Val.ToDecimal(Dr["EXCHRATEDIFF"]), 2, MidpointRounding.AwayFromZero);
                                                //NewDr["CREDITFE"] = "0";
                                            }
                                            else
                                            {
                                                decimal DecExcRate = Math.Round(Val.ToDecimal(Dr["EXCHRATEDIFF"]) * -1, 3);
                                                NewDr["DEBITFE"] = "0";
                                                NewDr["CREDITFE"] = DecExcRate;
                                                decExcRateDiffDebitFe += DecExcRate;
                                                decCreditFe += DecExcRate;

                                                //decimal DecExcRate = Math.Round(Val.ToDecimal(Dr["EXCHRATEDIFF"]) * -1, 2, MidpointRounding.AwayFromZero);
                                                //NewDr["DEBITFE"] = "0";
                                                //NewDr["CREDITFE"] = Math.Round(Val.Val(DecExcRate), 2, MidpointRounding.AwayFromZero);
                                                //decExcRateDiffDebitFe += Val.ToDecimal(Math.Round(Val.Val(DecExcRate), 2, MidpointRounding.AwayFromZero));
                                                //decCreditFe += Val.ToDecimal(Math.Round(Val.Val(DecExcRate), 2, MidpointRounding.AwayFromZero));
                                            }
                                            NewDr["DEBIT"] = "0";
                                            NewDr["CREDIT"] = "0";
                                        }
                                        else
                                        {
                                            if (Val.ToDecimal(Dr["EXCHRATEDIFF"]) < 0)
                                            {
                                                decimal DecExcRate = Math.Round(Val.ToDecimal(Dr["EXCHRATEDIFF"]) * -1, 3);
                                                NewDr["DEBITFE"] = DecExcRate;
                                                decDebitFe += DecExcRate;
                                                decExcRateDiffCreditFe += DecExcRate;
                                                NewDr["CREDITFE"] = "0";

                                                //decimal DecExcRate = Math.Round(Val.ToDecimal(Dr["EXCHRATEDIFF"]) * -1, 2, MidpointRounding.AwayFromZero);
                                                //NewDr["DEBITFE"] = Math.Round(Val.Val(DecExcRate), 2, MidpointRounding.AwayFromZero);
                                                //decDebitFe += Val.ToDecimal(Math.Round(Val.Val(DecExcRate), 2, MidpointRounding.AwayFromZero));
                                                //decExcRateDiffCreditFe += Val.ToDecimal(Math.Round(Val.Val(DecExcRate), 2, MidpointRounding.AwayFromZero));
                                                //NewDr["CREDITFE"] = "0";
                                            }
                                            else
                                            {
                                                NewDr["DEBITFE"] = "0";
                                                NewDr["CREDITFE"] = Math.Round(Val.ToDecimal(Dr["EXCHRATEDIFF"]), 3);
                                                decExcRateDiffDebitFe += Math.Round(Val.ToDecimal(Dr["EXCHRATEDIFF"]), 3);
                                                decCreditFe += Math.Round(Val.ToDecimal(Dr["EXCHRATEDIFF"]), 3);

                                                //NewDr["DEBITFE"] = "0";
                                                //NewDr["CREDITFE"] = Math.Round(Val.ToDecimal(Dr["EXCHRATEDIFF"]), 2, MidpointRounding.AwayFromZero);
                                                //decExcRateDiffDebitFe += Math.Round(Val.ToDecimal(Dr["EXCHRATEDIFF"]), 2, MidpointRounding.AwayFromZero);
                                                //decCreditFe += Math.Round(Val.ToDecimal(Dr["EXCHRATEDIFF"]), 2, MidpointRounding.AwayFromZero);
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
                                        NewDr["REFLEDGER_ID"] = DtExchRateDiff.Rows.Count > 0 ? Val.ToString(DtExchRateDiff.Rows[0]["LEDGER_ID"]) : Val.ToString(Guid.Empty);
                                        //kuldeep 05012021
                                        NewDr["REFBOOKTYPEFULL"] = Dr["BOOKTYPEFULL"];
                                        NewDr["REFVOUCHERNOSTR"] = Dr["VOUCHERNOSTR"];
                                        NewDr["REFVOUCHERDATE"] = Dr["VOUCHERDATE"];
                                        NewDr["ENTRYDATE"] = Dr["ENTRYDATE"];

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

                                                //NewDr["DEBIT"] = Math.Round(Val.Val(Dr["EXCHRATEDIFFUSD"]), 2, MidpointRounding.AwayFromZero);
                                                //decDebit += Val.ToDecimal(Math.Round(Val.Val(Dr["EXCHRATEDIFFUSD"]), 2, MidpointRounding.AwayFromZero));
                                                //decExcRateDiffCredit += Val.ToDecimal(Math.Round(Val.Val(Dr["EXCHRATEDIFFUSD"]), 2, MidpointRounding.AwayFromZero));
                                                //NewDr["CREDIT"] = "0";
                                            }
                                            else
                                            {
                                                decimal DecExcRate = Val.ToDecimal(Dr["EXCHRATEDIFFUSD"]) * -1;
                                                NewDr["DEBIT"] = "0";
                                                NewDr["CREDIT"] = DecExcRate;
                                                decExcRateDiffDebit += DecExcRate;
                                                decCredit += DecExcRate;

                                                //decimal DecExcRate = Val.ToDecimal(Math.Round(Val.Val(Dr["EXCHRATEDIFFUSD"]) * -1, 2, MidpointRounding.AwayFromZero));
                                                //NewDr["DEBIT"] = "0";
                                                //NewDr["CREDIT"] = Math.Round(Val.Val(DecExcRate), 2, MidpointRounding.AwayFromZero);
                                                //decExcRateDiffDebit += Val.ToDecimal(Math.Round(Val.Val(DecExcRate), 2, MidpointRounding.AwayFromZero));
                                                //decCredit += Val.ToDecimal(Math.Round(Val.Val(DecExcRate), 2, MidpointRounding.AwayFromZero));
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

                                                //decimal DecExcRate = Val.ToDecimal(Dr["EXCHRATEDIFFUSD"]) * -1;
                                                //NewDr["DEBIT"] = Math.Round(Val.Val(DecExcRate), 2, MidpointRounding.AwayFromZero);
                                                //decDebit += Val.ToDecimal(Math.Round(Val.Val(DecExcRate), 2, MidpointRounding.AwayFromZero));
                                                //decExcRateDiffCredit += Val.ToDecimal(Math.Round(Val.Val(DecExcRate), 2, MidpointRounding.AwayFromZero));
                                                //NewDr["CREDIT"] = "0";
                                            }
                                            else
                                            {
                                                NewDr["DEBIT"] = "0";
                                                NewDr["CREDIT"] = Dr["EXCHRATEDIFFUSD"];
                                                decExcRateDiffDebit += Val.ToDecimal(Dr["EXCHRATEDIFFUSD"]);
                                                decCredit += Val.ToDecimal(Dr["EXCHRATEDIFFUSD"]);

                                                //NewDr["DEBIT"] = "0";
                                                //NewDr["CREDIT"] = Math.Round(Val.Val(Dr["EXCHRATEDIFFUSD"]), 2, MidpointRounding.AwayFromZero);
                                                //decExcRateDiffDebit += Val.ToDecimal(Math.Round(Val.Val(Dr["EXCHRATEDIFFUSD"]), 2, MidpointRounding.AwayFromZero));
                                                //decCredit += Val.ToDecimal(Math.Round(Val.Val(Dr["EXCHRATEDIFFUSD"]), 2, MidpointRounding.AwayFromZero));
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
                                        NewDr["REFLEDGER_ID"] = DtExchRateDiff.Rows.Count > 0 ? Val.ToString(DtExchRateDiff.Rows[0]["LEDGER_ID"]) : Val.ToString(Guid.Empty);
                                        //kuldeep 05012021
                                        NewDr["REFBOOKTYPEFULL"] = Dr["BOOKTYPEFULL"];
                                        NewDr["REFVOUCHERNOSTR"] = Dr["VOUCHERNOSTR"];
                                        NewDr["REFVOUCHERDATE"] = Dr["VOUCHERDATE"];
                                        NewDr["ENTRYDATE"] = Dr["ENTRYDATE"];

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

                                    //GrdDetPayment.SetFocusedRowCellValue("DEBITAMOUNT", Math.Round(Val.Val(decDebit - decCredit), 2, MidpointRounding.AwayFromZero));
                                    //GrdDetPayment.SetFocusedRowCellValue("CREDITAMOUNT", 0);
                                    //GrdDetPayment.SetFocusedRowCellValue("DEBITAMOUNTFE", Math.Round(Val.Val(decDebitFe - decCreditFe), 2, MidpointRounding.AwayFromZero));
                                    //GrdDetPayment.SetFocusedRowCellValue("CREDITAMOUNTFE", 0);
                                    //GrdDetPayment.SetFocusedRowCellValue("EDTBL", "Yes");


                                    //int index = DtabPaymentSummry.Rows.Count - 1;
                                    //DtabPaymentSummry.Rows.RemoveAt(index);
                                    //DtabPaymentSummry.AcceptChanges();

                                    DtabPaymentSummry.Rows.Clear();
                                    DtabPaymentSummry.AcceptChanges();
                                    DataTable dt = new DataTable();

                                    //Comment By Gunjan:26-09-2023
                                    //var DRow = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToString(row["STATUS"]) == "").ToArray();
                                    //foreach (DataRow dr in DRow)
                                    //    dt = DtabPaymentPickupBillDetail.AsEnumerable().Where(rows1 => Val.ToString(rows1["STATUS"]) == "").CopyToDataTable();
                                    //End As Gunjan

                                    TempDataTable = DtabPaymentPickupBillDetail.Clone();

                                    if (BOConfiguration.gStrLoginSection == "B")
                                    {
                                        //Added By Gunjan:26-09-2023
                                        var DRow = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToString(row["ENTRYDATE"]) != "").ToArray();
                                        foreach (DataRow dr in DRow)
                                            dt = DtabPaymentPickupBillDetail.AsEnumerable().Where(rows1 => Val.ToString(rows1["ENTRYDATE"]) != "").CopyToDataTable();
                                        //End As Gunjan

                                        TempDataTable = dt.AsEnumerable()
                                        .GroupBy(r => new
                                        {
                                            SRNO = r.Field<int>("SRNO"),
                                            ENTRYDATE = r.Field<string>("ENTRYDATE"),
                                            LEDGER_ID = r.Field<Guid>("LEDGER_ID"),
                                            LEDGERNAME = r.Field<string>("LEDGERNAME"),
                                            REFLEDGER_ID = r.Field<Guid>("REFLEDGER_ID"),
                                            NOTE = r.Field<string>("NOTE"),
                                            STATUS = r.Field<string>("STATUS"),//Added By Gunjan:26-09-2023
                                        })
                                        .Select(g =>
                                        {
                                            var row = DtabPaymentPickupBillDetail.NewRow();
                                            row["SRNO"] = g.Key.SRNO;
                                            row["LEDGER_ID"] = g.Key.LEDGER_ID;
                                            row["LEDGERNAME"] = g.Key.LEDGERNAME;
                                            row["REFLEDGER_ID"] = g.Key.REFLEDGER_ID;
                                            row["ENTRYDATE"] = g.Key.ENTRYDATE;
                                            row["NOTE"] = g.Key.NOTE;
                                            row["DEBIT"] = g.Sum(r => r.Field<decimal>("DEBIT"));
                                            row["STATUS"] = g.Key.STATUS;//Added By Gunjan:26-09-2023
                                            return row;
                                        }).CopyToDataTable();



                                        int countno = GrdDetPayment.RowCount;
                                        foreach (DataRow DrNotFnd in TempDataTable.Rows)
                                        {
                                            if (DrNotFnd != null)
                                            {
                                                DataRow DRNotNew = DtabPaymentSummry.NewRow();

                                                DRNotNew["SRNO"] = countno = countno + 1;
                                                DRNotNew["LEDGER_ID"] = DrNotFnd["LEDGER_ID"];
                                                DRNotNew["LEDGERNAME"] = DrNotFnd["LEDGERNAME"];
                                                DRNotNew["REFLEDGER_ID"] = DrNotFnd["REFLEDGER_ID"];
                                                DRNotNew["VOUCHERDATE"] = DrNotFnd["ENTRYDATE"];
                                                DRNotNew["CREDITAMOUNT"] = 0;
                                                DRNotNew["CREDITAMOUNTFE"] = 0;
                                                DRNotNew["DEBITAMOUNT"] = DrNotFnd["DEBIT"];
                                                DRNotNew["DEBITAMOUNTFE"] = DrNotFnd["DEBIT"];
                                                DRNotNew["REFTYPE"] = "Agnst Ref";
                                                DRNotNew["EDTBL"] = "YES";
                                                DRNotNew["NOTE"] = DrNotFnd["NOTE"];
                                                DRNotNew["STATUS"] = DrNotFnd["STATUS"];//Added By Gunjan:26-09-2023

                                                DtabPaymentSummry.Rows.Add(DRNotNew);
                                                DtabPaymentSummry.AcceptChanges();

                                                DataRow[] foundRows = DtabPaymentPickupBillDetail.Select("ENTRYDATE=" + "'" + DrNotFnd["ENTRYDATE"] + "'");
                                                if (foundRows.Length > 0)
                                                {
                                                    foreach (DataRow drAdd in foundRows)
                                                    {
                                                        drAdd["MainGrdRow_id"] = countno;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //Added By Gunjan:26-09-2023
                                        var DRow = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToString(row["STATUS"]) == "").ToArray();
                                        foreach (DataRow dr in DRow)
                                            dt = DtabPaymentPickupBillDetail.AsEnumerable().Where(rows1 => Val.ToString(rows1["STATUS"]) == "").CopyToDataTable();
                                        //End As Gunjan

                                        //TempDataTable = dt;
                                        string Str1 = Val.Left(Val.ToString(CmbPaymentType.SelectedItem), 2);
                                        if (Str1 == "CP" || Str1 == "BP")
                                        {
                                            TempDataTable = dt.AsEnumerable()
                                             .GroupBy(r => new
                                             {
                                                 SRNO = r.Field<int>("SRNO"),
                                                 ENTRYDATE = r.Field<string>("ENTRYDATE"),
                                                 LEDGER_ID = r.Field<Guid>("LEDGER_ID"),
                                                 LEDGERNAME = r.Field<string>("LEDGERNAME"),
                                                 REFLEDGER_ID = r.Field<Guid>("REFLEDGER_ID"),
                                                 NOTE = r.Field<string>("NOTE"),
                                             })
                                            .Select(g =>
                                            {
                                                var row = DtabPaymentPickupBillDetail.NewRow();
                                                row["SRNO"] = g.Key.SRNO;
                                                row["LEDGER_ID"] = g.Key.LEDGER_ID;
                                                row["LEDGERNAME"] = g.Key.LEDGERNAME;
                                                row["REFLEDGER_ID"] = g.Key.REFLEDGER_ID;
                                                row["ENTRYDATE"] = g.Key.ENTRYDATE;
                                                row["NOTE"] = g.Key.NOTE;
                                                row["DEBIT"] = g.Sum(r => r.Field<decimal>("DEBIT"));
                                                return row;
                                            }).CopyToDataTable();
                                        }
                                        else
                                        {

                                            TempDataTable = dt.AsEnumerable()
                                             .GroupBy(r => new
                                             {
                                                 SRNO = r.Field<int>("SRNO"),
                                                 ENTRYDATE = r.Field<string>("ENTRYDATE"),
                                                 LEDGER_ID = r.Field<Guid>("LEDGER_ID"),
                                                 LEDGERNAME = r.Field<string>("LEDGERNAME"),
                                                 REFLEDGER_ID = r.Field<Guid>("REFLEDGER_ID"),
                                                 NOTE = r.Field<string>("NOTE"),
                                             })
                                            .Select(g =>
                                            {
                                                var row = DtabPaymentPickupBillDetail.NewRow();
                                                row["SRNO"] = g.Key.SRNO;
                                                row["LEDGER_ID"] = g.Key.LEDGER_ID;
                                                row["LEDGERNAME"] = g.Key.LEDGERNAME;
                                                row["REFLEDGER_ID"] = g.Key.REFLEDGER_ID;
                                                row["ENTRYDATE"] = g.Key.ENTRYDATE;
                                                row["NOTE"] = g.Key.NOTE;
                                                row["CREDIT"] = g.Sum(r => r.Field<decimal>("CREDIT"));
                                                return row;
                                            }).CopyToDataTable();
                                        }

                                        int countno = GrdDetPayment.RowCount;
                                        foreach (DataRow DrNotFnd in TempDataTable.Rows)
                                        {
                                            if (DrNotFnd != null)
                                            {
                                                DataRow DRNotNew = DtabPaymentSummry.NewRow();

                                                DRNotNew["SRNO"] = countno = countno + 1;
                                                DRNotNew["LEDGER_ID"] = DrNotFnd["LEDGER_ID"];
                                                DRNotNew["LEDGERNAME"] = DrNotFnd["LEDGERNAME"];
                                                DRNotNew["REFLEDGER_ID"] = DrNotFnd["REFLEDGER_ID"];
                                                DRNotNew["VOUCHERDATE"] = DrNotFnd["ENTRYDATE"];
                                                DRNotNew["CREDITAMOUNT"] = 0;
                                                DRNotNew["CREDITAMOUNTFE"] = 0;
                                                DRNotNew["DEBITAMOUNT"] = DrNotFnd["DEBIT"];
                                                DRNotNew["DEBITAMOUNTFE"] = DrNotFnd["DEBIT"];
                                                DRNotNew["REFTYPE"] = "Agnst Ref";
                                                DRNotNew["EDTBL"] = "YES";
                                                DRNotNew["VOUCHERNOSTR"] = DrNotFnd["VOUCHERNOSTR"];
                                                DRNotNew["NOTE"] = DrNotFnd["NOTE"];

                                                DtabPaymentSummry.Rows.Add(DRNotNew);
                                                DtabPaymentSummry.AcceptChanges();
                                                    
                                                //DataRow[] foundRows = DtabPaymentPickupBillDetail.Select("VOUCHERNOSTR=" + "'" + DrNotFnd["VOUCHERNOSTR"] + "'");
                                                DataRow[] foundRows = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToString(row["ENTRYDATE"]) == Val.ToString(DrNotFnd["ENTRYDATE"]) && Val.ToInt32(row["MainGrdRow_id"]) == Val.ToInt32(DrNotFnd["SRNO"])).ToArray();
                                                if (foundRows.Length > 0)
                                                {
                                                    foundRows[0]["MainGrdRow_id"] = countno;
                                                }
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    //foreach (DataRow dr in DtabPaymentSummry.Rows)
                                    //{
                                    //    dr.Delete();
                                    //}

                                    DtabPaymentSummry.Rows.Clear();
                                    DtabPaymentSummry.AcceptChanges();
                                    DataTable dt = new DataTable();
                                                                                                            
                                    TempDataTable = DtabPaymentPickupBillDetail.Clone();
                                    if (BOConfiguration.gStrLoginSection == "B")
                                    {
                                        var DRow = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToString(row["ENTRYDATE"]) != "").ToArray();
                                        foreach (DataRow dr in DRow)
                                            dt = DtabPaymentPickupBillDetail.AsEnumerable().Where(rows1 => Val.ToString(rows1["ENTRYDATE"]) != "").CopyToDataTable();

                                        TempDataTable = dt.AsEnumerable()
                                        .GroupBy(r => new
                                        {
                                            SRNO = r.Field<int>("MainGrdRow_id"),
                                            ENTRYDATE = r.Field<string>("ENTRYDATE"),
                                            LEDGER_ID = r.Field<Guid>("LEDGER_ID"),
                                            LEDGERNAME = r.Field<string>("LEDGERNAME"),
                                            REFLEDGER_ID = r.Field<Guid>("REFLEDGER_ID"),
                                            NOTE = r.Field<string>("NOTE"),
                                            //TRN_ID = r.Field<Guid>("TRN_ID"),
                                            STATUS = r.Field<string>("STATUS"),
                                        })
                                        .Select(g =>
                                        {
                                            var row = DtabPaymentPickupBillDetail.NewRow();
                                            row["SRNO"] = g.Key.SRNO;
                                            row["LEDGER_ID"] = g.Key.LEDGER_ID;
                                            row["LEDGERNAME"] = g.Key.LEDGERNAME;
                                            row["REFLEDGER_ID"] = g.Key.REFLEDGER_ID;
                                            row["ENTRYDATE"] = g.Key.ENTRYDATE;
                                            row["NOTE"] = g.Key.NOTE;
                                            //row["TRN_ID"] = g.Key.TRN_ID;
                                            row["STATUS"] = g.Key.STATUS;
                                            row["CREDIT"] = g.Sum(r => r.Field<decimal>("CREDIT"));
                                            return row;
                                        }).CopyToDataTable();



                                        int countno = GrdDetPayment.RowCount;
                                        foreach (DataRow DrNotFnd in TempDataTable.Rows)
                                        {
                                            if (DrNotFnd != null)
                                            {
                                                DataRow DRNotNew = DtabPaymentSummry.NewRow();

                                                DRNotNew["SRNO"] = countno = countno + 1;
                                                DRNotNew["LEDGER_ID"] = DrNotFnd["LEDGER_ID"];
                                                DRNotNew["LEDGERNAME"] = DrNotFnd["LEDGERNAME"];
                                                DRNotNew["REFLEDGER_ID"] = DrNotFnd["REFLEDGER_ID"];
                                                DRNotNew["VOUCHERDATE"] = DrNotFnd["ENTRYDATE"];
                                                DRNotNew["CREDITAMOUNT"] = DrNotFnd["CREDIT"];
                                                DRNotNew["CREDITAMOUNTFE"] = DrNotFnd["CREDIT"];
                                                DRNotNew["DEBITAMOUNT"] = 0;
                                                DRNotNew["DEBITAMOUNTFE"] = 0;
                                                DRNotNew["REFTYPE"] = "Agnst Ref";
                                                DRNotNew["EDTBL"] = "YES";
                                                DRNotNew["NOTE"] = DrNotFnd["NOTE"];
                                                //DRNotNew["TRN_ID"] = DrNotFnd["TRN_ID"];
                                                DRNotNew["STATUS"] = DrNotFnd["STATUS"];

                                                DtabPaymentSummry.Rows.Add(DRNotNew);
                                                DtabPaymentSummry.AcceptChanges();

                                                //DataRow[] foundRows = DtabPaymentPickupBillDetail.Select("ENTRYDATE=" + "'" + DrNotFnd["ENTRYDATE"] + "'");
                                                DataRow[] foundRows = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToString(row["ENTRYDATE"]) == Val.ToString(DrNotFnd["ENTRYDATE"]) && Val.ToInt32(row["MainGrdRow_id"]) == Val.ToInt32(DrNotFnd["SRNO"])).ToArray();
                                                if (foundRows.Length > 0)
                                                {
                                                    foreach (DataRow drAdd in foundRows)
                                                    {
                                                        drAdd["MainGrdRow_id"] = countno;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var DRow = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToString(row["STATUS"]) == "").ToArray();
                                        foreach (DataRow dr in DRow)
                                            dt = DtabPaymentPickupBillDetail.AsEnumerable().Where(rows1 => Val.ToString(rows1["STATUS"]) == "").CopyToDataTable();

                                        //TempDataTable = dt;
                                        TempDataTable = dt.AsEnumerable()
                                        .GroupBy(r => new
                                        {
                                            SRNO = r.Field<int>("SRNO"),
                                            ENTRYDATE = r.Field<string>("ENTRYDATE"),
                                            LEDGER_ID = r.Field<Guid>("LEDGER_ID"),
                                            LEDGERNAME = r.Field<string>("LEDGERNAME"),
                                            REFLEDGER_ID = r.Field<Guid>("REFLEDGER_ID"),
                                            NOTE = r.Field<string>("NOTE"),
                                        })
                                        .Select(g =>
                                        {
                                            var row = DtabPaymentPickupBillDetail.NewRow();
                                            row["SRNO"] = g.Key.SRNO;
                                            row["LEDGER_ID"] = g.Key.LEDGER_ID;
                                            row["LEDGERNAME"] = g.Key.LEDGERNAME;
                                            row["REFLEDGER_ID"] = g.Key.REFLEDGER_ID;
                                            row["ENTRYDATE"] = g.Key.ENTRYDATE;
                                            row["NOTE"] = g.Key.NOTE;
                                            row["CREDIT"] = g.Sum(r => r.Field<decimal>("CREDIT"));
                                            return row;
                                        }).CopyToDataTable();

                                        int countno = GrdDetPayment.RowCount;
                                        foreach (DataRow DrNotFnd in TempDataTable.Rows)
                                        {
                                            if (DrNotFnd != null)
                                            {
                                                DataRow DRNotNew = DtabPaymentSummry.NewRow();

                                                DRNotNew["SRNO"] = countno = countno + 1;
                                                DRNotNew["LEDGER_ID"] = DrNotFnd["LEDGER_ID"];
                                                DRNotNew["LEDGERNAME"] = DrNotFnd["LEDGERNAME"];
                                                DRNotNew["REFLEDGER_ID"] = DrNotFnd["REFLEDGER_ID"];
                                                DRNotNew["VOUCHERDATE"] = DrNotFnd["ENTRYDATE"];
                                                DRNotNew["CREDITAMOUNT"] = DrNotFnd["CREDIT"];
                                                DRNotNew["CREDITAMOUNTFE"] = DrNotFnd["CREDIT"];
                                                DRNotNew["DEBITAMOUNT"] = 0;
                                                DRNotNew["DEBITAMOUNTFE"] = 0;
                                                DRNotNew["REFTYPE"] = "Agnst Ref";
                                                DRNotNew["EDTBL"] = "YES";
                                                DRNotNew["VOUCHERNOSTR"] = DrNotFnd["VOUCHERNOSTR"];
                                                DRNotNew["NOTE"] = DrNotFnd["NOTE"];

                                                DtabPaymentSummry.Rows.Add(DRNotNew);
                                                DtabPaymentSummry.AcceptChanges();

                                                //DataRow[] foundRows = DtabPaymentPickupBillDetail.Select("ENTRYDATE=" + "'" + DrNotFnd["ENTRYDATE"] + "'");
                                                DataRow[] foundRows = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToString(row["ENTRYDATE"]) == Val.ToString(DrNotFnd["ENTRYDATE"]) && Val.ToInt32(row["MainGrdRow_id"]) == Val.ToInt32(DrNotFnd["SRNO"])).ToArray();
                                                if (foundRows.Length > 0)
                                                {
                                                    foundRows[0]["MainGrdRow_id"] = countno;
                                                }
                                            }
                                        }
                                    }
                                    //GrdDetPayment.SetFocusedRowCellValue("DEBITAMOUNT", 0);
                                    //GrdDetPayment.SetFocusedRowCellValue("CREDITAMOUNT", decCredit - decDebit);
                                    //GrdDetPayment.SetFocusedRowCellValue("DEBITAMOUNTFE", 0);
                                    //GrdDetPayment.SetFocusedRowCellValue("CREDITAMOUNTFE", decCreditFe - decDebitFe);
                                    //GrdDetPayment.SetFocusedRowCellValue("EDTBL", "Yes");

                                    //GrdDetPayment.SetFocusedRowCellValue("DEBITAMOUNT", 0);
                                    //GrdDetPayment.SetFocusedRowCellValue("CREDITAMOUNT", Math.Round(Val.Val(decCredit - decDebit), 2, MidpointRounding.AwayFromZero));
                                    //GrdDetPayment.SetFocusedRowCellValue("DEBITAMOUNTFE", 0);
                                    //GrdDetPayment.SetFocusedRowCellValue("CREDITAMOUNTFE", Math.Round(Val.Val(decCreditFe - decDebitFe), 2, MidpointRounding.AwayFromZero));
                                    //GrdDetPayment.SetFocusedRowCellValue("EDTBL", "Yes");
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
                                    //Drow["DEBITAMOUNT"] = Math.Round(Val.Val(decBankDebit), 2, MidpointRounding.AwayFromZero);
                                    //Drow["CREDITAMOUNT"] = Math.Round(Val.Val(decBankCredit), 2, MidpointRounding.AwayFromZero);
                                    //Drow["DEBITAMOUNTFE"] = Math.Round(Val.Val(decBankDebitFe), 3, MidpointRounding.AwayFromZero);
                                    //Drow["CREDITAMOUNTFE"] = Math.Round(Val.Val(decBankCreditFe), 3, MidpointRounding.AwayFromZero);
                                    Drow["LEDGERNAME"] = DtBankCharges.Rows.Count > 0 ? Val.ToString(DtBankCharges.Rows[0]["LEDGERNAME"]) : "";
                                    Drow["LEDGER_ID"] = DtBankCharges.Rows.Count > 0 ? Val.ToString(DtBankCharges.Rows[0]["LEDGER_ID"]) : Val.ToString(Guid.Empty);
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
                                    Drow["LEDGERNAME"] = DtDiscount.Rows.Count > 0 ? Val.ToString(DtDiscount.Rows[0]["LEDGERNAME"]) : "";
                                    Drow["LEDGER_ID"] = DtDiscount.Rows.Count > 0 ? Val.ToString(DtDiscount.Rows[0]["LEDGER_ID"]) : Val.ToString(Guid.Empty);
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
                                    Drow["LEDGERNAME"] = DtExchRateDiff.Rows.Count > 0 ? Val.ToString(DtExchRateDiff.Rows[0]["LEDGERNAME"]) : "";
                                    Drow["LEDGER_ID"] = DtExchRateDiff.Rows.Count > 0 ? Val.ToString(DtExchRateDiff.Rows[0]["LEDGER_ID"]) : Val.ToString(Guid.Empty);
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
                                    Drow["LEDGERNAME"] = DtExchRateDiff.Rows.Count > 0 ? Val.ToString(DtExchRateDiff.Rows[0]["LEDGERNAME"]) : ""; 
                                    Drow["LEDGER_ID"] = DtExchRateDiff.Rows.Count > 0 ? Val.ToString(DtExchRateDiff.Rows[0]["LEDGER_ID"]) : Val.ToString(Guid.Empty);
                                    Drow["REFTYPE"] = "New Ref";
                                    Drow["REFLEDGER_ID"] = Val.ToString(GrdDetPayment.GetFocusedRowCellValue("LEDGER_ID"));
                                    Drow["EDTBL"] = "No";
                                    DtabPaymentSummry.Rows.Add(Drow);
                                }
                                MainGridDetail.DataSource = DtabPaymentPickupBillDetail;
                                MainGridDetail.RefreshDataSource();

                                MainGrdPayment.DataSource = DtabPaymentSummry;
                                MainGrdPayment.RefreshDataSource();
                                calculate();

                                //Add shiv
                                if (BOConfiguration.gStrLoginSection == "B")
                                {
                                    BtnSave_Click(null, null);
                                    BtnAddRow_Click(null, null);
                                }
                                //MainGrid.RefreshDataSource();
                            }
                        }
                        else
                        {
                            //Global.MessageError("Auto Generated Row Cant Be Allocated");
                            //GrdDet.FocusedRowHandle = GrdDet.GetFocusedDataSourceRowIndex();
                            //GrdDet.FocusedColumn = GrdDet.VisibleColumns[GrdDet.Columns["LEDGERNAME"].VisibleIndex];
                            //GrdDet.ShowEditor();
                        }
                    }
                    else
                    {
                        Global.MessageError("Party Is Required");
                        //GrdDet.FocusedRowHandle = GrdDet.GetFocusedDataSourceRowIndex();
                        //GrdDet.FocusedColumn = GrdDet.VisibleColumns[GrdDet.Columns["LEDGERNAME"].VisibleIndex];
                        //GrdDet.ShowEditor();
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
            else if (Val.ToInt32(RdoCurrency.SelectedIndex.ToString()) == 0 && (strColName == "DEBITAMOUNTFE" || strColName == "CREDITAMOUNTFE") && strRefType == "New Ref")
                e.Cancel = true;
            else if (Val.ToInt32(RdoCurrency.SelectedIndex.ToString()) == 1 && (strColName == "DEBITAMOUNT" || strColName == "CREDITAMOUNT") && strRefType == "New Ref")
                e.Cancel = true;
        }

        private void cmbPaymentMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPaymentMode.Text == "Cash")
            {
                lblChqIssue.Text = "Cash Date";
                lblChqNo.Text = "Cash No.";
                GrdDetPayment.Columns["CHQISSUEDT"].Visible = false;
                GrdDetPayment.Columns["Chq_no"].Visible = false;
            }
            else if (cmbPaymentMode.Text == "Cheque")
            {
                lblChqIssue.Text = "Chq Issue";
                lblChqNo.Text = "Chq No.";
                 string Str = Val.Left(Val.ToString(CmbPaymentType.SelectedItem), 2);
                 if (Str == "BR")
                 {
                     GrdDetPayment.Columns["CHQISSUEDT"].Visible = true;
                     GrdDetPayment.Columns["Chq_no"].Visible = true;
                     GrdDetPayment.Columns["CHQISSUEDT"].VisibleIndex = 5;
                     GrdDetPayment.Columns["Chq_no"].VisibleIndex = 6;
                 }
                 else
                 {
                     GrdDetPayment.Columns["CHQISSUEDT"].Visible = false;
                     GrdDetPayment.Columns["Chq_no"].Visible = false;
                 }
            }
            else if (cmbPaymentMode.Text == "IMPS")
            {
                lblChqIssue.Text = "IMPS Date";
                lblChqNo.Text = "IMPS No.";
                GrdDetPayment.Columns["CHQISSUEDT"].Visible = false;
                GrdDetPayment.Columns["Chq_no"].Visible = false;
            }
            else if (cmbPaymentMode.Text == "RTGS")
            {
                lblChqIssue.Text = "RTGS Date";
                lblChqNo.Text = "RTGS No.";
                GrdDetPayment.Columns["CHQISSUEDT"].Visible = false;
                GrdDetPayment.Columns["Chq_no"].Visible = false;
            }
            else if (cmbPaymentMode.Text == "NEFT")
            {
                lblChqIssue.Text = "NETF Date";
                lblChqNo.Text = "NETF No.";
                GrdDetPayment.Columns["CHQISSUEDT"].Visible = false;
                GrdDetPayment.Columns["Chq_no"].Visible = false;
            }
            else if (cmbPaymentMode.Text == "Others")
            {
                lblChqIssue.Text = "Others Date";
                lblChqNo.Text = "Others No.";
                GrdDetPayment.Columns["CHQISSUEDT"].Visible = false;
                GrdDetPayment.Columns["Chq_no"].Visible = false;
            }
        }

        private void GrdDetPayment_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int IntBillCurId;
                decimal DecAmt, DecExcRate;
                Guid TRNID;
                if (e.RowHandle < 0)
                    return;
                //Kuldeep 19012021
                if(BOConfiguration.gStrLoginSection != "B")
                {
                   if (IntAllowToCalculate != 0)
                    return;
                }
                   
                if (txtExcRate.Text == string.Empty || txtExcRate.Text == "")
                    txtExcRate.Text = "1";

                string Str = Val.Left(Val.ToString(CmbPaymentType.SelectedItem), 2);
                switch (Val.ToString(e.Column.FieldName))
                {
                    case "DEBITAMOUNT":
                        if (RdoCurrency.SelectedIndex.ToString() == "1")
                        {
                            GrdDetPayment.PostEditor();
                            IntBillCurId = Val.ToInt32(GrdDetPayment.GetFocusedRowCellValue("CURRENCY_ID"));
                            DecExcRate = Val.ToDecimal(txtExcRate.Text);
                            DataRow Dr = GrdDetPayment.GetDataRow(e.RowHandle);

                            DecAmt = Math.Round(Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("DEBITAMOUNT")),3);

                            GrdDetPayment.SetFocusedRowCellValue("DEBITAMOUNTFE", DecExcRate * DecAmt);
                        }
                        break;

                    case "CREDITAMOUNT":
                        if (RdoCurrency.SelectedIndex.ToString() == "1")
                        {
                            GrdDetPayment.PostEditor();
                            DecExcRate = Val.ToDecimal(txtExcRate.Text);

                            DataRow Dr = GrdDetPayment.GetDataRow(e.RowHandle);
                            DecAmt = Math.Round(Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("CREDITAMOUNT")),3);
                            GrdDetPayment.SetFocusedRowCellValue("CREDITAMOUNTFE", DecExcRate * DecAmt);
                        }
                        break;
                    case "DEBITAMOUNTFE":
                        if (RdoCurrency.SelectedIndex.ToString() != "1")
                        {
                            GrdDetPayment.PostEditor();
                            DecExcRate = Val.ToDecimal(txtExcRate.Text);

                            DataRow Dr = GrdDetPayment.GetDataRow(e.RowHandle);
                            DecAmt = Math.Round(Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("DEBITAMOUNTFE")),3);

                            if (DecAmt != 0)
                            {
                                GrdDetPayment.SetFocusedRowCellValue("DEBITAMOUNT", Math.Round(DecAmt / DecExcRate, 3));
                            }
                        }
                        break;
                    case "CREDITAMOUNTFE":
                        if (RdoCurrency.SelectedIndex.ToString() != "1")
                        {
                            GrdDetPayment.PostEditor();
                            DecExcRate = Val.ToDecimal(txtExcRate.Text);

                            DataRow Dr = GrdDetPayment.GetDataRow(e.RowHandle);
                            DecAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("CREDITAMOUNTFE"));

                            if (DecAmt != 0)
                            {
                                GrdDetPayment.SetFocusedRowCellValue("CREDITAMOUNT", Math.Round(DecAmt / DecExcRate, 3));
                            }
                        }
                        break;
                    case "VOUCHERDATE":
                         DataRow Drow = GrdDetPayment.GetDataRow(e.RowHandle);

                         if (Val.ToString(Drow["TRN_ID"]) == String.Empty)
                         {
                         //    if (BOConfiguration.gStrLoginSection == "B")
                         //    {
                                 
                         //        //DataSet DSet = new DataSet();

                         //        //DSet = objLedgerTrn.GetBillWisePaymentGetDataNew("DETAIL", "", "", Val.ToString(Drow["BOOKTYPEFULL"]), Guid.Parse(Val.ToString(Drow["TRN_ID"])));
                         //        //if (DSet.Tables.Count <= 0)
                         //        //{
                         //        //    this.Cursor = Cursors.Default;
                         //        //    Global.Message("Some Thing Went Wrong");
                         //        //    MainGrid.DataSource = null;
                         //        //    return;
                         //        //}
                         //        //DSet.DataSetName = "DocumentElement";
                         //        //if (DSet.Tables.Count == 3)
                         //        //{
                         //        //    DtabPaymentPickupBillDetail = DSet.Tables[0];
                         //        //    DtabPaymentSummry = DSet.Tables[1];
                         //        //    DtabPaymentDetail = DSet.Tables[2];
                         //        //    LblMode.Text = "Edit Mode";

                         //        //    if (!DtabPaymentSummry.Columns.Contains("CLOSINGBALANCE"))
                         //        //    {
                         //        //        DtabPaymentSummry.Columns.Add("CLOSINGBALANCE");
                         //        //    }

                         //        //    for (int i = 0; i < DtabPaymentSummry.Rows.Count; i++)
                         //        //        DtabPaymentSummry.Rows[i]["CLOSINGBALANCE"] = Global.FindLedgerClosingStr(Val.ToString(DtabPaymentSummry.Rows[i]["LEDGER_ID"]).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(DtabPaymentSummry.Rows[i]["LEDGER_ID"])));

                         //        //    for (int j = 0; j < DtabPaymentSummry.Rows.Count; j++)
                         //        //    {
                         //        //        DtabPaymentSummry.Rows[j]["STATUS"] = 1;
                         //        //    }

                         //        //    //lblBalance.Text = Global.FindLedgerClosingStr(Val.ToString(txtCashBankAC.Tag).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtCashBankAC.Tag)));



                         //        //    if ((Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH PAYMENT") || (Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK PAYMENT"))
                         //        //        CmbPaymentType.SelectedIndex = 0;
                         //        //    RdoPayType.SelectedIndex = 0;

                         //        //    if ((Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH RECEIPT") || (Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK RECEIPT"))
                         //        //        CmbPaymentType.SelectedIndex = 1;
                         //        //    RdoPayType.SelectedIndex = 1;

                         //        //    if (Val.ToInt32(Drow["CONVERTTOINR"]) == 1)
                         //        //        ChkBxConvertToInr.Checked = true;
                         //        //    else
                         //        //        ChkBxConvertToInr.Checked = false;

                         //        //    if (Val.ToInt32(Drow["CURRENCY_ID"]) == 1)
                         //        //    {
                         //        //        if (Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH PAYMENT" || Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK PAYMENT")
                         //        //            txtAmount.Text = Val.ToString(Drow["CREDITAMOUNT"]);
                         //        //        else
                         //        //            txtAmount.Text = Val.ToString(Drow["DEBITAMOUNT"]);
                         //        //    }
                         //        //    else
                         //        //    {
                         //        //        if (Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH PAYMENT" || Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK PAYMENT")
                         //        //            txtAmount.Text = Val.ToString(Drow["CREDITAMOUNTFE"]);
                         //        //        else
                         //        //            txtAmount.Text = Val.ToString(Drow["DEBITAMOUNTFE"]);
                         //        //    }

                         //        //    if (Val.ToString(RdoCurrency.SelectedIndex.ToString()) == "1")
                         //        //    { ChkBxConvertToInr.Visible = true; lblAmount.Text = "Amount ($)"; }
                         //        //    else
                         //        //    { ChkBxConvertToInr.Visible = false; lblAmount.Text = "Amount (₹)"; }

                         //        //    MainGrdPayment.DataSource = DtabPaymentSummry;
                         //        //    MainGrdPayment.Refresh();

                         //        //    MainGridDetail.DataSource = DtabPaymentPickupBillDetail;
                         //        //    MainGridDetail.Refresh();


                         //        //    DataTable DtCheckAllocated = ObjFinance.CheckBillAllocatedOrNot(txtTrnID.Text, "");
                         //        //    if (DtCheckAllocated.Rows.Count > 0)
                         //        //    {
                         //        //        LblBillPickupMessage.Text = "This Entry Allocated In Accounts ( " + Val.ToString(DtCheckAllocated.Rows[0][0]) + " ) ,Please Delete For Edit.";
                         //        //        BtnSave.Enabled = false;
                         //        //        BtnDelete.Enabled = false;
                         //        //        LblMode.Text = "View Mode";
                         //        //    }
                         //        //    else
                         //        //    {
                         //        //        LblBillPickupMessage.Text = "";
                         //        //        BtnSave.Enabled = true;
                         //        //        BtnDelete.Enabled = true;
                         //        //        LblMode.Text = "Edit Mode";
                         //        //    }

                         //        //    DTBal = new BusLib.Account.BOLedgerTransaction().FindLedgerClosingNew(Val.ToString(txtCashBankAC.Tag).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtCashBankAC.Tag)));
                         //        //    if (DTBal.Rows.Count > 0)
                         //        //    {
                         //        //        if (RdoCurrency.SelectedIndex.ToString() == "1")
                         //        //        {
                         //        //            if (Val.ToDouble(DTBal.Rows[0]["CloseBalanceDollar"]) < 0)
                         //        //            {
                         //        //                lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Dr";
                         //        //            }
                         //        //            else
                         //        //            {
                         //        //                lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Cr";
                         //        //            }
                         //        //            //lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]);
                         //        //        }
                         //        //        else
                         //        //        {
                         //        //            if (Val.ToDouble(DTBal.Rows[0]["CloseBalanceRs"]) < 0)
                         //        //            {
                         //        //                lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Dr";
                         //        //            }
                         //        //            else
                         //        //            {
                         //        //                lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Cr";
                         //        //            }
                         //        //            //lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceRs"]); }
                         //        //        }
                         //        //    }

                         //        //}
                         //        //calculate();
                         //        //BtnSave_Click(null, null);
                         //        AUTOSAVE = false;
                         //    }
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
            DataTable dt = new DataTable();

            var DRow = DtabPaymentSummry.Rows.Cast<DataRow>().Where(row => Val.ToString(row["STATUS"]) == "" || Val.ToString(row["STATUS"]) == "1").ToArray();
            foreach (DataRow dr in DRow)
                dt = DtabPaymentSummry.AsEnumerable().Where(rows1 => Val.ToString(rows1["STATUS"]) == "" || Val.ToString(rows1["STATUS"]) == "1").CopyToDataTable();


            if (RdoCurrency.SelectedIndex.ToString() == "1")
            {
                decTotalDeb = Val.ToDecimal(dt.Compute("SUM(DEBITAMOUNT)", string.Empty));
                decTotalCrd = Val.ToDecimal(dt.Compute("SUM(CREDITAMOUNT)", string.Empty));
                string Str = Val.Left(Val.ToString(CmbPaymentType.SelectedItem), 2);
                if (Str == "CP" || Str == "BP")
                {
                   // txtAmount.Text = Val.ToString(decTotalDeb - decTotalCrd);
                    double Total = 0.000;
                    Total = Val.ToDouble(decTotalDeb) - Val.ToDouble(decTotalCrd);
                    txtAmount.Text = Val.ToString(Total);
                }
                else
                {
                    //txtAmount.Text = Val.ToString(decTotalCrd - decTotalDeb);
                    double Total = 0.000;
                    Total = Val.ToDouble(decTotalCrd) - Val.ToDouble(decTotalDeb);
                    txtAmount.Text = Val.ToString(Total);
                }
            }
            else
            {
                decTotalDeb = Val.ToDecimal(dt.Compute("SUM(DEBITAMOUNTFE)", string.Empty));
                decTotalCrd = Val.ToDecimal(dt.Compute("SUM(CREDITAMOUNTFE)", string.Empty));
                string Str = Val.Left(Val.ToString(CmbPaymentType.SelectedItem), 2);
                if (Str == "CP" || Str == "BP")
                {
                    double Total = 0.000;
                    Total = Val.ToDouble(decTotalDeb) - Val.ToDouble(decTotalCrd);
                    txtAmount.Text = Val.ToString(Total);
                }
                else
                {
                    double Total = 0.000;
                    Total = Val.ToDouble(decTotalCrd) - Val.ToDouble(decTotalDeb);
                    txtAmount.Text = Val.ToString(Total);
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
                    if (DTPEntryDate.Value < DatFromDate || DTPEntryDate.Value > DatToDate)
                    {
                        if (DateTime.Now.Date > DatToDate)
                            DTPEntryDate.Value = DatToDate;
                        else
                            DTPEntryDate.Value = DateTime.Now.Date;
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

        private void reptxtRemark_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.KeyCode == Keys.Enter)
                //{
                //    DataRow dr = GrdDetPayment.GetFocusedDataRow();
                //    if (!Val.ToString(dr["SRNO"]).Equals(string.Empty))
                //    {
                //        if (Val.ToString(GrdDetPayment.GetFocusedRowCellValue("LEDGERNAME")) != "")
                //        {
                //            if (BOConfiguration.gStrLoginSection == "B")
                //            {
                //                //calculate();
                //                //BtnSave_Click(null, null);
                //                //if (AUTOSAVE == true)
                //                //{
                //                //    BtnAddRow_Click(null, null);
                //                //}
                //            }
                //            //else
                //            //{ BtnAddRow_Click(null, null); }
                //        }
                //    }
                //    else if (GrdDetPayment.IsLastRow)
                //    {
                //        e.Handled = true;
                //        BtnSave.Focus();
                //        BtnSave.PerformClick();
                //    }
                //}
                //if (EditModeDateChange == true)
                //{
                //    calculate();
                //    BtnSave_Click(null, null);
                //    EditModeDateChange = false;
                //}
                //else
                //{
                //BtnAddRow_Click(null, null);
                //}
                if (BOConfiguration.gStrLoginSection == "B")
                {
                    BtnAddRow_Click(null, null);
                }
                else {
                    if (e.KeyCode == Keys.Enter)
                    {
                        DataRow dr = GrdDetPayment.GetFocusedDataRow();
                        if (!Val.ToString(dr["SRNO"]).Equals(string.Empty))
                        {
                            if (Val.ToString(GrdDetPayment.GetFocusedRowCellValue("LEDGERNAME")) != "")
                            {
                                BtnAddRow_Click(null, null);
                            }

                            //DataRow[] foundRows = DtabPaymentPickupBillDetail.Select("SRNO=" + "'" + Val.ToString(dr["SRNO"]) + "'" && "VOUCHERDATE=" + "'" + Val.ToString(dr["VOUCHERDATE"]) + "'");
                            DataRow[] foundRows = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToString(row["MainGrdRow_id"]) == Val.ToString(dr["SRNO"]) && Val.ToString(row["ENTRYDATE"]) == Val.ToString(dr["VOUCHERDATE"])).ToArray();
                            if (foundRows.Length > 0)
                            {
                                foreach (DataRow drAdd in foundRows)
                                {
                                    drAdd["NOTE"] = Val.ToString(dr["NOTE"]);
                                }
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

        private void RdoPayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (BOConfiguration.gStrLoginSection == "B")
                {
                    if (RdoPayType.SelectedIndex == 0)
                    {
                        CmbPaymentType.Refresh();
                        CmbPaymentType.Items.Remove("CR - CASH RECEIVE");
                        CmbPaymentType.Items.Remove("BR - BANK RECEIVE");
                        CmbPaymentType.Items.Add("CP - CASH PAYMENT");
                        CmbPaymentType.Items.Add("BP - BANK PAYMENT");
                        CmbPaymentType.SelectedIndex = 0;
                    }
                    else
                    {
                        CmbPaymentType.Refresh();
                        CmbPaymentType.Items.Add("CR - CASH RECEIVE");
                        CmbPaymentType.Items.Add("BR - BANK RECEIVE");
                        CmbPaymentType.Items.Remove("CP - CASH PAYMENT");
                        CmbPaymentType.Items.Remove("BP - BANK PAYMENT");
                        CmbPaymentType.SelectedIndex = 0;
                    }
                }
                else
                {
                    if (RdoPayType.SelectedIndex == 0)
                    {
                        CmbPaymentType.Refresh();
                        //CmbPaymentType.Items.Remove("CR - CASH RECEIVE");
                        CmbPaymentType.Items.Remove("BR - BANK RECEIVE");
                        //CmbPaymentType.Items.Add("CP - CASH PAYMENT");
                        CmbPaymentType.Items.Add("BP - BANK PAYMENT");
                        CmbPaymentType.SelectedIndex = 0;
                    }
                    else
                    {
                        CmbPaymentType.Refresh();
                        //CmbPaymentType.Items.Add("CR - CASH RECEIVE");
                        CmbPaymentType.Items.Add("BR - BANK RECEIVE");
                        //CmbPaymentType.Items.Remove("CP - CASH PAYMENT");
                        CmbPaymentType.Items.Remove("BP - BANK PAYMENT");
                        CmbPaymentType.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void RdoCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Val.ToString(RdoCurrency.SelectedIndex.ToString()) == "1")
            { 
              ChkBxConvertToInr.Visible = true; lblAmount.Text = "Amount ($)";
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
                ChkBxConvertToInr.Visible = false; lblAmount.Text = "Amount (₹)";
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

            if (BOConfiguration.gStrLoginSection == "B")
            {
                txtExcRate.Text = "1.000";
            }
            else
            {
                txtExcRate.Text = new BOTRN_MemoEntry().GetExchangeRate(Val.ToInt(RdoCurrency.SelectedIndex.ToString()), Val.SqlDate(DTPEntryDate.Value.ToShortDateString()), mFormType.ToString()).ToString();
            }
            txtExcRate_Validated(null, null);
        }

        private void BtnSearch_Click_1(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (txtPartyName.Text.Length == 0) txtPartyName.Tag = null;
                DataSet DSetSummary = new DataSet();
                //Added by Daksha on 12/09/2023
                string FromDate = "", ToDate = "";
                if (DTPSFromDate.Checked)
                {
                    FromDate = Val.SqlDate(DTPSFromDate.Text);
                }
                if (DTPSToDate.Checked)
                {
                    ToDate = Val.SqlDate(DTPSToDate.Text);
                }
                //End as Daksha
                DSetSummary = objLedgerTrn.GetBillWisePaymentGetDataNew("SUMMARY", FromDate, ToDate, "", Guid.Empty, 
                    txtPartyName.Text == string.Empty ? Guid.Empty : Val.ToGuid(Val.ToString(txtPartyName.Tag)), txtSearchVoucher.Text, 
                    Val.ToString(ChkcmbPaymentTypeSearch.EditValue),
                    Val.ToInt32(RGSearchCurrency.EditValue));
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

        private void GrdDet_RowClick(object sender, RowClickEventArgs e)
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

                    //DSet = objLedgerTrn.GetBillWisePaymentGetDataNew("DETAIL", "", "", Val.ToString(Drow["BOOKTYPEFULL"]), Guid.Parse(Val.ToString(Drow["TRN_ID"])), ""); //Comment by Daksha on 13/09/2023
                    DSet = objLedgerTrn.GetBillWisePaymentGetDataNew("DETAIL", "", "", Val.ToString(Drow["BOOKTYPEFULL"]),
                        Val.ToString(Drow["VOUCHERNOSTR"]) == string.Empty ? Val.ToGuid(Val.ToString(Drow["TRN_ID"])) : Guid.Empty, Guid.Empty, Val.ToString(Drow["VOUCHERNOSTR"])); //Changes by Daksha on 13/09/2023
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

                        if (LblMode.Text == "Edit Mode")
                        {
                            for (int j = 0; j < DtabPaymentSummry.Rows.Count; j++)
                            {
                                DtabPaymentSummry.Rows[j]["STATUS"] = "";
                            }
                        }

                        //lblBalance.Text = Global.FindLedgerClosingStr(Val.ToString(txtCashBankAC.Tag).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtCashBankAC.Tag)));


                        if (BOConfiguration.gStrLoginSection != "B")
                        {
                            //Added & Comment by Daksha on 10/04/2023
                            RdoPayType.SelectedIndex = Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK PAYMENT" ? 0 : 1; //0-Payment 1-Receive
                            
                            //Old Code
                            //if (Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK PAYMENT")
                            //    CmbPaymentType.SelectedIndex = 1;
                            //RdoPayType.SelectedIndex = 1;
                            //End as Daksha
                        }
                        else
                        {
                            if ((Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH PAYMENT") || (Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK PAYMENT"))
                            {
                                CmbPaymentType.SelectedIndex = 0;
                                RdoPayType.SelectedIndex = 0;
                            }
                            else if ((Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH RECEIPT") || (Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK RECEIPT"))
                            {
                                CmbPaymentType.SelectedIndex = 1;
                                RdoPayType.SelectedIndex = 1;
                            } 
                        }

                        if (Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK RECEIPT")
                        {
                            CmbPaymentType.Text = "BR - BANK RECEIVE";
                        }
                        else if (Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH RECEIPT")
                        {
                            CmbPaymentType.Text = "CR - CASH RECEIVE";
                        }
                        else if (Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK PAYMENT")
                        {
                            CmbPaymentType.Text = "BP - BANK PAYMENT";
                        }
                        else if (Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH PAYMENT")
                        {
                            CmbPaymentType.Text = "CP - CASH PAYMENT";
                        }

                        if (Val.ToInt32(Drow["CURRENCY_ID"]) == 1)
                        {
                            RdoCurrency.SelectedIndex = 1;
                        }
                        else {
                            RdoCurrency.SelectedIndex = 0;
                        }

                        //txtTrnID.Text = Val.ToString(Drow["TRN_ID"]);                        
                        DTPEntryDate.Text = Val.ToString(Drow["ENTRYDATE"]);
                        txtVoucherNo.Text = Val.ToString(Drow["VOUCHERNO"]);
                        txtVoucherStr.Text = Val.ToString(Drow["VOUCHERNOSTR"]);
                        txtCurrency.Tag = Val.ToString(Drow["CURRENCY_ID"]);
                        txtCurrency.Text = Val.ToString(Drow["CURRENCYNAME"]);
                        txtExcRate.Text = Val.ToString(Drow["EXCRATE"]);
                        txtRemark.Text = Val.ToString(Drow["NOTE"]);
                        txtChqNo.Text = Val.ToString(Drow["CHQ_NO"]);
                        DtpChqIssue.Text = Val.ToString(Drow["CHQISSUEDT"]);
                        cmbPaymentMode.SelectedItem = Val.ToString(Drow["PAYTYPE"]);

                        //Added & Comment by Daksha on 10/04/2023
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


                        //Added by Daksha on 13/09/2023
                        if (Val.ToInt32(Drow["CURRENCY_ID"]) == 1)
                        {
                            if (Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH PAYMENT" || Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK PAYMENT")                                
                                txtAmount.Text = Val.ToString(DtabPaymentSummry.Compute("SUM(DEBITAMOUNT)", ""));
                            else                                
                                txtAmount.Text = Val.ToString(DtabPaymentSummry.Compute("SUM(CREDITAMOUNT)", ""));
                        }
                        else
                        {
                            if (Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH PAYMENT" || Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK PAYMENT")                                
                                txtAmount.Text = Val.ToString(DtabPaymentSummry.Compute("SUM(DEBITAMOUNTFE)", ""));
                            else                                
                                txtAmount.Text = Val.ToString(DtabPaymentSummry.Compute("SUM(CREDITAMOUNTFE)", ""));
                        }
                        //End as Daksha

                        //Old Code
                        //Added by Daksha on 10/04/2023 //For Bank payment-dr and For Party Payment-cr
                        //if (Val.ToInt32(Drow["CURRENCY_ID"]) == 1)
                        //{
                        //    if (Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH PAYMENT" || Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK PAYMENT")
                        //        txtAmount.Text = Val.ToString(Drow["DEBAMT"]);
                        //    else
                        //        txtAmount.Text = Val.ToString(Drow["CRDAMT"]);
                        //}
                        //else
                        //{
                        //    if (Val.ToString(Drow["BOOKTYPEFULL"]) == "CASH PAYMENT" || Val.ToString(Drow["BOOKTYPEFULL"]) == "BANK PAYMENT")
                        //        txtAmount.Text = Val.ToString(Drow["DEBAMTFE"]);                                
                        //    else
                        //        txtAmount.Text = Val.ToString(Drow["CRDAMTFE"]);
                        //}

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

                        if (Val.ToString(RdoCurrency.SelectedIndex.ToString()) != "1")
                        { ChkBxConvertToInr.Visible = true; lblAmount.Text = "Amount ($)"; }
                        else
                        { ChkBxConvertToInr.Visible = false; lblAmount.Text = "Amount (₹)"; }

                        MainGrdPayment.DataSource = DtabPaymentSummry;
                        MainGrdPayment.Refresh();

                        MainGridDetail.DataSource = DtabPaymentPickupBillDetail;
                        MainGridDetail.Refresh();
                        
                        for (int i = 0; i < DtabPaymentSummry.Rows.Count; i++) //Added by Daksha on 13/09/2023
                        {
                            DataTable DtCheckAllocated = ObjFinance.CheckBillAllocatedOrNot(Val.ToString(DtabPaymentSummry.Rows[i]["ACCLEDGTRNTRN_ID"]), "");
                            //DataTable DtCheckAllocated = ObjFinance.CheckBillAllocatedOrNot(txtTrnID.Text, "");
                            if (DtCheckAllocated.Rows.Count > 0)
                            {
                                LblBillPickupMessage.Text = "This Entry Allocated In Accounts ( " + Val.ToString(DtCheckAllocated.Rows[0][0]) + " ) ,Please Delete For Edit.";
                                BtnSave.Enabled = false;
                                BtnDelete.Enabled = false;
                                LblMode.Text = "View Mode";
                                break;
                            }
                            else
                            {
                                LblBillPickupMessage.Text = "";
                                BtnSave.Enabled = true;
                                BtnDelete.Enabled = true;
                                LblMode.Text = "Edit Mode";
                            }

                        }
                        
                        DTBal = new BusLib.Account.BOLedgerTransaction().FindLedgerClosingNew(Val.ToString(txtCashBankAC.Tag).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtCashBankAC.Tag)));
                        if (DTBal.Rows.Count > 0)
                        {
                            if (RdoCurrency.SelectedIndex.ToString() == "1")
                            {
                                if (Val.ToDouble(DTBal.Rows[0]["CloseBalanceDollar"]) < 0)
                                {
                                    lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Dr";
                                }
                                else
                                {
                                    lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]).ToString() + " Cr";
                                }
                                //lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceDollar"]);
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
                                //lblBalance.Text = Val.ToString(DTBal.Rows[0]["CloseBalanceRs"]); }
                            }
                        }
                        if(LblMode.Text == "Edit Mode")
                        {
                            GrdDetail.Columns["CREDIT"].OptionsColumn.AllowEdit = true;
                            GrdDetail.Columns["CREDITFE"].OptionsColumn.AllowEdit = true;
                            GrdDetail.Columns["CREDIT"].OptionsColumn.ReadOnly = false;
                            GrdDetail.Columns["CREDITFE"].OptionsColumn.ReadOnly = false;
                            GrdDetail.Columns["INVAMOUNT"].Visible = true;                            
                        }
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

        private void txtCashBankAC_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    if (txtCashBankAC.Text != "")
                    {
                        BtnAddRow_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDetPayment_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (GrdDetPayment.FocusedColumn.FieldName.ToString() == "LEDGERNAME")
                {
                    if (e.KeyCode == Keys.Tab)
                    {
                        if (Val.ToString(GrdDetPayment.GetFocusedRowCellValue("LEDGERNAME")) != "")
                        {
                            rspBtnBillAllocation_Click(null, null);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void FrmPaymentRemittance_Load(object sender, EventArgs e)
        {
            try
            {
                if (BOConfiguration.gStrLoginSection == "B")
                { 
                    BtnSave.Enabled = false;
                    lblExcRate.Visible = false;
                    lblExcRateStar.Visible = false;
                    txtExcRate.Visible = false;
                }
                else 
                { 
                    BtnSave.Enabled = true;
                    lblExcRate.Visible = true;
                    lblExcRateStar.Visible = true;
                    txtExcRate.Visible = true;                    
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void deleteSelectedAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdDetPayment.FocusedRowHandle >= 0)
                {
                    if (Global.Confirm("ARE YOU SURE YOU WANT TO DELETE ENTRY") == System.Windows.Forms.DialogResult.Yes)
                    {
                        this.Cursor = Cursors.WaitCursor;
                        LedgerTransactionProperty Property = new LedgerTransactionProperty();
                        Property.Trn_ID = LblMode.Text == "Add Mode" ? Val.ToGuid(GrdDetPayment.GetFocusedRowCellValue("TRN_ID")) : Val.ToGuid(GrdDetPayment.GetFocusedRowCellValue("ACCLEDGTRNTRN_ID"));
                        Property = objLedgerTrn.DeleteNew(Property);
                        
                        var DRows = DtabPaymentSummry.Rows.Cast<DataRow>().Where(row => Val.ToGuid(row["TRN_ID"]) == Property.Trn_ID || Val.ToGuid(row["ACCLEDGTRNTRN_ID"]) == Property.Trn_ID).ToArray();
                        foreach (DataRow dr in DRows)
                            DtabPaymentSummry.Rows.Remove(dr);

                        MainGrdPayment.DataSource = DtabPaymentSummry;
                        MainGrdPayment.Refresh();
                        if (Property.Trn_ID != Guid.Empty)
                        {
                            Global.Message(Property.ReturnMessageDesc);
                            if (Property.ReturnMessageType == "SUCCESS")
                            {
                                if (BOConfiguration.gStrLoginSection != "B")
                                {
                                    BtnClear_Click(null, null);
                                }
                                else
                                { BtnSave.Enabled = false; }
                            }
                        }
                        


                        //if (Val.ToString(GrdDetPayment.GetFocusedRowCellValue("TRN_ID")) == "")
                        //{
                        //    var DRows = DtabPaymentSummry.Rows.Cast<DataRow>().Where(row => Val.ToString(row["SRNO"]) == "").ToArray();
                        //    foreach (DataRow dr in DRows)
                        //        DtabPaymentSummry.Rows.Remove(dr);

                        //    var DRows1 = DtabPaymentSummry.Rows.Cast<DataRow>().Where(row => Val.ToString(row["SRNO"]) == "" && Val.ToString(row["TRN_ID"]) == "").ToArray();                            
                        //    foreach (DataRow dr in DRows1)
                        //        DtabPaymentSummry.Rows.Remove(dr);

                        //    MainGrdPayment.DataSource = DtabPaymentSummry;
                        //    MainGrdPayment.Refresh();
                        //}
                        //else
                        //{                            
                        //    //LedgerTransactionProperty Property = new LedgerTransactionProperty();                           
                        //    //Property.Trn_ID = Guid.Parse(Val.ToString(GrdDetPayment.GetFocusedRowCellValue("TRN_ID")));
                        //    Property.Trn_ID = Guid.Parse(Val.ToString(GrdDetPayment.GetFocusedRowCellValue("ACCLEDGTRNTRN_ID")));
                        //    //if (AUTOSAVE == true)
                        //    //{
                        //        Property = objLedgerTrn.DeleteNew(Property);
                        //    //}
                        //    var DRows = DtabPaymentSummry.Rows.Cast<DataRow>().Where(row => Val.ToString(row["TRN_ID"]) ==  Id && Val.ToString(row["STATUS"]) == "1").ToArray();                            
                        //    foreach (DataRow dr in DRows)
                        //        DtabPaymentSummry.Rows.Remove(dr);

                        //    MainGrdPayment.DataSource = DtabPaymentSummry;
                        //    MainGrdPayment.Refresh();

                        //    Global.Message(Property.ReturnMessageDesc);
                        //    if (Property.ReturnMessageType == "SUCCESS")
                        //    {
                        //        if (BOConfiguration.gStrLoginSection != "B")
                        //        {
                        //            BtnClear_Click(null, null);                                    
                        //        }
                        //        else
                        //        { BtnSave.Enabled = false; }
                        //    }
                        //}
                        //FrmPassword FrmPassword = new FrmPassword();
                        //if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                        //{

                        //}
                        this.Cursor = Cursors.Default;

                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void rptxtEbtryDate_Validating(object sender, CancelEventArgs e)
        {
            //DataRow drow = GrdDetPayment.GetFocusedDataRow();
            //if (Val.ToString(drow["TRN_ID"]) != String.Empty)
            //{
            //    calculate();
            //    BtnSave_Click(null, null);
            //}
        }

        private void GrdDetail_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int IntBillCurId;
                double CreditAmt, CreditAmtFE, INVAMOUNT, PrevAmt;
                double TotalAmount, TotalAmountFE;
                Guid TRNID, SummryTrnID;
                if (e.RowHandle < 0)
                    return;
                if (BOConfiguration.gStrLoginSection != "B")
                {
                    if (IntAllowToCalculate != 0)
                        return;
                }

                if (txtExcRate.Text == string.Empty || txtExcRate.Text == "")
                    txtExcRate.Text = "1";

                string Str = Val.Left(Val.ToString(CmbPaymentType.SelectedItem), 2);
                switch (e.Column.FieldName.ToUpper())
                {
                    case "CREDIT":
                        if (RdoCurrency.SelectedIndex.ToString() == "1")
                        {
                            TotalAmount = 0; TotalAmountFE = 0;
                            GrdDetail.PostEditor();
                            DataRow Dr = GrdDetail.GetDataRow(e.RowHandle);
                            CreditAmt = Val.Val(Dr["CREDIT"]);
                            PrevAmt = Val.Val(Dr["CREDITFE"]);
                            INVAMOUNT = Val.Val(Dr["INVAMOUNT"]);
                            GrdDetail.SetFocusedRowCellValue("CREDITFE", CreditAmt);
                            //if (INVAMOUNT <= CreditAmt)
                            if (INVAMOUNT < CreditAmt)
                            {
                                Global.Message("This Invoice Amount Is " + INVAMOUNT + "");
                                GrdDetail.SetFocusedRowCellValue("CREDIT", PrevAmt);
                                GrdDetail.SetFocusedRowCellValue("CREDITFE", PrevAmt);
                            }
                            else
                                GrdDetail.SetFocusedRowCellValue("CREDITFE", CreditAmt);

                            SummryTrnID = Val.ToGuid(Dr["SUMMRYTRN_ID"]);
                            DataTable dt1 = new DataTable();
                            var DRow1 = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToGuid(row["SUMMRYTRN_ID"]) == SummryTrnID).ToArray();
                            foreach (DataRow dr in DRow1)
                            {
                                TotalAmount += Val.Val(dr["CREDIT"]);
                                TotalAmountFE += Val.Val(dr["CREDITFE"]);
                            }
                            var DRow2 = DtabPaymentSummry.Rows.Cast<DataRow>().Where(row => Val.ToGuid(row["ACCLEDGTRNTRN_ID"]) == SummryTrnID).ToArray();
                            foreach (DataRow dr1 in DRow2)
                            {
                                dr1["CREDITAMOUNT"] = TotalAmount.ToString();
                                dr1["CREDITAMOUNTFE"] = TotalAmountFE.ToString();
                            }

                            var DRow3 = DtabPaymentDetail.Rows.Cast<DataRow>().Where(row => Val.ToGuid(row["ACCLEDGTRNTRN_ID"]) == SummryTrnID).ToArray();
                            foreach (DataRow dr2 in DRow3)
                            {
                                dr2["CREDITAMOUNT"] = TotalAmount.ToString();
                                dr2["CREDITAMOUNTFE"] = TotalAmountFE.ToString();
                            }
                            txtAmount.Text = Val.ToString(TotalAmount);
                        }
                        break;

                    case "CREDITFE":
                        if (RdoCurrency.SelectedIndex.ToString() == "0")
                        {
                            TotalAmount = 0; TotalAmountFE = 0;
                            GrdDetail.PostEditor();
                            DataRow Dr = GrdDetail.GetDataRow(e.RowHandle);
                            CreditAmtFE = Val.Val(Dr["CREDITFE"]);
                            PrevAmt = Val.Val(Dr["CREDIT"]);
                            INVAMOUNT = Val.Val(Dr["INVAMOUNT"]);
                            if (INVAMOUNT < CreditAmtFE)
                            {
                                Global.Message("This Invoice Amount Is " + INVAMOUNT + "");
                                GrdDetail.SetFocusedRowCellValue("CREDIT", PrevAmt);
                                GrdDetail.SetFocusedRowCellValue("CREDITFE", PrevAmt);
                            }
                            else
                                GrdDetail.SetFocusedRowCellValue("CREDIT", CreditAmtFE);

                            SummryTrnID = Val.ToGuid(Dr["SUMMRYTRN_ID"]);
                            DataTable dt1 = new DataTable();
                            var DRow1 = DtabPaymentPickupBillDetail.Rows.Cast<DataRow>().Where(row => Val.ToGuid(row["SUMMRYTRN_ID"]) == SummryTrnID).ToArray();
                            foreach (DataRow dr in DRow1)
                            {
                               TotalAmount += Val.Val(dr["CREDIT"]);
                               TotalAmountFE += Val.Val(dr["CREDITFE"]);
                            }
                            var DRow2 = DtabPaymentSummry.Rows.Cast<DataRow>().Where(row => Val.ToGuid(row["ACCLEDGTRNTRN_ID"]) == SummryTrnID).ToArray();
                            foreach (DataRow dr1 in DRow2)
                            {
                                dr1["CREDITAMOUNT"] = TotalAmount.ToString();
                                dr1["CREDITAMOUNTFE"] = TotalAmountFE.ToString();
                            }

                            var DRow3 = DtabPaymentDetail.Rows.Cast<DataRow>().Where(row => Val.ToGuid(row["ACCLEDGTRNTRN_ID"]) == SummryTrnID).ToArray();
                            foreach (DataRow dr2 in DRow3)
                            {
                                dr2["CREDITAMOUNT"] = TotalAmount.ToString();
                                dr2["CREDITAMOUNTFE"] = TotalAmountFE.ToString();
                            }
                            txtAmount.Text = Val.ToString(TotalAmountFE);
                        }
                        break;
                }
                //calculate();
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message.ToString());
            }
        }

        private void txtPartyName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "LEDGERCODE,LEDGERNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.Width = 700;
                    FrmSearch.Height = 500;
                    //string Str = Val.Left(Val.ToString(CmbPaymentType.SelectedItem), 2);
                    DataTable DTFINAL = new DataTable();
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LEDGER);
                    FrmSearch.mStrColumnsToHide = "LEDGER_ID,COMPANYNAME";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtPartyName.Text = Val.ToString(FrmSearch.DRow["LEDGERNAME"]);
                        txtPartyName.Tag = Val.ToString(FrmSearch.DRow["LEDGER_ID"]);
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
        
        private void BtnAdd_Click(object sender, EventArgs e)
        {
            DTPSFromDate.Checked = true;
            DTPSToDate.Checked = true;
            DTPSFromDate.Value = DateTime.Now;
            DTPSToDate.Value = DateTime.Now;
            txtPartyName.Text = string.Empty;
            txtPartyName.Tag = string.Empty;
            txtSearchVoucher.Text = string.Empty;
            txtSearchVoucher.Tag = string.Empty;
            ChkcmbPaymentTypeSearch.SetEditValue(0);
            RGSearchCurrency.EditValue = -1;
            MainGrid.DataSource = null;
        }

        //Added by Daksha on 12/09/2023
        private void txtSearchVoucher_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearchPopupBoxMultipleSelect FrmSearch = new FrmSearchPopupBoxMultipleSelect();
                    FrmSearch.mDTab = objLedgerTrn.PaymetReceive_GetVoucherNoStr();
                    FrmSearch.mStrColumnsToHide = "";
                    FrmSearch.ValueMemeter = "VoucherNo";
                    FrmSearch.DisplayMemeter = "VoucherNoStr";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.SelectedDisplaymember != "" && FrmSearch.SelectedValuemember != "")
                    {
                        txtSearchVoucher.Text = Val.ToString(FrmSearch.SelectedDisplaymember);
                        txtSearchVoucher.Tag = Val.ToString(FrmSearch.SelectedValuemember);
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

        //End as Daksha

        //private void rptxtEbtryDate_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.Enter)
        //    {
        //        EditModeDateChange = true;
        //    }
        //}

    }
}
