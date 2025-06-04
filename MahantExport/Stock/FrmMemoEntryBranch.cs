using MahantExport.Class;
using MahantExport.Master;
using MahantExport.Masters;
using MahantExport.Utility;
using BusLib;
using BusLib.Configuration;
using BusLib.Master;
using BusLib.TableName;
using BusLib.Transaction;
using BusLib.Account;
using DevExpress.Data;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.BandedGrid;
using DevExpress.XtraGrid.Views.Grid;
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
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
//Busing BusLib.Rapaport;
using BusLib.EInvoice;
using Microsoft.VisualBasic;
using System.Net;
using System.Web.Script.Serialization;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto.Parameters;
using Microsoft.VisualBasic.CompilerServices;
using QRCoder;
using BarcodeLib.Barcode;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using BusLib.Rapaport;
using System.Data.SqlClient;
using System.Transactions;
//BtnOtherActivity
namespace MahantExport.Stock
{
    public partial class FrmMemoEntryBranch : DevControlLib.cDevXtraForm
    {
        AxonDataLib.BOFormEvents ObjFormEvent = new AxonDataLib.BOFormEvents();
        AxonDataLib.BOConversion Val = new AxonDataLib.BOConversion();
        BOTRN_EInvoice ObjEInvoice = new BOTRN_EInvoice();
        BOFormPer ObjPer = new BOFormPer();
        BOTRN_MemoEntry ObjMemo = new BOTRN_MemoEntry();

        BOACC_FinanceJournalEntry ObjFinance = new BOACC_FinanceJournalEntry();

        DataTable DtabInvoice = new DataTable();

        DataTable DTabMemoDetail = new DataTable();
        DataTable DTabMemoSummury = new DataTable();
        DataTable DTabMemo = new DataTable();
        DataTable DtabPara = new DataTable();
        DataTable DtabProcess = new DataTable();

        string PstrFinalBuyer_ID = "";
        string PstrFinalBuyerName = "";
        string PstrFinalAddress1 = "";
        string PstrFinalAddress2 = "";
        string PstrFinalAddress3 = "";
        string PstrFinalCountry_ID = "";
        string PstrFinalCountryName = "";
        string PstrFinalSatet = "";
        string PstrFinalCity = "";
        string PstrFinalZipCode = "";
        string PstrApprove_MemoId = "";
        string PstrOrder_MemoId = ""; // b part mathi sales ni entry thay tyare pending order ni detail save thase.  
        string PstrOrderJangedNo = "";

        DataTable DtabBackPriceUpdate = new DataTable();

        double DouCarat = 0;
        double DouSaleRapaport = 0;
        double DouSaleRapaportAmt = 0;
        double DouSaleDisc = 0;
        double DouSalePricePerCarat = 0;
        double DouSaleAmount = 0;

        double DouMemoRapaport = 0;
        double DouMemoRapaportAmt = 0;
        double DouMemoDisc = 0;
        double DouMemoPricePerCarat = 0;
        double DouMemoAmount = 0;

        //#K : 15122020
        DataTable DtAccountingEffect = new DataTable();
        DataTable DtBrokerAccountingEffect = new DataTable();
        BODevGridSelection ObjGridSelection;
        string StrMainBillType = "";

        //HINA - START
        string mStrStockType = "", mStrFormTypeHK = "";
        //HINA - END

        //For E-Invoice
        string AuthToken = "";
        string key = "";
        string AckNo = "";
        string IrnNo = "";
        string IrnDate = "";
        string status = "";
        string SignedInvoice = "";
        string SignedQRCode = "";
        string StrMessage = "";
        string StrCancelDate = "";
        int IntIsError = 0;
        int IntAccCnt = 0;

        DataTable DtExportAccountingEffect = new DataTable(); // add Khushbu 18-06-21

        #region Property Settings

        public FrmMemoEntryBranch()
        {
            InitializeComponent();
        }

        public FORMTYPE mFormType = FORMTYPE.SALEINVOICE;
        public enum FORMTYPE
        {
            PURCHASEISSUE = 1,
            PURCHASERETURN = 2,
            MEMOISSUE = 3,
            MEMORETURN = 4, SALEINVOICE = 5,
            SALESDELIVERYRETURN = 6,
            HOLD = 7,
            RELEASE = 8,
            ORDERCONFIRM = 9,
            ORDERCONFIRMRETURN = 10,
            ONLINE = 11,
            OFFLINE = 12,
            LABISSUE = 13,
            LABRETURN = 14,
            CONSIGNMENTISSUE = 15,
            CONSIGNMENTRETURN = 16
        }

        public void ShowForm(FORMTYPE pFormType, DataTable pDtInvoice, string pStockType = "ALL", string StrMainMemo_ID = "")  //Call When Double Click On Current Stock Color Clarity Wise Report Data
        {

            Val.FormGeneralSetting(this);
            AttachFormDefaultEvent();
            this.Show();

            BtnReturn.Enabled = false;
            BtnOtherActivity.Enabled = false;

            mFormType = pFormType;
            mStrStockType = pStockType;
            DataSet DS = ObjMemo.GetMemoListData(-1, null, null, "", "", "", 0, "", 0, "", "", "", mStrStockType, false, 0);

            StrMainMemo_ID = Val.ToString(StrMainMemo_ID).Trim().Equals(string.Empty) ? Val.ToString(Guid.Empty) : StrMainMemo_ID;

            DTabMemo = DS.Tables[0];
            DTabMemoDetail = DS.Tables[1];

            DTabMemoDetail.Columns.Add("OLDMEMODISCOUNT", typeof(double));
            DTabMemoDetail.Columns.Add("OLDMEMOPRICEPERCARAT", typeof(double));
            DTabMemoDetail.Columns.Add("OLDMEMODETAIL_IDFORBRANCHRCV", typeof(Guid));


            MainGrdDetail.DataSource = DTabMemoDetail;
            MainGrdDetail.Refresh();

            cmbAddresstype.SelectedIndex = 1; //#P : 14-07-2021

            lblStockType.Text = mStrStockType + " STOCK";
            if (mStrStockType == "PARCEL")
            {
                GrdDetail.Columns["POLNAME"].Visible = false;
                GrdDetail.Columns["SYMNAME"].Visible = false;
                GrdDetail.Columns["FLNAME"].Visible = false;
                GrdDetail.Columns["LABREPORTNO"].Visible = false;

                if (Val.ToString(lblMode.Text).ToUpper() == "EDIT MODE")
                {
                    GrdDetail.Columns["RETURNPCS"].Visible = true;
                    GrdDetail.Columns["RETURNCARAT"].Visible = true;
                }
                else
                {
                    GrdDetail.Columns["RETURNPCS"].Visible = false;
                    GrdDetail.Columns["RETURNCARAT"].Visible = false;
                }
                GrdDetail.Columns["SALERAPAPORT"].Visible = false;
                GrdDetail.Columns["MEMORAPAPORT"].Visible = false;
                GrdDetail.Columns["SALEDISCOUNT"].Visible = false;
                GrdDetail.Columns["MEMODISCOUNT"].Visible = false;
            }
            else
            {
                GrdDetail.Columns["POLNAME"].Visible = true;
                GrdDetail.Columns["SYMNAME"].Visible = true;
                GrdDetail.Columns["FLNAME"].Visible = true;
                GrdDetail.Columns["LABREPORTNO"].Visible = true;

                GrdDetail.Columns["SALERAPAPORT"].Visible = true;
                GrdDetail.Columns["MEMORAPAPORT"].Visible = true;
                GrdDetail.Columns["MEMORAPAPORT"].Fixed = FixedStyle.Right; //#P
                GrdDetail.Columns["SALEDISCOUNT"].Visible = true;
                GrdDetail.Columns["MEMODISCOUNT"].Visible = true;

                GrdDetail.Columns["RETURNPCS"].Visible = false;
                GrdDetail.Columns["RETURNCARAT"].Visible = false;
            }

            //#P : 05-05-2021: Coz JangedDisc And PerCts prthi Sale Amt Consider thay 6e etle MemoDisc Editable nathi..
            GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
            GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
            //#P : 05-05-2021


            lblSource.Text = "SOFTWARE";
            CmbPaymentMode.SelectedIndex = 1;
            CmbDeliveryType.SelectedIndex = 0;
            //cmbAddresstype.SelectedIndex = 0;
            cmbAddresstype.SelectedIndex = 1; //#P : 14-07-2021
            cmbBillType.SelectedIndex = 0;
            lblMode.Text = "Add Mode";
            FillControlName();

            DTabMemoDetail.Rows.Clear();

            if (pDtInvoice != null)
            {
                DataTable DTabCurr = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CURRENCY);

                foreach (DataRow DRow in DTabCurr.Rows)
                {
                    if (pDtInvoice != null)
                    {
                        if (pFormType == FORMTYPE.SALEINVOICE && Val.ToString(DRow["CURRENCY_ID"]) == pDtInvoice.Rows[0]["INVCURRENCY_ID"].ToString())
                        {
                            txtCurrency.Text = Val.ToString(DRow["CURRENCYNAME"]);
                            txtCurrency.Tag = Val.ToString(DRow["CURRENCY_ID"]);

                            //GrdDetail.Columns["FMEMOPRICEPERCARAT"].Caption = "  " + Val.ToString(DRow["SYMBOL"]) + "/Cts";
                            //GrdDetail.Columns["FMEMOAMOUNT"].Caption = "  Amt (" + Val.ToString(DRow["SYMBOL"]) + ")";
                        }
                        else if (Val.ToString(DRow["CURRENCY_ID"]) == "1")//#P : 09-07-2021 : Coz InvCurr Blank hoy to default Usd consider kare
                        {
                            txtCurrency.Text = Val.ToString(DRow["CURRENCYNAME"]);
                            txtCurrency.Tag = Val.ToString(DRow["CURRENCY_ID"]);

                            //GrdDetail.Columns["FMEMOPRICEPERCARAT"].Caption = "  " + Val.ToString(DRow["SYMBOL"]) + "/Cts";
                            //GrdDetail.Columns["FMEMOAMOUNT"].Caption = "  Amt (" + Val.ToString(DRow["SYMBOL"]) + ")";
                        }
                    }
                    else if (Val.ToString(DRow["CURRENCY_ID"]) == "1")
                    {
                        txtCurrency.Text = Val.ToString(DRow["CURRENCYNAME"]);
                        txtCurrency.Tag = Val.ToString(DRow["CURRENCY_ID"]);

                        //GrdDetail.Columns["FMEMOPRICEPERCARAT"].Caption = "  " + Val.ToString(DRow["SYMBOL"]) + "/Cts";
                        //GrdDetail.Columns["FMEMOAMOUNT"].Caption = "  Amt (" + Val.ToString(DRow["SYMBOL"]) + ")";
                    }
                }
                DTabCurr.Dispose();
                DTabCurr = null;

                if (BOConfiguration.gStrLoginSection == "" && (mFormType == FORMTYPE.SALEINVOICE))
                {
                    foreach (DataRow DRow in pDtInvoice.DefaultView.ToTable(true, "MEMO_ID", "JANGEDNOSTR").Rows)
                    {
                        PstrApprove_MemoId = PstrApprove_MemoId + "," + Val.ToString(DRow["MEMO_ID"]);
                        PstrOrder_MemoId = PstrOrder_MemoId + "," + Val.ToString(DRow["MEMO_ID"]);
                        PstrOrderJangedNo = PstrOrderJangedNo + "," + Val.ToString(DRow["JANGEDNOSTR"]);

                        if (PstrApprove_MemoId.Substring(0, 1) == ",")
                        {
                            PstrApprove_MemoId = PstrApprove_MemoId.Substring(1);
                            PstrOrder_MemoId = PstrOrder_MemoId.Substring(1);
                            PstrOrderJangedNo = PstrOrderJangedNo.Substring(1);
                        }
                    }
                }

                foreach (DataRow DRow in pDtInvoice.Rows)
                {
                    //PstrOrder_MemoId = Val.ToString(DRow["MEMO_ID"]);
                    //PstrOrderJangedNo = Val.ToString(DRow["JANGEDNOSTR"]);

                    DataRow DRNew = DTabMemoDetail.NewRow();

                    DRNew["MEMODETAIL_ID"] = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                    DRNew["STOCK_ID"] = DRow["STOCK_ID"];
                    DRNew["STOCKNO"] = DRow["STOCKNO"];
                    DRNew["PARTYSTOCKNO"] = DRow["PARTYSTOCKNO"];
                    DRNew["STOCKTYPE"] = DRow["STOCKTYPE"];

                    if (mStrStockType == "PARCEL" && (Val.ToString(StrMainMemo_ID).Equals(string.Empty) || Val.ToString(StrMainMemo_ID) == Val.ToString(Guid.Empty))) // Only IF Condtn : #P : 17-01-2020
                    {
                        DRNew["PCS"] = DRow["BALANCEPCS"];
                        DRNew["BALANCEPCS"] = DRow["BALANCEPCS"];
                        DRNew["CARAT"] = DRow["BALANCECARAT"];
                        DRNew["BALANCECARAT"] = DRow["BALANCECARAT"];
                    }
                    else
                    {
                        if (Val.Val(DRow["MEMOPENDINGCARAT"]) != 0)
                        {
                            DRNew["CARAT"] = DRow["MEMOPENDINGCARAT"];
                            DRNew["BALANCECARAT"] = DRow["MEMOPENDINGCARAT"];
                        }

                        if (Val.Val(DRow["MEMOPENDINGPCS"]) != 0)
                        {
                            DRNew["PCS"] = DRow["MEMOPENDINGPCS"];
                            DRNew["BALANCEPCS"] = DRow["MEMOPENDINGPCS"];
                        }
                        if (Val.Val(DRow["MEMOPENDINGCARAT"]) == 0)
                        {
                            DRNew["CARAT"] = DRow["BALANCECARAT"];
                            DRNew["BALANCECARAT"] = DRow["BALANCECARAT"];
                        }
                        if (Val.Val(DRow["MEMOPENDINGPCS"]) == 0)
                        {
                            DRNew["PCS"] = DRow["BALANCEPCS"];
                            DRNew["BALANCEPCS"] = DRow["BALANCEPCS"];

                        }
                    }


                    DRNew["SHAPE_ID"] = DRow["SHAPE_ID"];
                    DRNew["SHAPENAME"] = DRow["SHAPENAME"];
                    DRNew["COLOR_ID"] = DRow["COLOR_ID"];
                    DRNew["COLORNAME"] = DRow["COLORNAME"];
                    DRNew["CLARITY_ID"] = DRow["CLARITY_ID"];
                    DRNew["CLARITYNAME"] = DRow["CLARITYNAME"];
                    DRNew["CUT_ID"] = DRow["CUT_ID"];
                    DRNew["CUTNAME"] = DRow["CUTNAME"];
                    DRNew["POL_ID"] = DRow["POL_ID"];
                    DRNew["POLNAME"] = DRow["POLNAME"];
                    DRNew["SYM_ID"] = DRow["SYM_ID"];
                    DRNew["SYMNAME"] = DRow["SYMNAME"];
                    DRNew["FL_ID"] = DRow["FL_ID"];
                    DRNew["FLNAME"] = DRow["FLNAME"];
                    DRNew["MEASUREMENT"] = DRow["MEASUREMENT"];
                    DRNew["SALERAPAPORT"] = DRow["SALERAPAPORT"];
                    DRNew["SALEDISCOUNT"] = DRow["SALEDISCOUNT"];
                    DRNew["SALEPRICEPERCARAT"] = DRow["SALEPRICEPERCARAT"];
                    DRNew["SALEAMOUNT"] = DRow["SALEAMOUNT"];

                    // #D: 13-01-2021
                    if (mFormType == FORMTYPE.LABRETURN)
                    {
                        DRNew["LABSERVICECODE_ID"] = DRow["LABSERVICECODE_ID"];
                        DRNew["LABSERVICECODE"] = DRow["LABSERVICECODE"];
                    }
                    // #D: 13-01-2021


                    txtExcRate.Text = ObjMemo.GetExchangeRate(Val.ToInt(txtCurrency.Tag), Val.SqlDate(DTPMemoDate.Value.ToShortDateString()), "SALEINVOICEBRANCH").ToString();
                    txtOrgExcRate.Text = txtExcRate.Text;

                    if (Val.ToString(DRow["STATUS"]).ToUpper() == "AVAILABLE") //Changed : #P :30-03-2020
                    {
                        DRNew["MEMORAPAPORT"] = DRow["SALERAPAPORT"];
                        DRNew["MEMODISCOUNT"] = DRow["SALEDISCOUNT"];
                        DRNew["MEMOPRICEPERCARAT"] = DRow["SALEPRICEPERCARAT"];
                        DRNew["MEMOAMOUNT"] = DRow["SALEAMOUNT"];
                        DRNew["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["SALEPRICEPERCARAT"]), 2);

                        if (BOConfiguration.gStrLoginSection == "B")
                        {
                            DRNew["FMEMOAMOUNT"] = Math.Round((Val.Val(txtExcRate.Text) * Val.Val(DRow["SALEAMOUNT"])) / 1000, 2);
                        }
                        else
                        {
                            DRNew["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["SALEAMOUNT"]), 2);
                        }

                        DRNew["JANGEDRAPAPORT"] = DRow["SALERAPAPORT"];
                        DRNew["JANGEDDISCOUNT"] = DRow["SALEDISCOUNT"];
                        DRNew["JANGEDPRICEPERCARAT"] = DRow["SALEPRICEPERCARAT"];
                        DRNew["JANGEDAMOUNT"] = DRow["SALEAMOUNT"];
                    }
                    else
                    {
                        DRNew["MEMORAPAPORT"] = DRow["MEMORAPAPORT"];
                        DRNew["MEMODISCOUNT"] = DRow["MEMODISCOUNT"];
                        DRNew["MEMOPRICEPERCARAT"] = DRow["MEMOPRICEPERCARAT"];
                        DRNew["MEMOAMOUNT"] = DRow["MEMOAMOUNT"];
                        DRNew["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOPRICEPERCARAT"]), 2);
                        //DRNew["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOAMOUNT"]), 2);
                        if (BOConfiguration.gStrLoginSection == "B")
                        {
                            DRNew["FMEMOAMOUNT"] = Math.Round((Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOAMOUNT"])) / 1000, 2);
                        }
                        else
                        {
                            DRNew["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOAMOUNT"]), 2);
                        }

                        DRNew["JANGEDRAPAPORT"] = DRow["MEMORAPAPORT"];
                        DRNew["JANGEDDISCOUNT"] = DRow["MEMODISCOUNT"];
                        DRNew["JANGEDPRICEPERCARAT"] = DRow["MEMOPRICEPERCARAT"];
                        DRNew["JANGEDAMOUNT"] = DRow["MEMOAMOUNT"];
                    }

                    DRNew["STATUS"] = "PENDING";
                    DRNew["REMARK"] = "";
                    DRNew["LOCATION_ID"] = DRow["LOCATION_ID"];
                    DRNew["LOCATIONNAME"] = DRow["LOCATIONNAME"];
                    DRNew["SIZE_ID"] = DRow["SIZE_ID"];
                    DRNew["SIZENAME"] = DRow["SIZENAME"];
                    DRNew["LAB_ID"] = DRow["LAB_ID"];
                    DRNew["LABNAME"] = DRow["LABNAME"];
                    // DRNew["ISPURCHASE"] = DRow["ISPURCHASE"];
                    DRNew["LABREPORTNO"] = DRow["LABREPORTNO"];
                    DRNew["MAINMEMO_ID"] = StrMainMemo_ID;
                    //DRNew["INVOICENO"] = DRow["INVOICENO"];
                    DRNew["OLDMEMODETAIL_IDFORBRANCHRCV"] = DRow["MEMODETAIL_ID"];

                    DTabMemoDetail.Rows.Add(DRNew);
                }

                txtGrossWeight.Text = Val.ToString(pDtInvoice.Rows[0]["GROSSWEIGHT"]);
                cmbInsuranceType.Text = Val.ToString(pDtInvoice.Rows[0]["INSURANCETYPE"]);

                txtBuyer.Text = Val.ToString(pDtInvoice.Rows[0]["FINALBUYERNAME"]);
                txtBuyer.Tag = Val.ToString(pDtInvoice.Rows[0]["FINALBUYER_ID"]);
                //chkIsConsingee.Checked = Val.ToBoolean(pDtInvoice.Rows[0]["ISCONSINGEE"]);

                DataTable DtBuyerDetail = new BusLib.BOComboFill().GetBuyerPartyDetail(Val.ToGuid(txtBuyer.Tag));
                if (DtBuyerDetail.Rows.Count != 0)
                {
                    txtBillingParty.Text = Val.ToString(DtBuyerDetail.Rows[0]["PARTYNAME"]);
                    txtBillingParty.Tag = Val.ToString(DtBuyerDetail.Rows[0]["PARTY_ID"]);
                    txtBAddress1.Text = Val.ToString(DtBuyerDetail.Rows[0]["BILLINGADDRESS1"]);
                    txtBAddress2.Text = Val.ToString(DtBuyerDetail.Rows[0]["BILLINGADDRESS2"]);
                    txtBAddress3.Text = Val.ToString(DtBuyerDetail.Rows[0]["BILLINGADDRESS3"]);
                    txtBCity.Text = Val.ToString(DtBuyerDetail.Rows[0]["BILLINGCITY"]);
                    txtBCountry.Tag = Val.ToString(DtBuyerDetail.Rows[0]["BILLINGCOUNTRY_ID"]);
                    txtBCountry.Text = Val.ToString(DtBuyerDetail.Rows[0]["BILLINGCOUNTRYNAME"]);
                    txtBState.Text = Val.ToString(DtBuyerDetail.Rows[0]["BILLINGSTATE"]);
                    txtBZipCode.Text = Val.ToString(DtBuyerDetail.Rows[0]["BILLINGZIPCODE"]);
                    txtBroker.Tag = Val.ToString(DtBuyerDetail.Rows[0]["COORDINATOR_ID"]);
                    txtBroker.Text = Val.ToString(DtBuyerDetail.Rows[0]["COORDINATORNAME"]);
                    txtFinalDestination.Text = Val.ToString(DtBuyerDetail.Rows[0]["BILLINGCITY"]);
                    txtCountryOfFinalDestination.Text = Val.ToString(DtBuyerDetail.Rows[0]["BILLINGCOUNTRYNAME"]);
                }

                // #D: 13-01-2021
                if (mFormType == FORMTYPE.LABRETURN)
                {
                    txtLabServiceCode.Text = Val.ToString(pDtInvoice.Rows[0]["LABSERVICECODE"]);
                    txtLabServiceCode.Tag = Val.ToInt32(pDtInvoice.Rows[0]["LABSERVICECODE_ID"]);
                }
                // #D: 13-01-2021

                string StrBillParty = Val.ToString(pDtInvoice.Rows[0]["BILLPARTY_ID"]);
                string StrShipParty = Val.ToString(pDtInvoice.Rows[0]["SHIPPARTY_ID"]);

                //txtInvoiceNo.Text = Val.ToString(pDtInvoice.Rows[0]["INVOICENO"]);


                /* : #P : 19-04-2022 : 
                if (StrBillParty != "" && StrBillParty != "00000000-0000-0000-0000-000000000000"
                    && (mFormType == FORMTYPE.PURCHASERETURN || Val.ToString(pDtInvoice.Rows[0]["STATUS"]) != "AVAILABLE")) //Changes : Pinali : 07-11-2019 : Add Only status Condtn
                {
                    DataRow DRow = new BOMST_Ledger().GetDataByPK(StrBillParty);
                    if (DRow != null)
                    {
                        txtBillingParty.Text = Val.ToString(DRow["LEDGERNAME"]) + "/" + Val.ToString(DRow["COMPANYNAME"]);
                        txtBillingParty.Tag = Val.ToString(DRow["LEDGER_ID"]);
                        txtBAddress1.Text = Val.ToString(DRow["BILLINGADDRESS1"]);
                        txtBAddress2.Text = Val.ToString(DRow["BILLINGADDRESS2"]);
                        txtBAddress3.Text = Val.ToString(DRow["BILLINGADDRESS3"]);
                        txtBCity.Text = Val.ToString(DRow["BILLINGCITY"]);
                        txtBCountry.Tag = Val.ToString(DRow["BILLINGCOUNTRY_ID"]);
                        txtBCountry.Text = Val.ToString(DRow["BILLINGCOUNTRYNAME"]);
                        txtBState.Text = Val.ToString(DRow["BILLINGSTATE"]);
                        txtBZipCode.Text = Val.ToString(DRow["BILLINGZIPCODE"]);
                        txtBlindAddLessPer.Text = Val.ToString(pDtInvoice.Rows[0]["BLINDADDLESSPER"]);
                        txtBackAddLess.Text = Val.ToString(pDtInvoice.Rows[0]["BACKADDLESS"]);
                        txtTermsAddLessPer.Text = Val.ToString(pDtInvoice.Rows[0]["TERMSADDLESSPER"]);


                    }
                }

                if (StrShipParty != "" && StrShipParty != "00000000-0000-0000-0000-000000000000")
                {
                    DataRow DRow = new BOMST_Ledger().GetDataByPK(StrBillParty);
                    if (DRow != null)
                    {
                        txtShippingParty.Text = Val.ToString(DRow["LEDGERNAME"]);
                        txtShippingParty.Tag = Val.ToString(DRow["LEDGER_ID"]);
                        txtSAddress1.Text = Val.ToString(DRow["SHIPPINGADDRESS1"]);
                        txtSAddress2.Text = Val.ToString(DRow["SHIPPINGADDRESS2"]);
                        txtSAddress3.Text = Val.ToString(DRow["SHIPPINGADDRESS3"]);
                        txtSCity.Text = Val.ToString(DRow["SHIPPINGCITY"]);
                        txtSCountry.Tag = Val.ToString(DRow["SHIPPINGCOUNTRY_ID"]);
                        txtSCountry.Text = Val.ToString(DRow["SHIPPINGCOUNTRYNAME"]);
                        txtSState.Text = Val.ToString(DRow["SHIPPINGSTATE"]);
                        txtSZipCode.Text = Val.ToString(DRow["SHIPPINGZIPCODE"]);
                    }
                }
                */

                if (Val.ToString(pDtInvoice.Rows[0]["BRANCHDELIVERYSTATUS"]) == "RECEIVED")
                    ChkOrderConfirmPickup.Checked = true;
                else
                    ChkOrderConfirmPickup.Checked = false;
            }


            DataTable DTabTerms = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_TERMS);

            foreach (DataRow DRow in DTabTerms.Rows)
            {
                if (Val.ToString(DRow["TERMSNAME"]) == "NONE")
                {
                    txtTerms.Text = Val.ToString(DRow["TERMSNAME"]);
                    txtTerms.Tag = Val.ToString(DRow["TERMS_ID"]);
                    txtTermsDays.Text = Val.ToString(DRow["TERMSDAYS"]);
                }
            }
            DTabTerms.Dispose();
            DTabTerms = null;


            lblMemoNo.Text = string.Empty;
            DTPMemoDate.Value = DateTime.Now;
            txtSellerName.Text = BOConfiguration.gEmployeeProperty.LEDGERNAME;
            txtSellerName.Tag = BOConfiguration.gEmployeeProperty.LEDGER_ID;

            //txtExcRate.Text = ObjMemo.GetExchangeRate(Val.ToInt(txtCurrency.Tag), Val.SqlDate(DTPMemoDate.Value.ToShortDateString())).ToString();
            //txtOrgExcRate.Text = txtExcRate.Text;

            //Add : #D : 20-06-2020
            if (mFormType == FORMTYPE.SALEINVOICE && pDtInvoice != null)
            {
                txtBroker.Text = pDtInvoice.Rows[0]["INVBROKERNAME"].ToString();
                txtBroker.Tag = pDtInvoice.Rows[0]["INVBROKER_ID"];
                txtBaseBrokeragePer.Text = pDtInvoice.Rows[0]["INVBROKRAGEPER"].ToString();
                txtProfitBrokeragePer.Text = pDtInvoice.Rows[0]["INVBROKRAGEPROFITPER"].ToString();
                txtExcRate.Text = Val.Val(pDtInvoice.Rows[0]["INVEXCRATE"]) != 0 ? pDtInvoice.Rows[0]["INVEXCRATE"].ToString() : Val.ToString(txtExcRate.Text);
                txtOrgExcRate.Text = txtExcRate.Text;

                txtSellerName.Text = pDtInvoice.Rows[0]["INSELLERNAME"].ToString();
                txtSellerName.Tag = pDtInvoice.Rows[0]["INVSELLER_ID"];
                txtTerms.Tag = pDtInvoice.Rows[0]["INVTERMS_ID"];
                txtTerms.Text = pDtInvoice.Rows[0]["INVTERMSNAME"].ToString();
                DTPTermsDate.Text = pDtInvoice.Rows[0]["INVTERMSDATE"].ToString();
                txtTermsDays.Text = pDtInvoice.Rows[0]["INVTERMSDAYS"].ToString();
                txtTransport.Text = pDtInvoice.Rows[0]["INVTRANSPORTNAME"].ToString();
                CmbDeliveryType.Text = pDtInvoice.Rows[0]["INVDELIVERYTYPE"].ToString();
                CmbPaymentMode.Text = pDtInvoice.Rows[0]["INVPAYMENTMODE"].ToString();
                cmbBillType.Text = pDtInvoice.Rows[0]["INVBILLTYPE"].ToString();
                CmbMemoType.Text = pDtInvoice.Rows[0]["INVMEMOTYPE"].ToString();
                txtPlaceOfSupply.Text = pDtInvoice.Rows[0]["INVPLACEOFSUPPLY"].ToString();
                txtExcRate_Validated(null, null);
                CmbPaymentMode.SelectedIndex = 1;
            }
            else if (mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.SALEINVOICE)
            {
                if (txtBroker.Text.Trim().Equals(string.Empty))
                {
                    txtBaseBrokeragePer.Text = string.Empty;
                }
                else
                {
                    txtBaseBrokeragePer.Text = "0.50";
                }

            }
            //End : #D : 20-06-2020

            // #D : 20-06-2020
            if (mFormType == FORMTYPE.LABISSUE)
            {
                txtLabServiceCode.Visible = true;
                BtnApplyAll.Visible = true;
                lblLabServiceCode.Visible = true;
            }
            else if (mFormType == FORMTYPE.LABRETURN)
            {
                txtLabServiceCode.Visible = true;
                BtnApplyAll.Visible = true;
                txtLabServiceCode.Enabled = false;
                BtnApplyAll.Enabled = false;
                lblLabServiceCode.Visible = true;
            }
            else
            {
                txtLabServiceCode.Visible = false;
                BtnApplyAll.Visible = false;
                lblLabServiceCode.Visible = false;
            }
            // #D : 20-06-2020

            CreateSummaryTable();
            if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.SALESDELIVERYRETURN)
            {

                DataRow Drow = DTabMemoSummury.NewRow();
                DTabMemoSummury.Rows.Add(Drow);

                GetSummuryDetailForGrid();
                MainGridSummury.DataSource = DTabMemoSummury;
                MainGridSummury.Refresh();
                RbDollar_CheckedChanged(null, null);
            }
            else
            {
                xtraTabControl1.TabPages.Remove(xtraTabPage5);
                MainGridSummury.DataSource = null;
                MainGridSummury.Refresh();
            }



            //#K: 05122020
            ChkApprovedOrder.Visible = false;
            if (mFormType == FORMTYPE.ORDERCONFIRM)
                ChkApprovedOrder.Visible = true;

            //Calculation();
            CalculationNew();
            DTPMemoDate.Focus();

            if (mFormType != FORMTYPE.SALEINVOICE && mFormType != FORMTYPE.SALESDELIVERYRETURN && mFormType != FORMTYPE.PURCHASERETURN && mFormType != FORMTYPE.PURCHASEISSUE)
            {
                xtraTabControl1.TabPages.Remove(xtraTabPage3);
                xtraTabControl1.TabPages.Remove(xtraTabPage4);
                xtraTabControl1.TabPages.Remove(xtraTabPage7);
            }

            if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRM)
            {
                txtBuyer.Visible = true;
                lblBuyer.Visible = true;
            }
            else
            {
                txtBuyer.Visible = false;
                lblBuyer.Visible = false;
            }
            cmbBillType.SelectedIndex = 3;
            cmbAccType.SelectedIndex = 1;
            chkIsConsingee_CheckedChanged(null, null);

        }

        //Call From Report List
        // public void ShowForm(string pStrMemoID)  //Call When Double Click On Current Stock Color Clarity Wise Report Data
        public void ShowForm(string pStrMemoID, string pStrStockType = "ALL")  //Call When Double Click On Current Stock Color Clarity Wise Report Data
        {
            try
            {

                this.Cursor = Cursors.WaitCursor;

                Val.FormGeneralSetting(this);
                AttachFormDefaultEvent();

                this.Show();

                BtnReturn.Enabled = true;
                BtnOtherActivity.Enabled = true;
                BtnClear_Click(null, null); //Add : Pinali : 15-09-2019


                // Dhara : 19/9/2021
                //HINA - START
                //DataSet DS = ObjMemo.GetMemoListData(0, null, null, "", "", "", 0, "", 0, "", "ALL", pStrMemoID);
                mStrStockType = pStrStockType;
                DataSet DS = ObjMemo.GetMemoListData(0, null, null, "", "", "", 0, "", 0, "", "ALL", pStrMemoID, mStrStockType, false, 0);

                lblStockType.Text = mStrStockType + " STOCK";

                DTabMemo = DS.Tables[0];
                DTabMemoDetail = DS.Tables[1];

                DTabMemoDetail.Columns.Add("OLDMEMODISCOUNT", typeof(double));
                DTabMemoDetail.Columns.Add("OLDMEMOPRICEPERCARAT", typeof(double));

                MainGrdDetail.DataSource = DTabMemoDetail;
                MainGrdDetail.Refresh();

                if (DTabMemo.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("THERE IS NO ANY MEMO DATA FOUND");
                    return;
                }


                DataRow DRow = DTabMemo.Rows[0];

                lblMode.Text = "Edit Mode";
                txtJangedNo.Text = Val.ToString(DRow["JANGEDNOSTR"]);
                txtJangedNo.Tag = Val.ToString(DRow["MEMO_ID"]);
                lblMemoNo.Tag = Val.ToString(DRow["MEMO_ID"]);
                lblMemoNo.Text = Val.ToString(DRow["MEMONO"]);
                lblStatus.Text = Val.ToString(DRow["STATUS"]);

                DTPMemoDate.Text = Val.ToString(DRow["MEMODATE"]);
                DTPMemoDate.Text = Val.ToDate(DateTime.Parse(DRow["MEMODATE"].ToString()), AxonDataLib.BOConversion.DateFormat.DDMMYYYY);

                txtBillingParty.Tag = Val.ToString(DRow["BILLINGPARTY_ID"]);
                txtBillingParty.Text = Val.ToString(DRow["BILLINGPARTYNAME"]);

                txtShippingParty.Tag = Val.ToString(DRow["SHIPPINGPARTY_ID"]);
                txtShippingParty.Text = Val.ToString(DRow["SHIPPINGPARTYNAME"]);

                txtSellerName.Tag = Val.ToString(DRow["SELLER_ID"]);
                txtSellerName.Text = Val.ToString(DRow["SELLERNAME"]);

                txtBroker.Tag = Val.ToString(DRow["BROKER_ID"]);
                txtBroker.Text = Val.ToString(DRow["BROKERNAME"]);
                txtBaseBrokeragePer.Text = Val.ToString(DRow["BROKRAGEPER"]);
                txtProfitBrokeragePer.Text = Val.ToString(DRow["BROKRAGEPROFITPER"]);

                txtAdat.Tag = Val.ToString(DRow["ADAT_ID"]);
                txtAdat.Text = Val.ToString(DRow["ADATNAME"]);
                txtAdatPer.Text = Val.ToString(DRow["ADATPER"]);

                txtTerms.Tag = Val.ToString(DRow["TERMS_ID"]);
                txtTerms.Text = Val.ToString(DRow["TERMSNAME"]);
                txtTermsDays.Text = Val.ToString(DRow["TERMSDAYS"]);

                CreateSummaryTable();
                if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.SALESDELIVERYRETURN)
                {

                    DataRow Drow = DTabMemoSummury.NewRow();
                    DTabMemoSummury.Rows.Add(Drow);

                    GetSummuryDetailForGrid();
                    MainGridSummury.DataSource = DTabMemoSummury;
                    MainGridSummury.Refresh();
                }
                else
                {
                    xtraTabControl1.TabPages.Remove(xtraTabPage5);
                    MainGridSummury.DataSource = null;
                    MainGridSummury.Refresh();
                }

                CmbDeliveryType.Text = Val.ToString(DRow["DELIVERYTYPE"]);
                CmbPaymentMode.Text = Val.ToString(DRow["PAYMENTMODE"]);
                cmbBillType.Text = Val.ToString(DRow["BILLTYPE"]);


                if (cmbBillType.Text.ToUpper() == "EXPORT")
                {
                    foreach (DataRow DR in DTabMemoDetail.Rows)
                    {
                        if (DR["MEMOPRICEPERCARAT"] != DR["EXPINVOICERATE"])
                        {
                            ChkUpdExport.Checked = true;
                        }
                    }
                }
                cmbAccType.Text = Val.ToString(DRow["ACCTYPE"]);

                StrMainBillType = Val.ToString(DRow["BILLTYPE"]);

                DTPTermsDate.Text = DateTime.Parse(DRow["TERMSDATE"].ToString()).ToShortDateString();

                txtCurrency.Tag = Val.ToString(DRow["CURRENCY_ID"]);
                txtCurrency.Text = Val.ToString(DRow["CURRENCYNAME"]);
                txtExcRate.Text = Val.ToString(DRow["EXCRATE"]);
                txtOrgExcRate.Text = Val.ToString(DRow["ORG_EXCRATE"]);
                txtAddLessExcRate.Text = Val.ToString(DRow["ADDLESS_EXCRATE"]);
                txtBrNo.Text = Val.ToString(DRow["BRNO"]);

                //GrdDetail.Columns["FMEMOPRICEPERCARAT"].Caption = "  " + Val.ToString(DRow["SYMBOL"]) + "/Cts";
                //GrdDetail.Columns["FMEMOAMOUNT"].Caption = "  Amt (" + Val.ToString(DRow["SYMBOL"]) + ")";


                txtBAddress1.Text = Val.ToString(DRow["BILLINGADDRESS1"]);
                txtBAddress2.Text = Val.ToString(DRow["BILLINGADDRESS2"]);
                txtBAddress3.Text = Val.ToString(DRow["BILLINGADDRESS3"]);
                txtBCountry.Tag = Val.ToString(DRow["BILLINGCOUNTRY_ID"]);
                txtBCountry.Text = Val.ToString(DRow["BILLINGCOUNTRYNAME"]);
                txtBState.Text = Val.ToString(DRow["BILLINGSTATE"]);
                txtBCity.Text = Val.ToString(DRow["BILLINGCITY"]);
                txtBZipCode.Text = Val.ToString(DRow["BILLINGZIPCODE"]);

                txtSAddress1.Text = Val.ToString(DRow["SHIPPINGADDRESS1"]);
                txtSAddress2.Text = Val.ToString(DRow["SHIPPINGADDRESS2"]);
                txtSAddress3.Text = Val.ToString(DRow["SHIPPINGADDRESS3"]);
                txtSCountry.Tag = Val.ToString(DRow["SHIPPINGCOUNTRY_ID"]);
                txtSCountry.Text = Val.ToString(DRow["SHIPPINGCOUNTRYNAME"]);
                txtSState.Text = Val.ToString(DRow["SHIPPINGSTATE"]);
                txtSCity.Text = Val.ToString(DRow["SHIPPINGCITY"]);
                txtSZipCode.Text = Val.ToString(DRow["SHIPPINGZIPCODE"]);

                txtRemark.Text = Val.ToString(DRow["REMARK"]);
                lblSource.Text = Val.ToString(DRow["SOURCE"]);

                txtTransport.Text = Val.ToString(DRow["TRANSPORTNAME"]);
                txtPlaceOfSupply.Text = Val.ToString(DRow["PLACEOFSUPPLY"]);

                txtCompanyBank.Tag = Val.ToString(DRow["COMPANYBANK_ID"]);
                txtCompanyBank.Text = Val.ToString(DRow["COMPANYBANKNAME"]);
                txtPartyBank.Tag = Val.ToString(DRow["PARTYBANK_ID"]);
                txtPartyBank.Text = Val.ToString(DRow["PARTYBANKNAME"]);
                txtCourier.Tag = Val.ToString(DRow["COURIER_ID"]);
                txtCourier.Text = Val.ToString(DRow["COURIERNAME"]);
                txtAirFreight.Tag = Val.ToString(DRow["AIRFREIGHT_ID"]);
                txtAirFreight.Text = Val.ToString(DRow["AIRFREIGHTNAME"]);


                if (Val.ToString(cmbAddresstype.SelectedItem) == "MUMBAI")
                {
                    txtPortOfLoading.Text = Val.ToString(DRow["BILLINGCITY"]);
                    txtPlaceOfReceiptByPreCarrier.Text = Val.ToString(DRow["PLACEOFRECEIPTBYPRECARRIER"]);

                }
                else
                {
                    txtPortOfLoading.Text = Val.ToString(DRow["SHIPPINGCITY"]);
                    txtPlaceOfReceiptByPreCarrier.Text = Val.ToString(DRow["PLACEOFRECEIPTBYPRECARRIER"]);

                }

                txtPortOfDischarge.Text = Val.ToString(DRow["PORTOFDISCHARGE"]);
                txtFinalDestination.Text = Val.ToString(DRow["BILLINGCITY"]);
                txtCountryOfOrigin.Text = Val.ToString(DRow["COUNTRYOFORIGIN"]);
                txtCountryOfFinalDestination.Text = Val.ToString(DRow["COUNTRYOFFINALDESTINATION"]);
                txtVoucherNoStr.Text = Val.ToString(DRow["VOUCHERNOSTR"]);

                txtGrossAmount.Text = Val.ToString(DRow["GROSSAMOUNT"]);
                txtGrossAmountFE.Text = Val.ToString(DRow["FGROSSAMOUNT"]);

                txtDiscPer.Text = Val.ToString(DRow["DISCOUNTPER"]);
                txtDiscAmount.Text = Val.ToString(DRow["DISCOUNTAMOUNT"]);
                txtDiscAmountFE.Text = Val.ToString(DRow["FDISCOUNTAMOUNT"]);

                txtInsurancePer.Text = Val.ToString(DRow["INSURANCEPER"]);
                txtInsuranceAmount.Text = Val.ToString(DRow["INSURANCEAMOUNT"]);
                txtInsuranceAmountFE.Text = Val.ToString(DRow["FINSURANCEAMOUNT"]);

                txtShippingPer.Text = Val.ToString(DRow["SHIPPINGPER"]);
                txtShippingAmount.Text = Val.ToString(DRow["SHIPPINGAMOUNT"]);
                txtShippingAmountFE.Text = Val.ToString(DRow["FSHIPPINGAMOUNT"]);

                txtGSTPer.Text = Val.ToString(DRow["GSTPER"]);
                txtGSTAmount.Text = Val.ToString(DRow["GSTAMOUNT"]);
                txtGSTAmountFE.Text = Val.ToString(DRow["FGSTAMOUNT"]);

                txtIGSTPer.Text = Val.ToString(DRow["IGSTPER"]);
                txtIGSTAmount.Text = Val.ToString(DRow["IGSTAMOUNT"]);
                txtIGSTAmountFE.Text = Val.ToString(DRow["FIGSTAMOUNT"]);
                txtCGSTPer.Text = Val.ToString(DRow["CGSTPER"]);
                txtCGSTAmount.Text = Val.ToString(DRow["CGSTAMOUNT"]);
                txtCGSTAmountFE.Text = Val.ToString(DRow["FCGSTAMOUNT"]);
                txtSGSTPer.Text = Val.ToString(DRow["SGSTPER"]);
                txtSGSTAmount.Text = Val.ToString(DRow["SGSTAMOUNT"]);
                txtSGSTAmountFE.Text = Val.ToString(DRow["FSGSTAMOUNT"]);

                txtNetAmount.Text = Val.ToString(DRow["NETAMOUNT"]);
                txtNetAmountFE.Text = Val.ToString(DRow["FNETAMOUNT"]);

                txtRemark.Text = Val.ToString(DRow["REMARK"]);
                txtTransport.Text = Val.ToString(DRow["TRANSPORTNAME"]);
                txtPlaceOfSupply.Text = Val.ToString(DRow["PLACEOFSUPPLY"]);

                lblTotalPcs.Text = Val.ToString(DRow["TOTALPCS"]);
                lblTotalCarat.Text = Val.ToString(DRow["TOTALCARAT"]);
                txtMemoAvgDisc.Text = Val.ToString(DRow["TOTALAVGDISC"]);
                txtMemoAvgRate.Text = Val.ToString(DRow["TOTALAVGRATE"]);

                lblTitle.Text = Val.ToString(DRow["PROCESSNAME"]);
                lblTitle.Tag = Val.ToString(DRow["PROCESS_ID"]);

                txtGrossWeight.Text = Val.ToString(DRow["GROSSWEIGHT"]);
                cmbInsuranceType.Text = Val.ToString(DRow["INSURANCETYPE"]);

                txtHkdRate.Text = Val.ToString(DRow["HKDRATE"]);//GUNJAN:07/04/2023
                txtHkdAmt.Text = Val.ToString(DRow["HKDAMOUNT"]);

                // #D: 19-08-2020

                txtconsignee.Text = Val.ToString(DRow["CONSIGNEE"]);
                cmbAddresstype.Text = Val.ToString(DRow["ADDRESSTYPE"]);
                if (Val.ToBooleanToInt(DRow["APPROVAL"]) == 1)
                    ChkApprovedOrder.Checked = true;
                else
                    ChkApprovedOrder.Checked = false;
                // #D: 19-08-2020

                // #d: 13-01-2021
                txtLabServiceCode.Text = Val.ToString(DRow["LABSERVICECODE"]);
                txtLabServiceCode.Tag = Val.ToInt32(DRow["LABSERVICECODE_ID"]);

                txtNarration.Text = Val.ToString(DRow["NARRATIONNAME"]);
                txtNarration.Tag = Val.ToInt32(DRow["NARRATION_ID"]);

                // #d: 13-01-2021

                txtBackAddLess.Text = Val.ToString(DRow["BACKADDLESS"]);
                txtTermsAddLessPer.Text = Val.ToString(DRow["TERMSADDLESSPER"]);
                txtBlindAddLessPer.Text = Val.ToString(DRow["BLINDADDLESSPER"]);

                txtInvoiceNo.Text = Val.ToString(DRow["INVOICENO"]);
                if (BOConfiguration.gStrLoginSection == "B")
                {
                    txtApproveMemoNo.Visible = true;
                    txtOrderMemoNo.Visible = true;
                    txtApproveMemoNo.Text = Val.ToString(DRow["APPROVEMEMONO"]);
                    txtOrderMemoNo.Text = Val.ToString(DRow["ORDERMEMONO"]);
                    PstrApprove_MemoId = Val.ToString(DRow["MEMO_ID"]);
                }

                /*
    1	NONE	NONE
    3	PUR	PURCHASE
    4	PURRET	PURCHASE RETURN
    5	MKMO	MEMO ISSUE
    6	MORET	MEMO RETURN
    7	HOLD	HOLD
    8	REALEASE	RELEASE
    9	OFFLINE	OFFLINE
    10	INV	SALES DELIVERY
    11	INVRET	SALES RETURN
    12	SOLD	SALES ORDER
    13	PRICE	PRICING
    14	ONLINE	ONLINE
    15	PARAMUPD	PARAMETER UPDATE
                 */

                if (Val.ToInt(lblTitle.Tag) == 2) mFormType = FORMTYPE.PURCHASEISSUE;
                else if (Val.ToInt(lblTitle.Tag) == 3) mFormType = FORMTYPE.PURCHASERETURN;
                else if (Val.ToInt(lblTitle.Tag) == 4) mFormType = FORMTYPE.MEMOISSUE;
                else if (Val.ToInt(lblTitle.Tag) == 5) mFormType = FORMTYPE.MEMORETURN;
                else if (Val.ToInt(lblTitle.Tag) == 6) mFormType = FORMTYPE.HOLD;
                else if (Val.ToInt(lblTitle.Tag) == 7) mFormType = FORMTYPE.RELEASE;
                else if (Val.ToInt(lblTitle.Tag) == 8) mFormType = FORMTYPE.OFFLINE;
                else if (Val.ToInt(lblTitle.Tag) == 9) mFormType = FORMTYPE.SALEINVOICE;
                else if (Val.ToInt(lblTitle.Tag) == 10) mFormType = FORMTYPE.SALESDELIVERYRETURN;
                else if (Val.ToInt(lblTitle.Tag) == 11) mFormType = FORMTYPE.ORDERCONFIRM;
                else if (Val.ToInt(lblTitle.Tag) == 13) mFormType = FORMTYPE.ONLINE;
                else if (Val.ToInt(lblTitle.Tag) == 15) mFormType = FORMTYPE.ORDERCONFIRMRETURN;
                else if (Val.ToInt(lblTitle.Tag) == 19) mFormType = FORMTYPE.LABISSUE;
                else if (Val.ToInt(lblTitle.Tag) == 21) mFormType = FORMTYPE.LABRETURN;
                else if (Val.ToInt(lblTitle.Tag) == 26) mFormType = FORMTYPE.CONSIGNMENTISSUE;
                else if (Val.ToInt(lblTitle.Tag) == 27) mFormType = FORMTYPE.CONSIGNMENTRETURN;

                //#P : 13-01-2020
                lblStockType.Text = mStrStockType + " STOCK";
                if (mStrStockType == "PARCEL")
                {
                    GrdDetail.Columns["POLNAME"].Visible = false;
                    GrdDetail.Columns["SYMNAME"].Visible = false;
                    GrdDetail.Columns["FLNAME"].Visible = false;
                    GrdDetail.Columns["LABREPORTNO"].Visible = false;

                    if (Val.ToString(lblMode.Text).ToUpper() == "EDIT MODE")
                    {
                        GrdDetail.Columns["RETURNPCS"].Visible = true;
                        GrdDetail.Columns["RETURNCARAT"].Visible = true;
                    }
                    else
                    {
                        GrdDetail.Columns["RETURNPCS"].Visible = false;
                        GrdDetail.Columns["RETURNCARAT"].Visible = false;
                    }
                    GrdDetail.Columns["SALERAPAPORT"].Visible = false;
                    GrdDetail.Columns["MEMORAPAPORT"].Visible = false;
                    GrdDetail.Columns["SALEDISCOUNT"].Visible = false;
                    GrdDetail.Columns["MEMODISCOUNT"].Visible = false;
                }
                else
                {
                    GrdDetail.Columns["POLNAME"].Visible = true;
                    GrdDetail.Columns["SYMNAME"].Visible = true;
                    GrdDetail.Columns["FLNAME"].Visible = true;
                    GrdDetail.Columns["LABREPORTNO"].Visible = true;

                    GrdDetail.Columns["SALERAPAPORT"].Visible = true;
                    GrdDetail.Columns["MEMORAPAPORT"].Visible = true;
                    GrdDetail.Columns["MEMORAPAPORT"].Fixed = FixedStyle.Right;  //#P
                    GrdDetail.Columns["SALEDISCOUNT"].Visible = true;
                    GrdDetail.Columns["MEMODISCOUNT"].Visible = true;

                    GrdDetail.Columns["RETURNPCS"].Visible = false;
                    GrdDetail.Columns["RETURNCARAT"].Visible = false;

                }
                //End : #P : 13-01-2020

                //#P : 05-05-2021: Coz JangedDisc And PerCts prthi Sale Amt Consider thay 6e etle MemoDisc Editable nathi..
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                //#P : 05-05-2021

                FillControlName();
                CalculationNew();

                CmbMemoType.SelectedItem = Val.ToString(DRow["MEMOTYPE"]);


                if (Val.ToString(lblMode.Text).ToUpper() == "EDIT MODE") //#P : 06-10-2019
                {
                    GrdDetail.BeginUpdate();
                    if (MainGrdDetail.RepositoryItems.Count == 20)
                    {
                        ObjGridSelection = new BODevGridSelection();
                        ObjGridSelection.View = GrdDetail;
                        ObjGridSelection.ClearSelection();
                        ObjGridSelection.CheckMarkColumn.VisibleIndex = 1;
                    }
                    else
                    {
                        ObjGridSelection.ClearSelection();
                    }
                    GrdDetail.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;

                    GrdDetail.Bands["BANDSTOCK"].Fixed = FixedStyle.None;
                    GridBand band = GrdDetail.Bands.AddBand("..");
                    band.Columns.Add(GrdDetail.Columns["COLSELECTCHECKBOX"]);
                    band.Fixed = FixedStyle.Left;
                    band.VisibleIndex = 0;

                    GrdDetail.Bands["BANDSTOCK"].Fixed = FixedStyle.Left;
                    if (ObjGridSelection != null)
                    {
                        ObjGridSelection.ClearSelection();
                        ObjGridSelection.CheckMarkColumn.VisibleIndex = 1;
                    }
                    GrdDetail.EndUpdate();
                }

                //#K: 05122020
                ChkApprovedOrder.Visible = false;
                if (mFormType == FORMTYPE.ORDERCONFIRM)
                    ChkApprovedOrder.Visible = true;

                DTPMemoDate.Focus();

                if (Val.ToString(DRow["BILLFORMAT"]) == RbDollar.Tag.ToString())
                {
                    RbDollar.Checked = true;
                    RbDollar_CheckedChanged(null, null);
                }
                else
                {
                    RbRupee.Checked = true;
                    RbRupee_CheckedChanged(null, null);
                }

                if (mFormType != FORMTYPE.SALEINVOICE && mFormType != FORMTYPE.SALESDELIVERYRETURN && mFormType != FORMTYPE.PURCHASERETURN && mFormType != FORMTYPE.PURCHASEISSUE)
                    xtraTabControl1.TabPages.Remove(xtraTabPage3);

                if (mFormType == FORMTYPE.LABISSUE)
                {
                    txtLabServiceCode.Visible = true;
                    BtnApplyAll.Visible = true;
                    lblLabServiceCode.Visible = true;
                }
                else if (mFormType == FORMTYPE.LABRETURN)
                {
                    txtLabServiceCode.Visible = true;
                    BtnApplyAll.Visible = true;
                    lblLabServiceCode.Visible = true;
                    txtLabServiceCode.Enabled = false;
                    BtnApplyAll.Enabled = false;
                    BtnApplyAll.Enabled = false;
                }

                if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.SALESDELIVERYRETURN)
                {
                    txtNarration.Visible = true;
                    lblNarration.Visible = true;
                    BtnNarrationApplyAll.Visible = true;
                    GrdDetail.Columns["NARRATIONNAME"].Visible = true;
                }
                else
                {
                    txtNarration.Visible = false;
                    lblNarration.Visible = false;
                    BtnNarrationApplyAll.Visible = false;
                    GrdDetail.Columns["NARRATIONNAME"].Visible = false;
                }

                xtraTabPage5.Focus();

                if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRM)
                {
                    txtBuyer.Visible = true;
                    lblBuyer.Visible = true;
                }
                else
                {
                    txtBuyer.Visible = false;
                    lblBuyer.Visible = false;
                }
                chkIsConsingee_CheckedChanged(null, null);
                if (Val.ToDouble(DRow["GSTPER"]) == 0.00)
                {
                    cmbBillType_SelectedIndexChanged(null, null);//Gunjan:17/04/2023
                }

                txtBuyer.Text = Val.ToString(DRow["FINALBUYERNAME"]);
                txtBuyer.Tag = Val.ToString(DRow["FINALBUYER_ID"]);
                chkIsConsingee.Checked = Val.ToBoolean(DRow["ISCONSINGEE"]);
                if (Val.ToString(DRow["ISPICKUP"]) == "YES")
                    ChkOrderConfirmPickup.Checked = true;
                else
                    ChkOrderConfirmPickup.Checked = false;

                this.Cursor = Cursors.Default;

                if (BOConfiguration.gStrLoginSection == "B" && mFormType == FORMTYPE.ORDERCONFIRM)
                {
                    pnlButton.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.ToString());
            }
        }

        public void CreateSummaryTable()
        {
            DTabMemoSummury.Columns.Add(new DataColumn("DESCRIPION", typeof(String)));
            DTabMemoSummury.Columns.Add(new DataColumn("TOTALPCS", typeof(double)));
            DTabMemoSummury.Columns.Add(new DataColumn("TOTALCARAT", typeof(double)));
            DTabMemoSummury.Columns.Add(new DataColumn("SALEAVGRATE", typeof(double)));
            DTabMemoSummury.Columns.Add(new DataColumn("SALEAVGDISC", typeof(double)));
            DTabMemoSummury.Columns.Add(new DataColumn("SALETOTALAMOUNT", typeof(double)));
            DTabMemoSummury.Columns.Add(new DataColumn("MEMOAVGRATE", typeof(double)));
            DTabMemoSummury.Columns.Add(new DataColumn("MEMOAVGDISC", typeof(double)));
            DTabMemoSummury.Columns.Add(new DataColumn("MEMOTOTALAMOUNT", typeof(double)));
            // Add Khushbu 09-09-21
            DTabMemoSummury.Columns.Add(new DataColumn("SALEAVGRATEFE", typeof(double)));
            DTabMemoSummury.Columns.Add(new DataColumn("SALEAVGDISCFE", typeof(double)));
            DTabMemoSummury.Columns.Add(new DataColumn("SALETOTALAMOUNTFE", typeof(double)));
            DTabMemoSummury.Columns.Add(new DataColumn("MEMOAVGRATEFE", typeof(double)));
            DTabMemoSummury.Columns.Add(new DataColumn("MEMOAVGDISCFE", typeof(double)));
            DTabMemoSummury.Columns.Add(new DataColumn("MEMOTOTALAMOUNTFE", typeof(double)));
        }
        //#P : 30-07-2020 : For Return
        public void ShowFormForReturn(FORMTYPE pFormType, string pStrMemoID, string pStrStoneNo, string pStrStockType = "ALL")
        {
            try
            {
                BOFormPer ObjPer = new BOFormPer();
                this.Cursor = Cursors.WaitCursor;

                Val.FormGeneralSetting(this);
                AttachFormDefaultEvent();

                this.Show();

                BtnReturn.Enabled = true;
                BtnOtherActivity.Enabled = true;
                BtnClear_Click(null, null); //Add : Pinali : 15-09-2019

                //HINA - START
                //DataSet DS = ObjMemo.GetMemoListData(0, null, null, "", "", "", 0, "", 0, "", "ALL", pStrMemoID);
                mStrStockType = pStrStockType;
                DataSet DS = ObjMemo.GetMemoListData(0, null, null, "", "", "", 0, "", 0, "", "ALL", pStrMemoID, mStrStockType, false, 0);

                //HINA - END

                lblStockType.Text = mStrStockType + " STOCK";

                DTabMemo = DS.Tables[0];
                DTabMemoDetail = DS.Tables[1];

                DTabMemoDetail.Columns.Add("OLDMEMODISCOUNT", typeof(double));
                DTabMemoDetail.Columns.Add("OLDMEMOPRICEPERCARAT", typeof(double));


                MainGrdDetail.DataSource = DTabMemoDetail;
                MainGrdDetail.Refresh();

                if (DTabMemo.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("THERE IS NO ANY MEMO DATA FOUND");
                    return;
                }


                DataRow DRow = DTabMemo.Rows[0];

                lblMode.Text = "Edit Mode";
                txtJangedNo.Text = Val.ToString(DRow["JANGEDNOSTR"]);
                txtJangedNo.Tag = Val.ToString(DRow["MEMO_ID"]);
                lblMemoNo.Tag = Val.ToString(DRow["MEMO_ID"]);
                lblMemoNo.Text = Val.ToString(DRow["MEMONO"]);
                lblStatus.Text = Val.ToString(DRow["STATUS"]);

                DTPMemoDate.Text = Val.ToString(DRow["MEMODATE"]);
                //DTPMemoDate.Text = DateTime.Parse(DRow["MEMODATE"].ToString()).ToString("dd/MM/yyyy");
                DTPMemoDate.Text = Val.ToDate(DateTime.Parse(DRow["MEMODATE"].ToString()), AxonDataLib.BOConversion.DateFormat.DDMMYYYY);

                //DTPMemoDate.Text = DateTime.Parse(DRow["MEMODATE"].ToString()).ToShortDateString();

                txtBillingParty.Tag = Val.ToString(DRow["BILLINGPARTY_ID"]);
                txtBillingParty.Text = Val.ToString(DRow["BILLINGPARTYNAME"]);

                txtShippingParty.Tag = Val.ToString(DRow["SHIPPINGPARTY_ID"]);
                txtShippingParty.Text = Val.ToString(DRow["SHIPPINGPARTYNAME"]);

                txtSellerName.Tag = Val.ToString(DRow["SELLER_ID"]);
                txtSellerName.Text = Val.ToString(DRow["SELLERNAME"]);

                txtBroker.Tag = Val.ToString(DRow["BROKER_ID"]);
                txtBroker.Text = Val.ToString(DRow["BROKERNAME"]);
                txtBaseBrokeragePer.Text = Val.ToString(DRow["BROKRAGEPER"]);
                txtProfitBrokeragePer.Text = Val.ToString(DRow["BROKRAGEPROFITPER"]);

                txtAdat.Tag = Val.ToString(DRow["ADAT_ID"]);
                txtAdat.Text = Val.ToString(DRow["ADATNAME"]);
                txtAdatPer.Text = Val.ToString(DRow["ADATPER"]);

                txtTerms.Tag = Val.ToString(DRow["TERMS_ID"]);
                txtTerms.Text = Val.ToString(DRow["TERMSNAME"]);
                txtTermsDays.Text = Val.ToString(DRow["TERMSDAYS"]);

                CmbDeliveryType.Text = Val.ToString(DRow["DELIVERYTYPE"]);
                CmbPaymentMode.Text = Val.ToString(DRow["PAYMENTMODE"]);
                cmbBillType.Text = Val.ToString(DRow["BILLTYPE"]);
                StrMainBillType = Val.ToString(DRow["BILLTYPE"]);

                DTPTermsDate.Text = DateTime.Parse(DRow["TERMSDATE"].ToString()).ToShortDateString();

                txtCurrency.Tag = Val.ToString(DRow["CURRENCY_ID"]);
                txtCurrency.Text = Val.ToString(DRow["CURRENCYNAME"]);
                txtExcRate.Text = Val.ToString(DRow["EXCRATE"]);

                GrdDetail.Columns["FMEMOPRICEPERCARAT"].Caption = "  " + Val.ToString(DRow["SYMBOL"]) + "/Cts";
                GrdDetail.Columns["FMEMOAMOUNT"].Caption = "  Amt (" + Val.ToString(DRow["SYMBOL"]) + ")";


                txtBAddress1.Text = Val.ToString(DRow["BILLINGADDRESS1"]);
                txtBAddress2.Text = Val.ToString(DRow["BILLINGADDRESS2"]);
                txtBAddress3.Text = Val.ToString(DRow["BILLINGADDRESS3"]);
                txtBCountry.Tag = Val.ToString(DRow["BILLINGCOUNTRY_ID"]);
                txtBCountry.Text = Val.ToString(DRow["BILLINGCOUNTRYNAME"]);
                txtBState.Text = Val.ToString(DRow["BILLINGSTATE"]);
                txtBCity.Text = Val.ToString(DRow["BILLINGCITY"]);
                txtBZipCode.Text = Val.ToString(DRow["BILLINGZIPCODE"]);

                txtSAddress1.Text = Val.ToString(DRow["SHIPPINGADDRESS1"]);
                txtSAddress2.Text = Val.ToString(DRow["SHIPPINGADDRESS2"]);
                txtSAddress3.Text = Val.ToString(DRow["SHIPPINGADDRESS3"]);
                txtSCountry.Tag = Val.ToString(DRow["SHIPPINGCOUNTRY_ID"]);
                txtSCountry.Text = Val.ToString(DRow["SHIPPINGCOUNTRYNAME"]);
                txtSState.Text = Val.ToString(DRow["SHIPPINGSTATE"]);
                txtSCity.Text = Val.ToString(DRow["SHIPPINGCITY"]);
                txtSZipCode.Text = Val.ToString(DRow["SHIPPINGZIPCODE"]);

                txtRemark.Text = Val.ToString(DRow["REMARK"]);
                lblSource.Text = Val.ToString(DRow["SOURCE"]);

                txtTransport.Text = Val.ToString(DRow["TRANSPORTNAME"]);
                txtPlaceOfSupply.Text = Val.ToString(DRow["PLACEOFSUPPLY"]);

                txtCompanyBank.Tag = Val.ToString(DRow["COMPANYBANK_ID"]);
                txtCompanyBank.Text = Val.ToString(DRow["COMPANYBANKNAME"]);
                txtPartyBank.Tag = Val.ToString(DRow["PARTYBANK_ID"]);
                txtPartyBank.Text = Val.ToString(DRow["PARTYBANKNAME"]);
                txtCourier.Tag = Val.ToString(DRow["COURIER_ID"]);
                txtCourier.Text = Val.ToString(DRow["COURIERNAME"]);
                txtAirFreight.Tag = Val.ToString(DRow["AIRFREIGHT_ID"]);
                txtAirFreight.Text = Val.ToString(DRow["AIRFREIGHTNAME"]);
                txtPlaceOfReceiptByPreCarrier.Text = Val.ToString(DRow["PLACEOFRECEIPTBYPRECARRIER"]);
                txtPortOfLoading.Text = Val.ToString(DRow["PORTOFLOADING"]);
                txtPortOfDischarge.Text = Val.ToString(DRow["PORTOFDISCHARGE"]);
                txtFinalDestination.Text = Val.ToString(DRow["BILLINGCITY"]);
                txtCountryOfOrigin.Text = Val.ToString(DRow["COUNTRYOFORIGIN"]);
                txtCountryOfFinalDestination.Text = Val.ToString(DRow["COUNTRYOFFINALDESTINATION"]);
                txtVoucherNoStr.Text = Val.ToString(DRow["VOUCHERNOSTR"]);

                txtGrossAmount.Text = Val.ToString(DRow["GROSSAMOUNT"]);
                txtGrossAmountFE.Text = Val.ToString(DRow["FGROSSAMOUNT"]);

                txtDiscPer.Text = Val.ToString(DRow["DISCOUNTPER"]);
                txtDiscAmount.Text = Val.ToString(DRow["DISCOUNTAMOUNT"]);
                txtDiscAmountFE.Text = Val.ToString(DRow["FDISCOUNTAMOUNT"]);

                txtInsurancePer.Text = Val.ToString(DRow["INSURANCEPER"]);
                txtInsuranceAmount.Text = Val.ToString(DRow["INSURANCEAMOUNT"]);
                txtInsuranceAmountFE.Text = Val.ToString(DRow["FINSURANCEAMOUNT"]);

                txtShippingPer.Text = Val.ToString(DRow["SHIPPINGPER"]);
                txtShippingAmount.Text = Val.ToString(DRow["SHIPPINGAMOUNT"]);
                txtShippingAmountFE.Text = Val.ToString(DRow["FSHIPPINGAMOUNT"]);

                txtGSTPer.Text = Val.ToString(DRow["GSTPER"]);
                txtGSTAmount.Text = Val.ToString(DRow["GSTAMOUNT"]);
                txtGSTAmountFE.Text = Val.ToString(DRow["FGSTAMOUNT"]);

                txtIGSTPer.Text = Val.ToString(DRow["IGSTPER"]);
                txtIGSTAmount.Text = Val.ToString(DRow["IGSTAMOUNT"]);
                txtIGSTAmountFE.Text = Val.ToString(DRow["FIGSTAMOUNT"]);
                txtCGSTPer.Text = Val.ToString(DRow["CGSTPER"]);
                txtCGSTAmount.Text = Val.ToString(DRow["CGSTAMOUNT"]);
                txtCGSTAmountFE.Text = Val.ToString(DRow["FCGSTAMOUNT"]);
                txtSGSTPer.Text = Val.ToString(DRow["SGSTPER"]);
                txtSGSTAmount.Text = Val.ToString(DRow["SGSTAMOUNT"]);
                txtSGSTAmountFE.Text = Val.ToString(DRow["FSGSTAMOUNT"]);

                ////#K : 15122020
                //txtTCSPer.Text = Val.ToString(DRow["TCSPER"]);
                //txtTCSAmount.Text = Val.ToString(DRow["TCSAMOUNT"]);
                //txtTCSAmountFE.Text = Val.ToString(DRow["FTCSAMOUNT"]);
                //txtRoundOffPer.Text = Val.ToString(DRow["ROUNDPER"]);
                //txtRoundOffAmount.Text = Val.ToString(DRow["ROUNDAMOUNT"]);
                //txtRoundOffAmountFE.Text = Val.ToString(DRow["FROUNDAMOUNT"]);
                //CmbRoundOff.SelectedValue = Val.ToString(DRow["ROUNDTYPE"]);

                txtNetAmount.Text = Val.ToString(DRow["NETAMOUNT"]);
                txtNetAmountFE.Text = Val.ToString(DRow["FNETAMOUNT"]);

                txtRemark.Text = Val.ToString(DRow["REMARK"]);
                txtTransport.Text = Val.ToString(DRow["TRANSPORTNAME"]);
                txtPlaceOfSupply.Text = Val.ToString(DRow["PLACEOFSUPPLY"]);

                lblTotalPcs.Text = Val.ToString(DRow["TOTALPCS"]);
                lblTotalCarat.Text = Val.ToString(DRow["TOTALCARAT"]);
                txtMemoAvgDisc.Text = Val.ToString(DRow["TOTALAVGDISC"]);
                txtMemoAvgRate.Text = Val.ToString(DRow["TOTALAVGRATE"]);

                lblTitle.Text = Val.ToString(DRow["PROCESSNAME"]);
                lblTitle.Tag = Val.ToString(DRow["PROCESS_ID"]);


                /*
    1	NONE	NONE
    3	PUR	PURCHASE
    4	PURRET	PURCHASE RETURN
    5	MKMO	MEMO ISSUE
    6	MORET	MEMO RETURN
    7	HOLD	HOLD
    8	REALEASE	RELEASE
    9	OFFLINE	OFFLINE
    10	INV	SALES DELIVERY
    11	INVRET	SALES RETURN
    12	SOLD	SALES ORDER
    13	PRICE	PRICING
    14	ONLINE	ONLINE
    15	PARAMUPD	PARAMETER UPDATE
                 */

                //if (Val.ToInt(lblTitle.Tag) == 3) mFormType = FORMTYPE.PURCHASEISSUE;
                //else if (Val.ToInt(lblTitle.Tag) == 4) mFormType = FORMTYPE.PURCHASERETURN;
                //else if (Val.ToInt(lblTitle.Tag) == 5) mFormType = FORMTYPE.MEMOISSUE;
                //else if (Val.ToInt(lblTitle.Tag) == 6) mFormType = FORMTYPE.MEMORETURN;
                //else if (Val.ToInt(lblTitle.Tag) == 7) mFormType = FORMTYPE.HOLD;
                //else if (Val.ToInt(lblTitle.Tag) == 8) mFormType = FORMTYPE.RELEASE;
                //else if (Val.ToInt(lblTitle.Tag) == 9) mFormType = FORMTYPE.OFFLINE;
                //else if (Val.ToInt(lblTitle.Tag) == 10) mFormType = FORMTYPE.SALEINVOICE;
                //else if (Val.ToInt(lblTitle.Tag) == 11) mFormType = FORMTYPE.SALESRETURN;
                //else if (Val.ToInt(lblTitle.Tag) == 12) mFormType = FORMTYPE.SALESORDER;
                //else if (Val.ToInt(lblTitle.Tag) == 14) mFormType = FORMTYPE.ONLINE;


                if (Val.ToInt(lblTitle.Tag) == 2) mFormType = FORMTYPE.PURCHASEISSUE;
                else if (Val.ToInt(lblTitle.Tag) == 3) mFormType = FORMTYPE.PURCHASERETURN;
                else if (Val.ToInt(lblTitle.Tag) == 4) mFormType = FORMTYPE.MEMOISSUE;
                else if (Val.ToInt(lblTitle.Tag) == 5) mFormType = FORMTYPE.MEMORETURN;
                else if (Val.ToInt(lblTitle.Tag) == 6) mFormType = FORMTYPE.HOLD;
                else if (Val.ToInt(lblTitle.Tag) == 7) mFormType = FORMTYPE.RELEASE;
                else if (Val.ToInt(lblTitle.Tag) == 8) mFormType = FORMTYPE.OFFLINE;
                else if (Val.ToInt(lblTitle.Tag) == 9) mFormType = FORMTYPE.SALEINVOICE;
                else if (Val.ToInt(lblTitle.Tag) == 10) mFormType = FORMTYPE.SALESDELIVERYRETURN;
                else if (Val.ToInt(lblTitle.Tag) == 11) mFormType = FORMTYPE.ORDERCONFIRM;
                else if (Val.ToInt(lblTitle.Tag) == 13) mFormType = FORMTYPE.ONLINE;
                else if (Val.ToInt(lblTitle.Tag) == 15) mFormType = FORMTYPE.ORDERCONFIRMRETURN;
                else if (Val.ToInt(lblTitle.Tag) == 19) mFormType = FORMTYPE.LABISSUE;
                else if (Val.ToInt(lblTitle.Tag) == 21) mFormType = FORMTYPE.LABRETURN;
                else if (Val.ToInt(lblTitle.Tag) == 26) mFormType = FORMTYPE.CONSIGNMENTISSUE;
                else if (Val.ToInt(lblTitle.Tag) == 27) mFormType = FORMTYPE.CONSIGNMENTRETURN;

                //mStrStockType = (mFormType == FORMTYPE.PURCHASEISSUE & mStrStockType == "SINGLE") ? "SINGLE" : ((mFormType == FORMTYPE.PURCHASEISSUE & mStrStockType == "PARCEL") ? "PARCEL" : "ALL");

                //#P : 13-01-2020
                lblStockType.Text = mStrStockType + " STOCK";
                if (mStrStockType == "PARCEL")
                {
                    GrdDetail.Columns["POLNAME"].Visible = false;
                    GrdDetail.Columns["SYMNAME"].Visible = false;
                    GrdDetail.Columns["FLNAME"].Visible = false;
                    GrdDetail.Columns["LABREPORTNO"].Visible = false;

                    if (Val.ToString(lblMode.Text).ToUpper() == "EDIT MODE")
                    {
                        GrdDetail.Columns["RETURNPCS"].Visible = true;
                        GrdDetail.Columns["RETURNCARAT"].Visible = true;
                    }
                    else
                    {
                        GrdDetail.Columns["RETURNPCS"].Visible = false;
                        GrdDetail.Columns["RETURNCARAT"].Visible = false;
                    }
                    GrdDetail.Columns["SALERAPAPORT"].Visible = false;
                    GrdDetail.Columns["MEMORAPAPORT"].Visible = false;
                    GrdDetail.Columns["SALEDISCOUNT"].Visible = false;
                    GrdDetail.Columns["MEMODISCOUNT"].Visible = false;
                }
                else
                {
                    GrdDetail.Columns["POLNAME"].Visible = true;
                    GrdDetail.Columns["SYMNAME"].Visible = true;
                    GrdDetail.Columns["FLNAME"].Visible = true;
                    GrdDetail.Columns["LABREPORTNO"].Visible = true;

                    GrdDetail.Columns["SALERAPAPORT"].Visible = true;
                    GrdDetail.Columns["MEMORAPAPORT"].Visible = true;
                    GrdDetail.Columns["MEMORAPAPORT"].Fixed = FixedStyle.Right;  //#P
                    GrdDetail.Columns["SALEDISCOUNT"].Visible = true;
                    GrdDetail.Columns["MEMODISCOUNT"].Visible = true;

                    GrdDetail.Columns["RETURNPCS"].Visible = false;
                    GrdDetail.Columns["RETURNCARAT"].Visible = false;

                }
                //End : #P : 13-01-2020

                //#P : 05-05-2021: Coz JangedDisc And PerCts prthi Sale Amt Consider thay 6e etle MemoDisc Editable nathi..
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                //#P : 05-05-2021

                //On : 20-02-2020
                //lblState.Visible = false;
                //txtBState.Visible = false;
                //lblZipCode.Visible = false;
                //txtBZipCode.Visible = false;
                //End : On : 20-02-2020

                FillControlName();
                //Calculation();
                CalculationNew();

                CmbMemoType.SelectedItem = Val.ToString(DRow["MEMOTYPE"]);


                if (Val.ToString(lblMode.Text).ToUpper() == "EDIT MODE") //#P : 06-10-2019
                {
                    GrdDetail.BeginUpdate();
                    if (MainGrdDetail.RepositoryItems.Count == 17)
                    //if (!GrdDetail.Columns.Contains(GrdDetail.Columns["COLSELECTCHECKBOX"]))
                    {
                        ObjGridSelection = new BODevGridSelection();
                        ObjGridSelection.View = GrdDetail;
                        ObjGridSelection.ClearSelection();
                        ObjGridSelection.CheckMarkColumn.VisibleIndex = 1;
                    }
                    else
                    {
                        ObjGridSelection.ClearSelection();
                    }
                    GrdDetail.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;

                    GrdDetail.Bands["BANDSTOCK"].Fixed = FixedStyle.None;
                    GridBand band = GrdDetail.Bands.AddBand("..");
                    band.Columns.Add(GrdDetail.Columns["COLSELECTCHECKBOX"]);
                    band.Fixed = FixedStyle.Left;
                    band.VisibleIndex = 0;

                    GrdDetail.Bands["BANDSTOCK"].Fixed = FixedStyle.Left;
                    //GrdDetail.Bands["BANDSTOCK"].VisibleIndex = 1;
                    if (ObjGridSelection != null)
                    {
                        ObjGridSelection.ClearSelection();
                        ObjGridSelection.CheckMarkColumn.VisibleIndex = 1;
                    }
                    GrdDetail.EndUpdate();
                }

                //#K: 05122020
                ChkApprovedOrder.Visible = false;
                if (mFormType == FORMTYPE.ORDERCONFIRM)
                    ChkApprovedOrder.Visible = true;

                DTPMemoDate.Focus();
                if (mFormType != FORMTYPE.SALEINVOICE && mFormType != FORMTYPE.SALESDELIVERYRETURN && mFormType != FORMTYPE.PURCHASERETURN && mFormType != FORMTYPE.PURCHASEISSUE)
                    xtraTabControl1.TabPages.Remove(xtraTabPage3);
                this.Cursor = Cursors.Default;

            }
            catch (Exception ex)
            {
                Global.Message(ex.ToString());
            }
        }
        //End :  #P : 30-07-2020

        public void ShowForm(string pStrMemoID, string pStrStockType = "ALL", string FormTypeHk = "")  //Call When Double Click On Current Stock Color Clarity Wise Report Data
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                Val.FormGeneralSetting(this);
                AttachFormDefaultEvent();
                this.Show();
                BtnReturn.Enabled = true;
                BtnOtherActivity.Enabled = true;
                BtnClear_Click(null, null);
                mStrStockType = pStrStockType;
                mStrFormTypeHK = FormTypeHk;
                DataSet DS = ObjMemo.GetMemoListData(0, null, null, "", "", "", 0, "", 0, "", "ALL", pStrMemoID, mStrStockType, false, 0);

                lblStockType.Text = mStrStockType + " STOCK";
                DTabMemo = DS.Tables[0];
                DTabMemoDetail = DS.Tables[1];
                DTabMemoDetail.Columns.Add("OLDMEMODISCOUNT", typeof(double));
                DTabMemoDetail.Columns.Add("OLDMEMOPRICEPERCARAT", typeof(double));
                MainGrdDetail.DataSource = DTabMemoDetail;
                MainGrdDetail.Refresh();

                if (DTabMemo.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("THERE IS NO ANY MEMO DATA FOUND");
                    return;
                }

                cmbBillType.Items.Clear();
                cmbBillType.Items.Add("None");
                cmbBillType.Items.Add("DollarBill");
                cmbBillType.Items.Add("Cash");
                cmbBillType.Items.Add("Export");
                cmbBillType.Items.Add("Consignment");
                cmbBillType.SelectedIndex = 0;

                CmbPaymentMode.Items.Clear();
                CmbPaymentMode.Items.Add("None");
                CmbPaymentMode.Items.Add("Cash");
                CmbPaymentMode.Items.Add("Cheque");
                CmbPaymentMode.Items.Add("Bank/WeChat Transfer");
                CmbPaymentMode.SelectedIndex = 0;

                cmbAccType.Items.Clear();
                cmbAccType.Items.Add("None");
                cmbAccType.Items.Add("Diamond Dollar");
                cmbAccType.Items.Add("Export");
                cmbAccType.Items.Add("Consignment");
                cmbAccType.SelectedIndex = 0;

                DataRow DRow = DTabMemo.Rows[0];

                lblMode.Text = "Edit Mode";
                txtJangedNo.Text = Val.ToString(DRow["JANGEDNOSTR"]);
                txtJangedNo.Tag = Val.ToString(DRow["MEMO_ID"]);
                lblMemoNo.Tag = Val.ToString(DRow["MEMO_ID"]);
                lblMemoNo.Text = Val.ToString(DRow["MEMONO"]);
                lblStatus.Text = Val.ToString(DRow["STATUS"]);

                DTPMemoDate.Text = Val.ToString(DRow["MEMODATE"]);
                DTPMemoDate.Text = Val.ToDate(DateTime.Parse(DRow["MEMODATE"].ToString()), AxonDataLib.BOConversion.DateFormat.DDMMYYYY);

                txtBillingParty.Tag = Val.ToString(DRow["BILLINGPARTY_ID"]);
                txtBillingParty.Text = Val.ToString(DRow["BILLINGPARTYNAME"]);

                txtShippingParty.Tag = Val.ToString(DRow["SHIPPINGPARTY_ID"]);
                txtShippingParty.Text = Val.ToString(DRow["SHIPPINGPARTYNAME"]);

                txtSellerName.Tag = Val.ToString(DRow["SELLER_ID"]);
                txtSellerName.Text = Val.ToString(DRow["SELLERNAME"]);

                txtBroker.Tag = Val.ToString(DRow["BROKER_ID"]);
                txtBroker.Text = Val.ToString(DRow["BROKERNAME"]);
                txtBaseBrokeragePer.Text = Val.ToString(DRow["BROKRAGEPER"]);
                txtProfitBrokeragePer.Text = Val.ToString(DRow["BROKRAGEPROFITPER"]);

                txtAdat.Tag = Val.ToString(DRow["ADAT_ID"]);
                txtAdat.Text = Val.ToString(DRow["ADATNAME"]);
                txtAdatPer.Text = Val.ToString(DRow["ADATPER"]);

                txtTerms.Tag = Val.ToString(DRow["TERMS_ID"]);
                txtTerms.Text = Val.ToString(DRow["TERMSNAME"]);
                txtTermsDays.Text = Val.ToString(DRow["TERMSDAYS"]);

                CreateSummaryTable();
                if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.SALESDELIVERYRETURN)
                {

                    DataRow Drow = DTabMemoSummury.NewRow();
                    DTabMemoSummury.Rows.Add(Drow);

                    GetSummuryDetailForGrid();
                    MainGridSummury.DataSource = DTabMemoSummury;
                    MainGridSummury.Refresh();
                }
                else
                {
                    xtraTabControl1.TabPages.Remove(xtraTabPage5);
                    MainGridSummury.DataSource = null;
                    MainGridSummury.Refresh();
                }

                CmbDeliveryType.Text = Val.ToString(DRow["DELIVERYTYPE"]);
                CmbPaymentMode.Text = Val.ToString(DRow["PAYMENTMODE"]);
                cmbBillType.Text = Val.ToString(DRow["BILLTYPE"]);


                if (cmbBillType.Text.ToUpper() == "EXPORT")
                {
                    foreach (DataRow DR in DTabMemoDetail.Rows)
                    {
                        if (DR["MEMOPRICEPERCARAT"] != DR["EXPINVOICERATE"])
                        {
                            ChkUpdExport.Checked = true;
                        }
                    }
                }
                cmbAccType.Text = Val.ToString(DRow["ACCTYPE"]);

                StrMainBillType = Val.ToString(DRow["BILLTYPE"]);

                DTPTermsDate.Text = DateTime.Parse(DRow["TERMSDATE"].ToString()).ToShortDateString();

                txtCurrency.Tag = Val.ToString(DRow["CURRENCY_ID"]);
                txtCurrency.Text = Val.ToString(DRow["CURRENCYNAME"]);
                txtExcRate.Text = Val.ToString(DRow["EXCRATE"]);
                txtOrgExcRate.Text = Val.ToString(DRow["ORG_EXCRATE"]);
                txtAddLessExcRate.Text = Val.ToString(DRow["ADDLESS_EXCRATE"]);
                txtBrNo.Text = Val.ToString(DRow["BRNO"]);

                //GrdDetail.Columns["FMEMOPRICEPERCARAT"].Caption = "  " + Val.ToString(DRow["SYMBOL"]) + "/Cts";
                //GrdDetail.Columns["FMEMOAMOUNT"].Caption = "  Amt (" + Val.ToString(DRow["SYMBOL"]) + ")";


                txtBAddress1.Text = Val.ToString(DRow["BILLINGADDRESS1"]);
                txtBAddress2.Text = Val.ToString(DRow["BILLINGADDRESS2"]);
                txtBAddress3.Text = Val.ToString(DRow["BILLINGADDRESS3"]);
                txtBCountry.Tag = Val.ToString(DRow["BILLINGCOUNTRY_ID"]);
                txtBCountry.Text = Val.ToString(DRow["BILLINGCOUNTRYNAME"]);
                txtBState.Text = Val.ToString(DRow["BILLINGSTATE"]);
                txtBCity.Text = Val.ToString(DRow["BILLINGCITY"]);
                txtBZipCode.Text = Val.ToString(DRow["BILLINGZIPCODE"]);

                txtSAddress1.Text = Val.ToString(DRow["SHIPPINGADDRESS1"]);
                txtSAddress2.Text = Val.ToString(DRow["SHIPPINGADDRESS2"]);
                txtSAddress3.Text = Val.ToString(DRow["SHIPPINGADDRESS3"]);
                txtSCountry.Tag = Val.ToString(DRow["SHIPPINGCOUNTRY_ID"]);
                txtSCountry.Text = Val.ToString(DRow["SHIPPINGCOUNTRYNAME"]);
                txtSState.Text = Val.ToString(DRow["SHIPPINGSTATE"]);
                txtSCity.Text = Val.ToString(DRow["SHIPPINGCITY"]);
                txtSZipCode.Text = Val.ToString(DRow["SHIPPINGZIPCODE"]);

                txtRemark.Text = Val.ToString(DRow["REMARK"]);
                lblSource.Text = Val.ToString(DRow["SOURCE"]);

                txtTransport.Text = Val.ToString(DRow["TRANSPORTNAME"]);
                txtPlaceOfSupply.Text = Val.ToString(DRow["PLACEOFSUPPLY"]);

                txtCompanyBank.Tag = Val.ToString(DRow["COMPANYBANK_ID"]);
                txtCompanyBank.Text = Val.ToString(DRow["COMPANYBANKNAME"]);
                txtPartyBank.Tag = Val.ToString(DRow["PARTYBANK_ID"]);
                txtPartyBank.Text = Val.ToString(DRow["PARTYBANKNAME"]);
                txtCourier.Tag = Val.ToString(DRow["COURIER_ID"]);
                txtCourier.Text = Val.ToString(DRow["COURIERNAME"]);
                txtAirFreight.Tag = Val.ToString(DRow["AIRFREIGHT_ID"]);
                txtAirFreight.Text = Val.ToString(DRow["AIRFREIGHTNAME"]);


                if (Val.ToString(cmbAddresstype.SelectedItem) == "MUMBAI")
                {
                    txtPortOfLoading.Text = Val.ToString(DRow["BILLINGCITY"]);
                    txtPlaceOfReceiptByPreCarrier.Text = Val.ToString(DRow["PLACEOFRECEIPTBYPRECARRIER"]);

                }
                else
                {
                    txtPortOfLoading.Text = Val.ToString(DRow["SHIPPINGCITY"]);
                    txtPlaceOfReceiptByPreCarrier.Text = Val.ToString(DRow["PLACEOFRECEIPTBYPRECARRIER"]);

                }

                txtPortOfDischarge.Text = Val.ToString(DRow["PORTOFDISCHARGE"]);
                txtFinalDestination.Text = Val.ToString(DRow["BILLINGCITY"]);
                txtCountryOfOrigin.Text = Val.ToString(DRow["COUNTRYOFORIGIN"]);
                txtCountryOfFinalDestination.Text = Val.ToString(DRow["COUNTRYOFFINALDESTINATION"]);
                txtVoucherNoStr.Text = Val.ToString(DRow["VOUCHERNOSTR"]);

                txtGrossAmount.Text = Val.ToString(DRow["GROSSAMOUNT"]);
                txtGrossAmountFE.Text = Val.ToString(DRow["FGROSSAMOUNT"]);

                txtDiscPer.Text = Val.ToString(DRow["DISCOUNTPER"]);
                txtDiscAmount.Text = Val.ToString(DRow["DISCOUNTAMOUNT"]);
                txtDiscAmountFE.Text = Val.ToString(DRow["FDISCOUNTAMOUNT"]);

                txtInsurancePer.Text = Val.ToString(DRow["INSURANCEPER"]);
                txtInsuranceAmount.Text = Val.ToString(DRow["INSURANCEAMOUNT"]);
                txtInsuranceAmountFE.Text = Val.ToString(DRow["FINSURANCEAMOUNT"]);

                txtShippingPer.Text = Val.ToString(DRow["SHIPPINGPER"]);
                txtShippingAmount.Text = Val.ToString(DRow["SHIPPINGAMOUNT"]);
                txtShippingAmountFE.Text = Val.ToString(DRow["FSHIPPINGAMOUNT"]);

                txtGSTPer.Text = Val.ToString(DRow["GSTPER"]);
                txtGSTAmount.Text = Val.ToString(DRow["GSTAMOUNT"]);
                txtGSTAmountFE.Text = Val.ToString(DRow["FGSTAMOUNT"]);

                txtIGSTPer.Text = Val.ToString(DRow["IGSTPER"]);
                txtIGSTAmount.Text = Val.ToString(DRow["IGSTAMOUNT"]);
                txtIGSTAmountFE.Text = Val.ToString(DRow["FIGSTAMOUNT"]);
                txtCGSTPer.Text = Val.ToString(DRow["CGSTPER"]);
                txtCGSTAmount.Text = Val.ToString(DRow["CGSTAMOUNT"]);
                txtCGSTAmountFE.Text = Val.ToString(DRow["FCGSTAMOUNT"]);
                txtSGSTPer.Text = Val.ToString(DRow["SGSTPER"]);
                txtSGSTAmount.Text = Val.ToString(DRow["SGSTAMOUNT"]);
                txtSGSTAmountFE.Text = Val.ToString(DRow["FSGSTAMOUNT"]);

                txtNetAmount.Text = Val.ToString(DRow["NETAMOUNT"]);
                txtNetAmountFE.Text = Val.ToString(DRow["FNETAMOUNT"]);

                txtRemark.Text = Val.ToString(DRow["REMARK"]);
                txtTransport.Text = Val.ToString(DRow["TRANSPORTNAME"]);
                txtPlaceOfSupply.Text = Val.ToString(DRow["PLACEOFSUPPLY"]);

                lblTotalPcs.Text = Val.ToString(DRow["TOTALPCS"]);
                lblTotalCarat.Text = Val.ToString(DRow["TOTALCARAT"]);
                txtMemoAvgDisc.Text = Val.ToString(DRow["TOTALAVGDISC"]);
                txtMemoAvgRate.Text = Val.ToString(DRow["TOTALAVGRATE"]);

                lblTitle.Text = Val.ToString(DRow["PROCESSNAME"]);
                lblTitle.Tag = Val.ToString(DRow["PROCESS_ID"]);

                txtGrossWeight.Text = Val.ToString(DRow["GROSSWEIGHT"]);
                cmbInsuranceType.Text = Val.ToString(DRow["INSURANCETYPE"]);

                // #D: 19-08-2020

                txtconsignee.Text = Val.ToString(DRow["CONSIGNEE"]);
                cmbAddresstype.Text = Val.ToString(DRow["ADDRESSTYPE"]);
                if (Val.ToBooleanToInt(DRow["APPROVAL"]) == 1)
                    ChkApprovedOrder.Checked = true;
                else
                    ChkApprovedOrder.Checked = false;
                // #D: 19-08-2020

                // #d: 13-01-2021
                txtLabServiceCode.Text = Val.ToString(DRow["LABSERVICECODE"]);
                txtLabServiceCode.Tag = Val.ToInt32(DRow["LABSERVICECODE_ID"]);

                txtNarration.Text = Val.ToString(DRow["NARRATIONNAME"]);
                txtNarration.Tag = Val.ToInt32(DRow["NARRATION_ID"]);

                // #d: 13-01-2021

                txtBackAddLess.Text = Val.ToString(DRow["BACKADDLESS"]);
                txtTermsAddLessPer.Text = Val.ToString(DRow["TERMSADDLESSPER"]);
                txtBlindAddLessPer.Text = Val.ToString(DRow["BLINDADDLESSPER"]);



                txtInvoiceNo.Text = Val.ToString(DRow["INVOICENO"]);
                if (BOConfiguration.gStrLoginSection == "B")
                {
                    txtApproveMemoNo.Visible = true;
                    txtOrderMemoNo.Visible = true;
                    txtApproveMemoNo.Text = Val.ToString(DRow["APPROVEMEMONO"]);
                    txtOrderMemoNo.Text = Val.ToString(DRow["ORDERMEMONO"]);
                    PstrApprove_MemoId = Val.ToString(DRow["MEMO_ID"]);
                }

                /*
    1	NONE	NONE
    3	PUR	PURCHASE
    4	PURRET	PURCHASE RETURN
    5	MKMO	MEMO ISSUE
    6	MORET	MEMO RETURN
    7	HOLD	HOLD
    8	REALEASE	RELEASE
    9	OFFLINE	OFFLINE
    10	INV	SALES DELIVERY
    11	INVRET	SALES RETURN
    12	SOLD	SALES ORDER
    13	PRICE	PRICING
    14	ONLINE	ONLINE
    15	PARAMUPD	PARAMETER UPDATE
                 */

                if (Val.ToInt(lblTitle.Tag) == 2) mFormType = FORMTYPE.PURCHASEISSUE;
                else if (Val.ToInt(lblTitle.Tag) == 3) mFormType = FORMTYPE.PURCHASERETURN;
                else if (Val.ToInt(lblTitle.Tag) == 4) mFormType = FORMTYPE.MEMOISSUE;
                else if (Val.ToInt(lblTitle.Tag) == 5) mFormType = FORMTYPE.MEMORETURN;
                else if (Val.ToInt(lblTitle.Tag) == 6) mFormType = FORMTYPE.HOLD;
                else if (Val.ToInt(lblTitle.Tag) == 7) mFormType = FORMTYPE.RELEASE;
                else if (Val.ToInt(lblTitle.Tag) == 8) mFormType = FORMTYPE.OFFLINE;
                else if (Val.ToInt(lblTitle.Tag) == 9) mFormType = FORMTYPE.SALEINVOICE;
                else if (Val.ToInt(lblTitle.Tag) == 10) mFormType = FORMTYPE.SALESDELIVERYRETURN;
                else if (Val.ToInt(lblTitle.Tag) == 11) mFormType = FORMTYPE.ORDERCONFIRM;
                else if (Val.ToInt(lblTitle.Tag) == 13) mFormType = FORMTYPE.ONLINE;
                else if (Val.ToInt(lblTitle.Tag) == 15) mFormType = FORMTYPE.ORDERCONFIRMRETURN;
                else if (Val.ToInt(lblTitle.Tag) == 19) mFormType = FORMTYPE.LABISSUE;
                else if (Val.ToInt(lblTitle.Tag) == 21) mFormType = FORMTYPE.LABRETURN;
                else if (Val.ToInt(lblTitle.Tag) == 26) mFormType = FORMTYPE.CONSIGNMENTISSUE;
                else if (Val.ToInt(lblTitle.Tag) == 27) mFormType = FORMTYPE.CONSIGNMENTRETURN;

                //#P : 13-01-2020
                lblStockType.Text = mStrStockType + " STOCK";
                if (mStrStockType == "PARCEL")
                {
                    GrdDetail.Columns["POLNAME"].Visible = false;
                    GrdDetail.Columns["SYMNAME"].Visible = false;
                    GrdDetail.Columns["FLNAME"].Visible = false;
                    GrdDetail.Columns["LABREPORTNO"].Visible = false;

                    if (Val.ToString(lblMode.Text).ToUpper() == "EDIT MODE")
                    {
                        GrdDetail.Columns["RETURNPCS"].Visible = true;
                        GrdDetail.Columns["RETURNCARAT"].Visible = true;
                    }
                    else
                    {
                        GrdDetail.Columns["RETURNPCS"].Visible = false;
                        GrdDetail.Columns["RETURNCARAT"].Visible = false;
                    }
                    GrdDetail.Columns["SALERAPAPORT"].Visible = false;
                    GrdDetail.Columns["MEMORAPAPORT"].Visible = false;
                    GrdDetail.Columns["SALEDISCOUNT"].Visible = false;
                    GrdDetail.Columns["MEMODISCOUNT"].Visible = false;
                }
                else
                {
                    GrdDetail.Columns["POLNAME"].Visible = true;
                    GrdDetail.Columns["SYMNAME"].Visible = true;
                    GrdDetail.Columns["FLNAME"].Visible = true;
                    GrdDetail.Columns["LABREPORTNO"].Visible = true;

                    GrdDetail.Columns["SALERAPAPORT"].Visible = true;
                    GrdDetail.Columns["MEMORAPAPORT"].Visible = true;
                    GrdDetail.Columns["MEMORAPAPORT"].Fixed = FixedStyle.Right;  //#P
                    GrdDetail.Columns["SALEDISCOUNT"].Visible = true;
                    GrdDetail.Columns["MEMODISCOUNT"].Visible = true;

                    GrdDetail.Columns["RETURNPCS"].Visible = false;
                    GrdDetail.Columns["RETURNCARAT"].Visible = false;

                }
                //End : #P : 13-01-2020

                //#P : 05-05-2021: Coz JangedDisc And PerCts prthi Sale Amt Consider thay 6e etle MemoDisc Editable nathi..
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                //#P : 05-05-2021

                FillControlName();
                CalculationNew();

                CmbMemoType.SelectedItem = Val.ToString(DRow["MEMOTYPE"]);


                if (Val.ToString(lblMode.Text).ToUpper() == "EDIT MODE") //#P : 06-10-2019
                {
                    GrdDetail.BeginUpdate();
                    if (MainGrdDetail.RepositoryItems.Count == 20)
                    {
                        ObjGridSelection = new BODevGridSelection();
                        ObjGridSelection.View = GrdDetail;
                        ObjGridSelection.ClearSelection();
                        ObjGridSelection.CheckMarkColumn.VisibleIndex = 1;
                    }
                    else
                    {
                        ObjGridSelection.ClearSelection();
                    }
                    GrdDetail.Columns["COLSELECTCHECKBOX"].Fixed = FixedStyle.Left;

                    GrdDetail.Bands["BANDSTOCK"].Fixed = FixedStyle.None;
                    GridBand band = GrdDetail.Bands.AddBand("..");
                    band.Columns.Add(GrdDetail.Columns["COLSELECTCHECKBOX"]);
                    band.Fixed = FixedStyle.Left;
                    band.VisibleIndex = 0;

                    GrdDetail.Bands["BANDSTOCK"].Fixed = FixedStyle.Left;
                    if (ObjGridSelection != null)
                    {
                        ObjGridSelection.ClearSelection();
                        ObjGridSelection.CheckMarkColumn.VisibleIndex = 1;
                    }
                    GrdDetail.EndUpdate();
                }

                //#K: 05122020
                ChkApprovedOrder.Visible = false;
                if (mFormType == FORMTYPE.ORDERCONFIRM)
                    ChkApprovedOrder.Visible = true;

                DTPMemoDate.Focus();

                if (Val.ToString(DRow["BILLFORMAT"]) == RbDollar.Tag.ToString())
                {
                    RbDollar.Checked = true;
                    RbDollar_CheckedChanged(null, null);
                }
                else
                {
                    RbRupee.Checked = true;
                    RbRupee_CheckedChanged(null, null);
                }

                if (mFormType != FORMTYPE.SALEINVOICE && mFormType != FORMTYPE.SALESDELIVERYRETURN && mFormType != FORMTYPE.PURCHASERETURN && mFormType != FORMTYPE.PURCHASEISSUE)
                    xtraTabControl1.TabPages.Remove(xtraTabPage3);

                if (mFormType == FORMTYPE.LABISSUE)
                {
                    txtLabServiceCode.Visible = true;
                    BtnApplyAll.Visible = true;
                    lblLabServiceCode.Visible = true;
                }
                else if (mFormType == FORMTYPE.LABRETURN)
                {
                    txtLabServiceCode.Visible = true;
                    BtnApplyAll.Visible = true;
                    lblLabServiceCode.Visible = true;
                    txtLabServiceCode.Enabled = false;
                    BtnApplyAll.Enabled = false;
                    BtnApplyAll.Enabled = false;
                }

                if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.SALESDELIVERYRETURN)
                {
                    txtNarration.Visible = true;
                    lblNarration.Visible = true;
                    BtnNarrationApplyAll.Visible = true;
                    GrdDetail.Columns["NARRATIONNAME"].Visible = true;
                }
                else
                {
                    txtNarration.Visible = false;
                    lblNarration.Visible = false;
                    BtnNarrationApplyAll.Visible = false;
                    GrdDetail.Columns["NARRATIONNAME"].Visible = false;
                }

                xtraTabPage5.Focus();

                //if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRM)
                //{
                //    txtBuyer.Visible = true;
                //    lblBuyer.Visible = true;
                //}
                //else
                //{
                //    txtBuyer.Visible = false;
                //    lblBuyer.Visible = false;
                //}
                chkIsConsingee_CheckedChanged(null, null);


                txtBuyer.Text = Val.ToString(DRow["FINALBUYERNAME"]);
                txtBuyer.Tag = Val.ToString(DRow["FINALBUYER_ID"]);
                chkIsConsingee.Checked = Val.ToBoolean(DRow["ISCONSINGEE"]);
                if (Val.ToString(DRow["ISPICKUP"]) == "YES")
                    ChkOrderConfirmPickup.Checked = true;
                else
                    ChkOrderConfirmPickup.Checked = false;

                this.Cursor = Cursors.Default;

                if (BOConfiguration.gStrLoginSection == "B" && mFormType == FORMTYPE.ORDERCONFIRM)
                {
                    pnlButton.Enabled = false;
                }

                /*HIDE FIELD*/

                cLabel1.Visible = false;
                txtBAddress1.Visible = false;
                cLabel2.Visible = false;
                txtBAddress2.Visible = false;
                cLabel3.Visible = false;
                txtBAddress3.Visible = false;
                cLabel5.Visible = false;
                txtBCountry.Visible = false;
                lblState.Visible = false;
                txtBState.Visible = false;
                cLabel7.Visible = false;
                txtBCity.Visible = false;
                lblZipCode.Visible = false;
                txtBZipCode.Visible = false;
                cLabel35.Visible = false;
                txtCurrency.Visible = false;
                txtOrgExcRate.Visible = false;
                txtAddLessExcRate.Visible = false;
                txtExcRate.Visible = false;
                txtBroker.Text = string.Empty;
                txtBaseBrokeragePer.Text = string.Empty;
                txtBrokerAmtFE.Text = string.Empty;
                txtAdat.Text = string.Empty;
                txtAdatPer.Text = string.Empty;
                txtAdatAmtFE.Text = string.Empty;
                lblSeller.Visible = false;
                txtSellerName.Visible = false;
                ChkUpdExport.Visible = false;
                cLabel63.Visible = false;
                txtGrossWeight.Visible = false;
                lblNarration.Visible = true;
                txtNarration.Visible = true;
                ChkOrderConfirmPickup.Visible = false;

                /*END*/
            }
            catch (Exception ex)
            {
                Global.Message(ex.ToString());
            }
        }


        public void AttachFormDefaultEvent()
        {
            ObjFormEvent.mForm = this;
            ObjFormEvent.FormKeyDown = true;
            ObjFormEvent.FormKeyPress = true;
            ObjFormEvent.FormResize = true;
            ObjFormEvent.FormClosing = true;
            ObjFormEvent.ObjToDisposeList.Add(ObjFormEvent);
            ObjFormEvent.ObjToDisposeList.Add(Val);
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


        public void GetSelectedProcessIssue(string StrProcessName)
        {

            DataRow[] UDRow = DtabProcess.Select("PROCESSNAME = '" + StrProcessName + "'");

            if (UDRow.Length != 0)
            {
                this.Text = Val.ToString(UDRow[0]["PROCESSNAME"]);
                lblTitle.Text = Val.ToString(UDRow[0]["PROCESSNAME"]);

                lblTitle.Tag = Val.ToInt(UDRow[0]["PROCESS_ID"]);

                BtnSave.Enabled = true;
            }
            else
            {
                BtnSave.Enabled = false;
                Global.Message("NOT VALID ISSUE PROCESS FOUND");
                this.Text = string.Empty;
                lblTitle.Text = string.Empty;

                lblTitle.Tag = 0;

            }
        }

        public void GetSelectedProcessReturn(string StrProcessName)
        {

            DataRow[] UDRow = DtabProcess.Select("PROCESSNAME = '" + StrProcessName + "'");

            if (UDRow.Length != 0)
            {
                BtnReturn.Text = Val.ProperText(UDRow[0]["PROCESSNAME"]);
                BtnReturn.Tag = Val.ToInt(UDRow[0]["PROCESS_ID"]);
            }
            else
            {
                BtnSave.Enabled = false;
                Global.Message("NOT VALID RETURN PROCESS FOUND");
                BtnReturn.Text = string.Empty;
                BtnReturn.Tag = string.Empty;
            }
        }

        public void GetSelectedProcessOtherActivity(string StrProcessName)
        {
            //i think here i need to coding to

            DataRow[] UDRow = DtabProcess.Select("PROCESSNAME = '" + StrProcessName + "'");

            if (UDRow.Length != 0)
            {
                BtnOtherActivity.Text = Val.ProperText(UDRow[0]["PROCESSNAME"]);
                BtnOtherActivity.Tag = Val.ToInt(UDRow[0]["PROCESS_ID"]);
            }
            else
            {
                BtnSave.Enabled = false;
                Global.Message("NOT VALID OTHER ACTIVITY PROCESS FOUND");
                BtnOtherActivity.Text = string.Empty;
                BtnOtherActivity.Tag = string.Empty;
            }
        }

        public void FillControlName()
        {
            DtabProcess = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_PROCESSALL);

            CmbMemoType.Items.Clear();

            GrdDetail.Bands["BANDMEMOPRICE"].Fixed = FixedStyle.Right;

            if (mFormType == FORMTYPE.HOLD)
            {
                GetSelectedProcessIssue("HOLD");

                GrdDetail.Bands["BandLabService"].Visible = false;

                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["ISPURCHASE"].Visible = false;
                CmbMemoType.Items.Add("INTERNAL");
                CmbMemoType.Items.Add("EXTERNAL");

                if (lblMode.Text == "Add Mode")
                {
                    BtnReturn.Enabled = false;


                }
                else
                {
                    BtnReturn.Enabled = true;
                    GetSelectedProcessReturn("RELEASE");
                }
                BtnOtherActivity.Visible = false;

                GrdDetail.Columns["LABREPORTNO"].OptionsColumn.AllowEdit = false;
                lblSeller.Visible = true;
                txtSellerName.Visible = true;
            }
            else if (mFormType == FORMTYPE.RELEASE)
            {
                GetSelectedProcessIssue("RELEASE");

                GrdDetail.Bands["BandLabService"].Visible = false;
                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["ISPURCHASE"].Visible = false;
                CmbMemoType.Items.Add("INTERNAL");
                CmbMemoType.Items.Add("EXTERNAL");

                BtnReturn.Enabled = false;
                BtnOtherActivity.Visible = false;

                //Add : Pinali : 31-08-2019
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["LABREPORTNO"].OptionsColumn.AllowEdit = false;
                lblSeller.Visible = true;
                txtSellerName.Visible = true;
            }

            else if (mFormType == FORMTYPE.OFFLINE)
            {
                GetSelectedProcessIssue("OFFLINE");
                GrdDetail.Bands["BandLabService"].Visible = false;

                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["ISPURCHASE"].Visible = false;
                CmbMemoType.Items.Add("INTERNAL");
                CmbMemoType.Items.Add("EXTERNAL");

                if (lblMode.Text == "Add Mode")
                {
                    BtnReturn.Enabled = false;
                }
                else
                {
                    BtnReturn.Enabled = true;
                    GetSelectedProcessReturn("ONLINE");
                }
                BtnOtherActivity.Visible = false;
                GrdDetail.Columns["LABREPORTNO"].OptionsColumn.AllowEdit = false;
                lblSeller.Visible = true;
                txtSellerName.Visible = true;
            }

            else if (mFormType == FORMTYPE.ONLINE)
            {
                GetSelectedProcessIssue("ONLINE");
                GrdDetail.Bands["BandLabService"].Visible = false;

                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["ISPURCHASE"].Visible = false;
                CmbMemoType.Items.Add("INTERNAL");
                CmbMemoType.Items.Add("EXTERNAL");
                BtnReturn.Enabled = false;
                BtnOtherActivity.Visible = false;
                GrdDetail.Columns["LABREPORTNO"].OptionsColumn.AllowEdit = false;
                lblSeller.Visible = true;
                txtSellerName.Visible = true;
            }


            else if (mFormType == FORMTYPE.ORDERCONFIRM)
            {
                GetSelectedProcessIssue("ORDER CONFIRM");
                GrdDetail.Bands["BandLabService"].Visible = false;

                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;

                CmbMemoType.Items.Add("LOCAL");
                CmbMemoType.Items.Add("EXPORT");

                if (lblMode.Text == "Add Mode")
                {
                    BtnReturn.Enabled = false;
                    BtnOtherActivity.Enabled = false;
                }
                else
                {
                    BtnReturn.Enabled = true;
                    BtnOtherActivity.Enabled = true;
                    barButtonItem12.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
                    GetSelectedProcessReturn("ORDER CONFIRM RETURN");
                    GetSelectedProcessOtherActivity("SALES DELIVERY");
                }
                GrdDetail.Columns["LABREPORTNO"].OptionsColumn.AllowEdit = false;
                lblSeller.Visible = true;
                txtSellerName.Visible = true;
            }
            else if (mFormType == FORMTYPE.ORDERCONFIRMRETURN)
            {
                GrdDetail.Bands["BandLabService"].Visible = false;

                GetSelectedProcessIssue("ORDER CONFIRM RETURN");
                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;

                CmbMemoType.Items.Add("LOCAL");
                CmbMemoType.Items.Add("EXPORT");

                BtnReturn.Enabled = false;
                BtnOtherActivity.Visible = false;

                GrdDetail.Columns["LABREPORTNO"].OptionsColumn.AllowEdit = false;
                lblSeller.Visible = true;
                txtSellerName.Visible = true;
            }
            else if (mFormType == FORMTYPE.SALEINVOICE)
            {
                GetSelectedProcessIssue("SALES DELIVERY");
                GrdDetail.Bands["BandLabService"].Visible = false;

                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;

                CmbMemoType.Items.Add("LOCAL");
                CmbMemoType.Items.Add("EXPORT");

                if (lblMode.Text == "Add Mode")
                {
                    BtnReturn.Enabled = false;
                }
                else
                {
                    BtnReturn.Enabled = true;
                    GetSelectedProcessReturn("SALES DELIVERY RETURN");
                }
                BtnOtherActivity.Visible = false;

                GrdDetail.Columns["LABREPORTNO"].OptionsColumn.AllowEdit = false;
                lblSeller.Visible = true;
                txtSellerName.Visible = true;
            }
            else if (mFormType == FORMTYPE.SALESDELIVERYRETURN)
            {
                GrdDetail.Bands["BandLabService"].Visible = false;

                GetSelectedProcessIssue("SALES DELIVERY RETURN");
                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;
                CmbMemoType.Items.Add("LOCAL");
                CmbMemoType.Items.Add("EXPORT");

                BtnReturn.Enabled = false;
                BtnOtherActivity.Visible = false;
                //Add : Pinali : 31-08-2019
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["LABREPORTNO"].OptionsColumn.AllowEdit = false;
                lblSeller.Visible = true;
                txtSellerName.Visible = true;
            }

            else if (mFormType == FORMTYPE.MEMOISSUE)
            {
                GetSelectedProcessIssue("MEMO ISSUE");
                GrdDetail.Bands["BandLabService"].Visible = false;

                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;

                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;

                CmbMemoType.Items.Add("INTERNAL");
                CmbMemoType.Items.Add("EXTERNAL");

                if (lblMode.Text == "Add Mode")
                {
                    BtnReturn.Enabled = false;
                }
                else
                {
                    BtnReturn.Enabled = true;
                    GetSelectedProcessReturn("MEMO RETURN");
                }
                BtnOtherActivity.Visible = false;
                GrdDetail.Columns["LABREPORTNO"].OptionsColumn.AllowEdit = false;
                lblSeller.Visible = true;
                txtSellerName.Visible = true;
            }

            else if (mFormType == FORMTYPE.MEMORETURN)
            {
                GrdDetail.Bands["BandLabService"].Visible = false;

                GetSelectedProcessIssue("MEMO RETURN");
                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;
                CmbMemoType.Items.Add("INTERNAL");
                CmbMemoType.Items.Add("EXTERNAL");

                BtnReturn.Enabled = false;
                BtnOtherActivity.Visible = false;

                //Add : Pinali : 31-08-2019
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["LABREPORTNO"].OptionsColumn.AllowEdit = false;
                lblSeller.Visible = true;
                txtSellerName.Visible = true;
            }

            else if (mFormType == FORMTYPE.LABISSUE)
            {
                GetSelectedProcessIssue("LAB ISSUE");
                GrdDetail.Bands["BandLabService"].Visible = true;

                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;

                CmbMemoType.Items.Add("INTERNAL");
                CmbMemoType.Items.Add("EXTERNAL");

                if (lblMode.Text == "Add Mode")
                {
                    BtnReturn.Enabled = false;
                }
                else
                {
                    BtnReturn.Enabled = true;
                    GetSelectedProcessReturn("LAB RETURN");
                }
                BtnOtherActivity.Visible = false;
                GrdDetail.Columns["LABREPORTNO"].OptionsColumn.AllowEdit = false;
                lblSeller.Visible = true;
                txtSellerName.Visible = true;
            }

            else if (mFormType == FORMTYPE.LABRETURN)
            {
                GetSelectedProcessIssue("LAB RETURN");
                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Bands["BandLabService"].Visible = true;
                GrdDetail.Columns["LABSERVICECODE"].OptionsColumn.AllowEdit = false;

                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;
                CmbMemoType.Items.Add("INTERNAL");
                CmbMemoType.Items.Add("EXTERNAL");

                BtnReturn.Enabled = false;
                BtnOtherActivity.Visible = false;

                //Add : Pinali : 31-08-2019
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["LABREPORTNO"].OptionsColumn.AllowEdit = false;
                lblSeller.Visible = true;
                txtSellerName.Visible = true;
            }
            else if (mFormType == FORMTYPE.CONSIGNMENTISSUE)
            {
                GetSelectedProcessIssue("CONSIGNMENT ISSUE");
                GrdDetail.Bands["BandLabService"].Visible = false;
                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;

                cmbBillType.SelectedItem = "Consignment";
                CmbMemoType.Items.Add("INTERNAL");
                CmbMemoType.Items.Add("EXTERNAL");

                if (lblMode.Text == "Add Mode")
                {
                    BtnReturn.Enabled = false;
                    BtnOtherActivity.Visible = false;
                }
                else
                {
                    BtnReturn.Enabled = true;
                    GetSelectedProcessReturn("CONSIGNMENT RETURN");

                    BtnOtherActivity.Visible = true;
                    barButtonItem12.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                    barButtonItem11.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
                }
                GrdDetail.Columns["LABREPORTNO"].OptionsColumn.AllowEdit = false;
                lblSeller.Visible = true;
                txtSellerName.Visible = true;
            }

            else if (mFormType == FORMTYPE.CONSIGNMENTRETURN)
            {
                GrdDetail.Bands["BandLabService"].Visible = false;
                GetSelectedProcessIssue("CONSIGNMENT RETURN");
                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = false;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = false;
                CmbMemoType.Items.Add("INTERNAL");
                CmbMemoType.Items.Add("EXTERNAL");

                BtnReturn.Enabled = false;
                BtnOtherActivity.Visible = false;

                //Add : Pinali : 31-08-2019
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["LABREPORTNO"].OptionsColumn.AllowEdit = false;
                lblSeller.Visible = true;
                txtSellerName.Visible = true;
            }

            else if (mFormType == FORMTYPE.PURCHASEISSUE)
            {
                DtabPara = new BOMST_Parameter().GetParameterData();

                //BtnNewRow.Visible = true;
                BtnNewRow.Visible = true;

                GetSelectedProcessIssue("PURCHASE");
                GrdDetail.Bands["BandLabService"].Visible = false;
                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = mStrStockType == "SINGLE" ? true : false;

                GrdDetail.Columns["SHAPENAME"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["COLORNAME"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["CLARITYNAME"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["CUTNAME"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["POLNAME"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["SYMNAME"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["FLNAME"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["LABNAME"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["ISPURCHASE"].Visible = false;
                GrdDetail.Columns["LABREPORTNO"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["SIZENAME"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["LOCATIONNAME"].OptionsColumn.AllowEdit = true;

                GrdDetail.Columns["LENGTH"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["WIDTH"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["HEIGHT"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["TABLEPER"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["DEPTHPER"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["CRANGLE"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["CRHEIGHT"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["PAVANGLE"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["PAVHEIGHT"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["GIRDLEPER"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["GIRDLEDESC"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["KEYTOSYMBOL"].OptionsColumn.AllowEdit = true;

                GrdDetail.Columns["SALERAPAPORT"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["SALEDISCOUNT"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["SALEPRICEPERCARAT"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["SALEAMOUNT"].OptionsColumn.AllowEdit = false;

                GrdDetail.Columns["MEMORAPAPORT"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["MEMOAMOUNT"].OptionsColumn.AllowEdit = false;

                GrdDetail.Columns["BALANCEPCS"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["BALANCECARAT"].OptionsColumn.AllowEdit = true;

                GrdDetail.Columns["BALANCEPCS"].Visible = false;
                GrdDetail.Columns["BALANCECARAT"].Visible = true;

                GrdDetail.Columns["PCS"].Visible = false;
                GrdDetail.Columns["CARAT"].Visible = false;

                GrdDetail.Columns["STATUS"].Visible = false;

                GrdDetail.Columns["SALERAPAPORT"].Caption = "Cost Rapa($)";
                GrdDetail.Columns["SALEDISCOUNT"].Caption = "Cost Disc($)";
                GrdDetail.Columns["SALEPRICEPERCARAT"].Caption = "Cost $/Cts($)";
                GrdDetail.Columns["SALEAMOUNT"].Caption = "Cost Amt($)";

                GrdDetail.Columns["MEMORAPAPORT"].Caption = "Sale Rapa($)";
                GrdDetail.Columns["MEMODISCOUNT"].Caption = "Sale Disc($)";
                GrdDetail.Columns["MEMOPRICEPERCARAT"].Caption = "Sale $/Cts($)";
                GrdDetail.Columns["MEMOAMOUNT"].Caption = "Sale Amt($)";

                GrdDetail.Columns["BALANCEPCS"].Caption = "Pcs";
                GrdDetail.Columns["BALANCECARAT"].Caption = "Carat";

                //HINA - START
                //GrdDetail.Columns["STOCKTYPE"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["STOCKTYPE"].OptionsColumn.AllowEdit = mStrStockType == "ALL" ? true : false;
                //HINA - END
                GrdDetail.Columns["STOCKNO"].OptionsColumn.AllowEdit = false;

                //GrdDetail.Columns["PARTYSTOCKNO"].OptionsColumn.AllowEdit = true;

                GrdDetail.Columns["MEMORAPAPORT"].OptionsColumn.AllowEdit = true;
                //HINA - START
                //GrdDetail.Columns["STOCKTYPE"].OptionsColumn.AllowEdit = true;
                GrdDetail.Columns["STOCKTYPE"].OptionsColumn.AllowEdit = mStrStockType == "ALL" ? true : false;
                //HINA - END

                lblSeller.Visible = false;
                txtSellerName.Visible = false;


                CmbMemoType.Items.Add("LOCAL");
                CmbMemoType.Items.Add("IMPORT");

                if (lblMode.Text == "Add Mode")
                {
                    BtnReturn.Enabled = false;
                    BtnOtherActivity.Enabled = false;
                }
                else
                {
                    BtnReturn.Enabled = true;
                    GetSelectedProcessReturn("PURCHASE RETURN");
                }
                BtnOtherActivity.Visible = false;
            }
            else if (mFormType == FORMTYPE.PURCHASERETURN)
            {
                GetSelectedProcessIssue("PURCHASE RETURN");
                GrdDetail.Bands["BandLabService"].Visible = false;
                GrdDetail.Bands["BANDEXTRAPARAM"].Visible = true;
                GrdDetail.Columns["ISPURCHASE"].Visible = false;
                CmbMemoType.Items.Add("LOCAL");
                CmbMemoType.Items.Add("IMPORT");

                BtnReturn.Enabled = false;
                BtnOtherActivity.Visible = false;

                //Add : Pinali : 31-08-2019
                GrdDetail.Columns["MEMODISCOUNT"].OptionsColumn.AllowEdit = false;
                GrdDetail.Columns["MEMOPRICEPERCARAT"].OptionsColumn.AllowEdit = false;

                GrdDetail.Columns["LABREPORTNO"].OptionsColumn.AllowEdit = false;

                lblSeller.Visible = false;
                txtSellerName.Visible = false;
            }

            //#P : 20-02-2020
            if (Val.ToString(mStrStockType).ToUpper() == "PARCEL")
            {
                BtnNewRow.Visible = false;
            }
            else
            {
                if (mFormType == FORMTYPE.PURCHASERETURN || mFormType == FORMTYPE.PURCHASEISSUE)
                    BtnNewRow.Visible = false;
                else
                    BtnNewRow.Visible = true;
            }
            //End : #P : 20-02-2020

            CmbMemoType.SelectedIndex = 0;
        }

        public void EInvoiceUploadGenerateIRN()
        {
            AuthToken = GetToken(ref key);
            if (AuthToken == "")
            {
                StrMessage = "No Any Token Generated For Invoice Posing";
                IntIsError = 1;
                return;
            }
            else
            {
                DataSet dsJson = ObjEInvoice.GetEInvoiceInvoiceInfo(Val.ToString(lblMemoNo.Tag));

                if (dsJson.Tables.Count == 0)
                {
                    StrMessage = "No Data Found For Upload";
                    IntIsError = 1;
                    return;
                }

                if (dsJson.Tables[0].Columns[0].ToString() == "Json")
                {
                    TRN_EInvoiceProperty Property = ObjEInvoice.GetEInvoiceCredential(BusLib.Configuration.BOConfiguration.COMPANY_ID);
                    string StrReqId = "SRDIRNREQID" + ObjEInvoice.GetMaxRequstId("EINVOICEREQUESTID");
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xC0 | 0x300 | 0xC00);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Property.URL + "/Invoice");
                    request.Method = "POST";
                    request.KeepAlive = true;
                    request.ProtocolVersion = HttpVersion.Version10;
                    request.ServicePoint.Expect100Continue = false;
                    request.AllowAutoRedirect = false;
                    request.Accept = "*/*";
                    request.UnsafeAuthenticatedConnectionSharing = true;
                    request.ContentType = "application/json";
                    request.Headers.Add("user_name", Property.USERNAME);
                    request.Headers.Add("password", Property.PASSWORD);
                    request.Headers.Add("Gstin", Property.GSTIN);
                    request.Headers.Add("requestid", StrReqId);
                    request.Headers.Add("Authorization", AuthToken);

                    byte[] _aeskey = GenerateSecureKey();
                    string straesKey = Convert.ToBase64String(_aeskey);

                    string JsonData = Val.ToString(dsJson.Tables[0].Rows[0]["Json"]);
                    JsonData = JsonData.Replace("\t", "");
                    var serializer = new JavaScriptSerializer();
                    using (var StreamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        StreamWriter.Write(JsonData);
                        StreamWriter.Flush();
                        StreamWriter.Close();
                    }

                    try
                    {
                        WebResponse response = request.GetResponse();
                        string Result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        Result = Result.Replace("result", "");
                        Result = Result.Replace(":{", "");
                        DataTable dtresult = ConvertJsonToDataTable(Result);

                        if (dtresult.Rows.Count > 0)
                        {
                            if (Val.ToString(dtresult.Rows[0]["success"]) == "false")
                            {
                                StrMessage = Val.ToString(dtresult.Rows[0]["message"]);
                                StrMessage = StrMessage.Replace("'", "");
                                IntIsError = 1;
                                ObjEInvoice.InsertEInvoiceReustIdMessage(StrReqId, StrMessage, Val.ToString(lblMemoNo.Tag));
                                return;
                            }
                            else
                            {
                                foreach (DataRow dr in dtresult.Rows)
                                {
                                    StrMessage = Val.ToString(dr["message"]);
                                    AckNo = Val.ToString(dr["AckNo"]);
                                    IrnNo = Val.ToString(dr["Irn"]);
                                    IrnDate = Val.ToString(dr["AckDt"]);
                                    SignedInvoice = Val.ToString(dr["SignedInvoice"]);
                                    SignedQRCode = Val.ToString(dr["SignedQRCode"]);
                                }
                                ObjEInvoice.InsertEInvoiceReustIdMessage(StrReqId, Val.ToString(dtresult.Rows[0]["message"]), Val.ToString(lblMemoNo.Tag));
                                ObjEInvoice.UpdateEInvoiceUploadDate(IrnDate, Val.ToString(lblMemoNo.Tag));
                                int RetValue = ObjEInvoice.InsertEInvoiceDetail(Val.ToString(lblMemoNo.Tag), AckNo, IrnNo, Val.SqlDate(IrnDate), SignedInvoice, SignedQRCode, null, Property);
                                if (RetValue == -1)
                                {
                                    StrMessage = "Record Not Inserted";
                                    IntIsError = 1;
                                    return;
                                }

                                StrMessage = "E-Invoice Upload Successfully";
                                IntIsError = 0;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        StrMessage = ex.Message.ToString();
                        IntIsError = 1;
                        return;
                    }
                }
                else
                {
                    StrMessage = Val.ToString(dsJson.Tables[0].Rows[0][0]);
                    IntIsError = 1;
                }
            }
        }

        public void EInvoiceCancel()
        {
            AuthToken = GetToken(ref key);
            if (AuthToken == "")
            {
                StrMessage = "No Any Token Generated For Invoice Posing";
                IntIsError = 1;
                return;
            }
            else
            {
                DataSet dsJson = ObjEInvoice.GetEInvoiceCancelInvoiceInfo(Val.ToString(lblMemoNo.Tag));

                if (dsJson.Tables.Count == 0)
                {
                    StrMessage = "No Data Found For Upload";
                    IntIsError = 1;
                    return;
                }

                if (dsJson.Tables[0].Columns[0].ToString() == "Json")
                {
                    TRN_EInvoiceProperty Property = ObjEInvoice.GetEInvoiceCredential(BusLib.Configuration.BOConfiguration.COMPANY_ID);
                    string StrReqId = "SRDCNCLREQID" + ObjEInvoice.GetMaxRequstId("EINVOICEREQUESTID");
                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xC0 | 0x300 | 0xC00);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Property.URL + "/Invoice/cancel");
                    request.Method = "POST";
                    request.KeepAlive = true;
                    request.ProtocolVersion = HttpVersion.Version10;
                    request.ServicePoint.Expect100Continue = false;
                    request.AllowAutoRedirect = false;
                    request.Accept = "*/*";
                    request.UnsafeAuthenticatedConnectionSharing = true;
                    request.ContentType = "application/json";
                    request.Headers.Add("user_name", Property.USERNAME);
                    request.Headers.Add("password", Property.PASSWORD);
                    request.Headers.Add("Gstin", Property.GSTIN);
                    request.Headers.Add("requestid", StrReqId);
                    request.Headers.Add("Authorization", AuthToken);

                    byte[] _aeskey = GenerateSecureKey();
                    string straesKey = Convert.ToBase64String(_aeskey);

                    string JsonData = Val.ToString(dsJson.Tables[0].Rows[0]["Json"]);
                    JsonData = JsonData.Replace("\t", "");
                    var serializer = new JavaScriptSerializer();
                    using (var StreamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        StreamWriter.Write(JsonData);
                        StreamWriter.Flush();
                        StreamWriter.Close();
                    }

                    try
                    {
                        WebResponse response = request.GetResponse();
                        string Result = new StreamReader(response.GetResponseStream()).ReadToEnd();

                        Result = Result.Replace("result", "");
                        Result = Result.Replace(":{", "");
                        DataTable dtresult = ConvertJsonToDataTable(Result);

                        if (dtresult.Rows.Count > 0)
                        {
                            if (Val.ToString(dtresult.Rows[0]["success"]) == "false")
                            {
                                StrMessage = Val.ToString(dtresult.Rows[0]["message"]);
                                StrMessage = StrMessage.Replace("'", "");
                                IntIsError = 1;
                                ObjEInvoice.InsertEInvoiceReustIdMessage(StrReqId, StrMessage, Val.ToString(lblMemoNo.Tag));
                                IntIsError = 1;
                                return;
                            }
                            else
                            {
                                foreach (DataRow dr in dtresult.Rows)
                                {
                                    StrMessage = Val.ToString(dr["message"]);
                                    IrnNo = Val.ToString(dr["Irn"]);
                                    StrCancelDate = Val.ToString(dr["CancelDate"]);
                                }
                                ObjEInvoice.InsertEInvoiceReustIdMessage(StrReqId, Val.ToString(dtresult.Rows[0]["message"]), Val.ToString(lblMemoNo.Tag));
                                ObjEInvoice.UpdateEInvoiceCancelDate(StrCancelDate, Val.ToString(lblMemoNo.Tag));
                                int RetValue = ObjEInvoice.InsertEInvoiceDetail(Val.ToString(lblMemoNo.Tag), AckNo, IrnNo, Val.SqlDate(IrnDate), SignedInvoice, SignedQRCode, StrCancelDate, Property);
                                if (RetValue == -1)
                                {
                                    StrMessage = "Record Not Inserted";
                                    IntIsError = 1;
                                    return;
                                }

                                StrMessage = "E-Invoice Cancled Successfully";
                                IntIsError = 0;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        StrMessage = ex.Message.ToString();
                        IntIsError = 1;
                        return;
                    }
                }
                else
                {
                    StrMessage = Val.ToString(dsJson.Tables[0].Rows[0][0]);
                    IntIsError = 1;
                }

            }
        }

        public void EInvoicePrint()
        {
            DataRow DRow = ObjEInvoice.GetEInvoiceExists(Val.ToString(lblMemoNo.Tag));
            if (DRow == null)
            {
                StrMessage = "First Generate IRN Number For E-Invoice , Upload Your Invoice";
                IntIsError = 1;
                return;
            }

            AuthToken = GetToken(ref key);
            if (AuthToken == "")
            {
                StrMessage = "No Any Token Generated For Invoice Posing";
                IntIsError = 1;
                return;
            }
            else
            {
                {
                    string public_key = "";

                    TRN_EInvoiceProperty Property = ObjEInvoice.GetEInvoiceCredential(BusLib.Configuration.BOConfiguration.COMPANY_ID);
                    using (var reader = File.OpenText(Application.StartupPath + @"\\einv_sandbox.pem"))
                    {
                        public_key = reader.ReadToEnd().Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace(Constants.vbLf, "");
                    }

                    ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xC0 | 0x300 | 0xC00);
                    string StrReqId = "SRDPRNTREQID" + ObjEInvoice.GetMaxRequstId("EINVOICEREQUESTID");
                    string StrUrl = Property.URL + "/Invoice/irn?irn=" + Val.ToString(DRow["IRNNO"]);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(StrUrl);
                    request.Method = "GET";
                    request.KeepAlive = true;
                    request.ProtocolVersion = HttpVersion.Version10;
                    request.ServicePoint.Expect100Continue = false;
                    request.AllowAutoRedirect = false;
                    request.Accept = "*/*";
                    request.UnsafeAuthenticatedConnectionSharing = true;
                    request.ContentType = "application/json";
                    request.Headers.Add("user_name", Property.USERNAME);
                    request.Headers.Add("password", Property.PASSWORD);
                    request.Headers.Add("Gstin", Property.GSTIN);
                    request.Headers.Add("requestid", StrReqId);
                    request.Headers.Add("Authorization", AuthToken);

                    try
                    {
                        WebResponse response = request.GetResponse();
                        string Result = new StreamReader(response.GetResponseStream()).ReadToEnd();
                        Result = Result.Replace("result", "");
                        Result = Result.Replace(":{", "");
                        DataTable dtresult = ConvertJsonToDataTable(Result);
                        if (dtresult.Rows.Count > 0)
                        {
                            if (Val.ToString(dtresult.Rows[0]["success"]) == "false")
                            {
                                StrMessage = Val.ToString(dtresult.Rows[0]["message"]);
                                StrMessage = StrMessage.Replace("'", "");
                                IntIsError = 1;
                                ObjEInvoice.InsertEInvoiceReustIdMessage(StrReqId, StrMessage, Val.ToString(lblMemoNo.Tag));
                                IntIsError = 1;
                                return;
                            }
                            else
                            {
                                foreach (DataRow dr in dtresult.Rows)
                                {
                                    AckNo = Val.ToString(dr["AckNo"]);
                                    IrnNo = Val.ToString(dr["Irn"]);
                                    IrnDate = Val.ToString(dr["AckDt"]);
                                    SignedInvoice = Val.ToString(dr["SignedInvoice"]);
                                    SignedQRCode = Val.ToString(dr["SignedQRCode"]);
                                }
                                ObjEInvoice.InsertEInvoiceReustIdMessage(StrReqId, Val.ToString(dtresult.Rows[0]["message"]), Val.ToString(lblMemoNo.Tag));
                                string result2 = "";
                                string resultFinal = "";
                                result2 = Decode(SignedInvoice);
                                resultFinal = DecodePayload(SignedInvoice);
                                JavaScriptSerializer serializer = new JavaScriptSerializer();
                                Acct_EinvoiceApiRequest Einvre = serializer.Deserialize<Acct_EinvoiceApiRequest>(resultFinal);
                                string Data = Einvre.Data;
                                var dsRDS = new DataSet();
                                var dtFinal = new DataTable();
                                XmlDocument doc = (XmlDocument)JsonConvert.DeserializeXmlNode(Data, "friends");
                                string str1 = doc.InnerXml.Replace("<TranDtls>", "").Replace("</TranDtls>", "").Replace("<DocDtls>", "").Replace("</DocDtls>", "").Replace("<SellerDtls>", "").Replace("</SellerDtls>", "").Replace("<BuyerDtls>", "").Replace("</BuyerDtls>", "").Replace("<DispDtls>", "").Replace("</DispDtls>", "").Replace("<ItemList>", "").Replace("</ItemList>", "").Replace("<ValDtls>", "").Replace("</ValDtls>", "");
                                ObjEInvoice.InsertEInvoicePrintOutPutXML(Val.ToString(lblMemoNo.Tag), str1);

                                doc.LoadXml(str1);
                                if (doc.InnerXml.Trim().Length > 0)
                                {
                                    using (var stringReader = new StringReader(doc.InnerXml))
                                    {
                                        dsRDS = new DataSet();
                                        dsRDS.ReadXml(stringReader, XmlReadMode.Auto);
                                    }
                                }

                                dtFinal = dsRDS.Tables[0];
                                var newColumn = new System.Data.DataColumn("SignedQRCode", typeof(byte[]));
                                dtFinal.Columns.Add(newColumn);
                                var newColumnBr = new System.Data.DataColumn("Barcode", typeof(byte[]));
                                dtFinal.Columns.Add(newColumnBr);
                                var newColumnBuyer = new System.Data.DataColumn("BuyerAdd", typeof(string));
                                dtFinal.Columns.Add(newColumnBuyer);
                                var newColumnBuyerGST = new System.Data.DataColumn("BuyerGST", typeof(string));
                                dtFinal.Columns.Add(newColumnBuyerGST);
                                var newColumnSeller = new System.Data.DataColumn("SellerAdd", typeof(string));
                                dtFinal.Columns.Add(newColumnSeller);
                                var newColumnSellerGST = new System.Data.DataColumn("SellerGST", typeof(string));
                                dtFinal.Columns.Add(newColumnSellerGST);
                                var newColumnTaxRateStr = new System.Data.DataColumn("TaxRateStr", typeof(string));
                                dtFinal.Columns.Add(newColumnTaxRateStr);

                                var Pic = new PictureBox();
                                Image Img;
                                var qr = new QRCodeGenerator();
                                QRCodeData Qrdata = qr.CreateQrCode(SignedQRCode, QRCodeGenerator.ECCLevel.Q);
                                var code = new QRCoder.QRCode(Qrdata);
                                Pic.Image = code.GetGraphic(2);
                                Img = Pic.Image;
                                var barcode = new Linear();
                                barcode.Type = BarcodeType.CODE128;
                                barcode.Data = AckNo;

                                DataSet dsJson = ObjEInvoice.GetEInvoiceInvoiceInfo(Val.ToString(lblMemoNo.Tag));

                                DataTable dtBuyer = dsJson.Tables[1];
                                DataTable dtSeller = dsJson.Tables[2];
                                string Str = "";
                                foreach (DataRow dr in dtFinal.Rows)
                                {
                                    var ms = new MemoryStream();
                                    Img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

                                    dr["SignedQRCode"] = ms.ToArray();
                                    dr["Barcode"] = barcode.drawBarcodeAsBytes();
                                    dr["BuyerAdd"] = Val.ToString(dtBuyer.Rows[0]["BuyerAdd"]);
                                    dr["BuyerGST"] = Val.ToString(dtBuyer.Rows[0]["BuyerGST"]);
                                    dr["SellerAdd"] = Val.ToString(dtSeller.Rows[0]["SellerAdd"]);
                                    dr["SellerGST"] = Val.ToString(dtSeller.Rows[0]["SellerGST"]);
                                    //dr["CesRt"] = Val.Val(dr["CesRt"]) == 0 ? 0.00 : Val.Val(dr["CesRt"]);

                                    Str = (Val.Val(dr["GstRt"]) == 0 ? "0.00" : Val.ToString(dr["GstRt"])) + "+" + (Val.Val(dr["CesRt"]) == 0 ? "0.00" : Val.ToString(dr["CesRt"])) + "|"

                                          + (Val.Val(dr["StateCesRt"]) == 0 ? "0.00" : Val.ToString(dr["StateCesRt"]))
                                          + "+"
                                          + (Val.Val(dr["StateCesNonAdvlAmt"]) == 0 ? "0.00" : Val.ToString(dr["StateCesNonAdvlAmt"]))
                                          ;

                                    dr["TaxRateStr"] = Val.ToString(Str);

                                }
                                this.BeginInvoke(new MethodInvoker(delegate
                                {
                                    Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                                    FrmReportViewer.MdiParent = Global.gMainRef;
                                    FrmReportViewer.ShowForm("rptEInvoice", dtFinal);
                                }));
                            }
                        }
                        IntIsError = -1;
                    }
                    catch (Exception ex)
                    {
                        Global.MessageError(ex.Message.ToString());
                        return;
                    }
                }
            }
        }

        #region process Bar

        BackgroundWorker bgw = new BackgroundWorker();
        void bgw_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Val.ToInt32(e.Argument) == 1)
            {
                EInvoiceUploadGenerateIRN();
            }
            else if (Val.ToInt32(e.Argument) == 2)
            {
                EInvoiceCancel();
            }
            else if (Val.ToInt32(e.Argument) == 3)
            {
                EInvoicePrint();
            }
        }

        void bgw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            PnlLoading.Visible = false;
            if (IntIsError == 0)
            {
                Global.Message(StrMessage);
            }
            else if (IntIsError == 1)
            {
                Global.MessageError(StrMessage);
            }
        }

        #endregion


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

        private void BtnExport_Click(object sender, EventArgs e)
        {
            Global.ExcelExport("Current Stock List", GrdDetail);
        }

        private void BtnBestFit_Click(object sender, EventArgs e)
        {
            GrdDetail.BestFitColumns();
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            try
            {
                lblMode.Text = "Add Mode";
                lblMemoNo.Text = string.Empty;
                lblMemoNo.Tag = string.Empty;
                txtJangedNo.Text = string.Empty;
                DTPMemoDate.Text = Val.ToString(DateTime.Now);

                CmbDeliveryType.SelectedIndex = 0;
                CmbPaymentMode.SelectedIndex = 0;
                cmbBillType.SelectedIndex = 0;
                cmbInsuranceType.SelectedIndex = 0;

                txtGrossWeight.Text = string.Empty;

                txtSellerName.Text = BOConfiguration.gEmployeeProperty.LEDGERNAME;
                txtSellerName.Tag = BOConfiguration.gEmployeeProperty.LEDGER_ID;

                lblSource.Text = "SOFTWARE";

                txtBillingParty.Text = string.Empty;
                txtBillingParty.Tag = string.Empty;
                txtBAddress1.Text = string.Empty;
                txtBAddress2.Text = string.Empty;
                txtBAddress3.Text = string.Empty;
                txtBCity.Text = string.Empty;
                txtBCountry.Text = string.Empty;
                txtBState.Text = string.Empty;
                txtBZipCode.Text = string.Empty;
                txtJangedNo.Text = string.Empty;

                txtShippingParty.Text = string.Empty;
                txtShippingParty.Tag = string.Empty;
                txtSAddress1.Text = string.Empty;
                txtSAddress2.Text = string.Empty;
                txtSAddress3.Text = string.Empty;
                txtSCity.Text = string.Empty;
                txtSCountry.Text = string.Empty;
                txtSState.Text = string.Empty;
                txtSZipCode.Text = string.Empty;

                txtBroker.Text = string.Empty;
                txtBaseBrokeragePer.Text = string.Empty;
                txtProfitBrokeragePer.Text = string.Empty;
                txtAdat.Text = string.Empty;
                txtAdat.Tag = string.Empty;

                txtCurrency.Text = string.Empty;
                txtCurrency.Tag = string.Empty;

                txtExcRate.Text = string.Empty;
                txtOrgExcRate.Text = string.Empty;
                txtAddLessExcRate.Text = string.Empty;
                txtTerms.Text = string.Empty;
                txtTermsDays.Text = string.Empty;
                DTPTermsDate.Text = Val.ToString(DateTime.Now);

                txtMemoAvgDisc.Text = string.Empty;
                txtMemoAvgRate.Text = string.Empty;
                txtMemoAmount.Text = string.Empty;
                txtRemark.Text = string.Empty;
                txtTransport.Text = string.Empty;
                txtPlaceOfSupply.Text = string.Empty;

                txtGrossAmount.Text = string.Empty;
                txtDiscPer.Text = string.Empty;
                txtDiscAmount.Text = string.Empty;
                txtInsurancePer.Text = string.Empty;
                txtInsuranceAmount.Text = string.Empty;
                txtShippingPer.Text = string.Empty;
                txtShippingAmount.Text = string.Empty;
                txtGSTPer.Text = string.Empty;
                txtGSTAmount.Text = string.Empty;
                txtNetAmount.Text = string.Empty;

                txtIGSTPer.Text = string.Empty;
                txtIGSTAmount.Text = string.Empty;
                txtCGSTPer.Text = string.Empty;
                txtCGSTAmount.Text = string.Empty;
                txtSGSTPer.Text = string.Empty;
                txtSGSTAmount.Text = string.Empty;

                //#K : 15122020
                txtTCSPer.Text = string.Empty;
                txtTCSAmount.Text = string.Empty;
                txtTCSAmountFE.Text = string.Empty;
                txtRoundOffPer.Text = string.Empty;
                txtRoundOffAmount.Text = string.Empty;
                txtRoundOffAmountFE.Text = string.Empty;
                CmbRoundOff.SelectedValue = string.Empty;

                txtconsignee.Text = string.Empty;
                //cmbAddresstype.SelectedIndex = 0;
                cmbAddresstype.SelectedIndex = 1; //#P : 14-07-2021

                txtLabServiceCode.Text = string.Empty;
                txtLabServiceCode.Tag = string.Empty;

                txtNarration.Text = string.Empty;
                txtNarration.Tag = string.Empty;

                DTPMemoDate.Focus();
                DTabMemoDetail.Rows.Clear();
                MainGrdDetail.Refresh();

                txtBackPriceFileName.Text = string.Empty;
                txtBackAddLess.Text = string.Empty;
                txtTermsAddLessPer.Text = string.Empty;
                txtBlindAddLessPer.Text = string.Empty;

                txtBuyer.Text = string.Empty;
                txtBuyer.Tag = string.Empty;
                chkIsConsingee.Checked = false;

                //Calculation();
                CalculationNew();
                ChkOrderConfirmPickup.Checked = false;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnContinue_Click(object sender, EventArgs e)
        {
            try
            {
                if (mFormType == FORMTYPE.PURCHASEISSUE)
                {
                    DataRow drow = DTabMemoDetail.NewRow();
                    //HINA - START
                    //drow["STOCKTYPE"] = "SINGLE";
                    drow["STOCKTYPE"] = mStrStockType == "PARCEL" ? "PARCEL" : "SINGLE";
                    //HINA - END
                    DTabMemoDetail.Rows.Add(drow);

                    GrdDetail.FocusedRowHandle = drow.Table.Rows.IndexOf(drow);
                    GrdDetail.FocusedColumn = GrdDetail.VisibleColumns[1];
                    GrdDetail.Focus();
                    GrdDetail.ShowEditor();
                }
                //else
                //{
                //    //DataRow drow = DtabFinalInvDetail.NewRow();
                //    //DtabFinalInvDetail.Rows.Add(drow);
                //    //GrdDetail.FocusedRowHandle = drow.Table.Rows.IndexOf(drow);
                //    //GrdDetail.FocusedColumn = GrdDetail.VisibleColumns[0];
                //    GrdDetail.Focus();
                //    GrdDetail.ShowEditor();
                //}
                else //Changed : Pinali : 27-08-2019
                {
                    DataRow drow = DTabMemoDetail.NewRow();
                    DTabMemoDetail.Rows.Add(drow);
                    GrdDetail.FocusedRowHandle = drow.Table.Rows.IndexOf(drow);
                    GrdDetail.FocusedColumn = GrdDetail.VisibleColumns[0];
                    GrdDetail.Focus();
                    GrdDetail.ShowEditor();
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtSellerName_KeyPress(object sender, KeyPressEventArgs e)
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
                        txtSellerName.Text = Val.ToString(FrmSearch.DRow["EMPLOYEENAME"]);
                        txtSellerName.Tag = Val.ToString(FrmSearch.DRow["EMPLOYEE_ID"]);
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

        private void txtBillingParty_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;

                    //FrmSearch.DTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.TABLE.MST_SALEPARTY);

                    DataTable DtabParty = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);

                    if (mFormType == FORMTYPE.PURCHASEISSUE || mFormType == FORMTYPE.PURCHASERETURN)
                        FrmSearch.mDTab = DtabParty.Select("PARTYTYPE = 'PURCHASE'").CopyToDataTable();
                    else if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.SALESDELIVERYRETURN || mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.ORDERCONFIRMRETURN)
                        FrmSearch.mDTab = DtabParty.Select("PARTYTYPE = 'SALE'").CopyToDataTable();
                    else if (mFormType == FORMTYPE.MEMOISSUE || mFormType == FORMTYPE.MEMORETURN)
                        FrmSearch.mDTab = DtabParty.Select("PARTYTYPE IN ('PURCHASE','SALE','EMPLOYEE')").CopyToDataTable();
                    else
                        FrmSearch.mDTab = DtabParty;
                    FrmSearch.mBoolISPostBack = true;
                    FrmSearch.mStrISPostBackColumn = "PARTYNAME";
                    FrmSearch.mStrColumnsToHide = "PARTY_ID,BILLINGCOUNTRY_ID,SHIPPINGCOUNTRY_ID,PARTYTYPE,COMPANYNAME";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBillingParty.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtBillingParty.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
                        txtBAddress1.Text = Val.ToString(FrmSearch.DRow["BILLINGADDRESS1"]);
                        txtBAddress2.Text = Val.ToString(FrmSearch.DRow["BILLINGADDRESS2"]);
                        txtBAddress3.Text = Val.ToString(FrmSearch.DRow["BILLINGADDRESS3"]);
                        txtBCity.Text = Val.ToString(FrmSearch.DRow["BILLINGCITY"]);
                        txtBCountry.Tag = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRY_ID"]);
                        txtBCountry.Text = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRYNAME"]);
                        txtBState.Text = Val.ToString(FrmSearch.DRow["BILLINGSTATE"]);
                        txtBZipCode.Text = Val.ToString(FrmSearch.DRow["BILLINGZIPCODE"]);

                        txtBroker.Tag = Val.ToString(FrmSearch.DRow["COORDINATOR_ID"]);
                        txtBroker.Text = Val.ToString(FrmSearch.DRow["COORDINATORNAME"]);

                        txtFinalDestination.Text = Val.ToString(FrmSearch.DRow["BILLINGCITY"]);
                        txtCountryOfFinalDestination.Text = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRYNAME"]);

                        if (txtBroker.Text.Trim().Equals(string.Empty))
                        {
                            txtBaseBrokeragePer.Text = string.Empty;
                        }
                        else
                        {
                            txtBaseBrokeragePer.Text = "0.50";
                        }

                        if (Val.ToString(txtBillingParty.Tag) == "")
                        {
                            txtBillingParty.Tag = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                        }

                        CalculationNew();
                        txtBState_Validated(null, null);
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

        private void txtShippingParty_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_BRANCHCOMPANY);

                    FrmSearch.mStrColumnsToHide = "PARTY_ID,BILLINGCOUNTRY_ID,SHIPPINGCOUNTRY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtShippingParty.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtShippingParty.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);

                        txtSAddress1.Text = Val.ToString(FrmSearch.DRow["SHIPPINGADDRESS1"]);
                        txtSAddress2.Text = Val.ToString(FrmSearch.DRow["SHIPPINGADDRESS2"]);
                        txtSAddress3.Text = Val.ToString(FrmSearch.DRow["SHIPPINGADDRESS3"]);
                        txtSCity.Text = Val.ToString(FrmSearch.DRow["SHIPPINGCITY"]);
                        txtSCountry.Text = Val.ToString(FrmSearch.DRow["SHIPPINGCOUNTRYNAME"]);
                        txtSCountry.Tag = Val.ToString(FrmSearch.DRow["SHIPPINGCOUNTRY_ID"]);
                        txtSState.Text = Val.ToString(FrmSearch.DRow["SHIPPINGSTATE"]);
                        txtSZipCode.Text = Val.ToString(FrmSearch.DRow["SHIPPINGZIPCODE"]);
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

        private bool ValSave()
        {
            int IntCol = -1, IntRow = -1;
            foreach (DataRow dr in DTabMemoDetail.Rows)
            {
                //For Update Validation
                if (Val.ToString(dr["PARTYSTOCKNO"]).Trim().Equals(string.Empty) && !Val.ToString(dr["STOCK_ID"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter 'Cliennt Ref. No'");
                    IntCol = 1;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }
                //end as


                if (Val.ToString(dr["PARTYSTOCKNO"]).Trim().Equals(string.Empty))
                {
                    if (DTabMemoDetail.Rows.Count == 1)
                    {
                        Global.Message("Please Enter 'Client Ref. No'");
                        IntCol = 1;
                        IntRow = dr.Table.Rows.IndexOf(dr);
                        break;

                    }
                    else
                        continue;
                }

                if (Val.ToString(dr["PARTYSTOCKNO"]).Trim().Equals(string.Empty))
                {
                    Global.Message("Please Enter 'Client Ref. No'");
                    IntCol = 1;
                    IntRow = dr.Table.Rows.IndexOf(dr);
                    break;
                }

            }
            if (IntRow >= 0)
            {
                GrdDetail.FocusedRowHandle = IntRow;
                GrdDetail.FocusedColumn = GrdDetail.VisibleColumns[IntCol];
                GrdDetail.Focus();
                return true;
            }
            return false;
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValSave())
                {
                    return;
                }

                if (Val.Val(txtNetAmount.Text) <= 0 && mFormType != FORMTYPE.PURCHASERETURN && mFormType != FORMTYPE.SALESDELIVERYRETURN && mFormType != FORMTYPE.ORDERCONFIRMRETURN && mFormType != FORMTYPE.MEMORETURN
                   && mFormType != FORMTYPE.LABISSUE && mFormType != FORMTYPE.LABRETURN)
                {
                    Global.Message("Please Add Proper Stone Detail.");
                    return;
                }

                if (txtBillingParty.Text.Length == 0)
                {
                    Global.Message("Billing Party Is Required");
                    txtBillingParty.Focus();
                    return;
                }

                if (txtBCountry.Text.Length == 0)
                {
                    Global.Message("Billing Country Is Required");
                    txtBCountry.Focus();
                    return;
                }

                if (txtSellerName.Text.Length == 0)
                {
                    Global.Message("Seller Name Is Required");
                    txtSellerName.Focus();
                    return;
                }

                if (txtTerms.Text.Length == 0)
                {
                    Global.Message("Terms Is Required");
                    txtTerms.Focus();
                    return;
                }

                if (Val.ToString(txtCurrency.Text).Trim().Equals(string.Empty))
                {
                    Global.Message("Currency Is Required");
                    txtCurrency.Focus();
                    return;
                }

                if (Val.ToString(cmbBillType.SelectedItem) == "None" && (mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.SALEINVOICE))
                {
                    Global.Message("Please Select BillType");
                    cmbBillType.Focus();
                    return;
                }

                if (Val.ToString(CmbPaymentMode.SelectedItem) == "None" && (mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.SALEINVOICE))
                {
                    Global.Message("Please Select PaymentMode");
                    CmbPaymentMode.Focus();
                    return;
                }

                if (Val.Val(txtExcRate.Text) == 0)
                {
                    Global.Message("Please Enter ExcRate.");
                    txtExcRate.Focus();
                    return;
                }

                if (Val.ToString(txtCompanyBank.Text).Length == 0 && mFormType == FORMTYPE.SALEINVOICE)
                {
                    Global.Message("Please Enter CompanyBank");
                    txtCompanyBank.Focus();
                    xtraTabMasterPanel.SelectedTabPageIndex = 1;
                    return;
                }

                if (Val.ToString(cmbAddresstype.SelectedItem) == "NONE" && mFormType == FORMTYPE.SALEINVOICE)
                {
                    Global.Message("Please Select Company Address Type");
                    cmbAddresstype.Focus();
                    xtraTabMasterPanel.SelectedTabPageIndex = 1;
                    return;
                }

                // #D: 01-03-2021
                if (Val.Val(txtExcRate.Text) == 0 || Val.Val(txtExcRate.Text) == 1)
                {
                    Global.Message("Please Enter Proper ExcRate");
                    txtExcRate.Focus();
                    xtraTabMasterPanel.SelectedTabPageIndex = 0;
                    return;
                }
                // #D: 01-03-2021

                // D: 19/05/2021
                if (chkIsConsingee.Checked == true && Val.ToString(txtBuyer.Text).Length == 0 && (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRM))
                {
                    Global.Message("Please Enter Final Buyer Detail");
                    txtBuyer.Focus();
                    xtraTabMasterPanel.SelectedTabPageIndex = 0;
                    return;
                }
                // D: 19/05/2021

                if (txtShippingParty.Text.Length == 0)
                {
                    txtShippingParty.Text = string.Empty;
                    txtShippingParty.Tag = string.Empty;
                    txtSAddress1.Text = string.Empty;
                    txtSAddress2.Text = string.Empty;
                    txtSAddress3.Text = string.Empty;
                    txtSCity.Text = string.Empty;
                    txtSCountry.Tag = string.Empty;
                    txtSCountry.Text = string.Empty;
                    txtSState.Text = string.Empty;
                    txtSZipCode.Text = string.Empty;
                }

                if (mFormType == FORMTYPE.LABISSUE && txtLabServiceCode.Text.Length == 0)
                {
                    Global.Message("Please Select Lab Service Code");
                    txtLabServiceCode.Focus();
                    xtraTabMasterPanel.SelectedTabPageIndex = 1;
                    return;
                }


                if (Global.Confirm("Are You Sure For Goods Entry") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                MemoEntryProperty Property = new MemoEntryProperty();

                if (lblMode.Text == "Add Mode")
                {
                    lblMemoNo.Tag = BusLib.Configuration.BOConfiguration.FindNewSequentialID().ToString();
                }

                Property.MEMO_ID = Val.ToString(lblMemoNo.Tag);


                if (lblMode.Text == "Edit Mode")
                {
                    if (BOConfiguration.gStrLoginSection == "B")
                    {
                        Property.MEMO_ID = PstrApprove_MemoId;
                    }
                    else
                    {
                        Property.MEMO_ID = Val.ToString(lblMemoNo.Tag);
                    }

                    //DataTable DTab = ObjMemo.ValDelete(Property);
                    //if (DTab.Rows.Count != 0 && BOConfiguration.gStrLoginSection != "B")
                    //{
                    //    Global.Message("Some Stones Are In Other Process\n\n You Can Not Delete");
                    //    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    //    FrmSearch.mStrSearchField = "STOCKNO";
                    //    FrmSearch.mStrSearchText = "";
                    //    this.Cursor = Cursors.WaitCursor;
                    //    FrmSearch.mDTab = DTab;
                    //    FrmSearch.mStrColumnsToHide = "STOCK_ID";
                    //    this.Cursor = Cursors.Default;
                    //    FrmSearch.ShowDialog();
                    //    FrmSearch.Hide();
                    //    FrmSearch.Dispose();
                    //    FrmSearch = null;

                    //    DTab.Dispose();
                    //    DTab = null;

                    //    return;
                    //}
                    //else if (DTab.Rows.Count == 0 && BOConfiguration.gStrLoginSection == "B")
                    //{
                    //    Global.Message("A Part Entry Is In Other Process\n\n You Can Not Delete");
                    //    DTab.Dispose();
                    //    DTab = null;

                    //    return;
                    //}

                    //DTab.Dispose();
                    //DTab = null;
                }

                Property.JANGEDNOSTR = txtJangedNo.Text;
                Property.MEMONO = Val.ToInt64(lblMemoNo.Text);
                Property.MEMOTYPE = Val.ToString(CmbMemoType.SelectedItem);
                Property.MEMODATE = Val.SqlDate(DTPMemoDate.Text);

                if (Val.ToString(txtBillingParty.Tag) == "")
                {
                    txtBillingParty.Tag = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                }
                Property.BILLINGPARTY_ID = Val.ToString(txtBillingParty.Tag);
                Property.BILLINGPARTY_NAMEFORBRANCH = Val.ToString(txtBillingParty.Text);

                if (txtShippingParty.Text.Trim().Length != 0)
                {
                    Property.SHIPPINGPARTY_ID = Val.ToString(txtShippingParty.Tag);
                }
                else
                {
                    Property.SHIPPINGPARTY_ID = null;
                }

                // #D: 19-08-2020

                Property.CONSIGNEE = Val.ToString(txtconsignee.Text);
                Property.ADDRESSTYPE = Val.ToString(cmbAddresstype.SelectedItem);

                // #D : 19-08-2020

                if (txtBroker.Text.Trim().Length != 0)
                {
                    Property.BROKER_ID = Val.ToString(txtBroker.Tag);
                    Property.BROKERBASEPER = Val.Val(txtBaseBrokeragePer.Text);
                    Property.BROKERPROFITPER = Val.Val(txtProfitBrokeragePer.Text);
                    Property.BrokerAmountFE = Val.Val(txtBrokerAmtFE.Text);
                    Property.BrokerAmount = Val.Val(txtBrokerAmt.Text);
                }
                else
                {
                    Property.BROKER_ID = null;
                    Property.BROKERBASEPER = 0.00;
                    Property.BROKERPROFITPER = 0.00;
                    Property.BrokerAmountFE = 0.00;
                    Property.BrokerAmount = 0.00;
                }
                if (txtAdat.Text.Trim().Length != 0)
                {
                    Property.ADAT_ID = Val.ToString(txtAdat.Tag);
                    Property.ADATPER = Val.Val(txtAdatPer.Text);
                    Property.AdatAmtFE = Val.Val(txtAdatAmtFE.Text);
                    Property.AdatAmt = Val.Val(txtAdatAmt.Text);
                }
                else
                {
                    Property.ADAT_ID = null;
                    Property.ADATPER = 0.00;
                    Property.AdatAmtFE = 0.00;
                    Property.AdatAmt = 0.00;

                }

                Property.SELLER_ID = Val.ToString(txtSellerName.Tag);

                Property.TERMS_ID = Val.ToInt32(txtTerms.Tag);
                Property.TERMSDAYS = Val.ToInt32(txtTermsDays.Text);
                Property.TERMSPER = 0;
                Property.TERMSDATE = Val.SqlDate(DTPTermsDate.Text);

                Property.CURRENCY_ID = Val.ToInt32(txtCurrency.Tag);
                Property.EXCRATE = Val.Val(txtExcRate.Text);

                Property.MEMODISCOUNT = 0;

                Property.BILLINGADDRESS1 = Val.ToString(txtBAddress1.Text);
                Property.BILLINGADDRESS2 = Val.ToString(txtBAddress2.Text);
                Property.BILLINGADDRESS3 = Val.ToString(txtBAddress3.Text);
                Property.BILLINGCOUNTRY_ID = Val.ToInt32(txtBCountry.Tag);
                Property.BILLINGSTATE = Val.ToString(txtBState.Text);
                Property.BILLINGCITY = Val.ToString(txtBCity.Text);
                Property.BILLINGZIPCODE = Val.ToString(txtBZipCode.Text);

                Property.SHIPPINGADDRESS1 = Val.ToString(txtSAddress1.Text);
                Property.SHIPPINGADDRESS2 = Val.ToString(txtSAddress2.Text);
                Property.SHIPPINGADDRESS3 = Val.ToString(txtSAddress3.Text);
                Property.SHIPPINGCOUNTRY_ID = Val.ToInt32(txtSCountry.Tag);
                Property.SHIPPINGSTATE = Val.ToString(txtSState.Text);
                Property.SHIPPINGCITY = Val.ToString(txtSCity.Text);
                Property.SHIPPINGZIPCODE = Val.ToString(txtSZipCode.Text);

                Property.DELIVERYTYPE = Val.ToString(CmbDeliveryType.SelectedItem);
                Property.PAYMENTMODE = Val.ToString(CmbPaymentMode.SelectedItem);
                Property.BILLTYPE = Val.ToString(cmbBillType.SelectedItem);

                Property.TOTALPCS = Val.ToInt(lblTotalPcs.Text);
                Property.TOTALCARAT = Val.Val(lblTotalCarat.Text);
                Property.TOTALAVGDISC = Val.Val(txtMemoAvgDisc.Text);
                Property.TOTALAVGRATE = Val.Val(txtMemoAvgRate.Text);

                Property.GROSSAMOUNT = Val.Val(txtGrossAmount.Text);
                Property.DISCOUNTPER = Val.Val(txtDiscPer.Text);
                Property.DISCOUNTAMOUNT = Val.Val(txtDiscAmount.Text);
                Property.INSURANCEPER = Val.Val(txtInsurancePer.Text);
                Property.INSURANCEAMOUNT = Val.Val(txtInsuranceAmount.Text);
                Property.SHIPPINGPER = Val.Val(txtShippingPer.Text);
                Property.SHIPPINGAMOUNT = Val.Val(txtShippingAmount.Text);
                Property.GSTPER = Val.Val(txtGSTPer.Text);
                Property.GSTAMOUNT = Val.Val(txtGSTAmount.Text);

                //#P : 02-08-2020
                Property.IGSTPER = Val.Val(txtIGSTPer.Text);
                Property.IGSTAMOUNT = Val.Val(txtIGSTAmount.Text);
                Property.FIGSTAMOUNT = Val.Val(txtIGSTAmountFE.Text);
                Property.CGSTPER = Val.Val(txtCGSTPer.Text);
                Property.CGSTAMOUNT = Val.Val(txtCGSTAmount.Text);
                Property.FCGSTAMOUNT = Val.Val(txtCGSTAmountFE.Text);
                Property.SGSTPER = Val.Val(txtSGSTPer.Text);
                Property.SGSTAMOUNT = Val.Val(txtSGSTAmount.Text);
                Property.FSGSTAMOUNT = Val.Val(txtSGSTAmountFE.Text);
                //End : #P : 02-08-2020

                Property.NETAMOUNT = Val.Val(txtNetAmount.Text);

                Property.FGROSSAMOUNT = Val.Val(txtGrossAmountFE.Text);
                Property.FDISCOUNTAMOUNT = Val.Val(txtDiscAmountFE.Text);
                Property.FINSURANCEAMOUNT = Val.Val(txtInsuranceAmountFE.Text);
                Property.FSHIPPINGAMOUNT = Val.Val(txtShippingAmountFE.Text);
                Property.FGSTAMOUNT = Val.Val(txtGSTAmountFE.Text);
                Property.FNETAMOUNT = Val.Val(txtNetAmountFE.Text);

                Property.REMARK = Val.ToString(txtRemark.Text);
                Property.SOURCE = lblSource.Text;
                Property.PROCESS_ID = Val.ToInt32(lblTitle.Tag);
                Property.PROCESSNAME = Val.ToString(lblTitle.Text);

                Property.TRANSPORTNAME = txtTransport.Text;
                Property.PLACEOFSUPPLY = txtPlaceOfSupply.Text;

                Property.COMPANYBANK_ID = Val.ToString(txtCompanyBank.Text).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtCompanyBank.Tag));
                Property.PARTYBANK_ID = Val.ToString(txtPartyBank.Text).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtPartyBank.Tag));
                Property.COURIER_ID = Val.ToString(txtCourier.Text).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtCourier.Tag));
                Property.AIRFREIGHT_ID = Val.ToString(txtAirFreight.Text).Trim().Equals(string.Empty) ? Guid.Empty : Guid.Parse(Val.ToString(txtAirFreight.Tag));
                Property.PLACEOFRECEIPTBYPRECARRIER = Val.ToString(txtPlaceOfReceiptByPreCarrier.Text);
                Property.PORTOFLOADING = Val.ToString(txtPortOfLoading.Text);
                Property.PORTOFDISCHARGE = Val.ToString(txtPortOfDischarge.Text);
                Property.FINALDESTINATION = Val.ToString(txtFinalDestination.Text);
                Property.COUNTRYOFORIGIN = Val.ToString(txtCountryOfOrigin.Text);
                Property.COUNTRYOFFINALDESTINATION = Val.ToString(txtCountryOfFinalDestination.Text);

                Property.FINYEAR = Global.GetFinancialYear(DTPMemoDate.Text);
                Property.VOUCHERNOSTR = txtVoucherNoStr.Text;

                Property.GROSSWEIGHT = Val.Val(txtGrossWeight.Text);
                Property.INSURANCETYPE = Val.ToString(cmbInsuranceType.SelectedItem);

                // #D:13-01-2021
                Property.LABSERVICECODE_ID = Val.ToInt32(txtLabServiceCode.Tag);
                Property.LABSERVICECODE = Val.ToString(txtLabServiceCode.Text);
                // #d:13-01-2021

                Property.BACKADDLESS = Val.Val(txtBackAddLess.Text);
                Property.TERMSADDLESSPER = Val.Val(txtTermsAddLessPer.Text);
                Property.BLINDADDLESSPER = Val.Val(txtBlindAddLessPer.Text);

                Property.ExpInvoiceAmt = Val.Val(txtExpInvAmt.Text);
                Property.ExpInvoiceAmtFE = Val.Val(txtExpInvAmtFE.Text);
                Property.StkAmtFE = Val.Val(txtStkAmtFE.Text);

                Property.HKDRATE = Val.Val(txtHkdRate.Text);//GUNJAN:07/04/2023
                Property.HKDAMOUNT = Val.Val(txtHkdAmt.Text);

                Property.ORDERJANGEDNO = PstrOrderJangedNo;
                Property.BRNO = txtBrNo.Text;
                Property.ENTRYTYPE = "BRANCHRECEIVE";

                if (PstrOrder_MemoId.Length == 0)
                {
                    Property.ORDERMEMO_ID = "00000000-0000-0000-0000-000000000000";
                }
                else
                {
                    Property.ORDERMEMO_ID = PstrOrder_MemoId;
                }

                Property.BILLFORMAT = (RbDollar.Checked) ? RbDollar.Tag.ToString() : RbRupee.Tag.ToString();
                int IntI = 0;
                foreach (DataRow DRow in DTabMemoDetail.Rows)
                {
                    IntI++;
                    DRow["ENTRYSRNO"] = IntI;

                    if (mFormType == FORMTYPE.PURCHASEISSUE && Val.ToString(DRow["STOCK_ID"]) == "")
                    {
                        DRow["STOCK_ID"] = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                    }

                }
                DTabMemoDetail.AcceptChanges();

                DataTable DTabExpMemoDetail = new DataTable();
                DTabExpMemoDetail = DTabMemoDetail.Copy();

                foreach (DataRow DRow in DTabExpMemoDetail.Rows)
                {
                    DRow["MEMOPRICEPERCARAT"] = DRow["EXPINVOICERATE"];
                    DRow["MEMOAMOUNT"] = DRow["EXPINVOICEAMT"];
                    DRow["FMEMOPRICEPERCARAT"] = DRow["EXPINVOICERATEFE"];
                    DRow["FMEMOAMOUNT"] = DRow["EXPINVOICEAMTFE"];
                }

                string MemoEntryDetailForXML = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabMemoDetail.WriteXml(sw);
                    MemoEntryDetailForXML = sw.ToString();
                }

                string ReturnMessageDesc = "";
                string ReturnMessageType = "";
                //#K: 05122020
                Property.ORDERAPPROVAL = Val.ToBooleanToInt(ChkApprovedOrder.Checked);

                //#K : 15122020

                Property.ORG_EXCRATE = Val.Val(txtOrgExcRate.Text);
                Property.ADDLESS_EXCRATE = Val.Val(txtAddLessExcRate.Text);

                Property.TCSPER = Val.Val(txtTCSPer.Text);
                Property.TCSAMOUNT = Val.Val(txtTCSAmount.Text);
                Property.FTCSAMOUNT = Val.Val(txtTCSAmountFE.Text);

                Property.ROUNDOFFPER = Val.Val(txtRoundOffPer.Text);
                Property.ROUNDOFFAMOUNT = Val.Val(txtRoundOffAmount.Text);
                Property.FROUNDOFFAMOUNT = Val.Val(txtRoundOffAmountFE.Text);
                Property.ROUNDOFFTPYE = Val.ToString(CmbRoundOff.SelectedValue);

                Property.NARRATION_ID = Val.ToInt32(txtNarration.Tag);

                Property.ISCONSINGEE = chkIsConsingee.Checked;
                if (chkIsConsingee.Checked == true)
                {
                    Property.FINALBUYER_ID = Val.ToString(txtBuyer.Tag);
                    Property.FINALBUYERADDRESS1 = PstrFinalAddress1;
                    Property.FINALBUYERADDRESS2 = PstrFinalAddress2;
                    Property.FINALBUYERADDRESS3 = PstrFinalAddress3;
                    Property.FINALBUYERCOUNTRY_ID = Val.ToInt32(PstrFinalCountry_ID);
                    Property.FINALBUYERSTATE = PstrFinalSatet;
                    Property.FINALBUYERCITY = PstrFinalCity;
                    Property.FINALBUYERZIPCODE = PstrFinalZipCode;
                }
                else
                {
                    Property.FINALBUYER_ID = null;
                    Property.FINALBUYERADDRESS1 = string.Empty;
                    Property.FINALBUYERADDRESS2 = string.Empty;
                    Property.FINALBUYERADDRESS3 = string.Empty;
                    Property.FINALBUYERCOUNTRY_ID = 0;
                    Property.FINALBUYERSTATE = string.Empty;
                    Property.FINALBUYERCITY = string.Empty;
                    Property.FINALBUYERZIPCODE = string.Empty;
                }
                Property.INVOICENO = Val.ToString(txtInvoiceNo.Text);
                Property.ACCTYPE = Val.ToString(cmbAccType.SelectedItem);
                Property.Pickup = (ChkOrderConfirmPickup.Checked) ? "YES" : "NO";

                //using (TransactionScope transactionScope = new TransactionScope()) //Comment by Daksha on 23/02/2023 B'coz returns TransactionAborted Exception
                //{
                try
                {
                    SqlConnection cn_T;

                    cn_T = new SqlConnection(BOConfiguration.ConnectionString);
                    if (cn_T.State == ConnectionState.Open) { cn_T.Close(); }
                    cn_T.Open();

                    if (BOConfiguration.gStrLoginSection != "B")
                    {
                        Property = ObjMemo.SaveMemoEntryBranch(cn_T, Property, MemoEntryDetailForXML, lblMode.Text, "");
                        txtJangedNo.Text = Property.ReturnValueJanged;
                        lblMemoNo.Text = Property.ReturnValue;
                    }
                    else // export value same as sales value
                    {
                        string MemoEntryDetailExpForXML = string.Empty;
                        using (StringWriter sw = new StringWriter())
                        {
                            foreach (DataRow DR in DTabExpMemoDetail.Rows)
                            {
                                // DR["FMEMOAMOUNT"] = Math.Round(Val.ToDecimal(DR["FMEMOAMOUNT"]) / 1000, 2);
                                DR["FMEMOAMOUNT"] = Math.Round(Val.ToDecimal(DR["FMEMOAMOUNT"]), 2);
                            }
                            DTabExpMemoDetail.WriteXml(sw);
                            MemoEntryDetailExpForXML = sw.ToString();
                            MemoEntryDetailExpForXML = MemoEntryDetailExpForXML.Replace("<DocumentElement>", "<NewDataSet>");
                            MemoEntryDetailExpForXML = MemoEntryDetailExpForXML.Replace("</DocumentElement>", "</NewDataSet>");
                        }

                        AmtConversation_From1000();

                        Property.NETAMOUNT = Val.Val(txtNetAmount.Text);
                        Property.FNETAMOUNT = Math.Round(Val.Val(txtNetAmountFE.Text), 2);
                        Property.BrokerAmountFE = Val.Val(txtBrokerAmtFE.Text);
                        Property.BrokerAmount = Val.Val(txtBrokerAmt.Text);
                        Property.AdatAmtFE = Val.Val(txtAdatAmtFE.Text);
                        Property.AdatAmt = Val.Val(txtAdatAmt.Text);
                        Property.TOTALAVGDISC = Val.Val(txtMemoAvgDisc.Text);
                        Property.TOTALAVGRATE = Val.Val(txtMemoAvgRate.Text);
                        Property.GROSSAMOUNT = Val.Val(txtGrossAmount.Text);
                        Property.FIGSTAMOUNT = Val.Val(txtIGSTAmountFE.Text);
                        Property.FCGSTAMOUNT = Val.Val(txtCGSTAmountFE.Text);
                        Property.FSGSTAMOUNT = Val.Val(txtSGSTAmountFE.Text);
                        Property.FGROSSAMOUNT = Val.Val(txtGrossAmountFE.Text);
                        Property.StkAmtFE = Val.Val(txtStkAmtFE.Text);

                        Property.APPROVEMEMO_ID_A = (PstrApprove_MemoId == "") ? "00000000-0000-0000-0000-000000000000" : PstrApprove_MemoId;
                        Property = ObjMemo.SaveMemoEntry(cn_T, Property, MemoEntryDetailExpForXML, lblMode.Text, "", ""); // save export amt and sale amount same

                        CalculationNew(false);

                        Property.BrokerAmountFE = Val.Val(txtBrokerAmtFE.Text);
                        Property.BrokerAmount = Val.Val(txtBrokerAmt.Text);
                        Property.AdatAmtFE = Val.Val(txtAdatAmtFE.Text);
                        Property.AdatAmt = Val.Val(txtAdatAmt.Text);
                        Property.TOTALAVGDISC = Val.Val(txtMemoAvgDisc.Text);
                        Property.TOTALAVGRATE = Val.Val(txtMemoAvgRate.Text);
                        Property.GROSSAMOUNT = Val.Val(txtGrossAmount.Text);
                        Property.FIGSTAMOUNT = Val.Val(txtIGSTAmountFE.Text);
                        Property.FCGSTAMOUNT = Val.Val(txtCGSTAmountFE.Text);
                        Property.FSGSTAMOUNT = Val.Val(txtSGSTAmountFE.Text);
                        Property.NETAMOUNT = Val.Val(txtNetAmount.Text);
                        Property.FGROSSAMOUNT = Val.Val(txtGrossAmountFE.Text);
                        Property.FNETAMOUNT = Val.Val(txtNetAmountFE.Text);
                        Property.StkAmtFE = Val.Val(txtStkAmtFE.Text);

                        txtJangedNo.Text = Property.ReturnValueJanged;
                        lblMemoNo.Text = Property.ReturnValue;

                        Property.NETAMOUNT = Val.Val(txtNetAmount.Text);
                        Property.FNETAMOUNT = Val.Val(txtNetAmountFE.Text);

                        txtApproveMemoNo.Text = Property.ReturnValueJanged;
                        txtApproveMemo_ID.Text = Property.MEMO_ID;

                        Property.APPROVEMEMO_ID = txtApproveMemo_ID.Text;
                        Property.APPROVEMEMONO = Val.ToString(txtApproveMemoNo.Text);
                    }
                    ReturnMessageDesc = Property.ReturnMessageDesc;
                    ReturnMessageType = Property.ReturnMessageType;

                    //Property = null;

                    if (ReturnMessageType == "SUCCESS" && (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.SALESDELIVERYRETURN || mFormType == FORMTYPE.PURCHASERETURN || mFormType == FORMTYPE.PURCHASEISSUE))
                    {
                        if (lblMode.Text == "Add Mode" && mFormType == FORMTYPE.SALEINVOICE || lblMode.Text == "Edit Mode" && mFormType == FORMTYPE.SALEINVOICE)
                        {
                            if (BOConfiguration.gStrLoginSection == "B")
                            {
                                // SaveAccountDetail(cn_T, true); export ni entry save thase
                                SaveAccountDetail(cn_T, true); // export ni entry save nai thay
                            }
                            else
                            {
                                SaveAccountDetail(cn_T, false);
                            }
                            SaveBrokerAccountDetail(cn_T);
                        }
                    }
                    //transactionScope.Complete();
                    //transactionScope.Dispose();
                    cn_T.Close();
                }

                catch (Exception ex)
                {
                    //transactionScope.Dispose();
                    Global.MessageError(ex.Message.ToString());
                }
                //}
                //Global.Message(ReturnMessageDesc); /Comment : Pinali : 05-11-2019
                if (ReturnMessageType == "SUCCESS" && (mFormType == FORMTYPE.MEMOISSUE || mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.SALEINVOICE))
                {
                    /*
                    lblMode.Text = "Edit Mode";
                    if (Global.Confirm("DO YOU WANT MAILED INVOICE : " + txtJangedNo.Text) == System.Windows.Forms.DialogResult.Yes)
                    {
                        BtnEmail_Click(null, null);
                    }
                    else
                    {

                    }public Trn_SinglePrdProperty MFGGradingSave(Trn_SinglePrdProperty pClsProperty) //Add : Pinali : 28-07-2019
        {
            try
            {
                Ope.ClearParams();
                DataTable DTab = new DataTable();

                Ope.AddParams("COMPANY_ID", Config.COMPANY_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("MFGGRADING_NO", pClsProperty.MFGGradingNo, DbType.Int64, ParameterDirection.Input);
                Ope.AddParams("XMLDETSTR", pClsProperty.XMLDETSTR, DbType.Xml, ParameterDirection.Input);

                Ope.AddParams("ENTRYBY", Config.gEmployeeProperty.LEDGER_ID, DbType.Guid, ParameterDirection.Input);
                Ope.AddParams("ENTRYIP", Config.ComputerIP, DbType.String, ParameterDirection.Input);
                Ope.AddParams("RETURNVALUE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGETYPE", "", DbType.String, ParameterDirection.Output);
                Ope.AddParams("RETURNMESSAGEDESC", "", DbType.String, ParameterDirection.Output);

                ArrayList AL = Ope.ExeNonQueryWithReturnValues(Config.ConnectionString, Config.ProviderName, "Trn_MFGGradingSave", CommandType.StoredProcedure);
                if (AL.Count != 0)
                {
                    pClsProperty.ReturnValue = Val.ToString(AL[0]);
                    pClsProperty.ReturnMessageType = Val.ToString(AL[1]);
                    pClsProperty.ReturnMessageDesc = Val.ToString(AL[2]);
                }
            }
            catch (System.Exception ex)
            {
                pClsProperty.ReturnValue = "";
                pClsProperty.ReturnMessageType = "FAIL";
                pClsProperty.ReturnMessageDesc = ex.Message;

            }
            return pClsProperty;
        }


                    lblMode.Text = "Edit Mode";
                    if (Global.Confirm("DO YOU WANT TO PRINT INVOICE : " + txtJangedNo.Text) == System.Windows.Forms.DialogResult.Yes)
                    {
                        BtnPrint_Click(null, null);
                    }
                    else
                    {

                    }
                     */
                    Global.Message(ReturnMessageDesc);
                    this.Close();
                }
                else if (ReturnMessageType == "SUCCESS")
                {
                    Global.Message(ReturnMessageDesc);
                    this.Close();
                }
                else
                    Global.MessageError(ReturnMessageDesc);

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtTerms_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "TERMSNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_TERMS);

                    FrmSearch.mStrColumnsToHide = "TERMS_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtTerms.Text = Val.ToString(FrmSearch.DRow["TERMSNAME"]);
                        txtTermsDays.Text = Val.ToString(FrmSearch.DRow["TERMSDAYS"]);
                        txtTerms.Tag = Val.ToString(FrmSearch.DRow["TERMS_ID"]);
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

        private void repTxtShape_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDetail.FocusedRowHandle < 0)
                    return;

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "SHAPECODE,SHAPENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SHAPE);
                    FrmSearch.mStrColumnsToHide = "SHAPE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDetail.SetFocusedRowCellValue("SHAPENAME", Val.ToString(FrmSearch.DRow["SHAPECODE"]));
                        GrdDetail.SetFocusedRowCellValue("SHAPE_ID", Val.ToString(FrmSearch.DRow["SHAPE_ID"]));
                        FindRap(GrdDetail.GetFocusedDataRow(), GrdDetail.FocusedRowHandle);
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

        private void repTxtColor_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDetail.FocusedRowHandle < 0)
                    return;

                if (Global.OnKeyPressEveToPopup(e))
                {
                    DataTable DtabColor = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COLOR);
                    string StrParam = "";

                    if (mStrStockType == "ALL")
                        StrParam = "1=1";
                    else
                        StrParam = "COLORTYPE = '" + mStrStockType + "'";

                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "COLORCODE,COLORNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;

                    //FrmSearch.DTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.TABLE.MST_COLOR);
                    FrmSearch.mDTab = DtabColor.Select(StrParam, "").CopyToDataTable();

                    FrmSearch.mStrColumnsToHide = "COLOR_ID,COLORTYPE";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDetail.SetFocusedRowCellValue("COLORNAME", Val.ToString(FrmSearch.DRow["COLORNAME"]));
                        GrdDetail.SetFocusedRowCellValue("COLOR_ID", Val.ToString(FrmSearch.DRow["COLOR_ID"]));
                        FindRap(GrdDetail.GetFocusedDataRow(), GrdDetail.FocusedRowHandle);
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

        private void repTxtClarity_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDetail.FocusedRowHandle < 0)
                    return;

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "CLARITYCODE,CLARITYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CLARITY);
                    FrmSearch.mStrColumnsToHide = "CLARITY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDetail.SetFocusedRowCellValue("CLARITYNAME", Val.ToString(FrmSearch.DRow["CLARITYNAME"]));
                        GrdDetail.SetFocusedRowCellValue("CLARITY_ID", Val.ToString(FrmSearch.DRow["CLARITY_ID"]));
                        FindRap(GrdDetail.GetFocusedDataRow(), GrdDetail.FocusedRowHandle);
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

        private void repTxtCut_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDetail.FocusedRowHandle < 0)
                    return;

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "CUTCODE,CUTNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_CUT);
                    FrmSearch.mStrColumnsToHide = "CUT_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDetail.SetFocusedRowCellValue("CUTNAME", Val.ToString(FrmSearch.DRow["CUTCODE"]));
                        GrdDetail.SetFocusedRowCellValue("CUT_ID", Val.ToString(FrmSearch.DRow["CUT_ID"]));
                        FindRap(GrdDetail.GetFocusedDataRow(), GrdDetail.FocusedRowHandle);
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

        private void repTxtPol_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDetail.FocusedRowHandle < 0)
                    return;

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "POLCODE,POLNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_POL);
                    FrmSearch.mStrColumnsToHide = "POL_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDetail.SetFocusedRowCellValue("POLNAME", Val.ToString(FrmSearch.DRow["POLCODE"]));
                        GrdDetail.SetFocusedRowCellValue("POL_ID", Val.ToString(FrmSearch.DRow["POL_ID"]));
                        FindRap(GrdDetail.GetFocusedDataRow(), GrdDetail.FocusedRowHandle);
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

        private void repTxtSym_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDetail.FocusedRowHandle < 0)
                    return;

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "SYMCODE,SYMNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SYM);
                    FrmSearch.mStrColumnsToHide = "SYM_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDetail.SetFocusedRowCellValue("SYMNAME", Val.ToString(FrmSearch.DRow["SYMCODE"]));
                        GrdDetail.SetFocusedRowCellValue("SYM_ID", Val.ToString(FrmSearch.DRow["SYM_ID"]));
                        FindRap(GrdDetail.GetFocusedDataRow(), GrdDetail.FocusedRowHandle);
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

        private void repTxtFL_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDetail.FocusedRowHandle < 0)
                    return;

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "FLCODE,FLNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_FL);
                    FrmSearch.mStrColumnsToHide = "FL_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDetail.SetFocusedRowCellValue("FLNAME", Val.ToString(FrmSearch.DRow["FLNAME"]));
                        GrdDetail.SetFocusedRowCellValue("FL_ID", Val.ToString(FrmSearch.DRow["FL_ID"]));
                        FindRap(GrdDetail.GetFocusedDataRow(), GrdDetail.FocusedRowHandle);
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

        public void ClearStoneDetail(DataRow Dr)
        {
            Dr["MEMODETAIL_ID"] = Guid.Empty;
            Dr["STOCK_ID"] = Guid.Empty;
            Dr["STOCKNO"] = string.Empty;
            Dr["STOCKTYPE"] = string.Empty;

            Dr["STATUS"] = "";

            Dr["PCS"] = 0;
            Dr["CARAT"] = 0;
            Dr["BALANCEPCS"] = 0;
            Dr["BALANCECARAT"] = 0;

            Dr["SHAPENAME"] = string.Empty;
            Dr["SHAPE_ID"] = 0;
            Dr["COLORNAME"] = string.Empty;
            Dr["COLOR_ID"] = 0;
            Dr["CLARITYNAME"] = string.Empty;
            Dr["CLARITY_ID"] = 0;
            Dr["CUTNAME"] = string.Empty;
            Dr["CUT_ID"] = 0;
            Dr["POLNAME"] = string.Empty;
            Dr["POL_ID"] = 0;
            Dr["SYMNAME"] = string.Empty;
            Dr["SYM_ID"] = 0;
            Dr["FLNAME"] = string.Empty;
            Dr["FL_ID"] = 0;
            Dr["MEASUREMENT"] = string.Empty;
            Dr["LABNAME"] = string.Empty;
            Dr["LAB_ID"] = 0;
            Dr["LABREPORTNO"] = string.Empty;
            Dr["LOCATIONNAME"] = string.Empty;
            Dr["LOCATION_ID"] = 0;

            Dr["SIZE_ID"] = 0;
            Dr["SIZENAME"] = string.Empty;

            Dr["SALERAPAPORT"] = 0;
            Dr["SALEPRICEPERCARAT"] = 0;
            Dr["SALEDISCOUNT"] = 0;
            Dr["SALEAMOUNT"] = 0;

            Dr["MEMORAPAPORT"] = 0;
            Dr["MEMOPRICEPERCARAT"] = 0;
            Dr["MEMODISCOUNT"] = 0;
            Dr["MEMOAMOUNT"] = 0;
            Dr["FMEMOPRICEPERCARAT"] = 0;
            Dr["FMEMOAMOUNT"] = 0;
            Dr["REMARK"] = string.Empty;
        }

        private void repTxtStoneNo_Validating(object sender, CancelEventArgs e)
        {
            if (GrdDetail.FocusedRowHandle < 0)
                return;

            try
            {
                //HINA - START
                if (mFormType == FORMTYPE.PURCHASEISSUE)
                {
                    return;
                }
                //HINA - END

                GrdDetail.PostEditor();
                DataRow dr = GrdDetail.GetFocusedDataRow();

                if (Val.ToString(GrdDetail.EditingValue).Trim().Equals(string.Empty))
                    return;

                LiveStockProperty Property = new LiveStockProperty();
                Property.STOCKNO = Val.ToString(GrdDetail.EditingValue);
                DataRow DrDetail = new BOTRN_StockUpload().GetStockDataStoneNoWise(Property);

                DataRow DRNew = DTabMemoDetail.NewRow();


                if (DrDetail == null)
                {
                    ClearStoneDetail(dr);
                    return;
                }

                //Add : Pinali : 27-08-2019 
                if (mFormType == FORMTYPE.PURCHASERETURN)
                {
                    if (!(Val.ToString(DrDetail["STATUS"]) == "NONE"
                         ) && Val.ToString(DrDetail["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        Global.Message("Opps... Kindly Select (SOLD) Status Stones\n\nThis Packet Has Another Status\n\n" + Val.ToString(DrDetail["STOCKNO"]) + " = " + Val.ToString(DrDetail["STATUS"]));
                        ClearStoneDetail(dr);
                        return;
                    }
                }
                else if (mFormType == FORMTYPE.ORDERCONFIRM)
                {
                    if (!(
                        Val.ToString(DrDetail["STATUS"]) == "AVAILABLE" ||
                        Val.ToString(DrDetail["STATUS"]) == "MEMO" ||
                        Val.ToString(DrDetail["STATUS"]) == "OFFLINE" ||
                        Val.ToString(DrDetail["STATUS"]) == "HOLD"
                        ) && Val.ToString(DrDetail["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        Global.Message("Opps...Kindly Select (Available / Memo / Offline / Hold) Status Stones\n\nThis Packet Has Another Status\n\n" + Val.ToString(DrDetail["STOCKNO"]) + " = " + Val.ToString(DrDetail["STATUS"]));
                        ClearStoneDetail(dr);
                        return;
                    }
                }
                else if (mFormType == FORMTYPE.SALEINVOICE)
                {
                    if (!(Val.ToString(DrDetail["STATUS"]) == "AVAILABLE" ||
                            Val.ToString(DrDetail["STATUS"]) == "MEMO" ||
                            Val.ToString(DrDetail["STATUS"]) == "OFFLINE" ||
                            Val.ToString(DrDetail["STATUS"]) == "SOLD"
                         ) && Val.ToString(DrDetail["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        Global.Message("Opps... Kindly Select (SOLD) Status Stones\n\nThis Packets Has Another Status\n\n" + Val.ToString(DrDetail["STOCKNO"]) + " = " + Val.ToString(DrDetail["STATUS"]));
                        ClearStoneDetail(dr);
                        return;
                    }
                }
                else if (mFormType == FORMTYPE.SALESDELIVERYRETURN)
                {
                    if (Val.ToString(DrDetail["STATUS"]) != "DELIVERY" && Val.ToString(DrDetail["STOCKTYPE"]).ToUpper() == "SINGLE")
                    {
                        Global.Message("Opps... Kindly Select (DELIVERY) Status Stones\n\nThese Packets Have Another Status\n\n" + Val.ToString(DrDetail["STOCKNO"]) + " = " + Val.ToString(DrDetail["STATUS"]));
                        ClearStoneDetail(dr);
                        return;
                    }
                }

                //End : Pinali : 27-08-2019


                dr["MEMODETAIL_ID"] = BusLib.Configuration.BOConfiguration.FindNewSequentialID();
                dr["STOCK_ID"] = DrDetail["STOCK_ID"];
                dr["STOCKNO"] = DrDetail["STOCKNO"];
                dr["STOCKTYPE"] = DrDetail["STOCKTYPE"];

                dr["STATUS"] = "PENDING";

                //dr["PCS"] = DrDetail["BALANCEPCS"];
                //dr["CARAT"] = DrDetail["BALANCECARAT"];
                //dr["BALANCEPCS"] = DrDetail["BALANCEPCS"];
                //dr["BALANCECARAT"] = DrDetail["BALANCECARAT"];

                if (Val.Val(DrDetail["MEMOPENDINGCARAT"]) != 0)
                {
                    dr["CARAT"] = DrDetail["MEMOPENDINGCARAT"];
                    dr["BALANCECARAT"] = DrDetail["MEMOPENDINGCARAT"];
                }

                if (Val.Val(DrDetail["MEMOPENDINGPCS"]) != 0)
                {
                    dr["PCS"] = DrDetail["MEMOPENDINGPCS"];
                    dr["BALANCEPCS"] = DrDetail["MEMOPENDINGPCS"];
                }

                if (Val.Val(DrDetail["MEMOPENDINGCARAT"]) == 0)
                {
                    dr["CARAT"] = DrDetail["BALANCECARAT"];
                    dr["BALANCECARAT"] = DrDetail["BALANCECARAT"];
                }

                if (Val.Val(DrDetail["MEMOPENDINGPCS"]) == 0)
                {
                    dr["PCS"] = DrDetail["BALANCEPCS"];
                    dr["BALANCEPCS"] = DrDetail["BALANCEPCS"];
                }

                dr["SHAPENAME"] = DrDetail["SHAPENAME"];
                dr["SHAPE_ID"] = DrDetail["SHAPE_ID"];
                dr["COLORNAME"] = DrDetail["COLORNAME"];
                dr["COLOR_ID"] = DrDetail["COLOR_ID"];
                dr["CLARITYNAME"] = DrDetail["CLARITYNAME"];
                dr["CLARITY_ID"] = DrDetail["CLARITY_ID"];
                dr["CUTNAME"] = DrDetail["CUTNAME"];
                dr["CUT_ID"] = DrDetail["CUT_ID"];
                dr["POLNAME"] = DrDetail["POLNAME"];
                dr["POL_ID"] = DrDetail["POL_ID"];
                dr["SYMNAME"] = DrDetail["SYMNAME"];
                dr["SYM_ID"] = DrDetail["SYM_ID"];
                dr["FLNAME"] = DrDetail["FLNAME"];
                dr["FL_ID"] = DrDetail["FL_ID"];
                dr["MEASUREMENT"] = DrDetail["MEASUREMENT"];
                dr["LABNAME"] = DrDetail["LABNAME"];
                dr["LAB_ID"] = DrDetail["LAB_ID"];
                dr["LABREPORTNO"] = DrDetail["LABREPORTNO"];
                dr["LOCATIONNAME"] = DrDetail["LOCATIONNAME"];
                dr["LOCATION_ID"] = DrDetail["LOCATION_ID"];

                dr["SIZE_ID"] = DrDetail["SIZE_ID"];
                dr["SIZENAME"] = DrDetail["SIZENAME"];

                dr["SALERAPAPORT"] = DrDetail["SALERAPAPORT"];
                dr["SALEPRICEPERCARAT"] = DrDetail["SALEPRICEPERCARAT"];
                dr["SALEDISCOUNT"] = DrDetail["SALEDISCOUNT"];
                dr["SALEAMOUNT"] = DrDetail["SALEAMOUNT"];

                //dr["MEMORAPAPORT"] = DrDetail["SALERAPAPORT"];
                //dr["MEMOPRICEPERCARAT"] = DrDetail["SALEPRICEPERCARAT"];
                //dr["MEMODISCOUNT"] = DrDetail["SALEDISCOUNT"];
                //dr["MEMOAMOUNT"] = DrDetail["SALEAMOUNT"];
                //dr["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DrDetail["SALEPRICEPERCARAT"]) * Val.Val(txtExcRate.Text));
                //dr["FMEMOAMOUNT"] = Math.Round(Val.Val(DrDetail["SALEAMOUNT"]) * Val.Val(txtExcRate.Text));

                if (Val.Val(DrDetail["MEMORAPAPORT"]) == 0)
                {
                    dr["MEMORAPAPORT"] = DrDetail["SALERAPAPORT"];
                    dr["MEMODISCOUNT"] = DrDetail["SALEDISCOUNT"];
                    dr["MEMOPRICEPERCARAT"] = DrDetail["SALEPRICEPERCARAT"];
                    dr["MEMOAMOUNT"] = DrDetail["SALEAMOUNT"];
                    dr["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DrDetail["SALEPRICEPERCARAT"]), 2);
                    if (BOConfiguration.gStrLoginSection == "B")
                    {
                        dr["FMEMOAMOUNT"] = Math.Round((Val.Val(txtExcRate.Text) * Val.Val(DrDetail["SALEAMOUNT"])) / 1000, 2);
                    }
                    else
                    {
                        dr["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DrDetail["SALEAMOUNT"]), 2);
                    }

                }
                else
                {
                    dr["MEMORAPAPORT"] = DrDetail["MEMORAPAPORT"];
                    dr["MEMODISCOUNT"] = DrDetail["MEMODISCOUNT"];
                    dr["MEMOPRICEPERCARAT"] = DrDetail["MEMOPRICEPERCARAT"];
                    dr["MEMOAMOUNT"] = DrDetail["MEMOAMOUNT"];
                    dr["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DrDetail["MEMOPRICEPERCARAT"]), 2);
                    dr["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DrDetail["MEMOAMOUNT"]), 2);
                    if (BOConfiguration.gStrLoginSection == "B")
                    {
                        dr["FMEMOAMOUNT"] = Math.Round((Val.Val(txtExcRate.Text) * Val.Val(DrDetail["MEMOAMOUNT"])) / 1000, 2);
                    }
                    else
                    {
                        dr["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DrDetail["MEMOAMOUNT"]), 2);
                    }

                }

                dr["REMARK"] = DrDetail["REMARK"];

                //Calculation();
                CalculationNew();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void GrdDetail_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            if (GrdDetail.FocusedRowHandle < 0)
                return;
            try
            {
                DataRow Dr = GrdDetail.GetFocusedDataRow();

                if (Val.ToString(Dr["STOCKTYPE"]).ToUpper() == "PARCEL")
                {
                    GrdDetail.Columns["PCS"].OptionsColumn.AllowEdit = true;
                    GrdDetail.Columns["CARAT"].OptionsColumn.AllowEdit = true;
                }
                else
                {
                    GrdDetail.Columns["PCS"].OptionsColumn.AllowEdit = false;
                    GrdDetail.Columns["CARAT"].OptionsColumn.AllowEdit = false;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void GrdDetail_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            if (GrdDetail.FocusedRowHandle < 0)
                return;
            try
            {
                DataRow Dr = GrdDetail.GetFocusedDataRow();

                if (Val.ToString(Dr["STOCKTYPE"]).ToUpper() == "PARCEL")
                {
                    GrdDetail.Columns["PCS"].OptionsColumn.AllowEdit = true;
                    GrdDetail.Columns["CARAT"].OptionsColumn.AllowEdit = true;
                }
                else
                {
                    GrdDetail.Columns["PCS"].OptionsColumn.AllowEdit = false;
                    GrdDetail.Columns["CARAT"].OptionsColumn.AllowEdit = false;
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void repTxtCostDisount_Validating(object sender, CancelEventArgs e)
        {
            if (GrdDetail.FocusedRowHandle < 0)
                return;

            try
            {
                GrdDetail.PostEditor();

                double DouCarat = 0;
                double DouCostDiscount = 0;
                double DouCostPricePerCarat = 0;
                double DouCostRapaport = 0;
                double DouCostAmount = 0;

                DataRow DR = GrdDetail.GetFocusedDataRow();

                DouCarat = Val.Val(GrdDetail.GetFocusedRowCellValue("CARAT"));
                DouCostDiscount = Val.Val(GrdDetail.EditingValue);
                DouCostRapaport = Val.Val(GrdDetail.GetFocusedRowCellValue("MEMORAPAPORT"));

                DouCostPricePerCarat = Math.Round(DouCostRapaport + DouCostRapaport * DouCostDiscount / 100, 2);
                DouCostAmount = Math.Round(DouCostPricePerCarat * DouCarat, 2);

                DR["MEMOPRICEPERCARAT"] = DouCostPricePerCarat;
                DR["MEMOAMOUNT"] = DouCostAmount;

                //Calculation();
                CalculationNew();

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void repTxtCostPricePerCarat_Validating(object sender, CancelEventArgs e)
        {
            if (GrdDetail.FocusedRowHandle < 0)
                return;

            try
            {
                GrdDetail.PostEditor();

                double DouCarat = 0;
                double DouCostDiscount = 0;
                double DouCostPricePerCarat = 0;
                double DouCostRapaport = 0;
                double DouCostAmount = 0;

                DataRow DR = GrdDetail.GetFocusedDataRow();
                DouCarat = Val.Val(GrdDetail.GetFocusedRowCellValue("CARAT"));
                DouCostPricePerCarat = Val.Val(GrdDetail.EditingValue);

                DouCostRapaport = Val.Val(GrdDetail.GetFocusedRowCellValue("MEMORAPAPORT"));

                if (DouCostRapaport != 0)
                    DouCostDiscount = Math.Round((DouCostPricePerCarat - DouCostRapaport) / DouCostRapaport * 100, 2);
                else
                    DouCostDiscount = 0;

                DouCostAmount = Math.Round(DouCostPricePerCarat * DouCarat, 2);

                DR["MEMODISCOUNT"] = DouCostDiscount;
                DR["MEMOAMOUNT"] = DouCostAmount;

                //Calculation();
                CalculationNew();

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        public void Calculation()
        {
            try
            {
                int IntPcs = 0;
                double DouCarat = 0;
                double DouSaleRapAmt = 0;
                double DouSaleAmt = 0;
                double DouMemoTotalAmt = 0;
                double DouMemoTotalAmtFE = 0;

                for (int IntI = 0; IntI < GrdDetail.RowCount; IntI++)
                {
                    DataRow DRow = GrdDetail.GetDataRow(IntI);

                    IntPcs = IntPcs + Val.ToInt(DRow["PCS"]);
                    DouCarat = DouCarat + Val.Val(DRow["CARAT"]);

                    DouSaleRapAmt = DouSaleRapAmt + Val.Val(DRow["SALERAPAPORT"]) * Val.Val(DRow["CARAT"]);

                    DouSaleAmt = DouSaleAmt + Val.Val(DRow["SALEAMOUNT"]);
                    DouMemoTotalAmt = DouMemoTotalAmt + Val.Val(DRow["MEMOAMOUNT"]);
                    DouMemoTotalAmtFE = DouMemoTotalAmtFE + Val.Val(DRow["FMEMOAMOUNT"]);
                }

                lblTotalPcs.Text = Val.Format(IntPcs, "########0");
                lblTotalCarat.Text = Val.Format(DouCarat, "########0.00");

                if (DouSaleRapAmt != 0)
                {
                    lblTotalAvgDisc.Text = Val.Format(((DouSaleAmt - DouSaleRapAmt) / DouSaleRapAmt) * 100, "########0.00");
                }
                else
                {
                    lblTotalAvgDisc.Text = "0.00";
                }

                lblTotalAmount.Text = Val.Format(DouSaleAmt, "########0.00");
                lblTotalAvgRate.Text = Val.Format(DouSaleAmt / DouCarat, "########0.00");

                if (DouSaleRapAmt != 0)
                {
                    txtMemoAvgDisc.Text = Val.Format(((DouMemoTotalAmt - DouSaleRapAmt) / DouSaleRapAmt) * 100, "########0.00"); ;
                }
                else
                {
                    txtMemoAvgDisc.Text = "0.00";
                }
                txtMemoAmount.Text = Val.Format(DouMemoTotalAmt, "########0.00");
                txtGrossAmount.Text = Val.Format(DouMemoTotalAmt, "########0.00");
                txtGrossAmountFE.Text = Val.Format(DouMemoTotalAmtFE, "########0.00");
                txtMemoAvgRate.Text = Val.Format(DouMemoTotalAmt / DouCarat, "########0.00");

                double DouDiscAmt = Math.Round(Val.Val(txtGrossAmount.Text) * Val.Val(txtDiscPer.Text) / 100, 4);
                double DouInsAmt = Math.Round(Val.Val(txtGrossAmount.Text) * Val.Val(txtInsurancePer.Text) / 100, 4);
                double DouShipAmt = Math.Round(Val.Val(txtGrossAmount.Text) * Val.Val(txtShippingPer.Text) / 100, 4);
                double DouGSTAmt = Math.Round(Val.Val(txtGrossAmount.Text) * Val.Val(txtGSTPer.Text) / 100, 4);

                txtDiscAmount.Text = Val.Format(DouDiscAmt, "########0.00");
                txtInsuranceAmount.Text = Val.Format(DouInsAmt, "########0.00");
                txtShippingAmount.Text = Val.Format(DouShipAmt, "########0.00");
                txtGSTAmount.Text = Val.Format(DouGSTAmt, "########0.00");
                double DouNetAmt = Val.Val(txtGrossAmount.Text) + DouDiscAmt + DouInsAmt + DouShipAmt + DouGSTAmt;
                txtNetAmount.Text = Val.Format(DouNetAmt, "########0.00");

                double DouDiscAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouDiscAmt, 2);
                double DouInsAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouInsAmt, 2);
                double DouShipAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouShipAmt, 2);
                double DouGSTAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouGSTAmt, 2);

                txtDiscAmountFE.Text = Val.Format(DouDiscAmtFE, "########0.00");
                txtInsuranceAmountFE.Text = Val.Format(DouInsAmtFE, "########0.00");
                txtShippingAmountFE.Text = Val.Format(DouShipAmtFE, "########0.00");
                txtGSTAmountFE.Text = Val.Format(DouGSTAmtFE, "########0.00");
                double DouNetAmtFE = Val.Val(txtGrossAmountFE.Text) + DouDiscAmtFE + DouInsAmtFE + DouShipAmtFE + DouGSTAmtFE;
                txtNetAmountFE.Text = Val.Format(DouNetAmtFE, "########0.00");


            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        public void CalculationNew(bool pBool = true) // Add : Pinali : 29-08-2019
        {
            try
            {
                int IntPcs = 0;
                double DouCarat = 0;
                double DouSaleRapAmt = 0;
                double DouSaleAmt = 0;
                double DouMemoTotalAmt = 0;
                double DouMemoTotalAmtFE = 0;
                double DouExpInvAmt = 0;
                double DouExpInvAmtFE = 0;

                for (int IntI = 0; IntI < GrdDetail.RowCount; IntI++)
                {
                    DataRow DRow = GrdDetail.GetDataRow(IntI);

                    IntPcs = IntPcs + Val.ToInt(DRow["PCS"]);
                    DouCarat = DouCarat + Val.Val(DRow["CARAT"]);

                    DouSaleRapAmt = DouSaleRapAmt + (Val.Val(DRow["SALERAPAPORT"]) * Val.Val(DRow["CARAT"]));

                    DouSaleAmt = DouSaleAmt + Val.Val(DRow["SALEAMOUNT"]);
                    DouMemoTotalAmt = DouMemoTotalAmt + Val.Val(DRow["MEMOAMOUNT"]);
                    DouMemoTotalAmtFE = DouMemoTotalAmtFE + Val.Val(DRow["FMEMOAMOUNT"]);

                    #region Export calculation

                    if (!ChkUpdExport.Checked)
                    {
                        DRow["EXPINVOICERATE"] = DRow["MEMOPRICEPERCARAT"];
                        DRow["EXPINVOICEAMT"] = DRow["MEMOAMOUNT"];
                        DRow["EXPINVOICERATEFE"] = DRow["FMEMOPRICEPERCARAT"];
                        DRow["EXPINVOICEAMTFE"] = DRow["FMEMOAMOUNT"];
                    }

                    DouExpInvAmt = DouExpInvAmt + Val.Val(DRow["EXPINVOICEAMT"]);
                    DouExpInvAmtFE = DouExpInvAmtFE + Val.Val(DRow["EXPINVOICEAMTFE"]);

                    #endregion
                }

                txtExpInvAmt.Text = Val.Format(DouExpInvAmt, "########0.0000");
                txtExpInvAmtFE.Text = Val.Format(DouExpInvAmtFE, "########0.0000");

                lblTotalPcs.Text = Val.Format(IntPcs, "########0");
                lblTotalCarat.Text = Val.Format(DouCarat, "########0.00");

                if (DouSaleRapAmt != 0)
                {
                    lblTotalAvgDisc.Text = Val.Format(((DouSaleAmt - DouSaleRapAmt) / DouSaleRapAmt) * 100, "########0.00"); //#P
                    //  lblTotalAvgDisc.Text = Val.Format(((DouSaleRapAmt - DouSaleAmt) / DouSaleRapAmt) * 100, "########0.00");
                }
                else
                {
                    lblTotalAvgDisc.Text = "0.00";
                }

                lblTotalAmount.Text = Val.Format(DouSaleAmt, "########0.00");
                lblTotalAvgRate.Text = Val.Format(DouSaleAmt / DouCarat, "########0.00");

                if ((mFormType != FORMTYPE.PURCHASEISSUE) && (DouSaleRapAmt != 0)) // Cndtn Changed & add Else if : Pinali : 29-08-2019
                {
                    //txtMemoAvgDisc.Text = Val.Format(((DouMemoTotalAmt - DouSaleRapAmt) / DouSaleRapAmt) * 100, "########0.00"); //#P
                    txtMemoAvgDisc.Text = Val.Format(((DouSaleRapAmt - DouMemoTotalAmt) / DouSaleRapAmt) * 100, "########0.00"); //#P:23-04-2021
                }
                else if ((mFormType == FORMTYPE.PURCHASEISSUE) && (DouSaleRapAmt != 0)) // For Purchase Invoice
                {
                    //txtMemoAvgDisc.Text = Val.Format(((DouSaleAmt - DouSaleRapAmt) / DouSaleRapAmt) * 100, "########0.00"); //#P
                    txtMemoAvgDisc.Text = Val.Format(((DouSaleRapAmt - DouSaleAmt) / DouSaleRapAmt) * 100, "########0.00"); //#P:23-04-2021
                }
                else
                {
                    txtMemoAvgDisc.Text = "0.00";
                }

                //txtMemoAmount.Text = Val.Format(DouMemoTotalAmt, "########0.00");
                //txtGrossAmount.Text = Val.Format(DouMemoTotalAmt, "########0.00");
                //txtGrossAmountFE.Text = Val.Format(DouMemoTotalAmtFE, "########0.00");
                //txtMemoAvgRate.Text = Val.Format(DouMemoTotalAmt / DouCarat, "########0.00");

                if (mFormType == FORMTYPE.PURCHASEISSUE) //Add If Portion : Pinali : 29-08-2019 : For Purchase Invoice
                {
                    txtMemoAmount.Text = Val.Format(DouSaleAmt, "########0.00");
                    txtGrossAmount.Text = Val.Format(DouSaleAmt, "########0.00");
                    txtGrossAmountFE.Text = Val.Format(Val.Val(DouSaleAmt) * Val.Val(txtExcRate.Text), "########0.00");
                    txtMemoAvgRate.Text = Val.Format(DouSaleAmt / DouCarat, "########0.00");
                }
                else
                {
                    txtMemoAmount.Text = Val.Format(DouMemoTotalAmt, "########0.00");
                    txtGrossAmount.Text = Val.Format(DouMemoTotalAmt, "########0.00");
                    txtGrossAmountFE.Text = Val.Format(DouMemoTotalAmtFE, "########0.00");
                    txtMemoAvgRate.Text = Val.Format(DouMemoTotalAmt / DouCarat, "########0.00");
                }


                //double DouDiscAmt = Math.Round(Val.Val(txtGrossAmount.Text) * Val.Val(txtDiscPer.Text) / 100, 4); --#P : 25-12-2019
                double DouDiscAmt = Math.Round(Val.Val(txtDiscAmount.Text), 4);

                //double DouGSTAmt = Math.Round(Val.Val(txtGrossAmount.Text) * Val.Val(txtGSTPer.Text) / 100, 4); --#P : 25-12-2019
                double DouGSTAmt = Math.Round(Val.Val(txtGSTAmount.Text), 4);

                //double DouInsAmt = Math.Round(Val.Val(txtGrossAmount.Text) * Val.Val(txtInsurancePer.Text) / 100, 4); --#P : 25-12-2019
                //double DouShipAmt = Math.Round(Val.Val(txtGrossAmount.Text) * Val.Val(txtShippingPer.Text) / 100, 4); --#P : 25-12-2019
                double DouInsAmt = Math.Round(Val.Val(txtInsuranceAmount.Text), 4);
                double DouShipAmt = Math.Round(Val.Val(txtShippingAmount.Text), 4);

                double DouIGSTAmt = Math.Round(Val.Val(txtIGSTAmount.Text), 4);
                double DouCGSTAmt = Math.Round(Val.Val(txtCGSTAmount.Text), 4);
                double DouSGSTAmt = Math.Round(Val.Val(txtSGSTAmount.Text), 4);

                txtDiscAmount.Text = Val.Format(DouDiscAmt, "########0.000");
                txtInsuranceAmount.Text = Val.Format(DouInsAmt, "########0.000");
                txtShippingAmount.Text = Val.Format(DouShipAmt, "########0.000");
                txtGSTAmount.Text = Val.Format(DouGSTAmt, "########0.000");

                txtIGSTAmount.Text = Val.Format(DouIGSTAmt, "########0.000");
                txtCGSTAmount.Text = Val.Format(DouCGSTAmt, "########0.000");
                txtSGSTAmount.Text = Val.Format(DouSGSTAmt, "########0.000");

                double DouNetAmt = (Val.Val(txtGrossAmount.Text) + DouIGSTAmt + DouCGSTAmt + DouSGSTAmt + DouInsAmt + DouShipAmt + DouGSTAmt) - DouDiscAmt;
                txtNetAmount.Text = Val.Format(DouNetAmt, "########0.000");

                //double DouDiscAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouDiscAmt, 2); ---#P : 25-12-2019
                double DouDiscAmtFE = Math.Round(Val.Val(txtDiscAmountFE.Text), 2);

                //double DouGSTAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouGSTAmt, 2); ---#P : 25-12-2019
                double DouGSTAmtFE = Math.Round(Val.Val(txtGSTAmountFE.Text), 2);

                double DouInsAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouInsAmt, 2);
                double DouShipAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouShipAmt, 2);

                double DouIGSTAmtFE = 0;
                double DouCGSTAmtFE = 0;
                double DouSGSTAmtFE = 0;

                if (BOConfiguration.gStrLoginSection == "B")
                {
                    DouIGSTAmtFE = Math.Round((Val.Val(txtIGSTAmount.Text) * Val.Val(txtExcRate.Text)) / 1000, 3);
                    DouCGSTAmtFE = Math.Round((Val.Val(txtCGSTAmount.Text) * Val.Val(txtExcRate.Text)) / 1000, 3);
                    DouSGSTAmtFE = Math.Round((Val.Val(txtSGSTAmount.Text) * Val.Val(txtExcRate.Text)) / 1000, 3);
                }
                else
                {
                    DouIGSTAmtFE = Math.Round(Val.Val(txtIGSTAmountFE.Text), 3);
                    DouCGSTAmtFE = Math.Round(Val.Val(txtCGSTAmountFE.Text), 3);
                    DouSGSTAmtFE = Math.Round(Val.Val(txtSGSTAmountFE.Text), 3);
                }


                //#K : 08122020
                if (mFormType == FORMTYPE.SALEINVOICE)
                {
                    // Comment and change code by khushbu 06/09/21. To remove rounding funtionality
                    //txtGrossAmountFE.Text = Val.ToString(Val.Format(Math.Round(Val.Val(txtGrossAmountFE.Text), 0, MidpointRounding.AwayFromZero), "########0.00"));
                    //txtDiscAmountFE.Text = Val.ToString(Val.Format(Math.Round(DouDiscAmtFE, 0, MidpointRounding.AwayFromZero), "########0.00"));
                    //txtInsuranceAmountFE.Text = Val.ToString(Val.Format(Math.Round(DouInsAmtFE, 0, MidpointRounding.AwayFromZero), "########0.00"));
                    //txtShippingAmountFE.Text = Val.ToString(Val.Format(Math.Round(DouShipAmtFE, 0, MidpointRounding.AwayFromZero), "########0.00"));
                    //txtGSTAmountFE.Text = Val.ToString(Val.Format(Math.Round(DouGSTAmtFE, 0, MidpointRounding.AwayFromZero), "########0.000"));

                    //txtIGSTAmountFE.Text = Val.ToString(Val.Format(Math.Round(DouIGSTAmtFE, 0, MidpointRounding.AwayFromZero), "########0.000"));
                    //txtCGSTAmountFE.Text = Val.ToString(Val.Format(Math.Round(DouCGSTAmtFE, 0, MidpointRounding.AwayFromZero), "########0.000"));
                    //txtSGSTAmountFE.Text = Val.ToString(Val.Format(Math.Round(DouSGSTAmtFE, 0, MidpointRounding.AwayFromZero), "########0.000"));

                    txtIGSTAmountFE.Text = Val.ToString(Val.Format(DouIGSTAmtFE, "########0.000"));
                    txtCGSTAmountFE.Text = Val.ToString(Val.Format(DouCGSTAmtFE, "########0.000"));
                    txtSGSTAmountFE.Text = Val.ToString(Val.Format(DouSGSTAmtFE, "########0.000"));

                    txtGrossAmountFE.Text = Val.ToString(Val.Format(Val.Val(txtGrossAmountFE.Text), "########0.000"));
                    txtDiscAmountFE.Text = Val.ToString(Val.Format(DouDiscAmtFE, "########0.000"));
                    txtInsuranceAmountFE.Text = Val.ToString(Val.Format(DouInsAmtFE, "########0.000"));
                    txtShippingAmountFE.Text = Val.ToString(Val.Format(DouShipAmtFE, "########0.000"));
                    txtGSTAmountFE.Text = Val.ToString(Val.Format(DouGSTAmtFE, "########0.000"));

                }
                else
                {
                    txtDiscAmountFE.Text = Val.Format(DouDiscAmtFE, "########0.000");
                    txtInsuranceAmountFE.Text = Val.Format(DouInsAmtFE, "########0.000");
                    txtShippingAmountFE.Text = Val.Format(DouShipAmtFE, "########0.000");
                    txtGSTAmountFE.Text = Val.Format(DouGSTAmtFE, "########0.000");

                    txtIGSTAmountFE.Text = Val.Format(DouIGSTAmtFE, "########0.000");
                    txtCGSTAmountFE.Text = Val.Format(DouCGSTAmtFE, "########0.000");
                    txtSGSTAmountFE.Text = Val.Format(DouSGSTAmtFE, "########0.000");
                }

                //if (cmbBillType.Text.ToUpper() == "RUPEESBILL" || cmbBillType.Text.ToUpper() == "DOLLARBILL")
                //{
                //    double DouNetAmtFE = Math.Round((Val.Val(txtGrossAmountFE.Text) + DouIGSTAmtFE + DouCGSTAmtFE + DouSGSTAmtFE), 0, MidpointRounding.AwayFromZero);
                //    txtNetAmountFE.Text = Val.Format(DouNetAmtFE, "########0.00");
                //}
                //else
                //{
                //    double DouNetAmtFE = Math.Round((Val.Val(txtGrossAmountFE.Text) + DouInsAmtFE + DouShipAmtFE + DouGSTAmtFE) - DouDiscAmtFE, 0, MidpointRounding.AwayFromZero);
                //    txtNetAmountFE.Text = Val.Format(DouNetAmtFE, "########0.00");
                //}

                double DouNetAmtFE = (Val.Val(txtGrossAmountFE.Text) + DouIGSTAmtFE + DouCGSTAmtFE + DouSGSTAmtFE + DouInsAmtFE + DouShipAmtFE + DouGSTAmtFE) - DouDiscAmtFE;
                txtNetAmountFE.Text = Val.Format(DouNetAmtFE, "########0.000");

                //Added & Comment by Daksha on 17/04/2023 'Calculate Adat&Broker on GrossAmt : And Less Adat then Calculate Broker
                //Old Code
                //txtBrokerAmtFE.Text = Val.ToString(Math.Round((Val.Val(txtNetAmountFE.Text) * Val.Val(txtBaseBrokeragePer.Text)) / 100, 3));
                //txtAdatAmtFE.Text = Val.ToString(Math.Round((Val.Val(txtNetAmountFE.Text) * Val.Val(txtAdatPer.Text)) / 100, 3));                              
                //txtStkAmtFE.Text = Val.Format(Val.Val(txtNetAmountFE.Text) - (Val.Val(txtBrokerAmtFE.Text) + Val.Val(txtAdatAmtFE.Text)), "########0.000");

                txtAdatAmtFE.Text = Val.ToString(Math.Round((Val.Val(txtGrossAmountFE.Text) * Val.Val(txtAdatPer.Text)) / 100, 3));
                txtBrokerAmtFE.Text = Val.ToString(Math.Round(((Val.Val(txtGrossAmountFE.Text) - Val.Val(txtAdatAmtFE.Text)) * Val.Val(txtBaseBrokeragePer.Text)) / 100, 3));
                txtStkAmtFE.Text = Val.Format(Val.Val(txtGrossAmountFE.Text) - (Val.Val(txtBrokerAmtFE.Text) + Val.Val(txtAdatAmtFE.Text)), "########0.000");
                //End as Daksha

                txtAdatAmt.Text = Val.ToString(Math.Round(Val.Val(txtAdatAmtFE.Text) / Val.Val(txtExcRate.Text), 3));
                txtBrokerAmt.Text = Val.ToString(Math.Round(Val.Val(txtBrokerAmtFE.Text) / Val.Val(txtExcRate.Text), 3));


                if (pBool)
                {
                    if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.SALESDELIVERYRETURN || mFormType == FORMTYPE.PURCHASERETURN || mFormType == FORMTYPE.PURCHASEISSUE)
                    {
                        AddAccountingEffect();
                        AddBrokerAccountingEffect();
                        AddExportAccountingEffect();
                    }
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }


        public void AmtConversation_From1000() // Add : Khushbu 08-09-21
        {
            try
            {
                int IntPcs = 0;
                double DouCarat = 0;
                double DouSaleRapAmt = 0;
                double DouSaleAmt = 0;
                double DouMemoTotalAmt = 0;
                double DouMemoTotalAmtFE = 0;
                double DouExpInvAmt = 0;
                double DouExpInvAmtFE = 0;

                DouExpInvAmt = Val.Val(txtExpInvAmt.Text);
                DouExpInvAmtFE = Val.Val(txtExpInvAmtFE.Text);

                for (int IntI = 0; IntI < GrdDetail.RowCount; IntI++)
                {
                    DataRow DRow = GrdDetail.GetDataRow(IntI);

                    IntPcs = IntPcs + Val.ToInt(DRow["PCS"]);
                    DouCarat = DouCarat + Val.Val(DRow["CARAT"]);
                }

                lblTotalPcs.Text = Val.Format(IntPcs, "########0");
                lblTotalCarat.Text = Val.Format(DouCarat, "########0.00");

                //if (DouSaleRapAmt != 0)
                //{
                //    lblTotalAvgDisc.Text = Val.Format(((DouSaleAmt - DouSaleRapAmt) / DouSaleRapAmt) * 100, "########0.00"); //#P
                //    //  lblTotalAvgDisc.Text = Val.Format(((DouSaleRapAmt - DouSaleAmt) / DouSaleRapAmt) * 100, "########0.00");
                //}
                //else
                //{
                //    lblTotalAvgDisc.Text = "0.00";
                //}

                lblTotalAmount.Text = Val.Format(DouExpInvAmt, "########0.00");
                lblTotalAvgRate.Text = Val.Format(DouExpInvAmt / DouCarat, "########0.00");

                txtMemoAmount.Text = Val.Format(DouExpInvAmt, "########0.00");
                txtGrossAmount.Text = Val.Format(DouExpInvAmt, "########0.00");
                txtGrossAmountFE.Text = Val.Format(DouExpInvAmtFE, "########0.00");
                txtMemoAvgRate.Text = Val.Format(DouExpInvAmt / DouCarat, "########0.00");


                double DouDiscAmt = Math.Round(Val.Val(txtDiscAmount.Text), 4);
                double DouGSTAmt = Math.Round(Val.Val(txtGSTAmount.Text), 4);
                double DouInsAmt = Math.Round(Val.Val(txtInsuranceAmount.Text), 4);
                double DouShipAmt = Math.Round(Val.Val(txtShippingAmount.Text), 4);

                double DouIGSTAmt = Math.Round(Val.Val(txtIGSTAmount.Text), 4);
                double DouCGSTAmt = Math.Round(Val.Val(txtCGSTAmount.Text), 4);
                double DouSGSTAmt = Math.Round(Val.Val(txtSGSTAmount.Text), 4);

                txtDiscAmount.Text = Val.Format(DouDiscAmt, "########0.000");
                txtInsuranceAmount.Text = Val.Format(DouInsAmt, "########0.000");
                txtShippingAmount.Text = Val.Format(DouShipAmt, "########0.000");
                txtGSTAmount.Text = Val.Format(DouGSTAmt, "########0.000");

                txtIGSTAmount.Text = Val.Format(DouIGSTAmt, "########0.000");
                txtCGSTAmount.Text = Val.Format(DouCGSTAmt, "########0.000");
                txtSGSTAmount.Text = Val.Format(DouSGSTAmt, "########0.000");

                double DouNetAmt = (Val.Val(txtGrossAmount.Text) + DouIGSTAmt + DouCGSTAmt + DouSGSTAmt + DouInsAmt + DouShipAmt + DouGSTAmt) - DouDiscAmt;
                txtNetAmount.Text = Val.Format(DouNetAmt, "########0.000");

                double DouDiscAmtFE = Math.Round(Val.Val(txtDiscAmountFE.Text), 2);
                double DouGSTAmtFE = Math.Round(Val.Val(txtGSTAmountFE.Text), 2);
                double DouInsAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouInsAmt, 2);
                double DouShipAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouShipAmt, 2);

                double DouIGSTAmtFE = 0;
                double DouCGSTAmtFE = 0;
                double DouSGSTAmtFE = 0;

                DouIGSTAmtFE = Math.Round(Val.Val(txtIGSTAmountFE.Text) * 1000, 3);
                DouCGSTAmtFE = Math.Round(Val.Val(txtCGSTAmountFE.Text) * 1000, 3);
                DouSGSTAmtFE = Math.Round(Val.Val(txtSGSTAmountFE.Text) * 1000, 3);

                if (mFormType == FORMTYPE.SALEINVOICE)
                {

                    txtIGSTAmountFE.Text = Val.ToString(Val.Format(DouIGSTAmtFE, "########0.000"));
                    txtCGSTAmountFE.Text = Val.ToString(Val.Format(DouCGSTAmtFE, "########0.000"));
                    txtSGSTAmountFE.Text = Val.ToString(Val.Format(DouSGSTAmtFE, "########0.000"));

                    txtGrossAmountFE.Text = Val.ToString(Val.Format(Val.Val(txtGrossAmountFE.Text), "########0.000"));
                    txtDiscAmountFE.Text = Val.ToString(Val.Format(DouDiscAmtFE, "########0.000"));
                    txtInsuranceAmountFE.Text = Val.ToString(Val.Format(DouInsAmtFE, "########0.000"));
                    txtShippingAmountFE.Text = Val.ToString(Val.Format(DouShipAmtFE, "########0.000"));
                    txtGSTAmountFE.Text = Val.ToString(Val.Format(DouGSTAmtFE, "########0.000"));

                }

                double DouNetAmtFE = (Val.Val(txtGrossAmountFE.Text) + DouIGSTAmtFE + DouCGSTAmtFE + DouSGSTAmtFE + DouInsAmtFE + DouShipAmtFE + DouGSTAmtFE) - DouDiscAmtFE;
                txtNetAmountFE.Text = Val.Format(DouNetAmtFE, "########0.000");

                //Added & Comment by Daksha on 17/04/2023 'Calculate Adat&Broker on GrossAmt : And Less Adat then Calculate Broker
                //Old Code
                //txtBrokerAmtFE.Text = Val.ToString(Math.Round((Val.Val(txtNetAmountFE.Text) * Val.Val(txtBaseBrokeragePer.Text)) / 100, 3));
                //txtAdatAmtFE.Text = Val.ToString(Math.Round((Val.Val(txtNetAmountFE.Text) * Val.Val(txtAdatPer.Text)) / 100, 3));
                //txtStkAmtFE.Text = Val.Format(Val.Val(txtNetAmountFE.Text) - (Val.Val(txtBrokerAmtFE.Text) + Val.Val(txtAdatAmtFE.Text)), "########0.000");

                txtAdatAmtFE.Text = Val.ToString(Math.Round((Val.Val(txtGrossAmountFE.Text) * Val.Val(txtAdatPer.Text)) / 100, 3));
                txtBrokerAmtFE.Text = Val.ToString(Math.Round(((Val.Val(txtGrossAmountFE.Text) - Val.Val(txtAdatAmtFE.Text)) * Val.Val(txtBaseBrokeragePer.Text)) / 100, 3));
                txtStkAmtFE.Text = Val.Format(Val.Val(txtGrossAmountFE.Text) - (Val.Val(txtBrokerAmtFE.Text) + Val.Val(txtAdatAmtFE.Text)), "########0.000");
                //End as Daksha

                txtAdatAmt.Text = Val.ToString(Math.Round(Val.Val(txtAdatAmtFE.Text) / Val.Val(txtExcRate.Text), 3));
                txtBrokerAmt.Text = Val.ToString(Math.Round(Val.Val(txtBrokerAmtFE.Text) / Val.Val(txtExcRate.Text), 3));

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtTermsDays_TextChanged(object sender, EventArgs e)
        {
            DTPTermsDate.Value = DTPMemoDate.Value.AddDays(Val.ToInt(txtTermsDays.Text));
        }

        private void DTPTermsDate_Validated(object sender, EventArgs e)
        {
            txtTermsDays.Text = Val.ToString(Val.DateDiff(Microsoft.VisualBasic.DateInterval.Day, DTPMemoDate.Value.ToShortDateString(), DTPTermsDate.Value.ToShortDateString())); ;
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

        private void txtCurrency_KeyPress(object sender, KeyPressEventArgs e)
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

                        GrdDetail.Columns["FMEMOPRICEPERCARAT"].Caption = "  " + Val.ToString(FrmSearch.DRow["SYMBOL"]) + "/Cts";
                        GrdDetail.Columns["FMEMOAMOUNT"].Caption = "  Amt (" + Val.ToString(FrmSearch.DRow["SYMBOL"]) + ")";

                        txtExcRate.Text = ObjMemo.GetExchangeRate(Val.ToInt(txtCurrency.Tag), Val.SqlDate(DTPMemoDate.Value.ToShortDateString()), "SALEINVOICEBRANCH").ToString();
                        txtOrgExcRate.Text = txtExcRate.Text;
                        txtExcRate_Validated(null, null);
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
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_BROKERADAT);

                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBroker.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtBroker.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
                        //#K : 04-11-2020
                        txtBaseBrokeragePer.Text = Val.ToString(FrmSearch.DRow["BROKERAGEPER"]);
                        if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.SALESDELIVERYRETURN || mFormType == FORMTYPE.PURCHASERETURN || mFormType == FORMTYPE.PURCHASEISSUE)
                            AddAccountingEffect();
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

        private void txtAdat_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_BROKERADAT);

                    FrmSearch.mStrColumnsToHide = "PARTY_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtAdat.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        txtAdat.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
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
            //txtExcRate.Text = ObjMemo.GetExchangeRate(Val.ToInt(txtCurrency.Tag), Val.SqlDate(DTPMemoDate.Value.ToShortDateString())).ToString();
            //txtExcRate_Validated(null, null);
            //this.Cursor = Cursors.Default;
        }


        private void txtExcRate_Validated(object sender, EventArgs e)
        {
            for (int IntI = 0; IntI < GrdDetail.RowCount; IntI++)
            {
                DataRow DRow = GrdDetail.GetDataRow(IntI);
                DRow["FMEMOPRICEPERCARAT"] = Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOPRICEPERCARAT"]);
                if (BOConfiguration.gStrLoginSection == "B")
                {
                    DRow["FMEMOAMOUNT"] = Math.Round((Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOAMOUNT"])) / 1000, 2);
                }
                else
                {
                    DRow["FMEMOAMOUNT"] = Math.Round(Val.Val(txtExcRate.Text) * Val.Val(DRow["MEMOAMOUNT"]), 2);
                }

            }
            DTabMemoDetail.AcceptChanges();
            //Calculation();
            AutoGstCalculation();
            CalculationNew();
        }

        private void txtLocation_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDetail.FocusedRowHandle < 0)
                    return;

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
                        GrdDetail.SetFocusedRowCellValue("LOCATIONNAME", Val.ToString(FrmSearch.DRow["LOCATIONNAME"]));
                        GrdDetail.SetFocusedRowCellValue("LOCATION_ID", Val.ToString(FrmSearch.DRow["LOCATION_ID"]));
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

        private void txtSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDetail.FocusedRowHandle < 0)
                    return;

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "SIZECODE,SIZENAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SIZE);
                    FrmSearch.mStrColumnsToHide = "SIZE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDetail.SetFocusedRowCellValue("SIZENAME", Val.ToString(FrmSearch.DRow["SIZENAME"]));
                        GrdDetail.SetFocusedRowCellValue("SIZE_ID", Val.ToString(FrmSearch.DRow["SIZE_ID"]));
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

        private void txtLab_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (GrdDetail.FocusedRowHandle < 0)
                    return;

                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "LABCODE,LABNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LAB);
                    FrmSearch.mStrColumnsToHide = "LAB_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDetail.SetFocusedRowCellValue("LABNAME", Val.ToString(FrmSearch.DRow["LABNAME"]));
                        GrdDetail.SetFocusedRowCellValue("LAB_ID", Val.ToString(FrmSearch.DRow["LAB_ID"]));
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

        private void BtnStoneDelete_Click(object sender, EventArgs e)
        {

        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {

                if (Global.Confirm("Are You Sure You Want To Delete This " + Val.ToString(lblInvoiceNo.Text) + " : " + Val.ToString(txtJangedNo.Text)) == System.Windows.Forms.DialogResult.Yes)
                {
                    FrmPassword FrmPassword = new FrmPassword();
                    ObjPer.PASSWORD = "123";
                    if (FrmPassword.ShowForm(ObjPer.PASSWORD) == System.Windows.Forms.DialogResult.No)
                    {
                        return;
                    }

                    MemoEntryProperty Property = new MemoEntryProperty();
                    if (BOConfiguration.gStrLoginSection == "B")
                    {
                        Property.MEMO_ID = PstrApprove_MemoId;
                    }
                    else
                    {
                        Property.MEMO_ID = Val.ToString(lblMemoNo.Tag);
                    }


                    if (Property.MEMO_ID == "")
                    {
                        Property = null;
                        Global.Message("No Memo Found For Delete");
                        return;
                    }

                    //DataTable DTab = ObjMemo.ValDelete(Property);
                    //if (DTab.Rows.Count != 0 && BOConfiguration.gStrLoginSection != "B")
                    //{
                    //    Global.Message("Some Stones Are In Other Process\n\n You Can Not Delete");
                    //    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    //    FrmSearch.mStrSearchField = "STOCKNO";
                    //    FrmSearch.mStrSearchText = "";
                    //    this.Cursor = Cursors.WaitCursor;
                    //    FrmSearch.mDTab = DTab;
                    //    FrmSearch.mStrColumnsToHide = "STOCK_ID";
                    //    this.Cursor = Cursors.Default;
                    //    FrmSearch.ShowDialog();
                    //    FrmSearch.Hide();
                    //    FrmSearch.Dispose();
                    //    FrmSearch = null;

                    //    DTab.Dispose();
                    //    DTab = null;

                    //    return;
                    //}
                    //else if (DTab.Rows.Count == 0 && BOConfiguration.gStrLoginSection == "B")
                    //{
                    //    Global.Message("A Part Entry Is In Other Process\n\n You Can Not Delete");
                    //    DTab.Dispose();
                    //    DTab = null;

                    //    return;
                    //}

                    //DTab.Dispose();
                    //DTab = null;

                    this.Cursor = Cursors.WaitCursor;
                    Property = ObjMemo.DeleteBranchRcv(Property);
                    this.Cursor = Cursors.Default;

                    Global.Message(Property.ReturnMessageDesc);
                    if (Property.ReturnMessageType == "SUCCESS")
                    {
                        BtnClear_Click(null, null);
                        this.Close();
                    }
                    Property = null;

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
                if (Val.ToString(lblMemoNo.Tag) == "")
                {
                    Global.Message("No Memo Found");
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                DataTable DTab = ObjMemo.Print(Val.ToString(lblMemoNo.Tag), "USD");
                if (DTab.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("There Is No Data Found For Print");
                    return;
                }

                DataSet DS = new DataSet();
                DTab.TableName = "Table";
                DS.Tables.Add(DTab);
                DataTable DTabDuplicate = DTab.Copy();
                DTabDuplicate.TableName = "Table1";
                foreach (DataRow DRow in DTabDuplicate.Rows)
                {
                    DRow["PRINTTYPE"] = "DUBLICATE";
                }
                DTabDuplicate.AcceptChanges();
                DS.Tables.Add(DTabDuplicate);

                Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                FrmReportViewer.MdiParent = Global.gMainRef;
                FrmReportViewer.ShowFormMemoPrint("MemoPrint", DS);
                this.Cursor = Cursors.Default;
            }
            catch (Exception EX)
            {
                this.Cursor = Cursors.Default;
                Global.Message(EX.Message);
            }




            //if (ChkNoBackPrint.Checked)
            //{
            //    Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
            //    FrmReportViewer.MdiParent = Global.gMainRef;
            //    FrmReportViewer.ShowForm("InvoicePrintWithoutBack", DTab);
            //    this.Cursor = Cursors.Default;
            //}
            //else
            //{
            //    Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
            //    FrmReportViewer.MdiParent = Global.gMainRef;
            //    FrmReportViewer.ShowForm("InvoicePrintWithBack", DTab);
            //    this.Cursor = Cursors.Default;
            //}
        }

        private void GrdDetail_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.RowHandle < 0)
            {
                return;
            }
            DataRow DRow = GrdDetail.GetDataRow(e.RowHandle);

            switch (e.Column.FieldName.ToUpper())
            {
                case "PCS":
                    if (Val.ToString(DRow["STOCKTYPE"]) == "PARCEL")
                    {
                        int IntBalPcs = Val.ToInt(DRow["BALANCEPCS"]);
                        double DouBalCarat = Val.Val(DRow["BALANCECARAT"]);

                        int IntPcs = Val.ToInt(DRow["PCS"]);

                        double DouSize = DouBalCarat / IntBalPcs;

                        DTabMemoDetail.Rows[e.RowHandle]["CARAT"] = Math.Round(IntPcs * DouSize, 2);
                        FindRap(DRow, e.RowHandle);
                    }

                    break;
                case "BALANCEPCS":
                case "BALANCECARAT":
                    DTabMemoDetail.Rows[e.RowHandle]["PCS"] = DTabMemoDetail.Rows[e.RowHandle]["BALANCEPCS"];
                    DTabMemoDetail.Rows[e.RowHandle]["CARAT"] = DTabMemoDetail.Rows[e.RowHandle]["BALANCECARAT"];
                    FindRap(DRow, e.RowHandle);
                    break;

                case "SHAPENAME":
                case "COLORNAME":
                case "CLARITYNAME":
                case "CUTNAME":
                case "POLNAME":
                case "SYMNAME":
                case "CARAT":

                    FindRap(DRow, e.RowHandle);

                    break;

                case "SALEDISCOUNT":
                case "SALERAPAPORT":
                    double DouCarat = 0;
                    double DouRapaport = 0;
                    double DouDiscount = 0;
                    double DouPricePerCarat = 0;
                    double DouAmount = 0;

                    DouCarat = Val.Val(DRow["CARAT"]);
                    DouRapaport = Val.Val(DRow["SALERAPAPORT"]);
                    DouDiscount = Val.Val(DRow["SALEDISCOUNT"]);

                    if (DouRapaport != 0)
                        //DouPricePerCarat = Math.Round(DouRapaport + ((DouRapaport * DouDiscount) / 100), 2); 
                        DouPricePerCarat = Math.Round(DouRapaport - ((DouRapaport * DouDiscount) / 100), 2); //#P : 23-04-2021
                    else
                        DouPricePerCarat = 0;

                    DouAmount = Math.Round(DouPricePerCarat * DouCarat, 2);

                    DTabMemoDetail.Rows[e.RowHandle]["SALEPRICEPERCARAT"] = DouPricePerCarat;
                    DTabMemoDetail.Rows[e.RowHandle]["SALEAMOUNT"] = DouAmount;
                    DTabMemoDetail.AcceptChanges();
                    GrdDetail.PostEditor();
                    MainGrdDetail.Refresh();

                    break;

                case "SALEPRICEPERCARAT":
                    DouCarat = Val.Val(DRow["CARAT"]);
                    DouRapaport = Val.Val(DRow["SALERAPAPORT"]);
                    DouPricePerCarat = Val.Val(DRow["SALEPRICEPERCARAT"]);
                    DouAmount = Math.Round(DouPricePerCarat * DouCarat, 2);

                    if (DouRapaport != 0)
                        //DouDiscount = Math.Round(((DouPricePerCarat - DouRapaport) / DouRapaport) * 100, 2);
                        DouDiscount = Math.Round(((DouRapaport - DouPricePerCarat) / DouRapaport) * 100, 2);  //#P:23-04-2021
                    else
                        DouDiscount = 0;

                    DTabMemoDetail.Rows[e.RowHandle]["SALEDISCOUNT"] = DouDiscount;
                    DTabMemoDetail.Rows[e.RowHandle]["SALEAMOUNT"] = DouAmount;
                    DTabMemoDetail.AcceptChanges();
                    GrdDetail.PostEditor();
                    break;

                /* //#P : 23-04-2021
            case "MEMORAPAPORT":
            case "MEMODISCOUNT":
                DouCarat = Val.Val(DRow["CARAT"]);
                DouRapaport = Val.Val(DRow["SALERAPAPORT"]);
                DouDiscount = Val.Val(DRow["MEMODISCOUNT"]);

                if (DouRapaport != 0)
                    //DouPricePerCarat = Math.Round(DouRapaport + ((DouRapaport * DouDiscount) / 100), 2);
                    DouPricePerCarat = Math.Round(DouRapaport - ((DouRapaport * DouDiscount) / 100), 2);   //#P:23-04-2021
                else
                    DouPricePerCarat = Val.Val(DRow["SALEPRICEPERCARAT"]);

                DouAmount = Math.Round(DouPricePerCarat * DouCarat, 2);

                DTabMemoDetail.Rows[e.RowHandle]["MEMOPRICEPERCARAT"] = DouPricePerCarat;
                DTabMemoDetail.Rows[e.RowHandle]["MEMOAMOUNT"] = DouAmount;
                DTabMemoDetail.Rows[e.RowHandle]["FMEMOPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(txtExcRate.Text), 2);
                DTabMemoDetail.Rows[e.RowHandle]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(txtExcRate.Text), 2);
                DTabMemoDetail.AcceptChanges();
                GrdDetail.PostEditor();
                MainGrdDetail.Refresh();

                break;

            case "MEMOPRICEPERCARAT":
                DouCarat = Val.Val(DRow["CARAT"]);
                DouRapaport = Val.Val(DRow["SALERAPAPORT"]);
                DouPricePerCarat = Val.Val(DRow["MEMOPRICEPERCARAT"]);
                DouAmount = Math.Round(DouPricePerCarat * DouCarat, 2);

                if (DouRapaport != 0)
                    //DouDiscount = Math.Round(((DouPricePerCarat - DouRapaport) / DouRapaport) * 100, 2);
                      DouDiscount = Math.Round(((DouRapaport - DouPricePerCarat) / DouRapaport) * 100, 2);  //#P:23-04-2021
                else
                    DouDiscount = 0;

                DTabMemoDetail.Rows[e.RowHandle]["MEMODISCOUNT"] = DouDiscount;
                DTabMemoDetail.Rows[e.RowHandle]["MEMOAMOUNT"] = DouAmount;
                DTabMemoDetail.Rows[e.RowHandle]["FMEMOPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(txtExcRate.Text), 2);
                DTabMemoDetail.Rows[e.RowHandle]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(txtExcRate.Text), 2);
                DTabMemoDetail.AcceptChanges();
                GrdDetail.PostEditor();
                break;
                  */
                //Cmnt : #P : 23-04-2021  

                case "JANGEDDISCOUNT":
                    DouCarat = 0;
                    DouRapaport = 0;
                    DouDiscount = 0;
                    DouPricePerCarat = 0;
                    DouAmount = 0;
                    DouCarat = Val.Val(DRow["CARAT"]);
                    DouRapaport = Val.Val(DRow["SALERAPAPORT"]);
                    DouDiscount = Val.Val(DRow["JANGEDDISCOUNT"]);

                    if (DouRapaport != 0)
                        DouPricePerCarat = Math.Round(DouRapaport - ((DouRapaport * DouDiscount) / 100), 2); //#P : 23-04-2021
                    else
                        DouPricePerCarat = 0;

                    DouAmount = Math.Round(DouPricePerCarat * DouCarat, 2);

                    DTabMemoDetail.Rows[e.RowHandle]["JANGEDRAPAPORT"] = DouRapaport;
                    DTabMemoDetail.Rows[e.RowHandle]["JANGEDPRICEPERCARAT"] = DouPricePerCarat;
                    DTabMemoDetail.Rows[e.RowHandle]["JANGEDAMOUNT"] = DouAmount;

                    DTabMemoDetail.Rows[e.RowHandle]["MEMODISCOUNT"] = DouDiscount;
                    DTabMemoDetail.Rows[e.RowHandle]["MEMOPRICEPERCARAT"] = DouPricePerCarat;
                    DTabMemoDetail.Rows[e.RowHandle]["MEMOAMOUNT"] = DouAmount;

                    DTabMemoDetail.Rows[e.RowHandle]["FMEMOPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(txtExcRate.Text), 2);
                    if (BOConfiguration.gStrLoginSection == "B")
                    {
                        DTabMemoDetail.Rows[e.RowHandle]["FMEMOAMOUNT"] = Math.Round((DouAmount * Val.Val(txtExcRate.Text)) / 1000, 2);
                    }
                    else
                    {
                        DTabMemoDetail.Rows[e.RowHandle]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(txtExcRate.Text), 2);
                    }

                    DTabMemoDetail.Rows[e.RowHandle]["OLDMEMODISCOUNT"] = DouDiscount;
                    DTabMemoDetail.Rows[e.RowHandle]["OLDMEMOPRICEPERCARAT"] = DouPricePerCarat;

                    DTabMemoDetail.AcceptChanges();


                    GrdDetail.PostEditor();
                    MainGrdDetail.Refresh();
                    break;

                case "JANGEDPRICEPERCARAT":
                    DouCarat = Val.Val(DRow["CARAT"]);
                    DouRapaport = Val.Val(DRow["SALERAPAPORT"]);
                    DouPricePerCarat = Val.Val(DRow["JANGEDPRICEPERCARAT"]);
                    DouAmount = Math.Round(DouPricePerCarat * DouCarat, 2);

                    if (DouRapaport != 0)
                        //DouDiscount = Math.Round(((DouPricePerCarat - DouRapaport) / DouRapaport) * 100, 2);
                        DouDiscount = Math.Round(((DouRapaport - DouPricePerCarat) / DouRapaport) * 100, 2);  //#P:23-04-2021
                    else
                        DouDiscount = 0;

                    DTabMemoDetail.Rows[e.RowHandle]["JANGEDRAPAPORT"] = DouRapaport;
                    DTabMemoDetail.Rows[e.RowHandle]["JANGEDDISCOUNT"] = DouDiscount;
                    DTabMemoDetail.Rows[e.RowHandle]["JANGEDAMOUNT"] = DouAmount;

                    DTabMemoDetail.Rows[e.RowHandle]["OLDMEMODISCOUNT"] = DouDiscount;
                    DTabMemoDetail.Rows[e.RowHandle]["OLDMEMOPRICEPERCARAT"] = DouPricePerCarat;

                    DTabMemoDetail.Rows[e.RowHandle]["MEMODISCOUNT"] = DouDiscount;
                    DTabMemoDetail.Rows[e.RowHandle]["MEMOAMOUNT"] = DouAmount;
                    DTabMemoDetail.Rows[e.RowHandle]["MEMOPRICEPERCARAT"] = DouPricePerCarat;
                    DTabMemoDetail.Rows[e.RowHandle]["FMEMOPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(txtExcRate.Text), 2);
                    if (BOConfiguration.gStrLoginSection == "B")
                    {
                        DTabMemoDetail.Rows[e.RowHandle]["FMEMOAMOUNT"] = Math.Round((DouAmount * Val.Val(txtExcRate.Text)) / 1000, 2);
                    }
                    else
                    {
                        DTabMemoDetail.Rows[e.RowHandle]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(txtExcRate.Text), 2);
                    }

                    DTabMemoDetail.AcceptChanges();
                    GrdDetail.PostEditor();
                    break;

                case "EXPINVOICERATE":
                    DouCarat = Val.Val(DRow["CARAT"]);
                    DouPricePerCarat = Val.Val(DRow["EXPINVOICERATE"]);
                    DouAmount = Math.Round(DouPricePerCarat * DouCarat, 2);

                    DTabMemoDetail.Rows[e.RowHandle]["EXPINVOICEAMT"] = DouAmount;
                    DTabMemoDetail.Rows[e.RowHandle]["EXPINVOICEAMTFE"] = Math.Round(DouAmount * Val.Val(txtExcRate.Text), 2);
                    DTabMemoDetail.Rows[e.RowHandle]["EXPINVOICERATEFE"] = Math.Round(Val.Val(DTabMemoDetail.Rows[e.RowHandle]["EXPINVOICEAMTFE"]) / DouCarat, 4);
                    break;


                default:
                    break;
            }
            //Calculation();

            if (Val.Val(txtBackAddLess.Text) != 0 && (e.Column.FieldName.ToUpper() == "JANGEDDISCOUNT" || e.Column.FieldName.ToUpper() == "JANGEDPRICEPERCARAT"))
            {
                txtBackAddLess.Text = "0";
                BtnModifyPrice_Click(null, null);
            }
            else
            {
                CalculationNew();
                GetSummuryDetailForGrid();
            }
        }

        private void txtMemoAmount_Validated(object sender, EventArgs e)
        {
            double DouAmount = Val.Val(lblTotalAmount.Text);

            foreach (DataRow DRow in DTabMemoDetail.Rows)
            {
                DRow["PER"] = Math.Round((Val.Val(DRow["SALEAMOUNT"]) / DouAmount) * 100, 5);
            }

            double DouMemoAmount = Val.Val(txtMemoAmount.Text);



            foreach (DataRow DRow in DTabMemoDetail.Rows)
            {
                DRow["MEMOAMOUNT"] = Math.Round((DouMemoAmount * Val.Val(DRow["PER"])) / 100, 2);
                DRow["MEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["MEMOAMOUNT"]) / Val.Val(DRow["CARAT"]), 2);

                if (Val.Val(DRow["SALERAPAPORT"]) != 0)
                    //DRow["MEMODISCOUNT"] = Math.Round(((Val.Val(DRow["MEMOPRICEPERCARAT"]) - Val.Val(DRow["SALERAPAPORT"])) / Val.Val(DRow["SALERAPAPORT"])) * 100, 2);
                    DRow["MEMODISCOUNT"] = Math.Round(((Val.Val(DRow["SALERAPAPORT"]) - Val.Val(DRow["MEMOPRICEPERCARAT"])) / Val.Val(DRow["SALERAPAPORT"])) * 100, 2); //#P:23-04-2021
                else
                    DRow["MEMODISCOUNT"] = 0;

                if (BOConfiguration.gStrLoginSection == "B")
                {
                    DRow["FMEMOAMOUNT"] = Math.Round((Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text)) / 1000, 2);
                }
                else
                {
                    DRow["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text), 2);
                }
                DRow["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtExcRate.Text), 2);

            }
            DTabMemoDetail.AcceptChanges();
            //Calculation();
            CalculationNew();
            GetSummuryDetailForGrid();

        }

        private void txtMemoAvgRate_Validated(object sender, EventArgs e)
        {
            double DouNewAvgRate = Val.Val(txtMemoAvgRate.Text);
            double DouSaleAvgRate = Val.Val(lblTotalAvgRate.Text);

            foreach (DataRow DRow in DTabMemoDetail.Rows)
            {
                DRow["PER"] = Math.Round((100 * (DouNewAvgRate - DouSaleAvgRate)) / DouSaleAvgRate, 5);
            }

            double DouMemoAmount = Val.Val(txtMemoAmount.Text);

            if (mFormType == FORMTYPE.PURCHASEISSUE)
            {
                foreach (DataRow DRow in DTabMemoDetail.Rows)
                {
                    DRow["SALEPRICEPERCARAT"] = Math.Round(Val.Val(DRow["SALEPRICEPERCARAT"]) + ((Val.Val(DRow["SALEPRICEPERCARAT"]) * Val.Val(DRow["PER"])) / 100), 2);
                    DRow["SALEAMOUNT"] = Math.Round(Val.Val(DRow["SALEPRICEPERCARAT"]) * Val.Val(DRow["CARAT"]), 2);

                    if (Val.Val(DRow["SALERAPAPORT"]) != 0)
                        //DRow["SALEDISCOUNT"] = Math.Round(((Val.Val(DRow["SALEPRICEPERCARAT"]) - Val.Val(DRow["SALERAPAPORT"])) / Val.Val(DRow["SALERAPAPORT"])) * 100, 2);
                        DRow["SALEDISCOUNT"] = Math.Round(((Val.Val(DRow["SALERAPAPORT"]) - Val.Val(DRow["SALEPRICEPERCARAT"])) / Val.Val(DRow["SALERAPAPORT"])) * 100, 2);  //#P:23-04-2021
                    else
                        DRow["SALEDISCOUNT"] = 0;

                    if (BOConfiguration.gStrLoginSection == "B")
                    {
                        DRow["FMEMOAMOUNT"] = Math.Round((Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text)) / 1000, 2);
                    }
                    else
                    {
                        DRow["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text), 2);
                    }
                    DRow["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtExcRate.Text), 2);
                }
            }
            else
            {
                foreach (DataRow DRow in DTabMemoDetail.Rows)
                {
                    DRow["MEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["SALEPRICEPERCARAT"]) + ((Val.Val(DRow["SALEPRICEPERCARAT"]) * Val.Val(DRow["PER"])) / 100), 2);
                    DRow["MEMOAMOUNT"] = Math.Round(Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(DRow["CARAT"]), 2);

                    if (Val.Val(DRow["SALERAPAPORT"]) != 0)
                        //DRow["MEMODISCOUNT"] = Math.Round(((Val.Val(DRow["MEMOPRICEPERCARAT"]) - Val.Val(DRow["SALERAPAPORT"])) / Val.Val(DRow["SALERAPAPORT"])) * 100, 2);
                        DRow["MEMODISCOUNT"] = Math.Round(((Val.Val(DRow["SALERAPAPORT"]) - Val.Val(DRow["MEMOPRICEPERCARAT"])) / Val.Val(DRow["SALERAPAPORT"])) * 100, 2); //#P:23-04-2021

                    else
                        DRow["MEMODISCOUNT"] = 0;

                    if (BOConfiguration.gStrLoginSection == "B")
                    {
                        DRow["FMEMOAMOUNT"] = Math.Round((Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text)) / 1000, 2);
                    }
                    else
                    {
                        DRow["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text), 2);
                    }
                    DRow["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtExcRate.Text), 2);

                }
            }
            DTabMemoDetail.AcceptChanges();
            CalculationNew();
            GetSummuryDetailForGrid();
        }

        private void txtMemoAvgDisc_Validated(object sender, EventArgs e)
        {
            double DouNewAvgDisc = Val.Val(txtMemoAvgDisc.Text);
            double DouSaleAvgDisc = Val.Val(lblTotalAvgDisc.Text);

            foreach (DataRow DRow in DTabMemoDetail.Rows)
            {
                DRow["PER"] = Math.Round(DouNewAvgDisc - DouSaleAvgDisc, 5);
            }

            if (mFormType == FORMTYPE.PURCHASEISSUE)
            {
                foreach (DataRow DRow in DTabMemoDetail.Rows)
                {
                    DRow["SALEDISCOUNT"] = Math.Round(Val.Val(DRow["SALEDISCOUNT"]) + Val.Val(DRow["PER"]), 2);
                    //DRow["SALEPRICEPERCARAT"] = Math.Round(Val.Val(DRow["SALERAPAPORT"]) + ((Val.Val(DRow["SALERAPAPORT"]) * Val.Val(DRow["SALEDISCOUNT"])) / 100), 2); 
                    DRow["SALEPRICEPERCARAT"] = Math.Round(Val.Val(DRow["SALERAPAPORT"]) - ((Val.Val(DRow["SALERAPAPORT"]) * Val.Val(DRow["SALEDISCOUNT"])) / 100), 2); //#P:23-04-2021
                    DRow["SALEAMOUNT"] = Math.Round(Val.Val(DRow["SALEPRICEPERCARAT"]) * Val.Val(DRow["CARAT"]), 2);

                    //DRow["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["SALEAMOUNT"]) * Val.Val(txtExcRate.Text), 2);
                    //DRow["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["SALEPRICEPERCARAT"]) * Val.Val(txtExcRate.Text), 2);

                    DRow["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text), 2);
                    DRow["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtExcRate.Text), 2);
                }
            }
            else
            {
                foreach (DataRow DRow in DTabMemoDetail.Rows)
                {
                    DRow["MEMODISCOUNT"] = Math.Round(Val.Val(DRow["SALEDISCOUNT"]) + Val.Val(DRow["PER"]), 2);
                    //DRow["MEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["SALERAPAPORT"]) + ((Val.Val(DRow["SALERAPAPORT"]) * Val.Val(DRow["MEMODISCOUNT"])) / 100), 2); 
                    DRow["MEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["SALERAPAPORT"]) - ((Val.Val(DRow["SALERAPAPORT"]) * Val.Val(DRow["MEMODISCOUNT"])) / 100), 2); //#P:23-04-2021
                    DRow["MEMOAMOUNT"] = Math.Round(Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(DRow["CARAT"]), 2);

                    if (BOConfiguration.gStrLoginSection == "B")
                    {
                        DRow["FMEMOAMOUNT"] = Math.Round((Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text)) / 1000, 2);
                    }
                    else
                    {
                        DRow["FMEMOAMOUNT"] = Math.Round(Val.Val(DRow["MEMOAMOUNT"]) * Val.Val(txtExcRate.Text), 2);
                    }
                    DRow["FMEMOPRICEPERCARAT"] = Math.Round(Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtExcRate.Text), 2);
                }
            }
            DTabMemoDetail.AcceptChanges();
            //Calculation();
            CalculationNew();
            GetSummuryDetailForGrid();
        }

        private void BtnNewRow_Click(object sender, EventArgs e)
        {
            if (mFormType == FORMTYPE.PURCHASEISSUE)
            {
                DataRow drow = DTabMemoDetail.NewRow();

                //HINA - START
                //drow["STOCKTYPE"] = "SINGLE";
                drow["STOCKTYPE"] = mStrStockType == "PARCEL" ? "PARCEL" : "SINGLE";
                //HINA - END
                DTabMemoDetail.Rows.Add(drow);

                GrdDetail.FocusedColumn = GrdDetail.VisibleColumns[1];
                GrdDetail.FocusedRowHandle = drow.Table.Rows.IndexOf(drow);
                GrdDetail.Focus();
            }
            else
            {
                DTabMemoDetail.Rows.Add(DTabMemoDetail.NewRow());
            }
        }

        private void BtnEmail_Click(object sender, EventArgs e)
        {
            try
            {
                if (Val.ToString(lblMemoNo.Tag) == "")
                {
                    Global.Message("No Memo Found");
                    return;
                }
                this.Cursor = Cursors.WaitCursor;

                //string Str = new Class.CommonEmail().MemoSendEmail(Val.ToString(lblMemoNo.Tag));
                string Str = new CommonEmail().MemoSendEmail(Val.ToString(lblMemoNo.Tag));
                Global.Message(Str);

                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                this.Cursor = Cursors.Default;
                Global.Message(ex.Message.ToString());
            }
        }

        public void FindRap(DataRow Dr, int pIntRowIndex)
        {
            double CostDiscount = 0;
            double CostPricePerCarat = 0;
            double CostAmount = 0;
            double Carat = 0;

            double SaleDiscount = 0;
            double SalePricePerCarat = 0;
            double SaleAmount = 0;

            CostDiscount = Val.Val(Dr["SALEDISCOUNT"]);
            SaleDiscount = Val.Val(Dr["MEMODISCOUNT"]);
            Carat = Val.Val(Dr["CARAT"]);

            var drShape = (from DrPara in DtabPara.AsEnumerable()
                           where Val.ToString(DrPara["PARANAME"]).ToUpper() == Val.ToString(Dr["SHAPENAME"]).ToUpper()
                           select DrPara);

            string StrShape = drShape.Count() > 0 ? Val.ToString(drShape.FirstOrDefault()["RAPAVALUE"]) : "";

            Trn_RapSaveProperty Property = new Trn_RapSaveProperty();

            Property.SHAPE_ID = Val.ToString(Dr["SHAPENAME"]) != "" ? Val.ToInt(Dr["SHAPE_ID"]) : 0;
            Property.COLOR_ID = Val.ToString(Dr["COLORNAME"]) != "" ? Val.ToInt(Dr["COLOR_ID"]) : 0;
            Property.CLARITY_ID = Val.ToString(Dr["CLARITYNAME"]) != "" ? Val.ToInt(Dr["CLARITY_ID"]) : 0;

            Property.CUT_ID = Val.ToString(Dr["CUTNAME"]) != "" ? Val.ToInt(Dr["CUT_ID"]) : 0;
            Property.POL_ID = Val.ToString(Dr["POLNAME"]) != "" ? Val.ToInt(Dr["POL_ID"]) : 0;
            Property.SYM_ID = Val.ToString(Dr["SYMNAME"]) != "" ? Val.ToInt(Dr["SYM_ID"]) : 0;
            Property.FL_ID = Val.ToString(Dr["FLNAME"]) != "" ? Val.ToInt(Dr["FL_ID"]) : 0;
            Property.CARAT = Val.Val(Dr["CARAT"]);
            //Property.DISCOUNT = Val.Val(Dr["SALEDISCOUNT"]);
            Property = new BOFindRap().FindRap(Property);

            if (Property.RAPAPORT != 0)
            {
                CostPricePerCarat = Property.RAPAPORT + (Property.RAPAPORT * CostDiscount / 100);
                CostAmount = CostPricePerCarat * Carat;

                SalePricePerCarat = Property.RAPAPORT + (Property.RAPAPORT * SaleDiscount / 100);
                SaleAmount = SalePricePerCarat * Carat;

                DTabMemoDetail.Rows[pIntRowIndex]["SALERAPAPORT"] = Property.RAPAPORT;
                DTabMemoDetail.Rows[pIntRowIndex]["SALEPRICEPERCARAT"] = CostPricePerCarat;
                DTabMemoDetail.Rows[pIntRowIndex]["SALEAMOUNT"] = CostAmount;

                DTabMemoDetail.Rows[pIntRowIndex]["MEMORAPAPORT"] = Property.RAPAPORT;
                DTabMemoDetail.Rows[pIntRowIndex]["MEMOPRICEPERCARAT"] = SalePricePerCarat;
                DTabMemoDetail.Rows[pIntRowIndex]["MEMOAMOUNT"] = SaleAmount;
                DTabMemoDetail.Rows[pIntRowIndex]["FMEMOPRICEPERCARAT"] = Math.Round(SalePricePerCarat * Val.Val(txtExcRate.Text), 2);
                if (BOConfiguration.gStrLoginSection == "B")
                {
                    DTabMemoDetail.Rows[pIntRowIndex]["FMEMOAMOUNT"] = Math.Round((SaleAmount * Val.Val(txtExcRate.Text)) / 1000, 2);
                }
                else
                {
                    DTabMemoDetail.Rows[pIntRowIndex]["FMEMOAMOUNT"] = Math.Round(SaleAmount * Val.Val(txtExcRate.Text), 2);
                }

                DTabMemoDetail.AcceptChanges();
            }
            else //#p : 13-01-2020 : Coz When RapValue Not Found Its Not Calculate properly
            {
                double DouCarat = Val.Val(Dr["CARAT"]);
                double DouRapaport = Val.Val(Dr["SALERAPAPORT"]);
                double DouDiscount = Val.Val(Dr["SALEDISCOUNT"]);
                double DouPricePerCarat = Math.Round(DouRapaport + ((DouRapaport * DouDiscount) / 100), 2);
                double DouAmount = Math.Round(DouPricePerCarat * DouCarat, 2);

                CostPricePerCarat = Val.Val(Dr["SALEPRICEPERCARAT"]);
                CostAmount = CostPricePerCarat * Carat;

                Property.RAPAPORT = Val.Val(Dr["MEMORAPAPORT"]);
                SalePricePerCarat = Val.Val(Dr["MEMOPRICEPERCARAT"]);
                SaleAmount = SalePricePerCarat * Carat;

                DTabMemoDetail.Rows[pIntRowIndex]["SALERAPAPORT"] = Property.RAPAPORT;
                DTabMemoDetail.Rows[pIntRowIndex]["SALEPRICEPERCARAT"] = CostPricePerCarat;
                DTabMemoDetail.Rows[pIntRowIndex]["SALEAMOUNT"] = CostAmount;

                DTabMemoDetail.Rows[pIntRowIndex]["MEMORAPAPORT"] = Property.RAPAPORT;
                DTabMemoDetail.Rows[pIntRowIndex]["MEMOPRICEPERCARAT"] = SalePricePerCarat;
                DTabMemoDetail.Rows[pIntRowIndex]["MEMOAMOUNT"] = SaleAmount;
                DTabMemoDetail.Rows[pIntRowIndex]["FMEMOPRICEPERCARAT"] = Math.Round(SalePricePerCarat * Val.Val(txtExcRate.Text), 2);
                if (BOConfiguration.gStrLoginSection == "B")
                {
                    DTabMemoDetail.Rows[pIntRowIndex]["FMEMOAMOUNT"] = Math.Round((SaleAmount * Val.Val(txtExcRate.Text)) / 1000, 2);
                }
                else
                {
                    DTabMemoDetail.Rows[pIntRowIndex]["FMEMOAMOUNT"] = Math.Round(SaleAmount * Val.Val(txtExcRate.Text), 2);
                }

                DTabMemoDetail.AcceptChanges();
            }

            //Calculation();
            CalculationNew();
        }

        private void BtnReturn_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtBillingParty.Text.Length == 0)
                {
                    Global.Message("Billing Party Is Required");
                    txtBillingParty.Focus();
                    return;
                }
                if (txtBCountry.Text.Length == 0)
                {
                    Global.Message("Billing Country Is Required");
                    txtBCountry.Focus();
                    return;
                }
                if (txtSellerName.Text.Length == 0)
                {
                    Global.Message("Seller Name Is Required");
                    txtSellerName.Focus();
                    return;
                }
                if (txtTerms.Text.Length == 0)
                {
                    Global.Message("Terms Is Required");
                    txtTerms.Focus();
                    return;
                }

                //#P : 06-10-2019
                DataTable DtReturnStoneList = GetTableOfSelectedRows(GrdDetail, true);
                if (DtReturnStoneList.Rows.Count <= 0)
                {
                    Global.Message("Please Select Records That You Want To Return..");
                    return;
                }

                this.Cursor = Cursors.WaitCursor;
                var SelectedPartyStoneNo = DtReturnStoneList.AsEnumerable().Select(s => s.Field<string>("PARTYSTOCKNO")).ToArray();
                string StrPartyStoneNoList = string.Join(",", SelectedPartyStoneNo);

                LiveStockProperty LStockProperty = new LiveStockProperty();
                LStockProperty.STOCKNO = Val.ToString(StrPartyStoneNoList);
                LStockProperty.STOCKTYPE = "All";
                //DataSet DsLiveStock = new BOTRN_StockUpload().GetLiveStockData(LStockProperty); //Cmnt : Pinali : 18-11-2019 Coz Cntain Pagination
                DataSet DsLiveStock = new BOTRN_StockUpload().GetStoneDetailForMemoForm(LStockProperty);

                DataTable DtabInvoiceDetail = new DataTable();
                DtabInvoiceDetail = DsLiveStock.Tables[0];

                if (DtabInvoiceDetail.Rows.Count > 0)
                {
                    this.Cursor = Cursors.Default;
                    FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                    FrmMemoEntry.MdiParent = Global.gMainRef;
                    FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                    //FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.ORDERCONFIRMRETURN, DtabInvoiceDetail);

                    if (mFormType == FORMTYPE.PURCHASEISSUE)
                        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.PURCHASERETURN, DtabInvoiceDetail, mStrStockType, Val.ToString(lblMemoNo.Tag));
                    else if (mFormType == FORMTYPE.MEMOISSUE)
                        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.MEMORETURN, DtabInvoiceDetail, mStrStockType, Val.ToString(lblMemoNo.Tag));
                    else if (mFormType == FORMTYPE.ORDERCONFIRM)
                        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.ORDERCONFIRMRETURN, DtabInvoiceDetail, mStrStockType, Val.ToString(lblMemoNo.Tag));
                    else if (mFormType == FORMTYPE.SALEINVOICE)
                        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.SALESDELIVERYRETURN, DtabInvoiceDetail, mStrStockType, Val.ToString(lblMemoNo.Tag));
                    else if (mFormType == FORMTYPE.LABISSUE)
                        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.LABRETURN, DtabInvoiceDetail, mStrStockType, Val.ToString(lblMemoNo.Tag));
                    else if (mFormType == FORMTYPE.CONSIGNMENTISSUE)
                        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.CONSIGNMENTRETURN, DtabInvoiceDetail, mStrStockType, Val.ToString(lblMemoNo.Tag));
                }
                this.Cursor = Cursors.Default;

                // End : #P : 06-10-2019


                ////Cmnt : Pinali : 09-10-2019
                /*

                if (Global.Confirm("Are You Sure For Goods Return ") == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }

                ////#P : Calculation Of Selected Stone's
                //int IntPcs = 0;
                //double DouCarat = 0;
                //double DouSaleRapAmt = 0;
                //double DouSaleAmt = 0;
                //double DouMemoTotalAmt = 0;
                //double DouMemoTotalAmtFE = 0;

                //int DouRetTotalPcs = 0;
                //double DouRetTotalCarat = 0;
                //double DouRetMemoAvgDisc = 0;
                //double DouRetMemoAvgRate = 0;
                //double DouRetMemoAmount = 0;

                //double DouRetGrossAmount = 0;
                //double DouRetGrossAmountFE = 0;


                //for (int IntJ = 0; IntJ < DtReturn.Rows.Count; IntJ++)
                //{
                //    DataRow DRow = DtReturn.Rows[IntJ];

                //    IntPcs = IntPcs + Val.ToInt(DRow["PCS"]);
                //    DouCarat = DouCarat + Val.Val(DRow["CARAT"]);

                //    DouSaleRapAmt = DouSaleRapAmt + Val.Val(DRow["SALERAPAPORT"]) * Val.Val(DRow["CARAT"]);

                //    DouSaleAmt = DouSaleAmt + Val.Val(DRow["SALEAMOUNT"]);
                //    DouMemoTotalAmt = DouMemoTotalAmt + Val.Val(DRow["MEMOAMOUNT"]);
                //    DouMemoTotalAmtFE = DouMemoTotalAmtFE + Val.Val(DRow["FMEMOAMOUNT"]);
                //}

                //DouRetTotalPcs = Val.ToInt32(IntPcs);
                //DouRetTotalCarat = Val.Val(DouCarat);

                //lblTotalAmount.Text = Val.Format(DouSaleAmt, "########0.00");
                //lblTotalAvgRate.Text = Val.Format(DouSaleAmt / DouCarat, "########0.00");

                //if (DouSaleRapAmt != 0)
                //{
                //    DouRetMemoAvgDisc = Val.Val((DouMemoTotalAmt - DouSaleRapAmt) / DouSaleRapAmt) * 100;
                //    //txtMemoAvgDisc.Text = Val.Format(((DouMemoTotalAmt - DouSaleRapAmt) / DouSaleRapAmt) * 100, "########0.00"); ;
                //}
                //else
                //{
                //    DouRetMemoAvgDisc = 0;
                //    //txtMemoAvgDisc.Text = "0.00";
                //}

                //DouRetMemoAmount = Val.Val(DouMemoTotalAmt);
                //DouRetGrossAmount = Val.Val(DouMemoTotalAmt);

                //DouRetGrossAmountFE = Val.Val(DouMemoTotalAmtFE);
                //DouRetMemoAvgRate = Val.Val(DouMemoTotalAmt / DouCarat);

                //double DouRetDiscAmt = Math.Round(Val.Val(DouRetGrossAmount) * Val.Val(txtDiscPer.Text) / 100, 4);
                //double DouRetInsAmt = Math.Round(Val.Val(DouRetGrossAmount) * Val.Val(txtInsurancePer.Text) / 100, 4);
                //double DouRetShipAmt = Math.Round(Val.Val(DouRetGrossAmount) * Val.Val(txtShippingPer.Text) / 100, 4);
                //double DouRetGSTAmt = Math.Round(Val.Val(DouRetGrossAmount) * Val.Val(txtGSTPer.Text) / 100, 4);

                //txtDiscAmount.Text = Val.Format(DouRetDiscAmt, "########0.00");
                //txtInsuranceAmount.Text = Val.Format(DouRetInsAmt, "########0.00");
                //txtShippingAmount.Text = Val.Format(DouRetShipAmt, "########0.00");
                //txtGSTAmount.Text = Val.Format(DouRetGSTAmt, "########0.00");
                //double DouRetNetAmt = Val.Val(DouRetGrossAmount) + DouRetDiscAmt + DouRetInsAmt + DouRetShipAmt + DouRetGSTAmt;
                //txtNetAmount.Text = Val.Format(DouRetNetAmt, "########0.00");

                //double DouRetDiscAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouRetDiscAmt, 2);
                //double DouRetInsAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouRetInsAmt, 2);
                //double DouRetShipAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouRetShipAmt, 2);
                //double DouRetGSTAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouRetGSTAmt, 2);

                //txtDiscAmountFE.Text = Val.Format(DouRetDiscAmtFE, "########0.00");
                //txtInsuranceAmountFE.Text = Val.Format(DouRetInsAmtFE, "########0.00");
                //txtShippingAmountFE.Text = Val.Format(DouRetShipAmtFE, "########0.00");
                //txtGSTAmountFE.Text = Val.Format(DouRetGSTAmtFE, "########0.00");
                //double DouNetAmtFE = Val.Val(DouRetGrossAmount) + DouRetDiscAmtFE + DouRetInsAmtFE + DouRetShipAmtFE + DouRetGSTAmtFE;
                //txtNetAmountFE.Text = Val.Format(DouNetAmtFE, "########0.00");

                ////End : #P : Calculation Of Selected Stone's



                MemoEntryProperty Property = new MemoEntryProperty();
                Property.MEMO_ID = Guid.NewGuid().ToString();
                Property.JANGEDNOSTR = txtJangedNo.Text;
                Property.MEMONO = Val.ToInt64(lblMemoNo.Text);
                Property.MEMOTYPE = Val.ToString(CmbMemoType.SelectedItem);
                Property.MEMODATE = Val.SqlDate(DTPMemoDate.Text);

                Property.BILLINGPARTY_ID = Val.ToString(txtBillingParty.Tag);
                Property.SHIPPINGPARTY_ID = Val.ToString(txtShippingParty.Tag);

                if (txtBroker.Text.Trim().Length != 0)
                {
                    Property.BROKER_ID = Val.ToString(txtBroker.Tag);
                    Property.BROKERPER = Val.Val(txtBrokerPer.Text);
                }
                else
                {
                    Property.BROKER_ID = null;
                    Property.BROKERPER = 0.00;
                }
                if (txtAdat.Text.Trim().Length != 0)
                {
                    Property.ADAT_ID = Val.ToString(txtAdat.Tag);
                    Property.ADATPER = Val.Val(txtAdatPer.Text);
                }
                else
                {
                    Property.ADAT_ID = null;
                    Property.ADATPER = 0.00;
                }

                Property.SELLER_ID = Val.ToString(txtSellerName.Tag);

                Property.TERMS_ID = Val.ToInt32(txtTerms.Tag);
                Property.TERMSDAYS = Val.ToInt32(txtTermsDays.Text);
                Property.TERMSPER = 0;
                Property.TERMSDATE = Val.SqlDate(DTPTermsDate.Text);

                Property.CURRENCY_ID = Val.ToInt32(txtCurrency.Tag);
                Property.EXCRATE = Val.Val(txtExcRate.Text);

                Property.MEMODISCOUNT = 0;

                Property.BILLINGADDRESS1 = Val.ToString(txtBAddress1.Text);
                Property.BILLINGADDRESS2 = Val.ToString(txtBAddress2.Text);
                Property.BILLINGADDRESS3 = Val.ToString(txtBAddress3.Text);
                Property.BILLINGCOUNTRY_ID = Val.ToInt32(txtBCountry.Tag);
                Property.BILLINGSTATE = Val.ToString(txtBState.Text);
                Property.BILLINGCITY = Val.ToString(txtBCity.Text);
                Property.BILLINGZIPCODE = Val.ToString(txtBZipCode.Text);

                Property.SHIPPINGADDRESS1 = Val.ToString(txtSAddress1.Text);
                Property.SHIPPINGADDRESS2 = Val.ToString(txtSAddress2.Text);
                Property.SHIPPINGADDRESS3 = Val.ToString(txtSAddress3.Text);
                Property.SHIPPINGCOUNTRY_ID = Val.ToInt32(txtSCountry.Tag);
                Property.SHIPPINGSTATE = Val.ToString(txtSState.Text);
                Property.SHIPPINGCITY = Val.ToString(txtSCity.Text);
                Property.SHIPPINGZIPCODE = Val.ToString(txtSZipCode.Text);

                Property.DELIVERYTYPE = Val.ToString(CmbDeliveryType.SelectedItem);
                Property.PAYMENTMODE = Val.ToString(CmbPaymentMode.SelectedItem);

                Property.TOTALPCS = Val.ToInt(lblTotalPcs.Text);
                Property.TOTALCARAT = Val.Val(lblTotalCarat.Text);
                Property.TOTALAVGDISC = Val.Val(txtMemoAvgDisc.Text);
                Property.TOTALAVGRATE = Val.Val(txtMemoAvgRate.Text);
                Property.GROSSAMOUNT = Val.Val(txtGrossAmount.Text);
                Property.DISCOUNTPER = Val.Val(txtDiscPer.Text);
                Property.DISCOUNTAMOUNT = Val.Val(txtDiscAmount.Text);
                Property.INSURANCEPER = Val.Val(txtInsurancePer.Text);
                Property.INSURANCEAMOUNT = Val.Val(txtInsuranceAmount.Text);
                Property.SHIPPINGPER = Val.Val(txtShippingPer.Text);
                Property.SHIPPINGAMOUNT = Val.Val(txtShippingAmount.Text);
                Property.GSTPER = Val.Val(txtGSTPer.Text);
                Property.GSTAMOUNT = Val.Val(txtGSTAmount.Text);
                Property.NETAMOUNT = Val.Val(txtNetAmount.Text);
                Property.FGROSSAMOUNT = Val.Val(txtGrossAmountFE.Text);
                Property.FDISCOUNTAMOUNT = Val.Val(txtDiscAmountFE.Text);
                Property.FINSURANCEAMOUNT = Val.Val(txtInsuranceAmountFE.Text);
                Property.FSHIPPINGAMOUNT = Val.Val(txtShippingAmountFE.Text);
                Property.FGSTAMOUNT = Val.Val(txtGSTAmountFE.Text);
                Property.FNETAMOUNT = Val.Val(txtNetAmountFE.Text);

                Property.REMARK = Val.ToString(txtRemark.Text);
                Property.SOURCE = lblSource.Text;

                Property.PROCESS_ID = Val.ToInt32(BtnReturn.Tag);
                Property.PROCESSNAME = Val.ToString(BtnReturn.Text);

                if (Property.PROCESS_ID == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.MessageError("NO RETURN PROCESS ID FOUND FOR THE SAME");
                    return;
                }
                ////List<MemoEntryProperty> _list;// a property class tmare mukvano
                //XmlDocument xmlSearchStone = Global.ConvertToXml(Property);
                //string MemoEntryMasterRecordsForXML = "<DocumentElement><ParamList>" + xmlSearchStone.DocumentElement.InnerXml + "</ParamList></DocumentElement>";

                int IntI = 0;
                foreach (DataRow DRow in DTabMemoDetail.Rows)
                {
                    IntI++;
                    DRow["ENTRYSRNO"] = IntI;
                }
                DTabMemoDetail.AcceptChanges();

                string MemoEntryDetailForXML = string.Empty;
                using (StringWriter sw = new StringWriter())
                {
                    DTabMemoDetail.WriteXml(sw);
                    MemoEntryDetailForXML = sw.ToString();
                }

                string ReturnMessageDesc = "";
                string ReturnMessageType = "";
                Property = ObjMemo.SaveMemoEntry(Property, MemoEntryDetailForXML, "Add Mode");

                txtJangedNo.Text = Property.ReturnValueJanged;
                lblMemoNo.Text = Property.ReturnValue;
                lblMemoNo.Tag = Property.MEMO_ID;
                ReturnMessageDesc = Property.ReturnMessageDesc;
                ReturnMessageType = Property.ReturnMessageType;

                Property = null;
                Global.Message(ReturnMessageDesc);
                if (ReturnMessageType == "SUCCESS")
                {
                    lblMode.Text = "Edit Mode";
                    if (Global.Confirm("DO YOU WANT MAILED INVOICE : " + txtJangedNo.Text) == System.Windows.Forms.DialogResult.Yes)
                    {
                        BtnEmail_Click(null, null);
                    }
                    else
                    {

                    }

                    lblMode.Text = "Edit Mode";
                    if (Global.Confirm("DO YOU WANT TO PRINT INVOICE : " + txtJangedNo.Text) == System.Windows.Forms.DialogResult.Yes)
                    {
                        BtnPrint_Click(null, null);
                    }
                    else
                    {

                    }
                    this.Close();

                }
                */
                ////End : Cmnt : Pinali : 09-10-2019
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private DataTable GetTableOfSelectedRows(GridView view, Boolean IsSelect)
        {
            if (view.RowCount <= 0)
            {
                return null;
            }
            ArrayList aryLst = new ArrayList();


            DataTable resultTable = new DataTable();
            DataTable sourceTable = null;
            sourceTable = ((DataView)view.DataSource).Table;

            if (IsSelect)
            {
                aryLst = ObjGridSelection.GetSelectedArrayList();
                resultTable = sourceTable.Clone();
                for (int i = 0; i < aryLst.Count; i++)
                {
                    DataRowView oDataRowView = aryLst[i] as DataRowView;
                    resultTable.Rows.Add(oDataRowView.Row.ItemArray);
                }
            }

            return resultTable;
        }

        private void FrmMemoEntry_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Close();
        }

        private void BtnOtherActivity_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtBillingParty.Text.Length == 0)
                {
                    Global.Message("Billing Party Is Required");
                    txtBillingParty.Focus();
                    return;
                }
                if (txtShippingParty.Text.Length == 0)
                {
                    Global.Message("Shipping Party Is Required");
                    txtShippingParty.Focus();
                    return;
                }
                if (txtBCountry.Text.Length == 0)
                {
                    Global.Message("Billing Country Is Required");
                    txtBCountry.Focus();
                    return;
                }
                if (txtSCountry.Text.Length == 0)
                {
                    Global.Message("Shipping Country Is Required");
                    txtSCountry.Focus();
                    return;
                }
                if (txtSellerName.Text.Length == 0)
                {
                    Global.Message("Seller Name Is Required");
                    txtSellerName.Focus();
                    return;
                }
                if (txtTerms.Text.Length == 0)
                {
                    Global.Message("Terms Is Required");
                    txtTerms.Focus();
                    return;
                }

                //#P : 06-10-2019
                DataTable DtReturnStoneList = GetTableOfSelectedRows(GrdDetail, true);
                if (DtReturnStoneList.Rows.Count <= 0)
                {
                    Global.Message("Please Select Records That You Want To Deliver..");
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                var SelectedPartyStoneNo = DtReturnStoneList.AsEnumerable().Select(s => s.Field<string>("PARTYSTOCKNO")).ToArray();
                string StrPartyStoneNoList = string.Join(",", SelectedPartyStoneNo);

                LiveStockProperty LStockProperty = new LiveStockProperty();
                LStockProperty.STOCKNO = Val.ToString(StrPartyStoneNoList);
                LStockProperty.STOCKTYPE = Val.ToString(mStrStockType);
                DataSet DsLiveStock = new BOTRN_StockUpload().GetStoneDetailForMemoForm(LStockProperty);

                DataTable DtabInvoiceDetail = new DataTable();
                DtabInvoiceDetail = DsLiveStock.Tables[0];

                if (DtabInvoiceDetail.Rows.Count > 0)
                {
                    this.Cursor = Cursors.Default;
                    FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                    FrmMemoEntry.MdiParent = Global.gMainRef;
                    FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                    if (mFormType == FORMTYPE.ORDERCONFIRM)
                        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.SALEINVOICE, DtabInvoiceDetail, mStrStockType);
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
                this.Cursor = Cursors.Default;
            }

        }

        private void txtBState_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "STATECODE,STATENAME,GSTCODE";
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
                        //txtBState.Tag = Val.ToString(FrmSearch.DRow["STATE_ID"]);
                    }
                    FrmSearch.Hide();
                    FrmSearch.Dispose();
                    FrmSearch = null;
                }
                // AutoGstCalculation();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }

        private void txtSState_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "STATECODE,STATENAME,GSTCODE";
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
                    //  AutoGstCalculation();
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message);
            }
        }
        private void BtnUsdPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (Val.ToString(lblMemoNo.Tag) == "")
                {
                    Global.Message("No Memo Found");
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                DataTable DTab = ObjMemo.Print(Val.ToString(lblMemoNo.Tag), "USD");
                if (DTab.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("There Is No Data Found For Print");
                    return;
                }

                DataSet DS = new DataSet();
                DTab.TableName = "Table";
                DS.Tables.Add(DTab);
                DataTable DTabDuplicate = DTab.Copy();
                DTabDuplicate.TableName = "Table1";
                foreach (DataRow DRow in DTabDuplicate.Rows)
                {
                    DRow["PRINTTYPE"] = "DUBLICATE";
                }
                DTabDuplicate.AcceptChanges();
                DS.Tables.Add(DTabDuplicate);

                Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                FrmReportViewer.MdiParent = Global.gMainRef;
                FrmReportViewer.ShowFormMemoPrint("MemoPrint", DS);
                this.Cursor = Cursors.Default;
            }
            catch (Exception EX)
            {
                this.Cursor = Cursors.Default;
                Global.Message(EX.Message);
            }

        }

        private void BtnInrPrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (Val.ToString(lblMemoNo.Tag) == "")
                {
                    Global.Message("No Memo Found");
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                DataTable DTab = ObjMemo.Print(Val.ToString(lblMemoNo.Tag), "INR");
                if (DTab.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("There Is No Data Found For Print");
                    return;
                }

                DataSet DS = new DataSet();
                DTab.TableName = "Table";
                DS.Tables.Add(DTab);
                DataTable DTabDuplicate = DTab.Copy();
                DTabDuplicate.TableName = "Table1";
                foreach (DataRow DRow in DTabDuplicate.Rows)
                {
                    DRow["PRINTTYPE"] = "DUBLICATE";
                }
                DTabDuplicate.AcceptChanges();
                DS.Tables.Add(DTabDuplicate);

                Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                FrmReportViewer.MdiParent = Global.gMainRef;
                FrmReportViewer.ShowFormMemoPrint("MemoPrint", DS);
                this.Cursor = Cursors.Default;
            }
            catch (Exception EX)
            {
                this.Cursor = Cursors.Default;
                Global.Message(EX.Message);
            }

        }

        private void BtnInvoicePrint_Click(object sender, EventArgs e)
        {

        }

        private void txtCompanyBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "LEDGERCODE,LEDGERNAME,BRANCHNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LEDGERBANK);
                    FrmSearch.mStrColumnsToHide = "LEDGER_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtCompanyBank.Tag = Val.ToString(FrmSearch.DRow["LEDGER_ID"]);
                        txtCompanyBank.Text = Val.ToString(FrmSearch.DRow["LEDGERNAME"]);
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

        private void txtPartyBank_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    if (txtBillingParty.Text.ToString().Trim().Equals(string.Empty))
                    {
                        Global.Message("Please Select Ship To Party.");
                        txtBillingParty.Focus();
                    }

                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "BANKNAME,BRANCHNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = ObjMemo.GetPartyBankDetail(Val.ToString(txtBillingParty.Tag));
                    FrmSearch.mStrColumnsToHide = "BANK_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtPartyBank.Tag = Val.ToString(FrmSearch.DRow["BANK_ID"]);
                        txtPartyBank.Text = Val.ToString(FrmSearch.DRow["BANKNAME"]);
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

        private void txtCourier_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "COURIERCODE,COURIERNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_COURIER);
                    FrmSearch.mStrColumnsToHide = "COURIER_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtCourier.Tag = Val.ToString(FrmSearch.DRow["COURIER_ID"]);
                        txtCourier.Text = Val.ToString(FrmSearch.DRow["COURIERNAME"]);
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

        private void txtAirFreight_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "AIRFREIGHTCODE,AIRFREIGHTNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_AIRFREIGHT);
                    FrmSearch.mStrColumnsToHide = "COURIER_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtAirFreight.Tag = Val.ToString(FrmSearch.DRow["AIRFREIGHT_ID"]);
                        txtAirFreight.Text = Val.ToString(FrmSearch.DRow["AIRFREIGHTNAME"]);
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

        private void cmbBillType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (new[] { "CASH" }.Contains(cmbBillType.Text.ToUpper()))
                {
                    DataTable DTab = ObjMemo.GetHKCashChargePer();//Change by gunjan:17/04/2023
                    txtGSTPer.Text = DTab.Rows[0]["SETTINGVALUE"].ToString();
                    // txtGSTPer.Text = "0.40";//"0.25"; //Changes by Daksha on 04/04/2023
                    txtGSTPer_Validated(null, null);
                }

            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void cmbAddresstype_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Val.ToString(cmbAddresstype.SelectedItem) == "MUMBAI")
            {
                txtPortOfLoading.Text = "MUMBAI";
            }
            else
            {
                txtPortOfLoading.Text = "SURAT";
            }
            //AutoGstCalculation();
        }

        private void AutoGstCalculation()
        {
            //string StrCmbState = "";
            //if (cmbAddresstype.SelectedIndex > -1)
            //{
            //    if (cmbAddresstype.SelectedIndex == 1)
            //        StrCmbState = "MAHARASHTRA";
            //    else if (cmbAddresstype.SelectedIndex == 2)
            //        StrCmbState = "GUJARAT";
            //}
            //else
            //    StrCmbState = "MAHARASHTRA";
            if (PnlGSTDetail.Visible == true && txtBCountry.Text == "INDIA")
            {
                string StrTxtBState = txtBState.Text.ToUpper();
                if (StrTxtBState == "MAHARASHTRA")
                {
                    txtCGSTPer.Text = "0.125";
                    txtSGSTPer.Text = "0.125";
                    txtIGSTPer.Text = "0.00";
                    txtIGSTAmount.Text = "0.00";
                    txtIGSTAmountFE.Text = "0.00";

                    if (Val.Val(txtGrossAmount.Text) != 0)
                    {

                        if (txtCurrency.Text == "USD")
                        {
                            txtCGSTAmount.Text = Math.Round((Val.Val(txtGrossAmount.Text) * Val.Val(txtCGSTPer.Text)) / 100, 3).ToString();
                            txtCGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtCGSTAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                            txtSGSTAmount.Text = Math.Round((Val.Val(txtGrossAmount.Text) * Val.Val(txtSGSTPer.Text)) / 100, 3).ToString();
                            txtSGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtSGSTAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                        }
                        else
                        {
                            txtCGSTAmountFE.Text = Math.Round((Val.Val(txtGrossAmountFE.Text) * Val.Val(txtCGSTPer.Text)) / 100, 3).ToString();
                            txtCGSTAmount.Text = Val.Format(Math.Round(Val.Val(txtCGSTAmountFE.Text) / Val.Val(txtExcRate.Text), 3), "########0.000");
                            txtSGSTAmountFE.Text = Math.Round((Val.Val(txtGrossAmountFE.Text) * Val.Val(txtSGSTPer.Text)) / 100, 3).ToString();
                            txtSGSTAmount.Text = Val.Format(Math.Round(Val.Val(txtSGSTAmountFE.Text) / Val.Val(txtExcRate.Text), 3), "########0.000");
                        }
                    }
                    else
                    {
                        txtCGSTPer.Text = "0.00";
                        txtCGSTAmount.Text = "0.00";
                        txtCGSTAmountFE.Text = "0.00";
                        txtSGSTPer.Text = "0.00";
                        txtSGSTAmount.Text = "0.00";
                        txtSGSTAmountFE.Text = "0.00";
                    }

                    txtCGSTPer.ReadOnly = false;
                    txtCGSTAmount.ReadOnly = false;
                    txtCGSTAmountFE.ReadOnly = false;
                    txtSGSTPer.ReadOnly = false;
                    txtSGSTAmount.ReadOnly = false;
                    txtSGSTAmountFE.ReadOnly = false;

                    txtIGSTPer.ReadOnly = true;
                    txtIGSTAmount.ReadOnly = true;
                    txtIGSTAmountFE.ReadOnly = true;
                }
                else
                {
                    txtCGSTPer.Text = "0.00";
                    txtCGSTAmount.Text = "0.00";
                    txtCGSTAmountFE.Text = "0.00";
                    txtSGSTPer.Text = "0.00";
                    txtSGSTAmount.Text = "0.00";
                    txtSGSTAmountFE.Text = "0.00";
                    txtIGSTPer.Text = "0.250";

                    if (Val.Val(txtGrossAmount.Text) != 0)
                    {
                        //txtCGSTAmount.Text = Math.Round((Val.Val(txtGrossAmount.Text) * Val.Val(txtCGSTPer.Text)) / 100, 2).ToString();
                        //txtCGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtCGSTAmount.Text) / Val.Val(txtExcRate.Text), 2), "########0.00");
                        if (txtCurrency.Text == "USD")
                        {
                            txtIGSTAmount.Text = Math.Round((Val.Val(txtGrossAmount.Text) * Val.Val(txtIGSTPer.Text)) / 100, 3).ToString();
                            txtIGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtIGSTAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                        }
                        else
                        {
                            txtIGSTAmountFE.Text = Math.Round((Val.Val(txtGrossAmountFE.Text) * Val.Val(txtIGSTPer.Text)) / 100, 3).ToString();
                            txtIGSTAmount.Text = Val.Format(Math.Round(Val.Val(txtIGSTAmountFE.Text) / Val.Val(txtExcRate.Text), 3), "########0.000");
                        }
                    }
                    else
                    {
                        txtIGSTPer.Text = "0.00";
                        txtIGSTAmount.Text = "0.00";
                        txtIGSTAmountFE.Text = "0.00";
                    }

                    txtCGSTPer.ReadOnly = true;
                    txtCGSTAmount.ReadOnly = true;
                    txtCGSTAmountFE.ReadOnly = true;
                    txtSGSTPer.ReadOnly = true;
                    txtSGSTAmount.ReadOnly = true;
                    txtSGSTAmountFE.ReadOnly = true;

                    txtIGSTPer.ReadOnly = false;
                    txtIGSTAmount.ReadOnly = false;
                    txtIGSTAmountFE.ReadOnly = false;
                }
                CalculationNew();
            }
        }
        /// <summary>
        /// Added Kuldeep , So GST Function Call When User Change Address Type
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbAddresstype_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (Val.ToString(cmbAddresstype.SelectedItem) == "MUMBAI")
            {
                txtPortOfLoading.Text = "MUMBAI";
            }
            else
            {
                txtPortOfLoading.Text = "SURAT";
            }
            //  AutoGstCalculation();
        }



        private void txtCGSTPer_Validated(object sender, EventArgs e)
        {
            double DouDiscAmt = Math.Round(Val.Val(txtDiscAmount.Text), 4);
            double DouGSTAmt = Math.Round(Val.Val(txtGSTAmount.Text), 4);
            double DouInsAmt = Math.Round(Val.Val(txtInsuranceAmount.Text), 4);
            double DouShipAmt = Math.Round(Val.Val(txtShippingAmount.Text), 4);


            double DouDiscAmtFE = Math.Round(Val.Val(txtDiscAmountFE.Text), 2);
            double DouGSTAmtFE = Math.Round(Val.Val(txtGSTAmountFE.Text), 2);
            double DouInsAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouInsAmt, 2);
            double DouShipAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouShipAmt, 2);

            double DouGrossAmt = (Val.Val(txtGrossAmount.Text) + DouGSTAmt + DouInsAmt + DouShipAmt) - DouDiscAmt;
            double DouGrossAmtFe = Val.Val(txtGrossAmountFE.Text);

            if (Val.Val(txtGrossAmount.Text) != 0)
            {
                if (txtCurrency.Text == "USD")
                {
                    txtCGSTAmount.Text = Math.Round((DouGrossAmt * Val.Val(txtCGSTPer.Text)) / 100, 3).ToString();
                    txtCGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtCGSTAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                }
                else
                {
                    txtCGSTAmountFE.Text = Math.Round((DouGrossAmtFe * Val.Val(txtCGSTPer.Text)) / 100, 3).ToString();
                    txtCGSTAmount.Text = Val.Format(Math.Round(Val.Val(txtCGSTAmountFE.Text) / Val.Val(txtExcRate.Text), 3), "########0.000");
                }
            }
            else
            {
                txtCGSTPer.Text = "0.00";
                txtCGSTAmount.Text = "0.00";
                txtCGSTAmountFE.Text = "0.00";
            }

            CalculationNew();
        }

        private void txtCGSTAmountFE_TextChanged(object sender, EventArgs e)
        {
            if (txtCGSTAmountFE.Focused)
            {
                try
                {
                    if (Val.Val(txtGrossAmountFE.Text) != 0)
                    {
                        double DouCGSTAmount = 0;
                        DouCGSTAmount = Math.Round(Val.Val(txtCGSTAmountFE.Text) / Val.Val(txtExcRate.Text), 3);

                        txtCGSTAmount.Text = Val.Format(DouCGSTAmount, "########0.000");
                        txtCGSTPer.Text = Math.Round((Val.Val(DouCGSTAmount) / Val.Val(txtGrossAmount.Text)) * 100, 3).ToString();
                    }
                    else
                    {
                        txtCGSTPer.Text = "0.00";
                        txtCGSTAmount.Text = "0.00";
                        txtCGSTAmountFE.Text = "0.00";
                    }
                    CalculationNew();
                }
                catch (Exception ex)
                {
                    Global.Message(ex.Message.ToString());
                }
            }
        }

        private void txtCGSTAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtCGSTAmount.Focused)
            {
                try
                {
                    if (Val.Val(txtGrossAmount.Text) != 0)
                    {
                        txtCGSTPer.Text = Math.Round((Val.Val(txtCGSTAmount.Text) / Val.Val(txtGrossAmount.Text)) * 100, 2).ToString();
                        txtCGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtCGSTAmount.Text) * Val.Val(txtExcRate.Text), 2), "########0.000");
                    }
                    else
                    {
                        txtCGSTPer.Text = "0.00";
                        txtCGSTAmount.Text = "0.00";
                        txtCGSTAmountFE.Text = "0.00";
                    }
                    CalculationNew();
                }
                catch (Exception ex)
                {
                    Global.Message(ex.Message.ToString());
                }
            }
        }



        private void txtSGSTPer_Validated(object sender, EventArgs e)
        {
            double DouDiscAmt = Math.Round(Val.Val(txtDiscAmount.Text), 4);
            double DouGSTAmt = Math.Round(Val.Val(txtGSTAmount.Text), 4);
            double DouInsAmt = Math.Round(Val.Val(txtInsuranceAmount.Text), 4);
            double DouShipAmt = Math.Round(Val.Val(txtShippingAmount.Text), 4);


            double DouDiscAmtFE = Math.Round(Val.Val(txtDiscAmountFE.Text), 2);
            double DouGSTAmtFE = Math.Round(Val.Val(txtGSTAmountFE.Text), 2);
            double DouInsAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouInsAmt, 2);
            double DouShipAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouShipAmt, 2);

            double DouGrossAmt = (Val.Val(txtGrossAmount.Text) + DouGSTAmt + DouInsAmt + DouShipAmt) - DouDiscAmt;
            double DouGrossAmtFe = Val.Val(txtGrossAmountFE.Text);

            if (Val.Val(txtGrossAmount.Text) != 0)
            {
                if (txtCurrency.Text == "USD")
                {
                    txtSGSTAmount.Text = Math.Round((DouGrossAmt * Val.Val(txtSGSTPer.Text)) / 100, 2).ToString();
                    txtSGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtSGSTAmount.Text) * Val.Val(txtExcRate.Text), 2), "########0.00");
                }
                else
                {
                    txtSGSTAmountFE.Text = Math.Round((DouGrossAmtFe * Val.Val(txtSGSTPer.Text)) / 100, 2).ToString();
                    txtSGSTAmount.Text = Val.Format(Math.Round(Val.Val(txtSGSTAmountFE.Text) / Val.Val(txtExcRate.Text), 2), "########0.00");
                }
            }
            else
            {
                txtSGSTPer.Text = "0.00";
                txtSGSTAmount.Text = "0.00";
                txtSGSTAmountFE.Text = "0.00";
            }

            CalculationNew();
        }

        private void txtSGSTAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtSGSTAmount.Focused)
            {
                try
                {
                    if (Val.Val(txtGrossAmount.Text) != 0)
                    {
                        txtSGSTPer.Text = Math.Round((Val.Val(txtSGSTAmount.Text) / Val.Val(txtGrossAmount.Text)) * 100, 2).ToString();
                        txtSGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtSGSTAmount.Text) * Val.Val(txtExcRate.Text), 2), "########0.000");
                    }
                    else
                    {
                        txtSGSTPer.Text = "0.00";
                        txtSGSTAmount.Text = "0.00";
                        txtSGSTAmountFE.Text = "0.00";
                    }
                    CalculationNew();
                }
                catch (Exception ex)
                {
                    Global.Message(ex.Message.ToString());
                }
            }
        }

        private void txtSGSTAmountFE_TextChanged(object sender, EventArgs e)
        {
            if (txtSGSTAmountFE.Focused)
            {
                try
                {
                    if (Val.Val(txtGrossAmountFE.Text) != 0)
                    {
                        double DouSGSTAmount = 0;
                        DouSGSTAmount = Math.Round(Val.Val(txtSGSTAmountFE.Text) / Val.Val(txtExcRate.Text), 3);

                        txtSGSTAmount.Text = Val.Format(DouSGSTAmount, "########0.000");
                        txtSGSTPer.Text = Math.Round((Val.Val(DouSGSTAmount) / Val.Val(txtGrossAmount.Text)) * 100, 3).ToString();
                    }
                    else
                    {
                        txtSGSTPer.Text = "0.00";
                        txtSGSTAmount.Text = "0.00";
                        txtSGSTAmountFE.Text = "0.00";
                    }
                    CalculationNew();
                }
                catch (Exception ex)
                {
                    Global.Message(ex.Message.ToString());
                }
            }
        }



        private void txtIGSTPer_Validated(object sender, EventArgs e)
        {
            double DouDiscAmt = Math.Round(Val.Val(txtDiscAmount.Text), 4);
            double DouGSTAmt = Math.Round(Val.Val(txtGSTAmount.Text), 4);
            double DouInsAmt = Math.Round(Val.Val(txtInsuranceAmount.Text), 4);
            double DouShipAmt = Math.Round(Val.Val(txtShippingAmount.Text), 4);


            double DouDiscAmtFE = Math.Round(Val.Val(txtDiscAmountFE.Text), 2);
            double DouGSTAmtFE = Math.Round(Val.Val(txtGSTAmountFE.Text), 2);
            double DouInsAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouInsAmt, 2);
            double DouShipAmtFE = Math.Round(Val.Val(txtExcRate.Text) * DouShipAmt, 2);

            double DouGrossAmt = (Val.Val(txtGrossAmount.Text) + DouGSTAmt + DouInsAmt + DouShipAmt) - DouDiscAmt;
            double DouGrossAmtFe = Val.Val(txtGrossAmountFE.Text);

            if (Val.Val(txtGrossAmount.Text) != 0)
            {
                if (txtCurrency.Text == "USD")
                {
                    txtIGSTAmount.Text = Math.Round((DouGrossAmt * Val.Val(txtIGSTPer.Text)) / 100, 3).ToString();
                    txtIGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtIGSTAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                }
                else
                {
                    txtIGSTAmountFE.Text = Math.Round((DouGrossAmtFe * Val.Val(txtIGSTPer.Text)) / 100, 3).ToString();
                    txtIGSTAmount.Text = Val.Format(Math.Round(Val.Val(txtIGSTAmountFE.Text) / Val.Val(txtExcRate.Text), 3), "########0.000");
                }
            }
            else
            {
                txtIGSTPer.Text = "0.00";
                txtIGSTAmount.Text = "0.00";
                txtIGSTAmountFE.Text = "0.00";
            }

            CalculationNew();
        }

        private void txtIGSTAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtIGSTAmount.Focused)
            {
                try
                {
                    if (Val.Val(txtGrossAmount.Text) != 0)
                    {
                        txtIGSTPer.Text = Math.Round((Val.Val(txtIGSTAmount.Text) / Val.Val(txtGrossAmount.Text)) * 100, 3).ToString();
                        txtIGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtIGSTAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                    }
                    else
                    {
                        txtIGSTPer.Text = "0.00";
                        txtIGSTAmount.Text = "0.00";
                        txtIGSTAmountFE.Text = "0.00";
                    }
                    CalculationNew();
                }
                catch (Exception ex)
                {
                    Global.Message(ex.Message.ToString());
                }
            }
        }

        private void txtIGSTAmountFE_TextChanged(object sender, EventArgs e)
        {
            if (txtIGSTAmountFE.Focused)
            {
                try
                {
                    if (Val.Val(txtGrossAmountFE.Text) != 0)
                    {
                        double DouIGSTAmount = 0;
                        DouIGSTAmount = Math.Round(Val.Val(txtIGSTAmountFE.Text) / Val.Val(txtExcRate.Text), 3);

                        txtIGSTAmount.Text = Val.Format(DouIGSTAmount, "########0.000");
                        txtIGSTPer.Text = Math.Round((Val.Val(DouIGSTAmount) / Val.Val(txtGrossAmount.Text)) * 100, 3).ToString();
                    }
                    else
                    {
                        txtIGSTPer.Text = "0.00";
                        txtIGSTAmount.Text = "0.00";
                        txtIGSTAmountFE.Text = "0.00";
                    }
                    CalculationNew();
                }
                catch (Exception ex)
                {
                    Global.Message(ex.Message.ToString());
                }
            }
        }



        private void txtTCSPer_Validated(object sender, EventArgs e)
        {
            if (Val.Val(txtGrossAmount.Text) != 0)
            {
                if (txtCurrency.Text == "USD")
                {
                    txtTCSAmount.Text = Math.Round((Val.Val(txtGrossAmount.Text) * Val.Val(txtTCSPer.Text)) / 100, 3).ToString();
                    txtTCSAmountFE.Text = Val.Format(Math.Round(Val.Val(txtTCSAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                }
                else
                {
                    txtTCSAmountFE.Text = Math.Round((Val.Val(txtGrossAmountFE.Text) * Val.Val(txtTCSPer.Text)) / 100, 3).ToString();
                    txtTCSAmount.Text = Val.Format(Math.Round(Val.Val(txtTCSAmountFE.Text) / Val.Val(txtExcRate.Text), 3), "########0.000");
                }
            }
            else
            {
                txtTCSPer.Text = "0.00";
                txtTCSAmount.Text = "0.00";
                txtTCSAmountFE.Text = "0.00";
            }
            CalculationNew();
        }

        private void txtTCSAmount_TextChanged(object sender, EventArgs e)
        {
            if (txtTCSAmount.Focused)
            {
                try
                {
                    if (Val.Val(txtGrossAmount.Text) != 0)
                    {
                        txtTCSPer.Text = Math.Round((Val.Val(txtTCSAmount.Text) / Val.Val(txtGrossAmount.Text)) * 100, 2).ToString();
                        txtTCSAmountFE.Text = Val.Format(Math.Round(Val.Val(txtTCSAmount.Text) * Val.Val(txtExcRate.Text), 2), "########0.00");
                    }
                    else
                    {
                        txtTCSPer.Text = "0.00";
                        txtTCSAmount.Text = "0.00";
                        txtTCSAmountFE.Text = "0.00";
                    }
                    CalculationNew();
                }
                catch (Exception ex)
                {
                    Global.Message(ex.Message.ToString());
                }
            }
        }

        private void txtTCSAmountFE_TextChanged(object sender, EventArgs e)
        {
            if (txtTCSAmountFE.Focused)
            {
                try
                {
                    if (Val.Val(txtGrossAmountFE.Text) != 0)
                    {
                        double DouIGSTAmount = 0;
                        DouIGSTAmount = Math.Round(Val.Val(txtTCSAmountFE.Text) / Val.Val(txtExcRate.Text), 3);


                        txtTCSAmount.Text = Val.Format(DouIGSTAmount, "########0.000");
                        txtTCSPer.Text = Math.Round((Val.Val(DouIGSTAmount) / Val.Val(txtGrossAmount.Text)) * 100, 3).ToString();
                    }
                    else
                    {
                        txtTCSPer.Text = "0.00";
                        txtTCSAmount.Text = "0.00";
                        txtTCSAmountFE.Text = "0.00";
                    }
                    CalculationNew();
                }
                catch (Exception ex)
                {
                    Global.Message(ex.Message.ToString());
                }
            }
        }



        private void CmbRoundOff_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtRoundOffAmount_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtRoundOffAmountFE_TextChanged(object sender, EventArgs e)
        {

        }



        private void txtGSTPer_Validated(object sender, EventArgs e)
        {
            //#P : 25-12-2019
            if (Val.Val(txtGrossAmount.Text) != 0)
            {
                if (txtCurrency.Text == "USD")
                {
                    txtGSTAmount.Text = Math.Round((Val.Val(txtGrossAmount.Text) * Val.Val(txtGSTPer.Text)) / 100, 3).ToString();
                    txtGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtGSTAmount.Text) * Val.Val(txtExcRate.Text), 3), "########0.000");
                }
                else
                {
                    txtGSTAmountFE.Text = Math.Round((Val.Val(txtGrossAmountFE.Text) * Val.Val(txtGSTPer.Text)) / 100, 3).ToString();
                    txtGSTAmount.Text = Val.Format(Math.Round(Val.Val(txtGSTAmountFE.Text) / Val.Val(txtExcRate.Text), 3), "########0.000");
                }
            }
            else
            {
                txtGSTPer.Text = "0.00";
                txtGSTAmount.Text = "0.00";
                txtGSTAmountFE.Text = "0.00";
            }
            //End : #P : 25-12-2019
            txtCGSTPer_Validated(sender, e);
            txtSGSTPer_Validated(sender, e);
            txtIGSTPer_Validated(sender, e);

            CalculationNew();
            GetSummuryDetailForGrid();

        }

        private void txtGSTAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtGSTAmount.Focused)
                {
                    if (Val.Val(txtGrossAmount.Text) != 0)
                    {
                        txtGSTPer.Text = Math.Round((Val.Val(txtGSTAmount.Text) / Val.Val(txtGrossAmount.Text)) * 100, 2).ToString();
                        txtGSTAmountFE.Text = Val.Format(Math.Round(Val.Val(txtGSTAmount.Text) * Val.Val(txtExcRate.Text), 2), "########0.00");
                    }
                    else
                    {
                        txtGSTPer.Text = "0.00";
                        txtGSTAmount.Text = "0.00";
                        txtGSTAmountFE.Text = "0.00";
                    }
                    CalculationNew();
                    GetSummuryDetailForGrid();

                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtGSTAmountFE_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtGSTAmountFE.Focused)
                {
                    if (Val.Val(txtGrossAmountFE.Text) != 0)
                    {
                        double DouGSTAmount = 0;
                        DouGSTAmount = Math.Round(Val.Val(txtGSTAmountFE.Text) / Val.Val(txtExcRate.Text), 3);

                        txtGSTAmount.Text = Val.Format(DouGSTAmount, "########0.000");
                        txtGSTPer.Text = Math.Round((Val.Val(DouGSTAmount) / Val.Val(txtGrossAmount.Text)) * 100, 3).ToString();
                    }
                    else
                    {
                        txtGSTPer.Text = "0.00";
                        txtGSTAmount.Text = "0.00";
                        txtGSTAmountFE.Text = "0.00";
                    }

                    CalculationNew();
                    GetSummuryDetailForGrid();

                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }



        private void txtInsurancePer_Validated(object sender, EventArgs e)
        {
            //#P : 25-12-2019
            if (Val.Val(txtGrossAmount.Text) != 0)
            {
                if (txtCurrency.Text == "USD")
                {
                    txtInsuranceAmount.Text = Math.Round((Val.Val(txtGrossAmount.Text) * Val.Val(txtInsurancePer.Text)) / 100, 2).ToString();
                    txtInsuranceAmountFE.Text = Val.Format(Math.Round(Val.Val(txtInsuranceAmount.Text) * Val.Val(txtExcRate.Text), 2), "########0.00");
                }
                else
                {
                    txtInsuranceAmountFE.Text = Math.Round((Val.Val(txtGrossAmountFE.Text) * Val.Val(txtInsurancePer.Text)) / 100, 2).ToString();
                    txtInsuranceAmount.Text = Val.Format(Math.Round(Val.Val(txtInsuranceAmountFE.Text) / Val.Val(txtExcRate.Text), 2), "########0.00");
                }
            }
            else
            {
                txtInsurancePer.Text = "0.00";
                txtInsuranceAmount.Text = "0.00";
                txtInsuranceAmountFE.Text = "0.00";
            }
            //End : #P : 25-12-2019
            txtCGSTPer_Validated(sender, e);
            txtSGSTPer_Validated(sender, e);
            txtIGSTPer_Validated(sender, e);
            CalculationNew();
            GetSummuryDetailForGrid();

        }

        private void txtInsuranceAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtInsuranceAmountFE.Focused)
                {
                    if (Val.Val(txtGrossAmount.Text) != 0)
                    {
                        txtInsurancePer.Text = Math.Round((Val.Val(txtInsuranceAmount.Text) / Val.Val(txtGrossAmount.Text)) * 100, 2).ToString();
                        txtInsuranceAmountFE.Text = Val.Format(Math.Round(Val.Val(txtInsuranceAmount.Text) * Val.Val(txtExcRate.Text), 2), "########0.00");
                    }
                    else
                    {
                        txtInsurancePer.Text = "0.00";
                        txtInsuranceAmount.Text = "0.00";
                        txtInsuranceAmountFE.Text = "0.00";
                    }
                    CalculationNew();
                    GetSummuryDetailForGrid();

                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtInsuranceAmountFE_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtInsuranceAmountFE.Focused)
                {
                    if (Val.Val(txtGrossAmountFE.Text) != 0)
                    {
                        double DouInsAmount = 0;
                        DouInsAmount = Math.Round(Val.Val(txtInsuranceAmountFE.Text) / Val.Val(txtExcRate.Text), 2);

                        txtInsuranceAmount.Text = Val.Format(DouInsAmount, "########0.00");
                        txtInsurancePer.Text = Math.Round((Val.Val(DouInsAmount) / Val.Val(txtGrossAmount.Text)) * 100, 2).ToString();
                    }
                    else
                    {
                        txtInsurancePer.Text = "0.00";
                        txtInsuranceAmount.Text = "0.00";
                        txtInsuranceAmountFE.Text = "0.00";
                    }
                    CalculationNew();
                    GetSummuryDetailForGrid();

                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }



        private void txtShippingPer_Validated(object sender, EventArgs e)
        {
            //#P : 25-12-2019
            if (Val.Val(txtGrossAmount.Text) != 0)
            {
                if (txtCurrency.Text == "USD")
                {
                    txtShippingAmount.Text = Math.Round((Val.Val(txtGrossAmount.Text) * Val.Val(txtShippingPer.Text)) / 100, 2).ToString();
                    txtShippingAmountFE.Text = Val.Format(Math.Round(Val.Val(txtShippingAmount.Text) * Val.Val(txtExcRate.Text), 2), "########0.00");
                }
                else
                {
                    txtShippingAmountFE.Text = Math.Round((Val.Val(txtGrossAmountFE.Text) * Val.Val(txtShippingPer.Text)) / 100, 2).ToString();
                    txtShippingAmount.Text = Val.Format(Math.Round(Val.Val(txtShippingAmountFE.Text) / Val.Val(txtExcRate.Text), 2), "########0.00");
                }
            }
            else
            {
                txtShippingPer.Text = "0.00";
                txtShippingAmount.Text = "0.00";
                txtShippingAmountFE.Text = "0.00";
            }
            //End : #P : 25-12-2019
            txtCGSTPer_Validated(sender, e);
            txtSGSTPer_Validated(sender, e);
            txtIGSTPer_Validated(sender, e);
            CalculationNew();
            GetSummuryDetailForGrid();

        }

        private void txtShippingAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtShippingAmount.Focused)
                {
                    if (Val.Val(txtGrossAmount.Text) != 0)
                    {
                        txtShippingPer.Text = Math.Round((Val.Val(txtShippingAmount.Text) / Val.Val(txtGrossAmount.Text)) * 100, 2).ToString();
                        txtShippingAmountFE.Text = Val.Format(Math.Round(Val.Val(txtShippingAmount.Text) * Val.Val(txtExcRate.Text), 2), "########0.00");
                    }
                    else
                    {
                        txtShippingPer.Text = "0.00";
                        txtShippingAmount.Text = "0.00";
                        txtShippingAmountFE.Text = "0.00";
                    }
                    CalculationNew();
                    GetSummuryDetailForGrid();

                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtShippingAmountFE_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtShippingAmount.Focused)
                {
                    if (Val.Val(txtGrossAmountFE.Text) != 0)
                    {
                        double DouShippingAmount = 0;
                        DouShippingAmount = Math.Round(Val.Val(txtShippingAmountFE.Text) / Val.Val(txtExcRate.Text), 2);

                        txtShippingAmount.Text = Val.Format(DouShippingAmount, "########0.00");
                        txtShippingPer.Text = Math.Round((Val.Val(DouShippingAmount) / Val.Val(txtGrossAmount.Text)) * 100, 2).ToString();
                    }
                    else
                    {
                        txtShippingPer.Text = "0.00";
                        txtShippingAmount.Text = "0.00";
                        txtShippingAmountFE.Text = "0.00";
                    }
                    CalculationNew();
                    GetSummuryDetailForGrid();

                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }


        private void txtDiscPer_Validated(object sender, EventArgs e)
        {
            //#P : 25-12-2019
            if (Val.Val(txtGrossAmount.Text) != 0)
            {
                if (txtCurrency.Text == "USD")
                {
                    txtDiscAmount.Text = Math.Round((Val.Val(txtGrossAmount.Text) * Val.Val(txtDiscPer.Text)) / 100, 2).ToString();
                    txtDiscAmountFE.Text = Val.Format(Math.Round(Val.Val(txtDiscAmount.Text) * Val.Val(txtExcRate.Text), 2), "########0.00");
                }
                else
                {
                    txtDiscAmountFE.Text = Math.Round((Val.Val(txtGrossAmountFE.Text) * Val.Val(txtDiscPer.Text)) / 100, 2).ToString();
                    txtDiscAmount.Text = Val.Format(Math.Round(Val.Val(txtDiscAmountFE.Text) / Val.Val(txtExcRate.Text), 2), "########0.00");
                }
            }
            else
            {
                txtDiscPer.Text = "0.00";
                txtDiscAmount.Text = "0.00";
                txtDiscAmountFE.Text = "0.00";
            }
            //End : #P : 25-12-2019
            txtCGSTPer_Validated(sender, e);
            txtSGSTPer_Validated(sender, e);
            txtIGSTPer_Validated(sender, e);
            CalculationNew();
            GetSummuryDetailForGrid();

        }

        private void txtDiscAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtGrossAmountFE.Focused)
                {
                    if (Val.Val(txtGrossAmount.Text) != 0)
                    {
                        txtDiscPer.Text = Math.Round((Val.Val(txtDiscAmount.Text) / Val.Val(txtGrossAmount.Text)) * 100, 2).ToString();
                        txtDiscAmountFE.Text = Val.Format(Math.Round(Val.Val(txtDiscAmount.Text) * Val.Val(txtExcRate.Text), 2), "########0.00");
                    }
                    else
                    {
                        txtDiscPer.Text = "0.00";
                        txtDiscAmount.Text = "0.00";
                        txtDiscAmountFE.Text = "0.00";
                    }
                    CalculationNew();
                    GetSummuryDetailForGrid();

                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtDiscAmountFE_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtGrossAmountFE.Focused)
                {
                    if (Val.Val(txtGrossAmountFE.Text) != 0)
                    {
                        double DouDiscountAmount = 0;

                        DouDiscountAmount = Math.Round(Val.Val(txtDiscAmountFE.Text) / Val.Val(txtExcRate.Text), 2);
                        txtDiscAmount.Text = Val.Format(DouDiscountAmount, "########0.00");
                        txtDiscPer.Text = Math.Round((Val.Val(DouDiscountAmount) / Val.Val(txtGrossAmount.Text)) * 100, 2).ToString();
                    }
                    else
                    {
                        txtDiscPer.Text = "0.00";
                    }

                    CalculationNew();
                    GetSummuryDetailForGrid();

                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }


        #region Account

        public void AddAccountingEffect()
        {
            // int IntCnt = 1;
            DataTable DtLedger;

            if (DtAccountingEffect.Columns.Count == 0)
                AddColumnsAccountDt();

            DtAccountingEffect.Rows.Clear();
            if (mFormType == FORMTYPE.SALEINVOICE)
            {
                if (Val.Val(txtGrossAmount.Text) > 0 || Val.Val(txtGrossAmountFE.Text) > 0)
                {
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "BASIC");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMT"] = txtGrossAmount.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["CRDAMTFE"] = txtGrossAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales Basic Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }

                if (Val.Val(txtGSTAmount.Text) > 0 || Val.Val(txtGSTAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "FREIGHT");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMT"] = txtGSTAmount.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["CRDAMTFE"] = txtGSTAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales FREIGHT Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtInsuranceAmount.Text) > 0 || Val.Val(txtInsuranceAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "INSURANCE");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMT"] = txtInsuranceAmount.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["CRDAMTFE"] = txtInsuranceAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase INSURANCE Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtShippingAmount.Text) > 0 || Val.Val(txtShippingAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "SHIPPING");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMT"] = txtShippingAmount.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["CRDAMTFE"] = txtShippingAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales SHIPPING Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtDiscAmount.Text) > 0 || Val.Val(txtDiscAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "DISCOUNT");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["DEBAMT"] = txtDiscAmount.Text;
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMTFE"] = txtDiscAmountFE.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales DISCOUNT Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }

                if (Val.Val(txtCGSTAmount.Text) > 0 || Val.Val(txtCGSTAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "CGST");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMT"] = txtCGSTAmount.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["CRDAMTFE"] = txtCGSTAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales CGST Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }

                if (Val.Val(txtSGSTAmount.Text) > 0 || Val.Val(txtSGSTAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "SGST");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMT"] = txtSGSTAmount.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["CRDAMTFE"] = txtSGSTAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales SGST Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }

                if (Val.Val(txtIGSTAmount.Text) > 0 || Val.Val(txtIGSTAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "IGST");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMT"] = txtIGSTAmount.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["CRDAMTFE"] = txtIGSTAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales IGST Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtTCSAmount.Text) > 0 || Val.Val(txtTCSAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "TCS");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["CRDAMT"] = txtTCSAmount.Text;
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMTFE"] = txtTCSAmountFE.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales TCS Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtRoundOffAmountFE.Text) != 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "ROUNDOFF");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        if (CmbRoundOff.SelectedIndex == 0)
                        {
                            Dr["CRDAMT"] = "0";
                            Dr["DEBAMT"] = "0";
                            Dr["DEBAMTFE"] = "0";
                            Dr["CRDAMTFE"] = txtRoundOffAmountFE.Text;
                        }
                        else
                        {
                            Dr["CRDAMT"] = "0";
                            Dr["DEBAMT"] = "0";
                            Dr["CRDAMTFE"] = "0";
                            Dr["DEBAMTFE"] = txtRoundOffAmountFE.Text;
                        }
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales RoundOff Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }

                if (Val.Val(txtNetAmount.Text) > 0 || Val.Val(txtNetAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "BASIC");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["LEDGERNAME"] = Val.ToString(txtBillingParty.Text);
                        Dr["REFLEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["DEBAMT"] = txtNetAmount.Text;
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMTFE"] = txtNetAmountFE.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["XCONTRA"] = "Y";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales Basic Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
            }
            else if (mFormType == FORMTYPE.PURCHASERETURN)
            {
                if (Val.Val(txtGrossAmount.Text) > 0 || Val.Val(txtGrossAmountFE.Text) > 0)
                {
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "BASIC");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMT"] = txtGrossAmount.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["CRDAMTFE"] = txtGrossAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase Basic Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }

                if (Val.Val(txtGSTAmount.Text) > 0 || Val.Val(txtGSTAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "FREIGHT");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMT"] = txtGSTAmount.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["CRDAMTFE"] = txtGSTAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase FREIGHT Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtInsuranceAmount.Text) > 0 || Val.Val(txtInsuranceAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "INSURANCE");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMT"] = txtInsuranceAmount.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["CRDAMTFE"] = txtInsuranceAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase INSURANCE Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtShippingAmount.Text) > 0 || Val.Val(txtShippingAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "SHIPPING");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMT"] = txtShippingAmount.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["CRDAMTFE"] = txtShippingAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase SHIPPING Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtDiscAmount.Text) > 0 || Val.Val(txtDiscAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "DISCOUNT");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["DEBAMT"] = txtDiscAmount.Text;
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMTFE"] = txtDiscAmountFE.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase DISCOUNT Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }

                if (Val.Val(txtCGSTAmount.Text) > 0 || Val.Val(txtCGSTAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "CGST");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMT"] = txtCGSTAmount.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["CRDAMTFE"] = txtCGSTAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase CGST Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }

                if (Val.Val(txtSGSTAmount.Text) > 0 || Val.Val(txtSGSTAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "SGST");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMT"] = txtSGSTAmount.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["CRDAMTFE"] = txtSGSTAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase SGST Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }

                if (Val.Val(txtIGSTAmount.Text) > 0 || Val.Val(txtIGSTAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "IGST");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMT"] = txtIGSTAmount.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["CRDAMTFE"] = txtIGSTAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase IGST Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtTCSAmount.Text) > 0 || Val.Val(txtTCSAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "TCS");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["CRDAMT"] = txtTCSAmount.Text;
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMTFE"] = txtTCSAmountFE.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase TCS Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtRoundOffAmountFE.Text) != 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "ROUNDOFF");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        if (CmbRoundOff.SelectedIndex == 0)
                        {
                            Dr["CRDAMT"] = "0";
                            Dr["DEBAMT"] = "0";
                            Dr["DEBAMTFE"] = "0";
                            Dr["CRDAMTFE"] = txtRoundOffAmountFE.Text;
                        }
                        else
                        {
                            Dr["CRDAMT"] = "0";
                            Dr["DEBAMT"] = "0";
                            Dr["CRDAMTFE"] = "0";
                            Dr["DEBAMTFE"] = txtRoundOffAmountFE.Text;
                        }
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase RoundOff Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtNetAmount.Text) > 0 || Val.Val(txtNetAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "BASIC");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["LEDGERNAME"] = Val.ToString(txtBillingParty.Text);
                        Dr["REFLEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["DEBAMT"] = txtNetAmount.Text;
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMTFE"] = txtNetAmountFE.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["XCONTRA"] = "Y";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase Basic Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
            }
            else if (mFormType == FORMTYPE.PURCHASEISSUE)
            {
                if (Val.Val(txtGrossAmount.Text) > 0 || Val.Val(txtGrossAmountFE.Text) > 0)
                {
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "BASIC");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMT"] = txtGrossAmount.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["DEBAMTFE"] = txtGrossAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase Basic Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }

                if (Val.Val(txtGSTAmount.Text) > 0 || Val.Val(txtGSTAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "FREIGHT");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMT"] = txtGSTAmount.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["DEBAMTFE"] = txtGSTAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase FREIGHT Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtInsuranceAmount.Text) > 0 || Val.Val(txtInsuranceAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "INSURANCE");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMT"] = txtInsuranceAmount.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["DEBAMTFE"] = txtInsuranceAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase INSURANCE Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtShippingAmount.Text) > 0 || Val.Val(txtShippingAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "SHIPPING");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMT"] = txtShippingAmount.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["DEBAMTFE"] = txtShippingAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase SHIPPING Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtDiscAmount.Text) > 0 || Val.Val(txtDiscAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "DISCOUNT");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["CRDAMT"] = txtDiscAmount.Text;
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMTFE"] = txtDiscAmountFE.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase DISCOUNT Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }

                if (Val.Val(txtCGSTAmount.Text) > 0 || Val.Val(txtCGSTAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "CGST");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMT"] = txtCGSTAmount.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["DEBAMTFE"] = txtCGSTAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase CGST Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }

                if (Val.Val(txtSGSTAmount.Text) > 0 || Val.Val(txtSGSTAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "SGST");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMT"] = txtSGSTAmount.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["DEBAMTFE"] = txtSGSTAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase SGST Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }

                if (Val.Val(txtIGSTAmount.Text) > 0 || Val.Val(txtIGSTAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "IGST");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMT"] = txtIGSTAmount.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["DEBAMTFE"] = txtIGSTAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase IGST Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtTCSAmount.Text) > 0 || Val.Val(txtTCSAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "TCS");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMT"] = txtTCSAmount.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["DEBAMTFE"] = txtTCSAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase TCS Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtRoundOffAmountFE.Text) != 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "ROUNDOFF");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        if (CmbRoundOff.SelectedIndex == 0)
                        {
                            Dr["CRDAMT"] = "0";
                            Dr["DEBAMT"] = "0";
                            Dr["CRDAMTFE"] = "0";
                            Dr["DEBAMTFE"] = txtRoundOffAmountFE.Text;
                        }
                        else
                        {
                            Dr["CRDAMT"] = "0";
                            Dr["DEBAMT"] = "0";
                            Dr["DEBAMTFE"] = "0";
                            Dr["CRDAMTFE"] = txtRoundOffAmountFE.Text;
                        }
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase RoundOff Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtNetAmount.Text) > 0 || Val.Val(txtNetAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("PLEDGER_ID", "BASIC");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["LEDGERNAME"] = Val.ToString(txtBillingParty.Text);
                        Dr["REFLEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["CRDAMT"] = txtNetAmount.Text;
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMTFE"] = txtNetAmountFE.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["XCONTRA"] = "Y";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Purchase Basic Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
            }
            else if (mFormType == FORMTYPE.SALESDELIVERYRETURN)
            {
                if (Val.Val(txtGrossAmount.Text) > 0 || Val.Val(txtGrossAmountFE.Text) > 0)
                {
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "BASIC");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMT"] = txtGrossAmount.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["DEBAMTFE"] = txtGrossAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales Basic Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }

                if (Val.Val(txtGSTAmount.Text) > 0 || Val.Val(txtGSTAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "FREIGHT");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMT"] = txtGSTAmount.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["DEBAMTFE"] = txtGSTAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales FREIGHT Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtInsuranceAmount.Text) > 0 || Val.Val(txtInsuranceAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "INSURANCE");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMT"] = txtInsuranceAmount.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["DEBAMTFE"] = txtInsuranceAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales INSURANCE Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtShippingAmount.Text) > 0 || Val.Val(txtShippingAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "SHIPPING");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMT"] = txtShippingAmount.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["DEBAMTFE"] = txtShippingAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales SHIPPING Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtDiscAmount.Text) > 0 || Val.Val(txtDiscAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "DISCOUNT");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["CRDAMT"] = txtDiscAmount.Text;
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMTFE"] = txtDiscAmountFE.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales DISCOUNT Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }

                if (Val.Val(txtCGSTAmount.Text) > 0 || Val.Val(txtCGSTAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "CGST");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMT"] = txtCGSTAmount.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["DEBAMTFE"] = txtCGSTAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales CGST Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }

                if (Val.Val(txtSGSTAmount.Text) > 0 || Val.Val(txtSGSTAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "SGST");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMT"] = txtSGSTAmount.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["DEBAMTFE"] = txtSGSTAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales SGST Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }

                if (Val.Val(txtIGSTAmount.Text) > 0 || Val.Val(txtIGSTAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "IGST");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMT"] = txtIGSTAmount.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["DEBAMTFE"] = txtIGSTAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales IGST Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtTCSAmount.Text) > 0 || Val.Val(txtTCSAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "TCS");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMT"] = txtTCSAmount.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["DEBAMTFE"] = txtTCSAmountFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales TCS Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtRoundOffAmountFE.Text) != 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "ROUNDOFF");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        if (CmbRoundOff.SelectedIndex == 0)
                        {
                            Dr["CRDAMT"] = "0";
                            Dr["DEBAMT"] = "0";
                            Dr["CRDAMTFE"] = "0";
                            Dr["DEBAMTFE"] = txtRoundOffAmountFE.Text;
                        }
                        else
                        {
                            Dr["CRDAMT"] = "0";
                            Dr["DEBAMT"] = "0";
                            Dr["DEBAMTFE"] = "0";
                            Dr["CRDAMTFE"] = txtRoundOffAmountFE.Text;
                        }
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales RoundOff Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
                if (Val.Val(txtNetAmount.Text) > 0 || Val.Val(txtNetAmountFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "BASIC");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["LEDGERNAME"] = Val.ToString(txtBillingParty.Text);
                        Dr["REFLEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["CRDAMT"] = txtNetAmount.Text;
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMTFE"] = txtNetAmountFE.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["XCONTRA"] = "Y";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales Basic Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
            }
            MainGridAccount.DataSource = DtAccountingEffect;
            MainGridAccount.RefreshDataSource();
        }

        public void AddExportAccountingEffect()
        {
            // int IntCnt = 1;
            DataTable DtLedger;

            if (DtExportAccountingEffect.Columns.Count == 0)
                AddColumnsExportAccountDt();

            DtExportAccountingEffect.Rows.Clear();
            if (mFormType == FORMTYPE.SALEINVOICE)
            {
                if (Val.Val(txtExpInvAmt.Text) > 0 || Val.Val(txtExpInvAmtFE.Text) > 0)
                {
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "BASIC");
                    if (DtLedger.Rows.Count > 0)
                    {
                        IntAccCnt++;

                        DataRow Dr = DtExportAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                        Dr["REFLEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["DEBAMT"] = "0";
                        Dr["CRDAMT"] = txtExpInvAmt.Text;
                        Dr["DEBAMTFE"] = "0";
                        Dr["CRDAMTFE"] = txtExpInvAmtFE.Text;
                        Dr["XCONTRA"] = "N";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtExportAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales Basic Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }

                if (Val.Val(txtExpInvAmt.Text) > 0 || Val.Val(txtExpInvAmtFE.Text) > 0)
                {
                    IntAccCnt++;
                    DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "BASIC");
                    if (DtLedger.Rows.Count > 0)
                    {
                        DataRow Dr = DtExportAccountingEffect.NewRow();
                        Dr["LEDGER_ID"] = Val.ToString(txtBillingParty.Tag);
                        Dr["LEDGERNAME"] = Val.ToString(txtBillingParty.Text);
                        Dr["REFLEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                        Dr["DEBAMT"] = txtExpInvAmt.Text;
                        Dr["CRDAMT"] = "0";
                        Dr["DEBAMTFE"] = txtExpInvAmtFE.Text;
                        Dr["CRDAMTFE"] = "0";
                        Dr["XCONTRA"] = "Y";
                        Dr["SRNO"] = Val.ToString(IntAccCnt);
                        DtExportAccountingEffect.Rows.Add(Dr);
                    }
                    else
                    {
                        Global.MessageError("Sales Basic Ledger Not Mapped, Proper Accounting Please Mapped It");
                    }
                }
            }

            MainGridExport.DataSource = DtExportAccountingEffect;
            MainGridExport.RefreshDataSource();
        }

        public void AddColumnsAccountDt()
        {
            DtAccountingEffect.Columns.Add("VOUCHERNO");
            DtAccountingEffect.Columns.Add("VOUCHERNOSTR");
            DtAccountingEffect.Columns.Add("LEDGER_ID");
            DtAccountingEffect.Columns.Add("LEDGERNAME");
            DtAccountingEffect.Columns.Add("REFLEDGER_ID");
            DtAccountingEffect.Columns.Add("DEBAMT");
            DtAccountingEffect.Columns.Add("CRDAMT");
            DtAccountingEffect.Columns.Add("DEBAMTFE");
            DtAccountingEffect.Columns.Add("CRDAMTFE");
            DtAccountingEffect.Columns.Add("XCONTRA");
            DtAccountingEffect.Columns.Add("SRNO");
        }

        public void AddColumnsExportAccountDt()
        {
            DtExportAccountingEffect.Columns.Add("VOUCHERNO");
            DtExportAccountingEffect.Columns.Add("VOUCHERNOSTR");
            DtExportAccountingEffect.Columns.Add("LEDGER_ID");
            DtExportAccountingEffect.Columns.Add("LEDGERNAME");
            DtExportAccountingEffect.Columns.Add("REFLEDGER_ID");
            DtExportAccountingEffect.Columns.Add("DEBAMT");
            DtExportAccountingEffect.Columns.Add("CRDAMT");
            DtExportAccountingEffect.Columns.Add("DEBAMTFE");
            DtExportAccountingEffect.Columns.Add("CRDAMTFE");
            DtExportAccountingEffect.Columns.Add("XCONTRA");
            DtExportAccountingEffect.Columns.Add("SRNO");
        }
        public void AddColumnsBrokerAccountDt()
        {
            DtBrokerAccountingEffect.Columns.Add("VOUCHERNO");
            DtBrokerAccountingEffect.Columns.Add("VOUCHERNOSTR");
            DtBrokerAccountingEffect.Columns.Add("LEDGER_ID");
            DtBrokerAccountingEffect.Columns.Add("LEDGERNAME");
            DtBrokerAccountingEffect.Columns.Add("REFLEDGER_ID");
            DtBrokerAccountingEffect.Columns.Add("DEBAMT");
            DtBrokerAccountingEffect.Columns.Add("CRDAMT");
            DtBrokerAccountingEffect.Columns.Add("DEBAMTFE");
            DtBrokerAccountingEffect.Columns.Add("CRDAMTFE");
            DtBrokerAccountingEffect.Columns.Add("XCONTRA");
            DtBrokerAccountingEffect.Columns.Add("SRNO");
            DtBrokerAccountingEffect.Columns.Add("ACCTYPE");
        }

        public void SaveAccountDetail(SqlConnection pConConnection, bool pExpValueAdd)
        {
            TRN_LedgerTranJournalProperty AccoutProperty = new TRN_LedgerTranJournalProperty();

            AccoutProperty.TRN_ID = Guid.NewGuid();
            AccoutProperty.ACCLEDGTRNTRN_ID = Val.ToString(lblMemoNo.Tag);
            AccoutProperty.ACCLEDGTRNSRNO = 1;

            AccoutProperty.VOUCHERDATE = Val.SqlDate(DTPMemoDate.Text);
            AccoutProperty.FINYEAR = BOConfiguration.FINYEARNAME;
            AccoutProperty.VOUCHERNO = Val.ToInt32(lblMemoNo.Text);
            AccoutProperty.VOUCHERSTR = txtJangedNo.Text;
            AccoutProperty.CURRENCY_ID = Val.ToInt32(txtCurrency.Tag);
            AccoutProperty.EXCRATE = Val.Val(txtExcRate.Text);
            AccoutProperty.NOTE = txtRemark.Text;
            //AccoutProperty.MEMO_ID = Guid.Parse("00000000-0000-0000-0000-000000000000");
            //AccoutProperty.REFTRN_ID = Guid.Parse("00000000-0000-0000-0000-000000000000");
            //AccoutProperty.REFSRNO = 0;
            ////Kuldeep 24122020
            //AccoutProperty.BILL_NO = txtVoucherNoStr.Text;
            //AccoutProperty.BILL_DT = Val.SqlDate(DTPMemoDate.Text);
            AccoutProperty.REFBOOKTYPEFULL = "";
            if (mFormType == FORMTYPE.SALESDELIVERYRETURN || mFormType == FORMTYPE.PURCHASERETURN)
            {
                AccoutProperty.BILL_NO = txtJangedNo.Text;
                AccoutProperty.BILL_DT = Val.SqlDate(DTPMemoDate.Text);
            }
            else
            {
                AccoutProperty.BILL_NO = txtVoucherNoStr.Text;
                AccoutProperty.BILL_DT = Val.SqlDate(DTPMemoDate.Text);
            }
            //End Kuldeep 24122020
            AccoutProperty.EXCRATEDIFF = 0;
            AccoutProperty.TERMSDATE = Val.SqlDate(DTPTermsDate.Text);
            AccoutProperty.CHQ_NO = "";
            //AccoutProperty.CHQISSUEDT = "01/01/1900";
            //AccoutProperty.CHQCLEARDT = "01/01/1900";
            AccoutProperty.DATAFREEZ = 0;
            AccoutProperty.PAYTYPE = "";
            AccoutProperty.REFTYPE = "";
            AccoutProperty.PAYTERMS = Val.ToInt32(txtTermsDays.Text);

            string AccountSaveForXML = string.Empty;
            // DtAccountingEffect.TableName = "ACCOUNT";
            if (pExpValueAdd)
            {
                using (StringWriter sw = new StringWriter())
                {
                    DtExportAccountingEffect.TableName = "ACCOUNT";
                    DtExportAccountingEffect.WriteXml(sw);
                    AccountSaveForXML = sw.ToString();
                }
            }
            else
            {
                using (StringWriter sw = new StringWriter())
                {
                    // DtAccountingEffect.Merge(DtExportAccountingEffect);
                    DtAccountingEffect.TableName = "ACCOUNT";
                    DtAccountingEffect.WriteXml(sw);
                    AccountSaveForXML = sw.ToString();
                }
            }
            //using (StringWriter sw = new StringWriter())
            //{
            //    DtAccountingEffect.WriteXml(sw);
            //    AccountSaveForXML = sw.ToString();
            //}

            if (mFormType == FORMTYPE.PURCHASERETURN)
            {
                AccoutProperty.ENTRYTYPE = "PRETURN";
                AccoutProperty.BOOKTYPE = "PRT";
                AccoutProperty.BOOKTYPEFULL = "PURCHASE REUTRN";
                AccoutProperty.TRNTYPE = "PURCHASE REUTRN";
            }
            else if (mFormType == FORMTYPE.SALEINVOICE)
            {
                AccoutProperty.ENTRYTYPE = "INVOICE";
                AccoutProperty.BOOKTYPE = "INV";
                AccoutProperty.BOOKTYPEFULL = "SALE DELIVERY";
                AccoutProperty.TRNTYPE = "SALE DELIVERY";
            }
            else if (mFormType == FORMTYPE.SALESDELIVERYRETURN)
            {
                AccoutProperty.ENTRYTYPE = "SRETURN";
                AccoutProperty.BOOKTYPE = "SRT";
                AccoutProperty.BOOKTYPEFULL = "SALE RETURN";
                AccoutProperty.TRNTYPE = "SALE RETURN";
            }
            else if (mFormType == FORMTYPE.PURCHASEISSUE)
            {
                AccoutProperty.ENTRYTYPE = "PURCHASE";
                AccoutProperty.BOOKTYPE = "PPUR";
                AccoutProperty.BOOKTYPEFULL = "POLISH PURCHASE";
                AccoutProperty.TRNTYPE = "POLISH PURCHASE";
            }
            ObjFinance.SaveAccountingEffect(pConConnection, AccoutProperty, AccountSaveForXML);
            string ReturnMessageDesc = "";
            string ReturnMessageType = "";
        }

        public void AddBrokerAccountingEffect()
        {
            int IntCnt = 1;
            DataTable DtLedger;

            if (DtBrokerAccountingEffect.Columns.Count == 0)
                AddColumnsBrokerAccountDt();

            DtBrokerAccountingEffect.Rows.Clear();
            if (Math.Round((Val.Val(txtNetAmount.Text) * Val.Val(txtBaseBrokeragePer.Text)) / 100, 2) > 0)
            {
                DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "BROKERAGE");
                DataRow Dr = DtBrokerAccountingEffect.NewRow();
                Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                Dr["REFLEDGER_ID"] = Val.ToString(txtBroker.Tag);
                Dr["DEBAMT"] = Math.Round((Val.Val(txtNetAmount.Text) * Val.Val(txtBaseBrokeragePer.Text)) / 100, 2);
                Dr["CRDAMT"] = "0";
                Dr["DEBAMTFE"] = Math.Round(((Val.Val(txtNetAmount.Text) * Val.Val(txtBaseBrokeragePer.Text)) / 100) * Val.Val(txtExcRate.Text), 2);
                Dr["CRDAMTFE"] = "0";
                Dr["XCONTRA"] = "N";
                Dr["SRNO"] = Val.ToString(IntCnt);
                Dr["ACCTYPE"] = "BROKERAGE";
                DtBrokerAccountingEffect.Rows.Add(Dr);
            }

            if (Math.Round((Val.Val(txtNetAmount.Text) * Val.Val(txtBaseBrokeragePer.Text)) / 100, 2) > 0)
            {
                IntCnt++;
                DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "BROKERAGE");
                DataRow Dr = DtBrokerAccountingEffect.NewRow();
                Dr["LEDGER_ID"] = Val.ToString(txtBroker.Tag);
                Dr["LEDGERNAME"] = Val.ToString(txtBroker.Text);
                Dr["REFLEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                Dr["DEBAMT"] = "0";
                Dr["CRDAMT"] = Math.Round((Val.Val(txtNetAmount.Text) * Val.Val(txtBaseBrokeragePer.Text)) / 100, 2);
                Dr["DEBAMTFE"] = "0";
                Dr["CRDAMTFE"] = Math.Round(((Val.Val(txtNetAmount.Text) * Val.Val(txtBaseBrokeragePer.Text)) / 100) * Val.Val(txtExcRate.Text), 2);
                Dr["XCONTRA"] = "Y";
                Dr["SRNO"] = Val.ToString(IntCnt);
                Dr["ACCTYPE"] = "BROKERAGE";
                DtBrokerAccountingEffect.Rows.Add(Dr);
            }

            // Add Adat effect by khushbu 19-08-21
            if (Math.Round((Val.Val(txtNetAmount.Text) * Val.Val(txtAdatPer.Text)) / 100, 2) > 0)
            {
                IntCnt++;
                DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "ADAT");
                DataRow Dr = DtBrokerAccountingEffect.NewRow();
                Dr["LEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                Dr["LEDGERNAME"] = Val.ToString(DtLedger.Rows[0]["LEDGERNAME"]);
                Dr["REFLEDGER_ID"] = Val.ToString(txtAdat.Tag);
                Dr["DEBAMT"] = Math.Round((Val.Val(txtNetAmount.Text) * Val.Val(txtAdatPer.Text)) / 100, 2);
                Dr["CRDAMT"] = "0";
                Dr["DEBAMTFE"] = Math.Round(((Val.Val(txtNetAmount.Text) * Val.Val(txtAdatPer.Text)) / 100) * Val.Val(txtExcRate.Text), 2);
                Dr["CRDAMTFE"] = "0";
                Dr["XCONTRA"] = "N";
                Dr["SRNO"] = Val.ToString(IntCnt);
                Dr["ACCTYPE"] = "ADAT";
                DtBrokerAccountingEffect.Rows.Add(Dr);
            }

            if (Math.Round((Val.Val(txtNetAmount.Text) * Val.Val(txtAdatPer.Text)) / 100, 2) > 0)
            {
                IntCnt++;
                DtLedger = ObjFinance.FindLEdgerIdNameFrmType("SLEDGER_ID", "ADAT");
                DataRow Dr = DtBrokerAccountingEffect.NewRow();
                Dr["LEDGER_ID"] = Val.ToString(txtAdat.Tag);
                Dr["LEDGERNAME"] = Val.ToString(txtAdat.Text);
                Dr["REFLEDGER_ID"] = Val.ToString(DtLedger.Rows[0]["LEDGER_ID"]);
                Dr["DEBAMT"] = "0";
                Dr["CRDAMT"] = Math.Round((Val.Val(txtNetAmount.Text) * Val.Val(txtAdatPer.Text)) / 100, 2);
                Dr["DEBAMTFE"] = "0";
                Dr["CRDAMTFE"] = Math.Round(((Val.Val(txtNetAmount.Text) * Val.Val(txtAdatPer.Text)) / 100) * Val.Val(txtExcRate.Text), 2);
                Dr["XCONTRA"] = "Y";
                Dr["SRNO"] = Val.ToString(IntCnt);
                Dr["ACCTYPE"] = "ADAT";
                DtBrokerAccountingEffect.Rows.Add(Dr);
            }
            //----------

            MaingridBrokerage.DataSource = DtBrokerAccountingEffect;
            MaingridBrokerage.RefreshDataSource();
        }

        public void SaveBrokerAccountDetail(SqlConnection pConConnection)
        {
            TRN_LedgerTranJournalProperty AccoutProperty = new TRN_LedgerTranJournalProperty();

            if (DtBrokerAccountingEffect.Rows.Count == 0)
            {
                return;
            }
            DataTable dtAdat = DtBrokerAccountingEffect.Clone();
            if (DtBrokerAccountingEffect.Select("ACCTYPE = 'ADAT'").Count() != 0)
            {
                dtAdat = DtBrokerAccountingEffect.Select("ACCTYPE = 'ADAT'").CopyToDataTable();
            }
            DataTable dtBroker = DtBrokerAccountingEffect.Select("ACCTYPE = 'BROKERAGE'").CopyToDataTable();

            AccoutProperty.TRN_ID = Guid.NewGuid();
            AccoutProperty.ACCLEDGTRNTRN_ID = Val.ToString(lblMemoNo.Tag);
            AccoutProperty.ACCLEDGTRNSRNO = 1;

            AccoutProperty.VOUCHERDATE = Val.SqlDate(DTPMemoDate.Text);
            AccoutProperty.FINYEAR = BOConfiguration.FINYEARNAME;
            AccoutProperty.VOUCHERNO = 0;
            AccoutProperty.VOUCHERSTR = "";
            AccoutProperty.CURRENCY_ID = Val.ToInt32(txtCurrency.Tag);
            AccoutProperty.EXCRATE = Val.Val(txtExcRate.Text);
            AccoutProperty.NOTE = txtRemark.Text;
            //AccoutProperty.MEMO_ID = Guid.Parse("00000000-0000-0000-0000-000000000000");
            //AccoutProperty.REFTRN_ID = Guid.Parse("00000000-0000-0000-0000-000000000000");
            //AccoutProperty.REFSRNO = 0;
            AccoutProperty.BILL_NO = txtVoucherNoStr.Text;
            AccoutProperty.BILL_DT = Val.SqlDate(DTPMemoDate.Text);
            AccoutProperty.EXCRATEDIFF = 0;
            AccoutProperty.TERMSDATE = Val.SqlDate(DTPTermsDate.Text);
            AccoutProperty.CHQ_NO = "";
            //AccoutProperty.CHQISSUEDT = "01/01/1900";
            //AccoutProperty.CHQCLEARDT = "01/01/1900";
            AccoutProperty.DATAFREEZ = 0;
            AccoutProperty.PAYTYPE = "";
            AccoutProperty.REFTYPE = "";
            AccoutProperty.PAYTERMS = Val.ToInt32(txtTermsDays.Text);

            string AccountSaveForXML = string.Empty;
            dtBroker.TableName = "ACCOUNT";
            using (StringWriter sw = new StringWriter())
            {
                dtBroker.WriteXml(sw);
                AccountSaveForXML = sw.ToString();
            }

            AccoutProperty.ENTRYTYPE = "BROKERAGE";
            AccoutProperty.BOOKTYPE = "BRK";
            AccoutProperty.BOOKTYPEFULL = "BROKERAGE";
            AccoutProperty.TRNTYPE = "BROKERAGE";
            if (dtBroker.Rows.Count > 0)
                ObjFinance.SaveAccountingEffect(pConConnection, AccoutProperty, AccountSaveForXML);

            if (dtAdat.Rows.Count > 0)
            {
                SaveAdatAccountDetail(pConConnection, dtAdat);
            }
        }

        public void SaveAdatAccountDetail(SqlConnection pConConnection, DataTable P_DT)
        {
            TRN_LedgerTranJournalProperty AccoutProperty = new TRN_LedgerTranJournalProperty();

            AccoutProperty.TRN_ID = Guid.NewGuid();
            AccoutProperty.ACCLEDGTRNTRN_ID = Val.ToString(lblMemoNo.Tag);
            AccoutProperty.ACCLEDGTRNSRNO = 1;

            AccoutProperty.VOUCHERDATE = Val.SqlDate(DTPMemoDate.Text);
            AccoutProperty.FINYEAR = BOConfiguration.FINYEARNAME;
            AccoutProperty.VOUCHERNO = 0;
            AccoutProperty.VOUCHERSTR = "";
            AccoutProperty.CURRENCY_ID = Val.ToInt32(txtCurrency.Tag);
            AccoutProperty.EXCRATE = Val.Val(txtExcRate.Text);
            AccoutProperty.NOTE = txtRemark.Text;
            //AccoutProperty.MEMO_ID = Guid.Parse("00000000-0000-0000-0000-000000000000");
            //AccoutProperty.REFTRN_ID = Guid.Parse("00000000-0000-0000-0000-000000000000");
            //AccoutProperty.REFSRNO = 0;
            AccoutProperty.BILL_NO = txtVoucherNoStr.Text;
            AccoutProperty.BILL_DT = Val.SqlDate(DTPMemoDate.Text);
            AccoutProperty.EXCRATEDIFF = 0;
            AccoutProperty.TERMSDATE = Val.SqlDate(DTPTermsDate.Text);
            AccoutProperty.CHQ_NO = "";
            //AccoutProperty.CHQISSUEDT = "01/01/1900";
            //AccoutProperty.CHQCLEARDT = "01/01/1900";
            AccoutProperty.DATAFREEZ = 0;
            AccoutProperty.PAYTYPE = "";
            AccoutProperty.REFTYPE = "";
            AccoutProperty.PAYTERMS = Val.ToInt32(txtTermsDays.Text);

            string AccountSaveForXML = string.Empty;
            P_DT.TableName = "ACCOUNT";
            using (StringWriter sw = new StringWriter())
            {
                P_DT.WriteXml(sw);
                AccountSaveForXML = sw.ToString();
            }

            AccoutProperty.ENTRYTYPE = "ADAT";
            AccoutProperty.BOOKTYPE = "ADT";
            AccoutProperty.BOOKTYPEFULL = "ADAT";
            AccoutProperty.TRNTYPE = "ADAT";
            if (P_DT.Rows.Count > 0)
                ObjFinance.SaveAccountingEffect(pConConnection, AccoutProperty, AccountSaveForXML);
        }

        #endregion

        private void txtBaseBrokeragePer_Validated(object sender, EventArgs e)
        {
            CalculationNew();
        }

        private void BtnMaximise_Click(object sender, EventArgs e)
        {
            if (PnlHeader.Visible == true)
            {
                BtnMaximise.Text = "Minimize";
                PnlHeader.Visible = false;
            }
            else if (PnlHeader.Visible == false)
            {
                BtnMaximise.Text = "Maximize";
                PnlHeader.Visible = true;
            }
        }

        private void BtnExportInvoicePrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (Val.ToString(lblMemoNo.Tag) == "")
                {
                    Global.Message("No Data Found For Print.");
                    return;
                }

                if (txtCompanyBank.Text.Trim().Equals(string.Empty))
                {
                    Global.Message("Company Bank Is Required While Print..");
                    txtCompanyBank.Focus();
                    xtraTabMasterPanel.SelectedTabPageIndex = 1;
                    return;
                }
                this.Cursor = Cursors.WaitCursor;

                int IntChkBrokerage = Val.ToBooleanToInt(ChkBrokerage.Checked);

                DataTable DTab = new DataTable();
                //Kuldeep 22102020
                DataTable DTabPackingList = new DataTable();
                if (ChkSummuryPrint.Checked == true)
                {
                    DTab = ObjMemo.MemoInvoicePrint(Val.ToString(lblMemoNo.Tag), "USD", "SUMMURY", IntChkBrokerage);
                    //Kuldeep 22102020
                    DTabPackingList = ObjMemo.MemoInvoicePrint(Val.ToString(lblMemoNo.Tag), "USD", "DETAIL", IntChkBrokerage);
                }
                else
                {
                    DTab = ObjMemo.MemoInvoicePrint(Val.ToString(lblMemoNo.Tag), "USD", "DETAIL", IntChkBrokerage);
                }
                if (DTab.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("There Is No Data Found For Print");
                    return;
                }

                //DataSet DS = new DataSet();
                //DTab.TableName = "Table";
                //DS.Tables.Add(DTab);
                //DataTable DTabDuplicate = DTab.Copy();
                //DTabDuplicate.TableName = "Table1";
                //foreach (DataRow DRow in DTabDuplicate.Rows)
                //{
                //    DRow["PRINTTYPE"] = "DUBLICATE";
                //}
                //DTabDuplicate.AcceptChanges();
                //DS.Tables.Add(DTabDuplicate);

                Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                FrmReportViewer.MdiParent = Global.gMainRef;
                //FrmReportViewer.ShowFormInvoicePrint("MemoInvoicePrint", DTab);

                if (cmbBillType.Text.ToUpper() == "RUPEESBILL")
                {
                    FrmReportViewer.ShowFormInvoicePrint("LSInvoicePrint", DTab);
                }
                else if (cmbBillType.Text.ToUpper() == "DOLLARBILL")
                {
                    FrmReportViewer.ShowFormInvoicePrint("DollarInvoicePrint", DTab);
                }
                else if (cmbBillType.Text.ToUpper() == "CONSIGNMENT")
                {
                    FrmReportViewer.ShowFormInvoicePrint("ConsInvoicePrint", DTab);
                }
                else
                {
                    FrmReportViewer.ShowFormInvoicePrint("MemoInvoicePrint", DTab);  //Export Format
                }
                if (ChkSummuryPrint.Checked == true)
                {
                    Report.FrmReportViewer FrmReportViewerPackingList = new Report.FrmReportViewer();
                    FrmReportViewerPackingList.MdiParent = Global.gMainRef;
                    if (cmbBillType.Text.ToUpper() == "CONSIGNMENT")
                        FrmReportViewerPackingList.ShowFormInvoicePrint("ConsInvoicePackingListPrint", DTabPackingList);
                    else
                        FrmReportViewerPackingList.ShowFormInvoicePrint("InvoicePackingListPrint", DTabPackingList);
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception EX)
            {
                this.Cursor = Cursors.Default;
                Global.Message(EX.Message);
            }
        }

        private void BtnExportPackingList_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (Val.ToString(lblMemoNo.Tag) == "")
                {
                    Global.Message("No Data Found For Print.");
                    return;
                }

                this.Cursor = Cursors.WaitCursor;

                DataTable DTabPackingList = ObjMemo.ExportPackingList(Val.ToString(lblMemoNo.Tag));

                if (DTabPackingList.Rows.Count == 0)
                {
                    this.Cursor = Cursors.Default;
                    Global.Message("There Is No Data Found For Print");
                    return;
                }

                Report.FrmReportViewer FrmReportViewer = new Report.FrmReportViewer();
                FrmReportViewer.MdiParent = Global.gMainRef;
                FrmReportViewer.ShowFormInvoicePrint("PackingDetailReport", DTabPackingList);

                this.Cursor = Cursors.Default;
            }
            catch (Exception EX)
            {
                this.Cursor = Cursors.Default;
                Global.Message(EX.Message);
            }
        }


        #region E-Invoice Events And Method

        private byte[] Base64UrlDecode(string input)
        {
            string output = input;
            output = output.Replace('-', '+');
            output = output.Replace('_', '/');
            switch (output.Length % 4)
            {
                case 0:
                    {
                        break;
                    }

                case 2:
                    {
                        output += "==";
                        break;
                    }

                case 3:
                    {
                        output += "=";
                        break;
                    }

                default:
                    {
                        throw new Exception("Illegal base64url string!");
                        break;
                    }
            }

            var converted = Convert.FromBase64String(output);
            return converted;
        }

        public string Decode(string token)
        {
            var parts = token.Split('.');
            string header = parts[0];
            string payload = parts[1];
            string signature = parts[2];
            byte[] crypto = Base64UrlDecode(parts[2]);
            string headerJson = Encoding.UTF8.GetString(Base64UrlDecode(header));
            var headerData = JObject.Parse(headerJson);
            string payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(payload));
            var payloadData = JObject.Parse(payloadJson);
            return headerData.ToString();
        }

        public string DecodePayload(string token)
        {
            var parts = token.Split('.');
            string header = parts[0];
            string payload = parts[1];
            string signature = parts[2];
            byte[] crypto = Base64UrlDecode(parts[2]);
            string headerJson = Encoding.UTF8.GetString(Base64UrlDecode(header));
            var headerData = JObject.Parse(headerJson);
            string payloadJson = Encoding.UTF8.GetString(Base64UrlDecode(payload));
            var payloadData = JObject.Parse(payloadJson);
            return payloadData.ToString();
        }

        public byte[] GenerateSecureKey()
        {
            Aes KEYGEN = Aes.Create();
            byte[] secretKey = KEYGEN.Key;
            return secretKey;
        }

        public string EncryptAsymmetric(string data, string key)
        {
            var keyBytes = Convert.FromBase64String(key);
            AsymmetricKeyParameter asymmetricKeyParameter = PublicKeyFactory.CreateKey(keyBytes);
            RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)asymmetricKeyParameter;
            var rsaParameters = new RSAParameters();
            rsaParameters.Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned();
            rsaParameters.Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned();
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(rsaParameters);
            var plaintext = Encoding.UTF8.GetBytes(data);
            byte[] ciphertext = rsa.Encrypt(plaintext, false);
            string cipherresult = Convert.ToBase64String(ciphertext);
            return cipherresult;
        }

        public string Encrypt(byte[] data, string key)
        {
            var keyBytes = Convert.FromBase64String(key);
            AsymmetricKeyParameter asymmetricKeyParameter = PublicKeyFactory.CreateKey(keyBytes);
            RsaKeyParameters rsaKeyParameters = (RsaKeyParameters)asymmetricKeyParameter;
            var rsaParameters = new RSAParameters();
            rsaParameters.Modulus = rsaKeyParameters.Modulus.ToByteArrayUnsigned();
            rsaParameters.Exponent = rsaKeyParameters.Exponent.ToByteArrayUnsigned();
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(rsaParameters);
            var plaintext = data;
            byte[] ciphertext = rsa.Encrypt(plaintext, false);
            string cipherresult = Convert.ToBase64String(ciphertext);
            return cipherresult;
        }


        public DataTable ConvertJsonToDataTable(string Json)
        {
            DataTable dt = new DataTable();
            string[] jsonStringArray = Regex.Split(Json.Replace("[", "").Replace("]", ""), "},{");
            List<string> ColumnsName = new List<string>();
            foreach (string jSA in jsonStringArray)
            {
                string[] jsonStringData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                foreach (string ColumnsNameData in jsonStringData)
                {
                    try
                    {
                        int idx = ColumnsNameData.IndexOf(":");
                        if (idx != -1)
                        {
                            string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "");
                            if (!ColumnsName.Contains(ColumnsNameString))
                            {
                                ColumnsName.Add(ColumnsNameString);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("Error Parsing Column Name : {0}", ColumnsNameData));
                    }
                }
                break;
            }
            foreach (string AddColumnName in ColumnsName)
            {
                dt.Columns.Add(AddColumnName);
            }
            string StrColumnName = "";
            foreach (string jSA in jsonStringArray)
            {
                string[] RowData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dt.NewRow();
                foreach (string rowData in RowData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        if (idx != -1)
                        {
                            string RowColumns = rowData.Substring(0, idx - 1).Replace("\"", "");
                            string RowDataString = rowData.Substring(idx + 1).Replace("\"", "");
                            nr[RowColumns] = RowDataString;
                            StrColumnName = RowColumns;
                        }
                        else
                        {
                            nr[StrColumnName] = nr[StrColumnName] + rowData;
                        }
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                dt.Rows.Add(nr);
            }
            return dt;
        }


        //public DataTable ToJsonDataTable(List<Output> list)
        //{
        //    var dt = new DataTable();

        //    // insert enough amount of rows
        //    var numRows = list.Select(x => x.results.Length).Max();
        //    for (int i = 0; i < numRows; i++)
        //        dt.Rows.Add(dt.NewRow());

        //    // process the data
        //    foreach (var field in list)
        //    {
        //        dt.Columns.Add(field.name);
        //        for (int i = 0; i < numRows; i++)
        //            // replacing missing values with empty strings
        //            dt.Rows[i][field.name] = i < field.results.Length ? field.results[i] : string.Empty;
        //    }

        //    return dt;
        //}

        public string DecryptBySymmetricKey(string encryptedText, byte[] key)
        {
            try
            {
                var dataToDecrypt = Convert.FromBase64String(encryptedText);
                var keyBytes = key;
                var tdes = new AesManaged();
                tdes.KeySize = 256;
                tdes.BlockSize = 128;
                tdes.Key = keyBytes;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform decrypt__1 = tdes.CreateDecryptor();
                byte[] deCipher = decrypt__1.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
                tdes.Clear();
                return Convert.ToBase64String(deCipher);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetToken(ref string key)
        {
            try
            {
                //this.Cursor = Cursors.WaitCursor;
                string public_key = "";
                string result = "";
                string sek = "";
                string AuthToken = "Bearer ";

                using (var reader = File.OpenText(Application.StartupPath + @"\\einv_sandbox.pem"))
                {
                    public_key = reader.ReadToEnd().Replace("-----BEGIN PUBLIC KEY-----", "").Replace("-----END PUBLIC KEY-----", "").Replace(Constants.vbLf, "");
                }
                string Json = "";

                TRN_EInvoiceProperty Property = ObjEInvoice.GetEInvoiceCredential(BusLib.Configuration.BOConfiguration.COMPANY_ID);
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)(0xC0 | 0x300 | 0xC00);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Property.TOKENURL);
                request.Method = "POST";
                request.KeepAlive = true;
                request.ProtocolVersion = HttpVersion.Version11;
                request.ServicePoint.Expect100Continue = false;
                request.AllowAutoRedirect = false;
                request.Accept = "*/*";
                request.UnsafeAuthenticatedConnectionSharing = true;
                request.ContentType = "application/json";
                request.Headers.Add("gspappid", Property.CLIENTID);
                request.Headers.Add("gspappsecret", Property.CLIENTSECRET);
                using (Stream dataStream = request.GetRequestStream())
                {
                    using (WebResponse tResponse = request.GetResponse())
                    {
                        using (Stream stream = tResponse.GetResponseStream())
                        {
                            var streamreader = new StreamReader(stream);
                            result = streamreader.ReadToEnd();
                            DataTable dtresult = ConvertJsonToDataTable(result);
                            if (dtresult.Rows.Count > 0)
                            {
                                foreach (DataRow dr in dtresult.Rows) // dt.Rows
                                {
                                    AuthToken = AuthToken + Val.ToString(dr["access_token"]);
                                    sek = Val.ToString(dr["jti"]);
                                }
                                // this.Cursor = Cursors.Default;
                            }
                        }
                    }
                    return AuthToken;
                }
            }
            catch (Exception ex)
            {
                Global.MessageError(ex.Message);
                return "";
            }
        }

        public string EncryptBySymmetricKey(string text, string sek)
        {
            // Encrypting SEK
            try
            {
                var dataToEncrypt = Convert.FromBase64String(text);
                var keyBytes = Convert.FromBase64String(sek);
                var tdes = new AesManaged();
                tdes.KeySize = 256;
                tdes.BlockSize = 128;
                tdes.Key = keyBytes;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform encrypt__1 = tdes.CreateEncryptor();
                byte[] deCipher = encrypt__1.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
                tdes.Clear();
                string EK_result = Convert.ToBase64String(deCipher);
                return EK_result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string DecryptBySymmetricKeyEBill(string encryptedText, string key)
        {
            try
            {
                var dataToDecrypt = Convert.FromBase64String(encryptedText);
                var keyBytes = Convert.FromBase64String(key);
                var tdes = new AesManaged();
                tdes.KeySize = 256;
                tdes.BlockSize = 128;
                tdes.Key = keyBytes;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;
                ICryptoTransform decrypt__1 = tdes.CreateDecryptor();
                byte[] deCipher = decrypt__1.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
                tdes.Clear();
                string EK_result = Convert.ToBase64String(deCipher);
                return EK_result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion

        private void BtnEInvoiceUpload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (Global.Confirm("Do You Want To Upload E-Invoice On GSTR ?") == System.Windows.Forms.DialogResult.No)
                    return;
            }
            catch (Exception ex)
            { Global.Message(ex.Message); }

            PnlLoading.Visible = true;
            bgw.RunWorkerAsync(1);
        }

        private void BtnEInvoiceCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                if (Global.Confirm("Do You Want To Cancel E-Invoice On GSTR ?") == System.Windows.Forms.DialogResult.No)
                    return;
            }
            catch (Exception ex)
            { Global.Message(ex.Message); }

            PnlLoading.Visible = true;
            bgw.RunWorkerAsync(2);
        }


        private void BtnEInvoicePrint_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            PnlLoading.Visible = true;
            bgw.RunWorkerAsync(3);
        }

        private void BtnApplyAll_Click(object sender, EventArgs e)
        {
            foreach (DataRow Dr in DTabMemoDetail.Rows)
            {
                Dr["LABSERVICECODE"] = Val.ToString(txtLabServiceCode.Text);
                Dr["LABSERVICECODE_ID"] = Val.ToInt32(txtLabServiceCode.Tag);
            }
        }

        private void txtLabServiceCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "LABSERVICECODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LABSERVICECODE);

                    FrmSearch.mStrColumnsToHide = "LABSERVICECODE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtLabServiceCode.Text = Val.ToString(FrmSearch.DRow["LABSERVICECODE"]);
                        txtLabServiceCode.Tag = Val.ToString(FrmSearch.DRow["LABSERVICECODE_ID"]);
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

        private void RepTxtLabServiceCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "LABSERVICECODE";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_LABSERVICECODE);

                    FrmSearch.mStrColumnsToHide = "LABSERVICECODE_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDetail.SetFocusedRowCellValue("LABSERVICECODE_ID", (Val.ToString(FrmSearch.DRow["LABSERVICECODE_ID"])));
                        GrdDetail.SetFocusedRowCellValue("LABSERVICECODE", (Val.ToString(FrmSearch.DRow["LABSERVICECODE"])));
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

        public void GetSummuryDetailForGrid()
        {
            if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.SALESDELIVERYRETURN)
            {
                string StrDescription = "";
                double DouTotalCarat = 0.00;
                Int32 DouTotalPCS = 0;

                double DouTotalRapaport = 0.00;
                double DouTotalAvgDisc = 0.00;
                double DouTotalRate = 0.00;
                double DouTotalAmount = 0.00;

                double DouTotalMemoRapaport = 0.00;
                double DouTotalMemoAvgDisc = 0.00;
                double DouTotalMemoRate = 0.00;
                double DouTotalMemoAmount = 0.00;

                double DouTotalMemoAvgDiscFE = 0.00;
                double DouTotalMemoRateFE = 0.00;
                double DouTotalMemoAmountFE = 0.00;
                double DouTotalAmountFE = 0.00;
                double DouTotalAvgDiscFE = 0.00;
                double DouTotalRateFE = 0.00;

                for (int IntI = 0; IntI < GrdDetail.RowCount; IntI++)
                {
                    DataRow DRow = GrdDetail.GetDataRow(IntI);

                    DouTotalPCS = DouTotalPCS + Val.ToInt(DRow["PCS"]);
                    DouTotalCarat = DouTotalCarat + Val.Val(DRow["CARAT"]);

                    DouTotalRapaport = DouTotalRapaport + Val.Val(DRow["SALERAPAPORT"]) * Val.Val(DRow["CARAT"]);
                    DouTotalAmount = DouTotalAmount + Val.Val(DRow["SALEAMOUNT"]);

                    DouTotalMemoRapaport = DouTotalRapaport + Val.Val(DRow["MEMORAPAPORT"]) * Val.Val(DRow["CARAT"]);
                    DouTotalMemoAmount = DouTotalMemoAmount + Val.Val(DRow["MEMOAMOUNT"]);

                    //DouTotalAmountFE = DouTotalAmountFE + Val.Val(DRow["SALEAMOUNTFE"]);
                    DouTotalMemoAmountFE = DouTotalMemoAmountFE + Val.Val(DRow["FMEMOAMOUNT"]);
                }

                DouTotalRate = Math.Round((DouTotalAmount / DouTotalCarat), 2);
                DouTotalAvgDisc = Math.Round(Val.Val(((DouTotalAmount - DouTotalRapaport) / DouTotalRapaport) * 100), 2);
                DouTotalMemoRate = Math.Round((DouTotalMemoAmount / DouTotalCarat), 2);
                DouTotalMemoAvgDisc = Math.Round(Val.Val(((DouTotalMemoAmount - DouTotalRapaport) / DouTotalRapaport) * 100), 2);

                DouTotalRateFE = Math.Round((DouTotalAmountFE / DouTotalCarat), 2);
                DouTotalAvgDiscFE = Math.Round(Val.Val(((DouTotalAmountFE - DouTotalRapaport) / DouTotalRapaport) * 100), 2);
                DouTotalMemoRateFE = Math.Round((DouTotalMemoAmountFE / DouTotalCarat), 2);
                DouTotalMemoAvgDiscFE = Math.Round(Val.Val(((DouTotalMemoAmountFE - DouTotalRapaport) / DouTotalRapaport) * 100), 2);

                StrDescription = "Cut & Polished Diamond";

                DTabMemoSummury.Rows[0]["DESCRIPION"] = StrDescription;
                DTabMemoSummury.Rows[0]["TOTALPCS"] = DouTotalPCS;
                DTabMemoSummury.Rows[0]["TOTALCARAT"] = DouTotalCarat;

                DTabMemoSummury.Rows[0]["SALEAVGRATE"] = DouTotalRate;
                DTabMemoSummury.Rows[0]["SALETOTALAMOUNT"] = DouTotalAmount;
                DTabMemoSummury.Rows[0]["SALEAVGDISC"] = DouTotalAvgDisc;
                DTabMemoSummury.Rows[0]["MEMOAVGRATE"] = DouTotalMemoRate;
                DTabMemoSummury.Rows[0]["MEMOTOTALAMOUNT"] = DouTotalMemoAmount;
                DTabMemoSummury.Rows[0]["MEMOAVGDISC"] = DouTotalMemoAvgDisc;

                DTabMemoSummury.Rows[0]["SALEAVGRATEFE"] = DouTotalRateFE;
                DTabMemoSummury.Rows[0]["SALETOTALAMOUNTFE"] = DouTotalAmountFE;
                DTabMemoSummury.Rows[0]["SALEAVGDISCFE"] = DouTotalAvgDiscFE;
                DTabMemoSummury.Rows[0]["MEMOAVGRATEFE"] = DouTotalMemoRateFE;
                DTabMemoSummury.Rows[0]["MEMOTOTALAMOUNTFE"] = DouTotalMemoAmountFE;
                DTabMemoSummury.Rows[0]["MEMOAVGDISCFE"] = DouTotalMemoAvgDiscFE;
            }
        }

        private void txtNarration_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "NARRATIONNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.ACC_NARRATION);

                    FrmSearch.mStrColumnsToHide = "NARRATION_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtNarration.Text = Val.ToString(FrmSearch.DRow["NARRATIONNAME"]);
                        txtNarration.Tag = Val.ToString(FrmSearch.DRow["NARRATION_ID"]);
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

        private void ReptxtNarration_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "NARRATIONNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;
                    FrmSearch.mDTab = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.ACC_NARRATION);

                    FrmSearch.mStrColumnsToHide = "NARRATION_ID";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        GrdDetail.SetFocusedRowCellValue("NARRATION_ID", (Val.ToString(FrmSearch.DRow["NARRATION_ID"])));
                        GrdDetail.SetFocusedRowCellValue("NARRATIONNAME", (Val.ToString(FrmSearch.DRow["NARRATIONNAME"])));
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

        private void BtnNarrationApplyAll_Click(object sender, EventArgs e)
        {
            foreach (DataRow Dr in DTabMemoDetail.Rows)
            {
                Dr["NARRATIONNAME"] = Val.ToString(txtNarration.Text);
                Dr["NARRATION_ID"] = Val.ToInt32(txtNarration.Tag);
            }
        }

        private void GrdSummury_CustomSummaryCalculate(object sender, CustomSummaryEventArgs e)
        {
            if (e.SummaryProcess == CustomSummaryProcess.Start)
            {
                DouCarat = 0;
                DouSaleRapaport = 0;
                DouSaleRapaportAmt = 0;
                DouSaleDisc = 0;
                DouSalePricePerCarat = 0;
                DouSaleAmount = 0;

                DouMemoRapaport = 0;
                DouMemoRapaportAmt = 0;
                DouMemoDisc = 0;
                DouMemoPricePerCarat = 0;
                DouMemoAmount = 0;
            }
            else if (e.SummaryProcess == CustomSummaryProcess.Calculate)
            {
                DouSaleAmount = DouSaleAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "SALEAMOUNT"));
                DouSaleRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "SALERAPAPORT"));
                DouSalePricePerCarat = DouSaleAmount / DouCarat;
                DouSaleRapaportAmt = DouSaleRapaportAmt + (DouSaleRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));

                DouMemoAmount = DouMemoAmount + Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "MEMOAMOUNT"));
                DouMemoRapaport = Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "MEMORAPAPORT"));
                DouMemoPricePerCarat = DouMemoAmount / DouCarat;
                DouMemoRapaportAmt = DouMemoRapaportAmt + (DouMemoRapaport * Val.Val(GrdDetail.GetRowCellValue(e.RowHandle, "CARAT")));
            }
            else if (e.SummaryProcess == CustomSummaryProcess.Finalize)
            {
                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("SALEPRICEPERCARAT") == 0)
                {
                    if (Val.Val(DouCarat) > 0)
                        e.TotalValue = Math.Round(Val.Val(DouSaleAmount) / Val.Val(DouCarat), 2);
                    else
                        e.TotalValue = 0;
                }
                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("SALERAPAPORT") == 0)
                {
                    if (Val.Val(DouCarat) > 0)
                        e.TotalValue = Math.Round(Val.Val(DouSaleRapaportAmt) / Val.Val(DouCarat), 2);
                    else
                        e.TotalValue = 0;
                }
                if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("SALEDISCOUNT") == 0)
                {
                    DouSaleRapaport = Math.Round(DouSaleRapaportAmt / DouCarat);
                    DouSaleDisc = Math.Round(((DouSaleRapaport - DouSalePricePerCarat) / DouSaleRapaport * 100), 2);
                    e.TotalValue = DouSaleDisc;
                }
            }
            if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MEMOPRICEPERCARAT") == 0)
            {
                if (Val.Val(DouCarat) > 0)
                    e.TotalValue = Math.Round(Val.Val(DouMemoAmount) / Val.Val(DouCarat), 2);
                else
                    e.TotalValue = 0;
            }
            if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MEMORAPAPORT") == 0)
            {
                if (Val.Val(DouCarat) > 0)
                    e.TotalValue = Math.Round(Val.Val(DouMemoRapaportAmt) / Val.Val(DouCarat), 2);
                else
                    e.TotalValue = 0;
            }
            if (((DevExpress.XtraGrid.GridSummaryItem)e.Item).FieldName.CompareTo("MEMODISCOUNT") == 0)
            {
                DouMemoRapaport = Math.Round(DouMemoRapaportAmt / DouCarat);
                DouMemoDisc = Math.Round(((DouMemoRapaport - DouMemoPricePerCarat) / DouMemoRapaport * 100), 2);
                e.TotalValue = DouMemoDisc;
            }

        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog OpenFileDialog = new OpenFileDialog();
                OpenFileDialog.Filter = "Excel Files (*.xls,*.xlsx)|*.xls;*.xlsx;";
                if (OpenFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txtBackPriceFileName.Text = OpenFileDialog.FileName;

                    string extension = Path.GetExtension(txtBackPriceFileName.Text.ToString());
                    string destinationPath = Application.StartupPath + @"\StoneFiles\" + Path.GetFileName(txtBackPriceFileName.Text);
                    destinationPath = destinationPath.Replace(extension, ".xlsx");
                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }
                    File.Copy(txtBackPriceFileName.Text, destinationPath);
                    DtabBackPriceUpdate = Global.GetDataTableFromExcel(destinationPath, true);

                    if (File.Exists(destinationPath))
                    {
                        File.Delete(destinationPath);
                    }
                }
                OpenFileDialog.Dispose();
                OpenFileDialog = null;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void BtnUploadBackPrice_Click(object sender, EventArgs e)
        {
            try
            {

                for (int k = 0; k < DtabBackPriceUpdate.Rows.Count; k++)
                {
                    if ((Val.Val(DtabBackPriceUpdate.Rows[k]["Disc"]) != 0) == (Val.Val(DtabBackPriceUpdate.Rows[k]["PerCarat"]) != 0))
                    {
                        Global.Message("Back And PerCarat Both Are Exists In StoneNo : '" + Val.ToString(DTabMemoDetail.Rows[k]["StockNo"]) + "' Pls Check..");
                        break;
                    }
                }

                for (int i = 0; i < DtabBackPriceUpdate.Rows.Count; i++)
                {
                    for (int j = 0; j < DTabMemoDetail.Rows.Count; j++)
                    {
                        if (
                              (RbtStoneNo.Checked && Val.ToString(DTabMemoDetail.Rows[j]["STOCKNO"]) == Val.ToString(DtabBackPriceUpdate.Rows[i]["StockNo"]))
                                ||
                               (RbtCertiNo.Checked && Val.ToString(DTabMemoDetail.Rows[j]["LABREPORTNO"]) == Val.ToString(DtabBackPriceUpdate.Rows[i]["StockNo"]))
                            )
                        {
                            if (Val.Val(DtabBackPriceUpdate.Rows[i]["Disc"]) != 0)
                            {
                                DTabMemoDetail.Rows[j]["MEMODISCOUNT"] = Val.ToString(DtabBackPriceUpdate.Rows[i]["Disc"]);
                                GrdDetail.SetRowCellValue(j, "MEMODISCOUNT", Val.ToString(DtabBackPriceUpdate.Rows[i]["Disc"]));

                                DTabMemoDetail.Rows[j]["OLDMEMODISCOUNT"] = Val.ToString(DtabBackPriceUpdate.Rows[i]["Disc"]);
                                GrdDetail.SetRowCellValue(j, "OLDMEMODISCOUNT", Val.ToString(DtabBackPriceUpdate.Rows[i]["Disc"]));
                            }
                            else if (Val.Val(DtabBackPriceUpdate.Rows[i]["PerCarat"]) != 0)
                            {
                                DTabMemoDetail.Rows[j]["MEMOPRICEPERCARAT"] = Val.ToString(DtabBackPriceUpdate.Rows[i]["PerCarat"]);
                                GrdDetail.SetRowCellValue(j, "MEMOPRICEPERCARAT", Val.ToString(DtabBackPriceUpdate.Rows[i]["PerCarat"]));

                                DTabMemoDetail.Rows[j]["OLDMEMOPRICEPERCARAT"] = Val.ToString(DtabBackPriceUpdate.Rows[i]["PerCarat"]);
                                GrdDetail.SetRowCellValue(j, "OLDMEMOPRICEPERCARAT", Val.ToString(DtabBackPriceUpdate.Rows[i]["PerCarat"]));
                            }
                            break;
                        }
                    }
                }
                CalculationNew();
                GetSummuryDetailForGrid();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void lblSampleExcelFile_Click(object sender, EventArgs e)
        {
            try
            {
                string StrFilePathDestination = "";

                StrFilePathDestination = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\JangedWiseBackPriceUpdateFormat" + DateTime.Now.Year.ToString() + DateTime.Now.ToString("MM") + DateTime.Now.Day.ToString() + ".xlsx";
                if (File.Exists(StrFilePathDestination))
                {
                    File.Delete(StrFilePathDestination);
                }
                File.Copy(AppDomain.CurrentDomain.BaseDirectory + "\\Format\\JangedWiseBackPriceUpdateFormat.xlsx", StrFilePathDestination);

                System.Diagnostics.Process.Start(StrFilePathDestination, "CMD");
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtBackAddLess_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                //if (RdbParameter.Checked)
                //{
                //    Global.Message("Please Select 'All' Or 'Price' Checkbox For Update Stone Price.");
                //    return;
                //}

                //double DouSaleDiscount = 0;
                //double DouSaleRapaport = 0;
                //double DouSalePricePerCarat = 0;
                //double DouSaleAmount = 0;

                //foreach (DataRow DRow in DTabMemoDetail.Rows)
                //{
                //	if (Val.Val(txtBackAddLess.Text) != 0)
                //		DouSaleDiscount = Val.Val(DRow["MEMODISCOUNT"]) + Val.Val(txtBackAddLess.Text);
                //	else
                //		DouSaleDiscount = Val.Val(DRow["SALEDISCOUNT"]);

                //	DouSaleRapaport = Val.Val(DRow["MEMORAPAPORT"]);
                //	DouSalePricePerCarat = Math.Round(DouSaleRapaport + ((DouSaleRapaport * DouSaleDiscount) / 100), 2); //#P
                //	DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);

                //	DRow["MEMODISCOUNT"] = DouSaleDiscount;
                //	DRow["MEMOPRICEPERCARAT"] = DouSalePricePerCarat;
                //	DRow["MEMOAMOUNT"] = DouSaleAmount;
                //}
                //DTabMemoDetail.AcceptChanges();
                //CalculationNew();
                //GetSummuryDetailForGrid();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void txtTermsPer_Validating(object sender, CancelEventArgs e)
        {
            //try
            //{
            //if (RdbParameter.Checked)
            //{
            //    Global.Message("Please Select 'All' Or 'Price' Checkbox For Update Stone Price.");
            //    return;
            //}

            //	double DouSaleDiscount = 0;
            //	double DouSaleRapaport = 0;
            //	double DouSalePricePerCarat = 0;
            //	double DouSaleAmount = 0;
            //	double OldCarat = 0;

            //	foreach (DataRow DRow in DTabMemoDetail.Rows)
            //	{
            //		if (Val.Val(txtTermsAddLessPer.Text) != 0)
            //		{
            //			DouSalePricePerCarat = Val.Val(DRow["MEMOPRICEPERCARAT"]) + Math.Round(((Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtTermsAddLessPer.Text)) / 100), 2);
            //		}
            //		else if (Val.Val(txtBlindAddLessPer.Text) != 0)
            //		{
            //			DouSalePricePerCarat = Val.Val(DRow["MEMOPRICEPERCARAT"]) + Math.Round(((Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtBlindAddLessPer.Text)) / 100), 2);
            //		}
            //		else if (Val.Val(txtTermsAddLessPer.Text) != 0 && Val.Val(txtBlindAddLessPer.Text) != 0)
            //		{
            //			DouSalePricePerCarat = Val.Val(DRow["MEMOPRICEPERCARAT"]) + Math.Round(((Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtTermsAddLessPer.Text)) / 100), 2);
            //			DouSalePricePerCarat = DouSalePricePerCarat - Math.Round(((Val.Val(DouSalePricePerCarat) * Val.Val(txtBlindAddLessPer.Text)) / 100), 2);
            //		}
            //		else
            //		{
            //			DouSalePricePerCarat = Val.Val(DRow["SALEPRICEPERCARAT"]);
            //		}

            //		if (Val.Val(DRow["MEMORAPAPORT"]) != 0)
            //			DouSaleDiscount = Math.Round(((Val.Val(DouSalePricePerCarat) - Val.Val(DRow["MEMORAPAPORT"])) / Val.Val(DRow["MEMORAPAPORT"])) * 100, 2);
            //		else
            //			DouSaleDiscount = 0;

            //		DouSaleRapaport = Val.Val(DRow["MEMORAPAPORT"]);
            //		DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);

            //		DRow["MEMODISCOUNT"] = DouSaleDiscount;
            //		DRow["MEMOPRICEPERCARAT"] = DouSalePricePerCarat;
            //		DRow["MEMOAMOUNT"] = DouSaleAmount;
            //	}
            //	DTabMemoDetail.AcceptChanges();
            //	CalculationNew();
            //	GetSummuryDetailForGrid();
            //}
            //catch (Exception ex)
            //{
            //	Global.Message(ex.Message.ToString());
            //}
        }

        private void txtBlindPer_Validating(object sender, CancelEventArgs e)
        {
            //try
            //{
            //	//if (RdbParameter.Checked)
            //	//{
            //	//    Global.Message("Please Select 'All' Or 'Price' Checkbox For Update Stone Price.");
            //	//    return;
            //	//}

            //	double DouSaleDiscount = 0;
            //	double DouSaleRapaport = 0;
            //	double DouSalePricePerCarat = 0;
            //	double DouSaleAmount = 0;

            //	foreach (DataRow DRow in DTabMemoDetail.Rows)
            //	{
            //		if (Val.Val(txtBlindAddLessPer.Text) != 0)
            //			DouSalePricePerCarat = Val.Val(DRow["MEMOPRICEPERCARAT"]) + Math.Round(((Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtBlindAddLessPer.Text)) / 100), 2);
            //		else if (Val.Val(txtTermsAddLessPer.Text) != 0)
            //			DouSalePricePerCarat = Val.Val(DRow["MEMOPRICEPERCARAT"]) + Math.Round(((Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtTermsAddLessPer.Text)) / 100), 2);
            //		else if (Val.Val(txtTermsAddLessPer.Text) != 0 && Val.Val(txtBlindAddLessPer.Text) != 0)
            //		{
            //			DouSalePricePerCarat = Val.Val(DRow["MEMOPRICEPERCARAT"]) + Math.Round(((Val.Val(DRow["MEMOPRICEPERCARAT"]) * Val.Val(txtTermsAddLessPer.Text)) / 100), 2);
            //			DouSalePricePerCarat = DouSalePricePerCarat - Math.Round(((Val.Val(DouSalePricePerCarat) * Val.Val(txtBlindAddLessPer.Text)) / 100), 2);
            //		}
            //		else
            //			DouSalePricePerCarat = Val.Val(DRow["SALEPRICEPERCARAT"]);


            //		if (Val.Val(DRow["MEMORAPAPORT"]) != 0)
            //			DouSaleDiscount = Math.Round(((Val.Val(DouSalePricePerCarat) - Val.Val(DRow["MEMORAPAPORT"])) / Val.Val(DRow["MEMORAPAPORT"])) * 100, 2);

            //		else
            //			DouSaleDiscount = 0;

            //		DouSaleRapaport = Val.Val(DRow["MEMORAPAPORT"]);
            //		DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);

            //		DRow["MEMODISCOUNT"] = DouSaleDiscount;
            //		DRow["MEMOPRICEPERCARAT"] = DouSalePricePerCarat;
            //		DRow["MEMOAMOUNT"] = DouSaleAmount;
            //	}
            //	DTabMemoDetail.AcceptChanges();
            //	CalculationNew();
            //	GetSummuryDetailForGrid();
            //}
            //catch (Exception ex)
            //{
            //	Global.Message(ex.Message.ToString());
            //}
        }

        private void BtnModifyPrice_Click(object sender, EventArgs e)
        {
            try
            {

                double DouSaleDiscount = 0;
                double DouSaleRapaport = 0;
                double DouSalePricePerCarat = 0;
                double DouSaleAmount = 0;
                double OLDMEMODISCOUNT = 0;
                double OLDMEMOPRICEPERCARAT = 0;

                double DouCarat = 0;
                double DouRapaport = 0;
                double DouDiscount = 0;
                double DouPricePerCarat = 0;
                double DouAmount = 0;

                //File Upload 
                if (DtabBackPriceUpdate.Rows.Count > 0)
                {
                    for (int k = 0; k < DtabBackPriceUpdate.Rows.Count; k++)
                    {
                        if ((Val.Val(DtabBackPriceUpdate.Rows[k]["Disc"]) != 0) == (Val.Val(DtabBackPriceUpdate.Rows[k]["PerCarat"]) != 0))
                        {
                            Global.Message("Back And PerCarat Both Are Exists In StoneNo : '" + Val.ToString(DTabMemoDetail.Rows[k]["StockNo"]) + "' Pls Check..");
                            break;
                        }
                    }
                    for (int i = 0; i < DtabBackPriceUpdate.Rows.Count; i++)
                    {
                        for (int j = 0; j < DTabMemoDetail.Rows.Count; j++)
                        {
                            if (
                                  (RbtStoneNo.Checked && Val.ToString(DTabMemoDetail.Rows[j]["STOCKNO"]) == Val.ToString(DtabBackPriceUpdate.Rows[i]["StockNo"]))
                                    ||
                                   (RbtCertiNo.Checked && Val.ToString(DTabMemoDetail.Rows[j]["LABREPORTNO"]) == Val.ToString(DtabBackPriceUpdate.Rows[i]["StockNo"]))
                                )
                            {
                                if (Val.Val(DtabBackPriceUpdate.Rows[i]["Disc"]) != 0) //Add calculation code khushbu 13-07-21
                                {
                                    //DTabMemoDetail.Rows[j]["JANGEDDISCOUNT"] = Val.ToString(DtabBackPriceUpdate.Rows[i]["Disc"]);
                                    //DTabMemoDetail.Rows[j]["OLDMEMODISCOUNT"] = Val.ToString(DtabBackPriceUpdate.Rows[i]["Disc"]);

                                    DouCarat = Val.Val(DTabMemoDetail.Rows[j]["CARAT"]);
                                    DouRapaport = Val.Val(DTabMemoDetail.Rows[j]["SALERAPAPORT"]);
                                    DouDiscount = Val.Val(DtabBackPriceUpdate.Rows[i]["Disc"]) * -1;

                                    if (DouRapaport != 0)
                                        DouPricePerCarat = Math.Round(DouRapaport - ((DouRapaport * DouDiscount) / 100), 2);
                                    else
                                        DouPricePerCarat = 0;

                                    DouAmount = Math.Round(DouPricePerCarat * DouCarat, 2);

                                    DTabMemoDetail.Rows[j]["JANGEDRAPAPORT"] = DouRapaport;
                                    DTabMemoDetail.Rows[j]["JANGEDPRICEPERCARAT"] = DouPricePerCarat;
                                    DTabMemoDetail.Rows[j]["JANGEDAMOUNT"] = DouAmount;

                                    DTabMemoDetail.Rows[j]["MEMODISCOUNT"] = DouDiscount;
                                    DTabMemoDetail.Rows[j]["MEMOPRICEPERCARAT"] = DouPricePerCarat;
                                    DTabMemoDetail.Rows[j]["MEMOAMOUNT"] = DouAmount;

                                    DTabMemoDetail.Rows[j]["FMEMOPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(txtExcRate.Text), 2);
                                    //DTabMemoDetail.Rows[j]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(txtExcRate.Text), 2);
                                    if (BOConfiguration.gStrLoginSection == "B")
                                    {
                                        DTabMemoDetail.Rows[j]["FMEMOAMOUNT"] = Math.Round((DouAmount * Val.Val(txtExcRate.Text)) / 1000, 2);
                                    }
                                    else
                                    {
                                        DTabMemoDetail.Rows[j]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(txtExcRate.Text), 2);
                                    }

                                    DTabMemoDetail.Rows[j]["OLDMEMODISCOUNT"] = DouDiscount;
                                    DTabMemoDetail.Rows[j]["OLDMEMOPRICEPERCARAT"] = DouPricePerCarat;

                                    DTabMemoDetail.AcceptChanges();

                                }
                                else if (Val.Val(DtabBackPriceUpdate.Rows[i]["PerCarat"]) != 0)
                                {
                                    //DTabMemoDetail.Rows[j]["JANGEDPRICEPERCARAT"] = Val.ToString(DtabBackPriceUpdate.Rows[i]["PerCarat"]);
                                    //DTabMemoDetail.Rows[j]["OLDMEMOPRICEPERCARAT"] = Val.ToString(DtabBackPriceUpdate.Rows[i]["PerCarat"]);

                                    DouCarat = Val.Val(DTabMemoDetail.Rows[j]["CARAT"]);
                                    DouRapaport = Val.Val(DTabMemoDetail.Rows[j]["SALERAPAPORT"]);
                                    DouPricePerCarat = Val.Val(DtabBackPriceUpdate.Rows[i]["PerCarat"]);
                                    DouAmount = Math.Round(DouPricePerCarat * DouCarat, 2);

                                    if (DouRapaport != 0)
                                        //DouDiscount = Math.Round(((DouPricePerCarat - DouRapaport) / DouRapaport) * 100, 2);
                                        DouDiscount = Math.Round(((DouRapaport - DouPricePerCarat) / DouRapaport) * 100, 2);  //#P:23-04-2021
                                    else
                                        DouDiscount = 0;

                                    DTabMemoDetail.Rows[j]["JANGEDRAPAPORT"] = DouRapaport;
                                    DTabMemoDetail.Rows[j]["JANGEDDISCOUNT"] = DouDiscount;
                                    DTabMemoDetail.Rows[j]["JANGEDAMOUNT"] = DouAmount;
                                    DTabMemoDetail.Rows[j]["JANGEDPRICEPERCARAT"] = DouPricePerCarat;

                                    DTabMemoDetail.Rows[j]["OLDMEMODISCOUNT"] = DouDiscount;
                                    DTabMemoDetail.Rows[j]["OLDMEMOPRICEPERCARAT"] = DouPricePerCarat;

                                    DTabMemoDetail.Rows[j]["MEMODISCOUNT"] = DouDiscount;
                                    DTabMemoDetail.Rows[j]["MEMOAMOUNT"] = DouAmount;
                                    DTabMemoDetail.Rows[j]["MEMOPRICEPERCARAT"] = DouPricePerCarat;
                                    DTabMemoDetail.Rows[j]["FMEMOPRICEPERCARAT"] = Math.Round(DouPricePerCarat * Val.Val(txtExcRate.Text), 2);
                                    //DTabMemoDetail.Rows[j]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(txtExcRate.Text), 2);
                                    if (BOConfiguration.gStrLoginSection == "B")
                                    {
                                        DTabMemoDetail.Rows[j]["FMEMOAMOUNT"] = Math.Round((DouAmount * Val.Val(txtExcRate.Text)) / 1000, 2);
                                    }
                                    else
                                    {
                                        DTabMemoDetail.Rows[j]["FMEMOAMOUNT"] = Math.Round(DouAmount * Val.Val(txtExcRate.Text), 2);
                                    }

                                    DTabMemoDetail.AcceptChanges();
                                }
                                break;
                            }
                        }
                    }
                    CalculationNew();
                    GetSummuryDetailForGrid();
                }

                /*
                    foreach (DataRow DRow in DTabMemoDetail.Rows)
                {
                    if (DtabBackPriceUpdate.Rows.Count > 0)
                    {
                        OLDMEMODISCOUNT = Val.Val(DRow["OLDMEMODISCOUNT"]);
                        OLDMEMOPRICEPERCARAT = Val.Val(DRow["OLDMEMOPRICEPERCARAT"]);
                    }
                    else if (Val.Val(DRow["OLDMEMODISCOUNT"]) == 0) //OLDMEMODISCOUNT == 0 || OldSalePricePerCarat == 0)
                    {
                        DRow["OLDMEMODISCOUNT"] = DRow["MEMODISCOUNT"];
                        DRow["OLDMEMOPRICEPERCARAT"] = DRow["MEMOPRICEPERCARAT"];
                        OLDMEMODISCOUNT = Val.Val(DRow["MEMODISCOUNT"]);
                        OLDMEMOPRICEPERCARAT = Val.Val(DRow["MEMOPRICEPERCARAT"]);
                    }
                    else
                    {
                        OLDMEMODISCOUNT = Val.Val(DRow["OLDMEMODISCOUNT"]);
                        OLDMEMOPRICEPERCARAT = Val.Val(DRow["OLDMEMOPRICEPERCARAT"]);
                    }
                    
                    if (Val.Val(txtBackAddLess.Text) != 0)
                    {
                        DouSaleDiscount = Val.Val(DRow["OLDMEMODISCOUNT"]) + Val.Val(txtBackAddLess.Text);
                        DouSaleRapaport = Val.Val(DRow["MEMORAPAPORT"]);
                        DouSalePricePerCarat = Math.Round(DouSaleRapaport + ((DouSaleRapaport * DouSaleDiscount) / 100), 2); //#P
                        DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);


                        OLDMEMODISCOUNT = DouSaleDiscount;
                        OLDMEMOPRICEPERCARAT = DouSalePricePerCarat;

                    }
                    else
                    {
                        DouSaleDiscount = OLDMEMODISCOUNT;
                        DouSalePricePerCarat = OLDMEMOPRICEPERCARAT;
                    }

                    if (Val.Val(txtTermsAddLessPer.Text) != 0)
                    {
                        DouSalePricePerCarat = Val.Val(DouSalePricePerCarat) + Math.Round(((Val.Val(DouSalePricePerCarat) * Val.Val(txtTermsAddLessPer.Text)) / 100), 2);

                        if (Val.Val(DRow["MEMORAPAPORT"]) != 0)
                            DouSaleDiscount = Math.Round(((Val.Val(DouSalePricePerCarat) - Val.Val(DRow["MEMORAPAPORT"])) / Val.Val(DRow["MEMORAPAPORT"])) * 100, 2);
                        else
                            DouSaleDiscount = 0;

                        DouSaleRapaport = Val.Val(DRow["MEMORAPAPORT"]);
                        DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);

                        OLDMEMODISCOUNT = DouSaleDiscount;
                        OLDMEMOPRICEPERCARAT = DouSalePricePerCarat;
                    }
                    //else
                    //{
                    //	DouSaleDiscount = OLDMEMODISCOUNT;
                    //	DouSalePricePerCarat = OldSalePricePerCarat;
                    //}


                    if (Val.Val(txtBlindAddLessPer.Text) != 0)
                    {
                        DouSalePricePerCarat = Val.Val(DouSalePricePerCarat) + Math.Round(((Val.Val(DouSalePricePerCarat) * Val.Val(txtBlindAddLessPer.Text)) / 100), 2);
                        if (Val.Val(DRow["MEMORAPAPORT"]) != 0)
                            DouSaleDiscount = Math.Round(((Val.Val(DouSalePricePerCarat) - Val.Val(DRow["MEMORAPAPORT"])) / Val.Val(DRow["MEMORAPAPORT"])) * 100, 2);
                        else
                            DouSaleDiscount = 0;

                        //DouSaleRapaport = Val.Val(DRow["MEMORAPAPORT"]);
                        //DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);
                    }

                    DouSaleRapaport = Val.Val(DRow["MEMORAPAPORT"]);
                    DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);

                    DRow["MEMODISCOUNT"] = DouSaleDiscount;
                    DRow["MEMOPRICEPERCARAT"] = DouSalePricePerCarat;
                    DRow["MEMOAMOUNT"] = DouSaleAmount;
                }
                 * */

                //Back Update
                foreach (DataRow DRow in DTabMemoDetail.Rows)
                {
                    if (DtabBackPriceUpdate.Rows.Count > 0)
                    {
                        OLDMEMODISCOUNT = Val.Val(DRow["OLDMEMODISCOUNT"]);
                        OLDMEMOPRICEPERCARAT = Val.Val(DRow["OLDMEMOPRICEPERCARAT"]);
                    }
                    else if (Val.Val(DRow["OLDMEMODISCOUNT"]) == 0) //OLDMEMODISCOUNT == 0 || OldSalePricePerCarat == 0)
                    {
                        //DRow["OLDMEMODISCOUNT"] = DRow["MEMODISCOUNT"];
                        //DRow["OLDMEMOPRICEPERCARAT"] = DRow["MEMOPRICEPERCARAT"];
                        //OLDMEMODISCOUNT = Val.Val(DRow["MEMODISCOUNT"]);
                        //OLDMEMOPRICEPERCARAT = Val.Val(DRow["MEMOPRICEPERCARAT"]);

                        DRow["OLDMEMODISCOUNT"] = DRow["JANGEDDISCOUNT"];
                        DRow["OLDMEMOPRICEPERCARAT"] = DRow["JANGEDPRICEPERCARAT"];
                        OLDMEMODISCOUNT = Val.Val(DRow["JANGEDDISCOUNT"]);
                        OLDMEMOPRICEPERCARAT = Val.Val(DRow["JANGEDPRICEPERCARAT"]);
                    }
                    else if (Val.Val(txtBackAddLess.Text) == 0 && Val.Val(txtBackAddLess.Text) == 0 && Val.Val(txtBackAddLess.Text) == 0)
                    {
                        //DRow["OLDMEMODISCOUNT"] = DRow["JANGEDDISCOUNT"];
                        //DRow["OLDMEMOPRICEPERCARAT"] = DRow["JANGEDPRICEPERCARAT"];
                        OLDMEMODISCOUNT = Val.Val(DRow["OLDMEMODISCOUNT"]);
                        OLDMEMOPRICEPERCARAT = Val.Val(DRow["OLDMEMOPRICEPERCARAT"]);
                        DouSaleDiscount = OLDMEMODISCOUNT;
                        DouSalePricePerCarat = OLDMEMOPRICEPERCARAT;
                    }
                    else
                    {
                        OLDMEMODISCOUNT = Val.Val(DRow["OLDMEMODISCOUNT"]);
                        OLDMEMOPRICEPERCARAT = Val.Val(DRow["OLDMEMOPRICEPERCARAT"]);
                    }

                    if (Val.Val(txtBackAddLess.Text) != 0)
                    {
                        DouSaleDiscount = Val.Val(DRow["SALEDISCOUNT"]) - Val.Val(txtBackAddLess.Text);
                        DouSaleRapaport = Val.Val(DRow["MEMORAPAPORT"]);
                        //DouSalePricePerCarat = Math.Round(DouSaleRapaport + ((DouSaleRapaport * DouSaleDiscount) / 100), 2);
                        DouSalePricePerCarat = Math.Round(DouSaleRapaport - ((DouSaleRapaport * DouSaleDiscount) / 100), 2); //#P:23-04-2021
                        DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);

                        OLDMEMODISCOUNT = DouSaleDiscount;
                        OLDMEMOPRICEPERCARAT = DouSalePricePerCarat;

                    }
                    else
                    {
                        DouSaleDiscount = OLDMEMODISCOUNT;
                        DouSalePricePerCarat = OLDMEMOPRICEPERCARAT;
                    }

                    if (Val.Val(txtTermsAddLessPer.Text) != 0)
                    {
                        //DouSalePricePerCarat = Val.Val(DouSalePricePerCarat) + Math.Round(((Val.Val(DouSalePricePerCarat) * Val.Val(txtTermsAddLessPer.Text)) / 100), 2);
                        if (Val.Val(txtTermsAddLessPer.Text) > 0)
                            DouSalePricePerCarat = Math.Round(((Val.Val(DouSalePricePerCarat) / (100 - Val.Val(txtTermsAddLessPer.Text))) * 100), 2);
                        else
                            DouSalePricePerCarat = Val.Val(DouSalePricePerCarat) + Math.Round(((Val.Val(DouSalePricePerCarat) * Val.Val(txtTermsAddLessPer.Text)) / 100), 2);

                        if (Val.Val(DRow["MEMORAPAPORT"]) != 0)
                            //DouSaleDiscount = Math.Round(((Val.Val(DouSalePricePerCarat) - Val.Val(DRow["MEMORAPAPORT"])) / Val.Val(DRow["MEMORAPAPORT"])) * 100, 2);
                            DouSaleDiscount = Math.Round(((Val.Val(DRow["MEMORAPAPORT"]) - Val.Val(DouSalePricePerCarat)) / Val.Val(DRow["MEMORAPAPORT"])) * 100, 2);
                        else
                            DouSaleDiscount = 0;

                        DouSaleRapaport = Val.Val(DRow["MEMORAPAPORT"]);
                        DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);

                        OLDMEMODISCOUNT = DouSaleDiscount;
                        OLDMEMOPRICEPERCARAT = DouSalePricePerCarat;
                    }
                    //else
                    //{
                    //	DouSaleDiscount = OLDMEMODISCOUNT;
                    //	DouSalePricePerCarat = OldSalePricePerCarat;
                    //}


                    if (Val.Val(txtBlindAddLessPer.Text) != 0)
                    {
                        //DouSalePricePerCarat = Val.Val(DouSalePricePerCarat) + Math.Round(((Val.Val(DouSalePricePerCarat) * Val.Val(txtBlindAddLessPer.Text)) / 100), 2);
                        if (Val.Val(txtBlindAddLessPer.Text) > 0)
                            DouSalePricePerCarat = Math.Round(((Val.Val(DouSalePricePerCarat) / (100 - Val.Val(txtBlindAddLessPer.Text))) * 100), 2);
                        else
                            DouSalePricePerCarat = Val.Val(DouSalePricePerCarat) + Math.Round(((Val.Val(DouSalePricePerCarat) * Val.Val(txtBlindAddLessPer.Text)) / 100), 2);


                        if (Val.Val(DRow["MEMORAPAPORT"]) != 0)
                            //DouSaleDiscount = Math.Round(((Val.Val(DouSalePricePerCarat) - Val.Val(DRow["MEMORAPAPORT"])) / Val.Val(DRow["MEMORAPAPORT"])) * 100, 2);
                            DouSaleDiscount = Math.Round(((Val.Val(DRow["MEMORAPAPORT"]) - Val.Val(DouSalePricePerCarat)) / Val.Val(DRow["MEMORAPAPORT"])) * 100, 2); //#P:23-04-2021
                        else
                            DouSaleDiscount = 0;

                        //DouSaleRapaport = Val.Val(DRow["MEMORAPAPORT"]);
                        //DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);
                    }

                    DouSaleRapaport = Val.Val(DRow["MEMORAPAPORT"]);
                    DouSaleAmount = Math.Round(DouSalePricePerCarat * Val.Val(DRow["CARAT"]), 2);

                    DRow["MEMODISCOUNT"] = DouSaleDiscount;
                    DRow["MEMOPRICEPERCARAT"] = DouSalePricePerCarat;
                    DRow["MEMOAMOUNT"] = DouSaleAmount;

                    DRow["FMEMOPRICEPERCARAT"] = Math.Round(DouSalePricePerCarat * Val.Val(txtExcRate.Text), 2);
                    //DRow["FMEMOAMOUNT"] = Math.Round(DouSaleAmount * Val.Val(txtExcRate.Text), 2);
                    if (BOConfiguration.gStrLoginSection == "B")
                    {
                        DRow["FMEMOAMOUNT"] = Math.Round((DouSaleAmount * Val.Val(txtExcRate.Text)) / 1000, 2);
                    }
                    else
                    {
                        DRow["FMEMOAMOUNT"] = Math.Round(DouSaleAmount * Val.Val(txtExcRate.Text), 2);
                    }

                    if (Val.Val(txtBackAddLess.Text) != 0)
                    {
                        DRow["JANGEDRAPAPORT"] = Val.Val(DRow["SALERAPAPORT"]);
                        DRow["JANGEDDISCOUNT"] = Val.Val(DRow["SALEDISCOUNT"]);
                        DRow["JANGEDPRICEPERCARAT"] = Val.Val(DRow["SALEPRICEPERCARAT"]);
                        DRow["JANGEDAMOUNT"] = Val.Val(DRow["SALEAMOUNT"]);
                    }
                    else if ((Val.Val(txtBackAddLess.Text) == 0 && Val.Val(txtTermsAddLessPer.Text) == 0 && Val.Val(txtBlindAddLessPer.Text) == 0))
                    {
                        DRow["JANGEDRAPAPORT"] = DouSaleRapaport;
                        DRow["JANGEDDISCOUNT"] = DouSaleDiscount;
                        DRow["JANGEDPRICEPERCARAT"] = DouSalePricePerCarat;
                        DRow["JANGEDAMOUNT"] = DouSaleAmount;
                    }

                    //DRow["MEMODISCOUNT"] = DouSaleDiscount;
                    //DRow["MEMOPRICEPERCARAT"] = DouSalePricePerCarat;
                    //DRow["MEMOAMOUNT"] = DouSaleAmount;
                }
                DTabMemoDetail.AcceptChanges();
                CalculationNew();
                GetSummuryDetailForGrid();

                //CalculationNew();
                //GetSummuryDetailForGrid();
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void MainGrdDetail_Click(object sender, EventArgs e)
        {

        }

        private void BtnParaClear_Click(object sender, EventArgs e)
        {
            try
            {
                txtBackPriceFileName.Text = "";
                txtBackAddLess.Text = "";
                txtTermsAddLessPer.Text = "";
                txtBlindAddLessPer.Text = "";

                foreach (DataRow DR in DTabMemoDetail.Rows)
                {
                    DR["MEMODISCOUNT"] = Val.Val(DR["SALEDISCOUNT"]);
                    DR["MEMOPRICEPERCARAT"] = Val.Val(DR["SALEPRICEPERCARAT"]);
                    DR["MEMOAMOUNT"] = Val.Val(DR["SALEAMOUNT"]);
                    DR["MEMORAPAPORT"] = Val.Val(DR["SALERAPAPORT"]);

                    DR["JANGEDDISCOUNT"] = Val.Val(DR["SALEDISCOUNT"]);
                    DR["JANGEDPRICEPERCARAT"] = Val.Val(DR["SALEPRICEPERCARAT"]);
                    DR["JANGEDAMOUNT"] = Val.Val(DR["SALEAMOUNT"]);
                    DR["JANGEDRAPAPORT"] = Val.Val(DR["SALERAPAPORT"]);

                    DR["OLDMEMODISCOUNT"] = Val.Val(DR["SALEDISCOUNT"]);
                    DR["OLDMEMOPRICEPERCARAT"] = Val.Val(DR["SALEPRICEPERCARAT"]);
                }
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
            }
        }

        private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) //FOR SALE DELIVERY
        {
            try
            {
                if (txtBillingParty.Text.Length == 0)
                {
                    Global.Message("Billing Party Is Required");
                    txtBillingParty.Focus();
                    return;
                }
                if (txtShippingParty.Text.Length == 0)
                {
                    Global.Message("Shipping Party Is Required");
                    txtShippingParty.Focus();
                    return;
                }
                if (txtBCountry.Text.Length == 0)
                {
                    Global.Message("Billing Country Is Required");
                    txtBCountry.Focus();
                    return;
                }
                if (txtSCountry.Text.Length == 0)
                {
                    Global.Message("Shipping Country Is Required");
                    txtSCountry.Focus();
                    return;
                }
                if (txtSellerName.Text.Length == 0)
                {
                    Global.Message("Seller Name Is Required");
                    txtSellerName.Focus();
                    return;
                }
                if (txtTerms.Text.Length == 0)
                {
                    Global.Message("Terms Is Required");
                    txtTerms.Focus();
                    return;
                }

                DataTable DtReturnStoneList = GetTableOfSelectedRows(GrdDetail, true);
                if (DtReturnStoneList.Rows.Count <= 0)
                {
                    Global.Message("Please Select Records That You Want To Deliver..");
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                var SelectedPartyStoneNo = DtReturnStoneList.AsEnumerable().Select(s => s.Field<string>("PARTYSTOCKNO")).ToArray();
                string StrPartyStoneNoList = string.Join(",", SelectedPartyStoneNo);

                LiveStockProperty LStockProperty = new LiveStockProperty();
                LStockProperty.STOCKNO = Val.ToString(StrPartyStoneNoList);
                LStockProperty.STOCKTYPE = Val.ToString(mStrStockType);
                DataSet DsLiveStock = new BOTRN_StockUpload().GetStoneDetailForMemoForm(LStockProperty);

                DataTable DtabInvoiceDetail = new DataTable();
                DtabInvoiceDetail = DsLiveStock.Tables[0];

                if (DtabInvoiceDetail.Rows.Count > 0)
                {
                    this.Cursor = Cursors.Default;
                    FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                    FrmMemoEntry.MdiParent = Global.gMainRef;
                    FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                    if (mFormType == FORMTYPE.ORDERCONFIRM || mFormType == FORMTYPE.CONSIGNMENTISSUE)
                        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.SALEINVOICE, DtabInvoiceDetail, mStrStockType);
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
                this.Cursor = Cursors.Default;
            }
        }

        private void barButtonItem12_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e) //FOR ORDER CONFIRM
        {
            try
            {
                if (txtBillingParty.Text.Length == 0)
                {
                    Global.Message("Billing Party Is Required");
                    txtBillingParty.Focus();
                    return;
                }
                if (txtShippingParty.Text.Length == 0)
                {
                    Global.Message("Shipping Party Is Required");
                    txtShippingParty.Focus();
                    return;
                }
                if (txtBCountry.Text.Length == 0)
                {
                    Global.Message("Billing Country Is Required");
                    txtBCountry.Focus();
                    return;
                }
                if (txtSCountry.Text.Length == 0)
                {
                    Global.Message("Shipping Country Is Required");
                    txtSCountry.Focus();
                    return;
                }
                if (txtSellerName.Text.Length == 0)
                {
                    Global.Message("Seller Name Is Required");
                    txtSellerName.Focus();
                    return;
                }
                if (txtTerms.Text.Length == 0)
                {
                    Global.Message("Terms Is Required");
                    txtTerms.Focus();
                    return;
                }


                DataTable DtReturnStoneList = GetTableOfSelectedRows(GrdDetail, true);
                if (DtReturnStoneList.Rows.Count <= 0)
                {
                    Global.Message("Please Select Records That You Want To Order Confirm..");
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                var SelectedPartyStoneNo = DtReturnStoneList.AsEnumerable().Select(s => s.Field<string>("PARTYSTOCKNO")).ToArray();
                string StrPartyStoneNoList = string.Join(",", SelectedPartyStoneNo);

                LiveStockProperty LStockProperty = new LiveStockProperty();
                LStockProperty.STOCKNO = Val.ToString(StrPartyStoneNoList);
                LStockProperty.STOCKTYPE = Val.ToString(mStrStockType);
                DataSet DsLiveStock = new BOTRN_StockUpload().GetStoneDetailForMemoForm(LStockProperty);

                DataTable DtabInvoiceDetail = new DataTable();
                DtabInvoiceDetail = DsLiveStock.Tables[0];

                if (DtabInvoiceDetail.Rows.Count > 0)
                {
                    this.Cursor = Cursors.Default;
                    FrmMemoEntry FrmMemoEntry = new FrmMemoEntry();
                    FrmMemoEntry.MdiParent = Global.gMainRef;
                    FrmMemoEntry.FormClosing += new FormClosingEventHandler(FrmMemoEntry_FormClosing);
                    if (mFormType == FORMTYPE.CONSIGNMENTISSUE)
                        FrmMemoEntry.ShowForm(Stock.FrmMemoEntry.FORMTYPE.ORDERCONFIRM, DtabInvoiceDetail, mStrStockType);
                }
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Global.Message(ex.Message.ToString());
                this.Cursor = Cursors.Default;
            }
        }

        private void chkIsConsingee_CheckedChanged(object sender, EventArgs e)
        {
            if (chkIsConsingee.Checked == true)
            {
                txtBuyer.Enabled = true;
            }
            else
            {
                txtBuyer.Enabled = false;
                txtBuyer.Text = string.Empty;
                txtBuyer.Tag = string.Empty;
            }
        }

        private void txtBuyer_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (Global.OnKeyPressEveToPopup(e))
                {
                    FrmSearchPopupBox FrmSearch = new FrmSearchPopupBox();
                    FrmSearch.mStrSearchField = "PARTYCODE,PARTYNAME,COMPANYNAME";
                    FrmSearch.mStrSearchText = e.KeyChar.ToString();
                    this.Cursor = Cursors.WaitCursor;

                    DataTable DtabParty = new BusLib.BOComboFill().FillCmb(BusLib.BOComboFill.SELTABLE.MST_SALEPARTY);
                    FrmSearch.mDTab = DtabParty;
                    FrmSearch.mStrColumnsToHide = "PARTY_ID,BILLINGCOUNTRY_ID,SHIPPINGCOUNTRY_ID,PARTYTYPE";
                    this.Cursor = Cursors.Default;
                    FrmSearch.ShowDialog();
                    e.Handled = true;
                    if (FrmSearch.DRow != null)
                    {
                        txtBuyer.Tag = Val.ToString(FrmSearch.DRow["PARTY_ID"]);
                        txtBuyer.Text = Val.ToString(FrmSearch.DRow["PARTYNAME"]);
                        PstrFinalAddress1 = Val.ToString(FrmSearch.DRow["BILLINGADDRESS1"]);
                        PstrFinalAddress2 = Val.ToString(FrmSearch.DRow["BILLINGADDRESS2"]);
                        PstrFinalAddress3 = Val.ToString(FrmSearch.DRow["BILLINGADDRESS3"]);
                        PstrFinalCountry_ID = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRY_ID"]);
                        PstrFinalCountryName = Val.ToString(FrmSearch.DRow["BILLINGCOUNTRYNAME"]);
                        PstrFinalSatet = Val.ToString(FrmSearch.DRow["BILLINGSTATE"]);
                        PstrFinalCity = Val.ToString(FrmSearch.DRow["BILLINGCITY"]);
                        PstrFinalZipCode = Val.ToString(FrmSearch.DRow["BILLINGZIPCODE"]);
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

        private void ChkUpdExport_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkUpdExport.Checked)
            {
                if (cmbBillType.Text.ToUpper() == "EXPORT")
                {
                    if (mFormType == FORMTYPE.SALEINVOICE || mFormType == FORMTYPE.ORDERCONFIRM)
                    {
                        GrdDetail.Bands["BANDEXPORT"].Visible = true;
                        GrdDetail.Columns["EXPINVOICERATE"].OptionsColumn.AllowEdit = true;
                    }
                }
                else
                {
                    GrdDetail.Bands["BANDEXPORT"].Visible = false;
                }
            }
        }

        private void txtExpInvAmtFE_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAddLessExcRate_Validated(object sender, EventArgs e)
        {
            txtExcRate.Text = Val.ToString(Val.Val(txtOrgExcRate.Text) - Val.Val(txtAddLessExcRate.Text));
            txtExcRate_Validated(null, null);
        }

        private void txtAdatPer_Validated(object sender, EventArgs e)
        {
            CalculationNew();
        }

        private void txtBState_Validated(object sender, EventArgs e)
        {
            AutoGstCalculation();
        }

        private void RbDollar_CheckedChanged(object sender, EventArgs e)
        {
            if (RbDollar.Checked && GrdDetail.DataSource != null)
            {
                GrdSummury.Columns["SALEAVGRATEFE"].Visible = false;
                GrdSummury.Columns["SALETOTALAMOUNTFE"].Visible = false;
                GrdSummury.Columns["SALEAVGDISCFE"].Visible = false;
                GrdSummury.Columns["MEMOAVGRATEFE"].Visible = false;
                GrdSummury.Columns["MEMOTOTALAMOUNTFE"].Visible = false;
                GrdSummury.Columns["MEMOAVGDISCFE"].Visible = false;

                GrdSummury.Columns["SALEAVGRATE"].Visible = true;
                GrdSummury.Columns["SALETOTALAMOUNT"].Visible = true;
                GrdSummury.Columns["SALEAVGDISC"].Visible = true;
                GrdSummury.Columns["MEMOAVGRATE"].Visible = true;
                GrdSummury.Columns["MEMOTOTALAMOUNT"].Visible = true;
                GrdSummury.Columns["MEMOAVGDISC"].Visible = true;

                GrdDetail.Columns["MEMOPRICEPERCARAT"].Visible = true;
                GrdDetail.Columns["MEMOAMOUNT"].Visible = true;
                GrdDetail.Columns["FMEMOPRICEPERCARAT"].Visible = false;
                GrdDetail.Columns["FMEMOAMOUNT"].Visible = false;
            }
        }

        private void RbRupee_CheckedChanged(object sender, EventArgs e)
        {
            if (RbRupee.Checked && GrdDetail.DataSource != null)
            {
                GrdSummury.Columns["SALEAVGRATE"].Visible = true;
                GrdSummury.Columns["SALETOTALAMOUNT"].Visible = true;
                GrdSummury.Columns["SALEAVGDISC"].Visible = true;
                GrdSummury.Columns["MEMOAVGRATE"].Visible = false;
                GrdSummury.Columns["MEMOTOTALAMOUNT"].Visible = false;
                GrdSummury.Columns["MEMOAVGDISC"].Visible = false;

                GrdSummury.Columns["SALEAVGRATEFE"].Visible = false;
                GrdSummury.Columns["SALETOTALAMOUNTFE"].Visible = false;
                GrdSummury.Columns["SALEAVGDISCFE"].Visible = false;
                GrdSummury.Columns["MEMOAVGRATEFE"].Visible = true;
                GrdSummury.Columns["MEMOTOTALAMOUNTFE"].Visible = true;
                GrdSummury.Columns["MEMOAVGDISCFE"].Visible = false;

                GrdDetail.Columns["MEMOPRICEPERCARAT"].Visible = false;
                GrdDetail.Columns["MEMOAMOUNT"].Visible = false;
                GrdDetail.Columns["FMEMOPRICEPERCARAT"].Visible = true;
                GrdDetail.Columns["FMEMOAMOUNT"].Visible = true;

            }
        }

        private void MainGridSummury_Click(object sender, EventArgs e)
        {

        }

        //Added by Daksha on 3/04/2023
        private void FrmMemoEntryBranch_Shown(object sender, EventArgs e)
        {
            try
            {
                PanelGst.Visible = BOConfiguration.COMPANY_ID == Val.ToGuid("FE4C657D-5452-44D3-84F7-C8C71E20446E") ? false : true; //If HK company then Hide

                if (mFormType == FORMTYPE.SALEINVOICE && BOConfiguration.COMPANY_ID == Val.ToGuid("FE4C657D-5452-44D3-84F7-C8C71E20446E")) //If HK Company then rename header
                {
                    this.Text = "Cash Invoice";
                    lblTitle.Text = this.Text;
                }
            }
            catch (Exception EX)
            {
                Global.MessageError(EX.Message);
            }
        }
        //End as Daksha

        private void txtHkdRate_TextChanged(object sender, EventArgs e)
        {
            txtHkdAmt.Text = Val.Format(Math.Round(Val.Val(txtHkdRate.Text) * Val.Val(txtNetAmount.Text), 3), "########0.000");
        }
    }

}
