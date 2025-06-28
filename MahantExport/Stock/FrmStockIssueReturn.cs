using MahantExport.Utility;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Windows.Forms;
using BusLib.Transaction;
using BusLib.TableName;

namespace MahantExport.Stock
{
    public partial class FrmStockIssueReturn : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        DataTable DtabStockDetail = new DataTable();
        DataTable DtabStockSummary = new DataTable();
        DataTable DTabSelection = new DataTable();

        LiveStockProperty mProperty = new LiveStockProperty();

        BODevGridSelection ObjGridSelection;

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

        double DouRapnetRapaport = 0;
        double DouRapnetRapaportAmt = 0;
        double DouRapnetDisc = 0;
        double DouRapnetPricePerCarat = 0;
        double DouRapnetAmount = 0;

        double DouJARapaport = 0;
        double DouJARapaportAmt = 0;
        double DouJADisc = 0;
        double DouJAPricePerCarat = 0;
        double DouJAAmount = 0;

        double DouMFGRapaport = 0;
        double DouMFGRapaportAmt = 0;
        double DouMFGDisc = 0;
        double DouMFGPricePerCarat = 0;
        double DouMFGAmount = 0;

        #region Property Settings

        public FrmStockIssueReturn()
        {
            InitializeComponent();
        }

        
        public void ShowForm() //HINA - END
        {
           
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            this.Show();
            txtStoneNo.Focus();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            //ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjStock);
        }

