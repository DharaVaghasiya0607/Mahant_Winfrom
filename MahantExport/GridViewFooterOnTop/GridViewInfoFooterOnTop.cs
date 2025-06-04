// Developer Express Code Central Example:
// How to provide the capability to display a footer at the top of a GridView
// 
// The current example illustrates how to customize the default GridView, so that
// it can display a footer at the top of a view.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E3029

// Developer Express Code Central Example:
// How to provide the capability to display a footer at the top of a GridView
// 
// The current example illustrates how to customize the default GridView, so that
// it can display a footer at the top of a view.
// 
// You can find sample updates and versions for different programming languages here:
// http://www.devexpress.com/example=E3029

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Grid;

namespace GridViewFooter
{
    class GridViewInfoFooterOnTop : GridViewInfo
    {
        public GridViewInfoFooterOnTop(GridView gridView) : base(gridView) { }

        public override void CalcRects(System.Drawing.Rectangle bounds, bool partital)
        {
            GridViewFooterOnTop gv = View as GridViewFooterOnTop;
            Rectangle r = Rectangle.Empty;
            ViewRects.Bounds = bounds;
            ViewRects.Scroll = CalcScrollRect();
            ViewRects.Client = CalcClientRect();
            FilterPanel.Bounds = Rectangle.Empty;
            if (!partital)
            {
                CalcRectsConstants();
            }
            if (gv.OptionsView.ShowIndicator)
            {
                ViewRects.IndicatorWidth = Math.Max(View.IndicatorWidth, ViewRects.MinIndicatorWidth);
            }
            int minTop = ViewRects.Client.Top;
            int maxBottom = ViewRects.Client.Bottom;
            if (gv.OptionsView.ShowViewCaption)
            {
                r = ViewRects.Client;
                r.Y = minTop;
                r.Height = CalcViewCaptionHeight(ViewRects.Client);
                ViewRects.ViewCaption = r;
                minTop = ViewRects.ViewCaption.Bottom;
            }
            minTop = UpdateFindControlVisibility(new Rectangle(ViewRects.Client.X, minTop, ViewRects.Client.Width, maxBottom - minTop), true).Y;
            if (gv.OptionsView.ShowGroupPanel)
            {
                r = ViewRects.Client;
                r.Y = minTop;
                r.Height = CalcGroupPanelHeight();
                ViewRects.GroupPanel = r;
                minTop = ViewRects.GroupPanel.Bottom;
            }
            minTop = CalcRectsColumnPanel(minTop);
            if (gv.IsShowFilterPanel)
            {
                r = ViewRects.Client;
                int fPanel = GetFilterPanelHeight();
                r.Y = maxBottom - fPanel;
                r.Height = fPanel;
                FilterPanel.Bounds = r;
                maxBottom = r.Top;
            }
            if (gv.OptionsView.ShowFooter)
            {
                r = ViewRects.Client;
                r.Height = GetFooterPanelHeight();

                if (gv.OptionsView.FooterLocation == FooterPosition.Top)
                {
                    r.Y = minTop;
                    ViewRects.Footer = r;
                    minTop = ViewRects.Footer.Bottom;
                }
                else
                {
                    r.Y = maxBottom - r.Height;
                    ViewRects.Footer = r;
                    maxBottom = r.Top;
                }
            }
            r = ViewRects.Client;
            r.Y = minTop;
            r.Height = maxBottom - minTop;
            ViewRects.Rows = r;
        }

       
        public override int GetRowFooterCount(int rowHandle, int nextRowHandle, bool isExpanded)
        {
            if (View.OptionsView.FooterLocation == FooterPosition.Top)              
            {
                if (View.IsGroupRow(rowHandle) && isExpanded)
                {
                    int childCount = View.GetChildRowCount(rowHandle);
                    rowHandle = View.GetChildRowHandle(rowHandle, childCount - 1);
                    int rowVisibleIndex = View.GetVisibleIndex(rowHandle);
                    nextRowHandle = View.GetVisibleRowHandle(View.GetNextVisibleRow(rowVisibleIndex));
                    return base.GetRowFooterCount(rowHandle, nextRowHandle, isExpanded);
                }
                return 0;
            }
            return base.GetRowFooterCount(rowHandle, nextRowHandle, isExpanded);
        }

        protected override void CalcRowCellsFooterInfo(GridRowFooterInfo fi, GridRowInfo ri)
        {
            if (View.OptionsView.FooterLocation == FooterPosition.Top && View.IsGroupRow(ri.RowHandle))
                fi.RowHandle = ri.RowHandle;
            base.CalcRowCellsFooterInfo(fi, ri);
        }

        new GridViewFooterOnTop View
        {
            get { return base.View as GridViewFooterOnTop; }
        }

    }
}
