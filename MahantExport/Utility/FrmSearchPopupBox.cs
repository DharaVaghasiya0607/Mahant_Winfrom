using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Globalization;
using System.Collections;

namespace MahantExport
{
    public partial class FrmSearchPopupBox : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();

        public DataTable mDTab;

        public string mStrColumnsToHide = "";

        public bool mBoolSearchWithoutLike = false;

        public bool mBoolAllowFirstColumnHide = false;
        public bool mBoolISPostBack = false;
        public string mStrISPostBackColumn = "";

        public string mStrSearchText = "";
        public string mStrSearchField = "";
        public string mStrValueMember = "";
        public string mStrSelectedValue = "";

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        public FilterType mFilterType = FilterType.FirstLike;

        public enum FilterType
        {
            Like,
            FirstLike,
            LastLike,
            Exact
        }

        public DataRow DRow { get; set; }

        public FrmSearchPopupBox()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            AttachFormDefaultEvent();
            Val.FormGeneralSetting(this);

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


        private void FrmSearch_Load(object sender, EventArgs e)
        {
            GrdDet.BeginUpdate();
            MainGrid.DataSource = mDTab;
            
            Hashtable list = new Hashtable();
            try
            {
                if (System.IO.File.Exists(Application.StartupPath + "//GridHeaderCaption.txt") == true)
                {
                    string[] Actor = System.IO.File.ReadAllLines(Application.StartupPath + "//GridHeaderCaption.txt");

                    foreach (string Str in Actor)
                    {
                        if (Str == "")
                        {
                            continue;
                        }
                        string[] S = Str.Split('=');
                        list.Add(Val.RTrim(Val.LTrim(S[0])), Val.RTrim(Val.LTrim(S[1])));
                    }

                }
            }
            catch (Exception ec)
            {
                Global.Message(ec.Message);
            }

            foreach (DevExpress.XtraGrid.Columns.GridColumn Column in this.GrdDet.Columns)
            {
                if (!(mStrSearchField.Contains(Column.Name.ToString().Replace("col", "")) == true))
                {
                    Column.Visible = false;
                }
                if (Column.FieldName.Contains("_ID") == true)
                {
                    Column.Visible = false;
                    continue;
                }

                //if (Column.FieldName != "")
                //{
                //    Column.Caption = Column.FieldName.Replace("_", " ");
                //    Column.Caption = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Column.FieldName.ToLower());
                //}

                if (list != null && list.Count != 0)
                {
                    string sCaption = Val.ToString(list[Column.FieldName]);
                    if (sCaption != "")
                    {
                        Column.Caption = sCaption;
                    }
                }
            }
            GrdDet.BestFitMaxRowCount = 300;
            GrdDet.BestFitColumns();
            GrdDet.FocusedRowHandle = 0;
            GrdDet.EndUpdate();
            txtSeach.Focus();
            txtSeach.Text = mStrSearchText;
            txtSeach.SelectionStart = txtSeach.Text.Length + 1;
            txtSeach.SelectionLength = 0;
            txtSeach.DeselectAll();

        }

        private void FocusOnSelectedRow()
        {
            try
            {
                if ((GrdDet.SelectedRowsCount > 0))
                {
                    DRow = GrdDet.GetDataRow(GrdDet.GetSelectedRows()[0]);
                    this.Close();
                    SendKeys.Send("{TAB}");
                }
                else
                {
                    DRow = null;
                    if (mBoolISPostBack == true)
                    {
                        //Commnet By Gunjan:15/07/2024
                        //if (Global.Confirm("No Row Found. You Want To PostBack ") == System.Windows.Forms.DialogResult.Yes)
                        //{
                            DRow = mDTab.NewRow();
                            DRow[mStrISPostBackColumn] = txtSeach.Text.ToUpper();
                        //}
                        this.Close();

                    }
                    else
                    {
                        Global.MessageError("No row selected.");
                        this.Close();
                    }
                }
                //else
                //{
                //    DRow = null;
                //    Global.Message("No row selected.");
                //    this.Close();

                //}
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
                FocusOnSelectedRow();
            }
        }

        private void GrdDet_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.Clicks == 2)
            {
                FocusOnSelectedRow();
            }
        }

        private void FrmSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            else if (e.KeyCode == Keys.Enter)
            {
                FocusOnSelectedRow();
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

        private void FilterText()
        {
            try
            {
                DataView myDataView = new DataView(mDTab);

                string[] StrSplit = mStrSearchField.Split(',');

                string RowFilter = "";
                for (int IntI = 0; IntI < StrSplit.Length; IntI++)
                {
                    if (mFilterType == FilterType.Exact)
                    {
                        RowFilter += " Convert(" + StrSplit[IntI] + ",'System.String')" + " = " + "'" + txtSeach.Text + "' ";
                    }
                    else if (mFilterType == FilterType.FirstLike)
                    {
                        RowFilter += " Convert(" + StrSplit[IntI] + ",'System.String')" + " Like " + "'" + txtSeach.Text + "%' ";
                    }
                    else if (mFilterType == FilterType.LastLike)
                    {
                        RowFilter += " Convert(" + StrSplit[IntI] + ",'System.String')" + " Like " + "'%" + txtSeach.Text + "' ";
                    }
                    else if (mFilterType == FilterType.Like)
                    {
                        RowFilter += " Convert(" + StrSplit[IntI] + ",'System.String')" + " Like " + "'%" + txtSeach.Text + "%' ";
                    }

                    if (IntI != StrSplit.Length - 1)
                    {
                        RowFilter += " Or ";
                    }

                    //if (mBoolSearchWithoutLike)
                    //{
                    //    RowFilter += " Convert(" + StrSplit[IntI] + ",'System.String')" + " Like " + "'" + txtSeach.Text + "%' ";
                    //}
                    //else
                    //{
                    //    RowFilter += " Convert(" + StrSplit[IntI] + ",'System.String')" + " Like " + "'%" + txtSeach.Text + "%' ";
                    //}

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
            FilterText();
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
            FocusOnSelectedRow();
            this.Hide();
        }


        private void lblExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("data.xlsx", GrdDet);
        }

    }
}