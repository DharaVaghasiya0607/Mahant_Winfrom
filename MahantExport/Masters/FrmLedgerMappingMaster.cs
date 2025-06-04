using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
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
using System.Windows.Forms;

namespace MahantExport.Masters
{
    public partial class FrmLedgerMappingMaster : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_LedgerMapping ObjMast = new BOMST_LedgerMapping();
        DataTable DtabLedgerMapping = new DataTable();
        string StrMesg = "";

        #region Property Settings

        public FrmLedgerMappingMaster()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
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
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMast);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);

        }

        #endregion

        public void Clear()
        {
            Fill();
            DtabLedgerMapping.Rows.Add(DtabLedgerMapping.NewRow());

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
                string ReturnMessageDesc = "";
                string ReturnMessageType = "";

                foreach (DataRow Dr in DtabLedgerMapping.Rows)
                {
                    LedgerMappingMasterProperty Property = new LedgerMappingMasterProperty();

                    if (Val.ToString(Dr["LEDGERMAPPING_ID"]).Trim().Equals(string.Empty))
                        continue;

                    Property.LedgerMapping_id = Val.ToInt32(Dr["LEDGERMAPPING_ID"]);
                    Property.LedgerMappingType = Val.ToString(Dr["LEDGERMAPPINGTYPE"]);

                    if (Val.ToString(Dr["PLEDGER_ID"]) == "" || Val.ToString(Dr["PLEDGER_ID"]).Trim().Equals(string.Empty))
                        Property.PLedger_id = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    else
                        Property.PLedger_id = Guid.Parse(Val.ToString(Dr["PLEDGER_ID"]));

                    if (Val.ToString(Dr["SLEDGER_ID"]) == "" || Val.ToString(Dr["SLEDGER_ID"]).Trim().Equals(string.Empty))
                        Property.SLedger_id = Guid.Parse("00000000-0000-0000-0000-000000000000");
                    else
                        Property.SLedger_id = Guid.Parse(Val.ToString(Dr["SLEDGER_ID"]));

                    Property.GSTPER = Val.ToDouble(Dr["GSTPER"]);

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
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        public void Fill()
        {
            DtabLedgerMapping = ObjMast.Fill();
           
            MainGrid.DataSource = DtabLedgerMapping;
            MainGrid.Refresh();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("Ledger Mapping List", GrdDet);
        }

        private void RepsTxtPLedgerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "LEDGERCODE,LEDGERNAME,LEDGERTYPE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;

                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LEDGER);
                    FrmSearch.mStrColumnsToHide = "LEDGER_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    GrdDet.SetRowCellValue(GrdDet.FocusedRowHandle, "PLEDGER_ID", "00000000-0000-0000-0000-000000000000");
                    GrdDet.SetRowCellValue(GrdDet.FocusedRowHandle, "PLEDGERNAME", "");
                    if (FrmSearch.DRow != null)
                    {
                        GrdDet.SetRowCellValue(GrdDet.FocusedRowHandle, "PLEDGER_ID", Val.ToString(FrmSearch.DRow["LEDGER_ID"]));
                        GrdDet.SetRowCellValue(GrdDet.FocusedRowHandle, "PLEDGERNAME", Val.ToString(FrmSearch.DRow["LEDGERNAME"]));
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

        private void RepsTxtSLedgerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "LEDGERCODE,LEDGERNAME,LEDGERTYPE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;

                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LEDGER);
                    FrmSearch.mStrColumnsToHide = "LEDGER_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    GrdDet.SetRowCellValue(GrdDet.FocusedRowHandle, "SLEDGER_ID", "00000000-0000-0000-0000-000000000000");
                    GrdDet.SetRowCellValue(GrdDet.FocusedRowHandle, "SLEDGERNAME", "");
                    if (FrmSearch.DRow != null)
                    {
                        GrdDet.SetRowCellValue(GrdDet.FocusedRowHandle, "SLEDGER_ID", Val.ToString(FrmSearch.DRow["LEDGER_ID"]));
                        GrdDet.SetRowCellValue(GrdDet.FocusedRowHandle, "SLEDGERNAME", Val.ToString(FrmSearch.DRow["LEDGERNAME"]));
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

     
    }
}
