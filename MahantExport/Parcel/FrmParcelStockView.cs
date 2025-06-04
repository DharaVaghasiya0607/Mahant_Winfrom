using BusLib.Transaction;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MahantExport.Parcel
{
    public partial class FrmParcelStockView : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUploadParcel ObjStock = new BOTRN_StockUploadParcel();
        string mStrStockType = "";

        public FrmParcelStockView()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            this.Show();
            string Str = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdDetail.Name);

            if (Str != "")
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                MemoryStream stream = new MemoryStream(byteArray);
                GrdDetail.RestoreLayoutFromStream(stream);
            }
        }
        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            //ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjStock);
        }

        private void txtPartyName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTYONLY);

                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtPartyName.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtPartyName.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
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

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Global.ExcelExport("ParcelStockView.xlsx", GrdDetail);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {               
                this.Cursor = Cursors.WaitCursor;
                mStrStockType = "";
                if (RbtStock.Checked == true)
                {
                    mStrStockType = RbtStock.Tag.ToString();
                }
                else if (RbtAll.Checked == true)
                {
                    mStrStockType = RbtAll.Tag.ToString();
                }
                else if (RbtSale.Checked == true)
                {
                    mStrStockType = RbtSale.Tag.ToString();
                }


                DataTable DTab = ObjStock.GetDataForParcelStockView(Val.ToGuid(txtPartyName.Tag), mStrStockType);
                MainGrdDetail.DataSource = DTab;
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