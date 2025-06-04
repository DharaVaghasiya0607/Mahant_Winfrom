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
    public partial class FrmRapNetStockSync : DevControlLib.cDevXtraForm
    {
        String PasteData = "";
        IDataObject PasteclipData = Clipboard.GetDataObject();

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        DataTable DTabSize = new DataTable();
        RapNetStockSync ObjRapNet = new RapNetStockSync();
        RapNetStockSyncProperty RapProperty = new RapNetStockSyncProperty();
        DataTable Dtab = new DataTable();
    
      
        #region Property Settings

        public FrmRapNetStockSync()
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
           FillListControls();
           Clear();
        }

        public void ShowForm(RapNetStockSyncProperty Property)
        {
            try
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
                Fill();

                RapProperty = Property;

                txtUserName.Tag = Val.ToInt32(RapProperty.ID);

                ListShape.SetSelectedCheckBox(RapProperty.SHAPE_ID);
                ListShape.SetSelectedCheckBox(RapProperty.SHAPENAME);
                ListColor.SetSelectedCheckBox(RapProperty.COLOR_ID);
                ListColor.SetSelectedCheckBox(RapProperty.COLORNAME);
                ListClarity.SetSelectedCheckBox(RapProperty.CLARITY_ID);
                ListClarity.SetSelectedCheckBox(RapProperty.CLARITYNAME);
                ListCut.SetSelectedCheckBox(RapProperty.CUT_ID);
                ListCut.SetSelectedCheckBox(RapProperty.CUTNAME);
                ListPol.SetSelectedCheckBox(RapProperty.POL_ID);
                ListPol.SetSelectedCheckBox(RapProperty.POLNAME);
                ListSym.SetSelectedCheckBox(RapProperty.SYM_ID);
                ListSym.SetSelectedCheckBox(RapProperty.SYMNAME);
                ListFL.SetSelectedCheckBox(RapProperty.FL_ID);
                ListFL.SetSelectedCheckBox(RapProperty.FLNAME);

                ListFancyColor.SetSelectedCheckBox(RapProperty.FANCYCOLOR_ID);
                ListFancyColor.SetSelectedCheckBox(RapProperty.FANCYCOLORNAME);
                ListMilky.SetSelectedCheckBox(RapProperty.MILKY_ID);
                ListMilky.SetSelectedCheckBox(RapProperty.MILKYNAME);
                ListLocation.SetSelectedCheckBox(RapProperty.LOCATION_ID);
                ListLocation.SetSelectedCheckBox(RapProperty.LOCATIONNAME);

                DTabSize.Rows[0]["FROMCARAT"] = RapProperty.FROMCARAT1;
                DTabSize.Rows[0]["TOCARAT"] = RapProperty.TOCARAT1;

                DTabSize.Rows[1]["FROMCARAT"] = RapProperty.FROMCARAT2;
                DTabSize.Rows[1]["TOCARAT"] = RapProperty.TOCARAT2;

                DTabSize.Rows[2]["FROMCARAT"] = RapProperty.FROMCARAT3;
                DTabSize.Rows[2]["TOCARAT"] = RapProperty.TOCARAT3;

                DTabSize.Rows[3]["FROMCARAT"] = RapProperty.FROMCARAT4;
                DTabSize.Rows[3]["TOCARAT"] = RapProperty.TOCARAT4;

                DTabSize.Rows[4]["FROMCARAT"] = RapProperty.FROMCARAT5;
                DTabSize.Rows[4]["TOCARAT"] = RapProperty.TOCARAT5;

                if (RbtStoneNo.Checked == true)
                {
                    txtStoneCertiMFGMemo.Text = RapProperty.STONENO;
                    RapProperty.CERTINO = string.Empty;
                    RapProperty.SERIALNO = string.Empty;
                    RapProperty.MEMONO = string.Empty;
                }
                else if (RbtCertiNo.Checked == true)
                {
                    RapProperty.STONENO = string.Empty;
                    txtStoneCertiMFGMemo.Text = RapProperty.CERTINO;
                    RapProperty.SERIALNO = string.Empty;
                    RapProperty.MEMONO = string.Empty;
                }
                else if (RbtSerialNo.Checked == true)
                {
                    RapProperty.STONENO = string.Empty;
                    RapProperty.CERTINO = string.Empty;
                    txtStoneCertiMFGMemo.Text = RapProperty.SERIALNO;
                    RapProperty.MEMONO = string.Empty;
                }
                else if (RbtMemoNo.Checked == true)
                {
                    RapProperty.STONENO = string.Empty;
                    RapProperty.CERTINO = string.Empty;
                    RapProperty.SERIALNO = string.Empty;
                    txtStoneCertiMFGMemo.Text = RapProperty.MEMONO;
                }

                ListLab.SetSelectedCheckBox(RapProperty.LAB_ID);
                ListLab.SetSelectedCheckBox(RapProperty.LABNAME);
                ListStatus.SetSelectedCheckBox(RapProperty.WEBSTATUS_ID);
                ListStatus.SetSelectedCheckBox(RapProperty.WEBSTATUSNAME);
                ListBox.SetSelectedCheckBox(RapProperty.BOX_ID);
                ListBox.SetSelectedCheckBox(RapProperty.BOXNAME);

                txtFromLength.Text = Val.ToString(RapProperty.FROMLENGTHPER);
                txtToLength.Text = Val.ToString(RapProperty.TOLENGTHPER);

                txtFromWidth.Text = Val.ToString(RapProperty.FROMWIDTHPER);
                txtToWidth.Text = Val.ToString(RapProperty.TOWIDTHPER);

                txtFromHeight.Text = Val.ToString(RapProperty.FROMHEIGHTPER);
                txtToHeight.Text = Val.ToString(RapProperty.TOHEIGHTPER);

                txtFromDepthPer.Text = Val.ToString(RapProperty.FROMDEPTHPER);
                txtToDepthPer.Text = Val.ToString(RapProperty.TODEPTHPER);

                txtFromTablePer.Text = Val.ToString(RapProperty.FROMTABLEPER);
                txtToTablePer.Text = Val.ToString(RapProperty.TOTABLEPER);

                ChkActive.Checked = RapProperty.ISACTIVE;

                MainGrdSize.DataSource = DTabSize;
                GrdSize.RefreshData();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjStock);
        }

        public void FillListControls()
        {
            try
            {
                DTabSize = new DataTable();
                DTabSize.Columns.Add(new DataColumn("FROMCARAT", typeof(Double)));
                DTabSize.Columns.Add(new DataColumn("TOCARAT", typeof(Double)));

                DTabSize.Rows.Add(DTabSize.NewRow());
                DTabSize.Rows.Add(DTabSize.NewRow());
                DTabSize.Rows.Add(DTabSize.NewRow());
                DTabSize.Rows.Add(DTabSize.NewRow());
                DTabSize.Rows.Add(DTabSize.NewRow());

                MainGrdSize.DataSource = DTabSize;
                GrdSize.RefreshData();


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
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

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
             
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;

            ChkActive.Checked = false;
            Fill();
        }

        public void Fill()
        {
            try
            {
                DataTable Dtab = ObjRapNet.GetData();
                MainGrd.DataSource = Dtab;
                if (Dtab.Rows.Count > 0)
                {
                    GrdDetails.BestFitColumns();
                }
            }
            catch (Exception ex)
            {

                Global.MessageError(ex.Message);
            }

        }

        #endregion

        #region Control Event

        private void lblResetCarat_Click(object sender, EventArgs e)
        {
            DTabSize.Rows.Clear();
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
        }

     
        private void txtStoneCertiMFGMemo_MouseDown(object sender, MouseEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtStoneCertiMFGMemo_TextChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtStoneCertiMFGMemo_KeyUp(object sender, KeyEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

       
        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValSave() == false) 
                {
                    Global.Message("Please Enter UserName Or Password !!!");
                    return;
                }

                RapProperty.ID = Val.ToInt32(txtUserName.Tag);
                RapProperty.USERNAME = Val.ToString(txtUserName.Text);
                RapProperty.PASSWORD = Val.ToString(txtPassword.Text);
                RapProperty.SHAPE_ID = ListShape.GetSelectedReportTagValues();
                RapProperty.SHAPENAME = ListShape.GetSelectedReportTextValues();
                RapProperty.COLOR_ID = ListColor.GetSelectedReportTagValues();
                RapProperty.COLORNAME = ListColor.GetSelectedReportTextValues();
                RapProperty.CLARITY_ID = ListClarity.GetSelectedReportTagValues();
                RapProperty.CLARITYNAME = ListClarity.GetSelectedReportTextValues();
                RapProperty.CUT_ID = ListCut.GetSelectedReportTagValues();
                RapProperty.CUTNAME = ListCut.GetSelectedReportTextValues();
                RapProperty.POL_ID = ListPol.GetSelectedReportTagValues();
                RapProperty.POLNAME = ListPol.GetSelectedReportTextValues();
                RapProperty.SYM_ID = ListSym.GetSelectedReportTagValues();
                RapProperty.SYMNAME = ListSym.GetSelectedReportTextValues();
                RapProperty.FL_ID = ListFL.GetSelectedReportTagValues();
                RapProperty.FLNAME = ListFL.GetSelectedReportTextValues();


                RapProperty.FANCYCOLOR_ID = ListFancyColor.GetSelectedReportTagValues();
                RapProperty.FANCYCOLORNAME = ListFancyColor.GetSelectedReportTextValues();
                RapProperty.LOCATION_ID = ListLocation.GetSelectedReportTagValues();
                RapProperty.LOCATIONNAME = ListLocation.GetSelectedReportTextValues();
                RapProperty.MILKY_ID = ListMilky.GetSelectedReportTagValues();
                RapProperty.MILKYNAME = ListMilky.GetSelectedReportTextValues();
                RapProperty.LAB_ID = ListLab.GetSelectedReportTagValues();
                RapProperty.LABNAME = ListLab.GetSelectedReportTextValues();
                RapProperty.BOX_ID = ListBox.GetSelectedReportTagValues();
                RapProperty.BOXNAME = ListBox.GetSelectedReportTextValues();
                RapProperty.WEBSTATUS_ID = ListStatus.GetSelectedReportTagValues();
                RapProperty.WEBSTATUSNAME = ListStatus.GetSelectedReportTextValues();

                RapProperty.FROMCARAT1 = Val.Val(DTabSize.Rows[0]["FROMCARAT"]);
                RapProperty.TOCARAT1 = Val.Val(DTabSize.Rows[0]["TOCARAT"]);

                RapProperty.FROMCARAT2 = Val.Val(DTabSize.Rows[1]["FROMCARAT"]);
                RapProperty.TOCARAT2 = Val.Val(DTabSize.Rows[1]["TOCARAT"]);

                RapProperty.FROMCARAT3 = Val.Val(DTabSize.Rows[2]["FROMCARAT"]);
                RapProperty.TOCARAT3 = Val.Val(DTabSize.Rows[2]["TOCARAT"]);

                RapProperty.FROMCARAT4 = Val.Val(DTabSize.Rows[3]["FROMCARAT"]);
                RapProperty.TOCARAT4 = Val.Val(DTabSize.Rows[3]["TOCARAT"]);

                RapProperty.FROMCARAT5 = Val.Val(DTabSize.Rows[4]["FROMCARAT"]);
                RapProperty.TOCARAT5 = Val.Val(DTabSize.Rows[4]["TOCARAT"]);

                RapProperty.FROMLENGTHPER = Val.Val(txtFromLength.Text);
                RapProperty.TOLENGTHPER = Val.Val(txtToLength.Text);

                RapProperty.FROMWIDTHPER = Val.Val(txtFromWidth.Text);
                RapProperty.TOWIDTHPER = Val.Val(txtToWidth.Text);

                RapProperty.FROMHEIGHTPER = Val.Val(txtFromHeight.Text);
                RapProperty.TOHEIGHTPER = Val.Val(txtToHeight.Text);

                RapProperty.FROMTABLEPER = Val.Val(txtFromTablePer.Text);
                RapProperty.TOTABLEPER = Val.Val(txtToTablePer.Text);

                RapProperty.FROMDEPTHPER = Val.Val(txtFromDepthPer.Text);
                RapProperty.TODEPTHPER = Val.Val(txtToDepthPer.Text);

                if (RbtStoneNo.Checked == true)
                {
                    RapProperty.STONENO = txtStoneCertiMFGMemo.Text;
                    RapProperty.CERTINO = string.Empty;
                    RapProperty.SERIALNO = string.Empty;
                    RapProperty.MEMONO = string.Empty;
                }
                else if (RbtCertiNo.Checked == true)
                {
                    RapProperty.STONENO = string.Empty;
                    RapProperty.CERTINO = txtStoneCertiMFGMemo.Text;
                    RapProperty.SERIALNO = string.Empty;
                    RapProperty.MEMONO = string.Empty;
                }
                else if (RbtSerialNo.Checked == true)
                {
                    RapProperty.STONENO = string.Empty;
                    RapProperty.CERTINO = string.Empty;
                    RapProperty.SERIALNO = txtStoneCertiMFGMemo.Text;
                    RapProperty.MEMONO = string.Empty;
                }
                else if (RbtMemoNo.Checked == true)
                {
                    RapProperty.STONENO = string.Empty;
                    RapProperty.CERTINO = string.Empty;
                    RapProperty.SERIALNO = string.Empty;
                    RapProperty.MEMONO = txtStoneCertiMFGMemo.Text;
                }

                RapProperty.ISACTIVE = Val.ToBoolean(ChkActive.Checked);

                RapProperty = ObjRapNet.Save(RapProperty);

                string StrReturnDesc = RapProperty.ReturnMessageDesc;
                Global.Message(StrReturnDesc);
                Clear();
               
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }

        private void btnClear_Click_1(object sender, EventArgs e)
        {
            Clear();
        }

        private void BtnExit_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region GridData
        public void FetchValue(DataRow DR)
        {
            try
            {

                txtUserName.Tag = Val.ToInt32(DR["ID"]);
                txtUserName.Text = Val.ToString(DR["USERNAME"]);
                txtPassword.Text = Val.ToString(DR["PASSWORD"]);
                ListShape.SetSelectedCheckBox(Val.ToString(DR["SHAPE_ID"]));
                ListColor.SetSelectedCheckBox(Val.ToString(DR["COLOR_ID"]));
                ListClarity.SetSelectedCheckBox(Val.ToString(DR["CLARITY_ID"]));
                ListCut.SetSelectedCheckBox(Val.ToString(DR["CUT_ID"]));
                ListPol.SetSelectedCheckBox(Val.ToString(DR["POL_ID"]));
                ListSym.SetSelectedCheckBox(Val.ToString(DR["SYM_ID"]));
                ListFL.SetSelectedCheckBox(Val.ToString(DR["FL_ID"]));

                ListFancyColor.SetSelectedCheckBox(Val.ToString(DR["FANCYCOLOR_ID"]));
                ListMilky.SetSelectedCheckBox(Val.ToString(DR["MILKY_ID"]));
                ListLocation.SetSelectedCheckBox(Val.ToString(DR["LOCATION_ID"]));

                DTabSize.Rows[0]["FROMCARAT"] = Val.ToDouble(DR["FROMCARAT1"]);
                DTabSize.Rows[0]["TOCARAT"] = Val.ToDouble(DR["TOCARAT1"]);

                DTabSize.Rows[1]["FROMCARAT"] = Val.ToDouble(DR["FROMCARAT2"]);
                DTabSize.Rows[1]["TOCARAT"] = Val.ToDouble(DR["TOCARAT2"]);

                DTabSize.Rows[2]["FROMCARAT"] = Val.ToDouble(DR["FROMCARAT3"]);
                DTabSize.Rows[2]["TOCARAT"] = Val.ToDouble(DR["TOCARAT3"]);

                DTabSize.Rows[3]["FROMCARAT"] = Val.ToDouble(DR["FROMCARAT4"]);
                DTabSize.Rows[3]["TOCARAT"] = Val.ToDouble(DR["TOCARAT4"]);

                DTabSize.Rows[4]["FROMCARAT"] = Val.ToDouble(DR["FROMCARAT5"]);
                DTabSize.Rows[4]["TOCARAT"] = Val.ToDouble(DR["TOCARAT5"]);

                if (RbtStoneNo.Checked == true)
                {
                    txtStoneCertiMFGMemo.Text = Val.ToString(DR["STONENO"]);
                }

                else if (RbtCertiNo.Checked == true)
                {
                    txtStoneCertiMFGMemo.Text = Val.ToString(DR["CERTINO"]);
                }

                else if (RbtSerialNo.Checked == true)
                {
                    txtStoneCertiMFGMemo.Text = Val.ToString(DR["SERIALNO"]);
                }

                else if (RbtMemoNo.Checked == true)
                {
                    txtStoneCertiMFGMemo.Text = Val.ToString(DR["MEMONO"]);
                }

                ListLab.SetSelectedCheckBox(Val.ToString(DR["LAB_ID"]));
                ListBox.SetSelectedCheckBox(Val.ToString(DR["BOX_ID"]));
                ListStatus.SetSelectedCheckBox(Val.ToString(DR["WEBSTATUS_ID"]));

                txtFromLength.Text = Val.ToString(DR["FROMLENGTHPER"]);
                txtToLength.Text = Val.ToString(DR["TOLENGTHPER"]);

                txtFromWidth.Text = Val.ToString(DR["FROMWIDTHPER"]);
                txtToWidth.Text = Val.ToString(DR["TOWIDTHPER"]);

                txtFromHeight.Text = Val.ToString(DR["FROMHEIGHTPER"]);
                txtToHeight.Text = Val.ToString(DR["TOHEIGHTPER"]);

                txtFromDepthPer.Text = Val.ToString(DR["FROMDEPTHPER"]);
                txtToDepthPer.Text = Val.ToString(DR["TODEPTHPER"]);

                txtFromTablePer.Text = Val.ToString(DR["FROMTABLEPER"]);
                txtToTablePer.Text = Val.ToString(DR["TOTABLEPER"]);

                ChkActive.Checked = Val.ToBoolean(DR["ISACTIVE"]);
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void GrdDetails_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {
                
                throw;
            }
            if (e.RowHandle < 0)
            {
                return;
            }

            if (e.Clicks == 2)
            {
                DataRow DR = GrdDetails.GetDataRow(e.RowHandle);
                FetchValue(DR);
                DR = null;
            }
        }
        #endregion

        #region Validation

        private bool ValSave()
        {
            string ErrMsg = "";
            bool flag = true;
            try
            {
                if (Val.ToString(txtUserName.Text) == string.Empty)
                {
                    ErrMsg += "Please Enter UserName";
                    if (flag)
                    {
                        txtUserName.Focus();
                    }
                    flag = false;
                }
                if (Val.ToString(txtPassword.Text) == string.Empty)
                {
                    ErrMsg += "Please Enter Password";
                    if (flag)
                    {
                        txtPassword.Focus();
                    }
                    flag = false;
                }
                return flag;
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
                return false;
            }
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
            try
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
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }

        private void RepTxtToCarat_Validating(object sender, CancelEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        #endregion

    }
}
