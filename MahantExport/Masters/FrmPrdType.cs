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
    public partial class FrmPrdType : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_PrdType ObjMast = new BOMST_PrdType();
        DataTable DtabPara = new DataTable();


        #region Property Settings

        public FrmPrdType()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
           
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            BtnSave.Enabled = ObjPer.ISINSERT;

            DtabPara.Columns.Add("PRDTYPE_ID", typeof(System.Int32));
            DtabPara.Columns.Add("PRDTYPENAME", typeof(System.String));

            DtabPara.Columns.Add("ISKAPAN", typeof(System.Boolean));
            DtabPara.Columns.Add("ISPACKETNO", typeof(System.Boolean));
            DtabPara.Columns.Add("ISTAG", typeof(System.Boolean));
            DtabPara.Columns.Add("ISEMPLOYEE", typeof(System.Boolean));
            DtabPara.Columns.Add("ISMANAGER", typeof(System.Boolean));
            DtabPara.Columns.Add("ISGRAPH", typeof(System.Boolean));
            DtabPara.Columns.Add("ISEXP", typeof(System.Boolean));
            DtabPara.Columns.Add("ISMAK", typeof(System.Boolean));
            DtabPara.Columns.Add("ISPOL", typeof(System.Boolean));
            
            DtabPara.Columns.Add("ISACTIVE", typeof(System.Boolean));

            DtabPara.Columns.Add("REMARK", typeof(System.String));


            DataTable DTabPrdType = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PRDTYPE);
            RepCmbPrdType.DataSource = DTabPrdType;
            RepCmbPrdType.DisplayMember = "PRDTYPENAME";
            RepCmbPrdType.ValueMember = "PRDTYPE_ID";

            //#P : 25-01-2020
            

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

            var Result = from row in DtabPara.AsEnumerable()
                         where Val.ToString(row[ColName]).ToUpper() == Val.ToString(ColValue).ToUpper() && row.Table.Rows.IndexOf(row) != IntRowIndex
                         select row;


            if (Result.Any())
            {
                Global.Message(" " + StrMsg + " ALREADY EXISTS.");
                return true;
            }
            return false;
        }

        #region Validation

        private bool ValSave()
        {
            //if (txtItemGroupCode.Text.Trim().Length == 0)
            //{
            //    Global.Message("Group Code Is Required");
            //    txtItemGroupCode.Focus();
            //    return false;
            //}
            int IntCol = 0, IntRow = 0;
            foreach (DataRow dr in DtabPara.Rows)
            {
                //For Update Validation
                if (Val.ToString(dr["PRDTYPENAME"]).Trim().Equals(string.Empty) && !Val.ToString(dr["PRDTYPE_ID"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter Code");
                    IntCol = 0;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }
                //end as



                if (Val.ToString(dr["PRDTYPENAME"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter Name");
                    IntCol = 1;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }
               
            }
            if (IntRow > 0)
            {
                GrdDet.FocusedRowHandle = IntRow;
                GrdDet.FocusedColumn = GrdDet.VisibleColumns[IntCol];
                GrdDet.Focus();
                return true;
            }
            return false;
        }

        private bool ValDelete()
        {
            //if (txtItemGroupCode.Text.Trim().Length == 0)
            //{
            //    Global.Message("Group Code Is Required");
            //    txtItemGroupCode.Focus();
            //    return false;
            //}

            return true;
        }

        #endregion

        public void Clear()
        {
            DtabPara.Rows.Clear();
            DtabPara.Rows.Add(DtabPara.NewRow());
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            Clear();

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {

                //if (ValSave())
                //{
                //    return;
                //}

                //if (Global.Confirm("Are Your Sure To Save All Records ?") == System.Windows.Forms.DialogResult.No)
                //    return;

                string ReturnMessageDesc = "";
                string ReturnMessageType = "";


                foreach (DataRow Dr in DtabPara.GetChanges().Rows)
                {
                    MSTPrdTypeMasterProperty Property = new MSTPrdTypeMasterProperty();

                    if (Val.ToString(Dr["PRDTYPENAME"]).Trim().Equals(string.Empty))
                        continue;

                    Property.PRDTYPE_ID = Val.ToInt32(Dr["PRDTYPE_ID"]);
                    Property.PRDTYPENAME = Val.ToString(Dr["PRDTYPENAME"]);
                    Property.PRDTYPECODE = Val.ToString(Dr["PRDTYPECODE"]);

                    Property.SEQUENCENO = Val.ToInt(Dr["SEQUENCENO"]);
                    Property.REQPRDTYPE_ID = Val.Trim(Dr["REQPRDTYPE_ID"]);

                    Property.ISACTIVE = Val.ToBoolean(Dr["ISACTIVE"]);
                    Property.REMARK = Val.ToString(Dr["REMARK"]);

                    Property = ObjMast.Save(Property);

                    ReturnMessageDesc = Property.ReturnMessageDesc;
                    ReturnMessageType = Property.ReturnMessageType;
                    Dr["PRDTYPE_ID"] = Property.ReturnValue;

                    Property = null;
                }

                DtabPara.AcceptChanges();

                Global.Message(ReturnMessageDesc);

                if (ReturnMessageType == "SUCCESS")
                {
                    Fill();
                    //BtnAdd_Click(null, null);

                    if (GrdDet.RowCount > 1)
                    {
                        GrdDet.FocusedRowHandle = GrdDet.RowCount - 1;
                    }
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
            DtabPara = ObjMast.Fill();
            DtabPara.Rows.Add(DtabPara.NewRow());
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
            Global.ExcelExport("PredictionList", GrdDet);
        }

        private void repTxtRemark_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {


                    DataRow dr = GrdDet.GetFocusedDataRow();
                    if (!Val.ToString(dr["PRDTYPENAME"]).Equals(string.Empty) && GrdDet.IsLastRow)
                    {
                        DtabPara.Rows.Add(DtabPara.NewRow());
                        //DtabPara.AcceptChanges();

                    }
                    else if(GrdDet.IsLastRow)
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

        private void repTxtParaName_Validating(object sender, CancelEventArgs e)
        {
            DataRow Dr = GrdDet.GetFocusedDataRow();
            if (CheckDuplicate("PRDTYPENAME", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "Name"))
                e.Cancel = true;
            return;

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
                        if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                        {

                            MSTPrdTypeMasterProperty Property = new MSTPrdTypeMasterProperty();
                            DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                            Property.PRDTYPE_ID = Val.ToInt32(Drow["PRDTYPE_ID"]);
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
                        }
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
