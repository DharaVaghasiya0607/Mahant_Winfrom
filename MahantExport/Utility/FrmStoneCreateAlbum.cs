using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Globalization;
using System.Collections;
using BusLib.Master;
using BusLib.Configuration;
using BusLib.Utility;
using System.IO;


namespace MahantExport
{
    public partial class FrmStoneCreateAlbum : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();   
        BOTRN_StockComment ObjMast = new BOTRN_StockComment();

        DataTable DTab = new DataTable();
        
        public FrmStoneCreateAlbum()
        {
            InitializeComponent();
        }

        public void ShowForm(DataTable pDTab)
        {
            DTab = pDTab;
            AttachFormDefaultEvent();
            Val.FormGeneralSettingForPopup(this);         
            this.ShowDialog();             
        }

        private void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.FormKeyPress = false;

            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
        }
      
        private void FrmSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.No;
                this.Close();
            }            
        }


        private void BtnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Close();
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtTitle.Text.Trim().Length == 0)
                {
                    Global.Message("Title Is Required");
                    txtTitle.Focus();
                    return;
                }
                if (txtDescription.Text.Trim().Length == 0)
                {
                    Global.Message("Description Is Required");
                    txtDescription.Focus();
                    return;
                }
                this.Cursor = Cursors.WaitCursor;

                string MemoEntryDetailForXML = string.Empty;
                DTab.TableName = "Table1";
                using (StringWriter sw = new StringWriter())
                {
                    DTab.WriteXml(sw);
                    MemoEntryDetailForXML = sw.ToString();
                }
                string StrRes = ObjMast.SaveAlbum(null, null, txtTitle.Text, txtDescription.Text, MemoEntryDetailForXML);
                if (StrRes != "")
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("ABLUM CREATED SUCCESSFULLY Your ID Is : " + StrRes);
                    txtDescription.Text = string.Empty;
                    txtTitle.Text = string.Empty;
                    Fill();
                }
                else
                {
                    Global.Message("Opps...there is some error while save");
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.MessageError(ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle < 0)
                {
                    return;
                }

                DataRow DR = GrdDet.GetFocusedDataRow();

                if (Val.ToString(DR["ENTRYBY"]).ToUpper() != BusLib.Configuration.BOConfiguration.gEmployeeProperty.LEDGER_ID.ToString().ToUpper())
                {
                    Global.MessageError("YOU ARE NOT SUPPOSE TO DELETE OTHER SELLER'S ALBUM");
                    return;
                }
                if (Global.Confirm ("Are You Sure To Delete This Album ?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                int IntRes = ObjMast.DeleteAlbum(Val.ToString(DR["ALBUM_ID"]));
                if (IntRes != -1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("ALBUM DELETED SUCCESSFULLY");
                    GrdDet.DeleteRow(GrdDet.FocusedRowHandle);
                    GrdDet.RefreshData();
                  
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.MessageError(ex.Message);
            }
            
        }

        public void Fill()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DTab = ObjMast.GetDataAlbum(Val.SqlDate(DateTime.Now.AddMonths(-6).ToShortDateString()), Val.SqlDate(DateTime.Now.ToShortDateString()));
                MainGrid.DataSource = DTab;
                MainGrid.Refresh();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.MessageError(ex.Message);
            }
        }

        private void FrmStoneComment_Load(object sender, EventArgs e)
        {
            Fill();
        }

        private void FrmStoneComment_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

    }
}