using MahantExport.Masters;
using MahantExport.Stock;
using MahantExport.Utility;
using BusLib;
using BusLib.Configuration;
using BusLib.CRM;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using Google.API.Translate;
using OfficeOpenXml;
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
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace MahantExport.CRM
{
    public partial class FrmCustomerAutoEmailSetting : DevControlLib.cDevXtraForm
    {
        String PasteData = "";
        IDataObject PasteclipData = Clipboard.GetDataObject();

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        DataTable DTabSize = new DataTable();
        DataTable DTabDays = new DataTable();
        //LiveStockProperty mLStockProperty = new LiveStockProperty();
        CRM_CustomerAutoMailCriteriaSave ObjEmail = new CRM_CustomerAutoMailCriteriaSave();

        CRMCustomerAutoMailCriteriaProperty mCRMStockProperty = new CRMCustomerAutoMailCriteriaProperty();
        BOFormPer ObjPer = new BOFormPer();
        LiveStockProperty mLStockProperty = new LiveStockProperty();

        DataTable DTabDetail = new DataTable();
        string StrEmail = "";

        #region Property Settings

        public FrmCustomerAutoEmailSetting()
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

            BtnEmail.Visible = false;
            btnExport.Visible = false;
            this.Show();
            FillListControls();
        }

        public void ShowForm(CRMCustomerAutoMailCriteriaProperty Property, string StrCustomeName)
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            BtnEmail.Visible = true;
            btnExport.Visible = true;
            txtCoustomer.Text = StrCustomeName;


            this.Show();
            FillListControls();

            mCRMStockProperty = Property;


            ListShape.SetSelectedCheckBox(mCRMStockProperty.MULTYSHAPE_ID);
            ListColor.SetSelectedCheckBox(mCRMStockProperty.MULTYCOLOR_ID);
            ListClarity.SetSelectedCheckBox(mCRMStockProperty.MULTYCLARITY_ID);
            ListCut.SetSelectedCheckBox(mCRMStockProperty.MULTYCUT_ID);
            ListPol.SetSelectedCheckBox(mCRMStockProperty.MULTYPOL_ID);
            ListSym.SetSelectedCheckBox(mCRMStockProperty.MULTYSYM_ID);
            ListFL.SetSelectedCheckBox(mCRMStockProperty.MULTYFL_ID);

            ListFancyColor.SetSelectedCheckBox(mCRMStockProperty.MULTYFANCYCOLOR_ID);
            ListLocation.SetSelectedCheckBox(mCRMStockProperty.MULTYLOCATION_ID);
            ListMilky.SetSelectedCheckBox(mCRMStockProperty.MULTYMILKY_ID);
            ListLab.SetSelectedCheckBox(mCRMStockProperty.MULTYLAB_ID);
            ListBox.SetSelectedCheckBox(mCRMStockProperty.MULTYBOX_ID);
            ListStatus.SetSelectedCheckBox(mCRMStockProperty.WEBSTATUSNAME);

            txtFromLength.Text = Val.ToString(mCRMStockProperty.FROMLENGTH);
            txtToLength.Text = Val.ToString(mCRMStockProperty.TOLENGTH);

            txtFromWidth.Text = Val.ToString(mCRMStockProperty.FROMWIDTH);
            txtToWidth.Text = Val.ToString(mCRMStockProperty.TOWIDTH);

            txtFromHeight.Text = Val.ToString(mCRMStockProperty.FROMHEIGHT);
            txtToHeight.Text = Val.ToString(mCRMStockProperty.TOHEIGHT);

            txtFromTablePer.Text = Val.ToString(mCRMStockProperty.FROMTABLEPER);
            txtToTablePer.Text = Val.ToString(mCRMStockProperty.TOTABLEPER);

            txtFromDepthPer.Text = Val.ToString(mCRMStockProperty.FROMDEPTHPER);
            txtToDepthPer.Text = Val.ToString(mCRMStockProperty.TODEPTHPER);

            CmbEmilType.Text = mCRMStockProperty.EMAILTYPE;
            ChkActive.Checked = mCRMStockProperty.ISACTIVE;

            txtCoustomer.Tag = mCRMStockProperty.CUSTOMER_ID;

            StrEmail = mCRMStockProperty.EMAIL_ID;




            if (RbtStoneNo.Checked == true)
            {
                txtStoneCertiMFGMemo.Text = mCRMStockProperty.STOCKNO;
                mCRMStockProperty.LABREPORTNO = string.Empty;
                mCRMStockProperty.SERIALNO = string.Empty;
                mCRMStockProperty.MEMONO = string.Empty;
            }
            else if (RbtCertiNo.Checked == true)
            {
                mCRMStockProperty.STOCKNO = string.Empty;
                txtStoneCertiMFGMemo.Text = mCRMStockProperty.LABREPORTNO;
                mCRMStockProperty.SERIALNO = string.Empty;
                mCRMStockProperty.MEMONO = string.Empty;
            }
            else if (RbtSerialNo.Checked == true)
            {
                mCRMStockProperty.STOCKNO = string.Empty;
                mCRMStockProperty.LABREPORTNO = string.Empty;
                txtStoneCertiMFGMemo.Text = mCRMStockProperty.SERIALNO;
                mCRMStockProperty.MEMONO = string.Empty;
            }
            else if (RbtMemoNo.Checked == true)
            {
                mCRMStockProperty.STOCKNO = string.Empty;
                mCRMStockProperty.LABREPORTNO = string.Empty;
                mCRMStockProperty.SERIALNO = string.Empty;
                txtStoneCertiMFGMemo.Text = mCRMStockProperty.MEMONO;
            }

            DTabSize.Rows[0]["FROMCARAT"] = mCRMStockProperty.FROMCARAT1;
            DTabSize.Rows[0]["TOCARAT"] = mCRMStockProperty.TOCARAT1;

            DTabSize.Rows[1]["FROMCARAT"] = mCRMStockProperty.FROMCARAT2;
            DTabSize.Rows[1]["TOCARAT"] = mCRMStockProperty.TOCARAT2;

            DTabSize.Rows[2]["FROMCARAT"] = mCRMStockProperty.FROMCARAT3;
            DTabSize.Rows[2]["TOCARAT"] = mCRMStockProperty.TOCARAT3;

            DTabSize.Rows[3]["FROMCARAT"] = mCRMStockProperty.FROMCARAT4;
            DTabSize.Rows[3]["TOCARAT"] = mCRMStockProperty.TOCARAT4;

            DTabSize.Rows[4]["FROMCARAT"] = mCRMStockProperty.FROMCARAT5;
            DTabSize.Rows[4]["TOCARAT"] = mCRMStockProperty.TOCARAT5;

            MainGrdSize.DataSource = DTabSize;
            GrdSize.RefreshData();

            DataTable DTab = ObjStock.GetDataFroAutoMailDateTime(mCRMStockProperty.CUSTOMER_ID);
            MainGrdDay.DataSource = DTab;
            GrdDays.RefreshData();

        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            //ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjStock);
        }

        public void FillListControls()
        {
            //TabSize = new DataTable();
            DTabSize.Columns.Add(new DataColumn("FROMCARAT", typeof(Double)));
            DTabSize.Columns.Add(new DataColumn("TOCARAT", typeof(Double)));

            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());

            MainGrdSize.DataSource = DTabSize;
            GrdSize.RefreshData();

            DTabDays = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_DAYS);
            MainGrdDay.DataSource = DTabDays;
            GrdDays.RefreshData();

            DataTable DTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PARAALL);

            DataRow[] DR = DTab.Select("PARATYPE='SHAPE'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                ListShape.DTab = DTTemp.DefaultView.ToTable();
                ListShape.DisplayMember = "SHORTNAME";
                ListShape.ValueMember = "PARA_ID";
            }

            DR = DTab.Select("PARATYPE='COLOR' AND PARAGROUP = 'SINGLE'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                ListColor.DTab = DTTemp.DefaultView.ToTable();
                ListColor.DisplayMember = "SHORTNAME";
                ListColor.ValueMember = "PARA_ID";
            }

            DR = DTab.Select("PARATYPE='CLARITY' AND PARAGROUP = 'SINGLE'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                ListClarity.DTab = DTTemp.DefaultView.ToTable();
                ListClarity.DisplayMember = "SHORTNAME";
                ListClarity.ValueMember = "PARA_ID";
            }

            DR = DTab.Select("PARATYPE='CUT'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                ListCut.DTab = DTTemp.DefaultView.ToTable();
                ListCut.DisplayMember = "SHORTNAME";
                ListCut.ValueMember = "PARA_ID";
            }

            DR = DTab.Select("PARATYPE='POLISH'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                ListPol.DTab = DTTemp.DefaultView.ToTable();
                ListPol.DisplayMember = "SHORTNAME";
                ListPol.ValueMember = "PARA_ID";
            }

            DR = DTab.Select("PARATYPE='SYMMETRY'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                ListSym.DTab = DTTemp.DefaultView.ToTable();
                ListSym.DisplayMember = "SHORTNAME";
                ListSym.ValueMember = "PARA_ID";
            }

            DR = DTab.Select("PARATYPE='FLUORESCENCE'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                ListFL.DTab = DTTemp.DefaultView.ToTable();
                ListFL.DisplayMember = "SHORTNAME";
                ListFL.ValueMember = "PARA_ID";
            }

            DR = DTab.Select("PARATYPE='FANCYCOLOR'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                ListFancyColor.DTab = DTTemp.DefaultView.ToTable();
                ListFancyColor.DisplayMember = "SHORTNAME";
                ListFancyColor.ValueMember = "PARA_ID";
            }

            DR = DTab.Select("PARATYPE='MILKY'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                ListMilky.DTab = DTTemp.DefaultView.ToTable();
                ListMilky.DisplayMember = "SHORTNAME";
                ListMilky.ValueMember = "PARA_ID";
            }

            DR = DTab.Select("PARATYPE='LOCATION'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                ListLocation.DTab = DTTemp.DefaultView.ToTable();
                ListLocation.DisplayMember = "SHORTNAME";
                ListLocation.ValueMember = "PARA_ID";
            }

            DR = DTab.Select("PARATYPE='BOX'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                ListBox.DTab = DTTemp.DefaultView.ToTable();
                ListBox.DisplayMember = "SHORTNAME";
                ListBox.ValueMember = "PARA_ID";
            }

            DR = DTab.Select("PARATYPE='LAB'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                ListLab.DTab = DTTemp.DefaultView.ToTable();
                ListLab.DisplayMember = "SHORTNAME";
                ListLab.ValueMember = "PARA_ID";
            }

            DR = DTab.Select("PARATYPE='WEBSTATUS'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "PARANAME";
                ListStatus.DTab = DTTemp.DefaultView.ToTable();
                ListStatus.DisplayMember = "SHORTNAME";
                ListStatus.ValueMember = "SHORTNAME";

                ListStatus.SetSelectedCheckBox("AVAILABLE");
            }

        }


        #endregion

        #region Control Event

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblResetCarat_Click(object sender, EventArgs e)
        {
            DTabSize.Rows.Clear();
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
        }

        public bool ValSave()
        {
            if (txtCoustomer.Text.Trim().Equals(string.Empty))
            {
                Global.Message("Please Enter CoustomerName First");
                txtCoustomer.Focus();
                return false;
            }
            else if (CmbEmilType.Text.Trim().Equals(string.Empty))
            {
                Global.Message("Please Select Email Type First");
                CmbEmilType.Focus();
                return false;
            }

            else if ((ListShape.GetSelectedReportTagValues() == "") && (ListColor.GetSelectedReportTextValues() == "") && (ListColor.GetSelectedReportTextValues() == "") && (ListClarity.GetSelectedReportTextValues() == "") && (ListCut.GetSelectedReportTextValues() == "") && (ListFL.GetSelectedReportTextValues() == "") && (ListPol.GetSelectedReportTextValues() == "") &&
                     (ListSym.GetSelectedReportTextValues() == "") && (ListLocation.GetSelectedReportTextValues() == "") && (ListMilky.GetSelectedReportTextValues() == "") && (ListStatus.GetSelectedReportTextValues() == "") && (ListLab.GetSelectedReportTextValues() == "") && (ListBox.GetSelectedReportTextValues() == "") &&
                     (txtFromLength.Text.Length == 0) && (txtToLength.Text.Length == 0) && (txtFromWidth.Text.Length == 0) && (txtToWidth.Text.Length == 0) && (txtFromHeight.Text.Length == 0) && (txtToHeight.Text.Length == 0) && (txtFromDepthPer.Text.Length == 0) && (txtToDepthPer.Text.Length == 0) &&
                     (txtFromDepthPer.Text.Length == 0) && (txtToTablePer.Text.Length == 0) && (Val.Val(DTabSize.Rows[0]["FROMCARAT"]) == 0) && (Val.Val(DTabSize.Rows[0]["TOCARAT"]) == 0) && (Val.Val(DTabSize.Rows[1]["FROMCARAT"]) == 0) && (Val.Val(DTabSize.Rows[1]["TOCARAT"]) == 0) && (Val.Val(DTabSize.Rows[2]["FROMCARAT"]) == 0) && (Val.Val(DTabSize.Rows[2]["TOCARAT"]) == 0)
                      && (Val.Val(DTabSize.Rows[3]["FROMCARAT"]) == 0) && (Val.Val(DTabSize.Rows[3]["TOCARAT"]) == 0) && (Val.Val(DTabSize.Rows[4]["FROMCARAT"]) == 0) && (Val.Val(DTabSize.Rows[4]["TOCARAT"]) == 0))
            {
                Global.Message("Please Seletec Any One Criteria");
                ListShape.Focus();
                return false;
            }

            return true;
        }

        private void txtStoneCertiMFGMemo_MouseDown(object sender, MouseEventArgs e)
        {
            if (txtStoneCertiMFGMemo.Focus())
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    PasteData = Convert.ToString(PasteclipData.GetData(System.Windows.Forms.DataFormats.Text));
                }
            }
            lblTotalCount.Text = "(" + txtStoneCertiMFGMemo.Text.Split(',').Length + ")";
        }

        private void txtStoneCertiMFGMemo_TextChanged(object sender, EventArgs e)
        {
            if (txtStoneCertiMFGMemo.Text.Length > 0 && Convert.ToString(PasteData) != "")
            {
                txtStoneCertiMFGMemo.SelectAll();
                String str1 = PasteData.Replace("\r\n", ",");                   //data.Replace(\n, ",");
                str1 = str1.Trim();
                str1 = str1.TrimEnd();
                str1 = str1.TrimStart();
                str1 = str1.TrimEnd(',');
                str1 = str1.TrimStart(',');
                txtStoneCertiMFGMemo.Text = str1;
                PasteData = "";
            }

            lblTotalCount.Text = "(" + txtStoneCertiMFGMemo.Text.Split(',').Length + ")";

        }

        private void txtStoneCertiMFGMemo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.V && e.Modifiers == Keys.Control)
            {
                IDataObject clipData = Clipboard.GetDataObject();
                String Data = Convert.ToString(clipData.GetData(System.Windows.Forms.DataFormats.Text));
                String str1 = Data.Replace("\r\n", ",");                   //data.Replace(\n, ",");
                str1 = str1.Trim();
                str1 = str1.TrimEnd();
                str1 = str1.TrimStart();
                str1 = str1.TrimEnd(',');
                str1 = str1.TrimStart(',');
                txtStoneCertiMFGMemo.Text = str1;
            }
            lblTotalCount.Text = "(" + txtStoneCertiMFGMemo.Text.Split(',').Length + ")";

        }
        #endregion

        #region Operation
        public void Clear()
        {
            ListShape.DeSelectAll();
            ListColor.DeSelectAll();
            ListClarity.DeSelectAll();
            ListCut.DeSelectAll();
            ListPol.DeSelectAll();
            ListSym.DeSelectAll();
            ListFL.DeSelectAll();
            ListMilky.DeSelectAll();
            ListFancyColor.DeSelectAll();
            ListLocation.DeSelectAll();
            ListStatus.DeSelectAll();
            ListBox.DeSelectAll();
            txtStoneCertiMFGMemo.Text = string.Empty;
            ListStatus.DeSelectAll();
            ListLab.DeSelectAll();

            DTabSize.Rows.Clear();
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());

            DTabDays = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_DAYS);
            MainGrdDay.DataSource = DTabDays;
            GrdDays.RefreshData();

            txtFromLength.Text = string.Empty;
            txtToLength.Text = string.Empty;

            txtFromWidth.Text = string.Empty;
            txtToWidth.Text = string.Empty;

            txtFromHeight.Text = string.Empty;
            txtToHeight.Text = string.Empty;

            txtFromTablePer.Text = string.Empty;
            txtToTablePer.Text = string.Empty;

            txtFromDepthPer.Text = string.Empty;
            txtToDepthPer.Text = string.Empty;
            txtCoustomer.Tag = string.Empty;
            txtCoustomer.Text = string.Empty;
            DtpvalidDate.Value = DateTime.Now;
            CmbEmilType.SelectedIndex = -1;
            ChkActive.Checked = false;
        }



        #endregion

        private void txtCoustomer_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE, PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    FrmSearch.mBoolISPostBack = true;
                    //FrmSearch.ISPostBackColumn = "ROUGHTYPE";
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);
                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtCoustomer.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtCoustomer.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
                    }
                    else
                    {
                        txtCoustomer.Text = string.Empty;
                    }

                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValSave() == false)
                {
                    return;
                }

                //for (int j = 0; j < DTabDays.Rows.Count; j++)
                //{
                //    //string dt = DTabDays.Rows[j]["TIME"].ToString();
                //    //string[] List = dt.Split(' ');
                //    //foreach (string author in List)
                //    //{
                //    //    if (dt == "")
                //    //        DTabDays.Rows[j]["TIME"] = DBNull.Value;
                //    //    else
                //    //        DTabDays.Rows[j]["TIME"] = Val.ToString(List[1]);
                //    //}
                //    DTabDays.Rows[j]["TIME"] = "null";
                //}
                //DTabDays.AcceptChanges();

                DTabDays.TableName = "Table";
                string StrXMLValues = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabDays.WriteXml(sw);
                    StrXMLValues = sw.ToString();
                }

                // mCRMStockProperty.AUTOEMAIL_ID = Val.ToString(txtAutoMailID.Tag).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtAutoMailID.Tag));
                mCRMStockProperty.CUSTOMER_ID = Guid.Parse(Val.ToString(txtCoustomer.Tag));
                mCRMStockProperty.EMAILTYPE = Val.ToString(CmbEmilType.SelectedItem);
                mCRMStockProperty.VALIDDATE = Val.SqlDate(DtpvalidDate.Value.ToShortDateString());
                mCRMStockProperty.ISACTIVE = Val.ToBoolean(ChkActive.Checked);
                mCRMStockProperty.MULTYSHAPE_ID = ListShape.GetSelectedReportTagValues();
                mCRMStockProperty.MULTISHAPENAME = ListShape.GetSelectedReportTextValues();
                mCRMStockProperty.MULTYCOLOR_ID = ListColor.GetSelectedReportTagValues();
                mCRMStockProperty.MULTYCOLORNAME = ListColor.GetSelectedReportTextValues();
                mCRMStockProperty.MULTYCLARITY_ID = ListClarity.GetSelectedReportTagValues();
                mCRMStockProperty.MULTYCLARITYNAME = ListClarity.GetSelectedReportTextValues();
                mCRMStockProperty.MULTYCUT_ID = ListCut.GetSelectedReportTagValues();
                mCRMStockProperty.MULTYCUTNAME = ListCut.GetSelectedReportTextValues();
                mCRMStockProperty.MULTYPOL_ID = ListPol.GetSelectedReportTagValues();
                mCRMStockProperty.MULTYPOLNAME = ListPol.GetSelectedReportTextValues();
                mCRMStockProperty.MULTYSYM_ID = ListSym.GetSelectedReportTagValues();
                mCRMStockProperty.MULTYSYMNAME = ListSym.GetSelectedReportTextValues();
                mCRMStockProperty.MULTYFL_ID = ListFL.GetSelectedReportTagValues();
                mCRMStockProperty.MULTYFLNAME = ListFL.GetSelectedReportTextValues();
                //mLStockProperty.MULTYDAYS_ID = ListDays.GetSelectedReportTagValues();

                mCRMStockProperty.MULTYFANCYCOLOR_ID = ListFancyColor.GetSelectedReportTagValues();
                mCRMStockProperty.MULTYFANCYCOLORNAME = ListFancyColor.GetSelectedReportTextValues();
                mCRMStockProperty.MULTYLOCATION_ID = ListLocation.GetSelectedReportTagValues();
                mCRMStockProperty.MULTYLOCATIONNAME = ListLocation.GetSelectedReportTextValues();
                mCRMStockProperty.MULTYMILKY_ID = ListMilky.GetSelectedReportTagValues();
                mCRMStockProperty.MULTYMILKYNAME = ListMilky.GetSelectedReportTextValues();
                mCRMStockProperty.MULTYLAB_ID = ListLab.GetSelectedReportTagValues();
                mCRMStockProperty.MULTYLABNAME = ListLab.GetSelectedReportTextValues();
                mCRMStockProperty.MULTYBOX_ID = ListBox.GetSelectedReportTagValues();
                mCRMStockProperty.MULTYBOXNAME = ListBox.GetSelectedReportTextValues();
                mCRMStockProperty.WEBSTATUS_ID = ListStatus.GetSelectedReportTagValues();
                mCRMStockProperty.WEBSTATUSNAME = ListStatus.GetSelectedReportTextValues();

                mCRMStockProperty.FROMCARAT1 = Val.Val(DTabSize.Rows[0]["FROMCARAT"]);
                mCRMStockProperty.TOCARAT1 = Val.Val(DTabSize.Rows[0]["TOCARAT"]);

                mCRMStockProperty.FROMCARAT2 = Val.Val(DTabSize.Rows[1]["FROMCARAT"]);
                mCRMStockProperty.TOCARAT2 = Val.Val(DTabSize.Rows[1]["TOCARAT"]);

                mCRMStockProperty.FROMCARAT3 = Val.Val(DTabSize.Rows[2]["FROMCARAT"]);
                mCRMStockProperty.TOCARAT3 = Val.Val(DTabSize.Rows[2]["TOCARAT"]);

                mCRMStockProperty.FROMCARAT4 = Val.Val(DTabSize.Rows[3]["FROMCARAT"]);
                mCRMStockProperty.TOCARAT4 = Val.Val(DTabSize.Rows[3]["TOCARAT"]);

                mCRMStockProperty.FROMCARAT5 = Val.Val(DTabSize.Rows[4]["FROMCARAT"]);
                mCRMStockProperty.TOCARAT5 = Val.Val(DTabSize.Rows[4]["TOCARAT"]);

                mCRMStockProperty.FROMLENGTH = Val.Val(txtFromLength.Text);
                mCRMStockProperty.TOLENGTH = Val.Val(txtToLength.Text);

                mCRMStockProperty.FROMWIDTH = Val.Val(txtFromWidth.Text);
                mCRMStockProperty.TOWIDTH = Val.Val(txtToWidth.Text);

                mCRMStockProperty.FROMHEIGHT = Val.Val(txtFromHeight.Text);
                mCRMStockProperty.TOHEIGHT = Val.Val(txtToHeight.Text);

                mCRMStockProperty.FROMTABLEPER = Val.Val(txtFromTablePer.Text);
                mCRMStockProperty.TOTABLEPER = Val.Val(txtToTablePer.Text);

                mCRMStockProperty.FROMDEPTHPER = Val.Val(txtFromDepthPer.Text);
                mCRMStockProperty.TODEPTHPER = Val.Val(txtToDepthPer.Text);

                if (RbtStoneNo.Checked == true)
                {
                    mCRMStockProperty.STOCKNO = txtStoneCertiMFGMemo.Text;
                    mCRMStockProperty.LABREPORTNO = string.Empty;
                    mCRMStockProperty.SERIALNO = string.Empty;
                    mCRMStockProperty.MEMONO = string.Empty;
                }
                else if (RbtCertiNo.Checked == true)
                {
                    mCRMStockProperty.STOCKNO = string.Empty;
                    mCRMStockProperty.LABREPORTNO = txtStoneCertiMFGMemo.Text;
                    mCRMStockProperty.SERIALNO = string.Empty;
                    mCRMStockProperty.MEMONO = string.Empty;
                }
                else if (RbtSerialNo.Checked == true)
                {
                    mCRMStockProperty.STOCKNO = string.Empty;
                    mCRMStockProperty.LABREPORTNO = string.Empty;
                    mCRMStockProperty.SERIALNO = txtStoneCertiMFGMemo.Text;
                    mCRMStockProperty.MEMONO = string.Empty;
                }
                else if (RbtMemoNo.Checked == true)
                {
                    mCRMStockProperty.STOCKNO = string.Empty;
                    mCRMStockProperty.LABREPORTNO = string.Empty;
                    mCRMStockProperty.SERIALNO = string.Empty;
                    mCRMStockProperty.MEMONO = txtStoneCertiMFGMemo.Text;
                }

                mCRMStockProperty.DAYXML = StrXMLValues;

                mCRMStockProperty = ObjEmail.Save(mCRMStockProperty);

                string StrReturnDesc = mCRMStockProperty.ReturnMessageDesc;
                Global.Message(StrReturnDesc);
                Clear();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }

        public void Fill()
        {
            mLStockProperty = new LiveStockProperty();

            mLStockProperty.MULTYSHAPE_ID = ListShape.GetSelectedReportTagValues();
            mLStockProperty.MULTYCOLOR_ID = ListColor.GetSelectedReportTagValues();
            mLStockProperty.MULTYCLARITY_ID = ListClarity.GetSelectedReportTagValues();
            mLStockProperty.MULTYCUT_ID = ListCut.GetSelectedReportTagValues();
            mLStockProperty.MULTYPOL_ID = ListPol.GetSelectedReportTagValues();
            mLStockProperty.MULTYSYM_ID = ListSym.GetSelectedReportTagValues();
            mLStockProperty.MULTYFL_ID = ListFL.GetSelectedReportTagValues();

            mLStockProperty.MULTYFANCYCOLOR_ID = ListFancyColor.GetSelectedReportTagValues();
            mLStockProperty.MULTYLOCATION_ID = ListLocation.GetSelectedReportTagValues();
            mLStockProperty.MULTYMILKY_ID = ListMilky.GetSelectedReportTagValues();
            mLStockProperty.MULTYLAB_ID = ListLab.GetSelectedReportTagValues();
            mLStockProperty.MULTYBOX_ID = ListBox.GetSelectedReportTagValues();
            mLStockProperty.WEBSTATUS = ListStatus.GetSelectedReportTagValues();

            mLStockProperty.FROMCARAT1 = Val.Val(DTabSize.Rows[0]["FROMCARAT"]);
            mLStockProperty.TOCARAT1 = Val.Val(DTabSize.Rows[0]["TOCARAT"]);

            mLStockProperty.FROMCARAT2 = Val.Val(DTabSize.Rows[1]["FROMCARAT"]);
            mLStockProperty.TOCARAT2 = Val.Val(DTabSize.Rows[1]["TOCARAT"]);

            mLStockProperty.FROMCARAT3 = Val.Val(DTabSize.Rows[2]["FROMCARAT"]);
            mLStockProperty.TOCARAT3 = Val.Val(DTabSize.Rows[2]["TOCARAT"]);

            mLStockProperty.FROMCARAT4 = Val.Val(DTabSize.Rows[3]["FROMCARAT"]);
            mLStockProperty.TOCARAT4 = Val.Val(DTabSize.Rows[3]["TOCARAT"]);

            mLStockProperty.FROMCARAT5 = Val.Val(DTabSize.Rows[4]["FROMCARAT"]);
            mLStockProperty.TOCARAT5 = Val.Val(DTabSize.Rows[4]["TOCARAT"]);

            mLStockProperty.FROMLENGTH = Val.Val(txtFromLength.Text);
            mLStockProperty.TOLENGTH = Val.Val(txtToLength.Text);

            mLStockProperty.FROMWIDTH = Val.Val(txtFromWidth.Text);
            mLStockProperty.TOWIDTH = Val.Val(txtToWidth.Text);

            mLStockProperty.FROMHEIGHT = Val.Val(txtFromHeight.Text);
            mLStockProperty.TOHEIGHT = Val.Val(txtToHeight.Text);

            mLStockProperty.FROMTABLEPER = Val.Val(txtFromTablePer.Text);
            mLStockProperty.TOTABLEPER = Val.Val(txtToTablePer.Text);

            mLStockProperty.FROMDEPTHPER = Val.Val(txtFromDepthPer.Text);
            mLStockProperty.TODEPTHPER = Val.Val(txtToDepthPer.Text);

            if (RbtStoneNo.Checked == true)
            {
                mLStockProperty.STOCKNO = txtStoneCertiMFGMemo.Text;
                mLStockProperty.LABREPORTNO = string.Empty;
                mLStockProperty.SERIALNO = string.Empty;
                mLStockProperty.MEMONO = string.Empty;
            }
            else if (RbtCertiNo.Checked == true)
            {
                mLStockProperty.STOCKNO = string.Empty;
                mLStockProperty.LABREPORTNO = txtStoneCertiMFGMemo.Text;
                mLStockProperty.SERIALNO = string.Empty;
                mLStockProperty.MEMONO = string.Empty;
            }
            else if (RbtSerialNo.Checked == true)
            {
                mLStockProperty.STOCKNO = string.Empty;
                mLStockProperty.LABREPORTNO = string.Empty;
                mLStockProperty.SERIALNO = txtStoneCertiMFGMemo.Text;
                mLStockProperty.MEMONO = string.Empty;
            }
            else if (RbtMemoNo.Checked == true)
            {
                mLStockProperty.STOCKNO = string.Empty;
                mLStockProperty.LABREPORTNO = string.Empty;
                mLStockProperty.SERIALNO = string.Empty;
                mLStockProperty.MEMONO = txtStoneCertiMFGMemo.Text;
            }

        }

        private string ExportExcelHeader(string pStrHeader, ExcelWorksheet worksheet, int pCol)
        {
            if (pStrHeader.ToLower() == "sr")
            {
                worksheet.Column(pCol).Width = 5;
                return "Sr";
            }
            if (pStrHeader.ToLower() == "srdno")
            {
                worksheet.Column(pCol).Width = 8;
                return "SRD No";
            }
            if (pStrHeader.ToLower() == "stockno")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "#Stock";
            }
            if (pStrHeader.ToLower() == "status")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Status";
            }
            if (pStrHeader.ToLower() == "shape")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Shape";
            }
            if (pStrHeader.ToLower() == "carat")
            {
                worksheet.Column(pCol).Width = 7;
                return "Cts";
            }
            if (pStrHeader.ToLower() == "clarity")
            {
                worksheet.Column(pCol).Width = 5;
                return "Cla";
            }
            if (pStrHeader.ToLower() == "color")
            {
                worksheet.Column(pCol).Width = 6;
                return "Col";
            }
            if (pStrHeader.ToLower() == "colorshade")
            {
                worksheet.Column(pCol).Width = 5;
                return "CS";
            }
            if (pStrHeader.ToLower() == "raprate")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Rap Rate";
            }
            if (pStrHeader.ToLower() == "rapvalue")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Rap Value";
            }
            if (pStrHeader.ToLower() == "rapper")
            {
                worksheet.Column(pCol).Width = 6.5;
                return "Rap %";
            }
            if (pStrHeader.ToLower() == "pricepercarat")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Pr/Ct";
            }
            if (pStrHeader.ToLower() == "amount")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Amount";
            }
            if (pStrHeader.ToLower() == "costraprate")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Cost Rap";
            }
            if (pStrHeader.ToLower() == "costrapvalue")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Cost RapValue";
            }
            if (pStrHeader.ToLower() == "costrapper")
            {
                worksheet.Column(pCol).Width = 6.5;
                return "Cost Rap%";
            }
            if (pStrHeader.ToLower() == "costpricepercarat")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Cost Pr/Ct";
            }
            if (pStrHeader.ToLower() == "costamount")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Cost Amt";
            }
            //if (pStrHeader.ToLower() == "expraprate")
            //{
            //    worksheet.Column(pCol).Width = 8.5;
            //    return "Exp Rap Rate";
            //}
            if (pStrHeader.ToLower() == "exprapvalue")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Exp Rap Value";
            }
            if (pStrHeader.ToLower() == "exprapper")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Exp Rap %";
            }
            if (pStrHeader.ToLower() == "exppricepercarat")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Exp Pr/Ct";
            }
            if (pStrHeader.ToLower() == "expamount")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Exp Amt";
            }
            //if (pStrHeader.ToLower() == "rapraprate")
            //{
            //    worksheet.Column(pCol).Width = 8.5;
            //    return "Rap Rate";
            //}
            if (pStrHeader.ToLower() == "rapnetrapvalue")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Rap Value";
            }
            if (pStrHeader.ToLower() == "rapnetrapper")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Rap %";
            }
            if (pStrHeader.ToLower() == "rapnetpricepercarat")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Rap Pr/Ct";
            }
            if (pStrHeader.ToLower() == "rapnetamount")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Rap Amt";
            }


            if (pStrHeader.ToLower() == "invoicerapvalue")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Sale RapValue";
            }
            if (pStrHeader.ToLower() == "invoicerapper")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Sale Rap%";
            }
            if (pStrHeader.ToLower() == "invoicepricepercarat")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Sale Pr/Ct";
            }
            if (pStrHeader.ToLower() == "invoiceamount")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Sale Amt";
            }


            if (pStrHeader.ToLower() == "size")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Size";
            }
            if (pStrHeader.ToLower() == "cut")
            {
                worksheet.Column(pCol).Width = 4.5;
                return "Cut";
            }
            if (pStrHeader.ToLower() == "pol")
            {
                worksheet.Column(pCol).Width = 4.5;
                return "Pol";
            }
            if (pStrHeader.ToLower() == "sym")
            {
                worksheet.Column(pCol).Width = 4.5;
                return "Sym";
            }
            if (pStrHeader.ToLower() == "fl")
            {
                worksheet.Column(pCol).Width = 4.5;
                return "FL";
            }
            if (pStrHeader.ToLower() == "flshade")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "FlShd";
            }
            if (pStrHeader.ToLower() == "depthper")
            {
                worksheet.Column(pCol).Width = 5.5;
                return "TD%";
            }
            if (pStrHeader.ToLower() == "tableper")
            {
                worksheet.Column(pCol).Width = 5.5;
                return "Tab%";
            }
            if (pStrHeader.ToLower() == "length")
            {
                worksheet.Column(pCol).Width = 5;
                return "L";
            }
            if (pStrHeader.ToLower() == "width")
            {
                worksheet.Column(pCol).Width = 5;
                return "W";
            }
            if (pStrHeader.ToLower() == "height")
            {
                worksheet.Column(pCol).Width = 5;
                return "H";
            }
            if (pStrHeader.ToLower() == "measurement")
            {
                worksheet.Column(pCol).Width = 10;
                return "Measurement";
            }
            if (pStrHeader.ToLower() == "lab")
            {
                worksheet.Column(pCol).Width = 5;
                return "Lab";
            }
            if (pStrHeader.ToLower() == "location")
            {
                worksheet.Column(pCol).Width = 10;
                return "Loc";
            }
            if (pStrHeader.ToLower() == "certno")
            {
                worksheet.Column(pCol).Width = 12;
                return "Cert No";
            }
            if (pStrHeader.ToLower() == "videourl")
            {
                worksheet.Column(pCol).Width = 12;
                return "Video URL";
            }
            if (pStrHeader.ToLower() == "blacktable")
            {
                worksheet.Column(pCol).Width = 5;
                return "BT";
            }
            if (pStrHeader.ToLower() == "blackcrown")
            {
                worksheet.Column(pCol).Width = 5;
                return "BC";
            }
            if (pStrHeader.ToLower() == "whitetable")
            {
                worksheet.Column(pCol).Width = 5;
                return "WT";
            }
            if (pStrHeader.ToLower() == "whitecrown")
            {
                worksheet.Column(pCol).Width = 5;
                return "WC";
            }
            if (pStrHeader.ToLower() == "tableopen")
            {
                worksheet.Column(pCol).Width = 5;
                return "TO";
            }
            if (pStrHeader.ToLower() == "crownopen")
            {
                worksheet.Column(pCol).Width = 5;
                return "CO";
            }
            if (pStrHeader.ToLower() == "pavillionopen")
            {
                worksheet.Column(pCol).Width = 5;
                return "PO";
            }
            if (pStrHeader.ToLower() == "milky")
            {
                worksheet.Column(pCol).Width = 5;
                return "Milky";
            }
            if (pStrHeader.ToLower() == "luster")
            {
                worksheet.Column(pCol).Width = 5;
                return "Luster";
            }
            if (pStrHeader.ToLower() == "ec")
            {
                worksheet.Column(pCol).Width = 5;
                return "EC";
            }
            if (pStrHeader.ToLower() == "ha")
            {
                worksheet.Column(pCol).Width = 5;
                return "HA";
            }
            if (pStrHeader.ToLower() == "ratio")
            {
                worksheet.Column(pCol).Width = 5;
                return "Ratio";
            }
            if (pStrHeader.ToLower() == "girdper")
            {
                worksheet.Column(pCol).Width = 6;
                return "Girdle%";
            }
            if (pStrHeader.ToLower() == "pavang")
            {
                worksheet.Column(pCol).Width = 6;
                return "PVAng";
            }
            if (pStrHeader.ToLower() == "pavheight")
            {
                worksheet.Column(pCol).Width = 6;
                return "PVHgt";
            }
            if (pStrHeader.ToLower() == "crang")
            {
                worksheet.Column(pCol).Width = 6;
                return "CRAng";
            }
            if (pStrHeader.ToLower() == "crheight")
            {
                worksheet.Column(pCol).Width = 6;
                return "CRHgt";
            }
            if (pStrHeader.ToLower() == "girddesc")
            {
                worksheet.Column(pCol).Width = 30;
                return "Girdle Desc";
            }
            if (pStrHeader.ToLower() == "girdcond")
            {
                worksheet.Column(pCol).Width = 10;
                return "Girdle";
            }
            if (pStrHeader.ToLower() == "keytosymbol")
            {
                worksheet.Column(pCol).Width = 50;
                return "Key To Sym";
            }
            if (pStrHeader.ToLower() == "comment")
            {
                worksheet.Column(pCol).Width = 50;
                return "Comment";
            }
            if (pStrHeader.ToLower() == "fancycolordescription")
            {
                worksheet.Column(pCol).Width = 50;
                return "Fancy Color Description";
            }
            if (pStrHeader.ToLower() == "imageurl")
            {
                worksheet.Column(pCol).Width = 50;
                worksheet.Column(pCol).Hidden = true;
                return "Image URL";
            }
            if (pStrHeader.ToLower() == "certurl")
            {
                worksheet.Column(pCol).Width = 50;
                worksheet.Column(pCol).Hidden = true;
                return "Cert URL";
            }
            if (pStrHeader.ToLower() == "videourl")
            {
                worksheet.Column(pCol).Width = 50;
                worksheet.Column(pCol).Hidden = true;
                return "Video URL";
            }
            if (pStrHeader.ToLower() == "dnapageurl")
            {
                worksheet.Column(pCol).Width = 50;
                worksheet.Column(pCol).Hidden = true;
                return "DNA Page URL";
            }

            if (pStrHeader.ToLower() == "colgroup")
            {
                worksheet.Column(pCol).Width = 5;
                worksheet.Column(pCol).Hidden = true;
                return "ColGroup";
            }
            if (pStrHeader.ToLower() == "clagroup")
            {
                worksheet.Column(pCol).Width = 5;
                worksheet.Column(pCol).Hidden = true;
                return "ClaGroup";
            }
            if (pStrHeader.ToLower() == "cutgroup")
            {
                worksheet.Column(pCol).Width = 5;
                worksheet.Column(pCol).Hidden = true;
                return "CutGroup";
            }
            if (pStrHeader.ToLower() == "polgroup")
            {
                worksheet.Column(pCol).Width = 5;
                worksheet.Column(pCol).Hidden = true;
                return "PolGroup";
            }
            if (pStrHeader.ToLower() == "symgroup")
            {
                worksheet.Column(pCol).Width = 5;
                worksheet.Column(pCol).Hidden = true;
                return "SymGroup";
            }
            if (pStrHeader.ToLower() == "flgroup")
            {
                worksheet.Column(pCol).Width = 5;
                worksheet.Column(pCol).Hidden = true;
                return "FLGroup";
            }
            return "";
        }

        public void AddProportionDetail(ExcelWorksheet worksheet, DataTable pDtabGroup, string SheetName, int Row, int Column,
           string pStrHeader, string pStrTitle,
           string pStrGroupColumn,
           string StrStartRow,
           string StrEndRow,
           DataTable pDtabDetail
           )
        {
            Color BackColor = Color.FromArgb(40, 56, 145);
            Color FontColor = Color.White;
            string FontName = "Calibri";
            float FontSize = 9;

            int StartRow = Row;
            int StartColumn = Column;

            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Value = pStrHeader;
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Merge = true;
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Font.Name = FontName;
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Font.Size = 12;

            StartRow = StartRow + 1;
            worksheet.Cells[StartRow, Column, StartRow, Column].Value = pStrTitle;
            worksheet.Cells[StartRow, Column + 1, StartRow, Column + 1].Value = "Pcs";
            worksheet.Cells[StartRow, Column + 2, StartRow, Column + 2].Value = "Carat";
            worksheet.Cells[StartRow, Column + 3, StartRow, Column + 3].Value = "Rap %";
            worksheet.Cells[StartRow, Column + 4, StartRow, Column + 4].Value = "Amount";
            worksheet.Cells[StartRow, Column + 5, StartRow, Column + 5].Value = "Rap Value";
            worksheet.Cells[StartRow, Column + 6, StartRow, Column + 6].Value = "%";

            StartRow = StartRow + 1;

            int IntSizeStartRow = StartRow;
            int IntSizeEndRow = StartRow + pDtabGroup.Rows.Count - 1;
            int IntSizeStartColumn = Row;
            int IntSizeEndColumn = Column + 6;

            string GroupCol = Global.ColumnIndexToColumnLetter(pDtabDetail.Columns[pStrGroupColumn].Ordinal + 1);
            string CaratCol = Global.ColumnIndexToColumnLetter(pDtabDetail.Columns["Carat"].Ordinal + 1);
            string AmountCol = Global.ColumnIndexToColumnLetter(pDtabDetail.Columns["Amount"].Ordinal + 1);
            string RapAmountCol = Global.ColumnIndexToColumnLetter(pDtabDetail.Columns["RapValue"].Ordinal + 1);

            string FormulaCol = "'" + SheetName + "'!$" + GroupCol + "$" + StrStartRow + ":$" + GroupCol + "$" + StrEndRow + "";
            string FormulaCaratCol = "'" + SheetName + "'!$" + CaratCol + "$" + StrStartRow + ":$" + CaratCol + "$" + StrEndRow + "";
            string FormulaAmountCol = "'" + SheetName + "'!$" + AmountCol + "$" + StrStartRow + ":$" + AmountCol + "$" + StrEndRow + "";
            string FormulaRapAmountCol = "'" + SheetName + "'!$" + RapAmountCol + "$" + StrStartRow + ":$" + RapAmountCol + "$" + StrEndRow + "";

            string SumGrpCol = Global.ColumnIndexToColumnLetter(Column);
            string SumPcsCol = Global.ColumnIndexToColumnLetter(Column + 1);
            string SumCaratCol = Global.ColumnIndexToColumnLetter(Column + 2);
            string SumRapPerCol = Global.ColumnIndexToColumnLetter(Column + 3);
            string SumAmountCol = Global.ColumnIndexToColumnLetter(Column + 4);
            string SumRapAmountCol = Global.ColumnIndexToColumnLetter(Column + 5);
            string SumPerCol = Global.ColumnIndexToColumnLetter(Column + 6);

            foreach (DataRow DRow in pDtabGroup.Rows)
            {
                worksheet.Cells[StartRow, Column, StartRow, Column].Value = Val.ToString(DRow[0]);

                //PCS
                worksheet.Cells[StartRow, Column + 1, StartRow, Column + 1].Formula = "SUMPRODUCT(SUBTOTAL(3,OFFSET(" + FormulaCol + ",ROW(" + FormulaCol + ")-MIN(ROW(" + FormulaCol + ")),,1)),--(" + FormulaCol + "=" + SumGrpCol + "" + StartRow + "))";
                worksheet.Cells[StartRow, Column + 2, StartRow, Column + 2].Formula = "SUMPRODUCT(SUBTOTAL(3,OFFSET(" + FormulaCol + ",ROW(" + FormulaCol + ")-MIN(ROW(" + FormulaCol + ")),,1)),--(" + FormulaCol + "=" + SumGrpCol + "" + StartRow + "),(" + FormulaCaratCol + "))";
                //Rap %
                worksheet.Cells[StartRow, Column + 3, StartRow, Column + 3].Formula = "IF(" + SumRapAmountCol + "" + StartRow + ">0,ROUND(SUM(((" + SumAmountCol + "" + StartRow + ")/((" + SumRapAmountCol + "" + StartRow + "*1)))*100),2)-100,)";

                // Amount
                worksheet.Cells[StartRow, Column + 4, StartRow, Column + 4].Formula = "SUMPRODUCT(SUBTOTAL(3,OFFSET(" + FormulaCol + ",ROW(" + FormulaCol + ")-MIN(ROW(" + FormulaCol + ")),,1)),--(" + FormulaCol + "=" + SumGrpCol + "" + StartRow + "),(" + FormulaAmountCol + "))";

                // Rap
                worksheet.Cells[StartRow, Column + 5, StartRow, Column + 5].Formula = "SUMPRODUCT(SUBTOTAL(3,OFFSET(" + FormulaCol + ",ROW(" + FormulaCol + ")-MIN(ROW(" + FormulaCol + ")),,1)),--(" + FormulaCol + "=" + SumGrpCol + "" + StartRow + "),(" + FormulaRapAmountCol + "))";

                //Per
                worksheet.Cells[StartRow, Column + 6, StartRow, Column + 6].Formula = "" + SumPcsCol + "" + StartRow + "/$" + SumPcsCol + "$" + (Val.ToInt(IntSizeStartRow) + pDtabGroup.Rows.Count) + "";
                worksheet.Cells[StartRow, Column + 6, StartRow, Column + 6].Style.Numberformat.Format = "0.00%";

                StartRow = StartRow + 1;
            }

            // Rap Amount Column
            worksheet.Column(Column + 5).OutlineLevel = 1;
            worksheet.Column(Column + 5).Collapsed = true;

            worksheet.Cells[StartRow, Column, StartRow, Column].Value = "Total";
            worksheet.Cells[StartRow, Column + 1, StartRow, Column + 1].Formula = "SUM(" + SumPcsCol + "" + IntSizeStartRow + ":" + SumPcsCol + "" + IntSizeEndRow + ")";
            worksheet.Cells[StartRow, Column + 2, StartRow, Column + 2].Formula = "SUM(" + SumCaratCol + "" + IntSizeStartRow + ":" + SumCaratCol + "" + IntSizeEndRow + ")";
            worksheet.Cells[StartRow, Column + 3, StartRow, Column + 3].Formula = "=IF(" + SumRapAmountCol + "" + StartRow + ">0,ROUND(SUM(((" + SumAmountCol + "" + StartRow + ")/((" + SumRapAmountCol + "" + StartRow + "*1)))*100),2)-100,)";
            worksheet.Cells[StartRow, Column + 4, StartRow, Column + 4].Formula = "SUM(" + SumAmountCol + "" + IntSizeStartRow + ":" + SumAmountCol + "" + IntSizeEndRow + ")";
            worksheet.Cells[StartRow, Column + 5, StartRow, Column + 5].Formula = "SUM(" + SumRapAmountCol + "" + IntSizeStartRow + ":" + SumRapAmountCol + "" + IntSizeEndRow + ")";
            worksheet.Cells[StartRow, Column + 6, StartRow, Column + 6].Formula = "SUM(" + SumPerCol + "" + IntSizeStartRow + ":" + SumPerCol + "" + IntSizeEndRow + ")";
            worksheet.Cells[StartRow, Column + 6, StartRow, Column + 6].Style.Numberformat.Format = "0.00%";


            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Cells[Row + 2, Column + 1, StartRow, Column + 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.Font.Name = FontName;
            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.Font.Size = FontSize;

            //Header
            worksheet.Cells[Row, Column, Row + 1, Column + 6].Style.Font.Bold = true;
            worksheet.Cells[Row, Column, Row + 1, Column + 6].Style.Font.Color.SetColor(FontColor);
            worksheet.Cells[Row, Column, Row + 1, Column + 6].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[Row, Column, Row + 1, Column + 6].Style.Fill.PatternColor.SetColor(BackColor);
            worksheet.Cells[Row, Column, Row + 1, Column + 6].Style.Fill.BackgroundColor.SetColor(BackColor);

            // Footer
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Font.Bold = true;
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Font.Color.SetColor(FontColor);
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Fill.PatternColor.SetColor(BackColor);
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Fill.BackgroundColor.SetColor(BackColor);

            //Left First Column
            worksheet.Cells[Row, Column, StartRow, Column].Style.Font.Bold = true;
            worksheet.Cells[Row, Column, StartRow, Column].Style.Font.Color.SetColor(FontColor);
            worksheet.Cells[Row, Column, StartRow, Column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[Row, Column, StartRow, Column].Style.Fill.PatternColor.SetColor(BackColor);
            worksheet.Cells[Row, Column, StartRow, Column].Style.Fill.BackgroundColor.SetColor(BackColor);

        }

        public void AddInclusionDetail(ExcelWorksheet worksheet, DataTable pDtab)
        {
            Color BackColor = Color.FromArgb(40, 56, 145);
            Color FontColor = Color.White;
            string FontName = "Calibri";
            float FontSize = 9;


            worksheet.Cells[2, 3, 4, 13].Value = "SRD Inclusion Grading";
            worksheet.Cells[2, 3, 4, 13].Style.Font.Name = FontName;
            worksheet.Cells[2, 3, 4, 13].Style.Font.Size = 20;
            worksheet.Cells[2, 3, 4, 13].Style.Font.Bold = true;

            worksheet.Cells[2, 3, 4, 13].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            worksheet.Cells[2, 3, 4, 13].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            worksheet.Cells[2, 3, 4, 13].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            worksheet.Cells[2, 3, 4, 13].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            worksheet.Cells[2, 3, 4, 13].Merge = true;
            worksheet.Cells[2, 3, 4, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 3, 4, 13].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;


            worksheet.Cells[2, 3, 4, 13].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[2, 3, 4, 13].Style.Fill.PatternColor.SetColor(BackColor);
            worksheet.Cells[2, 3, 4, 13].Style.Fill.BackgroundColor.SetColor(BackColor);
            worksheet.Cells[2, 3, 4, 13].Style.Font.Color.SetColor(FontColor);

            DataTable DTabDistinct = pDtab.DefaultView.ToTable(true, "PARATYPE_ID", "PARATYPECODE", "PARATYPENAME");
            DTabDistinct.DefaultView.Sort = "PARATYPE_ID";
            DTabDistinct = DTabDistinct.DefaultView.ToTable();

            int StartRow = 0;
            int StartColumn = 3;
            int IntRow = 0;

            for (int i = 0; i < DTabDistinct.Rows.Count; i++)
            {
                string Str = Val.ToString(DTabDistinct.Rows[i]["PARATYPECODE"]);
                string StrName = Val.ToString(DTabDistinct.Rows[i]["PARATYPENAME"]);

                DataTable DTab = pDtab.Select("PARATYPECODE='" + Str + "'").CopyToDataTable();

                if (i % 4 == 0)
                {
                    StartColumn = 3;
                    StartRow = IntRow + (i % 4) + 6;
                    IntRow = StartRow;
                }
                else
                {
                    StartRow = IntRow;
                }

                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Value = StrName + " (" + Str + ")";
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Merge = true;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Fill.PatternColor.SetColor(BackColor);
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Fill.BackgroundColor.SetColor(BackColor);
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Color.SetColor(FontColor);
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Name = FontName;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Size = 11;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Bold = true;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                StartRow = StartRow + 1;
                for (int J = 0; J < DTab.Rows.Count; J++)
                {
                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Value = Val.ToString(DTab.Rows[J]["CODE"]);
                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Style.Font.Bold = true;
                    worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = Val.ToString(DTab.Rows[J]["NAME"]);
                    StartRow = StartRow + 1;

                }
                worksheet.Column(StartColumn + 1).Width = 20;

                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Name = FontName;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Size = 11;

                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Name = FontName;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Size = FontSize;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                StartColumn = StartColumn + 3;

            }
            worksheet.Cells[1, 1, 50, 50].AutoFitColumns();
        }

        public string ExportExcel()
        {
            try
            {

                string FormatName = "GENERAL";

                Fill();

                this.Cursor = Cursors.WaitCursor;
                DataSet DS = ObjStock.GetDataForExcelExportEmailSettingNew(mCRMStockProperty);
                this.Cursor = Cursors.Default;

                DTabDetail = DS.Tables[0];
                DataTable DTabSize = DS.Tables[1];
                DataTable DTabShape = DS.Tables[2];
                DataTable DTabClarity = DS.Tables[3];
                DataTable DTabColor = DS.Tables[4];
                DataTable DTabCut = DS.Tables[5];
                DataTable DTabPolish = DS.Tables[6];
                DataTable DTabSym = DS.Tables[7];
                DataTable DTabFL = DS.Tables[8];
                DataTable DTabInclusion = DS.Tables[9];


                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }

                DTabDetail.DefaultView.Sort = "SR";
                DTabDetail = DTabDetail.DefaultView.ToTable();

                DTabSize.DefaultView.Sort = "FromCarat";
                DTabSize = DTabSize.DefaultView.ToTable();

                DTabShape.DefaultView.Sort = "SequenceNo";
                DTabShape = DTabShape.DefaultView.ToTable();

                DTabColor.DefaultView.Sort = "SequenceNo";
                DTabColor = DTabColor.DefaultView.ToTable();

                DTabClarity.DefaultView.Sort = "SequenceNo";
                DTabClarity = DTabClarity.DefaultView.ToTable();

                DTabCut.DefaultView.Sort = "SequenceNo";
                DTabCut = DTabCut.DefaultView.ToTable();

                DTabPolish.DefaultView.Sort = "SequenceNo";
                DTabPolish = DTabPolish.DefaultView.ToTable();

                DTabSym.DefaultView.Sort = "SequenceNo";
                DTabSym = DTabSym.DefaultView.ToTable();

                DTabFL.DefaultView.Sort = "SequenceNo";
                DTabFL = DTabFL.DefaultView.ToTable();

                this.Cursor = Cursors.WaitCursor;

                string StrFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.FromArgb(40, 56, 145);
                Color FontColor = Color.White;
                string FontName = "Calibri";
                float FontSize = 9;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Demand_" + DateTime.Now.ToString("ddMMyyyy"));
                    ExcelWorksheet worksheetProportion = xlPackage.Workbook.Worksheets.Add("Proportion");
                    ExcelWorksheet worksheetInclusion = xlPackage.Workbook.Worksheets.Add("Inclusion Detail");


                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    #region Add Image

                    Image img = Image.FromFile(Application.StartupPath + "//logo.jpg");
                    OfficeOpenXml.Drawing.ExcelPicture pic = worksheet.Drawings.AddPicture("Logo", img);
                    pic.SetPosition(2, 23);
                    pic.SetSize(100, 55);

                    worksheet.Cells[1, 1, 3, 3].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                    worksheet.Cells[1, 1, 3, 3].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                    worksheet.Cells[1, 1, 3, 3].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                    worksheet.Cells[1, 1, 3, 3].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                    worksheet.Cells[1, 1, 3, 3].Merge = true;

                    #endregion

                    #region Stock Detail

                    StartRow = 5;
                    EndRow = StartRow + DTabDetail.Rows.Count;
                    StartColumn = 1;
                    EndColumn = DTabDetail.Columns.Count;

                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].LoadFromDataTable(DTabDetail, true);
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Name = FontName;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Size = FontSize;

                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Font.Bold = true;
                    //   worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].AutoFilter = true;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(FontColor);

                    //#P : 06-08-2020
                    if (FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale")
                    {
                        worksheet.Cells[5, 15, 5, 18].Style.Font.Color.SetColor(Color.Red);
                    }

                    if (FormatName == "With Rapnet")
                    {
                        worksheet.Cells[5, 19, 5, 22].Style.Font.Color.SetColor(FontColor);
                    }
                    else if (FormatName == "With Sale")
                    {
                        worksheet.Cells[5, 19, 5, 22].Style.Font.Color.SetColor(Color.FromArgb(174, 201, 121));
                    }
                    //End : #P : 06-08-2020

                    worksheet.View.FreezePanes(6, 1);

                    // Set Hyperlink
                    int IntCertColumn = DTabDetail.Columns["CertNo"].Ordinal;
                    int IntVideoUrlColumn = DTabDetail.Columns["VideoUrl"].Ordinal;
                    for (int IntI = 6; IntI <= EndRow; IntI++)
                    {


                        if (!Val.ToString(DTabDetail.Rows[IntI - 6]["DNAPAGEURL"]).Trim().Equals(string.Empty))
                        {
                            worksheet.Cells[IntI, 2, IntI, 2].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["DNAPAGEURL"]));
                            worksheet.Cells[IntI, 2, IntI, 2].Style.Font.Name = FontName;
                            worksheet.Cells[IntI, 2, IntI, 2].Style.Font.Bold = true;
                            worksheet.Cells[IntI, 2, IntI, 2].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                            worksheet.Cells[IntI, 2, IntI, 2].Style.Font.UnderLine = true;

                            worksheet.Cells[IntI, 3, IntI, 3].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["DNAPAGEURL"]));
                            worksheet.Cells[IntI, 3, IntI, 3].Style.Font.Name = FontName;
                            worksheet.Cells[IntI, 3, IntI, 3].Style.Font.Bold = true;
                            worksheet.Cells[IntI, 3, IntI, 3].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                            worksheet.Cells[IntI, 3, IntI, 3].Style.Font.UnderLine = true;
                        }

                        //#P :  03-09-2020
                        //worksheet.Cells[IntI, 28, IntI, 28].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["CERTURL"]));
                        //worksheet.Cells[IntI, 28, IntI, 28].Style.Font.Name = FontName;
                        //worksheet.Cells[IntI, 28, IntI, 28].Style.Font.Bold = true;
                        //worksheet.Cells[IntI, 28, IntI, 28].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                        //worksheet.Cells[IntI, 28, IntI, 28].Style.Font.UnderLine = true;


                        if (!Val.ToString(DTabDetail.Rows[IntI - 6]["CERTURL"]).Trim().Equals(string.Empty))
                        {
                            worksheet.Cells[IntI, IntCertColumn + 1].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["CERTURL"]));
                            worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.Name = FontName;
                            worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.Bold = true;
                            worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                            worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.UnderLine = true;
                        }

                        if (!Val.ToString(DTabDetail.Rows[IntI - 6]["VIDEOURL"]).Trim().Equals(string.Empty))
                        {
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["VIDEOURL"]));
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Style.Font.Name = FontName;
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Style.Font.Bold = true;
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Style.Font.UnderLine = true;
                        }
                        //End : #P :  03-09-2020

                        //#P : 06-08-2020
                        if (FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale")
                        {
                            worksheet.Cells[IntI, 15, IntI, 18].Style.Font.Color.SetColor(Color.Red);
                        }
                        if (FormatName == "With Rapnet")
                        {
                            worksheet.Cells[IntI, 19, IntI, 22].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                        }
                        else if (FormatName == "With Sale")
                        {
                            worksheet.Cells[IntI, 19, IntI, 22].Style.Font.Color.SetColor(Color.FromArgb(0, 150, 68));
                        }
                        //End : #P : 06-08-2020

                    }

                    // Header Set
                    for (int i = 1; i <= DTabDetail.Columns.Count; i++)
                    {
                        string StrHeader = ExportExcelHeader(Val.ToString(worksheet.Cells[5, i].Value), worksheet, i);
                        worksheet.Cells[5, i].Value = StrHeader;
                    }

                    int IntRowStartsFrom = 3;
                    int IntRowEndTo = (DTabDetail.Rows.Count - 1 + IntRowStartsFrom);

                    //CHECK COLUMN EXISTS IN DATATABLE..
                    #region :: Check Column Exists In Datatable ::
                    int SrNo = 0, CaratNo = 0, AmountNo = 0, RapAmountNo = 0, SizeNo = 0, ShapeNo = 0, ColorNo = 0, ClarityNo = 0, CutNo = 0, PolNo = 0, SymNo = 0, FLNo = 0,
                        ExpAmountNo = 0, ExpRapAmountNo = 0, RapnetAmountNo = 0, RapnetRapAmountNo = 0, InvoiceAmountNo = 0, InvoiceRapAmountNo = 0;


                    DataColumnCollection columns = DTabDetail.Columns;

                    if (columns.Contains("SR"))
                        SrNo = DTabDetail.Columns["SR"].Ordinal + 1;
                    if (columns.Contains("Size"))
                        SizeNo = DTabDetail.Columns["Size"].Ordinal + 1;
                    if (columns.Contains("Carat"))
                        CaratNo = DTabDetail.Columns["Carat"].Ordinal + 1;
                    if (columns.Contains("RapValue"))
                        RapAmountNo = DTabDetail.Columns["RapValue"].Ordinal + 1;
                    if (columns.Contains("Amount"))
                        AmountNo = DTabDetail.Columns["Amount"].Ordinal + 1;

                    //#P : 06-08-2020
                    if ((FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale") && columns.Contains("ExpRapValue"))
                        ExpRapAmountNo = DTabDetail.Columns["ExpRapValue"].Ordinal + 1;
                    if ((FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale") && columns.Contains("ExpAmount"))
                        ExpAmountNo = DTabDetail.Columns["ExpAmount"].Ordinal + 1;
                    if (FormatName == "With Rapnet" && columns.Contains("RapnetRapValue"))
                        RapnetRapAmountNo = DTabDetail.Columns["RapnetRapValue"].Ordinal + 1;
                    if (FormatName == "With Rapnet" && columns.Contains("RapnetAmount"))
                        RapnetAmountNo = DTabDetail.Columns["RapnetAmount"].Ordinal + 1;

                    if (FormatName == "With Sale" && columns.Contains("InvoiceRapValue"))
                        InvoiceRapAmountNo = DTabDetail.Columns["InvoiceRapValue"].Ordinal + 1;
                    if (FormatName == "With Sale" && columns.Contains("InvoiceAmount"))
                        InvoiceAmountNo = DTabDetail.Columns["InvoiceAmount"].Ordinal + 1;

                    //End : #P : 06-08-2020

                    if (columns.Contains("Shape"))
                        ShapeNo = DTabDetail.Columns["Shape"].Ordinal + 1;
                    if (columns.Contains("Color"))
                        ColorNo = DTabDetail.Columns["Color"].Ordinal + 1;
                    if (columns.Contains("Clarity"))
                        ClarityNo = DTabDetail.Columns["Clarity"].Ordinal + 1;
                    if (columns.Contains("Cut"))
                        CutNo = DTabDetail.Columns["Cut"].Ordinal + 1;
                    if (columns.Contains("Pol"))
                        PolNo = DTabDetail.Columns["Pol"].Ordinal + 1;
                    if (columns.Contains("Sym"))
                        SymNo = DTabDetail.Columns["Sym"].Ordinal + 1;
                    if (columns.Contains("FL"))
                        FLNo = DTabDetail.Columns["FL"].Ordinal + 1;

                    #endregion

                    string StrStartRow = "6";
                    string StrEndRow = EndRow.ToString();

                    #region Top Formula

                    worksheet.Cells[1, 5, 1, 5].Value = "Pcs";
                    worksheet.Cells[1, 6, 1, 6].Value = "Carat";
                    worksheet.Cells[1, 11, 1, 11].Value = "Rap Value";
                    worksheet.Cells[1, 12, 1, 12].Value = "Rap %";
                    worksheet.Cells[1, 13, 1, 13].Value = "Pr/Ct";
                    worksheet.Cells[1, 14, 1, 14].Value = "Amount";

                    //#P : 06-08-2020
                    if (FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale")
                    {
                        worksheet.Cells[1, 15, 1, 15].Value = "Exp RapValue";
                        worksheet.Cells[1, 16, 1, 16].Value = "Exp Rap%";
                        worksheet.Cells[1, 17, 1, 17].Value = "Exp Pr/Ct";
                        worksheet.Cells[1, 18, 1, 18].Value = "Exp Amount";
                    }
                    if (FormatName == "With Rapnet")
                    {
                        worksheet.Cells[1, 19, 1, 19].Value = "Rapnet RapValue";
                        worksheet.Cells[1, 20, 1, 20].Value = "Rapnet Rap%";
                        worksheet.Cells[1, 21, 1, 21].Value = "Rapnet Pr/Ct";
                        worksheet.Cells[1, 22, 1, 22].Value = "Rapnet Amount";
                    }
                    if (FormatName == "With Sale")
                    {
                        worksheet.Cells[1, 19, 1, 19].Value = "Sale RapValue";
                        worksheet.Cells[1, 20, 1, 20].Value = "Sale Rap%";
                        worksheet.Cells[1, 21, 1, 21].Value = "Sale Pr/Ct";
                        worksheet.Cells[1, 22, 1, 22].Value = "Sale Amount";
                    }
                    //End : #P : 06-08-2020


                    worksheet.Cells[2, 4, 2, 4].Value = "Total";
                    worksheet.Cells[3, 4, 3, 4].Value = "Selected";

                    worksheet.Cells[1, 7, 3, 10].Merge = true;
                    worksheet.Cells[1, 7, 3, 10].Value = "Note : Use filter to select stones and Check your ObjGridSelection Average Discount and Total amount.";
                    //worksheet.Cells[1, 7, 3, 9].Value = "Note : Use filter to select stones and Check your ObjGridSelection Avg Disc and Total amt.";
                    worksheet.Cells[1, 7, 3, 10].Style.WrapText = true;

                    // Total Pcs Formula
                    string S = Global.ColumnIndexToColumnLetter(SrNo) + StrStartRow;
                    string E = Global.ColumnIndexToColumnLetter(SrNo) + StrEndRow;
                    worksheet.Cells[2, 5, 2, 5].Formula = "ROUND(COUNTA(" + S + ":" + E + "),2)";
                    worksheet.Cells[3, 5, 3, 5].Formula = "ROUND(SUBTOTAL(3," + S + ":" + E + "),2)";

                    // Total Carat Formula
                    S = Global.ColumnIndexToColumnLetter(CaratNo) + StrStartRow;
                    E = Global.ColumnIndexToColumnLetter(CaratNo) + StrEndRow;
                    worksheet.Cells[2, 6, 2, 6].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                    worksheet.Cells[3, 6, 3, 6].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                    // Rap Value Formula
                    S = Global.ColumnIndexToColumnLetter(RapAmountNo) + StrStartRow;
                    E = Global.ColumnIndexToColumnLetter(RapAmountNo) + StrEndRow;
                    worksheet.Cells[2, 11, 2, 11].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                    worksheet.Cells[3, 11, 3, 11].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                    // Amount Formula
                    S = Global.ColumnIndexToColumnLetter(AmountNo) + StrStartRow;
                    E = Global.ColumnIndexToColumnLetter(AmountNo) + StrEndRow;
                    worksheet.Cells[2, 14, 2, 14].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                    worksheet.Cells[3, 14, 3, 14].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                    // Price Per Carat Formula
                    worksheet.Cells[2, 13, 2, 13].Formula = "ROUND(N2/F2,2)";
                    worksheet.Cells[3, 13, 3, 13].Formula = "ROUND(N3/F3,2)";

                    // Discount Formula
                    S = Global.ColumnIndexToColumnLetter(AmountNo) + StrStartRow;
                    E = Global.ColumnIndexToColumnLetter(AmountNo) + StrEndRow;
                    //worksheet.Cells[2, 11, 2, 11].Formula = "ROUND(SUM(((M2)/((J2*1)))*100),2)-100";
                    //worksheet.Cells[3, 11, 3, 11].Formula = "ROUND(SUM(((M3)/((J3*1)))*100),2)-100";

                    worksheet.Cells[2, 12, 2, 12].Formula = "100-ROUND(SUM(((N2)/((K2*1)))*100),2)";
                    worksheet.Cells[3, 12, 3, 12].Formula = "100-ROUND(SUM(((N3)/((K3*1)))*100),2)";


                    #region Exp Summary Detail
                    if (FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale")
                    {
                        //Exp RapValue
                        S = Global.ColumnIndexToColumnLetter(ExpRapAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(ExpRapAmountNo) + StrEndRow;
                        worksheet.Cells[2, 15, 2, 15].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                        worksheet.Cells[3, 15, 3, 15].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                        // Exp Amount Formula
                        S = Global.ColumnIndexToColumnLetter(ExpAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(ExpAmountNo) + StrEndRow;
                        worksheet.Cells[2, 18, 2, 18].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                        worksheet.Cells[3, 18, 3, 18].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                        // Exp Price Per Carat Formula
                        worksheet.Cells[2, 17, 2, 17].Formula = "ROUND(R2/F2,2)";
                        worksheet.Cells[3, 17, 3, 17].Formula = "ROUND(R3/F3,2)";

                        // Exp Discount Formula (Rap%)
                        S = Global.ColumnIndexToColumnLetter(ExpAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(ExpAmountNo) + StrEndRow;
                        //worksheet.Cells[2, 11, 2, 11].Formula = "ROUND(SUM(((M2)/((J2*1)))*100),2)-100";
                        //worksheet.Cells[3, 11, 3, 11].Formula = "ROUND(SUM(((M3)/((J3*1)))*100),2)-100";

                        worksheet.Cells[2, 16, 2, 16].Formula = "100-ROUND(SUM(((R2)/((O2*1)))*100),2)";
                        worksheet.Cells[3, 16, 3, 16].Formula = "100-ROUND(SUM(((R3)/((O3*1)))*100),2)";
                    }
                    #endregion

                    #region Rapnet Summary Detail
                    if (FormatName == "With Rapnet")
                    {
                        //Exp RapValue
                        S = Global.ColumnIndexToColumnLetter(RapnetRapAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(RapnetRapAmountNo) + StrEndRow;
                        worksheet.Cells[2, 19, 2, 19].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                        worksheet.Cells[3, 19, 3, 19].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                        // Exp Amount Formula
                        S = Global.ColumnIndexToColumnLetter(RapnetAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(RapnetAmountNo) + StrEndRow;
                        worksheet.Cells[2, 22, 2, 22].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                        worksheet.Cells[3, 22, 3, 22].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                        // Exp Price Per Carat Formula
                        worksheet.Cells[2, 21, 2, 21].Formula = "ROUND(V2/F2,2)";
                        worksheet.Cells[3, 21, 3, 21].Formula = "ROUND(V3/F3,2)";

                        // Exp Discount Formula (Rap%)
                        S = Global.ColumnIndexToColumnLetter(RapnetAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(RapnetAmountNo) + StrEndRow;
                        //worksheet.Cells[2, 11, 2, 11].Formula = "ROUND(SUM(((M2)/((J2*1)))*100),2)-100";
                        //worksheet.Cells[3, 11, 3, 11].Formula = "ROUND(SUM(((M3)/((J3*1)))*100),2)-100";

                        worksheet.Cells[2, 20, 2, 20].Formula = "100-ROUND(SUM(((V2)/((S2*1)))*100),2)";
                        worksheet.Cells[3, 20, 3, 20].Formula = "100-ROUND(SUM(((V3)/((S3*1)))*100),2)";
                    }
                    #endregion
                    #region Invoice(Sale) Summary Detail
                    if (FormatName == "With Sale")
                    {
                        //Exp RapValue
                        S = Global.ColumnIndexToColumnLetter(InvoiceRapAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(InvoiceRapAmountNo) + StrEndRow;
                        worksheet.Cells[2, 19, 2, 19].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                        worksheet.Cells[3, 19, 3, 19].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                        // Exp Amount Formula
                        S = Global.ColumnIndexToColumnLetter(InvoiceAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(InvoiceAmountNo) + StrEndRow;
                        worksheet.Cells[2, 22, 2, 22].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                        worksheet.Cells[3, 22, 3, 22].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                        // Exp Price Per Carat Formula
                        worksheet.Cells[2, 21, 2, 21].Formula = "ROUND(V2/F2,2)";
                        worksheet.Cells[3, 21, 3, 21].Formula = "ROUND(V3/F3,2)";

                        // Exp Discount Formula (Rap%)
                        S = Global.ColumnIndexToColumnLetter(InvoiceAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(InvoiceAmountNo) + StrEndRow;
                        //worksheet.Cells[2, 11, 2, 11].Formula = "ROUND(SUM(((M2)/((J2*1)))*100),2)-100";
                        //worksheet.Cells[3, 11, 3, 11].Formula = "ROUND(SUM(((M3)/((J3*1)))*100),2)-100";

                        worksheet.Cells[2, 20, 2, 20].Formula = "100-ROUND(SUM(((V2)/((S2*1)))*100),2)";
                        worksheet.Cells[3, 20, 3, 20].Formula = "100-ROUND(SUM(((V3)/((S3*1)))*100),2)";
                    }
                    #endregion


                    if (FormatName == "With Exp") //#P : 06-08-2020
                    {
                        worksheet.Cells[1, 4, 4, 18].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[1, 4, 4, 18].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[1, 4, 4, 18].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 18].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 18].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 18].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 18].Style.Font.Name = "Calibri";
                        worksheet.Cells[1, 4, 4, 18].Style.Font.Size = 9;

                        worksheet.Cells[1, 4, 1, 18].Style.Font.Bold = true;
                        worksheet.Cells[1, 4, 1, 18].Style.Font.Color.SetColor(Color.White);
                        worksheet.Cells[1, 4, 1, 18].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 4, 1, 18].Style.Fill.PatternColor.SetColor(BackColor);
                        worksheet.Cells[1, 4, 1, 18].Style.Fill.BackgroundColor.SetColor(BackColor);

                        worksheet.Cells[1, 15, 3, 18].Style.Font.Color.SetColor(Color.Red);

                    }
                    else if (FormatName == "With Rapnet" || FormatName == "With Sale") //#P : 06-08-2020
                    {
                        worksheet.Cells[1, 4, 4, 22].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[1, 4, 4, 22].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[1, 4, 4, 22].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 22].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 22].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 22].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 22].Style.Font.Name = "Calibri";
                        worksheet.Cells[1, 4, 4, 22].Style.Font.Size = 9;

                        worksheet.Cells[1, 4, 1, 22].Style.Font.Bold = true;
                        worksheet.Cells[1, 4, 1, 22].Style.Font.Color.SetColor(Color.White);
                        worksheet.Cells[1, 4, 1, 22].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 4, 1, 22].Style.Fill.PatternColor.SetColor(BackColor);
                        worksheet.Cells[1, 4, 1, 22].Style.Fill.BackgroundColor.SetColor(BackColor);

                        worksheet.Cells[1, 15, 3, 18].Style.Font.Color.SetColor(Color.Red);

                        if (FormatName == "With Sale")
                        {
                            worksheet.Cells[1, 19, 1, 22].Style.Font.Color.SetColor(Color.FromArgb(174, 201, 121)); //Green
                            worksheet.Cells[2, 19, 3, 22].Style.Font.Color.SetColor(Color.FromArgb(0, 150, 68)); //Green
                        }
                        else
                        {
                            worksheet.Cells[1, 19, 1, 22].Style.Font.Color.SetColor(FontColor); //Blue
                            worksheet.Cells[2, 19, 3, 22].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192)); //Blue
                        }
                    }
                    else
                    {
                        worksheet.Cells[1, 4, 4, 14].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[1, 4, 4, 14].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[1, 4, 4, 14].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 14].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 14].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 14].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 14].Style.Font.Name = "Calibri";
                        worksheet.Cells[1, 4, 4, 14].Style.Font.Size = 9;

                        worksheet.Cells[1, 4, 1, 14].Style.Font.Bold = true;
                        worksheet.Cells[1, 4, 1, 14].Style.Font.Color.SetColor(Color.White);
                        worksheet.Cells[1, 4, 1, 14].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 4, 1, 14].Style.Fill.PatternColor.SetColor(BackColor);
                        worksheet.Cells[1, 4, 1, 14].Style.Fill.BackgroundColor.SetColor(BackColor);
                    }

                    worksheet.Cells[1, 4, 3, 4].Style.Font.Bold = true;
                    worksheet.Cells[1, 4, 3, 4].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, 4, 3, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, 4, 3, 4].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[1, 4, 3, 4].Style.Fill.BackgroundColor.SetColor(BackColor);

                    worksheet.Cells[1, 7, 3, 10].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, 7, 3, 10].Style.Fill.PatternColor.SetColor(Color.Gray);
                    worksheet.Cells[1, 7, 3, 10].Style.Fill.BackgroundColor.SetColor(Color.Gray);



                    if (FormatName == "With Exp") //#P : 06-08-2020
                    {
                        worksheet.Column(11).OutlineLevel = 1;//RapValue
                        worksheet.Column(11).Collapsed = true;

                        worksheet.Column(15).OutlineLevel = 1; //ExpRapValue
                        worksheet.Column(15).Collapsed = true;
                        worksheet.Column(24).OutlineLevel = 1; //FLShade
                        worksheet.Column(24).Collapsed = true;
                        //worksheet.Column(27).OutlineLevel = 1; //L (Length)
                        //worksheet.Column(27).Collapsed = true;
                        //worksheet.Column(28).OutlineLevel = 1; //W (Width)
                        //worksheet.Column(28).Collapsed = true;
                        //worksheet.Column(29).OutlineLevel = 1; //H (Height)
                        //worksheet.Column(29).Collapsed = true;
                    }
                    if (FormatName == "With Rapnet" || FormatName == "With Sale") //#P : 06-08-2020
                    {
                        worksheet.Column(11).OutlineLevel = 1;//RapValue
                        worksheet.Column(11).Collapsed = true;

                        worksheet.Column(15).OutlineLevel = 1; //ExpRapValue
                        worksheet.Column(15).Collapsed = true;

                        worksheet.Column(19).OutlineLevel = 1; //RapnetRapValue/SaleRapValue
                        worksheet.Column(19).Collapsed = true;

                        worksheet.Column(28).OutlineLevel = 1; //FLShade
                        worksheet.Column(28).Collapsed = true;
                        //worksheet.Column(31).OutlineLevel = 1; //L (Length)
                        //worksheet.Column(31).Collapsed = true;
                        //worksheet.Column(32).OutlineLevel = 1; //W (Width)
                        //worksheet.Column(32).Collapsed = true;
                        //worksheet.Column(33).OutlineLevel = 1; //H (Height)
                        //worksheet.Column(33).Collapsed = true;
                    }
                    else
                    {
                        worksheet.Column(11).OutlineLevel = 1;//RapValue
                        worksheet.Column(11).Collapsed = true;

                        worksheet.Column(20).OutlineLevel = 1;
                        worksheet.Column(20).Collapsed = true;

                        //worksheet.Column(23).OutlineLevel = 1;
                        //worksheet.Column(23).Collapsed = true;
                        //worksheet.Column(24).OutlineLevel = 1;
                        //worksheet.Column(24).Collapsed = true;
                        //worksheet.Column(25).OutlineLevel = 1;
                        //worksheet.Column(25).Collapsed = true;
                    }

                    #endregion

                    #endregion

                    #region Inclusion Detail

                    AddInclusionDetail(worksheetInclusion, DTabInclusion);

                    #endregion

                    #region Proporstion Detail

                    worksheetProportion.Cells[2, 2, 3, 17].Value = "Live Stock Proportion";
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Font.Name = FontName;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Font.Size = 20;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Font.Bold = true;

                    worksheetProportion.Cells[2, 2, 3, 17].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 17].Merge = true;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheetProportion.Cells[2, 2, 3, 17].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Font.Color.SetColor(FontColor);

                    int NewRow = 6;
                    AddProportionDetail(worksheetProportion, DTabSize, worksheet.Name, 6, 2, "SIZE WISE SUMMARY", "Size", "Size", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabShape, worksheet.Name, 6, 11, "SHAPE WISE SUMMARY", "Shape", "Shape", StrStartRow, StrEndRow, DTabDetail);

                    if (DTabSize.Rows.Count > DTabShape.Rows.Count)
                    {
                        NewRow = NewRow + DTabSize.Rows.Count + 5;
                    }
                    else
                    {
                        NewRow = NewRow + DTabShape.Rows.Count + 5;
                    }

                    AddProportionDetail(worksheetProportion, DTabClarity, worksheet.Name, NewRow, 2, "CLARITY WISE SUMMARY", "Clarity", "ClaGroup", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabColor, worksheet.Name, NewRow, 11, "COLOR WISE SUMMARY", "Color", "ColGroup", StrStartRow, StrEndRow, DTabDetail);


                    if (DTabClarity.Rows.Count > DTabColor.Rows.Count)
                    {
                        NewRow = NewRow + DTabClarity.Rows.Count + 5;
                    }
                    else
                    {
                        NewRow = NewRow + DTabColor.Rows.Count + 5;
                    }

                    AddProportionDetail(worksheetProportion, DTabCut, worksheet.Name, NewRow, 2, "CUT WISE SUMMARY", "Cut", "CutGroup", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabPolish, worksheet.Name, NewRow, 11, "POLISH WISE SUMMARY", "Pol", "PolGroup", StrStartRow, StrEndRow, DTabDetail);


                    if (DTabCut.Rows.Count > DTabPolish.Rows.Count)
                    {
                        NewRow = NewRow + DTabCut.Rows.Count + 5;
                    }
                    else
                    {
                        NewRow = NewRow + DTabPolish.Rows.Count + 5;
                    }

                    AddProportionDetail(worksheetProportion, DTabSym, worksheet.Name, NewRow, 2, "SYM WISE SUMMARY", "Sym", "SymGroup", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabFL, worksheet.Name, NewRow, 11, "FL WISE SUMMARY", "FL", "FLGroup", StrStartRow, StrEndRow, DTabDetail);

                    #endregion

                    xlPackage.Save();
                }

                this.Cursor = Cursors.Default;
                return StrFilePath;

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
            return "";
        }


        private void BtnSearch_Click(object sender, EventArgs e)
        {
            Fill();
        }

        private void BtnEmail_Click(object sender, EventArgs e)
        {

            string StrFileName = ExportExcel();

            string StrTomail = StrEmail;

            if (StrFileName == "")
            {
                if (Global.Confirm("No any attachememnt found\n\nStil you want to send email ?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }
            FrmEmailSend FrmEmailSend = new FrmEmailSend();
            FrmEmailSend.MdiParent = Global.gMainRef;
            FrmEmailSend.ShowForm(StrFileName, StrTomail);
        }

        public bool CheckDuplicate(string ColName, string ColValue, int IntRowIndex, string StrMsg)
        {
            if (Val.ToString(ColValue).Trim().Equals(string.Empty))
                return false;

            var Result = from row in DTabSize.AsEnumerable()
                         where Val.ToString(row[ColName]).ToUpper() == Val.ToString(ColValue).ToUpper() && row.Table.Rows.IndexOf(row) != IntRowIndex
                         select row;


            if (Result.Any())
            {
                Global.Message(StrMsg + " ALREADY EXISTS.");
                return true;
            }
            return false;
        }

        private void RepTxtFromCarat_Validating(object sender, CancelEventArgs e)
        {
            GrdSize.PostEditor();
            DataRow Dr = GrdSize.GetFocusedDataRow();
            if (Val.Val(GrdSize.EditingValue) != 0 || Val.Val(Dr["FROMCARAT"]) != 0)
            {
                if (CheckDuplicate("FROMCARAT", Val.ToString(GrdSize.EditingValue), GrdSize.FocusedRowHandle, "FROMCARAT"))
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    DTabSize.AcceptChanges();

                }
                if (Val.ToDecimal(Dr["TOCARAT"]) != 0)
                {
                    if (Val.ToDecimal(GrdSize.EditingValue) > Val.ToDecimal(Dr["TOCARAT"]))
                    {
                        Global.Message("From Amount must be Greter Than To To Carat");
                        e.Cancel = true;
                        return;
                    }
                }

                var dValue = from row in DTabSize.AsEnumerable()
                             where Val.Val(row["FROMCARAT"]) <= Val.Val(GrdSize.EditingValue) && Val.Val(row["TOCARAT"]) >= Val.Val(GrdSize.EditingValue) && row.Table.Rows.IndexOf(row) != GrdSize.FocusedRowHandle
                             select row;

                if (dValue.Any())
                {
                    Global.Message("This Value Already Exist Between Some From Carat and To Carat Please Check.!");
                    e.Cancel = true;
                    return;
                }
            }

        }

        private void RepTxtToCarat_Validating(object sender, CancelEventArgs e)
        {
            GrdSize.PostEditor();
            DataRow Dr = GrdSize.GetFocusedDataRow();
            if (Val.ToDecimal(Dr["TOCARAT"]) != 0 || Val.Val(GrdSize.EditingValue) != 0)
            {
                if (CheckDuplicate("TOCARAT", Val.ToString(GrdSize.EditingValue), GrdSize.FocusedRowHandle, "TOCARAT"))
                {
                    e.Cancel = true;
                    return;
                }
                if (Val.ToDecimal(Dr["FROMCARAT"]) > Val.ToDecimal(GrdSize.EditingValue))
                {
                    Global.Message("To Carat must be Greter Than From Carat");
                    e.Cancel = true;
                    return;
                }
                else
                {
                    DTabSize.AcceptChanges();

                }

                var dValue = from row in DTabSize.AsEnumerable()
                             where Val.Val(row["FROMCARAT"]) <= Val.Val(GrdSize.EditingValue) && Val.Val(row["TOCARAT"]) >= Val.Val(GrdSize.EditingValue) && row.Table.Rows.IndexOf(row) != GrdSize.FocusedRowHandle
                             select row;

                if (dValue.Any())
                {
                    Global.Message("This Value Already Exist Between Some From Carat and To Carat Please Check.!");
                    e.Cancel = true;
                    return;
                }

            }

        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            Clear();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            string StrFileName = ExportExcel();

            if (Global.Confirm("Do You Want To Open File ? ") == System.Windows.Forms.DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(StrFileName, "CMD");
            }
        }
    }
}
