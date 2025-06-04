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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.Utils.Drawing;
using DevExpress.Data;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
using DevExpress.XtraGrid.Columns;
using MahantExport.Utility;
using DevExpress.XtraGrid.Views.BandedGrid;
using OfficeOpenXml;
using DevExpress.XtraPrinting;
using DevExpress.Utils;
using DevExpress.XtraPivotGrid;
using System.Collections;
using Microsoft.VisualBasic.CompilerServices;

namespace MahantExport.Report
{
    public partial class FrmSaleAnalysisReport : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_KapanInward ObjKapan = new BOTRN_KapanInward();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        DataTable DTabSale = new DataTable();
        double DouTAmt = 0;
        double DouTCarat = 0;
        double DouAmount = 0;
        double DouCarat= 0;
        bool IsBombayTransfer = false;

        #region Property

        public FrmSaleAnalysisReport()
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
            ObjFormEvent.ObjToDisposeList.Add(ObjKapan);
            ObjFormEvent.ObjToDisposeList.Add(Val);
           // ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }

        public void ShowForm(string _KapanName = "", bool _IsBombayTransfer = false)
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();
        }

        #endregion


    
        private void BtnBack_Click(object sender, EventArgs e)
        {
             this.Close();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                
                this.Cursor = Cursors.WaitCursor;

                string pStrFromDate = "";
                string PStrToDate = "";

                if (DtpFromDate.Checked == true)
                {
                    pStrFromDate = Val.SqlDate(DtpFromDate.Value.ToShortDateString());
                }
                else
                {
                    pStrFromDate = "";
                }
                
                if (DtpToDate.Checked == true)
                {
                    PStrToDate = Val.SqlDate(DtpToDate.Value.ToShortDateString());
                }
                else
                {
                    PStrToDate = "";
                }

                DataTable DTabSale = ObjKapan.GetDataForDSaleAnalysis(pStrFromDate, PStrToDate);
                pivotGrid.DataSource = DTabSale;
                pivotGrid.BestFit();

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Global.ExcelExport("Kapan Analysis", pivotGrid);

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
      
        
        private void pivotGrid_EditValueChanged(object sender, EditValueChangedEventArgs e)
        {
            //try
            //{              
            //    double Carat = 0;
            //    if (e.DataField == colRate)
            //    {
            //        PivotDrillDownDataSource ds = e.CreateDrillDownDataSource();
            //        for (int i = 0; i < ds.RowCount; i++)
            //        {
            //            ds.SetValue(i, e.DataField, Val.ToDouble(e.Editor.EditValue));
            //            Carat = Val.ToDouble(ds.GetValue(i, "CARAT"));
            //            ds.SetValue(i, "AMOUNT", Val.Format(Val.ToDouble(e.Editor.EditValue) * Carat, "n2"));
            //        }
            //    }
            //}
             
            //catch (Exception ex)
            //{
            //    Global.Message(ex.Message.ToString());
            //}
        }

        private void pivotGrid_CustomCellValue(object sender, PivotCellValueEventArgs e)
        {
            //try
            //{
            //    double TCarat = 0, TAmount = 0;                
            //    if (object.ReferenceEquals(e.DataField, colRate))
            //    {
            //        TCarat = Val.ToDouble(e.GetCellValue(colCarat));
            //        TAmount = Val.ToDouble(e.GetCellValue(colAmount));
            //        e.Value = TAmount == 0 ? 0 : Math.Round(TAmount / TCarat, 2);                    
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Global.Message(ex.Message);
            //}
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                DtpFromDate.Checked = false;
                DtpToDate.Checked = false;

                pivotGrid.DataSource = null;
                BtnSearch.Enabled = true;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void pivotGrid_Click(object sender, EventArgs e)
        {

        }
        //End as Daksha
    }
}
