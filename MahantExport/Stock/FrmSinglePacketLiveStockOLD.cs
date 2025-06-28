using BusLib.Configuration;
using BusLib.Rapaport;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.XtraEditors;
using OfficeOpenXml;
using MahantExport.Utility;
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
using System.Drawing.Printing;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using System.Runtime.InteropServices;

namespace MahantExport.Stock
{
    public partial class FrmSinglePacketLiveStockOLD : DevExpress.XtraEditors.XtraForm
    {
        [DllImport("Winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool SetDefaultPrinter(string printerName);
        private string GetDefaultPrinter()
        {
            PrinterSettings settings = new PrinterSettings();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                settings.PrinterName = printer;
                if (settings.IsDefaultPrinter)
                    return printer;
            }
            return string.Empty;
        }
        String PasteData = "";
        IDataObject PasteclipData = Clipboard.GetDataObject();

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();

        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        BOTRN_SingleFileUpload ObjUpload = new BOTRN_SingleFileUpload();
        BOFindRap ObjRap = new BOFindRap();

        LiveStockProperty mProperty = new LiveStockProperty();

        BODevGridSelection ObjGridSelection;

        DataTable DTabParameter = new DataTable();
        DataTable DTabDiamondType = new DataTable();

        DataTable DtabLiveStockDetail = new DataTable();
        DataTable DTabSize = new DataTable();

        Color mSelectedColor = Color.FromArgb(192, 0, 0);
        Color mDeSelectColor = Color.Black;
        Color mSelectedBackColor = Color.FromArgb(255, 224, 192);
        Color mDSelectedBackColor = Color.WhiteSmoke;

        string StrEmail = "";
        string mStrStockType = "";
        bool chkOnAndOff;
        string pStrDiamondType = "";

        

        public FrmSinglePacketLiveStockOLD()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            if (MainGrdDetail.RepositoryItems.Count == 9)
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
            GrdDetail.Columns["COLSELECTCHECKBOX"].Fixed = DevExpress.XtraGrid.Columns.FixedStyle.Left;
            if (ObjGridSelection != null)
            {
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            }

            //CmbDiamondType.SelectedIndex = 0;

            string Str = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdDetail.Name);

            if (Str != "")
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                MemoryStream stream = new MemoryStream(byteArray);
                GrdDetail.RestoreLayoutFromStream(stream);
            }
            RbtBarcode_CheckedChanged(null,null);

