using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Globalization;
using BusLib.TableName;
using System.IO;
using BusLib.Transaction;

namespace MahantExport
{
    public partial class FrmPopupGrid : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        public DataTable DTab;

        public string ColumnsToHide = "";

        public bool AllowFirstColumnHide = false;

        public bool AllowMultiSelect = false;


        public string SearchText = "";
        public string SearchField = "";
        public string ValueMember = "";
        public string SelectedValue = "";
        public bool ISPostBack = false;
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();

        private string _SummrisedColumn = "";

        public string SummrisedColumn
        {
            get { return _SummrisedColumn; }
            set { _SummrisedColumn = value; }
        }

        private string _CountedColumn = "";

        public string CountedColumn
        {
            get { return _CountedColumn; }
            set { _CountedColumn = value; }
        }

        public DataRow DRow { get; set; }


        public FrmPopupGrid()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            AttachFormDefaultEvent();
            Val.FormGeneralSetting(this);
            //ObjMast.Fill();
            //SetDataBinding();
            this.ShowDialog();
        }
        private void AttachFormDefaultEvent()
        {
            Val.FormGeneralSetting(this);
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.FormKeyPress = true;

            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);

        }
      

      

        private void SelectRow()
        {
            try
            {
                if (AllowMultiSelect == false)
                {
                    if ((GrdDet.SelectedRowsCount > 0))
                    {
                        DRow = GrdDet.GetDataRow(GrdDet.GetSelectedRows()[0]);
                        this.Close();
                    }
                    else
                    {
                        DRow = null;
                        if (ISPostBack == true)
                        {
                            if (Global.Confirm("No Row Found. You Want To PostBack ") == System.Windows.Forms.DialogResult.Yes)
                            {
                                DRow = DTab.NewRow();

                                //if (ColumnsToHide.Contains("LEDGER_ID"))
                                //{
                                //    FrmLedger FrmLedger = new FrmLedger();
                                //    FrmLedger.ShowForm(txtSeach.Text);
                                //    DRow["LEDGERNAME"] = txtSeach.Text;
                                //    DRow["LEDGER_ID"] = FrmLedger.mIntLedgerID;
                                //}
                                //else if (ColumnsToHide.Contains("ITEM_ID"))
                                //{
                                //    FrmItem FrmItem = new FrmItem();
                                //    FrmItem.ShowForm(txtSeach.Text);
                                //    DRow["ITEMNAME"] = txtSeach.Text;
                                //    DRow["ITEM_ID"] = FrmItem.mIntItemID;
                                //}
                               
                                //else
                                //{
                                //    DRow[0] = txtSeach.Text;
                                //}

                            }
                            this.Close();

                        }
                        else
                        {
                            Global.Message("No row selected.");
                            this.Close();
                        }

                    }
                }
                else  if (AllowMultiSelect == true)
                {
                   SelectedValue = "";
                   for (int IntI = 0; IntI < GrdDet.RowCount; IntI++)
                   {
                       if (Val.ToBoolean( GrdDet.GetRowCellValue(IntI,"SEL")) == true)
                       {
                           SelectedValue = SelectedValue + Val.ToString(GrdDet.GetRowCellValue(IntI,ValueMember)) + ",";
                       }
                   }
                  
                    if (SelectedValue.Length != 0 )
                    {
                        SelectedValue = SelectedValue.Substring(0, SelectedValue.Length - 1);
                    }
                }
               
            }
            catch (Exception ex)
            {
                // KGKDiamond.Class.Global.Message(ex.Message.ToString());                
            }
        }

        private void GrdDet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                SelectRow();
            }
        }

        private void GrdDet_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.Clicks == 2)
            {
                if (AllowMultiSelect == false)
                {
                    SelectRow();
                }
                
            }
        }

       

        private void ChangeData()
        {
            try
            {
                DataView myDataView = new DataView(DTab);

                string[] StrSplit = SearchField.Split(',');

                string RowFilter = "";
                for (int IntI = 0; IntI < StrSplit.Length; IntI++)
                {
                    //RowFilter += " Convert(" + StrSplit[IntI] + ",'System.String')" + " Like " + "'%" + txtSeach.Text + "%' ";

                    if (IntI != StrSplit.Length - 1)
                    {
                        RowFilter += " Or ";
                    }
                }

                myDataView.RowFilter = RowFilter;
    
                //myDataView.RowFilter = "Convert(" + _FrmSearchProperty.SearchField + ",'System.String')" + " Like " + "'" + txtSearch.Text + "%'";

                //myDataView.Sort = _FrmSearchProperty.SearchField;
                MainGrid.DataSource = myDataView;

                //dgvSearch.Sort(dgvSearch.Columns[_FrmSearchProperty.SearchField], _FrmSearchProperty.SearchOrder);
            }
            catch (Exception ex)
            {
                //  KGKDiamond.Class.Global.Message(ex.ToString());
            }
        }

        private void txtSeach_TextChanged(object sender, EventArgs e)
        {
            ChangeData();
        }

        private void GrdDet_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column.FieldName == "SEL")
            {
                if (Val.ToBoolean(GrdDet.GetRowCellValue(e.RowHandle, "SEL")) == true)
                {
                    GrdDet.SetRowCellValue(e.RowHandle, "SEL", false);
                }
                else
                {
                    GrdDet.SetRowCellValue(e.RowHandle, "SEL", true);
                }
            }
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            SelectRow();
            this.Hide();
        }

        private void txtSeach_EditValueChanged(object sender, EventArgs e)
        {

        }

        private void MainGrid_Click(object sender, EventArgs e)
        {

        }

        private void FrmPopupGrid_Load(object sender, EventArgs e)
        {
            //MainGrid.DataSource = DTab;

            if (Val.ToString(SummrisedColumn) != "")
            {
                foreach (string Str in SummrisedColumn.Split(','))
                {
                    GrdDet.Columns[Str].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    GrdDet.Columns[Str].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                }
            }
            if (Val.ToString(CountedColumn) != "")
            {
                foreach (string Str in CountedColumn.Split(','))
                {
                    GrdDet.Columns[Str].DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
                    GrdDet.Columns[Str].SummaryItem.SummaryType = DevExpress.Data.SummaryItemType.Count;
                }
            }
            foreach (DevExpress.XtraGrid.Columns.GridColumn Col in GrdDet.Columns)
            {
                if (Col.FieldName.ToUpper() == "SEL")
                {
                    Col.OptionsColumn.AllowEdit = true;
                }
                else
                {
                    Col.OptionsColumn.AllowEdit = false;
                }
            }

            foreach (DevExpress.XtraGrid.Columns.GridColumn Column in this.GrdDet.Columns)
            {
                if (ColumnsToHide.Contains(Column.Name.ToString().Replace("col", "")) == true)
                {
                    Column.Visible = false;
                }
                //if (Column.FieldName != "")
                //{
                //    Column.Caption = Column.FieldName.Replace("_", " ");
                //    Column.Caption = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Column.FieldName.ToLower());
                //}
            }

            //if (GrdDet.Columns.Count == 1)
            //{
            //    GrdDet.Columns[0].Width = 200;
            //}
            //else if (GrdDet.Columns.Count == 2)
            //{
            //    GrdDet.Columns[0].Width = 70;
            //    GrdDet.Columns[1].Width = 200;
            //}
            //else if (GrdDet.Columns.Count == 3)
            //{
            //    GrdDet.Columns[0].Width = 70;
            //    GrdDet.Columns[1].Width = 150;
            //    GrdDet.Columns[2].Width = 150;
            //}
            //else if (GrdDet.Columns.Count == 4)
            //{
            //    GrdDet.Columns[0].Width = 70;
            //    GrdDet.Columns[1].Width = 150;
            //    GrdDet.Columns[2].Width = 150;
            //    GrdDet.Columns[3].Width = 150;
            //}
            //else if (GrdDet.Columns.Count > 4)
            //{
            //    GrdDet.Columns[0].Width = 70;
            //    GrdDet.Columns[1].Width = 150;
            //    GrdDet.Columns[2].Width = 150;
            //    GrdDet.Columns[3].Width = 150;
            //}

            MainGrid.RefreshDataSource();
            //   GrdDet.BestFitColumns();
            GrdDet.FocusedRowHandle = 0;
            if (LblTitle.Text == "List Of Opening Stock Record Which Are Not Exist in Box Master" || LblTitle.Text == "List Of Assortment Upload Record Which Are Not Exist in Box Master")
            {
                btnSave.Visible = true;
            }
           
        }

        private void FrmPopupGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                SelectRow();
            }
            else if (e.KeyCode == Keys.Down)
            {
                GrdDet.FocusedRowHandle = GrdDet.FocusedRowHandle + 1;
            }
            else if (e.KeyCode == Keys.Up)
            {
                GrdDet.FocusedRowHandle = GrdDet.FocusedRowHandle - 1;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dtData = (DataTable)MainGrid.DataSource;

            if (Global.Confirm("Are You Sure Want To Create This Box?") == System.Windows.Forms.DialogResult.Yes)
            {
                PurchaseProperty Property = new PurchaseProperty();
                dtData.TableName = "Table";
                string BoxSaveXml;
                using (StringWriter sw = new StringWriter())
                {
                    dtData.WriteXml(sw);
                    BoxSaveXml = sw.ToString();
                }

                Property = ObjStock.OpeninfStockNotExistsRecord_Save(BoxSaveXml, Property);

                Global.Message(Property.ReturnMessageDesc);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    this.Close();
                }
            }
        }
    }
}