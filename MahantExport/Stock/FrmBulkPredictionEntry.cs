using BusLib;
using BusLib.Configuration;
using BusLib.Rapaport;
using BusLib.TableName;
using BusLib.Transaction;
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

namespace MahantExport.Stock
{
    public partial class FrmBulkPredictionEntry : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();
        BOFindRap ObjRap = new BOFindRap();

        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        DataTable DTabStcokGrid = new DataTable();
        DataTable DTabStcok = new DataTable();

        public FrmBulkPredictionEntry()
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

            DataRow[] DR = null;
            DataTable DTabPrdType = new BOComboFill().FillCmb(BOComboFill.SELTABLE.MST_PRDTYPE);
            DR = DTabPrdType.Select("PRDTYPE_ID IN(3,4,5,6,1)");
            foreach (DataRow DRow in DR)
            {
                DTabPrdType.Rows.Remove(DRow);
            }
            //End: Dhara: 18-06-2020

            DTabPrdType.DefaultView.Sort = "SEQUENCENO";
            DTabPrdType = DTabPrdType.DefaultView.ToTable();

            CmbPrdType.DataSource = DTabPrdType;
            CmbPrdType.DisplayMember = "PRDTYPENAME";
            CmbPrdType.ValueMember = "PRDTYPE_ID";

