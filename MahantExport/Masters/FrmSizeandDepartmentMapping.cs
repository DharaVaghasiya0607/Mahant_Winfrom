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

namespace MahantExport.Master
{
    public partial class FrmSerialNoSizeMaster : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOMST_SerialNoSize ObjSize = new BOMST_SerialNoSize();

        DataTable DtabSerialSize = new DataTable();

        public FrmSerialNoSizeMaster()
        {
            InitializeComponent();
            
        }
        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            BtnSave.Enabled = ObjPer.ISINSERT;
            DeleteSelectedSizeToolStripMenuItem.Enabled = ObjPer.ISDELETE;

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

            var Result = from row in DtabSerialSize.AsEnumerable()
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
           
            int IntCol = 0, IntRow = -1;
            foreach (DataRow dr in DtabSerialSize.Rows)
            {
                //For Update Validation
                if (Val.Val(dr["FROMSIZE"]) == 0 && Val.Val(dr["TOSIZE"]) != 0)
                {
                    Global.Message("Please Enter FROM CARAT");
                    IntCol = 0;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }
                //end as


                if (Val.Val(dr["FROMSIZE"]) == 0 )
                {
                    if (DtabSerialSize.Rows.Count == 1)
                    {
                        Global.Message("Please Enter FROM CARAT");
                        IntCol = 1;
                        IntRow = dr.Table.Rows.IndexOf(dr);
                        break;

                    }
                    else
                        continue;
                }
                //if (Val.Val(dr["TOCARAT"]) == 0)
                //{
                //    Global.Message("Please Enter TO CARAT");
                //    IntCol = 2;
                //    IntRow = dr.Table.Rows.IndexOf(dr);
                //    break;
                //}
                if (Val.ToString(dr["SIZENAME"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter NAME");
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
                return true;
            }
            return false;
        }

        #endregion

        public void Clear()
        {
            DtabSerialSize.Rows.Clear();
            DtabSerialSize.Rows.Add(DtabSerialSize.NewRow());
            Fill();
        }

        public void Fill()
        {
            DtabSerialSize = ObjSize.Fill();
            DtabSerialSize.Rows.Add(DtabSerialSize.NewRow());
            MainGrid.DataSource = DtabSerialSize;
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
                if (ValSave())
                {
                    return;
                }
                string ReturnMessageDesc = "";
                string ReturnMessageType = "";

                foreach (DataRow Dr in DtabSerialSize.Rows)
                {
                    MstSerialNoSizeMasterProperty Property = new MstSerialNoSizeMasterProperty();
                
                    if (Val.Val(Dr["FROMSIZE"]) == 0 ) //|| Val.Val(Dr["TOCARAT"]) == 0
                        continue;

                    Property.ID = Val.ToInt32(Dr["ID"]);
                    Property.SIZENAME = Val.ToString(Dr["SIZENAME"]);
                    Property.FROMSIZE= Val.Val(Dr["FROMSIZE"]);
                    Property.TOSIZE = Val.Val(Dr["TOSIZE"]);
                    Property.ISACTIVE = Val.ToBoolean(Dr["ISACTIVE"]);
                    Property.REMARK = Val.ToString(Dr["REMARK"]);
                    Property.SEQUENCENO = Val.ToInt32(Dr["SEQUENCENO"]);
                    Property.DEPARTMENT_ID = Val.ToInt32(Dr["DEPARTMENT_ID"]);
                    Property = ObjSize.Save(Property);

                    ReturnMessageDesc = Property.ReturnMessageDesc;
                    ReturnMessageType = Property.ReturnMessageType;

                    Property = null;
                }
                DtabSerialSize.AcceptChanges();

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
                    if (Val.Val(dr["FROMSIZE"])!=0 && Val.Val(dr["TOSIZE"])!=0 && GrdDet.IsLastRow)
                    {
                        DtabSerialSize.Rows.Add(DtabSerialSize.NewRow());

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
                GrdDet.EditingValue = Val.ToString(Dr["FROMSIZE"]) + "-" + Val.ToString(Dr["TOSIZE"]);      
            }
        }

        private void reptxtFromCarat_Validating(object sender, CancelEventArgs e)
        {
            GrdDet.PostEditor();
            DataRow Dr = GrdDet.GetFocusedDataRow();
            if (CheckDuplicate("FROMSIZE", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "FROMSIZE"))
            {
                e.Cancel = true;
                return;
            }
            else
            {
                Dr["SIZENAME"] =  Val.ToString(GrdDet.EditingValue) +  "-" + Val.ToString(Dr["TOSIZE"])  ;
                DtabSerialSize.AcceptChanges();

            }
            if (Val.ToDecimal(Dr["TOSIZE"]) != 0)
            {
                if (Val.ToDecimal(GrdDet.EditingValue) > Val.ToDecimal(Dr["TOSIZE"]))
                {
                    Global.Message("From Size must be Greter Than To Carat");
                    e.Cancel = true;
                    return;
                }
            }

            var dValue = from row in DtabSerialSize.AsEnumerable()
                         where Val.Val(row["FROMSIZE"]) <= Val.Val(GrdDet.EditingValue) && Val.Val(row["TOSIZE"]) >= Val.Val(GrdDet.EditingValue) && row.Table.Rows.IndexOf(row) != GrdDet.FocusedRowHandle
                         select row;

            if (dValue.Any())
            {
                Global.Message("This Value Already Exist Between Some From Size and To Size Please Check.!");
                e.Cancel = true;
                return;
            }
            
        }

        private void reptxtToCarat_Validating(object sender, CancelEventArgs e)
        {
            DataRow Dr = GrdDet.GetFocusedDataRow();
            if (CheckDuplicate("TOSIZE", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "TOSIZE"))
            {
                e.Cancel = true;
                return;
            }
            if (Val.ToDecimal(Dr["FROMSIZE"]) > Val.ToDecimal(GrdDet.EditingValue))
            {
                 Global.Message("To Size must be Greter Than From Size");
                 e.Cancel = true;
                 return;
            }
            else
            {
                    Dr["SIZENAME"] = Val.ToString(Dr["FROMSIZE"]) + "-"+ Val.ToString(GrdDet.EditingValue) ;
                    DtabSerialSize.AcceptChanges();

            }

            var dValue = from row in DtabSerialSize.AsEnumerable()
                         where Val.Val(row["FROMSIZE"]) <= Val.Val(GrdDet.EditingValue) && Val.Val(row["TOSIZE"]) >= Val.Val(GrdDet.EditingValue) && row.Table.Rows.IndexOf(row) != GrdDet.FocusedRowHandle
                         select row;

            if (dValue.Any())
            {
                Global.Message("This Value Already Exist Between Some From Size and To Carat Please Check.!");
                e.Cancel = true;
                return;
            }

        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("SerialSize List", GrdDet);
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
                         if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                         {
                             MstSerialNoSizeMasterProperty Property = new MstSerialNoSizeMasterProperty();
                             DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                             Property.ID = Val.ToInt32(Drow["ID"]);

                             Property = ObjSize.Delete(Property);

                             if (Property.ReturnMessageType == "SUCCESS")
                             {
                                 Global.Message("ENTRY DELETED SUCCESSFULLY");
                                 DtabSerialSize.Rows.RemoveAt(GrdDet.FocusedRowHandle);
                                 DtabSerialSize.AcceptChanges();
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

        private void repTxtDepartmentName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "DEPARTMENTCODE,DEPARTMENTNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_DEPARTMENT);
                    FrmSearch.mStrColumnsToHide = "DEPARTMENT_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                      
                            GrdDet.SetRowCellValue(GrdDet.FocusedRowHandle, "DEPARTMENTNAME", Val.ToString(FrmSearch.DRow["DEPARTMENTNAME"]));
                            GrdDet.SetRowCellValue(GrdDet.FocusedRowHandle, "DEPARTMENT_ID", Val.ToString(FrmSearch.DRow["DEPARTMENT_ID"]));
                       
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



    }
}
