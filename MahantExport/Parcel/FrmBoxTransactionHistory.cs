using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using BusLib.Transaction;
using BusLib.Configuration;
using MahantExport.Utility;

namespace MahantExport.Parcel
{
    public partial class FrmBoxTransactionHistory : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUploadParcel ObjStock = new BOTRN_StockUploadParcel();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        BODevGridSelection ObjGridSelection;

        DataTable DTabStockStatment = new DataTable();

        public FrmBoxTransactionHistory()
        {
            InitializeComponent();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = false;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjStock);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            this.Show();

        }
        public void ShowForm(string pStrBoxId, string pStrBoxName)
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            this.Show();

            txtBoxName.Text = pStrBoxName;
            txtBoxName.Tag = pStrBoxId;

            BtnSearch_Click(null, null);
        }

        private void txtBoxName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "BOX_ID,BOXNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_BOXMASTER);

                    FrmSearch.mStrColumnsToHide = "";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBoxName.Text = Val.ToString(FrmSearch.DRow["BOXNAME"]);
                        txtBoxName.Tag = Val.ToString(FrmSearch.DRow["BOX_ID"]);
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

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Global.ExcelExport("ParcelStock_History.xlsx", GrdDetail);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBoxName.Text.Length == 0)
                {
                    Global.Message("Box Name Is Required");
                    txtBoxName.Focus();
                    return;
                }
                this.Cursor = Cursors.WaitCursor;

                DTabStockStatment = ObjStock.GetDataForStockHistory(Val.ToInt32(txtBoxName.Tag));
                MainGrdDetail.DataSource = DTabStockStatment;
                MainGrdDetail.Refresh();
                GrdDetail.BestFitColumns();

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }
    }
}