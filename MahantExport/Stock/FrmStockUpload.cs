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
using SDK_SC_RFID_Devices;
using DataClass;
using System.Reflection;
using BusLib.Rapaport;

namespace MahantExport.Stock
{
    public partial class FrmStockUpload : DevControlLib.cDevXtraForm
    {
        public delegate void SetControlValueCallback(Control oControl, string propName, object propValue);

        System.Diagnostics.Stopwatch watch = null;
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFindRap ObjRap = new BOFindRap();

        BOTRN_RFIDConnection ObjBORFIDCon = new BOTRN_RFIDConnection();
        RFID_Device RFIDCurrDevice;
        int IntCntScanTotal = 0, IntCntScanMatched = 0, IntCntScanUnMatched = 0;
        bool IsNextImage = false;

        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        DataTable DtabStockUpload = new DataTable();
        DataTable DtabExcelData = new DataTable();

        DataTable DtabFinalData = new DataTable();
        DataTable DtabPara = new DataTable();
        DataTable DtabSupplier = new DataTable();
        DataTable DtabExcRateCurrencyWise = new DataTable();
        DataTable DtabStockSync = new DataTable();
        DataTable DtabStockDateWiseSummary = new DataTable();
        DataTable DtabPrdType = new DataTable();

        DataTable DtabLot = new DataTable();

        BODevGridSelection ObjGridSelection;

        int mIntCurrencyID = 0;
        double mDouExcRate = 0;

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
        string mStrParty = "";
        string mStrStockStatus = "";
        int mIntPrdType_ID = 0;
        string StrBillFormat = "";
        string StrBillType = "";

        #region Property Settings

        public FrmStockUpload()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            mStrOpe = "";
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            mTrn_ID = BusLib.Configuration.BOConfiguration.FindNewSequentialID();

            //DtabPara = new BOMST_Parameter().GetParameterData();
            //DtabExcRateCurrencyWise = new BOComboFill().FillCmb(BusLib.BOComboFill.TABLE.MST_CURRENTEXCRATECURRENCYWISE);

            mStrStockType = "";

            DtabLot = new BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.TRN_KAPAN);

