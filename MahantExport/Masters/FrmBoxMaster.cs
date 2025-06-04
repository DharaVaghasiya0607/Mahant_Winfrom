using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using MahantExport.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using BusLib.Transaction;

namespace MahantExport.Masters
{
    public partial class FrmBoxMaster  : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        DataTable DTabBox = new DataTable();
        BOFormPer ObjPer = new BOFormPer();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_ShapeMaster ObjMast = new BOMST_ShapeMaster();   
        
        #region Property

        public FrmBoxMaster()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            try
            {
                ObjPer.GetFormPermission(Val.ToString(this.Tag));
                Val.FormGeneralSetting(this);
                AttachFormDefaultEvent();
                //ObjPer.GetFormPermission(Val.ToString(this.Tag));
                //if (ObjPer.ISVIEW == false)
                //{
                //    Global.MessageError(BusLib.TPV.BOMessage.ViewDeniedMsg);
                //    return;
                //}

                BtnContinue_Click(null, null);
                Clear();
                string Str = new BOTRN_StockUpload().GetGridLayout(this.Name, GrdDet.Name);

                if (Str != "")
                {
                    byte[] byteArray = Encoding.ASCII.GetBytes(Str);
                    MemoryStream stream = new MemoryStream(byteArray);
                    GrdDet.RestoreLayoutFromStream(stream);
                }
                this.Show();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        public void ShowForm(string pStrBoxName)
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            this.Show();
            txtBoxName.Text = pStrBoxName;
            txtSearchBoxName.Text = pStrBoxName;
            BtnSearch_Click(null, null);

            //DataRow DRows = ObjMast.GetBox_ID(Val.ToString(txtBoxName.Text));//Comment By Gunjan:16/08/2023
            //txtBoxName.Tag = Val.ToInt(DRows["BOX_ID"]);//Comment By Gunjan:16/08/2023

            DataTable dtData = (DataTable)MainGridDetailView.DataSource;

            if (dtData.Rows.Count != 0 || dtData != null)
            {
                txtBoxName.Tag = Val.ToInt32(dtData.Rows[0]["BOX_ID"]);
                txtBoxName.Text = Val.ToString(dtData.Rows[0]["BOXNAME"]);
                txtLot.Tag = Val.ToInt32(dtData.Rows[0]["LOT_ID"]);
                txtLot.Text = Val.ToString(dtData.Rows[0]["LOTNAME"]);
                txtClarity.Tag = Val.ToInt32(dtData.Rows[0]["CLARITY_ID"]);
                txtClarity.Text = Val.ToString(dtData.Rows[0]["CLARITYNAME"]);
                txtColor.Tag = Val.ToInt32(dtData.Rows[0]["COLOR_ID"]);
                txtColor.Text = Val.ToString(dtData.Rows[0]["COLORNAME"]);
                txtShade.Tag = Val.ToInt32(dtData.Rows[0]["COLORSHADE_ID"]);
                txtShade.Text = Val.ToString(dtData.Rows[0]["COLORSHADENAME"]);
                txtSize.Tag = Val.ToInt32(dtData.Rows[0]["SIZE_ID"]);
                txtSize.Text = Val.ToString(dtData.Rows[0]["SIZENAME"]);
                txtShape.Tag = Val.ToInt32(dtData.Rows[0]["SHAPE_ID"]);
                txtShape.Text = Val.ToString(dtData.Rows[0]["SHAPENAME"]);
                txtCutNo.Text = Val.ToString(dtData.Rows[0]["CUTNO"]);
                txtRemark.Text = Val.ToString(dtData.Rows[0]["REMARK"]);
                if (Val.ToBoolean(dtData.Rows[0]["ISACTIVE"]) == true)
                {
                    RbtYes.Checked = true;
                }
                else
                {
                    RbtNo.Checked = true;
                }
            }
            
            DTabBox = ObjMast.FillDetail(Val.ToString(txtBoxName.Tag));           
            MainGrid.DataSource = DTabBox;

            BtnDelete.Enabled = false;
          
        }

