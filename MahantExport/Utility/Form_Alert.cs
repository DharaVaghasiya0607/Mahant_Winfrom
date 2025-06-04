using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BusLib.Transaction;
using BusLib.Master;
using BusLib.Configuration;
using MahantExport.Stock;

namespace MahantExport.View
{
    public partial class Form_Alert  : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        public enum enmAction
        {
            wait,
            start,
            close
        }

        public enum enmType
        {
            Success,
            Warning,
            Error,
            Info
        }
        private Form_Alert.enmAction action;
        private int x, y;

        public Form_Alert()
        {
            InitializeComponent();
        }

        public void showAlert(string msg, enmType type, string UniqueID)
        {
            this.Opacity = 0.0;
            this.StartPosition = FormStartPosition.Manual;
            string fname;

            for (int i = 1; i < 10; i++)
            {
                fname = "alert" + i.ToString();
                Form_Alert frm = (Form_Alert)Application.OpenForms[fname];
                BtnDone.Tag = UniqueID;

                if (frm == null)
                {
                    this.Name = fname;
                    this.x = Screen.PrimaryScreen.WorkingArea.Width - this.Width + 15;
                    this.y = Screen.PrimaryScreen.WorkingArea.Height - this.Height * i - 5 * i;
                    this.Location = new Point(this.x, this.y);
                    break;

                }

            }
            this.x = Screen.PrimaryScreen.WorkingArea.Width - base.Width - 5;

            switch (type)
            {
                case enmType.Success:
                    //this.pictureEdit1.Image = Resources.success;
                    this.BackColor = Color.SeaGreen;
                    break;
                case enmType.Error:
                    //this.pictureEdit1.Image = Resources.error;
                    this.BackColor = Color.DarkRed;
                    break;
                case enmType.Info:
                    //this.pictureEdit1.Image = Resources.info;
                    //this.BackColor = Color.RoyalBlue;
                    //this.BackColor = Color.FromArgb(111, 127, 176);
                    this.BackColor = Color.SteelBlue;
                    break;
                case enmType.Warning:
                    //this.pictureEdit1.Image = Resources.warning;
                    this.BackColor = Color.DarkOrange;
                    break;
            }


            this.lblMsg.Text = msg;

            this.Show();
            this.action = enmAction.start;
            this.timer1.Interval = 1;
            this.timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (this.action)
            {
                case enmAction.wait:
                    timer1.Interval = 20000;
                    action = enmAction.close;
                    break;
                case Form_Alert.enmAction.start:
                    this.timer1.Interval = 1;
                    this.Opacity += 0.1;
                    if (this.x < this.Location.X)
                    {
                        this.Left--;
                    }
                    else
                    {
                        if (this.Opacity == 1.0)
                        {
                            action = Form_Alert.enmAction.wait;
                        }
                    }
                    break;
                case enmAction.close:
                    timer1.Interval = 1;
                    this.Opacity -= 0.1;

                    this.Left -= 3;
                    if (base.Opacity == 0.0)
                    {
                        base.Close();
                    }
                    break;
            }
        }

        private void BtnDone_Click(object sender, EventArgs e)
        {
            bool IsDone = true;
            string gStrMemoID = Convert.ToString(BtnDone.Tag);
            switch (this.action)
            {
                case enmAction.close:
                    timer1.Interval = 1;
                    this.Opacity -= 0.1;

                    this.Left -= 3;
                    if (base.Opacity == 0.0)
                    {
                        base.Close();
                    }
                    break;
            }
            if (IsDone == true)
            {
                //Added by Daksha on 17/01/2023
                if (Val.ToString(BOConfiguration.gEmployeeProperty.LEDGER_ID).ToUpper() == "B0703EEC-A579-EC11-A8B7-ACB57D1F87CF")
                {
                    FrmMemoEntry objForm = new FrmMemoEntry();
                    objForm.MdiParent = Global.gMainRef;
                    objForm.ShowForm(gStrMemoID);
                }
                else //End as Daksha
                {                    
                    new BOMST_FormPermission().UpdateNotificationOrderConfirm(gStrMemoID);
                }
                IsDone = false;
                gStrMemoID = string.Empty;
            }
        }

        private void Form_Alert_FormClosing(object sender, FormClosingEventArgs e)
        {            
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            switch (this.action)
            {
                case enmAction.close:
                    timer1.Interval = 1;
                    this.Opacity -= 0.1;

                    this.Left -= 3;
                    if (base.Opacity == 0.0)
                    {
                        base.Close();
                    }
                    break;
            }
        }
    }
}
