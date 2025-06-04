using BusLib;
using BusLib.Configuration;
using BusLib.Transaction;
using DataClass;
using SDK_SC_RFID_Devices;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace MahantExport.Utility
{
    public partial class FrmConnectRFIDMachine : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_RFIDConnection ObjBORFIDCon = new BOTRN_RFIDConnection();
        RFID_Device RFIDCurrDevice;
        #region Property Settings

        public FrmConnectRFIDMachine()
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
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjBORFIDCon);
        }

        #endregion

        private void FrmConnectRFIDMachine_Load(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }

           
        }
        private void DisposeRFIDObject()
        {
            if (RFIDCurrDevice == null) return;

            if (RFIDCurrDevice.ConnectionStatus == ConnectionStatus.CS_Connected)
                RFIDCurrDevice.ReleaseDevice();
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DisposeRFIDObject();
                
                string StrMessage = ObjBORFIDCon.FindDevice();

                if (StrMessage != "Info : No device detected")
                {
                    RFIDCurrDevice = ObjBORFIDCon.ConnectDevice();
                    
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {

                if (RFIDCurrDevice != null)
                {
                    BOConfiguration.RFIDCurrDevice = RFIDCurrDevice;
                    Global.Message("Info : Device Connected");
                }
                else
                    Global.MessageError("Info : No Device Detected");
                this.Close();
            }
            catch (Exception ex)
            {
               
            }
        }

        //private const int CP_NOCLOSE_BUTTON = 0x200;
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams myCp = base.CreateParams;
        //        myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
        //        return myCp;
        //    }
        //}
    }
}
