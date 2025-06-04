using BusLib;
using DevExpress.XtraPrinting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using BusLib.Transaction;
using BusLib.Master;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
using OfficeOpenXml;
using MahantExport;
using MahantExport.Utility;
using System.Reflection;

namespace MahantExport.Stock
{
    public partial class FrmSingleFileUpload : DevControlLib.cDevXtraForm
    {
        public delegate void SetControlValueCallback(Control oControl, string propName, object propValue);

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_Attendance ObjMast = new BOMST_Attendance();

        DataTable DtabExcelData = new DataTable();
        DataTable DtabFinal = new DataTable();
        DataTable DtabFileUpload = new DataTable();
        DataTable DtabPara = new DataTable();
        string StrUploadFilename = "";

        DataSet DSet = new DataSet();

        BOTRN_SingleFileUpload ObjUpload = new BOTRN_SingleFileUpload();
        Guid mUpload_ID = Guid.Empty;
        Guid mGroup_ID = Guid.Empty;
        int IntCheck = 0;
        string StrMessage = "";
        Int32 IntColor_ID = 0;
        BODevGridSelection ObjGridSelection;
        string mStrGIAAction = "";
        #region Property Settings

        string mStrStockType = ""; //Added By Keyur
        string mStrParty = "";
        int mIntCurrencyID = 0;
        double mDouExcRate = 0;

        public FrmSingleFileUpload()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            BtnClear_Click(null, null);
            Fill();
            DtabPara = new BOMST_Parameter().GetParameterData();
            GrdDetail.BeginUpdate();
            if (MainGrid.RepositoryItems.Count == 0)
            {
                ObjGridSelection = new BODevGridSelection();
                ObjGridSelection.View = GrdDetail;
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            }
            else
            {
                ObjGridSelection.ClearSelection();
            }
            GrdDetail.Columns["COLSELECTCHECKBOX"].OwnerBand = BandGeneral;
            GrdDetail.Columns["COLSELECTCHECKBOX"].VisibleIndex = 0;
            GrdDetail.EndUpdate();

            CmbLabType.DataSource = new BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LAB);
            CmbLabType.DisplayMember = "LABNAME";
            CmbLabType.ValueMember = "LABNAME";
            CmbLabType.SelectedIndex = -1;
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
            ObjFormEvent.ObjToDisposeList.Add(ObjUpload);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        #endregion


        #region Enter Event

        private void ControlEnterForGujarati_Enter(object sender, EventArgs e)
        {
            Global.SelectLanguage(Global.LANGUAGE.GUJARATI);
        }
        private void ControlEnterForEnglish_Enter(object sender, EventArgs e)
        {
            Global.SelectLanguage(Global.LANGUAGE.ENGLISH);
        }


        #endregion

        #region Operation

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


        public void Clear()
        {
            this.Cursor = Cursors.WaitCursor;
            CmbSheetName.Text = string.Empty;
            DtabFileUpload.Rows.Clear();
            CmbSheetName.SelectedIndex = -1;
            CmbLabType.SelectedIndex = 0;
            DTPUploadDate.Text = Val.ToString(DateTime.Now);
            txtFileName.Text = string.Empty;
            DtabExcelData.Rows.Clear();
            CmbLabType.Focus();
            mUpload_ID = Guid.Empty;
            Fill();
            GrdDetail.OptionsBehavior.Editable = true;
            this.Cursor = Cursors.Default;
            DtabPara = new BOMST_Parameter().GetParameterData();

        }

