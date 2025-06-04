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


namespace MahantExport
{
    public partial class FrmStoneComment : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();   
        BOTRN_StockComment ObjMast = new BOTRN_StockComment();

        public int mIntCount = 0;

        public FrmStoneComment()
        {
            InitializeComponent();
        }

        public void ShowForm(string StrStockNo,string pStrStockID)
        {
            GrpStock.Tag = pStrStockID;
            GrpStock.Text = "Enter Your Comment of : " + StrStockNo;
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
                this.Cursor = Cursors.WaitCursor;
                int IntRes = ObjMast.Save(Val.ToString(GrpStock.Tag), txtComment.Text);
                if (IntRes != -1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("COMMENT SAVED SUCCESSFULLY");
                    Fill();
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

                if (Val.ToString(DR["EMPLOYEE_ID"]).ToUpper() != BusLib.Configuration.BOConfiguration.gEmployeeProperty.LEDGER_ID.ToString().ToUpper())
                {
                    Global.MessageError("YOU ARE NOT SUPPOSE TO DELETE OTHER SELLER'S COMMENT");
                    return;
                }
                if (Global.Confirm ("Are You Sure To Delete This Comment ?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                int IntRes = ObjMast.Delete(Val.ToString(DR["COMMENT_ID"]));
                if (IntRes != -1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("COMMENT DELETED SUCCESSFULLY");
                    GrdDet.DeleteRow(GrdDet.FocusedRowHandle);
                    GrdDet.RefreshData();
                    mIntCount = GrdDet.RowCount;
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
                DataTable DTab = ObjMast.GetData(Val.ToString(GrpStock.Tag));
                MainGrid.DataSource = DTab;
                MainGrid.Refresh();
                mIntCount = DTab.Rows.Count;
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