            this.Show();
            SetControl();
        }

        #region DesignDynamiButtons

        public void DesignSystemButtion(Panel PNL, string pStrParaType, string pStrDisplayText, string toolTips, int pIntHeight, int pIntWidth)
        {
            DataRow[] UDRow = DTabParameter.Select("ParaType = '" + pStrParaType + "'");

            if (UDRow.Length == 0)
            {
                return;
            }

            DataTable DTab = UDRow.CopyToDataTable();
            DTab.DefaultView.Sort = "SequenceNo";
            DTab = DTab.DefaultView.ToTable();

            PNL.Controls.Clear();

            int IntI = 0;
            foreach (DataRow DRow in DTab.Rows)
            {
                AxonContLib.cButton ValueList = new AxonContLib.cButton();
                ValueList.Text = DRow[pStrDisplayText].ToString();
                ValueList.FlatStyle = FlatStyle.Flat;
                ValueList.Width = pIntWidth;
                ValueList.Height = pIntHeight;
                ValueList.Tag = DRow["PARA_ID"].ToString();
                ValueList.AccessibleDescription = Val.ToString(DRow["PARACODE"]);
                ValueList.ToolTips = toolTips;
                ValueList.AutoSize = true;
                ValueList.Click += new EventHandler(cButton_Click);
                ValueList.Cursor = Cursors.Hand;
                ValueList.Font = new Font("Tahoma", 9, FontStyle.Regular);
                ValueList.ForeColor = mDeSelectColor;
                ValueList.BackColor = mDSelectedBackColor;

                PNL.Controls.Add(ValueList);

                IntI++;
            }
        }

        private void cButton_Click(object sender, EventArgs e)
        {
            try
            {
                AxonContLib.cButton btn = (AxonContLib.cButton)sender;
                if (btn.ForeColor == mSelectedColor)
                {
                    btn.ForeColor = mDeSelectColor;
                    btn.BackColor = mDSelectedBackColor;
                    btn.AccessibleName = "true";
                }
                else
                {
                    btn.ForeColor = mSelectedColor;
                    btn.BackColor = mSelectedBackColor;
                    btn.AccessibleName = "true";
                }
                if (MainGrdDetail.Enabled == false)
                {
                    Global.MessageError("Grid Is Unable To Update");
                    return;
                }
            }
            catch (Exception EX)
            {
                this.Cursor = Cursors.WaitCursor;
                Global.MessageError(EX.Message);
                return;
            }

        }

        private void SetControl()
        {
            DTabParameter = ObjRap.GetAllParameterTable();

            DesignSystemButtion(PanelShape, "SHAPE", "PARACODE", "SHAPE", 16, 45);
            DesignSystemButtion(PanelColor, "COLOR", "PARANAME", "COLOR", 16, 45);
            DesignSystemButtion(PanelClarity, "CLARITY", "PARANAME", "CLARITY", 14, 45);
            DesignSystemButtion(PanelCut, "CUT", "PARACODE", "CUT", 14, 45);
            DesignSystemButtion(PanelPol, "POLISH", "PARACODE", "POL", 16, 45);
            DesignSystemButtion(PanelSym, "SYMMETRY", "PARACODE", "SYM", 14, 45);
            DesignSystemButtion(PanelFL, "FLUORESCENCE", "PARANAME", "FL", 14, 45);


            DTabDiamondType = new DataTable();
            DTabDiamondType.Columns.Add(new DataColumn("TYPE", typeof(string)));
            DTabDiamondType.Columns.Add(new DataColumn("VALUE", typeof(string)));

            DataRow row = DTabDiamondType.NewRow();
            row["TYPE"] = "ALL";
            row["VALUE"] = "ALL";
            DTabDiamondType.Rows.Add(row);

            DataRow row1 = DTabDiamondType.NewRow();
            row1["TYPE"] = "NAT";
            row1["VALUE"] = "NATURAL";
            DTabDiamondType.Rows.Add(row1);

            DataRow row2 = DTabDiamondType.NewRow();
            row2["TYPE"] = "CVD";
            row2["VALUE"] = "CVD";
            DTabDiamondType.Rows.Add(row2);

            DataRow row3 = DTabDiamondType.NewRow();
            row3["TYPE"] = "HPHT";
            row3["VALUE"] = "HPHT";
            DTabDiamondType.Rows.Add(row3);

            DesignSystemDiamondTypeButtion(DTabDiamondType, PanelDiamondType, "TYPE", "TYPE", 26, 45);

        }

        public void DesignSystemDiamondTypeButtion(DataTable DTab, Panel PNL, string pStrDisplayText, string toolTips, int pIntHeight, int pIntWidth)
        {

            if (DTab.Rows.Count == 0)
            {
                return;
            }

            PNL.Controls.Clear();

            int IntI = 0;
            foreach (DataRow DRow in DTab.Rows)
            {
                AxonContLib.cButton ValueList = new AxonContLib.cButton();
                ValueList.Text = DRow[pStrDisplayText].ToString();
                ValueList.FlatStyle = FlatStyle.Flat;
                ValueList.Width = pIntWidth;
                ValueList.Height = pIntHeight;
                ValueList.Tag = DRow["VALUE"].ToString();
                ValueList.AccessibleDescription = Val.ToString(DRow["TYPE"]);
                ValueList.ToolTips = toolTips;
                ValueList.AutoSize = true;
                ValueList.Click += new EventHandler(cButton_Click);
                ValueList.Cursor = Cursors.Hand;
                ValueList.Font = new Font("Tahoma", 9, FontStyle.Regular);
                ValueList.ForeColor = mDeSelectColor;
                ValueList.BackColor = mDSelectedBackColor;

                PNL.Controls.Add(ValueList);

                IntI++;
            }
        }

        #endregion
        public string GetSelectedBtnID(Panel StrPanel)
        {
            string StrSelectedID = "";
            for (int i = 0; i < StrPanel.Controls.Count; i++)
            {
                if (StrPanel.Controls[i].BackColor == mSelectedBackColor)
                {
                    StrSelectedID += StrPanel.Controls[i].Tag + ",";
                }
            }
            return StrSelectedID;
        }
        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjStock);
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string StockType = "";
                bool StrLabStock = false;

                DataSet DsLiveStock = new DataSet();

                mProperty.MULTYSHAPE_ID = GetSelectedBtnID(PanelShape);
                mProperty.MULTYCOLOR_ID = GetSelectedBtnID(PanelColor);
                mProperty.MULTYCLARITY_ID = GetSelectedBtnID(PanelClarity);
                mProperty.MULTYCUT_ID = GetSelectedBtnID(PanelCut);
                mProperty.MULTYSYM_ID = GetSelectedBtnID(PanelSym);
                mProperty.MULTYPOL_ID = GetSelectedBtnID(PanelPol);
                mProperty.MULTYFL_ID = GetSelectedBtnID(PanelFL);

                mProperty.BARCODE = Val.ToString(txtBarcode.Text);
                mProperty.STOCKNO = Val.ToString(txtStoneNo.Text);

                mProperty.DIAMONDTYPE = GetSelectedBtnID(PanelDiamondType);

                if (mProperty.DIAMONDTYPE.EndsWith(","))
                {
                    mProperty.DIAMONDTYPE = mProperty.DIAMONDTYPE.Substring(0, mProperty.DIAMONDTYPE.Length - 1);
                }

                if(RbtFullStock.Checked == true)
                {
                    StockType = "FULLSTOCK";
                }
                else
                {
                    StockType = "MYSTOCK";
                }                
                
                StrLabStock = Val.ToBoolean(ChkIsLabStock.Checked);

                //mProperty.DIAMONDTYPE = pStrDiamondType;
                mProperty.ISUNGRADEDTOMIX = Val.ToBoolean(ChkISMixStones.Checked);

                DsLiveStock = ObjStock.GetNewLiveStockDataNew(mProperty, "All", StockType, StrLabStock);
                DtabLiveStockDetail = DsLiveStock.Tables[0];

                DtabLiveStockDetail.DefaultView.Sort = "SrNo";
                DtabLiveStockDetail = DtabLiveStockDetail.DefaultView.ToTable();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                MainGrdDetail.DataSource = DtabLiveStockDetail;
                progressPanel1.Visible = false;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
            //pStrDiamondType = Val.ToString(CmbDiamondType.SelectedItem);
            progressPanel1.Visible = true;
            backgroundWorker1.RunWorkerAsync();
        }

        #region ExcelExport

        public void AddProportionDetail(ExcelWorksheet worksheet, DataTable pDtabGroup, string SheetName, int Row, int Column,
          string pStrHeader, string pStrTitle,
          string pStrGroupColumn,
          string StrStartRow,
          string StrEndRow,
          DataTable pDtabDetail
          )
        {
            Color BackColor = Color.FromArgb(2, 68, 143);
            Color FontColor = Color.White;
            string FontName = "Calibri";
            float FontSize = 9;

            int StartRow = Row;
            int StartColumn = Column;

            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Value = pStrHeader;
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Merge = true;
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Font.Name = FontName;
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Font.Size = 12;

            StartRow = StartRow + 1;
            worksheet.Cells[StartRow, Column, StartRow, Column].Value = pStrTitle;
            worksheet.Cells[StartRow, Column + 1, StartRow, Column + 1].Value = "Pcs";
            worksheet.Cells[StartRow, Column + 2, StartRow, Column + 2].Value = "Carat";
            worksheet.Cells[StartRow, Column + 3, StartRow, Column + 3].Value = "Rap %";
            worksheet.Cells[StartRow, Column + 4, StartRow, Column + 4].Value = "Amount";
            worksheet.Cells[StartRow, Column + 5, StartRow, Column + 5].Value = "Rap Value";
            worksheet.Cells[StartRow, Column + 6, StartRow, Column + 6].Value = "%";

            StartRow = StartRow + 1;

            int IntSizeStartRow = StartRow;
            int IntSizeEndRow = StartRow + pDtabGroup.Rows.Count - 1;
            int IntSizeStartColumn = Row;
            int IntSizeEndColumn = Column + 6;

            string GroupCol = Global.ColumnIndexToColumnLetter(pDtabDetail.Columns[pStrGroupColumn].Ordinal + 1);
            string CaratCol = Global.ColumnIndexToColumnLetter(pDtabDetail.Columns["Carat"].Ordinal + 1);
            string AmountCol = Global.ColumnIndexToColumnLetter(pDtabDetail.Columns["Amount"].Ordinal + 1);
            string RapAmountCol = Global.ColumnIndexToColumnLetter(pDtabDetail.Columns["RapValue"].Ordinal + 1);

            string FormulaCol = "'" + SheetName + "'!$" + GroupCol + "$" + StrStartRow + ":$" + GroupCol + "$" + StrEndRow + "";
            string FormulaCaratCol = "'" + SheetName + "'!$" + CaratCol + "$" + StrStartRow + ":$" + CaratCol + "$" + StrEndRow + "";
            string FormulaAmountCol = "'" + SheetName + "'!$" + AmountCol + "$" + StrStartRow + ":$" + AmountCol + "$" + StrEndRow + "";
            string FormulaRapAmountCol = "'" + SheetName + "'!$" + RapAmountCol + "$" + StrStartRow + ":$" + RapAmountCol + "$" + StrEndRow + "";

            string SumGrpCol = Global.ColumnIndexToColumnLetter(Column);
            string SumPcsCol = Global.ColumnIndexToColumnLetter(Column + 1);
            string SumCaratCol = Global.ColumnIndexToColumnLetter(Column + 2);
            string SumRapPerCol = Global.ColumnIndexToColumnLetter(Column + 3);
            string SumAmountCol = Global.ColumnIndexToColumnLetter(Column + 4);
            string SumRapAmountCol = Global.ColumnIndexToColumnLetter(Column + 5);
            string SumPerCol = Global.ColumnIndexToColumnLetter(Column + 6);

            foreach (DataRow DRow in pDtabGroup.Rows)
            {
                worksheet.Cells[StartRow, Column, StartRow, Column].Value = Val.ToString(DRow[0]);

                //PCS
                worksheet.Cells[StartRow, Column + 1, StartRow, Column + 1].Formula = "SUMPRODUCT(SUBTOTAL(3,OFFSET(" + FormulaCol + ",ROW(" + FormulaCol + ")-MIN(ROW(" + FormulaCol + ")),,1)),--(" + FormulaCol + "=" + SumGrpCol + "" + StartRow + "))";
                worksheet.Cells[StartRow, Column + 2, StartRow, Column + 2].Formula = "SUMPRODUCT(SUBTOTAL(3,OFFSET(" + FormulaCol + ",ROW(" + FormulaCol + ")-MIN(ROW(" + FormulaCol + ")),,1)),--(" + FormulaCol + "=" + SumGrpCol + "" + StartRow + "),(" + FormulaCaratCol + "))";
                //Rap %
                worksheet.Cells[StartRow, Column + 3, StartRow, Column + 3].Formula = "IF(" + SumRapAmountCol + "" + StartRow + ">0,ROUND(SUM(((" + SumAmountCol + "" + StartRow + ")/((" + SumRapAmountCol + "" + StartRow + "*1)))*100),2)-100,)";

                // Amount
                worksheet.Cells[StartRow, Column + 4, StartRow, Column + 4].Formula = "SUMPRODUCT(SUBTOTAL(3,OFFSET(" + FormulaCol + ",ROW(" + FormulaCol + ")-MIN(ROW(" + FormulaCol + ")),,1)),--(" + FormulaCol + "=" + SumGrpCol + "" + StartRow + "),(" + FormulaAmountCol + "))";

                // Rap
                worksheet.Cells[StartRow, Column + 5, StartRow, Column + 5].Formula = "SUMPRODUCT(SUBTOTAL(3,OFFSET(" + FormulaCol + ",ROW(" + FormulaCol + ")-MIN(ROW(" + FormulaCol + ")),,1)),--(" + FormulaCol + "=" + SumGrpCol + "" + StartRow + "),(" + FormulaRapAmountCol + "))";

                //Per
                worksheet.Cells[StartRow, Column + 6, StartRow, Column + 6].Formula = "" + SumPcsCol + "" + StartRow + "/$" + SumPcsCol + "$" + (Val.ToInt(IntSizeStartRow) + pDtabGroup.Rows.Count) + "";
                worksheet.Cells[StartRow, Column + 6, StartRow, Column + 6].Style.Numberformat.Format = "0.00%";

                StartRow = StartRow + 1;
            }

            // Rap Amount Column
            worksheet.Column(Column + 5).OutlineLevel = 1;
            worksheet.Column(Column + 5).Collapsed = true;

            worksheet.Cells[StartRow, Column, StartRow, Column].Value = "Total";
            worksheet.Cells[StartRow, Column + 1, StartRow, Column + 1].Formula = "SUM(" + SumPcsCol + "" + IntSizeStartRow + ":" + SumPcsCol + "" + IntSizeEndRow + ")";
            worksheet.Cells[StartRow, Column + 2, StartRow, Column + 2].Formula = "SUM(" + SumCaratCol + "" + IntSizeStartRow + ":" + SumCaratCol + "" + IntSizeEndRow + ")";
            worksheet.Cells[StartRow, Column + 3, StartRow, Column + 3].Formula = "=IF(" + SumRapAmountCol + "" + StartRow + ">0,ROUND(SUM(((" + SumAmountCol + "" + StartRow + ")/((" + SumRapAmountCol + "" + StartRow + "*1)))*100),2)-100,)";
            worksheet.Cells[StartRow, Column + 4, StartRow, Column + 4].Formula = "SUM(" + SumAmountCol + "" + IntSizeStartRow + ":" + SumAmountCol + "" + IntSizeEndRow + ")";
            worksheet.Cells[StartRow, Column + 5, StartRow, Column + 5].Formula = "SUM(" + SumRapAmountCol + "" + IntSizeStartRow + ":" + SumRapAmountCol + "" + IntSizeEndRow + ")";
            worksheet.Cells[StartRow, Column + 6, StartRow, Column + 6].Formula = "SUM(" + SumPerCol + "" + IntSizeStartRow + ":" + SumPerCol + "" + IntSizeEndRow + ")";
            worksheet.Cells[StartRow, Column + 6, StartRow, Column + 6].Style.Numberformat.Format = "0.00%";


            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Cells[Row + 2, Column + 1, StartRow, Column + 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.Font.Name = FontName;
            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.Font.Size = FontSize;

            //Header
            worksheet.Cells[Row, Column, Row + 1, Column + 6].Style.Font.Bold = true;
            worksheet.Cells[Row, Column, Row + 1, Column + 6].Style.Font.Color.SetColor(FontColor);
            worksheet.Cells[Row, Column, Row + 1, Column + 6].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[Row, Column, Row + 1, Column + 6].Style.Fill.PatternColor.SetColor(BackColor);
            worksheet.Cells[Row, Column, Row + 1, Column + 6].Style.Fill.BackgroundColor.SetColor(BackColor);

            // Footer
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Font.Bold = true;
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Font.Color.SetColor(FontColor);
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Fill.PatternColor.SetColor(BackColor);
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Fill.BackgroundColor.SetColor(BackColor);

            //Left First Column
            worksheet.Cells[Row, Column, StartRow, Column].Style.Font.Bold = true;
            worksheet.Cells[Row, Column, StartRow, Column].Style.Font.Color.SetColor(FontColor);
            worksheet.Cells[Row, Column, StartRow, Column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[Row, Column, StartRow, Column].Style.Fill.PatternColor.SetColor(BackColor);
            worksheet.Cells[Row, Column, StartRow, Column].Style.Fill.BackgroundColor.SetColor(BackColor);

        }

        public string ExportExcelWithExp(DataSet DS, string PStrFilePath) //Add Khushbu 15-05-21
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

                DTabDetail.DefaultView.Sort = "SrNo";
                DTabDetail = DTabDetail.DefaultView.ToTable();

                this.Cursor = Cursors.WaitCursor;

                string StrFilePath = PStrFilePath;

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.FromArgb(2, 68, 143);
                Color FontColor = Color.White;
                string FontName = "Calibri";
                float FontSize = 9;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("SKE_Stock_" + DateTime.Now.ToString("ddMMyyyy"));

                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    #region Add Image

                    Image img = Image.FromFile(Application.StartupPath + "//logo.jpg");
                    OfficeOpenXml.Drawing.ExcelPicture pic = worksheet.Drawings.AddPicture("Logo", img);
                    pic.SetPosition(2, 23);
                    pic.SetSize(100, 55);

                    worksheet.Cells[1, 1, 3, 3].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                    worksheet.Cells[1, 1, 3, 3].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                    worksheet.Cells[1, 1, 3, 3].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                    worksheet.Cells[1, 1, 3, 3].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                    worksheet.Cells[1, 1, 3, 3].Merge = true;

                    #endregion

                    #region Stock Detail

                    StartRow = 5;
                    EndRow = StartRow + DTabDetail.Rows.Count;
                    StartColumn = 1;
                    EndColumn = DTabDetail.Columns.Count;

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


                    int RxWColumn = DTabDetail.Columns["RW"].Ordinal + 1;
                    int RapaportColumn = DTabDetail.Columns["RapPrice"].Ordinal + 1;
                    int PricePerCaratColumn = DTabDetail.Columns["NetRate"].Ordinal + 1;
                    int DiscountColumn = DTabDetail.Columns["Disc"].Ordinal + 1;
                    int CaratColumn = DTabDetail.Columns["Carats"].Ordinal + 1;
                    int AmountColumn = DTabDetail.Columns["NetValue"].Ordinal + 1;

                    for (int IntI = 6; IntI <= EndRow; IntI++)
                    {
                        string RapColumns = Global.ColumnIndexToColumnLetter(RapaportColumn) + IntI.ToString();
                        string Discount = Global.ColumnIndexToColumnLetter(DiscountColumn) + IntI.ToString();
                        string Carat = Global.ColumnIndexToColumnLetter(CaratColumn) + IntI.ToString();
                        string PricePerCarat = Global.ColumnIndexToColumnLetter(PricePerCaratColumn) + IntI.ToString();

                        worksheet.Cells[IntI, RxWColumn].Formula = "=ROUND(" + RapColumns + " * " + Carat + ",2)";

                        worksheet.Cells[IntI, PricePerCaratColumn].Formula = "=ROUND( (100 +" + Discount + ") * " + RapColumns + "/100,2)";
                        worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";

                    }


                    int IntRowStartsFrom = 3;
                    int IntRowEndTo = (DTabDetail.Rows.Count - 1 + IntRowStartsFrom);

                    int SrNo = 0, CaratNo = 0, AmountNo = 0, RapAmountNo = 0;

                    DataColumnCollection columns = DTabDetail.Columns;

                    if (columns.Contains("SrNo"))
                        SrNo = DTabDetail.Columns["SrNo"].Ordinal + 1;
                    if (columns.Contains("Carats"))
                        CaratNo = DTabDetail.Columns["Carats"].Ordinal + 1;
                    if (columns.Contains("RW"))
                        RapAmountNo = DTabDetail.Columns["RW"].Ordinal + 1;
                    if (columns.Contains("NetValue"))
                        AmountNo = DTabDetail.Columns["NetValue"].Ordinal + 1;

                    string StrStartRow = "6";
                    string StrEndRow = EndRow.ToString();

                    #region Top Formula


                    worksheet.Cells[1, 5, 1, 5].Value = "Pcs";
                    worksheet.Cells[1, 6, 1, 6].Value = "Carat";
                    worksheet.Cells[1, 17, 1, 17].Value = "Rap Value";
                    worksheet.Cells[1, 18, 1, 18].Value = "Rap %";
                    worksheet.Cells[1, 19, 1, 19].Value = "Pr/Ct";
                    worksheet.Cells[1, 20, 1, 20].Value = "Amount";

                    worksheet.Cells[5, 2, 5, 2].Value = "Stone No";
                    worksheet.Cells[5, 4, 5, 4].Value = "Report No";
                    worksheet.Cells[5, 12, 5, 12].Value = "Flour.";
                    worksheet.Cells[5, 14, 5, 14].Value = "Depth%";
                    worksheet.Cells[5, 15, 5, 15].Value = "Table%";
                    worksheet.Cells[5, 16, 5, 16].Value = "Rap. Price";
                    worksheet.Cells[5, 17, 5, 17].Value = "RxW";
                    worksheet.Cells[5, 18, 5, 18].Value = "Disc%";
                    worksheet.Cells[5, 19, 5, 19].Value = "Net Rate";
                    worksheet.Cells[5, 20, 5, 20].Value = "Net Value";

                    worksheet.Cells[2, 4, 2, 4].Value = "Total";
                    worksheet.Cells[3, 4, 3, 4].Value = "Selected";

                    worksheet.Cells[1, 7, 3, 16].Merge = true;
                    worksheet.Cells[1, 7, 3, 16].Value = "Note : Use filter to select stones and Check your ObjGridSelection Avg Disc and Total amt.";
                    worksheet.Cells[1, 7, 3, 16].Style.WrapText = true;

                    string S = Global.ColumnIndexToColumnLetter(SrNo) + StrStartRow;
                    string E = Global.ColumnIndexToColumnLetter(SrNo) + StrEndRow;
                    worksheet.Cells[2, 5, 2, 5].Formula = "ROUND(COUNTA(" + S + ":" + E + "),2)";
                    worksheet.Cells[3, 5, 3, 5].Formula = "ROUND(SUBTOTAL(3," + S + ":" + E + "),2)";


                    S = Global.ColumnIndexToColumnLetter(CaratNo) + StrStartRow;
                    E = Global.ColumnIndexToColumnLetter(CaratNo) + StrEndRow;
                    worksheet.Cells[2, 6, 2, 6].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                    worksheet.Cells[3, 6, 3, 6].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                    S = Global.ColumnIndexToColumnLetter(RapAmountNo) + StrStartRow;
                    E = Global.ColumnIndexToColumnLetter(RapAmountNo) + StrEndRow;
                    worksheet.Cells[2, 17, 2, 17].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                    worksheet.Cells[3, 17, 3, 17].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";


                    S = Global.ColumnIndexToColumnLetter(AmountNo) + StrStartRow;
                    E = Global.ColumnIndexToColumnLetter(AmountNo) + StrEndRow;
                    worksheet.Cells[2, 20, 2, 20].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                    worksheet.Cells[3, 20, 3, 20].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";


                    worksheet.Cells[2, 19, 2, 19].Formula = "ROUND(T2/F2,2)";
                    worksheet.Cells[3, 19, 3, 19].Formula = "ROUND(T3/F3,2)";


                    S = Global.ColumnIndexToColumnLetter(AmountNo) + StrStartRow;
                    E = Global.ColumnIndexToColumnLetter(AmountNo) + StrEndRow;

                    worksheet.Cells[2, 18, 2, 18].Formula = "ROUND(SUM(((T2)/((Q2*1)))*100),2)-100";
                    worksheet.Cells[3, 18, 3, 18].Formula = "ROUND(SUM(((T3)/((Q3*1)))*100),2)-100";


                    worksheet.Cells[1, 4, 4, 20].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 4, 4, 20].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 4, 4, 20].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, 4, 4, 20].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, 4, 4, 20].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, 4, 4, 20].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, 4, 4, 20].Style.Font.Name = "Calibri";
                    worksheet.Cells[1, 4, 4, 20].Style.Font.Size = 9;

                    worksheet.Cells[1, 4, 1, 20].Style.Font.Bold = true;
                    worksheet.Cells[1, 4, 1, 20].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, 4, 1, 20].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, 4, 1, 20].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[1, 4, 1, 20].Style.Fill.BackgroundColor.SetColor(BackColor);

                    worksheet.Cells[1, 4, 3, 4].Style.Font.Bold = true;
                    worksheet.Cells[1, 4, 3, 4].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, 4, 3, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, 4, 3, 4].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[1, 4, 3, 4].Style.Fill.BackgroundColor.SetColor(BackColor);

                    worksheet.Cells[1, 7, 3, 10].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, 7, 3, 10].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[1, 7, 3, 10].Style.Fill.BackgroundColor.SetColor(BackColor);

                    #endregion

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
        public string ExportExcelWithStockList(DataSet DS, string PStrFilePath) //Add Khushbu 12-07-21
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

                DTabDetail.DefaultView.Sort = "SrNo";
                DTabDetail = DTabDetail.DefaultView.ToTable();

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
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Full Stock List");

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


                    int RxWColumn = DTabDetail.Columns["RxW"].Ordinal + 1;
                    int RapaportColumn = DTabDetail.Columns["Rap. Price"].Ordinal + 1;
                    int PricePerCaratColumn = DTabDetail.Columns["Net Rate"].Ordinal + 1;
                    int DiscountColumn = DTabDetail.Columns["Disc%"].Ordinal + 1;
                    int CaratColumn = DTabDetail.Columns["Carats"].Ordinal + 1;
                    int AmountColumn = DTabDetail.Columns["Net Value"].Ordinal + 1;
                    int VideoLinkColumn = DTabDetail.Columns["Video Link"].Ordinal + 1;
                    int VideoColumn = DTabDetail.Columns["Video"].Ordinal + 1;
                    int DepthPerColumn = DTabDetail.Columns["Depth%"].Ordinal + 1;
                    int TablePerColumn = DTabDetail.Columns["Table%"].Ordinal + 1;
                    int ShapeColumn = DTabDetail.Columns["Shape"].Ordinal + 1;

                    int GirdlePerColumn = DTabDetail.Columns["Girldle Per"].Ordinal + 1;
                    int CAColumn = DTabDetail.Columns["Crown Angle"].Ordinal + 1;



                    for (int IntI = 2; IntI <= EndRow; IntI++)
                    {
                        string RapColumns = Global.ColumnIndexToColumnLetter(RapaportColumn) + IntI.ToString();
                        string Discount = Global.ColumnIndexToColumnLetter(DiscountColumn) + IntI.ToString();
                        string Carat = Global.ColumnIndexToColumnLetter(CaratColumn) + IntI.ToString();
                        string PricePerCarat = Global.ColumnIndexToColumnLetter(PricePerCaratColumn) + IntI.ToString();
                        string VideoLink = Global.ColumnIndexToColumnLetter(VideoLinkColumn) + IntI.ToString();

                        worksheet.Cells[IntI, RxWColumn].Formula = "=ROUND(" + RapColumns + " * " + Carat + ",2)";

                        if (Val.ToString(DTabDetail.Rows[IntI - 2]["FANCYCOLOR"]) == "") //Add if condition khushbu 08-10-21 for skip formula in fancy color
                        {
                            worksheet.Cells[IntI, PricePerCaratColumn].Formula = "=(" + RapColumns + " + ((" + RapColumns + " * " + Discount + ") / 100))";
                            worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                            worksheet.Cells[IntI, AmountColumn].Style.Font.Bold = true;
                        }
                        else
                        {
                            worksheet.Cells[IntI, PricePerCaratColumn].Value = Val.ToDouble(DTabDetail.Rows[IntI - 2]["Net Rate"]);
                            worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                            worksheet.Cells[IntI, AmountColumn].Style.Font.Bold = true;

                        }

                        if (IntI != 1)
                        {
                            if (worksheet.Cells[IntI, VideoColumn].Value.ToString() == "")
                            {
                                worksheet.Cells[IntI, VideoColumn, IntI, VideoColumn].Value = "N/A";
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Color.SetColor(Color.Red);
                            }
                            else
                            {
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Color.SetColor(Color.Blue);
                                worksheet.Cells[IntI, VideoColumn].Formula = "=HYPERLINK(" + VideoLink + ", \"Image\")";
                            }

                        }
                    }

                    EndRow = EndRow + 2;
                    worksheet.Cells[EndRow, 1, EndRow, 1].Value = "Summary";

                    string RxW = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["RxW"].Ordinal + 1);
                    string CaratCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Carats"].Ordinal + 1);
                    string Discount1 = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Disc%"].Ordinal + 1);
                    string NetRate = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Net Rate"].Ordinal + 1);
                    string NetValue = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Net Value"].Ordinal + 1);

                    int IntTotRow = DTabDetail.Rows.Count + 1;

                    StartRow = StartRow + 1;

                    worksheet.Cells[EndRow, ShapeColumn, EndRow, ShapeColumn].Formula = "SUBTOTAL(2," + CaratCol + StartRow + ":" + CaratCol + IntTotRow + ")";
                    worksheet.Cells[EndRow, CaratColumn, EndRow, CaratColumn].Formula = "ROUND(SUBTOTAL(9," + CaratCol + StartRow + ":" + CaratCol + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, RxWColumn, EndRow, RxWColumn].Formula = "SUBTOTAL(9," + RxW + StartRow + ":" + RxW + IntTotRow + ")";
                    worksheet.Cells[EndRow, AmountColumn, EndRow, AmountColumn].Formula = "ROUND(SUBTOTAL(9," + NetValue + StartRow + ":" + NetValue + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, PricePerCaratColumn, EndRow, PricePerCaratColumn].Formula = "ROUND(" + NetValue + EndRow + "/" + CaratCol + EndRow + ",0)";
                    worksheet.Cells[EndRow, DiscountColumn, EndRow, DiscountColumn].Formula = "ROUND((" + NetValue + EndRow + "/" + RxW + EndRow + "-1 ) * 100,2)";

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Font.Bold = true;

                    worksheet.Cells[StartRow, CaratColumn, EndRow, CaratColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, DiscountColumn, EndRow, DiscountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, AmountColumn, EndRow, AmountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, DepthPerColumn, EndRow, DepthPerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, TablePerColumn, EndRow, TablePerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, GirdlePerColumn, EndRow, GirdlePerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, CAColumn, EndRow, CAColumn + 3].Style.Numberformat.Format = "0.00";

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

        public string ExportExcelWithSmartIList(DataSet DS, string PStrFilePath) //Add Khushbu 12-07-21
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
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Full Stock List");

                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    #region Smart-I

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

                    worksheet.Cells[1, 1, 100, 100].AutoFitColumns();

                    xlPackage.Save();
                    #endregion
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

        public string ExportExcelWithLabRecheck(DataSet DS, string PStrFilePath) //Add Khushbu 12-07-21
        {
            try
            {

                DataTable DTabDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);

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
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Lab Recheck List");

                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    #region Smart-I

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

                    worksheet.Cells[1, 1, 100, 100].AutoFitColumns();

                    xlPackage.Save();
                    #endregion
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

        public string ExportExcelWithOfferPriceFormat(DataSet DS, string PStrFilePath) //Add Krina
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

                DTabDetail.DefaultView.Sort = "SrNo";
                DTabDetail = DTabDetail.DefaultView.ToTable();

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
                Color BackColor1 = Color.FromArgb(146, 205, 220);
                Color FontColor1 = Color.FromArgb(99, 37, 35);
                Color FontColor2 = Color.Red;


                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;
                int EndRow1 = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Full Stock List");

                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    #region Stock Detail

                    EndRow = StartRow + DTabDetail.Rows.Count;
                    EndRow1 = StartRow + DTabDetail.Rows.Count;

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
                    worksheet.Cells[1, 33, EndRow1, 37].Style.Font.Color.SetColor(FontColor1);
                    worksheet.Cells[1, 38, EndRow1, 40].Style.Font.Color.SetColor(FontColor2);
                    worksheet.Cells[1, 6, 1, 17].Style.Fill.BackgroundColor.SetColor(BackColor1);

                    int RxWColumn = DTabDetail.Columns["RxW"].Ordinal + 1;
                    int RapaportColumn = DTabDetail.Columns["Rap. Price"].Ordinal + 1;
                    int PricePerCaratColumn = DTabDetail.Columns["Net Rate"].Ordinal + 1;
                    int DiscountColumn = DTabDetail.Columns["Disc%"].Ordinal + 1;
                    int CaratColumn = DTabDetail.Columns["Carats"].Ordinal + 1;
                    int AmountColumn = DTabDetail.Columns["Net Value"].Ordinal + 1;
                    int VideoLinkColumn = DTabDetail.Columns["Video Link"].Ordinal + 1;
                    int VideoColumn = DTabDetail.Columns["Video"].Ordinal + 1;
                    int DepthPerColumn = DTabDetail.Columns["Depth%"].Ordinal + 1;
                    int TablePerColumn = DTabDetail.Columns["Table%"].Ordinal + 1;
                    int ShapeColumn = DTabDetail.Columns["Shape"].Ordinal + 1;

                    int GirdlePerColumn = DTabDetail.Columns["Girldle Per"].Ordinal + 1;
                    int CAColumn = DTabDetail.Columns["Crown Angle"].Ordinal + 1;
                    int OfferAmountColumn = DTabDetail.Columns["Offer Net Value"].Ordinal + 1;
                    int OfferPricePerCaratColumn = DTabDetail.Columns["Offer Net Rate"].Ordinal + 1;
                    int OfferDiscountColumn = DTabDetail.Columns["Offer Disc%"].Ordinal + 1;
                    int ExpAmountColumn = DTabDetail.Columns["Exp.Net Value"].Ordinal + 1;
                    int ExpPricePerCaratColumn = DTabDetail.Columns["Exp.Net Rate"].Ordinal + 1;
                    int ExpDiscColumnn = DTabDetail.Columns["Exp Disc%"].Ordinal + 1;
                    int ExpRxWColumn = DTabDetail.Columns["Exp.RxW"].Ordinal + 1;

                    int MRxWColumn = DTabDetail.Columns["M.RxW"].Ordinal + 1;
                    int MRapaportColumn = DTabDetail.Columns["M.RapPrice"].Ordinal + 1;
                    int MPricePerCaratColumn = DTabDetail.Columns["M.NetRate"].Ordinal + 1;
                    int MDiscountColumn = DTabDetail.Columns["M.Disc%"].Ordinal + 1;
                    int MAmountColumn = DTabDetail.Columns["M.NetValue"].Ordinal + 1;

                    for (int IntI = 2; IntI <= EndRow; IntI++)
                    {
                        string RapColumns = Global.ColumnIndexToColumnLetter(RapaportColumn) + IntI.ToString();
                        string Discount = Global.ColumnIndexToColumnLetter(DiscountColumn) + IntI.ToString();
                        string Carat = Global.ColumnIndexToColumnLetter(CaratColumn) + IntI.ToString();
                        string PricePerCarat = Global.ColumnIndexToColumnLetter(PricePerCaratColumn) + IntI.ToString();
                        string VideoLink = Global.ColumnIndexToColumnLetter(VideoLinkColumn) + IntI.ToString();
                        string OfferPricePerCarat = Global.ColumnIndexToColumnLetter(OfferPricePerCaratColumn) + IntI.ToString();
                        string OfferDiscount = Global.ColumnIndexToColumnLetter(OfferDiscountColumn) + IntI.ToString();


                        worksheet.Cells[IntI, RxWColumn].Formula = "=ROUND(" + RapColumns + " * " + Carat + ",2)";

                        if (Val.ToString(DTabDetail.Rows[IntI - 2]["FANCYCOLOR"]) == "") //Add if condition khushbu 08-10-21 for skip formula in fancy color
                        {
                            worksheet.Cells[IntI, PricePerCaratColumn].Formula = "=(" + RapColumns + " + ((" + RapColumns + " * " + Discount + ") / 100))";
                            worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                            worksheet.Cells[IntI, AmountColumn].Style.Font.Bold = true;
                            worksheet.Cells[IntI, OfferPricePerCaratColumn].Formula = "=(" + RapColumns + " + ((" + RapColumns + " * " + OfferDiscount + ") / 100))";
                            worksheet.Cells[IntI, OfferAmountColumn].Formula = "=ROUND(" + OfferPricePerCarat + " * " + Carat + ",2)";
                            worksheet.Cells[IntI, OfferAmountColumn].Style.Font.Bold = true;
                            worksheet.Cells[IntI, ExpAmountColumn].Style.Font.Bold = true;
                            worksheet.Cells[IntI, MAmountColumn].Style.Font.Bold = true;
                        }
                        else
                        {
                            worksheet.Cells[IntI, PricePerCaratColumn].Value = Val.ToDouble(DTabDetail.Rows[IntI - 2]["Net Rate"]);
                            worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                            worksheet.Cells[IntI, AmountColumn].Style.Font.Bold = true;
                            worksheet.Cells[IntI, OfferPricePerCaratColumn].Value = Val.ToDouble(DTabDetail.Rows[IntI - 2]["Offer Net Rate"]);
                            worksheet.Cells[IntI, OfferAmountColumn].Formula = "=ROUND(" + OfferPricePerCarat + " * " + Carat + ",2)";
                            worksheet.Cells[IntI, OfferAmountColumn].Style.Font.Bold = true;
                            worksheet.Cells[IntI, ExpAmountColumn].Style.Font.Bold = true;
                            worksheet.Cells[IntI, MAmountColumn].Style.Font.Bold = true;
                        }

                        if (IntI != 1)
                        {
                            if (worksheet.Cells[IntI, VideoColumn].Value.ToString() == "")
                            {
                                worksheet.Cells[IntI, VideoColumn, IntI, VideoColumn].Value = "N/A";
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Color.SetColor(Color.Red);
                            }
                            else
                            {
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Color.SetColor(Color.Blue);
                                worksheet.Cells[IntI, VideoColumn].Formula = "=HYPERLINK(" + VideoLink + ", \"Image\")";
                            }

                        }
                    }

                    EndRow = EndRow + 2;
                    worksheet.Cells[EndRow, 1, EndRow, 1].Value = "Summary";

                    string RxW = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["RxW"].Ordinal + 1);
                    string CaratCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Carats"].Ordinal + 1);
                    string Discount1 = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Disc%"].Ordinal + 1);
                    string NetRate = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Net Rate"].Ordinal + 1);
                    string NetValue = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Net Value"].Ordinal + 1);
                    string OfferNetRate = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Offer Net Rate"].Ordinal + 1);
                    string OfferNetValue = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Offer Net Value"].Ordinal + 1);
                    string OfferDiscount1 = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Offer Disc%"].Ordinal + 1);
                    string ExpRxW = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Exp.RxW"].Ordinal + 1);
                    string ExpDisc = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Exp Disc%"].Ordinal + 1);
                    string ExpNetRate = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Exp.Net Rate"].Ordinal + 1);
                    string ExpNetValue = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Exp.Net Value"].Ordinal + 1);

                    string MRxW = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["M.RxW"].Ordinal + 1);
                    string MDiscount = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["M.Disc%"].Ordinal + 1);
                    string MNetRate = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["M.NetRate"].Ordinal + 1);
                    string MNetValue = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["M.NetValue"].Ordinal + 1);


                    int IntTotRow = DTabDetail.Rows.Count + 1;

                    StartRow = StartRow + 1;

                    worksheet.Cells[EndRow, ShapeColumn, EndRow, ShapeColumn].Formula = "SUBTOTAL(2," + CaratCol + StartRow + ":" + CaratCol + IntTotRow + ")";
                    worksheet.Cells[EndRow, CaratColumn, EndRow, CaratColumn].Formula = "ROUND(SUBTOTAL(9," + CaratCol + StartRow + ":" + CaratCol + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, RxWColumn, EndRow, RxWColumn].Formula = "SUBTOTAL(9," + RxW + StartRow + ":" + RxW + IntTotRow + ")";
                    worksheet.Cells[EndRow, AmountColumn, EndRow, AmountColumn].Formula = "ROUND(SUBTOTAL(9," + NetValue + StartRow + ":" + NetValue + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, PricePerCaratColumn, EndRow, PricePerCaratColumn].Formula = "ROUND(" + NetValue + EndRow + "/" + CaratCol + EndRow + ",0)";
                    worksheet.Cells[EndRow, DiscountColumn, EndRow, DiscountColumn].Formula = "ROUND((" + NetValue + EndRow + "/" + RxW + EndRow + "-1 ) * 100,2)";
                    worksheet.Cells[EndRow, OfferAmountColumn, EndRow, OfferAmountColumn].Formula = "ROUND(SUBTOTAL(9," + OfferNetValue + StartRow + ":" + OfferNetValue + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, OfferPricePerCaratColumn, EndRow, OfferPricePerCaratColumn].Formula = "ROUND(" + OfferNetValue + EndRow + "/" + CaratCol + EndRow + ",0)";
                    worksheet.Cells[EndRow, OfferDiscountColumn, EndRow, OfferDiscountColumn].Formula = "ROUND((" + OfferNetValue + EndRow + "/" + RxW + EndRow + "-1 ) * 100,2)";

                    worksheet.Cells[EndRow, ExpAmountColumn, EndRow, ExpAmountColumn].Formula = "ROUND(SUBTOTAL(9," + ExpNetValue + StartRow + ":" + ExpNetValue + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, ExpPricePerCaratColumn, EndRow, ExpPricePerCaratColumn].Formula = "ROUND(" + ExpNetValue + EndRow + "/" + CaratCol + EndRow + ",0)";
                    worksheet.Cells[EndRow, ExpDiscColumnn, EndRow, ExpDiscColumnn].Formula = "ROUND((" + ExpNetValue + EndRow + "/" + ExpRxW + EndRow + "-1 ) * 100,2)";
                    worksheet.Cells[EndRow, ExpRxWColumn, EndRow, ExpRxWColumn].Formula = "SUBTOTAL(9," + ExpRxW + StartRow + ":" + ExpRxW + IntTotRow + ")";

                    worksheet.Cells[EndRow, MAmountColumn, EndRow, MAmountColumn].Formula = "ROUND(SUBTOTAL(9," + MNetValue + StartRow + ":" + MNetValue + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, MPricePerCaratColumn, EndRow, MPricePerCaratColumn].Formula = "ROUND(" + MNetValue + EndRow + "/" + CaratCol + EndRow + ",0)";
                    worksheet.Cells[EndRow, MDiscountColumn, EndRow, MDiscountColumn].Formula = "ROUND((" + MNetValue + EndRow + "/" + MRxW + EndRow + " ) * 100,2)";
                    worksheet.Cells[EndRow, MRxWColumn, EndRow, MRxWColumn].Formula = "SUBTOTAL(9," + MRxW + StartRow + ":" + MRxW + IntTotRow + ")";

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Font.Bold = true;

                    worksheet.Cells[StartRow, CaratColumn, EndRow, CaratColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, DiscountColumn, EndRow, DiscountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, AmountColumn, EndRow, AmountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, DepthPerColumn, EndRow, DepthPerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, TablePerColumn, EndRow, TablePerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, GirdlePerColumn, EndRow, GirdlePerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, CAColumn, EndRow, CAColumn + 3].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, OfferDiscountColumn, EndRow, OfferDiscountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, OfferAmountColumn, EndRow, OfferAmountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, ExpDiscColumnn, EndRow, ExpDiscColumnn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, ExpAmountColumn, EndRow, ExpAmountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, MDiscountColumn, EndRow, MDiscountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, MAmountColumn, EndRow, MAmountColumn].Style.Numberformat.Format = "0.00";

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

        public string ExportExcelWithReviseList(DataSet DS, string PStrFilePath) //Add Darshan
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

                DTabDetail.DefaultView.Sort = "SrNo";
                DTabDetail = DTabDetail.DefaultView.ToTable();

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
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Revise Stock List");

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

                    int RxWColumn = DTabDetail.Columns["RxW"].Ordinal + 1;
                    int RapaportColumn = DTabDetail.Columns["Rap. Price"].Ordinal + 1;
                    int PricePerCaratColumn = DTabDetail.Columns["Net Rate"].Ordinal + 1;
                    int DiscountColumn = DTabDetail.Columns["Disc%"].Ordinal + 1;
                    int CaratColumn = DTabDetail.Columns["Carats"].Ordinal + 1;
                    int AmountColumn = DTabDetail.Columns["Net Value"].Ordinal + 1;
                    int VideoLinkColumn = DTabDetail.Columns["Video Link"].Ordinal + 1;
                    int VideoColumn = DTabDetail.Columns["Video"].Ordinal + 1;
                    int DepthPerColumn = DTabDetail.Columns["Depth%"].Ordinal + 1;
                    int TablePerColumn = DTabDetail.Columns["Table%"].Ordinal + 1;
                    int ShapeColumn = DTabDetail.Columns["Shape"].Ordinal + 1;

                    int GirdlePerColumn = DTabDetail.Columns["Girldle Per"].Ordinal + 1;
                    int CAColumn = DTabDetail.Columns["Crown Angle"].Ordinal + 1;

                    int SysRxWColumn = DTabDetail.Columns["Sys RxW"].Ordinal + 1;
                    int SysRapaportColumn = DTabDetail.Columns["Sys Rap. Price"].Ordinal + 1;
                    int SysDiscountColumn = DTabDetail.Columns["Sys Disc%"].Ordinal + 1;
                    int SysPricePerCaratColumn = DTabDetail.Columns["Sys Net Rate"].Ordinal + 1;
                    int SysAmountColumn = DTabDetail.Columns["Sys Net Value"].Ordinal + 1;


                    for (int IntI = 2; IntI <= EndRow; IntI++)
                    {
                        string RapColumns = Global.ColumnIndexToColumnLetter(RapaportColumn) + IntI.ToString();
                        string Discount = Global.ColumnIndexToColumnLetter(DiscountColumn) + IntI.ToString();
                        string Carat = Global.ColumnIndexToColumnLetter(CaratColumn) + IntI.ToString();
                        string PricePerCarat = Global.ColumnIndexToColumnLetter(PricePerCaratColumn) + IntI.ToString();
                        string VideoLink = Global.ColumnIndexToColumnLetter(VideoLinkColumn) + IntI.ToString();

                        string SysRapColumns = Global.ColumnIndexToColumnLetter(SysRapaportColumn) + IntI.ToString();
                        string SysDiscount = Global.ColumnIndexToColumnLetter(SysDiscountColumn) + IntI.ToString();
                        string SysPricePerCarat = Global.ColumnIndexToColumnLetter(SysPricePerCaratColumn) + IntI.ToString();

                        worksheet.Cells[IntI, RxWColumn].Formula = "=ROUND(" + RapColumns + " * " + Carat + ",2)";

                        if (Val.ToString(DTabDetail.Rows[IntI - 2]["FANCYCOLOR"]) == "") //Add if condition khushbu 08-10-21 for skip formula in fancy color
                        {
                            worksheet.Cells[IntI, PricePerCaratColumn].Formula = "=(" + RapColumns + " + ((" + RapColumns + " * " + Discount + ") / 100))";
                            worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                            worksheet.Cells[IntI, AmountColumn].Style.Font.Bold = true;

                            worksheet.Cells[IntI, SysPricePerCaratColumn].Formula = "=(" + SysRapColumns + " + ((" + SysRapColumns + " * " + SysDiscount + ") / 100))";
                            worksheet.Cells[IntI, SysAmountColumn].Formula = "=ROUND(" + SysPricePerCarat + " * " + Carat + ",2)";
                            worksheet.Cells[IntI, SysAmountColumn].Style.Font.Bold = true;
                        }
                        else
                        {
                            worksheet.Cells[IntI, PricePerCaratColumn].Value = Val.ToDouble(DTabDetail.Rows[IntI - 2]["Net Rate"]);
                            worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                            worksheet.Cells[IntI, AmountColumn].Style.Font.Bold = true;

                            worksheet.Cells[IntI, SysPricePerCaratColumn].Value = Val.ToDouble(DTabDetail.Rows[IntI - 2]["Sys Net Rate"]);
                            worksheet.Cells[IntI, SysAmountColumn].Formula = "=ROUND(" + SysPricePerCarat + " * " + Carat + ",2)";
                            worksheet.Cells[IntI, SysAmountColumn].Style.Font.Bold = true;
                        }

                        if (IntI != 1)
                        {
                            if (worksheet.Cells[IntI, VideoColumn].Value.ToString() == "")
                            {
                                worksheet.Cells[IntI, VideoColumn, IntI, VideoColumn].Value = "N/A";
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Color.SetColor(Color.Red);
                            }
                            else
                            {
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Color.SetColor(Color.Blue);
                                worksheet.Cells[IntI, VideoColumn].Formula = "=HYPERLINK(" + VideoLink + ", \"Image\")";
                            }

                        }
                    }

                    EndRow = EndRow + 2;
                    worksheet.Cells[EndRow, 1, EndRow, 1].Value = "Summary";

                    string RxW = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["RxW"].Ordinal + 1);
                    string CaratCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Carats"].Ordinal + 1);
                    string Discount1 = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Disc%"].Ordinal + 1);
                    string NetRate = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Net Rate"].Ordinal + 1);
                    string NetValue = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Net Value"].Ordinal + 1);

                    string SysRxW = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Sys RxW"].Ordinal + 1);
                    string SysDiscount1 = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Sys Disc%"].Ordinal + 1);
                    string SysNetRate = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Sys Net Rate"].Ordinal + 1);
                    string SysNetValue = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Sys Net Value"].Ordinal + 1);

                    int IntTotRow = DTabDetail.Rows.Count + 1;

                    StartRow = StartRow + 1;

                    worksheet.Cells[EndRow, ShapeColumn, EndRow, ShapeColumn].Formula = "SUBTOTAL(2," + CaratCol + StartRow + ":" + CaratCol + IntTotRow + ")";
                    worksheet.Cells[EndRow, CaratColumn, EndRow, CaratColumn].Formula = "ROUND(SUBTOTAL(9," + CaratCol + StartRow + ":" + CaratCol + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, RxWColumn, EndRow, RxWColumn].Formula = "SUBTOTAL(9," + RxW + StartRow + ":" + RxW + IntTotRow + ")";
                    worksheet.Cells[EndRow, AmountColumn, EndRow, AmountColumn].Formula = "ROUND(SUBTOTAL(9," + NetValue + StartRow + ":" + NetValue + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, PricePerCaratColumn, EndRow, PricePerCaratColumn].Formula = "ROUND(" + NetValue + EndRow + "/" + CaratCol + EndRow + ",0)";
                    worksheet.Cells[EndRow, DiscountColumn, EndRow, DiscountColumn].Formula = "ROUND((" + NetValue + EndRow + "/" + RxW + EndRow + "-1 ) * 100,2)";

                    worksheet.Cells[EndRow, SysRxWColumn, EndRow, SysRxWColumn].Formula = "SUBTOTAL(9," + SysRxW + StartRow + ":" + SysRxW + IntTotRow + ")";
                    worksheet.Cells[EndRow, SysAmountColumn, EndRow, SysAmountColumn].Formula = "ROUND(SUBTOTAL(9," + SysNetValue + StartRow + ":" + SysNetValue + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, SysPricePerCaratColumn, EndRow, SysPricePerCaratColumn].Formula = "ROUND(" + SysNetValue + EndRow + "/" + CaratCol + EndRow + ",0)";
                    worksheet.Cells[EndRow, SysDiscountColumn, EndRow, SysDiscountColumn].Formula = "ROUND((" + SysNetValue + EndRow + "/" + SysRxW + EndRow + "-1 ) * 100,2)";

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Font.Bold = true;

                    worksheet.Cells[StartRow, CaratColumn, EndRow, CaratColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, DiscountColumn, EndRow, DiscountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, AmountColumn, EndRow, AmountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, DepthPerColumn, EndRow, DepthPerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, TablePerColumn, EndRow, TablePerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, GirdlePerColumn, EndRow, GirdlePerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, CAColumn, EndRow, CAColumn + 3].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, SysDiscountColumn, EndRow, SysDiscountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, SysAmountColumn, EndRow, SysAmountColumn].Style.Numberformat.Format = "0.00";

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    #endregion

                    worksheet.Cells[1, 1, 100, 100].AutoFitColumns();
                    worksheet.Column(19).Hidden = true;
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

        public string ExportExcelNew()
        {
            try
            {
                string MemoEntryDetailForXML = "";

                //DataTable DtInvDetail = GetSelectedRowToTable();

                DataTable DtInvDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);

                DtInvDetail = DtInvDetail.DefaultView.ToTable(false, "STOCK_ID");

                DtInvDetail.TableName = "Table";
                MemoEntryDetailForXML = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DtInvDetail.WriteXml(sw);
                    MemoEntryDetailForXML = sw.ToString();
                }

                string WebStatus = "";

                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "FORMAT";
                this.Cursor = Cursors.WaitCursor;
                FrmSearch.mDTab = Global.GetExportFileTemplate();

                FrmSearch.mStrColumnsToHide = "";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                string FormatName = "";
                if (FrmSearch.DRow != null)
                {
                    FormatName = Val.ToString(FrmSearch.DRow["FORMAT"]);
                }
                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;

                if (FormatName == "")
                {
                    Global.Message("PLEASE SELECT ANY OF ONE FORMAT");
                    return "";
                }

                LiveStockProperty LStockProperty = new LiveStockProperty();

                this.Cursor = Cursors.WaitCursor;
                DataSet DS = ObjStock.GetDataForExcelExportNew(MemoEntryDetailForXML, WebStatus, "SINGLE", FormatName, LStockProperty);
                this.Cursor = Cursors.Default;

                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = ".xlsx";
                svDialog.Title = "Export to Excel";
                svDialog.FileName = BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";
                svDialog.Filter = "Excel File (*.xlsx)|*.xlsx ";
                string StrFilePath = "";
                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    StrFilePath = svDialog.FileName;
                }

                if (FormatName == "With Exp")
                {
                    string Result = ExportExcelWithExp(DS, StrFilePath);
                    return Result;
                }

                if (FormatName == "Stock List")
                {

                    string Result = ExportExcelWithStockList(DS, StrFilePath);
                    return Result;
                }
                else if (FormatName == "Offer Price Report")
                {
                    string Result = ExportExcelWithOfferPriceFormat(DS, StrFilePath);
                    return Result;
                }
                else if (FormatName == "Revise List")
                {
                    string Result = ExportExcelWithReviseList(DS, StrFilePath);
                    return Result;
                }
                else if (FormatName == "Smart-I")
                {
                    string Result = ExportExcelWithSmartIList(DS, StrFilePath);
                    return Result;
                }
                else if (FormatName == "Lab Recheck")
                {
                    string Result = ExportExcelWithSmartIList(DS, StrFilePath);
                    return Result;
                }

                DataTable DTabDetail = DS.Tables[0];
                DataTable DTabSize = DS.Tables[1];
                DataTable DTabShape = DS.Tables[2];
                DataTable DTabClarity = DS.Tables[3];
                DataTable DTabColor = DS.Tables[4];
                DataTable DTabCut = DS.Tables[5];
                DataTable DTabPolish = DS.Tables[6];
                DataTable DTabSym = DS.Tables[7];
                DataTable DTabFL = DS.Tables[8];
                DataTable DTabInclusion = DS.Tables[9];


                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }

                DTabDetail.DefaultView.Sort = "SR";
                DTabDetail = DTabDetail.DefaultView.ToTable();

                DTabSize.DefaultView.Sort = "FromCarat";
                DTabSize = DTabSize.DefaultView.ToTable();

                DTabShape.DefaultView.Sort = "SequenceNo";
                DTabShape = DTabShape.DefaultView.ToTable();

                DTabColor.DefaultView.Sort = "SequenceNo";
                DTabColor = DTabColor.DefaultView.ToTable();

                DTabClarity.DefaultView.Sort = "SequenceNo";
                DTabClarity = DTabClarity.DefaultView.ToTable();

                DTabCut.DefaultView.Sort = "SequenceNo";
                DTabCut = DTabCut.DefaultView.ToTable();

                DTabPolish.DefaultView.Sort = "SequenceNo";
                DTabPolish = DTabPolish.DefaultView.ToTable();

                DTabSym.DefaultView.Sort = "SequenceNo";
                DTabSym = DTabSym.DefaultView.ToTable();

                DTabFL.DefaultView.Sort = "SequenceNo";
                DTabFL = DTabFL.DefaultView.ToTable();

                this.Cursor = Cursors.WaitCursor;

                // string StrFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.FromArgb(2, 68, 143);
                //Color BackColor = Color.FromArgb(119, 50, 107);
                Color FontColor = Color.White;
                string FontName = "Calibri";
                float FontSize = 9;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("JV_Stock_" + DateTime.Now.ToString("ddMMyyyy"));
                    ExcelWorksheet worksheetProportion = xlPackage.Workbook.Worksheets.Add("Proportion");
                    ExcelWorksheet worksheetInclusion = xlPackage.Workbook.Worksheets.Add("Inclusion Detail");


                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    #region Stock Detail

                    StartRow = 5;
                    EndRow = StartRow + DTabDetail.Rows.Count;
                    StartColumn = 1;
                    EndColumn = DTabDetail.Columns.Count;

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
                    //   worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].AutoFilter = true;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(FontColor);

                    //#P : 06-08-2020
                    if (FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale")
                    {
                        worksheet.Cells[5, 15, 5, 18].Style.Font.Color.SetColor(Color.Red);
                    }

                    if (FormatName == "With Rapnet")
                    {
                        worksheet.Cells[5, 19, 5, 22].Style.Font.Color.SetColor(FontColor);
                    }
                    else if (FormatName == "With Sale")
                    {
                        worksheet.Cells[5, 19, 5, 22].Style.Font.Color.SetColor(Color.FromArgb(174, 201, 121));
                    }
                    //End : #P : 06-08-2020

                    worksheet.View.FreezePanes(6, 1);

                    // Set Hyperlink
                    int IntCertColumn = DTabDetail.Columns["CertNo"].Ordinal;
                    int IntVideoUrlColumn = DTabDetail.Columns["VideoUrl"].Ordinal;
                    int IntStoneDEtailUrlColumn = DTabDetail.Columns["StoneDetailURL"].Ordinal;

                    int RapaportColumn = DTabDetail.Columns["RapRate"].Ordinal + 1;
                    int PricePerCaratColumn = DTabDetail.Columns["PricePerCarat"].Ordinal + 1;
                    int DiscountColumn = DTabDetail.Columns["RapPer"].Ordinal + 1;
                    int CaratColumn = DTabDetail.Columns["Carat"].Ordinal + 1;
                    int AmountColumn = DTabDetail.Columns["Amount"].Ordinal + 1;

                    for (int IntI = 6; IntI <= EndRow; IntI++)
                    {
                        string RapColumns = Global.ColumnIndexToColumnLetter(RapaportColumn) + IntI.ToString();
                        string Discount = Global.ColumnIndexToColumnLetter(DiscountColumn) + IntI.ToString();
                        string Carat = Global.ColumnIndexToColumnLetter(CaratColumn) + IntI.ToString();
                        string PricePerCarat = Global.ColumnIndexToColumnLetter(PricePerCaratColumn) + IntI.ToString();

                        if (Val.ToString(DTabDetail.Rows[IntI - 6]["FANCYCOLOR"]) == "") //Add if condition khushbu 23-09-21 for skip formula in fancy color
                        {
                            worksheet.Cells[IntI, PricePerCaratColumn].Formula = "=ROUND(" + RapColumns + " + ((" + RapColumns + " * " + Discount + ") / 100),2)";
                            worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                        }
                        else
                        {
                            worksheet.Cells[IntI, PricePerCaratColumn].Value = Val.ToString(DTabDetail.Rows[IntI - 6]["PRICEPERCARAT"]);
                            //worksheet.Cells[IntI, AmountColumn].Value = Val.ToString(DTabDetail.Rows[IntI - 6]["AMOUNT"]);
                            worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                        }

                        //if (!Val.ToString(DTabDetail.Rows[IntI - 6]["DNAPAGEURL"]).Trim().Equals(string.Empty))
                        //{
                        //    //worksheet.Cells[IntI, 2, IntI, 2].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["DNAPAGEURL"]));
                        //    worksheet.Cells[IntI, 2, IntI, 2].Style.Font.Name = FontName;
                        //    worksheet.Cells[IntI, 2, IntI, 2].Style.Font.Bold = true;
                        //    worksheet.Cells[IntI, 2, IntI, 2].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                        //    //worksheet.Cells[IntI, 2, IntI, 2].Style.Font.UnderLine = true;
                        //}

                        //if (!Val.ToString(DTabDetail.Rows[IntI - 6]["CERTURL"]).Trim().Equals(string.Empty))
                        //{
                        //    worksheet.Cells[IntI, IntCertColumn + 1].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["CERTURL"]));
                        //    worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.Name = FontName;
                        //    worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.Bold = true;
                        //    worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                        //    worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.UnderLine = true;
                        //}


                        if (!Val.ToString(DTabDetail.Rows[IntI - 6]["VIDEOURL"]).Trim().Equals(string.Empty))
                        {
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Value = "Video";
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["VIDEOURL"]));
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Style.Font.Name = FontName;
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Style.Font.Bold = true;
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Style.Font.UnderLine = true;
                        }
                        //End : #P :  03-09-2020

                        if (!Val.ToString(DTabDetail.Rows[IntI - 6]["STONEDETAILURL"]).Trim().Equals(string.Empty))
                        {
                            //worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Value = "Detail";
                            worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["STONEDETAILURL"]));
                            worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Style.Font.Name = FontName;
                            worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Style.Font.Bold = true;
                            worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                            worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Style.Font.UnderLine = true;
                        }

                        //#P : 06-08-2020
                        if (FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale")
                        {
                            worksheet.Cells[IntI, 15, IntI, 18].Style.Font.Color.SetColor(Color.Red);
                        }
                        if (FormatName == "With Rapnet")
                        {
                            worksheet.Cells[IntI, 19, IntI, 22].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                        }
                        else if (FormatName == "With Sale")
                        {
                            worksheet.Cells[IntI, 19, IntI, 22].Style.Font.Color.SetColor(Color.FromArgb(0, 150, 68));
                        }
                        //End : #P : 06-08-2020

                    }

                    // Header Set
                    for (int i = 1; i <= DTabDetail.Columns.Count; i++)
                    {
                        string StrHeader = Global.ExportExcelHeader(Val.ToString(worksheet.Cells[5, i].Value), worksheet, i);
                        worksheet.Cells[5, i].Value = StrHeader;

                    }

                    int IntRowStartsFrom = 3;
                    int IntRowEndTo = (DTabDetail.Rows.Count - 1 + IntRowStartsFrom);

                    //CHECK COLUMN EXISTS IN DATATABLE..
                    #region :: Check Column Exists In Datatable ::
                    int SrNo = 0, CaratNo = 0, AmountNo = 0, RapAmountNo = 0, SizeNo = 0, ShapeNo = 0, ColorNo = 0, ClarityNo = 0, CutNo = 0, PolNo = 0, SymNo = 0, FLNo = 0,
                        ExpAmountNo = 0, ExpRapAmountNo = 0, RapnetAmountNo = 0, RapnetRapAmountNo = 0, InvoiceAmountNo = 0, InvoiceRapAmountNo = 0;


                    DataColumnCollection columns = DTabDetail.Columns;

                    if (columns.Contains("SR"))
                        SrNo = DTabDetail.Columns["SR"].Ordinal + 1;
                    if (columns.Contains("Size"))
                        SizeNo = DTabDetail.Columns["Size"].Ordinal + 1;
                    if (columns.Contains("Carat"))
                        CaratNo = DTabDetail.Columns["Carat"].Ordinal + 1;
                    if (columns.Contains("RapValue"))
                        RapAmountNo = DTabDetail.Columns["RapValue"].Ordinal + 1;
                    if (columns.Contains("Amount"))
                        AmountNo = DTabDetail.Columns["Amount"].Ordinal + 1;

                    //#P : 06-08-2020
                    if ((FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale") && columns.Contains("ExpRapValue"))
                        ExpRapAmountNo = DTabDetail.Columns["ExpRapValue"].Ordinal + 1;
                    if ((FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale") && columns.Contains("ExpAmount"))
                        ExpAmountNo = DTabDetail.Columns["ExpAmount"].Ordinal + 1;
                    if (FormatName == "With Rapnet" && columns.Contains("RapnetRapValue"))
                        RapnetRapAmountNo = DTabDetail.Columns["RapnetRapValue"].Ordinal + 1;
                    if (FormatName == "With Rapnet" && columns.Contains("RapnetAmount"))
                        RapnetAmountNo = DTabDetail.Columns["RapnetAmount"].Ordinal + 1;

                    if (FormatName == "With Sale" && columns.Contains("InvoiceRapValue"))
                        InvoiceRapAmountNo = DTabDetail.Columns["InvoiceRapValue"].Ordinal + 1;
                    if (FormatName == "With Sale" && columns.Contains("InvoiceAmount"))
                        InvoiceAmountNo = DTabDetail.Columns["InvoiceAmount"].Ordinal + 1;

                    //End : #P : 06-08-2020

                    if (columns.Contains("Shape"))
                        ShapeNo = DTabDetail.Columns["Shape"].Ordinal + 1;
                    if (columns.Contains("Color"))
                        ColorNo = DTabDetail.Columns["Color"].Ordinal + 1;
                    if (columns.Contains("Clarity"))
                        ClarityNo = DTabDetail.Columns["Clarity"].Ordinal + 1;
                    if (columns.Contains("Cut"))
                        CutNo = DTabDetail.Columns["Cut"].Ordinal + 1;
                    if (columns.Contains("Pol"))
                        PolNo = DTabDetail.Columns["Pol"].Ordinal + 1;
                    if (columns.Contains("Sym"))
                        SymNo = DTabDetail.Columns["Sym"].Ordinal + 1;
                    if (columns.Contains("FL"))
                        FLNo = DTabDetail.Columns["FL"].Ordinal + 1;

                    #endregion

                    string StrStartRow = "6";
                    string StrEndRow = EndRow.ToString();

                    #region Top Formula

                    worksheet.Cells[1, 5, 1, 5].Value = "Pcs";
                    worksheet.Cells[1, 6, 1, 6].Value = "Carat";
                    worksheet.Cells[1, 11, 1, 11].Value = "Rap Value";
                    worksheet.Cells[1, 12, 1, 12].Value = "Rap %";
                    worksheet.Cells[1, 13, 1, 13].Value = "Pr/Ct";
                    worksheet.Cells[1, 14, 1, 14].Value = "Amount";

                    //#P : 06-08-2020
                    if (FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale")
                    {
                        worksheet.Cells[1, 15, 1, 15].Value = "Exp RapValue";
                        worksheet.Cells[1, 16, 1, 16].Value = "Exp Rap%";
                        worksheet.Cells[1, 17, 1, 17].Value = "Exp Pr/Ct";
                        worksheet.Cells[1, 18, 1, 18].Value = "Exp Amount";
                    }
                    if (FormatName == "With Rapnet")
                    {
                        worksheet.Cells[1, 19, 1, 19].Value = "Rapnet RapValue";
                        worksheet.Cells[1, 20, 1, 20].Value = "Rapnet Rap%";
                        worksheet.Cells[1, 21, 1, 21].Value = "Rapnet Pr/Ct";
                        worksheet.Cells[1, 22, 1, 22].Value = "Rapnet Amount";
                    }
                    if (FormatName == "With Sale")
                    {
                        worksheet.Cells[1, 19, 1, 19].Value = "Sale RapValue";
                        worksheet.Cells[1, 20, 1, 20].Value = "Sale Rap%";
                        worksheet.Cells[1, 21, 1, 21].Value = "Sale Pr/Ct";
                        worksheet.Cells[1, 22, 1, 22].Value = "Sale Amount";
                    }
                    //End : #P : 06-08-2020


                    worksheet.Cells[2, 4, 2, 4].Value = "Total";
                    worksheet.Cells[3, 4, 3, 4].Value = "Selected";

                    worksheet.Cells[1, 7, 3, 10].Merge = true;
                    worksheet.Cells[1, 7, 3, 10].Value = "Note : Use filter to select stones and Check your ObjGridSelection Avg Disc and Total amt.";
                    worksheet.Cells[1, 7, 3, 10].Style.WrapText = true;

                    // Total Pcs Formula
                    string S = Global.ColumnIndexToColumnLetter(SrNo) + StrStartRow;
                    string E = Global.ColumnIndexToColumnLetter(SrNo) + StrEndRow;
                    worksheet.Cells[2, 5, 2, 5].Formula = "ROUND(COUNTA(" + S + ":" + E + "),2)";
                    worksheet.Cells[3, 5, 3, 5].Formula = "ROUND(SUBTOTAL(3," + S + ":" + E + "),2)";

                    // Total Carat Formula
                    S = Global.ColumnIndexToColumnLetter(CaratNo) + StrStartRow;
                    E = Global.ColumnIndexToColumnLetter(CaratNo) + StrEndRow;
                    worksheet.Cells[2, 6, 2, 6].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                    worksheet.Cells[3, 6, 3, 6].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                    S = Global.ColumnIndexToColumnLetter(RapAmountNo) + StrStartRow;
                    E = Global.ColumnIndexToColumnLetter(RapAmountNo) + StrEndRow;
                    worksheet.Cells[2, 11, 2, 11].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                    worksheet.Cells[3, 11, 3, 11].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                    // Amount Formula
                    S = Global.ColumnIndexToColumnLetter(AmountNo) + StrStartRow;
                    E = Global.ColumnIndexToColumnLetter(AmountNo) + StrEndRow;
                    worksheet.Cells[2, 14, 2, 14].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                    worksheet.Cells[3, 14, 3, 14].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                    // Price Per Carat Formula
                    worksheet.Cells[2, 13, 2, 13].Formula = "ROUND(N2/F2,2)";
                    worksheet.Cells[3, 13, 3, 13].Formula = "ROUND(N3/F3,2)";

                    // Discount Formula
                    S = Global.ColumnIndexToColumnLetter(AmountNo) + StrStartRow;
                    E = Global.ColumnIndexToColumnLetter(AmountNo) + StrEndRow;

                    worksheet.Cells[2, 12, 2, 12].Formula = "ROUND(SUM(((N2)/((K2*1)))*100),2)-100";
                    worksheet.Cells[3, 12, 3, 12].Formula = "ROUND(SUM(((N3)/((K3*1)))*100),2)-100";


                    #region Exp Summary Detail
                    if (FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale")
                    {
                        //Exp RapValue
                        S = Global.ColumnIndexToColumnLetter(ExpRapAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(ExpRapAmountNo) + StrEndRow;
                        worksheet.Cells[2, 15, 2, 15].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                        worksheet.Cells[3, 15, 3, 15].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                        // Exp Amount Formula
                        S = Global.ColumnIndexToColumnLetter(ExpAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(ExpAmountNo) + StrEndRow;
                        worksheet.Cells[2, 18, 2, 18].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                        worksheet.Cells[3, 18, 3, 18].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                        // Exp Price Per Carat Formula
                        worksheet.Cells[2, 17, 2, 17].Formula = "ROUND(R2/F2,2)";
                        worksheet.Cells[3, 17, 3, 17].Formula = "ROUND(R3/F3,2)";

                        // Exp Discount Formula (Rap%)
                        S = Global.ColumnIndexToColumnLetter(ExpAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(ExpAmountNo) + StrEndRow;

                        worksheet.Cells[2, 16, 2, 16].Formula = "ROUND(SUM(((R2)/((O2*1)))*100),2)-100";
                        worksheet.Cells[3, 16, 3, 16].Formula = "ROUND(SUM(((R3)/((O3*1)))*100),2)-100";
                    }
                    #endregion

                    #region Rapnet Summary Detail
                    if (FormatName == "With Rapnet")
                    {
                        //Exp RapValue
                        S = Global.ColumnIndexToColumnLetter(RapnetRapAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(RapnetRapAmountNo) + StrEndRow;
                        worksheet.Cells[2, 19, 2, 19].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                        worksheet.Cells[3, 19, 3, 19].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                        // Exp Amount Formula
                        S = Global.ColumnIndexToColumnLetter(RapnetAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(RapnetAmountNo) + StrEndRow;
                        worksheet.Cells[2, 22, 2, 22].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                        worksheet.Cells[3, 22, 3, 22].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                        // Exp Price Per Carat Formula
                        worksheet.Cells[2, 21, 2, 21].Formula = "ROUND(V2/F2,2)";
                        worksheet.Cells[3, 21, 3, 21].Formula = "ROUND(V3/F3,2)";

                        // Exp Discount Formula (Rap%)
                        S = Global.ColumnIndexToColumnLetter(RapnetAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(RapnetAmountNo) + StrEndRow;

                        worksheet.Cells[2, 20, 2, 20].Formula = "ROUND(SUM(((V2)/((S2*1)))*100),2)-100";
                        worksheet.Cells[3, 20, 3, 20].Formula = "ROUND(SUM(((V3)/((S3*1)))*100),2)-100";
                    }
                    #endregion

                    #region Invoice(Sale) Summary Detail
                    if (FormatName == "With Sale")
                    {
                        //Exp RapValue
                        S = Global.ColumnIndexToColumnLetter(InvoiceRapAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(InvoiceRapAmountNo) + StrEndRow;
                        worksheet.Cells[2, 19, 2, 19].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                        worksheet.Cells[3, 19, 3, 19].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                        // Exp Amount Formula
                        S = Global.ColumnIndexToColumnLetter(InvoiceAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(InvoiceAmountNo) + StrEndRow;
                        worksheet.Cells[2, 22, 2, 22].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                        worksheet.Cells[3, 22, 3, 22].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                        // Exp Price Per Carat Formula
                        worksheet.Cells[2, 21, 2, 21].Formula = "ROUND(V2/F2,2)";
                        worksheet.Cells[3, 21, 3, 21].Formula = "ROUND(V3/F3,2)";

                        // Exp Discount Formula (Rap%)
                        S = Global.ColumnIndexToColumnLetter(InvoiceAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(InvoiceAmountNo) + StrEndRow;

                        worksheet.Cells[2, 20, 2, 20].Formula = "ROUND(SUM(((V2)/((S2*1)))*100),2)-100";
                        worksheet.Cells[3, 20, 3, 20].Formula = "ROUND(SUM(((V3)/((S3*1)))*100),2)-100";
                    }
                    #endregion

                    if (FormatName == "With Exp") //#P : 06-08-2020
                    {
                        worksheet.Cells[1, 4, 4, 18].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[1, 4, 4, 18].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[1, 4, 4, 18].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 18].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 18].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 18].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 18].Style.Font.Name = "Calibri";
                        worksheet.Cells[1, 4, 4, 18].Style.Font.Size = 9;

                        worksheet.Cells[1, 4, 1, 18].Style.Font.Bold = true;
                        worksheet.Cells[1, 4, 1, 18].Style.Font.Color.SetColor(Color.White);
                        worksheet.Cells[1, 4, 1, 18].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 4, 1, 18].Style.Fill.PatternColor.SetColor(BackColor);
                        worksheet.Cells[1, 4, 1, 18].Style.Fill.BackgroundColor.SetColor(BackColor);

                        worksheet.Cells[1, 15, 3, 18].Style.Font.Color.SetColor(Color.Red);

                    }
                    else if (FormatName == "With Rapnet" || FormatName == "With Sale") //#P : 06-08-2020
                    {
                        worksheet.Cells[1, 4, 4, 22].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[1, 4, 4, 22].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[1, 4, 4, 22].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 22].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 22].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 22].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 22].Style.Font.Name = "Calibri";
                        worksheet.Cells[1, 4, 4, 22].Style.Font.Size = 9;

                        worksheet.Cells[1, 4, 1, 22].Style.Font.Bold = true;
                        worksheet.Cells[1, 4, 1, 22].Style.Font.Color.SetColor(Color.White);
                        worksheet.Cells[1, 4, 1, 22].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 4, 1, 22].Style.Fill.PatternColor.SetColor(BackColor);
                        worksheet.Cells[1, 4, 1, 22].Style.Fill.BackgroundColor.SetColor(BackColor);

                        worksheet.Cells[1, 15, 3, 18].Style.Font.Color.SetColor(Color.Red);

                        if (FormatName == "With Sale")
                        {
                            worksheet.Cells[1, 19, 1, 22].Style.Font.Color.SetColor(Color.FromArgb(174, 201, 121)); //Green
                            worksheet.Cells[2, 19, 3, 22].Style.Font.Color.SetColor(Color.FromArgb(0, 150, 68)); //Green
                        }
                        else
                        {
                            worksheet.Cells[1, 19, 1, 22].Style.Font.Color.SetColor(FontColor); //Blue
                            worksheet.Cells[2, 19, 3, 22].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192)); //Blue
                        }
                    }
                    else
                    {
                        worksheet.Cells[1, 4, 4, 14].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[1, 4, 4, 14].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[1, 4, 4, 14].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 14].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 14].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 14].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 14].Style.Font.Name = "Calibri";
                        worksheet.Cells[1, 4, 4, 14].Style.Font.Size = 9;

                        worksheet.Cells[1, 4, 1, 14].Style.Font.Bold = true;
                        worksheet.Cells[1, 4, 1, 14].Style.Font.Color.SetColor(Color.White);
                        worksheet.Cells[1, 4, 1, 14].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 4, 1, 14].Style.Fill.PatternColor.SetColor(BackColor);
                        worksheet.Cells[1, 4, 1, 14].Style.Fill.BackgroundColor.SetColor(BackColor);
                    }

                    worksheet.Cells[1, 4, 3, 4].Style.Font.Bold = true;
                    worksheet.Cells[1, 4, 3, 4].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, 4, 3, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, 4, 3, 4].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[1, 4, 3, 4].Style.Fill.BackgroundColor.SetColor(BackColor);

                    worksheet.Cells[1, 7, 3, 10].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, 7, 3, 10].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[1, 7, 3, 10].Style.Fill.BackgroundColor.SetColor(BackColor);



                    if (FormatName == "With Exp") //#P : 06-08-2020
                    {
                        worksheet.Column(11).OutlineLevel = 1;//RapValue
                        worksheet.Column(11).Collapsed = true;

                        worksheet.Column(15).OutlineLevel = 1; //ExpRapValue
                        worksheet.Column(15).Collapsed = true;
                        worksheet.Column(24).OutlineLevel = 1; //FLShade
                        worksheet.Column(24).Collapsed = true;
                    }
                    if (FormatName == "With Rapnet" || FormatName == "With Sale") //#P : 06-08-2020
                    {
                        worksheet.Column(11).OutlineLevel = 1;//RapValue
                        worksheet.Column(11).Collapsed = true;

                        worksheet.Column(15).OutlineLevel = 1; //ExpRapValue
                        worksheet.Column(15).Collapsed = true;

                        worksheet.Column(19).OutlineLevel = 1; //RapnetRapValue/SaleRapValue
                        worksheet.Column(19).Collapsed = true;

                        worksheet.Column(28).OutlineLevel = 1; //FLShade
                        worksheet.Column(28).Collapsed = true;
                    }
                    else
                    {
                        worksheet.Column(11).OutlineLevel = 1;//RapValue
                        worksheet.Column(11).Collapsed = true;

                        worksheet.Column(20).OutlineLevel = 1;
                        worksheet.Column(20).Collapsed = true;
                    }

                    #endregion

                    #endregion

                    #region Inclusion Detail

                    AddInclusionDetail(worksheetInclusion, DTabInclusion);

                    #endregion

                    #region Proporstion Detail

                    worksheetProportion.Cells[2, 2, 3, 17].Value = "Stock Proportion";
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Font.Name = FontName;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Font.Size = 20;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Font.Bold = true;

                    worksheetProportion.Cells[2, 2, 3, 17].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 17].Merge = true;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheetProportion.Cells[2, 2, 3, 17].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Font.Color.SetColor(FontColor);

                    int NewRow = 6;
                    AddProportionDetail(worksheetProportion, DTabSize, worksheet.Name, 6, 2, "SIZE WISE SUMMARY", "Size", "Size", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabShape, worksheet.Name, 6, 11, "SHAPE WISE SUMMARY", "Shape", "Shape", StrStartRow, StrEndRow, DTabDetail);

                    if (DTabSize.Rows.Count > DTabShape.Rows.Count)
                    {
                        NewRow = NewRow + DTabSize.Rows.Count + 5;
                    }
                    else
                    {
                        NewRow = NewRow + DTabShape.Rows.Count + 5;
                    }

                    AddProportionDetail(worksheetProportion, DTabClarity, worksheet.Name, NewRow, 2, "CLARITY WISE SUMMARY", "Clarity", "ClaGroup", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabColor, worksheet.Name, NewRow, 11, "COLOR WISE SUMMARY", "Color", "ColGroup", StrStartRow, StrEndRow, DTabDetail);


                    if (DTabClarity.Rows.Count > DTabColor.Rows.Count)
                    {
                        NewRow = NewRow + DTabClarity.Rows.Count + 5;
                    }
                    else
                    {
                        NewRow = NewRow + DTabColor.Rows.Count + 5;
                    }

                    AddProportionDetail(worksheetProportion, DTabCut, worksheet.Name, NewRow, 2, "CUT WISE SUMMARY", "Cut", "CutGroup", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabPolish, worksheet.Name, NewRow, 11, "POLISH WISE SUMMARY", "Pol", "PolGroup", StrStartRow, StrEndRow, DTabDetail);


                    if (DTabCut.Rows.Count > DTabPolish.Rows.Count)
                    {
                        NewRow = NewRow + DTabCut.Rows.Count + 5;
                    }
                    else
                    {
                        NewRow = NewRow + DTabPolish.Rows.Count + 5;
                    }

                    AddProportionDetail(worksheetProportion, DTabSym, worksheet.Name, NewRow, 2, "SYM WISE SUMMARY", "Sym", "SymGroup", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabFL, worksheet.Name, NewRow, 11, "FL WISE SUMMARY", "FL", "FLGroup", StrStartRow, StrEndRow, DTabDetail);

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

        public void AddInclusionDetail(ExcelWorksheet worksheet, DataTable pDtab)
        {
            Color BackColor = Color.FromArgb(2, 68, 143);
            Color FontColor = Color.White;
            string FontName = "Calibri";
            float FontSize = 9;


            worksheet.Cells[2, 3, 4, 13].Value = "Rijia Gems Export Inclusion Grading";
            worksheet.Cells[2, 3, 4, 13].Style.Font.Name = FontName;
            worksheet.Cells[2, 3, 4, 13].Style.Font.Size = 20;
            worksheet.Cells[2, 3, 4, 13].Style.Font.Bold = true;

            worksheet.Cells[2, 3, 4, 13].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            worksheet.Cells[2, 3, 4, 13].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            worksheet.Cells[2, 3, 4, 13].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            worksheet.Cells[2, 3, 4, 13].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            worksheet.Cells[2, 3, 4, 13].Merge = true;
            worksheet.Cells[2, 3, 4, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 3, 4, 13].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;


            worksheet.Cells[2, 3, 4, 13].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[2, 3, 4, 13].Style.Fill.PatternColor.SetColor(BackColor);
            worksheet.Cells[2, 3, 4, 13].Style.Fill.BackgroundColor.SetColor(BackColor);
            worksheet.Cells[2, 3, 4, 13].Style.Font.Color.SetColor(FontColor);

            DataTable DTabDistinct = pDtab.DefaultView.ToTable(true, "PARATYPE_ID", "PARATYPECODE", "PARATYPENAME");
            DTabDistinct.DefaultView.Sort = "PARATYPE_ID";
            DTabDistinct = DTabDistinct.DefaultView.ToTable();

            int StartRow = 0;
            int StartColumn = 3;
            int IntRow = 0;

            int[] array = new int[4];

            for (int i = 0; i < DTabDistinct.Rows.Count; i++)
            {
                string Str = Val.ToString(DTabDistinct.Rows[i]["PARATYPECODE"]);
                string StrName = Val.ToString(DTabDistinct.Rows[i]["PARATYPENAME"]);

                DataTable DTab = pDtab.Select("PARATYPECODE='" + Str + "'").CopyToDataTable();

                if (i % 4 == 0)
                {
                    StartColumn = 3;
                    StartRow = IntRow + (i % 4) + (array.Max() == 0 ? 6 : array.Max()) + 2;
                    IntRow = StartRow;
                    array = new int[4];
                }
                else
                {
                    StartRow = IntRow;
                }
                array[i % 4] = DTab.Rows.Count;

                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Value = StrName;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Merge = true;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Fill.PatternColor.SetColor(BackColor);
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Fill.BackgroundColor.SetColor(BackColor);
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Color.SetColor(FontColor);
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Name = FontName;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Size = 11;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Bold = true;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                StartRow = StartRow + 1;
                for (int J = 0; J < DTab.Rows.Count; J++)
                {
                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Value = Val.ToString(DTab.Rows[J]["CODE"]);
                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Style.Font.Bold = true;
                    worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = Val.ToString(DTab.Rows[J]["NAME"]);
                    StartRow = StartRow + 1;

                }
                worksheet.Column(StartColumn + 1).Width = 20;

                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Name = FontName;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Size = 11;

                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Name = FontName;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Size = FontSize;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                StartColumn = StartColumn + 3;

            }
            worksheet.Cells[1, 1, 50, 50].AutoFitColumns();
        }

        #endregion
        private void BtnExport_Click(object sender, EventArgs e)
        {
            string StrFileName = ExportExcelNew();
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

        #region GridEvent
        private void GrdDetail_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000")
            {
                e.DisplayText = String.Empty;
            }
        }

        private void GrdDetail_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            Global.CustomDrawColumnHeader(sender, e);
        }
        private void GrdDetail_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }

                string StrCol = Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "STATUS"));
                string StrStockType = Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "STOCKTYPE"));
                if (StrStockType == "SINGLE")
                {
                    if (StrCol.ToUpper() == "AVAILABLE")
                    {
                        e.Appearance.BackColor = lblAvailable.BackColor;
                        e.Appearance.BackColor2 = lblAvailable.BackColor;
                    }
                    else if (StrCol.ToUpper() == "NONE")
                    {
                        e.Appearance.BackColor = lblNone.BackColor;
                        e.Appearance.BackColor2 = lblNone.BackColor;
                    }
                    else if (StrCol.ToUpper() == "LAB")
                    {
                        e.Appearance.BackColor = lblInLab.BackColor;
                        e.Appearance.BackColor2 = lblInLab.BackColor;
                    }
                    else if (StrCol.ToUpper() == "LAB-RETURN")
                    {
                        e.Appearance.BackColor = lblLabReturn.BackColor;
                        e.Appearance.BackColor2 = lblLabReturn.BackColor;
                    }
                    else if (StrCol.ToUpper() == "LAB-RESULT")
                    {
                        e.Appearance.BackColor = lblLabResult.BackColor;
                        e.Appearance.BackColor2 = lblLabResult.BackColor;
                    }
                    else if (StrCol.ToUpper() == "ASSISS")
                    {
                        e.Appearance.BackColor = lblAssortIssue.BackColor;
                        e.Appearance.BackColor2 = lblAssortIssue.BackColor;
                    }
                    else if (StrCol.ToUpper() == "ASSRET")
                    {
                        e.Appearance.BackColor = lblAssortreturn.BackColor;
                        e.Appearance.BackColor2 = lblAssortreturn.BackColor;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDetail_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                if (GrdDetail.FocusedRowHandle < 0)
                    return;
                if (e.Clicks == 2 && Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("PARTYSTOCKNO"))
                {
                    FrmStoneHistory FrmStoneHistory = new FrmStoneHistory();
                    FrmStoneHistory.MdiParent = Global.gMainRef;
                    FrmStoneHistory.ShowForm(Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "STOCK_ID")), Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "PARTYSTOCKNO")), Stock.FrmStoneHistory.FORMTYPE.DISPLAY);
                }
                if (e.Clicks == 2 && Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("ISCERTI"))
                {
                    string CertificateUrl = Val.ToString(GrdDetail.GetFocusedRowCellValue("CERTIFICATEURL"));
                    System.Diagnostics.Process.Start(CertificateUrl, "cmd");
                }
                if (e.Clicks == 2 && Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("ISVIDEO"))
                {
                    string VideoUrl = Val.ToString(GrdDetail.GetFocusedRowCellValue("VIDEOURL"));
                    System.Diagnostics.Process.Start(VideoUrl, "cmd");
                }
                if (e.Clicks == 2 && Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("ISIMAGE"))
                {
                    string ImageUrl = Val.ToString(GrdDetail.GetFocusedRowCellValue("IMAGEURL"));
                    System.Diagnostics.Process.Start(ImageUrl, "cmd");
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }

        }

        #endregion

        private void BtnSelectedPrint_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                //DataTable DTab = GetSelectedRowToTable();
                DataTable DTab = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);
                if (DTab.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select Atleast One Record");
                    return;
                }
                DataSet DS = new DataSet();
                DTab.TableName = "Table";
                DS.Tables.Add(DTab);

                Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                FrmReportViewer.MdiParent = Global.gMainRef;
                FrmReportViewer.ShowFormInvoicePrint("LiveStockReport", DTab);
                this.Cursor = Cursors.Default;

            }

            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnLabIssue_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {


                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                string StrStoneNo = string.Empty;
                string StrStoneNoForAvgPrice = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                        return;
                    }

                    if ((Val.ToString(DRow["STATUS"]) == "LAB") && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }

                    if (Val.Val(DRow["AVGPRICEPERCARAT"]) == 0 || Val.Val(DRow["AVGAMOUNT"]) == 0)
                    {
                        StrStoneNoForAvgPrice = StrStoneNoForAvgPrice + Val.ToString(DRow["STOCKNO"]) + "\n";
                    }

                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Available Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                string strStoneNo = "";
                DtInvDetail.DefaultView.Sort = "SERIALNO";
                if (DtInvDetail.Rows.Count > 0)
                {
                    var list = DtInvDetail.AsEnumerable().Select(r => r["STOCKNO"].ToString());
                    strStoneNo = string.Join(",", list);
                }

                //Comment and Added By Gunjan:08/10/2024
                FrmGIAControlNoMap FrmGIAControlNoMap = new FrmGIAControlNoMap();
                FrmGIAControlNoMap.MdiParent = Global.gMainRef;
                FrmGIAControlNoMap.ShowForm(strStoneNo);
                //FrmMemoEntryNew FrmMemoEntryNew = new FrmMemoEntryNew();
                //FrmMemoEntryNew.ShowForm(Stock.FrmMemoEntryNew.FORMTYPE.LABISSUE, DtInvDetail, mStrStockType);
                //End As Gunjan


                //BtnGIAExport_Click(null, null);
                this.Cursor = Cursors.Default;
                if (ObjGridSelection != null)
                {
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnLabReturn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                        return;
                    }
                    if (Val.ToString(DRow["STATUS"]) != "LAB" && Val.ToString(DRow["STATUS"]) != "LAB-RESULT" && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Memo Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                if (DtInvDetail.DefaultView.ToTable(true, "BILLPARTY_ID").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                    return;
                }

                FrmMemoEntryNew FrmMemoEntryNew = new FrmMemoEntryNew();
                FrmMemoEntryNew.ShowForm(Stock.FrmMemoEntryNew.FORMTYPE.LABRETURN, DtInvDetail, mStrStockType);
                this.Cursor = Cursors.Default;
                if (ObjGridSelection != null)
                {
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnPriceUpdate_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtStoneDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);

                if (DtStoneDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                string StrStoneNo = string.Empty;
                string StrStoneNoList = string.Empty;

                foreach (DataRow DRow in DtStoneDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                        return;
                    }
                    //if (Val.ToString(DRow["STATUS"]) != "OFFLINE" && Val.ToString(DRow["STATUS"]) != "AVAILABLE" && Val.ToString(DRow["STATUS"]) != "NONE" && Val.ToString(DRow["STATUS"]) != "PURCHASE"
                    //    && Val.ToString(DRow["STATUS"]) != "LAB-RETURN"
                    //    && Val.ToString(DRow["STATUS"]) != "FACTGRD"
                    //    )
                    //{
                    //    StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    //}
                    //else
                    //{
                        StrStoneNoList = StrStoneNoList + Val.ToString(DRow["STOCKNO"]) + ",";
                    //}
                }

                //if (StrStoneNo != string.Empty)
                //{
                //    this.Cursor = Cursors.Default;
                //    Global.Message("Opps... Kindly Select 'Available/None/Offline/LabReturn' Status Stones\n\nThis Packets Have Another Status\n\n" + StrStoneNo);
                //    return;
                //}

                if (StrStoneNoList.Length != 0)
                {
                    StrStoneNoList = StrStoneNoList.Substring(0, StrStoneNoList.Length - 1);
                }

                FrmParameterUpdate FrmParameterUpdate = new FrmParameterUpdate();
                FrmParameterUpdate.MdiParent = Global.gMainRef;
                FrmParameterUpdate.Tag = "PriceOrParameterUpdate";
                FrmParameterUpdate.ShowForm(StrStoneNoList);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void AssortmentIssue_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                if (DtInvDetail.DefaultView.ToTable(true, "BILLPARTY_ID").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                    return;
                }

                FrmMemoEntryNew FrmMemoEntryNew = new FrmMemoEntryNew();
                FrmMemoEntryNew.ShowForm(Stock.FrmMemoEntryNew.FORMTYPE.ASSORTERISSUE, DtInvDetail, mStrStockType);
                this.Cursor = Cursors.Default;
                if (ObjGridSelection != null)
                {
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void AssortmentReturn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = new DataTable();
                DtInvDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                        return;
                    }
                    if (Val.ToString(DRow["STATUS"]) != "ASSISS")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Memo Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                if (DtInvDetail.DefaultView.ToTable(true, "BILLPARTY_ID").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                    return;
                }

                FrmMemoEntryNew FrmMemoEntryNew = new FrmMemoEntryNew();
                FrmMemoEntryNew.ShowForm(Stock.FrmMemoEntryNew.FORMTYPE.ASSORTERRETURN, DtInvDetail, mStrStockType);
                this.Cursor = Cursors.Default;
                if (ObjGridSelection != null)
                {
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void RbtBarcode_CheckedChanged(object sender, EventArgs e)
        {
            if (RbtBarcode.Checked)
            {
                txtStoneNo.Text = string.Empty;
                txtBarcode.Focus();
            }
            else if (RbtStockNo.Checked)
            {
                txtBarcode.Text = string.Empty;
                txtStoneNo.Focus();
            }
            PanelBarcode.Visible = RbtBarcode.Checked;
            PanelPktSerialNo.Visible = RbtStockNo.Checked;
        }

        private void BtnGIAExport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DTabNew = new DataTable();
                DTabNew = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);
                string strMFG_Id = "";

                if (DTabNew == null || DTabNew.Rows.Count == 0)
                {
                    Global.Message("Please Select at lease One Record For Transfer");
                    return;
                    this.Cursor = Cursors.Default;
                }
                if (DTabNew.Rows.Count > 0)
                {
                    var list = DTabNew.AsEnumerable().Select(r => r["MEMO_ID"].ToString());
                    strMFG_Id = string.Join(",", list);
                }
                else
                {
                    strMFG_Id = Val.ToString(GrdDetail.GetFocusedRowCellValue("MEMO_ID"));
                }
                DataTable DTabGetData = ObjRap.ExportPackingList(strMFG_Id);
                if (DTabGetData.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select Atleast One Record");
                    return;
                    this.Cursor = Cursors.Default;
                }

                Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                FrmReportViewer.MdiParent = Global.gMainRef;
                FrmReportViewer.ShowFormInvoicePrint("RPT_LabIssueStonePrint", DTabGetData);
                this.Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }
       
        private void txtStoneNo_MouseDown(object sender, MouseEventArgs e)
        {
            if (txtStoneNo.Focus())
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    PasteData = Convert.ToString(PasteclipData.GetData(System.Windows.Forms.DataFormats.Text));
                }
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
                    str1 = str1.Replace("\r", "");
                }

                txtStoneNo.Text = str1;
                //rTxtStoneCertiMfgMemo.Text = str1.Trim().TrimStart().TrimEnd();
                txtStoneNo.Select(txtStoneNo.Text.Length, 0);
                //rTxtStoneCertiMfgMemo.Text = rTxtStoneCertiMfgMemo.Text.Trim().TrimStart().TrimEnd();

                //lblTotalCount.Text = "(" + rTxtStoneCertiMfgMemo.Text.Split(',').Length + ")";

                
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }


        private void txtStoneNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnRefresh_Click(null, null);
            }
        }

        private void BtnFrontPrint_Click(object sender, EventArgs e)
        {
            try
            {

                DataTable DTab = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);

                if (DTab.Rows.Count == 0 || DTab == null)
                {
                    Global.Message("Please Select At Least One Record For Barcode Print.. ");
                    return;
                }

                if (Global.Confirm("Are you Sure You Want For Print Barcode of All Selected Packets?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                string StrBatchFileName = "";
                StrBatchFileName = Application.StartupPath + "\\TSC_MakableBarcodeNew.txt ";

                string[] lines = File.ReadAllLines(StrBatchFileName);
                string PATH = lines[0];

                DialogResult dialogResult = MessageBox.Show("Sure You Want To PRINT ", "PRINT", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    TextWriter txt = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "TSC_TE210.txt");
                    StringBuilder SB = new StringBuilder();
                    SB.Length = 0;
                    try
                    {
                        DTab.DefaultView.Sort = "PARTYSTOCKNO ASC";

                        // Apply the sorted view back to the grid or as a new table
                        DTab = DTab.DefaultView.ToTable();
                        for (int i = 0; i < DTab.Rows.Count; i++)
                        {

                            string ID = "", BLACK = "", TABLE = "", LUSTER = "", TOO = "", COO = "", POO = "", HA = "", BRO = "", MILKY = "";

                            ID = DTab.Rows[i]["PARTYSTOCKNO"].ToString();
                            BLACK = DTab.Rows[i]["TABLEBLACKNAME"].ToString();
                            TABLE = DTab.Rows[i]["TABLEINCNAME"].ToString();
                            LUSTER = DTab.Rows[i]["LUSTERNAME"].ToString();
                            TOO = DTab.Rows[i]["TABLEOPENNAME"].ToString();
                            COO = "";
                            POO = "";
                            //COMMENT AND GDDED BY GUNJAN:01/03/2024
                            HA = "";
                            //HA = DTab.Rows[i]["HA"].ToString();

                            BRO = "";
                            MILKY = DTab.Rows[i]["MILKYNAME"].ToString();
                            //END AS GUNJAN

                            // SB.AppendLine("     ");

                            SB.AppendLine("<xpml><page quantity='0' pitch='30.0 mm'></xpml>SIZE 57.5 mm, 30 mm ");
                            SB.AppendLine("GAP 2 mm, 0 mm ");
                            SB.AppendLine("DIRECTION 0,0 ");
                            SB.AppendLine("REFERENCE 0,0 ");
                            SB.AppendLine("OFFSET 0 mm ");
                            SB.AppendLine("SET TEAR OFF ");
                            SB.AppendLine("SET PEEL OFF ");
                            SB.AppendLine("SET PARTIAL_CUTTER OFF ");
                            SB.AppendLine("<xpml></page></xpml><xpml><page quantity='1' pitch='30.0 mm'></xpml>SET CUTTER 1 ");
                            SB.AppendLine("CLS ");
                            SB.AppendLine("BOX 5,8,453,185,2 ");
                            SB.AppendLine("BAR 125,120, 288, 2 ");
                            SB.AppendLine("BAR 6,64, 407, 2 ");
                            SB.AppendLine("BAR 158,9, 2, 176 ");
                            SB.AppendLine("BAR 253,9, 2, 176 ");
                            SB.AppendLine("BAR 317,9, 2, 176 ");
                            SB.AppendLine("BAR 285,9, 2, 176 ");
                            SB.AppendLine("BAR 126,9, 2, 176 ");
                            SB.AppendLine("BAR 189,9, 2, 176 ");
                            SB.AppendLine("BAR 222,9, 2, 176 ");
                            SB.AppendLine("BAR 86,9, 2, 176 ");
                            SB.AppendLine("BAR 46,9, 2, 176 ");
                            SB.AppendLine("BAR 413,9, 2, 176 ");
                            SB.AppendLine("BAR 349,9, 2, 176 ");
                            SB.AppendLine("CODEPAGE 1252 ");
                            SB.AppendLine("TEXT 403,17,\"0\",90,7,7,\"BLA\" ");
                            SB.AppendLine("TEXT 371,17,\"0\",90,7,7,\"TAB\" ");
                            SB.AppendLine("TEXT 339,17,\"0\",90,7,7,\"MIL\" ");
                            SB.AppendLine("TEXT 307,17,\"0\",90,7,7,\"LUS\" ");
                            SB.AppendLine("TEXT 275,17,\"0\",90,7,7,\"T.O.\" ");
                            SB.AppendLine("TEXT 211,17,\"0\",90,7,7,\"P.O.\" ");
                            SB.AppendLine("TEXT 180,17,\"0\",90,7,7,\"BRN\" ");
                            SB.AppendLine("TEXT 147,17,\"0\",90,7,7,\"H&A\" ");
                            SB.AppendLine("TEXT 243,17,\"0\",90,7,7,\"C.O.\" ");
                            SB.AppendLine("TEXT 36,17,\"0\",90,7,7,\"COM\" ");
                            SB.AppendLine("TEXT 116,17,\"0\",90,7,7,\"DIS%\" ");
                            SB.AppendLine("BARCODE 453,228,\"39\",41,0,180,2,5,\"" + ID + "/\" ");
                            //  SB.AppendLine("TEXT 453,182,\"0\",180,8,8,\"111-1111\" ");
                            SB.AppendLine("TEXT 451,33,\"0\",90,11,11,\"" + ID + "\" ");
                            SB.AppendLine("BAR 381,9, 2, 176 ");
                            SB.AppendLine("TEXT 75,17,\"0\",90,7,7,\"GRD\" ");
                            SB.AppendLine("TEXT 403,73,\"0\",90,7,7,\"" + BLACK + "\" ");
                            SB.AppendLine("TEXT 371,73,\"0\",90,7,7,\"" + TABLE + "\" ");
                            SB.AppendLine("TEXT 339,73,\"0\",90,7,7,\"" + MILKY + "\" ");
                            SB.AppendLine("TEXT 307,73,\"0\",90,7,7,\"" + LUSTER + "\" ");
                            SB.AppendLine("TEXT 275,73,\"0\",90,7,7,\"" + TOO + "\" ");
                            SB.AppendLine("TEXT 243,73,\"0\",90,7,7,\"" + COO + "\" ");
                            SB.AppendLine("TEXT 211,73,\"0\",90,7,7,\"" + POO + "\" ");
                            SB.AppendLine("TEXT 179,73,\"0\",90,7,7,\"" + BRO + "\" ");
                            SB.AppendLine("TEXT 151,81,\"0\",90,18,18,\"" + HA + "\" ");
                            SB.AppendLine("PRINT 1,1 ");
                            SB.AppendLine("<xpml></page></xpml><xpml><end/></xpml> ");
                        }
                    }
                    catch (Exception ex)
                    {
                        Global.Message(ex.Message.ToString());
                        this.Cursor = Cursors.Default;
                    }
                    txt.Write(SB.ToString());
                    txt.Close();

                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C COPY " + AppDomain.CurrentDomain.BaseDirectory + "TSC_TE210.txt " + PATH;
                    process.StartInfo = startInfo;
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
                this.Cursor = Cursors.Default;
            }
        }

        private void BtnBackPrint_Click(object sender, EventArgs e)
        {
            try
            {


                DataTable DTab = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);

                if (DTab.Rows.Count == 0 || DTab == null)
                {
                    Global.Message("Please Select At Least One Record For Barcode Print.. ");
                    return;
                }

                if (Global.Confirm("Are you Sure You Want For Print Barcode of All Selected Packets?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                string StrBatchFileName = "";
                StrBatchFileName = Application.StartupPath + "\\TSC_MakableBarcodeNew.txt ";

                string[] lines = File.ReadAllLines(StrBatchFileName);
                string PATH = lines[0];

                DialogResult dialogResult = MessageBox.Show("Sure You Want To PRINT ", "PRINT", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {

                    TextWriter txt = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "TSC_TE210.txt");
                    StringBuilder SB1 = new StringBuilder();
                    SB1.Length = 0;

                    try
                    {
                        DTab.DefaultView.Sort = "PARTYSTOCKNO ASC";

                        // Apply the sorted view back to the grid or as a new table
                        DTab = DTab.DefaultView.ToTable();
                        for (int i = 0; i < DTab.Rows.Count; i++)
                        {
                            string DPC = "", GIA = "", ID = "", weight = "", CUT = "", POL = "", SYM = "", FLO = "", PA = "", MEAS = "", SHAPE = "", SHADE = "", MILKY = "", COLOR = "", CLARITY = "";

                            // cno = grid.Rows[i].Cells[5].Value.ToString();

                            //Comment And Added By Gunjan:01/03/2024
                            //GIA = DTab.Rows[i]["GIA_SINMIX"].ToString();
                            GIA = "";
                            //End as Gunjan

                            ID = DTab.Rows[i]["PARTYSTOCKNO"].ToString();
                            weight = DTab.Rows[i]["CARAT"].ToString();
                            SHAPE = DTab.Rows[i]["SHAPENAME"].ToString();
                            CUT = DTab.Rows[i]["CUTNAME"].ToString();
                            POL = DTab.Rows[i]["POLNAME"].ToString();
                            SYM = DTab.Rows[i]["SYMNAME"].ToString();
                            FLO = DTab.Rows[i]["FLNAME"].ToString();
                            PA = DTab.Rows[i]["PAVANGLE"].ToString();
                            MEAS = DTab.Rows[i]["MEASUREMENT"].ToString();
                            SHADE = DTab.Rows[i]["COLORSHADENAME"].ToString();
                            MILKY = DTab.Rows[i]["MILKYNAME"].ToString();
                            COLOR = DTab.Rows[i]["COLORNAME"].ToString();
                            CLARITY = DTab.Rows[i]["CLARITYNAME"].ToString();
                            DPC = DTab.Rows[i]["COSTPRICEPERCARAT"].ToString();

                            SB1.AppendLine("<xpml><page quantity='0' pitch='30.0 mm'></xpml>SIZE 60 mm, 30 mm");
                            SB1.AppendLine("GAP 2 mm, 0 mm");
                            SB1.AppendLine("DIRECTION 0,0");
                            SB1.AppendLine("REFERENCE 0,0");
                            SB1.AppendLine("OFFSET 0 mm");
                            SB1.AppendLine("SET TEAR OFF");
                            SB1.AppendLine("SET PEEL OFF");
                            SB1.AppendLine("SET PARTIAL_CUTTER OFF");
                            if (DTab.Rows.Count - 1 == i)
                            {
                                SB1.AppendLine("<xpml></page></xpml><xpml><page quantity='1' pitch='30.0 mm'></xpml>SET CUTTER 1");//SET CUTTER 1
                            }
                            else
                            {
                                SB1.AppendLine("<xpml></page></xpml><xpml><page quantity='1' pitch='30.0 mm'></xpml>");//SET CUTTER 1
                            }

                            SB1.AppendLine("CLS");
                            SB1.AppendLine("BARCODE 335,223,\"39\",38,0,180,2,5,\"" + ID + "/\"");
                            SB1.AppendLine("CODEPAGE 1252");
                            SB1.AppendLine("TEXT 471,229,\"ROMAN.TTF\",180,1,11,\"" + ID + "\"");
                            SB1.AppendLine("TEXT 471,196,\"0\",180,10,10,\"" + SHAPE + "\"");
                            SB1.AppendLine("TEXT 471,157,\"ROMAN.TTF\",180,1,9,\"" + SHADE + "\"");
                            SB1.AppendLine("TEXT 471,133,\"ROMAN.TTF\",180,1,9,\"" + MILKY + "\"");
                            SB1.AppendLine("TEXT 471,109,\"ROMAN.TTF\",180,1,9,\"" + COLOR + "\"");
                            SB1.AppendLine("TEXT 471,85,\"ROMAN.TTF\",180,1,9,\"" + CLARITY + "\"");
                            SB1.AppendLine("TEXT 471,61,\"ROMAN.TTF\",180,1,9,\"" + DPC + "\"");
                            SB1.AppendLine("TEXT 471,29,\"ROMAN.TTF\",180,1,9,\"" + CUT + "\"");
                            SB1.AppendLine("TEXT 431,29,\"ROMAN.TTF\",180,1,9,\"" + POL + "\"");
                            SB1.AppendLine("TEXT 391,29,\"ROMAN.TTF\",180,1,9,\"" + SYM + "\"");
                            SB1.AppendLine("TEXT 343,29,\"ROMAN.TTF\",180,1,9,\"" + FLO + "\"");
                            SB1.AppendLine("TEXT 215,29,\"ROMAN.TTF\",180,1,9,\"" + GIA + "\"");
                            SB1.AppendLine("TEXT 63,29,\"ROMAN.TTF\",180,1,9,\"" + weight + "\"");
                            SB1.AppendLine("TEXT 335,181,\"ROMAN.TTF\",180,1,7,\"" + MEAS + "\"");
                            SB1.AppendLine("CODEPAGE 850");
                            SB1.AppendLine("TEXT 287,31,\"8\",180,1,1,\"" + PA + "\"");
                            SB1.AppendLine("PRINT 1,1");
                            SB1.AppendLine("<xpml></page></xpml><xpml><end/></xpml>");

                        }
                    }
                    catch (Exception ex)
                    {
                        Global.Message(ex.Message.ToString());
                        this.Cursor = Cursors.Default;
                    }
                    txt.Write(SB1.ToString());
                    txt.Close();
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C COPY " + AppDomain.CurrentDomain.BaseDirectory + "TSC_TE210.txt " + PATH;//"\\\\191.168.2.62\\TSC_TE210"
                    process.StartInfo = startInfo;
                    process.Start();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
                this.Cursor = Cursors.Default;
            }
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnRefresh_Click(null, null);
            }
        }

        private void lblSaveLayout_Click(object sender, EventArgs e)
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

        private void lblDefaultLayout_Click(object sender, EventArgs e)
        {
            int IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdDetail.Name);
            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Deleted");
            }
        }

        private void BtnMix_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                if (DtInvDetail.DefaultView.ToTable(true, "BILLPARTY_ID").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                    return;
                }

                FrmMemoEntryNew FrmMemoEntryNew = new FrmMemoEntryNew();
                FrmMemoEntryNew.ShowForm(Stock.FrmMemoEntryNew.FORMTYPE.UNGRADEDTOMIX, DtInvDetail, mStrStockType);
                this.Cursor = Cursors.Default;
                if (ObjGridSelection != null)
                {
                    ObjGridSelection.ClearSelection();
                    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void RBTLab_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //if(RBTLab.Checked == true)
                //{
                //    RBTLab.Checked = false;
                //}
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnBarcodePrint_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DTab = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);

                if (DTab.Rows.Count == 0 || DTab == null)
                {
                    Global.Message("Please Select At Least One Record For Barcode Print.. ");
                    return;
                }

                if (Global.Confirm("Are you Sure You Want For Print Barcode of All Selected Packets?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
                string StrBatchFileName = "";
                StrBatchFileName = Application.StartupPath + "\\BarcodePrintNew.txt ";

                string[] lines = File.ReadAllLines(StrBatchFileName);
                string PATH = lines[0];

                DialogResult dialogResult = MessageBox.Show("Sure You Want To PRINT ", "PRINT", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {

                    TextWriter txt = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "StickerPrn.txt");
                    StringBuilder SB1 = new StringBuilder();
                    SB1.Length = 0;

                    try
                    {
                        DTab.DefaultView.Sort = "PARTYSTOCKNO ASC";

                        // Apply the sorted view back to the grid or as a new table
                        DTab = DTab.DefaultView.ToTable();
                        for (int i = 0; i < DTab.Rows.Count; i++)
                        {
                            string ID = "", weight = "",SHAPE = "";

                            ID = DTab.Rows[i]["PARTYSTOCKNO"].ToString();
                            weight = DTab.Rows[i]["CARAT"].ToString();
                            SHAPE = DTab.Rows[i]["SHAPENAME"].ToString();                            

                            SB1.AppendLine("^AT");
                            SB1.AppendLine("^O0");
                            SB1.AppendLine("^D0");
                            SB1.AppendLine("^S4");
                            SB1.AppendLine("^H16");
                            SB1.AppendLine("^C1");
                            SB1.AppendLine("^P1");
                            SB1.AppendLine("^Q33.0,3.0");
                            SB1.AppendLine("^W100");
                            SB1.AppendLine("^L");
                            SB1.AppendLine("^O0");
                            SB1.AppendLine("^D0");
                            SB1.AppendLine("^C1");
                            SB1.AppendLine("^P1");
                            SB1.AppendLine("^Q15.0,3.0");
                            SB1.AppendLine("^W55");
                            SB1.AppendLine("^L");

                            SB1.AppendLine("BA,200,13,1,3,64,0,0," + ID + "/");
                            SB1.AppendLine("AB,360,85,1,1,0,0," + weight + "");
                            SB1.AppendLine("AD,23,13,1,1,0,0," + ID + "");
                            SB1.AppendLine("AD,23,51,1,1,0,0," + SHAPE + "");
                            SB1.AppendLine("E");

                        }
                    }
                    catch (Exception ex)
                    {
                        Global.Message(ex.Message.ToString());
                        this.Cursor = Cursors.Default;
                    }
                    txt.Write(SB1.ToString());
                    txt.Close();
                    System.Diagnostics.Process process = new System.Diagnostics.Process();
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    startInfo.FileName = "cmd.exe";
                    startInfo.Arguments = "/C COPY " + AppDomain.CurrentDomain.BaseDirectory + "StickerPrn.txt " + PATH;//"\\\\191.168.2.62\\TSC_TE210"
                    process.StartInfo = startInfo;
                    process.Start();
                    if (ObjGridSelection != null)
                    {
                        ObjGridSelection.ClearSelection();
                        ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                    }
                }
                
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
                this.Cursor = Cursors.Default;
            }
        }

        private void FrmSinglePacketLiveStockNew_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Global.gStrCompanyName.Contains("TRP"))
                {
                    cFlowLayoutPanel1.Visible = true;
                    ChkIsLabStock.Visible = true;
                }
                else
                {
                    cFlowLayoutPanel1.Visible = false;
                    ChkIsLabStock.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
                this.Cursor = Cursors.Default;
            }
        }
    }
}