using BusLib.Transaction;
using DevExpress.Data.Filtering;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MahantExport
{
    public partial class FrmstonePriceHistoryComparision: DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();

        private bool isPasteAction = false;
        private const Keys PasteKeys = Keys.Control | Keys.V;

        DataTable DTabData = new DataTable();
        DataTable DTabDataTotal = new DataTable();

        DataTable DTabFinalData = new DataTable();
        DataTable DTabFinalDataTotal = new DataTable();
        private GridBand gridBand;

        public FrmstonePriceHistoryComparision()
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

        private void BtnExportRapaport_Click(object sender, EventArgs e)
        {
            try
            {
               
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }
        private string GetSelectedRadioButtonText(GroupBox groupBox)
        {
            RadioButton checkedRadioButton = groupBox.Controls
                .OfType<RadioButton>()
                .FirstOrDefault(rb => rb.Checked);
            string StrType = Val.ToString(checkedRadioButton?.Tag);
            return StrType;
        }
        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                PanelProgress.Visible = true;

                string StrFromDate = null;
                string StrToDate = null;
                string StrStoneNo = "";


                if (DTPFromDate.Checked == true)
                {
                    StrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
                }
                if (DTPToDate.Checked == true)
                {
                    StrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());
                }
                string StrReporytType = GetSelectedRadioButtonText(GrpViewType);
                StrStoneNo = Val.ToString(txtStoneNo.Text);

                // Ensure data source is cleared before reloading data
                MainGrid.DataSource = null;
                GrdDet.RefreshData();


                // Fetch the data again
                DataSet DSet = ObjStock.GetStonePriceHistoryReportGetData(StrFromDate, StrToDate, StrReporytType, StrStoneNo);

                DTabData = DSet.Tables[1];
                DTabDataTotal = DSet.Tables[0];

                #region GrdDet
                // Clear previous bands and columns before adding new ones
                if (GrdDet.Bands.Count != 0)
                {
                    GrdDet.Bands.Clear();
                }
                GrdDet.Columns.Clear();  // Clear previous columns

                // Begin update to prevent intermediate rendering
                GrdDet.BeginUpdate();

                try
                {
                    // Create a new General band
                    gridBand = new GridBand
                    {
                        Name = "BandGeneral",
                        Caption = "General",
                        Tag = "General",
                        RowCount = 1,
                        VisibleIndex = 0,
                        Fixed = FixedStyle.Left
                    };
                    gridBand.AppearanceHeader.Font = new Font("Verdana", 10, FontStyle.Bold);
                    GrdDet.Bands.Add(gridBand);

                    // Create distinct bands
                    var distinctBandData = DTabData.AsEnumerable()
                        .Select(row => new
                        {
                            BandName = row.Field<string>("BANDNAME"),
                            SeqNo = row.Field<long>("BANDSEQNO").ToString()  // Convert Int64 to string
                        })
                        .Distinct();

                    DataTable DTabDistinct = new DataTable();
                    DTabDistinct.Columns.Add("BANDNAME", typeof(string));
                    DTabDistinct.Columns.Add("SEQNO", typeof(string));  // Change to string if BANDSEQNO is not int

                    foreach (var bandData in distinctBandData)
                    {
                        DTabDistinct.Rows.Add(bandData.BandName, bandData.SeqNo);
                    }

                    // Set up final data columns
                    DTabFinalData.Columns.Clear();
                    DTabFinalData.Rows.Clear();
                    DTabFinalData.Columns.Add(new DataColumn("STOCKNO", typeof(string)));
                    DTabFinalData.Columns.Add(new DataColumn("CARAT", typeof(double)));
                    DTabFinalData.Columns.Add(new DataColumn("SHAPE", typeof(string)));                    
                    DTabFinalData.Columns.Add(new DataColumn("COLOR", typeof(string)));
                    DTabFinalData.Columns.Add(new DataColumn("CLARITY", typeof(string)));
                    DTabFinalData.Columns.Add(new DataColumn("CUT", typeof(string)));
                    DTabFinalData.Columns.Add(new DataColumn("POL", typeof(string)));
                    DTabFinalData.Columns.Add(new DataColumn("SYM", typeof(string)));
                    DTabFinalData.Columns.Add(new DataColumn("DayCount", typeof(Int32)));
                    DTabFinalData.Columns.Add(new DataColumn("AvailableDate", typeof(string)));


                    foreach (DataRow DRow in DTabDistinct.Rows)
                    {
                        string Col = "BANDNAME_" + Val.ToString(DRow["BANDNAME"]) + "_";
                        DTabFinalData.Columns.Add(new DataColumn(Col + "Rappaport", typeof(double)));
                        DTabFinalData.Columns.Add(new DataColumn(Col + "Disc", typeof(double)));
                        DTabFinalData.Columns.Add(new DataColumn(Col + "Rate", typeof(double)));
                        DTabFinalData.Columns.Add(new DataColumn(Col + "Amount", typeof(double)));
                    }
                    MainGrid.DataSource = DTabFinalData;

                    // Set up General band columns
                    GrdDet.Columns["STOCKNO"].OwnerBand = gridBand;
                    GrdDet.Columns["STOCKNO"].Caption = "Stone No";
                    GrdDet.Columns["STOCKNO"].OptionsColumn.AllowEdit = false;
                    GrdDet.Columns["STOCKNO"].AppearanceHeader.Font = new Font(GrdDet.Columns["STOCKNO"].AppearanceHeader.Font, FontStyle.Bold);
                    GrdDet.Columns["STOCKNO"].AppearanceHeader.ForeColor = Color.Black;

                    GrdDet.Columns["CARAT"].OwnerBand = gridBand;
                    GrdDet.Columns["CARAT"].Caption = "Cts";
                    GrdDet.Columns["CARAT"].OptionsColumn.AllowEdit = false;
                    GrdDet.Columns["CARAT"].AppearanceHeader.Font = new Font(GrdDet.Columns["CARAT"].AppearanceHeader.Font, FontStyle.Bold);
                    GrdDet.Columns["CARAT"].AppearanceHeader.ForeColor = Color.Black;

                    GrdDet.Columns["SHAPE"].OwnerBand = gridBand;
                    GrdDet.Columns["SHAPE"].Caption = "Shape";
                    GrdDet.Columns["SHAPE"].OptionsColumn.AllowEdit = false;
                    GrdDet.Columns["SHAPE"].AppearanceHeader.Font = new Font(GrdDet.Columns["SHAPE"].AppearanceHeader.Font, FontStyle.Bold);
                    GrdDet.Columns["SHAPE"].AppearanceHeader.ForeColor = Color.Black;

                    GrdDet.Columns["COLOR"].OwnerBand = gridBand;
                    GrdDet.Columns["COLOR"].Caption = "Color";
                    GrdDet.Columns["COLOR"].OptionsColumn.AllowEdit = false;
                    GrdDet.Columns["COLOR"].AppearanceHeader.Font = new Font(GrdDet.Columns["COLOR"].AppearanceHeader.Font, FontStyle.Bold);
                    GrdDet.Columns["COLOR"].AppearanceHeader.ForeColor = Color.Black;

                    GrdDet.Columns["CLARITY"].OwnerBand = gridBand;
                    GrdDet.Columns["CLARITY"].Caption = "Clarity";
                    GrdDet.Columns["CLARITY"].OptionsColumn.AllowEdit = false;
                    GrdDet.Columns["CLARITY"].AppearanceHeader.Font = new Font(GrdDet.Columns["CLARITY"].AppearanceHeader.Font, FontStyle.Bold);
                    GrdDet.Columns["CLARITY"].AppearanceHeader.ForeColor = Color.Black;

                    GrdDet.Columns["CUT"].OwnerBand = gridBand;
                    GrdDet.Columns["CUT"].Caption = "Cut";
                    GrdDet.Columns["CUT"].OptionsColumn.AllowEdit = false;
                    GrdDet.Columns["CUT"].AppearanceHeader.Font = new Font(GrdDet.Columns["CUT"].AppearanceHeader.Font, FontStyle.Bold);
                    GrdDet.Columns["CUT"].AppearanceHeader.ForeColor = Color.Black;

                    GrdDet.Columns["POL"].OwnerBand = gridBand;
                    GrdDet.Columns["POL"].Caption = "Pol";
                    GrdDet.Columns["POL"].OptionsColumn.AllowEdit = false;
                    GrdDet.Columns["POL"].AppearanceHeader.Font = new Font(GrdDet.Columns["POL"].AppearanceHeader.Font, FontStyle.Bold);
                    GrdDet.Columns["POL"].AppearanceHeader.ForeColor = Color.Black;

                    GrdDet.Columns["SYM"].OwnerBand = gridBand;
                    GrdDet.Columns["SYM"].Caption = "Sym";
                    GrdDet.Columns["SYM"].OptionsColumn.AllowEdit = false;
                    GrdDet.Columns["SYM"].AppearanceHeader.Font = new Font(GrdDet.Columns["SYM"].AppearanceHeader.Font, FontStyle.Bold);
                    GrdDet.Columns["SYM"].AppearanceHeader.ForeColor = Color.Black;

                    GrdDet.Columns["DayCount"].OwnerBand = gridBand;
                    GrdDet.Columns["DayCount"].Caption = "Day Count";
                    GrdDet.Columns["DayCount"].OptionsColumn.AllowEdit = false;
                    GrdDet.Columns["DayCount"].AppearanceHeader.Font = new Font(GrdDet.Columns["DayCount"].AppearanceHeader.Font, FontStyle.Bold);
                    GrdDet.Columns["DayCount"].AppearanceHeader.ForeColor = Color.Black;

                    GrdDet.Columns["AvailableDate"].OwnerBand = gridBand;
                    GrdDet.Columns["AvailableDate"].Caption = "Available Date";
                    GrdDet.Columns["AvailableDate"].OptionsColumn.AllowEdit = false;
                    GrdDet.Columns["AvailableDate"].AppearanceHeader.Font = new Font(GrdDet.Columns["AvailableDate"].AppearanceHeader.Font, FontStyle.Bold);
                    GrdDet.Columns["AvailableDate"].AppearanceHeader.ForeColor = Color.Black;


                    // Add bands and assign columns
                    foreach (DataRow DRow in DTabDistinct.Rows)
                    {
                        gridBand = new GridBand
                        {
                            Name = "BANDNAME_" + Val.ToString(DRow["BANDNAME"]),
                            Caption = Val.ToString(DRow["BANDNAME"]),
                            RowCount = 1,
                            Tag = Val.ToString(DRow["BANDNAME"]),
                            VisibleIndex = Val.ToInt32(DRow["SEQNO"])
                        };
                        gridBand.AppearanceHeader.Font = new Font("Verdana", 10, FontStyle.Bold);
                        GrdDet.Bands.Add(gridBand);

                        string Col = "BANDNAME_" + Val.ToString(DRow["BANDNAME"]) + "_";

                        // Check and assign columns if they exist
                        if (GrdDet.Columns[Col + "Rappaport"] != null)
                        {
                            GrdDet.Columns[Col + "Rappaport"].OwnerBand = gridBand;
                            GrdDet.Columns[Col + "Rappaport"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            GrdDet.Columns[Col + "Rappaport"].AppearanceHeader.Font = new Font(GrdDet.Columns[Col + "Rappaport"].AppearanceHeader.Font, FontStyle.Bold);
                            GrdDet.Columns[Col + "Rappaport"].AppearanceHeader.ForeColor = Color.Black;
                            GrdDet.Columns[Col + "Rappaport"].Caption = "Rap";
                            GrdDet.Columns[Col + "Rappaport"].OptionsColumn.AllowEdit = false;
                        }

                        if (GrdDet.Columns[Col + "Disc"] != null)
                        {
                            GrdDet.Columns[Col + "Disc"].OwnerBand = gridBand;
                            GrdDet.Columns[Col + "Disc"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            GrdDet.Columns[Col + "Disc"].AppearanceHeader.Font = new Font(GrdDet.Columns[Col + "Disc"].AppearanceHeader.Font, FontStyle.Bold);
                            GrdDet.Columns[Col + "Disc"].AppearanceHeader.ForeColor = Color.Black;
                            GrdDet.Columns[Col + "Disc"].Caption = "Disc %";
                            GrdDet.Columns[Col + "Disc"].OptionsColumn.AllowEdit = false;
                        }

                        if (GrdDet.Columns[Col + "Rate"] != null)
                        {
                            GrdDet.Columns[Col + "Rate"].OwnerBand = gridBand;
                            GrdDet.Columns[Col + "Rate"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            GrdDet.Columns[Col + "Rate"].AppearanceHeader.Font = new Font(GrdDet.Columns[Col + "Rate"].AppearanceHeader.Font, FontStyle.Bold);
                            GrdDet.Columns[Col + "Rate"].AppearanceHeader.ForeColor = Color.Black;
                            GrdDet.Columns[Col + "Rate"].Caption = "Rate";
                            GrdDet.Columns[Col + "Rate"].OptionsColumn.AllowEdit = false;
                        }
                        if (GrdDet.Columns[Col + "Amount"] != null)
                        {
                            GrdDet.Columns[Col + "Amount"].OwnerBand = gridBand;
                            GrdDet.Columns[Col + "Amount"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            GrdDet.Columns[Col + "Amount"].AppearanceHeader.Font = new Font(GrdDet.Columns[Col + "Amount"].AppearanceHeader.Font, FontStyle.Bold);
                            GrdDet.Columns[Col + "Amount"].AppearanceHeader.ForeColor = Color.Black;
                            GrdDet.Columns[Col + "Amount"].Caption = "Amt";
                            GrdDet.Columns[Col + "Amount"].OptionsColumn.AllowEdit = false;
                        }

                    }

                    for (int i = 0; i < GrdDet.Columns.Count; i++)
                    {
                        GrdDet.Columns[i].OptionsFilter.FilterPopupMode = FilterPopupMode.CheckedList;
                    }

                    DataTable DTabStoneNo = DTabData.DefaultView.ToTable(true, "STOCKNO");
                    foreach (DataRow DRow in DTabStoneNo.Rows)
                    {

                        DataRow DRNew = DTabFinalData.NewRow();

                        DRNew["STOCKNO"] = Val.ToString(DRow["STOCKNO"]);


                        string StrQueryPol = "STOCKNO = '" + Val.ToString(DRow["STOCKNO"]) + "'";

                        foreach (DataRow DRowPrd in DTabDistinct.Rows)
                        {

                            string Col = "BANDNAME_" + Val.ToString(DRowPrd["BANDNAME"]) + "_";

                            string StrParty = Val.ToString(DRow["STOCKNO"]);
                            string StrBandname = Val.ToString(DRowPrd["BANDNAME"]);

                            string StrQuery = "BANDNAME = '" + StrBandname + "' AND  STOCKNO = '" + StrParty + "'";

                            DataRow[] UDROW = DTabData.Select(StrQuery);

                            if (UDROW != null)
                            {
                                foreach (DataRow dddd in UDROW)
                                {
                                    DRNew["STOCKNO"] = Val.ToString(dddd["STOCKNO"]);
                                    DRNew["CARAT"] = Val.ToDouble(dddd["CARAT"]);
                                    DRNew["SHAPE"] = Val.ToString(dddd["SHAPE"]);
                                    DRNew["COLOR"] = Val.ToString(dddd["COLOR"]);
                                    DRNew["CLARITY"] = Val.ToString(dddd["CLARITY"]);
                                    DRNew["CUT"] = Val.ToString(dddd["CUT"]);
                                    DRNew["POL"] = Val.ToString(dddd["POL"]);
                                    DRNew["SYM"] = Val.ToString(dddd["SYM"]);
                                    DRNew["AvailableDate"] = Val.ToString(dddd["AvailableDate"]);
                                    DRNew["DayCount"] = Val.ToInt32(dddd["DayCount"]);


                                    DRNew[Col + "Rappaport"] = Val.Val(dddd["RAPPAPORT"] != DBNull.Value ? dddd["RAPPAPORT"] : 0);
                                    DRNew[Col + "Rate"] = Val.Val(dddd["PRICEPERCARAT"] != DBNull.Value ? dddd["PRICEPERCARAT"] : 0);
                                    DRNew[Col + "Disc"] = Val.Val(dddd["DISCOUNT"] != DBNull.Value ? dddd["DISCOUNT"] : 0);
                                    DRNew[Col + "Amount"] = Val.Val(dddd["AMOUNT"] != DBNull.Value ? dddd["AMOUNT"] : 0);
                                    
                                }
                            }
                            UDROW = null;
                        }
                        DTabFinalData.Rows.Add(DRNew);
                    }

                }
                finally
                {
                    // End update to resume rendering
                    GrdDet.EndUpdate();
                }

                MainGrid.DataSource = DTabFinalData;
                GrdDet.Columns["STOCKNO"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "STOCKNO", "Total: {0:n0}");
                GrdDet.Columns["CARAT"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "CARAT", "{0:n2}");

                GrdDet.BestFitColumns();
                #endregion

                PanelProgress.Visible = false;

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

      
        private void GrdDet_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000")
                {
                    e.DisplayText = String.Empty;
                }

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDet_CustomDrawBandHeader(object sender, BandHeaderCustomDrawEventArgs e)
        {
            try
            {
                if (e.Band == gridBand)
                {
                    //e.Appearance.ForeColor = Color.Black;
                }

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtStoneNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                
                    String str1 = Val.ToString(txtStoneNo.Text);
                    string result = Regex.Replace(str1, @"\r\n?|\n", ",");
                    if (result.EndsWith(",,"))
                    {
                        result = result.Remove(result.Length - 1);
                    }
                    txtStoneNo.Text = result;
                    txtStoneNo.Select(txtStoneNo.Text.Length, 0);
               
                if (isPasteAction)
                {
                    isPasteAction = false;
                    txtStoneNo.Select(txtStoneNo.Text.Length, 0);
                }
            }
            catch (Exception EX)
            {
                Global.MessageError(EX.Message);
                return;
            }
        }

        private void txtStoneNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                isPasteAction = true;
            }
        }
    }
}