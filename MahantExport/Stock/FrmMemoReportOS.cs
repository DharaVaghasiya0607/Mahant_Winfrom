using MahantExport.Stock;
using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.Data;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;
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

namespace MahantExport.Masters
{
    public partial class FrmMemoReportOS : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();
        BOFormPer ObjPer = new BOFormPer();
        #region Property Settings

        public FrmMemoReportOS()
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

            this.Show();

            DTPASOnDate.Value = DateTime.Now;
            CmbReportType.SelectedIndex = 0;
            DTPASOnDate.Focus();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjMemo);
        }

        #endregion

        
        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("Memo OS", GrdDet);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string StrFromDate = Val.SqlDate(DTPASOnDate.Value.ToShortDateString());

                DataTable DTab = ObjMemo.GetMemoReportOutStanding(StrFromDate, Val.ToString(CmbReportType.SelectedItem));
                    
                GrdDet.BeginUpdate();
                MainGrid.DataSource = DTab;

                if (Val.ToString(CmbReportType.SelectedItem) == "PARTY WISE")
                {
                    GrdDet.Columns["STOCKNO"].Visible = false;
                    GrdDet.Columns["BILLINGPARTYNAME"].Visible = true;
                }
                else if (Val.ToString(CmbReportType.SelectedItem) == "STONENO WISE")
                {
                    GrdDet.Columns["STOCKNO"].Visible = true;
                    GrdDet.Columns["BILLINGPARTYNAME"].Visible = false;
                }
                else if (Val.ToString(CmbReportType.SelectedItem) == "PARTY + STONENO WISE")
                {
                    GrdDet.Columns["BILLINGPARTYNAME"].Visible = true;
                    GrdDet.Columns["STOCKNO"].Visible = true;
                }
                GrdDet.EndUpdate();



                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
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
                    e.Graphics.DrawLine(new Pen(Brushes.Black, 1), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.BandsInfo[i].Bounds.Top), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.RowsInfo[info.RowsInfo.Count - 1].Bounds.Bottom - 1));
                    //if (i == 1) e.Graphics.DrawLine(new Pen(Brushes.Black), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.BandsInfo[i].Bounds.Top), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.RowsInfo[info.RowsInfo.Count - 1].Bounds.Bottom - 1));
                    //if (i == info.BandsInfo.BandCount - 1) e.Graphics.DrawLine(new Pen(Brushes.Black), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.BandsInfo[i].Bounds.Top), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.RowsInfo[info.RowsInfo.Count - 1].Bounds.Bottom - 1));
                }

            }
            catch (Exception)
            {
                
            }
           
        }

    }
}
