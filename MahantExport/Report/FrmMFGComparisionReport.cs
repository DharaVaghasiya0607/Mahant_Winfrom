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
using System.Xml;
using BusLib.Rapaport;
using BusLib.Report;
using MahantExport.Utility;
using DevExpress.XtraGrid.Views.Grid;
using BusLib.ReportGrid;

namespace MahantExport.MFG
{
    public partial class FrmMFGComparisionReport : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();

        BOTRN_PredictionView ObjStock = new BOTRN_PredictionView();
        BOFindRap ObjRap = new BOFindRap();
        DataTable DTabGrdData = new DataTable();
        DataTable DTabPara = new DataTable();
        DataTable DTabStockNo = new DataTable();
        BODevGridSelection ObjGridSelection;
        string mStrStockNo = "";

        #region Property Settings

        public FrmMFGComparisionReport()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            if (ObjPer.ISVIEW == false)
            {
                Global.MessageError(BusLib.TPV.BOMessage.ViewDeniedMsg);
                return;
            }


            DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            DtpToDate.Value = DateTime.Now;

            this.Show();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = false;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        #endregion
        
        private void BtnShow_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                //Trn_SinglePrdProperty Property = new Trn_SinglePrdProperty();
                //mStrStockNo = Val.Trim(CmbStockNo.Properties.GetCheckedItems());
                //DTabGrdData = ObjStock.MFGComparisionData(Val.SqlDate(DTPFromDate.Value.ToShortDateString()), Val.SqlDate(DtpToDate.Value.ToShortDateString()), mStrStockNo);
                                
                //MainGrd.DataSource = DTabGrdData;
                //GrdDet.RefreshData();
                //GrdDet.BestFitColumns();          

                DTabGrdData.Rows.Clear();
                PanelProgress.Visible = true;
                if (!backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.RunWorkerAsync();
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }
                        
