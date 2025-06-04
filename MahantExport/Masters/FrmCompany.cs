using BusLib;
using BusLib.Configuration;
using BusLib.EInvoice;
using BusLib.Master;
using BusLib.TableName;
using DevExpress.XtraPrinting;
using Google.API.Translate;
using MahantExport.Utility;
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
    public partial class FrmCompany : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOMST_Ledger ObjMast = new BOMST_Ledger();
        BOFormPer ObjPer = new BOFormPer();

        #region Property Settings

        public FrmCompany()
        {
            InitializeComponent();
        }
        public void ShowForm(string pStrFormType)
        {
            ObjPer.GetFormPermission(Val.ToString(this.Tag));
            BtnSave.Enabled = ObjPer.ISINSERT;
            BtnDelete.Enabled = ObjPer.ISDELETE;

            CmbLedgerType.SelectedItem = "COMPANY";
            CmbLedgerType.Enabled = false;

            //lblAccID.Text = "Comp ID";
            lblAccGrp.Text = "Comp Grp";
            lblAccCode.Text = "Comp Code";
        
            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();

            BtnAdd_Click(null, null);
            Fill();
            this.Show();

            txtSCity.Text = "SURAT";
            txtBCity.Text = "MUMBAI";

            txtBDistrictcode.Text = "483";
            txtSDistrictcode.Text = "459";

            txtLedgerName.Focus();

        }

        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = false;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(ObjMast);
            ObjFormEvent.ObjToDisposeList.Add(Val);
            ObjFormEvent.ObjToDisposeList.Add(ObjPer);
            
        }

        #endregion

        #region Validation

        private bool ValSave()
        {

            //if (txtLedgerName.Text.Trim().Length == 0)

            if (txtLedgerName.ForeColor == Color.Silver)
            {
                Global.Message("Company Name Is Required");
                txtLedgerName.Focus();
                return false;
            }
            else if (txtLedgerCode.Text.Trim().Length == 0)
            {
                Global.Message("Company Code Is Required");
                txtLedgerCode.Focus();
                return false;
            }

            //else if (Val.ToString(CmbLedgerType.SelectedItem) == "PURCHASE" ||
            //    Val.ToString(CmbLedgerType.SelectedItem) == "SALE" ||
            //    Val.ToString(CmbLedgerType.SelectedItem) == "COMPANY"
            //    )
            //{

            //    if (txtBState.Text.Trim().Length == 0)
            //    {
            //        Global.Message("Billing State Is Required");
            //        txtBState.Focus();
            //        return false;
            //    }
            //}

            return true;
        }

        private bool ValDelete()
        {
            if (txtLedgerName.Text.Trim().Length == 0)
            {
                Global.Message("Company Code Is Required");
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



        #endregion

        private void BtnAdd_Click(object sender, EventArgs e)
        {

            txtLedgerCode.Text = Val.ToString(ObjMast.FindMaxLedgerCode());
            //txtLedgerCode.Text = string.Empty;
            txtLedgerCode.Enabled = false;

            ChangeTextBoxPlaceHolder(txtLedgerName, txtLedgerName.Text, "Name", Color.Silver);
            //txtLedgerName.Text = string.Empty;
            txtLedgerName.Tag = string.Empty;
            
            CmbStatus.SelectedIndex = 0;

            txtEInvClientID.Text = string.Empty;
            txtClientSecret.Text = string.Empty;
            txtEInvGSTIN.Text = string.Empty;
            txtEInvUsername.Text = string.Empty;
            txtEInvPassword.Text = string.Empty;
            txtEInvURL.Text = string.Empty;
            txtEInvTokenURL.Text = string.Empty;

            txtContactPer.Text = string.Empty;
            txtDepartment.Text = string.Empty;
            txtDepartment.Tag = string.Empty;

            txtEmailID.Text = string.Empty;
            ChangeTextBoxPlaceHolder(txtEmailID, "", "Primary@Email.com", Color.Silver);

            txtQQID.Text = string.Empty;
            txtWebsite.Text = string.Empty;
            txtSkypeID.Text = string.Empty;

            txtMobileNo1.Text = string.Empty;
            txtMobileNo2.Text = string.Empty;
            txtLandlineNo.Text = string.Empty;
            txtPANNo.Text = string.Empty;
            txtBGSTNo.Text = string.Empty;
            txtIECode.Text = string.Empty;

            txtBAddress1.Text = string.Empty;
            txtBAddress2.Text = string.Empty;
            txtBAddress3.Text = string.Empty;
            ChangeTextBoxPlaceHolder(txtBAddress1, "", "Address 1", Color.Silver);
            ChangeTextBoxPlaceHolder(txtBAddress2, "", "Address 2", Color.Silver);
            ChangeTextBoxPlaceHolder(txtBAddress3, "", "Address 3", Color.Silver);
            txtBState.Text = string.Empty;
            txtBCountry.Text = string.Empty;
            txtBCountry.Tag = string.Empty;
            txtBCity.Text = "MUMBAI";
            txtBZipCode.Text = string.Empty;


            txtSAddress1.Text = string.Empty;
            txtSAddress2.Text = string.Empty;
            txtSAddress3.Text = string.Empty;
            ChangeTextBoxPlaceHolder(txtSAddress1, "", "Address 1", Color.Silver);
            ChangeTextBoxPlaceHolder(txtSAddress2, "", "Address 2", Color.Silver);
            ChangeTextBoxPlaceHolder(txtSAddress3, "", "Address 3", Color.Silver);
            txtSState.Text = string.Empty;
            txtSCountry.Text = string.Empty;
            txtSCountry.Tag = string.Empty;
            txtSCity.Text = "SURAT";
            txtSZipCode.Text = string.Empty;

            txtRemark.Text = string.Empty;
            txtLedgerName.Focus();

            DtpGrDate.Checked = false;
            DtpGrDate.Value = DateTime.Now;
           
            txtGrNo.Text = string.Empty;
            txtArNo.Text = string.Empty;

            TxtSGstNo.Text = string.Empty;
            txtSPlaceOfReceiptByPreCurrier.Text = string.Empty;
            txtBPlaceOfReceiptByPreCurrier.Text = string.Empty;
            txtBDistrictcode.Text = "483";
            txtSDistrictcode.Text = "459";

            //Add shiv 21-05-2022
            txtCompDecl.Text = string.Empty;
            txtDDADec.Text = string.Empty;
            txtExportDec.Text = string.Empty;

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

                Property.LEDGER_ID = Val.ToString(txtLedgerName.Tag).Trim().Equals(string.Empty) ? BusLib.Configuration.BOConfiguration.FindNewSequentialID() : Guid.Parse(Val.ToString(txtLedgerName.Tag));
                Property.LEDGERCODE = Val.ToInt32(txtLedgerCode.Text);
                Property.LEDGERNAME = txtLedgerName.Text;
                Property.LEDGERTYPE = Val.ToString(CmbLedgerType.SelectedItem);

                Property.CONTACTPER = txtContactPer.Text;
                Property.DEPARTMENT_ID = Val.ToInt32(txtDepartment.Tag);

                //Property.EMAILID = Val.ToString(txtEmailID.Text);
                Property.EMAILID = txtEmailID.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtEmailID.Text);

                Property.QQID = Val.ToString(txtQQID.Text);
                Property.SKYPEID = Val.ToString(txtSkypeID.Text);
                Property.WEBSITE = Val.ToString(txtWebsite.Text);

                Property.MOBILENO1 = Val.ToString(txtMobileNo1.Text);
                Property.MOBILENO2 = Val.ToString(txtMobileNo2.Text);
                Property.LANDLINENO = Val.ToString(txtLandlineNo.Text);

                Property.REMARK = Val.ToString(txtRemark.Text);

                Property.LOCATION_ID = Val.ToInt32(txtLoacation.Tag);

                Property.STATUS = Val.ToString(CmbStatus.SelectedItem);

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

                //Property.SHIPPINGADDRESS1 = txtBAddress1.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtBAddress1.Text);
                //Property.SHIPPINGADDRESS2 = txtBAddress2.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtBAddress2.Text);
                //Property.SHIPPINGADDRESS3 = txtBAddress3.ForeColor == Color.Silver ? string.Empty : Val.ToString(txtBAddress3.Text);
                //Property.SHIPPINGSTATE = Val.ToString(txtBState.Text);
                //Property.SHIPPINGCOUNTRY_ID = Val.ToInt32(txtBCountry.Tag);
                //Property.SHIPPINGCITY = Val.ToString(txtBCity.Text);
                //Property.SHIPPINGZIPCODE = Val.ToString(txtBZipCode.Text);

                Property.PANNO = Val.ToString(txtPANNo.Text);
                Property.GSTNO = Val.ToString(txtBGSTNo.Text);
                Property.IECODE = Val.ToString(txtIECode.Text);

                Property.GRFORMNO = Val.ToString(txtGrNo.Text);
                if(DtpGrDate.Checked == true)
                {
                    Property.GRFORMDATE = Val.SqlDate(DtpGrDate.Value.ToShortDateString());
                }
                else
                {
                    Property.GRFORMDATE = null;
                }
                Property.ARNO = Val.ToString(txtArNo.Text);
                
                // ADD #D : 19-08-2020

                Property.BILLINGDISTRICTCODE = Val.ToInt32(txtBDistrictcode.Text);
                Property.BILLINGPLACEOFRECEIPTBYPRECARRIER = Val.ToString(txtBPlaceOfReceiptByPreCurrier.Text);
                Property.SHIPPINGGSTNO = Val.ToString(TxtSGstNo.Text);
                Property.SHIPPINGDISTRICTCODE = Val.ToInt32(txtSDistrictcode.Text);
                Property.SHIPPINGPLACEOFRECEIPTBYPRECARRIER = Val.ToString(txtSPlaceOfReceiptByPreCurrier.Text);

                // END #D : 19-08-2020


                //EInvoice

                Property.EINV_CLIENTID = txtEInvClientID.Text;
                Property.EINV_CLIENTSECRET = txtClientSecret.Text;
                Property.EINV_GSTIN = txtEInvGSTIN.Text;
                Property.EINV_USERNAME = txtEInvUsername.Text;
                Property.EINV_PASSWORD = txtEInvPassword.Text;
                Property.EINV_URL = txtEInvURL.Text;
                Property.EINV_TOKENURL = txtEInvTokenURL.Text;

                //Add Shiv 21-05-2022
                Property.COMPDEC = Val.ToString(txtCompDecl.Text);
                Property.DDADEC = Val.ToString(txtDDADec.Text);
                Property.EXPORTDEC = Val.ToString(txtExportDec.Text);

                Property = ObjMast.Save(Property);

                Global.Message(Property.ReturnMessageDesc);

                if (Property.ReturnMessageType == "SUCCESS")
                {
                    Fill();
                    BtnAdd_Click(null, null);

                    if (GrdDet.RowCount > 1)
                    {
                        GrdDet.FocusedRowHandle = GrdDet.RowCount - 1;
                    }
                }
                else
                {
                    txtLedgerName.Focus();
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        public void Fill()
        {
            string StrLedgerGroup = "";
            if (this.Text == "COMPANY MASTER")
            {
                StrLedgerGroup = "COMPANY";
                GrdDet.Columns["LEDGERNAME"].Caption = "Company Name";
                GrdDet.Columns["DEPARTMENTNAME"].Visible = false;
            }
            else if (this.Text == "EMPLOYEE MASTER")
            {
                StrLedgerGroup = "EMPLOYEE";
                GrdDet.Columns["LEDGERNAME"].Caption = "Employee Name";
                GrdDet.Columns["DEPARTMENTNAME"].Visible = true;
            }
            else
            {
                StrLedgerGroup = "OTHER";
                GrdDet.Columns["DEPARTMENTNAME"].Visible = false;
            }

            DataTable DTab = ObjMast.Fill(StrLedgerGroup, "ALL");
            MainGrid.DataSource = DTab;

            lblTotal.Text = "Total Record : " + DTab.Rows.Count.ToString();

            MainGrid.Refresh();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            LedgerMasterProperty Property = new LedgerMasterProperty();
            try
            {

                if (Val.ToString(txtLedgerName.Text).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Select Records From Search Panel That You Want To Delete");
                    return;
                }

                if (Global.Confirm("Are Your Sure To Delete The Record ?") == System.Windows.Forms.DialogResult.No)
                    return;

               FrmPassword FrmPassword = new FrmPassword();
               if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.Yes)
               {

                   Property.LEDGER_ID = Guid.Parse(Val.ToString(txtLedgerName.Tag));
                   Property = ObjMast.Delete(Property);
                   Global.Message(Property.ReturnMessageDesc);

                   if (Property.ReturnMessageType == "SUCCESS")
                   {
                       BtnAdd_Click(null, null);
                       Fill();
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

        private void GrdDet_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                return;
            }

            if (e.Clicks == 2)
            {
                DataRow DR = GrdDet.GetDataRow(e.RowHandle);
                FetchValue(DR);
                DR = null;
            }

        }
        private void GrdDet_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DataRow DR = GrdDet.GetFocusedDataRow();
                FetchValue(DR);
                DR = null;
            }
        }


        public void FetchValue(DataRow DR)
        {
            CmbLedgerType.SelectedItem = Val.ToString(DR["LEDGERTYPE"]);
            txtLedgerCode.Text = Val.ToString(DR["LEDGERCODE"]);
            //txtLedgerName.Text = Val.ToString(DR["LEDGERNAME"]);

            if (!Val.ToString(DR["LEDGERNAME"]).Trim().Equals(string.Empty))
                ChangeTextBoxPlaceHolder(txtLedgerName, txtLedgerName.Text, Val.ToString(DR["LEDGERNAME"]), Color.Black);
            else
                ChangeTextBoxPlaceHolder(txtLedgerName, txtLedgerName.Text, "Name", Color.Silver);

            txtLedgerName.Tag = Val.ToString(DR["LEDGER_ID"]);

            txtContactPer.Text = Val.ToString(DR["CONTACTPER"]);

            txtDepartment.Text = Val.ToString(DR["DEPARTMENTNAME"]);
            txtDepartment.Tag = Val.ToString(DR["DEPARTMENT_ID"]);

            if (!Val.ToString(DR["EMAILID"]).Trim().Equals(string.Empty))
                ChangeTextBoxPlaceHolder(txtEmailID, txtEmailID.Text, Val.ToString(DR["EMAILID"]), Color.Black);
            else
                ChangeTextBoxPlaceHolder(txtEmailID, txtEmailID.Text, "Primary@Email.com", Color.Silver);

            txtQQID.Text = Val.ToString(DR["QQID"]);
            txtSkypeID.Text = Val.ToString(DR["SKYPEID"]);
            txtWebsite.Text = Val.ToString(DR["WEBSITE"]);

            txtMobileNo1.Text = Val.ToString(DR["MOBILENO1"]);
            txtMobileNo2.Text = Val.ToString(DR["MOBILENO2"]);
            txtLandlineNo.Text = Val.ToString(DR["LANDLINENO"]);
            txtBGSTNo.Text = Val.ToString(DR["GSTNO"]);
            txtPANNo.Text = Val.ToString(DR["PANNO"]);
            txtIECode.Text = Val.ToString(DR["IECODE"]);

            txtRemark.Text = Val.ToString(DR["REMARK"]);

            CmbStatus.SelectedItem = Val.ToString(DR["STATUS"]);

            if(!Val.ToString(DR["BILLINGADDRESS1"]).Trim().Equals(string.Empty))
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


            //txtBAddress1.Text = Val.ToString(DR["BILLINGADDRESS1"]);
            //txtBAddress2.Text = Val.ToString(DR["BILLINGADDRESS2"]);
            //txtBAddress3.Text = Val.ToString(DR["BILLINGADDRESS3"]);
            txtBState.Text = Val.ToString(DR["BILLINGSTATE"]);
            txtBCountry.Text = Val.ToString(DR["BILLINGCOUNTRYNAME"]);
            txtBCountry.Tag = Val.ToString(DR["BILLINGCOUNTRY_ID"]);
            txtBCity.Text = Val.ToString(DR["BILLINGCITY"]);
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

            //txtSAddress1.Text = Val.ToString(DR["SHIPPINGADDRESS1"]);
            //txtSAddress2.Text = Val.ToString(DR["SHIPPINGADDRESS2"]);
            //txtSAddress3.Text = Val.ToString(DR["SHIPPINGADDRESS3"]);
            txtSState.Text = Val.ToString(DR["SHIPPINGSTATE"]);
            txtSCountry.Text = Val.ToString(DR["SHIPPINGCOUNTRYNAME"]);
            txtSCountry.Tag = Val.ToString(DR["SHIPPINGCOUNTRY_ID"]);
            txtSCity.Text = Val.ToString(DR["SHIPPINGCITY"]);
            txtSZipCode.Text = Val.ToString(DR["SHIPPINGZIPCODE"]);

            TxtSGstNo.Text = Val.ToString(DR["SHIPPINGGSTNO"]);
            txtBPlaceOfReceiptByPreCurrier.Text = Val.ToString(DR["BillingPlaceofReceiptbyPrecarrier"]);
            txtBDistrictcode.Text = Val.ToString(DR["BillingDistrictCode"]);
            txtSPlaceOfReceiptByPreCurrier.Text = Val.ToString(DR["ShippingPlaceofReceiptbyPrecarrier"]);
            txtSDistrictcode.Text = Val.ToString(DR["ShippingDistrictCode"]);

            txtLoacation.Text = Val.ToString(DR["LOCATIONNAME"]);
            txtLoacation.Tag = Val.ToString(DR["LOCATION_ID"]);

            txtGrNo.Text = Val.ToString(DR["GRFORMNO"]);
            if (Val.ToString(DR["GRFORMDATE"]) != "")
            {
                DtpGrDate.Checked = true;
                DtpGrDate.Text = Val.ToString(DR["GRFORMDATE"]);
            }
            else
                DtpGrDate.Checked = false;

            txtArNo.Text = Val.ToString(DR["ARNNO"]);


            //Add shiv 21-05-2022
            txtCompDecl.Text = Val.ToString(DR["COMPDEC"]);
            txtDDADec.Text = Val.ToString(DR["DDADEC"]);
            txtExportDec.Text = Val.ToString(DR["EXPORTDEC"]);

            TRN_EInvoiceProperty Property =  new BOTRN_EInvoice().GetEInvoiceCredential(Guid.Parse(Val.ToString(txtLedgerName.Tag)));
            
            txtEInvClientID.Text =  Property.CLIENTID ;
            txtClientSecret.Text = Property.CLIENTSECRET ;
            txtEInvGSTIN.Text = Property.GSTIN;
            txtEInvUsername.Text = Property.USERNAME;
            txtEInvPassword.Text = Property.PASSWORD;
            txtEInvURL.Text = Property.URL;
            txtEInvTokenURL.Text = Property.TOKENURL;

            xtraTabControl1.SelectedTabPageIndex = 0;
            txtLedgerName.Focus();
        }

        private void txtLedgerCode_Validated(object sender, EventArgs e)
        {
            string StrGroup = string.Empty;
            if (this.Text == "COMPANY MASTER")
            {
                StrGroup = "COMPANY";
            }
            else if (this.Text == "ACCOUNT MASTER")
            {
                StrGroup = "OTHER";
            }

            string Str = txtLedgerCode.Text;

            BtnAdd_Click(null, null);

            txtLedgerCode.Text = Str;
            DataRow DRow = ObjMast.GetLedgerInfoByCode(StrGroup, txtLedgerCode.Text, Guid.Empty);
            if (DRow != null)
            {
                FetchValue(DRow);
            }

            txtLedgerName.Focus();
        }

        public string Transliterate(string latinCharacters)
        {
            StringBuilder gujarati = new StringBuilder(latinCharacters.Length);
            for (int i = 0; i < latinCharacters.Length; i++)
            {
                switch (char.ToLower(latinCharacters[i]))
                {
                    case 'a':
                        gujarati.Append('\u0abe');
                        break;
                    case 'h':
                        gujarati.Append('\u0ab9');
                        break;
                    case 'j':
                        gujarati.Append('\u0a9c');
                        break;
                    case 'l':
                        gujarati.Append('\u0ab2');
                        break;
                    case 'm':
                        gujarati.Append('\u0aae');
                        break;
                    case 't':
                        gujarati.Append('\u0aa4');
                        break;
                }
            }
            return gujarati.ToString();
        }

        private void lblSameAsBilling_Click(object sender, EventArgs e)
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

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("LedgerList", GrdDet);
        }

        private void BtnBestFit_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            GrdDet.BestFitColumns();
            this.Cursor = Cursors.Default;
        }

        private void txtPrimaryEmail_Leave(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtEmailID, "", "Primary@Email.com", Color.Silver);
        }

        private void txtPrimaryEmail_Enter(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtEmailID, "Primary@Email.com", "", Color.Black);
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
        }

        #region PlaceHolder Events
        private void txtBAddress1_Leave(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtBAddress1, "", "Address 1", Color.Silver);
        }

        private void txtBAddress1_Enter(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtBAddress1, "Address 1", "", Color.Black);
        }

        private void txtBAddress2_Leave(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtBAddress2, "", "Address 2", Color.Silver);
        }

        private void txtBAddress2_Enter(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtBAddress2, "Address 2", "", Color.Black);
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

        private void txtSAddress3_Enter(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtSAddress3, "Address 3", "", Color.Black);
        }

        private void txtSAddress3_Leave(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtSAddress3, "", "Address 3", Color.Silver);
        }

        #endregion PlaceHolder Events

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

        private void txtLedgerName_Leave(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtLedgerName, "", "Name", Color.Silver);
        }

        private void txtLedgerName_Enter(object sender, EventArgs e)
        {
            ChangeTextBoxPlaceHolder(txtLedgerName, "Name", "", Color.Black);
        }

        private void txtBState_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "STATECODE,STATENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().GetState(Val.ToInt(txtBCountry.Tag));

                    FrmSearch.mStrColumnsToHide = "STATE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBState.Text = Val.ToString(FrmSearch.DRow["STATENAME"]);
                        txtBState.Tag = Val.ToString(FrmSearch.DRow["STATE_ID"]);
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

        private void FrmCompany_Load(object sender, EventArgs e)
        {

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
                        txtSState.Tag = Val.ToString(FrmSearch.DRow["STATE_ID"]);
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
