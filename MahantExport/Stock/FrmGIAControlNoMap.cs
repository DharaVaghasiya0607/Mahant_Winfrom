using MahantExport.Utility;
using BusLib.Rapaport;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MahantExport.Stock
{
    public partial class FrmGIAControlNoMap : DevExpress.XtraEditors.XtraForm
    {
        BODevGridSelection ObjGridSelection;
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFindRap ObjRap = new BOFindRap();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();

        DataTable DtabStockDetail = new DataTable();
        public FrmGIAControlNoMap()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
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
                if (ObjGridSelection != null)
                    ObjGridSelection.ClearSelection();
            }
            if (ObjGridSelection != null)
            {
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            }
            this.Show();

        }

        public void ShowForm(string StrStoneNo)
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
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
                if (ObjGridSelection != null)
                    ObjGridSelection.ClearSelection();
            }
            if (ObjGridSelection != null)
            {
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            }

            txtStoneCertiMFGMemo.Text = StrStoneNo;
            BtnSearch_Click(null, null);
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

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                string StoneNo = txtStoneCertiMFGMemo.Text.Trim();

                // Assuming values are separated by commas or spaces, adjust the split accordingly
                string[] stoneNos = StoneNo.Split(new[] { ',', ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                // Use a HashSet to track duplicates
                var seenStoneNos = new HashSet<string>();
                bool hasDuplicate = false;
                string duplicateValue = string.Empty;

                foreach (string stone in stoneNos)
                {
                    if (!seenStoneNos.Add(stone))
                    {
                        hasDuplicate = true;
                        duplicateValue = stone;
                        break; // Stop at the first duplicate found
                    }
                }
                if (hasDuplicate)
                {
                    Global.Message(duplicateValue + "--- Duplicate Stone Id in Stone List , Please Check !!");
                    return;
                }
                DtabStockDetail = new BOTRN_StockUpload().GetDataForGIAControlNoMap(StoneNo, "SUMMARY");

                if (DtabStockDetail.Rows.Count <= 0)
                {
                    Global.Message("No Data Found");
                    return;
                }
                MainGrdDetail.DataSource = DtabStockDetail;
                MainGrdDetail.Refresh();
                GrdDetail.BestFitColumns();

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.ToString());
            }
        }

        private void txtStoneControlNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtStoneControlNo_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {

                    for (int i = 0; i < DtabStockDetail.Rows.Count; i++)
                    {
                        DtabStockDetail.Rows[i]["CONTROLNO"] = Val.ToInt64(txtStoneControlNo.Text) + i;
                    }
                    DtabStockDetail.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.ToString());
            }
        }

        private void txtStoneCertiMFGMemo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String str1 = "";
                if (txtStoneCertiMFGMemo.Text.Trim().Contains("\t\n"))
                {
                    str1 = txtStoneCertiMFGMemo.Text.Trim().Replace("\t\n", ",");
                }
                else
                {
                    str1 = txtStoneCertiMFGMemo.Text.Trim().Replace("\n", ",");
                    str1 = str1.Replace("\r", "");
                }

                txtStoneCertiMFGMemo.Text = str1;
                txtStoneCertiMFGMemo.Select(txtStoneCertiMFGMemo.Text.Length, 0);

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                //DataTable DTab = DtabStockDetail;
                DataTable DTab = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);
                if (DTab.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("No Data Found For Excel Export");
                    return;
                    this.Cursor = Cursors.Default;
                }

                // Check if LABNAME values in DTabStcokGrid are not the same
                if (DTab.Rows.Count > 0)
                {
                    // Get the first LABNAME value to compare with others
                    string firstLabName = "GIA";

                    foreach (DataRow row in DTab.Rows)
                    {
                        // Compare each LABNAME value with the first one
                        if (!row["LABNAME"].ToString().Trim().Equals(firstLabName, StringComparison.OrdinalIgnoreCase))
                        {
                            // Show error message if LABNAME values are different
                            Global.Message("LAB Are Not Same In The Selected Stone List , Please Check");
                            return;
                        }
                    }
                }

                DataTable DtabCSV = new DataTable();

                DtabCSV.Columns.Add("AClientRefNo");
                DtabCSV.Columns.Add("ASubClientId");
                DtabCSV.Columns.Add("AControlNumber");
                DtabCSV.Columns.Add("AServices");
                DtabCSV.Columns.Add("AShape");
                DtabCSV.Columns.Add("AStatedWeight");
                DtabCSV.Columns.Add("AStatedValue");
                DtabCSV.Columns.Add("AReportNo");
                DtabCSV.Columns.Add("AInscServices");
                DtabCSV.Columns.Add("AInscText");
                DtabCSV.Columns.Add("ASortingColor");
                DtabCSV.Columns.Add("ASortingClarity");
                DtabCSV.Columns.Add("ASortingPolish");
                DtabCSV.Columns.Add("ASortingSymmetry");
                DtabCSV.Columns.Add("ACut");
                DtabCSV.Columns.Add("AClientStatedFluorescenceIntensity");
                DtabCSV.Columns.Add("AClientStatedFluorescenceColor");
                DtabCSV.Columns.Add("ACEGFlag");
                DtabCSV.Columns.Add("AStatedLength");
                DtabCSV.Columns.Add("AStatedWidth");
                DtabCSV.Columns.Add("AStatedDepth");
                DtabCSV.Columns.Add("ARoughNumber");
                DtabCSV.Columns.Add("AStatedCount");
                DtabCSV.Columns.Add("AColorOption");
                DtabCSV.Columns.Add("ANoColor");
                DtabCSV.Columns.Add("AStatedDiameterMin");
                DtabCSV.Columns.Add("AStatedDiameterMax");
                DtabCSV.Columns.Add("ARequestedDia");
                DtabCSV.Columns.Add("ARequestedDiameterMin");
                DtabCSV.Columns.Add("ARequestedDiameterMax");
                DtabCSV.Columns.Add("AMineCompany");
                DtabCSV.Columns.Add("ACountryOfOrigin");
                DtabCSV.Columns.Add("AMineName");
                DtabCSV.Columns.Add("AExchangeName");
                DtabCSV.Columns.Add("ADigitalReport");
                DtabCSV.Columns.Add("ARoughReportNo");
                DtabCSV.Columns.Add("AFancyColor");
                DtabCSV.Columns.Add("AHPHTYN");


                for (int i = 0; i < DTab.Rows.Count; i++)
                {
                    DtabCSV.Rows.Add();

                    DtabCSV.Rows[i]["AClientRefNo"] = DTab.Rows[i]["PARTYSTOCKNO"];
                    DtabCSV.Rows[i]["ASubClientId"] = DTab.Rows[i]["CLIENTID"];
                    DtabCSV.Rows[i]["AControlNumber"] = DTab.Rows[i]["CONTROLNO"];
                    DtabCSV.Rows[i]["AServices"] = DTab.Rows[i]["AServices"];
                    DtabCSV.Rows[i]["AShape"] = DTab.Rows[i]["SHAPENAME"];
                    DtabCSV.Rows[i]["AStatedWeight"] = DTab.Rows[i]["CARAT"];
                    DtabCSV.Rows[i]["AStatedValue"] = DTab.Rows[i]["COSTAMOUNT"];
                    DtabCSV.Rows[i]["AReportNo"] = DTab.Rows[i]["LABREPORTNO"];
                    DtabCSV.Rows[i]["AInscServices"] = "";
                    DtabCSV.Rows[i]["AInscText"] = "";
                    DtabCSV.Rows[i]["ASortingColor"] = DTab.Rows[i]["COLORNAMENEW"];
                    DtabCSV.Rows[i]["ASortingClarity"] = DTab.Rows[i]["CLARITYNAME"];
                    DtabCSV.Rows[i]["ASortingPolish"] = DTab.Rows[i]["POLNAME"];
                    DtabCSV.Rows[i]["ASortingSymmetry"] = DTab.Rows[i]["SYMNAME"];
                    DtabCSV.Rows[i]["ACut"] = DTab.Rows[i]["CUTNAME"];
                    DtabCSV.Rows[i]["AClientStatedFluorescenceIntensity"] = "";
                    DtabCSV.Rows[i]["AClientStatedFluorescenceColor"] = DTab.Rows[i]["FLNAME"];
                    DtabCSV.Rows[i]["ACEGFlag"] = "";
                    DtabCSV.Rows[i]["AStatedLength"] = DTab.Rows[i]["LENGTH"];
                    DtabCSV.Rows[i]["AStatedWidth"] = DTab.Rows[i]["WIDTH"];
                    DtabCSV.Rows[i]["AStatedDepth"] = DTab.Rows[i]["DEPTHPER"];
                    DtabCSV.Rows[i]["ARoughNumber"] = "";
                    DtabCSV.Rows[i]["AStatedCount"] = "";
                    DtabCSV.Rows[i]["AColorOption"] = "";
                    DtabCSV.Rows[i]["ANoColor"] = "";
                    DtabCSV.Rows[i]["AStatedDiameterMin"] = "";
                    DtabCSV.Rows[i]["AStatedDiameterMax"] = "";
                    DtabCSV.Rows[i]["ARequestedDia"] = "";
                    DtabCSV.Rows[i]["ARequestedDiameterMin"] = "";
                    DtabCSV.Rows[i]["ARequestedDiameterMax"] = "";
                    DtabCSV.Rows[i]["AMineCompany"] = "";
                    DtabCSV.Rows[i]["ACountryOfOrigin"] = "";
                    DtabCSV.Rows[i]["AMineName"] = "";
                    DtabCSV.Rows[i]["AExchangeName"] = "";
                    DtabCSV.Rows[i]["ADigitalReport"] = "";
                    DtabCSV.Rows[i]["ARoughReportNo"] = "";
                    DtabCSV.Rows[i]["AFancyColor"] = DTab.Rows[i]["FANCYCOLOR"];
                    DtabCSV.Rows[i]["AHPHTYN"] = "";

                }

                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = ".CSV";
                svDialog.Title = "Export to Excel";
                svDialog.FileName = "GIAControlNoMap" + ".CSV";

                svDialog.Filter = "Excel File (*.CSV)|*.CSV ";
                string StrFilePath = "";
                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    StrFilePath = svDialog.FileName;
                }

                StrFilePath = Global.ExportToCSV(DtabCSV, StrFilePath);

                if (Global.Confirm("Do You Want To Open The File ? ") == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(StrFilePath, "CMD");
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnExcelExport_Click(object sender, EventArgs e)
        {
            try
            {
                string extension = "";
                string destinationPath = "";

                MemoEntryProperty Property = new MemoEntryProperty();

                DataTable DTab = DtabStockDetail;

                if (DTab.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("No Data Found For Excel Export");
                    return;
                    this.Cursor = Cursors.Default;
                }

                // Check if LABNAME values in DTabStcokGrid are not the same
                if (DTab.Rows.Count > 0)
                {
                    // Get the first LABNAME value to compare with others
                    string firstLabName = "GIA";

                    foreach (DataRow row in DTab.Rows)
                    {
                        // Compare each LABNAME value with the first one
                        if (!row["LABNAME"].ToString().Trim().Equals(firstLabName, StringComparison.OrdinalIgnoreCase))
                        {
                            // Show error message if LABNAME values are different
                            Global.Message("LAB Are Not Same In The Selected Stone List , Please Check");
                            return;
                        }
                    }
                }
                double StrCostAmtFE = 0;

                DataTable DtabNew = new DataTable();

                DtabNew.Columns.Add("SRNO");
                DtabNew.Columns.Add("ClientRefNo");
                DtabNew.Columns.Add("Shape");
                DtabNew.Columns.Add("InscriptionService");
                DtabNew.Columns.Add("Carat");
                DtabNew.Columns.Add("Diameter");
                DtabNew.Columns.Add("HEIGHT");
                DtabNew.Columns.Add("Color");
                DtabNew.Columns.Add("Clarity");
                DtabNew.Columns.Add("[Value$]");
                DtabNew.Columns.Add("amount");
                DtabNew.Columns.Add("GIAControlNo");

                for (int i = 0; i < DTab.Rows.Count; i++)
                {
                    string StrMeasurment = Val.ToString(DTab.Rows[i]["MEASUREMENT"]);


                    DtabNew.Rows.Add();

                    DtabNew.Rows[i]["SRNO"] = DTab.Rows[i]["SEQUENCE"];
                    DtabNew.Rows[i]["ClientRefNo"] = DTab.Rows[i]["STOCKNO"];
                    DtabNew.Rows[i]["Shape"] = DTab.Rows[i]["SHAPENAME"];
                    DtabNew.Rows[i]["InscriptionService"] = DTab.Rows[i]["ASERVICES"];
                    DtabNew.Rows[i]["Carat"] = DTab.Rows[i]["CARAT"];
                    DtabNew.Rows[i]["Diameter"] = StrMeasurment.Split('X')[0];
                    DtabNew.Rows[i]["HEIGHT"] = DTab.Rows[i]["Height"];
                    DtabNew.Rows[i]["Color"] = DTab.Rows[i]["COLORNAME"];
                    DtabNew.Rows[i]["Clarity"] = DTab.Rows[i]["CLARITYNAME"];
                    DtabNew.Rows[i]["[Value$]"] = DTab.Rows[i]["COSTPRICEPERCARAT"];
                    DtabNew.Rows[i]["amount"] = DTab.Rows[i]["COSTAMOUNT"];
                    DtabNew.Rows[i]["GIAControlNo"] = DTab.Rows[i]["CONTROLNO"];

                    StrCostAmtFE += Val.ToDouble(DTab.Rows[i]["COSTAMOUNT"]) * Val.Val(txtExcRate.Text);
                }

                //Guid StrBillingParty = Val.ToGuid(DTab.Rows[0]["BILLINGPARTY_ID"]);

                string StrBillingParty = Val.ToString(DTab.Rows[0]["BILLINGPARTY_ID"]);

                if (StrBillingParty == "")
                {
                    Global.Message("Billing Party is Required");
                    return;
                }

                DataRow DRow = new BOTRN_StockUpload().GetGIAPartyDetail(StrBillingParty);

                if (DRow != null)
                {
                    string StrPartyName = DRow["LedgerName"].ToString();
                    string StrAddress1 = DRow["BillingAddress1"].ToString();
                    string StrAddress2 = DRow["BillingAddress2"].ToString();
                    string StrGSTNo = DRow["GSTNo"].ToString();
                    string StrClientID = DRow["Remark"].ToString();
                    string StrGIAGSTNo = "";
                    if (StrPartyName.Contains("MUMBAI"))
                    {
                        StrBillingParty = Val.ToString("C980A0C2-03DE-EE11-AFFE-78E3B5C3C2B6");//RIJIYA MUMBAI
                        DataRow DRow1 = new BOTRN_StockUpload().GetGIAPartyDetail(StrBillingParty);
                        StrGIAGSTNo = DRow1["GSTNo"].ToString();
                    }
                    else if (StrPartyName.Contains("PROSPEROUS"))
                    {
                        StrBillingParty = Val.ToString("E1BDC2AE-03DE-EE11-AFFE-78E3B5C3C2B6");//PROSPEROUS
                        DataRow DRow2 = new BOTRN_StockUpload().GetGIAPartyDetail(StrBillingParty);
                        StrGIAGSTNo = DRow2["GSTNo"].ToString();
                    }
                    else if (StrPartyName.Contains("SURAT"))
                    {
                        StrBillingParty = Val.ToString("A04A519A-03DE-EE11-AFFE-78E3B5C3C2B6");//RIJIYA 
                        DataRow DRow3 = new BOTRN_StockUpload().GetGIAPartyDetail(StrBillingParty);
                        StrGIAGSTNo = DRow3["GSTNo"].ToString();
                    }

                    string StrMemoNo = Val.ToString(txtMemoNo.Text);
                    if (StrMemoNo == "")
                    {
                        Global.Message("Please Enter Memo No...");
                    }

                    DataSet DS = new DataSet();
                    DS.Tables.Add(DtabNew);
                    string DDATE = DateTime.Now.ToString("dd-MM-yyyy");

                    string amountInWords = Global.ConvertAmountToWords(StrCostAmtFE);

                    ExportToExcel_GIA_SURAT(DS, AppDomain.CurrentDomain.BaseDirectory + "EXCEL\\GIA Format.xlsx", " ", DDATE, " ", amountInWords, 
                        StrPartyName, StrAddress1, StrAddress2, StrGSTNo, StrClientID, StrGIAGSTNo, StrMemoNo, StrCostAmtFE);

                    extension = Path.GetExtension(Path.GetFileName(AppDomain.CurrentDomain.BaseDirectory + "EXCEL\\GIA Format.xlsx"));
                    destinationPath = AppDomain.CurrentDomain.BaseDirectory + "EXCEL\\LAbReturnStonr.xlsx";
                    destinationPath = destinationPath.Replace(extension, ".pdf");
                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }
                    File.Copy(AppDomain.CurrentDomain.BaseDirectory + "EXCEL\\GIA Format.xlsx", destinationPath);

                    Global.Message("Excel File And PDF File Generated Sucessfully !");
                    if (Global.Confirm("Do You Want To Open File ? ") == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "EXCEL\\GIA Format.xlsx");
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        public static void ExportToExcel_GIA_SURAT(DataSet dataSet, string outputPath, string MEMONO, string DDATE, string rate, string amount_word, string StrPartyName,
                                                    string StrAddress1, string StrAddress2, string StrGSTNo, string StrClientID, string StrGIAGSTNo, string StrMemoNo, double StrCostAmtFE)
        {
            // Create the Excel Application object
            Excel.Application excelApp = new Excel.Application();

            Excel.Workbook excelWorkbook = excelApp.Workbooks.Add(Type.Missing);

            //Microsoft.Office.Interop.Excel.Range xlRange;
            int sheetIndex = 0;

           // DDATE = DateTime.Now.ToString("dd-MM-yyyy");

            // Copy each DataTable
            foreach (System.Data.DataTable dt in dataSet.Tables)
            {
                decimal RECORD_ONE_PAGGE = 37;
                decimal total_recored = dt.Rows.Count;
                decimal NUM_PAGE = Convert.ToDecimal(total_recored / RECORD_ONE_PAGGE);

                int PAGE = 0;
                string[] digits = NUM_PAGE.ToString().Split('.');
                PAGE = Convert.ToInt32(digits[0]);
                if (Convert.ToDecimal(digits[1]) > 0)
                {
                    PAGE = PAGE + 1;
                }

                int addrow = dt.Rows.Count + (PAGE * 14);


                int R_ROW = 9;
                int r_count = 0;

                // Copy the DataTable to an object array
                object[,] rawData = new object[addrow, dt.Columns.Count];

                // Copy the values to the object array
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    if (r_count == RECORD_ONE_PAGGE)
                    {
                        r_count = 0;

                        R_ROW = R_ROW + 14;


                        for (int col = 0; col < dt.Columns.Count; col++)
                        {
                            rawData[R_ROW, col] = dt.Rows[row][col];
                        }
                        R_ROW += 1;
                    }
                    else
                    {
                        for (int col = 0; col < dt.Columns.Count; col++)
                        {
                            rawData[R_ROW, col] = dt.Rows[row][col];
                        }
                        R_ROW += 1;
                    }
                    r_count = r_count + 1;
                }

                // Calculate the final column letter
                string finalColLetter = string.Empty;
                string colCharset = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                int colCharsetLen = colCharset.Length;

                if (dt.Columns.Count > colCharsetLen)
                {
                    finalColLetter = colCharset.Substring((dt.Columns.Count - 1) / colCharsetLen - 1, 1);
                }

                finalColLetter += colCharset.Substring((dt.Columns.Count - 1) % colCharsetLen, 1);

                // Create a new Sheet
                Excel.Worksheet excelSheet = (Excel.Worksheet)excelWorkbook.Sheets.Add(excelWorkbook.Sheets.get_Item(++sheetIndex), Type.Missing, 1, Excel.XlSheetType.xlWorksheet);

                excelSheet.Name = dt.TableName;

                // Fast data export to Excel
                string excelRange = string.Format("A1:{0}{1}", finalColLetter, addrow);

                excelSheet.get_Range(excelRange, Type.Missing).Value2 = rawData;


                int F = 0;
                for (int i = 0; i < PAGE; i++)
                {

                    excelSheet.Columns[1].ColumnWidth = 5.14;
                    excelSheet.Columns[2].ColumnWidth = 8.57;
                    excelSheet.Columns[3].ColumnWidth = 6;
                    excelSheet.Columns[4].ColumnWidth = 6.71;
                    excelSheet.Columns[5].ColumnWidth = 6.71;
                    excelSheet.Columns[6].ColumnWidth = 9.71;
                    excelSheet.Columns[7].ColumnWidth = 6.29;
                    excelSheet.Columns[8].ColumnWidth = 6;
                    excelSheet.Columns[9].ColumnWidth = 7.14;
                    excelSheet.Columns[10].ColumnWidth = 9.14;
                    excelSheet.Columns[11].ColumnWidth = 8.57;
                    excelSheet.Columns[12].ColumnWidth = 12;


                    string A2 = "A" + (F + 2).ToString();
                    string G2 = "G" + (F + 2).ToString();
                    string A3 = "A" + (F + 3).ToString();
                    string G3 = "G" + (F + 3).ToString();
                    string A4 = "A" + (F + 4).ToString();
                    string G4 = "G" + (F + 4).ToString();
                    string A5 = "A" + (F + 5).ToString();
                    string G5 = "G" + (F + 5).ToString();

                    //excelSheet.Range[A2, G2].Merge();
                    //excelSheet.Range[A2, A2].Value = "TO,";
                    //excelSheet.Range[A3, G3].Merge();
                    //excelSheet.Range[A3, A3].Value = "GIA INDIA PVT. LTD ";
                    //excelSheet.Range[A4, G4].Merge();
                    //excelSheet.Range[A4, A4].Value = "2-3 FL, SWASTICK UNIVERSAL, NEAR VELANTINE CINEMA, ";
                    //excelSheet.Range[A5, G5].Merge();
                    //excelSheet.Range[A5, A5].Value = "DUMAS ROAD, PIPLOD, SURAT    ";
                    excelSheet.Range[A2, G2].Merge();
                    excelSheet.Range[A2, A2].Value = "TO,";
                    excelSheet.Range[A3, G3].Merge();
                    excelSheet.Range[A3, A3].Value = StrPartyName;
                    excelSheet.Range[A4, G4].Merge();
                    excelSheet.Range[A4, A4].Value = StrAddress1;
                    excelSheet.Range[A5, G5].Merge();
                    excelSheet.Range[A5, A5].Value = StrAddress2;

                    string H2 = "H" + (F + 2).ToString();
                    string L2 = "L" + (F + 2).ToString();
                    string H3 = "H" + (F + 3).ToString();
                    string L3 = "L" + (F + 3).ToString();
                    string H4 = "H" + (F + 4).ToString();
                    string L4 = "L" + (F + 4).ToString();
                    string H5 = "H" + (F + 5).ToString();
                    string L5 = "L" + (F + 5).ToString();

                    //excelSheet.Range[H2, L2].Merge();
                    //excelSheet.Range[H2, H2].Value = "GST No. 24AANFR1290A1ZI";
                    //excelSheet.Range[H3, L3].Merge();
                    //excelSheet.Range[H3, H3].Value = "GIA-GST No. 24AACCG9457G1ZH";
                    //excelSheet.Range[H4, L4].Merge();
                    //excelSheet.Range[H4, H4].Value = "MEMO NO. " + MEMONO + " /DATE : " + DDATE;
                    //excelSheet.Range[H5, L5].Merge();
                    //excelSheet.Range[H5, H5].Value = "GIA Client ID : 300300845142";
                    excelSheet.Range[H2, L2].Merge();
                    excelSheet.Range[H2, H2].Value = "GST No." + StrGSTNo;
                    excelSheet.Range[H3, L3].Merge();
                    excelSheet.Range[H3, H3].Value = "GIA-GST No. " + StrGIAGSTNo;
                    excelSheet.Range[H4, L4].Merge();
                    excelSheet.Range[H4, H4].Value = "MEMO NO. " + StrMemoNo + " /DATE : " + DDATE;
                    //excelSheet.Range[H4, H4].Value = "MEMO NO. " + StrMemoNo + " /DATE : " + DateTime.Now.ToString("ddMMyyyy");
                    excelSheet.Range[H5, L5].Merge();
                    excelSheet.Range[H5, H5].Value = "GIA Client ID :" + StrClientID;

                    string A6 = "A" + (F + 6).ToString();
                    string l6 = "l" + (F + 6).ToString();

                    excelSheet.Range[A6, l6].Merge();
                    excelSheet.Range[A6, A6].Value = " Packing List Cut & Polish Diamond    ";
                    excelSheet.Range[A6, A6].HorizontalAlignment = -4108;
                    ((Excel.Range)excelSheet.Rows[(F + 6).ToString(), Type.Missing]).Font.Bold = true;

                    string A7 = "A" + (F + 7).ToString();
                    string l7 = "l" + (F + 7).ToString();

                    excelSheet.Range[A7, l7].Merge();
                    excelSheet.Range[A7, A7].Value = "PAGE-" + (i + 1);
                    excelSheet.Range[A7, A7].HorizontalAlignment = -4108;

                    string A8 = "A" + (F + 8).ToString();
                    string B8 = "B" + (F + 8).ToString();
                    string C8 = "C" + (F + 8).ToString();
                    string D8 = "D" + (F + 8).ToString();
                    string E8 = "E" + (F + 8).ToString();
                    string F8 = "F" + (F + 8).ToString();
                    string G8 = "G" + (F + 8).ToString();
                    string H8 = "H" + (F + 8).ToString();
                    string I8 = "I" + (F + 8).ToString();
                    string J8 = "J" + (F + 8).ToString();
                    string K8 = "K" + (F + 8).ToString();
                    string L8 = "L" + (F + 8).ToString();

                    excelSheet.Range[A8, A8].Value = "SR.NO";
                    excelSheet.Range[B8, B8].Value = "CLIENT ID";
                    excelSheet.Range[C8, C8].Value = "SHAPE";
                    excelSheet.Range[D8, D8].Value = "SERVICE";
                    excelSheet.Range[E8, E8].Value = "CARAT";
                    excelSheet.Range[F8, F8].Value = "DIAMETER";
                    excelSheet.Range[G8, G8].Value = "HEIGHT";
                    excelSheet.Range[H8, H8].Value = "COLOR";
                    excelSheet.Range[I8, I8].Value = "CLARITY";
                    excelSheet.Range[J8, J8].Value = "$/CT";
                    excelSheet.Range[K8, K8].Value = "TOTAL";
                    excelSheet.Range[L8, L8].Value = "CONTROL NO.";

                    string A10 = "A" + (F + 10).ToString();
                    string L10 = "L" + (F + 47).ToString();
                    excelSheet.Range[A10, L10].HorizontalAlignment = -4108;


                    string B47 = "B" + (F + 47).ToString();
                    string D47 = "D" + (F + 47).ToString();
                    string F47 = "F" + (F + 47).ToString();
                    string J47 = "J" + (F + 47).ToString();

                    excelSheet.Range[B47, D47].Merge();
                    excelSheet.Range[F47, J47].Merge();

                    string A47 = "A" + (F + 47).ToString();
                    excelSheet.Range[A47, A47].Value = "TOTAL";

                    string E47 = "E" + (F + 47).ToString();
                    string E46 = "E" + (F + 46).ToString();

                    string E9 = "E" + (F + 9).ToString();
                    string CT = "E" + (F - 4).ToString();
                    excelSheet.Range[E9, E9].Formula = "= IFERROR(ROUND(" + CT + ",2),\"\") ";

                    string K9 = "K" + (F + 9).ToString();
                    string AM = "K" + (F - 4).ToString();
                    excelSheet.Range[K9, K9].Formula = "= IFERROR(ROUND(" + AM + ",2),\"\") ";

                    excelSheet.Range[E47, E47].Formula = "=ROUND(SUM(" + E9 + ":" + E46 + "),2) ";

                    string K47 = "K" + (F + 47).ToString();

                    string K46 = "K" + (F + 46).ToString();
                    excelSheet.Range[K47, K47].Formula = "=ROUND(SUM(" + K9 + ":" + K46 + "),2) ";

                    if (i == PAGE - 1)
                    {
                        string F47F = "F" + (F + 47).ToString();
                        excelSheet.Range[F47F, F47F].Value = "Total US Dollar $";
                        excelSheet.Range[F47F, F47F].HorizontalAlignment = -4152;

                        //                        Left: -4131
                        //Center: -4108
                        //Right: -4152

                        excelSheet.Range[A47].HorizontalAlignment = -4152;

                        string K48 = "K" + (F + 48).ToString();
                        //excelSheet.Range[K48, K48].Formula = "= IFERROR(ROUND(" + K47 + "*" + rate + ",2),\"\") ";

                        string A48 = "A" + (F + 48).ToString();
                        string H48 = "H" + (F + 48).ToString();
                        excelSheet.Range[A48, H48].Merge();
                        excelSheet.Range[A48, A48].Value = amount_word;

                        string I48 = "I" + (F + 48).ToString();
                        string J48 = "J" + (F + 48).ToString();
                        excelSheet.Range[I48, J48].Merge();
                        excelSheet.Range[I48, I48].Value = "Total INR. Rupees";
                        excelSheet.Range[K48, K48].Value = StrCostAmtFE;


                        string A49 = "A" + (F + 49).ToString();
                        string E52 = "E" + (F + 52).ToString();
                        excelSheet.Range[A49, E52].Merge();

                        string F49 = "F" + (F + 49).ToString();
                        string H49 = "H" + (F + 49).ToString();
                        string F50 = "F" + (F + 50).ToString();
                        string H51 = "H" + (F + 51).ToString();
                        string F52 = "F" + (F + 52).ToString();
                        string H52 = "H" + (F + 52).ToString();

                        excelSheet.Range[F49, H49].Merge();
                        excelSheet.Range[F49, F49].Value = "For- RIJIYA GEMS";
                        excelSheet.Range[F50, H51].Merge();
                        excelSheet.Range[F52, H52].Merge();
                        excelSheet.Range[F52, F52].Value = "PARTNER";


                        string I49 = "I" + (F + 49).ToString();
                        string L52 = "L" + (F + 52).ToString();
                        excelSheet.Range[I49, L52].Merge();



                        excelSheet.Range[A2, L52].Borders.LineStyle = BorderStyle.FixedSingle;
                        excelSheet.Range[A2, L52].Borders.Color = Color.Black;
                    }
                    else
                    {
                        string A48 = "A" + (F + 48).ToString();
                        string E51 = "E" + (F + 51).ToString();

                        excelSheet.Range[A48, E51].Merge();

                        string F48 = "F" + (F + 48).ToString();
                        string H48 = "H" + (F + 48).ToString();
                        string F49 = "F" + (F + 49).ToString();
                        string H50 = "H" + (F + 50).ToString();
                        string F51 = "F" + (F + 51).ToString();
                        string H51 = "H" + (F + 51).ToString();

                        excelSheet.Range[F48, H48].Merge();
                        excelSheet.Range[F48, F48].Value = "For- RIJIYA GEMS";
                        excelSheet.Range[F49, H50].Merge();
                        excelSheet.Range[F51, H51].Merge();
                        excelSheet.Range[F51, F51].Value = "PARTNER";


                        string I48 = "I" + (F + 48).ToString();
                        string L51 = "L" + (F + 51).ToString();
                        excelSheet.Range[I48, L51].Merge();

                        excelSheet.Range[A2, L51].Borders.LineStyle = BorderStyle.FixedSingle;
                        excelSheet.Range[A2, L51].Borders.Color = Color.Black;

                    }
                    F = F + 51;
                }
            }

            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            excelApp.DisplayAlerts = false;
            excelWorkbook.SaveCopyAs(outputPath);
            excelWorkbook.Saved = true;

            //string StrFilrPath = AppDomain.CurrentDomain.BaseDirectory + "EXCEL\\LabIssueStone1.pdf";
            //excelWorkbook.SaveAs(outputPath,".pdf");

            excelWorkbook.Close(true, Type.Missing, Type.Missing);
            excelWorkbook = null;

            // Release the Application object
            excelApp.Quit();
            excelApp = null;
        }

        public DataTable GetSelectedRowToTable()
        {
            Int32[] selectedRowHandles = GrdDetail.GetSelectedRows();
            DataTable resultTable = DtabStockDetail.Clone();
            for (int i = 0; i < selectedRowHandles.Length; i++)
            {
                int J = selectedRowHandles[i];
                DataRowView DR = (DataRowView)GrdDetail.GetRow(J);
                resultTable.Rows.Add(DR.Row.ItemArray);
            }
            return resultTable;
        }

        private void BtnLabIssue_Click(object sender, EventArgs e)
        {
            try
            {


                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);

                //DataTable DtInvDetail = GetSelectedRowToTable();

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                string StrStone = "";
                if (DtInvDetail.Rows.Count != 0)
                {
                    var list = DtInvDetail.AsEnumerable().Select(r => r["STOCKNO"].ToString());
                    StrStone = string.Join(",", list);
                }


                LiveStockProperty mProperty = new LiveStockProperty();
                mProperty.STOCKNO = Val.ToString(StrStone);
                mProperty.MULTYSHAPE_ID = "";
                mProperty.MULTYCOLOR_ID = "";
                mProperty.MULTYCLARITY_ID = "";
                mProperty.MULTYCUT_ID = "";
                mProperty.MULTYSYM_ID = "";
                mProperty.MULTYPOL_ID = "";
                mProperty.MULTYFL_ID = "";

                mProperty.BARCODE = "";
                mProperty.DIAMONDTYPE = "ALL";
                DataSet DsLiveStock = ObjStock.GetNewLiveStockDataNew(mProperty, "All","FULLSTOCK",false);
                DataTable DtabLiveStockDetail = DsLiveStock.Tables[0];

                if (DtabLiveStockDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                string StrStoneNo = string.Empty;
                string StrStoneNoForAvgPrice = string.Empty;

                FrmMemoEntryNew FrmMemoEntryNew = new FrmMemoEntryNew();
                FrmMemoEntryNew.ShowForm(Stock.FrmMemoEntryNew.FORMTYPE.LABISSUE, DtabLiveStockDetail, "SINGLE");

                this.Cursor = Cursors.Default;
                if (ObjGridSelection != null)
                {
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                    BtnSearch_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void MainGrdDetail_Click(object sender, EventArgs e)
        {

        }

        private void BtnPickupSave_Click(object sender, EventArgs e)
        {
            //DataTable DTab = DtabStockDetail;
            DataTable DTab = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);

            if (DTab.Rows.Count == 0)
            {
                this.Cursor = Cursors.Default;
                Global.Message("No Data Found For Excel Export");
                return;
                this.Cursor = Cursors.Default;
            }
            // Check if LABNAME values in DTabStcokGrid are not the same
            if (DTab.Rows.Count > 0)
            {
                // Get the first LABNAME value to compare with others
                string firstLabName = "GIA";

                foreach (DataRow row in DTab.Rows)
                {
                    // Compare each LABNAME value with the first one
                    if (!row["LABNAME"].ToString().Trim().Equals(firstLabName, StringComparison.OrdinalIgnoreCase))
                    {
                        // Show error message if LABNAME values are different
                        Global.Message("LAB Are Not Same In The Selected Stone List , Please Check");
                        return;
                    }
                }
            }
            GrpMemoNoGenerate.Visible = true;
            DataRow DR = ObjMemo.GetCurrency();
            if (DR == null)
            {
                txtCurrency.Text = "";
                txtCurrency.Tag = "";
            }
            else
            {
                txtCurrency.Text = Val.ToString(DR["CurrencyName"]);
                txtCurrency.Tag = Val.ToString(DR["Currency_ID"]);
            }
        }

        private void btnPickupClose_Click(object sender, EventArgs e)
        {
            GrpMemoNoGenerate.Visible = false;
        }

        private void txtCurrency_KeyPress(object sender, KeyPressEventArgs e)
        {

            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "CURRENCYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CURRENCY);

                    FrmSearch.mStrColumnsToHide = "CURRENCY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtCurrency.Text = Val.ToString(FrmSearch.DRow["CURRENCYNAME"]);
                        txtCurrency.Tag = Val.ToString(FrmSearch.DRow["CURRENCY_ID"]);

                        txtExcRate.Text = ObjMemo.GetExchangeRate(Val.ToInt(txtCurrency.Tag), "", "").ToString();
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

        private void BtnIGIExport_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    DataTable DtInvDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);
                    if (DtInvDetail == null)
                    {
                        Global.Message("Please Select AtLeast One Record From The List.");
                        return;
                    }
                    if (DtInvDetail.Rows.Count <= 0)
                    {
                        Global.Message("Please Select AtLeast One Record From The List.");
                        return;
                    }
                    string strStoneNo = "";

                    // Check if LABNAME values in DTabStcokGrid are not the same
                    if (DtInvDetail.Rows.Count > 0)
                    {
                        // Get the first LABNAME value to compare with others
                        string firstLabName = "IGI";

                        foreach (DataRow row in DtInvDetail.Rows)
                        {
                            // Compare each LABNAME value with the first one
                            if (!row["LABNAME"].ToString().Trim().Equals(firstLabName, StringComparison.OrdinalIgnoreCase))
                            {
                                // Show error message if LABNAME values are different
                                Global.Message("LAB Are Not Same In The Selected Stone List , Please Check");
                                return;
                            }
                        }
                    }
                    DtInvDetail.DefaultView.Sort = "SEQUENCE";
                    if (DtInvDetail.Rows.Count > 0)
                    {
                        var list = DtInvDetail.AsEnumerable().Select(r => r["PARTYSTOCKNO"].ToString());
                        strStoneNo = string.Join(",", list);
                    }


                    DataTable DTabExcel = ObjStock.GetSingleStoneManualPredictionData(strStoneNo);


                    DataTable DTABEXCELNEW = DTabExcel;

                    object misValue = System.Reflection.Missing.Value;
                    SaveFileDialog svDialog = new SaveFileDialog();
                    svDialog.DefaultExt = "xlsx";
                    svDialog.Title = "Export to Excel";
                    svDialog.FileName = "Export Excel.xlsx";
                    svDialog.Filter = "Excel files 2007(*.xlsx)|*.xlsx|All files (*.*)|*.*";



                    if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                    {
                        string StrFilePath = svDialog.FileName;

                        if (File.Exists(StrFilePath))
                        {
                            File.Delete(StrFilePath);
                        }

                        FileInfo workBook = new FileInfo(StrFilePath);
                        Color BackColor = Color.LightGray;
                        Color FontColor = Color.Black;
                        string FontName = "Verdana";
                        float FontSize = 8;

                        int StartRow = 0;
                        int StartColumn = 0;
                        int EndRow = 0;

                        int EndColumn = 0;

                        using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                        {
                            ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Result_" + DateTime.Now.ToString("ddMMyyyy"));

                            StartRow = 1;
                            EndRow = StartRow + DTABEXCELNEW.Rows.Count;
                            StartColumn = 1;
                            EndColumn = DTABEXCELNEW.Columns.Count;


                            worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].LoadFromDataTable(DTABEXCELNEW, true);
                            worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;


                            //worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            //worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            //worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            //worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Name = FontName;
                            worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Size = FontSize;

                            worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                            worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                            worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(FontColor);


                            worksheet.Cells["A:Z"].AutoFitColumns();
                            xlPackage.Save();

                            if (Global.Confirm("Do You Want To Open [Export Excel.xlsx] ?") == System.Windows.Forms.DialogResult.Yes)
                            {
                                System.Diagnostics.Process.Start(svDialog.FileName, "CMD");
                            }
                        }
                        svDialog.Dispose();
                        svDialog = null;
                    }
                }
                catch (Exception ex)
                {
                    Global.Message(ex.Message.ToString());
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }

        public static void ExportToExcel_HRD(DataSet dataSet, string outputPath, string MEMONO, string DDATE, string rate, string amount_word, string StrPartyName,
                                                    string StrAddress1, string StrAddress2, string StrGSTNo, string StrClientID, string StrGIAGSTNo, string StrMemoNo, double StrCostAmtFE)
        {
            // Create the Excel Application object
            Excel.Application excelApp = new Excel.Application();

            Excel.Workbook excelWorkbook = excelApp.Workbooks.Add(Type.Missing);

            //Microsoft.Office.Interop.Excel.Range xlRange;
            int sheetIndex = 0;

            // DDATE = DateTime.Now.ToString("dd-MM-yyyy");

            // Copy each DataTable
            foreach (System.Data.DataTable dt in dataSet.Tables)
            {
                decimal RECORD_ONE_PAGGE = 37;
                decimal total_recored = dt.Rows.Count;
                decimal NUM_PAGE = Convert.ToDecimal(total_recored / RECORD_ONE_PAGGE);

                int PAGE = 0;
                string[] digits = NUM_PAGE.ToString().Split('.');
                PAGE = Convert.ToInt32(digits[0]);
                if (Convert.ToDecimal(digits[1]) > 0)
                {
                    PAGE = PAGE + 1;
                }

                int addrow = dt.Rows.Count + (PAGE * 14);


                int R_ROW = 7;
                int r_count = 0;

                // Copy the DataTable to an object array
                object[,] rawData = new object[addrow, dt.Columns.Count];

                // Copy the values to the object array
                for (int row = 0; row < dt.Rows.Count; row++)
                {
                    if (r_count == RECORD_ONE_PAGGE)
                    {
                        r_count = 0;

                        R_ROW = R_ROW + 14;


                        for (int col = 0; col < dt.Columns.Count; col++)
                        {
                            rawData[R_ROW, col] = dt.Rows[row][col];
                        }
                        R_ROW += 1;
                    }
                    else
                    {
                        for (int col = 0; col < dt.Columns.Count; col++)
                        {
                            rawData[R_ROW, col] = dt.Rows[row][col];
                        }
                        R_ROW += 1;
                    }
                    r_count = r_count + 1;
                }

                // Calculate the final column letter
                string finalColLetter = string.Empty;
                string colCharset = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                int colCharsetLen = colCharset.Length;

                if (dt.Columns.Count > colCharsetLen)
                {
                    finalColLetter = colCharset.Substring((dt.Columns.Count - 1) / colCharsetLen - 1, 1);
                }

                finalColLetter += colCharset.Substring((dt.Columns.Count - 1) % colCharsetLen, 1);

                // Create a new Sheet
                Excel.Worksheet excelSheet = (Excel.Worksheet)excelWorkbook.Sheets.Add(excelWorkbook.Sheets.get_Item(++sheetIndex), Type.Missing, 1, Excel.XlSheetType.xlWorksheet);

                excelSheet.Name = dt.TableName;

                // Fast data export to Excel
                string excelRange = string.Format("B1:{0}{1}", finalColLetter, addrow);

                excelSheet.get_Range(excelRange, Type.Missing).Value2 = rawData;


                int F = 0;
                for (int i = 0; i < PAGE; i++)
                {

                    excelSheet.Columns[1].ColumnWidth = 4;
                    excelSheet.Columns[2].ColumnWidth = 4.30;
                    excelSheet.Columns[3].ColumnWidth = 10;
                    excelSheet.Columns[4].ColumnWidth = 12;
                    excelSheet.Columns[5].ColumnWidth = 8;
                    excelSheet.Columns[6].ColumnWidth = 8;
                    excelSheet.Columns[7].ColumnWidth = 8;
                    excelSheet.Columns[8].ColumnWidth = 8;
                    excelSheet.Columns[9].ColumnWidth = 10;
                    excelSheet.Columns[10].ColumnWidth = 9;
                    excelSheet.Columns[11].ColumnWidth = 8;
                    excelSheet.Columns[12].ColumnWidth = 12;
                    excelSheet.Columns[13].ColumnWidth = 13;
                    excelSheet.Columns[14].ColumnWidth = 24;
                    excelSheet.Columns[15].ColumnWidth = 14;
                    excelSheet.Columns[16].ColumnWidth = 14;
                    excelSheet.Columns[17].ColumnWidth = 14;


                    string B1 = "B" + (F + 1).ToString();
                    string Q1 = "Q" + (F + 1).ToString();

                    excelSheet.Range[B1, Q1].Merge();
                    excelSheet.Range[B1, B1].Value = " ANNEXURE TO INVOICE    ";
                    excelSheet.Range[B1, B1].HorizontalAlignment = -4108;
                    ((Excel.Range)excelSheet.Rows[(F + 1).ToString(), Type.Missing]).Font.Bold = true;

                    string B2 = "B" + (F + 2).ToString();
                    string Q2 = "Q" + (F + 2).ToString();

                    excelSheet.Range[B2, Q2].Merge();
                    excelSheet.Range[B2, B2].Value = " PACKING LIST OF HRD Diamond Institute Private Ltd    ";
                    excelSheet.Range[B2, B2].HorizontalAlignment = -4108;
                    ((Excel.Range)excelSheet.Rows[(F + 2).ToString(), Type.Missing]).Font.Bold = true;
                    ((Excel.Range)excelSheet.Rows[(F + 2).ToString(), Type.Missing]).Font.Underline = true;
                    ((Excel.Range)excelSheet.Rows[(F + 2).ToString(), Type.Missing]).Font.Size = 24;
                    ((Excel.Range)excelSheet.Rows[(F + 2).ToString(), Type.Missing]).Font.Name = "Times New Roman";

                    string B3 = "B" + (F + 3).ToString();
                    string N3 = "N" + (F + 3).ToString();

                    excelSheet.Range[B3, N3].Merge();
                    excelSheet.Range[B3, B3].Value = "NAME OF CLIENT :-" + "____" + StrPartyName + "_______________________________________________";
                    //excelSheet.Range[B3, B3].HorizontalAlignment = -4108;
                    ((Excel.Range)excelSheet.Rows[(F + 3).ToString(), Type.Missing]).Font.Bold = true;
                    ((Excel.Range)excelSheet.Rows[(F + 3).ToString(), Type.Missing]).Font.Size = 14;
                    ((Excel.Range)excelSheet.Rows[(F + 3).ToString(), Type.Missing]).Font.Name = "Arial";

                    //((Excel.Range)excelSheet.Rows[(F + 2).ToString(), Type.Missing]).Font.Underline = true;

                    string O3 = "O" + (F + 3).ToString();
                    string Q3 = "Q" + (F + 3).ToString();

                    excelSheet.Range[O3, Q3].Merge();
                    excelSheet.Range[O3, O3].Value = " DATE :- " + DDATE;
                    //excelSheet.Range[B3, B3].HorizontalAlignment = -4108;
                    ((Excel.Range)excelSheet.Rows[(F + 3).ToString(), Type.Missing]).Font.Bold = true;
                    ((Excel.Range)excelSheet.Rows[(F + 3).ToString(), Type.Missing]).Font.Size = 14;
                    ((Excel.Range)excelSheet.Rows[(F + 3).ToString(), Type.Missing]).Font.Name = "Arial";




                    string B5 = "B" + (F + 5).ToString();
                    string B6 = "B" + (F + 6).ToString();
                    string C5 = "C" + (F + 5).ToString();
                    string C6 = "C" + (F + 6).ToString();
                    string D5 = "D" + (F + 5).ToString();
                    string D6 = "D" + (F + 6).ToString();

                    string E5 = "E" + (F + 5).ToString();
                    string E6 = "E" + (F + 6).ToString();
                    string F5 = "F" + (F + 5).ToString();
                    string F6 = "F" + (F + 6).ToString();
                    string G5 = "G" + (F + 5).ToString();
                    string G6 = "G" + (F + 6).ToString();
                    string H5 = "H" + (F + 5).ToString();
                    string H6 = "H" + (F + 6).ToString();
                    string I5 = "I" + (F + 5).ToString();
                    string I6 = "I" + (F + 6).ToString();
                    string J5 = "J" + (F + 5).ToString();
                    string J6 = "J" + (F + 6).ToString();
                    string K5 = "K" + (F + 5).ToString();
                    string K6 = "K" + (F + 6).ToString();
                    string L5 = "L" + (F + 5).ToString();
                    string L6 = "L" + (F + 6).ToString();
                    string M5 = "M" + (F + 5).ToString();
                    string M6 = "M" + (F + 6).ToString();

                    string N5 = "N" + (F + 5).ToString();
                    string N6 = "N" + (F + 6).ToString();

                    string O5 = "O" + (F + 5).ToString();
                    string O6 = "O" + (F + 6).ToString();

                    string P5 = "P" + (F + 5).ToString();
                    string P6 = "P" + (F + 6).ToString();

                    string Q5 = "Q" + (F + 5).ToString();
                    string Q6 = "Q" + (F + 6).ToString();

                    excelSheet.Range[B5, B6].Merge();
                    excelSheet.Range[B5, B5].Value = "SR NO";
                    excelSheet.Range[B5, B5].HorizontalAlignment = -4108;
                    excelSheet.Range[B5, B5].WrapText = true;

                    excelSheet.Range[C5, C6].Merge();
                    excelSheet.Range[C5, C5].Value = "LOT NO.";
                    excelSheet.Range[C5, C5].HorizontalAlignment = -4108;
                    excelSheet.Range[D5, D5].Value = "DIAMOND";
                    excelSheet.Range[D5, D5].HorizontalAlignment = -4108;
                    excelSheet.Range[D6, D6].Value = "SHAPE";
                    excelSheet.Range[D6, D6].HorizontalAlignment = -4108;

                    excelSheet.Range[E5, E5].Value = "DIA";
                    excelSheet.Range[E5, E5].HorizontalAlignment = -4108;
                    excelSheet.Range[E6, E6].Value = "CTS. WT.";
                    excelSheet.Range[E6, E6].HorizontalAlignment = -4108;

                    excelSheet.Range[F5, F5].Value = "DIA";
                    excelSheet.Range[F5, F5].HorizontalAlignment = -4108;
                    excelSheet.Range[F6, F6].Value = "COLOR";
                    excelSheet.Range[F6, F6].HorizontalAlignment = -4108;

                    excelSheet.Range[G5, G5].Value = "DIA";
                    excelSheet.Range[G5, G5].HorizontalAlignment = -4108;
                    excelSheet.Range[G6, G6].Value = "CLARITY";
                    excelSheet.Range[G6, G6].HorizontalAlignment = -4108;

                    excelSheet.Range[H5, H5].Value = "DIA";
                    excelSheet.Range[H5, H5].HorizontalAlignment = -4108;
                    excelSheet.Range[H6, H6].Value = "CUT";
                    excelSheet.Range[H6, H6].HorizontalAlignment = -4108;

                    excelSheet.Range[I5, I5].Value = "DIAMETER";
                    excelSheet.Range[I5, I5].HorizontalAlignment = -4108;
                    excelSheet.Range[I6, I6].Value = "(mm)";
                    excelSheet.Range[I6, I6].HorizontalAlignment = -4108;

                    excelSheet.Range[J5, J5].Value = "HEIGHT";
                    excelSheet.Range[J5, J5].HorizontalAlignment = -4108;
                    excelSheet.Range[J6, J6].Value = "(mm)";
                    excelSheet.Range[J6, J6].HorizontalAlignment = -4108;

                    excelSheet.Range[K5, K5].Value = "WIDTH";
                    excelSheet.Range[K5, K5].HorizontalAlignment = -4108;
                    excelSheet.Range[K6, K6].Value = "(mm)";
                    excelSheet.Range[K6, K6].HorizontalAlignment = -4108;

                    excelSheet.Range[L5, L5].Value = "RATE/CT";
                    excelSheet.Range[L5, L5].HorizontalAlignment = -4108;
                    excelSheet.Range[L6, L6].Value = "IN US$";
                    excelSheet.Range[L6, L6].HorizontalAlignment = -4108;

                    excelSheet.Range[M5, M5].Value = "DIA VALUE";
                    excelSheet.Range[M5, M5].HorizontalAlignment = -4108;
                    excelSheet.Range[M6, M6].Value = "IN US$";
                    excelSheet.Range[M6, M6].HorizontalAlignment = -4108;

                    excelSheet.Range[N5, N6].Merge();
                    excelSheet.Range[N5, N5].Value = "Service";
                    excelSheet.Range[N5, N5].HorizontalAlignment = -4108;

                    excelSheet.Range[O5, O5].Value = "Removal of Laser";
                    excelSheet.Range[O5, O5].HorizontalAlignment = -4108;
                    excelSheet.Range[O5, O5].WrapText = true;
                    excelSheet.Range[O6, O6].Value = "(Yes or no)";
                    excelSheet.Range[O6, O6].HorizontalAlignment = -4108;

                    excelSheet.Range[P5, P5].Value = "Laser inscription";
                    excelSheet.Range[P5, P5].HorizontalAlignment = -4108;
                    excelSheet.Range[P5, P5].WrapText = true;
                    excelSheet.Range[P6, P6].Value = "(Yes or no)";
                    excelSheet.Range[P6, P6].HorizontalAlignment = -4108;

                    excelSheet.Range[Q5, Q5].Value = "Sealing";
                    excelSheet.Range[Q5, Q5].HorizontalAlignment = -4108;
                    excelSheet.Range[Q6, Q6].Value = "(Yes or no)";
                    excelSheet.Range[Q6, Q6].HorizontalAlignment = -4108;

                    excelSheet.Range[B5, Q6].Borders.LineStyle = BorderStyle.FixedSingle;
                    excelSheet.Range[B5, Q6].Borders.Color = Color.Black;

                    string A8 = "A" + (F + 8).ToString();
                    string Q8 = "Q" + (F + 8).ToString();
                    excelSheet.Range[A8, Q8].HorizontalAlignment = -4108;

                    string D35 = "D" + (F + 35).ToString();
                    string F35 = "F" + (F + 35).ToString();
                    string J35 = "J" + (F + 35).ToString();
                    string L35 = "L" + (F + 35).ToString();

                    string B35 = "B" + (F + 35).ToString();
                    excelSheet.Range[B35, D35].Merge();
                    excelSheet.Range[B35, B35].Value = "Total Carat Weight";
                    excelSheet.Range[B35, B35].HorizontalAlignment = -4108;

                    string E35 = "E" + (F + 35).ToString();
                    string E34 = "E" + (F + 34).ToString();

                    string E8 = "E" + (F + 8).ToString();
                    string CT = "E" + (F - 4).ToString();
                    //excelSheet.Range[E8, E8].Formula = "= IFERROR(ROUND(" + CT + ",2),\"\") ";

                    string M8 = "M" + (F + 8).ToString();
                    string AM = "M" + (F - 4).ToString();
                    //excelSheet.Range[M8, M8].Formula = "= IFERROR(ROUND(" + AM + ",2),\"\") ";

                    excelSheet.Range[E35, E35].Formula = "=ROUND(SUM(" + E8 + ":" + E34 + "),2) ";

                    string M35 = "M" + (F + 35).ToString();

                    string M34 = "M" + (F + 34).ToString();
                    excelSheet.Range[J35, L35].Merge();
                    excelSheet.Range[J35, J35].Value = "Total Value (US $)";
                    excelSheet.Range[J35, J35].HorizontalAlignment = -4108;
                    excelSheet.Range[M35, M35].Formula = "=ROUND(SUM(" + M8 + ":" + M34 + "),2) ";

                    string B8 = "B" + (F + 8).ToString();
                    string Q35 = "Q" + (F + 35).ToString();

                    excelSheet.Range[B5, Q35].Borders.LineStyle = BorderStyle.FixedSingle;
                    excelSheet.Range[B5, Q35].Borders.Color = Color.Black;
                    excelSheet.Range[B5, Q35].Font.Bold = true;

                    string D37 = "D" + (F + 37).ToString();
                    string I37 = "I" + (F + 37).ToString();
                    excelSheet.Range[D37, I37].Merge();
                    excelSheet.Range[D37, D37].Value = "GST :-" + StrGSTNo;
                    excelSheet.Range[B35, B35].HorizontalAlignment = -4108;
                    excelSheet.Range[D37, I37].Font.Bold = true;
                    excelSheet.Range[D37, I37].Borders.LineStyle = BorderStyle.FixedSingle;
                    excelSheet.Range[D37, I37].Borders.Color = Color.Black;


                    string N37 = "N" + (F + 37).ToString();
                    string P37 = "P" + (F + 37).ToString();
                    excelSheet.Range[N37, P37].Merge();
                    excelSheet.Range[N37, N37].Value = " Company Seal / Signature";
                    excelSheet.Range[B35, B35].HorizontalAlignment = -4108;
                    excelSheet.Range[N37, P37].Font.Bold = true;

                    F = F + 51;
                }
            }

            if (File.Exists(outputPath))
            {
                File.Delete(outputPath);
            }

            excelApp.DisplayAlerts = false;
            excelWorkbook.SaveCopyAs(outputPath);
            excelWorkbook.Saved = true;

            //string StrFilrPath = AppDomain.CurrentDomain.BaseDirectory + "EXCEL\\LabIssueStone1.pdf";
            //excelWorkbook.SaveAs(outputPath,".pdf");

            excelWorkbook.Close(true, Type.Missing, Type.Missing);
            excelWorkbook = null;

            // Release the Application object
            excelApp.Quit();
            excelApp = null;
        }

        private void BtnHRDExport_Click(object sender, EventArgs e)
        {
            try
            {
                try
                {
                    string extension = "";
                    string destinationPath = "";

                    MemoEntryProperty Property = new MemoEntryProperty();

                    DataTable DtInvDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);
                    if (DtInvDetail == null)
                    {
                        return;
                    }
                    if (DtInvDetail.Rows.Count <= 0)
                    {
                        Global.Message("Please Select AtLeast One Record From The List.");
                        return;
                    }

                    // Check if LABNAME values in DTabStcokGrid are not the same
                    if (DtInvDetail.Rows.Count > 0)
                    {
                        // Get the first LABNAME value to compare with others
                        string firstLabName = "HRD";

                        foreach (DataRow row in DtInvDetail.Rows)
                        {
                            // Compare each LABNAME value with the first one
                            if (!row["LABNAME"].ToString().Trim().Equals(firstLabName, StringComparison.OrdinalIgnoreCase))
                            {
                                // Show error message if LABNAME values are different
                                Global.Message("LAB Are Not Same In The Selected Stone List , Please Check");
                                return;
                            }
                        }
                    }
                    string strStoneNo = "";

                    DtInvDetail.DefaultView.Sort = "SEQUENCE";
                    if (DtInvDetail.Rows.Count > 0)
                    {
                        var list = DtInvDetail.AsEnumerable().Select(r => r["PARTYSTOCKNO"].ToString());
                        strStoneNo = string.Join(",", list);
                    }

                    DataTable DTabDetailExcel = ObjStock.GetSingleStoneManualPredictionDetailData(strStoneNo);

                    DataTable DTab = DTabDetailExcel;

                    if (DTab.Rows.Count == 0)
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("No Data Found For Excel Export");
                        return;
                        this.Cursor = Cursors.Default;
                    }

                    double StrCostAmtFE = 0;

                    DataTable DtabNew = new DataTable();

                    DtabNew.Columns.Add("SRNO");
                    DtabNew.Columns.Add("[LOT NO]");
                    DtabNew.Columns.Add("[DIAMOND SHAPE]");
                    DtabNew.Columns.Add("[DIA CTS. WT.]");
                    DtabNew.Columns.Add("[DIA COLOR]");
                    DtabNew.Columns.Add("[DIA CLARITY]");
                    DtabNew.Columns.Add("[DIA CUT]");
                    DtabNew.Columns.Add("[DIAMETER mm]");
                    DtabNew.Columns.Add("[HEIGHT mm]");
                    DtabNew.Columns.Add("[WIDTH mm]");
                    DtabNew.Columns.Add("[RATE/CT IN US$]");
                    DtabNew.Columns.Add("[DIA VALUE IN US$]");
                    DtabNew.Columns.Add("Service");
                    DtabNew.Columns.Add("[Removal of Laser]");
                    DtabNew.Columns.Add("[Laser inscription]");
                    DtabNew.Columns.Add("Sealing");

                    for (int i = 0; i < DTab.Rows.Count; i++)
                    {
                        DtabNew.Rows.Add();

                        DtabNew.Rows[i]["SRNO"] = DTab.Rows[i]["SRNO"];
                        DtabNew.Rows[i]["[LOT NO]"] = DTab.Rows[i]["LOT NO"];
                        DtabNew.Rows[i]["[DIAMOND SHAPE]"] = DTab.Rows[i]["DIAMOND SHAPE"];
                        DtabNew.Rows[i]["[DIA CTS. WT.]"] = DTab.Rows[i]["DIA CTS. WT."];
                        DtabNew.Rows[i]["[DIA COLOR]"] = DTab.Rows[i]["DIA COLOR"];
                        DtabNew.Rows[i]["[DIA CLARITY]"] = DTab.Rows[i]["DIA CLARITY"];
                        DtabNew.Rows[i]["[DIA CUT]"] = DTab.Rows[i]["DIA CUT"];
                        DtabNew.Rows[i]["[DIAMETER mm]"] = DTab.Rows[i]["DIAMETER mm"];
                        DtabNew.Rows[i]["[HEIGHT mm]"] = DTab.Rows[i]["HEIGHT mm"];
                        DtabNew.Rows[i]["[WIDTH mm]"] = DTab.Rows[i]["WIDTH mm"];
                        DtabNew.Rows[i]["[RATE/CT IN US$]"] = DTab.Rows[i]["RATE/CT IN US$"];
                        DtabNew.Rows[i]["[DIA VALUE IN US$]"] = DTab.Rows[i]["DIA VALUE IN US$"];
                        DtabNew.Rows[i]["Service"] = DTab.Rows[i]["Service"];
                        DtabNew.Rows[i]["[Removal of Laser]"] = DTab.Rows[i]["Removal of Laser"];
                        DtabNew.Rows[i]["[Laser inscription]"] = DTab.Rows[i]["Laser inscription"];
                        DtabNew.Rows[i]["Sealing"] = DTab.Rows[i]["Sealing"];

                    }


                    string StrPartyName = "RIJIYA GEMS ( GW - 5031 BDB, BKC )";
                    string StrAddress1 = "";
                    string StrAddress2 = "";
                    string StrGSTNo = "27AABCH9887J1ZU";
                    string StrClientID = "";
                    string StrGIAGSTNo = StrGSTNo;
                    DataSet DS = new DataSet();
                    DS.Tables.Add(DtabNew);
                    string DDATE = DateTime.Now.ToString("dd-MM-yyyy");

                    ExportToExcel_HRD(DS, AppDomain.CurrentDomain.BaseDirectory + "EXCEL\\Format New.xlsx", " ", DDATE, " ", " ", StrPartyName, StrAddress1, StrAddress2, StrGSTNo, StrClientID, StrGIAGSTNo, " ", StrCostAmtFE);

                    extension = Path.GetExtension(Path.GetFileName(AppDomain.CurrentDomain.BaseDirectory + "EXCEL\\Format New.xlsx"));
                    destinationPath = AppDomain.CurrentDomain.BaseDirectory + "EXCEL\\LAbReturnStonr.xlsx";
                    destinationPath = destinationPath.Replace(extension, ".pdf");
                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }
                    File.Copy(AppDomain.CurrentDomain.BaseDirectory + "EXCEL\\Format New.xlsx", destinationPath);

                    Global.Message("Excel File And PDF File Generated Sucessfully !");
                    if (Global.Confirm("Do You Want To Open File ? ") == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "EXCEL\\Format New.xlsx");
                    }

                }
                catch (Exception ex)
                {
                    Global.Message(ex.Message.ToString());
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
        public string ExportExcelWithIGIStock(DataSet DS, string PStrFilePath) //Add Khushbu 12-07-21
        {
            try
            {

                DataTable DTabDetail = DS.Tables[0];

                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }

                this.Cursor = Cursors.WaitCursor;

                string StrFilePath = PStrFilePath;

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.Yellow;
                Color FontColor = Color.Black;
                string FontName = "Calibri";
                float FontSize = 11;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("IGI Stock List");

                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    #region Stock Detail

                    EndRow = StartRow + DTabDetail.Rows.Count;

                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].LoadFromDataTable(DTabDetail, true);
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Name = FontName;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Size = FontSize;

                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Font.Bold = true;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(FontColor);

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    #endregion

                    worksheet.Cells[1, 1, 100, 100].AutoFitColumns();


                    xlPackage.Save();
                }

                this.Cursor = Cursors.Default;
                return StrFilePath;

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
            return "";
        }

        private void BtnIGIHphtExport_Click(object sender, EventArgs e)
        {
            try
            {
                string extension = "";
                string destinationPath = "";

                DataTable DtInvDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);
                if (DtInvDetail == null)
                {
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                if (DtInvDetail.Rows.Count <= 0)
                {
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                string strStoneNo = "";

                DtInvDetail.DefaultView.Sort = "SEQUENCE";
                if (DtInvDetail.Rows.Count > 0)
                {
                    var list = DtInvDetail.AsEnumerable().Select(r => r["PARTYSTOCKNO"].ToString());
                    strStoneNo = string.Join(",", list);
                }

                DataTable DTab = ObjRap.GetDataForIGI(strStoneNo);
                if (DTab.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("No Data Found For Export");
                    return;
                    this.Cursor = Cursors.Default;
                }
                DataSet DSIGI = new DataSet();
                DSIGI.Tables.Add(DTab);

                ExportExcelWithIGIStock(DSIGI, AppDomain.CurrentDomain.BaseDirectory + "EXCEL\\IGI Format.xlsx");

                extension = Path.GetExtension(Path.GetFileName(AppDomain.CurrentDomain.BaseDirectory + "EXCEL\\IGI Format.xlsx"));
                strStoneNo = string.Empty;
                Global.Message("Excel File And PDF File Generated Sucessfully !");
                if (Global.Confirm("Do You Want To Open File ? ") == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(AppDomain.CurrentDomain.BaseDirectory + "EXCEL\\IGI Format.xlsx");
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
    }
}