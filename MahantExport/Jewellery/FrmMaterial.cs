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
    public partial class FrmMaterial : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_Material ObjMast = new BOMST_Material();
        DataTable DtabPara = new DataTable();

        //string StrMesg = "";


        #region Property Settings

        public FrmMaterial()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {


            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            //DtabPara = new BOMST_Material().GetParameterData();
            //CmbMaterialType.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_MATERIALTYPE);
            //CmbMaterialType.Properties.DisplayMember = "MATERIALTYPENAME";
            //CmbMaterialType.Properties.ValueMember = "MATERIALTYPE_ID";

            CmbMaterialType.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_MATERIALTYPE);
            CmbMaterialType.DisplayMember = "MATERIALTYPENAME";
            CmbMaterialType.ValueMember = "MATERIALTYPE_ID";

            CmbMaterialType.SelectedIndex = 0;

            //BtnAdd_Click(null, null);
            //Fill();


            BtnAdd_Click(null, null);
            Fill();
            this.Show();            
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            //ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMast);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }

        #endregion


        #region Validation

        private bool ValSave()
        {
            //if (txtItemGroupCode.Text.Trim().Length == 0)
            //{
            //    Global.Message("Group Code Is Required");
            //    txtItemGroupCode.Focus();
            //    return false;
            //}
            int IntCol = -1, IntRow = -1;
            foreach (DataRow dr in DtabPara.Rows)
            {
                //For Update Validation
                if (Val.ToString(dr["MATERIALCODE"]).Trim().Equals(string.Empty) && !Val.ToString(dr["MATERIAL_ID"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter '" + CmbMaterialType.DisplayMember + " Code'");
                    IntCol = 0;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }
                //end as


                if (Val.ToString(dr["MATERIALCODE"]).Trim().Equals(string.Empty))
                {
                    if (DtabPara.Rows.Count == 1)
                    {
                        Global.Message("Please Enter '" + CmbMaterialType.DisplayMember + " Code'");
                        IntCol = 0;
                        IntRow = dr.Table.Rows.IndexOf(dr);
                        break;

                    }
                    else
                        continue;
                }
                if (Val.ToString(dr["MATERIALNAME"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter '" + CmbMaterialType.DisplayMember + " Name'");
                    IntCol = 1;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }
                //if (Val.ToString(dr["PARAGROUP"]).Trim().Equals(string.Empty) && (Val.ToString(CmbMaterialType.SelectedItem) == "COLOR" || Val.ToString(CmbMaterialType.SelectedItem) == "CLARITY"))
                //{
                //    Global.Message("Please Select  'Group'");
                //    IntCol = 2;
                //    IntRow = dr.Table.Rows.IndexOf(dr);
                //    break;
                //}

            }
            if (IntRow >= 0)
            {
                GrdDet.FocusedRowHandle = IntRow;
                GrdDet.FocusedColumn = GrdDet.VisibleColumns[IntCol];
                GrdDet.Focus();
                return true;
            }
            return false;
        }

        #endregion

        public void Clear()
        {
            //GrdDet.FocusedRowHandle = 0;
            //GrdDet.FocusedColumn = GrdDet.VisibleColumns[0];
            //GrdDet.Focus();
            //GrdDet.ShowEditor();

            Fill();
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
                DataTable DTabChange = DtabPara.GetChanges();
                if (DTabChange != null)
                {

                    foreach (DataRow Dr in DTabChange.Rows)
                    {
                        MaterialMasterProperty Property = new MaterialMasterProperty();


                        if (Val.ToString(Dr["MATERIALCODE"]).Trim().Equals(string.Empty) || Val.ToString(Dr["MATERIALNAME"]).Trim().Equals(string.Empty))
                            continue;
                        Property.MATERIALTYPE_ID = Val.ToInt32(CmbMaterialType.SelectedValue);

                        Property.MATERIAL_ID = Val.ToInt32(Dr["MATERIAL_ID"]);
                        Property.MATERIALCODE = Val.ToString(Dr["MATERIALCODE"]);
                        Property.MATERIALNAME = Val.ToString(Dr["MATERIALNAME"]);
                        Property.PARENTMATERIAL_ID = Val.ToInt32(Dr["PARENTMATERIAL_ID"]);

                        Property.STATUS = Val.ToInt32(Dr["STATUS"]);
                        Property.ISACTIVE = Val.ToBoolean(Dr["ISACTIVE"]);
                        Property.REMARK = Val.ToString(Dr["REMARK"]);
                      
                        Property = ObjMast.Save(Property);

                        ReturnMessageDesc = Property.ReturnMessageDesc;
                        ReturnMessageType = Property.ReturnMessageType;

                        Property = null;
                    }
                    DtabPara.AcceptChanges();

                    Global.Message(ReturnMessageDesc);

                    if (ReturnMessageType == "SUCCESS")
                    {
                        Fill();


                        if (GrdDet.RowCount > 1)
                        {
                            GrdDet.FocusedRowHandle = GrdDet.RowCount - 1;
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        public void Fill()
        {
            DtabPara = ObjMast.Fill(Val.ToInt32(CmbMaterialType.SelectedValue));
            DataRow Dr = DtabPara.NewRow();
            DtabPara.Rows.Add(Dr);
            MainGrid.DataSource = DtabPara;
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

        private void GrdDet_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
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
        private void GrdDet_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Enter)
            //{
            //    DataRow DR = GrdDet.GetFocusedDataRow();
            //    FetchValue(DR);
            //    DR = null;
            //}
        }


        public void FetchValue(DataRow DR)
        {
            //txtParaType.Text = Val.ToString(DR["ITEMGROUP_ID"]);
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport(CmbMaterialType.ValueMember.ToString() + "List", GrdDet);
        }

        private void repTxtRemark_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DataRow dr = GrdDet.GetFocusedDataRow();
                    if (!Val.ToString(dr["MATERIALCODE"]).Equals(string.Empty) && !Val.ToString(dr["MATERIALNAME"]).Equals(string.Empty) && GrdDet.IsLastRow)
                    {
                        DtabPara.Rows.Add(DtabPara.NewRow());
                    }
                    else if (GrdDet.IsLastRow)
                    {
                        BtnSave.Focus();
                        e.Handled = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }


            //try
            //{
            //    if (e.KeyCode == Keys.Enter)
            //    {


            //        DataRow dr = GrdDet.GetFocusedDataRow();
            //        if (!Val.ToString(dr["MATERIALCODE"]).Equals(string.Empty) && !Val.ToString(dr["MATERIALNAME"]).Equals(string.Empty) && GrdDet.IsLastRow)
            //        {
            //            DataRow Dr = DtabPara.NewRow();
            //            if (Val.ToString(CmbMaterialType.SelectedItem) == "COLOR" || Val.ToString(CmbMaterialType.SelectedItem) == "CLARITY")
            //                Dr["PARAGROUP"] = "SINGLE";
            //            else
            //                Dr["PARAGROUP"] = "";

            //            DtabPara.Rows.Add(Dr);
            //            //DtabPara.AcceptChanges();

            //        }
            //        else if (GrdDet.IsLastRow)
            //        {
            //            BtnSave.Focus();
            //            e.Handled = true;
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Global.Message(ex.Message);
            //}
        }


        private void deleteSelectedAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle >= 0)
                {
                    if (Global.Confirm("ARE YOU SURE YOU WANT TO DELETE ENTRY") == System.Windows.Forms.DialogResult.Yes)
                    {
                        //FrmPassword FrmPassword = new FrmPassword();
                        //if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                        //{
                            MaterialMasterProperty Property = new MaterialMasterProperty();
                            DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                            Property.MATERIAL_ID = Val.ToInt32(Drow["MATERIAL_ID"]);
                            Property = ObjMast.Delete(Property);

                            if (Property.ReturnMessageType == "SUCCESS")
                            {
                                Global.Message("ENTRY DELETED SUCCESSFULLY");
                                DtabPara.Rows.RemoveAt(GrdDet.FocusedRowHandle);
                                DtabPara.AcceptChanges();
                                Fill();
                            }
                            else
                            {
                                Global.Message("ERROR IN DELETE ENTRY");
                            }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }

        private void repTxtMaterialCode_Validating(object sender, CancelEventArgs e)
        {
            //if (GrdDet.FocusedRowHandle < 0)
            //    return;

            //DataRow Dr = GrdDet.GetFocusedDataRow();
            //if (CheckDuplicate("MATERIALCODE", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "Code", "PARAGROUP", Val.ToString(Dr["PARAGROUP"])))
            //    e.Cancel = true;
            //return;

        }

        private void CmbParameterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear();
            Fill();
        }


        
    }
}
