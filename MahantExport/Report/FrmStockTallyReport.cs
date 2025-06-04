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
    public partial class FrmStockTallyReport : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockTallyReport ObjStock = new BOTRN_StockTallyReport();
        BOFormPer ObjPer = new BOFormPer();
        string StrJangedNo = "";

        public FrmStockTallyReport()
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
            //ObjFormEvent.FormKeyDown = true;
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

                string StrFromDate = null;
                string StrToDate = null;
                string StrOpe = "";
                string StrMemo_ID = "";
                string StrType = "";


                if (dtFromDate.Checked == true)
                {
                    StrFromDate = Val.SqlDate(dtFromDate.Value.ToShortDateString());
                }
                if (dtToDate.Checked == true)
                {
                    StrToDate = Val.SqlDate(dtToDate.Value.ToShortDateString());
                }

                if(RbtNatural.Checked ==  true)
                {
                    StrType = Val.ToString(RbtNatural.Text);
                }
                else
                {
                    StrType = Val.ToString(RbtLabgrown.Text);
                }

                StrOpe = "Summary";
                DataSet DSetStock= ObjStock.StockTallyReportGetData(StrFromDate, StrToDate, StrOpe,StrMemo_ID,"",0, StrType);

                MainGrdUngraded.DataSource = DSetStock.Tables[0];
                GrdUngraded.BestFitColumns();

                MainGrdStock.DataSource = DSetStock.Tables[1];
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

                    string StrColumn = e.Column.FieldName;

                    string StrFromDate = null;
                    string StrToDate = null;
                    string StrOpe = "";
                    string StrMemo_ID = "";
                    string StrEntryDate = null;
                    Int32 StrLab = 0;
                    string StrType = "";


                    if (dtFromDate.Checked == true)
                    {
                        StrFromDate = Val.SqlDate(dtFromDate.Value.ToShortDateString());
                    }
                    if (dtToDate.Checked == true)
                    {
                        StrToDate = Val.SqlDate(dtToDate.Value.ToShortDateString());
                    }

                    if (RbtNatural.Checked == true)
                    {
                        StrType = Val.ToString(RbtNatural.Text);
                    }
                    else
                    {
                        StrType = Val.ToString(RbtLabgrown.Text);
                    }

                    if (StrColumn.Contains("Inward"))
                    {
                        StrOpe = "Inward";
                    }
                    if (StrColumn.Contains("Ungraded"))
                    {
                        StrOpe = "Ungraded";
                    }
                    if (StrColumn.Contains("LabIssue"))
                    {
                        StrOpe = "Lab Issue";
                    }
                    if (StrColumn.Contains("LabResult"))
                    {
                        StrOpe = "Lab Result";
                    }
                    if (StrColumn.Contains("LabRecheck"))
                    {
                        StrOpe = "Lab Recheck";
                    }
                    if (StrColumn.Contains("LabReturn"))
                    {
                        StrOpe = "Lab Return";
                    }
                    if (StrColumn.Contains("AvailablePending"))
                    {
                        StrOpe = "Available Pending";
                    }
                    if (StrColumn.Contains("PhysicalStock"))
                    {
                        StrOpe = "Available";
                    }
                    if (StrColumn.Contains("SalesDelivery"))
                    {
                        StrOpe = "Sales Delivery";
                    }
                    if (StrColumn.Contains("Mix"))
                    {
                        StrOpe = "Mix";
                    }
                    if (StrColumn.Contains("Delete"))
                    {
                        StrOpe = "Delete";
                    }
                    if (StrColumn.Contains("Memo"))
                    {
                        StrOpe = "Memo";
                    }

                    StrMemo_ID = Val.ToString(GrdStock.GetFocusedRowCellValue("MemoID").ToString());
                    StrLab = Val.ToInt32(GrdStock.GetFocusedRowCellValue("LAB_ID").ToString());


                    DataSet DSetStockDetail = ObjStock.StockTallyReportGetData(StrFromDate, StrToDate, StrOpe, StrMemo_ID, StrEntryDate, StrLab, StrType);
                    
                    if(DSetStockDetail.Tables[0] == null)
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

        private void LblStockWiseExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("Stock List", GrdStock);
        }

        private void lblstoneDetailWiseExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("Stone Detail List", GrdStoneDetail);
        }

        private void MainGrdStock_Click(object sender, EventArgs e)
        {

        }

        private void GrdUngraded_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
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

                    string StrColumn = e.Column.FieldName;

                    string StrFromDate = null;
                    string StrToDate = null;
                    string StrOpe = "";
                    string StrMemo_ID = "";
                    string StrEntryDate = "";
                    string StrType = "";

                    if (dtFromDate.Checked == true)
                    {
                        StrFromDate = Val.SqlDate(dtFromDate.Value.ToShortDateString());
                    }
                    if (dtToDate.Checked == true)
                    {
                        StrToDate = Val.SqlDate(dtToDate.Value.ToShortDateString());
                    }

                    if (RbtNatural.Checked == true)
                    {
                        StrType = Val.ToString(RbtNatural.Text);
                    }
                    else
                    {
                        StrType = Val.ToString(RbtLabgrown.Text);
                    }

                    if (StrColumn.Contains("Inward"))
                    {
                        StrOpe = "Inward";
                    }
                    if (StrColumn.Contains("Ungraded"))
                    {
                        StrOpe = "Ungraded";
                    }
                    if (StrColumn.Contains("LabIssue"))
                    {
                        StrOpe = "Lab Issue";
                    }
                    if (StrColumn.Contains("Mix"))
                    {
                        StrOpe = "UN-Mix";
                    }
                    if (StrColumn.Contains("Total"))
                    {
                        StrOpe = "Inward";
                    }

                    if (StrColumn.Contains("Delete"))
                    {
                        StrOpe = "Delete";
                    }

                    StrEntryDate = Val.SqlDate(GrdUngraded.GetFocusedRowCellValue("EntryDate").ToString());
                    DataSet DSetStockDetail = ObjStock.StockTallyReportGetData(StrFromDate, StrToDate, StrOpe, StrMemo_ID, StrEntryDate,0, StrType);

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
    }
}