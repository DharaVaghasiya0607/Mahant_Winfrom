using BusLib.Configuration;
using BusLib.Transaction;
using DevExpress.XtraCharts;
using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace MahantExport.View
{
    public partial class FrmSalesAnalysis : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();
        BOFormPer ObjPer = new BOFormPer();
        #region Property Settings

        public FrmSalesAnalysis()
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

            DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            DTPToDate.Value = DateTime.Now;
            FillListControls();
            DTPFromDate.Focus();
        }

        public void FillListControls()
        {
            DataTable DTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PARAALL);

            DataRow[] DR = DTab.Select("PARATYPE='SHAPE'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";

                chkShape.DataSource = DTTemp;
                chkShape.DisplayMember = "PARANAME";
                chkShape.ValueMember = "PARA_ID";
            }

            DR = DTab.Select("PARATYPE='COLOR'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";

                chkColor.DataSource = DTTemp;
                chkColor.DisplayMember = "PARANAME";
                chkColor.ValueMember = "PARA_ID";
            }

            DR = DTab.Select("PARATYPE='CLARITY'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";

                chkClarity.DataSource = DTTemp;
                chkClarity.DisplayMember = "PARANAME";
                chkClarity.ValueMember = "PARA_ID";
            }

            DR = DTab.Select("PARATYPE='CUT'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";

                chkCut.DataSource = DTTemp;
                chkCut.DisplayMember = "PARANAME";
                chkCut.ValueMember = "PARA_ID";
            }

            DR = DTab.Select("PARATYPE='POLISH'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";

                chkPolish.DataSource = DTTemp;
                chkPolish.DisplayMember = "PARANAME";
                chkPolish.ValueMember = "PARA_ID";
            }

            DR = DTab.Select("PARATYPE='SYMMETRY'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";

                chkSymm.DataSource = DTTemp;
                chkSymm.DisplayMember = "PARANAME";
                chkSymm.ValueMember = "PARA_ID";
            }

            DR = DTab.Select("PARATYPE='FLUORESCENCE'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";

                chkFlou.DataSource = DTTemp;
                chkFlou.DisplayMember = "PARANAME";
                chkFlou.ValueMember = "PARA_ID";
            }

            DR = DTab.Select("PARATYPE='COUNTRY'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "PARANAME";

                chkCountry.DataSource = DTTemp;
                chkCountry.DisplayMember = "PARANAME";
                chkCountry.ValueMember = "PARA_ID";
            }
            DR = DTab.Select("PARATYPE='LOCATION'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "PARANAME";

                chkLocation.DataSource = DTTemp;
                chkLocation.DisplayMember = "PARANAME";
                chkLocation.ValueMember = "PARA_ID";
            }

            DR = DTab.Select("PARATYPE='LAB'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "PARANAME";

                chkLab.DataSource = DTTemp;
                chkLab.DisplayMember = "PARANAME";
                chkLab.ValueMember = "PARA_ID";
            }
            chkParty.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);
            chkParty.DisplayMember = "PARTYNAME";
            chkParty.ValueMember = "PARTY_ID";

            chkSeller.DataSource = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_EMPLOYEE);
            chkSeller.DisplayMember = "EMPLOYEENAME";
            chkSeller.ValueMember = "EMPLOYEE_ID";

        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMemo);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        #endregion


        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public string GetSelectCmbValue(CheckedListBoxControl chkBoxcontrol)
        {
            string tempstr = string.Empty;
            if (chkBoxcontrol.DataSource != null)
            {
                foreach (object itemChecked in chkBoxcontrol.CheckedItems)
                {
                    DataRowView castedItem = itemChecked as DataRowView;
                    tempstr = tempstr + Val.ToString(castedItem[chkBoxcontrol.ValueMember]) + ",";
                }
            }
            if (tempstr.Length != 0)
            {
                tempstr = tempstr.Substring(0, tempstr.Length - 1);
            }
            return tempstr;
        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;

                string StrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
                string StrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());
                string StrOpe = string.Empty;
                string StrProcess = string.Empty;

                if (RbtPcs.Checked == true) StrOpe = Val.ToString(RbtPcs.Tag);
                else if (RbtCarat.Checked == true) StrOpe = Val.ToString(RbtCarat.Tag);
                else if (RbtAmount.Checked == true) StrOpe = Val.ToString(RbtAmount.Tag);

                if (RbtPurchase.Checked == true) StrProcess = Val.ToString(RbtPurchase.Tag);
                else if (RbtMemo.Checked == true) StrProcess = Val.ToString(RbtMemo.Tag);
                else if (RbtOrder.Checked == true) StrProcess = Val.ToString(RbtOrder.Tag);
                else if (RbtDelivery.Checked == true) StrProcess = Val.ToString(RbtDelivery.Tag);

                string StrShape = GetSelectCmbValue(chkShape);
                string StrColor = GetSelectCmbValue(chkColor);
                string StrClarity = GetSelectCmbValue(chkClarity);
                string StrCut = GetSelectCmbValue(chkCut);
                string StrPol = GetSelectCmbValue(chkPolish);
                string StrSym = GetSelectCmbValue(chkSymm);
                string StrFL = GetSelectCmbValue(chkFlou);
                string StrLocation = GetSelectCmbValue(chkLocation);
                string StrSeller = GetSelectCmbValue(chkSeller);
                string StrParty = GetSelectCmbValue(chkParty);

                DataSet DS = ObjMemo.GetSalesAnalysisReport(StrOpe, StrProcess, StrFromDate, StrToDate,
                    StrShape,
                    StrColor,
                    StrClarity,
                    StrCut,
                    StrPol,
                    StrSym,
                    StrFL,
                    StrLocation,
                    StrSeller,
                    StrParty
                    );

                MainGridParty.DataSource = DS.Tables[0];
                MainGridParty.Refresh();

                MainGridSeller.DataSource = DS.Tables[1];
                MainGridSeller.Refresh();

               DispalyChart(ChartDateWise, ViewType.Line, DS.Tables[2], "MEMODATE", "PARTICULAR");

               DispalyChart(ChartShape, ViewType.Pie, DS.Tables[3], "SHAPENAME", "PARTICULAR");

               DispalyChart(ChartColor, ViewType.Pie, DS.Tables[4], "COLORNAME", "PARTICULAR");

               DispalyChart(ChartClarity, ViewType.Pie, DS.Tables[5], "CLARITYNAME", "PARTICULAR");

               DispalyChart(ChartCut, ViewType.Pie, DS.Tables[6], "CUTNAME", "PARTICULAR");

               DispalyChart(ChartPol, ViewType.Pie, DS.Tables[7], "POLNAME", "PARTICULAR");

               DispalyChart(ChartSym, ViewType.Pie, DS.Tables[8], "SYMNAME", "PARTICULAR");

               DispalyChart(ChartFl, ViewType.Pie, DS.Tables[9], "FLNAME", "PARTICULAR");

               DispalyChart(ChartLocation, ViewType.Pie, DS.Tables[10], "LOCATIONNAME", "PARTICULAR");

               DispalyChart(ChartSource, ViewType.Pie, DS.Tables[11], "SOURCENAME", "PARTICULAR");

               DispalyChart(ChartSeller, ViewType.Pie, DS.Tables[12], "SELLERNAME", "PARTICULAR");

               DispalyChart(ChartCountry, ViewType.Pie, DS.Tables[13], "BILLINGCOUNTRY", "PARTICULAR");

               DispalyChart(ChartState, ViewType.Pie, DS.Tables[14], "BILLINGSTATE", "PARTICULAR");

                tabControl1.SelectedIndex = 0;

                string StrCaption = "";
                if (RbtPcs.Checked == true)
                {
                    StrCaption = "Pcs";
                }
                else if (RbtCarat.Checked == true)
                {
                    StrCaption = "Carat";
                }
                else if (RbtAmount.Checked == true)
                {
                    StrCaption = "Amount";
                }

                GrdShape.Columns["PARTICULAR"].Caption = StrCaption;
                MainGrdShape.DataSource = DS.Tables[3];
                MainGrdShape.Refresh();

                GrdColor.Columns["PARTICULAR"].Caption = StrCaption;
                MainGrdColor.DataSource = DS.Tables[4];
                MainGrdColor.Refresh();

                GrdClarity.Columns["PARTICULAR"].Caption = StrCaption;
                MainGrdClarity.DataSource = DS.Tables[5];
                MainGrdClarity.Refresh();

                GrdCut.Columns["PARTICULAR"].Caption = StrCaption;
                MainGrdCut.DataSource = DS.Tables[6];
                MainGrdCut.Refresh();

                GrdPolish.Columns["PARTICULAR"].Caption = StrCaption;
                MainGrdPolish.DataSource = DS.Tables[7];
                MainGrdPolish.Refresh();

                GrdSymmetry.Columns["PARTICULAR"].Caption = StrCaption;
                MainGrdSymmetry.DataSource = DS.Tables[8];
                MainGrdSymmetry.Refresh();

                GrdFlourcscene.Columns["PARTICULAR"].Caption = StrCaption;
                MainGrdFlourcscene.DataSource = DS.Tables[9];
                MainGrdFlourcscene.Refresh();

                GrdLocation.Columns["PARTICULAR"].Caption = StrCaption;
                MainGrdLocation.DataSource = DS.Tables[10];
                MainGrdLocation.Refresh();

                GrdSource.Columns["PARTICULAR"].Caption = StrCaption;
                MainGrdSource.DataSource = DS.Tables[11];
                MainGrdSource.Refresh();

                GrdSeller.Columns["PARTICULAR"].Caption = StrCaption;
                MainGrdSeller.DataSource = DS.Tables[12];
                MainGrdSource.Refresh();

                GrdCountry.Columns["PARTICULAR"].Caption = StrCaption;
                MainGrdCountry.DataSource = DS.Tables[13];
                MainGrdCountry.Refresh();

                GrdState.Columns["PARTICULAR"].Caption = StrCaption;
                MainGrdState.DataSource = DS.Tables[14];
                MainGrdState.Refresh();

                this.Cursor = Cursors.Default;


            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void DispalyChart(DevExpress.XtraCharts.ChartControl ChartControl, DevExpress.XtraCharts.ViewType ViewType, DataTable dt, String X, String Y)
        {
            if (ViewType == ViewType.Area)
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains(X))
                    {
                        Series series1 = new Series("", ViewType);
                        ChartControl.Series.Clear();

                        double TotalStone = Convert.ToDouble(dt.Copy().Compute("Max(" + Y + ")", ""));
                        DataView view = dt.Copy().DefaultView;
                        DataTable distinctValues = view.ToTable(true, X, Y);

                        series1.DataSource = distinctValues;
                        series1.ArgumentScaleType = ScaleType.Auto;
                        series1.ArgumentDataMember = X;
                        series1.ValueScaleType = ScaleType.Numerical;
                        series1.ValueDataMembers.AddRange(new string[] { Y });
                        ChartControl.Series.Add(series1);

                        DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
                        ((DevExpress.XtraCharts.XYDiagram)ChartControl.Diagram).AxisX.Range.SideMarginsEnabled = true;
                        ((DevExpress.XtraCharts.XYDiagram)ChartControl.Diagram).AxisX.Range.Auto = false;


                    }
                }
            }
            else
            {
                DataView view = dt.Copy().DefaultView;
                DataTable distinctValues = view.ToTable(true, X, Y);
                DataTable Xtable = distinctValues.Copy().DefaultView.ToTable(true, X);
                ChartControl.Series.Clear();
                Series Pie3DSeries = new Series("", ViewType);

                var ownerGroups = distinctValues.AsEnumerable()
               .GroupBy(row => row.Field<string>(X));

                var dt2 = distinctValues.Clone();
                dt2.Columns[Y].DataType = typeof(decimal);
                var intColumns = dt2.Columns.Cast<DataColumn>()
                    .Where(c => c.DataType == typeof(decimal)).ToArray();
                foreach (var grp in ownerGroups)
                {
                    if (grp.Key != null && grp.Key != "")
                    {
                        var row = dt2.Rows.Add();
                        row.SetField(X, grp.Key);

                        foreach (DataColumn col in intColumns)
                        {
                            string Exp = X + "='" + grp.Key + "'";
                            double sum = Convert.ToDouble(distinctValues.Compute("sum(" + col + ")", Exp));
                            row.SetField(col, sum);
                        }
                    }
                }


                Pie3DSeries.DataSource = dt2;
                ChartControl.Series.Add(Pie3DSeries);
                Pie3DSeries.ArgumentScaleType = ScaleType.Auto;
                Pie3DSeries.ArgumentDataMember = X;
                Pie3DSeries.ValueScaleType = ScaleType.Numerical;
                Pie3DSeries.ValueDataMembers.AddRange(new string[] { Y });

                ChartControl.Legend.Visible = false;
                Pie3DSeries.Label.PointOptions.PointView = PointView.ArgumentAndValues;
                Pie3DSeries.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
                Pie3DSeries.Label.PointOptions.ValueNumericOptions.Precision = 0;
                //((SimpleDiagram3D)ChartControl.Diagram).RuntimeRotation = true;
                //((SimpleDiagram3D)ChartControl.Diagram).RuntimeZooming = true;

            }
        }
        public void DispalyBARChart(DevExpress.XtraCharts.ChartControl ChartControl, DevExpress.XtraCharts.ViewType ViewType, DataSet dset)
        {
            if (ViewType == ViewType.Area)
            {
                if (dset.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dset.Tables.Count; i++)
                    {
                        if (dset.Tables[i].Columns.Contains(dset.Tables[i].Columns[1].ColumnName))
                        {
                            Series series1 = new Series("", ViewType);
                            ChartControl.Series.Clear();

                            double TotalStone = Convert.ToDouble(dset.Tables[i].Copy().Compute("Max(" + dset.Tables[i].Columns[0] + ")", ""));
                            DataView view = dset.Tables[i].Copy().DefaultView;
                            DataTable distinctValues = view.ToTable(true, dset.Tables[i].Columns[1].ColumnName, dset.Tables[i].Columns[0].ColumnName);

                            series1.DataSource = distinctValues;
                            series1.ArgumentScaleType = ScaleType.Auto;
                            series1.ArgumentDataMember = dset.Tables[i].Columns[1].ColumnName;
                            series1.ValueScaleType = ScaleType.Numerical;
                            series1.ValueDataMembers.AddRange(new string[] { dset.Tables[i].Columns[0].ColumnName });
                            ChartControl.Series.Add(series1);

                            DevExpress.XtraCharts.XYDiagram xyDiagram1 = new DevExpress.XtraCharts.XYDiagram();
                            ((DevExpress.XtraCharts.XYDiagram)ChartControl.Diagram).AxisX.Range.SideMarginsEnabled = true;
                            ((DevExpress.XtraCharts.XYDiagram)ChartControl.Diagram).AxisX.Range.Auto = false;

                        }
                    }
                }
            }

        }

        private void BtnReset_Click(object sender, EventArgs e)
        {

            chkParty.UnCheckAll();
            chkShape.UnCheckAll();
            chkColor.UnCheckAll();
            chkClarity.UnCheckAll();
            chkCut.UnCheckAll();
            chkPolish.UnCheckAll();
            chkSymm.UnCheckAll();
            chkFlou.UnCheckAll();
            chkLab.UnCheckAll();
            chkLocation.UnCheckAll();
            chkCountry.UnCheckAll();
            chkSeller.UnCheckAll();
        }


    }
}
