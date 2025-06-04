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
    public partial class FrmYearMaster : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_Year ObjMast = new BOMST_Year();
        DataTable DTabYear = new DataTable();


        #region Property Settings

        public FrmYearMaster()
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

            var Result = from row in DTabYear.AsEnumerable()
                         where Val.ToString(row[ColName]).Trim().ToUpper() == Val.ToString(ColValue).Trim().ToUpper() && row.Table.Rows.IndexOf(row) != IntRowIndex
                         select row;


            if (Result.Any())
            {
                Global.Message("'" + StrMsg + "' ALREADY EXISTS.");
                return true;
            }
            return false;
        }

        

        public void Clear()
        {
            Fill();
            DTabYear.Rows.Add(DTabYear.NewRow());

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
            try{
                string ReturnMessageDesc = "";
                string ReturnMessageType = "";

                foreach (DataRow Dr in DTabYear.GetChanges().Rows)
                {
                    YearMasterProperty Property = new YearMasterProperty();
                
                    if (Val.ToString(Dr["YEARNAME"]).Trim().Equals(string.Empty))
                        continue;

                    Property.YEAR_ID = Val.ToInt32(Dr["YEAR_ID"]);
                    Property.YEARNAME = Val.ToString(Dr["YEARNAME"]);
                    Property.YEARSHORTNAME = Val.ToString(Dr["YEARSHORTNAME"]);
                    Property.ISACTIVE = Val.ToBoolean(Dr["ISACTIVE"]);
                    Property.ISLOCK = Val.ToBoolean(Dr["ISLOCK"]);
                    Property.REMARK = Val.ToString(Dr["REMARK"]);
                    Property.FROMDATE = Val.SqlDate(Dr["FROMDATE"].ToString());
                    Property.TODATE = Val.SqlDate(Dr["TODATE"].ToString());

                    Property = ObjMast.Save(Property);

                    Dr["YEAR_ID"] = Property.ReturnValue;

                    ReturnMessageDesc = Property.ReturnMessageDesc;
                    ReturnMessageType = Property.ReturnMessageType;

                    Property = null;
                }
                DTabYear.AcceptChanges();

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
            DTabYear = ObjMast.Fill();
            MainGrid.DataSource = DTabYear;
            MainGrid.Refresh();

        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
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
                        YearMasterProperty Property = new YearMasterProperty();

                        FrmPassword FrmPassword = new FrmPassword();
                        if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                        {
                            DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                            Property.YEAR_ID = Val.ToInt32(Drow["YEAR_ID"]);
                            Property = ObjMast.Delete(Property);

                            if (Property.ReturnMessageType == "SUCCESS")
                            {
                                Global.Message("ENTRY DELETED SUCCESSFULLY");
                                DTabYear.Rows.RemoveAt(GrdDet.FocusedRowHandle);
                                DTabYear.AcceptChanges();
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
                    if (!Val.ToString(dr["YEARNAME"]).Equals(string.Empty) && GrdDet.IsLastRow)
                    {
                        DTabYear.Rows.Add(DTabYear.NewRow());
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

        private void repTxtYearName_Validating(object sender, CancelEventArgs e)
        {
            //try
            //{
            //    if (GrdDet.FocusedRowHandle < 0)
            //        return;

            //    DataRow Dr = GrdDet.GetFocusedDataRow();
            //    if (CheckDuplicate("YEARNAME", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "Year Name"))
            //        e.Cancel = true;
            //    return;
            //}
            //catch (Exception ex)
            //{
            //    Global.Message(ex.Message.ToString());
            //}
        }

     
    }
}
