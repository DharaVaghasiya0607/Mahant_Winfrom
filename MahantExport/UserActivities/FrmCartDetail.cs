using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BusLib.Configuration;
using BusLib.Transaction;
using DevExpress.Data;

namespace MahantExport.UserActivities
{
    public partial class FrmCartDetail : DevControlLib.cDevXtraForm
    {

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_Basket ObjCart= new BOTRN_Basket();
        DataTable DtabCartDetail = new DataTable();
        DataTable DtabCartSummary = new DataTable();
        DataTable DTab = new DataTable();

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


        public FrmCartDetail()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            BtnShow.Enabled = ObjPer.ISVIEW;

            this.Show();
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
            ObjFormEvent.ObjToDisposeList.Add(ObjCart);
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            //if (Val.SqlDate(DTPFromDate.Text) >= Val.SqlDate(DTPToDate.Text))
            //{
            //    Global.Message("From Date must be Less Than To Date");
            //    return;
            //}

            DataSet DS = ObjCart.GetCartDetailData(Val.SqlDate(DTPFromDate.Text), Val.SqlDate(DTPToDate.Text));

            DtabCartDetail = DS.Tables[0];
            DtabCartSummary = DS.Tables[1];

            MainGrdSum.DataSource = DtabCartSummary;

            //MainGrid.DataSource = DtabCart;
            //MainGrid.Refresh();
        }

        private void GrdSum_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                if (e.Clicks == 2)
                {

                    DataRow Dr = GrdSum.GetFocusedDataRow();
                     DTab = DtabCartDetail.Select("PARTYNAME ='" + Val.ToString(Dr["PARTYNAME"]) + "'").CopyToDataTable();

                     if (DTab.Rows.Count <= 0)
                     {
                         Global.Message("No Data  Found");
                         return;
                     }
                         MainGrdDetail.DataSource = DTab;
                     
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (DTab.Rows.Count <= 0)
            {
                Global.Message("No Data  Found");
                return;
            }
             Global.ExcelExport("Cart Details", GrdDetail);
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
                        DouCostRapaport = Math.Round(DouCostRapaportAmt / DouCarat);
                        DouCostDisc = Math.Round(((DouCostPricePerCarat - DouCostRapaport) / DouCostRapaport * 100), 2);
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
                        DouSaleDisc = Math.Round(((DouSalePricePerCarat - DouSaleRapaport) / DouSaleRapaport * 100), 2);
                        e.TotalValue = DouSaleDisc;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void DTPFromDate_ValueChanged(object sender, EventArgs e)
        {
            DTPFromDate.MaxDate = DateTime.Parse(DTPToDate.Text);
        }

    }
}