        private void GrdDet_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                return;
            }
            SetRowCellColor(e);
        }

        public void SetRowCellColor(RowCellStyleEventArgs e)
        {
            if (e.Column.FieldName == "CLARITYNAME")
            {
                int IntMfgSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, "MFGCLASEQ"));
                int IntSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, "CLASEQ"));
                if (IntSeqNo != 0 && IntMfgSeqNo != 0 && IntSeqNo > IntMfgSeqNo)
                {
                    e.Appearance.BackColor = lblUp.BackColor;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (IntSeqNo != 0 && IntMfgSeqNo != 0 && IntSeqNo < IntMfgSeqNo)
                {
                    e.Appearance.BackColor = lblDown.BackColor;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
            if (e.Column.FieldName == "COLORNAME")
            {
                int IntMfgSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, "MFGCOLSEQ"));
                int IntSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, "COLSEQ"));
                if (IntSeqNo != 0 && IntMfgSeqNo != 0 && IntSeqNo > IntMfgSeqNo)
                {
                    e.Appearance.BackColor = lblUp.BackColor;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (IntSeqNo != 0 && IntMfgSeqNo != 0 && IntSeqNo < IntMfgSeqNo)
                {
                    e.Appearance.BackColor = lblDown.BackColor;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
            if (e.Column.FieldName == "CUTNAME")
            {
                int IntMfgSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, "MFGCUTSEQ"));
                int IntSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, "CUTSEQ"));
                if (IntSeqNo != 0 && IntMfgSeqNo != 0 && IntSeqNo > IntMfgSeqNo)
                {
                    e.Appearance.BackColor = lblUp.BackColor;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (IntSeqNo != 0 && IntMfgSeqNo != 0 && IntSeqNo < IntMfgSeqNo)
                {
                    e.Appearance.BackColor = lblDown.BackColor;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
            if (e.Column.FieldName == "POLNAME")
            {
                int IntMfgSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, "MFGPOLSEQ"));
                int IntSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, "POLSEQ"));
                if (IntSeqNo != 0 && IntMfgSeqNo != 0 && IntSeqNo > IntMfgSeqNo)
                {
                    e.Appearance.BackColor = lblUp.BackColor;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (IntSeqNo != 0 && IntMfgSeqNo != 0 && IntSeqNo < IntMfgSeqNo)
                {
                    e.Appearance.BackColor = lblDown.BackColor;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
            if (e.Column.FieldName == "SYMNAME")
            {
                int IntMfgSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, "MFGSYMSEQ"));
                int IntSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, "SYMSEQ"));
                if (IntSeqNo != 0 && IntMfgSeqNo != 0 && IntSeqNo > IntMfgSeqNo)
                {
                    e.Appearance.BackColor = lblUp.BackColor;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (IntSeqNo != 0 && IntMfgSeqNo != 0 && IntSeqNo < IntMfgSeqNo)
                {
                    e.Appearance.BackColor = lblDown.BackColor;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }
            if (e.Column.FieldName == "FLNAME")
            {
                int IntMfgSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, "MFGFLSEQ"));
                int IntSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, "FLSEQ"));
                if (IntSeqNo != 0 && IntMfgSeqNo != 0 && IntSeqNo > IntMfgSeqNo)
                {
                    e.Appearance.BackColor = lblUp.BackColor;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
                else if (IntSeqNo != 0 && IntMfgSeqNo != 0 && IntSeqNo < IntMfgSeqNo)
                {
                    e.Appearance.BackColor = lblDown.BackColor;
                    e.Appearance.ForeColor = Color.White;
                    e.Appearance.FontStyleDelta = FontStyle.Bold;
                }
            }            
        }

        private void btnExcelExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = ".xlsx";
                svDialog.Title = "Export to Excel";
                svDialog.FileName = "MFG Comparision";
                svDialog.Filter = "Excel files 97-2003 (*.xls)|*.xls|Excel files 2007(*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    {
                        PrintableComponentLinkBase link = new PrintableComponentLinkBase()
                        {
                            PrintingSystemBase = new PrintingSystemBase(),
                            Component = MainGrd,
                            Landscape = true,
                            PaperKind = PaperKind.A4,
                            Margins = new System.Drawing.Printing.Margins(20, 20, 200, 20)
                        };
                        link.CreateReportHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);

                        link.ExportToXls(svDialog.FileName);

                        if (Global.Confirm("Do You Want To Open [MFGComparision.xlsx] ?") == System.Windows.Forms.DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(svDialog.FileName, "CMD");
                        }
                    }
                }
                svDialog.Dispose();
                svDialog = null;
            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
            }
        }

        public void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            //// Company Title
            //TextBrick BrickTitle = e.Graph.DrawString(BusLib.Configuration.BOConfiguration.gEmployeeProperty.COMPANYNAME, System.Drawing.Color.Navy, new RectangleF(0, 0, e.Graph.ClientPageSize.Width + 360, 35), DevExpress.XtraPrinting.BorderSide.None);
            //BrickTitle.Font = new Font("verdana", 12, FontStyle.Bold);
            //BrickTitle.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            //BrickTitle.VertAlignment = DevExpress.Utils.VertAlignment.Center;

            //// Report Name
            //TextBrick BrickTitleseller = e.Graph.DrawString("MFG Comparision Report", System.Drawing.Color.Navy, new RectangleF(0, 35, e.Graph.ClientPageSize.Width + 360, 25), DevExpress.XtraPrinting.BorderSide.None);
            //BrickTitleseller.Font = new Font("verdana", 10, FontStyle.Bold);
            //BrickTitleseller.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            //BrickTitleseller.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            //BrickTitleseller.ForeColor = Color.Black;

            //// ' For Stone NO
            //TextBrick BrickTitlesParam = e.Graph.DrawString("Stone No :- " + Val.ToString(mStrStockNo), System.Drawing.Color.Navy, new RectangleF(0, 60, e.Graph.ClientPageSize.Width + 360, 25), DevExpress.XtraPrinting.BorderSide.None);
            //BrickTitlesParam.Font = new Font("verdana", 8, FontStyle.Bold);
            //BrickTitlesParam.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
            //BrickTitlesParam.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            //BrickTitlesParam.ForeColor = Color.Black;
            
            // FROM TO DATE
            TextBrick BrickTitlesParamFROMTO = e.Graph.DrawString("From Date :- " + Val.SqlDate(DTPFromDate.Value.ToShortDateString()) + " To Date :- " + Val.SqlDate(DtpToDate.Value.ToShortDateString()), System.Drawing.Color.Navy, new RectangleF(0, 0, e.Graph.ClientPageSize.Width + 360, 25), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitlesParamFROMTO.Font = new Font("verdana", 8, FontStyle.Bold);
            BrickTitlesParamFROMTO.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
            BrickTitlesParamFROMTO.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            BrickTitlesParamFROMTO.ForeColor = Color.Black;

            //// Print Date
            //int IntX = Convert.ToInt32(Math.Round(e.Graph.ClientPageSize.Width - 140, 0));
            //TextBrick BrickTitledate = e.Graph.DrawString("Print Date :- " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), System.Drawing.Color.Navy, new RectangleF(IntX, 85, 450, 25), DevExpress.XtraPrinting.BorderSide.None);
            //BrickTitledate.Font = new Font("verdana", 8, FontStyle.Bold);
            //BrickTitledate.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            //BrickTitledate.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            //BrickTitledate.ForeColor = Color.Black;

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                mStrStockNo = Val.Trim(txtStoneMFGComparision.Text);
                DTabGrdData = ObjStock.MFGComparisionData(Val.SqlDate(DTPFromDate.Value.ToShortDateString()), Val.SqlDate(DtpToDate.Value.ToShortDateString()), mStrStockNo);
            }
            catch (Exception Ex)
            {
                PanelProgress.Visible = false;
                this.Cursor = Cursors.Default;
                Global.Message(Ex.Message.ToString());
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {            
            try
            {
                PanelProgress.Visible = false;

                MainGrd.DataSource = DTabGrdData;
                //GrdDet.BestFitColumns();
                GrdDet.RefreshData();
            }
            catch (Exception Ex)
            {
                PanelProgress.Visible = false;
                this.Cursor = Cursors.Default;
                Global.Message(Ex.Message.ToString());
            }
        }

        private void txtStoneMFGComparision_TextChanged(object sender, EventArgs e)
        {
            String str1 = "";
            if (txtStoneMFGComparision.Text.Trim().Contains("\t\n"))
            {
                str1 = txtStoneMFGComparision.Text.Trim().Replace("\t\n", ",");
            }
            else if (txtStoneMFGComparision.Text.Trim().Contains("\r"))
            {
                str1 = txtStoneMFGComparision.Text.Trim().Replace("\r", "");
            }
            else
            {
                str1 = txtStoneMFGComparision.Text.Trim().Replace("\n", ",");
            }

            txtStoneMFGComparision.Text = str1;
            txtStoneMFGComparision.Select(txtStoneMFGComparision.Text.Length, 0);
        }

    }
}
