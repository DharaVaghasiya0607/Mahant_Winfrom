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
    public partial class FrmBanner : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_Banner ObjMast = new BOMST_Banner();
        DataTable DtabBanner = new DataTable();


        #region Property Settings

        public FrmBanner()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            BtnSave.Enabled = ObjPer.ISINSERT;
            deleteSelectedAmountToolStripMenuItem.Enabled = ObjPer.ISDELETE;
            BtnDelete.Enabled = ObjPer.ISINSERT;
           
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();

            Clear();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMast);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }

        #endregion

        public bool CheckDuplicate(string ColName, string ColValue, int IntRowIndex, string StrMsg)
        {
            if (Val.ToString(ColValue).Trim().Equals(string.Empty))
                return false;

            var Result = from row in DtabBanner.AsEnumerable()
                         where Val.ToString(row[ColName]).ToUpper() == Val.ToString(ColValue).ToUpper() && row.Table.Rows.IndexOf(row) != IntRowIndex
                         select row;


            if (Result.Any())
            {
                Global.Message(StrMsg + " ALREADY EXISTS.");
                return true;
            }
            return false;
        }

        #region Validation

        private bool ValSave()
        { 
            if (Val.ToString(txtFileName.Text).Trim().Equals(string.Empty))
            {
                Global.Message("Please Select File Path");
                txtFileName.Focus();
                return true;
            }
            return false;
        }

        #endregion

        public void Clear()
        {
            txtFileName.Text = string.Empty;
            txtFileName.Tag = string.Empty;
            txtRemark.Text = string.Empty;
            Fill();
            txtFileName.Focus();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            Clear();

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (ValSave())
                {
                    return;
                }
                string ReturnMessageDesc = "";
                string ReturnMessageType = "";

               BannerMasterProperty Property = new BannerMasterProperty();

               Property.BANNER_ID = Val.ToInt32(txtFileName.Tag);
               Property.FILEPATH = Val.ToString(txtFileName.Text);
               
                Property.ISACTIVE = Val.ToBoolean(chkISActive.Checked);
                Property.REMARK = Val.ToString(txtRemark.Text);

                Property = ObjMast.Save(Property);

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
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        public void Fill()
        {
            DtabBanner = ObjMast.Fill();

            MainGrid.DataSource = DtabBanner;
            MainGrid.Refresh();

        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmLedger_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Escape)
            //{
            //    if (Global.Confirm("Do You Want To Close The Form?") == System.Windows.Forms.DialogResult.Yes)
            //        BtnBack_Click(null, null);
            //}
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("Banner List", GrdDet);
        }

        private void deleteSelectedAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle >= 0)
                {
                    if (Global.Confirm("ARE YOU SURE YOU WANT TO DELETE ENTRY") == System.Windows.Forms.DialogResult.Yes)
                    {
                        BannerMasterProperty Property = new BannerMasterProperty();
                        DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                        Property.BANNER_ID = Val.ToInt32(Drow["BANNER_ID"]);
                        Property = ObjMast.Delete(Property);

                        if (Property.ReturnMessageType == "SUCCESS")
                        {
                            Global.Message("ENTRY DELETED SUCCESSFULLY");
                            DtabBanner.Rows.RemoveAt(GrdDet.FocusedRowHandle);
                            DtabBanner.AcceptChanges();
                            Clear();
                        }
                        else
                        {
                            Global.Message("ERROR IN DELETE ENTRY");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog OpenFileDialog = new OpenFileDialog();
                //OpenFileDialog.Filter = "Excel Files (*.xls,*.xlsx)|*.xls;*.xlsx;";
                OpenFileDialog.Filter = "All files (*.*)|*.*|All files (*.*)|*.*";
                if (OpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtFileName.Text = OpenFileDialog.FileName;

                }
                OpenFileDialog.Dispose();
                OpenFileDialog = null;
            }
            catch (Exception ex)
            {
                Global.Message(ex.ToString() + "InValid File Name");
            }
        }

        private void GrdDet_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                MainGrid.DataSource = DtabBanner;
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

        public void FetchValue(DataRow DR)
        {
            txtFileName.Tag = Val.ToString(DR["BANNER_ID"]);
            txtFileName.Text = Val.ToString(DR["FILEPATH"]);
            

            txtRemark.Text = Val.ToString(DR["REMARK"]);


            txtFileName.Focus();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            BannerMasterProperty Property = new BannerMasterProperty();
            try
            {
                if (Val.ToString(txtFileName.Tag).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Select Records From the List That You Want To Delete");
                    return;
                }

                if (Global.Confirm("Are Your Sure To Delete The Record ?") == System.Windows.Forms.DialogResult.No)
                    return;

                  FrmPassword FrmPassword = new FrmPassword();
                  if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                  {
                      Property.BANNER_ID = Val.ToInt32(txtFileName.Tag);
                      Property.FILEPATH = Val.ToString(txtFileName.Text);

                      Property = ObjMast.Delete(Property);
                      Global.Message(Property.ReturnMessageDesc);

                      if (Property.ReturnMessageType == "SUCCESS")
                      {
                          BtnAdd_Click(null, null);
                          Fill();
                      }
                      else
                      {
                          txtFileName.Focus();
                      }
                  }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
            Property = null;
        }

    }
}
