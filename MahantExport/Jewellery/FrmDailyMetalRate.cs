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
    public partial class FrmDailyMetalRate : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_DailyMetalRate ObjMast = new BOTRN_DailyMetalRate();
        DataTable DtabDailyRate = new DataTable();
        string StrMesg = "";

        #region Property Settings

        public FrmDailyMetalRate()
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

            if (Val.ToString(txtMetal.Text).Trim().Equals(string.Empty))
            {
                Global.Message("'Currency' Is Required.");
                txtMetal.Focus();
                return true;
            }
            else if (Val.Val(txtPerGramRate.Text) == 0)
            {
                Global.Message("'Exchange Rate' Is Required.");
                txtPerGramRate.Focus();
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
            txtMetal.Text = string.Empty;
            txtMetal.Tag = string.Empty;
            txtPerGramRate.Text = string.Empty;
            txtPerGramRate.Tag = string.Empty;
            DTPRateDate.Text = Val.ToString(DateTime.Now);

            Fill();

            DTPRateDate.Focus();
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

                DailyMatelRateProperty Property = new DailyMatelRateProperty();

                Property.ID = Val.ToString(txtPerGramRate.Tag).Trim().Equals(string.Empty) ? BusLib.Configuration.BOConfiguration.FindNewSequentialID() : Guid.Parse(Val.ToString(txtPerGramRate.Tag));
                Property.METAL_ID = Val.ToInt32(txtMetal.Tag);
                Property.RATEDATE = Val.SqlDate(DTPRateDate.Text);                
                Property.PERGRAMRATE = Val.Val(txtPerGramRate.Text);
                Property.REMARK = Val.ToString(txtPerGramRate.Tag);


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
                        txtMetal.Text = Val.ToString(FrmSearch.DRow["CURRENCYNAME"]);
                        txtMetal.Tag = Val.ToString(FrmSearch.DRow["CURRENCY_ID"]);
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
            txtPerGramRate.Tag = Val.ToString(DR["DAILYRATE_ID"]);
            DTPRateDate.Text = Val.ToString(DR["APPLICATIONFROM"]);
            txtMetal.Text = Val.ToString(DR["CURRENCYNAME"]);
            txtMetal.Tag = Val.ToString(DR["CURRENCY_ID"]);
            txtPerGramRate.Text = Val.ToString(DR["EXCRATE"]);
            

            DTPRateDate.Focus();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            DailyMatelRateProperty Property = new DailyMatelRateProperty();
            try
            {
                if (Val.ToString(txtPerGramRate.Tag).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Select Records From the List That You Want To Delete");
                    return;
                }

                if (Global.Confirm("Are Your Sure To Delete The Record ?") == System.Windows.Forms.DialogResult.No)
                    return;

                 FrmPassword FrmPassword = new FrmPassword();
                 if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                 {
                     Property.ID = Guid.Parse(Val.ToString(txtPerGramRate.Tag));

                     Property = ObjMast.Delete(Property);
                     Global.Message(Property.ReturnMessageDesc);

                     if (Property.ReturnMessageType == "SUCCESS")
                     {
                         BtnAdd_Click(null, null);
                         Fill();
                     }
                     else
                     {
                         DTPRateDate.Focus();
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

        private void lblLatestRate_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            
            txtPerGramRate.Text = Global.GetLiveExchangeRate(txtMetal.Text).ToString();

            this.Cursor = Cursors.Default;
        }

        
    }
}
