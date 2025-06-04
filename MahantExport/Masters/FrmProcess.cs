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
    public partial class FrmProcess : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_Process ObjMast = new BOMST_Process();
        DataTable DtabProcess = new DataTable();
        string StrMesg = "";

        #region Property Settings

        public FrmProcess()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
           // BtnSave.Enabled = ObjPer.ISINSERT;
            deleteSelectedAmountToolStripMenuItem.Enabled = ObjPer.ISDELETE;

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();

            Clear();

            DataTable DTabPrevProcess = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PROCESS);
            repChkCmbPrevProcess.DataSource = DTabPrevProcess;
            repChkCmbPrevProcess.DisplayMember = "PROCESSNAME";
            repChkCmbPrevProcess.ValueMember = "PROCESS_ID";

        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            //ObjFormEvent.FormKeyDown = true;
           // ObjFormEvent.FormKeyPress = true;
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

            var Result = from row in DtabProcess.AsEnumerable()
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
            foreach (DataRow dr in DtabProcess.Rows)
            {
                //For Update Validation
                if (Val.ToString(dr["PROCESSCODE"]).Trim().Equals(string.Empty) && !Val.ToString(dr["PROCESS_ID"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter 'Process Code'");
                    IntCol = 0;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }
                //end as

                if (Val.ToString(dr["PROCESSCODE"]).Trim().Equals(string.Empty))
                {
                    if (DtabProcess.Rows.Count == 1)
                    {
                        Global.Message("Please Enter 'Process Code'");
                        IntCol = 0;
                        IntRow = dr.Table.Rows.IndexOf(dr);
                        break;

                    }
                    else
                        continue;
                }

                if (Val.ToString(dr["PROCESSNAME"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter 'Process Name'");
                    IntCol = 1;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }
               
            }
            if (IntRow >= 0)
            {
                GrdDet.FocusedRowHandle = IntRow;
                GrdDet.FocusedColumn = GrdDet.VisibleColumns[IntCol];
                GrdDet.Focus();
                GrdDet.ShowEditor();
                return true;
            }
            return false;
        }

        #endregion

        public void Clear()
        {
            Fill();
            DtabProcess.Rows.Add(DtabProcess.NewRow());

            GrdDet.FocusedRowHandle = 0;
            GrdDet.FocusedColumn = GrdDet.VisibleColumns[0];
            GrdDet.Focus();
            GrdDet.ShowEditor();
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

                foreach (DataRow Dr in DtabProcess.Rows)
                {
                    ProcessMasterProperty Property = new ProcessMasterProperty();
                
                    if (Val.ToString(Dr["PROCESSCODE"]).Trim().Equals(string.Empty) || Val.ToString(Dr["PROCESSNAME"]).Trim().Equals(string.Empty))
                        continue;

                    Property.PROCESS_ID = Val.ToInt32(Dr["PROCESS_ID"]);
                    Property.PROCESSCODE = Val.ToString(Dr["PROCESSCODE"]);
                    Property.PROCESSNAME = Val.ToString(Dr["PROCESSNAME"]);
                    Property.JANGEDPREFIX = Val.ToString(Dr["JANGEDPREFIX"]);
                    Property.PRINTHEADER = Val.ToString(Dr["PRINTHEADER"]);
                    Property.BILLDATEPREFIX = Val.ToString(Dr["BILLDATEPREFIX"]);
                    Property.BILLNOPREFIX = Val.ToString(Dr["BILLNOPREFIX"]);
                    Property.SEQUENCENO = Val.ToInt(Dr["SEQUENCENO"]);
                    
                    Property.PREVPROCESS_ID = Val.ToString(Val.Trim(Dr["PREVPROCESS_ID"]));
                    Property.ISACTIVE = Val.ToBoolean(Dr["ISACTIVE"]);
                    Property.ISACTIVESTATUS = Val.ToBoolean(Dr["ISACTIVESTATUS"]);

                    Property.STOCKSTATUS = Val.ToString(Dr["STOCKSTATUS"]);
                    Property.WEBSTATUS = Val.ToString(Dr["WEBSTATUS"]);
                    
                    Property.REMARK = Val.ToString(Dr["REMARK"]);

                    Property = ObjMast.Save(Property);

                    ReturnMessageDesc = Property.ReturnMessageDesc;
                    ReturnMessageType = Property.ReturnMessageType;

                    Property = null;
                }
                DtabProcess.AcceptChanges();

                Global.Message(ReturnMessageDesc);

                if (ReturnMessageType == "SUCCESS")
                {
                    Clear();

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
            DtabProcess = ObjMast.Fill();
            MainGrid.DataSource = DtabProcess;
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
            Global.ExcelExport("Process List", GrdDet);
        }

        private void deleteSelectedAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle >= 0)
                {
                    if (Global.Confirm("ARE YOU SURE YOU WANT TO DELETE ENTRY") == System.Windows.Forms.DialogResult.Yes)
                    {
                        ProcessMasterProperty Property = new ProcessMasterProperty();

                       FrmPassword FrmPassword = new FrmPassword();
                       if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                       {

                           DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                           Property.PROCESS_ID = Val.ToInt32(Drow["PROCESS_ID"]);
                           Property = ObjMast.Delete(Property);

                           if (Property.ReturnMessageType == "SUCCESS")
                           {
                               Global.Message("ENTRY DELETED SUCCESSFULLY");
                               DtabProcess.Rows.RemoveAt(GrdDet.FocusedRowHandle);
                               DtabProcess.AcceptChanges();
                               Clear();
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

        private void repTxrRemark_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DataRow dr = GrdDet.GetFocusedDataRow();
                    if (!Val.ToString(dr["PROCESSCODE"]).Equals(string.Empty) && !Val.ToString(dr["PROCESSNAME"]).Equals(string.Empty) && GrdDet.IsLastRow)
                    {
                        DtabProcess.Rows.Add(DtabProcess.NewRow());
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

        private void repTxtProcessName_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle < 0)
                    return;

                DataRow Dr = GrdDet.GetFocusedDataRow();
                if (CheckDuplicate("PROCESSNAME", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "Process Name"))
                    e.Cancel = true;
                return;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void repTxtPrevProcessName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle < 0)
                    return;

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PROCESSNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PROCESS);
                    FrmSearch.mStrColumnsToHide = "PROCESS_ID,STATUSCODE";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDet.PostEditor();
                        DataRow Dr = GrdDet.GetFocusedDataRow();

                        GrdDet.SetFocusedRowCellValue("PREVPROCESSNAME", Val.ToString(FrmSearch.DRow["PROCESSNAME"]));
                        GrdDet.SetFocusedRowCellValue("PREVPROCESS_ID", Val.ToString(FrmSearch.DRow["PROCESS_ID"]));
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
        }

        private void repTxtProcessCode_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle < 0)
                    return;

                DataRow Dr = GrdDet.GetFocusedDataRow();
                if (CheckDuplicate("PROCESSCODE", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "Process Code"))
                    e.Cancel = true;
                return;
            }
            catch(Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
     
    }
}