        public void AttachFormDefaultEvent()
        {
            try
            {
                ObjFormEvent.mForm = this;
                ObjFormEvent.FormKeyPress = false;
                ObjFormEvent.FormResize = true;
                ObjFormEvent.FormClosing = true;
                ObjFormEvent.FormKeyDown = true;
                ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
                ObjFormEvent.ObjToDisposeList.Add(ObjMast);
                ObjFormEvent.ObjToDisposeList.Add(Val);
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
            try
            {
                txtBoxName.Tag = string.Empty;
                txtBoxName.Text = "";
                txtLot.Text = string.Empty;
                txtColor.Text = string.Empty;
                txtClarity.Text = string.Empty;
                txtSize.Text = string.Empty;
                txtShape.Text = string.Empty;
                txtBoxName.Tag = string.Empty;
                txtLot.Tag = string.Empty;
                txtColor.Tag = string.Empty;
                txtClarity.Tag = string.Empty;
                txtSize.Tag = string.Empty;
                txtShape.Tag = string.Empty;
                txtCutNo.Text = string.Empty;
                txtRemark.Text = string.Empty;
                txtShade.Text = string.Empty;
                txtShade.Tag = string.Empty;
                DTabBox.Rows.Clear();
                txtBoxName.Focus();
                
                //Fill();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        //public void Fill()//Comment By Gunjan:17/05/2023
        //{
        //    try
        //    {
        //        DataTable dt= ObjMast.BoxGetData();
        //        MainGridDetailView.DataSource = dt;
        //        if (dt.Rows.Count > 0)
        //        {
        //            GridDetail.BestFitColumns();
        //        }                      
        //    }
        //    catch (Exception ex)
        //    {
        //        Global.Message(ex.Message);
        //    }
        //}

        public void FetchValue(DataRow DR)
        {
            try
            {
                txtBoxName.Tag = Val.ToInt32(DR["BOX_ID"]);
                txtBoxName.Text = Val.ToString(DR["BOXNAME"]);
                txtLot.Tag = Val.ToInt32(DR["LOT_ID"]);
                txtLot.Text = Val.ToString(DR["LOTNAME"]);
                txtClarity.Tag = Val.ToInt32(DR["CLARITY_ID"]);
                txtClarity.Text = Val.ToString(DR["CLARITYNAME"]);
                txtColor.Tag = Val.ToInt32(DR["COLOR_ID"]);
                txtColor.Text = Val.ToString(DR["COLORNAME"]);
                txtShade.Tag = Val.ToInt32(DR["COLORSHADE_ID"]);
                txtShade.Text = Val.ToString(DR["COLORSHADENAME"]);
                txtSize.Tag = Val.ToInt32(DR["SIZE_ID"]);
                txtSize.Text = Val.ToString(DR["SIZENAME"]);
                txtShape.Tag = Val.ToInt32(DR["SHAPE_ID"]);
                txtShape.Text = Val.ToString(DR["SHAPENAME"]);
                txtCutNo.Text = Val.ToString(DR["CUTNO"]);
                txtRemark.Text = Val.ToString(DR["REMARK"]);
                if (Val.ToBoolean(DR["ISACTIVE"]) == true)
                {
                    RbtYes.Checked = true;
                }
                else
                {
                    RbtNo.Checked = true;
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }
        #endregion

        #region Validation

        private bool ValSave()
        {
            try
            {

                if (txtBoxName.Text.Trim().Length == 0)
                {
                    Global.Message("Box Name Is Required");
                    txtBoxName.Focus();
                    return false;
                }
                else if (txtLot.Text.Trim().Length == 0)
                {
                    Global.Message("Lot Name Is Required");
                    txtLot.Focus();
                    return false;
                }
                else if (txtClarity.Text.Trim().Length == 0)
                {
                    Global.Message("Clarity Name Is Required");
                    txtClarity.Focus();
                    return false;
                }
                else if (txtColor.Text.Trim().Length == 0)
                {
                    Global.Message("Color Name Is Required");
                    txtColor.Focus();
                    return false;
                }
                else if (txtSize.Text.Trim().Length == 0)
                {
                    Global.Message("Size Name Is Required");
                    txtSize.Focus();
                    return false;
                }
                else if (txtCutNo.Text.Trim().Length == 0)
                {
                    Global.Message("CutNo Is Required");
                    txtCutNo.Focus();
                    return false;
                }
                else if (txtShade.Text.Trim().Length == 0)
                {
                    Global.Message("Color Shade Is Required");
                    txtCutNo.Focus();
                    return false;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
            return true;
        }

        #endregion

        #region Control event

        private void BtnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                Global.ExcelExport("Box List", GrdDet);
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
                    return;
                }

                DTabBox.AcceptChanges();
                DTabBox.TableName = "DETAIL";

                string BoxDetailXml;
                using (StringWriter sw = new StringWriter())
                {
                    DTabBox.WriteXml(sw);
                    BoxDetailXml = sw.ToString();
                }

                ShapeMasterProperty Property = new ShapeMasterProperty();

                Property.BOX_ID = Val.ToInt32(txtBoxName.Tag);
                Property.BOXNAME = Val.ToString(txtBoxName.Text);
                Property.LOT_ID = Val.ToInt32(txtLot.Tag);
                Property.CLARITY_ID = Val.ToInt32(txtClarity.Tag);
                Property.COLOR_ID = Val.ToInt32(txtColor.Tag);
                Property.COLORSHADE_ID = Val.ToInt32(txtShade.Tag);
                Property.SIZE_ID = Val.ToInt32(txtSize.Tag);
                Property.SHAPE_ID = Val.ToInt32(txtShape.Tag);
                Property.CUTNO = Val.ToInt32(txtCutNo.Text);
                Property.REMARK = Val.ToString(txtRemark.Text);
                if (RbtYes.Checked == true)
                {
                    Property.ISACTIVE = Val.ToBoolean(RbtYes.Checked);
                }

                Property.XMLDETSTR = BoxDetailXml;

                Property = ObjMast.BoxSave(Property);

                Global.Message(Property.ReturnMessageDesc);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    Clear();                    
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            DTabBox = ObjMast.FillDetail(Val.ToString(txtBoxName.Tag));
            DataRow Dr = DTabBox.NewRow();
            DTabBox.Rows.Add(Dr);
            MainGrid.DataSource = DTabBox;
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (Global.Confirm("ARE YOU SURE YOU WANT TO DELETE ENTRY ?") == System.Windows.Forms.DialogResult.Yes)
                {
                    ShapeMasterProperty Property = new ShapeMasterProperty();

                    Property.BOX_ID = Val.ToInt32(txtBoxName.Tag);

                    Property = ObjMast.BoxDelete(Property);

                    if (Property.ReturnMessageType == "SUCCESS")
                    {
                        Global.Message(Property.ReturnMessageDesc);                        
                        Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        #endregion

        #region KeyPress
        private void txtCutNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void txtLot_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "LOT_ID,LOTNAME,LOTCODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mStrColumnsToHide = "LOT_ID";
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_LOT);
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtLot.Tag = Val.ToString(FrmSearch.DRow["LOT_ID"]);
                        txtLot.Text = Val.ToString(FrmSearch.DRow["LOTNAME"]);
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

        private void txtClarity_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "CLARITY_ID,CLARITYNAME,CLARITYCODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mStrColumnsToHide = "CLARITY_ID";
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_CLARITY);
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtClarity.Tag = Val.ToString(FrmSearch.DRow["CLARITY_ID"]);
                        txtClarity.Text = Val.ToString(FrmSearch.DRow["CLARITYNAME"]);
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

        private void txtColor_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "COLOR_ID,COLORNAME,COLORCODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mStrColumnsToHide = "COLOR_ID";
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_COLOR);
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtColor.Tag = Val.ToString(FrmSearch.DRow["COLOR_ID"]);
                        txtColor.Text = Val.ToString(FrmSearch.DRow["COLORNAME"]);
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

        private void txtSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "SIZE_ID,SIZENAME,SIZECODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mStrColumnsToHide = "SIZE_ID";
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_SIZE);
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtSize.Tag = Val.ToString(FrmSearch.DRow["SIZE_ID"]);
                        txtSize.Text = Val.ToString(FrmSearch.DRow["SIZENAME"]);
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

        private void txtShape_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "SHAPE_ID,SHAPENAME,SHAPECODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mStrColumnsToHide = "SHAPE_ID";
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.PARCEL_SHAPE);
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtShape.Tag = Val.ToString(FrmSearch.DRow["SHAPE_ID"]);
                        txtShape.Text = Val.ToString(FrmSearch.DRow["SHAPENAME"]);
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
        private void RepQuotePrice_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DataRow Dro = GrdDet.GetFocusedDataRow();

                    if (Val.ToString(Dro["FROMDATE"]) != "" && Val.ToString(Dro["TODATE"]) != "")
                    {
                        DataRow DRNew = DTabBox.NewRow();
                        DTabBox.Rows.Add(DRNew);
                    }
                    else if (GrdDet.IsLastRow)
                    {
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
        #endregion

        #region GridDetail
        private void GrdDet_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }
                if (e.Column.FieldName == "OPENINGCARAT" || e.Column.FieldName == "OPENINGRATE")
                {
                    DataRow DRow = GrdDet.GetDataRow(e.RowHandle);
                    DTabBox.Rows[e.RowHandle]["OPENINGAMOUNT"] = Math.Round(Val.Val(DRow["OPENINGCARAT"]) * Val.Val(DRow["OPENINGRATE"]), 2);
                    DTabBox.AcceptChanges();
                }
            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
            }
        }

        private void GridDetail_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0)
                {
                    return;
                }

