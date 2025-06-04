using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using System;
using System.Data;

namespace MahantExport.Utility
{
    public partial class FrmAPISetting : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
       // BusLib.Utility.BOMST_ApiSetting ObjMast = new BusLib.Utility.BOMST_ApiSetting();
        BOFormPer ObjPer = new BOFormPer();
        DataTable DtabDailyRate = new DataTable();
        BOMST_ApiSettingMaster ObjApi = new BOMST_ApiSettingMaster();

        #region Property Setting
        public FrmAPISetting()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            //ObjPer.GetFormPermission(Val.ToString(this.Tag));
            //btnSave.Enabled = ObjPer.ISINSERT;
            //btnDelete.Enabled = ObjPer.ISDELETE;

            AttachFormDefaultEvent();
            Val.FormGeneralSetting(this);
           
            this.Show();
            Fill();

        }

        private void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
           // ObjFormEvent.ObjToDisposeList.Add(ObjMast);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }
        #endregion

        #region Validation
        private bool ValSave()
        {

            if (Val.ToString(txtCode.Text).Trim().Equals(string.Empty))
            {
                Global.Message("ApiCode Is Required.");
                txtCode.Focus();
                return true;
            }

            return false;
        }

        #endregion

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            txtApi_ID.Tag = string.Empty;
            txtCode.Text = string.Empty;
            txtName.Text = string.Empty;
            txtUrl.Text = string.Empty;
            txtPort.Text = string.Empty;
            txtHost.Text = string.Empty;
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            CmbApiType.SelectedIndex = 0;

            ChkActive.Checked = true;

            Fill();

            txtCode.Focus();
        }

        private void Fill()
        {
            DtabDailyRate = ObjApi.Fill();
            MainGrid.DataSource = DtabDailyRate;
            MainGrid.Refresh();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValSave())
                {
                    return;
                }

                string ReturnMessageDesc = "";
                string ReturnMessageType = "";

                ApiSettingProperty Property = new ApiSettingProperty();


                Property.API_ID = Val.ToInt32(txtApi_ID.Tag);
                Property.APICODE = Val.ToString(txtCode.Text);
                Property.APINAME = Val.ToString(txtName.Text);
                Property.URL = Val.ToString(txtUrl.Text);
                Property.PORT = Val.ToInt32(txtPort.Text);
                Property.USERNAME = Val.ToString(txtUserName.Text);
                Property.PASSWORD = Val.ToString(txtPassword.Text);
                Property.APITYPE = Val.ToString(CmbApiType.SelectedItem);
                Property.ACTIVE = Val.ToBoolean(ChkActive.Checked);
                Property.HOSTNAME = Val.ToString(txtHost.Text);
                Property.EXCEL = Val.ToString(txtExcel.Text);
                Property.DIAMONDTYPE = Val.ToString(CmbDiamondType.SelectedItem);

                Property = ObjApi.Save(Property);

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
            catch(Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            ApiSettingProperty Property = new ApiSettingProperty();
            try
            {
                if (Val.ToString(txtCode.Text).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Select Records From the List That You Want To Delete");
                    return;
                }

                if (Global.Confirm("Are Your Sure To Delete The Record ?") == System.Windows.Forms.DialogResult.No)
                    return;

                Property.API_ID = Val.ToInt32(txtApi_ID.Tag);

                Property = ObjApi.Delete(Property);
                Global.Message(Property.ReturnMessageDesc);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    //BtnAdd_Click(null, null);
                    Clear();
                    //Fill();
                }
                else
                {
                    txtCode.Focus();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
            Property = null;
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

        private void FetchValue(DataRow DR)
        {

            txtApi_ID.Tag = Val.ToInt32(DR["API_ID"]);
            txtCode.Text = Val.ToString(DR["APICODE"]);
            txtName.Text = Val.ToString(DR["APINAME"]);
            txtUrl.Text = Val.ToString(DR["URL"]);
            txtHost.Text = Val.ToString(DR["HOSTNAME"]);
            txtPort.Text = Val.ToString(DR["PORT"]);
            txtUserName.Text = Val.ToString(DR["USERNAME"]);
            txtPassword.Text = Val.ToString(DR["PASSWORD"]);
            ChkActive.Checked = Val.ToBoolean(DR["ACTIVE"]);
            CmbApiType.Text = Val.ToString(DR["APITYPE"]);
            CmbDiamondType.Text = Val.ToString(DR["DIAMONDTYPE"]);
            txtExcel.Text = Val.ToString(DR["EXCELNAME"]);

            txtCode.Focus();
        }
    }
}
