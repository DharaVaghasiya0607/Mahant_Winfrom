using MahantExport.Masters;
using MahantExport.Utility;
using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.Rapaport;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using Google.API.Translate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MahantExport.Stock
{
    public partial class FrmStoneHistory : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFindRap ObjRap = new BOFindRap();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        BOTRN_StockUploadParcel ObjStockParcel = new BOTRN_StockUploadParcel();
        DataTable DtabPara = new DataTable();
        FORMTYPE mFormType = FORMTYPE.DISPLAY;

        double DouSaleAmount = 0;
        double DouSalePricePerCarat = 0;

        double DouCostAmount = 0;
        double DouCostPricePerCarat = 0;

        double DouSuratAmount = 0;
        double DouSuratPricePerCarat = 0;

        double DouRapnetAmount = 0;
        double DouRapnetPricePerCarat = 0;

        double DouJAAmount = 0;
        double DouJAPricePerCarat = 0;

        double DouExpAmount = 0;
        double DouExpPricePerCarat = 0;

        public enum FORMTYPE
        {
            DISPLAY = 0,
            UPDATE = 1
        }

        #region Property Settings

        public FrmStoneHistory()
        {
            InitializeComponent();
        }

        public void ShowForm(string pStrStockId, string pStrStockNo, FORMTYPE pFormType)
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            this.Show();

            BtnUpdate.Enabled = false;
            txtStoneNo.Tag = pStrStockId;
            txtStoneNo.Text = pStrStockNo;

            mFormType = pFormType;

            GrpCost.Visible = BusLib.Configuration.BOConfiguration.gEmployeeProperty.ISDISPLAYCOSTPRICE;
            GrpSurat.Visible = BusLib.Configuration.BOConfiguration.gEmployeeProperty.ISDISPLAYALLMFGCOST;

            //if (mFormType == FORMTYPE.UPDATE)
            //{
            //    DtabPara = new BOMST_Parameter().GetParameterData();
            //}

            if (mFormType == FORMTYPE.UPDATE)
            {
                BtnUpdate.Visible = true;
            }
            else
            {
                BtnUpdate.Visible = false;
            }

            FillListControls();
            BtnSearch_Click(null, null);

        }

        public void FillListControls()
        {
            DtabPara = new BOMST_Parameter().GetParameterData();
            DataTable DTab = new DataTable();
            DTab = DtabPara.Select("PARATYPE = 'TABLEINC'").CopyToDataTable();
            CmbTableInc.Properties.DataSource = DTab;
            CmbTableInc.Properties.DisplayMember = "SHORTNAME";
            CmbTableInc.Properties.ValueMember = "PARA_ID";

            RepCmbTableInc.DataSource = DTab;
            RepCmbTableInc.DisplayMember = "SHORTNAME";
            RepCmbTableInc.ValueMember = "PARA_ID";

            DTab = DtabPara.Select("PARATYPE = 'SIDETABLEINC'").CopyToDataTable();
            CmbSideTable.Properties.DataSource = DTab;
            CmbSideTable.Properties.DisplayMember = "SHORTNAME";
            CmbSideTable.Properties.ValueMember = "PARA_ID";

            RepCmbSideTable.DataSource = DTab;
            RepCmbSideTable.DisplayMember = "SHORTNAME";
            RepCmbSideTable.ValueMember = "PARA_ID";
        }

        public void ShowForm(FORMTYPE pFormType)
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            mFormType = pFormType;

            this.Show();
            BtnUpdate.Enabled = false;
            GrpCost.Visible = BusLib.Configuration.BOConfiguration.gEmployeeProperty.ISDISPLAYCOSTPRICE;
            GrpSurat.Visible = BusLib.Configuration.BOConfiguration.gEmployeeProperty.ISDISPLAYALLMFGCOST;
            GrpComputerPrice.Visible = BusLib.Configuration.BOConfiguration.gEmployeeProperty.ISCOMPUTERPRICE;

            //if (mFormType == FORMTYPE.UPDATE)
            //{
            //    DtabPara = new BOMST_Parameter().GetParameterData();
            //}

            txtStoneNo.Focus();

            if (mFormType == FORMTYPE.UPDATE)
            {
                BtnUpdate.Visible = true;
            }
            else
            {
                BtnUpdate.Visible = false;
            }
            FillListControls();
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
            ObjFormEvent.ObjToDisposeList.Add(ObjStock);
        }

        #endregion

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport(lblStockNo.Text + ".xlsx", GrdHistory);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GrdHistory_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                return;
            }
            if (e.Clicks == 2 && e.Column.FieldName == "JANGEDNOSTR")
            {
                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                FrmMemoEntry.ShowForm(Val.ToString(GrdHistory.GetRowCellValue(e.RowHandle, "MEMO_ID")), "SINGLE");

                ////HINA - START
                ////FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                ////FrmMemoEntry.MdiParent = Global.gMainRef;
                ////FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                ////FrmMemoEntry.ShowForm(Val.ToString(GrdHistory.GetRowCellValue(e.RowHandle, "MEMO_ID")));

                ////KULDEEP START 28082020
                //string strStockType = Val.ToString(GrdHistory.GetRowCellValue(0, "STOCKTYPE"));
                //if (strStockType == "SINGLE")
                //{

                //}
                //else
                //{
                //    FrmMemoEntryParcel FrmMemoEntryParcel = new FrmMemoEntryParcel();
                //    FrmMemoEntryParcel.MdiParent = Global.gMainRef;
                //    FrmMemoEntryParcel.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                //    FrmMemoEntryParcel.ShowForm(Val.ToString(GrdHistory.GetRowCellValue(e.RowHandle, "MEMO_ID")), Val.ToString(CardViewDet.GetRowCellValue(0, "STOCKTYPE")));

                //}
                //// KULDEEP END
                ////HINA - END
            }
        }

        private void FrmMemoEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            BtnSearch.PerformClick();
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;


            if (txtStoneNo.Text.Trim().Length == 0)
            {
                Global.Message("Stone No Is Required");
                txtStoneNo.Focus();
                return;
            }
            Guid StockID = Val.ToString(txtStoneNo.Tag).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtStoneNo.Tag));

            DataSet DS = ObjStock.GetStoneHistoryData(StockID);

            DataTable DTabStockStatment = ObjStockParcel.GetDataForStockStatment(Val.ToString(txtStoneNo.Tag));

            if (DS.Tables.Count == 0)
            {
                Global.Message("Invalid Stone Found");
                return;
            }
            //if (DS.Tables[0].Rows.Count == 0)
            //{
            //    Global.Message("No Data Found For This Stone");
            //    return;
            //}


            lblStockNo.Tag = Val.ToString(StockID);
            BtnUpdate.Enabled = true;

            if (DS.Tables[0].Rows.Count > 0)
            {

                DataRow DRow = DS.Tables[0].Rows[0];

                txtShape.Text = Val.ToString(DRow["SHAPENAME"]);
                txtShape.Tag = Val.ToString(DRow["SHAPE_ID"]);

                txtColor.Text = Val.ToString(DRow["COLORNAME"]);
                txtColor.Tag = Val.ToString(DRow["COLOR_ID"]);

                txtCarat.Text = Val.ToString(DRow["CARAT"]);

                txtClarity.Text = Val.ToString(DRow["CLARITYNAME"]);
                txtClarity.Tag = Val.ToString(DRow["CLARITY_ID"]);

                txtCut.Text = Val.ToString(DRow["CUTNAME"]);
                txtCut.Tag = Val.ToString(DRow["CUT_ID"]);

                txtPol.Text = Val.ToString(DRow["POLNAME"]);
                txtPol.Tag = Val.ToString(DRow["POL_ID"]);

                txtFL.Text = Val.ToString(DRow["FLNAME"]);
                txtFL.Tag = Val.ToString(DRow["FL_ID"]);

                txtFLColor.Text = Val.ToString(DRow["FLUORESCENCECOLOR"]);

                txtSym.Text = Val.ToString(DRow["SYMNAME"]);
                txtSym.Tag = Val.ToString(DRow["SYM_ID"]);

                txtColorShade.Text = Val.ToString(DRow["COLORSHADENAME"]);
                txtColorShade.Tag = Val.ToString(DRow["COLORSHADE_ID"]);

                txtSize.Text = Val.ToString(DRow["SIZENAME"]);
                txtSize.Tag = Val.ToString(DRow["SIZE_ID"]);

                txtLab.Text = Val.ToString(DRow["LABNAME"]);
                txtLab.Tag = Val.ToString(DRow["LAB_ID"]);

                txtLocation.Text = Val.ToString(DRow["LOCATIONNAME"]);
                txtLocation.Tag = Val.ToString(DRow["LOCATION_ID"]);

                txtReportNo.Text = Val.ToString(DRow["LABREPORTNO"]);

                txtBox.Text = Val.ToString(DRow["BOXNAME"]);
                txtBox.Tag = Val.ToString(DRow["BOX_ID"]);

                txtMfgColor.Text = Val.ToString(DRow["MFGCOLOR"]);
                txtMfgColor.Tag = Val.ToString(DRow["MFGCOLOR_ID"]);

                txtMfgClarity.Text = Val.ToString(DRow["MFGCLARITY"]);
                txtMfgClarity.Tag = Val.ToString(DRow["MFGCLARITY_ID"]);

                txtMfgCut.Text = Val.ToString(DRow["MFGCUT"]);
                txtMfgCut.Tag = Val.ToString(DRow["MFGCUT_ID"]);

                txtMfgPol.Text = Val.ToString(DRow["MFGPOL"]);
                txtMfgPol.Tag = Val.ToString(DRow["MFGPOL_ID"]);

                txtMfgSym.Text = Val.ToString(DRow["MFGSYM"]);
                txtMfgSym.Tag = Val.ToString(DRow["MFGSYM_ID"]);

                txtMfgFL.Text = Val.ToString(DRow["MFGFL"]);
                txtMfgFL.Tag = Val.ToString(DRow["MFGFL_ID"]);

                txtDiaMin.Text = Val.ToString(DRow["DIAMIN"]);
                txtDiaMax.Text = Val.ToString(DRow["DIAMAX"]);
                txtLength.Text = Val.ToString(DRow["LENGTH"]);
                txtWidth.Text = Val.ToString(DRow["WIDTH"]);
                txtHeight.Text = Val.ToString(DRow["HEIGHT"]);
                txtRatio.Text = Val.ToString(DRow["RATIO"]);
                txtDiameter.Text = Val.ToString(DRow["DIAMETER"]);
                txtCRAngle.Text = Val.ToString(DRow["CRANGLE"]);
                txtCRHeight.Text = Val.ToString(DRow["CRHEIGHT"]);
                txtPavAngle.Text = Val.ToString(DRow["PAVANGLE"]);
                txtPavHeight.Text = Val.ToString(DRow["PAVHEIGHT"]);
                txtDepthPer.Text = Val.ToString(DRow["DEPTHPER"]);
                txtTablePer.Text = Val.ToString(DRow["TABLEPER"]);
                txtGirdlePer.Text = Val.ToString(DRow["GIRDLEPER"]);

                //txtTableInc.Text = Val.ToString(DRow["TABLEINCNAME"]);
                //txtTableInc.Tag = Val.ToString(DRow["TABLEINC_ID"]);

                CmbTableInc.SetEditValue(Val.ToString(DRow["TABLEINCNAME"]));

                txtTableOpenInc.Text = Val.ToString(DRow["TABLEOPENNAME"]);
                txtTableOpenInc.Tag = Val.ToString(DRow["TABLEOPENINC_ID"]);

                // txtSideTableInc.Text = Val.ToString(DRow["SIDETABLENAME"]);
                //txtSideTableInc.Tag = Val.ToString(DRow["SIDETABLEINC_ID"]);
                CmbSideTable.SetEditValue(Val.ToString(DRow["SIDETABLENAME"]));

                txtSideOpenInc.Text = Val.ToString(DRow["SIDEOPENNAME"]);
                txtSideOpenInc.Tag = Val.ToString(DRow["SIDEOPENINC_ID"]);

                txtTableBlackInc.Text = Val.ToString(DRow["TABLEBLACKNAME"]);
                txtTableBlackInc.Tag = Val.ToString(DRow["TABLEBLACKINC_ID"]);

                txtSideBlackInc.Text = Val.ToString(DRow["SIDEBLACKNAME"]);
                txtSideBlackInc.Tag = Val.ToString(DRow["SIDEBLACKINC_ID"]);

                txtRedSportInc.Text = Val.ToString(DRow["REDSPORTNAME"]);
                txtRedSportInc.Tag = Val.ToString(DRow["REDSPORTINC_ID"]);

                txtGirdle.Text = Val.ToString(DRow["GIRDLENAME"]);
                txtGirdle.Tag = Val.ToString(DRow["GIRDLE_ID"]);

                txtMilky.Text = Val.ToString(DRow["MILKYNAME"]);
                txtMilky.Tag = Val.ToString(DRow["MILKY_ID"]);

                txtGirdleFrom.Text = Val.ToString(DRow["FROMGIRDLENAME"]);
                txtGirdleFrom.Tag = Val.ToString(DRow["FROMGIRDLE_ID"]);

                txtGirdleTo.Text = Val.ToString(DRow["TOGIRDLENAME"]);
                txtGirdleTo.Tag = Val.ToString(DRow["TOGIRDLE_ID"]);

                txtLuster.Text = Val.ToString(DRow["LUSTERNAME"]);
                txtLuster.Tag = Val.ToString(DRow["LUSTER_ID"]);

                txtCulet.Text = Val.ToString(DRow["CULETNAME"]);
                txtCulet.Tag = Val.ToString(DRow["CULET_ID"]);

                txtEyeClean.Text = Val.ToString(DRow["EYECLEANNAME"]);
                txtEyeClean.Tag = Val.ToString(DRow["EYECLEAN_ID"]);

                txtHA.Text = Val.ToString(DRow["HANAME"]);
                txtHA.Tag = Val.ToString(DRow["HA_ID"]);

                txtStarLength.Text = Val.ToString(DRow["STARLENGTH"]);
                txtLowerHalf.Text = Val.ToString(DRow["LOWERHALF"]);

                txtUploadDate.Text = Val.ToString(DRow["UPLOADDATE"]);
                txtAvailableDate.Text = Val.ToString(DRow["AVAILABLEDATE"]);
                txtLabResultDate.Text = Val.ToString(DRow["LABRETURNDATE"]);
                txtLabReturnDate.Text = Val.ToString(DRow["LABRETURNDATE"]);
                txtLabIssueDate.Text = Val.ToString(DRow["LABISSUEDATE"]);
                txtPriceRevisedDate.Text = Val.ToString(DRow["PRICEREVISEDATE"]);

                ChkNOBlack.Checked = Val.ToBoolean(DRow["ISNOBLACK"]);
                ChkNoBGM.Checked = Val.ToBoolean(DRow["ISNOBGM"]);
                ChkISExclusive.Checked = Val.ToBoolean(DRow["ISNOBGM"]);

                txtProcess.Text = Val.ToString(DRow["PROCESSNAME"]);
                txtProcess.Tag = Val.ToString(DRow["PROCESS_ID"]);

                txtColorDescription.Text = Val.ToString(DRow["COLORDESC"]);
                txtFancyColor.Text = Val.ToString(DRow["FANCYCOLOR"]);
                txtFancyIntensity.Text = Val.ToString(DRow["FANCYCOLORINTENSITY"]);
                txtFancyOvertone.Text = Val.ToString(DRow["FANCYCOLOROVERTONE"]);
                txtKeyToSymbol.Text = Val.ToString(DRow["KEYTOSYMBOL"]);
                txtReportCommnet.Text = Val.ToString(DRow["REPORTCOMMENT"]);

                txtGirdleCondition.Text = Val.ToString(DRow["GIRDLECONDITION"]);
                txtGirdleDescription.Text = Val.ToString(DRow["GIRDLEDESC"]);
                txtPainting.Text = Val.ToString(DRow["PAINTING"]);
                txtProportion.Text = Val.ToString(DRow["PROPORTIONS"]);
                txtPaintComment.Text = Val.ToString(DRow["PAINTCOMM"]);
                txtSymFeatures.Text = Val.ToString(DRow["SYMMETRYFEATURES"]);
                txtRemark.Text = Val.ToString(DRow["REMARK"]);
                txtInscription.Text = Val.ToString(DRow["INSCRIPTION"]);
                txtClientComment.Text = Val.ToString(DRow["CLIENTCOMMENT"]);
                txtPolishFeatures.Text = Val.ToString(DRow["POLISHFEATURES"]);
                txtSymIndiction.Text = Val.ToString(DRow["SYNTHETICINDICATOR"]);

                txtMfgRapaport.Text = Val.ToString(DRow["MFGRAPAPORT"]);
                txtMfgDiscount.Text = Val.ToString(DRow["MFGDISCOUNT"]);
                txtMfgPricePerCarat.Text = Val.ToString(DRow["MFGPRICEPERCARAT"]);
                txtMfgAmount.Text = Val.ToString(DRow["MFGAMOUNT"]);

                txtSaleRapaport.Text = Val.ToString(DRow["SALERAPAPORT"]);
                txtSaleDiscount.Text = Val.ToString(DRow["SALEDISCOUNT"]);
                txtSalePricePerCarat.Text = Val.ToString(DRow["SALEPRICEPERCARAT"]);
                txtSaleAmount.Text = Val.ToString(DRow["SALEAMOUNT"]);

                txtCostRapaport.Text = Val.ToString(DRow["COSTRAPAPORT"]);
                txtCostDiscount.Text = Val.ToString(DRow["COSTDISCOUNT"]);
                txtCostPricePerCarat.Text = Val.ToString(DRow["COSTPRICEPERCARAT"]);
                txtCostAmount.Text = Val.ToString(DRow["COSTAMOUNT"]);

                txtRapnetRapaport.Text = Val.ToString(DRow["RAPNETRAPAPORT"]);
                txtRapnetDiscount.Text = Val.ToString(DRow["RAPNETDISCOUNT"]);
                txtRapnetPricePerCarat.Text = Val.ToString(DRow["RAPNETPRICEPERCARAT"]);
                txtRapnetAmount.Text = Val.ToString(DRow["RAPNETAMOUNT"]);

                txtJARapaport.Text = Val.ToString(DRow["JAMESALLENRAPAPORT"]);
                txtJADiscount.Text = Val.ToString(DRow["JAMESALLENDISCOUNT"]);
                txtJAPricePerCarat.Text = Val.ToString(DRow["JAMESALLENPRICEPERCARAT"]);
                txtJAAmount.Text = Val.ToString(DRow["JAMESALLENAMOUNT"]);

                txtExpRapaport.Text = Val.ToString(DRow["EXPRAPAPORT"]);
                txtExpDiscount.Text = Val.ToString(DRow["EXPDISCOUNT"]);
                txtExpPricePerCarat.Text = Val.ToString(DRow["EXPPRICEPERCARAT"]);
                txtExpAmount.Text = Val.ToString(DRow["EXPAMOUNT"]);

                TxtMasurment.Text = Val.ToString(DRow["MEASUREMENT"]);

                txtCompRapaport.Text = Val.ToString(DRow["COMPRAPAPORT"]);
                txtCompDiscount.Text = Val.ToString(DRow["COMPDISCOUNT"]);
                txtCompPricePerCarat.Text = Val.ToString(DRow["COMPPRICEPERCARAT"]);
                txtCompAmount.Text = Val.ToString(DRow["COMPAMOUNT"]);
                txtCompDiscount.AccessibleDescription = Val.ToString(DRow["BACKDETAIL"]);
            }

            MainGrdHistory.DataSource = DS.Tables[1];
            MainGrdHistory.Refresh();

            MainGridParameter.DataSource = DS.Tables[2];
            MainGridParameter.Refresh();

            MainGridPrice.DataSource = DS.Tables[3];
            MainGridPrice.Refresh();

            MainGridComment.DataSource = DS.Tables[4];
            MainGridComment.Refresh();

            MainGrdPrediction.DataSource = DS.Tables[5];
            MainGrdPrediction.Refresh();

            MainGrid.DataSource = DTabStockStatment;
            MainGrid.RefreshDataSource();

            GrdPrediction.BestFitColumns();

            this.Cursor = Cursors.Default;

        }

        private void txtStoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "STOCKNO";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.TRN_SINGLESTOCK);

                    FrmSearch.mStrColumnsToHide = "STOCK_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtStoneNo.Text = Val.ToString(FrmSearch.DRow["STOCKNO"]);
                        txtStoneNo.Tag = Val.ToString(FrmSearch.DRow["STOCK_ID"]);
                        BtnUpdate.Enabled = false;
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

        private void txtShape_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (mFormType == FORMTYPE.DISPLAY)
                {
                    e.Handled = true;
                    return;
                }

                AxonContLib.cTextBox txt = (AxonContLib.cTextBox)sender;

                if (Val.ToString(txt.AccessibleDescription) == "")
                    return;

                DataTable DTab = new DataTable();

                if (Val.ToString(txt.AccessibleDescription).ToUpper() == "SHAPE")
                    DTab = DtabPara.Select("PARATYPE = 'SHAPE'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "COLOR" || Val.ToString(txt.AccessibleDescription).ToUpper() == "MFGCOLOR")
                    DTab = DtabPara.Select("PARATYPE = 'COLOR'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "CLARITY" || Val.ToString(txt.AccessibleDescription).ToUpper() == "MFGCLARITY")
                    DTab = DtabPara.Select("PARATYPE = 'CLARITY'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "CUT" || Val.ToString(txt.AccessibleDescription).ToUpper() == "MFGCUT")
                    DTab = DtabPara.Select("PARATYPE = 'CUT'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "POL" || Val.ToString(txt.AccessibleDescription).ToUpper() == "MFGPOL")
                    DTab = DtabPara.Select("PARATYPE = 'POLISH'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "SYM" || Val.ToString(txt.AccessibleDescription).ToUpper() == "MFGSYM")
                    DTab = DtabPara.Select("PARATYPE = 'SYMMETRY'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "FL" || Val.ToString(txt.AccessibleDescription).ToUpper() == "MFGFL")
                    DTab = DtabPara.Select("PARATYPE = 'FLUORESCENCE'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "FLCOLOR")
                    DTab = DtabPara.Select("PARATYPE = 'FLUORESCENCE_COLOR'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "LOCATION")
                    DTab = DtabPara.Select("PARATYPE = 'LOCATION'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "SIZE")
                    DTab = DtabPara.Select("PARATYPE = 'SIZE'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "LAB")
                    DTab = DtabPara.Select("PARATYPE = 'LAB'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "COLORSHADE")
                    DTab = DtabPara.Select("PARATYPE = 'COLORSHADE'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "MILKY")
                    DTab = DtabPara.Select("PARATYPE = 'MILKY'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "EYECLEAN")
                    DTab = DtabPara.Select("PARATYPE = 'EYECLEAN'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "LUSTER")
                    DTab = DtabPara.Select("PARATYPE = 'LUSTER'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "HA")
                    DTab = DtabPara.Select("PARATYPE = 'HEARTANDARROW'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "CULET")
                    DTab = DtabPara.Select("PARATYPE = 'CULET'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "GIRDLE")
                    DTab = DtabPara.Select("PARATYPE = 'GIRDLE'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "TABLEINC")
                    DTab = DtabPara.Select("PARATYPE = 'TABLEBLACKINC'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "TABLEOPEN")
                    DTab = DtabPara.Select("PARATYPE = 'TABLEOPENINC'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "SIDETABLE")
                    DTab = DtabPara.Select("PARATYPE = 'SIDETABLEINC'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "SIDEOPEN")
                    DTab = DtabPara.Select("PARATYPE = 'SIDEOPENINC'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "TABLEBLACK")
                    DTab = DtabPara.Select("PARATYPE = 'TABLEBLACKINC'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "SIDEBLACK")
                    DTab = DtabPara.Select("PARATYPE = 'SIDEBLACKINC'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "REDSPORT")
                    DTab = DtabPara.Select("PARATYPE = 'REDSPORTINC'").CopyToDataTable();

                else if (Val.ToString(txt.AccessibleDescription).ToUpper() == "FLCOLOR")
                    DTab = DtabPara.Select("PARATYPE = 'FLUORESCENCE_COLOR'").CopyToDataTable();


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
                        txt.Tag = Val.ToString(FrmSearch.DRow["PARA_ID"]);
                        txt.Text = Val.ToString(FrmSearch.DRow["SHORTNAME"]);

                        FindRap();

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

        public void FindRap()
        {
            try
            {

                Trn_RapSaveProperty Property = new Trn_RapSaveProperty();


                /*
                Property.SHAPE_ID = Val.ToString(txtShape.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='SHAPE' And ShortName='" + Val.ToString(txtShape.Text) + "'")[0]["PARA_ID"]) : 0;
                Property.SHAPECODE = txtShape.Text;
                Property.SHAPERAPVALUE = Val.ToString(txtShape.Text) != "" ? Val.ToString(DtabPara.Select("ParaType='SHAPE' And ShortName='" + Val.ToString(txtShape.Text) + "'")[0]["RAPAVALUE"]) : "";

                Property.COLOR_ID = Val.ToString(txtColor.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='COLOR' And ShortName='" + Val.ToString(txtColor.Text) + "'")[0]["PARA_ID"]) : 0;
                Property.COLORCODE = txtColor.Text;
                Property.COLORRAPVALUE = Val.ToString(txtColor.Text) != "" ? Val.ToString(DtabPara.Select("ParaType='COLOR' And ShortName='" + Val.ToString(txtColor.Text) + "'")[0]["RAPAVALUE"]) : "";

                Property.CLARITY_ID = Val.ToString(txtClarity.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='CLARITY' And ShortName='" + Val.ToString(txtClarity.Text) + "'")[0]["PARA_ID"]) : 0;
                Property.CLARITYCODE = txtClarity.Text;
                Property.CLARITYRAPVALUE = Val.ToString(txtClarity.Text) != "" ? Val.ToString(DtabPara.Select("ParaType='CLARITY' And ShortName='" + Val.ToString(txtClarity.Text) + "'")[0]["RAPAVALUE"]) : "";

                Property.CUT_ID = Val.ToString(txtCut.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='CUT' And ShortName='" + Val.ToString(txtCut.Text) + "'")[0]["PARA_ID"]) : 0;
                Property.CUTCODE = txtCut.Text;

                Property.POL_ID = Val.ToString(txtPol.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='POLISH' And ShortName='" + Val.ToString(txtPol.Text) + "'")[0]["PARA_ID"]) : 0;
                Property.POLCODE = txtPol.Text;

                Property.SYM_ID = Val.ToString(txtSym.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='SYMMETRY' And ShortName='" + Val.ToString(txtSym.Text) + "'")[0]["PARA_ID"]) : 0;
                Property.SYMCODE = txtSym.Text;

                Property.FL_ID = Val.ToString(txtFL.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='FLUORESCENCE' And ShortName='" + Val.ToString(txtFL.Text) + "'")[0]["PARA_ID"]) : 0;
                Property.FLCODE = txtFL.Text;

                Property.MILKY_ID = Val.ToString(txtMilky.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='MILKY' And ShortName='" + Val.ToString(txtMilky.Text) + "'")[0]["PARA_ID"]) : 0;
                Property.MILKYCODE = txtMilky.Text;

                Property.GIRDLE_ID = Val.ToString(txtGirdle.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='GIRDLE' And PARANAME='" + Val.ToString(txtGirdle.Text) + "'")[0]["PARA_ID"]) : 0;
                Property.GIRDLECODE = txtGirdle.Text;

                //Property.TABLEINC_ID = Val.ToString(txtTableInc.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='TABLEINC' And PARANAME='" + Val.ToString(txtTableInc.Text) + "'")[0]["PARA_ID"]) : 0;
                //Property.TABLEINCCODE = txtTableInc.Text;
                Property.TABLEINC_ID = Val.Trim(Val.ToString(CmbTableInc.EditValue));
                Property.TABLEINCCODE = Val.Trim(Val.ToString(CmbTableInc.Text));

                Property.TABLEOPENINC_ID = Val.ToString(txtTableOpenInc.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='TABLEOPENINC' And SHORTNAME ='" + Val.ToString(txtTableOpenInc.Text) + "'")[0]["PARA_ID"]) : 0;
                Property.TABLEOPENINCCODE = txtTableOpenInc.Text;

                Property.SIDEOPENINC_ID = Val.ToString(txtSideOpenInc.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='SIDEOPENINC' And SHORTNAME='" + Val.ToString(txtSideOpenInc.Text) + "'")[0]["PARA_ID"]) : 0;
                Property.SIDEOPENINCCODE = txtSideOpenInc.Text;

                //Property.SIDETABLEINC_ID = Val.ToString(txtSideTableInc.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='SIDETABLEINC' And PARANAME='" + Val.ToString(txtSideTableInc.Text) + "'")[0]["PARA_ID"]) : 0;
                //Property.SIDETABLEINCCODE = txtSideTableInc.Text;
                Property.SIDETABLEINC_ID = Val.ToString(CmbSideTable.EditValue);
                Property.SIDETABLEINCCODE = CmbSideTable.Text;

                Property.TABLEBLACKINC_ID = Val.ToString(txtTableBlackInc.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='TABLEBLACKINC' And PARANAME='" + Val.ToString(txtTableBlackInc.Text) + "'")[0]["PARA_ID"]) : 0;
                Property.TABLEBLACKINCCODE = txtTableBlackInc.Text;

                Property.SIDEBLACKINC_ID = Val.ToString(txtSideBlackInc.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='SIDEBLACKINC' And PARANAME='" + Val.ToString(txtSideBlackInc.Text) + "'")[0]["PARA_ID"]) : 0;
                Property.SIDEBLACKINCCODE = txtSideBlackInc.Text;

                Property.REDSPORTINC_ID = Val.ToString(txtRedSportInc.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='REDSPORTINC' And PARANAME='" + Val.ToString(txtRedSportInc.Text) + "'")[0]["PARA_ID"]) : 0;
                Property.REDSPORTINCCODE = txtRedSportInc.Text;

                Property.CULET_ID = Val.ToString(txtCulet.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='CULET' And PARANAME='" + Val.ToString(txtCulet.Text) + "'")[0]["PARA_ID"]) : 0;
                Property.CULETCODE = txtCulet.Text;

                Property.LUSTER_ID = Val.ToString(txtLuster.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='LUSTER' And PARANAME='" + Val.ToString(txtLuster.Text) + "'")[0]["PARA_ID"]) : 0;
                Property.LUSTERCODE = txtLuster.Text;

                Property.LAB_ID = Val.ToString(txtLab.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='LAB' And PARANAME='" + Val.ToString(txtLab.Text) + "'")[0]["PARA_ID"]) : 0;
                Property.LABCODE = txtLab.Text;

                Property.HA_ID = Val.ToString(txtHA.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='HEARTANDARROW' And PARANAME='" + Val.ToString(txtHA.Text) + "'")[0]["PARA_ID"]) : 0;
                Property.HACODE = txtHA.Text;

                Property.EYECLEAN_ID = Val.ToString(txtEyeClean.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='EYECLEAN' And PARANAME='" + Val.ToString(txtEyeClean.Text) + "'")[0]["PARA_ID"]) : 0;
                Property.EYECLEANCODE = txtEyeClean.Text;
                */

                Property.SHAPE_ID = Val.ToString(txtShape.Text) != "" ? Val.ToInt(txtShape.Tag) : 0;
                Property.SHAPECODE = txtShape.Text;
                Property.SHAPERAPVALUE = Val.ToString(txtShape.Text) != "" ? Val.ToString(DtabPara.Select("ParaType='SHAPE' And ShortName='" + Val.ToString(txtShape.Text) + "'")[0]["RAPAVALUE"]) : "";

                Property.COLOR_ID = Val.ToString(txtColor.Text) != "" ? Val.ToInt(txtColor.Tag) : 0;
                Property.COLORCODE = txtColor.Text;
                Property.COLORRAPVALUE = Val.ToString(txtColor.Text) != "" ? Val.ToString(DtabPara.Select("ParaType='COLOR' And ShortName='" + Val.ToString(txtColor.Text) + "'")[0]["RAPAVALUE"]) : "";

                Property.CLARITY_ID = Val.ToString(txtClarity.Text) != "" ? Val.ToInt(txtClarity.Tag) : 0;
                Property.CLARITYCODE = txtClarity.Text;
                Property.CLARITYRAPVALUE = Val.ToString(txtClarity.Text) != "" ? Val.ToString(DtabPara.Select("ParaType='CLARITY' And ShortName='" + Val.ToString(txtClarity.Text) + "'")[0]["RAPAVALUE"]) : "";

                Property.CUT_ID = Val.ToString(txtCut.Text) != "" ? Val.ToInt(txtCut.Tag) : 0;
                Property.CUTCODE = txtCut.Text;

                Property.POL_ID = Val.ToString(txtPol.Text) != "" ? Val.ToInt(txtPol.Tag) : 0;
                Property.POLCODE = txtPol.Text;

                Property.SYM_ID = Val.ToString(txtSym.Text) != "" ? Val.ToInt(txtSym.Tag) : 0;
                Property.SYMCODE = txtSym.Text;

                Property.FL_ID = Val.ToString(txtFL.Text) != "" ? Val.ToInt(txtFL.Tag) : 0;
                Property.FLCODE = txtFL.Text;

                Property.MILKY_ID = Val.ToString(txtMilky.Text) != "" ? Val.ToInt(txtMilky.Tag) : 0;
                Property.MILKYCODE = txtMilky.Text;

                Property.GIRDLE_ID = Val.ToString(txtGirdle.Text) != "" ? Val.ToInt(txtGirdle.Tag) : 0;
                Property.GIRDLECODE = txtGirdle.Text;

                //Property.TABLEINC_ID = Val.ToString(txtTableInc.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='TABLEINC' And PARANAME='" + Val.ToString(txtTableInc.Text) + "'")[0]["PARA_ID"]) : 0;
                //Property.TABLEINCCODE = txtTableInc.Text;
                Property.TABLEINC_ID = Val.Trim(Val.ToString(CmbTableInc.EditValue));
                Property.TABLEINCCODE = Val.Trim(Val.ToString(CmbTableInc.Text));

                Property.TABLEOPENINC_ID = Val.ToString(txtTableOpenInc.Text) != "" ? Val.ToInt(txtTableOpenInc.Tag) : 0;
                Property.TABLEOPENINCCODE = txtTableOpenInc.Text;

                Property.SIDEOPENINC_ID = Val.ToString(txtSideOpenInc.Text) != "" ? Val.ToInt(txtSideOpenInc.Tag) : 0;
                Property.SIDEOPENINCCODE = txtSideOpenInc.Text;

                //Property.SIDETABLEINC_ID = Val.ToString(txtSideTableInc.Text) != "" ? Val.ToInt(DtabPara.Select("ParaType='SIDETABLEINC' And PARANAME='" + Val.ToString(txtSideTableInc.Text) + "'")[0]["PARA_ID"]) : 0;
                //Property.SIDETABLEINCCODE = txtSideTableInc.Text;
                Property.SIDETABLEINC_ID = Val.ToString(CmbSideTable.EditValue);
                Property.SIDETABLEINCCODE = CmbSideTable.Text;

                Property.TABLEBLACKINC_ID = Val.ToString(txtTableBlackInc.Text) != "" ? Val.ToInt(txtTableBlackInc.Tag) : 0;
                Property.TABLEBLACKINCCODE = txtTableBlackInc.Text;

                Property.SIDEBLACKINC_ID = Val.ToString(txtSideBlackInc.Text) != "" ? Val.ToInt(txtSideBlackInc.Tag) : 0;
                Property.SIDEBLACKINCCODE = txtSideBlackInc.Text;

                Property.REDSPORTINC_ID = Val.ToString(txtRedSportInc.Text) != "" ? Val.ToInt(txtRedSportInc.Tag) : 0;
                Property.REDSPORTINCCODE = txtRedSportInc.Text;

                Property.CULET_ID = Val.ToString(txtCulet.Text) != "" ? Val.ToInt(txtCulet.Tag) : 0;
                Property.CULETCODE = txtCulet.Text;

                Property.LUSTER_ID = Val.ToString(txtLuster.Text) != "" ? Val.ToInt(txtLuster.Tag) : 0;
                Property.LUSTERCODE = txtLuster.Text;

                Property.LAB_ID = Val.ToString(txtLab.Text) != "" ? Val.ToInt(txtLab.Tag) : 0;
                Property.LABCODE = txtLab.Text;

                Property.HA_ID = Val.ToString(txtHA.Text) != "" ? Val.ToInt(txtHA.Tag) : 0;
                Property.HACODE = txtHA.Text;

                Property.EYECLEAN_ID = Val.ToString(txtEyeClean.Text) != "" ? Val.ToInt(txtEyeClean.Tag) : 0;
                Property.EYECLEANCODE = txtEyeClean.Text;

                Property.DEPTHPER = Val.Val(txtDepthPer.Text);
                Property.TABLEPER = Val.Val(txtTablePer.Text);

                Property.DIAMETER = Val.Val(txtDiameter.Text);
                Property.LENGTH = Val.Val(txtLength.Text);
                Property.WIDTH = Val.Val(txtWidth.Text);
                Property.HEIGHT = Val.Val(txtHeight.Text);

                Property.ISFANCY = txtFancyColor.Text.Length == 0 ? false : true;
                Property.SALERAPAPORT = Val.Val(txtSaleRapaport.Text);

                Property.SALEDISCOUNT = Val.Val(txtSaleRapaport.Text);
                Property.SALEPRICEPERCARAT = Val.Val(txtSaleRapaport.Text);
                Property.SALEAMOUNT = Val.Val(txtSaleRapaport.Text);
                Property.RAPDATE = Val.SqlDate(DateTime.Now.ToShortDateString());

                Property.CARAT = Val.Val(txtCarat.Text);

                Property = ObjRap.FindRap(Property);

                txtSaleRapaport.Text = Val.ToString(Property.RAPAPORT);
                txtSalePricePerCarat.Text = Val.ToString(Math.Round(Property.RAPAPORT + ((Property.RAPAPORT * Val.Val(txtSaleDiscount.Text)) / 100)));
                txtSaleAmount.Text = Val.ToString(Math.Round(Property.CARAT * Val.Val(txtSalePricePerCarat.Text), 2));

                txtRapnetRapaport.Text = Val.ToString(Property.RAPAPORT);
                txtRapnetPricePerCarat.Text = Val.ToString(Math.Round(Property.RAPAPORT + ((Property.RAPAPORT * Val.Val(txtRapnetDiscount.Text)) / 100)));
                txtRapnetAmount.Text = Val.ToString(Math.Round(Property.CARAT * Val.Val(txtRapnetPricePerCarat.Text), 2));

                txtExpRapaport.Text = Val.ToString(Property.RAPAPORT);
                txtExpPricePerCarat.Text = Val.ToString(Math.Round(Property.RAPAPORT + ((Property.RAPAPORT * Val.Val(txtExpDiscount.Text) / 100))));
                txtExpAmount.Text = Val.ToString(Math.Round(Property.CARAT * Val.Val(txtExpPricePerCarat.Text), 2));

                txtJARapaport.Text = Val.ToString(Property.RAPAPORT);
                txtJAPricePerCarat.Text = Val.ToString(Math.Round(Property.RAPAPORT + ((Property.RAPAPORT * Val.Val(txtJADiscount)) / 100)));
                txtJAAmount.Text = Val.ToString(Math.Round(Property.CARAT * Val.Val(txtJAPricePerCarat.Text), 2));

                txtCompRapaport.Text = Val.ToString(Property.RAPAPORT);
                txtCompDiscount.Text = Val.ToString(Property.FINALBACK);
                txtCompDiscount.AccessibleDescription = Val.ToString(Property.XMLDETAIL);
                txtCompPricePerCarat.Text = Val.ToString(Property.FINALPRICEPERCARAT);
                txtCompAmount.Text = Val.ToString(Property.FINALAMOUNT);

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e) && mFormType == FORMTYPE.UPDATE)
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "BOXCODE,BOXNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_BOX);

                    FrmSearch.mStrColumnsToHide = "BOX_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBox.Text = Val.ToString(FrmSearch.DRow["BOXNAME"]);
                        txtBox.Tag = Val.ToString(FrmSearch.DRow["BOX_ID"]);
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }

            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        public void Clear()
        {
            txtStoneNo.Text = string.Empty;
            txtStoneNo.Tag = string.Empty;

            txtShape.Text = string.Empty;
            txtShape.Tag = string.Empty;

            txtColor.Text = string.Empty;
            txtColor.Tag = string.Empty;

            txtCarat.Text = string.Empty;

            txtClarity.Text = string.Empty;
            txtClarity.Tag = string.Empty;

            txtCut.Text = string.Empty;
            txtCut.Tag = string.Empty;

            TxtMasurment.Text = string.Empty;

            txtPol.Text = string.Empty;
            txtPol.Tag = string.Empty;

            txtSym.Text = string.Empty;
            txtSym.Tag = string.Empty;

            txtFL.Text = string.Empty;
            txtFL.Tag = string.Empty;

            txtFLColor.Text = string.Empty;
            txtFLColor.Tag = string.Empty;

            txtLab.Text = string.Empty;
            txtLab.Tag = string.Empty;

            txtLocation.Text = string.Empty;
            txtLocation.Tag = string.Empty;

            txtSize.Text = string.Empty;
            txtSize.Tag = string.Empty;

            txtColorShade.Text = string.Empty;
            txtColorShade.Tag = string.Empty;

            txtBox.Text = string.Empty;
            txtBox.Tag = string.Empty;

            txtReportNo.Text = string.Empty;
            txtReportNo.Tag = string.Empty;

            txtMfgColor.Text = string.Empty;
            txtMfgColor.Tag = string.Empty;

            txtMfgClarity.Text = string.Empty;
            txtMfgClarity.Tag = string.Empty;

            txtMfgCut.Text = string.Empty;
            txtMfgCut.Tag = string.Empty;

            txtMfgPol.Text = string.Empty;
            txtMfgPol.Tag = string.Empty;

            txtMfgSym.Text = string.Empty;
            txtMfgSym.Tag = string.Empty;

            txtMfgFL.Text = string.Empty;
            txtMfgFL.Tag = string.Empty;

            txtMilky.Text = string.Empty;
            txtMilky.Tag = string.Empty;

            txtDiaMax.Text = string.Empty;
            txtDiaMin.Text = string.Empty;
            txtLength.Text = string.Empty;
            txtHeight.Text = string.Empty;
            txtWidth.Text = string.Empty;
            txtDiameter.Text = string.Empty;
            txtRatio.Text = string.Empty;

            txtCRAngle.Text = string.Empty;
            txtCRHeight.Text = string.Empty;
            txtPavAngle.Text = string.Empty;
            txtPavHeight.Text = string.Empty;
            txtTablePer.Text = string.Empty;
            txtGirdlePer.Text = string.Empty;
            txtDepthPer.Text = string.Empty;

            txtTableOpenInc.Text = string.Empty;
            txtTableOpenInc.Tag = string.Empty;

            txtSideOpenInc.Text = string.Empty;
            txtSideOpenInc.Tag = string.Empty;

            txtSideBlackInc.Text = string.Empty;
            txtSideBlackInc.Tag = string.Empty;

            txtRedSportInc.Text = string.Empty;
            txtRedSportInc.Tag = string.Empty;

            txtStarLength.Text = string.Empty;
            txtLowerHalf.Text = string.Empty;

            txtGirdle.Text = string.Empty;
            txtGirdle.Tag = string.Empty;

            txtGirdleFrom.Text = string.Empty;
            txtGirdleFrom.Tag = string.Empty;

            txtGirdleTo.Text = string.Empty;
            txtGirdleTo.Tag = string.Empty;

            txtCulet.Text = string.Empty;
            txtCulet.Tag = string.Empty;

            txtLuster.Text = string.Empty;
            txtLuster.Tag = string.Empty;

            txtEyeClean.Text = string.Empty;
            txtEyeClean.Tag = string.Empty;

            txtHA.Text = string.Empty;
            txtHA.Tag = string.Empty;

            txtUploadDate.Text = string.Empty;
            txtLabIssueDate.Text = string.Empty;
            txtLabResultDate.Text = string.Empty;
            txtLabReturnDate.Text = string.Empty;
            txtAvailableDate.Text = string.Empty;
            txtPriceRevisedDate.Text = string.Empty;

            txtProcess.Text = string.Empty;
            txtProcess.Tag = string.Empty;

            ChkISExclusive.Checked = false;
            ChkNoBGM.Checked = false;
            ChkNOBlack.Checked = false;

            txtColorDescription.Text = string.Empty;
            txtFancyColor.Text = string.Empty;
            txtFancyIntensity.Text = string.Empty;
            txtFancyOvertone.Text = string.Empty;
            txtKeyToSymbol.Text = string.Empty;
            txtReportCommnet.Text = string.Empty;

            txtGirdleCondition.Text = string.Empty;
            txtGirdleDescription.Text = string.Empty;
            txtPainting.Text = string.Empty;
            txtProportion.Text = string.Empty;
            txtPaintComment.Text = string.Empty;
            txtSymIndiction.Text = string.Empty;

            txtRemark.Text = string.Empty;
            txtInscription.Text = string.Empty;
            txtClientComment.Text = string.Empty;
            txtPolishFeatures.Text = string.Empty;
            txtSymFeatures.Text = string.Empty;

            txtSaleRapaport.Text = string.Empty;
            txtSaleDiscount.Text = string.Empty;
            txtSalePricePerCarat.Text = string.Empty;
            txtSaleAmount.Text = string.Empty;

            txtMfgAmount.Text = string.Empty;
            txtMfgDiscount.Text = string.Empty;
            txtMfgPricePerCarat.Text = string.Empty;
            txtMfgRapaport.Text = string.Empty;

            txtCostRapaport.Text = string.Empty;
            txtCostDiscount.Text = string.Empty;
            txtCostPricePerCarat.Text = string.Empty;
            txtCostAmount.Text = string.Empty;

            txtExpRapaport.Text = string.Empty;
            txtExpDiscount.Text = string.Empty;
            txtExpPricePerCarat.Text = string.Empty;
            txtExpAmount.Text = string.Empty;

            txtJARapaport.Text = string.Empty;
            txtJADiscount.Text = string.Empty;
            txtJAPricePerCarat.Text = string.Empty;
            txtJAAmount.Text = string.Empty;

            txtRapnetRapaport.Text = string.Empty;
            txtRapnetDiscount.Text = string.Empty;
            txtRapnetPricePerCarat.Text = string.Empty;
            txtRapnetAmount.Text = string.Empty;

            txtCompRapaport.Text = string.Empty;
            txtCompDiscount.Text = string.Empty;
            txtCompPricePerCarat.Text = string.Empty;
            txtCompAmount.Text = string.Empty;

            txtCompDiscount.AccessibleDescription = string.Empty;

            CmbSideTable.DeselectAll();
            CmbTableInc.DeselectAll();
            BtnUpdate.Enabled = false;

        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (Val.ToString(txtStoneNo.Text).Trim().Equals(string.Empty))
                {
                    Global.Message("Stone No Is Required");
                    txtStoneNo.Focus();
                    return;
                }


                this.Cursor = Cursors.WaitCursor;
                SingleStockUpdateProperty Property = new SingleStockUpdateProperty();

                Property.STOCK_ID = Guid.Parse(Val.ToString(txtStoneNo.Tag));

                Property.SHAPE_ID = Val.ToInt32(txtShape.Tag);
                Property.COLOR_ID = Val.ToInt32(txtColor.Tag);
                Property.CARAT = Val.Val(txtCarat.Text);
                Property.CLARITY_ID = Val.ToInt32(txtClarity.Tag);
                Property.CUT_ID = Val.ToInt32(txtCut.Tag);
                Property.POL_ID = Val.ToInt32(txtPol.Tag);
                Property.SYM_ID = Val.ToInt32(txtSym.Tag);
                Property.FL_ID = Val.ToInt32(txtFL.Tag);
                Property.FLCOLOR = Val.ToString(txtFLColor.Text);
                Property.COLORSHADE_ID = Val.ToInt32(txtColorShade.Tag);
                Property.SIZE_ID = Val.ToInt32(txtSize.Tag);
                Property.LAB_ID = Val.ToInt32(txtLab.Tag);
                Property.LOCATION_ID = Val.ToInt32(txtLocation.Tag);
                Property.BOX_ID = Val.ToInt32(txtBox.Tag);
                Property.LABREPORTNO = txtReportNo.Text;

                Property.MFGCOLOR_ID = Val.ToInt32(txtMfgColor.Tag);
                Property.MFGCLARITY_ID = Val.ToInt32(txtMfgClarity.Tag);
                Property.MFGCUT_ID = Val.ToInt32(txtMfgCut.Tag);
                Property.MFGPOL_ID = Val.ToInt32(txtMfgPol.Tag);
                Property.MFGFL_ID = Val.ToInt32(txtMfgFL.Tag);
                Property.MFGSYM_ID = Val.ToInt32(txtMfgSym.Tag);

                Property.DIAMIN = Val.Val(txtDiaMin.Text);
                Property.DIAMAX = Val.Val(txtDiaMax.Text);
                Property.LENGTH = Val.Val(txtLength.Text);
                Property.WIDTH = Val.Val(txtWidth.Text);
                Property.HEIGHT = Val.Val(txtHeight.Text);
                Property.RATIO = Val.Val(txtRatio.Text);
                Property.DIAMETER = Val.Val(txtDiameter.Text);
                Property.CRANGLE = Val.Val(txtCRAngle.Text);
                Property.CRHEIGHT = Val.Val(txtCRHeight.Text);
                Property.PAVANGLE = Val.Val(txtPavAngle.Text);
                Property.PAVHEIGHT = Val.Val(txtPavHeight.Text);
                Property.DEPTHPER = Val.Val(txtDepthPer.Text);
                Property.TABLEPER = Val.Val(txtTablePer.Text);
                Property.GIRDLEPER = Val.Val(txtGirdlePer.Text);

                Property.TABLEINC_ID = Val.ToString(CmbTableInc.EditValue);
                Property.TABLEOPENINC_ID = Val.ToInt32(txtTableOpenInc.Tag);
                Property.SIDETABLEINC_ID = Val.ToString(CmbSideTable.EditValue);
                Property.SIDEOPENINC_ID = Val.ToInt32(txtSideOpenInc.Tag);
                Property.TABLEBLACKINC_ID = Val.ToInt32(txtTableBlackInc.Tag);
                Property.SIDEBLACKINC_ID = Val.ToInt32(txtSideBlackInc.Tag);
                Property.REDSPORTINC_ID = Val.ToInt32(txtRedSportInc.Tag);

                Property.GIRDLE_ID = Val.ToInt32(txtGirdle.Tag);
                Property.FROMGIRDLE_ID = Val.ToInt32(txtGirdleFrom.Tag);
                Property.TOGIRDLE_ID = Val.ToInt32(txtGirdleTo.Tag);
                Property.LUSTER_ID = Val.ToInt32(txtLuster.Tag);
                Property.CULET_ID = Val.ToInt32(txtCulet.Tag);
                Property.MILKY_ID = Val.ToInt32(txtMilky.Tag);
                Property.EYECLEAN_ID = Val.ToInt32(txtEyeClean.Tag);
                Property.HA_ID = Val.ToInt32(txtHA.Tag);
                Property.STARLENGTH = Val.ToString(txtStarLength.Text);
                Property.LOWERHALF = Val.ToString(txtLowerHalf.Text);

                Property.UPLOADDATE = Val.SqlDate(txtUploadDate.Text);
                Property.AVAILABLEDATE = Val.SqlDate(txtAvailableDate.Text);
                Property.LABRESULTDATE = Val.SqlDate(txtLabResultDate.Text);
                Property.LABRETURNDATE = Val.SqlDate(txtLabReturnDate.Text);
                Property.LABISSUEDATE = Val.SqlDate(txtLabIssueDate.Text);
                Property.PRICEREVISEDDATE = Val.SqlDate(txtPriceRevisedDate.Text);

                Property.ISBLACK = ChkNOBlack.Checked;
                Property.ISNOBGM = ChkNoBGM.Checked;
                Property.ISEXCLUSIVE = ChkISExclusive.Checked;
                Property.PROCESS_ID = Val.ToInt32(txtProcess.Tag);
                Property.PROCESSNAME = Val.ToString(txtProcess.Text);

                Property.COLORDESC = Val.ToString(txtColorDescription.Text);
                Property.FANCYCOLOR = Val.ToString(txtFancyColor.Text);
                Property.FANCYCOLORINTENSITY = Val.ToString(txtFancyIntensity.Text);
                Property.FANCYCOLOROVERTONE = Val.ToString(txtFancyOvertone.Text);
                Property.KEYTOSYMBOL = Val.ToString(txtKeyToSymbol.Text);
                Property.REPORTCOMMENT = Val.ToString(txtReportCommnet.Text);

                Property.GIRDLECONDITION = Val.ToString(txtGirdleCondition.Text);
                Property.GIRDLEDESC = Val.ToString(txtGirdleDescription.Text);
                Property.PAINTING = Val.ToString(txtPainting.Text);
                Property.PROPORTIONS = Val.ToString(txtProportion.Text);
                Property.PAINTCOMM = Val.ToString(txtPaintComment.Text);
                Property.SYMMETRYFEATURES = Val.ToString(txtSymFeatures.Text);
                Property.REMARK = Val.ToString(txtRemark.Text);
                Property.INSCRIPTION = Val.ToString(txtInscription.Text);
                Property.CLIENTCOMMENT = Val.ToString(txtClientComment.Text);
                Property.POLISHFEATURES = Val.ToString(txtPolishFeatures.Text);
                Property.SYNTHETICINDICATOR = Val.ToString(txtSymIndiction.Text);

                Property.MFGRAPAPORT = Val.Val(txtMfgRapaport.Text);
                Property.MFGDISCOUNT = Val.Val(txtMfgDiscount.Text);
                Property.MFGPRICEPERCARAT = Val.Val(txtMfgPricePerCarat.Text);
                Property.MFGAMOUNT = Val.Val(txtMfgAmount.Text);

                Property.SALERAPAPORT = Val.Val(txtSaleRapaport.Text);
                Property.SALEDISCOUNT = Val.Val(txtSaleDiscount.Text);
                Property.SALEPRICEPERCARAT = Val.Val(txtSalePricePerCarat.Text);
                Property.SALEAMOUNT = Val.Val(txtSaleAmount.Text);

                Property.COSTRAPAPORT = Val.Val(txtCostRapaport.Text);
                Property.COSTDISCOUNT = Val.Val(txtCostDiscount.Text);
                Property.COSTPRICEPERCARAT = Val.Val(txtCostPricePerCarat.Text);
                Property.COSTAMOUNT = Val.Val(txtCostAmount.Text);

                Property.RAPNETRAPAPORT = Val.Val(txtRapnetRapaport.Text);
                Property.RAPNETDISCOUNT = Val.Val(txtRapnetDiscount.Text);
                Property.RAPNETPRICEPERCARAT = Val.Val(txtRapnetPricePerCarat.Text);
                Property.COMPAMOUNT = Val.Val(txtRapnetAmount.Text);

                Property.JAMESALLENRAPAPORT = Val.Val(txtJARapaport.Text);
                Property.JAMESALLENDISCOUNT = Val.Val(txtJADiscount.Text);
                Property.JAMESALLENPRICEPERCARAT = Val.Val(txtJAPricePerCarat.Text);
                Property.JAMESALLENAMOUNT = Val.Val(txtJAAmount.Text);

                Property.EXPRAPAPORT = Val.Val(txtExpRapaport.Text);
                Property.EXPDISCOUNT = Val.Val(txtExpDiscount.Text);
                Property.EXPPRICEPERCARAT = Val.Val(txtExpPricePerCarat.Text);
                Property.EXPAMOUNT = Val.Val(txtExpAmount.Text);

                Property.COMPRAPAPORT = Val.Val(txtCompRapaport.Text);
                Property.COMPDISCOUNT = Val.Val(txtCompDiscount.Text);
                Property.COMPPRICEPERCARAT = Val.Val(txtCompPricePerCarat.Text);
                Property.COMPAMOUNT = Val.Val(txtCompAmount.Text);

                //Property.MEASUREMENT = Val.ToString(TxtMasurment.Text);
                Property.MEASUREMENT = Val.ToString(txtLength.Text) + "*" + Val.ToString(txtWidth.Text) + "*" + Val.ToString(txtHeight.Text);

                Property = ObjStock.UpdateSingleStock(Property);

                string StrReturnDesc = Property.ReturnMessageDesc;

                this.Cursor = Cursors.Default;
                Global.Message(StrReturnDesc);
                if (Property.ReturnMessageType == "SUCCESS")
                {
                    Clear();
                }
                else
                {
                    txtStoneNo.Focus();
                }
            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
            }
        }

        private void txtMfgDiscount_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                DouSuratPricePerCarat = Math.Round(Val.Val(txtMfgRapaport.Text) + ((Val.Val(txtMfgRapaport.Text) * Val.Val(txtMfgDiscount.Text)) / 100), 2);
                DouSuratAmount = Math.Round((DouSuratPricePerCarat * Val.Val(txtCarat.Text)), 2);
                txtMfgPricePerCarat.Text = Val.ToString(DouSuratPricePerCarat);
                txtMfgAmount.Text = Val.ToString(DouSuratAmount);
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void txtCostDiscount_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                DouCostPricePerCarat = Math.Round(Val.Val(txtCostRapaport.Text) + ((Val.Val(txtCostRapaport.Text) * Val.Val(txtCostDiscount.Text)) / 100), 2);
                DouCostAmount = Math.Round((DouCostPricePerCarat * Val.Val(txtCarat.Text)), 2);
                txtCostPricePerCarat.Text = Val.ToString(DouCostPricePerCarat);
                txtCostAmount.Text = Val.ToString(DouCostAmount);
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void txtSaleDiscount_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                DouSalePricePerCarat = Math.Round(Val.Val(txtSaleRapaport.Text) + ((Val.Val(txtSaleRapaport.Text) * Val.Val(txtSaleDiscount.Text)) / 100), 2);
                DouSaleAmount = Math.Round((DouSalePricePerCarat * Val.Val(txtCarat.Text)), 2);
                txtSalePricePerCarat.Text = Val.ToString(DouSalePricePerCarat);
                txtSaleAmount.Text = Val.ToString(DouSaleAmount);
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void txtRapnetDiscount_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                DouRapnetPricePerCarat = Math.Round(Val.Val(txtRapnetRapaport.Text) + ((Val.Val(txtRapnetRapaport.Text) * Val.Val(txtRapnetDiscount.Text)) / 100), 2);
                DouRapnetAmount = Math.Round((DouRapnetPricePerCarat * Val.Val(txtCarat.Text)), 2);
                txtRapnetPricePerCarat.Text = Val.ToString(DouRapnetPricePerCarat);
                txtRapnetAmount.Text = Val.ToString();
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void txtJADiscount_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                DouJAPricePerCarat = Math.Round(Val.Val(txtJARapaport.Text) + ((Val.Val(txtJARapaport.Text) * Val.Val(txtJADiscount.Text)) / 100), 2);
                DouJAAmount = Math.Round((DouJAPricePerCarat * Val.Val(txtCarat.Text)), 2);
                txtJAPricePerCarat.Text = Val.ToString(DouJAPricePerCarat);
                txtJAAmount.Text = Val.ToString(DouJAAmount);
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void txtExpDiscount_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                DouExpPricePerCarat = Math.Round(Val.Val(txtExpRapaport.Text) + ((Val.Val(txtExpRapaport.Text) * Val.Val(txtExpDiscount.Text)) / 100), 2);
                DouExpAmount = Math.Round((DouExpPricePerCarat * Val.Val(txtCarat.Text)), 2);
                txtExpPricePerCarat.Text = Val.ToString(DouExpPricePerCarat);
                txtExpAmount.Text = Val.ToString(DouExpAmount);
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }

        }

        private void txtCostPricePerCarat_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                double DouPer = Math.Round(((Val.Val(txtCostPricePerCarat.Text) - Val.Val(txtCostRapaport.Text)) / Val.Val(txtCostRapaport.Text)) * 100, 2);
                txtCostDiscount.Text = Val.ToString(DouPer);
                txtCostAmount.Text = Val.ToString(Math.Round(Val.Val(txtCarat.Text) * Val.Val(txtCostPricePerCarat.Text), 2));
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void txtSalePricePerCarat_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                double DouPer = Math.Round(((Val.Val(txtSalePricePerCarat.Text) - Val.Val(txtSaleRapaport.Text)) / Val.Val(txtSaleRapaport.Text)) * 100, 2);
                txtSaleDiscount.Text = Val.ToString(DouPer);
                txtSaleAmount.Text = Val.ToString(Math.Round(Val.Val(txtCarat.Text) * Val.Val(txtSalePricePerCarat.Text), 2));
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void txtRapnetPricePerCarat_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                double DouPer = Math.Round(((Val.Val(txtRapnetPricePerCarat.Text) - Val.Val(txtRapnetRapaport.Text)) / Val.Val(txtRapnetRapaport.Text)) * 100, 2);
                txtRapnetDiscount.Text = Val.ToString(DouPer);
                txtRapnetAmount.Text = Val.ToString(Math.Round(Val.Val(txtCarat.Text) * Val.Val(txtRapnetPricePerCarat.Text), 2));
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void txtJAPricePerCarat_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                double DouPer = Math.Round(((Val.Val(txtJAPricePerCarat.Text) - Val.Val(txtJARapaport.Text)) / Val.Val(txtJARapaport.Text)) * 100, 2);
                txtJADiscount.Text = Val.ToString(DouPer);
                txtJAAmount.Text = Val.ToString(Math.Round(Val.Val(txtCarat.Text) * Val.Val(txtJAPricePerCarat.Text), 2));
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void txtExpPricePerCarat_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                double DouPer = Math.Round(((Val.Val(txtExpPricePerCarat.Text) - Val.Val(txtExpRapaport.Text)) / Val.Val(txtExpRapaport.Text)) * 100, 2);
                txtExpDiscount.Text = Val.ToString(DouPer);
                txtExpAmount.Text = Val.ToString(Math.Round(Val.Val(txtCarat.Text) * Val.Val(txtExpPricePerCarat.Text), 2));
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void txtMfgPricePerCarat_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                double DouPer = Math.Round(((Val.Val(txtMfgPricePerCarat.Text) - Val.Val(txtMfgRapaport.Text)) / Val.Val(txtMfgRapaport.Text)) * 100, 2);
                txtMfgDiscount.Text = Val.ToString(DouPer);
                txtMfgAmount.Text = Val.ToString(Math.Round(Val.Val(txtCarat.Text) * Val.Val(txtMfgPricePerCarat.Text), 2));
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void lblBackDetail_Click(object sender, EventArgs e)
        {
            string StrXml = txtCompDiscount.AccessibleDescription;
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

        private void txtProcess_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PROCESSCODE,PROCESSNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_PROCESS);

                    FrmSearch.mStrColumnsToHide = "STATE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtProcess.Text = Val.ToString(FrmSearch.DRow["PROCESSNAME"]);
                        txtProcess.Tag = Val.ToString(FrmSearch.DRow["PROCESS_ID"]);
                        lblStatus.Text = Val.ToString(FrmSearch.DRow["WEBSTATUS"]);
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

        private void txtCarat_Validating(object sender, CancelEventArgs e)
        {
            FindRap();
        }

        private void TabSummaryDetail_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void CmbSideTable_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
