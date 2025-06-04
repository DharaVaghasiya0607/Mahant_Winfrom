using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraPrinting;
using DevExpress.XtraGrid.Views.BandedGrid;

using DevExpress.Data;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid;
using BusLib.TableName;
using BusLib.Master;

using DevExpress.XtraPivotGrid;
using System.Collections;

namespace MahantExport.Report
{
    public partial class FrmPivotReportViewer : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        string OrderOn = string.Empty;
        string MergeOnStr = string.Empty;
        string MergeOn = string.Empty;
        string mStrFromDate = string.Empty;
        string mStrToDate = string.Empty;
        string mStrASOnDate = string.Empty;

        public DataTable DTabReportField = new DataTable();

        public DataTable DTabSummary = new DataTable();

        public DataRow DRowReportMast = null;
        public string mStrGroupByTag = "";
        public string mStrGroupByText = "";
        public string mStrOrderByTag = "";
        public string mStrOrderByText = "";
        public bool BoolSelection = false;

        public string mStrFilter = "";
        MST_ReportProperty mReportProperty = null;


        string mStrRowArea = string.Empty;
        string mStrColumnArea = string.Empty;
        string mStrDataArea = string.Empty;

        string mStrShiftType = "";

        #region Constructor

        public FrmPivotReportViewer()
        {
            InitializeComponent();
        }

        public void ShowForm(DataSet pDS, DataRow pDRowReportMast, DataTable pDTabFieldDetail, string pStrGroupByTag, string pStrGroupByText, string pStrFilterText, bool pBoolNoGroup, string pStrOrderByTag, string pStrOrderByText, MST_ReportProperty pReportProperty,
            string pStrFromDate,
            string pStrToDate,
            string pStrAsOnDate,
            string pStrRowArea,
            string pStrColumnArea,
            string pStrDataArea,
            int mIntFilterHeight,
            string pStrShiftType
         )
        {
            mStrFromDate = pStrFromDate;
            mStrToDate = pStrToDate;
            mStrASOnDate = pStrAsOnDate;

            mStrRowArea = pStrRowArea;
            mStrColumnArea = pStrColumnArea;
            mStrDataArea = pStrDataArea;

            //pFormType = mFormType;
            mReportProperty = pReportProperty;
            DRowReportMast = pDRowReportMast;
            DTabReportField = pDTabFieldDetail;
            DTabSummary = pDS.Tables[0];

            ChkNoGroup.EditValue = pBoolNoGroup;

            mStrGroupByTag = pStrGroupByTag;
            mStrGroupByText = pStrGroupByText;

            mStrOrderByTag = pStrOrderByTag;
            mStrOrderByText = pStrOrderByText;

            mStrFilter = pStrFilterText;
            mStrShiftType = pStrShiftType;
            Val.FormGeneralSetting(this);
            AttachFormEvents();
            CmbPrintOrientation.EditValue = Val.ToString(DRowReportMast["PrintOrientation"]);// "Portrait";

            if (mReportProperty.REPORTTYPE == "D")
            {
                BarBtnSummaryDetail.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
            {
                BarBtnSummaryDetail.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            }

            this.Show();
        }

        private void AttachFormEvents()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
        }

        #endregion

        #region Menu Events

        private void ToExcel_Click(object sender, EventArgs e)
        {
            Export("xls", "Export to Excel", "Excel files (*.xls)|*.xls|All files (*.*)|*.*");
        }

        private void ToText_Click(object sender, EventArgs e)
        {
            Export("txt", "Export to Text", "Text files (*.txt)|*.txt|All files (*.*)|*.*");
        }

        private void ToHTML_Click(object sender, EventArgs e)
        {
            Export("html", "Export to HTML", "Html files (*.html)|*.html|Htm files (*.htm)|*.htm");
        }

        private void ToRTF_Click(object sender, EventArgs e)
        {
            Export("rtf", "Export to RTF", "Word (*.doc) |*.doc;*.rtf|(*.txt) |*.txt|(*.*) |*.*");
        }

        private void ToPDF_Click(object sender, EventArgs e)
        {
            Export("pdf", "Export Report to PDF", "PDF (*.PDF)|*.PDF");
        }

