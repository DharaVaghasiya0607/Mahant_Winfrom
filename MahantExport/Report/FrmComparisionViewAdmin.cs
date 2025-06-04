using BusLib;
using BusLib.Configuration;
using BusLib.Attendance;
using BusLib.Transaction;
using DevExpress.XtraPrinting;
using Google.API.Translate;
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
using BusLib.TableName;
using MahantExport.Utility;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using BusLib.Master;
using MahantExport.Stock;
using BusLib.ReportGrid;
using DevExpress.XtraGrid.Views.BandedGrid;
using System.Reflection;
using DevExpress.Data;
using DevExpress.XtraPrintingLinks;
using System.Drawing.Printing;
using BusLib.Report;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.Utils;
using BusLib.Rapaport;
using BusLib.View;
using DevExpress.XtraPivotGrid;
using BusLib.Account;
using OfficeOpenXml;

namespace MahantExport.Masters
{
    public partial class FrmComparisionViewAdmin : DevControlLib.cDevXtraForm
    {
        BOFindRap ObjRap = new BOFindRap();
        public delegate void SetControlValueCallback(Control oControl, string propName, object propValue);

        String PasteData = "";
        IDataObject PasteclipData = Clipboard.GetDataObject();

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_PredictionView ObjView = new BOTRN_PredictionView();
        BOTRN_RunninPossition ObjTra = new BOTRN_RunninPossition();
        BOLedgerTransaction objLedgerTrn = new BOLedgerTransaction();

        DataTable DTabPredictionView = new DataTable();
        DataTable DTabPrdType = new DataTable();
        DataTable DTabPacketDetail = new DataTable();
        DataTable DTabPrdDetail = new DataTable();
        DataTable DTabPrdSummary = new DataTable();
        DataTable DTabPolishOkDetail = new DataTable();
        DataTable DTabDistinct = new DataTable();
        DataTable DtabBackPriceUpdate = new DataTable();

        DataSet DGrd = new DataSet();

        string mStrClientRefNo = "";
        string mStrPredictionType = "";
        int mIntPacketTag = 0;
        string mStrPredictionTypeOther = "";
        string StrFromDate = null;
        string StrToDate = null;
        string StrEmployee_ID = "";
        string StrFilePath = "";


        int mIntFromPacketNo = 0;
        int mIntToPacketNo = 0;
        int mIntEmpCode = 0;

        int mIntRefPacketNo = 0;

        string mStrKapan = "";
        string mStrTag = "";
        string mStrParentTag = "";
        Int64 mIntEmployeeID = 0;
        string mStrMainTag = "";
        string StrOpe = "";
        string StrType = "";

        double Amount = 0;
        double Carat = 0;


        public FORMTYPE mFormType = FORMTYPE.ADMIN;

        public enum FORMTYPE
        {
            ADMIN = 0,
            MARKER = 1
        }


        #region Property Settings

        public FrmComparisionViewAdmin()
        {
            InitializeComponent();
        }

        public void ShowForm(FORMTYPE pFormType)
        {
            mFormType = pFormType;

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();

            DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            DTPToDate.Value = DateTime.Now;

            DTPFromDate.Checked = false;
            DTPToDate.Checked = false;

            DTabPredictionView.Columns.Add(new DataColumn("ClientRefNo", typeof(string)));

            DTabPrdType = new BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_PRDTYPE_PREDICTION);

            CmbPrdType.Properties.DataSource = DTabPrdType;
            CmbPrdType.Properties.DisplayMember = "PRDTYPENAME";
            CmbPrdType.Properties.ValueMember = "PRDTYPE_ID";


            foreach (DataRow DRow in DTabPrdType.Rows)
            {
                // string Col = "PRDTYPE_" + Val.ToString(DRow["PRDTYPE_ID"]) + "_";
                string Col = "PRDTYPE_" + Val.ToString(DRow["PRDTYPENAME"]) + "_";

                //if (Val.ToInt32(DRow["PRDTYPE_ID"]) == 6)
                //{

                DTabPredictionView.Columns.Add(new DataColumn(Col + "LabCharge", typeof(double)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "AmountWithLabCharge", typeof(double)));
                //}
                //else
                //{
                DTabPredictionView.Columns.Add(new DataColumn(Col + "ClientRefNo", typeof(string)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "EmpCode", typeof(string)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "EmpName", typeof(string)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "Shp", typeof(string)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "Col", typeof(string)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "ColSeqNo", typeof(int)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "Cla", typeof(string)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "ClaSeqNo", typeof(int)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "Cut", typeof(string)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "CutSeqNo", typeof(int)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "Pol", typeof(string)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "PolSeqNo", typeof(int)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "Sym", typeof(string)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "SymSeqNo", typeof(int)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "FL", typeof(string)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "FLSeqNo", typeof(int)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "RoughCarat", typeof(double)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "ExpCarat", typeof(double)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "ExpPer", typeof(double)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "Rapaport", typeof(double)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "Discount", typeof(double)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "PricePerCarat", typeof(double)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "Amount", typeof(double)));
                DTabPredictionView.Columns.Add(new DataColumn(Col + "EntryDate", typeof(DateTime)));
            }

            GrdDet.BeginUpdate();

            MainGrid.DataSource = DTabPredictionView;
            MainGrid.Refresh();
            GrdDet.PopulateColumns();

            GrdDet.Bands.Clear();

            var gridBand = new GridBand();
            gridBand.Name = "BandGeneral";
            gridBand.Caption = "General";
            gridBand.Tag = "General";
            gridBand.RowCount = 1;
            gridBand.VisibleIndex = 0;
            gridBand.Fixed = FixedStyle.Left;
            GrdDet.Bands.Add(gridBand);

            GrdDet.Columns["ClientRefNo"].OwnerBand = gridBand;
            GrdDet.Columns["ClientRefNo"].Caption = "StockNo";

