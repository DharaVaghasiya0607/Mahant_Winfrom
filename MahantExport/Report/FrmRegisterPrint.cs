using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
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

namespace MahantExport.Report
{
    public partial class FrmRegisterPrint : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();
        BOFormPer ObjPer = new BOFormPer();

        string StrFromDate = "";
        string StrToDate = "";
        string StrLedger = "";
        string StrAcctType = "";
        string StrReportType = "";

        #region Property Settings

        public FrmRegisterPrint()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();

            DataTable DTabLedger = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LEDGER);
            if (DTabLedger.Rows.Count > 0)
            {
                DTabLedger.DefaultView.Sort = "LEDGERNAME";
                CmbChkLedger.Properties.DataSource = DTabLedger;
                CmbChkLedger.Properties.DisplayMember = "LEDGERNAME";
                CmbChkLedger.Properties.ValueMember = "LEDGER_ID";
            }

            DataTable DTabAccount = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_ACCTTYPE);
            if (DTabAccount.Rows.Count > 0)
            {
                DTabAccount.DefaultView.Sort = "ACCTTYPENAME";
                CmbAccountType.DataSource = DTabAccount;
                CmbAccountType.DisplayMember = "ACCTTYPENAME";
                CmbAccountType.ValueMember = "ACCTTYPE_ID";
            }
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMemo);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
            
        }

        #endregion

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                StrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
                StrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());

                StrLedger = Val.Trim(CmbChkLedger.Properties.GetCheckedItems());

                StrAcctType = Val.ToString(CmbAccountType.Tag);

                StrReportType = Val.ToString(CmbReportType.SelectedItem);

                DataTable DTab = ObjMemo.RegisterPrint(StrFromDate, StrToDate, StrLedger, StrAcctType, StrReportType);
                if (DTab.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("There Is No Data Found For Print");
                    return;
                }

                Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                FrmReportViewer.MdiParent = Global.gMainRef;
                FrmReportViewer.ShowFormInvoicePrint(StrReportType, DTab);
                this.Cursor = Cursors.Default;

                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
           
        }
    }
}
