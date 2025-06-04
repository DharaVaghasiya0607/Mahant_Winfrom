using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using DevExpress.XtraPrinting;
using Google.API.Translate;
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

namespace MahantExport.Pricing
{
    public partial class FrmParameterDiscountMaster : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_ParameterDiscount ObjMast = new BOMST_ParameterDiscount();
        DataTable DtabParameterDisc = new DataTable();



        #region Property Settings

        public FrmParameterDiscountMaster()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {

            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            BtnSave.Enabled = ObjPer.ISINSERT;
            deleteSelectedAmountToolStripMenuItem.Enabled = ObjPer.ISDELETE;

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            BtnAdd_Click(null, null);
            Fill();
            this.Show();
        }

        public void ShowForm(string pStrParaType)
        {

            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            BtnSave.Enabled = ObjPer.ISINSERT;
            deleteSelectedAmountToolStripMenuItem.Enabled = ObjPer.ISDELETE;

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

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

        public bool CheckDuplicate(string ColName, string ColValue, int IntRowIndex, string StrMsg)
        {
            if (Val.ToString(ColValue).Trim().Equals(string.Empty))
                return false;

            var Result = from row in DtabParameterDisc.AsEnumerable()
                         where Val.ToString(row[ColName]).Trim().ToUpper() == Val.ToString(ColValue).Trim().ToUpper() && row.Table.Rows.IndexOf(row) != IntRowIndex
                         select row;


            if (Result.Any())
            {
                Global.Message("'" + StrMsg + "' ALREADY EXISTS.");
                return true;
            }
            return false;
        }

        #region Validation

        private bool ValSave()
        {
            int IntCol = -1, IntRow = -1;
            foreach (DataRow dr in DtabParameterDisc.Rows)
            {
                //For Update Validation
                if (Val.ToString(dr["PARAMETER_ID"]).Trim().Equals(string.Empty) && !Val.ToString(dr["PARAMETERNAME"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter ParameterID'");
                    IntCol = 0;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }
                //end as


                if (Val.ToString(dr["PARAMETERNAME"]).Trim().Equals(string.Empty))
                {
                    if (DtabParameterDisc.Rows.Count == 1)
                    {
                        Global.Message("Please Enter ParameterName");
                        IntCol = 0;
                        IntRow = dr.Table.Rows.IndexOf(dr);
                        break;

                    }
                    else
                        continue;
                }

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

                    if (Global.Confirm("Are Your Sure To Save All Records ?") == System.Windows.Forms.DialogResult.No)
                    return;

                string ReturnMessageDesc = "";
                string ReturnMessageType = "";
               // DataTable DTabChange = DtabParameterDisc.GetChanges();
                if (DtabParameterDisc != null)
                {

                    foreach (DataRow Dr in DtabParameterDisc.Rows)
                    {
                        ParameterDiscountMasterProperty Property = new ParameterDiscountMasterProperty();

                        if (Val.ToString(Dr["PARAMETER_ID"]).Trim().Equals(string.Empty) || Val.ToString(Dr["PARAMETERNAME"]).Trim().Equals(string.Empty))
                            continue;

                        Property.PARAMETER_ID = Val.ToString(Dr["PARAMETER_ID"]);
                        Property.PARAMETERNAME = Val.ToString(Dr["PARAMETERNAME"]);
                        Property.PARAMETERRANK = Val.ToInt32(Dr["PARAMETERRANK"]);
                        Property.MIXRATE = Val.ToBoolean(Dr["MIXRATE"]);
                        Property.GROUPTYPE = Val.ToString(Dr["GROUPTYPE"]);

                        Property = ObjMast.Save(Property);

                        ReturnMessageDesc = Property.ReturnMessageDesc;
                        ReturnMessageType = Property.ReturnMessageType;

                        Property = null;
                    }
                    DtabParameterDisc.AcceptChanges();

                    Global.Message(ReturnMessageDesc);

                    if (ReturnMessageType == "SUCCESS")
                    {
                        Fill();

                        if (GrdDet.RowCount > 1)
                        {
                            GrdDet.FocusedRowHandle = GrdDet.RowCount - 1;
                        }
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
            DtabParameterDisc = ObjMast.Fill();
            DataRow Dr = DtabParameterDisc.NewRow();
            DtabParameterDisc.Rows.Add(Dr);
            MainGrid.DataSource = DtabParameterDisc;
            MainGrid.Refresh();

        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void FetchValue(DataRow DR)
        {
            //txtParaType.Text = Val.ToString(DR["ITEMGROUP_ID"]);
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("ParameterDiscountMaster", GrdDet);
        }

        private void deleteSelectedAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle >= 0)
                {
                    if (Global.Confirm("ARE YOU SURE YOU WANT TO DELETE ENTRY") == System.Windows.Forms.DialogResult.Yes)
                    {
                        ParameterDiscountMasterProperty Property = new ParameterDiscountMasterProperty();
                        DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                        Property.PARAMETER_ID = Val.ToString(Drow["PARAMETER_ID"]);
                        Property = ObjMast.Delete(Property);

                        if (Property.ReturnMessageType == "SUCCESS")
                        {
                            Global.Message("ENTRY DELETED SUCCESSFULLY");
                            DtabParameterDisc.Rows.RemoveAt(GrdDet.FocusedRowHandle);
                            DtabParameterDisc.AcceptChanges();
                            Fill();
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
       
        private void ReptxtParameter_ID_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle < 0)
                    return;

                DataRow Dr = GrdDet.GetFocusedDataRow();
                if (CheckDuplicate("PARAMETERNAME", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "Name"))
                    e.Cancel = true;
                return;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void ReptxtParameterName_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle < 0)
                    return;

                DataRow Dr = GrdDet.GetFocusedDataRow();
                if (CheckDuplicate("PARAMETER_ID", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "ParameterID"))
                    e.Cancel = true;
                return;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void RepTxtGroupType_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DataRow dr = GrdDet.GetFocusedDataRow();
                    if (!Val.ToString(dr["PARAMETERNAME"]).Equals(string.Empty) && GrdDet.IsLastRow)
                    {
                        DtabParameterDisc.Rows.Add(DtabParameterDisc.NewRow());
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
        }
    }
}
