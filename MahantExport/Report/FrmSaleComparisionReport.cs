using BusLib.Transaction;
using DevExpress.XtraCharts;
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
    public partial class FrmSaleComparisionReport : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        public FrmSaleComparisionReport()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

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
        private string GetSelectedRadioButtonText(GroupBox groupBox)
        {
            RadioButton checkedRadioButton = groupBox.Controls
                .OfType<RadioButton>()
                .FirstOrDefault(rb => rb.Checked);
            string StrType = Val.ToString(checkedRadioButton?.Tag);
            return StrType;
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
            else if (ViewType == ViewType.Bar)
            {
                List<Color> colors = new List<Color> {
                            ColorTranslator.FromHtml("#eb7a58"), // Light Peach color
                            ColorTranslator.FromHtml("#bdaf2c"), // Olive Green color
                            ColorTranslator.FromHtml("#f68738"), // Orange color                            
                            ColorTranslator.FromHtml("#d3b28d"),  // Muted Beige color
                            ColorTranslator.FromHtml("#f8bc6b"), // Light Brown color
                            ColorTranslator.FromHtml("#ffc7af"), // Light Red color
                            ColorTranslator.FromHtml("#decc41"), // Yellowish color
                            ColorTranslator.FromHtml("#864c38"), // Dark Brown color
                            // Add more colors as needed
                        };

                DataView view = dt.Copy().DefaultView;
                DataTable distinctValues = view.ToTable(true, X, Y);
                DataTable Xtable = distinctValues.Copy().DefaultView.ToTable(true, X);

                // Clear any existing series and set up the new series
                ChartControl.Series.Clear();
                Series Pie3DSeries = new Series("", ViewType);

                // Group the data as before
                var ownerGroups = distinctValues.AsEnumerable()
                    .GroupBy(row => row.Field<string>(X));

                var dt2 = distinctValues.Clone();
                dt2.Columns[Y].DataType = typeof(decimal);
                var intColumns = dt2.Columns.Cast<DataColumn>()
                    .Where(c => c.DataType == typeof(decimal)).ToArray();

                int colorIndex = 0;
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

                        // Add a data point with the custom color
                        SeriesPoint point = new SeriesPoint(grp.Key, row[Y]);
                        point.Color = colors[colorIndex % colors.Count];  // Cycle through colors if there are more groups than colors
                        Pie3DSeries.Points.Add(point);

                        colorIndex++;
                    }
                }

                // Set up the series in the chart control
                ChartControl.Series.Add(Pie3DSeries);
                Pie3DSeries.ArgumentScaleType = ScaleType.Auto;
                Pie3DSeries.ArgumentDataMember = X;
                Pie3DSeries.ValueScaleType = ScaleType.Numerical;
                Pie3DSeries.ValueDataMembers.AddRange(new string[] { Y });

                ChartControl.Legend.Visible = false;
                Pie3DSeries.Label.PointOptions.PointView = PointView.ArgumentAndValues;
                Pie3DSeries.Label.PointOptions.ValueNumericOptions.Format = NumericFormat.Percent;
                Pie3DSeries.Label.PointOptions.ValueNumericOptions.Precision = 0;

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

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                
                string StrFromDate = null;
                string StrToDate = null;

                string StrReporytType = GetSelectedRadioButtonText(GrpViewType);

                if (DTPFromDate.Checked == true)
                {
                    StrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
                }
                if (DTPToDate.Checked == true)
                {
                    StrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());
                }

                DataSet DSet = ObjStock.GetSaleReportComparisionData(StrReporytType, StrFromDate, StrToDate);
               
                MainGridRapaport.DataSource = DSet.Tables[1];                
                GrdDet.BestFitColumns();

                lblViewType.Text = Val.ToString(DSet.Tables[2].Rows[0]["VIEWTYPE"]);

                DispalyChart(ChartDateWise, ViewType.Bar, DSet.Tables[2], "PARTICULAR", "PCS");

                GrdDet.Bands["BAND1"].Caption = Val.ToString(DSet.Tables[0].Rows[0]["PARTICULAR"]);
                GrdDet.Bands["BAND2"].Caption = Val.ToString(DSet.Tables[0].Rows[1]["PARTICULAR"]);
                GrdDet.Bands["BAND3"].Caption = Val.ToString(DSet.Tables[0].Rows[2]["PARTICULAR"]);
                GrdDet.Bands["BAND4"].Caption = Val.ToString(DSet.Tables[0].Rows[3]["PARTICULAR"]);
                GrdDet.Bands["BAND5"].Caption = Val.ToString(DSet.Tables[0].Rows[4]["PARTICULAR"]);

                this.Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnExportRapaport_Click(object sender, EventArgs e)
        {
            try
            {
                Global.ExcelExport("Sale Comparision View",GrdDet);
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnRapaportExit_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }
    }
}