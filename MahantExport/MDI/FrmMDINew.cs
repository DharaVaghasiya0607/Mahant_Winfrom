
using MahantExport.Account;
using MahantExport.CRM;
using MahantExport.Grading;
using MahantExport.Master;
using MahantExport.Masters;
using MahantExport.Parcel;
using MahantExport.Pricing;
using MahantExport.Report;
using MahantExport.Stock;
using MahantExport.UserActivities;
using MahantExport.Utility;
using MahantExport.View;
using BusLib.Configuration;
using BusLib.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Deployment.Application;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MahantExport.Pricing;
using System.Data.SqlClient;
using BusLib.Transaction;
using MahantExport.UserControl;
using MahantExport.Properties;
using DevExpress.XtraBars.Alerter;
using MahantExport.MFG;
using System.Reflection;

namespace MahantExport.MDI
{
    public partial class FrmMDINew : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();

        string mStrPassword = "";
        bool mBoolSecurityKey = false;

        public FrmMDINew()
        {
            InitializeComponent();
        }

        private void FrmMDI_Load(object sender, EventArgs e)
        {
            ObjPer.GetFormPermission(this.Tag.ToString());

            if (File.Exists(Application.StartupPath + "\\Background.png"))
            {
                picBackground.Image = Image.FromFile(Application.StartupPath + "\\Background.png");
                picBackground.SizeMode = PictureBoxSizeMode.CenterImage;
            }

            lblYearMaster.Tag = BusLib.Configuration.BOConfiguration.FINYEAR_ID;
            lblYearMaster.Text = BusLib.Configuration.BOConfiguration.FINYEARNAME;
            lblYearMaster.AccessibleDescription = BusLib.Configuration.BOConfiguration.FINYEARSHORTNAME;

            //Global.gStrSuvichar = new BOMST_FormPermission().GetMessage();
            if (Global.gStrSuvichar.Trim() == "")
            {
                //Global.gStrSuvichar = "!! WELCOME " + Global.gStrCompanyName + " !!";
                Global.gStrSuvichar = "!! WELCOME !!";
            }

            if (BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME != "AXONEADMIN")
            {
                CheckMDIMenuVisibility();
            }
            //else
            //{
            //    DataTable DTab = new BOMST_FormPermission().GetUserAuthenticationGetData(BusLib.Configuration.BOConfiguration.gEmployeeProperty.LEDGER_ID);
            //    foreach (ToolStripMenuItem mainmenu in menuStrip1.Items)
            //    {
            //        mainmenu.Text = GetMenuLanguage(Val.ToString(mainmenu.Tag), DTab);
            //        if (mainmenu.DropDownItems.Count > 0)
            //        {
            //            foreach (ToolStripDropDownItem submenu1 in mainmenu.DropDownItems)
            //            {
            //                if (submenu1.GetType() == typeof(ToolStripSeparator))
            //                {
            //                    continue;
            //                }
            //                submenu1.Text = GetMenuLanguage(Val.ToString(submenu1.Tag), DTab);
            //            }
            //        }
            //    }
            //}

            DateTime lastUpdateDate = File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location); //Added by Daksha on 27/02/2023
            this.Text = "Welcome " + BusLib.Configuration.BOConfiguration.gEmployeeProperty.COMPANYNAME + " [ USERNAME : " + BusLib.Configuration.BOConfiguration.gEmployeeProperty.LEDGERNAME + "  & IP : " + BusLib.Configuration.BOConfiguration.ComputerIP + " ] [V : " + Global.gStrExeVersion + " ] [Last Update Date : " + lastUpdateDate + "]";

        }

        private void CheckMDIMenuVisibility()
        {
            ToolStripDropDownItem sub1menu;

            DataTable DTab = new BOMST_FormPermission().GetUserAuthenticationGetData(BusLib.Configuration.BOConfiguration.gEmployeeProperty.LEDGER_ID);

            foreach (ToolStripMenuItem mainmenu in menuStrip1.Items)
            {
                if (mainmenu.DropDownItems.Count > 0)
                {
                    int mainmenuCount = 0;

                    foreach (object submenu1 in mainmenu.DropDownItems)
                    {
                        if (submenu1.GetType() == typeof(ToolStripSeparator))
                        {
                            continue;
                        }


                        sub1menu = (ToolStripDropDownItem)submenu1;

                        //For SubLevel 2 Menu
                        if (sub1menu.DropDownItems.Count > 0)
                        {
                            int sub1menuCount = 0;

                            foreach (ToolStripDropDownItem sub2menu in sub1menu.DropDownItems)
                            {
                                //For SubLevel 3 Menu
                                if (sub2menu.DropDownItems.Count > 0)
                                {
                                    int sub2menuCount = 0;
                                    foreach (ToolStripDropDownItem sub3menu in sub2menu.DropDownItems)
                                    {
                                        if (CheckViewPermission(Val.ToString(sub3menu.Tag), DTab))
                                        {

                                            mainmenu.Visible = true;
                                            sub1menu.Visible = true;
                                            sub2menu.Visible = true;
                                            sub3menu.Visible = true;
                                        }
                                        else
                                        {

                                            sub3menu.Visible = false;
                                            sub2menuCount = sub2menuCount + 1;
                                            if (sub2menuCount == sub2menu.DropDownItems.Count)
                                            {
                                                sub2menu.Visible = false;
                                                sub1menuCount = sub1menuCount + 1;
                                                if (sub1menuCount == sub1menu.DropDownItems.Count)
                                                {
                                                    sub1menu.Visible = false;
                                                    mainmenuCount = mainmenuCount + 1;
                                                    if (mainmenuCount == mainmenu.DropDownItems.Count)
                                                    {
                                                        mainmenu.Visible = false;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                else if (sub2menu.DropDownItems.Count == 0)
                                {

                                    if (CheckViewPermission(Val.ToString(sub2menu.Tag), DTab))
                                    {
                                        mainmenu.Visible = true;
                                        sub1menu.Visible = true;
                                        sub2menu.Visible = true;
                                    }
                                    else
                                    {
                                        sub2menu.Visible = false;
                                        sub1menuCount = sub1menuCount + 1;
                                        if (sub1menuCount == sub1menu.DropDownItems.Count)
                                        {
                                            sub1menu.Visible = false;
                                            mainmenuCount = mainmenuCount + 1;
                                            if (mainmenuCount == mainmenu.DropDownItems.Count)
                                            {
                                                mainmenu.Visible = false;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (sub1menu.DropDownItems.Count == 0)
                        {

                            if (CheckViewPermission(Val.ToString(sub1menu.Tag), DTab))
                            {
                                mainmenu.Visible = true;
                                sub1menu.Visible = true;
                            }
                            else
                            {
                                sub1menu.Visible = false;
                                mainmenuCount = mainmenuCount + 1;
                                if (mainmenuCount == mainmenu.DropDownItems.Count)
                                {
                                    mainmenu.Visible = false;
                                }
                            }
                        }
                        //End For SubLevel 2 Menu
                    }
                }
                else if (mainmenu.DropDownItems.Count == 0)
                {

                    if (CheckViewPermission(Val.ToString(mainmenu.Tag), DTab))
                    {
                        mainmenu.Visible = true;
                    }
                }
                //End For SubLevel 1 Menu
            }

            //foreach (ToolStripMenuItem mainmenu in menuStrip1.Items)
            //{
            //    mainmenu.Text = GetMenuLanguage(Val.ToString(mainmenu.Tag), DTab);
            //    if (mainmenu.DropDownItems.Count > 0)
            //    {
            //        foreach (ToolStripDropDownItem submenu1 in mainmenu.DropDownItems)
            //        {
            //            if (submenu1.GetType() == typeof(ToolStripSeparator))
            //            {
            //                continue;
            //            }
            //            submenu1.Text = GetMenuLanguage(Val.ToString(submenu1.Tag), DTab);
            //        }
            //    }
            //}

            //logOut.Visible = true;
            //-----End For Main Menu
        }

        private bool CheckViewPermission(string MenuName, DataTable DTab)
        {
            bool Flag = false;
            try
            {
                DataRow[] DRow = DTab.Select("FormName = '" + MenuName + "'");

                if (DRow.Length != 0)
                {
                    Flag = Val.ToBoolean(DRow[0]["ISVIEW"]);
                }
                else
                {
                    Flag = false;
                }


            }
            catch (Exception ex)
            {
            }
            return Flag;
        }

        private string GetMenuLanguage(string MenuName, DataTable DTab)
        {
            string Flag = "";
            try
            {
                DataRow[] DRow = DTab.Select("FormName = '" + MenuName + "'");

                if (DRow.Length != 0)
                {
                    Flag = Val.ToString(DRow[0]["FORMDESC"]) + " ( " + Val.ToString(DRow[0]["FORMDESCNEW"]) + " )";
                }
            }
            catch (Exception ex)
            {
            }
            return Flag;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Global.SelectLanguage(Global.LANGUAGE.ENGLISH);
                if (this.ActiveMdiChild == null && this.MdiChildren.Length == 0)
                {

                    if (Global.Confirm("Are you sure to close the application ?") == System.Windows.Forms.DialogResult.Yes)
                    {
                        Application.Exit();
                    }
                }
                else
                {
                    if (this.ActiveMdiChild == null && this.MdiChildren.Length != 0)
                    {
                        if (this.MdiChildren.Length != 0)
                        {
                            foreach (DevControlLib.cDevXtraForm Frm in this.MdiChildren)
                            {
                                if (Frm.Tag + "" != "ExplicitClose")
                                {
                                    Frm.Activate();
                                    Frm.Focus();
                                    Frm.Close();
                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        this.ActiveMdiChild.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.ToString());
            }
        }


        private void BtnBackup_Click(object sender, EventArgs e)
        {
            BOConfiguration.BackUp();
            Global.Message("BACKUP SUCCESSFULLY DONE...............");
        }


        private void FrmMDI_FormClosing(object sender, FormClosingEventArgs e)
        {
            //BOConfiguration.BackUp();
        }

        private void aboutUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAboutUs FrmAboutUs = new FrmAboutUs();
            FrmAboutUs.ShowForm();
        }

        private void backupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BOConfiguration.BackUp();
            Global.Message("BACKUP SUCCESSFULLY DONE...............");
        }



        private void yearTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Global.Message("UNDER DEVELOPMENT");
        }

        private void shapeMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmParameter FrmParameter = new FrmParameter();
            FrmParameter.MdiParent = this;
            FrmParameter.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmParameter.ShowForm();
        }
        private void userPermissionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmEmployeeRights FrmEmployeeRights = new FrmEmployeeRights();
            FrmEmployeeRights.MdiParent = this;
            FrmEmployeeRights.ShowForm();
        }

        private void listToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FrmReportList FrmReportList = new FrmReportList();
            //FrmReportList.MdiParent = this;
            //FrmReportList.ShowForm();
        }

        private void xtraTabbedMdiManager1_PageAdded(object sender, DevExpress.XtraTabbedMdi.MdiTabPageEventArgs e)
        {
            picBackground.Visible = false;
        }

        private void xtraTabbedMdiManager1_PageRemoved(object sender, DevExpress.XtraTabbedMdi.MdiTabPageEventArgs e)
        {
            if (xtraTabbedMdiManager1.Pages.Count == 0)
            {
                picBackground.Visible = true;
            }
        }

     
        private void timer30Second_Tick(object sender, EventArgs e)
        {
            try
            {

                if (Val.ToString(BOConfiguration.gEmployeeProperty.LEDGER_ID).ToUpper() == "F42447D1-0BD8-EB11-9CD1-283926297D36" ||
                    Val.ToString(BOConfiguration.gEmployeeProperty.LEDGER_ID).ToUpper() == "4DDCB083-0ED8-EB11-9CD1-283926297D36" ||
                    Val.ToString(BOConfiguration.gEmployeeProperty.LEDGER_ID).ToUpper() == "B14D6A0F-0DD8-EB11-9CD1-283926297D36" ||
                    Val.ToString(BOConfiguration.gEmployeeProperty.LEDGER_ID).ToUpper() == "BB7919E6-0ED8-EB11-9CD1-283926297D36" ||
                    Val.ToString(BOConfiguration.gEmployeeProperty.LEDGER_ID).ToUpper() == "FC62254E-5417-4C40-BD49-27D266697765")
                {
                    DataTable DTab = new BOMST_FormPermission().GetOrderConfNotificationData();
                    if (DTab.Rows.Count == 0)
                    {
                        return;
                    }
                    string Str = "", MemoID = "";
                    foreach (DataRow DRow in DTab.Rows)
                    {
                        Str = ""; MemoID = "";
                        MemoID = "'" + Val.ToString(DRow["Memo_ID"]) + "'";
                        Str += "\nInvoiceNo : " + Val.ToString(DRow["INVOICENO"]);
                        Str += "\nParty Name : " + Val.ToString(DRow["BILLINGPARTYNAME"]);
                        Str += "\nTotal Pcs : " + Val.ToString(DRow["TotalPcs"]) + "\nTotal Cts : " + Val.ToString(DRow["TotalCarat"]);
                        Str += "\nTotal Amt : " + Val.ToString(DRow["TotalAmount"]);
                        Str += "\nSource : " + Val.ToString(DRow["Source"]);

                        Alert(Str, Form_Alert.enmType.Info, MemoID);
                    }
                    DTab.Dispose();
                    DTab = null;
                }
            }
            catch (Exception EX)
            {
            }
        }

        public void Alert(string msg, Form_Alert.enmType type, string UniqueID)
        {
            Form_Alert frm = new Form_Alert();
            frm.showAlert(msg, type, UniqueID);
        }

        private void gridDynamicReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FrmReportList FrmReportList = new FrmReportList();
            //FrmReportList.MdiParent = this;
            //FrmReportList.ShowForm();
        }


        private void lblRefreshMenu_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            //foreach (DevControlLib.cDevXtraForm frm in this.MdiChildren)
            //{
            //    frm.Dispose();
            //    frm.Close();
            //}
            foreach (System.Windows.Forms.Form frm in this.MdiChildren)
            {
                frm.Dispose();
                frm.Close();
            }

            if (BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME != "AXONEADMIN")
            {
                CheckMDIMenuVisibility();
            }

            this.Cursor = Cursors.Default;
            Global.Message("MENU REFRESHED AS PER RIGHTS");
        }

        private void messageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmInputBox FrmInputBox = new MahantExport.FrmInputBox();
            FrmInputBox.MdiParent = Global.gMainRef;
            FrmInputBox.ShowForm();

        }

        private void TimSuvichar_Tick(object sender, EventArgs e)
        {
            lblSuVichar.Visible = true;
            if (lblSuVichar.Left <= (PBox.Left - lblSuVichar.Width))
            {
                lblSuVichar.Left = PBox.Width;
                lblSuVichar.Text = Global.gStrSuvichar;
            }
            else if (lblSuVichar.Text.Length != 0)
            {
                lblSuVichar.Text = Global.gStrSuvichar;
                lblSuVichar.Left = (lblSuVichar.Left - 2);
            }
        }
        private void iForm_LinkClicked()
        {

        }

        private void countryMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCountry FrmCountry = new FrmCountry();
            FrmCountry.MdiParent = this;
            FrmCountry.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmCountry.ShowForm();
        }

        private void termsMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTerms FrmTerms = new FrmTerms();
            FrmTerms.MdiParent = this;
            FrmTerms.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmTerms.ShowForm();

        }


        private void currencyMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCurrency FrmCurrency = new FrmCurrency();
            FrmCurrency.MdiParent = this;
            FrmCurrency.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmCurrency.ShowForm();
        }

        private void dailyRateMaserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDailyRate FrmDailyRate = new FrmDailyRate();
            FrmDailyRate.MdiParent = this;
            FrmDailyRate.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmDailyRate.ShowForm();
        }

        private void partyMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLedgerList FrmLedgerList = new FrmLedgerList();
            FrmLedgerList.MdiParent = this;
            FrmLedgerList.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmLedgerList.ShowForm();
            
        }

        private void importColumnSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmExcelSetting FrmExcelSetting = new FrmExcelSetting();
            FrmExcelSetting.MdiParent = this;
            FrmExcelSetting.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmExcelSetting.ShowForm();
        }

        private void stockUploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmStockUpload FrmStockUpload = new FrmStockUpload();
            FrmStockUpload.MdiParent = this;
            FrmStockUpload.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmStockUpload.ShowForm("SINGLE");
        }

        private void purchaseToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmMemoList FrmMemoList = new FrmMemoList();
            FrmMemoList.MdiParent = this;
            FrmMemoList.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmMemoList.ShowForm(FrmMemoList.FORMTYPE.PURCHASE, false, "SINGLE");

        }

        private void sizeMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSize FrmSize = new FrmSize();
            FrmSize.MdiParent = this;
            FrmSize.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmSize.ShowForm();
        }

        private void bannerMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBanner FrmBanner = new FrmBanner();
            FrmBanner.MdiParent = this;
            FrmBanner.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmBanner.ShowForm();
        }

        private void cartDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCartDetail FrmCartDetail = new FrmCartDetail();
            FrmCartDetail.MdiParent = this;
            FrmCartDetail.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmCartDetail.ShowForm();
        }

        private void loginHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLoginHistory FrmLoginHistory = new FrmLoginHistory();
            FrmLoginHistory.MdiParent = this;
            FrmLoginHistory.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmLoginHistory.ShowForm();
        }

        private void searchCriteriaHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSearchHistory FrmSearchHistory = new FrmSearchHistory();
            FrmSearchHistory.MdiParent = this;
            FrmSearchHistory.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmSearchHistory.ShowForm();
        }

        private void priceRevisedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPriceRevised FrmPriceRevised = new FrmPriceRevised();
            FrmPriceRevised.MdiParent = this;
            FrmPriceRevised.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmPriceRevised.ShowForm();
        }


        private void purchaseToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
            FrmMemoEntry.MdiParent = this;
            FrmMemoEntry.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.PURCHASEISSUE, null);
        }
      
        private void memoEntryListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMemoList FrmMemoList = new FrmMemoList();
            FrmMemoList.MdiParent = this;
            FrmMemoList.Tag = ((ToolStripMenuItem)sender).Tag;            
            FrmMemoList.ShowForm(FrmMemoList.FORMTYPE.MEMOISSUE, true, "SINGLE");
        }

        private void memoPivotReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMemoReport FrmMemoReport = new FrmMemoReport();
            FrmMemoReport.MdiParent = this;
            FrmMemoReport.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmMemoReport.ShowForm();
        }

        private void stockNoHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmStoneHistory FrmStoneHistory = new FrmStoneHistory();
            FrmStoneHistory.MdiParent = this;
            FrmStoneHistory.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmStoneHistory.ShowForm(Stock.FrmStoneHistory.FORMTYPE.DISPLAY);

        }

        private void nPNLReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMemoReportNPNL FrmMemoReportNPNL = new FrmMemoReportNPNL();
            FrmMemoReportNPNL.MdiParent = this;
            FrmMemoReportNPNL.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmMemoReportNPNL.ShowForm();
        }

        private void memoPendingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMemoReportOS FrmMemoReportOS = new FrmMemoReportOS();
            FrmMemoReportOS.MdiParent = this;
            FrmMemoReportOS.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmMemoReportOS.ShowForm();

        }

        private void quickSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmQuickSearch FrmQuickSearch = new FrmQuickSearch();
            FrmQuickSearch.MdiParent = this;
            FrmQuickSearch.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmQuickSearch.ShowForm();
        }


        private void picBackground_Click(object sender, EventArgs e)
        {


        }

        private void salesAnalysisChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSalesAnalysis FrmSalesAnalysis = new FrmSalesAnalysis();
            FrmSalesAnalysis.MdiParent = this;
            FrmSalesAnalysis.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmSalesAnalysis.ShowForm();

        }

        private void emailSendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmEmailSend FrmEmailSend = new FrmEmailSend();
            FrmEmailSend.MdiParent = this;
            FrmEmailSend.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmEmailSend.ShowForm();
        }

        private void formMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmFormMaster FrmFormMaster = new FrmFormMaster();
            FrmFormMaster.MdiParent = this;
            FrmFormMaster.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmFormMaster.ShowForm();
        }

        private void saleStokToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMemoList FrmMemoList = new FrmMemoList();
            FrmMemoList.MdiParent = this;
            FrmMemoList.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmMemoList.ShowForm(FrmMemoList.FORMTYPE.MEMOISSUE, true, "SINGLE");

        }

        private void saleOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {FrmMemoList FrmMemoList = new FrmMemoList();
            FrmMemoList.MdiParent = this;
            FrmMemoList.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmMemoList.ShowForm(FrmMemoList.FORMTYPE.ORDERCONFIRM, false, "SINGLE");            
        }

        private void saleInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMemoList FrmMemoList = new FrmMemoList();
            FrmMemoList.MdiParent = this;
            FrmMemoList.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmMemoList.ShowForm(FrmMemoList.FORMTYPE.SALEINVOICE, false, "SINGLE");           
        }

        private void purchaseReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMemoList FrmMemoList = new FrmMemoList();
            FrmMemoList.MdiParent = this;
            FrmMemoList.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmMemoList.ShowForm(FrmMemoList.FORMTYPE.PURCHASERETURN, false, "SINGLE");
        }

        private void saleReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            FrmMemoList FrmMemoList = new FrmMemoList();
            FrmMemoList.MdiParent = this;
            FrmMemoList.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmMemoList.ShowForm(FrmMemoList.FORMTYPE.SALESDELIVERYRETURN, false, "SINGLE");            
        }

        private void messageToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FrmInputBox FrmInputBox = new MahantExport.FrmInputBox();
            FrmInputBox.MdiParent = Global.gMainRef;
            FrmInputBox.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmInputBox.ShowForm();
        }

        private void saleOrderCancellationToolStripMenuItem_Click(object sender, EventArgs e)
        {
           FrmMemoList FrmMemoList = new FrmMemoList();
            FrmMemoList.MdiParent = this;
            FrmMemoList.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmMemoList.ShowForm(FrmMemoList.FORMTYPE.ORDERCONFIRMRETURN, false, "SINGLE");            
        }

        private void parcelLiveStockSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmParcelLiveStock FrmParcelLiveStock = new FrmParcelLiveStock();
            FrmParcelLiveStock.MdiParent = this;
            FrmParcelLiveStock.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmParcelLiveStock.ShowForm();
        }

        
        private void parcelStockUploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmStockUploadParcel FrmStockUpload = new FrmStockUploadParcel();
            FrmStockUpload.MdiParent = this;
            FrmStockUpload.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmStockUpload.ShowForm("PARCEL");
        }
        
        private void aboutUSToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmInputBoxOther FrmInputBoxOther = new MahantExport.FrmInputBoxOther();
            FrmInputBoxOther.MdiParent = Global.gMainRef;
            FrmInputBoxOther.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmInputBoxOther.ShowForm(FrmInputBoxOther.FORMTYPE.ABOUTUS);
        }

        private void ourTermsMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmInputBoxOther FrmInputBoxOther = new MahantExport.FrmInputBoxOther();
            FrmInputBoxOther.MdiParent = Global.gMainRef;
            FrmInputBoxOther.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmInputBoxOther.ShowForm(FrmInputBoxOther.FORMTYPE.OURTERMS);
        }


        private void stockOpeningClosingReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmOpeningClosingNew FrmOpeningClosingNew = new FrmOpeningClosingNew();
            FrmOpeningClosingNew.MdiParent = this;
            FrmOpeningClosingNew.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmOpeningClosingNew.ShowForm();
        }

        private void contactUSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmInputBoxOther FrmInputBoxOther = new MahantExport.FrmInputBoxOther();
            FrmInputBoxOther.MdiParent = Global.gMainRef;
            FrmInputBoxOther.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmInputBoxOther.ShowForm(FrmInputBoxOther.FORMTYPE.CONTACTUS);
        }

        private void emailSendTestingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmEmailSendTesting FrmEmailSendTesting = new FrmEmailSendTesting();
            FrmEmailSendTesting.MdiParent = this;
            FrmEmailSendTesting.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmEmailSendTesting.ShowForm();
        }

        private void privacyPolicyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmInputBoxOther FrmInputBoxOther = new MahantExport.FrmInputBoxOther();
            FrmInputBoxOther.MdiParent = Global.gMainRef;
            FrmInputBoxOther.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmInputBoxOther.ShowForm(FrmInputBoxOther.FORMTYPE.PRIVACYPOLICY);
        }

        private void screenCaptureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmScreenCapture FrmScreenCapture = new MahantExport.FrmScreenCapture();
            FrmScreenCapture.MdiParent = Global.gMainRef;
            FrmScreenCapture.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmScreenCapture.ShowForm();
        }

        

        
        private void purchaseAPIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPurchaseAPI FrmPurchaseAPI = new FrmPurchaseAPI();
            FrmPurchaseAPI.MdiParent = this;
            FrmPurchaseAPI.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmPurchaseAPI.ShowForm(Stock.FrmPurchaseAPI.FORMTYPE.PURCHASEISSUE, null, "SINGLE");
        }

        

        private void pricingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmParameterUpdate FrmParameterUpdate = new FrmParameterUpdate();
            FrmParameterUpdate.MdiParent = this;
            FrmParameterUpdate.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmParameterUpdate.ShowForm();

        }


        private void aPISettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAPI FrmAPI = new FrmAPI();
            FrmAPI.MdiParent = this;
            FrmAPI.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmAPI.ShowForm();
        }

        private void boxMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmBoxMaster FrmBoxMaster = new FrmBoxMaster();
            FrmBoxMaster.MdiParent = this;
            FrmBoxMaster.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmBoxMaster.ShowForm();
        }

       

        private void stockStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmStockStatusSearch FrmStockStatusSearch = new FrmStockStatusSearch();
            FrmStockStatusSearch.MdiParent = this;
            FrmStockStatusSearch.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmStockStatusSearch.ShowForm();
        }


        private void certificateDownlaodToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCertificateDownload FrmCertificateDownload = new FrmCertificateDownload();
            FrmCertificateDownload.MdiParent = this;
            FrmCertificateDownload.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmCertificateDownload.ShowForm();
        }

        private void stockBarcodePrintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmStockBarcodePrint FrmStockBarcodePrint = new FrmStockBarcodePrint();
            FrmStockBarcodePrint.MdiParent = this;
            FrmStockBarcodePrint.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmStockBarcodePrint.ShowForm();
        }

        private void iamgeCertiFlagUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //FrmImageCertiFlagUpdate FrmImageCertiFlagUpdate = new FrmImageCertiFlagUpdate();
            //FrmImageCertiFlagUpdate.MdiParent = this;
            //FrmImageCertiFlagUpdate.Tag = ((ToolStripMenuItem)sender).Tag;
            //FrmImageCertiFlagUpdate.ShowForm();

            FrmImageVideoCertiUrlUpdate FrmImageVideoCertiUrlUpdate = new FrmImageVideoCertiUrlUpdate();
            FrmImageVideoCertiUrlUpdate.MdiParent = this;
            FrmImageVideoCertiUrlUpdate.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmImageVideoCertiUrlUpdate.Show();
        }

        private void roughPurchaseViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmRoughPurchaseView FrmRoughPurchaseView = new FrmRoughPurchaseView();
            FrmRoughPurchaseView.MdiParent = this;
            FrmRoughPurchaseView.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmRoughPurchaseView.ShowForm();
        }

        private void seirlaNoSizeMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSerialNoSizeMaster FrmSerialNoSizeMaster = new FrmSerialNoSizeMaster();
            FrmSerialNoSizeMaster.MdiParent = this;
            FrmSerialNoSizeMaster.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmSerialNoSizeMaster.ShowForm();
        }

        private void cutomerAutoEmailSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCustomerAutoEmailSetting FrmCustomerAutoEmailSetting = new FrmCustomerAutoEmailSetting();
            FrmCustomerAutoEmailSetting.MdiParent = this;
            FrmCustomerAutoEmailSetting.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmCustomerAutoEmailSetting.ShowForm();
        }

        private void matchingStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMatchingStock FrmMatchingStock = new FrmMatchingStock();
            FrmMatchingStock.MdiParent = this;
            FrmMatchingStock.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmMatchingStock.ShowForm(FrmMatchingStock.FORMTYPE.MEMOISSUE, true, "SINGLE");
        }

        private void stoneOfferPriceReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmOfferPriceReport FrmOfferPriceReport = new FrmOfferPriceReport();
            FrmOfferPriceReport.MdiParent = this;
            FrmOfferPriceReport.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmOfferPriceReport.ShowForm();
        }

        private void kapanWiseMumbaiRateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmKapanWiseMumbaiRate FrmKapanWiseMumbaiRate = new FrmKapanWiseMumbaiRate();
            FrmKapanWiseMumbaiRate.MdiParent = this;
            FrmKapanWiseMumbaiRate.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmKapanWiseMumbaiRate.ShowForm();
        }

        

        private void mixSizeMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMixSize FrmMixSize = new FrmMixSize();
            FrmMixSize.MdiParent = this;
            FrmMixSize.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmMixSize.ShowForm();
        }

        private void stockTallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmStockTally FrmStockTally = new FrmStockTally();
            FrmStockTally.MdiParent = this;
            FrmStockTally.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmStockTally.ShowForm();
        }

        private void connectRFIDMachineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmConnectRFIDMachine FrmConnectRFIDMachine = new FrmConnectRFIDMachine();
            FrmConnectRFIDMachine.ShowDialog();
        }


        private void mixClarityPriceMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMixPriceChart FrmParcelMixClarityPriceUpload = new FrmMixPriceChart();
            FrmParcelMixClarityPriceUpload.MdiParent = this;
            FrmParcelMixClarityPriceUpload.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmParcelMixClarityPriceUpload.ShowForm();
        }

        private void packetGradingEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPrdType FrmPrdType = new FrmPrdType();
            FrmPrdType.MdiParent = this;
            FrmPrdType.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmPrdType.ShowForm();
        }

        private void processMasterToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FrmProcess FrmProcess = new FrmProcess();
            FrmProcess.MdiParent = this;
            FrmProcess.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmProcess.ShowForm();
        }

        private void compnayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCompany FrmCompany = new FrmCompany();
            FrmCompany.MdiParent = this;
            FrmCompany.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmCompany.ShowForm("COMPANY");
        }

