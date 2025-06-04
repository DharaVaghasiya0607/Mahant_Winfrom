
using MahantExport.Utility;
using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using Google.API.Translate;
using OfficeOpenXml;
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
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MahantExport.Pricing
{
    public partial class FrmPricingViewNew : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_ParcelParameterDiscount ObjTrn = new BOTRN_ParcelParameterDiscount();
        BOFormPer ObjPer = new BOFormPer();
        BODevGridSelection ObjGridSelection;

        DataTable DtabPara = new DataTable();
        DataTable DtabPriceView = new DataTable();

        string MergeOn = string.Empty;
        string MergeOnStr = string.Empty;

        public FrmPricingViewNew()
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

            CmbCut.Properties.DataSource = DtabPara.Select("PARATYPE = 'CUT'").CopyToDataTable();
            CmbCut.Properties.ValueMember = "PARA_ID";
            CmbCut.Properties.DisplayMember = "SHORTNAME";

            cmbPol.Properties.DataSource = DtabPara.Select("PARATYPE = 'POLISH'").CopyToDataTable();
            cmbPol.Properties.ValueMember = "PARA_ID";
            cmbPol.Properties.DisplayMember = "SHORTNAME";

            CmbSym.Properties.DataSource = DtabPara.Select("PARATYPE = 'SYMMETRY'").CopyToDataTable();
            CmbSym.Properties.ValueMember = "PARA_ID";
            CmbSym.Properties.DisplayMember = "SHORTNAME";

            CmbFL.Properties.DataSource = DtabPara.Select("PARATYPE = 'FLUORESCENCE'").CopyToDataTable();
            CmbFL.Properties.ValueMember = "PARA_ID";
            CmbFL.Properties.DisplayMember = "SHORTNAME";

            cmbLab.Properties.DataSource = DtabPara.Select("PARATYPE = 'LAB'").CopyToDataTable();
            cmbLab.Properties.ValueMember = "PARA_ID";
            cmbLab.Properties.DisplayMember = "SHORTNAME";

            DataTable DTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PARAALL);
            DataRow[] DR = DTab.Select("PARATYPE='WEBSTATUS'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "PARANAME";
                CmbWebStatus.Properties.DataSource = DTTemp.DefaultView.ToTable();
                CmbWebStatus.Properties.ValueMember = "SHORTNAME";
                CmbWebStatus.Properties.DisplayMember = "SHORTNAME";
            }

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
            ObjFormEvent.ObjToDisposeList.Add(ObjTrn);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                LiveStockProperty LStockProperty = new LiveStockProperty();

                LStockProperty.MULTYSHAPE_ID = cmbShape.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYCOLOR_ID = cmbColor.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYCLARITY_ID = cmbClarity.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYCUT_ID = CmbCut.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYPOL_ID = cmbPol.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYSYM_ID = CmbSym.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYFL_ID = CmbFL.EditValue.ToString().Replace(" ", "");
                LStockProperty.MULTYLAB_ID = cmbLab.EditValue.ToString().Replace(" ", "");
                LStockProperty.WEBSTATUS = CmbWebStatus.EditValue.ToString().Replace(" ", null);

                LStockProperty.STOCKNO = Val.ToString(txtStoneNo.Text);
                LStockProperty.LABREPORTNO = Val.ToString(txtreportno.Text);

                if (DtpAvailbleFromDate.InvokeRequired)
                {
                    DtpAvailbleFromDate.Invoke(new MethodInvoker(() =>
                    {
                        if (DtpAvailbleFromDate.Checked == true)
                        {
                            LStockProperty.AVAILBLEFROMDATE = Val.SqlDate(DtpAvailbleFromDate.Value.ToShortDateString());
                        }
                    }));
                }
                else
                {
                    if (DtpAvailbleFromDate.Checked == true)
                    {
                        LStockProperty.AVAILBLEFROMDATE = Val.SqlDate(DtpAvailbleFromDate.Value.ToShortDateString());
                    }
                }

                if (DtpAvailbleToDate.InvokeRequired)
                {
                    DtpAvailbleToDate.Invoke(new MethodInvoker(() =>
                    {
                        if (DtpAvailbleToDate.Checked == true)
                        {
                            LStockProperty.AVAILBLETODATE = Val.SqlDate(DtpAvailbleToDate.Value.ToShortDateString());
                        }
                    }));
                }
                else
                {
                    if (DtpAvailbleToDate.Checked == true)
                    {
                        LStockProperty.AVAILBLETODATE = Val.SqlDate(DtpAvailbleToDate.Value.ToShortDateString());
                    }
                }

                if (DtpSalesFromDate.InvokeRequired)
                {
                    DtpSalesFromDate.Invoke(new MethodInvoker(() =>
                    {
                        if (DtpSalesFromDate.Checked == true)
                        {
                            LStockProperty.SALESFROMDATE = Val.SqlDate(DtpSalesFromDate.Value.ToShortDateString());
                        }
                    }));
                }
                else
                {
                    if (DtpSalesFromDate.Checked == true)
                    {
                        LStockProperty.SALESFROMDATE = Val.SqlDate(DtpSalesFromDate.Value.ToShortDateString());
                    }
                }


                if (DtpSalesToDate.InvokeRequired)
                {
                    DtpSalesToDate.Invoke(new MethodInvoker(() =>
                    {
                        if (DtpSalesToDate.Checked == true)
                        {
                            LStockProperty.SALESTODATE = Val.SqlDate(DtpSalesToDate.Value.ToShortDateString());
                        }
                    }));
                }
                else
                {
                    if (DtpSalesToDate.Checked == true)
                    {
                        LStockProperty.SALESTODATE = Val.SqlDate(DtpSalesToDate.Value.ToShortDateString());
                    }
                }

                DtabPriceView = ObjTrn.GetPricePriceViewGetData(LStockProperty);

                if (DtabPriceView.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }
                if (MainGrid.InvokeRequired)
                {
                    MainGrid.Invoke(new MethodInvoker(() =>
                    {
                        MainGrid.DataSource = DtabPriceView;
                        GrdDet.BestFitColumns();
                    }));
                }
                else
                {
                    MainGrid.DataSource = DtabPriceView;
                    GrdDet.BestFitColumns();
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }


        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            progressPanel1.Visible = false;
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            try
            {
                if (backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.CancelAsync();
                }
                else
                {
                    progressPanel1.Visible = true;
                    backgroundWorker1.RunWorkerAsync();

                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDet_CellMerge(object sender, CellMergeEventArgs e)
        {
            try
            {
                MergeOnStr = "SrNo,SHAPENAME,COLORNAME,CLARITYNAME,CUTNAME,POLNAME,SYMNAME,FLNAME,LOCATIONNAME,SIZENAME,LABNAME,LABREPORTNO,CARAT,TABLEPER,DEPTHPER,MEASUREMENT,COSTRAPAPORT,COSTDISCOUNT,COSTPRICEPERCARAT,COSTAMOUNT,LIVERAP";
                MergeOn = "PARTYSTOCKNO";
                if (MergeOnStr.Contains(e.Column.FieldName))
                {
                    string val1 = Val.ToString(GrdDet.GetRowCellValue(e.RowHandle1, GrdDet.Columns[MergeOn]));
                    string val2 = Val.ToString(GrdDet.GetRowCellValue(e.RowHandle2, GrdDet.Columns[MergeOn]));
                    if (val1 == val2)
                        e.Merge = true;
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDet_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {

                if (e.RowHandle < 0)
                {
                    return;
                }
                DataRow DRow = GrdDet.GetDataRow(e.RowHandle);

                //if (MergeOnStr.Contains(e.Column.FieldName))
                //{
                //    string val1 = Val.ToString(GrdDet.GetRowCellValue(e.RowHandle, GrdDet.Columns[MergeOn]));
                //    for (int i = 0; i < GrdDet.RowCount; i++)
                //    {
                //        string val2 = Val.ToString(GrdDet.GetRowCellValue(i, GrdDet.Columns[MergeOn]));
                //        if (val1 == val2)
                //        {
                //            GrdDet.SetRowCellValue(i, e.Column, e.Value); // Update value for all merged rows
                //        }
                //    }
                //}

                switch (e.Column.FieldName)
                {                    
                    case "DISCOUNT":

                        double Rapaport = Val.Val(DRow["LIVERAP"]);
                        double Carat = Val.Val(DRow["CARAT"]);

                        DtabPriceView.Rows[e.RowHandle]["LIVERAP"] = Rapaport;
                        DtabPriceView.Rows[e.RowHandle]["PRICEPERCARAT"] = Math.Round(Rapaport + ((Rapaport * Val.Val(DRow["DISCOUNT"])) / 100));
                        DtabPriceView.Rows[e.RowHandle]["AMOUNT"] = Math.Round(Carat * Val.Val(DtabPriceView.Rows[e.RowHandle]["PRICEPERCARAT"]), 2);
                        DtabPriceView.AcceptChanges();
                        break;

                    case "PRICEPERCARAT":

                        Rapaport = Val.Val(DRow["LIVERAP"]);
                        double PricePerCarat = Val.Val(DRow["PRICEPERCARAT"]);
                        Carat = Val.Val(DRow["CARAT"]);
                        double DouPer = 0;
                        if (Rapaport != 0)
                        {
                            DouPer = Math.Round(((Rapaport - PricePerCarat) / Rapaport) * 100, 2);
                        }
                        else
                            DouPer = 0;

                        DtabPriceView.Rows[e.RowHandle]["LIVERAP"] = Rapaport;
                        DtabPriceView.Rows[e.RowHandle]["DISCOUNT"] = DouPer;
                        DtabPriceView.Rows[e.RowHandle]["AMOUNT"] = Math.Round(Carat * Val.Val(DtabPriceView.Rows[e.RowHandle]["PRICEPERCARAT"]), 2);
                        DtabPriceView.AcceptChanges();
                        break;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }

        }

        private void GrdDet_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
        {
            try
            {
                //if (MergeOnStr.Contains(e.Column.FieldName))
                //{
                //    string val1 = Val.ToString(GrdDet.GetRowCellValue(e.RowHandle, GrdDet.Columns[MergeOn]));
                //    if (!string.IsNullOrEmpty(val1))
                //    {
                //        e.RepositoryItem = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
                //    }
                //}
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void cLabel9_Click(object sender, EventArgs e)
        {
            try
            {
                if (DtabPriceView != null && DtabPriceView.Rows.Count > 0)
                {
                    // Iterate through the rows in the DataTable
                    foreach (DataRow row in DtabPriceView.Rows)
                    {
                        // Check if the CATEGORY column value is 'SALE'
                        if (row["CATEGORY"].ToString().Equals("SALE", StringComparison.OrdinalIgnoreCase))
                        {
                            // Update the SUGGESTEDDISCOUNT column value
                            row["DISCOUNT"] = row["SUGGESTEDDISCOUNT"];
                            row["PRICEPERCARAT"] = row["SUGGESTEDPRICEPERCARAT"];
                            row["AMOUNT"] = row["SUGGESTEDAMOUNT"];

                        }
                    }

                    // Optionally, refresh the bound grid if applicable
                    MainGrid.DataSource = DtabPriceView; // Replace with your actual grid control reference
                    MainGrid.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void cLabel14_Click(object sender, EventArgs e)
        {
            try
            {
                if (DtabPriceView != null && DtabPriceView.Rows.Count > 0)
                {
                    // Iterate through the rows in the DataTable
                    foreach (DataRow row in DtabPriceView.Rows)
                    {
                        // Check if the CATEGORY column value is 'SALE'
                        if (row["CATEGORY"].ToString().Equals("AVAILABLE", StringComparison.OrdinalIgnoreCase))
                        {
                            // Update the SUGGESTEDDISCOUNT column value
                            row["DISCOUNT"] = row["SUGGESTEDDISCOUNT"];
                            row["PRICEPERCARAT"] = row["SUGGESTEDPRICEPERCARAT"];
                            row["AMOUNT"] = row["SUGGESTEDAMOUNT"];

                        }
                    }

                    // Optionally, refresh the bound grid if applicable
                    MainGrid.DataSource = DtabPriceView; // Replace with your actual grid control reference
                    MainGrid.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDet_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0 || e.Clicks < 2)
                    return;

                var focusedColumn = e.Column;
                string bandName = "";

                // Get the band name of the focused column
                if (focusedColumn != null)
                {
                    // Assuming GrdDet is a BandedGridView
                    var band = GrdDet.Bands.FirstOrDefault(b => b.Columns.Contains((DevExpress.XtraGrid.Views.BandedGrid.BandedGridColumn)focusedColumn));

                    if (band != null)
                    {
                        bandName = band.Name; // Get the band name
                    }
                }

                if (bandName == "Analysis" && e.Column.FieldName != "CATEGORY")
                {
                    LiveStockProperty LStockProperty = new LiveStockProperty();

                    LStockProperty.SHAPE_ID = Val.ToInt32(GrdDet.GetFocusedRowCellValue("Shape_ID"));
                    LStockProperty.COLOR_ID = Val.ToInt32(GrdDet.GetFocusedRowCellValue("Color_ID"));
                    LStockProperty.CLARITY_ID = Val.ToInt32(GrdDet.GetFocusedRowCellValue("Clarity_ID"));
                    LStockProperty.CUT_ID = Val.ToInt32(GrdDet.GetFocusedRowCellValue("Cut_ID"));
                    LStockProperty.POL_ID = Val.ToInt32(GrdDet.GetFocusedRowCellValue("Pol_ID"));
                    LStockProperty.SYM_ID = Val.ToInt32(GrdDet.GetFocusedRowCellValue("Sym_ID"));
                    LStockProperty.WEBSTATUS = Val.ToString(GrdDet.GetFocusedRowCellValue("CATEGORY"));

                    LStockProperty.CARAT = Val.ToDouble(GrdDet.GetFocusedRowCellValue("CARAT"));
                    string Str = Val.ToString(GrdDet.GetFocusedRowCellValue(e.Column.FieldName));

                    string cleanedStr = Regex.Replace(Str, @"\(\d+\)", "").Trim();
                    double.TryParse(cleanedStr, out double numericValue);
                    LStockProperty.SALEDISCOUNT = numericValue;

                    DataTable DTabMemo = ObjTrn.GetDataForPriceDetail(LStockProperty);

                    if (DTabMemo.Rows.Count > 0)
                    {
                        FrmPopupGrid FrmPopupGrid = new FrmPopupGrid();

                        FrmPopupGrid.MainGrid.DataSource = DTabMemo;
                        FrmPopupGrid.GrdDet.BestFitColumns();
                        FrmPopupGrid.MainGrid.Dock = DockStyle.Fill;
                        FrmPopupGrid.Text = LStockProperty.WEBSTATUS;
                        FrmPopupGrid.LblTitle.Text = "List Of Discount Details";
                        FrmPopupGrid.ISPostBack = true;

                        FrmPopupGrid.Width = 1000;
                        FrmPopupGrid.GrdDet.OptionsBehavior.Editable = false;

                        FrmPopupGrid.GrdDet.Columns["SrNo"].Summary.Add(new DevExpress.XtraGrid.GridColumnSummaryItem
                        {
                            SummaryType = DevExpress.Data.SummaryItemType.Count, // For example, count the number of items
                            DisplayFormat = "Total: {0}" // Display format for the summary
                        });

                        FrmPopupGrid.ShowDialog();

                        FrmPopupGrid.Hide();
                        FrmPopupGrid.Dispose();
                        FrmPopupGrid = null;
                    }
                    else
                    {
                        Global.Message("No Data Found !!");
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
    }
}