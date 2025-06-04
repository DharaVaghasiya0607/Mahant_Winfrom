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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MahantExport.Parcel
{
    public partial class FrmMixSize : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOMST_Size ObjSize = new BOMST_Size();

        DataTable DtabSize = new DataTable();

        public FrmMixSize()
        {
            InitializeComponent();

        }
        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            if (ObjPer.ISVIEW == false)
            {
                Global.MessageError(BusLib.TPV.BOMessage.ViewDeniedMsg);
                return;
            }

            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            BtnSave.Enabled = ObjPer.ISINSERT;
            DeleteSelectedSizeToolStripMenuItem.Enabled = ObjPer.ISDELETE;

            DataTable DtabSize = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_DEPARTMENT);
            repCmbSizeWiseDept.DataSource = DtabSize;
            repCmbSizeWiseDept.DisplayMember = "DEPARTMENTNAME";
            repCmbSizeWiseDept.ValueMember = "DEPARTMENT_ID";

            Fill();
            this.Show();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjSize);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        public bool CheckDuplicate(string ColName, string ColValue, int IntRowIndex, string StrMsg)
        {
            if (Val.ToString(ColValue).Trim().Equals(string.Empty))
                return false;

            var Result = from row in DtabSize.AsEnumerable()
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


        #endregion

        public void Clear()
        {
            DtabSize.Rows.Clear();
            DtabSize.Rows.Add(DtabSize.NewRow());
            Fill();
        }

        public void Fill()
        {
            DtabSize = ObjSize.FillMix();
            DtabSize.Rows.Add(DtabSize.NewRow());
            MainGrid.DataSource = DtabSize;
            MainGrid.Refresh();

        }


        private void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
               
                string ReturnMessageDesc = "";
                string ReturnMessageType = "";

                foreach (DataRow Dr in DtabSize.GetChanges().Rows)
                {
                    SizeMasterProperty Property = new SizeMasterProperty();

                    if (Val.ToString(Dr["SIZENAME"]).Length == 0) //|| Val.Val(Dr["TOCARAT"]) == 0
                        continue;

                    Property.SIZE_ID = Val.ToInt32(Dr["SIZE_ID"]);
                    Property.SIZENAME = Val.ToString(Dr["SIZENAME"]);
                    Property.FROMCARAT = 0.001;
                    Property.TOCARAT = 999;
                    Property.ISACTIVE = Val.ToBoolean(Dr["ISACTIVE"]);
                    Property.REMARK = Val.ToString(Dr["REMARK"]);
                    Property.SEQUENCENO = Val.ToInt32(Dr["SEQUENCENO"]);
                    Property.DEPARTMENT_ID = Val.ToInt32(Dr["DEPARTMENT_ID"]);
                    Property.SIZEWISEDEPARTMENT_ID = Val.Trim(Dr["SIZEWISEDEPARTMENT_ID"]);
                    Property = ObjSize.SaveMix(Property);
                    
                    ReturnMessageDesc = Property.ReturnMessageDesc;
                    ReturnMessageType = Property.ReturnMessageType;

                    Dr["SIZE_ID"] = Property.ReturnValue;
                    Property = null;
                }
                DtabSize.AcceptChanges();

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
                    //txtItemGroupCode.Focus();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void reptxtremark_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DataRow dr = GrdDet.GetFocusedDataRow();
                    if (Val.ToString(dr["SIZENAME"]).Length != 0 && GrdDet.IsLastRow)
                    {
                        DtabSize.Rows.Add(DtabSize.NewRow());

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

        private void reptxtName_Validating(object sender, CancelEventArgs e)
        {
            DataRow Dr = GrdDet.GetFocusedDataRow();
            if (CheckDuplicate("SIZENAME", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "SIZENAME"))
            {
                e.Cancel = true;
                return;
            }
            else if (Val.ToString(GrdDet.EditingValue).Trim().Equals(String.Empty))
            {
                GrdDet.EditingValue = Val.ToString(Dr["FROMCARAT"]) + "-" + Val.ToString(Dr["TOCARAT"]);
            }
            int IntRowNo = GrdDet.GetFocusedDataSourceRowIndex();
        }

        private void reptxtFromCarat_Validating(object sender, CancelEventArgs e)
        {
            GrdDet.PostEditor();
            DataRow Dr = GrdDet.GetFocusedDataRow();
            if (CheckDuplicate("FROMCARAT", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "FROMCARAT"))
            {
                e.Cancel = true;
                return;
            }
            //else
            //{
            //    Dr["SIZENAME"] =  Val.ToString(GrdDet.EditingValue) +  "-" + Val.ToString(Dr["TOCARAT"])  ;
            //    DtabSize.AcceptChanges();

            //}
            if (Val.ToDecimal(Dr["TOCARAT"]) != 0)
            {
                if (Val.ToDecimal(GrdDet.EditingValue) > Val.ToDecimal(Dr["TOCARAT"]))
                {
                    Global.Message("From Carat must be Greter Than To Carat");
                    e.Cancel = true;
                    return;
                }
            }

            var dValue = from row in DtabSize.AsEnumerable()
                         where Val.Val(row["FROMCARAT"]) <= Val.Val(GrdDet.EditingValue) && Val.Val(row["TOCARAT"]) >= Val.Val(GrdDet.EditingValue) && row.Table.Rows.IndexOf(row) != GrdDet.FocusedRowHandle
                         select row;

            if (dValue.Any())
            {
                Global.Message("This Value Already Exist Between Some From Carat and To Carat Please Check.!");
                e.Cancel = true;
                return;
            }

        }

        private void reptxtToCarat_Validating(object sender, CancelEventArgs e)
        {
            DataRow Dr = GrdDet.GetFocusedDataRow();
            if (CheckDuplicate("TOCARAT", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "TOCARAT"))
            {
                e.Cancel = true;
                return;
            }
            if (Val.ToDecimal(Dr["FROMCARAT"]) > Val.ToDecimal(GrdDet.EditingValue))
            {
                Global.Message("To Carat must be Greter Than From Carat");
                e.Cancel = true;
                return;
            }
            //else
            //{
            //        Dr["SIZENAME"] = Val.ToString(Dr["FROMCARAT"]) + "-"+ Val.ToString(GrdDet.EditingValue) ;
            //        DtabSize.AcceptChanges();

            //}

            var dValue = from row in DtabSize.AsEnumerable()
                         where Val.Val(row["FROMCARAT"]) <= Val.Val(GrdDet.EditingValue) && Val.Val(row["TOCARAT"]) >= Val.Val(GrdDet.EditingValue) && row.Table.Rows.IndexOf(row) != GrdDet.FocusedRowHandle
                         select row;

            if (dValue.Any())
            {
                Global.Message("This Value Already Exist Between Some From Carat and To Carat Please Check.!");
                e.Cancel = true;
                return;
            }

        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("Size List", GrdDet);
        }

        private void DeleteSelectedSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle >= 0)
                {
                    if (Global.Confirm("ARE YOU SURE YOU WANT TO DELETE ENTRY ?") == System.Windows.Forms.DialogResult.Yes)
                    {
                        FrmPassword FrmPassword = new FrmPassword();
                        FrmPassword.ShowForm(ObjPer.PASSWORD);
                        SizeMasterProperty Property = new SizeMasterProperty();
                        DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                        Property.SIZE_ID = Val.ToInt32(Drow["SIZE_ID"]);

                        Property = ObjSize.DeleteMix(Property);

                        if (Property.ReturnMessageType == "SUCCESS")
                        {
                            Global.Message("ENTRY DELETED SUCCESSFULLY");
                            DtabSize.Rows.RemoveAt(GrdDet.FocusedRowHandle);
                            DtabSize.AcceptChanges();
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

        private void RpTxtDeptName_KeyPress(object sender, KeyPressEventArgs e)
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
                        GrdDet.SetFocusedRowCellValue("DEPARTMENT_ID", Val.ToString(FrmSearch.DRow["DEPARTMENT_ID"]));
                        GrdDet.SetFocusedRowCellValue("DEPARTMENT_NAME", Val.ToString(FrmSearch.DRow["DEPARTMENTNAME"]));
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
