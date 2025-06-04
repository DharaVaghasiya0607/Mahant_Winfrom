using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MahantExport.Account
{
    public partial class FrmPaymentReceive : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();

        public FrmPaymentReceive()
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
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        private void txtParty_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;

                    DataTable DtabParty = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);
                    FrmSearch.mDTab = DtabParty.Select("PARTYTYPE = 'SALE'").CopyToDataTable();

                    FrmSearch.mStrColumnsToHide = "PARTY_ID,BILLINGCOUNTRY_ID,SHIPPINGCOUNTRY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtParty.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtParty.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
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

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                if (txtParty.Text.Length == 0) txtParty.Tag = null;

                string StrType = "";

                if (RbtNatural.Checked == true)
                {
                    StrType = Val.ToString(RbtNatural.Text);
                }
                else
                {
                    StrType = Val.ToString(RbtLabgrown.Text);
                }

                DataSet DS = ObjMemo.GetPaymentReceiveData(Val.ToString(txtParty.Tag), "", StrType, Val.ToBoolean(ChkAll.Checked), Val.ToBoolean(ChkClear.Checked));

                MainGrdSummary.DataSource = DS.Tables[0];
                GrdSummary.BestFitColumns();

                MainGrdDetail.DataSource = DS.Tables[1];
                GrdDetail.BestFitColumns();

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void GrdSummary_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }
                if (e.Clicks == 2)
                {
                    this.Cursor = Cursors.WaitCursor;

                    string StrMemo_ID = Val.ToString(GrdSummary.GetFocusedRowCellValue("MEMO_ID"));

                    DataSet DS = ObjMemo.GetPaymentReceiveData("", StrMemo_ID, "",false,false);

                    MainGrdDetail.DataSource = DS.Tables[1];

                    GrdDetail.BestFitColumns();

                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void MainGrdSummary_Click(object sender, EventArgs e)
        {

        }

        private void GrdSummary_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }
                if (e.Column.FieldName == "ISPAYMENTCLEAR" || e.Column.FieldName == "ISPAYMENTRECEIVE")
                {
                    NotificationSendAndReceive Property = new NotificationSendAndReceive();

                    Property.MEMO_ID = Val.ToGuid(GrdSummary.GetFocusedRowCellValue("MEMO_ID"));
                    Property.ISRECEIVE = Val.ToBoolean(GrdSummary.GetFocusedRowCellValue("ISPAYMENTRECEIVE"));
                    Property.ISCLEAR = Val.ToBoolean(GrdSummary.GetFocusedRowCellValue("ISPAYMENTCLEAR"));

                    Property = ObjMemo.PaymentReceiveAndSale(Property);
                    //Global.Message(Property.ReturnMessageDesc);

                    if (Property.ReturnMessageType == "SUCCESS")
                    {
                        BtnSearch_Click(null,null);
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
    }
}