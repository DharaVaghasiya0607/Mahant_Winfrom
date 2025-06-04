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
using BusLib.Configuration;
using System.IO;
using BusLib.Parcel;
using BusLib.TableName;

namespace MahantExport.Parcel
{
    public partial class FrmParcelBoxMasterNew : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        DataTable DTabBox = new DataTable();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_Box ObjMast = new BOTRN_Box();

        public FrmParcelBoxMasterNew()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();


            this.Show();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
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
                else if (txtMixClarity.Text.Trim().Length == 0)
                {
                    Global.Message("Mix Clarity Name Is Required");
                    txtMixClarity.Focus();
                    return false;
                }                
                else if (txtMixSize.Text.Trim().Length == 0)
                {
                    Global.Message("Mix Size Name Is Required");
                    txtMixSize.Focus();
                    return false;
                }
                else if (txtShape.Text.Trim().Length == 0)
                {
                    Global.Message("Shape Name Is Required");
                    txtMixSize.Focus();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
            return true;
        }
        public void Clear()
        {
            txtBoxName.Text = string.Empty;
            txtMixClarity.Text = string.Empty;
            txtMixSize.Text = string.Empty;
            txtShape.Text = string.Empty;
            txtBoxName.Tag = string.Empty;
            txtMixClarity.Tag = string.Empty;
            txtMixSize.Tag = string.Empty;
            txtShape.Tag = string.Empty;
        }
        public void FetchValue(DataRow DR)
        {
            try
            {
                txtBoxName.Tag = Val.ToInt32(DR["BOX_ID"]);
                txtBoxName.Text = Val.ToString(DR["BOXNAME"]);
                txtMixClarity.Text = Val.ToString(DR["MIXCLARITY"]);
                txtMixClarity.Tag = Val.ToString(DR["MIXCLARITY_ID"]);
                txtMixSize.Text = Val.ToString(DR["SIZENAME"]);
                txtMixSize.Tag = Val.ToString(DR["MIXSIZE_ID"]);
                txtShape.Text = Val.ToString(DR["SHAPE"]);
                txtShape.Tag = Val.ToString(DR["SHAPE_ID"]);

                txtRate.Text = Val.ToString(DR["RATE"]);

                if (Val.ToBoolean(DR["ISACTIVE"]) == true)
                {
                    RbtYes.Checked = true;
                }
                else
                {
                    RbtNo.Checked = true;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }
        private void txtShapeID_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "SHAPE_ID,SHAPECODE,SHAPENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SHAPE);
                    FrmSearch.mStrColumnsToHide = "";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {

                        txtShape.Text = Val.ToString(FrmSearch.DRow["SHAPENAME"]);
                        txtShape.Tag = Val.ToString(FrmSearch.DRow["SHAPE_ID"]);
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

        private void txtMixSizeID_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "MIXSIZENAME,MIXSIZE_ID";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_MIXSIZE);
                    FrmSearch.mStrColumnsToHide = "MIXSIZE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtMixSize.Text = Val.ToString(FrmSearch.DRow["MIXSIZENAME"]);
                        txtMixSize.Tag = Val.ToString(FrmSearch.DRow["MIXSIZE_ID"]);
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

        private void txtMixClarityID_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "MIXCLARITY_ID,MIXCLARITYCODE,MIXCLARITYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_MIXCLARITY);
                    FrmSearch.mStrColumnsToHide = "MIXCLARITY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {

                        txtMixClarity.Text = Val.ToString(FrmSearch.DRow["MIXCLARITYNAME"]);
                        txtMixClarity.Tag = Val.ToString(FrmSearch.DRow["MIXCLARITY_ID"]);

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

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Global.ExcelExport("Parcel_BoxMaster.xlsx", GrdDet);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
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
                Property.BOXNAME = Val.ToString(txtBoxName.Text);
                Property.MIXCLARITY_ID = Val.ToInt32(txtMixClarity.Tag);
                Property.MIXSIZE_ID = Val.ToInt32(txtMixSize.Tag);
                Property.SHAPE_ID = Val.ToInt32(txtShape.Tag);
                Property.RATE = Val.ToDouble(txtRate.Text);

                if (RbtYes.Checked == true)
                {
                    Property.ISACTIVE = Val.ToBoolean(RbtYes.Checked);
                }

                Property = ObjMast.BoxMasterSave(Property);

                Global.Message(Property.RETURNMESSAGEDESC);

                if (Property.RETURNMESSAGETYPE == "SUCCESS")
                {
                    Clear();
                    BtnSearch_Click(null, null);
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
                string StrFromDate = ""; String StrToDate = "";
                if (DTPFromDate.Checked)
                {
                    StrFromDate = Val.ToString(DTPFromDate.Text);
                }
                if (DTPToDate.Checked)
                {
                    StrToDate = Val.ToString(DTPToDate.Text);
                }
                DataTable dt = ObjMast.BoxMasterGetData(StrFromDate, StrToDate);
                MainGrid.DataSource = dt;
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
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
                Global.Message(ex.Message);
            }
        }
    }
}