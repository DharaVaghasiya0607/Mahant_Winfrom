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
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.Utils.Serializing;
using System.ComponentModel;

namespace GridViewFooter
{
    class GridViewFooterOnTop : GridView
    {
        public GridViewFooterOnTop(DevExpress.XtraGrid.GridControl grid) : base(grid) { }
        public GridViewFooterOnTop() : this(null) { }
        protected override string ViewName { get { return "GridViewFooterOnTop"; } }

        protected override DevExpress.XtraGrid.Views.Base.ColumnViewOptionsView CreateOptionsView()
            { return new GridFooterOnTopOptionsView(); }

        [Description("Provides access to the View's display options."), Category("Options"), DesignerSerializationVisibility(DesignerSerializationVisibility.Content),
        XtraSerializableProperty(XtraSerializationVisibility.Content, XtraSerializationFlags.DefaultValue),
        XtraSerializablePropertyId(LayoutIdOptionsView)]
        public new GridFooterOnTopOptionsView OptionsView { get { return base.OptionsView as GridFooterOnTopOptionsView; } }
    }
}
