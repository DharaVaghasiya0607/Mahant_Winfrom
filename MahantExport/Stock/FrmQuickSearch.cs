using MahantExport.Stock;
using MahantExport.Stock;
using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraPrinting;
using DevExpress.XtraPrintingLinks;
using Google.API.Translate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MahantExport.Utility;

namespace MahantExport.Stock
{
    public partial class FrmQuickSearch : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        DataTable DtabColClWise = new DataTable();

        DataTable DtabStock = new DataTable();
        DataTable DtabSale = new DataTable();
        DataTable DtabStockVSSale = new DataTable();


        DataTable DtabParam = new DataTable();

        string mStrStockStatus = "";
        bool IsExportPrint = false;

        #region Property Settings

        public FrmQuickSearch()
        {
            InitializeComponent();
        }


        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            this.Show();

            FillControl();

            DtabParam = new BOMST_Parameter().GetParameterData();
            MainGrdStock.DataSource = DtabColClWise;
            MainGrdStock.RefreshDataSource();

            GrdDetail.FocusedRowHandle = 1;
            GrdDetail.FocusedColumn = GrdDetail.VisibleColumns[1];
            GrdDetail.Focus();
        }


        public void ClearFilters()
        {
            Global.FILTERSHAPE_ID = string.Empty;
            Global.FILTERSIZE_ID = string.Empty;
            Global.FILTERCOLOR_ID = string.Empty;
            Global.FILTERCLARITY_ID = string.Empty;
            Global.FILTERCUT_ID = string.Empty;
            Global.FILTERPOL_ID = string.Empty;
            Global.FILTERSYM_ID = string.Empty;
            Global.FILTERFL_ID = string.Empty;
            Global.FILTERCOLORSHADE_ID = string.Empty;
            Global.FILTERMILKY_ID = string.Empty;
            Global.FILTERLAB_ID = string.Empty;
            Global.FILTERWEBSTATUS = string.Empty;
            Global.FILTERTABLEBLACKINC_ID = string.Empty;
            Global.FILTERSIDEBLACKINC_ID = string.Empty;
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            //ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjStock);
            ObjFormEvent.ObjToDisposeList.Add(Val);

        }

        public void FillControl()
        {

            ////Shape
            //ChkCmbShape.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SHAPE);
            //ChkCmbShape.Properties.DisplayMember = "SHAPENAME";
            //ChkCmbShape.Properties.ValueMember = "SHAPE_ID";

            ////SIZE
            //ChkCmbSize.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SIZEGROUP);
            //ChkCmbSize.Properties.DisplayMember = "SIZENAME";
            //ChkCmbSize.Properties.ValueMember = "SIZE_ID";

            ////CUT
            //ChkCmbCut.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CUT);
            //ChkCmbCut.Properties.DisplayMember = "CUTNAME";
            //ChkCmbCut.Properties.ValueMember = "CUT_ID";

            ////POL
            //ChkCmbPol.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_POL);
            //ChkCmbPol.Properties.DisplayMember = "POLNAME";
            //ChkCmbPol.Properties.ValueMember = "POL_ID";

            ////SYM
            //ChkCmbSym.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SYM);
            //ChkCmbSym.Properties.DisplayMember = "SYMNAME";
            //ChkCmbSym.Properties.ValueMember = "SYM_ID";

            ////FL
            //ChkCmbFL.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_FL);
            //ChkCmbFL.Properties.DisplayMember = "FLNAME";
            //ChkCmbFL.Properties.ValueMember = "FL_ID";


        }
        #endregion

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            IsExportPrint = true;
            SaveFileDialog svDialog = new SaveFileDialog();
            svDialog.DefaultExt = ".xlsx";
            svDialog.Title = "Export to Excel";
            svDialog.FileName = "Quick Search Stock";
            svDialog.Filter = "Excel files 97-2003 (*.xls)|*.xls|Excel files 2007(*.xlsx)|*.xlsx|All files (*.*)|*.*";
            if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
            {
                PrintableComponentLinkBase link = new PrintableComponentLinkBase()
                {
                    PrintingSystemBase = new PrintingSystemBase(),
                    Component = MainGrdStock,
                    //Landscape = true,
                    PaperKind = PaperKind.A4,
                    Margins = new System.Drawing.Printing.Margins(20, 25, 20, 20)
                };

                DevExpress.XtraPrinting.XlsExportOptions a = new DevExpress.XtraPrinting.XlsExportOptions();
                a.Suppress256ColumnsWarning = true;
                a.Suppress65536RowsWarning = true;

                //a.RawDataMode = false;
                //a.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Value;

                link.ExportToXls(svDialog.FileName, a);

                if (Global.Confirm("Do You Want To Open [" + svDialog.FileName + ".xlsx] ?") == System.Windows.Forms.DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start(svDialog.FileName, "CMD");
                }
            }
            IsExportPrint = false;



            //Global.ExcelExport("Current Stock Color Clarity Wise", GrdDetail);
        }

        private void BtnBestFit_Click(object sender, EventArgs e)
        {
            GrdDetail.BestFitColumns();
        }


        private void GrdDetail_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }

                if (e.Clicks == 2)
                {
                    this.Cursor = Cursors.WaitCursor;
                    DataRow Dr = GrdDetail.GetFocusedDataRow();

                    string StrCol = Val.ToString(GrdDetail.FocusedColumn.FieldName);

                    string StrClarityName = "";

                    string StrSizeName = "";
                    string StrFromSize = Val.ToString(Dr["FROMSIZE"]);
                    string StrToSize = Val.ToString(Dr["TOSIZE"]);
                    string StrColorName = Val.ToString(Dr["COL1"]);
                    string StrShapName = "ro";// Regex.Replace(Val.ToString(ChkCmbShape.EditValue), @"\s", ""); ;

                    Int32 IntColor_Id = 0, IntClarity_Id = 0;

                    if (Val.Val(GrdDetail.GetFocusedValue()) == 0)
                    {
                        this.Cursor = Cursors.Default;
                        return;
                    }

                    StrSizeName = StrFromSize + "-" + StrToSize;


                    if ((Val.Val(StrFromSize) != 0) && (Val.Val(StrToSize) == 0))
                    {
                        StrSizeName = ">" + Val.ToString(Val.Val(StrFromSize) - 0.01);
                    }

                    DataRow[] drow = DtabColClWise.Select("COL1 = '" + StrSizeName + "' AND ISCOLOR = 1");

                    if (drow != null && drow.Length > 0)
                        StrClarityName = Val.ToString(drow[0][StrCol]);

                    var drCol = (from DrPara in DtabParam.AsEnumerable()
                                 where Val.ToString(DrPara["PARANAME"]) == StrColorName && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "COLOR" && Val.ToString(DrPara["PARAGROUP"]).ToUpper() == "SINGLE"
                                 select DrPara);

                    IntColor_Id = drCol.Count() > 0 ? Val.ToInt32(drCol.FirstOrDefault()["PARA_ID"]) : 0;

                    var drClarity = (from DrPara in DtabParam.AsEnumerable()
                                     where Val.ToString(DrPara["PARANAME"]) == StrClarityName && Val.ToString(DrPara["PARATYPE"]).ToUpper() == "CLARITY" && Val.ToString(DrPara["PARAGROUP"]).ToUpper() == "SINGLE"
                                     select DrPara);

                    IntClarity_Id = drClarity.Count() > 0 ? Val.ToInt32(drClarity.FirstOrDefault()["PARA_ID"]) : 0;


                   
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        public static void DrawThickFocusRectangle(Graphics g, Rectangle r)
        {
            Brush hb = Brushes.Black;
            g.FillRectangle(hb, new Rectangle(r.X, r.Y, 2, r.Height - 2)); // left
            g.FillRectangle(hb, new Rectangle(r.X, r.Y, r.Width - 2, 2));  // top
            g.FillRectangle(hb, new Rectangle(r.Right - 2, r.Y, 2, r.Height - 2)); // right
            g.FillRectangle(hb, new Rectangle(r.X, r.Bottom - 2, r.Width, 2)); // bottom
        }

        private void GrdDetail_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }

                int IntIsColor = Val.ToInt32(GrdDetail.GetRowCellValue(e.RowHandle, "ISCOLOR"));

                string StrCol1 = Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "COL1"));

                string StrCol2 = Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle - 1, "COL1"));

                if (!Val.ToString(StrCol1).Trim().Equals(string.Empty))
                {
                    GridViewInfo vi = GrdDetail.GetViewInfo() as GridViewInfo;
                    Point p1 = new Point(vi.ColumnsInfo[e.Column].Bounds.Right - 1, vi.ViewRects.Rows.Y);
                    Point p2 = new Point(vi.ColumnsInfo[e.Column].Bounds.Right - 1, vi.ViewRects.Rows.Bottom);

                    Point p3 = new Point(vi.ColumnsInfo[e.Column].Bounds.Right, vi.ViewRects.Rows.Y);
                    Point p4 = new Point(vi.ColumnsInfo[e.Column].Bounds.Right, vi.ViewRects.Rows.Bottom);

                    Pen p = new Pen(Color.Black);
                    e.Graphics.DrawLine(p, p1, p2);

                    if (IntIsColor == 1)
                    {
                        e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(238, 234, 221)), e.Bounds);

                        // Color Is Fixed like Solid then focus is not set in those columns
                        // e.Cache.FillRectangle(new SolidBrush(Color.FromArgb(238, 234, 221)), e.Bounds);
                        //e.Cache.DrawString(e.DisplayText, e.Appearance.Font, new SolidBrush(Color.Black), e.Bounds, e.Appearance.GetStringFormat());
                        //e.Handled = true;
                    }
                    if (e.Column.FieldName == "COL1")
                    {
                        Point p5 = new Point(vi.ColumnsInfo[e.Column].Bounds.Right, vi.ViewRects.Rows.Y);
                        Point p6 = new Point(vi.ColumnsInfo[e.Column].Bounds.Right, vi.ViewRects.Rows.Bottom);
                        Pen pen = new Pen(Color.Black);
                        e.Graphics.DrawLine(pen, p5, p6);
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            IsExportPrint = true;
            DevExpress.XtraPrinting.PrintingSystem PrintSystem = new DevExpress.XtraPrinting.PrintingSystem();  

            PrinterSettingsUsing pst = new PrinterSettingsUsing();


            PrintSystem.PageSettings.AssignDefaultPrinterSettings(pst);

            //Lesson2 link = new Lesson2(PrintSystem);
            PrintableComponentLink link = new PrintableComponentLink(PrintSystem);

            GrdDetail.OptionsPrint.AutoWidth = true;
            GrdDetail.OptionsPrint.UsePrintStyles = true;

            link.Component = MainGrdStock;
            link.Landscape = true;
            link.PaperKind = System.Drawing.Printing.PaperKind.A4;

            link.Margins.Left = 40;
            link.Margins.Right = 40;
            link.Margins.Bottom = 40;
            link.Margins.Top = 80;

            link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);
            link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);

            link.CreateDocument();

            link.ShowPreview();
            link.PrintDlg();
            IsExportPrint = false;
        }

        public void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            // ' For Report Title
            TextBrick BrickTitle = e.Graph.DrawString(Val.ToString(lblTitle.Text), System.Drawing.SystemColors.ActiveCaptionText, new RectangleF(0, 0, e.Graph.ClientPageSize.Width, 20), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitle.Font = new Font("Verdana", 12, FontStyle.Bold);
            BrickTitle.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            BrickTitle.VertAlignment = DevExpress.Utils.VertAlignment.Center;


            int IntX = Convert.ToInt32(Math.Round(e.Graph.ClientPageSize.Width - 400, 0));
            TextBrick BrickTitledate = e.Graph.DrawString("Print Date :- " + System.DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), System.Drawing.Color.Navy, new RectangleF(IntX, 25, 400, 18), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitledate.Font = new Font("Verdana", 11, FontStyle.Bold);
            BrickTitledate.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            BrickTitledate.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            BrickTitledate.ForeColor = Color.Black;
        }

        public void Link_CreateMarginalFooterArea(object sender, CreateAreaEventArgs e)
        {
            int IntX = Convert.ToInt32(Math.Round(e.Graph.ClientPageSize.Width - 100, 0));

            PageInfoBrick BrickPageNo = e.Graph.DrawPageInfo(PageInfo.NumberOfTotal, "Page {0} of {1}", System.Drawing.Color.Navy, new RectangleF(IntX, 0, 100, 15), DevExpress.XtraPrinting.BorderSide.None);
            BrickPageNo.LineAlignment = BrickAlignment.Far;
            BrickPageNo.Alignment = BrickAlignment.Far;
            // BrickPageNo.AutoWidth = true;
            BrickPageNo.Font = new Font("Verdana", 10, FontStyle.Bold);
            BrickPageNo.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            BrickPageNo.VertAlignment = DevExpress.Utils.VertAlignment.Center;
        }

        private void GrdDetail_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000")
                {
                    e.DisplayText = String.Empty;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }


        private void BtnShow_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string StrOpe = "Pcs";
                string StrReporytType = "Stock";

                string StrShape = "";
                string StrSize = "";
                string StrCut = "";
                string StrPol = "";
                string StrSym = "";
                string StrFL = "";

                string StrColor = "";
                string StrClarity = "";
                string StrMilky = "";
                string StrLab = "";
                string StrStatus = "";
                string StrColorShade = "";
                string StrTableBlackInc = "";
                string StrSideBlackInc = "";

                string StrFromDate = null;
                string StrToDate = null;

                if (RbtPcs.Checked)
                {
                    StrOpe = "Pcs";
                }
                else if (RbtCarat.Checked)
                {
                    StrOpe = "Carat";
                }
                else if (RbtDiscount.Checked)
                {
                    StrOpe = "Discount";
                }
                else if (RbtAmount.Checked)
                {
                    StrOpe = "Amount";
                }
                else if (RbtPricePerCarat.Checked)
                {
                    StrOpe = "Rate";
                }

                if (RbtStock.Checked)
                {
                    StrReporytType = "Stock";
                    StrShape = Global.FILTERSHAPE_ID;//Gunjan:16/08/2023                                                          
                    StrSize = Global.FILTERSIZE_ID;
                    StrCut = Global.FILTERCUT_ID;
                    StrPol = Global.FILTERPOL_ID;
                    StrSym = Global.FILTERSYM_ID;
                    StrColor = Global.FILTERCOLOR_ID;
                    StrClarity = Global.FILTERCLARITY_ID;
                    StrMilky = Global.FILTERMILKY_ID;
                    StrLab = Global.FILTERLAB_ID;
                    StrStatus = Global.FILTERWEBSTATUS;
                    StrColorShade = Global.FILTERCOLORSHADE_ID;
                    StrTableBlackInc = Global.FILTERTABLEBLACKINC_ID;
                    StrFL = Global.FILTERFL_ID;
                    StrSideBlackInc = Global.FILTERSIDEBLACKINC_ID;//Ens As gunjan
                }
                else if (RbtSale.Checked)
                {
                    StrReporytType = "Sale";
                    StrShape = Global.FILTERSHAPE_ID;//Gunjan:16/08/2023
                    StrSize = Global.FILTERSIZE_ID;
                    StrCut = Global.FILTERCUT_ID;
                    StrPol = Global.FILTERPOL_ID;
                    StrSym = Global.FILTERSYM_ID;
                    StrColor = Global.FILTERCOLOR_ID;
                    StrClarity = Global.FILTERCLARITY_ID;
                    StrMilky = Global.FILTERMILKY_ID;
                    StrLab = Global.FILTERLAB_ID;
                    StrStatus = Global.FILTERWEBSTATUS;
                    StrColorShade = Global.FILTERCOLORSHADE_ID;
                    StrTableBlackInc = Global.FILTERTABLEBLACKINC_ID;
                    StrFL = Global.FILTERFL_ID;
                    StrSideBlackInc = Global.FILTERSIDEBLACKINC_ID;//Ens As gunjan
                }
                else if (RbtSaleStock.Checked)
                {
                    StrReporytType = "StockSale";
                }
                else if (RbtStockAge.Checked)
                {
                    StrReporytType = "StockAge";
                }
                else if (RbtSaleAge.Checked)
                {
                    StrReporytType = "SaleAge";
                }
                if (DTPFromDate.Checked == true)
                {
                    StrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
                }
                if (DTPToDate.Checked == true)
                {
                    StrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());
                }
                                                    
                GrdDetail.BeginUpdate();

                DataSet DsData = ObjStock.MISHeatMapGetData(StrReporytType, StrOpe, StrShape, StrSize, StrCut, StrPol, StrSym, StrFL, StrFromDate, StrToDate, StrColor, StrClarity, StrMilky, StrLab, StrStatus, StrColorShade, StrTableBlackInc, StrSideBlackInc);
                DtabStock = DsData.Tables[0];
                if (DsData.Tables.Count > 1)
                {
                    DtabSale = DsData.Tables[1];
                    DtabStockVSSale = DsData.Tables[2];
                }

                MainGrdStock.DataSource = DtabStock;
                MainGrdDetail2.DataSource = DtabSale;
                MainGrdDetail3.DataSource = DtabStockVSSale;


                GrdDetail.EndUpdate();
                MainGrdStock.Refresh();
                ClearFilters();
                this.Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }


        private void ChkBtnStock_CheckedChanged(object sender, EventArgs e)
        {
            //if (ChkBtnStock.Checked == true)
            //{
            //    ChkBtnSale.Checked = false;
            //    ChkBtnStockVSSale.Checked = false;
            //}
            //else if (ChkBtnSale.Checked == true)
            //{
            //    ChkBtnStock.Checked = false;
            //    ChkBtnStockVSSale.Checked = false;
            //}
            //else if (ChkBtnStockVSSale.Checked == true)
            //{
            //    ChkBtnSale.Checked = false;
            //    ChkBtnStock.Checked = false;
            //}

            //if (ChkBtnPcs.Checked == true)
            //{
            //    ChkBtnCarat.Checked = false;
            //    ChkBtnDiscount.Checked = false;
            //    ChkBtnRate.Checked = false;
            //    ChkBtnAmount.Checked = false;
            //}
            //else if (ChkBtnCarat.Checked == true)
            //{
            //    ChkBtnPcs.Checked = false;
            //    ChkBtnDiscount.Checked = false;
            //    ChkBtnRate.Checked = false;
            //    ChkBtnAmount.Checked = false;
            //}
            //else if (ChkBtnRate.Checked == true)
            //{
            //    ChkBtnPcs.Checked = false;
            //    ChkBtnDiscount.Checked = false;
            //    ChkBtnCarat.Checked = false;
            //    ChkBtnAmount.Checked = false;
            //}
            //else if (ChkBtnDiscount.Checked == true)
            //{
            //    ChkBtnPcs.Checked = false;
            //    ChkBtnRate.Checked = false;
            //    ChkBtnCarat.Checked = false;
            //    ChkBtnAmount.Checked = false;
            //}
            //else if (ChkBtnAmount.Checked == true)
            //{
            //    ChkBtnPcs.Checked = false;
            //    ChkBtnRate.Checked = false;
            //    ChkBtnCarat.Checked = false;
            //    ChkBtnDiscount.Checked = false;
            //}
            //BtnShow_Click(null, null);
        }

        private void BtnFilter_Click(object sender, EventArgs e)//Add By Gunjan:17/07/2023
        {
            FrmSearchFilterNew FrmSearchFilterNew = new FrmSearchFilterNew();
            FrmSearchFilterNew.ShowDialog();
            BtnShow.Focus();
        }//End As Gunjan

        private void RbtStock_CheckedChanged(object sender, EventArgs e)
        {
            if (RbtStock.Checked || RbtSale.Checked)
            {
                BtnFilter.Visible = true;
            }
            else
            {
                BtnFilter.Visible = false;
            }
        }

        private void RbtSale_CheckedChanged(object sender, EventArgs e)
        {
            if (RbtStock.Checked || RbtSale.Checked)
            {
                BtnFilter.Visible = true;
            }
            else
            {
                BtnFilter.Visible = false;
            }
        }

        private void RbtSaleStock_CheckedChanged(object sender, EventArgs e)
        {
            if (RbtStock.Checked || RbtSale.Checked)
            {
                BtnFilter.Visible = true;
            }
            else
            {
                BtnFilter.Visible = false;
            }
        }

        private void RbtStockAge_CheckedChanged(object sender, EventArgs e)
        {
            if (RbtStock.Checked || RbtSale.Checked)
            {
                BtnFilter.Visible = true;
            }
            else
            {
                BtnFilter.Visible = false;
            }
        }

        private void RbtSaleAge_CheckedChanged(object sender, EventArgs e)
        {
            if (RbtStock.Checked || RbtSale.Checked)
            {
                BtnFilter.Visible = true;
            }
            else
            {
                BtnFilter.Visible = false;
            }
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void MainGrdDetail2_Click(object sender, EventArgs e)
        {

        }

        private void GrdDetail2_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try 
            {
                if (e.RowHandle < 0)
                {
                    return;
                }


                if ((Val.ToInt(GrdDetail2.GetRowCellValue(e.RowHandle, "SEQNO")) == 0 ||
                    Val.ToInt(GrdDetail2.GetRowCellValue(e.RowHandle, "SEQNO")) == 99) &&

                    Val.ToString(GrdDetail2.GetRowCellValue(e.RowHandle, "SIZE")) != ""
                    )
                {

                    e.Appearance.BackColor = Color.DimGray;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                }

                else if (e.Column.FieldName == "FL" ||
                    e.Column.FieldName == "IF" ||
                    e.Column.FieldName == "VVS1" ||
                    e.Column.FieldName == "VVS2" ||
                    e.Column.FieldName == "VS1" ||
                    e.Column.FieldName == "VS2" ||
                    e.Column.FieldName == "SI1" ||
                    e.Column.FieldName == "SI2" ||
                    e.Column.FieldName == "I1" ||
                    e.Column.FieldName == "I2" ||
                    e.Column.FieldName == "I3" ||
                    e.Column.FieldName == "OTH"

                    )
                {
                    string[] s = GrdDetail2.GetRowCellValue(e.RowHandle, e.Column.FieldName + "RGB").ToString().Split(',');
                    if (s.Length == 3)
                    {
                        e.Appearance.BackColor = Color.FromArgb(Val.ToInt(s[0]), Val.ToInt(s[1]), Val.ToInt(s[2]));
                    }
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDetail3_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }


                if ((Val.ToInt(GrdDetail3.GetRowCellValue(e.RowHandle, "SEQNO")) == 0 ||
                    Val.ToInt(GrdDetail3.GetRowCellValue(e.RowHandle, "SEQNO")) == 99) &&

                    Val.ToString(GrdDetail3.GetRowCellValue(e.RowHandle, "SIZE")) != ""
                    )
                {

                    e.Appearance.BackColor = Color.DimGray;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                }

                else if (e.Column.FieldName == "FL" ||
                    e.Column.FieldName == "IF" ||
                    e.Column.FieldName == "VVS1" ||
                    e.Column.FieldName == "VVS2" ||
                    e.Column.FieldName == "VS1" ||
                    e.Column.FieldName == "VS2" ||
                    e.Column.FieldName == "SI1" ||
                    e.Column.FieldName == "SI2" ||
                    e.Column.FieldName == "I1" ||
                    e.Column.FieldName == "I2" ||
                    e.Column.FieldName == "I3" ||
                    e.Column.FieldName == "OTH"

                    )
                {
                    string[] s = GrdDetail3.GetRowCellValue(e.RowHandle, e.Column.FieldName + "RGB").ToString().Split(',');
                    if (s.Length == 3)
                    {
                        e.Appearance.BackColor = Color.FromArgb(Val.ToInt(s[0]), Val.ToInt(s[1]), Val.ToInt(s[2]));
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDetail_CustomColumnDisplayText_1(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {

            try
            {
                if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000")
                {
                    e.DisplayText = String.Empty;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDetail2_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {

            try
            {
                if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000")
                {
                    e.DisplayText = String.Empty;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDetail3_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {

            try
            {
                if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000")
                {
                    e.DisplayText = String.Empty;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }

        }

        private void GrdDetail_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }


                if ((Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "SEQNO")) == 0 ||
                    Val.ToInt(GrdDetail.GetRowCellValue(e.RowHandle, "SEQNO")) == 99) &&

                    Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "SIZE")) != ""
                    )
                {

                    e.Appearance.BackColor = Color.DimGray;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                }

                else if (e.Column.FieldName == "FL" ||
                    e.Column.FieldName == "IF" ||
                    e.Column.FieldName == "VVS1" ||
                    e.Column.FieldName == "VVS2" ||
                    e.Column.FieldName == "VS1" ||
                    e.Column.FieldName == "VS2" ||
                    e.Column.FieldName == "SI1" ||
                    e.Column.FieldName == "SI2" ||
                    e.Column.FieldName == "I1" ||
                    e.Column.FieldName == "I2" ||
                    e.Column.FieldName == "I3" ||
                    e.Column.FieldName == "OTH"

                    )
                {
                    string[] s = GrdDetail.GetRowCellValue(e.RowHandle, e.Column.FieldName + "RGB").ToString().Split(',');
                    if (s.Length == 3)
                    {
                        e.Appearance.BackColor = Color.FromArgb(Val.ToInt(s[0]), Val.ToInt(s[1]), Val.ToInt(s[2]));
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
    }
}
              