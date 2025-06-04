using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using DevExpress.XtraPrinting;
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
using MahantExport.Utility;
using System.Drawing.Text;

namespace MahantExport.Masters
{
    public partial class FrmReportMasterNew : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        
        BOMST_Report ObjTrn = new BOMST_Report();
        DataTable DTabLotDetail = new DataTable();
        DataTable DTabSummary = new DataTable();
        DataTable DTabGroupByTitle = new DataTable();


        #region Property Settings

        public FrmReportMasterNew()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            // Add By Dhara : 04-04-2022
            //if (ObjPermission.ISView == false)
            //{
            //    Global.MessageError(BOMessage.FormOpenDeniedMsg);
            //    return;
            //}

            DataSet DS = ObjTrn.GetData(-1);

            DTabLotDetail = DS.Tables[0].Copy();
            DTabGroupByTitle = DS.Tables[1].Copy();

            MainGrid.DataSource = DTabLotDetail;
            MainGrid.Refresh();

            MainGridGroup.DataSource = DTabGroupByTitle;
            MainGridGroup.Refresh();

            CmbPrintFont.Items.Clear();
            CmbDisplayFont.Items.Clear();
            using (InstalledFontCollection col = new InstalledFontCollection())
            {
                foreach (FontFamily fa in col.Families)
                {
                    CmbPrintFont.Items.Add(fa.Name);
                    CmbDisplayFont.Items.Add(fa.Name);
                }
            } 

            BtnNew_Click(null, null);