                if (e.Clicks == 2)
                {
                    DataRow DR = GridDetail.GetDataRow(e.RowHandle);
                    FetchValue(DR);
                    DTabBox = ObjMast.FillDetail(Val.ToString(txtBoxName.Tag));
                    DataRow DRNew = DTabBox.NewRow();
                    DTabBox.Rows.Add(DRNew);
                    MainGrid.DataSource = DTabBox;
                    DR = null;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }


        #endregion

        #region Layout
        private void lblSaveLayout_Click(object sender, EventArgs e)
        {
            try
            {
                Stream str = new System.IO.MemoryStream();
                GrdDet.SaveLayoutToStream(str);
                str.Seek(0, System.IO.SeekOrigin.Begin);
                StreamReader reader = new StreamReader(str);
                string text = reader.ReadToEnd();

                int IntRes = new BOTRN_StockUpload().SaveGridLayout(this.Name, GrdDet.Name, text);
                if (IntRes != -1)
                {
                    Global.Message("Layout Successfully Saved");
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void lblDefaultLayout_Click(object sender, EventArgs e)
        {
            try
            {
                int IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdDet.Name);
                if (IntRes != -1)
                {
                    Global.Message("Layout Successfully Deleted");
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
        #endregion

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                BtnDelete.Enabled = true;
                string StrFromDate = null;
                string StrToDate = null;

                if (dtFromDate.Checked)
                {
                    StrFromDate = Val.SqlDate(dtFromDate.Value.ToShortDateString());
                }
                if (dtToDate.Checked)
                {
                    StrToDate = Val.SqlDate(dtToDate.Value.ToShortDateString());
                }


                DataTable dt = ObjMast.BoxGetData(Val.SqlDate(StrFromDate), Val.SqlDate(StrToDate),Val.ToString(txtSearchBoxName.Text));
                MainGridDetailView.DataSource = dt;
                GridDetail.BestFitColumns();
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
        }

        private void txtLot_TextChanged(object sender, EventArgs e)
        {
           
            if (Val.ToString(txtBoxName.Tag) == "")
            {
                txtBoxName.Text = txtLot.Text + ":" + txtSize.Text + ":" + txtColor.Text + ":" + txtClarity.Text + ":" + txtCutNo.Text ;
            }
        }

        private void txtCutNo_Validated(object sender, EventArgs e)
        {
            int num;
            bool isNum = Int32.TryParse(txtCutNo.Text.Trim(), out num);

            if (!isNum)
            {
                int cutno = Val.ToInt32("0" + txtCutNo.Text);
                txtCutNo.Text = Val.ToString(cutno);
            }

        }

        private void txtShade_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "COLORSHADE_ID,COLORSHADENAME,COLORSHADECODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mStrColumnsToHide = "COLORSHADE_ID";
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PARACOLORSHADE);
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtShade.Tag = Val.ToString(FrmSearch.DRow["COLORSHADE_ID"]);
                        txtShade.Text = Val.ToString(FrmSearch.DRow["COLORSHADENAME"]);
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
    }
}

