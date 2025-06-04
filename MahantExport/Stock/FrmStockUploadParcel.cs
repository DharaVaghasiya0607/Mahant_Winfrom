using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using DevExpress.XtraPrinting;
using Google.API.Translate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OfficeOpenXml;
using Spire.Xls;
using DevExpress.Data;
using DevExpress.XtraPrintingLinks;
using System.Drawing.Printing;
using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using BusLib.Transaction;
using MahantExport.Utility;
using DevExpress.XtraGrid.Views.Grid;

namespace MahantExport.Stock
{
    public partial class FrmStockUploadParcel : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUploadParcel ObjStock = new BOTRN_StockUploadParcel();
        DataTable DtabStockUpload = new DataTable();
        DataTable DtabExcelData = new DataTable();

        DataTable DtabFinalData = new DataTable();
        DataTable DtabPara = new DataTable();
        DataTable DtabSupplier = new DataTable();
        DataTable DtabExcRateCurrencyWise = new DataTable();
        DataTable DtabStockSync = new DataTable();
        DataTable DtabStockDateWiseSummary = new DataTable();

        BODevGridSelection ObjGridSelection;

        Guid mTrn_ID;
        Int64 IntMainParty_ID = 0;
        string mStrOpe = "";
        string StrUploadFilename = "";

        double DouCarat = 0;
        double DouFileRap = 0;
        double DouFileRapAmount = 0;
        double DouFileBack = 0;
        double DouFilePricePerCarat = 0;
        double DouFileAmount = 0;

        double DouCompRap = 0;
        double DouCompRapAmount = 0;
        double DouCompBack = 0;
        double DouCompPricePerCarat = 0;
        double DouCompAmount = 0;

        string mStrStockType = "";

        #region Property Settings

        public FrmStockUploadParcel()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            mStrOpe = "";
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            mTrn_ID = BusLib.Configuration.BOConfiguration.FindNewSequentialID();

            //DtabPara = new BOMST_Parameter().GetParameterDataParcel();
            //DtabExcRateCurrencyWise = new BOComboFill().FillCmb(BusLib.BOComboFill.TABLE.MST_CURRENTEXCRATECURRENCYWISE);



            mStrStockType = "";

