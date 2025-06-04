using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BusLib.Configuration;
using BusLib.Transaction;
using DevExpress.Data;

namespace MahantExport.UserActivities
{
    public partial class FrmLoginHistory : DevControlLib.cDevXtraForm
    {

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        BOTRN_StockUpload ObjLogin = new BOTRN_StockUpload();

        DataTable DTab = new DataTable();



        public FrmLoginHistory()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            BtnShow.Enabled = ObjPer.ISVIEW;

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
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjLogin);
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            try
            {
                DTab = ObjLogin.GetLoginHistoryData(Val.SqlDate(DTPFromDate.Text), Val.SqlDate(DTPToDate.Text));
                MainGrdDetail.DataSource = DTab;
                MainGrdDetail.Refresh();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }


        private void BtnExit_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (DTab.Rows.Count <= 0)
            {
                Global.Message("No Data  Found");
                return;
            }
            Global.ExcelExport("Login History", GrdDetail);
        }

        private void DTPFromDate_Validated(object sender, EventArgs e)
        {
            DTPFromDate.MaxDate = DateTime.Parse(DTPToDate.Text);
        }
    }
}