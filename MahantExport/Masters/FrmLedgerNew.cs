using MahantExport.Utility;
using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using DevExpress.XtraPrinting;
using Google.API.Translate;
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
using System.Windows.Forms;
using System.Xml;

namespace MahantExport.Masters
{
    public partial class FrmLedgerNew : DevControlLib.cDevXtraForm
    {
        string mStrStatus = string.Empty;
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_Ledger ObjMast = new BOMST_Ledger();
        BOFormPer ObjPer = new BOFormPer();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();

        DataTable DtabExperience = new DataTable();
        DataTable DtabFamily = new DataTable();
        DataTable DtabReference = new DataTable();
        DataTable DtabAttachment = new DataTable();
        DataTable DtabBank = new DataTable();
        DataTable DtabPartnerDetail = new DataTable();
        string mStrPartyType = "", mStrKYCType = "", mStrMode = "";

        #region Property Settings

        public FrmLedgerNew()
        {
            InitializeComponent();
        }

        public FORMTYPE mFormType = FORMTYPE.BROKER;
        public enum FORMTYPE
        {
            PURCHASE = 1,
            SALE = 2,
            BROKER = 3,
            CASH = 4,
            BANK = 5,
            AIRFRIGHT = 6,
            CURIOUR = 7
        }
        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            this.Text = "PATRY MASTER";

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            txtLedgerCode.Focus();

            BtnAdd_Click(null, null);

        //    xtraTabPage6.PageVisible = false;

            GrpKYCTypeSelection.BringToFront();
            GrpKYCTypeSelection.Visible = true;
            GrpKYCTypeSelection.Enabled = true;
            panel1.Enabled = false;
            xtraTabControl2.Enabled = false;
            panel3.Enabled = false;
            CmbKYCType.Focus();
            mStrMode = "Add";
            this.Show();
            CmbPartyType.Focus();
        }
        public void ShowForm(string StrPartyType, string StrParty_ID)  // Used Foe Getting Party Data
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            this.Text = "PATRY MASTER";

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            mStrPartyType = StrPartyType;
            CmbPartyType.SelectedItem = StrPartyType;
            CmbPartyTypeBYR.SelectedItem = StrPartyType;

