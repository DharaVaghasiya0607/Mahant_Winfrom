using BusLib.Configuration;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MahantExport.Stock
{
    public partial class FrmPolishOpeningStock : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        BOFormPer ObjPer = new BOFormPer();
        DataTable DtabExcelData = new DataTable();
        DataTable dtExist = new DataTable();

        #region Property
        public FrmPolishOpeningStock()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            try
            {
                Val.FormGeneralSetting(this);
                AttachFormDefaultEvent();
                ObjPer.GetFormPermission(Val.ToString(this.Tag));
                if (ObjPer.ISVIEW == false)
                {
                    Global.MessageError(BusLib.TPV.BOMessage.ViewDeniedMsg);
                    return;
                }

                Clear();

                string Str = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdDetStock.Name);

                if (Str != "")
                {
                    byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                    MemoryStream stream = new MemoryStream(byteArray);
                    GrdDetStock.RestoreLayoutFromStream(stream);
                }
                this.Show();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
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
            if (Val.ToString(txtFileName.Text).Trim().Equals(string.Empty))
            {
                Global.Message(" File Name Is Required !");
                BtnBrowse.Focus();
                return true;
            }
           else if (Val.ToString(DTUploadDate.Text).Trim().Equals(string.Empty))
            {
                Global.Message(" Upload Date Is Required !");
                DTUploadDate.Focus();
                return true;
            }
            else if (Val.ToString(CmbSheetname.Text).Trim().Equals(string.Empty))
            {
                Global.Message("Please Select Sheet.");
                CmbSheetname.Focus();
                return true;
            }
            return false;
        }

        private bool PriceValidation()
        {
            DataTable dtData = (DataTable)MainGrdStock.DataSource;
            string StrEntryPrice = "", StrCostPrice = "", StrSalePrice = "", StrQuotePrice = "";
            for (int i = 0; i < dtData.Rows.Count; i++)
            {
                if (Val.ToString(dtData.Rows[i]["ENTRYPRICE"]) == "")
                {
                    StrEntryPrice = StrEntryPrice + ',' + Val.ToString(dtData.Rows[i]["STOCK_ID"]);
                }
                if (Val.ToString(dtData.Rows[i]["COSTPRICE"]) == "")
                {
                    StrCostPrice = StrCostPrice + ',' + Val.ToString(dtData.Rows[i]["STOCK_ID"]);
                }
                if (Val.ToString(dtData.Rows[i]["SALEPRICE"]) == "")
                {
                    StrSalePrice = StrSalePrice + ',' + Val.ToString(dtData.Rows[i]["STOCK_ID"]);
                }
                if (Val.ToString(dtData.Rows[i]["QUOTEPRICE"]) == "")
                {
                    StrQuotePrice = StrQuotePrice + ',' + Val.ToString(dtData.Rows[i]["STOCK_ID"]);
                }
            }
            if (StrEntryPrice != "")
            {
                Global.Message("Stock ID -->>" + StrEntryPrice + "-- " + "Entry Price Values Are Required In Excel Sheet");
                return true;
            }
            if (StrCostPrice != "")
            {
                Global.Message("Stock ID -->>" + StrCostPrice + "-- " + "Cost Price Values Are Required In Excel Sheet");
                return true;
            }
            if (StrSalePrice != "")
            {
                Global.Message("Stock ID -->>" + StrSalePrice + "-- " + "Sale Price Values Are Required In Excel Sheet");
                return true;
            }
            if (StrQuotePrice != "")
            {
                Global.Message("Stock ID -->>" + StrQuotePrice + "-- " + "Quote Price Values Are Required In Excel Sheet");
                return true;
            }

            string StrStockID = "";
            var duplicates = from row in dtData.AsEnumerable()
                             group row by new { KapanName = row["STOCK_ID"] } into grp
                             where grp.Count() > 1
                             select grp.Key;
            var duplicateRows = dtData.AsEnumerable().Where(row => duplicates.Contains(new { KapanName = row["STOCK_ID"] }));

            DataTable DTduplicate = duplicateRows.Any() ? duplicateRows.CopyToDataTable() : dtData.Clone();
            if(DTduplicate.Rows.Count > 0)
            {
                for (int i = 0; i < DTduplicate.Rows.Count; i++)
                {
                    StrStockID = StrStockID + " , " + Val.ToString(dtData.Rows[i]["STOCK_ID"]); 
                }                
            }
            if(StrStockID != "")
            {
                Global.Message("Stock ID -->>" + "[ " + StrStockID +" ]"+ " Has Duplicate Value In Excel Sheet");
                return true;
            }

            return false;
        }
        #endregion

        #region ExcelSheet Class
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

        private String[] GetExcelSheetNames(string excelFile)
        {
            OleDbConnection objConn = null;
            System.Data.DataTable dt = null;

            try
            {
                String connString = "";
                if (Path.GetExtension(excelFile).Equals(".xls"))//for 97-03 Excel file
                {
                    connString = "Provider=Microsoft.ACE.OLEDB.4.0;" +
                      "Data Source=" + excelFile + ";Extended Properties=Excel 8.0;";
                }
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
                CmbSheetname.Items.Clear();
                foreach (DataRow row in dt.Rows)
                {
                    string sheetName = (string)row["TABLE_NAME"];
                    sheets.Add(sheetName);
                    CmbSheetname.Items.Add(sheetName);
                }

                return excelSheets;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
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

        #endregion

        #region Button Event

        public void Clear()
        {
            DtabExcelData.Rows.Clear();
            dtExist.Rows.Clear();
            MainGrdStock.DataSource = dtExist;
            txtFileName.Text = string.Empty;
            DTUploadDate.Text = string.Empty;
            CmbSheetname.SelectedIndex = -1;
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                progressPanel1.Visible = true;
                if (ValSave())
                {
                    return;
                }

                if (Path.GetExtension(txtFileName.Text.ToString()).ToUpper().Contains("XLSX") || Path.GetExtension(txtFileName.Text.ToString()).ToUpper().Contains("XLS"))
                {
                    string extension = Path.GetExtension(txtFileName.Text.ToString());
                    string destinationPath = Application.StartupPath + @"\StoneFiles\" + Path.GetFileName(txtFileName.Text);
                    destinationPath = destinationPath.Replace(extension, ".xlsx");
                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }
                    File.Copy(txtFileName.Text, destinationPath);

                    DtabExcelData = Global.ImportExcelXLSWithSheetName(destinationPath, true, CmbSheetname.SelectedItem.ToString());//Gunjan:07/07/2023

                    // DtabExcelData = GetDataTableFromExcel(destinationPath, true);//Comment By Gunjan :07/07/2023

                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }

                   
                    DtabExcelData.TableName = "Table";
                    string OpeningFileUploadXml;
                    using (StringWriter sw = new StringWriter())
                    {
                        DtabExcelData.WriteXml(sw);
                        OpeningFileUploadXml = sw.ToString();
                    }
                    dtExist = ObjStock.CheckExistsOrNotExists(OpeningFileUploadXml);

                    string StrEntryPrice = "", StrCostPrice = "", StrSalePrice = "", StrQuotePrice = "";
                    for (int i = 0; i < dtExist.Rows.Count; i++)
                    {
                        if (Val.ToString(dtExist.Rows[i]["ENTRYPRICE"]) == "")
                        {
                            StrEntryPrice = StrEntryPrice + ',' + Val.ToString(dtExist.Rows[i]["STOCK_ID"]);
                        }
                        if (Val.ToString(dtExist.Rows[i]["COSTPRICE"]) == "")
                        {
                            StrCostPrice = StrCostPrice + ',' + Val.ToString(dtExist.Rows[i]["STOCK_ID"]);
                        }
                        if (Val.ToString(dtExist.Rows[i]["SALEPRICE"]) == "")
                        {
                            StrSalePrice = StrSalePrice + ',' + Val.ToString(dtExist.Rows[i]["STOCK_ID"]);
                        }
                        if (Val.ToString(dtExist.Rows[i]["QUOTEPRICE"]) == "")
                        {
                            StrQuotePrice = StrQuotePrice + ',' + Val.ToString(dtExist.Rows[i]["STOCK_ID"]);
                        }
                    }
                    if (StrEntryPrice != "")
                    {
                        Global.Message("Stock ID -->>" + StrEntryPrice + "-- " + "Entry Price Values Are Required In Excel Sheet");
                        return ;
                    }
                    if (StrCostPrice != "")
                    {
                        Global.Message("Stock ID -->>" + StrCostPrice + "-- " + "Cost Price Values Are Required In Excel Sheet");
                        return ;
                    }
                    if (StrSalePrice != "")
                    {
                        Global.Message("Stock ID -->>" + StrSalePrice + "-- " + "Sale Price Values Are Required In Excel Sheet");
                        return;
                    }
                    if (StrQuotePrice != "")
                    {
                        Global.Message("Stock ID -->>" + StrQuotePrice + "-- " + "Quote Price Values Are Required In Excel Sheet");
                        return;
                    }

                    string StrStockID = "";
                    var duplicates = from row in dtExist.AsEnumerable()
                                     group row by new { KapanName = row["STOCK_ID"] } into grp
                                     where grp.Count() > 1
                                     select grp.Key;
                    var duplicateRows = dtExist.AsEnumerable().Where(row => duplicates.Contains(new { KapanName = row["STOCK_ID"] }));

                    DataTable DTduplicateID = duplicateRows.Any() ? duplicateRows.CopyToDataTable() : dtExist.Clone();
                    if (DTduplicateID.Rows.Count > 0)
                    {
                        for (int i = 0; i < DTduplicateID.Rows.Count; i++)
                        {
                            StrStockID = StrStockID + " , " + Val.ToString(dtExist.Rows[i]["STOCK_ID"]);
                        }
                    }
                    if (StrStockID != "")
                    {
                        Global.Message("Stock ID -->>" + "[ " + StrStockID + " ]" + " Has Duplicate Value In Excel Sheet");
                        return;
                    }

                    DataTable DTduplicate = dtExist.Clone();
                    foreach (DataRow dr in dtExist.Rows)
                    {
                        if (Val.ToString(dr["ErrorMessage"]) == "Not Exists")
                        {
                            DTduplicate.Rows.Add(dr.ItemArray);
                        }
                    }

                    if (DTduplicate.Rows.Count > 0)
                     {
                            FrmPopupGrid FrmPopupGrid = new FrmPopupGrid();
                            FrmPopupGrid.MainGrid.DataSource = DTduplicate;
                            FrmPopupGrid.GrdDet.BestFitColumns();
                            FrmPopupGrid.MainGrid.Dock = DockStyle.Fill;
                            FrmPopupGrid.Text = "Opening Stock Record Not Exists";
                            FrmPopupGrid.LblTitle.Text = "List Of Opening Stock Record Which Are Not Exist in Box Master";
                            FrmPopupGrid.ISPostBack = true;

                            FrmPopupGrid.Width = 1000;
                            FrmPopupGrid.GrdDet.OptionsBehavior.Editable = false;

                            FrmPopupGrid.GrdDet.Columns["ASSORTMENT_ID"].Visible = false;
                            FrmPopupGrid.GrdDet.Columns["LOT_ID"].Visible = false;
                            FrmPopupGrid.GrdDet.Columns["SIZE_ID"].Visible = false;
                            FrmPopupGrid.GrdDet.Columns["COLOR_ID"].Visible = false;
                            FrmPopupGrid.GrdDet.Columns["COLORSHADE_ID"].Visible = false;
                            FrmPopupGrid.GrdDet.Columns["PURITY_ID"].Visible = false;
                            FrmPopupGrid.GrdDet.Columns["FL_ID"].Visible = false;
                            FrmPopupGrid.GrdDet.Columns["SHAPE_ID"].Visible = false;
                            FrmPopupGrid.GrdDet.Columns["SHAPE1_ID"].Visible = false;
                            FrmPopupGrid.GrdDet.Columns["COLOR1_ID"].Visible = false;
                            FrmPopupGrid.GrdDet.Columns["CLARITY1_ID"].Visible = false;
                            FrmPopupGrid.GrdDet.Columns["FLR1_ID"].Visible = false;
                            FrmPopupGrid.GrdDet.Columns["SHAPE2_ID"].Visible = false;
                            FrmPopupGrid.GrdDet.Columns["COLOR2_ID"].Visible = false;
                            FrmPopupGrid.GrdDet.Columns["CLARITY2_ID"].Visible = false;
                            FrmPopupGrid.GrdDet.Columns["FLR2_ID"].Visible = false;

                            FrmPopupGrid.ShowDialog();
                            FrmPopupGrid.Hide();
                            FrmPopupGrid.Dispose();
                            FrmPopupGrid = null;
                            this.Cursor = Cursors.Default;                       
                    }
                  
                    MainGrdStock.DataSource = dtExist;                   
                    GrdDetStock.BestFitColumns();
                    progressPanel1.Visible = false;
                }
            }
            catch (Exception ex)
            {   
                Global.Message(ex.Message);
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
                    txtFileName.Text = OpenFileDialog.FileName;

                    string extension = Path.GetExtension(txtFileName.Text.ToString());
                    string destinationPath = Application.StartupPath + @"\StoneFiles\" + Path.GetFileName(txtFileName.Text);
                    destinationPath = destinationPath.Replace(extension, ".xlsx");
                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }
                    File.Copy(txtFileName.Text, destinationPath);

                    GetExcelSheetNames(destinationPath);
                    CmbSheetname.SelectedIndex = 0;

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

        private void lblSampleExcelFile_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Application.StartupPath + "\\Format\\OpeningStock_Format.xlsx", "CMD");
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValSave())
                {
                    return;
                }

                if (PriceValidation())
                {
                    return;
                }
                DataTable dtData = (DataTable)MainGrdStock.DataSource;               

                string StrLotName = ""; string ErrorMessage = "";string StrExists = "";
                for (int i = 0; i < dtData.Rows.Count; i++)
                {
                    ErrorMessage = Val.ToString(dtData.Rows[i]["ErrorMessage"]);

                    if (ErrorMessage == "Not Exists" || ErrorMessage == "Exists")
                    {
                        StrExists = "";
                    }
                    else
                    {
                        StrLotName = StrLotName + ',' + Val.ToString(dtData.Rows[i]["LOTNO"]);
                    }
                }

                if(StrLotName != "")
                {
                    Global.Message("Please enter a valid value in the column of Error Message column list!");
                    return;
                }

                if (Global.Confirm("Are You Sure Want To Save This Entry ?") == System.Windows.Forms.DialogResult.Yes)
                {
                    PurchaseProperty Property = new PurchaseProperty();
                    dtData.TableName = "Table";
                    string OpeningStockSaveXml;
                    using (StringWriter sw = new StringWriter())
                    {
                        dtData.WriteXml(sw);
                        OpeningStockSaveXml = sw.ToString();
                    }

                    Property = ObjStock.OpeningStockSave(OpeningStockSaveXml, Val.ToString(DTUploadDate.Text), Property);

                    Global.Message(Property.ReturnMessageDesc);

                    if (Property.ReturnMessageType == "SUCCESS")
                    {
                        Clear();
                    }
                
                 }
            }
            catch (Exception ex)
            {

                Global.MessageError(ex.Message);
            }
        }

        #endregion

        #region Grid Event
        private void GrdDetStock_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                string ErrorMessage = "";
                GridView currentView = sender as GridView;
                ErrorMessage = Val.ToString(currentView.GetRowCellValue(e.RowHandle, "ErrorMessage")).ToUpper();
                string strFieldName = e.Column.FieldName.ToUpper();
              
                if (ErrorMessage.Contains("LOTNO") && strFieldName == "LOTNO")
                {
                    e.Appearance.BackColor = Color.LightPink;
                }
                if (ErrorMessage.Contains("SHAPE") && strFieldName == "SHAPE")
                {
                    e.Appearance.BackColor = Color.LightPink;
                }
                if (ErrorMessage.Contains("SIZE") && strFieldName == "SIZE")
                {
                    e.Appearance.BackColor = Color.LightPink;
                }               
                if (ErrorMessage.Contains("PURITY") && strFieldName == "PURITY")
                {
                    e.Appearance.BackColor = Color.LightPink;
                }
                if (ErrorMessage.Contains("FLR") && strFieldName == "FL")
                {
                    e.Appearance.BackColor = Color.LightPink;
                }
                if (ErrorMessage.Contains("COLORSHADE") && strFieldName == "COLORSHADE")
                {
                    e.Appearance.BackColor = Color.LightPink;
                }
                if (ErrorMessage.Contains("COLOR") && strFieldName == "COLOR")
                {
                    e.Appearance.BackColor = Color.LightPink;
                }
                if (ErrorMessage.Contains("SHAPE1") && strFieldName == "SHAPE1")
                {
                    e.Appearance.BackColor = Color.LightPink;
                }
                if (ErrorMessage.Contains("COLOR1") && strFieldName == "COLOR1")
                {
                    e.Appearance.BackColor = Color.LightPink;
                }
                if (ErrorMessage.Contains("CLARITY1") && strFieldName == "CLARITY1")
                {
                    e.Appearance.BackColor = Color.LightPink;
                }
                if (ErrorMessage.Contains("FLR1") && strFieldName == "FLR1")
                {
                    e.Appearance.BackColor = Color.LightPink;
                }
                if (ErrorMessage.Contains("SHAPE2") && strFieldName == "SHAPE2")
                {
                    e.Appearance.BackColor = Color.LightPink;
                }
                if (ErrorMessage.Contains("COLOR2") && strFieldName == "COLOR2")
                {
                    e.Appearance.BackColor = Color.LightPink;
                }
                if (ErrorMessage.Contains("CLARITY2") && strFieldName == "CLARITY2")
                {
                    e.Appearance.BackColor = Color.LightPink;
                }
                if (ErrorMessage.Contains("FLR2") && strFieldName == "FLR2")
                {
                    e.Appearance.BackColor = Color.LightPink;
                }
               
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        #endregion

        #region LayOut
        private void lblSaveLayout_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void lblDefaultLayout_Click(object sender, EventArgs e)
        {
            try
            {
                int IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdDetStock.Name);
                if (IntRes != -1)
                {
                    Global.Message("Layout Successfully Deleted");
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        #endregion

        private void BtnExport_Click(object sender, EventArgs e)
        {

            try
            {
                Global.ExcelExport("OpeningEntry", GrdDetStock);
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void DTUploadDate_ParseEditValue(object sender, DevExpress.XtraEditors.Controls.ConvertEditValueEventArgs e)
        {
            
        }
    }
}