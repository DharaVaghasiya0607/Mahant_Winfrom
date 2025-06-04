using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using DevExpress.XtraPrinting;
using Google.API.Translate;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OfficeOpenXml;
using Spire.Xls;
using DevExpress.Data;
using DevExpress.XtraPrintingLinks;
using System.Drawing.Printing;
using DevExpress.Data.Filtering;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Columns;
using BusLib.Transaction;
using System.Xml;

namespace MahantExport.Stock
{
    public partial class FrmAssignBox : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();
        DataTable DTabBox = new DataTable();

        Guid mTrn_ID;
        Int64 IntMainParty_ID = 0;
        string mStrOpe = "";
        string StrUploadFilename = "";

        double DouCarat = 0;
        double DouCostRapaport = 0;
        double DouCostRapaportAmt = 0;
        double DouCostDisc = 0;
        double DouCostPricePerCarat = 0;
        double DouCostAmount = 0;

        double DouSaleRapaport = 0;
        double DouSaleRapaportAmt = 0;
        double DouSaleDisc = 0;
        double DouSalePricePerCarat = 0;
        double DouSaleAmount = 0;


        #region Property Settings

        public FrmAssignBox()
        {
            InitializeComponent();
        }

        public void ShowForm()
        {
            mStrOpe = "";
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            mTrn_ID = BusLib.Configuration.BOConfiguration.FindNewSequentialID();

            this.Show();

        }
        public void ShowForm(DataTable Dtab)
        {
          
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            GrdDet.BestFitColumns();
           
            DTabBox = Dtab;

            MainGrd.DataSource = DTabBox;
            MainGrd.RefreshDataSource();
           


            this.Show();
            txtAssignBox.Focus();
        }


        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            //ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        #endregion


       
        private void FrmParameterUpdate_Load(object sender, EventArgs e)
        {

        }

        private void txtAssignBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "BOXECODE,BOXNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_BOX);

                    FrmSearch.mStrColumnsToHide = "BOX_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtAssignBox.Text = Val.ToString(FrmSearch.DRow["BOXNAME"]);
                        txtAssignBox.Tag = Val.ToInt32(FrmSearch.DRow["BOX_ID"]);

                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (DTabBox.Rows.Count <= 0)
                {
                    return;
                }
               
                if (txtAssignBox.Text.Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter Box Name");
                    return;
                }

                string AssignBoxUpdateXml;
                using (StringWriter sw = new StringWriter())
                {
                    DTabBox.WriteXml(sw);
                    AssignBoxUpdateXml = sw.ToString();
                }

                LiveStockProperty Property = new LiveStockProperty();
                Property = new BOTRN_StockUpload().SaveAssignBox(AssignBoxUpdateXml, Val.ToInt32(txtAssignBox.Tag));

                this.Cursor = Cursors.Default;

                Global.Message(Property.ReturnMessageDesc);
                
                if (Property.ReturnMessageType == "SUCCESS")
                {
                    this.Close();
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }

    }
}
