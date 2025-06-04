using MahantExport.Utility;
using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using DevExpress.XtraPrinting;
using Google.API.Translate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.Utils.Drawing;
using DevExpress.XtraBars.Ribbon;
using System.Net;
//using MahantExport;

namespace MahantExport.Masters
{
    public partial class FrmStyleMaster : DevExpress.XtraEditors.XtraForm
    {
        string MergeOn = string.Empty;
        string MergeOnStr = string.Empty;

        System.Diagnostics.Stopwatch watch = null;
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_Style ObjMast = new BOMST_Style();
        BOFormPer ObjPer = new BOFormPer();
        DataTable DtabExcelData = new DataTable();
        DataTable DtabStockUpload = new DataTable();

        DataTable DtabFinalData = new DataTable();
        DataTable DtabPara = new DataTable();

        DataTable DTabStyle = new DataTable();
        DataTable DTabMaterial = new DataTable();
        DataTable DTabCollection = new DataTable();
        DataTable DTabMedia = new DataTable();
        DataTable DTabProperty = new DataTable();
        DataTable DTabRanking = new DataTable();
        DataTable DTabReview = new DataTable();

        string StrUploadFilename = "";

        #region Property Settings

        public FrmStyleMaster()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            BtnSave.Enabled = ObjPer.ISINSERT;
            BtnDelete.Enabled = ObjPer.ISDELETE;

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            

            Fill();
            FillProperty();
            


            this.Show();

            //CmbCollection.Properties.DataSource = new BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_COLLECTION);
            //CmbCollection.Properties.ValueMember = "COLLECTION_ID";
            //CmbCollection.Properties.DisplayMember = "COLLECTIONNAME";

            //CmbAdditionalProperty.Properties.DataSource = new BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_ADDITIONALPROPERTY);
            //CmbAdditionalProperty.Properties.ValueMember = "PROPERTY_ID";
            //CmbAdditionalProperty.Properties.DisplayMember = "PROPERTYVALUE";

            BtnAdd_Click(null, null);

            if(DTabMaterial.Rows.Count ==0 || DTabMaterial == null)
            {
                DataRow Dr = DTabMaterial.NewRow();
                DTabMaterial.Rows.Add(Dr);
            }

            if (DTabCollection.Rows.Count == 0 || DTabCollection == null)
            {
                DataRow Dr = DTabCollection.NewRow();
                DTabCollection.Rows.Add(Dr);
            }

            if (DTabMedia.Rows.Count == 0 || DTabMedia == null)
            {
                DataRow Dr = DTabMedia.NewRow();
                DTabMedia.Rows.Add(Dr);
            }

            if (DTabProperty.Rows.Count == 0 || DTabProperty == null)
            {
                DataRow Dr = DTabProperty.NewRow();
                DTabProperty.Rows.Add(Dr);
            }

            if (DTabRanking.Rows.Count == 0 || DTabRanking == null)
            {
                DataRow Dr = DTabRanking.NewRow();
                DTabRanking.Rows.Add(Dr);
            }

            if (DTabReview.Rows.Count == 0 || DTabReview == null)
            {
                DataRow Dr = DTabReview.NewRow();
                DTabReview.Rows.Add(Dr);
            }

        }

