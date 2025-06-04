using BusLib;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.XtraTreeList;
using System;
using System.Data;
using System.Windows.Forms;
using Config = BusLib.Configuration.BOConfiguration;

namespace MahantExport.Report
{
    public partial class FrmFilterStockReport : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        MST_ReportProperty mProperty = new MST_ReportProperty();
        System.Diagnostics.Stopwatch watch = null;

        BOMST_Report ObjReport = new BOMST_Report();
        DataTable mDTabFieldDetail = new DataTable();
        DataTable mDTabRowArea = new DataTable();
        DataTable mDTabColumnArea = new DataTable();
        DataTable mDTabDataArea = new DataTable();
        int mIntFilterHeight = 0;
        DataRow mDrow = null;
        DataSet mDS = new DataSet();
       string mStrReportGroupNew = string.Empty;
        string pStrShiftType = "";

        String PasteData = "";
        IDataObject PasteclipData = Clipboard.GetDataObject();

        #region Property

        public FrmFilterStockReport()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            mStrReportGroupNew = "Stock Reports";

            AttachFormDefaultEvent();
            Val.FormGeneralSetting(this);
            lblTitle.Text = "Stock Reports";

            this.Show();
        }

        private void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            //ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.FormKeyPress = true;

            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);

        }
        
        private void FrmSearch_Load(object sender, EventArgs e)
        {
            DataTable DTab = new BOMST_Report().GetDataSummaryForReportNew(mStrReportGroupNew);
            treLstAccGroupMaster.DataSource = DTab;
            treLstAccGroupMaster.ParentFieldName = "REPORT_ID";
            treLstAccGroupMaster.KeyFieldName = "REPORTGROUP_ID";
            treLstAccGroupMaster.CollapseAll();
        }
        
        private void FrmSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