            GrdDet.Columns["ClientRefNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            try
            {
                foreach (DataRow DRow in DTabPrdType.Rows)
                {
                    gridBand = new GridBand();
                    gridBand.Name = "PRDTYPE_" + Val.ToString(DRow["PRDTYPENAME"]);
                    gridBand.Caption = Val.ToString(DRow["SEQUENCENO"]) + ". " + Val.ToString(DRow["PRDTYPENAME"]);
                    gridBand.RowCount = 1;
                    gridBand.Tag = Val.ToString(DRow["PRDTYPE_ID"]);
                    gridBand.VisibleIndex = Val.ToInt(DRow["SEQUENCENO"]);

                    GrdDet.Bands.Add(gridBand);

                    string Col = "PRDTYPE_" + Val.ToString(DRow["PRDTYPENAME"]) + "_";

                    if (Val.ToInt32(DRow["PRDTYPE_ID"]) == 6)
                    {
                        GrdDet.Columns[Col + "Discount"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "PricePerCarat"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "Amount"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "Rapaport"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "LabCharge"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "AmountWithLabCharge"].OwnerBand = gridBand;

                        GrdDet.Columns[Col + "Rapaport"].Caption = "Rap";
                        GrdDet.Columns[Col + "Discount"].Caption = "Dis %";
                        GrdDet.Columns[Col + "PricePerCarat"].Caption = "$/Cts";
                        GrdDet.Columns[Col + "Amount"].Caption = "Amt";

                        GrdDet.Columns[Col + "LabCharge"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        GrdDet.Columns[Col + "AmountWithLabCharge"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;

                        GrdDet.Columns[Col + "LabCharge"].AppearanceCell.ForeColor = Color.Red;

                        GrdDet.Columns[Col + "LabCharge"].Caption = "LabCharge";
                        GrdDet.Columns[Col + "AmountWithLabCharge"].Caption = "Amt With LabCharge";

                        GrdDet.Columns[Col + "Discount"].OptionsColumn.AllowEdit = true;
                        GrdDet.Columns[Col + "PricePerCarat"].OptionsColumn.AllowEdit = true;
                        GrdDet.Columns[Col + "Amount"].OptionsColumn.AllowEdit = false;
                        GrdDet.Columns[Col + "Rapaport"].OptionsColumn.AllowEdit = false;
                        GrdDet.Columns[Col + "LabCharge"].OptionsColumn.AllowEdit = false;
                        GrdDet.Columns[Col + "AmountWithLabCharge"].OptionsColumn.AllowEdit = false;

                    }
                    else
                    {
                        GrdDet.Columns[Col + "EmpCode"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "EmpName"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "Shp"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "ExpCarat"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "Col"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "ColSeqNo"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "Cla"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "ClaSeqNo"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "Cut"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "CutSeqNo"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "Pol"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "PolSeqNo"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "Sym"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "SymSeqNo"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "FL"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "FLSeqNo"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "RoughCarat"].OwnerBand = gridBand;

                        GrdDet.Columns[Col + "ExpPer"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "Rapaport"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "Discount"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "PricePerCarat"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "Amount"].OwnerBand = gridBand;
                        GrdDet.Columns[Col + "EntryDate"].OwnerBand = gridBand;

                        GrdDet.Columns[Col + "ClientRefNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        GrdDet.Columns[Col + "EmpCode"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        GrdDet.Columns[Col + "EmpName"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        GrdDet.Columns[Col + "Shp"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        GrdDet.Columns[Col + "Col"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        GrdDet.Columns[Col + "ColSeqNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        GrdDet.Columns[Col + "Cla"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        GrdDet.Columns[Col + "ClaSeqNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        GrdDet.Columns[Col + "Cut"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        GrdDet.Columns[Col + "CutSeqNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        GrdDet.Columns[Col + "Pol"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        GrdDet.Columns[Col + "PolSeqNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        GrdDet.Columns[Col + "Sym"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        GrdDet.Columns[Col + "SymSeqNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        GrdDet.Columns[Col + "FL"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        GrdDet.Columns[Col + "FLSeqNo"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        GrdDet.Columns[Col + "RoughCarat"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        GrdDet.Columns[Col + "ExpCarat"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        GrdDet.Columns[Col + "ExpPer"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        GrdDet.Columns[Col + "Rapaport"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        GrdDet.Columns[Col + "Discount"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        GrdDet.Columns[Col + "PricePerCarat"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        GrdDet.Columns[Col + "Amount"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
                        GrdDet.Columns[Col + "EntryDate"].AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
                        GrdDet.Columns[Col + "EntryDate"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
                        GrdDet.Columns[Col + "EntryDate"].DisplayFormat.FormatString = "dd/MM/yyyy";

                        GrdDet.Columns[Col + "ExpCarat"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                        GrdDet.Columns[Col + "ExpCarat"].DisplayFormat.FormatString = "{0:N3}";

                        GrdDet.Columns[Col + "Discount"].AppearanceCell.Font = new Font(GrdDet.Columns[Col + "Discount"].AppearanceCell.Font.Name, GrdDet.Columns[Col + "Discount"].AppearanceCell.Font.Size, FontStyle.Bold);
                        GrdDet.Columns[Col + "ExpCarat"].AppearanceCell.Font = new Font(GrdDet.Columns[Col + "Amount"].AppearanceCell.Font.Name, GrdDet.Columns[Col + "Amount"].AppearanceCell.Font.Size, FontStyle.Bold);
                        GrdDet.Columns[Col + "Amount"].AppearanceCell.Font = new Font(GrdDet.Columns[Col + "Amount"].AppearanceCell.Font.Name, GrdDet.Columns[Col + "Amount"].AppearanceCell.Font.Size, FontStyle.Bold);

                        GrdDet.Columns[Col + "Discount"].AppearanceCell.ForeColor = Color.Purple;

                        GrdDet.Columns[Col + "EmpCode"].Caption = "Emp";
                        GrdDet.Columns[Col + "EmpName"].Caption = "Name";
                        GrdDet.Columns[Col + "Shp"].Caption = "Shp";
                        GrdDet.Columns[Col + "Col"].Caption = "Col";
                        GrdDet.Columns[Col + "ColSeqNo"].Caption = "ColSeq";
                        GrdDet.Columns[Col + "Cla"].Caption = "Cla";
                        GrdDet.Columns[Col + "ClaSeqNo"].Caption = "ClaSeq";
                        GrdDet.Columns[Col + "Cut"].Caption = "Cut";
                        GrdDet.Columns[Col + "CutSeqNo"].Caption = "CutSeqNo";
                        GrdDet.Columns[Col + "Pol"].Caption = "Pol";
                        GrdDet.Columns[Col + "PolSeqNo"].Caption = "PolSeqNo";
                        GrdDet.Columns[Col + "Sym"].Caption = "Sym";
                        GrdDet.Columns[Col + "SymSeqNo"].Caption = "SymSeqNo";
                        GrdDet.Columns[Col + "FL"].Caption = "FL";
                        GrdDet.Columns[Col + "FLSeqNo"].Caption = "FLSeqNo";
                        GrdDet.Columns[Col + "RoughCarat"].Caption = "RouCts";
                        GrdDet.Columns[Col + "ExpCarat"].Caption = "Cts";
                        GrdDet.Columns[Col + "ExpPer"].Caption = "Exp %";
                        GrdDet.Columns[Col + "Rapaport"].Caption = "Rap";
                        GrdDet.Columns[Col + "Discount"].Caption = "Dis %";
                        GrdDet.Columns[Col + "PricePerCarat"].Caption = "$/Cts";
                        GrdDet.Columns[Col + "Amount"].Caption = "Amt";
                        GrdDet.Columns[Col + "EntryDate"].Caption = "Date";
                        
                        GrdDet.Columns[Col + "RoughCarat"].Summary.Add(SummaryItemType.Sum, Col + "RoughCarat", "{0:N3}");
                        GrdDet.GroupSummary.Add(SummaryItemType.Sum, Col + "RoughCarat", GrdDet.Columns[Col + "RoughCarat"], "{0:N3}");

                        GrdDet.Columns[Col + "ExpCarat"].Summary.Add(SummaryItemType.Sum, Col + "ExpCarat", "{0:N3}");
                        GrdDet.GroupSummary.Add(SummaryItemType.Sum, Col + "ExpCarat", GrdDet.Columns[Col + "ExpCarat"], "{0:N0}");

                        if (GrdDet.Columns[Col + "PricePerCarat"] != null)
                        {
                            GrdDet.Columns[Col + "PricePerCarat"].Summary.Add(SummaryItemType.Custom, Col + "PricePerCarat", "{0:N0}");
                            GrdDet.GroupSummary.Add(SummaryItemType.Sum, Col + "PricePerCarat", GrdDet.Columns[Col + "PricePerCarat"], "{0:N0}");
                        }
                       
                        GrdDet.Columns[Col + "EmpName"].Visible = false;
                        GrdDet.Columns[Col + "ColSeqNo"].Visible = false;
                        GrdDet.Columns[Col + "ClaSeqNo"].Visible = false;
                        GrdDet.Columns[Col + "CutSeqNo"].Visible = false;
                        GrdDet.Columns[Col + "PolSeqNo"].Visible = false;
                        GrdDet.Columns[Col + "SymSeqNo"].Visible = false;
                        GrdDet.Columns[Col + "FLSeqNo"].Visible = false;
                        GrdDet.Columns[Col + "RoughCarat"].Visible = false;
                        GrdDet.Columns[Col + "ExpPer"].Visible = false;

                        GrdDet.Columns[Col + "ExpCarat"].Caption = "Cts";
                        GrdDet.Columns[Col + "ExpPer"].Caption = "Exp %";

                        GrdDet.Columns[Col + "Discount"].OptionsColumn.AllowEdit = false;
                        GrdDet.Columns[Col + "PricePerCarat"].OptionsColumn.AllowEdit = false;
                        GrdDet.Columns[Col + "Amount"].OptionsColumn.AllowEdit = false;
                        GrdDet.Columns[Col + "Rapaport"].OptionsColumn.AllowEdit = false;
                        GrdDet.Columns[Col + "LabCharge"].OptionsColumn.AllowEdit = false;
                        GrdDet.Columns[Col + "AmountWithLabCharge"].OptionsColumn.AllowEdit = false;
                    }
                }

                

                for (int i = 0; i < GrdDet.Columns.Count; i++)
                {
                    GrdDet.Columns[i].OptionsFilter.FilterPopupMode = FilterPopupMode.CheckedList;
                }


                txtClientRefNo.Focus();

                //CmbPrdType.SetEditValue("1,4");

                string Str = new BOTRN_KapanCreate().GetGridLayout(this.Name, GrdDet.Name);

                if (Str != "")
                {
                    byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                    MemoryStream stream = new MemoryStream(byteArray);
                    GrdDet.RestoreLayoutFromStream(stream);

                }

                GrdDet.EndUpdate();

                CmbPrdType.SelectAll();


            }
            catch (Exception EX)
            {
                GrdDet.EndUpdate();
                Global.MessageError(EX.Message);
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
            ObjFormEvent.ObjToDisposeList.Add(ObjView);
            ObjFormEvent.ObjToDisposeList.Add(Val);

        }

        #endregion

        #region Background Worker


        private void SetControlPropertyValue(Control oControl, string propName, object propValue)
        {
            if (oControl.InvokeRequired)
            {
                SetControlValueCallback d = new SetControlValueCallback(SetControlPropertyValue);
                oControl.Invoke(d, new object[]
                        {
                            oControl,
                            propName,
                            propValue
                        });
            }
            else
            {
                Type t = oControl.GetType();
                PropertyInfo[] props = t.GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if ((p.Name.ToUpper() == propName.ToUpper()))
                    {
                        p.SetValue(oControl, propValue, null);
                    }
                }
            }
        }

        #endregion

        public void Link_CreateMarginalHeaderArea(object sender, CreateAreaEventArgs e)
        {
            // ' For Report Title

            TextBrick BrickTitle = e.Graph.DrawString(BusLib.Configuration.BOConfiguration.gEmployeeProperty.COMPANYNAME, System.Drawing.Color.Navy, new RectangleF(0, 0, e.Graph.ClientPageSize.Width, 35), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitle.Font = new Font("verdana", 12, FontStyle.Bold);
            BrickTitle.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            BrickTitle.VertAlignment = DevExpress.Utils.VertAlignment.Center;

            // ' For Group 
            TextBrick BrickTitleseller = e.Graph.DrawString("Prediction View", System.Drawing.Color.Navy, new RectangleF(0, 35, e.Graph.ClientPageSize.Width, 35), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitleseller.Font = new Font("verdana", 10, FontStyle.Bold);
            BrickTitleseller.HorzAlignment = DevExpress.Utils.HorzAlignment.Center;
            BrickTitleseller.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            BrickTitleseller.ForeColor = Color.Black;

            // ' For Filter 
            TextBrick BrickTitlesParam = e.Graph.DrawString("Kapan Name :- " + Val.ToString(txtClientRefNo.Text), System.Drawing.Color.Navy, new RectangleF(0, 70, e.Graph.ClientPageSize.Width, 30), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitlesParam.Font = new Font("verdana", 8, FontStyle.Bold);
            BrickTitlesParam.HorzAlignment = DevExpress.Utils.HorzAlignment.Near;
            BrickTitlesParam.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            BrickTitlesParam.ForeColor = Color.Black;


            int IntX = Convert.ToInt32(Math.Round(e.Graph.ClientPageSize.Width - 400, 0));
            TextBrick BrickTitledate = e.Graph.DrawString("Print Date :- " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss tt"), System.Drawing.Color.Navy, new RectangleF(IntX, 70, 400, 30), DevExpress.XtraPrinting.BorderSide.None);
            BrickTitledate.Font = new Font("verdana", 8, FontStyle.Bold);
            BrickTitledate.HorzAlignment = DevExpress.Utils.HorzAlignment.Far;
            BrickTitledate.VertAlignment = DevExpress.Utils.VertAlignment.Center;
            BrickTitledate.ForeColor = Color.Black;

        }

        private void BtnBestFit_Click(object sender, EventArgs e)
        {
            GrdDet.BestFitColumns();
        }

        private void BtnExpandAll_Click(object sender, EventArgs e)
        {
            GrdDet.ExpandAllGroups();
        }

        private void BtnCollepsAll_Click(object sender, EventArgs e)
        {
            GrdDet.CollapseAllGroups();
        }

        private void MainGrid_Paint(object sender, PaintEventArgs e)
        {
            GridControl gridC = sender as GridControl;
            GridView gridView = gridC.FocusedView as GridView;
            BandedGridViewInfo info = (BandedGridViewInfo)gridView.GetViewInfo();
            for (int i = 0; i < info.BandsInfo.BandCount; i++)
            {
                e.Graphics.DrawLine(new Pen(Brushes.Black, 1), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.BandsInfo[i].Bounds.Top), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.RowsInfo[info.RowsInfo.Count - 1].Bounds.Bottom - 1));
                //if (i == 1) e.Graphics.DrawLine(new Pen(Brushes.Black), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.BandsInfo[i].Bounds.Top), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.RowsInfo[info.RowsInfo.Count - 1].Bounds.Bottom - 1));
                //if (i == info.BandsInfo.BandCount - 1) e.Graphics.DrawLine(new Pen(Brushes.Black), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.BandsInfo[i].Bounds.Top), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.RowsInfo[info.RowsInfo.Count - 1].Bounds.Bottom - 1));
            }
        }


        public void SetRowCellColor(RowCellStyleEventArgs e, string BaseType, string Current)
        {
            if (e.Column.FieldName == Current + "_Shp")
            {
                string StrMakShape = Val.ToString(GrdDet.GetRowCellValue(e.RowHandle, BaseType + "_Shp"));
                string StrShape = Val.ToString(GrdDet.GetRowCellValue(e.RowHandle, Current + "_Shp"));
                if (StrMakShape != "" && StrShape != "" && StrMakShape != StrShape)
                {
                    e.Appearance.BackColor = lblUp.BackColor;
                }
            }

            if (e.Column.FieldName == Current + "_Col")
            {
                int IntMakSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, BaseType + "_ColSeqNo"));
                int IntSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, Current + "_ColSeqNo"));
                if (IntSeqNo != 0 && IntMakSeqNo != 0 && IntSeqNo > IntMakSeqNo)
                {
                    e.Appearance.BackColor = lblUp.BackColor;
                }
                else if (IntSeqNo != 0 && IntMakSeqNo != 0 && IntSeqNo < IntMakSeqNo)
                {
                    e.Appearance.BackColor = lblDown.BackColor;
                }
            }
            if (e.Column.FieldName == Current + "_Cla")
            {
                int IntMakSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, BaseType + "_ClaSeqNo"));
                int IntSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, Current + "_ClaSeqNo"));
                if (IntSeqNo != 0 && IntMakSeqNo != 0 && IntSeqNo > IntMakSeqNo)
                {
                    e.Appearance.BackColor = lblUp.BackColor;
                }
                else if (IntSeqNo != 0 && IntMakSeqNo != 0 && IntSeqNo < IntMakSeqNo)
                {
                    e.Appearance.BackColor = lblDown.BackColor;
                }
            }
            if (e.Column.FieldName == Current + "_Cut")
            {
                int IntMakSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, BaseType + "_CutSeqNo"));
                int IntSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, Current + "_CutSeqNo"));
                if (IntSeqNo != 0 && IntMakSeqNo != 0 && IntSeqNo > IntMakSeqNo)
                {
                    e.Appearance.BackColor = lblUp.BackColor;
                }
                else if (IntSeqNo != 0 && IntMakSeqNo != 0 && IntSeqNo < IntMakSeqNo)
                {
                    e.Appearance.BackColor = lblDown.BackColor;
                }
            }
            if (e.Column.FieldName == Current + "_Pol")
            {
                int IntMakSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, BaseType + "_PolSeqNo"));
                int IntSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, Current + "_PolSeqNo"));
                if (IntSeqNo != 0 && IntMakSeqNo != 0 && IntSeqNo > IntMakSeqNo)
                {
                    e.Appearance.BackColor = lblUp.BackColor;
                }
                else if (IntSeqNo != 0 && IntMakSeqNo != 0 && IntSeqNo < IntMakSeqNo)
                {
                    e.Appearance.BackColor = lblDown.BackColor;
                }
            }
            if (e.Column.FieldName == Current + "_Sym")
            {
                int IntMakSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, BaseType + "_SymSeqNo"));
                int IntSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, Current + "_SymSeqNo"));
                if (IntSeqNo != 0 && IntMakSeqNo != 0 && IntSeqNo > IntMakSeqNo)
                {
                    e.Appearance.BackColor = lblUp.BackColor;
                }
                else if (IntSeqNo != 0 && IntMakSeqNo != 0 && IntSeqNo < IntMakSeqNo)
                {
                    e.Appearance.BackColor = lblDown.BackColor;
                }
            }
            if (e.Column.FieldName == Current + "_FL")
            {
                int IntMakSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, BaseType + "_FLSeqNo"));
                int IntSeqNo = Val.ToInt(GrdDet.GetRowCellValue(e.RowHandle, Current + "_FLSeqNo"));
                if (IntSeqNo != 0 && IntMakSeqNo != 0 && IntSeqNo > IntMakSeqNo)
                {
                    e.Appearance.BackColor = lblUp.BackColor;
                }
                else if (IntSeqNo != 0 && IntMakSeqNo != 0 && IntSeqNo < IntMakSeqNo)
                {
                    e.Appearance.BackColor = lblDown.BackColor;
                }
            }
            if (e.Column.FieldName == Current + "_ExpCarat")
            {
                double DouMakCarat = Val.Val(GrdDet.GetRowCellValue(e.RowHandle, BaseType + "_ExpCarat"));
                double DouCarat = Val.Val(GrdDet.GetRowCellValue(e.RowHandle, Current + "_ExpCarat"));
                if (DouCarat != 0 && DouMakCarat != 0 && DouCarat > DouMakCarat)
                {
                    e.Appearance.BackColor = lblDown.BackColor;
                }
                else if (DouCarat != 0 && DouMakCarat != 0 && DouCarat < DouMakCarat)
                {
                    e.Appearance.BackColor = lblUp.BackColor;
                }
            }
            if (e.Column.FieldName == Current + "_Amount")
            {
                double DouMakAmount = Val.Val(GrdDet.GetRowCellValue(e.RowHandle, BaseType + "_Amount"));
                double DouAmount = Val.Val(GrdDet.GetRowCellValue(e.RowHandle, Current + "_Amount"));
                if (DouAmount != 0 && DouMakAmount != 0 && DouAmount > DouMakAmount)
                {
                    e.Appearance.BackColor = lblDown.BackColor;
                }
                else if (DouAmount != 0 && DouMakAmount != 0 && DouAmount < DouMakAmount)
                {
                    e.Appearance.BackColor = lblUp.BackColor;
                }
            }

