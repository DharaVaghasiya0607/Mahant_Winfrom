using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace MahantExport.UserControl
{
    public partial class CustomNotifiation : DevExpress.XtraEditors.XtraUserControl
    {
        public CustomNotifiation()
        {
            InitializeComponent();
        }
        public enum enmAction
        {
            Wait,
            Start,
            Close
        }
        private CustomNotifiation.enmAction action;

        public void ShowAlert(string StrMsg)
        {

            for (int i = 0; i < 10; i++)
            {
                this.Width = Screen.PrimaryScreen.WorkingArea.Width - this.Width + 15;
                this.Height = Screen.PrimaryScreen.WorkingArea.Height - this.Height + i;
                this.Location = new Point(this.Width, this.Height);
                break;
            }
            this.Width = Screen.PrimaryScreen.WorkingArea.Width - this.Width - 5;
            this.lblMessage.Text = StrMsg;
            this.timer1.Interval = 1;
            timer1.Start();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            timer1.Interval = 1;
            action = enmAction.Close;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch(this.action)
            {
                case enmAction.Wait:
                    timer1.Interval = 5000;
                    action = enmAction.Close;
                    break;
                case enmAction.Start:
                    timer1.Interval = 1;
                    if (this.Width < this.Location.X)
                    {
                        this.Left--;
                    }
                    else
                    {

                    }
                    break;
                case enmAction.Close:
                    timer1.Interval = 1;
                    this.Left -= 3;
                    break;
            }
        }



    }
}
