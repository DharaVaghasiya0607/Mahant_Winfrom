using System;
using System.Data;
using System.Drawing;
using System.Collections;
using System.Windows.Forms;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors.Repository;
using System.Linq;

namespace MahantExport.Utility
{
    public class BODevGridSelection
    {
        private RepositoryItemCheckEdit mRepEdit;
        private bool mBoolSelectionColrFlag;
        const int mIntCheckboxIndent = 4;

        protected ArrayList mSelectionList;
        protected ArrayList mselectionPageList;
        public bool ISBoolApplicableForPageConcept = false;
        public bool ISCreateHeaderWithCheckBox = true;

        public BODevGridSelection(GridView pGridView)
            : this()
        {
            View = pGridView;
        }

        public BODevGridSelection(bool pBoolSelectionColrFlag = true)
        {
            mselectionPageList = new ArrayList();
            mSelectionList = new ArrayList();
            mBoolSelectionColrFlag = pBoolSelectionColrFlag;
        }

        private GridColumn mGridcolumn;
        public GridColumn CheckMarkColumn
        {
            get { return mGridcolumn; }
        }

        private GridView mGridview;
        public GridView View
        {
            get { return mGridview; }
            set
            {
                if (!object.ReferenceEquals(mGridview, value))
                {
                    DetachGrid();
                    AttachGrid(value);
                }
            }
        }


        public int SelectedCount
        {
            get { return mSelectionList.Count; }
        }

        public int SelectedCountPage
        {
            get { return mselectionPageList.Count; }
        }

        public object GetSelectedRow(int pIntIndex)
        {
            return mSelectionList[pIntIndex];
        }

        public object GetSelectedRowPage(int pIntIndex)
        {
            return mselectionPageList[pIntIndex];
        }

        public int GetSelectedIndex(object pObjRow)
        {
            return mSelectionList.IndexOf(pObjRow);
        }
        public int GetSelectedIndexPage(object pObjRow)
        {
            return mselectionPageList.IndexOf(pObjRow);
        }

        public void ClearSelection()
        {
            if (ISBoolApplicableForPageConcept == true)
            {
                foreach (DataRowView DRow in mSelectionList)
                {
                    foreach (DataRow DRPage in mselectionPageList)
                    {
                        if (DRPage.Table.Rows.Count > 0 && Convert.ToString(DRow.Row["STOCK_ID"]) == Convert.ToString(DRPage["STOCK_ID"]))
                        {
                            mselectionPageList.Remove(DRPage);
                            break;
                        }
                    }
                }
            }
            mSelectionList.Clear();
            InvalidateGrid();
        }
        public void ClearSelectionPage()
        {
            mselectionPageList.Clear();
            InvalidateGrid();
        }

        public void SelectGroup(int pIntRowHandle, bool pBoolselect)
        {
            if (IsGroupRowSelected(pIntRowHandle) && pBoolselect)
                return;
            for (int i = 0; i <= mGridview.GetChildRowCount(pIntRowHandle) - 1; i++)
            {
                int childRowHandle = mGridview.GetChildRowHandle(pIntRowHandle, i);
                if (mGridview.IsGroupRow(childRowHandle))
                {
                    SelectGroup(childRowHandle, pBoolselect);
                }
                else
                {
                    SelectRow(childRowHandle, pBoolselect, false);
                }
            }
            InvalidateGrid();
        }
        public void SelectRow(int pIntRowHandle, bool pBoolselect)
        {
            SelectRow(pIntRowHandle, pBoolselect, true);
        }
        public void InvertRowSelection(int pIntRowHandle)
        {
            if (View.IsDataRow(pIntRowHandle))
            {
                SelectRow(pIntRowHandle, !IsRowSelected(pIntRowHandle));
            }
            if (View.IsGroupRow(pIntRowHandle))
            {
                SelectGroup(pIntRowHandle, !IsGroupRowSelected(pIntRowHandle));
            }
        }
        public bool IsGroupRowSelected(int pIntRowHandle)
        {
            for (int i = 0; i <= mGridview.GetChildRowCount(pIntRowHandle) - 1; i++)
            {
                int IntRow = mGridview.GetChildRowHandle(pIntRowHandle, i);
                if (mGridview.IsGroupRow(IntRow))
                {
                    if (!IsGroupRowSelected(IntRow))
                    {
                        return false;
                    }
                }
                else if (!IsRowSelected(IntRow))
                {
                    return false;
                }
            }
            return true;
        }
        public bool IsRowSelected(int pIntRowHandle)
        {
            if (mGridview.IsGroupRow(pIntRowHandle))
            {
                return IsGroupRowSelected(pIntRowHandle);
            }

            object ObjRow = mGridview.GetRow(pIntRowHandle);
            return GetSelectedIndex(ObjRow) != -1;
        }
        public ArrayList GetSelectedArrayList()
        {
            return mSelectionList;
        }

