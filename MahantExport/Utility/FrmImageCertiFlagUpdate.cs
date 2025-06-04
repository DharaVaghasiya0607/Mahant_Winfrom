using System;
using System.Windows.Forms;
using BusLib.Utility;

namespace MahantExport.Utility
{
    public partial class FrmImageCertiFlagUpdate : DevControlLib.cDevXtraForm
    {
        BOMST_ImageCertiFlagUpdate ObjMast = new BOMST_ImageCertiFlagUpdate();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();


        public FrmImageCertiFlagUpdate()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            this.Show();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            try
            {
                int res = ObjMast.ImageVedioCertiFlagUpdate();
                if (res > 0)
                {
                    Global.Message("Successfully Update");
                }
                else
                {
                    Global.Message("Opps...Something Wents Wrong");
                }

            }
            catch(Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
            this.Cursor = Cursors.Default;
        }

        private void BtnupdateWithUrl_Click(object sender, EventArgs e)
        {
            try
            {

                //DataTable Dtab = new 


                //var url = "http://www.domain.com/image.png";
                //HttpWebResponse response = null;
                //var request = (HttpWebRequest)WebRequest.Create(url);
                //request.Method = "HEAD";

            }
            catch(Exception ex)
            {
                Global.Message(ex.Message.ToString());                   
            }
        }
        
    }
}
