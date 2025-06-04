using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
//using DevExpress.XtraGrid;
//using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
//using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Printing;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;

namespace MahantExport.Report
{
    public partial class FrmCLVPriceChartSummary  : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_ParcelParameterDiscount ObjTrn = new BOTRN_ParcelParameterDiscount();
        BOFormPer ObjPer = new BOFormPer();

        DataTable DTab = new DataTable();

        public FrmCLVPriceChartSummary()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            AttachFormDefaultEvent();
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            if (ObjPer.ISVIEW == false)
            {
                Global.MessageError(BusLib.TPV.BOMessage.ViewDeniedMsg);
                return;
            }

            this.Show();
            cmbPricingType.SelectedIndex = 0;
            txtParameter.Focus();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjTrn);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);

        }

        private void BtnShow_Click(object sender, EventArgs e)
        {
            if (Val.ToString(txtParameter.Text) == "")
            {
                Global.Message("Please Select Parameter ID First");
                txtParameter.Focus();
                return;
            }
            if (Val.ToString(txtRepDate.Text) == "")
            {
                Global.Message("Please Select Rap Date");
                txtRepDate.Focus();
                return;
            }

            this.Cursor = Cursors.WaitCursor;

            double DouFromSize = 0;
            double DouToSize = 0;
            string StrPricingType;

            if (Val.ToString(txtSize.Text) != "")
            {
                DouFromSize = Val.Val(Val.ToString(txtSize.Text).Split('-')[0]);
                DouToSize = Val.Val(Val.ToString(txtSize.Text).Split('-')[1]);
            }

            DTab = ObjTrn.GetClvMixPriceChartData("DISCOUNT", Val.ToString(txtParameter.Tag), Val.SqlDate(txtRepDate.Text), Val.ToString(txtShape.Text), DouFromSize, DouToSize, Val.ToString(cmbPricingType.Text));

            MainGrid.DataSource = DTab;
            MainGrid.Refresh();
            GrdDet.BestFitColumns();

            this.Cursor = Cursors.Default;
        }

        private void txtParameter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Global.OnKeyPressEveToPopup(e))
            {
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "PARAMETER_ID,PARAMETER_NAME";
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                this.Cursor = Cursors.WaitCursor;

                FrmSearch.mDTab = ObjTrn.GetParameterDiscountData("PARAMETER", "", "", "", 0, 0, Val.ToString(cmbPricingType.Text));

                FrmSearch.mStrColumnsToHide = "";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.DRow != null)
                {
                    txtParameter.Text = Val.ToString(FrmSearch.DRow["PARAMETER_ID"]);
                    txtParameter.Tag = Val.ToString(FrmSearch.DRow["PARAMETER_ID"]);

                    txtRepDate.Tag = string.Empty;
                    txtRepDate.Text = string.Empty;
                    txtShape.Tag = string.Empty;
                    txtShape.Text = string.Empty;
                    txtSize.Tag = string.Empty;
                    txtSize.Text = string.Empty;
                }

                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
        }

        private void txtShape_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Global.OnKeyPressEveToPopup(e))
            {
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "SHAPE";
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                this.Cursor = Cursors.WaitCursor;

                FrmSearch.mDTab = ObjTrn.GetParameterDiscountData("SHAPE", Val.ToString(txtParameter.Tag), Val.SqlDate(txtRepDate.Text), "", 0, 0, Val.ToString(cmbPricingType.Text));

                FrmSearch.mStrColumnsToHide = "";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.DRow != null)
                {
                    txtShape.Text = Val.ToString(FrmSearch.DRow["SHAPE"]);
                    txtShape.Tag = Val.ToString(FrmSearch.DRow["SHAPE_ID"]);

                    txtSize.Tag = string.Empty;
                    txtSize.Text = string.Empty;
                }

                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
        }

        private void txtRepDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Global.OnKeyPressEveToPopup(e))
            {
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "RAPDATE";
                FrmSearch.mStrSearchText = e.KeyChar.ToString();
                this.Cursor = Cursors.WaitCursor;

                //FrmSearch.mDTab = ObjTrn.GetParameterDiscountData("RAPDATE", Val.ToString(txtParameter.Tag), "", "", 0, 0, Val.ToString(cmbPricingType.Text));
                FrmSearch.mDTab = ObjTrn.GetParameterDiscountData("RAPDATE", Val.ToString(txtParameter.Tag), "", "", 0, 0, Val.ToString(cmbPricingType.Text));

                FrmSearch.mStrColumnsToHide = "";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                e.Handled = true;
                if (FrmSearch.DRow != null)
                {
                    txtRepDate.Text = Val.ToString(FrmSearch.DRow["RAPDATE"]);
                    txtRepDate.Tag = Val.ToString(FrmSearch.DRow["RAPDATE"]);

                    txtShape.Tag = string.Empty;
                    txtShape.Text = string.Empty;
                    txtSize.Tag = string.Empty;
                    txtSize.Text = string.Empty;
                }

                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
        }

        private void GrdDet_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            GrdDet.PostEditor();
            if (e.RowHandle < 0)
            {
                return;
            }

            string pStrFeildName = e.Column.FieldName;

            string[] Str = pStrFeildName.Split('_');

            if (e.Column.FieldName.Contains("BACK"))
            {
                double pDouPricePerCarat = 0;
                double pDouRapaport = 0;
                double pDouDis = 0;

                pDouRapaport = Val.Val(GrdDet.GetRowCellValue(e.RowHandle,Str[0] + "_" + "RAPAPORT"));
                pDouDis = Val.Val(GrdDet.GetRowCellValue(e.RowHandle,Str[0] + "_" + Str[1]));
                pDouPricePerCarat = Math.Round(Val.Val(pDouRapaport - (-(pDouRapaport * pDouDis) / 100)),2);
                GrdDet.SetRowCellValue(e.RowHandle,Str[0] + "_" + "PERCTS",pDouPricePerCarat);
            }

            if (e.Column.FieldName.Contains("PERCTS")
               )
            {

                DataRow DRow = GrdDet.GetDataRow(e.RowHandle);

                if (Val.ToString(DRow["COLORNAME"]).Trim().Equals(string.Empty) || Val.ToString(DRow["SHAPE"]).Trim().Equals(string.Empty) || Val.Val(DRow["F_CARAT"]) <= 0 || Val.Val(DRow["T_CARAT"]) <= 0)
                    return;

                ParameterDiscountProperty Property = new ParameterDiscountProperty();

                Property.F_CARAT = Val.Val(DRow["F_CARAT"]);
                Property.T_CARAT = Val.Val(DRow["T_CARAT"]);

                Property.SHAPE_ID = Val.ToInt(DRow["SHAPE_ID"]);
                Property.COLOR_ID = Val.ToInt(DRow["COLOR_ID"]);

                Property.Q_CODE = e.Column.FieldName;
                Property.Q_NAME = e.Column.Caption;
                Property.RAPDATE = Val.SqlDate(Val.ToString(DRow["RAPDATE"]));
                Property.PARAMETER_ID = Val.ToString(DRow["PARAMETER_ID"]);
                Property.PARAMETER_VALUE = Val.ToString(DRow["PARAMETER_VALUE"]);
                Property.PRICINGTYPE = Val.ToString(DRow["PRICINGTYPE"]);

                if (Property.F_CARAT == 0 || Property.T_CARAT == 0 || Property.SHAPE_ID == 0 || Property.COLOR_ID == 0 || Property.Q_CODE == "" || Property.PARAMETER_ID == "")
                {
                    Global.Message("Some Data Has Been Missing");
                    return;
                }

                double OldValue = Val.Val(GrdDet.ActiveEditor.OldEditValue);

                Property.OLDVALUE = Val.Val(GrdDet.ActiveEditor.OldEditValue);
                Property.NEWVALUE = Val.Val(DRow[e.Column.FieldName, DataRowVersion.Default]);
                Property = ObjTrn.SaveParcelParameterDiscount(Property);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    lblMessage.Text = e.Column.Caption + " Value Updateed From [ " + Property.OLDVALUE.ToString() + " ]  To [ " + Property.NEWVALUE.ToString() + " ] ";
                }
                else
                {
                    lblMessage.Text = "Error......";
                }

                GrdDet.RefreshData();
                DTab.AcceptChanges();
                Property = null;

                BtnShow_Click(null, null);
            }
        }


        private void BtnSave_Click(object sender, EventArgs e)
        {

        }

        private void MainGrid_PaintEx(object sender, PaintExEventArgs e)
        {
            GridControl gridC = sender as GridControl;
            GridView gridView = gridC.FocusedView as GridView;
            BandedGridViewInfo info = (BandedGridViewInfo)gridView.GetViewInfo();
            for (int i = 0; i < info.BandsInfo.BandCount; i++)
            {
                e.Cache.Graphics.DrawLine(new Pen(Brushes.Black, 1), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.BandsInfo[i].Bounds.Top), new Point(info.BandsInfo[i].Bounds.X + info.BandsInfo[i].Bounds.Width, info.RowsInfo[info.RowsInfo.Count - 1].Bounds.Bottom - 1));

            }
        }
    }
}
