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
    public partial class FrmSubscribeMaster : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFromEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
         BOMST_Subscribe ObjMast = new BOMST_Subscribe();
        DataTable DtabSubscribe = new DataTable();  
        string StrMesg = "";

        #region Property Settings

        public FrmSubscribeMaster()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            BtnSave.Enabled = ObjPer.ISINSERT;
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();
            clear();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFromEvent.mForm = this;
            ObjFromEvent.FormKeyDown = true;
            ObjFromEvent.FormKeyPress = true;
            ObjFromEvent.FormResize = true;
            ObjFromEvent.FormClosing = true;
            ObjFromEvent.ObjToDisposeList.Add(ObjFromEvent);
            ObjFromEvent.ObjToDisposeList.Add(ObjMast);
            ObjFromEvent.ObjToDisposeList.Add(Val);
            ObjFromEvent.ObjToDisposeList.Add(ObjPer);
        }

        #endregion

        //#region Validation

        //private bool ValSave()
        //{
        //    int IntCol = -1, IntRow = -1;
        //    foreach (DataRow dr in DtabSubscribe.Rows)
        //    {
        //        if (Val.ToString(dr["EMAILID"]).Trim().Equals(string.Empty) && !Val.ToString(dr["ISACTIVE"]).Trim().Equals(string.Empty))
        //        {
        //            Global.Message("Please Enter 'EMAILID'");
        //            IntCol = 0;
        //            IntRow = dr.Table.Rows.IndexOf(dr);
        //            break;
        //        }

        //        if (Val.ToString(dr["EMAILID"]).Trim().Equals(string.Empty))
        //        {
        //            if (DtabSubscribe.Rows.Count == 1)
        //            {
        //                Global.Message("Please Enter 'EMAILID'");
        //                IntCol = 0;
        //                IntRow = dr.Table.Rows.IndexOf(dr);
        //                break;
        //            }
        //            else
        //                continue;
        //        }
        //        if (IntRow >= 0)
        //        {
        //            GrdDet.FocusedRowHandle = IntRow;
        //            GrdDet.FocusedColumn = GrdDet.VisibleColumns[IntCol];
        //            GrdDet.Focus();
        //            return true;
        //        }
        //        return false;
        //    }
        //}
        //#endregion

        
        public void clear()
        {
            Fill();
            DtabSubscribe.Rows.Add(DtabSubscribe.NewRow());

            GrdDet.FocusedRowHandle = 0;
            GrdDet.FocusedColumn = GrdDet.VisibleColumns[0];
            GrdDet.Focus();
            GrdDet.ShowEditor();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {

                //if(ValSave())
                //{
                //    return;
                //}

                if(Global.Confirm("Are Your Sure To Save All Records ?") == System.Windows.Forms.DialogResult.No)
                    return;

                string ReturnMessageDesc = "";
                string ReturnMessageType = "";

                foreach (DataRow Dr in DtabSubscribe.Rows)
                {
                  SubscribeProperty Property = new SubscribeProperty();

                    if (Val.ToString(Dr["EMAILID"]).Trim ().Equals(string.Empty) || Val.ToString(Dr["ISACTIVE"]).Trim().Equals(string.Empty))
                        continue;

                    Property.EMAILID = Val.ToInt32(Dr["EMAILID"]);
                    Property.ISACTIVE = Val.ToBoolean(Dr["ISACTIVE"]);

                    Property = ObjMast.Save(Property);

                    ReturnMessageDesc = Property.ReturnMessageDesc;
                    ReturnMessageType = Property.ReturnMessageType;

                    Property = null;
                }
                Global.Message(ReturnMessageDesc);

                if (ReturnMessageType == "SUCCESS")
                {
                    clear();

                    if(GrdDet.RowCount > 1)
                    {
                        GrdDet.FocusedRowHandle = GrdDet.RowCount - 1;
                    }
                }
                else
                {
                  
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
        public void Fill()
        {
            DtabSubscribe = ObjMast.Fill();
            MainGrid.DataSource = DtabSubscribe;
            MainGrid.Refresh();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("Subscribe List",GrdDet);
        }

        public bool CheckDuplicate (string ColName , string ColValue, int IntRowIndex,string StrMsg)
        {
            if(Val.ToString(ColValue).Trim().Equals(string.Empty))
                return false;
            var Result =  from row in DtabSubscribe.AsEnumerable()
                          where Val.ToString(row[ColName]).Trim().ToUpper() == Val .ToString(ColValue).Trim().ToUpper() && row.Table.Rows.IndexOf(row) != IntRowIndex
                          select row;

            if(Result.Any())
            {
                Global.Message(StrMsg + "ALREADY EXISTS.");
                return true;
            }
            return false;
        }
    }
}
    
