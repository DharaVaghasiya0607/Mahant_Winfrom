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
    public partial class FrmProductSubCategory : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_ProductSubCategory ObjMast = new BOMST_ProductSubCategory();
        DataTable DtabSubProductCategory = new DataTable();
        string StrMesg = "";

        #region Property Settings

        public FrmProductSubCategory()
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
            foreach (DataRow dr in DtabSubProductCategory.Rows)
            {
                //For Update Validation
                if (Val.ToString(dr["PRODUCTSUBCATEGORYCODE"]).Trim().Equals(string.Empty) && !Val.ToString(dr["PRODUCTSUBCATEGORYNAME"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter 'ProductSubCategory Code'");
                    IntCol = 0;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }
                //end as


                if (Val.ToString(dr["PRODUCTSUBCATEGORYCODE"]).Trim().Equals(string.Empty))
                {
                    if (DtabSubProductCategory.Rows.Count == 1)
                    {
                        Global.Message("Please Enter 'ProductSubCategory Code'");
                        IntCol = 0;
                        IntRow = dr.Table.Rows.IndexOf(dr);
                        break;

                    }
                    else
                        continue;
                }

                if (Val.ToString(dr["PRODUCTSUBCATEGORYNAME"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter 'ProductSubCategory Name'");
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
            DtabSubProductCategory.Rows.Add(DtabSubProductCategory.NewRow());

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

                foreach (DataRow Dr in DtabSubProductCategory.Rows)
                {
                    ProductSubCategoryProperty Property = new ProductSubCategoryProperty();

                    if (Val.ToString(Dr["PRODUCTSUBCATEGORYCODE"]).Trim().Equals(string.Empty) || Val.ToString(Dr["PRODUCTSUBCATEGORYNAME"]).Trim().Equals(string.Empty))
                        continue;

                    Property.PRODUCTSUBCATEGORY_ID = Val.ToInt32(Dr["PRODUCTSUBCATEGORY_ID"]);                    
                    Property.PRODUCTSUBCATEGORYCODE = Val.ToString(Dr["PRODUCTSUBCATEGORYCODE"]);
                    Property.PRODUCTSUBCATEGORYNAME = Val.ToString(Dr["PRODUCTSUBCATEGORYNAME"]);
                    Property.PRODUCTCATEGORY_ID = Val.ToInt32(Dr["PRODUCTCATEGORY_ID"]);
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
            DtabSubProductCategory = ObjMast.Fill();
            MainGrid.DataSource = DtabSubProductCategory;
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
            Global.ExcelExport("Product Sub Category List", GrdDet);
        }

        public bool CheckDuplicate(string ColName, string ColValue, int IntRowIndex, string StrMsg)
        {
            if (Val.ToString(ColValue).Trim().Equals(string.Empty))
                return false;

            var Result = from row in DtabSubProductCategory.AsEnumerable()
                         where Val.ToString(row[ColName]).Trim().ToUpper() == Val.ToString(ColValue).Trim().ToUpper() && row.Table.Rows.IndexOf(row) != IntRowIndex
                         select row;


            if (Result.Any())
            {
                Global.Message(StrMsg + " ALREADY EXISTS.");
                return true;
            }
            return false;
        }
        private void repTxtSubCategoryCode_Validating(object sender, CancelEventArgs e)
        {
            if (GrdDet.FocusedRowHandle < 0)
                return;

            DataRow Dr = GrdDet.GetFocusedDataRow();
            if (CheckDuplicate("PRODUCTSUBCATEGORYCODE", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "'Sub Category Code'"))
                e.Cancel = true;
            return;
        }

        private void repTxtSubCategoryName_Validating(object sender, CancelEventArgs e)
        {
            if (GrdDet.FocusedRowHandle < 0)
                return;

            DataRow Dr = GrdDet.GetFocusedDataRow();
            if (CheckDuplicate("PRODUCTSUBCATEGORYNAME", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "'Sub Category Name'"))
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
                             ProductSubCategoryProperty Property = new ProductSubCategoryProperty();
                             DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                             Property.PRODUCTSUBCATEGORY_ID = Val.ToInt32(Drow["PRODUCTSUBCATEGORY_ID"]);
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
                    if (!Val.ToString(dr["PRODUCTSUBCATEGORYCODE"]).Equals(string.Empty) && !Val.ToString(dr["PRODUCTSUBCATEGORYNAME"]).Equals(string.Empty) && GrdDet.IsLastRow)
                    {
                        DtabSubProductCategory.Rows.Add(DtabSubProductCategory.NewRow());
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

        private void repTxtProductCategoryID_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PRODUCTCATEGORY_ID,PRODUCTCATEGORYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;

                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PRODUCT_CATEGORY);
                    //FrmSearch.mStrColumnsToHide = "PRODUCTCATEGORYCODE";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;

                    if (FrmSearch.DRow != null)
                    {
                        GrdDet.SetFocusedRowCellValue("PRODUCTCATEGORY_ID", Val.ToString(FrmSearch.DRow["PRODUCTCATEGORY_ID"]));
                        GrdDet.SetFocusedRowCellValue("PRODUCTCATEGORYNAME", Val.ToString(FrmSearch.DRow["PRODUCTCATEGORYNAME"]));
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
