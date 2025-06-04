using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MahantExport.Utility
{
    public partial class FrmNotification  : DevControlLib.cDevXtraForm
    {
        public FrmNotification()
        {
            InitializeComponent();
        }
        public enum enmAction
        {
            Wait,
            Start,
            Close
        }
        private FrmNotification.enmAction action;

        private int x, y;
        public void ShowAlert(string StrMsg,Image Img)
        {
            this.Opacity = 0.0;
            this.StartPosition = FormStartPosition.Manual;
            string fname;

            //this.x = Screen.PrimaryScreen.WorkingArea.Width - this.Width + 15;
            //this.y = Screen.PrimaryScreen.WorkingArea.Height - this.Height * 10;

            for (int i = 0; i < 20; i++)
            {
                fname = "alert" + i.ToString();
                FrmNotification Frm = (FrmNotification)Application.OpenForms[fname];
                if (Frm == null)
                {
                    this.Name = fname;
                    this.x = Screen.PrimaryScreen.WorkingArea.Width - this.Width + 15;
                    this.y = Screen.PrimaryScreen.WorkingArea.Height - this.Height * i;
                    this.Location = new Point(this.x, this.y);
                    break;
                }
            }
            this.x = Screen.PrimaryScreen.WorkingArea.Width - this.Width - 5;

            this.BtnIcon.BackgroundImage = Img;

            this.lblMessage.Text = StrMsg;
            this.Show();
            this.action = enmAction.Start;
            this.timer1.Interval = 1;
            timer1.Start();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            try
            {
                timer1.Interval = 1;
                action = enmAction.Close;
            }
            catch (Exception ex)
            {
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                switch(this.action)
                {
                    case enmAction.Wait:
                        timer1.Interval = 5000;
                        action = enmAction.Close;
                        break;
                    case enmAction.Start:
                        timer1.Interval = 1;
                        this.Opacity += 0.1;
                        if (this.x < this.Location.X)
                        {
                            this.Left--;
                        }
                        else
                        {
                            if (this.Opacity == 1.0)
                            {
                                action = enmAction.Wait;
                            }
                        }
                        break;
                    case enmAction.Close:
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
            catch (Exception ex)
            {
            }
        }

    }
}