        private void AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DgvPivot.BestFit();
        }

        private void ExpandTool_Click(object sender, EventArgs e)
        {
            DgvPivot.ExpandAll();
        }

        private void Collapse_Click(object sender, EventArgs e)
        {
            DgvPivot.CollapseAll();
        }

        private void PrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintableComponentLink link = new PrintableComponentLink(new PrintingSystem());
            link.Component = DgvPivot;
            link.Landscape = true;
            link.Margins.Left = 20;
            link.Margins.Right = 20;
            link.Margins.Bottom = 40;
            link.Margins.Top = 80;
            link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);
            link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
            link.CreateDocument();
            link.ShowPreview();
        }

        #endregion

        #region Events

        private void FrmPReportViewer_Load(object sender, EventArgs e)
        {
            LoadData(DTabSummary);
        }

        #endregion

        #region Operation

        private void Export(string format, string dlgHeader, string dlgFilter)
        {
            try
            {
                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = format;
                svDialog.Title = dlgHeader;
                svDialog.FileName = "Report";
                svDialog.Filter = dlgFilter;
                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    string Filepath = svDialog.FileName;
                    switch (format)
                    {
                        case "pdf":
                            DgvPivot.ExportToPdf(Filepath);
                            break;
                        case "xls":
                            DgvPivot.ExportToXls(Filepath);
                            break;
                        case "rtf":
                            DgvPivot.ExportToRtf(Filepath);
                            break;
                        case "txt":
                            DgvPivot.ExportToText(Filepath);
                            break;
                        case "html":
                            DgvPivot.ExportToHtml(Filepath);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void LoadData(DataTable pDTab)
        {
            InsertReportTrace();
            int IntError = 0;
            try
            {
                DgvPivot.BeginUpdate();
                DgvPivot.Fields.Clear();
                foreach (DataRow DRow in DTabReportField.Rows)
                {
                    IntError++;
                    PivotGridField Field = new PivotGridField();
                    Field.FieldName = DRow["FIELDNAME"].ToString();
                    Field.Caption = DRow["CAPTION"].ToString();
                    Field.Visible = Val.ToBoolean(DRow["ISVISIBLE"]);
                    Field.Appearance.Value.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
                    // Set Order
                    if (Val.ToString(DRow["ORDERTYPE"]) == "ASC")
                    {
                        Field.SortOrder = PivotSortOrder.Ascending;
                        Field.SortMode = PivotSortMode.Default;
                    }
                    else if (Val.ToString(DRow["ORDERTYPE"]) == "DESC")
                    {
                        Field.SortOrder = PivotSortOrder.Descending;
                        Field.SortMode = PivotSortMode.Default;
                    }
                    else if (Val.ToString(DRow["ORDERTYPE"]) == "CUSTOM")
                    {
                        OrderOn = OrderOn + Field.FieldName + ",";
                        Field.SortMode = PivotSortMode.Custom;
                        Field.Tag = Val.ToString(DRow["ORDERCOLUMN"]);
                    }

                    if (Val.ToString(DRow["DISPLAYFORMAT"]).Contains("{0:N"))
                    {
                        Field.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    }

                    if (Val.ToInt(DRow["COLUMNWIDTH"]) == 0)
                    {
                        Field.Width = 60;
                    }
                    else
                    {
                        Field.Width = Val.ToInt(DRow["COLUMNWIDTH"]);
                    }

                    if (Val.ToString(DRow["DATATYPE"].ToString()) == "S")
                    {
                        Field.CellFormat.FormatType = DevExpress.Utils.FormatType.Custom;
                        Field.SummaryType = DevExpress.Data.PivotGrid.PivotSummaryType.Max;
                        Field.CellFormat.FormatString = "S";
                    }
                    else
                    {
                        Field.CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        Field.CellFormat.FormatString = Val.ToString(DRow["DISPLAYFORMAT"]);
                    }

                    if (Val.ToBoolean(DRow["ISBOLD"]) == true)
                    {
                        Field.Appearance.Value.Font = new Font(Field.Appearance.Value.Font, FontStyle.Bold);
                    }
                    if (Val.ToBoolean(DRow["ISITALIC"]) == true)
                    {
                        Field.Appearance.Value.Font = new Font(Field.Appearance.Value.Font, FontStyle.Italic);
                    }
                    if (Val.ToBoolean(DRow["ISUNDERLINE"]) == true)
                    {
                        Field.Appearance.Value.Font = new Font(Field.Appearance.Value.Font, FontStyle.Underline);
                    }

                    // Set Format

                    if (Val.ToString(DRow["DATATYPE"]) == "date")
                    {
                        DgvPivot.Fields[DRow["FIELDNAME"].ToString()].CellFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        DgvPivot.Fields[DRow["FIELDNAME"].ToString()].CellFormat.FormatString = "dd/MM/yy";
                    }
                    else if (Val.ToString(DRow["DATATYPE"]).ToLower() == "time")
                    {
                        DgvPivot.Fields[DRow["FIELDNAME"].ToString()].CellFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        DgvPivot.Fields[DRow["FIELDNAME"].ToString()].CellFormat.FormatString = "hh:mm:ss tt";
                    }
                    else if (Val.ToString(DRow["DATATYPE"]).ToLower() == "datetime")
                    {
                        DgvPivot.Fields[DRow["FIELDNAME"].ToString()].CellFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        DgvPivot.Fields[DRow["FIELDNAME"].ToString()].CellFormat.FormatString = "dd/MM/yyyy hh:mm:ss tt";
                    }
                    else if (Val.ToString(DRow["DATATYPE"].ToString()) == "F")
                    {
                        DgvPivot.Fields[DRow["FIELDNAME"].ToString()].CellFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    }

                    // Set Area
                    if (mStrRowArea.Contains(DRow["FIELDNAME"].ToString())) //   Val.ToBoolean(DRow["ROWAREA"].ToString()) == true)
                    {
                        Field.Visible = true;
                        Field.Area = PivotArea.RowArea;
                        Field.Options.AllowExpand = DevExpress.Utils.DefaultBoolean.False;
                        DgvPivot.Fields.Add(Field);
                    }
                    else if (mStrColumnArea.Contains(DRow["FIELDNAME"].ToString()))
                    {
                        Field.Visible = true;
                        Field.Area = PivotArea.ColumnArea;
                        Field.Options.AllowExpand = DevExpress.Utils.DefaultBoolean.False;
                        DgvPivot.Fields.Add(Field);
                    }

                    else if (mStrDataArea.Contains(DRow["FIELDNAME"].ToString()))
                    {
                        Field.Visible = true;
                        Field.Area = PivotArea.DataArea;
                        Field.Options.AllowExpand = DevExpress.Utils.DefaultBoolean.False;
                        DgvPivot.Fields.Add(Field);
                    }

                }

                DgvPivot.DataSource = pDTab;

                DgvPivot.Appearance.FieldValue.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Bold);
                DgvPivot.AppearancePrint.FieldValue.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Bold);
                DgvPivot.Appearance.FieldHeader.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Bold);
                DgvPivot.AppearancePrint.FieldHeader.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Bold);
                DgvPivot.Appearance.ColumnHeaderArea.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Bold);
                DgvPivot.Appearance.DataHeaderArea.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Bold);
                DgvPivot.Appearance.FilterHeaderArea.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Bold);
                DgvPivot.Appearance.HeaderArea.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Bold);
                DgvPivot.Appearance.FieldValueGrandTotal.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Bold);
                DgvPivot.AppearancePrint.FieldValueGrandTotal.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Bold);
                DgvPivot.Appearance.FieldValueTotal.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Bold);
                DgvPivot.AppearancePrint.FieldValueTotal.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Bold);
                DgvPivot.Appearance.GrandTotalCell.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Bold);
                DgvPivot.AppearancePrint.GrandTotalCell.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Bold);
                DgvPivot.Appearance.TotalCell.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Bold);
                DgvPivot.AppearancePrint.TotalCell.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Bold);
                DgvPivot.OptionsPrint.UsePrintAppearance = true;
                DgvPivot.OptionsSelection.MultiSelect = true;
                DgvPivot.EndUpdate();
                DgvPivot.BestFit();
            }
            catch (Exception ex)
            {
                Global.Message("Error In Column Index : " + IntError.ToString() + "    " + ex.Message);
            }
        }

        public void InsertReportTrace()
        {
            //string MM = Val.ToString(DateTime.Today.Month);
            //if (Val.ToInt(MM) < 10)
            //{
            //    MM = "0" + MM;
            //}
            //int YYMM = Val.ToInt(Val.ToString(DateTime.Today.Year) + MM);
            //int SRNO = ObjNewReport.FindNewSrNo(YYMM);
            //ObjNewReport.SaveReportTrace(YYMM, 0, Report_Code, Report_Type);
            //string MM = Val.ToString(DateTime.Today.Month);
            //if (Val.ToInt(MM) < 10)
            //{
            //    MM = "0" + MM;
            //}
            //int YYMM = Val.ToInt(Val.ToString(DateTime.Today.Year) + MM);
        }

        #endregion

        #region Pivot Grid Events

        private void DgvPivot_CustomDrawCell(object sender, PivotCustomDrawCellEventArgs e)
        {
            try
            {
                if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000")
                {
                    e.Appearance.ForeColor = System.Drawing.Color.White;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion



        private void DgvPivot_CellClick(object sender, PivotCellEventArgs e)
        {
            //PivotCellEventArgs CurrentCell = DgvPivot.Cells.GetFocusedCellInfo();

            //PivotGridField[] fields = CurrentCell.GetRowFields();
            //string Str  = CurrentCell.GetFieldValue(fields[0])

            //string StrDepartmentName = Val.ToString(CurrentCell.GetFieldValue(CurrentCell.ColumnField));
            //string StrColumnName = Val.ToString(CurrentCell.DataField.FieldName);
            //string StrProcessName = Val.ToString(CurrentCell.GetFieldValue(CurrentCell.RowField));

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DgvPivot_CustomCellDisplayText(object sender, PivotCellDisplayTextEventArgs e)
        {
            //if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000")
            //{
            //    e.DisplayText = String.Empty;
            //    // e.Appearance.ForeColor = System.Drawing.Color.White;
            //}
        }

        private void DgvPivot_CustomFieldSort(object sender, PivotGridCustomFieldSortEventArgs e)
        {
            if (OrderOn.Contains(e.Field.FieldName))
            {
                object orderValue1 = e.GetListSourceColumnValue(e.ListSourceRowIndex1, Val.ToString(e.Field.Tag)),
                        orderValue2 = e.GetListSourceColumnValue(e.ListSourceRowIndex2, Val.ToString(e.Field.Tag));
                e.Result = Comparer.Default.Compare(orderValue1, orderValue2);
            }

        }


        private void BtnPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                //if (ObjPer.AllowPrint == false) // If Condition Add by Khushbu 07/04/2014
                //{
                //    Global.Message(BLL.GlobalDec.gStrPermissionPrintMsg);
                //    return;
                //}

                DevExpress.XtraPrinting.PrintingSystem PrintSystem = new DevExpress.XtraPrinting.PrintingSystem();

                PrinterSettingsUsing pst = new PrinterSettingsUsing();

                PrintSystem.PageSettings.AssignDefaultPrinterSettings(pst);
                PrintableComponentLink link = new PrintableComponentLink(PrintSystem);

                link.Component = DgvPivot;

                if (CmbPrintOrientation.EditValue.ToString() == "Landscape")
                {
                    link.Landscape = true;
                }
                else
                {
                    link.Landscape = false;
                }


                link.Margins.Left = 25;
                link.Margins.Right = 25;
                link.Margins.Bottom = 50;

                int IntTop = 0;
                if (Val.ToBoolean(DRowReportMast["ISPRINTFIRMNAME"]) == true)
                {
                    IntTop = IntTop + 20;
                }
                if (Val.ToBoolean(DRowReportMast["ISPRINTFIRMADDRESS"]) == true)
                {
                    IntTop = IntTop + 40;
                }
                IntTop = IntTop + 40; // Report Name
                IntTop = IntTop + 40; // Fixed Date Filter

                if (Val.ToBoolean(DRowReportMast["ISPRINTFILTERCRITERIA"]) == true)
                {
                    IntTop = IntTop + 80;
                }

                link.Margins.Top = IntTop;
                link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);
                link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);

                link.CreateDocument();
                link.ShowPreview(DevExpress.LookAndFeel.UserLookAndFeel.Default);

                //var frm = new StimulsoftViewer.FrmRepReportViewer(this);
                //frm.documentViewer1.DocumentSource = link;
                //frm.MdiParent = MdiParent;
                //frm.Show();
            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
            }
        }
        public void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            int IntHeight = 0;
            if (Val.ToBoolean(DRowReportMast["ISPRINTFIRMNAME"]) == true)
            {
                TextBrick BrickCompany = e.Graph.DrawString(BusLib.Configuration.BOConfiguration.gEmployeeProperty.COMPANYNAME, System.Drawing.Color.Black, new RectangleF(0, 0, e.Graph.ClientPageSize.Width, 20), DevExpress.XtraPrinting.BorderSide.None);
                BrickCompany.Font = new Font("Times New Roman", 12, FontStyle.Bold);
                BrickCompany.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
                BrickCompany.VertAlignment = DevExpress.Utils.VertAlignment.Center;
                BrickCompany.ForeColor = Color.FromArgb(27, 66, 105);
            }

            IntHeight = IntHeight + 20;
            TextBrick BrickTitle = e.Graph.DrawString(lblReportHeader.Text, System.Drawing.Color.Navy, new RectangleF(0, IntHeight, e.Graph.ClientPageSize.Width, 20), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitle.Font = new Font("Times New Roman", 9, FontStyle.Bold);
            BrickTitle.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
            BrickTitle.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            BrickTitle.ForeColor = Color.FromArgb(27, 66, 105);

            IntHeight = IntHeight + 20;

            string DateFilter = "";
            if (mStrFromDate != "" && mStrToDate != "")
            {
                DateFilter = "FROM : " + mStrFromDate + " TO " + mStrToDate;
            }
            if (mStrASOnDate != "")
            {
                DateFilter = DateFilter + " AND ASONDATE : " + mStrASOnDate;
            }

            TextBrick BrickDateFilter = e.Graph.DrawString(DateFilter, System.Drawing.Color.Navy, new RectangleF(0, IntHeight, e.Graph.ClientPageSize.Width, 20), DevExpress.XtraPrinting.BorderSide.None);
            BrickDateFilter.Font = new Font("Times New Roman", 7, FontStyle.Regular);
            BrickDateFilter.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
            BrickDateFilter.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            BrickDateFilter.ForeColor = Color.Black;

            if (Val.ToBoolean(DRowReportMast["ISPRINTFILTERCRITERIA"]) == true)
            {
                IntHeight = IntHeight + 20;
                TextBrick BrickTitlesParam = e.Graph.DrawString(txtFilterBy.Text, System.Drawing.Color.Navy, new RectangleF(0, IntHeight, e.Graph.ClientPageSize.Width, 80), DevExpress.XtraPrinting.BorderSide.None);
                BrickTitlesParam.Font = new Font("Times New Roman", 9, FontStyle.Regular);
                BrickTitlesParam.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
                BrickTitlesParam.VertAlignment = DevExpress.Utils.VertAlignment.Top;
                BrickTitlesParam.ForeColor = Color.Black;
            }
        }

        public void Link_CreateMarginalFooterArea(object sender, CreateAreaEventArgs e)
        {
            int IntX = Convert.ToInt32(Math.Round(e.Graph.ClientPageSize.Width - 100, 0));

            PageInfoBrick BrickPageNo = e.Graph.DrawPageInfo(PageInfo.NumberOfTotal, "Page {0} of {1}", System.Drawing.Color.FromArgb(27, 66, 105), new RectangleF(IntX, 5, 100, 15), DevExpress.XtraPrinting.BorderSide.None);
            BrickPageNo.LineAlignment = BrickAlignment.Far;
            BrickPageNo.Alignment = BrickAlignment.Far;
            // BrickPageNo.AutoWidth = true;
            BrickPageNo.Font = new Font("Times New Roman", 7, FontStyle.Regular); ;
            BrickPageNo.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            BrickPageNo.VertAlignment = DevExpress.Utils.VertAlignment.Center;


            if (Val.ToBoolean(DRowReportMast["ISPRINTDATETIME"]) == true)
            {
                TextBrick BrickTitledate = e.Graph.DrawString("Print Date :- " + lblDateTime.Text, System.Drawing.Color.FromArgb(27, 66, 105), new RectangleF(0, 5, 300, 30), DevExpress.XtraPrinting.BorderSide.None);
                BrickTitledate.Font = new Font("Times New Roman", 7, FontStyle.Regular);
                BrickTitledate.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
                BrickTitledate.VertAlignment = DevExpress.Utils.VertAlignment.Center;

            }
        }

        private void BtnColleps_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DgvPivot.CollapseAll();
        }

        private void BtnExpand_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DgvPivot.ExpandAll();
        }

        private void BtnAutoWidthColumn_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DgvPivot.BestFit();
        }

        private void BtnExportXlsx_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = ".xlsx";
                svDialog.Title = "Export";
                svDialog.FileName = "Report";

                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    Global.ExcelExport(svDialog.FileName, DgvPivot);
                    //DevExpress.XtraPrinting.PrintingSystem PrintSystem = new DevExpress.XtraPrinting.PrintingSystem();

                    //PrinterSettingsUsing pst = new PrinterSettingsUsing();

                    //PrintSystem.PageSettings.AssignDefaultPrinterSettings(pst);
                    //PrintableComponentLink link = new PrintableComponentLink(PrintSystem);

                    //link.Component = MainGrid;
                    ////GrdDet.Bands["SEL"].Visible = false;
                    //if (CmbOrientation.EditValue.ToString() == "Landscape")
                    //{
                    //    link.Landscape = true;
                    //}
                    //else
                    //{
                    //    link.Landscape = false;
                    //}

                    //GrdDet.OptionsPrint.AutoWidth = Val.ToBoolean(BatBtnAutoFit.EditValue);

                    //GrdDet.OptionsPrint.ExpandAllGroups = true;

                    //GrdDet.OptionsView.ShowViewCaption = false;

                    //link.Margins.Left = 25;
                    //link.Margins.Right = 25;
                    //link.Margins.Bottom = 25;
                    //link.Margins.Top = 200;
                    //link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);
                    //link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);
                    //link.CreateDocument();
                    //link.ExportToXlsx(svDialog.FileName);
                    //GrdDet.OptionsView.ShowViewCaption = true;
                    ////GrdDet.Bands["SEL"].Visible = true;
                    //if (Global.Confirm("Do you want to Open this file ?") == System.Windows.Forms.DialogResult.Yes)
                    //{
                    //    System.Diagnostics.Process.Start(svDialog.FileName, "CMD");
                    //}
                }

            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
            }
        }




        private void BtnRefreshReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            BarBtnSummaryDetail.Caption = "Summary (F2)";
            mReportProperty.REPORTTYPE = "D";



            DgvPivot.DataSource = null;
            DgvPivot.Refresh();

            LoadData(DTabSummary);

            this.Cursor = Cursors.Default;
        }



        private void MNPrint_Click(object sender, EventArgs e)
        {

        }

        private void MNAutoFit_Click(object sender, EventArgs e)
        {

        }

        private void MNExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = ".xlsx";
                svDialog.Title = "Export";
                svDialog.FileName = "Report";

                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    Global.ExcelExport(svDialog.FileName, DgvPivot);
                }
            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
            }
        }

        private void MNExpandGroup_Click(object sender, EventArgs e)
        {

        }

        private void MNCollapseGroup_Click(object sender, EventArgs e)
        {

        }

        private void MNBestFit_Click(object sender, EventArgs e)
        {

        }

        private void MNRowFilter_Click(object sender, EventArgs e)
        {

        }

        private void MNRefresh_Click(object sender, EventArgs e)
        {

        }

        private void MNExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
