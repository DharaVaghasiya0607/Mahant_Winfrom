using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using DevExpress.XtraBars;
using BusLib;
using BusLib.Configuration;
using MahantExport;
using MahantExport.Account;
using MahantExport.CRM;
using MahantExport.Grading;
using MahantExport.Master;
using MahantExport.Masters;
using MahantExport.Parcel;
using MahantExport.Pricing;
using MahantExport.Report;
using MahantExport.Stock;
using MahantExport.MDI;
using MahantExport.UserActivities;
using MahantExport.Utility;
using MahantExport.View;
using MahantExport.MFG;
using BusLib.Master;
using System.IO;
using System.Reflection;
using DevExpress.XtraBars.Ribbon;
using System.Drawing;

namespace MahantExport.MDI
{
    public partial class FrmFinalMDIRibbon : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        string mStrPassword = "";
        bool mBoolSecurityKey = false;

        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOFormPer ObjPer = new BOFormPer();
        public FrmFinalMDIRibbon()
        {
            InitializeComponent();
        }


        private void MNExist_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (Global.Confirm("Are You Sure ?  You Want To Exit From Application ?") == System.Windows.Forms.DialogResult.Yes)
            {
                Application.Exit();
            }
        }
        private void FrmFinalMDIRibbon_Load(object sender, EventArgs e)
        {
            picBackground.Image = Properties.Resources.BackgroundNew;
            picBackground.SizeMode = PictureBoxSizeMode.Zoom;

            ObjPer.GetFormPermission(this.Tag.ToString());

            if (Global.gStrSuvichar.Trim() == "")
            {
                Global.gStrSuvichar = "!! WELCOME !!";
            }

            if (BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME != "AXONEADMIN")
            {
                CheckMDIMenuVisibility();
            }

            DateTime lastUpdateDate = File.GetLastWriteTime(Assembly.GetExecutingAssembly().Location); //Added by Daksha on 27/02/2023
            this.Text = "Welcome " + BusLib.Configuration.BOConfiguration.gEmployeeProperty.COMPANYNAME + " [ USERNAME : " + BusLib.Configuration.BOConfiguration.gEmployeeProperty.LEDGERNAME + "  & IP : " + BusLib.Configuration.BOConfiguration.ComputerIP + " ] [V : " + Global.gStrExeVersion + " ] [Last Update Date : " + lastUpdateDate + "]";

            if(BOConfiguration.ConnectionString.Contains("_Local"))
            {
                PBox.BackColor = Color.DeepSkyBlue;
            }

        }

        public void CheckMDIMenuVisibility()
        {
            DataTable DTab = new BOMST_FormPermission().GetUserAuthenticationGetData(BusLib.Configuration.BOConfiguration.gEmployeeProperty.LEDGER_ID);

            BarItem mCurrentItem = null/* TODO Change to default(_) if this is not a reference type */;
            BarSubItem mBarSubItem = null/* TODO Change to default(_) if this is not a reference type */;
            BarItemLink mSubLink = null/* TODO Change to default(_) if this is not a reference type */;
            BarSubItemLink mSubItem = null;
            DataView myDataView = new DataView(DTab);

            foreach (RibbonPage currentPage in this.ribbon.Pages)
            {
                bool hidemenu = false;

                if (Val.ToString(currentPage.Tag) == "LookNFeel")
                {
                    continue;
                }
                
                
                if (Val.ToString(currentPage.Tag) == "Jewellery")
                {
                    if (BOConfiguration.COMPANYNAME.Contains("Rijiya"))
                    {
                        currentPage.Visible = false;
                        continue;
                    }                        
                }


                foreach (RibbonPageGroup currentGroup in currentPage.Groups)
                {
                    foreach (BarItemLink currenLink in currentGroup.ItemLinks)
                    {
                        mCurrentItem = currenLink.Item;
                        myDataView.RowFilter = ("FORMNAME Like " + "'") + mCurrentItem.Tag + "'";
                        if (myDataView.ToTable().Rows.Count > 0)
                        {
                            if (Val.ToBoolean(myDataView.ToTable().Rows[0]["ISVIEW"]) == false)
                                mCurrentItem.Visibility = BarItemVisibility.Never;
                            else
                            {
                                mCurrentItem.Visibility = BarItemVisibility.Always;
                                hidemenu = true;
                            }
                        }
                        else
                            mCurrentItem.Visibility = BarItemVisibility.Never;

                        BarSubItemLink barSubItemLink = currenLink as BarSubItemLink;
                        if (barSubItemLink != null)
                        {
                            foreach (BarItemLink itemLink in barSubItemLink.Item.ItemLinks)
                            {
                                BarButtonItem barButtonItem = itemLink.Item as BarButtonItem;
                                myDataView.RowFilter = ("FORMNAME Like " + "'") + barButtonItem.Tag + "'";
                                if (barButtonItem != null)
                                {
                                    if (myDataView.ToTable().Rows.Count > 0)
                                    {
                                        if (Val.ToBoolean(myDataView.ToTable().Rows[0]["ISVIEW"]) == false)
                                        {
                                            itemLink.Visible = false;
                                            mCurrentItem.Visibility = BarItemVisibility.Never;
                                        }
                                        else
                                        {
                                            mCurrentItem.Visibility = BarItemVisibility.Always;
                                            itemLink.Visible = true;
                                        }
                                    }
                                    else
                                        mCurrentItem.Visibility = BarItemVisibility.Never;
                                }
                                //itemLink.Visible = false;
                            }
                        }
                    }

                    Boolean hideGroup = false;
                    foreach (BarItemLink currenLink in currentGroup.ItemLinks)
                    {
                        mCurrentItem = currenLink.Item;
                        if (mCurrentItem.Visibility == BarItemVisibility.Always)
                            hideGroup = true;
                    }
                    currentGroup.Visible = hideGroup;
                    currentPage.Visible = hidemenu;
                }
            }

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

        private void timer30Second_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!Global.gStrCompanyName.Contains("TRP"))
                {
                    string timecheck = DateTime.Now.ToString("hh:mm tt");
                    if (timecheck == "04:00 PM" || timecheck == "04:30 PM" || timecheck == "05:00 PM" || timecheck == "05:30 PM" || timecheck == "06:00 PM" || timecheck == "06:30 PM" || timecheck == "07:00 PM")
                    {
                        DataTable DTab = new BOMST_FormPermission().GetDataSingleNotificationOrderSend();
                        if (DTab.Rows.Count == 0)
                        {
                            return;
                        }
                        string Str = ""; string Header = "Notification Pending To Send";

                        for (int i = 0; i < DTab.Rows.Count; i++)
                        {
                            Str += "\nStone No : " + Val.ToString(DTab.Rows[i]["STONENO"]) + " ==> Location : " + Val.ToString(DTab.Rows[i]["LOCATION"]);                           
                        }

                        toastNotificationsManager1.Notifications[0].Body = Str;
                        toastNotificationsManager1.Notifications[0].Header = Header;
                        toastNotificationsManager1.ShowNotification(toastNotificationsManager1.Notifications[0]);

                        Global.Message(Header + "\n" + Str);

                        DTab.Dispose();
                        DTab = null;
                    }

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

        private void lblRefreshMenu_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
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

        private void timerStockiestNotification_Tick(object sender, EventArgs e)
        {
            try
            {
                if (Val.ToString(BOConfiguration.gEmployeeProperty.LEDGER_ID).ToUpper() == "B0703EEC-A579-EC11-A8B7-ACB57D1F87CF")
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

        private void barButtonItem21_ItemClick(object sender, ItemClickEventArgs e)
        {
            //System.Diagnostics.Process.Start("notepad.exe");
        }

        private void barButtonItem22_ItemClick(object sender, ItemClickEventArgs e)
        {
            /// System.Diagnostics.Process.Start("calc.exe");
        }


        private void FrmFinalMDIRibbon_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void FrmFinalMDIRibbon_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F2)
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
                    FrmSmartSearch FrmSmartSearch = new FrmSmartSearch();
                    FrmSmartSearch.MdiParent = this;
                    FrmSmartSearch.ShowForm();
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
                }
                else if (e.KeyCode == Keys.F6)
                {
                    DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
                    FrmMemoList FrmMemoList = new FrmMemoList();
                    FrmMemoList.MdiParent = this;
                    FrmMemoList.Tag = "SaleOrder";
                    FrmMemoList.ShowForm(FrmMemoList.FORMTYPE.ORDERCONFIRM, false, "SINGLE");
                    DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
                }
            }
            catch (Exception EX)
            {
                Global.Message(EX.Message);
                this.Cursor = Cursors.Default;
            }
        }
        private void FrmFinalMDIRibbon_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (mBoolSecurityKey == true)
            {
                mStrPassword += Convert.ToChar(e.KeyChar);
            }
        }

        private void FormMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmFormMaster FrmFormMaster = new FrmFormMaster();
            FrmFormMaster.MdiParent = this;
            FrmFormMaster.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void UserPermissionbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmEmployeeRights FrmEmployeeRights = new FrmEmployeeRights();
            FrmEmployeeRights.MdiParent = this;
            FrmEmployeeRights.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void CompanyMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmCompany FrmCompany = new FrmCompany();
            FrmCompany.MdiParent = this;
            FrmCompany.ShowForm("COMPANY");
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void YearMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmYearMaster FrmYearMaster = new FrmYearMaster();
            FrmYearMaster.MdiParent = this;
            FrmYearMaster.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void CountryMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmCountry FrmCountry = new FrmCountry();
            FrmCountry.MdiParent = this;
            FrmCountry.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void CurrancyMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmCurrency FrmCurrency = new FrmCurrency();
            FrmCurrency.MdiParent = this;
            FrmCurrency.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void BannerMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmBanner FrmBanner = new FrmBanner();
            FrmBanner.MdiParent = this;
            FrmBanner.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void DailyRateMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmDailyRate FrmDailyRate = new FrmDailyRate();
            FrmDailyRate.MdiParent = this;
            FrmDailyRate.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void ImportColumnSettingbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmExcelSetting FrmExcelSetting = new FrmExcelSetting();
            FrmExcelSetting.MdiParent = this;
            FrmExcelSetting.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void LedgerMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmLedgerList FrmLedgerList = new FrmLedgerList();
            FrmLedgerList.MdiParent = this;
            FrmLedgerList.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void TermsMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmTerms FrmTerms = new FrmTerms();
            FrmTerms.MdiParent = this;
            FrmTerms.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void ProcessMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmProcess FrmProcess = new FrmProcess();
            FrmProcess.MdiParent = this;
            FrmProcess.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void PartyMergebarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmPartyMerge FrmPartyMerge = new FrmPartyMerge();
            FrmPartyMerge.MdiParent = this;
            FrmPartyMerge.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void LedgerMappingMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmLedgerMappingMaster FrmLedgerMappingMaster = new FrmLedgerMappingMaster();
            FrmLedgerMappingMaster.MdiParent = this;
            FrmLedgerMappingMaster.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void PacketGradingMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmPrdType FrmPrdType = new FrmPrdType();
            FrmPrdType.MdiParent = this;
            FrmPrdType.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void ParameterMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmParameter FrmParameter = new FrmParameter();
            FrmParameter.MdiParent = this;
            FrmParameter.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void SizeMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmSize FrmSize = new FrmSize();
            FrmSize.MdiParent = this;
            FrmSize.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void SizeAndDepartmentMappingbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmSerialNoSizeMaster FrmSerialNoSizeMaster = new FrmSerialNoSizeMaster();
            FrmSerialNoSizeMaster.MdiParent = this;
            FrmSerialNoSizeMaster.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void StoneDayMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmStoneDayMaster FrmStoneDayMaster = new FrmStoneDayMaster();
            FrmStoneDayMaster.MdiParent = this;
            FrmStoneDayMaster.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void FancyColorMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmFancyColor FrmFancyColor = new FrmFancyColor();
            FrmFancyColor.MdiParent = this;
            FrmFancyColor.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void WebsiteMessagebarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmInputBox FrmInputBox = new MahantExport.FrmInputBox();
            FrmInputBox.MdiParent = Global.gMainRef;
            FrmInputBox.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void AboutusbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmInputBoxOther FrmInputBoxOther = new MahantExport.FrmInputBoxOther();
            FrmInputBoxOther.MdiParent = Global.gMainRef;
            FrmInputBoxOther.ShowForm(FrmInputBoxOther.FORMTYPE.ABOUTUS);
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void OurTermsbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmInputBoxOther FrmInputBoxOther = new MahantExport.FrmInputBoxOther();
            FrmInputBoxOther.MdiParent = Global.gMainRef;
            FrmInputBoxOther.ShowForm(FrmInputBoxOther.FORMTYPE.OURTERMS);
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void ContactUsbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmInputBoxOther FrmInputBoxOther = new MahantExport.FrmInputBoxOther();
            FrmInputBoxOther.MdiParent = Global.gMainRef;
            FrmInputBoxOther.ShowForm(FrmInputBoxOther.FORMTYPE.CONTACTUS);
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void PrivacyPolicybarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmInputBoxOther FrmInputBoxOther = new MahantExport.FrmInputBoxOther();
            FrmInputBoxOther.MdiParent = Global.gMainRef;
            FrmInputBoxOther.ShowForm(FrmInputBoxOther.FORMTYPE.PRIVACYPOLICY);
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void InsuranceTypebarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmParameter FrmParameter = new FrmParameter();
            FrmParameter.MdiParent = this;
            FrmParameter.ShowForm("INSURANCETYPE");
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void FileStockUploadbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmStockUpload FrmStockUpload = new FrmStockUpload();
            FrmStockUpload.MdiParent = this;
            FrmStockUpload.ShowForm("SINGLE");
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void PurchaesInvoicebarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMemoList FrmMemoList = new FrmMemoList();
            FrmMemoList.MdiParent = this;
            FrmMemoList.ShowForm(FrmMemoList.FORMTYPE.PURCHASE, false, "SINGLE");
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void PurchseReturnbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMemoList FrmMemoList = new FrmMemoList();
            FrmMemoList.MdiParent = this;
            FrmMemoList.ShowForm(FrmMemoList.FORMTYPE.PURCHASERETURN, false, "SINGLE");
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void PurchseEntryAPIbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmPurchaseAPI FrmPurchaseAPI = new FrmPurchaseAPI();
            FrmPurchaseAPI.MdiParent = this;
            FrmPurchaseAPI.ShowForm(MahantExport.Stock.FrmPurchaseAPI.FORMTYPE.PURCHASEISSUE, null, "SINGLE");
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void PricingParameterUpdatebarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmParameterUpdate FrmParameterUpdate = new FrmParameterUpdate();
            FrmParameterUpdate.MdiParent = this;
            FrmParameterUpdate.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void RoughPurchaseViewbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmRoughPurchaseView FrmRoughPurchaseView = new FrmRoughPurchaseView();
            FrmRoughPurchaseView.MdiParent = this;
            FrmRoughPurchaseView.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void RoughPurchseCPDEntrybarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmRoughPurchaseEntryNew FrmRoughPurchaseEntryNew = new FrmRoughPurchaseEntryNew();
            FrmRoughPurchaseEntryNew.MdiParent = this;
            FrmRoughPurchaseEntryNew.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void KapanbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmKapan FrmKapan = new FrmKapan();
            FrmKapan.MdiParent = this;
            FrmKapan.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void KapanAvgDetailbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmKapanAvgDetail FrmKapanAvgDetail = new FrmKapanAvgDetail();
            FrmKapanAvgDetail.MdiParent = this;
            FrmKapanAvgDetail.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void SaleInvoiceEntrybarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
            FrmMemoEntry.MdiParent = this;
            FrmMemoEntry.ShowForm("", "SALEINVOICEENTRY", 0);
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void SaleInvoiceViewbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmSaleInvoiceView FrmSaleInvoiceView = new FrmSaleInvoiceView();
            FrmSaleInvoiceView.MdiParent = this;
            FrmSaleInvoiceView.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();

        }

        private void DeliveryChallanbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmDeliveryChallan FrmDeliveryChallan = new FrmDeliveryChallan();
            FrmDeliveryChallan.MdiParent = this;
            FrmDeliveryChallan.Show();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void PolishRoughtInwardEntrybarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmPolishRoughInward FrmPolishRoughInward = new FrmPolishRoughInward();
            FrmPolishRoughInward.MdiParent = this;
            FrmPolishRoughInward.Show();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        
        private void StockStatusSearchbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmStockStatusSearch FrmStockStatusSearch = new FrmStockStatusSearch();
            FrmStockStatusSearch.MdiParent = this;
            FrmStockStatusSearch.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void OrderConfirmbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMemoList FrmMemoList = new FrmMemoList();
            FrmMemoList.MdiParent = this;
            FrmMemoList.ShowForm(FrmMemoList.FORMTYPE.ORDERCONFIRM, false, "SINGLE");
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void OrderConformReturnbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMemoList FrmMemoList = new FrmMemoList();
            FrmMemoList.MdiParent = this;
            FrmMemoList.ShowForm(FrmMemoList.FORMTYPE.ORDERCONFIRMRETURN, false, "SINGLE");
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void SaleInvoicebarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMemoList FrmMemoList = new FrmMemoList();
            FrmMemoList.MdiParent = this;
            FrmMemoList.ShowForm(FrmMemoList.FORMTYPE.SALEINVOICE, false, "SINGLE");
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void SaleDeliveryReturnbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMemoList FrmMemoList = new FrmMemoList();
            FrmMemoList.MdiParent = this;
            FrmMemoList.ShowForm(FrmMemoList.FORMTYPE.SALESDELIVERYRETURN, false, "SINGLE");
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void TransactionListbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMemoList FrmMemoList = new FrmMemoList();
            FrmMemoList.MdiParent = this;
            FrmMemoList.ShowForm(FrmMemoList.FORMTYPE.MEMOISSUE, true, "SINGLE");
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void StockBarcodePrintbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmStockBarcodePrint FrmStockBarcodePrint = new FrmStockBarcodePrint();
            FrmStockBarcodePrint.MdiParent = this;
            FrmStockBarcodePrint.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void KapanWiseMumbaiRatebarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmSaleDeliveryLiveStock FrmSaleDeliveryLiveStock = new FrmSaleDeliveryLiveStock();
            FrmSaleDeliveryLiveStock.MdiParent = this;
            FrmSaleDeliveryLiveStock.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void StockTallybarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmStockTally FrmStockTally = new FrmStockTally();
            FrmStockTally.MdiParent = this;
            FrmStockTally.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void LabResultFileUploadbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmSingleFileUpload FrmSingleFileUpload = new FrmSingleFileUpload();
            FrmSingleFileUpload.MdiParent = this;
            FrmSingleFileUpload.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void RFIDScanningbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmRFIDScanning FrmRFIDScanning = new FrmRFIDScanning();
            FrmRFIDScanning.MdiParent = this;
            FrmRFIDScanning.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void StockIssueReturnbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmStockIssueReturn FrmStockIssueReturn = new FrmStockIssueReturn();
            FrmStockIssueReturn.MdiParent = this;
            FrmStockIssueReturn.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void PriceParameterUpdatebarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmParameterUpdate FrmParameterUpdate = new FrmParameterUpdate();
            FrmParameterUpdate.MdiParent = this;
            FrmParameterUpdate.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void SingleStoneUpdatebarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmStoneHistory FrmStoneHistory = new FrmStoneHistory();
            FrmStoneHistory.MdiParent = this;
            FrmStoneHistory.ShowForm(MahantExport.Stock.FrmStoneHistory.FORMTYPE.UPDATE);
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void SingleStonePriceUpdatebarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmSingleStonePriceUpdate FrmSingleStonePriceUpdate = new FrmSingleStonePriceUpdate();
            FrmSingleStonePriceUpdate.MdiParent = this;
            FrmSingleStonePriceUpdate.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void ShapeMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmShapeMaster FrmShapeMaster = new FrmShapeMaster();//Gunjan:07/04/2023
            FrmShapeMaster.MdiParent = this;
            FrmShapeMaster.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void SizeMasterbarButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmSizeMaster FrmSizeMaster = new FrmSizeMaster();//Gunjan:07/04/2023
            FrmSizeMaster.MdiParent = this;
            FrmSizeMaster.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void ColorMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmColorMaster FrmColorMaster = new FrmColorMaster();//Gunjan:07/04/2023
            FrmColorMaster.MdiParent = this;
            FrmColorMaster.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void ClarityMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmClarityMaster FrmClarityMaster = new FrmClarityMaster();//Gunjan:07/04/2023
            FrmClarityMaster.MdiParent = this;
            FrmClarityMaster.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void LotMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmLotMaster FrmLotMaster = new FrmLotMaster();//Gunjan:07/04/2023
            FrmLotMaster.MdiParent = this;
            FrmLotMaster.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void BoxMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmBoxMaster FrmBoxMaster = new FrmBoxMaster();
            FrmBoxMaster.MdiParent = this;
            FrmBoxMaster.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();

        }

        

        private void OpeningEntryFormbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmPolishOpeningStock FrmPolishOpeningStock = new FrmPolishOpeningStock();//Gunjan:14/06/2023
            FrmPolishOpeningStock.MdiParent = this;
            FrmPolishOpeningStock.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        

        private void ParcelLiveStockbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmParcelLiveStock FrmParcelLiveStock = new FrmParcelLiveStock();
            FrmParcelLiveStock.MdiParent = this;
            FrmParcelLiveStock.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void ParcelStockTallybarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmParcelStockTally FrmParcelStockTally = new FrmParcelStockTally();
            FrmParcelStockTally.MdiParent = this;
            FrmParcelStockTally.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void ReportListbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMemoList FrmMemoList = new FrmMemoList();
            FrmMemoList.MdiParent = this;
            FrmMemoList.ShowForm(FrmMemoList.FORMTYPE.MEMOISSUE, true, "SINGLE");
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void DetailReportbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMemoReport FrmMemoReport = new FrmMemoReport();
            FrmMemoReport.MdiParent = this;
            FrmMemoReport.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();

        }

        private void MemoOutstandingReportbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMemoReportOS FrmMemoReportOS = new FrmMemoReportOS();
            FrmMemoReportOS.MdiParent = this;
            FrmMemoReportOS.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void StoneHistroybarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmStoneHistory FrmStoneHistory = new FrmStoneHistory();
            FrmStoneHistory.MdiParent = this;
            FrmStoneHistory.ShowForm(MahantExport.Stock.FrmStoneHistory.FORMTYPE.DISPLAY);
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void CostProfitLossProfotReportbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMemoReportNPNL FrmMemoReportNPNL = new FrmMemoReportNPNL();
            FrmMemoReportNPNL.MdiParent = this;
            FrmMemoReportNPNL.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void SalesAnalysisChartbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmSalesAnalysis FrmSalesAnalysis = new FrmSalesAnalysis();
            FrmSalesAnalysis.MdiParent = this;
            FrmSalesAnalysis.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void StockOpeninfClosingReportbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmOpeningClosingReportNew FrmOpeningClosingReportNew = new FrmOpeningClosingReportNew();
            FrmOpeningClosingReportNew.MdiParent = this;
            FrmOpeningClosingReportNew.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void ClorClarityReportbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmQuickSearch FrmQuickSearch = new FrmQuickSearch();
            FrmQuickSearch.MdiParent = this;
            FrmQuickSearch.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void StonrOfferPriceReportbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmOfferPriceReport FrmOfferPriceReport = new FrmOfferPriceReport();
            FrmOfferPriceReport.MdiParent = this;
            FrmOfferPriceReport.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void GradingComparisionbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmComparisionViewAdmin FrmComparisionViewAdmin = new FrmComparisionViewAdmin();
            FrmComparisionViewAdmin.MdiParent = this;
            FrmComparisionViewAdmin.ShowForm(MahantExport.Masters.FrmComparisionViewAdmin.FORMTYPE.ADMIN);
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void StockAnalysisChartbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmStockAnalysis FrmStockAnalysis = new FrmStockAnalysis();
            FrmStockAnalysis.MdiParent = this;
            FrmStockAnalysis.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void MFGComparisionReportbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMFGComparisionReport FrmMFGComparisionReport = new FrmMFGComparisionReport();
            FrmMFGComparisionReport.MdiParent = this;
            FrmMFGComparisionReport.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void RoundReportbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmRoundReport FrmRoundReport = new FrmRoundReport();
            FrmRoundReport.MdiParent = this;
            FrmRoundReport.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void SaleReportbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmSaleReport FrmSaleReport = new FrmSaleReport();
            FrmSaleReport.MdiParent = this;
            FrmSaleReport.Show();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void StockParcelOpeningReportbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmOpeningClosingParcel FrmOpeningClosingParcel = new FrmOpeningClosingParcel();
            FrmOpeningClosingParcel.MdiParent = this;
            FrmOpeningClosingParcel.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void StockMarketingReportbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmOpeningClosingMarketing FrmOpeningClosingMarketing = new FrmOpeningClosingMarketing();
            FrmOpeningClosingMarketing.MdiParent = this;
            FrmOpeningClosingMarketing.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

       
        private void MFGGradingEntrybarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMFGGradingEntry FrmMFGGradingEntry = new FrmMFGGradingEntry();
            FrmMFGGradingEntry.MdiParent = this;
            FrmMFGGradingEntry.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();

        }

        private void GradingViewbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMFGGradingView FrmMFGGradingView = new FrmMFGGradingView();
            FrmMFGGradingView.MdiParent = this;
            FrmMFGGradingView.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();

        }

        private void MFGSingleStonePriceUpdatebarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmSingleStonePriceUpdate FrmSingleStonePriceUpdate = new FrmSingleStonePriceUpdate();
            FrmSingleStonePriceUpdate.MdiParent = this;
            FrmSingleStonePriceUpdate.ShowForm_MFG();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();

        }

        private void RapSizeMappingbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMixRapSizeMapping FrmRapSizeMapping = new FrmMixRapSizeMapping();
            FrmRapSizeMapping.MdiParent = this;
            FrmRapSizeMapping.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();

        }

        private void PriceChartMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmParameterDiscount FrmParameterDiscount = new FrmParameterDiscount();
            FrmParameterDiscount.MdiParent = this;
            FrmParameterDiscount.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();

        }

        private void RapnetPriceUpdatebarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmPriceRevised FrmPriceRevised = new FrmPriceRevised();
            FrmPriceRevised.MdiParent = this;
            FrmPriceRevised.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();

        }

        private void PriceDateMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmPriceDateMaster FrmPriceDateMaster = new FrmPriceDateMaster();
            FrmPriceDateMaster.MdiParent = this;
            FrmPriceDateMaster.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();

        }

        private void MixAssortmentPricebarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMixPriceChart FrmParcelMixClarityPriceUpload = new FrmMixPriceChart();
            FrmParcelMixClarityPriceUpload.MdiParent = this;
            FrmParcelMixClarityPriceUpload.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();

        }

        private void MixCLVPriceChartbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmCLVPriceChartUpload FrmCLVPriceChartUpload = new FrmCLVPriceChartUpload();
            FrmCLVPriceChartUpload.MdiParent = this;
            FrmCLVPriceChartUpload.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();

        }

        private void MixCLVPriceChartSummarybarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmCLVPriceChartSummary FrmCLVPriceChartSummary = new FrmCLVPriceChartSummary();
            FrmCLVPriceChartSummary.MdiParent = this;
            FrmCLVPriceChartSummary.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void CLVMixSizeRapSizeMapingbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmClvMixRapSizeMapping FrmClvMixRapSizeMapping = new FrmClvMixRapSizeMapping();
            FrmClvMixRapSizeMapping.MdiParent = this;
            FrmClvMixRapSizeMapping.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void StoneSystemPricingbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmStoneSystemPricing FrmStoneSystemPricing = new FrmStoneSystemPricing();
            FrmStoneSystemPricing.MdiParent = this;
            FrmStoneSystemPricing.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void LabChargeUploadbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmLabChargesUpload FrmLabChargesUpload = new FrmLabChargesUpload();
            FrmLabChargesUpload.MdiParent = this;
            FrmLabChargesUpload.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void RapnetPriceUpadatebarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmRapnetPriceCostUpdate FrmRapnetPriceCostUpdate = new FrmRapnetPriceCostUpdate();
            FrmRapnetPriceCostUpdate.MdiParent = this;
            FrmRapnetPriceCostUpdate.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void RapnetStockSyncbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmRapNetStockSync objFrmRapNetStockSync = new FrmRapNetStockSync();
            objFrmRapNetStockSync.MdiParent = this;
            objFrmRapNetStockSync.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void CustomerAutoEmailSettingbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmCustomerAutoEmailSetting FrmCustomerAutoEmailSetting = new FrmCustomerAutoEmailSetting();
            FrmCustomerAutoEmailSetting.MdiParent = this;
            FrmCustomerAutoEmailSetting.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void CustomerDemandViewbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmDimandView FrmDimandView = new FrmDimandView();
            FrmDimandView.MdiParent = this;
            FrmDimandView.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void MatchingStockbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMatchingStock FrmMatchingStock = new FrmMatchingStock();
            FrmMatchingStock.MdiParent = this;
            FrmMatchingStock.ShowForm(FrmMatchingStock.FORMTYPE.MEMOISSUE, true, "SINGLE");
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void TargetCreateMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmTargetCreateMaster FrmTargetCreateMaster = new FrmTargetCreateMaster();
            FrmTargetCreateMaster.MdiParent = this;
            FrmTargetCreateMaster.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void WebActivityListbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmWebActivityList FrmWebActivityList = new FrmWebActivityList();
            FrmWebActivityList.MdiParent = this;
            FrmWebActivityList.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void DashBoardbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            //DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            //FrmDashBoard FrmDashBoard = new FrmDashBoard();
            //FrmDashBoard.MdiParent = this;
            //FrmDashBoard.ShowForm();
            //DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void AboutUsbarButtonItem30_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmAboutUs FrmAboutUs = new FrmAboutUs();
            FrmAboutUs.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void EmailSendbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmEmailSend FrmEmailSend = new FrmEmailSend();
            FrmEmailSend.MdiParent = this;
            FrmEmailSend.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void EmailSendTestingbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmEmailSendTesting FrmEmailSendTesting = new FrmEmailSendTesting();
            FrmEmailSendTesting.MdiParent = this;
            FrmEmailSendTesting.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void ScreenCapturebarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmScreenCapture FrmScreenCapture = new MahantExport.FrmScreenCapture();
            FrmScreenCapture.MdiParent = Global.gMainRef;
            FrmScreenCapture.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void APISyncbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmAPI FrmAPI = new FrmAPI();
            FrmAPI.MdiParent = this;
            FrmAPI.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void CertificateDownloadbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmCertificateDownload FrmCertificateDownload = new FrmCertificateDownload();
            FrmCertificateDownload.MdiParent = this;
            FrmCertificateDownload.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void ImageCertiFlagUpdatebarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmImageVideoCertiUrlUpdate FrmImageVideoCertiUrlUpdate = new FrmImageVideoCertiUrlUpdate();
            FrmImageVideoCertiUrlUpdate.MdiParent = this;
            FrmImageVideoCertiUrlUpdate.Show();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void ConnectRFIDMachinebarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmConnectRFIDMachine FrmConnectRFIDMachine = new FrmConnectRFIDMachine();
            FrmConnectRFIDMachine.ShowDialog();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void MediaUpdatebarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmStockMediaUpdate FrmStockMediaUpdate = new FrmStockMediaUpdate();
            FrmStockMediaUpdate.MdiParent = this;
            FrmStockMediaUpdate.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void APISettingbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmAPISetting FrmAPISetting = new FrmAPISetting();
            FrmAPISetting.MdiParent = this;
            FrmAPISetting.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void skinRibbonGalleryBarItem5_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
        
        private void BtnBack_Click(object sender, EventArgs e)
        {
            if (Global.Confirm("Are You Sure Want To Close Application?") == System.Windows.Forms.DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void CoupleStoneUploadbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmCoupleStoneUpload FrmCoupleStoneUpload = new FrmCoupleStoneUpload();
            FrmCoupleStoneUpload.MdiParent = this;
            FrmCoupleStoneUpload.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void IssueReturnLiveStockbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            //DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));//Comment By Gunjan:19-09-2023
            //FrmSinglePacketLiveStock FrmSinglePacketLiveStock = new FrmSinglePacketLiveStock();
            //FrmSinglePacketLiveStock.MdiParent = this;
            //FrmSinglePacketLiveStock.ShowForm();
            //DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmSinglePacketLiveStockOLD FrmSinglePacketLiveStock = new FrmSinglePacketLiveStockOLD();
            FrmSinglePacketLiveStock.MdiParent = this;
            FrmSinglePacketLiveStock.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();//End as Gunjan
        }

        private void barButtonItem33_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmLiveStockSearch FrmLiveStockSearch = new FrmLiveStockSearch();
            FrmLiveStockSearch.MdiParent = this;
            FrmLiveStockSearch.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem34_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            Stock.FrmTransferToMarketing FrmTransferToMarketing = new Stock.FrmTransferToMarketing();
            FrmTransferToMarketing.MdiParent = this;
            FrmTransferToMarketing.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem35_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmSingleLiveStockNew FrmSingleLiveStockNew = new FrmSingleLiveStockNew();
            FrmSingleLiveStockNew.MdiParent = this;
            FrmSingleLiveStockNew.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();

        }

        private void barButtonItem36_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmStoneCreateAndUpdate FrmStoneCreateAndUpdate = new FrmStoneCreateAndUpdate();
            FrmStoneCreateAndUpdate.MdiParent = this;
            FrmStoneCreateAndUpdate.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void StyleMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmStyleMaster FrmStyleMaster = new FrmStyleMaster();
            FrmStyleMaster.MdiParent = this;
            FrmStyleMaster.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void LabResultViewButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmSingleFileUploadView FrmSingleFileUploadView = new FrmSingleFileUploadView();
            FrmSingleFileUploadView.MdiParent = this;
            FrmSingleFileUploadView.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem48_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmStockTallyReport FrmStockTallyReport = new FrmStockTallyReport();
            FrmStockTallyReport.MdiParent = this;
            FrmStockTallyReport.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem49_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmGIAControlNoMap FrmGIAControlNoMap = new FrmGIAControlNoMap();
            FrmGIAControlNoMap.MdiParent = this;
            FrmGIAControlNoMap.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void BrachReceiveButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmBranchReceive FrmBranchReceive = new FrmBranchReceive();
            FrmBranchReceive.MdiParent = this;
            FrmBranchReceive.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void KapanInwardButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmKapanInward FrmKapanInward = new FrmKapanInward();
            FrmKapanInward.MdiParent = this;
            FrmKapanInward.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void SizeWiseAssortmentButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmSizeAssortment FrmSizeAssortment = new FrmSizeAssortment();
            FrmSizeAssortment.MdiParent = this;
            FrmSizeAssortment.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void MixSizebarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMixSize FrmMixSize = new FrmMixSize();
            FrmMixSize.MdiParent = this;
            FrmMixSize.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void MixClaritybarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmParameter FrmParameter = new FrmParameter();
            FrmParameter.MdiParent = this;
            FrmParameter.ShowForm("MIX_CLARITY");
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
            
        }

        private void ClarityAssortmentbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmClarityAssortment FrmClarityAssortment = new FrmClarityAssortment();
            FrmClarityAssortment.MdiParent = this;
            FrmClarityAssortment.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
           
        }

        private void ClarityAssortmentViewbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmClarityAssortmentView FrmClarityAssortmentView = new FrmClarityAssortmentView();
            FrmClarityAssortmentView.MdiParent = this;
            FrmClarityAssortmentView.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void BombayAssortmentViewReportButtonItem51_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmBombayTransferViewReport FrmBombayTransferViewReport = new FrmBombayTransferViewReport();
            FrmBombayTransferViewReport.MdiParent = this;
            FrmBombayTransferViewReport.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void MixPriceChartbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMixPriceChart FrmMixPriceChart = new FrmMixPriceChart();
            FrmMixPriceChart.MdiParent = this;
            FrmMixPriceChart.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void PriceDateMasterButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmPriceDateMaster FrmPriceDateMaster = new FrmPriceDateMaster();
            FrmPriceDateMaster.MdiParent = this;
            FrmPriceDateMaster.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void ParcelLiveStpckbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmParcelLiveStockNew FrmParcelLiveStockNew = new FrmParcelLiveStockNew();
            FrmParcelLiveStockNew.MdiParent = this;
            FrmParcelLiveStockNew.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void ParcelBoxMasterbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmParcelBoxMasterNew FrmParcelBoxMasterNew = new FrmParcelBoxMasterNew();
            FrmParcelBoxMasterNew.MdiParent = this;
            FrmParcelBoxMasterNew.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void ReportMaster_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmReportMasterNew FrmReportMasterNew = new FrmReportMasterNew();
            FrmReportMasterNew.MdiParent = this;
            FrmReportMasterNew.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void GridReportForStock_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmFilterStockReport FrmFilterStockReport = new FrmFilterStockReport();
            FrmFilterStockReport.MdiParent = this;
            FrmFilterStockReport.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem51_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmParcelStockView FrmParcelStockView = new FrmParcelStockView();
            FrmParcelStockView.MdiParent = this;
            FrmParcelStockView.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem52_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmStoneCreateAndUpdateNew FrmStoneCreateAndUpdateNew = new FrmStoneCreateAndUpdateNew();
            FrmStoneCreateAndUpdateNew.MdiParent = this;
            FrmStoneCreateAndUpdateNew.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem53_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmstonePriceHistoryComparision FrmstonePriceHistoryComparision = new FrmstonePriceHistoryComparision();
            FrmstonePriceHistoryComparision.MdiParent = this;
            FrmstonePriceHistoryComparision.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem54_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmDashBoard FrmDashBoard = new FrmDashBoard();
            FrmDashBoard.MdiParent = this;
            FrmDashBoard.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void BarBtnFinalStockReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmFinalStockReport FrmFinalStockReport = new FrmFinalStockReport();
            FrmFinalStockReport.MdiParent = this;
            FrmFinalStockReport.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void BtnBillWisePayment_ItemClick(object sender, ItemClickEventArgs e)
        {
          
        }

        private void barButtonItem55_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmPaymentPending FrmPaymentPending = new FrmPaymentPending();
            FrmPaymentPending.MdiParent = this;
            FrmPaymentPending.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem56_ItemClick(object sender, ItemClickEventArgs e)
        {
            //DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            //FrmPaymentRemittance FrmPaymentRemittance = new FrmPaymentRemittance();
            //FrmPaymentRemittance.MdiParent = this;
            //FrmPaymentRemittance.ShowForm();
            //DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmPaymentReceive FrmPaymentReceive = new FrmPaymentReceive();
            FrmPaymentReceive.MdiParent = this;
            FrmPaymentReceive.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void LiveStockSearchbarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void btnPricingView_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmPricingViewNew FrmPriceView = new FrmPricingViewNew();
            FrmPriceView.MdiParent = this;
            FrmPriceView.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem37_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmProductCategory FrmProductCategory = new FrmProductCategory();
            FrmProductCategory.MdiParent = this;
            FrmProductCategory.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void timerorderNotifiaction_Tick(object sender, EventArgs e)
        {
            try
            {
                //if (Val.ToString(BOConfiguration.ComputerIP).ToUpper() != "192.168.0.131")
                //{
                //    DataTable DTab = new BOMST_FormPermission().GetOrderConfNotificationData_ForStockiest();
                //    if (DTab.Rows.Count == 0)
                //    {
                //        return;
                //    }
                //    string Str = "", MemoID = "" , Header = "";
                //    foreach (DataRow DRow in DTab.Rows)
                //    {
                //        Str = "";
                //        //Header = "Process Details";
                //        MemoID = Val.ToString(DRow["Memo_ID"]);
                //        Str += "\nParty Name : " + Val.ToString(DRow["PARTY"]);
                //        Str += "\nSeller Name : " + Val.ToString(DRow["SELLER"]);
                //        Str += "\nTotal Cts : " + Val.ToString(DRow["TotalCarat"]) + " (" + Val.ToString(DRow["TotalPcs"]) + ")";
                //        Str += "\nProcess : " + Val.ToString(DRow["Status"]);
                      
                //        toastNotificationsManager1.Notifications[0].Body = Str;
                //        toastNotificationsManager1.Notifications[0].Header = Header;
                //        toastNotificationsManager1.ShowNotification(toastNotificationsManager1.Notifications[0]);

                //        //Alert(Str, Form_Alert.enmType.Info, MemoID);

                //        new BOMST_FormPermission().UpdateNotificationOrderConfirm(MemoID);
                //    }
                //    DTab.Dispose();
                //    DTab = null;
                //}
            }
            catch (Exception EX)
            {
            }
        }

        private void barButtonItem57_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmBulkPredictionEntry FrmBulkPredictionEntry = new FrmBulkPredictionEntry();
            FrmBulkPredictionEntry.MdiParent = this;
            FrmBulkPredictionEntry.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void BarBtnSaleComparisionReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmSaleComparisionReport FrmSaleComparisionReport = new FrmSaleComparisionReport();
            FrmSaleComparisionReport.MdiParent = this;
            FrmSaleComparisionReport.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void BarBtnRepeatCustomerReport_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmRepeatCustomerReport FrmRepeatCustomerReport = new FrmRepeatCustomerReport();
            FrmRepeatCustomerReport.MdiParent = this;
            FrmRepeatCustomerReport.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void BarBtnRapComparision_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmRapComparision FrmRapComparision = new FrmRapComparision();
            FrmRapComparision.MdiParent = this;
            FrmRapComparision.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem58_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmGIAAPIIntegration FrmGIAAPIIntegration = new FrmGIAAPIIntegration();
            FrmGIAAPIIntegration.MdiParent = this;
            FrmGIAAPIIntegration.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem43_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmStyleMaster FrmStyleMaster = new FrmStyleMaster();
            FrmStyleMaster.MdiParent = this;
            FrmStyleMaster.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem47_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmSubscribeMaster FrmSubscribeMaster = new FrmSubscribeMaster();
            FrmSubscribeMaster.MdiParent = this;
            FrmSubscribeMaster.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem38_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmProductSubCategory FrmProductSubCategory = new FrmProductSubCategory();
            FrmProductSubCategory.MdiParent = this;
            FrmProductSubCategory.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem40_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMaterial FrmMaterial = new FrmMaterial();
            FrmMaterial.MdiParent = this;
            FrmMaterial.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem39_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMaterialType FrmMaterialType = new FrmMaterialType();
            FrmMaterialType.MdiParent = this;
            FrmMaterialType.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem41_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmCollectionMaster FrmCollectionMaster = new FrmCollectionMaster();
            FrmCollectionMaster.MdiParent = this;
            FrmCollectionMaster.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem46_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmDailyRate FrmDailyRate = new FrmDailyRate();
            FrmDailyRate.MdiParent = this;
            FrmDailyRate.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem42_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmDailyMetalRate FrmDailyMetalRate = new FrmDailyMetalRate();
            FrmDailyMetalRate.MdiParent = this;
            FrmDailyMetalRate.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem44_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmOrderList FrmOrderList = new FrmOrderList();
            FrmOrderList.MdiParent = this;
            FrmOrderList.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem45_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmCartList FrmCartList = new FrmCartList();
            FrmCartList.MdiParent = this;
            FrmCartList.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem48_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMixSize FrmMixSize = new FrmMixSize();
            FrmMixSize.MdiParent = this;
            FrmMixSize.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem49_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmParameter FrmParameter = new FrmParameter();
            FrmParameter.MdiParent = this;
            FrmParameter.ShowForm("MIX_CLARITY");
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem54_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmPriceDateMaster FrmPriceDateMaster = new FrmPriceDateMaster();
            FrmPriceDateMaster.MdiParent = this;
            FrmPriceDateMaster.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem57_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmParcelBoxMasterNew FrmParcelBoxMasterNew = new FrmParcelBoxMasterNew();
            FrmParcelBoxMasterNew.MdiParent = this;
            FrmParcelBoxMasterNew.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem58_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmKapanInward FrmKapanInward = new FrmKapanInward();
            FrmKapanInward.MdiParent = this;
            FrmKapanInward.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem53_ItemClick_1(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmMixPriceChart FrmMixPriceChart = new FrmMixPriceChart();
            FrmMixPriceChart.MdiParent = this;
            FrmMixPriceChart.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem60_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmSizeAssortment FrmSizeAssortment = new FrmSizeAssortment();
            FrmSizeAssortment.MdiParent = this;
            FrmSizeAssortment.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem61_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmClarityAssortment FrmClarityAssortment = new FrmClarityAssortment();
            FrmClarityAssortment.MdiParent = this;
            FrmClarityAssortment.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem62_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmClarityAssortmentView FrmClarityAssortmentView = new FrmClarityAssortmentView();
            FrmClarityAssortmentView.MdiParent = this;
            FrmClarityAssortmentView.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem63_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmBombayTransferViewReport FrmBombayTransferViewReport = new FrmBombayTransferViewReport();
            FrmBombayTransferViewReport.MdiParent = this;
            FrmBombayTransferViewReport.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem64_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmParcelLiveStockNew FrmParcelLiveStockNew = new FrmParcelLiveStockNew();
            FrmParcelLiveStockNew.MdiParent = this;
            FrmParcelLiveStockNew.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem65_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmParcelStockView FrmParcelStockView = new FrmParcelStockView();
            FrmParcelStockView.MdiParent = this;
            FrmParcelStockView.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }

        private void barButtonItem67_ItemClick(object sender, ItemClickEventArgs e)
        {
            DevExpress.XtraSplashScreen.SplashScreenManager.ShowForm(typeof(WaitFormNew));
            FrmBulkPropertyUpdate FrmBulkPropertyUpdate = new FrmBulkPropertyUpdate();
            FrmBulkPropertyUpdate.MdiParent = this;
            FrmBulkPropertyUpdate.ShowForm();
            DevExpress.XtraSplashScreen.SplashScreenManager.CloseForm();
        }
    }


}