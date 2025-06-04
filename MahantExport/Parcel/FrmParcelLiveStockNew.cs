using BusLib.TableName;
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
    public partial class FrmParcelLiveStockNew : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUploadParcel ObjStock = new BOTRN_StockUploadParcel();

        DataTable DtabPara = new DataTable();
        DataTable DTabSummary = new DataTable();
        public FrmParcelLiveStockNew()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            cmbShape.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SHAPE);
            cmbShape.Properties.ValueMember = "SHAPE_ID";
            cmbShape.Properties.DisplayMember = "SHAPENAME";

            cmbSize.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_MIXSIZE);
            cmbSize.Properties.ValueMember = "MIXSIZE_ID";
            cmbSize.Properties.DisplayMember = "MIXSIZENAME";

            cmbClarity.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_MIXCLARITY);
            cmbClarity.Properties.ValueMember = "MIXCLARITY_ID";
            cmbClarity.Properties.DisplayMember = "MIXCLARITYNAME";

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
        private bool ValSave()
        {
            try
            {

                if (txtBoxName.Text.Trim().Length == 0)
                {
                    Global.Message("Box Name Is Required");
                    txtBoxName.Focus();
                    return false;
                }
                else if (txtPartyName.Text.Trim().Length == 0)
                {
                    Global.Message("Party Name Is Required");
                    txtPartyName.Focus();
                    return false;
                }
                else if (txtCarat.Text.Trim().Length == 0)
                {
                    Global.Message("Carat Is Required");
                    txtCarat.Focus();
                    return false;
                }               
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
            return true;
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Global.ExcelExport("ParcelStock.xlsx", GrdDetail);
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
                string StrFromDate = ""; String StrToDate = "";
                if(DTPFromDate.Checked)
                {
                    StrFromDate = Val.ToString(DTPFromDate.Text);
                }
                if(DTPToDate.Checked)
                {
                    StrToDate = Val.ToString(DTPToDate.Text);
                }

                string pStrShape = Val.ToString(cmbShape.Properties.GetCheckedItems());
                string pStrSize = Val.ToString(cmbSize.Properties.GetCheckedItems());
                string pStrClarity = Val.ToString(cmbClarity.Properties.GetCheckedItems());

                DTabSummary = ObjStock.GetParcelLiveStockDataNew(StrFromDate, StrToDate, Val.ToInt32(txtBox.Tag), pStrShape, pStrSize, pStrClarity);
                MainGrdDetail.DataSource = DTabSummary;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void RepBtnAddStock_Click(object sender, EventArgs e)
        {
            try
            {
                GrpAddAndLessStock.Visible = true;
                LblEntryType.Text = "ADD STOCK";
                DataRow Drow = GrdDetail.GetDataRow(GrdDetail.FocusedRowHandle);
                txtBoxName.Text = Val.ToString(Drow["BOXNAME"]);
                txtBoxName.Tag = Val.ToString(Drow["BOX_ID"]);
                txtRate.Text = Val.ToString(Drow["RATE"]);

                txtCarat.Focus();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void RepBtnLessStock_Click(object sender, EventArgs e)
        {
            try
            {
                GrpAddAndLessStock.Visible = true;
                LblEntryType.Text = "LESS STOCK";
                txtRate.ReadOnly = false;
                DataRow Drow = GrdDetail.GetDataRow(GrdDetail.FocusedRowHandle);
                txtBoxName.Text = Val.ToString(Drow["BOXNAME"]);
                txtBoxName.Tag = Val.ToString(Drow["BOX_ID"]);
                txtRate.Text = Val.ToString(Drow["RATE"]);
                txtPartyName.Focus();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
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

        private void MainGrdDetail_Click(object sender, EventArgs e)
        {

        }

        private void BtnGrpClear_Click(object sender, EventArgs e)
        {
            txtBoxName.Tag = string.Empty;
            txtBoxName.Text = string.Empty;
            txtPartyName.Tag = string.Empty;
            txtPartyName.Text = string.Empty;
            txtCarat.Text = string.Empty;
            txtRate.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtRemark.Text = string.Empty;
            txtBroker.Text = string.Empty;
            txtBrokerPer.Text = string.Empty;
            txtBrokrageAmt.Text = string.Empty;
        }

        private void GrpBoxCloseButton_Click(object sender, EventArgs e)
        {
            BtnGrpClear_Click(null,null);
            GrpAddAndLessStock.Visible = false;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (ValSave() == false)
                {
                    return;
                }

                ParcelBoxMasterProperty Property = new ParcelBoxMasterProperty();

                Property.BOX_ID = Val.ToInt32(txtBoxName.Tag);
                Property.PARTY_ID = Val.ToGuid(txtPartyName.Tag);
                Property.BROKER_ID = Val.ToGuid(txtBroker.Tag);
                Property.BROKPER = Val.ToDouble(txtBrokerPer.Text);
                Property.DISCOUNT = Val.ToDouble(txtDiscount.Text);


                Property.CARAT = Val.ToDouble(txtCarat.Text);
                Property.RATE = Val.ToDouble(txtRate.Text);
                Property.AMOUNT = Val.ToDouble(txtAmount.Text);
                Property.ENTRYTYPE = Val.ToString(LblEntryType.Text);
                Property.REMARK = Val.ToString(txtRemark.Text);

                Property.BRKAMOUNT = Val.ToDouble(txtBrokrageAmt.Text);

                Property = ObjStock.AddLessStockSave(Property);

                Global.Message(Property.RETURNMESSAGEDESC);

                if (Property.RETURNMESSAGETYPE == "SUCCESS")
                {
                    BtnGrpClear_Click(null, null);
                    GrpAddAndLessStock.Visible = false;
                    BtnSearch_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtBox_KeyPress(object sender, KeyPressEventArgs e)
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
                        txtBox.Text = Val.ToString(FrmSearch.DRow["BOXNAME"]);
                        txtBox.Tag = Val.ToString(FrmSearch.DRow["BOX_ID"]);
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

        private void txtRate_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (Val.Val(txtDiscount.Text) == 0)
                {
                    txtAmount.Text = Math.Round(Val.Val(txtCarat.Text) * Val.Val(txtRate.Text), 2).ToString();
                }
                else
                {
                    double Disc = Val.ToDouble(txtDiscount.Text);
                    double Amount = Val.ToDouble(Math.Round(Val.Val(txtCarat.Text) * Val.Val(txtRate.Text), 2));
                    double DiscAmount = Val.ToDouble(Math.Round((Amount * Disc / 100), 2));

                    txtAmount.Text = Math.Round(Amount - DiscAmount, 2).ToString();
                }
                txtBrokrageAmt.Text = Math.Round((Val.Val(txtBrokerPer.Text) * Val.Val(txtAmount.Text)) / 100, 2).ToString();

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
        
        private void BtnClear_Click(object sender, EventArgs e)
        {
            DTabSummary.Rows.Clear();
            txtBox.Text = string.Empty;
            txtBox.Tag = string.Empty;
            DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            DTPToDate.Value = DateTime.Now;
        }

        private void GrdDetail_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                if (GrdDetail.FocusedRowHandle < 0)
                    return;

                if (e.Clicks == 2 && Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("BOXNAME"))
                {
                    FrmBoxTransactionHistory FrmBoxTransactionHistory = new FrmBoxTransactionHistory();
                    FrmBoxTransactionHistory.MdiParent = Global.gMainRef;
                    FrmBoxTransactionHistory.ShowForm(Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "BOX_ID")), Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "BOXNAME")));
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtBroker_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_BROKER);

                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBroker.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtBroker.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
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

        private void txtBrokerPer_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtBrokrageAmt.Text = Math.Round((Val.Val(txtAmount.Text) * Val.Val(txtBrokerPer.Text)) / 100, 2).ToString();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
    }
}