            //if (e.Column.FieldName == Current + "_Diff")
            //{
            //    double DouAmount = Val.Val(GrdDet.GetRowCellValue(e.RowHandle, Current + "_Diff"));
            //    if (DouAmount > 0)
            //    {
            //        e.Appearance.BackColor = lblDown.BackColor;
            //    }
            //    else if (DouAmount < 0)
            //    {
            //        e.Appearance.BackColor = lblUp.BackColor;
            //    }
            //}
        }

        private void RbtAll_CheckedChanged(object sender, EventArgs e)
        {
            BtnSearch_Click(null, null);
        }

        private void GrdDet_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }

                Int64 IntIsColor = Val.ToInt64(GrdDet.GetRowCellValue(e.RowHandle, "ISCOLOR")); //.ToUpper();

                if (IntIsColor == 1)
                {
                    //e.Graphics.DrawLine(Pens.Red, e.Bounds.Right, e.Bounds.Top, e.Bounds.Right, e.Bounds.Bottom);
                    e.Graphics.DrawLine(Pens.Red, e.Bounds.Top, e.Bounds.Right, e.Bounds.Top, e.Bounds.Right);
                    e.Appearance.DrawString(e.Cache, e.DisplayText, e.Bounds);
                    e.Handled = true;

                    Point p1, p2;
                    p1 = new Point(e.Bounds.Left, e.Bounds.Top + 1);
                    p2 = new Point(e.Bounds.Right, e.Bounds.Top + 1);

                    e.Graphics.DrawLine(Pens.Black, p1, p2);

                    p1 = new Point(e.Bounds.Left, e.Bounds.Top + 3);
                    p2 = new Point(e.Bounds.Right, e.Bounds.Top + 3);
                    e.Graphics.DrawLine(Pens.Black, p1, p2);

                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtEmployee_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "EMPLOYEECODE,EMPLOYEENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_EMPLOYEE);

                    FrmSearch.mStrColumnsToHide = "EMPLOYEE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtEmployee.Text = Val.ToString(FrmSearch.DRow["EMPLOYEENAME"]);
                        txtEmployee.Tag = Val.ToString(FrmSearch.DRow["EMPLOYEE_ID"]);
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void GrdDet_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
        }

        private void GrdDet_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000")
            {
                e.DisplayText = string.Empty;
            }
        }

        private void txtYear_Validated(object sender, EventArgs e)
        {
            if (Val.Val(txtYear.Text) == 0)
            {
                DTPFromDate.Checked = false;
                DTPToDate.Checked = false;
                return;
            }
            else if (txtYear.Text.Length != 6)
            {
                DTPFromDate.Checked = false;
                DTPToDate.Checked = false;
                Global.MessageError("Invalid Form Of Year And Month");
                return;
            }
            int IntYear = Val.ToInt(Val.Left(txtYear.Text, 4));
            int IntMonth = Val.ToInt(Val.Right(txtYear.Text, 2));
            if (IntMonth > 12)
            {
                DTPFromDate.Checked = false;
                DTPToDate.Checked = false;

                Global.MessageError("Month Number > 12 Does Not Exists");
                return;
            }

            DateTime startDate = new DateTime(IntYear, IntMonth, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);
            DTPFromDate.Checked = true;
            DTPToDate.Checked = true;
            DTPFromDate.Value = startDate;
            DTPToDate.Value = endDate;

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DataSet DS = ObjView.PredictionViewGetDataForAdmin(mStrClientRefNo, mStrPredictionType, StrFromDate, StrToDate, StrEmployee_ID, mStrPredictionTypeOther, mStrKapan);
                // For Pivot Grid
                DGrd = ObjTra.GerGradingComparisionWithLatestGrdByLab(mStrClientRefNo, "GRD", StrFromDate, StrToDate, StrEmployee_ID);

                DTabPrdSummary = DS.Tables[0].Copy();
                DTabPacketDetail = DS.Tables[1].Copy();
                DTabPrdDetail = DS.Tables[2].Copy();
            }
            catch (Exception Ex)
            {
                PanelProgress.Visible = false;
                BtnSearch.Enabled = true;
                Global.Message(Ex.Message.ToString());
            }

        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                PanelProgress.Visible = false;
                BtnSearch.Enabled = true;

                GrdDet.BeginUpdate();

                DTabPredictionView.Rows.Clear();


                DataTable DTabStoneNo = DTabPrdSummary.DefaultView.ToTable(true, "ClientRefNo");

                foreach (DataRow DRow in DTabStoneNo.Rows)
                {

                    DataRow DRNew = DTabPredictionView.NewRow();

                    DRNew["ClientRefNo"] = Val.ToString(DRow["ClientRefNo"]);

                    string StrQueryPol = "CLIENTREFNO = '" + Val.ToString(DRow["CLIENTREFNO"]) + "'";

                    foreach (DataRow DRowPrd in DTabPrdType.Rows)
                    {
                        string Col = "PRDTYPE_" + Val.ToString(DRowPrd["PRDTYPENAME"]) + "_";

                        string StrPrdTypeID = Val.ToString(DRowPrd["PrdTypeName"]);
                        string StrClientRefNo = Val.ToString(DRow["ClientRefNo"]);

                        string StrQuery = "ClientRefNo = '" + StrClientRefNo + "' AND  PrdTypeName = '" + StrPrdTypeID + "'";

                        DataRow[] UDROW = DTabPrdSummary.Select(StrQuery);

                        if (Val.ToInt32(DRowPrd["PRDTYPE_ID"]) == 6)
                        {
                            if (GrdDet.Columns[Col + "Discount"].OwnerBand.Caption.Contains("FINAL"))
                            {
                                GrdDet.Columns[Col + "Discount"].OptionsColumn.AllowEdit = true;
                                GrdDet.Columns[Col + "AmountWithLabCharge"].Visible = false;

                            }
                            else
                            {
                                GrdDet.Columns[Col + "Discount"].OptionsColumn.AllowEdit = false;
                            }
                        }

                        GrdDet.Columns[Col + "ClientRefNo"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                        GrdDet.Columns[Col + "ClientRefNo"].SummaryItem.DisplayFormat = "{0:N0}";

                        GrdDet.Columns[Col + "ExpCarat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        GrdDet.Columns[Col + "ExpCarat"].SummaryItem.DisplayFormat = "{0:N2}";

                        GrdDet.Columns[Col + "Amount"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                        GrdDet.Columns[Col + "Amount"].SummaryItem.DisplayFormat = "{0:N2}";

                        GrdDet.Columns[Col + "PricePerCarat"].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Custom;
                        GrdDet.Columns[Col + "PricePerCarat"].SummaryItem.DisplayFormat = "{0:N2}";

                        if (UDROW != null)
                        {
                            foreach (DataRow dddd in UDROW)
                            {
                                DRNew[Col + "ClientRefNo"] = Val.ToString(dddd["CLIENTREFNO"]);
                                DRNew[Col + "EmpCode"] = Val.ToString(dddd["EMPCODE"]);
                                DRNew[Col + "EmpName"] = Val.ToString(dddd["EMPNAME"]);
                                DRNew[Col + "Shp"] = Val.ToString(dddd["SHPCODE"]);
                                DRNew[Col + "Col"] = Val.ToString(dddd["COLCODE"]);
                                DRNew[Col + "ColSeqNo"] = Val.ToInt(dddd["COLSEQNO"]);
                                DRNew[Col + "Cla"] = Val.ToString(dddd["CLACODE"]);
                                DRNew[Col + "ClaSeqNo"] = Val.ToInt(dddd["CLASEQNO"]);
                                DRNew[Col + "Cut"] = Val.ToString(dddd["CUTCODE"]);
                                DRNew[Col + "CutSeqNo"] = Val.ToInt(dddd["CUTSEQNO"]);
                                DRNew[Col + "Pol"] = Val.ToString(dddd["POLCODE"]);
                                DRNew[Col + "PolSeqNo"] = Val.ToInt(dddd["POLSEQNO"]);
                                DRNew[Col + "Sym"] = Val.ToString(dddd["SYMCODE"]);
                                DRNew[Col + "SymSeqNo"] = Val.ToInt(dddd["SYMSEQNO"]);
                                DRNew[Col + "FL"] = Val.ToString(dddd["FLCODE"]);
                                DRNew[Col + "FLSeqNo"] = Val.ToInt(dddd["FLSEQNO"]);
                                DRNew[Col + "ExpCarat"] = Val.Val(dddd["CARAT"]);
                                DRNew[Col + "Rapaport"] = Val.Val(dddd["RAPAPORT"]);
                                DRNew[Col + "Discount"] = Val.Val(dddd["DISCOUNT"]);
                                DRNew[Col + "PricePerCarat"] = Val.Val(dddd["PRICEPERCARAT"]);
                                DRNew[Col + "Amount"] = Val.Val(dddd["AMOUNT"]);
                                DRNew[Col + "EntryDate"] = dddd["ENTRYDATE"];
                                DRNew[Col + "LabCharge"] = dddd["LABCHARGE"];
                                DRNew[Col + "AmountWithLabCharge"] = dddd["AMOUNTWITHLABCHARGE"];

                                double DoubleAmount = Val.Val(dddd["AMOUNT"]);

                                string StrQueryAmt = "CLIENTREFNO = '" + Val.ToString(DRow["ClientRefNo"]) + "'";
                                DataRow[] UDROWAmt = DTabPrdSummary.Select(StrQueryAmt);
                                if (UDROWAmt != null && UDROWAmt.Length == 1)
                                {
                                    double DouBaseAmount = Val.Val(UDROWAmt[0]["AMOUNT"]);
                                }
                                UDROWAmt = null;
                            }
                        }
                        UDROW = null;
                    }
                    DTabPredictionView.Rows.Add(DRNew);
                }


                DTabDistinct = DTabPrdDetail.DefaultView.ToTable(true, "PRDTYPE_ID");

                if (DTabDistinct.Rows.Count != 0)
                {
                    foreach (GridBand band in GrdDet.Bands)
                    {
                        foreach (DataRow DRPrdID in DTabDistinct.Rows)
                        {
                            if (band.Name == "BandGeneral" || band.Name == "POLISHOK" || band.Name == "BandRefDetail")
                            {
                                continue;
                            }
                            if (Val.ToString(DRPrdID["PRDTYPE_ID"]) != "")
                            {
                                if (band.Tag.ToString() == Val.ToString(DRPrdID["PRDTYPE_ID"]))
                                {
                                    band.Visible = true;
                                    break;
                                }
                                else
                                {
                                    band.Visible = false;
                                }
                            }

                        }
                    }

                }

                DTabDistinct.Dispose();
                DTabDistinct = null;

                GrdDet.EndUpdate();

                GrdDet.RefreshData();
                GrdDet.BestFitColumns();

                // Fill Pivot 

                PvtGrdColor.DataSource = DGrd.Tables[0];
                PvtGrdColor.Refresh();

                PvtGrdClarity.DataSource = DGrd.Tables[1];
                PvtGrdClarity.Refresh();

                PvtGrdCut.DataSource = DGrd.Tables[2];
                PvtGrdCut.Refresh();

                PvtGrdPol.DataSource = DGrd.Tables[3];
                PvtGrdPol.Refresh();

                PvtGrdSym.DataSource = DGrd.Tables[4];
                PvtGrdSym.Refresh();

                MainGrdTotal.DataSource = DGrd.Tables[5];
                MainGrdTotal.Refresh();
                GrdDetTotal.BestFitColumns();

                MainGrid.DataSource = DTabPredictionView;
                GrdDet.RefreshData();
            }
            catch (Exception ex)
            {
                PanelProgress.Visible = false;
                BtnSearch.Enabled = true;
                Global.Message(ex.Message.ToString());
            }
        }

        public void Clear()
        {
            txtClientRefNo.Text = string.Empty;
            txtClientRefNo.Tag = string.Empty;
            txtYear.Text = string.Empty;
            DTPFromDate.Checked = true;
            DTPToDate.Checked = true;
            DTabPrdSummary.Rows.Clear();
            DTabPredictionView.Rows.Clear();
            DTabPrdType.Rows.Clear();
            DGrd.Tables[0].Rows.Clear();
            DGrd.Tables[1].Rows.Clear();
            DGrd.Tables[2].Rows.Clear();
            DGrd.Tables[3].Rows.Clear();
            DGrd.Tables[4].Rows.Clear();
            DGrd.Tables[5].Rows.Clear();
            DTPFromDate.Value = DateTime.Now.AddMonths(-1);
            DTPToDate.Value = DateTime.Now;
            //DataTable dt = objLedgerTrn.GetFromToDateYear();
            //if (dt.Rows.Count > 0)
            //{
            //    DTPFromDate.Text = Val.ToString(dt.Rows[0][0]);
            //    DTPToDate.Text = Val.ToString(dt.Rows[0][1]);
            //}
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtClientRefNo.Text.Length == 0)
                {
                    txtClientRefNo.Tag = string.Empty;
                }

                if (txtKapanName.Text.Length == 0)
                {
                    txtKapanName.Tag = string.Empty;
                }

                //if (txtClientRefNo.Text.Length == 0)
                //{
                //    Global.Message("Please Select Client Ref No");
                //    txtClientRefNo.Focus();
                //    return;
                //}

                mStrClientRefNo = Val.ToString(txtClientRefNo.Text);

                if (txtEmployee.Text.Trim().Length == 0)
                {
                    txtEmployee.Tag = "";
                }

                mStrKapan = Val.ToString(txtKapanName.Text);
                mStrPredictionType = Val.Trim(CmbPrdType.Properties.GetCheckedItems());
                StrEmployee_ID = Val.ToString(txtEmployee.Tag);
                if (DTPFromDate.Checked == true)
                {
                    StrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
                }
                if (DTPToDate.Checked == true)
                {
                    StrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());
                }

                DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;

                DTabPacketDetail.Rows.Clear();
                DTabPrdDetail.Rows.Clear();
                DTabPrdSummary.Rows.Clear();
                DTabPolishOkDetail.Rows.Clear();

                BtnSearch.Enabled = false;
                PanelProgress.Visible = true;
                if (!backgroundWorker1.IsBusy)
                {
                    backgroundWorker1.RunWorkerAsync();
                }

            }
            catch (Exception ex)
            {
                // this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
                return;
            }

        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lblSaveLayout_Click(object sender, EventArgs e)
        {
            Stream str = new System.IO.MemoryStream();
            GrdDet.SaveLayoutToStream(str);
            str.Seek(0, System.IO.SeekOrigin.Begin);
            StreamReader reader = new StreamReader(str);
            string text = reader.ReadToEnd();

            int IntRes = new BOTRN_KapanCreate().SaveGridLayout(this.Name, GrdDet.Name, text);
            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Saved");
            }
        }

        private void lblDefaultLayout_Click(object sender, EventArgs e)
        {
            int IntRes = new BOTRN_KapanCreate().DeleteGridLayout(this.Name, GrdDet.Name);
            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Deleted");
            }


        }

        private void BtnKapanLiveStockExcelExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = ".xlsx";
                svDialog.Title = "Export to Excel";
                svDialog.FileName = "Prediction";
                svDialog.Filter = "Excel files 97-2003 (*.xls)|*.xls|Excel files 2007(*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    {
                        PrintableComponentLinkBase link = new PrintableComponentLinkBase()
                        {
                            PrintingSystemBase = new PrintingSystemBase(),
                            Component = MainGrid,
                            Landscape = true,
                            PaperKind = PaperKind.A4,
                            Margins = new System.Drawing.Printing.Margins(20, 20, 200, 20)
                        };

                        link.CreateReportHeaderArea += new CreateAreaEventHandler(Link_CreateMarginalHeaderArea);

                        link.ExportToXls(svDialog.FileName);

                        if (Global.Confirm("Do You Want To Open [Prediction.xlsx] ?") == System.Windows.Forms.DialogResult.Yes)
                        {
                            System.Diagnostics.Process.Start(svDialog.FileName, "CMD");
                        }
                    }
                }
                svDialog.Dispose();
                svDialog = null;

            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
            }


        }

        private void txtClientRefNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (txtClientRefNo.Enabled == false)
                {
                    return;
                }
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "STOCKNO";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.TRN_STOCK);
                    FrmSearch.mStrColumnsToHide = "STOCK_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtClientRefNo.Text = Val.ToString(FrmSearch.DRow["STOCKNO"]);
                        txtClientRefNo.Tag = Val.ToString(FrmSearch.DRow["STOCK_ID"]);
                    }

                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }

        private void PvtGrdColor_CustomDrawCell(object sender, DevExpress.XtraPivotGrid.PivotCustomDrawCellEventArgs e)
        {
            try
            {

                if (Val.ToString(e.DataField.FieldName) == "PCS")
                {
                    PivotDrillDownDataSource ds = e.CreateDrillDownDataSource();
                    if (ds.RowCount > 0)
                    {
                        int Sequence;
                        int Sequence1;
                        Sequence = Val.ToInt(ds.GetValue(0, "EXPSEQNO"));
                        Sequence1 = Val.ToInt(ds.GetValue(0, "GRDSEQNO"));

                        if (Sequence1 > Sequence)
                        {
                            e.Appearance.BackColor = lblPurple.BackColor;
                            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000" || e.DisplayText == "")
                            {
                                e.Appearance.ForeColor = lblPurple.BackColor;
                            }
                        }
                        else if (Sequence1 < Sequence)
                        {
                            e.Appearance.BackColor = lblGreen.BackColor;
                            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000" || e.DisplayText == "")
                            {
                                e.Appearance.ForeColor = lblGreen.BackColor;
                            }
                        }
                    }
                }


            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }

        }

        private void PvtGrdClarity_CustomDrawCell(object sender, PivotCustomDrawCellEventArgs e)
        {
            try
            {

                if (Val.ToString(e.DataField.FieldName) == "PCS")
                {
                    PivotDrillDownDataSource ds = e.CreateDrillDownDataSource();
                    if (ds.RowCount > 0)
                    {
                        int Sequence;
                        int Sequence1;
                        Sequence = Val.ToInt(ds.GetValue(0, "EXPSEQNO"));
                        Sequence1 = Val.ToInt(ds.GetValue(0, "GRDSEQNO"));

                        if (Sequence1 > Sequence)
                        {
                            e.Appearance.BackColor = lblPurple.BackColor;
                            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000" || e.DisplayText == "")
                            {
                                e.Appearance.ForeColor = lblPurple.BackColor;
                            }
                        }
                        else if (Sequence1 < Sequence)
                        {
                            e.Appearance.BackColor = lblGreen.BackColor;
                            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000" || e.DisplayText == "")
                            {
                                e.Appearance.ForeColor = lblGreen.BackColor;
                            }
                        }
                    }
                }


            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void PvtGrdCut_CustomDrawCell(object sender, PivotCustomDrawCellEventArgs e)
        {
            try
            {

                if (Val.ToString(e.DataField.FieldName) == "PCS")
                {
                    PivotDrillDownDataSource ds = e.CreateDrillDownDataSource();
                    if (ds.RowCount > 0)
                    {
                        int Sequence;
                        int Sequence1;
                        Sequence = Val.ToInt(ds.GetValue(0, "EXPSEQNO"));
                        Sequence1 = Val.ToInt(ds.GetValue(0, "GRDSEQNO"));

                        if (Sequence1 > Sequence)
                        {
                            e.Appearance.BackColor = lblPurple.BackColor;
                            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000" || e.DisplayText == "")
                            {
                                e.Appearance.ForeColor = lblPurple.BackColor;
                            }
                        }
                        else if (Sequence1 < Sequence)
                        {
                            e.Appearance.BackColor = lblGreen.BackColor;
                            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000" || e.DisplayText == "")
                            {
                                e.Appearance.ForeColor = lblGreen.BackColor;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void PvtGrdPol_CustomDrawCell(object sender, PivotCustomDrawCellEventArgs e)
        {
            try
            {

                if (Val.ToString(e.DataField.FieldName) == "PCS")
                {
                    PivotDrillDownDataSource ds = e.CreateDrillDownDataSource();
                    if (ds.RowCount > 0)
                    {
                        int Sequence;
                        int Sequence1;
                        Sequence = Val.ToInt(ds.GetValue(0, "EXPSEQNO"));
                        Sequence1 = Val.ToInt(ds.GetValue(0, "GRDSEQNO"));

                        if (Sequence1 > Sequence)
                        {
                            e.Appearance.BackColor = lblPurple.BackColor;
                            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000" || e.DisplayText == "")
                            {
                                e.Appearance.ForeColor = lblPurple.BackColor;
                            }
                        }
                        else if (Sequence1 < Sequence)
                        {
                            e.Appearance.BackColor = lblGreen.BackColor;
                            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000" || e.DisplayText == "")
                            {
                                e.Appearance.ForeColor = lblGreen.BackColor;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void PvtGrdSym_CustomDrawCell(object sender, PivotCustomDrawCellEventArgs e)
        {
            try
            {

                if (Val.ToString(e.DataField.FieldName) == "PCS")
                {
                    PivotDrillDownDataSource ds = e.CreateDrillDownDataSource();
                    if (ds.RowCount > 0)
                    {
                        int Sequence;
                        int Sequence1;
                        Sequence = Val.ToInt(ds.GetValue(0, "EXPSEQNO"));
                        Sequence1 = Val.ToInt(ds.GetValue(0, "GRDSEQNO"));

                        if (Sequence1 > Sequence)
                        {
                            e.Appearance.BackColor = lblPurple.BackColor;
                            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000" || e.DisplayText == "")
                            {
                                e.Appearance.ForeColor = lblPurple.BackColor;
                            }
                        }
                        else if (Sequence1 < Sequence)
                        {
                            e.Appearance.BackColor = lblGreen.BackColor;
                            if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000" || e.DisplayText == "")
                            {
                                e.Appearance.ForeColor = lblGreen.BackColor;
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message);
            }
        }

        private void PvtGrdColor_CellDoubleClick(object sender, PivotCellEventArgs e)
        {
            try
            {
                string StrClick = string.Empty;
                string StrTitle = string.Empty;

                GrdDet.Columns["PRDTYPE_4_Cla"].ClearFilter();
                GrdDet.Columns["PRDTYPE_1_Cla"].ClearFilter();
                GrdDet.Columns["PRDTYPE_4_Cut"].ClearFilter();
                GrdDet.Columns["PRDTYPE_1_Cut"].ClearFilter();
                GrdDet.Columns["PRDTYPE_4_Pol"].ClearFilter();
                GrdDet.Columns["PRDTYPE_1_Pol"].ClearFilter();
                GrdDet.Columns["PRDTYPE_4_Sym"].ClearFilter();
                GrdDet.Columns["PRDTYPE_1_Sym"].ClearFilter();

                string StrControlName = ((PivotGridControl)sender).Name;

                PivotDrillDownDataSource ds = e.CreateDrillDownDataSource();
                string StrExp = Val.ToString(ds.GetValue(0, "EXPNAME"));
                string StrGrd = Val.ToString(ds.GetValue(0, "GRDNAME"));
                string StrClientRefNo = mStrClientRefNo;

                if (StrControlName == "PvtGrdColor")
                {
                    StrClick = "COLOR";
                    StrTitle = "Color Comparision Of Grading [" + StrExp + "]  Vs SuratGrading [" + StrGrd + "]";
                    GrdDet.Columns["PRDTYPE_4_Col"].ClearFilter();
                    GrdDet.Columns["PRDTYPE_4_Col"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo("PRDTYPE_4_Col='" + StrGrd + "'");

                    GrdDet.Columns["PRDTYPE_1_Col"].ClearFilter();
                    GrdDet.Columns["PRDTYPE_1_Col"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo("PRDTYPE_1_Col='" + StrExp + "'");

                    xtraTabDetail.SelectedTabPageIndex = -1;
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }

        }

        private void GrdDet_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                return;
            }

            if (Val.Val(GrdDet.GetRowCellValue(e.RowHandle, "BalanceCarat")) == 0)
            {
                e.Appearance.BackColor = Color.Transparent;
            }

            string StrBase = "PRDTYPE_" + "1";

            // Set Lab And Factory Grading

            SetRowCellColor(e, StrBase, "PRDTYPE_4");

        }

        private void PvtGrdClarity_CellDoubleClick(object sender, PivotCellEventArgs e)
        {
            try
            {
                string StrClick = string.Empty;
                string StrTitle = string.Empty;

                GrdDet.Columns["PRDTYPE_4_Col"].ClearFilter();
                GrdDet.Columns["PRDTYPE_1_Col"].ClearFilter();
                GrdDet.Columns["PRDTYPE_4_Cut"].ClearFilter();
                GrdDet.Columns["PRDTYPE_1_Cut"].ClearFilter();
                GrdDet.Columns["PRDTYPE_4_Pol"].ClearFilter();
                GrdDet.Columns["PRDTYPE_1_Pol"].ClearFilter();
                GrdDet.Columns["PRDTYPE_4_Sym"].ClearFilter();
                GrdDet.Columns["PRDTYPE_1_Sym"].ClearFilter();

                string StrControlName = ((PivotGridControl)sender).Name;

                PivotDrillDownDataSource ds = e.CreateDrillDownDataSource();
                string StrExp = Val.ToString(ds.GetValue(0, "EXPNAME"));
                string StrGrd = Val.ToString(ds.GetValue(0, "GRDNAME"));
                string StrClientRefNo = mStrClientRefNo;

                if (StrControlName == "PvtGrdClarity")
                {
                    StrClick = "CLARITY";
                    StrTitle = "Clarity Comparision Of Grading [" + StrExp + "]  Vs SuratGrading [" + StrGrd + "]";

                    GrdDet.Columns["PRDTYPE_4_Cla"].ClearFilter();
                    GrdDet.Columns["PRDTYPE_4_Cla"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo("PRDTYPE_4_Cla='" + StrGrd + "'");

                    GrdDet.Columns["PRDTYPE_1_Cla"].ClearFilter();
                    GrdDet.Columns["PRDTYPE_1_Cla"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo("PRDTYPE_1_Cla='" + StrExp + "'");

                    xtraTabDetail.SelectedTabPageIndex = -1;
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }

        }

        private void PvtGrdCut_CellDoubleClick(object sender, PivotCellEventArgs e)
        {
            try
            {
                string StrClick = string.Empty;
                string StrTitle = string.Empty;

                GrdDet.Columns["PRDTYPE_4_Col"].ClearFilter();
                GrdDet.Columns["PRDTYPE_1_Col"].ClearFilter();
                GrdDet.Columns["PRDTYPE_4_Cla"].ClearFilter();
                GrdDet.Columns["PRDTYPE_1_Cla"].ClearFilter();
                GrdDet.Columns["PRDTYPE_4_Pol"].ClearFilter();
                GrdDet.Columns["PRDTYPE_1_Pol"].ClearFilter();
                GrdDet.Columns["PRDTYPE_4_Sym"].ClearFilter();
                GrdDet.Columns["PRDTYPE_1_Sym"].ClearFilter();

                string StrControlName = ((PivotGridControl)sender).Name;

                PivotDrillDownDataSource ds = e.CreateDrillDownDataSource();
                string StrExp = Val.ToString(ds.GetValue(0, "EXPNAME"));
                string StrGrd = Val.ToString(ds.GetValue(0, "GRDNAME"));
                string StrClientRefNo = mStrClientRefNo;

                if (StrControlName == "PvtGrdCut")
                {
                    StrClick = "CUT";
                    StrTitle = "Cut Comparision Of Grading [" + StrExp + "]  Vs SuratGrading [" + StrGrd + "]";
                    GrdDet.Columns["PRDTYPE_4_Cut"].ClearFilter();
                    GrdDet.Columns["PRDTYPE_4_Cut"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo("PRDTYPE_4_Cut='" + StrGrd + "'");

                    GrdDet.Columns["PRDTYPE_1_Cut"].ClearFilter();
                    GrdDet.Columns["PRDTYPE_1_Cut"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo("PRDTYPE_1_Cut='" + StrExp + "'");

                    xtraTabDetail.SelectedTabPageIndex = -1;
                }

            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void PvtGrdPol_CellDoubleClick(object sender, PivotCellEventArgs e)
        {
            try
            {
                string StrClick = string.Empty;
                string StrTitle = string.Empty;

                GrdDet.Columns["PRDTYPE_4_Col"].ClearFilter();
                GrdDet.Columns["PRDTYPE_1_Col"].ClearFilter();
                GrdDet.Columns["PRDTYPE_4_Cla"].ClearFilter();
                GrdDet.Columns["PRDTYPE_1_Cla"].ClearFilter();
                GrdDet.Columns["PRDTYPE_4_Cut"].ClearFilter();
                GrdDet.Columns["PRDTYPE_1_Cut"].ClearFilter();
                GrdDet.Columns["PRDTYPE_4_Sym"].ClearFilter();
                GrdDet.Columns["PRDTYPE_1_Sym"].ClearFilter();

                string StrControlName = ((PivotGridControl)sender).Name;

                PivotDrillDownDataSource ds = e.CreateDrillDownDataSource();
                string StrExp = Val.ToString(ds.GetValue(0, "EXPNAME"));
                string StrGrd = Val.ToString(ds.GetValue(0, "GRDNAME"));
                string StrClientRefNo = mStrClientRefNo;

                if (StrControlName == "PvtGrdPol")
                {
                    StrClick = "POLISH";
                    StrTitle = "Pol Comparision Of Grading [" + StrExp + "]  Vs SuratGrading [" + StrGrd + "]";
                    GrdDet.Columns["PRDTYPE_4_Pol"].ClearFilter();
                    GrdDet.Columns["PRDTYPE_4_Pol"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo("PRDTYPE_4_Pol='" + StrGrd + "'");

                    GrdDet.Columns["PRDTYPE_1_Pol"].ClearFilter();
                    GrdDet.Columns["PRDTYPE_1_Pol"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo("PRDTYPE_1_Pol='" + StrExp + "'");

                    xtraTabDetail.SelectedTabPageIndex = -1;
                }


            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void PvtGrdSym_CellDoubleClick(object sender, PivotCellEventArgs e)
        {
            try
            {
                string StrClick = string.Empty;
                string StrTitle = string.Empty;

                GrdDet.Columns["PRDTYPE_4_Col"].ClearFilter();
                GrdDet.Columns["PRDTYPE_1_Col"].ClearFilter();
                GrdDet.Columns["PRDTYPE_4_Cla"].ClearFilter();
                GrdDet.Columns["PRDTYPE_1_Cla"].ClearFilter();
                GrdDet.Columns["PRDTYPE_4_Cut"].ClearFilter();
                GrdDet.Columns["PRDTYPE_1_Cut"].ClearFilter();
                GrdDet.Columns["PRDTYPE_4_Pol"].ClearFilter();
                GrdDet.Columns["PRDTYPE_1_Pol"].ClearFilter();

                string StrControlName = ((PivotGridControl)sender).Name;

                PivotDrillDownDataSource ds = e.CreateDrillDownDataSource();
                string StrExp = Val.ToString(ds.GetValue(0, "EXPNAME"));
                string StrGrd = Val.ToString(ds.GetValue(0, "GRDNAME"));
                string StrClientRefNo = mStrClientRefNo;

                if (StrControlName == "PvtGrdSym")
                {
                    StrClick = "SYM";
                    StrTitle = "Sym Comparision Of Grading [" + StrExp + "]  Vs SuratGrading [" + StrGrd + "]";
                    GrdDet.Columns["PRDTYPE_4_Sym"].ClearFilter();
                    GrdDet.Columns["PRDTYPE_4_Sym"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo("PRDTYPE_4_Sym='" + StrGrd + "'");

                    GrdDet.Columns["PRDTYPE_1_Sym"].ClearFilter();
                    GrdDet.Columns["PRDTYPE_1_Sym"].FilterInfo = new DevExpress.XtraGrid.Columns.ColumnFilterInfo("PRDTYPE_1_Sym='" + StrExp + "'");

                    xtraTabDetail.SelectedTabPageIndex = -1;
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void BtnPivotExcel_Click(object sender, EventArgs e)
        {
            ExportExcelNew();
            string StrFileName = "MFG_Grading_PIVOT";
            if (Global.Confirm("Do You Want To Open File ? ") == System.Windows.Forms.DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(StrFilePath, "CMD");
            }
        }

        public string ExportExcelNew()
        {
            try
            {
                if (DTPFromDate.Checked == true)
                {
                    StrFromDate = Val.SqlDate(DTPFromDate.Value.ToShortDateString());
                }
                if (DTPToDate.Checked == true)
                {
                    StrToDate = Val.SqlDate(DTPToDate.Value.ToShortDateString());
                }
                this.Cursor = Cursors.WaitCursor;
                DataSet DS = ObjView.MFGGradingReport_Export(StrFromDate, StrToDate);
                this.Cursor = Cursors.Default;
                if (DS.Tables[0].Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }
                DataTable DTabColorStock = DS.Tables[0];

                DTabColorStock.DefaultView.Sort = "TYPESRNO";
                DTabColorStock = DTabColorStock.DefaultView.ToTable();

                this.Cursor = Cursors.WaitCursor;
                StrFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";
                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.White;
                Color FontColor = Color.Black;
                string FontName = "Calibri";
                float FontSize = 11;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int StartHeaderColumn = 2;

                int CLRROWSTART = 0;
                int CLAROWSTART = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("PLUS MINUS REPORT");

                    #region DATA Print

                    StartRow = 2;
                    EndRow = StartRow;
                    StartColumn = 2;

                    worksheet.Cells[1, StartColumn, 1, 19].Value = "PLUS MINUS REPORT";
                    worksheet.Cells[1, StartColumn, 1, 19].Merge = true;
                    worksheet.Cells[1, StartColumn, 1, 19].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, StartColumn, 1, 19].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, StartColumn, 1, 19].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, StartColumn, 1, 19].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, StartColumn, 1, 19].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, StartColumn, 1, 19].Style.Font.Name = FontName;
                    worksheet.Cells[1, StartColumn, 1, 19].Style.Font.Size = 14;
                    worksheet.Cells[1, StartColumn, 1, 19].Style.Font.Bold = true;

                    worksheet.Cells[2, StartColumn, 2, 19].Value = "From Date :- " + DTPFromDate.Value.Date.ToString("dd/MM/yyyy") + " To Date :- " + DTPToDate.Value.Date.ToString("dd/MM/yyyy");
                    worksheet.Cells[2, StartColumn, 2, 19].Merge = true;
                    worksheet.Cells[2, StartColumn, 2, 19].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[2, StartColumn, 2, 19].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[2, StartColumn, 2, 19].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[2, StartColumn, 2, 19].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[2, StartColumn, 2, 19].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[2, StartColumn, 2, 19].Style.Font.Name = FontName;
                    worksheet.Cells[2, StartColumn, 2, 19].Style.Font.Size = 14;
                    worksheet.Cells[2, StartColumn, 2, 19].Style.Font.Bold = true;

                    StartRow = StartRow + 2;
                    CLRROWSTART = CLAROWSTART = StartRow;

                    int intCnt = 0; string StrType = ""; bool SingleType = false;
                    foreach (DataRow DRow in DTabColorStock.Rows)
                    {
                        if (StrType == "")
                        {
                            StrType = Val.ToString(DRow["TYPE"]);
                            SingleType = true;
                        }
                        else if (StrType == Val.ToString(DRow["TYPE"]))
                        {
                            SingleType = false;
                        }
                        else if (StrType != Val.ToString(DRow["TYPE"]))
                        {
                            StrType = Val.ToString(DRow["TYPE"]);
                            SingleType = true;
                        }

                        if (SingleType == true)
                        {
                            #region Add Data
                            if (intCnt == 0)
                            {
                                CLAROWSTART = StartRow;
                                intCnt = 1;
                                StartColumn = 2;
                                StartHeaderColumn = 2;
                                EndRow = StartRow;
                                StartRow = CLRROWSTART;
                            }
                            else
                            {
                                CLRROWSTART = StartRow;
                                intCnt = 0;
                                StartColumn = 12;
                                StartHeaderColumn = 12;
                                StartRow = EndRow;
                                StartRow = CLAROWSTART;
                            }

                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Value = StrType;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 7].Merge = true;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 7].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 7].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 7].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 7].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 7].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 7].Style.Font.Name = FontName;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 7].Style.Font.Size = FontSize;
                            worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 7].Style.Font.Bold = true;

                            StartRow = StartRow + 1;
                            StartColumn = StartColumn + 1;

                            DataRow[] UDRow = DTabColorStock.Select("TYPE='" + StrType + "'");
                            if (UDRow.Length == 0)
                            {
                                continue;
                            }

                            worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn - 1].Value = "NAME";
                            worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Value = "TOTALPCS";
                            worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = "UP";
                            worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = "DOWN";
                            worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = "OK";
                            worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = "UP%";
                            worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = "DOWN%";
                            worksheet.Cells[StartRow, StartColumn + 6, StartRow, StartColumn + 6].Value = "OK%";
                            worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn + 6].Style.Font.Bold = true;

                            StartRow = StartRow + 1;

                            foreach (DataRow DRowDetail in UDRow)
                            {
                                worksheet.Cells[StartRow, StartHeaderColumn, StartRow, StartHeaderColumn].Value = Val.ToString(DRowDetail["NAME"]);
                                worksheet.Cells[StartRow, StartHeaderColumn, StartRow, StartHeaderColumn].Style.Font.Bold = true;
                                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Value = Val.Val(DRowDetail["TOTALPCS"]);
                                worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = Val.Val(DRowDetail["UP"]);
                                worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Value = Val.Val(DRowDetail["DOWN"]);
                                worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Value = Val.Val(DRowDetail["OK"]);
                                worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Value = Val.Val(DRowDetail["UP_PER"]);
                                worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Value = Val.Val(DRowDetail["DOWN_PER"]);
                                worksheet.Cells[StartRow, StartColumn + 6, StartRow, StartColumn + 6].Value = Val.Val(DRowDetail["OK_PER"]);

                                string TotalPcLoopColumnText = Global.ColumnIndexToColumnLetter(DTabColorStock.Columns["TOTALPCS"].Ordinal + StartColumn - 3);
                                string UPLoopColumnText = Global.ColumnIndexToColumnLetter(DTabColorStock.Columns["UP"].Ordinal + StartColumn - 3);
                                string DOWNLoopColumnText = Global.ColumnIndexToColumnLetter(DTabColorStock.Columns["DOWN"].Ordinal + StartColumn - 3);
                                string OKLoopColumnText = Global.ColumnIndexToColumnLetter(DTabColorStock.Columns["OK"].Ordinal + StartColumn - 3);

                                worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Formula = "ROUND((" + UPLoopColumnText + StartRow + "/" + TotalPcLoopColumnText + StartRow + " ) * 100,2)";
                                worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Formula = "ROUND((" + DOWNLoopColumnText + StartRow + "/" + TotalPcLoopColumnText + StartRow + " ) * 100,2)";
                                worksheet.Cells[StartRow, StartColumn + 6, StartRow, StartColumn + 6].Formula = "ROUND((" + OKLoopColumnText + StartRow + "/" + TotalPcLoopColumnText + StartRow + " ) * 100,2)";

                                worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 6].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 6].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 6].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 6].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                                worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                                worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 6].Style.Font.Name = FontName;
                                worksheet.Cells[StartRow - 1, StartColumn - 1, StartRow, StartColumn + 6].Style.Font.Size = FontSize;

                                StartRow = StartRow + 1;
                            }


                            int IntTotRow = StartRow - 1;
                            string TotalPcColumnText = Global.ColumnIndexToColumnLetter(DTabColorStock.Columns["TOTALPCS"].Ordinal + StartColumn - 3);
                            string UPColumnText = Global.ColumnIndexToColumnLetter(DTabColorStock.Columns["UP"].Ordinal + StartColumn - 3);
                            string DOWNColumnText = Global.ColumnIndexToColumnLetter(DTabColorStock.Columns["DOWN"].Ordinal + StartColumn - 3);
                            string OKColumnText = Global.ColumnIndexToColumnLetter(DTabColorStock.Columns["OK"].Ordinal + StartColumn - 3);
                            string UP_PERColumnText = Global.ColumnIndexToColumnLetter(DTabColorStock.Columns["UP_PER"].Ordinal + StartColumn - 3);
                            string DOWN_PERColumnText = Global.ColumnIndexToColumnLetter(DTabColorStock.Columns["DOWN_PER"].Ordinal + StartColumn - 3);
                            string OK_PERColumnText = Global.ColumnIndexToColumnLetter(DTabColorStock.Columns["OK_PER"].Ordinal + StartColumn - 3);

                            int StartCalRow = StartRow - UDRow.Length;
                            worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn - 1].Value = "Total";
                            worksheet.Cells[StartRow, StartColumn + 0, StartRow, StartColumn + 0].Formula = "ROUND(SUBTOTAL(9," + TotalPcColumnText + StartCalRow + ":" + TotalPcColumnText + IntTotRow + "),0)";
                            worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Formula = "ROUND(SUBTOTAL(9," + UPColumnText + StartCalRow + ":" + UPColumnText + IntTotRow + "),0)";
                            worksheet.Cells[StartRow, StartColumn + 2, StartRow, StartColumn + 2].Formula = "ROUND(SUBTOTAL(9," + DOWNColumnText + StartCalRow + ":" + DOWNColumnText + IntTotRow + "),0)";
                            worksheet.Cells[StartRow, StartColumn + 3, StartRow, StartColumn + 3].Formula = "ROUND(SUBTOTAL(9," + OKColumnText + StartCalRow + ":" + OKColumnText + IntTotRow + "),0)";
                            worksheet.Cells[StartRow, StartColumn + 4, StartRow, StartColumn + 4].Formula = "ROUND((" + UPColumnText + StartRow + "/" + TotalPcColumnText + StartRow + " ) * 100,2)";
                            worksheet.Cells[StartRow, StartColumn + 5, StartRow, StartColumn + 5].Formula = "ROUND((" + DOWNColumnText + StartRow + "/" + TotalPcColumnText + StartRow + " ) * 100,2)";
                            worksheet.Cells[StartRow, StartColumn + 6, StartRow, StartColumn + 6].Formula = "ROUND((" + OKColumnText + StartRow + "/" + TotalPcColumnText + StartRow + " ) * 100,2)";
                            worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn + 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn + 6].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn + 6].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn + 6].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn + 6].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn + 6].Style.Font.Name = FontName;
                            worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn + 6].Style.Font.Size = FontSize;
                            worksheet.Cells[StartRow, StartColumn - 1, StartRow, StartColumn + 6].Style.Font.Bold = true;

                            StartRow = StartRow + 3;

                            #endregion
                        }
                    }

                    #endregion

                    worksheet.Cells.AutoFitColumns();

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

        public static string ExportExcelHeaderMain(String pstrHeader, ExcelWorksheet worksheet, int pCol)
        {
            if (pstrHeader.ToUpper() == "NAME")
            {
                worksheet.Column(pCol).Width = 10;
                return "Name";
            }
            else if (pstrHeader.ToUpper() == "TOTALPCS")
            {
                worksheet.Column(pCol).Width = 10;
                return "TotalPcs";
            }
            else if (pstrHeader.ToUpper() == "UP")
            {
                worksheet.Column(pCol).Width = 10;
                return "Up";
            }
            else if (pstrHeader.ToUpper() == "DOWN")
            {
                worksheet.Column(pCol).Width = 10;
                return "Down";
            }
            else if (pstrHeader.ToUpper() == "OK")
            {
                worksheet.Column(pCol).Width = 10;
                return "Ok";
            }
            else if (pstrHeader.ToUpper() == "UP_PER")
            {
                worksheet.Column(pCol).Width = 10;
                return "Up%";
            }
            else if (pstrHeader.ToUpper() == "DOWN_PER")
            {
                worksheet.Column(pCol).Width = 10;
                return "Down%";
            }
            else if (pstrHeader.ToUpper() == "OK_PER")
            {
                worksheet.Column(pCol).Width = 10;
                return "Ok%";
            }
            return "";
        }

        private void txtClientRefNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String str1 = "";
                if (txtClientRefNo.Text.Trim().Contains("\t\n"))
                {
                    str1 = txtClientRefNo.Text.Trim().Replace("\t\n", ",");
                }
                else
                {
                    str1 = txtClientRefNo.Text.Trim().Replace("\n", ",");
                    str1 = str1.Replace("\r", "");
                }

                txtClientRefNo.Text = str1;
                //rTxtStoneCertiMfgMemo.Text = str1.Trim().TrimStart().TrimEnd();
                txtClientRefNo.Select(txtClientRefNo.Text.Length, 0);
                //rTxtStoneCertiMfgMemo.Text = rTxtStoneCertiMfgMemo.Text.Trim().TrimStart().TrimEnd();

                //lblTotalCount.Text = "(" + rTxtStoneCertiMfgMemo.Text.Split(',').Length + ")";


            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtClientRefNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                BtnSearch_Click(null, null);
            }
        }

        private void txtClientRefNo_MouseDown(object sender, MouseEventArgs e)
        {
            if (txtClientRefNo.Focus())
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    PasteData = Convert.ToString(PasteclipData.GetData(System.Windows.Forms.DataFormats.Text));
                }
            }
        }

        private void GrdDet_CustomSummaryCalculate_1(object sender, CustomSummaryEventArgs e)
        {
            try
            {
                // Get all field names where the summary type is Custom
                List<string> customSummaryFields = new List<string>();


                foreach (DevExpress.XtraGrid.GridSummaryItem summaryItem in GrdDet.Columns.OfType<GridColumn>().SelectMany(col => col.Summary))
                {
                    if (summaryItem.SummaryType == DevExpress.Data.SummaryItemType.Custom)
                    {
                        customSummaryFields.Add(summaryItem.FieldName);
                    }
                }

                // Now you have all field names with Custom summary type in the customSummaryFields list
                foreach (string fieldName in customSummaryFields)
                {
                    string StrCtsColumn = "";
                    string StrAmountColumn = fieldName.Replace("PricePerCarat", "Amount");
                    if (fieldName == "PRDTYPE_FINAL_COSTING_PricePerCarat")
                    {
                        StrCtsColumn = "PRDTYPE_EMPLOYEE_GRADING_COSTING_ExpCarat";
                    }
                    else
                    {
                        StrCtsColumn = fieldName.Replace("PricePerCarat", "ExpCarat");
                    }

                    Amount = Val.ToDouble(GrdDet.Columns[StrAmountColumn].SummaryItem.SummaryValue);
                    Carat = Val.ToDouble(GrdDet.Columns[StrCtsColumn].SummaryItem.SummaryValue);

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo(fieldName) == 0)
                    {
                        if (Val.Val(Carat) > 0)
                            e.TotalValue = Math.Round((Val.ToInt32(Amount) / Val.Val(Carat)), 3);
                        else
                            e.TotalValue = 0;
                    }
                }

            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void GrdDet_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }

                GrdDet.PostEditor();

                if (e.Column.FieldName.Contains("Discount"))
                {
                    DataRow DRow = GrdDet.GetDataRow(e.RowHandle);

                    string pStrStockNo = Val.ToString(DRow["ClientRefNo"]);
                    Double pDouCostDiscount = Val.Val(DRow["PRDTYPE_FINAL_COSTING_Discount"]);
                    Double pDouCostPricePerCarat = 0;

                    DataTable DTab = ObjView.GetCostRapaport(pStrStockNo, pDouCostDiscount, pDouCostPricePerCarat);

                    DRow["PRDTYPE_FINAL_COSTING_Discount"] = 0;
                    DRow["PRDTYPE_FINAL_COSTING_PricePerCarat"] = 0;
                    DRow["PRDTYPE_FINAL_COSTING_Rapaport"] = 0;
                    DRow["PRDTYPE_FINAL_COSTING_Amount"] = 0;
                    DRow["PRDTYPE_FINAL_COSTING_LabCharge"] = 0;
                    DRow["PRDTYPE_FINAL_COSTING_AmountWithLabCharge"] = 0;

                    if (DTab.Rows.Count != 0)
                    {
                        DRow["PRDTYPE_FINAL_COSTING_Discount"] = Val.Val(DTab.Rows[0]["Discount"]);
                        DRow["PRDTYPE_FINAL_COSTING_PricePerCarat"] = Val.Val(DTab.Rows[0]["PricePerCarat"]);
                        DRow["PRDTYPE_FINAL_COSTING_Rapaport"] = Val.Val(DTab.Rows[0]["Rapaport"]);
                        DRow["PRDTYPE_FINAL_COSTING_Amount"] = Val.Val(DTab.Rows[0]["Amount"]);
                        DRow["PRDTYPE_FINAL_COSTING_LabCharge"] = Val.Val(DTab.Rows[0]["LabCharge"]);
                        DRow["PRDTYPE_FINAL_COSTING_AmountWithLabCharge"] = Val.Val(DTab.Rows[0]["AmountWithLabCharge"]);
                    }
                }

                else if (e.Column.FieldName.Contains("PricePerCarat"))
                {
                    DataRow DRow = GrdDet.GetDataRow(e.RowHandle);

                    string pStrStockNo = Val.ToString(DRow["ClientRefNo"]);
                    Double pDouCostDiscount = 0;
                    Double pDouCostPricePerCarat = Val.Val(DRow["PRDTYPE_FINAL_COSTING_PricePerCarat"]);

                    DataTable DTab = ObjView.GetCostRapaport(pStrStockNo, pDouCostDiscount, pDouCostPricePerCarat);

                    DRow["PRDTYPE_FINAL_COSTING_Discount"] = 0;
                    DRow["PRDTYPE_FINAL_COSTING_PricePerCarat"] = 0;
                    DRow["PRDTYPE_FINAL_COSTING_Rapaport"] = 0;
                    DRow["PRDTYPE_FINAL_COSTING_Amount"] = 0;
                    DRow["PRDTYPE_FINAL_COSTING_LabCharge"] = 0;
                    DRow["PRDTYPE_FINAL_COSTING_AmountWithLabCharge"] = 0;

                    if (DTab.Rows.Count != 0)
                    {
                        DRow["PRDTYPE_FINAL_COSTING_Discount"] = Val.Val(DTab.Rows[0]["Discount"]);
                        DRow["PRDTYPE_FINAL_COSTING_PricePerCarat"] = Val.Val(DTab.Rows[0]["PricePerCarat"]);
                        DRow["PRDTYPE_FINAL_COSTING_Rapaport"] = Val.Val(DTab.Rows[0]["Rapaport"]);
                        DRow["PRDTYPE_FINAL_COSTING_Amount"] = Val.Val(DTab.Rows[0]["Amount"]);
                        DRow["PRDTYPE_FINAL_COSTING_LabCharge"] = Val.Val(DTab.Rows[0]["LabCharge"]);
                        DRow["PRDTYPE_FINAL_COSTING_AmountWithLabCharge"] = Val.Val(DTab.Rows[0]["AmountWithLabCharge"]);
                    }
                }
            }
            catch (Exception Ex)
            {
                Global.MessageError(Ex.Message.ToString());
            }
        }

        private void txtKapanName_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    BtnSearch_Click(null, null);
                }
            }
            catch(Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void txtKapanName_MouseDown(object sender, MouseEventArgs e)
        {
            if (txtKapanName.Focus())
            {
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    PasteData = Convert.ToString(PasteclipData.GetData(System.Windows.Forms.DataFormats.Text));
                }
            }
        }

        private void txtKapanName_TextChanged(object sender, EventArgs e)
        {

            try
            {
                String str1 = "";
                if (txtKapanName.Text.Trim().Contains("\t\n"))
                {
                    str1 = txtKapanName.Text.Trim().Replace("\t\n", ",");
                }
                else
                {
                    str1 = txtKapanName.Text.Trim().Replace("\n", ",");
                    str1 = str1.Replace("\r", "");
                }

                txtKapanName.Text = str1;
                //rTxtStoneCertiMfgMemo.Text = str1.Trim().TrimStart().TrimEnd();
                txtKapanName.Select(txtKapanName.Text.Length, 0);
                //rTxtStoneCertiMfgMemo.Text = rTxtStoneCertiMfgMemo.Text.Trim().TrimStart().TrimEnd();

                //lblTotalCount.Text = "(" + rTxtStoneCertiMfgMemo.Text.Split(',').Length + ")";


            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog OpenFileDialog = new OpenFileDialog();
                OpenFileDialog.Filter = "Excel Files (*.xls,*.xlsx)|*.xls;*.xlsx;";
                if (OpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtBackPriceFileName.Text = OpenFileDialog.FileName;

                    string extension = Path.GetExtension(txtBackPriceFileName.Text.ToString());
                    string destinationPath = Application.StartupPath + @"\StoneFiles\" + Path.GetFileName(txtBackPriceFileName.Text);
                    destinationPath = destinationPath.Replace(extension, ".xlsx");
                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }
                    File.Copy(txtBackPriceFileName.Text, destinationPath);
                    DtabBackPriceUpdate = Global.GetDataTableFromExcel(destinationPath, true);

                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }
                }
                OpenFileDialog.Dispose();
                OpenFileDialog = null;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnPriceFileUpload_Click(object sender, EventArgs e)
        {
            try
            {
                if (Val.ToString(txtBackPriceFileName.Text).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Selet File That You Want To Update...");
                    txtBackPriceFileName.Focus();
                    return;
                }
                string StrStonePriceXml = "";
               
                DtabBackPriceUpdate.TableName = "Table1";
                using (StringWriter sw = new StringWriter())
                {
                    DtabBackPriceUpdate.WriteXml(sw);
                    StrStonePriceXml = sw.ToString();
                }

                DataTable DtabParamUpdate = ObjView.GetStonePriceExcelWiseGetData(StrStonePriceXml);

                BtnSearch_Click(null, null);
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void lblSampleExcelFile_Click(object sender, EventArgs e)
        {
            try
            {
                string StrFilePathDestination = "";

                StrFilePathDestination = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\CostingPriceUpdateFormat" + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.Day.ToString() + ".xlsx";
                if (File.Exists(StrFilePathDestination))
                {
                    File.Delete(StrFilePathDestination);
                }
                File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\Format\\CostingPriceUpdateFormat.xlsx", StrFilePathDestination);

                System.Diagnostics.Process.Start(StrFilePathDestination, "CMD");
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
    }
}
