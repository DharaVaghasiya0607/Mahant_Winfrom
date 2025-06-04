using BusLib.Transaction;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MahantExport.Report
{
    public partial class FrmOpeningClosingReportNew : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();

        DataTable DTabDetail = new DataTable();

        public FrmOpeningClosingReportNew()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            this.Show();
        }
        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMemo);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            try
            {
                PanelProgress.Visible = true;
                if (!backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                string mStrFromDate = "";
                string mStrToDate = "";
                mStrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
                mStrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());
                DTabDetail = ObjMemo.GetOpeningClosingReportGetDataNew(mStrFromDate, mStrToDate);
            }
            catch (Exception ex)
            {
                PanelProgress.Visible = false;
                BtnShow.Enabled = true;
                Global.Message(ex.Message.ToString());
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                PanelProgress.Visible = false;
                BtnShow.Enabled = true;

                if (DTabDetail.Rows.Count <= 0)
                {
                    Global.Message("No Data Found..");
                }

                MainGrdSummary.DataSource = DTabDetail;
                GrdSummary.BestFitColumns();

            }
            catch (Exception ex)
            {
                PanelProgress.Visible = false;
                BtnShow.Enabled = true;
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdSummary_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
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
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }
    }
}