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
    public partial class FrmParameter : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_Parameter ObjMast = new BOMST_Parameter();
        DataTable DtabPara = new DataTable();

        string StrMesg = "";


        #region Property Settings

        public FrmParameter()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {

           
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            DataTable DtabSize = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_DEPARTMENT);
            rpChkCmbClarityWiseDept.DataSource = DtabSize;
            rpChkCmbClarityWiseDept.DisplayMember = "DEPARTMENTNAME";
            rpChkCmbClarityWiseDept.ValueMember = "DEPARTMENT_ID";

            BtnAdd_Click(null, null);
            Fill();
            this.Show();
            
            CmbParameterType.SelectedIndex = 0;
        }

        public void ShowForm(string pStrParaType)
        {

            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            BtnSave.Enabled = ObjPer.ISINSERT;
            deleteSelectedAmountToolStripMenuItem.Enabled = ObjPer.ISDELETE;

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();


            DataTable DtabSize = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_DEPARTMENT);
            rpChkCmbClarityWiseDept.DataSource = DtabSize;
            rpChkCmbClarityWiseDept.DisplayMember = "DEPARTMENTNAME";
            rpChkCmbClarityWiseDept.ValueMember = "DEPARTMENT_ID";

            BtnAdd_Click(null, null);
            Fill();
            this.Show();

            CmbParameterType.SelectedItem = pStrParaType;
            CmbParameterType.Enabled = false;
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

        public bool CheckDuplicate(string ColName, string ColValue, int IntRowIndex, string StrMsg, string ColName2, string ColValue2)
        {
            if (Val.ToString(ColValue).Trim().Equals(string.Empty))
                return false;

            var Result = from row in DtabPara.AsEnumerable()
                         where Val.ToString(row[ColName]).ToUpper() == Val.ToString(ColValue).ToUpper() && row.Table.Rows.IndexOf(row) != IntRowIndex && Val.ToString(row[ColName2]).ToUpper() == Val.ToString(ColValue2)
                         select row;


            if (Result.Any())
            {
                if (Val.ToString(CmbParameterType.SelectedItem) == "COLOR" || Val.ToString(CmbParameterType.SelectedItem) == "CLARITY")
                    Global.Message("'" + CmbParameterType.SelectedItem + " " + StrMsg + "' ALREADY EXISTS IN '" + Val.ToString(ColValue2) + "' TYPE.");
                else
                    Global.Message("'" + CmbParameterType.SelectedItem + " " + StrMsg + "' ALREADY EXISTS.");
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
            int IntCol = -1, IntRow = -1;
            foreach (DataRow dr in DtabPara.Rows)
            {
                //For Update Validation
                if (Val.ToString(dr["PARACODE"]).Trim().Equals(string.Empty) && !Val.ToString(dr["PARA_ID"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter '" + CmbParameterType.SelectedItem + " Code'");
                    IntCol = 0;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }
                //end as


                if (Val.ToString(dr["PARACODE"]).Trim().Equals(string.Empty))
                {
                    if (DtabPara.Rows.Count == 1)
                    {
                        Global.Message("Please Enter '" + CmbParameterType.SelectedItem + " Code'");
                        IntCol = 0;
                        IntRow = dr.Table.Rows.IndexOf(dr);
                        break;

                    }
                    else
                        continue;
                }
                if (Val.ToString(dr["PARANAME"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter '" + CmbParameterType.SelectedItem + " Name'");
                    IntCol = 1;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }
                if (Val.ToString(dr["PARAGROUP"]).Trim().Equals(string.Empty) && (Val.ToString(CmbParameterType.SelectedItem) == "COLOR" || Val.ToString(CmbParameterType.SelectedItem) == "CLARITY"))
                {
                    Global.Message("Please Select  'Group'");
                    IntCol = 2;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
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

                //if (Global.Confirm("Are Your Sure To Save All Records ?") == System.Windows.Forms.DialogResult.No)
                //    return;

                string ReturnMessageDesc = "";
                string ReturnMessageType = "";
                DataTable DTabChange = DtabPara.GetChanges();
                if (DTabChange != null)
                {

                    foreach (DataRow Dr in DTabChange.Rows)
                    {
                        ParameterMasterProperty Property = new ParameterMasterProperty();

                        //Property.ITEMGROUP_ID = Val.ToInt64(txtParaType.Text);
                        Property.PARATYPE = Val.ToString(CmbParameterType.SelectedItem);

                        if (Val.ToString(Dr["PARACODE"]).Trim().Equals(string.Empty) || Val.ToString(Dr["PARANAME"]).Trim().Equals(string.Empty))
                            continue;

                        Property.PARA_ID = Val.ToInt32(Dr["PARA_ID"]);
                        Property.PARACODE = Val.ToString(Dr["PARACODE"]);
                        Property.PARANAME = Val.ToString(Dr["PARANAME"]);
                        Property.PARAGROUP = Val.ToString(Dr["PARAGROUP"]);
                        Property.RAPAVALUE = Val.ToString(Dr["RAPAVALUE"]);
                        Property.SHORTNAME = Val.ToString(Dr["SHORTNAME"]);
                        Property.WEBDISPLAY = Val.ToString(Dr["WEBDISPLAY"]);

                        Property.RAPNETUPLOADCODE = Val.ToString(Dr["RAPNETUPLOADCODE"]);
                        Property.SEQUENCENO = Val.ToInt32(Dr["SEQUENCENO"]);
                        Property.LABCODE = Val.ToString(Dr["LABCODE"]);
                        Property.ISACTIVE = Val.ToBoolean(Dr["ISACTIVE"]);
                        Property.REMARK = Val.ToString(Dr["REMARK"]);
                        Property.DEPT_ID = Val.ToInt32(Dr["DEPT_ID"]);

                        Property.NIKUNJSEQNO = Val.ToInt32(Dr["NIKUNJSEQNO"]);
                        Property.ANKITSEQNO = Val.ToInt32(Dr["ANKITSEQNO"]);

                        Property.JACCODE = Val.ToString(Dr["JACCODE"]); //hinal

                        // #D: 13-01-2021
                        Property.IMAGEPATH = Val.ToString(Dr["IMAGEPATH"]);
                        Property.SELIMAGEPATH = Val.ToString(Dr["SELIMAGEPATH"]);
                        // #D: 13-01-2021
                        Property.CLARITYWISEDEPARTMENT_ID = Val.Trim(Dr["CLARITYWISEDEPARTMENT_ID"]);

                        //#S 28-04-2022
                        Property.ACCCODE = Val.ToString(Dr["ACC_CODE"]);
                        //#S 16-07-2022
                        Property.ACCDESC = Val.ToString(Dr["ACC_DESC"]);
                        //Property.EXCELNAME = Val.ToString(Dr["EXCELNAME"]);

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
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        public void Fill()
        {
            DtabPara = ObjMast.Fill(Val.ToString(CmbParameterType.SelectedItem));
            
            DataRow Dr = DtabPara.NewRow();
            if (Val.ToString(CmbParameterType.SelectedItem) == "COLOR" || Val.ToString(CmbParameterType.SelectedItem) == "CLARITY")
                Dr["PARAGROUP"] = "SINGLE";
            else
                Dr["PARAGROUP"] = "";
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
            Global.ExcelExport(CmbParameterType.SelectedItem.ToString() + "List", GrdDet);
        }

        private void repTxtRemark_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {


                    DataRow dr = GrdDet.GetFocusedDataRow();
                    if (!Val.ToString(dr["PARACODE"]).Equals(string.Empty) && !Val.ToString(dr["PARANAME"]).Equals(string.Empty) && GrdDet.IsLastRow)
                    {
                        DataRow Dr = DtabPara.NewRow();
                        if (Val.ToString(CmbParameterType.SelectedItem) == "COLOR" || Val.ToString(CmbParameterType.SelectedItem) == "CLARITY")
                            Dr["PARAGROUP"] = "SINGLE";
                        else
                            Dr["PARAGROUP"] = "";

                        DtabPara.Rows.Add(Dr);
                        //DtabPara.AcceptChanges();

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

        private void repTxtParaName_Validating(object sender, CancelEventArgs e)
        {
            if (GrdDet.FocusedRowHandle < 0)
                return;

            DataRow Dr = GrdDet.GetFocusedDataRow();
            if (CheckDuplicate("PARANAME", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "Name", "PARAGROUP", Val.ToString(Dr["PARAGROUP"])))
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
                            ParameterMasterProperty Property = new ParameterMasterProperty();
                            DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                            Property.PARA_ID = Val.ToInt32(Drow["PARA_ID"]);
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

        private void repTxtParaCode_Validating(object sender, CancelEventArgs e)
        {
            if (GrdDet.FocusedRowHandle < 0)
                return;

            DataRow Dr = GrdDet.GetFocusedDataRow();
            if (CheckDuplicate("PARACODE", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "Code", "PARAGROUP", Val.ToString(Dr["PARAGROUP"])))
                e.Cancel = true;
            return;

        }

        private void CmbParameterType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear();
            Fill();

            if (Val.ToString(CmbParameterType.SelectedItem) == "COLOR" || Val.ToString(CmbParameterType.SelectedItem) == "CLARITY")
            {
                GrdDet.Columns["PARAGROUP"].Visible = true;
            }
            else
            {
                GrdDet.Columns["PARAGROUP"].Visible = false;
            }
            if (Val.ToString(CmbParameterType.SelectedItem) == "MIX SIZE")
            {
                GrdDet.Columns["DEPT_NAME"].Visible = true;
            }
            else
            {
                GrdDet.Columns["DEPT_NAME"].Visible = false;
            }
            //hinal
            if (Val.ToString(CmbParameterType.SelectedItem) == "SHAPE" || Val.ToString(CmbParameterType.SelectedItem) == "CLARITY" || Val.ToString(CmbParameterType.SelectedItem) == "MIX_CLARITY" || Val.ToString(CmbParameterType.SelectedItem) == "CUT" || Val.ToString(CmbParameterType.SelectedItem) == "COLOR" || Val.ToString(CmbParameterType.SelectedItem) == "POLISH" || Val.ToString(CmbParameterType.SelectedItem) == "SYMMETRY" || Val.ToString(CmbParameterType.SelectedItem) == "FLUORESCENCE")
            {
                GrdDet.Columns["JACCODE"].Visible = true;
            }
            else
            {
                GrdDet.Columns["JACCODE"].Visible = false;
            }
            if (Val.ToString(CmbParameterType.SelectedItem) == "MIX_CLARITY")
            {
                GrdDet.Columns["CLARITYWISEDEPARTMENT_ID"].Visible = true;
                GrdDet.Columns["NIKUNJSEQNO"].Visible = true;
                GrdDet.Columns["ANKITSEQNO"].Visible = true;
            }
            else
            {
                GrdDet.Columns["CLARITYWISEDEPARTMENT_ID"].Visible = false;
                GrdDet.Columns["NIKUNJSEQNO"].Visible = false;
                GrdDet.Columns["ANKITSEQNO"].Visible = false;
            }

            // DHARA : 01-01-2024
            if (Val.ToString(CmbParameterType.SelectedItem) == "LAB" )
            {
                GrdDet.Columns["EXCELNAME"].Visible = true;
            }
            else
            {
                GrdDet.Columns["EXCELNAME"].Visible = false;
            }

            // DHARA : 01-01-2024
        }

        private void rpTxtDeptName_KeyPress(object sender, KeyPressEventArgs e)
        {

            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "DEPARTMENTNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;

                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_DEPARTMENT);
                    FrmSearch.mStrColumnsToHide = "DEPARTMENT_ID,DEPARTMENTCODE";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;

                    if (FrmSearch.DRow != null)
                    {
                        GrdDet.SetFocusedRowCellValue("DEPT_ID", Val.ToString(FrmSearch.DRow["DEPARTMENT_ID"]));
                        GrdDet.SetFocusedRowCellValue("DEPT_NAME", Val.ToString(FrmSearch.DRow["DEPARTMENTNAME"]));
                    }

                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;

                }
            }
            catch { }
        }

    }
}
