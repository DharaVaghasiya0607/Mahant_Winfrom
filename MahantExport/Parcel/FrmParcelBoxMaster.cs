using BusLib.Configuration;
using BusLib.Master;
using BusLib.Parcel;
using BusLib.TableName;
using DevExpress.Data;
using MahantExport.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MahantExport.Parcel
{
    public partial class FrmParcelBoxMaster  : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        DataTable DTabBox = new DataTable();
        DataTable DTabpara = new DataTable();
        DataTable DTabSize = new DataTable();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();
        BOTRN_Box ObjMast = new BOTRN_Box();


        double DouCarat = 0;
        double DouPricePerCarat = 0;
        double DouAmount = 0;


        double DouOpeCarat = 0;
        double DouBalCarat = 0;

        public FrmParcelBoxMaster()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            DTabpara = new BOMST_Parameter().GetParameterData();
            BtnAdd_Click(null, null);
            Fill();

            this.Show();
        }

        public void ShowForm(DataTable pDTab)
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            DTabpara = new BOMST_Parameter().GetParameterData();
            BtnAdd_Click(null, null);
            if (pDTab != null)
            {
                MainGrdBox.DataSource = pDTab;
                GrdBox.RefreshData();
            }

            this.Show();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMast);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        private bool ValSave()
        {
            int IntCol = -1, IntRow = -1;
            foreach (DataRow dr in DTabBox.Rows)
            {
                //For Update Validation
                if (Val.ToString(dr["STOCKNO"]).Trim().Equals(string.Empty) && !Val.ToString(dr["STOCK_ID"]).Trim().Equals(string.Empty))
                {
                    Global.Message("PLEASE ENTER 'STOCKNO'");
                    IntCol = 0;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }
                //end as


                if (Val.ToString(dr["STOCKNO"]).Trim().Equals(string.Empty))
                {
                    if (DTabBox.Rows.Count == 1)
                    {
                        Global.Message("PLEASE ENTER 'STOCKNO'");
                        IntCol = 0;
                        IntRow = dr.Table.Rows.IndexOf(dr);
                        break;

                    }
                    else
                        continue;
                }

                if (Val.ToString(dr["SHAPENAME"]).Trim().Equals(string.Empty))
                {
                    Global.Message("PLEASE SELECT 'SHAPE'");
                    IntCol = 2;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }

                if (Val.ToString(dr["MIXSIZENAME"]).Trim().Equals(string.Empty))
                {
                    Global.Message("PLEASE SELECT 'SIZE'");
                    IntCol = 2;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }

                if (Val.ToString(dr["MIXCLARITYNAME"]).Trim().Equals(string.Empty))
                {
                    Global.Message("PLEASE SELECT 'CLARITY'");
                    IntCol = 2;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }

                if (Val.Val(dr["SALEPRICEPERCARAT"]) == 0)
                {
                    Global.Message("PLEASE SELECT 'SALE $/CTS'");
                    IntCol = 2;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }

            }
            if (IntRow >= 0)
            {
                GrdBox.FocusedRowHandle = IntRow;
                GrdBox.FocusedColumn = GrdBox.VisibleColumns[IntCol];
                GrdBox.Focus();
                return true;
            }
            return false;
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

                foreach (DataRow Dr in DTabBox.Rows)
                {
                    ParcelBoxMasterProperty Property = new ParcelBoxMasterProperty();

                    if (Val.ToString(Dr["STOCKNO"]).Trim().Equals(string.Empty) || Val.ToBoolean(Dr["ISONETIMEUPDATE"])==true)
                        continue;

                    Property.STOCK_ID = Val.ToString(Dr["STOCK_ID"]).Trim().Equals(string.Empty) ? BusLib.Configuration.BOConfiguration.FindNewSequentialID() : Guid.Parse(Val.ToString(Dr["STOCK_ID"]));

                    Property.STOCKNO = Val.ToString(Dr["STOCKNO"]);
                    Property.DEPARTMENT_ID = Val.ToInt32(Dr["DEPARTMENT_ID"]);
                    Property.SHAPE_ID = Val.ToInt32(Dr["SHAPE_ID"]);
                    Property.MIXSIZE_ID = Val.ToInt32(Dr["MIXSIZE_ID"]);
                    Property.MIXSIZENAME = Val.ToString(Dr["MIXSIZENAME"]);
                    Property.MIXCLARITY_ID = Val.ToInt32(Dr["MIXCLARITY_ID"]);
                    Property.MIXCLARITYNAME = Val.ToString(Dr["MIXCLARITYNAME"]);
                    Property.OPENINGCARAT = Val.Val(Dr["OPENINGCARAT"]);
                    Property.OPENINGPRICEPERCARAT = Val.Val(Dr["OPENINGPRICEPERCARAT"]);
                    Property.OPENINGAMOUNT = Val.Val(Dr["OPENINGAMOUNT"]);
                    Property.MFGPRICEPERCARAT = Val.Val(Dr["MFGPRICEPERCARAT"]);
                    Property.COSTPRICEPERCARAT = Val.Val(Dr["COSTPRICEPERCARAT"]);
                    Property.SALEPRICEPERCARAT = Val.Val(Dr["SALEPRICEPERCARAT"]);
                    Property.EXPPRICEPERCARAT = Val.Val(Dr["EXPPRICEPERCARAT"]);
                    Property.ISSALEPRICEPERCARAT = Val.ToInt32(Dr["ISSALEPRICEPERCARAT"]);

                    Property.PARCELGROUPNO = Val.ToString(Dr["PARCELGROUPNO"]);
                    Property.PARCELSEQNO = Val.ToInt(Dr["PARCELSEQNO"]);
                    //Property.ISONETIMEUPDATE = Val.ToBoolean(Dr["ISONETIMEUPDATE"]);
                    //Property.ISONETIMEUPDATE = Val.ToBoolean("True");

                    Property = ObjMast.Save(Property);

                    Dr["STOCK_ID"] = Property.RETURNVALUE;
                    ReturnMessageDesc = Property.RETURNMESSAGEDESC;
                    ReturnMessageType = Property.RETURNMESSAGETYPE; 

                    Property = null;
                }
                DTabBox.AcceptChanges();

                Global.Message(ReturnMessageDesc);

                if (ReturnMessageType == "SUCCESS")
                {
                    Fill();
                    if (GrdBox.RowCount > 1)
                    {
                       
                        GrdBox.FocusedRowHandle = GrdBox.RowCount - 1;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        public void Fill()
        {
            DTabBox = ObjMast.Fill();
            DTabBox.Rows.Add(DTabBox.NewRow());
            MainGrdBox.DataSource = DTabBox;
            MainGrdBox.Refresh();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            Clear();
            
        }

        public void Clear()
        {
            
            DTabBox.Rows.Clear();
            DTabBox.Rows.Add(DTabBox.NewRow());
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deleteSelectedItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdBox.FocusedRowHandle >= 0)
                {
                    if (Global.Confirm("ARE YOU SURE YOU WANT TO DELETE ENTRY") == System.Windows.Forms.DialogResult.Yes)
                    {
                        FrmPassword FrmPassword = new FrmPassword();
                        FrmPassword.ShowForm(ObjPer.PASSWORD);
                        ParcelBoxMasterProperty Property = new ParcelBoxMasterProperty();
                        DataRow Drow = GrdBox.GetDataRow(GrdBox.FocusedRowHandle);
                        Property.STOCK_ID = Guid.Parse(Val.ToString(Drow["STOCK_ID"]));
                        Property = ObjMast.Delete(Property);

                        if (Property.RETURNMESSAGETYPE == "SUCCESS")
                        {
                            Global.Message("ENTRY DELETED SUCCESSFULLY");
                            DTabBox.Rows.RemoveAt(GrdBox.FocusedRowHandle);
                            DTabBox.AcceptChanges();
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

        public bool CheckDuplicate(string ColName, string ColValue, int IntRowIndex, string StrMsg)
        {

            if (Val.ToString(ColValue).Trim().Equals(string.Empty))
                return false;

            var Result = from row in DTabBox.AsEnumerable()
                         where Val.ToString(row[ColName]).ToUpper() == Val.ToString(ColValue).ToUpper() && row.Table.Rows.IndexOf(row) != IntRowIndex
                         select row;


            if (Result.Any())
            {
                Global.Message("ALREADY EXISTS.");
                return true;
            }
            return false;
        }


        private void RepName_Validating(object sender, CancelEventArgs e)
        {
            if (GrdBox.FocusedRowHandle < 0)
                return;

            DataRow Dr = GrdBox.GetFocusedDataRow();
            if (CheckDuplicate("BOXNAME", Val.ToString(GrdBox.EditingValue), GrdBox.FocusedRowHandle, "NAME"))
                e.Cancel = true;
            return;
        }

        private void RepCode_Validating(object sender, CancelEventArgs e)
        {
            if (GrdBox.FocusedRowHandle < 0)
                return;

            DataRow Dr = GrdBox.GetFocusedDataRow();
            if (CheckDuplicate("BOXCODE", Val.ToString(GrdBox.EditingValue), GrdBox.FocusedRowHandle, "CODE"))
                e.Cancel = true;
            return;
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Application.StartupPath + "\\ParcelBox.xlsx", "CMD");
        }

        private void RepTxtExpPricePerCarat_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DataRow dr = GrdBox.GetFocusedDataRow();
                    if (!Val.ToString(dr["STOCKNO"]).Equals(string.Empty) && GrdBox.IsLastRow)
                    {
                        DTabBox.Rows.Add(DTabBox.NewRow());
                    }
                    else if (GrdBox.IsLastRow)
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

        private void RepTxtMixSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "MIXSIZENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_MIXSIZE); ;
                    FrmSearch.mStrColumnsToHide = "MIXSIZE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdBox.SetFocusedRowCellValue("MIXSIZENAME", (Val.ToString(FrmSearch.DRow["MIXSIZENAME"])));
                        GrdBox.SetFocusedRowCellValue("MIXSIZE_ID", (Val.ToString(FrmSearch.DRow["MIXSIZE_ID"])));
                        DataRow Dr = GrdBox.GetFocusedDataRow();

                        Dr["STOCKNO"] = Val.ToString(Dr["SHAPENAME"] + "-" + Val.ToString(Dr["MIXCLARITYNAME"] + "-" + Val.ToString(GrdBox.EditingValue)));
                        DTabBox.AcceptChanges();
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

        private void RepTxtDepartment_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "DEPARTMENTNAME, DEPARTMENTCODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_DEPARTMENT); ;
                    FrmSearch.mStrColumnsToHide = "DEPARTMENT_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdBox.SetFocusedRowCellValue("DEPARTMENTNAME", (Val.ToString(FrmSearch.DRow["DEPARTMENTNAME"])));
                        GrdBox.SetFocusedRowCellValue("DEPARTMENT_ID", (Val.ToString(FrmSearch.DRow["DEPARTMENT_ID"])));
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

        private void RepTxtShape_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "SHAPENAME, SHAPECODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SHAPE); ;
                    FrmSearch.mStrColumnsToHide = "SHAPE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdBox.SetFocusedRowCellValue("SHAPENAME", (Val.ToString(FrmSearch.DRow["SHAPECODE"])));
                        GrdBox.SetFocusedRowCellValue("SHAPE_ID", (Val.ToString(FrmSearch.DRow["SHAPE_ID"])));
                        DataRow Dr = GrdBox.GetFocusedDataRow();
                        Dr["STOCKNO"] = Val.ToString(GrdBox.EditingValue) + "-" + Val.ToString(Dr["MIXCLARITYNAME"] + "-" + Val.ToString(Dr["MIXSIZENAME"]));
                        DTabBox.AcceptChanges();
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

        private void TxtRepMixCla_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "MIXCLARITYNAME, MIXCLARITYCODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_MIXCLARITY); ;
                    FrmSearch.mStrColumnsToHide = "MIXCLARITY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdBox.SetFocusedRowCellValue("MIXCLARITYNAME", (Val.ToString(FrmSearch.DRow["MIXCLARITYNAME"])));
                        GrdBox.SetFocusedRowCellValue("MIXCLARITY_ID", (Val.ToString(FrmSearch.DRow["MIXCLARITY_ID"])));
                        DataRow Dr = GrdBox.GetFocusedDataRow();
                        Dr["STOCKNO"] = Val.ToString(Dr["SHAPENAME"] + "-" + Val.ToString(GrdBox.EditingValue) + "-" + Val.ToString(Dr["MIXSIZENAME"]));
                        DTabBox.AcceptChanges();
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

        private void GrdBox_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }
                if (e.Column.FieldName == "OPENINGCARAT" || e.Column.FieldName == "OPENINGPRICEPERCARAT")
                {
                    DataRow DRow = GrdBox.GetDataRow(e.RowHandle);
                    DTabBox.Rows[e.RowHandle]["OPENINGAMOUNT"] = Math.Round(Val.Val(DRow["OPENINGCARAT"]) * Val.Val(DRow["OPENINGPRICEPERCARAT"]), 2);
                    DTabBox.AcceptChanges();
                }
                else if (e.Column.FieldName == "SALEPRICEPERCARAT")
                {
                    DataRow DRow = GrdBox.GetDataRow(e.RowHandle);
                    if(Val.Val(DRow["OPENINGCARAT"]) != Val.Val(DRow["BALANCECARAT"]))
                        DTabBox.Rows[e.RowHandle]["ISSALEPRICEPERCARAT"] = 1;
                    DTabBox.AcceptChanges();
                }
                DataRow Dr = GrdBox.GetDataRow(e.RowHandle);
                Dr["ISONETIMEUPDATE"] = false;

                //SALEPRICEPERCARAT
            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
            }
        }

        private void GrdBox_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            try
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    DouCarat = 0;
                    DouPricePerCarat = 0;
                    DouAmount = 0;
                }
                else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DouCarat = DouCarat + Val.Val(GrdBox.GetRowCellValue(e.RowHandle, "OPENINGCARAT"));
                    DouAmount = DouAmount + Val.Val(GrdBox.GetRowCellValue(e.RowHandle, "OPENINGAMOUNT"));
                }
                else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("OPENINGPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdBox_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)//hinal
        {
            if (e.FocusedRowHandle < 0)
                return;


            DataRow Dr = GrdBox.GetDataRow(e.FocusedRowHandle);

            if (Val.Val(Dr["OPENINGCARAT"]) != Val.Val(Dr["BALANCECARAT"]) && !Val.ToString(Dr["STOCK_ID"]).Trim().Equals(string.Empty))
            {
                //DTabBox.Rows[e.FocusedRowHandle]["ISSALEPRICEPERCARAT"] = 1;
                Dr["ISSALEPRICEPERCARAT"] = 1;
            }

            DTabBox.AcceptChanges();

            if (Val.ToInt(Dr["ISSALEPRICEPERCARAT"]) == 0)
            {
                GrdBox.Columns["DEPARTMENTNAME"].OptionsColumn.AllowEdit = true;
                GrdBox.Columns["MIXSIZENAME"].OptionsColumn.AllowEdit = true;
                GrdBox.Columns["SHAPENAME"].OptionsColumn.AllowEdit = true;
                GrdBox.Columns["MIXCLARITYNAME"].OptionsColumn.AllowEdit = true;
                GrdBox.Columns["OPENINGCARAT"].OptionsColumn.AllowEdit = true;
                GrdBox.Columns["OPENINGPRICEPERCARAT"].OptionsColumn.AllowEdit = true;
                GrdBox.Columns["STOCKNO"].OptionsColumn.AllowEdit = true;
                GrdBox.Columns["OPENINGAMOUNT"].OptionsColumn.AllowEdit = true;
                GrdBox.Columns["MFGPRICEPERCARAT"].OptionsColumn.AllowEdit = true;
                GrdBox.Columns["COSTPRICEPERCARAT"].OptionsColumn.AllowEdit = true;
                GrdBox.Columns["SALEPRICEPERCARAT"].OptionsColumn.AllowEdit = true;
                GrdBox.Columns["EXPPRICEPERCARAT"].OptionsColumn.AllowEdit = true;
                GrdBox.Columns["ISONETIMEUPDATE"].OptionsColumn.AllowEdit = true;
            }
            else
            {
                
                //txtPassword.Text = "";
            }
        }
    }
}