            DataTable DTabColor = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COLOR);
            foreach (DataRow row in DTabColor.Rows)
            {
                RepColorDropDown.Items.Add(row["COLORNAME"]);
            }

            DataTable DTabColorShade = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COLORSHADE);
            foreach (DataRow row in DTabColorShade.Rows)
            {
                RepColorShadeDropDown.Items.Add(row["COLORSHADENAME"]);
            }

            DataTable DTabClarity = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CLARITY);
            foreach (DataRow row in DTabClarity.Rows)
            {
                RepClarityDropDown.Items.Add(row["CLARITYNAME"]);
            }

            DataTable DTabMilky = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_MILKY);
            foreach (DataRow row in DTabMilky.Rows)
            {
                RepMilkyDropDown.Items.Add(row["MILKYNAME"]);
            }

            this.Show();
                    
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

        private void txtPartyStockNo_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                if(Val.ToString(txtEmpName.Tag) == "")
                {
                    return;
                }
                DTabStcok = ObjStock.GetBulkPredictionStockDetail(Val.ToInt32(CmbPrdType.SelectedValue), Val.ToString(txtPartyStockNo.Text), Val.ToString(txtEmpName.Tag));
                if (DTabStcok.Rows.Count > 0)
                {
                    txtPartyStockNo.Tag = Val.ToString(DTabStcok.Rows[0]["STOCK_ID"]);
                    txtCarat.Text = Val.ToString(DTabStcok.Rows[0]["CARAT"]);

                    txtShape.Text = Val.ToString(DTabStcok.Rows[0]["SHAPENAME"]);
                    txtShape.Tag = Val.ToString(DTabStcok.Rows[0]["SHAPE_ID"]);

                    txtCut.Text = Val.ToString(DTabStcok.Rows[0]["CUTNAME"]);
                    txtCut.Tag = Val.ToString(DTabStcok.Rows[0]["CUT_ID"]);

                    txtPol.Text = Val.ToString(DTabStcok.Rows[0]["POLNAME"]);
                    txtPol.Tag = Val.ToString(DTabStcok.Rows[0]["POL_ID"]);

                    txtSym.Text = Val.ToString(DTabStcok.Rows[0]["SYMNAME"]);
                    txtSym.Tag = Val.ToString(DTabStcok.Rows[0]["SYM_ID"]);

                    txtFlo.Text = Val.ToString(DTabStcok.Rows[0]["FLNAME"]);
                    txtFlo.Tag = Val.ToString(DTabStcok.Rows[0]["FL_ID"]);

                    if(txtCarat.Text.Length == 0)
                    {
                        Global.Message("No data found !!");
                        return;
                    }
                    DTabStcokGrid  = DTabStcok.Clone(); // Clone structure (columns)

                    // Select rows where EmployeeID is not null or empty
                    DataRow[] filteredRows = DTabStcok.Select("EMPLOYEE IS NOT NULL AND EMPLOYEE <> '' AND PRDTYPE_ID <> 1");

                    // Add the filtered rows to the new DataTable
                    foreach (DataRow row in filteredRows)
                    {
                        DTabStcokGrid.ImportRow(row);
                    }

                    DTabStcokGrid.AcceptChanges();
                    MainGrdDetail.DataSource = DTabStcokGrid;
                    GrdDetail.RefreshData();
                    GrdDetail.Focus();
                }
            }
            catch (Exception Ex)
            {
                Global.Message(Ex.Message.ToString());
            }
        }

        private void txtEmpName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearchPopupBoxMultipleSelect FrmSearch = new FrmSearchPopupBoxMultipleSelect();
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_EMPLOYEE);
                    FrmSearch.mStrColumnsToHide = "";
                    FrmSearch.ValueMemeter = "EMPLOYEE_ID";
                    FrmSearch.DisplayMemeter = "EMPLOYEENAME";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.SelectedDisplaymember != "" && FrmSearch.SelectedValuemember != "")
                    {
                        txtEmpName.Text = Val.ToString(FrmSearch.SelectedDisplaymember);
                        txtEmpName.Tag = Val.ToString(FrmSearch.SelectedValuemember);                        
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

        private void GrdDetail_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            try
            {

                if (e.RowHandle < 0)
                {
                    return;
                }
                DataRow DRow = GrdDetail.GetDataRow(e.RowHandle);

                switch (e.Column.FieldName)
                {
                    case "COLOR":
                    case "CLARITY":                   

                        Trn_RapSaveProperty Property = new Trn_RapSaveProperty();

                        Property.SHAPE_ID = Val.ToInt32(txtShape.Tag);
                        Property.COLOR_ID = Val.ToInt32(GrdDetail.GetFocusedRowCellValue("COLOR_ID"));
                        Property.CLARITY_ID = Val.ToInt32(GrdDetail.GetFocusedRowCellValue("CLARITY_ID"));

                        Property.COLORCODE = Val.ToString(GrdDetail.GetFocusedRowCellValue("COLOR"));
                        Property.CLARITYCODE = Val.ToString(GrdDetail.GetFocusedRowCellValue("CLARITY"));

                        Property.CUT_ID = Val.ToInt32(txtCut.Tag);
                        Property.POL_ID = Val.ToInt32(txtPol.Tag);
                        Property.SYM_ID = Val.ToInt32(txtSym.Tag);
                        Property.FL_ID = Val.ToInt32(txtFlo.Tag);
                        Property.CARAT = Val.Val(txtCarat.Text);
                        Property = ObjRap.FindRap(Property);

                        //Added by Daksha on 11/07/2023
                        DTabStcokGrid.Rows[e.RowHandle]["RAPPOPRT"] = Property.RAPAPORT;
                        DTabStcokGrid.Rows[e.RowHandle]["PRICEPERCTS"] = Math.Round(Property.RAPAPORT - ((Property.RAPAPORT * Val.Val(DRow["DISCOUNT"])) / 100));
                        DTabStcokGrid.Rows[e.RowHandle]["AMOUNT"] = Math.Round(Property.CARAT * Val.Val(DRow["PRICEPERCTS"]), 2);

                        DTabStcokGrid.AcceptChanges();
                        MainGrdDetail.RefreshDataSource();
                        break;
                    case "DISCOUNT":

                        double Rapaport = Val.Val(DRow["RAPPOPRT"]);
                        double Carat = Val.Val(txtCarat.Text);

                        DTabStcokGrid.Rows[e.RowHandle]["RAPPOPRT"] = Rapaport;
                        DTabStcokGrid.Rows[e.RowHandle]["PRICEPERCTS"] = Math.Round(Rapaport + ((Rapaport * Val.Val(DRow["DISCOUNT"])) / 100));
                        DTabStcokGrid.Rows[e.RowHandle]["AMOUNT"] = Math.Round(Carat * Val.Val(DTabStcokGrid.Rows[e.RowHandle]["PRICEPERCTS"]), 2);
                        DTabStcokGrid.AcceptChanges();
                        MainGrdDetail.RefreshDataSource();
                        break;

                    case "PRICEPERCTS":

                        Rapaport = Val.Val(DRow["RAPPOPRT"]);
                        double PricePerCarat = Val.Val(DRow["PRICEPERCTS"]);
                        Carat = Val.Val(txtCarat.Text);
                        double DouPer = 0;
                        if (Rapaport != 0)
                        {
                            DouPer = Math.Round(((Rapaport - PricePerCarat) / Rapaport) * -100, 2);
                        }
                        else
                            DouPer = 0;

                        DTabStcokGrid.Rows[e.RowHandle]["RAPPOPRT"] = Rapaport;
                        DTabStcokGrid.Rows[e.RowHandle]["DISCOUNT"] = DouPer;
                        DTabStcokGrid.Rows[e.RowHandle]["AMOUNT"] = Math.Round(Carat * Val.Val(DTabStcokGrid.Rows[e.RowHandle]["PRICEPERCTS"]), 2);
                        DTabStcokGrid.AcceptChanges();
                        MainGrdDetail.RefreshDataSource();
                        break;
                }
               
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }

        }
      
        private void RepPopup_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDetail.FocusedRowHandle < 0)
                    return;

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearchPopupBox = new FrmSearchPopupBox();
                    FrmSearchPopupBox.mStrSearchField = "COLORCODE,COLORNAME";
                    FrmSearchPopupBox.mStrSearchText = e.KeyChar.ToString();
                    FrmSearchPopupBox.mFilterType = FrmSearchPopupBox.FilterType.FirstLike; //Added by Daksha on 12/04/2023
                    FrmSearchPopupBox.mBoolSearchWithoutLike = true;
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearchPopupBox.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COLOR); 
                    FrmSearchPopupBox.mStrColumnsToHide = "COLOR_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearchPopupBox.ShowDialog();
                    e.Handled = true;
                    if (FrmSearchPopupBox.DRow != null)
                    {
                        GrdDetail.SetFocusedRowCellValue("COLOR_ID", (Val.ToString(FrmSearchPopupBox.DRow["COLOR_ID"])));
                        GrdDetail.SetFocusedRowCellValue("COLOR", (Val.ToString(FrmSearchPopupBox.DRow["COLORNAME"])));
                    }
                    FrmSearchPopupBox.Hide();
                    FrmSearchPopupBox.Dispose();
                    FrmSearchPopupBox = null;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void RepmilkyPopup_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDetail.FocusedRowHandle < 0)
                    return;

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearchPopupBox = new FrmSearchPopupBox();
                    FrmSearchPopupBox.mStrSearchField = "MILKYCODE,MILKYNAME";
                    FrmSearchPopupBox.mStrSearchText = e.KeyChar.ToString();
                    FrmSearchPopupBox.mFilterType = FrmSearchPopupBox.FilterType.FirstLike; //Added by Daksha on 12/04/2023
                    FrmSearchPopupBox.mBoolSearchWithoutLike = true;
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearchPopupBox.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_MILKY);
                    FrmSearchPopupBox.mStrColumnsToHide = "MILKY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearchPopupBox.ShowDialog();
                    e.Handled = true;
                    if (FrmSearchPopupBox.DRow != null)
                    {
                        GrdDetail.SetFocusedRowCellValue("MILKY_ID", (Val.ToString(FrmSearchPopupBox.DRow["MILKY_ID"])));
                        GrdDetail.SetFocusedRowCellValue("MILKY", (Val.ToString(FrmSearchPopupBox.DRow["MILKYNAME"])));
                    }
                    FrmSearchPopupBox.Hide();
                    FrmSearchPopupBox.Dispose();
                    FrmSearchPopupBox = null;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void RepcolorShadePopup_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDetail.FocusedRowHandle < 0)
                    return;

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearchPopupBox = new FrmSearchPopupBox();
                    FrmSearchPopupBox.mStrSearchField = "COLORSHADECODE,COLORSHADENAME";
                    FrmSearchPopupBox.mStrSearchText = e.KeyChar.ToString();
                    FrmSearchPopupBox.mFilterType = FrmSearchPopupBox.FilterType.FirstLike; //Added by Daksha on 12/04/2023
                    FrmSearchPopupBox.mBoolSearchWithoutLike = true;
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearchPopupBox.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COLORSHADE);
                    FrmSearchPopupBox.mStrColumnsToHide = "COLORSHADE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearchPopupBox.ShowDialog();
                    e.Handled = true;
                    if (FrmSearchPopupBox.DRow != null)
                    {
                        GrdDetail.SetFocusedRowCellValue("COLORSHADE_ID", (Val.ToString(FrmSearchPopupBox.DRow["COLORSHADE_ID"])));
                        GrdDetail.SetFocusedRowCellValue("COLORSHADE", (Val.ToString(FrmSearchPopupBox.DRow["COLORSHADENAME"])));
                    }
                    FrmSearchPopupBox.Hide();
                    FrmSearchPopupBox.Dispose();
                    FrmSearchPopupBox = null;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void RepClarityPopup_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDetail.FocusedRowHandle < 0)
                    return;

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearchPopupBox = new FrmSearchPopupBox();
                    FrmSearchPopupBox.mStrSearchField = "CLARITYCODE,CLARITYNAME";
                    FrmSearchPopupBox.mStrSearchText = e.KeyChar.ToString();
                    FrmSearchPopupBox.mFilterType = FrmSearchPopupBox.FilterType.FirstLike; //Added by Daksha on 12/04/2023
                    FrmSearchPopupBox.mBoolSearchWithoutLike = true;
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearchPopupBox.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CLARITY);
                    FrmSearchPopupBox.mStrColumnsToHide = "CLARITY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearchPopupBox.ShowDialog();
                    e.Handled = true;
                    if (FrmSearchPopupBox.DRow != null)
                    {
                        GrdDetail.SetFocusedRowCellValue("CLARITY_ID", (Val.ToString(FrmSearchPopupBox.DRow["CLARITY_ID"])));
                        GrdDetail.SetFocusedRowCellValue("CLARITY", (Val.ToString(FrmSearchPopupBox.DRow["CLARITYNAME"])));

                    }
                    FrmSearchPopupBox.Hide();
                    FrmSearchPopupBox.Dispose();
                    FrmSearchPopupBox = null;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtEmpName.Text = string.Empty;
                txtEmpName.Tag = string.Empty;

                txtPartyStockNo.Tag = string.Empty;
                txtPartyStockNo.Text = string.Empty;

                txtCarat.Text = string.Empty;

                txtShape.Text = string.Empty;
                txtShape.Tag = string.Empty;

                txtCut.Text = string.Empty;
                txtCut.Tag = string.Empty;

                txtPol.Text = string.Empty;
                txtPol.Tag = string.Empty;

                txtSym.Text = string.Empty;
                txtSym.Tag = string.Empty;

                txtFlo.Text = string.Empty;
                txtFlo.Tag = string.Empty;

                DTabStcokGrid.Rows.Clear();
                DTabStcok.Rows.Clear();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtPartyStockNo_Validated(object sender, EventArgs e)
        {
            
        }

        private void RepBtnSave_Click(object sender, EventArgs e)
        {
            try
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

                    Property.STOCK_ID = Val.ToGuid(txtPartyStockNo.Tag);
                    Property.STOCKNO = Val.ToString(txtPartyStockNo.Text);
                    Property.CARAT = Val.Val(txtCarat.Text);

                    Property.SHAPE_ID = Val.ToInt32(txtShape.Tag);
                    Property.COLOR_ID = Val.ToInt32(GrdDetail.GetFocusedRowCellValue("COLOR_ID"));
                    Property.CUT_ID = Val.ToInt32(txtCut.Tag);
                    Property.POL_ID = Val.ToInt32(txtPol.Tag);
                    Property.SYM_ID = Val.ToInt32(txtSym.Tag);
                    Property.FL = Val.ToInt32(txtFlo.Tag);                   
                    Property.CLARITY_ID = Val.ToInt32(GrdDetail.GetFocusedRowCellValue("CLARITY_ID"));
                    Property.COLORSHADE_ID = Val.ToInt32(GrdDetail.GetFocusedRowCellValue("COLORSHADE_ID"));

                    Property.MILKY_ID = Val.ToInt32(GrdDetail.GetFocusedRowCellValue("MILKY_ID"));
                  
                    Property.COSTRAPAPORT = Val.Val(GrdDetail.GetFocusedRowCellValue("RAPPOPRT"));
                    Property.COSTDISCOUNT = Val.Val(GrdDetail.GetFocusedRowCellValue("DISCOUNT"));
                    Property.COSTPRICEPERCARAT = Val.Val(GrdDetail.GetFocusedRowCellValue("PRICEPERCTS"));
                    Property.COSTAMOUNT = Val.Val(GrdDetail.GetFocusedRowCellValue("AMOUNT"));
                    Property.PRDTYPE_ID = Val.ToInt32(CmbPrdType.SelectedValue);

                    Property.EMPLOYEE_ID = Val.ToString(GrdDetail.GetFocusedRowCellValue("EMPLOYEE_ID"));

                    Property = ObjStock.SingleBulkPredictionInsert(Property);
                    this.Cursor = Cursors.Default;
                    string StrReturnDesc = Property.ReturnMessageDesc;
                    if (Property.ReturnMessageType == "SUCCESS")
                    {
                        Global.Message(Property.ReturnMessageDesc);
                        this.Cursor = Cursors.Default;                        
                    }
                    else
                    {
                        this.Cursor = Cursors.Default;
                        Global.Message(Property.ReturnMessageDesc);
                        txtPartyStockNo.Focus();
                    }

                    Property = null;

                }
                catch (Exception Ex)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message(Ex.Message.ToString());
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void RepColorDropDown_SelectedValueChanged(object sender, EventArgs e)
        {
            
        }

        private void RepColorDropDown_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraEditors.ComboBoxEdit editor = sender as DevExpress.XtraEditors.ComboBoxEdit;
                if (editor != null)
                {
                    string selectedValue = editor.EditValue?.ToString(); // Get the selected value
                    string StrColor = Val.ToString(selectedValue);      // Convert and assign
                    DataTable DTabColor = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COLOR);

                    // Correct syntax for DataTable.Select
                    DataRow[] matchingRows = DTabColor.Select($"COLORNAME = '{StrColor}'");

                    // Get the Color_ID if a match is found
                    int Color_ID = 0;
                    if (matchingRows.Length > 0)
                    {
                        Color_ID = Convert.ToInt32(matchingRows[0]["Color_ID"]);
                    }

                    GrdDetail.SetFocusedRowCellValue("COLOR_ID", Color_ID);
                }
                
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void RepClarityDropDown_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraEditors.ComboBoxEdit editor = sender as DevExpress.XtraEditors.ComboBoxEdit;
                if (editor != null)
                {
                    string selectedValue = editor.EditValue?.ToString(); // Get the selected value
                    string StrClarity = Val.ToString(selectedValue);      // Convert and assign
                    DataTable DTabColor = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CLARITY);

                    // Correct syntax for DataTable.Select
                    DataRow[] matchingRows = DTabColor.Select($"CLARITYNAME = '{StrClarity}'");

                    // Get the Color_ID if a match is found
                    int CLARITY_ID = 0;
                    if (matchingRows.Length > 0)
                    {
                        CLARITY_ID = Convert.ToInt32(matchingRows[0]["CLARITY_ID"]);
                    }

                    GrdDetail.SetFocusedRowCellValue("CLARITY_ID", CLARITY_ID);
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void RepColorShadeDropDown_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraEditors.ComboBoxEdit editor = sender as DevExpress.XtraEditors.ComboBoxEdit;
                if (editor != null)
                {
                    string selectedValue = editor.EditValue?.ToString(); // Get the selected value
                    string StrColorShade = Val.ToString(selectedValue);      // Convert and assign
                    DataTable DTabColor = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COLORSHADE);

                    // Correct syntax for DataTable.Select
                    DataRow[] matchingRows = DTabColor.Select($"COLORSHADENAME = '{StrColorShade}'");

                    // Get the Color_ID if a match is found
                    int COLORSHADE_ID = 0;
                    if (matchingRows.Length > 0)
                    {
                        COLORSHADE_ID = Convert.ToInt32(matchingRows[0]["COLORSHADE_ID"]);
                    }

                    GrdDetail.SetFocusedRowCellValue("COLORSHADE_ID", COLORSHADE_ID);
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void RepMilkyDropDown_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                DevExpress.XtraEditors.ComboBoxEdit editor = sender as DevExpress.XtraEditors.ComboBoxEdit;
                if (editor != null)
                {
                    string selectedValue = editor.EditValue?.ToString(); // Get the selected value
                    string StrColorShade = Val.ToString(selectedValue);      // Convert and assign
                    DataTable DTabColor = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_MILKY);

                    // Correct syntax for DataTable.Select
                    DataRow[] matchingRows = DTabColor.Select($"MILKYNAME = '{StrColorShade}'");

                    // Get the Color_ID if a match is found
                    int MILKYNAME_ID = 0;
                    if (matchingRows.Length > 0)
                    {
                        MILKYNAME_ID = Convert.ToInt32(matchingRows[0]["MILKY_ID"]);
                    }

                    GrdDetail.SetFocusedRowCellValue("MILKY_ID", MILKYNAME_ID);
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }
    }
}