        public ArrayList GetSelectedArrayListPage()
        {
            return mselectionPageList;
        }

        protected virtual void AttachGrid(GridView pGridView)
        {
            if (pGridView == null)
                return;
            mSelectionList.Clear();
            this.mGridview = pGridView;

            try
            {
                pGridView.BeginUpdate();

                if (pGridView.GridControl == null)
                    return;

                mRepEdit = pGridView.GridControl.RepositoryItems.Add("CheckEdit") as RepositoryItemCheckEdit;
                mRepEdit.CheckStyle = CheckStyles.UserDefined;
                mRepEdit.PictureChecked = Properties.Resources.Checked;
                mRepEdit.PictureUnchecked = Properties.Resources.Unchecked;
                mGridcolumn = pGridView.Columns.Add();
                mGridcolumn.Visible = true;
                mGridcolumn.OptionsColumn.AllowSort = DevExpress.Utils.DefaultBoolean.False;
                mGridcolumn.FieldName = "COLSELECTCHECKBOX";
                mGridcolumn.VisibleIndex = 0;
                mGridcolumn.Caption = "Sel";

                mGridcolumn.OptionsColumn.AllowEdit = false;
                mGridcolumn.OptionsColumn.AllowSize = false;
                mGridcolumn.OptionsColumn.ShowCaption = false;
                mGridcolumn.UnboundType = DevExpress.Data.UnboundColumnType.Boolean;
                mGridcolumn.Width = GetCheckBoxWidthBasedOnColumn();
                mGridcolumn.ColumnEdit = mRepEdit;

                pGridView.Click += GridView_Click;
                pGridView.CustomDrawColumnHeader += GridView_CustomDrawColumnHeader;
                pGridView.CustomDrawGroupRow += GridView_CustomDrawGroupRow;
                pGridView.CustomUnboundColumnData += GridView_CustomUnboundColumnData;
                pGridView.KeyDown += GridView_KeyDown;
                pGridView.RowStyle += GridView_RowStyle;

            }
            finally
            {
                pGridView.EndUpdate();
            }
        }
        protected virtual void DetachGrid()
        {
            if (mGridview == null)
            {
                return;
            }
            if (mGridcolumn != null)
            {
                mGridcolumn.Dispose();
            }
            if (mRepEdit != null)
            {
                mGridview.GridControl.RepositoryItems.Remove(mRepEdit);
                mRepEdit.Dispose();
            }

            View.Click += GridView_Click;
            View.KeyDown += GridView_KeyDown;
            View.RowStyle += GridView_RowStyle;
            View.CustomDrawColumnHeader += GridView_CustomDrawColumnHeader;
            View.CustomDrawGroupRow += GridView_CustomDrawGroupRow;
            View.CustomUnboundColumnData += GridView_CustomUnboundColumnData;

            mGridview = null;
        }
        protected int GetCheckBoxWidthBasedOnColumn()
        {
            DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo ChkInfo = mRepEdit.CreateViewInfo() as DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo;
            int IntWidth = 0;
            GraphicsInfo.Default.AddGraphics(null);
            try
            {
                IntWidth = ChkInfo.CalcBestFit(GraphicsInfo.Default.Graphics).Width;
            }
            finally
            {
                GraphicsInfo.Default.ReleaseGraphics();
            }
            return IntWidth + mIntCheckboxIndent * 2;
        }