#endregion

        #region Control Event
        private void BtnGenerateReport_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtStock.Text.Length == 0) txtStock.Tag = string.Empty;
                if (txtShape.Text.Length == 0) txtShape.Tag = string.Empty;
                if (txtColor.Text.Length == 0) txtColor.Tag = string.Empty;
                if (txtCut.Text.Length == 0) txtCut.Tag = string.Empty;
                if (txtPol.Text.Length == 0) txtPol.Tag = string.Empty;
                if (txtSym.Text.Length == 0) txtSym.Tag = string.Empty;
                if (txtFromCarat.Text.Length == 0) txtFromCarat.Tag = string.Empty;
                if (txtFL.Text.Length == 0) txtFL.Tag = string.Empty;

                if (mDrow == null)
                {
                    Global.Message("Select Atleast One Report For Generate");
                    return;
                }

                mProperty = new MST_ReportProperty();
                mProperty.REPORT_ID = Val.ToInt(mDrow["REPORT_ID"]);
                mProperty.REPORTTYPE = Val.ToString(mDrow["REPORTTYPE"]);
                mProperty.REMARK = Val.ToString(mDrow["REMARK"]);

                if (RbtFullStock.Checked == true)
                {
                    mProperty.STOCKTYPE = RbtFullStock.Tag.ToString();
                }
                else if (RbtDeptStock.Checked == true)
                {
                    mProperty.STOCKTYPE = RbtDeptStock.Tag.ToString();
                }
                else if (RbtMYStock.Checked == true)
                {
                    mProperty.STOCKTYPE = RbtMYStock.Tag.ToString();
                }
                else if (RbtOtherStock.Checked == true)
                {
                    mProperty.STOCKTYPE = RbtOtherStock.Tag.ToString();
                }


                if (DTPFromDate.Checked == true && DTPToDate.Checked == true)
                {
                    mProperty.FROMDATE = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
                    mProperty.TODATE = Val.SqlDate(DTPToDate.Value.ToShortDateString());
                }
                else
                {
                    mProperty.FROMDATE = null;
                    mProperty.TODATE = null;
                }

                if (DtpFromSaleDate.Checked == true && DtpToSaleDate.Checked == true)
                {
                    mProperty.FROMSALEDATE = Val.SqlDate(DtpFromSaleDate.Value.ToShortDateString());
                    mProperty.TOSALEDATE = Val.SqlDate(DtpToSaleDate.Value.ToShortDateString());
                }
                else
                {
                    mProperty.FROMSALEDATE = null;
                    mProperty.TOSALEDATE = null;
                }


                if (Val.ToString(mDrow["REPORTVIEW"]) == "GridView")
                {
                    mProperty.GROUPBY = GroupByBox.GetTagValue;
                }
                if (Val.ToString(mDrow["REPORTVIEW"]) == "PivotView")
                {
                    mProperty.GROUPBY = RowByBox.GetTagValue + "," + ColumnByBox.GetTagValue;
                }
                mProperty.SPNAME = Val.ToString(mDrow["SPNAME"]);
           
                if (RbtSummary.Checked == true)
                {
                    mProperty.REPORTTYPE = "S";
                }

                else if (RbtDetail.Checked == true)
                {
                    mProperty.REPORTTYPE = "D";
                }

                DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;

                BtnGenerateReport.Enabled = false;
                PnlLoding.Visible = true;

                if (backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.CancelAsync();
                }
                watch = System.Diagnostics.Stopwatch.StartNew();
                backgroundWorker1.RunWorkerAsync();


                
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
                return;
            }
       }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            txtStock.Text = string.Empty;
            txtStock.Tag = string.Empty;

            txtShape.Text = string.Empty;
            txtShape.Tag = string.Empty;

            txtColor.Text = string.Empty;
            txtColor.Tag = string.Empty;

            txtCut.Text = string.Empty;
            txtCut.Tag = string.Empty;

            txtPol.Text = string.Empty;
            txtPol.Tag = string.Empty;

            txtSym.Text = string.Empty;
            txtSym.Tag = string.Empty;

            txtFromCarat.Text = string.Empty;
            
            txtFL.Text = string.Empty;
            txtFL.Tag = string.Empty;
            
            txtToCarat.Text = string.Empty;

        }

        
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

          private void treLstAccGroupMaster_Click(object sender, EventArgs e)
        {
            try
            {
                TreeList tree = sender as TreeList;
                TreeListHitInfo hi = tree.CalcHitInfo(tree.PointToClient(Control.MousePosition));
                if (hi.Node != null)
                {
                    treLstAccGroupMaster.GetDataRecordByNode(hi.Node);

                    DataRowView DR = (DataRowView)treLstAccGroupMaster.GetDataRecordByNode(hi.Node);

                    mDrow = (DataRow)DR.Row;

                    DataSet DS = new BOMST_Report().GetData(Val.ToInt(mDrow["OLDREPORT_ID"]));

                    mDTabFieldDetail = DS.Tables[0].Copy();

                    lblTitle.Text = Val.ToString(mDrow["REPORTNAME"]);

                    mDTabFieldDetail.DefaultView.RowFilter = "ISGROUP = 1";
                    mDTabFieldDetail.DefaultView.Sort = "SRNO";
                    DataTable DTabGroupBy = mDTabFieldDetail.DefaultView.ToTable();
                    GroupByBox.DTab = DTabGroupBy;
                    GroupByBox.SetDefauleGroupBy(Val.ToString(mDrow["GROUPBYTAG"]), Val.ToString(mDrow["GROUPBYCAPTION"]));

                    mDTabColumnArea = mDTabFieldDetail.Copy();
                    mDTabRowArea = mDTabFieldDetail.Copy();

                    ColumnByBox.DTab = mDTabColumnArea;
                    ColumnByBox.SetDefauleGroupBy(Val.ToString(mDrow["COLUMNAREA"]), Val.ToString(mDrow["COLUMNAREA"]));

                    mDTabFieldDetail.DefaultView.RowFilter = "ISGROUP = 0";
                    mDTabFieldDetail.DefaultView.Sort = "SRNO";
                    mDTabDataArea = mDTabFieldDetail.DefaultView.ToTable().Copy();

                    RowByBox.DTab = mDTabRowArea;
                    RowByBox.SetDefauleGroupBy(Val.ToString(mDrow["ROWAREA"]), Val.ToString(mDrow["ROWAREA"]));

                    DataByBox.DTab = mDTabDataArea;
                    DataByBox.SetDefauleGroupBy(Val.ToString(mDrow["DATAAREA"]), Val.ToString(mDrow["DATAAREA"]));

                    PanelGrid.Visible = false;
                    PanelPivot.Visible = false;

                    if (Val.ToString(mDrow["REPORTVIEW"]) == "GridView")
                    {
                        PanelGrid.Visible = true;
                        PanelGrid.Height = 600;
                        GroupByBox.Height = 600;
                        GrpPanel.AutoScroll = false;
                    }
                    else if (Val.ToString(mDrow["REPORTVIEW"]) == "PivotView")
                    {
                        PanelPivot.Visible = true;
                        GrpPanel.AutoScroll = true;
                    }
                    DR = null;
                }
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.ToString());
            }
        }

