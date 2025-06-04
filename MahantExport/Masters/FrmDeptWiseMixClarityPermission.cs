using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using System;
using MahantExport.Utility;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MahantExport.Masters
{
    public partial class FrmDeptWiseMixClarityPermission : Form
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        BOMST_DeptMixClarityPermission ObjMast = new BOMST_DeptMixClarityPermission();
        DataTable DTabMixClarity = new DataTable();
        public FrmDeptWiseMixClarityPermission()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            BtnSave.Enabled = ObjPer.ISINSERT;
            DataTable DtabDept = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_DEPARTMENT);
            CmbDepartment.DataSource = DtabDept;
            CmbDepartment.DisplayMember = "DEPARTMENTNAME";
            CmbDepartment.ValueMember = "DEPARTMENT_ID";
            CmbDepartment.SelectedIndex = -1;

            DataTable DtabMix = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_MIXCLARITY);
            repClarity.DataSource = DtabMix;
            repClarity.DisplayMember = "MIXCLARITYNAME";
            repClarity.ValueMember = "MIXCLARITY_ID";

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            //  BtnAdd_Click(null, null);
            Fill();
            this.Show();

        }
        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            // ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMast);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }
        public bool CheckDuplicate(string ColName, string ColValue, int IntRowIndex, string StrMsg)
        {
            if (Val.ToString(ColValue).Trim().Equals(string.Empty))
                return false;

            var Result = from row in DTabMixClarity.AsEnumerable()
                         where Val.ToString(row[ColName]).ToUpper() == Val.ToString(ColValue).ToUpper() && row.Table.Rows.IndexOf(row) != IntRowIndex
                         select row;


            if (Result.Any())
            {
                Global.Message(StrMsg + " ALREADY EXISTS.");
                return true;
            }
            return false;
        }

        private bool ValSave()
        {
            int IntCol = -1, IntRow = -1;
            //foreach (DataRow dr in DTabMixClarity.Rows)
            //{
            //    if (Val.ToString(dr["DEPARTMENT_ID"]).Trim().Equals(string.Empty))
            //    {
            //        Global.Message("Please Enter Department");
            //        IntCol = 1;
            //        IntRow = dr.Table.Rows.IndexOf(dr);
            //        break; 
            //    }
            //}



            if (IntRow >= 0)
            {
                GrdDet.FocusedRowHandle = IntRow;
                GrdDet.FocusedColumn = GrdDet.VisibleColumns[IntCol];
                GrdDet.Focus();
                return true;
            }
            return false;
        }

        public void Fill()
        {
            Guid gStrUser_ID = Val.ToString(txtUser_ID.Text).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtUser_ID.Tag));

            DTabMixClarity = ObjMast.Fill(Val.ToInt(CmbDepartment.SelectedValue), gStrUser_ID);
            DTabMixClarity.Rows.Add(DTabMixClarity.NewRow());
            MainGrid.DataSource = DTabMixClarity;
            MainGrid.Refresh();


        }
        private void txtSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "MIXSIZENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_MIXSIZE); ;
                    FrmSearch.mStrColumnsToHide = "MIXSIZE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDet.SetFocusedRowCellValue("MIXSIZENAME", (Val.ToString(FrmSearch.DRow["MIXSIZENAME"])));
                        GrdDet.SetFocusedRowCellValue("MIXSIZE_ID", (Val.ToInt32(FrmSearch.DRow["MIXSIZE_ID"])));
                        DataRow Dr = GrdDet.GetFocusedDataRow();

                        DTabMixClarity.AcceptChanges();
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
            }

            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
            if (CheckDuplicate("MIXSIZENAME", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "MIXSIZENAME"))
            {
                //e.Cancel = true;
                GrdDet.SetFocusedRowCellValue("MIXSIZENAME", string.Empty);
                GrdDet.SetFocusedRowCellValue("MIXSIZE_ID", 0);

            }
        }
        public void FetchValue(DataRow DR)
        {
        }

        private void GrdDet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataRow DR = GrdDet.GetFocusedDataRow();
                FetchValue(DR);
                DR = null;
            }
        }

        private void GrdDet_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.RowHandle < 1)
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

        private void repTxrRemark_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DataRow dr = GrdDet.GetFocusedDataRow();
                    if (!Val.ToString(dr["MIXSIZENAME"]).Equals(string.Empty) && GrdDet.IsLastRow)
                    {
                        DTabMixClarity.Rows.Add(DTabMixClarity.NewRow());
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
        public void Clear()
        {
            DTabMixClarity.Rows.Clear();
            DTabMixClarity.Rows.Add(DTabMixClarity.NewRow());

        }

        private void CmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Clear();
            //Fill();
            DTabMixClarity.Rows.Clear();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {

            Clear();

        }

        private void BtnExport_Click(object sender, EventArgs e)
        {

            Global.ExcelExport(CmbDepartment.SelectedItem.ToString() + "List", GrdDet);

        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (ValSave())
                {
                    return;
                }



                if (DTabMixClarity.Rows.Count <= 0)
                {
                    Global.MessageError("No Data Found ");
                    return;
                }


                string ReturnMessageDesc = "";
                string ReturnMessageType = "";

                foreach (DataRow Dr in DTabMixClarity.Rows)
                {
                    DeptWiseMixClaPermissionProperty Property = new DeptWiseMixClaPermissionProperty();

                    Property.DEPARTMENT_ID = Val.ToInt32(CmbDepartment.SelectedValue);
                    Property.USER_ID = Val.ToGuid(txtUser_ID.Tag);

                    if (Val.ToString(Dr["MIXSIZE_ID"]).Trim().Equals(string.Empty) || Val.ToString(Dr["MIXCLARITY_ID"]).Trim().Equals(string.Empty))
                        continue;

                    Property.MIXSIZE_ID = Val.ToInt32(Dr["MIXSIZE_ID"]);
                    Property.PERMISSION_ID = Val.ToInt32(Dr["PERMISSION_ID"]);
                    Property.MIXCLARITY_ID = Val.ToString(Dr["MIXCLARITY_ID"]);
                    Property.REMARK = Val.ToString(Dr["REMARK"]);
                    Property.ISACTIVE = Val.ToBoolean(Dr["ISACTIVE"]);

                    Property = ObjMast.Save(Property);
                    ReturnMessageDesc = Property.RETURNMESSAGEDESC;
                    ReturnMessageType = Property.RETURNMESSAGETYPE;

                    Property = null;
                }
                DTabMixClarity.AcceptChanges();

                Global.Message(ReturnMessageDesc);


                if (ReturnMessageType == "SUCCESS")
                {
                    Fill();
                    //if (GrdDet.RowCount > 1)
                    //{

                    //    GrdDet.FocusedRowHandle = GrdDet.RowCount - 1;
                    //}
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void deleteSelectedItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle >= 0)
                {
                    if (Global.Confirm("ARE YOU SURE YOU WANT TO DELETE ENTRY") == System.Windows.Forms.DialogResult.Yes)
                    {
                        DeptWiseMixClaPermissionProperty Property = new DeptWiseMixClaPermissionProperty();

                        // FrmPassword FrmPassword = new FrmPassword();
                        // if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                        {
                            DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                            Property.PERMISSION_ID = Val.ToInt32(Drow["PERMISSION_ID"]);
                            Property = ObjMast.Delete(Property);

                            if (Property.RETURNMESSAGETYPE == "SUCCESS")
                            {
                                Global.Message("ENTRY DELETED SUCCESSFULLY");
                                DTabMixClarity.Rows.RemoveAt(GrdDet.FocusedRowHandle);
                                DTabMixClarity.AcceptChanges();
                                Fill();
                            }
                            else
                            {
                                Global.Message("ERROR IN DELETE ENTRY");
                            }
                        }
                        DataRow Dr = GrdDet.GetFocusedDataRow();

                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }

        private void txtUser_ID_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "EMPLOYEECODE,EMPLOYEENAME";
                    FrmSearch.mStrColumnsToHide = "EMPLOYEE_ID,,DEPARTMENT_ID,DEPARTMENTNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_EMPLOYEE);
                    // FrmSearch.ColumnsToHide = "EMPLOYEE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtUser_ID.Text = Val.ToString(FrmSearch.DRow["EMPLOYEENAME"]);
                        txtUser_ID.Tag = Val.ToString(FrmSearch.DRow["EMPLOYEE_ID"]);
                        DTabMixClarity.Rows.Clear();
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

        private void BtnShow_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
                Fill();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        //private void txtSize_Validating(object sender, CancelEventArgs e)
        //{
        //    DataRow Dr = GrdDet.GetFocusedDataRow();
        //    if (CheckDuplicate("MIXSIZENAME", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "MIXSIZENAME"))
        //    {
        //        e.Cancel = true;
        //        return;
        //    }
        //}



        //private void txtSize_Validating(object sender, CancelEventArgs e)
        //{
        //    DataRow Dr = GrdDet.GetFocusedDataRow();
        //    if (CheckDuplicate("MIXSIZENAME", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "MIXSIZENAME"))
        //    {
        //        e.Cancel = true;
        //        return;
        //    }
        //    else if (Val.ToString(GrdDet.EditingValue).Trim().Equals(String.Empty))
        //    {
        //        GrdDet.EditingValue = Val.ToString(Dr["MIXSIZENAME"]);
        //    }
        //}
    }
}
