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
    public partial class FrmCartList : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_Style ObjMast = new BOMST_Style();
        DataTable DtabCollection = new DataTable();

        //DataSet DSet = new DataSet();
        string StrMesg = "";

        #region Property Settings

        public FrmCartList()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();
            DtpFromDate.Value = DateTime.Today.AddMonths(-1);
            DtpToDate.Value = DateTime.Now;

        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            //ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMast);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
            
        }

        #endregion

        private void BtnFind_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;



                Guid pCustomer_ID = Val.ToGuid(txtCustomer.Tag);
                string pStrStyleNo = Val.ToString(txtStyleNo.Text);

                string pStrFromDate = "";
                string pStrTODate = "";

                if (DtpFromDate.Checked == true)
                {
                    pStrFromDate = Val.SqlDate(DtpFromDate.Value.ToShortDateString());
                }

                if (DtpToDate.Checked == true)
                {
                    pStrTODate = Val.SqlDate(DtpToDate.Value.ToShortDateString());
                }

                //DSet = ObjMast.GetDataForCartSummary(pCustomer_ID, pStrStyleNo, pStrFromDate, pStrTODate);

                DtabCollection = ObjMast.GetDataForCartSummary(pCustomer_ID, pStrStyleNo, pStrFromDate, pStrTODate);
                //DtabDetails.Rows.Add(DtabDetails.NewRow());
                MainGrdSummary.DataSource = DtabCollection;
                MainGrdSummary.Refresh();
                GrdSummary.BestFitColumns();

                //MainGrdSummary.DataSource = DSet.Tables[0];
                //MainGrdSummary.RefreshDataSource();


                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                
                txtCustomer.Text = string.Empty;
                txtCustomer.Tag = string.Empty;
                txtStyleNo.Text = string.Empty;
                txtStyleNo.Tag = string.Empty;
                DtpFromDate.Value = DateTime.Today.AddMonths(-1);
                DtpToDate.Value = DateTime.Now;
                RbtAll.Checked = true;

                //DSet.Clear();
                MainGrdSummary.Refresh();
                GrdSummary.BestFitColumns();

                DtabCollection.Rows.Clear();
                //DTabDetail.Rows.Clear();
                //DtabMaterial.Rows.Clear();
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void txtCustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "CUSTOMERNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CUSTOMER);
                    FrmSearch.mStrColumnsToHide = "CUSTOMER_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtCustomer.Text = Val.ToString(FrmSearch.DRow["CUSTOMERNAME"]);
                        txtCustomer.Tag = Val.ToString(FrmSearch.DRow["CUSTOMER_ID"]);
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {

        }
    }
}
