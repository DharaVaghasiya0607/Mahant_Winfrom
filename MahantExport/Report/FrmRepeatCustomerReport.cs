using BusLib.Transaction;
using DevExpress.Data.Filtering;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections;
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
    public partial class FrmRepeatCustomerReport : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();

        DataTable DTabData = new DataTable();

        DataTable DTabFinalData = new DataTable();
        DataTable DTabFinalDataTotal = new DataTable();

        public FrmRepeatCustomerReport()
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

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                PanelProgress.Visible = true;

                string StrFromDate = null;
                string StrToDate = null;

                if (DTPFromDate.Checked == true)
                {
                    StrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
                }
                if (DTPToDate.Checked == true)
                {
                    StrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());
                }

                // Ensure data source is cleared before reloading data
                MainGrid.DataSource = null;
                GrdDet.RefreshData();


                // Fetch the data again
                DataSet DSet = ObjStock.GetRepeatCustomerReportGetData(StrFromDate, StrToDate);

                DTabData = DSet.Tables[0];
                
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
                    var gridBand = new GridBand
                    {
                        Name = "BandGeneral",
                        Caption = "General",
                        Tag = "General",
                        RowCount = 1,
                        VisibleIndex = 0,
                        Fixed = FixedStyle.Left,
                        AppearanceHeader =
                            {
                                ForeColor = Color.White, // Set header text color to black
                                Font = new Font("Verdana", 8.25F, FontStyle.Bold) ,// Set header font to bold
                                BackColor = Color.FromArgb(53, 61, 84)
                            }
                    };
                    gridBand.AppearanceHeader.ForeColor = Color.White; // Set header text color to black
                    gridBand.AppearanceHeader.Font = new Font("Verdana", 8.25F, FontStyle.Bold); // Set header font to bold

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
                    DTabFinalData.Columns.Add(new DataColumn("PARTYNAME", typeof(string)));
                    DTabFinalData.Columns.Add(new DataColumn("IsNewParty", typeof(bool)));
                    DTabFinalData.Columns.Add(new DataColumn("IsTotalParty", typeof(bool)));

                    foreach (DataRow DRow in DTabDistinct.Rows)
                    {
                        string Col = "BANDNAME_" + Val.ToString(DRow["BANDNAME"]) + "_";
                        DTabFinalData.Columns.Add(new DataColumn(Col + "Amount", typeof(double)));
                        DTabFinalData.Columns.Add(new DataColumn(Col + "Pcs", typeof(int)));
                        DTabFinalData.Columns.Add(new DataColumn(Col + "DiffAmount", typeof(double)));
                        DTabFinalData.Columns.Add(new DataColumn(Col + "DiffAmountPer", typeof(double)));
                        DTabFinalData.Columns.Add(new DataColumn(Col + "IsExistsParty", typeof(bool)));
                        DTabFinalData.Columns.Add(new DataColumn(Col + "ISNoBuying", typeof(bool)));
                    }
                    MainGrid.DataSource = DTabFinalData;

                    // Set up General band columns
                    GrdDet.Columns["PARTYNAME"].OwnerBand = gridBand;
                    GrdDet.Columns["PARTYNAME"].Caption = "Party";
                    GrdDet.Columns["PARTYNAME"].OptionsColumn.AllowEdit = false;
                    //GrdDet.Columns["PARTYNAME"].AppearanceCell.ForeColor = Color.Black;
                    GrdDet.Columns["PARTYNAME"].AppearanceHeader.ForeColor = Color.Black;
                    GrdDet.Columns["PARTYNAME"].AppearanceHeader.Font = new Font(GrdDet.Columns["PARTYNAME"].AppearanceHeader.Font, FontStyle.Bold);

                    GrdDet.Columns["IsNewParty"].OwnerBand = gridBand;
                    GrdDet.Columns["IsNewParty"].Caption = "IsNewParty";
                    GrdDet.Columns["IsNewParty"].OptionsColumn.AllowEdit = false;

                    GrdDet.Columns["IsNewParty"].Visible = false;
                    GrdDet.Columns["IsTotalParty"].Visible = false;
                    GrdDet.Columns["IsTotalParty"].SortMode = ColumnSortMode.Custom;

                    // Add bands and assign columns
                    foreach (DataRow DRow in DTabDistinct.Rows)
                    {
                        gridBand = new GridBand
                        {
                            Name = "BANDNAME_" + Val.ToString(DRow["BANDNAME"]),
                            Caption = Val.ToString(DRow["BANDNAME"]),
                            RowCount = 1,
                            Tag = Val.ToString(DRow["BANDNAME"]),
                            VisibleIndex = Val.ToInt32(DRow["SEQNO"]),
                            AppearanceHeader =
                            {
                                ForeColor = Color.White, // Set header text color to black
                                Font = new Font("Verdana", 8.25F, FontStyle.Bold), // Set header font to bold
                                BackColor = Color.FromArgb(53, 61, 84)
                            }
                        };


                        GrdDet.Bands.Add(gridBand);

                        string Col = "BANDNAME_" + Val.ToString(DRow["BANDNAME"]) + "_";


                        // Check and assign columns if they exist
                        if (GrdDet.Columns[Col + "Amount"] != null)
                        {
                            GrdDet.Columns[Col + "Amount"].OwnerBand = gridBand;
                            GrdDet.Columns[Col + "Amount"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            //GrdDet.Columns[Col + "Amount"].AppearanceCell.ForeColor = Color.Black;
                            GrdDet.Columns[Col + "Amount"].AppearanceHeader.ForeColor = Color.Black;
                            GrdDet.Columns[Col + "Amount"].AppearanceHeader.Font = new Font(GrdDet.Columns[Col + "Amount"].AppearanceHeader.Font, FontStyle.Bold);

                            GrdDet.Columns[Col + "Amount"].Caption = "Amt";
                            GrdDet.Columns[Col + "Amount"].OptionsColumn.AllowEdit = false;


                            //// Apply grid format rule for the "Amount" column
                            //var gridFormatRule = new DevExpress.XtraGrid.GridFormatRule();
                            //var formatConditionIconSet = new DevExpress.XtraEditors.FormatConditionRuleIconSet();
                            //var iconSet = new DevExpress.XtraEditors.FormatConditionIconSet
                            //{
                            //    ValueType = DevExpress.XtraEditors.FormatConditionValueType.Number,
                            //    CategoryName = "Positive/Negative"
                            //};

                            //// Configure icons
                            //var icon1 = new DevExpress.XtraEditors.FormatConditionIconSetIcon
                            //{
                            //    PredefinedName = "Arrows3_3.png",
                            //    Value = -1000m,
                            //    ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.GreaterOrEqual
                            //};
                            //var icon2 = new DevExpress.XtraEditors.FormatConditionIconSetIcon
                            //{
                            //    PredefinedName = "Arrows3_2.png",
                            //    Value = 0m,
                            //    ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.GreaterOrEqual
                            //};
                            //var icon3 = new DevExpress.XtraEditors.FormatConditionIconSetIcon
                            //{
                            //    PredefinedName = "Arrows3_1.png",
                            //    Value = 1000m,
                            //    ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.GreaterOrEqual
                            //};

                            //iconSet.Icons.Add(icon1);
                            //iconSet.Icons.Add(icon2);
                            //iconSet.Icons.Add(icon3);

                            //// Assign icon set to the rule
                            //formatConditionIconSet.IconSet = iconSet;

                            //// Link the rule to the column
                            //gridFormatRule.Rule = formatConditionIconSet;
                            //gridFormatRule.Column = GrdDet.Columns[Col + "Amount"];
                            //GrdDet.FormatRules.Add(gridFormatRule);

                        }

                        if (GrdDet.Columns[Col + "Pcs"] != null)
                        {
                            GrdDet.Columns[Col + "Pcs"].OwnerBand = gridBand;
                            GrdDet.Columns[Col + "Pcs"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            GrdDet.Columns[Col + "Pcs"].AppearanceHeader.ForeColor = Color.Black;
                            GrdDet.Columns[Col + "Pcs"].AppearanceHeader.Font = new Font(GrdDet.Columns[Col + "Pcs"].AppearanceHeader.Font, FontStyle.Bold);
                            GrdDet.Columns[Col + "Pcs"].Caption = "Pcs";
                            GrdDet.Columns[Col + "Pcs"].OptionsColumn.AllowEdit = false;

                            //// Configure Format Rules
                            //var gridFormatRule = new DevExpress.XtraGrid.GridFormatRule();
                            //var formatConditionIconSet = new DevExpress.XtraEditors.FormatConditionRuleIconSet();
                            //var iconSet = new DevExpress.XtraEditors.FormatConditionIconSet
                            //{
                            //    ValueType = DevExpress.XtraEditors.FormatConditionValueType.Number,
                            //    CategoryName = "Positive/Negative"
                            //};

                            //// Configure icons for PCS
                            //var icon1 = new DevExpress.XtraEditors.FormatConditionIconSetIcon
                            //{
                            //    PredefinedName = "Arrows3_3.png", // Down arrow for values < 0
                            //    Value = -1000m, // Minimum value
                            //    ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.GreaterOrEqual
                            //};
                            //var icon2 = new DevExpress.XtraEditors.FormatConditionIconSetIcon
                            //{
                            //    PredefinedName = "Arrows3_2.png", // Neutral arrow for values >= 0 and < 1000
                            //    Value = 0m,
                            //    ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.GreaterOrEqual
                            //};
                            //var icon3 = new DevExpress.XtraEditors.FormatConditionIconSetIcon
                            //{
                            //    PredefinedName = "Arrows3_1.png", // Up arrow for values >= 1000
                            //    Value = 1000m,
                            //    ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.GreaterOrEqual
                            //};

                            //iconSet.Icons.Add(icon1);
                            //iconSet.Icons.Add(icon2);
                            //iconSet.Icons.Add(icon3);

                            //formatConditionIconSet.IconSet = iconSet;
                            //gridFormatRule.Rule = formatConditionIconSet;
                            //gridFormatRule.Column = GrdDet.Columns[Col + "Pcs"];
                            //GrdDet.FormatRules.Add(gridFormatRule);
                        }


                        if (GrdDet.Columns[Col + "DiffAmount"] != null)
                        {
                            GrdDet.Columns[Col + "DiffAmount"].OwnerBand = gridBand;
                            GrdDet.Columns[Col + "DiffAmount"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            //GrdDet.Columns[Col + "DiffAmount"].AppearanceCell.ForeColor = Color.Black;
                            GrdDet.Columns[Col + "DiffAmount"].AppearanceHeader.ForeColor = Color.Black;
                            GrdDet.Columns[Col + "DiffAmount"].AppearanceHeader.Font = new Font(GrdDet.Columns[Col + "DiffAmount"].AppearanceHeader.Font, FontStyle.Bold);
                            GrdDet.Columns[Col + "DiffAmount"].Caption = "Diff";
                            GrdDet.Columns[Col + "DiffAmount"].OptionsColumn.AllowEdit = false;

                            // Apply grid format rule for the "Amount" column
                            var gridFormatRule = new DevExpress.XtraGrid.GridFormatRule();
                            var formatConditionIconSet = new DevExpress.XtraEditors.FormatConditionRuleIconSet();
                            var iconSet = new DevExpress.XtraEditors.FormatConditionIconSet
                            {
                                ValueType = DevExpress.XtraEditors.FormatConditionValueType.Number,
                                CategoryName = "Positive/Negative"
                            };

                            // Configure icons
                            var icon1 = new DevExpress.XtraEditors.FormatConditionIconSetIcon
                            {
                                PredefinedName = "Arrows3_3.png",
                                Value = -7922816251426433759,
                                ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.GreaterOrEqual
                            };
                            var icon2 = new DevExpress.XtraEditors.FormatConditionIconSetIcon
                            {
                                PredefinedName = "Arrows3_2.png",
                                Value = 0,
                                ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.GreaterOrEqual
                            };
                            var icon3 = new DevExpress.XtraEditors.FormatConditionIconSetIcon
                            {
                                PredefinedName = "Arrows3_1.png",
                                Value = 0,
                                ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.Greater
                            };

                            iconSet.Icons.Add(icon1);
                            iconSet.Icons.Add(icon2);
                            iconSet.Icons.Add(icon3);

                            // Assign icon set to the rule
                            formatConditionIconSet.IconSet = iconSet;

                            // Link the rule to the column
                            gridFormatRule.Rule = formatConditionIconSet;
                            gridFormatRule.Column = GrdDet.Columns[Col + "DiffAmount"];
                            GrdDet.FormatRules.Add(gridFormatRule);

                        }

                        if (GrdDet.Columns[Col + "DiffAmountPer"] != null)
                        {
                            GrdDet.Columns[Col + "DiffAmountPer"].OwnerBand = gridBand;
                            GrdDet.Columns[Col + "DiffAmountPer"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            //GrdDet.Columns[Col + "DiffAmountPer"].AppearanceCell.ForeColor = Color.Black;
                            GrdDet.Columns[Col + "DiffAmountPer"].AppearanceHeader.ForeColor = Color.Black;
                            GrdDet.Columns[Col + "DiffAmountPer"].AppearanceHeader.Font = new Font(GrdDet.Columns[Col + "DiffAmountPer"].AppearanceHeader.Font, FontStyle.Bold);
                            GrdDet.Columns[Col + "DiffAmountPer"].Caption = "Diff%";
                            GrdDet.Columns[Col + "DiffAmountPer"].OptionsColumn.AllowEdit = false;

                            // Apply grid format rule for the "Amount" column
                            var gridFormatRule1 = new DevExpress.XtraGrid.GridFormatRule();
                            var formatConditionIconSet1 = new DevExpress.XtraEditors.FormatConditionRuleIconSet();
                            var iconSet1 = new DevExpress.XtraEditors.FormatConditionIconSet
                            {
                                ValueType = DevExpress.XtraEditors.FormatConditionValueType.Number,
                                CategoryName = "Positive/Negative"
                            };

                            // Configure icons
                            var icon1 = new DevExpress.XtraEditors.FormatConditionIconSetIcon
                            {
                                PredefinedName = "Arrows3_3.png",
                                Value = -7922816251426433759,
                                ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.GreaterOrEqual
                            };
                            var icon2 = new DevExpress.XtraEditors.FormatConditionIconSetIcon
                            {
                                PredefinedName = "Arrows3_2.png",
                                Value = 0,
                                ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.GreaterOrEqual
                            };
                            var icon3 = new DevExpress.XtraEditors.FormatConditionIconSetIcon
                            {
                                PredefinedName = "Arrows3_1.png",
                                Value = 0,
                                ValueComparison = DevExpress.XtraEditors.FormatConditionComparisonType.Greater
                            };

                            iconSet1.Icons.Add(icon1);
                            iconSet1.Icons.Add(icon2);
                            iconSet1.Icons.Add(icon3);

                            // Assign icon set to the rule
                            formatConditionIconSet1.IconSet = iconSet1;

                            // Link the rule to the column
                            gridFormatRule1.Rule = formatConditionIconSet1;
                            gridFormatRule1.Column = GrdDet.Columns[Col + "DiffAmountPer"];
                            GrdDet.FormatRules.Add(gridFormatRule1);
                        }

                        if (GrdDet.Columns[Col + "IsExistsParty"] != null)
                        {
                            GrdDet.Columns[Col + "IsExistsParty"].OwnerBand = gridBand;
                            GrdDet.Columns[Col + "IsExistsParty"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            //GrdDet.Columns[Col + "IsExistsParty"].AppearanceCell.ForeColor = Color.Black;
                            GrdDet.Columns[Col + "IsExistsParty"].AppearanceHeader.ForeColor = Color.Black;
                            GrdDet.Columns[Col + "IsExistsParty"].AppearanceHeader.Font = new Font(GrdDet.Columns[Col + "IsExistsParty"].AppearanceHeader.Font, FontStyle.Bold);
                            GrdDet.Columns[Col + "IsExistsParty"].Caption = "IsExistsParty";
                            GrdDet.Columns[Col + "IsExistsParty"].OptionsColumn.AllowEdit = false;
                            GrdDet.Columns[Col + "IsExistsParty"].Visible = false;
                        }

                        if (GrdDet.Columns[Col + "ISNoBuying"] != null)
                        {
                            GrdDet.Columns[Col + "ISNoBuying"].OwnerBand = gridBand;
                            GrdDet.Columns[Col + "ISNoBuying"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                            //GrdDet.Columns[Col + "ISNoBuying"].AppearanceCell.ForeColor = Color.Black;
                            GrdDet.Columns[Col + "ISNoBuying"].AppearanceHeader.ForeColor = Color.Black;
                            GrdDet.Columns[Col + "ISNoBuying"].AppearanceHeader.Font = new Font(GrdDet.Columns[Col + "ISNoBuying"].AppearanceHeader.Font, FontStyle.Bold);
                            GrdDet.Columns[Col + "ISNoBuying"].Caption = "ISNoBuying";
                            GrdDet.Columns[Col + "ISNoBuying"].OptionsColumn.AllowEdit = false;
                            GrdDet.Columns[Col + "ISNoBuying"].Visible = false;
                        }
                    }

                    for (int i = 0; i < GrdDet.Columns.Count; i++)
                    {
                        GrdDet.Columns[i].OptionsFilter.FilterPopupMode = FilterPopupMode.CheckedList;
                    }

                    DataTable DTabStoneNo = DTabData.DefaultView.ToTable(true, "PARTYNAME");
                    foreach (DataRow DRow in DTabStoneNo.Rows)
                    {
                        DataRow DRNew = DTabFinalData.NewRow();

                        DRNew["PARTYNAME"] = Val.ToString(DRow["PARTYNAME"]);

                        string StrQueryPol = "PARTYNAME = '" + Val.ToString(DRow["PARTYNAME"]).Replace("'", "''") + "'";

                        foreach (DataRow DRowPrd in DTabDistinct.Rows)
                        {
                            string Col = "BANDNAME_" + Val.ToString(DRowPrd["BANDNAME"]) + "_";

                            string StrParty = Val.ToString(DRow["PARTYNAME"]).Replace("'", "''");
                            string StrBandname = Val.ToString(DRowPrd["BANDNAME"]).Replace("'", "''");

                            string StrQuery = "BANDNAME = '" + StrBandname + "' AND PARTYNAME = '" + StrParty + "'";

                            DataRow[] UDROW = DTabData.Select(StrQuery);

                            if (UDROW != null)
                            {
                                foreach (DataRow dddd in UDROW)
                                {
                                    DRNew["PARTYNAME"] = Val.ToString(dddd["PARTYNAME"]);
                                    DRNew["IsNewParty"] = Val.ToInt32(dddd["IsNewParty"]);
                                    DRNew["IsTotalParty"] = Val.ToInt32(dddd["IsTotalParty"]);

                                    DRNew[Col + "Amount"] = Val.Val(dddd["AMOUNT"] != DBNull.Value ? dddd["AMOUNT"] : 0);
                                    DRNew[Col + "Pcs"] = Val.Val(dddd["PCS"] != DBNull.Value ? dddd["PCS"] : 0);
                                    DRNew[Col + "DiffAmount"] = Val.Val(dddd["DiffAmount"] != DBNull.Value ? dddd["DiffAmount"] : 0);
                                    DRNew[Col + "DiffAmountPer"] = Val.Val(dddd["DiffAmountPer"] != DBNull.Value ? dddd["DiffAmountPer"] : 0);

                                    DRNew[Col + "IsExistsParty"] = dddd["IsExistsParty"] != DBNull.Value && !string.IsNullOrWhiteSpace(dddd["IsExistsParty"].ToString())
                                                                   ? Convert.ToBoolean(dddd["IsExistsParty"])
                                                                   : false;
                                    DRNew[Col + "ISNoBuying"] = dddd["ISNoBuying"] != DBNull.Value && !string.IsNullOrWhiteSpace(dddd["ISNoBuying"].ToString())
                                                                 ? Convert.ToBoolean(dddd["ISNoBuying"])
                                                                 : false;

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
                GrdDet.Columns["PARTYNAME"].Summary.Add(DevExpress.Data.SummaryItemType.Count, "PARTYNAME", "Total: {0:n0}");

                GrdDet.BestFitColumns();

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

                //if (e.Column.FieldName.Contains("Amount") && e.ListSourceRowIndex != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
                //{
                //    GridView view = sender as GridView;
                //    string partyName = view.GetListSourceRowCellValue(e.ListSourceRowIndex, "PARTYNAME").ToString();

                //    if (partyName == "Total")
                //    {
                //        e.DisplayText = string.Format("<a href='#'>{0}</a>", e.Value);
                //    }
                //}
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDet_RowStyle(object sender, RowStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }
                Int32 inttflag = Val.ToBooleanToInt(GrdDet.GetRowCellValue(e.RowHandle, "IsTotalParty"));
                if (inttflag == 1)
                {
                    e.Appearance.ForeColor = Color.Navy;
                    e.Appearance.Font = new Font(e.Appearance.Font, FontStyle.Bold);
                }
                else
                {
                    e.Appearance.ForeColor = Color.Black;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }
        private void GrdDet_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                if (e.Clicks == 2)
                {
                    if (e.Column.FieldName.Contains("_Amount"))
                    {
                        this.Cursor = Cursors.WaitCursor;

                        string strPartyname = Val.ToString(GrdDet.GetFocusedRowCellValue("PARTYNAME"));
                        string StrFieldName = e.Column.FieldName;

                        int lastUnderscoreIndex = StrFieldName.LastIndexOf('_');
                        string StrBand = StrFieldName.Substring(0, lastUnderscoreIndex + 1);

                        // Check if IsTotalParty is true for the current focused row
                        bool isTotalParty = Val.ToBoolean(GrdDet.GetFocusedRowCellValue("IsTotalParty"));
                        bool isNewParty = Val.ToBoolean(GrdDet.GetFocusedRowCellValue("IsNewParty"));

                        if (strPartyname.Contains("Total "))
                        {
                            GrdDet.Columns[e.Column.FieldName].ClearFilter();
                            GrdDet.ActiveFilter.Clear();

                            if (strPartyname == "Total Sale")
                            {
                                GrdDet.ActiveFilter.Clear();
                            }
                            if (strPartyname == "Total No of New Customer")
                            {
                                GrdDet.ActiveFilter.Clear();
                                GrdDet.Columns["IsNewParty"].ClearFilter();
                                //GrdDet.Columns["IsNewParty"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo("IsNewParty = true");
                                GrdDet.Columns["IsTotalParty"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo("IsTotalParty = true OR IsNewParty = true");
                            }
                            if (strPartyname == "Total No of Existing Customer")
                            {
                                string columnName = StrBand + "IsExistsParty";

                                if (GrdDet.Columns[columnName] != null)
                                {
                                    GrdDet.Columns[columnName].ClearFilter();
                                    //GrdDet.Columns[columnName].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo($"[{columnName}] = true"); 
                                    GrdDet.Columns["IsTotalParty"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo($"IsTotalParty = true OR [{columnName}] = true");
                                }
                            }
                            if (strPartyname == "Total Sale of New Customer")
                            {
                                GrdDet.ActiveFilter.Clear();
                                GrdDet.Columns["IsNewParty"].ClearFilter();
                                //GrdDet.Columns["IsNewParty"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo("IsNewParty = true");
                                GrdDet.Columns["IsTotalParty"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo("IsTotalParty = true OR IsNewParty = true");
                            }
                            if (strPartyname == "Total Sale of Existing Customer")
                            {
                                string columnName = StrBand + "IsExistsParty";

                                if (GrdDet.Columns[columnName] != null)
                                {
                                    GrdDet.Columns[columnName].ClearFilter();
                                    //GrdDet.Columns[columnName].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo($"[{columnName}] = true");
                                    GrdDet.Columns["IsTotalParty"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo($"IsTotalParty = true OR [{columnName}] = true");
                                }
                            }
                            if (strPartyname == "Total No of No Buying Customer")
                            {
                                string columnName = StrBand + "ISNoBuying";

                                if (GrdDet.Columns[columnName] != null)
                                {
                                    GrdDet.Columns[columnName].ClearFilter();
                                    //GrdDet.Columns[columnName].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo($"[{columnName}] = true");
                                    GrdDet.Columns["IsTotalParty"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo($"IsTotalParty = true OR [{columnName}] = true");
                                }
                            }

                            // Set the OR condition filter for IsTotalParty or IsNewParty
                           
                        }

                        this.Cursor = Cursors.Default;
                    }
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
                if (e.Band != null) // Check for the specific band
                {
                    e.Appearance.ForeColor = Color.Black; // Set text color to black
                    e.Appearance.Font = new Font("Verdana", 8.25F, FontStyle.Bold); // Set font to bold
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDet_ShowingEditor(object sender, CancelEventArgs e)
        {
            try
            {
                var gridView = sender as GridView;
                if (gridView != null)
                {
                    bool isTotalParty = Convert.ToBoolean(gridView.GetRowCellValue(gridView.FocusedRowHandle, "IsTotalParty"));
                    if (isTotalParty)
                    {
                        e.Cancel = true; // Prevent editing
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDet_CustomColumnSort(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnSortEventArgs e)
        {
            try
            {
                if (e.Column.FieldName == "IsTotalParty")
                {
                    bool val1 = Convert.ToBoolean(e.Value1);
                    bool val2 = Convert.ToBoolean(e.Value2);

                    // Place rows with IsTotalParty = true at the top
                    if (val1 && !val2)
                    {
                        e.Result = -1; // val1 comes before val2
                    }
                    else if (!val1 && val2)
                    {
                        e.Result = 1; // val2 comes before val1
                    }
                    else
                    {
                        e.Result = Comparer.Default.Compare(e.Value1, e.Value2); // Default sorting for others
                    }

                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDet_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0) // Only for data rows
                {
                    // Check if IsTotalParty is true
                    var isTotalParty = Convert.ToBoolean(GrdDet.GetRowCellValue(e.RowHandle, "IsTotalParty"));

                    if (isTotalParty)
                    {
                        // Color for rows where IsTotalParty is true
                        e.Appearance.BackColor = Color.FromArgb(217, 226, 241);
                    }
                    else
                    {
                        // Retrieve the column's owner band
                        var column = GrdDet.Columns[e.Column.FieldName];
                        var band = column?.OwnerBand;

                        if (band != null)
                        {
                            string bandName = band.Caption; // Use the band caption or a similar identifier


                            if (bandName == "General") // Check if the column belongs to BandGeneral
                            {
                                bool isEvenRow = e.RowHandle % 2 == 0;

                                // Apply coloring based on row number (even/odd)
                                if (isEvenRow)
                                {
                                    e.Appearance.BackColor = Color.FromArgb(223, 235, 248); // Even row color for BandGeneral
                                }
                                else
                                {
                                    e.Appearance.BackColor = Color.Transparent; // Odd row - transparent
                                }
                            }
                            else
                            {
                                // Determine band index for alternating colors
                                int bandIndex = GrdDet.Bands.IndexOf(band);
                                bool isEvenRow = e.RowHandle % 2 == 0;

                                switch (bandIndex % 2)
                                {
                                    case 0:
                                        if (isEvenRow)
                                        {
                                            // First band
                                            e.Appearance.BackColor = Color.FromArgb(223, 235, 248);
                                        }
                                        else
                                        {
                                            e.Appearance.BackColor = Color.Transparent;
                                        }
                                        break; // Break here to avoid falling through to the next case

                                    case 1:
                                        if (isEvenRow)
                                        {// Second band
                                            e.Appearance.BackColor = Color.FromArgb(223, 235, 248);
                                        }
                                        else
                                        {
                                            e.Appearance.BackColor = Color.Transparent;
                                        }
                                        break;

                                    case 2:
                                        if (isEvenRow)
                                        {// Third band (repeats first color)
                                            e.Appearance.BackColor = Color.FromArgb(223, 235, 248);
                                        }
                                        else
                                        {
                                            e.Appearance.BackColor = Color.Transparent;
                                        }
                                        break;
                                }

                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
    }
}
