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

namespace MahantExport.Masters
{
    public partial class FrmLedger : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_Ledger ObjMast = new BOMST_Ledger();
        BOFormPer ObjPer = new BOFormPer();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();
       
        DataTable DtabExperience = new DataTable();
        DataTable DtabFamily = new DataTable();
        DataTable DtabReference = new DataTable();
        DataTable DtabAttachment = new DataTable();
        string mStrPartyType = "";

        #region Property Settings

        public FrmLedger()
        {
            InitializeComponent();
        }
        public void ShowForm()
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            this.Text = "PATRY MASTER";

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            txtLedgerCode.Focus();

            BtnAdd_Click(null, null);
          
            try
            {
                foreach (BOCaptureDevice device in BOCaptureDevice.GetDevices())
                {
                    cboDevices.Items.Add(device);
                }

                if (cboDevices.Items.Count > 0)
                {
                    cboDevices.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
            }

            this.Show();
            CmbPartyType.Focus();
        }
        public void ShowForm(string StrPartyType,string StrParty_ID)  // Used Foe Getting Party Data
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            this.Text = "PATRY MASTER";

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            mStrPartyType = StrPartyType;
            CmbPartyType.SelectedItem = StrPartyType;

            txtLedgerName.Tag = StrParty_ID;

            txtLedgerCode_Validated(null, null);
            try
            {
                foreach (BOCaptureDevice device in BOCaptureDevice.GetDevices())
                {
                    cboDevices.Items.Add(device);
                }

                if (cboDevices.Items.Count > 0)
                {
                    cboDevices.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
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

            
            //CmbPartyType.SelectedItem = mStrPartyType;
            CmbPartyType.Enabled = false;

            try
            {
                foreach (BOCaptureDevice device in BOCaptureDevice.GetDevices())
                {
                    cboDevices.Items.Add(device);
                }

                if (cboDevices.Items.Count > 0)
                {
                    cboDevices.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
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

            //if (txtLedgerCode.Text.Trim().Length == 0)
            //{
            //    Global.Message("Party Code Is Required");
            //    txtLedgerCode.Focus();
            //    return false;
            //}
            if (txtLedgerName.Text.Trim().Length == 0 || txtLedgerName.ForeColor == Color.Silver)
            {
                Global.Message("Party Name Is Required");
                txtLedgerName.Focus();
                return false;
            }
            //else if (txtDepartment.Text.Trim().Length == 0)
            //{
            //    Global.Message("Department Is Required");
            //    txtDepartment.Focus();
            //    return false;
            //}
            //else if (txtDesignation.Text.Trim().Length == 0)
            //{
            //    Global.Message("Designation Is Required");
            //    txtDesignation.Focus();
            //    return false;
            //}
            else if (Val.ToString(CmbPartyType.Text) == "SALE" && Val.ToString(txtDefaultSeller.Text).Trim().Equals(string.Empty))
            {
                Global.Message("Default Seller Is Required");
                txtDefaultSeller.Focus();
                return false;
            }
            else if (Val.ToString(CmbStatus.SelectedItem) == "ACTIVE" && Val.ToString(CmbPartyType.SelectedItem) == "SALE")
            {
                if (Val.ToString(txtUserName.Text).Trim().Equals(string.Empty))
                {
                    Global.Message("UserName Is Required");
                    txtUserName.Focus();
                    return false;
                }
                if (Val.ToString(txtPassword.Text).Trim().Equals(string.Empty))
                {
                    Global.Message("Password Is Required");
                    txtPassword.Focus();
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
                CmbPartyType.SelectedIndex = 0;
            else
                CmbPartyType.SelectedItem = mStrPartyType;

            Clear();
        }


        public void Clear()
        {
            txtLedgerID.Text = string.Empty;
            //txtLedgerCode.Text = string.Empty;

            txtLedgerCode.Text = Val.ToString(ObjMast.FindMaxLedgerCode());
            txtLedgerCode.Enabled = false;

            ChangeTextBoxPlaceHolder(txtLedgerName, txtLedgerName.Text, "Name", Color.Silver);
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
            txtQQID.Text = string.Empty;
            txtSkypeID.Text = string.Empty;
            txtWebsite.Text = string.Empty;

            if (Val.ToString(CmbPartyType.SelectedItem) == "SALE")
            {
                GrpLoginInfo.Visible = true;
                txtRemark.Location = new Point(564, 452);
                lblRemark.Location = new Point(503, 454);
            }
            else
            {
                GrpLoginInfo.Visible = false;
                txtRemark.Location = new Point(560, 358);
                lblRemark.Location = new Point(498, 360);
            }

            ChangeTextBoxPlaceHolder(txtBAddress1, txtBAddress1.Text, "Address 1", Color.Silver);
            ChangeTextBoxPlaceHolder(txtBAddress2, txtBAddress2.Text, "Address 2", Color.Silver);
            ChangeTextBoxPlaceHolder(txtBAddress3, txtBAddress3.Text, "Address 3", Color.Silver);
            txtBState.Text = string.Empty;
            txtBCountry.Text = string.Empty;
            txtBCountry.Tag = string.Empty;
            txtBCity.Text = string.Empty;
            txtBZipCode.Text = string.Empty;

            ChangeTextBoxPlaceHolder(txtSAddress1, txtSAddress1.Text, "Address 1", Color.Silver);
            ChangeTextBoxPlaceHolder(txtSAddress2, txtSAddress2.Text, "Address 2", Color.Silver);
            ChangeTextBoxPlaceHolder(txtSAddress3, txtSAddress3.Text, "Address 3", Color.Silver);
            txtSState.Text = string.Empty;
            txtSCountry.Text = string.Empty;
            txtSCountry.Tag = string.Empty;
            txtSCity.Text = string.Empty;
            txtSZipCode.Text = string.Empty;

            txtDefaultSeller.Text = string.Empty;
            txtDefaultSeller.Tag = string.Empty;

            ChkAllowWebLogin.Checked = false;

            ChkAllowDefDiscDiff.Checked = false;
            txtDefDiscDiff.Text = "0.00";

            ChkAllowMemberDisc.Checked = false;
            txtMemberDisc.Text = "0.00";

            txtRemark.Text = string.Empty;

            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;

            PicEmpPhoto.Image = null;

            CmbPartyType.Focus();

            CmbGender.SelectedIndex = 0;
            FillLedgerDetailInfo(Val.ToString(txtLedgerID.Text));
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
                Property.LEDGERTYPE = Val.ToString(CmbPartyType.SelectedItem);

                Property.CONTACTPER = Val.ToString(txtLedgerName.Text);
                Property.COMPANYNAME = Val.ToString(txtCompany.Text);

                Property.EMAILID = Val.ToString(txtEmailID.Text);
                Property.QQID = Val.ToString(txtQQID.Text);
                Property.WECHATID = Val.ToString(txtWeChatID.Text);

                Property.SKYPEID = Val.ToString(txtSkypeID.Text);
                Property.WEBSITE = Val.ToString(txtWebsite.Text);
                Property.GENDER = Val.ToString(CmbGender.SelectedItem);

                Property.DEPARTMENT_ID = Val.ToInt32(txtDepartment.Tag);
                Property.DESIGNATION_ID = Val.ToInt32(txtDesignation.Tag);

                Property.BILLINGADDRESS1 = txtBAddress1.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtBAddress1.Text);
                Property.BILLINGADDRESS2 = txtBAddress2.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtBAddress2.Text);
                Property.BILLINGADDRESS3 = txtBAddress3.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtBAddress3.Text);
                Property.BILLINGSTATE = Val.ToString(txtBState.Text);
                Property.BILLINGCOUNTRY_ID = Val.ToInt32(txtBCountry.Tag);
                Property.BILLINGCITY = Val.ToString(txtBCity.Text);
                Property.BILLINGZIPCODE = Val.ToString(txtBZipCode.Text);

                Property.SHIPPINGADDRESS1 = txtSAddress1.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtSAddress1.Text);
                Property.SHIPPINGADDRESS2 = txtSAddress2.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtSAddress2.Text);
                Property.SHIPPINGADDRESS3 = txtSAddress3.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtSAddress3.Text);
                Property.SHIPPINGSTATE = Val.ToString(txtSState.Text);
                Property.SHIPPINGCOUNTRY_ID = Val.ToInt32(txtSCountry.Tag);
                Property.SHIPPINGCITY = Val.ToString(txtSCity.Text);
                Property.SHIPPINGZIPCODE = Val.ToString(txtSZipCode.Text);

