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
    public partial class FrmFancyColor : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOMST_FancyColor ObjFancyColor = new BOMST_FancyColor();

        DataTable DTabFancyColor = new DataTable();

        public FrmFancyColor()
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
            ObjFormEvent.ObjToDisposeList.Add(ObjFancyColor);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        public bool CheckDuplicate(string ColName, string ColValue, int IntRowIndex, string StrMsg)
        {
            if (Val.ToString(ColValue).Trim().Equals(string.Empty))
                return false;

            var Result = from row in DTabFancyColor.AsEnumerable()
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
            foreach (DataRow dr in DTabFancyColor.Rows)
            {

                //For Update Validation
                if (Val.ToString(dr["FANCYCOLORNAME"]).Trim().Equals(string.Empty) && !Val.ToString(dr["FANCYCOLOR_ID"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter Fancy Color Name");
                    IntCol = 0;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }
                //end as

                if (Val.ToString(dr["SHORTNAME"]).Trim().Equals(string.Empty))
                {
                    if (DTabFancyColor.Rows.Count == 1)
                    {
                        Global.Message("Please Enter SHORTNAME");
                        IntCol = 1;
                        IntRow = dr.Table.Rows.IndexOf(dr);
                        break;

                    }
                    else
                        continue;
                }

                if (Val.ToString(dr["FANCYCOLORNAME"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter FANCY COLOR NAME");
                    IntCol = 1;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }
                if (Val.ToString(dr["SHORTNAME"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter SHORTNAME");
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
            DTabFancyColor.Rows.Clear();
            DTabFancyColor.Rows.Add(DTabFancyColor.NewRow());
            Fill();
        }

        public void Fill()
        {
            DTabFancyColor = ObjFancyColor.Fill();
            DTabFancyColor.Rows.Add(DTabFancyColor.NewRow());
            MainGrid.DataSource = DTabFancyColor;
            MainGrid.Refresh();
            GrdDet.BestFitColumns();
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

                foreach (DataRow Dr in DTabFancyColor.Rows)
                {

                    if (Val.ToString(Dr["FANCYCOLORNAME"]).Trim().Equals(string.Empty))
                        continue;

                    FancyColorMasterProperty Property = new FancyColorMasterProperty();

                    Property.FANCYCOLOR_ID = Val.ToInt32(Dr["FANCYCOLOR_ID"]);
                    Property.FANCYCODE = Val.ToInt32(Dr["FANCYCODE"]);
                    Property.FANCYCOLORNAME = Val.ToString(Dr["FANCYCOLORNAME"]);
                    Property.SHORTNAME = Val.ToString(Dr["SHORTNAME"]);
                    Property.FANCYCOLOR = Val.ToString(Dr["FANCYCOLOR"]);
                    Property.FANCYOVERTONE = Val.ToString(Dr["FANCYOVERTONE"]);
                    Property.FANCYINTENSITY = Val.ToString(Dr["FANCYINTENSITY"]);
                    Property.ISACTIVE = Val.ToBoolean(Dr["ISACTIVE"]);
                    Property.REMARK = Val.ToString(Dr["REMARK"]);
                    
                    Property = ObjFancyColor.Save(Property);

                    ReturnMessageDesc = Property.ReturnMessageDesc;
                    ReturnMessageType = Property.ReturnMessageType;

                    Property = null;
                }
                DTabFancyColor.AcceptChanges();

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
                    if (Val.ToString(dr["FANCYCOLORNAME"])!= string.Empty && GrdDet.IsLastRow)
                    {
                        DTabFancyColor.Rows.Add(DTabFancyColor.NewRow());

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
            Global.ExcelExport("Fancy Color List", GrdDet);
        }

        private void DeleteSelectedSizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle >= 0)
                {
                    if (Global.Confirm("ARE YOU SURE YOU WANT TO DELETE ENTRY ?") == System.Windows.Forms.DialogResult.Yes)
                    {
                        FancyColorMasterProperty Property = new FancyColorMasterProperty();
                        DataRow Drow = GrdDet.GetDataRow(GrdDet.FocusedRowHandle);
                        Property.FANCYCOLOR_ID = Val.ToInt32(Drow["FANCYCOLOR_ID"]);

                        Property = ObjFancyColor.Delete(Property);

                        if (Property.ReturnMessageType == "SUCCESS")
                        {
                            Global.Message("ENTRY DELETED SUCCESSFULLY");
                            DTabFancyColor.Rows.RemoveAt(GrdDet.FocusedRowHandle);
                            DTabFancyColor.AcceptChanges();
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



    }
}
