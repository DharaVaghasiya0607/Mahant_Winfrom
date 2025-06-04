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

namespace MahantExport.Stock
{
    public partial class FrmRapnetPriceCostUpdate: DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        BOTRN_RapnetPrieCostUpdate ObjTrn = new BOTRN_RapnetPrieCostUpdate();
        BOFormPer ObjPer = new BOFormPer();

        DataTable DTabRapaportData = new DataTable();

        DataTable DTabStoneReviseData = new DataTable();

        double DouCarat = 0;
        double DouNewRapaport = 0;
        double DouNewRapaportAmt = 0;
        double DouNewDisc = 0;
        double DouNewPricePerCarat = 0;
        double DouNewAmount = 0;

        double DouSaleRapaport = 0;
        double DouSaleRapaportAmt = 0;
        double DouSaleDisc = 0;
        double DouSalePricePerCarat = 0;
        double DouSaleAmount = 0;


        double DouCompRapaport = 0;
        double DouCompRapaportAmt = 0;
        double DouCompDisc = 0;
        double DouCompPricePerCarat = 0;
        double DouCompAmount = 0;

        double DouDiffRapaport = 0;
        double DouDiffRapaportAmt = 0;
        double DouDiffDisc = 0;
        double DouDiffPricePerCarat = 0;
        double DouDiffAmount = 0;

        #region Property Settings

        public FrmRapnetPriceCostUpdate()
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

            DataTable DTabRapDate =  new BOTRN_ParameterDiscount().GetParameterDiscountData("RAPDATE", "ORIGINAL_RAP", "", "", 0, 0,"");
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

        private void FillStoneReviseGrid()
        {
            if (DTabStoneReviseData.Rows.Count ==0)
            {
                MainGrdDetail.DataSource = null;
                return;
            }
           
            DataView dv = new DataView(DTabStoneReviseData);
            if (rbtnIdle.Checked)
            {
                dv.RowFilter = "FILTERSTATUS = 0";    
            }
            else if (rbtnUp.Checked)
            {
                dv.RowFilter = "FILTERSTATUS = 1";
            }
            else if (rbtnDown.Checked)
            {
                dv.RowFilter = "FILTERSTATUS = 2";
            }


            MainGrdDetail.DataSource = dv.ToTable();
            MainGrdDetail.Refresh();
            //GrdDetail.BestFitColumns();
            CalculateTotalSummary();
         
        }

        private void CalculateTotalSummary()
        {
            try
            {
                txtTotalPackets.Text = GrdDetail.Columns["PCS"].SummaryItem.SummaryValue.ToString();
                txtSelectedPackets.Text = txtTotalPackets.Text;
                txtDiffPackets.Text = "0";
                txtTotalPcs.Text = GrdDetail.Columns["PCS"].SummaryItem.SummaryValue.ToString();
                txtSelectedPcs.Text = txtTotalPcs.Text;
                txtDiffPcs.Text = "0";
                txtTotalCarat.Text = GrdDetail.Columns["CARAT"].SummaryItem.SummaryValue.ToString();
                txtSelectedCarat.Text = txtTotalCarat.Text;
                txtDiffCarat.Text = "0";

                txtTotalDisc.Text = GrdDetail.Columns["COSTDISCOUNT"].SummaryItem.SummaryValue.ToString();
                txtSelectedDisc.Text = GrdDetail.Columns["NEWDISCOUNT"].SummaryItem.SummaryValue.ToString();
                txtDiffDisc.Text = Val.Format( Val.ToDouble(txtSelectedDisc.Text) - Val.ToDouble(txtTotalDisc.Text),"####0.00");
                txtTotalPricePerCarat.Text = GrdDetail.Columns["COSTPRICEPERCARAT"].SummaryItem.SummaryValue.ToString();
                txtSelectedPricePerCarat.Text = GrdDetail.Columns["NEWPRICEPERCARAT"].SummaryItem.SummaryValue.ToString();
                txtDiffPricePerCarat.Text = Val.Format(Val.ToDouble(txtSelectedPricePerCarat.Text) - Val.ToDouble(txtTotalPricePerCarat.Text), "####0.00");
                txtTotalAmount.Text = GrdDetail.Columns["COSTAMOUNT"].SummaryItem.SummaryValue.ToString();
                txtSelectedAmount.Text = GrdDetail.Columns["NEWAMOUNT"].SummaryItem.SummaryValue.ToString();
                txtDiffAmount.Text = Val.Format(Val.ToDouble(txtSelectedAmount.Text) - Val.ToDouble(txtTotalAmount.Text), "####0.00");
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
            txtSelectedPackets.Text = "";
            txtDiffPackets.Text = "";
            txtTotalPcs.Text = "";
            txtSelectedPcs.Text = "";
            txtDiffPcs.Text = "";
            txtTotalCarat.Text = "";
            txtSelectedCarat.Text = "";
            txtDiffCarat.Text = "";

            txtTotalDisc.Text = "";
            txtSelectedDisc.Text = "";
            txtDiffDisc.Text = "";
            txtTotalPricePerCarat.Text = "";
            txtSelectedPricePerCarat.Text = "";
            txtDiffPricePerCarat.Text = "";
            txtTotalAmount.Text = "";
            txtSelectedAmount.Text = "";
            txtDiffAmount.Text = "";
            MainGrdDetail.DataSource = null;
        }

      
        private void btnStoneRevShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbRapDate.Text.Trim() =="")
                {
                     Global.Message("Please Select Rap Date..");
                     return;
                }

