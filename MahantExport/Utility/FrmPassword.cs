using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusLib.TableName;
using BusLib.Configuration;
using BusLib.Master;
using System.Deployment.Application;
using System.Configuration;
using System.IO;
using System.Xml;
using MahantExport;
namespace MahantExport.Utility
{
    public partial class FrmPassword : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();        
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        System.Windows.Forms.DialogResult mDialog;

        string mStrPassword = ""; 
        #region Constructor

        public FrmPassword()
        {
            InitializeComponent();
        }
        public DialogResult ShowForm(string pFromPassword)
        {
            mStrPassword = pFromPassword;
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            
            this.ShowDialog();
            return mDialog;
        }
        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;

            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = false;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);            
        }


        #endregion

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            if (Val.Trim(txtPassword.Text) != "" && mStrPassword != "" && txtPassword.Text.ToUpper() == mStrPassword.ToUpper())
            {
                this.DialogResult = System.Windows.Forms.DialogResult.Yes;
                mDialog = this.DialogResult;
                this.Close();
            }
            else
            {
                if (Val.Trim(txtPassword.Text) != "" && mStrPassword != "" && txtPassword.Text != mStrPassword)
                {
                    Global.Message("Your Password is not Valid");
                }
                this.DialogResult = System.Windows.Forms.DialogResult.No;
                mDialog = this.DialogResult;
                this.Close();
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.No;
                mDialog = this.DialogResult;
                this.Close();
            }
       }
  
    }
}
