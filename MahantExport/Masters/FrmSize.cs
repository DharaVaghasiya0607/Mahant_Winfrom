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
    public partial class FrmSize : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOMST_Size ObjSize = new BOMST_Size();

        DataTable DtabSize = new DataTable();

        public FrmSize()
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

        private bool ValSave()
        {
           
            int IntCol = 0, IntRow = -1;
            foreach (DataRow dr in DtabSize.Rows)
            {
                //For Update Validation
                //if (Val.Val(dr["TOCARAT"]) != 0)
                //{
                //    Global.Message("Please Enter FROM CARAT");
                //    IntCol = 0;
                //    IntRow = dr.Table.Rows.IndexOf(dr);
                //    break;
                //}
                //end as


                //if (Val.Val(dr["FROMCARAT"]) == 0 )
                //{
                //    if (DtabSize.Rows.Count == 1)
                //    {
                //        Global.Message("Please Enter FROM CARAT");
                //        IntCol = 1;
                //        IntRow = dr.Table.Rows.IndexOf(dr);
                //        break;

                //    }
                //    else
                //        continue;
                //}
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
            DtabSize.Rows.Clear();
            DtabSize.Rows.Add(DtabSize.NewRow());
            Fill();
        }

        public void Fill()
        {
            DtabSize = ObjSize.Fill();
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
                //if (ValSave())
                //{
                //    return;
                //}
                string ReturnMessageDesc = "";
                string ReturnMessageType = "";

                foreach (DataRow Dr in DtabSize.Rows)
                {
                    SizeMasterProperty Property = new SizeMasterProperty();
                
                    if (Val.Val(Dr["FROMCARAT"]) == 0 ) //|| Val.Val(Dr["TOCARAT"]) == 0
                        continue;

                    Property.SIZE_ID = Val.ToInt32(Dr["SIZE_ID"]);
                    Property.SIZENAME = Val.ToString(Dr["SIZENAME"]);
                    Property.FROMCARAT= Val.Val(Dr["FROMCARAT"]);
                    Property.TOCARAT = Val.Val(Dr["TOCARAT"]);
                    Property.ISACTIVE = Val.ToBoolean(Dr["ISACTIVE"]);
                    Property.REMARK = Val.ToString(Dr["REMARK"]);
                    Property.SEQUENCENO = Val.ToInt32(Dr["SEQUENCENO"]);
                    Property = ObjSize.Save(Property);

                    ReturnMessageDesc = Property.ReturnMessageDesc;
                    ReturnMessageType = Property.ReturnMessageType;

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
                    if (Val.Val(dr["FROMCARAT"])!=0 && Val.Val(dr["TOCARAT"])!=0 && GrdDet.IsLastRow)
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
            else
            {
                Dr["SIZENAME"] =  Val.ToString(GrdDet.EditingValue) +  "-" + Val.ToString(Dr["TOCARAT"])  ;
                DtabSize.AcceptChanges();

            }
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
            else
            {
                    Dr["SIZENAME"] = Val.ToString(Dr["FROMCARAT"]) + "-"+ Val.ToString(GrdDet.EditingValue) ;
                    DtabSize.AcceptChanges();

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
                        if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                        {
                            SizeMasterProperty Property = new SizeMasterProperty();
                            DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                            Property.SIZE_ID = Val.ToInt32(Drow["SIZE_ID"]);

                            Property = ObjSize.Delete(Property);

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
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }



    }
}