        public void Fill()
        {
            //DtabAtd = ObjMast.Fill(StrLedgerGroup);
            //DtabAtd.Rows.Add(DtabAtd.NewRow());
            //MainGrid.DataSource = DtabAtd;
            //MainGrid.Refresh();
            DtabFileUpload = ObjUpload.GetFileUploadData(mUpload_ID, "", "", "", "", "", "");
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmLedger_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                //BtnSearch_Click(null, null);
            }
        }

        public void FetchValue(DataRow DR)
        {
            //txtParaType.Text = Val.ToString(DR["ITEMGROUP_ID"]);
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
                CmbSheetName.Items.Clear(); //ADD:KULDEEP[24/05/18]
                foreach (DataRow row in dt.Rows)
                {
                    string sheetName = (string)row["TABLE_NAME"];
                    sheets.Add(sheetName);
                    CmbSheetName.Items.Add(sheetName);
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
        #endregion

        #region Excel Export

        public string ExportExcel(string strClientRefNo)//Gunjan:14/03/2023
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                DataTable DTabDetail = ObjUpload.ExcelExportData(strClientRefNo);

                this.Cursor = Cursors.Default;


                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }

                this.Cursor = Cursors.WaitCursor;

                string StrFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);

                Color BackColor = Color.White;
                Color FontColor = Color.Black;
                string FontName = "Calibri";
                float FontSize = 11;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;



                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Result Upload");

                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;


                    #region Stock Detail
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].LoadFromDataTable(DTabDetail, true);
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheet.Cells[StartRow, StartColumn, DTabDetail.Rows.Count + 1, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, DTabDetail.Rows.Count + 1, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, DTabDetail.Rows.Count + 1, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, DTabDetail.Rows.Count + 1, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Name = FontName;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Size = FontSize;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Font.Bold = true;
                    //worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    //worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    //worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    //worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheet.Cells[StartRow, StartColumn, DTabDetail.Rows.Count + 1, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, StartColumn, DTabDetail.Rows.Count + 1, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, StartColumn, DTabDetail.Rows.Count + 1, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, StartColumn, DTabDetail.Rows.Count + 1, EndColumn].Style.Font.Color.SetColor(FontColor);

                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                    int CaratsColumn = DTabDetail.Columns["Carat"].Ordinal + 1;
                    int GIACaratColumn = DTabDetail.Columns["GIA Carat"].Ordinal + 1;
                    int RapColumn = DTabDetail.Columns["Rap"].Ordinal + 1;
                    int DiscColumn = DTabDetail.Columns["Disc"].Ordinal + 1;
                    int PerCtsColumn = DTabDetail.Columns["$/Cts"].Ordinal + 1;
                    int AmtColumn = DTabDetail.Columns["Amount"].Ordinal + 1;
                    int DepthperColumn = DTabDetail.Columns["Depth%"].Ordinal + 1;
                    int TablePerColumn = DTabDetail.Columns["Table%"].Ordinal + 1;
                    int CRAgColumn = DTabDetail.Columns["Crn Ag"].Ordinal + 1;

                    worksheet.Cells[StartRow, CaratsColumn, StartColumn, CaratsColumn].Style.Numberformat.Format = "0.000";
                    worksheet.Cells[StartRow, GIACaratColumn, StartRow, GIACaratColumn].Style.Numberformat.Format = "0.000";
                    worksheet.Cells[StartRow, RapColumn, StartRow, RapColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, DiscColumn, StartRow, DiscColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, PerCtsColumn, StartRow, PerCtsColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, AmtColumn, StartRow, AmtColumn].Style.Numberformat.Format = "0.000";
                    worksheet.Cells[StartRow, DepthperColumn, StartRow, DepthperColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, TablePerColumn, StartRow, TablePerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, CRAgColumn, StartRow, CRAgColumn].Style.Numberformat.Format = "0.00";

                    int ShapeColumn = DTabDetail.Columns["GIA Shp"].Ordinal + 1;
                    int ShapeUpdownColumn = DTabDetail.Columns["ShapeUpDown"].Ordinal + 1;
                    int ColorColumn = DTabDetail.Columns["GIA Color"].Ordinal + 1;
                    int ColorUpdownColumn = DTabDetail.Columns["ColorUpDown"].Ordinal + 1;
                    int ClarityColumn = DTabDetail.Columns["GIA Clarity"].Ordinal + 1;
                    int ClarityUpdownColumn = DTabDetail.Columns["ClarityUpDown"].Ordinal + 1;
                    int CutColumn = DTabDetail.Columns["GIA Cut"].Ordinal + 1;
                    int CutUpdownColumn = DTabDetail.Columns["CutUpDown"].Ordinal + 1;
                    int PolColumn = DTabDetail.Columns["GIA Pol"].Ordinal + 1;
                    int PolUpdownColumn = DTabDetail.Columns["PolUpDown"].Ordinal + 1;
                    int SymColumn = DTabDetail.Columns["GIA Sym"].Ordinal + 1;
                    int SymUpdownColumn = DTabDetail.Columns["SymUpDown"].Ordinal + 1;
                    int FloColumn = DTabDetail.Columns["GIA Flo"].Ordinal + 1;
                    int FloUpdownColumn = DTabDetail.Columns["FloUpDown"].Ordinal + 1;
                    int CaratColumn = DTabDetail.Columns["GIA Carat"].Ordinal + 1;
                    int CaratUpdownColumn = DTabDetail.Columns["CaratUpDown"].Ordinal + 1;

                    for (int i = 2; i <= DTabDetail.Rows.Count + 1; i++)
                    {
                        if (Val.ToString(worksheet.Cells[i, ShapeUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[i, ShapeColumn, i, ShapeColumn].Style.Fill.BackgroundColor.SetColor(lblUp.BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[i, ShapeUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[i, ShapeColumn, i, ShapeColumn].Style.Fill.BackgroundColor.SetColor(lblDown.BackColor);
                        }
                        else
                        {
                            worksheet.Cells[i, ShapeColumn, i, ShapeColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }

                        if (Val.ToString(worksheet.Cells[i, ColorUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[i, ColorColumn, i, ColorColumn].Style.Fill.BackgroundColor.SetColor(lblUp.BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[i, ColorUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[i, ColorColumn, i, ColorColumn].Style.Fill.BackgroundColor.SetColor(lblDown.BackColor);
                        }
                        else
                        {
                            worksheet.Cells[i, ColorColumn, i, ColorColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }

                        if (Val.ToString(worksheet.Cells[i, ClarityUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[i, ClarityColumn, i, ClarityColumn].Style.Fill.BackgroundColor.SetColor(lblUp.BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[i, ClarityUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[i, ClarityColumn, i, ClarityColumn].Style.Fill.BackgroundColor.SetColor(lblDown.BackColor);
                        }
                        else
                        {
                            worksheet.Cells[i, ClarityColumn, i, ClarityColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }

                        if (Val.ToString(worksheet.Cells[i, CutUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[i, CutColumn, i, CutColumn].Style.Fill.BackgroundColor.SetColor(lblUp.BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[i, CutUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[i, CutColumn, i, CutColumn].Style.Fill.BackgroundColor.SetColor(lblDown.BackColor);
                        }
                        else
                        {
                            worksheet.Cells[i, CutColumn, i, CutColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }

                        if (Val.ToString(worksheet.Cells[i, PolUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[i, PolColumn, i, PolColumn].Style.Fill.BackgroundColor.SetColor(lblUp.BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[i, PolUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[i, PolColumn, i, PolColumn].Style.Fill.BackgroundColor.SetColor(lblDown.BackColor);
                        }
                        else
                        {
                            worksheet.Cells[i, PolColumn, i, PolColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }

                        if (Val.ToString(worksheet.Cells[i, SymUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[i, SymColumn, i, SymColumn].Style.Fill.BackgroundColor.SetColor(lblUp.BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[i, SymUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[i, SymColumn, i, SymColumn].Style.Fill.BackgroundColor.SetColor(lblDown.BackColor);
                        }
                        else
                        {
                            worksheet.Cells[i, SymColumn, i, SymColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }

                        if (Val.ToString(worksheet.Cells[i, FloUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[i, FloColumn, i, FloColumn].Style.Fill.BackgroundColor.SetColor(lblUp.BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[i, FloUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[i, FloColumn, i, FloColumn].Style.Fill.BackgroundColor.SetColor(lblDown.BackColor);
                        }
                        else
                        {
                            worksheet.Cells[i, FloColumn, i, FloColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }

                        if (Val.ToString(worksheet.Cells[i, CaratUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[i, CaratColumn, i, CaratColumn].Style.Fill.BackgroundColor.SetColor(lblUp.BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[i, CaratUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[i, CaratColumn, i, CaratColumn].Style.Fill.BackgroundColor.SetColor(lblDown.BackColor);
                        }
                        else
                        {
                            worksheet.Cells[i, CaratColumn, i, CaratColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }
                    }

                    worksheet.Cells[1, 1, 100, 100].AutoFitColumns();
                    worksheet.Column(ShapeUpdownColumn).Hidden = true;
                    worksheet.Column(ColorUpdownColumn).Hidden = true;
                    worksheet.Column(ClarityUpdownColumn).Hidden = true;
                    worksheet.Column(CutUpdownColumn).Hidden = true;
                    worksheet.Column(PolUpdownColumn).Hidden = true;
                    worksheet.Column(SymUpdownColumn).Hidden = true;
                    worksheet.Column(FloUpdownColumn).Hidden = true;
                    worksheet.Column(CaratUpdownColumn).Hidden = true;


                    #endregion

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

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTabDetail = GetTableOfSelectedRows(GrdDetail, true);

                if (DTabDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                var list = DTabDetail.AsEnumerable().Select(r => r["CLIENTREFNO"].ToString());
                string StrFileName = ExportExcel(string.Join(",", list));

                if (StrFileName == "")
                {
                    Global.Message("Please Select Atleast One Packet");
                    return;
                }
                if (Global.Confirm("Do You Want To Open File ? ") == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(StrFileName, "CMD");
                }
            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
            }
            //try/////Comment By Gunjan : 14/03/2023
            //{
            //    SaveFileDialog svDialog = new SaveFileDialog();
            //    svDialog.DefaultExt = ".xlsx";
            //    svDialog.Title = "Export to Excel";
            //    svDialog.FileName = "Lab Result File";
            //    svDialog.Filter = "Excel files 97-2003 (*.xls)|*.xls|Excel files 2007(*.xlsx)|*.xlsx|All files (*.*)|*.*";

            //    if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
            //    {
            //        {
            //            PrintableComponentLinkBase link = new PrintableComponentLinkBase()
            //            {
            //                PrintingSystemBase = new PrintingSystemBase(),
            //                Component = MainGrid,
            //                Landscape = true,
            //                PaperKind = PaperKind.A4,
            //                Margins = new System.Drawing.Printing.Margins(20, 20, 200, 20)
            //            };

            //            link.CreateReportHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderAreaExcel);

            //            link.ExportToXls(svDialog.FileName);

            //            if (Global.Confirm("Do You Want To Open [LabResultFile.xlsx] ?") == System.Windows.Forms.DialogResult.Yes)
            //            {
            //                System.Diagnostics.Process.Start(svDialog.FileName, "CMD");
            //            }
            //        }
            //    }
            //    svDialog.Dispose();
            //    svDialog = null;
            //}
            //catch (Exception EX)
            //{
            //    Global.Message(EX.Message);
            //}

        }

        public void Link_CreateMarginalHeaderAreaExcel(object sender, CreateAreaEventArgs e)
        {
            // ' For Report Title

            //TextBrick BrickTitle = e.Graph.DrawString(BusLib.Configuration.BOConfiguration.gEmployeeProperty.COMPANYNAME, System.Drawing.Color.Navy, new RectangleF(0, 0, e.Graph.ClientPageSize.Width, 35), DevExpress.XtraPrinting.BorderSide.None);
            //BrickTitle.Font = new Font("verdana", 12, FontStyle.Bold);
            //BrickTitle.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            //BrickTitle.VertAlignment = DevExpress.Utils.VertAlignment.Center;



            //// ' For Group 
            //TextBrick BrickTitleseller = e.Graph.DrawString("Prediction View", System.Drawing.Color.Navy, new RectangleF(0, 35, e.Graph.ClientPageSize.Width, 35), DevExpress.XtraPrinting.BorderSide.None);
            //BrickTitleseller.Font = new Font("verdana", 10, FontStyle.Bold);
            //BrickTitleseller.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            //BrickTitleseller.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            //BrickTitleseller.ForeColor = Color.Black;

            //int IntX = Convert.ToInt32(Math.Round(e.Graph.ClientPageSize.Width - 400, 0));
            //TextBrick BrickTitledate = e.Graph.DrawString("Print Date :- " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), System.Drawing.Color.Navy, new RectangleF(IntX, 70, 400, 30), DevExpress.XtraPrinting.BorderSide.None);
            //BrickTitledate.Font = new Font("verdana", 8, FontStyle.Bold);
            //BrickTitledate.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            //BrickTitledate.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            //BrickTitledate.ForeColor = Color.Black;

        }

        public void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            // ' For Report Title

            TextBrick BrickTitle = e.Graph.DrawString("Daily Attendance Paper Of ( " + DTPUploadDate.Text + " )", System.Drawing.Color.Navy, new RectangleF(0, 0, e.Graph.ClientPageSize.Width, 20), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitle.Font = new Font("Verdana", 12, FontStyle.Bold);
            BrickTitle.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            BrickTitle.VertAlignment = DevExpress.Utils.VertAlignment.Center;

            int IntX = Convert.ToInt32(Math.Round(e.Graph.ClientPageSize.Width - 400, 0));
            TextBrick BrickTitledate = e.Graph.DrawString("Print Date :- " + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), System.Drawing.Color.Black, new RectangleF(IntX, 25, 400, 18), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitledate.Font = new Font("Verdana", 8, FontStyle.Bold);
            BrickTitledate.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            BrickTitledate.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            BrickTitledate.ForeColor = Color.Black;
        }

        public void Link_CreateMarginalFooterArea(object sender, CreateAreaEventArgs e)
        {
            int IntX = Convert.ToInt32(Math.Round(e.Graph.ClientPageSize.Width - 100, 0));

            PageInfoBrick BrickPageNo = e.Graph.DrawPageInfo(PageInfo.NumberOfTotal, "Page {0} of {1}", System.Drawing.Color.Black, new RectangleF(IntX, 0, 100, 15), DevExpress.XtraPrinting.BorderSide.None);
            BrickPageNo.LineAlignment = BrickAlignment.Far;
            BrickPageNo.Alignment = BrickAlignment.Far;
            // BrickPageNo.AutoWidth = true;
            BrickPageNo.Font = new Font("Verdana", 8, FontStyle.Bold);
            BrickPageNo.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            BrickPageNo.VertAlignment = DevExpress.Utils.VertAlignment.Center;
        }

        #endregion


        #region Control Event

        void HandleExcelFile(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            string destinationPath = Application.StartupPath + @"\StoneFiles\" + Path.GetFileName(fileName);
            destinationPath = destinationPath.Replace(extension, ".xlsx");

            if (File.Exists(destinationPath))
            {
                File.Delete(destinationPath);
            }
            File.Copy(fileName, destinationPath);

            StrUploadFilename = Path.GetFileName(fileName);
            GetExcelSheetNames(destinationPath);

            CmbSheetName.SelectedIndex = 0;

            if (File.Exists(destinationPath))
            {
                File.Delete(destinationPath);
            }
        }
        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "All files (*.*)|*.*|All files (*.*)|*.*"
                };

                if (CmbLabType.Text == "GIA" || CmbLabType.Text == "IGI" || CmbLabType.Text == "HRD")
                {
                    if (openFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        txtFileName.Text = openFileDialog.FileName;
                        FileInfo f = new FileInfo(txtFileName.Text);

                        if (f.Extension.ToUpper().Contains("CSV"))
                        {
                            DtabExcelData = Global.GetDataTableFromCsv(txtFileName.Text, true);
                            StrUploadFilename = Path.GetFileName(txtFileName.Text);
                        }
                        else
                        {
                            HandleExcelFile(openFileDialog.FileName);
                        }
                    }
                }
                else
                {
                    if (openFileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        txtFileName.Text = openFileDialog.FileName;
                        HandleExcelFile(openFileDialog.FileName);
                    }
                }

                openFileDialog.Dispose();
            }
            catch (Exception ex)
            {
                Global.Message(ex.ToString() + " InValid File Name");
            }

            //try
            //{
            //    CmbSheetName.Items.Clear();
            //    OpenFileDialog Open = new OpenFileDialog();
            //    Open.Filter = "Excel Files|*.xls;*.xlsx";
            //    if (Open.ShowDialog() == DialogResult.OK)
            //    {
            //        txtFileName.Text = Open.FileName;
            //        Global.SprirelGetSheetNameFromExcel(CmbSheetName, txtFileName.Text);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Global.Message(ex.ToString() + "InValid File Name");
            //}
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


        private void BtnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (Val.ToString(CmbLabType.Text).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Select Upload Type..");
                    CmbLabType.Focus();
                    return;
                }
                else if (Val.ToString(txtFileName.Text).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Select File That You Want To Upload..");
                    txtFileName.Focus();
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                int IntSrNo = 1;
                mGroup_ID = Guid.NewGuid();

                int IntCount = 0;

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

                    //DtabExcelData = Global.SprireGetDataTableFromExcel(txtFileName.Text, Val.ToString(CmbSheetName.SelectedItem), true);
                    StrUploadFilename = Path.GetFileName(txtFileName.Text);

                    DtabExcelData = Global.ImportExcelXLSWithSheetName(destinationPath, true, CmbSheetName.SelectedItem.ToString());

                    //DtabExcelData = GetDataTableFromExcel(destinationPath, true);

                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }
                }

                if (DtabExcelData.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    return;
                }

                Fill();

                DtabFinal = DtabFileUpload.Clone();


                if (!DtabExcelData.Columns.Contains("Status"))
                {
                    DtabExcelData.Columns.Add("Status", typeof(string));
                }
                for (int Intcol = 0; Intcol < DtabExcelData.Columns.Count; Intcol++)
                {

                    if (Val.ToString("Sr. No,Srno,SRNO").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("SRNO");

                    if (Val.ToString("Client ref No,Client Ref,StoneNo,Customer Ref No.,#Stock,Stock Id,Customer Ref No#").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("CLIENTREFNO");

                    if (Val.ToString("Report No,ReportNo,Report Number,Report#No,Report.No").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("REPORTNO");

                    if (Val.ToString("JobNo,Job No,Certif.No,Document No,SampleResults: Our Ref.").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("JOBNO");

                    if (Val.ToString("Control No,ControlNo,Dir.No,Other Report Number").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("CONTROLNO");

                    if (Val.ToString("Diamond Dossier").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("DIAMONDDOSSIER");

                    if (Val.ToString("Report Dt,Report Date,ReportDate,ReportDt,Cert.Date,SaleDate,GrdDate,Valid From").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("REPORTDATE");

                    if (Val.ToString("MemoNo,Memo,Memo No").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("MEMONO");

                    if (Val.ToString("Shape,Shp,Shape Name").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("SHAPE");

                    if (Val.ToString("Length,Len,Measurement1,Diam / LW Min").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("LENGTH");

                    if (Val.ToString("Width,Wid,Measurement2,Diam / LW Max").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("WIDTH");

                    if (Val.ToString("Depth,Dep,Measurement3,Height CAvg").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("DEPTH");

                    if (Val.ToString("Weight,Carat,Wt,Weight R").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("CARAT");

                    if (Val.ToString("Color,Col.,Col,Color (Short),Colour").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("COLOR");

                    if (Val.ToString("Color Descriptions,Color Desc,Color Desc.,Color (Long)").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("COLORDESC");

                    if (Val.ToString("Clarity").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper().Trim())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("CLARITY");

                    if (Val.ToString("Clarity Status").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("CLARITYSTATUS");

                    if (Val.ToString("Final Cut,cut,CUT-PROP,Proportions").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("CUT");

                    if (Val.ToString("Polish,Pol,POL or pol/sym").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("POLISH");

                    if (Val.ToString("Symmetry,Symm,Sym").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("SYMMETRY");

                    if (Val.ToString("Fl,Fluorescence,Flo,Fluor,Fluorescence Intensity,LW-fluo").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("FLUORESCENCE");

                    if (Val.ToString("Fluorescence Color").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("FLUORESCENCECOLOR");

                    if (Val.ToString("Girdle,Girdle Desc,Girdle description,Girdle Name").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("GIRDLEDESC");

                    if (Val.ToString("Girdle Condition,Girdle Cond,Girdle nature,Girdle description,Girdle Condtn").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("GIRDLECONDITION");

                    if (Val.ToString("Culet Size,Culet nature").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("CULETSIZE");

                    if (Val.ToString("Depth Per,Depth %,Total Depth,Total depth%").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("DEPTHPER");

                    if (Val.ToString("Table %,Table Per,Table,Table width %").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("TABLE1");

                    if (Val.ToString("Crn Ag,Crown Angle,Crown angle,CR Ang,Crown angle°").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("CRANGLE");

                    if (Val.ToString("Crn Ht,Crown Height,Crown Ht,Cr Height,Height CAvg,Cr# Height%,Cr. Height%,CR Hgt").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("CRHEIGHT");

                    if (Val.ToString("Pav Ag,Pav Ang,Pav Angle,Pavillion Angle,Pavillion Ag,Pavilion Angle,Pavillion angle°, Pavillion angle°,PV Ang").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("PAVANGLE");

                    if (Val.ToString("Pav Dp,Pavillion Depth,pav Depth,Pavilion Depth,Pav# depth%,Pav. depth%,PV Hgt").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("PAVHEIGHT");

                    if (Val.ToString("Star Length,Str Ln,StarLength").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("STARLENGTH");

                    if (Val.ToString("Lr Half,Lower Half,LowerHalf").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("LOWERHALF");

                    if (Val.ToString("Painting").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("PAINTING");

                    if (Val.ToString("Proportion,Proportions").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("PROPORTIONS");

                    if (Val.ToString("Paint Comm,PaintComm").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("PAINTCOMM");

                    if (Val.ToString("Key to Symbols,Key To Sym,Key To Symm").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("KEYTOSYMBOL");

                    if (Val.ToString("Report Comments,Report Comment,ReportComment").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("REPORTCOMMENT");

                    if (Val.ToString("Inscription,Laserscribe ,Laserscribe").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("INSCRIPTION");

                    if (Val.ToString("Synthetic Indicator,SyntheticIndicator").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("SYNTHETICINDICATOR");

                    if (Val.ToString("Girdle %,Girdle Per,Girdle size %,Girdle Percent,Girdle size %").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("GIRDLEPER");

                    if (Val.ToString("Polish Features").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("POLISHFEATURES");

                    if (Val.ToString("Symmetry Features,Symm Features").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("SYMMETRYFEATURES");

                    if (Val.ToString("Shape Description,Shape Desc").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("SHAPEDESC");

                    if (Val.ToString("Report Type").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("REPORTTYPE");

                    if (Val.ToString("Diamond Link").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("DIAMONDLINK");

                    if (Val.ToString("Validitydate").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("VALIDITYDATE");

                    if (Val.ToString("Culet nature").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("CULETNATURE");

                    if (Val.ToString("SaleDoneBy,GrdDoneBy").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("GRDDONEBY");

                    if (Val.ToString("Rapaport").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("RAPAPORT");

                    if (Val.ToString("Back").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("BACK");

                    if (Val.ToString("PricePerCarat").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("PRICEPERCARAT");

                    if (Val.ToString("Amount").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("AMOUNT");

                    if (Val.ToString("BackDisc").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("BACKDISC");

                    if (Val.ToString("SalePrice").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("SALEPRICE");

                    if (Val.ToString("SaleAmount,Sale Amount").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("SALEAMOUNT");

                    if (Val.ToString("Lab Charge").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("LABCHARGE");

                    if (CmbLabType.Text == "HRD")
                    {
                        if (Val.ToString("Remark").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                            DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("REMARK");
                    }
                    else
                    {
                        if (Val.ToString("Remarks,Remark").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                            DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("REMARK");
                    }
                    if (Val.ToString("Milky").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("MILKY");

                    if (Val.ToString("LBLC").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("LBLC");

                    if (Val.ToString("Natts").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("Natts");


                    if (Val.ToString("HNA,HA").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("HA");

                    if (Val.ToString("PAV").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("PAV");

                    if (Val.ToString("BINC").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("BINC");

                    if (Val.ToString("OINC").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("OINC");

                    if (Val.ToString("WINC").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("WINC");

                    if (Val.ToString("Tension").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("TENSION");

                    if (Val.ToString("CS").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("CS");

                    if (Val.ToString("Luster").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("LUSTER");

                    if (Val.ToString("Natural").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("NATURAL");

                    if (Val.ToString("Grain ").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("GRAIN");

                    if (Val.ToString("EyeClean").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("EYECLEAN");

                    if (Val.ToString("Lab").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("LAB");

                    if (Val.ToString("Special Comments").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("SPECIALCOMMENT");   //Used In IGI Format

                    if (Val.ToString("Result").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("RESULT");          //Used In IGI Format


                }

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
                //DataTable DTTableBlack = DtabPara.Select("PARATYPE = 'TABLEBLACKINC'").CopyToDataTable();
                DataTable DTSideBlack = DtabPara.Select("PARATYPE = 'SIDEBLACKINC'").CopyToDataTable();
                DataTable DTRedSport = DtabPara.Select("PARATYPE = 'REDSPORTINC'").CopyToDataTable();
                DataTable DTFancy = new BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_FANCYCOLOR);


                string StrResultStatus = "";
                if (RbtResult.Checked)
                {
                    StrResultStatus = "RESULT";
                }
                else
                {
                    StrResultStatus = "RETURN";
                }

                if (CmbLabType.Text == "GIA")
                {
                    string pStrMemo_ID = BusLib.Configuration.BOConfiguration.FindNewSequentialID().ToString();

                    foreach (DataRow Dr in DtabExcelData.Rows)
                    {
                        string Str = "";
                        string PacketNo = "";
                        string StrPcketTag = "";
                        string StrPartyStokNo = "";
                        DataRow DrFinal = DtabFinal.NewRow();
                        DrFinal["UPLOAD_ID"] = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                        DrFinal["STOCK_ID"] = Guid.Empty;
                        DrFinal["GROUP_ID"] = mGroup_ID;

                        DrFinal["UPLOADDATE"] = Val.ToString(DTPUploadDate.Text);
                        DrFinal["UPLOADTYPE"] = Val.ToString(CmbLabType.Text);

                        if (Val.ToString(Dr["CLIENTREFNO"]).Trim().Equals(string.Empty))
                            continue;

                        //if (Val.ToString(CmbLabType.Text).Trim().ToUpper() == "IGI")
                        if (Val.ToString(Dr["CLIENTREFNO"]).Contains("/"))
                        {

                            DrFinal["KAPANNAME"] = Val.ToString(Dr["CLIENTREFNO"]).Substring(0, Val.ToString(Dr["CLIENTREFNO"]).IndexOf("/")).Trim();
                            Str = Dr["CLIENTREFNO"].ToString().Substring(Val.ToString(Dr["CLIENTREFNO"]).LastIndexOf("/") + 1);
                            PacketNo = Regex.Replace(Val.ToString(Str), "[^0-9]+", string.Empty).Replace("-", "");
                        }
                        else if (Val.ToString(Dr["CLIENTREFNO"]).Contains("-"))
                        {
                            DrFinal["KAPANNAME"] = Val.ToString(Dr["CLIENTREFNO"]).Substring(0, Val.ToString(Dr["CLIENTREFNO"]).IndexOf("-")).Trim();
                            Str = Dr["CLIENTREFNO"].ToString().Substring(Val.ToString(Dr["CLIENTREFNO"]).LastIndexOf("-") + 1);
                            PacketNo = Regex.Replace(Val.ToString(Str), "[^0-9]+", string.Empty);
                        }
                        else
                        {
                            DrFinal["KAPANNAME"] = Val.ToString(Dr["CLIENTREFNO"]);
                            DrFinal["PACKETNO"] = "";
                        }
                        StrPcketTag = Regex.Replace(Str.ToString(), @"[\d-]", "");

                        IntCheck = 0;
                        StrMessage = "";
                        string StrStoneNo = Val.ToString(Dr["CLIENTREFNO"]);


                        /* //Cmnt : #P : 09-01-2021
                        DtabPara = new BOMST_Parameter().GetParameterData();
                        DataTable DTColor = DtabPara.Select("PARATYPE = 'COLOR'").CopyToDataTable();
                        IntColor_ID = FindID(DTColor, Val.ToString(Dr["COLOR"]), StrStoneNo, "Color", ref IntCheck, ref StrMessage);

                        if (IntColor_ID == -1)
                        {
                            if (Val.ToString(Dr["COLORDESC"]) != "" && Val.ToString(Dr["COLOR"]) != "")
                            {
                                Dr["COLORDESC"] = Val.ToString(Dr["COLOR"]) + "," + Val.ToString(Dr["COLORDESC"]);
                                DrFinal["COLORDESC"] = Val.ToString(Dr["COLOR"]) + "," + Val.ToString(Dr["COLORDESC"]);
                            }
                            else
                            {
                                //Dr["ISFANCY"] = true;
                                DrFinal["ISFANCY"] = true;

                                //Dr["FANCYCOLOR"] = Val.ToString(Dr["COLOR"]);
                                DrFinal["FANCYCOLOR"] = Val.ToString(Dr["COLOR"]);
                                //Dr["COLORDESC"] = Val.ToString(Dr["COLOR"]);
                                DrFinal["COLORDESC"] = Val.ToString(Dr["COLOR"]);
                            }
                        }
                        else if (Val.ToString(Dr["COLORDESC"]) != "" && Val.ToString(Dr["COLOR"]) != "")
                        {
                            Dr["COLORDESC"] = Val.ToString(Dr["COLOR"]) + "," + Val.ToString(Dr["COLORDESC"]);
                            DrFinal["COLORDESC"] = Val.ToString(Dr["COLOR"]) + "," + Val.ToString(Dr["COLORDESC"]);
                        }
                        */ //Cmnt : #P : 09-01-2021

                        DrFinal["STATUS"] = Dr["STATUS"];
                        //DrFinal["REMARK"] = Val.ToString(CmbLabType.Text) == "HRD" ? Val.ToString(Dr["REMARK"]) : "";
                        //DrFinal["REMARK"] = Val.ToString(Dr["REMARK"]);

                        DrFinal["PACKETNO"] = Val.ToString(PacketNo);
                        DrFinal["TAG"] = Val.ToString(StrPcketTag);

                        DrFinal["GRDFLAG"] = 0;
                        DrFinal["TYPEFLAG"] = 0;
                        DrFinal["UPLOADFILENAME"] = StrUploadFilename;
                        DrFinal["SRNO"] = IntSrNo;
                        DrFinal["JOBNO"] = Dr["JOBNO"];
                        DrFinal["CONTROLNO"] = Val.ToString(Dr["CONTROLNO"]);
                        DrFinal["DIAMONDDOSSIER"] = Dr["DIAMONDDOSSIER"];
                        DrFinal["REPORTNO"] = Dr["REPORTNO"];
                        DrFinal["REPORTDATE"] = Val.ToString(Dr["REPORTDATE"]);

                        DrFinal["CLIENTREFNO"] = Dr["CLIENTREFNO"];
                        DrFinal["MEMONO"] = Dr["MEMONO"];

                        DrFinal["SHAPE"] = Dr["SHAPE"];
                        DrFinal["LENGTH"] = Dr["LENGTH"];
                        DrFinal["WIDTH"] = Dr["WIDTH"];
                        DrFinal["DEPTH"] = Dr["DEPTH"];
                        DrFinal["CARAT"] = Dr["CARAT"];
                        DrFinal["COLOR"] = Dr["COLOR"];

                        if (Val.ToString(Dr["COLOR"]) == "*")
                        {
                            string StrColorDesc = Val.ToString(Dr["COLORDESC"]);
                            if (StrColorDesc.Contains("[*]"))
                            {
                                StrColorDesc = StrColorDesc.Remove(0, 4);
                                if (StrColorDesc.ToUpper().Contains("NATURAL"))
                                {
                                    StrColorDesc = Regex.Replace(StrColorDesc.ToUpper(), "NATURAL", string.Empty);
                                    StrColorDesc = StrColorDesc.Remove(StrColorDesc.IndexOf(","), 2);
                                    StrColorDesc = "NATURAL" + StrColorDesc;
                                }
                                if (StrColorDesc.ToUpper().Contains("UNDETERMINED"))
                                {
                                    StrColorDesc = Regex.Replace(StrColorDesc.ToUpper(), "UNDETERMINED", string.Empty);
                                    StrColorDesc = StrColorDesc.Remove(StrColorDesc.IndexOf(","), 2);
                                    StrColorDesc = "UNDETERMINED" + StrColorDesc;
                                }
                                DrFinal["COLORDESC"] = StrColorDesc;

                                DtabPara = new BOMST_Parameter().GetFancyColorData();

                                int IntCnt = 0;
                                bool exitLoop = false;
                                foreach (DataRow DRow in DtabPara.Rows)
                                {
                                    string[] strSplit = Val.ToString(DRow["REMARK"]).ToUpper().Split(',');
                                    foreach (string STR in strSplit)
                                    {
                                        if (STR.Contains(StrColorDesc))
                                        {
                                            IntCnt = 1;
                                            exitLoop = true;
                                            break;
                                        }
                                    }
                                    if (exitLoop)
                                    {
                                        break;
                                    }
                                }

                                if (IntCnt == 0)
                                {
                                    Global.Message("Stone No : " + Dr["CLIENTREFNO"] + " ->  Fancy color : [ " + StrColorDesc + " ]  Not Exist in Master Table..");
                                    this.Cursor = Cursors.Default;
                                    return;
                                }


                                //IntCheck = dr.Count() > 0 ? Val.ToInt(dr.FirstOrDefault()["PARA_ID"]) : 0;
                                //if (IntCheck == 0)
                                //{
                                //    Global.Message( "Stone No : " + Dr["CLIENTREFNO"] + " ->  Fancy color : [ " + StrColorDesc + " ]  Not Exist in Master Table..");
                                //    this.Cursor = Cursors.Default;
                                //    return;
                                //}

                            }
                        }
                        else
                        {
                            DrFinal["COLORDESC"] = Dr["COLORDESC"];
                        }

                        DrFinal["CLARITY"] = Dr["CLARITY"];
                        DrFinal["CLARITYSTATUS"] = Dr["CLARITYSTATUS"];
                        DrFinal["CUT"] = Dr["CUT"];
                        DrFinal["POLISH"] = Dr["POLISH"];
                        DrFinal["SYMMETRY"] = Dr["SYMMETRY"];
                        DrFinal["FLUORESCENCE"] = Dr["FLUORESCENCE"];
                        DrFinal["FLUORESCENCECOLOR"] = Dr["FLUORESCENCECOLOR"];

                        DrFinal["GIRDLEDESC"] = Dr["GIRDLEDESC"];
                        DrFinal["GIRDLECONDITION"] = Dr["GIRDLECONDITION"];

                        DrFinal["CULETSIZE"] = Dr["CULETSIZE"];
                        DrFinal["DEPTHPER"] = Dr["DEPTHPER"];
                        DrFinal["TABLE1"] = Dr["TABLE1"];


                        DrFinal["CRANGLE"] = Regex.Replace(Val.ToString(Dr["CRANGLE"]), @"[^0-9\.]+", "");
                        DrFinal["CRHEIGHT"] = Regex.Replace(Val.ToString(Dr["CRHEIGHT"]), @"[^0-9\.]+", "");
                        DrFinal["PAVANGLE"] = Regex.Replace(Val.ToString(Dr["PAVANGLE"]), @"[^0-9\.]+", "");
                        DrFinal["PAVHEIGHT"] = Regex.Replace(Val.ToString(Dr["PAVHEIGHT"]), @"[^0-9\.]+", "");

                        DrFinal["STARLENGTH"] = Regex.Replace(Val.ToString(Dr["STARLENGTH"]), @"[^0-9\.]+", "");
                        DrFinal["LOWERHALF"] = Regex.Replace(Val.ToString(Dr["LOWERHALF"]), @"[^0-9\.]+", "");

                        DrFinal["PAINTING"] = Dr["PAINTING"];
                        DrFinal["PROPORTIONS"] = Dr["PROPORTIONS"];
                        DrFinal["PAINTCOMM"] = Dr["PAINTCOMM"];

                        DrFinal["KEYTOSYMBOL"] = Dr["KEYTOSYMBOL"];

                        DrFinal["REPORTCOMMENT"] = Dr["REPORTCOMMENT"];

                        DrFinal["INSCRIPTION"] = Dr["INSCRIPTION"];

                        DrFinal["SYNTHETICINDICATOR"] = Dr["SYNTHETICINDICATOR"];
                        DrFinal["GIRDLEPER"] = Regex.Replace(Val.ToString(Dr["GIRDLEPER"]), @"[^0-9\.]+", "");
                        DrFinal["POLISHFEATURES"] = Dr["POLISHFEATURES"];
                        DrFinal["SYMMETRYFEATURES"] = Dr["SYMMETRYFEATURES"];
                        DrFinal["SHAPEDESC"] = Dr["SHAPEDESC"];

                        DrFinal["REPORTTYPE"] = CmbLabType.Text == "GIA" ? Dr["REPORTTYPE"] : "";

                        if (RbtResult.Checked == true)
                        {
                            DrFinal["GIARESULTSTATUS"] = "RESULT";
                        }
                        else if (RbtReturn.Checked == true)
                        {
                            DrFinal["GIARESULTSTATUS"] = "RETURN";
                        }

                        IntSrNo++;

                        DtabFinal.Rows.Add(DrFinal);
                    }

                    DtabFileUpload = ObjUpload.Save(DtabFinal, mGroup_ID, Val.ToString(CmbLabType.Text), Val.SqlDate(DTPUploadDate.Value.ToShortDateString()), StrResultStatus, pStrMemo_ID);
                }
                else if (CmbLabType.Text == "IGI")
                {
                    string pStrMemo_ID = BusLib.Configuration.BOConfiguration.FindNewSequentialID().ToString();
                    foreach (DataRow Dr in DtabExcelData.Rows)
                    {
                        string Str = "";
                        string PacketNo = "";
                        string StrPcketTag = "";
                        DataRow DrFinal = DtabFinal.NewRow();
                        DrFinal["UPLOAD_ID"] = Guid.NewGuid();
                        DrFinal["STOCK_ID"] = Guid.Empty;
                        DrFinal["GROUP_ID"] = mGroup_ID;

                        DrFinal["UPLOADDATE"] = Val.ToString(DTPUploadDate.Text);
                        DrFinal["UPLOADTYPE"] = Val.ToString(CmbLabType.Text);

                        if (Val.ToString(Dr["CLIENTREFNO"]).Trim().Equals(string.Empty))
                            continue;

                        //if (Val.ToString(CmbLabType.Text).Trim().ToUpper() == "IGI")
                        if (Val.ToString(Dr["CLIENTREFNO"]).Contains("/"))
                        {

                            DrFinal["KAPANNAME"] = Val.ToString(Dr["CLIENTREFNO"]).Substring(0, Val.ToString(Dr["CLIENTREFNO"]).IndexOf("/")).Trim();
                            Str = Dr["CLIENTREFNO"].ToString().Substring(Val.ToString(Dr["CLIENTREFNO"]).LastIndexOf("/") + 1);
                            PacketNo = Regex.Replace(Val.ToString(Str), "[^0-9]+", string.Empty).Replace("-", "");
                        }
                        else if (Val.ToString(Dr["CLIENTREFNO"]).Contains("-"))
                        {
                            DrFinal["KAPANNAME"] = Val.ToString(Dr["CLIENTREFNO"]).Substring(0, Val.ToString(Dr["CLIENTREFNO"]).IndexOf("-")).Trim();
                            Str = Dr["CLIENTREFNO"].ToString().Substring(Val.ToString(Dr["CLIENTREFNO"]).LastIndexOf("-") + 1);
                            PacketNo = Regex.Replace(Val.ToString(Str), "[^0-9]+", string.Empty);
                        }
                        else
                        {
                            DrFinal["KAPANNAME"] = Val.ToString(Dr["CLIENTREFNO"]);
                            DrFinal["PACKETNO"] = "";
                        }
                        StrPcketTag = Regex.Replace(Str.ToString(), @"[\d-]", "");

                        IntCheck = 0;
                        StrMessage = "";
                        string StrStoneNo = Val.ToString(Dr["CLIENTREFNO"]);
                        DtabPara = new BOMST_Parameter().GetParameterData();
                        IntColor_ID = FindID(DTColor, Val.ToString(Dr["COLOR"]), StrStoneNo, "Color", ref IntCheck, ref StrMessage);

                        if (IntColor_ID == -1)
                        {
                            if (Val.ToString(Dr["COLORDESC"]) != "" && Val.ToString(Dr["COLOR"]) != "")
                            {
                                Dr["COLORDESC"] = Val.ToString(Dr["COLOR"]) + "," + Val.ToString(Dr["COLORDESC"]);
                                DrFinal["COLORDESC"] = Val.ToString(Dr["COLOR"]) + "," + Val.ToString(Dr["COLORDESC"]);
                            }
                            else
                            {
                                Dr["ISFANCY"] = true;
                                DrFinal["ISFANCY"] = true;

                                Dr["FANCYCOLOR"] = Val.ToString(Dr["COLOR"]);
                                DrFinal["FANCYCOLOR"] = Val.ToString(Dr["COLOR"]);
                                Dr["COLORDESC"] = Val.ToString(Dr["COLOR"]);
                                DrFinal["COLORDESC"] = Val.ToString(Dr["COLOR"]);
                            }
                        }
                        else if (Val.ToString(Dr["COLORDESC"]) != "" && Val.ToString(Dr["COLOR"]) != "")
                        {
                            Dr["COLORDESC"] = Val.ToString(Dr["COLOR"]) + "," + Val.ToString(Dr["COLORDESC"]);
                            DrFinal["COLORDESC"] = Val.ToString(Dr["COLOR"]) + "," + Val.ToString(Dr["COLORDESC"]);
                        }

                        DrFinal["PACKETNO"] = Val.ToString(PacketNo);
                        DrFinal["TAG"] = Val.ToString(StrPcketTag);


                        DrFinal["STATUS"] = Dr["STATUS"];
                        //   DrFinal["REMARK"] = Val.ToString(Dr["REMARK"]);

                        DrFinal["GRDFLAG"] = 0;
                        DrFinal["TYPEFLAG"] = 0;
                        DrFinal["UPLOADFILENAME"] = StrUploadFilename;
                        DrFinal["SRNO"] = IntSrNo;
                        DrFinal["JOBNO"] = Dr["JOBNO"];
                        DrFinal["CONTROLNO"] = Val.ToString(Dr["CONTROLNO"]);
                        DrFinal["DIAMONDDOSSIER"] = "";
                        DrFinal["REPORTNO"] = Dr["REPORTNO"];
                        DrFinal["REPORTDATE"] = Val.ToString(Dr["REPORTDATE"]);

                        DrFinal["CLIENTREFNO"] = Dr["CLIENTREFNO"];
                        DrFinal["MEMONO"] = Dr["MEMONO"];
                        Int32 pIntShape_ID = FindID(DTShape, Val.ToString(Dr["SHAPE"]), StrStoneNo, "Shape", ref IntCheck, ref StrMessage);
                        if (IntCheck == -1)
                        {
                            Global.Message(StrMessage);
                            return;
                        }
                        else
                        {
                            DrFinal["SHAPE"] = Dr["SHAPE"];
                        }

                        DrFinal["LENGTH"] = Val.ToString(Dr["LENGTH"]).Trim().Equals(string.Empty) ? "" : Val.ToString(Dr["LENGTH"]);
                        DrFinal["WIDTH"] = Dr["WIDTH"];
                        DrFinal["DEPTH"] = Dr["DEPTH"];
                        DrFinal["CARAT"] = Dr["CARAT"];

                        IntColor_ID = FindID(DTColor, Val.ToString(Dr["COLOR"]), StrStoneNo, "Color", ref IntCheck, ref StrMessage);
                        if (IntCheck == -1)
                        {
                            Global.Message(StrMessage);
                            return;
                        }
                        else
                        {
                            DrFinal["COLOR"] = Dr["COLOR"];
                        }

                        Int32 pIntClarityID = FindID(DTClarity, Val.ToString(Dr["CLARITY"]), StrStoneNo, "Clarity", ref IntCheck, ref StrMessage);
                        if (IntCheck == -1)
                        {
                            Global.Message(StrMessage);
                            return;
                        }
                        else
                        {
                            DrFinal["CLARITY"] = Dr["CLARITY"];
                        }

                        DrFinal["CLARITYSTATUS"] = "";
                        Int32 pIntCutID = FindID(DTCut, Val.ToString(Dr["CUT"]), StrStoneNo, "Cut", ref IntCheck, ref StrMessage);
                        if (IntCheck == -1)
                        {
                            Global.Message(StrMessage);
                            return;
                        }
                        else
                        {
                            DrFinal["CUT"] = Dr["CUT"];
                        }

                        Int32 pIntPolishID = FindID(DTPolish, Val.ToString(Dr["POLISH"]), StrStoneNo, "Polish", ref IntCheck, ref StrMessage);
                        if (IntCheck == -1)
                        {
                            Global.Message(StrMessage);
                            return;
                        }
                        else
                        {
                            DrFinal["POLISH"] = Dr["POLISH"];
                        }

                        Int32 pIntSymID = FindID(DTSym, Val.ToString(Dr["SYMMETRY"]), StrStoneNo, "Symmetry", ref IntCheck, ref StrMessage);
                        if (IntCheck == -1)
                        {
                            Global.Message(StrMessage);
                            return;
                        }
                        else
                        {
                            DrFinal["SYMMETRY"] = Dr["SYMMETRY"];

                        }

                        Int32 pIntFLID = FindID(DTFL, Val.ToString(Dr["FLUORESCENCE"]), StrStoneNo, "Fluorescence", ref IntCheck, ref StrMessage);
                        if (IntCheck == -1)
                        {
                            Global.Message(StrMessage);
                            return;
                        }
                        else
                        {
                            DrFinal["FLUORESCENCE"] = Dr["FLUORESCENCE"];
                        }

                        DrFinal["FLUORESCENCECOLOR"] = "";

                        DrFinal["GIRDLEDESC"] = Dr["GIRDLEDESC"];
                        DrFinal["GIRDLECONDITION"] = "";

                        DrFinal["CULETSIZE"] = Dr["CULETSIZE"];
                        DrFinal["DEPTHPER"] = Dr["DEPTHPER"];
                        DrFinal["TABLE1"] = Dr["TABLE1"];


                        DrFinal["CRANGLE"] = Regex.Replace(Val.ToString(Dr["CRANGLE"]), @"[^0-9\.]+", "");
                        DrFinal["CRHEIGHT"] = Regex.Replace(Val.ToString(Dr["CRHEIGHT"]), @"[^0-9\.]+", "");
                        DrFinal["PAVANGLE"] = Regex.Replace(Val.ToString(Dr["PAVANGLE"]), @"[^0-9\.]+", "");
                        DrFinal["PAVHEIGHT"] = Regex.Replace(Val.ToString(Dr["PAVHEIGHT"]), @"[^0-9\.]+", "");

                        DrFinal["STARLENGTH"] = "";
                        DrFinal["LOWERHALF"] = "";

                        DrFinal["PAINTING"] = "";
                        DrFinal["PROPORTIONS"] = "";
                        DrFinal["PAINTCOMM"] = "";

                        DrFinal["KEYTOSYMBOL"] = "";

                        DrFinal["REPORTCOMMENT"] = Dr["REPORTCOMMENT"];

                        //Commment and Added By Gunjan:17/10/2024
                        //DrFinal["INSCRIPTION"] = "";
                        DrFinal["INSCRIPTION"] = Dr["INSCRIPTION"];
                        //End As Gunjan

                        DrFinal["SYNTHETICINDICATOR"] = "";
                        DrFinal["GIRDLEPER"] = Dr["GIRDLEPER"];
                        DrFinal["POLISHFEATURES"] = "";
                        DrFinal["SYMMETRYFEATURES"] = "";
                        DrFinal["SHAPEDESC"] = "";

                        DrFinal["REPORTTYPE"] = "";

                        DrFinal["DIAMONDLINK"] = "";

                        DrFinal["SPECIALCOMMENT"] = Dr["SPECIALCOMMENT"];
                        //DrFinal["RESULT"] = Dr["RESULT"];

                        IntSrNo++;

                        DtabFinal.Rows.Add(DrFinal);
                    }
                    DtabFileUpload = ObjUpload.Save(DtabFinal, mGroup_ID, Val.ToString(CmbLabType.Text), Val.SqlDate(DTPUploadDate.Value.ToShortDateString()), StrResultStatus, pStrMemo_ID);
                    if (DtabFileUpload.Rows.Count > 0)
                    {
                        Global.Message("File Upload Successfully");
                    }


                }
                else if (CmbLabType.Text == "HRD")
                {
                    string pStrMemo_ID = BusLib.Configuration.BOConfiguration.FindNewSequentialID().ToString();
                    foreach (DataRow Dr in DtabExcelData.Rows)
                    {
                        string Str = "";
                        string PacketNo = "";
                        string StrPcketTag = "";
                        DataRow DrFinal = DtabFinal.NewRow();
                        DrFinal["UPLOAD_ID"] = Guid.NewGuid();
                        DrFinal["STOCK_ID"] = Guid.Empty;
                        DrFinal["GROUP_ID"] = mGroup_ID;

                        DrFinal["UPLOADDATE"] = Val.ToString(DTPUploadDate.Text);
                        DrFinal["UPLOADTYPE"] = Val.ToString(CmbLabType.Text);

                        if (Val.ToString(Dr["CLIENTREFNO"]).Trim().Equals(string.Empty))
                            continue;

                        //if (Val.ToString(CmbLabType.Text).Trim().ToUpper() == "IGI")
                        if (Val.ToString(Dr["CLIENTREFNO"]).Contains("/"))
                        {

                            DrFinal["KAPANNAME"] = Val.ToString(Dr["CLIENTREFNO"]).Substring(0, Val.ToString(Dr["CLIENTREFNO"]).IndexOf("/")).Trim();
                            Str = Dr["CLIENTREFNO"].ToString().Substring(Val.ToString(Dr["CLIENTREFNO"]).LastIndexOf("/") + 1);
                            PacketNo = Regex.Replace(Val.ToString(Str), "[^0-9]+", string.Empty).Replace("-", "");
                        }
                        else if (Val.ToString(Dr["CLIENTREFNO"]).Contains("-"))
                        {
                            DrFinal["KAPANNAME"] = Val.ToString(Dr["CLIENTREFNO"]).Substring(0, Val.ToString(Dr["CLIENTREFNO"]).IndexOf("-")).Trim();
                            Str = Dr["CLIENTREFNO"].ToString().Substring(Val.ToString(Dr["CLIENTREFNO"]).LastIndexOf("-") + 1);
                            PacketNo = Regex.Replace(Val.ToString(Str), "[^0-9]+", string.Empty);
                        }
                        else
                        {
                            DrFinal["KAPANNAME"] = Val.ToString(Dr["CLIENTREFNO"]);
                            DrFinal["PACKETNO"] = "";
                        }
                        StrPcketTag = Regex.Replace(Str.ToString(), @"[\d-]", "");

                        IntCheck = 0;
                        StrMessage = "";
                        string StrStoneNo = Val.ToString(Dr["CLIENTREFNO"]);
                        DtabPara = new BOMST_Parameter().GetParameterData();
                        DTColor = DtabPara.Select("PARATYPE = 'COLOR'").CopyToDataTable();
                        IntColor_ID = FindID(DTColor, Val.ToString(Dr["COLOR"]), StrStoneNo, "Color", ref IntCheck, ref StrMessage);

                        

                        DrFinal["PACKETNO"] = Val.ToString(PacketNo);
                        DrFinal["TAG"] = Val.ToString(StrPcketTag);


                        DrFinal["STATUS"] = Dr["STATUS"];
                        // DrFinal["REMARK"] = Val.ToString(Dr["REMARK"]);

                        DrFinal["GRDFLAG"] = 0;
                        DrFinal["TYPEFLAG"] = 0;
                        DrFinal["UPLOADFILENAME"] = StrUploadFilename;
                        DrFinal["SRNO"] = IntSrNo;
                        //DrFinal["JOBNO"] = Dr["JOBNO"];
                        DrFinal["CONTROLNO"] = string.Empty; // Val.ToString(Dr["CONTROLNO"]);
                        DrFinal["DIAMONDDOSSIER"] = "";
                        DrFinal["REPORTNO"] = Dr["REPORTNO"];
                        DrFinal["REPORTDATE"] = Val.ToString(Dr["REPORTDATE"]);

                        DrFinal["CLIENTREFNO"] = Dr["CLIENTREFNO"];
                        DrFinal["MEMONO"] = string.Empty; // Dr["MEMONO"];

                        DrFinal["SHAPE"] = Dr["SHAPE"];
                        DrFinal["LENGTH"] = Val.ToString(Dr["LENGTH"]).Trim().Equals(string.Empty) ? "" : Val.ToString(Dr["LENGTH"]);
                        DrFinal["WIDTH"] = Dr["WIDTH"];
                        DrFinal["DEPTH"] = Dr["DEPTH"];
                        DrFinal["CARAT"] = Dr["CARAT"];
                        DrFinal["COLOR"] = Dr["COLOR"];

                        DrFinal["COLORDESC"] = string.Empty; // Dr["COLORDESC"];

                        DrFinal["CLARITY"] = Dr["CLARITY"];
                        DrFinal["CLARITYSTATUS"] = "";
                        DrFinal["CUT"] = Dr["CUT"];
                        DrFinal["POLISH"] = Dr["POLISH"];
                        DrFinal["SYMMETRY"] = Dr["SYMMETRY"];
                        DrFinal["FLUORESCENCE"] = Dr["FLUORESCENCE"];
                        DrFinal["FLUORESCENCECOLOR"] = "";

                        DrFinal["GIRDLEDESC"] = Dr["GIRDLEDESC"];
                        DrFinal["GIRDLECONDITION"] = "";

                        DrFinal["CULETSIZE"] = Dr["CULETSIZE"];
                        DrFinal["DEPTHPER"] = Dr["DEPTHPER"];
                        DrFinal["TABLE1"] = Dr["TABLE1"];


                        DrFinal["CRANGLE"] = Regex.Replace(Val.ToString(Dr["CRANGLE"]), @"[^0-9\.]+", "");
                        DrFinal["CRHEIGHT"] = Regex.Replace(Val.ToString(Dr["CRHEIGHT"]), @"[^0-9\.]+", "");
                        DrFinal["PAVANGLE"] = Regex.Replace(Val.ToString(Dr["PAVANGLE"]), @"[^0-9\.]+", "");
                        DrFinal["PAVHEIGHT"] = Regex.Replace(Val.ToString(Dr["PAVHEIGHT"]), @"[^0-9\.]+", "");

                        DrFinal["STARLENGTH"] = "";
                        DrFinal["LOWERHALF"] = "";

                        DrFinal["PAINTING"] = "";
                        DrFinal["PROPORTIONS"] = "";
                        DrFinal["PAINTCOMM"] = "";

                        DrFinal["KEYTOSYMBOL"] = "";

                        DrFinal["REPORTCOMMENT"] = ""; // Dr["REPORTCOMMENT"];

                        DrFinal["INSCRIPTION"] = "";

                        DrFinal["SYNTHETICINDICATOR"] = "";
                        DrFinal["GIRDLEPER"] = Dr["GIRDLEPER"];
                        DrFinal["POLISHFEATURES"] = "";
                        DrFinal["SYMMETRYFEATURES"] = "";
                        DrFinal["SHAPEDESC"] = "";

                        DrFinal["REPORTTYPE"] = "";

                        DrFinal["DIAMONDLINK"] = "";

                        DrFinal["SPECIALCOMMENT"] = ""; // Dr["SPECIALCOMMENT"];
                        //DrFinal["RESULT"] = Dr["RESULT"];

                        IntSrNo++;

                        DtabFinal.Rows.Add(DrFinal);
                    }
                    DtabFileUpload = ObjUpload.Save(DtabFinal, mGroup_ID, Val.ToString(CmbLabType.Text), Val.SqlDate(DTPUploadDate.Value.ToShortDateString()), StrResultStatus, pStrMemo_ID);
                }
                else if (CmbLabType.Text == "SALES" || CmbLabType.Text == "BOMBAY")
                {
                    string pStrMemo_ID = BusLib.Configuration.BOConfiguration.FindNewSequentialID().ToString();

                    foreach (DataRow Dr in DtabExcelData.Rows)
                    {

                        if (Val.ToString(Dr["CLIENTREFNO"]).Trim().Equals(string.Empty))
                            continue;

                        string Str = "";
                        string PacketNo = "";
                        string StrPcketTag = "";
                        DataRow DrFinal = DtabFinal.NewRow();
                        DrFinal["UPLOAD_ID"] = Guid.NewGuid();
                        DrFinal["PACKET_ID"] = Guid.Empty;
                        DrFinal["GROUP_ID"] = mGroup_ID;

                        DrFinal["UPLOADDATE"] = Val.ToString(DTPUploadDate.Text);
                        DrFinal["UPLOADTYPE"] = Val.ToString(CmbLabType.Text);

                        if (Val.ToString(Dr["CLIENTREFNO"]).Contains("-"))
                        {
                            DrFinal["KAPANNAME"] = Val.ToString(Dr["CLIENTREFNO"]).Substring(0, Val.ToString(Dr["CLIENTREFNO"]).IndexOf("-")).Trim();
                            Str = Dr["CLIENTREFNO"].ToString().Substring(Val.ToString(Dr["CLIENTREFNO"]).LastIndexOf("-") + 1);
                            PacketNo = Regex.Replace(Val.ToString(Str), "[^0-9]+", string.Empty);
                        }
                        else
                        {
                            DrFinal["KAPANNAME"] = Val.ToString(Dr["CLIENTREFNO"]);
                            DrFinal["PACKETNO"] = "";
                        }
                        StrPcketTag = Regex.Replace(Str.ToString(), @"[\d-]", "");

                        DrFinal["PACKETNO"] = Val.ToString(PacketNo);
                        DrFinal["TAG"] = Val.ToString(StrPcketTag);

                        DrFinal["STATUS"] = Dr["STATUS"];

                        DrFinal["BACKDISC"] = "";
                        DrFinal["SALEPRICE"] = "";
                        DrFinal["SALEAMOUNT"] = CmbLabType.Text == "SALES" ? Dr["SALEAMOUNT"] : "";
                        DrFinal["LABCHARGE"] = "";
                        DrFinal["AMOUNT"] = Dr["AMOUNT"];
                        DrFinal["UPLOADFILENAME"] = StrUploadFilename;

                        if (CmbLabType.Text == "BOMBAY")
                        {
                            DrFinal["REMARK"] = Dr["REMARK"];

                            DrFinal["GRDFLAG"] = 0;
                            DrFinal["TYPEFLAG"] = 0;

                            DrFinal["SRNO"] = IntSrNo;

                            DrFinal["CLIENTREFNO"] = Dr["CLIENTREFNO"];
                            DrFinal["GRDDONEBY"] = Dr["GRDDONEBY"];
                            DrFinal["REPORTDATE"] = Val.ToString(Dr["REPORTDATE"]);

                            DrFinal["CARAT"] = Dr["CARAT"];
                            DrFinal["SHAPE"] = Dr["SHAPE"];
                            DrFinal["COLOR"] = Dr["COLOR"];
                            DrFinal["CLARITY"] = Dr["CLARITY"];
                            DrFinal["CUT"] = Dr["CUT"];
                            DrFinal["POLISH"] = Dr["POLISH"];
                            DrFinal["SYMMETRY"] = Dr["SYMMETRY"];
                            DrFinal["FLUORESCENCE"] = Dr["FLUORESCENCE"];

                            DrFinal["RAPAPORT"] = Dr["RAPAPORT"];
                            DrFinal["BACK"] = Dr["BACK"];
                            DrFinal["PRICEPERCARAT"] = Dr["PRICEPERCARAT"];
                            DrFinal["AMOUNT"] = Dr["AMOUNT"];

                            //DrFinal["BACKDISC"] = CmbLabType.Text == "SALES" ? Dr["BACKDISC"] : "";
                            //DrFinal["SALEPRICE"] = CmbLabType.Text == "SALES" ? Dr["SALEPRICE"] : "";
                            //DrFinal["SALEAMOUNT"] = CmbLabType.Text == "SALES" ? Dr["SALEAMOUNT"] : "";
                            //DrFinal["LABCHARGE"] = CmbLabType.Text == "SALES" ? Dr["LABCHARGE"] : "";

                            DrFinal["REMARK"] = Dr["REMARK"];

                            DrFinal["MILKY"] = CmbLabType.Text == "BOMBAY" ? Val.ToString(Dr["MILKY"]) : "";
                            DrFinal["LBLC"] = CmbLabType.Text == "BOMBAY" ? Val.ToString(Dr["LBLC"]) : "";
                            DrFinal["NATTS"] = CmbLabType.Text == "BOMBAY" ? Val.ToString(Dr["NATTS"]) : "";
                            DrFinal["HA"] = CmbLabType.Text == "BOMBAY" ? Val.ToString(Dr["HA"]) : "";
                            DrFinal["PAV"] = CmbLabType.Text == "BOMBAY" ? Val.ToString(Dr["PAV"]) : "";
                            DrFinal["BINC"] = CmbLabType.Text == "BOMBAY" ? Val.ToString(Dr["BINC"]) : "";
                            DrFinal["OINC"] = CmbLabType.Text == "BOMBAY" ? Val.ToString(Dr["OINC"]) : "";
                            DrFinal["WINC"] = CmbLabType.Text == "BOMBAY" ? Val.ToString(Dr["WINC"]) : "";
                            DrFinal["TENSION"] = CmbLabType.Text == "BOMBAY" ? Val.ToString(Dr["TENSION"]) : "";
                            DrFinal["CS"] = CmbLabType.Text == "BOMBAY" ? Val.ToString(Dr["CS"]) : "";
                            DrFinal["LUSTER"] = CmbLabType.Text == "BOMBAY" ? Val.ToString(Dr["LUSTER"]) : "";
                            DrFinal["NATURAL"] = CmbLabType.Text == "BOMBAY" ? Val.ToString(Dr["NATURAL"]) : "";
                            DrFinal["GRAIN"] = CmbLabType.Text == "BOMBAY" ? Val.ToString(Dr["GRAIN"]) : "";
                            DrFinal["EYECLEAN"] = CmbLabType.Text == "BOMBAY" ? Val.ToString(Dr["EYECLEAN"]) : "";
                            DrFinal["LAB"] = CmbLabType.Text == "BOMBAY" ? Val.ToString(Dr["LAB"]) : "";
                        }

                        IntSrNo++;
                        DtabFinal.Rows.Add(DrFinal);
                    }
                    DtabFileUpload = ObjUpload.Save(DtabFinal, mGroup_ID, CmbLabType.Text, Val.SqlDate(DTPUploadDate.Value.ToShortDateString()), StrResultStatus, pStrMemo_ID);
                }

                if (DtabFileUpload.Rows.Count > 0 && Val.ToString(DtabFileUpload.Rows[0]["RETURNTYPE"]) == "FAIL")
                {
                    FrmPopupGrid FrmPopupGrid = new FrmPopupGrid();
                    FrmPopupGrid.CountedColumn = "PARTYSTOCKNO";
                    FrmPopupGrid.MainGrid.DataSource = DtabFileUpload;
                    FrmPopupGrid.MainGrid.Dock = DockStyle.Fill;
                    FrmPopupGrid.ColumnsToHide = "RETURNTYPE";
                    FrmPopupGrid.Text = "Lab Issue Not Exists";
                    FrmPopupGrid.LblTitle.Text = "List Of Packets Which Lab Issue Not Exists.";
                    if (DtabFileUpload.Columns.Count == 3)
                    {
                        FrmPopupGrid.Text = "Fancy Color Not Exists in Master";
                        FrmPopupGrid.LblTitle.Text = "List Of Packets Which Fancy Color Not Exists.";

                    }
                    FrmPopupGrid.ISPostBack = true;
                    this.Cursor = Cursors.Default;

                    FrmPopupGrid.Width = 1000;
                    FrmPopupGrid.GrdDet.OptionsBehavior.Editable = false;
                    //FrmPopupGrid.Size = this.Size;

                    FrmPopupGrid.GrdDet.Columns["PARTYSTOCKNO"].Caption = "StoneNo";
                    FrmPopupGrid.GrdDet.Columns["PARTYSTOCKNO"].Width = 100;
                    FrmPopupGrid.ShowDialog();
                    FrmPopupGrid.Hide();
                    FrmPopupGrid.Dispose();
                    FrmPopupGrid = null;
                    this.Cursor = Cursors.Default;

                    //DtabFinal.Columns.Clear();
                    DtabFinal.Rows.Clear();

                    return;
                }
                //if (DtabFileUpload.Rows.Count > 0)
                else
                {
                    this.Cursor = Cursors.Default;

                    MainGrid.DataSource = DtabFileUpload;
                    MainGrid.Refresh();
                    GrdDetail.OptionsBehavior.Editable = false;
                    ChangeGridColumnsCaption(GrdDetail);
                    GrdDetail.BestFitColumns();

                    mUpload_ID = Guid.Empty;
                    //DtabFinal.Columns.Clear();
                    DtabFinal.Rows.Clear();
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }


        }
        public void ChangeGridColumnsCaption(DevExpress.XtraGrid.Views.Grid.GridView GrdView)
        {
            //if (CmbLabType.Text == "GIA" || CmbLabType.Text == "IGI")
            //{
            //    GrdView.Columns["KAPANNAME"].Caption = "Kapan";
            //    GrdView.Columns["PACKETNO"].Caption = "Packet No";
            //    GrdView.Columns["TAG"].Caption = "Tag";
            //    GrdView.Columns["UPLOADTYPE"].Caption = "Lab Type";
            //    GrdView.Columns["STATUS"].Caption = "Status";
            //    GrdView.Columns["SRNO"].Caption = "Sr No";
            //    GrdView.Columns["JOBNO"].Caption = "Job No";
            //    GrdView.Columns["CONTROLNO"].Caption = "Control No";
            //    GrdView.Columns["DIAMONDDOSSIER"].Caption = "Diamond Dossier";
            //    GrdView.Columns["REPORTNO"].Caption = "Report No";
            //    GrdView.Columns["REPORTDATE"].Caption = "Report Date";
            //    GrdView.Columns["CLIENTREFNO"].Caption = "Client Ref";
            //    GrdView.Columns["MEMONO"].Caption = "Memo No";
            //    GrdView.Columns["SHAPE"].Caption = "Shape";
            //    GrdView.Columns["LENGTH"].Caption = "Length";
            //    GrdView.Columns["WIDTH"].Caption = "Width";
            //    GrdView.Columns["DEPTH"].Caption = "Depth";

            //    GrdView.Columns["CARAT"].Caption = "Carat";
            //    GrdView.Columns["COLOR"].Caption = "Color";
            //    GrdView.Columns["COLORDESC"].Caption = "Color Desc";
            //    GrdView.Columns["CLARITY"].Caption = "Clarity";
            //    GrdView.Columns["CLARITYSTATUS"].Caption = "Clarity Status";
            //    GrdView.Columns["CUT"].Caption = "Cut";

            //    GrdView.Columns["POLISH"].Caption = "Polish";

            //    GrdView.Columns["SYMMETRY"].Caption = "Symm";
            //    GrdView.Columns["FLUORESCENCE"].Caption = "FL";
            //    GrdView.Columns["FLUORESCENCECOLOR"].Caption = "FL Color";

            //    GrdView.Columns["GIRDLEDESC"].Caption = "Girdle Desc";
            //    GrdView.Columns["GIRDLECONDITION"].Caption = "Grd Condition";
            //    GrdView.Columns["CULETSIZE"].Caption = "Culte Size";

            //    GrdView.Columns["DEPTHPER"].Caption = "Depth Per";
            //    GrdView.Columns["TABLE1"].Caption = "Table";
            //    GrdView.Columns["CRANGLE"].Caption = "Cr Angle";
            //    GrdView.Columns["CRHEIGHT"].Caption = "Cr Height";
            //    GrdView.Columns["PAVANGLE"].Caption = "Pav Angle";
            //    GrdView.Columns["PAVHEIGHT"].Caption = "Pav Height";

            //    GrdView.Columns["STARLENGTH"].Caption = "Str Length";
            //    GrdView.Columns["LOWERHALF"].Caption = "Lwr Half";
            //    GrdView.Columns["PAINTING"].Caption = "Painting";
            //    GrdView.Columns["PAINTCOMM"].Caption = "Paint Comm";
            //    GrdView.Columns["KEYTOSYMBOL"].Caption = "Key To Symbol";

            //    GrdView.Columns["REPORTCOMMENT"].Caption = "Report Comment";
            //    GrdView.Columns["INSCRIPTION"].Caption = "Inscription";
            //    GrdView.Columns["SYNTHETICINDICATOR"].Caption = "SyntheticIndicator";
            //    GrdView.Columns["GIRDLEPER"].Caption = "Girdle %";
            //    GrdView.Columns["POLISHFEATURES"].Caption = "Pol Features";
            //    GrdView.Columns["SYMMETRYFEATURES"].Caption = "Symm Features";
            //    GrdView.Columns["SHAPEDESC"].Caption = "Shape Desc";
            //    GrdView.Columns["REPORTTYPE"].Caption = "Report Type";
            //    GrdView.Columns["DIAMONDLINK"].Caption = "Diamond Link";

            //    GrdView.Columns["SPECIALCOMMENT"].Caption = "Special Comm.";
            //    GrdView.Columns["RESULT"].Caption = "Result";


            //}
            //else if (CmbLabType.Text == "SALES" || CmbLabType.Text == "BOMBAY")
            //{
            //    GrdView.Columns["KAPANNAME"].Caption = "Kapan";
            //    GrdView.Columns["PACKETNO"].Caption = "Packet No";
            //    GrdView.Columns["TAG"].Caption = "Tag";
            //    GrdView.Columns["UPLOADTYPE"].Caption = "Lab Type";
            //    GrdView.Columns["STATUS"].Caption = "Status";
            //    GrdView.Columns["SRNO"].Caption = "Sr No";
            //    GrdView.Columns["REPORTDATE"].Caption = "Sale Date";
            //    GrdView.Columns["CLIENTREFNO"].Caption = "Stone No";

            //    GrdView.Columns["GRDDONEBY"].Caption = "Done By";

            //    GrdView.Columns["SHAPE"].Caption = "Shape";
            //    GrdView.Columns["CARAT"].Caption = "Carat";
            //    GrdView.Columns["COLOR"].Caption = "Color";
            //    GrdView.Columns["CLARITY"].Caption = "Clarity";
            //    GrdView.Columns["CUT"].Caption = "Cut";

            //    GrdView.Columns["POLISH"].Caption = "Polish";

            //    GrdView.Columns["SYMMETRY"].Caption = "Symm";
            //    GrdView.Columns["FLUORESCENCE"].Caption = "FL";

            //    GrdView.Columns["RAPAPORT"].Caption = "Rapaport";
            //    GrdView.Columns["BACK"].Caption = "Back";
            //    GrdView.Columns["PRICEPERCARAT"].Caption = "PricePerCarat";
            //    GrdView.Columns["AMOUNT"].Caption = "Amount";

            //    GrdView.Columns["BACKDISC"].Caption = "BackDisc";

            //    GrdView.Columns["SALEPRICE"].Caption = "SalePrice";
            //    GrdView.Columns["SALEAMOUNT"].Caption = "SaleAmount";

            //    GrdView.Columns["LABCHARGE"].Caption = "Lab Charge";

            //    GrdView.Columns["REMARK"].Caption = "Remark";

            //    GrdView.Columns["BACKDISC"].Visible = CmbLabType.Text == "SALES" ? true : false; ;
            //    GrdView.Columns["SALEPRICE"].Visible = CmbLabType.Text == "SALES" ? true : false; ;
            //    GrdView.Columns["SALEAMOUNT"].Visible = CmbLabType.Text == "SALES" ? true : false; ;
            //    GrdView.Columns["LABCHARGE"].Visible = CmbLabType.Text == "SALES" ? true : false; ;

            //    GrdView.Columns["MILKY"].Visible = CmbLabType.Text == "BOMBAY" ? true : false;
            //    GrdView.Columns["LBLC"].Visible = CmbLabType.Text == "BOMBAY" ? true : false;
            //    GrdView.Columns["NATTS"].Visible = CmbLabType.Text == "BOMBAY" ? true : false;
            //    GrdView.Columns["HA"].Visible = CmbLabType.Text == "BOMBAY" ? true : false;
            //    GrdView.Columns["PAV"].Visible = CmbLabType.Text == "BOMBAY" ? true : false;
            //    GrdView.Columns["BINC"].Visible = CmbLabType.Text == "BOMBAY" ? true : false;
            //    GrdView.Columns["OINC"].Visible = CmbLabType.Text == "BOMBAY" ? true : false;
            //    GrdView.Columns["WINC"].Visible = CmbLabType.Text == "BOMBAY" ? true : false;
            //    GrdView.Columns["TENSION"].Visible = CmbLabType.Text == "BOMBAY" ? true : false;
            //    GrdView.Columns["CS"].Visible = CmbLabType.Text == "BOMBAY" ? true : false;
            //    GrdView.Columns["LUSTER"].Visible = CmbLabType.Text == "BOMBAY" ? true : false;
            //    GrdView.Columns["NATURAL"].Visible = CmbLabType.Text == "BOMBAY" ? true : false;
            //    GrdView.Columns["GRAIN"].Visible = CmbLabType.Text == "BOMBAY" ? true : false;
            //    GrdView.Columns["EYECLEAN"].Visible = CmbLabType.Text == "BOMBAY" ? true : false;
            //    GrdView.Columns["LAB"].Visible = CmbLabType.Text == "BOMBAY" ? true : false;

            //}
        }

        private void CmbLabType_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblDownload_Click(object sender, EventArgs e)
        {
            if (Val.ToString(CmbLabType.Text) == "GIA")
            {
                System.Diagnostics.Process.Start(Application.StartupPath + "\\Format\\GIAGrading.xlsx", "CMD");
            }
            else if (Val.ToString(CmbLabType.Text) == "IGI")
            {
                System.Diagnostics.Process.Start(Application.StartupPath + "\\Format\\IGIGrading.xlsx", "CMD");
            }
            else if (Val.ToString(CmbLabType.Text) == "BOMBAY")
            {
                System.Diagnostics.Process.Start(Application.StartupPath + "\\Format\\BYGradingFormat.xlsx", "CMD");
            }
            else if (Val.ToString(CmbLabType.Text) == "SALES")
            {
                System.Diagnostics.Process.Start(Application.StartupPath + "\\Format\\SaleUploadFormat.xlsx", "CMD");
            }
            else if (Val.ToString(CmbLabType.Text) == "HRD")
            {
                System.Diagnostics.Process.Start(Application.StartupPath + "\\Format\\HRDGrading.xlsx", "CMD");
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void GrdDetail_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }
                //if (Val.ToInt32(GrdDetail.GetRowCellValue(e.RowHandle, "ISEQUALSHAPE")) == 1 && (e.Column.FieldName == "SHAPENAME" || e.Column.FieldName == "SHAPE"))
                //{
                //    e.Appearance.BackColor = lblpara.BackColor;
                //}
                //if (Val.ToInt32(GrdDetail.GetRowCellValue(e.RowHandle, "ISEQUALCOL")) == 1 && (e.Column.FieldName == "COLORNAME" || e.Column.FieldName == "COLOR"))
                //{
                //    e.Appearance.BackColor = lblpara.BackColor;
                //}
                //if (Val.ToInt32(GrdDetail.GetRowCellValue(e.RowHandle, "ISEQUALCLA")) == 1 && (e.Column.FieldName == "CLARITYNAME" || e.Column.FieldName == "CLARITY"))
                //{
                //    e.Appearance.BackColor = lblpara.BackColor;
                //}
                //if (Val.ToInt32(GrdDetail.GetRowCellValue(e.RowHandle, "ISEQUALCUT")) == 1 && (e.Column.FieldName == "CUTNAME" || e.Column.FieldName == "CUT"))
                //{
                //    e.Appearance.BackColor = lblpara.BackColor;
                //}
                //if (Val.ToInt32(GrdDetail.GetRowCellValue(e.RowHandle, "ISEQUALPOL")) == 1 && (e.Column.FieldName == "POLNAME" || e.Column.FieldName == "POLISH"))
                //{
                //    e.Appearance.BackColor = lblpara.BackColor;
                //}
                //if (Val.ToInt32(GrdDetail.GetRowCellValue(e.RowHandle, "ISEQUALSYM")) == 1 && (e.Column.FieldName == "SYMMETRY" || e.Column.FieldName == "SYMNAME"))
                //{
                //    e.Appearance.BackColor = lblpara.BackColor;
                //}
                //if (Val.ToInt32(GrdDetail.GetRowCellValue(e.RowHandle, "ISEQUALFL")) == 1 && (e.Column.FieldName == "FLNAME" || e.Column.FieldName == "FLUORESCENCE"))
                //{
                //    e.Appearance.BackColor = lblpara.BackColor;
                //}
                //if (Val.ToInt32(GrdDetail.GetRowCellValue(e.RowHandle, "ISEQUALCARAT")) == 1 && (e.Column.FieldName == "STOCKCARAT" || e.Column.FieldName == "CARAT"))
                //{
                //    e.Appearance.BackColor = lblpara.BackColor;
                //}

                if (e.Column.FieldName == "SHAPE")
                {
                    int IntISSSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "SHAPENAMESEQ"));
                    int IntRETSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "SHAPESEQ"));
                    if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo > IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblUp
                            .BackColor;
                        e.Appearance.ForeColor = Color.Black;
                        //e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo < IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblDown.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                        //e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                }
                if (e.Column.FieldName == "COLOR")
                {
                    int IntISSSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "COLORNAMESEQ"));
                    int IntRETSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "COLORSEQ"));
                    if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo > IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblUp.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                        //e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo < IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblDown.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                        //e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                }
                if (e.Column.FieldName == "CLARITY")
                {
                    int IntISSSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "CLARITYNAMESEQ"));
                    int IntRETSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "CLARITYSEQ"));
                    if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo > IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblUp.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                        //e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo < IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblDown.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                        //e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                }
                if (e.Column.FieldName == "CUT")
                {
                    int IntISSSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "CUTNAMESEQ"));
                    int IntRETSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "CUTSEQ"));
                    if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo > IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblUp.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                        //e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo < IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblDown.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                        //e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                }
                if (e.Column.FieldName == "POLISH")
                {
                    int IntISSSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "POLNAMESEQ"));
                    int IntRETSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "POLISHSEQ"));
                    if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo > IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblUp.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                        //e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo < IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblDown.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                        //e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                }
                if (e.Column.FieldName == "SYMMETRY")
                {
                    int IntISSSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "SYMNAMESEQ"));
                    int IntRETSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "SYMMETRYSEQ"));
                    if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo > IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblUp.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                        //e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo < IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblDown.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                        //e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                }
                if (e.Column.FieldName == "FLUORESCENCE")
                {
                    int IntISSSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "FLNAMESEQ"));
                    int IntRETSeqNo = Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "FLUORESCENCESEQ"));
                    if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo > IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblUp.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                        //e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo < IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblDown.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                        //e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                }
                if (e.Column.FieldName == "CARAT")
                {
                    double IntISSSeqNo = Val.ToDouble(GrdDetail.GetRowCellValue(e.RowHandle, "STOCKCTS"));
                    double IntRETSeqNo = Val.ToDouble(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT"));
                    if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo < IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblUp.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                        //e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                    else if (IntRETSeqNo != 0 && IntISSSeqNo != 0 && IntRETSeqNo > IntISSSeqNo)
                    {
                        e.Appearance.BackColor = lblDown.BackColor;
                        e.Appearance.ForeColor = Color.Black;
                        //e.Appearance.FontStyleDelta = FontStyle.Bold;
                    }
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void MainGrid_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                GridControl gridC = sender as GridControl;
                GridView gridView = gridC.FocusedView as GridView;
                BandedGridViewInfo info = (BandedGridViewInfo)gridView.GetViewInfo();
                for (int i = 0; i < info.BandsInfo.BandCount; i++)
                {
                    e.Graphics.DrawLine(new Pen(Brushes.Black, 1), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.BandsInfo[i].Bounds.Top), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.RowsInfo[info.RowsInfo.Count - 1].Bounds.Bottom - 1));
                }

            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }


        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                GrdDetail.RefreshData();

                DataTable DTabSelected = GetTableOfSelectedRows(GrdDetail, true);

                if (DTabSelected.Rows.Count == 0)
                {
                    Global.Message("Please Select Atleast One Record For Print Request");
                    return;
                }


                foreach (DataRow DRow in DTabSelected.Rows)
                {
                    if ((Val.ToString(DRow["CONTROLNO"]) == "") && (Val.ToString(CmbLabType.SelectedItem) == "GIA"))
                    {
                        Global.Message("Control No Is Blank In This Packet No " + Val.ToString(DRow["CLIENTREFNO"]));
                        return;
                    }
                }

                if (Global.Confirm("Are You Sure To [PRINT] ?") == DialogResult.No)
                {
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                DTabSelected.TableName = "Table";
                string StrXMLValues = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabSelected.WriteXml(sw);
                    StrXMLValues = sw.ToString();
                }

                int IntResult = ObjUpload.SaveGIA(StrXMLValues, "PRINT", "", "", "", "");

                if (IntResult != -1)
                {
                    Global.Message("Record Inserte Sucessfully");
                }
                else
                {
                    Global.Message("Something Wrong...");
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void BtnPrintAndInscripion_Click(object sender, EventArgs e)
        {
            GrpInscription.BringToFront();
            GrpInscription.Visible = true;
        }

        private void BtnPrintInscription_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable DTabSelected = GetTableOfSelectedRows(GrdDetail, true);

                if (DTabSelected.Rows.Count == 0)
                {
                    Global.Message("Please Select Atleast One Record For Print Request");
                    return;
                }


                foreach (DataRow DRow in DTabSelected.Rows)
                {
                    if (Val.ToString(DRow["CONTROLNO"]) == "")
                    {
                        Global.Message("Control No Is Blank In This Packet No " + Val.ToString(DRow["CLIENTREFNO"]));
                        return;
                    }
                }

                if (Global.Confirm("Are You Sure To [INSCRIPTION AND PRINT] ?") == DialogResult.No)
                {
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                DTabSelected.TableName = "Table";
                string StrXMLValues = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabSelected.WriteXml(sw);
                    StrXMLValues = sw.ToString();
                }

                int IntResult = ObjUpload.SaveGIA(StrXMLValues, "INSCRIBE AND PRINT", Val.ToString(txtServiceCode.Text), Val.ToString(txtInscriptionText.Text), Val.ToString(txtClientComment.Text), "");

                if (IntResult != 0)
                {
                    Global.Message("Record Inserte Sucessfully");
                }
                else
                {
                    Global.Message("Something Wrong...");
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void txtServiceCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "INSCRIPTIONNAME,INSCRIPTIONCODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_INSCRIPTIONCODE);

                    FrmSearch.mStrColumnsToHide = "INSCRIPTION_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtServiceCode.Text = Val.ToString(FrmSearch.DRow["INSCRIPTIONCODE"]);
                        txtServiceCode.Tag = Val.ToString(FrmSearch.DRow["INSCRIPTION_ID"]);
                    }

                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
            }
        }

        private void txtRecheckCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "RECHECKNAME,RECHECKCODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_RECHECKCODE);

                    FrmSearch.mStrColumnsToHide = "RECHECK_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtRecheckCode.Text = Val.ToString(FrmSearch.DRow["RECHECKCODE"]);
                        txtRecheckCode.Tag = Val.ToString(FrmSearch.DRow["RECHECK_ID"]);
                    }

                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
            }
        }

        private void BtnCloseInscription_Click(object sender, EventArgs e)
        {
            txtServiceCode.Text = "";
            txtServiceCode.Tag = "";

            txtInscriptionText.Text = "Inscription check";
            txtClientComment.Text = "Inscribe and Print";
            GrpInscription.Visible = false;
        }

        private void BtnCloseRecheck_Click(object sender, EventArgs e)
        {
            txtRecheckCode.Text = "";
            txtRecheckCode.Tag = "";

            GrpRecheck.Visible = false;
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable DTabSelected = GetTableOfSelectedRows(GrdDetail, true);

                if (DTabSelected.Rows.Count == 0)
                {
                    Global.Message("Please Select Atleast One Record For Print Request");
                    return;
                }


                foreach (DataRow DRow in DTabSelected.Rows)
                {
                    if (Val.ToString(DRow["CONTROLNO"]) == "")
                    {
                        Global.Message("Control No Is Blank In This Packet No " + Val.ToString(DRow["CLIENTREFNO"]));
                        return;
                    }
                }

                if (Global.Confirm("Are You Sure To [Recheck] ?") == DialogResult.No)
                {
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                DTabSelected.TableName = "Table";
                string StrXMLValues = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabSelected.WriteXml(sw);
                    StrXMLValues = sw.ToString();
                }

                int IntResult = ObjUpload.SaveGIA(StrXMLValues, "RECHECK", Val.ToString(txtServiceCode.Text), "", Val.ToString(txtClientComment.Text), Val.ToString(txtRecheckCode.Text));

                if (IntResult != 0)
                {
                    Global.Message("Record Inserte Sucessfully");
                }
                else
                {
                    Global.Message("Something Wrong...");
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void btnRecheck_Click(object sender, EventArgs e)
        {
            txtRecheckCode.Text = "";
            txtRecheckCode.Tag = "";

            GrpRecheck.Visible = true;
        }

        private void lblSaveLayout_Click(object sender, EventArgs e) // K : 22/12/2022
        {
            try
            {
                Stream str = new System.IO.MemoryStream();
                GrdDetail.SaveLayoutToStream(str);
                str.Seek(0, System.IO.SeekOrigin.Begin);
                StreamReader reader = new StreamReader(str);
                string text = reader.ReadToEnd();

                int IntRes = new BOTRN_StockUpload().SaveGridLayout(this.Name, GrdDetail.Name, text);
                if (IntRes != -1)
                {
                    Global.Message("Layout Successfully Saved");
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void lblDefaultLayout_Click(object sender, EventArgs e) // K : 22/12/2022
        {
            try
            {
                int IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdDetail.Name);
                if (IntRes != -1)
                {
                    Global.Message("Layout Successfully Deleted");
                }
            }
            catch (Exception ex)
            {
            }
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

        #endregion

        private void MainGrid_Click(object sender, EventArgs e)
        {

        }
    }
}

