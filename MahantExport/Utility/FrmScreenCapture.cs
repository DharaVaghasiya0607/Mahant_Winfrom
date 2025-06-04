using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Globalization;
using System.Collections;
using BusLib.Master;
using System.IO;
using System.Net.Sockets;
using System.Net;
using System.Security.Principal;
using MahantExport.Utility;
using System.Threading;
using BusLib.Configuration;

namespace MahantExport
{
    public partial class FrmScreenCapture : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();
        public DataTable DTab;

        public string ColumnsToHide = "";

        public bool AllowFirstColumnHide = false;

        public string StrInoutText = "";

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();

        public DataRow DRow { get; set; }

        public FrmScreenCapture()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            AttachFormDefaultEvent();
            Val.FormGeneralSetting(this);
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            if (ObjPer.ISVIEW == false)
            {
                Global.MessageError(BusLib.TPV.BOMessage.ViewDeniedMsg);
                return;
            }
            this.Show();
        }
        private void AttachFormDefaultEvent()
        {
            Val.FormGeneralSetting(this);
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.FormKeyPress = false;

            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);

        }

        private void FrmSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.No;
                this.Close();
            }
        }


        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Close();
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Bitmap bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                Graphics g = Graphics.FromImage(bm);
                g.CopyFromScreen(0, 0, 0, 0, bm.Size);
                pictureBox1.Image = bm;
                
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        public void Capture()
        {
            while (true)
            {
                Bitmap bm = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
                Graphics g = Graphics.FromImage(bm);
                g.CopyFromScreen(0, 0, 0, 0, bm.Size);
                pictureBox1.Image = bm;
                Thread.Sleep(1000);
            }
        }

        private void BtnRecord_Click(object sender, EventArgs e)
        {
            try
            {
                Thread t = new Thread(Capture);
                t.Start();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
      
    }
}