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
using BusLib.Account;
using MahantExport.Utility;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;

namespace MahantExport.Stock
{
    public partial class FrmSaleInvoiceView : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_RoughPurchase ObjMast = new BOTRN_RoughPurchase();
        BOLedgerTransaction objLedgerTrn = new BOLedgerTransaction();
        DataTable DtabSalary = new DataTable();
        DataTable DTabPurchase = new DataTable();
        DataTable DTabSummary = new DataTable();
        DataTable DTabDetail = new DataTable();
        DataTable DTabDetailParcel = new DataTable();
        BODevGridSelection ObjGridSelection;
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();

        Stream strOrigin = new System.IO.MemoryStream();

        #region Property Settings

        public FrmSaleInvoiceView()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            GrdSummary.SaveLayoutToStream(strOrigin);

            string currentMonth = DateTime.Now.Month.ToString();
            string currentYear = DateTime.Now.Year.ToString();

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

            GrdSummary.OptionsPrint.AutoWidth = true;
            GrdSummary.OptionsPrint.UsePrintStyles = true;

            link.Component = MainGrdSummary;
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
                //BtnSearch_Click(null, null);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            DataSet DS = ObjMast.GetDataForSaleInvoiceList(9, "ALL", "", "SINGLE",cmbBillType.Text,Val.SqlDate(DTPFormDate.Text),Val.SqlDate(DTPToDate.Text));
            DTabSummary = DS.Tables[0];
            DTabDetail = DS.Tables[1];
            DTabDetailParcel = DS.Tables[2];
            MainGrdSummary.DataSource = DTabSummary;
            MainGridDetail.DataSource = DTabDetail;
            MainGridDetailParcel.DataSource = DTabDetailParcel;
            MainGrdSummary.Refresh();
            MainGridDetail.Refresh();
            MainGridDetailParcel.Refresh();
                        
            if (MainGrdSummary.RepositoryItems.Count == 6)
            {
                ObjGridSelection = new BODevGridSelection();
                ObjGridSelection.View = GrdSummary;
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            }
            else
            {
                ObjGridSelection.ClearSelection();
            }
            GrdSummary.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;
            if (ObjGridSelection != null)
            {
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            }