                Property.MOBILENO1 = txtMobileNo1.Text;
                Property.MOBILENO2 = txtMobileNo2.Text;
                Property.LANDLINENO = txtLandlineNo.Text;

                Property.ISALLOWWEBLOGIN = Val.ToBoolean(ChkAllowWebLogin.Checked);
                
                Property.USERNAME = txtUserName.Text;
                Property.PASSWORD = txtPassword.Text;

                Property.ISDEFAULTDISCOUNTDIFF = Val.ToBoolean(ChkAllowDefDiscDiff.Checked);
                Property.DEFAULTDISCOUNTDIFF = Val.Val(txtDefDiscDiff.Text);

                Property.ISMEMBERDISCOUNT = Val.ToBoolean(ChkAllowMemberDisc.Checked);
                Property.MEMBERDISCOUNT = Val.Val(txtMemberDisc.Text);

                Property.DEFAULTSELLER_ID = Val.ToString(txtDefaultSeller.Tag).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtDefaultSeller.Tag));

                Property.STATUS = Val.ToString(CmbStatus.SelectedItem);
                Property.REMARK = Val.ToString(txtRemark.Text);
                //ADD BHAGYASHREE 03/08/2019
                Property.ACCTTYPE_ID = Val.ToInt32(txtAccountType.Tag);
                Property.CURRENCY_ID = Val.ToInt32(txtCurrency.Tag);
                Property.EXCRATE = Val.Val(txtExcRate.Text);
                Property.OPENINGCREDITUSD = Val.ToString(txtCurrency.Text) == "USD" ? Val.Val(txtOpeningCr.Text) : Math.Round(Val.Val(txtOpeningCr.Text) * Val.Val(txtExcRate.Text), 2);
                Property.OPENINGDEBITUSD = Val.ToString(txtCurrency.Text) == "USD" ? Val.Val(txtOpeningDr.Text) : Math.Round(Val.Val(txtOpeningDr.Text) * Val.Val(txtExcRate.Text), 2);

                Property.OPENINGCREDITFE = Val.ToString(txtCurrency.Text) == "USD" ? Math.Round(Val.Val(txtOpeningCr.Text) / Val.Val(txtExcRate.Text), 2) : Val.Val(txtOpeningCr.Text);
                Property.OPENINGDEBITFE = Val.ToString(txtCurrency.Text) == "USD" ? Math.Round(Val.Val(txtOpeningDr.Text) / Val.Val(txtExcRate.Text), 2) : Val.Val(txtOpeningDr.Text);

                //
                Property.EMPPHOTO = null;
                if (PicEmpPhoto.Image != null)
                {
                    MemoryStream ms = new MemoryStream();
                    PicEmpPhoto.Image.Save(ms, ImageFormat.Png);
                    byte[] Byte = new byte[ms.Length];
                    ms.Position = 0;
                    ms.Read(Byte, 0, Byte.Length);
                    Property.EMPPHOTO = Byte;
                }

                Property.LOCATION_ID = Val.ToInt32(txtLoacation.Tag);

                Property = ObjMast.Save(Property);

                string StrReturnDesc = Property.ReturnMessageDesc;


                if (Property.ReturnMessageType == "SUCCESS")
                {
                    DataRow[] dr = DtabAttachment.Select("DOCTYPENAME = 'PROFILE'");
                    if (dr.Length > 0)
                    {
                        dr[0]["SRNO"] = 0;
                        dr[0]["ATTACHMENT"] = Property.EMPPHOTO;
                        dr[0]["DOCTYPENAME"] = "PROFILE";
                    }
                    else
                    {
                        string Str = PicEmpPhoto.ImageLocation;
                        DataRow DRA = DtabAttachment.NewRow();
                        DRA["SRNO"] = 0;
                        DRA["DOCTYPENAME"] = "PROFILE";
                        DRA["ATTACHMENT"] = Property.EMPPHOTO;
                        DRA["ATTACHMENTFILENAME"] = "EmployeeProfileImage";
                        DtabAttachment.Rows.Add(DRA);
                    }
                    //Property.LEDGER_ID = Val.ToInt64(Property.ReturnValue);
                    Property.LEDGER_ID = Guid.Parse(Val.ToString(Property.ReturnValue));
                    Property = ObjMast.SaveLedgerDetailInfo(Property, DtabExperience, DtabFamily, DtabReference, DtabAttachment);
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
            txtLedgerCode.Text = Val.ToString(DR["LEDGERCODE"]);

            if (!Val.ToString(DR["LEDGERNAME"]).Trim().Equals(string.Empty))
                ChangeTextBoxPlaceHolder(txtLedgerName, txtLedgerName.Text, Val.ToString(DR["LEDGERNAME"]), Color.Black);
            else
                ChangeTextBoxPlaceHolder(txtLedgerName, txtLedgerName.Text, "Name", Color.Silver);

            //txtLedgerName.Text = Val.ToString(DR["LEDGERNAME"]);
            txtLedgerID.Text = Val.ToString(DR["LEDGER_ID"]);

            txtCompany.Text = Val.ToString(DR["COMPANYNAME"]);

            txtDepartment.Text = Val.ToString(DR["DEPARTMENTNAME"]);
            txtDepartment.Tag = Val.ToString(DR["DEPARTMENT_ID"]);

            txtDesignation.Text = Val.ToString(DR["DESIGNATIONNAME"]);
            txtDesignation.Tag = Val.ToString(DR["DESIGNATION_ID"]);

            txtMobileNo1.Text = Val.ToString(DR["MOBILENO1"]);
            txtMobileNo2.Text = Val.ToString(DR["MOBILENO2"]);
            txtLandlineNo.Text = Val.ToString(DR["LANDLINENO"]);

            if (!Val.ToString(DR["BILLINGADDRESS1"]).Trim().Equals(string.Empty))
                ChangeTextBoxPlaceHolder(txtBAddress1, txtBAddress1.Text, Val.ToString(DR["BILLINGADDRESS1"]), Color.Black);
            else
                ChangeTextBoxPlaceHolder(txtBAddress1, txtBAddress1.Text, "Address 1", Color.Silver);

            if (!Val.ToString(DR["BILLINGADDRESS2"]).Trim().Equals(string.Empty))
                ChangeTextBoxPlaceHolder(txtBAddress2, txtBAddress2.Text, Val.ToString(DR["BILLINGADDRESS2"]), Color.Black);
            else
                ChangeTextBoxPlaceHolder(txtBAddress2, txtBAddress2.Text, "Address 2", Color.Silver);

            if (!Val.ToString(DR["BILLINGADDRESS3"]).Trim().Equals(string.Empty))
                ChangeTextBoxPlaceHolder(txtBAddress3, txtBAddress3.Text, Val.ToString(DR["BILLINGADDRESS3"]), Color.Black);
            else
                ChangeTextBoxPlaceHolder(txtBAddress3, txtBAddress3.Text, "Address 3", Color.Silver);

            txtBState.Text = Val.ToString(DR["BILLINGSTATE"]);
            txtBCity.Text = Val.ToString(DR["BILLINGCITY"]);
            txtBCountry.Text = Val.ToString(DR["BILLINGCOUNTRYNAME"]);
            txtBCountry.Tag = Val.ToString(DR["BILLINGCOUNTRY_ID"]);
            txtBZipCode.Text = Val.ToString(DR["BILLINGZIPCODE"]);
            

            if (!Val.ToString(DR["SHIPPINGADDRESS1"]).Trim().Equals(string.Empty))
                ChangeTextBoxPlaceHolder(txtSAddress1, txtSAddress1.Text, Val.ToString(DR["SHIPPINGADDRESS1"]), Color.Black);
            else
                ChangeTextBoxPlaceHolder(txtSAddress1, txtSAddress1.Text, "Address 1", Color.Silver);

            if (!Val.ToString(DR["SHIPPINGADDRESS2"]).Trim().Equals(string.Empty))
                ChangeTextBoxPlaceHolder(txtSAddress2, txtSAddress2.Text, Val.ToString(DR["SHIPPINGADDRESS2"]), Color.Black);
            else
                ChangeTextBoxPlaceHolder(txtSAddress2, txtSAddress2.Text, "Address 2", Color.Silver);

            if (!Val.ToString(DR["SHIPPINGADDRESS3"]).Trim().Equals(string.Empty))
                ChangeTextBoxPlaceHolder(txtSAddress3, txtSAddress3.Text, Val.ToString(DR["SHIPPINGADDRESS3"]), Color.Black);
            else
                ChangeTextBoxPlaceHolder(txtSAddress3, txtSAddress3.Text, "Address 3", Color.Silver);
            txtSState.Text = Val.ToString(DR["SHIPPINGSTATE"]);
            txtSCity.Text = Val.ToString(DR["SHIPPINGCITY"]);
            txtSCountry.Text = Val.ToString(DR["SHIPPINGCOUNTRYNAME"]);
            txtSCountry.Tag = Val.ToString(DR["SHIPPINGCOUNTRY_ID"]);
            txtSZipCode.Text = Val.ToString(DR["SHIPPINGZIPCODE"]);

            txtWeChatID.Text = Val.ToString(DR["WECHATID"]);
            txtEmailID.Text = Val.ToString(DR["EMAILID"]);
            txtQQID.Text = Val.ToString(DR["QQID"]);
            txtSkypeID.Text = Val.ToString(DR["SKYPEID"]);
            txtWebsite.Text = Val.ToString(DR["WEBSITE"]);

            ChkAllowWebLogin.Checked = Val.ToBoolean(DR["ISALLOWWEBLOGIN"]);

            ChkAllowDefDiscDiff.Checked = Val.ToBoolean(DR["ISDEFAULTDISCOUNTDIFF"]);
            txtDefDiscDiff.Text = Val.ToString(DR["DEFAULTDISCOUNTDIFF"]);

            ChkAllowMemberDisc.Checked = Val.ToBoolean(DR["ISMEMBERDISCOUNT"]);
            txtMemberDisc.Text = Val.ToString(DR["MEMBERDISCOUNT"]);

            txtDefaultSeller.Text = Val.ToString(DR["DEFAULTSELLERNAME"]);
            txtDefaultSeller.Tag = Val.ToString(DR["DEFAULTSELLER_ID"]);

            CmbStatus.SelectedItem = Val.ToString(DR["STATUS"]);
            CmbGender.SelectedItem = Val.ToString(DR["GENDER"]);

            txtRemark.Text = Val.ToString(DR["REMARK"]);

            txtUserName.Text = Val.ToString(DR["USERNAME"]);
            txtPassword.Text = Val.ToString(DR["PASSWORD"]);

            txtLoacation.Text = Val.ToString(DR["LOCATIONNAME"]);
            txtLoacation.Tag = Val.ToString(DR["LOCATION_ID"]);

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
            txtLedgerName.Focus();
        }
        public void FillLedgerDetailInfo(string IntLedger_ID)
        {
            //Ledger Details
            Guid gLedger_ID = Val.ToString(IntLedger_ID).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(IntLedger_ID));
            DataSet DsDetailInfo = ObjMast.GetledgerDetailDInfoata(gLedger_ID);

            //Attachment Details
            DtabAttachment = DsDetailInfo.Tables[0];

            if (DtabAttachment.Rows.Count > 0)
            {
                //Display Profile Image
                DataRow[] DR = DtabAttachment.Select("DOCTYPENAME ='PROFILE'");
                if (DR.Length > 0)
                {
                    byte[] OFFICELOGO = DR[0]["ATTACHMENT"] as byte[] ?? null;
                    if (OFFICELOGO != null)
                    {
                        using (MemoryStream ms = new MemoryStream(OFFICELOGO))
                        {
                            PicEmpPhoto.Image = Image.FromStream(ms);
                        }
                    }
                    else
                    {
                        PicEmpPhoto.Image = null;
                    }

                }
                else
                {
                    PicEmpPhoto.Image = null;
                }


                DataView dv = DtabAttachment.DefaultView;
                dv.RowFilter = "DOCTYPENAME <> 'PROFILE'";
                DtabAttachment = dv.ToTable();

                if (DtabAttachment.Rows.Count > 0)
                {
                    int MaxSrNo = 0;
                    MaxSrNo = (int)DtabAttachment.Compute("Max(SRNO)", "");
                    DataRow Dr = DtabAttachment.NewRow();
                    Dr["SRNO"] = MaxSrNo + 1;
                    DtabAttachment.Rows.Add(Dr);
                    DtabAttachment.AcceptChanges();
                }
                else
                {
                    DataRow Dr = DtabAttachment.NewRow();
                    Dr["SRNO"] = 1;
                    DtabAttachment.Rows.Add(Dr);
                    DtabAttachment.AcceptChanges();
                }

                //Delete Profile Image from DataTable

            }
            else
            {
                DataRow Dr = DtabAttachment.NewRow();
                Dr["SRNO"] = 1;
                DtabAttachment.Rows.Add(Dr);
                DtabAttachment.AcceptChanges();
                PicEmpPhoto.Image = null;
            }
            MainGrdAttachment.DataSource = DtabAttachment;
            MainGrdAttachment.RefreshDataSource();
            //End : Ledger Details : 06-04-2019

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

        private void lblBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog OpenDialog = new OpenFileDialog();
            if (OpenDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (PicEmpPhoto.Image != null)
                {
                    PicEmpPhoto.Image.Dispose();
                    PicEmpPhoto.Image = null;
                }
                PicEmpPhoto.Image = Image.FromFile(OpenDialog.FileName);
            }
            OpenDialog.Dispose();
            OpenDialog = null;
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

        private void repBtnBrowse_Click(object sender, EventArgs e)
        {
            DataRow DRow = GrdAttachment.GetFocusedDataRow();

            OpenFileDialog OpenDg = new OpenFileDialog();
            if (OpenDg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                byte[] ba = File.ReadAllBytes(OpenDg.FileName);
                //MemoryStream ms = new MemoryStream();
                //ms.Position = 0;
                //ms.Read(ba, 0, ba.Length);

                byte[] file;
                using (var stream = new FileStream(OpenDg.FileName, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = new BinaryReader(stream))
                    {
                        file = reader.ReadBytes((int)stream.Length);
                    }
                }




                DRow["ATTACHMENTFILENAME"] = OpenDg.SafeFileName;
                DRow["ATTACHMENT"] = file;
                //DRow["FILEPATH"] = OpenDg.FileName;
                //DRow["FILENAME"] = @"";
                //DtabAttachment.Rows.Add(DRow);
                DtabAttachment.AcceptChanges();


                //----------  Add New Row --------------//
                if (!Val.ToString(DRow["ATTACHMENTFILENAME"]).Equals(string.Empty) && GrdAttachment.IsLastRow)
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
                    //BtnSave.Focus();
                    //e.Handled = true;
                }

            }
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

        private void repBtnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                DataRow dr = GrdAttachment.GetFocusedDataRow();

                if (Val.ToString(dr["ATTACHMENTFILENAME"]).Trim().Equals(string.Empty))
                    return;

                byte[] BT = (byte[])dr["ATTACHMENT"];
                string LoadPath = "";
                SaveFileDialog Savedlg = new SaveFileDialog();
                Savedlg.Title = "Save File";

                string StrExtension = Path.GetExtension(Val.ToString(dr["ATTACHMENTFILENAME"]));

                Savedlg.FileName = Val.ToString(dr["ATTACHMENTFILENAME"]);
                if (Savedlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (Val.ToString(Path.GetExtension(Savedlg.FileName)).Trim().Equals(string.Empty))
                    {
                        LoadPath = Val.ToString(Savedlg.FileName) + StrExtension;
                    }
                    else
                    {
                        LoadPath = Val.ToString(Savedlg.FileName);//.Replace(Path.GetExtension(Savedlg.FileName), StrExtension);
                    }

                    File.WriteAllBytes(LoadPath, BT);
                    System.Diagnostics.Process.Start(LoadPath, "CMD");
                }
                Savedlg.Dispose();
                Savedlg = null;
                //File.Open(LoadPath, FileMode.Open);
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
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
                DataTable DtabLedger = ObjMast.GetDataForLedgerPrint(Val.ToString(txtLedgerID.Tag), Val.ToString(CmbPartyType.SelectedItem));
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

        private void lblWebCam_Click(object sender, EventArgs e)
        {
            try
            {
                int index = cboDevices.SelectedIndex;
                if (index != -1)
                {

                    ((BOCaptureDevice)cboDevices.SelectedItem).Attach(PicEmpPhoto);
                }

            }
            catch (Exception ex)
            {
                Global.MessageError(ex.ToString());
            }
        }

        private void lblCapture_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboDevices.SelectedItem == null)
                {
                    Global.MessageError("Capture Device Not Found.");
                    return;
                }
                if (((BOCaptureDevice)cboDevices.SelectedItem).Capture() != null)
                {

                    Image image = ((BOCaptureDevice)cboDevices.SelectedItem).Capture();
                    PicEmpPhoto.Image = image;
                    ((BOCaptureDevice)cboDevices.SelectedItem).Detach();
                }
                else
                {
                    Global.MessageError("Image Not Captured.");
                }

            }
            catch (Exception ex)
            {
                Global.MessageError(ex.ToString());
            }
        }

        private void txtBCountry_KeyPress(object sender, KeyPressEventArgs e)
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
                        txtBCountry.Text = Val.ToString(FrmSearch.DRow["COUNTRYNAME"]);
                        txtBCountry.Tag = Val.ToString(FrmSearch.DRow["COUNTRY_ID"]);
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

        private void txtBAddress1_Leave(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtBAddress1, "", "Address 1", Color.Silver);
        }

        private void txtBAddress1_Enter(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtBAddress1, "Address 1", "", Color.Black);
        }

        private void txtBAddress2_Enter(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtBAddress2, "Address 2", "", Color.Black);
        }

        private void txtBAddress2_Leave(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtBAddress2, "", "Address 2", Color.Silver);
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

        private void txtBAddress3_Leave(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtBAddress3, "", "Address 3", Color.Silver);
        }

        private void txtBAddress3_Enter(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtBAddress3, "Address 3", "", Color.Black);
        }

        private void txtSAddress1_Enter(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtSAddress1, "Address 1", "", Color.Black);
        }

        private void txtSAddress1_Leave(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtSAddress1, "", "Address 1", Color.Silver);
        }

        private void txtSAddress2_Enter(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtSAddress2, "Address 2", "", Color.Black);
        }

        private void txtSAddress2_Leave(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtSAddress2, "", "Address 2", Color.Silver);
        }

        private void txtSAddress3_Leave(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtSAddress3, "", "Address 3", Color.Silver);
        }

        private void txtSAddress3_Enter(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtSAddress3, "Address 3", "", Color.Black);
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

        private void lblSameAsBilling_Click(object sender, EventArgs e)
        {
            try
            {
                ChangeTextBoxPlaceHolder(txtSAddress1, Val.ToString(txtSAddress1.Text), txtBAddress1.Text, txtBAddress1.ForeColor);
                ChangeTextBoxPlaceHolder(txtSAddress2, Val.ToString(txtSAddress2.Text), txtBAddress2.Text, txtBAddress2.ForeColor);
                ChangeTextBoxPlaceHolder(txtSAddress3, Val.ToString(txtSAddress3.Text), txtBAddress3.Text, txtBAddress3.ForeColor);

                txtSState.Text = txtBState.Text;
                txtSCity.Text = txtBCity.Text;

                txtSCountry.Text = txtBCountry.Text;
                txtSCountry.Tag = txtBCountry.Tag;

                txtSZipCode.Text = txtBZipCode.Text;
            }
            catch(Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void CmbPartyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            Clear();
        }

        private void BtnEmail_Click(object sender, EventArgs e)
        {
            if (txtLedgerName.Text.Trim() == "")
            {
                Global.Message("Ledger Name Is Required");
                return;
            }
            this.Cursor = Cursors.WaitCursor;
            string Str = new Class.CommonEmail().RegistrationApproval(txtLedgerName.Text,txtEmailID.Text, txtUserName.Text, Val.ToString(CmbStatus.SelectedItem),"");
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
            this.Cursor = Cursors.WaitCursor;
            txtExcRate.Text = new BOTRN_MemoEntry().GetExchangeRate(Val.ToInt(txtCurrency.Tag), Val.SqlDate(DateTime.Now.ToShortDateString()), "").ToString();
            this.Cursor = Cursors.Default;
        }

        private void txtLedgerName_Leave(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtLedgerName, "", "Name", Color.Silver);
        }

        private void txtLedgerName_Enter(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtLedgerName, "Name", "", Color.Black);
        }

        private void txtLoacation_KeyPress(object sender, KeyPressEventArgs e)
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
                        txtLoacation.Text = Val.ToString(FrmSearch.DRow["LOCATIONNAME"]);
                        txtLoacation.Tag = Val.ToString(FrmSearch.DRow["LOCATION_ID"]);
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

    }
}
