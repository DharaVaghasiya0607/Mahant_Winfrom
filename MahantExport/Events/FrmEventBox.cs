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
using BusLib.Configuration;
using MahantExport.Utility;
using System.Net;

namespace MahantExport.Events
{
    public partial class FrmEventBox : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();

        public DataTable DTab;
        public string ColumnsToHide = "";
        public bool AllowFirstColumnHide = false;
        public string StrInoutText = "";
        BOFormPer ObjPer = new BOFormPer();

        public DataRow DRow { get; set; }

        public FrmEventBox()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            
            AttachFormDefaultEvent();
            Val.FormGeneralSetting(this);         
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
      
        private void FrmEventBox_Load(object sender, EventArgs e)
        {
            var request = WebRequest.Create(Global.gstrEventUrl);

            using (var response = request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                pImage.Image = Bitmap.FromStream(stream);
                pImage.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        private void BtnClose_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();

            timer30Second.Stop();

            FrmYearCompanySelect FrmYearCompanySelect = new FrmYearCompanySelect();
            ObjFormEvent.ObjToDisposeList.Add(FrmYearCompanySelect);
            FrmYearCompanySelect.ShowForm();
        }

        private void timer30Second_Tick(object sender, EventArgs e)
        {
            try
            {
                BtnClose_Click_1(null,null);

            }
            catch (Exception EX)
            {
                Global.Message(EX.Message.ToString());
            }
        }
    }
}