#endregion

        #region Filter

        public string GetFilterString()
        {
            mIntFilterHeight = 0;
            string Str = string.Empty;

            if (RbtFullStock.Checked == true)
            {
                Str = Str + "Stock Type : " + RbtFullStock.Text.ToString() + "\n";
                mIntFilterHeight = mIntFilterHeight + 15;
            }
            else if (RbtDeptStock.Checked == true)
            {
                Str = Str + "Stock Type : " + RbtDeptStock.Text.ToString() + "\n";
                mIntFilterHeight = mIntFilterHeight + 15;
            }
            else if (RbtMYStock.Checked == true)
            {
                Str = Str + "Stock Type : " + RbtMYStock.Text.ToString() + "\n";
                mIntFilterHeight = mIntFilterHeight + 15;
            }
            else if (RbtOtherStock.Checked == true)
            {
                Str = Str + "Stock Type : " + RbtOtherStock.Text.ToString() + "\n";
                mIntFilterHeight = mIntFilterHeight + 15;
            }

            if (txtStock.Text.Length != 0)
            {
                Str = Str + "Kapan : " + txtStock.Text + "\n";
                mIntFilterHeight = mIntFilterHeight + 15;
            }
            if (txtShape.Text.Length != 0)
            {
                Str = Str + "From Process : " + txtShape.Text + "\n";
                mIntFilterHeight = mIntFilterHeight + 15;
            }
            if (txtColor.Text.Length != 0)
            {
                Str = Str + "To Process : " + txtColor.Text + "\n";
                mIntFilterHeight = mIntFilterHeight + 15;
            }
            if (txtCut.Text.Length != 0)
            {
                Str = Str + "Sub Process : " + txtCut.Text + "\n";
                mIntFilterHeight = mIntFilterHeight + 15;
            }

            if (txtPol.Text.Length != 0)
            {
                Str = Str + "From Department : " + txtPol.Text + "\n";
                mIntFilterHeight = mIntFilterHeight + 15;
            }
            if (txtSym.Text.Length != 0)
            {
                Str = Str + "To Department : " + txtSym.Text + "\n";
                mIntFilterHeight = mIntFilterHeight + 15;
            }
            
            if (txtFL.Text.Length != 0)
            {
                Str = Str + "From Employee : " + txtFL.Text + "\n";
                mIntFilterHeight = mIntFilterHeight + 15;
            }
            if (DTPFromDate.Checked == true && DTPToDate.Checked == true)
            {
                Str = Str + "Transaction Date : " + DTPFromDate.Text + " To " + DTPToDate.Text + "\n";
                mIntFilterHeight = mIntFilterHeight + 15;
            }
            return Str;
        }
        #endregion

        #region KeyPress

        private void txtNextProcess_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Global.OnKeyPressEveToPopup(e))
            {
                this.Cursor = Cursors.WaitCursor;
                FrmSearchPopupBoxMultipleSelect FrmSearch = new FrmSearchPopupBoxMultipleSelect();
                FrmSearch.mDTab = new BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_PROCESS);
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                FrmSearch.mStrSearchField = "PROCESSNAME";
                FrmSearch.mStrColumnsToHide = "PROCESS_ID";
                FrmSearch.ValueMemeter = "PROCESS_ID";
                FrmSearch.DisplayMemeter = "PROCESSNAME";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.SelectedDisplaymember != "" && FrmSearch.SelectedValuemember != "")
                {
                    txtCut.Text = Val.ToString(FrmSearch.SelectedDisplaymember);
                    txtCut.Tag = Val.ToString(FrmSearch.SelectedValuemember);
                }
                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
        }

        private void txtFromFactoryHead_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void txtToFactoryHead_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void txtFromManager_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void txtToManager_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void txtLotNo_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }


        private void txtKapan_KeyPress(object sender, KeyPressEventArgs e)
        {
            //try
            //{
            //    if (Global.OnKeyPressEveToPopup(e))
            //    {
            //        this.Cursor = Cursors.WaitCursor;
            //        FrmSearchPopupBoxMultipleSelect FrmSearch = new FrmSearchPopupBoxMultipleSelect();
            //        FrmSearch.mDTab = new BOTRN_SinglePacketCreate().FindKapan();
            //        FrmSearch.mStrSearchText = e.KeyChar.ToString();
            //        FrmSearch.mStrSearchField = "KAPANNAME";
            //        FrmSearch.mStrColumnsToHide = "KAPAN_ID";
            //        FrmSearch.ValueMemeter = "KAPAN_ID";
            //        FrmSearch.DisplayMemeter = "KAPANNAME";
            //        this.Cursor = Cursors.Default;
            //        FrmSearch.ShowDialog();
            //        e.Handled = true;
            //        if (FrmSearch.SelectedDisplaymember != "" && FrmSearch.SelectedValuemember != "")
            //        {
            //            txtKapan.Text = Val.ToString(FrmSearch.SelectedDisplaymember);
            //            txtKapan.Tag = Val.ToString(FrmSearch.SelectedValuemember);
            //        }
            //        FrmSearch.Hide();
            //        FrmSearch.Dispose();
            //        FrmSearch = null;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    this.Cursor = Cursors.Default;
            //    Global.MessageError(ex.Message);
            //}
        }

        private void txtFromProcess_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearchPopupBoxMultipleSelect FrmSearch = new FrmSearchPopupBoxMultipleSelect();
                    FrmSearch.mDTab = new BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_PROCESS);
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    FrmSearch.mStrColumnsToHide = "PROCESS_ID";
                    FrmSearch.mStrSearchField = "PROCESSNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    FrmSearch.ValueMemeter = "PROCESS_ID";
                    FrmSearch.DisplayMemeter = "PROCESSNAME";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.SelectedDisplaymember != "" && FrmSearch.SelectedValuemember != "")
                    {
                        txtShape.Text = Val.ToString(FrmSearch.SelectedDisplaymember);
                        txtShape.Tag = Val.ToString(FrmSearch.SelectedValuemember);
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.MessageError(ex.Message);
            }
        }

        private void txtToProcess_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearchPopupBoxMultipleSelect FrmSearch = new FrmSearchPopupBoxMultipleSelect();
                    FrmSearch.mDTab = new BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_PROCESS);
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    FrmSearch.mStrColumnsToHide = "PROCESS_ID";
                    FrmSearch.mStrSearchField = "PROCESSNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    FrmSearch.ValueMemeter = "PROCESS_ID";
                    FrmSearch.DisplayMemeter = "PROCESSNAME";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.SelectedDisplaymember != "" && FrmSearch.SelectedValuemember != "")
                    {
                        txtColor.Text = Val.ToString(FrmSearch.SelectedDisplaymember);
                        txtColor.Tag = Val.ToString(FrmSearch.SelectedValuemember);
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.MessageError(ex.Message);
            }
        }

        private void txtFromDepartment_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearchPopupBoxMultipleSelect FrmSearch = new FrmSearchPopupBoxMultipleSelect();
                    FrmSearch.mDTab = new BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_DEPARTMENT);
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    FrmSearch.mStrColumnsToHide = "DEPARTMENT_ID";
                    FrmSearch.mStrSearchField = "DEPARTMENTNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    FrmSearch.ValueMemeter = "DEPARTMENT_ID";
                    FrmSearch.DisplayMemeter = "DEPARTMENTNAME";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.SelectedDisplaymember != "" && FrmSearch.SelectedValuemember != "")
                    {
                        txtPol.Text = Val.ToString(FrmSearch.SelectedDisplaymember);
                        txtPol.Tag = Val.ToString(FrmSearch.SelectedValuemember);
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.MessageError(ex.Message);
            }
        }

        private void txtToDepartment_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearchPopupBoxMultipleSelect FrmSearch = new FrmSearchPopupBoxMultipleSelect();
                    FrmSearch.mDTab = new BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_DEPARTMENT);
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    FrmSearch.mStrColumnsToHide = "DEPARTMENT_ID";
                    FrmSearch.mStrSearchField = "DEPARTMENTNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    FrmSearch.ValueMemeter = "DEPARTMENT_ID";
                    FrmSearch.DisplayMemeter = "DEPARTMENTNAME";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.SelectedDisplaymember != "" && FrmSearch.SelectedValuemember != "")
                    {
                        txtSym.Text = Val.ToString(FrmSearch.SelectedDisplaymember);
                        txtSym.Tag = Val.ToString(FrmSearch.SelectedValuemember);
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.MessageError(ex.Message);
           }
        }


        private void txtKapanManager_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        
        private void txtFromEmployee_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearchPopupBoxMultipleSelect FrmSearch = new FrmSearchPopupBoxMultipleSelect();
                    FrmSearch.mDTab = new BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_EMPLOYEE);
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    FrmSearch.mStrSearchField = "EMPLOYEENAME,EMPLOYEECODE";
                    FrmSearch.mStrColumnsToHide = "EMPLOYEE_ID";
                    FrmSearch.ValueMemeter = "EMPLOYEE_ID";
                    FrmSearch.DisplayMemeter = "EMPLOYEENAME";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.SelectedDisplaymember != "" && FrmSearch.SelectedValuemember != "")
                    {
                        txtFL.Text = Val.ToString(FrmSearch.SelectedDisplaymember);
                        txtFL.Tag = Val.ToString(FrmSearch.SelectedValuemember);
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.MessageError(ex.Message);
            }
        }

        private void txtToEmployee_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

