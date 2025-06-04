using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Globalization;
using Config = BusLib.Configuration.BOConfiguration;

namespace MahantExport
{
    public partial class FrmSearchFilterNew : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        public FrmSearchFilterNew()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();
            FillListControls();
            Clear();

        }
        private void AttachFormDefaultEvent()
        {
            Val.FormGeneralSetting(this);
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = false;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.FormKeyPress = true;

            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
        }


        public void FillListControls()
        {

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
            DR = DTab.Select("PARATYPE='TABLEBLACKINC'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                ListTableBlackinc.DTab = DTTemp.DefaultView.ToTable();
                ListTableBlackinc.DisplayMember = "SHORTNAME";
                ListTableBlackinc.ValueMember = "PARA_ID";
            }
            DR = DTab.Select("PARATYPE='SIDEBLACKINC'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                ListSideBlackinc.DTab = DTTemp.DefaultView.ToTable();
                ListSideBlackinc.DisplayMember = "SHORTNAME";
                ListSideBlackinc.ValueMember = "PARA_ID";
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

            DR = DTab.Select("PARATYPE='MILKY'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                ListMilky.DTab = DTTemp.DefaultView.ToTable();
                ListMilky.DisplayMember = "SHORTNAME";
                ListMilky.ValueMember = "PARA_ID";
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
            DR = DTab.Select("PARATYPE='SIZE'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                ListSize.DTab = DTTemp.DefaultView.ToTable();
                ListSize.DisplayMember = "SHORTNAME";
                ListSize.ValueMember = "PARA_ID";
            }          
            DR = DTab.Select("PARATYPE='WEBSTATUS'"); // UnCmment upper line When hide delivery status from selection
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "PARANAME";
                DTTemp.Rows.Add(999, "WEBSTATUS", "DEPTTRAN", "DEPTTRANSFER", "DEPTTRANSFER", "", 9999);
                DTTemp.Rows.Add(999, "WEBSTATUS", "SINGLETOPARCEL", "SINGLETOPARCEL", "SINGLETOPARCEL", "", 9999);
                ListStatus.DTab = DTTemp.DefaultView.ToTable();
                ListStatus.DisplayMember = "SHORTNAME";
                ListStatus.ValueMember = "SHORTNAME";
                //ListStatus.SetSelectedCheckBox("AVAILABLE");
            }
            DR = DTab.Select("PARATYPE='COLORSHADE'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                ListColorShade.DTab = DTTemp.DefaultView.ToTable();
                ListColorShade.DisplayMember = "SHORTNAME";
                ListColorShade.ValueMember = "PARA_ID";
            }
        }

        public void Clear()
        {
            Global.FILTERSHAPE_ID = string.Empty;
            Global.FILTERSIZE_ID = string.Empty;
            Global.FILTERCOLOR_ID = string.Empty;
            Global.FILTERCLARITY_ID = string.Empty;
            Global.FILTERCUT_ID = string.Empty;
            Global.FILTERPOL_ID = string.Empty;
            Global.FILTERSYM_ID = string.Empty;
            Global.FILTERFL_ID = string.Empty;
            Global.FILTERCOLORSHADE_ID = string.Empty;
            Global.FILTERMILKY_ID = string.Empty;
            Global.FILTERLAB_ID = string.Empty;
            Global.FILTERWEBSTATUS = string.Empty;
            Global.FILTERTABLEBLACKINC_ID = string.Empty;
            Global.FILTERSIDEBLACKINC_ID = string.Empty;
        }
        private void GetSelectedFilter()
        {
            Global.FILTERSHAPE_ID = ListShape.GetSelectedReportTagValues();
            Global.FILTERSIZE_ID = ListSize.GetSelectedReportTagValues();

            Global.FILTERCOLOR_ID = ListColor.GetSelectedReportTagValues();
            Global.FILTERCLARITY_ID = ListClarity.GetSelectedReportTagValues();
            Global.FILTERCUT_ID = ListCut.GetSelectedReportTagValues();
            Global.FILTERPOL_ID = ListPol.GetSelectedReportTagValues();
            Global.FILTERSYM_ID = ListSym.GetSelectedReportTagValues();
            Global.FILTERFL_ID = ListFL.GetSelectedReportTagValues();
            Global.FILTERCOLORSHADE_ID = ListColorShade.GetSelectedReportTagValues();

            Global.FILTERMILKY_ID = ListMilky.GetSelectedReportTagValues();
            Global.FILTERLAB_ID = ListLab.GetSelectedReportTagValues();
            Global.FILTERWEBSTATUS = ListStatus.GetSelectedReportTagValues();
            Global.FILTERTABLEBLACKINC_ID = ListTableBlackinc.GetSelectedReportTagValues();
            Global.FILTERSIDEBLACKINC_ID = ListSideBlackinc.GetSelectedReportTagValues();

        }       
        private void FrmSearchFilterNew_Load(object sender, EventArgs e)
        {
            try
            {
                Val.FormGeneralSetting(this);
                AttachFormDefaultEvent();
                FillListControls();
                Clear();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        private void FrmSearchFilterNew_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Escape)
                {
                    this.Close();
                    Clear();
                }
                else if (e.KeyCode == Keys.Enter)
                {
                    GetSelectedFilter();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        private void BtnBack_Click(object sender, EventArgs e)
        {
            Clear();
            this.Close();
        }
    }
}