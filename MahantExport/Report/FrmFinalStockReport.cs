using BusLib.Configuration;
using BusLib.Report;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MahantExport.Report
{
    public partial class FrmFinalStockReport : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockTallyReport ObjStock = new BOTRN_StockTallyReport();
        BOFormPer ObjPer = new BOFormPer();

        public FrmFinalStockReport()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            if (ObjPer.ISVIEW == false)
            {
                Global.MessageError(BusLib.TPV.BOMessage.ViewDeniedMsg);
                return;
            }

            this.Show();
        }
        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjStock);
            ObjFormEvent.ObjToDisposeList.Add(Val);

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                string StrOpe = "";
                
                StrOpe = "Summary";
                DataSet DSetStock = ObjStock.GetDataForFinalStockReport(StrOpe);

                MainGrdStock.DataSource = DSetStock.Tables[0];
                GrdStock.BestFitColumns();

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdStock_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
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

                    string StrOpe = "";                  
                    string StrType = "";


                    StrType = Val.ToString(GrdStock.GetFocusedRowCellValue("EntryType").ToString());
                    
                    DataSet DSetStockDetail = ObjStock.GetDataForFinalStockReport(StrType);

                    if (DSetStockDetail.Tables[0] == null)
                    {
                        Global.Message("No Data Found");
                        return;
                    }
                    MainGrdStoneDetail.DataSource = DSetStockDetail.Tables[0];
                    GrdStoneDetail.BestFitColumns();

                    this.Cursor = Cursors.Default;

                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Global.ExcelExport("Final Stock Report", GrdStock);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnDetailExport_Click(object sender, EventArgs e)
        {
            try
            {
                Global.ExcelExport("Final Stock Report Detail", GrdStoneDetail);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }
    }
}