private void ledgerMappingMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLedgerMappingMaster FrmLedgerMappingMaster = new FrmLedgerMappingMaster();
            FrmLedgerMappingMaster.MdiParent = this;
            FrmLedgerMappingMaster.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmLedgerMappingMaster.ShowForm();
        }

        private void roughPurchaseEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmRoughPurchaseEntryNew FrmRoughPurchaseEntryNew = new FrmRoughPurchaseEntryNew();
            FrmRoughPurchaseEntryNew.MdiParent = this;
            FrmRoughPurchaseEntryNew.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmRoughPurchaseEntryNew.ShowForm();
        }

        private void yearMasterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmYearMaster FrmYearMaster = new FrmYearMaster();
            FrmYearMaster.MdiParent = this;
            FrmYearMaster.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmYearMaster.ShowForm();
        }

        private void lblYearMaster_Click(object sender, EventArgs e)
        {
            try
            {
                FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                FrmSearch.mStrSearchField = "YEARNAME,YEARSHORTNAME";

                this.Cursor = Cursors.WaitCursor;

                FrmSearch.mDTab = new BOMST_Ledger().FillFinYear(BOConfiguration.DEPTNAME);

                FrmSearch.mStrColumnsToHide = "DEPARTMENT_ID";
                this.Cursor = Cursors.Default;
                FrmSearch.ShowDialog();
                if (FrmSearch.DRow != null)
                {
                    this.Cursor = Cursors.WaitCursor;
                    foreach (DevControlLib.cDevXtraForm frm in this.MdiChildren)
                    {
                        frm.Dispose();
                        frm.Close();
                    }

                    if (BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME != "AXONEADMIN")
                    {
                        CheckMDIMenuVisibility();
                    }

                    this.Cursor = Cursors.Default;

                    lblYearMaster.Text = Val.ToString(FrmSearch.DRow["YEARNAME"]);
                    lblYearMaster.Tag = Val.ToString(FrmSearch.DRow["YEAR_ID"]);

                    BusLib.Configuration.BOConfiguration.FINYEAR_ID = Val.ToInt(FrmSearch.DRow["YEAR_ID"]);
                    BusLib.Configuration.BOConfiguration.FINYEARNAME = Val.ToString(FrmSearch.DRow["YEARNAME"]);
                    BusLib.Configuration.BOConfiguration.FINYEARSHORTNAME = Val.ToString(FrmSearch.DRow["YEARSHORTNAME"]);
                }
                FrmSearch.Hide();
                FrmSearch.Dispose();
                FrmSearch = null;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }


        }

        private void labFileUploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSingleFileUpload FrmSingleFileUpload = new FrmSingleFileUpload();
            FrmSingleFileUpload.MdiParent = this;
            FrmSingleFileUpload.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmSingleFileUpload.ShowForm();
        }

        private void predictionComparisionViewForAdminMNGTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmComparisionViewAdmin FrmComparisionViewAdmin = new FrmComparisionViewAdmin();
            FrmComparisionViewAdmin.MdiParent = this;
            FrmComparisionViewAdmin.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmComparisionViewAdmin.ShowForm(Masters.FrmComparisionViewAdmin.FORMTYPE.ADMIN);
        }

        private void stoneDayMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmStoneDayMaster FrmStoneDayMaster = new FrmStoneDayMaster();
            FrmStoneDayMaster.MdiParent = this;
            FrmStoneDayMaster.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmStoneDayMaster.ShowForm();
        }

        private void mixSizeMasterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmMixSize FrmMixSize = new FrmMixSize();
            FrmMixSize.MdiParent = this;
            FrmMixSize.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmMixSize.ShowForm();
        }


        private void mixClarityPriceMasterToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void mixClarityMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmParameter FrmParameter = new FrmParameter();
            FrmParameter.MdiParent = this;
            FrmParameter.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmParameter.ShowForm("MIX_CLARITY");
        }

        private void rapnetPriceUpdateToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void priceParameterUpdateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmParameterUpdate FrmParameterUpdate = new FrmParameterUpdate();
            FrmParameterUpdate.MdiParent = this;
            FrmParameterUpdate.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmParameterUpdate.ShowForm();
        }

        private void bulkPropertyUpdateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmBulkPropertyUpdate FrmBulkPropertyUpdate = new FrmBulkPropertyUpdate();
            FrmBulkPropertyUpdate.MdiParent = this;
            FrmBulkPropertyUpdate.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmBulkPropertyUpdate.ShowForm();
        }

        private void singleStoneUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmStoneHistory FrmStoneHistory = new FrmStoneHistory();
            FrmStoneHistory.MdiParent = this;
            FrmStoneHistory.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmStoneHistory.ShowForm(Stock.FrmStoneHistory.FORMTYPE.UPDATE);
        }


        private void kapanInwardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmKapanInward FrmKapanInward = new FrmKapanInward();
            FrmKapanInward.MdiParent = this;
            FrmKapanInward.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmKapanInward.ShowForm();
        }

        private void sizeWiseAssortmentToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmSizeAssortment FrmSizeAssortment = new FrmSizeAssortment();
            FrmSizeAssortment.MdiParent = this;
            FrmSizeAssortment.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmSizeAssortment.ShowForm();
        }

        private void clarityWiseAssortmentToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmClarityAssortment FrmClarityAssortment = new FrmClarityAssortment();
            FrmClarityAssortment.MdiParent = this;
            FrmClarityAssortment.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmClarityAssortment.ShowForm();
        }

        private void parcelBoxMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmParcelBoxMaster FrmParcelBoxMaster = new FrmParcelBoxMaster();
            FrmParcelBoxMaster.MdiParent = this;
            FrmParcelBoxMaster.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmParcelBoxMaster.ShowForm();
        }

        

        private void targetCreateMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmTargetCreateMaster FrmTargetCreateMaster = new FrmTargetCreateMaster();
            FrmTargetCreateMaster.MdiParent = this;
            FrmTargetCreateMaster.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmTargetCreateMaster.ShowForm();
        }

        private void parameterDiscountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmParameterDiscount FrmParameterDiscount = new FrmParameterDiscount();
            FrmParameterDiscount.MdiParent = this;
            FrmParameterDiscount.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmParameterDiscount.ShowForm();
        }

        private void parameterDiscountMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmParameterDiscountMaster FrmParameterDiscountMaster = new FrmParameterDiscountMaster();
            FrmParameterDiscountMaster.MdiParent = this;
            FrmParameterDiscountMaster.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmParameterDiscountMaster.ShowForm();
        }

        private void rapSizeMappingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMixRapSizeMapping FrmRapSizeMapping = new FrmMixRapSizeMapping();
            FrmRapSizeMapping.MdiParent = this;
            FrmRapSizeMapping.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmRapSizeMapping.ShowForm();
        }

        private void parameterDiscountToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmParameterDiscount FrmParameterDiscount = new FrmParameterDiscount();
            FrmParameterDiscount.MdiParent = this;
            FrmParameterDiscount.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmParameterDiscount.ShowForm();
        }

        private void rapnetPriceUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPriceRevised FrmPriceRevised = new FrmPriceRevised();
            FrmPriceRevised.MdiParent = this;
            FrmPriceRevised.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmPriceRevised.ShowForm();
        }

        private void webActivityListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmWebActivityList FrmWebActivityList = new FrmWebActivityList();
            FrmWebActivityList.MdiParent = this;
            FrmWebActivityList.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmWebActivityList.ShowForm();
        }


        private void trasferToMarketingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Parcel.FrmTransferToMarketing FrmTransferToMarketing = new Parcel.FrmTransferToMarketing();
            FrmTransferToMarketing.MdiParent = this;
            FrmTransferToMarketing.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmTransferToMarketing.ShowForm();
        }

        private void kapanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmKapan FrmKapan = new FrmKapan();
            FrmKapan.MdiParent = this;
            FrmKapan.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmKapan.ShowForm();
        }

        private void parcelStockTallyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmParcelStockTally FrmParcelStockTally = new FrmParcelStockTally();
            FrmParcelStockTally.MdiParent = this;
            FrmParcelStockTally.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmParcelStockTally.ShowForm();
        }

        private void dashBoardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDashBoard FrmDashBoard = new FrmDashBoard();
            FrmDashBoard.MdiParent = this;
            FrmDashBoard.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmDashBoard.ShowForm();
        }

        

        private void kapanWiseAssortmentViewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmKapanAssortmentView FrmKapanAssortmentView = new FrmKapanAssortmentView();
            FrmKapanAssortmentView.MdiParent = this;
            FrmKapanAssortmentView.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmKapanAssortmentView.ShowForm();
        }

        private void priceDateMasterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmPriceDateMaster FrmPriceDateMaster = new FrmPriceDateMaster();
            FrmPriceDateMaster.MdiParent = this;
            FrmPriceDateMaster.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmPriceDateMaster.ShowForm();

        }

        private void mixPriceChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMixPriceChart FrmParcelMixClarityPriceUpload = new FrmMixPriceChart();
            FrmParcelMixClarityPriceUpload.MdiParent = this;
            FrmParcelMixClarityPriceUpload.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmParcelMixClarityPriceUpload.ShowForm();
        }

        private void registerPrintToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FrmRegisterPrint FrmRegisterPrint = new FrmRegisterPrint();
            FrmRegisterPrint.MdiParent = this;
            FrmRegisterPrint.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmRegisterPrint.ShowForm();

        }

        private void parcelOpeningClosingReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmOpeningClosingForParcel FrmOpeningClosingForParcel = new FrmOpeningClosingForParcel();
            FrmOpeningClosingForParcel.MdiParent = this;
            FrmOpeningClosingForParcel.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmOpeningClosingForParcel.ShowForm();

        }

        private void partyMergeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPartyMerge FrmPartyMerge = new FrmPartyMerge();
            FrmPartyMerge.MdiParent = this;
            FrmPartyMerge.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmPartyMerge.ShowForm();
        }
                
        

        private void customerDemandViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDimandView FrmDimandView = new FrmDimandView();
            FrmDimandView.MdiParent = this;
            FrmDimandView.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmDimandView.ShowForm();
        }

        private void stockAnalysisChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmStockAnalysis FrmStockAnalysis = new FrmStockAnalysis();
            FrmStockAnalysis.MdiParent = this;
            FrmStockAnalysis.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmStockAnalysis.ShowForm();
        }

        private void pricingToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            FrmStoneSystemPricing FrmStoneSystemPricing = new FrmStoneSystemPricing();
            FrmStoneSystemPricing.MdiParent = this;
            FrmStoneSystemPricing.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmStoneSystemPricing.ShowForm();
        }

        private void labChargesUploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLabChargesUpload FrmLabChargesUpload = new FrmLabChargesUpload();
            FrmLabChargesUpload.MdiParent = this;
            FrmLabChargesUpload.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmLabChargesUpload.ShowForm();
        }

        private void mFGGRadingEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMFGGradingEntry FrmMFGGradingEntry = new FrmMFGGradingEntry();
            FrmMFGGradingEntry.MdiParent = this;
            FrmMFGGradingEntry.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmMFGGradingEntry.ShowForm();
        }

        private void gradingViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMFGGradingView FrmMFGGradingView = new FrmMFGGradingView();
            FrmMFGGradingView.MdiParent = this;
            FrmMFGGradingView.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmMFGGradingView.ShowForm();
        }

        private void rapnetPriceUpdateCOSTToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmRapnetPriceCostUpdate FrmRapnetPriceCostUpdate = new FrmRapnetPriceCostUpdate();
            FrmRapnetPriceCostUpdate.MdiParent = this;
            FrmRapnetPriceCostUpdate.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmRapnetPriceCostUpdate.ShowForm();
        }

        private void quickLinksToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void FrmMDINew_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F2)
                {
                    FrmSmartSearch FrmSmartSearch = new FrmSmartSearch();
                    FrmSmartSearch.MdiParent = this;
                    FrmSmartSearch.ShowForm();
                }
                else if (e.KeyCode == Keys.F6)
                {
                    FrmMemoList FrmMemoList = new FrmMemoList();
                    FrmMemoList.MdiParent = this;
                    FrmMemoList.Tag = "SaleOrder";
                    FrmMemoList.ShowForm(FrmMemoList.FORMTYPE.ORDERCONFIRM, false, "SINGLE");
                }               
            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
                this.Cursor = Cursors.Default;
            }
        }

        private void FrmMDINew_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (mBoolSecurityKey == true)
            {
                mStrPassword += Convert.ToChar(e.KeyChar);
            }
        }

        private void singleStonePriceUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSingleStonePriceUpdate FrmSingleStonePriceUpdate = new FrmSingleStonePriceUpdate();
            FrmSingleStonePriceUpdate.MdiParent = this;
            FrmSingleStonePriceUpdate.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmSingleStonePriceUpdate.ShowForm();
        }

        

        private void claritiyAssormentShapeSizeWiseReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmClarityAssortmenShapeSizeWise FrmClarityAssortmenShapeSizeWise = new FrmClarityAssortmenShapeSizeWise();
            FrmClarityAssortmenShapeSizeWise.MdiParent = this;
            FrmClarityAssortmenShapeSizeWise.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmClarityAssortmenShapeSizeWise.ShowForm();
        }

        private void roughPurchaseEntryNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmRoughPurchaseEntryNew FrmRoughPurchaseEntryNew = new FrmRoughPurchaseEntryNew();
            FrmRoughPurchaseEntryNew.MdiParent = this;
            FrmRoughPurchaseEntryNew.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmRoughPurchaseEntryNew.ShowForm();
        }

        
        private void mediaUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmStockMediaUpdate FrmStockMediaUpdate = new FrmStockMediaUpdate();
            FrmStockMediaUpdate.MdiParent = this;
            FrmStockMediaUpdate.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmStockMediaUpdate.ShowForm();
        }

        private void fancyColorMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmFancyColor FrmFancyColor = new FrmFancyColor();
            FrmFancyColor.MdiParent = this;
            FrmFancyColor.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmFancyColor.ShowForm();

        }

        
        private void jangedEntryModuleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmJangedEntry FrmJangedEntry = new FrmJangedEntry();
            FrmJangedEntry.MdiParent = this;
            FrmJangedEntry.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmJangedEntry.ShowForm();
        }

        

        private void mixCLVPriceChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCLVPriceChartUpload FrmCLVPriceChartUpload = new FrmCLVPriceChartUpload();
            FrmCLVPriceChartUpload.MdiParent = this;
            FrmCLVPriceChartUpload.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmCLVPriceChartUpload.ShowForm();
        }

        private void mixCLVPrieChartSummaryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmCLVPriceChartSummary FrmCLVPriceChartSummary = new FrmCLVPriceChartSummary();
            FrmCLVPriceChartSummary.MdiParent = this;
            FrmCLVPriceChartSummary.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmCLVPriceChartSummary.ShowForm();
        }

        private void ClvMixRapSizeMapingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmClvMixRapSizeMapping FrmClvMixRapSizeMapping = new FrmClvMixRapSizeMapping();
            FrmClvMixRapSizeMapping.MdiParent = this;
            FrmClvMixRapSizeMapping.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmClvMixRapSizeMapping.ShowForm();
        }

        private void kapanAvgDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmKapanAvgDetail FrmKapanAvgDetail = new FrmKapanAvgDetail();
            FrmKapanAvgDetail.MdiParent = this;
            FrmKapanAvgDetail.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmKapanAvgDetail.ShowForm();
        }

        private void rFIDScanningToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmRFIDScanning FrmRFIDScanning = new FrmRFIDScanning();
            FrmRFIDScanning.MdiParent = this;
            FrmRFIDScanning.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmRFIDScanning.ShowForm();
        }

        private void invTermsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmParameter FrmParameter = new FrmParameter();
            FrmParameter.MdiParent = this;
            FrmParameter.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmParameter.ShowForm("INSURANCETYPE");
        }

   
        private void stockIssueReturnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmStockIssueReturn FrmStockIssueReturn = new FrmStockIssueReturn();
            FrmStockIssueReturn.MdiParent = this;
            FrmStockIssueReturn.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmStockIssueReturn.ShowForm();
        }

        

        private void mFGComparisionReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMFGComparisionReport FrmMFGComparisionReport = new FrmMFGComparisionReport();
            FrmMFGComparisionReport.MdiParent = this;
            FrmMFGComparisionReport.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmMFGComparisionReport.ShowForm();
        }

        private void roundReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmRoundReport FrmRoundReport = new FrmRoundReport();
            FrmRoundReport.MdiParent = this;
            FrmRoundReport.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmRoundReport.ShowForm();
        }

        private void saleInvoiceViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSaleInvoiceView FrmSaleInvoiceView = new FrmSaleInvoiceView();
            FrmSaleInvoiceView.MdiParent = this;
            FrmSaleInvoiceView.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmSaleInvoiceView.ShowForm();
        }

        private void saleInvoiceEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
            FrmMemoEntry.MdiParent = this;
            FrmMemoEntry.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmMemoEntry.ShowForm("", "SALEINVOICEENTRY", 0);
        }

        

        
        private void CashInvoicesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmMemoList FrmMemoList = new FrmMemoList();
            FrmMemoList.MdiParent = this;
            FrmMemoList.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmMemoList.ShowForm(FrmMemoList.FORMTYPE.MEMOISSUE, true, "SINGLE", "CASHINVOICES");
        }

        private void saleReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSaleReport FrmSaleReport = new FrmSaleReport();
            FrmSaleReport.MdiParent = this;
            FrmSaleReport.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmSaleReport.Show();
        }

        private void StockParcelOpeningClosingReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmOpeningClosingParcel FrmOpeningClosingParcel = new FrmOpeningClosingParcel();
            FrmOpeningClosingParcel.MdiParent = this;
            FrmOpeningClosingParcel.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmOpeningClosingParcel.ShowForm();
        }

        private void StockMarketingOpeningClosingReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmOpeningClosingMarketing FrmOpeningClosingMarketing = new FrmOpeningClosingMarketing();
            FrmOpeningClosingMarketing.MdiParent = this;
            FrmOpeningClosingMarketing.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmOpeningClosingMarketing.ShowForm();
        }

        private void MST_deliveryChallan_Click(object sender, EventArgs e)
        {
            FrmDeliveryChallan FrmDeliveryChallan = new FrmDeliveryChallan();
            FrmDeliveryChallan.MdiParent = this;
            FrmDeliveryChallan.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmDeliveryChallan.Show();
        }

        private void mFGSingleStonePriceUpdateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmSingleStonePriceUpdate FrmSingleStonePriceUpdate = new FrmSingleStonePriceUpdate();
            FrmSingleStonePriceUpdate.MdiParent = this;
            FrmSingleStonePriceUpdate.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmSingleStonePriceUpdate.ShowForm_MFG();
        }

        private void aPIMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmAPISetting FrmAPISetting = new FrmAPISetting();
            FrmAPISetting.MdiParent = this;
            FrmAPISetting.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmAPISetting.ShowForm();
        }

      
        private void deptWiseMixClaPermissionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDeptWiseMixClarityPermission FrmDeptWiseMixClarityPermission = new FrmDeptWiseMixClarityPermission();
            FrmDeptWiseMixClarityPermission.MdiParent = this;
            FrmDeptWiseMixClarityPermission.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmDeptWiseMixClarityPermission.ShowForm();
        }

        private void MS_PolishRoughInwardEntry_Click(object sender, EventArgs e)
        {
            FrmPolishRoughInward FrmPolishRoughInward = new FrmPolishRoughInward();
            FrmPolishRoughInward.MdiParent = this;
            FrmPolishRoughInward.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmPolishRoughInward.Show();
        }

       

        //Added by Daksha on 16/01/2023
        private void timerStockiestNotification_Tick(object sender, EventArgs e)
        {
            try
            {
                if(Val.ToString(BOConfiguration.gEmployeeProperty.LEDGER_ID).ToUpper() == "B0703EEC-A579-EC11-A8B7-ACB57D1F87CF")
                { 
                    DataTable DTab = new BOMST_FormPermission().GetOrderConfNotificationData_ForStockiest();
                    if (DTab.Rows.Count == 0)
                    {
                        return;
                    }
                    string Str = "", MemoID = "";
                    foreach (DataRow DRow in DTab.Rows)
                    {
                        Str = ""; 
                        MemoID = Val.ToString(DRow["Memo_ID"]);
                        Str += "\nInvoiceNo : " + Val.ToString(DRow["InvoiceNo"]);
                        Str += "\nParty Name : " + Val.ToString(DRow["BillingPartyName"]);
                        Str += "\nTotal Pcs : " + Val.ToString(DRow["TotalPcs"]) + "\nTotal Cts : " + Val.ToString(DRow["TotalCarat"]);
                        Str += "\nTotal Amt : " + Val.ToString(DRow["TotalAmount"]);
                        Str += "\nSource : " + Val.ToString(DRow["Source"]);

                        Alert(Str, Form_Alert.enmType.Info, MemoID);
                    }
                    DTab.Dispose();
                    DTab = null;
                }
            }
            catch (Exception EX)
            {
            }
        }
        //End as Daksha


        private void MS_RapNetStockSync_Click(object sender, EventArgs e)
        {
            FrmRapNetStockSync objFrmRapNetStockSync = new FrmRapNetStockSync();
            objFrmRapNetStockSync.MdiParent = this;
            objFrmRapNetStockSync.Tag = ((ToolStripMenuItem)sender).Tag;
            objFrmRapNetStockSync.ShowForm();
        }

        
        private void shapeMasterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmShapeMaster FrmShapeMaster = new FrmShapeMaster();//Gunjan:07/04/2023
            FrmShapeMaster.MdiParent = this;
            FrmShapeMaster.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmShapeMaster.ShowForm();
        }

        private void clarityMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmClarityMaster FrmClarityMaster = new FrmClarityMaster();//Gunjan:07/04/2023
            FrmClarityMaster.MdiParent = this;
            FrmClarityMaster.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmClarityMaster.ShowForm();
        }

        private void sizeMasterToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            FrmSizeMaster FrmSizeMaster = new FrmSizeMaster();//Gunjan:07/04/2023
            FrmSizeMaster.MdiParent = this;
            FrmSizeMaster.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmSizeMaster.ShowForm();
        }

        private void lotMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmLotMaster FrmLotMaster = new FrmLotMaster();//Gunjan:07/04/2023
            FrmLotMaster.MdiParent = this;
            FrmLotMaster.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmLotMaster.ShowForm();
        }

        private void colorMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmColorMaster FrmColorMaster = new FrmColorMaster();//Gunjan:07/04/2023
            FrmColorMaster.MdiParent = this;
            FrmColorMaster.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmColorMaster.ShowForm();
        }

        

        private void parcelStoneMasterToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openingEntryFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmPolishOpeningStock FrmPolishOpeningStock = new FrmPolishOpeningStock();//Gunjan:14/06/2023
            FrmPolishOpeningStock.MdiParent = this;
            FrmPolishOpeningStock.Tag = ((ToolStripMenuItem)sender).Tag;
            FrmPolishOpeningStock.ShowForm();
        }

    }
}