                this.Cursor = Cursors.WaitCursor;
               
                LiveStockProperty LStockProperty = new LiveStockProperty();

                LStockProperty.MULTYSHAPE_ID = cmbShape.EditValue.ToString().Replace(" ","");
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
                DTabStoneReviseData = ObjTrn.GetStoneRevisionData(LStockProperty, Val.ToString(cmbRapDate.SelectedItem),true);

                int IntAll = 0;
                int IntIdle = 0;
                int IntUP = 0;
                int IntDown = 0;

                foreach (DataRow DRow in DTabStoneReviseData.Rows)
                {
                    IntAll++;
                    if (Val.Val(DRow["COSTRAPAPORT"]) == Val.Val(DRow["NEWRAPAPORT"]))
                    {
                        IntIdle++;
                    }
                    else if (Val.Val(DRow["NEWRAPAPORT"]) > Val.Val(DRow["COSTRAPAPORT"]))
                    {
                        IntUP++;
                    }
                    else if (Val.Val(DRow["NEWRAPAPORT"]) < Val.Val(DRow["COSTRAPAPORT"]))
                    {
                        IntDown++;
                    }
                }

                rbtnAll.Text = "ALL(" + IntAll.ToString() + ")";
                rbtnIdle.Text = "IDLE(" + IntIdle.ToString() + ")";
                rbtnUp.Text = "UP(" + IntUP.ToString() + ")";
                rbtnDown.Text = "DOWN(" + IntDown.ToString() + ")";
                
                FillStoneReviseGrid();

