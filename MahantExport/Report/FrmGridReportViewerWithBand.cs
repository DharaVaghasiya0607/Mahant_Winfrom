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
using DevExpress.XtraCharts;
using System.Linq;


namespace MahantExport.Report
{
    public partial class FrmGridReportViewerWithBand : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
       
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

        double DouFoldMeter = 0.00;
        double DouTaxableAmount = 0;

        double DouColorCost = 0.00;
        double DouChemicalCost = 0.00;
        double DouTotalCost = 0.00;
        double DouFinishMeter = 0.00;

        double DouIssuePcs = 0;
        double DouIssueCts = 0.00;
        double DouNetPolishCts = 0.00;
        double DouDemandCts = 0.00;

        double DouGhatCts = 0.00; // Dhara : 11-08-2023
        double DouPolCts = 0.00;
        string mStrManualReportTitle = string.Empty;


        double DouReadyCts = 0.00;
        double DouRepairingLoss = 0.00;
        int mIntFilterHeight = 0;
        bool ISFilter = false;
        bool mBoolNoGroup = false;
        string mStrShiftType = "";

        double pDouPrdBalanceCarat = 0;
        double pDouOriginalCarat = 0;

        double pDouTotalPolCarat = 0;
        #region Constructor


        public FrmGridReportViewerWithBand()
        {
            InitializeComponent();
        }

