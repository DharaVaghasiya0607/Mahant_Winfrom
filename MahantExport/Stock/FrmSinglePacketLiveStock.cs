using BusLib;
using BusLib.Configuration;
using BusLib.Attendance;
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
using MahantExport.Utility;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using BusLib.Master;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.Utils;
using BusLib.Rapaport;

namespace MahantExport.Stock
{
    public partial class FrmSinglePacketLiveStock : DevExpress.XtraEditors.XtraForm
    {
        BODevGridSelection ObjGridSelection;
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();

        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        BOFindRap ObjRap = new BOFindRap();

        LiveStockProperty mProperty = new LiveStockProperty();

        DataTable DtabLiveStockDetail = new DataTable();


        #region Property Settings

        public FrmSinglePacketLiveStock()
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
            GrdDetail.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;
            if (ObjGridSelection != null)
            {
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            }

            string Str = new BOTRN_KapanCreate().GetGridLayout(this.Name, GrdDetail.Name);

            if (Str != "")
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                MemoryStream stream = new MemoryStream(byteArray);
                GrdDetail.RestoreLayoutFromStream(stream);

            }
            this.Show();

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

        #endregion
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DataSet DsLiveStock = new DataSet();
                mProperty.WEBSTATUS = "AVAILABLE";
                DsLiveStock = ObjStock.GetLiveStockDataNew(mProperty, "All");
                DtabLiveStockDetail = DsLiveStock.Tables[0];

            }
            catch (Exception ex)
            {
                BtnSearch.Enabled = true;
                progressPanel1.Visible = false;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnKapanLiveStockAutoFit_Click(object sender, EventArgs e)
        {
            GrdDetail.BestFitColumns();
        }


        private void BtnKapanLiveStockExcelExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("PacketLiveStock", GrdDetail);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBestFit_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            GrdDetail.BestFitMaxRowCount = 5000000;
            GrdDetail.BestFitColumns();
            this.Cursor = Cursors.Default;
        }

        private void toolTipController1_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {

        }

        private void lblSaveLayout_Click(object sender, EventArgs e)
        {
            Stream str = new System.IO.MemoryStream();
            GrdDetail.SaveLayoutToStream(str);
            str.Seek(0, System.IO.SeekOrigin.Begin);
            StreamReader reader = new StreamReader(str);
            string text = reader.ReadToEnd();

            int IntRes = new BOTRN_KapanCreate().SaveGridLayout(this.Name, GrdDetail.Name, text);
            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Saved");
            }
        }

        private void lblDefaultLayout_Click(object sender, EventArgs e)
        {
            int IntRes = new BOTRN_KapanCreate().DeleteGridLayout(this.Name, GrdDetail.Name);
            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Deleted");
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
                BtnSearch.Enabled = true;
                progressPanel1.Visible = false;
                Global.Message(ex.Message.ToString());
            }
        }
        
        private void BtnSearch_Click_1(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
            progressPanel1.Visible = true;
            backgroundWorker1.RunWorkerAsync();
        }
    }
}