#endregion

        private void txtPriceDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PRICEDATE,REMARK";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_PRICEHEAD);
                    FrmSearch.mStrColumnsToHide = "PRICE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtClarity.Tag = Val.ToString(FrmSearch.DRow["PRICE_ID"]);
                        txtClarity.Text = Val.ToString(FrmSearch.DRow["PRICEDATE"]);
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

        private void txtTable_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            mDS = ObjReport.GenerateMaintainanceReport(mProperty);

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            BtnGenerateReport.Enabled = true;
            PnlLoding.Visible = false;
            watch.Stop();
            lblTime.Text = string.Format("{0:hh\\:mm\\:ss}", watch.Elapsed);

            if (Val.ToString(mDrow["REPORTVIEW"]) == "GridView")
            {
                FrmGridReportViewerWithBand FrmGridReportViewerWithBand = new FrmGridReportViewerWithBand();
                FrmGridReportViewerWithBand.MdiParent = Global.gMainRef;
                FrmGridReportViewerWithBand.ShowForm(mDS,
                    mDrow,
                    mDTabFieldDetail,
                    GroupByBox.GetTagValue,
                    GroupByBox.GetTextValue,
                    GetFilterString(),
                    ChkNoGrouping.Checked,
                    GroupByBox.GetTagValue,
                    GroupByBox.GetTextValue,
                    mProperty,
                    DTPFromDate.Value.ToShortDateString(),
                    DTPToDate.Value.ToShortDateString(),
                    "",
                    mIntFilterHeight,
                    pStrShiftType
                    );
            }

            else if (Val.ToString(mDrow["REPORTVIEW"]) == "PivotView")
            {
                FrmPivotReportViewer FrmPReportViewer = new FrmPivotReportViewer();
                FrmPReportViewer.MdiParent = Global.gMainRef;
                FrmPReportViewer.ShowForm(mDS,
                                    mDrow,
                                    mDTabFieldDetail,
                                    GroupByBox.GetTagValue,
                                    GroupByBox.GetTextValue,
                                    GetFilterString(),
                                    ChkNoGrouping.Checked,
                                    GroupByBox.GetTagValue,
                                    GroupByBox.GetTextValue,
                                    mProperty,
                                    DTPFromDate.Value.ToShortDateString(),
                                    DTPToDate.Value.ToShortDateString(),
                                    "",
                                    RowByBox.GetTagValue,
                                    ColumnByBox.GetTagValue,
                                    DataByBox.GetTagValue,
                                    mIntFilterHeight,
                                    pStrShiftType
                    );
            }

        }

        private void txtStock_MouseDown(object sender, MouseEventArgs e)
        {
            if (txtStock.Focus())
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    PasteData = Convert.ToString(PasteclipData.GetData(System.Windows.Forms.DataFormats.Text));
                }
            }
        }

        private void cLabel11_Click(object sender, EventArgs e)
        {

        }

        private void DTPFromDate_ValueChanged(object sender, EventArgs e)
        {

        }
    }

}