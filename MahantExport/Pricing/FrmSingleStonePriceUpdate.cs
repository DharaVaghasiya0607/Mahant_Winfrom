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
    public partial class FrmSingleStonePriceUpdate : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        BOTRN_SingleStonePriceUpdate ObjTrn = new BOTRN_SingleStonePriceUpdate();
        BOFormPer ObjPer = new BOFormPer();

        DataTable DTabRapaportData = new DataTable();

        DataTable DTabStoneReviseData = new DataTable();

        double DouCarat = 0;
        double DouOrgCarat = 0;
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

        double DouExpRapaport = 0;
        double DouExpRapaportAmt = 0;
        double DouExpDisc = 0;
        double DouExpPricePerCarat = 0;
        double DouExpAmount = 0;

        string StrStonePriceXml = "";
        string StrSearchType = "";
        string StrPriceType = "";

        int count = 0;

        bool ISFromMFGSide = false;

        #region Property Settings

        public FrmSingleStonePriceUpdate()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();

            Clear();
            ISFromMFGSide = false;
            GrdDetail.Bands["BANDMFG"].Visible = ChkMFGPricing.Checked;
            GrdDetail.Bands["BANDCOST"].Visible = ChkCOSTPricing.Checked;
            GrdDetail.Bands["BANDAVG"].Visible = ChkAVGPricing.Checked;
            GrdDetail.Bands["BANDSALE"].Visible = ChkSALEPricing.Checked;
            GrdDetail.Bands["BANDEXP"].Visible = chkExpPricing.Checked;
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
        public void ShowForm_MFG()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();

            Clear();
            ISFromMFGSide = true;
            ChkCOSTPricing.Visible = false;
            ChkAVGPricing.Visible = false;
            ChkSALEPricing.Visible = false;
            chkExpPricing.Visible = false;

            GrdDetail.Columns["COLORNAME"].Visible = false;
            GrdDetail.Columns["CLARITYNAME"].Visible = false;

            GrdDetail.Bands["BANDMFG"].Visible = ChkMFGPricing.Checked;
            GrdDetail.Bands["BANDCOST"].Visible = ChkCOSTPricing.Checked;
            GrdDetail.Bands["BANDAVG"].Visible = ChkAVGPricing.Checked;
            GrdDetail.Bands["BANDSALE"].Visible = ChkSALEPricing.Checked;
            GrdDetail.Bands["BANDEXP"].Visible = chkExpPricing.Checked;


        }

        #region Button Events

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnStoneRevShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtStoneNo.Text == "")
                {
                    Global.Message("Please Enter Stone No That You Want To Update...");
                    txtBackPriceFileName.Focus();
                    return;
                }


                this.Cursor = Cursors.WaitCursor;

                LiveStockProperty LStockProperty = new LiveStockProperty();

                LStockProperty.STOCKNO = Val.ToString(txtStoneNo.Text);

                DTabStoneReviseData.TableName = "Detail";
                DTabStoneReviseData = ObjTrn.GetSingleStonePriceData(LStockProperty);

                MainGrdDetail.DataSource = DTabStoneReviseData;
                if (DTabStoneReviseData.Rows.Count>0)
                {
                    GrdDetail.BestFitColumns();
                }
                //GrdDetail.RefreshData();

                ChkMFGPricing_CheckedChanged(null, null);
                ChkCOSTPricing_CheckedChanged(null, null);
                ChkAVGPricing_CheckedChanged(null, null);
                ChkSALEPricing_CheckedChanged(null, null);
                chkExpPricing_CheckedChanged(null, null);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void btnStoneRevUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTab = DTabStoneReviseData.GetChanges();
                if (DTab == null || DTab.Rows.Count <= 0)
                {
                    if (Global.Confirm("No Any Changes , Still You Want Save This All Record ?") == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }
                    DTab = DTabStoneReviseData.Copy();
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
                Property = ObjTrn.SaveSingleStonePriceUpdateDetail(ParameterUpdateXml, ISFromMFGSide);
                this.Cursor = Cursors.Default;
                Global.Message(Property.ReturnMessageDesc);
                if (Property.ReturnMessageType == "SUCCESS")
                {
                    DTabStoneReviseData.AcceptChanges();
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
            Global.ExcelExport("Single Stone Price List", GrdDetail);
        }

        private void btnStoneRevExit_Click(object sender, EventArgs e)
        {
            this.Close();
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
                    DTabStoneReviseData = Global.GetDataTableFromExcel(destinationPath, true);

                    for (int Intcol = 0; Intcol < DTabStoneReviseData.Columns.Count; Intcol++)
                    {
                        if (Val.ToString("Stone No,StockNo").ToUpper().Split(',').Contains(Val.ToString(DTabStoneReviseData.Columns[Intcol].ColumnName.ToUpper())))
                            DTabStoneReviseData.Columns[Intcol].ColumnName = Val.ToString("STOCKNO");
                        if (Val.ToString("Rap. Price").ToUpper().Split(',').Contains(Val.ToString(DTabStoneReviseData.Columns[Intcol].ColumnName.ToUpper())))
                            DTabStoneReviseData.Columns[Intcol].ColumnName = Val.ToString("RAPAPORT");
                        if (Val.ToString("Disc %,Disc").ToUpper().Split(',').Contains(Val.ToString(DTabStoneReviseData.Columns[Intcol].ColumnName.ToUpper())))
                            DTabStoneReviseData.Columns[Intcol].ColumnName = Val.ToString("DISC");
                        if (Val.ToString("Net Rate,PerCarat").ToUpper().Split(',').Contains(Val.ToString(DTabStoneReviseData.Columns[Intcol].ColumnName.ToUpper())))
                            DTabStoneReviseData.Columns[Intcol].ColumnName = Val.ToString("PRICEPERCARAT");
                        if (Val.ToString("Net Value").ToUpper().Split(',').Contains(Val.ToString(DTabStoneReviseData.Columns[Intcol].ColumnName.ToUpper())))
                            DTabStoneReviseData.Columns[Intcol].ColumnName = Val.ToString("AMOUNT");
                    }

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

        private void BtnPriceFileUpload_Click(object sender, EventArgs e)
        {
            try
            {
                // int count = 0;

                if (Val.ToString(txtBackPriceFileName.Text).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Select File That You Want To Update...");
                    txtBackPriceFileName.Focus();
                    return;
                }

                //foreach (Control c in panelCheckbox.Controls)
                //{
                //    DevExpress.XtraEditors.CheckEdit cb = c as DevExpress.XtraEditors.CheckEdit;
                //    if (cb != null && cb.Checked)
                //    {
                //        count++;
                //        StrPriceType = cb.Tag.ToString();
                //    }
                //}

                //if (count != 1)
                //{
                //    Global.Message("Please Select Atleast One Pricing Checkbox...");
                //    return;
                //}

                if (StrPriceType != "EXP")
                {
                    if (DTabStoneReviseData.Columns.Count > 3)
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Upload Invalid File. Pls Check..");
                        return;
                    }
                    for (int k = 0; k < DTabStoneReviseData.Rows.Count; k++)
                    {
                        if ((Val.Val(DTabStoneReviseData.Rows[k]["DISC"]) != 0) && (Val.Val(DTabStoneReviseData.Rows[k]["PRICEPERCARAT"]) != 0))
                        {
                            this.Cursor = Cursors.Default;
                            Global.Message("Back And PerCarat Both Are Exists In StoneNo : '" + Val.ToString(DTabStoneReviseData.Rows[k]["STOCKNO"]) + "' Pls Check..");
                            return;
                        }
                    }
                }
                else
                {
                    if (DTabStoneReviseData.Columns.Count <= 3)
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Upload Invalid File. Pls Check..");
                        return;
                    }
                }

                panelCheckbox.Enabled = false;
                DTabStoneReviseData.TableName = "Table1";
                using (StringWriter sw = new StringWriter())
                {
                    DTabStoneReviseData.WriteXml(sw);
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

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            Clear();
        }
        #endregion

        private void Clear()
        {
            MainGrdDetail.DataSource = null;
            panelCheckbox.Enabled = true;
            ChkUpdateExpPrice.Checked = false;
            txtBackPriceFileName.Text = "";
            txtStoneNo.Text = "";

            ChkMFGPricing.Checked = true;
            ChkCOSTPricing.Checked = false;
            ChkAVGPricing.Checked = false;
            ChkSALEPricing.Checked = false;
            chkExpPricing.Checked = false;

            PnlFileWisePriceUpdate.Visible = false;
        }

        private void FrmPriceRevised_Load(object sender, EventArgs e)
        {
            string Str = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdDetail.Name);

            if (Str != "")
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                MemoryStream stream = new MemoryStream(byteArray);
                GrdDetail.RestoreLayoutFromStream(stream);
            }
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


        #region  Checkbox Events

        private void ChkMFGPricing_CheckedChanged(object sender, EventArgs e)
        {
            GrdDetail.Bands["BANDMFG"].Visible = ChkMFGPricing.Checked;
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

        private void ChkUpdateExpPrice_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkUpdateExpPrice.Checked)
            {
                count = 0;
                foreach (Control c in panelCheckbox.Controls)
                {
                    DevExpress.XtraEditors.CheckEdit cb = c as DevExpress.XtraEditors.CheckEdit;
                    if (cb != null && cb.Checked)
                    {
                        count++;
                        StrPriceType = cb.Tag.ToString();
                    }
                }
                if (count != 1)
                {
                    Global.Message("Please Select Atleast One Pricing Checkbox...");
                    ChkUpdateExpPrice.Checked = false;
                    return;
                }

                PnlFileWisePriceUpdate.Visible = true;
                panelCheckbox.Enabled = false;
            }
            else
            {
                PnlFileWisePriceUpdate.Visible = false;
                panelCheckbox.Enabled = true;
            }
        }

        private void chkExpPricing_CheckedChanged(object sender, EventArgs e)
        {
            GrdDetail.Bands["BANDEXP"].Visible = chkExpPricing.Checked;
        }

        #endregion

        #region Grid Events

        private void GrdDetail_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                return;
            }
            DataRow DRow = GrdDetail.GetDataRow(e.RowHandle);
            switch (e.Column.FieldName)
            {
                case "SALEDISCOUNT":

                    double Rapaport = Val.Val(DRow["SALERAPAPORT"]);
                    double Carat = Val.Val(DRow["CARAT"]);

                    DTabStoneReviseData.Rows[e.RowHandle]["SALERAPAPORT"] = Rapaport;
                    DTabStoneReviseData.Rows[e.RowHandle]["SALEPRICEPERCARAT"] = Math.Round(Rapaport - ((Rapaport * Val.Val(DRow["SALEDISCOUNT"])) / 100));
                    DTabStoneReviseData.Rows[e.RowHandle]["SALEAMOUNT"] = Math.Round(Carat * Val.Val(DTabStoneReviseData.Rows[e.RowHandle]["SALEPRICEPERCARAT"]), 2);

                    break;

                case "MFGDISCOUNT":

                    Rapaport = Val.Val(DRow["MFGRAPAPORT"]);
                    Carat = Val.Val(DRow["CARAT"]);

                    DTabStoneReviseData.Rows[e.RowHandle]["MFGRAPAPORT"] = Rapaport;
                    DTabStoneReviseData.Rows[e.RowHandle]["MFGPRICEPERCARAT"] = Math.Round(Rapaport - ((Rapaport * Val.Val(DRow["MFGDISCOUNT"])) / 100));
                    DTabStoneReviseData.Rows[e.RowHandle]["MFGAMOUNT"] = Math.Round(Carat * Val.Val(DTabStoneReviseData.Rows[e.RowHandle]["MFGPRICEPERCARAT"]), 2);

                    break;

                case "COSTDISCOUNT":

                    Rapaport = Val.Val(DRow["COSTRAPAPORT"]);
                    Carat = Val.Val(DRow["ORGCARAT"]);

                    DTabStoneReviseData.Rows[e.RowHandle]["COSTRAPAPORT"] = Rapaport;
                    DTabStoneReviseData.Rows[e.RowHandle]["COSTPRICEPERCARAT"] = Math.Round(Rapaport - ((Rapaport * Val.Val(DRow["COSTDISCOUNT"])) / 100));
                    DTabStoneReviseData.Rows[e.RowHandle]["COSTAMOUNT"] = Math.Round(Carat * Val.Val(DTabStoneReviseData.Rows[e.RowHandle]["COSTPRICEPERCARAT"]), 2);

                    break;

                case "AVGDISCOUNT":

                    Rapaport = Val.Val(DRow["AVGRAPAPORT"]);
                    Carat = Val.Val(DRow["ORGCARAT"]);

                    DTabStoneReviseData.Rows[e.RowHandle]["AVGRAPAPORT"] = Rapaport;
                    DTabStoneReviseData.Rows[e.RowHandle]["AVGPRICEPERCARAT"] = Math.Round(Rapaport - ((Rapaport * Val.Val(DRow["AVGDISCOUNT"])) / 100));
                    DTabStoneReviseData.Rows[e.RowHandle]["AVGAMOUNT"] = Math.Round(Carat * Val.Val(DTabStoneReviseData.Rows[e.RowHandle]["AVGPRICEPERCARAT"]), 2);

                    break;

                case "EXPDISCOUNT":

                    Rapaport = Val.Val(DRow["EXPRAPAPORT"]);
                    Carat = Val.Val(DRow["CARAT"]);

                    DTabStoneReviseData.Rows[e.RowHandle]["EXPRAPAPORT"] = Rapaport;
                    DTabStoneReviseData.Rows[e.RowHandle]["EXPPRICEPERCARAT"] = Math.Round(Rapaport - ((Rapaport * Val.Val(DRow["EXPDISCOUNT"])) / 100));
                    DTabStoneReviseData.Rows[e.RowHandle]["EXPAMOUNT"] = Math.Round(Carat * Val.Val(DTabStoneReviseData.Rows[e.RowHandle]["EXPPRICEPERCARAT"]), 2);

                    break;

                case "SALEPRICEPERCARAT":

                    Rapaport = Val.Val(DRow["SALERAPAPORT"]);
                    double PricePerCarat = Val.Val(DRow["SALEPRICEPERCARAT"]);
                    Carat = Val.Val(DRow["CARAT"]);
                    double DouPer = 0;
                    if (Rapaport != 0)
                    {
                        DouPer = Math.Round(((Rapaport - PricePerCarat) / Rapaport) * 100, 2);
                    }
                    else
                        DouPer = 0;

                    DTabStoneReviseData.Rows[e.RowHandle]["SALERAPAPORT"] = Rapaport;
                    DTabStoneReviseData.Rows[e.RowHandle]["SALEDISCOUNT"] = DouPer;
                    DTabStoneReviseData.Rows[e.RowHandle]["SALEAMOUNT"] = Math.Round(Carat * Val.Val(DTabStoneReviseData.Rows[e.RowHandle]["SALEPRICEPERCARAT"]), 2);

                    break;

                case "MFGPRICEPERCARAT":

                    Rapaport = Val.Val(DRow["MFGRAPAPORT"]);
                    PricePerCarat = Val.Val(DRow["MFGPRICEPERCARAT"]);
                    Carat = Val.Val(DRow["CARAT"]);
                    DouPer = 0;
                    if (Rapaport != 0)
                    {
                        DouPer = Math.Round(((Rapaport - PricePerCarat) / Rapaport) * 100, 2);
                    }
                    else
                        DouPer = 0;

                    DTabStoneReviseData.Rows[e.RowHandle]["MFGRAPAPORT"] = Rapaport;
                    DTabStoneReviseData.Rows[e.RowHandle]["MFGDISCOUNT"] = DouPer;
                    DTabStoneReviseData.Rows[e.RowHandle]["MFGAMOUNT"] = Math.Round(Carat * Val.Val(DTabStoneReviseData.Rows[e.RowHandle]["MFGPRICEPERCARAT"]), 2);

                    break;

                case "COSTPRICEPERCARAT":

                    Rapaport = Val.Val(DRow["COSTRAPAPORT"]);
                    PricePerCarat = Val.Val(DRow["COSTPRICEPERCARAT"]);
                    Carat = Val.Val(DRow["CARAT"]);
                    DouPer = 0;
                    if (Rapaport != 0)
                    {
                        DouPer = Math.Round(((Rapaport - PricePerCarat) / Rapaport) * 100, 2);
                    }
                    else
                        DouPer = 0;

                    DTabStoneReviseData.Rows[e.RowHandle]["COSTRAPAPORT"] = Rapaport;
                    DTabStoneReviseData.Rows[e.RowHandle]["COSTDISCOUNT"] = DouPer;
                    DTabStoneReviseData.Rows[e.RowHandle]["COSTAMOUNT"] = Math.Round(Carat * Val.Val(DTabStoneReviseData.Rows[e.RowHandle]["COSTPRICEPERCARAT"]), 2);

                    break;

                case "AVGPRICEPERCARAT":

                    Rapaport = Val.Val(DRow["AVGRAPAPORT"]);
                    PricePerCarat = Val.Val(DRow["AVGPRICEPERCARAT"]);
                    Carat = Val.Val(DRow["CARAT"]);
                    DouPer = 0;
                    if (Rapaport != 0)
                    {
                        DouPer = Math.Round(((Rapaport - PricePerCarat) / Rapaport) * 100, 2);
                    }
                    else
                        DouPer = 0;

                    DTabStoneReviseData.Rows[e.RowHandle]["AVGRAPAPORT"] = Rapaport;
                    DTabStoneReviseData.Rows[e.RowHandle]["AVGDISCOUNT"] = DouPer;
                    DTabStoneReviseData.Rows[e.RowHandle]["AVGAMOUNT"] = Math.Round(Carat * Val.Val(DTabStoneReviseData.Rows[e.RowHandle]["AVGPRICEPERCARAT"]), 2);

                    break;

                case "EXPPRICEPERCARAT":

                    Rapaport = Val.Val(DRow["EXPRAPAPORT"]);
                    PricePerCarat = Val.Val(DRow["EXPPRICEPERCARAT"]);
                    Carat = Val.Val(DRow["CARAT"]);
                    DouPer = 0;
                    if (Rapaport != 0)
                    {
                        DouPer = Math.Round(((Rapaport - PricePerCarat) / Rapaport) * 100, 2);
                    }
                    else
                        DouPer = 0;

                    DTabStoneReviseData.Rows[e.RowHandle]["EXPRAPAPORT"] = Rapaport;
                    DTabStoneReviseData.Rows[e.RowHandle]["EXPDISCOUNT"] = DouPer;
                    DTabStoneReviseData.Rows[e.RowHandle]["EXPAMOUNT"] = Math.Round(Carat * Val.Val(DTabStoneReviseData.Rows[e.RowHandle]["EXPPRICEPERCARAT"]), 2);

                    break;

                default:
                    break;
            }

        }

        private void GrdDetail_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            try
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    DouCarat = 0;
                    DouOrgCarat = 0;
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

                    DouExpRapaport = 0;
                    DouExpRapaportAmt = 0;
                    DouExpDisc = 0;
                    DouExpPricePerCarat = 0;
                    DouExpAmount = 0;

                }
                else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DouCarat = DouCarat + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT"));
                    DouOrgCarat = DouOrgCarat + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "ORGCARAT"));

                    DouCostAmount = DouCostAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "COSTAMOUNT"));
                    DouCostRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "COSTRAPAPORT"));
                    DouCostPricePerCarat = DouCostAmount / DouOrgCarat;
                    DouCostRapaportAmt = DouCostRapaportAmt + (DouCostRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "ORGCARAT")));

                    DouSaleAmount = DouSaleAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "SALEAMOUNT"));
                    DouSaleRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "SALERAPAPORT"));
                    DouSalePricePerCarat = DouSaleAmount / DouCarat;
                    DouSaleRapaportAmt = DouSaleRapaportAmt + (DouSaleRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

                    DouAvgAmount = DouAvgAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "AVGAMOUNT"));
                    DouAvgRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "AVGRAPAPORT"));
                    DouAvgPricePerCarat = DouAvgAmount / DouOrgCarat;
                    DouAvgRapaportAmt = DouAvgRapaportAmt + (DouAvgRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "ORGCARAT")));

                    DouMFGAmount = DouMFGAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "MFGAMOUNT"));
                    DouMFGRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "MFGRAPAPORT"));
                    DouMFGPricePerCarat = DouMFGAmount / DouCarat;
                    DouMFGRapaportAmt = DouMFGRapaportAmt + (DouMFGRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

                    DouExpAmount = DouExpAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "EXPAMOUNT"));
                    DouExpRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "EXPRAPAPORT"));
                    DouExpPricePerCarat = DouExpAmount / DouCarat;
                    DouExpRapaportAmt = DouExpRapaportAmt + (DouExpRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

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
                        e.TotalValue = DouMFGDisc;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("EXPPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouExpAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("EXPRAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouExpRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("EXPDISCOUNT") == 0)
                    {
                        DouExpRapaport = Math.Round(DouExpRapaportAmt / DouCarat);
                        DouExpDisc = Math.Round(((DouExpRapaport - DouExpPricePerCarat) / DouExpRapaport * 100), 2);
                        e.TotalValue = DouExpDisc;
                    }
                }
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


        #endregion

        #region Background Worker Events

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DTabStoneReviseData = ObjTrn.GetStonePriceExcelWiseGetData(StrStonePriceXml, StrSearchType, StrPriceType);
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
                if (DTabStoneReviseData.Rows.Count <= 0)
                {
                    Global.Message("Excel File Stone's Not Found Please check..");
                    progressPanel1.Visible = false;
                    return;
                }

                MainGrdDetail.DataSource = DTabStoneReviseData;
                GrdDetail.RefreshData();
                //Calculation();

                progressPanel1.Visible = false;
                ChkMFGPricing_CheckedChanged(null, null);
                ChkCOSTPricing_CheckedChanged(null, null);
                ChkAVGPricing_CheckedChanged(null, null);
                ChkSALEPricing_CheckedChanged(null, null);
                chkExpPricing_CheckedChanged(null, null);
            }
            catch (Exception ex)
            {
                progressPanel1.Visible = false;
                Global.Message(ex.Message.ToString());
            }
        }

        #endregion

        private void lblSampleExcelFile_Click(object sender, EventArgs e)
        {
            try
            {
                string StrFilePathDestination = "";

                if (StrPriceType != "EXP")
                {
                    StrFilePathDestination = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\JangedWiseBackPriceUpdateFormat" + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.Day.ToString() + ".xlsx";
                    if (File.Exists(StrFilePathDestination))
                    {
                        File.Delete(StrFilePathDestination);
                    }
                    File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\Format\\JangedWiseBackPriceUpdateFormat.xlsx", StrFilePathDestination);
                }
                else
                {
                    StrFilePathDestination = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\ExpPriceUpdateFormat" + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.Day.ToString() + ".xlsx";
                    if (File.Exists(StrFilePathDestination))
                    {
                        File.Delete(StrFilePathDestination);
                    }
                    File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\Format\\ExpPriceUpdateFormat.xlsx", StrFilePathDestination);
                }

                System.Diagnostics.Process.Start(StrFilePathDestination, "CMD");
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void lblSaveLayout_Click(object sender, EventArgs e)
        {
            Stream str = new System.IO.MemoryStream();
            GrdDetail.SaveLayoutToStream(str);
            str.Seek(0, System.IO.SeekOrigin.Begin);
            StreamReader reader = new StreamReader(str);
            string text = reader.ReadToEnd();

            int IntRes = new BOTRN_StockUpload().SaveGridLayout(this.Name, GrdDetail.Name, text);
            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Saved");
            }
        }

        private void lblDefaultLayout_Click(object sender, EventArgs e)
        {
            int IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdDetail.Name);
            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Deleted");
            }
        }


    }
}