        #endregion

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            this.Close();
            Global.gMainLiveStock.Focus();
            Global.gMainLiveStock.Activate();
        }

        private void BtnBestFit_Click(object sender, EventArgs e)
        {
            GrdDetail.BestFitColumns();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void GrdDetail_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
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

                    DouRapnetRapaport = 0;
                    DouRapnetRapaportAmt = 0;
                    DouRapnetDisc = 0;
                    DouRapnetPricePerCarat = 0;
                    DouRapnetAmount = 0;

                    DouJARapaport = 0;
                    DouJARapaportAmt = 0;
                    DouJADisc = 0;
                    DouJAPricePerCarat = 0;
                    DouJAAmount = 0;

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

                    DouRapnetAmount = DouRapnetAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "RAPNETAMOUNT"));
                    DouRapnetRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "RAPNETRAPAPORT"));
                    DouRapnetPricePerCarat = DouRapnetAmount / DouCarat;
                    DouRapnetRapaportAmt = DouRapnetRapaportAmt + (DouRapnetRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

                    //Kuldeep 02-11-2020
                    DouJAAmount = DouJAAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "JAMESALLENAMOUNT"));
                    DouJARapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "JAMESALLENRAPAPORT"));
                    DouJAPricePerCarat = DouJAAmount / DouCarat;
                    DouJARapaportAmt = DouJARapaportAmt + (DouJARapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

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
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("RAPNETPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouRapnetAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("RAPNETRAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouRapnetRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("RAPNETDISCOUNT") == 0)
                    {
                        DouRapnetRapaport = Math.Round(DouRapnetRapaportAmt / DouCarat);
                        DouRapnetDisc = Math.Round(((DouRapnetRapaport - DouRapnetPricePerCarat) / DouRapnetRapaport * 100), 2);
                        e.TotalValue = DouRapnetDisc;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("JAMESALLENPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouJAAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("JAMESALLENRAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouJARapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("JAMESALLENDISCOUNT") == 0)
                    {
                        DouJARapaport = Math.Round(DouJARapaportAmt / DouCarat);
                        DouJADisc = Math.Round(((DouJARapaport - DouJAPricePerCarat) / DouJARapaport * 100), 2);
                        e.TotalValue = DouJADisc;
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
                        //DouCostDisc = Math.Round(((DouCostPricePerCarat - DouCostRapaport) / DouCostRapaport * 100), 2);
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

        private void GrdDetail_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                if (GrdDetail.FocusedRowHandle < 0)
                    return;

                if (Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("CHECKMARKSELECTION"))
                {
                    Int32[] selectedRowHandles = GrdDetail.GetSelectedRows();
                    if (selectedRowHandles.Length > 1)
                    {
                        for (int i = 0; i < selectedRowHandles.Length; i++)
                        {
                            GrdDetail.SetRowCellValue(selectedRowHandles[i], e.Column, e.CellValue);
                            e.Handled = true;
                        }
                    }
                }
                if (e.Clicks == 2 && Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("PARTYSTOCKNO")) //Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("STOCKNO")
                {
                    FrmStoneHistory FrmStoneHistory = new FrmStoneHistory();
                    FrmStoneHistory.MdiParent = Global.gMainRef;
                    FrmStoneHistory.ShowForm(Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "STOCK_ID")), Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "PARTYSTOCKNO")), Stock.FrmStoneHistory.FORMTYPE.DISPLAY); //Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "STOCKNO"))
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }

        }

        private void GrdSum_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
          
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            DataSet DsLiveStock = ObjStock.GetIssueReturnStockData(mProperty);
            DtabStockDetail = DsLiveStock.Tables[0];            
            DtabStockSummary = DsLiveStock.Tables[1];
           
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                GrdDetail.BeginUpdate();
                GrdSum.BeginUpdate();

                MainGrdDetail.DataSource = DtabStockDetail;
                MainGrdSum.DataSource = DtabStockSummary;

                GrdSum.ExpandAllGroups();
                GrdSum.BestFitColumns();


                MainGrdDetail.Refresh();
                MainGrdSum.Refresh();

                if (MainGrdDetail.RepositoryItems.Count == 9)
                {
                    ObjGridSelection = new BODevGridSelection();
                    ObjGridSelection.View = GrdDetail;
                    ObjGridSelection.ISBoolApplicableForPageConcept = true;
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                    GrdDetail.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;
                }
                else
                {
                    ObjGridSelection.ClearSelection();
                }
                if (ObjGridSelection != null)
                {
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                }
                progressPanel1.Visible = false;

                GrdDetail.EndUpdate();
                GrdSum.EndUpdate();

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            mProperty = new LiveStockProperty();
            mProperty.STOCKNO = Val.Trim(txtStoneNo.Text);
            mProperty.ISISSUESTOCK = (chkIssueStock.Checked) ? 1 : 0;
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
            progressPanel1.Visible = true;
            backgroundWorker1.RunWorkerAsync();

        }

        private void GrdSum_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.Clicks == 2)
            {
                if (e.Column.FieldName == "DISPLAYSTATUS")
                {
                    this.Cursor = Cursors.WaitCursor;
                    if (Val.ToString(GrdSum.GetFocusedRowCellValue("STOCKTYPE")).Contains("SINGLE"))
                    {
                        GrdDetail.Columns["STATUS"].ClearFilter();
                        GrdDetail.Columns["STATUS"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo("STATUS='" + Val.ToString(GrdSum.GetRowCellValue(e.RowHandle, "STATUS")) + "' And STOCKTYPE = 'SINGLE'");
                    }
                    else
                    {
                        // GrdDetail.Columns["STATUS"].ClearFilter();
                    }
                    this.Cursor = Cursors.Default;
                }

            }
        }
      
        private void GrdDetail_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            Global.CustomDrawColumnHeader(sender, e);
        }

        private void GrdDetail_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000")
            {
                e.DisplayText = String.Empty;
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
                }

                txtStoneNo.Text = str1;
                txtStoneNo.Select(txtStoneNo.Text.Length, 0);
                lblTotalCount.Text = "(" + txtStoneNo.Text.Split(',').Length + ")";
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTab = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);
                if (DTab.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                foreach (DataRow DRow in DTab.Rows)
                {
                    if (Val.ToInt(DRow["STOCKISSUERETURN_ID"]) != 0 && Val.ToInt(DRow["RETURNTOKENNO"]) == 0)
                    {
                        Global.Message("Some Stones Are Already Issue. Please Check.");
                        return;
                    }
                }

                if (Global.Confirm("Are You Sure To Issue Stock ?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                //FrmStockIssueReturnPopup FrmStockIssueReturnPopup = new FrmStockIssueReturnPopup();
                //FrmStockIssueReturnPopup.DTabDetail = DTab;
                //FrmStockIssueReturnPopup.ShowDialog();
                //BtnRefresh_Click(null, null);
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDetail_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }

                Int32 IntIssueNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "ISSUETOKENNO"));
                Int32 IntReturnNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "RETURNTOKENNO"));
                if (IntIssueNo != 0)
                {
                    e.Appearance.BackColor = lblIssue.BackColor;
                    e.Appearance.BackColor2 = lblIssue.BackColor;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTab = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);
                if (DTab.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                foreach (DataRow DRow in DTab.Rows)
                {
                    if (Val.ToInt(DRow["STOCKISSUERETURN_ID"]) == 0)
                    {
                        Global.Message("Please Select Only Issue Stones.");
                        return;
                    }
                    if (Val.ToInt(DRow["RETURNTOKENNO"]) != 0)
                    {
                        Global.Message("Please Select Only Issue Stones. Some Stones Are Already Return. ");
                        return;
                    }
                }

                if (Global.Confirm("Are You Sure To Return Stock ?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                string StockIssueReturnXML = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTab.WriteXml(sw);
                    StockIssueReturnXML = sw.ToString();
                }

                LiveStockProperty Property = new LiveStockProperty();
                Property = ObjStock.StockIssueReturn(Property, StockIssueReturnXML, "RETURN");

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    Global.Message(Property.ReturnMessageDesc);
                    BtnRefresh_Click(null, null);
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {

        }
    }
}