            this.Show();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyPress = false;
            ObjFormEvent.FormKeyDown = false;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjTrn);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(DTabLotDetail);

            //ObjPermission.GetPermission(this);
            //BtnSave.Visible = ObjPermission.ISAdd | ObjPermission.ISEdit;
            //BtnExport.Visible = ObjPermission.ISExport;
            //BtnPdf.Visible = ObjPermission.ISGridPdf;
            //BtnFind.Visible = ObjPermission.ISView;
        }

        #endregion

        #region Validation

        private bool ValSave()
        {
            if (txtReportName.Text.Trim().Length == 0)
            {
                Global.MessageError("Report Name Is Required");
                txtReportName.Focus();
                return false;
            }
            if (txtSPName.Text.Trim().Length == 0)
            {
                Global.MessageError("SP Name Is Required");
                txtSPName.Focus();
                return false;
            }

            if (Val.ToString(CmbReportGroupNew.SelectedItem) == "")
            {
                Global.MessageError("Report Group Name Is Required");
                CmbReportGroupNew.Focus();
                return false;
            }
            /*
            if (Val.ToString(CmbReportView.SelectedItem) == "PivotView")
            {
                int IntRes = 0;
                foreach (DataRow DRow in DTabLotDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISVISIBLE"]) == true
                        && Val.ToBoolean(DRow["ROWAREA"]) == false
                        && Val.ToBoolean(DRow["DATAAREA"]) == false
                        &&  Val.ToBoolean(DRow["COLUMNAREA"]) == false
                        )
                    {
                        Global.MessageError("Row : " + (IntRes + 1).ToString() + " Please Select Field Area While Pivot View");
                        GrdDet.FocusedRowHandle = IntRes + 1;
                        IntRes++;
                        return false;
                    }
                }
            }
            */
            return true;
        }

        #endregion

        #region Form Events

        private void FrmLRReceive_KeyDown(object sender, KeyEventArgs e)
        {
           
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
               
            }
        }

        #endregion

        #region Control Events

        private void BtnFind_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            Clear();
           
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (ValSave() == false)
                {
                    return;
                }

                
                if (Val.ToInt(txtReportID.Text) == 0)
                {
                    txtReportID.Text = Val.ToString(ObjTrn.FindNewID());
                }

                if (lblMode.Text == "Add Mode")
                {
                    while (ObjTrn.CheckRecordExistsOrNot(Val.ToInt(txtReportID.Text)) == true)
                    {
                        if (Global.Confirm("[" + txtReportID.Text + "] No Is Already Generated\n\nDo You Want To Generate New ?") == System.Windows.Forms.DialogResult.No)
                        {
                            return;
                        }
                        txtReportID.Text = Val.ToString(ObjTrn.FindNewID());
                    }
                }

                DTabLotDetail.AcceptChanges();
                int IntSrNo = 0;
                foreach (DataRow DRow in DTabLotDetail.Rows)
                {
                    IntSrNo++;
                    DRow["SRNO"] = IntSrNo;
                }

                DTabLotDetail.AcceptChanges();
                
                DTabGroupByTitle.AcceptChanges();

                IntSrNo = 0;
                foreach (DataRow DRow in DTabGroupByTitle.Rows)
                {
                    IntSrNo++;
                    DRow["SRNO"] = IntSrNo;
                }

                DTabGroupByTitle.AcceptChanges();

                this.Cursor = Cursors.WaitCursor;

                MST_ReportProperty Property = new MST_ReportProperty();

                Property.REPORT_ID = Val.ToInt(txtReportID.Text);
	            Property.REPORTGROUP = Val.ToString(CmbReportGroup.SelectedItem);
                Property.REPORTGROUPNEW = Val.ToString(CmbReportGroupNew.SelectedItem);
	            Property.REPORTNAME = txtReportName.Text;
	            Property.REPORTTYPE = Val.ToString(CmbReportType.SelectedItem);
	            Property.FORMNAME = txtFilterFormName.Text;
	            Property.SEQUENCENO = Val.ToInt(txtSeqNo.Text);
	            Property.SPNAME = txtSPName.Text;
	            Property.REPORTVIEW = Val.ToString(CmbReportView.SelectedItem);
	            Property.DISPLAYFONTNAME = Val.ToString(CmbDisplayFont.SelectedItem);
	            Property.DISPLAYFONTSIZE= Val.Val(txtDisplaySize.Text);
	            Property.PRINTFONTNAME = Val.ToString(CmbPrintFont.SelectedItem);
	            Property.PRINTFONTSIZE= Val.Val(txtPrintSize.Text);
	            Property.PRINTORIENTATION = Val.ToString(CmbOrientation.SelectedItem);
	            Property.ISACTIVE = ChkActive.Checked;
                Property.REMARK = txtRemark.Text;

                Property.ISPRINTFIRMNAME = ChkIsPrintFirmName.Checked;
                Property.ISPRINTFIRMADDRESS = ChkIsPrintFirmAddress.Checked;
                Property.ISPRINTFILTERCRITERIA = ChkIsPrintFilterCriteria.Checked;
                Property.ISPRINTHEADINGONEACHPAGE = ChkIsPrintHeadingOnEachPage.Checked;
                Property.ISPRINTDATETIME = ChkIsPrintDateTime.Checked;
                
                DTabLotDetail.TableName = "Detail";
                
                string xmlData = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabLotDetail.WriteXml(sw);
                    xmlData = sw.ToString();
                }

                Property.XMLDATA = xmlData;

                DTabGroupByTitle.TableName = "Detail";

                string xmlDataGroup = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabGroupByTitle.WriteXml(sw);
                    xmlDataGroup = sw.ToString();
                }

                Property.XMLDATAGROUP = xmlDataGroup;

                Property = ObjTrn.Save(Property);
                this.Cursor = Cursors.Default;

                if (Property.RETURNMESSAGETYPE == "SUCCESS")
                {
                    Global.Message(Property.RETURNMESSAGEDESC);
                    Clear();
                    BtnFind_Click(null, null);
                }
                else if (Property.RETURNMESSAGETYPE == "FAIL")
                {
                    Global.MessageError(Property.RETURNMESSAGEDESC);
                    txtReportName.Focus();
                }
                Property = null;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.WaitCursor;
                Global.MessageError(ex.Message);
            //    BusLib.BOException.Save(this.GetType().Name, new System.Diagnostics.StackFrame(1, true).GetMethod().ToString(), new System.Diagnostics.StackFrame(0, true).GetFileLineNumber(), ex.Message, ex.ToString());
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            
            Global.ExcelExport("LotInwardList", GrdDetSummary);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            
        }

        private void txtRepMeter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                GrdDet.PostEditor();
                if (Val.ToString(GrdDet.GetFocusedRowCellValue("FIELDNAME")) != "" && GrdDet.IsLastRow == true)
                {
                    DataRow DRow = DTabLotDetail.NewRow();
                    DRow["SRNO"] = DTabLotDetail.Rows.Count + 1;
                    DRow["ISVISIBLE"] = true;
                    DRow["ISMERGE"] = false;
                    DRow["ISGROUP"] = false;
                    DRow["ALIGNMENT"] = "Left";
                    DRow["DATATYPE"] = "String";
                    DTabLotDetail.Rows.Add(DRow);

                    GrdDet.FocusedRowHandle = GrdDet.FocusedRowHandle + 1;
                    GrdDet.FocusedColumn = GrdDet.Columns["FIELDNAME"];
                }
                else if (Val.ToString(GrdDet.GetFocusedRowCellValue("FIELDNAME")) != "" && GrdDet.IsLastRow == false)
                {
                    GrdDet.FocusedRowHandle = GrdDet.FocusedRowHandle + 1;
                    GrdDet.FocusedColumn = GrdDet.Columns["FIELDNAME"];
                }
                else
                {
                    BtnSave.Focus();
                    BtnSave.PerformClick();
                }
            }
          
        }

        private void txtRemark_Validated(object sender, EventArgs e)
        {
            DataRow DRow = DTabLotDetail.NewRow();
            DRow["SRNO"] = DTabLotDetail.Rows.Count + 1;
            DRow["ISVISIBLE"] = true;
            DRow["ISMERGE"] = false;
            DRow["ISGROUP"] = false;
            DRow["ALIGNMENT"] = "Left";
            DRow["DATATYPE"] = "String";
            DTabLotDetail.Rows.Add(DRow);

            GrdDet.FocusedRowHandle = 0;
            GrdDet.FocusedColumn = GrdDet.Columns["FIELDNAME"];

            MainGrid.DataSource = DTabLotDetail;

        }

        private void BtnUp_Click(object sender, EventArgs e)
        {
            try
            {
                int idx = GrdDet.FocusedRowHandle;
                if (idx == 0)
                    return;
                DataRow row = DTabLotDetail.Rows[idx];
                DataRow oldRow = row;
                DataRow newRow = DTabLotDetail.NewRow();
                newRow.ItemArray = oldRow.ItemArray;
                int oldRowIndex = DTabLotDetail.Rows.IndexOf(row);
                int newRowIndex = oldRowIndex - 1;
                if (oldRowIndex > 0)
                {
                    DTabLotDetail.Rows.Remove(oldRow);
                    DTabLotDetail.Rows.InsertAt(newRow, newRowIndex);
                    DTabLotDetail.Rows.IndexOf(newRow);
                }
                DTabLotDetail.AcceptChanges();
                GrdDet.FocusedRowHandle = newRowIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnDown_Click(object sender, EventArgs e)
        {
            try
            {
                int totalRows = DTabLotDetail.Rows.Count;
                int idx = GrdDet.FocusedRowHandle;
                if (idx == totalRows - 1)
                    return;
                DataRow row = DTabLotDetail.Rows[idx];
                DataRow oldRow = row;
                DataRow newRow = DTabLotDetail.NewRow();
                newRow.ItemArray = oldRow.ItemArray;
                int oldRowIndex = DTabLotDetail.Rows.IndexOf(row);
                int newRowIndex = oldRowIndex + 1;
                if (oldRowIndex < (DTabLotDetail.Rows.Count))
                {
                    DTabLotDetail.Rows.Remove(oldRow);
                    DTabLotDetail.Rows.InsertAt(newRow, newRowIndex);
                    DTabLotDetail.Rows.IndexOf(newRow);
                }
                DTabLotDetail.AcceptChanges();
                GrdDet.FocusedRowHandle = newRowIndex;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnSequence_Click(object sender, EventArgs e)
        {
            DTabLotDetail.AcceptChanges();
            int IntSrNo = 0;
            foreach (DataRow DRow in DTabLotDetail.Rows)
            {
                if (Val.ToString(DRow["FIELDNAME"]) == "")
                {
                    continue;
                }
                DRow["SRNO"] = Val.ToString(IntSrNo + 1);
                IntSrNo++;
            }
            DTabLotDetail.AcceptChanges();
            MainGrid.DataSource = DTabLotDetail;
            MainGrid.RefreshDataSource();
            GrdDet.BestFitColumns();
        }



        #endregion

        #region Grid Events

        private void GrdDet_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }
                if (e.Clicks == 2 )
                {
                    DataRow dr = GrdDetSummary.GetDataRow(e.RowHandle);
                    FetchValue(dr);
                    dr = null;
                }
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
               // BusLib.BOException.Save(this.GetType().Name, new System.Diagnostics.StackFrame(1, true).GetMethod().ToString(), new System.Diagnostics.StackFrame(0, true).GetFileLineNumber(), ex.Message, ex.ToString());
            }
        }

        private void GrdDet_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DataRow dr = GrdDet.GetFocusedDataRow();
                    FetchValue(dr);
                    dr = null;
                }
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
               // BusLib.BOException.Save(this.GetType().Name, new System.Diagnostics.StackFrame(1, true).GetMethod().ToString(), new System.Diagnostics.StackFrame(0, true).GetFileLineNumber(), ex.Message, ex.ToString());
            }
        }

        private void GrdDet_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GrdDet.BestFitColumns();
        }


        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (GrdDet.FocusedRowHandle < 0)
                {
                    return;
                }

                DataRow dr = GrdDet.GetFocusedDataRow();

                Int32 IntID = Val.ToInt32(dr["SRNO"]);

                if (IntID == 0)
                {
                    GrdDet.DeleteRow(GrdDet.FocusedRowHandle);
                    DTabLotDetail.AcceptChanges();
                    return;
                }
                else if (IntID != 0)
                {
                    this.Cursor = Cursors.WaitCursor;
                    MST_ReportProperty Property = new MST_ReportProperty();
                    
                    Property.SRNO = IntID;
                    Property.REPORT_ID = Val.ToInt(txtReportID.Text);
              
                    Property = ObjTrn.Delete(Property);

                    this.Cursor = Cursors.Default;

                    if (Property.RETURNMESSAGETYPE == "SUCCESS")
                    {
                        Global.Message(Property.RETURNMESSAGEDESC);
                        GrdDet.DeleteRow(GrdDet.FocusedRowHandle);
                        DTabLotDetail.AcceptChanges();
                    }
                    else
                    {
                        Global.MessageError(Property.RETURNMESSAGEDESC);
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.MessageError(ex.Message);
              //  BusLib.BOException.Save(this.GetType().Name, new System.Diagnostics.StackFrame(1, true).GetMethod().ToString(), new System.Diagnostics.StackFrame(0, true).GetFileLineNumber(), ex.Message, ex.ToString());
            }
        }

        #endregion

        #region Other Operation

        public void GetData()
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                GrdDetSummary.BeginUpdate();
                DTabLotDetail.Rows.Clear();
                DTabSummary = ObjTrn.GetDataSummary("All");
                DTabSummary.DefaultView.Sort = "Report_ID";
                DTabSummary = DTabSummary.DefaultView.ToTable();
                MainGridSummary.DataSource = DTabSummary;
                MainGridSummary.Refresh();
                GrdDetSummary.BestFitColumns();
                GrdDetSummary.EndUpdate();

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
                //BusLib.BOException.Save(this.GetType().Name, new System.Diagnostics.StackFrame(1, true).GetMethod().ToString(), new System.Diagnostics.StackFrame(0, true).GetFileLineNumber(), ex.Message, ex.ToString());
            }
        }

        public void FetchValue(DataRow dr)
        {
            try
            {
                lblMode.Text = "Edit Mode";


                this.Cursor = Cursors.WaitCursor;
                
                txtReportID.Text = Val.ToString(dr["REPORT_ID"]);
                CmbReportGroup.SelectedItem = Val.ToString(dr["REPORTGROUP"]);
                CmbReportGroupNew.SelectedItem = Val.ToString(dr["REPORTGROUPNEW"]);
                txtReportName.Text = Val.ToString(dr["REPORTNAME"]);
                CmbReportType.SelectedItem = Val.ToString(dr["REPORTTYPE"]);
                txtFilterFormName.Text = Val.ToString(dr["FORMNAME"]);
                txtSeqNo.Text = Val.ToString(dr["SEQUENCENO"]);
                txtSPName.Text = Val.ToString(dr["SPNAME"]);
                CmbReportView.SelectedItem = Val.ToString(dr["REPORTVIEW"]);
                CmbDisplayFont.SelectedItem = Val.ToString(dr["DISPLAYFONTNAME"]);
                txtDisplaySize.Text = Val.ToString(dr["DISPLAYFONTSIZE"]);
                CmbPrintFont.SelectedItem = Val.ToString(dr["PRINTFONTNAME"]);
                txtPrintSize.Text = Val.ToString(dr["PRINTFONTSIZE"]);
                CmbOrientation.SelectedItem = Val.ToString(dr["PRINTORIENTATION"]);
                ChkActive.Checked = Val.ToBoolean(dr["ISACTIVE"]);

                ChkIsPrintFirmName.Checked = Val.ToBoolean(dr["ISPRINTFIRMNAME"]);
                ChkIsPrintFirmAddress.Checked = Val.ToBoolean(dr["ISPRINTFIRMADDRESS"]);
                ChkIsPrintFilterCriteria.Checked = Val.ToBoolean(dr["ISPRINTFILTERCRITERIA"]);
                ChkIsPrintHeadingOnEachPage.Checked = Val.ToBoolean(dr["ISPRINTHEADINGONEACHPAGE"]);
                ChkIsPrintDateTime.Checked = Val.ToBoolean(dr["ISPRINTDATETIME"]);

                txtRemark.Text = Val.ToString(dr["REMARK"]);
          
                DTabLotDetail.Rows.Clear();

                DataSet DS = ObjTrn.GetData(Val.ToInt32(txtReportID.Text));

                DTabLotDetail = DS.Tables[0].Copy();
                DTabGroupByTitle = DS.Tables[1].Copy();


                DTabLotDetail.Rows.Add(DTabLotDetail.NewRow());
                DTabGroupByTitle.Rows.Add(DTabGroupByTitle.NewRow());

                GrdDet.BeginUpdate();
                MainGrid.DataSource = DTabLotDetail;
                MainGrid.Refresh();
                GrdDet.BestFitColumns();
                GrdDet.EndUpdate();

                GrdDetGroup.BeginUpdate();
                MainGridGroup.DataSource = DTabGroupByTitle;
                MainGridGroup.Refresh();
                GrdDetGroup.BestFitColumns();
                GrdDetGroup.EndUpdate();

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
                //BusLib.BOException.Save(this.GetType().Name, new System.Diagnostics.StackFrame(1, true).GetMethod().ToString(), new System.Diagnostics.StackFrame(0, true).GetFileLineNumber(), ex.Message, ex.ToString());
            }
           
        }

        private void Clear()
        {
            try
            {
                lblMode.Text = "Add Mode";

                txtReportID.Text = string.Empty;
                txtReportName.Text = string.Empty;
                txtSPName.Text = string.Empty;
                txtFilterFormName.Text = string.Empty;
                txtDisplaySize.Text = "8";
                txtPrintSize.Text = "8";
                txtRemark.Text = string.Empty;
                CmbReportType.SelectedIndex = 0;
                CmbReportGroup.SelectedIndex = 0;
                CmbReportGroupNew.Text = string.Empty;
                CmbPrintFont.SelectedItem = "Verdana";
                CmbDisplayFont.SelectedItem = "Verdana";
                CmbOrientation.SelectedIndex = 0;
                CmbReportView.SelectedIndex = 0;

                ChkIsPrintFirmName.Checked = false;
                ChkIsPrintFirmAddress.Checked = false;
                ChkIsPrintFilterCriteria.Checked = false;
                ChkIsPrintHeadingOnEachPage.Checked = false;
                ChkIsPrintDateTime.Checked = false;
                DTabLotDetail.Rows.Clear();
                DTabGroupByTitle.Rows.Clear();

                txtReportID.Text = ObjTrn.FindNewID().ToString();

                DTabLotDetail.Rows.Clear();

                MainGrid.DataSource = null;

                txtReportName.Focus();

            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
                //BusLib.BOException.Save(this.GetType().Name, new System.Diagnostics.StackFrame(1, true).GetMethod().ToString(), new System.Diagnostics.StackFrame(0, true).GetFileLineNumber(), ex.Message, ex.ToString());
            }
        }


        #endregion

        private void txGroupBy_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    GrdDetGroup.PostEditor();
                    if (Val.ToString(GrdDetGroup.GetFocusedRowCellValue("GROUPTITLE")) != "" && GrdDetGroup.IsLastRow == true)
                    {
                        DataRow DRow = DTabGroupByTitle.NewRow();
                        DRow["SRNO"] = DTabGroupByTitle.Rows.Count + 1;
                        DTabLotDetail.Rows.Add(DRow);
                        GrdDetGroup.FocusedRowHandle = GrdDetGroup.FocusedRowHandle + 1;
                        GrdDetGroup.FocusedColumn = GrdDetGroup.Columns["GROUPTITLE"];
                    }
                    else if (Val.ToString(GrdDetGroup.GetFocusedRowCellValue("GROUPTITLE")) != "" && GrdDetGroup.IsLastRow == false)
                    {
                        GrdDetGroup.FocusedRowHandle = GrdDetGroup.FocusedRowHandle + 1;
                        GrdDetGroup.FocusedColumn = GrdDetGroup.Columns["GROUPTITLE"];
                    }
                    else
                    {
                        BtnSave.Focus();
                        BtnSave.PerformClick();
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            
        }

        private void BtnGroupRowDelete_Click(object sender, EventArgs e)
        {
            GrdDetGroup.DeleteRow(GrdDetGroup.FocusedRowHandle);
        }

        private void txtNewGroup_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void CmbReportView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Val.ToString(CmbReportView.SelectedItem) == "GridView")
            {
                GrdDet.Columns["ISDATAAREA"].Visible = false;
                GrdDet.Columns["ISROWAREA"].Visible = false;
                GrdDet.Columns["ISCOLUMNAREA"].Visible = false;

                GrdDetGroup.Columns["DATAAREA"].Visible = false;
                GrdDetGroup.Columns["ROWAREA"].Visible = false;
                GrdDetGroup.Columns["COLUMNAREA"].Visible = false;
            }
            else if (Val.ToString(CmbReportView.SelectedItem) == "PivotView")
            {
                GrdDet.Columns["ISDATAAREA"].Visible = true;
                GrdDet.Columns["ISROWAREA"].Visible = true;
                GrdDet.Columns["ISCOLUMNAREA"].Visible = true;
                
                GrdDetGroup.Columns["DATAAREA"].Visible = true;
                GrdDetGroup.Columns["ROWAREA"].Visible = true;
                GrdDetGroup.Columns["COLUMNAREA"].Visible = true;
            }
        }

        private void BtnAddRow_Click(object sender, EventArgs e)
        {
            if (xtraTabControl1.SelectedTabPageIndex == 0)
            {
                DTabLotDetail.Rows.Add(DTabLotDetail.NewRow());
            }
            else
            {
                DTabGroupByTitle.Rows.Add(DTabGroupByTitle.NewRow());
            }
           
        }
    }
}
