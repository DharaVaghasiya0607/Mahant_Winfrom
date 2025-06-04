using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MahantExport.Stock
{
    public partial class FrmKapanAvgDetail : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOTRN_KapanAvgDetail ObjKapan = new BOTRN_KapanAvgDetail();

        DataTable DtabKapan = new DataTable();

        public FrmKapanAvgDetail()
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
            ObjFormEvent.ObjToDisposeList.Add(ObjKapan);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        public bool CheckDuplicate(string ColName, string ColValue, int IntRowIndex, string StrMsg)
        {
            if (Val.ToString(ColValue).Trim().Equals(string.Empty))
                return false;

            var Result = from row in DtabKapan.AsEnumerable()
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

            int IntCol = 0, IntRow = -1;
            foreach (DataRow dr in DtabKapan.Rows)
            {
                //For Update Validation
                //if (Val.ToString(dr["KAPANNAME"]) != "")
                //{
                //    Global.Message("Please Enter 'Kapan Name'");
                //    IntCol = 0;
                //    IntRow = dr.Table.Rows.IndexOf(dr);
                //    break;
                //}
                //end as


                if (Val.ToString(dr["KAPANNAME"]).Trim().Equals(string.Empty))
                {
                    if (DtabKapan.Rows.Count == 1)
                    {
                        Global.Message("Please Enter 'KAPAN NAME'");
                        IntCol = 1;
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
            DtabKapan.Rows.Clear();
            DtabKapan.Rows.Add(DtabKapan.NewRow());
            Fill();
        }

        public void Fill()
        {
            DtabKapan = ObjKapan.Fill();
            DtabKapan.Rows.Add(DtabKapan.NewRow());
            MainGrid.DataSource = DtabKapan;
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

                this.Cursor = Cursors.WaitCursor;
                foreach (DataRow Dr in DtabKapan.Rows)
                {
                    TrnKapanAvgKapanProperty Property = new TrnKapanAvgKapanProperty();

                    if (Val.ToString(Dr["KAPANNAME"]).Trim().Equals(string.Empty) || (Val.Val(Dr["EXCRATE"]) == 0 && (Val.Val(Dr["SURATAVARAGE"]) == 0)))
                        continue;

                    Property.DETAIL_ID = Val.ToInt(Dr["DETAIL_ID"]);
                    Property.KAPAN_ID = Guid.Parse(Val.ToString(Dr["KAPAN_ID"]));
                    Property.REMARK = Val.ToString(Dr["REMARK"]);
                    Property.EXCRATE = Val.Val(Dr["EXCRATE"]);
                    Property.SURATAVARAGE = Val.Val(Dr["SURATAVARAGE"]);

                    Property = ObjKapan.Save(Property);

                    ReturnMessageDesc = Property.ReturnMessageDesc;
                    ReturnMessageType = Property.ReturnMessageType;

                    Property = null;
                }
                DtabKapan.AcceptChanges();

                Global.Message(ReturnMessageDesc);

                if (ReturnMessageType == "SUCCESS")
                {
                    Fill();

                    if (GrdDet.RowCount > 1)
                    {
                        GrdDet.FocusedRowHandle = GrdDet.RowCount - 1;
                    }
                }

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
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
                    if (Val.ToString(dr["KAPANNAME"]) != "" && GrdDet.IsLastRow)
                    {
                        DtabKapan.Rows.Add(DtabKapan.NewRow());

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




        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("Kapan Avg Detail", GrdDet);
        }

        private void DeleteSelectedSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle >= 0)
                {
                    if (Global.Confirm("ARE YOU SURE YOU WANT TO DELETE ENTRY ?") == System.Windows.Forms.DialogResult.Yes)
                    {
                        TrnKapanAvgKapanProperty Property = new TrnKapanAvgKapanProperty();
                        DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);

                        Property.DETAIL_ID = Val.ToInt(Drow["DETAIL_ID"]);

                        Property = ObjKapan.Delete(Property);

                        if (Property.ReturnMessageType == "SUCCESS")
                        {
                            Global.Message("ENTRY DELETED SUCCESSFULLY");
                            DtabKapan.Rows.RemoveAt(GrdDet.FocusedRowHandle);
                            DtabKapan.AcceptChanges();
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
        private void reptxtName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "KAPANNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.TRN_KAPAN);
                    //FrmSearch.ColumnsToHide = "DEPARTMENT_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDet.SetFocusedRowCellValue("KAPANNAME", Val.ToString(FrmSearch.DRow["KAPANNAME"]));
                        GrdDet.SetFocusedRowCellValue("KAPAN_ID", Val.ToString(FrmSearch.DRow["KAPAN_ID"]));

                        if (CheckDuplicate("KAPANNAME", Val.ToString(FrmSearch.DRow["KAPANNAME"]), GrdDet.FocusedRowHandle, "Name"))
                            GrdDet.SetFocusedRowCellValue("KAPANNAME", "");
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

        private void reptxtName_Validating(object sender, CancelEventArgs e)
        {
            //try
            //{
            //    if (GrdDet.FocusedRowHandle < 0)
            //        return;

            //    DataRow Dr = GrdDet.GetFocusedDataRow();
            //    if (CheckDuplicate("KAPANNAME", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "Name"))
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
