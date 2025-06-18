using BusLib.Configuration;
using BusLib.Rapaport;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.XtraEditors;
using OfficeOpenXml;
using MahantExport.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Data;
using System.Text.RegularExpressions;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Columns;

namespace MahantExport.Stock
{
    public partial class FrmSingleLiveStockNew : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();

        private bool isPasteAction = false;
        private const Keys PasteKeys = Keys.Control | Keys.V;

        String PasteData = "";
        IDataObject PasteclipData = Clipboard.GetDataObject();

        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        BOFindRap ObjRap = new BOFindRap();

        LiveStockProperty mProperty = new LiveStockProperty();

        DataTable DTabParameter = new DataTable();

        DataTable DtabLiveStockDetail = new DataTable();
        DataTable DTabSize = new DataTable();
        DataTable DTabDiamondType = new DataTable();


        Color mSelectedColor = Color.FromArgb(192, 0, 0);
        Color mDeSelectColor = Color.Black;
        Color mSelectedBackColor = Color.FromArgb(255, 224, 192);
        Color mDSelectedBackColor = Color.WhiteSmoke;

        private FrmMemoEntryNew FrmMemoEntryNew;
        private FrmMemoEntry FrmMemoEntry;


        string StrEmail = "";
        string mStrStockType = "";
        bool chkOnAndOff;
        string pStrWhatsappMessage = "";

        string pStrDiamondType = "";
        string pStrStockNo = "";
        double DouCarat = 0;
        double DouCostRapaport = 0;
        double DouCostRapaportAmt = 0;
        double DouCostDisc = 0;
        double DouCostPricePerCarat = 0;
        double DouCostAmount = 0;

        double DouSaleRapaport = 0;
        double DouSaleRapaportAmt = 0;
        double DouSaleDisc = 0;
        double DouSalePricePerCarat = 0;
        double DouSaleAmount = 0;

        double DouExpRapaport = 0;
        double DouExpRapaportAmt = 0;
        double DouExpDisc = 0;
        double DouExpPricePerCarat = 0;
        double DouExpAmount = 0;

        double DouOfferRapaport = 0;
        double DouOfferRapaportAmt = 0;
        double DouOfferDisc = 0;
        double DouOfferPricePerCarat = 0;
        double DouOfferAmount = 0;

        double DouMemoRapaport = 0;
        double DouMemoRapaportAmt = 0;
        double DouMemoDisc = 0;
        double DouMemoPricePerCarat = 0;
        double DouMemoAmount = 0;

        double DouRapnetRapaport = 0;
        double DouRapnetRapaportAmt = 0;
        double DouRapnetDisc = 0;
        double DouRapnetPricePerCarat = 0;
        double DouRapnetAmount = 0;

        double DouJARapaport = 0;
        double DouJARapaportAmt = 0;
        double DouJADisc = 0;
        double DouJAPricePerCarat = 0;
        double DouJAAmount = 0;

        double DouMFGRapaport = 0;
        double DouMFGRapaportAmt = 0;

        double DouMFGDisc = 0;
        double DouMFGPricePerCarat = 0;
        double DouMFGAmount = 0;

        public FrmSingleLiveStockNew()
        {
            InitializeComponent();

        }
        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            CmbDiamondType.SelectedIndex = 0;

            PanelGrdCts.Visible = false;
            PanelGrdCts.SendToBack();
            cPanel5.Size = new System.Drawing.Size(755, 34); // Width = 200, Height = 150
            panel5.Size = new System.Drawing.Size(755, 35); // Width = 200, Height = 150
            panel6.Size = new System.Drawing.Size(755, 35); // Width = 200, Height = 150
            panel7.Size = new System.Drawing.Size(755, 36); // Width = 200, Height = 150

