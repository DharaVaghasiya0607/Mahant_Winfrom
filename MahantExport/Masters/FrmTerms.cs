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
    public partial class FrmTerms : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_Terms ObjMast = new BOMST_Terms();
        DataTable DtabTerms = new DataTable();
        string StrMesg = "";

        #region Property Settings

        public FrmTerms()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            //ObjPer.GetFormPermission(Val.ToString(this.Tag));
            //BtnSave.Enabled = ObjPer.ISINSERT;
            //deleteSelectedAmountToolStripMenuItem.Enabled = ObjPer.ISDELETE;
           
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

        public bool CheckDuplicate(string ColName, string ColValue, int IntRowIndex, string StrMsg)
        {
            if (Val.ToString(ColValue).Trim().Equals(string.Empty))
                return false;

            var Result = from row in DtabTerms.AsEnumerable()
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
            int IntCol = -1, IntRow = -1;
            foreach (DataRow dr in DtabTerms.Rows)
            {
                //For Update Validation
                if (Val.ToString(dr["TERMSNAME"]).Trim().Equals(string.Empty) && !Val.ToString(dr["TERMS_ID"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter 'Terms Name'");
                    IntCol = 0;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }
                
                //end as

                if (Val.ToString(dr["TERMSNAME"]).Trim().Equals(string.Empty))
                {
                    if (DtabTerms.Rows.Count == 1)
                    {
                        Global.Message("Please Enter 'Terms Name'");
                        IntCol = 0;
                        IntRow = dr.Table.Rows.IndexOf(dr);
                        break;

                    }
                    else
                        continue;
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
            Fill();
            DtabTerms.Rows.Add(DtabTerms.NewRow());

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


                foreach (DataRow Dr in DtabTerms.Rows)
                {
                    TermsMasterProperty Property = new TermsMasterProperty();
                
                    if (Val.ToString(Dr["TERMSNAME"]).Trim().Equals(string.Empty))
                        continue;

                    Property.TERMS_ID = Val.ToInt32(Dr["TERMS_ID"]);
                    Property.TERMSNAME = Val.ToString(Dr["TERMSNAME"]);
                    Property.TERMSDAYS = Val.ToInt32(Dr["TERMSDAYS"]);
                    Property.TERMSPER = Val.Val(Dr["TERMSPER"]);
                    Property.ISACTIVE = Val.ToBoolean(Dr["ISACTIVE"]);
                    Property = ObjMast.Save(Property);

                    ReturnMessageDesc = Property.ReturnMessageDesc;
                    ReturnMessageType = Property.ReturnMessageType;

                    Property = null;
                }
                DtabTerms.AcceptChanges();

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
           DtabTerms = ObjMast.Fill();
           
            MainGrid.DataSource = DtabTerms;
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
            Global.ExcelExport("Terms List", GrdDet);
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
                            TermsMasterProperty Property = new TermsMasterProperty();
                            DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                            Property.TERMS_ID = Val.ToInt32(Drow["TERMS_ID"]);
                            Property = ObjMast.Delete(Property);

                            if (Property.ReturnMessageType == "SUCCESS")
                            {
                                Global.Message("ENTRY DELETED SUCCESSFULLY");
                                DtabTerms.Rows.RemoveAt(GrdDet.FocusedRowHandle);
                                DtabTerms.AcceptChanges();
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

        private void repTxrPer_KeyDown(object sender, KeyEventArgs e)
        {
            //try
            //{
            //    if (e.KeyCode == Keys.Enter)
            //    {
            //        DataRow dr = GrdDet.GetFocusedDataRow();
            //        if (!Val.ToString(dr["TERMSNAME"]).Equals(string.Empty) && GrdDet.IsLastRow)
            //        {
            //            DtabTerms.Rows.Add(DtabTerms.NewRow());
            //        }
            //        else if (GrdDet.IsLastRow)
            //        {
            //            BtnSave.Focus();
            //            e.Handled = true;
            //        }
            //    }

            //}
            //catch (Exception ex)
            //{
            //    Global.Message(ex.Message);
            //}
        }

        private void repTxtTermsCode_Validating(object sender, CancelEventArgs e)
        {
            if (GrdDet.FocusedRowHandle < 0)
                return;

            DataRow Dr = GrdDet.GetFocusedDataRow();
            if (CheckDuplicate("TERMSCODE", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "Code"))
                e.Cancel = true;
            return;
        }

        private void repTxtTermsName_Validating(object sender, CancelEventArgs e)
        {
            if (GrdDet.FocusedRowHandle < 0)
                return;

            DataRow Dr = GrdDet.GetFocusedDataRow();
            if (CheckDuplicate("TERMSNAME", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "Name"))
                e.Cancel = true;
            return;
        }

        private void repTxtDays_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DataRow dr = GrdDet.GetFocusedDataRow();
                    if (!Val.ToString(dr["TERMSNAME"]).Equals(string.Empty) && GrdDet.IsLastRow)
                    {
                        DtabTerms.Rows.Add(DtabTerms.NewRow());
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

    }
}
