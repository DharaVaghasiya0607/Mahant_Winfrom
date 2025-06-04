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
using MahantExport.Utility;
using DevExpress.XtraGrid.Columns;


namespace MahantExport
{
    public partial class FrmSearchPopupBoxMultipleSelect : DevExpress.XtraEditors.XtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BODevGridSelection ObjGridSelection;

        public DataTable mDTab;

        public string mStrSearchText = "";
        public string mStrSearchField = "";
        public string mStrColumnsToHide = "";

        public string DisplayMemeter = "";
        public string ValueMemeter = "";

        public string SelectedDisplaymember = "";
        public string SelectedValuemember = "";

        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        public DataTable DTabResult = new DataTable("Detail");

        public FrmSearchPopupBoxMultipleSelect()
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
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = false;
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
                if (mStrColumnsToHide.Contains(Column.Name.ToString().Replace("col", "")) == true)
                {
                    Column.Visible = false;
                }
                if (Column.FieldName.Contains("_ID") == true || Column.FieldName.Contains("STAKA") == true || Column.FieldName.Contains("SMETER") == true)
                {
                    Column.Visible = false;
                }

                if (Column.FieldName.Contains("LOTNO") == true ||
                    Column.FieldName.Contains("LOTNOCOMBINE") == true ||
                    Column.FieldName.Contains("PARTYMARKA") == true ||
                     Column.FieldName.Contains("QUALITYNAME") == true ||
                     Column.FieldName.Contains("QUALITY") == true

                    )
                {
                    Column.AppearanceCell.Font = new Font(GrdDet.Appearance.Row.Font.FontFamily, GrdDet.Appearance.Row.Font.Size, FontStyle.Bold);
                }

