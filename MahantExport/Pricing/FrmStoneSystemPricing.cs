using MahantExport;
using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.Data;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using Google.API.Translate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace MahantExport.Pricing
{
    public partial class FrmStoneSystemPricing : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        BOTRN_StoneSystemPricing ObjTrn = new BOTRN_StoneSystemPricing();
        BOFormPer ObjPer = new BOFormPer();

        DataTable DTabRapaportData = new DataTable();

        DataTable DTabStoneReviseData = new DataTable();

        double DouCarat = 0;
        double DouCostRapaport = 0;
        double DouCostRapaportAmt = 0;
        double DouCostDisc = 0;
        double DouCostPricePerCarat = 0;
        double DouCostAmount = 0;

        double DouSaleRapaport = 0;
        double DouSaleRapaportAmt = 0;
        double DouSaleDisc = 0;
        double DouSalePricePerCarat = 0;
        double DouSaleAmount = 0;


        double DouAvgRapaport = 0;
        double DouAvgRapaportAmt = 0;
        double DouAvgDisc = 0;
        double DouAvgPricePerCarat = 0;
        double DouAvgAmount = 0;

        double DouMFGRapaport = 0;
        double DouMFGRapaportAmt = 0;
        double DouMFGDisc = 0;
        double DouMFGPricePerCarat = 0;
        double DouMFGAmount = 0;

        #region Property Settings

        public FrmStoneSystemPricing()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            if (ObjPer.ISVIEW == false)
            {
                Global.MessageError(BusLib.TPV.BOMessage.ViewDeniedMsg);
                return;
            }

            this.Show();

            FillCombo();

            ChkMFGPricing.Checked = true;
            ChkCOSTPricing.Checked = false;
            ChkAVGPricing.Checked = false;
            ChkSALEPricing.Checked = false;
            ChkMFGPricing_CheckedChanged(null, null);
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjTrn);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);

        }

        #endregion

        #region Rapaport

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        private void FillCombo()
        {
            this.Cursor = Cursors.WaitCursor;
            cmbShape.Properties.ValueMember = "SHAPE_ID";
            cmbShape.Properties.DisplayMember = "SHAPENAME";
            cmbShape.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SHAPE);
            cmbColor.Properties.ValueMember = "COLOR_ID";
            cmbColor.Properties.DisplayMember = "COLORNAME";
            cmbColor.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COLOR);
            cmbClarity.Properties.ValueMember = "CLARITY_ID";
            cmbClarity.Properties.DisplayMember = "CLARITYCODE";
            cmbClarity.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CLARITY);
            cmbCut.Properties.ValueMember = "CUT_ID";
            cmbCut.Properties.DisplayMember = "CUTNAME";
            cmbCut.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CUT);
            cmbPol.Properties.ValueMember = "POL_ID";
            cmbPol.Properties.DisplayMember = "POLNAME";
            cmbPol.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_POL);
            cmbSym.Properties.ValueMember = "SYM_ID";
            cmbSym.Properties.DisplayMember = "SYMNAME";
            cmbSym.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SYM);
            cmbFL.Properties.ValueMember = "FL_ID";
            cmbFL.Properties.DisplayMember = "FLNAME";
            cmbFL.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_FL);

            DataTable DTabRapDate = new BOTRN_ParameterDiscount().GetParameterDiscountData("RAPDATE", "ORIGINAL_RAP", "", "", 0, 0, "");
            DTabRapDate.DefaultView.Sort = "RAPDATE DESC";
            DTabRapDate = DTabRapDate.DefaultView.ToTable();

            cmbRapDate.Items.Clear();
            foreach (DataRow DRow in DTabRapDate.Rows)
            {
                cmbRapDate.Items.Add(DateTime.Parse(Val.ToString(DRow["RAPDATE"])).ToString("dd/MM/yyyy"));
            }
            cmbRapDate.SelectedIndex = 0;

            this.Cursor = Cursors.Default;
        }

        private void CalculateTotalSummary()
        {
            try
            {

                txtTotalPackets.Text = GrdDetail.Columns["PCS"].SummaryItem.SummaryValue.ToString();
                txtTotalPcs.Text = GrdDetail.Columns["PCS"].SummaryItem.SummaryValue.ToString();
                txtTotalCarat.Text = GrdDetail.Columns["CARAT"].SummaryItem.SummaryValue.ToString();

                //sale
                txtTotalDisc.Text = GrdDetail.Columns["SALEDISCOUNT"].SummaryItem.SummaryValue.ToString();
                txtTotalPricePerCarat.Text = GrdDetail.Columns["SALEPRICEPERCARAT"].SummaryItem.SummaryValue.ToString();
                txtTotalAmount.Text = GrdDetail.Columns["SALEAMOUNT"].SummaryItem.SummaryValue.ToString();

                //mfg
                txtMfgDisc.Text = GrdDetail.Columns["MFGDISCOUNT"].SummaryItem.SummaryValue.ToString();
                txtMfgPricePerCarat.Text = GrdDetail.Columns["MFGPRICEPERCARAT"].SummaryItem.SummaryValue.ToString();
                txtMfgTotalAmount.Text = GrdDetail.Columns["MFGAMOUNT"].SummaryItem.SummaryValue.ToString();

                //cost
                txtCostDisc.Text = GrdDetail.Columns["COSTDISCOUNT"].SummaryItem.SummaryValue.ToString();
                txtCostPricePerCarat.Text = GrdDetail.Columns["COSTPRICEPERCARAT"].SummaryItem.SummaryValue.ToString();
                txtCostTotalAmount.Text = GrdDetail.Columns["COSTAMOUNT"].SummaryItem.SummaryValue.ToString();

                //avg
                txtAvgDisc.Text = GrdDetail.Columns["AVGDISCOUNT"].SummaryItem.SummaryValue.ToString();
                txtAvgPricePerCarat.Text = GrdDetail.Columns["AVGPRICEPERCARAT"].SummaryItem.SummaryValue.ToString();
                txtAvgTotalAmount.Text = GrdDetail.Columns["AVGAMOUNT"].SummaryItem.SummaryValue.ToString();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void Clear()
        {
            cmbShape.SetEditValue("");
            cmbColor.SetEditValue("");
            cmbClarity.SetEditValue("");
            cmbPol.SetEditValue("");
            cmbSym.SetEditValue("");
            cmbCut.SetEditValue("");
            cmbFL.SetEditValue("");
            txtFromCarat.Text = "";
            txtToCarat.Text = "";
            cmbRapDate.Text = "";
            txtTotalPackets.Text = "";
            txtTotalPcs.Text = "";
            txtTotalCarat.Text = "";
            txtTotalDisc.Text = "";
            txtTotalPricePerCarat.Text = "";
            txtTotalAmount.Text = "";
            MainGrdDetail.DataSource = null;
        }

        private void FrmPriceRevised_Load(object sender, EventArgs e)
        {
        }

        private void btnStoneRevShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbRapDate.Text.Trim() == "")
                {
                    Global.Message("Please Select Rap Date..");
                    return;
                }

                this.Cursor = Cursors.WaitCursor;


                LiveStockProperty LStockProperty = new LiveStockProperty();

                LStockProperty.MULTYSHAPE_ID = cmbShape.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYCOLOR_ID = cmbColor.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYCLARITY_ID = cmbClarity.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYCUT_ID = cmbCut.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYPOL_ID = cmbPol.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYSYM_ID = cmbSym.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYFL_ID = cmbFL.EditValue.ToString().Replace(" ", "");

                LStockProperty.FROMCARAT = Val.Val(txtFromCarat.Text);
                LStockProperty.TOCARAT = Val.Val(txtToCarat.Text);
                LStockProperty.STOCKNO = Val.ToString(txtStoneNo.Text);

                DTabStoneReviseData.TableName = "Detail";
                DTabStoneReviseData = ObjTrn.GetStoneSystemPricingData(LStockProperty, Val.ToString(cmbRapDate.SelectedItem));

                MainGrdDetail.DataSource = DTabStoneReviseData;
                GrdDetail.RefreshData();

                //int IntAll = 0;
                //int IntIdle = 0;
                //int IntUP = 0;
                //int IntDown = 0;

                //foreach (DataRow DRow in DTabStoneReviseData.Rows)
                //{
                //    IntAll++;
                //    if (Val.Val(DRow["SALERAPAPORT"]) == Val.Val(DRow["NEWRAPAPORT"]))
                //    {
                //        IntIdle++;
                //    }
                //    else if (Val.Val(DRow["NEWRAPAPORT"]) > Val.Val(DRow["SALERAPAPORT"]))
                //    {
                //        IntUP++;
                //    }
                //    else if (Val.Val(DRow["NEWRAPAPORT"]) < Val.Val(DRow["SALERAPAPORT"]))
                //    {
                //        IntDown++;
                //    }
                //}
                CalculateTotalSummary();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDetail_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            try
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    DouCarat = 0;
                    DouCostRapaport = 0;
                    DouCostRapaportAmt = 0;
                    DouCostDisc = 0;
                    DouCostPricePerCarat = 0;
                    DouCostAmount = 0;

                    DouSaleRapaport = 0;
                    DouSaleRapaportAmt = 0;
                    DouSaleDisc = 0;
                    DouSalePricePerCarat = 0;
                    DouSaleAmount = 0;

                    DouAvgRapaport = 0;
                    DouAvgRapaportAmt = 0;
                    DouAvgDisc = 0;
                    DouAvgPricePerCarat = 0;
                    DouAvgAmount = 0;

                    DouMFGRapaport = 0;
                    DouMFGRapaportAmt = 0;
                    DouMFGDisc = 0;
                    DouMFGPricePerCarat = 0;
                    DouMFGAmount = 0;

                }
                else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DouCarat = DouCarat + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT"));
                    DouCostAmount = DouCostAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "COSTAMOUNT"));
                    DouCostRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "COSTRAPAPORT"));
                    DouCostPricePerCarat = DouCostAmount / DouCarat;
                    DouCostRapaportAmt = DouCostRapaportAmt + (DouCostRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

                    DouSaleAmount = DouSaleAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "SALEAMOUNT"));
                    DouSaleRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "SALERAPAPORT"));
                    DouSalePricePerCarat = DouSaleAmount / DouCarat;
                    DouSaleRapaportAmt = DouSaleRapaportAmt + (DouSaleRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

                    DouAvgAmount = DouAvgAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "AVGAMOUNT"));
                    DouAvgRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "AVGRAPAPORT"));
                    DouAvgPricePerCarat = DouAvgAmount / DouCarat;
                    DouAvgRapaportAmt = DouAvgRapaportAmt + (DouAvgRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

                    DouMFGAmount = DouMFGAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "MFGAMOUNT"));
                    DouMFGRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "MFGRAPAPORT"));
                    DouMFGPricePerCarat = DouMFGAmount / DouCarat;
                    DouMFGRapaportAmt = DouMFGRapaportAmt + (DouMFGRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));
                }
                else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("COSTPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouCostAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("COSTRAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouCostRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("COSTDISCOUNT") == 0)
                    {
                        DouCostRapaport = Math.Round((DouCostRapaportAmt / DouCarat), 2);
                        DouCostDisc = Math.Round(((DouCostRapaport - DouCostPricePerCarat) / DouCostRapaport * 100), 2);
                        e.TotalValue = DouCostDisc;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("SALEPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouSaleAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("SALERAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouSaleRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("SALEDISCOUNT") == 0)
                    {
                        DouSaleRapaport = Math.Round(DouSaleRapaportAmt / DouCarat);
                        DouSaleDisc = Math.Round(((DouSaleRapaport - DouSalePricePerCarat) / DouSaleRapaport * 100), 2);
                        e.TotalValue = DouSaleDisc;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("AVGPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouSaleAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("AVGRAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouSaleRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("AVGDISCOUNT") == 0)
                    {
                        DouSaleRapaport = Math.Round(DouSaleRapaportAmt / DouCarat);
                        DouSaleDisc = Math.Round(((DouSaleRapaport - DouSalePricePerCarat) / DouSaleRapaport * 100), 2);
                        e.TotalValue = DouSaleDisc;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MFGPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouMFGAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MFGRAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouMFGRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MFGDISCOUNT") == 0)
                    {
                        DouMFGRapaport = Math.Round((DouMFGRapaportAmt / DouCarat), 2);
                        DouMFGDisc = Math.Round(((DouMFGRapaport - DouMFGPricePerCarat) / DouMFGRapaport * 100), 2);
                        e.TotalValue = DouCostDisc;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void btnStoneRevUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                /*
                this.Cursor = Cursors.WaitCursor;
                BOTRN_PriceRevised ObjMemo = new BOTRN_PriceRevised();
                MemoEntryProperty ObjMstProper = new MemoEntryProperty();
                ObjMstProper.MEMO_ID = null;
                ObjMstProper.MEMONO = 0;
                ObjMstProper.MEMOTYPE = "PRICING";
                ObjMstProper.MEMODATE = Val.SqlDate(System.DateTime.Now.ToShortDateString());

                ObjMstProper.BILLINGPARTY_ID = null;
                ObjMstProper.SHIPPINGPARTY_ID = null;

                ObjMstProper.BROKER_ID = null;
                ObjMstProper.BROKERBASEPER = 0;
                ObjMstProper.BROKERPROFITPER = 0;
                ObjMstProper.ADAT_ID = null;
                ObjMstProper.ADATPER = 0;
                ObjMstProper.SOLDPARTY_ID = null;

                ObjMstProper.SELLER_ID = null;
                ObjMstProper.TERMS_ID =0;
                ObjMstProper.TERMSDAYS = 0;
                ObjMstProper.TERMSPER =0;
                ObjMstProper.TERMSDATE = null;

                ObjMstProper.CURRENCY_ID = 0;
                ObjMstProper.EXCRATE = 0;

                ObjMstProper.MEMODISCOUNT = 0;

                ObjMstProper.BILLINGADDRESS1 = "";
                ObjMstProper.BILLINGADDRESS2 = "";
                ObjMstProper.BILLINGADDRESS3 = "";
                ObjMstProper.BILLINGCOUNTRY_ID = 0;
                ObjMstProper.BILLINGSTATE ="";
                ObjMstProper.BILLINGCITY = "";
                ObjMstProper.BILLINGZIPCODE ="";

                ObjMstProper.SHIPPINGADDRESS1 = "";
                ObjMstProper.SHIPPINGADDRESS2 = "";
                ObjMstProper.SHIPPINGADDRESS3 = "";
                ObjMstProper.SHIPPINGCOUNTRY_ID =0;
                ObjMstProper.SHIPPINGSTATE ="";
                ObjMstProper.SHIPPINGCITY ="";
                ObjMstProper.SHIPPINGZIPCODE = "";

                ObjMstProper.TOTALPCS = 0;
                ObjMstProper.TOTALCARAT = 0;

                ObjMstProper.GROSSAMOUNT = Val.Val(txtTotalAmount.Text);
                ObjMstProper.DISCOUNTPER = 0;
                ObjMstProper.DISCOUNTAMOUNT = 0;
                ObjMstProper.INSURANCEPER = 0;
                ObjMstProper.INSURANCEAMOUNT =0;
                ObjMstProper.SHIPPINGPER =0;
                ObjMstProper.SHIPPINGAMOUNT = 0;
                ObjMstProper.GSTPER = 0;
                ObjMstProper.GSTAMOUNT = 0;
                ObjMstProper.NETAMOUNT = Val.Val(txtTotalAmount.Text);

                ObjMstProper.REMARK = "STONE REVISION PRICING";
                ObjMstProper.SOURCE = "SOFTWARE";
                ObjMstProper.PROCESS_ID = 13;
                ObjMstProper.PROCESSNAME = "PRICING";

                
                XmlDocument xmlSearchStone = Global.ConvertToXml(ObjMstProper);
                string MemoEntryMasterRecordsForXML = "<DocumentElement><ParamList>" + xmlSearchStone.DocumentElement.InnerXml + "</ParamList></DocumentElement>";
                string MemoEntryDetailForXML;
                using (StringWriter sw = new StringWriter())
                {
                    DTabStoneReviseData.TableName ="Detail";
                    DTabStoneReviseData.WriteXml(sw);
                    MemoEntryDetailForXML = sw.ToString();
                }

                string ReturnMessageDesc = "";
                string ReturnMessageType = "";
                ObjMstProper = ObjMemo.SavePricingMemoEntry(MemoEntryMasterRecordsForXML, MemoEntryDetailForXML,ChkUpdateExpPrice.Checked,Val.ToString(cmbRapDate.SelectedItem));

                ReturnMessageDesc = ObjMstProper.ReturnMessageDesc;
                ReturnMessageType = ObjMstProper.ReturnMessageType;

                ObjMstProper = null;
                Global.Message(ReturnMessageDesc);
                if (ReturnMessageType == "SUCCESS")
                {
                    this.Clear();
                }
                this.Cursor = Cursors.Default;
                */

                if (Global.Confirm("Are You Sure You Want Update System Price Into Live Price?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                string PriceParameterUpdateXml;
                using (StringWriter sw = new StringWriter())
                {
                    DTabStoneReviseData.TableName = "Detail";
                    DTabStoneReviseData.WriteXml(sw);
                    PriceParameterUpdateXml = sw.ToString();
                }

                MemoEntryProperty Property = new MemoEntryProperty();
                Property = ObjTrn.SaveStoneSystemPriceUpdateDetail(PriceParameterUpdateXml, ChkSALEPricing.Checked, ChkMFGPricing.Checked, ChkCOSTPricing.Checked, ChkAVGPricing.Checked);
                Global.Message(Property.ReturnMessageDesc);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    DTabStoneReviseData.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void btnStoneRevExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("Stone Revise List", GrdDetail);
        }

        private void btnStoneRevExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtStoneNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String str1 = txtStoneNo.Text.Trim().Replace("\r\n", ",");
                txtStoneNo.Text = str1;
                txtStoneNo.Select(txtStoneNo.Text.Length, 0);

                string[] Str = str1.Split(',');
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDetail_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Clicks == 2 && e.Column.FieldName == "COMPDISCOUNT")
            {
                string StrXml = Val.ToString(GrdDetail.GetFocusedRowCellValue("BACKDETAIL"));
                if (StrXml == "")
                {
                    return;
                }
                StringReader theReader = new StringReader(StrXml);

                DataSet ds = new DataSet();
                ds.ReadXml(theReader);

                DataTable DTab = ds.Tables[0];

                DataTable DTabDiscountDetail = new DataTable();

                DTabDiscountDetail.Columns.Add(new DataColumn("KEY", typeof(string)));
                DTabDiscountDetail.Columns.Add(new DataColumn("VALUE", typeof(string)));

                if (DTab != null && DTab.Rows.Count != 0)
                {
                    foreach (DataColumn Col in DTab.Columns)
                    {
                        if (Col.ColumnName.ToUpper() == "STOCK_ID")
                        {
                            continue;
                        }
                        DTabDiscountDetail.Rows.Add(Val.ProperText(Col.ColumnName.ToUpper().Replace("_", " ")), Val.ToString(DTab.Rows[0][Col.ColumnName]));
                    }
                }

                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "KEY,VALUE";
                FrmSearch.mStrSearchText = "";
                this.Cursor = Cursors.WaitCursor;
                FrmSearch.mDTab = DTabDiscountDetail;
                FrmSearch.mStrColumnsToHide = "";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.DRow != null)
                {

                }

                DTabDiscountDetail.Dispose();
                DTabDiscountDetail = null;

                DTab.Dispose();
                DTab = null;

                ds.Dispose();
                ds = null;

                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
        }

        private void ChkMFGPricing_CheckedChanged(object sender, EventArgs e)
        {
            GrdDetail.Bands["BANDMFG"].Visible = ChkMFGPricing.Checked;
            GrdDetail.Bands["BANDCOST"].Visible = ChkCOSTPricing.Checked;
            GrdDetail.Bands["BANDAVG"].Visible = ChkAVGPricing.Checked;
            GrdDetail.Bands["BANDSALE"].Visible = ChkSALEPricing.Checked;
            //GrdDetail.Columns["MFGRAPAPORT"].Visible = ChkMFGPricing.Checked;
            //GrdDetail.Columns["MFGDISCOUNT"].Visible = ChkMFGPricing.Checked;
            //GrdDetail.Columns["MFGPRICEPERCARAT"].Visible = ChkMFGPricing.Checked;
            //GrdDetail.Columns["MFGAMOUNT"].Visible = ChkMFGPricing.Checked;
        }

        private void ChkCOSTPricing_CheckedChanged(object sender, EventArgs e)
        {
            GrdDetail.Bands["BANDCOST"].Visible = ChkCOSTPricing.Checked;
            //GrdDetail.Columns["COSTRAPAPORT"].Visible = ChkCOSTPricing.Checked;
            //GrdDetail.Columns["COSTDISCOUNT"].Visible = ChkMFGPricing.Checked;
            //GrdDetail.Columns["COSTPRICEPERCARAT"].Visible = ChkMFGPricing.Checked;
            //GrdDetail.Columns["COSTAMOUNT"].Visible = ChkMFGPricing.Checked;
        }

        private void ChkAVGPricing_CheckedChanged(object sender, EventArgs e)
        {
            GrdDetail.Bands["BANDAVG"].Visible = ChkAVGPricing.Checked;
            //GrdDetail.Columns["AVGRAPAPORT"].Visible = ChkAVGPricing.Checked;
            //GrdDetail.Columns["AVGDISCOUNT"].Visible = ChkAVGPricing.Checked;
            //GrdDetail.Columns["AVGPRICEPERCARAT"].Visible = ChkAVGPricing.Checked;
            //GrdDetail.Columns["AVGAMOUNT"].Visible = ChkAVGPricing.Checked;
        }

        private void ChkSALEPricing_CheckedChanged(object sender, EventArgs e)
        {
            GrdDetail.Bands["BANDSALE"].Visible = ChkSALEPricing.Checked;
            //GrdDetail.Columns["SALERAPAPORT"].Visible = ChkSALEPricing.Checked;
            //GrdDetail.Columns["SALEDISCOUNT"].Visible = ChkSALEPricing.Checked;
            //GrdDetail.Columns["SALEPRICEPERCARAT"].Visible = ChkSALEPricing.Checked;
            //GrdDetail.Columns["SALEAMOUNT"].Visible = ChkSALEPricing.Checked;
        }

    }
}