        public void ShowForm(string pStrStyleID)
        {
            //ObjPer.GetFormPermission(Val.ToString(this.Tag));
            //BtnSave.Enabled = ObjPer.ISINSERT;
            //BtnDelete.Enabled = ObjPer.ISDELETE;

            //Val.FormGeneralSetting(this);
            //AttachFormDefaultEvent();


            //this.Show();

            //CmbCollection.Properties.DataSource = new BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_COLLECTION);
            //CmbCollection.Properties.ValueMember = "COLLECTION_ID";
            //CmbCollection.Properties.DisplayMember = "COLLECTIONNAME";

            //DataSet DS = ObjMast.GetStyleDetailDInfoata(pGuidStyle_Id);

            ////DataSet DS = ObjMast.GetStyleDetailDInfoata(Guid.Parse(pStrStyleID));

            //DataRow DR = DS.Tables[0].Rows[0];
            //FetchValue(DR);

        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = false;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMast);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
        }

        #endregion

        #region Validation

        private bool ValSave()
        {
            if (txtStyleNo.Text.Trim().Equals(string.Empty))
            {
                Global.Message("StyleNo Is Required");
                txtStyleNo.Focus();
                return false;
            }
            else if (txtProductCatagory.Text.Trim().Equals(string.Empty))
            {
                Global.Message("Catagory Is Required");
                txtProductCatagory.Focus();
                return false;
            }
            else if (txtLocation.Text.Trim().Equals(string.Empty))
            {
                Global.Message("Location Is Required");
                txtLocation.Focus();
                return false;
            }
            //else if (txtCurrency.Text.Trim().Equals(string.Empty))
            //{
            //    Global.Message("Currency Is Required");
            //    txtCurrency.Focus();
            //    return false;
            //}
            else if (txtShortDescription.Text.Trim().Equals(string.Empty))
            {
                Global.Message("Short Description Is Required");
                txtShortDescription.Focus();
                return false;
            }

            else if (txtLongDescription.Text.Trim().Equals(string.Empty))
            {
                Global.Message("Long Description Is Required");
                txtLongDescription.Focus();
                return false;
            }

            return true;
        }

        private bool ValDelete()
        {
            if (txtStyleNo.Text.Trim().Length == 0)
            {
                Global.Message("Style No Is Required");
                txtStyleNo.Focus();
                return false;
            }

            return true;
        }

        #endregion

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            MainGrdStyle.Refresh();
            MainGrdMaterial.Refresh();
            MainGrdCollection.Refresh();
            MainGrdMedia.Refresh();
            MainGrdProperty.Refresh();
            MainGrdRanking.Refresh();
            MainGridReview.Refresh();


            DTabMaterial.Rows.Clear();
            DTabCollection.Rows.Clear();
            DTabMedia.Rows.Clear();
            DTabProperty.Rows.Clear();
            DTabRanking.Rows.Clear();
            DTabReview.Rows.Clear();

            BtnAddCollectionNewRow.PerformClick();
            BtnAddMaterialNewRow.PerformClick();
            BtnAddMediaNewRow.PerformClick();
            BtnAddPropertyNewRow.PerformClick();
            BtnAddRankingNewRow.PerformClick();
            BtnAddReviewNewRow.PerformClick();

            txtStyleNo.Text = string.Empty;
            txtStyleNo.Tag = string.Empty;

            txtProductCatagory.Tag = string.Empty;
            txtProductCatagory.Text = string.Empty;

            txtProductSubCatagory.Tag = string.Empty;
            txtProductSubCatagory.Text = string.Empty;

            txtLocation.Tag = string.Empty;
            txtLocation.Text = string.Empty;

            //CmbCollection.SetEditValue(-1);
            CmbSimilarStyle.SetEditValue(-1);
            CmbStatus.SelectedIndex = 0;
            //CmbAdditionalProperty.SetEditValue(-1);          

            txtGrossWt.Text = string.Empty;
            txtNetWt.Text = string.Empty;
            txtMetalWeight.Text = string.Empty;
            txtDiamondPcs.Text = string.Empty;
            txtDiamondWeight.Text = string.Empty;
            txtColorPcs.Text = string.Empty;
            txtColorWeight.Text = string.Empty;
            txtDiscountPrice.Text = string.Empty;
            txtSalePrice.Text = string.Empty;

            txtShortDescription.Text = string.Empty;
            txtLongDescription.Text = string.Empty;
            txtRemark.Text = string.Empty;

            txtStyleNo.Focus();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValSave() == false)
                {
                    return;
                }
                this.Cursor = Cursors.WaitCursor;

                StyleMasterProperty Property = new StyleMasterProperty();
                Property.STYLE_ID = Val.ToString(txtStyleNo.Tag).Trim().Equals(string.Empty) ? BusLib.Configuration.BOConfiguration.FindNewSequentialID() : Guid.Parse(Val.ToString(txtStyleNo.Tag));
                
                //Property.STYLE_ID = Val.ToString(txtStyleNo.Tag).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtStyleNo.Tag));
                Property.LOCATION_ID = Val.ToInt(txtLocation.Tag);
                Property.STYLENO = txtStyleNo.Text;

                Property.PRODUCTCATEGORY_ID = Val.ToInt32(txtProductCatagory.Tag);
                Property.PRODUCTSUBCATEGORY_ID = Val.ToInt32(txtProductSubCatagory.Tag);

                //Property.PRODUCTCATEGORY_ID = Val.ToInt(txtProductCatagory.Tag);
                //Property.PRODUCTSUBCATEGORY_ID = Val.ToString(txtProductSubCatagory.Text).Trim().Equals(string.Empty) ? 0 : Val.ToInt(txtProductSubCatagory.Tag);

                Property.GROSSWT = Val.ToDouble(txtGrossWt.Text);
                Property.NETWT = Val.ToDouble(txtNetWt.Text);
                Property.METALWT = Val.ToDouble(txtMetalWeight.Text);
                Property.DIAMONDPCS = Val.ToInt32(txtDiamondPcs.Text);
                Property.DIAMONDWT = Val.ToDouble(txtDiamondWeight.Text);
                Property.COLORPCS = Val.ToInt32(txtColorPcs.Text);
                Property.COLORWT = Val.ToDouble(txtColorWeight.Text);

                //Property.COLLECTION_ID = Val.ToInt32(CmbCollection.Properties.GetCheckedItems());
                Property.SIMILLARSTYLE = Val.Trim(CmbSimilarStyle.Properties.GetCheckedItems());
                //Property.PROPERTY_ID = Val.ToInt32(CmbAdditionalProperty.EditValue);
                //Property.PROPERTYVALUE = Val.ToString(CmbAdditionalProperty.Text);

                Property.SHORTDESCRIPTION = Val.ToString(txtShortDescription.Text);
                Property.LONGDESCRIPTION = Val.ToString(txtLongDescription.Text);
                //Property.STATUS = Val.ToString(CmbStatus.SelectedItem);
                Property.REMARK = Val.ToString(txtRemark.Text);

                DTabMaterial.TableName = "Table";
                string StrMaterial = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabMaterial.WriteXml(sw);
                    StrMaterial = sw.ToString();
                }

                DTabCollection.TableName = "Table";
                string StrCollection = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabCollection.WriteXml(sw);
                    StrCollection = sw.ToString();
                }

                DTabMedia.TableName = "Table";
                string StrMedia = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabMedia.WriteXml(sw);
                    StrMedia = sw.ToString();
                }

                DTabProperty.TableName = "Table";
                string StrProperty = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabProperty.WriteXml(sw);
                    StrProperty = sw.ToString();
                }

                DTabRanking.TableName = "Table";
                string StrRanking = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabRanking.WriteXml(sw);
                    StrProperty = sw.ToString();
                }

                DTabReview.TableName = "Table";
                string StrReview = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabReview.WriteXml(sw);
                    StrProperty = sw.ToString();
                }

                Property.MATERIAL = StrMaterial;
                Property.COLLECTION = StrCollection;
                Property.MEDIA = StrMedia;
                Property.PROPERTY = StrProperty;
                Property.RANKING = StrRanking;
                Property.REVIEW = StrReview;

                Property = ObjMast.Save(Property);

                string StrReturnDesc = Property.ReturnMessageDesc;

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    Property.STYLE_ID = Guid.Parse(Val.ToString(Property.ReturnValue));
                }

                this.Cursor = Cursors.Default;
                Global.Message(StrReturnDesc);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    BtnAdd_Click(null, null);
                    Fill();
                    
                    if (GrdDetStyle.RowCount > 1)
                    {
                        GrdDetStyle.FocusedRowHandle = GrdDetStyle.RowCount - 1;
                    }
                }
                else
                {
                    txtStyleNo.Focus();
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        public void Fill()
        {

            DataTable DTab = ObjMast.Fill();
            MainGrdStyle.DataSource = DTab;
            GrdDetStyle.RefreshData();
            GrdDetStyle.BestFitColumns();

            lblTotal.Text = "Total Record : " + DTab.Rows.Count.ToString();

            MainGrdStyle.Refresh();

            FillStyleDetailSave(Val.ToString(txtStyleNo.Tag).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtStyleNo.Tag)));

        }

        public void FillProperty()
        {
            

            this.Cursor = Cursors.WaitCursor;
            DataTable DTab = ObjMast.FillPro();

            //DTabProperty = DTab.Tables[0];
            MainGrdProperty.DataSource = DTab;
            GrdProperty.RefreshData();

            this.Cursor = Cursors.Default;

        }

        public void FetchValue(DataRow DR)
        {
            txtStyleNo.Tag = Val.ToString(txtStyleNo.Tag).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtStyleNo.Tag));
            txtStyleNo.Text = Val.ToString(DR["STYLENO"]);

            //txtLocation.Tag = Val.ToString(DR["LOCATION_ID"]);
            //txtLocation.Text = Val.ToString(DR["LOCATION"]);

            //CmbCollection.Tag = Val.ToString(DR["COLLECTION_ID"]);
            //CmbCollection.Text = Val.ToString(DR["COLLECTION"]);

            //CmbAdditionalProperty.Tag = Val.ToString(DR["PROPERTY_ID"]);
            //CmbAdditionalProperty.Text = Val.ToString(DR["PROPERTYVALUE"]);

            //txtProductCatagory.Tag = Val.ToString(DR["PRODUCTCATEGORY_ID"]);
            txtProductCatagory.Text = Val.ToString(DR["PRODUCTCATEGORYNAME"]);

            //txtProductSubCatagory.Tag = Val.ToString(DR["PRODUCTSUBCATEGORY_ID"]);
            txtProductSubCatagory.Text = Val.ToString(DR["PRODUCTSUBCATEGORYNAME"]);


            txtShortDescription.Text = Val.ToString(DR["SHORTDESCRIPTION"]);
            txtLongDescription.Text = Val.ToString(DR["LONGDESCRIPTION"]);

            txtGrossWt.Text = Val.ToString(DR["GROSSWT"]);
            txtNetWt.Text = Val.ToString(DR["NETWT"]);
            //txtMetalWeight.Text = Val.ToString(DR["METALWT"]);
            //txtDiamondPcs.Text = Val.ToString(DR["DIAMONDPCS"]);
            //txtDiamondWeight.Text = Val.ToString(DR["DIAMONDWT"]);
            //txtColorPcs.Text = Val.ToString(DR["COLORPCS"]);
            //txtColorWeight.Text = Val.ToString(DR["COLORWT"]);



            //txtLocation.Text = Val.ToString(DR["LOCATIONNAME"]);
            //txtLocation.Tag = Val.ToString(DR["LOCATION_ID"]);

            //CmbStatus.SelectedItem = Val.ToString(DR["WEBSTATUS"]);

            //CmbCollection.SetEditValue(DR["JEWELLERYCOLLECTION_ID"]);
            //txtShortDescription.Text = Val.ToString(DR["JEWELLERYTITLE"]);
            //txtLongDescription.Text = Val.ToString(DR["JEWELLERYDESCRIPTION"]);
            //txtRemark.Text = Val.ToString(DR["REMARK"]);

            //ChkActive.Checked = Val.ToBoolean(DR["ISACTIVE"]);

            FillStyleDetailSave(Val.ToString(txtStyleNo.Text).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtStyleNo.Tag)));  //06-04-2019

            xtraTabControl1.SelectedTabPageIndex = 0;
            txtStyleNo.Focus();
        }
        public void FillStyleDetailSave(Guid pGuidStyle_Id)
        {
            DataSet DsDetailInfo = ObjMast.GetStyleDetailDInfoata(pGuidStyle_Id);

            //DataSet DsDetailInfo = ObjMast.GetStyleDetailDInfoata(pGuidStyle_Id);

            //Style Details
            DTabStyle = DsDetailInfo.Tables[0].Copy();
            DTabStyle.Rows.Add(DTabStyle.NewRow());
            MainGrdStyle.DataSource = DTabStyle;
            MainGrdStyle.RefreshDataSource();
            GrdDetStyle.BestFitColumns();

            //Style Material Details
            DTabMaterial = DsDetailInfo.Tables[1].Copy();
            DTabMaterial.Rows.Add(DTabMaterial.NewRow());
            MainGrdMaterial.DataSource = DTabMaterial;
            MainGrdMaterial.RefreshDataSource();
            GrdMaterial.BestFitColumns();

            //Property Details
            DTabProperty = DsDetailInfo.Tables[2].Copy();
            DTabProperty.Rows.Add(DTabProperty.NewRow());
            MainGrdProperty.DataSource = DTabProperty;
            MainGrdProperty.RefreshDataSource();
            GrdProperty.BestFitColumns();

            //Style Media Details
            DTabMedia = DsDetailInfo.Tables[3].Copy();
            DTabMedia.Rows.Add(DTabMedia.NewRow());
            MainGrdMedia.DataSource = DTabMedia;
            MainGrdMedia.RefreshDataSource();
            GrdMedia.BestFitColumns();

            //Style Collection Details
            DTabCollection = DsDetailInfo.Tables[4].Copy();
            DTabCollection.Rows.Add(DTabCollection.NewRow());
            MainGrdCollection.DataSource = DTabCollection;
            MainGrdCollection.RefreshDataSource();
            GrdCollection.BestFitColumns();

            //Style Collection Details
            DTabRanking = DsDetailInfo.Tables[5].Copy();
            DTabRanking.Rows.Add(DTabRanking.NewRow());
            MainGrdRanking.DataSource = DTabRanking;
            MainGrdRanking.RefreshDataSource();
            GrdRanking.BestFitColumns();

            //Style Collection Details
            DTabReview = DsDetailInfo.Tables[6].Copy();
            DTabReview.Rows.Add(DTabReview.NewRow());
            MainGridReview.DataSource = DTabReview;
            MainGridReview.RefreshDataSource();
            GrdReview.BestFitColumns();

        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            StyleMasterProperty Property = new StyleMasterProperty();
            try
            {

                if (Global.Confirm("Are Your Sure To Delete The Record ?") == System.Windows.Forms.DialogResult.No)
                    return;


                //Property.LEDGER_ID = Val.ToInt64(txtLedgerID.Text);
                Property.STYLE_ID = Guid.Parse(Val.ToString(txtStyleNo.Tag));
                Property = ObjMast.Delete(Property);
                Global.Message(Property.ReturnMessageDesc);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    BtnAdd_Click(null, null);
                    Fill();
                }
                else
                {
                    txtProductSubCatagory.Focus();
                }

            }
            catch (System.Exception ex)
            {
                Global.MessageToster(ex.Message);
            }
            Property = null;
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GrdDet_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                return;
            }

            if (e.Clicks == 2)
            {
                this.Cursor = Cursors.WaitCursor;
                DataRow DR = GrdDetStyle.GetDataRow(e.RowHandle);
                FetchValue(DR);
                DR = null;
                this.Cursor = Cursors.Default;
            }

        }
        private void GrdDet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataRow DR = GrdDetStyle.GetFocusedDataRow();
                FetchValue(DR);
                DR = null;
            }
        }
        public void ChangeTextBoxPlaceHolder(TextBox TxtBox, string StrCheckText, string StrChangeText, Color ColTextColor)
        {

            if (Val.ToString(TxtBox.Text) == StrCheckText)
            {
                TxtBox.Text = StrChangeText;
                TxtBox.ForeColor = ColTextColor;
            }
            if (TxtBox.Text == "")
            {
                TxtBox.ForeColor = Color.Black;
            }
            //if (TxtBox.ForeColor == Color.Silver)
            //    TxtBox.TextAlign = HorizontalAlignment.Center;
            //else
            //    TxtBox.TextAlign = HorizontalAlignment.Left;
            TxtBox = null;
        }
       

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("StyleList", GrdDetStyle);
        }
        
        private void BtnBestFit_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            GrdDetStyle.BestFitColumns();
            this.Cursor = Cursors.Default;
        }

        

        

        private void txtProductCatagory_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PRODUCTCATEGORYCODE,PRODUCTCATEGORYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PRODUCT_CATEGORY);

                    FrmSearch.mStrColumnsToHide = "PRODUCTCATEGORY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtProductCatagory.Text = Val.ToString(FrmSearch.DRow["PRODUCTCATEGORYNAME"]);
                        txtProductCatagory.Tag = Val.ToString(FrmSearch.DRow["PRODUCTCATEGORY_ID"]);
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

        private void txtProductSubCatagory_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PRODUCTSUBCATEGORYCODE,PRODUCTSUBCATEGORYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PRODUCT_SUBCATEGORY);

                    FrmSearch.mStrColumnsToHide = "PRODUCTSUBCATEGORY_ID,PRODUCTCATEGORY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtProductSubCatagory.Text = Val.ToString(FrmSearch.DRow["PRODUCTSUBCATEGORYNAME"]);
                        txtProductSubCatagory.Tag = Val.ToString(FrmSearch.DRow["PRODUCTSUBCATEGORY_ID"]);
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

        private void txtLocation_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "LOCATIONCODE,LOCATIONNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LOCATION);

                    FrmSearch.mStrColumnsToHide = "LOCATION_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtLocation.Text = Val.ToString(FrmSearch.DRow["LOCATIONNAME"]);
                        txtLocation.Tag = Val.ToString(FrmSearch.DRow["LOCATION_ID"]);
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
        private void RepMaterialType_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "MATERIALTYPECODE,MATERIALTYPENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_MATERIALTYPE);

                    FrmSearch.mStrColumnsToHide = "MATERIALTYPE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdMaterial.SetFocusedRowCellValue("MATERIALTYPENAME", Val.ToString(FrmSearch.DRow["MATERIALTYPENAME"]));
                        GrdMaterial.SetFocusedRowCellValue("MATERIALTYPE_ID", Val.ToString(FrmSearch.DRow["MATERIALTYPE_ID"]));
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

        private void RepMaterialColor_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "COLORCODE,COLORNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_MATERIALCOLOR);

                    FrmSearch.mStrColumnsToHide = "COLOR_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdMaterial.SetFocusedRowCellValue("COLORNAME", Val.ToString(FrmSearch.DRow["COLORNAME"]));
                        GrdMaterial.SetFocusedRowCellValue("COLOR_ID", Val.ToString(FrmSearch.DRow["COLOR_ID"]));
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

        private void RepMaterialName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "MATERIALCODE,MATERIALNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BOMST_Style().GetMaterialTypeData(Val.ToInt(GrdMaterial.GetFocusedRowCellValue("MATERIALTYPE_ID")));
                    FrmSearch.mStrColumnsToHide = "MATERIALTYPE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdMaterial.SetFocusedRowCellValue("MATERIALNAME", Val.ToString(FrmSearch.DRow["MATERIALNAME"]));
                        GrdMaterial.SetFocusedRowCellValue("MATERIAL_ID", Val.ToString(FrmSearch.DRow["MATERIAL_ID"]));
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
            //try
            //{
            //    if (Global.OnKeyPressEveToPopup(e))
            //    {
            //        FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
            //        FrmSearch.mStrSearchField = "MATERIALCODE,MATERIALNAME";
            //        FrmSearch.mStrSearchText = e.KeyChar.ToString();
            //        this.Cursor = Cursors.WaitCursor;
            //        FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_MATERIAL);

            //        FrmSearch.mStrColumnsToHide = "MATERIAL_ID";
            //        this.Cursor = Cursors.Default;
            //        FrmSearch.ShowDialog();
            //        e.Handled = true;
            //        if (FrmSearch.DRow != null)
            //        {
            //            GrdMaterial.SetFocusedRowCellValue("MATERIALNAME", Val.ToString(FrmSearch.DRow["MATERIALNAME"]));
            //            GrdMaterial.SetFocusedRowCellValue("MATERIAL_ID", Val.ToString(FrmSearch.DRow["MATERIAL_ID"]));
            //        }
            //        FrmSearch.Hide();
            //        FrmSearch.Dispose();
            //        FrmSearch = null;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Global.Message(ex.Message);
            //}
        }



        private void RepDiamondRemark_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DataRow dr = GrdCollection.GetFocusedDataRow();
                    if (Val.Val(dr["CARAT"]) != 0 && GrdCollection.IsLastRow)
                    {
                        DataRow Dr = DTabCollection.NewRow();
                        DTabCollection.Rows.Add(Dr);

                        GrdCollection.Focus();
                        GrdCollection.FocusedRowHandle = GrdCollection.RowCount - 1;
                        GrdCollection.FocusedColumn = GrdCollection.Columns["STONETYPE"];
                    }
                    else if (GrdCollection.IsLastRow)
                    {
                        BtnSave.Focus();
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtRemark_Validated(object sender, EventArgs e)
        {
            //MainGrdReview.SelectedTabPageIndex = 0;
            GrdMaterial.Focus();
            GrdMaterial.FocusedRowHandle = 0;
            GrdMaterial.FocusedColumn = GrdMaterial.Columns["METALNAME"];

        }

        private void BtnAddCollectionNewRow_Click(object sender, EventArgs e)
        {
            DataRow Dr = DTabCollection.NewRow();
            DTabCollection.Rows.Add(Dr);
        }

        private void BtnAddMaterialNewRow_Click(object sender, EventArgs e)
        {
            DataRow Dr = DTabMaterial.NewRow();
            DTabMaterial.Rows.Add(Dr);
        }

        private void BtnAddMediaNewRow_Click(object sender, EventArgs e)
        {
            DataRow Dr = DTabMedia.NewRow();
            DTabMedia.Rows.Add(Dr);
        }

        private void BtnAddPropertyNewRow_Click(object sender, EventArgs e)
        {
            DataRow Dr = DTabProperty.NewRow();
            DTabProperty.Rows.Add(Dr);
        }

        private void BtnAddRankingNewRow_Click(object sender, EventArgs e)
        {
            DataRow Dr = DTabRanking.NewRow();
            DTabRanking.Rows.Add(Dr);
        }

        private void BtnAddReviewNewRow_Click(object sender, EventArgs e)
        {
            DataRow Dr = DTabReview.NewRow();
            DTabReview.Rows.Add(Dr);
        }

        private void BtnMetalDelete_Click(object sender, EventArgs e)
        {
            if (Global.Confirm("Are You Sure You Want To Delete ?") == DialogResult.Yes)
            {
                GrdMaterial.DeleteRow(GrdMaterial.FocusedRowHandle);
                DTabMaterial.AcceptChanges();
                GrdMaterial.RefreshData();

                Global.Confirm("Press Save Button To Apply Effect");
            }
        }

        private void BtnCollectionDelete_Click(object sender, EventArgs e)
        {
            if (Global.Confirm("Are You Sure You Want To Delete ?") == DialogResult.Yes)
            {
                GrdCollection.DeleteRow(GrdCollection.FocusedRowHandle);
                DTabCollection.AcceptChanges();
                GrdCollection.RefreshData();

                Global.Confirm("Press Save Button To Apply Effect");
            }
        }

        private void BtnMediaDelete_Click(object sender, EventArgs e)
        {
            if (Global.Confirm("Are You Sure You Want To Delete ?") == DialogResult.Yes)
            {
                GrdMedia.DeleteRow(GrdMedia.FocusedRowHandle);
                DTabMedia.AcceptChanges();
                GrdMedia.RefreshData();

                Global.Confirm("Press Save Button To Apply Effect");
            }
        }

        private void BtnPropertyDelete_Click(object sender, EventArgs e)
        {
            if (Global.Confirm("Are You Sure You Want To Delete ?") == DialogResult.Yes)
            {
                GrdProperty.DeleteRow(GrdProperty.FocusedRowHandle);
                DTabProperty.AcceptChanges();
                GrdProperty.RefreshData();

                Global.Confirm("Press Save Button To Apply Effect");
            }
        }

        private void BtnUploadImage_Click(object sender, EventArgs e)
        {

            //var request = WebRequest.Create("http://www.gravatar.com/avatar/6810d91caff032b202c50701dd3af745?d=identicon&r=PG");

            //using (var response = request.GetResponse())
            //using (var stream = response.GetResponseStream())
            //{
            //    Image im1 = Image.FromStream(stream);
            //    Image im2 = Image.FromStream(stream);
            //    Image im3 = Image.FromStream(stream);
            //    Image im4 = Image.FromStream(stream);
            //    Image im5 = Image.FromStream(stream);
            //    Image im6 = Image.FromStream(stream);

            //    ImageGallaryControl.Gallery.ItemImageLayout = ImageLayoutMode.ZoomInside;
            //    ImageGallaryControl.Gallery.ImageSize = new Size(120, 90);
            //    ImageGallaryControl.Gallery.ShowItemText = true;

            //    GalleryItemGroup group1 = new GalleryItemGroup();
            //    group1.Caption = "Cars";
            //    ImageGallaryControl.Gallery.Groups.Add(group1);

            //    GalleryItemGroup group2 = new GalleryItemGroup();
            //    group2.Caption = "People";
            //    ImageGallaryControl.Gallery.Groups.Add(group2);

            //    group1.Items.Add(new GalleryItem(im1, "BMW", ""));
            //    group1.Items.Add(new GalleryItem(im2, "Ford", ""));
            //    group1.Items.Add(new GalleryItem(im3, "Mercedec-Benz", ""));

            //    group2.Items.Add(new GalleryItem(im4, "Anne Dodsworth", ""));
            //    group2.Items.Add(new GalleryItem(im5, "Hanna Moos", ""));
            //    group2.Items.Add(new GalleryItem(im6, "Janet Leverling", ""));
            //}

            

        }

        private void xtraTabPage5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtStyleNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void RepMaterialShape_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "SHAPECODE,SHAPENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_MATERIALSHAPE);

                    FrmSearch.mStrColumnsToHide = "SHAPE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdMaterial.SetFocusedRowCellValue("SHAPENAME", Val.ToString(FrmSearch.DRow["SHAPENAME"]));
                        GrdMaterial.SetFocusedRowCellValue("SHAPE_ID", Val.ToString(FrmSearch.DRow["SHAPE_ID"]));
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

        private void RepMaterialQuality_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
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
                        GrdMaterial.SetFocusedRowCellValue("QUALITYNAME", Val.ToString(FrmSearch.DRow["CLARITYNAME"]));
                        GrdMaterial.SetFocusedRowCellValue("QUALITY_ID", Val.ToString(FrmSearch.DRow["CLARITY_ID"]));
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

        private void RepCollection_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdCollection.FocusedRowHandle < 0)
                    return;

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "COLLECTIONCODE,COLLECTIONNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BOMST_Style().GetCollectionData();
                    FrmSearch.mStrColumnsToHide = "COLLECTION_ID";
                    //FrmSearch.ColumnHeaderCaptions = "PARACODE=Code,PARANAME=Name,SHORTNAME=Short";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdCollection.SetFocusedRowCellValue("COLLECTIONNAME", Val.ToString(FrmSearch.DRow["COLLECTIONNAME"]));
                        GrdCollection.SetFocusedRowCellValue("COLLECTION_ID", Val.ToString(FrmSearch.DRow["COLLECTION_ID"]));
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

        private void RepPropertyValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PROPERTYCODE,PROPERTYVALUE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_ADDITIONALPROPERTY);

                    FrmSearch.mStrColumnsToHide = "PROPERTY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdProperty.SetFocusedRowCellValue("PROPERTYVALUE", Val.ToString(FrmSearch.DRow["PROPERTYVALUE"]));
                        GrdProperty.SetFocusedRowCellValue("PROPERTY_ID", Val.ToString(FrmSearch.DRow["PROPERTY_ID"]));
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

        private void groupControl8_Paint(object sender, PaintEventArgs e)
        {

        }

        private String[] GetExcelSheetNames(string excelFile)
        {
            OleDbConnection objConn = null;
            System.Data.DataTable dt = null;

            try
            {
                String connString = "";
                //String connString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                //  "Data Source=" + excelFile + ";Extended Properties=Excel 12.0;";
                if (Path.GetExtension(excelFile).Equals(".xls"))//for 97-03 Excel file
                {
                    connString = "Provider=Microsoft.ACE.OLEDB.4.0;" +
                      "Data Source=" + excelFile + ";Extended Properties=Excel 8.0;";                   
                }
                //else if (Path.GetExtension(filePath).Equals(".xlsx"))  //for 2007 Excel file
                else
                {
                    connString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                                 "Data Source=" + excelFile + ";Extended Properties=Excel 12.0;";
                }

                objConn = new OleDbConnection(connString);
                objConn.Open();
                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                List<string> sheets = new List<string>();
                if (dt == null)
                {
                    return null;
                }
                String[] excelSheets = new String[dt.Rows.Count];
                CmbSheetname.Items.Clear(); //ADD:KULDEEP[24/05/18]
                foreach (DataRow row in dt.Rows)
                {
                    string sheetName = (string)row["TABLE_NAME"];
                    sheets.Add(sheetName);
                    CmbSheetname.Items.Add(sheetName);
                }

                return excelSheets;
            }
            catch (Exception ex)
            {
                Global.Message(ex.ToString());

                return null;
            }
            finally
            {
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
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
                    txtFileName.Text = OpenFileDialog.FileName;

                    string extension = Path.GetExtension(txtFileName.Text.ToString());
                    string destinationPath = Application.StartupPath + @"\StoneFiles\" + Path.GetFileName(txtFileName.Text);
                    destinationPath = destinationPath.Replace(extension, ".xlsx");
                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }
                    File.Copy(txtFileName.Text, destinationPath);

                    GetExcelSheetNames(destinationPath);

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
                Global.Message(ex.ToString() + "InValid File Name");
            }
        }

        private void lblSampleExcelFile_Click(object sender, EventArgs e)
        {
            try
            {
                string StrFilePathDestination = "";
                StrFilePathDestination = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\StyleUpload_Format" + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.Day.ToString() + ".xlsx";
                if (File.Exists(StrFilePathDestination))
                {
                    File.Delete(StrFilePathDestination);
                }
                File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\Format\\StyleUpload_Format.xlsx", StrFilePathDestination);

                System.Diagnostics.Process.Start(StrFilePathDestination, "CMD");
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        public static DataTable GetDataTableFromExcel(string path, bool hasHeader = true)
        {
            using (var pck = new OfficeOpenXml.ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                DataTable tbl = new DataTable();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    if (Convert.ToString(firstRowCell.Text).Equals(string.Empty))
                        continue;

                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        if (Convert.ToString(cell.Text).Equals(string.Empty))
                            continue;

                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
                return tbl;
            }
        }

        private void BtnCalculate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFileName.Text == "" || txtFileName.Text.Trim().Equals(string.Empty))
                {
                    Global.Message("Please Select Excel File To Upload");
                    txtFileName.Focus();
                    return;
                }
                //txtAddless1.Text = Val.ToString(0);
                //txtAddLess2.Text = Val.ToString(0);

                this.Cursor = Cursors.WaitCursor;
                BtnBrowse.Enabled = false;

                DtabExcelData.Rows.Clear();
                string extension = Path.GetExtension(txtFileName.Text.ToString());
                string destinationPath = Application.StartupPath + @"\StoneFiles\" + Path.GetFileName(txtFileName.Text);
                destinationPath = destinationPath.Replace(extension, ".xlsx");
                if (File.Exists(destinationPath))
                {
                    File.Delete(destinationPath);
                }
                File.Copy(txtFileName.Text, destinationPath);

                DtabExcelData = GetDataTableFromExcel(destinationPath);

                if (File.Exists(destinationPath))
                {
                    File.Delete(destinationPath);
                }


                for (int Intcol = 0; Intcol < DtabExcelData.Columns.Count; Intcol++)
                {
                    if (Val.ToString("Sr. No,Srno,SRNO,Sr").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("SR");

                    if (Val.ToString("StyleNo,Style No,STYLE NO").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("STYLENO");

                    if (Val.ToString("ProductCategory").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("PRODUCTCATEGORYNAME");

                    if (Val.ToString("ProductSubCategory").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("PRODUCTSUBCATEGORYNAME");

                    if (Val.ToString("Location").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("LOCATION");

                    if (Val.ToString("Collection").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("COLLECTION");

                    if (Val.ToString("GrossWt").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("GROSSWT");

                    if (Val.ToString("NetWt").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("NETWT");

                    if (Val.ToString("Discount Price,DiscountPrice,Discounted Price").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("DISCOUNTPRICE");

                    if (Val.ToString("Sale Price,SalePrice,Actual Sale Price").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("SALEPRICE");

                    if (Val.ToString("Material Type,MaterialType").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("MATERIALTYPENAME");

                    if (Val.ToString("Material").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("MATERIALNAME");

                    if (Val.ToString("Shape").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("SHAPENAME");

                    if (Val.ToString("Quality").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("QUALITY");

                    if (Val.ToString("Color").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("COLOR");

                    if (Val.ToString("KT").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("KT");

                    if (Val.ToString("MMSize").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("MMSIZE");

                    if (Val.ToString("Short Description").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("SHORTDESCRIPTION");

                    if (Val.ToString("Long Description").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("LONGDESCRIPTION");

                    if (Val.ToString("Similar Styles").ToUpper().Split(',').Contains(Val.ToString(DtabExcelData.Columns[Intcol].ColumnName.ToUpper())))
                        DtabExcelData.Columns[Intcol].ColumnName = Val.ToString("SIMILLARSTYLE");
                }




                MainGrdStyle.DataSource = DtabExcelData;

                this.Cursor = Cursors.Default;
                BtnBrowse.Enabled = true;
                //BtnVerify.Enabled = true;
                
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
                BtnBrowse.Enabled = true;
                //BtnVerify.Enabled = false;
            }
            
        }

        private void GrdDetStyle_CellMerge(object sender, DevExpress.XtraGrid.Views.Grid.CellMergeEventArgs e)
        {
            //MergeOnStr = "STYLENO"; //,PRODUCTCATEGORYNAME,PRODUCTSUBCATEGORYNAME,LOCATION,COLLECTION
            //MergeOn = "STYLENO";
            //if (MergeOnStr.Contains(e.Column.FieldName))
            //{
            //    string val1 = Val.ToString(GrdDetStyle.GetRowCellValue(e.RowHandle1, GrdDetStyle.Columns[MergeOn]));
            //    string val2 = Val.ToString(GrdDetStyle.GetRowCellValue(e.RowHandle2, GrdDetStyle.Columns[MergeOn]));
            //    if (val1 == val2)
            //        e.Merge = true;
            //    e.Handled = true;
            //}

            MergeOnStr = "STYLENO,PRODUCTCATEGORYNAME,PRODUCTSUBCATEGORYNAME";
            MergeOn = "STYLENO";
            if (MergeOnStr.Contains(e.Column.FieldName))
            {
                string val1 = Val.ToString(GrdDetStyle.GetRowCellValue(e.RowHandle1, GrdDetStyle.Columns[MergeOn]));
                string val2 = Val.ToString(GrdDetStyle.GetRowCellValue(e.RowHandle2, GrdDetStyle.Columns[MergeOn]));
                if (val1 == val2)
                    e.Merge = true;
                e.Handled = true;
            }
        }
        
        private void BtnCollectionDelete_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.KeyCode == Keys.Enter)
                //{
                    DataRow dr = GrdCollection.GetFocusedDataRow();
                    if (!Val.ToString(dr["COLLECTIONNAME"]).Equals(string.Empty) && GrdCollection.IsLastRow)
                    {
                        DTabCollection.Rows.Add(DTabCollection.NewRow());
                    }
                    else if (GrdCollection.IsLastRow)
                    {
                        BtnSave.Focus();
                        e.Handled = true;
                    }
               // }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void btnMediaDelete_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.KeyCode == Keys.Enter)
                //{
                    DataRow dr = GrdMedia.GetFocusedDataRow();
                    if (!Val.ToString(dr["MEDIATYPE"]).Equals(string.Empty) && !Val.ToString(dr["MEDIAURL"]).Equals(string.Empty) && GrdMedia.IsLastRow)
                    {
                        DTabMedia.Rows.Add(DTabMedia.NewRow());
                    }
                    else if (GrdMedia.IsLastRow)
                    {
                        BtnSave.Focus();
                        e.Handled = true;
                    }
                //}
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void BtnPropertyDelete_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DataRow dr = GrdProperty.GetFocusedDataRow();
                    if (!Val.ToString(dr["PROPERTYVALUE"]).Equals(string.Empty) && GrdProperty.IsLastRow)
                    {
                        DTabProperty.Rows.Add(DTabProperty.NewRow());
                    }
                    else if (GrdProperty.IsLastRow)
                    {
                        BtnSave.Focus();
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        

        private void BtnRankingDelete_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.KeyCode == Keys.Enter)
                //{
                    DataRow dr = GrdRanking.GetFocusedDataRow();
                    if (!Val.ToString(dr["Rank"]).Equals(string.Empty) && GrdRanking.IsLastRow)
                    {
                        DTabRanking.Rows.Add(DTabRanking.NewRow());
                    }
                    else if (GrdRanking.IsLastRow)
                    {
                        BtnSave.Focus();
                        e.Handled = true;
                    }
                //}
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }           
        }

        private void BtnRankingDelete_Click(object sender, EventArgs e)
        {
            if (Global.Confirm("Are You Sure You Want To Delete ?") == DialogResult.Yes)
            {
                GrdRanking.DeleteRow(GrdRanking.FocusedRowHandle);
                DTabRanking.AcceptChanges();
                GrdRanking.RefreshData();

                Global.Confirm("Press Save Button To Apply Effect");
            }
        }

        private void BtnReviewDelete_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                    DataRow dr = GrdReview.GetFocusedDataRow();
                    if (!Val.ToString(dr["Review"]).Equals(string.Empty) && GrdReview.IsLastRow)
                    {
                        DTabReview.Rows.Add(DTabReview.NewRow());
                    }
                    else if (GrdReview.IsLastRow)
                    {
                        BtnSave.Focus();
                        e.Handled = true;
                    }
                
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void BtnReviewDelete_Click(object sender, EventArgs e)
        {
            if (Global.Confirm("Are You Sure You Want To Delete ?") == DialogResult.Yes)
            {
                GrdReview.DeleteRow(GrdReview.FocusedRowHandle);
                DTabReview.AcceptChanges();
                GrdReview.RefreshData();

                Global.Confirm("Press Save Button To Apply Effect");
            }
        }

        private void xtraTabPage9_Paint(object sender, PaintEventArgs e)
        {

        }

        private void BtnMaterialDelete_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                DataRow dr = GrdMaterial.GetFocusedDataRow();
                if (!Val.ToString(dr["WEIGHT"]).Equals(string.Empty) && GrdMaterial.IsLastRow)
                {
                    DTabMaterial.Rows.Add(DTabMaterial.NewRow());
                }
                else if (GrdMaterial.IsLastRow)
                {
                    BtnSave.Focus();
                    e.Handled = true;
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
    }
}
