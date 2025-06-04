using BusLib.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
using DevExpress.XtraGrid.Views.BandedGrid;
using OfficeOpenXml;
using DevExpress.XtraPrinting;
using DevExpress.Utils;

namespace MahantExport.Parcel
{
    public partial class FrmKapanAssortmentView  : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_KapanInward ObjKapan = new BOTRN_KapanInward();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();

        double DouCarat = 0;
        double DouAmount = 0;
        double DouNewCarat = 0;
        double DouNewAmount = 0;
        double DouByCarat = 0;
        double DouByAmount = 0;

        double DouGCarat = 0;
        double DouGRate = 0;
        double DouGAmount = 0;

        double DouExcRate = 0;
        double DouMumbaiAmt = 0;

        BODevGridSelection ObjGridSelection;

        DataTable DTabDetail = new DataTable();

        public FrmKapanAssortmentView()
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
            ObjFormEvent.ObjToDisposeList.Add(ObjKapan);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            this.Show();

			GrdSummry.BestFitColumns();

            if (MainGridKapan.RepositoryItems.Count == 0)
            {
                ObjGridSelection = new BODevGridSelection();
                ObjGridSelection.View = GrdDetKapan;
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 1;
            }
            else
            {
                ObjGridSelection.ClearSelection();
            }
            GrdDetKapan.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;
            GrdDetKapan.Bands["gridBand5"].Fixed = FixedStyle.None;
            GridBand band = GrdDetKapan.Bands.AddBand("..");
            band.Columns.Add(GrdDetKapan.Columns["COLSELECTCHECKBOX"]);
            band.Fixed = FixedStyle.Left;
            band.VisibleIndex = 0;
            GrdDetKapan.Bands["gridBand5"].Fixed = FixedStyle.Left;
            if (ObjGridSelection != null)
            {
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 1;
            }


        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                Global.ExcelExport("AssortmentViewSummary", GrdSummry);
            }
            else
            {
                Global.ExcelExport("AssortmentView", GrdDetSummry);
            }
        }


        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;
                string StrStatus = ",";

                StrStatus = StrStatus + ",";

                DataSet DS = ObjKapan.AssortmentViewKapanSummaryGetData(txtSearchKapanName.Text,txtMergeKapan.Text);

                MainGrid.DataSource = DS.Tables[0];
                MainGrid.Refresh();

                MainGridKapan.DataSource = DS.Tables[1];
                MainGridKapan.Refresh();

                MainGrdSummry.DataSource = DS.Tables[2];
                MainGrdSummry.Refresh();

                GrdSummry.Columns["KAPANNAME"].Group();
                GrdSummry.ExpandAllGroups();

                if (GrdSummry.GroupSummary.Count == 0)
                {
                    GrdSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, "KAPANNAME", GrdSummry.Columns["KAPANNAME"], "{0:N0}");
                    GrdSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "CARAT", GrdSummry.Columns["CARAT"], "{0:N2}");
                    GrdSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Custom, "PRICEPERCARAT", GrdSummry.Columns["PRICEPERCARAT"], "{0:N2}");
                    GrdSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Custom, "MUMBAI", GrdSummry.Columns["MUMBAI"], "{0:N2}");
                    GrdSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Average, "EXCRATE", GrdSummry.Columns["EXCRATE"], "{0:N2}");
                    GrdSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "AMOUNT", GrdSummry.Columns["AMOUNT"], "{0:N2}");
                }
                ChkGroup_CheckedChanged(null, null);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }

        }

        private void GrdDetSummry_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            //if (e.RowHandle < 0)
            //{
            //    return;
            //}

            //string StrStatus = Val.ToString(GrdDetSummry.GetRowCellValue(e.RowHandle, "STATUS"));
            //if (StrStatus == "PENDING")
            //{
            //    e.Appearance.BackColor = lblPending.BackColor;
            //    e.Appearance.BackColor2 = lblPending.BackColor;
            //}
            //else if (StrStatus == "PARTIAL")
            //{
            //    e.Appearance.BackColor = lblPartial.BackColor;
            //    e.Appearance.BackColor2 = lblPartial.BackColor;
            //}
            //else if (StrStatus == "COMPLETE")
            //{
            //    e.Appearance.BackColor = Color.Transparent;
            //    e.Appearance.BackColor2 = Color.Transparent;
            //}
        }

        private void GrdDetSummry_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000")
            {
                e.DisplayText = String.Empty;
            }
        }

        private void ChkGroup_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkGroup.Checked == true)
            {
                GrdDetSummry.Columns["SHAPESIZE"].Group();
                GrdDetSummry.Columns["SHAPESIZE"].Visible = false;

                if (GrdDetSummry.GroupSummary.Count == 0)
                {
                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Count, "MIXCLARITYNAME", GrdDetSummry.Columns["MIXCLARITYNAME"], "{0:N0}");

                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "CARAT", GrdDetSummry.Columns["CARAT"], "{0:N3}");
                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Custom, "PRICEPERCARAT", GrdDetSummry.Columns["PRICEPERCARAT"], "{0:N3}");
                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "AMOUNT", GrdDetSummry.Columns["AMOUNT"], "{0:N3}");

                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "NEWCARAT", GrdDetSummry.Columns["NEWCARAT"], "{0:N5}");
                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Custom, "NEWPRICEPERCARAT", GrdDetSummry.Columns["NEWPRICEPERCARAT"], "{0:N3}");
                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "NEWAMOUNT", GrdDetSummry.Columns["NEWAMOUNT"], "{0:N5}");

                    //GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "CARAT", GrdDetSummry.Columns["BYTRANSCARAT"], "{0:N3}");
                    //GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Custom, "PRICEPERCARAT", GrdDetSummry.Columns["BYTRANSFERPRICEPERCARAT"], "{0:N3}");
                    //GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "AMOUNT", GrdDetSummry.Columns["BYTRANSAMOUNT"], "{0:N3}");
                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "BYTRANSCARAT", GrdDetSummry.Columns["BYTRANSCARAT"], "{0:N5}");
                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Custom, "BYTRANSFERPRICEPERCARAT", GrdDetSummry.Columns["BYTRANSFERPRICEPERCARAT"], "{0:N3}");
                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "BYTRANSAMOUNT", GrdDetSummry.Columns["BYTRANSAMOUNT"], "{0:N5}");

                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "CARATPER", GrdDetSummry.Columns["CARATPER"], "{0:N3}");
                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "AMOUNTPER", GrdDetSummry.Columns["AMOUNTPER"], "{0:N3}");
                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "NEWCARATPER", GrdDetSummry.Columns["NEWCARATPER"], "{0:N3}");
                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "NEWAMOUNTPER", GrdDetSummry.Columns["NEWAMOUNTPER"], "{0:N3}");

                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "NEWCARATPER", GrdDetSummry.Columns["BYTRANSFERCARATPER"], "{0:N3}");
                    GrdDetSummry.GroupSummary.Add(DevExpress.Data.SummaryItemType.Sum, "NEWAMOUNTPER", GrdDetSummry.Columns["BYTRANSFERAMOUNTPER"], "{0:N3}");

                }

                GrdDetSummry.ExpandAllGroups();
            }
            else
            {
                GrdDetSummry.Columns["SHAPESIZE"].UnGroup();
                GrdDetSummry.Columns["SHAPESIZE"].Visible = false;
            }
        }

        private void GrdDetSummry_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            try
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    DouCarat = 0;
                    DouAmount = 0;
                   
                    DouNewCarat = 0;
                    DouNewAmount = 0;

                    DouByCarat = 0;
                    DouByAmount = 0;
                }
                else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DouCarat = DouCarat + Val.Val(GrdDetSummry.GetRowCellValue(e.RowHandle, "CARAT"));
                    DouAmount = DouAmount + Val.Val(GrdDetSummry.GetRowCellValue(e.RowHandle, "AMOUNT"));

                    DouNewCarat = DouNewCarat + Val.Val(GrdDetSummry.GetRowCellValue(e.RowHandle, "NEWCARAT"));
                    DouNewAmount = DouNewAmount + Val.Val(GrdDetSummry.GetRowCellValue(e.RowHandle, "NEWAMOUNT"));

                    DouByCarat = DouByCarat + Val.Val(GrdDetSummry.GetRowCellValue(e.RowHandle, "BYTRANSCARAT"));
                    DouByAmount = DouByAmount + Val.Val(GrdDetSummry.GetRowCellValue(e.RowHandle, "BYTRANSAMOUNT"));

                }
                else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("PRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("NEWPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouNewCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouNewAmount) / Val.Val(DouNewCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("BYTRANSFERPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouByCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouByAmount) / Val.Val(DouByCarat), 2);
                        else
                            e.TotalValue = 0;
                    }

                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
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
                    //e.Graphics.DrawLine(new Pen(Brushes.Black, 1), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.BandsInfo[i].Bounds.Top), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.RowsInfo[info.RowsInfo.Count - 1].Bounds.Bottom - 1)); //comment on 11-03-2023
                    //if (i == 1) e.Graphics.DrawLine(new Pen(Brushes.Black), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.BandsInfo[i].Bounds.Top), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.RowsInfo[info.RowsInfo.Count - 1].Bounds.Bottom - 1));
                    //if (i == info.BandsInfo.BandCount - 1) e.Graphics.DrawLine(new Pen(Brushes.Black), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.BandsInfo[i].Bounds.Top), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.RowsInfo[info.RowsInfo.Count - 1].Bounds.Bottom - 1));
                }

            }
            catch (Exception)
            {

            }
        }

        private void GrdDetKapan_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                string StrKapan = txtSearchKapanName.Text;
                //GrdDetKapan.PostEditor();
                //for (int i = 0; i < GrdDetKapan.RowCount; i++)
                //{
                //    DataRow DrDetail = GrdDetKapan.GetDataRow(i);

                //    if (DrDetail != null && Val.ToBoolean(GrdDetKapan.GetFocusedRowCellValue("COLSELECTCHECKBOX")))
                //    {
                //        StrKapan = ((StrKapan == "") ? Val.ToString(DrDetail["KAPANNAME"]) : (StrKapan + "," + Val.ToString(DrDetail["KAPANNAME"])));
                //    }
                //}
                // txtSearchKapanName.Text = StrKapan;


                if (Val.ToBoolean(GrdDetKapan.GetFocusedRowCellValue("COLSELECTCHECKBOX")))
                {
                    StrKapan = StrKapan == "" ? Val.ToString(GrdDetKapan.GetFocusedRowCellValue("KAPANNAME")) : StrKapan + "," + Val.ToString(GrdDetKapan.GetFocusedRowCellValue("KAPANNAME"));
                }
                else
                {
                    StrKapan = Val.ToString(StrKapan).Replace(Val.ToString(GrdDetKapan.GetFocusedRowCellValue("KAPANNAME")), "");
                }
                txtSearchKapanName.Text = StrKapan;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void BtnReportExport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                DataSet DS = ObjKapan.AssortmentViewKapanSummaryExportExcel(txtSearchKapanName.Text,"");
                
                DataTable DTabDetail = DS.Tables[0];
                DataTable DTabKapanDeptShape = DS.Tables[1];
                DataTable DTabKapanDept = DS.Tables[2];
                DataTable DTabKapan = DS.Tables[3];
                DataTable DTabDepartment = DS.Tables[4];
                DataTable DTabShape = DS.Tables[5];

                if (DTabDetail.Rows.Count == 0 )
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("No Detail Found For Export");
                    return;
                }
                
                string StrFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.LightGray;
                Color FontColor = Color.Black;
                string FontName = "verdana";
                float FontSize = 8;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                double DouTotalAmount = 0;
                double DouTotalCarat = 0;
                int IntTotalCount = 0;

                double DouAmountPer = 0;
                double DouCaratPer = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("SKE_Assortment_" + DateTime.Now.ToString("ddMMyyyy"));

                    StartRow = 2;
                    EndRow = StartRow ;
                    StartColumn = 1;
                    EndColumn = 10;

                    DataTable DTabKapanDistinct =  DTabKapan.DefaultView.ToTable(true, "KAPANNAME");
                    
                    foreach (DataRow DRowKapan in DTabKapanDistinct.Rows)
                    {
                        StartRow = StartRow + 1;
                        StartColumn = 1;
                        string StrKapan = Val.ToString(DRowKapan["KAPANNAME"]);

                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Value = "Kapan : " + StrKapan;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Merge = true;

                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Name = FontName;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Size = FontSize;

                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Bold = true;

                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Fill.PatternColor.SetColor(Color.FromArgb(225, 150, 150));
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(225, 150, 150));
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Color.SetColor(FontColor);

                        //worksheet.Row(StartRow).OutlineLevel = 5;//RapValue
                        //worksheet.Column(StartRow).Collapsed = true;
                        
                        StartRow = StartRow + 1;
                        EndRow = StartRow;
                        StartColumn = StartColumn + 1;
                        EndColumn = 10;

                        DataRow[] UDRow = DTabDetail.Select("KAPANNAME='" + StrKapan + "'");
                        if (UDRow.Length == 0)
                        {
                            continue;
                        }
                        DataTable DTabDistinctDepartment = UDRow.CopyToDataTable().DefaultView.ToTable(true, "DEPARTMENT_ID", "DEPARTMENTNAME");

                        string StrDepartment = string.Empty;
                        int IntDepartmentID = 0;

                        foreach (DataRow DRowDept in DTabDistinctDepartment.Rows)
                        {
                            IntDepartmentID = Val.ToInt(DRowDept["DEPARTMENT_ID"]);
                            StrDepartment = Val.ToString(DRowDept["DEPARTMENTNAME"]);

                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Value = "Dept : " + StrDepartment;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Merge = true;

                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Name = FontName;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Size = FontSize;

                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Bold = true;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Fill.PatternColor.SetColor(Color.FromArgb(176, 196, 222));
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(176, 196, 222));
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Color.SetColor(FontColor);

                            UDRow = DTabDetail.Select("KAPANNAME='" + StrKapan + "' AND DEPARTMENT_ID='"+IntDepartmentID.ToString() +"'");
                            if (UDRow.Length == 0)
                            {
                                continue;
                            }

                            DataTable DTabDistinctShape = UDRow.CopyToDataTable().DefaultView.ToTable(true, "SHAPE_ID", "SHAPENAME", "SHAPESEQ");
                            DTabDistinctShape.DefaultView.Sort = "SHAPESEQ";
                            DTabDistinctShape = DTabDistinctShape.DefaultView.ToTable();

                            StartRow = StartRow + 1;
                            EndRow = StartRow;
                            StartColumn = StartColumn + 1;
                            EndColumn = 10;

                            string StrShape = string.Empty;
                            int IntShapeID = 0;
                            foreach (DataRow DRowShape in DTabDistinctShape.Rows)
                            {
                                IntShapeID = Val.ToInt(DRowShape["SHAPE_ID"]);
                                StrShape = Val.ToString(DRowShape["SHAPENAME"]);

                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Value = "Shape : " + StrShape;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Merge = true;

                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Name = FontName;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Size = FontSize;

                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Bold = true;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Fill.PatternColor.SetColor(Color.FromArgb(143, 188, 139));
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(143, 188, 139));
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Color.SetColor(FontColor);

                                UDRow = DTabDetail.Select("KAPANNAME='" + StrKapan + "' AND DEPARTMENT_ID='" + IntDepartmentID.ToString() + "' AND SHAPE_ID='" + IntShapeID.ToString() + "'");
                                if (UDRow.Length == 0)
                                {
                                    continue;
                                }
                                DataTable DTabSizeDetail = UDRow.CopyToDataTable().DefaultView.ToTable(true, "MIXSIZE_ID", "MIXSIZENAME", "MIXSIZESEQ");
                                DTabSizeDetail.DefaultView.Sort = "MIXSIZESEQ";
                                DTabSizeDetail = DTabSizeDetail.DefaultView.ToTable();

                                StartRow = StartRow + 1;
                                EndRow = StartRow;

                                string StrSize = "";
                                int IntSizeID = 0;
                                int IntDetailRow = StartRow;
                                int IntDetailColumn = 3;

                                int Col = 0;

                                StartColumn = 3;
                                EndRow = StartRow;
                                List<int> anArray = new List<int>();
                                List<int> anArraySummary = new List<int>();
                                
                                foreach (DataRow DRowSize in DTabSizeDetail.Rows)
                                {
                                    IntSizeID = Val.ToInt(DRowSize["MIXSIZE_ID"]);
                                    StrSize = Val.ToString(DRowSize["MIXSIZENAME"]);

                                    if (Col % 4 == 0)
                                    {
                                        StartRow = EndRow;
                                        StartColumn = 3;
                                        IntDetailRow = StartRow;

                                        if (anArray.Count != 0)
                                        {
                                            int maxValue = anArray.Max();
                                            EndRow = maxValue + 1;
                                        }
                                        else
                                        {
                                            EndRow = StartRow;
                                        } StartColumn = IntDetailColumn;
                                        
                                        EndColumn = 5;
                                        anArray.Clear();

                                    }
                                    else
                                    {

                                        StartRow = IntDetailRow;
                                        StartColumn = StartColumn + 1;
                                        EndColumn = StartColumn + 5;
                                    }

                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Value = "" + StrSize;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Merge = true;

                                    worksheet.Cells[StartRow + 1, StartColumn + 0, StartRow + 1, StartColumn + 0].Value = "Clarity";
                                    worksheet.Cells[StartRow + 1, StartColumn + 1, StartRow + 1, StartColumn + 1].Value = "Carat";
                                    worksheet.Cells[StartRow + 1, StartColumn + 2, StartRow + 1, StartColumn + 2].Value = "$/Cts";
                                    worksheet.Cells[StartRow + 1, StartColumn + 3, StartRow + 1, StartColumn + 3].Value = "Amount";
                                    worksheet.Cells[StartRow + 1, StartColumn + 4, StartRow + 1, StartColumn + 4].Value = "Cts %";
                                    worksheet.Cells[StartRow + 1, StartColumn + 5, StartRow + 1, StartColumn + 5].Value = "Amt %";

                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Font.Bold = true;
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Font.Name = FontName;
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Font.Size = FontSize;

                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Fill.PatternColor.SetColor(BackColor);
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(BackColor);
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                                    StartRow = StartRow + 2;

                                    UDRow = DTabDetail.Select("KAPANNAME='" + StrKapan + "' AND DEPARTMENT_ID='" + IntDepartmentID.ToString() + "' AND SHAPE_ID='" + IntShapeID.ToString() + "' And MIXSIZE_ID='" + IntSizeID.ToString() + "'");
                                    if (UDRow.Length == 0)
                                    {
                                        continue;
                                    }

                                    DataTable DTabFinalDetail = UDRow.CopyToDataTable();
                                    DTabFinalDetail.DefaultView.Sort = "MIXCLARITYSEQ";
                                    DTabFinalDetail = DTabFinalDetail.DefaultView.ToTable();

                                    anArray.Add(DTabFinalDetail.Rows.Count);
                                  

                                    DouTotalAmount = Val.Val(DTabFinalDetail.Compute("SUM(AMOUNT)",""));
                                    DouTotalCarat = Val.Val(DTabFinalDetail.Compute("SUM(CARAT)", ""));
                                    IntTotalCount = DTabFinalDetail.Rows.Count;
                                    
                                    DouAmountPer = 0;
                                    DouCaratPer = 0;
                                        
                                    foreach (DataRow DRowClarityDetail in DTabFinalDetail.Rows)
                                    {
                                        DouAmountPer = DouTotalAmount == 0 ? 0 : Math.Round(Val.Val(DRowClarityDetail["AMOUNT"]) / DouTotalAmount, 2);
                                        DouCaratPer = DouTotalCarat == 0 ? 0 : Math.Round(Val.Val(DRowClarityDetail["CARAT"]) / DouTotalCarat, 2);
                                        
                                        worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = Val.ToString(DRowClarityDetail["MIXCLARITYNAME"]);
                                        worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = Val.Val(DRowClarityDetail["CARAT"]);
                                        worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Style.Numberformat.Format = "0.00";

                                        worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = Val.Val(DRowClarityDetail["PRICEPERCARAT"]);
                                        worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Style.Numberformat.Format = "0.00";

                                        worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = Val.Val(DRowClarityDetail["AMOUNT"]);
                                        worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Style.Numberformat.Format = "0.00";
                                        
                                        worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = DouCaratPer;
                                        worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Style.Numberformat.Format = "0.00%";

                                        worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = DouAmountPer;
                                        worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Style.Numberformat.Format = "0.00%";

                                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;

                                        StartRow = StartRow + 1;

                                    }

                                    
                                    // StartColumn = IntDetailColumn;
                                    worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = "Total (" + IntTotalCount.ToString() + ")";
                                    worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = DouTotalCarat;
                                    worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Style.Numberformat.Format = "0.00";

                                    worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = DouTotalCarat == 0 ? 0 : Math.Round(DouTotalAmount / DouTotalCarat, 2);
                                    worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Style.Numberformat.Format = "0.00";
                                    
                                    worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = DouTotalAmount;
                                    worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Style.Numberformat.Format = "0.00";
                                    
                                    worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = "100";
                                    worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = "100";

                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;

                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Bold = true;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternColor.SetColor(BackColor);
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(BackColor);
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                                    Col++;

                                    StartColumn = StartColumn + 6;
                                    anArraySummary.Add(StartRow);
                                  
                                } // Size End

                                // Shape Wise Summary
                                StartRow = anArraySummary.Max() + 1;

                                UDRow= DTabKapanDeptShape.Select("KAPANNAME='" + StrKapan + "' AND DEPARTMENT_ID='" + IntDepartmentID.ToString() + "' AND SHAPE_ID='" + IntShapeID.ToString() + "'");
                                if (UDRow.Length == 0)
                                {
                                    continue;
                                }
                                DataTable DTabSizeDetailSummary = UDRow.CopyToDataTable();
                                DTabSizeDetailSummary.DefaultView.Sort = "MIXSIZESEQ";
                                DTabSizeDetailSummary = DTabSizeDetailSummary.DefaultView.ToTable();

                                StartColumn = 3;
                                
                                StartRow = StartRow + 1;

                                worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = "Shp Summ";
                                worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = "Carat";
                                worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = "$/Cts";
                                worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = "Amount";
                                worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = "Cts %";
                                worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = "Amt %";

                                worksheet.Cells[StartRow, StartColumn, StartRow , StartColumn + 5].Style.Font.Bold = true;
                                worksheet.Cells[StartRow, StartColumn, StartRow , StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow , StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow , StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow , StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow , StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheet.Cells[StartRow, StartColumn, StartRow , StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                worksheet.Cells[StartRow, StartColumn, StartRow , StartColumn + 5].Style.Font.Name = FontName;
                                worksheet.Cells[StartRow, StartColumn, StartRow , StartColumn + 5].Style.Font.Size = FontSize;
                                worksheet.Cells[StartRow, StartColumn, StartRow , StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[StartRow, StartColumn, StartRow , StartColumn + 5].Style.Fill.PatternColor.SetColor(Color.FromArgb(143, 188, 139));
                                worksheet.Cells[StartRow, StartColumn, StartRow , StartColumn + 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(143, 188, 139));
                                worksheet.Cells[StartRow, StartColumn, StartRow , StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                                DouTotalAmount = Val.Val(DTabSizeDetailSummary.Compute("SUM(AMOUNT)", ""));
                                DouTotalCarat = Val.Val(DTabSizeDetailSummary.Compute("SUM(CARAT)", ""));
                                IntTotalCount = DTabSizeDetailSummary.Rows.Count;

                                DouAmountPer = 0;
                                DouCaratPer = 0;

                                StartRow = StartRow + 1;
                                EndRow = StartRow;

                                foreach (DataRow DRowClarityDetail in DTabSizeDetailSummary.Rows)
                                {
                                    DouAmountPer = DouTotalAmount == 0 ? 0 : Math.Round(Val.Val(DRowClarityDetail["AMOUNT"]) / DouTotalAmount, 2);
                                    DouCaratPer = DouTotalCarat == 0 ? 0 : Math.Round(Val.Val(DRowClarityDetail["CARAT"]) / DouTotalCarat, 2);

                                    worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = Val.ToString(DRowClarityDetail["MIXSIZENAME"]);
                                    worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = Val.Val(DRowClarityDetail["CARAT"]);
                                    worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Style.Numberformat.Format = "0.00";

                                    worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = Val.Val(DRowClarityDetail["PRICEPERCARAT"]);
                                    worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Style.Numberformat.Format = "0.00";

                                    worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = Val.Val(DRowClarityDetail["AMOUNT"]);
                                    worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Style.Numberformat.Format = "0.00";

                                    worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = DouCaratPer;
                                    worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Style.Numberformat.Format = "0.00%";

                                    worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = DouAmountPer;
                                    worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Style.Numberformat.Format = "0.00%";

                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternColor.SetColor(Color.FromArgb(143, 188, 139));
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(143, 188, 139));
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                                    StartRow = StartRow + 1;

                                } // Size End
                                // StartColumn = IntDetailColumn;
                                worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = StrShape;
                                worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = DouTotalCarat;
                                worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Style.Numberformat.Format = "0.00";

                                worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = DouTotalCarat == 0 ? 0 : Math.Round(DouTotalAmount / DouTotalCarat, 2);
                                worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Style.Numberformat.Format = "0.00";

                                worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = DouTotalAmount;
                                worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Style.Numberformat.Format = "0.00";

                                worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = "100";
                                worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = "100";

                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;

                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Bold = true;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternColor.SetColor(Color.FromArgb(143, 188, 139));
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(143, 188, 139));
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                                StartColumn = StartColumn + 6;
                                StartRow = StartRow + 1;

                            } // Shape End
                            StartRow = StartRow + 1;

                            // Department Wise Summary

                            UDRow = DTabKapanDept.Select("KAPANNAME='" + StrKapan + "' AND DEPARTMENT_ID='" + IntDepartmentID.ToString() + "'");
                            if (UDRow.Length == 0)
                            {
                                continue;
                            }
                            DataTable DTabDeptDetailSummary = UDRow.CopyToDataTable();
                            DTabDeptDetailSummary.DefaultView.Sort = "MIXSIZESEQ";
                            DTabDeptDetailSummary = DTabDeptDetailSummary.DefaultView.ToTable();

                            StartColumn = 2;

                            worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = "Dept Summ";
                            worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = "Carat";
                            worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = "$/Cts";
                            worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = "Amount";
                            worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = "Cts %";
                            worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = "Amt %";

                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Bold = true;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternColor.SetColor(Color.FromArgb(176, 196, 222));
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(176, 196, 222));
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                            DouTotalAmount = Val.Val(DTabDeptDetailSummary.Compute("SUM(AMOUNT)", ""));
                            DouTotalCarat = Val.Val(DTabDeptDetailSummary.Compute("SUM(CARAT)", ""));
                            IntTotalCount = DTabDeptDetailSummary.Rows.Count;

                            DouAmountPer = 0;
                            DouCaratPer = 0;

                            StartRow = StartRow + 1;
                            EndRow = StartRow;

                            foreach (DataRow DRowClarityDetail in DTabDeptDetailSummary.Rows)
                            {
                                DouAmountPer = DouTotalAmount == 0 ? 0 : Math.Round(Val.Val(DRowClarityDetail["AMOUNT"]) / DouTotalAmount, 2);
                                DouCaratPer = DouTotalCarat == 0 ? 0 : Math.Round(Val.Val(DRowClarityDetail["CARAT"]) / DouTotalCarat, 2);

                                worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = Val.ToString(DRowClarityDetail["MIXSIZENAME"]);
                                worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = Val.Val(DRowClarityDetail["CARAT"]);
                                worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Style.Numberformat.Format = "0.00";

                                worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = Val.Val(DRowClarityDetail["PRICEPERCARAT"]);
                                worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Style.Numberformat.Format = "0.00";

                                worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = Val.Val(DRowClarityDetail["AMOUNT"]);
                                worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Style.Numberformat.Format = "0.00";

                                worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = DouCaratPer;
                                worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Style.Numberformat.Format = "0.00%";

                                worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = DouAmountPer;
                                worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Style.Numberformat.Format = "0.00%";

                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternColor.SetColor(Color.FromArgb(176, 196, 222));
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(176, 196, 222));
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                                StartRow = StartRow + 1;

                            } // Size End
                            // StartColumn = IntDetailColumn;
                            worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = StrDepartment;
                            worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = DouTotalCarat;
                            worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Style.Numberformat.Format = "0.00";

                            worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = DouTotalCarat == 0 ? 0 : Math.Round(DouTotalAmount / DouTotalCarat, 2);
                            worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Style.Numberformat.Format = "0.00";

                            worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = DouTotalAmount;
                            worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Style.Numberformat.Format = "0.00";

                            worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = "100";
                            worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = "100";

                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;

                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Bold = true;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternColor.SetColor(Color.FromArgb(176, 196, 222));
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(176, 196, 222));
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                            StartColumn = StartColumn + 6;
                            StartRow = StartRow + 1;

                        } // Department End


                        // Kapan Wise Summary

                        UDRow = DTabKapan.Select("KAPANNAME='" + StrKapan + "' ");
                        if (UDRow.Length == 0)
                        {
                            continue;
                        }
                        DataTable DTabKapanSummary = UDRow.CopyToDataTable();
                        DTabKapanSummary.DefaultView.Sort = "MIXSIZESEQ";
                        DTabKapanSummary = DTabKapanSummary.DefaultView.ToTable();

                        StartColumn = 1;
                        StartRow = StartRow + 1;

                        worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = "Kapan Summ";
                        worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = "Carat";
                        worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = "$/Cts";
                        worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = "Amount";
                        worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = "Cts %";
                        worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = "Amt %";

                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Bold = true;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternColor.SetColor(Color.FromArgb(225, 150, 150));
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(225, 150, 150));
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                        DouTotalAmount = Val.Val(DTabKapanSummary.Compute("SUM(AMOUNT)", ""));
                        DouTotalCarat = Val.Val(DTabKapanSummary.Compute("SUM(CARAT)", ""));
                        IntTotalCount = DTabKapanSummary.Rows.Count;

                        DouAmountPer = 0;
                        DouCaratPer = 0;

                        StartRow = StartRow + 1;
                        EndRow = StartRow;

                        foreach (DataRow DRowClarityDetail in DTabKapanSummary.Rows)
                        {
                            DouAmountPer = DouTotalAmount == 0 ? 0 : Math.Round(Val.Val(DRowClarityDetail["AMOUNT"]) / DouTotalAmount, 2);
                            DouCaratPer = DouTotalCarat == 0 ? 0 : Math.Round(Val.Val(DRowClarityDetail["CARAT"]) / DouTotalCarat, 2);

                            worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = Val.ToString(DRowClarityDetail["MIXSIZENAME"]);
                            worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = Val.Val(DRowClarityDetail["CARAT"]);
                            worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Style.Numberformat.Format = "0.00";

                            worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = Val.Val(DRowClarityDetail["PRICEPERCARAT"]);
                            worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Style.Numberformat.Format = "0.00";

                            worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = Val.Val(DRowClarityDetail["AMOUNT"]);
                            worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Style.Numberformat.Format = "0.00";

                            worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = DouCaratPer;
                            worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Style.Numberformat.Format = "0.00%";

                            worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = DouAmountPer;
                            worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Style.Numberformat.Format = "0.00%";

                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternColor.SetColor(Color.FromArgb(225, 150, 150));
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(225, 150, 150));
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                            StartRow = StartRow + 1;

                        } // Size End
                        // StartColumn = IntDetailColumn;
                        worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = StrKapan;
                        worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = DouTotalCarat;
                        worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Style.Numberformat.Format = "0.00";

                        worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = DouTotalCarat == 0 ? 0 : Math.Round(DouTotalAmount / DouTotalCarat, 2);
                        worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Style.Numberformat.Format = "0.00";

                        worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = DouTotalAmount;
                        worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Style.Numberformat.Format = "0.00";

                        worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = "100";
                        worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = "100";

                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;

                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Bold = true;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternColor.SetColor(Color.FromArgb(225, 150, 150));
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(225, 150, 150));
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                        StartColumn = StartColumn + 6;
                        StartRow = StartRow + 1;

                    } // Kapan End

                    xlPackage.Save();

                    System.Diagnostics.Process.Start(StrFilePath, "CMD");
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void btnCaratWiseExport_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "FORMAT";
                this.Cursor = Cursors.WaitCursor;
                FrmSearch.mDTab = Global.GetKapanAssortmentExportFileTemplate();

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
                    return;
                }

                DataSet DS = ObjKapan.AssortmentViewKapanSummaryExportExcel(txtSearchKapanName.Text, FormatName);

                DataTable DTabDetail = DS.Tables[0];
                DataTable DTabKapanDeptShape = DS.Tables[1];
                DataTable DTabKapanDept = DS.Tables[2];
                DataTable DTabKapan = DS.Tables[3];
                DataTable DTabDepartment = DS.Tables[4];
                DataTable DTabShape = DS.Tables[5];

                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("No Detail Found For Export");
                    return;
                }

                string StrFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.LightGray;
                Color FontColor = Color.Black;
                string FontName = "verdana";
                float FontSize = 8;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                double DouTotalAmount = 0;
                double DouTotalCarat = 0;
                int IntTotalCount = 0;

                double DouAmountPer = 0;
                double DouCaratPer = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("SKE_Assortment_" + DateTime.Now.ToString("ddMMyyyy"));

                    StartRow = 2;
                    EndRow = StartRow;
                    StartColumn = 1;
                    EndColumn = 10;

                    DataTable DTabKapanDistinct = DTabKapan.DefaultView.ToTable(true, "KAPANNAME");

                    foreach (DataRow DRowKapan in DTabKapanDistinct.Rows)
                    {
                        StartRow = StartRow + 1;
                        StartColumn = 1;
                        string StrKapan = Val.ToString(DRowKapan["KAPANNAME"]);

                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Value = "Kapan : " + StrKapan;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Merge = true;

                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Name = FontName;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Size = FontSize;

                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Bold = true;

                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Fill.PatternColor.SetColor(Color.FromArgb(225, 150, 150));
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(225, 150, 150));
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Color.SetColor(FontColor);

                        //worksheet.Row(StartRow).OutlineLevel = 5;//RapValue
                        //worksheet.Column(StartRow).Collapsed = true;

                        StartRow = StartRow + 1;
                        EndRow = StartRow;
                        StartColumn = StartColumn + 1;
                        EndColumn = 10;

                        DataRow[] UDRow = DTabDetail.Select("KAPANNAME='" + StrKapan + "'");
                        if (UDRow.Length == 0)
                        {
                            continue;
                        }
                        DataTable DTabDistinctDepartment = UDRow.CopyToDataTable().DefaultView.ToTable(true, "DEPARTMENT_ID", "DEPARTMENTNAME");

                        string StrDepartment = string.Empty;
                        int IntDepartmentID = 0;

                        foreach (DataRow DRowDept in DTabDistinctDepartment.Rows)
                        {
                            IntDepartmentID = Val.ToInt(DRowDept["DEPARTMENT_ID"]);
                            StrDepartment = Val.ToString(DRowDept["DEPARTMENTNAME"]);

                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Value = "Dept : " + StrDepartment;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Merge = true;

                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Name = FontName;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Size = FontSize;

                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Bold = true;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Fill.PatternColor.SetColor(Color.FromArgb(176, 196, 222));
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(176, 196, 222));
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Color.SetColor(FontColor);

                            UDRow = DTabDetail.Select("KAPANNAME='" + StrKapan + "' AND DEPARTMENT_ID='" + IntDepartmentID.ToString() + "'");
                            if (UDRow.Length == 0)
                            {
                                continue;
                            }

                            DataTable DTabDistinctShape = UDRow.CopyToDataTable().DefaultView.ToTable(true, "SHAPE_ID", "SHAPENAME", "SHAPESEQ");
                            DTabDistinctShape.DefaultView.Sort = "SHAPESEQ";
                            DTabDistinctShape = DTabDistinctShape.DefaultView.ToTable();

                            StartRow = StartRow + 1;
                            EndRow = StartRow;
                            StartColumn = StartColumn + 1;
                            EndColumn = 10;

                            string StrShape = string.Empty;
                            int IntShapeID = 0;
                            foreach (DataRow DRowShape in DTabDistinctShape.Rows)
                            {
                                IntShapeID = Val.ToInt(DRowShape["SHAPE_ID"]);
                                StrShape = Val.ToString(DRowShape["SHAPENAME"]);

                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Value = "Shape : " + StrShape;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Merge = true;

                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Name = FontName;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Size = FontSize;

                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Bold = true;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Fill.PatternColor.SetColor(Color.FromArgb(143, 188, 139));
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(143, 188, 139));
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 3].Style.Font.Color.SetColor(FontColor);

                                UDRow = DTabDetail.Select("KAPANNAME='" + StrKapan + "' AND DEPARTMENT_ID='" + IntDepartmentID.ToString() + "' AND SHAPE_ID='" + IntShapeID.ToString() + "'");
                                if (UDRow.Length == 0)
                                {
                                    continue;
                                }
                                DataTable DTabSizeDetail = UDRow.CopyToDataTable().DefaultView.ToTable(true, "MIXSIZE_ID", "MIXSIZENAME", "MIXSIZESEQ");
                                DTabSizeDetail.DefaultView.Sort = "MIXSIZESEQ";
                                DTabSizeDetail = DTabSizeDetail.DefaultView.ToTable();

                                StartRow = StartRow + 1;
                                EndRow = StartRow;

                                string StrSize = "";
                                int IntSizeID = 0;
                                int IntDetailRow = StartRow;
                                int IntDetailColumn = 3;

                                int Col = 0;

                                StartColumn = 3;
                                EndRow = StartRow;
                                List<int> anArray = new List<int>();
                                List<int> anArraySummary = new List<int>();

                                foreach (DataRow DRowSize in DTabSizeDetail.Rows)
                                {
                                    IntSizeID = Val.ToInt(DRowSize["MIXSIZE_ID"]);
                                    StrSize = Val.ToString(DRowSize["MIXSIZENAME"]);

                                    if (Col % 4 == 0)
                                    {
                                        StartRow = EndRow;
                                        StartColumn = 3;
                                        IntDetailRow = StartRow;

                                        if (anArray.Count != 0)
                                        {
                                            //Add Code Khushbu 05-06-21

                                            //int maxValue = anArray.Max();
                                            int maxValue = anArraySummary.Max();
                                            EndRow = maxValue + 1;
                                            StartRow = maxValue + 2; // aDD 1 BLANK ROW
                                            IntDetailRow = StartRow;
                                        }
                                        else
                                        {
                                            EndRow = StartRow;
                                        } StartColumn = IntDetailColumn;

                                        EndColumn = 5;
                                        anArray.Clear();

                                    }
                                    else
                                    {

                                        StartRow = IntDetailRow;
                                        StartColumn = StartColumn + 1;
                                        EndColumn = StartColumn + 5;
                                    }

                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Value = "" + StrSize;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Merge = true;

                                    worksheet.Cells[StartRow + 1, StartColumn + 0, StartRow + 1, StartColumn + 0].Value = "Clarity";
                                    worksheet.Cells[StartRow + 1, StartColumn + 1, StartRow + 1, StartColumn + 1].Value = "Carat";
                                    worksheet.Cells[StartRow + 1, StartColumn + 2, StartRow + 1, StartColumn + 2].Value = "$/Cts";
                                    worksheet.Cells[StartRow + 1, StartColumn + 3, StartRow + 1, StartColumn + 3].Value = "Amount";
                                    worksheet.Cells[StartRow + 1, StartColumn + 4, StartRow + 1, StartColumn + 4].Value = "Cts %";
                                    worksheet.Cells[StartRow + 1, StartColumn + 5, StartRow + 1, StartColumn + 5].Value = "Amt %";

                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Font.Bold = true;
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Font.Name = FontName;
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Font.Size = FontSize;

                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Fill.PatternColor.SetColor(BackColor);
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(BackColor);
                                    worksheet.Cells[StartRow, StartColumn, StartRow + 1, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                                    StartRow = StartRow + 2;

                                    UDRow = DTabDetail.Select("KAPANNAME='" + StrKapan + "' AND DEPARTMENT_ID='" + IntDepartmentID.ToString() + "' AND SHAPE_ID='" + IntShapeID.ToString() + "' And MIXSIZE_ID='" + IntSizeID.ToString() + "'");
                                    if (UDRow.Length == 0)
                                    {
                                        continue;
                                    }

                                    DataTable DTabFinalDetail = UDRow.CopyToDataTable();
                                    DTabFinalDetail.DefaultView.Sort = "MIXCLARITYSEQ";
                                    DTabFinalDetail = DTabFinalDetail.DefaultView.ToTable();

                                    anArray.Add(DTabFinalDetail.Rows.Count);


                                    DouTotalAmount = Val.Val(DTabFinalDetail.Compute("SUM(AMOUNT)", ""));
                                    DouTotalCarat = Val.Val(DTabFinalDetail.Compute("SUM(CARAT)", ""));
                                    IntTotalCount = DTabFinalDetail.Rows.Count;

                                    DouAmountPer = 0;
                                    DouCaratPer = 0;

                                    foreach (DataRow DRowClarityDetail in DTabFinalDetail.Rows)
                                    {
                                        DouAmountPer = DouTotalAmount == 0 ? 0 : Math.Round(Val.Val(DRowClarityDetail["AMOUNT"]) / DouTotalAmount, 2);
                                        DouCaratPer = DouTotalCarat == 0 ? 0 : Math.Round(Val.Val(DRowClarityDetail["CARAT"]) / DouTotalCarat, 2);

                                        worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = Val.ToString(DRowClarityDetail["MIXCLARITYNAME"]);
                                        worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = Val.Val(DRowClarityDetail["CARAT"]);
                                        worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Style.Numberformat.Format = "0.00";

                                        worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = Val.Val(DRowClarityDetail["PRICEPERCARAT"]);
                                        worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Style.Numberformat.Format = "0.00";

                                        worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = Val.Val(DRowClarityDetail["AMOUNT"]);
                                        worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Style.Numberformat.Format = "0.00";

                                        worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = DouCaratPer;
                                        worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Style.Numberformat.Format = "0.00%";

                                        worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = DouAmountPer;
                                        worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Style.Numberformat.Format = "0.00%";

                                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;

                                        StartRow = StartRow + 1;

                                    }


                                    // StartColumn = IntDetailColumn;
                                    worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = "Total (" + IntTotalCount.ToString() + ")";
                                    worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = DouTotalCarat;
                                    worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Style.Numberformat.Format = "0.00";

                                    worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = DouTotalCarat == 0 ? 0 : Math.Round(DouTotalAmount / DouTotalCarat, 2);
                                    worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Style.Numberformat.Format = "0.00";

                                    worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = DouTotalAmount;
                                    worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Style.Numberformat.Format = "0.00";

                                    worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = "100";
                                    worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = "100";

                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;

                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Bold = true;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternColor.SetColor(BackColor);
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(BackColor);
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                                    Col++;

                                    StartColumn = StartColumn + 6;
                                    anArraySummary.Add(StartRow);

                                } // Size End

                                // Shape Wise Summary
                                StartRow = anArraySummary.Max() + 1;

                                UDRow = DTabKapanDeptShape.Select("KAPANNAME='" + StrKapan + "' AND DEPARTMENT_ID='" + IntDepartmentID.ToString() + "' AND SHAPE_ID='" + IntShapeID.ToString() + "'");
                                if (UDRow.Length == 0)
                                {
                                    continue;
                                }
                                DataTable DTabSizeDetailSummary = UDRow.CopyToDataTable();
                                DTabSizeDetailSummary.DefaultView.Sort = "MIXSIZESEQ";
                                DTabSizeDetailSummary = DTabSizeDetailSummary.DefaultView.ToTable();

                                StartColumn = 3;

                                StartRow = StartRow + 1;

                                worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = "Shp Summ";
                                worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = "Carat";
                                worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = "$/Cts";
                                worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = "Amount";
                                worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = "Cts %";
                                worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = "Amt %";

                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Bold = true;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternColor.SetColor(Color.FromArgb(143, 188, 139));
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(143, 188, 139));
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                                DouTotalAmount = Val.Val(DTabSizeDetailSummary.Compute("SUM(AMOUNT)", ""));
                                DouTotalCarat = Val.Val(DTabSizeDetailSummary.Compute("SUM(CARAT)", ""));
                                IntTotalCount = DTabSizeDetailSummary.Rows.Count;

                                DouAmountPer = 0;
                                DouCaratPer = 0;

                                StartRow = StartRow + 1;
                                EndRow = StartRow;

                                foreach (DataRow DRowClarityDetail in DTabSizeDetailSummary.Rows)
                                {
                                    
                                    DouAmountPer = DouTotalAmount == 0 ? 0 : Math.Round(Val.Val(DRowClarityDetail["AMOUNT"]) / DouTotalAmount, 2);
                                    DouCaratPer = DouTotalCarat == 0 ? 0 : Math.Round(Val.Val(DRowClarityDetail["CARAT"]) / DouTotalCarat, 2);

                                    worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = Val.ToString(DRowClarityDetail["MIXSIZENAME"]);
                                    worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = Val.Val(DRowClarityDetail["CARAT"]);
                                    worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Style.Numberformat.Format = "0.00";

                                    worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = Val.Val(DRowClarityDetail["PRICEPERCARAT"]);
                                    worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Style.Numberformat.Format = "0.00";

                                    worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = Val.Val(DRowClarityDetail["AMOUNT"]);
                                    worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Style.Numberformat.Format = "0.00";

                                    worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = DouCaratPer;
                                    worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Style.Numberformat.Format = "0.00%";

                                    worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = DouAmountPer;
                                    worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Style.Numberformat.Format = "0.00%";

                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternColor.SetColor(Color.FromArgb(143, 188, 139));
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(143, 188, 139));
                                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                                    StartRow = StartRow + 1;

                                } // Size End
                                // StartColumn = IntDetailColumn;
                                worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = StrShape;
                                worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = DouTotalCarat;
                                worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Style.Numberformat.Format = "0.00";

                                worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = DouTotalCarat == 0 ? 0 : Math.Round(DouTotalAmount / DouTotalCarat, 2);
                                worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Style.Numberformat.Format = "0.00";

                                worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = DouTotalAmount;
                                worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Style.Numberformat.Format = "0.00";

                                worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = "100";
                                worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = "100";

                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;

                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Bold = true;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternColor.SetColor(Color.FromArgb(143, 188, 139));
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(143, 188, 139));
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                                StartColumn = StartColumn + 6;
                                StartRow = StartRow + 1;

                            } // Shape End
                            StartRow = StartRow + 1;

                            // Department Wise Summary

                            UDRow = DTabKapanDept.Select("KAPANNAME='" + StrKapan + "' AND DEPARTMENT_ID='" + IntDepartmentID.ToString() + "'");
                            if (UDRow.Length == 0)
                            {
                                continue;
                            }
                            DataTable DTabDeptDetailSummary = UDRow.CopyToDataTable();
                            DTabDeptDetailSummary.DefaultView.Sort = "MIXSIZESEQ";
                            DTabDeptDetailSummary = DTabDeptDetailSummary.DefaultView.ToTable();

                            StartColumn = 2;

                            worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = "Dept Summ";
                            worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = "Carat";
                            worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = "$/Cts";
                            worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = "Amount";
                            worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = "Cts %";
                            worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = "Amt %";

                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Bold = true;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternColor.SetColor(Color.FromArgb(176, 196, 222));
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(176, 196, 222));
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                            DouTotalAmount = Val.Val(DTabDeptDetailSummary.Compute("SUM(AMOUNT)", ""));
                            DouTotalCarat = Val.Val(DTabDeptDetailSummary.Compute("SUM(CARAT)", ""));
                            IntTotalCount = DTabDeptDetailSummary.Rows.Count;

                            DouAmountPer = 0;
                            DouCaratPer = 0;

                            StartRow = StartRow + 1;
                            EndRow = StartRow;

                            foreach (DataRow DRowClarityDetail in DTabDeptDetailSummary.Rows)
                            {
                                DouAmountPer = DouTotalAmount == 0 ? 0 : Math.Round(Val.Val(DRowClarityDetail["AMOUNT"]) / DouTotalAmount, 2);
                                DouCaratPer = DouTotalCarat == 0 ? 0 : Math.Round(Val.Val(DRowClarityDetail["CARAT"]) / DouTotalCarat, 2);

                                worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = Val.ToString(DRowClarityDetail["MIXSIZENAME"]);
                                worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = Val.Val(DRowClarityDetail["CARAT"]);
                                worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Style.Numberformat.Format = "0.00";

                                worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = Val.Val(DRowClarityDetail["PRICEPERCARAT"]);
                                worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Style.Numberformat.Format = "0.00";

                                worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = Val.Val(DRowClarityDetail["AMOUNT"]);
                                worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Style.Numberformat.Format = "0.00";

                                worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = DouCaratPer;
                                worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Style.Numberformat.Format = "0.00%";

                                worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = DouAmountPer;
                                worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Style.Numberformat.Format = "0.00%";

                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternColor.SetColor(Color.FromArgb(176, 196, 222));
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(176, 196, 222));
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                                StartRow = StartRow + 1;

                            } // Size End
                            // StartColumn = IntDetailColumn;
                            worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = StrDepartment;
                            worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = DouTotalCarat;
                            worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Style.Numberformat.Format = "0.00";

                            worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = DouTotalCarat == 0 ? 0 : Math.Round(DouTotalAmount / DouTotalCarat, 2);
                            worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Style.Numberformat.Format = "0.00";

                            worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = DouTotalAmount;
                            worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Style.Numberformat.Format = "0.00";

                            worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = "100";
                            worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = "100";

                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;

                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Bold = true;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternColor.SetColor(Color.FromArgb(176, 196, 222));
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(176, 196, 222));
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                            StartColumn = StartColumn + 6;
                            StartRow = StartRow + 1;

                        } // Department End


                        // Kapan Wise Summary

                        UDRow = DTabKapan.Select("KAPANNAME='" + StrKapan + "' ");
                        if (UDRow.Length == 0)
                        {
                            continue;
                        }
                        DataTable DTabKapanSummary = UDRow.CopyToDataTable();
                        DTabKapanSummary.DefaultView.Sort = "MIXSIZESEQ";
                        DTabKapanSummary = DTabKapanSummary.DefaultView.ToTable();

                        StartColumn = 1;
                        StartRow = StartRow + 1;

                        worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = "Kapan Summ";
                        worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = "Carat";
                        worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = "$/Cts";
                        worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = "Amount";
                        worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = "Cts %";
                        worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = "Amt %";

                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Bold = true;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternColor.SetColor(Color.FromArgb(225, 150, 150));
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(225, 150, 150));
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                        DouTotalAmount = Val.Val(DTabKapanSummary.Compute("SUM(AMOUNT)", ""));
                        DouTotalCarat = Val.Val(DTabKapanSummary.Compute("SUM(CARAT)", ""));
                        IntTotalCount = DTabKapanSummary.Rows.Count;

                        DouAmountPer = 0;
                        DouCaratPer = 0;

                        StartRow = StartRow + 1;
                        EndRow = StartRow;

                        foreach (DataRow DRowClarityDetail in DTabKapanSummary.Rows)
                        {
                            DouAmountPer = DouTotalAmount == 0 ? 0 : Math.Round(Val.Val(DRowClarityDetail["AMOUNT"]) / DouTotalAmount, 2);
                            DouCaratPer = DouTotalCarat == 0 ? 0 : Math.Round(Val.Val(DRowClarityDetail["CARAT"]) / DouTotalCarat, 2);

                            worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = Val.ToString(DRowClarityDetail["MIXSIZENAME"]);
                            worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = Val.Val(DRowClarityDetail["CARAT"]);
                            worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Style.Numberformat.Format = "0.00";

                            worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = Val.Val(DRowClarityDetail["PRICEPERCARAT"]);
                            worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Style.Numberformat.Format = "0.00";

                            worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = Val.Val(DRowClarityDetail["AMOUNT"]);
                            worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Style.Numberformat.Format = "0.00";

                            worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = DouCaratPer;
                            worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Style.Numberformat.Format = "0.00%";

                            worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = DouAmountPer;
                            worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Style.Numberformat.Format = "0.00%";

                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternColor.SetColor(Color.FromArgb(225, 150, 150));
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(225, 150, 150));
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                            StartRow = StartRow + 1;

                        } // Size End
                        // StartColumn = IntDetailColumn;
                        worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = StrKapan;
                        worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = DouTotalCarat;
                        worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Style.Numberformat.Format = "0.00";

                        worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = DouTotalCarat == 0 ? 0 : Math.Round(DouTotalAmount / DouTotalCarat, 2);
                        worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Style.Numberformat.Format = "0.00";

                        worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = DouTotalAmount;
                        worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Style.Numberformat.Format = "0.00";

                        worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = "100";
                        worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = "100";

                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Name = FontName;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Size = FontSize;

                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Bold = true;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.PatternColor.SetColor(Color.FromArgb(225, 150, 150));
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(225, 150, 150));
                        worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 5].Style.Font.Color.SetColor(FontColor);

                        StartColumn = StartColumn + 6;
                        StartRow = StartRow + 1;

                    } // Kapan End

                    xlPackage.Save();

                    System.Diagnostics.Process.Start(StrFilePath, "CMD");
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }

        }

        private void GrdSummry_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            try
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    DouGCarat = 0;
                    DouGRate = 0;
                    DouGAmount = 0;

                    DouExcRate = 0;
                    DouMumbaiAmt = 0;
                }
                else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DouGCarat = DouGCarat + Val.Val(GrdSummry.GetRowCellValue(e.RowHandle, "CARAT"));
                    DouGRate = Val.Val(GrdSummry.GetRowCellValue(e.RowHandle, "PRICEPERCARAT"));
                    DouGAmount = DouGAmount + (Val.Val(GrdSummry.GetRowCellValue(e.RowHandle, "CARAT")) * DouGRate);
                    DouExcRate = Val.Val(GrdSummry.GetRowCellValue(e.RowHandle, "EXCRATE"));
                   //DouMumbaiAmt = DouMumbaiAmt + Val.Val(GrdDetSummry.GetRowCellValue(e.RowHandle, "MUMBAI"));
                }
                else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("PRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouGCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouGAmount) / Val.Val(DouGCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MUMBAI") == 0)
                    {
                        if (Val.Val(DouGCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouExcRate) * (Val.Val(DouGAmount) / Val.Val(DouGCarat)), 2);
                        else
                            e.TotalValue = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        public void Link_CreateMarginalHeaderAreaSummary(object sender, CreateAreaEventArgs e)
        {
            // ' For Report Title

            TextBrick BrickTitle = e.Graph.DrawString(BusLib.Configuration.BOConfiguration.gEmployeeProperty.COMPANYNAME, System.Drawing.Color.Navy, new RectangleF(0, 0, e.Graph.ClientPageSize.Width, 35), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitle.Font = new Font("verdana", 12, FontStyle.Bold);
            BrickTitle.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            BrickTitle.VertAlignment = DevExpress.Utils.VertAlignment.Center;

            // ' For Group 
            TextBrick BrickTitleseller = e.Graph.DrawString("KAPAN ASSORTMENT WISE SUMMARY", System.Drawing.Color.Navy, new RectangleF(0, 35, e.Graph.ClientPageSize.Width, 35), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitleseller.Font = new Font("verdana", 10, FontStyle.Bold);
            BrickTitleseller.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            BrickTitleseller.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            BrickTitleseller.ForeColor = Color.Black;

            int IntX = Convert.ToInt32(Math.Round(e.Graph.ClientPageSize.Width - 250, 0));
            TextBrick BrickTitledate = e.Graph.DrawString("Print Date : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), System.Drawing.Color.Navy, new RectangleF(IntX, 70, 250, 30), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitledate.Font = new Font("verdana", 8, FontStyle.Bold);
            BrickTitledate.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            BrickTitledate.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            BrickTitledate.ForeColor = Color.Black;

        }

        public void Link_CreateMarginalFooterAreaSummary(object sender, CreateAreaEventArgs e)
        {
            int IntX = Convert.ToInt32(Math.Round(e.Graph.ClientPageSize.Width - 100, 0));

            PageInfoBrick BrickPageNo = e.Graph.DrawPageInfo(PageInfo.NumberOfTotal, "Page {0} of {1}", System.Drawing.Color.Navy, new RectangleF(IntX, 0, 100, 15), DevExpress.XtraPrinting.BorderSide.None);
            BrickPageNo.LineAlignment = BrickAlignment.Far;
            BrickPageNo.Alignment = BrickAlignment.Far;
            // BrickPageNo.AutoWidth = true;
            BrickPageNo.Font = new Font("verdana", 8, FontStyle.Bold); ;
            BrickPageNo.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            BrickPageNo.VertAlignment = DevExpress.Utils.VertAlignment.Center;
        }

        public void Link_CreateMarginalHeaderAreaDetail(object sender, CreateAreaEventArgs e)
        {
            // ' For Report Title

            TextBrick BrickTitle = e.Graph.DrawString(BusLib.Configuration.BOConfiguration.gEmployeeProperty.COMPANYNAME, System.Drawing.Color.Navy, new RectangleF(0, 0, e.Graph.ClientPageSize.Width, 35), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitle.Font = new Font("verdana", 12, FontStyle.Bold);
            BrickTitle.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            BrickTitle.VertAlignment = DevExpress.Utils.VertAlignment.Center;

            // ' For Group 
            TextBrick BrickTitleseller = e.Graph.DrawString("KAPAN ASSORTMENT WISE Detail", System.Drawing.Color.Navy, new RectangleF(0, 35, e.Graph.ClientPageSize.Width, 35), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitleseller.Font = new Font("verdana", 10, FontStyle.Bold);
            BrickTitleseller.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            BrickTitleseller.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            BrickTitleseller.ForeColor = Color.Black;

            int IntX = Convert.ToInt32(Math.Round(e.Graph.ClientPageSize.Width - 250, 0));
            TextBrick BrickTitledate = e.Graph.DrawString("Print Date : " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), System.Drawing.Color.Navy, new RectangleF(IntX, 70, 250, 30), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitledate.Font = new Font("verdana", 8, FontStyle.Bold);
            BrickTitledate.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            BrickTitledate.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            BrickTitledate.ForeColor = Color.Black;

        }

        public void Link_CreateMarginalFooterAreaDetail(object sender, CreateAreaEventArgs e)
        {
            int IntX = Convert.ToInt32(Math.Round(e.Graph.ClientPageSize.Width - 100, 0));

            PageInfoBrick BrickPageNo = e.Graph.DrawPageInfo(PageInfo.NumberOfTotal, "Page {0} of {1}", System.Drawing.Color.Navy, new RectangleF(IntX, 0, 100, 15), DevExpress.XtraPrinting.BorderSide.None);
            BrickPageNo.LineAlignment = BrickAlignment.Far;
            BrickPageNo.Alignment = BrickAlignment.Far;
            // BrickPageNo.AutoWidth = true;
            BrickPageNo.Font = new Font("verdana", 8, FontStyle.Bold); ;
            BrickPageNo.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            BrickPageNo.VertAlignment = DevExpress.Utils.VertAlignment.Center;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                try
                {

                    DevExpress.XtraPrinting.PrintingSystem PrintSystem = new DevExpress.XtraPrinting.PrintingSystem();

                    PrintableComponentLink link = new PrintableComponentLink(PrintSystem);

                    string Str = txtSearchKapanName.Text.Trim().Length == 0 ? "All Kapan's" : txtSearchKapanName.Text;

                    link.Component = MainGrdSummry;
                    link.Landscape = true;

                    link.Margins.Left = 10;
                    link.Margins.Right = 10;
                    link.Margins.Bottom = 40;
                    link.Margins.Top = 130;

                    link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderAreaSummary);
                    link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterAreaSummary);
                    link.CreateDocument();
                    link.ShowPreview();
                    link.PrintDlg();


                }
                catch (Exception ex)
                {
                    Global.Message(ex.Message.ToString());
                }
            }
            else
            {
                try
                {

                    DevExpress.XtraPrinting.PrintingSystem PrintSystem = new DevExpress.XtraPrinting.PrintingSystem();

                    PrintableComponentLink link = new PrintableComponentLink(PrintSystem);

                    string Str = txtSearchKapanName.Text.Trim().Length == 0 ? "All Kapan's" : txtSearchKapanName.Text;

                    link.Component = MainGrid;
                    link.Landscape = true;

                    link.Margins.Left = 10;
                    link.Margins.Right = 10;
                    link.Margins.Bottom = 40;
                    link.Margins.Top = 130;

                    link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderAreaDetail);
                    link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterAreaDetail);
                    link.CreateDocument();
                    link.ShowPreview();
                    link.PrintDlg();


                }
                catch (Exception ex)
                {
                    Global.Message(ex.Message.ToString());
                }
            }
        }

        private void GrdSummry_CustomDrawRowFooterCell(object sender, FooterCellCustomDrawEventArgs e)
        {
            /*
             * DataRow dr = GrdSummry.GetDataRow(e.RowHandle);

            if (Val.ToString(dr["KAPANNAME"]) == "182")
            {
                GridSummaryItem item = e.Column.SummaryItem;
                e.Column.Summary.Remove(item);
                e.Handled = true;
            }
            else
            {
                int dx = e.Bounds.Height;
                Brush brush = e.Cache.GetGradientBrush(e.Bounds, Color.Wheat, Color.FloralWhite, System.Drawing.Drawing2D.LinearGradientMode.Vertical);
                Rectangle r = e.Bounds;
                //Draw a 3D border
                BorderPainter painter = BorderHelper.GetPainter(DevExpress.XtraEditors.Controls.BorderStyles.Style3D);
                AppearanceObject borderAppearance = new AppearanceObject(e.Appearance);
                borderAppearance.BorderColor = Color.DarkGray;
                painter.DrawObject(new BorderObjectInfoArgs(e.Cache, borderAppearance, r));
                //Fill the inner region of the cell
                r.Inflate(-1, -1);
                e.Cache.FillRectangle(brush, r);
                //Draw a summary value
                r.Inflate(-2, 0);
                e.Appearance.DrawString(e.Cache, e.Info.DisplayText, r);
                //Prevent default drawing of the cell
                e.Handled = true;
            }
            */
        }
    }
}
