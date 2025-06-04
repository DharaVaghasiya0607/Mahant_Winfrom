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
    public partial class FrmExcelSetting : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();

        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_ExcelSetting ObjMast = new BOMST_ExcelSetting();
        DataTable DtabExcelSetting = new DataTable();

        DataTable DtabExcelSetting_TEMP = new DataTable();
        bool IsNextImage = true;

        #region Property Settings

        public FrmExcelSetting()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
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

        public bool CheckDuplicateForSetting(string ColName, string ColValue, int IntRowIndex, string StrMsg)
        {
            if (Val.ToString(ColValue).Trim().Equals(string.Empty))
                return false;

            var Result = from row in DtabExcelSetting_TEMP.AsEnumerable()
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

            //if (Val.ToString(txtPartyName.Text).Trim().Equals(string.Empty))
            //{
            //    Global.Message("Party Is Required.");
            //    txtPartyName.Focus();
            //    return true;
            //}

            int IntCol = 0, IntRow = 0;
            foreach (DataRow dr in DtabExcelSetting.Rows)
            {
                //For Update Validation
                if (Val.ToString(dr["EXCELSETTINGNAME"]).Trim().Equals(string.Empty) && !Val.ToString(dr["EXCELSETTINGREFNAME"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter Setting Name");
                    IntCol = 0;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }
                //end as


                if (Val.ToString(dr["EXCELSETTINGNAME"]).Trim().Equals(string.Empty))
                {
                    if (DtabExcelSetting.Rows.Count == 1)
                    {
                        Global.Message("Please Enter Setting Name");
                        IntCol = 0;
                        IntRow = dr.Table.Rows.IndexOf(dr);
                        break;

                    }
                    else
                        continue;
                }
                if (Val.ToString(dr["EXCELSETTINGREFNAME"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter Reference Value");
                    IntCol = 1;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }

            }
            if (IntRow > 0)
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
            //if (txtItemGroupCode.Text.Trim().Length == 0)
            //{
            //    Global.Message("Group Code Is Required");
            //    txtItemGroupCode.Focus();
            //    return false;
            //}

            return true;
        }

        #endregion

        public void Clear()
        {
            txtPartyName.Text = string.Empty;
            txtPartyName.Tag = string.Empty;

            MainGrid.DataSource = null;

            //Changed : Pinali : 01-04-2019
            DtabExcelSetting_TEMP.Columns.Clear();
            DtabExcelSetting_TEMP.Rows.Clear();
            DtabExcelSetting_TEMP.Columns.Add("EXCELSETTING_ID", typeof(int));
            DtabExcelSetting_TEMP.Columns.Add("SEQUENCENO", typeof(int));
            DtabExcelSetting_TEMP.Columns.Add("EXCELSETTINGREFNAME", typeof(string));
            DtabExcelSetting_TEMP.Columns.Add("EXCELSETTINGNAME", typeof(string));
            //End : Pinali  : 01-04-201

            MainGrdSetting.DataSource = DtabExcelSetting_TEMP;
            MainGrdSetting.Refresh();

            txtCopyToPartySetting.Text = string.Empty;
            txtCopyToPartySetting.Tag = string.Empty;

            //Fill();
            txtPartyName.Focus();
            lblStatus.Visible = false;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            Clear();

        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;
                if (ValSave())
                {
                    return;
                }

                //if (Global.Confirm("Are Your Sure To Save All Records ?") == System.Windows.Forms.DialogResult.No)
                //    return;


                string ReturnMessageDesc = "";
                string ReturnMessageType = "";


                //foreach (DataRow Dr in DtabExcelSetting.GetChanges().Rows)
                foreach (DataRow Dr in DtabExcelSetting.Rows)
                {
                    ExcelSettingMasterProperty Property = new ExcelSettingMasterProperty();

                    Property.LEDGER_ID = Val.ToString(txtPartyName.Tag) == "" ? Guid.Empty : Guid.Parse(Val.ToString(txtPartyName.Tag));

                    if (Val.ToString(Dr["EXCELSETTINGNAME"]).Trim().Equals(string.Empty) || Val.ToString(Dr["EXCELSETTINGREFNAME"]).Trim().Equals(string.Empty))
                        continue;

                    Property.EXCELSETTING_ID = Val.ToInt32(Dr["EXCELSETTING_ID"]);
                    Property.EXCELSETTINGNAME = Val.ToString(Dr["EXCELSETTINGNAME"]);
                    Property.EXCELSETTINGREFNAME = Val.ToString(Dr["EXCELSETTINGREFNAME"]);
                    Property.SEQUENCENO = Val.ToInt32(Dr["SEQUENCENO"]);
                    Property.ISACTIVE = Val.ToBoolean(Dr["ISACTIVE"]);
                    Property.ISCOMPULSORYINSINGLE = Val.ToBoolean(Dr["ISCOMPULSORYINSINGLE"]);
                    Property.ISCOMPULSORYINPARCEL = Val.ToBoolean(Dr["ISCOMPULSORYINPARCEL"]);
                    Property.REMARK = Val.ToString(Dr["REMARK"]);
                    Property = ObjMast.Save(Property);

                    ReturnMessageDesc = Property.ReturnMessageDesc;
                    ReturnMessageType = Property.ReturnMessageType;

                    Property = null;
                }
                DtabExcelSetting.AcceptChanges();

                Global.Message(ReturnMessageDesc);

                if (ReturnMessageType == "SUCCESS")
                {
                    Clear();
                    //BtnAdd_Click(null, null);
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
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        public void Fill()
        {

            Guid gLedger_ID;
            gLedger_ID = Val.ToString(txtPartyName.Tag).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtPartyName.Tag));

            DtabExcelSetting = ObjMast.Fill(gLedger_ID);

            if (DtabExcelSetting.Rows.Count > 0)
            {
                int MaxSrNo = 0;
                MaxSrNo = (int)DtabExcelSetting.Compute("Max(SEQUENCENO)", "");
                DataRow Dr = DtabExcelSetting.NewRow();
                //Dr["SEQUENCENO"] = MaxSrNo + 1;
                //DtabExcelSetting.Rows.Add(Dr);
                //DtabExcelSetting.AcceptChanges();

            }
            else
            {
                Global.Message("Default Setting Is Not Exists");
                Clear();
                return;
            }

            if (Val.ToInt32(DtabExcelSetting.Rows[0]["ISDEFAULTSETTING"]) == 1)
            {
                lblStatus.Visible = true;
                lblStatus.Text = "(Default Setting)";
                lblStatus.ForeColor = Color.Maroon;
            }
            else
                lblStatus.Visible = false;

            MainGrid.DataSource = DtabExcelSetting;
            MainGrid.Refresh();
            GrdDet.FocusedColumn = GrdDet.Columns[0];
            GrdDet.FocusedRowHandle = 0;
            GrdDet.ShowEditor();

            //DtabExcelSetting.Rows.Add(DtabExcelSetting.NewRow());

        }

        public bool CheckDuplicate(string ColName, string ColValue,int IntRowIndex, string StrMsg)
        {
            if (Val.ToString(ColValue).Trim().Equals(string.Empty))
                return false;

            var Result = from row in DtabExcelSetting.AsEnumerable()
                         where Val.ToString(row[ColName]).ToUpper() == Val.ToString(ColValue).ToUpper() && row.Table.Rows.IndexOf(row) != IntRowIndex
                         select row;


            if (Result.Any())
            {
                Global.Message(StrMsg + " ALREADY EXISTS.");
                return true;
            }
            return false;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("Excel Setting", GrdDet);
        }

        private void repTxtRemark_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {


                    DataRow dr = GrdDet.GetFocusedDataRow();
                    if (!Val.ToString(dr["EXCELSETTINGNAME"]).Equals(string.Empty) && !Val.ToString(dr["EXCELSETTINGREFNAME"]).Equals(string.Empty) && GrdDet.IsLastRow)
                    {
                        int MaxSrNo = 0;
                        MaxSrNo = (int)DtabExcelSetting.Compute("Max(SEQUENCENO)", "");
                        DataRow Dr = DtabExcelSetting.NewRow();
                        Dr["SEQUENCENO"] = MaxSrNo + 1;
                        DtabExcelSetting.Rows.Add(Dr);
                        DtabExcelSetting.AcceptChanges();

                        //DtabPara.AcceptChanges();
                        GrdDet.FocusedColumn = GrdDet.Columns[0];
                        GrdDet.FocusedRowHandle = GrdDet.FocusedRowHandle + 1;
                        GrdDet.Focus();

                        //DtabExcelSetting.Rows.Add(DtabExcelSetting.NewRow());

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

        private void deleteSelectedAmountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle >= 0)
                {
                    if (Global.Confirm("ARE YOU SURE YOU WANT TO DELETE ENTRY") == System.Windows.Forms.DialogResult.Yes)
                    {
                        ExcelSettingMasterProperty Property = new ExcelSettingMasterProperty();
                        DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                        Property.EXCELSETTING_ID = Val.ToInt32(Drow["EXCELSETTING_ID"]);
                        Property.LEDGER_ID = Guid.Empty;
                        Property = ObjMast.Delete(Property);

                        if (Property.ReturnMessageType == "SUCCESS")
                        {
                            Global.Message("ENTRY DELETED SUCCESSFULLY");
                            DtabExcelSetting.Rows.RemoveAt(GrdDet.FocusedRowHandle);
                            DtabExcelSetting.AcceptChanges();
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


     
        private void repTxtExcelSettingName_Validating(object sender, CancelEventArgs e)
        {
            if (GrdDet.FocusedRowHandle <= 0)
            {
                return;
            }
            DataRow Dr = GrdDet.GetFocusedDataRow();
            if (CheckDuplicate("EXCELSETTINGNAME", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "Name"))
            {
                e.Cancel = true;
                return;
            }

            else
            {
                if (Val.ToString(Dr["EXCELSETTINGREFNAME"]).Trim().Equals(string.Empty))
                {
                    Dr["EXCELSETTINGREFNAME"] = Val.ToString(GrdDet.EditingValue);
                    DtabExcelSetting.AcceptChanges();
                }
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void repTxtExcelSettingRefName_Validating(object sender, CancelEventArgs e)
        {
            DataRow Dr = GrdDet.GetFocusedDataRow();
            if (CheckDuplicate("EXCELSETTINGREFNAME", Val.ToString(GrdDet.EditingValue), GrdDet.FocusedRowHandle, "Reference Value"))
                e.Cancel = true;
            return;

        }

        private void BtnClearAllRows_Click(object sender, EventArgs e)
        {
            try
            {
                txtPartyName.Text = string.Empty;
                txtPartyName.Tag = string.Empty;

                Fill();
             
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnDeleteTheme_Click(object sender, EventArgs e)
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
                            ExcelSettingMasterProperty Property = new ExcelSettingMasterProperty();
                            DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                            Property.EXCELSETTING_ID = 0;
                            Property.LEDGER_ID = Guid.Parse(Val.ToString(txtPartyName.Tag));
                            Property = ObjMast.Delete(Property);

                            if (Property.ReturnMessageType == "SUCCESS")
                            {
                                Global.Message("ENTRY DELETED SUCCESSFULLY");
                                DtabExcelSetting.Rows.RemoveAt(GrdDet.FocusedRowHandle);
                                DtabExcelSetting.AcceptChanges();
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



        private void GrdDet_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {

                if (e == null || e.FocusedRowHandle < 0)
                {
                    return;
                }

                DataRow Drow = GrdDet.GetFocusedDataRow();
                if (Drow != null)
                {
                    FilterAdditionalInfo(Drow);
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }

        }
        private void FilterAdditionalInfo(DataRow Drow)
        {
            try
            {


                DtabExcelSetting_TEMP.Rows.Clear();

                //IEnumerable<DataRow> rowsNewReset = DtabExcelSetting_TEMP.Rows.Cast<DataRow>();
                //rowsNewReset
                //.ToList().ForEach(
                //                    r =>
                //                    {
                //                        r["EXCELSETTING_ID"] = DBNull.Value;
                //                        r["SEQUENCENO"] = DBNull.Value;
                //                        r["EXCELSETTINGREFNAME"] = DBNull.Value;
                //                        r["EXCELSETTINGNAME"] = DBNull.Value;
                //                    }
                //                 );

                var Qry = from Dr in DtabExcelSetting.AsEnumerable()
                          where (Val.ToString(Dr["EXCELSETTING_ID"]) == Val.ToString(Drow["EXCELSETTING_ID"]))
                          //orderby Val.ToInt32(Dr["SEQUENCENO"])
                          select Dr;
                if (Qry.Any())
                {
                    foreach (DataRow Dr in Qry)
                    {
                        List<String> SettingRefValue = new List<String>(Val.ToString(Dr["EXCELSETTINGREFNAME"]).Trim().Split(','));


                        int IntCount = 1;
                        foreach (string item in SettingRefValue)
                        {
                            DataRow DRS = DtabExcelSetting_TEMP.NewRow();
                            DRS["EXCELSETTING_ID"] = Dr["EXCELSETTING_ID"];
                            DRS["EXCELSETTINGNAME"] = Dr["EXCELSETTINGNAME"];
                            DRS["EXCELSETTINGREFNAME"] = item;
                            DRS["SEQUENCENO"] = IntCount;

                            DtabExcelSetting_TEMP.Rows.Add(DRS);

                            IntCount++;
                        }
                    }

                    string strExcelsetting_Id = (Val.ToString(Drow["EXCELSETTING_ID"]).Trim().Equals(string.Empty) ? "0" : Val.ToString(Drow["EXCELSETTING_ID"]));
                    int IntSrno = 0;
                    if (strExcelsetting_Id == "0")
                        IntSrno = 0;
                    else
                        IntSrno = (int)DtabExcelSetting_TEMP.Compute("Max(SEQUENCENO)", "EXCELSETTING_ID = " + strExcelsetting_Id);
                    //int IntSrno = Val.ToInt(DtabExcelSetting_TEMP.Select("Max(SEQUENCENO", ""));
                    DataRow DRSNew = DtabExcelSetting_TEMP.NewRow();
                    DRSNew["EXCELSETTING_ID"] = Drow["EXCELSETTING_ID"];
                    DRSNew["EXCELSETTINGNAME"] = Drow["EXCELSETTINGNAME"];
                    DRSNew["EXCELSETTINGREFNAME"] = "";
                    DRSNew["SEQUENCENO"] = IntSrno + 1;

                    DtabExcelSetting_TEMP.Rows.Add(DRSNew);
                }
                else
                {
                    //IEnumerable<DataRow> rowsNew = DtDetailAdditionalInfo_Temp.Rows.Cast<DataRow>();
                    //rowsNew
                    //.ToList().ForEach(
                    //                    r =>
                    //                    {
                    //                        r["SRNO"] = Val.ToInt32(Drow["SRNO"]);
                    //                    }
                    //                 );
                }

                DtabExcelSetting_TEMP.AcceptChanges(); // Add : Narendra : 19-09-2017
                GrdSetting.RefreshData();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        private void repTxtRefValue_Validating(object sender, CancelEventArgs e)
        {
            if (GrdDet.FocusedRowHandle < 0)
                return;


            GrdSetting.PostEditor();
            DataRow DrSetting = GrdSetting.GetFocusedDataRow();

            if (DrSetting == null)
                return;

            if (CheckDuplicateForSetting("EXCELSETTINGREFNAME", Val.ToString(GrdSetting.EditingValue), GrdSetting.FocusedRowHandle, "Reference Value"))
            {
                //GrdSetting.SetRowCellValue(GrdSetting.FocusedRowHandle, "EXCELSETTINGREFNAME", "");
                //GrdSetting.FocusedRowHandle = GrdSetting.FocusedRowHandle;
                //GrdSetting.FocusedColumn = GrdSetting.Columns[1];
                //GrdSetting.Focus();
                e.Cancel = true;
                return;
            }
            else
            {
                try
                {

                    DrSetting["EXCELSETTINGREFNAME"] = GrdSetting.EditingValue;
                    DtabExcelSetting_TEMP.AcceptChanges();

                    var Qry = from Dr2 in DtabExcelSetting_TEMP.AsEnumerable()
                              where (Val.ToString(Dr2["EXCELSETTING_ID"]).Trim() == Val.ToString(DrSetting["EXCELSETTING_ID"]))
                              select Dr2;


                    if (Qry.Any())
                    {
                        string STR = "";
                        foreach (DataRow Dr2 in Qry)
                        {
                            STR = STR + "," + Val.ToString(Dr2["EXCELSETTINGREFNAME"]);

                            if (Val.ToString(Dr2["EXCELSETTING_ID"]).Trim().Equals(string.Empty) || Val.ToString(Dr2["EXCELSETTINGREFNAME"]).Trim().Equals(string.Empty))
                                continue;


                            IEnumerable<DataRow> rowsNew = DtabExcelSetting.Rows.Cast<DataRow>();
                            rowsNew.Where(s => Val.ToInt32(s["EXCELSETTING_ID"]) == Val.ToInt32(Dr2["EXCELSETTING_ID"]))
                            .ToList().ForEach(
                                                r =>
                                                {
                                                    r["EXCELSETTINGREFNAME"] = STR.Trim().TrimStart(',');
                                                }
                                             );
                        }
                        DtabExcelSetting.AcceptChanges();
                    }

                    //GrdSetting.PostEditor();
                    //DataRow DrSetting = GrdSetting.GetFocusedDataRow();
                    if (!Val.ToString(DrSetting["EXCELSETTINGNAME"]).Equals(string.Empty) && !Val.ToString(DrSetting["EXCELSETTINGREFNAME"]).Equals(string.Empty) && GrdSetting.IsLastRow)
                    {
                        int MaxSrNo = 0;
                        MaxSrNo = (int)DtabExcelSetting_TEMP.Compute("Max(SEQUENCENO)", "EXCELSETTING_ID = " + Val.ToString(DrSetting["EXCELSETTING_ID"]));
                        DataRow DrNew = DtabExcelSetting_TEMP.NewRow();
                        DrNew["EXCELSETTING_ID"] = DrSetting["EXCELSETTING_ID"];
                        DrNew["EXCELSETTINGNAME"] = DrSetting["EXCELSETTINGNAME"];
                        DrNew["EXCELSETTINGREFNAME"] = "";
                        DrNew["SEQUENCENO"] = MaxSrNo + 1;

                        DtabExcelSetting_TEMP.Rows.Add(DrNew);
                        DtabExcelSetting_TEMP.AcceptChanges();

                        GrdSetting.FocusedRowHandle = GrdSetting.FocusedRowHandle + 1;
                        GrdSetting.FocusedColumn = GrdSetting.Columns["SEQUENCENO"];
                        GrdSetting.Focus();
                    }
                    else if (GrdSetting.IsLastRow)
                    {
                        //BtnSave.Focus();
                        //e.Handled = true;
                    }
                }
                catch (Exception ex)
                {
                    Global.Message(ex.Message.ToString());
                }
            }
            
            //try
            //{
            //    DtabExcelSetting_TEMP.AcceptChanges();
            //    DataRow DrSetting = GrdSetting.GetFocusedDataRow();


            //    DtabExcelSetting_TEMP.AcceptChanges();

            //    var Qry = from Dr in DtabExcelSetting_TEMP.AsEnumerable()
            //              where (Val.ToString(Dr["EXCELSETTING_ID"]).Trim() == Val.ToString(DrSetting["EXCELSETTING_ID"]))
            //              select Dr;

            //    //foreach (var item in DtabExcelSetting)
            //    //    str = str + item + ",";

            //    //string str = string.Join(",", Qry.ToArray());

            //    if (Qry.Any())
            //    {
            //        string STR = "";
            //        foreach (DataRow Dr in Qry)
            //        {
            //            STR = STR + "," + Val.ToString(Dr["EXCELSETTINGREFNAME"]);

            //            if (Val.ToString(Dr["EXCELSETTINGREFNAME"]).Trim().Equals(string.Empty))
            //            {

            //            }

            //            IEnumerable<DataRow> rowsNew = DtabExcelSetting.Rows.Cast<DataRow>();
            //            rowsNew.Where(s => Val.ToInt32(s["EXCELSETTING_ID"]) == Val.ToInt32(Dr["EXCELSETTING_ID"]))
            //            .ToList().ForEach(
            //                                r =>
            //                                {
            //                                    r["EXCELSETTINGREFNAME"] = STR.Trim().TrimStart(',');
            //                                }
            //                             );
            //        }
            //        DtabExcelSetting.AcceptChanges();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Global.Message(ex.Message.ToString());
            //}

        }


        private void repBtnDeleteSetting_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdSetting.FocusedRowHandle < 0)
                {
                    return;
                }
                DataRow DRow = GrdSetting.GetFocusedDataRow();
                //if (Global.Confirm("Are You Sure To Delete File :- " + Val.ToString(DRow["FILENO"]) + "?") == System.Windows.Forms.DialogResult.Yes)
                //{

                        string SETTING_ID = Val.ToString(DRow["EXCELSETTING_ID"]);

                        GrdSetting.DeleteRow(GrdSetting.FocusedRowHandle);
                        GrdSetting.RefreshData();

                        DtabExcelSetting_TEMP.AcceptChanges();
                        //Dr["EXCELSETTING_ID", DataRowVersion.Original]
                        var Qry = from Dr in DtabExcelSetting_TEMP.AsEnumerable()
                                  where (Val.ToString(Dr["EXCELSETTING_ID"]).Trim() == Val.ToString(SETTING_ID))
                                  select Dr;

                        if (Qry.Any())
                        {
                            string STR = "";
                            foreach (DataRow Dr in Qry)
                            {
                                STR = STR + "," + Val.ToString(Dr["EXCELSETTINGREFNAME"]);

                                if (Val.ToString(Dr["EXCELSETTING_ID"]).Trim().Equals(string.Empty) || Val.ToString(Dr["EXCELSETTINGREFNAME"]).Trim().Equals(string.Empty))
                                    continue;

                                IEnumerable<DataRow> rowsNew = DtabExcelSetting.Rows.Cast<DataRow>();
                                rowsNew.Where(s => Val.ToInt32(s["EXCELSETTING_ID"]) == Val.ToInt32(Dr["EXCELSETTING_ID"]))
                                .ToList().ForEach(
                                                    r =>
                                                    {
                                                        r["EXCELSETTINGREFNAME"] = STR.Trim().TrimStart(',');
                                                    }
                                                 );
                            }
                            DtabExcelSetting.AcceptChanges();
                        }

                //}

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void repTxtRefValue_Leave(object sender, EventArgs e)
        {
            try
            {

                if (GrdSetting.FocusedRowHandle < 0)
                    return;

                GrdSetting.PostEditor();
                DataRow DrSetting = GrdSetting.GetFocusedDataRow();

                if (DrSetting == null)
                    return;

                if (!Val.ToString(DrSetting["EXCELSETTINGNAME"]).Equals(string.Empty) && !Val.ToString(DrSetting["EXCELSETTINGREFNAME"]).Equals(string.Empty) && GrdSetting.IsLastRow)
                {
                    int MaxSrNo = 0;
                    MaxSrNo = (int)DtabExcelSetting_TEMP.Compute("Max(SEQUENCENO)", "EXCELSETTING_ID = " + Val.ToString(DrSetting["EXCELSETTING_ID"]));
                    DataRow DrNew = DtabExcelSetting_TEMP.NewRow();
                    DrNew["EXCELSETTING_ID"] = DrSetting["EXCELSETTING_ID"];
                    DrNew["EXCELSETTINGNAME"] = DrSetting["EXCELSETTINGNAME"];
                    DrNew["EXCELSETTINGREFNAME"] = "";
                    DrNew["SEQUENCENO"] = MaxSrNo + 1;

                    DtabExcelSetting_TEMP.Rows.Add(DrNew);
                    DtabExcelSetting_TEMP.AcceptChanges();

                    GrdSetting.FocusedRowHandle = GrdSetting.FocusedRowHandle + 1;
                    GrdSetting.FocusedColumn = GrdSetting.Columns["SEQUENCENO"];
                    GrdSetting.Focus();
                }
                else if (GrdSetting.IsLastRow)
                {
                    //BtnSave.Focus();
                    //e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtPartyName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PURCHASEPARTY);

                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtPartyName.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtPartyName.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);

                        Fill();
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

        private void repChkIsCompulsory_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataRow dr = GrdDet.GetFocusedDataRow();
                if (!Val.ToString(dr["EXCELSETTINGNAME"]).Equals(string.Empty) && !Val.ToString(dr["EXCELSETTINGREFNAME"]).Equals(string.Empty) && GrdDet.IsLastRow)
                {
                    int MaxSrNo = 0;
                    MaxSrNo = (int)DtabExcelSetting.Compute("Max(SEQUENCENO)", "");
                    DataRow Dr = DtabExcelSetting.NewRow();
                    Dr["SEQUENCENO"] = MaxSrNo + 1;
                    Dr["EXCELSETTING_ID"] = 0;
                    DtabExcelSetting.Rows.Add(Dr);
                    DtabExcelSetting.AcceptChanges();

                    //DtabPara.AcceptChanges();
                    GrdDet.FocusedColumn = GrdDet.Columns[0];
                    GrdDet.FocusedRowHandle = GrdDet.FocusedRowHandle + 1;
                    GrdDet.Focus();

                    //DtabExcelSetting.Rows.Add(DtabExcelSetting.NewRow());

                }
                else if (GrdDet.IsLastRow)
                {
                    BtnSave.Focus();
                    e.Handled = true;
                }
            }
        }

        private void BtnLeft_Click(object sender, EventArgs e)
        {
            //if (IsNextImage)
            //{
            //    BtnLeft.Image = MahantExport.Properties.Resources.A1;
            //    PnlCopyPaste.Visible = false;
            //    IsNextImage = false;
            //}
            //else
            //{
            //    BtnLeft.Image = MahantExport.Properties.Resources.A2;
            //    PnlCopyPaste.Visible = true;
            //    IsNextImage = true;
            //    txtCopyToPartySetting.Focus();
            //}
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                //if (Val.ToString(txtPartyName.Text).Trim().Equals(string.Empty))
                //{
                //    Global.Message("Please Select Party First.");
                //    txtPartyName.Focus();
                //}
                //else 
                if (Val.ToString(txtCopyToPartySetting.Text).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Select Copy To Party First.");
                    txtCopyToPartySetting.Focus();
                }
                
                ExcelSettingMasterProperty Property = new ExcelSettingMasterProperty();

                Guid gCopyParty = Val.ToString(txtCopyToPartySetting.Tag).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtCopyToPartySetting.Tag));

                Property.LEDGER_ID = Guid.Parse(Val.ToString(txtPartyName.Tag));
                Property = ObjMast.SaveCopySettingPartyWise(Property,gCopyParty);

                Global.Message(Property.ReturnMessageDesc);

                if (Property .ReturnMessageType == "SUCCESS")
                {
                    Clear();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtCopyToPartySetting_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME,PARTYTYPE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PARTY);

                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtCopyToPartySetting.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtCopyToPartySetting.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
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

        private void BtnNewRowInMainGrid_Click(object sender, EventArgs e)
        {
            try
            {
                DtabExcelSetting.Rows.Add(DtabExcelSetting.NewRow());
                DtabExcelSetting.AcceptChanges();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
    }

}
