using BusLib.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BusLib.Configuration;
using BusLib.TableName;
using System.Data.SqlClient;
using MahantExport;
using System.IO;
using System.Text.RegularExpressions;
using BusLib.Parcel;


namespace MahantExport.Parcel
{
    public partial class FrmParcelStockTally  : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_ParcelStockTally ObjStockTally = new BOTRN_ParcelStockTally();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();

        DataTable DTabStockTally = new DataTable();
        bool mISSaleRateUpdate = false;

        public FrmParcelStockTally()
        {
            InitializeComponent();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = false;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjStockTally);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            txtPassword.Tag = ObjPer.PASSWORD;
            BtnContinue_Click(null, null);
            this.Show();

        }


        #region function

        public void Clears()
        {
            DTabStockTally.Rows.Clear();
        }

        public void Fill()
        {
            int Dept_ID = Val.ToInt(BOConfiguration.gEmployeeProperty.DEPARTMENT_ID) == 436 ? 0 : Val.ToInt(BOConfiguration.gEmployeeProperty.DEPARTMENT_ID); //436:Admin Dept

            DTabStockTally = ObjStockTally.Fill(Val.SqlDate(DTPStockTallyDate.Value.ToShortDateString()), Dept_ID);
            MainGrid.DataSource = DTabStockTally;
            MainGrid.Refresh();
        }

        #endregion

        private void GrdDet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                int Dept_ID = Val.ToInt(BOConfiguration.gEmployeeProperty.DEPARTMENT_ID) == 436 ? 0 : Val.ToInt(BOConfiguration.gEmployeeProperty.DEPARTMENT_ID); //436:Admin Dept
                DTabStockTally = ObjStockTally.Fill(Val.SqlDate(DTPStockTallyDate.Value.ToShortDateString()), Dept_ID);
                MainGrid.DataSource = DTabStockTally;
                MainGrid.Refresh();
                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }

        }

        private void GrdDet_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000")
            {
                e.DisplayText = String.Empty;
            }
        }

        private void DeleteSelectedSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle >= 0)
                {
                    if (Global.Confirm("ARE YOU SURE YOU WANT TO DELETE ENTRY ?") == System.Windows.Forms.DialogResult.Yes)
                    {
                        StockTallyProperty Property = new StockTallyProperty();
                        DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                        Property.STOCKTALLY_ID = Guid.Parse(Val.ToString(Drow["STOCKTALLY_ID"]));

                        Property = ObjStockTally.Delete(Property);

                        if (Property.ReturnMessageType == "SUCCESS")
                        {
                            Global.Message("ENTRY DELETED SUCCESSFULLY");
                            DTabStockTally.Rows.RemoveAt(GrdDet.FocusedRowHandle);
                            DTabStockTally.AcceptChanges();
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

        private void BtnShow_Click(object sender, EventArgs e)
        {
            Fill();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string ReturnMessageDesc = "";
                string ReturnMessageType = "";



                foreach (DataRow Dr in DTabStockTally.Rows)
                {
                    StockTallyProperty Property = new StockTallyProperty();

                    Property.STOCKTALLYDATE = Val.SqlDate(DTPStockTallyDate.Value.ToShortDateString());

                    Property.STOCKTALLY_ID = Val.ToString(Dr["STOCKTALLY_ID"]).Trim().Equals(string.Empty) ? BusLib.Configuration.BOConfiguration.FindNewSequentialID() : Guid.Parse(Val.ToString(Dr["STOCKTALLY_ID"]));
                    Property.STOCK_ID = Guid.Parse(Val.ToString(Dr["STOCK_ID"]));
                    Property.SHAPE_ID = Val.ToInt32(Dr["SHAPE_ID"]);
                    Property.SIZE_ID = Val.ToInt32(Dr["SIZE_ID"]);
                    Property.CLARITY_ID = Val.ToInt32(Dr["CLARITY_ID"]);
                    Property.BALANCECARAT = Val.Val(Dr["BALANCECARAT"]);
                    Property.ACTUALCARAT = Val.Val(Dr["ACTUALCARAT"]);
                    Property.PLUSCARAT = Val.Val(Dr["PLUSCARAT"]);
                    Property.LOSSCARAT = Val.Val(Dr["LOSSCARAT"]);
                    Property.SALERATE = Val.Val(Dr["SALEPRICEPERCARAT"]);
                    Property.OLDSALERATE = Val.Val(Dr["OLDSALEPRICEPERCARAT"]);
                    Property.STOCKNO = Val.ToString(Dr["STOCKNO"]);
                    Property.STOCKTALLYDATE = Val.SqlDate(DTPStockTallyDate.Value.ToShortDateString());
                    Property.ISSALERATEUPDATE = mISSaleRateUpdate == true && Val.ToInt(Dr["ISSALERATEUPDATE"]) == 1 ? 1 : 0;
                    Property.REMARK = Val.ToString(Dr["REMARK"]);
                    Property = ObjStockTally.Save(Property);

                    ReturnMessageDesc = Property.ReturnMessageDesc;
                    ReturnMessageType = Property.ReturnMessageType;

                    if (Property.ReturnMessageType == "FAIL")
                    {
                        break;
                    }

                    Property = null;
                }
                DTabStockTally.AcceptChanges();

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

        private void GrdDet_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }

                DataRow DRow = GrdDet.GetDataRow(e.RowHandle);

                if (e.Column.FieldName == "ACTUALCARAT")
                {
                    if (Val.Val(DRow["BALANCECARAT"]) < Val.Val(DRow["ACTUALCARAT"]))
                    {
                        //DTabStockTally.Rows[e.RowHandle]["PLUSCARAT"] = Math.Round(Val.Val(DRow["ACTUALCARAT"]) - Val.Val(DRow["BALANCECARAT"]), 3);
                        //DTabStockTally.Rows[e.RowHandle]["LOSSCARAT"] = "0.00";
                        DRow["PLUSCARAT"] = Math.Round(Val.Val(DRow["ACTUALCARAT"]) - Val.Val(DRow["BALANCECARAT"]), 3);
                        DRow["LOSSCARAT"] = "0.00";
                    }
                    else
                    {
                        //DTabStockTally.Rows[e.RowHandle]["PLUSCARAT"] = "0.00";
                        //DTabStockTally.Rows[e.RowHandle]["LOSSCARAT"] = Math.Round(Val.Val(DRow["BALANCECARAT"]) - Val.Val(DRow["ACTUALCARAT"]), 3);
                        DRow["PLUSCARAT"] = "0.00";
                        DRow["LOSSCARAT"] = Math.Round(Val.Val(DRow["BALANCECARAT"]) - Val.Val(DRow["ACTUALCARAT"]), 3);
                    }
                }
                else if (e.Column.FieldName == "SALEPRICEPERCARAT")
                {
                    //DTabStockTally.Rows[e.RowHandle]["ISSALERATEUPDATE"] = "1";
                    DRow["ISSALERATEUPDATE"] = "1";
                }
                DTabStockTally.AcceptChanges();
            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
            }

        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Clears();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("Stock Tally List", GrdDet);

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (Val.ToString(txtPassword.Tag) != "" && Val.ToString(txtPassword.Tag).ToUpper() == txtPassword.Text.ToUpper())
            {
                mISSaleRateUpdate = true;
                GrdDet.Columns["SALEPRICEPERCARAT"].OptionsColumn.AllowEdit = true;
            }
            else
            {
                mISSaleRateUpdate = false;
                GrdDet.Columns["SALEPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
            }
        }

    }
}
