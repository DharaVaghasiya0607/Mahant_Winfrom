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
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MahantExport.Account
{
    public partial class FrmPaymentPending : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();

        public FrmPaymentPending()
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

        private void BtnAll_Click(object sender, EventArgs e)
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
                DataSet DS = ObjMemo.GetPendingPaymentData(Val.ToString(txtParty.Tag),"", StrType);

                MainGrdSummary.DataSource = DS.Tables[0];
                GrdSummary.BestFitColumns();

                MainGrdDetail.DataSource = DS.Tables[1];
                GrdDetail.BestFitColumns();

                MainGrdSale.DataSource = DS.Tables[2];
                GrdSale.BestFitColumns();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void GrdDetail_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
           
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

                    DataSet DS = ObjMemo.GetPendingPaymentData("", StrMemo_ID,"");

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

        private void txtSaleNo_KeyDown(object sender, KeyEventArgs e)
        {
           
        }

        private void TxtOrderNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String str1 = Val.ToString(TxtOrderNo.Text);
                string result = Regex.Replace(str1, @"\r\n?|\n", ",");
                if (result.EndsWith(",,"))
                {
                    result = result.Remove(result.Length - 1);
                }
                TxtOrderNo.Text = result;
                TxtOrderNo.Select(TxtOrderNo.Text.Length, 0);
            }
            catch (Exception EX)
            {
                Global.MessageError(EX.Message);
                return;
            }
        }

        private void RepSaleNo_KeyDown(object sender, KeyEventArgs e)
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
                if (e.Column.FieldName == "SALENO")
                {
                    if (Val.ToString(GrdSummary.GetFocusedRowCellValue("JANGEDNOSTR")).Length == 0)
                    {
                        Global.MessageError("Order No Is Required !!");
                        return;
                    }
                    if (Val.ToString(GrdSummary.GetFocusedRowCellValue("SALENO")).Length == 0)
                    {
                        Global.MessageError("Sale No Is Required !!");
                        return;
                    }

                    NotificationSendAndReceive Property = new NotificationSendAndReceive();
                     
                    Property.ORDERNO = Val.ToString(GrdSummary.GetFocusedRowCellValue("JANGEDNOSTR"));
                    Property.SALENO = Val.ToString(GrdSummary.GetFocusedRowCellValue("SALENO"));

                    Property = ObjMemo.UpdatePaymentOrderNo(Property);

                    if (Property.ReturnMessageType != "SUCCESS")
                    {
                        Global.Message(Property.ReturnMessageDesc);
                    }
                }
                if (e.Column.FieldName == "EXCRATE")
                {

                    MemoEntryProperty Property = new MemoEntryProperty();

                    Property.MEMO_ID = Val.ToString(GrdSummary.GetFocusedRowCellValue("MEMO_ID"));
                    Property.EXCRATE = Val.ToDouble(GrdSummary.GetFocusedRowCellValue("EXCRATE"));

                    Property = ObjMemo.UpdateSaleExcRate(Property);

                    Global.Message(Property.ReturnMessageDesc);                    
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
    }
}