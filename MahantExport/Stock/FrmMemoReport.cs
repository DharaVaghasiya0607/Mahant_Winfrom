using MahantExport.Stock;
using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.Data;
using DevExpress.XtraPrinting;
using Google.API.Translate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MahantExport.Masters
{
    public partial class FrmMemoReport : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();
        BOFormPer ObjPer = new BOFormPer();
        DataTable DTabSummary = new DataTable();
        DataTable DTabDetail = new DataTable();
        DataTable DTabProcess = new DataTable();

        double DouCarat = 0;
        double DouWebSiteRap = 0;
        double DouWebSiteRapAmount = 0;
        double DouWebSitePricePerCarat = 0;
        double DouWebSiteAmount = 0;

        double DouMemoRap = 0;
        double DouMemoRapAmount = 0;
        double DouMemoPricePerCarat = 0;
        double DouMemoAmount = 0;
        double DouMemoAmountFE = 0;


        #region Property Settings

        public FrmMemoReport()
        {
            InitializeComponent();
        }

      
        public void ShowForm()
        {
           
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            if (ObjPer.ISVIEW == false)
            {
                Global.MessageError(BusLib.TPV.BOMessage.ViewDeniedMsg);
                return;
            }

            this.Show();

            DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            DTPToDate.Value = DateTime.Now;

            DTPFromDate.Focus();
         
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
            ObjFormEvent.ObjToDisposeList.Add(ObjMemo);
        }

        #endregion

        
        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("MemoPivot", PivotGridDet);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                string StrFromDate = "";
                string StrToDate = "";
              
                if (DTPFromDate.Checked)
                {
                    StrFromDate = Val.SqlDate(DTPFromDate.Text);
                }
                if(DTPToDate.Checked)
                {
                    StrToDate = Val.SqlDate(DTPToDate.Text);
                }

                if (txtBillTo.Text.Length == 0) txtBillTo.Tag = null;
                if (txtShipTo.Text.Length == 0) txtShipTo.Tag = null;
                if (txtBillToCountry.Text.Length == 0) txtBillToCountry.Tag = string.Empty;
                if (txtShipToCountry.Text.Length == 0) txtShipToCountry.Tag = string.Empty;
                if (txtSeller.Text.Length == 0) txtSeller.Tag = null;
                
                string StrStaus = "ALL";
                if (RbtPending.Checked == true)
                {
                    StrStaus = "PENDING";
                }
                else if (RbtPartial.Checked == true)
                {
                    StrStaus = "PARTIAL";
                }
                else if (RbtCompleted.Checked == true)
                {
                    StrStaus = "COMPLETED";
                }

                DataTable DTab = ObjMemo.GetMemoReport(
                    StrFromDate,
                    StrToDate,
                    txtMemoNo.Text,
                    txtStoneNo.Text,
                    Val.ToString(txtBillTo.Tag),
                    Val.ToInt(txtBillToCountry.Tag),
                    Val.ToString(txtShipTo.Tag),
                    Val.ToInt(txtShipToCountry.Tag),
                    Val.ToString(txtSeller.Tag), StrStaus, "");

                //PivotGridDet.BeginUpdate();
                PivotGridDet.DataSource = DTab;
                //PivotGridDet.BestFit();
                //PivotGridDet.EndUpdate();

                this.Cursor = Cursors.Default;


            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtSeller_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "EMPLOYEECODE,EMPLOYEENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_EMPLOYEE);

                    FrmSearch.mStrColumnsToHide = "EMPLOYEE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtSeller.Text = Val.ToString(FrmSearch.DRow["EMPLOYEENAME"]);
                        txtSeller.Tag = Val.ToString(FrmSearch.DRow["EMPLOYEE_ID"]);
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

        private void txtBillTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);

                    FrmSearch.mStrColumnsToHide = "PARTY_ID,BILLINGCOUNTRY_ID,SHIPPINGCOUNTRY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBillTo.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtBillTo.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
                        txtBillToCountry.Tag = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRY_ID"]);
                        txtBillToCountry.Text = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRYNAME"]);
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

        private void txtBillToCountry_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "COUNTRYCODE,COUNTRYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COUNTRY);

                    FrmSearch.mStrColumnsToHide = "COUNTRY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBillToCountry.Text = Val.ToString(FrmSearch.DRow["COUNTRYNAME"]);
                        txtBillToCountry.Tag = Val.ToString(FrmSearch.DRow["COUNTRY_ID"]);
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

        private void txtShipTo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);

                    FrmSearch.mStrColumnsToHide = "PARTY_ID,BILLINGCOUNTRY_ID,SHIPPINGCOUNTRY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtShipTo.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtShipTo.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
                        txtShipToCountry.Tag = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRY_ID"]);
                        txtShipToCountry.Text = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRYNAME"]);
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

        private void txtShipToCountry_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "COUNTRYCODE,COUNTRYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COUNTRY);

                    FrmSearch.mStrColumnsToHide = "COUNTRY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtShipToCountry.Text = Val.ToString(FrmSearch.DRow["COUNTRYNAME"]);
                        txtShipToCountry.Tag = Val.ToString(FrmSearch.DRow["COUNTRY_ID"]);
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

    }
}
