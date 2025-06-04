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
using MahantExport.Account;
using MahantExport.CRM;
using MahantExport.Grading;
using MahantExport.Master;
using MahantExport.Masters;
using MahantExport.Parcel;
using MahantExport.Pricing;
using MahantExport.Report;
using MahantExport.Stock;

namespace MahantExport.MDI
{
    public partial class FrmHome : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();

        public FrmHome()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            try
            {
                Val.FormGeneralSetting(this);
                AttachFormDefaultEvent();
                this.Show();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
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
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        private void TileLiveStock_ItemClick(object sender, TileItemEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmParcelLiveStock FrmParcelLiveStock = new FrmParcelLiveStock();
            FrmParcelLiveStock.MdiParent = Global.gMainRef; ;
            FrmParcelLiveStock.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }
    }
}