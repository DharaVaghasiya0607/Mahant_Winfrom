using BusLib.Configuration;
using BusLib.Master;
using BusLib.Rapaport;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace MahantExport.Stock
{
    public partial class FrmParameterUpdate : DevControlLib.cDevXtraForm
    {
        String PasteData = "";
        IDataObject PasteclipData = Clipboard.GetDataObject();

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        BOFindRap ObjRap = new BOFindRap();
        DataTable DtabParamUpdate = new DataTable();

        DataTable DtabPara = new DataTable();
        DataTable DtabExcelData = new DataTable();

        DataTable DtabBackPriceUpdate = new DataTable();

        string StrStonePriceXml = "";
        string StrSearchType = "";


        Guid mTrn_ID;

        #region Property Settings

        public FrmParameterUpdate()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            string Str = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdDet.Name);

            if (Str != "")
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                MemoryStream stream = new MemoryStream(byteArray);
                GrdDet.RestoreLayoutFromStream(stream);
            }

            mTrn_ID = BusLib.Configuration.BOConfiguration.FindNewSequentialID();

            DtabPara = new BOMST_Parameter().GetParameterData();

            cmbShape.Properties.DataSource = DtabPara.Select("PARATYPE = 'SHAPE'").CopyToDataTable();
            cmbShape.Properties.ValueMember = "PARA_ID";
            cmbShape.Properties.DisplayMember = "SHORTNAME";

            cmbColor.Properties.DataSource = DtabPara.Select("PARATYPE = 'COLOR'").CopyToDataTable();
            cmbColor.Properties.ValueMember = "PARA_ID";
            cmbColor.Properties.DisplayMember = "SHORTNAME";

            cmbClarity.Properties.DataSource = DtabPara.Select("PARATYPE = 'CLARITY'").CopyToDataTable();
            cmbClarity.Properties.ValueMember = "PARA_ID";
            cmbClarity.Properties.DisplayMember = "SHORTNAME";

            if (BusLib.Configuration.BOConfiguration.gEmployeeProperty.ISDISPLAYCOSTPRICE == false)
            {
                GrdDet.Columns["COSTRAPAPORT"].Visible = false;
                GrdDet.Columns["COSTDISCOUNT"].Visible = false;
                GrdDet.Columns["COSTPRICEPERCARAT"].Visible = false;
                GrdDet.Columns["COSTAMOUNT"].Visible = false;

            }
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            txtPassForDisplayLockPrice.Tag = ObjPer.PASSWORD;
            txtPassForDisplayLockPrice_TextChanged(null, null);
            this.Show();

        }
        public void ShowForm(String pStrStoneNo)
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            GrdDet.BestFitColumns();

            string Str = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdDet.Name);

            if (Str != "")
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                MemoryStream stream = new MemoryStream(byteArray);
                GrdDet.RestoreLayoutFromStream(stream);
            }

            DtabPara = new BOMST_Parameter().GetParameterData();

            cmbShape.Properties.DataSource = DtabPara.Select("PARATYPE = 'SHAPE'").CopyToDataTable();
            cmbShape.Properties.ValueMember = "PARA_ID";
            cmbShape.Properties.DisplayMember = "SHORTNAME";

            cmbColor.Properties.DataSource = DtabPara.Select("PARATYPE = 'COLOR'").CopyToDataTable();
            cmbColor.Properties.ValueMember = "PARA_ID";
            cmbColor.Properties.DisplayMember = "SHORTNAME";

            cmbClarity.Properties.DataSource = DtabPara.Select("PARATYPE = 'CLARITY'").CopyToDataTable();
            cmbClarity.Properties.ValueMember = "PARA_ID";
            cmbClarity.Properties.DisplayMember = "SHORTNAME";

            RepCmbTableInc.DataSource = DtabPara.Select("PARATYPE = 'TABLEINC'").CopyToDataTable(); ;
            RepCmbTableInc.DisplayMember = "SHORTNAME";
            RepCmbTableInc.ValueMember = "PARA_ID";

            RepCmbSideTable.DataSource = DtabPara.Select("PARATYPE = 'SIDETABLEINC'").CopyToDataTable(); ;
            RepCmbSideTable.DisplayMember = "SHORTNAME";
            RepCmbSideTable.ValueMember = "PARA_ID";

            txtStoneNo.Text = pStrStoneNo;

            if (BusLib.Configuration.BOConfiguration.gEmployeeProperty.ISDISPLAYCOSTPRICE == false)
            {
                GrdDet.Columns["COSTRAPAPORT"].Visible = false;
                GrdDet.Columns["COSTDISCOUNT"].Visible = false;
                GrdDet.Columns["COSTPRICEPERCARAT"].Visible = false;
                GrdDet.Columns["COSTAMOUNT"].Visible = false;

            }

            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            txtPassForDisplayLockPrice.Tag = ObjPer.PASSWORD;
            txtPassForDisplayLockPrice_TextChanged(null, null);

            BtnShow_Click(null, null);

            this.Show();
        }


        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = false;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        #endregion

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void BtnBestFit_Click(object sender, EventArgs e)
        {
            GrdDet.BestFitColumns();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTab = DtabParamUpdate.GetChanges();
                if (DTab == null || DTab.Rows.Count <= 0)
                {
                    if (Global.Confirm("No Any Changes , Still You Want Save This All Record ?") == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                    DTab = DtabParamUpdate.Copy();
                }

                this.Cursor = Cursors.WaitCursor;

                DTab.TableName = "DETAIL";

                string ParameterUpdateXml;
                using (StringWriter sw = new StringWriter())
                {
                    DTab.WriteXml(sw);
                    ParameterUpdateXml = sw.ToString();
                }

                MemoEntryProperty Property = new MemoEntryProperty();
                Property = new BOTRN_StockUpload().SaveParameterOrPriceUpdateDetail(ParameterUpdateXml, false);
                this.Cursor = Cursors.Default;
                Global.Message(Property.ReturnMessageDesc);
                if (Property.ReturnMessageType == "SUCCESS")
                {
                    DtabParamUpdate.AcceptChanges();
                }
                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }
        private void BtnUpdateWithAvailable_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTab = DtabParamUpdate.GetChanges();
                if (DTab == null || DTab.Rows.Count <= 0)
                {
                    if (Global.Confirm("No Any Changes , Still You Want Save This All Record ?") == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                    DTab = DtabParamUpdate.Copy();
                }

                this.Cursor = Cursors.WaitCursor;

                //Gunjan:27/02/2024
                string StrStoneNo = "";
                for (int i = 0; i < DTab.Rows.Count; i++)
                {
                    if ((Val.ToString(DTab.Rows[i]["FANCYCOLOR"]) == ""))
                    {
                        if (Val.ToDouble(DTab.Rows[i]["SALEAMOUNT"]) == 0 || Val.ToDouble(DTab.Rows[i]["SALEDISCOUNT"]) == 0)
                        {
                            StrStoneNo = StrStoneNo + ',' + Val.ToString(DTab.Rows[i]["PARTYSTOCKNO"]);
                        }
                    }
                    else
                    {
                        if (Val.ToDouble(DTab.Rows[i]["SALEAMOUNT"]) == 0)
                        {
                            StrStoneNo = StrStoneNo + ',' + Val.ToString(DTab.Rows[i]["PARTYSTOCKNO"]);
                        }
                    }

                }

                if (StrStoneNo != "")
                {
                    Global.Message("Stock No -->>" + StrStoneNo + "-- " + "Sale Amount And Sale Discount Is 0 Plz Check Once");
                    return;
                }
                //End As Gunjan

                DTab.TableName = "DETAIL";

                string ParameterUpdateXml;
                using (StringWriter sw = new StringWriter())
                {
                    DTab.WriteXml(sw);
                    ParameterUpdateXml = sw.ToString();
                }

                MemoEntryProperty Property = new MemoEntryProperty();
                Property = new BOTRN_StockUpload().SaveParameterOrPriceUpdateDetail(ParameterUpdateXml, true);
                this.Cursor = Cursors.Default;
                Global.Message(Property.ReturnMessageDesc);
                if (Property.ReturnMessageType == "SUCCESS")
                {
                    DtabParamUpdate.AcceptChanges();
                }
                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }
        private void RdbAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (RdbAll.Checked)
                {
                    PnlFileWisePriceUpdate.Visible = false;
                    for (int i = 0; i < GrdDet.Columns.Count; i++)
                    {
                        if (
                            GrdDet.Columns[i].FieldName == "STOCK_ID" ||
                            GrdDet.Columns[i].FieldName == "STOCKTYPE" ||
                            GrdDet.Columns[i].FieldName == "STOCKNO"
                            )
                        {
                            GrdDet.Columns[i].Visible = false;
                        }
                        else
                        {
                            GrdDet.Columns[i].Visible = true;
                        }
                    }

                }
                else if (RdbParameter.Checked)
                {
                    PnlFileWisePriceUpdate.Visible = false;
                    for (int i = 0; i < GrdDet.Columns.Count; i++)
                    {
                        if (
                            GrdDet.Columns[i].FieldName == "MFGRAPAPORT" ||
                            GrdDet.Columns[i].FieldName == "MFGAMOUNT" ||
                            GrdDet.Columns[i].FieldName == "MFGDISCOUNT" ||
                            GrdDet.Columns[i].FieldName == "MFGPRICEPERCARAT" ||

                            //GrdDet.Columns[i].FieldName == "AVGRAPAPORT" ||
                            //GrdDet.Columns[i].FieldName == "AVGDISCOUNT" ||
                            GrdDet.Columns[i].FieldName == "AVGPRICEPERCARAT" ||
                            GrdDet.Columns[i].FieldName == "AVGAMOUNT" ||

                            GrdDet.Columns[i].FieldName == "COSTRAPAPORT" ||
                            GrdDet.Columns[i].FieldName == "COSTAMOUNT" ||
                            GrdDet.Columns[i].FieldName == "COSTDISCOUNT" ||
                            GrdDet.Columns[i].FieldName == "COSTPRICEPERCARAT" ||

                            GrdDet.Columns[i].FieldName == "SALERAPAPORT" ||
                            GrdDet.Columns[i].FieldName == "SALEAMOUNT" ||
                            GrdDet.Columns[i].FieldName == "SALEDISCOUNT" ||
                            GrdDet.Columns[i].FieldName == "SALEPRICEPERCARAT" ||

                            GrdDet.Columns[i].FieldName == "EXPRAPAPORT" ||
                            GrdDet.Columns[i].FieldName == "EXPAMOUNT" ||
                            GrdDet.Columns[i].FieldName == "EXPDISCOUNT" ||
                            GrdDet.Columns[i].FieldName == "EXPPRICEPERCARAT" ||

                            GrdDet.Columns[i].FieldName == "RAPNETRAPAPORT" ||
                            GrdDet.Columns[i].FieldName == "RAPNETAMOUNT" ||
                            GrdDet.Columns[i].FieldName == "RAPNETDISCOUNT" ||
                            GrdDet.Columns[i].FieldName == "RAPNETPRICEPERCARAT" ||

                            GrdDet.Columns[i].FieldName == "JAMESALLENRAPAPORT" ||
                            GrdDet.Columns[i].FieldName == "JAMESALLENAMOUNT" ||
                            GrdDet.Columns[i].FieldName == "JAMESALLENDISCOUNT" ||
                            GrdDet.Columns[i].FieldName == "JAMESALLENPRICEPERCARAT" ||

                            GrdDet.Columns[i].FieldName == "STOCK_ID" ||
                            GrdDet.Columns[i].FieldName == "STOCKTYPE" ||
                            GrdDet.Columns[i].FieldName == "STOCKNO"

                            )
                        {
                            GrdDet.Columns[i].Visible = false;
                        }
                        else
                        {
                            GrdDet.Columns[i].Visible = true;
                        }
                    }

                }
                else if (RdbPrice.Checked)
                {
                    PnlFileWisePriceUpdate.Visible = true;

                    for (int i = 0; i < GrdDet.Columns.Count; i++)
                    {
                        if (
                            GrdDet.Columns[i].FieldName == "MFGRAPAPORT" ||
                            GrdDet.Columns[i].FieldName == "MFGAMOUNT" ||
                            GrdDet.Columns[i].FieldName == "MFGDISCOUNT" ||
                            GrdDet.Columns[i].FieldName == "MFGPRICEPERCARAT" ||

                            GrdDet.Columns[i].FieldName == "COSTRAPAPORT" ||
                            GrdDet.Columns[i].FieldName == "COSTAMOUNT" ||
                            GrdDet.Columns[i].FieldName == "COSTDISCOUNT" ||
                            GrdDet.Columns[i].FieldName == "COSTPRICEPERCARAT" ||

                            //GrdDet.Columns[i].FieldName == "AVGRAPAPORT" ||
                            //GrdDet.Columns[i].FieldName == "AVGDISCOUNT" ||
                            GrdDet.Columns[i].FieldName == "AVGPRICEPERCARAT" ||
                            GrdDet.Columns[i].FieldName == "AVGAMOUNT" ||

                            GrdDet.Columns[i].FieldName == "SALERAPAPORT" ||
                            GrdDet.Columns[i].FieldName == "SALEAMOUNT" ||
                            GrdDet.Columns[i].FieldName == "SALEDISCOUNT" ||
                            GrdDet.Columns[i].FieldName == "SALEPRICEPERCARAT" ||

                            GrdDet.Columns[i].FieldName == "EXPRAPAPORT" ||
                            GrdDet.Columns[i].FieldName == "EXPAMOUNT" ||
                            GrdDet.Columns[i].FieldName == "EXPDISCOUNT" ||
                            GrdDet.Columns[i].FieldName == "EXPPRICEPERCARAT" ||

                            GrdDet.Columns[i].FieldName == "RAPNETRAPAPORT" ||
                            GrdDet.Columns[i].FieldName == "RAPNETAMOUNT" ||
                            GrdDet.Columns[i].FieldName == "RAPNETDISCOUNT" ||
                            GrdDet.Columns[i].FieldName == "RAPNETPRICEPERCARAT" ||

                            GrdDet.Columns[i].FieldName == "JAMESALLENRAPAPORT" ||
                            GrdDet.Columns[i].FieldName == "JAMESALLENAMOUNT" ||
                            GrdDet.Columns[i].FieldName == "JAMESALLENDISCOUNT" ||
                            GrdDet.Columns[i].FieldName == "JAMESALLENPRICEPERCARAT" ||

                            GrdDet.Columns[i].FieldName == "PARTYSTOCKNO" ||
                            GrdDet.Columns[i].FieldName == "CARAT"
                            )
                        {
                            GrdDet.Columns[i].Visible = true;
                        }
                        else
                        {
                            GrdDet.Columns[i].Visible = false;
                        }
                    }

                }
                else if (RdbCustmisePrice.Checked)
                {
                    PnlFileWisePriceUpdate.Visible = true;

                    for (int i = 0; i < GrdDet.Columns.Count; i++)
                    {
                        if (
                            GrdDet.Columns[i].FieldName == "LUSTERNAME" ||

                            GrdDet.Columns[i].FieldName == "TABLEBLACKNAME" ||
                            GrdDet.Columns[i].FieldName == "TABLEINCNAME" ||
                            GrdDet.Columns[i].FieldName == "MILKYNAME" ||
                            GrdDet.Columns[i].FieldName == "COLORSHADENAME" ||
                            GrdDet.Columns[i].FieldName == "TABLEOPENNAME" ||
                            GrdDet.Columns[i].FieldName == "EYECLEANNAME" ||
                            GrdDet.Columns[i].FieldName == "HEARTANDARROW" ||

                            GrdDet.Columns[i].FieldName == "SALERAPAPORT" ||
                            GrdDet.Columns[i].FieldName == "SALEAMOUNT" ||
                            GrdDet.Columns[i].FieldName == "SALEDISCOUNT" ||
                            GrdDet.Columns[i].FieldName == "SALEPRICEPERCARAT" ||

                            GrdDet.Columns[i].FieldName == "PARTYSTOCKNO" ||
                            GrdDet.Columns[i].FieldName == "CARAT" ||

                            GrdDet.Columns[i].FieldName == "SHAPENAME" ||
                            GrdDet.Columns[i].FieldName == "COLORNAME" ||
                            GrdDet.Columns[i].FieldName == "CLARITYNAME" ||
                            GrdDet.Columns[i].FieldName == "CUTNAME" ||
                            GrdDet.Columns[i].FieldName == "POLNAME" ||
                            GrdDet.Columns[i].FieldName == "SYMNAME" ||
                            GrdDet.Columns[i].FieldName == "FLNAME"
                            )
                        {
                            GrdDet.Columns[i].Visible = true;
                        }
                        else
                        {
                            GrdDet.Columns[i].Visible = false;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        public void Calculation()
        {
            try
            {
                GrdDet.PostEditor();
                GrdDet.RefreshData();
                double DCCarat = 0;
                double DCRapaport = 0;
                double DCRapaportAmt = 0;
                double DCDiscount = 0;

                double DSRapaport = 0;
                double DSRapaportAmt = 0;
                double DSDiscount = 0;

                double DERapaport = 0;
                double DERapaportAmt = 0;
                double DEDiscount = 0;

                double DRRapaport = 0;
                double DRRapaportAmt = 0;
                double DRDiscount = 0;

                DtabParamUpdate.AcceptChanges();

                for (int IntI = 0; IntI < GrdDet.RowCount; IntI++)
                {
                    DataRow DRow = GrdDet.GetDataRow(IntI);
                    DCRapaportAmt += Val.Val(DRow["COSTRAPAPORT"]) * Val.Val(DRow["CARAT"]);
                    DSRapaportAmt += Val.Val(DRow["SALERAPAPORT"]) * Val.Val(DRow["CARAT"]);
                    DERapaportAmt += Val.Val(DRow["EXPRAPAPORT"]) * Val.Val(DRow["CARAT"]);
                    DRRapaportAmt += Val.Val(DRow["RAPNETRAPAPORT"]) * Val.Val(DRow["CARAT"]);
                }

                DCCarat = Val.Val(DtabParamUpdate.Compute("SUM(CARAT)", string.Empty));

                txtCostAvgAmt.Text = string.Format("{0:0.00}", DtabParamUpdate.Compute("SUM(COSTAMOUNT)", string.Empty));
                txtCostAvgPerCts.Text = string.Format("{0:0.00}", Val.Val(txtCostAvgAmt.Text) / Val.Val(DCCarat));

                txtSaleAvgAmt.Text = string.Format("{0:0.00}", DtabParamUpdate.Compute("SUM(SALEAMOUNT)", string.Empty));
                txtSaleAvgPerCts.Text = string.Format("{0:0.00}", Val.Val(txtSaleAvgAmt.Text) / Val.Val(DCCarat));

                txtExpAvgAmt.Text = string.Format("{0:0.00}", DtabParamUpdate.Compute("SUM(EXPAMOUNT)", string.Empty));
                txtExpAvgPerCts.Text = string.Format("{0:0.00}", Val.Val(txtExpAvgAmt.Text) / Val.Val(DCCarat));

                txtRapnetAvgAmt.Text = string.Format("{0:0.00}", DtabParamUpdate.Compute("SUM(RAPNETAMOUNT)", string.Empty));
                txtRapnetAvgPerCts.Text = string.Format("{0:0.00}", Val.Val(txtRapnetAvgAmt.Text) / Val.Val(DCCarat));

                if (DCCarat > 0)
                {
                    DCRapaport = Math.Round(DCRapaportAmt / Val.Val(DCCarat), 4);
                    //DCDiscount = Val.Val(DCRapaport) == 0 ? 0 : Math.Round((Val.Val(txtCostAvgPerCts.Text) - DCRapaport) / DCRapaport * 100, 2);
                    DCDiscount = Val.Val(DCRapaport) == 0 ? 0 : Math.Round((DCRapaport - Val.Val(txtCostAvgPerCts.Text)) / DCRapaport * 100, 2); //#P
                    DSRapaport = Math.Round(DSRapaportAmt / Val.Val(DCCarat), 4);
                    //DSDiscount = Val.Val(DSRapaport) == 0 ? 0 : Math.Round((Val.Val(txtSaleAvgPerCts.Text) - DSRapaport) / DSRapaport * 100, 2);
                    DSDiscount = Val.Val(DSRapaport) == 0 ? 0 : Math.Round((DSRapaport - Val.Val(txtSaleAvgPerCts.Text)) / DSRapaport * 100, 2);

                    DERapaport = Math.Round(DERapaportAmt / Val.Val(DCCarat), 4);
                    //DSDiscount = Val.Val(DSRapaport) == 0 ? 0 : Math.Round((Val.Val(txtSaleAvgPerCts.Text) - DSRapaport) / DSRapaport * 100, 2);
                    DSDiscount = Val.Val(DSRapaport) == 0 ? 0 : Math.Round((DSRapaport - Val.Val(txtSaleAvgPerCts.Text)) / DSRapaport * 100, 2);
                    //DEDiscount = Val.Val(DERapaport) == 0 ? 0 : Math.Round((DERapaport - Val.Val(txtExpAvgPerCts.Text)) / DERapaport * 100, 2);

                    DRRapaport = Math.Round(DRRapaportAmt / Val.Val(DCCarat), 4);
                    //DRDiscount = Val.Val(DRRapaport) == 0 ? 0 : Math.Round((Val.Val(txtRapnetAvgAmt.Text) - DRRapaport) / DRRapaport * 100, 2);
                    DRDiscount = Val.Val(DRRapaport) == 0 ? 0 : Math.Round((DRRapaport - Val.Val(txtRapnetAvgAmt.Text)) / DRRapaport * 100, 2);
                    //DRDiscount = Val.Val(DRRapaport) == 0 ? 0 : Math.Round((DRRapaport - Val.Val(txtRapnetAvgAmt.Text)) / DRRapaport * 100, 2);
                }
                txtCostAvgDisc.Text = string.Format("{0:0.00}", DCDiscount, string.Empty);
                txtCostAvgRap.Text = string.Format("{0:0.00}", DCRapaport, string.Empty);

                txtSaleAvgDisc.Text = string.Format("{0:0.00}", DSDiscount, string.Empty);
                txtSaleAvgRap.Text = string.Format("{0:0.00}", DSRapaport, string.Empty);

                txtExpAvgDisc.Text = string.Format("{0:0.00}", DEDiscount, string.Empty);
                txtExpAvgRap.Text = string.Format("{0:0.00}", DERapaport, string.Empty);

                txtRapnetAvgDisc.Text = string.Format("{0:0.00}", DEDiscount, string.Empty);
                txtRapnetAvgRap.Text = string.Format("{0:0.00}", DERapaport, string.Empty);
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;
                string StrType = "";
                LiveStockProperty LStockProperty = new LiveStockProperty();
                LStockProperty.MULTYSHAPE_ID = Val.Trim(cmbShape.Properties.GetCheckedItems());
                LStockProperty.MULTYCOLOR_ID = Val.Trim(cmbColor.Properties.GetCheckedItems());
                LStockProperty.MULTYCLARITY_ID = Val.Trim(cmbClarity.Properties.GetCheckedItems());
                LStockProperty.FROMCARAT = Val.Val(txtFromCarat.Text);
                LStockProperty.TOCARAT = Val.Val(txtToCarat.Text);
                LStockProperty.STOCKNO = Val.ToString(txtStoneNo.Text);

                if (RbtNatural.Checked == true)
                {
                    StrType = Val.ToString(RbtNatural.Text);
                }
                else
                {
                    StrType = Val.ToString(RbtLabgrown.Text);
                }
                bool IsAvailable = chkIsDisplayAvailablestone.Checked;

                DtabParamUpdate = ObjStock.GetPriceParameterGetData(LStockProperty, Val.ToBoolean(ChkIsCalcAvgPrice.Checked), StrType, IsAvailable);

                if (DtabParamUpdate.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }

                MainGrd.DataSource = DtabParamUpdate;
                GrdDet.RefreshData();
                GrdDet.BestFitMaxRowCount = 500;
                GrdDet.BestFitColumns();
                Calculation();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }


        private void repTextPopup_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle < 0)
                    return;

                DataTable DTab = new DataTable();

                if (GrdDet.FocusedColumn.FieldName.ToUpper() == "SHAPENAME")
                    DTab = DtabPara.Select("PARATYPE = 'SHAPE'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "COLORNAME" || GrdDet.FocusedColumn.FieldName.ToUpper() == "MFGCOLORNAME")
                    DTab = DtabPara.Select("PARATYPE = 'COLOR'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "CLARITYNAME" || GrdDet.FocusedColumn.FieldName.ToUpper() == "MFGCLARITYNAME")
                    DTab = DtabPara.Select("PARATYPE = 'CLARITY'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "CUTNAME" || GrdDet.FocusedColumn.FieldName.ToUpper() == "MFGCUTNAME")
                    DTab = DtabPara.Select("PARATYPE = 'CUT'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "POLNAME" || GrdDet.FocusedColumn.FieldName.ToUpper() == "MFGPOLNAME")
                    DTab = DtabPara.Select("PARATYPE = 'POLISH'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "SYMNAME" || GrdDet.FocusedColumn.FieldName.ToUpper() == "MFGSYMNAME")
                    DTab = DtabPara.Select("PARATYPE = 'SYMMETRY'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "FLNAME" || GrdDet.FocusedColumn.FieldName.ToUpper() == "MFGFLNAME")
                    DTab = DtabPara.Select("PARATYPE = 'FLUORESCENCE'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "LOCATIONNAME")
                    DTab = DtabPara.Select("PARATYPE = 'LOCATION'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "SIZENAME")
                    DTab = DtabPara.Select("PARATYPE = 'SIZE'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "LABNAME")
                    DTab = DtabPara.Select("PARATYPE = 'LAB'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "COLORSHADENAME")
                    DTab = DtabPara.Select("PARATYPE = 'COLORSHADE'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "MILKYNAME")
                    DTab = DtabPara.Select("PARATYPE = 'MILKY'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "EYECLEANNAME")
                    DTab = DtabPara.Select("PARATYPE = 'EYECLEAN'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "LUSTERNAME")
                    DTab = DtabPara.Select("PARATYPE = 'LUSTER'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "HANAME")
                    DTab = DtabPara.Select("PARATYPE = 'HEARTANDARROW'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "CULETNAME")
                    DTab = DtabPara.Select("PARATYPE = 'CULET'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "GIRDLENAME")
                    DTab = DtabPara.Select("PARATYPE = 'GIRDLE'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "TABLEINCNAME")
                    DTab = DtabPara.Select("PARATYPE = 'TABLEINC'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "TABLEOPENNAME")
                    DTab = DtabPara.Select("PARATYPE = 'TABLEOPENINC'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "SIDETABLENAME")
                    DTab = DtabPara.Select("PARATYPE = 'SIDETABLEINC'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "SIDEOPENNAME")
                    DTab = DtabPara.Select("PARATYPE = 'SIDEOPENINC'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "TABLEBLACKNAME")
                    DTab = DtabPara.Select("PARATYPE = 'TABLEBLACKINC'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "SIDEBLACKNAME")
                    DTab = DtabPara.Select("PARATYPE = 'SIDEBLACKINC'").CopyToDataTable();

                else if (GrdDet.FocusedColumn.FieldName.ToUpper() == "REDSPORTNAME")
                    DTab = DtabPara.Select("PARATYPE = 'REDSPORTINC'").CopyToDataTable();

                else if ((GrdDet.FocusedColumn.FieldName.ToUpper() == "COLORCOMMENT") || (GrdDet.FocusedColumn.FieldName.ToUpper() == "CLARITYCOMMENT"))
                    DTab = new BOMST_Parameter().GetColorClarityComment();


                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARACODE,PARANAME,SHORTNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = DTab;
                    FrmSearch.mStrColumnsToHide = "PARA_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDet.SetFocusedRowCellValue(GrdDet.FocusedColumn.FieldName, (Val.ToString(FrmSearch.DRow["SHORTNAME"])));
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

        private void txtStoneNo_MouseDown(object sender, MouseEventArgs e)
        {
            if (txtStoneNo.Focus())
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    PasteData = Convert.ToString(PasteclipData.GetData(System.Windows.Forms.DataFormats.Text));
                }
            }
        }

        private void txtStoneNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String str1 = "";
                if (txtStoneNo.Text.Trim().Contains("\t\n"))
                {
                    str1 = txtStoneNo.Text.Trim().Replace("\t\n", ",");
                }
                else
                {
                    str1 = txtStoneNo.Text.Trim().Replace("\n", ",");
                    str1 = str1.Replace("\r", "");
                }

                txtStoneNo.Text = str1;
                //rTxtStoneCertiMfgMemo.Text = str1.Trim().TrimStart().TrimEnd();
                txtStoneNo.Select(txtStoneNo.Text.Length, 0);
                //rTxtStoneCertiMfgMemo.Text = rTxtStoneCertiMfgMemo.Text.Trim().TrimStart().TrimEnd();

                //lblTotalCount.Text = "(" + rTxtStoneCertiMfgMemo.Text.Split(',').Length + ")";
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtStoneNo_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                String str1 = "";
                if (txtStoneNo.Text.Trim().Contains("\t\n"))
                {
                    str1 = txtStoneNo.Text.Trim().Replace("\t\n", ",");
                }
                else
                {
                    str1 = txtStoneNo.Text.Trim().Replace("\n", ",");
                    str1 = str1.Replace("\r", "");
                }

                txtStoneNo.Text = str1;
                //rTxtStoneCertiMfgMemo.Text = str1.Trim().TrimStart().TrimEnd();
                txtStoneNo.Select(txtStoneNo.Text.Length, 0);
                //rTxtStoneCertiMfgMemo.Text = rTxtStoneCertiMfgMemo.Text.Trim().TrimStart().TrimEnd();

                //lblTotalCount.Text = "(" + rTxtStoneCertiMfgMemo.Text.Split(',').Length + ")";
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDet_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            try
            {

                if (e.RowHandle < 0)
                {
                    return;
                }
                DataRow DRow = GrdDet.GetDataRow(e.RowHandle);

                switch (e.Column.FieldName)
                {
                    case "MFGCOLORNAME":
                    case "MFGCLARITYNAME":
                    case "MFGCUTNAME":
                    case "MFGPOLNAME":
                    case "MFGSYMNAME":
                    case "MFGFLNAME":


                        Trn_RapSaveProperty Property = new Trn_RapSaveProperty();

                        Property.SHAPE_ID = Val.ToString(DRow["SHAPENAME"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='SHAPE' And ShortName='" + Val.ToString(DRow["SHAPENAME"]) + "'")[0]["PARA_ID"]) : 0;
                        Property.COLOR_ID = Val.ToString(DRow["MFGCOLORNAME"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='COLOR' And ShortName='" + Val.ToString(DRow["MFGCOLORNAME"]) + "'")[0]["PARA_ID"]) : 0;
                        Property.CLARITY_ID = Val.ToString(DRow["MFGCLARITYNAME"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='CLARITY' And ShortName='" + Val.ToString(DRow["MFGCLARITYNAME"]) + "'")[0]["PARA_ID"]) : 0;

                        Property.CUT_ID = Val.ToString(DRow["MFGCUTNAME"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='CUT' And ShortName='" + Val.ToString(DRow["MFGCUTNAME"]) + "'")[0]["PARA_ID"]) : 0;
                        Property.POL_ID = Val.ToString(DRow["MFGPOLNAME"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='POLISH' And ShortName='" + Val.ToString(DRow["MFGPOLNAME"]) + "'")[0]["PARA_ID"]) : 0;
                        Property.SYM_ID = Val.ToString(DRow["MFGSYMNAME"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='SYMMETRY' And ShortName='" + Val.ToString(DRow["MFGSYMNAME"]) + "'")[0]["PARA_ID"]) : 0;
                        Property.FL_ID = Val.ToString(DRow["MFGFLNAME"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='FLUORESCENCE' And ShortName='" + Val.ToString(DRow["MFGFLNAME"]) + "'")[0]["PARA_ID"]) : 0;

                        Property.CARAT = Val.Val(DRow["CARAT"]);
                        // Property.DISCOUNT = Val.Val(DRow["MFGDISCOUNT"]);
                        Property = ObjRap.FindRap(Property);

                        DtabParamUpdate.Rows[e.RowHandle]["MFGRAPAPORT"] = Property.RAPAPORT;
                        DtabParamUpdate.Rows[e.RowHandle]["MFGPRICEPERCARAT"] = Math.Round(Property.RAPAPORT + ((Property.RAPAPORT * Val.Val(DRow["MFGDISCOUNT"])) / 100));
                        DtabParamUpdate.Rows[e.RowHandle]["MFGAMOUNT"] = Math.Round(Property.CARAT * Val.Val(DtabParamUpdate.Rows[e.RowHandle]["MFGPRICEPERCARAT"]), 2);

                        DtabParamUpdate.AcceptChanges();
                        Calculation();

                        break;

                    case "SHAPENAME":
                    case "COLORNAME":
                    case "CLARITYNAME":
                    case "CARAT":
                    case "CUTNAME":
                    case "POLNAME":
                    case "SYMNAME":
                    case "FLNAME":

                        Property = new Trn_RapSaveProperty();

                        Property.SHAPE_ID = Val.ToString(DRow["SHAPENAME"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='SHAPE' And ShortName='" + Val.ToString(DRow["SHAPENAME"]) + "'")[0]["PARA_ID"]) : 0;
                        Property.COLOR_ID = Val.ToString(DRow["COLORNAME"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='COLOR' And ShortName='" + Val.ToString(DRow["COLORNAME"]) + "'")[0]["PARA_ID"]) : 0;
                        Property.CLARITY_ID = Val.ToString(DRow["CLARITYNAME"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='CLARITY' And ShortName='" + Val.ToString(DRow["CLARITYNAME"]) + "'")[0]["PARA_ID"]) : 0;

                        Property.CUT_ID = Val.ToString(DRow["CUTNAME"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='CUT' And ShortName='" + Val.ToString(DRow["CUTNAME"]) + "'")[0]["PARA_ID"]) : 0;
                        Property.POL_ID = Val.ToString(DRow["POLNAME"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='POLISH' And ShortName='" + Val.ToString(DRow["POLNAME"]) + "'")[0]["PARA_ID"]) : 0;
                        Property.SYM_ID = Val.ToString(DRow["SYMNAME"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='SYMMETRY' And ShortName='" + Val.ToString(DRow["SYMNAME"]) + "'")[0]["PARA_ID"]) : 0;
                        Property.FL_ID = Val.ToString(DRow["FLNAME"]) != "" ? Val.ToInt(DtabPara.Select("ParaType='FLUORESCENCE' And ShortName='" + Val.ToString(DRow["FLNAME"]) + "'")[0]["PARA_ID"]) : 0;
                        Property.CARAT = Val.Val(DRow["CARAT"]);
                        // Property.DISCOUNT = Val.Val(DRow["SALEDISCOUNT"]);
                        Property = ObjRap.FindRap(Property);

                        DtabParamUpdate.Rows[e.RowHandle]["SALERAPAPORT"] = Property.RAPAPORT;
                        DtabParamUpdate.Rows[e.RowHandle]["SALEPRICEPERCARAT"] = Math.Round(Property.RAPAPORT + ((Property.RAPAPORT * Val.Val(DRow["SALEDISCOUNT"])) / 100));
                        //DtabParamUpdate.Rows[e.RowHandle]["SALEPRICEPERCARAT"] = Math.Round(Property.RAPAPORT - ((Property.RAPAPORT * Val.Val(DRow["SALEDISCOUNT"])) / 100)); //#P:23-04-2021
                        DtabParamUpdate.Rows[e.RowHandle]["SALEAMOUNT"] = Math.Round(Property.CARAT * Val.Val(DtabParamUpdate.Rows[e.RowHandle]["SALEPRICEPERCARAT"]), 2);

                        DtabParamUpdate.Rows[e.RowHandle]["RAPNETRAPAPORT"] = Property.RAPAPORT;
                        DtabParamUpdate.Rows[e.RowHandle]["RAPNETPRICEPERCARAT"] = Math.Round(Property.RAPAPORT + ((Property.RAPAPORT * Val.Val(DRow["RAPNETDISCOUNT"])) / 100));
                        //DtabParamUpdate.Rows[e.RowHandle]["RAPNETPRICEPERCARAT"] = Math.Round(Property.RAPAPORT - ((Property.RAPAPORT * Val.Val(DRow["RAPNETDISCOUNT"])) / 100)); //#P:23-04-2021 
                        DtabParamUpdate.Rows[e.RowHandle]["RAPNETAMOUNT"] = Math.Round(Property.CARAT * Val.Val(DtabParamUpdate.Rows[e.RowHandle]["RAPNETPRICEPERCARAT"]), 2);

                        DtabParamUpdate.Rows[e.RowHandle]["EXPRAPAPORT"] = Property.RAPAPORT;
                        DtabParamUpdate.Rows[e.RowHandle]["EXPPRICEPERCARAT"] = Math.Round(Property.RAPAPORT + ((Property.RAPAPORT * Val.Val(DRow["EXPDISCOUNT"])) / 100));
                        //DtabParamUpdate.Rows[e.RowHandle]["EXPPRICEPERCARAT"] = Math.Round(Property.RAPAPORT - ((Property.RAPAPORT * Val.Val(DRow["EXPDISCOUNT"])) / 100)); //#P:23-04-2021
                        DtabParamUpdate.Rows[e.RowHandle]["EXPAMOUNT"] = Math.Round(Property.CARAT * Val.Val(DtabParamUpdate.Rows[e.RowHandle]["EXPPRICEPERCARAT"]), 2);

                        DtabParamUpdate.Rows[e.RowHandle]["JAMESALLENRAPAPORT"] = Property.RAPAPORT;
                        DtabParamUpdate.Rows[e.RowHandle]["JAMESALLENPRICEPERCARAT"] = Math.Round(Property.RAPAPORT + ((Property.RAPAPORT * Val.Val(DRow["JAMESALLENDISCOUNT"])) / 100));
                        //DtabParamUpdate.Rows[e.RowHandle]["JAMESALLENPRICEPERCARAT"] = Math.Round(Property.RAPAPORT - ((Property.RAPAPORT * Val.Val(DRow["JAMESALLENDISCOUNT"])) / 100)); //#P:23-04-2021
                        DtabParamUpdate.Rows[e.RowHandle]["JAMESALLENAMOUNT"] = Math.Round(Property.CARAT * Val.Val(DtabParamUpdate.Rows[e.RowHandle]["JAMESALLENPRICEPERCARAT"]), 2);

                        DtabParamUpdate.AcceptChanges();
                        Calculation();

                        break;

                    case "SALEDISCOUNT":

                        double Rapaport = Val.Val(DRow["SALERAPAPORT"]);
                        double Carat = Val.Val(DRow["CARAT"]);

                        DtabParamUpdate.Rows[e.RowHandle]["SALERAPAPORT"] = Rapaport;
                        DtabParamUpdate.Rows[e.RowHandle]["SALEPRICEPERCARAT"] = Math.Round(Rapaport + ((Rapaport * Val.Val(DRow["SALEDISCOUNT"])) / 100));
                        //DtabParamUpdate.Rows[e.RowHandle]["SALEPRICEPERCARAT"] = Math.Round(Rapaport - ((Rapaport * Val.Val(DRow["SALEDISCOUNT"])) / 100)); //#P:23-04-2021
                        DtabParamUpdate.Rows[e.RowHandle]["SALEAMOUNT"] = Math.Round(Carat * Val.Val(DtabParamUpdate.Rows[e.RowHandle]["SALEPRICEPERCARAT"]), 2);
                        Calculation();

                        break;
                    case "EXPDISCOUNT":
                        Rapaport = Val.Val(DRow["EXPRAPAPORT"]);
                        Carat = Val.Val(DRow["CARAT"]);

                        DtabParamUpdate.Rows[e.RowHandle]["EXPRAPAPORT"] = Rapaport;
                        DtabParamUpdate.Rows[e.RowHandle]["EXPPRICEPERCARAT"] = Math.Round(Rapaport + ((Rapaport * Val.Val(DRow["EXPDISCOUNT"])) / 100));
                        //DtabParamUpdate.Rows[e.RowHandle]["EXPPRICEPERCARAT"] = Math.Round(Rapaport - ((Rapaport * Val.Val(DRow["EXPDISCOUNT"])) / 100)); //#P:23-04-2021
                        DtabParamUpdate.Rows[e.RowHandle]["EXPAMOUNT"] = Math.Round(Carat * Val.Val(DtabParamUpdate.Rows[e.RowHandle]["EXPPRICEPERCARAT"]), 2);
                        Calculation();

                        break;

                    case "RAPNETDISCOUNT":
                        Rapaport = Val.Val(DRow["RAPNETRAPAPORT"]);
                        Carat = Val.Val(DRow["CARAT"]);

                        DtabParamUpdate.Rows[e.RowHandle]["RAPNETRAPAPORT"] = Rapaport;
                        DtabParamUpdate.Rows[e.RowHandle]["RAPNETPRICEPERCARAT"] = Math.Round(Rapaport + ((Rapaport * Val.Val(DRow["RAPNETDISCOUNT"])) / 100));
                        //DtabParamUpdate.Rows[e.RowHandle]["RAPNETPRICEPERCARAT"] = Math.Round(Rapaport - ((Rapaport * Val.Val(DRow["RAPNETDISCOUNT"])) / 100)); //#P:23-04-2021
                        DtabParamUpdate.Rows[e.RowHandle]["RAPNETAMOUNT"] = Math.Round(Carat * Val.Val(DtabParamUpdate.Rows[e.RowHandle]["RAPNETPRICEPERCARAT"]), 2);
                        Calculation();

                        break;

                    case "JAMESALLENDISCOUNT":
                        Rapaport = Val.Val(DRow["JAMESALLENRAPAPORT"]);
                        Carat = Val.Val(DRow["CARAT"]);

                        DtabParamUpdate.Rows[e.RowHandle]["JAMESALLENRAPAPORT"] = Rapaport;
                        DtabParamUpdate.Rows[e.RowHandle]["JAMESALLENPRICEPERCARAT"] = Math.Round(Rapaport + ((Rapaport * Val.Val(DRow["JAMESALLENDISCOUNT"])) / 100));
                        //DtabParamUpdate.Rows[e.RowHandle]["JAMESALLENPRICEPERCARAT"] = Math.Round(Rapaport - ((Rapaport * Val.Val(DRow["JAMESALLENDISCOUNT"])) / 100)); //#P:23-04-2021
                        DtabParamUpdate.Rows[e.RowHandle]["JAMESALLENAMOUNT"] = Math.Round(Carat * Val.Val(DtabParamUpdate.Rows[e.RowHandle]["JAMESALLENPRICEPERCARAT"]), 2);
                        Calculation();

                        break;

                    case "SALEPRICEPERCARAT":

                        Rapaport = Val.Val(DRow["SALERAPAPORT"]);
                        double PricePerCarat = Val.Val(DRow["SALEPRICEPERCARAT"]);
                        Carat = Val.Val(DRow["CARAT"]);
                        //double DouPer = Math.Round(((PricePerCarat - Rapaport) / Rapaport) * 100, 2);
                        double DouPer = 0;
                        if (Rapaport != 0)
                        {
                            DouPer = Math.Round(((Rapaport - PricePerCarat) / Rapaport) * 100, 2); //#P:23-04-2021
                        }
                        else
                            DouPer = 0;

                        DtabParamUpdate.Rows[e.RowHandle]["SALERAPAPORT"] = Rapaport;
                        DtabParamUpdate.Rows[e.RowHandle]["SALEDISCOUNT"] = DouPer;
                        DtabParamUpdate.Rows[e.RowHandle]["SALEAMOUNT"] = Math.Round(Carat * Val.Val(DtabParamUpdate.Rows[e.RowHandle]["SALEPRICEPERCARAT"]), 2);
                        Calculation();
                        break;

                    case "EXPPRICEPERCARAT":
                        Rapaport = Val.Val(DRow["EXPRAPAPORT"]);
                        PricePerCarat = Val.Val(DRow["EXPPRICEPERCARAT"]);
                        Carat = Val.Val(DRow["CARAT"]);
                        //DouPer = Math.Round(((PricePerCarat - Rapaport) / Rapaport) * 100, 2);
                        if (Rapaport != 0)
                        {
                            DouPer = Math.Round(((Rapaport - PricePerCarat) / Rapaport) * 100, 2); //#P:23-04-2021
                        }
                        else
                            DouPer = 0;

                        DtabParamUpdate.Rows[e.RowHandle]["EXPRAPAPORT"] = Rapaport;
                        DtabParamUpdate.Rows[e.RowHandle]["EXPDISCOUNT"] = DouPer;
                        DtabParamUpdate.Rows[e.RowHandle]["EXPAMOUNT"] = Math.Round(Carat * Val.Val(DtabParamUpdate.Rows[e.RowHandle]["EXPPRICEPERCARAT"]), 2);
                        Calculation();
                        break;

                    case "RAPNETPRICEPERCARAT":
                        Rapaport = Val.Val(DRow["RAPNETRAPAPORT"]);
                        PricePerCarat = Val.Val(DRow["RAPNETPRICEPERCARAT"]);
                        Carat = Val.Val(DRow["CARAT"]);
                        //DouPer = Math.Round(((PricePerCarat - Rapaport) / Rapaport) * 100, 2);
                        if (Rapaport != 0)
                        {
                            DouPer = Math.Round(((Rapaport - PricePerCarat) / Rapaport) * 100, 2); //#P:23-04-2021
                        }
                        else
                            DouPer = 0;

                        DtabParamUpdate.Rows[e.RowHandle]["RAPNETRAPAPORT"] = Rapaport;
                        DtabParamUpdate.Rows[e.RowHandle]["RAPNETDISCOUNT"] = DouPer;
                        DtabParamUpdate.Rows[e.RowHandle]["RAPNETAMOUNT"] = Math.Round(Carat * Val.Val(DtabParamUpdate.Rows[e.RowHandle]["RAPNETPRICEPERCARAT"]), 2);
                        Calculation();
                        break;

                    case "JAMESALLENPRICEPERCARAT":
                        Rapaport = Val.Val(DRow["JAMESALLENRAPAPORT"]);
                        PricePerCarat = Val.Val(DRow["JAMESALLENPRICEPERCARAT"]);
                        Carat = Val.Val(DRow["CARAT"]);
                        //DouPer = Math.Round(((PricePerCarat - Rapaport) / Rapaport) * 100, 2);
                        if (Rapaport != 0)
                        {
                            DouPer = Math.Round(((Rapaport - PricePerCarat) / Rapaport) * 100, 2); //#P:23-04-2021
                        }
                        else
                            DouPer = 0;

                        DtabParamUpdate.Rows[e.RowHandle]["JAMESALLENRAPAPORT"] = Rapaport;
                        DtabParamUpdate.Rows[e.RowHandle]["JAMESALLENDISCOUNT"] = DouPer;
                        DtabParamUpdate.Rows[e.RowHandle]["JAMESALLENAMOUNT"] = Math.Round(Carat * Val.Val(DtabParamUpdate.Rows[e.RowHandle]["JAMESALLENPRICEPERCARAT"]), 2);
                        Calculation();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }


        }

        private void txtSaleDiscAddLess_Validated(object sender, EventArgs e)
        {
            try
            {
                if (RdbParameter.Checked)
                {
                    Global.Message("Please Select 'All' Or 'Price' Checkbox For Update Stone Price.");
                    return;
                }

                double DouSaleDiscount = 0;
                double DouSaleRapaport = 0;
                double DouSalePricePerCarat = 0;
                double DouSaleAmount = 0;

                if (Val.ToString(CmbPriceUpdateType.Text) == "")
                {
                    foreach (DataRow DRow in DtabParamUpdate.Rows)
                    {
                        DouSaleDiscount = Val.Val(DRow["SALEDISCOUNT"]) + Val.Val(txtSaleDiscAddLess.Text);

                        DouSaleRapaport = Val.Val(DRow["SALERAPAPORT"]);
                        DouSalePricePerCarat = Math.Round(DouSaleRapaport + ((DouSaleRapaport * DouSaleDiscount) / 100), 2); //#P
                        DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);

                        DRow["SALEDISCOUNT"] = DouSaleDiscount;
                        DRow["SALEPRICEPERCARAT"] = DouSalePricePerCarat;
                        DRow["SALEAMOUNT"] = DouSaleAmount;
                    }
                }
                else if (Val.ToString(CmbPriceUpdateType.Text) == "Update Price Only Available")
                {
                    foreach (DataRow DRow in DtabParamUpdate.Rows)
                    {
                        if (Val.ToString(DRow["STATUS"]) == "AVAILABLE")
                        {
                            DouSaleDiscount = Val.Val(DRow["SALEDISCOUNT"]) + Val.Val(txtSaleDiscAddLess.Text);

                            DouSaleRapaport = Val.Val(DRow["SALERAPAPORT"]);
                            DouSalePricePerCarat = Math.Round(DouSaleRapaport + ((DouSaleRapaport * DouSaleDiscount) / 100), 2); //#P
                            DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);

                            DRow["SALEDISCOUNT"] = DouSaleDiscount;
                            DRow["SALEPRICEPERCARAT"] = DouSalePricePerCarat;
                            DRow["SALEAMOUNT"] = DouSaleAmount;
                        }
                    }
                }
                else if (Val.ToString(CmbPriceUpdateType.Text) == "Update Price Not in Fancy")
                {
                    foreach (DataRow DRow in DtabParamUpdate.Rows)
                    {
                        if (Val.ToBoolean(DRow["ISFANCY"]) == false)
                        {
                            DouSaleDiscount = Val.Val(DRow["SALEDISCOUNT"]) + Val.Val(txtSaleDiscAddLess.Text);

                            DouSaleRapaport = Val.Val(DRow["SALERAPAPORT"]);
                            DouSalePricePerCarat = Math.Round(DouSaleRapaport + ((DouSaleRapaport * DouSaleDiscount) / 100), 2); //#P
                            DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);

                            DRow["SALEDISCOUNT"] = DouSaleDiscount;
                            DRow["SALEPRICEPERCARAT"] = DouSalePricePerCarat;
                            DRow["SALEAMOUNT"] = DouSaleAmount;
                        }
                    }
                }
                DtabParamUpdate.AcceptChanges();
                Calculation();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }

        }

        private void txtExpDiscAddLess_Validated(object sender, EventArgs e)
        {


        }

        private void txtPassForDisplayLockPrice_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Val.ToString(txtPassForDisplayLockPrice.Tag) != "" && Val.ToString(txtPassForDisplayLockPrice.Tag).ToUpper() == txtPassForDisplayLockPrice.Text.ToUpper())
                {
                    GrdDet.Columns["ISLOCKSALEPRICE"].Visible = true;
                    GrdDet.Columns["ISLOCKSALEPRICE"].Fixed = FixedStyle.Left;
                    ChkAllPriceLock.Visible = true;
                }
                else
                {
                    GrdDet.Columns["ISLOCKSALEPRICE"].Visible = false;
                    ChkAllPriceLock.Visible = false;

                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnPriceFileUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (Val.ToString(txtBackPriceFileName.Text).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Selet File That You Want To Update...");
                    txtBackPriceFileName.Focus();
                    return;
                }

                //this.Cursor = Cursors.WaitCursor;
                for (int k = 0; k < DtabBackPriceUpdate.Rows.Count; k++)
                {
                    if ((Val.Val(DtabBackPriceUpdate.Rows[k]["DISC"]) != 0) && (Val.Val(DtabBackPriceUpdate.Rows[k]["PerCarat"]) != 0))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("Back And PerCarat Both Are Exists In StoneNo : '" + Val.ToString(DtabBackPriceUpdate.Rows[k]["StockNo"]) + "' Pls Check..");
                        return;
                    }
                }

                DtabBackPriceUpdate.TableName = "Table1";
                using (StringWriter sw = new StringWriter())
                {
                    DtabBackPriceUpdate.WriteXml(sw);
                    StrStonePriceXml = sw.ToString();
                }

                if (RbtStoneNo.Checked)
                {
                    StrSearchType = "STONENO";
                }
                else if (RbtCertiNo.Checked)
                {
                    StrSearchType = "LABREPORTNO";
                }

                if (backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.CancelAsync();
                }
                progressPanel1.Visible = true;
                backgroundWorker1.RunWorkerAsync();

                /*
                DtabParamUpdate = ObjStock.GetStonePriceExcelWiseGetData(StrStonePriceXml, StrSearchType);

                if (DtabParamUpdate.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Excel File Stone's Not Found Please check..");
                    return;
                }

                MainGrd.DataSource = DtabParamUpdate;
                GrdDet.RefreshData();
                this.Cursor = Cursors.Default;
                */

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog OpenFileDialog = new OpenFileDialog();
                OpenFileDialog.Filter = "Excel Files (*.xls,*.xlsx)|*.xls;*.xlsx;";
                if (OpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtBackPriceFileName.Text = OpenFileDialog.FileName;

                    string extension = Path.GetExtension(txtBackPriceFileName.Text.ToString());
                    string destinationPath = Application.StartupPath + @"\StoneFiles\" + Path.GetFileName(txtBackPriceFileName.Text);
                    destinationPath = destinationPath.Replace(extension, ".xlsx");
                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }
                    File.Copy(txtBackPriceFileName.Text, destinationPath);
                    DtabBackPriceUpdate = Global.GetDataTableFromExcel(destinationPath, true);

                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }
                }
                OpenFileDialog.Dispose();
                OpenFileDialog = null;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void lblSampleExcelFile_Click(object sender, EventArgs e)
        {
            try
            {
                string StrFilePathDestination = "";

                StrFilePathDestination = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\JangedWiseBackPriceUpdateFormat" + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.Day.ToString() + ".xlsx";
                if (File.Exists(StrFilePathDestination))
                {
                    File.Delete(StrFilePathDestination);
                }
                File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\Format\\JangedWiseBackPriceUpdateFormat.xlsx", StrFilePathDestination);

                System.Diagnostics.Process.Start(StrFilePathDestination, "CMD");
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void ChkAllPriceLock_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (DataRow DR in DtabParamUpdate.Rows)
                {
                    DR["ISLOCKSALEPRICE"] = ChkAllPriceLock.Checked;
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDet_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            Global.CustomDrawColumnHeader(sender, e);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DtabParamUpdate = ObjStock.GetStonePriceExcelWiseGetData(StrStonePriceXml, StrSearchType);
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
                progressPanel1.Visible = false;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (DtabParamUpdate.Rows.Count <= 0)
                {
                    Global.Message("Excel File Stone's Not Found Please check..");
                    progressPanel1.Visible = false;
                    return;
                }

                MainGrd.DataSource = DtabParamUpdate;
                GrdDet.RefreshData();
                Calculation();
                progressPanel1.Visible = false;
            }
            catch (Exception ex)
            {
                progressPanel1.Visible = false;
                Global.Message(ex.Message.ToString());
            }
        }

        private void repTxtFancyColor_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "FANCYCOLORNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_FANCYCOLOR);

                    FrmSearch.mStrColumnsToHide = "FANCYCOLOR_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDet.SetFocusedRowCellValue("FANCYCOLOR", (Val.ToString(FrmSearch.DRow["FANCYCOLORNAME"])));
                        GrdDet.SetFocusedRowCellValue("FANCYCOLOROVERTONE", (Val.ToString(FrmSearch.DRow["FANCYOVERTONE"])));
                        GrdDet.SetFocusedRowCellValue("FANCYCOLORINTENSITY", (Val.ToString(FrmSearch.DRow["FANCYINTENSITY"])));
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

        private void lblSaveLayout_Click(object sender, EventArgs e)
        {
            Stream str = new System.IO.MemoryStream();
            GrdDet.SaveLayoutToStream(str);
            str.Seek(0, System.IO.SeekOrigin.Begin);
            StreamReader reader = new StreamReader(str);
            string text = reader.ReadToEnd();

            int IntRes = new BOTRN_StockUpload().SaveGridLayout(this.Name, GrdDet.Name, text);
            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Saved");
            }
        }

        private void lblDefaultLayout_Click(object sender, EventArgs e)
        {
            int IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdDet.Name);
            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Deleted");
            }
        }

        private void RdbCustmisePrice_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void txtSaleDiscAddLess_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    try
            //    {
            //        if (RdbParameter.Checked)
            //        {
            //            Global.Message("Please Select 'All' Or 'Price' Checkbox For Update Stone Price.");
            //            return;
            //        }

            //        double DouSaleDiscount = 0;
            //        double DouSaleRapaport = 0;
            //        double DouSalePricePerCarat = 0;
            //        double DouSaleAmount = 0;

            //        foreach (DataRow DRow in DtabParamUpdate.Rows)
            //        {
            //            if (ChkcheckDiscModify.Checked == true)
            //            {

            //                DouSaleDiscount = Val.Val(txtSaleDiscAddLess.Text);
            //                DouSaleRapaport = Val.Val(DRow["SALERAPAPORT"]);
            //                DouSalePricePerCarat = Math.Round(DouSaleRapaport - ((DouSaleRapaport * DouSaleDiscount) / 100), 2);
            //                DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);

            //                DRow["SALEDISCOUNT"] = DouSaleDiscount;
            //                DRow["SALEPRICEPERCARAT"] = DouSalePricePerCarat;
            //                DRow["SALEAMOUNT"] = DouSaleAmount;
            //            }
            //            else
            //            {

            //                DouSaleDiscount = Val.Val(DRow["SALEDISCOUNT"]) + Val.Val(txtSaleDiscAddLess.Text);
            //                DouSaleRapaport = Val.Val(DRow["SALERAPAPORT"]);
            //                DouSalePricePerCarat = Math.Round(DouSaleRapaport - ((DouSaleRapaport * DouSaleDiscount) / 100), 2);
            //                DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);

            //                DRow["SALEDISCOUNT"] = DouSaleDiscount;
            //                DRow["SALEPRICEPERCARAT"] = DouSalePricePerCarat;
            //                DRow["SALEAMOUNT"] = DouSaleAmount;
            //            }



            //        }
            //        DtabParamUpdate.AcceptChanges();
            //        Calculation();
            //    }
            //    catch (Exception ex)
            //    {
            //        Global.Message(ex.Message.ToString());
            //    }
            //  }
        }

        private void txtExpDiscAddLess_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {



                    if (RdbParameter.Checked)
                    {
                        Global.Message("Please Select 'All' Or 'Price' Checkbox For Update Stone Price.");
                        return;
                    }

                    double DouExpDiscount = 0;
                    double DouExpRapaport = 0;
                    double DouExpPricePerCarat = 0;
                    double DouExpAmount = 0;

                    foreach (DataRow DRow in DtabParamUpdate.Rows)
                    {
                        if (ChkcheckDiscModify.Checked == true)
                        {
                            DouExpDiscount = Val.Val(txtExpDiscAddLess.Text);
                            DouExpRapaport = Val.Val(DRow["EXPRAPAPORT"]);
                            DouExpPricePerCarat = Math.Round(DouExpRapaport + ((DouExpRapaport * DouExpDiscount) / 100), 2); //#P
                            DouExpAmount = Math.Round(DouExpPricePerCarat * Val.Val(DRow["CARAT"]), 2);

                            DRow["EXPDISCOUNT"] = DouExpDiscount;
                            DRow["EXPPRICEPERCARAT"] = DouExpPricePerCarat;
                            DRow["EXPAMOUNT"] = DouExpAmount;

                        }
                        else
                        {
                            DouExpDiscount = Val.Val(DRow["EXPDISCOUNT"]) + Val.Val(txtExpDiscAddLess.Text);
                            DouExpRapaport = Val.Val(DRow["EXPRAPAPORT"]);
                            DouExpPricePerCarat = Math.Round(DouExpRapaport + ((DouExpRapaport * DouExpDiscount) / 100), 2); //#P
                            DouExpAmount = Math.Round(DouExpPricePerCarat * Val.Val(DRow["CARAT"]), 2);

                            DRow["EXPDISCOUNT"] = DouExpDiscount;
                            DRow["EXPPRICEPERCARAT"] = DouExpPricePerCarat;
                            DRow["EXPAMOUNT"] = DouExpAmount;

                        }
                        DRow["EXPDISCOUNT"] = DouExpDiscount;
                        DRow["EXPPRICEPERCARAT"] = DouExpPricePerCarat;
                        DRow["EXPAMOUNT"] = DouExpAmount;

                    }
                    DtabParamUpdate.AcceptChanges();
                    Calculation();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }

        }
    }
}
