using MahantExport.CRM;
using MahantExport.Masters;
using MahantExport.Utility;
using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraPrinting;
using Google.API.Translate;
using OfficeOpenXml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BusLib.CRM;

namespace MahantExport.Stock
{
    public partial class FrmDimandView : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_StockUpload ObjStock = new BOTRN_StockUpload();

        DataTable DTabDetail = new DataTable();


        string StrCoustmoerName = "";
        string StrEmail = "";
        string StrFilePath = "";


        DataSet DS = new DataSet();

        CRMCustomerAutoMailCriteriaProperty mLStockProperty = new CRMCustomerAutoMailCriteriaProperty();
        LiveStockProperty mCRMStockProperty = new LiveStockProperty();

        string StrCoustomer_ID = "";

        #region Property Settings

        public FrmDimandView()
        {
            InitializeComponent();
        }

        //HINA - START
        public FORMTYPE mFormType = FORMTYPE.SINGLELIVESTOCK;
        public enum FORMTYPE
        {
            SINGLELIVESTOCK = 1,
            PARCELLIVESTOCK = 2
        }

        public void ShowForm()
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            this.Show();

            DataTable DTabCustomer = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);
            if (DTabCustomer.Rows.Count > 0)
            {
                DTabCustomer.DefaultView.Sort = "PARTYNAME";
                ChkCmbParty.Properties.DataSource = DTabCustomer;
                ChkCmbParty.Properties.DisplayMember = "PARTYNAME";
                ChkCmbParty.Properties.ValueMember = "PARTY_ID";
            }

        }

        public void ShowForm(FORMTYPE pFormType) //HINA - END
        {
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            //HINA - START
            mFormType = pFormType;

        }

        //ADD BHAGYASHREE 30/07/2019
        public void ShowForm(string StrFromSize, string StrToSize, string StrColor_ID, string StrClarity_ID, string StrShapName)  //Call When Double Click On Current Stock Color Clarity Wise Report Data
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            this.Show();
        }

        public void ShowForm(string PurchaseName, string SalesName, string SaleJangadNo, string PurchaseJangadno)
        {
            try
            {
                ShowForm();
                //ChkCmbParty.Properties.Items[partyName].CheckState = CheckState.Checked;
                ChkCmbParty.SetEditValue(PurchaseName);

            }
            catch
            {
            }
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
            ObjFormEvent.ObjToDisposeList.Add(ObjStock);
        }

        #endregion



        #region Enter Event

        private void ControlEnterForGujarati_Enter(object sender, EventArgs e)
        {
            Global.SelectLanguage(Global.LANGUAGE.GUJARATI);
        }
        private void ControlEnterForEnglish_Enter(object sender, EventArgs e)
        {
            Global.SelectLanguage(Global.LANGUAGE.ENGLISH);
        }


        #endregion

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBestFit_Click(object sender, EventArgs e)
        {
            GrdDetail.BestFitColumns();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void FrmMemoEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            BtnSearch.PerformClick();
        }

        private void lblSaveLayout_Click(object sender, EventArgs e)
        {
            Stream str = new System.IO.MemoryStream();
            GrdDetail.SaveLayoutToStream(str);
            str.Seek(0, System.IO.SeekOrigin.Begin);
            StreamReader reader = new StreamReader(str);
            string text = reader.ReadToEnd();

            int IntRes = new BOTRN_StockUpload().SaveGridLayout(this.Name, GrdDetail.Name, text);
            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Saved");
            }
        }

        private void lblDefaultLayout_Click(object sender, EventArgs e)
        {
            int IntRes = new BOTRN_StockUpload().DeleteGridLayout(this.Name, GrdDetail.Name);
            if (IntRes != -1)
            {
                Global.Message("Layout Successfully Deleted");
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            StrCoustomer_ID = Val.Trim(ChkCmbParty.Properties.GetCheckedItems());

            DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
            DTabDetail.Rows.Clear();
            PanelProgress.Visible = true;
            if (!backgroundWorker1.IsBusy)
            {
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                DS = ObjStock.GetDataForDemandView(StrCoustomer_ID);
                DTabDetail = DS.Tables[0];
            }
            catch (Exception ex)
            {
                PanelProgress.Visible = false;
                BtnSearch.Enabled = true;
                Global.Message(ex.Message.ToString());
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                PanelProgress.Visible = false;
                BtnSearch.Enabled = true;

                if (DTabDetail.Rows.Count <= 0)
                {
                    Global.Message("No Data Found..");
                }

                GrdDetail.BeginUpdate();

                MainGrdDetail.DataSource = DTabDetail;
                GrdDetail.RefreshData();

                GrdDetail.EndUpdate();

            }
            catch (Exception ex)
            {
                PanelProgress.Visible = false;
                BtnSearch.Enabled = true;
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDetail_RowCellClick(object sender, RowCellClickEventArgs e)
        {
            try
            {
                if (e.RowHandle < 0 || e.Clicks < 2)
                    return;

                this.Cursor = Cursors.WaitCursor;

                string StrCoustomerName = "";
                DataRow Dr = GrdDetail.GetFocusedDataRow();

                DataTable DtabSize = new DataTable();

                StrCoustomerName = Val.ToString(Dr["CUSTOMERNAME"]);

                mLStockProperty = new CRMCustomerAutoMailCriteriaProperty();

                mLStockProperty.AUTOEMAIL_ID = Guid.Parse(Val.ToString(Dr["AutoEmail_ID"]));
                mLStockProperty.CUSTOMER_ID = Guid.Parse(Val.ToString(Dr["COUSTOMER_ID"]));
                mLStockProperty.MULTYSHAPE_ID = Val.ToString(Dr["SHAPE_ID"]);
                mLStockProperty.MULTISHAPENAME = Val.ToString(Dr["SHAPENAME"]);
                mLStockProperty.MULTYKAPAN_ID = Val.ToString(Dr["KAPAN_ID"]);
                mLStockProperty.MULTIKAPANNAME = Val.ToString(Dr["KAPANNAME"]);
                mLStockProperty.MULTYCOLOR_ID = Val.ToString(Dr["COLOR_ID"]);
                mLStockProperty.MULTYCOLORNAME = Val.ToString(Dr["COLORNAME"]);
                mLStockProperty.MULTYCLARITY_ID = Val.ToString(Dr["CLARITY_ID"]);
                mLStockProperty.MULTYCLARITYNAME = Val.ToString(Dr["CLARITYNAME"]);
                mLStockProperty.MULTYCUT_ID = Val.ToString(Dr["CUT_ID"]);
                mLStockProperty.MULTYCUTNAME = Val.ToString(Dr["CUTNAME"]);
                mLStockProperty.MULTYPOL_ID = Val.ToString(Dr["POL_ID"]);
                mLStockProperty.MULTYPOLNAME = Val.ToString(Dr["POLNAME"]);
                mLStockProperty.MULTYSYM_ID = Val.ToString(Dr["SYM_ID"]);
                mLStockProperty.MULTYSYMNAME = Val.ToString(Dr["SYMNAME"]);
                mLStockProperty.MULTYFL_ID = Val.ToString(Dr["FL_ID"]);
                mLStockProperty.MULTYFLNAME = Val.ToString(Dr["FLNAME"]);
                mLStockProperty.MULTYCOLORSHADE_ID = Val.ToString(Dr["COLORSHADE_ID"]);
                mLStockProperty.MULTYCOLORSHADENAME = Val.ToString(Dr["COLORSHADENAME"]);
                mLStockProperty.VALIDDATE = Val.ToString(Dr["VALIDDATE"]);
                mLStockProperty.LABFROMDATE = Val.ToString(Dr["LABFROMDATE"]);
                mLStockProperty.LABTODATE = Val.ToString(Dr["LABTODATE"]);
                mLStockProperty.AVAILBLEFROMDATE = Val.ToString(Dr["AVAILBLEFROMDATE"]);
                mLStockProperty.AVAILBLETODATE = Val.ToString(Dr["AVAILBLETODATE"]);
                mLStockProperty.UPLOADFROMDATE = Val.ToString(Dr["UPLOADFROMDATE"]);
                mLStockProperty.UPLOADTODATE = Val.ToString(Dr["UPLOADTODATE"]);
                mLStockProperty.FROMLABRETURNDATE = Val.ToString(Dr["FROMLABRETURNDATE"]);
                mLStockProperty.TOLABRETURNDATE = Val.ToString(Dr["TOLABRETURNDATE"]);
                mLStockProperty.FROMLABRESULTDATE = Val.ToString(Dr["FROMLABRESULTDATE"]);
                mLStockProperty.LABRESULTTODATE = Val.ToString(Dr["LABRESULTTODATE"]);
                mLStockProperty.SALESFROMDATE = Val.ToString(Dr["SALESFROMDATE"]);
                mLStockProperty.SALESTODATE = Val.ToString(Dr["SALESTODATE"]);
                mLStockProperty.PRICEREVICESFROMDATE = Val.ToString(Dr["PRICEREVICESFROMDATE"]);
                mLStockProperty.PRICEREVICESTODATE = Val.ToString(Dr["PRICEREVICESTODATE"]);
                mLStockProperty.DELIVERYFROMDATE = Val.ToString(Dr["DELIVERYFROMDATE"]);
                mLStockProperty.DELIVERYTODATE = Val.ToString(Dr["DELIVERYTODATE"]);

                mLStockProperty.MULTYFANCYCOLOR_ID = Val.ToString(Dr["FANCYCOLOR_ID"]);
                mLStockProperty.MULTYFANCYCOLORNAME = Val.ToString(Dr["FANCYCOLORNAME"]);
                mLStockProperty.MULTYLOCATION_ID = Val.ToString(Dr["LOCATION_ID"]);
                mLStockProperty.MULTYLOCATIONNAME = Val.ToString(Dr["LOCATIONNAME"]);
                mLStockProperty.MULTYMILKY_ID = Val.ToString(Dr["MILKY_ID"]);
                mLStockProperty.MULTYMILKYNAME = Val.ToString(Dr["MILKYNAME"]);

                mLStockProperty.MULTYLAB_ID = Val.ToString(Dr["LAB_ID"]);
                mLStockProperty.MULTYLABNAME = Val.ToString(Dr["LABNAME"]);
                mLStockProperty.MULTYBOX_ID = Val.ToString(Dr["BOX_ID"]);
                mLStockProperty.MULTYBOXNAME = Val.ToString(Dr["BOXNAME"]);
                mLStockProperty.WEBSTATUS_ID = Val.ToString(Dr["WEBSTATUS_ID"]);
                mLStockProperty.WEBSTATUSNAME = Val.ToString(Dr["WEBSTATUS_ID"]);

                mLStockProperty.MULTYTABLEBLACKINC_ID = Val.ToString(Dr["TABLEBLACKINC_ID"]);
                mLStockProperty.MULTYTABLEBLACKINCNAME = Val.ToString(Dr["TABLEBLACKINCNAME"]); 
                mLStockProperty.MULTYSIDEBLACKINC_ID = Val.ToString(Dr["SIDEBLACKINC_ID"]);
                mLStockProperty.MULTYSIDEBLACKINCNAME = Val.ToString(Dr["SIDEBLACKINCNAME"]);




                mLStockProperty.FROMCARAT1 = Val.Val(Dr["FROMCARAT1"]);
                mLStockProperty.TOCARAT1 = Val.Val(Dr["TOCARAT1"]);

                mLStockProperty.FROMCARAT2 = Val.Val(Dr["FROMCARAT2"]);
                mLStockProperty.TOCARAT2 = Val.Val(Dr["TOCARAT2"]);

                mLStockProperty.FROMCARAT3 = Val.Val(Dr["FROMCARAT3"]);
                mLStockProperty.TOCARAT3 = Val.Val(Dr["TOCARAT3"]);

                mLStockProperty.FROMCARAT4 = Val.Val(Dr["FROMCARAT4"]);
                mLStockProperty.TOCARAT4 = Val.Val(Dr["TOCARAT4"]);

                mLStockProperty.FROMCARAT5 = Val.Val(Dr["FROMCARAT5"]);
                mLStockProperty.TOCARAT5 = Val.Val(Dr["TOCARAT5"]);

                mLStockProperty.FROMLENGTH = Val.Val(Dr["FROMLENGTHPER"]);
                mLStockProperty.TOLENGTH = Val.Val(Dr["TOLENGTHPER"]);

                mLStockProperty.FROMWIDTH = Val.Val(Dr["FROMWIDTHPER"]);
                mLStockProperty.TOWIDTH = Val.Val(Dr["TOWIDTHPER"]);

                mLStockProperty.FROMHEIGHT = Val.Val(Dr["FROMHEIGHTPER"]);
                mLStockProperty.TOHEIGHT = Val.Val(Dr["TOHEIGHTPER"]);

                mLStockProperty.FROMTABLEPER = Val.Val(Dr["FROMTABLEPER"]);
                mLStockProperty.TOTABLEPER = Val.Val(Dr["TOTABLEPER"]);

                mLStockProperty.FROMDEPTHPER = Val.Val(Dr["FROMDEPTHPER"]);
                mLStockProperty.TODEPTHPER = Val.Val(Dr["TODEPTHPER"]);

                mLStockProperty.STOCKNO = Val.ToString(Dr["STONENO"]);

                mLStockProperty.LABREPORTNO = Val.ToString(Dr["CERTINO"]);

                mLStockProperty.SERIALNO = Val.ToString(Dr["SERIALNO"]);

                mLStockProperty.MEMONO = Val.ToString(Dr["MEMONO"]);

                mLStockProperty.EMAILTYPE = Val.ToString(Dr["EMAILTYPE"]);
                mLStockProperty.ISACTIVE = Val.ToBoolean(Dr["ISACTIVE"]);

                mLStockProperty.EMAIL_ID = Val.ToString(Dr["EMAIL_ID"]);

               // StrEmail = Val.ToString(Dr["EMAIL_ID"]);

                if (e.Column.FieldName == "EMAIL")
                {
                    StrFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";

                    DataSet DS = ObjStock.GetDataForExcelExportEmailSettingNew(mLStockProperty);
                    string Result = ExportExcelWithStockList(DS, StrFilePath);

                    string StrTomail = StrEmail;

                    if (Result == "")
                    {
                        if (Global.Confirm("No any attachememnt found\n\nStil you want to send email ?") == System.Windows.Forms.DialogResult.No)
                        {
                            return;
                        }
                    }
                    FrmEmailSend FrmEmailSend = new FrmEmailSend();
                    FrmEmailSend.MdiParent = Global.gMainRef;
                    FrmEmailSend.ShowForm(Result, mLStockProperty.EMAIL_ID);
                }
                else if (e.Column.FieldName == "EXPORT")
                {
                    SaveFileDialog svDialog = new SaveFileDialog();
                    svDialog.DefaultExt = ".xlsx";
                    svDialog.Title = "Export to Excel";
                    svDialog.FileName = BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";
                    svDialog.Filter = "Excel File (*.xlsx)|*.xlsx ";
                    if ((svDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK))
                    {
                        StrFilePath = svDialog.FileName;
                    }
                    if (StrFilePath == "")
                    {
                        Global.Message("Please Select File Path..");
                        return;
                    }
                    DataSet DS = ObjStock.GetDataForExcelExportEmailSettingNew(mLStockProperty);
                    string Result = ExportExcelWithStockList(DS, StrFilePath);
                  
                    if (Global.Confirm("Do You Want To Open File ? ") == System.Windows.Forms.DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(Result, "CMD");
                    }
                   
                }
                //Added by Daksha on 12/01/2023
                else if (e.Column.FieldName=="DELETE")
                {
                    if (Global.Confirm("Are You Sure to Delete this Record ?") == DialogResult.Yes)
                    {
                        this.Cursor = Cursors.WaitCursor;                       
                        CRM_CustomerAutoMailCriteriaSave ObjEmail = new CRM_CustomerAutoMailCriteriaSave();                                                
                        ObjEmail.Delete(mLStockProperty);
                        Global.Message(mLStockProperty.ReturnMessageDesc);
                        if (mLStockProperty.ReturnMessageType == "SUCCESS")
                        {
                            BtnSearch_Click(null, null);
                        }                        
                    }
                }
                //End as Daksha
               
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());                
            }
            finally 
            {
                this.Cursor = Cursors.Default;
            }
        }

        private void FrmCustomerAutoEmailSetting_FormClosing(object sender, FormClosingEventArgs e)
        {
            BtnSearch_Click(null, null);
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            DTabDetail.Rows.Clear();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            string StrFileName = ExportExcel();
            Global.ExcelExport("List", GrdDetail);
            if (Global.Confirm("Do You Want To Open File ? ") == System.Windows.Forms.DialogResult.Yes)
            {
                System.Diagnostics.Process.Start(StrFileName, "CMD");
            }
            
        }

        private string ExportExcelHeader(string pStrHeader, ExcelWorksheet worksheet, int pCol)
        {
            if (pStrHeader.ToLower() == "sr")
            {
                worksheet.Column(pCol).Width = 5;
                return "Sr";
            }
            if (pStrHeader.ToLower() == "skeno")
            {
                worksheet.Column(pCol).Width = 8;
                return "SKE No";
            }
            if (pStrHeader.ToLower() == "stockno")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "#Stock";
            }
            if (pStrHeader.ToLower() == "status")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Status";
            }
            if (pStrHeader.ToLower() == "shape")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Shape";
            }
            if (pStrHeader.ToLower() == "carat")
            {
                worksheet.Column(pCol).Width = 7;
                return "Cts";
            }
            if (pStrHeader.ToLower() == "clarity")
            {
                worksheet.Column(pCol).Width = 5;
                return "Cla";
            }
            if (pStrHeader.ToLower() == "color")
            {
                worksheet.Column(pCol).Width = 6;
                return "Col";
            }
            if (pStrHeader.ToLower() == "colorshade")
            {
                worksheet.Column(pCol).Width = 5;
                return "CS";
            }
            if (pStrHeader.ToLower() == "raprate")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Rap Rate";
            }
            if (pStrHeader.ToLower() == "rapvalue")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Rap Value";
            }
            if (pStrHeader.ToLower() == "rapper")
            {
                worksheet.Column(pCol).Width = 6.5;
                return "Rap %";
            }
            if (pStrHeader.ToLower() == "pricepercarat")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Pr/Ct";
            }
            if (pStrHeader.ToLower() == "amount")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Amount";
            }
            if (pStrHeader.ToLower() == "costraprate")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Cost Rap";
            }
            if (pStrHeader.ToLower() == "costrapvalue")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Cost RapValue";
            }
            if (pStrHeader.ToLower() == "costrapper")
            {
                worksheet.Column(pCol).Width = 6.5;
                return "Cost Rap%";
            }
            if (pStrHeader.ToLower() == "costpricepercarat")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Cost Pr/Ct";
            }
            if (pStrHeader.ToLower() == "costamount")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Cost Amt";
            }
            //if (pStrHeader.ToLower() == "expraprate")
            //{
            //    worksheet.Column(pCol).Width = 8.5;
            //    return "Exp Rap Rate";
            //}
            if (pStrHeader.ToLower() == "exprapvalue")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Exp Rap Value";
            }
            if (pStrHeader.ToLower() == "exprapper")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Exp Rap %";
            }
            if (pStrHeader.ToLower() == "exppricepercarat")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Exp Pr/Ct";
            }
            if (pStrHeader.ToLower() == "expamount")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Exp Amt";
            }
            //if (pStrHeader.ToLower() == "rapraprate")
            //{
            //    worksheet.Column(pCol).Width = 8.5;
            //    return "Rap Rate";
            //}
            if (pStrHeader.ToLower() == "rapnetrapvalue")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Rap Value";
            }
            if (pStrHeader.ToLower() == "rapnetrapper")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Rap %";
            }
            if (pStrHeader.ToLower() == "rapnetpricepercarat")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Rap Pr/Ct";
            }
            if (pStrHeader.ToLower() == "rapnetamount")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Rap Amt";
            }


            if (pStrHeader.ToLower() == "invoicerapvalue")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Sale RapValue";
            }
            if (pStrHeader.ToLower() == "invoicerapper")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Sale Rap%";
            }
            if (pStrHeader.ToLower() == "invoicepricepercarat")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Sale Pr/Ct";
            }
            if (pStrHeader.ToLower() == "invoiceamount")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Sale Amt";
            }


            if (pStrHeader.ToLower() == "size")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "Size";
            }
            if (pStrHeader.ToLower() == "cut")
            {
                worksheet.Column(pCol).Width = 4.5;
                return "Cut";
            }
            if (pStrHeader.ToLower() == "pol")
            {
                worksheet.Column(pCol).Width = 4.5;
                return "Pol";
            }
            if (pStrHeader.ToLower() == "sym")
            {
                worksheet.Column(pCol).Width = 4.5;
                return "Sym";
            }
            if (pStrHeader.ToLower() == "fl")
            {
                worksheet.Column(pCol).Width = 4.5;
                return "FL";
            }
            if (pStrHeader.ToLower() == "flshade")
            {
                worksheet.Column(pCol).Width = 8.5;
                return "FlShd";
            }
            if (pStrHeader.ToLower() == "depthper")
            {
                worksheet.Column(pCol).Width = 5.5;
                return "TD%";
            }
            if (pStrHeader.ToLower() == "tableper")
            {
                worksheet.Column(pCol).Width = 5.5;
                return "Tab%";
            }
            if (pStrHeader.ToLower() == "length")
            {
                worksheet.Column(pCol).Width = 5;
                return "L";
            }
            if (pStrHeader.ToLower() == "width")
            {
                worksheet.Column(pCol).Width = 5;
                return "W";
            }
            if (pStrHeader.ToLower() == "height")
            {
                worksheet.Column(pCol).Width = 5;
                return "H";
            }
            if (pStrHeader.ToLower() == "measurement")
            {
                worksheet.Column(pCol).Width = 10;
                return "Measurement";
            }
            if (pStrHeader.ToLower() == "lab")
            {
                worksheet.Column(pCol).Width = 5;
                return "Lab";
            }
            if (pStrHeader.ToLower() == "location")
            {
                worksheet.Column(pCol).Width = 10;
                return "Loc";
            }
            if (pStrHeader.ToLower() == "certno")
            {
                worksheet.Column(pCol).Width = 12;
                return "Cert No";
            }
            if (pStrHeader.ToLower() == "videourl")
            {
                worksheet.Column(pCol).Width = 12;
                return "Video URL";
            }
            if (pStrHeader.ToLower() == "blacktable")
            {
                worksheet.Column(pCol).Width = 5;
                return "BT";
            }
            if (pStrHeader.ToLower() == "blackcrown")
            {
                worksheet.Column(pCol).Width = 5;
                return "BC";
            }
            if (pStrHeader.ToLower() == "whitetable")
            {
                worksheet.Column(pCol).Width = 5;
                return "WT";
            }
            if (pStrHeader.ToLower() == "whitecrown")
            {
                worksheet.Column(pCol).Width = 5;
                return "WC";
            }
            if (pStrHeader.ToLower() == "tableopen")
            {
                worksheet.Column(pCol).Width = 5;
                return "TO";
            }
            if (pStrHeader.ToLower() == "crownopen")
            {
                worksheet.Column(pCol).Width = 5;
                return "CO";
            }
            if (pStrHeader.ToLower() == "pavillionopen")
            {
                worksheet.Column(pCol).Width = 5;
                return "PO";
            }
            if (pStrHeader.ToLower() == "milky")
            {
                worksheet.Column(pCol).Width = 5;
                return "Milky";
            }
            if (pStrHeader.ToLower() == "luster")
            {
                worksheet.Column(pCol).Width = 5;
                return "Luster";
            }
            if (pStrHeader.ToLower() == "ec")
            {
                worksheet.Column(pCol).Width = 5;
                return "EC";
            }
            if (pStrHeader.ToLower() == "ha")
            {
                worksheet.Column(pCol).Width = 5;
                return "HA";
            }
            if (pStrHeader.ToLower() == "ratio")
            {
                worksheet.Column(pCol).Width = 5;
                return "Ratio";
            }
            if (pStrHeader.ToLower() == "girdper")
            {
                worksheet.Column(pCol).Width = 6;
                return "Girdle%";
            }
            if (pStrHeader.ToLower() == "pavang")
            {
                worksheet.Column(pCol).Width = 6;
                return "PVAng";
            }
            if (pStrHeader.ToLower() == "pavheight")
            {
                worksheet.Column(pCol).Width = 6;
                return "PVHgt";
            }
            if (pStrHeader.ToLower() == "crang")
            {
                worksheet.Column(pCol).Width = 6;
                return "CRAng";
            }
            if (pStrHeader.ToLower() == "crheight")
            {
                worksheet.Column(pCol).Width = 6;
                return "CRHgt";
            }
            if (pStrHeader.ToLower() == "girddesc")
            {
                worksheet.Column(pCol).Width = 30;
                return "Girdle Desc";
            }
            if (pStrHeader.ToLower() == "girdcond")
            {
                worksheet.Column(pCol).Width = 10;
                return "Girdle";
            }
            if (pStrHeader.ToLower() == "keytosymbol")
            {
                worksheet.Column(pCol).Width = 50;
                return "Key To Sym";
            }
            if (pStrHeader.ToLower() == "comment")
            {
                worksheet.Column(pCol).Width = 50;
                return "Comment";
            }
            if (pStrHeader.ToLower() == "fancycolordescription")
            {
                worksheet.Column(pCol).Width = 50;
                return "Fancy Color Description";
            }
            if (pStrHeader.ToLower() == "imageurl")
            {
                worksheet.Column(pCol).Width = 50;
                worksheet.Column(pCol).Hidden = true;
                return "Image URL";
            }
            if (pStrHeader.ToLower() == "certurl")
            {
                worksheet.Column(pCol).Width = 50;
                worksheet.Column(pCol).Hidden = true;
                return "Cert URL";
            }
            if (pStrHeader.ToLower() == "videourl")
            {
                worksheet.Column(pCol).Width = 50;
                worksheet.Column(pCol).Hidden = true;
                return "Video URL";
            }
            if (pStrHeader.ToLower() == "dnapageurl")
            {
                worksheet.Column(pCol).Width = 50;
                worksheet.Column(pCol).Hidden = true;
                return "DNA Page URL";
            }

            if (pStrHeader.ToLower() == "colgroup")
            {
                worksheet.Column(pCol).Width = 5;
                worksheet.Column(pCol).Hidden = true;
                return "ColGroup";
            }
            if (pStrHeader.ToLower() == "clagroup")
            {
                worksheet.Column(pCol).Width = 5;
                worksheet.Column(pCol).Hidden = true;
                return "ClaGroup";
            }
            if (pStrHeader.ToLower() == "cutgroup")
            {
                worksheet.Column(pCol).Width = 5;
                worksheet.Column(pCol).Hidden = true;
                return "CutGroup";
            }
            if (pStrHeader.ToLower() == "polgroup")
            {
                worksheet.Column(pCol).Width = 5;
                worksheet.Column(pCol).Hidden = true;
                return "PolGroup";
            }
            if (pStrHeader.ToLower() == "symgroup")
            {
                worksheet.Column(pCol).Width = 5;
                worksheet.Column(pCol).Hidden = true;
                return "SymGroup";
            }
            if (pStrHeader.ToLower() == "flgroup")
            {
                worksheet.Column(pCol).Width = 5;
                worksheet.Column(pCol).Hidden = true;
                return "FLGroup";
            }
            return "";
        }

        public void AddProportionDetail(ExcelWorksheet worksheet, DataTable pDtabGroup, string SheetName, int Row, int Column,
           string pStrHeader, string pStrTitle,
           string pStrGroupColumn,
           string StrStartRow,
           string StrEndRow,
           DataTable pDtabDetail
           )
        {
            Color BackColor = Color.FromArgb(40, 56, 145);
            Color FontColor = Color.White;
            string FontName = "Calibri";
            float FontSize = 9;

            int StartRow = Row;
            int StartColumn = Column;

            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Value = pStrHeader;
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Merge = true;
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Font.Name = FontName;
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Font.Size = 12;

            StartRow = StartRow + 1;
            worksheet.Cells[StartRow, Column, StartRow, Column].Value = pStrTitle;
            worksheet.Cells[StartRow, Column + 1, StartRow, Column + 1].Value = "Pcs";
            worksheet.Cells[StartRow, Column + 2, StartRow, Column + 2].Value = "Carat";
            worksheet.Cells[StartRow, Column + 3, StartRow, Column + 3].Value = "Rap %";
            worksheet.Cells[StartRow, Column + 4, StartRow, Column + 4].Value = "Amount";
            worksheet.Cells[StartRow, Column + 5, StartRow, Column + 5].Value = "Rap Value";
            worksheet.Cells[StartRow, Column + 6, StartRow, Column + 6].Value = "%";

            StartRow = StartRow + 1;

            int IntSizeStartRow = StartRow;
            int IntSizeEndRow = StartRow + pDtabGroup.Rows.Count - 1;
            int IntSizeStartColumn = Row;
            int IntSizeEndColumn = Column + 6;

            string GroupCol = Global.ColumnIndexToColumnLetter(pDtabDetail.Columns[pStrGroupColumn].Ordinal + 1);
            string CaratCol = Global.ColumnIndexToColumnLetter(pDtabDetail.Columns["Carat"].Ordinal + 1);
            string AmountCol = Global.ColumnIndexToColumnLetter(pDtabDetail.Columns["Amount"].Ordinal + 1);
            string RapAmountCol = Global.ColumnIndexToColumnLetter(pDtabDetail.Columns["RapValue"].Ordinal + 1);

            string FormulaCol = "'" + SheetName + "'!$" + GroupCol + "$" + StrStartRow + ":$" + GroupCol + "$" + StrEndRow + "";
            string FormulaCaratCol = "'" + SheetName + "'!$" + CaratCol + "$" + StrStartRow + ":$" + CaratCol + "$" + StrEndRow + "";
            string FormulaAmountCol = "'" + SheetName + "'!$" + AmountCol + "$" + StrStartRow + ":$" + AmountCol + "$" + StrEndRow + "";
            string FormulaRapAmountCol = "'" + SheetName + "'!$" + RapAmountCol + "$" + StrStartRow + ":$" + RapAmountCol + "$" + StrEndRow + "";

            string SumGrpCol = Global.ColumnIndexToColumnLetter(Column);
            string SumPcsCol = Global.ColumnIndexToColumnLetter(Column + 1);
            string SumCaratCol = Global.ColumnIndexToColumnLetter(Column + 2);
            string SumRapPerCol = Global.ColumnIndexToColumnLetter(Column + 3);
            string SumAmountCol = Global.ColumnIndexToColumnLetter(Column + 4);
            string SumRapAmountCol = Global.ColumnIndexToColumnLetter(Column + 5);
            string SumPerCol = Global.ColumnIndexToColumnLetter(Column + 6);

            foreach (DataRow DRow in pDtabGroup.Rows)
            {
                worksheet.Cells[StartRow, Column, StartRow, Column].Value = Val.ToString(DRow[0]);

                //PCS
                worksheet.Cells[StartRow, Column + 1, StartRow, Column + 1].Formula = "SUMPRODUCT(SUBTOTAL(3,OFFSET(" + FormulaCol + ",ROW(" + FormulaCol + ")-MIN(ROW(" + FormulaCol + ")),,1)),--(" + FormulaCol + "=" + SumGrpCol + "" + StartRow + "))";
                worksheet.Cells[StartRow, Column + 2, StartRow, Column + 2].Formula = "SUMPRODUCT(SUBTOTAL(3,OFFSET(" + FormulaCol + ",ROW(" + FormulaCol + ")-MIN(ROW(" + FormulaCol + ")),,1)),--(" + FormulaCol + "=" + SumGrpCol + "" + StartRow + "),(" + FormulaCaratCol + "))";
                //Rap %
                worksheet.Cells[StartRow, Column + 3, StartRow, Column + 3].Formula = "IF(" + SumRapAmountCol + "" + StartRow + ">0,ROUND(SUM(((" + SumAmountCol + "" + StartRow + ")/((" + SumRapAmountCol + "" + StartRow + "*1)))*100),2)-100,)";

                // Amount
                worksheet.Cells[StartRow, Column + 4, StartRow, Column + 4].Formula = "SUMPRODUCT(SUBTOTAL(3,OFFSET(" + FormulaCol + ",ROW(" + FormulaCol + ")-MIN(ROW(" + FormulaCol + ")),,1)),--(" + FormulaCol + "=" + SumGrpCol + "" + StartRow + "),(" + FormulaAmountCol + "))";

                // Rap
                worksheet.Cells[StartRow, Column + 5, StartRow, Column + 5].Formula = "SUMPRODUCT(SUBTOTAL(3,OFFSET(" + FormulaCol + ",ROW(" + FormulaCol + ")-MIN(ROW(" + FormulaCol + ")),,1)),--(" + FormulaCol + "=" + SumGrpCol + "" + StartRow + "),(" + FormulaRapAmountCol + "))";

                //Per
                worksheet.Cells[StartRow, Column + 6, StartRow, Column + 6].Formula = "" + SumPcsCol + "" + StartRow + "/$" + SumPcsCol + "$" + (Val.ToInt(IntSizeStartRow) + pDtabGroup.Rows.Count) + "";
                worksheet.Cells[StartRow, Column + 6, StartRow, Column + 6].Style.Numberformat.Format = "0.00%";

                StartRow = StartRow + 1;
            }

            // Rap Amount Column
            worksheet.Column(Column + 5).OutlineLevel = 1;
            worksheet.Column(Column + 5).Collapsed = true;

            worksheet.Cells[StartRow, Column, StartRow, Column].Value = "Total";
            worksheet.Cells[StartRow, Column + 1, StartRow, Column + 1].Formula = "SUM(" + SumPcsCol + "" + IntSizeStartRow + ":" + SumPcsCol + "" + IntSizeEndRow + ")";
            worksheet.Cells[StartRow, Column + 2, StartRow, Column + 2].Formula = "SUM(" + SumCaratCol + "" + IntSizeStartRow + ":" + SumCaratCol + "" + IntSizeEndRow + ")";
            worksheet.Cells[StartRow, Column + 3, StartRow, Column + 3].Formula = "=IF(" + SumRapAmountCol + "" + StartRow + ">0,ROUND(SUM(((" + SumAmountCol + "" + StartRow + ")/((" + SumRapAmountCol + "" + StartRow + "*1)))*100),2)-100,)";
            worksheet.Cells[StartRow, Column + 4, StartRow, Column + 4].Formula = "SUM(" + SumAmountCol + "" + IntSizeStartRow + ":" + SumAmountCol + "" + IntSizeEndRow + ")";
            worksheet.Cells[StartRow, Column + 5, StartRow, Column + 5].Formula = "SUM(" + SumRapAmountCol + "" + IntSizeStartRow + ":" + SumRapAmountCol + "" + IntSizeEndRow + ")";
            worksheet.Cells[StartRow, Column + 6, StartRow, Column + 6].Formula = "SUM(" + SumPerCol + "" + IntSizeStartRow + ":" + SumPerCol + "" + IntSizeEndRow + ")";
            worksheet.Cells[StartRow, Column + 6, StartRow, Column + 6].Style.Numberformat.Format = "0.00%";


            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Cells[Row + 2, Column + 1, StartRow, Column + 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.Font.Name = FontName;
            worksheet.Cells[Row, Column, StartRow, Column + 6].Style.Font.Size = FontSize;

            //Header
            worksheet.Cells[Row, Column, Row + 1, Column + 6].Style.Font.Bold = true;
            worksheet.Cells[Row, Column, Row + 1, Column + 6].Style.Font.Color.SetColor(FontColor);
            worksheet.Cells[Row, Column, Row + 1, Column + 6].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[Row, Column, Row + 1, Column + 6].Style.Fill.PatternColor.SetColor(BackColor);
            worksheet.Cells[Row, Column, Row + 1, Column + 6].Style.Fill.BackgroundColor.SetColor(BackColor);

            // Footer
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Font.Bold = true;
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Font.Color.SetColor(FontColor);
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Fill.PatternColor.SetColor(BackColor);
            worksheet.Cells[StartRow, Column, StartRow, Column + 6].Style.Fill.BackgroundColor.SetColor(BackColor);

            //Left First Column
            worksheet.Cells[Row, Column, StartRow, Column].Style.Font.Bold = true;
            worksheet.Cells[Row, Column, StartRow, Column].Style.Font.Color.SetColor(FontColor);
            worksheet.Cells[Row, Column, StartRow, Column].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[Row, Column, StartRow, Column].Style.Fill.PatternColor.SetColor(BackColor);
            worksheet.Cells[Row, Column, StartRow, Column].Style.Fill.BackgroundColor.SetColor(BackColor);

        }

        public void AddInclusionDetail(ExcelWorksheet worksheet, DataTable pDtab)
        {
            Color BackColor = Color.FromArgb(40, 56, 145);
            Color FontColor = Color.White;
            string FontName = "Calibri";
            float FontSize = 9;


            worksheet.Cells[2, 3, 4, 13].Value = "SKE Inclusion Grading";
            worksheet.Cells[2, 3, 4, 13].Style.Font.Name = FontName;
            worksheet.Cells[2, 3, 4, 13].Style.Font.Size = 20;
            worksheet.Cells[2, 3, 4, 13].Style.Font.Bold = true;

            worksheet.Cells[2, 3, 4, 13].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            worksheet.Cells[2, 3, 4, 13].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            worksheet.Cells[2, 3, 4, 13].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            worksheet.Cells[2, 3, 4, 13].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
            worksheet.Cells[2, 3, 4, 13].Merge = true;
            worksheet.Cells[2, 3, 4, 13].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Cells[2, 3, 4, 13].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;


            worksheet.Cells[2, 3, 4, 13].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            worksheet.Cells[2, 3, 4, 13].Style.Fill.PatternColor.SetColor(BackColor);
            worksheet.Cells[2, 3, 4, 13].Style.Fill.BackgroundColor.SetColor(BackColor);
            worksheet.Cells[2, 3, 4, 13].Style.Font.Color.SetColor(FontColor);

            DataTable DTabDistinct = pDtab.DefaultView.ToTable(true, "PARATYPE_ID", "PARATYPECODE", "PARATYPENAME");
            DTabDistinct.DefaultView.Sort = "PARATYPE_ID";
            DTabDistinct = DTabDistinct.DefaultView.ToTable();

            int StartRow = 0;
            int StartColumn = 3;
            int IntRow = 0;

            for (int i = 0; i < DTabDistinct.Rows.Count; i++)
            {
                string Str = Val.ToString(DTabDistinct.Rows[i]["PARATYPECODE"]);
                string StrName = Val.ToString(DTabDistinct.Rows[i]["PARATYPENAME"]);

                DataTable DTab = pDtab.Select("PARATYPECODE='" + Str + "'").CopyToDataTable();

                if (i % 4 == 0)
                {
                    StartColumn = 3;
                    StartRow = IntRow + (i % 4) + 6;
                    IntRow = StartRow;
                }
                else
                {
                    StartRow = IntRow;
                }

                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Value = StrName + " (" + Str + ")";
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Merge = true;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Fill.PatternColor.SetColor(BackColor);
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Fill.BackgroundColor.SetColor(BackColor);
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Color.SetColor(FontColor);
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Name = FontName;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Size = 11;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Bold = true;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                StartRow = StartRow + 1;
                for (int J = 0; J < DTab.Rows.Count; J++)
                {
                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Value = Val.ToString(DTab.Rows[J]["CODE"]);
                    worksheet.Cells[StartRow, StartColumn, StartRow, StartColumn].Style.Font.Bold = true;
                    worksheet.Cells[StartRow, StartColumn + 1, StartRow, StartColumn + 1].Value = Val.ToString(DTab.Rows[J]["NAME"]);
                    StartRow = StartRow + 1;

                }
                worksheet.Column(StartColumn + 1).Width = 20;

                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Name = FontName;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Size = 11;

                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Name = FontName;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Font.Size = FontSize;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[IntRow, StartColumn, StartRow, StartColumn + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                StartColumn = StartColumn + 3;

            }
            worksheet.Cells[1, 1, 50, 50].AutoFitColumns();
        }


        public string ExportExcel()
        {
            try
            {

                string FormatName = "GENERAL";

                DataRow Dr = GrdDetail.GetFocusedDataRow();

                DataTable DtabSize = new DataTable();

                StrCoustmoerName = Val.ToString(Dr["CUSTOMERNAME"]);

                mLStockProperty = new CRMCustomerAutoMailCriteriaProperty();

                mLStockProperty.AUTOEMAIL_ID = Guid.Parse(Val.ToString(Dr["AutoEmail_ID"]));
                mLStockProperty.CUSTOMER_ID = Guid.Parse(Val.ToString(Dr["COUSTOMER_ID"]));
                mLStockProperty.MULTYSHAPE_ID = Val.ToString(Dr["SHAPE_ID"]);
                mLStockProperty.MULTISHAPENAME = Val.ToString(Dr["SHAPENAME"]);
                mLStockProperty.MULTYCOLOR_ID = Val.ToString(Dr["COLOR_ID"]);
                mLStockProperty.MULTYCOLORNAME = Val.ToString(Dr["COLORNAME"]);
                mLStockProperty.MULTYCLARITY_ID = Val.ToString(Dr["CLARITY_ID"]);
                mLStockProperty.MULTYCLARITYNAME = Val.ToString(Dr["CLARITYNAME"]);
                mLStockProperty.MULTYCUT_ID = Val.ToString(Dr["CUT_ID"]);
                mLStockProperty.MULTYCUTNAME = Val.ToString(Dr["CUTNAME"]);
                mLStockProperty.MULTYPOL_ID = Val.ToString(Dr["POL_ID"]);
                mLStockProperty.MULTYPOLNAME = Val.ToString(Dr["POLNAME"]);
                mLStockProperty.MULTYSYM_ID = Val.ToString(Dr["SYM_ID"]);
                mLStockProperty.MULTYSYMNAME = Val.ToString(Dr["SYMNAME"]);
                mLStockProperty.MULTYFL_ID = Val.ToString(Dr["FL_ID"]);
                mLStockProperty.MULTYFLNAME = Val.ToString(Dr["FLNAME"]);

                mLStockProperty.MULTYFANCYCOLOR_ID = Val.ToString(Dr["FANCYCOLOR_ID"]);
                mLStockProperty.MULTYFANCYCOLORNAME = Val.ToString(Dr["FANCYCOLORNAME"]);
                mLStockProperty.MULTYLOCATION_ID = Val.ToString(Dr["LOCATION_ID"]);
                mLStockProperty.MULTYLOCATIONNAME = Val.ToString(Dr["LOCATIONNAME"]);
                mLStockProperty.MULTYMILKY_ID = Val.ToString(Dr["MILKY_ID"]);
                mLStockProperty.MULTYMILKYNAME = Val.ToString(Dr["MILKYNAME"]);
                mLStockProperty.MULTYLAB_ID = Val.ToString(Dr["LAB_ID"]);
                mLStockProperty.MULTYLABNAME = Val.ToString(Dr["LABNAME"]);
                mLStockProperty.MULTYBOX_ID = Val.ToString(Dr["BOX_ID"]);
                mLStockProperty.MULTYBOXNAME = Val.ToString(Dr["BOXNAME"]);
                mLStockProperty.WEBSTATUS_ID = Val.ToString(Dr["WEBSTATUS_ID"]);
                mLStockProperty.WEBSTATUSNAME = Val.ToString(Dr["WEBSTATUSNAME"]);

                mLStockProperty.FROMCARAT1 = Val.Val(Dr["FROMCARAT1"]);
                mLStockProperty.TOCARAT1 = Val.Val(Dr["TOCARAT1"]);

                mLStockProperty.FROMCARAT2 = Val.Val(Dr["FROMCARAT2"]);
                mLStockProperty.TOCARAT2 = Val.Val(Dr["TOCARAT2"]);

                mLStockProperty.FROMCARAT3 = Val.Val(Dr["FROMCARAT3"]);
                mLStockProperty.TOCARAT3 = Val.Val(Dr["TOCARAT3"]);

                mLStockProperty.FROMCARAT4 = Val.Val(Dr["FROMCARAT4"]);
                mLStockProperty.TOCARAT4 = Val.Val(Dr["TOCARAT4"]);

                mLStockProperty.FROMCARAT5 = Val.Val(Dr["FROMCARAT5"]);
                mLStockProperty.TOCARAT5 = Val.Val(Dr["TOCARAT5"]);

                mLStockProperty.FROMLENGTH = Val.Val(Dr["FROMLENGTHPER"]);
                mLStockProperty.TOLENGTH = Val.Val(Dr["TOLENGTHPER"]);

                mLStockProperty.FROMWIDTH = Val.Val(Dr["FROMWIDTHPER"]);
                mLStockProperty.TOWIDTH = Val.Val(Dr["TOWIDTHPER"]);

                mLStockProperty.FROMHEIGHT = Val.Val(Dr["FROMHEIGHTPER"]);
                mLStockProperty.TOHEIGHT = Val.Val(Dr["TOHEIGHTPER"]);

                mLStockProperty.FROMTABLEPER = Val.Val(Dr["FROMTABLEPER"]);
                mLStockProperty.TOTABLEPER = Val.Val(Dr["TOTABLEPER"]);

                mLStockProperty.FROMDEPTHPER = Val.Val(Dr["FROMDEPTHPER"]);
                mLStockProperty.TODEPTHPER = Val.Val(Dr["TODEPTHPER"]);

                mLStockProperty.STOCKNO = Val.ToString(Dr["STONENO"]);

                mLStockProperty.LABREPORTNO = Val.ToString(Dr["CERTINO"]);

                mLStockProperty.SERIALNO = Val.ToString(Dr["SERIALNO"]);

                mLStockProperty.MEMONO = Val.ToString(Dr["MEMONO"]);

                mLStockProperty.EMAILTYPE = Val.ToString(Dr["EMAILTYPE"]);
                mLStockProperty.ISACTIVE = Val.ToBoolean(Dr["ISACTIVE"]);

                mLStockProperty.EMAIL_ID = Val.ToString(Dr["EMAIL_ID"]);


                this.Cursor = Cursors.WaitCursor;
               DataSet DS = ObjStock.GetDataForExcelExportEmailSettingNew(mLStockProperty);
                this.Cursor = Cursors.Default;
             
                DTabDetail = DS.Tables[0];
                DataTable DTabSize = DS.Tables[1];
                DataTable DTabShape = DS.Tables[2];
                DataTable DTabClarity = DS.Tables[3];
                DataTable DTabColor = DS.Tables[4];
                DataTable DTabCut = DS.Tables[5];
                DataTable DTabPolish = DS.Tables[6];
                DataTable DTabSym = DS.Tables[7];
                DataTable DTabFL = DS.Tables[8];
                DataTable DTabInclusion = DS.Tables[9];


                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }

                DTabDetail.DefaultView.Sort = "SR";
                DTabDetail = DTabDetail.DefaultView.ToTable();

                DTabSize.DefaultView.Sort = "FromCarat";
                DTabSize = DTabSize.DefaultView.ToTable();

                DTabShape.DefaultView.Sort = "SequenceNo";
                DTabShape = DTabShape.DefaultView.ToTable();

                DTabColor.DefaultView.Sort = "SequenceNo";
                DTabColor = DTabColor.DefaultView.ToTable();

                DTabClarity.DefaultView.Sort = "SequenceNo";
                DTabClarity = DTabClarity.DefaultView.ToTable();

                DTabCut.DefaultView.Sort = "SequenceNo";
                DTabCut = DTabCut.DefaultView.ToTable();

                DTabPolish.DefaultView.Sort = "SequenceNo";
                DTabPolish = DTabPolish.DefaultView.ToTable();

                DTabSym.DefaultView.Sort = "SequenceNo";
                DTabSym = DTabSym.DefaultView.ToTable();

                DTabFL.DefaultView.Sort = "SequenceNo";
                DTabFL = DTabFL.DefaultView.ToTable();

                this.Cursor = Cursors.WaitCursor;

               
               string StrFilePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\" + BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME + "_" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".xlsx";

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }

                FileInfo workBook = new FileInfo(StrFilePath);
                //Color BackColor = Color.FromArgb(119, 50, 107);
                Color BackColor = Color.FromArgb(2, 68, 143);
                Color FontColor = Color.White;
                string FontName = "Calibri";
                float FontSize = 9;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("SKE_Stock_" + DateTime.Now.ToString("ddMMyyyy"));
                    ExcelWorksheet worksheetProportion = xlPackage.Workbook.Worksheets.Add("Proportion");
                    ExcelWorksheet worksheetInclusion = xlPackage.Workbook.Worksheets.Add("Inclusion Detail");


                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    #region Add Image

                    Image img = Image.FromFile(Application.StartupPath + "//logo.jpg");
                    OfficeOpenXml.Drawing.ExcelPicture pic = worksheet.Drawings.AddPicture("Logo", img);
                    pic.SetPosition(2, 23);
                    pic.SetSize(100, 55);

                    worksheet.Cells[1, 1, 3, 3].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                    worksheet.Cells[1, 1, 3, 3].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                    worksheet.Cells[1, 1, 3, 3].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                    worksheet.Cells[1, 1, 3, 3].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thick;
                    worksheet.Cells[1, 1, 3, 3].Merge = true;

                    #endregion

                    #region Stock Detail

                    StartRow = 5;
                    EndRow = StartRow + DTabDetail.Rows.Count;
                    StartColumn = 1;
                    EndColumn = DTabDetail.Columns.Count;

                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].LoadFromDataTable(DTabDetail, true);
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Name = FontName;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Size = FontSize;

                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Font.Bold = true;
                    //   worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].AutoFilter = true;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(FontColor);

                    //#P : 06-08-2020
                    if (FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale")
                    {
                        worksheet.Cells[5, 15, 5, 18].Style.Font.Color.SetColor(Color.Red);
                    }

                    if (FormatName == "With Rapnet")
                    {
                        worksheet.Cells[5, 19, 5, 22].Style.Font.Color.SetColor(FontColor);
                    }
                    else if (FormatName == "With Sale")
                    {
                        worksheet.Cells[5, 19, 5, 22].Style.Font.Color.SetColor(Color.FromArgb(174, 201, 121));
                    }
                    //End : #P : 06-08-2020

                    worksheet.View.FreezePanes(6, 1);

                    // Set Hyperlink
                    int IntCertColumn = DTabDetail.Columns["CertNo"].Ordinal;
                    int IntVideoUrlColumn = DTabDetail.Columns["VideoUrl"].Ordinal;
                    int IntStoneDEtailUrlColumn = DTabDetail.Columns["StoneDetailURL"].Ordinal;

                    int RapaportColumn = DTabDetail.Columns["RapRate"].Ordinal + 1;
                    int PricePerCaratColumn = DTabDetail.Columns["PricePerCarat"].Ordinal + 1;
                    int DiscountColumn = DTabDetail.Columns["RapPer"].Ordinal + 1;
                    int CaratColumn = DTabDetail.Columns["Carat"].Ordinal + 1;
                    int AmountColumn = DTabDetail.Columns["Amount"].Ordinal + 1;

                    for (int IntI = 6; IntI <= EndRow; IntI++)
                    {
                        string RapColumns = Global.ColumnIndexToColumnLetter(RapaportColumn) + IntI.ToString();
                        string Discount = Global.ColumnIndexToColumnLetter(DiscountColumn) + IntI.ToString();
                        string Carat = Global.ColumnIndexToColumnLetter(CaratColumn) + IntI.ToString();
                        string PricePerCarat = Global.ColumnIndexToColumnLetter(PricePerCaratColumn) + IntI.ToString();

                        if (Val.ToString(DTabDetail.Rows[IntI - 6]["FANCYCOLOR"]) == "") //Add if condition khushbu 23-09-21 for skip formula in fancy color
                        {
                            worksheet.Cells[IntI, PricePerCaratColumn].Formula = "=ROUND(" + RapColumns + " + ((" + RapColumns + " * " + Discount + ") / 100),2)";
                            worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                        }
                        else
                        {
                            worksheet.Cells[IntI, PricePerCaratColumn].Value = Val.ToString(DTabDetail.Rows[IntI - 6]["PRICEPERCARAT"]);
                            worksheet.Cells[IntI, AmountColumn].Value = Val.ToString(DTabDetail.Rows[IntI - 6]["AMOUNT"]);
                        }

                        if (!Val.ToString(DTabDetail.Rows[IntI - 6]["DNAPAGEURL"]).Trim().Equals(string.Empty))
                        {
                            //worksheet.Cells[IntI, 2, IntI, 2].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["DNAPAGEURL"]));
                            worksheet.Cells[IntI, 2, IntI, 2].Style.Font.Name = FontName;
                            worksheet.Cells[IntI, 2, IntI, 2].Style.Font.Bold = true;
                            worksheet.Cells[IntI, 2, IntI, 2].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                            //worksheet.Cells[IntI, 2, IntI, 2].Style.Font.UnderLine = true;
                        }

                        //#P :  03-09-2020
                        //worksheet.Cells[IntI, 28, IntI, 28].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["CERTURL"]));
                        //worksheet.Cells[IntI, 28, IntI, 28].Style.Font.Name = FontName;
                        //worksheet.Cells[IntI, 28, IntI, 28].Style.Font.Bold = true;
                        //worksheet.Cells[IntI, 28, IntI, 28].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                        //worksheet.Cells[IntI, 28, IntI, 28].Style.Font.UnderLine = true;


                        if (!Val.ToString(DTabDetail.Rows[IntI - 6]["CERTURL"]).Trim().Equals(string.Empty))
                        {
                            worksheet.Cells[IntI, IntCertColumn + 1].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["CERTURL"]));
                            worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.Name = FontName;
                            worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.Bold = true;
                            worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                            worksheet.Cells[IntI, IntCertColumn + 1].Style.Font.UnderLine = true;
                        }


                        if (!Val.ToString(DTabDetail.Rows[IntI - 6]["VIDEOURL"]).Trim().Equals(string.Empty))
                        {
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Value = "Video";
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["VIDEOURL"]));
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Style.Font.Name = FontName;
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Style.Font.Bold = true;
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                            worksheet.Cells[IntI, IntVideoUrlColumn + 1].Style.Font.UnderLine = true;
                        }
                        //End : #P :  03-09-2020

                        if (!Val.ToString(DTabDetail.Rows[IntI - 6]["STONEDETAILURL"]).Trim().Equals(string.Empty))
                        {
                            //worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Value = "Detail";
                            worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Hyperlink = new Uri(Val.ToString(DTabDetail.Rows[IntI - 6]["STONEDETAILURL"]));
                            worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Style.Font.Name = FontName;
                            worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Style.Font.Bold = true;
                            worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                            worksheet.Cells[IntI, IntStoneDEtailUrlColumn + 1].Style.Font.UnderLine = true;
                        }

                        //#P : 06-08-2020
                        if (FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale")
                        {
                            worksheet.Cells[IntI, 15, IntI, 18].Style.Font.Color.SetColor(Color.Red);
                        }
                        if (FormatName == "With Rapnet")
                        {
                            worksheet.Cells[IntI, 19, IntI, 22].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192));
                        }
                        else if (FormatName == "With Sale")
                        {
                            worksheet.Cells[IntI, 19, IntI, 22].Style.Font.Color.SetColor(Color.FromArgb(0, 150, 68));
                        }
                        //End : #P : 06-08-2020

                    }

                    // Header Set
                    for (int i = 1; i <= DTabDetail.Columns.Count; i++)
                    {
                        string StrHeader = Global.ExportExcelHeader(Val.ToString(worksheet.Cells[5, i].Value), worksheet, i);
                        worksheet.Cells[5, i].Value = StrHeader;

                    }

                    int IntRowStartsFrom = 3;
                    int IntRowEndTo = (DTabDetail.Rows.Count - 1 + IntRowStartsFrom);

                    //CHECK COLUMN EXISTS IN DATATABLE..
                    #region :: Check Column Exists In Datatable ::
                    int SrNo = 0, CaratNo = 0, AmountNo = 0, RapAmountNo = 0, SizeNo = 0, ShapeNo = 0, ColorNo = 0, ClarityNo = 0, CutNo = 0, PolNo = 0, SymNo = 0, FLNo = 0,
                        ExpAmountNo = 0, ExpRapAmountNo = 0, RapnetAmountNo = 0, RapnetRapAmountNo = 0, InvoiceAmountNo = 0, InvoiceRapAmountNo = 0;


                    DataColumnCollection columns = DTabDetail.Columns;

                    if (columns.Contains("SR"))
                        SrNo = DTabDetail.Columns["SR"].Ordinal + 1;
                    if (columns.Contains("Size"))
                        SizeNo = DTabDetail.Columns["Size"].Ordinal + 1;
                    if (columns.Contains("Carat"))
                        CaratNo = DTabDetail.Columns["Carat"].Ordinal + 1;
                    if (columns.Contains("RapValue"))
                        RapAmountNo = DTabDetail.Columns["RapValue"].Ordinal + 1;
                    if (columns.Contains("Amount"))
                        AmountNo = DTabDetail.Columns["Amount"].Ordinal + 1;

                    //#P : 06-08-2020
                    if ((FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale") && columns.Contains("ExpRapValue"))
                        ExpRapAmountNo = DTabDetail.Columns["ExpRapValue"].Ordinal + 1;
                    if ((FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale") && columns.Contains("ExpAmount"))
                        ExpAmountNo = DTabDetail.Columns["ExpAmount"].Ordinal + 1;
                    if (FormatName == "With Rapnet" && columns.Contains("RapnetRapValue"))
                        RapnetRapAmountNo = DTabDetail.Columns["RapnetRapValue"].Ordinal + 1;
                    if (FormatName == "With Rapnet" && columns.Contains("RapnetAmount"))
                        RapnetAmountNo = DTabDetail.Columns["RapnetAmount"].Ordinal + 1;

                    if (FormatName == "With Sale" && columns.Contains("InvoiceRapValue"))
                        InvoiceRapAmountNo = DTabDetail.Columns["InvoiceRapValue"].Ordinal + 1;
                    if (FormatName == "With Sale" && columns.Contains("InvoiceAmount"))
                        InvoiceAmountNo = DTabDetail.Columns["InvoiceAmount"].Ordinal + 1;

                    //End : #P : 06-08-2020

                    if (columns.Contains("Shape"))
                        ShapeNo = DTabDetail.Columns["Shape"].Ordinal + 1;
                    if (columns.Contains("Color"))
                        ColorNo = DTabDetail.Columns["Color"].Ordinal + 1;
                    if (columns.Contains("Clarity"))
                        ClarityNo = DTabDetail.Columns["Clarity"].Ordinal + 1;
                    if (columns.Contains("Cut"))
                        CutNo = DTabDetail.Columns["Cut"].Ordinal + 1;
                    if (columns.Contains("Pol"))
                        PolNo = DTabDetail.Columns["Pol"].Ordinal + 1;
                    if (columns.Contains("Sym"))
                        SymNo = DTabDetail.Columns["Sym"].Ordinal + 1;
                    if (columns.Contains("FL"))
                        FLNo = DTabDetail.Columns["FL"].Ordinal + 1;

                    #endregion

                    string StrStartRow = "6";
                    string StrEndRow = EndRow.ToString();

                    #region Top Formula

                    worksheet.Cells[1, 5, 1, 5].Value = "Pcs";
                    worksheet.Cells[1, 6, 1, 6].Value = "Carat";
                    worksheet.Cells[1, 11, 1, 11].Value = "Rap Value";
                    worksheet.Cells[1, 12, 1, 12].Value = "Rap %";
                    worksheet.Cells[1, 13, 1, 13].Value = "Pr/Ct";
                    worksheet.Cells[1, 14, 1, 14].Value = "Amount";

                    //#P : 06-08-2020
                    if (FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale")
                    {
                        worksheet.Cells[1, 15, 1, 15].Value = "Exp RapValue";
                        worksheet.Cells[1, 16, 1, 16].Value = "Exp Rap%";
                        worksheet.Cells[1, 17, 1, 17].Value = "Exp Pr/Ct";
                        worksheet.Cells[1, 18, 1, 18].Value = "Exp Amount";
                    }
                    if (FormatName == "With Rapnet")
                    {
                        worksheet.Cells[1, 19, 1, 19].Value = "Rapnet RapValue";
                        worksheet.Cells[1, 20, 1, 20].Value = "Rapnet Rap%";
                        worksheet.Cells[1, 21, 1, 21].Value = "Rapnet Pr/Ct";
                        worksheet.Cells[1, 22, 1, 22].Value = "Rapnet Amount";
                    }
                    if (FormatName == "With Sale")
                    {
                        worksheet.Cells[1, 19, 1, 19].Value = "Sale RapValue";
                        worksheet.Cells[1, 20, 1, 20].Value = "Sale Rap%";
                        worksheet.Cells[1, 21, 1, 21].Value = "Sale Pr/Ct";
                        worksheet.Cells[1, 22, 1, 22].Value = "Sale Amount";
                    }
                    //End : #P : 06-08-2020


                    worksheet.Cells[2, 4, 2, 4].Value = "Total";
                    worksheet.Cells[3, 4, 3, 4].Value = "Selected";

                    worksheet.Cells[1, 7, 3, 10].Merge = true;
                    worksheet.Cells[1, 7, 3, 10].Value = "Note : Use filter to select stones and Check your ObjGridSelection Avg Disc and Total amt.";
                    worksheet.Cells[1, 7, 3, 10].Style.WrapText = true;

                    // Total Pcs Formula
                    string S = Global.ColumnIndexToColumnLetter(SrNo) + StrStartRow;
                    string E = Global.ColumnIndexToColumnLetter(SrNo) + StrEndRow;
                    worksheet.Cells[2, 5, 2, 5].Formula = "ROUND(COUNTA(" + S + ":" + E + "),2)";
                    worksheet.Cells[3, 5, 3, 5].Formula = "ROUND(SUBTOTAL(3," + S + ":" + E + "),2)";

                    // Total Carat Formula
                    S = Global.ColumnIndexToColumnLetter(CaratNo) + StrStartRow;
                    E = Global.ColumnIndexToColumnLetter(CaratNo) + StrEndRow;
                    worksheet.Cells[2, 6, 2, 6].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                    worksheet.Cells[3, 6, 3, 6].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                    S = Global.ColumnIndexToColumnLetter(RapAmountNo) + StrStartRow;
                    E = Global.ColumnIndexToColumnLetter(RapAmountNo) + StrEndRow;
                    worksheet.Cells[2, 11, 2, 11].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                    worksheet.Cells[3, 11, 3, 11].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                    // Amount Formula
                    S = Global.ColumnIndexToColumnLetter(AmountNo) + StrStartRow;
                    E = Global.ColumnIndexToColumnLetter(AmountNo) + StrEndRow;
                    worksheet.Cells[2, 14, 2, 14].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                    worksheet.Cells[3, 14, 3, 14].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                    // Price Per Carat Formula
                    worksheet.Cells[2, 13, 2, 13].Formula = "ROUND(N2/F2,2)";
                    worksheet.Cells[3, 13, 3, 13].Formula = "ROUND(N3/F3,2)";

                    // Discount Formula
                    S = Global.ColumnIndexToColumnLetter(AmountNo) + StrStartRow;
                    E = Global.ColumnIndexToColumnLetter(AmountNo) + StrEndRow;
                    //worksheet.Cells[2, 11, 2, 11].Formula = "ROUND(SUM(((M2)/((J2*1)))*100),2)-100";
                    //worksheet.Cells[3, 11, 3, 11].Formula = "ROUND(SUM(((M3)/((J3*1)))*100),2)-100";

                    worksheet.Cells[2, 12, 2, 12].Formula = "ROUND(SUM(((N2)/((K2*1)))*100),2)-100";
                    worksheet.Cells[3, 12, 3, 12].Formula = "ROUND(SUM(((N3)/((K3*1)))*100),2)-100";


                    #region Exp Summary Detail
                    if (FormatName == "With Exp" || FormatName == "With Rapnet" || FormatName == "With Sale")
                    {
                        //Exp RapValue
                        S = Global.ColumnIndexToColumnLetter(ExpRapAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(ExpRapAmountNo) + StrEndRow;
                        worksheet.Cells[2, 15, 2, 15].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                        worksheet.Cells[3, 15, 3, 15].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                        // Exp Amount Formula
                        S = Global.ColumnIndexToColumnLetter(ExpAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(ExpAmountNo) + StrEndRow;
                        worksheet.Cells[2, 18, 2, 18].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                        worksheet.Cells[3, 18, 3, 18].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                        // Exp Price Per Carat Formula
                        worksheet.Cells[2, 17, 2, 17].Formula = "ROUND(R2/F2,2)";
                        worksheet.Cells[3, 17, 3, 17].Formula = "ROUND(R3/F3,2)";

                        // Exp Discount Formula (Rap%)
                        S = Global.ColumnIndexToColumnLetter(ExpAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(ExpAmountNo) + StrEndRow;
                        //worksheet.Cells[2, 11, 2, 11].Formula = "ROUND(SUM(((M2)/((J2*1)))*100),2)-100";
                        //worksheet.Cells[3, 11, 3, 11].Formula = "ROUND(SUM(((M3)/((J3*1)))*100),2)-100";

                        worksheet.Cells[2, 16, 2, 16].Formula = "ROUND(SUM(((R2)/((O2*1)))*100),2)-100";
                        worksheet.Cells[3, 16, 3, 16].Formula = "ROUND(SUM(((R3)/((O3*1)))*100),2)-100";
                    }
                    #endregion

                    #region Rapnet Summary Detail
                    if (FormatName == "With Rapnet")
                    {
                        //Exp RapValue
                        S = Global.ColumnIndexToColumnLetter(RapnetRapAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(RapnetRapAmountNo) + StrEndRow;
                        worksheet.Cells[2, 19, 2, 19].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                        worksheet.Cells[3, 19, 3, 19].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                        // Exp Amount Formula
                        S = Global.ColumnIndexToColumnLetter(RapnetAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(RapnetAmountNo) + StrEndRow;
                        worksheet.Cells[2, 22, 2, 22].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                        worksheet.Cells[3, 22, 3, 22].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                        // Exp Price Per Carat Formula
                        worksheet.Cells[2, 21, 2, 21].Formula = "ROUND(V2/F2,2)";
                        worksheet.Cells[3, 21, 3, 21].Formula = "ROUND(V3/F3,2)";

                        // Exp Discount Formula (Rap%)
                        S = Global.ColumnIndexToColumnLetter(RapnetAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(RapnetAmountNo) + StrEndRow;
                        //worksheet.Cells[2, 11, 2, 11].Formula = "ROUND(SUM(((M2)/((J2*1)))*100),2)-100";
                        //worksheet.Cells[3, 11, 3, 11].Formula = "ROUND(SUM(((M3)/((J3*1)))*100),2)-100";

                        worksheet.Cells[2, 20, 2, 20].Formula = "ROUND(SUM(((V2)/((S2*1)))*100),2)-100";
                        worksheet.Cells[3, 20, 3, 20].Formula = "ROUND(SUM(((V3)/((S3*1)))*100),2)-100";
                    }
                    #endregion

                    #region Invoice(Sale) Summary Detail
                    if (FormatName == "With Sale")
                    {
                        //Exp RapValue
                        S = Global.ColumnIndexToColumnLetter(InvoiceRapAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(InvoiceRapAmountNo) + StrEndRow;
                        worksheet.Cells[2, 19, 2, 19].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                        worksheet.Cells[3, 19, 3, 19].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                        // Exp Amount Formula
                        S = Global.ColumnIndexToColumnLetter(InvoiceAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(InvoiceAmountNo) + StrEndRow;
                        worksheet.Cells[2, 22, 2, 22].Formula = "ROUND(SUM(" + S + ":" + E + "),2)";
                        worksheet.Cells[3, 22, 3, 22].Formula = "ROUND(SUBTOTAL(9," + S + ":" + E + "),2)";

                        // Exp Price Per Carat Formula
                        worksheet.Cells[2, 21, 2, 21].Formula = "ROUND(V2/F2,2)";
                        worksheet.Cells[3, 21, 3, 21].Formula = "ROUND(V3/F3,2)";

                        // Exp Discount Formula (Rap%)
                        S = Global.ColumnIndexToColumnLetter(InvoiceAmountNo) + StrStartRow;
                        E = Global.ColumnIndexToColumnLetter(InvoiceAmountNo) + StrEndRow;
                        //worksheet.Cells[2, 11, 2, 11].Formula = "ROUND(SUM(((M2)/((J2*1)))*100),2)-100";
                        //worksheet.Cells[3, 11, 3, 11].Formula = "ROUND(SUM(((M3)/((J3*1)))*100),2)-100";

                        worksheet.Cells[2, 20, 2, 20].Formula = "ROUND(SUM(((V2)/((S2*1)))*100),2)-100";
                        worksheet.Cells[3, 20, 3, 20].Formula = "ROUND(SUM(((V3)/((S3*1)))*100),2)-100";
                    }
                    #endregion

                    if (FormatName == "With Exp") //#P : 06-08-2020
                    {
                        worksheet.Cells[1, 4, 4, 18].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[1, 4, 4, 18].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[1, 4, 4, 18].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 18].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 18].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 18].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 18].Style.Font.Name = "Calibri";
                        worksheet.Cells[1, 4, 4, 18].Style.Font.Size = 9;

                        worksheet.Cells[1, 4, 1, 18].Style.Font.Bold = true;
                        worksheet.Cells[1, 4, 1, 18].Style.Font.Color.SetColor(Color.White);
                        worksheet.Cells[1, 4, 1, 18].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 4, 1, 18].Style.Fill.PatternColor.SetColor(BackColor);
                        worksheet.Cells[1, 4, 1, 18].Style.Fill.BackgroundColor.SetColor(BackColor);

                        worksheet.Cells[1, 15, 3, 18].Style.Font.Color.SetColor(Color.Red);

                    }
                    else if (FormatName == "With Rapnet" || FormatName == "With Sale") //#P : 06-08-2020
                    {
                        worksheet.Cells[1, 4, 4, 22].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[1, 4, 4, 22].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[1, 4, 4, 22].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 22].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 22].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 22].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 22].Style.Font.Name = "Calibri";
                        worksheet.Cells[1, 4, 4, 22].Style.Font.Size = 9;

                        worksheet.Cells[1, 4, 1, 22].Style.Font.Bold = true;
                        worksheet.Cells[1, 4, 1, 22].Style.Font.Color.SetColor(Color.White);
                        worksheet.Cells[1, 4, 1, 22].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 4, 1, 22].Style.Fill.PatternColor.SetColor(BackColor);
                        worksheet.Cells[1, 4, 1, 22].Style.Fill.BackgroundColor.SetColor(BackColor);

                        worksheet.Cells[1, 15, 3, 18].Style.Font.Color.SetColor(Color.Red);

                        if (FormatName == "With Sale")
                        {
                            worksheet.Cells[1, 19, 1, 22].Style.Font.Color.SetColor(Color.FromArgb(174, 201, 121)); //Green
                            worksheet.Cells[2, 19, 3, 22].Style.Font.Color.SetColor(Color.FromArgb(0, 150, 68)); //Green
                        }
                        else
                        {
                            worksheet.Cells[1, 19, 1, 22].Style.Font.Color.SetColor(FontColor); //Blue
                            worksheet.Cells[2, 19, 3, 22].Style.Font.Color.SetColor(Color.FromArgb(0, 112, 192)); //Blue
                        }
                    }
                    else
                    {
                        worksheet.Cells[1, 4, 4, 14].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        worksheet.Cells[1, 4, 4, 14].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        worksheet.Cells[1, 4, 4, 14].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 14].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 14].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 14].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        worksheet.Cells[1, 4, 4, 14].Style.Font.Name = "Calibri";
                        worksheet.Cells[1, 4, 4, 14].Style.Font.Size = 9;

                        worksheet.Cells[1, 4, 1, 14].Style.Font.Bold = true;
                        worksheet.Cells[1, 4, 1, 14].Style.Font.Color.SetColor(Color.White);
                        worksheet.Cells[1, 4, 1, 14].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                        worksheet.Cells[1, 4, 1, 14].Style.Fill.PatternColor.SetColor(BackColor);
                        worksheet.Cells[1, 4, 1, 14].Style.Fill.BackgroundColor.SetColor(BackColor);
                    }

                    worksheet.Cells[1, 4, 3, 4].Style.Font.Bold = true;
                    worksheet.Cells[1, 4, 3, 4].Style.Font.Color.SetColor(Color.White);
                    worksheet.Cells[1, 4, 3, 4].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, 4, 3, 4].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[1, 4, 3, 4].Style.Fill.BackgroundColor.SetColor(BackColor);

                    worksheet.Cells[1, 7, 3, 10].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[1, 7, 3, 10].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[1, 7, 3, 10].Style.Fill.BackgroundColor.SetColor(BackColor);



                    if (FormatName == "With Exp") //#P : 06-08-2020
                    {
                        worksheet.Column(11).OutlineLevel = 1;//RapValue
                        worksheet.Column(11).Collapsed = true;

                        worksheet.Column(15).OutlineLevel = 1; //ExpRapValue
                        worksheet.Column(15).Collapsed = true;
                        worksheet.Column(24).OutlineLevel = 1; //FLShade
                        worksheet.Column(24).Collapsed = true;
                        //worksheet.Column(27).OutlineLevel = 1; //L (Length)
                        //worksheet.Column(27).Collapsed = true;
                        //worksheet.Column(28).OutlineLevel = 1; //W (Width)
                        //worksheet.Column(28).Collapsed = true;
                        //worksheet.Column(29).OutlineLevel = 1; //H (Height)
                        //worksheet.Column(29).Collapsed = true;
                    }
                    if (FormatName == "With Rapnet" || FormatName == "With Sale") //#P : 06-08-2020
                    {
                        worksheet.Column(11).OutlineLevel = 1;//RapValue
                        worksheet.Column(11).Collapsed = true;

                        worksheet.Column(15).OutlineLevel = 1; //ExpRapValue
                        worksheet.Column(15).Collapsed = true;

                        worksheet.Column(19).OutlineLevel = 1; //RapnetRapValue/SaleRapValue
                        worksheet.Column(19).Collapsed = true;

                        worksheet.Column(28).OutlineLevel = 1; //FLShade
                        worksheet.Column(28).Collapsed = true;
                        //worksheet.Column(31).OutlineLevel = 1; //L (Length)
                        //worksheet.Column(31).Collapsed = true;
                        //worksheet.Column(32).OutlineLevel = 1; //W (Width)
                        //worksheet.Column(32).Collapsed = true;
                        //worksheet.Column(33).OutlineLevel = 1; //H (Height)
                        //worksheet.Column(33).Collapsed = true;
                    }
                    else
                    {
                        worksheet.Column(11).OutlineLevel = 1;//RapValue
                        worksheet.Column(11).Collapsed = true;

                        worksheet.Column(20).OutlineLevel = 1;
                        worksheet.Column(20).Collapsed = true;

                        //worksheet.Column(23).OutlineLevel = 1;
                        //worksheet.Column(23).Collapsed = true;
                        //worksheet.Column(24).OutlineLevel = 1;
                        //worksheet.Column(24).Collapsed = true;
                        //worksheet.Column(25).OutlineLevel = 1;
                        //worksheet.Column(25).Collapsed = true;
                    }

                    #endregion

                    #endregion

                    #region Inclusion Detail

                    AddInclusionDetail(worksheetInclusion, DTabInclusion);

                    #endregion

                    #region Proporstion Detail

                    worksheetProportion.Cells[2, 2, 3, 17].Value = "Stock Proportion";
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Font.Name = FontName;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Font.Size = 20;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Font.Bold = true;

                    worksheetProportion.Cells[2, 2, 3, 17].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheetProportion.Cells[2, 2, 3, 17].Merge = true;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheetProportion.Cells[2, 2, 3, 17].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheetProportion.Cells[2, 2, 3, 17].Style.Font.Color.SetColor(FontColor);

                    int NewRow = 6;
                    AddProportionDetail(worksheetProportion, DTabSize, worksheet.Name, 6, 2, "SIZE WISE SUMMARY", "Size", "Size", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabShape, worksheet.Name, 6, 11, "SHAPE WISE SUMMARY", "Shape", "Shape", StrStartRow, StrEndRow, DTabDetail);

                    if (DTabSize.Rows.Count > DTabShape.Rows.Count)
                    {
                        NewRow = NewRow + DTabSize.Rows.Count + 5;
                    }
                    else
                    {
                        NewRow = NewRow + DTabShape.Rows.Count + 5;
                    }

                    AddProportionDetail(worksheetProportion, DTabClarity, worksheet.Name, NewRow, 2, "CLARITY WISE SUMMARY", "Clarity", "ClaGroup", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabColor, worksheet.Name, NewRow, 11, "COLOR WISE SUMMARY", "Color", "ColGroup", StrStartRow, StrEndRow, DTabDetail);


                    if (DTabClarity.Rows.Count > DTabColor.Rows.Count)
                    {
                        NewRow = NewRow + DTabClarity.Rows.Count + 5;
                    }
                    else
                    {
                        NewRow = NewRow + DTabColor.Rows.Count + 5;
                    }

                    AddProportionDetail(worksheetProportion, DTabCut, worksheet.Name, NewRow, 2, "CUT WISE SUMMARY", "Cut", "CutGroup", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabPolish, worksheet.Name, NewRow, 11, "POLISH WISE SUMMARY", "Pol", "PolGroup", StrStartRow, StrEndRow, DTabDetail);


                    if (DTabCut.Rows.Count > DTabPolish.Rows.Count)
                    {
                        NewRow = NewRow + DTabCut.Rows.Count + 5;
                    }
                    else
                    {
                        NewRow = NewRow + DTabPolish.Rows.Count + 5;
                    }

                    AddProportionDetail(worksheetProportion, DTabSym, worksheet.Name, NewRow, 2, "SYM WISE SUMMARY", "Sym", "SymGroup", StrStartRow, StrEndRow, DTabDetail);

                    AddProportionDetail(worksheetProportion, DTabFL, worksheet.Name, NewRow, 11, "FL WISE SUMMARY", "FL", "FLGroup", StrStartRow, StrEndRow, DTabDetail);

                    #endregion

                    xlPackage.Save();
                }

                this.Cursor = Cursors.Default;
                return StrFilePath;

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
            return "";
        }
        public string ExportExcelWithStockList(DataSet DS, string PStrFilePath) //Add Khushbu 12-07-21
        {
            try
            {

                DataTable DTabDetail = DS.Tables[0];

                if (DTabDetail.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;

                    Global.Message("NO DATA FOUND FOR EXPORT");
                    return "";
                }


                DTabDetail.DefaultView.Sort = "SrNo";
                DTabDetail = DTabDetail.DefaultView.ToTable();

                this.Cursor = Cursors.WaitCursor;


                string StrFilePath = PStrFilePath; 

                if (File.Exists(StrFilePath))
                {
                    File.Delete(StrFilePath);
                }
               

                FileInfo workBook = new FileInfo(StrFilePath);
                Color BackColor = Color.Yellow;
                Color FontColor = Color.Black;
                string FontName = "Calibri";
                float FontSize = 11;

                int StartRow = 0;
                int StartColumn = 0;
                int EndRow = 0;
                int EndColumn = 0;

                using (ExcelPackage xlPackage = new ExcelPackage(workBook))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Full Stock List");
                    //Excel.Application worksheet = new Excel.Application();
                    //excelApp.ErrorCheckingOptions.BackgroundChecking = false;

                    StartRow = 1;
                    StartColumn = 1;
                    EndRow = StartRow;
                    EndColumn = DTabDetail.Columns.Count;

                    #region Stock Detail

                    EndRow = StartRow + DTabDetail.Rows.Count;

                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].LoadFromDataTable(DTabDetail, true);
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;

                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Name = FontName;
                    worksheet.Cells[StartRow, StartColumn, EndRow, EndColumn].Style.Font.Size = FontSize;

                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Font.Bold = true;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;
                    worksheet.Cells[StartRow, StartColumn, StartRow, EndColumn].Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.PatternColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Fill.BackgroundColor.SetColor(BackColor);
                    worksheet.Cells[StartRow, 1, StartRow, EndColumn].Style.Font.Color.SetColor(FontColor);

                    //worksheet.Column(17).OutlineLevel = 1;
                    //worksheet.Column(17).Collapsed = true;

                    /*
                    int RxWColumn = DTabDetail.Columns["RW"].Ordinal + 1;
                    int RapaportColumn = DTabDetail.Columns["RapPrice"].Ordinal + 1;
                    int PricePerCaratColumn = DTabDetail.Columns["NetRate"].Ordinal + 1;
                    int DiscountColumn = DTabDetail.Columns["Disc"].Ordinal + 1;
                    int CaratColumn = DTabDetail.Columns["Carats"].Ordinal + 1;
                    int AmountColumn = DTabDetail.Columns["NetValue"].Ordinal + 1;
                    int VideoLinkColumn = DTabDetail.Columns["VIDEOLINK"].Ordinal + 1;
                    int VideoColumn = DTabDetail.Columns["Video"].Ordinal + 1;
                    int DepthPerColumn = DTabDetail.Columns["DepthPer"].Ordinal + 1;
                    int TablePerColumn = DTabDetail.Columns["TablePer"].Ordinal + 1;
                    int ShapeColumn = DTabDetail.Columns["Shape"].Ordinal + 1;

                    int GirdlePerColumn = DTabDetail.Columns["GIRDLEPER"].Ordinal + 1;
                    int CAColumn = DTabDetail.Columns["CRANGLE"].Ordinal + 1;
                    */
                    int RxWColumn = DTabDetail.Columns["RxW"].Ordinal + 1;
                    int RapaportColumn = DTabDetail.Columns["Rap. Price"].Ordinal + 1;
                    int PricePerCaratColumn = DTabDetail.Columns["Net Rate"].Ordinal + 1;
                    int DiscountColumn = DTabDetail.Columns["Disc%"].Ordinal + 1;
                    int CaratColumn = DTabDetail.Columns["Carats"].Ordinal + 1;
                    int AmountColumn = DTabDetail.Columns["Net Value"].Ordinal + 1;
                    int VideoLinkColumn = DTabDetail.Columns["Video Link"].Ordinal + 1;
                    int VideoColumn = DTabDetail.Columns["Video"].Ordinal + 1;
                    int DepthPerColumn = DTabDetail.Columns["Depth%"].Ordinal + 1;
                    int TablePerColumn = DTabDetail.Columns["Table%"].Ordinal + 1;
                    int ShapeColumn = DTabDetail.Columns["Shape"].Ordinal + 1;

                    int GirdlePerColumn = DTabDetail.Columns["Girldle Per"].Ordinal + 1;
                    int CAColumn = DTabDetail.Columns["Crown Angle"].Ordinal + 1;



                    for (int IntI = 2; IntI <= EndRow; IntI++)
                    {
                        string RapColumns = Global.ColumnIndexToColumnLetter(RapaportColumn) + IntI.ToString();
                        string Discount = Global.ColumnIndexToColumnLetter(DiscountColumn) + IntI.ToString();
                        string Carat = Global.ColumnIndexToColumnLetter(CaratColumn) + IntI.ToString();
                        string PricePerCarat = Global.ColumnIndexToColumnLetter(PricePerCaratColumn) + IntI.ToString();
                        string VideoLink = Global.ColumnIndexToColumnLetter(VideoLinkColumn) + IntI.ToString();

                        worksheet.Cells[IntI, RxWColumn].Formula = "=ROUND(" + RapColumns + " * " + Carat + ",2)";
                        //worksheet.Cells[IntI, PricePerCaratColumn].Formula = "=ROUND( (100 +" + Discount + ") * " + RapColumns + "/100,2)";
                        //worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";

                        if (Val.ToString(DTabDetail.Rows[IntI - 2]["FANCYCOLOR"]) == "") //Add if condition khushbu 08-10-21 for skip formula in fancy color
                        {
                            //worksheet.Cells[IntI, PricePerCaratColumn].Formula = "=ROUND(" + RapColumns + " + ((" + RapColumns + " * " + Discount + ") / 100),0)";
                            worksheet.Cells[IntI, PricePerCaratColumn].Formula = "=(" + RapColumns + " + ((" + RapColumns + " * " + Discount + ") / 100))";
                            worksheet.Cells[IntI, AmountColumn].Formula = "=ROUND(" + PricePerCarat + " * " + Carat + ",2)";
                            worksheet.Cells[IntI, AmountColumn].Style.Font.Bold = true;
                        }
                        else
                        {
                            //worksheet.Cells[IntI, PricePerCaratColumn].Value = Math.Round(Val.ToDouble(DTabDetail.Rows[IntI - 2]["NetRate"]), 0);
                            worksheet.Cells[IntI, PricePerCaratColumn].Value = Val.ToDouble(DTabDetail.Rows[IntI - 2]["Net Rate"]);
                            worksheet.Cells[IntI, AmountColumn].Value = Math.Round(Val.ToDouble(DTabDetail.Rows[IntI - 2]["Net Value"]), 2);
                            worksheet.Cells[IntI, AmountColumn].Style.Font.Bold = true;

                        }

                        if (IntI != 1)
                        {
                            if (worksheet.Cells[IntI, VideoColumn].Value.ToString() == "")
                            {
                                worksheet.Cells[IntI, VideoColumn, IntI, VideoColumn].Value = "N/A";
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Color.SetColor(Color.Red);
                            }
                            else
                            {
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Name = FontName;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Bold = true;
                                worksheet.Cells[IntI, VideoColumn].Style.Font.Color.SetColor(Color.Blue);
                                worksheet.Cells[IntI, VideoColumn].Formula = "=HYPERLINK(" + VideoLink + ", \"Image\")";
                            }

                        }
                    }

                    EndRow = EndRow + 2;
                    worksheet.Cells[EndRow, 1, EndRow, 1].Value = "Summary";

                    string RxW = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["RxW"].Ordinal + 1);
                    string CaratCol = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Carats"].Ordinal + 1);
                    string Discount1 = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Disc%"].Ordinal + 1);
                    string NetRate = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Net Rate"].Ordinal + 1);
                    string NetValue = Global.ColumnIndexToColumnLetter(DTabDetail.Columns["Net Value"].Ordinal + 1);

                    int IntTotRow = DTabDetail.Rows.Count + 1;

                    StartRow = StartRow + 1;

                    worksheet.Cells[EndRow, ShapeColumn, EndRow, ShapeColumn].Formula = "SUBTOTAL(2," + CaratCol + StartRow + ":" + CaratCol + IntTotRow + ")";
                    worksheet.Cells[EndRow, CaratColumn, EndRow, CaratColumn].Formula = "ROUND(SUBTOTAL(9," + CaratCol + StartRow + ":" + CaratCol + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, RxWColumn, EndRow, RxWColumn].Formula = "SUBTOTAL(9," + RxW + StartRow + ":" + RxW + IntTotRow + ")";
                    worksheet.Cells[EndRow, AmountColumn, EndRow, AmountColumn].Formula = "ROUND(SUBTOTAL(9," + NetValue + StartRow + ":" + NetValue + IntTotRow + "),2)";
                    worksheet.Cells[EndRow, PricePerCaratColumn, EndRow, PricePerCaratColumn].Formula = "ROUND(" + NetValue + EndRow + "/" + CaratCol + EndRow + ",0)";
                    worksheet.Cells[EndRow, DiscountColumn, EndRow, DiscountColumn].Formula = "ROUND((" + NetValue + EndRow + "/" + RxW + EndRow + "-1 ) * 100,2)";

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Font.Bold = true;

                    worksheet.Cells[StartRow, CaratColumn, EndRow, CaratColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, DiscountColumn, EndRow, DiscountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, AmountColumn, EndRow, AmountColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, DepthPerColumn, EndRow, DepthPerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, TablePerColumn, EndRow, TablePerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, GirdlePerColumn, EndRow, GirdlePerColumn].Style.Numberformat.Format = "0.00";
                    worksheet.Cells[StartRow, CAColumn, EndRow, CAColumn + 3].Style.Numberformat.Format = "0.00";

                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    worksheet.Cells[EndRow, StartColumn, EndRow, EndColumn].Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Medium;

                    //worksheet.Cells[1, 2, 1, 2].Value = "Stone No";
                    //worksheet.Cells[1, 4, 1, 4].Value = "Report No";
                    //worksheet.Cells[1, 12, 1, 12].Value = "Flour.";
                    //worksheet.Cells[1, 14, 1, 14].Value = "Depth%";
                    //worksheet.Cells[1, 15, 1, 15].Value = "Table%";
                    //worksheet.Cells[1, 16, 1, 16].Value = "Rap. Price";
                    //worksheet.Cells[1, 17, 1, 17].Value = "RxW";
                    //worksheet.Cells[1, 18, 1, 18].Value = "Disc%";
                    //worksheet.Cells[1, 19, 1, 19].Value = "Net Rate";
                    //worksheet.Cells[1, 20, 1, 20].Value = "Net Value";

                    //worksheet.Cells[1, 25, 1, 25].Value = "Video";
                    //worksheet.Cells[1, 26, 1, 26].Value = "Table Incl";
                    //worksheet.Cells[1, 27, 1, 27].Value = "Side Incl";
                    //worksheet.Cells[1, 28, 1, 28].Value = "Table Black";
                    //worksheet.Cells[1, 29, 1, 29].Value = "Side Black";
                    //worksheet.Cells[1, 30, 1, 30].Value = "Table Open";
                    //worksheet.Cells[1, 31, 1, 31].Value = "Side Open";
                    //worksheet.Cells[1, 32, 1, 32].Value = "Girldle Per";
                    //worksheet.Cells[1, 35, 1, 35].Value = "Crown Angle";
                    //worksheet.Cells[1, 36, 1, 36].Value = "Crown Height";
                    //worksheet.Cells[1, 37, 1, 37].Value = "Pav Angle";
                    //worksheet.Cells[1, 38, 1, 38].Value = "Pav Height";
                    //worksheet.Cells[1, 39, 1, 39].Value = "Key To Symbols";
                    //worksheet.Cells[1, 41, 1, 41].Value = "Video Link";
                    #endregion

                    worksheet.Cells[1, 1, 100, 100].AutoFitColumns();
                    //worksheet.Column(17).Hidden = true; //Cmnt : Coz due to AutoFitcolumns column can not hide

                    xlPackage.Save();
                }


                this.Cursor = Cursors.Default;
                return StrFilePath;

            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }
            return "";
        }


        private void RepBtnEmail_Click(object sender, EventArgs e)
        {
            
        }

        private void FrmDimandView_Load(object sender, EventArgs e)
        {



        }
    }
}
