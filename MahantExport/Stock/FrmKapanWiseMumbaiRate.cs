using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
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

namespace MahantExport.Stock
{
    public partial class FrmKapanWiseMumbaiRate : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_KapanWiseMumbaiRate ObjTrn = new BOTRN_KapanWiseMumbaiRate();
        DataTable DtabKapanWiseRate = new DataTable();


        #region Property Settings

        public FrmKapanWiseMumbaiRate()
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
            ObjFormEvent.ObjToDisposeList.Add(ObjTrn);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }

        #endregion

        #region Validation

        private bool ValSave()
        {

            if (Val.ToString(txtKapanName.Text).Trim().Equals(string.Empty))
            {
                Global.Message("'Kapan' Is Required.");
                txtKapanName.Focus();
                return true;
            }
            else if (Val.Val(txtGIAKapanCarat.Text) == 0)
            {
                Global.Message("'Kapan Carat' Is Required.");
                txtGIAKapanCarat.Focus();
                return true;
            }
            else if (Val.Val(txtGIAKapanRate.Text) == 0)
            {
                Global.Message("'Kapan Rate' Is Required.");
                txtGIAKapanRate.Focus();
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
            txtKapanRate_ID.Text = string.Empty;
            txtKapanName.Text = string.Empty;
            DtpKapanDate.Text = DateTime.Now.ToString();

            txtGIAKapanCarat.Text = string.Empty;
            txtGIAKapanRate.Text = string.Empty;
            txtGIAKapanAmount.Text = string.Empty;

            txtMixKapanCarat.Text = string.Empty;
            txtMixKapanRate.Text = string.Empty;
            txtMixKapanAmount.Text = string.Empty;

            Fill();

            txtKapanName.Focus();
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

                KapanWiseMumbaiRateProperty Property = new KapanWiseMumbaiRateProperty();

                Property.KAPANRATE_ID = Val.ToString(txtKapanRate_ID.Text).Trim().Equals(string.Empty) ? BusLib.Configuration.BOConfiguration.FindNewSequentialID() : Guid.Parse(Val.ToString(txtKapanRate_ID.Text));
                Property.KAPANNAME = Val.ToString(txtKapanName.Text);
                Property.KAPANDATE = Val.SqlDate(DtpKapanDate.Text);
                Property.GIAKAPANCARAT = Val.Val(txtGIAKapanCarat.Text);
                Property.GIAKAPANRATE = Val.Val(txtGIAKapanRate.Text);
                Property.GIAKAPANAMOUNT = Val.Val(txtGIAKapanAmount.Text);

                Property.MIXKAPANCARAT = Val.Val(txtMixKapanCarat.Text);
                Property.MIXKAPANRATE = Val.Val(txtMixKapanRate.Text);
                Property.MIXKAPANAMOUNT = Val.Val(txtMixKapanAmount.Text);

                Property = ObjTrn.Save(Property);

                ReturnMessageDesc = Property.ReturnMessageDesc;
                ReturnMessageType = Property.ReturnMessageType;

                Property = null;
                if (ReturnMessageType == "SUCCESS")
                {
                    Global.Message(ReturnMessageDesc);
                    Clear();
                }
                else
                {
                    Global.MessageError(ReturnMessageDesc);
                    txtKapanName.Focus();
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
            DtabKapanWiseRate = ObjTrn.Fill(null, null);
            MainGrid.DataSource = DtabKapanWiseRate;
            MainGrid.Refresh();
        }


        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("KapanWise Mumbai Rate List", GrdDet);
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
            txtKapanRate_ID.Text = Val.ToString(DR["KAPANRATE_ID"]);
            DtpKapanDate.Text = Val.ToString(DR["KAPANDATE"]);
            txtKapanName.Text = Val.ToString(DR["KAPANNAME"]);

            txtGIAKapanCarat.Text = Val.ToString(DR["GIAKAPANCARAT"]);
            txtGIAKapanRate.Text = Val.ToString(DR["GIAKAPANRATE"]);
            txtGIAKapanAmount.Text = Val.ToString(DR["GIAKAPANAMOUNT"]);

            txtMixKapanCarat.Text = Val.ToString(DR["MIXKAPANCARAT"]);
            txtMixKapanRate.Text = Val.ToString(DR["MIXKAPANRATE"]);
            txtMixKapanAmount.Text = Val.ToString(DR["MIXKAPANAMOUNT"]);
            txtKapanName.Focus();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            KapanWiseMumbaiRateProperty Property = new KapanWiseMumbaiRateProperty();
            try
            {
                if (Val.ToString(txtKapanRate_ID.Text).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Select Records From the List That You Want To Delete");
                    return;
                }

                if (Global.Confirm("Are Your Sure To Delete The Record ?") == System.Windows.Forms.DialogResult.No)
                    return;

                FrmPassword FrmPassword = new FrmPassword();
                FrmPassword.ShowForm(ObjPer.PASSWORD);

                Property.KAPANRATE_ID = Guid.Parse(Val.ToString(txtKapanRate_ID.Text));

                Property = ObjTrn.Delete(Property);
                Global.Message(Property.ReturnMessageDesc);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    BtnAdd_Click(null, null);
                    Fill();
                }
                else
                {
                    DtpKapanDate.Focus();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
            Property = null;
        }

        private void txtKapanName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "KAPANNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                   // FrmSearch.mDTab= ObjTrn.FetchInvoiceWiseKapanDetail();
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtKapanName.Text = Val.ToString(FrmSearch.DRow["KAPANNAME"]);
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
        public void Calculation()
        {
            try
            {
                txtGIAKapanAmount.Text = Math.Round(Val.Val(txtGIAKapanCarat.Text) * Val.Val(txtGIAKapanRate.Text), 3).ToString();
                txtMixKapanAmount.Text = Math.Round(Val.Val(txtMixKapanCarat.Text) * Val.Val(txtMixKapanRate.Text), 3).ToString();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtGIAKapanRate_Validated(object sender, EventArgs e)
        {
            Calculation();
        }

        private void txtGIAKapanCarat_Validated(object sender, EventArgs e)
        {
            Calculation();
        }

        private void txtMixKapanCarat_Validated(object sender, EventArgs e)
        {
            Calculation();
        }
    }
}
