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
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Base.Handler;
using DevExpress.XtraGrid.Views.Base.ViewInfo;
using DevExpress.XtraGrid.Registrator;

namespace GridViewFooter
{
    class GridFooterOnTopViewInfoRegistrator : GridInfoRegistrator
    {
        public override string ViewName { get { return "GridViewFooterOnTop"; } }
        public override BaseView CreateView(GridControl grid)
            { return new GridViewFooterOnTop(grid as GridControl); }
        public override BaseViewInfo CreateViewInfo(BaseView view)
            { return new GridViewInfoFooterOnTop(view as GridViewFooterOnTop); }
    }
}
