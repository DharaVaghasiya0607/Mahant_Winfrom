using MahantExport.Utility;
using BusLib;
using BusLib.Account;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using Google.API.Translate;
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
using System.Windows.Forms;
using MahantExport.Stock;

namespace MahantExport.Account
{
    public partial class FrmPickupBillAllocation : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();
        BOLedgerTransaction objLedgerTrn = new BOLedgerTransaction();
        BOACC_FinanceJournalEntry ObjFinance = new BOACC_FinanceJournalEntry();
        DataTable DtabPaymentDetail = new DataTable();
        BODevGridSelection ObjGridSelection;
        public DataTable DtAllocatedBill = new DataTable();
        public DataTable Allocation = new DataTable();
        public string FrmSubited = "N";
        public string FormType;
        public Guid Ledger_Id, TRN_ID;
        public decimal ExcRAte;
        public string StrEntryDate;
        public int IntRowIndex = 0, IntCurrency_ID = 0;
        bool IsSpaceSelect = false;
        //Add shiv
        public string HKTYPE = "";

        public FrmPickupBillAllocation()
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
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            FrmSubited = "N";
            this.Close();
        }

        private void GrdDetPayment_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                int IntBillCurId;
                decimal DecPendingAmt = 0, DecAllocateAmt = 0, DecExcRate = 0;
                if (e.RowHandle < 0)

                    return;
                GrdDetPayment.PostEditor();
                switch (Val.ToString(e.Column.FieldName))
                {
                    case "PAYMENTAMOUNT":

                        decimal DecAdjustAmt = 0, DecCreditAmt = 0;

                        DecAdjustAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("PAYMENTAMOUNT"));
                        DecCreditAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("PENDCREDITAMOUNTFE"));

                        if (Val.ToString(GrdDetPayment.GetFocusedRowCellValue("CURRENCY")) == "USD")
                        {

                            IntBillCurId = Val.ToInt32(GrdDetPayment.GetFocusedRowCellValue("CURRENCY_ID"));
                            DecExcRate = Val.ToDecimal(lblExcRate.Text);

                            DataRow Dr = GrdDetPayment.GetDataRow(e.RowHandle);
                            if (FormType == "BP" || FormType == "CP" || Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PICKUPTYPE")) == "CP")
                            {
                                DecPendingAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("PENDDEBITAMOUNT"));
                                DecAllocateAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("PAYMENTAMOUNT"));

                                DecAllocateAmt += Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("BANKCHARGES"));
                                if (DecAllocateAmt > DecPendingAmt)
                                {
                                    //Added byDaksha on 05/06/2023
                                    if (BOConfiguration.gStrLoginSection != "B")
                                    {
                                        GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                                        GrdDetPayment.SetFocusedRowCellValue("FPAYMENTAMOUNT", Math.Round(Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("PAYMENTAMOUNT")) * DecExcRate, 3));
                                    }
                                    //End as Daksha
                                    else
                                    {
                                        Global.MessageError("Amount Cant be Greater Then Pending Amount");
                                        GrdDetPayment.SetFocusedRowCellValue("PAYMENTAMOUNT", 0);
                                    }
                                }
                                else if (DecAllocateAmt == 0)
                                {
                                    GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", false);
                                    GrdDetPayment.SetFocusedRowCellValue("FPAYMENTAMOUNT", 0);
                                }
                                else
                                {
                                    GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                                    GrdDetPayment.SetFocusedRowCellValue("FPAYMENTAMOUNT", DecExcRate * Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("PAYMENTAMOUNT")));
                                }

                            }
                            else if (FormType == "BR" || FormType == "CR" || Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PICKUPTYPE")) == "CR")
                            {
                                DecPendingAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("PENDCREDITAMOUNT"));
                                DecAllocateAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("PAYMENTAMOUNT"));

                                DecAllocateAmt += Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("BANKCHARGES"));
                                if (DecAllocateAmt > DecPendingAmt)
                                {
                                    //Added by Daksha on 05/06/2023
                                    if (BOConfiguration.gStrLoginSection != "B")
                                    {
                                        GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                                        GrdDetPayment.SetFocusedRowCellValue("FPAYMENTAMOUNT", Math.Round(Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("PAYMENTAMOUNT")) * DecExcRate, 3));
                                    }
                                    //End as Daksha
                                    else
                                    {
                                        Global.MessageError("Amount Cant be Greater Then Pending Amount");
                                        GrdDetPayment.SetFocusedRowCellValue("PAYMENTAMOUNT", 0);
                                    }
                                }
                                else if (DecAllocateAmt == 0)
                                {
                                    GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", false);
                                    GrdDetPayment.SetFocusedRowCellValue("FPAYMENTAMOUNT", 0);
                                }
                                else
                                {
                                    GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                                    GrdDetPayment.SetFocusedRowCellValue("FPAYMENTAMOUNT", DecExcRate * Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("PAYMENTAMOUNT")));

                                }
                            }
                            if (DecAllocateAmt == DecPendingAmt)
                            {
                                GrdDetPayment.SetFocusedRowCellValue("PAYMENTMADE", "Full");
                                GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                            }
                            else if (DecAllocateAmt == 0)
                            {
                                GrdDetPayment.SetFocusedRowCellValue("PAYMENTMADE", "Partial");
                                GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", false);
                            }
                            else
                            {
                                GrdDetPayment.SetFocusedRowCellValue("PAYMENTMADE", "Partial");
                                GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                            }
                            calculate();
                            string PaymentMade = Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PAYMENTMADE"));
                            if (PaymentMade != "Partial" && IntCurrency_ID == 1 && BOConfiguration.gStrLoginSection != "B")
                                CalculateExcDiff();
                        }
                        break;
                    case "FPAYMENTAMOUNT":
                        if (Val.ToString(GrdDetPayment.GetFocusedRowCellValue("CURRENCY")) != "USD")
                        {
                            IntBillCurId = Val.ToInt32(GrdDetPayment.GetFocusedRowCellValue("CURRENCY_ID"));
                            GrdDetPayment.PostEditor();
                            if (FormType == "BP" || FormType == "CP" || Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PICKUPTYPE")) == "CP")
                            {
                                DecPendingAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("PENDDEBITAMOUNTFE"));
                                DecAllocateAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("FPAYMENTAMOUNT"));
                                DecAllocateAmt += Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("FEBANKCHARGES"));
                                DecExcRate = Val.ToDecimal(lblExcRate.Text);

                                if (DecAllocateAmt > DecPendingAmt)
                                {
                                    //Added by Daksha on 05/06/2023
                                    if (BOConfiguration.gStrLoginSection != "B")
                                    {
                                        GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                                        GrdDetPayment.SetFocusedRowCellValue("PAYMENTAMOUNT", Math.Round(Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("FPAYMENTAMOUNT")) / DecExcRate, 3));
                                    }
                                    //End as Daksha
                                    else
                                    {
                                        Global.MessageError("Amount Cant be Greater Then Pending Amount");
                                        GrdDetPayment.SetFocusedRowCellValue("FPAYMENTAMOUNT", 0);
                                    }
                                }
                                else if (DecAllocateAmt == 0)
                                {
                                    GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", false);
                                    GrdDetPayment.SetFocusedRowCellValue("PAYMENTAMOUNT", 0);
                                }
                                else if (DecExcRate > 0)
                                {
                                    GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                                    GrdDetPayment.SetFocusedRowCellValue("PAYMENTAMOUNT", Math.Round(Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("FPAYMENTAMOUNT")) / DecExcRate, 3)); //B 
                                }
                            }
                            else if (FormType == "BR" || FormType == "CR" || Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PICKUPTYPE")) == "CR")
                            {
                                DecPendingAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("PENDCREDITAMOUNTFE"));
                                DecAllocateAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("FPAYMENTAMOUNT"));
                                DecAllocateAmt += Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("FEBANKCHARGES"));
                                DecExcRate = Val.ToDecimal(lblExcRate.Text);

                                if (DecAllocateAmt > DecPendingAmt)
                                {
                                    //Global.MessageError("Amount Cant be Greater Then Pending Amount");
                                    //GrdDetPayment.SetFocusedRowCellValue("FPAYMENTAMOUNT", 0);
                                    GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                                    GrdDetPayment.SetFocusedRowCellValue("PAYMENTAMOUNT", Math.Round(Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("FPAYMENTAMOUNT")) / DecExcRate, 3));//B
                                }
                                else if (DecAllocateAmt == 0)
                                {
                                    GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", false);
                                    GrdDetPayment.SetFocusedRowCellValue("PAYMENTAMOUNT", 0);
                                }
                                else if (DecExcRate > 0)
                                {
                                    GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                                    GrdDetPayment.SetFocusedRowCellValue("PAYMENTAMOUNT", Math.Round(Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("FPAYMENTAMOUNT")) / DecExcRate, 3));//B
                                }
                            }
                            if (DecAllocateAmt == DecPendingAmt)
                            {
                                GrdDetPayment.SetFocusedRowCellValue("PAYMENTMADE", "Full");
                                GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                            }
                            else if (DecAllocateAmt == 0)
                            {
                                GrdDetPayment.SetFocusedRowCellValue("PAYMENTMADE", "Partial");
                                GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", false);
                            }
                            else if (DecAllocateAmt < DecPendingAmt)
                            {
                                GrdDetPayment.SetFocusedRowCellValue("PAYMENTMADE", "Partial");
                                //GrdDetPayment.SetFocusedRowCellValue("PAYMENTMADE", "Full");
                                GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                            }
                            else
                            {
                                //GrdDetPayment.SetFocusedRowCellValue("PAYMENTMADE", "Partial");
                                GrdDetPayment.SetFocusedRowCellValue("PAYMENTMADE", "Full");
                                GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                            }
                            calculate();
                            string PaymentMade = Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PAYMENTMADE"));
                            if (PaymentMade != "Partial" && IntCurrency_ID == 1)
                                CalculateExcDiff();
                        }
                        break;
                    case "BANKCHARGES":
                        if (Val.ToString(GrdDetPayment.GetFocusedRowCellValue("CURRENCY")) == "USD")
                        {
                            GrdDetPayment.PostEditor();
                            IntBillCurId = Val.ToInt32(GrdDetPayment.GetFocusedRowCellValue("CURRENCY_ID"));
                            DecExcRate = Val.ToDecimal(lblExcRate.Text);

                            DataRow Dr = GrdDetPayment.GetDataRow(e.RowHandle);
                            if (FormType == "BP" || FormType == "CP" || Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PICKUPTYPE")) == "CP")
                            {
                                DecPendingAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("PENDDEBITAMOUNT"));
                                DecAllocateAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("PAYMENTAMOUNT"));
                                DecAllocateAmt += Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("BANKCHARGES"));

                                //DecAllocateAmt = Val.ToDecimal(Dr["PAYMENTAMOUNT"]);

                                if (DecAllocateAmt > DecPendingAmt)
                                {
                                    Global.MessageError("Amount Cant be Greater Then Pending Amount");
                                    GrdDetPayment.SetFocusedRowCellValue("BANKCHARGES", 0);
                                }
                                else if (DecAllocateAmt == 0)
                                {
                                    GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", false);
                                    GrdDetPayment.SetFocusedRowCellValue("FEBANKCHARGES", 0);
                                }
                                else
                                {
                                    GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                                    GrdDetPayment.SetFocusedRowCellValue("FEBANKCHARGES", DecExcRate * Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("BANKCHARGES")));
                                }

                            }
                            else if (FormType == "BR" || FormType == "CR" || Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PICKUPTYPE")) == "CR")
                            {
                                DecPendingAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("PENDCREDITAMOUNT"));
                                DecAllocateAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("PAYMENTAMOUNT"));
                                DecAllocateAmt += Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("BANKCHARGES"));
                                //DecAllocateAmt = Val.ToDecimal(Dr["PAYMENTAMOUNT"]);
                                if (DecAllocateAmt > DecPendingAmt)
                                {
                                    Global.MessageError("Amount Cant be Greater Then Pending Amount");
                                    GrdDetPayment.SetFocusedRowCellValue("BANKCHARGES", 0);
                                }
                                else if (DecAllocateAmt == 0)
                                {
                                    GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", false);
                                    GrdDetPayment.SetFocusedRowCellValue("FEBANKCHARGES", 0);
                                }
                                else
                                {
                                    GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                                    GrdDetPayment.SetFocusedRowCellValue("FEBANKCHARGES", DecExcRate * Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("BANKCHARGES")));
                                }
                            }
                            if (DecAllocateAmt == DecPendingAmt)
                            {
                                GrdDetPayment.SetFocusedRowCellValue("PAYMENTMADE", "Full");
                                GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                            }
                            else if (DecAllocateAmt == 0)
                            {
                                GrdDetPayment.SetFocusedRowCellValue("PAYMENTMADE", "Partial");
                                GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", false);
                            }
                            else
                            {
                                GrdDetPayment.SetFocusedRowCellValue("PAYMENTMADE", "Partial");
                                GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                            }
                            calculate();
                            string PaymentMade = Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PAYMENTMADE"));
                            if (PaymentMade != "Partial" && IntCurrency_ID == 1)
                                CalculateExcDiff();
                        }
                        break;
                    case "FEBANKCHARGES":
                        if (Val.ToString(GrdDetPayment.GetFocusedRowCellValue("CURRENCY")) != "USD")
                        {
                            IntBillCurId = Val.ToInt32(GrdDetPayment.GetFocusedRowCellValue("CURRENCY_ID"));
                            GrdDetPayment.PostEditor();
                            if (FormType == "BP" || FormType == "CP" || Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PICKUPTYPE")) == "CP")
                            {
                                DecPendingAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("PENDDEBITAMOUNTFE"));
                                DecAllocateAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("FPAYMENTAMOUNT"));
                                DecAllocateAmt += Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("FEBANKCHARGES"));
                                DecExcRate = Val.ToDecimal(lblExcRate.Text);

                                if (DecAllocateAmt > DecPendingAmt)
                                {
                                    Global.MessageError("Amount Cant be Greater Then Pending Amount");
                                    GrdDetPayment.SetFocusedRowCellValue("FEBANKCHARGES", 0);
                                }
                                else if (DecAllocateAmt == 0)
                                {
                                    GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", false);
                                    GrdDetPayment.SetFocusedRowCellValue("BANKCHARGES", 0);
                                }
                                else if (DecExcRate > 0)
                                {
                                    GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                                    GrdDetPayment.SetFocusedRowCellValue("BANKCHARGES", Math.Round(Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("FEBANKCHARGES")) / DecExcRate, 3)); //B PART
                                }
                            }
                            else if (FormType == "BR" || FormType == "CR" || Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PICKUPTYPE")) == "CR")
                            {
                                DecPendingAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("PENDCREDITAMOUNTFE"));
                                DecAllocateAmt = Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("FPAYMENTAMOUNT"));
                                DecAllocateAmt += Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("FEBANKCHARGES"));
                                DecExcRate = Val.ToDecimal(lblExcRate.Text);

                                if (DecAllocateAmt > DecPendingAmt)
                                {
                                    Global.MessageError("Amount Cant be Greater Then Pending Amount");
                                    GrdDetPayment.SetFocusedRowCellValue("FEBANKCHARGES", 0);
                                }
                                else if (DecAllocateAmt == 0)
                                {
                                    GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", false);
                                    GrdDetPayment.SetFocusedRowCellValue("BANKCHARGES", 0);
                                }
                                else if (DecExcRate > 0)
                                {
                                    GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                                    GrdDetPayment.SetFocusedRowCellValue("BANKCHARGES", Math.Round(Val.ToDecimal(GrdDetPayment.GetFocusedRowCellValue("FEBANKCHARGES")) / DecExcRate, 3)); // B PART
                                }
                            }
                            if (DecAllocateAmt == DecPendingAmt)
                            {
                                GrdDetPayment.SetFocusedRowCellValue("PAYMENTMADE", "Full");
                                GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                            }
                            else if (DecAllocateAmt == 0)
                            {
                                GrdDetPayment.SetFocusedRowCellValue("PAYMENTMADE", "Partial");
                                GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", false);
                            }
                            else
                            {
                                GrdDetPayment.SetFocusedRowCellValue("PAYMENTMADE", "Partial");
                                GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                            }
                            calculate();
                            string PaymentMade = Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PAYMENTMADE"));
                            if (PaymentMade != "Partial" && IntCurrency_ID == 1)
                                CalculateExcDiff();
                        }
                        break;
                }

                GrdDetPayment.RefreshData(); //Added by Daksha on 13/09/2023
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message.ToString());
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            
            FrmSubited = "Y";
            Allocation = GetTableOfSelectedRows(GrdDetPayment, "COLSELECTCHECKBOX");

            //Gunjan:16/04/2024
            for (int i = 0; i < Allocation.Rows.Count; i++)
            {
                decimal DecAdjustAmt = 0, DecCreditAmt = 0;

                DecAdjustAmt = Val.ToDecimal(Allocation.Rows[i]["PAYMENTAMOUNT"]);

                if (FormType == "BR" || FormType == "CR")
                {
                    DecCreditAmt = Val.ToDecimal(Allocation.Rows[i]["PENDCREDITAMOUNTFE"]);
                }
                else
                {
                    DecCreditAmt = Val.ToDecimal(Allocation.Rows[i]["PENDDEBITAMOUNTFE"]);
                }

                if (DecCreditAmt < DecAdjustAmt)
                {
                    Global.Message("YOUR ADJUST AMOUNT IS GRETER THEN PENDING AMOUNT , PLEASE CHECK !!");
                    FrmSubited = "N";
                    return;
                }
            }
            //End As Gunjan   
            var DRows = Allocation.Rows.Cast<DataRow>().Where(row => Val.ToDecimal(row["FPAYMENTAMOUNT"]) == 0).ToArray();
            foreach (DataRow dr in DRows)
                Allocation.Rows.Remove(dr);

            this.Close();
            
        }

        private DataTable GetTableOfSelectedRows(GridView view, string pStrColumnNAme)
        {
            if (view.RowCount <= 0)
            {
                return null;
            }
            DataTable resultTable = new DataTable();
            DataTable sourceTable = null;
            try
            {
                ArrayList aryLst = new ArrayList();
                sourceTable = ((DataView)view.DataSource).Table;

                if (ObjGridSelection != null)
                {
                    aryLst = ObjGridSelection.GetSelectedArrayList();
                    resultTable = sourceTable.Clone();
                    for (int i = 0; i < aryLst.Count; i++)
                    {
                        DataRowView oDataRowView = aryLst[i] as DataRowView;
                        resultTable.Rows.Add(oDataRowView.Row.ItemArray);
                    }
                }
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.ToString());
            }
            return resultTable;
        }

        private void FrmPickupBillAllocation_Load(object sender, EventArgs e)
        {
            GrdDetPayment.BeginUpdate();
            if (mainPickup.RepositoryItems.Count == 5)
            {
                ObjGridSelection = new BODevGridSelection();
                ObjGridSelection.View = GrdDetPayment;
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;

            }
            else
            {
                ObjGridSelection.ClearSelection();
            }

            if (ObjGridSelection != null)
            {
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                GrdDetPayment.Columns["COLSELECTCHECKBOX"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
                GrdDetPayment.Columns["COLSELECTCHECKBOX"].VisibleIndex = 0;
                GrdDetPayment.Columns["PAYMENTMADE"].VisibleIndex = 1;
                GrdDetPayment.Columns["BOOKTYPEFULL"].VisibleIndex = 2;
            }
            GrdDetPayment.EndUpdate();

            if (FormType == "BP" || FormType == "CP")
            {
                GrdDetPayment.Columns["CREDITAMOUNTFE"].Visible = false;
                GrdDetPayment.Columns["CREDITAMOUNT"].Visible = false;
                GrdDetPayment.Columns["PENDCREDITAMOUNT"].Visible = false;
                GrdDetPayment.Columns["PENDCREDITAMOUNTFE"].Visible = false;
            }
            else if (FormType == "BR" || FormType == "CR")
            {
                GrdDetPayment.Columns["DEBITAMOUNTFE"].Visible = false;
                GrdDetPayment.Columns["DEBITAMOUNT"].Visible = false;
                GrdDetPayment.Columns["PENDDEBITAMOUNT"].Visible = false;
                GrdDetPayment.Columns["PENDDEBITAMOUNTFE"].Visible = false;
            }

            DtabPaymentDetail = objLedgerTrn.GetBillWiseOutstandingNew(Ledger_Id, TRN_ID, StrEntryDate, IntCurrency_ID, FormType,HKTYPE);
            if (DtAllocatedBill.Rows.Count > 0)
            {

                for (int j = 0; j < DtAllocatedBill.Rows.Count; j++)
                {
                    string refTran_id, tran_id, refSrno, srno, refAccLedgTrnTrn_ID, AccLedgTrnTrn_ID, refAccLedgTrnSrno, AccLedgTrnSrno;
                    int mainGrdRowId;
                    refTran_id = Val.ToString(DtAllocatedBill.Rows[j]["REFTRN_ID"]);
                    refSrno = Val.ToString(DtAllocatedBill.Rows[j]["REFSRNO"]);
                    refAccLedgTrnTrn_ID = Val.ToString(DtAllocatedBill.Rows[j]["REFACCLEDGTRNTRN_ID"]);
                    refAccLedgTrnSrno = Val.ToString(DtAllocatedBill.Rows[j]["REFACCLEDGTRNSRNO"]);
                    mainGrdRowId = Val.ToInt(DtAllocatedBill.Rows[j]["MainGrdRow_id"]);
                    if (mainGrdRowId == IntRowIndex)
                    {
                        for (int i = 0; i < DtabPaymentDetail.Rows.Count; i++)
                        {
                            tran_id = Val.ToString(DtabPaymentDetail.Rows[i]["TRN_ID"]);
                            srno = Val.ToString(DtabPaymentDetail.Rows[i]["SRNO"]);
                            AccLedgTrnTrn_ID = Val.ToString(DtabPaymentDetail.Rows[i]["ACCLEDGTRNTRN_ID"]);
                            AccLedgTrnSrno = Val.ToString(DtabPaymentDetail.Rows[i]["ACCLEDGTRNSRNO"]);
                            if ((refTran_id == tran_id) && (refSrno == srno) && (refAccLedgTrnTrn_ID == AccLedgTrnTrn_ID) && (refAccLedgTrnSrno == AccLedgTrnSrno))
                            {
                                //if (FormType == "BP" || FormType == "CP")
                                //{
                                //    DtabPaymentDetail.Rows[i]["PAYMENTAMOUNT"] = DtAllocatedBill.Rows[j]["DEBIT"];
                                //    DtabPaymentDetail.Rows[i]["FPAYMENTAMOUNT"] = DtAllocatedBill.Rows[j]["DEBITFE"];
                                //}
                                //else
                                //{
                                //    DtabPaymentDetail.Rows[i]["PAYMENTAMOUNT"] = DtAllocatedBill.Rows[j]["CREDIT"];
                                //    DtabPaymentDetail.Rows[i]["FPAYMENTAMOUNT"] = DtAllocatedBill.Rows[j]["CREDITFE"];
                                //}
                                DtabPaymentDetail.Rows[i]["PAYMENTAMOUNT"] = DtAllocatedBill.Rows[j]["PAYMENTAMOUNT"];
                                DtabPaymentDetail.Rows[i]["FPAYMENTAMOUNT"] = DtAllocatedBill.Rows[j]["FPAYMENTAMOUNT"];
                                DtabPaymentDetail.Rows[i]["BANKCHARGES"] = DtAllocatedBill.Rows[j]["BANKCHARGES"];
                                DtabPaymentDetail.Rows[i]["FEBANKCHARGES"] = DtAllocatedBill.Rows[j]["FEBANKCHARGES"];
                                DtabPaymentDetail.Rows[i]["PAYMENTMADE"] = DtAllocatedBill.Rows[j]["PAYMENTMADE"];
                                DtabPaymentDetail.Rows[i]["EXCHRATEDIFF"] = DtAllocatedBill.Rows[j]["EXCHRATEDIFF"];

                                DtabPaymentDetail.Rows[i]["EXCHRATEDIFFUSD"] = DtAllocatedBill.Rows[j]["EXCHRATEDIFFUSD"];
                                DtabPaymentDetail.Rows[i]["DISCOUNTAMOUNT"] = DtAllocatedBill.Rows[j]["DISCOUNTAMOUNT"];
                                DtabPaymentDetail.Rows[i]["FDISCOUNTAMOUNT"] = DtAllocatedBill.Rows[j]["FDISCOUNTAMOUNT"];

                            }
                        }
                    }
                }
            }
            if (DtabPaymentDetail.Rows.Count > 0)
            {
                mainPickup.DataSource = DtabPaymentDetail;
                GrdDetPayment.FocusedColumn = GrdDetPayment.Columns["FPAYMENTAMOUNT"];
                mainPickup.Refresh();
                for (int i = 0; i < DtabPaymentDetail.Rows.Count; i++)
                {
                    GrdDetPayment.SetRowCellValue(i, "ENTRYDATE", DateTime.Now.ToString("dd/MM/yyyy"));
                    if (Val.ToDecimal(DtabPaymentDetail.Rows[i]["PAYMENTAMOUNT"]) != 0 || Val.ToDecimal(DtabPaymentDetail.Rows[i]["FPAYMENTAMOUNT"]) != 0)
                        GrdDetPayment.SetRowCellValue(i, "COLSELECTCHECKBOX", true);
                }
                lblExcRate.Text = Val.ToString(ExcRAte);
                calculate();
                ColumnHide();
                string PaymentMade = Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PAYMENTMADE"));
                if (PaymentMade != "Partial" && IntCurrency_ID == 1)
                    CalculateExcDiff();
                mainPickup.Focus();
                GrdDetPayment.Focus();
                mainPickup.Select();
            }
            else
            {
                Global.Message("No Record Found");
                FrmSubited = "N";
                this.Close();
            }
        }

        void ColumnHide()
        {
            try
            {
                if (BOConfiguration.gStrLoginSection == "B")
                {
                    if (DtabPaymentDetail.Rows[0]["BILLFORMAT"].ToString() == "RUPEES")
                    {
                        GrdDetPayment.Columns["DEBITAMOUNTFE"].Visible = true;
                        GrdDetPayment.Columns["PENDDEBITAMOUNTFE"].Visible = true;
                        GrdDetPayment.Columns["CREDITAMOUNTFE"].Visible = true;
                        GrdDetPayment.Columns["PENDCREDITAMOUNTFE"].Visible = true;
                        GrdDetPayment.Columns["FPAYMENTAMOUNT"].Visible = true;

                        GrdDetPayment.Columns["DEBITAMOUNT"].Visible = false;
                        GrdDetPayment.Columns["PENDDEBITAMOUNT"].Visible = false;
                        GrdDetPayment.Columns["CREDITAMOUNT"].Visible = false;
                        GrdDetPayment.Columns["PENDCREDITAMOUNT"].Visible = false;
                        GrdDetPayment.Columns["PAYMENTAMOUNT"].Visible = false;

                        if (FormType == "BP" || FormType == "CP")
                        {
                            GrdDetPayment.Columns["CREDITAMOUNTFE"].Visible = false;
                            GrdDetPayment.Columns["CREDITAMOUNT"].Visible = false;
                            GrdDetPayment.Columns["PENDCREDITAMOUNT"].Visible = false;
                            GrdDetPayment.Columns["PENDCREDITAMOUNTFE"].Visible = false;
                        }
                        else if (FormType == "BR" || FormType == "CR")
                        {
                            GrdDetPayment.Columns["DEBITAMOUNTFE"].Visible = false;
                            GrdDetPayment.Columns["DEBITAMOUNT"].Visible = false;
                            GrdDetPayment.Columns["PENDDEBITAMOUNT"].Visible = false;
                            GrdDetPayment.Columns["PENDDEBITAMOUNTFE"].Visible = false;
                        }
                    }
                    else
                    {
                        GrdDetPayment.Columns["DEBITAMOUNTFE"].Visible = false;
                        GrdDetPayment.Columns["PENDDEBITAMOUNTFE"].Visible = false;
                        GrdDetPayment.Columns["CREDITAMOUNTFE"].Visible = false;
                        GrdDetPayment.Columns["PENDCREDITAMOUNTFE"].Visible = false;
                        GrdDetPayment.Columns["FPAYMENTAMOUNT"].Visible = false;

                        GrdDetPayment.Columns["DEBITAMOUNT"].Visible = true;
                        GrdDetPayment.Columns["PENDDEBITAMOUNT"].Visible = true;
                        GrdDetPayment.Columns["CREDITAMOUNT"].Visible = true;
                        GrdDetPayment.Columns["PENDCREDITAMOUNT"].Visible = true;
                        GrdDetPayment.Columns["PAYMENTAMOUNT"].Visible = true;

                        if (FormType == "BP" || FormType == "CP")
                        {
                            GrdDetPayment.Columns["CREDITAMOUNTFE"].Visible = false;
                            GrdDetPayment.Columns["CREDITAMOUNT"].Visible = false;
                            GrdDetPayment.Columns["PENDCREDITAMOUNT"].Visible = false;
                            GrdDetPayment.Columns["PENDCREDITAMOUNTFE"].Visible = false;
                        }
                        else if (FormType == "BR" || FormType == "CR")
                        {
                            GrdDetPayment.Columns["DEBITAMOUNTFE"].Visible = false;
                            GrdDetPayment.Columns["DEBITAMOUNT"].Visible = false;
                            GrdDetPayment.Columns["PENDDEBITAMOUNT"].Visible = false;
                            GrdDetPayment.Columns["PENDDEBITAMOUNTFE"].Visible = false;
                        }
                    }

                    //Added by Daksha on 14/09/2023
                    if (DtabPaymentDetail.Rows[0]["ENTRYTYPE"].ToString() == "BROKERAGE")
                    {
                        if (DtabPaymentDetail.Rows[0]["BILLFORMAT"].ToString() == "RUPEES")
                        {
                            GrdDetPayment.Columns["DEBITAMOUNT"].Visible = true;
                        }   
                        else
                        {
                            GrdDetPayment.Columns["DEBITAMOUNTFE"].Visible = true;                            
                        }
                        GrdDetPayment.Columns["DEBITAMOUNT"].Caption = "Broker Amt";                        
                        GrdDetPayment.Columns["DEBITAMOUNT"].VisibleIndex = GrdDetPayment.Columns["DEBITAMOUNTFE"].VisibleIndex + 1;
                    }
                    else
                        GrdDetPayment.Columns["DEBITAMOUNT"].Caption = "Bill Amt";
                    //End as Daksha
                }
                else
                {
                    if ((DtabPaymentDetail.Rows[0]["BILLTYPE"].ToString().ToUpper() == "RUPEES BILL") || (DtabPaymentDetail.Rows[0]["BILLTYPE"].ToString().ToUpper() == "RUPEESBILL"))
                    {
                        GrdDetPayment.Columns["DEBITAMOUNTFE"].Visible = true;
                        GrdDetPayment.Columns["PENDDEBITAMOUNTFE"].Visible = true;
                        GrdDetPayment.Columns["CREDITAMOUNTFE"].Visible = true;
                        GrdDetPayment.Columns["PENDCREDITAMOUNTFE"].Visible = true;
                        GrdDetPayment.Columns["FPAYMENTAMOUNT"].Visible = true;

                        GrdDetPayment.Columns["DEBITAMOUNT"].Visible = false;
                        GrdDetPayment.Columns["PENDDEBITAMOUNT"].Visible = false;
                        GrdDetPayment.Columns["CREDITAMOUNT"].Visible = false;
                        GrdDetPayment.Columns["PENDCREDITAMOUNT"].Visible = false;
                        GrdDetPayment.Columns["PAYMENTAMOUNT"].Visible = false;


                        if (FormType == "BP" || FormType == "CP")
                        {
                            GrdDetPayment.Columns["CREDITAMOUNTFE"].Visible = false;
                            GrdDetPayment.Columns["CREDITAMOUNT"].Visible = false;
                            GrdDetPayment.Columns["PENDCREDITAMOUNT"].Visible = false;
                            GrdDetPayment.Columns["PENDCREDITAMOUNTFE"].Visible = false;
                        }
                        else if (FormType == "BR" || FormType == "CR")
                        {
                            GrdDetPayment.Columns["DEBITAMOUNTFE"].Visible = false;
                            GrdDetPayment.Columns["DEBITAMOUNT"].Visible = false;
                            GrdDetPayment.Columns["PENDDEBITAMOUNT"].Visible = false;
                            GrdDetPayment.Columns["PENDDEBITAMOUNTFE"].Visible = false;
                        }
                    }
                    else
                    {
                        GrdDetPayment.Columns["DEBITAMOUNTFE"].Visible = false;
                        GrdDetPayment.Columns["PENDDEBITAMOUNTFE"].Visible = false;
                        GrdDetPayment.Columns["CREDITAMOUNTFE"].Visible = false;
                        GrdDetPayment.Columns["PENDCREDITAMOUNTFE"].Visible = false;
                        GrdDetPayment.Columns["FPAYMENTAMOUNT"].Visible = false;

                        GrdDetPayment.Columns["DEBITAMOUNT"].Visible = true;
                        GrdDetPayment.Columns["PENDDEBITAMOUNT"].Visible = true;
                        GrdDetPayment.Columns["CREDITAMOUNT"].Visible = true;
                        GrdDetPayment.Columns["PENDCREDITAMOUNT"].Visible = true;
                        GrdDetPayment.Columns["PAYMENTAMOUNT"].Visible = true;

                        if (FormType == "BP" || FormType == "CP")
                        {
                            GrdDetPayment.Columns["CREDITAMOUNTFE"].Visible = false;
                            GrdDetPayment.Columns["CREDITAMOUNT"].Visible = false;
                            GrdDetPayment.Columns["PENDCREDITAMOUNT"].Visible = false;
                            GrdDetPayment.Columns["PENDCREDITAMOUNTFE"].Visible = false;
                        }
                        else if (FormType == "BR" || FormType == "CR")
                        {
                            GrdDetPayment.Columns["DEBITAMOUNTFE"].Visible = false;
                            GrdDetPayment.Columns["DEBITAMOUNT"].Visible = false;
                            GrdDetPayment.Columns["PENDDEBITAMOUNT"].Visible = false;
                            GrdDetPayment.Columns["PENDDEBITAMOUNTFE"].Visible = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDetPayment_ShowingEditor(object sender, CancelEventArgs e)
        {
            GridView view = (GridView)sender;
            string strRefType = Val.ToString(view.GetFocusedRowCellValue("REFTYPE"));
            string strColName = Val.ToString(view.FocusedColumn.FieldName);

            if (Val.ToString(GrdDetPayment.GetFocusedRowCellValue("CURRENCY")) != "USD" && (strColName == "PAYMENTAMOUNT" || strColName == "BANKCHARGES"))
                e.Cancel = true;
            if (Val.ToString(GrdDetPayment.GetFocusedRowCellValue("CURRENCY")) == "USD" && (strColName == "FPAYMENTAMOUNT" || strColName == "FEBANKCHARGES"))
                e.Cancel = true;
        }

        void calculate()
        {
            try
            {

                decimal DecTotalUSDDeb, DecTotalUSDCrd, DecSelectUSDDeb, DecPendUSD = 0;
                decimal DecTotalInrDeb, DecTotalInrCrd, DecSelectInrDeb, DecPendInr = 0;

                DataTable dtSelected = GetTableOfSelectedRows(GrdDetPayment, "");
                //DataTable dtSelected = Global.GetSelectedRecordOfGrid(GrdDetPayment, true, ObjGridSelection);

                DecTotalUSDDeb = Val.ToDecimal(DtabPaymentDetail.Compute("SUM(PENDDEBITAMOUNT)", string.Empty));
                DecTotalUSDCrd = Val.ToDecimal(DtabPaymentDetail.Compute("SUM(PENDCREDITAMOUNT)", string.Empty));
                DecTotalInrDeb = Val.ToDecimal(DtabPaymentDetail.Compute("SUM(PENDDEBITAMOUNTFE)", string.Empty));
                DecTotalInrCrd = Val.ToDecimal(DtabPaymentDetail.Compute("SUM(PENDCREDITAMOUNTFE)", string.Empty));

                DecSelectUSDDeb = Val.ToDecimal(dtSelected.Compute("SUM(PAYMENTAMOUNT)", string.Empty));
                DecSelectInrDeb = Val.ToDecimal(dtSelected.Compute("SUM(FPAYMENTAMOUNT)", string.Empty));
                DecSelectUSDDeb += Val.ToDecimal(dtSelected.Compute("SUM(BANKCHARGES)", string.Empty));
                DecSelectInrDeb += Val.ToDecimal(dtSelected.Compute("SUM(FEBANKCHARGES)", string.Empty));

                if (FormType == "CP" || FormType == "BP")
                {
                    DecPendUSD = (DecTotalUSDDeb - DecTotalUSDCrd) - DecSelectUSDDeb;
                    DecPendInr = (DecTotalInrDeb - DecTotalInrCrd) - DecSelectInrDeb;
                }
                else if (FormType == "CR" || FormType == "BR")
                {
                    DecPendUSD = (DecTotalUSDCrd - DecTotalUSDDeb) - DecSelectUSDDeb;
                    DecPendInr = (DecTotalInrCrd - DecTotalInrDeb) - DecSelectInrDeb;
                }
                if (FormType == "CP" || FormType == "BP")
                {
                    lblUsdPend.Text = Val.ToString((DecTotalUSDDeb - DecTotalUSDCrd));
                    lblInrPend.Text = Val.ToString((DecTotalInrDeb - DecTotalInrCrd));
                }
                else if (FormType == "CR" || FormType == "BR")
                {
                    lblInrPend.Text = Val.ToString((DecTotalUSDCrd - DecTotalUSDDeb));
                    lblUsdPend.Text = Val.ToString((DecTotalInrCrd - DecTotalInrDeb));
                }
                if (FormType == "ALL")
                {
                    DecPendUSD = (DecTotalUSDDeb - DecTotalUSDCrd) - DecSelectUSDDeb;
                    DecPendInr = (DecTotalInrDeb - DecTotalInrCrd) - DecSelectInrDeb;
                    DecPendUSD -= (DecTotalUSDCrd - DecTotalUSDDeb) - DecSelectUSDDeb;
                    DecPendInr -= (DecTotalInrCrd - DecTotalInrDeb) - DecSelectInrDeb;

                    lblUsdPend.Text = Val.ToString(DecPendUSD);
                    lblInrPend.Text = Val.ToString(DecPendInr);
                }
                lblUsdSelec.Text = Val.ToString(DecSelectUSDDeb);
                lblInrSelec.Text = Val.ToString(DecSelectInrDeb);

                lblInrRemn.Text = Val.ToString(DecPendInr);
                lblUsdRemn.Text = Val.ToString(DecPendUSD);
                RspCmbBox_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message.ToString());
            }

        }

        private void mainPickup_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                int TotalData = DtabPaymentDetail.Rows.Count;
                DataTable DTab = Global.GetSelectedRecordOfGrid(GrdDetPayment, true, ObjGridSelection);
                int SelectAll = DTab.Rows.Count;

                if (IsSpaceSelect == false)
                {
                    if (TotalData == SelectAll)
                        CALCULATEBILLAMT();
                    else if (SelectAll == 0)
                        CALCULATEBILLAMT();
                    else
                        CALCULATESINGLE();
                }
                else
                {
                    CALCULATESINGLE();
                }
                GrdDetPayment.RefreshData(); //Added by Daksha on 13/09/2023
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void RspCmbBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GrdDetPayment.PostEditor();
            string PaymentMade = Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PAYMENTMADE"));
            int IntCurrId = Val.ToInt32(GrdDetPayment.GetFocusedRowCellValue("CURRENCY_ID"));
            if (PaymentMade == "Full")
                CalculateDiscount();

            //if (PaymentMade != "Partial" )
            //{
            if (BOConfiguration.gStrLoginSection == "B")
            {

            }
            else
            {
                CalculateExcDiff();
                CalculateExcDiffUSD();
            }
            //}

        }

        private void CalculateExcDiff()
        {
            string PaymentMade = Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PAYMENTMADE"));
            int IntCurrId = Val.ToInt32(GrdDetPayment.GetFocusedRowCellValue("CURRENCY_ID"));
            if (IntCurrId == 1)
            {
                double DouPendAmt, DouBankChrge, DouDiscountChrge, DouPaymntAmtFE, DouPaymntAmt, DouInvExcRate;
                double DouExcRate = Val.Val(lblExcRate.Text);
                DouPaymntAmtFE = Val.Val(GrdDetPayment.GetFocusedRowCellValue("FPAYMENTAMOUNT"));

                DouPaymntAmt = Val.Val(GrdDetPayment.GetFocusedRowCellValue("PAYMENTAMOUNT"));
                DouInvExcRate = Val.Val(GrdDetPayment.GetFocusedRowCellValue("EXCRATE"));
                if (DouPaymntAmtFE > 0)
                {
                    //if (FormType == "CP" || FormType == "BP")
                    //    DouPendAmt = Val.Val(GrdDetPayment.GetFocusedRowCellValue("PENDDEBITAMOUNTFE"));
                    //else
                    //    DouPendAmt = Val.Val(GrdDetPayment.GetFocusedRowCellValue("PENDCREDITAMOUNTFE"));

                    DouPendAmt = DouPaymntAmt * DouInvExcRate;
                    DouBankChrge = Val.Val(GrdDetPayment.GetFocusedRowCellValue("FEBANKCHARGES"));
                    DouDiscountChrge = Val.Val(GrdDetPayment.GetFocusedRowCellValue("FDISCOUNTAMOUNT"));


                    GrdDetPayment.SetFocusedRowCellValue("EXCHRATEDIFF", Math.Round(DouPendAmt - (DouPaymntAmtFE + DouBankChrge + DouDiscountChrge), 2));
                }
            }
            else
                GrdDetPayment.SetFocusedRowCellValue("EXCHRATEDIFF", 0);
        }

        private void CalculateExcDiffUSD()
        {
            string PaymentMade = Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PAYMENTMADE"));
            int IntCurrId = Val.ToInt32(GrdDetPayment.GetFocusedRowCellValue("CURRENCY_ID"));
            //if (IntCurrId != 1)
            if (IntCurrId == 1)
            {
                double DouPendAmt, DouBankChrge, DouDiscountChrge, DouPaymntAmt, DouPaymntAmtFE, DouInvExcRate;
                double DouExcRate = Val.Val(lblExcRate.Text);
                DouPaymntAmt = Val.Val(GrdDetPayment.GetFocusedRowCellValue("PAYMENTAMOUNT"));
                DouPaymntAmtFE = Val.Val(GrdDetPayment.GetFocusedRowCellValue("FPAYMENTAMOUNT"));
                DouPaymntAmtFE += Val.Val(GrdDetPayment.GetFocusedRowCellValue("FEBANKCHARGES"));
                DouPaymntAmtFE += Val.Val(GrdDetPayment.GetFocusedRowCellValue("FDISCOUNTAMOUNT"));
                DouInvExcRate = Val.Val(GrdDetPayment.GetFocusedRowCellValue("EXCRATE"));
                if (DouPaymntAmt > 0)
                {
                    //if (FormType == "CP" || FormType == "BP")
                    //    DouPendAmt = Val.Val(GrdDetPayment.GetFocusedRowCellValue("PENDDEBITAMOUNT"));
                    //else
                    //    DouPendAmt = Val.Val(GrdDetPayment.GetFocusedRowCellValue("PENDCREDITAMOUNT"));

                    DouPendAmt = Math.Round(DouPaymntAmtFE / DouInvExcRate, 2);
                    DouBankChrge = Val.Val(GrdDetPayment.GetFocusedRowCellValue("BANKCHARGES"));
                    DouDiscountChrge = Val.Val(GrdDetPayment.GetFocusedRowCellValue("DISCOUNTAMOUNT"));
                    GrdDetPayment.SetFocusedRowCellValue("EXCHRATEDIFFUSD", Math.Round(DouPendAmt - (DouPaymntAmt + DouBankChrge + DouDiscountChrge), 3));
                }
            }
            else
                GrdDetPayment.SetFocusedRowCellValue("EXCHRATEDIFFUSD", 0);
        }

        private void CalculateDiscount()
        {
            string PaymentMade = Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PAYMENTMADE"));
            int IntCurrId = Val.ToInt32(GrdDetPayment.GetFocusedRowCellValue("CURRENCY_ID"));
            if (PaymentMade == "Full")
            {
                if (IntCurrId == 1)
                {
                    double DouPendAmt, DouBankChrge, DouPaymntAmt;
                    double DouExcRate = Val.Val(lblExcRate.Text);
                    DouPaymntAmt = Val.Val(GrdDetPayment.GetFocusedRowCellValue("PAYMENTAMOUNT"));
                    if (DouPaymntAmt > 0)
                    {
                        if (FormType == "CP" || FormType == "BP" || Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PICKUPTYPE")) == "CP")
                            DouPendAmt = Val.Val(GrdDetPayment.GetFocusedRowCellValue("PENDDEBITAMOUNT"));
                        else
                            DouPendAmt = Val.Val(GrdDetPayment.GetFocusedRowCellValue("PENDCREDITAMOUNT"));
                        DouBankChrge = Val.Val(GrdDetPayment.GetFocusedRowCellValue("BANKCHARGES"));
                        if (DouExcRate != 0)
                        {
                            GrdDetPayment.SetFocusedRowCellValue("DISCOUNTAMOUNT", Math.Round(DouPendAmt - (DouPaymntAmt + DouBankChrge), 2));
                            GrdDetPayment.SetFocusedRowCellValue("FDISCOUNTAMOUNT", Math.Round((DouPendAmt - (DouPaymntAmt + DouBankChrge)) * DouExcRate, 2));
                        }
                    }
                    else
                        GrdDetPayment.SetFocusedRowCellValue("DISCOUNTAMOUNT", 0);
                }
                else
                {
                    double DouPendAmt, DouBankChrge, DouPaymntAmt;
                    double DouExcRate = Val.Val(lblExcRate.Text);
                    DouPaymntAmt = Val.Val(GrdDetPayment.GetFocusedRowCellValue("FPAYMENTAMOUNT"));
                    if (DouPaymntAmt > 0)
                    {
                        if (FormType == "CP" || FormType == "BP" || Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PICKUPTYPE")) == "CP")
                            DouPendAmt = Val.Val(GrdDetPayment.GetFocusedRowCellValue("PENDDEBITAMOUNTFE"));
                        else
                            DouPendAmt = Val.Val(GrdDetPayment.GetFocusedRowCellValue("PENDCREDITAMOUNTFE"));

                        DouBankChrge = Val.Val(GrdDetPayment.GetFocusedRowCellValue("FEBANKCHARGES"));

                        if (DouExcRate != 0)
                        {
                            GrdDetPayment.SetFocusedRowCellValue("FDISCOUNTAMOUNT", Math.Round(DouPendAmt - (DouPaymntAmt + DouBankChrge), 2));
                            GrdDetPayment.SetFocusedRowCellValue("DISCOUNTAMOUNT", Math.Round((DouPendAmt - (DouPaymntAmt + DouBankChrge)) / DouExcRate, 2));
                        }
                    }
                    else
                        GrdDetPayment.SetFocusedRowCellValue("FDISCOUNTAMOUNT", 0);
                }
            }
        }

        private void mainPickup_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                IsSpaceSelect = true;
                mainPickup_MouseUp(null, null);
                IsSpaceSelect = false;
            }
        }

        private void FrmPickupBillAllocation_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void FrmPickupBillAllocation_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void GrdDetPayment_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                GrdDetPayment.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                BtnSave_Click(null, null);
            }
            else if (e.KeyCode == Keys.Escape)
            {
                BtnClose_Click(null, null);
            }
        }

        private void GrdDetPayment_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                    return;

                if (e.Clicks == 2 && e.Column.FieldName == "VOUCHERNOSTR")
                {
                    DataRow Drow = GrdDetPayment.GetFocusedDataRow();
                    string StrBookType = Val.ToString(Drow["BOOKTYPEFULL"]);
                    string StrTrn_id = Val.ToString(Drow["TRN_ID"]);
                    if (StrBookType == "POLISH PURCHASE" || StrBookType == "PURCHASE REUTRN" || StrBookType == "SALE DELIVERY" || StrBookType == "SALE RETURN")
                    {

                        DataTable DtAccTrn_id = ObjFinance.GetMemoIdFromFinanceID(StrTrn_id);
                        if (DtAccTrn_id.Rows.Count > 0)
                        {
                            FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                            FrmMemoEntry.MdiParent = Global.gMainRef;
                            FrmMemoEntry.ShowForm(Val.ToString(DtAccTrn_id.Rows[0][0]), "SINGLE");
                            BtnClose_Click(null, null);
                        }
                    }
                    //comment by gunjan:22/10/2024
                    //else if (StrBookType == "CONTRA ENTRY")
                    //{
                    //    FrmContraEntryNew FrmContraEntryNew = new FrmContraEntryNew();
                    //    FrmContraEntryNew.MdiParent = Global.gMainRef;
                    //    FrmContraEntryNew.ShowForm(StrTrn_id);
                    //}
                    //else if (StrBookType == "CONTRA ENTRY")
                    //{
                    //    FrmContraEntryNew FrmContraEntryNew = new FrmContraEntryNew();
                    //    FrmContraEntryNew.MdiParent = Global.gMainRef;
                    //    FrmContraEntryNew.ShowForm(StrTrn_id);
                    //}
                    //else if (StrBookType == "BANK PAYMENT" || StrBookType == "CASH PAYMENT" || StrBookType == "BANK RECEIPT" || StrBookType == "CASH RECEIPT")
                    //{
                    //    FrmBillWiseEntryNew FrmBillWiseEntryNew = new FrmBillWiseEntryNew();
                    //    FrmBillWiseEntryNew.MdiParent = Global.gMainRef;
                    //    FrmBillWiseEntryNew.ShowForm(StrBookType, StrTrn_id);
                    //}
                    //else if (StrBookType == "PURCHASE EXPENCE")
                    //{
                    //    FrmExpense FrmExpense = new FrmExpense();
                    //    FrmExpense.MdiParent = Global.gMainRef;
                    //    FrmExpense.ShowForm(StrTrn_id);
                    //}
                    //else if (StrBookType == "JOURNAL VOUCHER")
                    //{
                    //    FrmJournalNew FrmJournalNew = new FrmJournalNew();
                    //    FrmJournalNew.MdiParent = Global.gMainRef;
                    //    FrmJournalNew.ShowForm(StrTrn_id);
                    //}
                    //else if (StrBookType == "ROUGH PURCHASE")
                    //{
                    //    DataTable DtAccTrn_id = ObjFinance.GetMemoIdFromFinanceID(StrTrn_id);
                    //    if (DtAccTrn_id.Rows.Count > 0)
                    //    {
                    //        FrmRoughPurchaseEntry FrmRoughPurchaseEntry = new FrmRoughPurchaseEntry();
                    //        FrmRoughPurchaseEntry.MdiParent = Global.gMainRef;
                    //        FrmRoughPurchaseEntry.ShowForm(Val.ToString(DtAccTrn_id.Rows[0][0]), "ALL");
                    //    }
                    //}
                    //if (StrBookType == "PARCEL POLISH PURCHASE" || StrBookType == "PARCEL PURCHASE REUTRN" || StrBookType == "PARCEL SALE DELIVERY" || StrBookType == "PARCEL SALE RETURN")
                    //{
                    //    DataTable DtAccTrn_id = ObjFinance.GetMemoIdFromFinanceID(StrTrn_id);
                    //    if (DtAccTrn_id.Rows.Count > 0)
                    //    {
                    //        FrmMemoEntryParcel FrmMemoEntryParcel = new FrmMemoEntryParcel();
                    //        FrmMemoEntryParcel.MdiParent = Global.gMainRef;
                    //        FrmMemoEntryParcel.ShowForm(Val.ToString(DtAccTrn_id.Rows[0][0]), "PARCEL");
                    //    }
                    //}
                    //End As Gunjan
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        void CALCULATEBILLAMT()
        {
            try
            {
                DataTable DTab = Global.GetSelectedRecordOfGrid(GrdDetPayment, true, ObjGridSelection);
                if (FormType == "CP" || FormType == "BP")
                {
                    
                   if (DTab != null && DTab.Rows.Count > 0)
                    {
                        if (Val.ToString(GrdDetPayment.GetFocusedRowCellValue("CURRENCY")) == "USD")
                        {
                            for (int i = 0; i < DTab.Rows.Count; i++)
                            {
                                DTab.Rows[i]["FPAYMENTAMOUNT"] = DTab.Rows[i]["PENDDEBITAMOUNTFE"];
                                DtabPaymentDetail.Select("TRN_ID = '" + Val.ToString(DTab.Rows[i]["TRN_ID"]) + "'")
                               .ToList<DataRow>()
                               .ForEach(p =>
                               {
                                   p.SetField<double>("FPAYMENTAMOUNT", Val.Val(DTab.Rows[i]["PENDDEBITAMOUNTFE"]));
                                   p.SetField<string>("PAYMENTMADE", Val.ToString("Full"));
                                   p.SetField<double>("PAYMENTAMOUNT", Val.Val(DTab.Rows[i]["PENDDEBITAMOUNTFE"]));
                               });
                            }
                            DTab.AcceptChanges();
                        }
                        else
                        {
                            for (int i = 0; i < DTab.Rows.Count; i++)
                            {
                                DTab.Rows[i]["FPAYMENTAMOUNT"] = DTab.Rows[i]["PENDDEBITAMOUNTFE"];
                                DtabPaymentDetail.Select("TRN_ID = '" + Val.ToString(DTab.Rows[i]["TRN_ID"]) + "'")
                               .ToList<DataRow>()
                               .ForEach(p =>
                               {
                                   p.SetField<double>("FPAYMENTAMOUNT", Val.Val(DTab.Rows[i]["PENDDEBITAMOUNTFE"]));
                                   p.SetField<string>("PAYMENTMADE", Val.ToString("Full"));
                                   p.SetField<double>("PAYMENTAMOUNT", Val.Val(DTab.Rows[i]["PENDDEBITAMOUNTFE"]));
                               });
                            }
                            DTab.AcceptChanges();
                        }
                    }
                    else
                    {
                        ObjGridSelection.ClearSelection();
                        DtabPaymentDetail.AsEnumerable().ToList().ForEach(p => {
                            p.SetField<double>("FPAYMENTAMOUNT", 0);
                            p.SetField<string>("PAYMENTMADE", Val.ToString("Partial"));
                            p.SetField<double>("PAYMENTAMOUNT", 0);
                            //p.SetField<double>("PENDDEBITAMOUNTFE", 0); //Comment by Daksha on 14/09/2023
                        });
                    }
                }
                else if (FormType == "CR" || FormType == "BR")
                {
                    if (DTab != null && DTab.Rows.Count > 0)
                    {
                        if (Val.ToString(GrdDetPayment.GetFocusedRowCellValue("CURRENCY")) == "USD")
                        {
                            for (int i = 0; i < DTab.Rows.Count; i++)
                            {
                                DTab.Rows[i]["FPAYMENTAMOUNT"] = DTab.Rows[i]["PENDCREDITAMOUNTFE"];
                                DtabPaymentDetail.Select("TRN_ID = '" + Val.ToString(DTab.Rows[i]["TRN_ID"]) + "'")
                               .ToList<DataRow>()
                               .ForEach(p =>
                               {
                                   p.SetField<double>("FPAYMENTAMOUNT", Val.Val(DTab.Rows[i]["PENDCREDITAMOUNTFE"]));
                                   p.SetField<string>("PAYMENTMADE", Val.ToString("Full"));
                                   p.SetField<double>("PAYMENTAMOUNT", Val.Val(DTab.Rows[i]["PENDCREDITAMOUNT"]));
                               });
                            }
                            DTab.AcceptChanges();
                        }
                        else
                        {
                            for (int j = 0; j < DTab.Rows.Count; j++)
                            {
                                DTab.Rows[j]["FPAYMENTAMOUNT"] = DTab.Rows[j]["PENDCREDITAMOUNTFE"];
                                DtabPaymentDetail.Select("TRN_ID = '" + Val.ToString(DTab.Rows[j]["TRN_ID"]) + "'")
                               .ToList<DataRow>()
                               .ForEach(p => {
                                   p.SetField<double>("FPAYMENTAMOUNT", Val.Val(DTab.Rows[j]["PENDCREDITAMOUNTFE"]));
                                   p.SetField<string>("PAYMENTMADE", Val.ToString("Full"));
                                   p.SetField<double>("PAYMENTAMOUNT", Val.Val(DTab.Rows[j]["PENDCREDITAMOUNTFE"]));
                               });
                            }
                            DTab.AcceptChanges();
                        }
                    }
                    else
                    {
                        ObjGridSelection.ClearSelection();
                        DtabPaymentDetail.AsEnumerable().ToList().ForEach(p => {
                            p.SetField<double>("FPAYMENTAMOUNT", 0);
                            p.SetField<string>("PAYMENTMADE", Val.ToString("Partial"));
                            p.SetField<double>("PAYMENTAMOUNT", 0);
                        });
                    }
                }
                
                calculate();
                if (IntCurrency_ID == 1 && BOConfiguration.gStrLoginSection != "B")
                    CalculateExcDiff();
                else if (IntCurrency_ID == 0)
                    CalculateExcDiffUSD();
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message.ToString());
            }
        }

        void CALCULATESINGLE()
        {
            GrdDetPayment.PostEditor();
            if (Val.ToBoolean(GrdDetPayment.GetFocusedRowCellValue("COLSELECTCHECKBOX")) == true)
            {
                if (Val.Val(GrdDetPayment.GetFocusedRowCellValue("PAYMENTAMOUNT")) == 0)
                {
                    if (FormType == "CP" || FormType == "BP" || Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PICKUPTYPE")) == "CP")
                    {
                        if (Val.ToString(GrdDetPayment.GetFocusedRowCellValue("CURRENCY")) == "USD")
                        {
                            GrdDetPayment.SetFocusedRowCellValue("PAYMENTAMOUNT", Convert.ToDouble(GrdDetPayment.GetFocusedRowCellValue("PENDDEBITAMOUNT")));
                            GrdDetPayment.SetFocusedRowCellValue("FPAYMENTAMOUNT", Convert.ToDouble(GrdDetPayment.GetFocusedRowCellValue("PENDDEBITAMOUNT")) * Val.Val(lblExcRate.Text));
                        }
                        else if (Val.ToString(GrdDetPayment.GetFocusedRowCellValue("CURRENCY")) != "USD")
                        {
                            if (Val.Val(lblExcRate.Text) != 0)
                                GrdDetPayment.SetFocusedRowCellValue("PAYMENTAMOUNT", Math.Round(Convert.ToDouble(GrdDetPayment.GetFocusedRowCellValue("PENDDEBITAMOUNTFE")) / Val.Val(lblExcRate.Text), 3));
                            else
                                GrdDetPayment.SetFocusedRowCellValue("PAYMENTAMOUNT", Convert.ToDouble(GrdDetPayment.GetFocusedRowCellValue("PENDDEBITAMOUNTFE")));

                            GrdDetPayment.SetFocusedRowCellValue("FPAYMENTAMOUNT", Convert.ToDouble(GrdDetPayment.GetFocusedRowCellValue("PENDDEBITAMOUNTFE")));
                        }
                    }
                    else if (FormType == "CR" || FormType == "BR" || Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PICKUPTYPE")) == "CR")
                    {
                        if (Val.ToString(GrdDetPayment.GetFocusedRowCellValue("CURRENCY")) == "USD")
                        {
                            GrdDetPayment.SetFocusedRowCellValue("PAYMENTAMOUNT", Convert.ToDouble(GrdDetPayment.GetFocusedRowCellValue("PENDCREDITAMOUNT")));
                            GrdDetPayment.SetFocusedRowCellValue("FPAYMENTAMOUNT", Convert.ToDouble(GrdDetPayment.GetFocusedRowCellValue("PENDCREDITAMOUNT")) * Val.Val(lblExcRate.Text));
                        }
                        else if (Val.ToString(GrdDetPayment.GetFocusedRowCellValue("CURRENCY")) != "USD")
                        {
                            if (Val.Val(lblExcRate.Text) != 0)
                                GrdDetPayment.SetFocusedRowCellValue("PAYMENTAMOUNT", Math.Round(Convert.ToDouble(GrdDetPayment.GetFocusedRowCellValue("PENDCREDITAMOUNTFE")) / Val.Val(lblExcRate.Text), 3));
                            else
                                GrdDetPayment.SetFocusedRowCellValue("PAYMENTAMOUNT", Convert.ToDouble(GrdDetPayment.GetFocusedRowCellValue("PENDCREDITAMOUNTFE")));

                            GrdDetPayment.SetFocusedRowCellValue("FPAYMENTAMOUNT", Convert.ToDouble(GrdDetPayment.GetFocusedRowCellValue("PENDCREDITAMOUNTFE")));
                        }
                    }
                }
            }
            else if (Val.ToBoolean(GrdDetPayment.GetFocusedRowCellValue("COLSELECTCHECKBOX")) == false)
            {
                GrdDetPayment.SetFocusedRowCellValue("FPAYMENTAMOUNT", 0);
                GrdDetPayment.SetFocusedRowCellValue("PAYMENTAMOUNT", 0);
                GrdDetPayment.SetFocusedRowCellValue("BANKCHARGES", 0);
                GrdDetPayment.SetFocusedRowCellValue("FEBANKCHARGES", 0);
            }
            calculate();
            string PaymentMade = Val.ToString(GrdDetPayment.GetFocusedRowCellValue("PAYMENTMADE"));
            //if (PaymentMade != "Partial" && IntCurrency_ID == 1)
            //if (IntCurrency_ID == 1)  commnet by shiv 
            if (IntCurrency_ID == 1 && BOConfiguration.gStrLoginSection != "B")
                 CalculateExcDiff();
            else if(IntCurrency_ID == 0)
                CalculateExcDiffUSD();
        }

    }
}