                if (list != null && list.Count != 0)
                {
                    string sCaption = Val.ToString(list[Column.FieldName]);
                    if (sCaption != "")
                    {
                        Column.Caption = sCaption;
                    }
                }
                //Column.Caption = Global.FindCaption(Column.FieldName);
                Column.OptionsFilter.FilterPopupMode = DevExpress.XtraGrid.Columns.FilterPopupMode.CheckedList;

            }


            MainGrid.RefreshDataSource();

            GrdDet.BestFitMaxRowCount = 100;
            GrdDet.BestFitColumns();
            GrdDet.FocusedRowHandle = 0;

            if (MainGrid.RepositoryItems.Count == 0)
            {
                ObjGridSelection = new BODevGridSelection();
                ObjGridSelection.View = GrdDet;
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;
            }
            else
            {
                ObjGridSelection.ClearSelection();
            }
            GrdDet.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;
            if (ObjGridSelection != null)
            {
                ObjGridSelection.ClearSelection();
                ObjGridSelection.CheckMarkColumn.VisibleIndex = 0;

            }




            GrdDet.EndUpdate();
            txtSeach.Focus();
            txtSeach.Text = mStrSearchText;
            txtSeach.SelectionStart = txtSeach.Text.Length + 1;
            txtSeach.SelectionLength = 0;
            txtSeach.DeselectAll();

            GrdDet.Focus();

            GrdDet.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle;
            if (DisplayMemeter != "")
            {
                GrdDet.FocusedColumn = GrdDet.Columns[DisplayMemeter];
            }
            else
            {
                GrdDet.FocusedColumn = GrdDet.Columns[2];
            } CalculateSummary();
        }


        private void FrmSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                DTabResult.Rows.Clear();
                this.Close();
            }

            else if (e.KeyCode == Keys.Space)
            {
                //if (this.ActiveControl != txtSeach)
                //{

                //}
                //GrdDet.PostEditor();
                //if (Val.ToBoolean(GrdDet.GetFocusedRowCellValue("COLSELECTCHECKBOX")) == true)
                //{
                //    GrdDet.SetFocusedRowCellValue("COLSELECTCHECKBOX", false);
                //}
                //else
                //{
                //    GrdDet.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                //}
            }
            //else if (e.KeyCode == Keys.Down)
            //{
            //    GrdDet.FocusedRowHandle = GrdDet.FocusedRowHandle + 1;
            //}
            //else if (e.KeyCode == Keys.Up)
            //{
            //    GrdDet.FocusedRowHandle = GrdDet.FocusedRowHandle - 1;
            //}
            else if (e.KeyCode == Keys.Enter)
            {
                BtnSelect_Click(null, null);
            }
        }

        private void ChangeData()
        {
            try
            {
                DataView myDataView = new DataView(mDTab);

                string[] StrSplit = mStrSearchField.Split(',');

                string RowFilter = "";
                for (int IntI = 0; IntI < StrSplit.Length; IntI++)
                {
                    if (txtSeach.Text.Length != 0)
                    {

                        if (Val.ISNumeric(txtSeach.Text) && (mStrSearchField.Contains("EMPLOYEECODE") || mStrSearchField.Contains("LEDGERCODE") || mStrSearchField.Contains("PROCESSCODE")))
                        {
                            RowFilter += " Convert(" + StrSplit[IntI] + ",'System.String')" + " = " + "'" + txtSeach.Text + "' ";
                        }
                        else
                        {
                            RowFilter += " Convert(" + StrSplit[IntI] + ",'System.String')" + " Like " + "'" + txtSeach.Text + "%' ";
                        }
                        if (IntI != StrSplit.Length - 1)
                        {
                            RowFilter += " Or ";
                        }
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

        private void lblDeptExport_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.DefaultExt = ".xlsx";
                svDialog.Title = "Export to Excel";
                svDialog.Filter = "Excel files 97-2003 (*.xls)|*.xls|Excel files 2007(*.xlsx)|*.xlsx|All files (*.*)|*.*";
                if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                {
                    GrdDet.ExportToXlsx(svDialog.FileName);

                    if (Global.Confirm("Do You Want To Open [" + svDialog.FileName + ".xlsx] ?") == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(svDialog.FileName, "CMD");
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

        private void BtnSelect_Click(object sender, EventArgs e)
        {
            SelectedDisplaymember = string.Empty;
            SelectedValuemember = string.Empty;
            if (DTabResult != null)
            {
                DTabResult = Global.GetSelectedRecordOfGrid(GrdDet, true, ObjGridSelection);
                DTabResult.TableName = "Detail";

                if (DisplayMemeter != "" && ValueMemeter != "")
                {
                    foreach (DataRow DRow in DTabResult.Rows)
                    {
                        SelectedDisplaymember = SelectedDisplaymember + Val.ToString(DRow[DisplayMemeter]) + ",";
                        SelectedValuemember = SelectedValuemember + Val.ToString(DRow[ValueMemeter]) + ",";
                    }
                    if (SelectedDisplaymember.ToString().Length != 0)
                    {
                        SelectedDisplaymember = SelectedDisplaymember.Substring(0, SelectedDisplaymember.Length - 1);
                    }
                    if (SelectedValuemember.ToString().Length != 0)
                    {
                        SelectedValuemember = SelectedValuemember.Substring(0, SelectedValuemember.Length - 1);
                    }
                }


            }
            this.Close();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (DTabResult != null)
            {
                DTabResult.Rows.Clear();
            }
            this.Close();
        }

        private void MainGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up)
            {
                if (GrdDet.FocusedRowHandle == 0)
                {
                    GrdDet.FocusedRowHandle = DevExpress.XtraGrid.GridControl.AutoFilterRowHandle;
                }
            }
            if (e.KeyCode == Keys.Space)
            {
                GrdDet.PostEditor();
                //if (Val.ToBoolean(GrdDet.GetFocusedRowCellValue("COLSELECTCHECKBOX")) == true)
                //{
                //    GrdDet.SetFocusedRowCellValue("COLSELECTCHECKBOX", false);
                //}
                //else
                //{
                //    GrdDet.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                //}
            }
            else if (e.KeyCode == Keys.Enter)
            {
                BtnSelect_Click(null, null);
            }
        }

        private void GrdDet_MouseUp(object sender, MouseEventArgs e)
        {
            CalculateSummary();
        }

        public void CalculateSummary()
        {
            int IntTaka = 0;
            double DouMeter = 0;
            int IntSelTaka = 0;
            double DouSelMeter = 0;
            if (GrdDet.Columns.Contains(GrdDet.Columns["ORGMETER"]) && GrdDet.Columns.Contains(GrdDet.Columns["ORGTAKA"]))
            {
                for (int IntI = 0; IntI < GrdDet.RowCount; IntI++)
                {
                    IntTaka = IntTaka + Val.ToInt(GrdDet.GetRowCellValue(IntI, "ORGTAKA"));
                    DouMeter = DouMeter + Val.Val(GrdDet.GetRowCellValue(IntI, "ORGMETER"));
                    if (Val.ToBoolean(GrdDet.GetRowCellValue(IntI, "COLSELECTCHECKBOX")) == true)
                    {
                        IntSelTaka = IntSelTaka + Val.ToInt(GrdDet.GetRowCellValue(IntI, "ORGTAKA"));
                        DouSelMeter = DouSelMeter + Val.Val(GrdDet.GetRowCellValue(IntI, "ORGMETER"));
                    }
                }
            }

            lblTaka.Visible = false;
            lblSelection.Visible = false;
            txtTotalTaka.Visible = false;
            txtTotalMeter.Visible = false;
            txtSelectedTaka.Visible = false;
            txtSelectedMeter.Visible = false;

            if (IntTaka != 0)
            {
                lblTaka.Visible = true;
                lblSelection.Visible = true;
                txtTotalTaka.Visible = true;
                txtTotalMeter.Visible = true;
                txtSelectedTaka.Visible = true;
                txtSelectedMeter.Visible = true;
            }

            txtTotalTaka.Text = IntTaka.ToString();
            txtTotalMeter.Text = DouMeter.ToString();

            txtSelectedTaka.Text = IntSelTaka.ToString();
            txtSelectedMeter.Text = DouSelMeter.ToString();

        }

        private void GrdDet_KeyUp(object sender, KeyEventArgs e)
        {
            CalculateSummary();
            if (e.KeyCode == Keys.Space)
            {
                GrdDet.PostEditor();
                if (Val.ToBoolean(GrdDet.GetFocusedRowCellValue("COLSELECTCHECKBOX")) == true)
                {
                    GrdDet.SetFocusedRowCellValue("COLSELECTCHECKBOX", false);
                }
                else
                {
                    GrdDet.SetFocusedRowCellValue("COLSELECTCHECKBOX", true);
                } GrdDet.PostEditor();

            }
        }

    }
}