            txtLedgerName.Tag = StrParty_ID;
            txtLedgerCode_Validated(null, null);
            mStrMode = "Edit";
           // xtraTabPage6.PageVisible = false;
            if (BOConfiguration.DEPTNAME == "ACCOUNT")
            {
                ChkSecondAddress.Visible = true;
                txtSecondAdd.Visible = true;
                txtSecondAdd1.Visible = true; //K : 21/11/2022
                txtSecondAdd2.Visible = true;
                ChkIsTdsLimit.Visible = true;
            }
            else
            {

                ChkSecondAddress.Visible = false;
                txtSecondAdd.Visible = false;
                txtSecondAdd1.Visible = false; //K : 21/11/2022
                txtSecondAdd2.Visible = false;
                ChkIsTdsLimit.Visible = false;
            }
            this.Show();
            txtLedgerName.Focus();
        }
        public void ShowForm(string StrPartyType) //Add : Pinali : 29-08-2019 : Used in Purchase -> Purchase Party
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            this.Text = "PATRY MASTER";

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            mStrPartyType = StrPartyType;
            txtLedgerCode.Focus();

            BtnAdd_Click(null, null);

            CmbPartyType.Enabled = false;
            CmbPartyTypeBYR.Enabled = false;
           // xtraTabPage6.PageVisible = false;

            if (mStrPartyType == "SALE" || mStrPartyType == "PURCHASE")
            {
                GrpKYCTypeSelection.BringToFront();
                GrpKYCTypeSelection.Visible = true;
                GrpKYCTypeSelection.Enabled = true;
                panel1.Enabled = false;
                xtraTabControl2.Enabled = false;
                panel3.Enabled = false;
                CmbPartyTypeBYR.Text = mStrPartyType;
                CmbKycTypeBYR.Focus();
            }
            else
            {
                xtraTabPage5.PageVisible = false;
            }
            mStrMode = "Add";
            if (BOConfiguration.DEPTNAME == "ACCOUNT")
            {
                ChkSecondAddress.Visible = true;
                txtSecondAdd.Visible = true;
                txtSecondAdd1.Visible = true; //K : 21/11/2022
                txtSecondAdd2.Visible = true;
                ChkIsTdsLimit.Visible = true;
            }
            else
            {

                ChkSecondAddress.Visible = false;
                txtSecondAdd.Visible = false;
                txtSecondAdd1.Visible = false;//K : 21/11/2022
                txtSecondAdd2.Visible = false;
                ChkIsTdsLimit.Visible = false;
            }
            this.Show();
            CmbPartyType.Focus();
        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            //ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMast);
            ObjFormEvent.ObjToDisposeList.Add(Val);
        }

        #endregion

        #region Validation

        private bool ValSave()
        {
            if (txtBusinessRegNo.Text.Trim().Length == 0 && Val.ToString(BOConfiguration.COMPANY_ID).ToUpper() == "FE4C657D-5452-44D3-84F7-C8C71E20446E") //HK Comp ma j BRNO Compulsory
            {
                Global.Message("BR No Is Required");
                txtBusinessRegNo.Focus();
                return false;
            }
            if (txtLedgerName.Text.Trim().Length == 0 || txtLedgerName.ForeColor == Color.Silver)
            {
                Global.Message("Party Name Is Required");
                txtLedgerName.Focus();
                return false;
            }
            if (mStrKYCType == "BILLINGKYC")
            {
                if (CmbPartyType.Text.ToUpper() != "COMMISSION")
                {
                    if ((txtAccCode.Text.Trim().Length == 0 || txtAccCode.ForeColor == Color.Silver) && Val.ToString(BOConfiguration.COMPANY_ID).ToUpper() == "EBC650E4-80D7-4651-A2B4-625A4F6E5BE9") //SHREE KRISHNA EXPORT : CompanyId coz HK Form mathi Party Data Save time AcCode Required nathi
                    {
                        Global.Message("Account Code Is Required");
                        txtAccCode.Focus();
                        return false;
                    }
                }
                if (txtAccCode.Text == Val.ToString(0))
                {
                    Global.Message("Account Code Is Not Allow Zero");
                    txtAccCode.Focus();
                    return false;
                }
                int length = txtAccCode.Text.Length;
                if (length > 5)
                {
                    Global.Message("Please Enter Account Code Is Less Then Six");
                    txtAccCode.Focus();
                    return false;
                }
            }
            else if (mStrKYCType == "BUYERKYC")
            {
                if (txtDefaultSellerBYR.Text.Trim().Length == 0)
                {
                    Global.Message("Saller Name Is Required");
                    txtDefaultSellerBYR.Focus();
                    return false;
                }
                //if (txtEmailIDBYR.Text.Trim().Length == 0)
                //{
                //    Global.Message("Email ID Is Required");
                //    txtEmailIDBYR.Focus();
                //    return false;
                //}
            }

            //if (Val.ToString(CmbPartyType.Text) == "SALE") //Validation For Sale Party
            //{
            //    if (Val.ToString(txtDefaultSeller.Text).Trim().Equals(string.Empty))
            //    {
            //        Global.Message("Default Seller Is Required");
            //        txtDefaultSeller.Focus();
            //        return false;
            //    }
            //}

                //else if (Val.ToString(txtEmailID.Text).Trim().Equals(string.Empty) && (mFormType == FORMTYPE.CASH || mFormType == FORMTYPE.BANK))
                //{
                //    Global.Message("Email ID Is Required");
                //    txtEmailID.Focus();
                //    return false;
                //}

                //if (Val.ToString(CmbPartyType.Text) == "EMPLOYEE") //Validation For Sale Party
                //{
                //    if (Val.ToString(txtDepartment.Text).Trim().Equals(string.Empty))
                //    {
                //        Global.Message("Department Is Required");
                //        txtDepartment.Focus();
                //        return false;
                //    }
                //    else if (Val.ToString(txtDesignation.Text).Trim().Equals(string.Empty))
                //    {
                //        Global.Message("Designation Is Required");
                //        txtDesignation.Focus();
                //        return false;

                //    }
                //    else if (Val.ToString(txtMobileNo1.Text).Trim().Equals(string.Empty))
                //    {
                //        Global.Message("Mobile No 1  Is Required");
                //        txtMobileNo1.Focus();
                //        return false;
                //    }
                //    else if (Val.ToString(txtEmailID.Text).Trim().Equals(string.Empty))
                //    {
                //        Global.Message("Email ID  Is Required");
                //        txtEmailID.Focus();
                //        return false;
                //    }
                //}

                //if (Val.ToString(CmbPartyType.Text) == "SALE") //Validation For Sale Party
                //{
                //    if (Val.ToString(txtDefaultSeller.Text).Trim().Equals(string.Empty))
                //    {
                //        Global.Message("Default Seller Is Required");
                //        txtDefaultSeller.Focus();
                //        return false;
                //    }
                //    else if (Val.ToString(txtBroker.Text).Trim().Equals(string.Empty) && ChkIsNoBroker.Checked == false)
                //    {
                //        Global.Message("Broker Is Required");
                //        txtBroker.Focus();
                //        return false;
                //    }


                //    else if (Val.ToString(txtCompany.Text).Trim().Equals(string.Empty))
                //    {
                //        Global.Message("Company Name Is Required");
                //        txtCompany.Focus();
                //        return false;
                //    }
                //    else if (Val.ToString(txtMobileNo1.Text).Trim().Equals(string.Empty) && Val.ToString(txtMobileNo2.Text).Trim().Equals(string.Empty) && Val.ToString(txtLandlineNo.Text).Trim().Equals(string.Empty))
                //    {
                //        Global.Message("Mobile No/Telephone No Is Required");
                //        txtMobileNo1.Focus();
                //        return false;
                //    }

                //    else if (Val.ToString(txtQQID.Text).Trim().Equals(string.Empty)
                //            && Val.ToString(txtWeChatID.Text).Trim().Equals(string.Empty)
                //            && Val.ToString(txtSkypeID.Text).Trim().Equals(string.Empty))
                //    {
                //        Global.Message("'QQID / WeChat / Skype' Any One Detail Is Required");
                //        txtQQID.Focus();
                //        return false;
                //    }

                //    else if (Val.ToString(CmbStatus.SelectedItem) == "ACTIVE")
                //    {
                //        if (Val.ToString(txtUserName.Text).Trim().Equals(string.Empty))
                //        {
                //            Global.Message("UserName Is Required");
                //            txtUserName.Focus();
                //            return false;
                //        }
                //        if (Val.ToString(txtPassword.Text).Trim().Equals(string.Empty))
                //        {
                //            Global.Message("Password Is Required");
                //            txtPassword.Focus();
                //            return false;
                //        }
                //    }

                //    if (Val.ToString(txtLocation_ID.Text).Trim().Equals(string.Empty) && (Val.ToString(CmbPartyType.Text) == "LEDGER" ||Val.ToString(CmbPartyType.Text) == "BRANCH RECEIVE"))
                //    {
                //        Global.Message("Please Enter Location");
                //        txtLocation_ID.Focus();
                //        return false;
                //    }
                //}

                //if ((Val.Val(txtOpeningDrUSD.Text) != 0 || Val.Val(txtOpeningCrUSD.Text) != 0) && (Val.ToString(txtCurrency.Text).Trim().Equals(string.Empty)))
                //{
                //    Global.Message("Please Select Currency For Opening Amount.");
                //    txtCurrency.Focus();
                //    return false;
                //}
                //if ((Val.Val(txtOpeningDrUSD.Text) != 0 || Val.Val(txtOpeningCrUSD.Text) != 0) && (Val.Val(txtExcRate.Text) == 0))
                //{
                //    Global.Message("Please Select ExcRate For Opening Amount.");
                //    txtExcRate.Focus();
                //    return false;
                //}

                //else if (Val.ToString(txtSCountry.Text).Trim().Equals(string.Empty) && (mFormType == FORMTYPE.CASH || mFormType == FORMTYPE.BANK))
                //{
                //    Global.Message("Country Is Required");
                //    txtSCountry.Focus();
                //    return false;
                //}

                if (DtpBirthDate.Checked)
            {
                DateTime bday = DateTime.Parse(DtpBirthDate.Text);
                DateTime joindate = DateTime.Parse(DTPDateOfJoin.Text);
                int age = joindate.Year - bday.Year;
                if (age < 18)
                {
                    Global.Message("Please Enter Valid JoinDate.You must 18 years old ... ");
                    DTPDateOfJoin.Focus();
                    return false;
                }
            }
            return true;
        }

        private bool ValDelete()
        {
            if (txtLedgerName.Text.Trim().Length == 0)
            {
                Global.Message("Employee Code Is Required");
                txtLedgerName.Focus();
                return false;
            }

            return true;
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

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            CmbStatus.SelectedIndex = 0;
            CmbGender.SelectedIndex = 0;

            if (Val.ToString(mStrPartyType).Trim().Equals(string.Empty))
                CmbPartyType.SelectedIndex = 1;
            else
                CmbPartyType.SelectedItem = mStrPartyType;

            Clear();

        }

        public void Clear()
        {
            // comment by vipul
            //xtraTabPage6.PageVisible = false;
            txtAccountType.Text = string.Empty;
            txtAccountType.Tag = string.Empty;
            txtMPIN.Text = string.Empty;
            txtBrokername.Text = string.Empty;
            mStrStatus = string.Empty;
            txtLedgerID.Text = string.Empty;
            //txtLedgerCode.Text = string.Empty;

            txtLedgerCode.Text = Val.ToString(ObjMast.FindMaxLedgerCode());
            txtLedgerCode.Enabled = false;

            ChangeTextBoxPlaceHolder(txtLedgerName, txtLedgerName.Text, "NAME", Color.Silver);
            //txtLedgerName.Text = string.Empty;
            txtLedgerName.Tag = string.Empty;

            txtMobileNo1.Text = string.Empty;
            txtMobileNo2.Text = string.Empty;
            txtLandlineNo.Text = string.Empty;

            txtDepartment.Text = string.Empty;
            txtDepartment.Tag = string.Empty;
            txtDesignation.Text = string.Empty;
            txtDesignation.Tag = string.Empty;
            txtCompany.Text = string.Empty;

            txtWeChatID.Text = string.Empty;
            txtEmailID.Text = string.Empty;
            TxtSalerEmailID.Text = string.Empty;
            txtQQID.Text = string.Empty;
            txtSkypeID.Text = string.Empty;
            txtWebsite.Text = string.Empty;
            txtPanNo.Text = string.Empty;
            txtGSTNo.Text = string.Empty;

            txtFindByWhomName.Text = string.Empty;
            txtFindByWhomName.Tag = string.Empty;
            CmbFindType.SelectedIndex = 0;
            txtBroker.Tag = string.Empty;
            txtBroker.Text = string.Empty;
            ChkIsNoBroker.Checked = false;

            DtpAnniversary.Text = DateTime.Now.ToString();
            DtpBirthDate.Text = DateTime.Now.ToString();
            DTPDateOfJoin.Text = DateTime.Now.ToString();
            DtpDateOfLeave.Text = DateTime.Now.ToString();

            DtpAnniversary.Checked = false;
            DtpBirthDate.Checked = false;
            DTPDateOfJoin.Checked = false;
            DtpDateOfLeave.Checked = false;

            txtAccCode.Text = string.Empty;


            xtraTabControl2.SelectedTabPageIndex = 0;

            

            txtAccountName.Text = string.Empty;
            txtAccountNo.Text = string.Empty;
            txtBarnch.Text = string.Empty;
            txtIFSCCode.Text = string.Empty;
            TxtSwiftCode.Text = string.Empty;

            txtOpeningCrUSD.Text = "0.00";
            txtOpeningDrUSD.Text = "0.00";
            txtOpeningDrInr.Text = "0.00";
            txtOpeningCrINR.Text = "0.00";
            txtCurrency.Text = string.Empty;
            txtCurrency.Tag = string.Empty;
            txtExcRate.Text = string.Empty;

            ChangeTextBoxPlaceHolder(txtSAddress1, txtSAddress1.Text, "ADDRESS 1", Color.Silver);
            ChangeTextBoxPlaceHolder(txtSAddress2, txtSAddress2.Text, "ADDRESS 2", Color.Silver);
            ChangeTextBoxPlaceHolder(txtSAddress3, txtSAddress3.Text, "ADDRESS 3", Color.Silver);
            txtSState.Text = string.Empty;
            txtSCountry.Text = string.Empty;
            txtSCountry.Tag = string.Empty;
            txtSCity.Text = string.Empty;
            txtSZipCode.Text = string.Empty;

            txtDefaultSeller.Text = string.Empty;
            txtDefaultSeller.Tag = string.Empty;

            ChkAllowWebLogin.Checked = false;
            ChkAllowHoldAccess.Checked = false;
            ChkAllowReleaseAccess.Checked = false;

            ChkAllowDefDiscDiff.Checked = false;
            txtDefDiscDiff.Text = "0.00";
            //#K 04-11-2020
            txtBrokeragePer.Text = "0.00";

            ChkAllowMemberDisc.Checked = false;
            txtMemberDisc.Text = "0.00";

            txtRemark.Text = string.Empty;

            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;

            txtBroker.Text = String.Empty;
            ChkIsNoBroker.Checked = false;

            if (DtpBirthDate.Checked == true)
            {
                DtpBirthDate.Value = DateTime.Now;
            }

            if (DtpAnniversary.Checked == true)
            {
                DtpAnniversary.Value = DateTime.Now;
            }

            if (DTPDateOfJoin.Checked == true)
            {
                DTPDateOfJoin.Checked = true;
                DTPDateOfJoin.Value = DateTime.Now;
            }

            if (DtpDateOfLeave.Checked == true)
            {
                DtpDateOfLeave.Value = DateTime.Now;
            }

            txtFaxno.Text = string.Empty;
            txtBusinessType.Text = string.Empty;
            txtPassportNo.Text = string.Empty;
            txtSocialSecurityNo.Text = string.Empty;
            if (DtpBusinessEsstaDate.Checked == true)
            {
                DtpBusinessEsstaDate.Checked = true;
                DtpBusinessEsstaDate.Value = DateTime.Now;
            }
            else
                DtpBusinessEsstaDate.Checked = false;

            txtBusinessRegNo.Text = string.Empty;
            txtOrganizationType.Text = string.Empty;
            txtQbcNO.Text = string.Empty;

            txtReference.Text = string.Empty;
            txtSaleLimit.Text = string.Empty;

            ChkISOtherStockDiscDiff.Checked = false;
            txtOtherStockDiscDiff.Text = "0.00";

            CmbPartyType.Focus();

            txtADcode.Text = string.Empty;
            txtContactPer.Text = string.Empty;
            txtInterMediateBankDetail.Text = string.Empty;

            txtLocation_ID.Tag = string.Empty;
            txtLocation_ID.Text = string.Empty;

            CmbGender.SelectedIndex = 0;
            FillLedgerDetailInfo(Val.ToString(txtLedgerID.Text));
            FillBankDetail();
            FillPartnerDetail();

            ChkAllowWebAPI.Checked = false;
            //Kuldeep Added Jamesallen price
            ChkSyncJamesAllen.Checked = false;
            txtPreCarriBy.Text = string.Empty;
            txtVessetFlight.Text = string.Empty;
            txtPortofLoding.Text = string.Empty;
            txtFinalDest.Text = string.Empty;
            txtPlaceofRec.Text = string.Empty;
            txtPortofDischarge.Text = string.Empty;
            txtLegalName.Text = string.Empty;
            txtAadharCardNo.Text = string.Empty;
            CmbKYCType.Text = string.Empty;


            //CmbPartyTypeBYR
            txtCompanyBYR.Text = string.Empty;
            txtMobileNo1BYR.Text = string.Empty;
            txtMobileNo2BYR.Text = string.Empty;
            txtLandlineNoBYR.Text = string.Empty;
            CmbFindTypeBYR.SelectedIndex = 0;
            txtFindByWhomNameBYR.Text = string.Empty;
            txtFindByWhomNameBYR.Tag = string.Empty;
            txtContactPerBYR.Text = string.Empty;
            ChkAllowWebAPIBYR.Checked = false;
            ChkIsNoBrokerBYR.Checked = false;
            txtDefaultSellerBYR.Text = string.Empty;
            txtSAddress1BYR.Text = string.Empty;
            txtSAddress2BYR.Text = string.Empty;
            txtSAddress3BYR.Text = string.Empty;
            txtSCountryBYR.Text = string.Empty;
            txtSCityBYR.Text = string.Empty;
            txtSStateBYR.Text = string.Empty;
            txtSZipCodeBYR.Text = string.Empty;
            txtEmailIDBYR.Text = string.Empty;
            CmbStatusBYR.SelectedIndex = 0;
            txtRemarkBYR.Text = string.Empty;
            if (DtpBirthDateBYR.Checked == true)
            {
                DtpBirthDateBYR.Value = DateTime.Now;
            }
            if (DtpAnniversaryBYR.Checked == true)
            {
                DtpAnniversaryBYR.Value = DateTime.Now;
            }
            if (DTPDateOfJoinBYR.Checked == true)
            {
                DTPDateOfJoinBYR.Checked = true;
                DTPDateOfJoinBYR.Value = DateTime.Now;
            }
            if (DtpDateOfLeaveBYR.Checked == true)
            {
                DtpDateOfLeaveBYR.Value = DateTime.Now;
            }
            ChkAllowWebLoginBYR.Checked = false;
            txtUserNameBYR.Text = string.Empty;
            txtPasswordBYR.Text = string.Empty;
            CmbKycTypeBYR.SelectedItem = "BUYERKYC";
            mStrKYCType = string.Empty;
            txtBrokerBYR.Text = string.Empty;
            txtQQIDBYR.Text = string.Empty;
            txtWeChatIDBYR.Text = string.Empty;
            txtSkypeIDBYR.Text = string.Empty;

            ChkIsTdsLimit.Checked = false;

            ChkMemberPricePerCarat.Checked = false;
            txtMemberPricePerCarat.Text = string.Empty;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValSave() == false)
                {
                    return;
                }
                this.Cursor = Cursors.WaitCursor;

                LedgerMasterProperty Property = new LedgerMasterProperty();

                Property.LEDGER_ID = Val.ToString(txtLedgerID.Text).Trim().Equals(string.Empty) ? BusLib.Configuration.BOConfiguration.FindNewSequentialID() : Guid.Parse(Val.ToString(txtLedgerID.Text));

                Property.LEDGERCODE = Val.ToInt32(txtLedgerCode.Text);
                Property.LEDGERNAME = Val.ToString(txtLedgerName.Text);

                if (mStrKYCType == "BILLINGKYC" || mStrKYCType == "")
                {
                    Property.LEDGERTYPE = Val.ToString(CmbPartyType.SelectedItem);

                    Property.CONTACTPER = Val.ToString(txtContactPer.Text);
                    Property.COMPANYNAME = Val.ToString(txtCompany.Text);

                    Property.EMAILID = Val.ToString(txtEmailID.Text.TrimEnd());
                    Property.SALLEREMAIL_ID = Val.ToString(TxtSalerEmailID.Text.TrimEnd()); // K : 22/12/2022
                    Property.QQID = Val.ToString(txtQQID.Text);
                    Property.WECHATID = Val.ToString(txtWeChatID.Text);

                    Property.SKYPEID = Val.ToString(txtSkypeID.Text);
                    Property.WEBSITE = Val.ToString(txtWebsite.Text);
                    Property.GENDER = Val.ToString(CmbGender.SelectedItem);

                    Property.DEPARTMENT_ID = Val.ToInt32(txtDepartment.Tag);
                    Property.DESIGNATION_ID = Val.ToInt32(txtDesignation.Tag);

                  //  Property.BROKERNAME = Val.ToString(txtBrokername.Text);
                    Property.BROKER_ID = Val.ToGuid(txtBrokername.Tag);
                  //  .Trim().Equals(string.ToString) ? Guid.Empty : Guid.Parse(txtBrokername.Tag.ToString());//add by urvisha  

                    Property.SHIPPINGADDRESS1 = txtSAddress1.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtSAddress1.Text);
                    Property.SHIPPINGADDRESS2 = txtSAddress2.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtSAddress2.Text);
                    Property.SHIPPINGADDRESS3 = txtSAddress3.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtSAddress3.Text);
                    Property.SHIPPINGSTATE = Val.ToString(txtSState.Text);
                    Property.SHIPPINGCOUNTRY_ID = Val.ToInt32(txtSCountry.Tag);
                    Property.SHIPPINGCITY = Val.ToString(txtSCity.Text);
                    Property.SHIPPINGZIPCODE = Val.ToString(txtSZipCode.Text);

                    Property.BILLINGADDRESS1 = txtSAddress1.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtSAddress1.Text);
                    Property.BILLINGADDRESS2 = txtSAddress2.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtSAddress2.Text);
                    Property.BILLINGADDRESS3 = txtSAddress3.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtSAddress3.Text);
                    Property.BILLINGSTATE = Val.ToString(txtSState.Text);
                    Property.BILLINGCOUNTRY_ID = Val.ToInt32(txtSCountry.Tag);
                    Property.BILLINGCITY = Val.ToString(txtSCity.Text);
                    Property.BILLINGZIPCODE = Val.ToString(txtSZipCode.Text);

                    Property.MOBILENO1 = txtMobileNo1.Text;
                    Property.MOBILENO2 = txtMobileNo2.Text;
                    Property.LANDLINENO = txtLandlineNo.Text;

                    Property.ISALLOWWEBLOGIN = Val.ToBoolean(ChkAllowWebLogin.Checked);

                    Property.USERNAME = txtUserName.Text;
                    Property.PASSWORD = txtPassword.Text;
                    Property.MPIN = txtMPIN.Text;
                    

                    Property.ISDEFAULTDISCOUNTDIFF = Val.ToBoolean(ChkAllowDefDiscDiff.Checked);
                    Property.DEFAULTDISCOUNTDIFF = Val.Val(txtDefDiscDiff.Text);
                    //#K 04-11-2020
                    Property.BROKERAGEPER = Val.Val(txtBrokeragePer.Text);

                    Property.ISMEMBERDISCOUNT = Val.ToBoolean(ChkAllowMemberDisc.Checked);
                    Property.MEMBERDISCOUNT = Val.Val(txtMemberDisc.Text);

                    Property.ISMEMBERPRICEPERCARAT = Val.ToBoolean(ChkMemberPricePerCarat.Checked);
                    Property.MEMBEPRICEPERCARAT = Val.Val(txtMemberPricePerCarat.Text);

                    Property.ISOTHERSTOCKDISCDIFF = Val.ToBoolean(ChkISOtherStockDiscDiff.Checked); //#P : 23-11-2019
                    Property.OTHERSTOCKDISCOUNTDIFF = Val.Val(txtOtherStockDiscDiff.Text); //#P : 23-11-2019

                    Property.DEFAULTSELLER_ID = Val.ToString(txtDefaultSeller.Tag).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtDefaultSeller.Tag));

                    Property.STATUS = Val.ToString(CmbStatus.SelectedItem);
                    Property.REMARK = Val.ToString(txtRemark.Text);
                    //ADD BHAGYASHREE 03/08/2019
                    Property.ACCTTYPE_ID = Val.ToInt32(txtAccountType.Tag);
                    Property.CURRENCY_ID = Val.ToString(txtCurrency.Text).Trim().Equals(string.Empty) ? 0 : Val.ToInt32(txtCurrency.Tag);
                    Property.EXCRATE = Val.Val(txtExcRate.Text);

                    Property.OPENINGCREDITUSD = Val.Val(txtOpeningCrUSD.Text);
                    Property.OPENINGDEBITUSD = Val.Val(txtOpeningDrUSD.Text);

                    Property.OPENINGCREDITFE = Val.Val(txtOpeningCrINR.Text);
                    Property.OPENINGDEBITFE = Val.Val(txtOpeningDrInr.Text);

                    Property.GSTNO = txtGSTNo.Text;
                    Property.PANNO = txtPanNo.Text;

                    Property.FINDBYWHOM_ID = Val.ToString(txtFindByWhomName.Text).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(txtFindByWhomName.Tag.ToString());
                    Property.PARTYFINDTYPE = Val.ToString(CmbFindType.Text);
                 

                    if (DtpBirthDate.Checked == true) // comment condition wise code and add common code for all date . by khushbu 10-12-21
                    {
                        Property.DATEOFBIRTH = Val.SqlDate(DtpBirthDate.Value.ToShortDateString());
                    }
                    else
                    {
                        Property.DATEOFBIRTH = null;
                    }
                    if (DtpAnniversary.Checked == true)
                    {
                        Property.DATEOFANNIVERSARY = Val.SqlDate(DtpAnniversary.Value.ToShortDateString());
                    }
                    else
                    {
                        Property.DATEOFANNIVERSARY = null;
                    }
                    if (DTPDateOfJoin.Checked == true)
                    {
                        Property.DATEOFJOIN = Val.SqlDate(DTPDateOfJoin.Value.ToShortDateString());
                    }
                    else
                    {
                        Property.DATEOFJOIN = null;
                    }
                    if (DtpDateOfLeave.Checked == true && Val.ToString(CmbStatus.SelectedItem) == "DEACTIVE")
                    {
                        Property.DATEOFLEAVE = Val.SqlDate(DtpDateOfLeave.Value.ToShortDateString());
                    }
                    else
                    {
                        Property.DATEOFLEAVE = null;
                    }



                    if (Val.ToString(txtBroker.Text).Trim().Equals(string.Empty) && ChkIsNoBroker.Checked == true)
                    {
                        Property.COORDINATOR_ID = Val.ToString(txtBroker.Text).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(txtBroker.Tag.ToString());
                    }
                    else
                    {
                        Property.COORDINATOR_ID = Val.ToString(txtBroker.Text).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(txtBroker.Tag.ToString());
                    }
                    Property.ISNOBROKER = Val.ToBoolean(ChkIsNoBroker.Checked);
                    Property.REFFERENCE = Val.ToString(txtReference.Text);
                    Property.SALELIMIT = Val.Val(txtSaleLimit.Text);
                    Property.BANKACCOUNTNAME = Val.ToString(txtAccountName.Text);
                    Property.BANKACCOUNTNO = Val.ToString(txtAccountNo.Text);
                    Property.BANKNAME = Val.ToString(txtBarnch.Text);
                    Property.BRANCHNAME = Val.ToString(txtBarnch.Text);
                    Property.IFSCCODE = Val.ToString(txtIFSCCode.Text);
                    Property.SWIFTCODE = Val.ToString(TxtSwiftCode.Text);
                    Property.ISALLOWWEBAPI = Val.ToBoolean(ChkAllowWebAPI.Checked);

                    //Property.OPENINGCREDIT = Val.Val(TxtOpeningCredit.Text);
                    //Property.OPENINGDEBIT = Val.Val(txtOpeningDebit.Text);

                    Property.ADCODE = Val.ToString(txtADcode.Text);
                    Property.INTERMEDIATEBANKDETAIL = Val.ToString(txtInterMediateBankDetail.Text);
                    // K ; 01-11-2020 For JameAllenPrice
                    Property.ISSYNCJAMESALLEN = Val.ToBoolean(ChkSyncJamesAllen.Checked);
                    Property.EMPPHOTO = null;

                    Property.FAXNO = Val.ToString(txtFaxno.Text);
                    Property.TYPEOFBUSINESS = Val.ToString(txtBusinessType.Text);
                    Property.PASSPORTNO = Val.ToString(txtPassportNo.Text);
                    Property.SOCIALSECURITYNO = Val.ToString(txtSocialSecurityNo.Text);
                    Property.NATUREOFBUSINESS = Val.ToString(txtNatureOfBuSiness.Text);

                    if (DtpBusinessEsstaDate.Checked == true)
                    {
                        Property.DATEOFBUSINESSESTABLISMENT = Val.SqlDate(DtpBusinessEsstaDate.Value.ToShortDateString());
                    }
                    else
                    {
                        Property.DATEOFBUSINESSESTABLISMENT = null;
                    }

                    Property.BUSINESSREGISTRATIONNO = Val.ToString(txtBusinessRegNo.Text);
                    Property.ORGANIZATIONNO = Val.ToString(txtOrganizationType.Text);
                    Property.QBCNO = Val.ToString(txtQbcNO.Text);

                    Property.LOCATION_ID = Val.ToInt32(txtLocation_ID.Tag);

                    Property.ACCCODE = Val.ToString(txtAccCode.Text);

                    //add shiv 17-05-2022
                    Property.PRECARRIBY = Val.ToString(txtPreCarriBy.Text);
                    Property.VESSETFLIGHT = Val.ToString(txtVessetFlight.Text);
                    Property.PORTOFLODING = Val.ToString(txtPortofLoding.Text);
                    Property.FINALDEST = Val.ToString(txtFinalDest.Text);
                    Property.PLACEOFREC = Val.ToString(txtPlaceofRec.Text);
                    Property.PORTOFDISCHARGE = Val.ToString(txtPortofDischarge.Text);

                    //add shiv 21-05-2022
                    Property.PARTYDEC = Val.ToString(txtPartyDeclaration.Text);
                    Property.COMPDEC = Val.ToString(txtCompDecl.Text);
                    Property.LEGALNAME = Val.ToString(txtLegalName.Text);
                    mStrKYCType = CmbKYCType.Text;
                    Property.KYCTYPE = Val.ToString(mStrKYCType);
                    Property.AADHARCARDNO = Val.ToString(txtAadharCardNo.Text);
                }
                else if (mStrKYCType == "BUYERKYC")
                {
                    Property.LEDGERTYPE = Val.ToString(CmbPartyTypeBYR.SelectedItem);
                    Property.CONTACTPER = Val.ToString(txtContactPerBYR.Text);
                    Property.COMPANYNAME = Val.ToString(txtCompanyBYR.Text);
                    Property.EMAILID = Val.ToString(txtEmailIDBYR.Text.TrimEnd());
                    Property.SHIPPINGADDRESS1 = txtSAddress1BYR.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtSAddress1BYR.Text);
                    Property.SHIPPINGADDRESS2 = txtSAddress2BYR.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtSAddress2BYR.Text);
                    Property.SHIPPINGADDRESS3 = txtSAddress3BYR.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtSAddress3BYR.Text);
                    Property.SHIPPINGSTATE = Val.ToString(txtSStateBYR.Text);
                    Property.SHIPPINGCOUNTRY_ID = Val.ToInt32(txtSCountryBYR.Tag);
                    Property.SHIPPINGCITY = Val.ToString(txtSCityBYR.Text);
                    Property.SHIPPINGZIPCODE = Val.ToString(txtSZipCodeBYR.Text);
                    Property.MOBILENO1 = txtMobileNo1BYR.Text;
                    Property.MOBILENO2 = txtMobileNo2BYR.Text;
                    Property.LANDLINENO = txtLandlineNoBYR.Text;
                    Property.ISALLOWWEBLOGIN = Val.ToBoolean(ChkAllowWebLoginBYR.Checked);
                    Property.USERNAME = txtUserNameBYR.Text;
                    Property.PASSWORD = txtPasswordBYR.Text;
                    Property.DEFAULTSELLER_ID = Val.ToString(txtDefaultSellerBYR.Tag).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtDefaultSellerBYR.Tag));
                    Property.STATUS = Val.ToString(CmbStatusBYR.SelectedItem);
                    Property.REMARK = Val.ToString(txtRemarkBYR.Text);
                    Property.FINDBYWHOM_ID = Val.ToString(txtFindByWhomNameBYR.Text).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(txtFindByWhomNameBYR.Tag.ToString());
                    Property.PARTYFINDTYPE = Val.ToString(CmbFindTypeBYR.Text);
                    Property.ISNOBROKER = Val.ToBoolean(ChkIsNoBrokerBYR.Checked);
                    Property.ISALLOWWEBAPI = Val.ToBoolean(ChkAllowWebAPIBYR.Checked);
                    if (DtpBirthDateBYR.Checked == true) // comment condition wise code and add common code for all date . by khushbu 10-12-21
                    {
                        Property.DATEOFBIRTH = Val.SqlDate(DtpBirthDateBYR.Value.ToShortDateString());
                    }
                    else
                    {
                        Property.DATEOFBIRTH = null;
                    }
                    if (DtpAnniversaryBYR.Checked == true)
                    {
                        Property.DATEOFANNIVERSARY = Val.SqlDate(DtpAnniversaryBYR.Value.ToShortDateString());
                    }
                    else
                    {
                        Property.DATEOFANNIVERSARY = null;
                    }
                    if (DTPDateOfJoinBYR.Checked == true)
                    {
                        Property.DATEOFJOIN = Val.SqlDate(DTPDateOfJoinBYR.Value.ToShortDateString());
                    }
                    else
                    {
                        Property.DATEOFJOIN = null;
                    }
                    if (DtpDateOfLeaveBYR.Checked == true && Val.ToString(CmbStatusBYR.SelectedItem) == "DEACTIVE")
                    {
                        Property.DATEOFLEAVE = Val.SqlDate(DtpDateOfLeaveBYR.Value.ToShortDateString());
                    }
                    else
                    {
                        Property.DATEOFLEAVE = null;
                    }
                    Property.ACCCODE = Val.ToString(txtAccCode.Text);
                    Property.COMPDEC = Val.ToString(txtCompDecl.Text);
                    Property.LEGALNAME = Val.ToString(txtLegalName.Text);
                    mStrKYCType = CmbKycTypeBYR.Text;
                    Property.KYCTYPE = Val.ToString(mStrKYCType);
                    if (Val.ToString(txtBrokerBYR.Text).Trim().Equals(string.Empty) && ChkIsNoBrokerBYR.Checked == true)
                    {
                        Property.COORDINATOR_ID = Val.ToString(txtBrokerBYR.Text).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(txtBrokerBYR.Tag.ToString());
                    }
                    else
                    {
                        Property.COORDINATOR_ID = Val.ToString(txtBrokerBYR.Text).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(txtBrokerBYR.Tag.ToString());
                    }
                    Property.QQID = Val.ToString(txtQQIDBYR.Text);
                    Property.WECHATID = Val.ToString(txtWeChatIDBYR.Text);
                    Property.SKYPEID = Val.ToString(txtSkypeIDBYR.Text);
                    Property.WEBSITE = Val.ToString(txtWebsite.Text);
                    Property.GENDER = Val.ToString(CmbGender.SelectedItem);
                    Property.DEPARTMENT_ID = Val.ToInt32(txtDepartment.Tag);
                    Property.DESIGNATION_ID = Val.ToInt32(txtDesignation.Tag);
                    Property.BILLINGADDRESS1 = txtSAddress1.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtSAddress1.Text);
                    Property.BILLINGADDRESS2 = txtSAddress2.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtSAddress2.Text);
                    Property.BILLINGADDRESS3 = txtSAddress3.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtSAddress3.Text);
                    Property.BILLINGSTATE = Val.ToString(txtSState.Text);
                    Property.BILLINGCOUNTRY_ID = Val.ToInt32(txtSCountry.Tag);
                    Property.BILLINGCITY = Val.ToString(txtSCity.Text);
                    Property.BILLINGZIPCODE = Val.ToString(txtSZipCode.Text);
                    Property.ISDEFAULTDISCOUNTDIFF = Val.ToBoolean(ChkAllowDefDiscDiff.Checked);
                    Property.DEFAULTDISCOUNTDIFF = Val.Val(txtDefDiscDiff.Text);                    //#K 04-11-2020
                    Property.BROKERAGEPER = Val.Val(txtBrokeragePer.Text);
                    Property.ISMEMBERDISCOUNT = Val.ToBoolean(ChkAllowMemberDisc.Checked);
                    Property.MEMBERDISCOUNT = Val.Val(txtMemberDisc.Text);
                    Property.ISOTHERSTOCKDISCDIFF = Val.ToBoolean(ChkISOtherStockDiscDiff.Checked); //#P : 23-11-2019
                    Property.OTHERSTOCKDISCOUNTDIFF = Val.Val(txtOtherStockDiscDiff.Text); //#P : 23-11-2019
                    Property.ACCTTYPE_ID = Val.ToInt32(txtAccountType.Tag);
                    Property.CURRENCY_ID = Val.ToString(txtCurrency.Text).Trim().Equals(string.Empty) ? 0 : Val.ToInt32(txtCurrency.Tag);
                    Property.EXCRATE = Val.Val(txtExcRate.Text);
                    Property.OPENINGCREDITUSD = Val.Val(txtOpeningCrUSD.Text);
                    Property.OPENINGDEBITUSD = Val.Val(txtOpeningDrUSD.Text);
                    Property.OPENINGCREDITFE = Val.Val(txtOpeningCrINR.Text);
                    Property.OPENINGDEBITFE = Val.Val(txtOpeningDrInr.Text);
                    Property.GSTNO = txtGSTNo.Text;
                    Property.PANNO = txtPanNo.Text;

                    Property.REFFERENCE = Val.ToString(txtReference.Text);
                    Property.SALELIMIT = Val.Val(txtSaleLimit.Text);
                    Property.BANKACCOUNTNAME = Val.ToString(txtAccountName.Text);
                    Property.BANKACCOUNTNO = Val.ToString(txtAccountNo.Text);
                    Property.BANKNAME = Val.ToString(txtBarnch.Text);
                    Property.BRANCHNAME = Val.ToString(txtBarnch.Text);
                    Property.IFSCCODE = Val.ToString(txtIFSCCode.Text);
                    Property.SWIFTCODE = Val.ToString(TxtSwiftCode.Text);
                    Property.INTERMEDIATEBANKDETAIL = Val.ToString(txtInterMediateBankDetail.Text);
                    Property.ISSYNCJAMESALLEN = Val.ToBoolean(ChkSyncJamesAllen.Checked);
                    Property.EMPPHOTO = null;
                    Property.FAXNO = Val.ToString(txtFaxno.Text);
                    Property.TYPEOFBUSINESS = Val.ToString(txtBusinessType.Text);
                    Property.PASSPORTNO = Val.ToString(txtPassportNo.Text);
                    Property.SOCIALSECURITYNO = Val.ToString(txtSocialSecurityNo.Text);
                    Property.NATUREOFBUSINESS = Val.ToString(txtNatureOfBuSiness.Text);
                    Property.ADCODE = Val.ToString(txtADcode.Text);
                    if (DtpBusinessEsstaDate.Checked == true)
                    {
                        Property.DATEOFBUSINESSESTABLISMENT = Val.SqlDate(DtpBusinessEsstaDate.Value.ToShortDateString());
                    }
                    else
                    {
                        Property.DATEOFBUSINESSESTABLISMENT = null;
                    }
                    Property.BUSINESSREGISTRATIONNO = Val.ToString(txtBusinessRegNo.Text);
                    Property.ORGANIZATIONNO = Val.ToString(txtOrganizationType.Text);
                    Property.QBCNO = Val.ToString(txtQbcNO.Text);
                    Property.LOCATION_ID = Val.ToInt32(txtLocation_ID.Tag);
                    Property.PRECARRIBY = Val.ToString(txtPreCarriBy.Text);
                    Property.VESSETFLIGHT = Val.ToString(txtVessetFlight.Text);
                    Property.PORTOFLODING = Val.ToString(txtPortofLoding.Text);
                    Property.FINALDEST = Val.ToString(txtFinalDest.Text);
                    Property.PLACEOFREC = Val.ToString(txtPlaceofRec.Text);
                    Property.PORTOFDISCHARGE = Val.ToString(txtPortofDischarge.Text);
                    Property.PARTYDEC = Val.ToString(txtPartyDeclaration.Text);
                    Property.AADHARCARDNO = Val.ToString(txtAadharCardNo.Text);
                }

                //ADD SHIV 14-09-2022
                Property.Sec_Address = Val.ToString(txtSecondAdd.Text);
                Property.IS_SecAddress = Val.ToBoolean(ChkSecondAddress.Checked);

                //ADD SHIV 20-09-2022
                Property.IS_TDSLIMIT = Val.ToBoolean(ChkIsTdsLimit.Checked);
                Property.Sec_Address1 = Val.ToString(txtSecondAdd1.Text);
                Property.Sec_Address2 = Val.ToString(txtSecondAdd2.Text);

                Property.IsAllowHoldAccess = ChkAllowHoldAccess.Checked;
                Property.IsAllowReleaseAccess = ChkAllowReleaseAccess.Checked;

                Property = ObjMast.Save(Property);

                string StrReturnDesc = Property.ReturnMessageDesc;
                if (Property.ReturnMessageType == "SUCCESS")
                {
                    if (txtLedgerID.Text == "")
                    {
                        ObjMast.UpdateLedgerCode(Val.ToInt(txtLedgerCode.Text));
                    }

                    if (mStrStatus != "" && mStrStatus != Val.ToString(CmbStatus.SelectedItem))
                    {
                        BtnEmail_Click(null, null);
                    }

                    foreach (DataRow DRow in DtabPartnerDetail.Rows)
                    {
                        if (Val.ToString(DRow["PARTNERNAME"]).Trim().Equals(string.Empty))
                        {

                            continue;
                        }
                        else
                        {

                            Property.PARTNER_ID = Val.ToString(DRow["PARTNER_ID"]).Trim().Equals(string.Empty) ? BusLib.Configuration.BOConfiguration.FindNewSequentialID() : Guid.Parse(Val.ToString(DRow["PARTNER_ID"]));
                            Property.LEDGER_ID = Val.ToString(txtLedgerID.Text).Trim().Equals(string.Empty) ? BusLib.Configuration.BOConfiguration.FindNewSequentialID() : Guid.Parse(Val.ToString(txtLedgerID.Text));
                            Property.PARTNERNAME = Val.ToString(DRow["PARTNERNAME"]);
                            Property.PARTNERMOBILENO = Val.ToString(DRow["MOBILENO"]);
                            Property.PARTNEREMAIL_ID = Val.ToString(DRow["EMAIL_ID"]);
                            Property.NATIVEPLACE = Val.ToString(DRow["NATIVEPLACE"]);
                            Property.DISTRICT = Val.ToString(DRow["DISTRICT"]);
                            Property.SAMAJ = Val.ToString(DRow["SAMAJ"]);

                            Property = ObjMast.SavePartnerDetail(Property);

                        }
                    }
                    DtabPartnerDetail.AcceptChanges();
                    MainGrdPartnerDetail.Refresh();

                    foreach (DataRow Dr in DtabBank.Rows)
                    {
                        if (Val.ToString(Dr["BANKNAME"]).Trim().Equals(string.Empty)) //|| Val.ToString(Dr["BANKACCOUNTNO"]).Trim().Equals(string.Empty)
                        {

                            this.Cursor = Cursors.Default;
                            Global.Message(StrReturnDesc);

                            if (Property.ReturnMessageType == "SUCCESS")
                            {
                                BtnAdd_Click(null, null);
                            }
                            else
                            {
                                txtLedgerName.Focus();
                            }
                            return;
                        }
                        else
                        {

                            Property.BANK_ID = Val.ToString(Dr["BANK_ID"]).Trim().Equals(string.Empty) ? BusLib.Configuration.BOConfiguration.FindNewSequentialID() : Guid.Parse(Val.ToString(Dr["BANK_ID"]));
                            Property.LEDGER_ID = Val.ToString(txtLedgerID.Text).Trim().Equals(string.Empty) ? BusLib.Configuration.BOConfiguration.FindNewSequentialID() : Guid.Parse(Val.ToString(txtLedgerID.Text));
                            Property.BANKNAME = Val.ToString(Dr["BANKNAME"]);
                            Property.BANKACCOUNTNO = Val.ToString(Dr["BANKACCOUNTNO"]);
                            Property.IFSCCODE = Val.ToString(Dr["IFSCCODE"]);
                            Property.SWIFTCODE = Val.ToString(Dr["SWIFTCODE"]);
                            Property.BRANCHNAME = Val.ToString(Dr["BRANCHNAME"]);
                            Property.ADDRESS = Val.ToString(Dr["ADDRESS"]);
                            //add shiv
                            Property.INTERMEDIARYBANK = Val.ToString(Dr["INTERMEDIARYBANK"]);
                            Property = ObjMast.SaveBank(Property);

                        }
                    }

                    DtabBank.AcceptChanges();
                    MainGrdBank.Refresh();


                }
                this.Cursor = Cursors.Default;
                Global.Message(StrReturnDesc);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    BtnAdd_Click(null, null);
                }
                else
                {
                    txtLedgerName.Focus();
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            LedgerMasterProperty Property = new LedgerMasterProperty();
            try
            {

                if (Global.Confirm("Are Your Sure To Delete The Record ?") == System.Windows.Forms.DialogResult.No)
                    return;

                FrmPassword FrmPassword = new FrmPassword();
                if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
                {

                    //Property.LEDGER_ID = Val.ToInt64(txtLedgerID.Text);
                    Property.LEDGER_ID = Guid.Parse(Val.ToString(txtLedgerID.Text));
                    Property = ObjMast.Delete(Property);
                    Global.Message(Property.ReturnMessageDesc);

                    if (Property.ReturnMessageType == "SUCCESS")
                    {
                        BtnAdd_Click(null, null);
                    }
                    else
                    {
                        txtLedgerName.Focus();
                    }
                }
            }
            catch (System.Exception ex)
            {
                Global.MessageToster(ex.Message);
            }
            Property = null;
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FrmLedger_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Escape)
            //{
            //    if (Global.Confirm("Do You Want To Close The Form?") == System.Windows.Forms.DialogResult.Yes)
            //        BtnBack_Click(null, null);
            //}
        }

        public void ChangeTextBoxPlaceHolder(TextBox TxtBox, string StrCheckText, string StrChangeText, Color ColTextColor)
        {
            if (Val.ToString(TxtBox.Text) == StrCheckText)
            {
                TxtBox.Text = StrChangeText;
                TxtBox.ForeColor = ColTextColor;
            }
            if (TxtBox.Text == "")
            {
                TxtBox.ForeColor = Color.Black;
            }
            //if (TxtBox.ForeColor == Color.Silver)
            //    TxtBox.TextAlign = HorizontalAlignment.Center;
            //else
            //    TxtBox.TextAlign = HorizontalAlignment.Left;
            TxtBox = null;
        }

        public void FetchValue(DataRow DR)
        {
            //xtraTabPage6.PageVisible = true;

            CmbKYCType.Text = Val.ToString(DR["KYCTYPE"]);
            mStrKYCType = CmbKYCType.Text;
            //GrpKYCTypeSelection.BringToFront();
            //GrpKYCTypeSelection.Visible = true;
            //GrpKYCTypeSelection.Enabled = true;
            //panel1.Enabled = false;
            //xtraTabControl2.Enabled = false;
            //panel3.Enabled = false;
            //CmbKYCType.Focus();

            //BILLING KYC
            CmbPartyType.SelectedItem = mStrPartyType;
            txtLedgerCode.Text = Val.ToString(DR["LEDGERCODE"]);

            if (!Val.ToString(DR["LEDGERNAME"]).Trim().Equals(string.Empty))
                ChangeTextBoxPlaceHolder(txtLedgerName, txtLedgerName.Text, Val.ToString(DR["LEDGERNAME"]), Color.Black);
            else
                ChangeTextBoxPlaceHolder(txtLedgerName, txtLedgerName.Text, "NAME", Color.Silver);

            //txtLedgerName.Text = Val.ToString(DR["LEDGERNAME"]);
            txtLedgerID.Text = Val.ToString(DR["LEDGER_ID"]);

            txtCompany.Text = Val.ToString(DR["COMPANYNAME"]);

            txtAccountType.Tag = Val.ToString(DR["ACCTTYPE_ID"]);
            txtAccountType.Text = Val.ToString(DR["ACCTTYPENAME"]);
            txtBrokername.Text = Val.ToString(DR["BROKERNAMENEW"]);//Add by urvisha 
            txtDepartment.Text = Val.ToString(DR["DEPARTMENTNAME"]);
            txtDepartment.Tag = Val.ToString(DR["DEPARTMENT_ID"]);

            txtDesignation.Text = Val.ToString(DR["DESIGNATIONNAME"]);
            txtDesignation.Tag = Val.ToString(DR["DESIGNATION_ID"]);

            txtMobileNo1.Text = Val.ToString(DR["MOBILENO1"]);
            txtMobileNo2.Text = Val.ToString(DR["MOBILENO2"]);
            txtLandlineNo.Text = Val.ToString(DR["LANDLINENO"]);


            if (!Val.ToString(DR["SHIPPINGADDRESS1"]).Trim().Equals(string.Empty))
                ChangeTextBoxPlaceHolder(txtSAddress1, txtSAddress1.Text, Val.ToString(DR["SHIPPINGADDRESS1"]), Color.Black);
            else
                ChangeTextBoxPlaceHolder(txtSAddress1, txtSAddress1.Text, "ADDRESS 1", Color.Silver);

            if (!Val.ToString(DR["SHIPPINGADDRESS2"]).Trim().Equals(string.Empty))
                ChangeTextBoxPlaceHolder(txtSAddress2, txtSAddress2.Text, Val.ToString(DR["SHIPPINGADDRESS2"]), Color.Black);
            else
                ChangeTextBoxPlaceHolder(txtSAddress2, txtSAddress2.Text, "ADDRESS 2", Color.Silver);

            if (!Val.ToString(DR["SHIPPINGADDRESS3"]).Trim().Equals(string.Empty))
                ChangeTextBoxPlaceHolder(txtSAddress3, txtSAddress3.Text, Val.ToString(DR["SHIPPINGADDRESS3"]), Color.Black);
            else
                ChangeTextBoxPlaceHolder(txtSAddress3, txtSAddress3.Text, "ADDRESS 3", Color.Silver);
            txtSState.Text = Val.ToString(DR["SHIPPINGSTATE"]);
            txtSCity.Text = Val.ToString(DR["SHIPPINGCITY"]);
            txtSCountry.Text = Val.ToString(DR["SHIPPINGCOUNTRYNAME"]);
            txtSCountry.Tag = Val.ToString(DR["SHIPPINGCOUNTRY_ID"]);
            txtSZipCode.Text = Val.ToString(DR["SHIPPINGZIPCODE"]);

            txtWeChatID.Text = Val.ToString(DR["WECHATID"]);
            txtEmailID.Text = Val.ToString(DR["EMAILID"]);
            TxtSalerEmailID.Text = Val.ToString(DR["SALLEREMAIL_ID"]);
            txtQQID.Text = Val.ToString(DR["QQID"]);
            txtSkypeID.Text = Val.ToString(DR["SKYPEID"]);
            txtWebsite.Text = Val.ToString(DR["WEBSITE"]);
            txtGSTNo.Text = Val.ToString(DR["GSTNO"]);
            txtPanNo.Text = Val.ToString(DR["PANNO"]);

            ChkAllowWebLogin.Checked = Val.ToBoolean(DR["ISALLOWWEBLOGIN"]);
            ChkAllowHoldAccess.Checked = Val.ToBoolean(DR["ISALLOWHOLDACCESS"]);
            ChkAllowReleaseAccess.Checked = Val.ToBoolean(DR["ISALLOWRELEASEACCESS"]);


            ChkAllowDefDiscDiff.Checked = Val.ToBoolean(DR["ISDEFAULTDISCOUNTDIFF"]);
            txtDefDiscDiff.Text = Val.ToString(DR["DEFAULTDISCOUNTDIFF"]);
            //#K: 04-11-2020
            txtBrokeragePer.Text = Val.ToString(DR["BROKERAGEPER"]);

            ChkAllowMemberDisc.Checked = Val.ToBoolean(DR["ISMEMBERDISCOUNT"]);
            txtMemberDisc.Text = Val.ToString(DR["MEMBERDISCOUNT"]);

            ChkMemberPricePerCarat.Checked = Val.ToBoolean(DR["ISMEMBERPRICEPERCARAT"]);
            txtMemberPricePerCarat.Text = Val.ToString(DR["MEMBERPRICEPERCARAT"]);

            txtDefaultSeller.Text = Val.ToString(DR["DEFAULTSELLERNAME"]);
            txtDefaultSeller.Tag = Val.ToString(DR["DEFAULTSELLER_ID"]);

            ChkISOtherStockDiscDiff.Checked = Val.ToBoolean(DR["ISOTHERSTOCKDISCDIFF"]);
            txtOtherStockDiscDiff.Text = Val.ToString(DR["OTHERSTOCKDISCDIFF"]);

            CmbStatus.SelectedItem = Val.ToString(DR["STATUS"]);
            mStrStatus = Val.ToString(DR["STATUS"]);

            CmbGender.SelectedItem = Val.ToString(DR["GENDER"]);

            txtRemark.Text = Val.ToString(DR["REMARK"]);

            txtUserName.Text = Val.ToString(DR["USERNAME"]);
            txtPassword.Text = Val.ToString(DR["PASSWORD"]);
            txtMPIN.Text = Val.ToString(DR["MPIN"]);

            txtFindByWhomName.Tag = Val.ToString(DR["FINDBYWHOM_ID"]);
            txtFindByWhomName.Text = Val.ToString(DR["FINDBYWHOMNAME"]);
            CmbFindType.Text = Val.ToString(DR["PARTYFINDTYPE"]);

            if (Val.ToString(DR["DATEOFBIRTH"]) != "")
            {
                DtpBirthDate.Checked = true;
                DtpBirthDate.Text = Val.ToString(DR["DATEOFBIRTH"]);
            }
            else
                DtpBirthDate.Checked = false;


            if (Val.ToString(DR["DATEOFANNIVERSARY"]) != "")
            {
                DtpAnniversary.Checked = true;
                DtpAnniversary.Text = Val.ToString(DR["DATEOFANNIVERSARY"]);
            }
            else
                DtpAnniversary.Checked = false;


            if (Val.ToString(DR["DATEOFJOIN"]) != "")
            {
                DTPDateOfJoin.Checked = true;
                DTPDateOfJoin.Text = Val.ToString(DR["DATEOFJOIN"]);
            }
            else
                DTPDateOfJoin.Checked = false;


            if (Val.ToString(DR["DATEOFLEAVE"]) != "")
            {
                DtpDateOfLeave.Checked = true;
                DtpDateOfLeave.Text = Val.ToString(DR["DATEOFLEAVE"]);
            }
            else
                DtpDateOfLeave.Checked = false;


            txtBroker.Tag = Val.ToString(DR["COORDINATOR_ID"]);
            txtBroker.Text = Val.ToString(DR["BROKERNAMENEW"]);
            ChkIsNoBroker.Checked = Val.ToBoolean(DR["ISNOBROKER"]);
            txtReference.Text = Val.ToString(DR["REFFERENCE"]);
            txtSaleLimit.Text = Val.ToString(DR["SALELIMIT"]);

            txtAccountNo.Text = Val.ToString(DR["BANKACCOUNTNO"]);
            txtAccountName.Text = Val.ToString(DR["BANKACCOUNTNAME"]);
            txtBarnch.Text = Val.ToString(DR["BANKBRANCHNAME"]);
            txtIFSCCode.Text = Val.ToString(DR["BANKIFSCCODE"]);
            TxtSwiftCode.Text = Val.ToString(DR["BANKSWIFTCODE"]);

            ChkAllowWebAPI.Checked = Val.ToBoolean(DR["ISALLOWWEBAPI"]);
            //K : 01-11-2020
            ChkSyncJamesAllen.Checked = Val.ToBoolean(DR["ISSYNCJAMESALLEN"]);


            txtCurrency.Tag = Val.ToString(DR["CURRENCY_ID"]);
            txtCurrency.Text = Val.ToString(DR["CURRENCYNAME"]);
            txtOpeningCrUSD.Text = Val.ToString(DR["OPENINGCREDITUSD"]);
            txtOpeningDrUSD.Text = Val.ToString(DR["OPENINGDEBITUSD"]);

            txtOpeningCrINR.Text = Val.ToString(DR["OPENINGCREDITFE"]);
            txtOpeningDrInr.Text = Val.ToString(DR["OPENINGDEBITFE"]);

            txtExcRate.Text = Val.ToString(DR["EXCRATE"]);

            txtADcode.Text = Val.ToString(DR["ADCODE"]);
            txtInterMediateBankDetail.Text = Val.ToString(DR["INTERMEDIATEBANKDETAIL"]);

            txtFaxno.Text = Val.ToString(DR["FAXNO"]);
            txtBusinessType.Text = Val.ToString(DR["TYPEOFBUSINESS"]);
            txtPassportNo.Text = Val.ToString(DR["PASSPORTNO"]);
            txtSocialSecurityNo.Text = Val.ToString(DR["SOCIALSECURITYNO"]);
            txtNatureOfBuSiness.Text = Val.ToString(DR["NATUREOFBUSINESS"]);
            if (Val.ToString(DR["DATEOFBUSINESSESTABLISMENT"]) != "")
            {
                DtpBusinessEsstaDate.Checked = true;
                DtpBusinessEsstaDate.Text = Val.ToString(DR["DATEOFBUSINESSESTABLISMENT"]);
            }
            else
                DtpBusinessEsstaDate.Checked = false;

            txtBusinessRegNo.Text = Val.ToString(DR["BUSINESSREGISTRATIONNO"]);
            txtOrganizationType.Text = Val.ToString(DR["ORGANIZATIONNO"]);
            txtQbcNO.Text = Val.ToString(DR["QBCNO"]);

            txtLocation_ID.Text = Val.ToString(DR["LOCATIONNAME"]);

            txtAccCode.Text = Val.ToString(DR["ACCCODE"]);

            txtContactPer.Text = Val.ToString(DR["CONTACTPER"]);

            //byte[] OFFICELOGO = DR["EMPPHOTO"] as byte[] ?? null;
            //if (OFFICELOGO != null)
            //{
            //    using (MemoryStream ms = new MemoryStream(OFFICELOGO))
            //    {
            //        PicEmpPhoto.Image = Image.FromStream(ms);
            //    }
            //}
            //else
            //{
            //    PicEmpPhoto.Image = null;
            //}

            FillLedgerDetailInfo(Val.ToString(txtLedgerID.Text));  //06-04-2019
            FillBankDetail();
            FillPartnerDetail();
            txtLedgerName.Focus();
            xtraTabControl2.SelectedTabPageIndex = 0;

            //add shiv 17-05-2022
            txtPreCarriBy.Text = Val.ToString(DR["PRECARRIBY"]);
            txtVessetFlight.Text = Val.ToString(DR["VESSETFLIGHT"]);
            txtPortofLoding.Text = Val.ToString(DR["PORTOFLODING"]);
            txtFinalDest.Text = Val.ToString(DR["FINALDEST"]);
            txtPlaceofRec.Text = Val.ToString(DR["PLACEOFREC"]);
            txtPortofDischarge.Text = Val.ToString(DR["PORTOFDISCHARGE"]);
            txtPartyDeclaration.Text = Val.ToString(DR["PARTYDEC"]);
            txtCompDecl.Text = Val.ToString(DR["COMPDEC"]);
            txtLegalName.Text = Val.ToString(DR["LEGALNAME"]);
            txtAadharCardNo.Text = Val.ToString(DR["AADHARCARDNO"]);


            //BUYER KYC
            CmbKycTypeBYR.Text = Val.ToString(DR["KYCTYPE"]);
            mStrKYCType = CmbKycTypeBYR.Text;
            CmbPartyTypeBYR.SelectedItem = mStrPartyType;
            txtLedgerCode.Text = Val.ToString(DR["LEDGERCODE"]);

            if (!Val.ToString(DR["LEDGERNAME"]).Trim().Equals(string.Empty))
                ChangeTextBoxPlaceHolder(txtLedgerName, txtLedgerName.Text, Val.ToString(DR["LEDGERNAME"]), Color.Black);
            else
                ChangeTextBoxPlaceHolder(txtLedgerName, txtLedgerName.Text, "NAME", Color.Silver);

            //txtLedgerName.Text = Val.ToString(DR["LEDGERNAME"]);
            txtLedgerID.Text = Val.ToString(DR["LEDGER_ID"]);

            txtCompanyBYR.Text = Val.ToString(DR["COMPANYNAME"]);
            txtMobileNo1BYR.Text = Val.ToString(DR["MOBILENO1"]);
            txtMobileNo2BYR.Text = Val.ToString(DR["MOBILENO2"]);
            txtLandlineNoBYR.Text = Val.ToString(DR["LANDLINENO"]);
            CmbFindTypeBYR.Text = Val.ToString(DR["PARTYFINDTYPE"]);
            txtFindByWhomNameBYR.Tag = Val.ToString(DR["FINDBYWHOM_ID"]);
            txtFindByWhomNameBYR.Text = Val.ToString(DR["FINDBYWHOMNAME"]);
            txtContactPerBYR.Text = Val.ToString(DR["CONTACTPER"]);
            ChkAllowWebAPIBYR.Checked = Val.ToBoolean(DR["ISALLOWWEBAPI"]);
            ChkIsNoBrokerBYR.Checked = Val.ToBoolean(DR["ISNOBROKER"]);
            txtDefaultSellerBYR.Text = Val.ToString(DR["DEFAULTSELLERNAME"]);
            txtDefaultSellerBYR.Tag = Val.ToString(DR["DEFAULTSELLER_ID"]);
            ChkAllowWebLoginBYR.Checked = Val.ToBoolean(DR["ISALLOWWEBLOGIN"]);

            if (!Val.ToString(DR["SHIPPINGADDRESS1"]).Trim().Equals(string.Empty))
                ChangeTextBoxPlaceHolder(txtSAddress1BYR, txtSAddress1BYR.Text, Val.ToString(DR["SHIPPINGADDRESS1"]), Color.Black);
            else
                ChangeTextBoxPlaceHolder(txtSAddress1BYR, txtSAddress1BYR.Text, "ADDRESS 1", Color.Silver);

            if (!Val.ToString(DR["SHIPPINGADDRESS2"]).Trim().Equals(string.Empty))
                ChangeTextBoxPlaceHolder(txtSAddress2BYR, txtSAddress2BYR.Text, Val.ToString(DR["SHIPPINGADDRESS2"]), Color.Black);
            else
                ChangeTextBoxPlaceHolder(txtSAddress2BYR, txtSAddress2BYR.Text, "ADDRESS 2", Color.Silver);

            if (!Val.ToString(DR["SHIPPINGADDRESS3"]).Trim().Equals(string.Empty))
                ChangeTextBoxPlaceHolder(txtSAddress3BYR, txtSAddress3BYR.Text, Val.ToString(DR["SHIPPINGADDRESS3"]), Color.Black);
            else
                ChangeTextBoxPlaceHolder(txtSAddress3BYR, txtSAddress3BYR.Text, "ADDRESS 3", Color.Silver);
            txtSStateBYR.Text = Val.ToString(DR["SHIPPINGSTATE"]);
            txtSCityBYR.Text = Val.ToString(DR["SHIPPINGCITY"]);
            txtSCountryBYR.Text = Val.ToString(DR["SHIPPINGCOUNTRYNAME"]);
            txtSCountryBYR.Tag = Val.ToString(DR["SHIPPINGCOUNTRY_ID"]);
            txtSZipCodeBYR.Text = Val.ToString(DR["SHIPPINGZIPCODE"]);
            txtEmailIDBYR.Text = Val.ToString(DR["EMAILID"]);
            TxtSalerEmailID.Text = Val.ToString(DR["SALLEREMAIL_ID"]);
            CmbStatusBYR.SelectedItem = Val.ToString(DR["STATUS"]);
            mStrStatus = Val.ToString(DR["STATUS"]);
            txtRemarkBYR.Text = Val.ToString(DR["REMARK"]);
            txtWeChatIDBYR.Text = Val.ToString(DR["WECHATID"]);
            txtQQIDBYR.Text = Val.ToString(DR["QQID"]);
            txtSkypeIDBYR.Text = Val.ToString(DR["SKYPEID"]);
            txtBrokerBYR.Tag = Val.ToString(DR["COORDINATOR_ID"]);
            txtBrokerBYR.Text = Val.ToString(DR["BROKERNAMENEW"]);

            if (Val.ToString(DR["DATEOFBIRTH"]) != "")
            {
                DtpBirthDateBYR.Checked = true;
                DtpBirthDateBYR.Text = Val.ToString(DR["DATEOFBIRTH"]);
            }
            else
                DtpBirthDateBYR.Checked = false;


            if (Val.ToString(DR["DATEOFANNIVERSARY"]) != "")
            {
                DtpAnniversaryBYR.Checked = true;
                DtpAnniversaryBYR.Text = Val.ToString(DR["DATEOFANNIVERSARY"]);
            }
            else
                DtpAnniversaryBYR.Checked = false;


            if (Val.ToString(DR["DATEOFJOIN"]) != "")
            {
                DTPDateOfJoinBYR.Checked = true;
                DTPDateOfJoinBYR.Text = Val.ToString(DR["DATEOFJOIN"]);
            }
            else
                DTPDateOfJoinBYR.Checked = false;


            if (Val.ToString(DR["DATEOFLEAVE"]) != "")
            {
                DtpDateOfLeaveBYR.Checked = true;
                DtpDateOfLeaveBYR.Text = Val.ToString(DR["DATEOFLEAVE"]);
            }
            else
                DtpDateOfLeaveBYR.Checked = false;

            FillLedgerDetailInfo(Val.ToString(txtLedgerID.Text));  //06-04-2019
            FillBankDetail();
            FillPartnerDetail();
            txtLedgerName.Focus();

            txtUserNameBYR.Text = Val.ToString(DR["USERNAME"]);
            txtPasswordBYR.Text = Val.ToString(DR["PASSWORD"]);

            txtPartyDeclaration.Text = Val.ToString(DR["PARTYDEC"]);
            txtCompDecl.Text = Val.ToString(DR["COMPDEC"]);
            txtAccCode.Text = Val.ToString(DR["ACCCODE"]);


            if (mStrKYCType == "BILLINGKYC" || mStrKYCType == "")
            {
                xtraTabPage2.PageVisible = true;
                xtraTabPage5.PageVisible = false;
                CmbPartyType.Focus();
            }
            else
            {
                if (mStrPartyType == "SALE" || mStrPartyType == "PURCHASE")
                {
                    xtraTabPage2.PageVisible = false;
                    xtraTabPage5.PageVisible = true;
                    xtraTabControl2.TabPages.Move(1, xtraTabPage5);
                    xtraTabControl2.SelectedTabPageIndex = 1;
                    CmbPartyTypeBYR.Focus();
                }
            }

            if (BOConfiguration.DEPTNAME == "ACCOUNT")
            {
                if (Val.ToString(DR["IS_SECADDRESS"]) == "True")
                {
                    ChkSecondAddress.Checked = true;
                }
                else { ChkSecondAddress.Checked = false; }
                txtSecondAdd.Text = Val.ToString(DR["SEC_ADDRESS"]);
                txtSecondAdd1.Text = Val.ToString(DR["Sec_Address1"]);
                txtSecondAdd2.Text = Val.ToString(DR["Sec_Address2"]);

                if (Val.ToString(DR["IS_TDSLIMIT"]) == "True")
                {
                    ChkIsTdsLimit.Checked = true;
                }
                else { ChkIsTdsLimit.Checked = false; }
            }
        }


        public void FillLedgerDetailInfo(string IntLedger_ID)
        {
            //Ledger Details
            Guid gLedger_ID = Val.ToString(IntLedger_ID).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(IntLedger_ID));
            DataSet DsDetailInfo = ObjMast.GetledgerDetailDInfoata(gLedger_ID);

            ////Attachment Details
            //if(Val.ToString(BusLib.Configuration.BOConfiguration.COMPANY_ID) == "fe4c657d-5452-44d3-84f7-c8c71e20446e")
            //{
            //    DtabAttachment = DsDetailInfo.Tables[1];
            //}
            //else{ DtabAttachment = DsDetailInfo.Tables[0]; }

            if (DsDetailInfo.Tables.Count > 0)
                DtabAttachment = DsDetailInfo.Tables[0];

            MainGrdAttachment.DataSource = DtabAttachment;
            MainGrdAttachment.RefreshDataSource();

            //End : Ledger Details : 06-04-2019
        }

        public void FillBankDetail()
        {
            DtabBank = ObjMast.FillLedgerBank(Val.ToString(txtLedgerID.Text).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtLedgerID.Text)));
            DtabBank.Rows.Add(DtabBank.NewRow());
            MainGrdBank.DataSource = DtabBank;
            MainGrdBank.Refresh();

        }

        public void FillPartnerDetail()
        {
            DtabPartnerDetail = ObjMast.FillLedgerPartnerDetail(Val.ToString(txtLedgerID.Text).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtLedgerID.Text)));
            DtabPartnerDetail.Rows.Add(DtabPartnerDetail.NewRow());
            MainGrdPartnerDetail.DataSource = DtabPartnerDetail;
            MainGrdPartnerDetail.Refresh();

        }


        private void txtDepartment_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "DEPARTMENTCODE,DEPARTMENTNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_DEPARTMENT);

                    FrmSearch.mStrColumnsToHide = "DEPARTMENT_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtDepartment.Text = Val.ToString(FrmSearch.DRow["DEPARTMENTNAME"]);
                        txtDepartment.Tag = Val.ToString(FrmSearch.DRow["DEPARTMENT_ID"]);
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

        private void txtDesignation_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "DESIGNATIONCODE,DESIGNATIONNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_DESIGNATION);

                    FrmSearch.mStrColumnsToHide = "DESIGNATION_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtDesignation.Text = Val.ToString(FrmSearch.DRow["DESIGNATIONNAME"]);
                        txtDesignation.Tag = Val.ToString(FrmSearch.DRow["DESIGNATION_ID"]);
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

        private void BtnAddManager_Click(object sender, EventArgs e)
        {
            //FrmLedger FrmLedger = new FrmLedger();
            //FrmLedger.MdiParent = Global.gMainRef;
            //ObjFormEvent.ObjToDisposeList.Add(FrmLedger);
            //FrmLedger.ShowForm("EMPLOYEE");
        }


        private void txtLedgerCode_Validated(object sender, EventArgs e)
        {
            Guid gLedger_ID = Val.ToString(txtLedgerName.Tag).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtLedgerName.Tag));

            DataRow DRow = ObjMast.GetLedgerInfoByCode(Val.ToString(CmbPartyType.SelectedItem), txtLedgerCode.Text, gLedger_ID);
            if (DRow != null)
            {
                FetchValue(DRow);
            }


            //if (ChkCodeUpdate.Checked == false)
            //{
            //    string Str = txtLedgerCode.Text;

            //    //BtnAdd_Click(null, null);
            //    Clear();

            //    txtLedgerCode.Text = Str;

            //    DataRow DRow = ObjMast.GetLedgerInfoByCode(Val.ToString(CmbPartyType.SelectedItem), txtLedgerCode.Text, gLedger_ID);
            //    if (DRow != null)
            //    {
            //        FetchValue(DRow);
            //    }

            //    txtLedgerName.Focus();
            //}
            //else
            //{
            //    if (!Val.ToString(txtLedgerName.Text).Trim().Equals(string.Empty))
            //    {
            //        DataRow DRow = ObjMast.GetLedgerInfoByCode(Val.ToString(CmbPartyType.SelectedItem), txtLedgerCode.Text, gLedger_ID);
            //        if (DRow != null)
            //        {
            //            if (Global.Confirm("This Employee Code Record Is Already Exists , [YES : For Update , NO : Refresh Record] ?") == System.Windows.Forms.DialogResult.No)
            //            {
            //                //BtnAdd_Click(null, null);
            //                Clear();
            //                FetchValue(DRow);
            //            }
            //        }
            //    }
            //}
        }


        private void txtLedgerCode_TextChanged(object sender, EventArgs e)
        {
            if (txtUserName.Text.Trim().Length == 0 && txtLedgerCode.Text.Length != 0)
            {
                txtUserName.Text = txtLedgerCode.Text;
                txtPassword.Text = txtLedgerCode.Text;
            }
        }

        private void repTxtUploadFilename_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DataRow dr = GrdAttachment.GetFocusedDataRow();
                    if (!Val.ToString(dr["ATTACHMENTFILENAME"]).Equals(string.Empty) && GrdAttachment.IsLastRow)
                    {
                        int MaxSrNo = 0;
                        MaxSrNo = (int)DtabAttachment.Compute("Max(SRNO)", "");
                        DataRow DRE = DtabAttachment.NewRow();
                        DRE["SRNO"] = MaxSrNo + 1;
                        DtabAttachment.Rows.Add(DRE);
                        //DtabPara.AcceptChanges();
                    }
                    else if (GrdAttachment.IsLastRow)
                    {
                        BtnSave.Focus();
                        e.Handled = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenDg = new OpenFileDialog();
            if (OpenDg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                DataRow DRow = DtabAttachment.NewRow();
                //MemoryStream ms = new MemoryStream();
                //Image image = Image.FromStream(ms);
                //Bitmap images = ResetResolution(image as Metafile, 300);
                byte[] ba = File.ReadAllBytes(OpenDg.FileName);
                //Bitmap Bt  = Image.FromStream(OpenDg.FileName);

                //Bitmap bm = new Bitmap(Image.FromStream(new MemoryStream(ba)));

                MemoryStream ms = new MemoryStream();
                ms.Position = 0;
                ms.Read(ba, 0, ba.Length);

                //Bitmap bmp;
                //using (var ms = new MemoryStream(imageData))
                //{
                //    bmp = new Bitmap(ms);
                //}




                DRow["SRNO"] = 0;
                DRow["ATTACHMENTFILENAME"] = OpenDg.SafeFileName;
                DRow["ATTACHMENT"] = ms.Read(ba, 0, ba.Length);
                //DRow["FILEPATH"] = OpenDg.FileName;
                //DRow["FILENAME"] = @"";
                DtabAttachment.Rows.Add(DRow);
                MainGrdAttachment.DataSource = DtabAttachment;
                MainGrdAttachment.Refresh();
            }
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {

            if (DtabAttachment.Rows.Count == 0)
            {
                Global.Message("Please add file for upload.");
                return;
            }

            //DevExpress.Data.CurrencyDataController.DisableThreadingProblemsDetection = true;
            //System.Threading.Thread Thread = new System.Threading.Thread(UploadFilesAttachment);
            //Thread.Start();

        }

        private void repBtnBrowse_KeyDown(object sender, KeyEventArgs e)
        {
            //DataRow dr = GrdAttachment.GetFocusedDataRow();
            //if (!Val.ToString(dr["UPLOADFILENAME"]).Equals(string.Empty) && !Val.ToString(dr["DOCUMENTTYPE"]).Equals(string.Empty) && GrdAttachment.IsLastRow)
            //{
            //    int MaxSrNo = 0;
            //    MaxSrNo = (int)DtabAttachment.Compute("Max(SRNO)", "");
            //    DataRow DRE = DtabAttachment.NewRow();
            //    DRE["SRNO"] = MaxSrNo + 1;
            //    DtabAttachment.Rows.Add(DRE);
            //    //DtabPara.AcceptChanges();
            //}
            //else if (GrdAttachment.IsLastRow)
            //{
            //    BtnSave.Focus();
            //    e.Handled = true;
            //}
        }

        private void repBtnDeleteAttachment_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dr = GrdAttachment.GetFocusedDataRow();
                if (dr != null)
                {
                    if (!Val.ToString(dr["ATTACHMENT_ID"]).Trim().Equals(string.Empty))
                    {
                        int res = ObjMast.DeleteLedgerDetailInfo("ATTACHMENTDETAIL", Val.ToString(dr["ATTACHMENT_ID"]));
                        if (res > 0)
                        {
                            dr.Delete();
                            DtabAttachment.AcceptChanges();
                        }
                    }
                    else
                    {
                        dr.Delete();
                        DtabAttachment.AcceptChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void repTxtDocumentType_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdAttachment.FocusedRowHandle < 0)
                    return;

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "DOCTYPECODE,DOCTYPENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_DOCUMENTTYPE);
                    FrmSearch.mStrColumnsToHide = "DOCTYPE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdAttachment.SetFocusedRowCellValue("DOCTYPENAME", Val.ToString(FrmSearch.DRow["DOCTYPENAME"]));
                        GrdAttachment.SetFocusedRowCellValue("DOCTYPE_ID", Val.ToString(FrmSearch.DRow["DOCTYPE_ID"]));
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }


        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable DtabLedger = ObjMast.GetDataForLedgerPrint(Val.ToString(txtLedgerName.Tag), Val.ToString(CmbPartyType.SelectedItem));
                Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                FrmReportViewer.MdiParent = Global.gMainRef;
                FrmReportViewer.ShowForm("KYCPrint", DtabLedger);
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message("sp : " + ex.Message.ToString());
            }
        }


        private void txtDefaultSeller_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "EMPLOYEECODE,EMPLOYEENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_EMPLOYEE);

                    FrmSearch.mStrColumnsToHide = "EMPLOYEE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtDefaultSeller.Text = Val.ToString(FrmSearch.DRow["EMPLOYEENAME"]);
                        txtDefaultSeller.Tag = Val.ToString(FrmSearch.DRow["EMPLOYEE_ID"]);
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


        private void txtSAddress1_Enter(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtSAddress1, "ADDRESS 1", "", Color.Black);
        }

        private void txtSAddress1_Leave(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtSAddress1, "", "ADDRESS 1", Color.Silver);
        }

        private void txtSAddress2_Enter(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtSAddress2, "ADDRESS 2", "", Color.Black);
        }

        private void txtSAddress2_Leave(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtSAddress2, "", "ADDRESS 2", Color.Silver);
        }

        private void txtSAddress3_Leave(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtSAddress3, "", "ADDRESS 3", Color.Silver);
        }

        private void txtSAddress3_Enter(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtSAddress3, "ADDRESS 3", "", Color.Black);
        }

        private void txtSCountry_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "COUNTRYCODE,COUNTRYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COUNTRY);

                    FrmSearch.mStrColumnsToHide = "COUNTRY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtSCountry.Text = Val.ToString(FrmSearch.DRow["COUNTRYNAME"]);
                        txtSCountry.Tag = Val.ToString(FrmSearch.DRow["COUNTRY_ID"]);
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

        private void CmbPartyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Clear();

            if (Val.ToString(CmbPartyType.SelectedItem) == "PURCHASE")
            {
                lblAccCode.Text = "Supplier";
                GrpAddress.Text = " Company Address";
                this.Text = "SUPPLIER DETAIL";

                lblDepartment.Visible = false;
                txtDepartment.Visible = false;
                lblDesignation.Visible = false;
                txtDesignation.Visible = false;

                GrpLoginInfo.Visible = false;
                GrpOtherStockDiscAddLess.Visible = true;
                //DtpBirthDate.Visible = true;
                //DtpAnniversary.Visible = true;
                //DTPDateOfJoin.Checked = true;
                //DTPDateOfJoin.Visible = true;
                //DtpDateOfLeave.Visible = true;
                //LblBirthDate.Visible = true;
                //LblAnniversaryDate.Visible = true;
                //LblJoinDate.Visible = true;
                //LblLeaveDate.Visible = true;
                txtBroker.Visible = true;
                LblBroker.Visible = true;
                ChkIsNoBroker.Visible = true;
                BtnAddNewParty.Visible = true;
                MainGrdBank.Refresh();
                GrpBankDetail.Visible = false;
                ChkAllowWebAPI.Visible = true;
                //Kuldeep Added Jamesallen price
                ChkSyncJamesAllen.Checked = true;
                //GrpAccountDetail.Visible = true;

                //#K: 04-11-2020
                txtBrokeragePer.Visible = false;
                cLabel52.Visible = false;


            }
            if (Val.ToString(CmbPartyType.SelectedItem) == "EMPLOYEE")
            {
                lblAccCode.Text = "Employee";
                GrpAddress.Text = " Employee Address";
                this.Text = "EMPLOYEE DETAIL";

                lblDepartment.Visible = true;
                txtDepartment.Visible = true;
                lblDesignation.Visible = true;
                txtDesignation.Visible = true;

                GrpLoginInfo.Visible = true;
                GrpOtherStockDiscAddLess.Visible = true;

                //DtpBirthDate.Visible = true;
                //DtpAnniversary.Visible = true;
                //DTPDateOfJoin.Checked = true;
                //DTPDateOfJoin.Visible = true;
                //DtpDateOfLeave.Visible = true;
                //LblBirthDate.Visible = true;
                //LblAnniversaryDate.Visible = true;
                //LblJoinDate.Visible = true;
                //LblLeaveDate.Visible = true;
                txtBroker.Visible = true;
                LblBroker.Visible = true;
                ChkIsNoBroker.Visible = true;
                BtnAddNewParty.Visible = true;
                MainGrdBank.Refresh();
                GrpBankDetail.Visible = false;
                ChkAllowWebAPI.Visible = true;
                //Kuldeep Added Jamesallen price
                ChkSyncJamesAllen.Checked = true;
                //GrpAccountDetail.Visible = true;

                //#K: 04-11-2020
                txtBrokeragePer.Visible = false;
                cLabel52.Visible = false;
            }
            else if (Val.ToString(CmbPartyType.SelectedItem) == "SALE")
            {
                lblAccCode.Text = "Customer";
                GrpAddress.Text = "Shipping Address";
                this.Text = "CUSTOMER DETAIL";

                lblDepartment.Visible = false;
                txtDepartment.Visible = false;
                lblDesignation.Visible = false;
                txtDesignation.Visible = false;


                GrpOtherStockDiscAddLess.Visible = false;
                //DtpBirthDate.Visible = true;
                //DtpAnniversary.Visible = true;
                //DTPDateOfJoin.Checked = true;
                //DTPDateOfJoin.Visible = true;
                //DtpDateOfLeave.Visible = true;
                //LblBirthDate.Visible = true;
                //LblAnniversaryDate.Visible = true;
                //LblJoinDate.Visible = true;
                //LblLeaveDate.Visible = true;
                txtBroker.Visible = true;
                LblBroker.Visible = true;
                ChkIsNoBroker.Visible = true;
                BtnAddNewParty.Visible = true;
                GrpBankDetail.Visible = false;
                ChkAllowWebAPI.Visible = true;
                //Kuldeep Added Jamesallen price
                ChkSyncJamesAllen.Checked = true;
                //GrpAccountDetail.Visible = true;

                //#K: 04-11-2020
                txtBrokeragePer.Visible = false;
                cLabel52.Visible = false;

            }
            else if (Val.ToString(CmbPartyType.SelectedItem) == "COMMISSION")
            {
                lblAccCode.Text = "Commission";
                GrpAddress.Text = " Company Address";
                this.Text = "COMMISSION PARTY DETAIL";
                txtAccCode.Enabled = false;

            }
            else if (Val.ToString(CmbPartyType.SelectedItem) == "BROKER")
            {
                lblAccCode.Text = "Broker";
                GrpAddress.Text = " Broker Address";

                this.Text = "BROKER DETAIL";

                lblDepartment.Visible = false;
                txtDepartment.Visible = false;
                lblDesignation.Visible = false;
                txtDesignation.Visible = false;


                GrpOtherStockDiscAddLess.Visible = false;
                //DtpBirthDate.Visible = true;
                //DtpAnniversary.Visible = true;
                //DTPDateOfJoin.Checked = true;
                //DTPDateOfJoin.Visible = true;
                //DtpDateOfLeave.Visible = true;
                //LblBirthDate.Visible = true;
                //LblAnniversaryDate.Visible = true;
                //LblJoinDate.Visible = true;
                //LblLeaveDate.Visible = true;
                txtBroker.Visible = false;
                LblBroker.Visible = false;
                ChkIsNoBroker.Visible = false;
                BtnAddNewParty.Visible = false;
                MainGrdBank.Refresh();
                GrpBankDetail.Visible = false;
                ChkAllowWebAPI.Visible = true;
                //Kuldeep Added Jamesallen price
                ChkSyncJamesAllen.Checked = true;
                //GrpAccountDetail.Visible = true;

                //#K: 04-11-2020
                txtBrokeragePer.Visible = true;
                cLabel52.Visible = true;
            }
            else if (Val.ToString(CmbPartyType.SelectedItem) == "AIRFREIGHT")
            {
                lblAccCode.Text = "AirFreight";
                GrpAddress.Text = " AirFreight Address";
                this.Text = "AIRFREIGHT DETAIL";

                lblDepartment.Visible = false;
                txtDepartment.Visible = false;
                lblDesignation.Visible = false;
                txtDesignation.Visible = false;

                GrpLoginInfo.Visible = false;
                GrpOtherStockDiscAddLess.Visible = false;
                DtpBirthDate.Visible = false;
                DtpAnniversary.Visible = false;
                DTPDateOfJoin.Visible = false;
                DtpDateOfLeave.Visible = false;
                //LblBirthDate.Visible = false;
                LblAnniversaryDate.Visible = false;
                LblJoinDate.Visible = false;
                LblLeaveDate.Visible = false;
                txtBroker.Visible = false;
                LblBroker.Visible = false;
                ChkIsNoBroker.Visible = false;
                BtnAddNewParty.Visible = false;
                MainGrdBank.Refresh();
                GrpBankDetail.Visible = false;
                ChkAllowWebAPI.Visible = false;
                //Kuldeep Added Jamesallen price
                ChkSyncJamesAllen.Checked = false;
                GrpAccountDetail.Visible = false;
                //#K: 04-11-2020
                txtBrokeragePer.Visible = false;
                cLabel52.Visible = false;
            }
            else if (Val.ToString(CmbPartyType.SelectedItem) == "COURIER")
            {
                lblAccCode.Text = "Courier";
                GrpAddress.Text = " Courier Address";
                this.Text = "COURIER DETAIL";

                lblDepartment.Visible = false;
                txtDepartment.Visible = false;
                lblDesignation.Visible = false;
                txtDesignation.Visible = false;

                GrpLoginInfo.Visible = false;
                GrpOtherStockDiscAddLess.Visible = false;
                DtpBirthDate.Visible = false;
                DtpAnniversary.Visible = false;
                DTPDateOfJoin.Visible = false;
                DtpDateOfLeave.Visible = false;
                //LblBirthDate.Visible = false;
                LblAnniversaryDate.Visible = false;
                LblJoinDate.Visible = false;
                LblLeaveDate.Visible = false;
                txtBroker.Visible = false;
                LblBroker.Visible = false;
                ChkIsNoBroker.Visible = false;
                BtnAddNewParty.Visible = false;
                MainGrdBank.Refresh();
                GrpBankDetail.Visible = false;
                ChkAllowWebAPI.Visible = false;
                GrpAccountDetail.Visible = false;
                //#K: 04-11-2020
                txtBrokeragePer.Visible = false;
                cLabel52.Visible = false;
            }
            else if (Val.ToString(CmbPartyType.SelectedItem) == "EXPENSE")
            {
                lblAccCode.Text = "Expense A/c";
                GrpAddress.Text = " Expense A/c Address";
                this.Text = "EXPENSE DETAIL";

                lblDepartment.Visible = false;
                txtDepartment.Visible = false;
                lblDesignation.Visible = false;
                txtDesignation.Visible = false;

                GrpLoginInfo.Visible = false;
                GrpOtherStockDiscAddLess.Visible = false;
                DtpBirthDate.Visible = false;
                DtpAnniversary.Visible = false;
                DTPDateOfJoin.Visible = false;
                DtpDateOfLeave.Visible = false;
                //LblBirthDate.Visible = false;
                LblAnniversaryDate.Visible = false;
                LblJoinDate.Visible = false;
                LblLeaveDate.Visible = false;
                txtBroker.Visible = false;
                LblBroker.Visible = false;
                ChkIsNoBroker.Visible = false;
                BtnAddNewParty.Visible = false;
                MainGrdBank.Refresh();
                GrpBankDetail.Visible = false;
                ChkAllowWebAPI.Visible = false;
                //Kuldeep Added Jamesallen price
                ChkSyncJamesAllen.Checked = false;
                GrpAccountDetail.Visible = false;
                //#K: 04-11-2020
                txtBrokeragePer.Visible = false;
                cLabel52.Visible = false;
            }
            else if (Val.ToString(CmbPartyType.SelectedItem) == "CASH")
            {
                lblAccCode.Text = "Cash A/c";
                GrpAddress.Text = " Cash A/C Address";
                this.Text = "CASH A/C DETAIL";

                lblDepartment.Visible = false;
                txtDepartment.Visible = false;
                lblDesignation.Visible = false;
                txtDesignation.Visible = false;

                GrpLoginInfo.Visible = false;
                GrpOtherStockDiscAddLess.Visible = false;
                DtpBirthDate.Visible = false;
                DtpAnniversary.Visible = false;
                DTPDateOfJoin.Visible = false;
                DtpDateOfLeave.Visible = false;
                //LblBirthDate.Visible = false;
                LblAnniversaryDate.Visible = false;
                LblJoinDate.Visible = false;
                LblLeaveDate.Visible = false;
                txtBroker.Visible = false;
                LblBroker.Visible = false;
                ChkIsNoBroker.Visible = false;
                BtnAddNewParty.Visible = false;
                MainGrdBank.Refresh();
                GrpBankDetail.Visible = false;
                ChkAllowWebAPI.Visible = false;
                //Kuldeep Added Jamesallen price
                ChkSyncJamesAllen.Checked = false;
                //GrpAccountDetail.Visible = true;

                //#K: 04-11-2020
                txtBrokeragePer.Visible = false;
                cLabel52.Visible = false;
            }
            else if (Val.ToString(CmbPartyType.SelectedItem) == "BANK")
            {
                lblAccCode.Text = "Bank A/c";
                GrpAddress.Text = " Bank Address";
                this.Text = "BANK A/C DETAIL";

                lblDepartment.Visible = false;
                txtDepartment.Visible = false;
                lblDesignation.Visible = false;
                txtDesignation.Visible = false;

                GrpLoginInfo.Visible = false;
                GrpOtherStockDiscAddLess.Visible = false;
                DtpBirthDate.Visible = false;
                DtpAnniversary.Visible = false;
                DTPDateOfJoin.Visible = false;
                DtpDateOfLeave.Visible = false;
                //LblBirthDate.Visible = false;
                LblAnniversaryDate.Visible = false;
                LblJoinDate.Visible = false;
                LblLeaveDate.Visible = false;
                txtBroker.Visible = false;
                LblBroker.Visible = false;
                ChkIsNoBroker.Visible = false;
                BtnAddNewParty.Visible = false;
                MainGrdBank.Refresh();
                GrpBankDetail.Visible = true;

                ChkAllowWebAPI.Visible = false;
                //Kuldeep Added Jamesallen price
                ChkSyncJamesAllen.Checked = false;
                // GrpAccountDetail.Visible = true;

                //#K: 04-11-2020
                txtBrokeragePer.Visible = false;
                cLabel52.Visible = false;
            }
            else if (Val.ToString(CmbPartyType.SelectedItem) == "OTHER")
            {
                lblAccCode.Text = "Other A/c";
                GrpAddress.Text = " Other A/c Address";
                this.Text = "OTHER DETAIL";

                lblDepartment.Visible = false;
                txtDepartment.Visible = false;
                lblDesignation.Visible = false;
                txtDesignation.Visible = false;

                GrpLoginInfo.Visible = false;
                GrpOtherStockDiscAddLess.Visible = false;
                DtpBirthDate.Visible = false;
                DtpAnniversary.Visible = false;
                DTPDateOfJoin.Visible = false;
                DtpDateOfLeave.Visible = false;
                //LblBirthDate.Visible = false;
                LblAnniversaryDate.Visible = false;
                LblJoinDate.Visible = false;
                LblLeaveDate.Visible = false;
                txtBroker.Visible = false;
                LblBroker.Visible = false;
                ChkIsNoBroker.Visible = false;
                BtnAddNewParty.Visible = false;
                MainGrdBank.Refresh();
                GrpBankDetail.Visible = false;
                ChkAllowWebAPI.Visible = false;
                //Kuldeep Added Jamesallen price
                ChkSyncJamesAllen.Checked = false;
                GrpAccountDetail.Visible = false;
                //#K: 04-11-2020
                txtBrokeragePer.Visible = false;
                cLabel52.Visible = false;
            }
            else
            {
                lblAccCode.Text = "Party";
                GrpAddress.Text = " Shipping Address";
                this.Text = "PARTY DETAIL";
            }

            if (BusLib.Configuration.BOConfiguration.gEmployeeProperty.USERNAME == "administrator")
            {
                GrpLoginInfo.Visible = true;
                groupBox2.Visible = true;
                cLabel56.Visible = true;
                cLabel57.Visible = true;
                cLabel58.Visible = true;
                cLabel59.Visible = true;
                cLabel61.Visible = true;
                cLabel60.Visible = true;
                txtPortofLoding.Visible = true;
                txtVessetFlight.Visible = true;
                txtPreCarriBy.Visible = true;
                txtFinalDest.Visible = true;
                txtPortofDischarge.Visible = true;
                txtPlaceofRec.Visible = true;
            }
            else if (BOConfiguration.DEPTNAME == "ACCOUNT")
            {
                cLabel56.Visible = true;
                cLabel57.Visible = true;
                cLabel58.Visible = true;
                cLabel59.Visible = true;
                cLabel61.Visible = true;
                cLabel60.Visible = true;
                txtPortofLoding.Visible = true;
                txtVessetFlight.Visible = true;
                txtPreCarriBy.Visible = true;
                txtFinalDest.Visible = true;
                txtPortofDischarge.Visible = true;
                txtPlaceofRec.Visible = true;
            }
            else
            {
                GrpLoginInfo.Visible = true;
                //GrpLoginInfo.Visible = (Val.ToString(BOConfiguration.gEmployeeProperty.LEDGER_ID).ToUpper() == "F42447D1-0BD8-EB11-9CD1-283926297D36" || Val.ToString(BOConfiguration.gEmployeeProperty.LEDGER_ID).ToUpper() == "B14D6A0F-0DD8-EB11-9CD1-283926297D36") ? true : false;  //Rahu And Milan have access to Update UserName and Password Os SaleParty
                groupBox2.Visible = false;
                cLabel56.Visible = false;
                cLabel57.Visible = false;
                cLabel58.Visible = false;
                cLabel59.Visible = false;
                cLabel61.Visible = false;
                cLabel60.Visible = false;
                txtPortofLoding.Visible = false;
                txtVessetFlight.Visible = false;
                txtPreCarriBy.Visible = false;
                txtFinalDest.Visible = false;
                txtPortofDischarge.Visible = false;
                txtPlaceofRec.Visible = false;
            }

            if (Val.ToString(BusLib.Configuration.BOConfiguration.COMPANY_ID) == "fe4c657d-5452-44d3-84f7-c8c71e20446e") //Company HK Login hase to following Visible nahi thay
            {
                groupBox2.Visible = true;
                txtAadharCardNo.Visible = false;
                lblAadharCardNo.Visible = false;
                txtGSTNo.Visible = false;
                lblGstNo.Visible = false;
                txtPanNo.Visible = false;
                lblPanNo.Visible = false;
                txtQbcNO.Visible = false;
                lblQBCNo.Visible = false;
                LblBroker.Visible = false;
                txtBroker.Visible = false;
                BtnAddNewParty.Visible = false;
                ChkAllowWebAPI.Visible = false;
                ChkAllowWebLogin.Visible = false;
                ChkSyncJamesAllen.Visible = false;
                ChkIsNoBroker.Visible = false;
            }

        }

        private void BtnEmail_Click(object sender, EventArgs e)
        {
            
            if (txtLedgerName.Text.Trim() == "")
            {
                Global.Message("Ledger Name Is Required");
                return;
            }

            if (Global.Confirm("Do You Want To Send Status Change Email ? ") == DialogResult.Yes)
            {
                this.Cursor = Cursors.WaitCursor;
                string Str = new Class.CommonEmail().RegistrationApproval(txtLedgerName.Text, txtEmailID.Text, txtUserName.Text, Val.ToString(CmbStatus.SelectedItem),TxtSalerEmailID.Text);
                if (Str == "SUCCESS")
                {
                    Global.Message("Email Sent Successfully");
                }
                else
                {
                    Global.Message(Str);
                }

                this.Cursor = Cursors.Default;
            }
        }

        private void txtAccountType_KeyPress(object sender, KeyPressEventArgs e) //ADD BHAGYASHREE 03/08/2019
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "ACCTTYPE_ID,ACCTTYPENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_ACCTTYPE);

                    FrmSearch.mStrColumnsToHide = "ACCTTYPE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtAccountType.Text = Val.ToString(FrmSearch.DRow["ACCTTYPENAME"]);
                        txtAccountType.Tag = Val.ToString(FrmSearch.DRow["ACCTTYPE_ID"]);
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

        private void txtCurrency_KeyPress(object sender, KeyPressEventArgs e) //ADD BHAGYASHREE 03/08/2019
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "CURRENCYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CURRENCY);

                    FrmSearch.mStrColumnsToHide = "CURRENCY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtCurrency.Text = Val.ToString(FrmSearch.DRow["CURRENCYNAME"]);
                        txtCurrency.Tag = Val.ToString(FrmSearch.DRow["CURRENCY_ID"]);

                        //lblGrossAmountFESymbol.Text = Val.ToString(FrmSearch.DRow["SYMBOL"]);
                        //lblInsuranceAmountFESymbol.Text = Val.ToString(FrmSearch.DRow["SYMBOL"]);
                        //lblShippingAmountFESymbol.Text = Val.ToString(FrmSearch.DRow["SYMBOL"]);
                        //lblDiscAmountFESymbol.Text = Val.ToString(FrmSearch.DRow["SYMBOL"]);
                        //lblGSTAmountFESymbol.Text = Val.ToString(FrmSearch.DRow["SYMBOL"]);
                        //lblNetAmountFESymbol.Text = Val.ToString(FrmSearch.DRow["SYMBOL"]);

                        this.Cursor = Cursors.WaitCursor;
                        txtExcRate.Text = new BOTRN_MemoEntry().GetExchangeRate(Val.ToInt(txtCurrency.Tag), Val.SqlDate(DateTime.Now.ToShortDateString()), mFormType.ToString()).ToString();
                        this.Cursor = Cursors.Default;

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

        private void txtCurrency_Validated(object sender, EventArgs e)
        {
            //this.Cursor = Cursors.WaitCursor;
            //txtExcRate.Text = new BOTRN_MemoEntry().GetExchangeRate(Val.ToInt(txtCurrency.Tag), Val.SqlDate(DateTime.Now.ToShortDateString())).ToString();
            //this.Cursor = Cursors.Default;
        }

        private void txtLedgerName_Leave(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtLedgerName, "", "NAME", Color.Silver);
        }

        private void txtLedgerName_Enter(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtLedgerName, "NAME", "", Color.Black);
        }

        private void txtSState_KeyPress(object sender, KeyPressEventArgs e)
        {

            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "STATECODE,STATENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().GetState(Val.ToInt(txtSCountry.Tag));

                    FrmSearch.mStrColumnsToHide = "STATE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtSState.Text = Val.ToString(FrmSearch.DRow["STATENAME"]);
                        //txtBState.Tag = Val.ToString(FrmSearch.DRow["STATE_ID"]);
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

        private void txtFindByWhomName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "EMPLOYEECODE,EMPLOYEENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_EMPLOYEE);

                    FrmSearch.mStrColumnsToHide = "EMPLOYEE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtFindByWhomName.Text = Val.ToString(FrmSearch.DRow["EMPLOYEENAME"]);
                        txtFindByWhomName.Tag = Val.ToString(FrmSearch.DRow["EMPLOYEE_ID"]);
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

        private void txtBroker_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME,MobileNo,PARTYACCCODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_BROKER);

                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBroker.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtBroker.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
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

        private void CmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Val.ToString(CmbStatus.SelectedItem) == "DEACTIVE")
            {
                DtpDateOfLeave.Enabled = true;
            }
            else
            {
                DtpDateOfLeave.Enabled = false;
            }


        }

        private void BtnAddNewParty_Click(object sender, EventArgs e)
        {
            try
            {
                string StrPartyType = "";
                if (mFormType == FORMTYPE.BROKER)
                {
                    StrPartyType = "BROKER";

                }

                FrmLedgerNew FrmLedgerNew = new FrmLedgerNew();
                FrmLedgerNew.MdiParent = Global.gMainRef;
                FrmLedgerNew.ShowForm(StrPartyType);

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }

        }

        private void MainGrdBank_Click(object sender, EventArgs e)
        {

        }

        private void reptxtaddress_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DataRow dr = GrdBank.GetFocusedDataRow();
                    if (!Val.ToString(dr["BANKNAME"]).Equals(string.Empty) && !Val.ToString(dr["BANKACCOUNTNO"]).Equals(string.Empty) && GrdBank.IsLastRow)
                    {
                        DtabBank.Rows.Add(DtabBank.NewRow());
                    }
                    else if (GrdBank.IsLastRow)
                    {
                        BtnSave.Focus();
                        e.Handled = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }

        }

        private void repBtnDelete_Click(object sender, EventArgs e)
        {
            LedgerMasterProperty Property = new LedgerMasterProperty();
            try
            {

                if (Global.Confirm("Are Your Sure To Delete The Record ?") == System.Windows.Forms.DialogResult.No)
                    return;

                DataRow dr = GrdBank.GetFocusedDataRow();

                Property.BANK_ID = Guid.Parse(Val.ToString(dr["BANK_ID"]));
                Property = ObjMast.LedgerBanKDelete(Property);
                Global.Message(Property.ReturnMessageDesc);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    FillBankDetail();
                    BtnAdd_Click(null, null);
                }
                else
                {
                    txtLedgerName.Focus();
                }

            }
            catch (System.Exception ex)
            {
                Global.MessageToster(ex.Message);
            }

            DtabBank.AcceptChanges();
            MainGrdBank.Refresh();
            Property = null;
        }

        private void MainGrdAttachment_Click(object sender, EventArgs e)
        {

        }

        private void BtnAPIEmail_Click(object sender, EventArgs e)
        {
            if (txtLedgerName.Text.Trim() == "")
            {
                Global.Message("Ledger Name Is Required");
                return;
            }

            if (ChkAllowWebAPI.Checked == false)
            {
                Global.Message("First Select Allow Web API Setting");
                ChkAllowWebAPI.Focus();
                return;
            }


            if (Global.Confirm("Do You Want To Send Status Change Email ? ") == DialogResult.Yes)
            {
                this.Cursor = Cursors.WaitCursor;
                string Str = new Class.CommonEmail().APIApproval(txtLedgerName.Text, txtLedgerID.Text, txtEmailID.Text, txtUserName.Text, Val.ToString(CmbStatus.SelectedItem));
                if (Str == "SUCCESS")
                {
                    Global.Message("Email Sent Successfully");
                }
                else
                {
                    Global.Message(Str);
                }

                this.Cursor = Cursors.Default;
            }
        }

        private void BtnAddNewCode_Click(object sender, EventArgs e)
        {
            txtLedgerCode.Text = Val.ToString(ObjMast.FindMaxLedgerCode());
           
        }



        private void repBtnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow DRow = GrdAttachment.GetFocusedDataRow();

                OpenFileDialog OpenDg = new OpenFileDialog();
                if (OpenDg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.Cursor = Cursors.WaitCursor;
                    byte[] ba = File.ReadAllBytes(OpenDg.FileName);

                    FileInfo F = new FileInfo(OpenDg.FileName);
                    string base64 = Convert.ToBase64String(ba);

                    DocumentDownload.WebService client = new DocumentDownload.WebService();
                    XmlNode Node = client.UploadDocument_XML(base64, F.Extension, Val.ToString(DRow["DOCTYPENAME"]), Val.ToString(txtLedgerID.Text), "", "WEB", "1.0");
                    string s = Node.InnerXml;

                    //XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(Node.InnerXml.ToString());

                    string StrMessage = Node.SelectSingleNode("//ZMESSAGE/RETURNMESSAGE").InnerText;
                    Global.Message(StrMessage);

                    FillLedgerDetailInfo(Val.ToString(txtLedgerID.Text));
                    Node = null;
                    this.Cursor = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message);
            }

        }


        private void repBtnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                DataRow DRow = GrdAttachment.GetFocusedDataRow();
                string URL = Val.ToString(DRow["URL"]);
                if (URL != "")
                {
                    System.Diagnostics.Process.Start(URL);
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.ToString());
            }
        }

        private void RepBtnPartnerDetailDelete_Click(object sender, EventArgs e)
        {

        }

        private void repTxtSamaj_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    DataRow dr = GrdPartner.GetFocusedDataRow();
                    if (!Val.ToString(dr["PARTNERNAME"]).Equals(string.Empty) && GrdPartner.IsLastRow)
                    {
                        DtabPartnerDetail.Rows.Add(DtabPartnerDetail.NewRow());
                    }
                    else if (GrdPartner.IsLastRow)
                    {
                        BtnSave.Focus();
                        e.Handled = true;
                    }
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtLocation_ID_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "LOCATIONCODE,LOCATIONNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LOCATION);

                    FrmSearch.mStrColumnsToHide = "LOCATION_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtLocation_ID.Text = Val.ToString(FrmSearch.DRow["LOCATIONNAME"]);
                        txtLocation_ID.Tag = Val.ToString(FrmSearch.DRow["LOCATION_ID"]);
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

        private void FrmLedgerNew_Load(object sender, EventArgs e)
        {

        }

        private void PnlHeader2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtFindByWhomNameBYR_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "EMPLOYEECODE,EMPLOYEENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_EMPLOYEE);

                    FrmSearch.mStrColumnsToHide = "EMPLOYEE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtFindByWhomNameBYR.Text = Val.ToString(FrmSearch.DRow["EMPLOYEENAME"]);
                        txtFindByWhomNameBYR.Tag = Val.ToString(FrmSearch.DRow["EMPLOYEE_ID"]);
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

        private void txtDefaultSellerBYR_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "EMPLOYEECODE,EMPLOYEENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_EMPLOYEE);

                    FrmSearch.mStrColumnsToHide = "EMPLOYEE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtDefaultSellerBYR.Text = Val.ToString(FrmSearch.DRow["EMPLOYEENAME"]);
                        txtDefaultSellerBYR.Tag = Val.ToString(FrmSearch.DRow["EMPLOYEE_ID"]);
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

        private void txtSCountryBYR_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "COUNTRYCODE,COUNTRYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COUNTRY);

                    FrmSearch.mStrColumnsToHide = "COUNTRY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtSCountryBYR.Text = Val.ToString(FrmSearch.DRow["COUNTRYNAME"]);
                        txtSCountryBYR.Tag = Val.ToString(FrmSearch.DRow["COUNTRY_ID"]);
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

        private void txtSStateBYR_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "STATECODE,STATENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().GetState(Val.ToInt(txtSCountryBYR.Tag));

                    FrmSearch.mStrColumnsToHide = "STATE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtSStateBYR.Text = Val.ToString(FrmSearch.DRow["STATENAME"]);
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

        private void txtSAddress1BYR_Enter(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtSAddress1BYR, "ADDRESS 1", "", Color.Black);
        }

        private void txtSAddress1BYR_Leave(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtSAddress1BYR, "", "ADDRESS 1", Color.Silver);
        }

        private void txtSAddress2BYR_Enter(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtSAddress2BYR, "ADDRESS 2", "", Color.Black);
        }

        private void txtSAddress2BYR_Leave(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtSAddress2BYR, "", "ADDRESS 2", Color.Silver);
        }

        private void txtSAddress3BYR_Enter(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtSAddress3BYR, "ADDRESS 3", "", Color.Black);
        }

        private void txtSAddress3BYR_Leave(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtSAddress3BYR, "", "ADDRESS 3", Color.Silver);
        }

        private void txtBrokerBYR_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME,PARTYTYPE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_BROKERADAT);

                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBrokerBYR.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtBrokerBYR.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
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

        private void BtnAddNewPartyBYR_Click(object sender, EventArgs e)
        {
            try
            {
                string StrPartyType = "";
                if (mFormType == FORMTYPE.BROKER)
                {
                    StrPartyType = "BROKER";

                }

                FrmLedgerNew FrmLedgerNew = new FrmLedgerNew();
                FrmLedgerNew.MdiParent = Global.gMainRef;
                FrmLedgerNew.ShowForm(StrPartyType);

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }



        private void txtPortofLoding_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "CITYNAME,CITY_ID";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CITY);

                    FrmSearch.mStrColumnsToHide = "CITY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtPortofLoding.Text = Val.ToString(FrmSearch.DRow["CITYNAME"]);
                        txtPortofLoding.Tag = Val.ToString(FrmSearch.DRow["CITY_ID"]);
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



        private void BtnKYCTypeOk_Click(object sender, EventArgs e)
        {
            mStrKYCType = CmbKYCType.Text;
            GrpKYCTypeSelection.SendToBack();
            GrpKYCTypeSelection.Visible = false;
            GrpKYCTypeSelection.Enabled = false;
            panel1.Enabled = true;
            xtraTabControl2.Enabled = true;
            panel3.Enabled = true;
            //CmbKYCType.SelectedIndex = 0;
            if (mStrKYCType == "BILLINGKYC")
            {
                xtraTabPage2.PageVisible = true;
                xtraTabPage5.PageVisible = false;
                CmbPartyType.Focus();
            }
            else
            {
                xtraTabPage2.PageVisible = false;
                xtraTabPage5.PageVisible = true;
                xtraTabControl2.TabPages.Move(1, xtraTabPage5);
                xtraTabControl2.SelectedTabPageIndex = 1;
                CmbPartyTypeBYR.Focus();
            }
        }

        private void ChkSecondAddress_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ChkSecondAddress.Checked == true)
                {
                    txtSAddress1.ReadOnly = true;
                    txtSAddress2.ReadOnly = true;
                    txtSAddress3.ReadOnly = true;
                    txtSCountry.ReadOnly = true;
                    txtSState.ReadOnly = true;
                    txtSCity.ReadOnly = true;
                    txtSecondAdd.ReadOnly = false;
                }
                else
                {

                    txtSAddress1.ReadOnly = false;
                    txtSAddress2.ReadOnly = false;
                    txtSAddress3.ReadOnly = false;
                    txtSCountry.ReadOnly = false;
                    txtSState.ReadOnly = false;
                    txtSCity.ReadOnly = false;
                    txtSecondAdd.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtLedgerName_KeyUp(object sender, KeyEventArgs e)
        {
            if (BOConfiguration.DEPTNAME == "ACCOUNT")
            {
                txtLedgerName.SelectionStart = Val.ToInt32(txtLedgerName.Text.Length);
            }
        }

        private void ChkOpenPassword_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkOpenPassword.Checked == true)
            {
                string StrPass = txtPassword.Text;
                txtPassword.PasswordChar = '\0';
                txtPassword.Text = StrPass;
            }
            else
            {
                string StrPass = txtPassword.Text;
                txtPassword.PasswordChar = '*';
                txtPassword.Text = StrPass;
            }
        }

        //Add shiv KYC LINK
        private void btnkyclink_Click(object sender, EventArgs e)
        {
            try
            {
                string Link = "";
                PnlkycLink.BringToFront();
                PnlkycLink.Visible = true;
                PnlkycLink.Enabled = true;
                panel1.Enabled = false;
                xtraTabControl2.Enabled = false;
                panel3.Enabled = false;

                if (Val.ToString(CmbPartyType.SelectedItem) == "BROKER")
                {
                    string URL = "https://skrishna.co/BrokerKYC/KYC?Prty_ID=";
                    if (URL != "")
                    {
                        Link = URL + txtLedgerID.Text;
                        txtKyclink.Text = Link;
                        btnCopylink.Select();
                    }
                }
                else
                {
                    string URL = "https://skrishna.co/CompanyKYC/KYC?Prty_ID=";
                    if (URL != "")
                    {
                        Link = URL + txtLedgerID.Text;
                        txtKyclink.Text = Link;
                        btnCopylink.Select();
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void btnCopylink_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                if (!string.IsNullOrEmpty(txtKyclink.Text))
                {
                    Clipboard.SetText(txtKyclink.Text);
                    PnlkycLink.SendToBack();
                    PnlkycLink.Visible = false;
                    PnlkycLink.Enabled = false;
                    panel1.Enabled = true;
                    xtraTabControl2.Enabled = true;
                    panel3.Enabled = true;
                }
                else
                {
                    PnlkycLink.SendToBack();
                    PnlkycLink.Visible = false;
                    PnlkycLink.Enabled = false;
                    panel1.Enabled = true;
                    xtraTabControl2.Enabled = true;
                    panel3.Enabled = true;
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtBrokerName_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "BEOKERNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LEDGERBROKER);

                    FrmSearch.mStrColumnsToHide = "BEOKERNAME";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBrokername.Text = Val.ToString(FrmSearch.DRow["BEOKERNAME"]);
                        txtBrokername.Tag = Val.ToString(FrmSearch.DRow["BROKER_ID"]);
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
            //try
            //{
            //    if (Global.OnKeyPressEveToPopup(e))
            //    {
            //        FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
            //        FrmSearch.mStrSearchField = "BEOKERNAME";
            //        FrmSearch.mStrSearchText = e.KeyChar.ToString();
            //        this.Cursor = Cursors.WaitCursor;
            //        FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_BROKER);

            //        FrmSearch.mStrColumnsToHide = "BROKER_ID";
            //        this.Cursor = Cursors.Default;
            //        FrmSearch.ShowDialog();
            //        e.Handled = true;
            //        if (FrmSearch.DRow != null)
            //        {
            //            txtBrokername.Text = Val.ToString(FrmSearch.DRow["BROKERNAME"]);
            //            txtBrokername.Tag = Val.ToString(FrmSearch.DRow["BROKER_ID"]);
            //        }

            //        FrmSearch.Hide();
            //        FrmSearch.Dispose();
            //        FrmSearch = null;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Global.MessageError(ex.Message);
            //}
        }


        }
    
}


