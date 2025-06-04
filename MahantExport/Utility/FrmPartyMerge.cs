using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Spire.Xls;
using System.Diagnostics;
using OfficeOpenXml;
using System.Net;
using System.Collections.Specialized;
using System.Threading;

using System.Configuration;
using System.Runtime.InteropServices;
using System.Reflection;
using GhostscriptSharp;
using WinAxoneActivity;
using BusLib.Utility;
using BusLib.Configuration;
using BusLib.TableName;
using BusLib.Master;


namespace MahantExport
{
    public partial class FrmPartyMerge  : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_Ledger ObjMst = new BOMST_Ledger();

        
        public FrmPartyMerge()
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
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMst);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }
     
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtFromParty.Text = string.Empty;
            txtFromParty.Tag = string.Empty;

            txtToParty.Text = string.Empty;
            txtToParty.Tag = string.Empty;
        }

        private void txtFromParty_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PARTYALL);
                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtFromParty.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtFromParty.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
                        DataTable DTab = ObjMst.GetProcessDataForParty(Guid.Parse(Val.ToString(txtFromParty.Tag)));
                        MainGrid.DataSource = DTab;
                        MainGrid.Refresh();
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtToParty_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PARTYALL);
                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtToParty.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtToParty.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }

        private void BtnTrnsfer_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtFromParty.Text.Length == 0)
                {
                    Global.Message("Please Select Form Party Name");
                    txtFromParty.Focus();
                    return;
                }

                if (txtToParty.Text.Length == 0)
                {
                    Global.Message("Please Select Form Party Name");
                    txtToParty.Focus();
                    return;
                }

                Guid GuidFromParty_ID = Guid.Parse(Val.ToString(txtFromParty.Tag));
                Guid GuidToParty_ID = Guid.Parse(Val.ToString(txtToParty.Tag));
                this.Cursor = Cursors.WaitCursor;

                if (Global.Confirm("Are You Sure For Goods Entry") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                string IntRes = ObjMst.MergeParty(GuidFromParty_ID, GuidToParty_ID);
                if (IntRes == "SUCESS")
                {
                    Global.Message("SUCCESSFULLY SAVED RIGHTS");
                }
                else
                {
                    Global.Message("OOPS SOMETHING GOES WRONG");
                }
                this.Cursor = Cursors.Default;
            }
            catch
            {
            }
        }

    }


}