            CmbCurrency.DataSource = new BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CURRENCY);
            CmbCurrency.DisplayMember = "CURRENCYNAME";
            CmbCurrency.ValueMember = "CURRENCY_ID";
            CmbCurrency.SelectedIndex = -1;

            Clear();
            Fill();


            this.Show();

        }
        public void ShowForm(string StrStockType)
        {
            mStrOpe = "";
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            mTrn_ID = BusLib.Configuration.BOConfiguration.FindNewSequentialID();

            //DtabPara = new BOMST_Parameter().GetParameterDataParcel();
            //DtabExcRateCurrencyWise = new BOComboFill().FillCmb(BusLib.BOComboFill.TABLE.MST_CURRENTEXCRATECURRENCYWISE);

            mStrStockType = StrStockType;

            CmbCurrency.DataSource = new BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CURRENCY);
            CmbCurrency.DisplayMember = "CURRENCYNAME";
            CmbCurrency.ValueMember = "CURRENCY_ID";
            CmbCurrency.SelectedIndex = -1;

            this.Text = mStrStockType + " " + this.Text;

            Clear();
            Fill();


            this.Show();

        }
        public void ShowForm(Guid GTrn_ID, Int64 Party_ID)
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            Clear();
            mStrOpe = "DISPLAY";
            mTrn_ID = GTrn_ID;
            IntMainParty_ID = Party_ID;
            Fill();
            GrdDetStock.BestFitColumns();
            DtabPara = new BOMST_Parameter().GetParameterDataParcel();
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
        }

        #endregion


        #region Validation

        private bool ValSave()
        {

            if (Val.ToString(txtPartyName.Text).Trim().Equals(string.Empty) && mStrStockType == "SINGLE")
            {
                Global.Message("Party Is Required");
                txtPartyName.Focus();
                return true;
            }
            else if (Val.ToString(txtFileName.Text).Trim().Equals(string.Empty))
            {
                Global.Message("Please Select  File.");
                BtnBrowse.Focus();
                return true;
            }
            //else if (Val.ToString(CmbSheetname.Text).Trim().Equals(string.Empty))
            //{
            //    Global.Message("Please Select Sheet.");
            //    CmbSheetname.Focus();
            //    return true;
            //}

            return false;
        }


        private bool ValDelete()
        {
            //if (txtItemGroupCode.Text.Trim().Length == 0)
            //{
            //    Global.Message("Group Code Is Required");
            //    txtItemGroupCode.Focus();
            //    return false;
            //}

            return true;
        }

        #endregion

        public void Clear()
        {
            mStrOpe = "";

            DtabSupplier = new BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PURCHASEPARTY);
            DtabPara = new BOMST_Parameter().GetParameterDataParcel();
            DtabExcRateCurrencyWise = new BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CURRENTEXCRATECURRENCYWISE);

            txtPartyName.Tag = string.Empty;
            txtPartyName.Text = string.Empty;
            txtFileName.Text = string.Empty;
            //CmbSheetname.Text = string.Empty;
            CmbSheetname.SelectedIndex = -1;
            RbtReplaceAllStock.Checked = true;
            mTrn_ID = Guid.Empty;

            if (!Val.ToString(mStrStockType).Trim().Equals(string.Empty))
            {
                CmbStockType.Enabled = false;
                CmbStockType.SelectedItem = mStrStockType;
            }
            else
            {
                CmbStockType.Enabled = true;
                CmbStockType.SelectedIndex = 0;
            }


            if (mStrStockType == "PARCEL")
            {
                GrdDetStock.Columns["POLNAME"].Visible = false;
                GrdDetStock.Columns["SYMNAME"].Visible = false;
                GrdDetStock.Columns["FLNAME"].Visible = true;
                GrdDetStock.Columns["LABREPORTNO"].Visible = false;
                GrdDetStock.Bands["BANDOTHERPARAM"].Visible = false;
                GrdDetStock.Columns["COSTRAPAPORT"].Visible = false;
                GrdDetStock.Columns["COSTDISCOUNT"].Visible = false;
            }
            else
            {
                GrdDetStock.Columns["POLNAME"].Visible = true;
                GrdDetStock.Columns["SYMNAME"].Visible = true;
                GrdDetStock.Columns["FLNAME"].Visible = true;
                GrdDetStock.Columns["LABREPORTNO"].Visible = true;
                GrdDetStock.Bands["BANDOTHERPARAM"].Visible = true;
                GrdDetStock.Columns["COSTRAPAPORT"].Visible = true;
                GrdDetStock.Columns["COSTDISCOUNT"].Visible = true;
            }


            // CmbStockStatus.SelectedIndex = 0;
            //CmbStockType.SelectedIndex = 0;
            CmbCurrency.SelectedIndex = 0;
            CmbStockStatus.SelectedIndex = 0;
            CmbStockStatus.Enabled = false;
            RbtAppendStock.Checked = true;

            lblMessage.Text = "Message";
            lblMessage.Visible = false;

            Fill();
            BtnCalculate.Enabled = true;

            CmbCurrency.SelectedValue = "1";

            //GrpExcRateDetail.Visible = false;

            txtPartyName.Focus();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            Clear();
        }



        public void Fill()
        {
            Guid gParty_ID;
            gParty_ID = Val.ToString(txtPartyName.Tag).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtPartyName.Tag));

            DtabStockUpload = ObjStock.GetStockUploadData(gParty_ID, "WITHOUTSTOCK");

            DtabFinalData = DtabStockUpload.Clone();

            if (DtabFinalData.Columns.Contains("ISNOTCONSIDERINEXPORT"))
                DtabFinalData.Columns.Remove("ISNOTCONSIDERINEXPORT");

            if (DtabStockUpload.Rows.Count > 0)
            {
                txtPartyName.Tag = DtabStockUpload.Rows[0]["SELLER_ID"];
                txtPartyName.Text = Val.ToString(DtabStockUpload.Rows[0]["SELLERNAME"]);
                txtFileName.Text = Val.ToString(DtabStockUpload.Rows[0]["UPLOADFILENAME"]);

                BtnCalculate.Enabled = false;
            }

            MainGrdStock.DataSource = DtabStockUpload;
            MainGrdStock.Refresh();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdDetStock.RowCount <= 0)
                    return;

                if (Global.Confirm("You Want that Stone's Also Which Is Not Consider In Export ?") == System.Windows.Forms.DialogResult.No)
                {
                    //DataView filteredDataView = new DataView(GrdDetPricing.GridControl.DataSource as DataTable);
                    ////filteredDataView.RowFilter = DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetDataSetWhere(GrdDetPricing.ActiveFilterCriteria.LegacyToString()); //original
                    //string filter = DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetDataSetWhere(GrdDetPricing.ActiveFilterCriteria);
                    //if(filter.Contains(Val.ToString("[Carat]").ToUpper()))
                    //    filter = filter.Replace(Val.ToString("[CARAT]").ToUpper(),"Convert([CARAT],'System.String')");
                    //filteredDataView.RowFilter = filter;
                    //CriteriaOperator currentFilter = CriteriaOperator.Clone(GrdDetPricing.ActiveFilterCriteria);
                    ////string expression = new ExtendedDataSetWhereGenerator().Process(currentFilter);
                    ////Text = expression;
                    //DataTable dt = filteredDataView.ToTable();
                    //MainGrdPricing.DataSource = dt.Select("ISNOTCONSIDERINEXPORT = 0").CopyToDataTable();
                    //IsNotConsiderExportStoneFlag = true;

                    CriteriaOperator currentFilter = CriteriaOperator.Clone(GrdDetStock.ActiveFilterCriteria);
                    GrdDetStock.ActiveFilter.Add(GrdDetStock.Columns["ISNOTCONSIDERINEXPORT"], new ColumnFilterInfo("[ISNOTCONSIDERINEXPORT] = 0"));
                    string STR = GrdDetStock.ActiveFilterCriteria.ToString();
                }
                else
                {
                    GrdDetStock.Columns["ISNOTCONSIDERINEXPORT"].ClearFilter();
                }

                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = ".xlsx";
                svDialog.Title = "Export to Excel";
                svDialog.FileName = txtPartyName.Text.Trim() + "_Listing";
                svDialog.Filter = "Excel files 97-2003 (*.xls)|*.xls|Excel files 2007(*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    GrdDetStock.OptionsPrint.PrintBandHeader = false;
                    GrdDetStock.Columns["WEBSITE"].Visible = false;
                    GrdDetStock.Columns["RATIO"].Visible = false;
                    GrdDetStock.Columns["GIRDLEDESC"].Visible = false;
                    GrdDetStock.Columns["HA"].Visible = false;
                    GrdDetStock.Columns["TABLEINC"].Visible = false;
                    GrdDetStock.Columns["SIDEINC"].Visible = false;
                    GrdDetStock.Columns["TABLEBLACKINC"].Visible = false;
                    GrdDetStock.Columns["SIDEBLACKINC"].Visible = false;
                    GrdDetStock.Columns["SIDEOPENINC"].Visible = false;
                    GrdDetStock.Columns["TABLEOPENINC"].Visible = false;
                    GrdDetStock.Columns["EXTRAFACET"].Visible = false;
                    GrdDetStock.Columns["LUSTER"].Visible = false;
                    GrdDetStock.Columns["COMMENT"].Visible = false;
                    GrdDetStock.Columns["GIRDLEPER"].Visible = false;
                    GrdDetStock.Columns["GRAINING"].Visible = false;
                    GrdDetStock.Columns["LOWERHALF"].Visible = false;
                    GrdDetStock.Columns["STARLENGTH"].Visible = false;
                    GrdDetStock.Columns["FILERAPAMT"].Visible = false;

                    GrdDetStock.Columns["DEPTH"].Visible = false;
                    GrdDetStock.Columns["TABLE1"].Visible = false;
                    GrdDetStock.Columns["CRANGLE"].Visible = false;
                    GrdDetStock.Columns["PAVANGLE"].Visible = false;
                    GrdDetStock.Columns["CRHEIGHT"].Visible = false;
                    GrdDetStock.Columns["PAVHEIGHT"].Visible = false;
                    GrdDetStock.Columns["KEYTOSYMBOL"].Visible = false;
                    GrdDetStock.Columns["HEIGHT"].Visible = false;
                    GrdDetStock.Columns["REPORTCOMMENT"].Visible = false;
                    GrdDetStock.Columns["KEYTOSYMBOL"].Visible = false;

                    GrdDetStock.Bands["BANDCOMPCALC"].Visible = false;
                    GrdDetStock.Bands["BANDDIFF"].Visible = false;
                    GrdDetStock.Bands["BANDSTATUS"].Visible = false;
                    GrdDetStock.Bands["BANDSNOTEXPORTED"].Visible = false;

                    PrintableComponentLinkBase link = new PrintableComponentLinkBase()
                    {
                        PrintingSystemBase = new PrintingSystemBase(),
                        Component = MainGrdStock,
                        //Landscape = true,
                        PaperKind = PaperKind.A4,
                        Margins = new System.Drawing.Printing.Margins(20, 25, 20, 20)
                    };

                    //link.CreateReportHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);

                    XlsExportOptions Option = new XlsExportOptions();
                    Option.Suppress256ColumnsWarning = true;
                    Option.Suppress65536RowsWarning = true;

                    DevExpress.XtraPrinting.XlsExportOptions a = new DevExpress.XtraPrinting.XlsExportOptions();
                    a.Suppress256ColumnsWarning = true;
                    a.Suppress65536RowsWarning = true;

                    link.ExportToXls(svDialog.FileName, a);

                    GrdDetStock.Columns["WEBSITE"].Visible = true;
                    GrdDetStock.Columns["RATIO"].Visible = true;
                    GrdDetStock.Columns["GIRDLEDESC"].Visible = true;
                    GrdDetStock.Columns["HA"].Visible = true;
                    GrdDetStock.Columns["TABLEINC"].Visible = true;
                    GrdDetStock.Columns["SIDEINC"].Visible = true;
                    GrdDetStock.Columns["TABLEBLACKINC"].Visible = true;
                    GrdDetStock.Columns["SIDEBLACKINC"].Visible = true;
                    GrdDetStock.Columns["SIDEOPENINC"].Visible = true;
                    GrdDetStock.Columns["TABLEOPENINC"].Visible = true;
                    GrdDetStock.Columns["EXTRAFACET"].Visible = true;
                    GrdDetStock.Columns["LUSTER"].Visible = true;
                    GrdDetStock.Columns["COMMENT"].Visible = true;
                    GrdDetStock.Columns["GIRDLEPER"].Visible = true;
                    GrdDetStock.Columns["GRAINING"].Visible = true;
                    GrdDetStock.Columns["LOWERHALF"].Visible = true;
                    GrdDetStock.Columns["STARLENGTH"].Visible = true;
                    GrdDetStock.Columns["FILERAPAMT"].Visible = true;

                    GrdDetStock.Columns["DEPTH"].Visible = true;
                    GrdDetStock.Columns["TABLE1"].Visible = true;
                    GrdDetStock.Columns["CRANGLE"].Visible = true;
                    GrdDetStock.Columns["PAVANGLE"].Visible = true;
                    GrdDetStock.Columns["CRHEIGHT"].Visible = true;
                    GrdDetStock.Columns["PAVHEIGHT"].Visible = true;
                    GrdDetStock.Columns["KEYTOSYMBOL"].Visible = true;
                    GrdDetStock.Columns["HEIGHT"].Visible = true;
                    GrdDetStock.Columns["REPORTCOMMENT"].Visible = true;
                    GrdDetStock.Columns["KEYTOSYMBOL"].Visible = true;
                    GrdDetStock.Columns["STATUS"].Visible = true;


                    GrdDetStock.Bands["BANDCOMPCALC"].Visible = true;
                    GrdDetStock.Bands["BANDDIFF"].Visible = true;
                    GrdDetStock.Bands["BANDSTATUS"].Visible = true;
                    GrdDetStock.Bands["BANDSNOTEXPORTED"].Visible = true;


                    if (Global.Confirm("Do You Want To Open [" + svDialog.FileName + ".xlsx] ?") == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(svDialog.FileName, "CMD");
                    }
                }
                GrdDetStock.Columns["ISNOTCONSIDERINEXPORT"].ClearFilter();

                svDialog.Dispose();
                svDialog = null;

            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
            }
        }



        private void BtnExport2_Click(object sender, EventArgs e)
        {
            if (GrdDetStock.RowCount <= 0)
                return;

            try
            {


                if (Global.Confirm("You Want that Stone's Also Which Is Not Consider In Export ?") == System.Windows.Forms.DialogResult.No)
                {
                    //DataView filteredDataView = new DataView(GrdDetPricing.GridControl.DataSource as DataTable);
                    ////filteredDataView.RowFilter = DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetDataSetWhere(GrdDetPricing.ActiveFilterCriteria.LegacyToString()); //original
                    //string filter = DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetDataSetWhere(GrdDetPricing.ActiveFilterCriteria);
                    //if(filter.Contains(Val.ToString("[Carat]").ToUpper()))
                    //    filter = filter.Replace(Val.ToString("[CARAT]").ToUpper(),"Convert([CARAT],'System.String')");
                    //filteredDataView.RowFilter = filter;
                    //CriteriaOperator currentFilter = CriteriaOperator.Clone(GrdDetPricing.ActiveFilterCriteria);
                    ////string expression = new ExtendedDataSetWhereGenerator().Process(currentFilter);
                    ////Text = expression;
                    //DataTable dt = filteredDataView.ToTable();
                    //MainGrdPricing.DataSource = dt.Select("ISNOTCONSIDERINEXPORT = 0").CopyToDataTable();
                    //IsNotConsiderExportStoneFlag = true;

                    CriteriaOperator currentFilter = CriteriaOperator.Clone(GrdDetStock.ActiveFilterCriteria);
                    GrdDetStock.ActiveFilter.Add(GrdDetStock.Columns["ISNOTCONSIDERINEXPORT"], new ColumnFilterInfo("[ISNOTCONSIDERINEXPORT] = 0"));
                    string STR = GrdDetStock.ActiveFilterCriteria.ToString();
                }
                else
                {
                    GrdDetStock.Columns["ISNOTCONSIDERINEXPORT"].ClearFilter();
                }


                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = ".xlsx";
                svDialog.Title = "Export to Excel";
                svDialog.FileName = txtPartyName.Text.Trim() + "_ListingWithBothPrice";
                svDialog.Filter = "Excel files 97-2003 (*.xls)|*.xls|Excel files 2007(*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    GrdDetStock.OptionsPrint.PrintBandHeader = true;
                    GrdDetStock.Columns["WEBSITE"].Visible = false;
                    GrdDetStock.Columns["RATIO"].Visible = false;
                    GrdDetStock.Columns["GIRDLEDESC"].Visible = false;
                    GrdDetStock.Columns["HA"].Visible = false;
                    GrdDetStock.Columns["TABLEINC"].Visible = false;
                    GrdDetStock.Columns["SIDEINC"].Visible = false;
                    GrdDetStock.Columns["TABLEBLACKINC"].Visible = false;
                    GrdDetStock.Columns["SIDEBLACKINC"].Visible = false;
                    GrdDetStock.Columns["SIDEOPENINC"].Visible = false;
                    GrdDetStock.Columns["TABLEOPENINC"].Visible = false;
                    GrdDetStock.Columns["EXTRAFACET"].Visible = false;
                    GrdDetStock.Columns["LUSTER"].Visible = false;
                    GrdDetStock.Columns["COMMENT"].Visible = false;
                    GrdDetStock.Columns["GIRDLEPER"].Visible = false;
                    GrdDetStock.Columns["GRAINING"].Visible = false;
                    GrdDetStock.Columns["LOWERHALF"].Visible = false;
                    GrdDetStock.Columns["STARLENGTH"].Visible = false;
                    GrdDetStock.Columns["FILERAPAMT"].Visible = false;

                    GrdDetStock.Columns["DEPTH"].Visible = false;
                    GrdDetStock.Columns["TABLE1"].Visible = false;
                    GrdDetStock.Columns["CRANGLE"].Visible = false;
                    GrdDetStock.Columns["PAVANGLE"].Visible = false;
                    GrdDetStock.Columns["CRHEIGHT"].Visible = false;
                    GrdDetStock.Columns["PAVHEIGHT"].Visible = false;
                    GrdDetStock.Columns["KEYTOSYMBOL"].Visible = false;
                    GrdDetStock.Columns["HEIGHT"].Visible = false;
                    GrdDetStock.Columns["REPORTCOMMENT"].Visible = false;
                    GrdDetStock.Columns["KEYTOSYMBOL"].Visible = false;

                    GrdDetStock.Bands["BANDSNOTEXPORTED"].Visible = false;

                    PrintableComponentLinkBase link = new PrintableComponentLinkBase()
                    {
                        PrintingSystemBase = new PrintingSystemBase(),
                        Component = MainGrdStock,
                        Landscape = true,
                        PaperKind = PaperKind.A4,
                        Margins = new System.Drawing.Printing.Margins(20, 25, 20, 20)
                    };

                    //link.CreateReportHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);

                    link.ExportToXls(svDialog.FileName);

                    GrdDetStock.Columns["WEBSITE"].Visible = true;
                    GrdDetStock.Columns["RATIO"].Visible = true;
                    GrdDetStock.Columns["GIRDLEDESC"].Visible = true;
                    GrdDetStock.Columns["HA"].Visible = true;
                    GrdDetStock.Columns["TABLEINC"].Visible = true;
                    GrdDetStock.Columns["SIDEINC"].Visible = true;
                    GrdDetStock.Columns["TABLEBLACKINC"].Visible = true;
                    GrdDetStock.Columns["SIDEBLACKINC"].Visible = true;
                    GrdDetStock.Columns["SIDEOPENINC"].Visible = true;
                    GrdDetStock.Columns["TABLEOPENINC"].Visible = true;
                    GrdDetStock.Columns["EXTRAFACET"].Visible = true;
                    GrdDetStock.Columns["LUSTER"].Visible = true;
                    GrdDetStock.Columns["COMMENT"].Visible = true;
                    GrdDetStock.Columns["GIRDLEPER"].Visible = true;
                    GrdDetStock.Columns["GRAINING"].Visible = true;
                    GrdDetStock.Columns["LOWERHALF"].Visible = true;
                    GrdDetStock.Columns["STARLENGTH"].Visible = true;
                    GrdDetStock.Columns["FILERAPAMT"].Visible = true;

                    GrdDetStock.Columns["DEPTH"].Visible = true;
                    GrdDetStock.Columns["TABLE1"].Visible = true;
                    GrdDetStock.Columns["CRANGLE"].Visible = true;
                    GrdDetStock.Columns["PAVANGLE"].Visible = true;
                    GrdDetStock.Columns["CRHEIGHT"].Visible = true;
                    GrdDetStock.Columns["PAVHEIGHT"].Visible = true;
                    GrdDetStock.Columns["KEYTOSYMBOL"].Visible = true;
                    GrdDetStock.Columns["HEIGHT"].Visible = true;
                    GrdDetStock.Columns["REPORTCOMMENT"].Visible = true;
                    GrdDetStock.Columns["KEYTOSYMBOL"].Visible = true;
                    GrdDetStock.Columns["STATUS"].Visible = true;

                    GrdDetStock.Bands["BANDSNOTEXPORTED"].Visible = true;

                    if (Global.Confirm("Do You Want To Open [" + svDialog.FileName + ".xlsx] ?") == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(svDialog.FileName, "CMD");
                    }
                }
                GrdDetStock.Columns["ISNOTCONSIDERINEXPORT"].ClearFilter();
                svDialog.Dispose();
                svDialog = null;

            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
            }
        }

        private void BtnExport3_Click(object sender, EventArgs e)
        {
            if (GrdDetStock.RowCount <= 0)
                return;

            try
            {
                if (Global.Confirm("You Want that Stone's Also Which Is Not Consider In Export ?") == System.Windows.Forms.DialogResult.No)
                {
                    //DataView filteredDataView = new DataView(GrdDetPricing.GridControl.DataSource as DataTable);
                    ////filteredDataView.RowFilter = DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetDataSetWhere(GrdDetPricing.ActiveFilterCriteria.LegacyToString()); //original
                    //string filter = DevExpress.Data.Filtering.CriteriaToWhereClauseHelper.GetDataSetWhere(GrdDetPricing.ActiveFilterCriteria);
                    //if(filter.Contains(Val.ToString("[Carat]").ToUpper()))
                    //    filter = filter.Replace(Val.ToString("[CARAT]").ToUpper(),"Convert([CARAT],'System.String')");
                    //filteredDataView.RowFilter = filter;
                    //CriteriaOperator currentFilter = CriteriaOperator.Clone(GrdDetPricing.ActiveFilterCriteria);
                    ////string expression = new ExtendedDataSetWhereGenerator().Process(currentFilter);
                    ////Text = expression;
                    //DataTable dt = filteredDataView.ToTable();
                    //MainGrdPricing.DataSource = dt.Select("ISNOTCONSIDERINEXPORT = 0").CopyToDataTable();
                    //IsNotConsiderExportStoneFlag = true;

                    CriteriaOperator currentFilter = CriteriaOperator.Clone(GrdDetStock.ActiveFilterCriteria);
                    GrdDetStock.ActiveFilter.Add(GrdDetStock.Columns["ISNOTCONSIDERINEXPORT"], new ColumnFilterInfo("[ISNOTCONSIDERINEXPORT] = 0"));
                    string STR = GrdDetStock.ActiveFilterCriteria.ToString();
                }
                else
                {
                    GrdDetStock.Columns["ISNOTCONSIDERINEXPORT"].ClearFilter();
                }



                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = ".xlsx";
                svDialog.Title = "Export to Excel";
                svDialog.FileName = txtPartyName.Text.Trim() + "_Offer";
                svDialog.Filter = "Excel files 97-2003 (*.xls)|*.xls|Excel files 2007(*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    GrdDetStock.OptionsPrint.PrintBandHeader = true;
                    GrdDetStock.Columns["WEBSITE"].Visible = false;
                    GrdDetStock.Columns["RATIO"].Visible = false;
                    GrdDetStock.Columns["GIRDLEDESC"].Visible = false;
                    GrdDetStock.Columns["HA"].Visible = false;
                    GrdDetStock.Columns["TABLEINC"].Visible = false;
                    GrdDetStock.Columns["SIDEINC"].Visible = false;
                    GrdDetStock.Columns["TABLEBLACKINC"].Visible = false;
                    GrdDetStock.Columns["SIDEBLACKINC"].Visible = false;
                    GrdDetStock.Columns["SIDEOPENINC"].Visible = false;
                    GrdDetStock.Columns["TABLEOPENINC"].Visible = false;
                    GrdDetStock.Columns["EXTRAFACET"].Visible = false;
                    GrdDetStock.Columns["LUSTER"].Visible = false;
                    GrdDetStock.Columns["COMMENT"].Visible = false;
                    GrdDetStock.Columns["GIRDLEPER"].Visible = false;
                    GrdDetStock.Columns["GRAINING"].Visible = false;
                    GrdDetStock.Columns["LOWERHALF"].Visible = false;
                    GrdDetStock.Columns["STARLENGTH"].Visible = false;
                    GrdDetStock.Columns["FILERAPAMT"].Visible = false;

                    GrdDetStock.Columns["DEPTH"].Visible = false;
                    GrdDetStock.Columns["TABLE1"].Visible = false;
                    GrdDetStock.Columns["CRANGLE"].Visible = false;
                    GrdDetStock.Columns["PAVANGLE"].Visible = false;
                    GrdDetStock.Columns["CRHEIGHT"].Visible = false;
                    GrdDetStock.Columns["PAVHEIGHT"].Visible = false;
                    GrdDetStock.Columns["KEYTOSYMBOL"].Visible = false;
                    GrdDetStock.Columns["HEIGHT"].Visible = false;
                    GrdDetStock.Columns["REPORTCOMMENT"].Visible = false;
                    GrdDetStock.Columns["KEYTOSYMBOL"].Visible = false;

                    GrdDetStock.Columns["FILERAP"].Visible = true;
                    GrdDetStock.Columns["FILEPRICEPERCARAT"].Visible = true;
                    GrdDetStock.Columns["FILEAMOUNT"].Visible = true;
                    GrdDetStock.Columns["DIFFAMOUNT"].Visible = false;

                    GrdDetStock.Columns["DIFFBACKPER"].Caption = "Diff %";

                    GrdDetStock.Bands["BANDCOMPCALC"].Visible = false;
                    GrdDetStock.Bands["BANDSTATUS"].Visible = false;
                    GrdDetStock.Bands["BANDSNOTEXPORTED"].Visible = false;

                    PrintableComponentLinkBase link = new PrintableComponentLinkBase()
                    {
                        PrintingSystemBase = new PrintingSystemBase(),
                        Component = MainGrdStock,
                        //Landscape = true,
                        PaperKind = PaperKind.A4,
                        Margins = new System.Drawing.Printing.Margins(20, 25, 20, 20)
                    };

                    //link.CreateReportHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);

                    link.ExportToXls(svDialog.FileName);

                    GrdDetStock.Columns["WEBSITE"].Visible = true;
                    GrdDetStock.Columns["RATIO"].Visible = true;
                    GrdDetStock.Columns["GIRDLEDESC"].Visible = true;
                    GrdDetStock.Columns["HA"].Visible = true;
                    GrdDetStock.Columns["TABLEINC"].Visible = true;
                    GrdDetStock.Columns["SIDEINC"].Visible = true;
                    GrdDetStock.Columns["TABLEBLACKINC"].Visible = true;
                    GrdDetStock.Columns["SIDEBLACKINC"].Visible = true;
                    GrdDetStock.Columns["SIDEOPENINC"].Visible = true;
                    GrdDetStock.Columns["TABLEOPENINC"].Visible = true;
                    GrdDetStock.Columns["EXTRAFACET"].Visible = true;
                    GrdDetStock.Columns["LUSTER"].Visible = true;
                    GrdDetStock.Columns["COMMENT"].Visible = true;
                    GrdDetStock.Columns["GIRDLEPER"].Visible = true;
                    GrdDetStock.Columns["GRAINING"].Visible = true;
                    GrdDetStock.Columns["LOWERHALF"].Visible = true;
                    GrdDetStock.Columns["STARLENGTH"].Visible = true;
                    GrdDetStock.Columns["FILERAPAMT"].Visible = true;

                    GrdDetStock.Columns["DEPTH"].Visible = true;
                    GrdDetStock.Columns["TABLE1"].Visible = true;
                    GrdDetStock.Columns["CRANGLE"].Visible = true;
                    GrdDetStock.Columns["PAVANGLE"].Visible = true;
                    GrdDetStock.Columns["CRHEIGHT"].Visible = true;
                    GrdDetStock.Columns["PAVHEIGHT"].Visible = true;
                    GrdDetStock.Columns["KEYTOSYMBOL"].Visible = true;
                    GrdDetStock.Columns["HEIGHT"].Visible = true;
                    GrdDetStock.Columns["REPORTCOMMENT"].Visible = true;
                    GrdDetStock.Columns["KEYTOSYMBOL"].Visible = true;
                    GrdDetStock.Columns["STATUS"].Visible = true;

                    GrdDetStock.Columns["FILERAP"].Visible = true;
                    GrdDetStock.Columns["FILEPRICEPERCARAT"].Visible = true;
                    GrdDetStock.Columns["FILEAMOUNT"].Visible = true;
                    GrdDetStock.Columns["DIFFAMOUNT"].Visible = true;

                    GrdDetStock.Columns["DIFFBACKPER"].Caption = "Profit %";

                    GrdDetStock.Bands["BANDCOMPCALC"].Visible = true;
                    GrdDetStock.Bands["BANDSTATUS"].Visible = true;
                    GrdDetStock.Bands["BANDSNOTEXPORTED"].Visible = true;


                    if (Global.Confirm("Do You Want To Open [" + svDialog.FileName + ".xlsx] ?") == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(svDialog.FileName, "CMD");
                    }
                }
                GrdDetStock.Columns["ISNOTCONSIDERINEXPORT"].ClearFilter();
                svDialog.Dispose();
                svDialog = null;

            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
            }
        }

        public void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            // ' For Report Title

            //TextBrick BrickTitle = e.Graph.DrawString("SELECTION LIST", System.Drawing.Color.Navy, new RectangleF(0, 0, e.Graph.ClientPageSize.Width - 100, 35), DevExpress.XtraPrinting.BorderSide.None);
            //BrickTitle.Font = new Font("verdana", 12, FontStyle.Bold);
            //BrickTitle.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            //BrickTitle.VertAlignment = DevExpress.Utils.VertAlignment.Center;

            // ' For Group 
            TextBrick BrickTitleseller = e.Graph.DrawString("Stone Details Of Customer  :-  " + txtPartyName.Text + " (" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + ")", System.Drawing.Color.Navy, new RectangleF(0, 0, e.Graph.ClientPageSize.Width, 30), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitleseller.Font = new Font("verdana", 10, FontStyle.Bold);
            BrickTitleseller.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            BrickTitleseller.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            BrickTitleseller.ForeColor = Color.Black;


            //int IntX = Convert.ToInt32(Math.Round(e.Graph.ClientPageSize.Width - 400, 0));
            //TextBrick BrickTitledate = e.Graph.DrawString("Print Date :- " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), System.Drawing.Color.Navy, new RectangleF(IntX, 70, 400, 30), DevExpress.XtraPrinting.BorderSide.None);
            //BrickTitledate.Font = new Font("verdana", 8, FontStyle.Bold);
            //BrickTitledate.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            //BrickTitledate.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            //BrickTitledate.ForeColor = Color.Black;

        }


        public void Link_CreateMarginalHeaderArea2(object sender, CreateAreaEventArgs e)
        {
            TextBrick BrickTitleseller = e.Graph.DrawString("Stone Details Of Customer  :-  " + txtPartyName.Text + " (" + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt") + ")", System.Drawing.Color.Navy, new RectangleF(0, 0, e.Graph.ClientPageSize.Width, 30), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitleseller.Font = new Font("verdana", 10, FontStyle.Bold);
            BrickTitleseller.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            BrickTitleseller.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            BrickTitleseller.ForeColor = Color.Black;
        }


        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog OpenFileDialog = new OpenFileDialog();
                OpenFileDialog.Filter = "Excel Files (*.xls,*.xlsx)|*.xls;*.xlsx;";
                //OpenFileDialog.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
                if (OpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtFileName.Text = OpenFileDialog.FileName;

                    string extension = Path.GetExtension(txtFileName.Text.ToString());
                    string destinationPath = Application.StartupPath + @"\StoneFiles\" + Path.GetFileName(txtFileName.Text);
                    destinationPath = destinationPath.Replace(extension, ".xlsx");
                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }
                    File.Copy(txtFileName.Text, destinationPath);


                    //GetExcelSheetNames(destinationPath);
                    //CmbSheetname.SelectedIndex = 0;
                    ////StrUploadFilename = OpenFileDialog.FileName;
                    ////string extension = Path.GetExtension(txtFileName.Text);
                    ////StrUploadFilename = StrUploadFilename.Replace(extension, ".xlsx");
                    ////string destinationPath = Application.StartupPath + @"\StoneFiles\" + Path.GetFileName(StrUploadFilename);
                    ////File.Copy(txtFileName.Text, destinationPath);

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
                Global.Message(ex.ToString() + "InValid File Name");
            }
        }
        private String[] GetExcelSheetNames(string excelFile)
        {
            OleDbConnection objConn = null;
            System.Data.DataTable dt = null;

            try
            {
                String connString = "";
                //String connString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                //  "Data Source=" + excelFile + ";Extended Properties=Excel 12.0;";
                if (Path.GetExtension(excelFile).Equals(".xls"))//for 97-03 Excel file
                {
                    connString = "Provider=Microsoft.ACE.OLEDB.4.0;" +
                      "Data Source=" + excelFile + ";Extended Properties=Excel 8.0;";
                }
                //else if (Path.GetExtension(filePath).Equals(".xlsx"))  //for 2007 Excel file
                else
                {
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                 "Data Source=" + excelFile + ";Extended Properties=Excel 12.0;";
                }

                objConn = new OleDbConnection(connString);
                objConn.Open();
                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                List<string> sheets = new List<string>();
                if (dt == null)
                {
                    return null;
                }
                String[] excelSheets = new String[dt.Rows.Count];
                CmbSheetname.Items.Clear(); //ADD:KULDEEP[24/05/18]
                foreach (DataRow row in dt.Rows)
                {
                    string sheetName = (string)row["TABLE_NAME"];
                    sheets.Add(sheetName);
                    CmbSheetname.Items.Add(sheetName);
                    //if (sheetName.EndsWith("$'"))
                    //{
                    //    sheets.Add(sheetName);
                    //    CmbSheetname.Properties.Items.Add(sheetName);
                    //}
                    //else
                    //{
                    //    sheets.Add(sheetName);
                    //    CmbSheetname.Properties.Items.Add(sheetName);
                    //}
                }

                return excelSheets;
            }
            catch (Exception ex)
            {
                Global.Message(ex.ToString());
                return null;
            }
            finally
            {
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
        }

        public static DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    if (Convert.ToString(firstRowCell.Text).Equals(string.Empty))
                        continue;

                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        if (Convert.ToString(cell.Text).Equals(string.Empty))
                            continue;

                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                return tbl;
            }
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //DtabPara = new BOMST_Parameter().GetParameterDataParcel();
                DtabExcelData.Rows.Clear();
                if (ValSave())
                {
                    this.Cursor = Cursors.Default;
                    return;
                }
                string StrStockType = Val.ToString(CmbStockType.SelectedItem).ToUpper();

                string extension = Path.GetExtension(txtFileName.Text.ToString());
                string destinationPath = Application.StartupPath + @"\StoneFiles\" + Path.GetFileName(txtFileName.Text);
                destinationPath = destinationPath.Replace(extension, ".xlsx");
                if (File.Exists(destinationPath))
                {
                    File.Delete(destinationPath);
                }
                File.Copy(txtFileName.Text, destinationPath);

                DtabExcelData = GetDataTableFromExcel(destinationPath);

                //DtabExcelData = Global.ImportExcelXLSWithSheetName(destinationPath, false, CmbSheetname.SelectedItem.ToString(), 1);

                ////Change Column Caption which Is Stored in Datatable First Row  : 26-06-2019 (Coz Issue On Number and String Type Data)
                //for (int ICount = 0; ICount < DtabExcelData.Columns.Count; ICount++)
                //{
                //    if (Val.ToString(DtabExcelData.Rows[0][ICount]).Trim().Equals(string.Empty))
                //    {
                //        DtabExcelData.Columns.Remove(DtabExcelData.Columns[ICount]);
                //        continue;
                //    }
                //    DtabExcelData.Columns[ICount].ColumnName = Val.ToString(DtabExcelData.Rows[0][ICount]);
                //}
                //DtabExcelData.Rows.Remove(DtabExcelData.Rows[0]);

                //Add : 30-08-2019 Coz File Contains tow header With English and Chinese Name
                if (Val.ToString(DtabExcelData.Rows[0][0]).ToUpper() == "SR. NO" || Val.ToString(DtabExcelData.Rows[0][0]).ToUpper() == Val.ToString("序号").ToUpper())
                {
                    int IntIndex = Val.ToInt32(DtabExcelData.Rows.IndexOf(DtabExcelData.Rows[0]));
                    DtabExcelData.Rows.Remove(DtabExcelData.Rows[0]);
                }
                //end : 30-08-2019

                //End : 26-06-2019
                //DtabExcelData = Global.GetDataTableFromExcel(destinationPath, true); //Another Option for get ExcelData Into DataTable

                if (File.Exists(destinationPath))
                {
                    File.Delete(destinationPath);
                }

                int IntCount = 0;
                StrUploadFilename = Path.GetFileName(destinationPath);

                DataTable DtabExcelSetting = new DataTable();
                int i = DtabStockUpload.Rows.Count;
                DtabExcelSetting = new BOMST_ExcelSetting().Fill(Guid.Parse(Val.ToString(txtPartyName.Tag)));
                DtabExcelSetting.Columns.Add("STATUS", typeof(string)); //Used For Identifying that Required Columns contains Value or not (DONE means Column Contains Value)
                string StrCompColumn = "";
                string StrNotCompNotExist = "";

                //Fetch Compulsory and Non-Compulsory Columns as Per PartyExcel Setting Wise and Update Column name From Excel Column Name with Setting Name
                for (int Intcol = 0; Intcol < DtabExcelData.Columns.Count; Intcol++)
                {
                    var VarQryForCol = (from DrSetting in DtabExcelSetting.AsEnumerable()
                                        where Val.ToString(DrSetting["EXCELSETTINGREFNAME"]).ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper().Replace("#", ".")))
                                        select DrSetting).ToList();

                    if (VarQryForCol.Any())
                    {
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString(VarQryForCol.FirstOrDefault()["EXCELSETTINGNAME"]);
                        VarQryForCol[0]["STATUS"] = "DONE";
                    }
                    else
                    {
                        if (Val.ToString(StrNotCompNotExist).Trim().Equals(string.Empty))
                            StrNotCompNotExist = Val.ToString(DtabExcelData.Columns[Intcol].ColumnName).Replace("#", ".");
                        else
                            StrNotCompNotExist = StrNotCompNotExist + "," + Val.ToString(DtabExcelData.Columns[Intcol].ColumnName).Replace("#", ".");
                    }
                }

                //Display Column Message Which Combination is Not Exists In DB
                if (!Val.ToString(StrNotCompNotExist).Trim().Equals(string.Empty))
                    if (Global.Confirm("Combination For This Excel Columns -> '" + StrNotCompNotExist + "' Is Not Exist Still You Want To Continue ?") == System.Windows.Forms.DialogResult.No)
                    {
                        this.Cursor = Cursors.Default;
                        return;
                    }

                //Display Required Column Message If Combination Not Exists (IN Single/Parcel Type File
                if (Val.ToString(CmbStockType.SelectedItem).Trim().ToUpper().Equals("PARCEL"))
                {
                    var VarParcelQry = (from DrSetting in DtabExcelSetting.AsEnumerable()
                                        where Val.ToBooleanToInt(DrSetting["ISCOMPULSORYINPARCEL"]) == 1 && Val.ToString(DrSetting["STATUS"]) != "DONE"
                                        select DrSetting).ToList();

                    if (VarParcelQry.Any())
                    {

                        foreach (DataRow dr in VarParcelQry)
                            StrCompColumn += "," + dr["REMARK"].ToString();

                        Global.Message("This Columns -> '" + StrCompColumn.Substring(1) + "' Are Required In Excel Sheet For Store Pricing Data..");
                        this.Cursor = Cursors.Default;
                        return;
                    }
                }
                else
                {
                    var VarSingleQry = (from DrSetting in DtabExcelSetting.AsEnumerable()
                                        where Val.ToBooleanToInt(DrSetting["ISCOMPULSORYINSINGLE"]) == 1 && Val.ToString(DrSetting["STATUS"]) != "DONE"
                                        select DrSetting).ToList();

                    if (VarSingleQry.Any())
                    {

                        foreach (DataRow dr in VarSingleQry)
                            StrCompColumn += "," + dr["REMARK"].ToString();

                        Global.Message("This Columns -> '" + StrCompColumn.Substring(1) + "' Are Required In Excel Sheet For Store Pricing Data..");
                        this.Cursor = Cursors.Default;
                        return;
                    }
                }



                int ICol = 0;
                foreach (DataRow dr in DtabExcelSetting.Rows)
                {
                    DataColumnCollection Dcol = DtabExcelData.Columns;

                    if (!Dcol.Contains(Val.ToString(dr["EXCELSETTINGNAME"])))
                    {
                        DtabExcelData.Columns.Add(Val.ToString(dr["EXCELSETTINGNAME"]), typeof(object));
                    }
                    ICol++;

                }


                //Check Required Column Value Exists Or Not
                foreach (DataRow DRow in DtabExcelData.Rows)
                {
                    if (Val.ToString(DRow["ENTRYSRNO"]).Length == 0)
                    {
                        continue;
                    }

                    if (Val.ToString(DRow["SHAPENAME"]).Length != 0)
                    {
                        if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = 'SHAPE'").CopyToDataTable(), "LABCODE", Val.ToString(DRow["SHAPENAME"]).ToUpper(), "PARA_ID", true)) == 0)
                        {
                            this.Cursor = Cursors.Default;
                            Global.Message("Shape [" + Val.ToString(DRow["SHAPENAME"]) + "] Is Not Valid ");
                            return;
                        }
                    }
                    if (Val.ToString(DRow["COLORNAME"]).Length != 0)
                    {
                        if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = 'COLOR' ").CopyToDataTable(), "PARANAME", Val.ToString(DRow["COLORNAME"]).ToUpper(), "PARA_ID", true)) == 0)
                        {
                            this.Cursor = Cursors.Default;
                            Global.Message("Color -> '" + Val.ToString(DRow["COLORNAME"]) + "' Is Not Valid ");
                            return;
                        }
                    }
                    if (Val.ToString(DRow["CLARITYNAME"]).Length != 0)
                    {
                        if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = 'MIX_CLARITY' ").CopyToDataTable(), "PARANAME", Val.ToString(DRow["CLARITYNAME"]).ToUpper(), "PARA_ID", true)) == 0)
                        {
                            this.Cursor = Cursors.Default;
                            Global.Message("Mix Clarity -> '" + Val.ToString(DRow["CLARITYNAME"]) + "' Is Not Valid ");
                            return;
                        }
                    }
                    if (Val.ToString(DRow["SIZE"]).Length != 0)
                    {
                        if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = 'MIX_SIZE' ").CopyToDataTable(), "LABCODE", Val.ToString(DRow["SIZE"]).ToUpper(), "PARA_ID", true)) == 0)
                        {
                            this.Cursor = Cursors.Default;
                            Global.Message("Mix Size -> '" + Val.ToString(DRow["SIZE"]) + "' Is Not Valid ");
                            return;
                        }
                    }
                    //if (Val.ToString(DRow["EYECLEAN"]).Length != 0)
                    //{
                    //    if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = 'EYECLEAN'").CopyToDataTable(), "LABCODE", Val.ToString(DRow["EYECLEAN"]).ToUpper(), "PARA_ID", true)) == 0)
                    //    {
                    //        this.Cursor = Cursors.Default;
                    //        Global.Message("EyeClean -> '" + Val.ToString(DRow["EYECLEAN"]) + "' Is Not Valid In Stone No : '" + DRow["PARTYSTOCKNO"] + "'");
                    //        return;
                    //    }
                    //}
                    //if (Val.ToString(DRow["TINGE"]).Length != 0)
                    //{
                    //    if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = 'TINGE'").CopyToDataTable(), "LABCODE", Val.ToString(DRow["TINGE"]).ToUpper(), "PARA_ID", true)) == 0)
                    //    {
                    //        this.Cursor = Cursors.Default;
                    //        Global.Message("Tinge -> '" + Val.ToString(DRow["TINGE"]) + "' Is Not Valid In Stone No : '" + DRow["PARTYSTOCKNO"] + "'");
                    //        return;
                    //    }
                    //}
                    if (Val.Val(DRow["CARAT"]) == 0)
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("Carat Not Valid ");
                        return;
                    }
                    if (Val.ToString(DRow["PARTYNAME"]).Length != 0)
                    {
                        if (Val.ToString(Val.SearchText(DtabSupplier, "PARTYNAME", Val.ToString(DRow["PARTYNAME"]).ToUpper(), "PARTY_ID", false)).Trim().Equals(string.Empty))
                        {
                            this.Cursor = Cursors.Default;
                            Global.Message("Supplier Is Not Valid ");
                            return;
                        }
                    }
                    if (Val.ToString(DRow["PARTYNAME"]).Length == 0 && Val.ToString(txtPartyName.Text).Length == 0)
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("Supplier Is Not Valid ");
                        return;
                    }

                    ////#P : 23-07-2020
                    //if (Val.ToString(DRow["LBLCNAME"]).Length != 0)
                    //{
                    //    if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = 'LBLC'").CopyToDataTable(), "PARANAME", Val.ToString(DRow["LBLCNAME"]).ToUpper(), "PARA_ID", true)) == 0)
                    //    {
                    //        this.Cursor = Cursors.Default;
                    //        Global.Message("LBLC -> '" + Val.ToString(DRow["LBLCNAME"]) + "' Is Not Valid In Stone No : '" + DRow["PARTYSTOCKNO"] + "'");
                    //        return;
                    //    }
                    //}
                    //if (Val.ToString(DRow["NATTSNAME"]).Length != 0)
                    //{
                    //    if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = 'NATTS'").CopyToDataTable(), "LABCODE", Val.ToString(DRow["NATTSNAME"]).ToUpper(), "PARA_ID", true)) == 0)
                    //    {
                    //        this.Cursor = Cursors.Default;
                    //        Global.Message("Natts -> '" + Val.ToString(DRow["NATTSNAME"]) + "' Is Not Valid In Stone No : '" + DRow["PARTYSTOCKNO"] + "'");
                    //        return;
                    //    }
                    //}
                    //if (Val.ToString(DRow["TENSIONNAME"]).Length != 0)
                    //{
                    //    if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = 'TENSION'").CopyToDataTable(), "LABCODE", Val.ToString(DRow["TENSIONNAME"]).ToUpper(), "PARA_ID", true)) == 0)
                    //    {
                    //        this.Cursor = Cursors.Default;
                    //        Global.Message("Tension -> '" + Val.ToString(DRow["TENSIONNAME"]) + "' Is Not Valid In Stone No : '" + DRow["PARTYSTOCKNO"] + "'");
                    //        return;
                    //    }
                    //}
                    //if (Val.ToString(DRow["BINCNAME"]).Length != 0)
                    //{
                    //    if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = 'BLACK'").CopyToDataTable(), "PARACODE", Val.ToString(DRow["BINCNAME"]).ToUpper(), "PARA_ID", true)) == 0)
                    //    {
                    //        this.Cursor = Cursors.Default;
                    //        Global.Message("BlackInclusion -> '" + Val.ToString(DRow["BINCNAME"]) + "' Is Not Valid In Stone No : '" + DRow["PARTYSTOCKNO"] + "'");
                    //        return;
                    //    }
                    //}
                    //if (Val.ToString(DRow["WINCNAME"]).Length != 0)
                    //{
                    //    if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = 'WHITE'").CopyToDataTable(), "PARACODE", Val.ToString(DRow["WINCNAME"]).ToUpper(), "PARA_ID", true)) == 0)
                    //    {
                    //        this.Cursor = Cursors.Default;
                    //        Global.Message("WhiteInclusion -> '" + Val.ToString(DRow["WINCNAME"]) + "' Is Not Valid In Stone No : '" + DRow["PARTYSTOCKNO"] + "'");
                    //        return;
                    //    }
                    //}
                    //if (Val.ToString(DRow["OINCNAME"]).Length != 0)
                    //{
                    //    if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = 'OPEN'").CopyToDataTable(), "PARACODE", Val.ToString(DRow["OINCNAME"]).ToUpper(), "PARA_ID", true)) == 0)
                    //    {
                    //        this.Cursor = Cursors.Default;
                    //        Global.Message("OpenInclusion -> '" + Val.ToString(DRow["OINCNAME"]) + "' Is Not Valid In Stone No : '" + DRow["PARTYSTOCKNO"] + "'");
                    //        return;
                    //    }
                    //}
                    //if (Val.ToString(DRow["LUSTERNAME"]).Length != 0)
                    //{
                    //    if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = 'LUSTER'").CopyToDataTable(), "PARANAME", Val.ToString(DRow["LUSTERNAME"]).ToUpper(), "PARA_ID", true)) == 0)
                    //    {
                    //        this.Cursor = Cursors.Default;
                    //        Global.Message("Luster -> '" + Val.ToString(DRow["OINCNAME"]) + "' Is Not Valid In Stone No : '" + DRow["PARTYSTOCKNO"] + "'");
                    //        return;
                    //    }
                    //}
                    //if (Val.ToString(DRow["HANAME"]).Length != 0)
                    //{
                    //    if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = 'HEARTANDARROW'").CopyToDataTable(), "PARANAME", Val.ToString(DRow["HANAME"]).ToUpper(), "PARA_ID", true)) == 0)
                    //    {
                    //        this.Cursor = Cursors.Default;
                    //        Global.Message("HeartAndArrow -> '" + Val.ToString(DRow["HANAME"]) + "' Is Not Valid In Stone No : '" + DRow["PARTYSTOCKNO"] + "'");
                    //        return;
                    //    }
                    //}
                    //if (Val.ToString(DRow["PAVNAME"]).Length != 0)
                    //{
                    //    if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = 'PAVALION'").CopyToDataTable(), "PARACODE", Val.ToString(DRow["PAVNAME"]).ToUpper(), "PARA_ID", true)) == 0)
                    //    {
                    //        this.Cursor = Cursors.Default;
                    //        Global.Message("Pavillion -> '" + Val.ToString(DRow["PAVNAME"]) + "' Is Not Valid In Stone No : '" + DRow["PARTYSTOCKNO"] + "'");
                    //        return;
                    //    }
                    //}
                    //if (Val.ToString(DRow["NATURALNAME"]).Length != 0)
                    //{
                    //    if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = 'NATURAL'").CopyToDataTable(), "LABCODE", Val.ToString(DRow["NATURALNAME"]).ToUpper(), "PARA_ID", true)) == 0)
                    //    {
                    //        this.Cursor = Cursors.Default;
                    //        Global.Message("Natural -> '" + Val.ToString(DRow["NATURALNAME"]) + "' Is Not Valid In Stone No : '" + DRow["PARTYSTOCKNO"] + "'");
                    //        return;
                    //    }
                    //}
                    //if (Val.ToString(DRow["GRAINNAME"]).Length != 0)
                    //{
                    //    if (Val.ToInt32(Val.SearchText(DtabPara.Select("PARATYPE = 'GRAIN'").CopyToDataTable(), "LABCODE", Val.ToString(DRow["GRAINNAME"]).ToUpper(), "PARA_ID", true)) == 0)
                    //    {
                    //        this.Cursor = Cursors.Default;
                    //        Global.Message("Grain -> '" + Val.ToString(DRow["GRAINNAME"]) + "' Is Not Valid In Stone No : '" + DRow["PARTYSTOCKNO"] + "'");
                    //        return;
                    //    }
                    //}
                    //END : #P : 23-07-2020
                }

                this.Cursor = Cursors.WaitCursor;
                int I = 0;

                if (DtabFinalData.Columns.Contains("SHAPE_CODE") == false)
                    DtabFinalData.Columns.Add("SHAPE_CODE");

                if (DtabFinalData.Columns.Contains("CLARITY_CODE") == false)
                    DtabFinalData.Columns.Add("CLARITY_CODE");

                if (DtabFinalData.Columns.Contains("SIZE_CODE") == false)
                    DtabFinalData.Columns.Add("SIZE_CODE");

                foreach (DataRow DrPrice in DtabExcelData.Rows)
                {
                    if (Val.ToString(DrPrice["ENTRYSRNO"]).Trim().Equals(string.Empty))
                        continue;

                    I++;
                    IntCount++;
                    DataRow Drfinal = DtabFinalData.NewRow();
                    Drfinal["STOCK_ID"] = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                    //Drfinal["PARTYSTOCKNO"] = DrPrice["PARTYSTOCKNO"];
                    Drfinal["STOCKTYPE"] = Val.ToString(CmbStockType.SelectedItem);


                    var drShape = (from DrPara in DtabPara.AsEnumerable()
                                   where Val.ToString(DrPara["LABCODE"]).ToUpper().Split(',').Contains(Val.ToString(DrPrice["SHAPENAME"]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "SHAPE"
                                   select DrPara);
                    Drfinal["SHAPE_ID"] = drShape.Count() > 0 ? Val.ToInt32(drShape.FirstOrDefault()["PARA_ID"]) : 0;

                    Drfinal["SHAPE_CODE"] = drShape.Count() > 0 ? Val.ToString(drShape.FirstOrDefault()["PARACODE"]) : "";

                    var drCol = (from DrPara in DtabPara.AsEnumerable()
                                 where Val.ToString(DrPara["PARANAME"]).ToUpper() == Val.ToString(DrPrice["COLORNAME"]).Trim().ToUpper() && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "COLOR"
                                 && Val.ToString(DrPara["PARAGROUP"]).ToUpper() == StrStockType
                                 select DrPara);
                    Drfinal["COLOR_ID"] = drCol.Count() > 0 ? Val.ToInt32(drCol.FirstOrDefault()["PARA_ID"]) : 0;

                    var drClarity = (from DrPara in DtabPara.AsEnumerable()
                                     where Val.ToString(DrPara["LABCODE"]).ToUpper().Split(',').Contains(Val.ToString(DrPrice["CLARITYNAME"]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "MIX_CLARITY"
                                     select DrPara);
                    Drfinal["CLARITY_ID"] = drClarity.Count() > 0 ? Val.ToInt32(drClarity.FirstOrDefault()["PARA_ID"]) : 0;
                    Drfinal["CLARITY_CODE"] = drClarity.Count() > 0 ? Val.ToString(drClarity.FirstOrDefault()["PARACODE"]) : "";

                    var drCut = (from DrPara in DtabPara.AsEnumerable()
                                 where Val.ToString(DrPara["LABCODE"]).ToUpper().Split(',').Contains(Val.ToString(DrPrice["CUTNAME"]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "CUT" && Val.ToString(DrPrice["CUTNAME"]) != ""
                                 select DrPara);
                    Drfinal["CUT_ID"] = drCut.Count() > 0 ? Val.ToInt32(drCut.FirstOrDefault()["PARA_ID"]) : 0;


                    //var drPol = (from DrPara in DtabPara.AsEnumerable()
                    //             where Val.ToString(DrPara["LABCODE"]).ToUpper().Split(',').Contains(Val.ToString(DrPrice["POLNAME"]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "POLISH"
                    //             select DrPara);
                    //Drfinal["POL_ID"] = drPol.Count() > 0 ? Val.ToInt32(drPol.FirstOrDefault()["PARA_ID"]) : 0;


                    //var drSym = (from DrPara in DtabPara.AsEnumerable()
                    //             where Val.ToString(DrPara["LABCODE"]).ToUpper().Split(',').Contains(Val.ToString(DrPrice["SYMNAME"]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "SYMMETRY"
                    //             select DrPara);
                    //Drfinal["SYM_ID"] = drSym.Count() > 0 ? Val.ToInt32(drSym.FirstOrDefault()["PARA_ID"]) : 0;

                    //var drFL = (from DrPara in DtabPara.AsEnumerable()
                    //            where Val.ToString(DrPara["LABCODE"]).ToUpper().Split(',').Contains(Val.ToString(DrPrice["FLNAME"]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "FLUORESCENCE"
                    //            select DrPara);
                    //Drfinal["FL_ID"] = drFL.Count() > 0 ? Val.ToInt32(drFL.FirstOrDefault()["PARA_ID"]) : 0;


                    var drLocation = (from DrPara in DtabPara.AsEnumerable()
                                      where Val.ToString(DrPara["LABCODE"]).ToUpper().Split(',').Contains(Val.ToString(DrPrice["LOCATION"]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "LOCATION"
                                      select DrPara);
                    Drfinal["LOCATION_ID"] = drLocation.Count() > 0 ? Val.ToInt32(drLocation.FirstOrDefault()["PARA_ID"]) : 0;


                    ////Add : Pinali : 03-08-2019
                    //var drBrown = (from DrPara in DtabPara.AsEnumerable()
                    //               where Val.ToString(DrPara["LABCODE"]).ToUpper().Split(',').Contains(Val.ToString(DrPrice["BROWN"]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "COLORSHADE"
                    //               select DrPara);
                    //Drfinal["COLORSHADE_ID"] = drBrown.Count() > 0 ? Val.ToInt32(drBrown.FirstOrDefault()["PARA_ID"]) : 0;

                    //var drGreen = (from DrPara in DtabPara.AsEnumerable()
                    //               where Val.ToString(DrPara["PARANAME"]).ToUpper() == Val.ToString(DrPrice["GREEN"]).Trim().ToUpper() && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "GREEN"
                    //               select DrPara);
                    //Drfinal["GREEN_ID"] = drGreen.Count() > 0 ? Val.ToInt32(drGreen.FirstOrDefault()["PARA_ID"]) : 0;

                    //var drMilky = (from DrPara in DtabPara.AsEnumerable()
                    //               where Val.ToString(DrPara["LABCODE"]).ToUpper().Split(',').Contains(Val.ToString(DrPrice["MILKY"]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "MILKY"
                    //               select DrPara);
                    //Drfinal["MILKY_ID"] = drMilky.Count() > 0 ? Val.ToInt32(drMilky.FirstOrDefault()["PARA_ID"]) : 0; 

                    //var drTinge = (from DrPara in DtabPara.AsEnumerable()
                    //               where Val.ToString(DrPara["REMARK"]).ToUpper().Split(',').Contains(Val.ToString(DrPrice["TINGE"]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "TINGE"
                    //               select DrPara);
                    //Drfinal["TINGE_ID"] = drTinge.Count() > 0 ? Val.ToInt32(drTinge.FirstOrDefault()["PARA_ID"]) : 0;

                    //var drEyeClean = (from DrPara in DtabPara.AsEnumerable()
                    //                  where Val.ToString(DrPara["LABCODE"]).ToUpper().Split(',').Contains(Val.ToString(DrPrice["EYECLEAN"]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "EYECLEAN"
                    //                  select DrPara);
                    //Drfinal["EYECLEAN_ID"] = drEyeClean.Count() > 0 ? Val.ToInt32(drEyeClean.FirstOrDefault()["PARA_ID"]) : 0;

                    //var drCulet = (from DrPara in DtabPara.AsEnumerable()
                    //               where Val.ToString(DrPara["PARANAME"]).ToUpper() == Val.ToString(DrPrice["CULET"]).Trim().ToUpper() && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "CULET"
                    //               select DrPara);
                    //Drfinal["CULET_ID"] = drCulet.Count() > 0 ? Val.ToInt32(drCulet.FirstOrDefault()["PARA_ID"]) : 0;

                    //Drfinal["LASER"] = DrPrice["LASER"];

                    //Drfinal["CULETSIZE"] = DrPrice["CULETSIZE"];

                    //End : Pinali : 03-08-2019

                    Drfinal["PCS"] = Val.ToInt32(DrPrice["PCS"]) == 0 ? 1 : Val.ToInt32(DrPrice["PCS"]);
                    Drfinal["CARAT"] = Val.Val(DrPrice["CARAT"]);

                    Drfinal["BALANCEPCS"] = Val.ToInt32(DrPrice["PCS"]) == 0 ? 1 : Val.ToInt32(DrPrice["PCS"]);
                    Drfinal["BALANCECARAT"] = Val.Val(DrPrice["CARAT"]);

                    var drSize = (from DrPara in DtabPara.AsEnumerable()
                                  where Val.ToString(DrPara["PARANAME"]).ToUpper().Split(',').Contains(Val.ToString(DrPrice["SIZE"]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "MIX_SIZE"
                                  select DrPara);
                    Drfinal["SIZE_ID"] = drSize.Count() > 0 ? Val.ToInt32(drSize.FirstOrDefault()["PARA_ID"]) : 0;
                    Drfinal["SIZE_CODE"] = drSize.Count() > 0 ? Val.ToString(drSize.FirstOrDefault()["PARACODE"]) : "";

                    //var drLab = (from DrPara in DtabPara.AsEnumerable()
                    //             where Val.ToString(DrPara["PARANAME"]).ToUpper().Split(',').Contains(Val.ToString(DrPrice["LAB"]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "LAB"
                    //             select DrPara);
                    //Drfinal["LAB_ID"] = drLab.Count() > 0 ? Val.ToInt32(drLab.FirstOrDefault()["PARA_ID"]) : 0;

                    //Drfinal["LABREPORTNO"] = Val.ToString(DrPrice["LABREPORTNO"]);

                    //Drfinal["LENGTH"] = Val.Val(DrPrice["LENGTH"]);
                    //Drfinal["WIDTH"] = Val.Val(DrPrice["WIDTH"]);
                    //Drfinal["HEIGHT"] = Val.Val(DrPrice["HEIGHT"]);

                    //Drfinal["TABLEPER"] = Val.Val(DrPrice["TABLEPER"]);
                    //Drfinal["DEPTHPER"] = Val.Val(DrPrice["DEPTHPER"]);

                    //Drfinal["MEASUREMENT"] = Val.ToString(DrPrice["MEASUREMENT"]);

                    //Drfinal["CRANGLE"] = Val.Val(DrPrice["CRANGLE"]);
                    //Drfinal["CRHEIGHT"] = Val.ToString(DrPrice["CRHEIGHT"]).Contains("%") ? Val.Val(Val.ToString(DrPrice["CRHEIGHT"]).Replace("%", "")) : Val.Val(DrPrice["CRHEIGHT"]);

                    //Drfinal["PAVANGLE"] = Val.Val(DrPrice["PAVANGLE"]);
                    ////Drfinal["PAVHEIGHT"] = Val.Val(DrPrice["PAVHEIGHT"]);
                    //Drfinal["PAVHEIGHT"] = Val.ToString(DrPrice["PAVHEIGHT"]).Contains("%") ? Val.Val(Val.ToString(DrPrice["PAVHEIGHT"]).Replace("%", "")) : Val.Val(DrPrice["PAVHEIGHT"]);

                    ////Drfinal["GIRDLEPER"] = Val.Val(DrPrice["GIRDLEPER"]);
                    //Drfinal["GIRDLEPER"] = Val.ToString(DrPrice["GIRDLEPER"]).Contains("%") ? Val.Val(Val.ToString(DrPrice["GIRDLEPER"]).Replace("%", "")) : Val.Val(DrPrice["GIRDLEPER"]);
                    //Drfinal["GIRDLEDESC"] = Val.ToString(DrPrice["GIRDLEDESC"]);

                    //Drfinal["CURRENCY_ID"] = 0;
                    //Drfinal["EXCRATE"] = Val.Val(txtExcRate.Text);

                    Drfinal["CURRENCY_ID"] = Val.ToInt32(CmbCurrency.SelectedValue);
                    Drfinal["EXCRATE"] = Val.Val(txtExcRate.Text);

                    if (Val.Val(DrPrice["COSTRAPAPORT"]) != 0) // Coz Sometime Single file Contains Only Rap and Disc
                    {
                        double DouCostPricePerCarat = 0, DouCostAmount = 0;
                        DouCostPricePerCarat = (Val.Val(DrPrice["COSTRAPAPORT"]) + Val.Val(DrPrice["COSTRAPAPORT"]) * Val.Val(DrPrice["COSTDISCOUNT"]) / 100);
                        DouCostAmount = DouCostPricePerCarat * Val.Val(DrPrice["CARAT"]);

                        Drfinal["COSTRAPAPORT"] = Val.Val(DrPrice["COSTRAPAPORT"]);
                        Drfinal["COSTDISCOUNT"] = Val.Val(DrPrice["COSTDISCOUNT"]);
                        //Drfinal["COSTPRICEPERCARAT"] = Val.Val(DrPrice["COSTPRICEPERCARAT"]) == 0 ? DouCostPricePerCarat : Val.Val(DrPrice["COSTPRICEPERCARAT"]);
                        Drfinal["COSTPRICEPERCARAT"] = DouCostPricePerCarat;
                        Drfinal["COSTAMOUNT"] = Val.Val(DrPrice["COSTAMOUNT"]) == 0 ? DouCostAmount : Val.Val(DrPrice["COSTAMOUNT"]);


                        Drfinal["SALERAPAPORT"] = Val.Val(DrPrice["SALERAPAPORT"]) == 0 ? Val.Val(Drfinal["COSTRAPAPORT"]) : Val.Val(DrPrice["SALERAPAPORT"]);
                        Drfinal["SALEDISCOUNT"] = Val.Val(DrPrice["SALEDISCOUNT"]) == 0 ? Val.Val(Drfinal["COSTDISCOUNT"]) : Val.Val(DrPrice["SALEDISCOUNT"]);

                        double DouSalePricePerCarat = 0, DouSaleAmount = 0;
                        DouSalePricePerCarat = (Val.Val(Drfinal["SALERAPAPORT"]) + Val.Val(Drfinal["SALERAPAPORT"]) * Val.Val(Drfinal["SALEDISCOUNT"]) / 100);
                        DouSaleAmount = DouSalePricePerCarat * Val.Val(DrPrice["CARAT"]);
                        Drfinal["SALEPRICEPERCARAT"] = DouSalePricePerCarat;
                        Drfinal["SALEAMOUNT"] = DouSaleAmount; // Val.Val(DrPrice["SALEAMOUNT"]) == 0 ? Val.Val(Drfinal["COSTAMOUNT"]) : Val.Val(DrPrice["SALEAMOUNT"]);

                        /*
                           double DouFCostPricePerCarat = 0, DouFCostAmount = 0, DouFSalePricePerCarat = 0, DouFSaleAmount = 0;
                           DouFCostPricePerCarat = Val.Val(DrPrice["FCOSTPRICEPERCARAT"]) == 0 ? Val.Val(Drfinal["COSTPRICEPERCARAT"]) * Val.Val(txtExcRate.Text) : Val.Val(DrPrice["FCOSTPRICEPERCARAT"]);
                           DouFCostAmount = Val.Val(DrPrice["FCOSTAMOUNT"]) == 0 ? Val.Val(Drfinal["COSTAMOUNT"]) * Val.Val(txtExcRate.Text) : Val.Val(DrPrice["FCOSTAMOUNT"]);

                           DouFSalePricePerCarat = Val.Val(DrPrice["FSALEPRICEPERCARAT"]) == 0 ? (Val.Val(Drfinal["SALEPRICEPERCARAT"]) * Val.Val(txtExcRate.Text)) : Val.Val(DrPrice["FSALEPRICEPERCARAT"]);
                           DouFSaleAmount = Val.Val(DrPrice["FSALEAMOUNT"]) == 0 ? (Val.Val(Drfinal["SALEAMOUNT"]) * Val.Val(txtExcRate.Text)) : Val.Val(DrPrice["FSALEAMOUNT"]);

                           Drfinal["FCOSTPRICEPERCARAT"] = Val.Val(DouFCostPricePerCarat);
                           Drfinal["FCOSTAMOUNT"] = Val.Val(DouFCostAmount);
                           Drfinal["FSALEPRICEPERCARAT"] = Val.Val(DouFSalePricePerCarat);
                           Drfinal["FSALEAMOUNT"] = Val.Val(DouFSaleAmount);
                          */
                    }
                    else // For Parcel File :  It's Only Contain PerCarat and Amount 
                    {

                        Drfinal["COSTRAPAPORT"] = Val.Val(DrPrice["COSTRAPAPORT"]);
                        Drfinal["COSTDISCOUNT"] = Val.Val(DrPrice["COSTDISCOUNT"]);
                        Drfinal["COSTPRICEPERCARAT"] = Val.Val(DrPrice["COSTPRICEPERCARAT"]);
                        Drfinal["COSTAMOUNT"] = Val.Val(DrPrice["COSTAMOUNT"]);

                        Drfinal["SALERAPAPORT"] = Val.Val(DrPrice["SALERAPAPORT"]) == 0 ? Val.Val(Drfinal["COSTRAPAPORT"]) : Val.Val(DrPrice["SALERAPAPORT"]);
                        Drfinal["SALEDISCOUNT"] = Val.Val(DrPrice["SALEDISCOUNT"]) == 0 ? Val.Val(Drfinal["COSTDISCOUNT"]) : Val.Val(DrPrice["SALEDISCOUNT"]);
                        Drfinal["SALEPRICEPERCARAT"] = Val.Val(DrPrice["SALEPRICEPERCARAT"]) == 0 ? Val.Val(Drfinal["COSTPRICEPERCARAT"]) : Val.Val(DrPrice["SALEPRICEPERCARAT"]);
                        Drfinal["SALEAMOUNT"] = Val.Val(DrPrice["SALEAMOUNT"]) == 0 ? Val.Val(Drfinal["COSTAMOUNT"]) : Val.Val(DrPrice["SALEAMOUNT"]);

                        /*
                        double DouFCostPricePerCarat = 0, DouFCostAmount = 0, DouFSalePricePerCarat = 0, DouFSaleAmount = 0;
                        DouFCostPricePerCarat = Val.Val(DrPrice["FCOSTPRICEPERCARAT"]) == 0 ? Val.Val(Drfinal["COSTPRICEPERCARAT"]) * Val.Val(txtExcRate.Text) : Val.Val(DrPrice["FCOSTPRICEPERCARAT"]);
                        DouFCostAmount = Val.Val(DrPrice["FCOSTAMOUNT"]) == 0 ? Val.Val(Drfinal["COSTAMOUNT"]) * Val.Val(txtExcRate.Text) : Val.Val(DrPrice["FCOSTAMOUNT"]);

                        DouFSalePricePerCarat = Val.Val(DrPrice["FSALEPRICEPERCARAT"]) == 0 ? (Val.Val(Drfinal["SALEPRICEPERCARAT"]) * Val.Val(txtExcRate.Text)) : Val.Val(DrPrice["FSALEPRICEPERCARAT"]);
                        DouFSaleAmount = Val.Val(DrPrice["FSALEAMOUNT"]) == 0 ? (Val.Val(Drfinal["SALEAMOUNT"]) * Val.Val(txtExcRate.Text)) : Val.Val(DrPrice["FSALEAMOUNT"]);

                        Drfinal["FCOSTPRICEPERCARAT"] = Val.Val(DouFCostPricePerCarat);
                        Drfinal["FCOSTAMOUNT"] = Val.Val(DouFCostAmount);
                        Drfinal["FSALEPRICEPERCARAT"] = Val.Val(DouFSalePricePerCarat);
                        Drfinal["FSALEAMOUNT"] = Val.Val(DouFSaleAmount);
                         */
                    }

                    ////#P : 23-07-2020
                    //var drLBLC = (from DrPara in DtabPara.AsEnumerable()
                    //             where Val.ToString(DrPara["PARANAME"]).ToUpper() == Val.ToString(DrPrice["LBLCNAME"]).Trim().ToUpper() && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "LBLC"
                    //             select DrPara);
                    //Drfinal["LBLC_ID"] = drLBLC.Count() > 0 ? Val.ToInt32(drLBLC.FirstOrDefault()["PARA_ID"]) : 0;

                    //var drNatts = (from DrPara in DtabPara.AsEnumerable()
                    //              where Val.ToString(DrPara["LABCODE"]).ToUpper().Split(',').Contains(Val.ToString(DrPrice["NATTSNAME"]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "NATTS"
                    //              select DrPara);
                    //Drfinal["NATTS_ID"] = drNatts.Count() > 0 ? Val.ToInt32(drNatts.FirstOrDefault()["PARA_ID"]) : 0;

                    //var drTension = (from DrPara in DtabPara.AsEnumerable()
                    //                 where Val.ToString(DrPara["LABCODE"]).ToUpper().Split(',').Contains(Val.ToString(DrPrice["TENSIONNAME"]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "TENSION"
                    //               select DrPara);
                    //Drfinal["TENSION_ID"] = drTension.Count() > 0 ? Val.ToInt32(drTension.FirstOrDefault()["PARA_ID"]) : 0;

                    //var drBInC = (from DrPara in DtabPara.AsEnumerable()
                    //                 where Val.ToString(DrPara["PARACODE"]).ToUpper() == Val.ToString(DrPrice["BINCNAME"]).Trim().ToUpper() && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "BLACK"
                    //                 select DrPara);
                    //Drfinal["BLACKINC_ID"] = drBInC.Count() > 0 ? Val.ToInt32(drBInC.FirstOrDefault()["PARA_ID"]) : 0;

                    //var drWInC = (from DrPara in DtabPara.AsEnumerable()
                    //              where Val.ToString(DrPara["PARACODE"]).ToUpper() == Val.ToString(DrPrice["WINCNAME"]).Trim().ToUpper() && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "WHITE"
                    //              select DrPara);
                    //Drfinal["WHITEINC_ID"] = drWInC.Count() > 0 ? Val.ToInt32(drWInC.FirstOrDefault()["PARA_ID"]) : 0;

                    //var drOInC = (from DrPara in DtabPara.AsEnumerable()
                    //              where Val.ToString(DrPara["PARACODE"]).ToUpper() == Val.ToString(DrPrice["OINCNAME"]).Trim().ToUpper() && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "OPEN"
                    //              select DrPara);
                    //Drfinal["OPENINC_ID"] = drOInC.Count() > 0 ? Val.ToInt32(drOInC.FirstOrDefault()["PARA_ID"]) : 0;

                    //var drLuster = (from DrPara in DtabPara.AsEnumerable()
                    //                where Val.ToString(DrPara["PARANAME"]).ToUpper() == Val.ToString(DrPrice["LUSTERNAME"]).Trim().ToUpper() && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "LUSTER"
                    //              select DrPara);
                    //Drfinal["LUSTER_ID"] = drLuster.Count() > 0 ? Val.ToInt32(drLuster.FirstOrDefault()["PARA_ID"]) : 0;

                    //var drHA = (from DrPara in DtabPara.AsEnumerable()
                    //            where Val.ToString(DrPara["PARANAME"]).ToUpper() == Val.ToString(DrPrice["HANAME"]).Trim().ToUpper() && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "HEARTANDARROW"
                    //                select DrPara);
                    //Drfinal["HA_ID"] = drHA.Count() > 0 ? Val.ToInt32(drHA.FirstOrDefault()["PARA_ID"]) : 0;

                    //var drPav = (from DrPara in DtabPara.AsEnumerable()
                    //             where Val.ToString(DrPara["PARACODE"]).ToUpper() == Val.ToString(DrPrice["PAVNAME"]).Trim().ToUpper() && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "PAVALION"
                    //            select DrPara);
                    //Drfinal["PAV_ID"] = drPav.Count() > 0 ? Val.ToInt32(drPav.FirstOrDefault()["PARA_ID"]) : 0;

                    //var drNatural = (from DrPara in DtabPara.AsEnumerable()
                    //                 where Val.ToString(DrPara["LABCODE"]).ToUpper().Split(',').Contains(Val.ToString(DrPrice["NATURALNAME"]).Trim().ToUpper()) && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "NATURAL"
                    //             select DrPara);
                    //Drfinal["NATURAL_ID"] = drNatural.Count() > 0 ? Val.ToInt32(drNatural.FirstOrDefault()["PARA_ID"]) : 0;

                    //var drGrain = (from DrPara in DtabPara.AsEnumerable()
                    //               where Val.ToString(DrPara["PARANAME"]).ToUpper() == Val.ToString(DrPrice["GRAINNAME"]).Trim().ToUpper() && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "GRAIN"
                    //                 select DrPara);
                    //Drfinal["GRAIN_ID"] = drGrain.Count() > 0 ? Val.ToInt32(drGrain.FirstOrDefault()["PARA_ID"]) : 0;

                    //Drfinal["LABPROCESS"] = Val.ToString(DrPrice["LABPROCESS"]);
                    //Drfinal["LABSELECTION"] = Val.ToString(DrPrice["LABSELECTION"]);
                    //Drfinal["ISNOBGM"] = Val.ToInt32(DrPrice["ISNOBGM"]);
                    //Drfinal["ISNOBLACK"] = Val.ToInt32(DrPrice["ISNOBLACK"]);
                    //Drfinal["FLUORESCENCECOLOR"] = Val.ToString(DrPrice["FLUORESCENCECOLOR"]);
                    //Drfinal["COLORDESC"] = Val.ToString(DrPrice["COLORDESC"]);
                    //Drfinal["GIRDLECONDITION"] = Val.ToString(DrPrice["GIRDLECONDITION"]);
                    //Drfinal["STARLENGTH"] = Val.ToString(DrPrice["STARLENGTH"]);
                    //Drfinal["LOWERHALF"] = Val.ToString(DrPrice["LOWERHALF"]);
                    //Drfinal["PAINTING"] = Val.ToString(DrPrice["PAINTING"]);
                    //Drfinal["PROPORTIONS"] = Val.ToString(DrPrice["PROPORTIONS"]);
                    //Drfinal["PAINTCOMM"] = Val.ToString(DrPrice["PAINTCOMM"]);
                    //Drfinal["REPORTCOMMENT"] = Val.ToString(DrPrice["REPORTCOMMENT"]);
                    //Drfinal["INSCRIPTION"] = Val.ToString(DrPrice["INSCRIPTION"]);
                    //Drfinal["SYNTHETICINDICATOR"] = Val.ToString(DrPrice["SYNTHETICINDICATOR"]);
                    //Drfinal["POLISHFEATURES"] = Val.ToString(DrPrice["POLISHFEATURES"]);
                    //Drfinal["SYMMETRYFEATURES"] = Val.ToString(DrPrice["SYMMETRYFEATURES"]);
                    //Drfinal["JOBNO"] = Val.ToString(DrPrice["JOBNO"]);
                    //Drfinal["ISFANCY"] = Val.ToInt32(DrPrice["ISFANCY"]);
                    //Drfinal["FANCYCOLOR"] = Val.ToString(DrPrice["FANCYCOLOR"]);
                    //Drfinal["FANCYCOLORINTENSITY"] = Val.ToString(DrPrice["FANCYCOLORINTENSITY"]);
                    //Drfinal["FANCYCOLOROVERTONE"] = Val.ToString(DrPrice["FANCYCOLOROVERTONE"]);
                    //Drfinal["EXPORTORDERNO"] = Val.ToString(DrPrice["EXPORTORDERNO"]);

                    //Kuldeep For Inserting FE Amount
                    double DouFCostPricePerCarat = 0, DouFCostAmount = 0, DouFSalePricePerCarat = 0, DouFSaleAmount = 0;
                    DouFCostPricePerCarat = Val.Val(DrPrice["FCOSTPRICEPERCARAT"]) == 0 ? Val.Val(Drfinal["COSTPRICEPERCARAT"]) * Val.Val(txtExcRate.Text) : Val.Val(DrPrice["FCOSTPRICEPERCARAT"]);
                    DouFCostAmount = Val.Val(DrPrice["FCOSTAMOUNT"]) == 0 ? Val.Val(Drfinal["COSTAMOUNT"]) * Val.Val(txtExcRate.Text) : Val.Val(DrPrice["FCOSTAMOUNT"]);

                    DouFSalePricePerCarat = Val.Val(DrPrice["FSALEPRICEPERCARAT"]) == 0 ? (Val.Val(Drfinal["SALEPRICEPERCARAT"]) * Val.Val(txtExcRate.Text)) : Val.Val(DrPrice["FSALEPRICEPERCARAT"]);
                    DouFSaleAmount = Val.Val(DrPrice["FSALEAMOUNT"]) == 0 ? (Val.Val(Drfinal["SALEAMOUNT"]) * Val.Val(txtExcRate.Text)) : Val.Val(DrPrice["FSALEAMOUNT"]);

                    Drfinal["FCOSTPRICEPERCARAT"] = Val.Val(DouFCostPricePerCarat);
                    Drfinal["FCOSTAMOUNT"] = Val.Val(DouFCostAmount);
                    Drfinal["FSALEPRICEPERCARAT"] = Val.Val(DouFSalePricePerCarat);
                    Drfinal["FSALEAMOUNT"] = Val.Val(DouFSaleAmount);


                    Drfinal["EXPRAPAPORT"] = Val.Val(DrPrice["EXPRAPAPORT"]);
                    Drfinal["EXPDISCOUNT"] = Val.Val(DrPrice["EXPDISCOUNT"]);
                    Drfinal["EXPPRICEPERCARAT"] = Val.Val(DrPrice["EXPPRICEPERCARAT"]);
                    Drfinal["EXPAMOUNT"] = Val.Val(DrPrice["EXPAMOUNT"]);
                    //Drfinal["RAPNETRAPAPORT"] = Val.Val(DrPrice["RAPNETRAPAPORT"]);
                    //Drfinal["RAPNETDISCOUNT"] = Val.Val(DrPrice["RAPNETDISCOUNT"]);
                    //Drfinal["RAPNETPRICEPERCARAT"] = Val.Val(DrPrice["RAPNETPRICEPERCARAT"]);
                    //Drfinal["RAPNETAMOUNT"] = Val.Val(DrPrice["RAPNETAMOUNT"]);

                    string ptyname = Val.ToString(DrPrice["PARTYNAME"]).Trim().ToUpper();
                    var drParty = (from DrSupplier in DtabSupplier.AsEnumerable()
                                   where Val.ToString(DrSupplier["PARTYNAME"]).ToUpper() == ptyname
                                   select DrSupplier);

                    Drfinal["PARTY_ID"] = drParty.Count() > 0 ? Guid.Parse(Val.ToString(drParty.FirstOrDefault()["PARTY_ID"])) : Guid.Parse(Val.ToString(txtPartyName.Tag));
                    //Drfinal["PARTY_ID"] =  Guid.Parse(Val.ToString(txtPartyName.Tag));
                    Drfinal["UPLOADDATE"] = DrPrice["UPLOADDATE"];
                    Drfinal["REMARK"] = DrPrice["REMARK"];

                    DtabFinalData.Rows.Add(Drfinal);
                }

                string StrStockUploadType = "";
                Guid gStrParyt_ID;

                if (RbtAppendStock.Checked) StrStockUploadType = "APPEND";
                else if (RbtReplaceAllStock.Checked) StrStockUploadType = "REPLACE";

                gStrParyt_ID = Val.ToString(txtPartyName.Tag).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtPartyName.Tag));

                DtabFinalData.TableName = "DETAIL";

                string StockUploadXML;
                using (StringWriter sw = new StringWriter())
                {
                    DtabFinalData.WriteXml(sw);
                    StockUploadXML = sw.ToString();
                }

                DtabStockUpload = ObjStock.SaveStockUploadUsingDataTable(StockUploadXML, Val.ToString(CmbStockStatus.SelectedItem), Val.ToString(StrStockUploadType), gStrParyt_ID);

                if (DtabStockUpload.Rows.Count > 0)
                {
                    lblMessage.Visible = true;
                    lblMessage.Text = "Stock Successfully Uploaded";
                }

                GrdDetStock.BeginUpdate();

                MainGrdStock.DataSource = DtabStockUpload;
                MainGrdStock.RefreshDataSource();
                GrdDetStock.BestFitColumns();
                BtnCalculate.Enabled = false;

                GrdDetStock.EndUpdate();

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
                this.Cursor = Cursors.Default;
            }
        }

        private void BtnDelStonePricing_Click(object sender, EventArgs e)
        {
            try
            {

                //this.Cursor = Cursors.WaitCursor;
                //if ((mTrn_ID) == Guid.Empty)
                //{
                //    return;
                //}

                //if (Global.Confirm("Are You Sure You Want To Delete This Stone Pricing ?") == System.Windows.Forms.DialogResult.No)
                //    return;

                //StonePricingProperty pClsProperty = new StonePricingProperty();
                //pClsProperty.TRN_ID = Guid.Parse(Val.ToString(mTrn_ID));
                //pClsProperty.SELLER_ID = Val.ToInt64(IntMainParty_ID);
                ////pClsProperty = ObjStock.DeleteStonePricing(pClsProperty);

                //Global.Message(pClsProperty.ReturnMessageDesc);

                //if (pClsProperty.ReturnMessageType == "SUCCESS")
                //{
                //    Clear();
                //}

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void GrdDetPricing_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            if (e.SummaryProcess == CustomSummaryProcess.Start)
            {
                DouCarat = 0;
                DouFileRap = 0;
                DouFileRapAmount = 0;
                DouFileBack = 0;
                DouFilePricePerCarat = 0;
                DouFileAmount = 0;


                DouCompRap = 0;
                DouCompRapAmount = 0;
                DouCompBack = 0;
                DouCompPricePerCarat = 0;
                DouCompAmount = 0;

            }
            else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
            {
                DouCarat = DouCarat + Val.Val(GrdDetStock.GetRowCellValue(e.RowHandle, "CARAT"));
                DouFileRapAmount = DouFileRapAmount + Val.Val(GrdDetStock.GetRowCellValue(e.RowHandle, "FILERAPAMT"));
                DouFileAmount = DouFileAmount + Val.Val(GrdDetStock.GetRowCellValue(e.RowHandle, "FILEAMOUNT"));

                DouCompRapAmount = DouCompRapAmount + Val.Val(GrdDetStock.GetRowCellValue(e.RowHandle, "COMPRAPAMT"));
                DouCompAmount = DouCompAmount + Val.Val(GrdDetStock.GetRowCellValue(e.RowHandle, "COMPAMOUNT"));

            }

            else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
            {
                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("FILERAP") == 0)
                {
                    DouFileRap = Math.Round(DouFileRapAmount / DouCarat, 2);
                    //e.TotalValue = Val.Format(DouFileRap, "#########0.00");
                    e.TotalValue = Math.Round(Val.Val(DouFileRap), 2);
                }
                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("FILEPRICEPERCARAT") == 0)
                {
                    DouFilePricePerCarat = Math.Round(DouFileAmount / DouCarat, 2);
                    e.TotalValue = Val.Val(DouFilePricePerCarat);
                    //e.TotalValue = DouFilePricePerCarat;
                }
                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("FILEBACK") == 0)
                {
                    e.TotalValue = Val.Val(Math.Round(((DouFileAmount) - Val.Val(DouFileRapAmount)) / Val.Val(DouFileRapAmount) * (100), 2));
                }


                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("COMPRAP") == 0)
                {
                    DouCompRap = Math.Round(DouCompRapAmount / DouCarat, 2);
                    e.TotalValue = Val.Val(DouCompRap);

                }
                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("COMPPRICEPERCARAT") == 0)
                {
                    DouCompPricePerCarat = Math.Round(DouCompAmount / DouCarat, 2);
                    e.TotalValue = Val.Val(DouCompPricePerCarat);
                }
                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("COMPBACK") == 0)
                {
                    e.TotalValue = Val.Val(Math.Round(((DouCompAmount) - Val.Val(DouCompRapAmount)) / Val.Val(DouCompRapAmount) * (100), 2));
                }

                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("DIFFAMOUNT") == 0)
                {
                    e.TotalValue = Val.Val(Math.Round(((DouCompAmount) - Val.Val(DouFileAmount)), 2));
                }
                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("DIFFBACKPER") == 0)
                {
                    DouCompRap = Math.Round(DouCompRapAmount / DouCarat, 2);
                    DouCompPricePerCarat = Math.Round(DouCompAmount / DouCarat, 2);
                    DouCompBack = Math.Round(((DouCompPricePerCarat) - Val.Val(DouCompRap)) / Val.Val(DouCompRap) * (100), 2);

                    DouFilePricePerCarat = Math.Round(DouFileAmount / DouCarat, 2);
                    DouFileRap = Math.Round(DouFileRapAmount / DouCarat, 2);
                    DouFileBack = Math.Round(((DouFilePricePerCarat) - Val.Val(DouFileRap)) / Val.Val(DouFileRap) * (100), 2);

                    e.TotalValue = Val.Val(Math.Round((DouCompBack) - Val.Val(DouFileBack), 2));
                }
            }
        }

        private void ChkBestFit_CheckedChanged(object sender, EventArgs e)
        {
            GrdDetStock.BestFitColumns();
        }

        private void FrmPricing_KeyDown(object sender, KeyEventArgs e)
        {
        }

        private void lblSampleExcelFile_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "\\Format\\EmployeeSampleFile.xlsx", "CMD");
            string StrFilePathDestination = "";

            if (Val.ToString(CmbStockType.SelectedItem) == "PARCEL")
            {
                StrFilePathDestination = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Parcel_FileUploadFixFormat_" + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.Day.ToString() + ".xlsx";
                if (File.Exists(StrFilePathDestination))
                {
                    File.Delete(StrFilePathDestination);
                }
                File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\Format\\Parcel_FileUploadFixFormat.xlsx", StrFilePathDestination);
            }
            else
            {
                StrFilePathDestination = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\Single_FileUploadFixFormat_" + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.Day.ToString() + ".xlsx";
                if (File.Exists(StrFilePathDestination))
                {
                    File.Delete(StrFilePathDestination);
                }
                File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\Format\\Single_FileUploadFixFormat.xlsx", StrFilePathDestination);
            }
            System.Diagnostics.Process.Start(StrFilePathDestination, "CMD");
        }

        private void GrdDetPricing_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {

        }

        private void BtnBestFit_Click(object sender, EventArgs e)
        {
            GrdDetStock.BestFitColumns();
        }

        private void GrdDetPricing_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }
                string StrStatus = Val.ToString(GrdDetStock.GetRowCellValue(e.RowHandle, "STATUS")).ToUpper();

                //if (StrStatus == "CONFIRM")
                //{
                //    e.Appearance.BackColor = lblUp.BackColor;
                //}
                //if (StrStatus == "NOT CONFIRM")
                //{
                //    e.Appearance.BackColor = lblDown.BackColor;
                //}
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnUpdatePrice_Click(object sender, EventArgs e)
        {
            //try
            //{

            //    this.Cursor = Cursors.WaitCursor;
            //    DataRow[] drconfirm = DtabStockUpload.Select("STATUS = '" + Val.ToString("confirm").ToUpper() + "'");

            //    if (drconfirm.Length <= 0)
            //    {
            //        this.Cursor = Cursors.Default;
            //        Global.Message("Confirm Stone's Are Not Found In This File.");
            //        return;
            //    }

            //    //DataTable DtabConfirm = drconfirm.CopyToDataTable();

            //    DataTable DtConfirm = drconfirm.CopyToDataTable().DefaultView.ToTable(true, "SHAPE_ID", "CARAT", "COLOR_ID", "CLARITY_ID", "CUT_ID", "POL_ID", "SYM_ID", "FL_ID", "FILEBACK", "COMPBACK");

            //    StonePricingProperty PricingProperty = new StonePricingProperty();

            //    PricingProperty = ObjStock.UpdateConfirmStoneBackInPriceMatrix(DtConfirm, PricingProperty);

            //    if (PricingProperty.ReturnMessageType == "SUCCESS")
            //    {
            //        Global.Message("File Back Updated Successfully In Matrix");
            //    }
            //    this.Cursor = Cursors.Default;
            //}
            //catch (Exception ex)
            //{
            //    this.Cursor = Cursors.Default;
            //    Global.Message(ex.Message.ToString());
            //}
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
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PURCHASEPARTY);

                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtPartyName.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtPartyName.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
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

        private void CmbStockType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (Val.ToString(CmbStockType.SelectedItem).Trim().ToUpper().Equals("PARCEL"))
            //    GrpExcRateDetail.Visible = true;
            //else
            //    GrpExcRateDetail.Visible = false;

            //CmbCurrency.SelectedIndex = 0;

            CmbCurrency.SelectedValue = "2";
        }

        private void CmbCurrency_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int IntCurrency_ID = 0;
                IntCurrency_ID = Val.ToInt(CmbCurrency.SelectedValue);
                double DouExcRate = 0;

                if (Val.ToInt(IntCurrency_ID) == 0)
                    return;

                DataRow[] Dr = DtabExcRateCurrencyWise.Select("CURRENCY_ID = " + Val.ToString(IntCurrency_ID));

                if (Dr.Length > 0)
                    DouExcRate = Val.Val(Dr[0]["EXCRATE"]);
                else
                    DouExcRate = 0.00;

                txtExcRate.Text = string.Format("{0:0.00}", DouExcRate);

                //CmbCurrency.SelectedValue = "2";
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void CmbStockStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbStockStatus.SelectedItem == "PURCHASE")
            {
                RbtReplaceAllStock.Checked = true;
                RbtReplaceAllStock.Enabled = false;
                RbtAppendStock.Enabled = false;
            }
            else
            {
                RbtReplaceAllStock.Enabled = true;
                RbtAppendStock.Enabled = true;
            }
        }

        private void BtnStockSync_Click(object sender, EventArgs e)
        {
            
        }

        private void BtnVerified_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable DTabVerfied = GetTableOfSelectedRows(GrdDetStock, true);
                if (DTabVerfied.Rows.Count <= 0)
                {
                    Global.Message("Please Select Records That You Want To Return..");
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                DTabVerfied.TableName = "Table1";

                string StockSyncVerifiedDetailForXml = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabVerfied.WriteXml(sw);
                    StockSyncVerifiedDetailForXml = sw.ToString();
                }

                DataTable DtabVarified = ObjStock.MFGStockVerifiedBeforeSave(StockSyncVerifiedDetailForXml);

                if (DtabVarified.Rows.Count > 0)
                {
                    MainGrdIvalidStone.DataSource = DtabVarified;
                    Global.Message("Some Stone Parameter Are Not Matched, Check Invalid Stone Details");
                    GrdIvalidStone.Focus();
                    this.Cursor = Cursors.Default;
                    return;
                }

                DataTable Dtab = ObjStock.MFGStockSyncSave(StockSyncVerifiedDetailForXml);

                if (Dtab.Rows.Count > 0)
                {
                    Global.Message("Stock Verified Successfully.");
                    DtabStockSync.Rows.Clear();
                }
                else
                {
                    Global.Message("Oops.. Something Goes Wrong.");
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }
        private DataTable GetTableOfSelectedRows(GridView view, Boolean IsSelect)
        {
            if (view.RowCount <= 0)
            {
                return null;
            }
            ArrayList aryLst = new ArrayList();


            DataTable resultTable = new DataTable();
            DataTable sourceTable = null;
            sourceTable = ((DataView)view.DataSource).Table;

            if (IsSelect)
            {
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

            return resultTable;
        }

        private void txtJangedNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String str1 = txtJangedNo.Text.Trim().Replace("\r\n", ",");
                txtJangedNo.Text = str1;
                txtJangedNo.Select(txtJangedNo.Text.Length, 0);

                bool IsFound = false;
                string[] Str = str1.Split(',');
                //foreach (string StrPacket in str1.Split(','))
                //{
                //    IEnumerable<DataRow> rowsNew = DtabLiveStockDetail.Rows.Cast<DataRow>();
                //    if (rowsNew.Where(s => Val.ToString(s["STOCKNO"]) == StrPacket).Count() > 0)
                //        IsFound = true;
                //    rowsNew.Where(s => Val.ToString(s["STOCKNO"]) == StrPacket).ToList().ForEach(r => r.SetField("SEL", true));

                //}
                if (IsFound)
                {
                    DtabStockSync.DefaultView.Sort = "SEL DESC";
                    DtabStockSync = DtabStockSync.DefaultView.ToTable();
                    MainGrdStock.DataSource = DtabStockSync;
                    MainGrdStock.RefreshDataSource();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDateWiseSum_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
        {
            //try
            //{

            //    if (e.FocusedRowHandle < 0)
            //    {
            //        return;
            //    }
            //    this.Cursor = Cursors.WaitCursor;

            //    this.Cursor = Cursors.WaitCursor;
            //    GrdDetStock.Columns["TRANSDATE"].ClearFilter();
            //    GrdDetStock.Columns["TRANSDATE"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo("TRANSDATE ='" + Val.ToString(GrdDateWiseSum.GetFocusedRowCellValue("TRANSDATE")) + "'");
            //    this.Cursor = Cursors.Default;

            //    this.Cursor = Cursors.Default;

            //}
            //catch (Exception ex)
            //{
            //    this.Cursor = Cursors.Default;
            //}
        }

        private void BtnClearFilter_Click(object sender, EventArgs e)
        {
            try
            {
                //GrdDetStock.Columns["TRANSDATE"].ClearFilter();
                GrdDetStock.ClearColumnsFilter();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDateWiseSum_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {

                if (e.RowHandle < 0 || e.Clicks < 2)
                {
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                this.Cursor = Cursors.WaitCursor;
                GrdDetStock.Columns["TRANSDATE"].ClearFilter();
                GrdDetStock.Columns["TRANSDATE"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo("TRANSDATE ='" + Val.ToString(GrdDateWiseSum.GetFocusedRowCellValue("TRANSDATE")) + "'");
                this.Cursor = Cursors.Default;

                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
            }
        }
        public void CalculateTotalSummary()
        {
            try
            {
                double DCCarat = 0;
                int DCPcs = 0;
                double DCRapaport = 0;
                double DCRapaportAmt = 0;
                double DCDiscount = 0;
                DtabStockSync.AcceptChanges();

                foreach (DataRow DRow in DtabStockSync.Rows)
                {
                    DCRapaportAmt += Val.Val(DRow["COSTRAPAPORT"]) * Val.Val(DRow["CARAT"]);
                }

                DCPcs = Val.ToInt32(DtabStockSync.Compute("SUM(PCS)", string.Empty));
                DCCarat = Val.Val(DtabStockSync.Compute("SUM(CARAT)", string.Empty));

                txtTotalCarat.Text = string.Format("{0:0.00}", DCCarat, string.Empty);
                txtTotalPcs.Text = string.Format("{0:0}", DCPcs, string.Empty);

                txtTotalAmount.Text = string.Format("{0:0.00}", DtabStockSync.Compute("SUM(COSTAMOUNT)", string.Empty));
                txtTotalPricePerCarat.Text = string.Format("{0:0.00}", Val.Val(txtTotalAmount.Text) / Val.Val(DCCarat));

                if (DCCarat > 0)
                {
                    DCRapaport = Math.Round(DCRapaportAmt / Val.Val(DCCarat), 4);
                    //DCDiscount = Val.Val(DCRapaport) == 0 ? 0 : Math.Round((Val.Val(txtCostAvgPerCts.Text) - DCRapaport) / DCRapaport * 100, 2);
                    DCDiscount = Val.Val(DCRapaport) == 0 ? 0 : Math.Round((DCRapaport - Val.Val(txtTotalPricePerCarat.Text)) / DCRapaport * 100, 2); //#P
                }
                txtTotalDisc.Text = string.Format("{0:0.00}", DCDiscount, string.Empty);
                txtTotalAvgRap.Text = string.Format("{0:0.00}", DCRapaport, string.Empty);
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        public void CalculateSelectedSummary()
        {
            double DblSelectedRapAmount = 0.00;
            double DblSelectedRapaport = 0.00;
            double DblSelectedDiscount = 0.00;

            DataTable DTab = GetTableOfSelectedRows(GrdDetStock, true);

            if (DTab != null && DTab.Rows.Count > 0)
            {
                txtSelectedPcs.Text = Val.ToString(DTab.Compute("SUM(PCS)", string.Empty));
                txtSelectedCarat.Text = Val.ToString(DTab.Compute("SUM(CARAT)", string.Empty));
                txtSelectedAmount.Text = Val.ToString(DTab.Compute("SUM(COSTAMOUNT)", string.Empty));
                txtSelectedPricePerCarat.Text = string.Format("{0:0.00}", Val.Val(txtSelectedAmount.Text) / Val.Val(txtSelectedCarat.Text));

                foreach (DataRow DRow in DTab.Rows)
                {
                    DblSelectedRapAmount += Val.Val(DRow["COSTRAPAPORT"]) * Val.Val(DRow["CARAT"]);
                }

                DblSelectedRapaport = DblSelectedRapAmount / Val.Val(txtSelectedCarat.Text);
                DblSelectedDiscount = (Val.Val(txtSelectedPricePerCarat.Text) - Math.Round(DblSelectedRapaport, 2)) / Math.Round(DblSelectedRapaport, 2) * -100;
                txtSelectedDisc.Text = string.Format("{0:0.00}", DblSelectedDiscount, string.Empty);
                txtSelectedAvgRap.Text = string.Format("{0:0.00}", DblSelectedRapaport, string.Empty);
            }
            else
            {
                txtSelectedPcs.Text = string.Empty;
                txtSelectedCarat.Text = string.Empty;
                txtSelectedDisc.Text = string.Empty;
                txtSelectedAmount.Text = string.Empty;
                txtSelectedAvgRap.Text = string.Empty;
                txtSelectedPricePerCarat.Text = string.Empty;
            }

        }

        private void GrdDetStock_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                CalculateSelectedSummary();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void FrmStockUploadParcel_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                CalculateSelectedSummary();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
    }
}
