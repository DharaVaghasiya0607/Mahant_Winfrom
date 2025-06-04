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
using DevExpress.Utils.Controls;

namespace GridViewFooter
{
    public enum FooterPosition { Top, Bottom };

    class GridFooterOnTopOptionsView : GridOptionsView
    {
        FooterPosition footerPosition;
        public GridFooterOnTopOptionsView()
            : base()
        { this.footerPosition = FooterPosition.Bottom; }

        public virtual FooterPosition FooterLocation
        {
            get { return footerPosition; }
            set 
            {
                FooterPosition prevValue = FooterLocation;
                footerPosition = value;
                OnChanged(new BaseOptionChangedEventArgs("FooterLocation", prevValue, ShowFooter));
            }
        }

        public override void Assign(DevExpress.Utils.Controls.BaseOptions options)
        {
            BeginUpdate();
            try
            {
                base.Assign(options);
                GridFooterOnTopOptionsView opt = options as GridFooterOnTopOptionsView;
                if (opt == null) return;
                this.footerPosition = opt.footerPosition;
            }
            finally
            {
                EndUpdate();
            }
        }
    }
}
