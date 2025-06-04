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
    public partial class FrmCollectionMaster : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_Collection ObjMast = new BOMST_Collection();
        DataTable DtabCollection = new DataTable();
        string StrMesg = "";

        #region Property Settings

        public FrmCollectionMaster()
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
            this.Show();
            Clear();
            
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

        #region Validation

        private bool ValSave()
        {
            int IntCol = -1, IntRow = -1;
            foreach (DataRow dr in DtabCollection.Rows)
            {
                //For Update Validation
                if (Val.ToString(dr["COLLECTIONCODE"]).Trim().Equals(string.Empty) && !Val.ToString(dr["COLLECTIONNAME"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter 'Collection Code'");
                    IntCol = 0;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }
                //end as


                if (Val.ToString(dr["COLLECTIONCODE"]).Trim().Equals(string.Empty))
                {
                    if (DtabCollection.Rows.Count == 1)
                    {
                        Global.Message("Please Enter 'Collection Code'");
                        IntCol = 0;
                        IntRow = dr.Table.Rows.IndexOf(dr);
                        break;
                    }
                    else
                        continue;
                }

                if (Val.ToString(dr["COLLECTIONNAME"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter 'Collection Name'");
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

        private bool ValDelete()
        {

            return true;
        }

        #endregion


        public void Clear()
        {
            Fill();
            DtabCollection.Rows.Add(DtabCollection.NewRow());

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

                if (Global.Confirm("Are Your Sure To Save All Records ?") == System.Windows.Forms.DialogResult.No)
                    return;

                string ReturnMessageDesc = "";
                string ReturnMessageType = "";

                foreach (DataRow Dr in DtabCollection.Rows)
                {
                    CollectionProperty Property = new CollectionProperty();

                    if (Val.ToString(Dr["COLLECTIONCODE"]).Trim().Equals(string.Empty) || Val.ToString(Dr["COLLECTIONNAME"]).Trim().Equals(string.Empty))
                        continue;

                    Property.COLLECTION_ID = Val.ToInt32(Dr["COLLECTION_ID"]);
                    Property.COLLECTIONCODE = Val.ToString(Dr["COLLECTIONCODE"]);
                    Property.COLLECTIONNAME = Val.ToString(Dr["COLLECTIONNAME"]);
                    Property.STATUS = Val.ToInt32(Dr["STATUS"]);
                    Property.ISACTIVE = Val.ToBoolean(Dr["ISACTIVE"]);
                    Property.REMARK = Val.ToString(Dr["REMARK"]);

                    Property = ObjMast.Save(Property);                   

                    ReturnMessageDesc = Property.ReturnMessageDesc;
                    ReturnMessageType = Property.ReturnMessageType;

                    Property = null;
                }
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
            DtabCollection = ObjMast.Fill();
            MainGrid.DataSource = DtabCollection;
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
            Global.ExcelExport("Collection List", GrdDet);
        }

        public bool CheckDuplicate(string ColName, string ColValue, int IntRowIndex, string StrMsg)
        {
            if (Val.ToString(ColValue).Trim().Equals(string.Empty))
                return false;

            var Result = from row in DtabCollection.AsEnumerable()
                         where Val.ToString(row[ColName]).Trim().ToUpper() == Val.ToString(ColValue).Trim().ToUpper() && row.Table.Rows.IndexOf(row) != IntRowIndex
                         select row;


            if (Result.Any())
            {
                Global.Message(StrMsg + " ALREADY EXISTS.");
                return true;
            }
            return false;
        }
        private void repTxtCollectionCode_Validating(object sender, CancelEventArgs e)
        {
            if (GrdDet.FocusedRowHandle < 0)
                return;

            DataRow Dr = GrdDet.GetFocusedDataRow();
            if (CheckDuplicate("COLLECTIONCODE", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "'Collection Code'"))
                e.Cancel = true;
            return;
        }

        private void repTxtCollectionName_Validating(object sender, CancelEventArgs e)
        {
            if (GrdDet.FocusedRowHandle < 0)
                return;

            DataRow Dr = GrdDet.GetFocusedDataRow();
            if (CheckDuplicate("COLLECTIONNAME", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "'Collection Name'"))
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
                         //FrmPassword FrmPassword = new FrmPassword();
                         //if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                         //{
                            CollectionProperty Property = new CollectionProperty();
                             DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                             Property.COLLECTION_ID = Val.ToInt32(Drow["COLLECTION_ID"]);
                             Property = ObjMast.Delete(Property);

                             if (Property.ReturnMessageType == "SUCCESS")
                             {
                                 Global.Message("ENTRY DELETED SUCCESSFULLY");
                                 Clear();
                             }
                             else
                             {
                                 Global.Message("ERROR IN DELETE ENTRY");
                             }
                         //}
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void repTxtRemark_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DataRow dr = GrdDet.GetFocusedDataRow();
                    if (!Val.ToString(dr["COLLECTIONCODE"]).Equals(string.Empty) && !Val.ToString(dr["COLLECTIONNAME"]).Equals(string.Empty) && GrdDet.IsLastRow)
                    {
                        DtabCollection.Rows.Add(DtabCollection.NewRow());
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

        private void RepTxtClientCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "EMPLOYEENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;

                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_EMPLOYEE);
                    FrmSearch.mStrColumnsToHide = "EMPLOYEE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;

                    if (FrmSearch.DRow != null)
                    {
                        GrdDet.SetFocusedRowCellValue("CLIENT_ID", Val.ToString(FrmSearch.DRow["CLIENT_ID"]));
                        GrdDet.SetFocusedRowCellValue("CLIENTNAME", Val.ToString(FrmSearch.DRow["CLIENTNAME"]));
                        GrdDet.SetFocusedRowCellValue("CLIENTCODE", Val.ToString(FrmSearch.DRow["CLIENTCODE"]));
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