        protected void DrawCheckBox(Graphics pGraphics, Rectangle pRectangle, bool pBoolChecked)
        {
            DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo ChkInfo = default(DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo);
            DevExpress.XtraEditors.Drawing.CheckEditPainter ChkPainter = default(DevExpress.XtraEditors.Drawing.CheckEditPainter);
            DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs ContArgs = default(DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs);
            ChkPainter = mRepEdit.CreatePainter() as DevExpress.XtraEditors.Drawing.CheckEditPainter;
            ChkInfo = mRepEdit.CreateViewInfo() as DevExpress.XtraEditors.ViewInfo.CheckEditViewInfo;
            ChkInfo.EditValue = pBoolChecked;
            ChkInfo.Bounds = pRectangle;
            ChkInfo.CalcViewInfo(pGraphics);
            ContArgs = new DevExpress.XtraEditors.Drawing.ControlGraphicsInfoArgs(ChkInfo, new DevExpress.Utils.Drawing.GraphicsCache(pGraphics), pRectangle);
            ChkPainter.Draw(ContArgs);
            ContArgs.Cache.Dispose();
        }
        private void InvalidateGrid()
        {
            mGridview.CloseEditor();
            mGridview.BeginUpdate();
            mGridview.EndUpdate();
        }
        private void SelectRow(int pIntRowHandle, bool pBoolSelect, bool pBoolInvalidate)
        {
            if (IsRowSelected(pIntRowHandle) == pBoolSelect)
            {
                return;
            }
            object ObjRow = mGridview.GetRow(pIntRowHandle);

            if (pBoolSelect)
            {
                mSelectionList.Add(ObjRow);

                if (ISBoolApplicableForPageConcept == true)
                {
                    DataRow DR = mGridview.GetDataRow(pIntRowHandle);
                    bool ISExists = false;
                    foreach (DataRow DRow in mselectionPageList)
                    {
                        if (DRow.Table.Rows.Count > 0 && Convert.ToString(DRow["STOCK_ID"]) == Convert.ToString(DR["STOCK_ID"]))
                        {
                            ISExists = true;
                            break;
                        }
                    }
                    if (ISExists == false)
                        mselectionPageList.Add(DR);
                }
            }
            else
            {
                mSelectionList.Remove(ObjRow);

                if (ISBoolApplicableForPageConcept == true)
                {
                    DataRow DR = mGridview.GetDataRow(pIntRowHandle);
                    foreach (DataRow DRow in mselectionPageList)
                    {
                        if (DRow.Table.Rows.Count > 0 && Convert.ToString(DRow["STOCK_ID"]) == Convert.ToString(DR["STOCK_ID"]))
                        {
                            mselectionPageList.Remove(DRow);
                            break;
                        }
                    }
                }
            }

            if (pBoolInvalidate)
                InvalidateGrid();
        }

