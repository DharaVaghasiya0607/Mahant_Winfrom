using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using DevExpress.XtraPrinting;
using Google.API.Translate;
using MahantExport.Utility;
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
    public partial class FrmDailyRate : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_DailyRate ObjMast = new BOTRN_DailyRate();
        DataTable DtabDailyRate = new DataTable();
        string StrMesg = "";

        #region Property Settings

        public FrmDailyRate()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            BtnSave.Enabled = ObjPer.ISINSERT;
            BtnDelete.Enabled = ObjPer.ISDELETE;

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();
            Clear();
            
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMast);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }

        #endregion

        #region Validation

        private bool ValSave()
        {

            if (Val.ToString(txtCurrency.Text).Trim().Equals(string.Empty))
            {
                Global.Message("'Currency' Is Required.");
                txtCurrency.Focus();
                return true;
            }
            else if (Val.Val(txtExcRate.Text) == 0)
            {
                Global.Message("'Exchange Rate' Is Required.");
                txtExcRate.Focus();
                return true;
            }

            return false;
        }

        private bool ValDelete()
        {
            //if (txtItemGroupCode.Text.Trim().Length == 0)
            //{
            //    Global.Message("Group Code Is Required");
            //    txtItemGroupCode.Focus();
            //    return false;
            //}

            return true;
        }

        #endregion


        public void Clear()
        {
            txtCurrency.Text = string.Empty;
            txtCurrency.Tag = string.Empty;
            txtExcRate.Text = string.Empty;
            txtExcRate.Tag = string.Empty;
            DTPAsApplicationFromDate.Text = Val.ToString(DateTime.Now);

            Fill();

            DTPAsApplicationFromDate.Focus();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (ValSave())
                {
                    return;
                }

                string ReturnMessageDesc = "";
                string ReturnMessageType = "";

                DailyRateMasterProperty Property = new DailyRateMasterProperty();

                Property.DAILYRATE_ID = Val.ToString(txtExcRate.Tag).Trim().Equals(string.Empty) ? BusLib.Configuration.BOConfiguration.FindNewSequentialID() : Guid.Parse(Val.ToString(txtExcRate.Tag));
                Property.APPLICATIONFROM = Val.SqlDate(DTPAsApplicationFromDate.Text);
                Property.CURRENCY_ID = Val.ToInt32(txtCurrency.Tag);
                Property.EXCRATE = Val.Val(txtExcRate.Text);

                Property = ObjMast.Save(Property);

                ReturnMessageDesc = Property.ReturnMessageDesc;
                ReturnMessageType = Property.ReturnMessageType;

                Property = null;
                Global.Message(ReturnMessageDesc);

                if (ReturnMessageType == "SUCCESS")
                {
                    Clear();
                }
                else
                {
                    //txtItemGroupCode.Focus();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        public void Fill()
        {
            DtabDailyRate = ObjMast.Fill();
           MainGrid.DataSource = DtabDailyRate;
            MainGrid.Refresh();
        }


        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("Daily Rate List", GrdDet);
        }


        private void txtCurrency_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "CURRENCYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CURRENCY);

                    FrmSearch.mStrColumnsToHide = "CURRENCY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtCurrency.Text = Val.ToString(FrmSearch.DRow["CURRENCYNAME"]);
                        txtCurrency.Tag = Val.ToString(FrmSearch.DRow["CURRENCY_ID"]);
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

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GrdDet_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }

                if (e.Clicks == 2)
                {
                    DataRow DR = GrdDet.GetDataRow(e.RowHandle);
                    FetchValue(DR);
                    DR = null;
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        public void FetchValue(DataRow DR)
        {
            txtExcRate.Tag = Val.ToString(DR["DAILYRATE_ID"]);
            DTPAsApplicationFromDate.Text = Val.ToString(DR["APPLICATIONFROM"]);
            txtCurrency.Text = Val.ToString(DR["CURRENCYNAME"]);
            txtCurrency.Tag = Val.ToString(DR["CURRENCY_ID"]);
            txtExcRate.Text = Val.ToString(DR["EXCRATE"]);
            

            DTPAsApplicationFromDate.Focus();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DailyRateMasterProperty Property = new DailyRateMasterProperty();
            try
            {
                if (Val.ToString(txtExcRate.Tag).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Select Records From the List That You Want To Delete");
                    return;
                }

                if (Global.Confirm("Are Your Sure To Delete The Record ?") == System.Windows.Forms.DialogResult.No)
                    return;

                 FrmPassword FrmPassword = new FrmPassword();
                 if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                 {
                     Property.DAILYRATE_ID = Guid.Parse(Val.ToString(txtExcRate.Tag));

                     Property = ObjMast.Delete(Property);
                     Global.Message(Property.ReturnMessageDesc);

                     if (Property.ReturnMessageType == "SUCCESS")
                     {
                         BtnAdd_Click(null, null);
                         Fill();
                     }
                     else
                     {
                         DTPAsApplicationFromDate.Focus();
                     }
                 }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
            Property = null;
        }

        private void BtnAddCurrency_Click(object sender, EventArgs e)
        {
            FrmCurrency FrmCurrency = new FrmCurrency();
            FrmCurrency.MdiParent = Global.gMainRef;
            ObjFormEvent.ObjToDisposeList.Add(FrmCurrency);
            FrmCurrency.ShowForm();
        }
        public string GET_RATE(String STR)
        {
            string SS = "";
            try
            {
                if (STR == "CNBC")
                {
                    webBrowser1.Navigate("https://www.cnbc.com/quotes/INR=");
                    webBrowser1.ScriptErrorsSuppressed = true;

                    SS = webBrowser1.DocumentText.Substring(webBrowser1.DocumentText.IndexOf("Exchange\",\"price\":\"") + "Exchange\",\"price\":\"".Length, 5);
                }
                else
                {
                    webBrowser2.Navigate("https://www.moneycontrol.com/currency/bse-usdinr-price.html?classic=true");
                    webBrowser2.ScriptErrorsSuppressed = true;
                    String SF = webBrowser2.DocumentText;

                    SS = webBrowser2.DocumentText.Substring(webBrowser2.DocumentText.IndexOf("<span class='r_20'><strong>") + "<span class='r_20'><strong>".Length, 5);

                }

                return SS;
            }


            catch (Exception ex)
            {

                return "";
            }
        }
        private void lblLatestRate_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

                txtExcRate.Text = GET_RATE("CNBC");
            
            this.Cursor = Cursors.Default;
        }

        
    }
}
