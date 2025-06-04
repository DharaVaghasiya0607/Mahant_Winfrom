using BusLib.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusLib.Configuration;
using BusLib.TableName;
using System.Data.SqlClient;
using MahantExport;
using System.IO;
using System.Text.RegularExpressions;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.Utils.Drawing;
using DevExpress.Data;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
using DevExpress.XtraGrid.Columns;
using MahantExport.Utility;
using BusLib.Transaction;

namespace MahantExport.Parcel
{
    public partial class FrmCoupleStoneUpload : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUploadParcel ObjStock = new BOTRN_StockUploadParcel();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        BODevGridSelection ObjGridSelection;
        DataTable DtabExcelData = new DataTable();
        DataTable  DtabUpload = new DataTable();

        public FrmCoupleStoneUpload()
        {
            InitializeComponent();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = false;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjStock);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            GrdDet.Columns["DELETE"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
            this.Show();

        }
        public void ShowForm(int pStrBoxId, string pStrBoxNo)
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            this.Show();

            txtBoxNameSearch.Text = pStrBoxNo;
            txtBoxNameSearch.Tag = pStrBoxId;

            BtnSearch_Click(null, null);
        }
     
        public void Clear()
        {                      
            txtFileName.Text = string.Empty;
            CmbSheetname.SelectedIndex = -1;
        }

        private bool ValSave()
        {
            if (Val.ToString(txtFileName.Text).Trim().Equals(string.Empty))
            {
                Global.Message(" File Name Is Required !");
                BtnBrowse.Focus();
                return true;
            }               
            else if (Val.ToString(CmbSheetname.Text).Trim().Equals(string.Empty))
            {
                Global.Message("Please Select Sheet !");
                CmbSheetname.Focus();
                return true;
            }
            return false;
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

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("Couple Stone", GrdDet);
        }
        
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                GrdDet.Columns["DELETE"].Visible = true;
               DataTable DTabCoupleStone = ObjStock.CoupleStoneGetData(Val.ToInt32(txtBoxNameSearch.Tag));
               GrdDet.Columns["DELETE"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Right;
                MainGrid.DataSource = DTabCoupleStone;
                MainGrid.Refresh();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void GrdDet_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
           
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
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
                System.Diagnostics.Process.Start(Application.StartupPath + "\\Format\\CoupleStoneUpload_Format.xlsx", "CMD");
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
            }
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            try
            {
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
                    string StrStockCol = "";
                   

                    var rows = from row in DtabExcelData.AsEnumerable()
                               where row.Field<string>("StockID") == null
                               select row;
                    foreach (DataRow dr in rows)
                    {
                        DtabExcelData.Rows.Remove(dr);
                    }

                    foreach (DataRow DRow in DtabExcelData.Rows)
                    {
                        if (Val.ToString(DRow["StockId"]) == "")
                        {
                            StrStockCol =  StrStockCol + "," + Val.ToString(DtabExcelData.Columns["Mfg_Id"]);
                        }                       
                    }
                    if (StrStockCol != "")
                    {
                        Global.Message(StrStockCol + "-->>' Value Are Required In Excel Sheet");
                        this.Cursor = Cursors.Default;
                        return;
                    }

                    if (Global.Confirm("Are You Sure Want To Save This Entry ?") == System.Windows.Forms.DialogResult.Yes)
                    {
                        PurchaseProperty Property = new PurchaseProperty();
                        DtabExcelData.TableName = "Table";
                        string CoupleStoneUploadXml;
                        using (StringWriter sw = new StringWriter())
                        {
                            DtabExcelData.WriteXml(sw);
                            CoupleStoneUploadXml = sw.ToString();
                        }

                        Property = ObjStock.CoupleStoneSave(CoupleStoneUploadXml, Property);

                        Global.Message(Property.ReturnMessageDesc);

                        if (Property.ReturnMessageType == "SUCCESS")
                        {
                            Clear();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtBoxNameSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "BOX_ID,BOXNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_BOXNAME);
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBoxNameSearch.Text = Val.ToString(FrmSearch.DRow["BOXNAME"]);
                        txtBoxNameSearch.Tag = Val.ToString(FrmSearch.DRow["BOX_ID"]);
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

        private void RepBtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle >= 0)
                {
                      
                    if (Global.Confirm("ARE YOU SURE YOU WANT TO DELETE THIS ENTRY ?") == System.Windows.Forms.DialogResult.Yes)
                    {
                        
                        CoupleStoneProperty Property = new CoupleStoneProperty();
                        DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                        Property.BOX_ID = Val.ToInt32(Val.ToString(Drow["BOX_ID"]));
                        Property.MFG_ID = Val.ToString(Val.ToString(Drow["MFG_ID"]));
                        Property = ObjStock.CoupleStone_Delete(Property);
                        


                        Global.Message(Property.ReturnMessageDesc);
                        if (Property.ReturnMessageType == "SUCCESS")
                        {
                            BtnSearch_Click(sender, e);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
        
    }
}