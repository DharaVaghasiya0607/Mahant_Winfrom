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

namespace GridViewFooter
{
    class GridControlFooterOnTop : GridControl
    {
        protected override void RegisterAvailableViewsCore(DevExpress.XtraGrid.Registrator.InfoCollection collection)
        {
            base.RegisterAvailableViewsCore(collection);
            collection.Add(new GridFooterOnTopViewInfoRegistrator());
        }

        protected override DevExpress.XtraGrid.Views.Base.BaseView CreateDefaultView()
            { return CreateView("GridViewFooterOnTop"); }
    }
}
