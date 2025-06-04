using BusLib;
using BusLib.Configuration;
using BusLib.Transaction;
using DevExpress.XtraPrinting;
using Google.API.Translate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusLib.TableName;
using MahantExport;

namespace MahantExport.Stock
{
    public partial class FrmRoughPurchaseView : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_RoughPurchase ObjMast = new BOTRN_RoughPurchase();
        DataTable DtabSalary = new DataTable();
        DataTable DTabPurchase = new DataTable();

        #region Property Settings

        public FrmRoughPurchaseView()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            //MainGrid.DataSource = DtabSalary;
            //MainGrid.RefreshDataSource();
            string currentMonth = DateTime.Now.Month.ToString();
            string currentYear = DateTime.Now.Year.ToString();
            cmbBillType.SelectedIndex = 0;
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
            ObjFormEvent.ObjToDisposeList.Add(ObjMast);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        #endregion


        private void BtnExport_Click(object sender, EventArgs e)
        {
            DevExpress.XtraPrinting.PrintingSystem PrintSystem = new DevExpress.XtraPrinting.PrintingSystem();

            PrinterSettingsUsing pst = new PrinterSettingsUsing();

            PrintSystem.PageSettings.AssignDefaultPrinterSettings(pst);

            //Lesson2 link = new Lesson2(PrintSystem);
            PrintableComponentLink link = new PrintableComponentLink(PrintSystem);

            GrdDet.OptionsPrint.AutoWidth = true;
            GrdDet.OptionsPrint.UsePrintStyles = true;

            link.Component = MainGrid;
            link.Landscape = true;
            link.PaperKind = System.Drawing.Printing.PaperKind.A4;

            link.Margins.Left = 40;
            link.Margins.Right = 40;
            link.Margins.Bottom = 40;
            link.Margins.Top = 130;

            link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);
            link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);

            link.CreateDocument();

            link.ShowPreview();
            link.PrintDlg();
        }

        public void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            // ' For Report Title

            TextBrick BrickTitle = e.Graph.DrawString("Item Group List", System.Drawing.Color.Navy, new RectangleF(0, 0, e.Graph.ClientPageSize.Width, 20), DevExpress.XtraPrinting.BorderSide.None);
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
            BrickPageNo.Font = new Font("Verdana", 11, FontStyle.Bold); ;
            BrickPageNo.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            BrickPageNo.VertAlignment = DevExpress.Utils.VertAlignment.Center;
        }


        private void FrmPurchaseLiveStock_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                BtnSearch_Click(null, null);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            DTabPurchase = ObjMast.GetDataForPurchaseLiveStock(ChkDisplayAllLot.Checked, cmbBillType.Text);
            MainGrid.DataSource = DTabPurchase;

            GrdDet.FocusedRowHandle = 0;
            GrdDet.FocusedColumn = GrdDet.VisibleColumns[0];
            GrdDet.BestFitMaxRowCount = 200;
            GrdDet.BestFitColumns();
            GrdDet.Focus();
            MainGrid.Refresh();
            this.Cursor = Cursors.Default;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnAutoFit_Click(object sender, EventArgs e)
        {
            GrdDet.BestFitColumns();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("PurchaseLiveStock", GrdDet);
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            BtnSearch_Click(null, null);
        }

        private void GrdDet_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Clicks == 2 && e.Column.FieldName == "JANGEDNOSTR")
            {
                FrmRoughPurchaseEntryNew FrmRoughPurchaseEntry = new FrmRoughPurchaseEntryNew();
                FrmRoughPurchaseEntry.MdiParent = Global.gMainRef;
                FrmRoughPurchaseEntry.FormClosing += new FormClosingEventHandler(FrmRoughPurchaseEntry_FormClosing);
                FrmRoughPurchaseEntry.ShowForm(Val.ToString(GrdDet.GetRowCellValue(e.RowHandle, "MEMO_ID")), "ALL");
            }
        }
        private void FrmRoughPurchaseEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            BtnSearch.PerformClick();
        }

        private void lblSaveLayout_Click(object sender, EventArgs e)
        {
            Stream str = new System.IO.MemoryStream();
            GrdDet.SaveLayoutToStream(str);
            str.Seek(0, System.IO.SeekOrigin.Begin);
            StreamReader reader = new StreamReader(str);
            string text = reader.ReadToEnd();

            int IntRes = new BOTRN_StockUpload().SaveGridLayout(this.Name, GrdDet.Name, text);

            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Saved");
            }
        }

        private void lblDefaultLayout_Click(object sender, EventArgs e)
        {
            int IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdDet.Name);
            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Deleted");
            }
        }

        private void FrmRoughPurchaseView_Load(object sender, EventArgs e)
        {
            string Str = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdDet.Name);
            if (Str != "")
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                MemoryStream stream = new MemoryStream(byteArray);
                GrdDet.RestoreLayoutFromStream(stream);
            }
        }
    }
}
