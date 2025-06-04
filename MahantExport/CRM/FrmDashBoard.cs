using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
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

namespace AxoneDiaSales.CRM
{
    public partial class FrmDashBoard : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        DataSet Ds = new DataSet();
        DataTable DTabLatestPurchase = new DataTable();
        DataTable DTabTargetAchive = new DataTable();


        #region Property Settings

        public FrmDashBoard()
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

            Fill();
            CalculateSummary();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            //ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjStock);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }

        #endregion


        public void Fill()
        {

            string StrProceeType = "";
            string StrStockType = "";

            if (RtbOrder.Checked == true)
            {
                StrProceeType = "ORDER CONFIRM";
            }
            else
            {
                StrProceeType = "SALES DELIVERY";
            }

            if (RtbSingle.Checked == true)
            {
                StrStockType = "SINGLE";
            }
            else
            {
                StrStockType = "PARCEL";
            }

            Ds = ObjStock.GetLatestPurchaseData(StrProceeType, StrStockType);

            DTabLatestPurchase = Ds.Tables[0];
            DTabTargetAchive = Ds.Tables[1];

            MainGrid.DataSource = DTabLatestPurchase;
            MainGrid.Refresh();

            MainGrd.DataSource = DTabTargetAchive;
            MainGrd.Refresh();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Fill();
        }

        public void CalculateSummary()
        {
            double DouAchiveAmount = 0.00;
            double DouTargetAmount = 0.00;
            double DouAchivechiveNewCustomer = 0.00;
            double DouTargetNewCustomer = 0.00;
            double DouAchiveFollowup = 0.00;
            double DouTargetFollowup = 0.00;

            if (DTabTargetAchive != null && DTabTargetAchive.Rows.Count > 0)
            {
                DouAchiveAmount = Val.Val(DTabTargetAchive.Compute("SUM(SALETARGETDOLLAR)", string.Empty));
                DouTargetAmount = Val.Val(DTabTargetAchive.Compute("SUM(MEMOAMOUNT)", string.Empty));
                if (DouTargetAmount != 0)
                {
                ProgrssPanleAmount.Value = Val.ToInt64(Math.Round(((DouAchiveAmount / DouTargetAmount) * 100), 2));
                }
                else
                {
                    ProgrssPanleAmount.Value = 0;
                }

                DouAchiveFollowup = Val.Val(DTabTargetAchive.Compute("SUM(NOOFATTENDCOUSTOMER)", string.Empty));
                DouTargetFollowup = Val.Val(DTabTargetAchive.Compute("SUM(NOOFCUSTOMER)", string.Empty));
                if (DouTargetFollowup != 0)
                {
                    ProgressPanleFollwUp.Value = Val.ToInt64(Math.Round(((DouAchiveFollowup / DouTargetFollowup) * 100), 2));
                }
                else
                {
                    ProgressPanleFollwUp.Value = 0;
                }

                DouAchivechiveNewCustomer = Val.Val(DTabTargetAchive.Compute("SUM(NOOFNEWACHIVECOUSTOMER)", string.Empty));
                DouTargetNewCustomer = Val.Val(DTabTargetAchive.Compute("SUM(NOOFNEWCUSTOMER)", string.Empty));
                if (DouTargetNewCustomer != 0)
                {
                    ProgressPanelNewCustomer.Value = Val.ToInt64(Math.Round(((DouAchivechiveNewCustomer / DouTargetNewCustomer) * 100), 2));
                }
                else
                {
                    ProgressPanelNewCustomer.Value = 0;
                }
            }
        }

        private void GrdDet_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            try
            {

                Int32 IntDiff = Val.ToInt32(GrdDet.GetRowCellValue(e.RowHandle, "DIFF"));

                if (IntDiff < 15)
                {
                    e.Appearance.BackColor = lbl15.BackColor;
                    e.Appearance.BackColor2 = lbl15.BackColor;
                }
                else if (IntDiff > 15 && IntDiff < 15)
                {
                    e.Appearance.BackColor = lbl16to30.BackColor;
                    e.Appearance.BackColor2 = lbl16to30.BackColor;
                }
                else
                {
                    e.Appearance.BackColor = lbl30Up.BackColor;
                    e.Appearance.BackColor2 = lbl30Up.BackColor;
                    
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
                return;
            }
        }

        

    }
}
