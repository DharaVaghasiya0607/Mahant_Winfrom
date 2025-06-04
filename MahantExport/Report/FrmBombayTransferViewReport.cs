//************************************************************************************************************"
//   Developer        Date            
//  Daksha Vasoya    25/08/2023
//************************************************************************************************************
using BusLib.Transaction;
using DevExpress.Data;
using System;
using System.Data;
using System.Windows.Forms;
using MahantExport.Parcel;

namespace MahantExport.Report
{
    public partial class FrmBombayTransferViewReport : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        
        BOTRN_Kapan ObjKapan = new BOTRN_Kapan();

        double TCts = 0, TAmt = 0;

        public FrmBombayTransferViewReport()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            this.Show();
            AttachFormDefaultEvent();
        }

        private void AttachFormDefaultEvent()
        {
            Val.FormGeneralSetting(this);
            ObjFormEvent.mForm = this;
            //	ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjKapan);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                string StrFromDate = "", StrToDate = "";
                if (dtpFromDate.Checked)
                {
                    StrFromDate = Val.SqlDate(dtpFromDate.Text);
                }
                if (dtpToDate.Checked)                
                {
                    StrToDate = Val.SqlDate(dtpToDate.Text);
                }

                DataTable dtData = ObjKapan.BombayTransferView_GetData(StrFromDate, StrToDate, txtKapan.Text);
                GCData.DataSource = dtData;
                dgvData.BestFitColumns();

                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            dtpFromDate.Value = DateTime.Now;
            dtpToDate.Value = DateTime.Now;
            txtKapan.Text = string.Empty;
            txtKapan.Tag = string.Empty;            
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtKapan_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                //if (Global.OnKeyPressEveToPopup(e))
                //{
                //    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                //    FrmSearch.mStrSearchField = "KAPANNAME,KAPANCODE";
                //    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                //    this.Cursor = Cursors.WaitCursor;

                //    DataTable DTabKapan = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.TRN_KAPAN);
                //    FrmSearch.mDTab = DTabKapan;
                //    FrmSearch.mStrColumnsToHide = "KAPAN_ID";
                //    this.Cursor = Cursors.Default;
                //    FrmSearch.ShowDialog();
                //    e.Handled = true;
                //    if (FrmSearch.DRow != null)
                //    {
                //        txtKapan.Text = Val.ToString(FrmSearch.DRow["KAPANNAME"]).ToUpper();
                //        txtKapan.Tag = Val.ToString(FrmSearch.DRow["KAPAN_ID"]);
                //    }

                //    FrmSearch.Hide();
                //    FrmSearch.Dispose();
                //    FrmSearch = null;
                //}
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }           
        }

        private void dgvData_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }
                if (e.Clicks == 2)
                {
                    this.Cursor = Cursors.WaitCursor;
                    string strKapan = Val.ToString(dgvData.GetRowCellValue(e.RowHandle, "KapanName"));
                    FrmClarityAssortmentView FrmClarityAssortmentView = new FrmClarityAssortmentView();
                    FrmClarityAssortmentView.MdiParent = Global.gMainRef;
                    FrmClarityAssortmentView.ShowForm(strKapan, true);
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void dgvData_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            try
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    TCts = 0;
                    TAmt = 0;

                }
                else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    TCts = TCts + Val.Val(dgvData.GetRowCellValue(e.RowHandle, "Carat"));
                    TAmt = TAmt + Val.Val(dgvData.GetRowCellValue(e.RowHandle, "Amount"));
                }
                else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("PricePerCarat") == 0)
                    {
                        if (TCts > 0)
                            e.TotalValue = Math.Round(TAmt / TCts, 2);
                        else
                            e.TotalValue = 0;
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
