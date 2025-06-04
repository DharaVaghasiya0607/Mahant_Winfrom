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
using BusLib.Transaction;

namespace MahantExport.Masters
{
    public partial class FrmSizeMaster : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_ShapeMaster ObjShape = new BOMST_ShapeMaster();

        #region Property Settings

        public FrmSizeMaster()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            try
            {
                ObjPer.GetFormPermission(Val.ToString(this.Tag));
                Val.FormGeneralSetting(this);
                AttachFormDefaultEvent();
                ObjPer.GetFormPermission(Val.ToString(this.Tag));
                if (ObjPer.ISVIEW == false)
                {
                    Global.MessageError(BusLib.TPV.BOMessage.ViewDeniedMsg);
                    return;
                }

                Clear();
                Fill();

                string Str = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdDet.Name);

                if (Str != "")
                {
                    byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                    MemoryStream stream = new MemoryStream(byteArray);
                    GrdDet.RestoreLayoutFromStream(stream);
                }
                this.Show();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        public void AttachFormDefaultEvent()
        {
            try
            {
                ObjFormEvent.mForm = this;
                //ObjFormEvent.FormKeyDown = true;
                ObjFormEvent.FormKeyPress = false;
                ObjFormEvent.FormResize = true;
                ObjFormEvent.FormClosing = true;
                ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
                ObjFormEvent.ObjToDisposeList.Add(Val);
                ObjFormEvent.ObjToDisposeList.Add(ObjPer);
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        #endregion

        #region Operation

        public void Clear()
        {
            try
            {
                txtSize.Tag = string.Empty;
                txtSize.Text = string.Empty;
                txtSequenceNo.Text = string.Empty;
                txtRemark.Text = string.Empty;
                txtCode.Text = string.Empty;
                txtCode.Focus();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        public void Fill()
        {
            try
            {
                DataTable Dtab = ObjShape.SizeGetData();
                MainGrid.DataSource = Dtab;
                if (Dtab.Rows.Count > 0)
                {
                    GrdDet.BestFitColumns();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
            

        }

        public void FetchValue(DataRow DR)
        {
            try
            {
                txtSize.Tag = Val.ToInt32(DR["ID"]);
                txtCode.Text = Val.ToString(DR["CODE"]);
                txtSize.Text = Val.ToString(DR["NAME"]);
                txtSequenceNo.Text = Val.ToString(DR["SEQUENCENO"]);
                txtRemark.Text = Val.ToString(DR["REMARK"]);
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

        #endregion

        #region Validation

        private bool ValSave()
        {
            try
            {

                if (txtSize.Text.Trim().Length == 0)
                {
                    Global.Message("Shape Name Is Required");
                    txtSize.Focus();
                    return false;
                }
                else if (txtCode.Text.Trim().Length == 0)
                {
                    Global.Message("Code Is Required");
                    txtCode.Focus();
                    return false;
                }
                else if (txtSequenceNo.Text.Trim().Length == 0)
                {
                    Global.Message("SequenceNo Is Required");
                    txtSequenceNo.Focus();
                    return false;
                }
            
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
            return true;
        }


        private void txtSequenceNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        #endregion

        #region Control Event

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Global.ExcelExport("Size List", GrdDet);
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
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

                ShapeMasterProperty Property = new ShapeMasterProperty();

                Property.ID = Val.ToInt32(txtSize.Tag);
                    Property.SIZE = Val.ToString(txtSize.Text);
                    Property.CODE = Val.ToString(txtCode.Text);
                    Property.SEQUENCENO = Val.ToInt32(txtSequenceNo.Text);
                    Property.REMARK = Val.ToString(txtRemark.Text);
                    if (RbtYes.Checked == true)
                    {
                        Property.ISACTIVE = Val.ToBoolean(RbtYes.Checked);
                    }
                   
                    Property = ObjShape.SizeSave(Property);

                    Global.Message(Property.ReturnMessageDesc);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    Clear();
                    Fill();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.Confirm("ARE YOU SURE YOU WANT TO DELETE ENTRY ?") == System.Windows.Forms.DialogResult.Yes)
                {
                    ShapeMasterProperty Property = new ShapeMasterProperty();

                    Property.ID = Val.ToInt32(txtSize.Tag);

                    Property = ObjShape.SizeDelete(Property);

                    if (Property.ReturnMessageType == "SUCCESS")
                    {
                        Global.Message(Property.ReturnMessageDesc);
                        Fill();
                        Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        #endregion

        #region GridEvent

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


        #endregion

        #region Lyaout
        private void lblSaveLayout_Click(object sender, EventArgs e)
        {
            try
            {
                Stream str = new System.IO.MemoryStream();
                GrdDet.SaveLayoutToStream(str);
                str.Seek(0, System.IO.SeekOrigin.Begin);
                StreamReader reader = new StreamReader(str);
                string text = reader.ReadToEnd();

                int IntRes = new BOTRN_StockUpload().SaveGridLayout(this.Name, GrdDet.Name, text);
                if (IntRes != -1)
                {
                    Global.Message("Layout Successfully Saved");
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void lblDefaultLayout_Click(object sender, EventArgs e)
        {

            try
            {
                int IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdDet.Name);
                if (IntRes != -1)
                {
                    Global.Message("Layout Successfully Deleted");
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
        #endregion
    }
}
