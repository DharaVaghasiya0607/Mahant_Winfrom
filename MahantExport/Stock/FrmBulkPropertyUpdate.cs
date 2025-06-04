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

namespace MahantExport.Stock
{
    public partial class FrmBulkPropertyUpdate : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();

        DataTable DTabLinkUpdate = new DataTable();

        String PasteData = "";
        IDataObject PasteclipData = Clipboard.GetDataObject();

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

        double DouExpRapaport = 0;
        double DouExpRapaportAmt = 0;
        double DouExpDisc = 0;
        double DouExpPricePerCarat = 0;
        double DouExpAmount = 0;

        double DouOfferRapaport = 0;
        double DouOfferRapaportAmt = 0;
        double DouOfferDisc = 0;
        double DouOfferPricePerCarat = 0;
        double DouOfferAmount = 0;

        double DouMemoRapaport = 0;
        double DouMemoRapaportAmt = 0;
        double DouMemoDisc = 0;
        double DouMemoPricePerCarat = 0;
        double DouMemoAmount = 0;

        #region Property Settings

        public FrmBulkPropertyUpdate()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();
            CmbParameterType.SelectedIndex = 0;
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
            ObjFormEvent.ObjToDisposeList.Add(ObjStock);
        }
        #endregion
        
        //private void txtStoneCertiMFGMemo_KeyUp(object sender, KeyEventArgs e)
        //{
        //    //if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
        //    //{
        //    //    IDataObject clipData = Clipboard.GetDataObject();
        //    //    String Data = Convert.ToString(clipData.GetData(System.Windows.Forms.DataFormats.Text));
        //    //    String str1 = Data.Replace("\r\n", ",");                   //data.Replace(\n, ",");
        //    //    str1 = str1.Trim();
        //    //    str1 = str1.TrimEnd();
        //    //    str1 = str1.TrimStart();
        //    //    str1 = str1.TrimEnd(',');
        //    //    str1 = str1.TrimStart(',');
        //    //    txtStoneCertiMFGMemo.Text = str1;
        //    //}
        //    //lblTotalCountStoneNo.Text = txtStoneCertiMFGMemo.Text.ToString().Trim().Equals(string.Empty) ? "(0)" : "(" + txtStoneCertiMFGMemo.Text.Split(',').Length + ")";
        //}

        //private void txtStoneCertiMFGMemo_MouseDown(object sender, MouseEventArgs e)
        //{
        //    //if (txtStoneCertiMFGMemo.Focus())
        //    //{
        //    //    if (e.Button == System.Windows.Forms.MouseButtons.Right)
        //    //    {
        //    //        PasteData = Convert.ToString(PasteclipData.GetData(System.Windows.Forms.DataFormats.Text));
        //    //    }
        //    //}
        //    //lblTotalCountStoneNo.Text = txtStoneCertiMFGMemo.Text.ToString().Trim().Equals(string.Empty) ? "(0)" : "(" + txtStoneCertiMFGMemo.Text.Split(',').Length + ")";
        //}

        private void txtStoneCertiMFGMemo_TextChanged(object sender, EventArgs e)
        {
            String str1 = txtStoneCertiMFGMemo.Text.Trim().Replace("\r\n", ",");
            txtStoneCertiMFGMemo.Text = str1;
            lblTotalCountStoneNo.Text = txtStoneCertiMFGMemo.Text.ToString().Trim().Equals(string.Empty) ? "(0)" : "(" + txtStoneCertiMFGMemo.Text.Split(',').Length + ")";
        }

        //private void txtValue_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
        //    {
        //        IDataObject clipData = Clipboard.GetDataObject();
        //        String Data = Convert.ToString(clipData.GetData(System.Windows.Forms.DataFormats.Text));
        //        String str1 = Data.Replace("\r\n", ",");                   //data.Replace(\n, ",");
        //        str1 = str1.Trim();
        //        str1 = str1.TrimEnd();
        //        str1 = str1.TrimStart();
        //        str1 = str1.TrimEnd(',');
        //        str1 = str1.TrimStart(',');
        //        txtValue.Text = str1;
        //    }
        //    //lblTotalCountValues.Text = "(" + txtStoneCertiMFGMemo.Text.Split(',').Length + ")";
        //    lblTotalCountValues.Text = txtValue.Text.ToString().Trim().Equals(string.Empty) ? "(0)" : "(" + txtValue.Text.Split(',').Length + ")";
        //}

        //private void txtValue_MouseDown(object sender, MouseEventArgs e)
        //{
        //    if (txtValue.Focus())
        //    {
        //        if (e.Button == System.Windows.Forms.MouseButtons.Right)
        //        {
        //            PasteData = Convert.ToString(PasteclipData.GetData(System.Windows.Forms.DataFormats.Text));
        //        }
        //    }
        //    //lblTotalCountValues.Text = "(" + txtValue.Text.Split(',').Length + ")";
        //    lblTotalCountValues.Text = txtValue.Text.ToString().Trim().Equals(string.Empty) ? "(0)" : "(" + txtValue.Text.Split(',').Length + ")";
        //}

        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            //if (txtValue.Text.Length > 0 && Convert.ToString(PasteData) != "")
            //{
            //    txtValue.SelectAll();
            //    String str1 = PasteData.Replace("\r\n", ",");                   //data.Replace(\n, ",");
            //    str1 = str1.Trim();
            //    str1 = str1.TrimEnd();
            //    str1 = str1.TrimStart();
            //    str1 = str1.TrimEnd(',');
            //    str1 = str1.TrimStart(',');
            //    txtValue.Text = str1;
            //    PasteData = "";
            //}
            String str1 = txtValue.Text.Trim().Replace("\r\n", ",");
            txtValue.Text = str1;
            lblTotalCountValues.Text = txtValue.Text.ToString().Trim().Equals(string.Empty) ? "(0)" : "(" + txtValue.Text.Split(',').Length + ")";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtStoneCertiMFGMemo.Text.Trim().Length == 0)
                {
                    Global.MessageError("Stone No Is Required");
                    txtStoneCertiMFGMemo.Focus();
                    return;
                }
                if (txtValue.Text.Trim().Length == 0)
                {
                    Global.MessageError("Values Is Required");
                    txtValue.Focus();
                    return;
                }

                //Comment And Added by Daksha on 12/01/2023
                //if (Val.ToString(CmbParameterType.Text) == "OFFERPRICE" && Val.ToString(txtParty.Text).Trim().Equals(string.Empty))
                //{
                //    Global.Message("Offer Party Is Required.");
                //    txtParty.Focus();
                //    return;
                //}

                if ((Val.ToString(CmbParameterType.Text) == "OFFERPRICE" || Val.ToString(CmbParameterType.Text) == "OFFERDISCOUNT") && Val.ToString(txtParty.Text).Trim().Equals(string.Empty) && Val.ToString(txtBroker.Text).Trim().Equals(string.Empty))
                {
                    Global.Message("Offer Party/Broker Is Required.");
                    txtParty.Focus();
                    return;
                }
                //End as Daksha

                string[] SplitStoneNo = txtStoneCertiMFGMemo.Text.Split(',');
                string[] SplitValues = txtValue.Text.Split(',');

                if (SplitStoneNo.Length != SplitValues.Length)
                {
                    Global.MessageError("Stone No And Values Count Mismatch");
                    txtValue.Focus();
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                DataTable DTabStoneNo = new DataTable("Table");
                DTabStoneNo.Columns.Add(new DataColumn("SRNO", typeof(int)));
                DTabStoneNo.Columns.Add(new DataColumn("STONENO", typeof(string)));

                DataTable DTabValues = new DataTable("Table");
                DTabValues.Columns.Add(new DataColumn("SRNO", typeof(int)));
                DTabValues.Columns.Add(new DataColumn("PARAVALUE", typeof(string)));

                for (int i = 0; i < SplitStoneNo.Length; i++)
                {
                    DataRow DR = DTabStoneNo.NewRow();
                    DR["SRNO"] = i + 1;
                    DR["STONENO"] = SplitStoneNo[i].ToString().Replace("\r\n", "");
                    DTabStoneNo.Rows.Add(DR);
                }

                for (int i = 0; i < SplitValues.Length; i++)
                {
                    DataRow DR = DTabValues.NewRow();
                    DR["SRNO"] = i + 1;
                    DR["PARAVALUE"] = SplitValues[i].ToString().Replace("\r\n", "");
                    DTabValues.Rows.Add(DR);
                }
                DTabStoneNo.AcceptChanges();
                DTabValues.AcceptChanges();

                string StrXMLStoneNo = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabStoneNo.WriteXml(sw);
                    StrXMLStoneNo = sw.ToString();
                }
                string StrXMLValues = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabValues.WriteXml(sw);
                    StrXMLValues = sw.ToString();
                }

                string StrStoneType = string.Empty;
                if (RbtStoneNo.Checked)
                {
                    StrStoneType = "STONENO";
                }
                else if (RbtSerialNo.Checked)
                {
                    StrStoneType = "SERIALNO";
                }
                else if (RbtCertiNo.Checked)
                {
                    StrStoneType = "CERTNO";
                }

                DataTable DTabStoneDetail = new DataTable();

                Guid gParty_ID;
                gParty_ID = Val.ToString(txtParty.Text).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtParty.Tag));

                //Added by Daksha on 12/01/2023
                Guid gBroker_ID;
                gBroker_ID = Val.ToString(txtBroker.Text).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtBroker.Tag));
                //End as Daksha

                DTabStoneDetail = ObjStock.BulkPropertUpdate(StrStoneType, Val.ToString(CmbParameterType.SelectedItem), StrXMLStoneNo, StrXMLValues, gParty_ID, gBroker_ID,Val.ToString(txtComment.Text));

                if (DTabStoneDetail.Rows[0]["RETURNTYPE"].ToString() == "SUCCESS")
                {
                    DTabStoneDetail.Rows[0]["RETURNDESC"] = "Successfully Updates";
                    MainGrdDetail.DataSource = DTabStoneDetail;
                    MainGrdDetail.Refresh();
                    Global.Message(DTabStoneDetail.Rows[0]["RETURNDESC"].ToString());
                    Clear();
                }
                else
                {
                    Global.Message(DTabStoneDetail.Rows[0]["RETURNDESC"].ToString());
                }                
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {


        }

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

                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTYONLY);
                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
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

        private void CmbParameterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbParameterType.Text == "OFFERPRICE" || CmbParameterType.Text == "OFFERDISCOUNT")
            {
                lblParty.Visible = true;
                txtParty.Visible = true;
                lblBroker.Visible = true;
                txtBroker.Visible = true;
                lblComment.Visible = true;
                txtComment.Visible = true;
            }
            else
            {
                lblParty.Visible = false;
                txtParty.Visible = false;
                lblBroker.Visible = false;
                txtBroker.Visible = false;
                lblComment.Visible = false;
                txtComment.Visible = false;
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

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
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

                    DouExpRapaport = 0;
                    DouExpRapaportAmt = 0;
                    DouExpDisc = 0;
                    DouExpPricePerCarat = 0;
                    DouExpAmount = 0;

                    DouOfferRapaport = 0;
                    DouOfferRapaportAmt = 0;
                    DouOfferDisc = 0;
                    DouOfferPricePerCarat = 0;
                    DouOfferAmount = 0;

                    DouMemoRapaport = 0;
                    DouMemoRapaportAmt = 0;
                    DouMemoDisc = 0;
                    DouMemoPricePerCarat = 0;
                    DouMemoAmount = 0;

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

                    DouExpAmount = DouExpAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "EXPAMOUNT"));
                    DouExpRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "EXPRAPAPORT"));
                    DouExpPricePerCarat = DouExpAmount / DouCarat;
                    DouExpRapaportAmt = DouExpRapaportAmt + (DouExpRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

                    DouOfferAmount = DouOfferAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "OFFERAMOUNT"));
                    DouOfferRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "OFFERRAPAPORT"));
                    DouOfferPricePerCarat = DouOfferAmount / DouCarat;
                    DouOfferRapaportAmt = DouOfferRapaportAmt + (DouOfferRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

                    DouMemoAmount = DouMemoAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "MEMOAMOUNT"));
                    DouMemoRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "MEMORAPAPORT"));
                    DouMemoPricePerCarat = DouMemoAmount / DouCarat;
                    DouMemoRapaportAmt = DouMemoRapaportAmt + (DouMemoRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));
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
                        //DouCostDisc = Math.Round(((DouCostPricePerCarat - DouCostRapaport) / DouCostRapaport * 100), 2);
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
                        //DouSaleDisc = Math.Round(((DouSalePricePerCarat - DouSaleRapaport) / DouSaleRapaport * 100), 2);
                        DouSaleDisc = Math.Round(((DouSaleRapaport - DouSalePricePerCarat) / DouSaleRapaport * 100), 2);
                        e.TotalValue = DouSaleDisc;
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
                        //DouSaleDisc = Math.Round(((DouSalePricePerCarat - DouSaleRapaport) / DouSaleRapaport * 100), 2);
                        DouExpDisc = Math.Round(((DouExpRapaport - DouExpPricePerCarat) / DouExpRapaport * 100), 2);
                        e.TotalValue = DouExpDisc;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("OFFERPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouOfferAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("OFFERRAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouOfferRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("OFFERDISCOUNT") == 0)
                    {
                        DouOfferRapaport = Math.Round(DouOfferRapaportAmt / DouCarat);
                        //DouSaleDisc = Math.Round(((DouSalePricePerCarat - DouSaleRapaport) / DouSaleRapaport * 100), 2);
                        DouOfferDisc = Math.Round(((DouOfferRapaport - DouOfferPricePerCarat) / DouOfferRapaport * 100), 2);
                        e.TotalValue = DouOfferDisc;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MEMOPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouMemoAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MEMORAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouMemoRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MEMODISCOUNT") == 0)
                    {
                        DouMemoRapaport = Math.Round(DouMemoRapaportAmt / DouCarat);
                        //DouSaleDisc = Math.Round(((DouSalePricePerCarat - DouSaleRapaport) / DouSaleRapaport * 100), 2);
                        DouMemoDisc = Math.Round(((DouMemoRapaport - DouMemoPricePerCarat) / DouMemoRapaport * 100), 2);
                        e.TotalValue = DouMemoDisc;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        public void Clear()
        {
            txtStoneCertiMFGMemo.Text = string.Empty;
            txtValue.Text = string.Empty;
            txtParty.Text = string.Empty;
            txtParty.Tag = string.Empty;
            txtBroker.Text = string.Empty;
            txtBroker.Tag = string.Empty;
            txtComment.Text = string.Empty;
            MainGrdDetail.Refresh();
        }


        private void BtnClear_Click_1(object sender, EventArgs e)
        {
            Clear();
        }

        //Added by Daksha on 12/01/2023
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

                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_BROKER);
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

        private void lblSampleExcelFile_Click(object sender, EventArgs e)
        {
            try
            {
                string StrFilePathDestination = "";

                StrFilePathDestination = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\StoneWiseLinkUpdateFormat" + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.Day.ToString() + ".xlsx";
                if (File.Exists(StrFilePathDestination))
                {
                    File.Delete(StrFilePathDestination);
                }
                File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\Format\\StoneWiseLinkUpdateFormat.xlsx", StrFilePathDestination);

                System.Diagnostics.Process.Start(StrFilePathDestination, "CMD");
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnModifyLink_Click(object sender, EventArgs e)
        {
            try
            {
                if (Val.ToString(txtBackPriceFileName.Text).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Selet File That You Want To Update...");
                    txtBackPriceFileName.Focus();
                    return;
                }

                this.Cursor = Cursors.WaitCursor;


                string StrStonePriceXml = "";

                DTabLinkUpdate.TableName = "Table1";
                using (StringWriter sw = new StringWriter())
                {
                    DTabLinkUpdate.WriteXml(sw);
                    StrStonePriceXml = sw.ToString();
                }

                DataTable DtabStockSync = ObjStock.UpdateLink(StrStonePriceXml);
                

                if (Val.ToString(DtabStockSync.Rows[0]["ReturnMessageType"]) == "SUCCESS")
                {
                    this.Cursor = Cursors.WaitCursor;
                    Global.Message(DtabStockSync.Rows[0]["ReturnMessageDesc"].ToString());
                }
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                    Global.Message("Opps..Something Wrong");
                }

                this.Cursor = Cursors.WaitCursor;


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
                    DTabLinkUpdate = Global.GetDataTableFromExcel(destinationPath, true);

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
        //End as Daksha
        private void txtStoneCertiMFGMemo_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }
     
    }
}
