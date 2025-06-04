using BusLib.Configuration;
using BusLib.Rapaport;
using BusLib.TableName;
using BusLib.Transaction;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using MahantExport.Utility;
using BusLib;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraPrintingLinks;
using DevExpress.XtraPrinting;
using System.IO;
using OfficeOpenXml;

namespace MahantExport.Stock
{
    public partial class FrmStoneCreateAndUpdateNew : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();

        BODevGridSelection ObjGridSelection;

        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        BOFindRap ObjRap = new BOFindRap();

        DataTable DTabParameter = new DataTable();
        DataTable DTabStcok = new DataTable();
        DataTable DTabStcokGrid = new DataTable();
        DataTable DTabExcel = new DataTable();
        DataTable DTabDetailExcel = new DataTable();


        Color mSelectedColor = Color.FromArgb(192, 0, 0);
        Color mDeSelectColor = Color.Black;
        Color mSelectedBackColor = Color.FromArgb(255, 224, 192);
        Color mDSelectedBackColor = Color.WhiteSmoke;

        #region Property Setting

        public FrmStoneCreateAndUpdateNew()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            ObjPer.GetFormPermission(Val.ToString(this.Tag));

            DataTable DTabRapDate = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.RAPDATE);
            DTabRapDate.DefaultView.Sort = "RAPDATE DESC";
            DTabRapDate = DTabRapDate.DefaultView.ToTable();

            //CmbRapDate.Items.Clear();
            //foreach (DataRow DRow in DTabRapDate.Rows)
            //{
            //    CmbRapDate.Items.Add(DateTime.Parse(Val.ToString(DRow["RAPDATE"])).ToString("dd/MM/yyyy"));
            //}
            //CmbRapDate.SelectedIndex = 0;

            //DataRow[] DR = null;
            DataTable DTabPrdType = new BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_LAB_PRD);//Keyur:04/09/2024
            //DR = DTabPrdType.Select("PRDTYPE_ID IN(3,4,5)");
            //foreach (DataRow DRow in DR)
            //{
            //    DTabPrdType.Rows.Remove(DRow);
            //}
            //End: Dhara: 18-06-2020

            //DTabPrdType.DefaultView.Sort = "SEQUENCENO";
            //DTabPrdType = DTabPrdType.DefaultView.ToTable();

            //CmbProcess.DataSource = DTabPrdType;
            //CmbProcess.DisplayMember = "PRDTYPENAME";
            //CmbProcess.ValueMember = "PRDTYPE_ID";

            //Start Keyur:04/09/2024
            //CmbLab.DataSource = DTabPrdType;
            //CmbLab.DisplayMember = "LABNAME";
            //CmbLab.ValueMember = "LAB_ID";
            //End Keyur:04/09/2024

            CmbLab.SelectedIndex = 0;
            CmbPrdType_SelectedIndexChanged(null, null);

            this.Show();
            
            //Added By Gunjan:28/02/2024

            if (MainGrdDetail.RepositoryItems.Count == 9)
            {
                ObjGridSelection = new BODevGridSelection();
                ObjGridSelection.View = GrdDetail;
                ObjGridSelection.ISBoolApplicableForPageConcept = true;
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
                GrdDetail.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;
            }
            else
            {
                if (ObjGridSelection != null)
                    ObjGridSelection.ClearSelection();
            }
            if (ObjGridSelection != null)
            {
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            }

            DTabStcokGrid.Columns.Add("STOCK_ID", typeof(string));
            DTabStcokGrid.Columns.Add("SRNO", typeof(Int32));
            DTabStcokGrid.Columns.Add("PRDTYPE", typeof(string));
            DTabStcokGrid.Columns.Add("EMPLOYEE", typeof(string));
            DTabStcokGrid.Columns.Add("PARTYSTOCKNO", typeof(string));
            DTabStcokGrid.Columns.Add("CARAT", typeof(double));
            DTabStcokGrid.Columns.Add("DIAMONDTYPE", typeof(string));
            DTabStcokGrid.Columns.Add("RAPPOPRT", typeof(double));
            DTabStcokGrid.Columns.Add("DISCOUNT", typeof(double));
            DTabStcokGrid.Columns.Add("PRICEPERCTS", typeof(double));
            DTabStcokGrid.Columns.Add("AMOUNT", typeof(double));
            DTabStcokGrid.Columns.Add("SHAPE", typeof(string));
            DTabStcokGrid.Columns.Add("COLOR", typeof(string));
            DTabStcokGrid.Columns.Add("CLARITY", typeof(string));
            DTabStcokGrid.Columns.Add("CUT", typeof(string));
            DTabStcokGrid.Columns.Add("POL", typeof(string));
            DTabStcokGrid.Columns.Add("SYM", typeof(string));
            DTabStcokGrid.Columns.Add("FLR", typeof(string));
            DTabStcokGrid.Columns.Add("BLACKINC", typeof(string));
            DTabStcokGrid.Columns.Add("TABLEINC", typeof(string));
            DTabStcokGrid.Columns.Add("MILKY", typeof(string));
            DTabStcokGrid.Columns.Add("LUSTER", typeof(string));
            DTabStcokGrid.Columns.Add("PAVILIONOPEN", typeof(string));
            DTabStcokGrid.Columns.Add("COLORSHADE", typeof(string));
            DTabStcokGrid.Columns.Add("TABLEOPENINC", typeof(string));
            DTabStcokGrid.Columns.Add("CROWNOPEN", typeof(string));
            DTabStcokGrid.Columns.Add("PAVOPEN", typeof(string));
            DTabStcokGrid.Columns.Add("TABLEOPEN", typeof(string));


            //End As Gunjan
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

        #endregion

        #region Buttion Desion

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            LiveStockProperty Property = new LiveStockProperty();
            try
            {

                if (Global.Confirm("Are Your Sure To Delete The Record ?") == System.Windows.Forms.DialogResult.No)
                    return;

                FrmPassword FrmPassword = new FrmPassword();
                if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                {

                    Property.STOCKNO = Val.ToString(txtExportStoneNo.Text);
                    Property = ObjStock.Delete(Property);
                    Global.Message(Property.ReturnMessageDesc);

                    if (Property.ReturnMessageType == "SUCCESS")
                    {
                        Global.Message(Property.ReturnMessageDesc);
                    }
                    else
                    {
                        txtExportStoneNo.Focus();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Global.MessageToster(ex.Message);
            }
            Property = null;
        }

        #endregion

        #region Save

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                LiveStockProperty Property = new LiveStockProperty();
                this.Cursor = Cursors.WaitCursor;

                if (Val.ToInt32(txtShape.Tag) == 0) //if (Val.ToInt32(GetSelectedBtnID(PanelShape)) == 0)
                {
                    Global.Message("Shape is Required");
                    return;
                }

                if (Val.Val(txtCarat.Text) == 0)
                {
                    Global.Message("Carat is Required");
                    txtCarat.Focus();
                    return;
                }

                Property.STOCK_ID = Val.ToString(txtExportStoneNo.Tag).Trim().Equals(string.Empty) ? Guid.NewGuid() : Guid.Parse(Val.ToString(txtExportStoneNo.Tag));
                Property.STOCKNO = Val.ToString(txtExportStoneNo.Text);
                Property.CARAT = Val.Val(txtCarat.Text);
                //Property.PAVANGLE = Val.Val(txtPavAngle.Text);
                Property.MEASUREMENT = Val.ToString(txtComment.Text);

                Property.SHAPE_ID = Val.ToInt32(txtShape.Tag);
                Property.COLOR_ID = Val.ToInt32(txtColor.Tag);
                Property.CUT_ID = Val.ToInt32(txtLocation.Tag);
                Property.BLACKINC_ID = Val.ToInt32(txtBlackInc.Tag);
                Property.TABLEINC_ID = Val.ToInt32(txtTableInc.Tag);
                Property.MILKY_ID = Val.ToInt32(txtMilky.Tag);
                Property.LUSTER_ID = Val.ToInt32(txtLuster.Tag);
                Property.TABLEOPEN_ID = Val.ToInt32(txtTableOpen.Tag);
                Property.CLARITY_ID = Val.ToInt32(txtClarity.Tag);
                Property.COLORSHADE_ID = Val.ToInt32(txtColorShade.Tag);
                Property.CROWNOPEN_ID = Val.ToInt32(txtCrownOpen.Tag);
                Property.PAVOPEN_ID = Val.ToInt32(txtPavilionOpen.Tag);

                Property.COSTRAPAPORT = Val.Val(txtRapaport.Text);
                Property.COSTDISCOUNT = Val.Val(txtDisc.Text);
                Property.COSTPRICEPERCARAT = Val.Val(txtPricePerCarat.Text);
                Property.COSTAMOUNT = Val.Val(txtAmount.Text);

                Property = ObjStock.SingleStoneCreateAndUpdate(Property);
                this.Cursor = Cursors.Default;
                string StrReturnDesc = Property.ReturnMessageDesc;
                if (Property.ReturnMessageType == "SUCCESS")
                {
                    Global.Message(Property.ReturnMessageDesc);

                    this.Cursor = Cursors.Default;
                    //BtnClear_Click(null,null);
                    DataRow Ds = DTabStcokGrid.NewRow();
                   
                    Ds["SRNO"] = Val.ToInt32(DTabStcokGrid.Rows.Count)+1;
                    Ds["STOCK_ID"] = Property.STOCK_ID;
                    Ds["PRDTYPE"] = Val.ToString(CmbLab.Text);
                    //Ds["EMPLOYEE"] = Val.ToString(txtEmpName.Text);
                    Ds["PARTYSTOCKNO"] = Val.ToString(txtExportStoneNo.Text);
                    Ds["CARAT"] = Val.ToDouble(txtCarat.Text);
                    //Ds["DIAMONDTYPE"] = Val.ToString(cmbDiamondType.SelectedItem);
                    Ds["RAPPOPRT"] = Val.ToDouble(txtRapaport.Text);
                    Ds["DISCOUNT"] = Val.ToDouble(txtDisc.Text);
                    Ds["PRICEPERCTS"] = Val.ToDouble(txtPricePerCarat.Text);
                    Ds["AMOUNT"] = Val.ToDouble(txtAmount.Text);

                    Ds["SHAPE"] = Val.ToString(txtShape.Text);
                    Ds["COLOR"] = Val.ToString(txtColor.Text);
                    Ds["CLARITY"] = Val.ToString(txtClarity.Text);
                    Ds["CUT"] = Val.ToString(txtLocation.Text);
                    //Ds["POL"] = Val.ToString(txtPolish.Text);
                    //Ds["SYM"] = Val.ToString(txtsym.Text);
                    //Ds["FLR"] = Val.ToString(txtFL.Text);
                    Ds["BLACKINC"] = Val.ToString(txtBlackInc.Text);
                    Ds["TABLEINC"] = Val.ToString(txtTableInc.Text);
                    Ds["MILKY"] = Val.ToString(txtMilky.Text);
                    Ds["LUSTER"] = Val.ToString(txtLuster.Text);
                    Ds["TABLEOPENINC"] = Val.ToString(txtTableOpen.Text);
                    Ds["COLORSHADE"] = Val.ToString(txtColorShade.Text);
                    Ds["CROWNOPEN"] = Val.ToString(txtCrownOpen.Text);
                    Ds["PAVOPEN"] = Val.ToString(txtPavilionOpen.Text);

                    DTabStcokGrid.Rows.Add(Ds);
                    MainGrdDetail.DataSource = DTabStcokGrid;
                    GrdDetail.RefreshData();
                    Clear();
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    Global.Message(Property.ReturnMessageDesc);
                    txtExportStoneNo.Focus();
                }

                Property = null;

            }
            catch (Exception Ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(Ex.Message.ToString());
            }
        }

        #endregion

        #region Other Operation


        public void Clear()
        {
            txtExportStoneNo.Text = string.Empty;
            txtCarat.Text = string.Empty;

            txtShape.Tag = string.Empty;
            txtColor.Tag = string.Empty;
            txtLocation.Tag = string.Empty;
            txtBlackInc.Tag = string.Empty;
            txtTableInc.Tag = string.Empty;
            txtMilky.Tag = string.Empty;
            txtLuster.Tag = string.Empty;
            txtTableOpen.Tag = string.Empty;
            txtClarity.Tag = string.Empty;
            txtColorShade.Tag = string.Empty;
            txtCrownOpen.Tag = string.Empty;
            txtPavilionOpen.Tag = string.Empty;

            txtShape.Text = string.Empty;
            txtColor.Text = string.Empty;
            txtLocation.Text = string.Empty;
            txtBlackInc.Text = string.Empty;
            txtTableInc.Text = string.Empty;
            txtMilky.Text = string.Empty;
            txtLuster.Text = string.Empty;
            txtTableOpen.Text = string.Empty;
            txtClarity.Text = string.Empty;
            txtColorShade.Text = string.Empty;
            txtCrownOpen.Text = string.Empty;
            txtPavilionOpen.Text = string.Empty;

            txtRapaport.Text = string.Empty;
            txtDisc.Text = string.Empty;
            txtPricePerCarat.Text = string.Empty;
            txtAfterRate.Text = string.Empty;
            txtAfterCarat.Text = string.Empty;
            txtAmount.Text = string.Empty;
            txtAfterStoneNo.Text = string.Empty;
            txtAfterStoneNo.Tag = string.Empty;
            txtComment.Text = string.Empty;

            //txtEmpName.Tag = string.Empty;
            //txtEmpName.Text = string.Empty;

            //cmbDiamondType.SelectedIndex = 0;
        }
        #endregion

        private void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Clear();
                DTabStcokGrid.Rows.Clear();
                MainGrdDetail.DataSource = null;
                GrdDetail.RefreshData();

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtStoneNo_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                DTabStcok = ObjStock.GetManuallStockDetail(Val.ToString(txtExportStoneNo.Text), "FACTORYGRADING");
                if (DTabStcok.Rows.Count > 0)
                {
                    txtExportStoneNo.Tag = Val.ToGuid(DTabStcok.Rows[0]["STOCK_ID"]);
                    txtCarat.Text = Val.ToString(DTabStcok.Rows[0]["CARAT"]);                    
                    txtShape.Tag = Val.ToInt(DTabStcok.Rows[0]["SHAPE_ID"]);                   
                    txtShape.Text = Val.ToString(DTabStcok.Rows[0]["SHAPENAME"]);                  
                    txtLocation.Text = Val.ToString(DTabStcok.Rows[0]["LOCATION"]);

                    txtColor.Tag = Val.ToInt(DTabStcok.Rows[0]["COLOR_ID"]);
                    txtColor.Text = Val.ToString(DTabStcok.Rows[0]["COLORNAME"]);
                    txtClarity.Tag = Val.ToInt(DTabStcok.Rows[0]["CLARITY_ID"]);
                    txtClarity.Text = Val.ToString(DTabStcok.Rows[0]["CLANAME"]);
                    txtPricePerCarat.Text = Val.ToString(DTabStcok.Rows[0]["PRICEPERCARAT"]);

                    if (Val.ToDouble(DTabStcok.Rows[0]["ExcRate"]) != 0)
                    {
                        txtExcRate.Text = Val.ToString(DTabStcok.Rows[0]["ExcRate"]);
                    }
                    txtLocation.Enabled = false;
                    txtColor.Focus();
                }
                else
                {
                    txtShape.Tag = string.Empty;
                    txtColor.Tag = string.Empty;
                    txtExportStoneNo.Tag = string.Empty;
                    txtClarity.Tag = string.Empty;
                 
                    txtShape.Text = string.Empty;
                    txtColor.Text = string.Empty;
                    txtLocation.Text = string.Empty;
                   
                    txtPricePerCarat.Text = string.Empty;

                    txtCarat.Text = string.Empty;
                    txtLocation.Text = string.Empty;
                    txtLocation.Enabled = true;

                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void CmbPrdType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Val.ToInt32(CmbLab.SelectedIndex) == 0)
                {
                    panelExportRappaport.Enabled = false;
                    PanelAfterLabRappaport.Enabled = true;
                }
                else
                {
                    panelExportRappaport.Enabled = true;
                    PanelAfterLabRappaport.Enabled = false;
                }
                Clear();
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void BtnGIAControlMap_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DtInvDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);
                if (DtInvDetail == null)
                {
                    return;
                }
                if (DtInvDetail.Rows.Count <= 0)
                {
                    Global.Message("Please Select AtLeast One Record From The List.");
                    return;
                }
                string strStoneNo = "";

                DtInvDetail.DefaultView.Sort = "SRNO";
                if (DtInvDetail.Rows.Count > 0)
                {
                    var list = DtInvDetail.AsEnumerable().Select(r => r["PARTYSTOCKNO"].ToString());
                    strStoneNo = string.Join(",", list);
                }

                //DtInvDetail.DefaultView.Sort = "SRNO";
                //if (DtInvDetail.Rows.Count > 0)
                //{
                //    var list = DtInvDetail.AsEnumerable().Select(r => r["PARTYSTOCKNO"].ToString());
                //    strStoneNo = string.Join(",", list);
                //}


                FrmGIAControlNoMap FrmGIAControlNoMap = new FrmGIAControlNoMap();
                FrmGIAControlNoMap.MdiParent = Global.gMainRef;
                FrmGIAControlNoMap.ShowForm(strStoneNo);

                this.Cursor = Cursors.Default;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        public void FindRap()
        {
            Trn_RapSaveProperty Property = new Trn_RapSaveProperty();
            Property.STOCKNO = Val.ToString(txtExportStoneNo.Text);

            Property.SHAPE_ID = Val.ToInt32(txtShape.Tag);
            Property.COLOR_ID = Val.ToInt32(txtColor.Tag);
            Property.CUT_ID = Val.ToInt32(txtLocation.Tag);
            //Property.POL_ID = Val.ToInt32(txtPolish.Tag);
            //Property.SYM_ID = Val.ToInt32(txtsym.Tag);
            //Property.FL_ID = Val.ToInt32(txtFL.Tag);
            //Property.LAB_ID = Val.ToInt32(txtLab.Text);
            Property.SIDEBLACKINC_ID = Val.ToInt32(txtBlackInc.Tag);
            Property.TABLEINC_ID = Val.ToString(txtTableInc.Tag);
            Property.MILKY_ID = Val.ToInt32(txtMilky.Tag);
            Property.LUSTER_ID = Val.ToInt32(txtLuster.Tag);
            Property.TABLEINC_ID = Val.ToString(txtTableInc.Tag);
            Property.CLARITY_ID = Val.ToInt32(txtClarity.Tag);

            Property.CARAT = Val.Val(txtCarat.Text);
            Property = ObjRap.FindRap(Property);
            txtRapaport.Text = Val.ToString(Property.RAPAPORT);
            txtPricePerCarat.Text = Math.Round(Val.Val(txtRapaport.Text) + (Val.Val(txtRapaport.Text) * Val.Val(txtDisc.Text) / 100)).ToString();
            txtAmount.Text = Math.Round(Val.Val(txtCarat.Text) * Val.Val(txtPricePerCarat.Text), 2).ToString();
        }

        private void txtDisc_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    FindRap();
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void txtPricePerCarat_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    //Trn_RapSaveProperty Property = new Trn_RapSaveProperty();

                    //Property.RAPAPORT = Val.Val(txtRapaport.Text);
                    //Property.MGPRICEPERCARAT = Val.Val(txtAfterRate.Text);
                    //Property.CARAT = Val.Val(txtCarat.Text);
                    //if (Property.RAPAPORT != 0)
                    //{
                    //    txtDisc.Text = Math.Round(((Property.MGPRICEPERCARAT - Property.RAPAPORT  ) / Property.RAPAPORT) * 100, 2).ToString(); //#P:23-04-2021
                    //}
                    //else
                    //    txtDisc.Text = string.Empty;

                    //txtAmount.Text = Math.Round(Property.CARAT * Val.Val(Property.MGPRICEPERCARAT), 2).ToString();


                    double Rappaport = Val.Val(txtRapaport.Text);
                    double Discount = Val.Val(txtDisc.Text);
                    double Carat = Val.Val(txtAfterCarat.Text);

                    if(Rappaport != 0)
                    {
                        txtAfterRate.Text = Math.Round(Rappaport + ((Rappaport * Discount) / 100),2).ToString();
                    }
                    else
                    {
                        txtAfterRate.Text = string.Empty;
                    }
                    txtAmount.Text = Math.Round(Carat * Val.Val(txtAfterRate.Text), 2).ToString();

                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void txtShape_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "SHAPECODE,SHAPENAME,SHORTNAME";
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                this.Cursor = Cursors.WaitCursor;
                FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SHAPE);

                FrmSearch.mStrColumnsToHide = "SHAPE_ID";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.DRow != null)
                {
                    txtShape.Text = Val.ToString(FrmSearch.DRow["SHAPENAME"]);
                    txtShape.Tag = Val.ToString(FrmSearch.DRow["SHAPE_ID"]);

                }

                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void txtAfterStoneNo_Validating(object sender, CancelEventArgs e)
        {
            try
            {

                DTabStcok = ObjStock.GetManuallStockDetail(Val.ToString(txtAfterStoneNo.Text), "");
                if (DTabStcok.Rows.Count > 0)
                {
                    txtAfterStoneNo.Tag = Val.ToGuid(DTabStcok.Rows[0]["STOCK_ID"]);
                    txtLuster.Text = Val.ToString(DTabStcok.Rows[0]["LUSTERNAME"]);
                    txtBlackInc.Text = Val.ToString(DTabStcok.Rows[0]["BLACKINCLUSION"]);
                    txtTableInc.Text = Val.ToString(DTabStcok.Rows[0]["TABLEINCLUSION"]);
                    txtMilky.Text = Val.ToString(DTabStcok.Rows[0]["MILKYNAME"]);
                    txtColorShade.Text = Val.ToString(DTabStcok.Rows[0]["COLORSHADE"]);
                    txtTableOpen.Text = Val.ToString(DTabStcok.Rows[0]["TABLEOPENINCNAME"]);
                    txtCrownOpen.Text = Val.ToString(DTabStcok.Rows[0]["CROWNOPEN"]);
                    txtPavilionOpen.Text = Val.ToString(DTabStcok.Rows[0]["PAVILIONOPEN"]);

                    txtLuster.Tag = Val.ToInt(DTabStcok.Rows[0]["LUSTER_ID"]);
                    txtBlackInc.Tag = Val.ToInt(DTabStcok.Rows[0]["TABLEBLACKINC_ID"]);
                    txtTableInc.Tag = Val.ToInt(DTabStcok.Rows[0]["TABLEINC_ID"]);
                    txtMilky.Tag = Val.ToInt(DTabStcok.Rows[0]["MILKY_ID"]);
                    txtColorShade.Tag = Val.ToInt(DTabStcok.Rows[0]["COLORSHADE_ID"]);
                    txtTableOpen.Tag = Val.ToInt(DTabStcok.Rows[0]["TABLEOPENINC_ID"]);
                    txtCrownOpen.Tag = Val.ToInt(DTabStcok.Rows[0]["CROWNOPEN_ID"]);
                    txtPavilionOpen.Tag = Val.ToInt(DTabStcok.Rows[0]["PAVILIONOPEN_ID"]);

                    txtAfterRate.Text = Val.ToString(DTabStcok.Rows[0]["PRICEPERCARAT"]);
                    txtDisc.Text = Val.ToString(DTabStcok.Rows[0]["DISCOUNT"]);
                    txtAmount.Text = Val.ToString(DTabStcok.Rows[0]["AMOUNT"]);
                    txtRapaport.Text = Val.ToString(DTabStcok.Rows[0]["RAPAPORT"]);

                    txtLocation.Text = Val.ToString(DTabStcok.Rows[0]["LOCATION"]);
                    txtAfterCarat.Text = Val.ToString(DTabStcok.Rows[0]["CARAT"]);

                    txtLocation.Enabled = false;

                }
                else
                {
                    txtAfterStoneNo.Tag = string.Empty;
                    txtLuster.Text = string.Empty;
                    txtBlackInc.Text = string.Empty;
                    txtTableInc.Text = string.Empty;
                    txtMilky.Text = string.Empty;
                    txtColorShade.Text = string.Empty;
                    txtTableOpen.Text = string.Empty;
                    txtCrownOpen.Text = string.Empty;
                    txtPavilionOpen.Text = string.Empty;

                    txtLuster.Tag = string.Empty;
                    txtBlackInc.Tag = string.Empty;
                    txtTableInc.Tag = string.Empty;
                    txtMilky.Tag = string.Empty;
                    txtColorShade.Tag = string.Empty;
                    txtTableOpen.Tag = string.Empty;
                    txtCrownOpen.Tag = string.Empty;
                    txtPavilionOpen.Tag = string.Empty;

                    txtAfterRate.Text = string.Empty;
                    txtDisc.Text = string.Empty;
                    txtAmount.Text = string.Empty;
                    txtRapaport.Text = string.Empty;

                    txtLocation.Text = string.Empty;
                    txtLocation.Enabled = true;
                    txtAfterCarat.Text = string.Empty;

                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            try
            {
                LiveStockProperty Property = new LiveStockProperty();
                this.Cursor = Cursors.WaitCursor;

                if (Val.ToInt32(txtShape.Tag) == 0)
                {
                    Global.Message("Shape is Required");
                    return;
                }

                if (Val.Val(txtCarat.Text) == 0)
                {
                    Global.Message("Carat is Required");
                    txtCarat.Focus();
                    return;
                }

                Property.LAB = Val.ToString(CmbLab.SelectedItem);//Keyur:04/09/2024
                Property.STOCK_ID = Val.ToGuid(txtExportStoneNo.Tag);
                Property.STOCKNO = Val.ToString(txtExportStoneNo.Text);
                Property.CARAT = Val.Val(txtCarat.Text);

                Property.SHAPE_ID = Val.ToInt32(txtShape.Tag);
                Property.COLOR_ID = Val.ToInt32(txtColor.Tag);
                Property.CLARITY_ID = Val.ToInt32(txtClarity.Tag);

                Property.COSTPRICEPERCARAT = Val.Val(txtPricePerCarat.Text);
                Property.EXCRATE = Val.Val(txtExcRate.Text);


                Property = ObjStock.SingleStoneManualPredictionCreateAndUpdate(Property, "FACTORYGRADING");
                this.Cursor = Cursors.Default;
                string StrReturnDesc = Property.ReturnMessageDesc;
                if (Property.ReturnMessageType == "SUCCESS")
                {
                    //Global.Message(Property.ReturnMessageDesc);

                    this.Cursor = Cursors.Default;
                    DataRow Ds = DTabStcokGrid.NewRow();

                    Ds["SRNO"] = Val.ToInt32(DTabStcokGrid.Rows.Count) + 1;
                    Ds["STOCK_ID"] = Property.STOCK_ID;
                    Ds["PRDTYPE"] = "FINAL_COSTING";
                    Ds["PARTYSTOCKNO"] = Val.ToString(txtExportStoneNo.Text);
                    Ds["CARAT"] = Val.ToDouble(txtCarat.Text);
                    Ds["DIAMONDTYPE"] = "Natural";
                    Ds["PRICEPERCTS"] = Val.ToDouble(txtPricePerCarat.Text);
                    Ds["AMOUNT"] = Val.ToDouble(txtAmount.Text);
                    Ds["SHAPE"] = Val.ToString(txtShape.Text);
                    Ds["COLOR"] = Val.ToString(txtColor.Text);
                    Ds["CLARITY"] = Val.ToString(txtClarity.Text);

                    DTabStcokGrid.Rows.Add(Ds);
                    DTabStcokGrid.AcceptChanges();
                    MainGrdDetail.DataSource = DTabStcokGrid;
                    GrdDetail.RefreshData();
                    Clear();
                    txtExportStoneNo.Focus();
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    Global.Message(Property.ReturnMessageDesc);
                    txtExportStoneNo.Focus();
                }

                Property = null;

            }
            catch (Exception Ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(Ex.Message.ToString());
            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            try
            {
                LiveStockProperty Property = new LiveStockProperty();
                this.Cursor = Cursors.WaitCursor;



                if (Val.ToString(txtAfterStoneNo.Tag) == "")
                {
                    Global.Message("Stock ID is Required");
                    txtAfterStoneNo.Focus();
                    return;
                }
                Property.STOCK_ID = Val.ToGuid(txtAfterStoneNo.Tag);
                Property.STOCKNO = Val.ToString(txtAfterStoneNo.Text);

                Property.BLACKINC_ID = Val.ToInt32(txtBlackInc.Tag);
                Property.TABLEINC_ID = Val.ToInt32(txtTableInc.Tag);
                Property.MILKY_ID = Val.ToInt32(txtMilky.Tag);
                Property.LUSTER_ID = Val.ToInt32(txtLuster.Tag);
                Property.TABLEOPEN_ID = Val.ToInt32(txtTableOpen.Tag);
                Property.COLORSHADE_ID = Val.ToInt32(txtColorShade.Tag);
                Property.CROWNOPEN_ID = Val.ToInt32(txtCrownOpen.Tag);
                Property.PAVOPEN_ID = Val.ToInt32(txtPavilionOpen.Tag);

                Property.COSTRAPAPORT = Val.Val(txtRapaport.Text);
                Property.COSTDISCOUNT = Val.Val(txtDisc.Text);
                Property.COSTPRICEPERCARAT = Val.Val(txtAfterRate.Text);
                Property.COSTAMOUNT = Val.Val(txtAmount.Text);
              
                Property.REMARK = Val.ToString(txtComment.Text);

                Property = ObjStock.SingleStoneManualPredictionCreateAndUpdate(Property,"LABGRADING");
                this.Cursor = Cursors.Default;
                string StrReturnDesc = Property.ReturnMessageDesc;
                if (Property.ReturnMessageType == "SUCCESS")
                {
                    Global.Message(Property.ReturnMessageDesc);

                    this.Cursor = Cursors.Default;
                    //BtnClear_Click(null,null);
                    DataRow Ds = DTabStcokGrid.NewRow();

                    Ds["SRNO"] = Val.ToInt32(DTabStcokGrid.Rows.Count) + 1;
                    Ds["STOCK_ID"] = Property.STOCK_ID;
                    Ds["PRDTYPE"] = "LABGRADING";


                    Ds["PARTYSTOCKNO"] = Val.ToString(txtAfterStoneNo.Text);
                    Ds["CARAT"] = Val.ToDouble(txtAfterCarat.Text);
                    Ds["PRICEPERCTS"] = Val.ToDouble(txtAfterRate.Text);

                    Ds["DIAMONDTYPE"] = "NATURAL";
                    Ds["RAPPOPRT"] = Val.ToDouble(txtRapaport.Text);
                    Ds["DISCOUNT"] = Val.ToDouble(txtDisc.Text);
                   
                    Ds["AMOUNT"] = Val.ToDouble(txtAmount.Text);
                    Ds["BLACKINC"] = Val.ToString(txtBlackInc.Text);
                    Ds["TABLEINC"] = Val.ToString(txtTableInc.Text);
                    Ds["MILKY"] = Val.ToString(txtMilky.Text);
                    Ds["LUSTER"] = Val.ToString(txtLuster.Text);
                    Ds["TABLEOPENINC"] = Val.ToString(txtTableInc.Text);
                    Ds["TABLEOPEN"] = Val.ToString(txtTableOpen.Text);
                    Ds["COLORSHADE"] = Val.ToString(txtColorShade.Text);
                    Ds["CROWNOPEN"] = Val.ToString(txtCrownOpen.Text);
                    Ds["PAVOPEN"] = Val.ToString(txtPavilionOpen.Text);

                    DTabStcokGrid.Rows.Add(Ds);
                    MainGrdDetail.DataSource = DTabStcokGrid;
                    GrdDetail.RefreshData();
                    Clear();
                    txtAfterStoneNo.Focus();
                }
                else
                {
                    this.Cursor = Cursors.Default;
                    Global.Message(Property.ReturnMessageDesc);
                    txtAfterStoneNo.Focus();
                }

                Property = null;

            }
            catch (Exception Ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(Ex.Message.ToString());
            }
        }


        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                if (panelExportRappaport.Enabled == true)
                {
                    GrdDetail.RefreshData();  // Ensure the grid is refreshed

                    // Fetch selected records
                    DataTable DtInvDetail = Global.GetSelectedRecordOfGrid(GrdDetail, true, ObjGridSelection);

                    if (DtInvDetail == null || DtInvDetail.Rows.Count <= 0)
                    {
                        Global.Message("Please Select AtLeast One Record For Export List");
                        return;
                    }

                    BtnGIAControlMap_Click(null, null);

                    ObjGridSelection.ClearSelection();
                }
                else
                {
                    Global.ExcelExport("Manual Prediction Entry",GrdDetail);
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnExcelExport_Click(object sender, EventArgs e)
        {
           
        }

        private void txtColor_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "COLORCODE,COLORNAME";
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                this.Cursor = Cursors.WaitCursor;
                FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COLOR);

                FrmSearch.mStrColumnsToHide = "SHAPE_ID";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.DRow != null)
                {
                    txtColor.Text = Val.ToString(FrmSearch.DRow["COLORNAME"]);
                    txtColor.Tag = Val.ToString(FrmSearch.DRow["COLOR_ID"]);
                }

                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void txtClarity_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "CLARITYCODE,CLARITYNAME";
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                this.Cursor = Cursors.WaitCursor;
                FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CLARITY);

                FrmSearch.mStrColumnsToHide = "CLARITY_ID";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.DRow != null)
                {
                    txtClarity.Text = Val.ToString(FrmSearch.DRow["CLARITYNAME"]);
                    txtClarity.Tag = Val.ToString(FrmSearch.DRow["CLARITY_ID"]);

                }

                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void txtLuster_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "LUSTERCODE,LUSTERNAME";
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                this.Cursor = Cursors.WaitCursor;
                FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LUSTER);

                FrmSearch.mStrColumnsToHide = "LUSTER_ID";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.DRow != null)
                {
                    txtLuster.Text = Val.ToString(FrmSearch.DRow["LUSTERNAME"]);
                    txtLuster.Tag = Val.ToString(FrmSearch.DRow["LUSTER_ID"]);

                }

                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void txtBlackInc_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "BLACKINCCODE,BLACKINCNAME";
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                this.Cursor = Cursors.WaitCursor;
                FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_BLACKINC);

                FrmSearch.mStrColumnsToHide = "BLACKINC_ID";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.DRow != null)
                {
                    txtBlackInc.Text = Val.ToString(FrmSearch.DRow["BLACKINCNAME"]);
                    txtBlackInc.Tag = Val.ToString(FrmSearch.DRow["BLACKINC_ID"]);

                }

                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void txtTableInc_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "OPENINCCODE,OPENINCNAME";
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                this.Cursor = Cursors.WaitCursor;
                FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_TABLE);

                FrmSearch.mStrColumnsToHide = "OPENINC_ID";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.DRow != null)
                {
                    txtTableInc.Text = Val.ToString(FrmSearch.DRow["OPENINCNAME"]);
                    txtTableInc.Tag = Val.ToString(FrmSearch.DRow["OPENINC_ID"]);

                }

                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void txtMilky_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "MILKYCODE,MILKYNAME";
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                this.Cursor = Cursors.WaitCursor;
                FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_MILKY);

                FrmSearch.mStrColumnsToHide = "MILKY_ID";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.DRow != null)
                {
                    txtMilky.Text = Val.ToString(FrmSearch.DRow["MILKYNAME"]);
                    txtMilky.Tag = Val.ToString(FrmSearch.DRow["MILKY_ID"]);

                }

                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void txtColorShade_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "COLORSHADECODE,COLORSHADENAME";
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                this.Cursor = Cursors.WaitCursor;
                FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COLORSHADE);

                FrmSearch.mStrColumnsToHide = "COLORSHADE_ID";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.DRow != null)
                {
                    txtColorShade.Text = Val.ToString(FrmSearch.DRow["COLORSHADENAME"]);
                    txtColorShade.Tag = Val.ToString(FrmSearch.DRow["COLORSHADE_ID"]);

                }

                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void txtTableOpen_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "OPENINCCODE,OPENINCNAME";
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                this.Cursor = Cursors.WaitCursor;
                FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_OPENINC);

                FrmSearch.mStrColumnsToHide = "OPENINC_ID";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.DRow != null)
                {
                    txtTableOpen.Text = Val.ToString(FrmSearch.DRow["OPENINCNAME"]);
                    txtTableOpen.Tag = Val.ToString(FrmSearch.DRow["OPENINC_ID"]);

                }

                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void txtCrownOpen_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "CROPENINCCODE,CROPENINCNAME";
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                this.Cursor = Cursors.WaitCursor;
                FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CROWN_OPEN);

                FrmSearch.mStrColumnsToHide = "CROPENINC_ID";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.DRow != null)
                {
                    txtCrownOpen.Text = Val.ToString(FrmSearch.DRow["CROPENINCNAME"]);
                    txtCrownOpen.Tag = Val.ToString(FrmSearch.DRow["CROPENINC_ID"]);

                }

                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void txtPavilionOpen_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "PAVOPENINCCODE,PAVOPENINCNAME";
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                this.Cursor = Cursors.WaitCursor;
                FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PAVILION_OPEN);

                FrmSearch.mStrColumnsToHide = "PAVOPENINC_ID";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.DRow != null)
                {
                    txtPavilionOpen.Text = Val.ToString(FrmSearch.DRow["PAVOPENINCNAME"]);
                    txtPavilionOpen.Tag = Val.ToString(FrmSearch.DRow["PAVOPENINC_ID"]);

                }

                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void txtAfterRate_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    //Trn_RapSaveProperty Property = new Trn_RapSaveProperty();

                    //Property.RAPAPORT = Val.Val(txtRapaport.Text);
                    //Property.MGPRICEPERCARAT = Val.Val(txtAfterRate.Text);
                    //Property.CARAT = Val.Val(txtCarat.Text);
                    //if (Property.RAPAPORT != 0)
                    //{
                    //    txtDisc.Text = Math.Round(((Property.MGPRICEPERCARAT - Property.RAPAPORT  ) / Property.RAPAPORT) * 100, 2).ToString(); //#P:23-04-2021
                    //}
                    //else
                    //    txtDisc.Text = string.Empty;

                    //txtAmount.Text = Math.Round(Property.CARAT * Val.Val(Property.MGPRICEPERCARAT), 2).ToString();


                    double Rappaport = Val.Val(txtRapaport.Text);
                    double Rate = Val.Val(txtAfterRate.Text);
                    double Carat = Val.Val(txtAfterCarat.Text);

                    if (Rappaport != 0)
                    {
                        txtDisc.Text = Math.Round(((Rate - Rappaport) / Rappaport) * 100, 2).ToString();
                    }
                    else
                    {
                        txtDisc.Text = string.Empty;
                    }
                    txtAmount.Text = Math.Round(Carat * Val.Val(txtAfterRate.Text), 2).ToString();

                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }
    }
}