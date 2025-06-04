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

namespace MahantExport.Parcel
{
    public partial class FrmMixPriceChart : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_MixClarityPricing ObjStock = new BOTRN_MixClarityPricing();
        DataTable DtabExcelData = new DataTable();
        DataTable DTabPriceData = new DataTable();
        string StrUploadFilename = "";

        #region Property Settings

        public FrmMixPriceChart()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            Clear();
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

        public void Clear()
        {
            lblMessage.Text = "Message";
            lblMessage.Visible = false;
            BtnCalculate.Enabled = true;

            DTabPriceData.Rows.Clear();

            txtShape.Text = string.Empty;
            txtShape.Text = string.Empty;

            txtPriceID.Text = string.Empty;
            txtPriceID.Text = string.Empty;

            txtDepartment.Text = string.Empty;
            txtDepartment.Tag = string.Empty;

            txtPriceID.Focus();
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
                //CmbRapeDate.Items.Clear(); //ADD:KULDEEP[24/05/18]
                //foreach (DataRow row in dt.Rows)
                //{
                //    string sheetName = (string)row["TABLE_NAME"];
                //    sheets.Add(sheetName);
                //    CmbRapeDate.Items.Add(sheetName);
                //}

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
                if (txtFileName.Text.Length == 0)
                {
                    Global.MessageError("File Name Is Required");
                    txtFileName.Focus();
                    return;
                }

                if (txtPriceID.Text.Length == 0)
                {
                    Global.MessageError("Price Head ID Is Required");
                    txtPriceID.Focus();
                    return;
                }


                if (txtDepartment.Text.Length == 0)
                {
                    Global.MessageError("Department Is Required");
                    txtDepartment.Focus();
                    return;
                }

                if (Global.Confirm("Are You Sure You Want To Upload Data By Price") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                DataTable DTabFile = new DataTable();
                DTabFile.Columns.Add(new DataColumn("PRICE_ID", typeof(Int32)));
                DTabFile.Columns.Add(new DataColumn("SHAPE", typeof(string)));
                DTabFile.Columns.Add(new DataColumn("SIZE", typeof(string)));
                DTabFile.Columns.Add(new DataColumn("CLARITY", typeof(string)));
                DTabFile.Columns.Add(new DataColumn("RATE", typeof(double)));
                
                DataTable DtabExcelData = Global.GetDataTableFromExcel(txtFileName.Text);

                foreach (DataRow DRow in DtabExcelData.Rows)
                {
                    if (Val.ToString(DRow["Shape"]).Length == 0)
                    {
                        continue;
                    }
                    if (Val.ToString(DRow["Clarity"]).Length == 0)
                    {
                        continue;
                    }

                    string StrShape = "";
                    string StrClarity = "";
                    foreach (DataColumn DCol in DtabExcelData.Columns)
                    {
                        if (DCol.ColumnName.ToString().ToUpper() == "SRNO")
                        {
                            continue;
                        }
                        else if (DCol.ColumnName.ToString().ToUpper() == "SHAPE")
                        {
                            StrShape = Val.ToString(DRow[DCol.ColumnName]);
                        }
                        else if (DCol.ColumnName.ToString().ToUpper() == "CLARITY")
                        {
                            StrClarity = Val.ToString(DRow[DCol.ColumnName]);
                        }
                        else
                        {
                            DataRow DRNew = DTabFile.NewRow();
                            DRNew["PRICE_ID"] = Val.ToInt(txtPriceID.Tag);
                            DRNew["SHAPE"] = StrShape;
                            DRNew["CLARITY"] = StrClarity;
                            DRNew["SIZE"] = DCol.ColumnName;
                            DRNew["RATE"] = Val.Val(DRow[DCol.ColumnName]);
                            DTabFile.Rows.Add(DRNew);
                        }
                    }
                }
                DTabFile.AcceptChanges();
                DTabFile.TableName = "Table";

                string StockUploadXML;
                using (StringWriter sw = new StringWriter())
                {
                    DTabFile.WriteXml(sw);
                    StockUploadXML = sw.ToString();
                }

                DataTable DTabResult = ObjStock.SavePriceListUsingDataTable(StockUploadXML,Val.ToInt(txtDepartment.Tag));

                this.Cursor = Cursors.Default;

                if (DTabResult.Rows.Count > 0)
                {
                    Global.Message("Data Is Saved Successfully But Found Some Invalid Master Data ");


                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "SHAPE,CLARITY,SIZE,RATE,REMARK";
                    FrmSearch.mStrSearchText = "";
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_SHAPE);
                    FrmSearch.mStrColumnsToHide = "";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    if (FrmSearch.DRow != null)
                    {
                       
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
                else
                {
                    Global.Message("Data Is Saved Successfully");
                }


                DTabResult.Dispose();
                DTabResult = null;

                DTabFile.Dispose();
                DTabFile = null;

                DtabExcelData.Dispose();
                DtabExcelData = null;

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
                this.Cursor = Cursors.Default;
            }
        }

        private void lblSampleExcelFile_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.StartupPath + "\\Format\\ParcelPriceUpload.xlsx", "CMD");
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtPriceID.Text.Trim().Length == 0)
                {
                    Global.Message("Price Date Is Required");
                    txtPriceID.Focus();
                    return;
                }
                if (txtDepartment.Text.Trim().Length == 0)
                {
                    Global.Message("Department Is Required");
                    txtDepartment.Focus();
                    return;
                }
                if (txtShape.Text.Trim().Length == 0)
                {
                    Global.Message("Shape Is Required");
                    txtShape.Focus();
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                GrdDetStock.BeginUpdate();

                DTabPriceData = ObjStock.GetParcelPriceData(Val.ToInt(txtPriceID.Tag), Val.ToInt(txtShape.Tag), Val.ToInt(txtDepartment.Tag));
                MainGrdStock.DataSource = DTabPriceData;
                MainGrdStock.Refresh();
                GrdDetStock.PopulateColumns();

                DTabPriceData.DefaultView.Sort = "SRNO";

                GrdDetStock.Columns["SHAPE_ID"].Visible = false;
                GrdDetStock.Columns["CLARITY_ID"].Visible = false;
                GrdDetStock.Columns["DEPARTMENT_ID"].Visible = false;
                GrdDetStock.Columns["PRICE_ID"].Visible = false;

                GrdDetStock.Columns["SRNO"].OptionsColumn.AllowEdit = false;
                GrdDetStock.Columns["CLARITYNAME"].OptionsColumn.AllowEdit = false;

                GrdDetStock.Columns["SRNO"].Fixed = FixedStyle.Left;
                GrdDetStock.Columns["SHAPENAME"].Fixed = FixedStyle.Left;
                GrdDetStock.Columns["CLARITYNAME"].Fixed = FixedStyle.Left;

                foreach (DataColumn Col in DTabPriceData.Columns)
                {
                    if (Col.ColumnName.ToUpper() == "CLARITY_ID" || Col.ColumnName.ToUpper() == "SHAPE_ID" || Col.ColumnName.ToUpper() == "DEPARTMENT_ID" || Col.ColumnName.ToUpper() == "PRICE_ID")
                    {
                        continue;
                    }
                    else if (Col.ColumnName.ToUpper() == "SRNO")
                    {
                        GrdDetStock.Columns[Col.ColumnName].Caption = "SrNo";
                    }
                    else if (Col.ColumnName.ToUpper() == "SHAPENAME")
                    {
                        GrdDetStock.Columns[Col.ColumnName].Caption = "Shape";
                    }
                    else if (Col.ColumnName.ToUpper() == "CLARITYNAME")
                    {
                        GrdDetStock.Columns[Col.ColumnName].Caption = "Clarity";
                    }
                    
                    else
                    {
                        GrdDetStock.Columns[Col.ColumnName].Caption = Val.ToString(Col.ColumnName).Split('~')[1];
                    }
                    GrdDetStock.Columns[Col.ColumnName].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                    GrdDetStock.Columns[Col.ColumnName].OptionsFilter.FilterPopupMode = FilterPopupMode.CheckedList;
                    GrdDetStock.Columns[Col.ColumnName].Width = 70;
                }
                GrdDetStock.EndUpdate();

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("ParcelPrice_" + txtPriceID.Text.Replace("/", "_"), GrdDetStock);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }


        private void GrdDetStock_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.RowHandle < 0 )
            {
                return;
            }

            GrdDetStock.PostEditor();
            DataRow DRow = GrdDetStock.GetDataRow(e.RowHandle);

            ParcelPriceChartProperty Property = new ParcelPriceChartProperty();

            Property.PRICE_ID = Val.ToInt(txtPriceID.Tag);
            Property.PRICEDATE = Val.SqlDate(txtPriceID.Text);
            Property.SHAPE_ID = Val.ToInt(txtShape.Tag);
            Property.DEPARTMENT_ID = Val.ToInt(txtDepartment.Tag);
            Property.MIXSIZE_ID = Val.ToInt( e.Column.FieldName.Split('~')[0]);
            Property.MIXCLARITY_ID = Val.ToInt(DRow["CLARITY_ID"]);
            Property.RATE = Val.Val(e.Value.ToString());

            ObjStock.SaveSingleData(Property);

            DTabPriceData.AcceptChanges();

        }

        private void txtPriceID_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PRICEDATE,REMARK";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb( BOComboFill.SELTABLE.MST_PRICEHEAD);
                    FrmSearch.mStrColumnsToHide = "PRICE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtPriceID.Tag = Val.ToString(FrmSearch.DRow["PRICE_ID"]);
                        txtPriceID.Text = Val.ToString(FrmSearch.DRow["PRICEDATE"]);
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

        private void txtShape_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "SHAPECODE,SHAPENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_SHAPE);
                    FrmSearch.mStrColumnsToHide = "PARA_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtShape.Tag = Val.ToString(FrmSearch.DRow["SHAPE_ID"]);
                        txtShape.Text = Val.ToString(FrmSearch.DRow["SHAPENAME"]);
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

        private void txtDepartment_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "DEPARTMENTNAME,DEPARTMENTCODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_DEPARTMENT);
                    FrmSearch.mStrColumnsToHide = "DEPARTMENT_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtDepartment.Tag = Val.ToString(FrmSearch.DRow["DEPARTMENT_ID"]);
                        txtDepartment.Text = Val.ToString(FrmSearch.DRow["DEPARTMENTNAME"]);
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

        private void GrdDetStock_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
        {
            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000")
            {
                e.DisplayText = String.Empty;
            }
        }

    }
}