        public void SelectAll()
        {
            mSelectionList.Clear();

            for (int i = 0; i <= mGridview.DataRowCount - 1; i++)
            {
                mSelectionList.Add(mGridview.GetRow(i));
                if (ISBoolApplicableForPageConcept == true)
                {
                    DataRow DR = mGridview.GetDataRow(i);
                    bool ISExists = false;
                    foreach (DataRow DRow in mselectionPageList)
                    {
                        if (DRow.Table.Rows.Count > 0 && Convert.ToString(DRow["STOCK_ID"]) == Convert.ToString(DR["STOCK_ID"]))
                        {
                            ISExists = true;
                            break;
                        }
                    }
                    if (ISExists == false)
                        mselectionPageList.Add(DR);
                }

            }
            InvalidateGrid();
        }
        private void GridView_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
        {
            if (object.ReferenceEquals(e.Column, CheckMarkColumn))
            {
                if (e.IsGetData)
                    e.Value = IsRowSelected(View.GetRowHandle(e.ListSourceRowIndex));
                else
                    SelectRow(View.GetRowHandle(e.ListSourceRowIndex), Convert.ToBoolean(e.Value));
            }
        }
        private void GridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (!object.ReferenceEquals(View.FocusedColumn, mGridcolumn) || e.KeyCode != Keys.Space)
                return;
            InvertRowSelection(View.FocusedRowHandle);
        }
        private void GridView_Click(object sender, EventArgs e)
        {
            GridHitInfo GridInfo = default(GridHitInfo);
            Point Pnt = mGridview.GridControl.PointToClient(Control.MousePosition);
            GridInfo = mGridview.CalcHitInfo(Pnt);
            if (object.ReferenceEquals(GridInfo.Column, mGridcolumn))
            {
                if (GridInfo.InColumn)
                {
                    if (SelectedCount == mGridview.DataRowCount)
                        ClearSelection();
                    //else    //Cmnt & Add : #P : 19-10-2022
                    //    SelectAll();
                    else if (ISCreateHeaderWithCheckBox)
                        SelectAll();
                }
                if (GridInfo.InRowCell)
                    InvertRowSelection(GridInfo.RowHandle);
            }

            if (GridInfo.InRow && mGridview.IsGroupRow(GridInfo.RowHandle) && GridInfo.HitTest != GridHitTest.RowGroupButton)
                InvertRowSelection(GridInfo.RowHandle);
        }

        private void GridView_CustomDrawColumnHeader(object sender, ColumnHeaderCustomDrawEventArgs e)
        {
            if (object.ReferenceEquals(e.Column, mGridcolumn) && ISCreateHeaderWithCheckBox)
            {
                e.Info.InnerElements.Clear();
                e.Painter.DrawObject(e.Info);
                DrawCheckBox(e.Graphics, e.Bounds, SelectedCount == mGridview.DataRowCount);
                e.Handled = true;
            }
        }
        private void GridView_CustomDrawGroupRow(object sender, RowObjectCustomDrawEventArgs e)
        {
            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridGroupRowInfo GridInfo = default(DevExpress.XtraGrid.Views.Grid.ViewInfo.GridGroupRowInfo);
            GridInfo = e.Info as DevExpress.XtraGrid.Views.Grid.ViewInfo.GridGroupRowInfo;

            GridInfo.GroupText = "         " + GridInfo.GroupText.TrimStart();
            e.Info.Paint.FillRectangle(e.Graphics, e.Appearance.GetBackBrush(e.Cache), e.Bounds);
            e.Painter.DrawObject(e.Info);

            Rectangle Rct = GridInfo.ButtonBounds;
            Rct.Offset(Rct.Width + mIntCheckboxIndent, 0);
            Rct.Y = Rct.Y - 3;
            DrawCheckBox(e.Graphics, Rct, IsGroupRowSelected(e.RowHandle));
            e.Handled = true;
        }
        private void GridView_RowStyle(object sender, RowStyleEventArgs e)
        {
            if (IsRowSelected(e.RowHandle))
            {
                if (mBoolSelectionColrFlag)
                {
                    //e.Appearance.BackColor = SystemColors.Highlight;
                    //e.Appearance.BackColor2 = SystemColors.Highlight;
                    //e.Appearance.ForeColor = SystemColors.HighlightText;

                    e.Appearance.BackColor = Color.FromArgb(224, 224, 224);
                    e.Appearance.BackColor2 = Color.FromArgb(224, 224, 224);
                    e.Appearance.ForeColor = Color.Black;
                }
            }
        }

        public DataTable SelectedRowTable()
        {
            DataTable DTab = new DataTable();
            if (mGridview.DataSource != null)
                DTab = ((DataView)mGridview.DataSource).ToTable().Copy().Clone();
            if (mSelectionList.Count > 0)
                DTab = mSelectionList.Cast<DataRowView>().Select(r => r.Row).CopyToDataTable();
            return DTab;
        }


    }
}
