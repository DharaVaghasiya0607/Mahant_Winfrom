using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MahantExport.Report
{
    public partial class FrmOfferPriceReport  : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        DataTable DTabOfferPrice = new DataTable();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUpload ObjMast = new BOTRN_StockUpload();
        BOFormPer ObjPer = new BOFormPer();

        double DouCarat = 0;
        double DouOfferRapaport = 0;
        double DouOfferRapaportAmt = 0;
        double DouOfferDisc = 0;
        double DouOfferPricePerCarat = 0;
        double DouOfferAmount = 0;

        public FrmOfferPriceReport()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            if (ObjPer.ISVIEW == false)
            {
                Global.MessageError(BusLib.TPV.BOMessage.ViewDeniedMsg);
                return;
            }
            Fill();
            this.Show();

            DataTable DtabOfferparty = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_OFFERPRICEPARTY);
            ChkCmbParty.Properties.DataSource = DtabOfferparty;
            ChkCmbParty.Properties.ValueMember = "OFFERPARTY_ID";
            ChkCmbParty.Properties.DisplayMember = "OFFERPARTYNAME";
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMast);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        public void Fill()
        {
            string StrParty_ID = Val.Trim(ChkCmbParty.Properties.GetCheckedItems());

            string StrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
            string StrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());


            DTabOfferPrice = ObjMast.GetDataForOfferPrice(Val.ToString(txtStockNo.Text), StrFromDate, StrToDate, StrParty_ID);
            MainGrid.DataSource = DTabOfferPrice;
            MainGrid.Refresh();
        }

        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            Fill();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GrdBox_CellMerge(object sender, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs e)
        {
            string MergeOnStr = "OFFERPRICE_ID,STOCK_ID,OFFERPARTY_ID,STOCKNO,OFFERRAPAPORT,OFFERDISCOUNT,OFFERPRICEPERCARAT,OFFERAMOUNT,OFFERCOMMENT";
            string MergeOn = "PARTYNAME";

            if (MergeOnStr.Contains(e.Column.FieldName))
            {
                string val1 = Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle1, GrdDetail.Columns[MergeOn]));
                string val2 = Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle2, GrdDetail.Columns[MergeOn]));
                if (val1 == val2)
                    e.Merge = true;
                e.Handled = true;
            }
        }

        private void txtStockNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String str1 = txtStockNo.Text.Trim().Replace("\r\n", ",");
                txtStockNo.Text = str1;
                txtStockNo.Select(txtStockNo.Text.Length, 0);

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDetail_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            if (e.SummaryProcess == CustomSummaryProcess.Start)
            {
                DouCarat = 0;
                DouOfferRapaport = 0;
                DouOfferRapaportAmt = 0;
                DouOfferDisc = 0;
                DouOfferPricePerCarat = 0;
                DouOfferAmount = 0;
            }
            else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
            {
                DouCarat = DouCarat + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT"));
                DouOfferAmount = DouOfferAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "OFFERAMOUNT"));
                DouOfferRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "OFFERRAPAPORT"));
                DouOfferPricePerCarat = DouOfferAmount / DouCarat;
                DouOfferRapaportAmt = DouOfferRapaportAmt + (DouOfferRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));
            }
            else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
            {
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
                    DouOfferDisc = Math.Round(((DouOfferRapaport - DouOfferPricePerCarat) / DouOfferRapaport * 100), 2);
                    e.TotalValue = DouOfferDisc;
                }

            }
        }

        //Added by Daksha on 23/01/2023
        private void RepBtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.Confirm("Are You Sure to Delete this Record ?") == DialogResult.Yes)
                {
                    this.Cursor = Cursors.WaitCursor;                    
                    PriceHeadDetailProperty objProperty = new PriceHeadDetailProperty();
                    objProperty.PRICE_ID = Val.ToGuid(GrdDetail.GetFocusedRowCellValue("OFFERPRICE_ID"));
                    ObjMast.OfferPrice_Delete(objProperty);
                    Global.Message(objProperty.ReturnMessageDesc);
                    if (objProperty.ReturnMessageType == "SUCCESS")
                    {
                        btnSearch_Click(null, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("Offer Price Report", GrdDetail);
        }        
        //End as Daksha
    }
}