            CmbCurrency.DataSource = new BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CURRENCY);
            CmbCurrency.DisplayMember = "CURRENCYNAME";
            CmbCurrency.ValueMember = "CURRENCY_ID";
            CmbCurrency.SelectedIndex = -1;

            cmbPrdType.DataSource = new BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PRDTYPE);
            cmbPrdType.DisplayMember = "PRDTYPENAME";
            cmbPrdType.ValueMember = "PRDTYPE_ID";
            cmbPrdType.SelectedIndex = 0;

            Clear();
            Fill();

            //#K: 20-11-2020
            RFIDCurrDevice = BOConfiguration.RFIDCurrDevice;
            if (RFIDCurrDevice != null)
            {
                RFIDCurrDevice.NotifyRFIDEvent += (rfidDev_NotifyRFIDEvent);
                lblDeviceName.Text = "Info : Device Connected";
                BtnScan.Enabled = true;
                BtnStop.Enabled = true;
                BtnLEDAllAtOnce.Enabled = true;
            }
            else
            {
                lblDeviceName.Text = "Info : No Device Detected";
                BtnScan.Enabled = false;
                BtnStop.Enabled = false;
                BtnLEDAllAtOnce.Enabled = false;
            }

            lblBillType.Visible = false;
            cmbBillType.Visible = false;
            cmbBillType.SelectedIndex = 0;

            this.Show();

        }

        public void ShowForm(string StrStockType)
        {
            mStrOpe = "";
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            mTrn_ID = BusLib.Configuration.BOConfiguration.FindNewSequentialID();

            //DtabPara = new BOMST_Parameter().GetParameterData();
            //DtabExcRateCurrencyWise = new BOComboFill().FillCmb(BusLib.BOComboFill.TABLE.MST_CURRENTEXCRATECURRENCYWISE);

            mStrStockType = StrStockType;

            CmbCurrency.DataSource = new BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CURRENCY);
            CmbCurrency.DisplayMember = "CURRENCYNAME";
            CmbCurrency.ValueMember = "CURRENCY_ID";
            CmbCurrency.SelectedIndex = -1;

            DtabPrdType = new BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_PRDTYPE);
            cmbPrdType.DataSource = DtabPrdType;
            cmbPrdType.DisplayMember = "PRDTYPENAME";
            cmbPrdType.ValueMember = "PRDTYPE_ID";

            this.Text = mStrStockType + " " + this.Text;


            Clear();
            Fill();
            //#K: 20-11-2020
            RFIDCurrDevice = BOConfiguration.RFIDCurrDevice;
            if (RFIDCurrDevice != null)
            {
                RFIDCurrDevice.NotifyRFIDEvent += (rfidDev_NotifyRFIDEvent);
                lblDeviceName.Text = "Info : Device Connected";
                BtnScan.Enabled = true;
                BtnStop.Enabled = true;
                BtnLEDAllAtOnce.Enabled = true;
            }
            else
            {
                lblDeviceName.Text = "Info : No Device Detected";
                BtnScan.Enabled = false;
                BtnStop.Enabled = false;
                BtnLEDAllAtOnce.Enabled = false;
            }
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
            DtabLot = new BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.TRN_KAPAN);

            DtabPara = new BOMST_Parameter().GetParameterData();

            //#K: 20-11-2020
            RFIDCurrDevice = BOConfiguration.RFIDCurrDevice;
            if (RFIDCurrDevice != null)
            {
                RFIDCurrDevice.NotifyRFIDEvent += (rfidDev_NotifyRFIDEvent);
                lblDeviceName.Text = "Info : Device Connected";
                BtnScan.Enabled = true;
                BtnStop.Enabled = true;
                BtnLEDAllAtOnce.Enabled = true;
            }
            else
            {
                lblDeviceName.Text = "Info : No Device Detected";
                BtnScan.Enabled = false;
                BtnStop.Enabled = false;
                BtnLEDAllAtOnce.Enabled = false;
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
            else if (Val.ToString(CmbSheetName.Text).Trim().Equals(string.Empty))
            {
                Global.Message("Please Select Sheet.");
                CmbSheetName.Focus();
                return true;
            }

            if (CmbStockStatus.SelectedItem.ToString() == "PURCHASE")
            {
                if (Val.ToString(cmbBillType.SelectedItem) == "")
                {
                    Global.Message("Please Select BillType.");
                    cmbBillType.Focus();
                    return true;
                }
            }
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
            DtabPara = new BOMST_Parameter().GetParameterData();
            DtabExcRateCurrencyWise = new BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CURRENTEXCRATECURRENCYWISE);

            DtabLot = new BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.TRN_KAPAN);

            txtPartyName.Tag = string.Empty;
            txtPartyName.Text = string.Empty;
            txtFileName.Text = string.Empty;
            //CmbSheetname.Text = string.Empty;
            //CmbSheetname.SelectedIndex = 0;
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

            CmbStockStatus.SelectedIndex = 0;
            //CmbStockType.SelectedIndex = 0;
            //CmbCurrency.SelectedIndex = 0;


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
            }

        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                CmbSheetName.Items.Clear();
                OpenFileDialog Open = new OpenFileDialog();
                Open.Filter = "Excel Files|*.xls;*.xlsx";
                if (Open.ShowDialog() == DialogResult.OK)
                {
                    txtFileName.Text = Open.FileName;
                    Global.SprirelGetSheetNameFromExcel(CmbSheetName, txtFileName.Text);
                }

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
                CmbSheetName.Items.Clear(); //ADD:KULDEEP[24/05/18]
                foreach (DataRow row in dt.Rows)
                {
                    string sheetName = (string)row["TABLE_NAME"];
                    sheets.Add(sheetName);
                    CmbSheetName.Items.Add(sheetName);
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

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                MainGrdStock.DataSource = null;

                DtabPara = new BOMST_Parameter().GetParameterData();

                DtabExcelData = new DataTable();
                DtabFinalData.Rows.Clear();

                if (ValSave())
                {
                    this.Cursor = Cursors.Default;
                    return;
                }

                BtnCalculate.Enabled = false;

                mStrStockType = Val.ToString(CmbStockType.SelectedItem).ToUpper();
                mStrParty = Val.ToString(txtPartyName.Tag);
                mIntPrdType_ID = Val.ToInt32(cmbPrdType.SelectedValue);
                mIntCurrencyID = Val.ToInt32(CmbCurrency.SelectedValue);
                mDouExcRate = Val.Val(txtExcRate.Text);
                mStrStockStatus = Val.ToString(CmbStockStatus.SelectedItem);

                SetControlPropertyValue(lblMessage, "Text", "Message");

                DtabExcelData = Global.SprireGetDataTableFromExcel(txtFileName.Text, Val.ToString(CmbSheetName.SelectedItem), true);
                StrUploadFilename = Path.GetFileName(txtFileName.Text);

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
                        //string Str = "Combination For This Excel Columns-> '" + StrNotCompNotExist + "' Is Not Exist Still You Want To Continue ? ";
                        BtnCalculate.Enabled = true;
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

                        BtnCalculate.Enabled = true;
                        Global.Message("This Columns -> '" + StrCompColumn.Substring(1) + "' Are Required In Excel Sheet For Store Pricing Data..");
                        this.Cursor = Cursors.Default;
                        return;
                    }

                    var VarQryChk = (from DrSetting in DtabExcelSetting.AsEnumerable()
                                     where Val.ToBooleanToInt(DrSetting["ISCOMPULSORYINPARCEL"]) == 1
                                     select DrSetting).ToList();
                    if (VarQryChk.Any())
                    {
                        foreach (DataRow DCol in VarQryChk) //Check compulsory column contain value or not Add Khushbu 08-02-22
                        {
                            string StrCol = DCol["EXCELSETTINGNAME"].ToString();
                            foreach (DataRow DRow in DtabExcelData.Rows)
                            {
                                if (Val.ToString(DRow[StrCol]) == "")
                                {
                                    BtnCalculate.Enabled = true;
                                    Global.Message(StrCol + "  -->> ' Value Are Required In Excel Sheet For Store Pricing Data..");
                                    this.Cursor = Cursors.Default;
                                    return;
                                }
                            }
                        }
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

                        BtnCalculate.Enabled = true;
                        Global.Message("This Columns -> '" + StrCompColumn.Substring(1) + "' Are Required In Excel Sheet For Store Pricing Data..");
                        this.Cursor = Cursors.Default;
                        return;
                    }
                }

                //ADD BY RAJVI FOR BARCODENO MAX GENERATE: 27/06/2025
                if (DtabExcelData != null && DtabExcelData.Rows.Count > 0)
                {
                    long maxBarcodeNo = 25001;
                    if (!DtabExcelData.Columns.Contains("BARCODENO"))
                        DtabExcelData.Columns.Add("BARCODENO", typeof(long));

                    foreach (DataRow row in DtabExcelData.Rows)
                    {
                        long temp = Val.ToInt64(row["BARCODENO"]);
                        if (temp > maxBarcodeNo)
                            maxBarcodeNo = temp;
                    }

                    string splitColumnName = "PARTYSTOCKNO";

                    List<DataRow> newRows = new List<DataRow>();

                    foreach (DataRow row in DtabExcelData.Rows)
                    {
                        if (row[splitColumnName] != DBNull.Value && row[splitColumnName].ToString().Contains(","))
                        {
                            string[] items = row[splitColumnName].ToString().Split(',');

                            foreach (string item in items)
                            {
                                DataRow newRow = DtabExcelData.NewRow();

                                foreach (DataColumn col in DtabExcelData.Columns)
                                {
                                    if (col.ColumnName == "BARCODENO")
                                        continue;

                                    newRow[col.ColumnName] = row[col.ColumnName];
                                }

                                newRow[splitColumnName] = item.Trim();

                                maxBarcodeNo++;
                                newRow["BARCODENO"] = maxBarcodeNo;

                                newRows.Add(newRow);
                            }
                            row["__DeleteMe__"] = true;
                        }
                        else
                        {
                            if (Val.ToInt64(row["BARCODENO"]) == 0)
                            {
                                maxBarcodeNo++;
                                row["BARCODENO"] = maxBarcodeNo;
                            }
                        }
                    }
                    foreach (var newRow in newRows)
                        DtabExcelData.Rows.Add(newRow);

                    MainGrdStock.DataSource = DtabExcelData;
                    MainGrdStock.Refresh();

                    DevExpress.XtraGrid.Views.Grid.GridView view = MainGrdStock.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
                    if (view != null)
                    {
                        if (view.Columns["BARCODENO"] != null)
                        {
                            view.Columns["BARCODENO"].Visible = true;
                            view.Columns["BARCODENO"].Caption = "Barcode No";
                            view.Columns["BARCODENO"].BestFit();
                        }
                        int barcodeCount = DtabExcelData.AsEnumerable()
                                              .Select(r => Val.ToString(r["BARCODENO"]))
                                              .Distinct()
                                              .Count();
                        //Global.Message("Total Barcode(s): " + barcodeCount);
                    }
                }
                //END RAJVI

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
                    this.Cursor = Cursors.Default;

                    if (backgroundWorker1.IsBusy)
                    {
                        backgroundWorker1.CancelAsync();
                    }
                    watch = System.Diagnostics.Stopwatch.StartNew();
                    progressPanel1.Visible = true;
                    backgroundWorker1.RunWorkerAsync();
                
            }
            catch (Exception EX)
            {
                this.Cursor = Cursors.Default;
                BtnCalculate.Enabled = true;
                Global.MessageError(EX.Message);
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
                string StrCol = Val.ToString(GrdDetStock.GetRowCellValue(e.RowHandle, "PROCESS_ID")).ToUpper();

                if (StrCol.ToUpper() == "14")
                {
                    e.Appearance.BackColor = lblAvailable.BackColor;
                    e.Appearance.BackColor2 = lblAvailable.BackColor;
                }
                else if (StrCol.ToUpper() == "1")
                {
                    e.Appearance.BackColor = lblNone.BackColor;
                    e.Appearance.BackColor2 = lblNone.BackColor;
                }
                else if (StrCol.ToUpper() == "4")
                {
                    e.Appearance.BackColor = lblMemo.BackColor;
                    e.Appearance.BackColor2 = lblMemo.BackColor;
                }
                else if (StrCol.ToUpper() == "11")
                {
                    e.Appearance.BackColor = lblSold.BackColor;
                    e.Appearance.BackColor2 = lblSold.BackColor;
                }
                else if (StrCol.ToUpper() == "9")
                {
                    e.Appearance.BackColor = lblInvoice.BackColor;
                    e.Appearance.BackColor2 = lblInvoice.BackColor;
                }
                else if (StrCol.ToUpper() == "3")
                {
                    e.Appearance.BackColor = lblPurchaseReturn.BackColor;
                    e.Appearance.BackColor2 = lblPurchaseReturn.BackColor;
                }
                //Kuldeep End

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
                    FrmSearch.mStrSearchField = "LEDGERCODE,LEDGERNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LEDGER);

                    FrmSearch.mStrColumnsToHide = "LEDGER_ID,COMPANYNAME";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtPartyName.Text = Val.ToString(FrmSearch.DRow["LEDGERNAME"]);
                        txtPartyName.Tag = Val.ToString(FrmSearch.DRow["LEDGER_ID"]);
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
                lblBillType.Visible = true;
                cmbBillType.Visible = true;
            }
            else
            {
                RbtReplaceAllStock.Enabled = true;
                RbtAppendStock.Enabled = true;
                lblBillType.Visible = false;
                cmbBillType.Visible = false;
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
                aryLst = ObjGridSelection.GetSelectedArrayList();
                resultTable = sourceTable.Clone();
                for (int i = 0; i < aryLst.Count; i++)
                {
                    DataRowView oDataRowView = aryLst[i] as DataRowView;
                    resultTable.Rows.Add(oDataRowView.Row.ItemArray);
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
                double DCPcs = 0;
                double DCRapaport = 0;
                double DCRapaportAmt = 0;
                double DCDiscount = 0;
                DtabStockSync.AcceptChanges();

                foreach (DataRow DRow in DtabStockSync.Rows)
                {
                    DCRapaportAmt += Val.Val(DRow["COSTRAPAPORT"]) * Val.Val(DRow["CARAT"]);
                }

                DCCarat = Val.Val(DtabStockSync.Compute("SUM(CARAT)", string.Empty));
                //DCPcs = Val.Val(DtabStockSync.Compute("SUM(PCS)", string.Empty));

                txtTotalCarat.Text = string.Format("{0:0.00}", DCCarat, string.Empty);
                //TxtTotalPcs.Text = string.Format("{0:00}", DCPcs, string.Empty);

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

            if (ObjGridSelection == null)
                return;

            DataTable DTab = GetTableOfSelectedRows(GrdDetStock, true);

            if (DTab != null && DTab.Rows.Count > 0)
            {
                txtSelectedCarat.Text = Val.ToString(DTab.Compute("SUM(CARAT)", string.Empty));
                TxtSelectedPcs.Text = Val.ToString(DTab.Compute("SUM(PCS)", string.Empty));
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
                txtSelectedCarat.Text = string.Empty;
                TxtSelectedPcs.Text = string.Empty;
                txtSelectedDisc.Text = string.Empty;
                txtSelectedAmount.Text = string.Empty;
                txtSelectedAvgRap.Text = string.Empty;
                txtSelectedPricePerCarat.Text = string.Empty;
            }
        }

        private void GrdDetStock_MouseUp(object sender, MouseEventArgs e)
        {
            //try
            //{
            //    CalculateSelectedSummary();
            //}
            //catch (Exception ex)
            //{
            //    Global.Message(ex.Message.ToString());
            //}
        }

        private void FrmStockUpload_KeyUp(object sender, KeyEventArgs e)
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

        // Kuldeep For RFID 09112020
        private void BtnFindAndConnect_Click(object sender, EventArgs e)
        {
            //DisposeRFIDObject();
            ////Kuldeep 09112020 For RFID Work.
            //lblDeviceName.Text = ObjBORFIDCon.FindDevice();
            //if (lblDeviceName.Text != "Info : No device detected")
            //{
            //    RFIDCurrDevice = ObjBORFIDCon.ConnectDevice();
            //    if (RFIDCurrDevice != null)
            //        RFIDCurrDevice.NotifyRFIDEvent += (rfidDev_NotifyRFIDEvent);
            //    else
            //        lblDeviceName.Text = "Device Not Found";
            //}
        }

        private void rfidDev_NotifyRFIDEvent(object sender, SDK_SC_RfidReader.rfidReaderArgs args)
        {
            switch (args.RN_Value)
            {
                // Event when failed to connect          
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_FailedToConnect:
                    Global.Message("Info : Failed to connect");
                    Invoke((MethodInvoker)delegate
                    {
                        BtnFindAndConnect.Enabled = true;
                    });
                    break;
                // Event when release the object
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_Disconnected:
                    Invoke((MethodInvoker)delegate
                    {
                        BtnScan.Enabled = false;
                        BtnStop.Enabled = false;
                        BtnLEDAllAtOnce.Enabled = false;
                    });
                    break;

                //Event when device is connected
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_Connected:
                    Global.Message("Info : Device connected");
                    Invoke((MethodInvoker)delegate
                    {
                        BtnFindAndConnect.Enabled = false;
                        BtnScan.Enabled = true;

                    });
                    break;

                // Event when scan started
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_ScanStarted:
                    Global.Message("Info : Scan started");
                    Invoke((MethodInvoker)delegate
                    {
                        BtnScan.Enabled = false;
                        BtnStop.Enabled = true;
                    });
                    break;

                //event when fail to start scan
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_ReaderFailToStartScan:
                    Invoke((MethodInvoker)delegate
                    {
                        BtnScan.Enabled = true;
                        BtnStop.Enabled = false;
                    });
                    Global.Message("Info : Failed to start scan");
                    break;

                //event when a new tag is identify
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_TagAdded:
                    lblTotal.Invoke((MethodInvoker)delegate
                    {
                        IntCntScanTotal++;
                        if (GrdDetStock.RowCount > 0)
                        {
                            SetGrdCheckBoxValue(args.Message);
                            lblMatched.Text = Val.ToString(IntCntScanMatched);
                            lblUnMatched.Text = Val.ToString(IntCntScanTotal - IntCntScanMatched);
                        }
                        else
                        {
                            RbtGIAControlNo.Checked = true;
                            if (IntCntScanTotal == 1)
                                txtJangedNo.Text = args.Message;
                            else
                                txtJangedNo.Text += "," + args.Message;
                        }
                        lblTotal.Text = Val.ToString(IntCntScanTotal);

                    });
                    Application.DoEvents();
                    break;

                // Event when scan completed
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_ScanCompleted:
                    Invoke((MethodInvoker)delegate
                    {
                        if (GrdDetStock.RowCount > 0)
                            BtnLEDAllAtOnce.Enabled = true;
                        BtnScan.Enabled = true;
                        BtnStop.Enabled = false;
                    });
                    Global.Message("Info : Scan completed");
                    break;

                //error when error during scan
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_ReaderScanTimeout:
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_ErrorDuringScan:
                    Invoke((MethodInvoker)delegate
                    {
                        BtnScan.Enabled = true;
                        BtnStop.Enabled = false;
                    });
                    Global.Message("Info : Scan has error");
                    break;
                case SDK_SC_RfidReader.rfidReaderArgs.ReaderNotify.RN_ScanCancelByHost:
                    Invoke((MethodInvoker)delegate
                    {
                        BtnScan.Enabled = true;
                        BtnStop.Enabled = false;
                    });
                    Global.Message("Info : Scan cancel by host");
                    break;
            }

            Application.DoEvents();
        }

        private void BtnScan_Click(object sender, EventArgs e)
        {
            txtJangedNo.Invoke((MethodInvoker)delegate
            {
                txtJangedNo.Text = "";
            });
            IntCntScanTotal = 0;
            IntCntScanMatched = 0; IntCntScanUnMatched = 0;
            if (RFIDCurrDevice != null)
            {
                if (!ObjBORFIDCon.StartScan(RFIDCurrDevice, true))
                    MessageBox.Show("Device is not ready or not connected");
            }
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            ObjBORFIDCon.StopScan(RFIDCurrDevice);
        }

        private void BtnLEDAllAtOnce_Click(object sender, EventArgs e)
        {
            if (GrdDetStock.RowCount >= 0)
            {
                ArrayList aryLst = new ArrayList();
                DataTable resultTable = new DataTable();
                List<string> listBoxTag = new List<string>();

                if (true)
                {
                    aryLst = ObjGridSelection.GetSelectedArrayList();
                    for (int i = 0; i < aryLst.Count; i++)
                    {
                        DataRowView oDataRowView = aryLst[i] as DataRowView;
                        listBoxTag.Add(oDataRowView["GIACONTROLNUMBER"].ToString());
                    }
                }
                ObjBORFIDCon.LedOnAll(RFIDCurrDevice, listBoxTag.Cast<string>().ToList());
                Global.Message("After disappear Of Message LED Will Stop"); // while user doesn't close the dialog, the led-lighting thread is still running
                RFIDCurrDevice.StopLightingLeds(); // stops lighting once user closed MessageBox
            }
        }

        private void SetGrdCheckBoxValue(string pStrControlNo)
        {
            if (GrdDetStock.RowCount >= 0)
            {
                ColumnView view = MainGrdStock.MainView as ColumnView;
                GridColumn grdColmnGIAControlNo = view.Columns["GIACONTROLNUMBER"];

                int i = GrdDetStock.LocateByDisplayText(0, grdColmnGIAControlNo, pStrControlNo);
                if (i > -1)
                {
                    IntCntScanMatched++;
                    GrdDetStock.SetRowCellValue(i, "COLSELECTCHECKBOX", true);
                }
            }
        }

        private void FrmStockUpload_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (RFIDCurrDevice != null)
                {
                    RFIDCurrDevice.NotifyRFIDEvent -= (rfidDev_NotifyRFIDEvent);
                }
            }
            catch { }
        }

        private void DisposeRFIDObject()
        {
            if (RFIDCurrDevice == null) return;

            if (RFIDCurrDevice.ConnectionStatus == ConnectionStatus.CS_Connected)
                RFIDCurrDevice.ReleaseDevice();
        }

        private void BtnLeft_Click(object sender, EventArgs e)
        {
            if (IsNextImage)
            {
                BtnLeft.Image = MahantExport.Properties.Resources.A1;
                GrpRFIDBox.Visible = false;
                IsNextImage = false;
            }
            else
            {
                BtnLeft.Image = MahantExport.Properties.Resources.A2;
                GrpRFIDBox.Visible = true;
                IsNextImage = true;
            }
        }

        public int FindID_FancyColor(DataTable pDTab, string pStrValue, string pStrStoneNo, string pStrColumnName, ref int IntCheck, ref string StrMessage)
        {
            try
            {
                pStrValue = pStrValue.Trim().ToUpper();

                if (pStrValue != "")
                {
                    var dr = (from DrPara in pDTab.AsEnumerable()
                              where Val.ToString(DrPara["REMARK"]).ToUpper().Split(',').Contains(pStrValue)
                              select DrPara);
                    IntCheck = dr.Count() > 0 ? Val.ToInt(dr.FirstOrDefault()["FANCYCOLOR_ID"]) : 0;
                    if (IntCheck == 0)
                    {
                        StrMessage = "Stone No : " + pStrStoneNo + " -> Column : [ " + pStrColumnName + " ] Has Invalid Values [ " + pStrValue + " ]";
                        IntCheck = -1;
                        return IntCheck;
                    }
                    else
                    {
                        return IntCheck;
                    }
                }
                else
                {
                    IntCheck = 0;
                    return IntCheck;
                }
            }
            catch (Exception ex)
            {
                IntCheck = -1;
                StrMessage = pStrStoneNo + " -> " + ex.Message;
                return IntCheck;
            }
        }

        public int FindID_FancyColor_WithOvertone(DataTable pDTab, string pStrValue, string FancyOvertone, string FancyIntensity, string pStrStoneNo, string pStrColumnName, ref int IntCheck, ref string StrMessage)
        {
            try
            {
                pStrValue = pStrValue.Trim().ToUpper();
                FancyOvertone = FancyOvertone.Trim().ToUpper();
                FancyIntensity = FancyIntensity.Trim().ToUpper();

                if (pStrValue != "")
                {
                    var dr = (from DrPara in pDTab.AsEnumerable()
                              where Val.ToString(DrPara["FANCYCOLOR"]).ToUpper() == pStrValue && Val.ToString(DrPara["FANCYOVERTONE"]).ToUpper() == FancyOvertone && Val.ToString(DrPara["FANCYINTENSITY"]).ToUpper() == FancyIntensity
                              select DrPara);
                    IntCheck = dr.Count() > 0 ? Val.ToInt(dr.FirstOrDefault()["FANCYCOLOR_ID"]) : 0;
                    if (IntCheck == 0)
                    {
                        StrMessage = "Stone No : " + pStrStoneNo + " -> Column : [ " + pStrColumnName + " ] Has Invalid Values [ " + pStrValue + " ]";
                        IntCheck = -1;
                        return IntCheck;
                    }
                    else
                    {
                        return IntCheck;
                    }
                }
                else
                {
                    IntCheck = 0;
                    return IntCheck;
                }
            }
            catch (Exception ex)
            {
                IntCheck = -1;
                StrMessage = pStrStoneNo + " -> " + ex.Message;
                return IntCheck;
            }
        }

        public int FindID(DataTable pDTab, string pStrValue, string pStrStoneNo, string pStrColumnName, ref int IntCheck, ref string StrMessage)
        {
            try
            {
                pStrValue = pStrValue.Trim().ToUpper();

                if (pStrValue != "")
                {
                    var dr = (from DrPara in pDTab.AsEnumerable()
                              where Val.ToString(DrPara["LABCODE"]).ToUpper().Split(',').Contains(pStrValue)
                              select DrPara);
                    IntCheck = dr.Count() > 0 ? Val.ToInt(dr.FirstOrDefault()["PARA_ID"]) : 0;
                    if (IntCheck == 0)
                    {
                        StrMessage = "Stone No : " + pStrStoneNo + " -> Column : [ " + pStrColumnName + " ] Has Invalid Values [ " + pStrValue + " ]";
                        IntCheck = -1;
                        return IntCheck;
                    }
                    else
                    {
                        return IntCheck;
                    }
                }
                else
                {
                    IntCheck = 0;
                    return IntCheck;
                }
            }
            catch (Exception ex)
            {
                IntCheck = -1;
                StrMessage = pStrStoneNo + " -> " + ex.Message;
                return IntCheck;
            }
        }

        public string FindIDWithGUIDType(DataTable pDTab, string pStrValue, string pStrStoneNo, string pStrColumnName, ref string IntCheck, ref string StrMessage)
        {
            try
            {
                pStrValue = pStrValue.Trim().ToUpper();

                if (pStrValue != "")
                {
                    var dr = (from DrPara in pDTab.AsEnumerable()
                              where Val.ToString(DrPara["KAPANCODE"]).ToUpper().Split(',').Contains(pStrValue)
                              select DrPara);
                    IntCheck = dr.Count() > 0 ? Val.ToString(dr.FirstOrDefault()["KAPAN_ID"]) : "FAIL";
                    if (IntCheck == "FAIL")
                    {
                        StrMessage = "Stone No : " + pStrStoneNo + " -> Column : [ " + pStrColumnName + " ] Has Invalid Values [ " + pStrValue + " ]";
                        IntCheck = "FAIL";
                        return IntCheck;
                    }
                    else
                    {
                        return IntCheck;
                    }
                }
                else
                {
                    IntCheck = "";
                    return IntCheck;
                }
            }
            catch (Exception ex)
            {
                IntCheck = "FAIL";
                StrMessage = pStrStoneNo + " -> " + ex.Message;
                return IntCheck;
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DataTable DTShape = DtabPara.Select("PARATYPE = 'SHAPE'").CopyToDataTable();
                DataTable DTColor = DtabPara.Select("PARATYPE = 'COLOR'").CopyToDataTable();
                DataTable DTClarity = DtabPara.Select("PARATYPE = 'CLARITY'").CopyToDataTable();
                DataTable DTCut = DtabPara.Select("PARATYPE = 'CUT'").CopyToDataTable();
                DataTable DTPolish = DtabPara.Select("PARATYPE = 'POLISH'").CopyToDataTable();
                DataTable DTSym = DtabPara.Select("PARATYPE = 'SYMMETRY'").CopyToDataTable();
                DataTable DTFL = DtabPara.Select("PARATYPE = 'FLUORESCENCE'").CopyToDataTable();
                DataTable DTLocation = DtabPara.Select("PARATYPE = 'LOCATION'").CopyToDataTable();
                DataTable DTColorShade = DtabPara.Select("PARATYPE = 'COLORSHADE'").CopyToDataTable();
                DataTable DTMilky = DtabPara.Select("PARATYPE = 'MILKY'").CopyToDataTable();
                DataTable DTEyeClean = DtabPara.Select("PARATYPE = 'EYECLEAN'").CopyToDataTable();
                DataTable DTSize = DtabPara.Select("PARATYPE = 'SIZE'").CopyToDataTable();
                DataTable DTLab = DtabPara.Select("PARATYPE = 'LAB'").CopyToDataTable();
                DataTable DTLuster = DtabPara.Select("PARATYPE = 'LUSTER'").CopyToDataTable();
                DataTable DTHeartArrow = DtabPara.Select("PARATYPE = 'HEARTANDARROW'").CopyToDataTable();
                DataTable DTCulet = DtabPara.Select("PARATYPE = 'CULET'").CopyToDataTable();
                DataTable DTGirdle = DtabPara.Select("PARATYPE = 'GIRDLE'").CopyToDataTable();
                DataTable DTTableInc = DtabPara.Select("PARATYPE = 'TABLEINC'").CopyToDataTable();
                DataTable DTTableOpenInc = DtabPara.Select("PARATYPE = 'TABLEOPENINC'").CopyToDataTable();
                DataTable DTSideTable = DtabPara.Select("PARATYPE = 'SIDETABLEINC'").CopyToDataTable();
                DataTable DTSideOpen = DtabPara.Select("PARATYPE = 'SIDEOPENINC'").CopyToDataTable();
                DataTable DTTableBlack = DtabPara.Select("PARATYPE = 'BLACKINC'").CopyToDataTable();
                DataTable DTSideBlack = DtabPara.Select("PARATYPE = 'SIDEBLACKINC'").CopyToDataTable();
                DataTable DTRedSport = DtabPara.Select("PARATYPE = 'REDSPORTINC'").CopyToDataTable();
                DataTable DTCROpen = DtabPara.Select("PARATYPE = 'CROWN_OPEN'").CopyToDataTable();
                DataTable DTPavOpen = DtabPara.Select("PARATYPE = 'PAVILION_OPEN'").CopyToDataTable();

                DataTable DTFancy = new BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_FANCYCOLOR);

                int I = 0;
                int IntCount = DtabExcelData.Rows.Count;
                int IntCheck = 0;
                string StrCheck = "";
                string StrMessage = "";
                foreach (DataRow DrPrice in DtabExcelData.Rows)
                {
                    if ((Val.ToString(DrPrice["PARTYSTOCKNO"]).Trim().Equals(string.Empty)) && (Val.ToString(DrPrice["STOCK ID"]).Trim().Equals(string.Empty)))
                        continue;

                    I++;

                    DataRow Drfinal = DtabFinalData.NewRow();
                    Drfinal["ENTRYSRNO"] = I;
                    //Drfinal["STOCK_ID"] = BusLib.Configuration.BOConfiguration.FindNewSequentialID();

                    string StrStoneNo = "";
                    if (Val.ToString(DrPrice["PARTYSTOCKNO"]) == "")
                    {
                        StrStoneNo = Val.ToString(DrPrice["STOCK ID"]);
                    }
                    else
                    {
                        StrStoneNo = Val.ToString(DrPrice["PARTYSTOCKNO"]);
                    }

                    Drfinal["PARTYSTOCKNO"] = StrStoneNo;
                    Drfinal["STOCKTYPE"] = mStrStockType;
                    SetControlPropertyValue(lblMessage, "Text", "[" + I + "/" + IntCount.ToString() + " ] Data Validating Of Stone No [" + Drfinal["PARTYSTOCKNO"].ToString() + "]");

                    IntCheck = 0;
                    StrMessage = "";
                    Drfinal["SHAPE_ID"] = FindID(DTShape, Val.ToString(DrPrice["SHAPE"]), StrStoneNo, "Shape", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;

                    Drfinal["COLOR_ID"] = FindID(DTColor, Val.ToString(DrPrice["COLOR"]), StrStoneNo, "Color", ref IntCheck, ref StrMessage);

                    if (Val.ToInt(Drfinal["COLOR_ID"]) == -1)
                    {
                        if (Val.ToString(DrPrice["COLORDESC"]) != "" && Val.ToString(DrPrice["COLOR"]) != "")
                        {
                            DrPrice["COLORDESC"] = Val.ToString(DrPrice["COLOR"]) + "," + Val.ToString(DrPrice["COLORDESC"]);
                            Drfinal["COLORDESC"] = Val.ToString(DrPrice["COLOR"]) + "," + Val.ToString(DrPrice["COLORDESC"]);
                        }
                        else
                        {
                            Drfinal["COLOR_ID"] = FindID_FancyColor(DTFancy, Val.ToString(DrPrice["COLOR"]), StrStoneNo, "Color", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                            DrPrice["ISFANCY"] = true;
                            Drfinal["ISFANCY"] = true;

                            DrPrice["FANCYCOLOR"] = Val.ToString(DrPrice["COLOR"]);
                            Drfinal["FANCYCOLOR"] = Val.ToString(DrPrice["COLOR"]);
                            DrPrice["COLORDESC"] = Val.ToString(DrPrice["COLOR"]);
                            Drfinal["COLORDESC"] = Val.ToString(DrPrice["COLOR"]);
                        }
                    }
                    else if (Val.ToString(DrPrice["COLORDESC"]) != "" && Val.ToString(DrPrice["COLOR"]) != "")
                    {
                        DrPrice["COLORDESC"] = Val.ToString(DrPrice["COLOR"]) + "," + Val.ToString(DrPrice["COLORDESC"]);
                        Drfinal["COLORDESC"] = Val.ToString(DrPrice["COLOR"]) + "," + Val.ToString(DrPrice["COLORDESC"]);
                    }

                    Drfinal["CLARITY_ID"] = FindID(DTClarity, Val.ToString(DrPrice["CLARITY"]), StrStoneNo, "Clarity", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;

                    Drfinal["CUT_ID"] = FindID(DTCut, Val.ToString(DrPrice["CUT"]), StrStoneNo, "Cut", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;

                    Drfinal["POL_ID"] = FindID(DTPolish, Val.ToString(DrPrice["POL"]), StrStoneNo, "Polish", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["SYM_ID"] = FindID(DTSym, Val.ToString(DrPrice["SYM"]), StrStoneNo, "Symmetry", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["FL_ID"] = FindID(DTFL, Val.ToString(DrPrice["FL"]), StrStoneNo, "Fluorescence", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["LOCATION_ID"] = FindID(DTLocation, Val.ToString(DrPrice["LOCATION"]), StrStoneNo, "Location", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["COLORSHADE_ID"] = FindID(DTColorShade, Val.ToString(DrPrice["COLORSHADE"]), StrStoneNo, "Color Shade", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["MILKY_ID"] = FindID(DTMilky, Val.ToString(DrPrice["MILKY"]), StrStoneNo, "Milky", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["EYECLEAN_ID"] = FindID(DTEyeClean, Val.ToString(DrPrice["EYECLEAN"]), StrStoneNo, "Eyeclean", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["SIZE_ID"] = FindID(DTSize, Val.ToString(DrPrice["SIZE"]), StrStoneNo, "Size", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["LAB_ID"] = FindID(DTLab, Val.ToString(DrPrice["LAB"]), StrStoneNo, "Lab", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["LUSTER_ID"] = FindID(DTLuster, Val.ToString(DrPrice["LUSTER"]), StrStoneNo, "Luster", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["HA_ID"] = FindID(DTHeartArrow, Val.ToString(DrPrice["HA"]), StrStoneNo, "HA", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["CULET_ID"] = FindID(DTCulet, Val.ToString(DrPrice["CULET"]), StrStoneNo, "Culet", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;

                    Drfinal["GIRDLE_ID"] = FindID(DTGirdle, Val.ToString(DrPrice["GIRDLE"]), StrStoneNo, "Girdle", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["FROMGIRDLE_ID"] = FindID(DTGirdle, Val.ToString(DrPrice["FROMGIRDLE"]), StrStoneNo, "From Girdle", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["TOGIRDLE_ID"] = FindID(DTGirdle, Val.ToString(DrPrice["TOGIRDLE"]), StrStoneNo, "To Girdle", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["TABLEINC_ID"] = FindID(DTTableInc, Val.ToString(DrPrice["TABLEINC"]), StrStoneNo, "Table Inc", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["TABLEOPENINC_ID"] = FindID(DTTableOpenInc, Val.ToString(DrPrice["TABLEOPEN"]), StrStoneNo, "Table Open", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["SIDETABLEINC_ID"] = FindID(DTSideTable, Val.ToString(DrPrice["SIDETABLE"]), StrStoneNo, "Side Table", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["SIDEOPENINC_ID"] = FindID(DTSideOpen, Val.ToString(DrPrice["SIDEOPEN"]), StrStoneNo, "Side Open", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["SIDEBLACKINC_ID"] = FindID(DTSideBlack, Val.ToString(DrPrice["SIDEBLACK"]), StrStoneNo, "Side Black", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["REDSPORTINC_ID"] = FindID(DTRedSport, Val.ToString(DrPrice["REDSPORT"]), StrStoneNo, "Red Sport", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;

                    Drfinal["MFGCOLOR_ID"] = FindID(DTColor, Val.ToString(DrPrice["MFGCOLOR"]), StrStoneNo, "Mfg Color", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["MFGCLARITY_ID"] = FindID(DTClarity, Val.ToString(DrPrice["MFGCLARITY"]), StrStoneNo, "Mfg Clarity", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["MFGCUT_ID"] = FindID(DTCut, Val.ToString(DrPrice["MFGCUT"]), StrStoneNo, "Mfg Cut", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["MFGPOL_ID"] = FindID(DTPolish, Val.ToString(DrPrice["MFGPOL"]), StrStoneNo, "Mfg Polish", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["MFGSYM_ID"] = FindID(DTSym, Val.ToString(DrPrice["MFGSYM"]), StrStoneNo, "Mfg Symmetry", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["MFGFL_ID"] = FindID(DTFL, Val.ToString(DrPrice["MFGFL"]), StrStoneNo, "Mfg Fluorescence", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;

                    Drfinal["INSCRIPTION"] = DrPrice["INSCRIPTION"];
                    Drfinal["PCS"] = 1;
                    Drfinal["CARAT"] = Val.Val(DrPrice["CARAT"]);
                    Drfinal["BALANCEPCS"] = 1;
                    Drfinal["BALANCECARAT"] = Val.Val(DrPrice["CARAT"]);
                    Drfinal["LABREPORTNO"] = Val.ToString(DrPrice["LABREPORTNO"]);

                    Drfinal["TABLEPER"] = Val.Val(DrPrice["TABLEPER"]);
                    Drfinal["DEPTHPER"] = Val.Val(DrPrice["DEPTHPER"]);

                    Drfinal["DIAMIN"] = Val.Val(DrPrice["DIAMIN"]);
                    Drfinal["DIAMAX"] = Val.Val(DrPrice["DIAMAX"]);
                    Drfinal["LENGTH"] = Val.Val(DrPrice["LENGTH"]);
                    Drfinal["WIDTH"] = Val.Val(DrPrice["WIDTH"]);
                    Drfinal["HEIGHT"] = Val.Val(DrPrice["HEIGHT"]);
                    Drfinal["MEASUREMENT"] = Val.ToString(DrPrice["MEASUREMENT"]);
                    Drfinal["RATIO"] = Val.Val(DrPrice["RATIO"]);
                    Drfinal["DIAMETER"] = Val.Val(DrPrice["DIAMETER"]);

                    Drfinal["MFG_ID"] = Val.ToString(DrPrice["MFG_ID"]);

                    string StrMeasurement = Val.Trim(Val.ToString(DrPrice["MEASUREMENT"]));
                    if (StrMeasurement.Length != 0)
                    {
                        try
                        {
                            string[] split = StrMeasurement.Split(new Char[] { '*', '-', 'x', 'X' }, StringSplitOptions.RemoveEmptyEntries);

                            Drfinal["LENGTH"] = Val.ToString(split[0]);
                            Drfinal["WIDTH"] = Val.ToString(split[1]);
                            Drfinal["HEIGHT"] = Val.ToString(split[2]);
                        }
                        catch (Exception)
                        {
                        }
                    }
                    else
                    {
                        StrMeasurement = Val.ToString(DrPrice["LENGTH"]);
                        if (Val.ToString(DrPrice["WIDTH"]).Length != 0)
                        {
                            StrMeasurement = StrMeasurement + "*" + Val.ToString(DrPrice["WIDTH"]);
                        }
                        if (Val.ToString(DrPrice["HEIGHT"]).Length != 0)
                        {
                            StrMeasurement = StrMeasurement + "*" + Val.ToString(DrPrice["HEIGHT"]);
                        }
                        Drfinal["MEASUREMENT"] = StrMeasurement;
                    }

                    Drfinal["CRANGLE"] = Val.Val(DrPrice["CRANGLE"]);
                    Drfinal["CRHEIGHT"] = Val.ToString(DrPrice["CRHEIGHT"]).Contains("%") ? Val.Val(Val.ToString(DrPrice["CRHEIGHT"]).Replace("%", "")) : Val.Val(DrPrice["CRHEIGHT"]);
                    Drfinal["PAVANGLE"] = Val.Val(DrPrice["PAVANGLE"]);
                    Drfinal["PAVHEIGHT"] = Val.ToString(DrPrice["PAVHEIGHT"]).Contains("%") ? Val.Val(Val.ToString(DrPrice["PAVHEIGHT"]).Replace("%", "")) : Val.Val(DrPrice["PAVHEIGHT"]);
                    Drfinal["GIRDLEPER"] = Val.ToString(DrPrice["GIRDLEPER"]).Contains("%") ? Val.Val(Val.ToString(DrPrice["GIRDLEPER"]).Replace("%", "")) : Val.Val(DrPrice["GIRDLEPER"]);
                    Drfinal["GIRDLEDESC"] = Val.ToString(DrPrice["GIRDLEDESC"]);
                    Drfinal["CURRENCY_ID"] = mIntCurrencyID;
                    //Drfinal["EXCRATE"] = mDouExcRate;

                    //Add By Dhara For Save MFG Price : 27-12-2023
                    
                    double DouMFGPricePerCarat = 0, DouMFGAmount = 0;
                    DouMFGPricePerCarat = (Val.Val(DrPrice["MFGRAPAPORT"]) - (Val.Val(DrPrice["MFGRAPAPORT"]) * Val.Val(DrPrice["MFGDISCOUNT"]) / 100)); //#P:23-04-2021
                    DouMFGAmount = DouMFGPricePerCarat * Val.Val(DrPrice["CARAT"]);
                    Drfinal["MFGPRICEPERCARAT"] = DouMFGPricePerCarat;
                    Drfinal["MFGAMOUNT"] = DouMFGAmount;
                    Drfinal["MFGRAPAPORT"] = Val.Val(DrPrice["MFGRAPAPORT"]);
                    Drfinal["MFGDISCOUNT"] = Val.Val(DrPrice["MFGDISCOUNT"]);

                    // END : 27-12-2023


                    if (Val.Val(DrPrice["COSTRAPAPORT"]) != 0) // Coz Sometime Single file Contains Only Rap and Disc
                    {
                        double DouCostPricePerCarat = 0, DouCostAmount = 0;
                        DouCostPricePerCarat = (Val.Val(DrPrice["COSTRAPAPORT"]) + (Val.Val(DrPrice["COSTRAPAPORT"]) * Val.Val(DrPrice["COSTDISCOUNT"]) / 100)); //#P:23-04-2021 
                        DouCostAmount = DouCostPricePerCarat * Val.Val(DrPrice["CARAT"]);

                        Drfinal["COSTRAPAPORT"] = Val.Val(DrPrice["COSTRAPAPORT"]);
                        Drfinal["COSTDISCOUNT"] = Val.Val(DrPrice["COSTDISCOUNT"]);
                        Drfinal["COSTPRICEPERCARAT"] = DouCostPricePerCarat;
                        Drfinal["COSTAMOUNT"] = Val.Val(DrPrice["COSTAMOUNT"]) == 0 ? DouCostAmount : Val.Val(DrPrice["COSTAMOUNT"]);

                        Drfinal["SALERAPAPORT"] = Val.Val(DrPrice["SALERAPAPORT"]) == 0 ? Val.Val(Drfinal["COSTRAPAPORT"]) : Val.Val(DrPrice["SALERAPAPORT"]);
                        Drfinal["SALEDISCOUNT"] = Val.Val(DrPrice["SALEDISCOUNT"]) == 0 ? Val.Val(Drfinal["COSTDISCOUNT"]) : Val.Val(DrPrice["SALEDISCOUNT"]);

                        double DouSalePricePerCarat = 0, DouSaleAmount = 0;
                        DouSalePricePerCarat = (Val.Val(Drfinal["SALERAPAPORT"]) + (Val.Val(Drfinal["SALERAPAPORT"]) * Val.Val(Drfinal["SALEDISCOUNT"]) / 100)); //#P:23-04-2021
                        DouSaleAmount = DouSalePricePerCarat * Val.Val(DrPrice["CARAT"]);
                        Drfinal["SALEPRICEPERCARAT"] = DouSalePricePerCarat;
                        Drfinal["SALEAMOUNT"] = DouSaleAmount; // Val.Val(DrPrice["SALEAMOUNT"]) == 0 ? Val.Val(Drfinal["COSTAMOUNT"]) : Val.Val(DrPrice["SALEAMOUNT"]);
                        Drfinal["SALEPRICEPERCARAT"] = Val.Val(DrPrice["SALEPRICEPERCARAT"]) == 0 ? Val.Val(Drfinal["COSTPRICEPERCARAT"]) : Val.Val(DrPrice["SALEPRICEPERCARAT"]);//ADD BY RAJVI : 18/06/2025
                        Drfinal["SALEAMOUNT"] = Val.Val(DrPrice["SALEAMOUNT"]) == 0 ? Val.Val(Drfinal["COSTAMOUNT"]) : Val.Val(DrPrice["SALEAMOUNT"]);//ADD BY RAJVI : 18/06/2025

                    }
                    else // For Parcel File :  It's Only Contain PerCarat and Amount 
                    {

                        Drfinal["COSTRAPAPORT"] = Val.Val(DrPrice["COSTRAPAPORT"]);
                        Drfinal["COSTDISCOUNT"] = Val.Val(DrPrice["COSTDISCOUNT"]);
                        Drfinal["COSTPRICEPERCARAT"] = Val.Val(DrPrice["COSTPRICEPERCARAT"]);
                        Drfinal["COSTAMOUNT"] = Val.Val(DrPrice["COSTAMOUNT"]);

                        Drfinal["SALERAPAPORT"] = Val.Val(DrPrice["SALERAPAPORT"]) == 0 ? Val.Val(Drfinal["COSTRAPAPORT"]) : Val.Val(DrPrice["SALERAPAPORT"]);
                        Drfinal["SALEDISCOUNT"] = Val.Val(DrPrice["SALEDISCOUNT"]) == 0 ? Val.Val(Drfinal["COSTDISCOUNT"]) : Val.Val(DrPrice["SALEDISCOUNT"]);
                        //Drfinal["SALEPRICEPERCARAT"] = Val.Val(DrPrice["SALEPRICEPERCARAT"]) == 0 ? Val.Val(Drfinal["COSTPRICEPERCARAT"]) : Val.Val(DrPrice["SALEPRICEPERCARAT"]);//COMMENT BY RAJVI : 18/06/2025
                        //Drfinal["SALEAMOUNT"] = Val.Val(DrPrice["SALEAMOUNT"]) == 0 ? Val.Val(Drfinal["COSTAMOUNT"]) : Val.Val(DrPrice["SALEAMOUNT"]);//COMMENT BY RAJVI : 18/06/2025

                    }

                    double DouFCostPricePerCarat = 0, DouFCostAmount = 0, DouFSalePricePerCarat = 0, DouFSaleAmount = 0;

                    DouFCostPricePerCarat = Val.Val(DrPrice["FCOSTPRICEPERCARAT"]) == 0 ? Val.Val(Drfinal["COSTPRICEPERCARAT"]) * Val.Val(txtExcRate.Text) : Val.Val(DrPrice["FCOSTPRICEPERCARAT"]);
                    DouFCostAmount = Val.Val(DrPrice["FCOSTAMOUNT"]) == 0 ? Val.Val(Drfinal["COSTAMOUNT"]) * Val.Val(txtExcRate.Text) : Val.Val(DrPrice["FCOSTAMOUNT"]);

                    DouFSalePricePerCarat = Val.Val(DrPrice["FSALEPRICEPERCARAT"]) == 0 ? (Val.Val(Drfinal["SALEPRICEPERCARAT"]) * Val.Val(txtExcRate.Text)) : Val.Val(DrPrice["FSALEPRICEPERCARAT"]);
                    DouFSaleAmount = Val.Val(DrPrice["FSALEAMOUNT"]) == 0 ? (Val.Val(Drfinal["SALEAMOUNT"]) * Val.Val(txtExcRate.Text)) : Val.Val(DrPrice["FSALEAMOUNT"]);

                    Drfinal["FCOSTPRICEPERCARAT"] = Val.Val(DouFCostPricePerCarat);
                    Drfinal["FCOSTAMOUNT"] = Val.Val(DouFCostAmount);
                    Drfinal["FSALEPRICEPERCARAT"] = Val.Val(DouFSalePricePerCarat);
                    Drfinal["FSALEAMOUNT"] = Val.Val(DouFSaleAmount);

                    Drfinal["ISNOBGM"] = Val.ToInt32(DrPrice["ISNOBGM"]);
                    Drfinal["ISNOBLACK"] = Val.ToInt32(DrPrice["ISNOBLACK"]);
                    Drfinal["FLUORESCENCECOLOR"] = Val.ToString(DrPrice["FLUORESCENCECOLOR"]);
                    Drfinal["COLORDESC"] = Val.ToString(DrPrice["COLORDESC"]);
                    Drfinal["GIRDLECONDITION"] = Val.ToString(DrPrice["GIRDLECONDITION"]);
                    Drfinal["STARLENGTH"] = Val.ToString(DrPrice["STARLENGTH"]);
                    Drfinal["LOWERHALF"] = Val.ToString(DrPrice["LOWERHALF"]);
                    Drfinal["PAINTING"] = Val.ToString(DrPrice["PAINTING"]);
                    Drfinal["PROPORTIONS"] = Val.ToString(DrPrice["PROPORTIONS"]);
                    Drfinal["PAINTCOMM"] = Val.ToString(DrPrice["PAINTCOMM"]);

                    Drfinal["KEYTOSYMBOL"] = Val.ToString(DrPrice["KEYTOSYMBOL"]);
                    Drfinal["REPORTCOMMENT"] = Val.ToString(DrPrice["REPORTCOMMENT"]);
                    Drfinal["INSCRIPTION"] = Val.ToString(DrPrice["INSCRIPTION"]);

                    Drfinal["SYNTHETICINDICATOR"] = Val.ToString(DrPrice["SYNTHETICINDICATOR"]);
                    Drfinal["POLISHFEATURES"] = Val.ToString(DrPrice["POLISHFEATURES"]);
                    Drfinal["SYMMETRYFEATURES"] = Val.ToString(DrPrice["SYMMETRYFEATURES"]);
                    Drfinal["JOBNO"] = Val.ToString(DrPrice["JOBNO"]);
                    Drfinal["ISFANCY"] = Val.ToBoolean(DrPrice["ISFANCY"]);
                    Drfinal["FANCYCOLOR"] = Val.ToString(DrPrice["FANCYCOLOR"]);
                    Drfinal["FANCYCOLORINTENSITY"] = Val.ToString(DrPrice["FANCYCOLORINTENSITY"]);
                    Drfinal["FANCYCOLOROVERTONE"] = Val.ToString(DrPrice["FANCYCOLOROVERTONE"]);
                    Drfinal["EXPORTORDERNO"] = Val.ToString(DrPrice["EXPORTORDERNO"]);

                    Drfinal["EXPRAPAPORT"] = Val.Val(DrPrice["EXPRAPAPORT"]);
                    Drfinal["EXPDISCOUNT"] = Val.Val(DrPrice["EXPDISCOUNT"]);
                    Drfinal["EXPPRICEPERCARAT"] = Val.Val(DrPrice["EXPPRICEPERCARAT"]);
                    Drfinal["EXPAMOUNT"] = Val.Val(DrPrice["EXPAMOUNT"]);
                    Drfinal["RAPNETRAPAPORT"] = Val.Val(DrPrice["RAPNETRAPAPORT"]);
                    Drfinal["RAPNETDISCOUNT"] = Val.Val(DrPrice["RAPNETDISCOUNT"]);
                    Drfinal["RAPNETPRICEPERCARAT"] = Val.Val(DrPrice["RAPNETPRICEPERCARAT"]);
                    Drfinal["RAPNETAMOUNT"] = Val.Val(DrPrice["RAPNET AMOUNT"]);
                    // Drfinal["RFIDTAGNO"] =Val.ToString(DrPrice["RFIDTAGNO"]); //urvisha add by 20-02-2023

                    var drParty = (from DrSupplier in DtabSupplier.AsEnumerable()
                                   where Val.ToString(DrSupplier["PARTYNAME"]).ToUpper() == Val.ToString(DrPrice["PARTY"]).Trim().ToUpper()
                                   select DrSupplier);

                    Drfinal["PARTY_ID"] = drParty.Count() > 0 ? Guid.Parse(Val.ToString(drParty.FirstOrDefault()["PARTY_ID"])) : Guid.Parse(mStrParty);
                    //Drfinal["PARTY_ID"] =  Guid.Parse(Val.ToString(txtPartyName.Tag));
                    Drfinal["UPLOADDATE"] = DrPrice["UPLOADDATE"];

                    Drfinal["REMARK"] = DrPrice["REMARK"];

                    Drfinal["VIDEOURL"] = DrPrice["VIDEOURL"];
                    Drfinal["IMAGEURL"] = DrPrice["IMAGEURL"];

                    //Added By Gunjan:27/12/2023
                    Drfinal["SUBCATAGORY"] = DrPrice["SUBCATAGORY"];
                    Drfinal["MAINCATAGORY"] = DrPrice["MAINCATAGORY"];

                    //Drfinal["NIVODADISCOUNT"] = DrPrice["NIVODADISCOUNT"];
                    //Drfinal["NIVODAPRICEPERCARAT"] = DrPrice["NIVODAPRICEPERCARAT"];
                    //Drfinal["NIVODARAPAPORT"] = DrPrice["NIVODARAPAPORT"];
                    //Drfinal["NIVODAAMOUNT"] = DrPrice["NIVODAAMOUNT"];

                    //Drfinal["VDBDISCOUNT"] = DrPrice["VDBDISCOUNT"];
                    //Drfinal["VDBPRICEPERCARAT"] = DrPrice["VDBPRICEPERCARAT"];
                    //Drfinal["VDBRAPAPORT"] = DrPrice["VDBRAPAPORT"];
                    //Drfinal["VDBAMOUNT"] = DrPrice["VDBAMOUNT"];
                    //Drfinal["REPORTDATE"] = DrPrice["REPORTDATE"];
                    Drfinal["TABLEBLACKINC_ID"] = FindID(DTTableBlack, Val.ToString(DrPrice["TABLEBLACK"]), StrStoneNo, "Black Inc", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["PAVILIONOPEN_ID"] = FindID(DTPavOpen, Val.ToString(DrPrice["PAVOPEN"]), StrStoneNo, "Pav Open", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;
                    Drfinal["CROWNOPEN_ID"] = FindID(DTCROpen, Val.ToString(DrPrice["CROWNOPN"]), StrStoneNo, "Crown Open", ref IntCheck, ref StrMessage); if (IntCheck == -1) break;

                    //End As Gunjan

                    //Drfinal["CERTIFICATEURL"] = DrPrice["REMARK"];
                    try
                    {
                        string[] StrPacketNo = Val.ToString(Drfinal["PARTYSTOCKNO"]).Split('-');
                        Drfinal["KAPANNAME"] = Val.Trim(StrPacketNo[0]);
                        Drfinal["PACKETNO"] = Val.Trim(new string(StrPacketNo[1].Where(c => c - '0' < 10).ToArray()));
                        Drfinal["TAG"] = Val.Trim(StrPacketNo[1].Replace(Val.ToString(Drfinal["PACKETNO"]), ""));

                        Drfinal["LOTNAME"] = Val.Trim(Val.ToString(DrPrice["LOTNAME"]));

                        //StrCheck = FindIDWithGUIDType(DtabLot, Val.ToString(DrPrice["LOTNAME"]), StrStoneNo, "LotName", ref StrCheck, ref StrMessage);
                        //if (StrCheck == "FAIL") break;
                        //Drfinal["LOT_ID"] = Guid.Parse(StrCheck); 

                    }
                    catch
                    {
                    }

                    DtabFinalData.Rows.Add(Drfinal);
                }

                if (IntCheck != -1 && StrCheck != "FAIL")
                {
                    string StrStockUploadType = "";
                    Guid gStrParyt_ID;

                    if (RbtAppendStock.Checked) StrStockUploadType = "APPEND";
                    else if (RbtReplaceAllStock.Checked) StrStockUploadType = "REPLACE";

                    gStrParyt_ID = Val.ToString(mStrParty).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(mStrParty));

                    DtabFinalData.TableName = "DETAIL";

                    string StockUploadXML;
                    using (StringWriter sw = new StringWriter())
                    {
                        DtabFinalData.WriteXml(sw);
                        StockUploadXML = sw.ToString();
                    }

                    SetControlPropertyValue(lblMessage, "Text", "Data Uploding On Server Start");
                    DtabStockSync = ObjStock.SaveStockUploadUsingDataTable(StockUploadXML, mStrStockStatus, Val.ToString(StrStockUploadType), gStrParyt_ID, mIntPrdType_ID, StrBillFormat, StrBillType);
                    SetControlPropertyValue(lblMessage, "Text", "Data Uploding On Server Done Successfully");

                    if (DtabStockUpload.Rows.Count > 0)
                    {
                        SetControlPropertyValue(lblMessage, "Text", "Stock Successfully Uploaded");
                    }
                }
                else
                {
                    Global.MessageError(StrMessage);
                }

                DTShape.Dispose();
                DTColor.Dispose();
                DTClarity.Dispose();
                DTCut.Dispose();
                DTPolish.Dispose();
                DTSym.Dispose();
                DTFL.Dispose();
                DTLocation.Dispose();
                DTColorShade.Dispose();
                DTMilky.Dispose();
                DTEyeClean.Dispose();
                DTSize.Dispose();
                DTLab.Dispose();
                DTLuster.Dispose();
                DTHeartArrow.Dispose();

                DTCulet.Dispose();
                DTGirdle.Dispose();
                DTTableInc.Dispose();
                DTTableOpenInc.Dispose();
                DTSideTable.Dispose();
                DTSideOpen.Dispose();
                DTTableBlack.Dispose();
                DTSideBlack.Dispose();
                DTRedSport.Dispose();
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
                SetControlPropertyValue(BtnCalculate, "Enabled", true);
                SetControlPropertyValue(lblMessage, "Text", ex.Message);
            }
        }

        private void cmbBillType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (new[] { "EXPORT", "CONSIGNMENT", "NONE" }.Contains(cmbBillType.Text.ToUpper()))
            {
                StrBillFormat = "DOLLAR";
            }
            else
            {
                StrBillFormat = "RUPEES";
            }
            StrBillType = Val.ToString(cmbBillType.SelectedItem);
        }

        //ADD BY RAJVI : 19/05/2025
        private void lblSaveLayout_Click(object sender, EventArgs e)
        {
            Stream str = new System.IO.MemoryStream();
            GrdDetStock.SaveLayoutToStream(str);
            str.Seek(0, System.IO.SeekOrigin.Begin);
            StreamReader reader = new StreamReader(str);
            string text = reader.ReadToEnd();

            int IntRes = new BOTRN_StockUpload().SaveGridLayout(this.Name, GrdDetStock.Name, text);
            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Saved");
            }
        }

        private void lblDefaultLayout_Click(object sender, EventArgs e)
        {
            int IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdDetStock.Name);
            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Deleted");
            }

        }
        //END BY RAJVI
        
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            GrdDetStock.BeginUpdate();

            MainGrdStock.DataSource = DtabStockSync;
            MainGrdStock.RefreshDataSource();
            GrdDetStock.BestFitColumns();
            GrdDetStock.EndUpdate();
            CalculateTotalSummary();
            BtnCalculate.Enabled = true;

            progressPanel1.Visible = false;

            //if (BusLib.Configuration.BOConfiguration.gEmployeeProperty.ISDISPLAYCOSTPRICE == false)
            //{
            //    GrdDetStock.Columns["COSTRAPAPORT"].Visible = false;
            //    GrdDetStock.Columns["COSTDISCOUNT"].Visible = false;
            //    GrdDetStock.Columns["COSTPRICEPERCARAT"].Visible = false;
            //    GrdDetStock.Columns["COSTAMOUNT"].Visible = false;
            //}

            watch.Stop();
            lblTime.Text = string.Format("{0:hh\\:mm\\:ss}", watch.Elapsed);
            SetControlPropertyValue(lblMessage, "Text", "Successfully Uploaded");
        }

        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[]
                        {
                            oControl,
                            propName,
                            propValue
                        });
            }
            else
            {
                Type t = oControl.GetType();
                PropertyInfo[] props = t.GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if ((p.Name.ToUpper() == propName.ToUpper()))
                    {
                        p.SetValue(oControl, propValue, null);
                    }
                }
            }
        }
    }
}