            cmbBillType_SelectedIndexChanged(null, null);
            string Str = new BOTRN_StockUpload().GetGridAccLayout(this.Name, GrdSummary.Name, cmbBillType.Text);
            if (Str != "")
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                MemoryStream stream = new MemoryStream(byteArray);
                GrdSummary.RestoreLayoutFromStream(stream);
            }
            else
            {
                try
                {
                    Stream strOrg = new System.IO.MemoryStream();
                    strOrg = strOrigin;
                    strOrg.Seek(0, System.IO.SeekOrigin.Begin);
                    GrdSummary.RestoreLayoutFromStream(strOrg);
                }
                catch (Exception ex)
                {
                    Global.MessageError(ex.Message.ToString());
                }
            }
            
            this.Cursor = Cursors.Default;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        
        private void BtnAutoFit_Click(object sender, EventArgs e)
        {
            GrdSummary.BestFitColumns();
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("PurchaseLiveStock", GrdSummary);
        }

        private void Form_Closing(object sender, FormClosingEventArgs e)
        {
            //BtnSearch_Click(null, null);
        }

        private void GrdSummary_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Clicks == 2 && Val.ToString(GrdSummary.GetFocusedRowCellValue("ORDERJANGEDNO")) == "")
            {
                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                FrmMemoEntry.ShowForm(Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "MEMO_ID")), "SALEINVOICEENTRY", 0);
                //FrmMemoEntry.ShowForm(Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "MEMO_ID")), "ALL");
            }
            else if (e.Clicks == 2 && Val.ToString(GrdSummary.GetFocusedRowCellValue("ORDERJANGEDNO")) != "")
            {
               //DataTable DtInvDetail  = DTabDetail.Select("MEMO_ID =" Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "MEMO_ID"))");
                string MEMOID = Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "MEMO_ID"));
                DataTable DtInvDetail = DTabDetail.Select("MEMO_ID='" + MEMOID + "'").CopyToDataTable();
                
                FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                //FrmMemoEntry.ShowForm(Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "MEMO_ID")), "SINGLE");
                FrmMemoEntry.ShowForm(MEMOID, "SALEINVOICEENTRY",0);
            }
        }

        private void FrmMemoEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            //BtnSearch.PerformClick();
        }

        private void FrmSaleInvoiceView_Load(object sender, EventArgs e)
        {
            try
            {
                cmbBillType.SelectedIndex = 0;
                string Str = new BOTRN_StockUpload().GetGridAccLayout(this.Name, GrdSummary.Name, cmbBillType.Text);
                if (Str != "")
                {
                    byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                    MemoryStream stream = new MemoryStream(byteArray);
                    GrdSummary.RestoreLayoutFromStream(stream);
                }
                else
                { 
                    
                }
                DataTable dt = objLedgerTrn.GetFromToDateYear();
                if (dt.Rows.Count > 0)
                {
                    DTPFormDate.Text = Val.ToString(dt.Rows[0][0]);
                    DTPToDate.Text = Val.ToString(dt.Rows[0][1]);
                }
                BtnSearch_Click(null, null);
                BtnAutoFit_Click(null, null);
            }


            catch (Exception ex)

            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void lblSaveLayout_Click(object sender, EventArgs e)
        {
            Stream str = new System.IO.MemoryStream();
            GrdSummary.SaveLayoutToStream(str);
            str.Seek(0, System.IO.SeekOrigin.Begin);
            StreamReader reader = new StreamReader(str);
            string text = reader.ReadToEnd();

            int IntRes = new BOTRN_StockUpload().SaveGridACCLayout(this.Name, GrdSummary.Name, text,cmbBillType.Text);

            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Saved");
            }
        }

        private void lblDefaultLayout_Click(object sender, EventArgs e)
        {
            int IntRes = new BOTRN_StockUpload().DeleteAccGridLayout(this.Name, GrdSummary.Name,cmbBillType.Text);
            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Deleted");
            }
        }

        private void GrdSummary_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            { 
                string StrEINVOICEUPLOADDATE = Val.ToString(GrdSummary.GetRowCellValue(e.RowHandle, "EINVOICEUPLOADDATE"));
                if (StrEINVOICEUPLOADDATE == "" || StrEINVOICEUPLOADDATE == "01/01/1900 00:00:00")
                {
                }
                else
                {
                    e.Appearance.BackColor = lblEInvoiceUploaded.BackColor;
                    e.Appearance.BackColor2 = lblEInvoiceUploaded.BackColor;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void cmbBillType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbBillType.SelectedIndex == 0)
                {
                    GrdSummary.Columns["FNETAMOUNT"].Visible = true;
                    GrdSummary.Columns["FGROSSAMOUNT"].Visible = true;

                    GrdSummary.Columns["NETAMOUNT"].Visible = false;
                    GrdSummary.Columns["GROSSAMOUNT"].Visible = false;
                    this.tabPage3.Hide();
                    this.TabSummaryDetail.TabPages.Remove(this.tabPage3);
                }
                else if (cmbBillType.SelectedIndex == 1)
                {
                    GrdSummary.Columns["FNETAMOUNT"].Visible = true;
                    GrdSummary.Columns["FGROSSAMOUNT"].Visible = true;

                    GrdSummary.Columns["NETAMOUNT"].Visible = true;
                    GrdSummary.Columns["GROSSAMOUNT"].Visible = true;
                    this.tabPage3.Hide();
                    this.TabSummaryDetail.TabPages.Remove(this.tabPage3);
                }
                else if (cmbBillType.SelectedIndex == 2)
                {
                    GrdSummary.Columns["FNETAMOUNT"].Visible = false;
                    GrdSummary.Columns["FGROSSAMOUNT"].Visible = false;

                    GrdSummary.Columns["NETAMOUNT"].Visible = true;
                    GrdSummary.Columns["GROSSAMOUNT"].Visible = true;
                    this.tabPage1.Hide();
                    this.tabPage2.Hide();
                    this.tabPage3.Hide();
                    this.TabSummaryDetail.TabPages.Remove(this.tabPage1);
                    this.TabSummaryDetail.TabPages.Remove(this.tabPage2);
                    this.TabSummaryDetail.TabPages.Remove(this.tabPage3);
                    this.tabPage1.Show();
                    this.tabPage2.Show();
                    this.tabPage3.Show();
                    this.TabSummaryDetail.TabPages.Insert(0, this.tabPage1);
                    this.TabSummaryDetail.TabPages.Insert(1, this.tabPage2);
                    this.TabSummaryDetail.TabPages.Insert(2, this.tabPage3);
                    TabSummaryDetail.SelectedIndex = 0;
                }
                else
                {
                    this.tabPage3.Hide();
                    this.TabSummaryDetail.TabPages.Remove(this.tabPage3);
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }

        }

        private void btnMultidebitNote_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                MemoEntryProperty objProperty = new MemoEntryProperty();
                string StrBrokerID = "";
                bool StrIsBroker = false;
                string MemoId = string.Empty;
                string MID = string.Empty;
                DataTable DtInvDetail = GetTableOfSelectedRows(GrdSummary, true, ObjGridSelection);
                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                if (DtInvDetail.Rows.Count > 1)
                {
                    int IntIsSameParty = 0;
                    for (int i = 0; i < DtInvDetail.Rows.Count; i++)
                    {
                        MemoId = MemoId == null ? Val.ToString(DtInvDetail.Rows[i]["MEMO_ID"]) :  MemoId + "," + Val.ToString(DtInvDetail.Rows[i]["MEMO_ID"]);
                    }
                    MID = MemoId.Remove(0, 1);
                    for (int i = 1; i < DtInvDetail.Rows.Count; i++)
                    {
                        StrBrokerID = DtInvDetail.Rows[0]["BROKER_ID"].ToString();
                        string StrBrokerID2 = DtInvDetail.Rows[i]["BROKER_ID"].ToString();
                        StrIsBroker = Val.ToBoolean(DtInvDetail.Rows[i]["IS_BROGST"]);
                       
                        if (StrBrokerID != StrBrokerID2)
                        {
                            IntIsSameParty = 1;
                            break;
                        }
                    }
                    if (BOConfiguration.DEPTNAME == "ACCOUNT")
                    {

                        if (IntIsSameParty == 1)
                        {
                            Global.MessageError("BROKER NAME ARE DIFFERENT, You Can't Debit Note Print...!");
                            this.Cursor = Cursors.Default;
                            return;
                        }
                        else
                        {
                            if (StrIsBroker == true)
                            {
                                if (Global.Confirm("Are You Sure For Print Entry") == System.Windows.Forms.DialogResult.No)
                                {
                                    return;
                                }
                                else
                                {
                                    if (DtInvDetail.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < DtInvDetail.Rows.Count; i++)
                                        {
                                            if (Val.ToString(DtInvDetail.Rows[i]["MDebitNoteDate"]) == "")
                                            {
                                                ObjMemo.UpdateDate(objProperty, Val.ToString(DtInvDetail.Rows[i]["MEMO_ID"]));
                                            }
                                        }
                                    }

                                    this.Cursor = Cursors.WaitCursor;
                                    DataTable DTab = new DataTable();

                                    DTab = ObjMemo.PrintBroDebitNoteMulti(Val.ToString(MID), "INR");


                                    if (DTab.Rows.Count == 0)
                                    {
                                        this.Cursor = Cursors.Default;
                                        Global.Message("There Is No Data Found For Print");
                                        return;
                                    }

                                    DataSet DS = new DataSet();
                                    DTab.TableName = "Table";
                                    DS.Tables.Add(DTab);
                                    DataTable DTabDuplicate = DTab.Copy();
                                    DTabDuplicate.TableName = "Table1";
                                    DTabDuplicate.AcceptChanges();
                                    DS.Tables.Add(DTabDuplicate);

                                    string BrokerName = "";
                                    if (DTabDuplicate.Rows.Count > 0)
                                    {
                                        BrokerName = Val.ToString(DTabDuplicate.Rows[0]["BrokerName"]);
                                    }

                                    Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                                    FrmReportViewer.MdiParent = Global.gMainRef;
                                    FrmReportViewer.ShowFormInvoicePrint("AccSaleBrokerGSTDebitNoteMulti", DTabDuplicate, BrokerName, "");
                                    this.Cursor = Cursors.Default;
                                }
                            }
                            else
                            {
                                if (Global.Confirm("Are You Sure For Print Entry") == System.Windows.Forms.DialogResult.No)
                                {
                                    return;
                                }
                                else
                                {
                                    this.Cursor = Cursors.WaitCursor;
                                    DataTable DTab = new DataTable();

                                    if (DtInvDetail.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < DtInvDetail.Rows.Count; i++)
                                        {
                                            if (Val.ToString(DtInvDetail.Rows[i]["MDebitNoteDate"]) == "01/01/1900 00:00:00")
                                            {
                                                ObjMemo.UpdateDate(objProperty, Val.ToString(DtInvDetail.Rows[i]["MEMO_ID"]));
                                            }
                                        }
                                    }

                                    DTab = ObjMemo.PrintBroDebitNoteMulti(Val.ToString(MID), "INR");
                                    if (DTab.Rows.Count == 0)
                                    {
                                        this.Cursor = Cursors.Default;
                                        Global.Message("There Is No Data Found For Print");
                                        return;
                                    }

                                    DataSet DS = new DataSet();
                                    DTab.TableName = "Table";
                                    DS.Tables.Add(DTab);
                                    DataTable DTabDuplicate = DTab.Copy();
                                    DTabDuplicate.TableName = "Table1";
                                    DTabDuplicate.AcceptChanges();
                                    DS.Tables.Add(DTabDuplicate);

                                    string BrokerName = "";
                                    if (DTabDuplicate.Rows.Count > 0)
                                    {
                                        BrokerName = Val.ToString(DTabDuplicate.Rows[0]["BrokerName"]);
                                    }

                                    Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                                    FrmReportViewer.MdiParent = Global.gMainRef;
                                    FrmReportViewer.ShowFormMemoPrint("AccBrokerSaleDebitNoteMulti", DS, BrokerName);
                                    this.Cursor = Cursors.Default;
                                }
                            }
                        }
                    }
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private DataTable GetTableOfSelectedRows(GridView view, Boolean IsSelect, BODevGridSelection pSelectGrid)
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
                if (pSelectGrid != null)
                {
                    aryLst = pSelectGrid.GetSelectedArrayList();
                    resultTable = sourceTable.Clone();
                    for (int i = 0; i < aryLst.Count; i++)
                    {
                        DataRowView oDataRowView = aryLst[i] as DataRowView;
                        resultTable.Rows.Add(oDataRowView.Row.ItemArray);
                    }
                }
            }

            return resultTable;
        }
    }
}
