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

namespace MahantExport.Pricing
{
    public partial class FrmMixRapSizeMapping : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_RapSizeMapping ObjMast = new BOMST_RapSizeMapping();
        DataTable Dtab = new DataTable();



        #region Property Settings

        public FrmMixRapSizeMapping()
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
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            if (ObjPer.ISVIEW == false)
            {
                Global.MessageError(BusLib.TPV.BOMessage.ViewDeniedMsg);
                return;
            }

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
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            if (ObjPer.ISVIEW == false)
            {
                Global.MessageError(BusLib.TPV.BOMessage.ViewDeniedMsg);
                return;
            }

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

            var Result = from row in Dtab.AsEnumerable()
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
            foreach (DataRow dr in Dtab.Rows)
            {
                //For Update Validation
                if (!Val.ToString(dr["ID"]).Trim().Equals(string.Empty) && Val.ToString(dr["S_CODE"]).Trim().Equals(string.Empty) ) 
                {
                    Global.Message("Please Enter S_Code");
                    IntCol = 0;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }
                //end as


                if (Val.ToString(dr["S_CODE"]).Trim().Equals(string.Empty))
                {
                    if (Dtab.Rows.Count == 1)
                    {
                        Global.Message("Please Enter S_Code");
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
                DataTable DTabChange = Dtab.GetChanges();
                if (DTabChange != null)
                {

                    foreach (DataRow Dr in DTabChange.Rows)
                    {
                        RapSizeMappingProperty Property = new RapSizeMappingProperty();

                        if (Val.ToString(Dr["S_CODE"]).Trim().Equals(string.Empty))
                            continue;

                        //Property.ID = Val.ToInt32(Dr["ID"]);
                        Property.S_CODE = Val.ToString(Dr["S_CODE"]);
                        Property.RAPFROMCARAT = Val.Val(Dr["RAPFROMCARAT"]);
                        Property.RAPTOCARAT = Val.Val(Dr["RAPTOCARAT"]);
                        Property.MAPFROMCARAT = Val.Val(Dr["MAPFROMCARAT"]);
                        Property.MAPTOCARAT = Val.Val(Dr["MAPTOCARAT"]);

                        Property = ObjMast.Save(Property);

                        ReturnMessageDesc = Property.ReturnMessageDesc;
                        ReturnMessageType = Property.ReturnMessageType;

                        Property = null;
                    }
                    Dtab.AcceptChanges();

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
            Dtab = ObjMast.Fill();
            DataRow Dr = Dtab.NewRow();
            Dtab.Rows.Add(Dr);
            MainGrid.DataSource = Dtab;
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
            Global.ExcelExport("RapSizeMapping", GrdDet);
        }

        private void deleteSelectedAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle >= 0)
                {
                    if (Global.Confirm("ARE YOU SURE YOU WANT TO DELETE ENTRY") == System.Windows.Forms.DialogResult.Yes)
                    {
                        FrmPassword FrmPassword = new FrmPassword();
                        FrmPassword.ShowForm(ObjPer.PASSWORD);

                        RapSizeMappingProperty Property = new RapSizeMappingProperty();
                        DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                        Property.ID = Val.ToInt32(Drow["ID"]);
                        Property = ObjMast.Delete(Property);

                        if (Property.ReturnMessageType == "SUCCESS")
                        {
                            Global.Message("ENTRY DELETED SUCCESSFULLY");
                            Dtab.Rows.RemoveAt(GrdDet.FocusedRowHandle);
                            Dtab.AcceptChanges();
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

        private void ReptxtMapToCts_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DataRow dr = GrdDet.GetFocusedDataRow();
                    if (!Val.ToString(dr["S_CODE"]).Equals(string.Empty) && GrdDet.IsLastRow)
                    {
                        Dtab.Rows.Add(Dtab.NewRow());
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