            PanelSizeNew.Size = new System.Drawing.Size(755, 30); // Width = 200, Height = 150
            PanelShape.Size = new System.Drawing.Size(755, 31); // Width = 200, Height = 150
            PanelColor.Size = new System.Drawing.Size(755, 31); // Width = 200, Height = 150
            PanelClarity.Size = new System.Drawing.Size(755, 32); // Width = 200, Height = 150
            this.Show();
            MainGrdSize.DataSource = DTabSize;
            GrdSize.RefreshData();
            SetControl();
        }
        public void ShowForm(string StrFromSize, string StrToSize, string StrColor_ID, string StrClarity_ID, string StrShapName, string StrStatus, string StrFromDate, string StrToDate)  //Call When Double Click On Current Stock Color Clarity Wise Report Data
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            mProperty = new LiveStockProperty();            

            if (StrColor_ID == "0")
            {
                StrColor_ID = "";
            }
            if (StrClarity_ID == "0")
            {
                StrClarity_ID = "";
            }

           
            mProperty.FROMCARAT1 = Val.Val(StrFromSize);
            mProperty.TOCARAT1 = Val.Val(StrToSize);
            mProperty.MULTYCOLOR_ID = StrColor_ID;
            mProperty.MULTYCLARITY_ID = StrClarity_ID;
            mProperty.WEBSTATUS = StrStatus;

            mProperty.SALESFROMDATE = StrFromDate;
            mProperty.SALESTODATE = StrToDate;
            mProperty.AVAILBLEFROMDATE = StrFromDate;
            mProperty.AVAILBLETODATE = StrToDate;

            mProperty.MULTYSHAPE_ID = Global.FILTERSHAPE_ID;
            mProperty.MULTYCUT_ID = Global.FILTERCUT_ID;
            mProperty.MULTYPOL_ID = Global.FILTERPOL_ID;
            mProperty.MULTYSYM_ID = Global.FILTERSYM_ID;
            mProperty.MULTYMILKY_ID = Global.FILTERMILKY_ID;
            mProperty.MULTYLAB_ID = Global.FILTERLAB_ID;
            mProperty.MULTYCOLORSHADE_ID = Global.FILTERCOLORSHADE_ID;
            mProperty.MULTYTABLEBLACK_ID = Global.FILTERTABLEBLACKINC_ID;
            mProperty.MULTYFL_ID = Global.FILTERFL_ID;
            mProperty.MULTYSIDEBLACK_ID = Global.FILTERSIDEBLACKINC_ID;//Ens As gunjan


            txtFromCts1.Text = StrFromSize;
            txtToCts1.Text = StrToSize;


            string Str = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdDetail.Name);

            if (Str != "")
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                MemoryStream stream = new MemoryStream(byteArray);
                GrdDetail.RestoreLayoutFromStream(stream);
            }
            BtnRefresh_Click(null, null);
           
            this.Show();

        }
        public string SetSelectedBtnID(Panel StrPanel , string Valuse)
        {
            string StrSelectedID = "";
            for (int i = 0; i < StrPanel.Controls.Count; i++)
            {
                if (StrPanel.Controls[i].BackColor == mSelectedBackColor)
                {
                    StrSelectedID += StrPanel.Controls[i].Tag + ",";
                }
            }
            return StrSelectedID;
        }
        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjStock);
        }
        public  DataTable GetSelectedRowToTable()
        {
            Int32[] selectedRowHandles = GrdDetail.GetSelectedRows();
            DataTable resultTable = DtabLiveStockDetail.Clone();
            for (int i = 0; i < selectedRowHandles.Length; i++)
            {
                int J = selectedRowHandles[i];
                DataRowView DR = (DataRowView)GrdDetail.GetRow(J);
                resultTable.Rows.Add(DR.Row.ItemArray);
            }
            return resultTable;
        }

        #region DesignDynamiButtons

        public void DesignSystemButtion(Panel PNL, string pStrParaType, string pStrDisplayText, string toolTips, int pIntHeight, int pIntWidth)
        {
            DataRow[] UDRow = DTabParameter.Select("ParaType = '" + pStrParaType + "'");

            if (UDRow.Length == 0)
            {
                return;
            }

            DataTable DTab = UDRow.CopyToDataTable();
            DTab.DefaultView.Sort = "SequenceNo";
            DTab = DTab.DefaultView.ToTable();

            PNL.Controls.Clear();

            int IntI = 0;
            //foreach (DataRow DRow in DTab.Rows)
            //{
            //    AxonContLib.cButton ValueList = new AxonContLib.cButton();
            //    ValueList.Text = DRow[pStrDisplayText].ToString();
            //    ValueList.FlatStyle = FlatStyle.Flat;
            //    ValueList.Width = pIntWidth;
            //    ValueList.Height = pIntHeight;
            //    ValueList.Tag = DRow["PARA_ID"].ToString();
            //    ValueList.AccessibleDescription = Val.ToString(DRow["PARACODE"]);
            //    ValueList.ToolTips = toolTips;
            //    ValueList.AutoSize = true;
            //    ValueList.Click += new EventHandler(cButton_Click);
            //    ValueList.Cursor = Cursors.Hand;
            //    ValueList.Font = new Font("Tahoma", 9, FontStyle.Regular);
            //    ValueList.ForeColor = mDeSelectColor;
            //    ValueList.BackColor = mDSelectedBackColor;

            //    PNL.Controls.Add(ValueList);

            //    IntI++;
            //}
            foreach (DataRow DRow in DTab.Rows)
            {
                AxonContLib.cButton ValueList = new AxonContLib.cButton();
                ValueList.Text = DRow[pStrDisplayText].ToString();
                ValueList.FlatStyle = FlatStyle.Flat;

                // AutoSize property should be set to true to adjust size based on content
                ValueList.AutoSize = true;
                ValueList.AutoSizeMode = AutoSizeMode.GrowAndShrink;

                // You can still set the height manually, as you want
                ValueList.Height = pIntHeight;

                ValueList.Tag = DRow["PARA_ID"].ToString();
                ValueList.AccessibleDescription = Val.ToString(DRow["PARACODE"]);
                ValueList.ToolTips = toolTips;
                ValueList.Click += new EventHandler(cButton_Click);
                ValueList.Cursor = Cursors.Hand;
                ValueList.Font = new Font("Tahoma", 9, FontStyle.Regular);
                ValueList.ForeColor = mDeSelectColor;
                ValueList.BackColor = mDSelectedBackColor;

                PNL.Controls.Add(ValueList);

                IntI++;
            }
        }

        private void cButton_Click(object sender, EventArgs e)
        {
            try
            {
                AxonContLib.cButton btn = (AxonContLib.cButton)sender;
                if (btn.ForeColor == mSelectedColor)
                {
                    btn.ForeColor = mDeSelectColor;
                    btn.BackColor = mDSelectedBackColor;
                    btn.AccessibleName = "true";
                }
                else
                {
                    btn.ForeColor = mSelectedColor;
                    btn.BackColor = mSelectedBackColor;
                    btn.AccessibleName = "true";
                }
                if (MainGrdDetail.Enabled == false)
                {
                    Global.MessageError("Grid Is Unable To Update");
                    return;
                }
                if(btn.ToolTips == "LOCATION")
                {
                    string StrStock_ID = "";
                    Int64 IntLocation_ID = 0;

                    DataTable DTab = GetSelectedRowToTable();

                    if (DTab.Rows.Count <= 0)
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("Please Select AtLeast One Record For Update Location");
                        return;
                    }
                    if (DTab.Rows.Count > 0)
                    {
                        var list = DTab.AsEnumerable().Select(r => r["STOCK_ID"].ToString());
                        StrStock_ID = string.Join(",", list);
                    }
                    else
                    {
                        StrStock_ID = Val.ToString(GrdDetail.GetFocusedRowCellValue("STOCK_ID"));
                    }
                    IntLocation_ID = Val.ToInt64(btn.Tag);

                    if (Global.Confirm("Are You Sure Want To Update Location In This Entry ? ") == System.Windows.Forms.DialogResult.Yes)
                    {
                        LiveStockProperty Property = new LiveStockProperty();

                        Property = ObjStock.LiveStockUpdateLocation(Property, StrStock_ID, IntLocation_ID);

                        Global.Message(Property.ReturnMessageDesc);
                        if (Property.ReturnMessageType == "SUCCESS")
                        {

                            BtnRefresh_Click(sender, e);
                            btn.ForeColor = mDeSelectColor;
                            btn.BackColor = mDSelectedBackColor;
                            btn.AccessibleName = "true";
                        }
                    }
                }
            }
            catch (Exception EX)
            {
                this.Cursor = Cursors.WaitCursor;
                Global.MessageError(EX.Message);
                return;
            }

        }
        public void DesignSystemDiamondTypeButtion(DataTable DTab, Panel PNL, string pStrDisplayText, string toolTips, int pIntHeight, int pIntWidth)
        {
          
            if (DTab.Rows.Count == 0)
            {
                return;
            }
           
            PNL.Controls.Clear();

            int IntI = 0;
            //foreach (DataRow DRow in DTab.Rows)
            //{
            //    AxonContLib.cButton ValueList = new AxonContLib.cButton();
            //    ValueList.Text = DRow[pStrDisplayText].ToString();
            //    ValueList.FlatStyle = FlatStyle.Flat;
            //    ValueList.Width = pIntWidth;
            //    ValueList.Height = pIntHeight;
            //    ValueList.Tag = DRow["VALUE"].ToString();
            //    ValueList.AccessibleDescription = Val.ToString(DRow["TYPE"]);
            //    ValueList.ToolTips = toolTips;
            //    ValueList.AutoSize = true;
            //    ValueList.Click += new EventHandler(cButton_Click);
            //    ValueList.Cursor = Cursors.Hand;
            //    ValueList.Font = new Font("Tahoma", 9, FontStyle.Regular);
            //    ValueList.ForeColor = mDeSelectColor;
            //    ValueList.BackColor = mDSelectedBackColor;

            //    PNL.Controls.Add(ValueList);

            //    IntI++;
            //}
            foreach (DataRow DRow in DTab.Rows)
            {
                AxonContLib.cButton ValueList = new AxonContLib.cButton();
                ValueList.Text = DRow[pStrDisplayText].ToString();
                ValueList.FlatStyle = FlatStyle.Flat;

                // AutoSize property should be set to true to adjust size based on content
                ValueList.AutoSize = true;
                ValueList.AutoSizeMode = AutoSizeMode.GrowAndShrink;

                // You can still set the height manually, as you want
                ValueList.Height = pIntHeight;

                ValueList.Tag = DRow["VALUE"].ToString();
                ValueList.AccessibleDescription = Val.ToString(DRow["TYPE"]);
                ValueList.ToolTips = toolTips;
                ValueList.Click += new EventHandler(cButton_Click);
                ValueList.Cursor = Cursors.Hand;
                ValueList.Font = new Font("Tahoma", 9, FontStyle.Regular);
                ValueList.ForeColor = mDeSelectColor;
                ValueList.BackColor = mDSelectedBackColor;

                PNL.Controls.Add(ValueList);

                IntI++;
            }
        }

        public void DesignSystemSizeButtion(DataTable DTab,Panel PNL, string pStrDisplayText, string toolTips, int pIntHeight, int pIntWidth)
        {
            //DataRow[] UDRow = DTabParameter.Select("ParaType = '" + pStrParaType + "'");

            //if (UDRow.Length == 0)
            //{
            //    return;
            //}

            //DataTable DTab = UDRow.CopyToDataTable();
            if (DTab.Rows.Count == 0)
            {
                return;
            }
            DTab.DefaultView.Sort = "SequenceNo";
            DTab = DTab.DefaultView.ToTable();

            PNL.Controls.Clear();

            int IntI = 0;
            //foreach (DataRow DRow in DTab.Rows)
            //{
            //    AxonContLib.cButton ValueList = new AxonContLib.cButton();
            //    ValueList.Text = DRow[pStrDisplayText].ToString();
            //    ValueList.FlatStyle = FlatStyle.Flat;

            //    ValueList.Width = pIntWidth;
            //    ValueList.Height = pIntHeight;
            //    ValueList.Tag = DRow["Size_ID"].ToString();
            //    ValueList.AccessibleDescription = Val.ToString(DRow["SizeName"]);
            //    ValueList.ToolTips = toolTips;
            //    ValueList.AutoSize = true;
            //    ValueList.Click += new EventHandler(cButton_Click);
            //    ValueList.Cursor = Cursors.Hand;
            //    ValueList.Font = new Font("Tahoma", 9, FontStyle.Regular);
            //    ValueList.ForeColor = mDeSelectColor;
            //    ValueList.BackColor = mDSelectedBackColor;

            //    PNL.Controls.Add(ValueList);

            //    IntI++;
            //}
            foreach (DataRow DRow in DTab.Rows)
            {
                AxonContLib.cButton ValueList = new AxonContLib.cButton();
                ValueList.Text = DRow[pStrDisplayText].ToString();
                ValueList.FlatStyle = FlatStyle.Flat;

                // AutoSize property should be set to true to adjust size based on content
                ValueList.AutoSize = true;
                ValueList.AutoSizeMode = AutoSizeMode.GrowAndShrink;

                // You can still set the height manually, as you want
                ValueList.Height = pIntHeight;

                ValueList.Tag = DRow["Size_ID"].ToString();
                ValueList.AccessibleDescription = Val.ToString(DRow["SizeName"]);
                ValueList.ToolTips = toolTips;
                ValueList.Click += new EventHandler(cButton_Click);
                ValueList.Cursor = Cursors.Hand;
                ValueList.Font = new Font("Tahoma", 9, FontStyle.Regular);
                ValueList.ForeColor = mDeSelectColor;
                ValueList.BackColor = mDSelectedBackColor;

                PNL.Controls.Add(ValueList);

                IntI++;
            }

        }

        private void SetControl()
        {
            DTabSize = new DataTable();
            DTabSize.Columns.Add(new DataColumn("FROMCARAT", typeof(Double)));
            DTabSize.Columns.Add(new DataColumn("TOCARAT", typeof(Double)));

            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());

            DTabDiamondType = new DataTable();
            DTabDiamondType.Columns.Add(new DataColumn("TYPE", typeof(string)));
            DTabDiamondType.Columns.Add(new DataColumn("VALUE", typeof(string)));

            DataRow row = DTabDiamondType.NewRow();
            row["TYPE"] = "ALL";
            row["VALUE"] = "ALL";
            DTabDiamondType.Rows.Add(row);

            DataRow row1 = DTabDiamondType.NewRow();
            row1["TYPE"] = "NAT";
            row1["VALUE"] = "NATURAL";
            DTabDiamondType.Rows.Add(row1);

            DataRow row2 = DTabDiamondType.NewRow();
            row2["TYPE"] = "CVD";
            row2["VALUE"] = "CVD";
            DTabDiamondType.Rows.Add(row2);

            DataRow row3 = DTabDiamondType.NewRow();
            row3["TYPE"] = "HPHT";
            row3["VALUE"] = "HPHT";
            DTabDiamondType.Rows.Add(row3);

            MainGrdSize.DataSource = DTabSize;
            GrdSize.RefreshData();

            DTabParameter = ObjRap.GetAllParameterTable();
            DataTable DTabSizeNew = ObjRap.GetSizeData();

            DesignSystemSizeButtion(DTabSizeNew, PanelSizeNew, "FromCarat", "SIZE", 30, 60);
            DesignSystemButtion(PanelShape, "SHAPE", "PARACODE", "SHAPE", 30, 45);
            DesignSystemButtion(PanelColor, "COLOR", "PARANAME", "COLOR", 30, 45);
            DesignSystemButtion(PanelClarity, "CLARITY", "PARANAME", "CLARITY", 30, 45);
            DesignSystemButtion(PanelCut, "CUT", "PARACODE", "CUT", 30, 45);
            DesignSystemButtion(PanelPol, "POLISH", "PARACODE", "POL", 30, 45);
            DesignSystemButtion(PanelSym, "SYMMETRY", "PARACODE", "SYM", 30, 45);
            DesignSystemButtion(PanelFL, "FLUORESCENCE", "PARANAME", "FL", 30, 45);
            DesignSystemButtion(PanelLab, "LAB", "PARANAME", "LAB", 30, 45);
            DesignSystemButtion(PanelLocation, "LOCATION", "PARANAME", "LOCATION", 30, 45);

            DesignSystemDiamondTypeButtion(DTabDiamondType,PanelDiamondType, "TYPE", "TYPE", 26, 45);



            DataTable DTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PARAALL);
            DataRow[] DR = DTab.Select("PARATYPE='WEBSTATUS'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "PARANAME";
                //DTTemp.Rows.Add(999, "WEBSTATUS", "DEPTTRAN", "DEPTTRANSFER", "DEPTTRANSFER", "", 9999);
                //DTTemp.Rows.Add(999, "WEBSTATUS", "SINGLETOPARCEL", "SINGLETOPARCEL", "SINGLETOPARCEL", "", 9999);
                ListStatus.DTab = DTTemp.DefaultView.ToTable();
                ListStatus.DisplayMember = "SHORTNAME";
                ListStatus.ValueMember = "SHORTNAME";
                //ListStatus.SetSelectedCheckBox("AVAILABLE");
            }
            DR = DTab.Select("PARATYPE='LOCATION'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                ListGetLocation.DTab = DTTemp.DefaultView.ToTable();
                ListGetLocation.DisplayMember = "SHORTNAME";
                ListGetLocation.ValueMember = "PARA_ID";
            }
            DR = DTab.Select("PARATYPE='MILKY'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                CmbMilky.Properties.DataSource = DTTemp;
                CmbMilky.Properties.DisplayMember = "SHORTNAME";
                CmbMilky.Properties.ValueMember = "PARA_ID";
            }
            DR = DTab.Select("PARATYPE='COLORSHADE'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                CmbCShade.Properties.DataSource = DTTemp;
                CmbCShade.Properties.DisplayMember = "SHORTNAME";
                CmbCShade.Properties.ValueMember = "PARA_ID";
            }
            DR = DTab.Select("PARATYPE='TABLEBLACKINC'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                CmbTable.Properties.DataSource = DTTemp;
                CmbTable.Properties.DisplayMember = "SHORTNAME";
                CmbTable.Properties.ValueMember = "PARA_ID";
            }
            DR = DTab.Select("PARATYPE='SIDEBLACKINC'");
            if (DR.Length != 0)
            {
                DataTable DTTemp = DR.CopyToDataTable();
                DTTemp.DefaultView.Sort = "SEQUENCENO";
                CmbSide.Properties.DataSource = DTTemp;
                CmbSide.Properties.DisplayMember = "SHORTNAME";
                CmbSide.Properties.ValueMember = "PARA_ID";
            }          
        }

        #endregion

        #region GridEvent
        private void GrdDetail_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            //if (e.DisplayText == "0.00" || e.DisplayText == "0" || e.DisplayText == "0.000")
            //{
            //    e.DisplayText = String.Empty;
            //}
        }

        private void GrdDetail_CustomDrawColumnHeader(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            Global.CustomDrawColumnHeader(sender, e);
        }

        private void GrdDetail_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                if (GrdDetail.FocusedRowHandle < 0)
                    return;
                if (e.Clicks == 2 && Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("PARTYSTOCKNO")) 
                {
                    FrmStoneHistory FrmStoneHistory = new FrmStoneHistory();
                    FrmStoneHistory.MdiParent = Global.gMainRef;
                    FrmStoneHistory.ShowForm(Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "STOCK_ID")), Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "PARTYSTOCKNO")), Stock.FrmStoneHistory.FORMTYPE.DISPLAY);
                }
                if (e.Clicks == 2 && Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("ISCERTI"))
                {
                    string CertificateUrl = Val.ToString(GrdDetail.GetFocusedRowCellValue("CERTIFICATEURL"));
                    System.Diagnostics.Process.Start(CertificateUrl, "cmd");
                }
                if (e.Clicks == 2 && Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("ISVIDEO"))
                {
                    string VideoUrl = Val.ToString(GrdDetail.GetFocusedRowCellValue("VIDEOURL"));
                    System.Diagnostics.Process.Start(VideoUrl, "cmd");
                }
                if (e.Clicks == 2 && Val.ToString(e.Column.FieldName).Trim().ToUpper().Equals("ISIMAGE"))
                {
                    string ImageUrl = Val.ToString(GrdDetail.GetFocusedRowCellValue("IMAGEURL"));
                    System.Diagnostics.Process.Start(ImageUrl, "cmd");
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }

        }

        #endregion

        #region BackGroundWorker

        public string GetSelectedBtnID(Panel StrPanel)
        {
            string StrSelectedID = "";
            for (int i = 0; i < StrPanel.Controls.Count; i++)
            {
                if (StrPanel.Controls[i].BackColor == mSelectedBackColor)
                {
                    StrSelectedID += StrPanel.Controls[i].Tag + ",";
                }
            }
            return StrSelectedID;
        }
        public void RemoveSelectedBtn(Panel StrPanel)
        {
            string StrSelectedID = "";
            for (int i = 0; i < StrPanel.Controls.Count; i++)
            {
                if (StrPanel.Controls[i].BackColor == mSelectedBackColor)
                {                    
                    StrPanel.Controls[i].ForeColor = mDeSelectColor;
                    StrPanel.Controls[i].BackColor = mDSelectedBackColor;
                    StrPanel.Controls[i].AccessibleName = "true";
                }
            }            
        }
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                mProperty = new LiveStockProperty();
                DataSet DsLiveStock = new DataSet();

                #region Comment Code
                //if (string.IsNullOrEmpty(pStrStockNo))//Gunjan:04/07/2024
                //{
                //    mProperty.WEBSTATUS = ListStatus.GetSelectedReportTagValues();
                //    if (mProperty.WEBSTATUS == "")
                //    {
                //        if (Val.ToString() != string.Empty)
                //        {
                //            mProperty.WEBSTATUS = "";
                //        }
                //        else
                //        {
                //            mProperty.WEBSTATUS = "PURCHASE,PURCHASE-RETURN,AVAILABLE,HOLD,DELIVERY,CONSIGNMENT,OUTSIDE,MEMO,SOLD";
                //        }
                //        mProperty.MULTYLOCATION_ID = ListGetLocation.GetSelectedReportTagValues();
                //        mProperty.MULTYSHAPE_ID = GetSelectedBtnID(PanelShape);
                //        mProperty.MULTYCOLOR_ID = GetSelectedBtnID(PanelColor);
                //        mProperty.MULTYCLARITY_ID = GetSelectedBtnID(PanelClarity);
                //        mProperty.MULTYLAB_ID = GetSelectedBtnID(PanelLab);
                //        mProperty.MULTYCUT_ID = GetSelectedBtnID(PanelCut);
                //        mProperty.MULTYSYM_ID = GetSelectedBtnID(PanelSym);
                //        mProperty.MULTYPOL_ID = GetSelectedBtnID(PanelPol);
                //        mProperty.MULTYFL_ID = GetSelectedBtnID(PanelFL);
                //        mProperty.MULTYSIZE_ID = GetSelectedBtnID(PanelSizeNew);//Gunjan:220/07/2024


                //        mProperty.FROMCARAT1 = Val.Val(DTabSize.Rows[0]["FROMCARAT"]);
                //        mProperty.TOCARAT1 = Val.Val(DTabSize.Rows[0]["TOCARAT"]);

                //        mProperty.FROMCARAT2 = Val.Val(DTabSize.Rows[1]["FROMCARAT"]);
                //        mProperty.TOCARAT2 = Val.Val(DTabSize.Rows[1]["TOCARAT"]);

                //        mProperty.FROMCARAT3 = Val.Val(DTabSize.Rows[2]["FROMCARAT"]);
                //        mProperty.TOCARAT3 = Val.Val(DTabSize.Rows[2]["TOCARAT"]);

                //        mProperty.FROMCARAT4 = Val.Val(DTabSize.Rows[3]["FROMCARAT"]);
                //        mProperty.TOCARAT4 = Val.Val(DTabSize.Rows[3]["TOCARAT"]);

                //        mProperty.FROMCARAT5 = Val.Val(DTabSize.Rows[4]["FROMCARAT"]);
                //        mProperty.TOCARAT5 = Val.Val(DTabSize.Rows[4]["TOCARAT"]);

                //        mProperty.FROMTABLEPER = Val.Val(txtFromTablePer.Text);
                //        mProperty.TOTABLEPER = Val.Val(txtToTablePer.Text);

                //        mProperty.FROMDEPTHPER = Val.Val(txtFromTabledepth.Text);
                //        mProperty.TODEPTHPER = Val.Val(txtToTableDepthPer.Text);

                //        if (RbtStoneNo.Checked == true)
                //        {
                //            mProperty.STOCKNO = Val.ToString(TxtStoneCertiMfgMemo.Text);
                //            mProperty.LABREPORTNO = string.Empty;
                //            mProperty.SERIALNO = string.Empty;
                //            mProperty.MEMONO = string.Empty;
                //            mProperty.MFG_ID = string.Empty;
                //        }
                //        else if (RbtCertiNo.Checked == true)
                //        {
                //            mProperty.STOCKNO = string.Empty;
                //            mProperty.LABREPORTNO = Val.ToString(TxtStoneCertiMfgMemo.Text);
                //            mProperty.SERIALNO = string.Empty;
                //            mProperty.MEMONO = string.Empty;
                //            mProperty.MFG_ID = string.Empty;
                //        }
                //        else if (RbtSerialNo.Checked == true)
                //        {
                //            mProperty.STOCKNO = string.Empty;
                //            mProperty.LABREPORTNO = string.Empty;
                //            mProperty.SERIALNO = Val.ToString(TxtStoneCertiMfgMemo.Text);
                //            mProperty.MEMONO = string.Empty;
                //            mProperty.MFG_ID = string.Empty;
                //        }

                //        mProperty.MULTYMILKY_ID = Val.ToString(CmbMilky.Properties.GetCheckedItems());
                //        mProperty.MULTYCOLORSHADE_ID = Val.ToString(CmbCShade.Properties.GetCheckedItems());

                //        mProperty.MULTYTABLEBLACK_ID = Val.ToString(CmbTable.Properties.GetCheckedItems());
                //        mProperty.MULTYSIDEBLACK_ID = Val.ToString(CmbSide.Properties.GetCheckedItems());

                //        //mProperty.FROMCARAT1 = Val.Val(txtFromCts.Text);
                //        //mProperty.TOCARAT1 = Val.Val(txtToCts.Text);
                //        mProperty.DIAMONDTYPE = pStrDiamondType;
                //    }
                //}
                //else
                //{
                //    mProperty.STOCKNO = pStrStockNo;

                //}//End As Gunjan:04/07/2024
                #endregion               
                   
                 if (Val.ToString(ListStatus.GetSelectedReportTagValues()) != string.Empty)
                 {
                     mProperty.WEBSTATUS = ListStatus.GetSelectedReportTagValues(); ;
                 }
                 else
                 {
                     mProperty.WEBSTATUS = "PURCHASE,PURCHASE-RETURN,AVAILABLE,HOLD,DELIVERY,CONSIGNMENT,OUTSIDE,MEMO,SOLD";
                 }
                 mProperty.MULTYLOCATION_ID = ListGetLocation.GetSelectedReportTagValues();
                 mProperty.MULTYSHAPE_ID = GetSelectedBtnID(PanelShape);
                 mProperty.MULTYCOLOR_ID = GetSelectedBtnID(PanelColor);
                 mProperty.MULTYCLARITY_ID = GetSelectedBtnID(PanelClarity);
                 mProperty.MULTYLAB_ID = GetSelectedBtnID(PanelLab);
                 mProperty.MULTYCUT_ID = GetSelectedBtnID(PanelCut);
                 mProperty.MULTYSYM_ID = GetSelectedBtnID(PanelSym);
                 mProperty.MULTYPOL_ID = GetSelectedBtnID(PanelPol);
                 mProperty.MULTYFL_ID = GetSelectedBtnID(PanelFL);
                 mProperty.MULTYSIZE_ID = GetSelectedBtnID(PanelSizeNew);//Gunjan:220/07/2024
                
                 mProperty.FROMTABLEPER = Val.Val(txtFromTablePer.Text);
                 mProperty.TOTABLEPER = Val.Val(txtToTablePer.Text);

                 mProperty.FROMDEPTHPER = Val.Val(txtFromTabledepth.Text);
                 mProperty.TODEPTHPER = Val.Val(txtToTableDepthPer.Text);

                 if (RbtStoneNo.Checked == true)
                 {
                     mProperty.STOCKNO = Val.ToString(TxtStoneCertiMfgMemo.Text);
                     mProperty.LABREPORTNO = string.Empty;
                     mProperty.SERIALNO = string.Empty;
                     mProperty.MEMONO = string.Empty;
                     mProperty.MFG_ID = string.Empty;
                 }
                 else if (RbtCertiNo.Checked == true)
                 {
                     mProperty.STOCKNO = string.Empty;
                     mProperty.LABREPORTNO = Val.ToString(TxtStoneCertiMfgMemo.Text);
                     mProperty.SERIALNO = string.Empty;
                     mProperty.MEMONO = string.Empty;
                     mProperty.MFG_ID = string.Empty;
                 }
                 else if (RbtSerialNo.Checked == true)
                 {
                     mProperty.STOCKNO = string.Empty;
                     mProperty.LABREPORTNO = string.Empty;
                     mProperty.SERIALNO = Val.ToString(TxtStoneCertiMfgMemo.Text);
                     mProperty.MEMONO = string.Empty;
                     mProperty.MFG_ID = string.Empty;
                 }

                 mProperty.MULTYMILKY_ID = Val.ToString(CmbMilky.Properties.GetCheckedItems());
                 mProperty.MULTYCOLORSHADE_ID = Val.ToString(CmbCShade.Properties.GetCheckedItems());

                 mProperty.MULTYTABLEBLACK_ID = Val.ToString(CmbTable.Properties.GetCheckedItems());
                 mProperty.MULTYSIDEBLACK_ID = Val.ToString(CmbSide.Properties.GetCheckedItems());

                //mProperty.FROMCARAT1 = Val.Val(txtFromCts.Text);
                //mProperty.TOCARAT1 = Val.Val(txtToCts.Text);
                //mProperty.DIAMONDTYPE = pStrDiamondType;
                mProperty.DIAMONDTYPE = GetSelectedBtnID(PanelDiamondType);

                //mProperty.FROMCARAT = Val.Val(txtFromCts1.Text);
                //mProperty.TOCARAT = Val.Val(txtToCts1.Text);

                //mProperty.FROMCARAT1 = Val.Val(DTabSize.Rows[0]["FROMCARAT"]);
                //mProperty.TOCARAT1 = Val.Val(DTabSize.Rows[0]["TOCARAT"]);                

                //mProperty.FROMCARAT2 = Val.Val(DTabSize.Rows[1]["FROMCARAT"]);
                //mProperty.TOCARAT2 = Val.Val(DTabSize.Rows[1]["TOCARAT"]);

                //mProperty.FROMCARAT3 = Val.Val(DTabSize.Rows[2]["FROMCARAT"]);
                //mProperty.TOCARAT3 = Val.Val(DTabSize.Rows[2]["TOCARAT"]);

                //mProperty.FROMCARAT4 = Val.Val(DTabSize.Rows[3]["FROMCARAT"]);
                //mProperty.TOCARAT4 = Val.Val(DTabSize.Rows[3]["TOCARAT"]);

                //mProperty.FROMCARAT5 = Val.Val(DTabSize.Rows[4]["FROMCARAT"]);
                //mProperty.TOCARAT5 = Val.Val(DTabSize.Rows[4]["TOCARAT"]);

                mProperty.FROMCARAT1 = Val.Val(txtFromCts1.Text);
                mProperty.TOCARAT1 = Val.Val(txtToCts1.Text);

                mProperty.FROMCARAT2 = Val.Val(txtFromCts2.Text);
                mProperty.TOCARAT2 = Val.Val(txtToCts2.Text);

                mProperty.FROMCARAT3 = Val.Val(txtFromCts3.Text);
                mProperty.TOCARAT3 = Val.Val(txtToCts3.Text);

                mProperty.FROMCARAT4 = Val.Val(txtFromCts4.Text);
                mProperty.TOCARAT4 = Val.Val(txtToCts4.Text);

                mProperty.FROMCARAT5 = Val.Val(txtFromCts5.Text);
                mProperty.TOCARAT5 = Val.Val(txtToCts5.Text);


                if (mProperty.DIAMONDTYPE.EndsWith(","))
                {
                    mProperty.DIAMONDTYPE = mProperty.DIAMONDTYPE.Substring(0, mProperty.DIAMONDTYPE.Length - 1);
                }
                DsLiveStock = ObjStock.GetLiveStockDataNew(mProperty, "All");

                DtabLiveStockDetail = DsLiveStock.Tables[0];

                DtabLiveStockDetail.DefaultView.Sort = "SrNo";
                DtabLiveStockDetail = DtabLiveStockDetail.DefaultView.ToTable();
                
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
    

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                MainGrdDetail.DataSource = DtabLiveStockDetail;
                progressPanel1.Visible = false;
                //MainGrdDetail.Refresh();               
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        #endregion

        #region CotrolEvent
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSendEmail_Click(object sender, EventArgs e)
        {
            string StrFileName = ExportExcelNew();

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
            FrmEmailSend.ShowForm(StrFileName, StrEmail);
            ObjFormEvent.ObjToDisposeList.Add(FrmEmailSend);
        }

        private void BtnSelectedPrint_Click(object sender, EventArgs e)
        {
            try
            {               
                this.Cursor = Cursors.WaitCursor;

                //Int32[] selectedRowHandles = GrdDetail.GetSelectedRows();
                //DataTable DTab = DtabLiveStockDetail.Clone();
                //for (int i = 0; i < selectedRowHandles.Length; i++)
                //{
                //    int J = selectedRowHandles[i];
                //    DataRowView DR =(DataRowView) GrdDetail.GetRow(J);
                //    DTab.Rows.Add(DR.Row.ItemArray);

                //}

                DataTable DTab = GetSelectedRowToTable();
                if (DTab.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("There Is No Data Found For Print");
                    return;
                }
                DataSet DS = new DataSet();
                DTab.TableName = "Table";
                DS.Tables.Add(DTab);

                Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                FrmReportViewer.MdiParent = Global.gMainRef;
                FrmReportViewer.ShowFormInvoicePrint("LiveStockReport", DTab);
                this.Cursor = Cursors.Default;
            }

            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }
        private void BtnExport_Click(object sender, EventArgs e)
        {
            string StrFileName = ExportExcelNew();
            if (StrFileName == "")
            {
                Global.Message("Please Select Atleast One Packet");
                return;
            }
            if (Global.Confirm("Do You Want To Open File ? ") == System.Windows.Forms.DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(StrFileName, "CMD");
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                backgroundWorker1.CancelAsync();
            }
            //txtStoneNo.Text = string.Empty;//Comment by Gunjan:14/03/2024
            pStrStockNo = Val.ToString(txtStoneNo.Text);
            pStrDiamondType = Val.ToString(CmbDiamondType.SelectedItem);
            progressPanel1.Visible = true;
            backgroundWorker1.RunWorkerAsync();
        }

        #endregion

        #region ExcelExport

        public void AddProportionDetail(ExcelWorksheet worksheet, DataTable pDtabGroup, string SheetName, int Row, int Column,
          string pStrHeader, string pStrTitle,
          string pStrGroupColumn,
          string StrStartRow,
          string StrEndRow,
          DataTable pDtabDetail
          )
        {
            Color BackColor = Color.FromArgb(2, 68, 143);
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

        private string ExportExcelWithSale(DataSet DS, string PStrFilePath)
        {
            try
            {
                DataTable DTabDetail = DS.Tables[0];
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

                //DTabDetail.DefaultView.Sort = "SR";
                //DTabDetail = DTabDetail.DefaultView.ToTable();

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

                string StrFilePath = PStrFilePath;
                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.FromArgb(2, 68, 143);
                //Color BackColor = Color.FromArgb(119, 50, 107);
                Color FontColor = Color.White;
                string FontName = "Calibri";
                float FontSize = 9;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheetShortStock = xlPackage.Workbook.Worksheets.Add("For Short Sale List");
                    ExcelWorksheet worksheetProportion = xlPackage.Workbook.Worksheets.Add("Proportion");
                    ExcelWorksheet worksheetInclusion = xlPackage.Workbook.Worksheets.Add("Inclusion Detail");


                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    #region Short Stock Detail

                    EndRow = StartRow + DTabDetail.Rows.Count;

                    worksheetShortStock.Cells[StartRow, StartColumn, EndRow, EndColumn].LoadFromDataTable(DTabDetail, true);
                    worksheetShortStock.Cells[StartRow, 1, StartRow, EndColumn].Style.WrapText = true;
                    worksheetShortStock.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheetShortStock.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheetShortStock.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheetShortStock.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheetShortStock.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheetShortStock.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheetShortStock.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Name = FontName;
                    worksheetShortStock.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Size = 10;

                    worksheetShortStock.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Font.Bold = true;
                    worksheetShortStock.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetShortStock.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetShortStock.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetShortStock.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;


                    worksheetShortStock.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetShortStock.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheetShortStock.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                    worksheetShortStock.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(Color.Black);


                    worksheetShortStock.Cells[1, 13, 1, 15].Style.Font.Bold = true;
                    worksheetShortStock.Cells[1, 13, 1, 15].Style.Font.Color.SetColor(Color.Blue);

                    //worksheetShortStock.Cells[1, 16, 1, 18].Style.Font.Color.SetColor(Color.Blue);

                    worksheetShortStock.Cells[1, 20, 1, 23].Style.Font.Bold = true;
                    worksheetShortStock.Cells[1, 20, 1, 23].Style.Font.Color.SetColor(Color.Red);

                    int ShortPcsColumn = DTabDetail.Columns["Pcs"].Ordinal + 1;//Gunjan:19/01/2024
                    int RxWColumn = DTabDetail.Columns["RxW"].Ordinal + 1;//Gunjan:19/01/2024

                    int ShortCaratColumn = DTabDetail.Columns["Carat"].Ordinal + 1;
                    int ShortMemoRapColumn = DTabDetail.Columns["Memo Rap($)"].Ordinal + 1;
                    int ShortMemoDiscColumn = DTabDetail.Columns["Memo Disc(%)($)"].Ordinal + 1;
                    int ShortMemoPricePerCaratColumn = DTabDetail.Columns["Memo $/Cts($)"].Ordinal + 1;
                    int ShortMemoAmtColumn = DTabDetail.Columns["Memo Amt($)"].Ordinal + 1;
                    int ShortDiscountColumn = DTabDetail.Columns["Dis(%)"].Ordinal + 1;
                    int ShortTermsColumn = DTabDetail.Columns["Term(%)"].Ordinal + 1;
                    int ShortSaleRapColumn = DTabDetail.Columns["Sale Rap($)"].Ordinal + 1;
                    int ShortSaleDiscountColumn = DTabDetail.Columns["Sale Disc(%)($)"].Ordinal + 1;
                    int ShortSalePricePerCaratColumn = DTabDetail.Columns["Sale $/Cts($)"].Ordinal + 1;
                    int ShortSaleAmtColumn = DTabDetail.Columns["Sale Amt($)"].Ordinal + 1;

                    for (int IntI = 2; IntI <= EndRow; IntI++)
                    {
                        //string MemoDiscColumns = Global.ColumnIndexToColumnLetter(ShortMemoDiscColumn) + IntI.ToString();
                        //string SaleDiscount = Global.ColumnIndexToColumnLetter(ShortSaleDiscountColumn) + IntI.ToString();

                        //worksheetShortStock.Cells[IntI, ShortDiscountColumn].Formula = "=ROUND(" + MemoDiscColumns + " - " + SaleDiscount + ",2)";
                        string MemoDiscColumns = Global.ColumnIndexToColumnLetter(ShortMemoDiscColumn) + IntI.ToString();
                        string SaleDiscount = Global.ColumnIndexToColumnLetter(ShortSaleDiscountColumn) + IntI.ToString();

                        string RapColumns = Global.ColumnIndexToColumnLetter(ShortMemoRapColumn) + IntI.ToString();//Gunjan:19/01/2024
                        string Carat = Global.ColumnIndexToColumnLetter(ShortCaratColumn) + IntI.ToString();//Gunjan:19/01/2024
                        string PricePerCarat = Global.ColumnIndexToColumnLetter(ShortMemoPricePerCaratColumn) + IntI.ToString();

                        worksheetShortStock.Cells[IntI, RxWColumn].Formula = "=ROUND(" + RapColumns + " * " + Carat + ",2)";
                        worksheetShortStock.Cells[IntI, ShortMemoPricePerCaratColumn].Formula = "=ROUND(" + RapColumns + " + (" + " ( " + RapColumns + " * " + MemoDiscColumns + " )/100),2)";
                        worksheetShortStock.Cells[IntI, ShortMemoAmtColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";

                        worksheetShortStock.Cells[IntI, 13, IntI, 15].Style.Font.Bold = true;
                        worksheetShortStock.Cells[IntI, 13, IntI, 15].Style.Font.Color.SetColor(Color.Blue);

                        //worksheetShortStock.Cells[IntI, 16, IntI, 18].Style.Font.Color.SetColor(Color.Blue);

                        worksheetShortStock.Cells[1, 20, 1, 23].Style.Font.Bold = true;
                        worksheetShortStock.Cells[1, 20, 1, 23].Style.Font.Color.SetColor(Color.Red);
                    }
                    EndRow = EndRow + 2;
                    worksheetShortStock.Cells[EndRow, 1, EndRow, 1].Value = "Summary";

                    string ShortPcsCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Pcs"].Ordinal + 1);//Gunjan:19/01/2024
                    string RxW = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["RxW"].Ordinal + 1);//Gunjan:19/01/2024

                    string ShortCaratCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Carat"].Ordinal + 1);
                    string ShortMemoRap = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Memo Rap($)"].Ordinal + 1);
                    string ShortMemoDiscount = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Memo Disc(%)($)"].Ordinal + 1);
                    string ShortMemoPricePerCarat = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Memo $/Cts($)"].Ordinal + 1);
                    string ShortMemoAmt = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Memo Amt($)"].Ordinal + 1);
                    string ShortDisc = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Dis(%)"].Ordinal + 1);
                    string ShortTerms = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Term(%)"].Ordinal + 1);
                    string ShortSaleRap = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Sale Rap($)"].Ordinal + 1);
                    string ShortSaleDisc = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Sale Disc(%)($)"].Ordinal + 1);
                    string ShortSalePricePerCarat = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Sale $/Cts($)"].Ordinal + 1);
                    string ShortSaleAmt = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Sale Amt($)"].Ordinal + 1);

                    int IntShortTotRow = DTabDetail.Rows.Count + 1;

                    StartRow = StartRow + 1;
                    worksheetShortStock.Cells[EndRow, ShortPcsColumn, EndRow, ShortPcsColumn].Formula = "ROUND(SUBTOTAL(9," + ShortPcsCol + StartRow + ":" + ShortPcsCol + IntShortTotRow + "),2)";//Gunjan:19/01/2024
                    worksheetShortStock.Cells[EndRow, ShortCaratColumn, EndRow, ShortCaratColumn].Formula = "ROUND(SUBTOTAL(9," + ShortCaratCol + StartRow + ":" + ShortCaratCol + IntShortTotRow + "),2)";

                    worksheetShortStock.Cells[EndRow, RxWColumn, EndRow, RxWColumn].Formula = "SUBTOTAL(9," + RxW + StartRow + ":" + RxW + IntShortTotRow + ")";
                    worksheetShortStock.Cells[EndRow, ShortMemoPricePerCaratColumn, EndRow, ShortMemoPricePerCaratColumn].Formula = "ROUND(" + ShortMemoAmt + EndRow + "/" + ShortCaratCol + EndRow + ",0)";

                    worksheetShortStock.Cells[EndRow, ShortMemoDiscColumn, EndRow, ShortMemoDiscColumn].Formula = "ROUND((" + ShortMemoAmt + EndRow + "/" + RxW + EndRow + "-1 ) * 100,2)";
                    worksheetShortStock.Cells[EndRow, ShortMemoRapColumn, EndRow, ShortMemoRapColumn].Formula = "ROUND(" + RxW + EndRow + "/" + ShortCaratCol + EndRow + ",2)";
                    worksheetShortStock.Cells[EndRow, ShortMemoAmtColumn, EndRow, ShortMemoAmtColumn].Formula = "ROUND(SUBTOTAL(9," + ShortMemoAmt + StartRow + ":" + ShortMemoAmt + IntShortTotRow + "),2)";

                    //worksheetShortStock.Cells[EndRow, ShortCaratColumn, EndRow, ShortCaratColumn].Formula = "ROUND(SUBTOTAL(9," + ShortCaratCol + StartRow + ":" + ShortCaratCol + IntShortTotRow + "),2)";
                    //worksheetShortStock.Cells[EndRow, ShortMemoRapColumn, EndRow, ShortMemoRapColumn].Formula = "ROUND(SUBTOTAL(9," + ShortMemoRap + StartRow + ":" + ShortMemoRap + IntShortTotRow + "),2)";
                    //worksheetShortStock.Cells[EndRow, ShortMemoDiscColumn, EndRow, ShortMemoDiscColumn].Formula = "SUBTOTAL(9," + ShortMemoDiscount + StartRow + ":" + ShortMemoDiscount + IntShortTotRow + ")";
                    //worksheetShortStock.Cells[EndRow, ShortMemoPricePerCaratColumn, EndRow, ShortMemoPricePerCaratColumn].Formula = "ROUND(" + ShortMemoAmt + EndRow + "/" + ShortCaratCol + IntShortTotRow + ",0)";
                    //worksheetShortStock.Cells[EndRow, ShortMemoAmtColumn, EndRow, ShortMemoAmtColumn].Formula = "ROUND(SUBTOTAL(9," + ShortMemoAmt + StartRow + ":" + ShortMemoAmt + IntShortTotRow + "),2)";
                    worksheetShortStock.Cells[EndRow, ShortDiscountColumn, EndRow, ShortDiscountColumn].Formula = "ROUND((" + ShortMemoDiscount + EndRow + "-" + ShortSaleDisc + EndRow + " ),2)";
                    worksheetShortStock.Cells[EndRow, ShortTermsColumn, EndRow, ShortTermsColumn].Formula = "SUBTOTAL(9," + ShortTerms + StartRow + ":" + ShortTerms + IntShortTotRow + ")";

                    //worksheetShortStock.Cells[EndRow, ShortSaleRapColumn, EndRow, ShortSaleRapColumn].Formula = "ROUND(SUBTOTAL(9," + ShortSaleRap + StartRow + ":" + ShortSaleRap + IntShortTotRow + "),2)";
                    //worksheetShortStock.Cells[EndRow, ShortSalePricePerCaratColumn, EndRow, ShortSalePricePerCaratColumn].Formula = "ROUND(" + ShortSaleAmt + EndRow + "/" + ShortCaratCol + IntShortTotRow + ",0)";
                    //worksheetShortStock.Cells[EndRow, ShortSaleAmtColumn, EndRow, ShortSaleAmtColumn].Formula = "ROUND(SUBTOTAL(9," + ShortSaleAmt + StartRow + ":" + ShortSaleAmt + IntShortTotRow + "),2)";
                    //worksheetShortStock.Cells[EndRow, ShortSaleDiscountColumn, EndRow, ShortSaleDiscountColumn].Formula = "SUBTOTAL(9," + ShortSaleDisc + StartRow + ":" + ShortSaleDisc + IntShortTotRow + ")";
                    worksheetShortStock.Cells[EndRow, ShortSaleRapColumn, EndRow, ShortSaleRapColumn].Formula = "ROUND(" + RxW + EndRow + "/" + ShortCaratCol + EndRow + ",2)";
                    worksheetShortStock.Cells[EndRow, ShortSalePricePerCaratColumn, EndRow, ShortSalePricePerCaratColumn].Formula = "ROUND(" + ShortMemoAmt + EndRow + "/" + ShortCaratCol + EndRow + ",0)";
                    worksheetShortStock.Cells[EndRow, ShortSaleAmtColumn, EndRow, ShortSaleAmtColumn].Formula = "ROUND(SUBTOTAL(9," + ShortSaleAmt + StartRow + ":" + ShortSaleAmt + IntShortTotRow + "),2)";
                    worksheetShortStock.Cells[EndRow, ShortSaleDiscountColumn, EndRow, ShortSaleDiscountColumn].Formula = "ROUND((" + ShortSaleAmt + EndRow + "/" + RxW + EndRow + "-1 ) * 100,2)";

                    worksheetShortStock.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Font.Bold = true;

                    worksheetShortStock.Cells[StartRow, ShortPcsColumn, EndRow, ShortPcsColumn].Style.Numberformat.Format = "0";//Gunjan:19/01/2024
                    worksheetShortStock.Cells[StartRow, RxWColumn, EndRow, RxWColumn].Style.Numberformat.Format = "0.00";//Gunjan:19/01/2024
                    worksheetShortStock.Cells[StartRow, ShortCaratColumn, EndRow, ShortCaratColumn].Style.Numberformat.Format = "0.00";
                    worksheetShortStock.Cells[StartRow, ShortMemoRapColumn, EndRow, ShortMemoRapColumn].Style.Numberformat.Format = "0.00";
                    worksheetShortStock.Cells[StartRow, ShortMemoDiscColumn, EndRow, ShortMemoDiscColumn].Style.Numberformat.Format = "0.00";
                    worksheetShortStock.Cells[StartRow, ShortMemoPricePerCaratColumn, EndRow, ShortMemoPricePerCaratColumn].Style.Numberformat.Format = "0.00";
                    worksheetShortStock.Cells[StartRow, ShortMemoAmtColumn, EndRow, ShortMemoAmtColumn].Style.Numberformat.Format = "0.00";
                    worksheetShortStock.Cells[StartRow, ShortDiscountColumn, EndRow, ShortDiscountColumn].Style.Numberformat.Format = "0.00";
                    worksheetShortStock.Cells[StartRow, ShortTermsColumn, EndRow, ShortTermsColumn].Style.Numberformat.Format = "0.00";
                    worksheetShortStock.Cells[StartRow, ShortSaleRapColumn, EndRow, ShortSaleRapColumn].Style.Numberformat.Format = "0.00";
                    worksheetShortStock.Cells[StartRow, ShortSalePricePerCaratColumn, EndRow, ShortSalePricePerCaratColumn].Style.Numberformat.Format = "0.00";
                    worksheetShortStock.Cells[StartRow, ShortSaleAmtColumn, EndRow, ShortSaleAmtColumn].Style.Numberformat.Format = "0.00";
                    worksheetShortStock.Cells[StartRow, ShortSaleDiscountColumn, EndRow, ShortSaleDiscountColumn].Style.Numberformat.Format = "0.00";

                    //Gunjan:19/01/2024
                    worksheetShortStock.Cells[StartRow, RxWColumn, EndRow, RxWColumn].Style.Font.Color.SetColor(Color.Blue);
                    worksheetShortStock.Cells[StartRow, ShortMemoRapColumn, EndRow, ShortMemoRapColumn].Style.Font.Color.SetColor(Color.Blue);
                    worksheetShortStock.Cells[StartRow, ShortMemoDiscColumn, EndRow, ShortMemoDiscColumn].Style.Font.Color.SetColor(Color.Blue);
                    worksheetShortStock.Cells[StartRow, ShortMemoPricePerCaratColumn, EndRow, ShortMemoPricePerCaratColumn].Style.Font.Color.SetColor(Color.Blue);
                    worksheetShortStock.Cells[StartRow, ShortMemoAmtColumn, EndRow, ShortMemoAmtColumn].Style.Font.Color.SetColor(Color.Blue);

                    worksheetShortStock.Cells[StartRow, ShortSaleRapColumn, EndRow, ShortSaleRapColumn].Style.Font.Color.SetColor(Color.Red);
                    worksheetShortStock.Cells[StartRow, ShortSalePricePerCaratColumn, EndRow, ShortSalePricePerCaratColumn].Style.Font.Color.SetColor(Color.Red);
                    worksheetShortStock.Cells[StartRow, ShortSaleAmtColumn, EndRow, ShortSaleAmtColumn].Style.Font.Color.SetColor(Color.Red);
                    worksheetShortStock.Cells[StartRow, ShortSaleDiscountColumn, EndRow, ShortSaleDiscountColumn].Style.Font.Color.SetColor(Color.Red);

                    worksheetShortStock.Cells[StartRow, RxWColumn, EndRow, RxWColumn].Style.Font.Bold = true;
                    worksheetShortStock.Cells[StartRow, ShortMemoRapColumn, EndRow, ShortMemoRapColumn].Style.Font.Bold = true;
                    worksheetShortStock.Cells[StartRow, ShortMemoDiscColumn, EndRow, ShortMemoDiscColumn].Style.Font.Bold = true;
                    worksheetShortStock.Cells[StartRow, ShortMemoPricePerCaratColumn, EndRow, ShortMemoPricePerCaratColumn].Style.Font.Bold = true;
                    worksheetShortStock.Cells[StartRow, ShortMemoAmtColumn, EndRow, ShortMemoAmtColumn].Style.Font.Bold = true;

                    worksheetShortStock.Cells[StartRow, ShortSaleRapColumn, EndRow, ShortSaleRapColumn].Style.Font.Bold = true;
                    worksheetShortStock.Cells[StartRow, ShortSalePricePerCaratColumn, EndRow, ShortSalePricePerCaratColumn].Style.Font.Bold = true;
                    worksheetShortStock.Cells[StartRow, ShortSaleAmtColumn, EndRow, ShortSaleAmtColumn].Style.Font.Bold = true;
                    worksheetShortStock.Cells[StartRow, ShortSaleDiscountColumn, EndRow, ShortSaleDiscountColumn].Style.Font.Bold = true;
                    //End As Gunjan

                    worksheetShortStock.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheetShortStock.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheetShortStock.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheetShortStock.Cells[1, 1, 100, 100].AutoFitColumns();

                    worksheetShortStock.Column(DTabDetail.Columns["RapValue"].Ordinal + 1).Hidden = true;
                    worksheetShortStock.Column(DTabDetail.Columns["Size"].Ordinal + 1).Hidden = true;
                    worksheetShortStock.Column(DTabDetail.Columns["ClaGroup"].Ordinal + 1).Hidden = true;
                    worksheetShortStock.Column(DTabDetail.Columns["ColGroup"].Ordinal + 1).Hidden = true;
                    worksheetShortStock.Column(DTabDetail.Columns["CutGroup"].Ordinal + 1).Hidden = true;
                    worksheetShortStock.Column(DTabDetail.Columns["PolGroup"].Ordinal + 1).Hidden = true;
                    worksheetShortStock.Column(DTabDetail.Columns["SymGroup"].Ordinal + 1).Hidden = true;
                    worksheetShortStock.Column(DTabDetail.Columns["FLGroup"].Ordinal + 1).Hidden = true;
                    worksheetShortStock.Column(DTabDetail.Columns["RxW"].Ordinal + 1).Hidden = true;

                    string StrStartRow = "6";
                    string StrEndRow = EndRow.ToString();
                    #endregion

                    #region Inclusion Detail

                    AddInclusionDetail(worksheetInclusion, DTabInclusion);

                    #endregion

                    #region Proporstion Detail

                    worksheetProportion.Cells[2, 2, 3, 22].Value = "Stock Proportion";
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Font.Name = FontName;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Font.Size = 20;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Font.Bold = true;

                    worksheetProportion.Cells[2, 2, 3, 22].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 22].Merge = true;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheetProportion.Cells[2, 2, 3, 22].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Font.Color.SetColor(FontColor);

                    int NewRow = 6;
                    AddProportionDetail(worksheetProportion, DTabSize, worksheetProportion.Name, 6, 2, "SIZE WISE SUMMARY", "Size", "Size", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabShape, worksheetProportion.Name, 6, 13, "SHAPE WISE SUMMARY", "Shape", "Shape", StrStartRow, StrEndRow, DTabDetail);

                    if (DTabSize.Rows.Count > DTabShape.Rows.Count)
                    {
                        NewRow = NewRow + DTabSize.Rows.Count + 5;
                    }
                    else
                    {
                        NewRow = NewRow + DTabShape.Rows.Count + 5;
                    }

                    AddProportionDetail(worksheetProportion, DTabClarity, worksheetProportion.Name, NewRow, 2, "CLARITY WISE SUMMARY", "Clarity", "ClaGroup", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabColor, worksheetProportion.Name, NewRow, 13, "COLOR WISE SUMMARY", "Color", "ColGroup", StrStartRow, StrEndRow, DTabDetail);


                    if (DTabClarity.Rows.Count > DTabColor.Rows.Count)
                    {
                        NewRow = NewRow + DTabClarity.Rows.Count + 5;
                    }
                    else
                    {
                        NewRow = NewRow + DTabColor.Rows.Count + 5;
                    }

                    AddProportionDetail(worksheetProportion, DTabCut, worksheetProportion.Name, NewRow, 2, "CUT WISE SUMMARY", "Cut", "CutGroup", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabPolish, worksheetProportion.Name, NewRow, 13, "POLISH WISE SUMMARY", "Pol", "PolGroup", StrStartRow, StrEndRow, DTabDetail);


                    if (DTabCut.Rows.Count > DTabPolish.Rows.Count)
                    {
                        NewRow = NewRow + DTabCut.Rows.Count + 5;
                    }
                    else
                    {
                        NewRow = NewRow + DTabPolish.Rows.Count + 5;
                    }

                    AddProportionDetail(worksheetProportion, DTabSym, worksheetProportion.Name, NewRow, 2, "SYM WISE SUMMARY", "Sym", "SymGroup", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabFL, worksheetProportion.Name, NewRow, 13, "FL WISE SUMMARY", "FL", "FLGroup", StrStartRow, StrEndRow, DTabDetail);
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




        public string ExportExcelWithExp(DataSet DS, string PStrFilePath) //Add Khushbu 15-05-21
        {
            try
            {

                DataTable DTabDetail = DS.Tables[0];

                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }

                DTabDetail.DefaultView.Sort = "SrNo";
                DTabDetail = DTabDetail.DefaultView.ToTable();

                this.Cursor = Cursors.WaitCursor;

                string StrFilePath = PStrFilePath;

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.FromArgb(2, 68, 143);
                Color FontColor = Color.White;
                string FontName = "Calibri";
                float FontSize = 9;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("RJ_Stock_" + DateTime.Now.ToString("ddMMyyyy"));

                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    #region Add Image

                    //Image img = Image.FromFile(Application.StartupPath + "//logo.jpg");
                    //OfficeOpenXml.Drawing.ExcelPicture pic = worksheet.Drawings.AddPicture("Logo", img);
                    //pic.SetPosition(2, 23);
                    //pic.SetSize(100, 55);

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
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(FontColor);


                    int RxWColumn = DTabDetail.Columns["RW"].Ordinal + 1;
                    int RapaportColumn = DTabDetail.Columns["RapPrice"].Ordinal + 1;
                    int PricePerCaratColumn = DTabDetail.Columns["NetRate"].Ordinal + 1;
                    int DiscountColumn = DTabDetail.Columns["Disc"].Ordinal + 1;
                    int CaratColumn = DTabDetail.Columns["Carats"].Ordinal + 1;
                    int AmountColumn = DTabDetail.Columns["NetValue"].Ordinal + 1;

                    for (int IntI = 6; IntI <= EndRow; IntI++)
                    {
                        string RapColumns = Global.ColumnIndexToColumnLetter(RapaportColumn) + IntI.ToString();
                        string Discount = Global.ColumnIndexToColumnLetter(DiscountColumn) + IntI.ToString();
                        string Carat = Global.ColumnIndexToColumnLetter(CaratColumn) + IntI.ToString();
                        string PricePerCarat = Global.ColumnIndexToColumnLetter(PricePerCaratColumn) + IntI.ToString();

                        worksheet.Cells[IntI, RxWColumn].Formula = "=ROUND(" + RapColumns + " * " + Carat + ",2)";

                        worksheet.Cells[IntI, PricePerCaratColumn].Formula = "=ROUND( (100 +" + Discount + ") * " + RapColumns + "/100,2)";
                        worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";

                    }

                   
                    int IntRowStartsFrom = 3;
                    int IntRowEndTo = (DTabDetail.Rows.Count - 1 + IntRowStartsFrom);

                    int SrNo = 0, CaratNo = 0, AmountNo = 0, RapAmountNo = 0;

                    DataColumnCollection columns = DTabDetail.Columns;

                    if (columns.Contains("SrNo"))
                        SrNo = DTabDetail.Columns["SrNo"].Ordinal + 1;
                    if (columns.Contains("Carats"))
                        CaratNo = DTabDetail.Columns["Carats"].Ordinal + 1;
                    if (columns.Contains("RW"))
                        RapAmountNo = DTabDetail.Columns["RW"].Ordinal + 1;
                    if (columns.Contains("NetValue"))
                        AmountNo = DTabDetail.Columns["NetValue"].Ordinal + 1;

                    string StrStartRow = "6";
                    string StrEndRow = EndRow.ToString();

                    #region Top Formula


                    worksheet.Cells[1, 5, 1, 5].Value = "Pcs";
                    worksheet.Cells[1, 6, 1, 6].Value = "Carat";
                    worksheet.Cells[1, 17, 1, 17].Value = "Rap Value";
                    worksheet.Cells[1, 18, 1, 18].Value = "Rap %";
                    worksheet.Cells[1, 19, 1, 19].Value = "Pr/Ct";
                    worksheet.Cells[1, 20, 1, 20].Value = "Amount";

                    worksheet.Cells[5, 2, 5, 2].Value = "Stone No";
                    worksheet.Cells[5, 4, 5, 4].Value = "Report No";
                    worksheet.Cells[5, 12, 5, 12].Value = "Flour.";
                    worksheet.Cells[5, 14, 5, 14].Value = "Depth%";
                    worksheet.Cells[5, 15, 5, 15].Value = "Table%";
                    worksheet.Cells[5, 16, 5, 16].Value = "Rap. Price";
                    worksheet.Cells[5, 17, 5, 17].Value = "RxW";
                    worksheet.Cells[5, 18, 5, 18].Value = "Disc%";
                    worksheet.Cells[5, 19, 5, 19].Value = "Net Rate";
                    worksheet.Cells[5, 20, 5, 20].Value = "Net Value";

                    worksheet.Cells[2, 4, 2, 4].Value = "Total";
                    worksheet.Cells[3, 4, 3, 4].Value = "Selected";

                    worksheet.Cells[1, 7, 3, 16].Merge = true;
                    worksheet.Cells[1, 7, 3, 16].Value = "Note : Use filter to select stones and Check your ObjGridSelection Avg Disc and Total amt.";
                    worksheet.Cells[1, 7, 3, 16].Style.WrapText = true;

                    string S = Global.ColumnIndexToColumnLetter(SrNo) + StrStartRow;
                    string E = Global.ColumnIndexToColumnLetter(SrNo) + StrEndRow;
                    worksheet.Cells[2, 5, 2, 5].Formula = "ROUND(COUNTA(" + S + ":" + E + "),2)";
                    worksheet.Cells[3, 5, 3, 5].Formula = "ROUND(SUBTOTAL(3," + S + ":" + E + "),2)";


                    S = Global.ColumnIndexToColumnLetter(CaratNo) + StrStartRow;
                    E = Global.ColumnIndexToColumnLetter(CaratNo) + StrEndRow;
                    worksheet.Cells[2, 6, 2, 6].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                    worksheet.Cells[3, 6, 3, 6].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                    S = Global.ColumnIndexToColumnLetter(RapAmountNo) + StrStartRow;
                    E = Global.ColumnIndexToColumnLetter(RapAmountNo) + StrEndRow;
                    worksheet.Cells[2, 17, 2, 17].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                    worksheet.Cells[3, 17, 3, 17].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";


                    S = Global.ColumnIndexToColumnLetter(AmountNo) + StrStartRow;
                    E = Global.ColumnIndexToColumnLetter(AmountNo) + StrEndRow;
                    worksheet.Cells[2, 20, 2, 20].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                    worksheet.Cells[3, 20, 3, 20].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";


                    worksheet.Cells[2, 19, 2, 19].Formula = "ROUND(T2/F2,2)";
                    worksheet.Cells[3, 19, 3, 19].Formula = "ROUND(T3/F3,2)";


                    S = Global.ColumnIndexToColumnLetter(AmountNo) + StrStartRow;
                    E = Global.ColumnIndexToColumnLetter(AmountNo) + StrEndRow;

                    worksheet.Cells[2, 18, 2, 18].Formula = "ROUND(SUM(((T2)/((Q2*1)))*100),2)-100";
                    worksheet.Cells[3, 18, 3, 18].Formula = "ROUND(SUM(((T3)/((Q3*1)))*100),2)-100";


                    worksheet.Cells[1, 4, 4, 20].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[1, 4, 4, 20].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[1, 4, 4, 20].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, 4, 4, 20].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, 4, 4, 20].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, 4, 4, 20].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[1, 4, 4, 20].Style.Font.Name = "Calibri";
                    worksheet.Cells[1, 4, 4, 20].Style.Font.Size = 9;

                    worksheet.Cells[1, 4, 1, 20].Style.Font.Bold = true;
                    worksheet.Cells[1, 4, 1, 20].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, 4, 1, 20].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, 4, 1, 20].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[1, 4, 1, 20].Style.Fill.BackgroundColor.SetColor(BackColor);

                    worksheet.Cells[1, 4, 3, 4].Style.Font.Bold = true;
                    worksheet.Cells[1, 4, 3, 4].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, 4, 3, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, 4, 3, 4].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[1, 4, 3, 4].Style.Fill.BackgroundColor.SetColor(BackColor);

                    worksheet.Cells[1, 7, 3, 10].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, 7, 3, 10].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[1, 7, 3, 10].Style.Fill.BackgroundColor.SetColor(BackColor);

                    #endregion

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



        public string ExportExcelWithStockList(DataSet DS, string PStrFilePath) //Add Khushbu 12-07-21
        {
            try
            {

                DataTable DTabDetail = DS.Tables[0];

                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }

                DTabDetail.DefaultView.Sort = "SrNo";
                DTabDetail = DTabDetail.DefaultView.ToTable();

                this.Cursor = Cursors.WaitCursor;

                string StrFilePath = PStrFilePath;

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.Yellow;
                Color FontColor = Color.Black;
                string FontName = "Calibri";
                float FontSize = 11;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Full Stock List");
                   
                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    #region Stock Detail

                    EndRow = StartRow + DTabDetail.Rows.Count;

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
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(FontColor);

                    int RapaportColumn = DTabDetail.Columns["Rapaport"].Ordinal + 1;
                    int PricePerCaratColumn = DTabDetail.Columns["$/Cts"].Ordinal + 1;
                    int DiscountColumn = DTabDetail.Columns["Disc%"].Ordinal + 1;
                    int AmountColumn = DTabDetail.Columns["Total"].Ordinal + 1;
                    int CaratColumn = DTabDetail.Columns["Carats"].Ordinal + 1;
                    int ShapeColumn = DTabDetail.Columns["Shape"].Ordinal + 1;


                    int VideoLinkColumn = DTabDetail.Columns["Video"].Ordinal + 1;
                    int DepthPerColumn = DTabDetail.Columns["Depth%"].Ordinal + 1;
                    int TablePerColumn = DTabDetail.Columns["Table%"].Ordinal + 1;

                    int ImageLinkColumn = DTabDetail.Columns["Image"].Ordinal + 1;

                    int CertiLinkColumn = DTabDetail.Columns["Certi"].Ordinal + 1;

                    //int GirdlePerColumn = DTabDetail.Columns["Girldle Per"].Ordinal + 1;
                    //int CAColumn = DTabDetail.Columns["Crown Angle"].Ordinal + 1;



                    for (int IntI = 2; IntI <= EndRow; IntI++)
                    {
                        string RapColumns = Global.ColumnIndexToColumnLetter(RapaportColumn) + IntI.ToString();
                        string Discount = Global.ColumnIndexToColumnLetter(DiscountColumn) + IntI.ToString();
                        string Carat = Global.ColumnIndexToColumnLetter(CaratColumn) + IntI.ToString();
                        string PricePerCarat = Global.ColumnIndexToColumnLetter(PricePerCaratColumn) + IntI.ToString();
                        string VideoLink = Global.ColumnIndexToColumnLetter(VideoLinkColumn) + IntI.ToString();
                        string ImageLink = Global.ColumnIndexToColumnLetter(ImageLinkColumn) + IntI.ToString();
                        string CertiLink = Global.ColumnIndexToColumnLetter(CertiLinkColumn) + IntI.ToString();

                        //if (Val.ToString(DTabDetail.Rows[IntI - 2]["FANCYCOLOR"]) == "") //Add if condition khushbu 08-10-21 for skip formula in fancy color
                        //{
                        //    worksheet.Cells[IntI, PricePerCaratColumn].Formula = "=(" + RapColumns + " + ((" + RapColumns + " * " + Discount + ") / 100))";
                        //    worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                        //    worksheet.Cells[IntI, AmountColumn].Style.Font.Bold = true;
                        //}
                        //else
                        //{
                        //    worksheet.Cells[IntI, PricePerCaratColumn].Value = Val.ToDouble(DTabDetail.Rows[IntI - 2]["$/Cts"]);
                        //    worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                        //    worksheet.Cells[IntI, AmountColumn].Style.Font.Bold = true;

                        //}

                        if (IntI != 1)
                        {
                            string videoLink = worksheet.Cells[IntI, VideoLinkColumn].Value?.ToString()?.Trim();

                            if (Uri.TryCreate(videoLink, UriKind.Absolute, out Uri validUri) &&
                                (validUri.Scheme == Uri.UriSchemeHttp || validUri.Scheme == Uri.UriSchemeHttps))
                            {
                                worksheet.Cells[IntI, VideoLinkColumn].Value = "Video";
                                worksheet.Cells[IntI, VideoLinkColumn].Hyperlink = validUri;
                                worksheet.Cells[IntI, VideoLinkColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, VideoLinkColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, VideoLinkColumn].Style.Font.Color.SetColor(Color.Blue);
                            }
                            else
                            {
                                worksheet.Cells[IntI, VideoLinkColumn].Value = "Invalid Link";
                                worksheet.Cells[IntI, VideoLinkColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, VideoLinkColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, VideoLinkColumn].Style.Font.Color.SetColor(Color.Red);
                            }

                            // Image Column Hyperlink
                            if (string.IsNullOrEmpty(worksheet.Cells[IntI, ImageLinkColumn].Value?.ToString()))
                            {
                                worksheet.Cells[IntI, ImageLinkColumn].Value = "N/A";
                                worksheet.Cells[IntI, ImageLinkColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, ImageLinkColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, ImageLinkColumn].Style.Font.Color.SetColor(Color.Red);
                            }
                            else
                            {
                                string imageLink = worksheet.Cells[IntI, ImageLinkColumn].Value?.ToString()?.Trim();

                                if (Uri.TryCreate(imageLink, UriKind.Absolute, out Uri imageUri) &&
                                    (imageUri.Scheme == Uri.UriSchemeHttp || imageUri.Scheme == Uri.UriSchemeHttps))
                                {
                                    worksheet.Cells[IntI, ImageLinkColumn].Value = "Image";
                                    worksheet.Cells[IntI, ImageLinkColumn].Hyperlink = imageUri;
                                    worksheet.Cells[IntI, ImageLinkColumn].Style.Font.Name = FontName;
                                    worksheet.Cells[IntI, ImageLinkColumn].Style.Font.Bold = true;
                                    worksheet.Cells[IntI, ImageLinkColumn].Style.Font.Color.SetColor(Color.Blue);
                                }
                                else
                                {
                                    worksheet.Cells[IntI, ImageLinkColumn].Value = "Invalid Link";
                                    worksheet.Cells[IntI, ImageLinkColumn].Style.Font.Name = FontName;
                                    worksheet.Cells[IntI, ImageLinkColumn].Style.Font.Bold = true;
                                    worksheet.Cells[IntI, ImageLinkColumn].Style.Font.Color.SetColor(Color.Red);
                                }
                            }


                            //// Certificate Column Hyperlink
                            if (string.IsNullOrEmpty(worksheet.Cells[IntI, CertiLinkColumn].Value?.ToString()))
                            {
                                worksheet.Cells[IntI, CertiLinkColumn].Value = "N/A";
                                worksheet.Cells[IntI, CertiLinkColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, CertiLinkColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, CertiLinkColumn].Style.Font.Color.SetColor(Color.Red);
                            }
                            else
                            {
                                string certiLink = worksheet.Cells[IntI, CertiLinkColumn].Value?.ToString(); // Assuming CertiLinkColumn contains the actual URL
                                worksheet.Cells[IntI, CertiLinkColumn].Value = "Certificate";
                                worksheet.Cells[IntI, CertiLinkColumn].Hyperlink = new Uri(certiLink, UriKind.Absolute);
                                worksheet.Cells[IntI, CertiLinkColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, CertiLinkColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, CertiLinkColumn].Style.Font.Color.SetColor(Color.Blue);
                            }
                        }
                    }

                    EndRow = EndRow + 2;
                    worksheet.Cells[EndRow, 1, EndRow, 1].Value = "Summary";

                    string CaratCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Carats"].Ordinal + 1);
                    string Discount1 = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Disc%"].Ordinal + 1);
                    string NetRate = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["$/Cts"].Ordinal + 1);
                    string NetValue = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Total"].Ordinal + 1);
                    string RAPCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Rapaport"].Ordinal + 1);

                    int IntTotRow = DTabDetail.Rows.Count + 1;

                    StartRow = StartRow + 1;

                    worksheet.Cells[EndRow, ShapeColumn, EndRow, ShapeColumn].Formula = "SUBTOTAL(2," + CaratCol + StartRow + ":" + CaratCol + IntTotRow + ")";
                    worksheet.Cells[EndRow, CaratColumn, EndRow, CaratColumn].Formula = "ROUND(SUBTOTAL(9," + CaratCol + StartRow + ":" + CaratCol + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, AmountColumn, EndRow, AmountColumn].Formula = "ROUND(SUBTOTAL(9," + NetValue + StartRow + ":" + NetValue + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, PricePerCaratColumn, EndRow, PricePerCaratColumn].Formula = "ROUND(" + NetValue + EndRow + "/" + CaratCol + EndRow + ",2)";
                    worksheet.Cells[EndRow, DiscountColumn, EndRow, DiscountColumn].Formula = "= -100 + (" + NetValue + EndRow + "/SUMPRODUCT(" + RAPCol + StartRow + ":" + RAPCol + IntTotRow + ", " + CaratCol + StartRow + ":" + CaratCol + IntTotRow + ")) * 100";

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Font.Bold = true;

                    worksheet.Cells[StartRow, CaratColumn, EndRow, CaratColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, DiscountColumn, EndRow, DiscountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, AmountColumn, EndRow, AmountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, DepthPerColumn, EndRow, DepthPerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, TablePerColumn, EndRow, TablePerColumn].Style.Numberformat.Format = "0.00";
                    //worksheet.Cells[StartRow, GirdlePerColumn, EndRow, GirdlePerColumn].Style.Numberformat.Format = "0.00";
                    //worksheet.Cells[StartRow, CAColumn, EndRow, CAColumn + 3].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, PricePerCaratColumn, EndRow, PricePerCaratColumn ].Style.Numberformat.Format = "0.00";


                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    #endregion

                    worksheet.Cells[1, 1, 100, 100].AutoFitColumns();

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



        public string ExportExcelWithSmartIList(DataSet DS, string PStrFilePath) //Add Khushbu 12-07-21
        {
            try
            {

                DataTable DTabDetail = DS.Tables[0];

                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }

               
                this.Cursor = Cursors.WaitCursor;

                string StrFilePath = PStrFilePath;

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.Yellow;
                Color FontColor = Color.Black;
                string FontName = "Calibri";
                float FontSize = 11;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Full Stock List");
                   
                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    #region Smart-I

                    EndRow = StartRow + DTabDetail.Rows.Count;

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
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(FontColor);

                    worksheet.Cells[1, 1, 100, 100].AutoFitColumns();

                    xlPackage.Save();
                    #endregion
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

        public string ExportExcelWithOfferPriceFormat(DataSet DS, string PStrFilePath) //Add Krina
        {
            try
            {

                DataTable DTabDetail = DS.Tables[0];

                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }

                DTabDetail.DefaultView.Sort = "SrNo";
                DTabDetail = DTabDetail.DefaultView.ToTable();

                this.Cursor = Cursors.WaitCursor;

                string StrFilePath = PStrFilePath;

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.Yellow;
                Color FontColor = Color.Black;
                string FontName = "Calibri";
                float FontSize = 11;
                Color BackColor1 = Color.FromArgb(146, 205, 220);
                Color FontColor1 = Color.FromArgb(99, 37, 35);
                Color FontColor2 = Color.Red;


                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;
                int EndRow1 = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Full Stock List");
                    
                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    #region Stock Detail

                    EndRow = StartRow + DTabDetail.Rows.Count;
                    EndRow1 = StartRow + DTabDetail.Rows.Count;

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
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(FontColor);
                    worksheet.Cells[1, 33, EndRow1, 37].Style.Font.Color.SetColor(FontColor1);
                    worksheet.Cells[1, 38, EndRow1, 40].Style.Font.Color.SetColor(FontColor2);
                    worksheet.Cells[1, 6, 1, 17].Style.Fill.BackgroundColor.SetColor(BackColor1);
                  
                    int RxWColumn = DTabDetail.Columns["RxW"].Ordinal + 1;
                    int RapaportColumn = DTabDetail.Columns["Rap. Price"].Ordinal + 1;
                    int PricePerCaratColumn = DTabDetail.Columns["Net Rate"].Ordinal + 1;
                    int DiscountColumn = DTabDetail.Columns["Disc%"].Ordinal + 1;
                    int CaratColumn = DTabDetail.Columns["Carats"].Ordinal + 1;
                    int AmountColumn = DTabDetail.Columns["Net Value"].Ordinal + 1;
                    int VideoLinkColumn = DTabDetail.Columns["Video Link"].Ordinal + 1;
                    int VideoColumn = DTabDetail.Columns["Video"].Ordinal + 1;
                    int DepthPerColumn = DTabDetail.Columns["Depth%"].Ordinal + 1;
                    int TablePerColumn = DTabDetail.Columns["Table%"].Ordinal + 1;
                    int ShapeColumn = DTabDetail.Columns["Shape"].Ordinal + 1;

                    int GirdlePerColumn = DTabDetail.Columns["Girldle Per"].Ordinal + 1;
                    int CAColumn = DTabDetail.Columns["Crown Angle"].Ordinal + 1;
                    int OfferAmountColumn = DTabDetail.Columns["Offer Net Value"].Ordinal + 1;
                    int OfferPricePerCaratColumn = DTabDetail.Columns["Offer Net Rate"].Ordinal + 1;
                    int OfferDiscountColumn = DTabDetail.Columns["Offer Disc%"].Ordinal + 1;
                    int ExpAmountColumn = DTabDetail.Columns["Exp.Net Value"].Ordinal + 1;
                    int ExpPricePerCaratColumn = DTabDetail.Columns["Exp.Net Rate"].Ordinal + 1;
                    int ExpDiscColumnn = DTabDetail.Columns["Exp Disc%"].Ordinal + 1;
                    int ExpRxWColumn = DTabDetail.Columns["Exp.RxW"].Ordinal + 1;

                    int MRxWColumn = DTabDetail.Columns["M.RxW"].Ordinal + 1;
                    int MRapaportColumn = DTabDetail.Columns["M.RapPrice"].Ordinal + 1;
                    int MPricePerCaratColumn = DTabDetail.Columns["M.NetRate"].Ordinal + 1;
                    int MDiscountColumn = DTabDetail.Columns["M.Disc%"].Ordinal + 1;
                    int MAmountColumn = DTabDetail.Columns["M.NetValue"].Ordinal + 1;

                    for (int IntI = 2; IntI <= EndRow; IntI++)
                    {
                        string RapColumns = Global.ColumnIndexToColumnLetter(RapaportColumn) + IntI.ToString();
                        string Discount = Global.ColumnIndexToColumnLetter(DiscountColumn) + IntI.ToString();
                        string Carat = Global.ColumnIndexToColumnLetter(CaratColumn) + IntI.ToString();
                        string PricePerCarat = Global.ColumnIndexToColumnLetter(PricePerCaratColumn) + IntI.ToString();
                        string VideoLink = Global.ColumnIndexToColumnLetter(VideoLinkColumn) + IntI.ToString();
                        string OfferPricePerCarat = Global.ColumnIndexToColumnLetter(OfferPricePerCaratColumn) + IntI.ToString();
                        string OfferDiscount = Global.ColumnIndexToColumnLetter(OfferDiscountColumn) + IntI.ToString();


                        worksheet.Cells[IntI, RxWColumn].Formula = "=ROUND(" + RapColumns + " * " + Carat + ",2)";
                        
                        if (Val.ToString(DTabDetail.Rows[IntI - 2]["FANCYCOLOR"]) == "") //Add if condition khushbu 08-10-21 for skip formula in fancy color
                        {
                            worksheet.Cells[IntI, PricePerCaratColumn].Formula = "=(" + RapColumns + " + ((" + RapColumns + " * " + Discount + ") / 100))";
                            worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                            worksheet.Cells[IntI, AmountColumn].Style.Font.Bold = true;
                            worksheet.Cells[IntI, OfferPricePerCaratColumn].Formula = "=(" + RapColumns + " + ((" + RapColumns + " * " + OfferDiscount + ") / 100))";
                            worksheet.Cells[IntI, OfferAmountColumn].Formula = "=ROUND(" + OfferPricePerCarat + " * " + Carat + ",2)";
                            worksheet.Cells[IntI, OfferAmountColumn].Style.Font.Bold = true;
                            worksheet.Cells[IntI, ExpAmountColumn].Style.Font.Bold = true;
                            worksheet.Cells[IntI, MAmountColumn].Style.Font.Bold = true;
                        }
                        else
                        {
                            worksheet.Cells[IntI, PricePerCaratColumn].Value = Val.ToDouble(DTabDetail.Rows[IntI - 2]["Net Rate"]);
                            worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                            worksheet.Cells[IntI, AmountColumn].Style.Font.Bold = true;
                            worksheet.Cells[IntI, OfferPricePerCaratColumn].Value = Val.ToDouble(DTabDetail.Rows[IntI - 2]["Offer Net Rate"]);
                            worksheet.Cells[IntI, OfferAmountColumn].Formula = "=ROUND(" + OfferPricePerCarat + " * " + Carat + ",2)";
                            worksheet.Cells[IntI, OfferAmountColumn].Style.Font.Bold = true;
                            worksheet.Cells[IntI, ExpAmountColumn].Style.Font.Bold = true;
                            worksheet.Cells[IntI, MAmountColumn].Style.Font.Bold = true;
                        }

                        if (IntI != 1)
                        {
                            if (worksheet.Cells[IntI, VideoColumn].Value.ToString() == "")
                            {
                                worksheet.Cells[IntI, VideoColumn, IntI, VideoColumn].Value = "N/A";
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Color.SetColor(Color.Red);
                            }
                            else
                            {
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Color.SetColor(Color.Blue);
                                worksheet.Cells[IntI, VideoColumn].Formula = "=HYPERLINK(" + VideoLink + ", \"Image\")";
                            }

                        }
                    }

                    EndRow = EndRow + 2;
                    worksheet.Cells[EndRow, 1, EndRow, 1].Value = "Summary";

                    string RxW = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["RxW"].Ordinal + 1);
                    string CaratCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Carats"].Ordinal + 1);
                    string Discount1 = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Disc%"].Ordinal + 1);
                    string NetRate = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Net Rate"].Ordinal + 1);
                    string NetValue = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Net Value"].Ordinal + 1);
                    string OfferNetRate = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Offer Net Rate"].Ordinal + 1);
                    string OfferNetValue = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Offer Net Value"].Ordinal + 1);
                    string OfferDiscount1 = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Offer Disc%"].Ordinal + 1);
                    string ExpRxW = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Exp.RxW"].Ordinal + 1);
                    string ExpDisc = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Exp Disc%"].Ordinal + 1);
                    string ExpNetRate = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Exp.Net Rate"].Ordinal + 1);
                    string ExpNetValue = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Exp.Net Value"].Ordinal + 1);

                    string MRxW = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["M.RxW"].Ordinal + 1);
                    string MDiscount = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["M.Disc%"].Ordinal + 1);
                    string MNetRate = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["M.NetRate"].Ordinal + 1);
                    string MNetValue = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["M.NetValue"].Ordinal + 1);


                    int IntTotRow = DTabDetail.Rows.Count + 1;

                    StartRow = StartRow + 1;

                    worksheet.Cells[EndRow, ShapeColumn, EndRow, ShapeColumn].Formula = "SUBTOTAL(2," + CaratCol + StartRow + ":" + CaratCol + IntTotRow + ")";
                    worksheet.Cells[EndRow, CaratColumn, EndRow, CaratColumn].Formula = "ROUND(SUBTOTAL(9," + CaratCol + StartRow + ":" + CaratCol + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, RxWColumn, EndRow, RxWColumn].Formula = "SUBTOTAL(9," + RxW + StartRow + ":" + RxW + IntTotRow + ")";
                    worksheet.Cells[EndRow, AmountColumn, EndRow, AmountColumn].Formula = "ROUND(SUBTOTAL(9," + NetValue + StartRow + ":" + NetValue + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, PricePerCaratColumn, EndRow, PricePerCaratColumn].Formula = "ROUND(" + NetValue + EndRow + "/" + CaratCol + EndRow + ",0)";
                    worksheet.Cells[EndRow, DiscountColumn, EndRow, DiscountColumn].Formula = "ROUND((" + NetValue + EndRow + "/" + RxW + EndRow + "-1 ) * 100,2)";
                    worksheet.Cells[EndRow, OfferAmountColumn, EndRow, OfferAmountColumn].Formula = "ROUND(SUBTOTAL(9," + OfferNetValue + StartRow + ":" + OfferNetValue + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, OfferPricePerCaratColumn, EndRow, OfferPricePerCaratColumn].Formula = "ROUND(" + OfferNetValue + EndRow + "/" + CaratCol + EndRow + ",0)";
                    worksheet.Cells[EndRow, OfferDiscountColumn, EndRow, OfferDiscountColumn].Formula = "ROUND((" + OfferNetValue + EndRow + "/" + RxW + EndRow + "-1 ) * 100,2)";

                    worksheet.Cells[EndRow, ExpAmountColumn, EndRow, ExpAmountColumn].Formula = "ROUND(SUBTOTAL(9," + ExpNetValue + StartRow + ":" + ExpNetValue + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, ExpPricePerCaratColumn, EndRow, ExpPricePerCaratColumn].Formula = "ROUND(" + ExpNetValue + EndRow + "/" + CaratCol + EndRow + ",0)";
                    worksheet.Cells[EndRow, ExpDiscColumnn, EndRow, ExpDiscColumnn].Formula = "ROUND((" + ExpNetValue + EndRow + "/" + ExpRxW + EndRow + "-1 ) * 100,2)";
                    worksheet.Cells[EndRow, ExpRxWColumn, EndRow, ExpRxWColumn].Formula = "SUBTOTAL(9," + ExpRxW + StartRow + ":" + ExpRxW + IntTotRow + ")";

                    worksheet.Cells[EndRow, MAmountColumn, EndRow, MAmountColumn].Formula = "ROUND(SUBTOTAL(9," + MNetValue + StartRow + ":" + MNetValue + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, MPricePerCaratColumn, EndRow, MPricePerCaratColumn].Formula = "ROUND(" + MNetValue + EndRow + "/" + CaratCol + EndRow + ",0)";
                    worksheet.Cells[EndRow, MDiscountColumn, EndRow, MDiscountColumn].Formula = "ROUND((" + MNetValue + EndRow + "/" + MRxW + EndRow + " ) * 100,2)";
                    worksheet.Cells[EndRow, MRxWColumn, EndRow, MRxWColumn].Formula = "SUBTOTAL(9," + MRxW + StartRow + ":" + MRxW + IntTotRow + ")";

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Font.Bold = true;

                    worksheet.Cells[StartRow, CaratColumn, EndRow, CaratColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, DiscountColumn, EndRow, DiscountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, AmountColumn, EndRow, AmountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, DepthPerColumn, EndRow, DepthPerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, TablePerColumn, EndRow, TablePerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, GirdlePerColumn, EndRow, GirdlePerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, CAColumn, EndRow, CAColumn + 3].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, OfferDiscountColumn, EndRow, OfferDiscountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, OfferAmountColumn, EndRow, OfferAmountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, ExpDiscColumnn, EndRow, ExpDiscColumnn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, ExpAmountColumn, EndRow, ExpAmountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, MDiscountColumn, EndRow, MDiscountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, MAmountColumn, EndRow, MAmountColumn].Style.Numberformat.Format = "0.00";

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    #endregion

                    worksheet.Cells[1, 1, 100, 100].AutoFitColumns();
                   
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

        public string ExportExcelWithReviseList(DataSet DS, string PStrFilePath) //Add Darshan
        {
            try
            {

                DataTable DTabDetail = DS.Tables[0];

                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }

                DTabDetail.DefaultView.Sort = "SrNo";
                DTabDetail = DTabDetail.DefaultView.ToTable();

                this.Cursor = Cursors.WaitCursor;

                string StrFilePath = PStrFilePath;

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.Yellow;
                Color FontColor = Color.Black;
                string FontName = "Calibri";
                float FontSize = 11;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Revise Stock List");
                   
                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    #region Stock Detail

                    EndRow = StartRow + DTabDetail.Rows.Count;

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
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(FontColor);

                    int RxWColumn = DTabDetail.Columns["RxW"].Ordinal + 1;
                    int RapaportColumn = DTabDetail.Columns["Rap. Price"].Ordinal + 1;
                    int PricePerCaratColumn = DTabDetail.Columns["Net Rate"].Ordinal + 1;
                    int DiscountColumn = DTabDetail.Columns["Disc%"].Ordinal + 1;
                    int CaratColumn = DTabDetail.Columns["Carats"].Ordinal + 1;
                    int AmountColumn = DTabDetail.Columns["Net Value"].Ordinal + 1;
                    int VideoLinkColumn = DTabDetail.Columns["Video Link"].Ordinal + 1;
                    int VideoColumn = DTabDetail.Columns["Video"].Ordinal + 1;
                    int DepthPerColumn = DTabDetail.Columns["Depth%"].Ordinal + 1;
                    int TablePerColumn = DTabDetail.Columns["Table%"].Ordinal + 1;
                    int ShapeColumn = DTabDetail.Columns["Shape"].Ordinal + 1;

                    int GirdlePerColumn = DTabDetail.Columns["Girldle Per"].Ordinal + 1;
                    int CAColumn = DTabDetail.Columns["Crown Angle"].Ordinal + 1;

                    int SysRxWColumn = DTabDetail.Columns["Sys RxW"].Ordinal + 1;
                    int SysRapaportColumn = DTabDetail.Columns["Sys Rap. Price"].Ordinal + 1;
                    int SysDiscountColumn = DTabDetail.Columns["Sys Disc%"].Ordinal + 1;
                    int SysPricePerCaratColumn = DTabDetail.Columns["Sys Net Rate"].Ordinal + 1;
                    int SysAmountColumn = DTabDetail.Columns["Sys Net Value"].Ordinal + 1;


                    for (int IntI = 2; IntI <= EndRow; IntI++)
                    {
                        string RapColumns = Global.ColumnIndexToColumnLetter(RapaportColumn) + IntI.ToString();
                        string Discount = Global.ColumnIndexToColumnLetter(DiscountColumn) + IntI.ToString();
                        string Carat = Global.ColumnIndexToColumnLetter(CaratColumn) + IntI.ToString();
                        string PricePerCarat = Global.ColumnIndexToColumnLetter(PricePerCaratColumn) + IntI.ToString();
                        string VideoLink = Global.ColumnIndexToColumnLetter(VideoLinkColumn) + IntI.ToString();

                        string SysRapColumns = Global.ColumnIndexToColumnLetter(SysRapaportColumn) + IntI.ToString();
                        string SysDiscount = Global.ColumnIndexToColumnLetter(SysDiscountColumn) + IntI.ToString();
                        string SysPricePerCarat = Global.ColumnIndexToColumnLetter(SysPricePerCaratColumn) + IntI.ToString();

                        worksheet.Cells[IntI, RxWColumn].Formula = "=ROUND(" + RapColumns + " * " + Carat + ",2)";
                       
                        if (Val.ToString(DTabDetail.Rows[IntI - 2]["FANCYCOLOR"]) == "") //Add if condition khushbu 08-10-21 for skip formula in fancy color
                        {
                            worksheet.Cells[IntI, PricePerCaratColumn].Formula = "=(" + RapColumns + " + ((" + RapColumns + " * " + Discount + ") / 100))";
                            worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                            worksheet.Cells[IntI, AmountColumn].Style.Font.Bold = true;

                            worksheet.Cells[IntI, SysPricePerCaratColumn].Formula = "=(" + SysRapColumns + " + ((" + SysRapColumns + " * " + SysDiscount + ") / 100))";
                            worksheet.Cells[IntI, SysAmountColumn].Formula = "=ROUND(" + SysPricePerCarat + " * " + Carat + ",2)";
                            worksheet.Cells[IntI, SysAmountColumn].Style.Font.Bold = true;
                        }
                        else
                        {
                            worksheet.Cells[IntI, PricePerCaratColumn].Value = Val.ToDouble(DTabDetail.Rows[IntI - 2]["Net Rate"]);
                            worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                            worksheet.Cells[IntI, AmountColumn].Style.Font.Bold = true;

                            worksheet.Cells[IntI, SysPricePerCaratColumn].Value = Val.ToDouble(DTabDetail.Rows[IntI - 2]["Sys Net Rate"]);
                            worksheet.Cells[IntI, SysAmountColumn].Formula = "=ROUND(" + SysPricePerCarat + " * " + Carat + ",2)";
                            worksheet.Cells[IntI, SysAmountColumn].Style.Font.Bold = true;
                        }

                        if (IntI != 1)
                        {
                            if (worksheet.Cells[IntI, VideoColumn].Value.ToString() == "")
                            {
                                worksheet.Cells[IntI, VideoColumn, IntI, VideoColumn].Value = "N/A";
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Color.SetColor(Color.Red);
                            }
                            else
                            {
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Color.SetColor(Color.Blue);
                                worksheet.Cells[IntI, VideoColumn].Formula = "=HYPERLINK(" + VideoLink + ", \"Image\")";
                            }

                        }
                    }

                    EndRow = EndRow + 2;
                    worksheet.Cells[EndRow, 1, EndRow, 1].Value = "Summary";

                    string RxW = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["RxW"].Ordinal + 1);
                    string CaratCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Carats"].Ordinal + 1);
                    string Discount1 = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Disc%"].Ordinal + 1);
                    string NetRate = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Net Rate"].Ordinal + 1);
                    string NetValue = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Net Value"].Ordinal + 1);

                    string SysRxW = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Sys RxW"].Ordinal + 1);
                    string SysDiscount1 = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Sys Disc%"].Ordinal + 1);
                    string SysNetRate = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Sys Net Rate"].Ordinal + 1);
                    string SysNetValue = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Sys Net Value"].Ordinal + 1);

                    int IntTotRow = DTabDetail.Rows.Count + 1;

                    StartRow = StartRow + 1;

                    worksheet.Cells[EndRow, ShapeColumn, EndRow, ShapeColumn].Formula = "SUBTOTAL(2," + CaratCol + StartRow + ":" + CaratCol + IntTotRow + ")";
                    worksheet.Cells[EndRow, CaratColumn, EndRow, CaratColumn].Formula = "ROUND(SUBTOTAL(9," + CaratCol + StartRow + ":" + CaratCol + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, RxWColumn, EndRow, RxWColumn].Formula = "SUBTOTAL(9," + RxW + StartRow + ":" + RxW + IntTotRow + ")";
                    worksheet.Cells[EndRow, AmountColumn, EndRow, AmountColumn].Formula = "ROUND(SUBTOTAL(9," + NetValue + StartRow + ":" + NetValue + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, PricePerCaratColumn, EndRow, PricePerCaratColumn].Formula = "ROUND(" + NetValue + EndRow + "/" + CaratCol + EndRow + ",0)";
                    worksheet.Cells[EndRow, DiscountColumn, EndRow, DiscountColumn].Formula = "ROUND((" + NetValue + EndRow + "/" + RxW + EndRow + "-1 ) * 100,2)";

                    worksheet.Cells[EndRow, SysRxWColumn, EndRow, SysRxWColumn].Formula = "SUBTOTAL(9," + SysRxW + StartRow + ":" + SysRxW + IntTotRow + ")";
                    worksheet.Cells[EndRow, SysAmountColumn, EndRow, SysAmountColumn].Formula = "ROUND(SUBTOTAL(9," + SysNetValue + StartRow + ":" + SysNetValue + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, SysPricePerCaratColumn, EndRow, SysPricePerCaratColumn].Formula = "ROUND(" + SysNetValue + EndRow + "/" + CaratCol + EndRow + ",0)";
                    worksheet.Cells[EndRow, SysDiscountColumn, EndRow, SysDiscountColumn].Formula = "ROUND((" + SysNetValue + EndRow + "/" + SysRxW + EndRow + "-1 ) * 100,2)";

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Font.Bold = true;

                    worksheet.Cells[StartRow, CaratColumn, EndRow, CaratColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, DiscountColumn, EndRow, DiscountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, AmountColumn, EndRow, AmountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, DepthPerColumn, EndRow, DepthPerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, TablePerColumn, EndRow, TablePerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, GirdlePerColumn, EndRow, GirdlePerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, CAColumn, EndRow, CAColumn + 3].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, SysDiscountColumn, EndRow, SysDiscountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, SysAmountColumn, EndRow, SysAmountColumn].Style.Numberformat.Format = "0.00";

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    #endregion

                    worksheet.Cells[1, 1, 100, 100].AutoFitColumns();                 
                    worksheet.Column(19).Hidden = true; 
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

        private string ExportExcelWithProportionFile(DataSet DS, string PStrFilePath)//Add Gunjan :03/03/2023
        {
            try
            {

                DataTable DTabDetail = DS.Tables[0];

                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }

                DTabDetail.DefaultView.Sort = "SrNo";
                DTabDetail = DTabDetail.DefaultView.ToTable();

                this.Cursor = Cursors.WaitCursor;

                string StrFilePath = PStrFilePath;

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.Yellow;
                Color FontColor = Color.Black;
                string FontName = "Calibri";
                float FontSize = 11;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Proportion File");

                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    int invAmtColumn = DTabDetail.Columns["Inv Amount"].Ordinal + 1;
                    int AmtColuumn = DTabDetail.Columns["Amount"].Ordinal + 1;
                    int PricePerCaratColumn = DTabDetail.Columns["Per Carat"].Ordinal + 1;
                    int DiscountColumn = DTabDetail.Columns["Disc %"].Ordinal + 1;
                    int InvDiscColumn = DTabDetail.Columns["Inv Disc %"].Ordinal + 1;
                    int InvPerCaratcolumn = DTabDetail.Columns["InvPer Carat"].Ordinal + 1;

                    EndRow = StartRow + DTabDetail.Rows.Count;

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
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(FontColor);

                    worksheet.Cells[StartRow, invAmtColumn, EndRow, invAmtColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, AmtColuumn, EndRow, AmtColuumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, PricePerCaratColumn, EndRow, PricePerCaratColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, DiscountColumn, EndRow, DiscountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, InvDiscColumn, EndRow, InvDiscColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, InvPerCaratcolumn, EndRow, InvPerCaratcolumn].Style.Numberformat.Format = "0.00";

                    worksheet.Cells[1, 1, 100, 100].AutoFitColumns();

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

        //ADD BY RAJVI : 21/05/2025
        private string ExportExcelWithIGI(DataSet DS, string PStrFilePath)
        {
            try
            {

                DataTable DTabDetail = DS.Tables[0];

                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }

                //DTabDetail.DefaultView.Sort = "SrNo";
                DTabDetail = DTabDetail.DefaultView.ToTable();
                this.Cursor = Cursors.WaitCursor;
                string StrFilePath = PStrFilePath;
                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }
                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.Yellow;
                Color FontColor = Color.Black;
                string FontName = "Calibri";
                float FontSize = 11;
                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("IGI");

                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    //int invAmtColumn = DTabDetail.Columns["Inv Amount"].Ordinal + 1;
                    //int AmtColuumn = DTabDetail.Columns["Amount"].Ordinal + 1;
                    //int PricePerCaratColumn = DTabDetail.Columns["Per Carat"].Ordinal + 1;
                    //int DiscountColumn = DTabDetail.Columns["Disc %"].Ordinal + 1;
                    //int InvDiscColumn = DTabDetail.Columns["Inv Disc %"].Ordinal + 1;
                    //int InvPerCaratcolumn = DTabDetail.Columns["InvPer Carat"].Ordinal + 1;

                    EndRow = StartRow + DTabDetail.Rows.Count;

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
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(FontColor);

                    //worksheet.Cells[StartRow, invAmtColumn, EndRow, invAmtColumn].Style.Numberformat.Format = "0.00";
                    //worksheet.Cells[StartRow, AmtColuumn, EndRow, AmtColuumn].Style.Numberformat.Format = "0.00";
                    //worksheet.Cells[StartRow, PricePerCaratColumn, EndRow, PricePerCaratColumn].Style.Numberformat.Format = "0.00";
                    //worksheet.Cells[StartRow, DiscountColumn, EndRow, DiscountColumn].Style.Numberformat.Format = "0.00";
                    //worksheet.Cells[StartRow, InvDiscColumn, EndRow, InvDiscColumn].Style.Numberformat.Format = "0.00";
                    //worksheet.Cells[StartRow, InvPerCaratcolumn, EndRow, InvPerCaratcolumn].Style.Numberformat.Format = "0.00";

                    worksheet.Cells[1, 1, 100, 100].AutoFitColumns();

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
        //END RAJVI 

        //ADD BY RAJVI : 21/05/2025
        private string ExportExcelWithVDB(DataSet DS, string PStrFilePath)
        {
            try
            {

                DataTable DTabDetail = DS.Tables[0];

                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }

                //DTabDetail.DefaultView.Sort = "SrNo";
                DTabDetail = DTabDetail.DefaultView.ToTable();
                this.Cursor = Cursors.WaitCursor;
                string StrFilePath = PStrFilePath;
                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }
                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.Yellow;
                Color FontColor = Color.Black;
                string FontName = "Calibri";
                float FontSize = 11;
                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("VDB");

                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    //int invAmtColumn = DTabDetail.Columns["Inv Amount"].Ordinal + 1;
                    //int AmtColuumn = DTabDetail.Columns["Amount"].Ordinal + 1;
                    //int PricePerCaratColumn = DTabDetail.Columns["Per Carat"].Ordinal + 1;
                    //int DiscountColumn = DTabDetail.Columns["Disc %"].Ordinal + 1;
                    //int InvDiscColumn = DTabDetail.Columns["Inv Disc %"].Ordinal + 1;
                    //int InvPerCaratcolumn = DTabDetail.Columns["InvPer Carat"].Ordinal + 1;

                    EndRow = StartRow + DTabDetail.Rows.Count;

                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].LoadFromDataTable(DTabDetail, true);
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Name = FontName;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Size = FontSize;

                    worksheet.View.FreezePanes(2, 1);
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].AutoFilter = true;

                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Font.Bold = true;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(FontColor);

                    //worksheet.Cells[StartRow, invAmtColumn, EndRow, invAmtColumn].Style.Numberformat.Format = "0.00";
                    //worksheet.Cells[StartRow, AmtColuumn, EndRow, AmtColuumn].Style.Numberformat.Format = "0.00";
                    //worksheet.Cells[StartRow, PricePerCaratColumn, EndRow, PricePerCaratColumn].Style.Numberformat.Format = "0.00";
                    //worksheet.Cells[StartRow, DiscountColumn, EndRow, DiscountColumn].Style.Numberformat.Format = "0.00";
                    //worksheet.Cells[StartRow, InvDiscColumn, EndRow, InvDiscColumn].Style.Numberformat.Format = "0.00";
                    //worksheet.Cells[StartRow, InvPerCaratcolumn, EndRow, InvPerCaratcolumn].Style.Numberformat.Format = "0.00";

                    worksheet.Cells[1, 1, 100, 100].AutoFitColumns();

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
        //END RAJVI 


        private string ExportExcelWithGeneral(DataSet DS, string PStrFilePath)
        {
            try
            {
                DataTable DTabDetail = DS.Tables[0];
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

                string StrFilePath = PStrFilePath;
                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.FromArgb(2, 68, 143);
                //Color BackColor = Color.FromArgb(119, 50, 107);
                Color FontColor = Color.White;
                string FontName = "Calibri";
                float FontSize = 9;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("TRP_Stock_" + DateTime.Now.ToString("ddMMyyyy"));
                    ExcelWorksheet worksheetProportion = xlPackage.Workbook.Worksheets.Add("Proportion");
                    ExcelWorksheet worksheetInclusion = xlPackage.Workbook.Worksheets.Add("Inclusion Detail");


                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow + DTabDetail.Rows.Count;
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

                    worksheet.View.FreezePanes(2, 1);

                    // Set Hyperlink
                    int IntCertColumn = DTabDetail.Columns["Cert No"].Ordinal;
                    int IntVideoUrlColumn = DTabDetail.Columns["Video URL"].Ordinal;
                    int IntStoneDEtailUrlColumn = DTabDetail.Columns["Media URL"].Ordinal;

                    int RapaportColumn = DTabDetail.Columns["RapRate"].Ordinal + 1;
                    int RapaportValueColumn = DTabDetail.Columns["RapValue"].Ordinal + 1;
                    int PricePerCaratColumn = DTabDetail.Columns["Pr/Ct"].Ordinal + 1;
                    int DiscountColumn = DTabDetail.Columns["Rap %"].Ordinal + 1;
                    int CaratColumn = DTabDetail.Columns["Carat"].Ordinal + 1;
                    int AmountColumn = DTabDetail.Columns["Amount"].Ordinal + 1;

                    for (int IntI = 2; IntI <= EndRow; IntI++)
                    {
                        string RapColumns = Global.ColumnIndexToColumnLetter(RapaportColumn) + IntI.ToString();
                        string Discount = Global.ColumnIndexToColumnLetter(DiscountColumn) + IntI.ToString();
                        string Carat = Global.ColumnIndexToColumnLetter(CaratColumn) + IntI.ToString();
                        string PricePerCarat = Global.ColumnIndexToColumnLetter(PricePerCaratColumn) + IntI.ToString();

                        worksheet.Cells[IntI, RapaportValueColumn].Formula = "=ROUND(" + RapColumns + " * " + Carat + ",2)";
                        if (Val.ToString(DTabDetail.Rows[IntI - 2]["FancyColor"]) == "")
                        {
                            worksheet.Cells[IntI, PricePerCaratColumn].Formula = "=ROUND(" + RapColumns + " + ((" + RapColumns + " * " + Discount + ") / 100),2)";
                            worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                        }
                        else
                        {
                            worksheet.Cells[IntI, PricePerCaratColumn].Value = Val.ToString(DTabDetail.Rows[IntI - 2]["Pr/Ct"]);
                            //worksheet.Cells[IntI, AmountColumn].Value = Val.ToString(DTabDetail.Rows[IntI - 6]["AMOUNT"]);
                            worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                        }

                        if (!Val.ToString(DTabDetail.Rows[IntI - 2]["DNA Page URL"]).Trim().Equals(string.Empty))
                        {
                            //worksheet.Cells[IntI, 2, IntI, 2].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["DNAPAGEURL"]));
                            worksheet.Cells[IntI, 2, IntI, 2].Style.Font.Name = FontName;
                            worksheet.Cells[IntI, 2, IntI, 2].Style.Font.Bold = true;
                            worksheet.Cells[IntI, 2, IntI, 2].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                            //worksheet.Cells[IntI, 2, IntI, 2].Style.Font.UnderLine = true;
                        }

                        if (!Val.ToString(DTabDetail.Rows[IntI - 2]["Cert URL"]).Trim().Equals(string.Empty))
                        {
                            worksheet.Cells[IntI, IntCertColumn + 1].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 2]["Cert URL"]));
                            worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.Name = FontName;
                            worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.Bold = true;
                            worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                            worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.UnderLine = true;
                        }


                        if (!Val.ToString(DTabDetail.Rows[IntI - 2]["Video URL"]).Trim().Equals(string.Empty))
                        {
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Value = "Video";
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 2]["Video URL"]));
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Style.Font.Name = FontName;
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Style.Font.Bold = true;
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Style.Font.UnderLine = true;
                        }

                        if (!Val.ToString(DTabDetail.Rows[IntI - 2]["Media URL"]).Trim().Equals(string.Empty))
                        {
                            //worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Value = "Detail";
                            worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 2]["Media URL"]));
                            worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Style.Font.Name = FontName;
                            worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Style.Font.Bold = true;
                            worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                            worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Style.Font.UnderLine = true;
                        }
                    }

                    EndRow = EndRow + 2;
                    worksheet.Cells[EndRow, 1, EndRow, 1].Value = "Summary";

                    string ShortCaratCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Carat"].Ordinal + 1);
                    string ShortRap = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["RapRate"].Ordinal + 1);
                    string ShortRapValue = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["RapValue"].Ordinal + 1);
                    string ShortDiscount = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Rap %"].Ordinal + 1);
                    string ShortPricePerCarat = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Pr/Ct"].Ordinal + 1);
                    string ShortAmt = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Amount"].Ordinal + 1);

                    int IntShortTotRow = DTabDetail.Rows.Count + 1;

                    StartRow = StartRow + 1;
                    worksheet.Cells[EndRow, CaratColumn, EndRow, CaratColumn].Formula = "ROUND(SUBTOTAL(9," + ShortCaratCol + StartRow + ":" + ShortCaratCol + IntShortTotRow + "),2)";

                    worksheet.Cells[EndRow, RapaportColumn, EndRow, RapaportColumn].Formula = "ROUND(" + ShortRapValue + EndRow + "/" + ShortCaratCol + EndRow + ",0)";
                    worksheet.Cells[EndRow, RapaportValueColumn, EndRow, RapaportValueColumn].Formula = "ROUND(SUBTOTAL(9," + ShortRapValue + StartRow + ":" + ShortRapValue + IntShortTotRow + "),2)";

                    worksheet.Cells[EndRow, DiscountColumn, EndRow, DiscountColumn].Formula = "ROUND((" + ShortAmt + EndRow + "/" + ShortRapValue + EndRow + "-1 ) * 100,2)";

                    worksheet.Cells[EndRow, PricePerCaratColumn, EndRow, PricePerCaratColumn].Formula = "ROUND(" + ShortAmt + EndRow + "/" + ShortCaratCol + EndRow + ",0)";
                    worksheet.Cells[EndRow, AmountColumn, EndRow, AmountColumn].Formula = "ROUND(SUBTOTAL(9," + ShortAmt + StartRow + ":" + ShortAmt + IntShortTotRow + "),2)";

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Font.Bold = true;

                    worksheet.Cells[StartRow, CaratColumn, EndRow, CaratColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, RapaportColumn, EndRow, RapaportColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, RapaportValueColumn, EndRow, RapaportValueColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, DiscountColumn, EndRow, DiscountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, PricePerCaratColumn, EndRow, PricePerCaratColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, AmountColumn, EndRow, AmountColumn].Style.Numberformat.Format = "0.00";


                    int IntRowStartsFrom = 3;
                    int IntRowEndTo = (DTabDetail.Rows.Count - 1 + IntRowStartsFrom);

                    string StrStartRow = "6";
                    string StrEndRow = EndRow.ToString();

                    #region Inclusion Detail

                    AddInclusionDetail(worksheetInclusion, DTabInclusion);

                    #endregion

                    #region Proporstion Detail

                    worksheetProportion.Cells[2, 2, 3, 22].Value = "Stock Proportion";
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Font.Name = FontName;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Font.Size = 20;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Font.Bold = true;

                    worksheetProportion.Cells[2, 2, 3, 22].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 22].Merge = true;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheetProportion.Cells[2, 2, 3, 22].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Font.Color.SetColor(FontColor);

                    int NewRow = 6;
                    AddProportionDetail(worksheetProportion, DTabSize, worksheet.Name, 6, 2, "SIZE WISE SUMMARY", "Size", "Size", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabShape, worksheet.Name, 6, 13, "SHAPE WISE SUMMARY", "Shape", "Shape", StrStartRow, StrEndRow, DTabDetail);

                    if (DTabSize.Rows.Count > DTabShape.Rows.Count)
                    {
                        NewRow = NewRow + DTabSize.Rows.Count + 5;
                    }
                    else
                    {
                        NewRow = NewRow + DTabShape.Rows.Count + 5;
                    }

                    AddProportionDetail(worksheetProportion, DTabClarity, worksheet.Name, NewRow, 2, "CLARITY WISE SUMMARY", "Cla", "ClaGroup", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabColor, worksheet.Name, NewRow, 13, "COLOR WISE SUMMARY", "Col", "ColGroup", StrStartRow, StrEndRow, DTabDetail);


                    if (DTabClarity.Rows.Count > DTabColor.Rows.Count)
                    {
                        NewRow = NewRow + DTabClarity.Rows.Count + 5;
                    }
                    else
                    {
                        NewRow = NewRow + DTabColor.Rows.Count + 5;
                    }

                    AddProportionDetail(worksheetProportion, DTabCut, worksheet.Name, NewRow, 2, "CUT WISE SUMMARY", "Cut", "CutGroup", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabPolish, worksheet.Name, NewRow, 13, "POLISH WISE SUMMARY", "Pol", "PolGroup", StrStartRow, StrEndRow, DTabDetail);


                    if (DTabCut.Rows.Count > DTabPolish.Rows.Count)
                    {
                        NewRow = NewRow + DTabCut.Rows.Count + 5;
                    }
                    else
                    {
                        NewRow = NewRow + DTabPolish.Rows.Count + 5;
                    }

                    AddProportionDetail(worksheetProportion, DTabSym, worksheet.Name, NewRow, 2, "SYM WISE SUMMARY", "Sym", "SymGroup", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabFL, worksheet.Name, NewRow, 13, "FL WISE SUMMARY", "FL", "FLGroup", StrStartRow, StrEndRow, DTabDetail);

                    #endregion

                    //worksheet.Cells[1, 1, 100, 100].AutoFitColumns();

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

        private string ExportExcelWithMFGComparision(DataSet DS, string PStrFilePath)
        {
            try
            {

                this.Cursor = Cursors.Default;

                Color lblDown = Color.Purple, lblUp = Color.Red;
                DataTable DTabDetail = DS.Tables[0];
                DataTable DtabMemo = DS.Tables[1];

                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }

                DTabDetail.DefaultView.Sort = "Lot Name";
                DTabDetail = DTabDetail.DefaultView.ToTable();

                DtabMemo.DefaultView.Sort = "KAPANNAME";
                DtabMemo = DtabMemo.DefaultView.ToTable();

                this.Cursor = Cursors.WaitCursor;

                string StrFilePath = PStrFilePath;
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
                int EndColumn = 0;


                int StartRowMemo = 0;
                int StartColumnMemo = 0;
                int EndRowMemo = 0;
                int EndColumnMemo = 0;


                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Mfg.Comp");
                    ExcelWorksheet worksheetMemo = xlPackage.Workbook.Worksheets.Add("Kapan");

                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;


                    StartRowMemo = 1;
                    StartColumnMemo = 1;
                    EndRowMemo = StartRowMemo;
                    EndColumnMemo = DtabMemo.Columns.Count;

                    #region Stock Detail
                    //Mfg.Comp Detail
                    EndRow = StartRow + DTabDetail.Rows.Count;

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
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Color.SetColor(FontColor);

                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(Color.Yellow);

                    int IntTotRow = DTabDetail.Rows.Count + 1;

                    int ShapeColumn = DTabDetail.Columns["Shape"].Ordinal + 1;
                    int CaratsColumn = DTabDetail.Columns["Carats"].Ordinal + 1;
                    int SuartRateColumn = DTabDetail.Columns["Surat Rate"].Ordinal + 1;
                    int SuratValueColumn = DTabDetail.Columns["Surat Value"].Ordinal + 1;
                    int CostRateColumn = DTabDetail.Columns["Cost Rate"].Ordinal + 1;
                    int CostValueColumn = DTabDetail.Columns["Cost Value"].Ordinal + 1;
                    int AvgRateColumn = DTabDetail.Columns["Avg Rate"].Ordinal + 1;
                    int AvgValueColumn = DTabDetail.Columns["Avg Value"].Ordinal + 1;
                    int LiveRateColumn = DTabDetail.Columns["Live Rate"].Ordinal + 1;
                    int LiveValueColumn = DTabDetail.Columns["Live Value"].Ordinal + 1;
                    int LiveRapColumn = DTabDetail.Columns["Live Rap"].Ordinal + 1;
                    int LiveDiscountColumn = DTabDetail.Columns["Live Disc%"].Ordinal + 1;
                    int RxWColumn = DTabDetail.Columns["RxW"].Ordinal + 1;
                    int VideoLinkColumn = DTabDetail.Columns["Video Link"].Ordinal + 1;
                    int VideoColumn = DTabDetail.Columns["Video"].Ordinal + 1;

                    int ColorColumn = DTabDetail.Columns["Color"].Ordinal + 1;
                    int ColorUpdownColumn = DTabDetail.Columns["ColorUpDown"].Ordinal + 1;
                    int ClarityColumn = DTabDetail.Columns["Clarity"].Ordinal + 1;
                    int ClarityUpdownColumn = DTabDetail.Columns["ClarityUpDown"].Ordinal + 1;
                    int CutColumn = DTabDetail.Columns["Cut"].Ordinal + 1;
                    int CutUpdownColumn = DTabDetail.Columns["CutUpDown"].Ordinal + 1;
                    int PolColumn = DTabDetail.Columns["Polish"].Ordinal + 1;
                    int PolUpdownColumn = DTabDetail.Columns["PolishUpDown"].Ordinal + 1;
                    int SymColumn = DTabDetail.Columns["Symm"].Ordinal + 1;
                    int SymUpdownColumn = DTabDetail.Columns["SymUpDown"].Ordinal + 1;
                    int FlrColumn = DTabDetail.Columns["Flour"].Ordinal + 1;
                    int FlrUpdownColumn = DTabDetail.Columns["FlrUpDown"].Ordinal + 1;

                    int RapColumn = DTabDetail.Columns["Cost Rap"].Ordinal + 1;
                    int RapUpdownColumn = DTabDetail.Columns["RapUpDown"].Ordinal + 1;
                    int GIAChargeColumn = DTabDetail.Columns["GIA Charge"].Ordinal + 1;
                    int ChargesAmtColumn = DTabDetail.Columns["Charges Amt"].Ordinal + 1;
                    int AddLess1AmtColumn = DTabDetail.Columns["AddLess1Amt"].Ordinal + 1;
                    int AddLess1Column = DTabDetail.Columns["AddLess1"].Ordinal + 1;
                    int AddLess2Column = DTabDetail.Columns["AddLess2"].Ordinal + 1;
                    int OrgCaratColumn = DTabDetail.Columns["Org.Carat"].Ordinal + 1;

                    for (int IntI = 2; IntI <= EndRow; IntI++)
                    {
                        string Carat = Global.ColumnIndexToColumnLetter(CaratsColumn) + IntI.ToString();
                        string LiveValue = Global.ColumnIndexToColumnLetter(LiveValueColumn) + IntI.ToString();
                        string LiveRap = Global.ColumnIndexToColumnLetter(LiveRapColumn) + IntI.ToString();
                        string Rxw = Global.ColumnIndexToColumnLetter(RxWColumn) + IntI.ToString();

                        string VideoLink = Global.ColumnIndexToColumnLetter(VideoLinkColumn) + IntI.ToString();

                        worksheet.Cells[IntI, LiveDiscountColumn].Formula = "ROUND((" + LiveValue + "/" + Rxw + "-1 ) * 100,2)";
                        worksheet.Cells[IntI, RxWColumn].Formula = "=ROUND(" + LiveRap + " * " + Carat + ",2)";

                        string CostValue = Global.ColumnIndexToColumnLetter(CostValueColumn) + IntI.ToString();
                        string GIACharge = Global.ColumnIndexToColumnLetter(GIAChargeColumn) + IntI.ToString();
                        string ChargesAmt = Global.ColumnIndexToColumnLetter(ChargesAmtColumn) + IntI.ToString();
                        string AddLess1 = Global.ColumnIndexToColumnLetter(AddLess1Column) + IntI.ToString();
                        string AddLess2 = Global.ColumnIndexToColumnLetter(AddLess2Column) + IntI.ToString();
                        string AddLess1Amt = Global.ColumnIndexToColumnLetter(AddLess1AmtColumn) + IntI.ToString();
                        string AvgValue = Global.ColumnIndexToColumnLetter(AvgValueColumn) + IntI.ToString();
                        string OrgCarat = Global.ColumnIndexToColumnLetter(OrgCaratColumn) + IntI.ToString();

                        worksheet.Cells[IntI, ChargesAmtColumn].Formula = "=" + CostValue + "-" + GIACharge;//=AF2-AG2                        
                        worksheet.Cells[IntI, AddLess1AmtColumn].Formula = "=(" + ChargesAmt + ")-(" + ChargesAmt + "*" + AddLess1 + "/100)"; //=(AJ2)-(AJ2*AH2/100)
                        worksheet.Cells[IntI, AvgValueColumn].Formula = "=(" + AddLess1Amt + ")-(" + AddLess1Amt + "*" + AddLess2 + "/100)"; //= (AK2) - (AK2 * AI2 / 100)
                        worksheet.Cells[IntI, AvgRateColumn].Formula = "=" + AvgValue + "/" + OrgCarat; //=AM2/H2

                        if (IntI != 1)
                        {
                            if (worksheet.Cells[IntI, VideoColumn].Value.ToString() == "")
                            {
                                worksheet.Cells[IntI, VideoColumn, IntI, VideoColumn].Value = "N/A";
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Color.SetColor(Color.Red);
                            }
                            else
                            {
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Color.SetColor(Color.Blue);
                                worksheet.Cells[IntI, VideoColumn].Formula = "=HYPERLINK(" + VideoLink + ", \"Image\")";
                            }
                        }

                        if (Val.ToString(worksheet.Cells[IntI, ColorUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[IntI, ColorColumn, IntI, ColorColumn].Style.Fill.BackgroundColor.SetColor(lblUp);
                            worksheet.Cells[IntI, ColorColumn, IntI, ColorColumn].Style.Font.Color.SetColor(BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[IntI, ColorUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[IntI, ColorColumn, IntI, ColorColumn].Style.Fill.BackgroundColor.SetColor(lblDown);
                            worksheet.Cells[IntI, ColorColumn, IntI, ColorColumn].Style.Font.Color.SetColor(BackColor);
                        }
                        else
                        {
                            worksheet.Cells[IntI, ColorColumn, IntI, ColorColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }

                        if (Val.ToString(worksheet.Cells[IntI, ClarityUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[IntI, ClarityColumn, IntI, ClarityColumn].Style.Fill.BackgroundColor.SetColor(lblUp);
                            worksheet.Cells[IntI, ClarityColumn, IntI, ClarityColumn].Style.Font.Color.SetColor(BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[IntI, ClarityUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[IntI, ClarityColumn, IntI, ClarityColumn].Style.Fill.BackgroundColor.SetColor(lblDown);
                            worksheet.Cells[IntI, ClarityColumn, IntI, ClarityColumn].Style.Font.Color.SetColor(BackColor);
                        }
                        else
                        {
                            worksheet.Cells[IntI, ClarityColumn, IntI, ClarityColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }

                        if (Val.ToString(worksheet.Cells[IntI, CutUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[IntI, CutColumn, IntI, CutColumn].Style.Fill.BackgroundColor.SetColor(lblUp);
                            worksheet.Cells[IntI, CutColumn, IntI, CutColumn].Style.Font.Color.SetColor(BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[IntI, CutUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[IntI, CutColumn, IntI, CutColumn].Style.Fill.BackgroundColor.SetColor(lblDown);
                            worksheet.Cells[IntI, CutColumn, IntI, CutColumn].Style.Font.Color.SetColor(BackColor);
                        }
                        else
                        {
                            worksheet.Cells[IntI, CutColumn, IntI, CutColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }

                        if (Val.ToString(worksheet.Cells[IntI, PolUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[IntI, PolColumn, IntI, PolColumn].Style.Fill.BackgroundColor.SetColor(lblUp);
                            worksheet.Cells[IntI, PolColumn, IntI, PolColumn].Style.Font.Color.SetColor(BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[IntI, PolUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[IntI, PolColumn, IntI, PolColumn].Style.Fill.BackgroundColor.SetColor(lblDown);
                            worksheet.Cells[IntI, PolColumn, IntI, PolColumn].Style.Font.Color.SetColor(BackColor);
                        }
                        else
                        {
                            worksheet.Cells[IntI, PolColumn, IntI, PolColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }

                        if (Val.ToString(worksheet.Cells[IntI, SymUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[IntI, SymColumn, IntI, SymColumn].Style.Fill.BackgroundColor.SetColor(lblUp);
                            worksheet.Cells[IntI, SymColumn, IntI, SymColumn].Style.Font.Color.SetColor(BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[IntI, SymUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[IntI, SymColumn, IntI, SymColumn].Style.Fill.BackgroundColor.SetColor(lblDown);
                            worksheet.Cells[IntI, SymColumn, IntI, SymColumn].Style.Font.Color.SetColor(BackColor);
                        }
                        else
                        {
                            worksheet.Cells[IntI, SymColumn, IntI, SymColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }

                        if (Val.ToString(worksheet.Cells[IntI, FlrUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[IntI, FlrColumn, IntI, FlrColumn].Style.Fill.BackgroundColor.SetColor(lblUp);
                            worksheet.Cells[IntI, FlrColumn, IntI, FlrColumn].Style.Font.Color.SetColor(BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[IntI, FlrUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[IntI, FlrColumn, IntI, FlrColumn].Style.Fill.BackgroundColor.SetColor(lblDown);
                            worksheet.Cells[IntI, FlrColumn, IntI, FlrColumn].Style.Font.Color.SetColor(BackColor);
                        }
                        else
                        {
                            worksheet.Cells[IntI, FlrColumn, IntI, FlrColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }

                        if (Val.ToString(worksheet.Cells[IntI, RapUpdownColumn].Value) == "UP")
                        {
                            worksheet.Cells[IntI, RapColumn, IntI, RapColumn].Style.Fill.BackgroundColor.SetColor(lblUp);
                            worksheet.Cells[IntI, RapColumn, IntI, RapColumn].Style.Font.Color.SetColor(BackColor);
                        }
                        else if (Val.ToString(worksheet.Cells[IntI, RapUpdownColumn].Value) == "DOWN")
                        {
                            worksheet.Cells[IntI, RapColumn, IntI, RapColumn].Style.Fill.BackgroundColor.SetColor(lblDown);
                            worksheet.Cells[IntI, RapColumn, IntI, RapColumn].Style.Font.Color.SetColor(BackColor);
                        }
                        else
                        {
                            worksheet.Cells[IntI, RapColumn, IntI, RapColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                        }
                    }

                    string CaratsCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Carats"].Ordinal + 1);
                    string SuratRateCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Surat Rate"].Ordinal + 1);
                    string SuratValueCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Surat Value"].Ordinal + 1);
                    string CostRateCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Cost Rate"].Ordinal + 1);
                    string CostValueCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Cost Value"].Ordinal + 1);
                    string AvgRateCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Avg Rate"].Ordinal + 1);
                    string AvgValueCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Avg Value"].Ordinal + 1);
                    string LiveRateCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Live Rate"].Ordinal + 1);
                    string LiveValueCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Live Value"].Ordinal + 1);
                    string LiveDiscount = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Live Disc%"].Ordinal + 1);
                    string RxW = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["RxW"].Ordinal + 1);

                    string OrgCaratsCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Org.Carat"].Ordinal + 1);
                    string GIAChargeCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["GIA Charge"].Ordinal + 1);
                    string ChargesAmtCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Charges Amt"].Ordinal + 1);
                    string AddLess1AmtCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["AddLess1Amt"].Ordinal + 1);

                    worksheet.Cells[IntTotRow, StartColumn, IntTotRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[IntTotRow + 1, StartColumn, IntTotRow + 1, EndColumn].Style.Font.Bold = true;
                    worksheet.Cells[IntTotRow + 1, StartRow, IntTotRow + 1, StartRow].Value = "SUMMARY";
                    worksheet.Cells[IntTotRow + 1, ShapeColumn, IntTotRow + 1, ShapeColumn].Formula = "SUBTOTAL(2," + CaratsCol + (StartRow + 1) + ":" + CaratsCol + IntTotRow + ")";
                    worksheet.Cells[IntTotRow + 1, CaratsColumn, IntTotRow + 1, CaratsColumn].Formula = "ROUND(SUBTOTAL(9," + CaratsCol + (StartRow + 1) + ":" + CaratsCol + IntTotRow + "),2)";
                    worksheet.Cells[IntTotRow + 1, OrgCaratColumn, IntTotRow + 1, OrgCaratColumn].Formula = "ROUND(SUBTOTAL(9," + OrgCaratsCol + (StartRow + 1) + ":" + OrgCaratsCol + IntTotRow + "),2)";
                    worksheet.Cells[IntTotRow + 1, SuartRateColumn, IntTotRow + 1, SuartRateColumn].Formula = "ROUND(" + SuratValueCol + (IntTotRow + 1) + "/" + CaratsCol + (IntTotRow + 1) + ",0)";
                    worksheet.Cells[IntTotRow + 1, SuratValueColumn, IntTotRow + 1, SuratValueColumn].Formula = "ROUND(SUBTOTAL(9," + SuratValueCol + (StartRow + 1) + ":" + SuratValueCol + IntTotRow + "),2)";
                    worksheet.Cells[IntTotRow + 1, CostRateColumn, IntTotRow + 1, CostRateColumn].Formula = "ROUND(" + CostValueCol + (IntTotRow + 1) + "/" + CaratsCol + (IntTotRow + 1) + ",0)";
                    worksheet.Cells[IntTotRow + 1, CostValueColumn, IntTotRow + 1, CostValueColumn].Formula = "ROUND(SUBTOTAL(9," + CostValueCol + (StartRow + 1) + ":" + CostValueCol + IntTotRow + "),2)";
                    worksheet.Cells[IntTotRow + 1, AvgRateColumn, IntTotRow + 1, AvgRateColumn].Formula = "ROUND(" + AvgValueCol + (IntTotRow + 1) + "/" + CaratsCol + (IntTotRow + 1) + ",0)";
                    worksheet.Cells[IntTotRow + 1, AvgValueColumn, IntTotRow + 1, AvgValueColumn].Formula = "ROUND(SUBTOTAL(9," + AvgValueCol + (StartRow + 1) + ":" + AvgValueCol + IntTotRow + "),2)";
                    worksheet.Cells[IntTotRow + 1, LiveRateColumn, IntTotRow + 1, LiveRateColumn].Formula = "ROUND(" + LiveValueCol + (IntTotRow + 1) + "/" + CaratsCol + (IntTotRow + 1) + ",0)";
                    worksheet.Cells[IntTotRow + 1, LiveValueColumn, IntTotRow + 1, LiveValueColumn].Formula = "ROUND(SUBTOTAL(9," + LiveValueCol + (StartRow + 1) + ":" + LiveValueCol + IntTotRow + "),2)";
                    worksheet.Cells[IntTotRow + 1, LiveDiscountColumn, IntTotRow + 1, LiveDiscountColumn].Formula = "ROUND((" + LiveValueCol + (IntTotRow + 1) + "/" + RxW + (IntTotRow + 1) + "-1 ) * 100,2)";
                    worksheet.Cells[IntTotRow + 1, RxWColumn, IntTotRow + 1, RxWColumn].Formula = "SUBTOTAL(9," + RxW + (StartRow + 1) + ":" + RxW + IntTotRow + ")";

                    worksheet.Cells[IntTotRow + 1, GIAChargeColumn, IntTotRow + 1, GIAChargeColumn].Formula = "ROUND(SUBTOTAL(9," + GIAChargeCol + (StartRow + 1) + ":" + GIAChargeCol + IntTotRow + "),2)";
                    worksheet.Cells[IntTotRow + 1, ChargesAmtColumn, IntTotRow + 1, ChargesAmtColumn].Formula = "ROUND(SUBTOTAL(9," + ChargesAmtCol + (StartRow + 1) + ":" + ChargesAmtCol + IntTotRow + "),2)";
                    worksheet.Cells[IntTotRow + 1, AddLess1AmtColumn, IntTotRow + 1, AddLess1AmtColumn].Formula = "ROUND(SUBTOTAL(9," + AddLess1AmtCol + (StartRow + 1) + ":" + AddLess1AmtCol + IntTotRow + "),2)";

                    worksheet.Cells[StartRow, OrgCaratColumn, EndRow, OrgCaratColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, GIAChargeColumn, EndRow, GIAChargeColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, AddLess1Column, EndRow, AddLess1Column].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, AddLess2Column, EndRow, AddLess2Column].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, ChargesAmtColumn, EndRow, ChargesAmtColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, AddLess1AmtColumn, EndRow, AddLess1AmtColumn].Style.Numberformat.Format = "0.00";

                    worksheet.Cells[StartRow, CaratsColumn, EndRow, CaratsColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, SuratValueColumn, EndRow, SuratValueColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, CostValueColumn, EndRow, CostValueColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, AvgValueColumn, EndRow, AvgValueColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, LiveValueColumn, EndRow, LiveValueColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, LiveDiscountColumn, EndRow, LiveDiscountColumn].Style.Numberformat.Format = "0.00";



                    worksheet.Cells[1, 1, 100, 100].AutoFitColumns();

                    worksheet.Column(ColorUpdownColumn).Hidden = true;
                    worksheet.Column(ClarityUpdownColumn).Hidden = true;
                    worksheet.Column(CutUpdownColumn).Hidden = true;
                    worksheet.Column(PolUpdownColumn).Hidden = true;
                    worksheet.Column(SymUpdownColumn).Hidden = true;
                    worksheet.Column(FlrUpdownColumn).Hidden = true;

                    worksheet.Column(RapUpdownColumn).Hidden = true;
                    worksheet.Column(GIAChargeColumn).Hidden = true;
                    worksheet.Column(AddLess1Column).Hidden = true;
                    worksheet.Column(AddLess2Column).Hidden = true;
                    worksheet.Column(ChargesAmtColumn).Hidden = true;
                    worksheet.Column(AddLess1AmtColumn).Hidden = true;

                    //Kapan Detail
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, EndRowMemo, EndColumnMemo].LoadFromDataTable(DtabMemo, true);
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, EndRowMemo, EndColumnMemo].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, EndRowMemo, EndColumnMemo].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, EndRowMemo, EndColumnMemo].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, EndRowMemo, EndColumnMemo].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, EndRowMemo, EndColumnMemo].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, EndRowMemo, EndColumnMemo].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, EndRowMemo, EndColumnMemo].Style.Font.Name = FontName;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, EndRowMemo, EndColumnMemo].Style.Font.Size = FontSize;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, StartRowMemo, EndColumnMemo].Style.Font.Bold = true;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, StartRowMemo, EndColumnMemo].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, StartRowMemo, EndColumnMemo].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, StartRowMemo, EndColumnMemo].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, StartRowMemo, EndColumnMemo].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheetMemo.Cells[StartRowMemo, 1, StartRowMemo, EndColumnMemo].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetMemo.Cells[StartRowMemo, 1, StartRowMemo, EndColumnMemo].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheetMemo.Cells[StartRowMemo, 1, StartRowMemo, EndColumnMemo].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheetMemo.Cells[StartRowMemo, 1, StartRowMemo, EndColumnMemo].Style.Font.Color.SetColor(FontColor);

                    worksheetMemo.Cells[StartRowMemo, StartColumnMemo, StartRowMemo, EndColumnMemo].Style.Fill.BackgroundColor.SetColor(Color.Yellow);


                    int IntTotRowMemo = DtabMemo.Rows.Count + 1;
                    int CaratColumn = DtabMemo.Columns["CARAT"].Ordinal + 1;
                    int PcsColumn = DtabMemo.Columns["PCS"].Ordinal + 1;
                    string CaratCol = Global.ColumnIndexToColumnLetter(DtabMemo.Columns["CARAT"].Ordinal + 1);
                    string PcsCol = Global.ColumnIndexToColumnLetter(DtabMemo.Columns["PCS"].Ordinal + 1);

                    worksheetMemo.Cells[IntTotRowMemo, StartColumnMemo, IntTotRowMemo, EndColumnMemo].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetMemo.Cells[IntTotRowMemo + 1, StartColumnMemo, IntTotRowMemo + 1, EndColumnMemo].Style.Font.Bold = true;
                    worksheetMemo.Cells[IntTotRowMemo + 1, StartRowMemo, IntTotRowMemo + 1, StartRowMemo].Value = "TOTAL";
                    worksheetMemo.Cells[IntTotRowMemo + 1, CaratColumn, IntTotRowMemo + 1, CaratColumn].Formula = "ROUND(SUBTOTAL(9," + CaratCol + (StartRowMemo + 1) + ":" + CaratCol + IntTotRowMemo + "),2)";
                    worksheetMemo.Cells[IntTotRowMemo + 1, PcsColumn, IntTotRowMemo + 1, PcsColumn].Formula = "SUBTOTAL(9," + PcsCol + (StartRowMemo + 1) + ":" + PcsCol + IntTotRowMemo + ")";

                    worksheetMemo.Cells[1, 1, 100, 100].AutoFitColumns();
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


        private string ExportExcelWithCostPrice(DataSet DS, string PStrFilePath)//Add Gunjan :03/03/2023
        {
            try
            {

                DataTable DTabDetail = DS.Tables[0];

                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }

                DTabDetail.DefaultView.Sort = "SrNo";
                DTabDetail = DTabDetail.DefaultView.ToTable();

                this.Cursor = Cursors.WaitCursor;

                //string StrFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";
                string StrFilePath = PStrFilePath;

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.Yellow;
                Color FontColor = Color.Black;
                string FontName = "Calibri";
                float FontSize = 11;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("ST Cost");

                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    #region Stock Detail

                    EndRow = StartRow + DTabDetail.Rows.Count;

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
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(FontColor);

                    int IntTotRow = DTabDetail.Rows.Count + 1;

                    int ShapeColumn = DTabDetail.Columns["Shape"].Ordinal + 1;
                    int CaratsColumn = DTabDetail.Columns["Carats"].Ordinal + 1;
                    int SuartRateColumn = DTabDetail.Columns["Surat Rate"].Ordinal + 1;
                    int SuratValueColumn = DTabDetail.Columns["Surat Value"].Ordinal + 1;
                    int RxWColumn = DTabDetail.Columns["RxW"].Ordinal + 1;
                    int CostDiscountColumn = DTabDetail.Columns["Cost Sys.Disc %"].Ordinal + 1;
                    int CostRateColumn = DTabDetail.Columns["Cost Sys.Rate"].Ordinal + 1;
                    int CostValueColumn = DTabDetail.Columns["Cost Sys.Value"].Ordinal + 1;

                    int LiveDiscountColumn = DTabDetail.Columns["LiveCo.Disc"].Ordinal + 1;//Added By Gunjan:14-10-2023
                    int LiveRateColumn = DTabDetail.Columns["LiveCo.Rate"].Ordinal + 1;
                    int LiveAmtColumn = DTabDetail.Columns["LiveCo.Amt"].Ordinal + 1;
                    int LiveRapColumn = DTabDetail.Columns["LiveCo.Rap"].Ordinal + 1;//End As Gunjan 

                    int SuratRapColumn = DTabDetail.Columns["Surat Rap"].Ordinal + 1;
                    int CostRapColumn = DTabDetail.Columns["Cost Sys.Rap"].Ordinal + 1;
                    int VideoColumn = DTabDetail.Columns["Video"].Ordinal + 1;
                    int VideoLinkColumn = DTabDetail.Columns["Video Link"].Ordinal + 1;
                    int SuratBackColumn = DTabDetail.Columns["Surat Back"].Ordinal + 1;

                    for (int IntI = 2; IntI <= IntTotRow; IntI++)
                    {
                        string SuartRap = Global.ColumnIndexToColumnLetter(SuratRapColumn) + IntI.ToString();
                        string SuratRate = Global.ColumnIndexToColumnLetter(SuartRateColumn) + IntI.ToString();
                        string SuratBack = Global.ColumnIndexToColumnLetter(SuratBackColumn) + IntI.ToString();
                        string Carat = Global.ColumnIndexToColumnLetter(CaratsColumn) + IntI.ToString();
                        string CostRap = Global.ColumnIndexToColumnLetter(CostRapColumn) + IntI.ToString();
                        string CostDisc = Global.ColumnIndexToColumnLetter(CostDiscountColumn) + IntI.ToString();
                        string CostRate = Global.ColumnIndexToColumnLetter(CostRateColumn) + IntI.ToString();
                        string VideoLink = Global.ColumnIndexToColumnLetter(VideoLinkColumn) + IntI.ToString();
                        string LiveRap = Global.ColumnIndexToColumnLetter(LiveRapColumn) + IntI.ToString();//Added By Gunjan:14-10-2023
                        string LiveDisc = Global.ColumnIndexToColumnLetter(LiveDiscountColumn) + IntI.ToString();
                        string LiveRate = Global.ColumnIndexToColumnLetter(LiveRateColumn) + IntI.ToString();//End As Gunjan

                        worksheet.Cells[IntI, SuartRateColumn].Formula = "=(" + SuartRap + " + ((" + SuartRap + " * " + SuratBack + ") / 100))";
                        worksheet.Cells[IntI, SuratValueColumn].Formula = "=ROUND(" + SuratRate + " * " + Carat + ",2)";
                        worksheet.Cells[IntI, RxWColumn].Formula = "=ROUND(" + CostRap + " * " + Carat + ",2)";

                        worksheet.Cells[IntI, CostRateColumn].Formula = "=(" + CostRap + " + ((" + CostRap + " * " + CostDisc + ") / 100))";
                        worksheet.Cells[IntI, CostValueColumn].Formula = "=ROUND(" + CostRate + " * " + Carat + ",2)";

                        worksheet.Cells[IntI, LiveRateColumn].Formula = "=(" + LiveRap + " + ((" + LiveRap + " * " + LiveDisc + ") / 100))";//Added By Gunjan:14-10-2023
                        worksheet.Cells[IntI, LiveAmtColumn].Formula = "=ROUND(" + LiveRate + " * " + Carat + ",2)";//Added By Gunjan:14-10-2023

                        if (IntI != 1)
                        {
                            if (worksheet.Cells[IntI, VideoColumn].Value.ToString() == "")
                            {
                                worksheet.Cells[IntI, VideoColumn, IntI, VideoColumn].Value = "N/A";
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Color.SetColor(Color.Red);
                            }
                            else
                            {
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Color.SetColor(Color.Blue);
                                worksheet.Cells[IntI, VideoColumn].Formula = "=HYPERLINK(" + VideoLink + ", \"Image\")";
                            }
                        }
                    }
                    string CaratsCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Carats"].Ordinal + 1);
                    string SuratRateCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Surat Rate"].Ordinal + 1);
                    string SuratValueCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Surat Value"].Ordinal + 1);
                    string RxW = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["RxW"].Ordinal + 1);
                    string CostDiscount = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Cost Sys.Disc %"].Ordinal + 1);
                    string CostRateCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Cost Sys.Rate"].Ordinal + 1);
                    string CostValueCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Cost Sys.Value"].Ordinal + 1);

                    string LiveDiscount = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["LiveCo.Disc"].Ordinal + 1);//Added By Gunjan:14-10-2023
                    string LiveRateCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["LiveCo.Rate"].Ordinal + 1);
                    string LiveValueCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["LiveCo.Amt"].Ordinal + 1);//End As Gunjan

                    worksheet.Cells[IntTotRow, StartColumn, IntTotRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[IntTotRow + 1, StartColumn, IntTotRow + 1, EndColumn].Style.Font.Bold = true;
                    worksheet.Cells[IntTotRow + 1, StartRow, IntTotRow + 1, StartRow].Value = "SUMMARY";
                    worksheet.Cells[IntTotRow + 1, ShapeColumn, IntTotRow + 1, ShapeColumn].Formula = "SUBTOTAL(2," + CaratsCol + (StartRow + 1) + ":" + CaratsCol + IntTotRow + ")";
                    worksheet.Cells[IntTotRow + 1, CaratsColumn, IntTotRow + 1, CaratsColumn].Formula = "ROUND(SUBTOTAL(9," + CaratsCol + (StartRow + 1) + ":" + CaratsCol + IntTotRow + "),2)";
                    worksheet.Cells[IntTotRow + 1, SuartRateColumn, IntTotRow + 1, SuartRateColumn].Formula = "ROUND(" + SuratValueCol + (IntTotRow + 1) + "/" + CaratsCol + (IntTotRow + 1) + ",0)";
                    worksheet.Cells[IntTotRow + 1, SuratValueColumn, IntTotRow + 1, SuratValueColumn].Formula = "ROUND(SUBTOTAL(9," + SuratValueCol + (StartRow + 1) + ":" + SuratValueCol + IntTotRow + "),2)";
                    worksheet.Cells[IntTotRow + 1, RxWColumn, IntTotRow + 1, RxWColumn].Formula = "SUBTOTAL(9," + RxW + (StartRow + 1) + ":" + RxW + IntTotRow + ")";
                    worksheet.Cells[IntTotRow + 1, CostDiscountColumn, IntTotRow + 1, CostDiscountColumn].Formula = "ROUND((" + CostValueCol + (IntTotRow + 1) + "/" + RxW + (IntTotRow + 1) + "-1 ) * 100,2)";
                    worksheet.Cells[IntTotRow + 1, CostRateColumn, IntTotRow + 1, CostRateColumn].Formula = "ROUND(" + CostValueCol + (IntTotRow + 1) + "/" + CaratsCol + (IntTotRow + 1) + ",0)";
                    worksheet.Cells[IntTotRow + 1, CostValueColumn, IntTotRow + 1, CostValueColumn].Formula = "ROUND(SUBTOTAL(9," + CostValueCol + (StartRow + 1) + ":" + CostValueCol + IntTotRow + "),2)";

                    worksheet.Cells[IntTotRow + 1, LiveDiscountColumn, IntTotRow + 1, LiveDiscountColumn].Formula = "ROUND((" + LiveValueCol + (IntTotRow + 1) + "/" + RxW + (IntTotRow + 1) + "-1 ) * 100,2)";//Added By Gunjan:14-10-2023
                    worksheet.Cells[IntTotRow + 1, LiveRateColumn, IntTotRow + 1, LiveRateColumn].Formula = "ROUND(" + LiveValueCol + (IntTotRow + 1) + "/" + CaratsCol + (IntTotRow + 1) + ",0)";
                    worksheet.Cells[IntTotRow + 1, LiveAmtColumn, IntTotRow + 1, LiveAmtColumn].Formula = "ROUND(SUBTOTAL(9," + LiveValueCol + (StartRow + 1) + ":" + LiveValueCol + IntTotRow + "),2)";//End As Gunjan


                    worksheet.Cells[StartRow, CaratsColumn, EndRow, CaratsColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, SuratValueColumn, EndRow, SuratValueColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, CostValueColumn, EndRow, CostValueColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, CostDiscountColumn, EndRow, CostDiscountColumn].Style.Numberformat.Format = "0.00";

                    worksheet.Cells[StartRow, LiveAmtColumn, EndRow, LiveAmtColumn].Style.Numberformat.Format = "0.00";//Added By Gunjan:14-10-2023
                    worksheet.Cells[StartRow, LiveDiscountColumn, EndRow, LiveDiscountColumn].Style.Numberformat.Format = "0.00";//Added By Gunjan:14-10-2023

                    #endregion

                    worksheet.Cells[1, 1, 100, 100].AutoFitColumns();

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




        public string ExportExcelNew()
        {
            try
            {
                string MemoEntryDetailForXML = "";
                DataTable DtInvDetail = GetSelectedRowToTable();

                DtInvDetail = DtInvDetail.DefaultView.ToTable(false, "STOCK_ID");

                DtInvDetail.TableName = "Table";
                MemoEntryDetailForXML = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DtInvDetail.WriteXml(sw);
                    MemoEntryDetailForXML = sw.ToString();
                }

                string WebStatus = "";

                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "FORMAT";
                this.Cursor = Cursors.WaitCursor;
                FrmSearch.mDTab = Global.GetExportFileTemplate();

                FrmSearch.mStrColumnsToHide = "";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                string FormatName = "";
                if (FrmSearch.DRow != null)
                {
                    FormatName = Val.ToString(FrmSearch.DRow["FORMAT"]);
                }
                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;

                if (FormatName == "")
                {
                    Global.Message("PLEASE SELECT ANY OF ONE FORMAT");
                    return "";
                }

                LiveStockProperty LStockProperty = new LiveStockProperty();
               
                this.Cursor = Cursors.WaitCursor;

                DataSet DS = ObjStock.GetDataForExcelExportNew(MemoEntryDetailForXML, WebStatus, "SINGLE", FormatName, LStockProperty);
                this.Cursor = Cursors.Default;

                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = ".xlsx";
                svDialog.Title = "Export to Excel";
                svDialog.FileName = BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";
                svDialog.Filter = "Excel File (*.xlsx)|*.xlsx ";
                string StrFilePath = "";
                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    StrFilePath = svDialog.FileName;
                }

                if (FormatName == "With Exp")
                {
                    string Result = ExportExcelWithExp(DS, StrFilePath);
                    return Result;
                }

                if (FormatName == "Stock List")
                {
                    string Result = ExportExcelWithStockList(DS, StrFilePath);
                    return Result;
                }
                else if (FormatName == "Offer Price Report")
                {
                    string Result = ExportExcelWithOfferPriceFormat(DS, StrFilePath);
                    return Result;
                }
                else if (FormatName == "Revise List")
                {
                    string Result = ExportExcelWithReviseList(DS, StrFilePath);
                    return Result;
                }
                else if (FormatName == "Smart-I")
                {
                    string Result = ExportExcelWithSmartIList(DS, StrFilePath);
                    return Result;
                }
                else if (FormatName == "ST & Cost System Price")//Gunjan : 03/03/2023
                {
                    string Result = ExportExcelWithCostPrice(DS, StrFilePath);
                    return Result;
                }
                else if (FormatName == "With Sale") //Added by Daksha on 5/08/2023
                {
                    string Result = ExportExcelWithSale(DS, StrFilePath);
                    return Result;
                }
                else if (FormatName == "MFG Comparision Report") //Added by Daksha on 01/09/2023
                {
                    string Result = ExportExcelWithMFGComparision(DS, StrFilePath);
                    return Result;
                }
                else if (FormatName == "General") //Added by Daksha on 04/09/2023
                {
                    string Result = ExportExcelWithGeneral(DS, StrFilePath);
                    return Result;
                }
                else if (FormatName == "Proportion File ")//Gunjan : 03/03/2023
                {
                    string Result = ExportExcelWithProportionFile(DS, StrFilePath);
                    return Result;
                }
                else if (FormatName == "IGI")//Gunjan : 03/03/2023
                {
                    string Result = ExportExcelWithIGI(DS, StrFilePath);
                    return Result;
                }
                else if (FormatName == "VDB")//Gunjan : 03/03/2023
                {
                    string Result = ExportExcelWithVDB(DS, StrFilePath);
                    return Result;
                }
                DataTable DTabDetail = DS.Tables[0];
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

                // string StrFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.FromArgb(2, 68, 143);
                //Color BackColor = Color.FromArgb(119, 50, 107);
                Color FontColor = Color.White;
                string FontName = "Calibri";
                float FontSize = 9;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("RJ_Stock_" + DateTime.Now.ToString("ddMMyyyy"));
                    ExcelWorksheet worksheetProportion = xlPackage.Workbook.Worksheets.Add("Proportion");
                    ExcelWorksheet worksheetInclusion = xlPackage.Workbook.Worksheets.Add("Inclusion Detail");


                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    #region Add Image 

                    //Image img = Image.FromFile(Application.StartupPath + "");
                    //OfficeOpenXml.Drawing.ExcelPicture pic = worksheet.Drawings.AddPicture("Logo", img);
                    //pic.SetPosition(2, 23);
                    //pic.SetSize(100, 55);

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
                    int IntCertColumn = DTabDetail.Columns["Cert No"].Ordinal;
                    int IntVideoUrlColumn = DTabDetail.Columns["Video Url"].Ordinal;

                    int RapaportColumn = DTabDetail.Columns["RapRate"].Ordinal + 1;
                    int PricePerCaratColumn = DTabDetail.Columns["Pr/Ct"].Ordinal + 1;
                    int DiscountColumn = DTabDetail.Columns["Rap %"].Ordinal + 1;
                    int CaratColumn = DTabDetail.Columns["Carat"].Ordinal + 1;
                    int AmountColumn = DTabDetail.Columns["Amount"].Ordinal + 1;

                    for (int IntI = 6; IntI <= EndRow; IntI++)
                    {
                        string RapColumns = Global.ColumnIndexToColumnLetter(RapaportColumn) + IntI.ToString();
                        string Discount = Global.ColumnIndexToColumnLetter(DiscountColumn) + IntI.ToString();
                        string Carat = Global.ColumnIndexToColumnLetter(CaratColumn) + IntI.ToString();
                        string PricePerCarat = Global.ColumnIndexToColumnLetter(PricePerCaratColumn) + IntI.ToString();

                        if (Val.ToString(DTabDetail.Rows[IntI - 6]["FANCYCOLOR"]) == "") //Add if condition khushbu 23-09-21 for skip formula in fancy color
                        {
                            worksheet.Cells[IntI, PricePerCaratColumn].Formula = "=ROUND(" + RapColumns + " + ((" + RapColumns + " * " + Discount + ") / 100),2)";
                            worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                        }
                        else
                        {
                            worksheet.Cells[IntI, PricePerCaratColumn].Value = Val.ToString(DTabDetail.Rows[IntI - 6]["PRICEPERCARAT"]);
                            //worksheet.Cells[IntI, AmountColumn].Value = Val.ToString(DTabDetail.Rows[IntI - 6]["AMOUNT"]);
                            worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                        }

                        //if (!Val.ToString(DTabDetail.Rows[IntI - 6]["DNAPAGEURL"]).Trim().Equals(string.Empty))
                        //{
                        //    //worksheet.Cells[IntI, 2, IntI, 2].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["DNAPAGEURL"]));
                        //    worksheet.Cells[IntI, 2, IntI, 2].Style.Font.Name = FontName;
                        //    worksheet.Cells[IntI, 2, IntI, 2].Style.Font.Bold = true;
                        //    worksheet.Cells[IntI, 2, IntI, 2].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                        //    //worksheet.Cells[IntI, 2, IntI, 2].Style.Font.UnderLine = true;
                        //}

                        //#P :  03-09-2020
                        //worksheet.Cells[IntI, 28, IntI, 28].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["CERTURL"]));
                        //worksheet.Cells[IntI, 28, IntI, 28].Style.Font.Name = FontName;
                        //worksheet.Cells[IntI, 28, IntI, 28].Style.Font.Bold = true;
                        //worksheet.Cells[IntI, 28, IntI, 28].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                        //worksheet.Cells[IntI, 28, IntI, 28].Style.Font.UnderLine = true;


                        if (!Val.ToString(DTabDetail.Rows[IntI - 6]["CERT URL"]).Trim().Equals(string.Empty))
                        {
                            worksheet.Cells[IntI, IntCertColumn + 1].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["CERT URL"]));
                            worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.Name = FontName;
                            worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.Bold = true;
                            worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                            worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.UnderLine = true;
                        }


                        if (!Val.ToString(DTabDetail.Rows[IntI - 6]["VIDEO URL"]).Trim().Equals(string.Empty))
                        {
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Value = "Video";
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["VIDEO URL"]));
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
                        string StrHeader = Global.ExportExcelHeader(Val.ToString(worksheet.Cells[5, i].Value), worksheet, i);
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
                    worksheet.Cells[1, 7, 3, 10].Value = "Note : Use filter to select stones and Check your ObjGridSelection Avg Disc and Total amt.";
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

                    worksheet.Cells[2, 12, 2, 12].Formula = "ROUND(SUM(((N2)/((K2*1)))*100),2)-100";
                    worksheet.Cells[3, 12, 3, 12].Formula = "ROUND(SUM(((N3)/((K3*1)))*100),2)-100";


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

                        worksheet.Cells[2, 16, 2, 16].Formula = "ROUND(SUM(((R2)/((O2*1)))*100),2)-100";
                        worksheet.Cells[3, 16, 3, 16].Formula = "ROUND(SUM(((R3)/((O3*1)))*100),2)-100";
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

                        worksheet.Cells[2, 20, 2, 20].Formula = "ROUND(SUM(((V2)/((S2*1)))*100),2)-100";
                        worksheet.Cells[3, 20, 3, 20].Formula = "ROUND(SUM(((V3)/((S3*1)))*100),2)-100";
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

                        worksheet.Cells[2, 20, 2, 20].Formula = "ROUND(SUM(((V2)/((S2*1)))*100),2)-100";
                        worksheet.Cells[3, 20, 3, 20].Formula = "ROUND(SUM(((V3)/((S3*1)))*100),2)-100";
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
                    worksheet.Cells[1, 7, 3, 10].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[1, 7, 3, 10].Style.Fill.BackgroundColor.SetColor(BackColor);



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

                    #region ProporstionProporstion Detail

                    worksheetProportion.Cells[2, 2, 3, 22].Value = "Stock Proportion";
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Font.Name = FontName;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Font.Size = 20;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Font.Bold = true;

                    worksheetProportion.Cells[2, 2, 3, 22].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 22].Merge = true;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheetProportion.Cells[2, 2, 3, 22].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheetProportion.Cells[2, 2, 3, 22].Style.Font.Color.SetColor(FontColor);

                    int NewRow = 6;
                    AddProportionDetail(worksheetProportion, DTabSize, worksheet.Name, 6, 2, "SIZE WISE SUMMARY", "Size", "Size", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabShape, worksheet.Name, 6, 13, "SHAPE WISE SUMMARY", "Shape", "Shape", StrStartRow, StrEndRow, DTabDetail);

                    if (DTabSize.Rows.Count > DTabShape.Rows.Count)
                    {
                        NewRow = NewRow + DTabSize.Rows.Count + 5;
                    }
                    else
                    {
                        NewRow = NewRow + DTabShape.Rows.Count + 5;
                    }

                    AddProportionDetail(worksheetProportion, DTabClarity, worksheet.Name, NewRow, 2, "CLARITY WISE SUMMARY", "Clarity", "ClaGroup", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabColor, worksheet.Name, NewRow, 13, "COLOR WISE SUMMARY", "Color", "ColGroup", StrStartRow, StrEndRow, DTabDetail);


                    if (DTabClarity.Rows.Count > DTabColor.Rows.Count)
                    {
                        NewRow = NewRow + DTabClarity.Rows.Count + 5;
                    }
                    else
                    {
                        NewRow = NewRow + DTabColor.Rows.Count + 5;
                    }

                    AddProportionDetail(worksheetProportion, DTabCut, worksheet.Name, NewRow, 2, "CUT WISE SUMMARY", "Cut", "CutGroup", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabPolish, worksheet.Name, NewRow, 13, "POLISH WISE SUMMARY", "Pol", "PolGroup", StrStartRow, StrEndRow, DTabDetail);


                    if (DTabCut.Rows.Count > DTabPolish.Rows.Count)
                    {
                        NewRow = NewRow + DTabCut.Rows.Count + 5;
                    }
                    else
                    {
                        NewRow = NewRow + DTabPolish.Rows.Count + 5;
                    }

                    AddProportionDetail(worksheetProportion, DTabSym, worksheet.Name, NewRow, 2, "SYM WISE SUMMARY", "Sym", "SymGroup", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabFL, worksheet.Name, NewRow, 13, "FL WISE SUMMARY", "FL", "FLGroup", StrStartRow, StrEndRow, DTabDetail);

                    #endregion

                    //worksheet.Cells[1, 1, 100, 100].AutoFitColumns();

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


        //public string ExportExcelNew()
        //{
        //    try
        //    {
        //        string MemoEntryDetailForXML = "";

        //        DataTable DtInvDetail = GetSelectedRowToTable();

        //        //DataTable DtInvDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);

        //        DtInvDetail = DtInvDetail.DefaultView.ToTable(false, "STOCK_ID");

        //        DtInvDetail.TableName = "Table";
        //        MemoEntryDetailForXML = string.Empty;
        //        using (StringWriter sw = new StringWriter())
        //        {
        //            DtInvDetail.WriteXml(sw);
        //            MemoEntryDetailForXML = sw.ToString();
        //        }

        //        string WebStatus = "";

        //        FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
        //        FrmSearch.mStrSearchField = "FORMAT";
        //        this.Cursor = Cursors.WaitCursor;
        //        FrmSearch.mDTab = Global.GetExportFileTemplate();

        //        FrmSearch.mStrColumnsToHide = "";
        //        this.Cursor = Cursors.Default;
        //        FrmSearch.ShowDialog();
        //        string FormatName = "";
        //        if (FrmSearch.DRow != null)
        //        {
        //            FormatName = Val.ToString(FrmSearch.DRow["FORMAT"]);
        //        }
        //        FrmSearch.Hide();
        //        FrmSearch.Dispose();
        //        FrmSearch = null;

        //        if (FormatName == "")
        //        {
        //            Global.Message("PLEASE SELECT ANY OF ONE FORMAT");
        //            return "";
        //        }

        //        LiveStockProperty LStockProperty = new LiveStockProperty();

        //        this.Cursor = Cursors.WaitCursor;
        //        DataSet DS = ObjStock.GetDataForExcelExportNew(MemoEntryDetailForXML, WebStatus, "SINGLE", FormatName, LStockProperty);
        //        this.Cursor = Cursors.Default;

        //        SaveFileDialog svDialog = new SaveFileDialog();
        //        svDialog.DefaultExt = ".xlsx";
        //        svDialog.Title = "Export to Excel";
        //        svDialog.FileName = BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";
        //        svDialog.Filter = "Excel File (*.xlsx)|*.xlsx ";
        //        string StrFilePath = "";
        //        if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
        //        {
        //            StrFilePath = svDialog.FileName;
        //        }

        //        if (FormatName == "With Exp")
        //        {
        //            string Result = ExportExcelWithExp(DS, StrFilePath);
        //            return Result;
        //        }

        //        if (FormatName == "Stock List")
        //        {

        //            string Result = ExportExcelWithStockList(DS, StrFilePath);
        //            return Result;
        //        }
        //        else if (FormatName == "Offer Price Report")
        //        {
        //            string Result = ExportExcelWithOfferPriceFormat(DS, StrFilePath);
        //            return Result;
        //        }
        //        else if (FormatName == "Revise List")
        //        {
        //            string Result = ExportExcelWithReviseList(DS, StrFilePath);
        //            return Result;
        //        }
        //        else if (FormatName == "Smart-I")
        //        {
        //            string Result = ExportExcelWithSmartIList(DS, StrFilePath);
        //            return Result;
        //        }

        //        DataTable DTabDetail = DS.Tables[0];
        //        DataTable DTabSize = DS.Tables[1];
        //        DataTable DTabShape = DS.Tables[2];
        //        DataTable DTabClarity = DS.Tables[3];
        //        DataTable DTabColor = DS.Tables[4];
        //        DataTable DTabCut = DS.Tables[5];
        //        DataTable DTabPolish = DS.Tables[6];
        //        DataTable DTabSym = DS.Tables[7];
        //        DataTable DTabFL = DS.Tables[8];
        //        DataTable DTabInclusion = DS.Tables[9];


        //        if (DTabDetail.Rows.Count == 0)
        //        {
        //            this.Cursor = Cursors.Default;

        //            Global.Message("NO DATA FOUND FOR EXPORT");
        //            return "";
        //        }

        //        DTabDetail.DefaultView.Sort = "SR";
        //        DTabDetail = DTabDetail.DefaultView.ToTable();

        //        DTabSize.DefaultView.Sort = "FromCarat";
        //        DTabSize = DTabSize.DefaultView.ToTable();

        //        DTabShape.DefaultView.Sort = "SequenceNo";
        //        DTabShape = DTabShape.DefaultView.ToTable();

        //        DTabColor.DefaultView.Sort = "SequenceNo";
        //        DTabColor = DTabColor.DefaultView.ToTable();

        //        DTabClarity.DefaultView.Sort = "SequenceNo";
        //        DTabClarity = DTabClarity.DefaultView.ToTable();

        //        DTabCut.DefaultView.Sort = "SequenceNo";
        //        DTabCut = DTabCut.DefaultView.ToTable();

        //        DTabPolish.DefaultView.Sort = "SequenceNo";
        //        DTabPolish = DTabPolish.DefaultView.ToTable();

        //        DTabSym.DefaultView.Sort = "SequenceNo";
        //        DTabSym = DTabSym.DefaultView.ToTable();

        //        DTabFL.DefaultView.Sort = "SequenceNo";
        //        DTabFL = DTabFL.DefaultView.ToTable();

        //        this.Cursor = Cursors.WaitCursor;

        //        // string StrFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";

        //        if (File.Exists(StrFilePath))
        //        {
        //            File.Delete(StrFilePath);
        //        }

        //        FileInfo workBook = new FileInfo(StrFilePath);
        //        Color BackColor = Color.FromArgb(2, 68, 143);
        //        //Color BackColor = Color.FromArgb(119, 50, 107);
        //        Color FontColor = Color.White;
        //        string FontName = "Calibri";
        //        float FontSize = 9;

        //        int StartRow = 0;
        //        int StartColumn = 0;
        //        int EndRow = 0;
        //        int EndColumn = 0;

        //        using (ExcelPackage xlPackage = new ExcelPackage(workBook))
        //        {
        //            ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("JV_Stock_" + DateTime.Now.ToString("ddMMyyyy"));
        //            ExcelWorksheet worksheetProportion = xlPackage.Workbook.Worksheets.Add("Proportion");
        //            ExcelWorksheet worksheetInclusion = xlPackage.Workbook.Worksheets.Add("Inclusion Detail");


        //            StartRow = 1;
        //            StartColumn = 1;
        //            EndRow = StartRow;
        //            EndColumn = DTabDetail.Columns.Count;

        //            #region Stock Detail

        //            StartRow = 5;
        //            EndRow = StartRow + DTabDetail.Rows.Count;
        //            StartColumn = 1;
        //            EndColumn = DTabDetail.Columns.Count;

        //            worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].LoadFromDataTable(DTabDetail, true);
        //            worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

        //            worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //            worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //            worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //            worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //            worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Name = FontName;
        //            worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Size = FontSize;

        //            worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Font.Bold = true;
        //            //   worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].AutoFilter = true;
        //            worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
        //            worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
        //            worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
        //            worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

        //            worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //            worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
        //            worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
        //            worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(FontColor);

        //            //#P : 06-08-2020
        //            if (FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale")
        //            {
        //                worksheet.Cells[5, 15, 5, 18].Style.Font.Color.SetColor(Color.Red);
        //            }

        //            if (FormatName == "With Rapnet")
        //            {
        //                worksheet.Cells[5, 19, 5, 22].Style.Font.Color.SetColor(FontColor);
        //            }
        //            else if (FormatName == "With Sale")
        //            {
        //                worksheet.Cells[5, 19, 5, 22].Style.Font.Color.SetColor(Color.FromArgb(174, 201, 121));
        //            }
        //            //End : #P : 06-08-2020

        //            worksheet.View.FreezePanes(6, 1);

        //            // Set Hyperlink
        //            int IntCertColumn = DTabDetail.Columns["CertNo"].Ordinal;
        //            int IntVideoUrlColumn = DTabDetail.Columns["VideoUrl"].Ordinal;
        //            int IntStoneDEtailUrlColumn = DTabDetail.Columns["StoneDetailURL"].Ordinal;

        //            int RapaportColumn = DTabDetail.Columns["RapRate"].Ordinal + 1;
        //            int PricePerCaratColumn = DTabDetail.Columns["PricePerCarat"].Ordinal + 1;
        //            int DiscountColumn = DTabDetail.Columns["RapPer"].Ordinal + 1;
        //            int CaratColumn = DTabDetail.Columns["Carat"].Ordinal + 1;
        //            int AmountColumn = DTabDetail.Columns["Amount"].Ordinal + 1;

        //            for (int IntI = 6; IntI <= EndRow; IntI++)
        //            {
        //                string RapColumns = Global.ColumnIndexToColumnLetter(RapaportColumn) + IntI.ToString();
        //                string Discount = Global.ColumnIndexToColumnLetter(DiscountColumn) + IntI.ToString();
        //                string Carat = Global.ColumnIndexToColumnLetter(CaratColumn) + IntI.ToString();
        //                string PricePerCarat = Global.ColumnIndexToColumnLetter(PricePerCaratColumn) + IntI.ToString();

        //                if (Val.ToString(DTabDetail.Rows[IntI - 6]["FANCYCOLOR"]) == "") //Add if condition khushbu 23-09-21 for skip formula in fancy color
        //                {
        //                    worksheet.Cells[IntI, PricePerCaratColumn].Formula = "=ROUND(" + RapColumns + " + ((" + RapColumns + " * " + Discount + ") / 100),2)";
        //                    worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
        //                }
        //                else
        //                {
        //                    worksheet.Cells[IntI, PricePerCaratColumn].Value = Val.ToString(DTabDetail.Rows[IntI - 6]["PRICEPERCARAT"]);
        //                    //worksheet.Cells[IntI, AmountColumn].Value = Val.ToString(DTabDetail.Rows[IntI - 6]["AMOUNT"]);
        //                    worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
        //                }

        //                if (!Val.ToString(DTabDetail.Rows[IntI - 6]["DNAPAGEURL"]).Trim().Equals(string.Empty))
        //                {
        //                    //worksheet.Cells[IntI, 2, IntI, 2].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["DNAPAGEURL"]));
        //                    worksheet.Cells[IntI, 2, IntI, 2].Style.Font.Name = FontName;
        //                    worksheet.Cells[IntI, 2, IntI, 2].Style.Font.Bold = true;
        //                    worksheet.Cells[IntI, 2, IntI, 2].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
        //                    //worksheet.Cells[IntI, 2, IntI, 2].Style.Font.UnderLine = true;
        //                }

        //                if (!Val.ToString(DTabDetail.Rows[IntI - 6]["CERTURL"]).Trim().Equals(string.Empty))
        //                {
        //                    worksheet.Cells[IntI, IntCertColumn + 1].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["CERTURL"]));
        //                    worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.Name = FontName;
        //                    worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.Bold = true;
        //                    worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
        //                    worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.UnderLine = true;
        //                }


        //                if (!Val.ToString(DTabDetail.Rows[IntI - 6]["VIDEOURL"]).Trim().Equals(string.Empty))
        //                {
        //                    worksheet.Cells[IntI, IntVideoUrlColumn + 1].Value = "Video";
        //                    worksheet.Cells[IntI, IntVideoUrlColumn + 1].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["VIDEOURL"]));
        //                    worksheet.Cells[IntI, IntVideoUrlColumn + 1].Style.Font.Name = FontName;
        //                    worksheet.Cells[IntI, IntVideoUrlColumn + 1].Style.Font.Bold = true;
        //                    worksheet.Cells[IntI, IntVideoUrlColumn + 1].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
        //                    worksheet.Cells[IntI, IntVideoUrlColumn + 1].Style.Font.UnderLine = true;
        //                }
        //                //End : #P :  03-09-2020

        //                if (!Val.ToString(DTabDetail.Rows[IntI - 6]["STONEDETAILURL"]).Trim().Equals(string.Empty))
        //                {
        //                    //worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Value = "Detail";
        //                    worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["STONEDETAILURL"]));
        //                    worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Style.Font.Name = FontName;
        //                    worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Style.Font.Bold = true;
        //                    worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
        //                    worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Style.Font.UnderLine = true;
        //                }

        //                //#P : 06-08-2020
        //                if (FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale")
        //                {
        //                    worksheet.Cells[IntI, 15, IntI, 18].Style.Font.Color.SetColor(Color.Red);
        //                }
        //                if (FormatName == "With Rapnet")
        //                {
        //                    worksheet.Cells[IntI, 19, IntI, 22].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
        //                }
        //                else if (FormatName == "With Sale")
        //                {
        //                    worksheet.Cells[IntI, 19, IntI, 22].Style.Font.Color.SetColor(Color.FromArgb(0, 150, 68));
        //                }
        //                //End : #P : 06-08-2020

        //            }

        //            // Header Set
        //            for (int i = 1; i <= DTabDetail.Columns.Count; i++)
        //            {
        //                string StrHeader = Global.ExportExcelHeader(Val.ToString(worksheet.Cells[5, i].Value), worksheet, i);
        //                worksheet.Cells[5, i].Value = StrHeader;

        //            }

        //            int IntRowStartsFrom = 3;
        //            int IntRowEndTo = (DTabDetail.Rows.Count - 1 + IntRowStartsFrom);

        //            //CHECK COLUMN EXISTS IN DATATABLE..
        //            #region :: Check Column Exists In Datatable ::
        //            int SrNo = 0, CaratNo = 0, AmountNo = 0, RapAmountNo = 0, SizeNo = 0, ShapeNo = 0, ColorNo = 0, ClarityNo = 0, CutNo = 0, PolNo = 0, SymNo = 0, FLNo = 0,
        //                ExpAmountNo = 0, ExpRapAmountNo = 0, RapnetAmountNo = 0, RapnetRapAmountNo = 0, InvoiceAmountNo = 0, InvoiceRapAmountNo = 0;


        //            DataColumnCollection columns = DTabDetail.Columns;

        //            if (columns.Contains("SR"))
        //                SrNo = DTabDetail.Columns["SR"].Ordinal + 1;
        //            if (columns.Contains("Size"))
        //                SizeNo = DTabDetail.Columns["Size"].Ordinal + 1;
        //            if (columns.Contains("Carat"))
        //                CaratNo = DTabDetail.Columns["Carat"].Ordinal + 1;
        //            if (columns.Contains("RapValue"))
        //                RapAmountNo = DTabDetail.Columns["RapValue"].Ordinal + 1;
        //            if (columns.Contains("Amount"))
        //                AmountNo = DTabDetail.Columns["Amount"].Ordinal + 1;

        //            //#P : 06-08-2020
        //            if ((FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale") && columns.Contains("ExpRapValue"))
        //                ExpRapAmountNo = DTabDetail.Columns["ExpRapValue"].Ordinal + 1;
        //            if ((FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale") && columns.Contains("ExpAmount"))
        //                ExpAmountNo = DTabDetail.Columns["ExpAmount"].Ordinal + 1;
        //            if (FormatName == "With Rapnet" && columns.Contains("RapnetRapValue"))
        //                RapnetRapAmountNo = DTabDetail.Columns["RapnetRapValue"].Ordinal + 1;
        //            if (FormatName == "With Rapnet" && columns.Contains("RapnetAmount"))
        //                RapnetAmountNo = DTabDetail.Columns["RapnetAmount"].Ordinal + 1;

        //            if (FormatName == "With Sale" && columns.Contains("InvoiceRapValue"))
        //                InvoiceRapAmountNo = DTabDetail.Columns["InvoiceRapValue"].Ordinal + 1;
        //            if (FormatName == "With Sale" && columns.Contains("InvoiceAmount"))
        //                InvoiceAmountNo = DTabDetail.Columns["InvoiceAmount"].Ordinal + 1;

        //            //End : #P : 06-08-2020

        //            if (columns.Contains("Shape"))
        //                ShapeNo = DTabDetail.Columns["Shape"].Ordinal + 1;
        //            if (columns.Contains("Color"))
        //                ColorNo = DTabDetail.Columns["Color"].Ordinal + 1;
        //            if (columns.Contains("Clarity"))
        //                ClarityNo = DTabDetail.Columns["Clarity"].Ordinal + 1;
        //            if (columns.Contains("Cut"))
        //                CutNo = DTabDetail.Columns["Cut"].Ordinal + 1;
        //            if (columns.Contains("Pol"))
        //                PolNo = DTabDetail.Columns["Pol"].Ordinal + 1;
        //            if (columns.Contains("Sym"))
        //                SymNo = DTabDetail.Columns["Sym"].Ordinal + 1;
        //            if (columns.Contains("FL"))
        //                FLNo = DTabDetail.Columns["FL"].Ordinal + 1;

        //            #endregion

        //            string StrStartRow = "6";
        //            string StrEndRow = EndRow.ToString();

        //            #region Top Formula

        //            worksheet.Cells[1, 5, 1, 5].Value = "Pcs";
        //            worksheet.Cells[1, 6, 1, 6].Value = "Carat";
        //            worksheet.Cells[1, 11, 1, 11].Value = "Rap Value";
        //            worksheet.Cells[1, 12, 1, 12].Value = "Rap %";
        //            worksheet.Cells[1, 13, 1, 13].Value = "Pr/Ct";
        //            worksheet.Cells[1, 14, 1, 14].Value = "Amount";

        //            //#P : 06-08-2020
        //            if (FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale")
        //            {
        //                worksheet.Cells[1, 15, 1, 15].Value = "Exp RapValue";
        //                worksheet.Cells[1, 16, 1, 16].Value = "Exp Rap%";
        //                worksheet.Cells[1, 17, 1, 17].Value = "Exp Pr/Ct";
        //                worksheet.Cells[1, 18, 1, 18].Value = "Exp Amount";
        //            }
        //            if (FormatName == "With Rapnet")
        //            {
        //                worksheet.Cells[1, 19, 1, 19].Value = "Rapnet RapValue";
        //                worksheet.Cells[1, 20, 1, 20].Value = "Rapnet Rap%";
        //                worksheet.Cells[1, 21, 1, 21].Value = "Rapnet Pr/Ct";
        //                worksheet.Cells[1, 22, 1, 22].Value = "Rapnet Amount";
        //            }
        //            if (FormatName == "With Sale")
        //            {
        //                worksheet.Cells[1, 19, 1, 19].Value = "Sale RapValue";
        //                worksheet.Cells[1, 20, 1, 20].Value = "Sale Rap%";
        //                worksheet.Cells[1, 21, 1, 21].Value = "Sale Pr/Ct";
        //                worksheet.Cells[1, 22, 1, 22].Value = "Sale Amount";
        //            }
        //            //End : #P : 06-08-2020


        //            worksheet.Cells[2, 4, 2, 4].Value = "Total";
        //            worksheet.Cells[3, 4, 3, 4].Value = "Selected";

        //            worksheet.Cells[1, 7, 3, 10].Merge = true;
        //            worksheet.Cells[1, 7, 3, 10].Value = "Note : Use filter to select stones and Check your ObjGridSelection Avg Disc and Total amt.";
        //            worksheet.Cells[1, 7, 3, 10].Style.WrapText = true;

        //            // Total Pcs Formula
        //            string S = Global.ColumnIndexToColumnLetter(SrNo) + StrStartRow;
        //            string E = Global.ColumnIndexToColumnLetter(SrNo) + StrEndRow;
        //            worksheet.Cells[2, 5, 2, 5].Formula = "ROUND(COUNTA(" + S + ":" + E + "),2)";
        //            worksheet.Cells[3, 5, 3, 5].Formula = "ROUND(SUBTOTAL(3," + S + ":" + E + "),2)";

        //            // Total Carat Formula
        //            S = Global.ColumnIndexToColumnLetter(CaratNo) + StrStartRow;
        //            E = Global.ColumnIndexToColumnLetter(CaratNo) + StrEndRow;
        //            worksheet.Cells[2, 6, 2, 6].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
        //            worksheet.Cells[3, 6, 3, 6].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

        //            S = Global.ColumnIndexToColumnLetter(RapAmountNo) + StrStartRow;
        //            E = Global.ColumnIndexToColumnLetter(RapAmountNo) + StrEndRow;
        //            worksheet.Cells[2, 11, 2, 11].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
        //            worksheet.Cells[3, 11, 3, 11].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

        //            // Amount Formula
        //            S = Global.ColumnIndexToColumnLetter(AmountNo) + StrStartRow;
        //            E = Global.ColumnIndexToColumnLetter(AmountNo) + StrEndRow;
        //            worksheet.Cells[2, 14, 2, 14].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
        //            worksheet.Cells[3, 14, 3, 14].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

        //            // Price Per Carat Formula
        //            worksheet.Cells[2, 13, 2, 13].Formula = "ROUND(N2/F2,2)";
        //            worksheet.Cells[3, 13, 3, 13].Formula = "ROUND(N3/F3,2)";

        //            // Discount Formula
        //            S = Global.ColumnIndexToColumnLetter(AmountNo) + StrStartRow;
        //            E = Global.ColumnIndexToColumnLetter(AmountNo) + StrEndRow;

        //            worksheet.Cells[2, 12, 2, 12].Formula = "ROUND(SUM(((N2)/((K2*1)))*100),2)-100";
        //            worksheet.Cells[3, 12, 3, 12].Formula = "ROUND(SUM(((N3)/((K3*1)))*100),2)-100";


        //            #region Exp Summary Detail
        //            if (FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale")
        //            {
        //                //Exp RapValue
        //                S = Global.ColumnIndexToColumnLetter(ExpRapAmountNo) + StrStartRow;
        //                E = Global.ColumnIndexToColumnLetter(ExpRapAmountNo) + StrEndRow;
        //                worksheet.Cells[2, 15, 2, 15].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
        //                worksheet.Cells[3, 15, 3, 15].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

        //                // Exp Amount Formula
        //                S = Global.ColumnIndexToColumnLetter(ExpAmountNo) + StrStartRow;
        //                E = Global.ColumnIndexToColumnLetter(ExpAmountNo) + StrEndRow;
        //                worksheet.Cells[2, 18, 2, 18].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
        //                worksheet.Cells[3, 18, 3, 18].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

        //                // Exp Price Per Carat Formula
        //                worksheet.Cells[2, 17, 2, 17].Formula = "ROUND(R2/F2,2)";
        //                worksheet.Cells[3, 17, 3, 17].Formula = "ROUND(R3/F3,2)";

        //                // Exp Discount Formula (Rap%)
        //                S = Global.ColumnIndexToColumnLetter(ExpAmountNo) + StrStartRow;
        //                E = Global.ColumnIndexToColumnLetter(ExpAmountNo) + StrEndRow;

        //                worksheet.Cells[2, 16, 2, 16].Formula = "ROUND(SUM(((R2)/((O2*1)))*100),2)-100";
        //                worksheet.Cells[3, 16, 3, 16].Formula = "ROUND(SUM(((R3)/((O3*1)))*100),2)-100";
        //            }
        //            #endregion

        //            #region Rapnet Summary Detail
        //            if (FormatName == "With Rapnet")
        //            {
        //                //Exp RapValue
        //                S = Global.ColumnIndexToColumnLetter(RapnetRapAmountNo) + StrStartRow;
        //                E = Global.ColumnIndexToColumnLetter(RapnetRapAmountNo) + StrEndRow;
        //                worksheet.Cells[2, 19, 2, 19].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
        //                worksheet.Cells[3, 19, 3, 19].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

        //                // Exp Amount Formula
        //                S = Global.ColumnIndexToColumnLetter(RapnetAmountNo) + StrStartRow;
        //                E = Global.ColumnIndexToColumnLetter(RapnetAmountNo) + StrEndRow;
        //                worksheet.Cells[2, 22, 2, 22].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
        //                worksheet.Cells[3, 22, 3, 22].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

        //                // Exp Price Per Carat Formula
        //                worksheet.Cells[2, 21, 2, 21].Formula = "ROUND(V2/F2,2)";
        //                worksheet.Cells[3, 21, 3, 21].Formula = "ROUND(V3/F3,2)";

        //                // Exp Discount Formula (Rap%)
        //                S = Global.ColumnIndexToColumnLetter(RapnetAmountNo) + StrStartRow;
        //                E = Global.ColumnIndexToColumnLetter(RapnetAmountNo) + StrEndRow;

        //                worksheet.Cells[2, 20, 2, 20].Formula = "ROUND(SUM(((V2)/((S2*1)))*100),2)-100";
        //                worksheet.Cells[3, 20, 3, 20].Formula = "ROUND(SUM(((V3)/((S3*1)))*100),2)-100";
        //            }
        //            #endregion

        //            #region Invoice(Sale) Summary Detail
        //            if (FormatName == "With Sale")
        //            {
        //                //Exp RapValue
        //                S = Global.ColumnIndexToColumnLetter(InvoiceRapAmountNo) + StrStartRow;
        //                E = Global.ColumnIndexToColumnLetter(InvoiceRapAmountNo) + StrEndRow;
        //                worksheet.Cells[2, 19, 2, 19].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
        //                worksheet.Cells[3, 19, 3, 19].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

        //                // Exp Amount Formula
        //                S = Global.ColumnIndexToColumnLetter(InvoiceAmountNo) + StrStartRow;
        //                E = Global.ColumnIndexToColumnLetter(InvoiceAmountNo) + StrEndRow;
        //                worksheet.Cells[2, 22, 2, 22].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
        //                worksheet.Cells[3, 22, 3, 22].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

        //                // Exp Price Per Carat Formula
        //                worksheet.Cells[2, 21, 2, 21].Formula = "ROUND(V2/F2,2)";
        //                worksheet.Cells[3, 21, 3, 21].Formula = "ROUND(V3/F3,2)";

        //                // Exp Discount Formula (Rap%)
        //                S = Global.ColumnIndexToColumnLetter(InvoiceAmountNo) + StrStartRow;
        //                E = Global.ColumnIndexToColumnLetter(InvoiceAmountNo) + StrEndRow;

        //                worksheet.Cells[2, 20, 2, 20].Formula = "ROUND(SUM(((V2)/((S2*1)))*100),2)-100";
        //                worksheet.Cells[3, 20, 3, 20].Formula = "ROUND(SUM(((V3)/((S3*1)))*100),2)-100";
        //            }
        //            #endregion

        //            if (FormatName == "With Exp") //#P : 06-08-2020
        //            {
        //                worksheet.Cells[1, 4, 4, 18].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //                worksheet.Cells[1, 4, 4, 18].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //                worksheet.Cells[1, 4, 4, 18].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                worksheet.Cells[1, 4, 4, 18].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                worksheet.Cells[1, 4, 4, 18].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                worksheet.Cells[1, 4, 4, 18].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                worksheet.Cells[1, 4, 4, 18].Style.Font.Name = "Calibri";
        //                worksheet.Cells[1, 4, 4, 18].Style.Font.Size = 9;

        //                worksheet.Cells[1, 4, 1, 18].Style.Font.Bold = true;
        //                worksheet.Cells[1, 4, 1, 18].Style.Font.Color.SetColor(Color.White);
        //                worksheet.Cells[1, 4, 1, 18].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                worksheet.Cells[1, 4, 1, 18].Style.Fill.PatternColor.SetColor(BackColor);
        //                worksheet.Cells[1, 4, 1, 18].Style.Fill.BackgroundColor.SetColor(BackColor);

        //                worksheet.Cells[1, 15, 3, 18].Style.Font.Color.SetColor(Color.Red);

        //            }
        //            else if (FormatName == "With Rapnet" || FormatName == "With Sale") //#P : 06-08-2020
        //            {
        //                worksheet.Cells[1, 4, 4, 22].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //                worksheet.Cells[1, 4, 4, 22].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //                worksheet.Cells[1, 4, 4, 22].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                worksheet.Cells[1, 4, 4, 22].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                worksheet.Cells[1, 4, 4, 22].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                worksheet.Cells[1, 4, 4, 22].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                worksheet.Cells[1, 4, 4, 22].Style.Font.Name = "Calibri";
        //                worksheet.Cells[1, 4, 4, 22].Style.Font.Size = 9;

        //                worksheet.Cells[1, 4, 1, 22].Style.Font.Bold = true;
        //                worksheet.Cells[1, 4, 1, 22].Style.Font.Color.SetColor(Color.White);
        //                worksheet.Cells[1, 4, 1, 22].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                worksheet.Cells[1, 4, 1, 22].Style.Fill.PatternColor.SetColor(BackColor);
        //                worksheet.Cells[1, 4, 1, 22].Style.Fill.BackgroundColor.SetColor(BackColor);

        //                worksheet.Cells[1, 15, 3, 18].Style.Font.Color.SetColor(Color.Red);

        //                if (FormatName == "With Sale")
        //                {
        //                    worksheet.Cells[1, 19, 1, 22].Style.Font.Color.SetColor(Color.FromArgb(174, 201, 121)); //Green
        //                    worksheet.Cells[2, 19, 3, 22].Style.Font.Color.SetColor(Color.FromArgb(0, 150, 68)); //Green
        //                }
        //                else
        //                {
        //                    worksheet.Cells[1, 19, 1, 22].Style.Font.Color.SetColor(FontColor); //Blue
        //                    worksheet.Cells[2, 19, 3, 22].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192)); //Blue
        //                }
        //            }
        //            else
        //            {
        //                worksheet.Cells[1, 4, 4, 14].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //                worksheet.Cells[1, 4, 4, 14].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
        //                worksheet.Cells[1, 4, 4, 14].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                worksheet.Cells[1, 4, 4, 14].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                worksheet.Cells[1, 4, 4, 14].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                worksheet.Cells[1, 4, 4, 14].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
        //                worksheet.Cells[1, 4, 4, 14].Style.Font.Name = "Calibri";
        //                worksheet.Cells[1, 4, 4, 14].Style.Font.Size = 9;

        //                worksheet.Cells[1, 4, 1, 14].Style.Font.Bold = true;
        //                worksheet.Cells[1, 4, 1, 14].Style.Font.Color.SetColor(Color.White);
        //                worksheet.Cells[1, 4, 1, 14].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //                worksheet.Cells[1, 4, 1, 14].Style.Fill.PatternColor.SetColor(BackColor);
        //                worksheet.Cells[1, 4, 1, 14].Style.Fill.BackgroundColor.SetColor(BackColor);
        //            }

        //            worksheet.Cells[1, 4, 3, 4].Style.Font.Bold = true;
        //            worksheet.Cells[1, 4, 3, 4].Style.Font.Color.SetColor(Color.White);
        //            worksheet.Cells[1, 4, 3, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //            worksheet.Cells[1, 4, 3, 4].Style.Fill.PatternColor.SetColor(BackColor);
        //            worksheet.Cells[1, 4, 3, 4].Style.Fill.BackgroundColor.SetColor(BackColor);

        //            worksheet.Cells[1, 7, 3, 10].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //            worksheet.Cells[1, 7, 3, 10].Style.Fill.PatternColor.SetColor(BackColor);
        //            worksheet.Cells[1, 7, 3, 10].Style.Fill.BackgroundColor.SetColor(BackColor);



        //            if (FormatName == "With Exp") //#P : 06-08-2020
        //            {
        //                worksheet.Column(11).OutlineLevel = 1;//RapValue
        //                worksheet.Column(11).Collapsed = true;

        //                worksheet.Column(15).OutlineLevel = 1; //ExpRapValue
        //                worksheet.Column(15).Collapsed = true;
        //                worksheet.Column(24).OutlineLevel = 1; //FLShade
        //                worksheet.Column(24).Collapsed = true;                     
        //            }
        //            if (FormatName == "With Rapnet" || FormatName == "With Sale") //#P : 06-08-2020
        //            {
        //                worksheet.Column(11).OutlineLevel = 1;//RapValue
        //                worksheet.Column(11).Collapsed = true;

        //                worksheet.Column(15).OutlineLevel = 1; //ExpRapValue
        //                worksheet.Column(15).Collapsed = true;

        //                worksheet.Column(19).OutlineLevel = 1; //RapnetRapValue/SaleRapValue
        //                worksheet.Column(19).Collapsed = true;

        //                worksheet.Column(28).OutlineLevel = 1; //FLShade
        //                worksheet.Column(28).Collapsed = true;                     
        //            }
        //            else
        //            {
        //                worksheet.Column(11).OutlineLevel = 1;//RapValue
        //                worksheet.Column(11).Collapsed = true;

        //                worksheet.Column(20).OutlineLevel = 1;
        //                worksheet.Column(20).Collapsed = true;
        //            }

        //            #endregion

        //            #endregion

        //            #region Inclusion Detail

        //            AddInclusionDetail(worksheetInclusion, DTabInclusion);

        //            #endregion

        //            #region Proporstion Detail

        //            worksheetProportion.Cells[2, 2, 3, 17].Value = "Stock Proportion";
        //            worksheetProportion.Cells[2, 2, 3, 17].Style.Font.Name = FontName;
        //            worksheetProportion.Cells[2, 2, 3, 17].Style.Font.Size = 20;
        //            worksheetProportion.Cells[2, 2, 3, 17].Style.Font.Bold = true;

        //            worksheetProportion.Cells[2, 2, 3, 17].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
        //            worksheetProportion.Cells[2, 2, 3, 17].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
        //            worksheetProportion.Cells[2, 2, 3, 17].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
        //            worksheetProportion.Cells[2, 2, 3, 17].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
        //            worksheetProportion.Cells[2, 2, 3, 17].Merge = true;
        //            worksheetProportion.Cells[2, 2, 3, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
        //            worksheetProportion.Cells[2, 2, 3, 17].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

        //            worksheetProportion.Cells[2, 2, 3, 17].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
        //            worksheetProportion.Cells[2, 2, 3, 17].Style.Fill.PatternColor.SetColor(BackColor);
        //            worksheetProportion.Cells[2, 2, 3, 17].Style.Fill.BackgroundColor.SetColor(BackColor);
        //            worksheetProportion.Cells[2, 2, 3, 17].Style.Font.Color.SetColor(FontColor);

        //            int NewRow = 6;
        //            AddProportionDetail(worksheetProportion, DTabSize, worksheet.Name, 6, 2, "SIZE WISE SUMMARY", "Size", "Size", StrStartRow, StrEndRow, DTabDetail);

        //            AddProportionDetail(worksheetProportion, DTabShape, worksheet.Name, 6, 11, "SHAPE WISE SUMMARY", "Shape", "Shape", StrStartRow, StrEndRow, DTabDetail);

        //            if (DTabSize.Rows.Count > DTabShape.Rows.Count)
        //            {
        //                NewRow = NewRow + DTabSize.Rows.Count + 5;
        //            }
        //            else
        //            {
        //                NewRow = NewRow + DTabShape.Rows.Count + 5;
        //            }

        //            AddProportionDetail(worksheetProportion, DTabClarity, worksheet.Name, NewRow, 2, "CLARITY WISE SUMMARY", "Clarity", "ClaGroup", StrStartRow, StrEndRow, DTabDetail);

        //            AddProportionDetail(worksheetProportion, DTabColor, worksheet.Name, NewRow, 11, "COLOR WISE SUMMARY", "Color", "ColGroup", StrStartRow, StrEndRow, DTabDetail);


        //            if (DTabClarity.Rows.Count > DTabColor.Rows.Count)
        //            {
        //                NewRow = NewRow + DTabClarity.Rows.Count + 5;
        //            }
        //            else
        //            {
        //                NewRow = NewRow + DTabColor.Rows.Count + 5;
        //            }

        //            AddProportionDetail(worksheetProportion, DTabCut, worksheet.Name, NewRow, 2, "CUT WISE SUMMARY", "Cut", "CutGroup", StrStartRow, StrEndRow, DTabDetail);

        //            AddProportionDetail(worksheetProportion, DTabPolish, worksheet.Name, NewRow, 11, "POLISH WISE SUMMARY", "Pol", "PolGroup", StrStartRow, StrEndRow, DTabDetail);


        //            if (DTabCut.Rows.Count > DTabPolish.Rows.Count)
        //            {
        //                NewRow = NewRow + DTabCut.Rows.Count + 5;
        //            }
        //            else
        //            {
        //                NewRow = NewRow + DTabPolish.Rows.Count + 5;
        //            }

        //            AddProportionDetail(worksheetProportion, DTabSym, worksheet.Name, NewRow, 2, "SYM WISE SUMMARY", "Sym", "SymGroup", StrStartRow, StrEndRow, DTabDetail);

        //            AddProportionDetail(worksheetProportion, DTabFL, worksheet.Name, NewRow, 11, "FL WISE SUMMARY", "FL", "FLGroup", StrStartRow, StrEndRow, DTabDetail);

        //            #endregion

        //            xlPackage.Save();
        //        }

        //        this.Cursor = Cursors.Default;
        //        return StrFilePath;

        //    }
        //    catch (Exception ex)
        //    {
        //        this.Cursor = Cursors.Default;
        //        Global.Message(ex.Message);
        //    }
        //    return "";
        //}

        public void AddInclusionDetail(ExcelWorksheet worksheet, DataTable pDtab)
        {
            Color BackColor = Color.FromArgb(2, 68, 143);
            Color FontColor = Color.White;
            string FontName = "Calibri";
            float FontSize = 9;


            worksheet.Cells[2, 3, 4, 13].Value = "Shree Krishna Export Inclusion Grading";
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

            int[] array = new int[4];

            for (int i = 0; i < DTabDistinct.Rows.Count; i++)
            {
                string Str = Val.ToString(DTabDistinct.Rows[i]["PARATYPECODE"]);
                string StrName = Val.ToString(DTabDistinct.Rows[i]["PARATYPENAME"]);

                DataTable DTab = pDtab.Select("PARATYPECODE='" + Str + "'").CopyToDataTable();

                if (i % 4 == 0)
                {
                    StartColumn = 3;
                    StartRow = IntRow + (i % 4) + (array.Max() == 0 ? 6 : array.Max()) + 2;
                    IntRow = StartRow;
                    array = new int[4];
                }
                else
                {
                    StartRow = IntRow;
                }
                array[i % 4] = DTab.Rows.Count;

                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Value = StrName;
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



        #endregion

        #region SelectionPageEvents

        private void BtnAdd1_Click(object sender, EventArgs e)
        {
            try
            {

                Int32[] selectedRowHandles = GrdDetail.GetSelectedRows();
                //txtAddStone1.Text = string.Empty;
                string Strstones = "";
                for (int i = 0; i < selectedRowHandles.Length; i++)
                {
                    int J = selectedRowHandles[i];
                    Strstones += GrdDetail.GetRowCellValue(J, "PARTYSTOCKNO") + ",";
                }
                if (txtAddStone1.Text.Length != 0)
                {
                    txtAddStone1.Text = txtAddStone1.Text + "," + Strstones.Remove(Strstones.Length - 1);
                }
                else
                {
                    txtAddStone1.Text = Strstones.Remove(Strstones.Length - 1);
                }
                //txtAddStone1.Text = txtAddStone1.Text.Length > 0 ? txtAddStone1.Text.Remove(txtAddStone1.Text.Length - 1) : string.Empty;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void BtnAdd2_Click(object sender, EventArgs e)
        {
            try
            {
                Int32[] selectedRowHandles = GrdDetail.GetSelectedRows();
                //txtAddStone1.Text = string.Empty;
                string Strstones = "";
                for (int i = 0; i < selectedRowHandles.Length; i++)
                {
                    int J = selectedRowHandles[i];
                    Strstones += GrdDetail.GetRowCellValue(J, "PARTYSTOCKNO") + ",";
                }
                if (txtAddStone2.Text.Length != 0)
                {
                    txtAddStone2.Text = txtAddStone2.Text + "," + Strstones.Remove(Strstones.Length - 1);
                }
                else
                {
                    txtAddStone2.Text = Strstones.Remove(Strstones.Length - 1);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void BtnAdd3_Click(object sender, EventArgs e)
        {
            try
            {
                Int32[] selectedRowHandles = GrdDetail.GetSelectedRows();
                //txtAddStone1.Text = string.Empty;
                string Strstones = "";
                for (int i = 0; i < selectedRowHandles.Length; i++)
                {
                    int J = selectedRowHandles[i];
                    Strstones += GrdDetail.GetRowCellValue(J, "PARTYSTOCKNO") + ",";
                }
                if (txtAddStone3.Text.Length != 0)
                {
                    txtAddStone3.Text = txtAddStone3.Text + "," + Strstones.Remove(Strstones.Length - 1);
                }
                else
                {
                    txtAddStone3.Text = Strstones.Remove(Strstones.Length - 1);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void BtnAdd4_Click(object sender, EventArgs e)
        {
            try
            {
                Int32[] selectedRowHandles = GrdDetail.GetSelectedRows();
                //txtAddStone1.Text = string.Empty;
                string Strstones = "";
                for (int i = 0; i < selectedRowHandles.Length; i++)
                {
                    int J = selectedRowHandles[i];
                    Strstones += GrdDetail.GetRowCellValue(J, "PARTYSTOCKNO") + ",";
                }
                if (txtAddStone4.Text.Length != 0)
                {
                    txtAddStone4.Text = txtAddStone4.Text + "," + Strstones.Remove(Strstones.Length - 1);
                }
                else
                {
                    txtAddStone4.Text = Strstones.Remove(Strstones.Length - 1);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void toggleSwitch2_Click(object sender, EventArgs e)
        {
            if (chkOnAndOff != true)
            {
                string StrStockNo = txtAddStone1.Text + "," + txtAddStone2.Text + "," + txtAddStone3.Text + "," + txtAddStone5.Text;
                for (int IntI = 0; IntI < GrdDetail.RowCount; IntI++)
                {
                    DataRow DRow = GrdDetail.GetDataRow(IntI);
                    string StrStone = Val.ToString(DRow["PARTYSTOCKNO"]);
                    if (StrStockNo.Split(',').Contains(StrStone))
                    {
                        GrdDetail.SelectRow(IntI);
                        GrdDetail.SetRowCellValue(IntI, "SELECTED", 1);
                    }
                }
                GrdDetail.Columns["SELECTED"].SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
                chkOnAndOff = true;
            }
            else
            {
                chkOnAndOff = false;
                GrdDetail.ClearSelection();
                for (int IntI = 0; IntI < GrdDetail.RowCount; IntI++)
                {
                    string StrStockNo = txtAddStone1.Text + "," + txtAddStone2.Text + "," + txtAddStone3.Text + "," + txtAddStone5.Text;
                    DataRow DRow = GrdDetail.GetDataRow(IntI);
                    string StrStone = Val.ToString(DRow["PARTYSTOCKNO"]);
                    if (StrStockNo.Split(',').Contains(StrStone))
                    {
                        GrdDetail.SetRowCellValue(IntI, "SELECTED", 0);
                    }
                }
            }
        }

        #endregion

        private void BtnLabIssue_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetSelectedRowToTable();

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                string StrStoneNo = string.Empty;
                string StrStoneNoForAvgPrice = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                        return;
                    }

                    if (!(Val.ToString(DRow["STATUS"]) == "AVAILABLE" || Val.ToString(DRow["STATUS"]) == "OFFLINE" || Val.ToString(DRow["STATUS"]) == "NONE") && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }

                    if (Val.Val(DRow["AVGPRICEPERCARAT"]) == 0 || Val.Val(DRow["AVGAMOUNT"]) == 0)
                    {
                        StrStoneNoForAvgPrice = StrStoneNoForAvgPrice + Val.ToString(DRow["STOCKNO"]) + "\n";
                    }

                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Available Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }
                FrmMemoEntryNew FrmMemoEntryNew = new FrmMemoEntryNew();
                FrmMemoEntryNew.ShowForm(Stock.FrmMemoEntryNew.FORMTYPE.LABISSUE, DtInvDetail, mStrStockType);
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnLabReturn_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetSelectedRowToTable();

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                        return;
                    }
                    if (Val.ToString(DRow["STATUS"]) != "LAB" && Val.ToString(DRow["STATUS"]) != "LAB-RESULT" && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Memo Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }
                if (DtInvDetail.DefaultView.ToTable(true, "BILLPARTY_ID").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                    return;
                }

                FrmMemoEntryNew FrmMemoEntryNew = new FrmMemoEntryNew();
                FrmMemoEntryNew.ShowForm(Stock.FrmMemoEntryNew.FORMTYPE.LABRETURN, DtInvDetail, mStrStockType);
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnMemoIssue_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetSelectedRowToTable();

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                int DiamondTypeCount = DtInvDetail.AsEnumerable().Select(row => row.Field<String>("MAINCATEGORY")).Distinct().Count();
                if (DiamondTypeCount > 1)
                {
                    Global.Message("Please Select Same DiamondType'S Stones...");
                    return;
                }
                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                        return;
                    }
                    if (!(Val.ToString(DRow["STATUS"]) == "AVAILABLE" || Val.ToString(DRow["STATUS"]) == "OFFLINE") && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }               

                FrmMemoEntryNew = new FrmMemoEntryNew();
                FrmMemoEntryNew.ShowForm(Stock.FrmMemoEntryNew.FORMTYPE.MEMOISSUE, DtInvDetail, mStrStockType);
                FrmMemoEntryNew.FormClosing += new FormClosingEventHandler(FrmMemoEntryNew_FormClosing);
                
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnMemoReturn_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetSelectedRowToTable();

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                int DiamondTypeCount = DtInvDetail.AsEnumerable().Select(row => row.Field<String>("MAINCATEGORY")).Distinct().Count();
                if (DiamondTypeCount > 1)
                {
                    Global.Message("Please Select Same DiamondType'S Stones...");
                    return;
                }
                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                        return;
                    }
                    if (Val.ToString(DRow["STATUS"]) != "MEMO" && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Memo Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                if (DtInvDetail.DefaultView.ToTable(true, "BILLPARTY_ID").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                    return;
                }
                FrmMemoEntryNew = new FrmMemoEntryNew();
                FrmMemoEntryNew.ShowForm(Stock.FrmMemoEntryNew.FORMTYPE.MEMORETURN, DtInvDetail, mStrStockType);
                FrmMemoEntryNew.FormClosing += new FormClosingEventHandler(FrmMemoEntryNew_FormClosing);
                DataTable Dtab = FrmMemoEntryNew.DTabMemoStock;

                if (Dtab == null || Dtab.Rows.Count < 1)
                {
                    return;
                }
                foreach (DataRow DRow in Dtab.Rows)
                {
                    string StrQuery = "STOCK_ID = '" + Val.ToString(DRow["STOCK_ID"]) + "'";

                    DataRow[] UDROW = DtabLiveStockDetail.Select(StrQuery);
                    if (UDROW.Length > 0)
                    {
                        UDROW[0]["MEMO_ID"] = DRow["MEMO_ID"];
                        UDROW[0]["MEMODETAIL_ID"] = DRow["MEMODETAIL_ID"];
                        UDROW[0]["STATUS"] = DRow["WEBSTATUS"];
                        UDROW[0]["PROCESS_ID"] = DRow["PROCESS_ID"];
                        UDROW[0]["PROCESSNAME"] = DRow["PROCESSNAME"];
                        UDROW[0]["BILLPARTY_ID"] = DRow["BILLINGPARTY_ID"];
                        UDROW[0]["BILLPARTYNAME"] = DRow["BILLPARTYNAME"];
                    }
                }
                DtabLiveStockDetail.AcceptChanges();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnOrderConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetSelectedRowToTable();

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                int DiamondTypeCount = DtInvDetail.AsEnumerable().Select(row => row.Field<String>("MAINCATEGORY")).Distinct().Count();
                if (DiamondTypeCount > 1)
                {
                    Global.Message("Please Select Same DiamondType'S Stones...");
                    return;
                }
                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                        return;
                    }
                    if (!(
                        Val.ToString(DRow["STATUS"]) == "AVAILABLE" ||
                        Val.ToString(DRow["STATUS"]) == "MEMO" ||
                        Val.ToString(DRow["STATUS"]) == "CONSIGNMENT" ||
                        Val.ToString(DRow["STATUS"]) == "OFFLINE" ||
                        Val.ToString(DRow["STATUS"]) == "HOLD"
                        ) && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select (Available / Memo / Offline / Hold) Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.ORDERCONFIRM, DtInvDetail, mStrStockType);
                FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;

                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnOrderConfirmReturn_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetSelectedRowToTable();

                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                int DiamondTypeCount = DtInvDetail.AsEnumerable().Select(row => row.Field<String>("MAINCATEGORY")).Distinct().Count();
                if (DiamondTypeCount > 1)
                {
                    Global.Message("Please Select Same DiamondType'S Stones...");
                    return;
                }
                if (DtInvDetail.DefaultView.ToTable(true, "BILLPARTY_ID").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                    return;
                }

                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                        return;
                    }
                    if (!(Val.ToString(DRow["STATUS"]) == "SOLD") && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }
                FrmMemoEntryNew FrmMemoEntryNew = new FrmMemoEntryNew();
                FrmMemoEntryNew.ShowForm(Stock.FrmMemoEntryNew.FORMTYPE.ORDERCONFIRMRETURN, DtInvDetail);
                FrmMemoEntryNew.FormClosing += new FormClosingEventHandler(FrmMemoEntryNew_FormClosing);
                DataTable Dtab = FrmMemoEntryNew.DTabMemoStock;

                if (Dtab == null || Dtab.Rows.Count < 1)
                {
                    return;
                }
                foreach (DataRow DRow in Dtab.Rows)
                {
                    string StrQuery = "STOCK_ID = '" + Val.ToString(DRow["STOCK_ID"]) + "'";

                    DataRow[] UDROW = DtabLiveStockDetail.Select(StrQuery);
                    if (UDROW.Length > 0)
                    {
                        UDROW[0]["MEMO_ID"] = DRow["MEMO_ID"];
                        UDROW[0]["MEMODETAIL_ID"] = DRow["MEMODETAIL_ID"];
                        UDROW[0]["STATUS"] = DRow["WEBSTATUS"];
                        UDROW[0]["PROCESS_ID"] = DRow["PROCESS_ID"];
                        UDROW[0]["PROCESSNAME"] = DRow["PROCESSNAME"];
                        UDROW[0]["BILLPARTY_ID"] = DRow["BILLINGPARTY_ID"];
                        UDROW[0]["BILLPARTYNAME"] = DRow["BILLPARTYNAME"];
                    }
                }
                DtabLiveStockDetail.AcceptChanges();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;

                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnSalesDelivery_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetSelectedRowToTable();
                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                int DiamondTypeCount = DtInvDetail.AsEnumerable().Select(row => row.Field<String>("MAINCATEGORY")).Distinct().Count();
                if (DiamondTypeCount > 1)
                {
                    Global.Message("Please Select Same DiamondType'S Stones...");
                    return;
                }
                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                        return;
                    }
                    if (!(
                        Val.ToString(DRow["STATUS"]) == "AVAILABLE" ||
                        Val.ToString(DRow["STATUS"]) == "MEMO" ||
                        Val.ToString(DRow["STATUS"]) == "OFFLINE" ||
                        Val.ToString(DRow["STATUS"]) == "SOLD" ||
                        Val.ToString(DRow["STATUS"]) == "HOLD"
                        ) && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select (SOLD) Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                FrmMemoEntry = new FrmMemoEntry();
                FrmMemoEntry.MdiParent = Global.gMainRef;
                FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.SALEINVOICE, DtInvDetail, mStrStockType);
                FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());

            }
        }

        private void BtnSalesDeliveryReturn_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetSelectedRowToTable();
                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                int DiamondTypeCount = DtInvDetail.AsEnumerable().Select(row => row.Field<String>("MAINCATEGORY")).Distinct().Count();
                if (DiamondTypeCount > 1)
                {
                    Global.Message("Please Select Same DiamondType'S Stones...");
                    return;
                }
                if (DtInvDetail.DefaultView.ToTable(true, "BILLPARTY_ID").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                    return;
                }

                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                        return;
                    }
                    if (Val.ToString(DRow["STATUS"]) != "DELIVERY" && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select (DELIVERY) Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                FrmMemoEntryNew FrmMemoEntryNew = new FrmMemoEntryNew();
                FrmMemoEntryNew.ShowForm(Stock.FrmMemoEntryNew.FORMTYPE.SALESDELIVERYRETURN, DtInvDetail, mStrStockType);
                FrmMemoEntryNew.FormClosing += new FormClosingEventHandler(FrmMemoEntryNew_FormClosing);
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());

            }
        }

        private void BtnConsignmentIssue_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetSelectedRowToTable();
                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                int DiamondTypeCount = DtInvDetail.AsEnumerable().Select(row => row.Field<String>("MAINCATEGORY")).Distinct().Count();
                if (DiamondTypeCount > 1)
                {
                    Global.Message("Please Select Same DiamondType'S Stones...");
                    return;
                }
                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                        return;
                    }
                    if (!(Val.ToString(DRow["STATUS"]) == "AVAILABLE" || Val.ToString(DRow["STATUS"]) == "OFFLINE") && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Available Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                FrmMemoEntryNew FrmMemoEntryNew = new FrmMemoEntryNew();
                FrmMemoEntryNew.ShowForm(Stock.FrmMemoEntryNew.FORMTYPE.CONSIGNMENTISSUE, DtInvDetail, mStrStockType);
                FrmMemoEntryNew.FormClosing += new FormClosingEventHandler(FrmMemoEntryNew_FormClosing);
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnConsignmentReturn_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetSelectedRowToTable();
                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                int DiamondTypeCount = DtInvDetail.AsEnumerable().Select(row => row.Field<String>("MAINCATEGORY")).Distinct().Count();
                if (DiamondTypeCount > 1)
                {
                    Global.Message("Please Select Same DiamondType'S Stones...");
                    return;
                }
                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                        return;
                    }
                    if (Val.ToString(DRow["STATUS"]) != "CONSIGNMENT" && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Memo Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                if (DtInvDetail.DefaultView.ToTable(true, "BILLPARTY_ID").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                    return;
                }

                FrmMemoEntryNew FrmMemoEntryNew = new FrmMemoEntryNew();
                FrmMemoEntryNew.ShowForm(Stock.FrmMemoEntryNew.FORMTYPE.CONSIGNMENTRETURN, DtInvDetail, mStrStockType);
                FrmMemoEntryNew.FormClosing += new FormClosingEventHandler(FrmMemoEntryNew_FormClosing);
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnHold_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetSelectedRowToTable();
                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                int DiamondTypeCount = DtInvDetail.AsEnumerable().Select(row => row.Field<String>("MAINCATEGORY")).Distinct().Count();
                if (DiamondTypeCount > 1)
                {
                    Global.Message("Please Select Same DiamondType'S Stones...");
                    return;
                }
                if (DtInvDetail.DefaultView.ToTable(true, "STATUS").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Status Stone. Please Select Only Single Status");
                    return;
                }
                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                        return;
                    }
                    if (!(Val.ToString(DRow["STATUS"]) == "AVAILABLE" || Val.ToString(DRow["STATUS"]) == "OFFLINE") && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }

                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Available Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                FrmMemoEntryNew = new FrmMemoEntryNew();
                FrmMemoEntryNew.ShowForm(Stock.FrmMemoEntryNew.FORMTYPE.HOLD, DtInvDetail, mStrStockType);
                FrmMemoEntryNew.FormClosing += new FormClosingEventHandler(FrmMemoEntryNew_FormClosing);
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnRelease_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetSelectedRowToTable();
                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                int DiamondTypeCount = DtInvDetail.AsEnumerable().Select(row => row.Field<String>("MAINCATEGORY")).Distinct().Count();
                if (DiamondTypeCount > 1)
                {
                    Global.Message("Please Select Same DiamondType'S Stones...");
                    return;
                }
                if (DtInvDetail.DefaultView.ToTable(true, "STATUS").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Status Stone. Please Select Only Single Status");
                    return;
                }
                if (DtInvDetail.DefaultView.ToTable(true, "BILLPARTY_ID").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                    return;
                }

                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                        return;
                    }
                    if (Val.ToString(DRow["STATUS"]) != "HOLD" && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }

                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Available Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                FrmMemoEntryNew = new FrmMemoEntryNew();
                FrmMemoEntryNew.ShowForm(Stock.FrmMemoEntryNew.FORMTYPE.RELEASE, DtInvDetail, mStrStockType);
                FrmMemoEntryNew.FormClosing += new FormClosingEventHandler(FrmMemoEntryNew_FormClosing);
                DataTable Dtab = FrmMemoEntryNew.DTabMemoStock;

                if (Dtab == null || Dtab.Rows.Count < 1)
                {
                    return;
                }
                foreach (DataRow DRow in Dtab.Rows)
                {
                    string StrQuery = "STOCK_ID = '" + Val.ToString(DRow["STOCK_ID"]) + "'";

                    DataRow[] UDROW = DtabLiveStockDetail.Select(StrQuery);
                    if (UDROW.Length > 0)
                    {
                        UDROW[0]["MEMO_ID"] = DRow["MEMO_ID"];
                        UDROW[0]["MEMODETAIL_ID"] = DRow["MEMODETAIL_ID"];
                        UDROW[0]["STATUS"] = DRow["WEBSTATUS"];
                        UDROW[0]["PROCESS_ID"] = DRow["PROCESS_ID"];
                        UDROW[0]["PROCESSNAME"] = DRow["PROCESSNAME"];
                        UDROW[0]["BILLPARTY_ID"] = DRow["BILLINGPARTY_ID"];
                        UDROW[0]["BILLPARTYNAME"] = DRow["BILLPARTYNAME"];
                    }
                }
                DtabLiveStockDetail.AcceptChanges();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnPurchaseReturn_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetSelectedRowToTable();
                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                if (DtInvDetail.DefaultView.ToTable(true, "STATUS").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Status Stone. Please Select Only Single Status");
                    return;
                }

                if (DtInvDetail.DefaultView.ToTable(true, "BILLPARTY_ID").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                    return;
                }

                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                        return;
                    }
                    if (Val.ToString(DRow["STATUS"]) != "PURCHASE" && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }

                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Available Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                FrmMemoEntryNew FrmMemoEntryNew = new FrmMemoEntryNew();
                FrmMemoEntryNew.ShowForm(Stock.FrmMemoEntryNew.FORMTYPE.PURCHASERETURN, DtInvDetail, mStrStockType);
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnPricing_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtStoneDetail = GetSelectedRowToTable();
                if (DtStoneDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }

                string StrStoneNo = string.Empty;
                string StrStoneNoList = string.Empty;

                foreach (DataRow DRow in DtStoneDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                        return;
                    }
                    if (Val.ToString(DRow["STATUS"]) != "OFFLINE" && Val.ToString(DRow["STATUS"]) != "AVAILABLE" && Val.ToString(DRow["STATUS"]) != "NONE" && Val.ToString(DRow["STATUS"]) != "PURCHASE"
                        && Val.ToString(DRow["STATUS"]) != "LAB-RETURN"
                        && Val.ToString(DRow["STATUS"]) != "FACTGRD" //#P : 25-12-2021 : FctGrd mathi Transfer karine None ma leva mate
                        )
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                    else
                    {
                        StrStoneNoList = StrStoneNoList + Val.ToString(DRow["STOCKNO"]) + ",";
                    }
                }

                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select 'Available/None/Offline/LabReturn' Status Stones\n\nThis Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                if (StrStoneNoList.Length != 0)
                {
                    StrStoneNoList = StrStoneNoList.Substring(0, StrStoneNoList.Length - 1);
                }

                FrmParameterUpdate FrmParameterUpdate = new FrmParameterUpdate();
                FrmParameterUpdate.MdiParent = Global.gMainRef;
                FrmParameterUpdate.Tag = "PriceOrParameterUpdate";
                FrmParameterUpdate.FormClosing += new FormClosingEventHandler(FrmMemoEntryNew_FormClosing);
                FrmParameterUpdate.ShowForm(StrStoneNoList);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnSingleToParcel_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtInvDetail = GetSelectedRowToTable();
                if (DtInvDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                int DiamondTypeCount = DtInvDetail.AsEnumerable().Select(row => row.Field<String>("MAINCATEGORY")).Distinct().Count();
                if (DiamondTypeCount > 1)
                {
                    Global.Message("Please Select Same DiamondType'S Stones...");
                    return;
                }

                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtInvDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                        return;
                    }
                    if ((Val.ToString(DRow["STATUS"]) != "NONE" && Val.ToString(DRow["STATUS"]) != "AVAILABLE") && Val.ToString(DRow["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select Memo Status Stones\n\nThese Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }

                if (DtInvDetail.DefaultView.ToTable(true, "BILLPARTY_ID").Rows.Count > 1)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... You Are Selecting Muliple Party Stones For This Activity. Please Select Only Single Party Stone");
                    return;
                }

                FrmMemoEntryNew FrmMemoEntryNew = new FrmMemoEntryNew();
                FrmMemoEntryNew.ShowForm(Stock.FrmMemoEntryNew.FORMTYPE.GRADEDTOMIX, DtInvDetail, mStrStockType);
                //Added and Comment By Gunjan:16/08/2024
                if (Val.ToString(txtStoneNo.Text) != "")
                {
                    BtnStoneNoRefresh_Click(null, null);
                }
                else
                {
                    BtnRefresh_Click(null, null);
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }
        private void FrmSingleToParcel_FormClosing(object sender, FormClosingEventArgs e)
        {
            BtnRefresh.PerformClick();
        }

        private void BtnAssignToBox_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataTable DtBoxDetail = GetSelectedRowToTable();
                if (DtBoxDetail == null || DtBoxDetail.Rows.Count <= 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                string StrStoneNo = string.Empty;
                foreach (DataRow DRow in DtBoxDetail.Rows)
                {
                    if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                        return;
                    }
                    if (Val.ToString(DRow["STATUS"]) != "AVAILABLE")
                    {
                        StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                    }
                }
                if (StrStoneNo != string.Empty)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("Opps... Kindly Select 'Available/None/Offline/LabReturn' Status Stones\n\nThis Packets Have Another Status\n\n" + StrStoneNo);
                    return;
                }
                FrmAssignBox FrmAssignBox = new FrmAssignBox();
                FrmAssignBox.MdiParent = Global.gMainRef;
                FrmAssignBox.FormClosing += new FormClosingEventHandler(FrmMemoEntryNew_FormClosing);
                FrmAssignBox.ShowForm(DtBoxDetail);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        private void FrmMemoEntryNew_FormClosing(object sender, FormClosingEventArgs e)
        {
            //BtnRefresh.PerformClick();
            //Added and Comment By Gunjan:16/08/2024
            //Comment By Gunjan:11/10/2024
            //if (Val.ToString(txtStoneNo.Text) != "")
            //{
            //    BtnStoneNoRefresh_Click(null, null);
            //}
            //else
            //{
            //    BtnRefresh_Click(null, null);
            //}
            //End As Gunjan
            //BtnRefresh_Click(null, null);
            //End As Gunjan
            try
            {
                if (FrmMemoEntryNew != null)
                {
                    DataTable Dtab = FrmMemoEntryNew.DTabMemoStock;

                    if ( Dtab == null || Dtab.Rows.Count < 1 )
                    {
                        return;
                    }
                    foreach (DataRow DRow in Dtab.Rows)
                    {
                        string StrQuery = "STOCK_ID = '" + Val.ToString(DRow["STOCK_ID"]) + "'";

                        DataRow[] UDROW = DtabLiveStockDetail.Select(StrQuery);
                        if (UDROW.Length > 0)
                        {
                            UDROW[0]["MEMO_ID"] = DRow["MEMO_ID"];
                            UDROW[0]["MEMODETAIL_ID"] = DRow["MEMODETAIL_ID"];
                            UDROW[0]["STATUS"] = DRow["WEBSTATUS"];
                            UDROW[0]["PROCESS_ID"] = DRow["PROCESS_ID"];
                            UDROW[0]["PROCESSNAME"] = DRow["PROCESSNAME"];
                            UDROW[0]["BILLPARTY_ID"] = DRow["BILLINGPARTY_ID"];
                            UDROW[0]["BILLPARTYNAME"] = DRow["BILLPARTYNAME"];
                            UDROW[0]["MEMORAPAPORT"] = DRow["SALERAPAPORT"];
                            UDROW[0]["MEMODISCOUNT"] = DRow["SALEDISCOUNT"];
                            UDROW[0]["MEMOPRICEPERCARAT"] = DRow["SALEPRICEPERCARAT"];
                            UDROW[0]["MEMOAMOUNT"] = DRow["SALEAMOUNT"];
                        }
                    }
                    DtabLiveStockDetail.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        private void FrmMemoEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            //BtnRefresh.PerformClick();
            //Added and Comment By Gunjan:16/08/2024
            //Comment By Gunjan:11/10/2024
            //if (Val.ToString(txtStoneNo.Text) != "")
            //{
            //    BtnStoneNoRefresh_Click(null, null);
            //}
            //else
            //{
            //    BtnRefresh_Click(null, null);
            //}
            //End As Gunjan
            //BtnRefresh_Click(null, null);
            //End As Gunjan
            try
            {
                if (FrmMemoEntry != null)
                {
                    DataTable Dtab = FrmMemoEntry.DTabMemoStock;

                    if (Dtab == null || Dtab.Rows.Count < 1)
                    {
                        return;
                    }
                    foreach (DataRow DRow in Dtab.Rows)
                    {
                        string StrQuery = "STOCK_ID = '" + Val.ToString(DRow["STOCK_ID"]) + "'";

                        DataRow[] UDROW = DtabLiveStockDetail.Select(StrQuery);
                        if (UDROW.Length > 0)
                        {
                            UDROW[0]["MEMO_ID"] = DRow["MEMO_ID"];
                            UDROW[0]["MEMODETAIL_ID"] = DRow["MEMODETAIL_ID"];
                            UDROW[0]["STATUS"] = DRow["WEBSTATUS"];
                            UDROW[0]["PROCESS_ID"] = DRow["PROCESS_ID"];
                            UDROW[0]["PROCESSNAME"] = DRow["PROCESSNAME"];
                            UDROW[0]["BILLPARTY_ID"] = DRow["BILLINGPARTY_ID"];
                            UDROW[0]["BILLPARTYNAME"] = DRow["BILLPARTYNAME"];
                        }
                    }
                    DtabLiveStockDetail.AcceptChanges();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
        private void GrdDetail_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }

                string StrCol = Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "STATUS"));
                string StrStockType = Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "STOCKTYPE"));
                if (StrStockType == "SINGLE")
                {
                    if (StrCol.ToUpper() == "AVAILABLE")
                    {
                        e.Appearance.BackColor = lblAvailable.BackColor;
                        e.Appearance.BackColor2 = lblAvailable.BackColor;
                    }
                    else if (StrCol.ToUpper() == "NONE" || StrCol.ToUpper() == "PURCHASE")
                    {
                        e.Appearance.BackColor = lblNone.BackColor;
                        e.Appearance.BackColor2 = lblNone.BackColor;
                    }
                    else if (StrCol.ToUpper() == "MEMO")
                    {
                        e.Appearance.BackColor = lblMemo.BackColor;
                        e.Appearance.BackColor2 = lblMemo.BackColor;
                    }
                    else if (StrCol.ToUpper() == "HOLD")
                    {
                        e.Appearance.BackColor = lblHold.BackColor;
                        e.Appearance.BackColor2 = lblHold.BackColor;
                    }
                    else if (StrCol.ToUpper() == "OFFLINE")
                    {
                        e.Appearance.BackColor = lblOffline.BackColor;
                        e.Appearance.BackColor2 = lblOffline.BackColor;
                    }
                    else if (StrCol.ToUpper() == "SOLD")
                    {
                        e.Appearance.BackColor = lblSold.BackColor;
                        e.Appearance.BackColor2 = lblSold.BackColor;
                    }
                    else if (StrCol.ToUpper() == "DELIVERY")
                    {
                        e.Appearance.BackColor = lblInvoice.BackColor;
                        e.Appearance.BackColor2 = lblInvoice.BackColor;
                    }
                    else if (StrCol.ToUpper() == "PURCHASE-RETURN")
                    {
                        e.Appearance.BackColor = lblPurchaseReturn.BackColor;
                        e.Appearance.BackColor2 = lblPurchaseReturn.BackColor;
                    }
                    else if (StrCol.ToUpper() == "FACTGRD")
                    {
                        e.Appearance.BackColor = lblConsignmentIssue.BackColor;
                        e.Appearance.BackColor2 = lblConsignmentIssue.BackColor;
                    }
                    else if (StrCol.ToUpper() == "LAB")
                    {
                        e.Appearance.BackColor = lblInLab.BackColor;
                        e.Appearance.BackColor2 = lblInLab.BackColor;
                    }
                    else if (StrCol.ToUpper() == "LAB-RETURN")
                    {
                        e.Appearance.BackColor = lblLabReturn.BackColor;
                        e.Appearance.BackColor2 = lblLabReturn.BackColor;
                    }
                    else if (StrCol.ToUpper() == "LAB-RESULT")
                    {
                        e.Appearance.BackColor = lblLabResult.BackColor;
                        e.Appearance.BackColor2 = lblLabResult.BackColor;
                    }
                    else if (StrCol.ToUpper() == "CONSIGNMENT")
                    {
                        e.Appearance.BackColor = lblConsignmentIssue.BackColor;
                        e.Appearance.BackColor2 = lblConsignmentIssue.BackColor;
                    }
                    else if (StrCol.ToUpper() == "DEPTTRANSFER")
                    {
                        e.Appearance.BackColor = lblDeptTran.BackColor;
                        e.Appearance.BackColor2 = lblDeptTran.BackColor;
                    }
                    else if (StrCol.ToUpper() == "SINGLETOPARCEL")
                    {
                        e.Appearance.BackColor = lblSingleToParcel.BackColor;
                        e.Appearance.BackColor2 = lblSingleToParcel.BackColor;
                    }

                }
                if (StrStockType == "PARCEL")
                {
                    double DouMemoPending = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "MEMOPENDINGCARAT"));

                    if (DouMemoPending != 0)
                    {
                        e.Appearance.BackColor = lblMemo.BackColor;
                        e.Appearance.BackColor2 = lblMemo.BackColor;
                    }

                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void xtraTabPage8_Paint(object sender, PaintEventArgs e)
        {

        }

        private void lblResetCarat_Click(object sender, EventArgs e)
        {

        }

        private void lblResetCarat_Click_1(object sender, EventArgs e)
        {
            DTabSize.Rows.Clear();
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
        }

        private void RbtLongMsg_CheckedChanged(object sender, EventArgs e)
        {
            if (RbtLongMsg.Checked == true )
            {
                RbtShortMsg.Checked = false;
            }
            DataTable DTabMessage = GetSelectedRowToTable();
            if (DTabMessage == null || DTabMessage.Rows.Count <= 0)
            {
                this.Cursor = Cursors.Default;
                Global.Message("Please Select AtLeast One Record From The List.");
                return;
            }

            StringBuilder sbBody = new StringBuilder();          
            if (RbtLongMsg.Checked == true)
            {
                for (int i = 0; i < DTabMessage.Rows.Count; i++)
                {
                    sbBody.Append(Environment.NewLine + "---------------------");
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["PARTYSTOCKNO"] + " | " + DTabMessage.Rows[i]["CARAT"]);
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["SHAPENAME"] + " | " + DTabMessage.Rows[i]["COLORNAME"] + " | " + DTabMessage.Rows[i]["CLARITYNAME"]);
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["CUTNAME"] + " | " + DTabMessage.Rows[i]["POLNAME"] + " | " + DTabMessage.Rows[i]["SYMNAME"] + " | " + DTabMessage.Rows[i]["FLNAME"]);
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["LABNAME"] + "  " + DTabMessage.Rows[i]["LABREPORTNO"] + " | " + DTabMessage.Rows[i]["MEASUREMENT"] );
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["COLORSHADENAME"] + " | " + DTabMessage.Rows[i]["MILKYNAME"] + " | " + DTabMessage.Rows[i]["TABLEBLACKNAME"] + " | " + DTabMessage.Rows[i]["TABLEINCNAME"]);
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["TABLEOPENNAME"] + " | " + DTabMessage.Rows[i]["CROWNOPEN"] + " | " + DTabMessage.Rows[i]["PAVOPEN"]); //+ DTabMessage.Rows[i]["CRANGLE"] + " | " + DTabMessage.Rows[i]["PAVANGLE"]);
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["SALERAPAPORT"] + " | " + DTabMessage.Rows[i]["SALEDISCOUNT"] + " | " + DTabMessage.Rows[i]["SALEPRICEPERCARAT"] + " | " + DTabMessage.Rows[i]["SALEAMOUNT"]);
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["VIDEOURL"] );
                }
                sbBody.Remove(0,1);
                txtMessage.Text = sbBody.ToString();
            }
            else if (RbtShortMsg.Checked == true)
            {
                for (int i = 0; i < DTabMessage.Rows.Count; i++)
                {
                    sbBody.Append(Environment.NewLine + "---------------------");
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["PARTYSTOCKNO"] + "  " + DTabMessage.Rows[i]["CARAT"] + "  " + DTabMessage.Rows[i]["SHAPENAME"] + "  " + DTabMessage.Rows[i]["COLORNAME"] + "  " + DTabMessage.Rows[i]["CLARITYNAME"]);
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["CUTNAME"] + "  " + DTabMessage.Rows[i]["POLNAME"] + "  " + DTabMessage.Rows[i]["SYMNAME"] + "  " + DTabMessage.Rows[i]["FLNAME"]);
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["LABNAME"] + "  " + DTabMessage.Rows[i]["LABREPORTNO"] + "  " + DTabMessage.Rows[i]["COLORSHADENAME"] + "  " + DTabMessage.Rows[i]["MILKYNAME"]);
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["SALERAPAPORT"] + "  " + DTabMessage.Rows[i]["SALEDISCOUNT"] + "  " + DTabMessage.Rows[i]["SALEPRICEPERCARAT"] + "  " + DTabMessage.Rows[i]["SALEAMOUNT"]);
                }
                sbBody.Remove(0, 1);
                txtMessage.Text = sbBody.ToString();
            }
            else
            {
                txtMessage.Text = string.Empty;
            }
            if (Val.ToString(txtMessage.Text) != "")
            {
                Clipboard.SetText(txtMessage.Text);
            }
        }

        private void BtnMessageCopy_Click(object sender, EventArgs e)
        {
            if (Val.ToString(txtMessage.Text) != "")
            {
                Clipboard.SetText(txtMessage.Text);
            }
        }

        private void MainGrdDetail_Click(object sender, EventArgs e)
        {

        }

        private void BtnCaratPopup_Click(object sender, EventArgs e)
        {
            GrpSelectCarat.Visible = true;
        }

        private void simpleButton13_Click(object sender, EventArgs e)
        {
            try
            {
                
                //txtFromCts.Text = "0.00";
                //txtToCts.Text = ((DevExpress.XtraEditors.SimpleButton)sender).Text;
                GrpSelectCarat.Visible = false;
                this.Cursor = Cursors.Default;
            }
            catch (Exception EX)
            {
                this.Cursor = Cursors.WaitCursor;
                Global.MessageError(EX.Message);
                return;
            }
        }

        private void txtMessage_Validated(object sender, EventArgs e)
        {
            BtnMessageCopy_Click(null, null);
        }

        private void txtStoneNo_MouseDown(object sender, MouseEventArgs e)
        {
           
        }

        private void txtStoneNo_Validated(object sender, EventArgs e)
        {
            
        }

        private void txtStoneNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                //if (backgroundWorker1.IsBusy)
                //{
                //    backgroundWorker1.CancelAsync();
                //}

                //pStrStockNo = Val.ToString(txtStoneNo.Text);
                //pStrDiamondType = Val.ToString(CmbDiamondType.SelectedItem);
                //progressPanel1.Visible = true;
                //backgroundWorker1.RunWorkerAsync();
                if (backgroundWorker2.IsBusy)
                {
                    backgroundWorker2.CancelAsync();
                }

                pStrStockNo = Val.ToString(txtStoneNo.Text);
                pStrDiamondType = Val.ToString(CmbDiamondType.SelectedItem);
                progressPanel1.Visible = true;
                backgroundWorker2.RunWorkerAsync();
                txtStoneNo.Select(txtStoneNo.Text.Length, 0);
                isPasteAction = true;
            }
            
        }

        private void GrdDetail_CustomSummaryCalculate(object sender, DevExpress.Data.CustomSummaryEventArgs e)
        {
            try
            {
                if (e.SummaryProcess == CustomSummaryProcess.Start)
                {
                    DouCarat = 0;
                    DouCostRapaport = 0;
                    DouCostRapaportAmt = 0;
                    DouCostDisc = 0;
                    DouCostPricePerCarat = 0;
                    DouCostAmount = 0;

                    DouSaleRapaport = 0;
                    DouSaleRapaportAmt = 0;
                    DouSaleDisc = 0;
                    DouSalePricePerCarat = 0;
                    DouSaleAmount = 0;

                    DouExpRapaport = 0;
                    DouExpRapaportAmt = 0;
                    DouExpDisc = 0;
                    DouExpPricePerCarat = 0;
                    DouExpAmount = 0;

                    DouOfferRapaport = 0;
                    DouOfferRapaportAmt = 0;
                    DouOfferDisc = 0;
                    DouOfferPricePerCarat = 0;
                    DouOfferAmount = 0;

                    DouMemoRapaport = 0;
                    DouMemoRapaportAmt = 0;
                    DouMemoDisc = 0;
                    DouMemoPricePerCarat = 0;
                    DouMemoAmount = 0;

                    DouRapnetRapaport = 0;
                    DouRapnetRapaportAmt = 0;
                    DouRapnetDisc = 0;
                    DouRapnetPricePerCarat = 0;
                    DouRapnetAmount = 0;

                    DouJARapaport = 0;
                    DouJARapaportAmt = 0;
                    DouJADisc = 0;
                    DouJAPricePerCarat = 0;
                    DouJAAmount = 0;

                    DouMFGRapaport = 0;
                    DouMFGRapaportAmt = 0;
                    DouMFGDisc = 0;
                    DouMFGPricePerCarat = 0;
                    DouMFGAmount = 0;
                }
                else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
                {
                    DouCarat = DouCarat + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT"));
                    DouCostAmount = DouCostAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "COSTAMOUNT"));
                    DouCostRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "COSTRAPAPORT"));
                    DouCostPricePerCarat = DouCostAmount / DouCarat;
                    DouCostRapaportAmt = DouCostRapaportAmt + (DouCostRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

                    DouSaleAmount = DouSaleAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "SALEAMOUNT"));
                    DouSaleRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "SALERAPAPORT"));
                    DouSalePricePerCarat = DouSaleAmount / DouCarat;
                    DouSaleRapaportAmt = DouSaleRapaportAmt + (DouSaleRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

                    DouExpAmount = DouExpAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "EXPAMOUNT"));
                    DouExpRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "EXPRAPAPORT"));
                    DouExpPricePerCarat = DouExpAmount / DouCarat;
                    DouExpRapaportAmt = DouExpRapaportAmt + (DouExpRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

                    DouOfferAmount = DouOfferAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "OFFERAMOUNT"));
                    DouOfferRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "OFFERRAPAPORT"));
                    DouOfferPricePerCarat = DouOfferAmount / DouCarat;
                    DouOfferRapaportAmt = DouOfferRapaportAmt + (DouOfferRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

                    DouMemoAmount = DouMemoAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "MEMOAMOUNT"));
                    DouMemoRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "MEMORAPAPORT"));
                    DouMemoPricePerCarat = DouMemoAmount / DouCarat;
                    DouMemoRapaportAmt = DouMemoRapaportAmt + (DouMemoRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

                    DouRapnetAmount = DouRapnetAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "RAPNETAMOUNT"));
                    DouRapnetRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "RAPNETRAPAPORT"));
                    DouRapnetPricePerCarat = DouRapnetAmount / DouCarat;
                    DouRapnetRapaportAmt = DouRapnetRapaportAmt + (DouRapnetRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

                    //Kuldeep 02-11-2020
                    DouJAAmount = DouJAAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "JAMESALLENAMOUNT"));
                    DouJARapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "JAMESALLENRAPAPORT"));
                    DouJAPricePerCarat = DouJAAmount / DouCarat;
                    DouJARapaportAmt = DouJARapaportAmt + (DouJARapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

                    DouMFGAmount = DouMFGAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "MFGAMOUNT"));
                    DouMFGRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "MFGRAPAPORT"));
                    DouMFGPricePerCarat = DouMFGAmount / DouCarat;
                    DouMFGRapaportAmt = DouMFGRapaportAmt + (DouMFGRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));
                }
                else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
                {
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("COSTPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouCostAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("COSTRAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouCostRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("COSTDISCOUNT") == 0)
                    {
                        DouCostRapaport = Math.Round((DouCostRapaportAmt / DouCarat), 2);
                        //DouCostDisc = Math.Round(((DouCostPricePerCarat - DouCostRapaport) / DouCostRapaport * 100), 2);
                        DouCostDisc = Math.Round(((DouCostRapaport - DouCostPricePerCarat) / DouCostRapaport * 100), 2);
                        e.TotalValue = DouCostDisc;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("SALEPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouSaleAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("SALERAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouSaleRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("SALEDISCOUNT") == 0)
                    {
                        DouSaleRapaport = Math.Round(DouSaleRapaportAmt / DouCarat);
                        DouSaleDisc = Math.Round(((DouSalePricePerCarat - DouSaleRapaport) / DouSaleRapaport * 100), 2);
                        //DouSaleDisc = Math.Round(((DouSaleRapaport - DouSalePricePerCarat) / DouSaleRapaport * 100), 2);
                        e.TotalValue = DouSaleDisc;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("EXPPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouExpAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("EXPRAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouExpRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("EXPDISCOUNT") == 0)
                    {
                        DouExpRapaport = Math.Round(DouExpRapaportAmt / DouCarat);
                        //DouSaleDisc = Math.Round(((DouSalePricePerCarat - DouSaleRapaport) / DouSaleRapaport * 100), 2);
                        DouExpDisc = Math.Round(((DouExpRapaport - DouExpPricePerCarat) / DouExpRapaport * 100), 2);
                        e.TotalValue = DouExpDisc;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("OFFERPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouOfferAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("OFFERRAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouOfferRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("OFFERDISCOUNT") == 0)
                    {
                        DouOfferRapaport = Math.Round(DouOfferRapaportAmt / DouCarat);
                        //DouSaleDisc = Math.Round(((DouSalePricePerCarat - DouSaleRapaport) / DouSaleRapaport * 100), 2);
                        DouOfferDisc = Math.Round(((DouOfferRapaport - DouOfferPricePerCarat) / DouOfferRapaport * 100), 2);
                        e.TotalValue = DouOfferDisc;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MEMOPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouMemoAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MEMORAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouMemoRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MEMODISCOUNT") == 0)
                    {
                        DouMemoRapaport = Math.Round(DouMemoRapaportAmt / DouCarat);
                        //DouSaleDisc = Math.Round(((DouSalePricePerCarat - DouSaleRapaport) / DouSaleRapaport * 100), 2);
                        DouMemoDisc = Math.Round(((DouMemoRapaport - DouMemoPricePerCarat) / DouMemoRapaport * 100), 2);
                        e.TotalValue = DouMemoDisc;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("RAPNETPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouRapnetAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("RAPNETRAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouRapnetRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("RAPNETDISCOUNT") == 0)
                    {
                        DouRapnetRapaport = Math.Round(DouRapnetRapaportAmt / DouCarat);
                        DouRapnetDisc = Math.Round(((DouRapnetRapaport - DouRapnetPricePerCarat) / DouRapnetRapaport * 100), 2);
                        e.TotalValue = DouRapnetDisc;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("JAMESALLENPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouJAAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("JAMESALLENRAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouJARapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("JAMESALLENDISCOUNT") == 0)
                    {
                        DouJARapaport = Math.Round(DouJARapaportAmt / DouCarat);
                        DouJADisc = Math.Round(((DouJARapaport - DouJAPricePerCarat) / DouJARapaport * 100), 2);
                        e.TotalValue = DouJADisc;
                    }

                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MFGPRICEPERCARAT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouMFGAmount) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MFGRAPAPORT") == 0)
                    {
                        if (Val.Val(DouCarat) > 0)
                            e.TotalValue = Math.Round(Val.Val(DouMFGRapaportAmt) / Val.Val(DouCarat), 2);
                        else
                            e.TotalValue = 0;
                    }
                    if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MFGDISCOUNT") == 0)
                    {
                        DouMFGRapaport = Math.Round((DouMFGRapaportAmt / DouCarat), 2);
                        //DouCostDisc = Math.Round(((DouCostPricePerCarat - DouCostRapaport) / DouCostRapaport * 100), 2);
                        DouMFGDisc = Math.Round(((DouMFGRapaport - DouMFGPricePerCarat) / DouMFGRapaport * 100), 2);
                        e.TotalValue = DouCostDisc;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void btnExcusive_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            DataTable DtInvDetail = GetSelectedRowToTable();

            if (DtInvDetail.Rows.Count <= 0)
            {
                this.Cursor = Cursors.Default;
                Global.Message("Please Select AtLeast One Record From The List.");
                return;
            }
            int DiamondTypeCount = DtInvDetail.AsEnumerable().Select(row => row.Field<String>("MAINCATEGORY")).Distinct().Count();
            if (DiamondTypeCount > 1)
            {
                Global.Message("Please Select Same DiamondType'S Stones...");
                return;
            }
            if (Global.Confirm("Are you sure to add stones in EXCLUSIVE ?") == DialogResult.No)
            {
                return;
            }

            string StrStoneNo = string.Empty;
            foreach (DataRow DRow in DtInvDetail.Rows)
            {
                if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                    return;
                }
                if (Val.ToString(DRow["STATUS"]) != "AVAILABLE")
                {
                    StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                }
            }

            string AddExclusiveXml = string.Empty;
            DtInvDetail.TableName = "Table";
            bool Isexclusive = true;
            using (StringWriter sw = new StringWriter())
            {
                DtInvDetail.WriteXml(sw);
                AddExclusiveXml = sw.ToString();
            }

            LiveStockProperty Property = new LiveStockProperty();
            Property = new BOTRN_StockUpload().UpdateExclusive(AddExclusiveXml, Isexclusive);

            this.Cursor = Cursors.Default;

            Global.Message(Property.ReturnMessageDesc);

        }

        private void btnRemoveExcusive_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            DataTable DtInvDetail = GetSelectedRowToTable();

            if (DtInvDetail.Rows.Count <= 0)
            {
                this.Cursor = Cursors.Default;
                Global.Message("Please Select AtLeast One Record From The List.");
                return;
            }
            int DiamondTypeCount = DtInvDetail.AsEnumerable().Select(row => row.Field<String>("MAINCATEGORY")).Distinct().Count();
            if (DiamondTypeCount > 1)
            {
                Global.Message("Please Select Same DiamondType'S Stones...");
                return;
            }

            if (Global.Confirm("Are you sure to remove stones from EXCLUSIVE ?") == DialogResult.No)
            {
                return;
            }

            string StrStoneNo = string.Empty;
            foreach (DataRow DRow in DtInvDetail.Rows)
            {
                if (Val.ToBoolean(DRow["ISDEPARTMENTTRANSFER"]) || Val.ToBoolean(DRow["ISSINGLETOPARCEL"]))
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("You Select Department Transfer / Single To Parcel Stock ..");
                    return;
                }
                if (Val.ToString(DRow["STATUS"]) != "AVAILABLE")
                {
                    StrStoneNo = StrStoneNo + Val.ToString(DRow["STOCKNO"]) + " = " + Val.ToString(DRow["STATUS"]) + "\n";
                }
            }

            string AddExclusiveXml = string.Empty;
            DtInvDetail.TableName = "Table";
            bool Isexclusive = false;
            using (StringWriter sw = new StringWriter())
            {
                DtInvDetail.WriteXml(sw);
                AddExclusiveXml = sw.ToString();
            }

            LiveStockProperty Property = new LiveStockProperty();
            Property = new BOTRN_StockUpload().UpdateExclusive(AddExclusiveXml, Isexclusive);

            this.Cursor = Cursors.Default;

            Global.Message(Property.ReturnMessageDesc);

        }

        private void BtnSelectedCts_Click(object sender, EventArgs e)
        {
            GrpSelectCarat.Visible = false;
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toggleSwitch2_Toggled(object sender, EventArgs e)
        {

        }

        private void BtnMaximize_Click(object sender, EventArgs e)
        {
            if (BtnMaximize.Text == "Maximize")
            {
                PanelParameter.Visible = false;
                BtnMaximize.Text = "Minimize";
            }
            else
            {
                PanelParameter.Visible = true;
                BtnMaximize.Text = "Maximize";
            }
        }

        private void lblSaveLayout_Click(object sender, EventArgs e)
        {
            Stream str = new System.IO.MemoryStream();
            GrdDetail.SaveLayoutToStream(str);
            str.Seek(0, System.IO.SeekOrigin.Begin);
            StreamReader reader = new StreamReader(str);
            string text = reader.ReadToEnd();

            int IntRes = new BOTRN_StockUpload().SaveGridLayout(this.Name, GrdDetail.Name, text);
            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Saved");
            }
        }

        private void lblDefaultLayout_Click(object sender, EventArgs e)
        {
            int IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdDetail.Name);
            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Deleted");
            }
        }

        private void FrmSingleLiveStockNew_Load(object sender, EventArgs e)
        {
            string Str = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdDetail.Name);

            if (Str != "")
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                MemoryStream stream = new MemoryStream(byteArray);
                GrdDetail.RestoreLayoutFromStream(stream);
            }

        }

        private void RptCert_Click(object sender, EventArgs e)
        {

        }

        private void BtnSelectedCts_Click_1(object sender, EventArgs e)
        {
            try
            {
                GrpSelectCarat.Visible = false;
                txtFromCts2.Text = string.Empty;
                txtFromCts3.Text = string.Empty;
                txtFromCts4.Text = string.Empty;
                txtFromCts5.Text = string.Empty;
                txtToCts2.Text = string.Empty;
                txtToCts3.Text = string.Empty;
                txtToCts4.Text = string.Empty;
                txtToCts5.Text = string.Empty;

                cPanel5.Size = new System.Drawing.Size(755, 34); // Width = 200, Height = 150
                panel5.Size = new System.Drawing.Size(755, 35); // Width = 200, Height = 150
                panel6.Size = new System.Drawing.Size(755, 35); // Width = 200, Height = 150
                panel7.Size = new System.Drawing.Size(755, 36); // Width = 200, Height = 150

                PanelSizeNew.Size = new System.Drawing.Size(755, 30); // Width = 200, Height = 150
                PanelShape.Size = new System.Drawing.Size(755, 31); // Width = 200, Height = 150
                PanelColor.Size = new System.Drawing.Size(755, 31); // Width = 200, Height = 150
                PanelClarity.Size = new System.Drawing.Size(755, 32); // Width = 200, Height = 150

                this.Cursor = Cursors.Default;
            }
            catch (Exception EX)
            {
                this.Cursor = Cursors.WaitCursor;
                Global.MessageError(EX.Message);
                return;
            }
        }

        private void BtnClearFilter_Click(object sender, EventArgs e)
        {
            try
            {
                CmbDiamondType.SelectedIndex = 0;
                txtStoneNo.Text = string.Empty;
                RemoveSelectedBtn(PanelSizeNew);
                RemoveSelectedBtn(PanelShape);
                RemoveSelectedBtn(PanelColor);
                RemoveSelectedBtn(PanelClarity);
                RemoveSelectedBtn(PanelLab);
                RemoveSelectedBtn(PanelCut);
                RemoveSelectedBtn(PanelSym);
                RemoveSelectedBtn(PanelPol);
                RemoveSelectedBtn(PanelFL);
                RemoveSelectedBtn(PanelDiamondType);
                ListStatus.DeSelectAll();
                ListGetLocation.DeSelectAll();

                txtFromCts1.Text = string.Empty;
                txtToCts1.Text = string.Empty;
                TxtStoneCertiMfgMemo.Text = string.Empty;

                txtFromTabledepth.Text = string.Empty;
                txtToTableDepthPer.Text = string.Empty;

                txtFromTablePer.Text = string.Empty;
                txtToTablePer.Text = string.Empty;

                CmbMilky.DeselectAll();
                CmbCShade.DeselectAll();
                CmbTable.DeselectAll();
                CmbSide.DeselectAll();

                txtFromCts2.Text = string.Empty;
                txtFromCts3.Text = string.Empty;
                txtFromCts4.Text = string.Empty;
                txtFromCts5.Text = string.Empty;
                txtToCts2.Text = string.Empty;
                txtToCts3.Text = string.Empty;
                txtToCts4.Text = string.Empty;
                txtToCts5.Text = string.Empty;

                DTabSize.Rows.Clear();
                DTabSize.Rows.Add(DTabSize.NewRow());
                DTabSize.Rows.Add(DTabSize.NewRow());
                DTabSize.Rows.Add(DTabSize.NewRow());
                DTabSize.Rows.Add(DTabSize.NewRow());
                DTabSize.Rows.Add(DTabSize.NewRow());

            }
            catch (Exception EX)
            {
                this.Cursor = Cursors.WaitCursor;
                Global.MessageError(EX.Message);
                return;
            }
        }

        private void txtStoneNo_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //if (isPasteAction)
                {
                    
                    String str1 = Val.ToString(txtStoneNo.Text);
                    string result = Regex.Replace(str1, @"\r\n?|\n", ",");
                    if (result.EndsWith(",,"))
                    {
                        result = result.Remove(result.Length - 1);
                    }
                    txtStoneNo.Text = result;                   
                }
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

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                mProperty = new LiveStockProperty();
                DataSet DsLiveStock = new DataSet();
               
                mProperty.STOCKNO = Val.ToString(txtStoneNo.Text);

                DsLiveStock = ObjStock.GetMumbaiLivestockData(mProperty, "All");
                DtabLiveStockDetail = DsLiveStock.Tables[0];

                DtabLiveStockDetail.DefaultView.Sort = "SrNo";
                DtabLiveStockDetail = DtabLiveStockDetail.DefaultView.ToTable();

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                MainGrdDetail.DataSource = DtabLiveStockDetail;
                progressPanel1.Visible = false;
                //MainGrdDetail.Refresh();               
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnStoneNoRefresh_Click(object sender, EventArgs e)
        {
            if (backgroundWorker2.IsBusy)
            {
                backgroundWorker2.CancelAsync();
            }
            //txtStoneNo.Text = string.Empty;//Comment by Gunjan:14/03/2024
            pStrStockNo = Val.ToString(txtStoneNo.Text);
            pStrDiamondType = Val.ToString(CmbDiamondType.SelectedItem);
            progressPanel1.Visible = true;
            backgroundWorker2.RunWorkerAsync();
        }

        private void GrdDetail_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Down && e.Control && e.Shift)
                {
                    GridView view = sender as GridView;

                    if (view != null)
                    {
                        int startRowHandle = view.FocusedRowHandle;
                        GridColumn focusedColumn = view.FocusedColumn;

                        // Start selecting from the current focused cell downwards
                        for (int i = startRowHandle; i < view.RowCount; i++)
                        {
                            // Select only the cell in the focused column for each row
                            view.SelectCell(i, focusedColumn);
                        }
                    }
                }
                if (e.KeyCode == Keys.Up && e.Control && e.Shift)
                {
                    GridView view = sender as GridView;

                    if (view != null)
                    {
                        int startRowHandle = view.FocusedRowHandle;
                        GridColumn focusedColumn = view.FocusedColumn;

                        // Start selecting from the current focused cell upwards
                        for (int i = startRowHandle; i >= 0; i--)
                        {
                            // Select only the cell in the focused column for each row
                            view.SelectCell(i, focusedColumn);
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void GrdDetail_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0) // Ignore invalid rows
                {
                    return;
                }

                string StrStatus = Val.ToString(GrdDetail.GetRowCellValue(e.RowHandle, "LOCATIONNAME"));

                if (e.Column.FieldName == "PARTYSTOCKNO")
                {
                    if (StrStatus == "HONG KONG")
                    {
                        e.Appearance.BackColor = Color.LightGray;
                        e.Appearance.BackColor2 = Color.LightGray;

                    }
                    else if (StrStatus == "SURAT")
                    {
                        e.Appearance.BackColor = Color.Thistle;
                        e.Appearance.BackColor2 = Color.Thistle;
                    }
                    else if (StrStatus == "JA")
                    {
                        e.Appearance.BackColor = Color.LightPink;
                        e.Appearance.BackColor2 = Color.LightPink;
                    }
                    else if (StrStatus == "MUMBAI")
                    {                        
                    }
                    else 
                    {
                        e.Appearance.BackColor = Color.FromArgb(192, 192, 255); 
                        e.Appearance.BackColor2 = Color.FromArgb(192, 192, 255);
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnAdd5_Click(object sender, EventArgs e)
        {
            try
            {
                Int32[] selectedRowHandles = GrdDetail.GetSelectedRows();
                //txtAddStone1.Text = string.Empty;
                string Strstones = "";
                for (int i = 0; i < selectedRowHandles.Length; i++)
                {
                    int J = selectedRowHandles[i];
                    Strstones += GrdDetail.GetRowCellValue(J, "PARTYSTOCKNO") + ",";
                }
                if (txtAddStone5.Text.Length != 0)
                {
                    txtAddStone5.Text = txtAddStone5.Text + "," + Strstones.Remove(Strstones.Length - 1);
                }
                else
                {
                    txtAddStone5.Text = Strstones.Remove(Strstones.Length - 1);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void BtnAdd6_Click(object sender, EventArgs e)
        {
            try
            {
                Int32[] selectedRowHandles = GrdDetail.GetSelectedRows();
                //txtAddStone1.Text = string.Empty;
                string Strstones = "";
                for (int i = 0; i < selectedRowHandles.Length; i++)
                {
                    int J = selectedRowHandles[i];
                    Strstones += GrdDetail.GetRowCellValue(J, "PARTYSTOCKNO") + ",";
                }
                if (txtAddStone6.Text.Length != 0)
                {
                    txtAddStone6.Text = txtAddStone6.Text + "," + Strstones.Remove(Strstones.Length - 1);
                }
                else
                {
                    txtAddStone6.Text = Strstones.Remove(Strstones.Length - 1);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void BtnAdd7_Click(object sender, EventArgs e)
        {
            try
            {
                Int32[] selectedRowHandles = GrdDetail.GetSelectedRows();
                //txtAddStone1.Text = string.Empty;
                string Strstones = "";
                for (int i = 0; i < selectedRowHandles.Length; i++)
                {
                    int J = selectedRowHandles[i];
                    Strstones += GrdDetail.GetRowCellValue(J, "PARTYSTOCKNO") + ",";
                }
                if (txtAddStone7.Text.Length != 0)
                {
                    txtAddStone7.Text = txtAddStone7.Text + "," + Strstones.Remove(Strstones.Length - 1);
                }
                else
                {
                    txtAddStone7.Text = Strstones.Remove(Strstones.Length - 1);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void RbtShortMsg_CheckedChanged(object sender, EventArgs e)
        {
            if (RbtShortMsg.Checked == true)
            {
                RbtLongMsg.Checked = false;
            }
            DataTable DTabMessage = GetSelectedRowToTable();
            if (DTabMessage == null || DTabMessage.Rows.Count <= 0)
            {
                this.Cursor = Cursors.Default;
                Global.Message("Please Select AtLeast One Record From The List.");
                return;
            }

            StringBuilder sbBody = new StringBuilder();
            if (RbtLongMsg.Checked == true)
            {
                for (int i = 0; i < DTabMessage.Rows.Count; i++)
                {
                    sbBody.Append(Environment.NewLine + "---------------------");
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["PARTYSTOCKNO"] + " | " + DTabMessage.Rows[i]["CARAT"]);
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["SHAPENAME"] + " | " + DTabMessage.Rows[i]["COLORNAME"] + " | " + DTabMessage.Rows[i]["CLARITYNAME"]);
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["CUTNAME"] + " | " + DTabMessage.Rows[i]["POLNAME"] + " | " + DTabMessage.Rows[i]["SYMNAME"] + " | " + DTabMessage.Rows[i]["FLNAME"]);
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["LABNAME"] + "  " + DTabMessage.Rows[i]["LABREPORTNO"] + " | " + DTabMessage.Rows[i]["MEASUREMENT"]);
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["COLORSHADENAME"] + " | " + DTabMessage.Rows[i]["MILKYNAME"] + " | " + DTabMessage.Rows[i]["TABLEBLACKNAME"] + " | " + DTabMessage.Rows[i]["TABLEINCNAME"]);
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["TABLEOPENNAME"] + " | " + DTabMessage.Rows[i]["CRANGLE"] + " | " + DTabMessage.Rows[i]["PAVANGLE"]);
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["SALERAPAPORT"] + " | " + DTabMessage.Rows[i]["SALEDISCOUNT"] + " | " + DTabMessage.Rows[i]["SALEPRICEPERCARAT"] + " | " + DTabMessage.Rows[i]["SALEAMOUNT"]);
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["VIDEOURL"]);
                }
                sbBody.Remove(0, 1);
                txtMessage.Text = sbBody.ToString();
            }
            else if (RbtShortMsg.Checked == true)
            {
                for (int i = 0; i < DTabMessage.Rows.Count; i++)
                {
                    sbBody.Append(Environment.NewLine + "---------------------");
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["PARTYSTOCKNO"] + "  " + DTabMessage.Rows[i]["CARAT"] + "  " + DTabMessage.Rows[i]["SHAPENAME"] + "  " + DTabMessage.Rows[i]["COLORNAME"] + "  " + DTabMessage.Rows[i]["CLARITYNAME"]);
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["CUTNAME"] + "  " + DTabMessage.Rows[i]["POLNAME"] + "  " + DTabMessage.Rows[i]["SYMNAME"] + "  " + DTabMessage.Rows[i]["FLNAME"]);
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["LABNAME"] + "  " + DTabMessage.Rows[i]["LABREPORTNO"] + "  " + DTabMessage.Rows[i]["COLORSHADENAME"] + "  " + DTabMessage.Rows[i]["MILKYNAME"]);
                    sbBody.Append(Environment.NewLine + DTabMessage.Rows[i]["SALERAPAPORT"] + "  " + DTabMessage.Rows[i]["SALEDISCOUNT"] + "  " + DTabMessage.Rows[i]["SALEPRICEPERCARAT"] + "  " + DTabMessage.Rows[i]["SALEAMOUNT"]);
                }
                sbBody.Remove(0, 1);
                txtMessage.Text = sbBody.ToString();
            }
            else
            {
                txtMessage.Text = string.Empty;
            }
            if (Val.ToString(txtMessage.Text) != "")
            {
                Clipboard.SetText(txtMessage.Text);
            }
        }

        private void FrmSingleLiveStockNew_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.S && e.Control)
                {
                    BtnRefresh_Click(null,null);
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void txtAddStone1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    progressPanel1.Visible = true;
                    progressPanel1.BringToFront();
                    mProperty = new LiveStockProperty();
                    DataSet DsLiveStock = new DataSet();

                    mProperty.STOCKNO = Val.ToString(txtAddStone1.Text);

                    DsLiveStock = ObjStock.GetMumbaiLivestockData(mProperty, "All");
                    DtabLiveStockDetail = DsLiveStock.Tables[0];

                    MainGrdDetail.DataSource = DtabLiveStockDetail;
                    progressPanel1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtAddStone2_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    progressPanel1.Visible = true;
                    mProperty = new LiveStockProperty();
                    DataSet DsLiveStock = new DataSet();

                    mProperty.STOCKNO = Val.ToString(txtAddStone2.Text);

                    DsLiveStock = ObjStock.GetMumbaiLivestockData(mProperty, "All");
                    DtabLiveStockDetail = DsLiveStock.Tables[0];

                    MainGrdDetail.DataSource = DtabLiveStockDetail;
                    progressPanel1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtAddStone3_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    progressPanel1.Visible = true;
                    mProperty = new LiveStockProperty();
                    DataSet DsLiveStock = new DataSet();

                    mProperty.STOCKNO = Val.ToString(txtAddStone3.Text);

                    DsLiveStock = ObjStock.GetMumbaiLivestockData(mProperty, "All");
                    DtabLiveStockDetail = DsLiveStock.Tables[0];

                    MainGrdDetail.DataSource = DtabLiveStockDetail;
                    progressPanel1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtAddStone4_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    progressPanel1.Visible = true;
                    mProperty = new LiveStockProperty();
                    DataSet DsLiveStock = new DataSet();

                    mProperty.STOCKNO = Val.ToString(txtAddStone4.Text);

                    DsLiveStock = ObjStock.GetMumbaiLivestockData(mProperty, "All");
                    DtabLiveStockDetail = DsLiveStock.Tables[0];

                    MainGrdDetail.DataSource = DtabLiveStockDetail;
                    progressPanel1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtAddStone5_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    progressPanel1.Visible = true;
                    mProperty = new LiveStockProperty();
                    DataSet DsLiveStock = new DataSet();

                    mProperty.STOCKNO = Val.ToString(txtAddStone5.Text);

                    DsLiveStock = ObjStock.GetMumbaiLivestockData(mProperty, "All");
                    DtabLiveStockDetail = DsLiveStock.Tables[0];

                    MainGrdDetail.DataSource = DtabLiveStockDetail;
                    progressPanel1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtAddStone6_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    progressPanel1.Visible = true;
                    mProperty = new LiveStockProperty();
                    DataSet DsLiveStock = new DataSet();

                    mProperty.STOCKNO = Val.ToString(txtAddStone6.Text);

                    DsLiveStock = ObjStock.GetMumbaiLivestockData(mProperty, "All");
                    DtabLiveStockDetail = DsLiveStock.Tables[0];

                    MainGrdDetail.DataSource = DtabLiveStockDetail;
                    progressPanel1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtAddStone7_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    progressPanel1.Visible = true;
                    mProperty = new LiveStockProperty();
                    DataSet DsLiveStock = new DataSet();

                    mProperty.STOCKNO = Val.ToString(txtAddStone7.Text);

                    DsLiveStock = ObjStock.GetMumbaiLivestockData(mProperty, "All");
                    DtabLiveStockDetail = DsLiveStock.Tables[0];

                    MainGrdDetail.DataSource = DtabLiveStockDetail;
                    progressPanel1.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtAddStone1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String str1 = Val.ToString(txtAddStone1.Text);
                string result = Regex.Replace(str1, @"\r\n?|\n", ",");
                if (result.EndsWith(",,"))
                {
                    result = result.Remove(result.Length - 1);
                }
                txtAddStone1.Text = result;
                txtAddStone1.Select(txtAddStone1.Text.Length, 0);
            }
            catch (Exception EX)
            {
                Global.MessageError(EX.Message);
                return;
            }
        }

        private void txtAddStone2_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String str1 = Val.ToString(txtAddStone2.Text);
                string result = Regex.Replace(str1, @"\r\n?|\n", ",");
                if (result.EndsWith(",,"))
                {
                    result = result.Remove(result.Length - 1);
                }
                txtAddStone2.Text = result;
                txtAddStone2.Select(txtAddStone2.Text.Length, 0);
            }
            catch (Exception EX)
            {
                Global.MessageError(EX.Message);
                return;
            }
        }

        private void txtAddStone3_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String str1 = Val.ToString(txtAddStone3.Text);
                string result = Regex.Replace(str1, @"\r\n?|\n", ",");
                if (result.EndsWith(",,"))
                {
                    result = result.Remove(result.Length - 1);
                }
                txtAddStone3.Text = result;
                txtAddStone3.Select(txtAddStone3.Text.Length, 0);
            }
            catch (Exception EX)
            {
                Global.MessageError(EX.Message);
                return;
            }
        }

        private void txtAddStone4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String str1 = Val.ToString(txtAddStone4.Text);
                string result = Regex.Replace(str1, @"\r\n?|\n", ",");
                if (result.EndsWith(",,"))
                {
                    result = result.Remove(result.Length - 1);
                }
                txtAddStone4.Text = result;
                txtAddStone4.Select(txtAddStone4.Text.Length, 0);
            }
            catch (Exception EX)
            {
                Global.MessageError(EX.Message);
                return;
            }
        }

        private void txtAddStone5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String str1 = Val.ToString(txtAddStone5.Text);
                string result = Regex.Replace(str1, @"\r\n?|\n", ",");
                if (result.EndsWith(",,"))
                {
                    result = result.Remove(result.Length - 1);
                }
                txtAddStone5.Text = result;
                txtAddStone5.Select(txtAddStone5.Text.Length, 0);
            }
            catch (Exception EX)
            {
                Global.MessageError(EX.Message);
                return;
            }
        }

        private void txtAddStone6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String str1 = Val.ToString(txtAddStone6.Text);
                string result = Regex.Replace(str1, @"\r\n?|\n", ",");
                if (result.EndsWith(",,"))
                {
                    result = result.Remove(result.Length - 1);
                }
                txtAddStone6.Text = result;
                txtAddStone6.Select(txtAddStone6.Text.Length, 0);
            }
            catch (Exception EX)
            {
                Global.MessageError(EX.Message);
                return;
            }
        }

        private void txtAddStone7_TextChanged(object sender, EventArgs e)
        {
            try
            {
                String str1 = Val.ToString(txtAddStone7.Text);
                string result = Regex.Replace(str1, @"\r\n?|\n", ",");
                if (result.EndsWith(",,"))
                {
                    result = result.Remove(result.Length - 1);
                }
                txtAddStone7.Text = result;
                txtAddStone7.Select(txtAddStone7.Text.Length, 0);
            }
            catch (Exception EX)
            {
                Global.MessageError(EX.Message);
                return;
            }
        }

        private void ListStatus_Load(object sender, EventArgs e)
        {

        }

        private void lblResetCarat_Click_2(object sender, EventArgs e)
        {
            DTabSize.Rows.Clear();
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
            DTabSize.Rows.Add(DTabSize.NewRow());
        }

        private void BtnCtsSelection_Click(object sender, EventArgs e)
        {
            try
            {
                PanelGrdCts.Visible = true;
                GrpSelectCarat.Visible = true;
                GrpSelectCarat.BringToFront();
                txtFromCts2.Text = string.Empty;
                txtFromCts3.Text = string.Empty;
                txtFromCts4.Text = string.Empty;
                txtFromCts5.Text = string.Empty;
                txtToCts2.Text = string.Empty;
                txtToCts3.Text = string.Empty;
                txtToCts4.Text = string.Empty;
                txtToCts5.Text = string.Empty;
            }
            catch (Exception EX)
            {
                Global.MessageError(EX.Message);
                return;
            }
        }

        private void BtnGridExport_Click(object sender, EventArgs e)
        {
            try
            {
                Global.ExcelExport("Mumbai Live Stock" , GrdDetail);
            }
            catch (Exception EX)
            {
                Global.MessageError(EX.Message);
            }
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            pStrWhatsappMessage = Val.ToString(txtMessage.Text);
            if (pStrWhatsappMessage == "")
            {
                if (Global.Confirm("No any Message found\n\nStil you want to send Message ?") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }
            FrmSendWhatsAppMessage FrmSendWhatsAppMessage = new FrmSendWhatsAppMessage();
            FrmSendWhatsAppMessage.MdiParent = Global.gMainRef;
            FrmSendWhatsAppMessage.ShowForm(pStrWhatsappMessage);
            ObjFormEvent.ObjToDisposeList.Add(FrmSendWhatsAppMessage);
        }
    }
}