                if (ChkIsCalcAvgPrice.Checked == true)
                {
                    AvgBand.Visible = true;
                }
                else
                {
                    AvgBand.Visible = false;
                }


                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void rbtnAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                FillStoneReviseGrid();
            }
            catch (Exception ex)
            {
               Global.Message(ex.Message.ToString());
            }
        }

        private void rbtnPPC_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //if (rbtnPPC.Checked || rbtnDis.Checked)
                //{
                //    btnStoneRevShow.PerformClick();
                //}
            }
            catch (Exception ex)
            {
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
                    DouNewRapaport = 0;
                    DouNewRapaportAmt = 0;
                    DouNewDisc = 0;
                    DouNewPricePerCarat = 0;
                    DouNewAmount = 0;

                    DouSaleRapaport = 0;
                    DouSaleRapaportAmt = 0;
                    DouSaleDisc = 0;
                    DouSalePricePerCarat = 0;
                    DouSaleAmount = 0;

                    DouCompRapaport = 0;
                    DouCompRapaportAmt = 0;
                    DouCompDisc = 0;
                    DouCompPricePerCarat = 0;
                    DouCompAmount = 0;

                }
                else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DouCarat = DouCarat + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT"));
                    DouNewAmount = DouNewAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "NEWAMOUNT"));
                    DouNewRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "NEWRAPAPORT"));
                    DouNewPricePerCarat = DouNewAmount / DouCarat;
                    DouNewRapaportAmt = DouNewRapaportAmt + (DouNewRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));


                    DouSaleAmount = DouSaleAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "COSTAMOUNT"));
                    DouSaleRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "COSTRAPAPORT"));
                    DouSalePricePerCarat = DouSaleAmount / DouCarat;
                    DouSaleRapaportAmt = DouSaleRapaportAmt + (DouSaleRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));


                    DouCompAmount = DouCompAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "COMPAMOUNT"));
                    DouCompRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "COMPRAPAPORT"));
                    DouCompPricePerCarat = DouCompAmount / DouCarat;
                    DouCompRapaportAmt = DouCompRapaportAmt + (DouCompRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

                    DouDiffAmount = DouDiffAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "DIFFAMOUNT"));
                    DouDiffRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "DIFFRAPAPORT"));
                    DouDiffPricePerCarat = DouDiffAmount / DouCarat;
                    DouDiffRapaportAmt = DouDiffRapaportAmt + (DouDiffRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));
                }
                else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("NEWPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouNewAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("NEWRAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouNewRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("NEWDISCOUNT") == 0)
                    {
                        DouNewRapaport = Math.Round((DouNewRapaportAmt / DouCarat), 2);
                        //DouNewDisc = Math.Round(((DouNewPricePerCarat - DouNewRapaport) / DouNewRapaport * 100), 2);
                        DouNewDisc = Math.Round(((DouNewRapaport - DouNewPricePerCarat) / DouNewRapaport * 100), 2);
                        e.TotalValue = DouNewDisc;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("COSTPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouSaleAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("COSTRAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouSaleRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("COSTDISCOUNT") == 0)
                    {
                        DouSaleRapaport = Math.Round(DouSaleRapaportAmt / DouCarat);
                        //DouSaleDisc = Math.Round(((DouSalePricePerCarat - DouSaleRapaport) / DouSaleRapaport * 100), 2);
                       DouSaleDisc = Math.Round(((DouSaleRapaport - DouSalePricePerCarat) / DouSaleRapaport * 100), 2);
                        e.TotalValue = DouSaleDisc;
                    }




                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("COMPPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouCompAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("COMPRAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouCompRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("COMPDISCOUNT") == 0)
                    {
                        DouCompRapaport = Math.Round(DouCompRapaportAmt / DouCarat);
                        DouCompDisc = Math.Round(((DouCompPricePerCarat - DouCompRapaport) / DouCompRapaport * 100), 2);
                        e.TotalValue = DouSaleDisc;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("DIFFPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouDiffAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("DIFFRAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouDiffRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("DIFFDISCOUNT") == 0)
                    {
                        DouDiffRapaport = Math.Round(DouDiffRapaportAmt / DouCarat);
                        //DouDiffDisc = Math.Round(((DouDiffPricePerCarat - DouDiffRapaport) / DouDiffRapaport * 100), 2);
                        DouDiffDisc = Math.Round(((DouDiffRapaport - DouDiffPricePerCarat) / DouDiffRapaport * 100), 2);
                        e.TotalValue = DouDiffDisc;
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
                this.Cursor = Cursors.WaitCursor;
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

                ObjMstProper.REMARK = "STONE COST PRICING UPDATE";
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
				ObjMstProper = ObjTrn.SaveEntry(MemoEntryMasterRecordsForXML, MemoEntryDetailForXML,ChkIsCalcAvgPrice.Checked, ChkUpdateExpPrice.Checked, Val.ToString(cmbRapDate.SelectedItem));

                ReturnMessageDesc = ObjMstProper.ReturnMessageDesc;
                ReturnMessageType = ObjMstProper.ReturnMessageType;

                ObjMstProper = null;
                Global.Message(ReturnMessageDesc);
                if (ReturnMessageType == "SUCCESS")
                {
                    this.Clear();
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void btnStoneRevExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("Stone Cost Update List", GrdDetail);
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
            if (e.Clicks == 2 &&  e.Column.FieldName == "COMPDISCOUNT")
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

        private void ChkIsCalcAvgPrice_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkIsCalcAvgPrice.Checked == true)
            {
                AvgBand.Visible = true;
            }
            else
            {
                AvgBand.Visible = false;
            }
        }
    }
}
