using BusLib;
using BusLib.Configuration;
using BusLib.TableName;
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
using DevExpress.XtraGrid.Columns;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using BusLib.Transaction;
using BusLib.Master;
using MahantExport.Utility;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
using DevExpress.XtraPrintingLinks;
using System.Drawing.Printing;

namespace MahantExport.Stock
{
    public partial class FrmSingleFileUploadView : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_Attendance ObjMast = new BOMST_Attendance();

        DataTable DtabExcelData = new DataTable();
        DataTable DtabFinal = new DataTable();
        DataTable DtabFileUpload = new DataTable();
        DataTable DtabPara = new DataTable();
        string StrUploadFilename = "";


        BOTRN_SingleFileUpload ObjUpload = new BOTRN_SingleFileUpload();
        Guid mUpload_ID = Guid.Empty;
        Guid mGroup_ID = Guid.Empty;
        int IntCheck = 0;
        string StrMessage = "";
        Int32 IntColor_ID = 0;
        BODevGridSelection ObjGridSelection;
        string mStrGIAAction = "";
        #region Property Settings

        public FrmSingleFileUploadView()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            BtnClear_Click(null, null);
            DtabPara = new BOMST_Parameter().GetParameterData();
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

        public void Clear()
        {
            this.Cursor = Cursors.WaitCursor;
            DtabFileUpload.Rows.Clear();
            CmbLabType.SelectedIndex = 0;
            DTPFromUploadDate.Text = Val.ToString(DateTime.Now);
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

            DtabFileUpload = ObjUpload.GetLabResultReturnData(mUpload_ID, Val.SqlDate(DTPFromUploadDate.Value.ToShortDateString()), Val.SqlDate(DtpToUploadDate.Value.ToShortDateString()), "", "", "", Val.ToString(CmbLabType.Text));
            MainGrid.DataSource = DtabFileUpload;
            GrdDetail.RefreshData();
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

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = ".xlsx";
                svDialog.Title = "Export to Excel";
                svDialog.FileName = "Lab Result File";
                svDialog.Filter = "Excel files 97-2003 (*.xls)|*.xls|Excel files 2007(*.xlsx)|*.xlsx|All files (*.*)|*.*";

                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    {
                        PrintableComponentLinkBase link = new PrintableComponentLinkBase()
                        {
                            PrintingSystemBase = new PrintingSystemBase(),
                            Component = MainGrid,
                            Landscape = true,
                            PaperKind = PaperKind.A4,
                            Margins = new System.Drawing.Printing.Margins(20, 20, 200, 20)
                        };

                        link.CreateReportHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderAreaExcel);

                        link.ExportToXls(svDialog.FileName);

                        if (Global.Confirm("Do You Want To Open [LabResultFile.xlsx] ?") == System.Windows.Forms.DialogResult.Yes)
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


            /*
            DevExpress.XtraPrinting.PrintingSystem PrintSystem = new DevExpress.XtraPrinting.PrintingSystem();
            PrinterSettingsUsing pst = new PrinterSettingsUsing();
            PrintSystem.PageSettings.AssignDefaultPrinterSettings(pst);
            //Lesson2 link = new Lesson2(PrintSystem);
            PrintableComponentLink link = new PrintableComponentLink(PrintSystem);
            GrdDetail.OptionsPrint.AutoWidth = true;
            GrdDetail.OptionsPrint.UsePrintStyles = true;
            link.Component = MainGrid;
            link.Landscape = false;
            link.PaperKind = System.Drawing.Printing.PaperKind.A4;

            link.Margins.Left = 40;
            link.Margins.Right = 40;
            link.Margins.Bottom = 40;
            link.Margins.Top = 120;

            link.CreateMarginalHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);
            link.CreateMarginalFooterArea += new CreateAreaEventHandler(Link_CreateMarginalFooterArea);

            link.CreateDocument();

            link.ShowPreview();
            link.PrintDlg();
            */
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

            TextBrick BrickTitle = e.Graph.DrawString("Daily Attendance Paper Of ( " + DTPFromUploadDate.Text + " )", System.Drawing.Color.Navy, new RectangleF(0, 0, e.Graph.ClientPageSize.Width, 20), DevExpress.XtraPrinting.BorderSide.None);
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

                this.Cursor = Cursors.WaitCursor;

                Fill();

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }

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
    }
}