        public void ShowForm(DataSet pDS, DataRow pDRowReportMast, DataTable pDTabFieldDetail, string pStrGroupByTag, string pStrGroupByText, string pStrFilterText, bool pBoolNoGroup, string pStrOrderByTag, string pStrOrderByText, MST_ReportProperty pReportProperty, 
            string pStrFromDate,
            string pStrToDate,
            string pStrAsOnDate,
            int pIntFilterHeight,
            string pStrShiftType
            )
        {
            mStrFromDate = pStrFromDate;
            mStrToDate = pStrToDate;
            mStrASOnDate = pStrAsOnDate;
            mIntFilterHeight = pIntFilterHeight;

            mReportProperty = pReportProperty;
            DRowReportMast = pDRowReportMast;
            DTabReportField = pDTabFieldDetail;
            DTabSummary = pDS.Tables[0];
            if (DTabSummary.Columns.Contains("REPORTTITLE"))
            {
                mStrManualReportTitle = Val.ToString(DTabSummary.Rows[0]["REPORTTITLE"]);
            }

            mStrGroupByTag = pStrGroupByTag;
            mStrGroupByText = pStrGroupByText;
            
            mBoolNoGroup = pBoolNoGroup;

            mStrOrderByTag = pStrOrderByTag;
            mStrOrderByText = pStrOrderByText;

            mStrFilter = pStrFilterText;
            mStrShiftType = pStrShiftType;

            Val.FormGeneralSetting(this);
            AttachFormEvents();
            MNPageOrentation.SelectedItem = Val.ToString(DRowReportMast["PrintOrientation"]);// "Portrait";

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

        #region Operation

        private void Export(string format, string dlgHeader, string dlgFilter)
        {
            GrdDet.OptionsPrint.ExpandAllDetails = true;
            DevExpress.XtraGrid.Export.GridViewExportLink gvlink;
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
                            GrdDet.ExportToPdf(Filepath);

                            break;
                        case "xls":
                            GrdDet.ExportToXls(Filepath);


                            //gvlink = (DevExpress.XtraGrid.Export.GridViewExportLink)GridView1.CreateExportLink(new DevExpress.XtraExport.ExportXlsProvider(Filepath));

                            //gvlink.ExportAll = true;

                            //gvlink.ExpandAll = true;

                            //gvlink.ExportDetails = true;

                            //gvlink.ExportTo(true);

                            break;
                        case "xlsx":
                            GrdDet.ExportToXlsx(Filepath);


                            //gvlink = (DevExpress.XtraGrid.Export.GridViewExportLink)GridView1.CreateExportLink(new DevExpress.XtraExport.ExportXlsxProvider(Filepath));

                            //gvlink.ExportAll = true;

                            //gvlink.ExpandAll = true;

                            //gvlink.ExportDetails = true;

                            //gvlink.ExportTo(true);

                            break;
                        case "rtf":
                            GrdDet.ExportToRtf(Filepath);
                            break;
                        case "txt":
                            //GrdDet.ExportToText(Filepath);
                            gvlink = (DevExpress.XtraGrid.Export.GridViewExportLink)GrdDet.CreateExportLink(new DevExpress.XtraExport.ExportTxtProvider(Filepath));

                            gvlink.ExportAll = true;

                            gvlink.ExpandAll = true;

                            gvlink.ExportDetails = true;

                            gvlink.ExportTo(true);
                            break;
                        case "html":
                            //GrdDet.ExportToHtml(Filepath);
                            gvlink = (DevExpress.XtraGrid.Export.GridViewExportLink)GrdDet.CreateExportLink(new DevExpress.XtraExport.ExportHtmlProvider(Filepath));

                            gvlink.ExportAll = true;

                            gvlink.ExpandAll = true;

                            gvlink.ExportDetails = true;

                            gvlink.ExportTo(true);
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message.ToString());
            }
        }

        public void LoadData(DataTable pDTab)
        {
            CmbXAxis.Items.Clear();
            CmbYAxis.Items.Clear();
            xtraTabControl2.SelectedTabPageIndex = 0;
            lblChart.Text = "Bar";

            lblReportHeader.Text = Val.ToString(DRowReportMast["REPORTNAME"]);
            this.Text = Val.ToString(DRowReportMast["REPORTNAME"]).ToUpper();
            lblGroupBy.Text = "Group By : " + mStrGroupByText;
            txtFilterBy.Text = mStrFilter;

            InsertReportTrace();

            int IntError = 0;

            try
            {
                GrdDet.BeginUpdate();
                DataView dv = new DataView(DTabReportField);
                dv.Sort = "SRNO";
                DTabReportField = dv.ToTable();

                int IntIndex = 0;
                foreach (DataRow DRow in DTabReportField.Rows)
                {
                    foreach (DataColumn DCol in pDTab.Columns)
                    {
                        // Arrancge  Column in Order by User
                        if (DCol.ColumnName == DRow["FIELDNAME"].ToString())
                        {
                            pDTab.Columns[DCol.ColumnName].SetOrdinal(IntIndex);
                            IntIndex++;
                            pDTab.AcceptChanges();
                            break;
                        }
                    }
                }

                for (int i = 0; i < pDTab.Columns.Count; i++)
                {
                    string test = pDTab.Columns[i].ColumnName;
                    DataRow[] DR = DTabReportField.Select("FIELDNAME='" + test + "'");
                    // Arrancge  Column in Order by User
                    if (DR == null || DR.Length == 0)
                    {
                        pDTab.Columns.Remove(pDTab.Columns[i]);
                        i = i - 1;
                    }
                }
                pDTab.AcceptChanges();

                for (int i = 0; i < pDTab.Columns.Count; i++)
                {
                    string test = pDTab.Columns[i].ColumnName;
                    CmbXAxis.Items.Add(test);
                    CmbYAxis.Items.Add(test);
                }
                pDTab.AcceptChanges();

                CmbXAxis.SelectedIndex = 0;
                CmbYAxis.SelectedIndex = 0;

                //Delete And Merge 
                foreach (DataRow DRow in DTabReportField.Rows)
                {
                    if (Val.ToBoolean(DRow["ISVISIBLE"]) == false)
                    {
                        //DTab.Columns.Remove(DRow["FIELD_NAME"].ToString());
                    }
                    else
                    {
                        if (DRow["MERGEON"].ToString() != "")
                        {
                            MergeOn = DRow["MERGEON"].ToString();

                            if (MergeOnStr == "")
                            {
                                MergeOnStr = DRow["MERGEON"].ToString();
                            }
                            else
                            {
                                MergeOnStr = MergeOnStr + "," + DRow["FIELDNAME"].ToString();
                            }
                        }
                    }
                }

                MergeOnStr = string.Empty;
                foreach (DataRow DRow in DTabReportField.Rows)
                {
                    if (Val.ToBoolean(DRow["ISMERGE"]) == true)
                    {
                        MergeOnStr = MergeOnStr + "," + DRow["FIELDNAME"].ToString();
                    }
                }

                List<GridBand> AL = new List<GridBand>();


                DataView view = new DataView(DTabReportField);
                DataTable distinctValues = view.ToTable(true, "BANDNAME");
                //GrdDet.Bands.Clear();
                GrdDet.Columns.Clear();
                GrdDet.GroupSummary.Clear();
                MainGrid.RepositoryItems.Clear();

                

                foreach (DataRow DRow in distinctValues.Rows)
                {
                    var gridBand = new GridBand();
                    gridBand.Caption = Val.ToString(DRow["BANDNAME"]);
                    gridBand.RowCount = 1;
                    AL.Add(gridBand);
                }


                if (pDTab.Columns.Contains("MERGESRNO") && MergeOnStr != "")
                {
                    foreach (DataRow dr in pDTab.Rows)
                    {
                        if (Val.ToInt(dr["MERGESRNO"]) != 1)
                        {
                            foreach (DataColumn DCol in pDTab.Columns)
                            {
                                if (MergeOnStr.Contains(DCol.ColumnName))
                                {
                                    dr[DCol] = DBNull.Value;
                                }
                            }
                        }
                    }
                }
           
                MainGrid.DataSource = pDTab;
                GrdDet.OptionsView.AllowCellMerge = true;

                foreach (DataRow DRow in DTabReportField.Rows)
                {
                    
                   
                    if (Val.ToBoolean(DRow["ISVISIBLE"]) == true && Val.ToString(DRow["UNBOUNDEXPRESSION"]) != "")
                    {
                        DevExpress.XtraGrid.Columns.GridColumn unbColumn = GrdDet.Columns.AddField(Val.ToString(DRow["FIELDNAME"]));
                        unbColumn.VisibleIndex = Val.ToInt(DRow["SRNO"]);
                        unbColumn.UnboundType = DevExpress.Data.UnboundColumnType.Decimal;
                        unbColumn.Caption = Val.ToString(DRow["CAPTION"]);
                        unbColumn.Tag = DRow["FIELDNAME"].ToString();
                        unbColumn.OptionsColumn.AllowEdit = false;
                        unbColumn.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        unbColumn.DisplayFormat.FormatString = Val.ToString(DRow["DISPLAYFORMAT"]);
                        unbColumn.UnboundExpression = Val.ToString(DRow["UNBOUNDEXPRESSION"]);
                        unbColumn.OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                    }
                    else
                    {

                        bool iBool = false;
                        foreach (DataColumn DCol in pDTab.Columns)
                        {
                            if (DCol.ColumnName == DRow["FIELDNAME"].ToString())
                            {
                                iBool = true;
                                break;
                            }

                        }

                        if (iBool == false)
                        {
                            continue;
                        }
                        if (Val.ToBoolean(DRow["ISVISIBLE"]) == false)
                        {
                            GrdDet.Columns[DRow["FIELDNAME"].ToString()].Visible = false;
                            continue;
                        }

                        if (Val.ToBoolean(DRow["ISMERGE"]) == false)
                        {
                            GrdDet.Columns[DRow["FIELDNAME"].ToString()].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                        }
                        else
                        {
                            GrdDet.Columns[DRow["FIELDNAME"].ToString()].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.True;
                        }
                        //Set Column Caption
                        GrdDet.Columns[DRow["FIELDNAME"].ToString()].Caption = DRow["CAPTION"].ToString();
                        GrdDet.Columns[DRow["FIELDNAME"].ToString()].Tag = DRow["FIELDNAME"].ToString();
                    }

                    GrdDet.Columns[DRow["FIELDNAME"].ToString()].OptionsColumn.AllowEdit = false;
                    if (Val.ToString(DRow["DISPLAYFORMAT"]).Contains("{0:N"))
                    {
                        GrdDet.Columns[DRow["FIELDNAME"].ToString()].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    }
                    GrdDet.Columns[DRow["FIELDNAME"].ToString()].DisplayFormat.FormatString = Val.ToString(DRow["DISPLAYFORMAT"]);
                    GrdDet.Columns[DRow["FIELDNAME"].ToString()].VisibleIndex = Val.ToInt(DRow["SRNO"]) + 1;
                    if (Val.ToInt(DRow["COLUMNWIDTH"]) ==0 )
                    {
                        GrdDet.Columns[DRow["FIELDNAME"].ToString()].Width = 150;
                    }
                    else
                    {
                        GrdDet.Columns[DRow["FIELDNAME"].ToString()].Width = Val.ToInt(DRow["COLUMNWIDTH"]);
                    }
                   
                    GrdDet.Columns[DRow["FIELDNAME"].ToString()].OptionsFilter.FilterPopupMode = FilterPopupMode.CheckedList;
                    if (Val.ToBoolean(DRow["ISBOLD"]) == true)
                    {
                        GrdDet.Columns[DRow["FIELDNAME"].ToString()].AppearanceCell.Font = new Font(GrdDet.Columns[DRow["FIELDNAME"].ToString()].AppearanceCell.Font, FontStyle.Bold);
                    }
                    if (Val.ToBoolean(DRow["ISITALIC"]) == true)
                    {
                        GrdDet.Columns[DRow["FIELDNAME"].ToString()].AppearanceCell.Font = new Font(GrdDet.Columns[DRow["FIELDNAME"].ToString()].AppearanceCell.Font, FontStyle.Italic);
                    }
                    if (Val.ToBoolean(DRow["ISUNDERLINE"]) == true)
                    {
                        GrdDet.Columns[DRow["FIELDNAME"].ToString()].AppearanceCell.Font = new Font(GrdDet.Columns[DRow["FIELDNAME"].ToString()].AppearanceCell.Font, FontStyle.Underline);
                    }

                    if (Val.ToString(DRow["DATATYPE"]).ToLower() == "date")
                    {
                        GrdDet.Columns[DRow["FIELDNAME"].ToString()].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        GrdDet.Columns[DRow["FIELDNAME"].ToString()].DisplayFormat.FormatString = "dd/MM/yy";
                    }
                    if (Val.ToString(DRow["DATATYPE"]).ToLower() == "time")
                    {
                        GrdDet.Columns[DRow["FIELDNAME"].ToString()].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        GrdDet.Columns[DRow["FIELDNAME"].ToString()].DisplayFormat.FormatString = "hh:mm:ss tt";
                    }
                    if (Val.ToString(DRow["DATATYPE"]).ToLower() == "datetime")
                    {
                        GrdDet.Columns[DRow["FIELDNAME"].ToString()].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        GrdDet.Columns[DRow["FIELDNAME"].ToString()].DisplayFormat.FormatString = "dd/MM/yyyy hh:mm:ss tt";
                    }

                    foreach (GridBand band in AL)
                    {
                        if (band.Caption == Val.ToString(DRow["BANDNAME"]))
                        {
                            GrdDet.Columns[DRow["FIELDNAME"].ToString()].OwnerBand = band;

                            bool ISExists = false;
                            foreach (GridBand band1 in GrdDet.Bands)
                            {
                                if (band1.Caption == band.Caption)
                                {
                                    ISExists = true;
                                    break;
                                }

                            }
                            if (ISExists == false)
                            {
                                GrdDet.Bands.Add(band);
                            }

                            break;
                        }
                    }

                    if (Val.ToBoolean(DRow["ISVISIBLE"]) == true && Val.ToBoolean(DRow["ISMERGE"]) == false)
                    {
                        
                        switch (DRow["SUMMARYTYPE"].ToString().ToUpper())
                        {
                            case "SUM":
                                GrdDet.Columns[DRow["FIELDNAME"].ToString()].Summary.Add(SummaryItemType.Sum, DRow["FIELDNAME"].ToString(), Val.ToString(DRow["DISPLAYFORMAT"].ToString()));
                                GrdDet.GroupSummary.Add(SummaryItemType.Sum, DRow["FIELDNAME"].ToString(), GrdDet.Columns[DRow["FIELDNAME"].ToString()], Val.ToString(DRow["DISPLAYFORMAT"].ToString()));
                                break;
                            case "AVG":
                                GrdDet.Columns[DRow["FIELDNAME"].ToString()].Summary.Add(SummaryItemType.Average, DRow["FIELDNAME"].ToString(), Val.ToString(DRow["DISPLAYFORMAT"].ToString()));
                                GrdDet.GroupSummary.Add(SummaryItemType.Average, DRow["FIELDNAME"].ToString(), GrdDet.Columns[DRow["FIELDNAME"].ToString()], Val.ToString(DRow["DISPLAYFORMAT"].ToString()));
                                break;
                            case "COUNT":
                                GrdDet.Columns[DRow["FIELDNAME"].ToString()].Summary.Add(SummaryItemType.Count, DRow["FIELDNAME"].ToString(), Val.ToString(DRow["DISPLAYFORMAT"].ToString()));
                                GrdDet.GroupSummary.Add(SummaryItemType.Count, DRow["FIELDNAME"].ToString(), GrdDet.Columns[DRow["FIELDNAME"].ToString()], Val.ToString(DRow["DISPLAYFORMAT"].ToString()));
                                break;
                            case "MAX":
                                GrdDet.Columns[DRow["FIELDNAME"].ToString()].Summary.Add(SummaryItemType.Max, DRow["FIELDNAME"].ToString(), Val.ToString(DRow["DISPLAYFORMAT"].ToString()));
                                GrdDet.GroupSummary.Add(SummaryItemType.Max, DRow["FIELDNAME"].ToString(), GrdDet.Columns[DRow["FIELDNAME"].ToString()], Val.ToString(DRow["DISPLAYFORMAT"].ToString()));
                                break;
                            case "MIN":
                                GrdDet.Columns[DRow["FIELDNAME"].ToString()].Summary.Add(SummaryItemType.Min, DRow["FIELDNAME"].ToString(), Val.ToString(DRow["DISPLAYFORMAT"].ToString()));
                                GrdDet.GroupSummary.Add(SummaryItemType.Min, DRow["FIELDNAME"].ToString(), GrdDet.Columns[DRow["FIELDNAME"].ToString()], Val.ToString(DRow["DISPLAYFORMAT"].ToString()));
                                break;
                            case "CUSTOME":
                                GrdDet.Columns[DRow["FIELDNAME"].ToString()].Summary.Add(SummaryItemType.Custom, DRow["FIELDNAME"].ToString(), Val.ToString(DRow["DISPLAYFORMAT"].ToString()));
                                GrdDet.GroupSummary.Add(SummaryItemType.Custom, DRow["FIELDNAME"].ToString(), GrdDet.Columns[DRow["FIELDNAME"].ToString()], Val.ToString(DRow["DISPLAYFORMAT"].ToString()));

                                break;
                            default:
                                break;
                        }
                    }

                    if (Val.ToString(DRow["ORDERTYPE"]) == "ASC")
                    {
                        GrdDet.Columns[DRow["FIELDNAME"].ToString()].SortOrder = ColumnSortOrder.Ascending;
                        GrdDet.Columns[DRow["FIELDNAME"].ToString()].SortMode = ColumnSortMode.Default;
                    }
                    else if (Val.ToString(DRow["ORDERTYPE"]) == "DESC")
                    {
                        GrdDet.Columns[DRow["FIELDNAME"].ToString()].SortOrder = ColumnSortOrder.Descending;
                        GrdDet.Columns[DRow["FIELDNAME"].ToString()].SortMode = ColumnSortMode.Default;
                    }
                    else if (Val.ToString(DRow["ORDERTYPE"]) == "CUSTOM")
                    {
                        GrdDet.Columns[DRow["FIELDNAME"].ToString()].SortMode = ColumnSortMode.Custom;
                        GrdDet.Columns[DRow["FIELDNAME"].ToString()].Tag = Val.ToString(DRow["ORDERCOLUMN"]);

                    }

                }


                for (int IntI = 0; IntI < GrdDet.Columns.Count; IntI++)
                {
                    if (GrdDet.Columns[IntI].FieldName.ToUpper().Contains("_ID"))
                    {
                        GrdDet.Columns[IntI].Visible = false;
                    }
                }

                //Group By Setting
                GrdDet.ClearSorting();

                string[] StrGroupBy = mStrGroupByTag.Split(',');

                int IntCount = 0;

                if (mReportProperty.REPORTTYPE == "S")
                {
                    IntCount = StrGroupBy.Length - 1;
                }
                else
                {
                    IntCount = StrGroupBy.Length;
                }


                if (mBoolNoGroup == false)
                {
                    for (int IntI = 0; IntI < IntCount; IntI++)
                    {
                        if (StrGroupBy[IntI] != "")
                        {
                            GrdDet.Columns[StrGroupBy[IntI]].GroupIndex = IntI;
                            GrdDet.Columns[StrGroupBy[IntI]].Group();
                            GrdDet.Columns[StrGroupBy[IntI]].OwnerBand = null;
                        }
                    }    
                }
                

                //if (mStrGroupByTag != "")
                //{
                //    foreach (string Str in Val.ToString(mStrGroupByTag).Split(','))
                //    {
                //        if (Str != "")
                //        {
                //            GrdDet.Columns[Str].SortMode = DevExpress.XtraGrid.ColumnSortMode.Default;
                //            GrdDet.Columns[Str].SortOrder = ColumnSortOrder.Ascending;
                //        }
                //    }
                //}

                GrdDet.Appearance.Row.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Regular);
                GrdDet.AppearancePrint.Row.Font = new Font(Val.ToString(DRowReportMast["PRINTFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["PRINTFONTSIZE"])), FontStyle.Regular);

                GrdDet.Appearance.OddRow.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Regular);
                GrdDet.AppearancePrint.OddRow.Font = new Font(Val.ToString(DRowReportMast["PRINTFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["PRINTFONTSIZE"])), FontStyle.Regular);

                GrdDet.Appearance.EvenRow.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Regular);
                GrdDet.AppearancePrint.EvenRow.Font = new Font(Val.ToString(DRowReportMast["PRINTFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["PRINTFONTSIZE"])), FontStyle.Regular);

                GrdDet.Appearance.Row.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Bold);
                GrdDet.AppearancePrint.Row.Font = new Font(Val.ToString(DRowReportMast["PRINTFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["PRINTFONTSIZE"])), FontStyle.Bold);

                GrdDet.Appearance.HeaderPanel.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Bold);
                GrdDet.AppearancePrint.HeaderPanel.Font = new Font(Val.ToString(DRowReportMast["PRINTFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["PRINTFONTSIZE"])), FontStyle.Bold);

                GrdDet.Appearance.GroupRow.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])), FontStyle.Bold);
                GrdDet.AppearancePrint.GroupRow.Font = new Font(Val.ToString(DRowReportMast["PRINTFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["PRINTFONTSIZE"])), FontStyle.Bold);

                GrdDet.Appearance.GroupFooter.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])) - 1, FontStyle.Bold);
                GrdDet.AppearancePrint.GroupFooter.Font = new Font(Val.ToString(DRowReportMast["PRINTFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["PRINTFONTSIZE"])) - 1, FontStyle.Bold);

                GrdDet.Appearance.FooterPanel.Font = new Font(Val.ToString(DRowReportMast["DISPLAYFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["DISPLAYFONTSIZE"])) - 1, FontStyle.Bold);
                GrdDet.AppearancePrint.FooterPanel.Font = new Font(Val.ToString(DRowReportMast["PRINTFONTNAME"]), float.Parse(Val.ToString(DRowReportMast["PRINTFONTSIZE"])) - 1, FontStyle.Bold);

                GrdDet.OptionsPrint.UsePrintStyles = true;
                GrdDet.OptionsSelection.MultiSelect = true;
                GrdDet.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;

                //if (MainGrid.RepositoryItems.Count == 0)
                //{
                //    ObjGridSelection = new BODevGridSelection();
                //    ObjGridSelection.View = GrdDet;
                //    ObjGridSelection.ClearSelection();
                //    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;

                //}
                //else
                //{
                //    ObjGridSelection.ClearSelection();
                //}
                //GridBand bandSel = new GridBand();
                //bandSel.Name = "SEL";
                //bandSel.Caption = "Sel";
                //bandSel.VisibleIndex = 0;
                //bandSel.Fixed = FixedStyle.Left;
                //GrdDet.Bands.Add(bandSel);

                //GrdDet.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;
                //GrdDet.Columns["COLSELECTCHECKBOX"].OptionsColumn.AllowMerge = DevExpress.Utils.DefaultBoolean.False;
                ////GrdDet.Columns["COLSELECTCHECKBOX"].OwnerBand = bandSel;
                //if (ObjGridSelection != null)
                //{
                //    ObjGridSelection.ClearSelection();
                //    ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                //    GrdDet.Columns["COLSELECTCHECKBOX"].VisibleIndex = 0;
                //}

               
                GrdDet.ExpandAllGroups();
                GrdDet.BestFitColumns();
                GrdDet.EndUpdate();

                GrdDet.RefreshData();
            }
            catch (Exception Ex)
            {
                GrdDet.EndUpdate();
            
                Global.Message("Error In Column Index : " + IntError.ToString() + "    " + Ex.Message);
            }

        }

        private void SetGridBand(BandedGridView bandedView, string gridBandCaption, string[] columnNames)
        {
            var gridBand = new GridBand();
            gridBand.Caption = gridBandCaption;
            int nrOfColumns = columnNames.Length;
            BandedGridColumn[] bandedColumns = new BandedGridColumn[nrOfColumns];
            for (int i = 0; i < nrOfColumns; i++)
            {
                bandedColumns[i] = (BandedGridColumn)bandedView.Columns.AddField(columnNames[i]);
                bandedColumns[i].OwnerBand = gridBand;
                bandedColumns[i].Visible = true;
            }
            bandedView.Bands.Add(gridBand);
        }


        public void InsertReportTrace()
        {
            string MM = Val.ToString(DateTime.Today.Month);
            if (Val.ToInt(MM) < 10)
            {
                MM = "0" + MM;
            }
            int YYMM = Val.ToInt(Val.ToString(DateTime.Today.Year) + MM);
            //int SRNO = ObjReport.FindNewSrNo(YYMM);

            //   ObjReport.SaveReportTrace(YYMM, 0, Report_Code, Report_Type);
        }

        public double GenerateTimeFieldSummry(GridView view, string Field)
        {
            if (view == null) return 0;

            if (Val.ToString(Field) == "") return 0;

            GridColumn TimetCol = view.Columns[Field];

            if (TimetCol == null) return 0;

            try
            {
                double totalWeight = 0;

                for (int i = 0; i < view.DataRowCount; i++)
                {
                    if (view.IsNewItemRow(i)) continue;

                    object temp;

                    double weight;

                    if (view.IsGroupRow(i))
                    {
                        temp = view.GetRowCellValue(i, TimetCol);
                    }
                    else
                    {
                        temp = view.GetRowCellValue(i, TimetCol);
                    }

                    temp = view.GetRowCellValue(i, TimetCol);

                    weight = (temp == DBNull.Value || temp == null) ? 0 : Val.Val(temp);

                    totalWeight += weight;

                }

                if (totalWeight == 0) return 0;

                string[] parts = totalWeight.ToString().Split('.');
                int i1 = Val.ToInt(parts[0]);
                int i2 = Val.ToInt(parts[1]);

                while (i2 > 60)
                {
                    i1 = i1 + 1;
                    i2 = i2 - 60;
                }

                return Val.Val(i1.ToString() + "." + i2.ToString());

            }
            catch
            {
                return 0;
            }
        }

        public double GetWeightedAverage(GridView view, string weightField, string valueField)
        {
            if (view == null) return 0;

            if (Val.ToString(weightField) == "" || Val.ToString(valueField) == "") return 0;

            GridColumn weightCol = view.Columns[weightField];

            GridColumn valueCol = view.Columns[valueField];

            if (weightCol == null || valueCol == null) return 0;

            try
            {
                double totalWeight = 0, totalValue = 0;

                for (int i = 0; i < view.DataRowCount; i++)
                {

                    if (view.IsNewItemRow(i)) continue;

                    object temp;

                    double weight, val;

                    temp = view.GetRowCellValue(i, weightCol);

                    weight = (temp == DBNull.Value || temp == null) ? 0 : Val.Val(temp);

                    temp = view.GetRowCellValue(i, valueCol);

                    val = (temp == DBNull.Value || temp == null) ? 0 : Val.Val(temp);

                    totalWeight += weight;

                    totalValue += weight * val;

                }

                if (totalWeight == 0) return 0;

                return Val.Val(totalValue / totalWeight);

            }
            catch
            {
                return 0;
            }
        }


        #endregion


        #region Form Event

        private void FrmGReportViewerBand1_Load(object sender, EventArgs e)
        {
            int IntIndex = 0;
            int IntSelectedIndex = 0;
            //ComboPageKind.Items.Clear();
            //foreach (System.Drawing.Printing.PaperKind foo in Enum.GetValues(typeof(System.Drawing.Printing.PaperKind)))
            //{
            //    ComboPageKind.Items.Add(foo.ToString());
            //    if (foo.ToString() == "A4")
            //    {
            //        //ComboPageKind.text = IntIndex;
            //        IntSelectedIndex = IntIndex;
            //    }
            //    IntIndex++;
            //}
            //ComboPageKind.SelectedIndex = IntSelectedIndex;
            lblDateTime.Text = DateTime.Now.ToString("dd/MMM/yyyy hh:mm:ss tt");
            LoadData(DTabSummary);
        }

        #endregion

        #region Grid Event

        private void GridView1_StartGrouping(object sender, EventArgs e)
        {
            GrdDet.BestFitColumns();
        }

        private void GridView1_RowStyle(object sender, RowStyleEventArgs e)
        {
            //if (e.RowHandle >= 0)
            //{
            //    DataRow DRow = GrdDet.GetDataRow(e.RowHandle);
            //}
        }

        #endregion

        private void BtnColleps_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GrdDet.CollapseAllGroups();
        }

        private void BtnExpand_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            GrdDet.ExpandAllGroups();
        }

       

   
        public void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            //A.,
            //A.ISPRINTFIRMADDRESS,
            //A.ISPRINTFILTERCRITERIA,
            //A.ISPRINTHEADINGONEACHPAGE,
            //A.ISPRINTDATETIME
            // ' For Report Title


            if (mReportProperty.REMARK == " FACTORY POLISH REPORT")
            {
                int IntHeight = 0;
                TextBrick BrickCompany = e.Graph.DrawString("RIJIYA GEMS ESTIMATE", System.Drawing.Color.Black, new RectangleF(0, 0, e.Graph.ClientPageSize.Width, 20), DevExpress.XtraPrinting.BorderSide.None);
                BrickCompany.Font = new Font("Verdana", 12, FontStyle.Bold);
                BrickCompany.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
                BrickCompany.VertAlignment = DevExpress.Utils.VertAlignment.Center;
                BrickCompany.ForeColor = Color.FromArgb(27, 66, 105);

                IntHeight = IntHeight + 20;
                TextBrick BrickTitle = e.Graph.DrawString("Group Wise Party Wise Receive Summary On Date : "+DateTime.Now.ToString("dd/MM/yyyy") + " On Time : "+DateTime.Now.ToString("HH:mm") , System.Drawing.Color.Navy, new RectangleF(0, IntHeight, e.Graph.ClientPageSize.Width, 20), DevExpress.XtraPrinting.BorderSide.None);
                BrickTitle.Font = new Font("Verdana", 9, FontStyle.Bold);
                BrickTitle.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
                BrickTitle.VertAlignment = DevExpress.Utils.VertAlignment.Center;
                BrickTitle.ForeColor = Color.FromArgb(27, 66, 105);

                IntHeight = IntHeight + 20;
                TextBrick BrickTitlesParam = e.Graph.DrawString(mStrManualReportTitle, System.Drawing.Color.Navy, new RectangleF(0, IntHeight, e.Graph.ClientPageSize.Width, mIntFilterHeight), DevExpress.XtraPrinting.BorderSide.None);
                BrickTitlesParam.Font = new Font("Verdana", 8, FontStyle.Regular);
                BrickTitlesParam.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
                BrickTitlesParam.VertAlignment = DevExpress.Utils.VertAlignment.Top;
                BrickTitlesParam.ForeColor = Color.Black;

                IntHeight = IntHeight + 20;
                TextBrick BrickTitlesTitle = e.Graph.DrawString("Final Polish Report", System.Drawing.Color.Navy, new RectangleF(0, IntHeight, e.Graph.ClientPageSize.Width, mIntFilterHeight), DevExpress.XtraPrinting.BorderSide.None);
                BrickTitlesTitle.Font = new Font("Verdana", 8, FontStyle.Regular);
                BrickTitlesTitle.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
                BrickTitlesTitle.VertAlignment = DevExpress.Utils.VertAlignment.Top;
                BrickTitlesTitle.ForeColor = Color.Black;
            }
            if (mReportProperty.REMARK == "FACTORY 4P REPORT")
            {
                int IntHeight = 0;
                TextBrick BrickCompany = e.Graph.DrawString("RIJIYA GEMS ESTIMATE", System.Drawing.Color.Black, new RectangleF(0, 0, e.Graph.ClientPageSize.Width, 20), DevExpress.XtraPrinting.BorderSide.None);
                BrickCompany.Font = new Font("Verdana", 12, FontStyle.Bold);
                BrickCompany.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
                BrickCompany.VertAlignment = DevExpress.Utils.VertAlignment.Center;
                BrickCompany.ForeColor = Color.FromArgb(27, 66, 105);

                IntHeight = IntHeight + 20;
                TextBrick BrickTitle = e.Graph.DrawString("Group Wise Party Wise Receive Summary On Date : "+DateTime.Now.ToString("dd/MM/yyyy") + " On Time : "+DateTime.Now.ToString("HH:mm") , System.Drawing.Color.Navy, new RectangleF(0, IntHeight, e.Graph.ClientPageSize.Width, 20), DevExpress.XtraPrinting.BorderSide.None);
                BrickTitle.Font = new Font("Verdana", 9, FontStyle.Bold);
                BrickTitle.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
                BrickTitle.VertAlignment = DevExpress.Utils.VertAlignment.Center;
                BrickTitle.ForeColor = Color.FromArgb(27, 66, 105);

                IntHeight = IntHeight + 20;
                TextBrick BrickTitlesParam = e.Graph.DrawString(mStrManualReportTitle, System.Drawing.Color.Navy, new RectangleF(0, IntHeight, e.Graph.ClientPageSize.Width, mIntFilterHeight), DevExpress.XtraPrinting.BorderSide.None);
                BrickTitlesParam.Font = new Font("Verdana", 8, FontStyle.Regular);
                BrickTitlesParam.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
                BrickTitlesParam.VertAlignment = DevExpress.Utils.VertAlignment.Top;
                BrickTitlesParam.ForeColor = Color.Black;

                IntHeight = IntHeight + 20;
                TextBrick BrickTitlesTitle = e.Graph.DrawString("Final Polish Report 4P", System.Drawing.Color.Navy, new RectangleF(0, IntHeight, e.Graph.ClientPageSize.Width, mIntFilterHeight), DevExpress.XtraPrinting.BorderSide.None);
                BrickTitlesTitle.Font = new Font("Verdana", 8, FontStyle.Regular);
                BrickTitlesTitle.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
                BrickTitlesTitle.VertAlignment = DevExpress.Utils.VertAlignment.Top;
                BrickTitlesTitle.ForeColor = Color.Black;
            }
            else
            {
                int IntHeight = 0;
                TextBrick BrickCompany = e.Graph.DrawString(BusLib.Configuration.BOConfiguration.gEmployeeProperty.COMPANYNAME, System.Drawing.Color.Black, new RectangleF(0, 0, e.Graph.ClientPageSize.Width, 20), DevExpress.XtraPrinting.BorderSide.None);
                BrickCompany.Font = new Font("Verdana", 12, FontStyle.Bold);
                BrickCompany.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
                BrickCompany.VertAlignment = DevExpress.Utils.VertAlignment.Center;
                BrickCompany.ForeColor = Color.FromArgb(27, 66, 105);
               
                IntHeight = IntHeight + 20;
                TextBrick BrickTitle = e.Graph.DrawString(lblReportHeader.Text, System.Drawing.Color.Navy, new RectangleF(0, IntHeight, e.Graph.ClientPageSize.Width, 20), DevExpress.XtraPrinting.BorderSide.None);
                BrickTitle.Font = new Font("Verdana", 9, FontStyle.Bold);
                BrickTitle.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
                BrickTitle.VertAlignment = DevExpress.Utils.VertAlignment.Center;
                BrickTitle.ForeColor = Color.FromArgb(27, 66, 105);

                IntHeight = IntHeight + 20;
                TextBrick BrickTitlesParam = e.Graph.DrawString(txtFilterBy.Text, System.Drawing.Color.Navy, new RectangleF(0, IntHeight, e.Graph.ClientPageSize.Width, mIntFilterHeight), DevExpress.XtraPrinting.BorderSide.None);
                BrickTitlesParam.Font = new Font("Verdana", 8, FontStyle.Regular);
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
            BrickPageNo.Font = new Font("Verdana", 8, FontStyle.Regular); ;
            BrickPageNo.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            BrickPageNo.VertAlignment = DevExpress.Utils.VertAlignment.Center;

            TextBrick BrickTitledate = e.Graph.DrawString("Print Date :- " + lblDateTime.Text, System.Drawing.Color.FromArgb(27, 66, 105), new RectangleF(0, 5, 300, 30), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitledate.Font = new Font("Verdana", 8, FontStyle.Regular);
            BrickTitledate.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
            BrickTitledate.VertAlignment = DevExpress.Utils.VertAlignment.Center;

            //if (Val.ToBoolean(DRowReportMast["ISPRINTDATETIME"]) == true)
            //{
            //    TextBrick BrickTitledate = e.Graph.DrawString("Print Date :- " + lblDateTime.Text, System.Drawing.Color.FromArgb(27, 66, 105), new RectangleF(0, 5, 300, 30), DevExpress.XtraPrinting.BorderSide.None);
            //    BrickTitledate.Font = new Font("Verdana", 7, FontStyle.Regular);
            //    BrickTitledate.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
            //    BrickTitledate.VertAlignment = DevExpress.Utils.VertAlignment.Center;

            //}
        }


        private void BtnExit_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Close();
        }

        private void GridView1_CellMerge(object sender, CellMergeEventArgs e)
        {
            if (MergeOnStr.Contains(e.Column.FieldName))
            {
                string val1 = Val.ToString(GrdDet.GetRowCellValue(e.RowHandle1, GrdDet.Columns[MergeOn]));
                string val2 = Val.ToString(GrdDet.GetRowCellValue(e.RowHandle2, GrdDet.Columns[MergeOn]));
                if (val1 == val2)
                    e.Merge = true;
                e.Handled = true;
            }
        }

        private void GrdDet_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000" || e.DisplayText == "0.0000")
            {
                e.DisplayText = String.Empty;
            }

        }

        private void GrdDet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                if (mReportProperty.REPORTTYPE == "S")
                {
                    GrdDet.ViewCaption = "Main View";
                    LoadData(DTabSummary);
                }
            }

            else if (e.KeyCode == Keys.S && e.Control && BoolSelection == false)
            {
                BoolSelection = true;
                GrdDet.Columns["COLSELECTCHECKBOX"].ClearFilter();
                GrdDet.Columns["COLSELECTCHECKBOX"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo("COLSELECTCHECKBOX=True");
            }
            else if (e.KeyCode == Keys.S && e.Control && BoolSelection == true)
            {
                BoolSelection = false;
                GrdDet.Columns["COLSELECTCHECKBOX"].ClearFilter();
            }
        }

        private void GrdDet_RowCellClick(object sender, RowCellClickEventArgs e)
        {
           
        }


        private void FrmGridReportViewerWithBand_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.P && e.Control)
            {
                MNPrint_Click(null, null);
            }
            if (e.KeyCode == Keys.F5)
            {
               BtnRefreshReport_ItemClick(null, null);
            }
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void BarBtnSummaryDetail_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

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
                    Global.ExcelExport(svDialog.FileName, GrdDet);
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


        private void GrdDet_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            try
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    DouIssuePcs = 0;
                    DouIssueCts = 0.00;
                    DouNetPolishCts = 0.00;
                    DouDemandCts = 0.00;
                    DouGhatCts = 0.00;
                    DouReadyCts = 0.00;
                    DouRepairingLoss = 0.00;
                    DouPolCts = 0.00;
                    pDouPrdBalanceCarat = 0.00;
                    pDouOriginalCarat = 0.00;
                    pDouTotalPolCarat = 0.00;
                }
                else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DouIssuePcs = DouIssuePcs + Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "ISSUEPCS"));
                    DouIssueCts = DouIssueCts + Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "ISSUECARAT"));
                    DouNetPolishCts = DouNetPolishCts + Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "NETPOLISHCARAT"));
                    DouDemandCts = DouDemandCts + Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "DEMANDCARAT"));
                    DouGhatCts = DouGhatCts + Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "GHATCARAT"));
                    DouReadyCts = DouReadyCts + Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "READYCARAT"));
                    DouRepairingLoss = DouRepairingLoss + Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "REPLOSS"));
                    DouPolCts = DouPolCts + Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "POLISHCARAT"));
                    pDouOriginalCarat = pDouOriginalCarat + Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "ORIGINALWT"));
                    pDouPrdBalanceCarat = pDouPrdBalanceCarat + Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "PRDBALANCECARAT"));
                    pDouTotalPolCarat = pDouTotalPolCarat + Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "TOTALPOLCARAT"));

                }
                else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    if (((GridSummaryItem)e.Item).FieldName.CompareTo("ISSUESIZE") == 0)
                    {
                        e.TotalValue = DouIssueCts == 0 ? 0 : Math.Round((DouIssuePcs / DouIssueCts), 2);
                    }
                    if (((GridSummaryItem)e.Item).FieldName.CompareTo("NETPOLISHCARAT") == 0)
                    {
                        e.TotalValue = Math.Round((DouReadyCts - DouRepairingLoss), 3);
                    }
                    if (((GridSummaryItem)e.Item).FieldName.CompareTo("RECPER") == 0)
                    {
                        e.TotalValue = DouIssueCts == 0 ? 0 : Math.Round((DouNetPolishCts / DouIssueCts) * 100, 2);
                    }
                    if (((GridSummaryItem)e.Item).FieldName.CompareTo("DEMANDPER") == 0)
                    {
                        e.TotalValue = DouIssueCts == 0 ? 0 : Math.Round((DouDemandCts / DouIssueCts) * 100, 2);
                    }
                    if (((GridSummaryItem)e.Item).FieldName.CompareTo("DIFFPER") == 0)
                    {
                        e.TotalValue = DouIssueCts == 0 ? 0 : Math.Round(((DouNetPolishCts / DouIssueCts) * 100) - ((DouDemandCts / DouIssueCts) * 100), 2);
                    }
                    if (((GridSummaryItem)e.Item).FieldName.CompareTo("GHATPER") == 0)
                    {
                        
                        e.TotalValue = DouIssueCts == 0 ? 0 : Math.Round((DouGhatCts / DouIssueCts) * 100, 2);
                    }
                    if (((GridSummaryItem)e.Item).FieldName.CompareTo("GHATPOLISHPER") == 0)
                    {
                        e.TotalValue = DouGhatCts == 0 ? 0 : Math.Round(((DouReadyCts - DouRepairingLoss) / DouGhatCts) * 100, 2);
                    }
                    if (((GridSummaryItem)e.Item).FieldName.CompareTo("KAPANGHATWITHPOLPER") == 0)
                    {
                        e.TotalValue = DouGhatCts == 0 ? 0 : Math.Round(((DouReadyCts - DouRepairingLoss) / DouGhatCts) * 100, 2);
                    }
                    if (((GridSummaryItem)e.Item).FieldName.CompareTo("POLPER") == 0)
                    {
                        e.TotalValue = DouIssueCts == 0 ? 0 : Math.Round(((DouPolCts) / DouIssueCts) * 100, 2);
                    }
                    if (((GridSummaryItem)e.Item).FieldName.CompareTo("GHATWITHPOLPER") == 0)
                    {
                        e.TotalValue = DouGhatCts == 0 ? 0 : Math.Round(((DouPolCts) / DouGhatCts) * 100, 3);
                    }
                    if (((GridSummaryItem)e.Item).FieldName.CompareTo("READYCARAT") == 0)
                    {
                        e.TotalValue = DouReadyCts;
                    }
                    if (((GridSummaryItem)e.Item).FieldName.CompareTo("PRDBALANCEPER") == 0)
                    {
                        e.TotalValue = pDouOriginalCarat == 0 ? 0 : Math.Round(((pDouPrdBalanceCarat) / pDouOriginalCarat) * 100, 3);
                    }
                    if (((GridSummaryItem)e.Item).FieldName.CompareTo("ORIGINALPER") == 0)
                    {
                        e.TotalValue = pDouOriginalCarat == 0 ? 0 : Math.Round(((DouDemandCts) / pDouOriginalCarat) * 100, 3);
                    }
                    if (((GridSummaryItem)e.Item).FieldName.CompareTo("TOTALPOLCARATPER") == 0)
                    {
                        e.TotalValue = pDouOriginalCarat == 0 ? 0 : Math.Round(((pDouTotalPolCarat) / pDouOriginalCarat) * 100, 3);
                    }
                    if (((GridSummaryItem)e.Item).FieldName.CompareTo("DIFFCAT") == 0)
                    {
                        e.TotalValue = DouNetPolishCts - DouDemandCts;
                    }
                    

                }

                if (mReportProperty.REMARK == "DISPATCH")
                {
                    if (e.SummaryProcess == CustomSummaryProcess.Start)
                    {
                        DouTaxableAmount = 0;
                        DouFoldMeter = 0;
                        DouChemicalCost = 0;
                        DouColorCost = 0;
                        DouTotalCost = 0;
                        DouFinishMeter = 0;
                    }
                    else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                    {
                        DouTaxableAmount = DouTaxableAmount + Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "TAXABLEAMOUNT"));
                        DouFoldMeter = DouFoldMeter + Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "GPFOLDMETER"));
                        DouColorCost = DouColorCost + Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "COLORCOST"));
                        DouTotalCost = DouTotalCost + Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "TOTALCOST"));
                        DouChemicalCost = DouChemicalCost + Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "CHEMICALCOST"));
                        DouFinishMeter = DouFinishMeter + Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "GPFINISHMETER"));
                    }
                    else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                    {
                        if (((GridSummaryItem)e.Item).FieldName.CompareTo("TAXAVGRATE") == 0)
                        {
                            e.TotalValue = DouFoldMeter == 0 ? 0 : Math.Round((DouTaxableAmount / DouFoldMeter), 3);
                        }
                        if (((GridSummaryItem)e.Item).FieldName.CompareTo("COLORCST") == 0)
                        {
                            e.TotalValue = DouFinishMeter == 0 ? 0 : Math.Round((DouColorCost / DouFinishMeter), 2);
                        }
                        if (((GridSummaryItem)e.Item).FieldName.CompareTo("CHEMICALCST") == 0)
                        {
                            e.TotalValue = DouFinishMeter == 0 ? 0 : Math.Round((DouChemicalCost / DouFinishMeter), 3);
                        }
                        if (((GridSummaryItem)e.Item).FieldName.CompareTo("TOTALCST") == 0)
                        {
                            e.TotalValue = DouFinishMeter == 0 ? 0 : Math.Round((DouTotalCost / DouFinishMeter), 3);
                        }
                        if (((GridSummaryItem)e.Item).FieldName.CompareTo("COLORPER") == 0)
                        {
                            e.TotalValue = DouTaxableAmount == 0 ? 0 : Math.Round((DouColorCost / DouTaxableAmount) * 100, 3);
                        }
                        if (((GridSummaryItem)e.Item).FieldName.CompareTo("CHEMICALPER") == 0)
                        {
                            e.TotalValue = DouTaxableAmount == 0 ? 0 : Math.Round((DouChemicalCost / DouTaxableAmount) * 100, 3);
                        }
                        if (((GridSummaryItem)e.Item).FieldName.CompareTo("TOTALPER") == 0)
                        {
                            e.TotalValue = DouTaxableAmount == 0 ? 0 : Math.Round((DouTotalCost / DouTaxableAmount) * 100, 3);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnRefreshReport_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
           
        }

        private void MNPrint_Click(object sender, EventArgs e)
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

                link.Component = MainGrid;

                if (Val.ToString(MNPageOrentation.SelectedItem)  == "Landscape")
                {
                    link.Landscape = true;
                }
                else
                {
                    link.Landscape = false;
                }

                GrdDet.OptionsPrint.AutoWidth = true;

                GrdDet.OptionsPrint.ExpandAllGroups = true;


                link.Margins.Left = 25;
                link.Margins.Right = 25;
                link.Margins.Bottom = 50;

                int IntTop = 0;
                IntTop = IntTop + 20;
                IntTop = IntTop + 20;

                IntTop = IntTop + 40; // Report Name

                IntTop = IntTop + mIntFilterHeight; 
                
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

        private void MNAutoFit_Click(object sender, EventArgs e)
        {

        }

        private void MNExport_Click(object sender, EventArgs e)
        {
            //Export("xlsx", "Export to Excel", "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*");

                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = "xlsx";
                svDialog.Title =  "Export to Excel";
                svDialog.FileName = "Report";
                svDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    string Filepath = svDialog.FileName;
                    try
                    {
                       
                        DevExpress.XtraPrinting.PrintingSystem PrintSystem = new DevExpress.XtraPrinting.PrintingSystem();

                        PrinterSettingsUsing pst = new PrinterSettingsUsing();

                        PrintSystem.PageSettings.AssignDefaultPrinterSettings(pst);
                        PrintableComponentLink link = new PrintableComponentLink(PrintSystem);

                        link.Component = MainGrid;

                        if (Val.ToString(MNPageOrentation.SelectedItem) == "Landscape")
                        {
                            link.Landscape = true;
                        }
                        else
                        {
                            link.Landscape = false;
                        }

                        GrdDet.OptionsPrint.AutoWidth = true;

                        GrdDet.OptionsPrint.ExpandAllGroups = true;


                        link.Margins.Left = 25;
                        link.Margins.Right = 25;
                        link.Margins.Bottom = 50;

                        int IntTop = 0;
                        IntTop = IntTop + 20;
                        IntTop = IntTop + 20;

                        IntTop = IntTop + 40; 

                        IntTop = IntTop + mIntFilterHeight;

                        link.Margins.Top = IntTop;
                        link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);
                        link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);

                        link.CreateDocument();
                        link.ExportToXlsx(Filepath);

                        System.Diagnostics.Process.Start(Filepath, "CMD");
                    }
                    catch (Exception EX)
                    {
                        Global.Message(EX.Message);
                    }
                }
            
        }

        private void MNExpandGroup_Click(object sender, EventArgs e)
        {
            GrdDet.ExpandAllGroups();
        }

        private void MNCollapseGroup_Click(object sender, EventArgs e)
        {
            GrdDet.CollapseAllGroups();
        }

        private void MNBestFit_Click(object sender, EventArgs e)
        {
            GrdDet.BestFitColumns();
        }

        private void MNRowFilter_Click(object sender, EventArgs e)
        {
            GrdDet.BeginUpdate();
            if (ISFilter == true)
            {
                GrdDet.OptionsView.ShowAutoFilterRow = false;
            }
            else
            {
                ISFilter = true;
               GrdDet.OptionsView.ShowAutoFilterRow = true;
            }
            GrdDet.EndUpdate();
        }

        private void MNRefresh_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            DataSet DS = new BOMST_Report().GenerateMaintainanceReport(mReportProperty);

            DTabSummary = DS.Tables[0];
           
            MainGrid.DataSource = null;
            MainGrid.Refresh();

            LoadData(DTabSummary);

            this.Cursor = Cursors.Default;
        }

        private void MNExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void LoadAllChartType()
        {
            //List<ViewType> lst = new List<ViewType>();
            //for (int IntI = 0; IntI < Enum.GetNames(typeof(ViewType)).Length; IntI++)
            //{
            //    lst.Add(Enum.GetNames(typeof(ViewType)));
            //}
            //foreach (string name in Enum.GetNames(typeof(ViewType)).Length)
            //{
                
            //}
        }


        public void GenerateChart(DevExpress.XtraCharts.ChartControl ChartControl, DevExpress.XtraCharts.ViewType ViewType, DataTable dt, String X, String Y)
        {
            if (!ViewType.ToString().Contains("3D"))
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains(X))
                    {
                        Series series1 = new Series("", ViewType);
                        ChartControl.Series.Clear();

                        double TotalStone = Convert.ToDouble(dt.Copy().Compute("Max(" + Y + ")", ""));
                        DataView view = dt.Copy().DefaultView;
                        DataTable distinctValues = view.ToTable(true, X, Y);

                        series1.DataSource = distinctValues;
                        series1.ArgumentScaleType = ScaleType.Auto;
                        series1.ArgumentDataMember = X;
                        series1.ValueScaleType = ScaleType.Numerical;
                        series1.ValueDataMembers.AddRange(new string[] { Y });
                        ChartControl.Series.Add(series1);

                        DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
                        ((DevExpress.XtraCharts.XYDiagram)ChartControl.Diagram).AxisX.Range.SideMarginsEnabled = true;
                        ((DevExpress.XtraCharts.XYDiagram)ChartControl.Diagram).AxisX.Range.Auto = false;

                    }
                }
            }
            else
            {
                DataView view = dt.Copy().DefaultView;
                DataTable distinctValues = view.ToTable(true, X, Y);
                DataTable Xtable = distinctValues.Copy().DefaultView.ToTable(true, X);
                ChartControl.Series.Clear();
                Series Pie3DSeries = new Series("", ViewType);

                var ownerGroups = distinctValues.AsEnumerable().GroupBy(row => row.Field<string>(X));

                var dt2 = distinctValues.Clone();
                dt2.Columns[Y].DataType = typeof(decimal);
                var intColumns = dt2.Columns.Cast<DataColumn>()
                    .Where(c => c.DataType == typeof(decimal)).ToArray();
                foreach (var grp in ownerGroups)
                {
                    if (grp.Key != null && grp.Key != "")
                    {
                        var row = dt2.Rows.Add();
                        row.SetField(X, grp.Key);

                        foreach (DataColumn col in intColumns)
                        {
                            string Exp = X + "='" + grp.Key + "'";
                            double sum = Convert.ToDouble(distinctValues.Compute("sum(" + col + ")", Exp));
                            row.SetField(col, sum);
                        }
                    }
                }


                Pie3DSeries.DataSource = dt2;
                ChartControl.Series.Add(Pie3DSeries);
                Pie3DSeries.ArgumentScaleType = ScaleType.Auto;
                Pie3DSeries.ArgumentDataMember = X;
                Pie3DSeries.ValueScaleType = ScaleType.Numerical;
                Pie3DSeries.ValueDataMembers.AddRange(new string[] { Y });

                ChartControl.Legend.Visible = false;
                Pie3DSeries.Label.PointOptions.PointView = PointView.ArgumentAndValues;
                Pie3DSeries.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
                Pie3DSeries.Label.PointOptions.ValueNumericOptions.Precision = 0;
                //((SimpleDiagram3D)ChartControl.Diagram).RuntimeRotation = true;
                //((SimpleDiagram3D)ChartControl.Diagram).RuntimeZooming = true;

            }
        }
        public void GenerateBARChart(DevExpress.XtraCharts.ChartControl ChartControl, DevExpress.XtraCharts.ViewType ViewType, DataSet dset)
        {
            if (ViewType == ViewType.Area)
            {
                if (dset.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dset.Tables.Count; i++)
                    {
                        if (dset.Tables[i].Columns.Contains(dset.Tables[i].Columns[1].ColumnName))
                        {
                            Series series1 = new Series("", ViewType);
                            ChartControl.Series.Clear();

                            double TotalStone = Convert.ToDouble(dset.Tables[i].Copy().Compute("Max(" + dset.Tables[i].Columns[0] + ")", ""));
                            DataView view = dset.Tables[i].Copy().DefaultView;
                            DataTable distinctValues = view.ToTable(true, dset.Tables[i].Columns[1].ColumnName, dset.Tables[i].Columns[0].ColumnName);

                            series1.DataSource = distinctValues;
                            series1.ArgumentScaleType = ScaleType.Auto;
                            series1.ArgumentDataMember = dset.Tables[i].Columns[1].ColumnName;
                            series1.ValueScaleType = ScaleType.Numerical;
                            series1.ValueDataMembers.AddRange(new string[] { dset.Tables[i].Columns[0].ColumnName });
                            ChartControl.Series.Add(series1);

                            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
                            ((DevExpress.XtraCharts.XYDiagram)ChartControl.Diagram).AxisX.Range.SideMarginsEnabled = true;
                            ((DevExpress.XtraCharts.XYDiagram)ChartControl.Diagram).AxisX.Range.Auto = false;

                        }
                    }
                }
            }

        }

        private void BtnGenerateChart_Click(object sender, EventArgs e)
        {
            try
            {
                if (Val.ToString(CmbXAxis.SelectedItem) == "")
                {
                    return;
                }
                if (Val.ToString(CmbYAxis.SelectedItem) == "")
                {
                    return;
                }
                if (Val.ToString(CmbXAxis.SelectedItem) == Val.ToString(CmbYAxis.SelectedItem))
                {
                    return;
                }
               
                SimpleButton btn = (SimpleButton)sender;
                lblChart.Text = btn.Text;
                ViewType en = (ViewType)ViewType.Parse(typeof(ViewType), btn.Text);
           
                GenerateChart(chartControl, en, DTabSummary, Val.ToString(CmbXAxis.SelectedItem), Val.ToString(CmbYAxis.SelectedItem));
    
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
            }
        }

        private void CmbXAxis_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Val.ToString(CmbXAxis.SelectedItem) == "")
                {
                    return;
                }
                if (Val.ToString(CmbYAxis.SelectedItem) == "")
                {
                    return;
                }
                if (Val.ToString(CmbXAxis.SelectedItem) == Val.ToString(CmbYAxis.SelectedItem))
                {
                    return;
                }
                if (lblChart.Text.Trim().Length == 0)
                {
                    lblChart.Text = "Bar";
                }
                ViewType en = (ViewType)ViewType.Parse(typeof(ViewType), lblChart.Text);

                GenerateChart(chartControl, en, DTabSummary, Val.ToString(CmbXAxis.SelectedItem), Val.ToString(CmbYAxis.SelectedItem));

            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
            }

            
        }

        private void BtnChartExport_Click(object sender, EventArgs e)
        {
            //Export("xlsx", "Export to Excel", "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*");

            SaveFileDialog svDialog = new SaveFileDialog();
            svDialog.DefaultExt = "xlsx";
            svDialog.Title = "Export to Excel";
            svDialog.FileName = "Report";
            svDialog.Filter = "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
            {
                string Filepath = svDialog.FileName;
                try
                {
                    chartControl.ExportToXlsx(Filepath);
                    System.Diagnostics.Process.Start(Filepath, "CMD");
                }
                catch (Exception EX)
                {
                    Global.Message(EX.Message);
                }
            }
        }

        private void BtnPdfChart_Click(object sender, EventArgs e)
        {
            //Export("xlsx", "Export to Excel", "Excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*");

            SaveFileDialog svDialog = new SaveFileDialog();
            svDialog.DefaultExt = "Pdf";
            svDialog.Title = "Export to Pdf";
            svDialog.FileName = "Report";
            svDialog.Filter = "Pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";
            if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
            {
                string Filepath = svDialog.FileName;
                try
                {
                    chartControl.ExportToPdf(Filepath);
                    System.Diagnostics.Process.Start(Filepath, "CMD");
                }
                catch (Exception EX)
                {
                    Global.Message(EX.Message);
                }
            }
        }

        private void GrdDet_CustomColumnSort(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
        {
            GridView view = sender as GridView;
            if (view == null) return;
            try
            {
                object val1 = Val.Val( view.GetListSourceRowCellValue(e.ListSourceRowIndex1, Val.ToString(e.Column.Tag)));
                object val2 = Val.Val(view.GetListSourceRowCellValue(e.ListSourceRowIndex2, Val.ToString(e.Column.Tag)));
                e.Handled = true;
                e.Result = System.Collections.Comparer.Default.Compare(val1, val2);
            }
            catch (Exception ee)
            {
                //...
            }
        }
    }
}