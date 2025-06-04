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

namespace MahantExport.Masters
{
    public partial class FrmEmployeeRights : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_FormPermission ObjMast = new BOMST_FormPermission();

        DataTable DTabForm = new DataTable();
        DataTable DTabDisplay = new DataTable();
        DataTable DTabTransfer = new DataTable();


        #region Property Settings

        public FrmEmployeeRights()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
           
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            CmbProcess.Properties.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PROCESSALL);
            CmbProcess.Properties.DisplayMember = "PROCESSNAME";
            CmbProcess.Properties.ValueMember = "PROCESS_ID";

            this.Show();
            txtEmployee.Focus();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyPress = false;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMast);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        #endregion

        private void txtEmployee_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "EMPLOYEECODE,EMPLOYEENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_EMPLOYEE);
                    FrmSearch.mStrColumnsToHide = "EMPLOYEE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtEmployee.Text = Val.ToString(FrmSearch.DRow["EMPLOYEENAME"]);
                        txtEmployee.Tag = Val.ToString(FrmSearch.DRow["EMPLOYEE_ID"]);
                        BtnShow_Click(null, null);
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
            DTabForm.Rows.Clear();
            DTabDisplay.Rows.Clear();
            DTabTransfer.Rows.Clear();
            Fill(Val.ToString(txtEmployee.Tag));
        }

        public void Fill(string pIntEmployeeID)
        {
            if (pIntEmployeeID == "")
            {
                Global.Message("Employee ID Required");
                txtEmployee.Focus();
                return;
            }

            this.Cursor = Cursors.WaitCursor;
            DataSet DS =  ObjMast.Fill(pIntEmployeeID);

            DTabForm = DS.Tables[0];
            MainGridForm.DataSource = DTabForm;
            GrdDetForm.RefreshData();

            ChkCostPrice.Checked = Val.ToBoolean(DS.Tables[1].Rows[0]["ISDISPLAYCOSTPRICE"]);
            ChkPurParty.Checked = Val.ToBoolean(DS.Tables[1].Rows[0]["ISDISPLAYPURPARTY"]);
            chkShowAllParty.Checked = Val.ToBoolean(DS.Tables[1].Rows[0]["ISDISPLAYALLPARTY"]);
            ChkShowAllOrder.Checked = Val.ToBoolean(DS.Tables[1].Rows[0]["ISDISPLAYALLORDER"]);
            ChkIsAllMfgCost.Checked = Val.ToBoolean(DS.Tables[1].Rows[0]["ISDISPLAYALLMFGCOST"]);
            ChkComputerPrice.Checked = Val.ToBoolean(DS.Tables[1].Rows[0]["ISCOMPUTERPRICE"]);
            CmbProcess.SetEditValue(Val.ToString(DS.Tables[1].Rows[0]["PROCESS_ID"]));

            this.Cursor = Cursors.Default;

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtEmployee.Text.Trim().Length == 0)
                {
                    Global.Message("Employee Name Is Required");
                    txtEmployee.Focus();
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                DTabForm.AcceptChanges();
                DTabDisplay.AcceptChanges();
                DTabTransfer.AcceptChanges();

                int IntRes = 0;
IntRes = ObjMast.Save(Val.ToString(txtEmployee.Tag), DTabForm, DTabDisplay, DTabTransfer, ChkCostPrice.Checked, ChkPurParty.Checked, chkShowAllParty.Checked, ChkShowAllOrder.Checked, Val.Trim(CmbProcess.Properties.GetCheckedItems()), ChkIsAllMfgCost.Checked, ChkComputerPrice.Checked);
               

                if (IntRes != -1)
                {
                    Global.Message("SUCCESSFULLY SAVED RIGHTS");
                    //BtnAdd_Click(null, null);
                }
                else
                {
                    Global.Message("OOPS SOMETHING GOES WRONG");
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCopyFrom_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "EMPLOYEECODE,EMPLOYEENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_EMPLOYEE);
                    FrmSearch.mStrColumnsToHide = "EMPLOYEE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtCopyFrom.Text = Val.ToString(FrmSearch.DRow["EMPLOYEENAME"]);
                        txtCopyFrom.Tag = Val.ToString(FrmSearch.DRow["EMPLOYEE_ID"]);

                        DTabForm.Rows.Clear();
                        DTabDisplay.Rows.Clear();
                        DTabTransfer.Rows.Clear();
                        Fill(Val.ToString(txtCopyFrom.Tag));
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
            ChkCostPrice.Checked = false;
            DTabForm.Rows.Clear();
            DTabDisplay.Rows.Clear();
            DTabTransfer.Rows.Clear();
            txtEmployee.Text = "";
            txtEmployee.Tag = "";
            txtCopyFrom.Text = "";
            txtCopyFrom.Tag = "";
            txtEmployee.Focus();
            ChkShowAllOrder.Checked = false;
            chkShowAllParty.Checked = false;


        }

        private void chkAllView_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataRow DR in DTabForm.Rows)
            {
                DR["ISVIEW"] = chkAllView.Checked;
            }
        }

        private void ChkAllInsert_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataRow DR in DTabForm.Rows)
            {
                DR["ISINSERT"] = ChkAllInsert.Checked;
            }
        }

        private void ChkAllUpdate_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataRow DR in DTabForm.Rows)
            {
                DR["ISUPDATE"] = ChkAllUpdate.Checked;
            }
        }

        private void ChkDelete_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataRow DR in DTabForm.Rows)
            {
                DR["ISDELETE"] = ChkDelete.Checked;
            }
        }

        private void BtnSaveLayoutRights_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtEmployee.Text.Trim().Length == 0)
                {
                    Global.Message("Employee Name Is Required");
                    txtEmployee.Focus();
                    return;
                }
                if (txtCopyFrom.Text.Trim().Length == 0)
                {
                    Global.Message("Employee Name Is Required");
                    txtEmployee.Focus();
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                int IntRes = 0;
                IntRes = ObjMast.SaveLayoutRights(Val.ToString(txtEmployee.Tag), Val.ToString(txtCopyFrom.Tag));


                if (IntRes != -1)
                {
                    Global.Message("SUCCESSFULLY SAVED RIGHTS");
                    //BtnAdd_Click(null, null);
                }
                else
                {
                    Global.Message("OOPS SOMETHING GOES WRONG");
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
    }
}
